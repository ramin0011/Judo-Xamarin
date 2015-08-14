
using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CoreFoundation;

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

			RegisterButton.SetTitleColor (UIColor.Black, UIControlState.Application);

			RegisterButton.TouchUpInside += (sender, ev) => {
				RegisterCard ();
			};

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
			RegisterButton.Enabled = enable;
			RegisterButton.Hidden = !enable;
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

			type = CreditCardType.InvalidCard;

			CardCellSource tableSource = new CardCellSource (CellsToShow);
			TableView.Source = tableSource;
			TableView.SeparatorColor = UIColor.Clear;

		}

		public void RegisterCard ()
		{
			CardViewModel cardViewModel = GatherCardDetails ();
			CardRegistrationViewModel card = new CardRegistrationViewModel () {
				
			};
			RegisterButton.Hidden = true;

			_tokenService.RegisterCard (card).ContinueWith (reponse => {
				var result = reponse.Result;
				if (!result.HasError) {
					PaymentReceiptModel paymentreceipt = result.Response as PaymentReceiptModel;
					PaymentReceiptViewModel receipt = new PaymentReceiptViewModel () {
						CreatedAt = paymentreceipt.CreatedAt.DateTime,
						Currency = paymentreceipt.Currency,
						OriginalAmount = paymentreceipt.Amount,
						ReceiptId = paymentreceipt.ReceiptId,
					};

					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
						RegisterButton.Hidden = false;
						CleanOutCardDetails();
						var view = JudoSDKManager.GetReceiptView (receipt);
						this.NavigationController.PushViewController (view, true);	
					});
				} else {
					DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {						
						var errorText = result.Error.ErrorMessage;
						UIAlertView _error = new UIAlertView ("Payment failed", errorText, null, "ok", null);
						_error.Show ();
						RegisterButton.Hidden = false;
					});
				}
			});

		}

		void CleanOutCardDetails ()
		{
			SetUpTableView ();

			if (JudoSDKManager.MaestroAccepted) {
				SetUpStartDateMask ();

			}	
			if (JudoSDKManager.AVSEnabled) {
				SetAVSComponents ();
			}

			DispatchQueue.MainQueue.DispatchAfter (DispatchTime.Now, () => {
				UIImage defaultImage = cardHelper.CreditCardImage (CreditCardType.InvalidCard);
				creditCardImage.Image = defaultImage;

			});
		}

		CardViewModel GatherCardDetails ()
		{
			string _cardMonth = cardMonth.ToString ();
			if (cardMonth < 10) {
				_cardMonth = "0" + _cardMonth;
			}
			CardViewModel cardViewModel = new CardViewModel () {
				CardName = "Ed Xample",
				CardNumber = creditCardNum,
				CV2 = ccv,
				ExpireDate = _cardMonth + "/" + year,
				CardType = type					
			};

			if (JudoSDKManager.AVSEnabled) {
				cardViewModel.PostCode = PostcodeTextField.Text;

				switch (selectedCountry) {
				case BillingCountryOptions.BillingCountryOptionUK:
					cardViewModel.CountryCode = @"826";
					break;
				case BillingCountryOptions.BillingCountryOptionUSA:
					cardViewModel.CountryCode = @"840";
					break;
				case BillingCountryOptions.BillingCountryOptionCanada:
					cardViewModel.CountryCode = @"124";
					break;
				default:					
					break;
				}

			}

			if (type == CreditCardType.Maestro) {
				cardViewModel.StartDate = StartDateTextField.Text.Replace (@"/", @"");
				cardViewModel.IssueNumber = IssueNumberTextField.Text;
			}

			return cardViewModel;
		}
	}
}

