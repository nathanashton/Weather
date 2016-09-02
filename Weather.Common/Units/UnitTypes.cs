using PropertyChanged;
using System.Collections.Generic;

namespace Weather.Common.Units
{
    public static class UnitTypes
    {
        public static List<UnitType> UnitsList { get; set; }

        static UnitTypes()
        {
            UnitsList = new List<UnitType>
            {
                new UnitType {Name = "Temperature"},
                new UnitType {Name = "Pressure"},
                new UnitType {Name = "Speed"},
                new UnitType {Name = "Precipitation"},
                new UnitType {Name = "Precipitation Rate"}
            };
        }
    }

    [ImplementPropertyChanged]
    public class UnitType
    {
        public string Name { get; set; }
    }
}