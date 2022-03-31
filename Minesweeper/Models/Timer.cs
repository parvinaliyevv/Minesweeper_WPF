using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Minesweeper.Models
{
    public class Timer: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        public TimeSpan GameTime { get; set; } = new();
        public DispatcherTimer MyTimer { get; set; } = new();


        private string _viewTime = string.Empty;
        public string ViewTime
        {
            get => _viewTime;
            set { _viewTime = value; OnPropertyChanged(); }
        }


        public Timer()
        {
            MyTimer.Interval = TimeSpan.FromSeconds(1);
            MyTimer.Tick += Timer_Tick;
            MyTimer.Start();
        }


        public void ResetTimer()
        {
            _viewTime = string.Empty;
            GameTime = new();
            MyTimer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (GameTime.TotalSeconds >= 999)
            {
                MyTimer.Stop();
                return;
            }

            GameTime = GameTime.Add(TimeSpan.FromSeconds(1));
            ViewTime = string.Format("{0:000}", GameTime.TotalSeconds);
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
