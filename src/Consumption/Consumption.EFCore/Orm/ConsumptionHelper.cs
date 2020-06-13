/*
*
* 文件名    ：ConsumptionHelper                             
* 程序说明  : 数据库上下文帮助类
* 更新时间  : 2020-05-22 13：48
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/


namespace Consumption.EFCore.Orm
{
    using Consumption.Core.Entity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 数据库上下文帮助类
    /// </summary>
    public class ConsumptionHelper
    {
        /// <summary>
        /// 初始化样本数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task InitSampleDataAsync(ConsumptionContext context)
        {
            if (context.Users.Any())
            {
                List<User> userList = new List<User>();
                for (int i = 0; i < 100; i++)
                {
                    userList.Add(new User()
                    {
                        Account = $"admin{i}",
                        UserName = $"tom{i}",
                        Address = "China",
                        FlagAdmin = 1,
                        Password = "123",
                        CreateTime = DateTime.Now,
                    });
                }
                userList.ForEach(async arg =>
                {
                    await context.Users.AddAsync(arg);
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
