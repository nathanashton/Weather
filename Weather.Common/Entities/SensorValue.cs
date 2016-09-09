using Weather.Common.Interfaces;
using Weather.Common.Units;

namespace Weather.Common.Entities
{
    public class SensorValue : ISensorValue
    {
        public int SensorValueId { get; set; }

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
        public int StationId { get; set; }
        public int SensorId { get; set; }

        public override string ToString()
        {
            return CorrectedValue.ToString();
        }
    }
}