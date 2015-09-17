
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;

namespace JudoDotNetXamariniOSSDK
{
	internal partial class PaymentReceipt : UIViewController
	{
		

		private List<ReceiptStringItemCell> CellsToShow { get; set; }

		private PaymentReceipt () : base ("PaymentReceipt", null)
		{
			
		}

		public override void DidReceiveMemoryWarning ()
		{

			base.DidReceiveMemoryWarning ();

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			SetUpTableView ();
			//ViewHeader.Text = _receipt.Message;
			this.View.BackgroundColor = new UIColor (245f, 245f, 245f, 1f);
			HomeButton.TouchUpInside += (sender, ev) => {
				this.NavigationController.PopToRootViewController (true);
			};

		}

		void SetUpTableView ()
		{
			ReceiptStringItemCell dateCell = new ReceiptStringItemCell ();
			dateCell.Label = "Date";
		//	dateCell.Value = _receipt.CreatedAt.ToLongDateString () + ", " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;

			ReceiptStringItemCell amountCell = new ReceiptStringItemCell (); 
			amountCell.Label = "Amount";
		//	amountCell.Value = _receipt.OriginalAmount + " " + _receipt.Currency;

			CellsToShow = new List<ReceiptStringItemCell> (){ dateCell, amountCell };

		
			ReceiptTableViewSource tableSource = new ReceiptTableViewSource (CellsToShow.ToArray ());
			ReceiptTableView.Source = tableSource;
			ReceiptTableView.ScrollEnabled = false;
			float height = tableSource.GetTableHeight ();


			TableVIewHeight.Constant = height;
		}
	}
}

