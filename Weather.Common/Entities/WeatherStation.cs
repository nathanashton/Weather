using System.Collections.Generic;
using System.Collections.ObjectModel;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class WeatherStation : IWeatherStation
    {
        public long Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name => Manufacturer + " " + Model;
        public ObservableCollection<IWeatherRecord> WeatherRecords { get; set; } = new ObservableCollection<IWeatherRecord>();
        public ObservableCollection<ISensor> Sensors { get; set; } = new ObservableCollection<ISensor>();

        public void AddRecord(IWeatherRecord record)
        {
            WeatherRecords.Add(record);
            foreach (var value in record.SensorValues)
            {
                if (!Sensors.Contains(value.Sensor))
                {
                    Sensors.Add(value.Sensor);
                }
            }
        }

        public void AddSensor(ISensor sensor)
        {
            Sensors.Add(sensor);
        }

        public ICollection<IWeatherRecordSingleSensor> GetValuesForSensorType(Enums.UnitType sensorType)
        {
            var results = new ObservableCollection<IWeatherRecordSingleSensor>();
            foreach (var record in WeatherRecords)
            {
                foreach (var sensor in record.SensorValues)
                {
                    if (sensor.Sensor.Type == sensorType)
                    {
                        results.Add(new WeatherRecordSingleSensor {TimeStamp = record.TimeStamp, SensorValue = sensor});
                    }
                }
            }
            return new ObservableCollection<IWeatherRecordSingleSensor>(results);
        }

        public override string ToString()
        {
            return Manufacturer + " " + Model;
        }
    }
}