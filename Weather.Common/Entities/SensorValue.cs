using Weather.Common.Interfaces;
using Weather.Common.Units;

namespace Weather.Common.Entities
{
    public class SensorValue : ISensorValue
    {
        public int SensorValueId { get; set; }
        public double? RawValue { get; set; }
        public double? CorrectedValue { get; }
        public Unit DisplayUnit { get; set; }
        public double? DisplayValue { get; set; }
        public ISensor Sensor { get; set; }

        public override string ToString()
        {
            return RawValue + DisplayUnit.ToString();
        }
    }
}