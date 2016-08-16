using System.Collections.Generic;
using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface IWeatherStation
    {
        int WeatherStationId { get; set; }
        string Manufacturer { get; set; }
        string Model { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        IList<IWeatherRecord> WeatherRecords { get; set; }
        IList<ISensor> Sensors { get; set; }

        void AddSensor(Sensor sensor);

        void AddRecord(WeatherRecord record);
    }
}