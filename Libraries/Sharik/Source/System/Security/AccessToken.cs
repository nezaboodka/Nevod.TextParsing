using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Sharik;
using Sharik.Text;

namespace Sharik.Security
{
    public class AccessToken
    {
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public TimeSpan Duration
        {
            get { return ValidTo - ValidFrom; }
            set { ValidTo = ValidFrom + value; }
        }
        public Guid TokenGuid { get; set; } // randomly generated to improve security
        public string Customer { get; set; }
        public string Account { get; set; }
        public string User { get; set; }
        public List<ResourcePermission> ResourcePermissions { get; set; }

        public override string ToString()
        {
            var token = new TokenWithEncodedPermissions()
            {
                ValidFrom = this.ValidFrom,
                ValidTo = this.ValidTo,
                TokenGuid = this.TokenGuid,
                Customer = this.Customer,
                Account = this.Account,
                User = this.User,
                Permissions = ResourcePermissionsToString()
            };
            var result = Format.EmitText(token, StringFormat);
            return result;
        }

        public string ToEncryptedString(byte[] key, byte[] iv)
        {
            var alg = new RijndaelManaged();
            alg.Padding = PaddingMode.PKCS7;
            alg.KeySize = 256;
            alg.Key = key;
            alg.IV = iv;
            var result = ConvertToStringAndEncrypt(alg);
            return result;
        }

        // Procedures

        public static AccessToken Create(TimeSpan duration, string customerId, string accountId,
            string userId, List<ResourcePermission> resourcePermissions)
        {
            var utcNow = DateTime.UtcNow;
            // Get rid of fractional milliseconds
            utcNow = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour,
                utcNow.Minute, utcNow.Second);
            var result = new AccessToken(utcNow, utcNow + duration, Guid.NewGuid(), customerId,
                accountId, userId, resourcePermissions);
            return result;
        }

        public static AccessToken CreateFromString(string tokenString)
        {
            return Parse(tokenString);
        }

        public static AccessToken CreateFromEncryptedString(string encryptedString, byte[] key, byte[] iv)
        {
            AccessToken result = null;
            try
            {
                var alg = new RijndaelManaged();
                alg.Padding = PaddingMode.PKCS7;
                alg.KeySize = 256;
                alg.Key = key;
                alg.IV = iv;
                result = DecryptAndParse(encryptedString, alg);
            }
            catch (Exception e)
            {
                throw new UnauthorizedAccessException("invalid access token", e);
            }
            return result;
        }

        public static AccessToken TryCreateFromEncryptedString(string encryptedString, byte[] key, byte[] iv)
        {
            AccessToken result = null;
            try
            {
                result = CreateFromEncryptedString(encryptedString, key, iv);
            }
            catch (Exception)
            {
                // Hide error
            }
            return result;
        }

        // Internals

        private AccessToken(DateTime validFrom, DateTime validTo, Guid tokenId, string customerId,
            string accountId, string userId, List<ResourcePermission> resourcePermissions)
        {
            ValidFrom = validFrom;
            ValidTo = validTo;
            TokenGuid = tokenId;
            Customer = customerId ?? string.Empty;
            Account = accountId ?? string.Empty;
            User = userId ?? string.Empty;
            ResourcePermissions = resourcePermissions ?? new List<ResourcePermission>();
        }

        private string ResourcePermissionsToString()
        {
            var result = new StringBuilder();
            if (ResourcePermissions.Count > 0)
            {
                result.Append(ResourcePermissions[0].ToString());
                if (ResourcePermissions.Count > 1)
                    for (var i = 1; i < ResourcePermissions.Count; i++)
                        result.AppendFormat("\t{0}", ResourcePermissions[i].ToString());
            }
            return result.ToString();
        }

