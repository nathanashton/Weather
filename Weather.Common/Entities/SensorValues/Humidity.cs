using System;
using PropertyChanged;
using Weather.Common.Interfaces;

namespace Weather.Common.Entities.SensorValues
{
    [ImplementPropertyChanged]
    public class Humidity : IHumidity
    {
        private double? _displayValue;

        public Humidity(double? humidity)
        {
            Value = humidity;
            DisplayUnit = Units.Humidity;
            DisplayValue = Value;
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