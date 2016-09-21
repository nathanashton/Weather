using System;
using Weather.Common;
using Weather.Common.Interfaces;

namespace Weather.Core
{
    public class SelectedStation : NotifyBase, ISelectedStation
    {
        private DateTime? _endDate;
        private DateTime? _startDate;


        private IWeatherStation _weatherStation;


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


        public event EventHandler GetRecordsStarted;
        public event EventHandler GetRecordsCompleted;
        public event EventHandler ChangesMadeToSelectedStation;
        public event EventHandler SelectedStationChanged;

        public event EventHandler RecordsUpdatedForSelectedStation;

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;

                if (EndDate == null)
                {
                    EndDate = StartDate + new TimeSpan(1, 0, 0, 0, 0);
                }
                if (StartDate > EndDate)
                {
                    StartDate = EndDate;
                }
                OnPropertyChanged(() => StartDate);
                OnRecordsUpdatedForSelectedStation();
            }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                if (StartDate == null)
                {
                    StartDate = StartDate - new TimeSpan(1, 0, 0, 0, 0);
                }
                if (EndDate < StartDate)
                {
                    EndDate = StartDate;
                }
                OnPropertyChanged(() => EndDate);
                OnRecordsUpdatedForSelectedStation();
            }
        }


        public void OnRecordsUpdatedForSelectedStation()
        {
            RecordsUpdatedForSelectedStation?.Invoke(this, null);
        }

        public void OnSelectedStationChanged()
        {
            SelectedStationChanged?.Invoke(this, null);
        }

        public void OnChangesMadeToSelectedStation()
        {
            ChangesMadeToSelectedStation?.Invoke(this, null);
        }

        public void OnGetRecordsStarted()
        {
            GetRecordsStarted?.Invoke(this, null);
        }

        public void OnGetRecordsCompleted()
        {
            GetRecordsCompleted?.Invoke(this, null);
        }
    }
}