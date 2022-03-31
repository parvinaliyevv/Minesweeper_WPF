using System.Windows;

namespace Minesweeper.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new ViewModels.MainViewModel();
        }


        private void CloseApplication_ButtonClicked(object sender, RoutedEventArgs e) => Close();
        private void WindowMove_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => this.DragMove();
    }
}
