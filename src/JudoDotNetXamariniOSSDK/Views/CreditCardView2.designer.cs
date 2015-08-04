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
	[Register ("CreditCardView2")]
	partial class CreditCardView2
	{
		[Outlet]
		UIKit.UITableViewCell AVSCell { get; set; }

		[Outlet]
		UIKit.UIButton CancelButton { get; set; }

		[Outlet]
		UIKit.UITableViewCell CardDetailCell { get; set; }

		[Outlet]
		UIKit.UITextView ccText { get; set; }

		[Outlet]
		UIKit.UIView containerView { get; set; }

		[Outlet]
		UIKit.UIImageView creditCardImage { get; set; }

		[Outlet]
		UIKit.UIButton ExpiryInfoButton { get; set; }

		[Outlet]
		UIKit.UITableViewCell MaestroCell { get; set; }

		[Outlet]
		UIKit.UITableViewCell PayCell { get; set; }

		[Outlet]
		UIKit.UILabel PaymentErrorLabel { get; set; }

		[Outlet]
		JudoDotNetXamariniOSSDK.PlaceHolderTextView placeView { get; set; }

		[Outlet]
		UIKit.UITableViewCell ReassuringTextCell { get; set; }

		[Outlet]
		UIKit.UITableViewCell SpacerCell { get; set; }

		[Outlet]
		UIKit.UILabel StatusHelpLabel { get; set; }

		[Outlet]
		UIKit.UIButton SubmitButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint TableHeight { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Outlet]
		UIKit.UIScrollView textScroller { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ExpiryInfoButton != null) {
				ExpiryInfoButton.Dispose ();
				ExpiryInfoButton = null;
			}

			if (AVSCell != null) {
				AVSCell.Dispose ();
				AVSCell = null;
			}

			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (CardDetailCell != null) {
				CardDetailCell.Dispose ();
				CardDetailCell = null;
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

			if (MaestroCell != null) {
				MaestroCell.Dispose ();
				MaestroCell = null;
			}

			if (PayCell != null) {
				PayCell.Dispose ();
				PayCell = null;
			}

			if (PaymentErrorLabel != null) {
				PaymentErrorLabel.Dispose ();
				PaymentErrorLabel = null;
			}

			if (placeView != null) {
				placeView.Dispose ();
				placeView = null;
			}

			if (ReassuringTextCell != null) {
				ReassuringTextCell.Dispose ();
				ReassuringTextCell = null;
			}

			if (SpacerCell != null) {
				SpacerCell.Dispose ();
				SpacerCell = null;
			}

			if (StatusHelpLabel != null) {
				StatusHelpLabel.Dispose ();
				StatusHelpLabel = null;
			}

			if (SubmitButton != null) {
				SubmitButton.Dispose ();
				SubmitButton = null;
			}

			if (TableHeight != null) {
				TableHeight.Dispose ();
				TableHeight = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}

			if (textScroller != null) {
				textScroller.Dispose ();
				textScroller = null;
			}
		}
	}
}
