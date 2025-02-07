using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Admin.Models;
using AppFramework.Shared.Services.Mapper;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AppFramework.Admin.Services;

namespace AppFramework.Admin.MaterialUI.Services
{
    public class PermissionTreesService : BindableBase, IPermissionTreesService
    {
        private readonly IAppMapper mapper;

        public PermissionTreesService(IAppMapper mapper)
        {
            this.mapper = mapper;
        }

        private ObservableCollection<PermissionModel> permissions;

        public ObservableCollection<PermissionModel> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<object> selectedItems;

        public ObservableCollection<object> SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; RaisePropertyChanged(); }
        }

        public void CreatePermissionTrees(List<FlatPermissionDto> permissions, List<string> grantedPermissionNames)
        {
            if (permissions == null)
                throw new NullReferenceException(nameof(permissions));

            if (grantedPermissionNames == null)
                throw new NullReferenceException(nameof(grantedPermissionNames));

            var flats = mapper.Map<List<PermissionModel>>(permissions);

            Permissions = flats.CreateTrees(null); 
            UpdatePermissionsIsCheckedState(Permissions, grantedPermissionNames);
        }

        public List<string> GetSelectedItems()
        {
            List<string> selectedItems = new List<string>();
            foreach (var item in Permissions)
            {
                if (item.IsChecked) selectedItems.Add(item.Name);
                if (item.Items!=null&& item.Items.Count>0)
                    GetParentIsCheckedItem(selectedItems, item);
            }

            return selectedItems;
        }

        private void GetParentIsCheckedItem(List<string> selectedItems, PermissionModel model)
        {
            foreach (var item in model.Items)
            {
                if (item.IsChecked) selectedItems.Add(item.Name);
                if (item.Items!=null&& item.Items.Count>0)
                    GetParentIsCheckedItem(selectedItems, item);
            }
        }
         
        public void UpdatePermissionsIsCheckedState(ObservableCollection<PermissionModel> nodes, List<string> GrantedPermissionNames)
        {
            foreach (var item in GrantedPermissionNames)
                UpdateIsCheckedState(nodes, item);

            void UpdateIsCheckedState(ObservableCollection<PermissionModel> nodes, string key)
            {
                foreach (var flat in nodes)
                {
                    if (flat.Name.Equals(key))
                        flat.IsChecked=true;

                    UpdateIsCheckedState(flat.Items, key);
                }
            }
        }
    }
}
