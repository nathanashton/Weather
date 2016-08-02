using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface ISensorValue
    {
        long Id { get; set; }
        ISensor Sensor { get; set; }
        double? RawValue { get; set; }
        double? CorrectedValue { get; }
        Unit DisplayUnit { get; set; }
        double? DisplayValue { get; set; }
        void SetNull();
    }
}