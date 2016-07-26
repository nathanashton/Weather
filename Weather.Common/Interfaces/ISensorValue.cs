using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface ISensorValue
    {
        ISensor Sensor { get; set; }
        double? Value { get; set; }
        Unit DisplayUnit { get; set; }
        double? DisplayValue { get; set; }
        void SetNull();
    }
}