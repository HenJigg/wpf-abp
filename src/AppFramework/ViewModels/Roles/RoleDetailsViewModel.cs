using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using Prism.Services.Dialogs; 

namespace AppFramework.ViewModels
{
    public class RoleDetailsViewModel : HostDialogViewModel
    {
        #region 字段/属性

        private readonly IRoleAppService appService;
        public IPermissionTreesService treesService { get; set; }
        private RoleEditModel role;

        public RoleEditModel Role
        {
            get { return role; }
            set { role = value; RaisePropertyChanged(); }
        }

        #endregion 字段/属性

        public RoleDetailsViewModel(IRoleAppService appService,
            IPermissionTreesService treesService)
        {
            this.appService = appService;
            this.treesService = treesService;
        }

        /// <summary>
        /// 保存
        /// </summary>
        protected override async void Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () =>
                {
                    await appService.CreateOrUpdateRole(new CreateOrUpdateRoleInput()
                    {
                        Role = Map<RoleEditDto>(Role),
                        GrantedPermissionNames = treesService.GetSelectedItems()
                    });
                });
                base.Save();
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                int? id = null;

                if (parameters != null && parameters.ContainsKey("Value"))
                    id = parameters.GetValue<RoleListDto>("Value").Id;
                var output = await appService.GetRoleForEdit(new NullableIdDto(id));
                Role = Map<RoleEditModel>(output.Role);
                treesService.CreatePermissionTrees(output.Permissions, output.GrantedPermissionNames);
            });
        }
    }
}
