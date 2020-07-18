using Consumption.Mobile.Template;
using Consumption.Mobile.View;
using Consumption.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Rg.Plugins.Popup.Services;
using XF.Material.Forms.UI.Dialogs;
using Consumption.Core.Common;

namespace Consumption.Mobile.ViewCenter
{
    /// <summary>
    /// 登录控制类
    /// </summary>
    public class LoginCenter : BaseDialogCenter<LoginPage, LoginViewModel>
    {
        public override void SubscribeMessenger()
        {
            Messenger.Default.Register<string>(View, "Snackbar", async arg =>
            {
                await PopupNavigation.Instance.PopAllAsync();
                await MaterialDialog.Instance.SnackbarAsync(message: arg);
            });

            Messenger.Default.Register<MsgInfo>(View, "UpdateDialog", async arg =>
             {
                 if (arg.IsOpen)
                     _ = PopupNavigation.Instance.PushAsync(new SplashScreenView(arg.Msg));
                 else
                     await PopupNavigation.Instance.PopAllAsync();
             });
            Messenger.Default.Register<bool>(View, "NavigationPage", arg =>
             {

             });
        }
    }
}
