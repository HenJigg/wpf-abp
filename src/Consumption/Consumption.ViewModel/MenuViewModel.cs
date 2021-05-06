
namespace Consumption.ViewModel
{
    using Consumption.Shared.Common;
    using Consumption.Shared.Dto;
    using Consumption.ViewModel.Interfaces;

    /// <summary>
    /// 菜单业务
    /// </summary>
    public class MenuViewModel : BaseRepository<MenuDto>, IMenuViewModel
    {
        public MenuViewModel(IMenuRepository repository) : base(repository)
        {

        }
    }
}
