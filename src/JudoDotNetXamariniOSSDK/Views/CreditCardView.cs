using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreFoundation;
using JudoPayDotNet.Models;
using CoreGraphics;
using System.IO;
using System.Text;

namespace JudoDotNetXamariniOSSDK
{
	public partial class CreditCardView : UIViewController
	{
		private UIView _activeview;
		private bool _moveViewUp;
		private readonly IPaymentService _paymentService;
		private bool _keyboardVisible;

		private List<CardCell> CellsToShow { get; set; }

		CardEntryCell detailCell;

		ReassuringTextCell reassuringCell{ get; set; }

		MaestroCell maestroCell { get; set; }

		AVSCell avsCell{ get; set; }

		public SuccessCallback successCallback { private get; set; }

		public FailureCallback failureCallback { private get; set; }

		private string ReceiptID;

		public PaymentViewModel cardPayment { get; set; }

		public CreditCardView (IPaymentService paymentService) : base ("CreditCardView", null)
		{
			_paymentService = paymentService;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.View.BackgroundColor = UIColor.FromRGB (245f, 245f, 245f);
		}

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.View.Superview.RepositionFormSheetForiPad ();
			}



		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			SetUpTableView ();

			this.View.BackgroundColor = UIColor.FromRGB (245f, 245f, 245f);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad) {
				NSNotificationCenter defaultCenter = NSNotificationCenter.DefaultCenter;
				defaultCenter.AddObserver (UIKeyboard.WillHideNotification, OnKeyboardNotification);
				defaultCenter.AddObserver (UIKeyboard.WillShowNotification, OnKeyboardNotification);
				defaultCenter.AddObserver (UIKeyboard.DidShowNotification, KeyBoardUpNotification);
			}

			UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer ();

			tapRecognizer.AddTarget (() => { 
				if (_keyboardVisible) {
					DismissKeyboardAction ();
				}
			});

			tapRecognizer.NumberOfTapsRequired = 1;
			tapRecognizer.NumberOfTouchesRequired = 1;

			EncapsulatingView.AddGestureRecognizer (tapRecognizer);

			SubmitButton.SetTitleColor (UIColor.Black, UIControlState.Application);

			SubmitButton.TouchUpInside += (sender, ev) => {
				MakePayment ();
			};

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				FormClose.TouchUpInside += (sender, ev) => {
					this.DismissViewController (true, null);
				};
			}
			SubmitButton.Disable();
			detailCell.ccTextOutlet.BecomeFirstResponder ();
		}

		private void OnKeyboardNotification (NSNotification notification)
		{
			_keyboardVisible = notification.Name == UIKeyboard.WillShowNotification;

			if (!_keyboardVisible) {
				if (_moveViewUp) {
					ScrollTheView (false);
				}
			}
		}

		private void KeyBoardUpNotification (NSNotification notification)
		{

			CGRect r = UIKeyboard.BoundsFromNotification (notification);

			if (avsCell.PostcodeTextFieldOutlet.IsFirstResponder)
				_activeview = avsCell.PostcodeTextFieldOutlet;
			if (_activeview != null && !detailCell.HasFocus ()) {
				_moveViewUp = true;
				ScrollTheView (_moveViewUp);

			}
		}


		private void ScrollTheView (bool move)
		{
			if (move) {
				TableView.SetContentOffset (new CoreGraphics.CGPoint (0, 100f), true);
				TableView.ScrollEnabled = false;
			} else {
				TableView.SetContentOffset (new CoreGraphics.CGPoint (0, 0), true);

			}

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

		}

		void DismissKeyboardAction ()
		{
			detailCell.DismissKeyboardAction ();
			avsCell.DismissKeyboardAction ();
			maestroCell.DismissKeyboardAction ();
		}

		private void UpdateUI ()
		{
			bool enable = false;
			enable = detailCell.EntryComplete ();

			List<CardCell> cellsToRemove = new List<CardCell> ();
			List<CardCell> insertedCells = new List<CardCell> ();
			List<CardCell> cellsBeforeUpdate = CellsToShow.ToList ();


			if (enable) {
				bool ccIsFirstResponder = detailCell.ccTextOutlet.IsFirstResponder;
				int row = CellsToShow.IndexOf (detailCell) + 1;

				if (JudoSDKManager.AVSEnabled) {
					if (!CellsToShow.Contains (avsCell)) {
						TableView.BeginUpdates ();
						//int row = CellsToShow.IndexOf (reassuringCell);
						CellsToShow.Insert (row, avsCell);
						row++;// icrementing the row incase an avs cell is also needed;
						insertedCells.Add (avsCell);
						avsCell.PostcodeTextFieldOutlet.BecomeFirstResponder ();
						ccIsFirstResponder = false;

	
						TableView.InsertRows (new NSIndexPath[]{ NSIndexPath.FromRowSection (CellsToShow.IndexOf (avsCell), 0) }, UITableViewRowAnimation.Fade);
						TableView.EndUpdates ();
					}

				}
					if (detailCell.Type == CardType.MAESTRO && JudoSDKManager.MaestroAccepted) {
					if (!CellsToShow.Contains (maestroCell)) {
						TableView.BeginUpdates ();
						CellsToShow.Insert (row, maestroCell);

						insertedCells.Add (maestroCell);
						maestroCell.StartDateTextFieldOutlet.BecomeFirstResponder ();
						ccIsFirstResponder = false;
						TableView.InsertRows (new NSIndexPath[]{ NSIndexPath.FromRowSection (CellsToShow.IndexOf (maestroCell), 0) }, UITableViewRowAnimation.Fade);
						TableView.EndUpdates ();
					}

					if (maestroCell.IssueNumberTextFieldOutlet.Text.Length == 0 && maestroCell.StartDateTextFieldOutlet.Text.Length != 5) {
						enable = false;
					}
						
				}
					
				if (ccIsFirstResponder) {
					DismissKeyboardAction ();

					ccIsFirstResponder = false;
				}
			} else {
				TableView.BeginUpdates ();
				if (JudoSDKManager.MaestroAccepted) {
					if (CellsToShow.Contains (maestroCell)) {
						cellsToRemove.Add (maestroCell);
					}
				}

				if (JudoSDKManager.AVSEnabled) {
					if (CellsToShow.Contains (avsCell)) {
						cellsToRemove.Add (avsCell);
					}
				}
				List<NSIndexPath> indexPathsToRemove = new List<NSIndexPath> ();

				foreach (CardCell cell in cellsToRemove) {
					indexPathsToRemove.Add (NSIndexPath.FromRowSection (cellsBeforeUpdate.IndexOf (cell), 0));
				}

				TableView.DeleteRows (indexPathsToRemove.ToArray (), UITableViewRowAnimation.Fade);

				foreach (CardCell cell in cellsToRemove) {
					CellsToShow.Remove (cell);
				}

				TableView.EndUpdates ();
			}
				
			SubmitButton.Enabled = enable;
			SubmitButton.Alpha = (enable == true ? 1f : 0.25f);

		}

		void SetUpTableView ()
		{
			detailCell = new CardEntryCell (new IntPtr ());
			reassuringCell = new ReassuringTextCell (new IntPtr ());
			avsCell = new AVSCell (new IntPtr ());
			maestroCell = new MaestroCell (new IntPtr ());


			detailCell = (CardEntryCell)detailCell.Create ();
			reassuringCell = (ReassuringTextCell)reassuringCell.Create ();
			avsCell = (AVSCell)avsCell.Create ();
			maestroCell = (MaestroCell)maestroCell.Create ();


			detailCell.UpdateUI = () => {
				UpdateUI ();
			};

			avsCell.UpdateUI = () => {
				UpdateUI ();
			};

			maestroCell.UpdateUI = () => {
				UpdateUI ();
			};

			CellsToShow = new List<CardCell> (){ detailCell, reassuringCell };



			CardCellSource tableSource = new CardCellSource (CellsToShow);
			TableView.Source = tableSource;
			TableView.SeparatorColor = UIColor.Clear;

			SecureWebView.ShouldStartLoad = (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType) => {
				if(request.Url.ToString().Equals("judo1234567890://threedsecurecallback") && ReceiptID !=null)
				{
				//	var uri = new Uri(request.Body.ToString());
					Dictionary<string,string> queryStringDictionary = new Dictionary<string,string>();

					var TrackTraceDataArray = request.Body.ToString().Split (new char[] { '&' });

					foreach (string keyValuePair in TrackTraceDataArray)
					{
						var pairComponents = keyValuePair.Split (new char[] { '=' });
						string key =pairComponents.First();//  [[pairComponents firstObject] stringByRemovingPercentEncoding];
						string value =pairComponents.Last();
						//NSString value = [[pairComponents lastObject] stringByRemovingPercentEncoding];
						queryStringDictionary.Add(key,value);
					}

				
					//var dictionary =  NSJsonSerialization.Deserialize (request.Body,0,out error);//[NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
					//var PaRes =NSUrlProtocol.GetProperty("PaRes",request);
					//var md = request.Body[@"MD"];
					_paymentService.CompleteDSecure (ReceiptID,queryStringDictionary["PaRes"],queryStringDictionary["MD"]).ContinueWith (reponse => {
						var result = reponse.Result;
						//var threedDSecureReceipt = result.Response as PaymentRequiresThreeDSecureModel;
					});
				}

				return true;
			};
		}	
	

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
				this.View.Hidden=true;
			}
		}



		private void MakePayment ()
		{
			try {
				JudoSDKManager.ShowLoading (this.View);
             
				cardPayment.Card = GatherCardDetails ();

				SubmitButton.Disable();



				_paymentService.MakePayment (cardPayment).ContinueWith (reponse => 
				{
					var result = reponse.Result;
					if(JudoSDKManager.ThreeDSecureEnabled && result.Response.GetType() == typeof(PaymentRequiresThreeDSecureModel))
					{
						var threedDSecureReceipt = result.Response as PaymentRequiresThreeDSecureModel;
						ReceiptID=	threedDSecureReceipt.ReceiptId;
						NSCharacterSet allowedCharecterSet =NSCharacterSet.FromString( @":/=,!$&'()*+;[]@#?").InvertedSet;
						NSString paReq = new NSString(threedDSecureReceipt.PaReq);
						var encodedPaReq = paReq.CreateStringByAddingPercentEncoding(allowedCharecterSet);

						NSString termUrl = new NSString("judo1234567890://threedsecurecallback"); //"judo1234567890://threedsecurecallback".stringByAddingPercentEncodingWithAllowedCharacters(allowedCharacterSet)
						var encodedTermUrl = termUrl.CreateStringByAddingPercentEncoding(allowedCharecterSet);


						NSUrl url = new NSUrl (threedDSecureReceipt.AcsUrl);

						NSMutableUrlRequest req = new NSMutableUrlRequest (url);

						//var postData = String.Format("PaReq={0}&TermUrl={1}&MD={2}", encodedPaReq,encodedTermUrl,threedDSecureReceipt.Md).
					
						NSString postString = new NSString("MD="+threedDSecureReceipt.Md+"&PaReq="+encodedPaReq+"&TermUrl="+encodedTermUrl+"");
						NSData postData = postString.Encode(NSStringEncoding.UTF8);
						//NSString postString = (NSString)postObj;


						req.HttpMethod = "POST";
						req.Body = postData;
						req["Content-Length"] = req.Body.Length.ToString();
						//req["Content-Type"] = "application/x-www-form-urlencoded charset=utf-8";

						try
						{
							DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
								SecureWebView.LoadRequest(req);

								JudoSDKManager.HideLoading ();
								SecureWebView.Hidden =false;
							});
						}
						catch(Exception e)
						{
						}


			

					}
					else
					{
					if (result != null && !result.HasError && result.Response.Result != "Declined") {
						var paymentreceipt = result.Response as PaymentReceiptModel;

						if (paymentreceipt != null) {
							// call success callback
							if (successCallback != null)

								successCallback (paymentreceipt);
						} else {
							throw new Exception ("JudoXamarinSDK: unable to find the receipt in response.");
						}

					} else {
						// Failure callback
						if (failureCallback != null) {
							var judoError = new JudoError { ApiError = result != null ? result.Error : null };
							var paymentreceipt = result != null ? result.Response as PaymentReceiptModel : null;

							if (paymentreceipt != null) {
								// send receipt even we got card declined

								failureCallback (judoError, paymentreceipt);
							} else {

								failureCallback (judoError);
							}
						}
					}

					JudoSDKManager.HideLoading ();
				}
				});

			} catch (Exception ex) {
				JudoSDKManager.HideLoading ();
				// Failure callback
				if (failureCallback != null) {
					var judoError = new JudoError { Exception = ex };
					failureCallback (judoError);
				}
			}
		

		}

		void CleanOutCardDetails ()
		{
			detailCell.CleanUp ();

			if (JudoSDKManager.MaestroAccepted) {
				maestroCell.CleanUp ();

			}	
			if (JudoSDKManager.AVSEnabled) {
				avsCell.CleanUp ();
			}
		}

		CardViewModel GatherCardDetails ()
		{
			CardViewModel cardViewModel = new CardViewModel ();
			detailCell.GatherCardDetails (cardViewModel);


			if (JudoSDKManager.AVSEnabled) {
				avsCell.GatherCardDetails (cardViewModel);

			}

			if (detailCell.Type == CardType.MAESTRO) {
				maestroCell.GatherCardDetails (cardViewModel);

			}

			return cardViewModel;
		}
			

	}


}

