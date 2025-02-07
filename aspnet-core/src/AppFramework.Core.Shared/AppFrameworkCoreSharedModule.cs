using Abp.Modules;
using Abp.Reflection.Extensions;

namespace AppFramework
{
    public class AppFrameworkCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AppFrameworkCoreSharedModule).GetAssembly());
        }
    }
}