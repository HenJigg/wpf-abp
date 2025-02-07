using Abp.Web.Models;
using Acr.UserDialogs;
using AppFramework.ApiClient;
using AppFramework.Shared;
using AppFramework.Extensions; 
using Castle.Core.Internal;
using Flurl.Http;
using Prism.Ioc;
using System.Net;
using System.Threading.Tasks;

namespace AppFramework.Shared
{
    public class ExceptionHandler
    {
        public async Task<bool> HandleIfAbpResponseAsync(FlurlHttpException httpException)
        {
            AjaxResponse ajaxResponse = await httpException.GetResponseJsonAsync<AjaxResponse>();
            if (ajaxResponse == null)
            {
                return false;
            }

            if (!ajaxResponse.__abp)
            {
                return false;
            }

            if (ajaxResponse.Error == null)
            {
                return false;
            }

            if (IsUnauthroizedResponseForSessionTimoutCase(httpException, ajaxResponse))
            {
                return true;
            }

            UserDialogs.Instance.HideLoading();

            if (string.IsNullOrEmpty(ajaxResponse.Error.Details))
            {
                UserDialogs.Instance.Alert(ajaxResponse.Error.GetConsolidatedMessage(), Local.Localize("Error"));
            }
            else
            {
                UserDialogs.Instance.Alert(ajaxResponse.Error.Details, ajaxResponse.Error.GetConsolidatedMessage());
            }

            return true;
        }

        /// <summary>
        /// AuthenticationHttpHandler handles unauthorized responses and reauthenticates if there's a valid refresh token.
        /// When the refresh token expires, the application logsout and forces user to re-enter credentials
        /// That's why the last unauthorized exception can be suspended.
        /// </summary>
        private static bool IsUnauthroizedResponseForSessionTimoutCase(FlurlHttpException httpException, AjaxResponse ajaxResponse)
        {
            if (httpException.Call.HttpResponseMessage.StatusCode != HttpStatusCode.Unauthorized)
            {
                return false;
            }

            var accessTokenManager = ContainerLocator.Container.Resolve<IAccessTokenManager>();
            var appContext = ContainerLocator.Container.Resolve<IApplicationContext>();

            var errorMsg = appContext.Configuration.Localization.Localize("CurrentUserDidNotLoginToTheApplication", "Abp");

            if (accessTokenManager.IsUserLoggedIn)
            {
                return false;
            }

            if (!ajaxResponse.Error.Message.EqualsText(errorMsg))
            {
                return false;
            }

            return true;
        }
    }
}