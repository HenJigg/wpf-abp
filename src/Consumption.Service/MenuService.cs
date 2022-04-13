/*
*
* 文件名    ：MenuService                             
* 程序说明  : 菜单服务
* 更新时间  : 2020-06-15 22：32 
*/


namespace Consumption.Service
{
    using Consumption.Shared.Dto;
    using Consumption.ViewModel.Interfaces;

    /// <summary>
    /// 菜单服务
    /// </summary>
    public partial class MenuService : BaseService<MenuDto>, IMenuRepository
    {

    }

}
