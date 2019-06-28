using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Common;

namespace Checkout.PaymentGateway.Services.BankClientApi
{
    [ExcludeFromCodeCoverage]
    public class MockBankPaymentClient : IBankPaymentClient
    {
        public Task<BankPaymentResult> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            if (paymentRequest == null) throw new ArgumentNullException(nameof(paymentRequest));

            var paymentResult =  new BankPaymentResult
            {
                Status = true,
                PaymentReferenceId = Guid.NewGuid().ToString("N")
            };

            return Task.FromResult(paymentResult);
        }
    }
}