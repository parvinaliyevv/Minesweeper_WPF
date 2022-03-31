using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Minesweeper.Models
{
    public class MatrixCell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        public MatrixCellMode Mode { get; set; }
        public int MineCount { get; set; }
        public bool HasFlag { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }


        private bool _isEnable;
        public bool IsEnable
        {
            get => _isEnable;
            set { _isEnable = value; OnPropertyChanged(); }
        }

        private UIElement? _content;
        public UIElement? Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(); }
        }

        private SolidColorBrush _background;
        public SolidColorBrush Background
        {
            get => _background;
            set { _background = value; OnPropertyChanged(); }
        }


        public MatrixCell(int row, int column, SolidColorBrush color)
        {
            Row = row;
            Column = column;
            Background = color;

            Mode = MatrixCellMode.EmptyCell;
            MineCount = -1;

            Content = null;
            
            IsEnable = true;
            HasFlag = false;
        }


        private void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
