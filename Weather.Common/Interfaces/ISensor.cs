using System.Collections.Generic;
using Weather.Common.Entities;
using Weather.Common.Units;

namespace Weather.Common.Interfaces
{
    public interface ISensor
    {
        int SensorId { get; set; }
        string Manufacturer { get; set; }
        string Model { get; set; }
        double Correction { get; set; }
        string Description { get; set; }
        ISensorType SensorType { get; set; }
        IList<ISensorValue> SensorValues { get; set; }
    }
}