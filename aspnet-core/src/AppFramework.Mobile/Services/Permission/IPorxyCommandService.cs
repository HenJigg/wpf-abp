using AppFramework.Shared.Services.Permission;
using System; 
using System.Collections.ObjectModel; 

namespace AppFramework.Shared
{
    public interface IPorxyCommandService
    {
        /// <summary>
        /// 可用的权限列表
        /// </summary>
        ObservableCollection<PermissionItem> Permissions { get; }

        /// <summary>
        /// 执行特定权限绑定的功能
        /// </summary>
        /// <param name="key"></param>
        void Execute(string key);

        /// <summary>
        /// 根据提供的权限清单生成可用权限
        /// </summary>
        /// <param name="items">权限清单</param>
        /// <exception cref="ArgumentNullException">权限清单不允许为NULL</exception>
        void Generate(PermissionItem[] items);
    }
}
