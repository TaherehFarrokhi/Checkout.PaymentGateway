namespace Checkout.PaymentGateway.Services.BankClientApi
{
    public class BankPaymentResult
    {
        public bool Status { get; set; }
        public string PaymentReferenceId { get; set; }
    }

    public class BankRevertPaymentResult
    {
        public bool Status { get; set; }
        public string RevertReferenceId { get; set; }
    }
}