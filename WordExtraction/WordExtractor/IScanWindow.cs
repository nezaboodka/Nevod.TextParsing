namespace WordExtraction
{
    public interface IScanWindow
    {
        void AddSymbol(char symbol);
        void MoveWindow();
        bool CheckForBreak();
    }
}
