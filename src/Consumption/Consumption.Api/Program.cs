namespace Consumption.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.EFCore.Context;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.OpenApi.Writers;
    using NLog.Web;

    /// <summary>
    /// 应用程序主入口
    /// </summary>
    public class Program
    {
        /*
        *  首次运行项目须知:
        *  1.请检查 appsettings.json 中 Connection 连接是否与当前的环境一致
        *  
        *  2.请确保 数据库的迁移文件已经更新到你的数据库当中。
        *    2.1. 打开程序包管理控制台, 确保 Consumption.Api 项目为启动项
        *    
        *    2.2. 首先创建迁移文件,确保默认项目选中为EfCore  
        *         命令: add-migration firstProjectName
        *         
        *    2.3. 然后更新至数据库  命令: update-database
        *    
        *    2.4. 启动项目, 如页面成功显示Open API 预览页, 通过设置接口访问数据,
        *    如正常访问, 则代表配置数据库部分成功。
        *    
        *  注意: 关于EfCore 创建迁移, 以及如何将迁移文件生成到数据库当中, 涉及到了解EfCore的相关知识
        *  
        *  博客地址: 
        *  https://www.cnblogs.com/zh7791/p/12931449.html
        *  
        *  微软官方文档地址(关于如何创建迁移部分以及根据项目生成数据库):
        *  https://docs.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
        *   
        */

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                var host = CreateHostBuilder(args).Build();
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// 创建主机构建器
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                     .ConfigureLogging(logging =>
                     {
                         logging.ClearProviders();
                         logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                     }).UseNLog()
                     .UseUrls("http://*:5001");
                });
    }
}
