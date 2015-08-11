using System;

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

//		public  string ApiToken = "5tZfrXDngpvu8iGS";
//		public  string ApiSecret = "da36e4c8f5805173060c934b12dcc14bb05761af310ea364cd787710b1da346b";
//		public  string JudoID = "100515592";//100515-592
//
//		public  string PaymentReference = "payment101010102";
//		public  string ConsumerRef = "consumer1010102";
	}
}

