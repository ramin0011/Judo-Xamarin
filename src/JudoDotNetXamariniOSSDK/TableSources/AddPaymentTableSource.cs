using System;
using UIKit;
using Foundation;

namespace JudoDotNetXamariniOSSDK
{
	public class AddPaymentTableSource : UITableViewSource {

		UITableViewCell[] TableItems;
		string CellIdentifier = "TableCell";

		public AddPaymentTableSource (UITableViewCell[] items)
		{
			TableItems = items;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return TableItems.Length;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);
			cell = TableItems[indexPath.Row];
			cell.IndentationLevel = 0;
			//---- if there are no cells to reuse, create a new one
			if (cell != null)
			{
			return cell;
			}
			else return  new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);
	}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			
			UITableViewCell cell = TableItems[indexPath.Row];
			return cell.Bounds.Height;
		}

		public float GetTableHeight()
		{
			float height=0f;
			foreach (UITableViewCell cell in TableItems) {
				height += (float)cell.Frame.Height;
			}
			return height;
		}
}
}

