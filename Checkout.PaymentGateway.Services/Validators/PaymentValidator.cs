using System;
using Checkout.PaymentGateway.Common;
using FluentValidation.Results;

namespace Checkout.PaymentGateway.Services.Validators
{
    public class PaymentValidator : IPaymentValidator
    {
        public ValidationResult Validate(PaymentRequest paymentRequest)
        {
            if (paymentRequest == null) throw new ArgumentNullException(nameof(paymentRequest));

            var paymentRequestValidator = new PaymentRequestValidator();
            return paymentRequestValidator.Validate(paymentRequest);
        }
    }
}