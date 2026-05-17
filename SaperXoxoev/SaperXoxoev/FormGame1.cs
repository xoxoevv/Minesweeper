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


        Image bomb, empty, flag, four, one, three, two, unknown;

        GameEngine engine;

        int[,] fieldFlags = new int[9, 9];

        bool defeat = false;

        int flagsCnt = 0;

        bool FirstClick = true;

        int bombsCount;


        public FormGame1(int bombs)
        {
            InitializeComponent();

            bombsCount = bombs;

            flagsCnt = bombs;

            engine = new GameEngine(bombs);

            bomb = Bitmap.FromFile("images/bomb.png");
            empty = Bitmap.FromFile("images/empty.png");
            flag = Bitmap.FromFile("images/flag.png");
            four = Bitmap.FromFile("images/four.png");
            one = Bitmap.FromFile("images/one.png");
            three = Bitmap.FromFile("images/three.png");
            two = Bitmap.FromFile("images/two.png");
            unknown = Bitmap.FromFile("images/unknown.png");
        }


        public FormGame1()
        {
            InitializeComponent();
        }


        private void CheckVictory()
        {
            int opened = 0;

            int safeCells = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (engine.field[i, j] == 0)
                    {
                        safeCells++;
                    }

                    if (dataGridViewGameField.Rows[i].Cells[j].Value != unknown
                        && engine.field[i, j] == 0)
                    {
                        opened++;
                    }
                }
            }

            if (opened == safeCells)
            {
                ShowBombs();

                DialogResult result =
                    MessageBox.Show(
                        "Вы выиграли! Сыграть ещё?",
                        "Победа",
                        MessageBoxButtons.YesNo);

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
                    if (engine.field[i, j] == 1)
                    {
                        dataGridViewGameField.Rows[i].Cells[j].Value = bomb;
                    }
                }
            }
        }



        public void Defeat()
        {
            ShowBombs();

            DialogResult result =
                MessageBox.Show(
                    "Вы проиграли. Сыграть ещё?",
                    "Поражение",
                    MessageBoxButtons.YesNo);

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
            if (row < 0 || row >= 9 || column < 0 || column >= 9)
                return;

            if (dataGridViewGameField.Rows[row].Cells[column].Value != unknown)
                return;

            if (fieldFlags[row, column] == 1)
                return;

            int bombs = engine.CountBombsAround(row, column);

            if (bombs == 0)
            {
                dataGridViewGameField.Rows[row].Cells[column].Value = empty;

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

            labelCountFlag.Text = flagsCnt.ToString();

            engine = new GameEngine(bombsCount);

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    fieldFlags[i, j] = 0;

                    dataGridViewGameField.Rows[i].Cells[j].Value = unknown;
                }
            }
        }


        private void dataGridViewGameField_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (FirstClick)
            {
                engine.GenerateBombs(e.RowIndex, e.ColumnIndex);

                FirstClick = false;
            }

            if (engine.field[e.RowIndex, e.ColumnIndex] == 1)
            {
                dataGridViewGameField.Rows[e.RowIndex]
                    .Cells[e.ColumnIndex]
                    .Value = bomb;

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

            if (e.Button == MouseButtons.Right)
            {
                if (fieldFlags[e.RowIndex, e.ColumnIndex] == 0
                    && flagsCnt > 0)
                {
                    dataGridViewGameField.Rows[e.RowIndex]
                        .Cells[e.ColumnIndex]
                        .Value = flag;

                    fieldFlags[e.RowIndex, e.ColumnIndex] = 1;

                    flagsCnt--;

                    labelCountFlag.Text = flagsCnt.ToString();
                }
            }

        }

        private void dataGridViewGameField_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.Button == MouseButtons.Right)
            {
                if (fieldFlags[e.RowIndex, e.ColumnIndex] == 1)
                {
                    dataGridViewGameField.Rows[e.RowIndex]
                        .Cells[e.ColumnIndex]
                        .Value = unknown;

                    fieldFlags[e.RowIndex, e.ColumnIndex] = 0;

                    flagsCnt++;

                    labelCountFlag.Text = flagsCnt.ToString();
                }
            }

        }

        private void FormGame1_Load(object sender, EventArgs e)
        {
            dataGridViewGameField.Columns.Clear();

            for (int i = 0; i < 9; i++)
            {
                DataGridViewImageColumn imgColumn = new DataGridViewImageColumn();

                imgColumn.Width = 35;  //  ширина клетки

                imgColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;

                dataGridViewGameField.Columns.Add(imgColumn);
            }

            dataGridViewGameField.Rows.Add(9);

            // Настройка строк
            for (int i = 0; i < 9; i++)
            {
                dataGridViewGameField.Rows[i].Height = 35;  //  высота клетки

            }

            dataGridViewGameField.Size = new Size(320, 320);

            NewGame();
        }
    }
}
