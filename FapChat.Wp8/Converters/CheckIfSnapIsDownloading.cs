using System.Windows.Data;
using FapChat.Core.Snapchat.Models;

namespace FapChat.Wp8.Converters
{
    public class CheckIfSnapIsDownloading : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var snap = value as Snap;
            if (snap == null)
                return null;

            return snap.Status == SnapStatus.Downloading ? "Visibility" : "Collapsed";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
