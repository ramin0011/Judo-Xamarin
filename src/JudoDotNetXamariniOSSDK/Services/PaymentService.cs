using System;

using JudoPayDotNet;
using JudoPayDotNet.Models;
using System.Threading.Tasks;

namespace JudoDotNetXamariniOSSDK
{
	public class PaymentService : IPaymentService
	{
		private	JudoPayApi _judoAPI;

		public PaymentService (JudoPayApi judoAPI)
		{
			_judoAPI = judoAPI;
		}

		public async Task<IResult<ITransactionResult>> MakePayment (PaymentViewModel paymentViewModel)
		{
			CardPaymentModel payment = new CardPaymentModel {
				JudoId = AppConfig.JudoID,
				YourPaymentReference = AppConfig.PaymentReference,
				YourConsumerReference = AppConfig.ConsumerRef,
				Amount = decimal.Parse(paymentViewModel.Amount),
				CardNumber = paymentViewModel.Card.CardNumber,
				CV2 = paymentViewModel.Card.CV2.ToString(),
				ExpiryDate = paymentViewModel.Card.ExpireDate,
				CardAddress = new CardAddressModel(){PostCode=paymentViewModel.Card.PostCode},
				StartDate = paymentViewModel.Card.StartDate,		
			};
			try
			{
				Task<IResult<ITransactionResult>> task =  _judoAPI.Payments.Create(payment);
				return await task;
			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
				return null;
			}

		}
	}
}
