using Checkout.PaymentGateway.Services;
using Checkout.PaymentGateway.Services.BankClientApi;
using Checkout.PaymentGateway.Services.Stores;
using Checkout.PaymentGateway.Services.Validators;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Checkout.PaymentGateway.Api.Extensions
{
    public static class ServiceConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IBankPaymentClient, MockBankPaymentClient>();
            services.AddSingleton<IPaymentValidator, PaymentValidator>();
            services.AddSingleton<IPaymentStore, PaymentStore>();
            services.AddTransient<IPaymentService, PaymentService>();

            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Payment Gateway API", Version = "v1" });
            });
        }
    }
}