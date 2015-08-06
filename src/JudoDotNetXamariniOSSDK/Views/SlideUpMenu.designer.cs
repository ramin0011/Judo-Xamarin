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
	partial class SlideUpMenu
	{
		[Outlet]
		UIKit.UISwitch AmexSwitch { get; set; }

		[Outlet]
		UIKit.UIImageView ArrowIcon { get; set; }

		[Outlet]
		UIKit.UISwitch AVSSwitch { get; set; }

		[Outlet]
		UIKit.UISwitch MaestroSwitch { get; set; }

		[Outlet]
		UIKit.UISwitch RiskSwitch { get; set; }

		[Outlet]
		UIKit.UISwitch ThreeDSwitch { get; set; }

		[Outlet]
		UIKit.UIView View { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ArrowIcon != null) {
				ArrowIcon.Dispose ();
				ArrowIcon = null;
			}

			if (AmexSwitch != null) {
				AmexSwitch.Dispose ();
				AmexSwitch = null;
			}

			if (AVSSwitch != null) {
				AVSSwitch.Dispose ();
				AVSSwitch = null;
			}

			if (MaestroSwitch != null) {
				MaestroSwitch.Dispose ();
				MaestroSwitch = null;
			}

			if (RiskSwitch != null) {
				RiskSwitch.Dispose ();
				RiskSwitch = null;
			}

			if (ThreeDSwitch != null) {
				ThreeDSwitch.Dispose ();
				ThreeDSwitch = null;
			}

			if (View != null) {
				View.Dispose ();
				View = null;
			}
		}
	}
}
