using System.Collections.Generic;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class Sensor : ISensor
    {
        public int SensorId { get; set; }
        public string Name { get; set; }
        public double Correction { get; set; } = 0;
        public Enums.UnitType Type { get; set; }
        public IList<ISensorValue> SensorValues { get; set; } = new List<ISensorValue>();

        public override string ToString()
        {
            return Name + " (" + Type + ")";
        }
    }
}