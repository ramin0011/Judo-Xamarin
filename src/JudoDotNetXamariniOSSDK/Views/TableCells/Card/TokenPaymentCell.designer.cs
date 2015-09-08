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
	[Register ("TokenPaymentCell")]
	partial class TokenPaymentCell
	{
		[Outlet]
		UIKit.UIImageView cardImage { get; set; }

		[Outlet]
		UIKit.UIView EntryEnclosingView { get; set; }

		[Outlet]
		UIKit.UITextField entryField { get; set; }

		[Outlet]
		UIKit.UILabel PreviousCardNumber { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (EntryEnclosingView != null) {
				EntryEnclosingView.Dispose ();
				EntryEnclosingView = null;
			}

			if (cardImage != null) {
				cardImage.Dispose ();
				cardImage = null;
			}

			if (entryField != null) {
				entryField.Dispose ();
				entryField = null;
			}

			if (PreviousCardNumber != null) {
				PreviousCardNumber.Dispose ();
				PreviousCardNumber = null;
			}
		}
	}
}
