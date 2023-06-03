using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MineSweeper;

namespace MineSweeper
{
   
    public class Game
    {
        // Field declarations go here
        public enum GameDifficulty
        {
            Easy,
            Medium,
            Expert,
            Custom
        }
        // The current game difficulty
        private GameDifficulty currentDifficulty;


        // Event declaration
        public event Action<GameDifficulty> DifficultyChanged;

        // Properties and methods go here
                // Method to change game difficulty
        public void ChangeDifficulty(GameDifficulty newDifficulty)
        {
            if(currentDifficulty != newDifficulty)
            {
                currentDifficulty = newDifficulty;
                DifficultyChanged?.Invoke(newDifficulty);  // trigger the event
            }
        
        }


    }



    public partial class MainForm : Form
    {
        private Game game;

        private TextBox openGamesTextBox;
        private TextBox playerNameTextBox;
        private RadioButton easyRadioButton;
        private RadioButton mediumRadioButton;
        private RadioButton expertRadioButton;
        private RadioButton customRadioButton;
        private Button newGameButton;
        private Button endGamesButton;
        private MenuStrip menuStrip;
        private ToolStripMenuItem playMenuItem;
        private ToolStripMenuItem easyMenuItem;
        private ToolStripMenuItem mediumMenuItem;
        private ToolStripMenuItem expertMenuItem;
        private ToolStripMenuItem customMenuItem;
        private ToolStripMenuItem closeAllMenuItem;
        private ToolStripMenuItem exitMenuItem;

        public MainForm()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Initialize TextBoxes
            openGamesTextBox = new TextBox();
            openGamesTextBox.Location = new Point(20, 20);
            openGamesTextBox.ReadOnly = true;
            openGamesTextBox.Text = "0";
            Controls.Add(openGamesTextBox);

            playerNameTextBox = new TextBox();
            playerNameTextBox.Location = new Point(20, 50);
            Controls.Add(playerNameTextBox);

            // Initialize Radio Buttons
            easyRadioButton = new RadioButton();
            easyRadioButton.Text = "Easy";
            easyRadioButton.Location = new Point(20, 90);
            Controls.Add(easyRadioButton);

            mediumRadioButton = new RadioButton();
            mediumRadioButton.Text = "Medium";
            mediumRadioButton.Location = new Point(20, 120);
            mediumRadioButton.Checked = true;
            Controls.Add(mediumRadioButton);

            expertRadioButton = new RadioButton();
            expertRadioButton.Text = "Expert";
            expertRadioButton.Location = new Point(20, 150);
            Controls.Add(expertRadioButton);

            customRadioButton = new RadioButton();
            customRadioButton.Text = "Custom";
            customRadioButton.Location = new Point(20, 180);
            Controls.Add(customRadioButton);

            // Initialize Buttons
            newGameButton = new Button();
            newGameButton.Text = "Start New Game";
            newGameButton.Location = new Point(20, 220);
            newGameButton.Click += new EventHandler(Play);
            Controls.Add(newGameButton);

            endGamesButton = new Button();
            endGamesButton.Text = "End All Games";
            endGamesButton.Location = new Point(20, 250);
            Controls.Add(endGamesButton);

            // Initialize Menu Strip
            menuStrip = new MenuStrip();

            playMenuItem = new ToolStripMenuItem();
            playMenuItem.Text = "Play a new game";

            easyMenuItem = new ToolStripMenuItem();
            easyMenuItem.Text = "Easy";
            easyMenuItem.Click += new EventHandler(PlayEasy);

            mediumMenuItem = new ToolStripMenuItem();
            mediumMenuItem.Text = "Medium";
            mediumMenuItem.Click += new EventHandler(PlayMedium);

            expertMenuItem = new ToolStripMenuItem();
            expertMenuItem.Text = "Expert";
            expertMenuItem.Click += new EventHandler(PlayExpert);

            customMenuItem = new ToolStripMenuItem();
            customMenuItem.Text = "Custom";
            customMenuItem.Click += new EventHandler(PlayCustom);

            closeAllMenuItem = new ToolStripMenuItem();
            closeAllMenuItem.Text = "Close all games";
            closeAllMenuItem.Click += new EventHandler(CloseAllGames);

            exitMenuItem = new ToolStripMenuItem();
            exitMenuItem.Text = "Exit";
            exitMenuItem.Click += new EventHandler(Exit);

            playMenuItem.DropDownItems.Add(easyMenuItem);
            playMenuItem.DropDownItems.Add(mediumMenuItem);
            playMenuItem.DropDownItems.Add(expertMenuItem);
            playMenuItem.DropDownItems.Add(customMenuItem);

            menuStrip.Items.Add(playMenuItem);
            menuStrip.Items.Add(closeAllMenuItem);
            menuStrip.Items.Add(exitMenuItem);

            Controls.Add(menuStrip);
        }

        private void PlayEasy(object sender, EventArgs e)
        {
            easyRadioButton.Checked = true;
            Play(sender, e);
        }

        private void PlayMedium(object sender, EventArgs e)
        {
            mediumRadioButton.Checked = true;
            Play(sender, e);
        }

        private void PlayExpert(object sender, EventArgs e)
        {
            expertRadioButton.Checked = true;
            Play(sender, e);
        }

