using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreAnimation;
using System.Text;


namespace JudoDotNetXamariniOSSDK	      
{
	public partial class CreditCardView2 : UIViewController, IUIScrollViewDelegate
	{
		public CreditCardControllerType CreditCardControllerType {get; set;}
		public Card judoCard {get; set;}
		public event Action<bool, Card> CompletionBlock;

		CreditCard cardHelper = new CreditCard ();
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

		private UILabel PleaseRecheckNumberLabel {get; set;}
		private UITableViewCell[] CellsToShow {get; set;}

		private NSLayoutConstraint PickBottomConstraint {get; set;}


		private UIView warningView;
		private UIButton updateCard;
		private UITextView dummyTextView;


		UIImageView ccBackImage;

		float oldX;
		int currentYear;

		CreditCardType type;
		int numberLength;
		string creditCardNum;
		int month;
		int year;
		int ccv;

		bool haveFullNames;
		bool completelyDone;

		string successMessage;

		/// <summary>
		/// DElegate properties here to avoid GOTO
		/// </summary>
		string formattedText;
		bool flashForError = false;
		bool updateText = false;
		bool scrollForward = false;
		bool deleting = false;
		bool ret = false;
		bool hasFullNumber = false;
		bool deletedSpace = false;



	

		public CreditCardView2() : base("CreditCardView2",null)
		{
		}


		float widthToLastGroup {get{ 
				int oldOffset = placeView.ShowTextOffset;
				int offsetToLastGroup = cardHelper.LengthOfFormattedStringTilLastGroupForType (type);// [CreditCard lengthOfFormattedStringTilLastGroupForType:type];
				placeView.ShowTextOffset = offsetToLastGroup;
				float width = placeView.WidthToOffset (); //[placeView widthToOffset];
				placeView.ShowTextOffset = oldOffset;
				return width;
			}
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

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);


			float width =  widthToLastGroup;

			CGRect frame = ccText.Frame;
			frame.Size = new CGSize (width + textScroller.Frame.Size.Width, frame.Size.Height);


			ccText.Frame = frame;
			placeView.Frame = frame;
			textScroller.ContentSize =  new CGSize(frame.Size.Width, textScroller.ContentSize.Height);

			textScroller.ScrollEnabled = true;
			textScroller.SetContentOffset(new CGPoint(0f,0f),animated);
			// todo add theses pickerBottomConstraint = -self.pickerViewContainer.bounds.size.height; 
			//[self.pickerViewContainer layoutIfNeeded];

			if (float.Parse( UIDevice.CurrentDevice.SystemVersion.Replace(".","")) >= 800f) {
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
	
			ccText.Text = "000011112222333344445555";

			UITextPosition start = ccText.BeginningOfDocument;
			UITextPosition end = ccText.GetPosition (start, 24);
			UITextRange range = ccText.GetTextRange (start, end);
			CGRect r = ccText.GetFirstRectForRange (range);
			CGSize frameRect = r.Size;
			frameRect.Width = (r.Size.Width/ 24.0f);
			ccText.Font = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;
			r .Size= frameRect;
			ccText.Text = "";
			
			CGRect frame = placeView.Frame;


			placeView.Font = ccText.Font;
			placeView.Text = "0000 0000 0000 0000";

			placeView.ShowTextOffset = 0;
			placeView.Offset = r;
	
			placeView.BackgroundColor =UIColor.Clear;
			textScroller.InsertSubview (placeView, 0);


			type = CreditCardType.InvalidCard;


			//dummyTextView.BecomeFirstResponder ();

			AddPaymentTableSource tableSource = new AddPaymentTableSource (CellsToShow);
			TableView.Source = tableSource;
			TableView.SeparatorColor = UIColor.Clear;
			SetUpMaskedInput ();
			NSArray fields = NSArray.FromObjects (ccText,dummyTextView , PostCodeTextField, StartDateTextField, IssueNumberTextView);
		}

