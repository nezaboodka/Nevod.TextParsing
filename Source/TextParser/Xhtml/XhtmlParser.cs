using System;
using System.IO;
using System.Xml;
using TextParser.Common;
using TextParser.Common.Contract;
using TextParser.Common.WordBreaking;
using TextParser.Xhtml.Tagging;

namespace TextParser.Xhtml
{
    internal class XhtmlParser : Parser
    {
        private readonly XmlReader fXmlReader;
        private readonly XhtmlTagger fXhtmlTagger;
        private readonly CharacterBuffer fCharacterBuffer = new CharacterBuffer();
        private int fPlainTextXhtmlIndex;
        private int fTokenStartXhtmlIndex;
        private int fXhtmlIndex;

        private int ProcessingXhtmlIndex => fCharacterBuffer.CurrentCharacterInfo.XhtmlIndex;
        private int ProcessingCharacterIndex => fCharacterBuffer.CurrentCharacterInfo.StringPosition;
        private bool IsLastCharacterInBuffer => fCharacterBuffer.CurrentCharacterInfo.IsLastCharacterInBuffer;

        // Public

        public XhtmlParser(string xhtmlText)
        {
            fXhtmlIndex = -1;
            fXmlReader = XmlReader.Create(new StringReader(xhtmlText));
            fXhtmlTagger = new XhtmlTagger(fParsedText);
        }

        public override void Dispose()
        {
            fXmlReader.Dispose();
        }

        // Internal

        protected override ParsedText Parse()
        {
            int currentTokenLength = 0;
            InitializeLookahead();
            ProcessTags();
            while (NextCharacter())
            {
                currentTokenLength++;
                fTokenClassifier.AddCharacter(fCharacterBuffer.CurrentCharacterInfo.Character);
                if (IsBreak())
                {
                    SaveToken(currentTokenLength);
                    currentTokenLength = 0;
                    ProcessTags();
                }
            }
            return fParsedText;
        }

        private void InitializeLookahead()
        {
            NextCharacter();
            NextCharacter();
            fTokenStartXhtmlIndex = fCharacterBuffer.NextCharacterInfo.XhtmlIndex;
        }

        private void ProcessTags()
        {
            if (IsLastCharacterInBuffer)
            {
                fXhtmlTagger.ProcessTagsBuffer(ProcessingXhtmlIndex, CurrentTokenIndex);
            }
        }        

        private bool NextCharacter()
        {
            bool hasNext;
            if (fCharacterBuffer.NextCharacter())
            {
                hasNext = true;
            }
            else
            {
                string newBuffer;
                hasNext = ReadXhtmlToPlainText(out newBuffer);
                if (hasNext && newBuffer.Length > 0)
                {
                    fCharacterBuffer.SetBuffer(newBuffer, fXhtmlIndex);
                }
            }
            if (hasNext)
            {
                char c = fCharacterBuffer.NextOfNextCharacterInfo.Character;
                WordBreak wordBreak = WordBreakTable.GetCharacterWordBreak(c);
                fWordBreaker.AddWordBreak(wordBreak);
            }
            else if (HasCharacters())
            {
                MoveNext();
                hasNext = true;
            }
            return hasNext;
        }
        
        private bool IsBreak()
        {
            return fWordBreaker.IsBreak() || (IsLastCharacterInBuffer && fXhtmlTagger.IsBreak(ProcessingCharacterIndex, ProcessingXhtmlIndex));
        }

        private bool ReadXhtmlToPlainText(out string plainText)
        {
            bool plainTextFound = false;
            plainText = default(string);
            while (!plainTextFound && fXmlReader.Read())
            {
                fXhtmlIndex++;
                switch (fXmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        TagKind tagKind = (fXmlReader.IsEmptyElement) ? TagKind.Empty : TagKind.Open;
                        ProcessElement(fXmlReader.Name, tagKind);
                        break;
                    case XmlNodeType.EndElement:
                        ProcessElement(fXmlReader.Name, TagKind.Close);
                        break;
                    case XmlNodeType.Text:
                        plainTextFound = true;
                        plainText = fXmlReader.Value;
                        ProcessText(plainText);
                        fPlainTextXhtmlIndex = fXhtmlIndex;
                        break;
                    default:
                        fXhtmlIndex--; // in case of XmlNodeType, that is not handled
                        break;
                }
            }
            return plainTextFound;
        }

        private void ProcessElement(string elementName, TagKind tagKind)
        {
            string elementRepresentation = string.Empty;
            switch (tagKind)
            {
                case TagKind.Open:
                    elementRepresentation = $"<{fXmlReader.Name}>";
                    break;
                case TagKind.Empty:
                    elementRepresentation = $"<{fXmlReader.Name}/>";
                    break;
                case TagKind.Close:
                    elementRepresentation = $"</{elementName}>";
                    break;
            }
            fParsedText.AddXhtmlElement(elementRepresentation);
            if (tagKind != TagKind.Empty)
            {
                fXhtmlTagger.ProcessXhtmlTag(elementName, tagKind, fPlainTextXhtmlIndex, fCharacterBuffer.NextOfNextCharacterInfo.StringPosition);
            }
        }

        private void ProcessText(string value)
        {
            fParsedText.AddPlainTextElement(value);
        }

        private void MoveNext()
        {
            fCharacterBuffer.MoveNext();
            fWordBreaker.NextWordBreak();
        }

        private bool HasCharacters()
        {
            return !fWordBreaker.IsEmptyBuffer();
        }

        private void SaveToken(int currentTokenLength)
        {
            var token = new Token
            {
                TokenKind = fTokenClassifier.TokenKind,
                XhtmlIndex = fTokenStartXhtmlIndex,
                StringPosition = fTokenStartPosition,
                StringLength = currentTokenLength
            };
            fParsedText.AddToken(token);
            fTokenStartPosition = fCharacterBuffer.NextCharacterInfo.StringPosition;
            fTokenStartXhtmlIndex = fCharacterBuffer.NextCharacterInfo.XhtmlIndex;
            fTokenClassifier.Reset();
        }        
    }
}