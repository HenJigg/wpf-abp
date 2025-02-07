using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.UI;
using Abp.Zero.EntityFrameworkCore;
using Castle.Core.Internal;
using Microsoft.Extensions.Configuration;
using AppFramework.Authorization;
using AppFramework.Authorization.Users;
using AppFramework.Configuration;
using AppFramework.Configuration.Dto;
using AppFramework.Configuration.Host.Dto;
using AppFramework.EntityFrameworkCore;
using AppFramework.Identity;
using AppFramework.Install.Dto;
using AppFramework.Migrations.Seed;
using AppFramework.Migrations.Seed.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppFramework.Install
{
    [AbpAllowAnonymous]
    [DisableAuditing]
    public class InstallAppService : AppFrameworkAppServiceBase, IInstallAppService
    {
        private readonly AbpZeroDbMigrator<AppFrameworkDbContext> _migrator;
        private readonly LogInManager _logInManager;
        private readonly SignInManager _signInManager;
        private readonly DatabaseCheckHelper _databaseCheckHelper;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IAppConfigurationWriter _appConfigurationWriter;

        public InstallAppService(AbpZeroDbMigrator migrator,
            LogInManager logInManager,
            SignInManager signInManager,
            DatabaseCheckHelper databaseCheckHelper,
            IAppConfigurationAccessor appConfigurationAccessor,
            IAppConfigurationWriter appConfigurationWriter)
        {
            _migrator = migrator;
            _logInManager = logInManager;
            _signInManager = signInManager;
            _databaseCheckHelper = databaseCheckHelper;
            _appConfiguration = appConfigurationAccessor.Configuration;
            _appConfigurationWriter = appConfigurationWriter;
        }

        public async Task Setup(InstallDto input)
        {
            if (CheckDatabaseInternal())
            {
                throw new UserFriendlyException("Setup process is already done.");
            }

            SetConnectionString(input.ConnectionString);

            _migrator.CreateOrMigrateForHost(SeedHelper.SeedHostDb);

            if (CheckDatabaseInternal())
            {
                await SetAdminPassword(input.AdminPassword);
                SetUrl(input.WebSiteUrl, input.ServerUrl);
                await SetDefaultLanguage(input.DefaultLanguage);
                await SetSmtpSettings(input.SmtpSettings);
                await SetBillingSettings(input.BillInfo);
            }
            else
            {
                throw new UserFriendlyException("Database couldn't be created!");
            }
        }

        [UnitOfWork(IsDisabled = true)]
        public AppSettingsJsonDto GetAppSettingsJson()
        {
            var appUrl = _appConfiguration.GetSection("App");

            if (appUrl["WebSiteRootAddress"].IsNullOrEmpty())
            {
                return new AppSettingsJsonDto
                {
                    WebSiteUrl = appUrl["ClientRootAddress"],
                    ServerSiteUrl = appUrl["ServerRootAddress"],
                    Languages = DefaultLanguagesCreator.InitialLanguages
                        .Select(l => new NameValue(l.DisplayName, l.Name)).ToList()
                };
            }

            return new AppSettingsJsonDto
            {
                WebSiteUrl = appUrl["WebSiteRootAddress"]
            };
        }

        public CheckDatabaseOutput CheckDatabase()
        {
            return new CheckDatabaseOutput
            {
                IsDatabaseExist = CheckDatabaseInternal()
            };
        }

        private bool CheckDatabaseInternal()
        {
            var connectionString = _appConfiguration[$"ConnectionStrings:{AppFrameworkConsts.ConnectionStringName}"];

            if (string.IsNullOrEmpty(connectionString))
            {
                return false;
            }

            return _databaseCheckHelper.Exist(connectionString);
        }

        private void SetConnectionString(string constring)
        {
            EditAppSettingsjson($"ConnectionStrings:{AppFrameworkConsts.ConnectionStringName}", constring);
        }

        private async Task SetAdminPassword(string adminPassword)
        {
            var admin = await UserManager.FindByIdAsync("1");

            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var loginResult = await _logInManager.LoginAsync(User.AdminUserName, "123qwe");
            var signInResult = await _signInManager.SignInOrTwoFactorAsync(loginResult, false);
            if (signInResult.Succeeded)
            {
                CheckErrors(await UserManager.ChangePasswordAsync(admin, adminPassword));
                admin.ShouldChangePasswordOnNextLogin = false;
                CheckErrors(await UserManager.UpdateAsync(admin));
            }
        }

        private void SetUrl(string webSitRUrl, string serverUrl)
        {
            if (!serverUrl.IsNullOrEmpty())
            {
                EditAppSettingsjson("App:ClientRootAddress", webSitRUrl);
                EditAppSettingsjson("App:ServerRootAddress", serverUrl);
            }
            else
            {
                EditAppSettingsjson("App:WebSiteRootAddress", webSitRUrl);
            }
        }

        private async Task SetDefaultLanguage(string language)
        {
            await SettingManager.ChangeSettingForApplicationAsync(LocalizationSettingNames.DefaultLanguage, language);
        }

        private async Task SetSmtpSettings(EmailSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress, input.DefaultFromAddress);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName, input.DefaultFromDisplayName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, input.SmtpHost);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port, input.SmtpPort.ToString(CultureInfo.InvariantCulture));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName, input.SmtpUserName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password, SimpleStringCipher.Instance.Encrypt(input.SmtpPassword));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, input.SmtpDomain);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl, input.SmtpEnableSsl.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials, input.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
        }

        private async Task SetBillingSettings(HostBillingSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingLegalName, input.LegalName);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingAddress, input.Address);
        }

        private void EditAppSettingsjson(string key, string value)
        {
            _appConfigurationWriter.Write(key, value);
        }
    }
}
