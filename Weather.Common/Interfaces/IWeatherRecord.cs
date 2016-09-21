using System;
using System.Collections.Generic;

namespace Weather.Common.Interfaces
{
    public interface IWeatherRecord
    {
        long WeatherRecordId { get; set; }
        DateTime TimeStamp { get; set; }
        IEnumerable<ISensorValue> SensorValues { get; set; }
        IWeatherStation WeatherStation { get; set; }
        long WeatherStationId { get; set; }
    }
}