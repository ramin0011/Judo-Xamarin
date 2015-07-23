using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreAnimation;


namespace JudoDotNetXamariniOSSDK	      
{
	public partial class CreditCardView2 : UIViewController, IUIScrollViewDelegate
	{
		public CreditCardControllerType CreditCardControllerType {get; set;}
		public Card judoCard {get; set;}
		public event Action<bool, Card> CompletionBlock;


		private UILabel PostCodeLabel { get; set;}
		private UIView PostCodeBackgroundView {get; set;}
		private UITextField PostCodeTextField {get; set;}
		private UIButton CountryButton {get; set;}
		private UILabel CountryLabel {get; set;}
		private UIButton HomeButton {get; set;}
		private UILabel CountryWarningLabel {get; set;}
		private UIView PostCodeContainerView {get; set;}

		private UITextField StartDateTextField {get; set;}
		private UILabel StartDateLabel { get; set;}
		private UIView StartDateContainerView {get; set;}
		private UILabel StartDatePlaceholder {get; set;}
		private UILabel StartDateWarningLabel {get; set;}
		private UITextField IssueNumberTextView {get; set;}
		private UILabel IssueNumberLabel {get; set;}
		private UIView IssueNumberContainerView {get; set;}

		private UIView PickerViewContainer {get; set;}
		private UIPickerView PickerView {get; set;}
		private UIButton PickerDoneCoverButton {get; set;}

		private UILabel TransactionInfoLabel {get; set;}
//		private UIButton SubmitButton {get; set;}
//		private UIButton CancelButton {get; set;}

		//private BSKeyboardControls KeyboardControls {get; set;}
		private UIButton NumberFieldClearButton {get; set;}
		private UIButton ExpiryInfoButton {get; set;}

		private UILabel StatusHelpLabel { get; set;}
		private UILabel PleaseRecheckNumberLabel {get; set;}
		private UITableViewCell[] CellsToShow {get; set;}

		private NSLayoutConstraint PickBottomConstraint {get; set;}


		private UIView warningView;
		private UIButton updateCard;
		private UITextView dummyTextView;


		UIImageView ccBackImage;

		nfloat oldX;
		nint currentYear;

		CreditCardType type;
		nuint numberLength;
		string creditCardNum;
		nint month;
		nint year;
		nint ccv;

		bool haveFullNames;
		bool completelyDone;

		string successMessage;




		public CreditCardView2() : base("CreditCardView2",null)
		{
		}


		private bool prefersStatusBarHidden()
		{
			return NavigationController == null;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			this.View.BackgroundColor = UIColor.FromRGB(245f,245f,245f);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			SetUpTableView ();

			this.View.BackgroundColor = UIColor.FromRGB(245f,245f,245f);

			switch (CreditCardControllerType) 
			{
			case CreditCardControllerType.CreditCardControllerTypeAddCard:
			case CreditCardControllerType.CreditCardControllerRegisterCard:
				{
					Title = ThemeBundleReplacement.BundledOrReplacementString("registerCardType", BundledOrReplacementOptions.BundledOrReplacement);
					break;
				}
			default:
				break;
			}

			NSNotificationCenter defaultCenter = NSNotificationCenter.DefaultCenter;
			defaultCenter.AddObserver(UIKeyboard.WillShowNotification, keyboardMoving);
			defaultCenter.AddObserver(UIKeyboard.DidShowNotification, keyboardMoving);
			defaultCenter.AddObserver(UIKeyboard.WillHideNotification, keyboardMoving);

			SubmitButton.SetTitleColor (UIColor.Black, UIControlState.Application);
			UIEdgeInsets insets = new UIEdgeInsets (0, 20, 0, 20);
			UIImage activeImage = ThemeBundleReplacement.BundledOrReplacementImage ("btn_pay_normal_iPhone6", BundledOrReplacementOptions.BundledOrReplacement);
			UIImage inactiveImage = ThemeBundleReplacement.BundledOrReplacementImage ("btn_pay_inactive_iPhone6", BundledOrReplacementOptions.BundledOrReplacement);
			UIImage resizableActiveImage = activeImage.CreateResizableImage (insets);
			UIImage resizableInactiveImage = inactiveImage.CreateResizableImage (insets);

			SubmitButton.SetBackgroundImage (resizableActiveImage, UIControlState.Normal);
			SubmitButton.SetBackgroundImage (resizableInactiveImage, UIControlState.Disabled);

			CancelButton.SetTitleColor (ThemeBundleReplacement.BundledOrReplacementColor("GRAYw_COLOR", BundledOrReplacementOptions.BundledOrReplacement), UIControlState.Normal);



			editCard();

		}

