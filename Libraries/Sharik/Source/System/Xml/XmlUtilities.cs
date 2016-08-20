using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Xml.Linq;
using System.Xml.XPath;
using Sharik.Text;

namespace Sharik.Xml
{
    
    public static class XmlUtilities
    {

        public static void PatchXmlFile(string inputXmlFilePath, string patchFilePath, string outputXmlFilePath)
        {
            XDocument document = XDocument.Load(inputXmlFilePath);
            Slice patchScript;
            using (var reader = new StreamReader(patchFilePath))
            {
                string cleanedText = reader.ReadToEnd().Replace("\r\n", "\n").Replace("\r", "\n");
                patchScript = cleanedText.Slice();
            }
            XmlUtilities.PatchXml(document.Root, patchScript);
            document.Save(outputXmlFilePath);
        }

        public static void PatchXml(this XElement xmlRoot, Slice patchScript)
        {
            Slice remainder = patchScript.TrimStart();
            while (remainder.Length > 0)
            {
                if (!XmlUtilities.SkipComments(ref remainder))
                {
                    PatchCommand command = XmlUtilities.ParseCommand(remainder);
                    Slice xpath = XmlUtilities.ParseXPath(remainder, out remainder);
                    List<Attribute> attributes = XmlUtilities.ParseAttributes(remainder, out remainder);
                    XElement element;
                    switch (command)
                    {
                        case PatchCommand.Add:
                            element = XmlUtilities.ForceXmlElement(xmlRoot, xpath, true);
                            XmlUtilities.ModifyXmlAttributes(element, attributes);
                            break;
                        case PatchCommand.Modify:
                            element = XmlUtilities.ForceXmlElement(xmlRoot, xpath, false);
                            XmlUtilities.ModifyXmlAttributes(element, attributes);
                            break;
                        case PatchCommand.Delete:
                            XmlUtilities.DeleteXmlElement(xmlRoot, xpath);
                            break;
                    }
                }
            }
        }

        public static string EscapeXml(this string s)
        {
            return SecurityElement.Escape(s);
        }

        public static string UnescapeXml(this string s)
        {
            string result = s;
            if (!string.IsNullOrEmpty(result))
            {
                result = result.Replace("&apos;", "'");
                result = result.Replace("&quot;", "\"");
                result = result.Replace("&gt;", ">");
                result = result.Replace("&lt;", "<");
                result = result.Replace("&amp;", "&");
            }
            return result;
        }

        // Internals

        enum PatchCommand { Add, Modify, Delete }

        static PatchCommand ParseCommand(Slice patchScript)
        {
            PatchCommand result = default(PatchCommand);
            char command = patchScript[0];
            switch (command)
            {
                case '+':
                    result = PatchCommand.Add;
                    break;
                case '*':
                    result = PatchCommand.Modify;
                    break;
                case '-':
                    result = PatchCommand.Delete;
                    break;
                default:
                    ThrowInvalidCommandException(command);
                    break;
            }
            return result;
        }

        static Slice ParseXPath(Slice patchScript, out Slice remainder)
        {
            Slice result = null;
            int n = patchScript.IndexOf('\n');
            if (n >= 0)
            {
                result = patchScript.SubSlice(1, n).Trim();
                remainder = patchScript.SubSlice(n + 1);
            }
            else
            {
                result = patchScript.SubSlice(1).Trim();
                remainder = patchScript.TailSibling();
            }
            if (result.Length == 0)
                ThrowInvalidXPathExpressionException(result);
            return result;
        }

        class Attribute
        {
            public Attribute(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name;
            public string Value;
        }

        class ParsedToken
        {
            public ParsedToken(Slice patchScript)
            {
                Token = patchScript.SubSlice(0, 0);
                Space = patchScript.SubSlice(0, 0);
                Remainder = patchScript.SubSlice(0, 0);
            }

            public Slice Token;
            public Slice Space;
            public Slice Remainder;
        }

