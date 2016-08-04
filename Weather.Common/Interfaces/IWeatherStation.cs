using System.Collections.Generic;
using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface IWeatherStation
    {
        long Id { get; set; }
        string Manufacturer { get; set; }
        string Model { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        ICollection<WeatherRecord> WeatherRecords { get; set; }
        ICollection<Sensor> Sensors { get; set; }

        void AddSensor(Sensor sensor);

        void AddRecord(WeatherRecord record);
    }
}