		void SetUpTableView ()
		{
			CellsToShow = new UITableViewCell[]{CardDetailCell, ReassuringTextCell };

			CGRect rectangle = ccText.Frame;
			//rectangle.Size.Height = 36;
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
			layer.BorderColor = ColourHelper.GetColour("0xC3C3C3FF").CGColor;  //ThemeBundleReplacement.BundledOrReplacementColor ("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.Bundled).CGColor;
			layer.BorderWidth = 1;
			layer = textScroller.Layer;
			layer.CornerRadius = 4.0f;
			layer.MasksToBounds = true;
			layer.BorderWidth = 0;

			textScroller.ScrollEnabled = false;
	
			ccText.Text = "0000 0000 0000 0000";

			UITextPosition start = ccText.BeginningOfDocument;
			UITextPosition end = ccText.GetPosition (start, 24);
			UITextRange range = ccText.GetTextRange (start, end);
			CGRect r = ccText.GetFirstRectForRange (range);
			//r.Size.Width /= 24.0f;
			//ccText.Text = String.Empty;

			CGRect frame = placeView.Frame;

			//placeView = new PlaceHolderTextView ();
			placeView.Font = ccText.Font;
			placeView.Text = "0000 0000 0000 0000";
			placeView.ShowTextOffset = 0;
			placeView.Offset = r;
	
			placeView.BackgroundColor =UIColor.White;
			textScroller.InsertSubview (placeView, 0);

			type = CreditCardType.InvalidCard;


			//dummyTextView.BecomeFirstResponder ();

			AddPaymentTableSource tableSource = new AddPaymentTableSource (CellsToShow);
			TableView.Source = tableSource;
			TableView.SeparatorColor = UIColor.Clear;

			NSArray fields = NSArray.FromObjects (ccText,dummyTextView , PostCodeTextField, StartDateTextField, IssueNumberTextView);
		}

