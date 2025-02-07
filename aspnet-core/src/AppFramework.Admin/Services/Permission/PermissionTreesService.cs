using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Admin.Models;
using AppFramework.Shared.Services.Mapper;  
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppFramework.Admin.Services
{
    [INotifyPropertyChanged]
    public partial class PermissionTreesService :   IPermissionTreesService
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
            set { permissions = value; OnPropertyChanged(); }
        }

        private ObservableCollection<object> selectedItems;

        public ObservableCollection<object> SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; OnPropertyChanged(); }
        }

        public void CreatePermissionTrees(List<FlatPermissionDto> permissions, List<string> grantedPermissionNames)
        {
            if (permissions == null)
                throw new NullReferenceException(nameof(permissions));

            if (grantedPermissionNames == null)
                throw new NullReferenceException(nameof(grantedPermissionNames));

            var flats = mapper.Map<List<PermissionModel>>(permissions);

            Permissions = flats.CreateTrees(null);
            SelectedItems = Permissions.GetSelectedItems(grantedPermissionNames);
        }

        public List<string> GetSelectedItems()
        {
            if (SelectedItems == null && SelectedItems.Count == 0) return null;

            return SelectedItems.Select(t => (t as PermissionModel)?.Name).ToList();
        }
    }
}
