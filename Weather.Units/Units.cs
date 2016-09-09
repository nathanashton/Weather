using System.Collections.Generic;
using System.Linq;
using Weather.Units.Interfaces;

namespace Weather.Units
{
    public class Units
    {
        public static List<IUnit> AllUnits { get; set; }

        public Units()
        {
            AllUnits = new List<IUnit>
            {
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Pressure),
                    EnumUnit = UnitEnums.EnumUnit.Hectopascal,
                    DisplayUnit = "Hpa",
                    DisplayName = "Hectopascal"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Pressure),
                    EnumUnit = UnitEnums.EnumUnit.Millibar,
                    DisplayUnit = "mb",
                    DisplayName = "Millibar"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Pressure),
                    EnumUnit = UnitEnums.EnumUnit.InchesMercury,
                    DisplayUnit = "inHg",
                    DisplayName = "In Hg"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Pressure),
                    EnumUnit = UnitEnums.EnumUnit.Centibar,
                    DisplayUnit = "cb",
                    DisplayName = "Centibar"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Temperature),
                    EnumUnit = UnitEnums.EnumUnit.Celsius,
                    DisplayUnit = "°C",
                    DisplayName = "Celsius"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Temperature),
                    EnumUnit = UnitEnums.EnumUnit.Fahrenheit,
                    DisplayUnit = "°F",
                    DisplayName = "Fahrenheit"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Temperature),
                    EnumUnit = UnitEnums.EnumUnit.Kelvin,
                    DisplayUnit = "K",
                    DisplayName = "Kelvin"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Velocity),
                    EnumUnit = UnitEnums.EnumUnit.MetresPerSecond,
                    DisplayUnit = "ms",
                    DisplayName = "Metres Per Second"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Velocity),
                    EnumUnit = UnitEnums.EnumUnit.KilometresPerHour,
                    DisplayUnit = "kmh",
                    DisplayName = "KmH"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Velocity),
                    EnumUnit = UnitEnums.EnumUnit.MilesPerHour,
                    DisplayUnit = "mph",
                    DisplayName = "Miles per Hour"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Velocity),
                    EnumUnit = UnitEnums.EnumUnit.Knots,
                    DisplayUnit = "k",
                    DisplayName = "Knots"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Direction),
                    EnumUnit = UnitEnums.EnumUnit.Cardinal,
                    DisplayUnit = "",
                    DisplayName = "Direction"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Direction),
                    EnumUnit = UnitEnums.EnumUnit.Degrees,
                    DisplayUnit = "°",
                    DisplayName = "Degrees"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Humidity),
                    EnumUnit = UnitEnums.EnumUnit.Percent,
                    DisplayUnit = "%",
                    DisplayName = "Percent"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Uv),
                    EnumUnit = UnitEnums.EnumUnit.UvIndex,
                    DisplayUnit = "",
                    DisplayName = "UV Index"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Precipitation),
                    EnumUnit = UnitEnums.EnumUnit.Millimeters,
                    DisplayUnit = "mm",
                    DisplayName = "mm"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Precipitation),
                    EnumUnit = UnitEnums.EnumUnit.Inches,
                    DisplayUnit = "in",
                    DisplayName = "in"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.PrecipitationRate),
                    EnumUnit = UnitEnums.EnumUnit.MillimetersPerHour,
                    DisplayUnit = "mm/h",
                    DisplayName = "mm/h"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.PrecipitationRate),
                    EnumUnit = UnitEnums.EnumUnit.InchesPerHour,
                    DisplayUnit = "in/h",
                    DisplayName = "in/h"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Irradiance),
                    EnumUnit = UnitEnums.EnumUnit.WattsPerSquareMeter,
                    DisplayUnit = "wsm",
                    DisplayName = "Watts Per Square Meter"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.Luminosity),
                    EnumUnit = UnitEnums.EnumUnit.Flux,
                    DisplayUnit = "F",
                    DisplayName = "Flux"
                },
                new Unit
                {
                    UnitType = (UnitType) UnitTypes.GetUnitType(UnitEnums.EnumUnitType.CloudCover),
                    EnumUnit = UnitEnums.EnumUnit.Okta,
                    DisplayUnit = "Okta",
                    DisplayName = "Okta"
                }
            };
        }

        public static List<IUnit> GetAllUnitsOfType(UnitEnums.EnumUnitType type)
        {
            return AllUnits.Where(x=> x.UnitType.Type == type).ToList();
        }

        public static IUnit GetUnit(UnitEnums.EnumUnit enumUnit)
        {
            return AllUnits.FirstOrDefault(x => x.EnumUnit == enumUnit);
        }

        public static IUnit GetUnitById(int id)
        {
            return AllUnits.FirstOrDefault(x => x.UnitId == id);
        }

        //public static Unit Kelvin = new Unit
        //{
        //    EnumUnitType = UnitEnums.EnumUnitType.Temperature,

        //    DisplayUnit = "K",
        //    DisplayName = "Kelvin"
        //};

        //public static Unit Humidity = new Unit
        //{
        //    EnumUnitType = UnitEnums.EnumUnitType.Humidity,
        //    DisplayUnit = "%",
        //    DisplayName = "Percent"
        //};

        //public static Unit Hectopascals = new Unit
        //{
        //    EnumUnitType = UnitEnums.EnumUnitType.Pressure,
        //    DisplayUnit = "hPa",
        //    DisplayName = "Hectopascals"
        //};

        //public static Unit InHg = new Unit
        //{
        //    EnumUnitType = UnitEnums.EnumUnitType.Pressure,
        //    DisplayUnit = "inHg",
        //    DisplayName = "Inches Hg"
        //};

        //public static Unit MmHg = new Unit
        //{
        //    EnumUnitType = UnitEnums.EnumUnitType.Pressure,
        //    DisplayUnit = "mmHg",
        //    DisplayName = "Mm Hg"
        //};

        //public static Unit Kmh = new Unit
        //{
        //    EnumUnitType = UnitEnums.EnumUnitType.Velocity,
        //    DisplayUnit = "kmh",
        //    DisplayName = "Km/H"
        //};
    }
}