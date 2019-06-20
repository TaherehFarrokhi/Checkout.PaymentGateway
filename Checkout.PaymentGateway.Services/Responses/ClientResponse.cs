namespace Checkout.PaymentGateway.Services.Responses
{
    public class ClientResponse<T> where T : class
    {
        public bool Success { get; set; }
        public T Payload { get; set; }
        public string ErrorMessage { get; set; }

        public static ClientResponse<T> FromPayload(T payload)
        {
            return new ClientResponse<T>
            {
                Success = true,
                Payload = payload
            };
        }

        public static ClientResponse<T> FromError(string errorMessage, T payload)
        {
            return new ClientResponse<T>
            {
                ErrorMessage = errorMessage,
                Payload = payload
            };
        }

    }
}