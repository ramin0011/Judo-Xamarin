﻿using System;

namespace JudoDotNetXamariniOSSDK
{
	public class JudoConfiguration
	{
		public JudoConfiguration ()
		{
		}

		private static readonly Lazy<JudoConfiguration> _singleton = new Lazy<JudoConfiguration>(() => new JudoConfiguration());

		public static JudoConfiguration Instance
		{
			get { return _singleton.Value; }
		}

		public  string ApiToken { get; set;}
		public  string ApiSecret { get; set;}
		public  string JudoID { get; set;}

		public  string PaymentReference { get; set;}
		public  string ConsumerRef { get; set;}

	}
}

