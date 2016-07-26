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
            Value = celsius;
            DisplayUnit = Units.Celsius;
            DisplayValue = Value;
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
                    Value = value;
                    DisplayValue = Value;
                    break;

                case "Fahrenheit":
                    if (value != null)
                    {
                        Value = UnitConversions.FahrenheitToCelsius((double) value);
                    }
                    DisplayValue = value;
                    break;

                case "Kelvin":
                    if (value != null)
                    {
                        Value = UnitConversions.KelvinToCelsius((double) value);
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
            set
            {
                _displayValue = value;
                OnPropertyChanged(() => DisplayValue);
            }
        }

        public void SetNull()
        {
            Value = null;
            DisplayValue = null;
        }

        public void DisplayDegreesCelsius()
        {
            switch (DisplayUnit.DisplayName)
            {
                case "Fahrenheit":
                    if (Value != null)
                    {
                        DisplayValue = UnitConversions.FahrenheitToCelsius((double) Value);
                    }
                    break;

                case "Kelvin":
                    if (Value != null)
                    {
                        DisplayValue = UnitConversions.KelvinToCelsius((double) Value);
                    }
                    break;
            }
            DisplayUnit = Units.Celsius;
        }

        public void DisplayDegreesFahrenheit()
        {
            switch (DisplayUnit.DisplayName)
            {
                case "Celsius":
                    if (Value != null)
                    {
                        DisplayValue = UnitConversions.CelsiusToFahrenheit((double) Value);
                    }
                    break;

                case "Kelvin":
                    if (Value != null)
                    {
                        DisplayValue = UnitConversions.KelvinToFahrenheit((double) Value);
                    }
                    break;
            }
            DisplayUnit = Units.Fahrenheit;
        }

        public void DisplayKelvin()
        {
            switch (DisplayUnit.DisplayName)
            {
                case "Celsius":
                    if (Value != null)
                    {
                        DisplayValue = UnitConversions.CelsiusToKelvin((double) Value);
                    }
                    break;

                case "Fahrenheit":
                    if (Value != null)
                    {
                        DisplayValue = UnitConversions.FahrenheitToKelvin((double) Value);
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