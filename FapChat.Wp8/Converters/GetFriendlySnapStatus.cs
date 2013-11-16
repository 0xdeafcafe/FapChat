using System;
using System.Globalization;
using System.Windows.Data;
using FapChat.Core.Snapchat.Models;

namespace FapChat.Wp8.Converters
{
    public class GetFriendlySnapStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var snap = value as Snap;
            if (snap == null)
                return "";

            switch (snap.Status)
            {
                case SnapStatus.Delivered:
                    return "Delivered...";

                case SnapStatus.None:
                    return "None...";

                case SnapStatus.Opened:
                    return "Opened...";

                case SnapStatus.Screenshotted:
                    return "Screenshotted... Poor you...";

                case SnapStatus.Sent:
                    return "Sent...";

                default:
                    return "Unknown...";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
