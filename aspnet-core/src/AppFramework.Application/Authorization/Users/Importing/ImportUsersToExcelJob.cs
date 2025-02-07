using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.ObjectMapping;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using AppFramework.Authorization.Roles;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Authorization.Users.Importing.Dto;
using AppFramework.Notifications;
using AppFramework.Storage;

namespace AppFramework.Authorization.Users.Importing
{
    public class ImportUsersToExcelJob : AsyncBackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IUserListExcelDataReader _userListExcelDataReader;
        private readonly IInvalidUserExporter _invalidUserExporter;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public UserManager UserManager { get; set; }

        public ImportUsersToExcelJob(
            RoleManager roleManager,
            IUserListExcelDataReader userListExcelDataReader,
            IInvalidUserExporter invalidUserExporter,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _roleManager = roleManager;
            _userListExcelDataReader = userListExcelDataReader;
            _invalidUserExporter = invalidUserExporter;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public override async Task ExecuteAsync(ImportUsersFromExcelJobArgs args)
        {
            var users = await GetUserListFromExcelOrNullAsync(args);
            if (users == null || !users.Any())
            {
                await SendInvalidExcelNotificationAsync(args);
                return;
            }

            await CreateUsersAsync(args, users);
        }

        private async Task<List<ImportUserDto>> GetUserListFromExcelOrNullAsync(ImportUsersFromExcelJobArgs args)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                {
                    try
                    {
                        var file = await _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId);
                        return _userListExcelDataReader.GetUsersFromExcel(file.Bytes);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    finally
                    {
                        await uow.CompleteAsync();
                    }
                }
            }
        }

        private async Task CreateUsersAsync(ImportUsersFromExcelJobArgs args, List<ImportUserDto> users)
        {
            var invalidUsers = new List<ImportUserDto>();

            foreach (var user in users)
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                    {
                        if (user.CanBeImported())
                        {
                            try
                            {
                                await CreateUserAsync(user);
                            }
                            catch (UserFriendlyException exception)
                            {
                                user.Exception = exception.Message;
                                invalidUsers.Add(user);
                            }
                            catch (Exception exception)
                            {
                                user.Exception = exception.ToString();
                                invalidUsers.Add(user);
                            }
                        }
                        else
                        {
                            invalidUsers.Add(user);
                        }
                    }

                    await uow.CompleteAsync();
                }
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                {
                    await ProcessImportUsersResultAsync(args, invalidUsers);
                }

                await uow.CompleteAsync();
            }
        }

        private async Task CreateUserAsync(ImportUserDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            if (tenantId.HasValue)
            {
                await _userPolicy.CheckMaxUserCountAsync(tenantId.Value);
            }

            var user = _objectMapper.Map<User>(input); //Passwords is not mapped (see mapping configuration)
            user.Password = input.Password;
            user.TenantId = tenantId;

            if (!input.Password.IsNullOrEmpty())
            {
                await UserManager.InitializeOptionsAsync(tenantId);
                foreach (var validator in _passwordValidators)
                {
                    (await validator.ValidateAsync(UserManager, user, input.Password)).CheckErrors();
                }

                user.Password = _passwordHasher.HashPassword(user, input.Password);
            }

            user.Roles = new List<UserRole>();
            var roleList = _roleManager.Roles.ToList();

            foreach (var roleName in input.AssignedRoleNames)
            {
                var correspondingRoleName = GetRoleNameFromDisplayName(roleName, roleList);
                var role = await _roleManager.GetRoleByNameAsync(correspondingRoleName);
                user.Roles.Add(new UserRole(tenantId, user.Id, role.Id));
            }

            (await UserManager.CreateAsync(user)).CheckErrors();
        }

        private async Task ProcessImportUsersResultAsync(ImportUsersFromExcelJobArgs args,
            List<ImportUserDto> invalidUsers)
        {
            if (invalidUsers.Any())
            {
                var file = _invalidUserExporter.ExportToFile(invalidUsers);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    new LocalizableString("AllUsersSuccessfullyImportedFromExcel",
                        AppFrameworkConsts.LocalizationSourceName),
                    null,
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private async Task SendInvalidExcelNotificationAsync(ImportUsersFromExcelJobArgs args)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (CurrentUnitOfWork.SetTenantId(args.TenantId))
                {
                    await _appNotifier.SendMessageAsync(
                        args.User,
                        new LocalizableString(
                            "FileCantBeConvertedToUserList",
                            AppFrameworkConsts.LocalizationSourceName
                        ),
                        null,
                        Abp.Notifications.NotificationSeverity.Warn);
                }

                await uow.CompleteAsync();
            }
        }

        private string GetRoleNameFromDisplayName(string displayName, List<Role> roleList)
        {
            return roleList.FirstOrDefault(
                r => r.DisplayName?.ToLowerInvariant() == displayName?.ToLowerInvariant()
            )?.Name;
        }
    }
}
