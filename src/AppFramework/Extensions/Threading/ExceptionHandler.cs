using Abp.Web.Models;
using AppFramework.ApiClient;
using AppFramework.Extensions;
using Castle.Core.Internal;
using Flurl.Http;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Net;
using System.Threading.Tasks;

namespace AppFramework.Common
{
    public class ExceptionHandler
    {
        private readonly IDialogService dialog;

        public ExceptionHandler(IDialogService dialog)
        {
            this.dialog = dialog;
        }

        public async Task<bool> HandleIfAbpResponseAsync(FlurlHttpException httpException)
        {
            AjaxResponse ajaxResponse = await httpException.GetResponseJsonAsync<AjaxResponse>();
            if (ajaxResponse == null)
                return false;

            if (!ajaxResponse.__abp)
                return false;

            if (ajaxResponse.Error == null)
                return false;

            if (IsUnauthroizedResponseForSessionTimoutCase(httpException, ajaxResponse))
                return true;
             
            if (string.IsNullOrEmpty(ajaxResponse.Error.Details))
                dialog.ShowMessage(Local.Localize("Error"), ajaxResponse.Error.GetConsolidatedMessage());
            else
                dialog.ShowMessage(ajaxResponse.Error.GetConsolidatedMessage(), ajaxResponse.Error.Details);

            return true;
        }

        /// <summary>
        /// AuthenticationHttpHandler 处理未经授权的响应并在存在有效刷新令牌时重新进行身份验证。
        /// 当刷新令牌过期时，应用程序注销并强制用户重新输入凭据
        /// 这就是为什么最后一个未授权的异常可以被挂起。
        ///</summary>
        private static bool IsUnauthroizedResponseForSessionTimoutCase(FlurlHttpException httpException, AjaxResponse ajaxResponse)
        {
            if (httpException.Call.HttpResponseMessage.StatusCode != HttpStatusCode.Unauthorized)
                return false;

            var accessTokenManager = ContainerLocator.Container.Resolve<IAccessTokenManager>();
            var appContext = ContainerLocator.Container.Resolve<IApplicationContext>();

            var errorMsg = appContext.Configuration.Localization.Localize("CurrentUserDidNotLoginToTheApplication", "Abp");

            if (accessTokenManager.IsUserLoggedIn)
                return false;

            if (!ajaxResponse.Error.Message.EqualsText(errorMsg))
                return false;

            return true;
        }
    }
}