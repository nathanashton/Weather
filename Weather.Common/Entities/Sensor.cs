using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities
{
    [ImplementPropertyChanged]
    public class Sensor : ISensor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Correction { get; set; }
        public Enums.UnitType Type { get; set; }
        public IWeatherStation Station { get; set; }
        public override string ToString()
        {
            if (Correction == 0)
            {
                return Name + " (" + Type + ")";
            }
            if (Correction > 0)
            {
                return Name + " (" + Type + ")" + " +" + Correction;
            } if (Correction <= 0)
            {
                return Name + " (" + Type + ")" + " " + Correction;
            }
            return Name + " (" + Type + ")" + " " + Correction;
        }
    }
}