using AppFramework.Editions.Dto;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace AppFramework.Shared.Models
{
    public class FlatFeatureModel : BindableBase
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; RaisePropertyChanged(); }
        }

        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public FeatureInputTypeDto InputType { get; set; }

        private ObservableCollection<FlatFeatureModel> items;

        public ObservableCollection<FlatFeatureModel> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }
    }
}