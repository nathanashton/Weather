using System.Collections.Generic;

namespace Weather.Common.Interfaces
{
    public interface ISensor
    {
        int SensorId { get; set; }
        string Manufacturer { get; set; }
        string Model { get; set; }
        string Description { get; set; }
        ISensorType SensorType { get; set; }
        IList<ISensorValue> SensorValues { get; set; }
        bool IsValid { get; }
        string FullName { get; }
    }
}