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
		CreditCardType type;

		string formattedText;
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
		int cardMonth;

		public bool CompletelyDone {
			get;
			set;
		}
			
		bool deletedSpace = false;

		public CardEntryCell (IntPtr handle) : base (handle)
		{
			Key= "CardEntryCell";
		}



		public override CardCell Create ()
		{
			return (CardEntryCell)Nib.Instantiate (null, null) [0];
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			SetUpCell ();
		}


		void SetUpCell ()
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
						CompletelyDone = true;
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
			//			if (JudoSDKManager.MaestroAccepted) {
			//				SetUpStartDateMask ();
			//			}	
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

		public bool EndDelegate ()
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
			DispatchQueue.MainQueue.DispatchAsync (() => {
				//UpdateUI (); TODO work this out
				UpdateUI.Invoke();
			});
			return ret;
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
			//PostcodeTextField.ResignFirstResponder ();
			//StartDateTextField.ResignFirstResponder ();
			//IssueNumberTextField.ResignFirstResponder ();

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
	}
}

