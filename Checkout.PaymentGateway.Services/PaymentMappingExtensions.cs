using System;
using Checkout.PaymentGateway.Common;

namespace Checkout.PaymentGateway.Services
{
    public static class PaymentMappingExtensions
    {
        public static Payment ToPayment(this PaymentRequest paymentRequest)
        {
            if (paymentRequest == null) throw new ArgumentNullException(nameof(paymentRequest));

            return new Payment
            {
                CardNumber = paymentRequest.CardNumber.MaskFromStart(4, '*'),
                Amount = paymentRequest.Amount,
                Currency = paymentRequest.Currency,
                PaymentRequestId = paymentRequest.PaymentRequestId,
            };
        }
    }
}