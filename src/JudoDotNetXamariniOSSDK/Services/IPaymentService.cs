using System;
using System.Threading.Tasks;
using JudoPayDotNet.Models;

namespace JudoDotNetXamariniOSSDK
{
	public interface IPaymentService
	{
		Task<IResult<ITransactionResult>> MakePayment (PaymentViewModel payment);
		Task<IResult<ITransactionResult>> PreAuthoriseCard (PreAuthorisationViewModel authorisation);
		Task<IResult<ITransactionResult>> MakeTokenPayment (TokenPaymentViewModel payment);
	}
}

