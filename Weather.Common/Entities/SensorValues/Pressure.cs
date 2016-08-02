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
            RawValue = hectopascals;
            DisplayUnit = Units.Hectopascals;
            DisplayValue = CorrectedValue;
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
                    RawValue = value;
                    DisplayValue = CorrectedValue;
                    break;

                case "Inches Hg":
                    if (value != null)
                    {
                        RawValue = UnitConversions.InHgToHectopascals((double) value);
                    }
                    DisplayValue = value;
                    break;
            }
            DisplayUnit = unit;
        }

        public long Id { get; set; }
        public ISensor Sensor { get; set; }
        public double? RawValue { get; set; }
        public double? CorrectedValue
        {
            get
            {
                if (Sensor != null)
                {
                    return RawValue + Sensor.Correction;
                }
                return RawValue;
            }
        }
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
                    if (CorrectedValue != null)
                    {
                        DisplayValue = UnitConversions.InHgToHectopascals((double)CorrectedValue);
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
                    if (CorrectedValue != null)
                    {
                        DisplayValue = UnitConversions.HectopascalsToInHg((double)CorrectedValue);
                    }
                    break;
            }
            DisplayUnit = Units.InHg;
        }

        public void SetNull()
        {
            RawValue = null;
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