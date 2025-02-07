using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class OrganizationListModel 
    {
        [ObservableProperty]
        private bool isChecked;

        [ObservableProperty]
        private string displayName;

        [ObservableProperty]
        private int memberCount;

        [ObservableProperty]
        private int roleCount;

        [ObservableProperty]
        private ObservableCollection<object> items = new ObservableCollection<object>();
          
        public long Id { get; set; }

        public long? ParentId { get; set; }

        public string Code { get; set; } 
    }
}