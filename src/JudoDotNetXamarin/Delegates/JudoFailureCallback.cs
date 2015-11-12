using JudoDotNetXamarin.Models;
using JudoPayDotNet.Models;

namespace JudoDotNetXamarin.Delegates
{
		public delegate void JudoFailureCallback(JudoError error, PaymentReceiptModel receipt = null);
}

