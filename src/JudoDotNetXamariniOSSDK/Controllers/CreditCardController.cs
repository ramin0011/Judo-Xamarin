using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreAnimation;

namespace JudoDotNetXamariniOSSDK
{
	public delegate void CreditCardSaved(Card card);

	public class CreditCardController : UIViewController, IUIScrollViewDelegate
	{
		public CreditCardControllerType CreditCardControllerType {get; set;}
		public Card judoCard {get; set;}
		public event Action<bool, Card> CompletionBlock;

		private UITableView TableView { get; set; }
		private UITableViewCell CardDetailsCell {get; set;}
		private UITableViewCell ReassuringTextCell { get; set;}
		private UITableViewCell AVSCell { get; set; }
		private UITableViewCell MaestroCell { get; set;}
		private UITableViewCell PayCell {get; set;}
		private UITableViewCell SpacerCell {get; set;}

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
		private UIButton SubmitButton {get; set;}
		private UIButton CancelButton {get; set;}

		//private BSKeyboardControls KeyboardControls {get; set;}
		private UIButton NumberFieldClearButton {get; set;}
		private UIButton ExpiryInfoButton {get; set;}

		private UILabel StatusHelpLabel { get; set;}
		private UILabel PleaseRecheckNumberLabel {get; set;}
		private NSMutableArray CellsToShow {get; set;}

		private NSLayoutConstraint PickBottomConstraint {get; set;}

		private UIImageView creditCardImage;
		private UIView containerView;

		private UIScrollView textScroller;
		private UIView warningView;
		private PlaceHolderTextView placeView;
		private UITextView ccText;
		private UIButton updateCard;
		private UITextView dummyTextView;

		UIImageView ccImage;
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

        //public CreditCardController() : base("CreditCardView", new NSBundle(NSBundle.MainBundle.GetUrlForResource("JudoPay", "bundle")))
        //{
        //}

	    public CreditCardController()
	    {
	        
	    }

