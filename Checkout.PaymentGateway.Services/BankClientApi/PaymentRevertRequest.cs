namespace Checkout.PaymentGateway.Services.BankClientApi
{
    public class PaymentRevertRequest
    {
        public string PaymentRequestId { get; set; }
        public string PaymentReferenceId { get; set; }
    }
}