		void SetUpMaskedInput()
		{
				
			ccText.ShouldChangeText.ShouldChangeCharacters = (UITextField textField, NSRange range, string replace) =>
			{
				string formattedText;
				bool flashForError = false;
				bool updateText = false;
				bool scrollForward = false;
				bool deleting = false;
				bool ret = false;
				bool deletedSpace = false;



				completelyDone = false;
				if(textField.Text.Length == 0)
				{
					updateText = true;
					deleting = true;
					if(textField.Text.Length) 
					{	// handle case of delete when there are no characters left to delete
						char c = textField.Text.Substring(range.Location);
						if(range.Location && range.Length == 1 && (c == ' ' || c == '/')) {
							--range.Location;
							++range.Location;
							deletedSpace = false;
						}
					} else {
						return false;
					}
				}

				string newTextOrig = this.Text.Substring(range.Location,range.Length);// [textView.text stringByReplacingCharactersInRange:range withString:text];
				int newTextLen = newTextOrig.length;

				// added by Rob Phillips
				// causes the cc entry field to scroll back if the user deletes back beyond the end of the cc number
				if (range.Location <= numberLength)
				{
					haveFullNumber = false;
				}

				if(haveFullNumber)
				{
					// commented out by Rob Phillips
					//		if(range.location <= numberLength) {	// <= account for space after last cc digit
					//			updateText = NO;
					//			flashForError = YES;
					//			goto eND;
					//		}


					if (range.Location > numberLength)
					{
						scrollForward = true;
					}

					// Test for delete of a space or /
					if(deleting) {
						formattedText = newTextOrig.Substring(range.Location); //[newTextOrig substringToIndex:range.location];	// handles case of deletion interior to the string
						updateText = false;
						EndDelegate();
					}

					if(newTextLen > placeView.Text.Length) {
						flashForError = true;
						EndDelegate();
					}

					formattedText = newTextOrig;

					CSRange monthRange = new CSRange(placeView.Text.IndexOf("MM"),2); // rangeOfString:@"MM"];
					if(newTextLen > monthRange.Location) {
						if(newTextOrig.Substring(monthRange.Location,1) > '1') {
							// support short cut - we prepend a '0' for them
							formattedText = newTextOrig = "0"+newTextOrig; // stringByReplacingCharactersInRange:range withString:[@"0" stringByAppendingString:text];
							newTextLen = newTextOrig.Length;
						}
						if(newTextLen >= (monthRange.Location + monthRange.Length)) {
							var month = Int32.Parse( newTextOrig.Substring(monthRange.Location,monthRange.Length));
							if(month < 1 || month > 12) {
								flashRecheckExpiryDateMessage = true;
								EndDelegate();
							}
						}
					}

					CSRange yearRange =  new CSRange(placeView.Text.IndexOf("YY"),2);// rangeOfString:@"YY";
					if(newTextLen > yearRange.Location) {
						int proposedDecade = (newTextOrig.Substring(yearRange.Location,1) - '0') * 10;
						int yearDecade = currentYear - (currentYear % 10);
						// NSLog(@"proposedDecade=%u yearDecade=%u", proposedDecade, yearDecade);
						if(proposedDecade < yearDecade) {
							flashRecheckExpiryDateMessage=true;
							EndDelegate();
						}
						if(newTextLen >= (yearRange.location + yearRange.length)) {
							year = newTextOrig.Substring(yearRange.Location,yearRange.Length); // [[newTextOrig substringWithRange:yearRange] integerValue];
							int diff = year - currentYear;
							if(diff < 0 || diff > 10) {	// blogs on internet suggest some CCs have dates 50 yeras in the future
								flashRecheckExpiryDateMessage=true;
								EndDelegate(); 
							}
							if(diff == 0) { // The entered year is the current year, so check that the month is in the future
								//NSDateComponents *components = [[NSCalendar currentCalendar] components:NSCalendarUnitDay | NSCalendarUnitMonth | NSCalendarUnitYear fromDate:[NSDate date]];
								var todaysDate =DateTime.Today;
								int currentMonth = todaysDate.Month;
								if (month < currentMonth) {
									flashRecheckExpiryDateMessage=true;
									EndDelegate();
								}
							}
							if(creditCardImage != ccBackImage)
							{
								UIViewAnimationOptions transType = (type == AMEX) ? UIViewAnimationOptionTransitionCrossDissolve : UIViewAnimationOptions.TransitionFlipFromBottom;


								creditCardImage.Animate (
										duration: 0.25f, 
										delay: 0,
										options: transType,
									animation: () => { creditCardImage = ccBackImage; },
									completion: () => { self.statusHelpLabel.text = ThemeBundleReplacement.BundledOrReplacementString("enterCardSecurityCodeText", BundledOrReplacementOptions.BundledOrReplacement); });
								
//								[UIView transitionFromView:creditCardImage toView:ccBackImage duration:0.25f options:transType completion:NULL];
//								creditCardImage = ccBackImage;

							}
						}
					}

					if(newTextLen == placeView.Text.Length) {
						completelyDone = true;

						CSRange ccvRange =new CSRange(placeView.Text.IndexOf("C"));// [placeView.text rangeOfString:@"C"]; // first one
						ccvRange.Length = type == AMEX ? 4 : 3;
						ccv = Int32.Parse( newTextOrig.Substring(ccvRange.Location,ccvRange.Length));//   substringWithRange:ccvRange] integerValue];
					}

					updateText = true;
				}
				else
				{
					// added by Rob Phillips
					// scrolls backward
					NSUInteger textViewLen = [[CreditCard formatForViewing:ccText.text] length];
					NSUInteger formattedLen = [placeView.text length];
					placeView.showTextOffset = MIN(textViewLen, formattedLen);
					textScroller.scrollEnabled = NO;
					[textScroller setContentOffset:CGPointMake(0, 0) animated:YES];

					self.statusHelpLabel.text = [ThemeBundleReplacement bundledOrReplacementStringNamed:@"enterCardDetailsText"];
					// added by Rob Phillips

					NSString *newText = [newTextOrig stringByReplacingOccurrencesOfString:@" " withString:@""];
					NSUInteger len = [newText length];
					if(len < CC_LEN_FOR_TYPE) {
						updateText = YES;
						formattedText = newTextOrig;
						// NSLog(@"NEWLEN=%d CC_LEN=%d formattedText=%@", len, CC_LEN_FOR_TYPE, formattedText);
						type = InvalidCard;
					} else {
						type = [CreditCard ccType:newText];
						if(type == InvalidCard) {
							flashForError = YES;
							goto eND;
						}
						if (type == Maestro && ![JudoSDKManager getMaestroAccepted]) {
							[self flashMessage:[ThemeBundleReplacement bundledOrReplacementStringNamed:@"maestroNotAcceptedText"]];
							goto eND;
						}
						if (type == AMEX && ![JudoSDKManager getAmExAccepted]) {
							[self flashMessage:[ThemeBundleReplacement bundledOrReplacementStringNamed:@"amexNotAcceptedText"]];
							goto eND;
						}
						if(len == CC_LEN_FOR_TYPE) {
							placeView.text = [CreditCard promptStringForType:type justNumber:YES];
						}
						formattedText = [CreditCard formatForViewing:newText];
						NSUInteger lenForCard = [CreditCard lengthOfStringForType:type];

						// NSLog(@"FT=%@ len=%d", formattedText, lenForCard);

						if(len < lenForCard) {
							updateText = YES;
						} else
							if(len == lenForCard) {
								if([CreditCard isValidNumber:newText]) {
									if([CreditCard isLuhnValid:newText]) {
										numberLength = [CreditCard lengthOfFormattedStringForType:type];
										creditCardNum = newText;

										updateText = YES;
										scrollForward = YES;
										haveFullNumber = YES;
									} else {

										[self flashRecheckNumberMessage];
									}
								} else {
									[self flashRecheckNumberMessage];
								}				
							}
					}
					[self updateCCimageWithTransitionTime:0.25f];
				}
				void EndDelegate()
				{

					// Order of these blocks important!
					if(scrollForward) {
						[self scrollForward:YES];
						dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (long long)250*NSEC_PER_MSEC), dispatch_get_main_queue(), ^{ [textScroller flashScrollIndicators]; } );
					}
					if(updateText) {
						NSUInteger textViewLen = [formattedText length];
						NSUInteger formattedLen = [placeView.text length];
						placeView.showTextOffset = MIN(textViewLen, formattedLen);

						if((formattedLen > textViewLen) && !deleting) {
							unichar c = [placeView.text characterAtIndex:textViewLen];
							if(c == ' ') formattedText = [formattedText stringByAppendingString:@" "];
							else
								if(c == '/') formattedText = [formattedText stringByAppendingString:@"/"];
						}
						if(!deleting || haveFullNumber || deletedSpace)
						{
							textView.text = formattedText;
						} else
						{
							ret = YES; // let textView do it to preserve the cursor location. User updating an incorrect number
						}
						// NSLog(@"formattedText=%@ PLACEVIEW=%@ showTextOffset=%u offset=%@ ret=%d", formattedText, placeView.text, placeView.showTextOffset, NSStringFromCGRect(placeView.offset), ret );

					}
					if(flashForError) {
						[self flashRecheckNumberMessage];
					}

					dispatch_async(dispatch_get_main_queue(), ^{ [self updateUI]; });
					//NSLog(@"placeholder=%@ text=%@", placeView.text, ccText.text);

					return ret;
				}


			}

		private void keyboardMoving(NSNotification note){

		}

		private void editCard()
		{

		}
	}

}

