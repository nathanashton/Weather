using System.Collections.Generic;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class WeatherStation : IWeatherStation
    {
        public int WeatherStationId { get; set; }
        public string Name => Manufacturer + " " + Model;
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public IList<IWeatherRecord> WeatherRecords { get; set; } = new List<IWeatherRecord>();
        public IList<ISensor> Sensors { get; set; } = new List<ISensor>();

        public void AddSensor(Sensor sensor)
        {
            Sensors.Add(sensor);
        }

        public void AddRecord(WeatherRecord record)
        {
            WeatherRecords.Add(record);
        }

        public override string ToString()
        {
            return Manufacturer + " " + Model;
        }
    }
}