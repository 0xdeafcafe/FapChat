using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FapChat.Core.Snapchat.Models;

namespace FapChat.Wp8.Converters
{
	public class IconFromSnapMediaType : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var snap = value as Snap;
			if (snap == null)
				return new BitmapImage();

			string imagePath = "/Assets/SnappySnaps/{0}.png";

			imagePath = string.Format(imagePath, snap.RecipientName == null ? "received.{0}" : "sent.{0}");

			switch (snap.MediaType)
			{
				case MediaType.Image:
					if (snap.Status == SnapStatus.Opened && snap.RecipientName == null)
						imagePath = string.Format(imagePath, "viewed.{0}");

					imagePath = string.Format(imagePath, "image");
					break;

				case MediaType.Video:
				case MediaType.VideoNoAudio:
					if (snap.Status == SnapStatus.Opened && snap.RecipientName == null)
						imagePath = string.Format(imagePath, "viewed.{0}");

					imagePath = string.Format(imagePath, "video");
					break;

				case MediaType.FriendRequestImage:
					imagePath = string.Format(imagePath, "friend.image");
					break;

				case MediaType.FriendRequestVideo:
				case MediaType.FriendRequestVideoNoAudio:
					imagePath = string.Format(imagePath, "friend.video");
					break;
			}

			return new BitmapImage(new Uri(imagePath, UriKind.Relative));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}