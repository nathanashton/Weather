﻿using System;
using System.Collections.Generic;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class WeatherRecord : IWeatherRecord
    {
        public long WeatherRecordId { get; set; }
        public DateTime TimeStamp { get; set; }
        public IWeatherStation WeatherStation { get; set; }
        public long WeatherStationId { get; set; }
        public IEnumerable<ISensorValue> SensorValues { get; set; } = new HashSet<ISensorValue>();
    }
}