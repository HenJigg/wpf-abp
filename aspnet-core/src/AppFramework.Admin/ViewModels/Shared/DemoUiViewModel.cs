using AppFramework.Shared;

namespace AppFramework.Admin.ViewModels.Shared
{
    public class DemoUiViewModel : NavigationViewModel
    {
        public DemoUiViewModel()
        {
            Title = Local.Localize("DemoUiComponents");
        }
    }
}
