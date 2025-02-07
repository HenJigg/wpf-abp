using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Shared;
using AppFramework.Admin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Shared.Services.Permission;
using Abp.Application.Services.Dto;
using Prism.Services.Dialogs;
using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Authorization.Permissions;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using System.Collections.ObjectModel;
using Prism.Regions;
using AppFramework.Shared.Services;
using AppFramework.Admin.Services;
using CommunityToolkit.Mvvm.Input;

namespace AppFramework.Admin.ViewModels
{
    public partial class UserViewModel : NavigationCurdViewModel
    {
        private readonly IUserAppService appService;
        private readonly IRoleAppService roleAppService;
        private readonly IAccountService accountService;
        private readonly IPermissionAppService permissionAppService;

        public UserListModel SelectedItem => Map<UserListModel>(dataPager.SelectedItem);

        public UserViewModel(IUserAppService appService,
            IRoleAppService roleAppService,
            IAccountService accountService,
            IPermissionAppService permissionAppService)
        {
            Title = Local.Localize("UserManagement");
            IsAdvancedFilter = false;
            input = new GetUsersInput
            {
                Filter = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };
            roleList = new ObservableCollection<RoleListModel>();
            this.appService = appService;
            this.roleAppService = roleAppService;
            this.accountService = accountService;
            this.permissionAppService = permissionAppService;

            UpdateTitle();

            dataPager.OnPageIndexChangedEventhandler += UsersOnPageIndexChangedEventhandler;
        }

