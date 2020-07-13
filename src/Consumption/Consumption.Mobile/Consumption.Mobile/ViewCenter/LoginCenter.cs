using Consumption.Core.Common;
using Consumption.Mobile.View;
using Consumption.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.Resources;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace Consumption.Mobile.ViewCenter
{
    /// <summary>
    /// 登录控制类
    /// </summary>
    public class LoginCenter : BaseDialogCenter<LoginPage, LoginViewModel>
    {
        public override void SubscribeMessenger()
        {
            Messenger.Default.Register<MsgInfo>(View, "UpdateDialog",
                async arg =>
            {
                await MaterialDialog.Instance.LoadingDialogAsync(message: arg.Msg,
                    lottieAnimation: "test");
            });
            Messenger.Default.Register<bool>(View, "NavigationPage", arg =>
           {

           });
            Messenger.Default.Register<bool>(View, "Exit", arg =>
            {
            });
        }
    }
}
