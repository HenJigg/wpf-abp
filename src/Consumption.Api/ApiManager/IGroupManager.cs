

namespace Consumption.Api.ApiManager
{
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact.Response;
    using System.Threading.Tasks;

    public interface IGroupManager
    {
        Task<ApiResponse> GetAll(QueryParameters param);


        Task<ApiResponse> Delete(int id);

        Task<ApiResponse> Save(GroupDataDto param);

        Task<ApiResponse> GetMenuModuleList();

        Task<ApiResponse> GetGroupData(int id);
    }
}
