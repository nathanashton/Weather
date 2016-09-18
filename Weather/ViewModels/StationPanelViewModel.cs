using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.DependencyResolver;
using Weather.Helpers;
using Weather.Views;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class StationPanelViewModel : NotifyBase
    {

        private readonly IStationCore _stationCore;

        private readonly IWeatherRecordCore _weatherRecordCore;

        private IWeatherStation _selected;

        private ISelectedStation _selectedStation;

        private List<IWeatherRecord> all;

        public ObservableCollection<IWeatherStation> Stations { get; set; }

        public ISelectedStation SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;

                OnPropertyChanged(() => SelectedStation);
            }
        }
      
        public IWeatherStation Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                SelectedStation.WeatherStation = value;
                Dispatcher.CurrentDispatcher.InvokeAsync(async() => await GetRecords());

                OnPropertyChanged(() => Selected);
            }
        }

        public void Start()
        {
            Dispatcher.CurrentDispatcher.InvokeAsync(async () => await GetRecords());
        }

        public StationPanelViewModel(IStationCore stationCore, ISelectedStation selectedStation,
            IWeatherRecordCore weatherRecordCore)
        {
            _weatherRecordCore = weatherRecordCore;
            SelectedStation = selectedStation;
            _stationCore = stationCore;

            SelectedStation.SelectedStationRecordsUpdated += SelectedStation_SelectedStationRecordsUpdated1;
            SelectedStation.SelectedStationChanged += SelectedStation_SelectedStationChanged;
            SelectedStation.SelectedStationUpdated += SelectedStation_SelectedStationUpdated;
        }

        private async Task SelectedStation_SelectedStationRecordsUpdated1(object sender, EventArgs e)
        {
            await GetRecords();
        }

     
        private void SelectedStation_SelectedStationUpdated(object sender, EventArgs e)
        {
            long id = 0;
            if ((SelectedStation != null) && (SelectedStation.WeatherStation != null))
            {
                id = SelectedStation.WeatherStation.WeatherStationId;
            }
            GetAllStations();
            var found = Stations.FirstOrDefault(x => x.WeatherStationId == id);
            if (found != null)
            {
                Selected = found;
            }
        }

        private void SelectedStation_SelectedStationChanged(object sender, EventArgs e)
        {
            // Stations may have changed so update
            // GetAllStations();
            Selected = SelectedStation.WeatherStation;
        }



        public async Task GetRecords()
        {
            if (SelectedStation.StartDate == null || SelectedStation.EndDate == null)
            {
                return;
            }

            if (_selectedStation?.WeatherStation == null)
            {
                return;
            }

            SelectedStation.OnGetRecordsStarted();

            var s = SelectedStation.StartDate;
            var e = SelectedStation.EndDate;


            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += async delegate
            {
                SelectedStation.WeatherStation.Records = await _weatherRecordCore.GetAllRecordsForStationBetweenDates(
                      SelectedStation.WeatherStation.WeatherStationId, (DateTime)s, (DateTime)e);
            };
            backgroundWorker.RunWorkerCompleted += (sender, g) =>
            {
                SelectedStation.OnGetRecordsCompleted();
            };
            backgroundWorker.RunWorkerAsync();



          

        }

        public void GetAllStations()
        {
            Stations = new ObservableCollection<IWeatherStation>(_stationCore.GetAllStations());
            if (Stations.Count == 2)
            {
                Selected = Stations.First();
              
            }

       
        }

     
    }
}