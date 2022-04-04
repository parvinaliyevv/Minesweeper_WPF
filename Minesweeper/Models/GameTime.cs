using System;
using System.ComponentModel;
using System.Windows.Threading;
using System.Runtime.CompilerServices;

namespace Minesweeper.Models
{
    public class GameTime: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        private DispatcherTimer Timer { get; set; }
        public TimeSpan ElapsedTime { get; set; }

        private string _viewTime;
        public string ViewTime
        {
            get => _viewTime;
            set { _viewTime = value; OnPropertyChanged(); }
        }


        public GameTime() => ResetTimer();


        public void ResetTimer()
        {
            Timer?.Stop();

            ElapsedTime = new();
            Timer = new();

            ViewTime = string.Format("{0:000}", 0);

            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += ElapsedTimer_Tick;
        }

        public void StopTimer() => Timer.Stop();

        public void StartTimer() => Timer.Start();

        private void ElapsedTimer_Tick(object? sender, EventArgs e)
        {
            if (ElapsedTime.TotalSeconds >= 999) Timer.Stop();

            ViewTime = string.Format("{0:000}", ElapsedTime.TotalSeconds);
            ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(1));
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
