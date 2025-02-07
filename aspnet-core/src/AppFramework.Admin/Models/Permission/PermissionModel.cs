using CommunityToolkit.Mvvm.ComponentModel; 
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFramework.Admin.Models
{
    [INotifyPropertyChanged]
    public partial class PermissionModel
    {
        public PermissionModel? Parent { get; set; }

        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsGrantedByDefault { get; set; }

        [ObservableProperty]
        private bool isChecked;

        [ObservableProperty]
        public ObservableCollection<PermissionModel> items;

        private void SetIsChecked(bool value, bool checkedChildren, bool checkedParent)
        {
            if (isChecked == value) return;
            isChecked = value;

            if (checkedChildren && Items != null)
            {
                for (int i = 0; i < Items.Count; i++)
                    Items[i].SetIsChecked(value, true, false);
            }

            if (checkedParent && this.Parent != null)
                this.Parent.CheckParentIsCheckedState();

            OnPropertyChanged(nameof(IsChecked));
        }

        private void CheckParentIsCheckedState()
        {
            List<PermissionModel> checkedItems = new List<PermissionModel>();
            string checkedNames = string.Empty;
            bool currentState = this.IsChecked;
            bool firstState = false;
            for (int i = 0; i < this.Items.Count; i++)
            {
                bool childrenState = this.Items[i].IsChecked;
                if (i == 0)
                    firstState = childrenState;
                else if (firstState != childrenState)
                    firstState = false;
            }

            if (!firstState) currentState = firstState;
            SetIsChecked(firstState, false, true);
        }
    }
}