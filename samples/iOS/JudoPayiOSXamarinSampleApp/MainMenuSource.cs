using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace JudoPayiOSXamarinSampleApp
{
	public class MainMenuSource: UITableViewSource
	{

		Dictionary<string,Delegate> ButtonDictionary;
		string CellIdentifier = "genericCell";

		public MainMenuSource (Dictionary<string,Delegate> buttonDictionary)
		{
			ButtonDictionary = buttonDictionary;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return ButtonDictionary.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);
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

