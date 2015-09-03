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
	[Register ("CardEntryCell")]
	partial class CardEntryCell
	{
		[Outlet]
		JudoDotNetXamariniOSSDK.PlaceHolderTextView ccPlaceHolder { get; set; }

		[Outlet]
		UIKit.UITextView ccText { get; set; }

		[Outlet]
		UIKit.UIView containerView { get; set; }

		[Outlet]
		UIKit.UIImageView creditCardImage { get; set; }

		[Outlet]
		JudoDotNetXamariniOSSDK.PlaceHolderTextView cvTwoPlaceHolder { get; set; }

		[Outlet]
		UIKit.UITextView cvTwoText { get; set; }

		[Outlet]
		UIKit.UIButton ExpiryInfoButton { get; set; }

		[Outlet]
		JudoDotNetXamariniOSSDK.PlaceHolderTextView expiryPlaceHolder { get; set; }

		[Outlet]
		UIKit.UITextView expiryText { get; set; }

		[Outlet]
		UIKit.UILabel PaymentErrorLabel { get; set; }

		[Outlet]
		UIKit.UILabel StatusHelpLabel { get; set; }

		[Outlet]
		UIKit.UIScrollView textScroller { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (cvTwoPlaceHolder != null) {
				cvTwoPlaceHolder.Dispose ();
				cvTwoPlaceHolder = null;
			}

			if (ccPlaceHolder != null) {
				ccPlaceHolder.Dispose ();
				ccPlaceHolder = null;
			}

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

			if (cvTwoText != null) {
				cvTwoText.Dispose ();
				cvTwoText = null;
			}

			if (ExpiryInfoButton != null) {
				ExpiryInfoButton.Dispose ();
				ExpiryInfoButton = null;
			}

			if (expiryPlaceHolder != null) {
				expiryPlaceHolder.Dispose ();
				expiryPlaceHolder = null;
			}

			if (expiryText != null) {
				expiryText.Dispose ();
				expiryText = null;
			}

			if (PaymentErrorLabel != null) {
				PaymentErrorLabel.Dispose ();
				PaymentErrorLabel = null;
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
