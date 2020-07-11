using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TillApp.Source
{
    class Clock
    {
        private Label _timeLabel;
        private Label _dateLabel;
        DispatcherTimer liveTime; 

        public Clock(Label timeLabel, Label dateLabel)
        {
            _timeLabel = timeLabel;
            _dateLabel = dateLabel;
            liveTime = new DispatcherTimer();
        }

        public void StartClock()
        {
            if (!liveTime.IsEnabled)
            {
                liveTime.Interval = TimeSpan.FromSeconds(1);
                liveTime.Tick += timerTick;
                liveTime.Start();
            }
        }

        void timerTick(object sender, EventArgs e)
        {
            _timeLabel.Content = DateTime.Now.ToString("HH:mm:ss");
            _dateLabel.Content = DateTime.Now.ToString("dd.MM.yyyy");
        }

        public DateTime GetTime()
        {
            return DateTime.Now;
        }
    }
}
