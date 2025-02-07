using AppFramework.Services; 
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using Prism.Ioc;
using AppFramework.Shared.Services;
using CommunityToolkit.Mvvm.Input;

namespace AppFramework.Shared
{
    public abstract partial class HostDialogViewModel : ViewModelBase, IHostDialogAware
    {
        public string Title { get; set; }

        public string IdentifierName { get; set; }

        private IHostDialogService dialogService;

        public HostDialogViewModel()
        {
            dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
        }

        [RelayCommand]
        public virtual void Cancel()
        {
            dialogService.Close(IdentifierName, new DialogResult(ButtonResult.No));
        }

        [RelayCommand]
        public virtual async Task Save()
        {
            dialogService.Close(IdentifierName, new DialogResult(ButtonResult.OK));
            await Task.CompletedTask;
        }

        protected virtual void Save(object value)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", value);

            dialogService.Close(IdentifierName, new DialogResult(ButtonResult.OK, param));
        }

        protected virtual void Save(DialogParameters param)
        {
            dialogService.Close(IdentifierName, new DialogResult(ButtonResult.OK, param));
        }

        public abstract void OnDialogOpened(IDialogParameters parameters);
    }
}