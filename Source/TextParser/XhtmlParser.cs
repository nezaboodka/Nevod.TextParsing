using System;
using System.IO;
using System.Xml;

namespace TextParser
{
    internal class XhtmlParser : Parser
    {
        private readonly XmlReader fXmlReader;
        private string fCurrentPlainTextElement;

        public XhtmlParser(string xhtmlText)
        {
            fCurrentPosition = 0;
            fXhtmlIndex = -1;
            fXmlReader = XmlReader.Create(new StringReader(xhtmlText));            
        }

        public override void Dispose()
        {
            fXmlReader.Dispose();
        }

        // Internals

        protected override bool Read(out char c)
        {
            bool result;
            c = default(char);
            if (HasPlainText)
            {
                fCurrentPosition++;
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

        private bool HasPlainText => (fCurrentPlainTextElement != null) && (fCurrentPosition < fCurrentPlainTextElement.Length - 1);

        private char CurrentCharacter => fCurrentPlainTextElement[fCurrentPosition];

        private bool ReadXhtmlToPlainText()
        {
            bool plainTextFound = false;
            while (!plainTextFound && fXmlReader.Read())
            {
                fXhtmlIndex++;
                switch (fXmlReader.NodeType)
                {
                    case XmlNodeType.Element:                        
                        ProcessElement(fXmlReader.Name, fXmlReader.IsEmptyElement);
                        break;
                    case XmlNodeType.EndElement:
                        ProcessEndElement(fXmlReader.Name);
                        break;
                    case XmlNodeType.Text:
                        plainTextFound = true;
                        ProcessText(fXmlReader.Value);
                        break;
                }
            }
            return plainTextFound;
        }

        private void ProcessElement(string name, bool isEmptyElement)
        {
            string xhtmlElement = isEmptyElement ? $"<{name}/>" : $"<{name}>";
            fParsedText.AddXhtmlElement(xhtmlElement);
        }

        private void ProcessEndElement(string name)
        {
            string xhtmlElements = $"</{name}>";
            fParsedText.AddXhtmlElement(xhtmlElements);
        }

        private void ProcessText(string value)
        {
            fCurrentPlainTextElement = value;            
            fCurrentPosition = 0;
            fParsedText.AddPlainTextElement(value);
        }
    }
}