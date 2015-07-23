
using System;
using UIKit;
using CoreGraphics;
using Foundation;
namespace JudoDotNetXamariniOSSDK
{
	[Register("CardNumberEntryView")]
	public class CardNumberEntryView : UITextView
	{

		public bool completelyDone { get; set;}
		public bool haveFullNumber { get; set;}
		public PlaceHolderTextView PlaceView { get; set;}

		public CardNumberEntryView (IntPtr p) : base(p)
		{
			this.ShouldChangeText.ShouldChangeCharacters = (UITextField textField, NSRange range, string replace) =>
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
				
									if(newTextLen > [placeView.text length]) {
										flashForError = YES;
										EndDelegate()
									}
				
									formattedText = newTextOrig;
				
									NSRange monthRange = [placeView.text rangeOfString:@"MM"];
									if(newTextLen > monthRange.location) {
										if([newTextOrig characterAtIndex:monthRange.location] > '1') {
											// support short cut - we prepend a '0' for them
											formattedText = newTextOrig = [textView.text stringByReplacingCharactersInRange:range withString:[@"0" stringByAppendingString:text]];
											newTextLen = [newTextOrig length];
										}
										if(newTextLen >= (monthRange.location + monthRange.length)) {
											month = [[newTextOrig substringWithRange:monthRange] integerValue];
											if(month < 1 || month > 12) {
												[self flashRecheckExpiryDateMessage];
												goto eND;
											}
										}
									}
				
									NSRange yearRange = [placeView.text rangeOfString:@"YY"];
									if(newTextLen > yearRange.location) {
										NSInteger proposedDecade = ([newTextOrig characterAtIndex:yearRange.location] - '0') * 10;
										NSInteger yearDecade = currentYear - (currentYear % 10);
										// NSLog(@"proposedDecade=%u yearDecade=%u", proposedDecade, yearDecade);
										if(proposedDecade < yearDecade) {
											[self flashRecheckExpiryDateMessage];
											goto eND;
										}
										if(newTextLen >= (yearRange.location + yearRange.length)) {
											year = [[newTextOrig substringWithRange:yearRange] integerValue];
											NSInteger diff = year - currentYear;
											if(diff < 0 || diff > 10) {	// blogs on internet suggest some CCs have dates 50 yeras in the future
												[self flashRecheckExpiryDateMessage];
												goto eND;
											}
											if(diff == 0) { // The entered year is the current year, so check that the month is in the future
												NSDateComponents *components = [[NSCalendar currentCalendar] components:NSCalendarUnitDay | NSCalendarUnitMonth | NSCalendarUnitYear fromDate:[NSDate date]];
												NSInteger currentMonth = [components month];
												if (month < currentMonth) {
													[self flashRecheckExpiryDateMessage];
													goto eND;
												}
											}
											if(creditCardImage != ccBackImage)
											{
												UIViewAnimationOptions transType = (type == AMEX) ? UIViewAnimationOptionTransitionCrossDissolve : UIViewAnimationOptionTransitionFlipFromBottom;
				
				
												[UIView transitionFromView:creditCardImage toView:ccBackImage duration:0.25f options:transType completion:NULL];
												creditCardImage = ccBackImage;
												self.statusHelpLabel.text = [ThemeBundleReplacement bundledOrReplacementStringNamed:@"enterCardSecurityCodeText"];
											}
										}
									}
				
									if(newTextLen == [placeView.text length]) {
										completelyDone = YES;
										NSRange ccvRange = [placeView.text rangeOfString:@"C"]; // first one
										ccvRange.length = type == AMEX ? 4 : 3;
										ccv = [[newTextOrig substringWithRange:ccvRange] integerValue];
									}
				
									updateText = YES;
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




			};
		}
			



	}
}

