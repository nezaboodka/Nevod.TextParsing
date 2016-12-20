using System.Collections.Generic;

namespace TextParser.Xhtml
{

    internal class XhtmlTagger
    {
        private readonly IDictionary<string, string> fXhtmlTagsNames = new Dictionary<string, string>()
        {
            {"p", "Paragraph"},
            {"title", "Heading"}
        };
        private readonly IDictionary<string, bool> fTagsStatuses = new Dictionary<string, bool>()
        {
            {"p", false },
            {"title", false }
        };

    }
}