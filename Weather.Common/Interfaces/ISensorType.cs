using Weather.Units.Interfaces;

namespace Weather.Common.Interfaces
{
    public interface ISensorType
    {
        int SensorTypeId { get; set; }
        string Name { get; set; }
        IUnitType UnitType { get; set; }
        bool IsValid { get; }
    }
}