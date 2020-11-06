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
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Consumption.ViewModel.Common;
    using Consumption.ViewModel.Interfaces;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Common;
    using Consumption.Shared.Dto;
    using Consumption.Shared.Common.Query;
    using Newtonsoft.Json;
    using Consumption.Shared.Common.Collections;
    using Microsoft.Toolkit.Mvvm.Input;


    /// <summary>
    /// 部门管理
    /// </summary>
    public class GroupViewModel : BaseRepository<GroupDto>, IGroupViewModel
    {
        private readonly IUserRepository userRepository;
        private readonly IGroupRepository groupRepository;

        public GroupViewModel(IGroupRepository repository) : base(repository)
        {
            userRepository = NetCoreProvider.Resolve<IUserRepository>();
            AddUserCommand = new RelayCommand<UserDto>(arg =>
            {
                if (arg == null) return;
                var u = GroupDto.GroupUsers?.FirstOrDefault(t => t.Account == arg.Account);
                if (u == null) GroupDto.GroupUsers?.Add(new GroupUserDto() { Account = arg.Account });
            });
            DelUserCommand = new RelayCommand<GroupUserDto>(arg =>
            {
                if (arg == null) return;
                var u = GroupDto.GroupUsers?.FirstOrDefault(t => t.Account == arg.Account);
                if (u != null) GroupDto.GroupUsers?.Remove(u);
            });
            groupRepository = repository;
        }

        #region Override

        public override async Task Execute(string arg)
        {
            switch (arg)
            {
                case "添加用户": GetUserData(); break;
                case "选中所有功能": break;
                case "返回上一页": SelectCardIndex = 0; break;
                case "添加所有选中项": AddAllUser(); break;
                case "删除所有选中用户": DeleteAllUser(); break;
            }
            await base.Execute(arg);
        }

        public override async void AddAsync()
        {
            GroupDto = new GroupDataDto();
            await UpdateMenuModules();
            base.AddAsync();
        }

        public override async void UpdateAsync()
        {
            if (GridModel == null) return;
            await UpdateMenuModules();
            var g = await groupRepository.GetGroupAsync(GridModel.Id);
            if (g.StatusCode != 200)
            {
                Msg.Warning(g.Message);
                return;
            }
            //其实这一步操作就是把当前用户组包含的权限,
            //绑定到所有菜单的列表当中,设定选中
            g.Result?.GroupFuncs?.ForEach(f =>
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
            GroupDto = g.Result;//绑定编辑项GroupHeader
            this.CreateDeaultCommand();
            SelectPageIndex = 1;
        }

        public override async Task SaveAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(GroupDto.group.GroupCode) ||
                    string.IsNullOrWhiteSpace(GroupDto.group.GroupName))
                {
                    Msg.Warning("组代码和名称为必填项！");
                    return;
                };

                //把选择的功能对应的权限保存到提交的参数当中
                GroupDto.GroupFuncs = new List<GroupFunc>();
                for (int i = 0; i < MenuModules.Count; i++)
                {
                    var m = MenuModules[i];
                    int value = m.Modules.Where(t => t.IsChecked).Sum(t => t.Value);
                    if (value > 0)
                    {
                        GroupDto.GroupFuncs.Add(new GroupFunc()
                        {
                            MenuCode = m.MenuCode,
                            Auth = value
                        });
                    }
                }
                var r = await groupRepository.SaveGroupAsync(GroupDto);
                if (r.StatusCode != 200)
                {
                    Msg.Error(r.Message);
                    return;
                }
                await GetPageData(0);
                InitPermissions(this.AuthValue);
                SelectPageIndex = 0;
            }
            catch (Exception ex)
            {
                Msg.Error(ex.Message);
            }
        }

        #endregion

        #region Property


        private int selectCardIndex = 0;

        /// <summary>
        /// 切换检索用户列表的页索引
        /// </summary>
        public int SelectCardIndex
        {
            get { return selectCardIndex; }
            set { selectCardIndex = value; OnPropertyChanged(); }
        }


        private string userSearch = string.Empty;

        /// <summary>
        /// 检索用户条件
        /// </summary>
        public string UserSearch
        {
            get { return userSearch; }
            set { userSearch = value; OnPropertyChanged(); }
        }

        private GroupDataDto groupDto;

        /// <summary>
        /// 操作实体
        /// </summary>
        public GroupDataDto GroupDto
        {
            get { return groupDto; }
            set { groupDto = value; OnPropertyChanged(); }
        }

        private ObservableCollection<UserDto> gridUserModelList;

        /// <summary>
        /// 所有的用户列表
        /// </summary>
        public ObservableCollection<UserDto> GridUserModelList
        {
            get { return gridUserModelList; }
            set { gridUserModelList = value; OnPropertyChanged(); }
        }


        private ObservableCollection<MenuModuleGroupDto> menuModules;

        /// <summary>
        /// 所有的菜单模块及对应的功能
        /// </summary>
        public ObservableCollection<MenuModuleGroupDto> MenuModules
        {
            get { return menuModules; }
            set { menuModules = value; OnPropertyChanged(); }
        }

        #endregion

        #region Command

        public RelayCommand<UserDto> AddUserCommand { get; private set; }
        public RelayCommand<GroupUserDto> DelUserCommand { get; private set; }

        #endregion

        #region Method

        /// <summary>
        /// 获取用户列表
        /// </summary>
        async void GetUserData()
        {
            var r = await userRepository.GetAllListAsync(new QueryParameters()
            {
                PageIndex = 0,
                PageSize = 30,
                Search = UserSearch
            });
            GridUserModelList = new ObservableCollection<UserDto>();
            if (r.StatusCode == 200)
                GridUserModelList = new ObservableCollection<UserDto>(r.Result.Items?.ToList());
            SelectCardIndex = 1;
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
                    var u = GroupDto.GroupUsers?.FirstOrDefault(t => t.Account == arg.Account);
                    if (u == null) GroupDto.GroupUsers?.Add(new GroupUserDto() { Account = arg.Account });
                }
            }
        }

        /// <summary>
        /// 删除所有用户
        /// </summary>
        void DeleteAllUser()
        {
            for (int i = GroupDto.GroupUsers.Count - 1; i >= 0; i--)
            {
                var arg = GroupDto.GroupUsers[i];
                if (arg.IsChecked)
                    GroupDto.GroupUsers.Remove(arg);
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
            var tm = await groupRepository.GetMenuModuleListAsync();
            if (tm.StatusCode == 200)
                MenuModules = new ObservableCollection<MenuModuleGroupDto>(tm.Result);
        }

        #endregion
    }

}
