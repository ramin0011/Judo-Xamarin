﻿using System;
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
	public partial class CreditCardView2 : UIViewController, IUIScrollViewDelegate
	{
		public CreditCardControllerType CreditCardControllerType { get; set; }

		public Card judoCard { get; set; }

		public event Action<bool, Card> CompletionBlock;

		CreditCard cardHelper = new CreditCard ();
	
		//private UIButton ExpiryInfoButton { get; set; }

		private List<UITableViewCell> CellsToShow { get; set; }


		UIImageView ccBackImage;
		UIImageView ccImage;


		int currentYear;

		CreditCardType type;
		int numberLength;
		string creditCardNum;
		int year;
		int ccv;

		int cardMonth;

		bool haveFullNames;
		bool completelyDone;

		string successMessage;

		string formattedText;
		bool flashForError = false;
		bool updateText = false;
		bool scrollForward = false;
		bool deleting = false;
		bool ret = false;
		bool hasFullNumber = false;
		bool deletedSpace = false;


		IPaymentService _paymentService;


		public CreditCardView2 (IPaymentService paymentService) : base ("CreditCardView2", null)
		{
			_paymentService = paymentService;
		}


		float widthToLastGroup {
			get { 
				int oldOffset = placeView.ShowTextOffset;
				int offsetToLastGroup = cardHelper.LengthOfFormattedStringTilLastGroupForType (type);// [CreditCard lengthOfFormattedStringTilLastGroupForType:type];
				placeView.SetShowTextOffSet (offsetToLastGroup);
				float width = placeView.WidthToOffset (); //[placeView widthToOffset];
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


		private bool prefersStatusBarHidden ()
		{
			return NavigationController == null;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.View.BackgroundColor = UIColor.FromRGB (245f, 245f, 245f);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			SetUpTableView ();

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
			defaultCenter.AddObserver (UIKeyboard.WillShowNotification, keyboardMoving);
			defaultCenter.AddObserver (UIKeyboard.DidShowNotification, keyboardMoving);
			defaultCenter.AddObserver (UIKeyboard.WillHideNotification, keyboardMoving);

			SubmitButton.SetTitleColor (UIColor.Black, UIControlState.Application);
			UIEdgeInsets insets = new UIEdgeInsets (0, 20, 0, 20);
			UIImage activeImage = ThemeBundleReplacement.BundledOrReplacementImage ("btn_pay_normal_iPhone6", BundledOrReplacementOptions.BundledOrReplacement);
			UIImage inactiveImage = ThemeBundleReplacement.BundledOrReplacementImage ("btn_pay_inactive_iPhone6", BundledOrReplacementOptions.BundledOrReplacement);
			UIImage resizableActiveImage = activeImage.CreateResizableImage (insets);
			UIImage resizableInactiveImage = inactiveImage.CreateResizableImage (insets);

			//SubmitButton.SetBackgroundImage (resizableActiveImage, UIControlState.Normal);
			//SubmitButton.SetBackgroundImage (resizableInactiveImage, UIControlState.Disabled);

			SubmitButton.TouchUpInside += (sender, ev) => {
				MakePayment ();
			};



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

		void SetUpTableView ()
		{
			CellsToShow = new List<UITableViewCell> (){ CardDetailCell, ReassuringTextCell };

			CGRect rectangle = ccText.Frame;
			ccText.Frame = rectangle;

			creditCardImage.Tag = (int)CreditCardType.InvalidCard;

			creditCardImage.Layer.CornerRadius = 4.0f;
			creditCardImage.Layer.MasksToBounds = true;

			//StatusHelpLabel.Text = ThemeBundleReplacement.BundledOrReplacementString ("enterCardDetailsText", BundledOrReplacementOptions.BundledOrReplacement);

			UIImage image = ThemeBundleReplacement.BundledOrReplacementImage ("ic_card_large_unknown", BundledOrReplacementOptions.BundledOrReplacement);

			creditCardImage.Image = image;

			currentYear = DateTime.Now.Year - 2000;

			CALayer layer = containerView.Layer;
			layer.CornerRadius = 4.0f;
			layer.MasksToBounds = true;
			layer.BorderColor = ColourHelper.GetColour ("0xC3C3C3FF").CGColor;  //ThemeBundleReplacement.BundledOrReplacementColor ("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.Bundled).CGColor;
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

			AddPaymentTableSource tableSource = new AddPaymentTableSource (CellsToShow.ToArray ());
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
						if (range.Location != null && range.Length == 1 && (c == ' ' || c == '/')) {
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

				// added by Rob Phillips
				// causes the cc entry field to scroll back if the user deletes back beyond the end of the cc number
				if (range.Location <= numberLength) {
					hasFullNumber = false;
				}

				if (hasFullNumber) {
					// commented out by Rob Phillips
					//		if(range.location <= numberLength) {	// <= account for space after last cc digit
					//			updateText = NO;
					//			flashForError = YES;
					//			goto eND;
					//		}


					if (range.Location > numberLength) {
						//	ScrollForward (true);
					}



					// Test for delete of a space or /
					if (deleting) {
						formattedText = newTextOrig.Substring (0, range.Location); //[newTextOrig substringToIndex:range.location];	// handles case of deletion interior to the string
						updateText = true;
						return EndDelegate ();
					}

					if (newTextLen > placeView.Text.Length) {
						flashForError = true;
						return EndDelegate ();
					}

					formattedText = newTextOrig;

					CSRange monthRange = new CSRange (placeView.Text.IndexOf ("MM"), 2); // rangeOfString:@"MM"];
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

					CSRange yearRange = new CSRange (placeView.Text.IndexOf ("YY"), 2);// rangeOfString:@"YY";
					if (newTextLen > yearRange.Location) {
						int proposedDecade = (newTextOrig.Substring (yearRange.Location, 1).ToCharArray () [0] - '0') * 10;
						int yearDecade = currentYear - (currentYear % 10);
						if (proposedDecade < yearDecade) {
							flashRecheckExpiryDateMessage ();
							return EndDelegate ();
						}
						if (newTextLen >= (yearRange.Location + yearRange.Length)) {
							year = Int32.Parse (newTextOrig.Substring (yearRange.Location, yearRange.Length)); // [[newTextOrig substringWithRange:yearRange] integerValue];
							int diff = year - currentYear;
							if (diff < 0 || diff > 10) {	// blogs on internet suggest some CCs have dates 50 yeras in the future
								flashRecheckExpiryDateMessage ();
								return EndDelegate ();
							}
							if (diff == 0) { // The entered year is the current year, so check that the month is in the future
								//NSDateComponents *components = [[NSCalendar currentCalendar] components:NSCalendarUnitDay | NSCalendarUnitMonth | NSCalendarUnitYear fromDate:[NSDate date]];
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
									});//ThemeBundleReplacement.BundledOrReplacementString("enterCardSecurityCodeText", BundledOrReplacementOptions.BundledOrReplacement); });								

							}
						}
					}

					if (newTextLen == placeView.Text.Length) {
						completelyDone = true;
						var cIndex = placeView.Text.IndexOf ("C");
						CSRange ccvRange = new CSRange (cIndex, placeView.Text.Substring (cIndex).Length);// [placeView.text rangeOfString:@"C"]; // first one
						ccvRange.Length = type == CreditCardType.AMEX ? 4 : 3;
						ccv = Int32.Parse (newTextOrig.Substring (ccvRange.Location, ccvRange.Length));//   substringWithRange:ccvRange] integerValue];
					}

					updateText = true;
				} else {
					// scrolls backward
					int textViewLen = ccText.Text.Length; //[[CreditCard formatForViewing:ccText.text] length];
					int formattedLen = placeView.Text.Length;
					placeView.SetShowTextOffSet (Math.Min (textViewLen, formattedLen)); //MIN(textViewLen, formattedLen);
					textScroller.ScrollEnabled = false;

					textScroller.SetContentOffset (new CGPoint (0, 0), true);

					StatusHelpLabel.Text = "Enter Card Details";// ThemeBundleReplacement.BundledOrReplacementString("enterCardDetailsText", BundledOrReplacementOptions.BundledOrReplacement);


					string newText = newTextOrig.Replace (" ", String.Empty);
					int len = newText.Length;
					if (len < Card.CC_LEN_FOR_TYPE) {
						updateText = true;
						formattedText = newTextOrig;

						type = CreditCardType.InvalidCard;
					} else {
						type = cardHelper.GetCCType (newText);
						switch (type) {
						// The following switch section causes an error.
						case CreditCardType.InvalidCard:
							flashForError = true;
							break;
						case CreditCardType.Maestro:
							if (!JudoSDKManager.MaestroAccepted) {

								flashForError = true; // maestroNotAcceptedText 
								return EndDelegate ();
							}
							break;

						case CreditCardType.AMEX:
							if (!JudoSDKManager.AmExAccepted) {

								flashForError = true; // amexNotAcceptedText bundled
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


				
		}


		void UpdateCCimageWithTransitionTime (float transittionTime, bool isBack = false)
		{
			if (creditCardImage.Tag != (int)type) {

				UIImage frontImage = cardHelper.CreditCardImage (type);
				ccImage = new UIImageView (frontImage); //[[UIImageView alloc] initWithImage:frontImage];
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

			_paymentService.MakePayment (payment).ContinueWith (reponse => {
				var result = reponse.Result;
				if (!result.HasError) {
					PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;
					PaymentReceiptViewModel receipt = new PaymentReceiptViewModel () {
						CreatedAt = paymentreceipt.CreatedAt.DateTime,
						Currency = paymentreceipt.Currency,
						OriginalAmount = paymentreceipt.Amount,
						ReceiptId = paymentreceipt.ReceiptId
					};


					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
						
						var view = JudoSDKManager.GetReceiptView (receipt);
						this.NavigationController.PushViewController (view, true);
	
					});
				} else {
					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
						
						var errorText = result.Error.ErrorMessage;
						UIAlertView _error = new UIAlertView ("Payment failed", errorText, null, "ok", null);
						_error.Show ();
					});
				}

			});

		}

		CardViewModel GatherCardDetails ()
		{
			CardViewModel cardViewModel = new CardViewModel () {
				CardName = "Ed Xample",
				CardNumber = creditCardNum,
				CV2 = ccv,
				ExpireDate = cardMonth + "/" + year,
				CardType = type
					
			};

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
			// Order of these blocks important!
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
					char c = placeView.Text.Substring (textViewLen, 1).ToCharArray () [0];// characterAtIndex:textViewLen];
					if (c == ' ')
						formattedText = formattedText + " "; //[formattedText stringByAppendingString:@" "];
					else if (c == '/')
						formattedText = formattedText + "/"; //[formattedText stringByAppendingString:@"/"];
				}
				if (!deleting || hasFullNumber || deletedSpace) {
					ccText.Text = formattedText;
				} else {
					ret = true; // let textView do it to preserve the cursor location. User updating an incorrect number
				}
				// NSLog(@"formattedText=%@ PLACEVIEW=%@ showTextOffset=%u offset=%@ ret=%d", formattedText, placeView.text, placeView.showTextOffset, NSStringFromCGRect(placeView.offset), ret );

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
				StatusHelpLabel.Text = "Please enter Expire Date";//[ThemeBundleReplacement bundledOrReplacementStringNamed:@"enterExpiryDateText"];
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

			//UpdateCCimageWithTransitionTime (0.25f, true);

			//UIView.Transition (creditCardImage, finalImage, transittionTime, UIViewAnimationOptions.TransitionFlipFromLeft, null);

			creditCardImage = ccBackImage;
			UIView.Transition (creditCardImage, ccBackImage, 0.25f, UIViewAnimationOptions.TransitionFlipFromLeft, null);
		}

		void DismissKeyboardAction ()
		{			
			placeView.ResignFirstResponder ();
			ccText.ResignFirstResponder ();
		}

		void PickerDoneButtonPressed ()
		{
			throw new NotImplementedException ();
		}

		private void UpdateUI ()
		{
			bool enable = false;
			enable = completelyDone;

			if (completelyDone) {
				DismissKeyboardAction ();
			}
				
			SubmitButton.Enabled = enable;
		}

		//		private void UpdateUI ()
		//		{
		//			bool enable = false;
		//			enable = completelyDone;
		//
		//			this.NavigationItem.RightBarButtonItem.Enabled = true;
		//
		//			//NSMutableArray *cellsToRemove = [NSMutableArray array];
		//			UITableViewCell[] cellsToRemove;
		//			//NSMutableArray *insertedCells = [NSMutableArray array];
		//			UITableViewCell[] insertedCells;
		//			//NSMutableArray *cellsBeforeUpdate = [self.cellsToShow copy];
		//			UITableViewCell[] cellsBeforeUpdate;
		//			Array.Copy (CellsToShow.ToArray(), cellsBeforeUpdate);
		//			TableView.BeginUpdates ();
		//
		//
		//			if(enable)
		//			{
		//				bool ccIsFirstResponder = ccText.IsFirstResponder;
		//				if (type == CreditCardType.Maestro && JudoSDKManager.MaestroAccepted) {
		//					if (!CellsToShow.Any(x=> x== MaestroCell)) {
		//						int row = CellsToShow.IndexOf(CardDetailCell) + 1;
		//						CellsToShow.Insert (MaestroCell, row);
		//						//[insertedCells addObject:self.maestroCell];
		//						insertedCells.add(MaestroCell);//// you sre here
		//					}
		//
		//					if (_issueNumberTextField.text.length==0 || !(_startDateTextField.text.length == 5)) { //SCRUTINIZE THIS
		//						enable = false;
		//					}
		//
		//					if (ccIsFirstResponder) {
		//						startDateTextField.BecomeFirstResponder;
		//						//[self.startDateTextField becomeFirstResponder];
		//						ccIsFirstResponder = false;
		//					}
		//				}
		//
		//				if (JudoSDKManager.GetAVSEnabled()) {
		//					if (!cellsToShow.Contains(AVSCell)) {
		//						int row = cellsToShow.IndexOfObject(reassuringTextCell);
		//						cellsToShow.InsertObject(AVSCell,row);
		//						insertedCells.add(AVSCell);
		//					}
		//
		//					if (ccIsFirstResponder) {
		//						postCodeTextField.BecomeFirstResponder;
		//						ccIsFirstResponder = false;
		//					}
		//
		//					if (pickerView.SelectedRowInComponent[0] == BillingCountryOptionOther) {
		//						enable = false;
		//					} else {
		//						// check postcode is OK.
		//					}
		//				}
		//
		//				if (ccIsFirstResponder) {
		//					DismissKeyboardAction ();
		//					ccIsFirstResponder = false;
		//				}
		//			} else {
		//				if (JudoSDKManager.GetMaestroAccepted()) {
		//					if (cellsToShow.Contains(MaestroCell)) {
		//						//[cellsToRemove addObject:self.maestroCell];
		//						cellsToRemove.add(MaestroCell);
		//					}
		//				}
		//
		//				if (JudoSDKManager.GetAVSEnabled()) {
		//
		//					if (cellsToShow.Contains(AVSCell)) {
		//						cellsToRemove.add(AVSCell);
		//					}
		//				}
		//			}
		//
		//			NSIndexPath[] indexPathsToRemove;
		//			foreach (UITableViewCell cell in cellsToRemove)
		//			{
		//				//[indexPathsToRemove addObject:[NSIndexPath indexPathForRow:[cellsBeforeUpdate indexOfObject:cell] inSection:0]];
		//
		//			}
		//
		//			TableView.DeleteRows (indexPathsToRemove, UITableViewRowAnimation.Fade); //  deleteRowsAtIndexPaths:indexPathsToRemove withRowAnimation:UITableViewRowAnimationFade];
		//			//[self.cellsToShow removeObjectsInArray:cellsToRemove];
		//			CellsToShow.Remove(cellsToRemove);
		//
		//			NSIndexPath[] indexPathsToAdd;
		//			//NSMutableArray *indexPathsToAdd = [NSMutableArray array];
		//
		//			foreach (UITableViewCell cell in insertedCells)
		//			{
		//				//[indexPathsToAdd addObject:[NSIndexPath indexPathForRow:[self.cellsToShow indexOfObject:cell] inSection:0]];
		//
		//			}
		//
		//			TableView.InsertRows(indexPathsToAdd, UITableViewRowAnimation.Fade);
		//
		//			TableView.EndUpdates ();
		//
		//			SubmitButton.Enabled = enable;
		//		}
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

