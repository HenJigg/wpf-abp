using Abp.Dependency;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AppFramework.ApiClient
{
    public class AuthenticationHttpHandler : DelegatingHandler
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private const string AuthorizationScheme = "Bearer";

        public Func<Task> OnSessionTimeOut { get; set; }

        public Func<string, Task> OnAccessTokenRefresh { get; set; }

        public AuthenticationHttpHandler(HttpMessageHandler innerHandler) :
            base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized &&
                HasBearerAuthorizationHeader(request))
            {
                return await HandleUnauthorizedResponse(request, response, cancellationToken);
            }

            return response;
        }

        private async Task<HttpResponseMessage> HandleUnauthorizedResponse(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                var tokenManager = IocManager.Instance.IocContainer.Resolve<IAccessTokenManager>();

                if (tokenManager.IsRefreshTokenExpired)
                {
                    await HandleSessionExpired(tokenManager);
                }
                else
                {
                    response = await RefreshAccessTokenAndSendRequestAgain(request, cancellationToken, tokenManager);
                }
            }
            finally
            {
                _semaphore.Release();
            }

            return response;
        }

        private async Task<HttpResponseMessage> RefreshAccessTokenAndSendRequestAgain(HttpRequestMessage request, CancellationToken cancellationToken, IAccessTokenManager tokenManager)
        {
            await RefreshToken(tokenManager, request);
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task HandleSessionExpired(IAccessTokenManager tokenManager)
        {
            tokenManager.Logout();

            if (OnSessionTimeOut != null)
            {
                await OnSessionTimeOut();
            }
        }

        private async Task RefreshToken(IAccessTokenManager tokenManager, HttpRequestMessage request)
        {
            var newAccessToken = await tokenManager.RefreshTokenAsync();

            if (OnAccessTokenRefresh != null)
            {
                await OnAccessTokenRefresh(newAccessToken);
            }

            SetRequestAccessToken(newAccessToken, request);
        }

        private static bool HasBearerAuthorizationHeader(HttpRequestMessage request)
        {
            if (request.Headers.Authorization == null)
            {
                return false;
            }

            if (request.Headers.Authorization.Scheme != AuthorizationScheme)
            {
                return false;
            }

            return true;
        }

        private static void SetRequestAccessToken(string accessToken, HttpRequestMessage request)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ApplicationException("Cannot handle empty access token!");
            }

            request.Headers.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, accessToken);
        }
    }
}