using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Util;
using Java.Lang;
using Java.Util;
using JudoDotNetXamarinSDK;
using String = System.String;
using Android.App;
using JudoDotNetXamarinAndroidSDK.Utils;
using Newtonsoft.Json.Linq;
using JudoDotNetXamarin;

namespace JudoDotNetXamarinAndroidSDK
{
	public class ClientService : IClientService
	{

		public JObject GetClientDetails ()
		{
			if (!JudoSDKManagerA.Instance.RiskSignals)
			{
				return null;
			}

			var connectivityManager =Application.App.Context.GetSystemService(Context.ConnectivityService).JavaCast<ConnectivityManager>();
			var telephonyManager = context.GetSystemService(Context.TelephonyService).JavaCast<TelephonyManager>();

			var clientDetails = new ClientDetails();

			clientDetails.OS = "android " + Build.VERSION.SdkInt;
			clientDetails.DeviceId = new DeviceUuidFactory(context).GetDeviceUuid();
			clientDetails.DeviceModel = Build.Model;
			clientDetails.Serial = Build.Serial;

			clientDetails.CultureLocale = Locale.Default.ISO3Country;

			try
			{
				clientDetails.IsRoaming = connectivityManager != null && connectivityManager.ActiveNetworkInfo.IsRoaming;
			}
			catch (SecurityException e)
			{
				Log.Warn("Not enough permissions to read ActiveNetworkInfo", e);
			}



			if (telephonyManager != null)
			{
				clientDetails.NetworkName = telephonyManager.NetworkOperatorName != String.Empty ? telephonyManager.NetworkOperatorName : telephonyManager.SimOperatorName;
			}

			RootCheck rootCheck = new RootCheck(context);
			clientDetails.Root = rootCheck.BuildRootCheckDetails();

			clientDetails.SSLPinningEnabled = JudoSDKManagerA.Instance.SSLPinningEnabled;

			return clientDetails;
		}

		public string GetSDKVersion ()
		{
			throw new NotImplementedException ();
		}
			
	}
}

