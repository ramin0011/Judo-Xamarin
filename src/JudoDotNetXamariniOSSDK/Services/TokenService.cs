using System;
using System.Threading.Tasks;
using JudoPayDotNet.Models;
using JudoPayDotNet;

namespace JudoDotNetXamariniOSSDK
{
	public class TokenService :ITokenService
	{
		private	JudoPayApi _judoAPI;

		public TokenService (JudoPayApi judoAPI)
		{
			_judoAPI = judoAPI;
		}



		public Task<JudoPayDotNet.Models.IResult<ITransactionResult>> GetToken (CardRegistrationViewModel registration)
		{
			throw new NotImplementedException ();
		}


	}
}

