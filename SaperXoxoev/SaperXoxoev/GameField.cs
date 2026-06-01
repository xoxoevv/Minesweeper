using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaperXoxoev
{
    public class GameField
    {
        public int Size { get; }

        public Cell[,] Cells;

        public GameField(int size)
        {
            Size = size;

            Cells = new Cell[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Cells[y, x] = new Cell();
                }
            }
        }

        public void SetMine(int y, int x)
        {
            Cells[y, x].IsMine = true;
        }

        public Cell GetCell(int y, int x)
        {
            return Cells[y, x];
        }
    }
}
