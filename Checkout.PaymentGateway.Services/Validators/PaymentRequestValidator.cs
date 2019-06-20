using System;
using Checkout.PaymentGateway.Common;
using FluentValidation;

namespace Checkout.PaymentGateway.Services.Validators
{
    internal class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(m => m.CardNumber).NotEmpty().Length(16).WithMessage($"{nameof(PaymentRequest.CardNumber)} can not be empty and must have 16 digits.");
            RuleFor(m => m.ExpiryDate).NotEmpty().GreaterThan(DateTime.Now).WithMessage($"{nameof(PaymentRequest.ExpiryDate)} should be in future.");
            RuleFor(m => m.Amount).GreaterThan(0).WithMessage($"{nameof(PaymentRequest.Amount)} should have value greater than zero.");
            RuleFor(m => m.Currency).NotEmpty().WithMessage($"{nameof(PaymentRequest.Currency)} can not be empty.");
            RuleFor(m => m.Cvv).NotEmpty().Length(3).WithMessage($"{nameof(PaymentRequest.Cvv)} can not be empty and must have 3 digits.");
        }
    }
}