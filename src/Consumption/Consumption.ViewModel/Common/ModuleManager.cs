/*
*
* 文件名    ：ModuleManager                             
* 程序说明  : 定义分组模块的数据管理器结构
* 更新时间  : 2020-05-12 21:10
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.ViewModel.Common
{
    using Consumption.Shared.Common;
    using Consumption.Shared.Common.Enums;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 模块管理器
    /// </summary>
    public class ModuleManager : ObservableObject
    {
        private ObservableCollection<Module> modules;
        /// <summary>
        /// 已加载模块
        /// </summary>
        public ObservableCollection<Module> Modules
        {
            get { return modules; }
            set { modules = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ModuleGroup> moduleGroups;

        /// <summary>
        /// 已加载模块-分组
        /// </summary>
        public ObservableCollection<ModuleGroup> ModuleGroups
        {
            get { return moduleGroups; }
            set { moduleGroups = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 加载程序集模块
        /// </summary>
        /// <returns></returns>
        public async Task LoadAssemblyModule()
        {
            try
            {
                Modules = new ObservableCollection<Module>();
                ModuleGroups = new ObservableCollection<ModuleGroup>();
                var emMt = Enum.GetValues(typeof(ModuleType));
                for (int i = 0; i < emMt.Length; i++)
                    ModuleGroups.Add(new ModuleGroup() { GroupName = emMt.GetValue(i).ToString(), Modules = new ObservableCollection<Module>() });
                var ms = await new ModuleComponent().GetAssemblyModules();
                foreach (var i in ms)
                {
                    //如果当前程序集的模快在服务器上可以匹配到就添加模块列表
                    var m = Contract.Menus.FirstOrDefault(t => t.MenuName.Equals(i.Name));
                    if (m != null)
                    {
                        var group = ModuleGroups.FirstOrDefault(t => t.GroupName == i.ModuleType.ToString());
                        if (group == null)
                        {
                            ModuleGroup newgroup = new ModuleGroup();
                            newgroup.GroupName = i.ModuleType.ToString();
                            newgroup.Modules = new ObservableCollection<Module>();
                            newgroup.Modules.Add(new Module()
                            {
                                Name = i.Name,
                                Code = m.MenuCaption,
                                TypeName = m.MenuNameSpace,
                                Auth = m.MenuAuth
                            });
                            ModuleGroups.Add(newgroup);
                        }
                        else
                        {
                            group.Modules.Add(new Module()
                            {
                                Name = i.Name,
                                Code = m.MenuCaption,
                                TypeName = m.MenuNameSpace,
                                Auth = m.MenuAuth
                            });
                        }
                        Modules.Add(new Module()
                        {
                            Name = i.Name,
                            Code = m.MenuCaption,
                            TypeName = m.MenuNameSpace,
                            Auth = m.MenuAuth
                        });
                    }
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                //Log.Error(ex.Message);
                throw ex;
            }
        }
    }
}
