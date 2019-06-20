using System;

namespace Checkout.PaymentGetway.Common
{
    public class PaymentRequest
    {
        public string PaymentRequestId { get; } = Guid.NewGuid().ToString();
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Cvv { get; set; }
    }
}
