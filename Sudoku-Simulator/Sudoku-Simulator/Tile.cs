using System.Collections.Generic;

namespace Sudoku_Simulator
{
    internal class Tile
    {
        private int tileNum;
        private bool valid;
        private HashSet<int> hashSet;

        public Tile()
        {
            this.valid = false;
            this.hashSet = new HashSet<int>();
        }

        public void SetTileNum(int tileNum)
        {
            this.tileNum = tileNum;
            this.valid = true;
            this.hashSet.Add(tileNum);
        }

        public int GetTileNum() => this.tileNum;

        public void Clear()
        {
            this.tileNum = 0;
            this.valid = false;
        }

        public void Reset()
        {
            Clear();
            this.hashSet.Clear();
        }

        public void SetValidity(bool validity) => this.valid = validity;

        public bool IsValid => this.valid;

        public void Attempt(int number) => this.hashSet.Add(number);

        public bool AlreadyAttempted(int number)
        {
            foreach (int attempt in hashSet)
            {
                if (attempt == number)
                    return true;
            }
            return false;
        }

        public int NumAttempts => this.hashSet.Count;
    }
}