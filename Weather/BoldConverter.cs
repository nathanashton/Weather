using System;
using System.Globalization;
using System.Windows.Data;
using Weather.Common.Interfaces;

namespace Weather
{
    public class BoldConverter : IValueConverter
    {

        public static void testc()
        {
            
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (ISensorValue)value;
            switch (v.Sensor.Type)
            {
                case Common.Entities.Enums.UnitType.Humidity:
                    return v.DisplayValue +" %";

                case Common.Entities.Enums.UnitType.Temperature:
                    return v.DisplayValue + " C";

                case Common.Entities.Enums.UnitType.Pressure:
                    return v.DisplayValue + " hpa";

                case Common.Entities.Enums.UnitType.WindSpeed:
                    return v.DisplayValue + " kmh";
            }
            return v.DisplayValue + "mm";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
