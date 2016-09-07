using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Weather.Converters
{
    public class NullCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            var collection = value as ICollection;
            return (collection == null) || (collection.Count != 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}