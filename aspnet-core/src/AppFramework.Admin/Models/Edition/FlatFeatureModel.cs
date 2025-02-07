using AppFramework.Editions.Dto;
using CommunityToolkit.Mvvm.ComponentModel; 
using System.Collections.ObjectModel;

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class FlatFeatureModel 
    {
        [ObservableProperty]
        public bool isChecked;
         
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public FeatureInputTypeDto InputType { get; set; }

        [ObservableProperty]
        public ObservableCollection<FlatFeatureModel> items;
    }
}