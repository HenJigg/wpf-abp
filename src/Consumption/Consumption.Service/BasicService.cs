/*
*
* 文件名    ：BasicService                             
* 程序说明  : 基础数据服务
* 更新时间  : 2020-09-11 09：46 
*/

namespace Consumption.Service
{
    using Consumption.Shared.Dto;
    using Consumption.ViewModel.Interfaces;

    /// <summary>
    /// 基础数据服务
    /// </summary>
    public partial class BasicService : BaseService<BasicDto>, IBasicRepository
    {

    }
}
