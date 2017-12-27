using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            board.GenerateRandomNumber(0,0); //Enter starting coordinates (should always be 0, 0);
            board.Print();
            Console.ReadLine();
        }
    }
}
