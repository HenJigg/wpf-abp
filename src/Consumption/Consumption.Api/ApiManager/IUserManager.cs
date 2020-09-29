

namespace Consumption.Api.ApiManager
{
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact.Response;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IUserManager
    {
        Task<ApiResponse> Login(LoginDto param);

        Task<ApiResponse> GetAll(QueryParameters param);

        Task<ApiResponse> Get(int id);

        Task<ApiResponse> Add(UserDto param);

        Task<ApiResponse> Delete(int id);

        Task<ApiResponse> Save(UserDto param);
    }
}
