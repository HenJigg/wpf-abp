
namespace Consumption.ViewModel
{
    using Consumption.Shared.Common;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Consumption.ViewModel.Interfaces;

    /// <summary>
    /// 基础数据
    /// </summary>
    public class BasicViewModel : BaseRepository<BasicDto>, IBasicViewModel
    {
        public BasicViewModel(IBasicRepository repository) : base(repository)
        { }
    }
}
