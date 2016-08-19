using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    public class SensorType : ISensorType
    {
        public int SensorTypeId { get; set; }
        public string Name { get; set; }
    }
}
