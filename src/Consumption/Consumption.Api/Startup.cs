/*
*
* 文件名    ：Startup                          
* 程序说明  : 启动项配置
* 更新时间  : 2020-05-21 11:44
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/


namespace Consumption.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.Core.Entity;
    using Consumption.EFCore;
    using Consumption.EFCore.Context;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

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

            services.AddDbContext<ConsumptionContext>(options =>
            {
                //迁移至Sqlite
                //var connectionString = Configuration.GetConnectionString("NoteConnection");
                //options.UseSqlite(connectionString);

                //迁移至MySql
                var connectionString = Configuration.GetConnectionString("MySqlNoteConnection");
                options.UseMySQL(connectionString);
            })
            .AddUnitOfWork<ConsumptionContext>()
            .AddCustomRepository<User, CustomUserRepository>()
            .AddCustomRepository<UserLog, CustomUserLogRepository>()
            .AddCustomRepository<Menu, CustomMenuRepository>()
            .AddCustomRepository<Group, CustomGroupRepository>()
            .AddCustomRepository<AuthItem, CustomAuthItemRepository>()
            .AddCustomRepository<Basic, CustomBasicRepository>();

            services.AddSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NoteApi.xml"));
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Note Service API",
                    Version = "v1",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "WPF-Xamarin-Blazor-Examples",
                        Url = new Uri("https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples")
                    }
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseCors("any");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.ShowExtensions();
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "NoteApi");
            });
        }
    }
}
