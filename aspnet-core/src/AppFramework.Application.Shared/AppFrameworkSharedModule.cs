using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AppFramework
{
    [DependsOn(typeof(AppFrameworkCoreSharedModule))]
    public class AppFrameworkSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkSharedModule).GetAssembly());
        }
    }
}