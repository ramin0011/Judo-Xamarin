using System;
using UIKit;
using Foundation;
using System.Threading;

namespace JudoDotNetXamariniOSSDK
{
	public enum BundledOrReplacementOptions { Bundled, BundledOrReplacement };

	public static class ThemeBundleReplacement
	{

		private static NSBundle frameworkBundle;

		private static NSBundle getFrameworkBundle() 
		{
			NSBundle bundle;
			//Check if the bundle was already loaded
			if ((bundle = Volatile.Read(ref frameworkBundle))!= null) 
			{
				var path = NSBundle.MainBundle.ResourcePath;
				var frameworkBundlePath = path + "JudoPay.bundle";

				//Try to set the bundle value, if the bundle was already loaded, discard this load and use the existing, else safely read the existing
				if (Interlocked.CompareExchange (ref frameworkBundle, NSBundle.FromPath (frameworkBundlePath), null) == null) 
				{
					bundle = frameworkBundle;
				} 
				else 
				{
					bundle = Volatile.Read (ref frameworkBundle);
				}
					
			}

			return bundle;
		}

		private static UIColor getColor(int hex)
		{
			const int mask = 0xFF;
			nfloat divisor = 255.0f;

			int r = (hex >> 24) & mask;
			int g = (hex >> 16) & mask;
			int b = (hex >> 8) & mask;
			int a = hex & mask;

			return UIColor.FromRGB (r / divisor, g / divisor, b / divisor, a / divisor);
		}

		private static UIColor getColor(string color)
		{
			int hex;
			if (int.TryParse (color, out hex)) {
				return getColor(hex);
			}

			return null;
		}

		public static UIImage BundledOrReplacementImage(string imageName, BundledOrReplacementOptions bundledOrReplacement)
		{
			const string pngPrefix = ".png";
			string name = imageName;

			switch (bundledOrReplacement) 
			{
				case BundledOrReplacementOptions.BundledOrReplacement:
				{
					if (name.EndsWith (pngPrefix)) 
					{
						name = imageName.Substring(0, imageName.Length - pngPrefix.Length);
					}
						
					var image = UIImage.FromFile (name);

					if (image != null) 
					{
						return image;
					}
				}
				case BundledOrReplacementOptions.Bundled:
				{
					return UIImage.FromBundle (getFrameworkBundle ().PathForResource (imageName, "png"));
				}

				break;
			}
		}

		public static string BundledOrReplacementString(string stringName, BundledOrReplacementOptions bundledOrReplacement)
		{
			switch(bundledOrReplacement)
			{
				case BundledOrReplacementOptions.BundledOrReplacement:
				{
					var replacedString = NSBundle.MainBundle.LocalizedString (stringName, null, "JudoName");

					if (replacedString != stringName) 
					{
						return replacedString;
					}
				}
				case BundledOrReplacementOptions.Bundled:
				{
					return getFrameworkBundle().LocalizedString (stringName, stringName, "JudoTheme", null);
				}
			}
		}

		public static UIColor BundledOrReplacementColor(string colourName, BundledOrReplacementOptions bundledOrReplacement)
		{
			switch(bundledOrReplacement)
			{
				case BundledOrReplacementOptions.BundledOrReplacement:
				{
					return getColor(BundledOrReplacementString(colourName, BundledOrReplacementOptions.BundledOrReplacement));
				}
				case BundledOrReplacementOptions.Bundled:
				{
					return getColor(BundledOrReplacementString(colourName, BundledOrReplacementOptions.Bundled));
				}
			}
		}



	}
}

