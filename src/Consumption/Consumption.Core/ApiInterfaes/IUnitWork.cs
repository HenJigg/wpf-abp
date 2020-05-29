
namespace Consumption.Core.ApiInterfaes
{
    using System.Threading.Tasks;

    public interface IUnitWork
    {
        Task<bool> SaveChangedAsync();
    }
}
