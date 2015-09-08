using System;
using System.Threading.Tasks;
using JudoPayDotNet.Models;

namespace JudoDotNetXamariniOSSDK
{
	public interface ITokenService
	{

		Task<IResult<ITransactionResult>> GetToken (CardRegistrationViewModel registration);
	}
}

