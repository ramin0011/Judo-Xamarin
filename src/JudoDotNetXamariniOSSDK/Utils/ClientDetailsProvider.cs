using System;
using System.Net.NetworkInformation;
using Foundation;
using UIKit;

namespace JudoDotNetXamariniOSSDK.Utils
{
    public static class ClientDetailsProvider
    {
        public static ClientDetails GetClientDetails()
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
                Serial = UIDevice.CurrentDevice.IdentifierForVendor.AsString(),
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
    }

    public class ClientDetails
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