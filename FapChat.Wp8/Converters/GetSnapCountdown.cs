using System;
using System.Globalization;
using System.Windows.Data;
using FapChat.Core.Snapchat.Models;

namespace FapChat.Wp8.Converters
{
	public class GetSnapCountdown : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var snap = value as Snap;
			if (snap == null || snap.OpenedAt == null || snap.GetState != SnapState.Available)
				return null;

			TimeSpan countDown = (DateTime.UtcNow - (DateTime) snap.OpenedAt);
			return countDown.TotalSeconds.ToString(CultureInfo.InvariantCulture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}