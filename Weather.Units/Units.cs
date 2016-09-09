using System.Collections.Generic;
using System.Linq;
using Weather.Units.Interfaces;

namespace Weather.Units
{
    public static class Units
    {
        public static List<IUnit> AllUnits { get; set; }

        static Units()
        {
            AllUnits = new List<IUnit>
            {
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Pressure,
                    EnumUnit = UnitEnums.EnumUnit.Hectopascal,
                    DisplayUnit = "Hpa",
                    DisplayName = "Hectopascal"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Pressure,
                    EnumUnit = UnitEnums.EnumUnit.Millibar,
                    DisplayUnit = "mb",
                    DisplayName = "Millibar"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Pressure,
                    EnumUnit = UnitEnums.EnumUnit.InchesMercury,
                    DisplayUnit = "inHg",
                    DisplayName = "In Hg"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Pressure,
                    EnumUnit = UnitEnums.EnumUnit.Centibar,
                    DisplayUnit = "cb",
                    DisplayName = "Centibar"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Temperature,
                    EnumUnit = UnitEnums.EnumUnit.Celsius,
                    DisplayUnit = "°C",
                    DisplayName = "Celsius"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Temperature,
                    EnumUnit = UnitEnums.EnumUnit.Fahrenheit,
                    DisplayUnit = "°F",
                    DisplayName = "Fahrenheit"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Temperature,
                    EnumUnit = UnitEnums.EnumUnit.Kelvin,
                    DisplayUnit = "K",
                    DisplayName = "Kelvin"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Velocity,
                    EnumUnit = UnitEnums.EnumUnit.MetresPerSecond,
                    DisplayUnit = "ms",
                    DisplayName = "Metres Per Second"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Velocity,
                    EnumUnit = UnitEnums.EnumUnit.KilometresPerHour,
                    DisplayUnit = "kmh",
                    DisplayName = "KmH"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Velocity,
                    EnumUnit = UnitEnums.EnumUnit.MilesPerHour,
                    DisplayUnit = "mph",
                    DisplayName = "Miles per Hour"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Velocity,
                    EnumUnit = UnitEnums.EnumUnit.Knots,
                    DisplayUnit = "k",
                    DisplayName = "Knots"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Direction,
                    EnumUnit = UnitEnums.EnumUnit.Cardinal,
                    DisplayUnit = "",
                    DisplayName = "Direction"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Direction,
                    EnumUnit = UnitEnums.EnumUnit.Degrees,
                    DisplayUnit = "°",
                    DisplayName = "Degrees"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Humidity,
                    EnumUnit = UnitEnums.EnumUnit.Percent,
                    DisplayUnit = "%",
                    DisplayName = "Percent"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Uv,
                    EnumUnit = UnitEnums.EnumUnit.UvIndex,
                    DisplayUnit = "",
                    DisplayName = "UV Index"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Precipitation,
                    EnumUnit = UnitEnums.EnumUnit.Millimeters,
                    DisplayUnit = "mm",
                    DisplayName = "mm"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Precipitation,
                    EnumUnit = UnitEnums.EnumUnit.Inches,
                    DisplayUnit = "in",
                    DisplayName = "in"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.PrecipitationRate,
                    EnumUnit = UnitEnums.EnumUnit.MillimetersPerHour,
                    DisplayUnit = "mm/h",
                    DisplayName = "mm/h"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.PrecipitationRate,
                    EnumUnit = UnitEnums.EnumUnit.InchesPerHour,
                    DisplayUnit = "in/h",
                    DisplayName = "in/h"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Irradiance,
                    EnumUnit = UnitEnums.EnumUnit.WattsPerSquareMeter,
                    DisplayUnit = "wsm",
                    DisplayName = "Watts Per Square Meter"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.Luminosity,
                    EnumUnit = UnitEnums.EnumUnit.Flux,
                    DisplayUnit = "F",
                    DisplayName = "Flux"
                },
                new Unit
                {
                    EnumType = UnitEnums.EnumUnitType.CloudCover,
                    EnumUnit = UnitEnums.EnumUnit.Okta,
                    DisplayUnit = "Okta",
                    DisplayName = "Okta"
                }
            };
        }

        public static List<IUnit> GetAllUnitsOfType(UnitEnums.EnumUnitType type)
        {
            return AllUnits.Where(x => x.EnumType == type).ToList();
        }

        public static IUnit GetUnit(UnitEnums.EnumUnit enumUnit)
        {
            return AllUnits.FirstOrDefault(x => x.EnumUnit == enumUnit);
        }

        public static IUnit GetUnitById(int id)
        {
            return AllUnits.FirstOrDefault(x => x.UnitId == id);
        }
    }
}