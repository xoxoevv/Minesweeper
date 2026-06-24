using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
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
        private GameEngine engine;
        private const int FIELD_SIZE = 9;  // Размер поля

        // Изображения для клеток
        Image bomb, empty, flag, four, one, three, two, unknown;

        // Состояние игры
        bool defeat = false;
        int flagsCnt = 0;
        bool FirstClick = true;
        int bombsCount;
        int currentLevelBombs;  // Количество бомб на текущем уровне

        //  поля рекордов
        private System.Windows.Forms.Timer gameTimer;           // Таймер для отсчета времени
        private int seconds = 0;           // Текущее время
        private Label labelTime;           // Метка для отображения времени
        private Label labelRecord;         // Метка для отображения рекорда
        private RecordsData records;       // Данные рекордов

        public FormGame1(int bombs)
        {
            InitializeComponent();

            currentLevelBombs = bombs;  // Запоминаем уровень
            bombsCount = bombs;
            flagsCnt = bombs;

            engine = new GameEngine(bombsCount);

            // Загружаем рекорды
            records = RecordsManager.LoadRecords();

            // Создаем таймер
            gameTimer = new System.Windows.Forms.Timer();

            gameTimer.Interval = 1000;  // 1 секунда
            gameTimer.Tick += GameTimer_Tick;

            // Создаем метки для времени и рекорда
            CreateLabels();

            try
            {
                string path = Application.StartupPath + "\\images\\";
                bomb = Bitmap.FromFile(path + "bomb.png");
                empty = Bitmap.FromFile(path + "empty.png");
                flag = Bitmap.FromFile(path + "flag.png");
                four = Bitmap.FromFile(path + "four.png");
                one = Bitmap.FromFile(path + "one.png");
                three = Bitmap.FromFile(path + "three.png");
                two = Bitmap.FromFile(path + "two.png");
                unknown = Bitmap.FromFile(path + "unknown.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки изображений: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateRecordDisplay();
        }

        private void CreateLabels()
        {
            labelTime = new Label();
            labelTime.Text = "Время: 0с";
            labelTime.Location = new Point(10, 335);
            labelTime.AutoSize = true;
            labelTime.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(labelTime);

            // Метка для рекорда
            labelRecord = new Label();
            labelRecord.Text = "Рекорд: -";
            labelRecord.Location = new Point(150, 335);
            labelRecord.AutoSize = true;
            labelRecord.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.Controls.Add(labelRecord);

            this.Size = new Size(340, 420);
        }

        private void UpdateRecordDisplay()
        {
            int record = 0;
            string levelName = "";

            switch (currentLevelBombs)
            {
                case 10: record = records.EasyRecord; levelName = "Новичок"; break;
                case 13: record = records.MediumRecord; levelName = "Любитель"; break;
                case 16: record = records.HardRecord; levelName = "Профессионал"; break;
                case 52: record = records.ExpertRecord; levelName = "Особый"; break;
            }

            if (record > 0)
                labelRecord.Text = "Рекорд (" + levelName + "): " + record + "с";
            else
                labelRecord.Text = "Рекорд (" + levelName + "): еще нет";
        }

        private bool IsNewRecord(int time)
        {
            switch (currentLevelBombs)
            {
                case 10: return records.EasyRecord == 0 || time < records.EasyRecord;
                case 13: return records.MediumRecord == 0 || time < records.MediumRecord;
                case 16: return records.HardRecord == 0 || time < records.HardRecord;
                case 52: return records.ExpertRecord == 0 || time < records.ExpertRecord;
                default: return false;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            seconds++;
            labelTime.Text = "Время: " + seconds + "с";
        }

        private void CheckVictory()
        {
            int opened = 0;
            int safeCells = 0;

            for (int i = 0; i < FIELD_SIZE; i++)
            {
                for (int j = 0; j < FIELD_SIZE; j++)
                {
                    Cell cell = engine.field[i, j];

                    if (!cell.IsMine) safeCells++;      
                    if (cell.IsOpen && !cell.IsMine) opened++; 
                }
            }

            if (opened == safeCells)
            {
                gameTimer.Stop();

                records.TotalGames++;
                records.Wins++;
                RecordsManager.SaveRecords(records);

                bool isNewRecord = IsNewRecord(seconds);
                if (isNewRecord)
                {
                    RecordsManager.UpdateRecord(currentLevelBombs, seconds, records);
                    UpdateRecordDisplay();
                }

                ShowBombs();

                string message = "Вы выиграли за " + seconds + " секунд!";
                if (isNewRecord)
                {
                    message += "\n НОВЫЙ РЕКОРД! ";
                }
                message += "\n\nСыграть ещё?";

                DialogResult result = MessageBox.Show(message, "Победа", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

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
            for (int i = 0; i < FIELD_SIZE; i++)
            {
                for (int j = 0; j < FIELD_SIZE; j++)
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
            gameTimer.Stop();

            records.TotalGames++;
            records.Losses++;
            RecordsManager.SaveRecords(records);

            ShowBombs();

            DialogResult result = MessageBox.Show("💥 Вы проиграли!\n\nСыграть ещё?", "Поражение", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

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
            if (row < 0 || row >= FIELD_SIZE || column < 0 || column >= FIELD_SIZE)
                return;

            Cell cell = engine.field[row, column];

            if (cell.IsOpen)
                return;

            if (cell.IsFlag)
                return;

            int bombs = engine.CountBombsAround(row, column);

            if (bombs == 0)
            {
                dataGridViewGameField.Rows[row].Cells[column].Value = empty;
                cell.IsOpen = true;

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

            seconds = 0;
            labelTime.Text = "⏱ Время: 0с";
            gameTimer.Stop();

            labelCountFlag.Text = "🚩 Флажков: " + flagsCnt.ToString();

            engine = new GameEngine(bombsCount);

            for (int i = 0; i < FIELD_SIZE; i++)
            {
                for (int j = 0; j < FIELD_SIZE; j++)
                {
                    dataGridViewGameField.Rows[i].Cells[j].Value = unknown;
                }
            }

            UpdateRecordDisplay();
        }

        private void dataGridViewGameField_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (defeat)
                return;

            if (engine.field[e.RowIndex, e.ColumnIndex].IsOpen)
                return;

            if (engine.field[e.RowIndex, e.ColumnIndex].IsFlag)
                return;

            if (FirstClick)
            {
                engine.GenerateBombs(e.RowIndex, e.ColumnIndex);
                FirstClick = false;
                gameTimer.Start();
            }

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

            if (defeat)
                return;

            if (e.Button == MouseButtons.Right)
            {
                Cell cell = engine.field[e.RowIndex, e.ColumnIndex];
                if (cell.IsOpen)
                    return;

                if (!cell.IsFlag && flagsCnt > 0)
                {
                    dataGridViewGameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = flag;
                    cell.IsFlag = true;
                    flagsCnt--;
                    labelCountFlag.Text = "🚩 Флажков: " + flagsCnt.ToString();
                }
            }
        }
        private void dataGridViewGameField_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (defeat)
                return;

            if (e.Button == MouseButtons.Right)
            {
                Cell cell = engine.field[e.RowIndex, e.ColumnIndex];

                if (cell.IsFlag)
                {
                    dataGridViewGameField.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = unknown;
                    cell.IsFlag = false;
                    flagsCnt++;
                    labelCountFlag.Text = "🚩 Флажков: " + flagsCnt.ToString();
                }
            }
        }

        private void FormGame1_Load(object sender, EventArgs e)
        {
            dataGridViewGameField.Columns.Clear();

            for (int i = 0; i < FIELD_SIZE; i++)
            {
                DataGridViewImageColumn imgColumn = new DataGridViewImageColumn();
                imgColumn.Width = 35;
                imgColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dataGridViewGameField.Columns.Add(imgColumn);
            }

            dataGridViewGameField.Rows.Add(FIELD_SIZE);

            for (int i = 0; i < FIELD_SIZE; i++)
            {
                dataGridViewGameField.Rows[i].Height = 35;
            }

            dataGridViewGameField.Size = new Size(35 * FIELD_SIZE + 5, 35 * FIELD_SIZE + 5);

            NewGame();
        }

        private void FormGame1_FormClosed(object sender, FormClosedEventArgs e)
        {
            RecordsManager.SaveRecords(records);
            Application.Exit();
        }
    }
}