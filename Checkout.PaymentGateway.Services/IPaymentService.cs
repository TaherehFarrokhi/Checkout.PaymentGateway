using System.Threading.Tasks;
using Checkout.PaymentGateway.Common;
using Checkout.PaymentGateway.Services.Responses;

namespace Checkout.PaymentGateway.Services
{
    public interface IPaymentService
    {
        Task<ClientResponse<PaymentResult>> ProcessPaymentAsync(PaymentRequest paymentRequest);
        Task<ClientResponse<Payment>> GetPaymentAsync(string paymentId);
    }
}