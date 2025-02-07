using AppFramework.Localization;
using System.Threading.Tasks; 
using AppFramework.Shared.Models;

namespace AppFramework.Shared.ViewModels
{
    public class LanguageViewModel : NavigationMasterViewModel
    {
        private readonly ILanguageAppService appService;

        public LanguageListModel SelectedItem => Map<LanguageListModel>(dataPager.SelectedItem);

        public LanguageViewModel(ILanguageAppService languageAppService)
        {
            this.appService = languageAppService;
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetLanguages(), dataPager.SetList);
            });
        } 
    }
}