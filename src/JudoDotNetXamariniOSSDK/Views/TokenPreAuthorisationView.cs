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
	public partial class TokenPreAuthorisationView : UIViewController
	{
		IPaymentService _paymentService;
		bool KeyboardVisible = false;
		IErrorPresenter errorPresenter;
		public TokenPreAuthorisationView (IPaymentService paymentService) : base ("TokenPreAuthorisationView", null)
		{
			_paymentService = paymentService;
			errorPresenter = new ResponseErrorPresenter ();
		}

		TokenPaymentCell tokenCell;

		private List<CardCell> CellsToShow { get; set; }

        public SuccessCallback successCallback { get; set; }
        public FailureCallback failureCallback { get; set; }
        public TokenPaymentViewModel tokenPayment { get; set; }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			TableView.SeparatorColor = UIColor.Clear;

			NSNotificationCenter defaultCenter = NSNotificationCenter.DefaultCenter;
			defaultCenter.AddObserver (UIKeyboard.WillHideNotification, OnKeyboardNotification);
			defaultCenter.AddObserver (UIKeyboard.WillShowNotification, OnKeyboardNotification);

			if (JudoConfiguration.Instance.CardToken == null) {

				DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {						

					UIAlertView _error = new UIAlertView ("Missing Token", "No Card Token found. Please provide application with token via Pre-Authentication or Payment", null, "ok", null);
					_error.Show ();

					_error.Clicked += (sender, args) => {
						PaymentButton.Alpha = 0.25f;
						PaymentButton.Enabled = false;
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
				PaymentButton.Alpha = 0.25f;
				PaymentButton.Enabled = false;

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
		    try
		    {
                var instance = JudoConfiguration.Instance;
		        
		        tokenPayment.ConsumerToken = instance.ConsumerToken;
                tokenPayment.CV2 = tokenCell.CCV;
                tokenPayment.Token = instance.CardToken;

                PaymentButton.Alpha = 0.25f;
                PaymentButton.Enabled = false;

                PaymentButton.Alpha = 0.25f;
                PaymentButton.Enabled = false;

                _paymentService.MakeTokenPreAuthorisation(tokenPayment).ContinueWith(reponse =>
                {
                    var result = reponse.Result;
                    if (result != null && !result.HasError && result.Response.Result != "Declined")
                    {
                        PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;
                        PaymentReceiptViewModel receipt = new PaymentReceiptViewModel()
                        {
                            CreatedAt = paymentreceipt.CreatedAt.DateTime,
                            Currency = paymentreceipt.Currency,
                            OriginalAmount = paymentreceipt.Amount,
                            ReceiptId = paymentreceipt.ReceiptId,
                            Message = "Pre Authorisation Success"
                        };

                        DispatchQueue.MainQueue.DispatchAfter(DispatchTime.Now, () =>
                        {
                            PaymentButton.Alpha = 0.25f;
                            PaymentButton.Enabled = false;
                            tokenCell.CleanUp();
                            //var view = ViewLocator.GetReceiptView (receipt);
                            //this.NavigationController.PushViewController (view, true);	
                        });
                    }
                    else
                    {
                        DispatchQueue.MainQueue.DispatchAfter(DispatchTime.Now, () =>
                        {
                            errorPresenter.DisplayError(result, "Token Pre Authorisation has failed");
                            PaymentButton.Alpha = 1f;
                            PaymentButton.Enabled = true;
                        });

                        // Failure callback
                        if (failureCallback != null)
                        {
                            var judoError = new JudoError { ApiError = result != null ? result.Error : null };
                            failureCallback(judoError);
                        }

                    }
                });
		    }
            catch (Exception ex)
            {
                // Failure callback
                if (failureCallback != null)
                {
                    var judoError = new JudoError { Exception = ex };
                    failureCallback(judoError);
                }
            }


		}

	}
}

