using System.Windows;
using Minesweeper.ViewModels;

namespace Minesweeper.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            var viewModel = new MainViewModel();
            viewModel.VisualGrid = LogicalGrid;

            viewModel.Row = 35;
            viewModel.Column = 35;
            viewModel.BombCount = 99;
            viewModel.FlagCount = 99;

            viewModel.InitializeGame();

            DataContext = viewModel;
        }
    }
}
