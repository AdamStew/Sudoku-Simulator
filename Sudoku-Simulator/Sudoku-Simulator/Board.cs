using System;

namespace Sudoku_Simulator
{
    internal class Board
    {
        private Tile[][] board;
        private const int sectionSize = 3;
        private const int boardSize = 9;

        public Board()
        {
            this.board = new Tile[9][];
            for (int row = 0; row < this.board.Length; row++)
            {
                this.board[row] = new Tile[9];
                for (int column = 0; column < this.board[row].Length; column++)
                {
                    this.board[row][column] = new Tile();
                }
            }
        }

        public Tile[][] getBoard() => this.board;

        public void GenerateRandomNumber(int row, int column)
        {
            Random rnd = new Random();

            if (!this.board[boardSize - 1][boardSize - 1].IsValid) //Check to see if all slots are full.
            {
                while (this.board[row][column].NumAttempts < boardSize) //While we haven't attempted all posibilities for this tile.
                {
                    int randomNumber;
                    do
                    {
                        randomNumber = rnd.Next(boardSize) + 1; //1-9;
                    } while (this.board[row][column].AlreadyAttempted(randomNumber)); //Check if it's a new random number.
                    if (CheckIfValidNum(row, column, randomNumber)) //Check if it's in a valid position.
                    {
                        this.board[row][column].SetTileNum(randomNumber);
                        //Console.WriteLine("Valid ({0}, {1}): {2}", row+1, column+1, randomNumber);

                        //Increment row and column for the next cell.
                        if(row != 8 || column != 8)
                        {
                            column = (column + 1) % 9;
                            if (column == 0)
                                row++;
                            GenerateRandomNumber(row, column); //Recursively generate next random tile.
                        }
                    } else
                        this.board[row][column].Attempt(randomNumber); //If it's not a valid position, mark it as an attempt.
                }
                if(!this.board[boardSize - 1][boardSize - 1].IsValid) //If the board isn't completed, we must backtrack.
                {
                    this.board[row][column].Reset();
                }
            }
        }

        public void Print()
        {
            for (int row = 0; row < this.board.Length; row++)
            {
                for (int column = 0; column < this.board[row].Length; column++)
                {
                    Console.Write(" {0} ", this.board[row][column].GetTileNum());
                    if ((column % 3) == 2)
                        Console.Write("|");
                }
                Console.WriteLine("");
                if ((row % 3) == 2)
                    Console.WriteLine("--------------------------------");
            }
        }

        public bool CheckIfValidNum(int row, int column, int entryNum)
        {
            //Check all criteria for inserting a number.
            if (CheckSection(row, column, entryNum) && CheckRow(row, entryNum) && CheckColumn(column, entryNum))
                return true;
            else
                return false;
        }

        private bool CheckSection(int row, int column, int entryNum)
        {
            int sectionRow = row / sectionSize;
            int sectionColumn = column / sectionSize;

            //Don't have to check every entry in the board, just the ones in our desired section.
            for (int i = 0; i < sectionSize; i++)
            {
                for (int j = 0; j < sectionSize; j++)
                {
                    if (this.board[i + (sectionRow * sectionSize)][j + (sectionColumn * sectionSize)].GetTileNum() == entryNum)
                        return false;
                }
            }
            return true;
        }

        private bool CheckRow(int row, int entryNum)
        {
            for (int i = 0; i < this.board[row].Length; i++)
            {
                if (this.board[row][i].GetTileNum() == entryNum)
                    return false;
            }
            return true;
        }

        private bool CheckColumn(int column, int entryNum)
        {
            for (int i = 0; i < this.board.Length; i++)
            {
                if (this.board[i][column].GetTileNum() == entryNum)
                    return false;
            }
            return true;
        }
    }
}