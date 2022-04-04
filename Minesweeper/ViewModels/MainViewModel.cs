using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Minesweeper.Commands;
using Minesweeper.DB;
using Minesweeper.Models;
using Minesweeper.Services;
using Minesweeper.Views;

namespace Minesweeper.ViewModels
{
    public class MainViewModel : DependencyObject
    {
        // Properties
        public GameDetails Game { get; set; }

        public Models.Matrix Matrix
        {
            get { return (Models.Matrix)GetValue(MatrixProperty); }
            set { SetValue(MatrixProperty, value); }
        }
        public static readonly DependencyProperty MatrixProperty =
            DependencyProperty.Register("Matrix", typeof(Models.Matrix), typeof(MainViewModel));

        public int Height
        {
            get { return (int)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(int), typeof(MainViewModel));

        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(MainViewModel));

        public Window Owner { get; set; }


        // Commands
        public RelayCommand LeftClickCommand { get; set; }
        public RelayCommand RightClickCommand { get; set; }
        public RelayCommand DifficultyChangedCommand { get; set; }
        public RelayCommand ResetGameCommand { get; set; }
        public RelayCommand SoundOffCommand { get; set; }


        // Constructors
        public MainViewModel()
        {
            Game = new GameDetails();
            Matrix = new Models.Matrix(Database.GetMatrixTemplate(Game.Difficulty));

            LeftClickCommand = new RelayCommand(LeftClickButton);
            RightClickCommand = new RelayCommand(RightClickButton);
            ResetGameCommand = new RelayCommand(sender => ResetGame());
            DifficultyChangedCommand = new RelayCommand(delegate (object sender)
            {
                if (Game.Difficulty == GameDifficulty.Custom)
                {
                    var customEditDialog = new CustomEditView();
                    customEditDialog.Owner = Owner;

                    if (customEditDialog.ShowDialog() == false)
                    {
                        if (sender is ComboBox component)
                        {
                            component.SelectedIndex = 1;
                            Game.Difficulty = GameDifficulty.Medium;
                        }
                    }
                }

                ResetGame();
            });
            SoundOffCommand = new RelayCommand(delegate (object sender)
            {
                var img = new PackIcon();

                img.MinHeight = 30;
                img.MinWidth = 60;
                img.Foreground = Brushes.GhostWhite;

                img.Kind = (Game.SoundOff == true) ? PackIconKind.Music : PackIconKind.MusicOff;
                SoundService.StopAllSounds();
                Game.SoundOff = (Game.SoundOff == true) ? false : true;

                (sender as Button).Content = img;
            });

            MatrixService.FillMatrix(Matrix);
        }


