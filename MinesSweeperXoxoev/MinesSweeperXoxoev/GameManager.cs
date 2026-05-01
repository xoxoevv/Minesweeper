using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesSweeperXoxoev
{
    internal class GameManager
    {
        public int Size = 8;
        public int Mines = 10;

        public Cell[,] Board;

        public GameManager()
        {
            Board = new Cell[Size, Size];
            InitBoard();
        }

        public void InitBoard()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Board[i, j] = new Cell();

            PlaceMines();
            CalculateNumbers();
        }

        private void PlaceMines()
        {
            Random rnd = new Random();
            int placed = 0;

            while (placed < Mines)
            {
                int x = rnd.Next(Size);
                int y = rnd.Next(Size);

                if (!Board[x, y].HasMine)
                {
                    Board[x, y].HasMine = true;
                    placed++;
                }
            }
        }

        private void CalculateNumbers()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (Board[x, y].HasMine)
                        continue;

                    int count = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int nx = x + i;
                            int ny = y + j;

                            if (nx >= 0 && ny >= 0 && nx < Size && ny < Size)
                                if (Board[nx, ny].HasMine)
                                    count++;
                        }
                    }

                    Board[x, y].NeighborMines = count;
                }
            }
        }
    }
}
