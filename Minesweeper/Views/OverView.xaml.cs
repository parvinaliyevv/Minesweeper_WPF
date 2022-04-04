using System.Windows;
using System.Windows.Input;
using Minesweeper.Models;

namespace Minesweeper.Views
{
    public partial class OverView : Window
    {
        public OverView(GameTime gameTime = null, GameTime bestTime = null)
        {
            InitializeComponent();

            DataContext = new ViewModels.OverViewModel(gameTime, bestTime);
        }


        private void CloseWindow_ButtonClicked(object sender, RoutedEventArgs e) => DialogResult = true;

        private void WindowMove_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
