using AppFramework.Authorization.Users.Dto;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using AppFramework.Authorization.Users;
using AppFramework.Common;
using System.Linq;

namespace AppFramework.ViewModels
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

        protected override async void Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(async () =>
                {
                    await userAppService.UpdateUserPermissions(new UpdateUserPermissionsInput()
                    {
                        Id = Id,
                        GrantedPermissionNames = treesService.GetSelectedItems()
                    });
                }, async () =>
                {
                    base.Save();
                    await Task.CompletedTask;
                });
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
