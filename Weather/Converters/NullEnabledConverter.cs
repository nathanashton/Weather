using System;
using System.Globalization;
using System.Windows.Data;

namespace Weather.Converters
{
    public class NullEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (s != null)
            {
                return !string.IsNullOrEmpty(s);
            }

            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}