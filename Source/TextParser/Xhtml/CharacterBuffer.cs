namespace TextParser.Xhtml
{
    internal struct CharacterPosition
    {
        public string Buffer;
        public int XhtmlIndex;
        public int StringPosition;
        public char Character => Buffer[StringPosition];
    }

    internal class CharacterBuffer
    {        
        private CharacterPosition fCurrentCharacterPosition;
        private CharacterPosition fNextCharacterPosition;
        private CharacterPosition fPosition;

        // Public

        public CharacterPosition CurrentCharacterInfo => fCurrentCharacterPosition;
        public CharacterPosition NextCharacterInfo => fNextCharacterPosition;
        public CharacterPosition NextOfNextCharacterInfo => fPosition;

        public void SetBuffer(string buffer, int xhtmlIndex)
        {
            MoveNext();
            fPosition.Buffer = buffer;
            fPosition.StringPosition = 0;
            fPosition.XhtmlIndex = xhtmlIndex;
        }

        public bool NextCharacter()
        {
            bool result;
            
            if ((!string.IsNullOrEmpty(fPosition.Buffer)) && (fPosition.StringPosition < fPosition.Buffer.Length - 1))
            {
                result = true;
                MoveNext();
                fPosition.StringPosition++;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public void MoveNext()
        {
            fCurrentCharacterPosition = NextCharacterInfo;
            fNextCharacterPosition = fPosition;            
        }
    }
}