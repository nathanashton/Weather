using System.Collections.Generic;
using System.Collections.ObjectModel;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class Sensor : ISensor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Correction { get; set; }
        public Enums.UnitType Type { get; set; }

        public virtual WeatherStation Station { get; set; }
        public virtual ICollection<SensorValue> SensorValues { get; set; } = new ObservableCollection<SensorValue>();

        public override string ToString()
        {
            return Name + " (" + Type + ")";
        }
    }
}