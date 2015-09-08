
using System;

using Foundation;
using UIKit;
using JudoDotNetXamariniOSSDK;
using System.Drawing;

namespace JudoPayiOSXamarinSampleApp
{
	public partial class RootView : UIViewController
	{
		SlideUpMenu menu;
		public RootView () : base ("RootView", null)
		{
			
		}
		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			MakeAPaymentButton.TouchUpInside += (sender, ev) => {				
				var creditCardView =JudoSDKManager.GetPaymentView();
				this.NavigationController.PushViewController(creditCardView,true);
			};

			RegisterCardButton.TouchUpInside += (sender, ev) => {				
				var registerCardView =JudoSDKManager.GetPreAuthView();
				this.NavigationController.PushViewController(registerCardView,true);
			};
			TokenPaymentButton.TouchUpInside += (sender, ev) => {				
				var tokenPaymentView =JudoSDKManager.GetTokenPaymentView();
				this.NavigationController.PushViewController(tokenPaymentView,true);
			};

			TokenPreauthButton.TouchUpInside += (sender, ev) => {				
				var tokenPreAuth =JudoSDKManager.GetTokenPreAuthView();
				this.NavigationController.PushViewController(tokenPreAuth,true);
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		 	menu = new SlideUpMenu (new RectangleF(0,(float)this.View.Frame.Bottom-40f,(float)this.View.Frame.Width,248f));
			menu.AwakeFromNib ();
			menu.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			this.View.AddSubview (menu);
		}

		public override void ViewWillDisappear (bool animated)
		{
			menu.RemoveFromSuperview ();
			base.ViewWillDisappear (animated);
		}

		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);
			menu.ResetMenu ();
		}
	}
}