		void SetUpMaskedInput()
		{
			ccText.ShouldChangeText = (UITextView textView, NSRange NSRange, string replace) =>
			{
				CSRange range = new CSRange((int)NSRange.Location,(int)NSRange.Length);
				 formattedText="";
				 flashForError = false;
				 updateText = false;
				 scrollForward = false;
				 deleting = false;
				 ret = false;
				 deletedSpace = false;

				completelyDone = false;
				if(replace.Length == 0)
				{
					updateText = true;
					deleting = true;
					if(textView.Text.Length!=0) 
					{	// handle case of delete when there are no characters left to delete
						
						char c = textView.Text.Substring(range.Location,1).ToCharArray()[0];
						if(range.Location ==1 && range.Length == 1 && (c == ' ' || c == '/')) {
							range.Location--;
							range.Length++;
							deletedSpace = false;
						}
					} else {
						return false;
					}
				}
				var aStringBuilder = new StringBuilder(textView.Text);
				aStringBuilder.Remove(range.Location,range.Length);
				aStringBuilder.Insert(range.Location, replace);
				string newTextOrig = aStringBuilder.ToString();

				int newTextLen = newTextOrig.Length;

				// added by Rob Phillips
				// causes the cc entry field to scroll back if the user deletes back beyond the end of the cc number
				if (range.Location <= numberLength)
				{
					hasFullNumber = false;
				}

				if(hasFullNumber)
				{
					// commented out by Rob Phillips
					//		if(range.location <= numberLength) {	// <= account for space after last cc digit
					//			updateText = NO;
					//			flashForError = YES;
					//			goto eND;
					//		}


					if (range.Location > numberLength)
					{
						ScrollForward(true);
					}

					// Test for delete of a space or /
					if(deleting) {
						formattedText = newTextOrig.Substring(range.Location); //[newTextOrig substringToIndex:range.location];	// handles case of deletion interior to the string
						updateText = false;
						return EndDelegate();
					}

					if(newTextLen > placeView.Text.Length) {
						flashForError = true;
						return EndDelegate();
					}

					formattedText = newTextOrig;

					CSRange monthRange = new CSRange(placeView.Text.IndexOf("MM"),2); // rangeOfString:@"MM"];
					if(newTextLen > monthRange.Location) {
						if(newTextOrig.Substring(monthRange.Location,1).ToCharArray()[0] > '1') {
							// support short cut - we prepend a '0' for them

							var aStringBuilder2 = new StringBuilder(textView.Text);
							aStringBuilder2.Remove(range.Location,range.Length);
							aStringBuilder2.Insert(range.Location,"0"+ replace);
							formattedText = aStringBuilder2.ToString();


							//formattedText = newTextOrig = "0"+newTextOrig; // stringByReplacingCharactersInRange:range withString:[@"0" stringByAppendingString:text];
							newTextLen = newTextOrig.Length;
						}
						if(newTextLen >= (monthRange.Location + monthRange.Length)) {
							var month = Int32.Parse( newTextOrig.Substring(monthRange.Location,monthRange.Length));
							if(month < 1 || month > 12) {
								flashRecheckExpiryDateMessage();
								return EndDelegate();
							}
						}
					}

					CSRange yearRange =  new CSRange(placeView.Text.IndexOf("YY"),2);// rangeOfString:@"YY";
					if(newTextLen > yearRange.Location) {
						int proposedDecade = (newTextOrig.Substring(yearRange.Location,1).ToCharArray()[0] - '0') * 10;
						int yearDecade = currentYear - (currentYear % 10);
						// NSLog(@"proposedDecade=%u yearDecade=%u", proposedDecade, yearDecade);
						if(proposedDecade < yearDecade) {
							flashRecheckExpiryDateMessage();
							return EndDelegate();
						}
						if(newTextLen >= (yearRange.Location + yearRange.Length)) {
							year = Int32.Parse( newTextOrig.Substring(yearRange.Location,yearRange.Length)); // [[newTextOrig substringWithRange:yearRange] integerValue];
							int diff = year - currentYear;
							if(diff < 0 || diff > 10) {	// blogs on internet suggest some CCs have dates 50 yeras in the future
								flashRecheckExpiryDateMessage();
								return EndDelegate();
							}
							if(diff == 0) { // The entered year is the current year, so check that the month is in the future
								//NSDateComponents *components = [[NSCalendar currentCalendar] components:NSCalendarUnitDay | NSCalendarUnitMonth | NSCalendarUnitYear fromDate:[NSDate date]];
								var todaysDate =DateTime.Today;
								int currentMonth = todaysDate.Month;
								if (month < currentMonth) {
									flashRecheckExpiryDateMessage();
									return EndDelegate();
								}
							}
							if(creditCardImage != ccBackImage)
							{
								UIViewAnimationOptions transType = (type == CreditCardType.AMEX) ? UIViewAnimationOptions.TransitionCrossDissolve : UIViewAnimationOptions.TransitionFlipFromBottom;


								UIImageView.Animate (
										duration: 0.25f, 
										delay: 0,
										options: transType,
									animation: () => { creditCardImage = ccBackImage; },
									completion: () => { StatusHelpLabel.Text = "Replace with bundled security text";});//ThemeBundleReplacement.BundledOrReplacementString("enterCardSecurityCodeText", BundledOrReplacementOptions.BundledOrReplacement); });
								
//								[UIView transitionFromView:creditCardImage toView:ccBackImage duration:0.25f options:transType completion:NULL];
//								creditCardImage = ccBackImage;

							}
						}
					}

					if(newTextLen == placeView.Text.Length) {
						completelyDone = true;
						var cIndex = placeView.Text.IndexOf("C");
						CSRange ccvRange =new CSRange(cIndex,placeView.Text.Substring(cIndex).Length);// [placeView.text rangeOfString:@"C"]; // first one
						ccvRange.Length = type == CreditCardType.AMEX ? 4 : 3;
						ccv = Int32.Parse( newTextOrig.Substring(ccvRange.Location,ccvRange.Length));//   substringWithRange:ccvRange] integerValue];
					}

					updateText = true;
				}
				else
				{
					// added by Rob Phillips
					// scrolls backward
					int textViewLen = ccText.Text.Length; //[[CreditCard formatForViewing:ccText.text] length];
					int formattedLen = placeView.Text.Length;
					placeView.SetShowTextOffSet(Math.Min(textViewLen,formattedLen)); //MIN(textViewLen, formattedLen);
					textScroller.ScrollEnabled = false;
					//[textScroller setContentOffset:CGPointMake(0, 0) animated:YES];
					textScroller.SetContentOffset(new CGPoint(0,0),true);

					StatusHelpLabel.Text = "replace with proper text";// ThemeBundleReplacement.BundledOrReplacementString("enterCardDetailsText", BundledOrReplacementOptions.BundledOrReplacement);
					//self.statusHelpLabel.text = [ThemeBundleReplacement bundledOrReplacementStringNamed:@"enterCardDetailsText"];

					// added by Rob Phillips

				string newText = newTextOrig.Replace(" ", String.Empty);// stringByReplacingOccurrencesOfString:@" " withString:@""];
					int len = newText.Length;
					if(len < Card.CC_LEN_FOR_TYPE) { //CC_LEN_FOR_TYPE replace with logic
						updateText = true;
						formattedText = newTextOrig;
						// NSLog(@"NEWLEN=%d CC_LEN=%d formattedText=%@", len, CC_LEN_FOR_TYPE, formattedText);
						type = CreditCardType.InvalidCard;
					} else {
						type = cardHelper.GetCCType(newText);
					switch (type)
					{
					// The following switch section causes an error.
					case CreditCardType.InvalidCard:
						flashForError = true;
						break;
					case CreditCardType.Maestro:
						if(!JudoSDKManager.MaestroAccepted)
						{

							flashForError =true; // maestroNotAcceptedText 
							return EndDelegate();
						}
						break;

					case CreditCardType.AMEX:
						if(!JudoSDKManager.AmExAccepted)
						{

							flashForError =true; // amexNotAcceptedText bundled
							return EndDelegate();
						}
						break;
					}
							
						if(len == Card.CC_LEN_FOR_TYPE) {//CC_LEN_FOR_TYPE replace with logic
							placeView.Text = cardHelper.promptStringForType(type,true);
							/// NEED TO WRITE OR FIND CLASS/Dictionary to return correctPrompt for type      placeView.text = [CreditCard promptStringForType:type justNumber:YES];
						}

						formattedText = cardHelper.FormatForViewing(newText);   // Probably need to format it to look like a cardNumber //[CreditCard formatForViewing:newText];
					int lenForCard =  cardHelper.LengthOfStringForType(type) ; // NEED DICTIONARY NSObjectFlag Card TYPES TO LENGTH //CardHelper. //[CreditCard lengthOfStringForType:type];

						// NSLog(@"FT=%@ len=%d", formattedText, lenForCard);

						if(len < lenForCard) {
							updateText = true;
						} else
							if(len == lenForCard) {
								if(cardHelper.isValidNumber(newText))
									{
									if(cardHelper.IsLuhnValid(newText)) {
									numberLength = cardHelper.LengthOfFormattedStringForType(type);
									creditCardNum = newText;

									updateText = true;
									scrollForward = true;
									hasFullNumber = true;
								} else {

										FlashRecheckNumberMessage();
								}
							} else {
									FlashRecheckNumberMessage();
							}	

						}
				}
				///[self updateCCimageWithTransitionTime:0.25f]; ///NEED THIS METHOD
				
				}
				return EndDelegate();
				};


				
			}

