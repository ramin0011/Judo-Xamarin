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
		UIKit.UILabel prefix { get; set; }

		[Outlet]
		UIKit.UILabel suffix { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (prefix != null) {
				prefix.Dispose ();
				prefix = null;
			}

			if (suffix != null) {
				suffix.Dispose ();
				suffix = null;
			}
		}
	}
}
