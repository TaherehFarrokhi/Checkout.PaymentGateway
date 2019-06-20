using System.Text;

namespace Checkout.PaymentGateway.Services
{
    public static class StringExtensions
    {
        public static string MaskFromStart(this string value, int displayCount, char mask)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            var sb = new StringBuilder(value);
            for (var i = 0; i < sb.Length - displayCount; i++)
            {
                sb[i] = mask;
            }

            return sb.ToString();
        }
    }
}