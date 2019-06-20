using System.Threading.Tasks;
using Checkout.PaymentGateway.Services.Responses;
using Checkout.PaymentGetway.Common;

namespace Checkout.PaymentGateway.Services
{
    public interface IPaymentService
    {
        Task<ClientResponse<PaymentResult>> ProcessPaymentAsync(PaymentRequest paymentRequest);
        Task<ClientResponse<Payment>> GetPaymentAsync(string paymentId);
    }
}