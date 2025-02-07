using Abp.Application.Services.Dto;
using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Shared;
using AppFramework.Admin.Models;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public class UserDetailsViewModel : HostDialogViewModel
    {
        public UserDetailsViewModel(IUserAppService userAppService,
            IPermissionService permissionService)
        {
            Input = new UserCreateOrUpdateModel();
            this.userAppService = userAppService;
            this.permissionService = permissionService;
        }

        #region 字段/属性

        private bool isNewUser;
        private UserForEditModel model;

        private UserCreateOrUpdateModel input;

        public UserCreateOrUpdateModel Input
        {
            get { return input; }
            set { input = value; OnPropertyChanged(); }
        }

        private GetPasswordComplexitySettingOutput PasswordComplexitySetting;
        private readonly IUserAppService userAppService;
        private readonly IPermissionService permissionService;

        /// <summary>
        /// 是否是新建用户
        /// </summary>
        public bool IsNewUser
        {
            get => isNewUser;
            set
            {
                isNewUser = value;
                OnPropertyChanged();
            }
        }

        public UserForEditModel Model
        {
            get => model;
            set
            {
                model = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// 保存用户
        /// </summary>
        public override async Task Save()
        {
            Input.User = Model.User;
            Input.AssignedRoleNames = Model.Roles.Where(x => x.IsAssigned).Select(x => x.RoleName).ToArray();
            Input.OrganizationUnits = Model.OrganizationUnits.Where(x => x.IsAssigned).Select(x => x.Id).ToList();

            if (!Verify(Input).IsValid) return;

            await SetBusyAsync(async () =>
            {
                var input = Map<CreateOrUpdateUserInput>(Input);
                await userAppService.CreateOrUpdateUser(input).WebAsync(base.Save);
            }, AppLocalizationKeys.SavingWithThreeDot);
        }

        /// <summary>
        /// 窗口打开时
        /// </summary>
        /// <param name="parameters"></param>
        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
              {
                  UserListDto? user = null;
                  if (parameters.ContainsKey("Value"))
                      user = parameters.GetValue<UserListDto>("Value");

                  IsNewUser = user == null;
                  Input.SetRandomPassword = IsNewUser;
                  Input.SendActivationEmail = IsNewUser;

                  await userAppService.GetUserForEdit(new NullableIdDto<long>(user?.Id)).WebAsync(GetUserForEditSuccessed);
              });

            if (parameters.ContainsKey("Config"))
                PasswordComplexitySetting = parameters.GetValue<GetPasswordComplexitySettingOutput>("Config");
        }

        /// <summary>
        /// 设置编辑用户数据
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        private async Task GetUserForEditSuccessed(GetUserForEditOutput output)
        {
            Model = Map<UserForEditModel>(output);
            Model.OrganizationUnits = Map<List<OrganizationUnitModel>>(output.AllOrganizationUnits);

            if (IsNewUser)
            {
                //Model.Photo = ImageSource.FromResource(AssetsHelper.ProfileImagePlaceholderNamespace);
                Model.User = new UserEditModel
                {
                    IsActive = true,
                    IsLockoutEnabled = true,
                    ShouldChangePasswordOnNextLogin = true,
                };
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// 生成可选的组织树
        /// </summary>
        /// <param name="organizationUnits"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private ObservableCollection<object> BuildOrganizationTree(
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
    }
}