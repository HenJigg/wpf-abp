using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.UI;
using Abp.Web.Models;
using AppFramework.Extensions;
using Flurl.Http;
using Flurl.Http.Content;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AppFramework.ApiClient
{
    public class AbpApiClient : ISingletonDependency, IDisposable
    {
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IApplicationContext _applicationContext;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private static FlurlClient _client;

        public static int? TimeoutSeconds { get; set; } = 30;

        public AbpApiClient(
            IAccessTokenManager accessTokenManager,
            IApplicationContext applicationContext,
            IMultiTenancyConfig multiTenancyConfig)
        {
            _accessTokenManager = accessTokenManager;
            _applicationContext = applicationContext;
            _multiTenancyConfig = multiTenancyConfig;
        }

        #region PostAsync<T>

        public async Task<T> PostAsync<T>(string endpoint)
        {
            return await PostAsync<T>(endpoint, null, null, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task<T> PostAnonymousAsync<T>(string endpoint)
        {
            return await PostAsync<T>(endpoint, null, null, null, true);
        }

        public async Task<T> PostAsync<T>(string endpoint, object inputDto)
        {
            return await PostAsync<T>(endpoint, inputDto, null, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task<T> PostAsync<T>(string endpoint, object inputDto, object queryParameters)
        {
            return await PostAsync<T>(endpoint, inputDto, queryParameters, _accessTokenManager.GetAccessToken(), true);
        }

        /// <summary>
        /// Makes POST request without authentication token.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<T> PostAnonymousAsync<T>(string endpoint, object inputDto)
        {
            return await PostAsync<T>(endpoint, inputDto, null, null, true);
        }

        public async Task<T> PostAsync<T>(string endpoint, object inputDto, object queryParameters, string accessToken, bool stripAjaxResponseWrapper)
        {
            var httpResponse = GetClient(accessToken)
                .Request(endpoint)
                .SetQueryParams(queryParameters)
                .PostJsonAsync(inputDto);

            return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
        }

        public async Task<T> PostMultipartAsync<T>(string endpoint, Action<CapturedMultipartContent> buildContent, bool stripAjaxResponseWrapper = true)
        {
            var httpResponse = GetClient(_accessTokenManager.GetAccessToken())
                .Request(endpoint)
                .PostMultipartAsync(buildContent);

            return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
        }

        public async Task<T> PostMultipartAsync<T>(string endpoint, Stream stream, string fileName, bool stripAjaxResponseWrapper = true)
        {
            var httpResponse = GetClient(_accessTokenManager.GetAccessToken())
                    .Request(endpoint)
                    .PostMultipartAsync(multiPartContent => multiPartContent.AddFile("file", stream, fileName));
            return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
        }

        public async Task<T> PostMultipartAsync<T>(string endpoint, string filePath, bool stripAjaxResponseWrapper = true)
        {
            var httpResponse = GetClient(_accessTokenManager.GetAccessToken())
                .Request(endpoint)
                .PostMultipartAsync(multiPartContent => multiPartContent.AddFile("file", filePath));
            return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
        }

        #endregion PostAsync<T>

        #region PostAsync

        public async Task PostAsync(string endpoint)
        {
            await PostAsync(endpoint, null, null, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task PostAsync(string endpoint, object inputDto)
        {
            await PostAsync(endpoint, inputDto, null, _accessTokenManager.GetAccessToken(), true);
        }

        /// <summary>
        /// Makes POST request without authentication token.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task PostAnonymousAsync(string endpoint, object inputDto)
        {
            await PostAsync(endpoint, inputDto, null, null, true);
        }

        public async Task PostAsync(string endpoint, object inputDto, object queryParameters)
        {
            await PostAsync(endpoint, inputDto, queryParameters, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task PostAsync(string endpoint, object inputDto, object queryParameters, string accessToken,
            bool stripAjaxResponseWrapper)
        {
            await PostAsync<object>(endpoint, inputDto, queryParameters, accessToken, stripAjaxResponseWrapper);
        }

        #endregion PostAsync

        #region GetAsync<T>

        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await GetAsync<T>(endpoint, null);
        }

        /// <summary>
        /// Makes GET request without authentication token.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<T> GetAnonymousAsync<T>(string endpoint)
        {
            return await GetAsync<T>(endpoint, null, null, true);
        }

        public async Task<T> GetAsync<T>(string endpoint, object queryParameters)
        {
            return await GetAsync<T>(endpoint, queryParameters, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task<T> GetAsync<T>(string endpoint, object queryParameters, string accessToken, bool stripAjaxResponseWrapper)
        {
            var httpResponse = GetClient(accessToken)
                .Request(endpoint)
                .SetQueryParams(queryParameters)
                .GetAsync();

            return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
        }

        #endregion GetAsync<T>

        #region GetAsync

        public async Task GetAsync(string endpoint)
        {
            await GetAsync(endpoint, null);
        }

        public async Task GetAsync(string endpoint, object queryParameters)
        {
            await GetAsync(endpoint, queryParameters, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task GetAsync(string endpoint, object queryParameters, string accessToken, bool stripAjaxResponseWrapper)
        {
            await GetAsync<object>(endpoint, queryParameters, accessToken, stripAjaxResponseWrapper);
        }

        #endregion GetAsync

        #region GetStringAsync

        public async Task GetStringAsync(string endpoint)
        {
            await GetStringAsync(endpoint, null);
        }

        public async Task GetStringAsync(string endpoint, object queryParameters)
        {
            await GetStringAsync(endpoint, queryParameters, _accessTokenManager.GetAccessToken());
        }

        public async Task<string> GetStringAsync(string endpoint, object queryParameters, string accessToken)
        {
            return await GetClient(accessToken)
                    .Request(endpoint)
                    .SetQueryParams(queryParameters)
                    .GetStringAsync();
        }

        #endregion GetStringAsync

        #region DeleteAsync

        public async Task DeleteAsync(string endpoint)
        {
            await DeleteAsync(endpoint, null, _accessTokenManager.GetAccessToken());
        }

        public async Task DeleteAsync(string endpoint, object queryParameters)
        {
            await DeleteAsync(endpoint, queryParameters, _accessTokenManager.GetAccessToken());
        }

        public async Task DeleteAsync(string endpoint, object queryParameters, string accessToken)
        {
            await DeleteAsync<object>(endpoint, queryParameters, accessToken, true);
        }

        #endregion DeleteAsync

        #region DeleteAsync<T>

        public async Task<T> DeleteAsync<T>(string endpoint)
        {
            return await DeleteAsync<T>(endpoint, null);
        }

        public async Task<T> DeleteAsync<T>(string endpoint, object queryParameters)
        {
            return await DeleteAsync<T>(endpoint, queryParameters, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task<T> DeleteAsync<T>(string endpoint, object queryParameters, string accessToken, bool stripAjaxResponseWrapper)
        {
            var httpResponse = GetClient(accessToken)
                .Request(endpoint)
                .SetQueryParams(queryParameters)
                .DeleteAsync();

            return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
        }

        #endregion DeleteAsync<T>

        #region PutAsync<T>

        public async Task<T> PutAsync<T>(string endpoint)
        {
            return await PutAsync<T>(endpoint, null, null, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task<T> PutAsync<T>(string endpoint, object inputDto)
        {
            return await PutAsync<T>(endpoint, inputDto, null, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task<T> PutAsync<T>(string endpoint, object inputDto, object queryParameters)
        {
            return await PutAsync<T>(endpoint, inputDto, queryParameters, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task<T> PutAsync<T>(string endpoint, object inputDto, object queryParameters, string accessToken, bool stripAjaxResponseWrapper)
        {
            var httpResponse = GetClient(accessToken)
                .Request(endpoint)
                .SetQueryParams(queryParameters)
                .PutJsonAsync(inputDto);

            return await ValidateAbpResponse<T>(httpResponse, stripAjaxResponseWrapper);
        }

        #endregion PutAsync<T>

        #region PutAsync

        public async Task PutAsync(string endpoint)
        {
            await PutAsync(endpoint, null, null, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task PutAsync(string endpoint, object inputDto)
        {
            await PutAsync(endpoint, inputDto, null, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task PutAsync(string endpoint, object inputDto, object queryParameters)
        {
            await PutAsync(endpoint, inputDto, queryParameters, _accessTokenManager.GetAccessToken(), true);
        }

        public async Task PutAsync(string endpoint, object inputDto, object queryParameters, string accessToken,
            bool stripAjaxResponseWrapper)
        {
            await PutAsync<object>(endpoint, inputDto, queryParameters, accessToken, stripAjaxResponseWrapper);
        }

        #endregion PutAsync

        #region Download

        public async Task<string> DownloadAsync(string endpoint, string localFolderPath, string localFileName = null)
        {
            return await GetClient(_accessTokenManager.GetAccessToken())
                  .Request(endpoint)
                  .DownloadFileAsync(localFolderPath, localFileName);
        }

        #endregion

        public FlurlClient GetClient(string accessToken)
        {
            if (_client == null)
            {
                CreateClient();
            }

            AddHeaders(accessToken);
            return _client;
        }

        private static void CreateClient()
        {
            _client = new FlurlClient(ApiUrlConfig.BaseUrl);

            if (TimeoutSeconds.HasValue)
            {
                _client.WithTimeout(TimeoutSeconds.Value);
            }
        }

        private void AddHeaders(string accessToken)
        {
            _client.Headers.Clear();
            _client.HttpClient.DefaultRequestHeaders.Clear();

            _client.WithHeader("Accept", "application/json");
            _client.WithHeader("User-Agent", AppFrameworkConsts.AbpApiClientUserAgent);
            /* Disabled due to https://github.com/paulcbetts/ModernHttpClient/issues/198#issuecomment-181263988
               _client.WithHeader("Accept-Encoding", "gzip,deflate");
            */

            if (_applicationContext.CurrentLanguage != null)
            {
                _client.WithHeader(".AspNetCore.Culture", "c=" + _applicationContext.CurrentLanguage.Name + "|uic=" + _applicationContext.CurrentLanguage.Name);
            }

            if (_applicationContext.CurrentTenant != null)
            {
                _client.WithHeader(_multiTenancyConfig.TenantIdResolveKey, _applicationContext.CurrentTenant.TenantId);
            }

            if (accessToken != null)
            {
                _client.WithOAuthBearerToken(accessToken);
            }
        }

        private static async Task<T> ValidateAbpResponse<T>(Task<IFlurlResponse> httpResponse,
            bool stripAjaxResponseWrapper)
        {
            if (!stripAjaxResponseWrapper)
            {
                return await httpResponse.ReceiveJson<T>();
            }

            AjaxResponse<T> response;
            try
            {
                response = await httpResponse.ReceiveJson<AjaxResponse<T>>();
            }
            catch (FlurlHttpException e)
            {
                response = await e.GetResponseJsonAsync<AjaxResponse<T>>();
            }

            if (response == null)
            {
                return default;
            }

            if (response.Success)
            {
                return response.Result;
            }

            if (response.Error == null)
            {
                return response.Result;
            }

            throw new UserFriendlyException(response.Error.GetConsolidatedMessage());
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}