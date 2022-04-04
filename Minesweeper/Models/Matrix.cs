using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Minesweeper.Services;

namespace Minesweeper.Models
{
    public class Matrix: INotifyPropertyChanged
    {
        public const short minimumMineCount = 5;
        public const short maximumMineCount = 100;

        public const short minimumRows = 10;
        public const short maximumRows = 100;

        public const short minimumColumns = 10;
        public const short maximumColumns = 100;


        public event PropertyChangedEventHandler? PropertyChanged;


        public ObservableCollection<MatrixDetails> Cells { get; set; }

        private short _rows;
        public short Rows
        {
            get => _rows;
            set { _rows = value; OnPropertyChanged(); }
        }

        private short _columns;
        public short Columns
        {
            get => _columns;
            set { _columns = value; OnPropertyChanged(); }
        }

        private short _flagCount;
        public short FlagCount
        {
            get => _flagCount;
            set { _flagCount = value; OnPropertyChanged(); }
        }

        public short MineCount { get; set; }


        public Matrix(Matrix other)
        {
            Cells = new ObservableCollection<MatrixDetails>();

            Rows = other.Rows;
            Columns = other.Columns;
            FlagCount = other.FlagCount;
            MineCount = other.MineCount;

            MatrixService.FillMatrix(this);
        }
        public Matrix(int rows, int columns, int mineCount)
        {
            Cells = new ObservableCollection<MatrixDetails>();

            Rows = (short)rows;
            Columns = (short)columns;
            FlagCount = (short)mineCount;
            MineCount = (short)mineCount;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler is not null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
