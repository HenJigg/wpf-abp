

namespace Consumption.Api.ApiManager
{
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact.Response;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IMenuManager
    {
        Task<ApiResponse> GetAll(QueryParameters param);

        Task<ApiResponse> Add(MenuDto param);

        Task<ApiResponse> Delete(int id);

        Task<ApiResponse> Save(MenuDto param);
    }
}
