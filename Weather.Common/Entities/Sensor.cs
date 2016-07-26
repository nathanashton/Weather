using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class Sensor : ISensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Enums.UnitType Type { get; set; }
        public virtual IWeatherStation Station { get; set; }
    }
}