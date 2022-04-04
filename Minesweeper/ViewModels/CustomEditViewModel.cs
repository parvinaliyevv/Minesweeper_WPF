using System.Windows;
using Minesweeper.Commands;
using Minesweeper.DB;
using Minesweeper.Models;

namespace Minesweeper.ViewModels
{
    public class CustomEditViewModel
    {
        // Properties
        public Matrix CustomMatrix { get; set; }

        public Window View { get; set; }


        // Commands
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }


        public CustomEditViewModel()
        {
            CustomMatrix = new Matrix(Database.GetMatrixTemplate(GameDifficulty.Custom));

            SaveCommand = new RelayCommand(sender => SaveCustomMatrix());
            CancelCommand = new RelayCommand(sender => View.DialogResult = false);
        }


        public void SaveCustomMatrix()
        {
            if (CustomMatrix.Rows < Matrix.minimumRows || CustomMatrix.Rows > Matrix.maximumRows)
                { MessageBox.Show(string.Format("Mine Count Range: ({0}-{1})", Matrix.minimumRows, Matrix.maximumRows), "Invalid Row Count", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            else if (CustomMatrix.Columns < Matrix.minimumColumns || CustomMatrix.Columns > Matrix.maximumColumns)
                { MessageBox.Show(string.Format("Mine Count Range: ({0}-{1})", Matrix.minimumColumns, Matrix.maximumColumns), "Invalid Column Count", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            else if (CustomMatrix.MineCount < Matrix.minimumMineCount || CustomMatrix.MineCount > Matrix.maximumMineCount)
                { MessageBox.Show(string.Format("Mine Count Range: ({0}-{1})", Matrix.minimumMineCount, Matrix.maximumMineCount), "Invalid Mine Count", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            else if (CustomMatrix.MineCount > CustomMatrix.Rows * CustomMatrix.Columns / 2)
                { MessageBox.Show(string.Format("The number of mines cannot be more than half the product of a row and a column.", Matrix.minimumMineCount, Matrix.maximumMineCount), "Invalid Mine Count", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            var customMatrix = Database.GetMatrixTemplate(GameDifficulty.Custom);

            customMatrix.Rows = CustomMatrix.Rows;
            customMatrix.Columns = CustomMatrix.Columns;
            customMatrix.MineCount = CustomMatrix.MineCount;
            customMatrix.FlagCount = customMatrix.MineCount;

            View.DialogResult = true;
        }
    }
}
