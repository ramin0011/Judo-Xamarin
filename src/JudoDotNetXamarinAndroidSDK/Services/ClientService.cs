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
using JudoDotNetXamarin;
using JudoDotNetXamarinAndroidSDK.Utils;
using System;
using Newtonsoft.Json.Linq;

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
			var context = Android.App.Application.Context;
			var connectivityManager =context.GetSystemService(Context.ConnectivityService).JavaCast<ConnectivityManager>();
			var telephonyManager = context.GetSystemService(Context.TelephonyService).JavaCast<TelephonyManager>();

			var clientDetails = new JudoDotNetXamarin.ClientDetails();

			clientDetails.OS = "android " + Build.VERSION.SdkInt;
			clientDetails.DeviceId = new DeviceUuidFactory(context).GetDeviceUuid();
			clientDetails.DeviceModel = Build.Model;
			clientDetails.Serial = Build.Serial;

			clientDetails.CultureLocale = Java.Util.Locale.Default.ISO3Country;

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
				clientDetails.NetworkName = telephonyManager.NetworkOperatorName != System.String.Empty ? telephonyManager.NetworkOperatorName : telephonyManager.SimOperatorName;
			}

			RootCheck rootCheck = new RootCheck(context);
			clientDetails.Rooted = rootCheck.IsRooted();

			clientDetails.SslPinningEnabled = JudoSDKManagerA.Instance.SSLPinningEnabled;

			return new JObject( clientDetails);
		}

		public string GetSDKVersion ()
		{
			throw new NotImplementedException ();
		}
			
	}
}

