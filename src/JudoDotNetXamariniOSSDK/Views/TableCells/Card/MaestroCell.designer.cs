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
	[Register ("MaestroCell")]
	partial class MaestroCell
	{
		[Outlet]
		UIKit.UITextField IssueNumberTextField { get; set; }

		[Outlet]
		UIKit.UILabel StartDatePlaceHolder { get; set; }

		[Outlet]
		UIKit.UITextField StartDateTextField { get; set; }

		[Outlet]
		UIKit.UILabel StartDateWarningLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (IssueNumberTextField != null) {
				IssueNumberTextField.Dispose ();
				IssueNumberTextField = null;
			}

			if (StartDateTextField != null) {
				StartDateTextField.Dispose ();
				StartDateTextField = null;
			}

			if (StartDatePlaceHolder != null) {
				StartDatePlaceHolder.Dispose ();
				StartDatePlaceHolder = null;
			}

			if (StartDateWarningLabel != null) {
				StartDateWarningLabel.Dispose ();
				StartDateWarningLabel = null;
			}
		}
	}
}
