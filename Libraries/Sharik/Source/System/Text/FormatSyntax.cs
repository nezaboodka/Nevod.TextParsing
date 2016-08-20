// Free open-source Sharik library - http://code.google.com/p/sharik/

using System.Collections.Generic;
using Sharik.Text.Tokenizing;

namespace Sharik.Text
{
    public class FormatParameterSyntax : FormatSyntax
    {
        public static TokenKind
            TkColon = Tk(":", ref gVocabulary),
            TkSobaka = Tk("@", ref gVocabulary),
            TkSemicolon = Tk(";", ref gVocabulary);

        public FormatParameterSyntax()
        {
            Vocabulary.AddRange(gVocabulary); // in addition to FormatSyntax tokens
            Predicates.ClearAndAdd(Space, Identifier, Number, String, Symbol);
        }

        private static Dictionary<string, TokenKind> gVocabulary;
    }

    // FormatSyntax

    public class FormatSyntax : SourceCodeSyntax
    {
        public static TokenKind
            TkTextBlock = Tk("a text block"),
            TkBraceLeft = Tk("{"),
            TkBraceRight = Tk("}"),
            TkBraceLeftEscaped = Tk("{{"),
            TkBraceRightEscaped = Tk("}}");

        public FormatSyntax()
        {
            VocabularySymbolsOnly = true;
            AddToVocabulary(TkBraceLeft, TkBraceRight, TkBraceLeftEscaped, TkBraceRightEscaped);
            Predicates.ClearAndAdd(TextBlock, Symbol);
        }

        public virtual bool TextBlock(char nextChar, Slice literal, ref TokenKind kind)
        {
            kind = TkTextBlock;
            return nextChar != '{' && nextChar != '}';
        }
    }
}
