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
            fCurrentPosition = -1;
            fXhtmlIndex = 0;
            fXmlReader = XmlReader.Create(new StringReader(xhtmlText));
            var xmlReaderNodeType = fXmlReader.NodeType;
        }

        public override void Dispose()
        {
            fXmlReader.Dispose();
        }

        // Internals

        protected override bool Read(out char c)
        {
            throw new NotImplementedException();
        }

        private void ReadXhtml()
        {
            while (fXmlReader.Read() && (fXmlReader.NodeType != XmlNodeType.Text))
            {
                
            }
        }
    }
}