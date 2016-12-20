using System;

namespace TextParser.Common
{
    internal struct CharacterInfo
    {
        public int XhtmlIndex;
        public int StringPosition;
        public char Character;

        public CharacterInfo(char character, int xhtmlIndex, int stringPosition)
        {
            XhtmlIndex = xhtmlIndex;
            StringPosition = stringPosition;
            Character = character;
        }
    }

    internal class CharacterBuffer
    {
        private const int BufferSize = 3;
        private readonly CharacterInfo[] fCharacters = new CharacterInfo[BufferSize];

        // Public

        public CharacterInfo CurrentCharacterInfo => fCharacters[BufferSize - 1];
        public CharacterInfo NextCharacterInfo => fCharacters[BufferSize - 2];
        public CharacterInfo NextOfNextCharacterInfo => fCharacters[0];

        public void AddCharacter(CharacterInfo characterInfo)
        {
            NextCharacter();
            fCharacters[0] = characterInfo;
        }

        public void NextCharacter()
        {
            Array.Copy(fCharacters, 0, fCharacters, 1, BufferSize - 1);            
        }        
    }
}