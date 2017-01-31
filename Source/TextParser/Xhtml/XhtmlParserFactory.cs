﻿using TextParser.Common;
using TextParser.Common.Contract;

namespace TextParser.Xhtml
{
    internal class XhtmlParserFactory : IParserFactory
    {
        public Parser CreateParser(string text)
        {
            return new XhtmlParser(text);
        }
    }
}