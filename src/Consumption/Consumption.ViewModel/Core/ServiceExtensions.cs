
namespace Consumption.ViewModel.Core
{
    using Autofac;
    using Consumption.ViewModel.Interfaces;

    public static class ServiceExtensions
    {
        public static ContainerBuilder AddViewCenter<TCenter, ICenter>(this ContainerBuilder services)
        {
            services.RegisterType<TCenter>().Named(typeof(TCenter).Name, typeof(ICenter));
            return services;
        }

        public static ContainerBuilder AddRepository<TRepository, IRepository>(this ContainerBuilder services)
        where TRepository : class
        {
            services.RegisterType<TRepository>().As<IRepository>();
            return services;
        }

        public static ContainerBuilder AddViewModel<TRepository, IRepository>(this ContainerBuilder services)
        where TRepository : class
        {
            services.RegisterType<TRepository>().As<IRepository>();
            return services;
        }
    }
}
