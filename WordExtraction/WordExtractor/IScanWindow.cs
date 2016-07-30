namespace WordExtraction
{
    interface IScanWindow
    {
        void AddSymbol(char symbol);
        bool CheckForBreak();
    }
}
