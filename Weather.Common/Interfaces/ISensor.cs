using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface ISensor
    {
        long Id { get; set; }
        string Name { get; set; }
        double Correction { get; set; }
        Enums.UnitType Type { get; set; }
        WeatherStation Station { get; set; }
    }
}