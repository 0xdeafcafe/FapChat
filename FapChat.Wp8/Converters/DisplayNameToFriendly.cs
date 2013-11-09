using System;
using System.Globalization;
using System.Windows.Data;

namespace FapChat.Wp8.Converters
{
    public class DisplayNameToFriendly : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var displayName = value.ToString();

            return (displayName == "") ? "No Display Name given" : displayName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
