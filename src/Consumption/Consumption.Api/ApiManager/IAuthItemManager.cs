

namespace Consumption.Api.ApiManager
{
    using Consumption.Shared.HttpContact.Response;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IAuthItemManager
    {
        Task<ApiResponse> GetAll();

    }
}
