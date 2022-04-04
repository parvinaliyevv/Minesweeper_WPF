using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Minesweeper.Models
{
    public class MatrixDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int Row { get; set; }
        public int Column { get; set; }

        public MatrixCellMode Mode { get; set; }

        public bool HasFlag { get; set; }
        public int MineCount { get; set; }


        private bool _isEnable;
        public bool IsEnable
        {
            get => _isEnable;
            set { _isEnable = value; OnPropertyChanged(); }
        }

        private UIElement? _content;
        public UIElement? Content
        {
            get => _content; 
            set { _content = value; OnPropertyChanged(); }
        }

        private SolidColorBrush _background;
        public SolidColorBrush Background
        {
            get => _background;
            set { _background = value; OnPropertyChanged(); }
        }


        public MatrixDetails(int row, int column, SolidColorBrush color)
        {
            Row = row;
            Column = column;

            Mode = MatrixCellMode.EmptyCell;

            HasFlag = false;
            MineCount = -1;
            
            _isEnable = true;
            _content = null;
            _background = color;
        }


        private void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
