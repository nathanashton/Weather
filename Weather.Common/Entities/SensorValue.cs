using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    public class SensorValue : ISensorValue
    {
        public long Id { get; set; }
        public double? RawValue { get; set; }
        public double? CorrectedValue { get; }
        public Unit DisplayUnit { get; set; }
        public double? DisplayValue { get; set; }

        public virtual Sensor Sensor { get; set; }

        public override string ToString()
        {
            return RawValue + DisplayUnit.ToString();
        }


    }
}