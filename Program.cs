using System.Diagnostics;

namespace CSharpGameOfLife
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Board Game Size
            const int boardW = 35;
            const int boardH = 28;

            
            // Table 2-D
            // Array Table and buffer for logic - Both Tables have size BoardW x BoardH
            GameTable[,] board = new GameTable[boardW, boardH];
            GameTable[,] boardBuffer;

            // Object for obtaining random numbers
            Random rng = new Random();
            Console.CursorVisible = false;

            GenerateBoard();
            PrintBoard();
            while (true) 
            {
                DeadOrAlive();
                PrintBoard();
                Thread.Sleep(100);
            }
            


            void GenerateBoard()
            {
                // Chance of spawning a living cell at start (in percent)
                const double aliveOdds = 15;

                // To fill the array, we can cross it in its ordinary direction (i then j)
                for (int i = 0; i < boardW; i++)
                {
                    for (int j = 0; j < boardH; j++)
                    {
                        // Draw a random double between 0.0 and 1.0
                        double randomValue = rng.NextDouble();

                        if (randomValue * 100 < aliveOdds)
                        {
                            board[i, j] = new GameTable(1);
                        }
                        else
                        {
                            board[i, j] = new GameTable(0);
                        }
                    }
                }
            }

            void PrintBoard()
            {
                // Move cursor back to top left to avoid console scroll bug
                Console.SetCursorPosition(0, 0);

                // To print the array, this time we must traverse it in its reverse direction, or 'row-first' (y (row) then x (col))
                for (int row = 0; row < boardH; row++)
                {
                    for (int col = 0; col < boardW; col++)
                    {
                        if (board[col, row].State == 0)
                        {
                            Console.Write('.');
                        }
                        else
                        {
                            Console.Write('o');
                        }
                    }
                    Console.WriteLine();
                }
            }

            void DeadOrAlive()
            {
                boardBuffer = new GameTable[boardW, boardH];

                for (int i = 0; i < boardW; i++)
                {
                    for (int j = 0; j < boardH; j++)
                    {
                        int neighbors = CountNeighbors(i, j);
                        boardBuffer[i, j].State = board[i, j].State == 0 ? (neighbors == 3 ? 1 : 0) : (neighbors < 2 || neighbors > 3 ? 0 : 1);
                    }
                }

                board = boardBuffer;
            }


            int CountNeighbors(int x, int y)
            {
                int accumulator = 0;
                for (int i = -1; i < 2; i++)
                {
                    int currentX = x + i;
                    if (currentX < 0 || currentX >= boardW)
                    {
                        continue;
                    }
                    for (int j = -1; j < 2; j++)
                    {
                        int currentY = y + j;
                        if (currentY < 0 || currentY >= boardH || (i == 0 && j == 0))
                        {
                            continue;
                        }
                        if (board[currentX, currentY].State == 1)
                        {
                            accumulator++;
                        }
                    }
                }
                return accumulator;
            }


        }





        struct GameTable
        {
            internal int State;
            public GameTable(int state)
            {
                State = state;
            }
        }



    }
}