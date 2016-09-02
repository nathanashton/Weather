using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    public class StationSensor : IStationSensor
    {
        public int StationSensorId { get; set; }
        public double? Correction { get; set; }
        public string Notes { get; set; }
        public ISensor Sensor { get; set; }

        public override string ToString()
        {
            return Sensor.ToString();
        }
    }
}