
namespace Consumption.ViewModel.Common
{
    using Consumption.Shared.DataModel;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// 模块管理器
    /// </summary>
    public class ModuleManager : ObservableObject
    {
        public ModuleManager()
        {
            Modules = new ObservableCollection<Module>();
        }

        private ObservableCollection<Module> modules;
        /// <summary>
        /// 已加载模块
        /// </summary>
        public ObservableCollection<Module> Modules
        {
            get { return modules; }
            set { modules = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 加载程序集模块
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public void LoadAssemblyModule(List<Menu> menus, int Id = 0)
        {
            if (menus == null || menus.Count == 0) return;

            foreach (var item in menus)
            {
                Modules.Add(new Module
                {
                    Code = item.MenuCaption,
                    Name = item.MenuName,
                    TypeName = item.MenuNameSpace,
                    Auth = item.MenuAuth,
                });
            }
        }
    }
}
