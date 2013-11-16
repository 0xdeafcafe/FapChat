using System;
using System.Globalization;
using System.Windows.Data;

namespace FapChat.Wp8.Converters
{
    public class GetFriendlyTimeFromSnapChatTimestamp : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeStamp = Int64.Parse(value.ToString());
            var dateTime = Core.Snapchat.Helpers.Timestamps.ConvertToDateTime(timeStamp);

            return dateTime.ToString("dd/M/yy - HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
