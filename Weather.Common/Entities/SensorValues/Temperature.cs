using System;
using System.ComponentModel;
using System.Linq.Expressions;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities.SensorValues
{
    [ImplementPropertyChanged]
    public class Temperature : ITemperature
    {
        private double? _displayValue;

        public Temperature(double? celsius)
        {
            RawValue = celsius;
            DisplayUnit = Units.Celsius;
            DisplayValue = CorrectedValue;
        }

        public Temperature(double? value, Unit unit)
        {
            if (unit.Type != Enums.UnitType.Temperature)
            {
                throw new Exception("Not a temperature unit");
            }
            switch (unit.DisplayName)
            {
                case "Celsius":
                    RawValue = value;
                    DisplayValue = CorrectedValue;
                    break;

                case "Fahrenheit":
                    if (value != null)
                    {
                        RawValue = UnitConversions.FahrenheitToCelsius((double) value);
                    }
                    DisplayValue = CorrectedValue;
                    break;

                case "Kelvin":
                    if (value != null)
                    {
                        RawValue = UnitConversions.KelvinToCelsius((double) value);
                    }
                    DisplayValue = CorrectedValue;
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
            set
            {
                _displayValue = value;
                OnPropertyChanged(() => DisplayValue);
            }
        }

        public void SetNull()
        {
            RawValue = null;
            DisplayValue = null;
        }

        public void DisplayDegreesCelsius()
        {
            switch (DisplayUnit.DisplayName)
            {
                case "Celsius":
                    if (CorrectedValue != null)
                    {
                        DisplayValue = CorrectedValue;
                    }
                    break;
                case "Fahrenheit":
                    if (CorrectedValue != null)
                    {
                        DisplayValue = UnitConversions.FahrenheitToCelsius((double)CorrectedValue);
                    }
                    break;

                case "Kelvin":
                    if (CorrectedValue != null)
                    {
                        DisplayValue = UnitConversions.KelvinToCelsius((double) CorrectedValue);
                    }
                    break;
            }
            DisplayUnit = Units.Celsius;
        }

        public void DisplayDegreesFahrenheit()
        {
            switch (DisplayUnit.DisplayName)
            {
                case "Fahrenheit":
                    if (CorrectedValue != null)
                    {
                        DisplayValue = CorrectedValue;
                    }
                    break;
                case "Celsius":
                    if (CorrectedValue != null)
                    {
                        DisplayValue = UnitConversions.CelsiusToFahrenheit((double)CorrectedValue);
                    }
                    break;

                case "Kelvin":
                    if (CorrectedValue != null)
                    {
                        DisplayValue = UnitConversions.KelvinToFahrenheit((double)CorrectedValue);
                    }
                    break;
            }
            DisplayUnit = Units.Fahrenheit;
        }

        public void DisplayKelvin()
        {
            switch (DisplayUnit.DisplayName)
            {
                case "Kelvin":
                    if (CorrectedValue != null)
                    {
                        DisplayValue = CorrectedValue;
                    }
                    break;
                case "Celsius":
                    if (CorrectedValue != null)
                    {
                        DisplayValue = UnitConversions.CelsiusToKelvin((double)CorrectedValue);
                    }
                    break;

                case "Fahrenheit":
                    if (CorrectedValue != null)
                    {
                        DisplayValue = UnitConversions.FahrenheitToKelvin((double)CorrectedValue);
                    }
                    break;
            }
            DisplayUnit = Units.Kelvin;
        }

        public override string ToString()
        {
            if (DisplayValue == null)
            {
                return "- " + DisplayUnit.DisplayUnit;
            }
            return DisplayValue + " " + DisplayUnit.DisplayUnit;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged<T>(Expression<Func<T>> exp)
        {
            //the cast will always succeed
            var memberExpression = (MemberExpression) exp.Body;
            var propertyName = memberExpression.Member.Name;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}