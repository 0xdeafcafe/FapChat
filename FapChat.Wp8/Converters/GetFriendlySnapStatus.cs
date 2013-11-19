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
            if (snap == null || snap.Status == SnapStatus.Downloading)
                return "";

            switch (snap.Status)
            {
                case SnapStatus.Delivered:
                    if (snap.RecipientName != null)
                        return "Delivered...";

                    return snap.HasMedia ? "Tap and Hold..." : "Tap to Load...";

                case SnapStatus.None:
                    return "None...";

                case SnapStatus.Opened:
                    return "Opened...";

                case SnapStatus.Screenshotted:
                    return "Screenshotted... Poor you...";

                case SnapStatus.Sent:
                    return "Sent...";

                case SnapStatus.Downloading:
                    return "Downloading...";

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
