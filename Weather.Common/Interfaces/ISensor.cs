using Weather.Common.Entities;

namespace Weather.Common.Interfaces
{
    public interface ISensor
    {
        int Id { get; set; }
        string Name { get; set; }
        Enums.UnitType Type { get; set; }
        IWeatherStation Station { get; set; }
    }
}