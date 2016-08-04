using System;
using System.Collections.Generic;
using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface IWeatherRecord
    {
        long Id { get; set; }
        DateTime TimeStamp { get; set; }
        ICollection<SensorValue> SensorValues { get; set; }
    }
}