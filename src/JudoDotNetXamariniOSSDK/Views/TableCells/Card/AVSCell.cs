
using System;

using Foundation;
using UIKit;
using CoreFoundation;

namespace JudoDotNetXamariniOSSDK
{
	public partial class AVSCell : CardCell
	{
		public static readonly UINib Nib = UINib.FromName ("AVSCell", NSBundle.MainBundle);

		public UIActionSheet countrySheet;
		BillingCountryOptions selectedCountry = BillingCountryOptions.BillingCountryOptionUK;


		public UITextField PostcodeTextFieldOutlet { get { return PostcodeTextField; } }
		public AVSCell (IntPtr handle) : base (handle)
		{
			Key = "AVSCell";
		}

		public override CardCell Create ()
		{
			return (AVSCell)Nib.Instantiate (null, null) [0];
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			SetUpCell ();
		}


		void SetUpCell ()
		{
			countrySheet = new UIActionSheet ("Select Country");
			countrySheet.TintColor = UIColor.Black;
			selectedCountry = BillingCountryOptions.BillingCountryOptionUK;
			countrySheet.Clicked += delegate(object sender, UIButtonEventArgs button) {
				switch (button.ButtonIndex) {
				case (int) BillingCountryOptions.BillingCountryOptionUK:
					selectedCountry = BillingCountryOptions.BillingCountryOptionUK;

					break;
				case (int)BillingCountryOptions.BillingCountryOptionUSA:
					selectedCountry = BillingCountryOptions.BillingCountryOptionUSA;

					break;
				case (int)BillingCountryOptions.BillingCountryOptionCanada:
					selectedCountry = BillingCountryOptions.BillingCountryOptionCanada;

					break;
				case (int)BillingCountryOptions.BillingCountryOptionOther:
					selectedCountry = BillingCountryOptions.BillingCountryOptionOther;

					break;
				default:
					selectedCountry = BillingCountryOptions.BillingCountryOptionUK;
					break;

				}
				CountryLabel.Text = selectedCountry.ToDescriptionString ();
			};

			foreach (BillingCountryOptions option in Enum.GetValues(typeof(BillingCountryOptions))) {
				countrySheet.AddButton (option.ToDescriptionString ());
			}
			CountryButton.TouchUpInside += (sender, ev) => {
				countrySheet.ShowInView (UIApplication.SharedApplication.KeyWindow);
			};
			PostcodeTextField.Text = "";	
			PostcodeTextField.Font = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;;
			PostcodeTextField.TextColor = UIColor.Black;

			PostcodeTextField.ShouldChangeCharacters = (UITextField textField, NSRange nsRange, string replacementString) => {
				CSRange range = new CSRange ((int)nsRange.Location, (int)nsRange.Length);
				DispatchQueue.MainQueue.DispatchAsync (() => {
				//	UpdateUI ();
				});
				int textLengthAfter = textField.Text.Length + replacementString.Length - range.Length;
				if (textLengthAfter > 10) {
					return false;
				}
				return true;
			};

		}

		public void GatherCardDetails (CardViewModel cardViewModel)
		{
			

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
		public void CleanUp ()
		{
			SetUpCell ();
		}
	}
}

