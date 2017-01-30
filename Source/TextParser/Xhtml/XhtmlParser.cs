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
        private readonly XhtmlTagger fXhtmlTagger;
        private int fPlainTextXhtmlIndex;

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

        protected override bool FillBuffer(out string buffer)
        {
            return ReadXhtmlToPlainText(out buffer);
        }

        protected override void ProcessTags()
        {
            fXhtmlTagger.ProcessTagsBuffer(ProcessingXhtmlIndex, ProcessingCharacterIndex, fLastXhtmlElement == ProcessingXhtmlIndex);
        }        

        protected override bool IsBreak()
        {
            return base.IsBreak() || fXhtmlTagger.IsBreak(ProcessingCharacterIndex, ProcessingXhtmlIndex);
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
                fXhtmlTagger.ProcessXhtmlTag(elementName, tagKind, fPlainTextXhtmlIndex, CharacterBuffer.NextOfNextCharacterInfo.StringPosition);
            }
        }

        private void ProcessText(string value)
        {
            fParsedText.AddPlainTextElement(value);
        }        
    }
}