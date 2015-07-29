using System;

namespace JudoDotNetXamariniOSSDK
{
	public static class AppConfig
	{
		private const string ApiToken   = "5tZfrXDngpvu8iGS";
		private const string ApiSecret  = "da36e4c8f5805173060c934b12dcc14bb05761af310ea364cd787710b1da346b"; // move onto a local resource textfile thats accessed seperatly
		private const string MY_JUDO_ID       = "100515-592";
		private const string currency         = "GBP";
		private const decimal amount           = 4.99m;
		private const string paymentReference = "payment101010102";// dummy doesn't matter
		private const string consumerRef      = "consumer1010102";

		private const int ACTION_CARD_PAYMENT   = 101;
		private const int ACTION_TOKEN_PAYMENT  = 102;
		private const int ACTION_PREAUTH        = 201;
		private const int ACTION_TOKEN_PREAUTH  = 202;
		private const int ACTION_REGISTER_CARD  = 301;
	}
}

