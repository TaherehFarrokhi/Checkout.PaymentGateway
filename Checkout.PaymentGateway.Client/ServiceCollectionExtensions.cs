using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace Checkout.PaymentGateway.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddClient<T>(this IServiceCollection serviceCollection, string baseUrl) where T : class
        {
            serviceCollection.AddSingleton(sp => RestService.For<T>(baseUrl, new RefitSettings
            {
                ContentSerializer = new JsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                )
            }));
        }

        public static void AddPaymentClient(this IServiceCollection serviceCollection, string baseUrl) => serviceCollection.AddClient<IPaymentGatewayClient>(baseUrl);
    }
}