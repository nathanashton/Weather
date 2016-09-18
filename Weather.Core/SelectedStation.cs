using System;
using Weather.Common;
using Weather.Common.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using System.Threading.Tasks;

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
                OnSelectedStationChanged();
                OnPropertyChanged(() => WeatherStation);
            }
        }

      
        public event EventHandler GetRecordsStarted;
        public event EventHandler GetRecordsCompleted;
        public event EventHandler SelectedStationUpdated;
        public event EventHandler SelectedStationChanged;

        public event EventHandlers.AsyncEventHandler SelectedStationRecordsUpdated;


        private DateTime? _endDate;
        private DateTime? _startDate;

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
                OnSelectedStationRecordsUpdated();
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
                OnSelectedStationRecordsUpdated();
            }
        }


        public void OnSelectedStationRecordsUpdated()
        {
            SelectedStationRecordsUpdated?.Invoke(this, null);
        }

        public void OnSelectedStationChanged()
        {
            SelectedStationChanged?.Invoke(this, null);
        }

        public void OnSelectedStationUpdated()
        {
            SelectedStationUpdated?.Invoke(this, null);
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