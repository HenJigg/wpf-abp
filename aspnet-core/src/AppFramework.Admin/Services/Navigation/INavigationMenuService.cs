using AppFramework.Shared.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFramework.Admin.Services
{
    public interface INavigationMenuService
    {
        ObservableCollection<NavigationItem> GetAuthMenus(Dictionary<string, string> permissions); 
    }
}