using System;
using UIKit;

namespace JudoDotNetXamariniOSSDK
{
    internal class JudoUIViewController :UIViewController
    {
        internal JudoUIViewController (string viewName) : base (viewName, null)
        {
        }

        internal void CloseView ()
        {
            this.DismissViewController (true, null);
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem ("Back", UIBarButtonItemStyle.Plain, (sender, args) => {
                CloseView ();
            });
        }
    }
}

