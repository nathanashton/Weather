﻿using System.Collections.Generic;
using PropertyChanged;

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
                new UnitType {Name = "Wind Direction"},
                new UnitType {Name = "Precipitation"},
                new UnitType {Name = "Precipitation Rate"},
                new UnitType {Name = "Solar Radiation"},
                new UnitType {Name = "UV Index"},
                new UnitType {Name = "Leaf Wetness"}
            };
        }
    }

    [ImplementPropertyChanged]
    public class UnitType
    {
        public string Name { get; set; }
    }
}