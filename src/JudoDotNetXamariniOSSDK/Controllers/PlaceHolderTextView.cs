#define LED_FONT

using System;
using UIKit;
using CoreGraphics;
using Foundation;

namespace JudoDotNetXamariniOSSDK
{
	[Register("PlaceHolderTextView")]
	public partial class PlaceHolderTextView : UIView
	{
		

		#if LED_FONT
		readonly UIColor TEXT_COLOR = UIColor.DarkGray;
		#else
		readonly UIColor TEXT_COLOR = UIColor.LightGray;
		#endif
		
		public  string Text { get; set;}
		public  UIFont Font { get; set;}
		public int ShowTextOffset { get; set; }
		public CGRect Offset { get; set;}

//		public PlaceHolderTextView (CGRect frame) : base(frame)
//		{
//			
//		}

		public PlaceHolderTextView(IntPtr p) : base(p)
		{
		}
			
	

		public void SetText(string newText)
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

		public void SetShowTextOffSet(int newOffset)
		{
			ShowTextOffset = newOffset;
			SetNeedsDisplay();
		}


		public override void Draw (CGRect rect)
		{
			CGRect r = Offset;

			string clearText = Text.Substring (0, ShowTextOffset);
			string grayText = Text.Substring (ShowTextOffset, Text.Length - ShowTextOffset);

			CGContext context = UIGraphics.GetCurrentContext ();

			CGColor clearColor = UIColor.Clear.CGColor;//ThemeBundleReplacement.BundledOrReplacementColor ("CLEAR_COLOR", BundledOrReplacementOptions.BundledOrReplacement).CGColor;
			context.SetStrokeColor(clearColor);
			context.SetFillColor(clearColor);
			context.FillRect(rect);

			if (clearText.Length > 0) 
			{
				r.X += clearText.DrawString (Offset.Location, Font).Width;
			}

			CSRange charsToDraw = new CSRange(0, grayText.Length);// grayText.ToCharArray ();

			if (charsToDraw.Length > 0) 
			{
				CGColor grayColor = UIColor.LightGray.CGColor;// ThemeBundleReplacement.BundledOrReplacementColor ("LIGHT_GRAY_COLOR", BundledOrReplacementOptions.BundledOrReplacement).CGColor;

				context.SetStrokeColor (grayColor);
				context.SetFillColor (grayColor);

				var chars = grayText.ToCharArray();
				int i;
				for (i = 0; i < chars.Length; i++) 
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
				if (charsToDraw.Length != 0) 
				{
					grayText.Substring((int)charsToDraw.Location, (int)charsToDraw.Length).DrawString (r.Location, Font);
					grayText.DrawString(new CGPoint(r.X,r.Y),Font);
				}
			}
		}
			
		public float WidthToOffset()
		{
			var startText = Text.Substring(0, ShowTextOffset);
			return (float)startText.StringSize(Font).Width;
		}

		public nfloat WidthFromOffset()
		{
			var endText = Text.Substring (ShowTextOffset, Text.Length - ShowTextOffset);

		    return 0;
		}
	}
}

