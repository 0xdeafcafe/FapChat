using System.Collections.Generic;

namespace FapChat.Core.Snapchat.Helpers
{
	/// <summary>
	///     Helper for Snapchat Events
	/// </summary>
	public static class Events
	{
		public enum EventType
		{
			SnapViewed,
			SnapExpired,
			SnapScreenshotted
		}

		/// <summary>
		///     Creates an Event Object from some params
		/// </summary>
		/// <param name="eventType">The type of event we are sending</param>
		/// <param name="paramId">The Id to send in the params section</param>
		/// <param name="timestamp">The timestamp of the event</param>
		/// <returns></returns>
		public static Dictionary<string, object> CreateEvent(EventType eventType, string paramId, int timestamp)
		{
			string eventName;

			switch (eventType)
			{
				case EventType.SnapExpired:
					eventName = "SNAP_EXPIRED";
					break;
				case EventType.SnapScreenshotted:
					eventName = "SNAP_SCREENSHOT";
					break;
				case EventType.SnapViewed:
					eventName = "SNAP_VIEW";
					break;
				default:
					eventName = "SNAP_UNKNOWN";
					break;
			}

			return new Dictionary<string, object>
			{
				{"eventName", eventName},
				{
					"params", new Dictionary<string, string>
					{
						{"id", paramId}
					}
				},
				{"ts", timestamp}
			};
		}
	}
}