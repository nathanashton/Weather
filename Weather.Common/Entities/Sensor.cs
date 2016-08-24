using System.Collections.Generic;
using PropertyChanged;
using Weather.Common.Interfaces;
using Weather.Common.Units;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class Sensor : ISensor
    {
        public int SensorId { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Correction { get; set; } = 0;
        public ISensorType SensorType { get; set; }
        public IList<ISensorValue> SensorValues { get; set; } = new List<ISensorValue>();

        public override string ToString()
        {
            return Manufacturer + " " + Model;
        }
    }
}