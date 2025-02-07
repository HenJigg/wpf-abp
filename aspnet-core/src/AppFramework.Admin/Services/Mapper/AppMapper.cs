using AppFramework.Shared.Services.Mapper;
using AutoMapper;
using System;

namespace AppFramework.Admin.Mapper
{
    public class AppMapper : IAppMapper
    {
        public AppMapper()
        {
            var configuration = new MapperConfiguration(configure =>
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies();
                configure.AddMaps(assemblys);
            });
            Current = configuration.CreateMapper();
        }

        public IMapper Current { get; }

        public TDestination Map<TDestination>(object source) => Current.Map<TDestination>(source);
    }
}
