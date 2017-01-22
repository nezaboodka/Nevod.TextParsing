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
        public int CharacterIndex { get; }
        public TagKind TagKind { get; }

        public TagBufferItem(TagState tagState, TagKind tagKind, int plainTextXhtmlIndex, int characterIndex)
        {
            TagState = tagState;
            TagKind = tagKind;
            PlainTextXhtmlIndex = plainTextXhtmlIndex;
            CharacterIndex = characterIndex;
        }
    }
}