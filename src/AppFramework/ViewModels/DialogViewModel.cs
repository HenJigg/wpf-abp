using AppFramework.Common.ViewModels;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;

namespace AppFramework.ViewModels
{
    public class DialogViewModel : ViewModelBase, IDialogAware
    {
        public string Title { get; set; }
        public event Action<IDialogResult> RequestClose;
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public DialogViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public virtual void Cancel() => OnDialogClosed(ButtonResult.Cancel);

        public virtual void Save() => OnDialogClosed(ButtonResult.OK);

        public virtual bool CanCloseDialog() => true;

        public void OnDialogClosed(ButtonResult result)
        {
            RequestClose?.Invoke(new DialogResult(result));
        }

        public void OnDialogClosed(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public void OnDialogClosed() => OnDialogClosed(ButtonResult.OK);

        public virtual void OnDialogOpened(IDialogParameters parameters)
        { }
    }
}