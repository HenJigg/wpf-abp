namespace Consumption.Api
{
    using System;
    using System.Reflection;
    using AutoMapper;
    using Consumption.Api.ApiManager;
    using Consumption.Api.Extensions;
    using Consumption.EFCore;
    using Consumption.EFCore.Context;
    using Consumption.Shared.DataInterfaces;
    using Consumption.Shared.DataModel;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;


    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin();
                });
            });
            services.AddControllers();
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            var migrationsAssembly = typeof(ConsumptionContext).GetTypeInfo().Assembly.GetName();
            var migrationsAssemblyName = migrationsAssembly.Name;

            services.AddDbContext<ConsumptionContext>(options =>
            {
                //设置至Sqlite
                //var connectionString = Configuration.GetConnectionString("NoteConnection");
                //options.UseSqlite(connectionString,sql => sql.MigrationsAssembly(migrationsAssemblyName));

                //设置至MySql
                //var connectionString = Configuration.GetConnectionString("MySqlNoteConnection");
                //options.UseMySQL(connectionString, sql => sql.MigrationsAssembly(migrationsAssemblyName));

                //设置至MsSql
                var connectionString = Configuration.GetConnectionString("MsSqlNoteConnection");
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssemblyName));
            })
            .AddUnitOfWork<ConsumptionContext>()
            .AddCustomRepository<User, CustomUserRepository>()
            .AddCustomRepository<UserLog, CustomUserLogRepository>()
            .AddCustomRepository<Menu, CustomMenuRepository>()
            .AddCustomRepository<Group, CustomGroupRepository>()
            .AddCustomRepository<AuthItem, CustomAuthItemRepository>()
            .AddCustomRepository<Basic, CustomBasicRepository>();

            services.AddTransient<IDataInitializer, DataInitializer>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IMenuManager, MenuManager>();
            services.AddTransient<IGroupManager, GroupManager>();
            services.AddTransient<IBasicManager, BasicManager>();
            services.AddTransient<IAuthItemManager, AuthManager>();

            var automapperConfig = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new AutoMappingFile());
            });
            var autoMapper = automapperConfig.CreateMapper();
            services.AddSingleton(autoMapper);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = ".NET Core WebApi v1", Version = "v1" });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //初始化数据库
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var databaseInitializer = serviceScope.ServiceProvider.GetService<IDataInitializer>();
                await databaseInitializer.InitSampleDataAsync();
            }

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseFileServer();
            app.UseHttpsRedirection();
            app.UseCors("any");
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.ShowExtensions();
                options.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET Core WebApi v1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapSwagger();
            });
        }
    }
}
