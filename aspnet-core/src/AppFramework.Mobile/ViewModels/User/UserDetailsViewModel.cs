using Abp.Application.Services.Dto;
using Acr.UserDialogs;
using AppFramework.Shared.Core;
using AppFramework.Shared.Models;
using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Shared.Services.Messenger;
using AppFramework.Shared.Extensions; 

namespace AppFramework.Shared.ViewModels
{
    public class UserDetailsViewModel : NavigationDetailViewModel
    {
        #region 字段/属性

        private readonly IUserAppService userAppService;
        private readonly IPermissionService permissionService;
        private readonly IMessenger messenger;

        public DelegateCommand UnlockCommand { get; private set; }
        public DelegateCommand AddUserOrRoleCommand { get; private set; }

        private UserForEditModel model;
        private UserCreateOrUpdateModel userInput; 
        private bool isNewUser;
        private bool isUnlockButtonVisible;

        public UserCreateOrUpdateModel UserInput
        {
            get => userInput;
            set
            {
                userInput = value;
                RaisePropertyChanged();
            }
        }

        public UserForEditModel Model
        {
            get => model;
            set
            {
                model = value;
                RaisePropertyChanged();
            }
        }
         
        public bool IsUnlockButtonVisible
        {
            get => isUnlockButtonVisible;
            set
            {
                isUnlockButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        public bool IsNewUser
        {
            get => isNewUser;
            set
            {
                isNewUser = value;
                IsUnlockButtonVisible = !isNewUser && permissionService.HasPermission(AppPermissions.UserEdit);
                PageTitle = isNewUser ? Local.Localize(LocalizationKeys.CreatingNewUser) : Local.Localize(LocalizationKeys.EditUser);
                RaisePropertyChanged();
            }
        }

        #endregion 字段/属性

        public UserDetailsViewModel(
            IUserAppService userAppService,
            IPermissionService permissionService,
            IMessenger messenger)
        {
            this.userAppService = userAppService;
            this.permissionService = permissionService;
            this.messenger = messenger;
            UserInput = new UserCreateOrUpdateModel();
            UnlockCommand = new DelegateCommand(UnlockUser);
            AddUserOrRoleCommand=new DelegateCommand(AddUserOrRole);
        }

        private void AddUserOrRole()
        {
            messenger.Send("SelectionUserOrRoleViewIsVisible", true);
        }

        #region 解锁/删除/保存用户

        public override async void Save()
        {
            UserInput.User = Model.User;
            UserInput.AssignedRoleNames = Model.Roles.Where(x => x.IsAssigned).Select(x => x.RoleName).ToArray();
            UserInput.OrganizationUnits = Model.OrganizationUnits.Where(x => x.IsAssigned).Select(x => x.Id).ToList();

            if (!Verify(UserInput).IsValid) return;

            await SetBusyAsync(async () =>
            {
                var input = Map<CreateOrUpdateUserInput>(UserInput);
                await WebRequest.Execute(() => userAppService.CreateOrUpdateUser(input), GoBackAsync);
            }, LocalizationKeys.SavingWithThreeDot);
        }

        private async void UnlockUser()
        {
            if (!Model.User.Id.HasValue)
                return;

            await SetBusyAsync(async () =>
            {
                await userAppService.UnlockUser(new EntityDto<long>(Model.User.Id.Value));
                await GoBackAsync();
            });
        }

        public override async void Delete()
        {
            var accepted = await UserDialogs.Instance.ConfirmAsync(
                Local.Localize(LocalizationKeys.UserDeleteWarningMessage, Model.User.UserName),
                Local.Localize(LocalizationKeys.AreYouSure),
                Local.Localize(LocalizationKeys.Ok),
                Local.Localize(LocalizationKeys.Cancel));

            if (!accepted)
                return;

            await SetBusyAsync(async () =>
            {
                if (!Model.User.Id.HasValue)
                    return;

                await userAppService.DeleteUser(new EntityDto<long>(Model.User.Id.Value));
                await GoBackAsync();
            });
        }

        public override async Task GoBackAsync()
        {
            messenger.Send(AppMessengerKeys.User); //通知更新列表
            await base.GoBackAsync();
        }

        #endregion 解锁/删除/保存用户


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                long? id = null;
                if (parameters.ContainsKey("Value"))
                    id = parameters.GetValue<UserListDto>("Value").Id;

                IsNewUser = id == null;
                UserInput.SetRandomPassword = IsNewUser;
                UserInput.SendActivationEmail = IsNewUser;

                await WebRequest.Execute(() =>
                userAppService.GetUserForEdit(new NullableIdDto<long>(id)),
                result => GetUserForEditSuccessed(result));
            });
        }

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
    }
}