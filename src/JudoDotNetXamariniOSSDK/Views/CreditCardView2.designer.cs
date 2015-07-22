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
		UIKit.UIImageView ccImage { get; set; }

		[Outlet]
		UIKit.UITextView ccText { get; set; }

		[Outlet]
		UIKit.UITableViewCell MaestroCell { get; set; }

		[Outlet]
		UIKit.UITableViewCell PayCell { get; set; }

		[Outlet]
		UIKit.UITableViewCell ReassuringTextCell { get; set; }

		[Outlet]
		UIKit.UITableViewCell SpacerCell { get; set; }

		[Outlet]
		UIKit.UIButton SubmitButton { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ccText != null) {
				ccText.Dispose ();
				ccText = null;
			}

			if (ccImage != null) {
				ccImage.Dispose ();
				ccImage = null;
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

			if (MaestroCell != null) {
				MaestroCell.Dispose ();
				MaestroCell = null;
			}

			if (PayCell != null) {
				PayCell.Dispose ();
				PayCell = null;
			}

			if (ReassuringTextCell != null) {
				ReassuringTextCell.Dispose ();
				ReassuringTextCell = null;
			}

			if (SpacerCell != null) {
				SpacerCell.Dispose ();
				SpacerCell = null;
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
