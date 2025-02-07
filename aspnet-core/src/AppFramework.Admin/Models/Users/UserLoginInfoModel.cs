using CommunityToolkit.Mvvm.ComponentModel; 

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class UserLoginInfoModel 
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string surname;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string emailAddress;
         
        public string ProfilePictureId { get; set; }
    }
}
