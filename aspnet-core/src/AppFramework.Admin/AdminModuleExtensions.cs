using Prism.Ioc;

namespace AppFramework.Admin
{
    public static class AdminModuleExtensions
    {
        public static void AddAdminsServices(this IContainerRegistry services)
        {
            services.AddValidators();
            services.AddServices();
        }
    }
}
