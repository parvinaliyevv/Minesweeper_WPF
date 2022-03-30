using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using Minesweeper.Models;
using Minesweeper.Services;
using System.IO;

namespace Minesweeper.ViewModels
{
    public class MainViewModel
    {
        // Properties
        public List<FlatnessCell> Flatness { get; set; }
        public List<int> BombCellIndexs { get; set; }
        public Grid VisualGrid { get; set; }

        public SoundPlayer Sound { get; set; }
        public bool SoundOff { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public int BombCount { get; set; }
        public int FlagCount { get; set; }

        public bool BombsInitialized { get; set; }


        // Constructors
        public MainViewModel()
        {
            Flatness = new List<FlatnessCell>();
            BombCellIndexs = new List<int>();
            Sound = new SoundPlayer();

            SoundOff = false;
            BombsInitialized = false;
        }


        // Methods
        public void InitializeGame()
        {
            Flatness.Clear();
            VisualGrid.Children.Clear();
            VisualGrid.RowDefinitions.Clear();
            VisualGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < Row * Column; i++) Flatness.Add(new FlatnessCell());

            for (int i = 0; i < Row; i++) VisualGrid.RowDefinitions.Add(new RowDefinition());
            for (int j = 0; j < Column; j++) VisualGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Button button = null;

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    button = new Button()
                    {
                        Background = ((i + j) % 2 == 0) ? new BrushConverter().ConvertFromString("#aed65e") as Brush : new BrushConverter().ConvertFromString("#89ab48") as Brush,
                        Template = VisualGrid.Resources["FlatnessCellStyle"] as ControlTemplate
                    };

                    button.Click += CellClicked;
                    button.MouseRightButtonDown += PlaceFlag;

                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);

