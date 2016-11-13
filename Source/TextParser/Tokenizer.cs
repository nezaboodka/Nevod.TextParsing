using System;
using System.Collections.Generic;
using System.Linq;
using Sharik.Text;

namespace TextParser
{
    public class Tokenizer
    {
        public virtual TokenizerResult GetTokens(string source)
        {   
            throw new NotImplementedException();         
            //if (!string.IsNullOrEmpty(source))
            //{
            //    int startPosition = 0;
            //    var tokenizerState = new TokenizerState();
            //    tokenizerState.AddCharacter(source[0]);
            //    for (int i = 0; i < source.Length - 1; i++)
            //    {
            //        tokenizerState.AddCharacter(source[i + 1]);
            //        if (tokenizerState.IsBreak())
            //        {
            //            yield return new TokenizerResult(source.Slice(startPosition, i - startPosition));
            //            startPosition = i;                        
            //        }                    
            //    }
            //    tokenizerState.NextCharacter();
            //    if (tokenizerState.IsBreak())
            //    {                    
            //        yield return new TokenizerResult(source.Slice(startPosition, source.Length - startPosition - 1));
            //        startPosition = source.Length - 1;
            //    }                
            //    yield return new TokenizerResult(source.Slice(startPosition));
            //}
        }         
    }
}
