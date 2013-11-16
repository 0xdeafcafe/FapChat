using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FapChat.Wp8.Converters
{
    public class CaptureScreenMessagesIconType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hasNewChats = (int.Parse(value.ToString()) > 0);
            var imagePath = "/Assets/SnappySnaps/received.{0}.png";
            imagePath = hasNewChats ? string.Format(imagePath, "new") : string.Format(imagePath, "clear");
            return new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
