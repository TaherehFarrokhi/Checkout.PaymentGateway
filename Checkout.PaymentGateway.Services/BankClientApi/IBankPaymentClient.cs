using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Common;

namespace Checkout.PaymentGateway.Services.BankClientApi
{
    public interface IBankPaymentClient
    {
        Task<BankPaymentResult> ProcessPaymentAsync(PaymentRequest bankPaymentRequest);
    }
}