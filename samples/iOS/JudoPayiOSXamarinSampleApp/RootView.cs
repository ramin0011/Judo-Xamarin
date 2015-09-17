
using System;

using Foundation;
using UIKit;
using JudoDotNetXamariniOSSDK;
using System.Drawing;
using System.Collections.Generic;
using CoreFoundation;
using CoreGraphics;
using JudoPayDotNet.Models;

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

        private void ShowMessage(string title, string message, string btnText = "OK")
        {
            UIAlertView msgbox = new UIAlertView (title, message, null, btnText, null);
            msgbox.Show ();
        }

        private void SuccessPayment(PaymentReceiptModel receipt)
        {
            DispatchQueue.MainQueue.DispatchAfter(DispatchTime.Now, () =>
            {
                // move back to home screen

				CloseView ();
						
                // show receipt
                ShowMessage("Transaction Successful", "Receipt ID - " + receipt.ReceiptId);

                // store token to further use
            });
        }

        private void FailurePayment(JudoError error, PaymentReceiptModel receipt)
        {
            DispatchQueue.MainQueue.DispatchAfter(DispatchTime.Now, () =>
            {
                // move back to home screen
				CloseView();
                // show receipt
                string message = "";
                if (error != null && error.ApiError != null)
                    message += error.ApiError.ErrorMessage + Environment.NewLine;

                if (error != null && error.Exception != null)
                    message += error.Exception.Message + Environment.NewLine;

                if (receipt != null)
                {
                    message += "Transaction : " + receipt.Result + Environment.NewLine;
                    message += receipt.Message + Environment.NewLine;
                    message += "Receipt ID - " + receipt.ReceiptId;
                }

                ShowMessage("Transaction Failed: ", message);
                // store token to further use
            });
        }

        void SetUpTableView()
        {
            UITableViewCell cell = new UITableViewCell();

            Dictionary<string, Action> buttonDictionary = new Dictionary<string, Action>();
            SuccessCallback successCallback = SuccessPayment;
            FailureCallback failureCallback = FailurePayment;

            var cardPayment = new PaymentViewModel { Amount = "4.5" };
            var tokenPayment = new TokenPaymentViewModel { Amount = "3.4" };

            buttonDictionary.Add("Make a Payment", () =>
            {
                JudoSDKManager.Payment(cardPayment, successCallback, failureCallback, this.NavigationController);
            });

            buttonDictionary.Add("PreAuthorise", delegate
            {
                JudoSDKManager.PreAuth(cardPayment, successCallback, failureCallback, this.NavigationController);
            });

            buttonDictionary.Add("Token Payment", delegate
            {
                JudoSDKManager.TokenPayment(tokenPayment, successCallback, failureCallback, this.NavigationController);
            });

            buttonDictionary.Add("Token PreAuthorise", delegate
            {
                JudoSDKManager.TokenPreAuth(tokenPayment, successCallback, failureCallback, this.NavigationController);
            });

            buttonDictionary.Add("Register a Card", delegate
            {
                JudoSDKManager.RegisterCard(cardPayment, successCallback, failureCallback, this.NavigationController);
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

		void CloseView ()
		{
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				NavigationController.DismissViewController (true, null);
			}
			else {
				NavigationController.PopToRootViewController (true);
			}
		}
    }
}

