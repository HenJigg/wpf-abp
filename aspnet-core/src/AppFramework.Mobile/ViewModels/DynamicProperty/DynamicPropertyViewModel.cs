using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using System.Threading.Tasks; 

namespace AppFramework.Shared.ViewModels
{
    public class DynamicPropertyViewModel : NavigationMasterViewModel
    {
        private readonly IDynamicPropertyAppService appService;

        public DynamicPropertyDto SelectedItem => Map<DynamicPropertyDto>(dataPager.SelectedItem);

        public DynamicPropertyViewModel(IDynamicPropertyAppService appService)
        {
            this.appService = appService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetAll(), dataPager.SetList);
            });
        } 
    }
}