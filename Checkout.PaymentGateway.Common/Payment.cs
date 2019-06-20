using System;

namespace Checkout.PaymentGateway.Common
{
    public class Payment
    {
        public string PaymentRequestId { get; set; }
        public string PaymentReferenceId { get; set; }
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime RequestedAt { get; } = DateTime.Now;
        public DateTime LastModifiedAt { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    }
}