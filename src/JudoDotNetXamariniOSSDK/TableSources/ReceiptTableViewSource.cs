using System;
using UIKit;
using Foundation;
using ObjCRuntime;


namespace JudoDotNetXamariniOSSDK
{
	public class ReceiptTableViewSource : UITableViewSource
	{

		ReceiptStringItemCell[] TableItems;
		string CellIdentifier = "genericCell";

		public ReceiptTableViewSource (ReceiptStringItemCell[] items)
		{
			TableItems = items;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return TableItems.Length;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			ReceiptStringItemCell receiptcell = tableView.DequeueReusableCell (CellIdentifier) as ReceiptStringItemCell;
			receiptcell = TableItems [indexPath.Row];
			UITableViewCell cell= new UITableViewCell (UITableViewCellStyle.Value1, CellIdentifier);
			cell.IndentationLevel = 0;
			cell.TextLabel.Text = receiptcell.Label;
			cell.DetailTextLabel.Text = receiptcell.Value;
			if (cell != null) {
				return cell;
			} else
				return  new UITableViewCell (UITableViewCellStyle.Value1, CellIdentifier);
		}

		public float GetTableHeight()
		{
			float height=0f;
			foreach (ReceiptStringItemCell cell in TableItems) {
				height += (float)cell.Frame.Height;
			}
			return height;
		}



		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{

			ReceiptStringItemCell cell = TableItems [indexPath.Row];
			return cell.Bounds.Height;
		}
	}
}

