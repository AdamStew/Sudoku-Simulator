using System;

namespace Sudoku_Simulator
{
    internal class Board
    {
        private Tile[][] board;
        private const int sectionSize = 3;
        private const int boardSize = 9;
        private int numEmpty;

        public Board()
        {
            this.board = new Tile[boardSize][];
            for (int row = 0; row < this.board.Length; row++)
            {
                this.board[row] = new Tile[boardSize];
                for (int column = 0; column < this.board[row].Length; column++)
                {
                    this.board[row][column] = new Tile();
                }
            }
            this.numEmpty = boardSize*boardSize; //Default 9x9 board.
        }

        public Tile[][] GetBoard() => this.board;

        public void SetBoard(Board board)
        {
            if(this.board.Length == board.GetBoardSize() && this.board[0].Length == board.GetBoardSize())
            {
                for(int row=0; row < this.board.Length; row++)
                {
                    for(int column=0; column < this.board[row].Length; column++)
                    {
                        Tile[][] temp = board.GetBoard();
                        this.board[row][column].SetTile(temp[row][column]);
                    }
                }
            }
            this.numEmpty = board.GetNumEmpty();
        }

        public int GetBoardSize() => boardSize;

        public int GetNumEmpty() => numEmpty;

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
                        this.numEmpty--;

                        //Increment row and column for the next cell.
                        if(row != 8 || column != 8)
                        {
                            int newRow = row;
                            int newColumn = (column + 1) % 9;
                            if (newColumn == 0)
                                newRow++;
                            GenerateRandomNumber(newRow, newColumn); //Recursively generate next random tile.
                        }
                    } else
                        this.board[row][column].Attempt(randomNumber); //If it's not a valid position, mark it as an attempt.
                }

                if(!this.board[boardSize - 1][boardSize - 1].IsValid) //If the board isn't completed, we must backtrack.
                {
                    this.board[row][column].Reset();
                    this.numEmpty++;
                }
            }
        }

        public void SelectDifficulty(string difficulty)
        {
            if(difficulty.ToLower().Equals("easy"))
            {
                Remove(15);
            } else if(difficulty.ToLower().Equals("medium"))
            {
                Remove(30);
            } else if(difficulty.ToLower().Equals("hard"))
            {
                Remove(45);
            } else if(difficulty.ToLower().Equals("expert"))
            {
                Remove(60);
            }
        }
        public void Print()
        {
            for (int row = 0; row < this.board.Length; row++)
            {
                for (int column = 0; column < this.board[row].Length; column++)
                {
                    if (board[row][column].GetTileNum() == 0)
                        Console.Write("   ");
                    else
                        Console.Write(" {0} ", this.board[row][column].GetTileNum());

                    if ((column % 3) == 2 && column != (this.board[row].Length - 1))
                        Console.Write("|");
                }
                Console.WriteLine("");
                if ((row % 3) == 2 && row != (this.board.Length - 1))
                    Console.WriteLine("---------+---------+---------");
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

        public int NumberSolutions()
        {
            int row = 0, column = 0;
            for(int i=0; i < this.numEmpty*9; i++) //Performing a Brute Force solve, to see if we have more than one solution (invalid).
            {
                while(this.board[row][column].IsValid) //Find empty slot.
                {
                    if (row != 8 || column != 8)
                    {
                        column = (column + 1) % 9;
                        if (column == 0)
                            row++;
                    }
                }
            }
            return 0;
        }

        public void Solve(int row, int column)
        {
            int newRow=row, newColumn=column;

            //Find a valid starting point.
            if (row==0 && column==0 && this.board[row][column].IsValid)
            {
                do
                {
                    if (newRow != 8 || newColumn != 8)
                    {
                        newColumn = (newColumn + 1) % 9;
                        if (newColumn == 0)
                            newRow++;
                    }
                } while (this.board[newRow][newColumn].IsValid);
                //Console.WriteLine("Slot {0}, {1} seems empty.", newRow+1, newColumn+1);
                Solve(newRow, newColumn); //Starts here, assuming first slot in puzzle isn't empty.
            }
            
            Random rnd = new Random();

            if (this.numEmpty != 0) //Run until no empty slots.
            {
                while (this.board[row][column].NumAttempts < boardSize) //While we haven't attempted all posibilities for this tile.
                {
                    int randomNumber;

                    do //Search for a new random number.
                    {
                        randomNumber = rnd.Next(boardSize) + 1; //1-9;
                    } while (this.board[row][column].AlreadyAttempted(randomNumber)); //Check if it's a new random number.

                    if (CheckIfValidNum(row, column, randomNumber)) //Check if it's in a valid position.
                    {
                        this.board[row][column].SetTileNum(randomNumber); //Set valid number.
                        this.numEmpty--; //One less empty.

                        //Increment row and column for the next empty cell.
                        do
                        {
                            if (newRow != 8 || newColumn != 8)
                            {
                                newColumn = (newColumn + 1) % 9;
                                if (newColumn == 0)
                                    newRow++;
                            }
                            else
                                return;
                        }while(this.board[newRow][newColumn].IsValid); //Look for an empty cell.

                        //Console.WriteLine("Next empty spot is {0}, {1}.", newRow+1, newColumn+1);
                        Solve(newRow, newColumn); //Recursively solve the next spot.
                    }
                    else
                        this.board[row][column].Attempt(randomNumber); //If it's not a valid position, mark it as an attempt.
                }
            }

            if (this.numEmpty != 0) //If the board isn't completed, we must backtrack.
            {
                if (this.board[row][column].IsValid)
                    this.numEmpty++;
                this.board[row][column].Reset();
            }
        }
        
        public void Remove(int numRemoving)
        {
            int row, column;
            Random rnd = new Random();
            
            while (this.numEmpty != numRemoving)
            {

                do
                {
                    row = rnd.Next(boardSize);
                    column = rnd.Next(boardSize);
                } while (!this.board[row][column].IsValid);

                    this.board[row][column].Reset();
                    this.numEmpty++; //New empty slot.
                    Remove(numRemoving); //Keep removing until we've removed as much as we wanted.
            }
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