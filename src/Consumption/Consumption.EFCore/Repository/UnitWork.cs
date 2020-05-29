/*
*
* 文件名    ：UnitWork                             
* 程序说明  : EF 工作单元
* 更新时间  : 2020-05-21 14：39
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/


namespace Consumption.EFCore.Repository
{
    using Consumption.Core.ApiInterfaes;
    using Consumption.EFCore.Orm;
    using System.Threading.Tasks;

    /// <summary>
    /// EF 工作单元
    /// </summary>
    public class UnitWork : IUnitWork
    {
        private readonly ConsumptionContext context;

        public UnitWork(ConsumptionContext context)
        {
            this.context = context;
        }
        public async Task<bool> SaveChangedAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
