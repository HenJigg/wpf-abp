using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using Prism.Navigation;
using System.Threading.Tasks;
using AppFramework.Shared.Core;
using AppFramework.Shared.Models;
using AppFramework.Shared.Services.Messenger;

namespace AppFramework.Shared.ViewModels
{
    public class RoleDetailsViewModel : NavigationDetailViewModel
    {
        #region 字段/属性

        public IPermissionTreesService treesService { get; set; }
        private readonly IRoleAppService appService;
        private readonly IMessenger messenger;
        private RoleEditModel role;

        public RoleEditModel Role
        {
            get { return role; }
            set { role = value; RaisePropertyChanged(); }
        }

        #endregion 字段/属性

        public RoleDetailsViewModel(
            IMessenger messenger,
            IRoleAppService appService,
            IPermissionTreesService treesService)
        {
            this.appService = appService;
            this.messenger = messenger;
            this.treesService = treesService;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override async void Save()
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
                }, GoBackAsync);
            });
        }

        public override async Task GoBackAsync()
        {
            messenger.Send(AppMessengerKeys.Role);//通知列表更新
            await base.GoBackAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
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