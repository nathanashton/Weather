using Weather.Common.Interfaces;
using Weather.Units;

namespace Weather.Common.Entities
{
    public class SensorValue : ISensorValue
    {
        public long SensorValueId { get; set; }

        public double? RawValue { get; set; }

        public double? CorrectedValue
        {
            get
            {
                //var sensor = Station.Sensors.FirstOrDefault(x => x.Sensor.SensorId == Sensor.SensorId);
                //if (sensor != null)
                //{
                //    return sensor.Correction + RawValue;
                //}
                return RawValue;
            }
        }

        public Unit DisplayUnit { get; set; }

        public double? DisplayValue { get; set; }

        public ISensor Sensor { get; set; }

        public IWeatherStation Station { get; set; }
        public long StationId { get; set; }
        public long SensorId { get; set; }

        public override string ToString()
        {
            return CorrectedValue.ToString();
        }
    }
}