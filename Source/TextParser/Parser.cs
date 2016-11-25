﻿using System;
using Sharik.Text;

namespace TextParser
{
    public class Parser
    {
        private readonly WordBreakerState fWordBreakerState;
        private readonly ParserState fTokenizerState;
        private readonly string fText;
        private int fCurrentPosition;
        private readonly ParsedText fParsedText;

        public static ParsedText GetTokensFromPlainText(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            var tokenizer = new Parser(text);
            return tokenizer.GetTokensFromPlainText();
        }

        // Internals

        private Parser(string text)
        {
            fText = text;
            fWordBreakerState = new WordBreakerState(text);
            fTokenizerState = new ParserState();
            fParsedText = new ParsedText(text);
            fCurrentPosition = -1;
        }
        
        protected virtual ParsedText GetTokensFromPlainText()
        {
            while (NextCharacter())
            {
                if (IsBreak())
                {
                    NextToken();
                }
            }                

            return fParsedText;
        }

        protected virtual bool NextCharacter()
        {
            bool result = false;
            fCurrentPosition++;
            if (fCurrentPosition < fText.Length)
            {
                fWordBreakerState.NextCharacter();
                fTokenizerState.AddCharacter(fText[fCurrentPosition]);
                result = true;
            }
            return result;            
        }

        protected virtual bool IsBreak()
        {
            return fWordBreakerState.IsBreak();
        }

        protected virtual void NextToken()
        {
            fParsedText.AddToken(fCurrentPosition, fTokenizerState.TokenKind);
            fTokenizerState.Reset();
        }
    }
}