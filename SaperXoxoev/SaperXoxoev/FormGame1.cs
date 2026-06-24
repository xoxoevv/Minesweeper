using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaperXoxoev
{
    public partial class FormGame1 : Form
    {
        // Храним всё в engine — больше никаких дублирующих полей!
        private GameEngine engine;

        Image bomb, empty, flag, four, one, three, two, unknown;

        bool defeat = false;
        int flagsCnt = 0;
        bool FirstClick = true;
        int bombsCount;

        public FormGame1(int bombs)
        {
            InitializeComponent();

            bombsCount = bombs;
            flagsCnt = bombs;

            engine = new GameEngine(bombsCount);

            // Загрузка изображений
            bomb = Bitmap.FromFile("images/bomb.png");
            empty = Bitmap.FromFile("images/empty.png");
            flag = Bitmap.FromFile("images/flag.png");
            four = Bitmap.FromFile("images/four.png");
            one = Bitmap.FromFile("images/one.png");
            three = Bitmap.FromFile("images/three.png");
            two = Bitmap.FromFile("images/two.png");
            unknown = Bitmap.FromFile("images/unknown.png");
        }

        private void CheckVictory()
        {
            int opened = 0;
            int safeCells = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Cell cell = engine.field[i, j];

                    if (!cell.IsMine) safeCells++;      // Безопасная клетка
                    if (cell.IsOpen && !cell.IsMine) opened++;  // Открытая безопасная
                }
            }

            if (opened == safeCells)
            {
                ShowBombs();

                DialogResult result = MessageBox.Show("Вы выиграли! Сыграть ещё?", "Победа", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    NewGame();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void ShowBombs()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (engine.field[i, j].IsMine)
                    {
                        dataGridViewGameField.Rows[i].Cells[j].Value = bomb;
                    }
                }
            }
        }

        public void Defeat()
        {
            ShowBombs();

            DialogResult result = MessageBox.Show("Вы проиграли. Сыграть ещё?", "Поражение", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                NewGame();
            }
            else
            {
                Application.Exit();
            }
        }

        private void OpenEmptyCells(int row, int column)
        {
            // Проверка выхода за границы
            if (row < 0 || row >= 9 || column < 0 || column >= 9)
                return;

            Cell cell = engine.field[row, column];

            // Если клетка уже открыта или стоит флаг — выходим
            if (cell.IsOpen)
                return;

            if (cell.IsFlag)
                return;

            int bombs = engine.CountBombsAround(row, column);

            if (bombs == 0)
            {
                // Пустая клетка — открываем и идем дальше (РЕКУРСИЯ!)
                dataGridViewGameField.Rows[row].Cells[column].Value = empty;
                cell.IsOpen = true;

                // Обходим все 8 соседних клеток
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;

                        OpenEmptyCells(row + i, column + j);
                    }
                }
            }
            else
            {
                // Открываем клетку с цифрой
                cell.IsOpen = true;

                switch (bombs)
                {
                    case 1:
                        dataGridViewGameField.Rows[row].Cells[column].Value = one;
                        break;

                    case 2:
                        dataGridViewGameField.Rows[row].Cells[column].Value = two;
                        break;

                    case 3:
                        dataGridViewGameField.Rows[row].Cells[column].Value = three;
                        break;

                    case 4:
                        dataGridViewGameField.Rows[row].Cells[column].Value = four;
                        break;
                }
            }
        }

        public void NewGame()
        {
            defeat = false;
            FirstClick = true;
            flagsCnt = bombsCount;

            labelCountFlag.Text = "Количество флажков: " + flagsCnt.ToString();

            // Создаем новую логику и поле
            engine = new GameEngine(bombsCount);

            // Сбрасываем отображение
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dataGridViewGameField.Rows[i].Cells[j].Value = unknown;
                }
            }
        }

        private void dataGridViewGameField_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Если уже поражение или победа — не даем кликать
            if (defeat)
                return;

            // Если клетка уже открыта — выходим
            if (engine.field[e.RowIndex, e.ColumnIndex].IsOpen)
                return;

            // Если стоит флаг — не открываем
            if (engine.field[e.RowIndex, e.ColumnIndex].IsFlag)
                return;

            // Первый клик — генерируем мины
            if (FirstClick)
            {
                engine.GenerateBombs(e.RowIndex, e.ColumnIndex);
                FirstClick = false;
            }

            // Если попали на мину — поражение
            if (engine.field[e.RowIndex, e.ColumnIndex].IsMine)
            {
                dataGridViewGameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = bomb;
                defeat = true;
                Defeat();
                return;
            }

            OpenEmptyCells(e.RowIndex, e.ColumnIndex);
            CheckVictory();
        }

        private void dataGridViewGameField_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Если уже поражение или победа — не даем ставить флаги
            if (defeat)
                return;

            if (e.Button == MouseButtons.Right)
            {
                Cell cell = engine.field[e.RowIndex, e.ColumnIndex];

                // Не ставим флаг на открытую клетку
                if (cell.IsOpen)
                    return;

                // Ставим флаг, если его нет и есть флажки
                if (!cell.IsFlag && flagsCnt > 0)
                {
                    dataGridViewGameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = flag;
                    cell.IsFlag = true;
                    flagsCnt--;
                    labelCountFlag.Text = "Количество флажков: " + flagsCnt.ToString();
                }
            }
        }

        private void dataGridViewGameField_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Если уже поражение или победа — не даем снимать флаги
            if (defeat)
                return;

            if (e.Button == MouseButtons.Right)
            {
                Cell cell = engine.field[e.RowIndex, e.ColumnIndex];

                // Снимаем флаг, если он есть
                if (cell.IsFlag)
                {
                    dataGridViewGameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = unknown;
                    cell.IsFlag = false;
                    flagsCnt++;
                    labelCountFlag.Text = "Количество флажков: " + flagsCnt.ToString();
                }
            }
        }

        private void FormGame1_Load(object sender, EventArgs e)
        {
            dataGridViewGameField.Columns.Clear();

            for (int i = 0; i < 9; i++)
            {
                DataGridViewImageColumn imgColumn = new DataGridViewImageColumn();
                imgColumn.Width = 35;
                imgColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dataGridViewGameField.Columns.Add(imgColumn);
            }

            dataGridViewGameField.Rows.Add(9);

            for (int i = 0; i < 9; i++)
            {
                dataGridViewGameField.Rows[i].Height = 35;
            }

            dataGridViewGameField.Size = new Size(320, 320);

            NewGame();
        }
    }
}