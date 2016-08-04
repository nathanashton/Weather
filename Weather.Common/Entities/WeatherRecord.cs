using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class WeatherRecord : IWeatherRecord
    {
        public long Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public virtual ICollection<SensorValue> SensorValues { get; set; } = new ObservableCollection<SensorValue>();
        public virtual WeatherStation Station { get; set; }
       
    }
}