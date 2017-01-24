using System;
using System.IO;
using System.Linq;
using System.Xml;
using TextParser.Common;
using TextParser.Xhtml.Tagging;

namespace TextParser.Xhtml
{
    internal class XhtmlParser : Parser
    {
        private readonly XmlReader fXmlReader;
        private string fCurrentPlainTextElement;        
        private readonly XhtmlTagger fXhtmlTagger;
        private int CurrentTokenIndex => fParsedText.Tokens.Count - 1; //fParsedText.Tokens.Count == 0 ? 0 : fParsedText.Tokens.Count - 1;
        private int fPlainTextXhtmlIndex = 0;

        // Public

        public XhtmlParser(string xhtmlText)
        {
            fCharacterIndex = 0;
            fXhtmlIndex = -1;
            fXmlReader = XmlReader.Create(new StringReader(xhtmlText));
            fXhtmlTagger = new XhtmlTagger(fParsedText);
        }

        public override void Dispose()
        {
            fXmlReader.Dispose();
        }

        // Internal

        protected override bool Read(out char c)
        {
            bool result;
            c = default(char);
            if (HasPlainText())
            {
                fCharacterIndex++;
                c = CurrentCharacter;
                result = true;
            }
            else
            {
                result = ReadXhtmlToPlainText();
                if (result)
                {
                    c = CurrentCharacter;
                }
            }
            return result;
        }

        protected override void ProcessTags()
        {
            fXhtmlTagger.ProcessTagsBuffer(ProcessingXhtmlIndex, ProcessingCharacterIndex, CurrentTokenIndex);
        }        

        protected override bool IsBreak()
        {
            return base.IsBreak() || fXhtmlTagger.IsBreak(ProcessingCharacterIndex, ProcessingXhtmlIndex);
        }

        private bool HasPlainText()
        {
            return (fCurrentPlainTextElement != null) && (fCharacterIndex < fCurrentPlainTextElement.Length - 1);
        } 

        private char CurrentCharacter => fCurrentPlainTextElement[fCharacterIndex];

        private bool ReadXhtmlToPlainText()
        {
            bool plainTextFound = false;
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
                        ProcessText(fXmlReader.Value);
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
            string elementRepresentation;
            if (tagKind == TagKind.Open)
            {
                elementRepresentation = (tagKind == TagKind.Empty) ? $"<{fXmlReader.Name}/>" : $"<{fXmlReader.Name}>";
                
            }
            else
            {
                elementRepresentation = $"</{elementName}>";
            }
            fParsedText.AddXhtmlElement(elementRepresentation);
            if (tagKind != TagKind.Empty)
            {
                fXhtmlTagger.ProcessXhtmlTag(elementName, tagKind, fPlainTextXhtmlIndex, fCharacterIndex);
            }
        }

        private void ProcessText(string value)
        {
            fCurrentPlainTextElement = value;            
            fCharacterIndex = 0;
            fParsedText.AddPlainTextElement(value);
        }
    }
}