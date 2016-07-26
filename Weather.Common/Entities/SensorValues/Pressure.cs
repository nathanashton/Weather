using System;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities.SensorValues
{
    [ImplementPropertyChanged]
    public class Pressure : IPressure
    {
        private double? _displayValue;

        public Pressure(double? hectopascals)
        {
            Value = hectopascals;
            DisplayUnit = Units.Hectopascals;
            DisplayValue = Value;
        }

        public Pressure(double? value, Unit unit)
        {
            if (unit.Type != Enums.UnitType.Pressure)
            {
                throw new Exception("Not a pressure unit");
            }
            switch (unit.DisplayName)
            {
                case "Hectopascals":
                    Value = value;
                    DisplayValue = Value;
                    break;

                case "Inches Hg":
                    if (value != null)
                    {
                        Value = UnitConversions.InHgToHectopascals((double) value);
                    }
                    DisplayValue = value;
                    break;
            }
            DisplayUnit = unit;
        }

        public ISensor Sensor { get; set; }
        public double? Value { get; set; }
        public Unit DisplayUnit { get; set; }

        public double? DisplayValue
        {
            get
            {
                if (_displayValue == null)
                {
                    return null;
                }
                return Math.Round((double) _displayValue, UnitConversions.Rounding, MidpointRounding.AwayFromZero);
            }
            set { _displayValue = value; }
        }

        public void DisplayHectopascals()
        {
            switch (DisplayUnit.DisplayName)
            {
                case "Inches Hg":
                    if (Value != null)
                    {
                        DisplayValue = UnitConversions.InHgToHectopascals((double) Value);
                    }
                    break;
            }
            DisplayUnit = Units.Hectopascals;
        }

        public void DisplayInHg()
        {
            switch (DisplayUnit.DisplayName)
            {
                case "Hectopascals":
                    if (Value != null)
                    {
                        DisplayValue = UnitConversions.HectopascalsToInHg((double) Value);
                    }
                    break;
            }
            DisplayUnit = Units.InHg;
        }

        public void SetNull()
        {
            Value = null;
            DisplayValue = null;
        }

        public override string ToString()
        {
            if (DisplayValue == null)
            {
                return "- " + DisplayUnit.DisplayUnit;
            }
            return DisplayValue + " " + DisplayUnit.DisplayUnit;
        }
    }
}