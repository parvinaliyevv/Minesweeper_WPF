using System;
using System.ComponentModel;
using System.Windows.Threading;
using System.Runtime.CompilerServices;

namespace Minesweeper.Models
{
    public class GameTimer: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        public TimeSpan Time { get; set; }
        public DispatcherTimer Timer { get; set; }


        private string _viewTime;
        public string ViewTime
        {
            get => _viewTime;
            set { _viewTime = value; OnPropertyChanged(); }
        }


        public GameTimer()
        {
            Time = new();
            Timer = new();
            ViewTime = string.Format("{0:000}", 0);

            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += SecondTimer_Tick;
        }


        public void StopTimer()
        {
            Time = new();
            ViewTime = string.Format("{0:000}", 0);
            Timer.Stop();
        }

        public void StartTimer()
        {
            Timer.Start();
        }

        private void SecondTimer_Tick(object? sender, EventArgs e)
        {
            if (Time.TotalSeconds >= 999) Timer.Stop();

            ViewTime = string.Format("{0:000}", Time.TotalSeconds);
            Time = Time.Add(TimeSpan.FromSeconds(1));
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
