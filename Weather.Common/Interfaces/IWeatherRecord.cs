using System;
using System.Collections.Generic;

namespace Weather.Common.Interfaces
{
    public interface IWeatherRecord
    {
        long Id { get; set; }
        DateTime TimeStamp { get; set; }
        ICollection<ISensorValue> SensorValues { get; set; }
    }
}