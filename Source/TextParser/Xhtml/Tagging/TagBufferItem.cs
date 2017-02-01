namespace TextParser.Xhtml.Tagging
{
    internal enum TagKind
    {
        Open,
        Close,
        Empty
    }

    internal class TagBufferItem
    {
        public TagState TagState { get; }
        public int PlainTextXhtmlIndex { get; }
        public TagKind TagKind { get; }

        public TagBufferItem(TagState tagState, TagKind tagKind, int plainTextXhtmlIndex)
        {
            TagState = tagState;
            TagKind = tagKind;
            PlainTextXhtmlIndex = plainTextXhtmlIndex;
        }
    }
}