using System;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    public class SelectedStation : NotifyBase, ISelectedStation
    {
        private DateTime _endDate;
        private DateTime _startDate;
        private TimeSpan _timeSpan;
        private bool _timeSpanDay;
        private bool _timeSpanMonth;
        private bool _timeSpanWeek;
        private string _timeSpanWords;
        private bool _timeSpanYear;
        private IWeatherStation _weatherStation;
        
        public SelectedStation()
        {
            SetTimeSpanWeek();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            UpdateDates();
        }
        
        public IWeatherStation WeatherStation
        {
            get { return _weatherStation; }
            set
            {
                _weatherStation = value;
                OnSelectedStationChanged();
                OnPropertyChanged(() => WeatherStation);
            }
        }

        public string TimeSpanWords
        {
            get { return _timeSpanWords; }
            set
            {
                _timeSpanWords = value;
                OnPropertyChanged(() => TimeSpanWords);
            }
        }

        public bool TimeSpanDay
        {
            get { return _timeSpanDay; }
            set
            {
                _timeSpanDay = value;
                OnPropertyChanged(() => TimeSpanDay);
            }
        }

        public bool TimeSpanWeek
        {
            get { return _timeSpanWeek; }
            set
            {
                _timeSpanWeek = value;
                OnPropertyChanged(() => TimeSpanWeek);
            }
        }

        public bool TimeSpanMonth
        {
            get { return _timeSpanMonth; }
            set
            {
                _timeSpanMonth = value;
                OnPropertyChanged(() => TimeSpanMonth);
            }
        }

        public bool TimeSpanYear
        {
            get { return _timeSpanYear; }
            set
            {
                _timeSpanYear = value;
                OnPropertyChanged(() => TimeSpanYear);
            }
        }

        public TimeSpan TimeSpan
        {
            get { return _timeSpan; }
            set
            {
                _timeSpan = value;
                OnPropertyChanged(() => TimeSpan);
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(() => StartDate);
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(() => EndDate);
            }
        }

        public event EventHandler StationsChanged;
        public event EventHandler TimeSpanChanged;
        public event EventHandler SelectedStationsChanged;


        public void OnStationsChanged()
        {
            StationsChanged?.Invoke(this, null);
        }

        public void OnSelectedStationChanged()
        {
            SelectedStationsChanged?.Invoke(this, null);
        }

        public void OnTimeSpanChanged()
        {
            TimeSpanChanged?.Invoke(this, null);
        }

        public void BackOnePeriod()
        {
            StartDate = StartDate - TimeSpan;
            EndDate = EndDate - TimeSpan;
            OnTimeSpanChanged();
        }

        public void ForwardOnePeriod()
        {
            StartDate = StartDate + TimeSpan;
            EndDate = EndDate + TimeSpan;
            OnTimeSpanChanged();
        }

        public void SetTimeSpanDay()
        {
            TimeSpan = new TimeSpan(1, 0, 0, 0, 0);
            TimeSpanDay = true;
            TimeSpanWeek = false;
            TimeSpanMonth = false;
            TimeSpanYear = false;
            TimeSpanWords = "Day";
            UpdateDates();
            OnTimeSpanChanged();
        }

        public void SetTimeSpanWeek()
        {
            TimeSpan = new TimeSpan(7, 0, 0, 0, 0);
            TimeSpanDay = false;
            TimeSpanWeek = true;
            TimeSpanMonth = false;
            TimeSpanYear = false;
            TimeSpanWords = "Week";
            UpdateDates();
            OnTimeSpanChanged();
        }

        public void SetTimeSpanMonth()
        {
            TimeSpan = new TimeSpan(31, 0, 0, 0, 0);
            TimeSpanDay = false;
            TimeSpanWeek = false;
            TimeSpanMonth = true;
            TimeSpanYear = false;
            TimeSpanWords = "Month";
            UpdateDates();
            OnTimeSpanChanged();
        }

        public void SetTimeSpanYear()
        {
            TimeSpan = new TimeSpan(365, 0, 0, 0, 0);
            TimeSpanDay = false;
            TimeSpanWeek = false;
            TimeSpanMonth = false;
            TimeSpanYear = true;
            TimeSpanWords = "Year";
            UpdateDates();
            OnTimeSpanChanged();
        }

        private void UpdateDates()
        {
            EndDate = StartDate + TimeSpan;
        }
    }
}