using System;
using System.Collections.Generic;
using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface IWeatherRecord
    {
        int WeatherRecordId { get; set; }
        DateTime TimeStamp { get; set; }
        List<ISensorValue> SensorValues { get; set; }
        IWeatherStation WeatherStation { get; set; }
        int WeatherStationId { get; set; }
    }
}