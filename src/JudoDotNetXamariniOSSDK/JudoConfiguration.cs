using System;

namespace JudoDotNetXamariniOSSDK
{
	public class JudoConfiguration
	{
		public JudoConfiguration ()
		{
		}

		private static readonly Lazy<JudoConfiguration> _singleton = new Lazy<JudoConfiguration> (() => new JudoConfiguration ());

		public static JudoConfiguration Instance {
			get { return _singleton.Value; }
		}

		public  string ApiToken { get; set; }

		public  string ApiSecret { get; set; }

		public  string JudoId { get; set; }

		public  string PaymentReference { get; set; }

		public  string ConsumerRef { get; set; }

		public string ConsumerToken {get;set;}

		public string CardToken { get; set; }

		public CreditCardType TokenCardType { get; set; }

		public string LastFour {get;set;}
	}
}

