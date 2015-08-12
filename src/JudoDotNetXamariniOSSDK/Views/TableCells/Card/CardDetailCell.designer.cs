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
	partial class CardDetailCell
	{
		[Outlet]
		UIKit.UITextView ccText { get; set; }

		[Outlet]
		UIKit.UIView containerView { get; set; }

		[Outlet]
		UIKit.UIImageView creditCardImage { get; set; }

		[Outlet]
		UIKit.UIButton ExpiryInfoButton { get; set; }

		[Outlet]
		UIKit.UILabel PaymentErrorLabel { get; set; }

		[Outlet]
		JudoDotNetXamariniOSSDK.PlaceHolderTextView placeView { get; set; }

		[Outlet]
		UIKit.UILabel StatusHelpLabel { get; set; }

		[Outlet]
		UIKit.UIScrollView textScroller { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ccText != null) {
				ccText.Dispose ();
				ccText = null;
			}

			if (containerView != null) {
				containerView.Dispose ();
				containerView = null;
			}

			if (creditCardImage != null) {
				creditCardImage.Dispose ();
				creditCardImage = null;
			}

			if (ExpiryInfoButton != null) {
				ExpiryInfoButton.Dispose ();
				ExpiryInfoButton = null;
			}

			if (PaymentErrorLabel != null) {
				PaymentErrorLabel.Dispose ();
				PaymentErrorLabel = null;
			}

			if (placeView != null) {
				placeView.Dispose ();
				placeView = null;
			}

			if (StatusHelpLabel != null) {
				StatusHelpLabel.Dispose ();
				StatusHelpLabel = null;
			}

			if (textScroller != null) {
				textScroller.Dispose ();
				textScroller = null;
			}
		}
	}
}