        private void PlayCustom(object sender, EventArgs e)
        {
            customRadioButton.Checked = true;

            using (CustomGameDialog dialog = new CustomGameDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    int row = dialog.Row;
                    int col = dialog.Column;
                    int mines = dialog.Mines;

                    // Handle custom game parameters here
                    // You can perform validation, set game difficulty, or any other necessary logic

                    int size = Math.Min(30, 1000 / Math.Max(row, col));
                    Form2 gameForm = new Form2(GetDifficultyLabel(), row, col, size, mines);
                    gameForm.Show();
                }
            }
        }

        private void Play(object sender, EventArgs e)
        {
            int row = 0, col = 0, mines = 0;

            if (easyRadioButton.Checked)
            {
                row = col = 9;
                mines = 10;
            }
            else if (mediumRadioButton.Checked)
            {
                row = col = 16;
                mines = 40;
            }
            else if (expertRadioButton.Checked)
            {
                row = 16;
                col = 30;
                mines = 99;
            }
            else if (customRadioButton.Checked)
            {
                using (CustomGameDialog dialog = new CustomGameDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        row = dialog.Row;
                        col = dialog.Column;
                        mines = dialog.Mines;

                        // Handle custom game parameters here
                        // You can perform validation, set game difficulty, or any other necessary logic

                        // Validate custom game parameters
                        if (row * col < 18 || mines > row * col / 2 || row <= 0 || col <= 0 || mines <= 0)
                        {
                            MessageBox.Show("Invalid custom game parameters. Please try again.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Set the game difficulty to custom
                        game.Difficulty = new CustomGameDifficulty(row, col, mines);
                    }
                    else
                    {
                        // User canceled the custom game dialog
                        return;
                    }
                }
            }
            else
            {
                // No game mode selected
                return;
            }

            int size = Math.Min(30, 1000 / Math.Max(row, col));
            Form2 gameForm = new Form2(GetDifficultyLabel(), row, col, size, mines);
            gameForm.Show();
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                // Assume radioButton1 represents an "Easy" difficulty
                this.game.SetDifficulty(GameDifficulty.Easy);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                // Assume radioButton1 represents an "Easy" difficulty
                this.game.SetDifficulty(GameDifficulty.Medium);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                // Assume radioButton1 represents an "Easy" difficulty
                this.game.SetDifficulty(GameDifficulty.Hard);
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                // Assume radioButton1 represents an "Easy" difficulty
                this.game.SetDifficulty(GameDifficulty.Custom);
            }
        }

        private void ShowCustomGameDialog()
        {
            var dialog = new CustomGameDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Assume these methods exist in your `Game` class
                this.game.SetRows(dialog.Row);
                this.game.SetColumns(dialog.Column);
                this.game.SetMines(dialog.Mines);
            }
        }



        private void CloseAllGames(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Form2)
                {
                    form.Close();
                }
            }
        }

        private void Exit(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void StartNewGame(object sender, EventArgs e)
        {
            int row = 0, col = 0, mines = 0;

            if (easyRadioButton.Checked)
            {
                row = col = 9;
                mines = 10;
            }
            else if (mediumRadioButton.Checked)
            {
                row = col = 16;
                mines = 40;
            }
            else if (expertRadioButton.Checked)
            {
                row = 16;
                col = 30;
                mines = 99;
            }
            else if (customRadioButton.Checked)
            {
                using (CustomGameDialog dialog = new CustomGameDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        int row = dialog.Row;
                        int col = dialog.Column;
                        int mines = dialog.Mines;

                        // Validate custom game parameters
                        if (row * col < 18 || mines > row * col / 2 || row <= 0 || col <= 0 || mines <= 0)
                        {
                            MessageBox.Show("Invalid custom game parameters. Please try again.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Set the game difficulty to custom
                        game.Difficulty = new CustomGameDifficulty(row, col, mines);
                    }
                }
            }
            else
            {
                return;
            }

            int size = Math.Min(30, 1000 / Math.Max(row, col));
            Form2 gameForm = new Form2(GetDifficultyLabel(), row, col, size, mines);
            gameForm.Show();
        }

        private string GetDifficultyLabel()
        {
            if (easyRadioButton.Checked)
            {
                return "Easy";
            }
            else if (mediumRadioButton.Checked)
            {
                return "Medium";
            }
            else if (expertRadioButton.Checked)
            {
                return "Expert";
            }
            else if (customRadioButton.Checked)
            {
                return "Custom";
            }
            else
            {
                return "";
            }
        }

        private void UpdateGameDifficulty(Difficulty difficulty)
        {
            if (difficulty == Difficulty.Easy)
            {
                 game.Difficulty = GameDifficulty.Easy;
            }
            else if (difficulty == Difficulty.Medium)
            {
                 game.Difficulty = GameDifficulty.Medium;    
            }
            else if (difficulty == Difficulty.Expert)
            {
                   game.Difficulty = GameDifficulty.Expert;
            }
            else if (difficulty == Difficulty.Custom)
            {
                 ShowCustomGameDialog();
            }
        }
        
      

        public GameDifficulty Difficulty 
        {
            get => _difficulty;
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    DifficultyChanged?.Invoke(_difficulty);
                }
            }
        }
      
        
        private GameDifficulty _difficulty;

        public Game(GameDifficulty difficulty)
        {
            _difficulty = difficulty;
        }

     }


}
