
namespace Consumption.ViewModel
{
    using Consumption.Shared.Dto;
    using Consumption.ViewModel.Common;
    using Consumption.ViewModel.Interfaces;
    using Prism.Ioc;

    /// <summary>
    /// 基础数据
    /// </summary>
    public class BasicViewModel : BaseRepository<BasicDto>, IBasicViewModel
    {
        public BasicViewModel(IBasicRepository repository, IContainerProvider containerProvider)
            : base(repository, containerProvider)
        { }
    }
}