        static List<Attribute> ParseAttributes(Slice patchScript, out Slice remainder)
        {
            var result = new List<Attribute>();
            var obj = new ParsedToken(patchScript);
            remainder = patchScript.TrimStart();
            Slice rem = remainder;
            while (rem.Length > 0 && rem[0] != '+' && rem[0] != '*' && rem[0] != '-')
            {
                if (!XmlUtilities.SkipComments(ref rem))
                {
                    Format.ParseText("{Token}={Remainder}", rem, obj, false);
                    Slice name = obj.Token.Trim();
                    rem = obj.Remainder.TrimStart();
                    if (name.Length > 0 && rem.Length > 0)
                    {
                        if (rem[0] == '\"')
                        {
                            Format.ParseText("\"{Token}\"{Space}\n{Remainder}", rem, obj, false);
                            Slice space = obj.Space.Trim();
                            if (space.Length == 0)
                            {
                                string value = obj.Token.ToString().Replace("\n", Environment.NewLine).UnescapeXml();
                                result.Add(new Attribute(name.ToString(), value));
                            }
                            else
                                ThrowInvalidAttributeSyntaxException(remainder);
                        }
                        else if (rem[0] == '-')
                        {
                            Format.ParseText("-{Space}\n{Remainder}", rem, obj, false);
                            Slice space = obj.Space.Trim();
                            if (space.Length == 0)
                                result.Add(new Attribute(name.ToString(), null));
                            else
                                ThrowInvalidAttributeSyntaxException(remainder);
                        }
                        else
                            ThrowInvalidAttributeSyntaxException(remainder);
                        rem = obj.Remainder.TrimStart();
                    }
                    else
                        ThrowInvalidAttributeSyntaxException(remainder);
                }
                remainder = rem;
            }
            return result;
        }

        static bool SkipComments(ref Slice patchScript)
        {
            bool result = false;
            if (patchScript.Length > 0 && patchScript[0] == '!')
            {
                int i = patchScript.IndexOf('\n');
                if (i >= 0)
                    patchScript = patchScript.SubSlice(i + 1).TrimStart();
                else
                    patchScript = patchScript.SubSlice(patchScript.Length);
                result = true;
            }
            return result;
        }

        class ParsedXPathAttributes
        {
            public ParsedXPathAttributes(Slice patchScript)
            {
                Node = patchScript.SubSlice(0, 0);
                Attributes = patchScript.SubSlice(0, 0);
                Remainder = patchScript.SubSlice(0, 0);
            }

            public Slice Node;
            public Slice Attributes;
            public Slice Remainder;
        }

        static XElement ForceXmlElement(XElement xmlRoot, Slice xpath, bool mustBeNewElement)
        {
            XElement result = xmlRoot.XPathSelectElement(xpath.ToString());
            if (result == null || mustBeNewElement)
            {
                var xpathFragments = new Stack<Slice>();
                XElement startElement = ParseXPathElements(xmlRoot, xpath, mustBeNewElement, xpathFragments);
                result = CreateXmlElements(startElement, xpathFragments, xpath);
            }
            return result;
        }

        static XElement ParseXPathElements(XElement xmlRoot, Slice xpath, bool mustBeNewElement,
            Stack<Slice> xpathFragments)
        {
            XElement result = null;
            Slice xhead = xpath;
            while ((result == null && xhead.Length > 0) || mustBeNewElement)
            {
                int i = xhead.LastIndexOf('/');
                if (i >= 0)
                {
                    Slice name = xhead.SubSlice(i + 1);
                    xpathFragments.Push(name);
                    xhead = xhead.SubSlice(0, i);
                    result = xmlRoot.XPathSelectElement(xhead.ToString());
                }
                else
                {
                    Slice name = xhead;
                    xpathFragments.Push(name);
                    xhead = xhead.SubSlice(0, 0);
                    result = xmlRoot;
                }
                mustBeNewElement = false;
            }
            return result;
        }

