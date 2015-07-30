// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JudoDotNetXamariniOSSDK
{
	[Register ("PaymentReceipt")]
	partial class PaymentReceipt
	{
		[Outlet]
		UIKit.UIButton HomeButton { get; set; }

		[Outlet]
		UIKit.UITableView ReceiptTableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ReceiptTableView != null) {
				ReceiptTableView.Dispose ();
				ReceiptTableView = null;
			}

			if (HomeButton != null) {
				HomeButton.Dispose ();
				HomeButton = null;
			}
		}
	}
}
