using Prism.Services.Dialogs; 

namespace AppFramework.ViewModels
{
    public class MessageViewModel : DialogViewModel
    { 
        private string message;
         
        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Title"))
                Title = parameters.GetValue<string>("Title");

            if (parameters.ContainsKey("Message"))
                Message = parameters.GetValue<string>("Message");
        }
    }
}
