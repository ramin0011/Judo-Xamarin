using System;
using UIKit;
using CoreGraphics;

namespace JudoDotNetXamariniOSSDK
{
	public class PlaceHolderTextView : UIView
	{
		public string Text { get; set;}
		public UIFont Font { get; set;}
		public int ShowTextOffset { get; set; }
		public CGRect Offset { get; set;}

		public float WidthToOffset { get; set; }
		public float WidthFromOffset { get; set; }

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

			CGColor clearColor = 
		}
	}
}

