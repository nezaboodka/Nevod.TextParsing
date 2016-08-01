namespace WordExtraction
{
    public abstract class ScanWindow
    {
        public abstract void AddSymbol(char symbol);
        public abstract void MoveWindow();
        public abstract bool CheckForBreak();
    }
}
