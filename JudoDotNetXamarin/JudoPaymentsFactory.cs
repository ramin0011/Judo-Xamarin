using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JudoDotNetXamarin.Logging;
using JudoPayDotNet;
using JudoPayDotNet.Authentication;
using JudoPayDotNet.Http;
using Environment = JudoPayDotNet.Enums.Environment;

namespace JudoDotNetXamarin
{
    public static class JudoPaymentsFactory
    {
        private static readonly string LIVE_URL = "http://devpartnerapi.judopay.com/";
        private static readonly string SANDBOX_URL = "";
        private static readonly string API_VERSION = "4.1.0.0";
        private const string Apiversionheader = "api-version";

        private static JudoPayApi Create(Credentials credentials, Environment environment)
        {
            string baseUrl = null;
            switch (environment)
            {
                case Environment.Live:
                    baseUrl = LIVE_URL;
                    break;
                case Environment.Sandbox:
                    baseUrl = SANDBOX_URL;
                    break;
            }

            var httpClient = new HttpClientWrapper(new AuthorizationHandler(credentials,
                                                    XamarinLoggerFactory.Create(typeof(AuthorizationHandler))),
                                                    new VersioningHandler(Apiversionheader, API_VERSION));
            var connection = new Connection(httpClient,
                                            XamarinLoggerFactory.Create,
                                            baseUrl);
            var client = new Client(connection);

            return new JudoPayApi(XamarinLoggerFactory.Create, client);
        }

        public static JudoPayApi Create(Environment environment, string token, string secret)
        {
            return Create(new Credentials(token, secret), environment);
        }

        public static JudoPayApi Create(Environment environment, string oauthAccessToken)
        {
            return Create(new Credentials(oauthAccessToken), environment);
        }
    }
}
