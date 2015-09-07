
using System;

using Foundation;
using UIKit;
using System.Text;
using CoreFoundation;
using CoreAnimation;

namespace JudoDotNetXamariniOSSDK
{
	public partial class TokenPaymentCell : CardCell
	{

		CreditCard cardHelper = new CreditCard ();
		public static readonly UINib Nib = UINib.FromName ("TokenPaymentCell", NSBundle.MainBundle);

		public string CCV{ get; set; }

		public bool Complete { get; set; }

		int LengthForType;

		public UITextField CCVEntryOutlet { get { return entryField; } }

		public TokenPaymentCell (IntPtr handle) : base (handle)
		{
			Key = "TokenPaymentCell";
		}

		public override CardCell Create ()
		{
			return (TokenPaymentCell)Nib.Instantiate (null, null) [0];
		}


		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
		}

		public override void SetUpCell ()
		{
			UIImage frontImage = cardHelper.CreditCardImage (JudoConfiguration.Instance.TokenCardType);

			CALayer layer = EntryEnclosingView.Layer;
			layer.CornerRadius = 4.0f;
			layer.MasksToBounds = true;
			layer.BorderColor = ColourHelper.GetColour ("0xC3C3C3FF").CGColor; 
			layer.BorderWidth = 1;

			cardImage.Image = frontImage;

			PreviousCardNumber.Text = "xxxx " + JudoConfiguration.Instance.LastFour;
			LengthForType = JudoConfiguration.Instance.TokenCardType == CreditCardType.AMEX ? 4 : 3;

			entryField.ShouldChangeCharacters = (UITextField textView, NSRange NSRange, string replace) => {
				
				CSRange range = new CSRange ((int)NSRange.Location, (int)NSRange.Length);
				Complete = false;

				if (range.Length > 1) {
					return false;
				}
				if (replace.Length > 1) {
					
					return false;
				}
				if (replace.Length == 1 && !char.IsDigit (replace.ToCharArray () [0])) {
					
					return false;
				}
				if (textView.Text.Length + replace.Length - range.Length > LengthForType) {
					
					return false;
				}
				if(replace!=""&&textView.Text.Length + replace.Length==LengthForType)
				{
					var aStringBuilder = new StringBuilder (textView.Text);
					aStringBuilder.Remove (range.Location, range.Length);
					aStringBuilder.Insert (range.Location, replace);
					string newTextOrig = aStringBuilder.ToString ();
					CCV = newTextOrig;
				Complete = true;
				}
				DispatchQueue.MainQueue.DispatchAsync (() => {
					UpdateUI ();
				});
				return true;

			};
			entryField.BecomeFirstResponder ();
		}

		public void CleanUp ()
		{
			CCV="";
			Complete = false;
			entryField.Text="";
		}
			
	}
}

