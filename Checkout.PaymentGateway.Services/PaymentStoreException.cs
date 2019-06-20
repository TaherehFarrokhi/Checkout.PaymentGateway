using System;

namespace Checkout.PaymentGateway.Services
{
    public class PaymentStoreException : Exception
    {
        public PaymentStoreException(string message, Exception exception) : base(message, exception)
        {
            
        }
    }
}