        private static List<ResourcePermission> ParseResourcePermissions(string text)
        {
            var result = new List<ResourcePermission>();
            var strs = text.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in strs)
                result.Add(ResourcePermission.Parse(s));
            return result;
        }

        private string ConvertToStringAndEncrypt(SymmetricAlgorithm algorithm)
        {
            var cipherStr = Encrypt(ToString(), algorithm);
            var encryptedToken = new EncryptedToken()
            {
                ValidFrom = this.ValidFrom,
                ValidTo = this.ValidTo,
                EncryptedBody = cipherStr
            };
            var result = Format.EmitText(encryptedToken, EncryptedStringFormat);
            return result;
        }

        private static string Encrypt(string tokenString, SymmetricAlgorithm algorithm)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(tokenString);
            using (var encryptor = algorithm.CreateEncryptor())
            {
                var cipher = encryptor.TransformFinalBlock(tokenBytes, 0, tokenBytes.Length);
                var result = ConvertString.ToModifiedBase64String(cipher);
                return result;
            }
        }

        private static AccessToken DecryptAndParse(string tokenString, SymmetricAlgorithm algorithm)
        {
            var encryptedToken = new EncryptedToken();
            Format.ParseText(EncryptedStringFormat, tokenString, encryptedToken);
            var decryptedToken = Decrypt(encryptedToken.EncryptedBody, algorithm);
            var result = Parse(decryptedToken);
            return result;
        }

        private static string Decrypt(string encryptedToken, SymmetricAlgorithm algorithm)
        {
            string result = null;
            var cipher = ConvertString.FromModifiedBase64String(encryptedToken);
            using (var decryptor = algorithm.CreateDecryptor())
            {
                var tokenBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
                result = Encoding.UTF8.GetString(tokenBytes);
            }
            return result;
        }

        private static AccessToken Parse(string tokenString)
        {
            var token = new TokenWithEncodedPermissions();
            Format.ParseText(StringFormat, tokenString, token);
            var result = new AccessToken(token.ValidFrom, token.ValidTo, token.TokenGuid,
                token.Customer, token.Account, token.User,
                ParseResourcePermissions(token.Permissions));
            return result;
        }

        private class EncryptedToken
        {
            public EncryptedToken()
            {
                var emptyUtc = DateTime.MinValue.ToUniversalTime();
                ValidFrom = emptyUtc;
                ValidTo = emptyUtc;
                EncryptedBody = string.Empty;
            }
            public DateTime ValidFrom;
            public DateTime ValidTo;
            public string EncryptedBody;
        };

        private class TokenWithEncodedPermissions
        {
            public TokenWithEncodedPermissions()
            {
                var emptyUtc = DateTime.MinValue.ToUniversalTime();
                ValidFrom = emptyUtc;
                ValidTo = emptyUtc;
                TokenGuid = Guid.Empty;
                Customer = string.Empty;
                Account = string.Empty;
                User = string.Empty;
                Permissions = string.Empty;
            }
            public DateTime ValidFrom { get; set; }
            public DateTime ValidTo { get; set; }
            public Guid TokenGuid { get; set; }
            public string Customer { get; set; }
            public string Account { get; set; }
            public string User { get; set; }
            public string Permissions { get; set; }
        };

        // Constants

        private const string StringFormat = "From={ValidFrom:DateTime;u};To={ValidTo:DateTime;u};" +
            "TokenGuid={TokenGuid:Guid;N};Customer={Customer};Account={Account};User={User};" +
            "Permissions={Permissions}";

        private const string EncryptedStringFormat =
            "From{ValidFrom:DateTime;u}/To{ValidTo:DateTime;u}/{EncryptedBody}";
    }

    public class ResourcePermission
    {
        public string Service { get; set; }
        public string Resource { get; set; }
        public string Permission { get; set; }

        public override string ToString()
        {
            var result = Format.EmitText(this, StringFormat);
            return result;
        }

        public static ResourcePermission Parse(string text)
        {
            var result = new ResourcePermission();
            Format.ParseText(StringFormat, text, result);
            return result;
        }

        private const string StringFormat = "{Service}.{Resource}.{Permission}";
    }
}
