using System;
using System.Collections.Generic;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class WeatherRecord : IWeatherRecord
    {
        public int WeatherRecordId { get; set; }
        public WeatherStation Station { get; set; }
        public DateTime TimeStamp { get; set; }
        public IList<ISensorValue> SensorValues { get; set; } = new List<ISensorValue>();
    }
}