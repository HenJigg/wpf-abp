using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Common;
using AppFramework.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFramework.Common.Services.Permission;
using Abp.Application.Services.Dto;
using Prism.Services.Dialogs;
using AppFramework.Common.Services.Account;
using Prism.Commands;
using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Authorization.Permissions;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Roles.Dto;
using System.Collections.ObjectModel;
using System;

namespace AppFramework.ViewModels
{
    public class UserViewModel : NavigationCurdViewModel
    {
        private readonly IUserAppService appService;
        private readonly IRoleAppService roleAppService;
        private readonly IAccountService accountService;
        private readonly IProfileAppService profileAppService;
        private readonly IPermissionAppService permissionAppService;

        public UserListModel SelectedItem => Map<UserListModel>(dataPager.SelectedItem);

        public UserViewModel(IUserAppService appService,
            IRoleAppService roleAppService,
            IAccountService accountService,
            IProfileAppService profileAppService,
            IPermissionAppService permissionAppService)
        {
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
            this.profileAppService = profileAppService;
            this.permissionAppService = permissionAppService;

            AdvancedCommand = new DelegateCommand(() => { IsAdvancedFilter = !IsAdvancedFilter; });
            SelectedCommand = new DelegateCommand(SelectedPermission);
            SearchCommand = new DelegateCommand(SearchUser);
            ResetCommand = new DelegateCommand(Reset);
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
                GetUserPermissionsForEditOutput output = null;
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() =>
                    appService.GetUserPermissionsForEdit(new EntityDto<long>(item.Id)),
                    async result =>
                    {
                        output = result;
                        await Task.CompletedTask;
                    });
                });

                if (output == null) return;

                DialogParameters param = new DialogParameters();
                param.Add("Id", item.Id);
                param.Add("Value", output);
                await dialog.ShowDialogAsync(AppViewManager.UserChangePermission, param);
            }
        }

        private async void UsersUnlock()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.UnlockUser(
                    new EntityDto<long>(SelectedItem.Id)));
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
                    await WebRequest.Execute(() => appService.DeleteUser(
                        new EntityDto<long>(SelectedItem.Id)),
                        RefreshAsync);
                });
            }
        }

        #endregion

        #region 条件高级筛选

        #region 字段/属性

        public GetUsersInput input { get; set; }
        public DelegateCommand AdvancedCommand { get; private set; }
        public DelegateCommand SelectedCommand { get; private set; }
        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand ResetCommand { get; private set; }

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
                RaisePropertyChanged();
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
                RaisePropertyChanged();
                SearchUser();
            }
        }

        /// <summary>
        /// 筛选标题文本: 收缩/展开
        /// </summary>
        public string FilerTitle
        {
            get { return filterTitle; }
            set { filterTitle = value; RaisePropertyChanged(); }
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
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 已选择权限的文本
        /// </summary>
        public string SelectPermissions
        {
            get { return selectPermissions; }
            set { selectPermissions = value; RaisePropertyChanged(); }
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

                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 绑定角色列表
        /// </summary>
        public ObservableCollection<RoleListModel> RoleList
        {
            get { return roleList; }
            set { roleList = value; RaisePropertyChanged(); }
        }

        #endregion

        /// <summary>
        /// 重置筛选条件
        /// </summary>
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
        private async void SelectedPermission()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", flatPermission);
            var dialogResult = await dialog.ShowDialogAsync(AppViewManager.SelectedPermission, param);
            if (dialogResult.Result == Prism.Services.Dialogs.ButtonResult.OK)
            {
                var selectedPermissions = dialogResult.Parameters.GetValue<List<string>>("Value");

                input.Permissions = selectedPermissions;
                UpdateTitle(selectedPermissions.Count);
                await RefreshAsync();
            }
        }

        /// <summary>
        /// 获取筛选权限列表
        /// </summary>
        /// <returns></returns>
        private async Task GetAllPermission()
        {
            if (flatPermission != null) return;

            await WebRequest.Execute(() => permissionAppService.GetAllPermissions(),
                        result =>
                        {
                            flatPermission = result;
                            return Task.CompletedTask;
                        });
        }

        /// <summary>
        /// 获取可选角色列表
        /// </summary>
        /// <returns></returns>
        private async Task GetAllRoles()
        {
            if (RoleList.Count > 0) return;

            await WebRequest.Execute(() => roleAppService.GetRoles(new GetRolesInput()),
                          result =>
                          {
                              foreach (var item in Map<List<RoleListModel>>(result.Items))
                                  RoleList.Add(item);

                              return Task.CompletedTask;
                          });
        }

        #endregion

        /// <summary>
        /// 搜索用户
        /// </summary>
        public void SearchUser()
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
            await WebRequest.Execute(() => appService.GetUsers(filter),
                        async result =>
                        {
                            dataPager.SetList(result);

                            await Task.CompletedTask;
                        });
        }

        /// <summary>
        /// 刷新用户列表模块
        /// </summary>
        /// <returns></returns>
        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await GetAllRoles();
                await GetAllPermission();
                await GetUsers(input);
            });
        }

        public override PermissionItem[] GetDefaultPermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(Permkeys.Users, Local.Localize("LoginAsThisUser"),()=>LoginAsThisUser()),
                new PermissionItem(Permkeys.UserEdit, Local.Localize("Change"),()=>Edit()),
                new PermissionItem(Permkeys.UserChangePermission, Local.Localize("Permissions"),()=>UserChangePermission()),
                new PermissionItem(Permkeys.UsersUnlock, Local.Localize("Unlock"),()=>UsersUnlock()),
                new PermissionItem(Permkeys.UserDelete, Local.Localize("Delete"),()=>Delete())
            };
        }
    }
}