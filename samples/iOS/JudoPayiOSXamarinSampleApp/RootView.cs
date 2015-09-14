
using System;

using Foundation;
using UIKit;
using JudoDotNetXamariniOSSDK;
using System.Drawing;
using System.Collections.Generic;
using CoreGraphics;

namespace JudoPayiOSXamarinSampleApp
{
    public partial class RootView : UIViewController
    {
        SlideUpMenu menu;
        public RootView()
            : base("RootView", null)
        {

        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetUpTableView();

            UILabel label = new UILabel(new CGRect(0, 0, 120f, 30f));
            label.TextAlignment = UITextAlignment.Center;
            label.Font = UIFont.FromName("Courier", 17.0f);
            label.BackgroundColor = UIColor.Clear;

            label.Text = "Judo Sample App";
            this.NavigationController.NavigationBar.TopItem.TitleView = label;

        }

        private void successPayment(PaymentReceiptViewModel receipt)
        {
            // move back to home screen
            // show receipt
            // store token to further use
        }

        private void failurePayment(JudoError error)
        {
            // move back to home screen
            // show failure receipt
            // show exceptions and API error
        }

        void SetUpTableView()
        {
            UITableViewCell cell = new UITableViewCell();

            Dictionary<string, Action> buttonDictionary = new Dictionary<string, Action>();
            SuccessCallback successCallback = successPayment;
            FailureCallback failureCallback = failurePayment;

            var cardPayment = new PaymentViewModel { Amount = "1.1" };

            buttonDictionary.Add("Make a Payment", () =>
            {
                JudoSDKManager.Payment(cardPayment, successCallback, failureCallback, this.NavigationController);
            });

            buttonDictionary.Add("PreAuthorise", delegate
            {
                 var preAuthorisation = new PreAuthorisationViewModel { Amount = "1.1" };
                JudoSDKManager.PreAuth(preAuthorisation, successCallback, failureCallback, this.NavigationController);
            });

            buttonDictionary.Add("Token Payment", delegate
            {
                JudoSDKManager.TokenPayment(cardPayment, successCallback, failureCallback, this.NavigationController);
            });

            buttonDictionary.Add("Token PreAuthorise", delegate
            {
                JudoSDKManager.TokenPreAuth(cardPayment, successCallback, failureCallback, this.NavigationController);
            });

            MainMenuSource menuSource = new MainMenuSource(buttonDictionary);
            ButtonTable.Source = menuSource;
            TableHeightConstrant.Constant = menuSource.GetTableHeight() + 60f;
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            menu = new SlideUpMenu(new RectangleF(0, (float)this.View.Frame.Bottom - 40f, (float)this.View.Frame.Width, 448f));
            menu.AwakeFromNib();
            menu.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
            this.View.AddSubview(menu);
        }

        public override void ViewWillDisappear(bool animated)
        {
            menu.RemoveFromSuperview();
            base.ViewWillDisappear(animated);
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            menu.ResetMenu();
        }
    }
}