        static XElement CreateXmlElements(XElement startElement, Stack<Slice> xpathFragments, Slice xpath)
        {
            XElement result = startElement;
            while (xpathFragments.Count > 0)
            {
                XElement element = null;
                Slice fragment = xpathFragments.Pop();
                var obj = new ParsedXPathAttributes(fragment);
                Format.ParseText("{Node}[{Attributes}]{Remainder}", fragment, obj, false);
                if (obj.Node.Length > 0) // Pattern matched at least partially
                {
                    obj.Node = obj.Node.Trim();
                    obj.Attributes = obj.Attributes.Trim();
                    obj.Remainder = obj.Remainder.Trim();
                    if (obj.Node.Length > 0 && obj.Attributes.Length > 0 && obj.Remainder.Length == 0)
                    {
                        element = new XElement(obj.Node.ToString());
                        List<Attribute> attributes = XmlUtilities.ParseXPathAttributes(obj.Attributes, xpath);
                        XmlUtilities.ModifyXmlAttributes(element, attributes);
                    }
                    else
                        ThrowInvalidXPathExpressionException(xpath);
                }
                else
                    element = new XElement(fragment.ToString());
                result.Add(element);
                result = element;
            }
            return result;
        }

        static List<Attribute> ParseXPathAttributes(Slice xpathAttributes, Slice xpath)
        {
            var result = new List<Attribute>();
            var obj = new ParsedToken(xpathAttributes);
            Slice rem = xpathAttributes.TrimStart();
            while (rem.Length > 0)
            {
                Format.ParseText("{Token}={Remainder}", rem, obj, false);
                Slice name = obj.Token.Trim();
                if (name.StartsWith('@'))
                    name = name.SubSlice(1).TrimStart();
                else if (name.StartsWith("attribute::".Slice()))
                    name = name.SubSlice("attribute::".Length).TrimStart();
                else
                    ThrowInvalidOrUnsupportedXPathExpressionException(xpath);
                rem = obj.Remainder.TrimStart();
                if (name.Length > 0 && rem.Length > 0)
                {
                    if (rem[0] == '\"')
                    {
                        Format.ParseText("\"{Token}\"{Remainder}", rem, obj, false);
                        Slice space = obj.Space.Trim();
                        if (space.Length == 0)
                        {
                            string value = obj.Token.ToString().UnescapeXml();
                            result.Add(new Attribute(name.ToString(), value));
                        }
                        else
                            ThrowInvalidOrUnsupportedXPathExpressionException(xpath);
                        rem = obj.Remainder.TrimStart();
                        if (rem.Length > 0)
                        {
                            if (rem.StartsWith("and".Slice()))
                                rem = rem.SubSlice("and".Length).TrimStart();
                            else
                                ThrowInvalidOrUnsupportedXPathExpressionException(xpath);
                        }
                    }
                    else
                        ThrowInvalidOrUnsupportedXPathExpressionException(xpath);
                }
                else
                    ThrowInvalidOrUnsupportedXPathExpressionException(xpath);
            }
            return result;
        }

        static void DeleteXmlElement(XElement xmlRoot, Slice xpath)
        {
            XElement element = xmlRoot.XPathSelectElement(xpath.ToString());
            if (element != null)
                element.Remove();
        }

        static void ModifyXmlAttributes(XElement element, List<Attribute> attributes)
        {
            foreach (var attr in attributes)
            {
                if (attr.Name == "@")
                    element.SetValue(attr.Value ?? "");
                else if (attr.Name == "@@")
                    element.Add(new XText(attr.Value));
                else
                    element.SetAttributeValue(attr.Name, attr.Value);
            }
        }

        static void ThrowInvalidCommandException(char command)
        {
            throw new Exception(string.Format(
                "Invalid patch command: \"{0}\".\n" +
                "Valid commands are \"+\", \"-\", \"*\".\n" +
                "Comment line starts with \"!\" and ends with line break.\n", command));
        }

        static void ThrowInvalidXPathExpressionException(Slice xpath)
        {
            throw new Exception(string.Format(
                "Invalid XPath expression: \"{0}\".\n", xpath.ToString()));
        }

