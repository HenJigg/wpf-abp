using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Json;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero.Configuration;
using AppFramework.Authentication;
using AppFramework.Authorization;
using AppFramework.Configuration.Dto;
using AppFramework.Configuration.Host.Dto;
using AppFramework.Editions;
using AppFramework.Security;
using AppFramework.Timing;

namespace AppFramework.Configuration.Host
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Host_Settings)]
    public class HostSettingsAppService : SettingsAppServiceBase, IHostSettingsAppService
    {
        public IExternalLoginOptionsCacheManager ExternalLoginOptionsCacheManager { get; set; }

        private readonly EditionManager _editionManager;
        private readonly ITimeZoneService _timeZoneService;
        readonly ISettingDefinitionManager _settingDefinitionManager;

        public HostSettingsAppService(
            IEmailSender emailSender,
            EditionManager editionManager,
            ITimeZoneService timeZoneService,
            ISettingDefinitionManager settingDefinitionManager,
            IAppConfigurationAccessor configurationAccessor) : base(emailSender, configurationAccessor)
        {
            ExternalLoginOptionsCacheManager = NullExternalLoginOptionsCacheManager.Instance;

            _editionManager = editionManager;
            _timeZoneService = timeZoneService;
            _settingDefinitionManager = settingDefinitionManager;
        }

        #region Get Settings

        public async Task<HostSettingsEditDto> GetAllSettings()
        {
            return new HostSettingsEditDto
            {
                General = await GetGeneralSettingsAsync(),
                TenantManagement = await GetTenantManagementSettingsAsync(),
                UserManagement = await GetUserManagementAsync(),
                Email = await GetEmailSettingsAsync(),
                Security = await GetSecuritySettingsAsync(),
                Billing = await GetBillingSettingsAsync(),
                OtherSettings = await GetOtherSettingsAsync(),
                ExternalLoginProviderSettings = await GetExternalLoginProviderSettings()
            };
        }

        private async Task<GeneralSettingsEditDto> GetGeneralSettingsAsync()
        {
            var timezone = await SettingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);
            var settings = new GeneralSettingsEditDto
            {
                Timezone = timezone,
                TimezoneForComparison = timezone
            };

            var defaultTimeZoneId =
                await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
            if (settings.Timezone == defaultTimeZoneId)
            {
                settings.Timezone = string.Empty;
            }

            return settings;
        }

        private async Task<TenantManagementSettingsEditDto> GetTenantManagementSettingsAsync()
        {
            var settings = new TenantManagementSettingsEditDto
            {
                AllowSelfRegistration =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.AllowSelfRegistration),
                IsNewRegisteredTenantActiveByDefault =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement
                        .IsNewRegisteredTenantActiveByDefault),
                UseCaptchaOnRegistration =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement
                        .UseCaptchaOnRegistration),
            };

            var defaultEditionId =
                await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.DefaultEdition);
            if (!string.IsNullOrEmpty(defaultEditionId) &&
                (await _editionManager.FindByIdAsync(Convert.ToInt32(defaultEditionId)) != null))
            {
                settings.DefaultEditionId = Convert.ToInt32(defaultEditionId);
            }

            return settings;
        }

        private async Task<HostUserManagementSettingsEditDto> GetUserManagementAsync()
        {
            return new HostUserManagementSettingsEditDto
            {
                IsEmailConfirmationRequiredForLogin =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .IsEmailConfirmationRequiredForLogin),
                SmsVerificationEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.SmsVerificationEnabled),
                IsCookieConsentEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsCookieConsentEnabled),
                IsQuickThemeSelectEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(
                        AppSettings.UserManagement.IsQuickThemeSelectEnabled),
                UseCaptchaOnLogin =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.UseCaptchaOnLogin),
                AllowUsingGravatarProfilePicture =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement
                        .AllowUsingGravatarProfilePicture),
                SessionTimeOutSettings = new SessionTimeOutSettingsEditDto
                {
                    IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement
                        .SessionTimeOut.IsEnabled),
                    TimeOutSecond =
                        await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.SessionTimeOut
                            .TimeOutSecond),
                    ShowTimeOutNotificationSecond =
                        await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.SessionTimeOut
                            .ShowTimeOutNotificationSecond),
                    ShowLockScreenWhenTimedOut =
                        await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.SessionTimeOut
                            .ShowLockScreenWhenTimedOut)
                },
                UserPasswordSettings = new UserPasswordSettingsEditDto()
                {
                    EnableCheckingLastXPasswordWhenPasswordChange = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.Password.EnableCheckingLastXPasswordWhenPasswordChange),
                    CheckingLastXPasswordCount = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.Password.CheckingLastXPasswordCount),
                    EnablePasswordExpiration = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.Password.EnablePasswordExpiration),
                    PasswordExpirationDayCount = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.Password.PasswordExpirationDayCount),
                }
            };
        }

        private async Task<EmailSettingsEditDto> GetEmailSettingsAsync()
        {
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);

            return new EmailSettingsEditDto
            {
                DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName =
                    await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
                SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port),
                SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName),
                SmtpPassword = SimpleStringCipher.Instance.Decrypt(smtpPassword),
                SmtpDomain = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl),
                SmtpUseDefaultCredentials =
                    await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials)
            };
        }

        private async Task<SecuritySettingsEditDto> GetSecuritySettingsAsync()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireDigit),
                RequireLowercase =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                        .PasswordComplexity.RequireUppercase),
                RequiredLength =
                    await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity
                        .RequiredLength)
            };

            var defaultPasswordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = Convert.ToBoolean(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit)
                    .DefaultValue),
                RequireLowercase = Convert.ToBoolean(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase)
                    .DefaultValue),
                RequireNonAlphanumeric = Convert.ToBoolean(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric)
                    .DefaultValue),
                RequireUppercase = Convert.ToBoolean(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase)
                    .DefaultValue),
                RequiredLength = Convert.ToInt32(_settingDefinitionManager
                    .GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
                    .DefaultValue)
            };

            return new SecuritySettingsEditDto
            {
                UseDefaultPasswordComplexitySettings =
                    passwordComplexitySetting.Equals(defaultPasswordComplexitySetting),
                PasswordComplexity = passwordComplexitySetting,
                DefaultPasswordComplexity = defaultPasswordComplexitySetting,
                UserLockOut = await GetUserLockOutSettingsAsync(),
                TwoFactorLogin = await GetTwoFactorLoginSettingsAsync(),
                AllowOneConcurrentLoginPerUser = await GetOneConcurrentLoginPerUserSetting()
            };
        }

        private async Task<HostBillingSettingsEditDto> GetBillingSettingsAsync()
        {
            return new HostBillingSettingsEditDto
            {
                LegalName = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingLegalName),
                Address = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingAddress)
            };
        }

        private async Task<OtherSettingsEditDto> GetOtherSettingsAsync()
        {
            return new OtherSettingsEditDto()
            {
                IsQuickThemeSelectEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(
                        AppSettings.UserManagement.IsQuickThemeSelectEnabled)
            };
        }

        private async Task<UserLockOutSettingsEditDto> GetUserLockOutSettingsAsync()
        {
            return new UserLockOutSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                    .UserLockOut.IsEnabled),
                MaxFailedAccessAttemptsBeforeLockout =
                    await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut
                        .MaxFailedAccessAttemptsBeforeLockout),
                DefaultAccountLockoutSeconds =
                    await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut
                        .DefaultAccountLockoutSeconds)
            };
        }

        private async Task<TwoFactorLoginSettingsEditDto> GetTwoFactorLoginSettingsAsync()
        {
            var twoFactorLoginSettingsEditDto = new TwoFactorLoginSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement
                    .TwoFactorLogin.IsEnabled),
                IsEmailProviderEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
                        .IsEmailProviderEnabled),
                IsSmsProviderEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
                        .IsSmsProviderEnabled),
                IsRememberBrowserEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin
                        .IsRememberBrowserEnabled),
                IsGoogleAuthenticatorEnabled =
                    await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.TwoFactorLogin
                        .IsGoogleAuthenticatorEnabled)
            };
            return twoFactorLoginSettingsEditDto;
        }

        private async Task<bool> GetOneConcurrentLoginPerUserSetting()
        {
            return await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement
                .AllowOneConcurrentLoginPerUser);
        }

        private async Task<ExternalLoginProviderSettingsEditDto> GetExternalLoginProviderSettings()
        {
            var facebookSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host
                    .Facebook);
            var googleSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host.Google);
            var twitterSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host.Twitter);
            var microsoftSettings =
                await SettingManager.GetSettingValueForApplicationAsync(
                    AppSettings.ExternalLoginProvider.Host.Microsoft);

            var openIdConnectSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host
                    .OpenIdConnect);
            var openIdConnectMapperClaims =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider
                    .OpenIdConnectMappedClaims);

            var wsFederationSettings =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider.Host
                    .WsFederation);
            var wsFederationMapperClaims =
                await SettingManager.GetSettingValueForApplicationAsync(AppSettings.ExternalLoginProvider
                    .WsFederationMappedClaims);

            return new ExternalLoginProviderSettingsEditDto
            {
                Facebook = facebookSettings.IsNullOrWhiteSpace()
                    ? new FacebookExternalLoginProviderSettings()
                    : facebookSettings.FromJsonString<FacebookExternalLoginProviderSettings>(),
                Google = googleSettings.IsNullOrWhiteSpace()
                    ? new GoogleExternalLoginProviderSettings()
                    : googleSettings.FromJsonString<GoogleExternalLoginProviderSettings>(),
                Twitter = twitterSettings.IsNullOrWhiteSpace()
                    ? new TwitterExternalLoginProviderSettings()
                    : twitterSettings.FromJsonString<TwitterExternalLoginProviderSettings>(),
                Microsoft = microsoftSettings.IsNullOrWhiteSpace()
                    ? new MicrosoftExternalLoginProviderSettings()
                    : microsoftSettings.FromJsonString<MicrosoftExternalLoginProviderSettings>(),

                OpenIdConnect = openIdConnectSettings.IsNullOrWhiteSpace()
                    ? new OpenIdConnectExternalLoginProviderSettings()
                    : openIdConnectSettings.FromJsonString<OpenIdConnectExternalLoginProviderSettings>(),
                OpenIdConnectClaimsMapping = openIdConnectMapperClaims.IsNullOrWhiteSpace()
                    ? new List<JsonClaimMapDto>()
                    : openIdConnectMapperClaims.FromJsonString<List<JsonClaimMapDto>>(),

                WsFederation = wsFederationSettings.IsNullOrWhiteSpace()
                    ? new WsFederationExternalLoginProviderSettings()
                    : wsFederationSettings.FromJsonString<WsFederationExternalLoginProviderSettings>(),
                WsFederationClaimsMapping = wsFederationMapperClaims.IsNullOrWhiteSpace()
                    ? new List<JsonClaimMapDto>()
                    : wsFederationMapperClaims.FromJsonString<List<JsonClaimMapDto>>()
            };
        }

        #endregion

        #region Update Settings

        public async Task UpdateAllSettings(HostSettingsEditDto input)
        {
            await UpdateGeneralSettingsAsync(input.General);
            await UpdateTenantManagementAsync(input.TenantManagement);
            await UpdateUserManagementSettingsAsync(input.UserManagement);
            await UpdateSecuritySettingsAsync(input.Security);
            await UpdateEmailSettingsAsync(input.Email);
            await UpdateBillingSettingsAsync(input.Billing);
            await UpdateOtherSettingsAsync(input.OtherSettings);
            await UpdateExternalLoginSettingsAsync(input.ExternalLoginProviderSettings);
        }

        private async Task UpdateOtherSettingsAsync(OtherSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.IsQuickThemeSelectEnabled,
                input.IsQuickThemeSelectEnabled.ToString().ToLowerInvariant()
            );
        }

        private async Task UpdateBillingSettingsAsync(HostBillingSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingLegalName,
                input.LegalName);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingAddress,
                input.Address);
        }

        private async Task UpdateGeneralSettingsAsync(GeneralSettingsEditDto settings)
        {
            if (Clock.SupportsMultipleTimezone)
            {
                if (settings.Timezone.IsNullOrEmpty())
                {
                    var defaultValue =
                        await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone,
                        settings.Timezone);
                }
            }
        }

        private async Task UpdateTenantManagementAsync(TenantManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.AllowSelfRegistration,
                settings.AllowSelfRegistration.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault,
                settings.IsNewRegisteredTenantActiveByDefault.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.UseCaptchaOnRegistration,
                settings.UseCaptchaOnRegistration.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.DefaultEdition,
                settings.DefaultEditionId?.ToString() ?? ""
            );
        }

        private async Task UpdateUserManagementSettingsAsync(HostUserManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                settings.IsEmailConfirmationRequiredForLogin.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SmsVerificationEnabled,
                settings.SmsVerificationEnabled.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.IsCookieConsentEnabled,
                settings.IsCookieConsentEnabled.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.UseCaptchaOnLogin,
                settings.UseCaptchaOnLogin.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.AllowUsingGravatarProfilePicture,
                settings.AllowUsingGravatarProfilePicture.ToString().ToLowerInvariant()
            );

            await UpdateUserManagementSessionTimeOutSettingsAsync(settings.SessionTimeOutSettings);
            await UpdateUserManagementPasswordSettingsAsync(settings.UserPasswordSettings);
        }

        private async Task UpdateUserManagementPasswordSettingsAsync(UserPasswordSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.Password.EnableCheckingLastXPasswordWhenPasswordChange,
                settings.EnableCheckingLastXPasswordWhenPasswordChange.ToString().ToLowerInvariant()
            );
            
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.Password.CheckingLastXPasswordCount,
                settings.CheckingLastXPasswordCount.ToString()
            );
            
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.Password.EnablePasswordExpiration,
                settings.EnablePasswordExpiration.ToString().ToLowerInvariant()
            );
            
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.Password.PasswordExpirationDayCount,
                settings.PasswordExpirationDayCount.ToString()
            );
        }

        private async Task UpdateUserManagementSessionTimeOutSettingsAsync(SessionTimeOutSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SessionTimeOut.IsEnabled,
                settings.IsEnabled.ToString().ToLowerInvariant()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SessionTimeOut.TimeOutSecond,
                settings.TimeOutSecond.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SessionTimeOut.ShowTimeOutNotificationSecond,
                settings.ShowTimeOutNotificationSecond.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.SessionTimeOut.ShowLockScreenWhenTimedOut,
                settings.ShowLockScreenWhenTimedOut.ToString()
            );
        }

        private async Task UpdateSecuritySettingsAsync(SecuritySettingsEditDto settings)
        {
            if (settings.UseDefaultPasswordComplexitySettings)
            {
                await UpdatePasswordComplexitySettingsAsync(settings.DefaultPasswordComplexity);
            }
            else
            {
                await UpdatePasswordComplexitySettingsAsync(settings.PasswordComplexity);
            }

            await UpdateUserLockOutSettingsAsync(settings.UserLockOut);
            await UpdateTwoFactorLoginSettingsAsync(settings.TwoFactorLogin);
            await UpdateOneConcurrentLoginPerUserSettingAsync(settings.AllowOneConcurrentLoginPerUser);
        }

        private async Task UpdatePasswordComplexitySettingsAsync(PasswordComplexitySetting settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit,
                settings.RequireDigit.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase,
                settings.RequireLowercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric,
                settings.RequireNonAlphanumeric.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase,
                settings.RequireUppercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength,
                settings.RequiredLength.ToString()
            );
        }

        private async Task UpdateUserLockOutSettingsAsync(UserLockOutSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled,
                settings.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds,
                settings.DefaultAccountLockoutSeconds.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout,
                settings.MaxFailedAccessAttemptsBeforeLockout.ToString());
        }

        private async Task UpdateTwoFactorLoginSettingsAsync(TwoFactorLoginSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled,
                settings.IsEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled,
                settings.IsEmailProviderEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled,
                settings.IsSmsProviderEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.TwoFactorLogin.IsGoogleAuthenticatorEnabled,
                settings.IsGoogleAuthenticatorEnabled.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled,
                settings.IsRememberBrowserEnabled.ToString().ToLowerInvariant());
        }

        private async Task UpdateEmailSettingsAsync(EmailSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress,
                settings.DefaultFromAddress);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName,
                settings.DefaultFromDisplayName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, settings.SmtpHost);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port,
                settings.SmtpPort.ToString(CultureInfo.InvariantCulture));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName,
                settings.SmtpUserName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password,
                SimpleStringCipher.Instance.Encrypt(settings.SmtpPassword));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, settings.SmtpDomain);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl,
                settings.SmtpEnableSsl.ToString().ToLowerInvariant());
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials,
                settings.SmtpUseDefaultCredentials.ToString().ToLowerInvariant());
        }

        private async Task UpdateOneConcurrentLoginPerUserSettingAsync(bool allowOneConcurrentLoginPerUser)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.UserManagement.AllowOneConcurrentLoginPerUser, allowOneConcurrentLoginPerUser.ToString());
        }

        private async Task UpdateExternalLoginSettingsAsync(ExternalLoginProviderSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.Facebook,
                input.Facebook == null || !input.Facebook.IsValid()
                    ? _settingDefinitionManager.GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.Facebook)
                        .DefaultValue
                    : input.Facebook.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.Google,
                input.Google == null || !input.Google.IsValid()
                    ? _settingDefinitionManager.GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.Google)
                        .DefaultValue
                    : input.Google.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.Twitter,
                input.Twitter == null || !input.Twitter.IsValid()
                    ? _settingDefinitionManager.GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.Twitter)
                        .DefaultValue
                    : input.Twitter.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.Microsoft,
                input.Microsoft == null || !input.Microsoft.IsValid()
                    ? _settingDefinitionManager.GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.Microsoft)
                        .DefaultValue
                    : input.Microsoft.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.OpenIdConnect,
                input.OpenIdConnect == null || !input.OpenIdConnect.IsValid()
                    ? _settingDefinitionManager
                        .GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.OpenIdConnect).DefaultValue
                    : input.OpenIdConnect.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.OpenIdConnectMappedClaims,
                input.OpenIdConnectClaimsMapping.IsNullOrEmpty()
                    ? _settingDefinitionManager
                        .GetSettingDefinition(AppSettings.ExternalLoginProvider.OpenIdConnectMappedClaims).DefaultValue
                    : input.OpenIdConnectClaimsMapping.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.Host.WsFederation,
                input.WsFederation == null || !input.WsFederation.IsValid()
                    ? _settingDefinitionManager
                        .GetSettingDefinition(AppSettings.ExternalLoginProvider.Host.WsFederation).DefaultValue
                    : input.WsFederation.ToJsonString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.ExternalLoginProvider.WsFederationMappedClaims,
                input.WsFederationClaimsMapping.IsNullOrEmpty()
                    ? _settingDefinitionManager
                        .GetSettingDefinition(AppSettings.ExternalLoginProvider.WsFederationMappedClaims).DefaultValue
                    : input.WsFederationClaimsMapping.ToJsonString()
            );

            ExternalLoginOptionsCacheManager.ClearCache();
        }

        #endregion
    }
}
