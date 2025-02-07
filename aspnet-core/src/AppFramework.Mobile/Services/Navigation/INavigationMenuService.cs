using AppFramework.Shared.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFramework.Shared.Services.Navigation
{
    public interface INavigationMenuService
    {
        ObservableCollection<NavigationItem> GetAuthMenus(Dictionary<string, string> permissions); 
    }
}