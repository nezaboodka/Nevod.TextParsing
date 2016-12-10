using System;
using System.Xml;

namespace TextParser
{
    internal class XhtmlParser : Parser
    {
        private XmlReader fXmlReader;
        //private int fXhtml

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