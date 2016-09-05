using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.Core
{
    public class SelectedStation : NotifyBase, ISelectedStation
    {
        private IWeatherStation _weatherStation;

        public IWeatherStation WeatherStation
        {
            get { return _weatherStation; }
            set
            {
                _weatherStation = value;
                OnPropertyChanged(() => WeatherStation);
                OnSelectedStationChanged();
            }
        }

        private string _timeSpanWords;
        public string TimeSpanWords
        {
            get { return _timeSpanWords; }
            set { _timeSpanWords = value;  OnPropertyChanged(()=> TimeSpanWords); }
        }



        private bool _timeSpanDay;
        private bool _timeSpanWeek;
        private bool _timeSpanMonth;
        private bool _timeSpanYear;

        public bool TimeSpanDay
        {
            get { return _timeSpanDay; }
            set
            {
                _timeSpanDay = value;
                OnPropertyChanged(() => TimeSpanDay);
                TimeSpan = new TimeSpan(1,0,0,0,0); 
                TimeSpanWeek = false;
                TimeSpanMonth = false;
                TimeSpanYear = false;
                UpdateDates();
                TimeSpanWords = "Day";
                OnTimeSpanChanged();
            }
        }

        public bool TimeSpanWeek
        {
            get { return _timeSpanWeek; }
            set
            {
                _timeSpanWeek = value; OnPropertyChanged(() => TimeSpanWeek); TimeSpan = new TimeSpan(7, 0, 0, 0, 0);
                TimeSpanDay = false;
                TimeSpanMonth = false;
                TimeSpanYear = false;
                UpdateDates();
                TimeSpanWords = "Week";
                OnTimeSpanChanged();
            }
        }

        public bool TimeSpanMonth
        {
            get { return _timeSpanMonth; }
            set
            {
                _timeSpanMonth = value; OnPropertyChanged(() => TimeSpanMonth); TimeSpan = new TimeSpan(30, 0, 0, 0, 0); 
                TimeSpanWeek = false;
                _timeSpanDay = false;
                TimeSpanYear = false;
                UpdateDates();
                TimeSpanWords = "Month";
                OnTimeSpanChanged();
            }
        }

        public bool TimeSpanYear
        {
            get { return _timeSpanYear; }
            set
            {
                _timeSpanYear = value; OnPropertyChanged(() => TimeSpanYear); TimeSpan = new TimeSpan(365, 0, 0, 0, 0); 
                TimeSpanWeek = false;
                TimeSpanMonth = false;
                TimeSpanDay = false;
                UpdateDates();
                TimeSpanWords = "Year";
                OnTimeSpanChanged();
            }
        }

        private TimeSpan _timeSpan;
        public TimeSpan TimeSpan
        {
            get { return _timeSpan; }
            set { _timeSpan = value;  OnPropertyChanged(()=> TimeSpan); }
        }

        private DateTime _startDate;
        private DateTime _endDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value;  OnPropertyChanged(() => StartDate); }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(() => EndDate); }
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

        public SelectedStation()
        {
            TimeSpanWeek = true;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            UpdateDates();
        }

        public void BackOnePeriod()
        {
            StartDate = StartDate - TimeSpan;
            EndDate = EndDate - TimeSpan;
        }

        public void ForwardOnePeriod()
        {
            StartDate = StartDate + TimeSpan;
            EndDate = EndDate + TimeSpan;
        }

        private void UpdateDates()
        {
            EndDate = StartDate + TimeSpan;
        }
    }
}
