using System.Windows;
using System.Windows.Input;

namespace Minesweeper.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new ViewModels.MainViewModel(){ Owner = this };
        }


        private void CloseApp_ButtonClicked(object sender, RoutedEventArgs e) => Close();
        private void WindowMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }
    }
}
