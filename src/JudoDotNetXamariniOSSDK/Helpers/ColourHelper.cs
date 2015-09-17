using System;using System;
using UIKit;
using Foundation;
using System.Threading;
namespace JudoDotNetXamariniOSSDK
{
	internal static class ColourHelper
	{
		public static UIColor GetColour (int hex)
		{
			const int mask = 0xFF;
			nfloat divisor = 255.0f;

			nfloat r = (hex >> 24) & mask;
			nfloat g = (hex >> 16) & mask;
			nfloat b = (hex >> 8) & mask;
			nfloat a = hex & mask;

			return UIColor.FromRGB(r/divisor, g / divisor, b / divisor);
		}

		public static UIColor GetColour(string color)
		{
			int hex;
			hex =Convert.ToInt32 (color, 16);
			if (hex!=null) {
				return GetColour(hex);
			}

			return null;
		}
	}
}

