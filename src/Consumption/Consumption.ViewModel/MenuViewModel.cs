
namespace Consumption.ViewModel
{
    using Consumption.Shared.Dto;
    using Consumption.ViewModel.Common;
    using Consumption.ViewModel.Interfaces;
    using Prism.Ioc;

    /// <summary>
    /// 菜单业务
    /// </summary>
    public class MenuViewModel : BaseRepository<MenuDto>, IMenuViewModel
    {
        public MenuViewModel(IMenuRepository repository, IContainerProvider containerProvider)
            : base(repository, containerProvider)
        { }
    }
}
