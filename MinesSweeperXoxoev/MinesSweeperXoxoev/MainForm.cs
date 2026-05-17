using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesSweeperXoxoev
{
    public partial class MainForm : Form
    {
        Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();

        GameManager game;
        Button[,] buttons;

        private void LoadImages()
        {
            images["unknown"] = new Bitmap("C:\\Users\\79188\\source\\repos\\Minesweeper\\MinesSweeperXoxoev\\MinesSweeperXoxoev\\images\\unknown.png");

            images["1"] = new Bitmap("C:\\Users\\79188\\source\\repos\\Minesweeper\\MinesSweeperXoxoev\\MinesSweeperXoxoev\\images\\one.png");
            images["2"] = new Bitmap("C:\\Users\\79188\\source\\repos\\Minesweeper\\MinesSweeperXoxoev\\MinesSweeperXoxoev\\images\\two.png");
            images["3"] = new Bitmap("C:\\Users\\79188\\source\\repos\\Minesweeper\\MinesSweeperXoxoev\\MinesSweeperXoxoev\\images\\three.png");

            images["bomb"] = new Bitmap("C:\\Users\\79188\\source\\repos\\Minesweeper\\MinesSweeperXoxoev\\MinesSweeperXoxoev\\images\\bomb.png");

            images["flag"] = new Bitmap("C:\\Users\\79188\\source\\repos\\Minesweeper\\MinesSweeperXoxoev\\MinesSweeperXoxoev\\images\\flag.png");
        }

        public MainForm()
        {
            InitializeComponent();
            LoadImages();
            CreateBoard();
        }

        private void CreateBoard()
        {
            // удалить старые кнопки
            foreach (Control c in panel1.Controls)
            {
                c.Dispose();
            }

            panel1.Controls.Clear();

            // новая игра
            game = new GameManager();

            // новый массив кнопок
            buttons = new Button[game.Size, game.Size];

            for (int i = 0; i < game.Size; i++)
            {
                for (int j = 0; j < game.Size; j++)
                {
                    Button btn = new Button();

                    btn.Width = 40;
                    btn.Height = 40;

                    btn.Left = j * 40;
                    btn.Top = i * 40;

                    btn.Tag = (i, j);

                    btn.Image = images["unknown"];

                    btn.Text = "";

                    btn.MouseDown += Cell_MouseDown;
                    btn.Click += Cell_Click;

                    buttons[i, j] = btn;

                    panel1.Controls.Add(btn);
                }
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            var (x, y) = ((int, int))btn.Tag;

            Cell cell = game.Board[x, y];

            // нельзя открыть клетку с флагом
            if (cell.IsFlagged)
                return;

            // если уже открыта
            if (cell.IsOpen)
                return;

            // если мина
            if (cell.HasMine)
            {
                btn.Image = images["bomb"];

                RevealAllMines();

                ResultForm form =
                    new ResultForm("Вы проиграли!");

                form.ShowDialog();

                return;
            }

            // открыть клетку
            cell.IsOpen = true;

            // показать число
            if (cell.NeighborMines == 0)
            {
                btn.Image = images["unknown"];
            }
            else
            {
                btn.Image = images[cell.NeighborMines.ToString()];
            }

            btn.Enabled = false;
        }

        private void Cell_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;

            var (x, y) = ((int, int))btn.Tag;

            Cell cell = game.Board[x, y];

            // ПКМ
            if (e.Button == MouseButtons.Right)
            {
                // нельзя ставить флаг на открытую клетку
                if (cell.IsOpen)
                    return;

                // переключение флага
                cell.IsFlagged = !cell.IsFlagged;

                if (cell.IsFlagged)
                    btn.Image = images["flag"]; // флаг
                else
                    btn.Image = images["unknown"]; // пустая клетка
            }
        }

        private void RevealAllMines()
        {
            for (int i = 0; i < game.Size; i++)
            {
                for (int j = 0; j < game.Size; j++)
                {
                    if (game.Board[i, j].HasMine)
                    {
                        buttons[i, j].Image = images["bomb"];
                    }
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            CreateBoard();
        }
    }
}
