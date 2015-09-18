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
	[Register ("CreditCardView")]
	partial class CreditCardView
	{
		[Outlet]
		UIKit.UIView EncapsulatingView { get; set; }

		[Outlet]
		UIKit.UIButton FormClose { get; set; }

		[Outlet]
		UIKit.UIWebView SecureWebView { get; set; }

		[Outlet]
		UIKit.UIButton SubmitButton { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SecureWebView != null) {
				SecureWebView.Dispose ();
				SecureWebView = null;
			}

			if (EncapsulatingView != null) {
				EncapsulatingView.Dispose ();
				EncapsulatingView = null;
			}

			if (FormClose != null) {
				FormClose.Dispose ();
				FormClose = null;
			}

			if (SubmitButton != null) {
				SubmitButton.Dispose ();
				SubmitButton = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
