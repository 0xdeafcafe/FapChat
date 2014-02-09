using System;
using System.Globalization;
using System.Windows.Data;
using FapChat.Core.Snapchat.Helpers;
using FapChat.Wp8.Helpers;

namespace FapChat.Wp8.Converters
{
	public class GetFriendlyTimeFromSnapChatTimestamp : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			long timeStamp = Int64.Parse(value.ToString());
			DateTime dateTime = Timestamps.ConvertToDateTime(timeStamp);

			return Time.GetReleativeDate(dateTime) ?? dateTime.ToString("dd/M/yy - HH:mm");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}