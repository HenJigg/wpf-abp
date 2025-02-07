using Abp.Application.Services.Dto; 
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using System.Threading.Tasks;
using AppFramework.Shared.Services.Permission;

namespace AppFramework.Shared.ViewModels
{
    public class EditionViewModel : NavigationMasterViewModel
    {
        private readonly IEditionAppService appService;
         
        public EditionViewModel(IEditionAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetEditions(), dataPager.SetList);
            });
        } 
    }
}