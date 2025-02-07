using Abp.Application.Services.Dto;
using AppFramework.Shared;
using AppFramework.Admin.Models;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Ioc;
using System.Threading.Tasks;
using Prism.Regions;
using AppFramework.Shared.Services;
using CommunityToolkit.Mvvm.Input;

namespace AppFramework.Admin.ViewModels
{
    public partial class OrganizationsViewModel : NavigationCurdViewModel
    {
        public OrganizationsViewModel(IOrganizationUnitAppService userAppService)
        {
            Title = Local.Localize("OrganizationUnits");
            this.appService = userAppService;

            usersInput = new GetOrganizationUnitUsersInput();
            rolesInput = new GetOrganizationUnitRolesInput();
            roledataPager = ContainerLocator.Container.Resolve<IDataPagerService>();
            roledataPager.OnPageIndexChangedEventhandler += RoleOnPageIndexChangedEventhandler;
            memberdataPager = ContainerLocator.Container.Resolve<IDataPagerService>();
            memberdataPager.OnPageIndexChangedEventhandler += MemberdataPager_OnPageIndexChangedEventhandler;

            ExecuteItemCommand = new DelegateCommand<string>(ExecuteItem);
        }

        private async void MemberdataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            if (usersInput.Id == 0) return;

            usersInput.SkipCount = e.SkipCount;
            usersInput.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
             {
                 await GetOrganizationUnitUsers(usersInput);
             });
        }

        private async void RoleOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            if (rolesInput.Id == 0) return;

            rolesInput.SkipCount = e.SkipCount;
            rolesInput.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
            {
                await GetOrganizationUnitRoles(rolesInput);
            });
        }

        #region 字段/属性

        private OrganizationListModel SelectedOrganizationUnit;

        private readonly IOrganizationUnitAppService appService;
        public IDataPagerService roledataPager { get; private set; }
        public IDataPagerService memberdataPager { get; private set; }
        private GetOrganizationUnitUsersInput usersInput;
        private GetOrganizationUnitRolesInput rolesInput;

        //选中组织、添加跟组织、修改、删除组织
        public DelegateCommand<string> ExecuteItemCommand { get; private set; }

        #endregion

        #region 组织机构

        /// <summary>
        /// 选中组织机构-更新成员和角色信息
        /// </summary>
        /// <param name="organizationUnit"></param>
        [RelayCommand]
        private async void Selected(OrganizationListModel organizationUnit)
        {
            if (organizationUnit == null) return;

            SelectedOrganizationUnit = organizationUnit;
            rolesInput.Id = SelectedOrganizationUnit.Id;
            usersInput.Id = SelectedOrganizationUnit.Id;

            await GetOrganizationUnitUsers(usersInput);
            await GetOrganizationUnitRoles(rolesInput);
        }

        /// <summary>
        /// UI的按钮命令
        /// 说明: 添加组织、添加成员、添加角色、刷新组织机构树
        /// </summary>
        /// <param name="arg"></param>
        public async void ExecuteItem(string arg)
        {
            switch (arg)
            {
                case "AddOrganizationUnit": AddRootUnit(); break;
                case "AddMember": await AddMember(SelectedOrganizationUnit); break;
                case "AddRole": await AddRole(SelectedOrganizationUnit); break;
                case "Refresh": await OnNavigatedToAsync(); break;
            }
        }

        /// <summary>
        /// 刷新组织结构树
        /// </summary>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await SetBusyAsync(async () =>
             {
                 await appService.GetOrganizationUnits().WebAsync(async result =>
                      {
                          dataPager.GridModelList.Clear();
                          var items = BuildOrganizationTree(Map<List<OrganizationListModel>>(result.Items));

                          foreach (var item in items)
                              dataPager.GridModelList.Add(item);

                          await Task.CompletedTask;
                      });
             });
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="organization"></param>
        [RelayCommand]
        public async void Remove(OrganizationListModel organization)
        {
            if (await dialog.Question(Local.Localize("OrganizationUnitDeleteWarningMessage", organization.DisplayName)))
            {
                await appService.DeleteOrganizationUnit(new EntityDto<long>() { Id = organization.Id })
                    .WebAsync(async () => await OnNavigatedToAsync());
            }
        }

        /// <summary>
        /// 编辑组织机构
        /// </summary>
        /// <param name="organization"></param>
        [RelayCommand]
        public async void Change(OrganizationListModel organization)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", organization);
            var dialogResult = await dialog.ShowDialogAsync("OrganizationsAddView", param);
            if (dialogResult.Result == ButtonResult.OK)
                await OnNavigatedToAsync();
        }

        /// <summary>
        /// 新增组织机构
        /// </summary>
        /// <param name="organization"></param>
        [RelayCommand]
        public async void AddRootUnit(OrganizationListModel organization = null)
        {
            DialogParameters param = new DialogParameters();
            if (organization != null) param.Add("ParentId", organization.Id);

            var dialogResult = await dialog.ShowDialogAsync("OrganizationsAddView", param);
            if (dialogResult.Result == ButtonResult.OK)
                await OnNavigatedToAsync();
        }

        /// <summary>
        /// 更新组织机构显示信息
        /// </summary>
        /// <param name="id"></param>
        private void UpdateOrganizationUnit(long id)
        {
            var organizationUnit = dataPager.GridModelList
                .FirstOrDefault(t => t is OrganizationListModel q && q.Id.Equals(id)) as OrganizationListModel;

            if (organizationUnit != null)
            {
                organizationUnit.MemberCount = memberdataPager.GridModelList.Count;
                organizationUnit.RoleCount = roledataPager.GridModelList.Count;
            }
        }

        public ObservableCollection<object> BuildOrganizationTree(
            List<OrganizationListModel> organizationUnits, long? parentId = null)
        {
            var masters = organizationUnits
                .Where(x => x.ParentId == parentId).ToList();

            var childs = organizationUnits
                .Where(x => x.ParentId != parentId).ToList();

            foreach (OrganizationListModel dpt in masters)
                dpt.Items = BuildOrganizationTree(childs, dpt.Id);

            return new ObservableCollection<object>(masters);
        }

        #endregion 

        #region 角色

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="organizationUnit"></param>
        /// <returns></returns>
        private async Task AddRole(OrganizationListModel organizationUnit)
        {
            if (organizationUnit == null) return;

            long Id = organizationUnit.Id;

            await appService.FindRoles(new FindOrganizationUnitRolesInput() { OrganizationUnitId = Id })
                .WebAsync(async result =>
                {
                    DialogParameters param = new DialogParameters();
                    param.Add("Id", Id);
                    param.Add("Value", result);
                    var dialogResult = await dialog.ShowDialogAsync(AppViews.AddRoles, param); if (dialogResult.Result == ButtonResult.OK)
                    {
                        rolesInput.Id = Id;
                        await GetOrganizationUnitRoles(rolesInput);
                    }
                });
        }

        /// <summary>
        /// 刷新角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private async Task GetOrganizationUnitRoles(GetOrganizationUnitRolesInput input)
        {
            await SetBusyAsync(async () =>
              {
                  var pagedResult = await appService.GetOrganizationUnitRoles(input);
                  if (pagedResult != null)
                      await roledataPager.SetList(pagedResult);

                  UpdateOrganizationUnit(input.Id);
              });
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="obj"></param>
        [RelayCommand]
        private async void DeleteRole(OrganizationUnitRoleListDto obj)
        {
            if (await dialog.Question(Local.Localize("RemoveRoleFromOuWarningMessage",
                SelectedOrganizationUnit.DisplayName, obj.DisplayName)))
            {
                await SetBusyAsync(async () =>
                {
                    await appService.RemoveRoleFromOrganizationUnit(new RoleToOrganizationUnitInput()
                    { RoleId = (int)obj.Id, OrganizationUnitId = SelectedOrganizationUnit.Id })
                    .WebAsync(async () =>
                    {
                        rolesInput.Id = SelectedOrganizationUnit.Id;
                        await GetOrganizationUnitRoles(rolesInput);
                    });
                });
            }
        }

        #endregion 

        #region 成员

        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="organizationUnit"></param>
        /// <returns></returns>
        private async Task AddMember(OrganizationListModel organizationUnit)
        {
            if (organizationUnit == null) return;

            long Id = organizationUnit.Id;

            await appService.FindUsers(new FindOrganizationUnitUsersInput() { OrganizationUnitId = Id }).WebAsync(async result =>
            {
                DialogParameters param = new DialogParameters();
                param.Add("Id", Id);
                param.Add("Value", result);
                var dialogResult = await dialog.ShowDialogAsync(AppViews.AddUsers, param);
                if (dialogResult.Result == ButtonResult.OK)
                    await GetOrganizationUnitUsers(usersInput);
            });
        }

        /// <summary>
        /// 刷新成员
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private async Task GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input)
        {
            await SetBusyAsync(async () =>
            {
                var pagedResult = await appService.GetOrganizationUnitUsers(input);
                if (pagedResult != null)
                    await memberdataPager.SetList(pagedResult);

                UpdateOrganizationUnit(input.Id);
            });
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="obj"></param>
        [RelayCommand]
        private async void DeleteMember(OrganizationUnitUserListDto obj)
        {
            if (await dialog.Question(Local.Localize("RemoveUserFromOuWarningMessage",
                SelectedOrganizationUnit.DisplayName, obj.UserName)))
            {
                await SetBusyAsync(async () =>
                {
                    await appService.RemoveUserFromOrganizationUnit(new UserToOrganizationUnitInput()
                    {
                        OrganizationUnitId = SelectedOrganizationUnit.Id,
                        UserId = obj.Id
                    }).WebAsync(async () =>
                    {
                        await GetOrganizationUnitUsers(usersInput);
                    });
                });
            }
        }

        #endregion  
    }
}