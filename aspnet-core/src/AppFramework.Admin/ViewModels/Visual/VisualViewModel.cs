using AppFramework.Shared;
using AppFramework.Shared.Services;
using CommunityToolkit.Mvvm.Input;
using DryIoc;
using Prism.Commands;

namespace AppFramework.Admin.ViewModels.Shared
{
    public partial class VisualViewModel : NavigationViewModel
    {
        public VisualViewModel(IThemeService themeService)
        {
            Title = Local.Localize("VisualSettings");
            this.themeService = themeService; 
        }
          
        public IThemeService themeService { get; set; }

        [RelayCommand]
        private void SetTheme(ThemeItem themeItem)
        {
            themeService.SetTheme(themeItem.DisplayName);
        }
    }
}