        // Methods
        private void RightClickButton(object sender)
        {
            var component = sender as Button;
            int index = (Grid.GetRow(component) * Matrix.Columns) + Grid.GetColumn(component);

            if (Matrix.Cells[index].HasFlag)
            {
                Matrix.Cells[index].HasFlag = false;
                Matrix.FlagCount++;

                Matrix.Cells[index].Content = null;

                if (!Game.SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\UnFlag.wav");
            }
            else
            {
                Matrix.Cells[index].HasFlag = true;
                Matrix.FlagCount--;

                Matrix.Cells[index].Content = new Image() { Source = ImageService.GetImageFromThisPath(@"\Assets\Images\Flag.png") };

                if (!Game.SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\Flag.wav");
            }
        }
        private void LeftClickButton(object sender)
        {
            var component = sender as Button;
            int index = (Grid.GetRow(component) * Matrix.Columns) + Grid.GetColumn(component);

            if (Matrix.Cells[index].HasFlag)
            {
                if (!Game.SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\Wrong.wav");
                return;
            }
            else if (!Game.MinesInitialized)
            {
                MatrixService.SpawnMines(Matrix, index);

                Game.MinesInitialized = true;
                Game.Time.StartTimer();
            }

            if (Matrix.Cells[index].MineCount == 0 && Matrix.Cells[index].Mode == MatrixCellMode.EmptyCell) NextCell(index);
            else
            {
                if (Matrix.Cells[index].Mode == MatrixCellMode.BombCell)
                {
                    var mineImage = ImageService.GetImageFromThisPath(@"\Assets\Images\Mine.png");
                    var invalidImage = ImageService.GetImageFromThisPath(@"\Assets\Images\Invalid.png");

                    foreach (var item in Matrix.Cells)
                    {
                        if (item.Mode == MatrixCellMode.BombCell) item.Content = new Image() { Source = mineImage };
                        else if (item.Mode == MatrixCellMode.EmptyCell && item.HasFlag) item.Content = new Image() { Source = invalidImage };

                        item.IsEnable = false;
                    }

                    if (!Game.SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\Explode.wav");

                    Game.Time.StopTimer();

                    var gameOverDialog = new OverView(null, null);
                    gameOverDialog.Owner = Owner;

                    if (gameOverDialog.ShowDialog() == true) ResetGame();

                    return;
                }
                else
                {
                    var backgroundColor = (Matrix.Cells[index].Row + Matrix.Cells[index].Column) % 2 == 0 ? ColorService.GetColorFromHexCode("#FFE6D2BE") : ColorService.GetColorFromHexCode("#FFE0B890");
                    var foregroundColor = Database.GetNumericColor(Matrix.Cells[index].MineCount);

                    Matrix.Cells[index].Background = backgroundColor;

                    Matrix.Cells[index].Content = new TextBlock()
                    {
                        Text = (Matrix.Cells[index].MineCount == 0) ? string.Empty : Matrix.Cells[index].MineCount.ToString(),
                        TextAlignment = TextAlignment.Center,
                        Foreground = foregroundColor,
                    };

                    Matrix.Cells[index].IsEnable = false;

                    if (!Game.SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\Click.wav");
                }
            }

            if (MatrixService.IsGameWon(Matrix))
            {
                Game.Time.StopTimer();

                var bestTime = Database.GetBestTime(Game.Difficulty);

                if (Game.Time.ElapsedTime.TotalSeconds < bestTime.ElapsedTime.TotalSeconds || bestTime.ElapsedTime.TotalSeconds == 0)
                {
                    bestTime.ElapsedTime = TimeSpan.FromSeconds(Game.Time.ElapsedTime.TotalSeconds);
                    bestTime.ViewTime = Game.Time.ViewTime;
                }

                var gameWinDialog = new OverView(Game.Time, bestTime);
                gameWinDialog.Owner = Owner;

                if (gameWinDialog.ShowDialog() == true) ResetGame();
            }
        }

        public void ResetGame()
        {
            Matrix = new Models.Matrix(Database.GetMatrixTemplate(Game.Difficulty));
            MatrixService.FillMatrix(Matrix);

            Height = ((int)Game.Difficulty + 1) * 20;
            FontSize = 20 / ((int)Game.Difficulty + 1);

            Game.MinesInitialized = false;
            Game.Time.ResetTimer();
        }
        public void NextCell(int index)
        {
            int startIndex = index - Matrix.Columns - 1;
            int result = default;
            MatrixDetails cell = null;

            for (int i = (startIndex < 0 && (index / Matrix.Columns) == 0) ? 1 : 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result = startIndex + (i * Matrix.Columns) + j;

                    if (result < 0 || result >= Matrix.Rows * Matrix.Columns) continue;
                    else if (result / Matrix.Columns != (result - j + 1) / Matrix.Columns || result / Matrix.Columns != (result - j + 1) / Matrix.Columns) continue;

                    cell = Matrix.Cells[result];

                    if (cell.Mode == MatrixCellMode.BombCell || !cell.IsEnable) continue;

                    if (cell.HasFlag) cell.HasFlag = false;


                    var backgroundColor = (cell.Row + cell.Column) % 2 == 0 ? ColorService.GetColorFromHexCode("#FFE6D2BE") : ColorService.GetColorFromHexCode("#FFE0B890");
                    var foregroundColor = Database.GetNumericColor(cell.MineCount);

                    cell.Background = backgroundColor;

                    cell.Content = new TextBlock()
                    {
                        Text = (cell.MineCount == 0) ? string.Empty : cell.MineCount.ToString(),
                        TextAlignment = TextAlignment.Center,
                        Foreground = foregroundColor,
                    };

                    cell.IsEnable = false;


                    if (cell.MineCount == 0) NextCell(result);
                }
            }
        }
    }
}
