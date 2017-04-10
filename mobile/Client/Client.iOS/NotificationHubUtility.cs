﻿using System;
using System.Collections.Generic;
using Client.Common;
using Common.Common;
using Common.iOS;
using Foundation;
using WindowsAzure.Messaging;

namespace Client.iOS
{
	public class NotificationHubUtility : INotificationHub
	{
		public NSData DeviceToken { get; set; }

		public void SaveTags(List<string> tags)
		{
			var Hub = new SBNotificationHub(Secrets.AzureConnectionString, Secrets.NotificationHubPath);
			Hub.UnregisterAllAsync(DeviceToken, (error) =>
			{
				if (error != null)
				{
					return;
				}

				NSSet tagsSet = new NSSet(tags.ToArray());
				Hub.RegisterNativeAsync(DeviceToken, tagsSet, callbackError =>
				{
					if (callbackError != null) return;
				});
			});
		}
	}
}