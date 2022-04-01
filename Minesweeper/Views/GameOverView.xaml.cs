using System.Windows;
using System.Windows.Input;

namespace Minesweeper.Views
{
    public partial class GameOverView : Window
    {
        public GameOverView(object dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;
        }


        private void CloseWindow_ButtonClicked(object sender, RoutedEventArgs e) => Close();

        private void WindowMove_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
