using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Form2 : Form
    {
        private Button[][] buttons;
        private Field field;

        public Form2(string text, int row, int col, int size, int mines)
        {
            InitializeComponent();
            InitializeGameBoard(text, row, col, size, mines);
        }

        private void InitializeGameBoard(string text, int row, int col, int size, int mines)
        {
            this.Text = text;
            field = new Field(row, col, mines);
            this.ClientSize = new Size(col * size, row * size);
            buttons = new Button[row][];
            
            for (int i = 0; i < row; i++)
            {
                buttons[i] = new Button[col];
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    buttons[i][j] = new Button();
                    buttons[i][j].Text = "";
                    buttons[i][j].BackColor = Color.White;
                    buttons[i][j].Name = i + "," + j;
                    buttons[i][j].Size = new Size(size, size);
                    buttons[i][j].Location = new Point(size * j, size * i);
                    buttons[i][j].UseVisualStyleBackColor = false;
                    buttons[i][j].MouseUp += new MouseEventHandler(Button_Click);
                    this.Controls.Add(buttons[i][j]);
                }
            }
        }

        private void Button_Click(object sender, MouseEventArgs e)
        {
            Button button = (Button)sender;
            int temp = button.Name.IndexOf(",");
            int click_x = Convert.ToInt32(button.Name.Substring(0, temp));
            int click_y = Convert.ToInt32(button.Name.Substring(temp + 1));

            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (!field.Started)
                    {
                        field.Initialize(click_x, click_y);
                    }

                    if (field.IsMine(click_x, click_y))
                    {
                        button.BackColor = Color.Red;
                        MessageBox.Show("Game Over! You clicked on a mine!");
                    }
                    else if (!field.Discovered.Contains(click_x * buttons[0].Length + click_y))
                    {
                        foreach (int k in field.GetSafeIsland(click_x, click_y))
                        {
                            int i = k / buttons[0].Length;
                            int j = k % buttons[0].Length;
                            buttons[i][j].BackColor = Color.LightGray;
                            int m = field.CountMines(i, j);

                            if (m > 0)
                            {
                                buttons[i][j].Text = m.ToString();
                                buttons[i][j].BackColor = Color.LightBlue;
                            }
                            else
                            {
                                buttons[i][j].Enabled = false;
                            }
                        }

                        if (field.Win())
                        {
                            MessageBox.Show("Congratulations! You discovered all safe squares!");
                        }
                    }

                    break;

                case MouseButtons.Right:
                    if (field.Discovered.Contains(click_x * buttons[0].Length + click_y))
                    {
                        break;
                    }

                    if (field.Flagged.Contains(click_x * buttons[0].Length + click_y))
                    {
                        button.BackColor = Color.White;
                        field.Flagged.Remove(click_x * buttons[0].Length + click_y);
                    }
                    else
                    {
                        button.BackColor = Color.Green;
                        field.Flagged.Add(click_x * buttons[0].Length + click_y);
                    }

                    break;

                case MouseButtons.Middle:
                    if (!field.Discovered.Contains(click_x * buttons[0].Length + click_y))
                    {
                        break;
                    }

                    int flaggedCount = 0;
                    foreach (int k in field.GetNeighbors(click_x, click_y))
                    {
                        if (field.Flagged.Contains(k))
                        {
                            flaggedCount++;
                        }
                    }

                    if (field.CountMines(click_x, click_y) != flaggedCount)
                    {
                        break;
                    }

                    foreach (int k in field.GetNeighbors(click_x, click_y))
                    {
                        int i = k / buttons[0].Length;
                        int j = k % buttons[0].Length;

                        if (field.Flagged.Contains(k) || field.Discovered.Contains(k))
                        {
                            continue;
                        }

                        if (field.IsMine(i, j))
                        {
                            button.BackColor = Color.Red;
                            MessageBox.Show("Game Over! You clicked on a mine!");
                            break;
                        }

                        foreach (int l in field.GetSafeIsland(i, j))
                        {
                            int x = l / buttons[0].Length;
                            int y = l % buttons[0].Length;

                            buttons[x][y].BackColor = Color.LightGray;
                            int m = field.CountMines(x, y);

                            if (m > 0)
                            {
                                buttons[x][y].Text = m.ToString();
                                buttons[x][y].BackColor = Color.LightBlue;
                            }
                            else
                            {
                                buttons[x][y].Enabled = false;
                            }
                        }

                        if (field.Win())
                        {
                            MessageBox.Show("Congratulations! You discovered all safe squares!");
                        }
                    }

                    break;
            }
        }
    }
}
