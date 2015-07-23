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

			ccText.Text = "000011112222333344445555";

			UITextPosition start = ccText.BeginningOfDocument;
			UITextPosition end = ccText.GetPosition (start, 24);
			UITextRange range = ccText.GetTextRange (start, end);
			CGRect r = ccText.GetFirstRectForRange (range);
			//r.Size.Width /= 24.0f;
			ccText.Text = String.Empty;

			CGRect frame = placeView.Frame;

			//placeView = new PlaceHolderTextView ();
			placeView.Font = ccText.Font;
			placeView.Text = "0000 0000 0000 0000";
			placeView.ShowTextOffset = 0;
			placeView.Offset = r;
			placeView.BackgroundColor = ColourHelper.GetColour ("0x00000000"); //  ThemeBundleReplacement.BundledOrReplacementColor ("CLEAR_COLOR", BundledOrReplacementOptions.BundledOrReplacement);

			textScroller.InsertSubview (placeView, 0);

			type = CreditCardType.InvalidCard;


			//dummyTextView.BecomeFirstResponder ();

			AddPaymentTableSource tableSource = new AddPaymentTableSource (CellsToShow);
			TableView.Source = tableSource;
			TableView.SeparatorColor = UIColor.Clear;

			NSArray fields = NSArray.FromObjects (ccText,dummyTextView , PostCodeTextField, StartDateTextField, IssueNumberTextView);
		}

		private void keyboardMoving(NSNotification note){

		}

		private void editCard()
		{

		}
	}

}

