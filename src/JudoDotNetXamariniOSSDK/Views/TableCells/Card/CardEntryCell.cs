using System;
using UIKit;
using Foundation;

using CoreGraphics;
using ObjCRuntime;
using System.Drawing;
using System.Collections.Generic;
using CoreAnimation;
using System.Text;
using CoreFoundation;



namespace JudoDotNetXamariniOSSDK
{
	public partial class CardEntryCell : CardCell
	{
		public static readonly UINib Nib = UINib.FromName ("CardEntryCell", NSBundle.MainBundle);

		public UITextView ccTextOutlet { get { return ccText; } }

		int currentYear;

		CreditCard cardHelper = new CreditCard ();
		public CreditCardType Type {get; set;}

		bool flashForError = false;
		bool updateText = false;
		bool scrollForward = false;

		int numberLength;

		UIImageView ccBackImage;
		UIImageView ccImage;

		string ccv;
		string creditCardNum;
		bool deleting = false;
		bool ret = false;
		int year;
		bool hasFullNumber = false;



		bool hasFullDate = false;

		bool hasFullCCV = false;

		int cardMonth;

		public bool CompletelyDone {
			get;
			set;
		}

		bool deletedSpace = false;

		public CardEntryCell (IntPtr handle) : base (handle)
		{
			Key = "CardEntryCell";
		}



		public override CardCell Create ()
		{
			return (CardEntryCell)Nib.Instantiate (null, null) [0];
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

		}

		public override void SetUpCell ()
		{

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

			textScroller.SetContentOffset (new CGPoint (0, 0), true);

			textScroller.ScrollEnabled = false;

			SetupPlaceViews ();

			Type = CreditCardType.InvalidCard;
			SetUpMaskedInput ();


			ExpiryInfoButton.TouchUpInside += (sender, ev) => {
				PushExpiryInfoView ();
			};

		}

