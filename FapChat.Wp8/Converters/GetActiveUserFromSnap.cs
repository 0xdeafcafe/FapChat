using System;
using System.Globalization;
using System.Windows.Data;
using FapChat.Core.Snapchat.Models;

namespace FapChat.Wp8.Converters
{
    public class GetActiveUserFromSnap : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var snap = value as Snap;
            if (snap == null)
                return null;

            return snap.ScreenName ?? snap.Recipient;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
