using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreFoundation;
using JudoPayDotNet.Models;
using CoreGraphics;

namespace JudoDotNetXamariniOSSDK
{
	public partial class TokenPaymentView : UIViewController
	{
		IPaymentService _paymentService;
		bool KeyboardVisible = false;
		IErrorPresenter errorPresenter;
		public TokenPaymentView (IPaymentService paymentService) : base ("TokenPaymentView", null)
		{
			_paymentService = paymentService;
			errorPresenter = new ResponseErrorPresenter ();
		}

		TokenPaymentCell tokenCell;

		private List<CardCell> CellsToShow { get; set; }

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.View.Superview.Bounds = new CGRect (0, 0, 320f, 460f);
			}

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			TableView.SeparatorColor = UIColor.Clear;

			if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad) {
				NSNotificationCenter defaultCenter = NSNotificationCenter.DefaultCenter;
				defaultCenter.AddObserver (UIKeyboard.WillHideNotification, OnKeyboardNotification);
				defaultCenter.AddObserver (UIKeyboard.WillShowNotification, OnKeyboardNotification);
			}

			if (JudoConfiguration.Instance.CardToken == null) {

				DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {						

					UIAlertView _error = new UIAlertView ("Missing Token", "No Card Token found. Please provide application with token via Pre-Authentication or Payment", null, "ok", null);
					_error.Show ();

					_error.Clicked += (sender, args) => {
						PaymentButton.Alpha = 0.25f;
						PaymentButton.Enabled = false;
						if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
							this.DismissViewController(true,null);
						}
						else
						{
						this.NavigationController.PopToRootViewController (true);
						}
					};

				});
			} else {

				SetUpTableView ();

				UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer ();

				tapRecognizer.AddTarget (() => { 
					if (KeyboardVisible) {
						DismissKeyboardAction ();
					}
				});

				tapRecognizer.NumberOfTapsRequired = 1;
				tapRecognizer.NumberOfTouchesRequired = 1;

				EncapsulatingView.AddGestureRecognizer (tapRecognizer);
				PaymentButton.Alpha = 0.25f;
				PaymentButton.Enabled = false;


				PaymentButton.TouchUpInside += (sender, ev) => {
					MakeTokenPayment ();
				};

				if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
					FormClose.TouchUpInside += (sender, ev) => {
						this.DismissViewController(true,null);
					};
				}
			}
		}

		private void OnKeyboardNotification (NSNotification notification)
		{
			KeyboardVisible = notification.Name == UIKeyboard.WillShowNotification;
		}

		void DismissKeyboardAction ()
		{
			tokenCell.DismissKeyboardAction ();
		}

		void SetUpTableView ()
		{
			tokenCell = new TokenPaymentCell (new IntPtr ());

			tokenCell = (TokenPaymentCell)tokenCell.Create ();


			tokenCell.UpdateUI = () => {
				UpdateUI ();
			};
				
			CellsToShow = new List<CardCell> (){ tokenCell };

			CardCellSource tableSource = new CardCellSource (CellsToShow);
			TableView.Source = tableSource;
		}

		private void UpdateUI ()
		{	
			PaymentButton.Enabled = tokenCell.Complete;
			PaymentButton.Alpha = (tokenCell.Complete == true ? 1f : 0.25f) ;
			if (tokenCell.Complete) {
				DismissKeyboardAction ();
			}
		}

		public void MakeTokenPayment ()
		{
			var instance = JudoConfiguration.Instance;
			TokenOperationViewModel tokenPayment = new TokenOperationViewModel () {
				ConsumerToken = instance.ConsumerToken,
				CV2 = tokenCell.CCV,
				Token = instance.CardToken,
				Amount = "6.66",
			};
			PaymentButton.Alpha = 0.25f;
			PaymentButton.Enabled = false;

			_paymentService.MakeTokenPayment (tokenPayment).ContinueWith (reponse => {
				var result = reponse.Result;
				if (result!=null&&!result.HasError&&result.Response.Result!="Declined") {
					PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;
					PaymentReceiptViewModel receipt = new PaymentReceiptViewModel () {
						CreatedAt = paymentreceipt.CreatedAt.DateTime,
						Currency = paymentreceipt.Currency,
						OriginalAmount = paymentreceipt.Amount,
						ReceiptId = paymentreceipt.ReceiptId,
						Message = "Token Payment Success"
					};

					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
						PaymentButton.Alpha = 0.25f;
						PaymentButton.Enabled = false;

						tokenCell.CleanUp ();
						var view = JudoSDKManager.GetReceiptView (receipt);
						this.NavigationController.PushViewController (view, true);	
					});
				} else {
					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {						
						errorPresenter.DisplayError(result,"Token Payment has failed");	
						PaymentButton.Alpha = 1f;
						PaymentButton.Enabled = true;
					});
				}
			});

		}

	}
}

