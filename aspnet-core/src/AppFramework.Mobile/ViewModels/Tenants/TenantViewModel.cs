using Abp.Application.Services.Dto;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto; 
using System.Threading.Tasks; 
using AppFramework.Shared.Models;

namespace AppFramework.Shared.ViewModels
{
    public class TenantViewModel : NavigationMasterViewModel
    {
        private readonly ITenantAppService appService;
        private GetTenantsInput filter;

        public TenantListModel SelectedItem => Map<TenantListModel>(dataPager.SelectedItem);

        public TenantViewModel(ITenantAppService appService)
        {
            filter = new GetTenantsInput()
            {
                EditionIdSpecified = false,
                MaxResultCount = 10,
                SkipCount = 0,
            };
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetTenants(filter), dataPager.SetList);
            });
        } 
    }
}