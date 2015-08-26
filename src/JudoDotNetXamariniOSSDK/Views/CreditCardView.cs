/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *
 * This file is part of CreditCard -- an iOS project that provides a smooth and elegant 
 * means to enter or edit credit cards. It was inspired by  a similar form created from 
 * scratch by Square (https://squareup.com/). To see this form in action visit:
 * 
 *   http://functionsource.com/post/beautiful-forms)
 *
 * Copyright 2012 Lot18 Holdings, Inc. All Rights Reserved.
 *
 *
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 *
 *    1. Redistributions of source code must retain the above copyright notice, this list of
 *       conditions and the following disclaimer.
 *
 *    2. Redistributions in binary form must reproduce the above copyright notice, this list
 *       of conditions and the following disclaimer in the documentation and/or other materials
 *       provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY Lot18 Holdings ''AS IS'' AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL David Hoerl OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreAnimation;
using System.Text;
using CoreFoundation;
using System.Linq;
using System.Collections.Generic;
using JudoPayDotNet.Models;


namespace JudoDotNetXamariniOSSDK
{
	public partial class CreditCardView : UIViewController, IUIScrollViewDelegate
	{
		public CreditCardControllerType CreditCardControllerType { get; set; }

		public Card judoCard { get; set; }

		public UIActionSheet countrySheet;

		CreditCard cardHelper = new CreditCard ();
		bool KeyboardVisible = false;
		BillingCountryOptions selectedCountry = BillingCountryOptions.BillingCountryOptionUK;

		private List<UITableViewCell> CellsToShow { get; set; }


		UIImageView ccBackImage;
		UIImageView ccImage;

		int currentYear;

		CreditCardType type;
		int numberLength;
		string creditCardNum;
		int year;
		string ccv;

		int cardMonth;
		bool completelyDone;

		string formattedText;
		bool flashForError = false;
		bool updateText = false;
		bool scrollForward = false;
		bool deleting = false;
		bool ret = false;
		bool hasFullNumber = false;
		bool deletedSpace = false;

		IPaymentService _paymentService;

		public CreditCardView (IPaymentService paymentService) : base ("CreditCardView", null)
		{
			_paymentService = paymentService;
		}
			
		float widthToLastGroup {
			get { 
				int oldOffset = placeView.ShowTextOffset;
				int offsetToLastGroup = cardHelper.LengthOfFormattedStringTilLastGroupForType (type);
				placeView.SetShowTextOffSet (offsetToLastGroup);
				float width = placeView.WidthToOffset ();
				placeView.SetShowTextOffSet (oldOffset);
				return width;
			}
		}
			
		void FlashMessage (string message)
		{
			PaymentErrorLabel.Text = message;
			PaymentErrorLabel.Hidden = false;

			DispatchQueue.MainQueue.DispatchAfter (new DispatchTime (DispatchTime.Now, 1 * 1000000000), () => {
				PaymentErrorLabel.Hidden = true;

			});
		}

		void FlashCheckDateLabel ()
		{
			StartDateWarningLabel.Hidden = false;
			DispatchQueue.MainQueue.DispatchAfter (new DispatchTime (DispatchTime.Now, 1 * 1000000000), () => {
				StartDateWarningLabel.Hidden = true;
			});
		}
			
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.View.BackgroundColor = UIColor.FromRGB (245f, 245f, 245f);
		}

		void SetAVSComponents ()
		{
			countrySheet = new UIActionSheet ("Select Country");
			countrySheet.TintColor = UIColor.Black;
			selectedCountry = BillingCountryOptions.BillingCountryOptionUK;
			countrySheet.Clicked += delegate(object sender, UIButtonEventArgs button) {
				switch (button.ButtonIndex) {
				case (int) BillingCountryOptions.BillingCountryOptionUK:
					selectedCountry = BillingCountryOptions.BillingCountryOptionUK;

					break;
				case (int)BillingCountryOptions.BillingCountryOptionUSA:
					selectedCountry = BillingCountryOptions.BillingCountryOptionUSA;
				
					break;
				case (int)BillingCountryOptions.BillingCountryOptionCanada:
					selectedCountry = BillingCountryOptions.BillingCountryOptionCanada;

					break;
				case (int)BillingCountryOptions.BillingCountryOptionOther:
					selectedCountry = BillingCountryOptions.BillingCountryOptionOther;

					break;
				default:
					selectedCountry = BillingCountryOptions.BillingCountryOptionUK;
					break;

				}
				CountryLabel.Text = selectedCountry.ToDescriptionString ();
			};

			foreach (BillingCountryOptions option in Enum.GetValues(typeof(BillingCountryOptions))) {
				countrySheet.AddButton (option.ToDescriptionString ());
			}
			CountryButton.TouchUpInside += (sender, ev) => {
				countrySheet.ShowInView (this.View);
			};
			PostcodeTextField.Text = "";	
			PostcodeTextField.Font = ccText.Font;
			PostcodeTextField.TextColor = ccText.TextColor;

			PostcodeTextField.ShouldChangeCharacters = (UITextField textField, NSRange nsRange, string replacementString) => {
				CSRange range = new CSRange ((int)nsRange.Location, (int)nsRange.Length);
				DispatchQueue.MainQueue.DispatchAsync (() => {
					UpdateUI ();
				});
				int textLengthAfter = textField.Text.Length + replacementString.Length - range.Length;
				if (textLengthAfter > 10) {
					return false;
				}
				return true;
			};

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			SetUpTableView ();

			if (JudoSDKManager.AVSEnabled) {
				SetAVSComponents ();
			}



			this.View.BackgroundColor = UIColor.FromRGB (245f, 245f, 245f);

			switch (CreditCardControllerType) {
			case CreditCardControllerType.CreditCardControllerTypeAddCard:
			case CreditCardControllerType.CreditCardControllerRegisterCard:
				{
					Title = ThemeBundleReplacement.BundledOrReplacementString ("registerCardType", BundledOrReplacementOptions.BundledOrReplacement);
					break;
				}
			default:
				break;
			}

			NSNotificationCenter defaultCenter = NSNotificationCenter.DefaultCenter;
			defaultCenter.AddObserver (UIKeyboard.WillHideNotification, OnKeyboardNotification);
			defaultCenter.AddObserver (UIKeyboard.WillShowNotification, OnKeyboardNotification);

			SubmitButton.SetTitleColor (UIColor.Black, UIControlState.Application);

			SubmitButton.TouchUpInside += (sender, ev) => {
				MakePayment ();
			};
			ExpiryInfoButton.TouchUpInside += (sender, ev) => {
				PushExpiryInfoView ();
			};

			UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer ();

			tapRecognizer.AddTarget (() => { 
				if (KeyboardVisible) {
					DismissKeyboardAction ();
				}
			});

			tapRecognizer.NumberOfTapsRequired = 1;
			tapRecognizer.NumberOfTouchesRequired = 1;

			EncapsulatingView.AddGestureRecognizer (tapRecognizer);
		}

		private void OnKeyboardNotification (NSNotification notification)
		{
			KeyboardVisible = notification.Name == UIKeyboard.WillShowNotification;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);


			float width = widthToLastGroup;

			CGRect frame = ccText.Frame;
			frame.Size = new CGSize (width + textScroller.Frame.Size.Width, frame.Size.Height);

			ccText.Frame = frame;
			placeView.Frame = frame;
			textScroller.ContentSize = new CGSize (frame.Size.Width, textScroller.ContentSize.Height);

			textScroller.ScrollEnabled = true;
			textScroller.SetContentOffset (new CGPoint (0f, 0f), animated);

			if (float.Parse (UIDevice.CurrentDevice.SystemVersion.Replace (".", "")) >= 800f) {
				if (this.View.Bounds.Size.Width > 320f) {
					float margin = 13f;
					if (this.View.Bounds.Size.Width > 375) {
						margin = 18f;
					}
					UIEdgeInsets insets = CardDetailCell.ContentView.LayoutMargins;
					insets.Left = margin;
					insets.Left = margin;
					CardDetailCell.ContentView.LayoutMargins = insets;
					ReassuringTextCell.ContentView.LayoutMargins = insets;
					AVSCell.ContentView.LayoutMargins = insets;
					MaestroCell.ContentView.LayoutMargins = insets;
					PayCell.ContentView.LayoutMargins = insets;
				}
			}
		}

		void PushExpiryInfoView ()
		{
			var alertText = "";
			if (creditCardImage != ccBackImage) {
				alertText = string.Format (@"MM/YY: {0}\n\n CV2: {1}", "The month and year your card expires", "The security code printed on the signature strip on the back of your card");
			} else {
				alertText = string.Format (@"CV2: {0}", "The security code printed on the signature strip on the back of your card");
			}
			UIAlertView alert = new UIAlertView ("Expiry Info", alertText, null, "OK", null);
			alert.Show ();
		}


		void SetUpTableView ()
		{
			CellsToShow = new List<UITableViewCell> (){ CardDetailCell, ReassuringTextCell };

			CGRect rectangle = ccText.Frame;
			ccText.Frame = rectangle;

			creditCardImage.Tag = (int)CreditCardType.InvalidCard;

			creditCardImage.Layer.CornerRadius = 4.0f;
			creditCardImage.Layer.MasksToBounds = true;

			UIImage image = ThemeBundleReplacement.BundledOrReplacementImage ("ic_card_large_unknown", BundledOrReplacementOptions.BundledOrReplacement);

			creditCardImage.Image = image;

			currentYear = DateTime.Now.Year - 2000;

			CALayer layer = containerView.Layer;
			layer.CornerRadius = 4.0f;
			layer.MasksToBounds = true;
			layer.BorderColor = ColourHelper.GetColour ("0xC3C3C3FF").CGColor; 
			layer.BorderWidth = 1;
			layer = textScroller.Layer;
			layer.CornerRadius = 4.0f;
			layer.MasksToBounds = true;
			layer.BorderWidth = 0;

			textScroller.ScrollEnabled = false;
	
			ccText.Text = "000011112222333344445555";

			UITextPosition start = ccText.BeginningOfDocument;
			UITextPosition end = ccText.GetPosition (start, 24);
			UITextRange range = ccText.GetTextRange (start, end);
			CGRect r = ccText.GetFirstRectForRange (range);
			CGSize frameRect = r.Size;
			frameRect.Width = (r.Size.Width / 24.0f);
			ccText.Font = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;
			r.Size = frameRect;
			ccText.Text = "";
			
			CGRect frame = placeView.Frame;
			placeView.Font = ccText.Font;
			placeView.Text = "0000 0000 0000 0000";

			placeView.SetShowTextOffSet (0);
			placeView.Offset = r;
	
			placeView.BackgroundColor = UIColor.Clear;
			textScroller.InsertSubview (placeView, 0);

			type = CreditCardType.InvalidCard;

			AddPaymentTableSource tableSource = new AddPaymentTableSource (CellsToShow);
			TableView.Source = tableSource;
			TableView.SeparatorColor = UIColor.Clear;
			SetUpMaskedInput ();

		}

		void SetUpMaskedInput ()
		{
			ccText.ShouldChangeText = (UITextView textView, NSRange NSRange, string replace) => {
				CSRange range = new CSRange ((int)NSRange.Location, (int)NSRange.Length);
				formattedText = "";
				flashForError = false;
				updateText = false;
				scrollForward = false;
				deleting = false;
				ret = false;
				deletedSpace = false;
				cardMonth = 0;

				completelyDone = false;
				if (replace.Length == 0) {
					updateText = true;
					deleting = true;
					if (textView.Text.Length != 0) {	// handle case of delete when there are no characters left to delete
						
						char c = textView.Text.Substring (range.Location, 1).ToCharArray () [0];
						if (range.Location != 0 && range.Length == 1 && (c == ' ' || c == '/')) {
							range.Location--;
							range.Length++;
							deletedSpace = true;
						}
					} else {
						return false;
					}
				}
				var aStringBuilder = new StringBuilder (textView.Text);
				aStringBuilder.Remove (range.Location, range.Length);
				aStringBuilder.Insert (range.Location, replace);
				string newTextOrig = aStringBuilder.ToString ();

				int newTextLen = newTextOrig.Length;

				// causes the cc entry field to scroll back if the user deletes back beyond the end of the cc number
				if (range.Location <= numberLength) {
					hasFullNumber = false;
				}

				if (hasFullNumber) {

					// Test for delete of a space or /
					if (deleting) {
						formattedText = newTextOrig.Substring (0, range.Location);
						updateText = true;
						return EndDelegate ();
					}

					if (newTextLen > placeView.Text.Length) {
						flashForError = true;
						return EndDelegate ();
					}

					formattedText = newTextOrig;

					CSRange monthRange = new CSRange (placeView.Text.IndexOf ("MM"), 2);
					if (newTextLen > monthRange.Location) {
						if (newTextOrig.Substring (monthRange.Location, 1).ToCharArray () [0] > '1') {
							// support short cut - we prepend a '0' for them

							var aStringBuilder2 = new StringBuilder (textView.Text);
							aStringBuilder2.Remove (range.Location, range.Length);
							aStringBuilder2.Insert (range.Location, "0" + replace);
							formattedText = aStringBuilder2.ToString ();
							newTextLen = newTextOrig.Length;
						}
						if (newTextLen >= (monthRange.Location + monthRange.Length)) {
							cardMonth = Int32.Parse (newTextOrig.Substring (monthRange.Location, monthRange.Length));
							if (cardMonth < 1 || cardMonth > 12) {
								flashRecheckExpiryDateMessage ();
								return EndDelegate ();
							}
						}
					}

					CSRange yearRange = new CSRange (placeView.Text.IndexOf ("YY"), 2);
					if (newTextLen > yearRange.Location) {
						int proposedDecade = (newTextOrig.Substring (yearRange.Location, 1).ToCharArray () [0] - '0') * 10;
						int yearDecade = currentYear - (currentYear % 10);
						if (proposedDecade < yearDecade) {
							flashRecheckExpiryDateMessage ();
							return EndDelegate ();
						}
						if (newTextLen >= (yearRange.Location + yearRange.Length)) {
							year = Int32.Parse (newTextOrig.Substring (yearRange.Location, yearRange.Length)); 
							int diff = year - currentYear;
							if (diff < 0 || diff > 10) {	
								flashRecheckExpiryDateMessage ();
								return EndDelegate ();
							}
							if (diff == 0) { 
								
								var todaysDate = DateTime.Today;
								int currentMonth = todaysDate.Month;

								if (cardMonth < currentMonth) {
									flashRecheckExpiryDateMessage ();
									return EndDelegate ();
								}
							}
							if (creditCardImage != ccBackImage) {
								UIViewAnimationOptions transType = (type == CreditCardType.AMEX) ? UIViewAnimationOptions.TransitionCrossDissolve : UIViewAnimationOptions.TransitionFlipFromBottom;
							
								UIImageView.Animate (
									duration: 0.25f, 
									delay: 0,
									options: transType,
									animation: () => {
										creditCardImage = ccBackImage;
									},
									completion: () => {
										StatusHelpLabel.Text = "Please enter CV2";
									});
							}
						}
					}

					if (newTextLen == placeView.Text.Length) {
						completelyDone = true;
						var cIndex = placeView.Text.IndexOf ("C");
						CSRange ccvRange = new CSRange (cIndex, placeView.Text.Substring (cIndex).Length);
						ccvRange.Length = type == CreditCardType.AMEX ? 4 : 3;
						ccv = newTextOrig.Substring (ccvRange.Location, ccvRange.Length);
					}

					updateText = true;
				} else {
					// scrolls backward
					int textViewLen = ccText.Text.Length; 
					int formattedLen = placeView.Text.Length;
					placeView.SetShowTextOffSet (Math.Min (textViewLen, formattedLen));
					textScroller.ScrollEnabled = false;

					textScroller.SetContentOffset (new CGPoint (0, 0), true);

					StatusHelpLabel.Text = "Enter Card Details";


					string newText = newTextOrig.Replace (" ", String.Empty);
					int len = newText.Length;
					if (len < Card.CC_LEN_FOR_TYPE) {
						updateText = true;
						formattedText = newTextOrig;

						type = CreditCardType.InvalidCard;
					} else {
						type = cardHelper.GetCCType (newText);
						switch (type) {

						case CreditCardType.InvalidCard:
							flashForError = true;
							break;
						case CreditCardType.Maestro:
							if (!JudoSDKManager.MaestroAccepted) {

								flashForError = true;
								return EndDelegate ();
							}
							break;
						case CreditCardType.AMEX:
							if (!JudoSDKManager.AmExAccepted) {
								
								flashForError = true; 
								return EndDelegate ();
							}
							break;
						}
							
						if (len == Card.CC_LEN_FOR_TYPE) {
							placeView.Text = cardHelper.promptStringForType (type, true);
						}

						formattedText = cardHelper.FormatForViewing (newText); 
						int lenForCard = cardHelper.LengthOfStringForType (type); 

						if (len < lenForCard) {
							updateText = true;
						} else if (len == lenForCard) {
							if (cardHelper.isValidNumber (newText)) {
								if (cardHelper.IsLuhnValid (newText)) {
									numberLength = cardHelper.LengthOfFormattedStringForType (type);
									creditCardNum = newText;

									updateText = true;
									scrollForward = true;
									hasFullNumber = true;
								} else {
									FlashRecheckNumberMessage ();
								}
							} else {
								FlashRecheckNumberMessage ();
							}	
						}
					}
					UpdateCCimageWithTransitionTime (0.25f);
				}
				return EndDelegate ();
			};
			if (JudoSDKManager.MaestroAccepted) {
				SetUpStartDateMask ();
			}	
		}
			
		void UpdateCCimageWithTransitionTime (float transittionTime, bool isBack = false)
		{
			if (creditCardImage.Tag != (int)type) {

				UIImage frontImage = cardHelper.CreditCardImage (type);
				ccImage = new UIImageView (frontImage);
				ccImage.Frame = creditCardImage.Frame;
				ccImage.Tag = (int)type;

				ccBackImage = new UIImageView (cardHelper.CreditCardBackImage (type));
				ccBackImage.Frame = creditCardImage.Frame;
				ccBackImage.Tag = (int)type;

				var finalImage = new UIImageView ();
				if (isBack) {
					finalImage = ccBackImage;
				} else {
					finalImage = ccImage;
				}
				UIView.Transition (creditCardImage, finalImage, transittionTime, UIViewAnimationOptions.TransitionFlipFromLeft, null);
				creditCardImage = ccImage;
			}
		}
			
		public void MakePayment ()
		{
			CardViewModel cardViewModel = GatherCardDetails ();
			PaymentViewModel payment = new PaymentViewModel () {
				Amount = "4.99",
				Card = cardViewModel
			};
			SubmitButton.Hidden = true;

			_paymentService.MakePayment (payment).ContinueWith (reponse => {
				var result = reponse.Result;
				if (result!=null&&!result.HasError&&result.Response.Result!="Declined") {
					PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;
					PaymentReceiptViewModel receipt = new PaymentReceiptViewModel ()
					{
						CreatedAt = paymentreceipt.CreatedAt.DateTime,
						Currency = paymentreceipt.Currency,
						OriginalAmount = paymentreceipt.Amount,
						ReceiptId = paymentreceipt.ReceiptId,
						Message="Payment Receipt"
					};
					JudoConfiguration.Instance.CardToken = paymentreceipt.CardDetails.CardToken;
					JudoConfiguration.Instance.TokenCardType = payment.Card.CardType;
					JudoConfiguration.Instance.ConsumerToken= paymentreceipt.Consumer.ConsumerToken;
					JudoConfiguration.Instance.LastFour = payment.Card.CardNumber.Substring(payment.Card.CardNumber.Length - Math.Min(4, payment.Card.CardNumber.Length));	
					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
						SubmitButton.Hidden = false;
						CleanOutCardDetails();
						var view = JudoSDKManager.GetReceiptView (receipt);
						this.NavigationController.PushViewController (view, true);	
					});
				} else {
					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {	
						var errorText = "No Response from Server";
						if(result!=null)
						{
							 errorText = result.Response.Message;
						}

						UIAlertView _error = new UIAlertView ("Payment failed", errorText, null, "ok", null);
						_error.Show ();
						SubmitButton.Hidden = false;
					});
				}
			});
		}

		void CleanOutCardDetails ()
		{
			SetUpTableView ();

			if (JudoSDKManager.MaestroAccepted) {
				SetUpStartDateMask ();

			}	
			if (JudoSDKManager.AVSEnabled) {
				SetAVSComponents ();
			}

			DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
				UIImage defaultImage = cardHelper.CreditCardImage (CreditCardType.InvalidCard);
				creditCardImage.Image = defaultImage;

			});
		}

		CardViewModel GatherCardDetails ()
		{
			string _cardMonth = cardMonth.ToString ();
			if (cardMonth < 10) {
				_cardMonth = "0" + _cardMonth;
			}
			CardViewModel cardViewModel = new CardViewModel () {
				CardName = "Ed Xample",
				CardNumber = creditCardNum,
				CV2 = ccv,
				ExpireDate = _cardMonth + "/" + year,
				CardType = type					
			};

			if (JudoSDKManager.AVSEnabled) {
				cardViewModel.PostCode = PostcodeTextField.Text;

				switch (selectedCountry) {
				case BillingCountryOptions.BillingCountryOptionUK:
					cardViewModel.CountryCode = @"826";
					break;
				case BillingCountryOptions.BillingCountryOptionUSA:
					cardViewModel.CountryCode = @"840";
					break;
				case BillingCountryOptions.BillingCountryOptionCanada:
					cardViewModel.CountryCode = @"124";
					break;
				default:					
					break;
				}

			}

			if (type == CreditCardType.Maestro) {
				cardViewModel.StartDate = StartDateTextField.Text.Replace (@"/", @"");
				cardViewModel.IssueNumber = IssueNumberTextField.Text;
			}

			return cardViewModel;
		}

		public void flashRecheckExpiryDateMessage ()
		{
			FlashMessage ("Invalid Expiry Date");
		}

		void FlashRecheckNumberMessage ()
		{
			FlashMessage ("Invalid Card Number");
		}

		DispatchQueue dispatchGetMainQueue ()
		{
			return  DispatchQueue.MainQueue;  
		}

		public bool EndDelegate ()
		{
			var queue = dispatchGetMainQueue ();
			if (scrollForward) {
				ScrollForward (true);
				queue.DispatchAfter (DispatchTime.Now, () => {
					textScroller.FlashScrollIndicators ();
				});
			}
			if (updateText) {
				int textViewLen = formattedText.Length;
				int formattedLen = placeView.Text.Length;
				placeView.SetShowTextOffSet (Math.Min (textViewLen, formattedLen));

				if ((formattedLen > textViewLen) && !deleting) {
					char c = placeView.Text.Substring (textViewLen, 1).ToCharArray () [0];
					if (c == ' ')
						formattedText = formattedText + " ";
					else if (c == '/')
						formattedText = formattedText + "/"; 
				}
				if (!deleting || hasFullNumber || deletedSpace) {
					ccText.Text = formattedText;
				} else {
					ret = true;
				}

			}
			if (flashForError) {
				FlashMessage ("Please recheck number");
			}
			queue.DispatchAsync (() => {
				UpdateUI ();
			});
			return ret;
		}

		void ScrollForward (bool animated)
		{
			if (creditCardImage != ccBackImage) {
				StatusHelpLabel.Text = "Please enter Expire Date";
			}
			float width = widthToLastGroup;
			CGRect frame = new CGRect (ccText.Frame.Location, new CGSize ((width) + textScroller.Frame.Size.Width, ccText.Frame.Size.Height));
			textScroller.ContentSize = new CGSize (frame.Size.Width, textScroller.ContentSize.Height);
			ccText.Frame = frame;

			placeView.SetText (cardHelper.promptStringForType (type, false));

			textScroller.ScrollEnabled = true;
			if (textScroller.ContentOffset != new CGPoint (width, 0)) {
				
				textScroller.SetContentOffset (new CGPoint (width, 0), animated);
			}
				

			UIView.Transition (creditCardImage, ccBackImage, 0.25f, UIViewAnimationOptions.TransitionFlipFromLeft, null);
		}

		void DismissKeyboardAction ()
		{			
			placeView.ResignFirstResponder ();
			ccText.ResignFirstResponder ();
			PostcodeTextField.ResignFirstResponder ();
			StartDateTextField.ResignFirstResponder ();
			IssueNumberTextField.ResignFirstResponder ();

		}

		private void UpdateUI ()
		{
			bool enable = false;
			enable = completelyDone;

			List<UITableViewCell> cellsToRemove = new List<UITableViewCell> ();
			List<UITableViewCell> insertedCells = new List<UITableViewCell> ();
			List<UITableViewCell> cellsBeforeUpdate = cellsToRemove.ToList ();
			TableView.BeginUpdates ();

			if (enable) {
				bool ccIsFirstResponder = ccText.IsFirstResponder;

				if (type == CreditCardType.Maestro && JudoSDKManager.MaestroAccepted) {
					if (!CellsToShow.Contains (MaestroCell)) {
						int row = CellsToShow.IndexOf (CardDetailCell) + 1;
						CellsToShow.Insert (row, MaestroCell);
						TableView.ReloadData ();
						insertedCells.Add (MaestroCell);
					}

					if (IssueNumberTextField.Text.Length == 0 || !(StartDateTextField.Text.Length == 5)) {
						enable = false;
					}

					if (ccIsFirstResponder) {
						StartDateTextField.BecomeFirstResponder ();
						ccIsFirstResponder = false;
					}
				}

				if (JudoSDKManager.AVSEnabled) {
					if (!CellsToShow.Contains (AVSCell)) {
						int row = CellsToShow.IndexOf (ReassuringTextCell);
						CellsToShow.Insert (row, AVSCell);
						TableView.ReloadData ();
						insertedCells.Add (AVSCell);
					}

					if (ccIsFirstResponder) {
						PostcodeTextField.BecomeFirstResponder ();
						ccIsFirstResponder = false;
					}
				}

				if (ccIsFirstResponder) {
					DismissKeyboardAction ();

					ccIsFirstResponder = false;
					TableView.ReloadData ();
				}
			} else {
				if (JudoSDKManager.MaestroAccepted) {
					if (CellsToShow.Contains (MaestroCell)) {
						cellsToRemove.Add (MaestroCell);
					}
				}

				if (JudoSDKManager.AVSEnabled) {
					if (CellsToShow.Contains (AVSCell)) {
						cellsToRemove.Add (AVSCell);
					}
				}
			}
			List<NSIndexPath> indexPathsToRemove = new List<NSIndexPath> ();

			foreach (UITableViewCell cell in cellsToRemove) {
				indexPathsToRemove.Add (NSIndexPath.FromRowSection (cellsBeforeUpdate.IndexOf (cell), 0));
			}

			TableView.DeleteRows (indexPathsToRemove.ToArray (), UITableViewRowAnimation.Fade);

			foreach (UITableViewCell cell in cellsToRemove) {
				CellsToShow.Remove (cell);
			}


			List<NSIndexPath> indexPathsToAdd = new List<NSIndexPath> ();

			foreach (UITableViewCell cell in insertedCells) {
				indexPathsToAdd.Add (NSIndexPath.FromRowSection (CellsToShow.IndexOf (cell), 0));
			}
				
			TableView.InsertRows (indexPathsToAdd.ToArray (), UITableViewRowAnimation.Fade);
			TableView.EndUpdates ();
			SubmitButton.Enabled = enable;
			SubmitButton.Hidden = !enable;
		}
			
		void SetUpStartDateMask ()
		{
			StartDateTextField.ShouldChangeCharacters = (UITextField textField, NSRange nsRange, string replacementString) => {
				CSRange range = new CSRange ((int)nsRange.Location, (int)nsRange.Length);
				DispatchQueue.MainQueue.DispatchAsync (() => {
					UpdateUI ();
				});
				bool changeText = true;

				if (range.Length > 1) {
					return false;
				}
				if (replacementString.Length > 1) {
					return false;
				}
				if (replacementString.Length == 1 && !char.IsDigit (replacementString.ToCharArray () [0])) {
					return false;
				}
				if (textField.Text.Length + replacementString.Length - range.Length > 5) {
					return false;
				}
			
				int textLengthAfter = (int)(textField.Text.Length + replacementString.Length - range.Length);

				if (replacementString.Length == 0 && range.Location < 2 && textField.Text.Contains ("/")) {
					textField.Text = textField.Text.Replace (@"/", @"");
					textLengthAfter--;
				}
				if (range.Length == 1 && textField.Text.Substring (range.Location, 1) == "/") { 
					textField.Text = textField.Text.Substring (0, 1);
					textLengthAfter = 1;
					changeText = false;
				}

				if (range.Location == 1 && textField.Text.Length == 1) {						

					var aStringBuilder = new StringBuilder (textField.Text);
					aStringBuilder.Remove (range.Location, range.Length);
					aStringBuilder.Insert (range.Location, replacementString);
					string newTextOrig = aStringBuilder.ToString ();

					string text = newTextOrig;
					if (Int32.Parse (text) > 12 || Int32.Parse (text) == 0) {
						FlashCheckDateLabel ();
						return false;
					}

					textField.Text = text;
					textField.Text = textField.Text + @"/";
					textLengthAfter++;
					changeText = false;
				} else if (range.Location == 0 && textField.Text.Length == 0) {
					if (replacementString.Substring (0, 1).ToCharArray () [0] > '1') { 
							

						var formatedString = string.Format (@"0{0}/", replacementString);

						var aStringBuilder = new StringBuilder (textField.Text);
						aStringBuilder.Remove (range.Location, range.Length);
						aStringBuilder.Insert (range.Location, formatedString);

						textField.Text = aStringBuilder.ToString ();
						textLengthAfter += 2;
						changeText = false;
					}
				}

				if (textLengthAfter >= 4) {

					var aStringBuilder = new StringBuilder (textField.Text);
					aStringBuilder.Remove (range.Location, range.Length);
					aStringBuilder.Insert (range.Location, replacementString);

					string textAfter =  aStringBuilder.ToString ();


					int proposedDecade = (textAfter.ToCharArray () [3] - '0') * 10;
					int yearDecade = currentYear - (currentYear % 10);

					if (proposedDecade > yearDecade) {
						FlashCheckDateLabel ();
						return false;
					}

					if (textLengthAfter == 5) {
						if (!cardHelper.IsStartDateValid (textAfter)) {
							FlashCheckDateLabel ();
							return false;
						}							

						var bStringBuilder = new StringBuilder (textField.Text);
						bStringBuilder.Remove (range.Location, range.Length);
						bStringBuilder.Insert (range.Location, replacementString);


						textField.Text =  bStringBuilder.ToString ();
						IssueNumberTextField.BecomeFirstResponder ();
						changeText = false;
					}
				}

				char[] placeHolder = "MM/YY".ToCharArray ();
				for (int iii = 0; iii < textLengthAfter; iii++) {
					placeHolder [iii] = ' ';
				}

				StateDatePlaceholder.Text = new string (placeHolder);
				return changeText;
			};


			IssueNumberTextField.ShouldChangeCharacters = (UITextField textField, NSRange nsRange, string replacementString) => {
				CSRange range = new CSRange ((int)nsRange.Location, (int)nsRange.Length);
				DispatchQueue.MainQueue.DispatchAsync (() => {
					UpdateUI ();
				});
				if (range.Length > 1) {
					return false;
				}
				if (replacementString.Length > 1) {
					return false;
				}
				if (replacementString.Length == 1 && !char.IsDigit (replacementString.ToCharArray () [0])) {
					return false;
				}
				if (textField.Text.Length + replacementString.Length - range.Length > 3) {
					return false;
				}
				return true;
			};
		}

		protected override void Dispose (bool disposing)
		{
			foreach (UITableViewCell cell in CellsToShow) {
				if (cell != null) {
					cell.Dispose ();
				}
			}

			if (ccBackImage != null) {
				ccBackImage.Dispose ();
			}
			base.Dispose (disposing);
		}


	}



}