		private bool prefersStatusBarHidden()
		{
			return NavigationController == null;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		
			CellsToShow = (NSMutableArray)NSMutableArray.FromObjects (new []{ CardDetailsCell, ReassuringTextCell, SpacerCell, PayCell, null });

			UIImage patternImage = ThemeBundleReplacement.BundledOrReplacementImage ("bg_light_iPhone5", BundledOrReplacementOptions.BundledOrReplacement);
			View.BackgroundColor = UIColor.FromPatternImage (patternImage);

			switch (CreditCardControllerType) 
			{
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
			defaultCenter.AddObserver(UIKeyboard.WillShowNotification, keyboardMoving);
			defaultCenter.AddObserver(UIKeyboard.DidShowNotification, keyboardMoving);
			defaultCenter.AddObserver(UIKeyboard.WillHideNotification, keyboardMoving);

			SubmitButton.SetTitleColor (ThemeBundleReplacement.BundledOrReplacementColor("GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement), UIControlState.Application);

			UIEdgeInsets insets = new UIEdgeInsets (0, 20, 0, 20);
			UIImage activeImage = ThemeBundleReplacement.BundledOrReplacementImage ("btn_pay_normal_iPhone6", BundledOrReplacementOptions.BundledOrReplacement);
			UIImage inactiveImage = ThemeBundleReplacement.BundledOrReplacementImage ("btn_pay_inactive_iPhone6", BundledOrReplacementOptions.BundledOrReplacement);
			UIImage resizableActiveImage = activeImage.CreateResizableImage (insets);
			UIImage resizableInactiveImage = inactiveImage.CreateResizableImage (insets);

			SubmitButton.SetBackgroundImage (resizableActiveImage, UIControlState.Normal);
			SubmitButton.SetBackgroundImage (resizableInactiveImage, UIControlState.Disabled);

			CancelButton.SetTitleColor (ThemeBundleReplacement.BundledOrReplacementColor("GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement), UIControlState.Normal);

			CountryButton.Layer.CornerRadius = 4.0f;
			CountryButton.Layer.MasksToBounds = true;
			CountryLabel.Layer.BorderColor = ThemeBundleReplacement.BundledOrReplacementColor ("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement).CGColor;
			CountryButton.Layer.BorderWidth = 1;

			PostCodeBackgroundView.Layer.CornerRadius = 4.0f;
			PostCodeBackgroundView.Layer.MasksToBounds = true;
			PostCodeBackgroundView.Layer.BorderColor = ThemeBundleReplacement.BundledOrReplacementColor ("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement).CGColor;
			PostCodeBackgroundView.Layer.BorderWidth = 1;

			StartDateContainerView.Layer.CornerRadius = 4.0f;
			StartDateContainerView.Layer.MasksToBounds = true;
			StartDateContainerView.Layer.BorderColor = ThemeBundleReplacement.BundledOrReplacementColor ("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement).CGColor;
			StartDateContainerView.Layer.BorderWidth = 1;

            IssueNumberContainerView.Layer.CornerRadius = 4.0f;
            IssueNumberContainerView.Layer.MasksToBounds = true;
            IssueNumberContainerView.Layer.BorderColor = ThemeBundleReplacement.BundledOrReplacementColor("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement).CGColor;
            IssueNumberContainerView.Layer.BorderWidth = 1;

			StartDatePlaceholder.TextColor = ThemeBundleReplacement.BundledOrReplacementColor ("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement);
			StartDateTextField.TextColor = ThemeBundleReplacement.BundledOrReplacementColor ("GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement);

            IssueNumberTextView.TextColor = ThemeBundleReplacement.BundledOrReplacementColor("GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement);

			editCard();

			CGRect rectangle = ccText.Frame;
			//rectangle.Size.Height = 36;
			ccText.Frame = rectangle;

			creditCardImage.Tag = (int)CreditCardType.InvalidCard;

			creditCardImage.Layer.CornerRadius = 4.0f;
			creditCardImage.Layer.MasksToBounds = true;

			StatusHelpLabel.Text = ThemeBundleReplacement.BundledOrReplacementString ("enterCardDetailsText", BundledOrReplacementOptions.BundledOrReplacement);

			UIImage image = ThemeBundleReplacement.BundledOrReplacementImage ("ic_card_large_unknown", BundledOrReplacementOptions.BundledOrReplacement);

			creditCardImage.Image = image;

			currentYear = DateTime.Now.Year - 2000;

			CALayer layer = containerView.Layer;
			layer.CornerRadius = 4.0f;
			layer.MasksToBounds = true;
			layer.BorderColor = ThemeBundleReplacement.BundledOrReplacementColor ("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement).CGColor;
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
			//r.Size.Width /= 24.0f;
			ccText.Text = String.Empty;

			CGRect frame = placeView.Frame;

			placeView = new PlaceHolderTextView (frame);
			placeView.Font = ccText.Font;
			placeView.Text = "0000 0000 0000 0000";
			placeView.ShowTextOffset = 0;
			placeView.Offset = r;
			placeView.BackgroundColor = ThemeBundleReplacement.BundledOrReplacementColor ("CLEAR_COLOR", BundledOrReplacementOptions.BundledOrReplacement);

			textScroller.InsertSubview (placeView, 0);

			type = CreditCardType.InvalidCard;

			ccText.Font = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;
			dummyTextView.Font = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20; 
			PostCodeTextField.Font = ccText.Font;
			PostCodeTextField.TextColor = ccText.TextColor;
			CountryLabel.Font = ccText.Font;
			CountryLabel.TextColor = ccText.TextColor;
			StartDateTextField.Font = ccText.Font;
			StartDateTextField.TextColor = ccText.TextColor;
			IssueNumberTextView.Font = ccText.Font;
			IssueNumberTextView.TextColor = ccText.TextColor;

			dummyTextView.BecomeFirstResponder ();

			NSArray fields = NSArray.FromObjects (ccText, dummyTextView, PostCodeTextField, StartDateTextField, IssueNumberTextView);
			//BSKeyboardControls bd = 
		}

		private void keyboardMoving(NSNotification note){

		}

		private void editCard()
		{

		}
	}
}

