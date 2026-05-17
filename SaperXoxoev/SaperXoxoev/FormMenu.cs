using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaperXoxoev
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();

            comboBoxDifficulty.SelectedIndex = 0;
        }


        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            int bombs = 10;

            switch (comboBoxDifficulty.SelectedIndex)
            {
                case 0:
                    bombs = 10;
                    break;

                case 1:
                    bombs = 13;
                    break;

                case 2:
                    bombs = 16;
                    break;

                case 3:
                    bombs = 52;
                    break;
            }

            FormGame1 game = new FormGame1(bombs);

            game.Show();

            this.Hide();
        }
    }
}
