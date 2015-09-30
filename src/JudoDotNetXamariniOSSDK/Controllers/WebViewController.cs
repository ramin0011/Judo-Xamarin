using System;
using System.Drawing;
using System.Text;

#if__UNIFIED__
using Foundation;
using UIKit;
using CoreFoundation;
using CoreGraphics;
// Mappings Unified CoreGraphic classes to MonoTouch classes
using RectangleF = global::CoreGraphics.CGRect;
using SizeF = global::CoreGraphics.CGSize;
using PointF = global::CoreGraphics.CGPoint;
using NSUrlRequest = global:: Foundation.NSURLRequest

#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
// Mappings Unified types to MonoTouch types
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;

#endif


namespace JudoDotNetXamariniOSSDK
{
    [Register("UniversalView")]
    public class UniversalView : UIView
    {
        public UniversalView()
        {
            Initialize();
        }

        public UniversalView(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.Red;
        }
    }

    [Register("WebViewController")]
    public class WebViewController : UIViewController
    {
        private UIWebView webView;
        public SuccessBlock successBlock;
        public FailureBlock failureBlock;

        public WebViewController()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            View = new UniversalView();

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }
										
		public bool shouldStartLoadWithRequest(NSUrlRequest request, UIWebViewNavigationType navigationType)
        { 
            string urlString = request.Url.AbsoluteString;
    
            if (urlString.Contains(@"threedsecurecallback")) {
                NSData body = request.HttpMethod;
			
                string bodyString= body.EncodeTo(NSUTF8StringEncoding);
        
                NSMutableDictionary results = NSMutableDictionary.FromDictionary(dictionary);
                Array pairs = bodyString.Split("&");
        
                foreach (string pair in pairs) {
                    if (pair.Contains("=")) {
                        Array components = pair.Split("=");
                        string value = components[1];//[components objectAtIndex:1]; 
                        string escapedVal = ""; //(__bridge_transfer NSString *)CFURLCreateStringByReplacingPercentEscapes(kCFAllocatorDefault, (CFStringRef)value, CFSTR("")); // what is it????
                
                        results.SetValueForKey(escapedVal, components[0]);
                    }
                }
                
				Console.WriteLine ("results :{0}", results);
        
                if(this.successBlock) {
                    this.successBlock(200, results);
                }
        
                return false;
            }
    
            return true;
        }
    }
}