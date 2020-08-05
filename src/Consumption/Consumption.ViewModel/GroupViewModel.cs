/*
*
* 文件名    ：GroupViewModel                             
* 程序说明  : 用户组
* 更新时间  : 2020-08-04 17：32
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/


namespace Consumption.ViewModel
{
    using Consumption.Common.Contract;
    using Consumption.Core.Entity;
    using Consumption.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using Core.Query;
    using Consumption.Core.Common;
    using System.Linq;
    using Consumption.ViewModel.Common;

    /// <summary>
    /// 部门管理
    /// </summary>
    public class GroupViewModel : BaseDataViewModel<Group>
    {
        private readonly IConsumptionService service;
        public GroupViewModel()
        {
            SelectPageTitle = "部门管理";
            NetCoreProvider.Get(out service);
        }

        private ObservableCollection<MenuModuleGroup> menuModules;

        /// <summary>
        /// 所有的菜单模块及对应的功能
        /// </summary>
        public ObservableCollection<MenuModuleGroup> MenuModules
        {
            get { return menuModules; }
            set { menuModules = value; RaisePropertyChanged(); }
        }

        public override void Excute(string arg)
        {
            switch (arg)
            {
                case "添加用户": break;
                case "选中所有功能": break;
            }
            base.Excute(arg);
        }

        public override async Task GetPageData(int pageIndex)
        {
            try
            {
                var r = await service.GetGroupListAsync(new QueryParameters()
                {
                    PageIndex = PageIndex,
                    PageSize = PageSize,
                    Search = SearchText
                });
                if (r != null && r.success)
                {
                    GridModelList = new ObservableCollection<Group>();
                    this.TotalCount = r.dynamicObj.TotalCount;
                    r.dynamicObj.Items?.ToList().ForEach(arg =>
                    {
                        GridModelList.Add(arg);
                    });
                    base.SetPageCount();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        public override void Add()
        {
            base.Add();
        }

        public override async void Edit()
        {
            try
            {
                if (GridModel == null) return;
                UpdateLoading(true, "正在查询...");
                var tm = await service.GetMenuModuleListAsync();
                if (!tm.success)
                {
                    Msg.Warning(tm.message);
                    return;
                }
                if (tm.success)
                {
                    MenuModules = new ObservableCollection<MenuModuleGroup>();
                    tm.dynamicObj?.ForEach(arg =>
                    {
                        MenuModules.Add(arg);
                    });
                }
                var g = await service.GetGroupAsync(GridModel.Id);
                if (!g.success)
                {
                    Msg.Warning(tm.message);
                    return;
                }
                //其实这一步操作就是把当前用户组包含的权限,绑定到所有菜单的列表当中,设定选中
                g.dynamicObj.GroupFuncs?.ForEach(f =>
                {
                    for (int i = 0; i < MenuModules.Count; i++)
                    {
                        var m = MenuModules[i];
                        if (m.MenuCode == f.MenuCode)
                        {
                            for (int j = 0; j < m.Modules.Count; j++)
                            {
                                if ((f.Auth & m.Modules[j].Value) == m.Modules[j].Value)
                                    m.Modules[j].IsChecked = true;
                            }
                        }
                    }
                });
                //绑定编辑项
                GridModel = g.dynamicObj;
                SelectPageTitle = "编辑组信息";
                SelectPageIndex = 1;
                base.Edit();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        public override void Save()
        {
            SelectPageTitle = "部门管理";
            base.Save();
        }

        public override void Cancel()
        {
            SelectPageTitle = "部门管理";
            base.Cancel();
        }
    }
}
