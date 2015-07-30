using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation;
using JudoDotNetXamariniOSSDK;
//using JudoDotNetXamariniOSSDK;
//using JudoDotNetXamarinSDK.Utils;
using UIKit;

namespace JudoPayiOSXamarinSampleApp
{
    public partial class RootViewController : UIViewController
    {
        // Configure your JudoID and payment detail
        private const string ApiToken   = "4eVWyZQnO5DyaXZy";
        private const string ApiSecret  = "1d5e8381ed9ef3cc1ecc1daaf8ce550bdc97ea058ac804be4b68c28d02fdb791";
        private string MY_JUDO_ID       = "100016";
        private string currency         = "GBP";
        private decimal amount           = 4.99m;
        private string paymentReference = "payment101010102";
        private string consumerRef      = "consumer1010102";

        private const int ACTION_CARD_PAYMENT   = 101;
        private const int ACTION_TOKEN_PAYMENT  = 102;
        private const int ACTION_PREAUTH        = 201;
        private const int ACTION_TOKEN_PREAUTH  = 202;
        private const int ACTION_REGISTER_CARD  = 301;

        //private volatile CardBase.CardType preAuth_cardType;

        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public RootViewController(IntPtr handle)
            : base(handle)
        {
			
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

        partial void payCard_TouchUpInside(UIButton sender)
        {
            // Optional: Supply meta data about this transaction, pass as last argument instead of null.
            Dictionary<string, string> metaData = new Dictionary<string, string> { { "test1", "test2" } };


            JudoSDKManager.MakeAPayment( amount, MY_JUDO_ID, paymentReference, consumerRef, metaData, this, SuccessResult, FailedResult);
        }

        private static void SuccessResult(string message)
        {
        }

        private static void FailedResult(string message)
        {
        }
    }
}