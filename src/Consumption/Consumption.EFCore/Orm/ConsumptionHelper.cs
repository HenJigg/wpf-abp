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
            if (!context.Users.Any())
            {
                var users = new[]
                {
                    new User(){ Account="admin",UserName="tom",Address="China", FlagAdmin=1,Password="123" },
                    new User(){ Account="qc001",UserName="marie",Address="USA", FlagAdmin=0,Password="123" },
                    new User(){ Account="qc002",UserName="darcy",Address="USA", FlagAdmin=0,Password="123" },
                };

                foreach (var U in users)
                {
                    await context.Users.AddAsync(U);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
