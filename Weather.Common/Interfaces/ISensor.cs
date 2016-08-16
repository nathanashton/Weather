using System.Collections.Generic;
using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface ISensor
    {
        int SensorId { get; set; }
        string Name { get; set; }
        double Correction { get; set; }
        Enums.UnitType Type { get; set; }
        IList<ISensorValue> SensorValues { get; set; }
    }
}