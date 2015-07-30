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
	[Register ("ReceiptStringItemCell")]
	partial class ReceiptStringItemCell
	{
		[Outlet]
		UIKit.UILabel ItemLabel { get; set; }

		[Outlet]
		UIKit.UILabel ItemValue { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ItemLabel != null) {
				ItemLabel.Dispose ();
				ItemLabel = null;
			}

			if (ItemValue != null) {
				ItemValue.Dispose ();
				ItemValue = null;
			}
		}
	}
}
