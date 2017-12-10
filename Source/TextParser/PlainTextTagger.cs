namespace TextParser
{
    internal class PlainTextTagger // Tagging of paragraphs in plain text. Double line feed is treated as a new paragraph.
    {
        private readonly ParsedText fParsedText;
        private TokenKind fPreviousTokenKind = TokenKind.Empty;
        private int fTagStart = 0;
        private int TokenPosition => fParsedText.PlainTextTokens.Count - 1;
        private const string TagName = "Parapraph";

        // Public

        public PlainTextTagger(ParsedText parsedText)
        {
            fParsedText = parsedText;
        }

        public void ProcessToken(TokenKind tokenKind)
        {
            if (tokenKind == TokenKind.LineFeed)
            {
                if (fPreviousTokenKind == TokenKind.LineFeed)
                {
                    fParsedText.AddTag(new FormattingTag
                    {
                        TagName = TagName,
                        TokenPosition = fTagStart,
                        TokenLength = TokenPosition - fTagStart - 1
                    });
                    fPreviousTokenKind = TokenKind.Empty;
                    fTagStart = TokenPosition + 1;
                }
                else
                    fPreviousTokenKind = TokenKind.LineFeed;
            }
            else
                fPreviousTokenKind = TokenKind.Empty;
        }

        public void ProcessEndOfText()
        {
            fParsedText.AddTag(new FormattingTag
            {
                TokenPosition = fTagStart,
                TokenLength = TokenPosition - fTagStart + 1,
                TagName = TagName
            });
        }
    }
}
