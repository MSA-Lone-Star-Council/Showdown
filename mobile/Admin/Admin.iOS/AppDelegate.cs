﻿using Admin.Common.API;
using Common.Common;
using Foundation;
using UIKit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Admin.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations

		public AdminRESTClient BackendClient { get; set; }

		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
            AppCenter.Start(Secrets.AdminiOSAppCenterSecret, typeof(Analytics), typeof(Crashes));

            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            BackendClient = new AdminRESTClient();

            Window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                RootViewController = BuildRootViewController()
            };
            Window.MakeKeyAndVisible();

			return true;
		}

		private UIViewController BuildRootViewController()
		{
			var navigationController = new UINavigationController(new LoginViewController());

			return navigationController;
		}
	}
}

