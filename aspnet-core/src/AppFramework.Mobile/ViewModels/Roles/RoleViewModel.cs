using Abp.Application.Services.Dto;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using System.Threading.Tasks; 
using AppFramework.Shared.Models; 

namespace AppFramework.Shared.ViewModels
{
    public class RoleViewModel : NavigationMasterViewModel
    {
        private readonly IRoleAppService appService;

        public RoleListModel SelectedItem => Map<RoleListModel>(dataPager.SelectedItem);

        public RoleViewModel(IRoleAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetRoles(new GetRolesInput()), dataPager.SetList);
            });
        } 
    }
}