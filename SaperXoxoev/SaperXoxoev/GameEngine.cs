using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaperXoxoev
{
    internal class GameEngine
    {

        public int[,] field;

        public int Size = 9;

        public int BombsCount;

        Random rnd = new Random();

        public GameEngine(int bombs)
        {
            BombsCount = bombs;

            field = new int[Size, Size];
        }

        public void GenerateBombs(int safeRow, int safeColumn)
        {
            int cntBomb = 0;

            while (cntBomb < BombsCount)
            {
                int row = rnd.Next(0, Size);

                int column = rnd.Next(0, Size);

                if (field[row, column] == 0 &&
                    !(row == safeRow && column == safeColumn))
                {
                    field[row, column] = 1;

                    cntBomb++;
                }
            }
        }

        public int CountBombsAround(int row, int column)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int r = row + i;

                    int c = column + j;

                    if (r >= 0 && r < Size &&
                        c >= 0 && c < Size)
                    {
                        if (field[r, c] == 1)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

    }
}
