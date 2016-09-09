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
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Pressure)
                },
                new UnitType
                {
                    UnitTypeId = 2,
                    Type = UnitEnums.EnumUnitType.Temperature,
                    DisplayName = "Temperature",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Temperature)
                },
                new UnitType
                {
                    UnitTypeId = 3,
                    Type = UnitEnums.EnumUnitType.Velocity,
                    DisplayName = "Velocity",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Velocity)
                },
                new UnitType
                {
                    UnitTypeId = 4,
                    Type = UnitEnums.EnumUnitType.Direction,
                    DisplayName = "Direction",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Direction)
                },
                new UnitType
                {
                    UnitTypeId = 5,
                    Type = UnitEnums.EnumUnitType.Humidity,
                    DisplayName = "Humidity",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Humidity)
                },
                new UnitType
                {
                    UnitTypeId = 6,
                    Type = UnitEnums.EnumUnitType.Uv,
                    DisplayName = "UV",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Uv)
                },
                new UnitType
                {
                    UnitTypeId = 7,
                    Type = UnitEnums.EnumUnitType.Precipitation,
                    DisplayName = "Precipitation",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Precipitation)
                },
                new UnitType
                {
                    UnitTypeId = 8,
                    Type = UnitEnums.EnumUnitType.PrecipitationRate,
                    DisplayName = "Precipitation Rate",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.PrecipitationRate)
                },
                new UnitType
                {
                    UnitTypeId = 9,
                    Type = UnitEnums.EnumUnitType.Irradiance,
                    DisplayName = "Irradiance",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Irradiance)
                },
                new UnitType
                {
                    UnitTypeId = 10,
                    Type = UnitEnums.EnumUnitType.Luminosity,
                    DisplayName = "Luminosity",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.Luminosity)
                },
                new UnitType
                {
                    UnitTypeId = 11,
                    Type = UnitEnums.EnumUnitType.CloudCover,
                    DisplayName = "Cloud Cover",
                    Units = Units.GetAllUnitsOfType(UnitEnums.EnumUnitType.CloudCover)
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