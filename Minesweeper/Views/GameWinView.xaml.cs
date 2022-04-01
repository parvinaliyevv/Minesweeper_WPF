using System.Windows;
using System.Windows.Input;

namespace Minesweeper.Views
{
    public partial class GameWinView : Window
    {
        public GameWinView(object dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;
        }


        private void CloseWindow_ButtonClicked(object sender, RoutedEventArgs e) => Close();

        private void WindowMove_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
