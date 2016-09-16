using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Weather.Common.Interfaces
{
    public interface IWeatherStation
    {
        long WeatherStationId { get; set; }
        string Manufacturer { get; set; }
        string Model { get; set; }
        double? Latitude { get; set; }
        double? Longitude { get; set; }
        string Description { get; set; }
        bool IsValid { get; }
        string ToString();
        ObservableCollection<IStationSensor> Sensors { get; set; }
        ObservableCollection<IWeatherRecord> Records { get; set; }
        string FullName { get; }
    }
}