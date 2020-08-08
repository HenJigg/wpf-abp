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
    using GalaSoft.MvvmLight.Command;
    using Org.BouncyCastle.Crypto.Engines;
    using Consumption.Core.RequestForm;

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
            AddUserCommand = new RelayCommand<User>(arg =>
              {
                  if (arg == null) return;
                  var u = GroupHeader.GroupUsers?.FirstOrDefault(t => t.Account == arg.Account);
                  if (u == null) GroupHeader.GroupUsers?.Add(new GroupUser() { Account = arg.Account });
              });
            DelUserCommand = new RelayCommand<GroupUser>(arg =>
              {
                  if (arg == null) return;
                  var u = GroupHeader.GroupUsers?.FirstOrDefault(t => t.Account == arg.Account);
                  if (u != null) GroupHeader.GroupUsers?.Remove(u);
              });
        }

        #region Property


        private int selectCardIndex = 0;

        /// <summary>
        /// 切换检索用户列表的页索引
        /// </summary>
        public int SelectCardIndex
        {
            get { return selectCardIndex; }
            set { selectCardIndex = value; RaisePropertyChanged(); }
        }


        private string userSearch = string.Empty;

        /// <summary>
        /// 检索用户条件
        /// </summary>
        public string UserSearch
        {
            get { return userSearch; }
            set { userSearch = value; RaisePropertyChanged(); }
        }

        private GroupHeader groupHeader;

        /// <summary>
        /// 操作实体
        /// </summary>
        public GroupHeader GroupHeader
        {
            get { return groupHeader; }
            set { groupHeader = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<User> gridUserModelList;

        /// <summary>
        /// 所有的用户列表
        /// </summary>
        public ObservableCollection<User> GridUserModelList
        {
            get { return gridUserModelList; }
            set { gridUserModelList = value; RaisePropertyChanged(); }
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

        #endregion

        #region Command

        public RelayCommand<User> AddUserCommand { get; private set; }
        public RelayCommand<GroupUser> DelUserCommand { get; private set; }

        #endregion

        public override void Execute(string arg)
        {
            switch (arg)
            {
                case "添加用户": GetUserData(); break;
                case "选中所有功能": break;
                case "返回上一页": SelectCardIndex = 0; break;
                case "添加所有选中项": AddAllUser(); break;
                case "删除所有选中用户": DeleteAllUser(); break;
            }
            base.Execute(arg);
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

        public override async void Add()
        {
            try
            {
                UpdateLoading(true, "正在加载...");
                GroupHeader = new GroupHeader();
                await UpdateMenuModules();
                SelectPageTitle = "编辑组信息";
                SelectPageIndex = 1;
                base.Add();
            }
            catch (Exception ex)
            {
                await Msg.Error(ex.Message);
                Log.Error(ex.Message);
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        public override async void Edit()
        {
            try
            {
                if (GridModel == null) return;
                UpdateLoading(true, "正在查询...");
                await UpdateMenuModules();
                var g = await service.GetGroupAsync(GridModel.Id);
                if (!g.success)
                {
                    await Msg.Warning("获取数据异常!");
                    return;
                }
                //其实这一步操作就是把当前用户组包含的权限,
                //绑定到所有菜单的列表当中,设定选中
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
                GroupHeader = g.dynamicObj;//绑定编辑项GroupHeader
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

        public override async void Save()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(GroupHeader.group.GroupCode) ||
                    string.IsNullOrWhiteSpace(GroupHeader.group.GroupName))
                {
                    await Msg.Warning("组代码和名称为必填项！");
                    return;
                };

                //把选择的功能对应的权限保存到提交的参数当中
                GroupHeader.GroupFuncs = new List<GroupFunc>();
                for (int i = 0; i < MenuModules.Count; i++)
                {
                    var m = MenuModules[i];
                    int value = m.Modules.Where(t => t.IsChecked).Sum(t => t.Value);
                    if (value > 0)
                    {
                        GroupHeader.GroupFuncs.Add(new GroupFunc()
                        {
                            MenuCode = m.MenuCode,
                            Auth = value
                        });
                    }
                }
                UpdateLoading(true, "正在保存...");
                var r = await service.SaveGroupAsync(GroupHeader);
                if (r == null || !r.success)
                {
                    await Msg.Error("保存数据异常！");
                    return;
                }
                await this.GetPageData(0);
                SelectPageTitle = "部门管理";
                base.Save();
            }
            catch (Exception ex)
            {
                await Msg.Error(ex.Message);
                Log.Error(ex.Message);
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        public override void Cancel()
        {
            SelectPageTitle = "部门管理";
            base.Cancel();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        async void GetUserData()
        {
            try
            {
                UpdateLoading(true, "获取用户列表...");
                var r = await service.GetUserListAsync(new UserParameters()
                {
                    PageIndex = 0,
                    PageSize = 30,
                    Search = UserSearch
                });
                GridUserModelList = new ObservableCollection<User>();
                if (r.success)
                {
                    r.dynamicObj.Items?.ToList().ForEach(arg =>
                    {
                        GridUserModelList.Add(arg);
                    });
                }
                SelectCardIndex = 1;
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

        /// <summary>
        /// 添加所有选中用户
        /// </summary>
        void AddAllUser()
        {
            for (int i = 0; i < GridUserModelList.Count; i++)
            {
                var arg = GridUserModelList[i];
                if (arg.IsChecked)
                {
                    var u = GroupHeader.GroupUsers?.FirstOrDefault(t => t.Account == arg.Account);
                    if (u == null) GroupHeader.GroupUsers?.Add(new GroupUser() { Account = arg.Account });
                }
            }
        }

        /// <summary>
        /// 删除所有用户
        /// </summary>
        void DeleteAllUser()
        {
            for (int i = GroupHeader.GroupUsers.Count - 1; i >= 0; i--)
            {
                var arg = GroupHeader.GroupUsers[i];
                if (arg.IsChecked)
                    GroupHeader.GroupUsers.Remove(arg);
            }
        }

        /// <summary>
        /// 刷新菜单列表
        /// </summary>
        /// <returns></returns>
        async Task UpdateMenuModules()
        {
            if (MenuModules != null && MenuModules.Count > 0)
            {
                for (int i = 0; i < MenuModules.Count; i++)
                {
                    var m = MenuModules[i].Modules;
                    for (int j = 0; j < m.Count; j++)
                        m[j].IsChecked = false;
                }
                return;
            }
            var tm = await service.GetMenuModuleListAsync();
            if (tm != null && tm.success)
            {
                MenuModules = new ObservableCollection<MenuModuleGroup>();
                tm.dynamicObj?.ForEach(arg =>
                {
                    MenuModules.Add(arg);
                });
            }
        }
    }
}
