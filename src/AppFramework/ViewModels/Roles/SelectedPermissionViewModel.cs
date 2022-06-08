using Abp.Application.Services.Dto;
using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class SelectedPermissionViewModel : HostDialogViewModel
    {
        public IPermissionTreesService treesService { get; set; }

        public SelectedPermissionViewModel(IPermissionTreesService treesService)
        {
            this.treesService = treesService;
        }

        protected override void Save()
        {
            base.Save(treesService.GetSelectedItems());
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                var flatPermissions = parameters.GetValue<ListResultDto<FlatPermissionWithLevelDto>>("Value");
                var permissions = flatPermissions.Items.Select(t => t as FlatPermissionDto).ToList();
                treesService.CreatePermissionTrees(permissions, new List<string>());
            }
        }
    }
}
