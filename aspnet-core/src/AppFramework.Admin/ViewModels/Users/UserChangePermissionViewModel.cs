using AppFramework.Authorization.Users.Dto;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using AppFramework.Authorization.Users;
using AppFramework.Shared;
using AppFramework.Admin.Services;

namespace AppFramework.Admin.ViewModels
{
    public class UserChangePermissionViewModel : HostDialogViewModel
    {
        public UserChangePermissionViewModel(IUserAppService userAppService,
            IPermissionTreesService treesService)
        {
            this.userAppService = userAppService;
            this.treesService = treesService;
        }

        private long Id;
        private readonly IUserAppService userAppService;
        public IPermissionTreesService treesService { get; set; }

        public override async Task Save()
        {
            await SetBusyAsync(async () =>
            {
                await userAppService.UpdateUserPermissions(new UpdateUserPermissionsInput()
                {
                    Id = Id,
                    GrantedPermissionNames = treesService.GetSelectedItems()
                }).WebAsync(base.Save);
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                if (parameters.ContainsKey("Value"))
                {
                    var output = parameters.GetValue<GetUserPermissionsForEditOutput>("Value");
                    treesService.CreatePermissionTrees(output.Permissions, output.GrantedPermissionNames);
                }

                if (parameters.ContainsKey("Id"))
                    Id = parameters.GetValue<long>("Id");

                await Task.CompletedTask;
            });
        }
    }
}
