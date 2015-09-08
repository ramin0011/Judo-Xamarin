// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JudoPayiOSXamarinSampleApp
{
	[Register ("RootView")]
	partial class RootView
	{
		[Outlet]
		UIKit.UITableView ButtonTable { get; set; }

		[Outlet]
		UIKit.UIButton MakeAPaymentButton { get; set; }

		[Outlet]
		UIKit.UIButton RegisterCardButton { get; set; }

		[Outlet]
		UIKit.UIButton TokenPaymentButton { get; set; }

		[Outlet]
		UIKit.UIButton TokenPreauthButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonTable != null) {
				ButtonTable.Dispose ();
				ButtonTable = null;
			}

			if (MakeAPaymentButton != null) {
				MakeAPaymentButton.Dispose ();
				MakeAPaymentButton = null;
			}

			if (RegisterCardButton != null) {
				RegisterCardButton.Dispose ();
				RegisterCardButton = null;
			}

			if (TokenPaymentButton != null) {
				TokenPaymentButton.Dispose ();
				TokenPaymentButton = null;
			}

			if (TokenPreauthButton != null) {
				TokenPreauthButton.Dispose ();
				TokenPreauthButton = null;
			}
		}
	}
}
