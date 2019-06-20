using System.Threading.Tasks;
using Checkout.PaymentGateway.Common;
using Checkout.PaymentGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> Get(string id)
        {
            var result = await _paymentService.GetPaymentAsync(id);
            if (result.Success)
                return Ok(result.Payload);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResult>> Post([FromBody] PaymentRequest paymentRequest)
        {
            var result = await _paymentService.ProcessPaymentAsync(paymentRequest);
            if (result.Success)
                return Ok(result.Payload);
            return BadRequest(result.ErrorMessage);
        }
    }
}
