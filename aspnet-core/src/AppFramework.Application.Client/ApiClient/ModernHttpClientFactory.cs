using Flurl.Http.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppFramework.ApiClient
{
    public class ModernHttpClientFactory : DefaultHttpClientFactory
    {
        /// <summary>
        /// Callback function for refresh token is expired
        /// </summary>
        public Func<Task> OnSessionTimeOut { get; set; }

        /// <summary>
        /// Callback function for access token refresh
        /// </summary>
        public Func<string, Task> OnAccessTokenRefresh { get; set; }

        public override HttpMessageHandler CreateMessageHandler()
        {
            var httpClientHandler = new System.Net.Http.HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

#if DEBUG
            TrustLocalDeveloperCert(httpClientHandler);
#endif

            return new AuthenticationHttpHandler(httpClientHandler)
            {
                OnSessionTimeOut = OnSessionTimeOut,
                OnAccessTokenRefresh = OnAccessTokenRefresh
            };
        }

        private static void TrustLocalDeveloperCert(HttpClientHandler messageHandler)
        {
            messageHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        }
    }
}