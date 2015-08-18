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
		UIKit.UIButton MakeAPaymentButton { get; set; }

		[Outlet]
		UIKit.UIButton RegisterCardButton { get; set; }

		[Outlet]
		UIKit.UIButton TokenPaymentButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TokenPaymentButton != null) {
				TokenPaymentButton.Dispose ();
				TokenPaymentButton = null;
			}

			if (MakeAPaymentButton != null) {
				MakeAPaymentButton.Dispose ();
				MakeAPaymentButton = null;
			}

			if (RegisterCardButton != null) {
				RegisterCardButton.Dispose ();
				RegisterCardButton = null;
			}
		}
	}
}
