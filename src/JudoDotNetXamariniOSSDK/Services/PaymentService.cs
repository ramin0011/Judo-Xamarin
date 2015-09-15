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
            try
            {
                CardPaymentModel payment = new CardPaymentModel
                {
				    JudoId = JudoConfiguration.Instance.JudoId,
				    YourPaymentReference = JudoConfiguration.Instance.PaymentReference,
				    YourConsumerReference = JudoConfiguration.Instance.ConsumerRef,
				    Amount = decimal.Parse(paymentViewModel.Amount),
				    CardNumber = paymentViewModel.Card.CardNumber,
				    CV2 = paymentViewModel.Card.CV2,
				    ExpiryDate = paymentViewModel.Card.ExpireDate,
				    CardAddress = new CardAddressModel() { PostCode = paymentViewModel.Card.PostCode, CountryCode = Convert.ToInt32(paymentViewModel.Card.CountryCode) },
				    StartDate = paymentViewModel.Card.StartDate,
                    IssueNumber = paymentViewModel.Card.IssueNumber,
                    YourPaymentMetaData = paymentViewModel.YourPaymentMetaData
                };

				Task<IResult<ITransactionResult>> task =  _judoAPI.Payments.Create(payment);

				return await task;
			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
				return null;
			}

		}
			
		public async Task<IResult<ITransactionResult>> PreAuthoriseCard (PaymentViewModel authorisation)
		{
            try
            {
                CardPaymentModel payment = new CardPaymentModel
                {
				    JudoId = JudoConfiguration.Instance.JudoId,
				    YourPaymentReference = JudoConfiguration.Instance.PaymentReference,
				    YourConsumerReference = JudoConfiguration.Instance.ConsumerRef,
				    Amount = decimal.Parse(authorisation.Amount),
				    CardNumber = authorisation.Card.CardNumber,
				    CV2 = authorisation.Card.CV2,
				    ExpiryDate = authorisation.Card.ExpireDate,
                    CardAddress = new CardAddressModel() { PostCode = authorisation.Card.PostCode, CountryCode = Convert.ToInt32(authorisation.Card.CountryCode) },
                    StartDate = authorisation.Card.StartDate,
                    IssueNumber = authorisation.Card.IssueNumber,
                    YourPaymentMetaData = authorisation.YourPaymentMetaData
                };

				Task<IResult<ITransactionResult>> task =  _judoAPI.PreAuths.Create(payment);
				return await task;
			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
				return null;
			}

		}

		public async Task<IResult<ITransactionResult>> MakeTokenPayment (TokenPaymentViewModel tokenPayment)
		{
            try
            {
                TokenPaymentModel payment = new TokenPaymentModel
                {
				    JudoId = JudoConfiguration.Instance.JudoId,
				    YourPaymentReference = JudoConfiguration.Instance.PaymentReference,
				    YourConsumerReference = JudoConfiguration.Instance.ConsumerRef,
				    Amount = decimal.Parse(tokenPayment.Amount),
				    CardToken = tokenPayment.Token,
				    CV2 = tokenPayment.CV2,
                    ConsumerToken = tokenPayment.ConsumerToken,
                    YourPaymentMetaData = tokenPayment.YourPaymentMetaData
			    };
				Task<IResult<ITransactionResult>> task =  _judoAPI.Payments.Create(payment);
				return await task;
			}
			catch(Exception e){
				Console.WriteLine(e.InnerException.ToString());
				return null;
			}

		}

		public async Task<IResult<ITransactionResult>> MakeTokenPreAuthorisation (TokenPaymentViewModel tokenPayment)
		{
			TokenPaymentModel payment = new TokenPaymentModel {
				JudoId = JudoConfiguration.Instance.JudoId,
				YourPaymentReference = JudoConfiguration.Instance.PaymentReference,
				YourConsumerReference = JudoConfiguration.Instance.ConsumerRef,
				Amount = decimal.Parse(tokenPayment.Amount),
				CardToken = tokenPayment.Token,
				CV2 = tokenPayment.CV2,
                ConsumerToken = tokenPayment.ConsumerToken,
                YourPaymentMetaData = tokenPayment.YourPaymentMetaData
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
