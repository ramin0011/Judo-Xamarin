using System.Threading.Tasks;
using JudoDotNetXamarin;
using JudoDotNetXamarin.ViewModels;
using JudoPayDotNet.Models;

namespace JudoDotNetXamariniOSSDK.Services
{
	internal interface IPaymentService
	{
		Task<IResult<ITransactionResult>> MakePayment (PaymentViewModel payment);
		Task<IResult<ITransactionResult>> PreAuthoriseCard (PaymentViewModel payment);
		Task<IResult<ITransactionResult>> MakeTokenPayment (TokenPaymentViewModel payment);
		Task<IResult<ITransactionResult>> MakeTokenPreAuthorisation (TokenPaymentViewModel payment);
	    Task<IResult<ITransactionResult>> RegisterCard(PaymentViewModel payment);
		Task<IResult<ITransactionResult>> CompleteDSecure (string receiptID,string PaRes, string MD);
	}
}

