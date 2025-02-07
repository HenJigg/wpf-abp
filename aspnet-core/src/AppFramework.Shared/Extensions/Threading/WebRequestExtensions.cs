using Abp.Runtime.Validation;
using Abp.UI;
using Flurl.Http;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using Prism.Ioc;
using AppFramework.Shared;
using AppFramework.Extensions;

namespace AppFramework
{
    public static class WebRequestExtensions
    {
        private static readonly IDialogService dialogService;

        static WebRequestExtensions()
        {
            dialogService = ContainerLocator.Container.Resolve<IDialogService>();
        }

        public static async Task WebAsync<TResult>(this
          Task<TResult> task,
          Func<TResult, Task> successCallback,
          Func<System.Exception, Task> failCallback = null,
          Action finallyCallback = null)
        {
            if (successCallback == null)
            {
                successCallback = _ => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                await successCallback(await task);
            }
            catch (Exception exception)
            {
                await HandleException(exception, () => task, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }

        public static async Task WebAsync(this
            Task func,
            Func<Task> successCallback = null,
            Func<Exception, Task> failCallback = null,
            Action finallyCallback = null)
        {
            if (successCallback == null)
            {
                successCallback = () => Task.CompletedTask;
            }

            if (failCallback == null)
            {
                failCallback = _ => Task.CompletedTask;
            }

            try
            {
                await func;
                await successCallback();
            }
            catch (System.Exception ex)
            {
                await HandleException(ex, () => func, successCallback, failCallback);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }

        private static async Task HandleException(Exception exception,
           Func<Task> func,
           Func<Task> successCallback,
           Func<Exception, Task> failCallback)
        {
            switch (exception)
            {
                case UserFriendlyException userFriendlyException:
                    await HandleUserFriendlyException(userFriendlyException, failCallback);
                    break;

                case FlurlHttpTimeoutException httpTimeoutException:
                    await HandleFlurlHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;

                case FlurlHttpException httpException:
                    await HandleFlurlHttpException(httpException, func, successCallback, failCallback);
                    break;

                case AbpValidationException abpValidationException:
                    await HandleAbpValidationException(abpValidationException, failCallback);
                    break;

                default:
                    await HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }

        private static async Task HandleException<TResult>(Exception exception,
           Func<Task<TResult>> func,
           Func<TResult, Task> successCallback,
           Func<Exception, Task> failCallback)
        {
            switch (exception)
            {
                case UserFriendlyException userFriendlyException:
                    await HandleUserFriendlyException(userFriendlyException, failCallback);
                    break;

                case FlurlHttpTimeoutException httpTimeoutException:
                    await HandleFlurlHttpTimeoutException(httpTimeoutException, func, successCallback, failCallback);
                    break;

                case FlurlHttpException httpException:
                    await HandleFlurlHttpException(httpException, func, successCallback, failCallback);
                    break;

                case AbpValidationException abpValidationException:
                    await HandleAbpValidationException(abpValidationException, failCallback);
                    break;

                default:
                    await HandleDefaultException(exception, func, successCallback, failCallback);
                    break;
            }
        }

        private static async Task HandleUserFriendlyException(UserFriendlyException userFriendlyException,
           Func<Exception, Task> failCallback)
        {
            if (string.IsNullOrEmpty(userFriendlyException.Details))
                DialogHelper.Error(Local.Localize("Error"), userFriendlyException.Message);
            else
                DialogHelper.Error(userFriendlyException.Message, userFriendlyException.Details);

            await failCallback(userFriendlyException);
        }

        private static async Task HandleFlurlHttpTimeoutException<TResult>(
           FlurlHttpTimeoutException httpTimeoutException,
           Func<Task<TResult>> func,
           Func<TResult, Task> successCallback,
           Func<System.Exception, Task> failCallback)
        {
            var accepted = dialogService.Question(
                LocalTranslationHelper.Localize("MessageTitle"),
                LocalTranslationHelper.Localize("RequestTimedOut"));

            if (accepted)
                await WebRequest.Execute(func, successCallback, failCallback);
            else
                await failCallback(httpTimeoutException);
        }

        private static async Task HandleFlurlHttpTimeoutException(FlurlHttpTimeoutException httpTimeoutException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var accepted = dialogService.Question(
                LocalTranslationHelper.Localize("MessageTitle"),
                LocalTranslationHelper.Localize("RequestTimedOut"));

            if (accepted)
                await WebRequest.Execute(func, successCallback, failCallback);
            else
                await failCallback(httpTimeoutException);
        }

        private static async Task HandleFlurlHttpException(FlurlHttpException httpException,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var httpExceptionMessage = LocalTranslationHelper.Localize("HttpException");
#if DEBUG 
            System.Diagnostics.Debug.WriteLine(httpExceptionMessage + httpException.Message);
#endif

            var accepted = dialogService.Question(
               LocalTranslationHelper.Localize("MessageTitle"),
               httpExceptionMessage);

            if (accepted)
                await WebRequest.Execute(func, successCallback, failCallback);
            else
                await failCallback(httpException);
        }

        private static async Task HandleFlurlHttpException<TResult>(FlurlHttpException httpException,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var httpExceptionMessage = LocalTranslationHelper.Localize("HttpException");
#if DEBUG 
            System.Diagnostics.Debug.WriteLine(httpExceptionMessage + httpException.Message);
#endif

            var accepted = dialogService.Question(
              LocalTranslationHelper.Localize("MessageTitle"),
              httpExceptionMessage);

            if (accepted)
                await WebRequest.Execute(func, successCallback, failCallback);
            else
                await failCallback(httpException);
        }

        private static async Task HandleAbpValidationException(AbpValidationException abpValidationException,
            Func<System.Exception, Task> failCallback)
        {
            DialogHelper.Warning(LocalTranslationHelper.Localize("MessageTitle"),
               abpValidationException.GetConsolidatedMessage());

            await failCallback(abpValidationException);
        }

        private static async Task HandleDefaultException(System.Exception exception,
            Func<Task> func,
            Func<Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            string message;
            var key = exception.Message;

            if (string.IsNullOrWhiteSpace(key))
                message = LocalTranslationHelper.Localize("UnhandledWebRequestException");
            else
                message = LocalTranslationHelper.Localize(key);

            var accepted = dialogService.Question(LocalTranslationHelper.Localize("MessageTitle"),
                message);

            if (accepted)
                await WebRequest.Execute(func, successCallback, failCallback);
            else
                await failCallback(exception);
        }

        private static async Task HandleDefaultException<TResult>(System.Exception exception,
            Func<Task<TResult>> func,
            Func<TResult, Task> successCallback,
            Func<System.Exception, Task> failCallback)
        {
            var accepted = dialogService.Question(
                LocalTranslationHelper.Localize("MessageTitle"),
                LocalTranslationHelper.Localize("UnhandledWebRequestException"));

            if (accepted)
                await WebRequest.Execute(func, successCallback, failCallback);
            else
                await failCallback(exception);
        }
    }
}
