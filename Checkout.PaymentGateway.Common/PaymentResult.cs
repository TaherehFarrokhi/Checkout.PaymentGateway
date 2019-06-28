using Newtonsoft.Json;

namespace Checkout.PaymentGateway.Common
{
    public class PaymentResult
    {
        public PaymentResult(string paymentRequestId)
        {
            PaymentRequestId = paymentRequestId;
        }

        [JsonConstructor]
        public PaymentResult(string paymentRequestId, string paymentReferenceId)
        {
            PaymentRequestId = paymentRequestId;
            PaymentReferenceId = paymentReferenceId;
        }

        public string PaymentReferenceId { get; }
        public string PaymentRequestId { get; }
    }
}