		public void flashRecheckExpiryDateMessage ()
		{
			// TODO Implement flash message 
		}

		void FlashRecheckNumberMessage ()
		{
			// TODO Implement flash message 
		}

		public bool EndDelegate()
		{

			// Order of these blocks important!
			if(scrollForward) {
				ScrollForward(true);
				//dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (long long)250*NSEC_PER_MSEC), dispatch_get_main_queue(), ^{ [textScroller flashScrollIndicators]; } );
			}
			if(updateText) {
				int textViewLen = formattedText.Length;
				int formattedLen = placeView.Text.Length;
				placeView.ShowTextOffset = Math.Min(textViewLen, formattedLen);

				if((formattedLen > textViewLen) && !deleting) {
					char c = placeView.Text.Substring(textViewLen,1).ToCharArray()[0];// characterAtIndex:textViewLen];
					if (c == ' ')
						formattedText = formattedText + " "; //[formattedText stringByAppendingString:@" "];
					else if (c == '/')
						formattedText = formattedText + "/"; //[formattedText stringByAppendingString:@"/"];
				}
				if(!deleting || hasFullNumber || deletedSpace)
				{
					ccText.Text = formattedText;
				} else
				{
					ret = true; // let textView do it to preserve the cursor location. User updating an incorrect number
				}
				// NSLog(@"formattedText=%@ PLACEVIEW=%@ showTextOffset=%u offset=%@ ret=%d", formattedText, placeView.text, placeView.showTextOffset, NSStringFromCGRect(placeView.offset), ret );

			}
			if(flashForError) {
				//[self flashRecheckNumberMessage];
			}



			//dispatch_async(dispatch_get_main_queue(), ^{ [self updateUI]; });


			//NSLog(@"placeholder=%@ text=%@", placeView.text, ccText.text);

			return ret;
		}

		void ScrollForward (bool b)
		{
			throw new NotImplementedException ();
		}

		private void keyboardMoving(NSNotification note){

		}

		private void editCard()
		{

		}
	}

}