                    VisualGrid.Children.Add(button);
                }
            }
        }

        private void PlaceFlag(object sender, MouseButtonEventArgs e)
        {
            var component = sender as Button;
            int index = (Grid.GetRow(component) * Column) + Grid.GetColumn(component);

            if (Flatness[index].HasFlag)
            {
                component.Content = null;
                Flatness[index].HasFlag = false;
                FlagCount++;

                Sound.Stream = new FileStream(DirectoryService.GetProjectParentFolder() + @"\Assets\Sounds\UnFlag.wav", FileMode.Open, FileAccess.Read);

                Sound.Play();
                Sound.Stream.Close();
            }
            else
            {
                if (component.HasContent)
                {
                    Sound.Stream = new FileStream(DirectoryService.GetProjectParentFolder() + @"\Assets\Sounds\Wrong.wav", FileMode.Open, FileAccess.Read);

                    Sound.Play();
                    Sound.Stream.Close();
                }
                else
                {
                    component.Content = new Image() { Source = new ImageSourceConverter().ConvertFromString(DirectoryService.GetProjectParentFolder() + @"\Assets\Images\Flag.ico") as ImageSource };
                    Flatness[index].HasFlag = true;
                    FlagCount--;

                    Sound.Stream = new FileStream(DirectoryService.GetProjectParentFolder() + @"\Assets\Sounds\Flag.wav", FileMode.Open, FileAccess.Read);

                    Sound.Play();
                    Sound.Stream.Close();
                }
            }
        }

        private void CellClicked(object sender, RoutedEventArgs e)
        {
            var component = sender as Button;
            int index = (Grid.GetRow(component) * Column) + Grid.GetColumn(component);

            if (Flatness[index].HasFlag)
            {
                Sound.Stream = new FileStream(DirectoryService.GetProjectParentFolder() + @"\Assets\Sounds\Wrong.wav", FileMode.Open, FileAccess.Read);

                Sound.Play();
                Sound.Stream.Close();

                return;
            }

            if (!BombsInitialized) InitializeBombs(index);

            if (Flatness[index].BombCount == 0 && Flatness[index].Mode == FlatnessCellMode.EmptyCell && !(VisualGrid.Children[index] as ContentControl).HasContent) Recursion(index);
            else
            {
                if (Flatness[index].Mode == FlatnessCellMode.BombCell)
                {
                    foreach (var item in BombCellIndexs)
                    {
                        var btn = VisualGrid.Children[item] as Button;
                        btn.IsEnabled = false;

                        btn.Content = new Image() { Source = new ImageSourceConverter().ConvertFromString(DirectoryService.GetProjectParentFolder() + @"\Assets\Images\Mine.png") as ImageSource };
                    }

                    Sound.Stream = new FileStream(DirectoryService.GetProjectParentFolder() + @"\Assets\Sounds\Explode.wav", FileMode.Open, FileAccess.Read);

                    Sound.Play();
                    Sound.Stream.Close();
                }
                else
                {
                    component.Content = new TextBlock()
                    {
                        FontSize = 24,
                        Foreground = Brushes.Blue,
                        TextAlignment = TextAlignment.Center,
                        Text = Flatness[index].BombCount.ToString()
                    };
                    component.IsEnabled = false;

                    Sound.Stream = new FileStream(DirectoryService.GetProjectParentFolder() + @"\Assets\Sounds\Click.wav", FileMode.Open, FileAccess.Read);

                    Sound.Play();
                    Sound.Stream.Close();
                }
            }
        }

        public void InitializeBombs(int index)
        {
            var mainExcept = Convert.ToInt32(index);
            var exceptions = new List<int>();
            var random = new Random();

            for (int i = 0, result = default, startIndex = mainExcept - Column - 1; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result = startIndex + (i * Column) + j;

                    if (result < 0 || result >= Row * Column) continue;
                    else exceptions.Add(result);
                }
            }

            for (int i = 0, result = default; i < BombCount; i++)
            {
                result = random.Next(Row * Column);

                if (exceptions.Contains(result) || result >= mainExcept - Column && result <= mainExcept + Column || Flatness[result].Mode == FlatnessCellMode.BombCell) i--;
                else
                {
                    Flatness[result].Mode = FlatnessCellMode.BombCell;
                    BombCellIndexs.Add(result);
                }
            }

            BombsInitialized = true;

            CheckFlatness();
        }

        public void CheckFlatness()
        {
            for (int c = 0, startIndex = default; c < Row * Column; c++)
            {
                if (Flatness[c].Mode == FlatnessCellMode.BombCell || Flatness[c].BombCount != -1) continue;
                Flatness[c].BombCount = 0;

                startIndex = c - Column - 1;

                for (int i = 0, result = default; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (c % Column == Column - 1 && j == 2 || c % Column == 0 && j == 0) continue;

                        result = startIndex + (i * Column) + j;

                        if (result == c || result < 0 || result >= Row * Column) continue;
                        else if (Flatness[result].Mode == FlatnessCellMode.BombCell) Flatness[c].BombCount++;
                    }
                }
            }
        }

        public void Recursion(int index)
        {
            var collection = new List<int>();
            var startIndex = index - Column - 1;
            Button button = null;

            for (int i = 0, result = default; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (index % Column == Column - 1 && j == 2 || index % Column == 0 && j == 0) continue;

                    result = startIndex + (i * Column) + j;

                    if (result < 0 || result >= Row * Column) continue;

                    button = VisualGrid.Children[result] as Button;

                    if (Flatness[result].Mode == FlatnessCellMode.BombCell || button.HasContent && !Flatness[result].HasFlag) continue;

                    if (Flatness[result].HasFlag) Flatness[result].HasFlag = false;

                    button.Content = new TextBlock()
                    {
                        Text = Flatness[result].BombCount.ToString(),
                        Foreground = Brushes.Blue,
                        FontSize = 24
                    };

                    button.IsEnabled = false;

                    if (Flatness[result].BombCount == 0) Recursion(result);
                }
            }
        }
    }
}
