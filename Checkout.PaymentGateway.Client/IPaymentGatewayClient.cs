using System.Threading.Tasks;
using Checkout.PaymentGateway.Common;
using Refit;

namespace Checkout.PaymentGateway.Client
{
    public interface IPaymentGatewayClient
    {
        [Post("/api/v1/payment/")]
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest paymentRequest);

        [Get("/api/v1/payment/{paymentId}")]
        Task<Payment> GetPaymentAsync(string paymentId);
    }
}