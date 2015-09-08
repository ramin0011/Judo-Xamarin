using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
    [Register("ExpiryInfoVC")]
    class ExpiryInfoVC : UIViewController
    {
        public UILabel[] labelCollection { get; set; }

        ExpiryInfoVC()
        { 
            // need to call base class init Method
            base.Init();
        }

        public bool prefersStatusBarHidden()
        {
            return false;
        }

        public override void ViewDidLoad()
        {
            this.Title = ThemeBundleReplacement.BundledOrReplacementString("cardEntry", BundledOrReplacementOptions.BundledOrReplacement);
            UIImage patternImage = ThemeBundleReplacement.BundledOrReplacementImage("bg_light_iPhone5", BundledOrReplacementOptions.BundledOrReplacement);
            if (patternImage != null) this.View.BackgroundColor = UIColor.FromPatternImage(patternImage);

            this.TabBarController.TabBar.Hidden = true;

            base.ViewDidLoad();
        }

        public override void DidReceiveMemoryWarning()
        {
            this.DidReceiveMemoryWarning();
            // Dispose of any resources that can be recreated.
        }
    }
}