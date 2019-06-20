using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Client;
using Checkout.PaymentGetway.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.ClientConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddPaymentClient("https://localhost:5001");
            var sp = services.BuildServiceProvider();
            var paymentGatewayClient = sp.GetRequiredService<IPaymentGatewayClient>();
            var paymentRequest = new PaymentRequest
            {
                CardNumber = "1234567891234567",
                Amount = 10,
                Currency = "USD",
                ExpiryDate = DateTime.Now.AddYears(1),
                Cvv = "123"
            };

            var paymentResult = await paymentGatewayClient.ProcessPaymentAsync(paymentRequest);
            var payment = await paymentGatewayClient.GetPaymentAsync(paymentResult.PaymentReferenceId);
        }
    }
}
