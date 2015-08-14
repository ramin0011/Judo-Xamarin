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
				JudoId = JudoConfiguration.Instance.JudoID,
				YourPaymentReference = JudoConfiguration.Instance.PaymentReference,
				YourConsumerReference = JudoConfiguration.Instance.ConsumerRef,
				Amount = decimal.Parse(paymentViewModel.Amount),
				CardNumber = paymentViewModel.Card.CardNumber,
				CV2 = paymentViewModel.Card.CV2,
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


		public async Task<IResult<ITransactionResult>> PreAuthoriseCard (PreAuthorisationViewModel authorisation)
		{
			CardPaymentModel payment = new CardPaymentModel {
				JudoId = JudoConfiguration.Instance.JudoID,
				YourPaymentReference = JudoConfiguration.Instance.PaymentReference,
				YourConsumerReference = JudoConfiguration.Instance.ConsumerRef,
				Amount = decimal.Parse(authorisation.Amount),
				CardNumber = authorisation.Card.CardNumber,
				CV2 = authorisation.Card.CV2,
				ExpiryDate = authorisation.Card.ExpireDate,
				CardAddress = new CardAddressModel(){PostCode=authorisation.Card.PostCode},
				StartDate = authorisation.Card.StartDate,		
			};
			try
			{
				Task<IResult<ITransactionResult>> task =  _judoAPI.PreAuths.Create(payment);
				return await task;
			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
				return null;
			}

		}
	}
}
