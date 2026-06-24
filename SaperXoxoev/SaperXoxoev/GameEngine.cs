using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaperXoxoev
{
    internal class GameEngine
    {
        public Cell[,] field;  
        public int Size = 9;
        public int BombsCount;
        Random rnd = new Random();

        public GameEngine(int bombs)
        {
            BombsCount = bombs;
            field = new Cell[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    field[i, j] = new Cell();
                }
            }
        }

        // Генерация мин после первого клика
        public void GenerateBombs(int safeRow, int safeColumn)
        {
            int cntBomb = 0;
            while (cntBomb < BombsCount)
            {
                int row = rnd.Next(0, Size);
                int column = rnd.Next(0, Size);

                if (!field[row, column].IsMine && !(row == safeRow && column == safeColumn))
                {
                    field[row, column].IsMine = true;
                    cntBomb++;
                }
            }
        }

        // Подсчет мин вокруг клетки
        public int CountBombsAround(int row, int column)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int r = row + i;
                    int c = column + j;

                    if (r >= 0 && r < Size && c >= 0 && c < Size)
                    {
                        if (field[r, c].IsMine)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        // Сброс всех клеток для новой игры
        public void ResetField()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    field[i, j] = new Cell();
                }
            }
        }

        // Проверка, открыта ли клетка
        public bool IsCellOpen(int row, int column)
        {
            return field[row, column].IsOpen;
        }

        // Проверка, стоит ли флаг
        public bool IsCellFlagged(int row, int column)
        {
            return field[row, column].IsFlag;
        }

        // Установить флаг
        public void SetFlag(int row, int column, bool value)
        {
            field[row, column].IsFlag = value;
        }

        // Открыть клетку
        public void OpenCell(int row, int column)
        {
            field[row, column].IsOpen = true;
        }
    }
}