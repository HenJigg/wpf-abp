

namespace Consumption.Api.ApiManager
{
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact.Response;
    using System.Threading.Tasks;

    public interface IBasicManager
    {
        Task<ApiResponse> GetAll(QueryParameters param);

        Task<ApiResponse> Add(BasicDto param);

        Task<ApiResponse> Delete(int id);

        Task<ApiResponse> Save(BasicDto param);
    }
}
