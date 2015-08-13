
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace JudoDotNetXamariniOSSDK
{
	public partial class RegisterCardView : UIViewController
	{



		ITokenService _tokenService;
		bool KeyboardVisible = false;
		CreditCardType type;
		private List<CardCell> CellsToShow { get; set; }

		CardEntryCell detailCell;
		ReassuringTextCell reassuringCell{ get; set;}
		MaestroCell maestroCell { get; set;}
		AVSCell avsCell{ get; set;}


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

		}

		void DismissKeyboardAction ()
		{
			//detailCell.PlaceViewOutlet.ResignFirstResponder ();
			detailCell.ccTextOutlet.ResignFirstResponder ();
			avsCell.PostcodeTextFieldOutlet.ResignFirstResponder ();
			maestroCell.StartDateTextFieldOutlet.ResignFirstResponder ();
			maestroCell.IssueNumberTextFieldOutlet.ResignFirstResponder ();
		}

		private void UpdateUI ()
		{
			bool enable = false;
			enable = detailCell.CompletelyDone;

			List<CardCell> cellsToRemove = new List<CardCell> ();
			List<CardCell> insertedCells = new List<CardCell> ();
			List<CardCell> cellsBeforeUpdate = cellsToRemove.ToList();
			TableView.BeginUpdates ();

			if (enable) {
				bool ccIsFirstResponder = detailCell.ccTextOutlet.IsFirstResponder;

				if (type == CreditCardType.Maestro && JudoSDKManager.MaestroAccepted) {
					if (!CellsToShow.Contains (maestroCell)) {
						int row = CellsToShow.IndexOf (detailCell) + 1;
						CellsToShow.Insert (row, maestroCell);

						insertedCells.Add (maestroCell);
					}

					if (maestroCell.IssueNumberTextFieldOutlet.Text.Length == 0 || !(maestroCell.StartDateTextFieldOutlet.Text.Length == 5)) {
						enable = false;
					}

					if (ccIsFirstResponder) {
						maestroCell.StartDateTextFieldOutlet.BecomeFirstResponder ();
						ccIsFirstResponder = false;
					}
				}

				if (JudoSDKManager.AVSEnabled) {
					if (!CellsToShow.Contains (avsCell)) {
						int row = CellsToShow.IndexOf (reassuringCell);
						CellsToShow.Insert (row, avsCell);

						insertedCells.Add (avsCell);
					}

					if (ccIsFirstResponder) {
						avsCell.PostcodeTextFieldOutlet.BecomeFirstResponder ();
						ccIsFirstResponder = false;
					}
				}

				if (ccIsFirstResponder) {
					DismissKeyboardAction ();

					ccIsFirstResponder = false;
				}
			} else {
				if (JudoSDKManager.MaestroAccepted) {
					if (CellsToShow.Contains (maestroCell)) {
						cellsToRemove.Add (maestroCell);
					}
				}

				if (JudoSDKManager.AVSEnabled) {
					if (CellsToShow.Contains (avsCell)) {
						cellsToRemove.Add (avsCell);
					}
				}
			}
			List<NSIndexPath> indexPathsToRemove = new List<NSIndexPath> ();

			foreach (CardCell cell in cellsToRemove) {
				indexPathsToRemove.Add (NSIndexPath.FromRowSection (cellsBeforeUpdate.IndexOf (cell), 0));
			}

			TableView.DeleteRows (indexPathsToRemove.ToArray (), UITableViewRowAnimation.Fade);

			foreach (CardCell cell in cellsToRemove) {
				CellsToShow.Remove (cell);
			}


			List<NSIndexPath> indexPathsToAdd = new List<NSIndexPath> ();

			foreach (CardCell cell in insertedCells) {
				indexPathsToAdd.Add (NSIndexPath.FromRowSection (CellsToShow.IndexOf (cell), 0));
			}

			TableView.InsertRows (indexPathsToAdd.ToArray (), UITableViewRowAnimation.Fade);
			TableView.EndUpdates ();
			//SubmitButton.Enabled = enable;
			//SubmitButton.Hidden = !enable;
		}

		void SetUpTableView ()
		{
			detailCell = new CardEntryCell (new IntPtr ());
		    reassuringCell =new ReassuringTextCell (new IntPtr ());
			avsCell =new AVSCell (new IntPtr ());
			maestroCell =new MaestroCell (new IntPtr ());


			detailCell = (CardEntryCell)detailCell.Create ();
			reassuringCell = (ReassuringTextCell)reassuringCell.Create ();
			avsCell = (AVSCell)avsCell.Create ();
			maestroCell = (MaestroCell)maestroCell.Create ();


			detailCell.UpdateUI = () => {
				UpdateUI();
			};

			avsCell.UpdateUI = () => {
				UpdateUI();
			};

			maestroCell.UpdateUI = () => {
				UpdateUI();
			};

			CellsToShow = new List<CardCell> (){detailCell,reassuringCell };

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

