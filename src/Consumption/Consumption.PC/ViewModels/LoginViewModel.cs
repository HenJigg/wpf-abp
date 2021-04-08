using Consumption.ViewModel;
using Consumption.ViewModel.Interfaces;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Consumption.PC.ViewModels
{
    public class LoginViewModel : LoginViewModelBase
    {
        public LoginViewModel(IUserRepository repository) : base(repository)
        {
            SnackbarMessage = new SnackbarMessageQueue();
        }

        private SnackbarMessageQueue snackbarMessageQueue;

        public SnackbarMessageQueue SnackbarMessage
        {
            get { return snackbarMessageQueue; }
            set { snackbarMessageQueue = value; OnPropertyChanged(); }
        }

        public override void SnackBar(string msg)
        {
            SnackbarMessage.Enqueue(msg);
        }
    }
}
