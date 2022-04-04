using System.Windows;
using System.Windows.Input;

namespace Minesweeper.Views
{
    public partial class CustomEditView : Window
    {
        public CustomEditView()
        {
            InitializeComponent();

            DataContext = new ViewModels.CustomEditViewModel() { View = this };
        }

        private void WindowMove_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
    }
}
