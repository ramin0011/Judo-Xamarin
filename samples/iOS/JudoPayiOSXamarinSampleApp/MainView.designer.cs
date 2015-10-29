// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JudoPayiOSXamarinSampleApp
{
	[Register ("MainView")]
	partial class MainView
	{
		[Outlet]
		UIKit.NSLayoutConstraint HeightConstraint { get; set; }

		[Outlet]
		UIKit.UITableView OptionTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (OptionTable != null) {
				OptionTable.Dispose ();
				OptionTable = null;
			}

			if (HeightConstraint != null) {
				HeightConstraint.Dispose ();
				HeightConstraint = null;
			}
		}
	}
}
