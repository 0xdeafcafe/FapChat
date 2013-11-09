using System;
using System.Globalization;
using System.Windows.Data;

namespace FapChat.Wp8.Converters
{
    /// <summary>
    /// Converts a IsVip Boolean to a friendlier string
    /// </summary>
    public class VipBoolToFriendlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isVip = Boolean.Parse(value.ToString());

            return isVip ? "VIP Account" : "Standard Account";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
