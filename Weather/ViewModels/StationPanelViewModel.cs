using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class StationPanelViewModel
    {
        public ObservableCollection<IWeatherStation> Stations { get; set; }
        public ISelectedStation SelectedStation { get; set; }

        private IStationCore _stationCore;

        public StationPanelViewModel(IStationCore stationCore, ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
            _stationCore = stationCore;
            SelectedStation.StationsChanged += SelectedStation_StationsChanged;
        }

        private void SelectedStation_StationsChanged(object sender, EventArgs e)
        {
            var id = SelectedStation.WeatherStation.WeatherStationId;
            GetAllStations();
            var s = Stations.FirstOrDefault(x => x.WeatherStationId == id);
            if (s != null)
            {
                SelectedStation.WeatherStation = s;
            }
            else
            {
                SelectedStation.WeatherStation = null;
            }
        }

        public void GetAllStations()
        {
            Stations = new ObservableCollection<IWeatherStation>(_stationCore.GetAllStations());
        }
    }
}