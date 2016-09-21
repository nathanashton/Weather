using System;
using System.Collections.ObjectModel;
using System.Linq;
using PropertyChanged;
using Weather.Common;
using Weather.Common.Interfaces;
using Weather.Units;

namespace Weather.Charts.WindPolar
{
    [ImplementPropertyChanged]
    public class WindPolarControlViewModel : NotifyBase, IChartViewModel
    {
        private IStationSensor _windDirectionSensor;
        private long _windDirectionSensorId;
        private long _windDirectionSensorIdSave;
        private IStationSensor _windSpeedSensor;

        private long _windSpeedSensorId;


        private long _windSpeedSensorIdSave;


        public ObservableCollection<IStationSensor> WindSpeedSensors { get; set; }
        public ObservableCollection<IStationSensor> WindDirectionSensors { get; set; }

        public IStationSensor WindSpeedSensor
        {
            get { return _windSpeedSensor; }
            set
            {
                if (value != null)
                {
                    _windSpeedSensorId = value.StationSensorId;
                }
                else
                {
                    _windSpeedSensorId = 0;
                }
                _windSpeedSensor = value;
                DrawChart();
                OnPropertyChanged(() => WindSpeedSensor);
            }
        }

        public IStationSensor WindDirectionSensor
        {
            get { return _windDirectionSensor; }
            set
            {
                _windDirectionSensorId = value?.StationSensorId ?? 0;
                _windDirectionSensor = value;
                DrawChart();
                OnPropertyChanged(() => WindDirectionSensor);
            }
        }

        public ObservableCollection<PlantData> Data { get; set; }

        public WindPolarControlViewModel(ISelectedStation selectedStation)
        {
            SelectedStation = selectedStation;
            GetSensors();
        }

        public string Header => "Wind Polar Chart";


        public ISelectedStation SelectedStation { get; set; }


        public void ChangesMadeToSelectedStation(object sender, EventArgs e)
        {
            GetSensors();
            DrawChart();
        }

        public void RecordsUpdatedForSelectedStation(object sender, EventArgs e)
        {
            if (SelectedStation?.WeatherStation != null)
            {
                WindSpeedSensor =
                    SelectedStation.WeatherStation.Sensors.FirstOrDefault(x => x.StationSensorId == _windSpeedSensorId);
                WindDirectionSensor =
                    SelectedStation.WeatherStation.Sensors.FirstOrDefault(
                        x => x.StationSensorId == _windDirectionSensorId);
            }
            DrawChart();
        }

        public void SelectedStation_GetRecordsCompleted(object sender, EventArgs e)
        {
            DrawChart();
        }

        public void SelectedStation_SelectedStationChanged(object sender, EventArgs e)
        {
            GetSensors();
            DrawChart();
        }

        public void DrawChart()
        {
            Data = new ObservableCollection<PlantData>
            {
                new PlantData {Direction = "N", Tree = 25},
                new PlantData {Direction = "NE", Tree = 15},
                new PlantData {Direction = "E", Tree = 30},
                new PlantData {Direction = "SE", Tree = 18},
                new PlantData {Direction = "S", Tree = 23},
                new PlantData {Direction = "SW", Tree = 19},
                new PlantData {Direction = "W", Tree = 5},
                new PlantData {Direction = "NW", Tree = 0}
            };
        }

        public bool OptionsOpened { get; set; }

        public event EventHandler ChartDone;

        public void OnChartDone()
        {
            throw new NotImplementedException();
        }

        public void SavePosition()
        {
            if (WindSpeedSensor != null)
            {
                _windSpeedSensorIdSave = WindSpeedSensor.StationSensorId;
            }
            if (WindDirectionSensor != null)
            {
                _windDirectionSensorIdSave = WindDirectionSensor.StationSensorId;
            }
        }

        public void LoadPosition()
        {
            if (_windSpeedSensorIdSave != 0)
            {
                WindSpeedSensor =
                    SelectedStation.WeatherStation.Sensors.FirstOrDefault(
                        x => x.StationSensorId == _windSpeedSensorIdSave);
            }

            if (_windDirectionSensorIdSave != 0)
            {
                WindDirectionSensor =
                    SelectedStation.WeatherStation.Sensors.FirstOrDefault(
                        x => x.StationSensorId == _windDirectionSensorIdSave);
            }
        }


        private void GetSensors()
        {
            var speed = SelectedStation.WeatherStation.Sensors.Where(
                x => x.Sensor.SensorType.UnitType == UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Velocity));

            WindSpeedSensors = new ObservableCollection<IStationSensor>(speed);

            var direction = SelectedStation.WeatherStation.Sensors.Where(
                x => x.Sensor.SensorType.UnitType == UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Direction));

            WindDirectionSensors = new ObservableCollection<IStationSensor>(direction);
        }
    }

    [ImplementPropertyChanged]
    public class PlantData
    {
        public string Direction { get; set; }
        public double Tree { get; set; }
    }
}