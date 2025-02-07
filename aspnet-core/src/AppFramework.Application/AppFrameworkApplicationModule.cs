using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AppFramework.Authorization;

namespace AppFramework
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(AppFrameworkSharedModule),
        typeof(AppFrameworkCoreModule)
        )]
    public class AppFrameworkApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkApplicationModule).GetAssembly());
        }
    }
}