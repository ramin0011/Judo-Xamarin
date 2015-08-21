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

		public TokenPaymentView (IPaymentService paymentService) : base ("TokenPaymentView", null)
		{
			_paymentService = paymentService;
		}

		TokenPaymentCell tokenCell;

		private List<CardCell> CellsToShow { get; set; }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			TableView.SeparatorColor = UIColor.Clear;

			NSNotificationCenter defaultCenter = NSNotificationCenter.DefaultCenter;
			defaultCenter.AddObserver (UIKeyboard.WillHideNotification, OnKeyboardNotification);
			defaultCenter.AddObserver (UIKeyboard.WillShowNotification, OnKeyboardNotification);

			if (JudoConfiguration.Instance.CardToken == null) {

				DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {						

					UIAlertView _error = new UIAlertView ("Missing Token", "No Card Token found. Please provide application with token via Pre-Authentication", null, "ok", null);
					_error.Show ();

					_error.Clicked += (sender, args) => {
						PaymentButton.Hidden = true;
						this.NavigationController.PopToRootViewController (true);
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

				PaymentButton.SetTitleColor (UIColor.Black, UIControlState.Application);

				PaymentButton.TouchUpInside += (sender, ev) => {
					MakeTokenPayment ();
				};
			}
		}

		private void OnKeyboardNotification (NSNotification notification)
		{
			KeyboardVisible = notification.Name == UIKeyboard.WillShowNotification;
		}

		void DismissKeyboardAction ()
		{
			tokenCell.CCVEntryOutlet.ResignFirstResponder ();
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
			PaymentButton.Hidden = !tokenCell.Complete;
			if (tokenCell.Complete) {
				DismissKeyboardAction ();
			}
		}

		public void MakeTokenPayment ()
		{
			var instance = JudoConfiguration.Instance;
			TokenPaymentViewModel tokenPayment = new TokenPaymentViewModel () {
				ConsumerToken = instance.ConsumerToken,
				CV2 = tokenCell.CCV,
				Token = instance.CardToken,
				Amount = "6.66",
			};
			PaymentButton.Hidden = true;

			_paymentService.MakeTokenPayment (tokenPayment).ContinueWith (reponse => {
				var result = reponse.Result;
				if (result != null || !result.HasError) {
					PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;
					PaymentReceiptViewModel receipt = new PaymentReceiptViewModel () {
						CreatedAt = paymentreceipt.CreatedAt.DateTime,
						Currency = paymentreceipt.Currency,
						OriginalAmount = paymentreceipt.Amount,
						ReceiptId = paymentreceipt.ReceiptId,
						Message = "Token Payment Success"
					};

					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
						PaymentButton.Hidden = false;
						tokenCell.CleanUp ();
						var view = JudoSDKManager.GetReceiptView (receipt);
						this.NavigationController.PushViewController (view, true);	
					});
				} else {
					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {						
						var errorText = result.Error.ErrorMessage;
						UIAlertView _error = new UIAlertView ("Token Payment has failed", errorText, null, "ok", null);
						_error.Show ();
						PaymentButton.Hidden = false;
					});
				}
			});

		}

	}
}

