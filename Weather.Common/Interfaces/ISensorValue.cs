using Weather.Units;

namespace Weather.Common.Interfaces
{
    public interface ISensorValue
    {
        long SensorValueId { get; set; }
        double? RawValue { get; set; }
        double? CorrectedValue { get; }
        Unit DisplayUnit { get; set; }
        double? DisplayValue { get; set; }
        ISensor Sensor { get; set; }
        IWeatherStation Station { get; set; }
        long StationId { get; set; }
        long SensorId { get; set; }
    }
}