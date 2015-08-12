using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using ObjCRuntime;

namespace JudoDotNetXamariniOSSDK
{
	public class AddPaymentTableSource : UITableViewSource
	{

		public List<UITableViewCell> TableItems;
		string CellIdentifier = "CellID";

		public AddPaymentTableSource (List<UITableViewCell> items)
		{
			TableItems = items;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return TableItems.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell("CardDetailCell") as CardDetailCell;

			if (cell == null)
			{
				cell = TableItems [indexPath.Row] as CardDetailCell;
				var views = NSBundle.MainBundle.LoadNib("CardDetailCell", cell, null);
				cell = Runtime.GetNSObject( views.ValueAt(0) ) as CardDetailCell;
			}

			//cell.BindDataToCell("You are on row " + indexPath.Row);

			return cell;


//			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);
//			cell = TableItems [indexPath.Row];
//			cell.IndentationLevel = 0;
//			cell.BackgroundColor = UIColor.Black;
//			if (cell != null) {
//				return cell;
//			} else
//				return  new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			
			UITableViewCell cell = TableItems [indexPath.Row];
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

