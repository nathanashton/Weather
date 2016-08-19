using System.Collections.Generic;

namespace Weather.Common.Units
{
    public class UnitTypes
    {
        public static List<UnitType> UnitTypesList = new List<UnitType>();

        public UnitTypes()
        {
            UnitTypesList.Add(Temperature);
            UnitTypesList.Add(Pressure);
            UnitTypesList.Add(Humidity);
            UnitTypesList.Add(WindSpeed);
        }

        public UnitType Temperature = new UnitType
        {
            Name = "Temperature",
            DefaultUnit = Units.Kelvin
        };

        public UnitType Pressure = new UnitType
        {
            Name = "Pressure",
            DefaultUnit = Units.Hectopascals
        };

        public UnitType Humidity = new UnitType
        {
            Name = "Humidity",
            DefaultUnit = Units.Humidity
        };

        public UnitType WindSpeed = new UnitType
        {
            Name = "Wind Speed",
            DefaultUnit = Units.Kmh
        };
    }
}