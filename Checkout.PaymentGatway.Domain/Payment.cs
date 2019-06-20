using System;

namespace Checkout.PaymentGatway.Domain
{
    public class Payment
    {
        public string PaymentId { get; set; }
        public string CardNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}