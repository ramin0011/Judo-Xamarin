
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;

namespace JudoDotNetXamariniOSSDK
{
	public partial class RegisterCardView : UIViewController
	{
		ITokenService _tokenService;
		bool KeyboardVisible = false;
		CreditCardType type;
		private List<CardCell> CellsToShow { get; set; }

		public RegisterCardView (ITokenService tokenService) : base ("RegisterCardView", null)
		{
			_tokenService = tokenService;
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


			NSNotificationCenter defaultCenter = NSNotificationCenter.DefaultCenter;
			defaultCenter.AddObserver (UIKeyboard.WillHideNotification, OnKeyboardNotification);
			defaultCenter.AddObserver (UIKeyboard.WillShowNotification, OnKeyboardNotification);


			UITapGestureRecognizer tapRecognizer = new UITapGestureRecognizer ();

			tapRecognizer.AddTarget (() => { 
				if (KeyboardVisible) {
					//DismissKeyboardAction ();
				}
			});

			tapRecognizer.NumberOfTapsRequired = 1;
			tapRecognizer.NumberOfTouchesRequired = 1;

			//EncapsulatingView.AddGestureRecognizer (tapRecognizer);
		}

		private void OnKeyboardNotification (NSNotification notification)
		{
			KeyboardVisible = notification.Name == UIKeyboard.WillShowNotification;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

		}

		void SetUpTableView ()
		{
			var detailCell = new CardEntryCell (new IntPtr ());

			CellsToShow = new List<CardCell> (){detailCell, new ReassuringTextCell (new IntPtr ())};

//			CGRect rectangle = ccText.Frame;
//			ccText.Frame = rectangle;
//
//			creditCardImage.Tag = (int)CreditCardType.InvalidCard;
//
//			creditCardImage.Layer.CornerRadius = 4.0f;
//			creditCardImage.Layer.MasksToBounds = true;
//
//			UIImage image = ThemeBundleReplacement.BundledOrReplacementImage ("ic_card_large_unknown", BundledOrReplacementOptions.BundledOrReplacement);
//
//			creditCardImage.Image = image;
//
//			currentYear = DateTime.Now.Year - 2000;
//
//			CALayer layer = containerView.Layer;
//			layer.CornerRadius = 4.0f;
//			layer.MasksToBounds = true;
//			layer.BorderColor = ColourHelper.GetColour ("0xC3C3C3FF").CGColor; 
//			layer.BorderWidth = 1;
//			layer = textScroller.Layer;
//			layer.CornerRadius = 4.0f;
//			layer.MasksToBounds = true;
//			layer.BorderWidth = 0;
//
//			textScroller.ScrollEnabled = false;
//
//			ccText.Text = "000011112222333344445555";
//
//			UITextPosition start = ccText.BeginningOfDocument;
//			UITextPosition end = ccText.GetPosition (start, 24);
//			UITextRange range = ccText.GetTextRange (start, end);
//			CGRect r = ccText.GetFirstRectForRange (range);
//			CGSize frameRect = r.Size;
//			frameRect.Width = (r.Size.Width / 24.0f);
//			ccText.Font = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;
//			r.Size = frameRect;
//			ccText.Text = "";
//
//			CGRect frame = placeView.Frame;
//			placeView.Font = ccText.Font;
//			placeView.Text = "0000 0000 0000 0000";
//
//			placeView.SetShowTextOffSet (0);
//			placeView.Offset = r;
//
//			placeView.BackgroundColor = UIColor.Clear;
//			textScroller.InsertSubview (placeView, 0);

			type = CreditCardType.InvalidCard;

			CardCellSource tableSource = new CardCellSource (CellsToShow);
			TableView.Source = tableSource;
			TableView.SeparatorColor = UIColor.Clear;


			//SetUpMaskedInput ();

		}
	}
}

