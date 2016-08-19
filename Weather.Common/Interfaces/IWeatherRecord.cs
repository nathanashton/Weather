using System;
using System.Collections.Generic;

namespace Weather.Common.Interfaces
{
    public interface IWeatherRecord
    {
        int WeatherRecordId { get; set; }
        DateTime TimeStamp { get; set; }
        IList<ISensorValue> SensorValues { get; set; }
        IWeatherStation WeatherStation { get; set; }
    }
}