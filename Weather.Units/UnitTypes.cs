using System.Collections.Generic;
using System.Linq;
using Weather.Units.Interfaces;

namespace Weather.Units
{
    public static class UnitTypes
    {
        public static List<IUnitType> AllUnitTypes { get; set; }

        static UnitTypes()
        {
            AllUnitTypes = new List<IUnitType>
            {
                new UnitType
                {
                    UnitTypeId = 1,
                    Type = UnitEnums.EnumUnitType.Pressure,
                    DisplayName = "Pressure",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Pressure),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.Hectopascal)
                },
                new UnitType
                {
                    UnitTypeId = 2,
                    Type = UnitEnums.EnumUnitType.Temperature,
                    DisplayName = "Temperature",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Temperature),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.Celsius)
                },
                new UnitType
                {
                    UnitTypeId = 3,
                    Type = UnitEnums.EnumUnitType.Velocity,
                    DisplayName = "Velocity",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Velocity),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.KilometresPerHour)
                },
                new UnitType
                {
                    UnitTypeId = 4,
                    Type = UnitEnums.EnumUnitType.Direction,
                    DisplayName = "Direction",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Direction),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.Degrees)
                },
                new UnitType
                {
                    UnitTypeId = 5,
                    Type = UnitEnums.EnumUnitType.Humidity,
                    DisplayName = "Humidity",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Humidity),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.Percent)
                },
                new UnitType
                {
                    UnitTypeId = 6,
                    Type = UnitEnums.EnumUnitType.Uv,
                    DisplayName = "UV",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Uv),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.UvIndex)
                },
                new UnitType
                {
                    UnitTypeId = 7,
                    Type = UnitEnums.EnumUnitType.Precipitation,
                    DisplayName = "Precipitation",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Precipitation),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.Millimeters)
                },
                new UnitType
                {
                    UnitTypeId = 8,
                    Type = UnitEnums.EnumUnitType.PrecipitationRate,
                    DisplayName = "Precipitation Rate",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.PrecipitationRate),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.MillimetersPerHour)
                },
                new UnitType
                {
                    UnitTypeId = 9,
                    Type = UnitEnums.EnumUnitType.Irradiance,
                    DisplayName = "Irradiance",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Irradiance),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.WattsPerSquareMeter)
                },
                new UnitType
                {
                    UnitTypeId = 10,
                    Type = UnitEnums.EnumUnitType.Luminosity,
                    DisplayName = "Luminosity",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Luminosity),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.Flux)
                },
                new UnitType
                {
                    UnitTypeId = 11,
                    Type = UnitEnums.EnumUnitType.CloudCover,
                    DisplayName = "Cloud Cover",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.CloudCover),
                    SiUnit = Units.GetUnit(UnitEnums.EnumUnit.Okta)
                }
            };
        }

        public static IUnitType GetUnitType(UnitEnums.EnumUnitType type)
        {
            return AllUnitTypes.FirstOrDefault(x => x.Type == type);
        }

        public static IUnitType GetUnitTypeById(int id)
        {
            var t = AllUnitTypes.FirstOrDefault(x => x.UnitTypeId == id);
            return t;
        }
    }
}