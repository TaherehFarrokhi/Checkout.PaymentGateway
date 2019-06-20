using Checkout.PaymentGetway.Common;
using FluentValidation.Results;

namespace Checkout.PaymentGateway.Services.Validators
{
    public interface IPaymentValidator
    {
        ValidationResult Validate(PaymentRequest paymentRequest);
    }
}