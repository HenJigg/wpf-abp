using Abp.Application.Services.Dto;
using CommunityToolkit.Mvvm.ComponentModel; 

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class ChooseItem 
    {
        public ChooseItem(NameValueDto nameValue)
        {
            value = nameValue;
        }

        [ObservableProperty]
        private bool isSelected;
          
        private NameValueDto value;

        public NameValueDto Value
        {
            get { return value; }
        }
    }
}