		void SetupPlaceViews()
		{
  
			ccText.Text = "11112222333344445555";

			UITextPosition start = ccText.BeginningOfDocument;
			UITextPosition end = ccText.GetPosition (start, 20);
			UITextRange range = ccText.GetTextRange (start, end);
			CGRect r = ccText.GetFirstRectForRange (range);
			CGSize frameRect = r.Size;
			frameRect.Width = (r.Size.Width / 24.0f);
			ccText.Font = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;
			r.Size = frameRect;
			ccText.Text = "";


			CGRect frame = ccPlaceHolder.Frame;
			ccPlaceHolder.Font =  JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;
			ccPlaceHolder.Text = "0000 0000 0000 0000";

			ccPlaceHolder.SetShowTextOffSet (0);
			ccPlaceHolder.Offset = r;

			ccPlaceHolder.BackgroundColor = UIColor.Clear;
			textScroller.InsertSubview (ccPlaceHolder, 0);

			/////////

			expiryText.Text = "MM/YY";

			UITextPosition exStart = expiryText.BeginningOfDocument;
			UITextPosition exEnd = expiryText.GetPosition (exStart, 5);
			UITextRange exRange = expiryText.GetTextRange (exStart, exEnd);
			CGRect exR = expiryText.GetFirstRectForRange (exRange);
			CGSize exFrameRect = exR.Size;
			exFrameRect.Width = (exR.Size.Width / 24.0f);
			expiryText.Font = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;
			exR.Size = exFrameRect;
			expiryText.Text = "";


	
			CGRect expiryFrame = expiryPlaceHolder.Frame;
			expiryPlaceHolder.Font = expiryText.Font;
			expiryPlaceHolder.Text = "MM/YY";

			expiryPlaceHolder.SetShowTextOffSet (0);
			expiryPlaceHolder.Offset = exR;

			expiryPlaceHolder.BackgroundColor = UIColor.Clear;
			textScroller.InsertSubview (expiryPlaceHolder, 1);

			////

			cvTwoText.Text = "CV2";

			UITextPosition cvStart = cvTwoText.BeginningOfDocument;
			UITextPosition cvEnd = cvTwoText.GetPosition (cvStart, 3);
			UITextRange cvRange = cvTwoText.GetTextRange (cvStart, cvEnd);
			CGRect cvR = cvTwoText.GetFirstRectForRange (cvRange);
			CGSize cvFrameRect = cvR.Size;
			cvFrameRect.Width = (cvR.Size.Width / 24.0f);
			cvTwoText.Font = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;
			cvR.Size = cvFrameRect;
			cvTwoText.Text = "";

			CGRect cvTwoFrame = cvTwoPlaceHolder.Frame;
			cvTwoPlaceHolder.Font = cvTwoText.Font;
			cvTwoPlaceHolder.Text = "CV2";

			cvTwoPlaceHolder.SetShowTextOffSet (0);
			cvTwoPlaceHolder.Offset = cvR;

			cvTwoPlaceHolder.BackgroundColor = UIColor.Clear;
			textScroller.InsertSubview (cvTwoPlaceHolder, 2);
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

		void SetUpMaskedInput ()
		{
			SetupCC ();
			SetupExpire ();
			SetupCVTwo ();
		}

		void SetupCC()
		{
			ccText.ShouldChangeText = (UITextView textView, NSRange NSRange, string replace) => {
				CSRange range = new CSRange ((int)NSRange.Location, (int)NSRange.Length);
				var formattedText = "";
				flashForError = false;
				updateText = false;
				scrollForward = false;
				deleting = false;
				ret = false;
				deletedSpace = false;
				cardMonth = 0;

				CompletelyDone = false;
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

				if(textScroller.ContentOffset.X< 10f)
				{
					ccPlaceHolderWidthConstraint.Constant =198f;
					ccPLaceHolderToScrollViewConstraint.Constant =-8f;
				}
				if (!hasFullNumber) 
				{

					// scrolls backward
					int textViewLen = ccText.Text.Length; 
					int formattedLen = ccPlaceHolder.Text.Length;
				
					textScroller.ScrollEnabled = false;

					textScroller.SetContentOffset (new CGPoint (0, 0), true);



					StatusHelpLabel.Text = "Enter Card Details";

					string newText = newTextOrig.Replace (" ", String.Empty);
					int len = newText.Length;
					if (len < Card.CC_LEN_FOR_TYPE) {
						updateText = true;
						formattedText = newTextOrig;

						Type = CreditCardType.InvalidCard;
					} else {
						Type = cardHelper.GetCCType (newText);
						switch (Type) {

						case CreditCardType.InvalidCard:
							flashForError = true;
							break;
						case CreditCardType.Maestro:
							if (!JudoSDKManager.MaestroAccepted) {

								flashForError = true;
								return EndDelegate (ccPlaceHolder,ccText,formattedText);
							}
							break;
						case CreditCardType.AMEX:
							if (!JudoSDKManager.AmExAccepted) {

								flashForError = true; 
								return EndDelegate (ccPlaceHolder,ccText,formattedText);
							}
							break;
						}

						if (len == Card.CC_LEN_FOR_TYPE) {
							ccPlaceHolder.Text = cardHelper.PromptStringForType (Type, true);
							cvTwoPlaceHolder.Text = cardHelper.CVTwoPromptForType (Type, true);
							cvTwoPlaceHolder.SetShowTextOffSet (Math.Min (0, 0));


						}

						formattedText = cardHelper.FormatForViewing (newText); 
						int lenForCard = cardHelper.LengthOfStringForType (Type); 

						if (len < lenForCard) {
							updateText = true;
						} else if (len == lenForCard) {
							if (cardHelper.isValidNumber (newText)) {
								if (cardHelper.IsLuhnValid (newText)) {
									numberLength = cardHelper.LengthOfFormattedStringForType (Type);
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
					
				return EndDelegate (ccPlaceHolder,ccText,formattedText);
			};
		}

		void SetupExpire ()
		{

			expiryText.ShouldChangeText = (UITextView textView, NSRange NSRange, string replace) => {
				CSRange range = new CSRange ((int)NSRange.Location, (int)NSRange.Length);
				flashForError = false;
				updateText = false;
				scrollForward = false;
				deleting = false;
				ret = false;
				deletedSpace = false;

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
						ccText.BecomeFirstResponder();

						if(ccText.Text.Length ==(cardHelper.LengthOfFormattedStringForType(Type)))
						{
							ccText.Text = ccText.Text.Remove(ccText.Text.Length - 1);
						}
						return false;
					}
				}

				var formattedText = "";
				scrollForward=false;

				var aStringBuilder = new StringBuilder (textView.Text);
				aStringBuilder.Remove (range.Location, range.Length);
				aStringBuilder.Insert (range.Location, replace);
				string newTextOrig = aStringBuilder.ToString ();

				int newTextLen = newTextOrig.Length;

				// Test for delete of a space or /
				if (deleting) {
					formattedText = newTextOrig.Substring (0, range.Location);
					updateText = true;
					return EndDelegate (expiryPlaceHolder,expiryText,formattedText);
				}

				if (newTextLen > expiryPlaceHolder.Text.Length) {
					flashForError = true;
					return EndDelegate (expiryPlaceHolder,expiryText,formattedText);
				}

				formattedText = newTextOrig;

				CSRange monthRange = new CSRange (expiryPlaceHolder.Text.IndexOf ("MM"), 2);
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
							return EndDelegate (expiryPlaceHolder,expiryText,formattedText);
						}
					}
				}

				CSRange yearRange = new CSRange (expiryPlaceHolder.Text.IndexOf ("YY"), 2);
				if (newTextLen > yearRange.Location) {
					int proposedDecade = (newTextOrig.Substring (yearRange.Location, 1).ToCharArray () [0] - '0') * 10;
					int yearDecade = currentYear - (currentYear % 10);
					if (proposedDecade < yearDecade) {
						flashRecheckExpiryDateMessage ();
						return EndDelegate (expiryPlaceHolder,expiryText,formattedText);
					}
					if (newTextLen >= (yearRange.Location + yearRange.Length)) {
						year = Int32.Parse (newTextOrig.Substring (yearRange.Location, yearRange.Length)); 
						int diff = year - currentYear;
						if (diff < 0 || diff > 10) {	
							flashRecheckExpiryDateMessage ();
							return EndDelegate (expiryPlaceHolder,expiryText,formattedText);
						}
						if (diff == 0) { 

							var todaysDate = DateTime.Today;
							int currentMonth = todaysDate.Month;

							if (cardMonth < currentMonth) {
								flashRecheckExpiryDateMessage ();
								return EndDelegate (expiryPlaceHolder,expiryText,formattedText);
							}
						}
						if (creditCardImage != ccBackImage) {
							UIViewAnimationOptions transType = (Type == CreditCardType.AMEX) ? UIViewAnimationOptions.TransitionCrossDissolve : UIViewAnimationOptions.TransitionFlipFromBottom;

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
				updateText = true;
				if (formattedText.Length==5) {
					hasFullDate=true;
					cvTwoText.BecomeFirstResponder();
				} 
				return EndDelegate (expiryPlaceHolder,expiryText,formattedText);
			};

		}

		void SetupCVTwo ()
		{
			cvTwoText.ShouldChangeText = (UITextView textView, NSRange NSRange, string replace) => {
				CSRange range = new CSRange ((int)NSRange.Location, (int)NSRange.Length);



				if (replace.Length == 0) {

				if (textView.Text.Length == 0) {	// handle case of delete when there are no characters left to delete
						expiryText.BecomeFirstResponder();

						if(expiryText.Text.Length ==5)
						{
							expiryText.Text = expiryText.Text.Remove(expiryText.Text.Length - 1);
						}
				
				}

				}

				if (cvTwoText.Text.Length== (Type == CreditCardType.AMEX ? 4 : 3)&& replace.Length != 0) 
					{
						return false;
					}


				scrollForward=false;
				var aStringBuilder = new StringBuilder (textView.Text);
				aStringBuilder.Remove (range.Location, range.Length);
				aStringBuilder.Insert (range.Location, replace);
				string newTextOrig = aStringBuilder.ToString ();

				int newTextLen = newTextOrig.Length;

				if (newTextLen == cvTwoPlaceHolder.Text.Length) {
					CompletelyDone = true;
					var cIndex = cvTwoPlaceHolder.Text.IndexOf ("C");
					CSRange ccvRange = new CSRange (cIndex, cvTwoPlaceHolder.Text.Substring (cIndex).Length);
					ccvRange.Length = Type == CreditCardType.AMEX ? 4 : 3;
					ccv = newTextOrig.Substring (ccvRange.Location, ccvRange.Length);
				}

				updateText = true;
				if (newTextOrig.Length== (Type == CreditCardType.AMEX ? 4 : 3)) {
					hasFullCCV=true;
					DismissKeyboardAction ();
				} 
				return EndDelegate (cvTwoPlaceHolder,cvTwoText,newTextOrig);
			};
		}

		void UpdateCCimageWithTransitionTime (float transittionTime, bool isBack = false)
		{
			if (creditCardImage.Tag != (int)Type) {

				UIImage frontImage = cardHelper.CreditCardImage (Type);
				ccImage = new UIImageView (frontImage);
				ccImage.Frame = creditCardImage.Frame;
				ccImage.Tag = (int)Type;

				ccBackImage = new UIImageView (cardHelper.CreditCardBackImage (Type));
				ccBackImage.Frame = creditCardImage.Frame;
				ccBackImage.Tag = (int)Type;

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

		public bool EndDelegate (PlaceHolderTextView placeView,UITextView textview,string formattedText)
		{




			if (scrollForward) {
				ScrollForward (true);
				DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
					textScroller.FlashScrollIndicators ();
				});
			} 
			if (updateText) {
				int textViewLen = formattedText.Length;
				int formattedLen = placeView.Text.Length;

				if ((formattedLen > textViewLen) && !deleting) {
					char c = placeView.Text.Substring (textViewLen, 1).ToCharArray () [0];
					if (c == ' ')
						formattedText = formattedText + " ";
					else if (c == '/')
						formattedText = formattedText + "/"; 

					textViewLen = formattedText.Length;
					formattedLen = placeView.Text.Length;

				}
				placeView.SetShowTextOffSet (Math.Min (textViewLen, formattedLen));
				if (!deleting || hasFullNumber || deletedSpace) {
					textview.Text = formattedText;
				} else {
					ret = true;
				}

			}
			if (flashForError) {
				FlashMessage ("Please recheck number");
			}
			DispatchQueue.MainQueue.DispatchAsync (() => {

				UpdateUI.Invoke ();
			});
			return ret;
		}

		float widthToLastGroup {
			get { 
				int oldOffset = ccPlaceHolder.ShowTextOffset;
				int offsetToLastGroup = cardHelper.LengthOfFormattedStringTilLastGroupForType (Type);
				ccPlaceHolder.SetShowTextOffSet (offsetToLastGroup);
				float width = ccPlaceHolder.WidthToOffset ();
				ccPlaceHolder.SetShowTextOffSet (oldOffset);
				return width;
			}
		}


		void ScrollForward (bool animated)
		{

			if(Type== CreditCardType.AMEX)
			{
				ccPlaceHolderWidthConstraint.Constant=178f;

			}
			else
			{
				ccPlaceHolderWidthConstraint.Constant =198f;
			}
			expiryText.BecomeFirstResponder ();
			if (creditCardImage != ccBackImage) {
				StatusHelpLabel.Text = "Please enter Expire Date";
			}
			float width = widthToLastGroup;
			CGRect frame = new CGRect (ccText.Frame.Location, new CGSize ((width) + textScroller.Frame.Size.Width, ccText.Frame.Size.Height));
			textScroller.ContentSize = new CGSize (frame.Size.Width, textScroller.ContentSize.Height);
			ccText.Frame = frame;

			ccPlaceHolder.SetText (cardHelper.PromptStringForType (Type, false));

			textScroller.ScrollEnabled = true;
			if (textScroller.ContentOffset != new CGPoint (width, 0)) {

				textScroller.SetContentOffset (new CGPoint (width, 0), animated);
			}


			UIView.Transition (creditCardImage, ccBackImage, 0.25f, UIViewAnimationOptions.TransitionFlipFromLeft, null);
		}

		public override void DismissKeyboardAction ()
		{			
			ccPlaceHolder.ResignFirstResponder ();
			ccText.ResignFirstResponder ();

			expiryText.ResignFirstResponder ();
			expiryPlaceHolder.ResignFirstResponder ();

			cvTwoPlaceHolder.ResignFirstResponder ();
			cvTwoText.ResignFirstResponder ();
		}

		public void flashRecheckExpiryDateMessage ()
		{
			FlashMessage ("Invalid Expiry Date");
		}

		void FlashRecheckNumberMessage ()
		{
			FlashMessage ("Invalid Card Number");
		}

		void FlashMessage (string message)
		{
			PaymentErrorLabel.Text = message;
			PaymentErrorLabel.Hidden = false;

			DispatchQueue.MainQueue.DispatchAfter (new DispatchTime (DispatchTime.Now, 1 * 1000000000), () => {
				PaymentErrorLabel.Hidden = true;

			});
		}

		public void GatherCardDetails (CardViewModel cardViewModel)
		{
			string _cardMonth = cardMonth.ToString ();
			if (cardMonth < 10) {
				_cardMonth = "0" + _cardMonth;
			}
			cardViewModel.CardNumber = creditCardNum;
			cardViewModel.CV2 = ccv;
			cardViewModel.ExpireDate = _cardMonth + "/" + year;
			cardViewModel.CardType = Type;
		}

		public void CleanUp ()
		{
		SetUpCell ();
	
			DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
				UIImage defaultImage = cardHelper.CreditCardImage (CreditCardType.InvalidCard);
				creditCardImage.Image = defaultImage;

			});
		}
			
	}
}

