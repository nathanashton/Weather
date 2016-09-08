using PropertyChanged;
using System;
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
        public ObservableCollection<IWeatherStation> Stations { get; set; }

        private ISelectedStation _selectedStation;
        public ISelectedStation SelectedStation
        {
            get { return _selectedStation; }
            set
            {
                _selectedStation = value;
                OnPropertyChanged(() => SelectedStation);
            }
        }

        private readonly IWeatherRecordCore _weatherRecordCore;

        private IWeatherStation _selected;
        public IWeatherStation Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                SelectedStation.WeatherStation = value;
                Dispatcher.CurrentDispatcher.InvokeAsync(async () =>  await GetRecords());
                OnPropertyChanged(() => Selected);
            }
        }

        private void CurrentDispatcher_ShutdownFinished(object sender, EventArgs e)
        {
        }

        public StationPanelViewModel(IStationCore stationCore, ISelectedStation selectedStation, IWeatherRecordCore weatherRecordCore)
        {
            _weatherRecordCore = weatherRecordCore;
            SelectedStation = selectedStation;
            _stationCore = stationCore;

            SelectedStation.StationsChanged += SelectedStation_StationsChanged;
            SelectedStation.TimeSpanChanged += SelectedStation_TimeSpanChanged;
        }

        private async void SelectedStation_TimeSpanChanged(object sender, EventArgs e)
        {
            await GetRecords();
        }

        private async Task GetRecords()
        {
            if (_selectedStation?.WeatherStation == null) return;
            SelectedStation.OnGetRecordsStarted();

            var r = await _weatherRecordCore.GetAllRecordsForStationBetweenDates(SelectedStation.WeatherStation.WeatherStationId, SelectedStation.StartDate, SelectedStation.EndDate, Callback);
            SelectedStation.WeatherStation.Records = new ObservableCollection<IWeatherRecord>(r);
        }





        private void Callback()
        {
            SelectedStation.OnGetRecordsCompleted();
        }





        private async void SelectedStation_StationsChanged(object sender, EventArgs e)
        {
            if (SelectedStation.WeatherStation == null)
            {
                GetAllStations();
                return;
            }
            var id = SelectedStation.WeatherStation.WeatherStationId;
            GetAllStations();
            var s = Stations.FirstOrDefault(x => x.WeatherStationId == id);
            SelectedStation.WeatherStation = s;
            await GetRecords();
        }

        public void GetAllStations()
        {
            Stations = new ObservableCollection<IWeatherStation>(_stationCore.GetAllStations());
        }
    }


}