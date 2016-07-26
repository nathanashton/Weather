using System;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    public class WeatherRecordSingleSensor : IWeatherRecordSingleSensor
    {
        public DateTime TimeStamp { get; set; }
        public ISensorValue SensorValue { get; set; }
    }
}