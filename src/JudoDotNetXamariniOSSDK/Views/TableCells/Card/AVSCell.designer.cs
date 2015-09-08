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
	[Register ("AVSCell")]
	partial class AVSCell
	{
		[Outlet]
		UIKit.UIButton CountryButton { get; set; }

		[Outlet]
		UIKit.UILabel CountryLabel { get; set; }

		[Outlet]
		UIKit.UIButton HomeButton { get; set; }

		[Outlet]
		UIKit.UIView PostCodeCountainerView { get; set; }

		[Outlet]
		UIKit.UITextField PostcodeTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CountryButton != null) {
				CountryButton.Dispose ();
				CountryButton = null;
			}

			if (CountryLabel != null) {
				CountryLabel.Dispose ();
				CountryLabel = null;
			}

			if (PostCodeCountainerView != null) {
				PostCodeCountainerView.Dispose ();
				PostCodeCountainerView = null;
			}

			if (PostcodeTextField != null) {
				PostcodeTextField.Dispose ();
				PostcodeTextField = null;
			}

			if (HomeButton != null) {
				HomeButton.Dispose ();
				HomeButton = null;
			}
		}
	}
}
