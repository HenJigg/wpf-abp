using AppFramework.Shared;
using AppFramework.Shared.Services.Permission;
using Prism.Mvvm;
using System; 
using System.Collections.ObjectModel;
using System.Linq;

namespace AppFramework.Admin.Services
{
    public class PermissionPorxyService : BindableBase, IPermissionPorxyService
    {
        public PermissionPorxyService(IPermissionService permissionService)
        {
            permissions = new ObservableCollection<PermissionItem>();
            this.permissionService = permissionService;
        }

        private ObservableCollection<PermissionItem> permissions;
        private readonly IPermissionService permissionService;

        public ObservableCollection<PermissionItem> Permissions
        {
            get { return permissions; }
        }

        public void Execute(string key)
        {
            var item = Permissions.FirstOrDefault(t => t.Key.Equals(key));
            if (item != null) item.Action?.Invoke();
        }
         
        public void Generate(PermissionItem[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            Permissions.Clear();

            foreach (var item in items)
            {
                if (permissionService.HasPermission(item.Key))
                    Permissions.Add(item);
            }
        }
    }
}
