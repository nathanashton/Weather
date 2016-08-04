using System.ComponentModel;

namespace Weather.Common.Entities
{
    public class Enums
    {
        public enum UnitType
        {
            [Description("°C")]
            Temperature,

            [Description("hPa")]
            Pressure,

            [Description("km/h")]
            WindSpeed,

            [Description("%")]
            Humidity,

            [Description("mm")]
            Rainfall,

            [Description("mm/h")]
            RainfallRate
        }
    }

    public class Unit
    {
        public Enums.UnitType Type { get; set; }
        public string DisplayName { get; set; }
        public string DisplayUnit { get; set; }

        public override string ToString()
        {
            return DisplayUnit;
        }
    }

    public static class Units
    {
        public static Unit Celsius = new Unit
        {
            Type = Enums.UnitType.Temperature,
            DisplayUnit = "°C",
            DisplayName = "Celsius"
        };

        public static Unit Fahrenheit = new Unit
        {
            Type = Enums.UnitType.Temperature,
            DisplayUnit = "°F",
            DisplayName = "Fahrenheit"
        };

        public static Unit Kelvin = new Unit
        {
            Type = Enums.UnitType.Temperature,
            DisplayUnit = "K",
            DisplayName = "Kelvin"
        };

        public static Unit Humidity = new Unit
        {
            Type = Enums.UnitType.Humidity,
            DisplayUnit = "%",
            DisplayName = "Percent"
        };

        public static Unit Hectopascals = new Unit
        {
            Type = Enums.UnitType.Pressure,
            DisplayUnit = "hPa",
            DisplayName = "Hectopascals"
        };

        public static Unit InHg = new Unit
        {
            Type = Enums.UnitType.Pressure,
            DisplayUnit = "inHg",
            DisplayName = "Inches Hg"
        };

        public static Unit MmHg = new Unit { Type = Enums.UnitType.Pressure, DisplayUnit = "mmHg", DisplayName = "Mm Hg" };
        public static Unit Kmh = new Unit { Type = Enums.UnitType.WindSpeed, DisplayUnit = "kmh", DisplayName = "Km/H" };
    }
}