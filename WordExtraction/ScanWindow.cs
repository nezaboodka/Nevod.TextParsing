namespace WordExtraction
{
    class ScanWindow : IScanWindow
    {
        public SymbolType? Ahead { get; private set; }

        public SymbolType? Current { get; private set; }

        public SymbolType? Behind { get; private set; }

        public SymbolType? BehindOfBehind { get; private set; }

        private void MoveWindow()
        {
            BehindOfBehind = Behind;
            Behind = Current;
            Current = Ahead;
        }

        public void AddSymbol(char symbol)
        {
            MoveWindow();
            Ahead = SymbolTable.GetSymbolType(symbol);
        }

        public void AddEmpty()
        {
            MoveWindow();
            Ahead = null;
        }

        public bool CheckForBreak()
        {
            bool result;

            if (Behind != null)
                result = true;
            else
                result = false;

            return result;
        }

        public ScanWindow(char firstSymbol)
        {
            AddSymbol(firstSymbol);
        }
    }
}
