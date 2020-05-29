/*
*
* 文件名    ：BasicTypeRepository                             
* 程序说明  : 基础数据类型数据接口实现
* 更新时间  : 2020-05-21 16：43
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/


namespace Consumption.EFCore.Repository
{
    using Consumption.Core.ApiInterfaes;
    using Consumption.Core.Entity;
    using Consumption.EFCore.Orm;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BasicTypeRepository : BaseRepository<BasicType>, IBasicTypeRepository
    {
        public BasicTypeRepository(ConsumptionContext consumptionContext) : base(consumptionContext)
        {
        }
    }
}
