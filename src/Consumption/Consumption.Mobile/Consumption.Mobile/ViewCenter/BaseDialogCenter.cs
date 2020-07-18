using System;
using System.Collections.Generic;
using System.Text;

namespace Consumption.Mobile.ViewCenter
{
    using GalaSoft.MvvmLight;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class BaseDialogCenter<TView, TViewModel>
        where TView : ContentPage, new()
        where TViewModel : ViewModelBase, new()
    {
        protected TView View = new TView();
        protected TViewModel ViewModel = new TViewModel();

        /// <summary>
        /// 绑定默认ViewModel
        /// </summary>
        protected void BindDefaultViewModel()
        {
            View.BindingContext = ViewModel;
        }

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns></returns>
        public virtual ContentPage GetContentPage()
        {
            this.BindDefaultViewModel();
            return View;
        }

        public virtual void SubscribeMessenger()
        {
        }
    }
}
