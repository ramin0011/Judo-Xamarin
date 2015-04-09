using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;

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

        public bool shouldStartLoadWithRequest(NSURLRequest request, UIWebViewNavigationType navigationType)
        { 
            string urlString = request.URL.absoluteString;
    
            if (urlString.Contains(@"threedsecurecallback")) {
                NSData body = request.HTTPBody;
                string bodyString body.EncodeTo(NSUTF8StringEncoding);
        
                NSMutableDictionary results = NSMutableDictionary.FromDictionary(dictionary);
                Array pairs = bodyString.Split("&");
        
                for (string pair in pairs) {
                    if (pair.Contains("=")) {
                        Array components = pair.Split("=");
                        string value = components[1];//[components objectAtIndex:1]; 
                        string escapedVal = ""; //(__bridge_transfer NSString *)CFURLCreateStringByReplacingPercentEscapes(kCFAllocatorDefault, (CFStringRef)value, CFSTR("")); // what is it????
                
                        results.SetValueForKey(escapedVal, components[0]);
                    }
                }
                
                Console.WriteLine("results :{0}", results)
                //DDLogVerbose("results: ", results);
        
                if (this.successBlock) {
                    this.successBlock(200, results);
                }
        
                return false;
            }
    
            return true;
        }
    }
}