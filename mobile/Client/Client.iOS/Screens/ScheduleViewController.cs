﻿using System;
using System.Collections.Generic;
using Client.Common;
using Common.Common.Models;
using Foundation;
using UIKit;

namespace Client.iOS
{
	public class ScheduleViewController : UIViewController, IScheduleView
	{
		static NSString EventCellID = new NSString("EventCellId");

		SchedulePresenter presenter;

		UITableView scheduleList;

		public async override void ViewWillAppear(bool animated)
		{
			var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
			presenter = new SchedulePresenter(appDelegate.BackendClient);
			presenter.TakeView(this);
			await presenter.OnBegin();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			View.BackgroundColor = new UIColor(0.16f, 0.75f, 1.00f, 1.0f);

			var tableSource = new ScheduleTableSource();
			tableSource.Events = new List<Event>();
			tableSource.RowTappedEvent += (row) => presenter.OnClickRow(row);

			scheduleList = new UITableView(View.Bounds)
			{
				BackgroundColor = UIColor.Clear,
				Source = tableSource,
				SeparatorStyle = UITableViewCellSeparatorStyle.None,
				RowHeight = 110,
			};
			scheduleList.RegisterClassForCellReuse(typeof(EventCell), EventCellID);

			View.AddSubview(scheduleList);
		}

		List<Event> IScheduleView.Events
		{
			set
			{
				ScheduleTableSource sts = scheduleList.Source as ScheduleTableSource;
				sts.Events = value;
				scheduleList.ReloadData();
			}
		}

		void IScheduleView.OpenEvent(Event row)
		{
			// TODO
		}

		void IScheduleView.ShowMessage(string message)
		{
			throw new NotImplementedException();
		}

		class ScheduleTableSource : UITableViewSource
		{
			public delegate void OnRowTapped(Event row);

			public List<Event> Events { get; set; }

			public event OnRowTapped RowTappedEvent;

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell(EventCellID) as EventCell;
				cell.BackgroundColor = UIColor.Clear;

				var item = Events[indexPath.Row];

				cell.UpdateCell(item);

				return cell;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return Events.Count;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				var row = Events[indexPath.Row];
				RowTappedEvent(row);
				tableView.DeselectRow(indexPath, false);
			}
		}
	}
}

