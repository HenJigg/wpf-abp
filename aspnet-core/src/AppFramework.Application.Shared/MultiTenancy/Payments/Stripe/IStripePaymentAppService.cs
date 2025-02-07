using System.Threading.Tasks;
using Abp.Application.Services;
using AppFramework.MultiTenancy.Payments.Dto;
using AppFramework.MultiTenancy.Payments.Stripe.Dto;

namespace AppFramework.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}