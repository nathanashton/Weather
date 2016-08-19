using System.Collections.Generic;

namespace Weather.Common.Units
{
    public class Units
    {
        public static List<Unit> UnitsList = new List<Unit>();

        static Units()
        {
            UnitsList.Add(Celsius);
            UnitsList.Add(Fahrenheit);
            UnitsList.Add(Kelvin);
            UnitsList.Add(Humidity);
            UnitsList.Add(Hectopascals);
            UnitsList.Add(InHg);
            UnitsList.Add(MmHg);
            UnitsList.Add(Kmh);
        }

        public static Unit Celsius = new Unit
        {
            Type = null,
            DisplayUnit = "°C",
            DisplayName = "Celsius"
        };

        public static Unit Fahrenheit = new Unit
        {
            Type = null,
            DisplayUnit = "°F",
            DisplayName = "Fahrenheit"
        };

        public static Unit Kelvin = new Unit
        {
            Type = null,
            DisplayUnit = "K",
            DisplayName = "Kelvin"
        };

        public static Unit Humidity = new Unit
        {
            Type = null,
            DisplayUnit = "%",
            DisplayName = "Percent"
        };

        public static Unit Hectopascals = new Unit
        {
            Type = null,
            DisplayUnit = "hPa",
            DisplayName = "Hectopascals"
        };

        public static Unit InHg = new Unit
        {
            Type = null,
            DisplayUnit = "inHg",
            DisplayName = "Inches Hg"
        };

        public static Unit MmHg = new Unit
        {
            Type = null,
            DisplayUnit = "mmHg",
            DisplayName = "Mm Hg"
        };

        public static Unit Kmh = new Unit
        {
            Type = null,
            DisplayUnit = "kmh",
            DisplayName = "Km/H"
        };
    }
}