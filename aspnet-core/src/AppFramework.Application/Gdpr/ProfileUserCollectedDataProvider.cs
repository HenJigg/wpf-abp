using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.AspNetZeroCore.Net;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using AppFramework.Authorization.Users;
using AppFramework.Dto;
using AppFramework.MultiTenancy;
using AppFramework.Storage;

namespace AppFramework.Gdpr
{
    public class ProfileUserCollectedDataProvider : IUserCollectedDataProvider, ITransientDependency
    {
        private readonly UserManager _userManager;
        private readonly TenantManager _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILocalizationManager _localizationManager;

        public ProfileUserCollectedDataProvider(
            UserManager userManager,
            TenantManager tenantManager,
            ITempFileCacheManager tempFileCacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            ILocalizationManager localizationManager)
        {
            _userManager = userManager;
            _tempFileCacheManager = tempFileCacheManager;
            _unitOfWorkManager = unitOfWorkManager;
            _localizationManager = localizationManager;
            _tenantManager = tenantManager;
        }

        public async Task<List<FileDto>> GetFiles(UserIdentifier user)
        {
            var tenancyName = ".";
            if (user.TenantId.HasValue)
            {
                using (_unitOfWorkManager.Current.SetTenantId(null))
                {
                    tenancyName = (await _tenantManager.GetByIdAsync(user.TenantId.Value)).TenancyName;
                }
            }

            var profileInfo = await _userManager.GetUserByIdAsync(user.UserId);

            var content = new List<string>
            {
                L("TenancyName")+ ": " + tenancyName,
                L("UserName") +": " + profileInfo.UserName,
                L("Name") +": " + profileInfo.Name,
                L("Surname") +": " + profileInfo.Surname,
                L("EmailAddress") +": " + profileInfo.EmailAddress,
                L("PhoneNumber") +": " + profileInfo.PhoneNumber
            };

            var profileInfoBytes = Encoding.UTF8.GetBytes(string.Join("\n\r", content));

            var file = new FileDto("ProfileInfo.txt", MimeTypeNames.TextPlain);
            _tempFileCacheManager.SetFile(file.FileToken, profileInfoBytes);

            return new List<FileDto> { file };
        }

        private string L(string name)
        {
            return _localizationManager.GetString(AppFrameworkConsts.LocalizationSourceName, name);
        }
    }
}