using System;

namespace Weather.Common.Interfaces
{
    public interface IWeatherRecordSingleSensor
    {
        DateTime TimeStamp { get; set; }
        ISensorValue SensorValue { get; set; }
    }
}