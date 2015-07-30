
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;

namespace JudoDotNetXamariniOSSDK
{
	public partial class PaymentReceipt : UIViewController
	{
		private PaymentReceiptViewModel _receipt;

		private List<UITableViewCell> CellsToShow { get; set; }

		public PaymentReceipt (PaymentReceiptViewModel receipt) : base ("PaymentReceipt", null)
		{
			_receipt = receipt;
		}



		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			SetUpTableView ();
			this.View.BackgroundColor = new UIColor(245f, 245f, 245f,1f);
		
		}

		void SetUpTableView ()
		{
			ReceiptStringItemCell dateCell = new ReceiptStringItemCell (new IntPtr ());
			dateCell.Label = "Date";
			dateCell.Value = _receipt.CreatedAt.ToLongDateString() + ", " + DateTime.Now.TimeOfDay.ToString();

			ReceiptStringItemCell amountCell = new ReceiptStringItemCell (new IntPtr ()); 
			amountCell.Label ="Amount";
			amountCell.Value = _receipt.OriginalAmount + " " + _receipt.Currency;
			ReceiptNoteCell noteCell = new ReceiptNoteCell (new IntPtr ()); 
			noteCell.Text= @"This is a receipt example showing only the createdAT,amount and currency properties of the receipt object.
			 Please refer to our API Reference Documentation for all the properties you can display";

			CellsToShow = new List<UITableViewCell> (){ dateCell, amountCell,noteCell };


			UITableViewSource tableSource = new GenericTableViewSource (CellsToShow.ToArray());
			ReceiptTableView.Source = tableSource;
			ReceiptTableView.ScrollEnabled = false;
		}
	}
}

