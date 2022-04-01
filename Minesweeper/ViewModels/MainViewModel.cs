using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;
using Minesweeper.Models;
using Minesweeper.Services;
using Minesweeper.Commands;
using Minesweeper.Views;

namespace Minesweeper.ViewModels
{
    public class MainViewModel : DependencyObject
    {
        // Properties
        public string GameDifficulty { get; set; }
        public bool SoundOff { get; set; }
        public int FontSize { get; set; }

        public ObservableCollection<MatrixCell> Matrix { get; set; }

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(MainViewModel));

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(MainViewModel));

        public int FlagCount
        {
            get { return (int)GetValue(FlagCountProperty); }
            set { SetValue(FlagCountProperty, value); }
        }
        public static readonly DependencyProperty FlagCountProperty =
            DependencyProperty.Register("FlagCount", typeof(int), typeof(MainViewModel));


        public GameTimer GameTime { get; set; }
        public GameTimer BestTime { get; set; }

        public GameTimer EasyBestTime { get; set; }
        public GameTimer MediumBestTime { get; set; }
        public GameTimer HardBestTime { get; set; }
        public GameTimer CustomBestTime { get; set; }
        
            
        public int MineCount { get; set; }

        public bool MinesInitialized { get; set; }

        public Window View { get; set; }


        // Commands
        public RelayCommand LeftClickCommand { get; set; }
        public RelayCommand RightClickCommand { get; set; }
        public RelayCommand DifficultyChangedCommand { get; set; }
        public RelayCommand ResetGameCommand { get; set; }
        public RelayCommand SoundOffCommand { get; set; }


        // Constructors
        public MainViewModel(Window owner)
        {
            GameDifficulty = GameDifficulties.Medium.ToString();

            View = owner;

            Matrix = new ObservableCollection<MatrixCell>();

            GameTime = new GameTimer();
            BestTime = new GameTimer();
            EasyBestTime = new GameTimer();
            MediumBestTime = new GameTimer();
            HardBestTime = new GameTimer();
            CustomBestTime = new GameTimer();

            LeftClickCommand = new RelayCommand(LeftClickButton);
            RightClickCommand = new RelayCommand(RightClickButton);
            DifficultyChangedCommand = new RelayCommand(obj => ResetGame());
            ResetGameCommand = new RelayCommand(obj => ResetGame());
            SoundOffCommand = new RelayCommand(delegate (object sender)
            {
                var img = new PackIcon();

                img.MinHeight = 30;
                img.MinWidth = 60;
                img.Foreground = Brushes.GhostWhite;

                img.Kind = (SoundOff == true) ? PackIconKind.Music : PackIconKind.MusicOff;
                SoundService.StopAllSounds();
                SoundOff = (SoundOff == true) ? false : true;

                (sender as Button).Content = img;
            });

            SoundOff = false;
            MinesInitialized = false;

            ResetGame();
        }


        // Methods
        public void FillMatrix()
        {
            SolidColorBrush color = null;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    color = (i + j) % 2 == 0 ? ColorService.GetColorFromString("#FFBAE86C") : ColorService.GetColorFromString("#FF9DC853");
                    Matrix.Add(new MatrixCell(i, j, color));
                }
            }
        }
        public void ResetGame()
        {
            if (GameDifficulty.Contains(GameDifficulties.Easy.ToString()))
            {
                Rows = 8;
                Columns = 10;
                MineCount = 10;
                FlagCount = MineCount;
                FontSize = 26;
            }
            else if (GameDifficulty.Contains(GameDifficulties.Medium.ToString()))
            {
                Rows = 14;
                Columns = 18;
                MineCount = 40;
                FlagCount = MineCount;
                FontSize = 20;
            }
            else if (GameDifficulty.Contains(GameDifficulties.Hard.ToString()))
            {
                Rows = 20;
                Columns = 24;
                MineCount = 99;
                FlagCount = MineCount;
                FontSize = 14;
            }
            else
            {
                Rows = RandomService.GetRandomNumber(8, 21);
                Columns = RandomService.GetRandomNumber(10, 25);
                MineCount = RandomService.GetRandomNumber(10, 100);
                FlagCount = MineCount;
            }

            MinesInitialized = false;
            GameTime.StopTimer();

            Matrix.Clear();

            FillMatrix();
        }

        private void RightClickButton(object sender)
        {
            var component = sender as Button;
            int index = (Grid.GetRow(component) * Columns) + Grid.GetColumn(component);

            if (Matrix[index].HasFlag)
            {
                Matrix[index].HasFlag = false;
                FlagCount++;

                Matrix[index].Content = null;

                if (!SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\UnFlag.wav");
            }
            else
            {
                Matrix[index].HasFlag = true;
                FlagCount--;

                Matrix[index].Content = new Image() { Source = ImageService.GetImageFromThisPath(@"\Assets\Images\Flag.png") };

                if (!SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\Flag.wav");
            }
        }
        private void LeftClickButton(object sender)
        {
            var component = sender as Button;
            int index = (Grid.GetRow(component) * Columns) + Grid.GetColumn(component);

            if (Matrix[index].HasFlag)
            {
                if (!SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\Wrong.wav");
                return;
            }
            else if (!MinesInitialized) InitializeMines(index);

            if (Matrix[index].MineCount == 0 && Matrix[index].Mode == MatrixCellMode.EmptyCell) NextCell(index);
            else
            {
                if (Matrix[index].Mode == MatrixCellMode.BombCell)
                {
                    var mineImage = ImageService.GetImageFromThisPath(@"\Assets\Images\Mine.png");
                    var invalidImage = ImageService.GetImageFromThisPath(@"\Assets\Images\Invalid.png");

                    foreach (var item in Matrix)
                    {
                        if (item.Mode == MatrixCellMode.BombCell) item.Content = new Image() { Source = mineImage };
                        else if (item.Mode == MatrixCellMode.EmptyCell && item.HasFlag) item.Content = new Image() { Source = invalidImage };

                        item.IsEnable = false;
                    }

                    if (!SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\Explode.wav");

                    GameTime.Timer.Stop();

                    var gameOverDialog = new GameOverView(this);
                    gameOverDialog.Owner = View;

                    gameOverDialog.ShowDialog();

                    return;
                }
                else
                {
                    var backgroundColor = (Matrix[index].Row + Matrix[index].Column) % 2 == 0 ? ColorService.GetColorFromString("#FFE6D2BE") : ColorService.GetColorFromString("#FFE0B890");
                    var foregroundColor = ColorService.GetNumericColor(Matrix[index].MineCount);

                    Matrix[index].Background = backgroundColor;

                    Matrix[index].Content = new TextBlock()
                    {
                        Text = (Matrix[index].MineCount == 0) ? string.Empty : Matrix[index].MineCount.ToString(),
                        TextAlignment = TextAlignment.Center,
                        Foreground = foregroundColor,
                        FontSize = FontSize
                    };

                    Matrix[index].IsEnable = false;

                    if (!SoundOff) SoundService.PlaySoundFromThisPath(@"\Assets\Sounds\Click.wav");
                }
            }

            if (CheckWin())
            {
                GameTime.Timer.Stop();

                if (GameDifficulty.ToString() == GameDifficulties.Easy.ToString())
                {
                    if (GameTime.Time.TotalSeconds < EasyBestTime.Time.TotalSeconds || EasyBestTime.Time.TotalSeconds == 0)
                    {
                        EasyBestTime.Time = TimeSpan.FromSeconds(GameTime.Time.TotalSeconds);
                        EasyBestTime.ViewTime = GameTime.ViewTime;
                    }

                    BestTime = EasyBestTime;
                }
                else if (GameDifficulty.ToString() == GameDifficulties.Medium.ToString())
                {
                    if (GameTime.Time.TotalSeconds < MediumBestTime.Time.TotalSeconds || MediumBestTime.Time.TotalSeconds == 0)
                    {
                        MediumBestTime.Time = TimeSpan.FromSeconds(GameTime.Time.TotalSeconds);
                        MediumBestTime.ViewTime = GameTime.ViewTime;
                    }

                    BestTime = MediumBestTime;
                }
                else if (GameDifficulty.ToString() == GameDifficulties.Hard.ToString())
                {
                    if (GameTime.Time.TotalSeconds < HardBestTime.Time.TotalSeconds || HardBestTime.Time.TotalSeconds == 0)
                    {
                        HardBestTime.Time = TimeSpan.FromSeconds(GameTime.Time.TotalSeconds);
                        HardBestTime.ViewTime = GameTime.ViewTime;
                    }

                    BestTime = HardBestTime;
                }
                else if (GameDifficulty.ToString() == GameDifficulties.Custom.ToString())
                {
                    if (GameTime.Time.TotalSeconds < CustomBestTime.Time.TotalSeconds || CustomBestTime.Time.TotalSeconds == 0)
                    {
                        CustomBestTime.Time = TimeSpan.FromSeconds(GameTime.Time.TotalSeconds);
                        CustomBestTime.ViewTime = GameTime.ViewTime;
                    }

                    BestTime = CustomBestTime;
                }

                var gameWinDialog = new GameWinView(this);
                gameWinDialog.Owner = View;

                gameWinDialog.ShowDialog();
            }
        }

        public void InitializeMines(int index)
        {
            var exceptions = new List<int>();
            var mainExcept = Convert.ToInt32(index);

            // Exceptions
            for (int i = 0, result = default, startIndex = mainExcept - Columns - 1; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result = startIndex + (i * Columns) + j;

                    if (result < 0 || result >= Rows * Columns) continue;
                    else exceptions.Add(result);
                }
            }

            // Spawn mines
            for (int i = 0, result = default; i < MineCount; i++)
            {
                result = RandomService.GetRandomNumber(0, Rows * Columns);

                if (result < 0 || result >= Rows * Columns) i--;
                else if (exceptions.Contains(result) || result >= mainExcept - Columns && result <= mainExcept + Columns || Matrix[result].Mode == MatrixCellMode.BombCell) i--;
                else Matrix[result].Mode = MatrixCellMode.BombCell;
            }

            // Calculate mine count
            for (int c = 0, startIndex = default; c < Rows * Columns; c++)
            {
                if (Matrix[c].Mode == MatrixCellMode.BombCell || Matrix[c].MineCount != -1) continue;
                Matrix[c].MineCount = 0;

                startIndex = c - Columns - 1;

                for (int i = 0, result = default; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        result = startIndex + (i * Columns) + j;

                        if (result == c || result < 0 || result >= Rows * Columns) continue;
                        else if (result / Columns != (result - j + 1) / Columns || result / Columns != (result - j + 1) / Columns) continue;
                        else if (Matrix[result].Mode == MatrixCellMode.BombCell) Matrix[c].MineCount++;
                    }
                }
            }

            MinesInitialized = true;
            GameTime.StartTimer();
        }
        public void NextCell(int index)
        {
            int startIndex = index - Columns - 1, result = default;
            MatrixCell cell = null;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result = startIndex + (i * Columns) + j;
                    
                    if (result < 0 || result >= Rows * Columns) continue;
                    else if (result / Columns != (result - j + 1) / Columns || result / Columns != (result - j + 1) / Columns) continue;

                    cell = Matrix[result];

                    if (cell.Mode == MatrixCellMode.BombCell || !cell.IsEnable) continue;

                    if (cell.HasFlag) cell.HasFlag = false;


                    var backgroundColor = (cell.Row + cell.Column) % 2 == 0 ? ColorService.GetColorFromString("#FFE6D2BE") : ColorService.GetColorFromString("#FFE0B890");
                    var foregroundColor = ColorService.GetNumericColor(cell.MineCount);

                    cell.Background = backgroundColor;

                    cell.Content = new TextBlock()
                    {
                        Text = (cell.MineCount == 0) ? string.Empty : cell.MineCount.ToString(),
                        TextAlignment = TextAlignment.Center,
                        Foreground = foregroundColor,
                        FontSize = FontSize
                    };

                    cell.IsEnable = false;


                    if (cell.MineCount == 0) NextCell(result);
                }
            }
        }

        public bool CheckWin()
        {
            foreach (var item in Matrix)
                if (item.Mode == MatrixCellMode.EmptyCell && item.IsEnable) return false;

            return true;
        }
    }
}
