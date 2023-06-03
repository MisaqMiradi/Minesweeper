using System;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class CustomGameDialog : Form
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public int Mines { get; private set; }

        public CustomGameDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Row = int.Parse(rowTextBox.Text);
                Column = int.Parse(columnTextBox.Text);
                Mines = int.Parse(minesTextBox.Text);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateInput()
        {
            if (!int.TryParse(rowTextBox.Text, out int row) || row <= 0)
            {
                MessageBox.Show("Invalid row value. Please enter a positive integer.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(columnTextBox.Text, out int column) || column <= 0)
            {
                MessageBox.Show("Invalid column value. Please enter a positive integer.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(minesTextBox.Text, out int mines) || mines <= 0)
            {
                MessageBox.Show("Invalid mines value. Please enter a positive integer.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (row * column < 18 || mines > row * column / 2)
            {
                MessageBox.Show("Invalid custom game parameters. Please try again.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
