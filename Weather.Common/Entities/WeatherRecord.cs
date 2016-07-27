using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class WeatherRecord : IWeatherRecord
    {

        public long Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public virtual ICollection<ISensorValue> SensorValues { get; set; } = new ObservableCollection<ISensorValue>();
        public virtual IWeatherStation Station { get; set; }
    }
}