using System;
using Sharik.Text;

namespace TextParser
{
    public abstract class Parser
    {
        public static ParsedText GetTokensFromPlainText(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            Parser parser = new PlainTextParser(text);
            return parser.Parse();
        }

        // Internals

        protected abstract ParsedText Parse();                
    }
}
