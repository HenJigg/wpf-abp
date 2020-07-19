

namespace Consumption.Mobile.ViewCenter
{
    using Consumption.Core.Entity;
    using Consumption.Core.Interfaces;
    using GalaSoft.MvvmLight;
    using NLog.Filters;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Xamarin.Forms;
    using IModule = Core.Interfaces.IModule;

    /// <summary>
    /// View/ViewModel 控制基类
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public class BaseCenter<TView, TViewModel> : IModule
        where TView : ContentPage, new()
        where TViewModel : ViewModelBase, new()
    {

        public TView View = new TView();
        public TViewModel ViewModel = new TViewModel();

        public virtual async Task BindDefaultModel(int AuthValue)
        {
            if (ViewModel is IAuthority authority)
                authority.InitPermissions(AuthValue);

            if (ViewModel is IDataPager dataPager)
                await dataPager.GetPageData(0);
            View.BindingContext = ViewModel;
        }

        public void BindDefaultModel()
        {
        }

        public Task<bool> ShowView()
        {
            throw new NotImplementedException();
        }

        object IModule.GetView()
        {
            return View;
        }
    }
}
