
using Foundation;
using UIKit;

#if __UNIFIED__
// Mappings Unified CoreGraphic classes to MonoTouch classes

#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
using MonoTouch.CoreAnimation;
// Mappings Unified types to MonoTouch types
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif


namespace JudoDotNetXamariniOSSDK.Views
{
    [Register ("TokenPaymentView")]
    partial class TokenPaymentView
    {
        [Outlet]
        UIKit.UIView EncapsulatingView { get; set; }

        [Outlet]
        UIKit.UIButton FormClose { get; set; }

        [Outlet]
        UIKit.UIButton PaymentButton { get; set; }

        [Outlet]
        JudoDotNetXamariniOSSDK.Controllers.SecureWebView SecureWebView { get; set; }

        [Outlet]
        UIKit.UITableView TableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (EncapsulatingView != null) {
                EncapsulatingView.Dispose ();
                EncapsulatingView = null;
            }

            if (FormClose != null) {
                FormClose.Dispose ();
                FormClose = null;
            }

            if (PaymentButton != null) {
                PaymentButton.Dispose ();
                PaymentButton = null;
            }

            if (SecureWebView != null) {
                SecureWebView.Dispose ();
                SecureWebView = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}
