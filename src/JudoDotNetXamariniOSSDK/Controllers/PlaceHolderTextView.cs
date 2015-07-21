#define LED_FONT

using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace JudoDotNetXamariniOSSDK
{
	
	public class PlaceHolderTextView : UIView
	{
		

		#if LED_FONT
		readonly UIColor TEXT_COLOR = UIColor.DarkGray;
		#else
		readonly UIColor TEXT_COLOR = UIColor.LightGray;
		#endif
		
		public string Text { get; set;}
		public UIFont Font { get; set;}
		public int ShowTextOffset { get; set; }
		public CGRect Offset { get; set;}

		public PlaceHolderTextView (CGRect frame) : base(frame)
		{
			
		}

		private void SetText(string newText)
		{
			if (newText == Text) 
			{
				return;
			}

			bool animate = (Text != null && newText.Length > Text.Length); 
			Text = newText;

			if (animate) 
			{
				Alpha = (nfloat)0.0;
				Animate (0.25, () => 
					{
						Alpha = 1;
					});
			}

			SetNeedsDisplay();
		}

		private void SetShowTextOffSet(int newOffset)
		{
			ShowTextOffset = newOffset;
			SetNeedsDisplay();
		}

		private void DrawRect(CGRect rect)
		{
			CGRect r = Offset;

			string clearText = Text.Substring (0, ShowTextOffset);
			string grayText = Text.Substring (ShowTextOffset, Text.Length - ShowTextOffset);

			CGContext context = UIGraphics.GetCurrentContext ();

			CGColor clearColor = ThemeBundleReplacement.BundledOrReplacementColor ("CLEAR_COLOR", BundledOrReplacementOptions.BundledOrReplacement).CGColor;
			context.SetStrokeColor(clearColor);
			context.SetFillColor(clearColor);
			context.FillRect(rect);

			if (clearText.Length > 0) 
			{
				r.X += clearText.DrawString (Offset.Location, Font).Width;
			}

			NSRange charsToDraw = new NSRange (0, grayText.Length);

			if (charsToDraw.Length > 0) 
			{
				CGColor grayColor = ThemeBundleReplacement.BundledOrReplacementColor ("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement).CGColor;

				context.SetStrokeColor (grayColor);
				context.SetFillColor (grayColor);

				var chars = charsToDraw.ToString ().ToCharArray();
			    int i;
				for (i = 0; i < charsToDraw.ToString ().Length; i++) 
				{
					char character = chars[i];
					if (character == ' ') {
						r.X += Offset.Size.Width;
						continue;
					}
					if (character == 'X') {
						#if LED_FONT
						CGRect box = rect.Inset(2, 1);
						#else
						CGRect box = rect.Inset (3, 3);
						#endif
						nfloat radius = 3;
						context.BeginPath ();
						context.MoveTo (box.GetMinX () + radius, box.GetMinY ());
						var mask = "0";
						UIFont drawFont = JudoSDKManager.FIXED_WIDTH_FONT_SIZE_20;
                        mask.DrawString(box, drawFont);
						r.X += Offset.Size.Width;
						continue;
					}
					break;
				}

				charsToDraw.Location += i;
				charsToDraw.Length -= i;
				if (charsToDraw.Length == 0) 
				{
					grayText.Substring((int)charsToDraw.Location, (int)charsToDraw.Length).DrawString (r.Location, Font);
				}
			}
		}

		public nfloat WidthToOffset()
		{
			var startText = Text.Substring(0, ShowTextOffset);
			return startText.StringSize(Font).Width;
		}

		public nfloat WidthFromOffset()
		{
			var endText = Text.Substring (ShowTextOffset, Text.Length - ShowTextOffset);

		    return 0;
		}
	}
}

