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
		
		void ReleaseDesignerOutlets ()
		{
			if (MakeAPaymentButton != null) {
				MakeAPaymentButton.Dispose ();
				MakeAPaymentButton = null;
			}
		}
	}
}
