using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using ObjCRuntime;

namespace JudoDotNetXamariniOSSDK
{
	public class CardCellSource : UITableViewSource
	{

		public List<CardCell> TableItems;
		string CellIdentifier = "CellID";

		public CardCellSource (List<CardCell> items)
		{
			TableItems = items;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return TableItems.Count;
		}



		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)  
		{
			var cell = tableView.DequeueReusableCell (TableItems [indexPath.Row].Key);
			if (cell == null) {
				var ccell = (CardCell)TableItems [indexPath.Row];
				ccell.SetUpCell ();
				return ccell;
			}

			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (TableItems [indexPath.Row].Key);
			if (cell == null) {
				cell = (CardCell)TableItems [indexPath.Row];

			}
			return cell.Bounds.Height;
		}


		public float GetTableHeight ()
		{
			float height = 0f;
			foreach (UITableViewCell cell in TableItems) {
				height += (float)cell.Frame.Height;
			}
			return height;
		}
	}
}

