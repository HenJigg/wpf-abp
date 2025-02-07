using System.Threading.Tasks;
using Abp.Application.Services;

namespace AppFramework.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
