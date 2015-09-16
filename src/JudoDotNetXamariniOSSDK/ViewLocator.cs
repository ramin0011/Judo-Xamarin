namespace JudoDotNetXamariniOSSDK
{
    internal class ViewLocator
    {
        readonly IPaymentService _paymentService;

        public ViewLocator(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public CreditCardView GetPaymentView()
        {
            CreditCardView ctrl = new CreditCardView(_paymentService);
            return ctrl;
        }

        //public PaymentReceipt GetReceiptView(PaymentReceiptViewModel receipt)
        //{
        //    PaymentReceipt receiptView = new PaymentReceipt(receipt);
        //    return receiptView;
        //}

        public PreAuthorisationView GetPreAuthView()
        {
            PreAuthorisationView ctrl = new PreAuthorisationView(_paymentService);
            return ctrl;
        }

        public TokenPaymentView GetTokenPaymentView()
        {
            TokenPaymentView ctrl = new TokenPaymentView(_paymentService);
            return ctrl;
        }

        public TokenPreAuthorisationView GetTokenPreAuthView()
        {
            TokenPreAuthorisationView ctrl = new TokenPreAuthorisationView(_paymentService);
            return ctrl;
        }
    }
}