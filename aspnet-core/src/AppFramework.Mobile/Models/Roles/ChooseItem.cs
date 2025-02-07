using Abp.Application.Services.Dto;
using Prism.Mvvm;

namespace AppFramework.Shared.Models
{
    public class ChooseItem : BindableBase
    {
        public ChooseItem(NameValueDto nameValue)
        {
            value = nameValue;
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; RaisePropertyChanged(); }
        }

        private NameValueDto value;

        public NameValueDto Value
        {
            get { return value; }
        }
    }
}