        private async void UsersOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
            {
                await GetUsers(input);
            });
        }

        #region 修改权限/解锁/使用当前账户登录

        private async void UserChangePermission()
        {
            if (dataPager.SelectedItem is IEntityDto<long> item)
            {
                GetUserPermissionsForEditOutput? output = null;
                await SetBusyAsync(async () =>
                {
                    await appService.GetUserPermissionsForEdit(new EntityDto<long>(item.Id)).WebAsync(async result =>
                     {
                         output = result;
                         await Task.CompletedTask;
                     });
                });

                if (output == null) return;

                DialogParameters param = new DialogParameters();
                param.Add("Id", item.Id);
                param.Add("Value", output);
                await dialog.ShowDialogAsync(AppViews.UserChangePermission, param);
            }
        }

        private async void UsersUnlock()
        {
            await SetBusyAsync(async () =>
            {
                await appService.UnlockUser(new EntityDto<long>(SelectedItem.Id)).WebAsync();
            });
        }

        private async void LoginAsThisUser()
        { 
            await accountService.LoginCurrentUserAsync(SelectedItem);
        }

        public async void Delete()
        {
            if (await dialog.Question(Local.Localize("UserDeleteWarningMessage", SelectedItem.UserName)))
            {
                await SetBusyAsync(async () =>
                {
                    await appService.DeleteUser(new EntityDto<long>(SelectedItem.Id))
                     .WebAsync(async () => await OnNavigatedToAsync());
                });
            }
        }

        #endregion

        #region 条件高级筛选

        #region 字段/属性

        public GetUsersInput input { get; set; }

        /// <summary>
        /// 仅锁定用户
        /// </summary>
        public bool IsLockUser
        {
            get { return isLockUser; }
            set
            {
                isLockUser = value;
                //更改查询条件
                input.OnlyLockedUsers = value;
                OnPropertyChanged();
            }
        }

        private bool isLockUser;
        private bool isAdvancedFilter;
        private string filterTitle = string.Empty;

        private string selectPermissions = string.Empty;
        private RoleListModel selectedRole;
        private ObservableCollection<RoleListModel> roleList;
        private ListResultDto<FlatPermissionWithLevelDto> flatPermission;

        public string FilterText
        {
            get { return input.Filter; }
            set
            {
                input.Filter = value;
                OnPropertyChanged();
                Search();
            }
        }

        /// <summary>
        /// 筛选标题文本: 收缩/展开
        /// </summary>
        public string FilerTitle
        {
            get { return filterTitle; }
            set { filterTitle = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 高级筛选
        /// </summary>
        public bool IsAdvancedFilter
        {
            get { return isAdvancedFilter; }
            set
            {
                isAdvancedFilter = value;

                FilerTitle = value ? "△ " + Local.Localize("HideAdvancedFilters") : "▽ " + Local.Localize("ShowAdvancedFilters");
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 已选择权限的文本
        /// </summary>
        public string SelectPermissions
        {
            get { return selectPermissions; }
            set { selectPermissions = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 选中角色
        /// </summary>
        public RoleListModel SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;

                //设置角色筛选条件
                if (value != null)
                    input.Role = value.Id;
                else
                    input.Role = null;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 绑定角色列表
        /// </summary>
        public ObservableCollection<RoleListModel> RoleList
        {
            get { return roleList; }
            set { roleList = value; OnPropertyChanged(); }
        }

        #endregion

        [RelayCommand]
        private void Advanced() => IsAdvancedFilter = !IsAdvancedFilter;

        /// <summary>
        /// 重置筛选条件
        /// </summary>
        [RelayCommand]
        private void Reset()
        {
            SelectedRole = null;
            FilterText = string.Empty;

            input.Permissions?.Clear();
            UpdateTitle(0);
        }

        /// <summary>
        /// 更新选中的权限筛选文本
        /// </summary>
        /// <param name="count"></param>
        private void UpdateTitle(int count = 0)
        {
            SelectPermissions = Local.Localize("SelectPermissions") + $"({count})";
        }

        /// <summary>
        /// 选择权限
        /// </summary>
        [RelayCommand]
        private async void Selected()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", flatPermission);
            var dialogResult = await dialog.ShowDialogAsync(AppViews.SelectedPermission, param);
            if (dialogResult.Result == Prism.Services.Dialogs.ButtonResult.OK)
            {
                var selectedPermissions = dialogResult.Parameters.GetValue<List<string>>("Value");

                input.Permissions = selectedPermissions;
                UpdateTitle(selectedPermissions.Count);
                await OnNavigatedToAsync();
            }
        }

        /// <summary>
        /// 获取筛选权限列表
        /// </summary>
        /// <returns></returns>
        private async Task GetAllPermission()
        {
            if (flatPermission != null) return;

            await permissionAppService.GetAllPermissions().WebAsync(async result =>
             {
                 flatPermission = result;
                 await Task.CompletedTask;
             });
        }

        /// <summary>
        /// 获取可选角色列表
        /// </summary>
        /// <returns></returns>
        private async Task GetAllRoles()
        {
            if (RoleList.Count > 0) return;

            await roleAppService.GetRoles(new GetRolesInput()).WebAsync(async result =>
             {
                 foreach (var item in Map<List<RoleListModel>>(result.Items))
                     RoleList.Add(item);

                 await Task.CompletedTask;
             });
        }

        #endregion

        /// <summary>
        /// 搜索用户
        /// </summary>
        [RelayCommand]
        public void Search()
        {
            dataPager.PageIndex = 0;
        }

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task GetUsers(GetUsersInput filter)
        {
            await appService.GetUsers(filter).WebAsync(dataPager.SetList);
        }

        /// <summary>
        /// 刷新用户列表模块
        /// </summary>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await SetBusyAsync(async () =>
            {
                await GetAllRoles();
                await GetAllPermission();
                await GetUsers(input);
            });
        }

        public override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.Users, Local.Localize("LoginAsThisUser"),LoginAsThisUser),
                new PermissionItem(AppPermissions.UserEdit, Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.UserChangePermission, Local.Localize("Permissions"),UserChangePermission),
                new PermissionItem(AppPermissions.UsersUnlock, Local.Localize("Unlock"),UsersUnlock),
                new PermissionItem(AppPermissions.UserDelete, Local.Localize("Delete"),Delete)
            };
        }
    }
}