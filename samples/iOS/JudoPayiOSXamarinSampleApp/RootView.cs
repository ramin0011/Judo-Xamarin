
using System;

using Foundation;
using UIKit;
using JudoDotNetXamariniOSSDK;

namespace JudoPayiOSXamarinSampleApp
{
	public partial class RootView : UIViewController
	{
		public RootView () : base ("RootView", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			MakeAPaymentButton.TouchUpInside += (sender, ev) => {
				
				var creditCardView =JudoSDKManager.GetPaymentView();
				this.NavigationController.PushViewController(creditCardView,true);
			};
		}
	}
}

