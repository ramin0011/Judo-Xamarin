using System;

namespace JudoDotNetXamariniOSSDK
{
	public static class AppConfig
	{
		public const string ApiToken = "5tZfrXDngpvu8iGS";
		public const string ApiSecret = "da36e4c8f5805173060c934b12dcc14bb05761af310ea364cd787710b1da346b";// move onto a local resource textfile thats accessed seperatly
		public const string JudoID = "100515592";//100515-592
		public const string Currency = "GBP";

		public const string PaymentReference = "payment101010102";// dummy doesn't matter
		public const string ConsumerRef = "consumer1010102";// dummy for now, should generate

		public const int ACTION_CARD_PAYMENT = 101;
		public const int ACTION_TOKEN_PAYMENT = 102;
		public const int ACTION_PREAUTH = 201;
		public const int ACTION_TOKEN_PREAUTH = 202;
		public const int ACTION_REGISTER_CARD = 301;
	}
}

