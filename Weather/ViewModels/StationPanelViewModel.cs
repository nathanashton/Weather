using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using PropertyChanged;
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
            set { _selectedStation = value;
                OnPropertyChanged(()=> SelectedStation);
            }
        }

        private IWeatherRecordCore _weatherRecordCore;


        private IWeatherStation _selected;

        public IWeatherStation Selected
        {
            get { return _selected; }
            set { _selected = value;
                SelectedStation.WeatherStation = value;
                GetRecords();

                OnPropertyChanged(()=> Selected);
            }
        }



        public StationPanelViewModel(IStationCore stationCore, ISelectedStation selectedStation, IWeatherRecordCore weatherRecordCore)
        {
            _weatherRecordCore = weatherRecordCore;
            SelectedStation = selectedStation;
            _stationCore = stationCore;
            SelectedStation.StationsChanged += SelectedStation_StationsChanged;
            SelectedStation.SelectedStationsChanged += SelectedStation_SelectedStationsChanged;
        }

        private void GetRecords()
        {
            if (_selectedStation == null || _selectedStation.WeatherStation == null) return;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            _selectedStation.WeatherStation.Records =
                new ObservableCollection<IWeatherRecord>(
                    _weatherRecordCore.GetAllRecordsForStationBetweenDates(
                        SelectedStation.WeatherStation.WeatherStationId, SelectedStation.StartDate,
                        SelectedStation.EndDate));
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;
        }


        private void SelectedStation_StationsChanged(object sender, EventArgs e)
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
        }

        private void SelectedStation_SelectedStationsChanged(object sender, EventArgs e)
        {
        }

        public void GetAllStations()
        {
            Stations = new ObservableCollection<IWeatherStation>(_stationCore.GetAllStations());
        }
    }
}