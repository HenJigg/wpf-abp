
namespace Consumption.ViewModel
{
    using Consumption.Shared.Dto;
    using Consumption.ViewModel.Common;
    using Consumption.ViewModel.Interfaces;
    using Prism.Ioc;

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserViewModel : BaseRepository<UserDto>, IUserViewModel
    {
        public UserViewModel(IUserRepository repository, IContainerProvider containerProvider)
            : base(repository, containerProvider)
        { }
    }
}
