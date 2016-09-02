using System.Collections.Generic;
using Weather.Common.Units;

namespace Weather.Common.Interfaces
{
    public interface ISensorType
    {
        int SensorTypeId { get; set; }
        string Name { get; set; }
        IList<Unit> Units { get; set; }
        Unit SIUnit { get; set; }
        bool IsValid { get; }
    }
}