using System;
using System.Threading.Tasks;
using Checkout.PaymentGetway.Common;

namespace Checkout.PaymentGateway.Services.BankClientApi
{
    public interface IBankPaymentClient
    {
        Task<BankPaymentResult> ProcessPaymentAsync(PaymentRequest bankPaymentRequest);
    }
}