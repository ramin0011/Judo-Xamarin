using System;
using JudoPayDotNet.Models;
using System.Threading.Tasks;

namespace JudoDotNetXamariniOSSDK
{
	public interface IApplePayService
	{
		Task<IResult<ITransactionResult>> MakeApplePayment (PaymentViewModel payment);
		Task<IResult<ITransactionResult>> ApplePreAuthoriseCard (PaymentViewModel payment);
	}
}

