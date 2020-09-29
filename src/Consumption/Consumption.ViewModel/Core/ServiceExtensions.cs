
namespace Consumption.ViewModel.Core
{
    using Autofac;

    public static class ServiceExtensions
    {
        public static ContainerBuilder AddCustomRepository<TRepository, IRepository>(this ContainerBuilder services)
           where TRepository : class
        {
            services.RegisterType<TRepository>().As<IRepository>();
            return services;
        }

        public static ContainerBuilder AddCustomViewModel<TRepository, IRepository>(this ContainerBuilder services)
          where TRepository : class
        {
            services.RegisterType<TRepository>().As<IRepository>();
            return services;
        }
    }
}
