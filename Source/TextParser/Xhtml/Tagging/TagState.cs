namespace TextParser.Xhtml.Tagging
{
    internal class TagState
    {
        public string TagName { get; }
        public int StartTokenPosition { get; set; }
        public bool IsOpen => StartTokenPosition != -1;

        public TagState(string tagName)
        {
            TagName = tagName;
            StartTokenPosition = -1;
        }

        public void Close()
        {
            StartTokenPosition = -1;
        }
    }
}