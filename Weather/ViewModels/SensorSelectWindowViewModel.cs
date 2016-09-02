using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Weather.Common.Entities;
using Weather.Common.Interfaces;
using Weather.Core.Interfaces;
using Weather.Helpers;

namespace Weather.ViewModels
{
    [ImplementPropertyChanged]
    public class SensorSelectWindowViewModel
    {
        private readonly ISensorCore _sensorCore;

        public SensorSelectWindowViewModel(ISensorCore sensorCore)
        {
            _sensorCore = sensorCore;
            StationSensor = new StationSensor();
        }

        public bool Editing { get; set; }
        public Window Window { get; set; }
        public ObservableCollection<ISensor> Sensors { get; set; }
        public ISensor SelectedSensor { get; set; }
        public IStationSensor StationSensor { get; set; }

        public IWeatherStation WeatherStation { get; set; }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Save, x => SelectedSensor != null); }
        }

        public void GetAllSensors()
        {
            if (WeatherStation == null) return;
            var allSensors = _sensorCore.GetAllSensors();
            Sensors = new ObservableCollection<ISensor>(allSensors);
            SelectedSensor = Sensors.Count == 0 ? null : Sensors.First();
        }

        private void Save(object obj)
        {
            var sensor = new StationSensor { StationSensorId = StationSensor.StationSensorId, Sensor = SelectedSensor, Correction = StationSensor.Correction, Notes = StationSensor.Notes };
            if (!Editing)
            {
                WeatherStation.Sensors.Add(sensor);
            }
            else
            {
                StationSensor = sensor;
            }
            Window.Close();
        }
    }
}