using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Foundation;
using JudoShieldXamarin;
using PassKit;
using UIKit;
#if __UNIFIED__
// Mappings Unified CoreGraphic classes to MonoTouch classes

#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
// Mappings Unified types to MonoTouch types
using nfloat = global::System.Single;
using nint = global::System.Int32;
using nuint = global::System.UInt32;
#endif

namespace JudoDotNetXamariniOSSDK.Utils
{
    public static class ClientDetailsProvider
    {
        internal static ClientDetails GetClientDetails()
        {
            if (!JudoSDKManager.RiskSignals)
            {
                return null;
            }

            var clientDetails = new ClientDetails
            {
                OS = "iOS " + UIDevice.CurrentDevice.SystemVersion,
                DeviceId = GetDeviceMacAddress(),
                DeviceModel = UIDevice.CurrentDevice.Model,
				Serial = JudoShield.GetDeviceIdentifier(),
                CultureLocale = NSLocale.CurrentLocale.CountryCode,
                SslPinningEnabled = JudoSDKManager.SSLPinningEnabled
            };


            return clientDetails;
        }

        private static string GetDeviceMacAddress()
        {
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    var address = netInterface.GetPhysicalAddress();
                    return BitConverter.ToString(address.GetAddressBytes());

                }
            }

            return "NoMac";
        }

		public static bool ApplePayAvailable{get{
				NSString[] paymentNetworks = new NSString[] {
					new NSString(@"Amex"),
					new NSString(@"MasterCard"),
					new NSString(@"Visa")
				};

				if (PKPaymentAuthorizationViewController.CanMakePayments && PKPaymentAuthorizationViewController.CanMakePaymentsUsingNetworks (paymentNetworks)) {
					return true;
				} else {

					return false;
				}		
			}}

		public static string GetSDKVersion ()
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fvi.FileVersion;

			return "Xamarin-iOS-" + version;
		}

		

    }

	internal class ClientDetails
    {
        public string OS { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceId { get; set; }
        public string CultureLocale { get; set; }
        public string Serial { get; set; }
        public bool SslPinningEnabled { get; set; }
        // couldn't find the direct API support for following things there are some work around with native APIs
        public bool IsRoaming { get; set; }
        public string NetworkName { get; set; }
    }
}