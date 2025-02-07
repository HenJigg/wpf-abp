using AppFramework.Services;
using AppFramework.Shared.Services;
using Prism.Common;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AppFramework.Admin.SyncUI.Services.Sessions
{
    /// <summary>
    /// 对话主机服务
    /// </summary>
    public class DialogHostService : DialogService, IHostDialogService
    {
        private readonly IContainerExtension _containerExtension;

        public DialogHostService(IContainerExtension containerExtension) : base(containerExtension)
        {
            _containerExtension = containerExtension;
        }

        public IDialogResult ShowWindow(string name)
        {
            IDialogResult dialogResult = new DialogResult(ButtonResult.None);

            var content = _containerExtension.Resolve<object>(name);

            if (!(content is Window dialogContent))
                throw new NullReferenceException("A dialog's content must be a Window");

            if (dialogContent is Window view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (!(dialogContent.DataContext is IDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            if (dialogContent is IDialogWindow dialogWindow)
            {
                ConfigureDialogWindowEvents(dialogWindow, result => { dialogResult = result; });
            }

            MvvmHelpers.ViewAndViewModelAction<IDialogAware>(viewModel, d => d.OnDialogOpened(null));
            dialogContent.ShowDialog();
            return dialogResult;
        }

        public async Task<IDialogResult> ShowDialogAsync(string name, IDialogParameters parameters = null, string IdentifierName = "Root")
        {
            var dialogContent = GetDialogContent(name, IdentifierName);

            if (!(dialogContent.DataContext is IHostDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogHostAware interface");

            var eventHandler = GetDialogOpenedEventHandler(viewModel, parameters);

            var dialogResult = await DialogHost.Show(dialogContent, IdentifierName, eventHandler);

            if (dialogResult == null)
                return new DialogResult(ButtonResult.Cancel);

            return (IDialogResult)dialogResult;
        }

        private FrameworkElement GetDialogContent(string name, string IdentifierName = "Root")
        {
            var content = _containerExtension.Resolve<object>(name);
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            if (dialogContent is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (!(dialogContent.DataContext is IHostDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogHostAware interface");

            viewModel.IdentifierName = IdentifierName;

            return dialogContent;
        }

        private DialogOpenedEventHandler GetDialogOpenedEventHandler(IHostDialogAware viewModel,
            IDialogParameters parameters)
        {
            if (parameters == null) parameters = new DialogParameters();

            DialogOpenedEventHandler eventHandler =
               (sender, eventArgs) =>
               {
                   var _content = eventArgs.Session.Content;
                   if (viewModel is IHostDialogAware aware)
                       aware.OnDialogOpened(parameters);
                   eventArgs.Session.UpdateContent(_content);
               };

            return eventHandler;
        }

        public void Close(string IdentifierName, DialogResult dialogResult)
        {
            DialogHost.Close(IdentifierName, dialogResult);
        }
    }
}