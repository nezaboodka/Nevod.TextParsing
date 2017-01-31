using System.Collections.Generic;
using TextParser.Common;
using TextParser.Common.Contract;

namespace TextParser.Xhtml.Tagging
{
    internal class XhtmlTagger
    {
        private readonly ParsedText fParsedText;
        private readonly Queue<TagBufferItem> fTagsBuffer = new Queue<TagBufferItem>();
        private readonly Dictionary<string, TagState> fTagsInfo = new Dictionary<string, TagState>()
        {
            {"p", new TagState("Paragraph") },
            {"title", new TagState("Title") },
            {"h1", new TagState("Heading") },
            {"h2", new TagState("Heading") },
            {"h3", new TagState("Heading") },
            {"h4", new TagState("Heading") },
            {"h5", new TagState("Heading") },
            {"h6", new TagState("Heading") }
        };
        private bool IsEmptyBuffer => fTagsBuffer.Count == 0;

        // Public

        public XhtmlTagger(ParsedText parsedText)
        {
            fParsedText = parsedText;
        }

        public void ProcessXhtmlTag(string xhtmlTagName, TagKind tagKind, int plainTextXhtmlIndex, int characterIndex)
        {
            TagState tagState;
            if (fTagsInfo.TryGetValue(xhtmlTagName, out tagState))
            {
                var tagBufferItem = new TagBufferItem(tagState, tagKind, plainTextXhtmlIndex, characterIndex);
                fTagsBuffer.Enqueue(tagBufferItem);
            }            
        }
        
        public void ProcessTagsBuffer(int xhtmlIndex, int characterIndex, int tokenPosition)
        {
            bool hasUnprocessedTags = true;
            while (!IsEmptyBuffer && hasUnprocessedTags)
            {
                TagBufferItem tagBufferItem = fTagsBuffer.Peek();
                if ((tagBufferItem.PlainTextXhtmlIndex == xhtmlIndex) && (tagBufferItem.CharacterIndex == characterIndex))
                {
                    TagState tagState = tagBufferItem.TagState;
                    switch (tagBufferItem.TagKind)
                    {
                        case TagKind.Open:                            
                            tagState.StartTokenPosition = tokenPosition + 1;
                            break;
                        case TagKind.Close:
                            int tokenLength = tokenPosition - tagState.StartTokenPosition + 1;
                            if (tokenLength > 0)
                            {
                                SaveTag(tagState, tokenLength);
                            }
                            break;
                    }
                    fTagsBuffer.Dequeue();
                }
                else
                {
                    hasUnprocessedTags = false;
                }
            }
        }

        public bool IsBreak(int characterIndex, int plainTextXhtmlIndex)
        {
            bool result = false;
            if (!IsEmptyBuffer)
            {
                TagBufferItem lastBufferItem = fTagsBuffer.Peek();
                result = (lastBufferItem.PlainTextXhtmlIndex == plainTextXhtmlIndex) && (lastBufferItem.CharacterIndex == characterIndex);
            }
            return result;
        }

        // Internal        

        private void SaveTag(TagState tagState, int tokenLength)
        {            
            var tag = new Tag
            {
                TagName = tagState.TagName,
                TokenPosition = tagState.StartTokenPosition,
                TokenLength = tokenLength
            };
            fParsedText.AddTag(tag);
            tagState.Close();
        }               
    }
}