using Consumption.ViewModel;
using Consumption.ViewModel.Interfaces;
using MaterialDesignThemes.Wpf;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Consumption.PC.ViewModels
{
    public class LoginViewModel : LoginBaseViewModel
    {
        public LoginViewModel(IUserRepository repository, IContainerProvider containerProvider)
            : base(repository, containerProvider)
        {
            SnackbarMessage = new SnackbarMessageQueue();
        }

        private SnackbarMessageQueue snackbarMessageQueue;

        public SnackbarMessageQueue SnackbarMessage
        {
            get { return snackbarMessageQueue; }
            set { snackbarMessageQueue = value; OnPropertyChanged(); }
        }

        public override void SnackBar(string msg) => SnackbarMessage.Enqueue(msg);
    }
}
