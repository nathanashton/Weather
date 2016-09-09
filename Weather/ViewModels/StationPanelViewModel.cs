using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

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
                Dispatcher.CurrentDispatcher.InvokeAsync(async () => await GetRecords());
                OnPropertyChanged(() => Selected);
            }
        }

        public StationPanelViewModel(IStationCore stationCore, ISelectedStation selectedStation,
            IWeatherRecordCore weatherRecordCore)
        {
            _weatherRecordCore = weatherRecordCore;
            SelectedStation = selectedStation;
            _stationCore = stationCore;

            SelectedStation.SelectedStationRecordsUpdated += SelectedStationRecordsSelectedStationRecordsUpdated;
            SelectedStation.SelectedStationChanged += SelectedStation_SelectedStationChanged;
            SelectedStation.SelectedStationUpdated += SelectedStation_SelectedStationUpdated;
        }

        private void SelectedStation_SelectedStationUpdated(object sender, EventArgs e)
        {
            var id = SelectedStation.WeatherStation.WeatherStationId;
            GetAllStations();
            var found = Stations.FirstOrDefault(x => x.WeatherStationId == id);
            if (found != null)
            { Selected = found; }
        }

        private void SelectedStation_SelectedStationChanged(object sender, EventArgs e)
        {
            // Stations may have changed so update
           // GetAllStations();
        }

        private async void SelectedStationRecordsSelectedStationRecordsUpdated(object sender, EventArgs e)
        {
            await GetRecords();
        }

        private async Task GetRecords()
        {
            if (_selectedStation?.WeatherStation == null)
            {
                return;
            }
            SelectedStation.OnGetRecordsStarted();

            SelectedStation.WeatherStation.Records = new ObservableCollection<IWeatherRecord>(await
                _weatherRecordCore.GetAllRecordsForStationBetweenDates(
                    SelectedStation.WeatherStation.WeatherStationId, SelectedStation.StartDate,
                    SelectedStation.EndDate).ConfigureAwait(true));
            SelectedStation.OnGetRecordsCompleted();
        }

        public void GetAllStations()
        {
            Stations = new ObservableCollection<IWeatherStation>(_stationCore.GetAllStations());
        }
    }
}