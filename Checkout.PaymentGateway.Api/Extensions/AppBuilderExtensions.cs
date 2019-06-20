using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;

namespace Checkout.PaymentGateway.Api.Extensions
{
    public static class AppBuilderExtensions
    {
        public static void UseSwaggerRedirect(this IApplicationBuilder app)
        {
            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);
        }
    }
}