using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using AppFramework.Dto;
using AppFramework.Localization;
using AppFramework.Notifications;
using AppFramework.Storage;

namespace AppFramework.Gdpr
{
    public class UserCollectedDataPrepareJob : AsyncBackgroundJob<UserIdentifier>, ITransientDependency
    {
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IAppNotifier _appNotifier;
        private readonly ISettingManager _settingManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        
        public UserCollectedDataPrepareJob(
            IBinaryObjectManager binaryObjectManager,
            ITempFileCacheManager tempFileCacheManager,
            IAppNotifier appNotifier,
            ISettingManager settingManager, 
            IUnitOfWorkManager unitOfWorkManager)
        {
            _binaryObjectManager = binaryObjectManager;
            _tempFileCacheManager = tempFileCacheManager;
            _appNotifier = appNotifier;
            _settingManager = settingManager;
            _unitOfWorkManager = unitOfWorkManager;
        }
        
        public override async Task ExecuteAsync(UserIdentifier args)
        {
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                using (UnitOfWorkManager.Current.SetTenantId(args.TenantId))
                {
                    var userLanguage = await _settingManager.GetSettingValueForUserAsync(
                        LocalizationSettingNames.DefaultLanguage,
                        args.TenantId,
                        args.UserId
                    );
                
                    var culture = CultureHelper.GetCultureInfoByChecking(userLanguage);

                    using (CultureInfoHelper.Use(culture))
                    {
                        var files = new List<FileDto>();

                        using (var scope = IocManager.Instance.CreateScope())
                        {
                            var providers = scope.ResolveAll<IUserCollectedDataProvider>();
                            foreach (var provider in providers)
                            {
                                var providerFiles = await provider.GetFiles(args);
                                if (providerFiles.Any())
                                {
                                    files.AddRange(providerFiles);
                                }
                            }
                        }

                        var zipFile = new BinaryObject
                        (
                            args.TenantId,
                            CompressFiles(files),
                            $"{args.UserId} {DateTime.UtcNow} UserCollectedDataPrepareJob result"
                        );

                        // Save zip file to object manager.
                        await _binaryObjectManager.SaveAsync(zipFile);

                        // Send notification to user.
                        await _appNotifier.GdprDataPrepared(args, zipFile.Id);
                    }
                }
            });
        }

        private byte[] CompressFiles(List<FileDto> files)
        {
            using (var outputZipFileStream = new MemoryStream())
            {
                using (var zipStream = new ZipArchive(outputZipFileStream, ZipArchiveMode.Create))
                {
                    foreach (var file in files)
                    {
                        var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
                        var entry = zipStream.CreateEntry(file.FileName);

                        using (var originalFileStream = new MemoryStream(fileBytes))
                        using (var zipEntryStream = entry.Open())
                        {
                            originalFileStream.CopyTo(zipEntryStream);
                        }
                    }
                }

                return outputZipFileStream.ToArray();
            }
        }

    }
}
