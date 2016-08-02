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
            RawValue = humidity;
            DisplayUnit = Units.Humidity;
            DisplayValue = CorrectedValue;
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