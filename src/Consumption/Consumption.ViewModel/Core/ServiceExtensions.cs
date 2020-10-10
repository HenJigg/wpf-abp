
namespace Consumption.ViewModel.Core
{
    using Autofac;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceExtensions
    {
        public static ServiceCollection AddCustomRepository<TRepository, IRepository>(this ServiceCollection services)
        where TRepository : class, IRepository
        where IRepository : class
        {
            services.AddScoped<IRepository, TRepository>();
            return services;
        }

        public static ServiceCollection AddCustomViewModel<TRepository, IRepository>(this ServiceCollection services)
        where TRepository : class, IRepository
        where IRepository : class
        {
            services.AddScoped<IRepository, TRepository>();
            return services;
        }

        public static ServiceCollection AddCustomViewCenter<TCenter>(this ServiceCollection services)
       where TCenter : class
        {
            services.AddTransient(typeof(TCenter));
            return services;
        }

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
