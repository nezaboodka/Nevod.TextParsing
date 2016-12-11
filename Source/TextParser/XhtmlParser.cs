using System;
using System.Xml;

namespace TextParser
{
    internal class XhtmlParser : Parser
    {
        private XmlReader fXmlReader;
        private string fXhtmlText;

        public XhtmlParser(string xhtmlText)
        {
            fXhtmlText = xhtmlText;
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
    }
}