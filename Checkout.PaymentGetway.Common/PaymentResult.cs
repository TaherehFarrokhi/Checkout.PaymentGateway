namespace Checkout.PaymentGetway.Common
{
    public class PaymentResult
    {
        public PaymentResult(string paymentRequestId)
        {
            PaymentRequestId = paymentRequestId;
        }

        public PaymentResult(string paymentRequestId, string paymentReferenceId)
        {
            PaymentRequestId = paymentRequestId;
            PaymentReferenceId = paymentReferenceId;
        }

        public string PaymentReferenceId { get; }
        public string PaymentRequestId { get; }
    }
}