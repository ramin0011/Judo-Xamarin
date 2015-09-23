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

		}
		private static readonly Encoding encoding = Encoding.UTF8;
		private static byte[] GetMultipartFormData(Dictionary<string, string> postParameters, string boundary)
		{
			Stream formDataStream = new System.IO.MemoryStream();
			bool needsCLRF = false;

			foreach (var param in postParameters)
			{
				// Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
				// Skip it on the first parameter, add it to subsequent parameters.
				if (needsCLRF)
					formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

				needsCLRF = true;

//				if (param.Value is FileParameter)
//				{
//					FileParameter fileToUpload = (FileParameter)param.Value;
//
//					// Add just the first part of this param, since we will write the file data directly to the Stream
//					string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
//						boundary,
//						param.Key,
//						fileToUpload.FileName ?? param.Key,
//						fileToUpload.ContentType ?? "application/octet-stream");
//
//					formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
//
//					// Write the file data directly to the Stream, rather than serializing it to a string.
//					formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
//				}
//				else
//				{
					string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
						boundary,
						param.Key,
						param.Value);
					formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
//				}

				string footer = "\r\n--" + boundary + "--\r\n";
				formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

				// Dump the Stream into a byte[]

			}
			formDataStream.Position = 0;
			byte[] formData = new byte[formDataStream.Length];
			formDataStream.Read(formData, 0, formData.Length);
			formDataStream.Close();

			return formData;
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



				_paymentService.MakePayment (cardPayment).ContinueWith (reponse => {
					var result = reponse.Result;
					if(JudoSDKManager.ThreeDSecureEnabled && result.Response.GetType() == typeof(PaymentRequiresThreeDSecureModel))
					{
						var threedDSecureReceipt = result.Response as PaymentRequiresThreeDSecureModel;

						var req = new NSMutableUrlRequest (new NSUrl (threedDSecureReceipt.AcsUrl));
						req.HttpMethod = "POST";



						NSDictionary bodyParams = new NSDictionary();

						Dictionary<string,string> dict = new Dictionary<string,string>();

						dict.Add("PaReq", threedDSecureReceipt.PaReq);
						dict.Add("Md", threedDSecureReceipt.Md);
						dict.Add("TermUrl", @"http://www.davidbowieisverydisappointedinyou.com");
						NSData httpBody =  //[self createBodyWithBoundary:boundary parameters:params paths:@[path] fieldName:fieldName];

						req.Body = NSData.FromArray(GetMultipartFormData(dict,"&"));
						//req.Body = NSData.FromString(String.Format("PaReq={0}&Md={1}&TermUrl={2}", threedDSecureReceipt.PaReq,threedDSecureReceipt.Md,"http://www.davidbowieisverydisappointedinyou.com"));
						req["Content-Length"] = req.Body.Length.ToString();

						req["Content-Type"] = "multipart/form-data";
						req["boundary"] = "&";
						try
						{
							DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
								SecureWebView.LoadRequest(req);
								JudoSDKManager.HideLoading ();
								SecureWebView.Hidden =false;
							});

						}

//						NSUrl url = new NSUrl (threedDSecureReceipt.AcsUrl);
//
//						NSMutableUrlRequest req = new NSMutableUrlRequest (url);
//
//						NSObject postObj = FromObject (String.Format("PaReq={0}&TermUrl={1}&MD={2}", threedDSecureReceipt.PaReq,"http://www.davidbowieisverydisappointedinyou.com",threedDSecureReceipt.Md));
//
//						NSString postString = (NSString)postObj;
//
//						req.HttpMethod = "POST";
//						req.
//
//						NSData postData = postString.Encode(NSStringEncoding.UTF8);
//
//						req.Body = postData;
//						try
//						{
//							DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
//								SecureWebView.LoadRequest(req);
//								JudoSDKManager.HideLoading ();
//								SecureWebView.Hidden =false;
//							});
//						}


						catch (Exception ex) {

						}
			

					}
					else
					{
					}
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

