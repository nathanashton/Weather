using System.Collections.Generic;
using System.Collections.ObjectModel;
using Weather.Common.Units;

namespace Weather.Common.Interfaces
{
    public interface ISensorType
    {
        int SensorTypeId { get; set; }
        string Name { get; set; }
        List<Unit> Units { get; set; }
    }
}