        static void ThrowInvalidOrUnsupportedXPathExpressionException(Slice xpath)
        {
            throw new Exception(string.Format(
                "Invalid or unsupported XPath expression: \"{0}\".\n" + 
                "This XPath expression cannot be used for creation of a new XML node.\n", xpath.ToString()));
        }

        static void ThrowInvalidAttributeSyntaxException(Slice attributes)
        {
            int i = attributes.IndexOf('\n');
            if (i < 0)
                i = attributes.Length;
            throw new Exception(string.Format(
                "Invalid attribute syntax:\n{0}\n" +
                "Valid attribute syntax looks line the following:\n" +
                "<attribute> = \"<value>\" <line-break>\nor\n" +
                "@ = \"<value>\" <line-break>\nor\n" +
                "@@ = \"<value>\" <line-break>\n\n" +
                "Example:\n" +
                "country=\"Belarus\"\n", attributes.SubSlice(0, i).ToString()));
        }

        static void ThrowInvalidTextBlockException(Slice textBlock)
        {
            throw new Exception(string.Format(
                "Invalid text block syntax:\n{0}\n" +
                "Valid text block syntax looks line the following:\n" +
                "@=\"Any XML-formatted text\"\n\n" +
                "Example:\n" +
                "@=\"The quick brown &quot;fox&quot; jumps over the lazy &quot;dog&quot;\"\n", textBlock.ToString()));
        }

    }

    public static class XmlPatchSample
    {

        public static void Main()
        {
            var inputXmlLines = new string[]
            {
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                "<configuration/>"
            };
            var patchScriptLines = new string[]
            {
                "!!This is the test XML patching script.",
                "! It's used to patch an empty XML file consisting of <configuration/> node only.",
                "",
                "+ /configuration/configSections",
                "testAttr = \"test attribute\"",
                "",
                "+ /configuration/configSections/section",
                "name=\"appSettings\"",
                "type=\"System.Configuration.AppSettingsSection\"",
                "restartOnExternalChanges=\"false\"",
                "requirePermission=\"false\"",
                "",
                "+ /configuration/configSections/section",
                "name=\"connectionStrings\"",
                "type=\"System.Configuration.ConnectionStringsSection\"",
                "requirePermission=\"false\"",
                "",
                "+ /configuration/configSections/section",
                "name=\"text\"",
                "@=\"The quick brown fox jumps over the lazy dog\"",
                "",
                "+ /configuration/configSections/section",
                "name=\"mscorlib\"",
                "type=\"System.Configuration.IgnoreSection\"",
                "allowLocation=\"false\"",
                "",
                "+ /configuration/configSections/section",
                "name=\"runtime\"",
                "type=\"System.Configuration.IgnoreSection\"",
                "allowLocation=\"false\"",
                "",
                "* /configuration/configSections/section[2]",
                "comment=\"some comment\"",
                "",
                "* /configuration/configSections/section[@name=\"mscorlib\"]",
                "comment=-",
                "",
                "* /configuration/configSections/section[1]",
                "comment=-",
                "",
                "- /configuration/configSections/section[@name=\"connectionStrings\" and @requirePermission=\"true\"]",
                "",
                "- /configuration/configSections/section[@name=\"appSettings\" and @requirePermission=\"false\"]",
                "",
                "* /configuration/configSections[@testAttr=\"test attribute\"]/section[@name=\"appSettings\" and @requirePermission=\"false\"]",
                "type=\"System.Configuration.AppSettingsSection\"",
                "restartOnExternalChanges=-",
                "requirePermission=\"false\"",
                "* /configuration/configSections",
                "@@=\"",
                "The quick brown fox\"",
                "",
                "+ /configuration/configSections/section",
                "name=\"mySettings\"",
                "type=\"MySettingsSection\"",
                "",
                "* /configuration/configSections",
                "@@=\"jumps over the lazy dog\""
            };
            File.Delete("output.xml");
            File.WriteAllLines("input.xml", inputXmlLines);
            File.WriteAllLines("patch.xp", patchScriptLines);
            XmlUtilities.PatchXmlFile("input.xml", "patch.xp", "output.xml");
        }

    }

}
