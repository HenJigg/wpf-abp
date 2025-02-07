using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.UI;
using AppFramework.Authorization.Users;
using AppFramework.Editions;
using AppFramework.MultiTenancy.Payments.Dto;
using AppFramework.MultiTenancy.Payments.Stripe;
using AppFramework.MultiTenancy.Payments.Stripe.Dto;
using Stripe;
using Stripe.Checkout;

namespace AppFramework.MultiTenancy.Payments
{
    public class StripePaymentAppService : AppFrameworkAppServiceBase, IStripePaymentAppService
    {
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly ISubscriptionPaymentExtensionDataRepository _subscriptionPaymentExtensionDataRepository;
        private readonly IPaymentAppService _paymentAppService;
        private readonly StripeGatewayManager _stripeGatewayManager;
        private readonly StripePaymentGatewayConfiguration _stripePaymentGatewayConfiguration;

        public StripePaymentAppService(
            StripeGatewayManager stripeGatewayManager,
            StripePaymentGatewayConfiguration stripePaymentGatewayConfiguration,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            ISubscriptionPaymentExtensionDataRepository subscriptionPaymentExtensionDataRepository,
            IPaymentAppService paymentAppService)
        {
            _stripeGatewayManager = stripeGatewayManager;
            _stripePaymentGatewayConfiguration = stripePaymentGatewayConfiguration;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _subscriptionPaymentExtensionDataRepository = subscriptionPaymentExtensionDataRepository;
            _paymentAppService = paymentAppService;
        }

        [RemoteService(false)]
        public async Task ConfirmPayment(StripeConfirmPaymentInput input)
        {
            var paymentId = await _subscriptionPaymentExtensionDataRepository.GetPaymentIdOrNullAsync(
                StripeGatewayManager.StripeSessionIdSubscriptionPaymentExtensionDataKey,
                input.StripeSessionId
            );

            if (!paymentId.HasValue)
            {
                throw new ApplicationException($"Cannot find any payment with sessionId {input.StripeSessionId}");
            }

            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId.Value);

            if (payment.Status != SubscriptionPaymentStatus.NotPaid)
            {
                throw new ApplicationException(
                    $"Invalid payment status {payment.Status}, cannot create a charge on stripe !"
                );
            }

            payment.Gateway = SubscriptionPaymentGatewayType.Stripe;

            var newEditionId = payment.EditionId;

            var service = new SessionService();
            var session = await service.GetAsync(input.StripeSessionId);

            if (session.Mode == "payment")
            {
                payment.ExternalPaymentId = session.PaymentIntentId;
            }
            else if (session.Mode == "subscription")
            {
                payment.ExternalPaymentId = session.SubscriptionId;
            }
            else
            {
                throw new ApplicationException(
                    $"Unexpected session mode {session.Mode}. 'payment' or 'subscription' expected");
            }

            payment.SetAsPaid();
            
            var tenant = await TenantManager.GetByIdAsync(payment.TenantId);
            if (tenant != null && payment.IsRecurring)
            {
                tenant.SubscriptionPaymentType = SubscriptionPaymentType.RecurringAutomatic;
                await TenantManager.UpdateAsync(tenant);
            }
            
            await CurrentUnitOfWork.SaveChangesAsync();

            if (payment.IsProrationPayment())
            {
                await ConfirmSubscriptionUpgradeProrationPayment(newEditionId, payment.TenantId);
            }

            await CompletePayment(paymentId.Value);
        }

        private async Task ConfirmSubscriptionUpgradeProrationPayment(int newEditionId, int tenantId)
        {
            await _stripeGatewayManager.UpdateSubscription(newEditionId, tenantId, true);
        }

        private async Task CompletePayment(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);

            switch (payment.EditionPaymentType)
            {
                case EditionPaymentType.BuyNow:
                    await _paymentAppService.BuyNowSucceed(paymentId);
                    break;
                case EditionPaymentType.Upgrade:
                    await _paymentAppService.UpgradeSucceed(paymentId);
                    break;
                case EditionPaymentType.Extend:
                    await _paymentAppService.ExtendSucceed(paymentId);
                    break;
                case EditionPaymentType.NewRegistration:
                    await _paymentAppService.NewRegistrationSucceed(paymentId);
                    break;
                default:
                    throw new ApplicationException(
                        $"Unhandled payment type: {payment.EditionPaymentType}. payment(id: {paymentId}) could not be completed.");
            }
        }

        public StripeConfigurationDto GetConfiguration()
        {
            return new StripeConfigurationDto
            {
                PublishableKey = _stripePaymentGatewayConfiguration.PublishableKey
            };
        }

        public async Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input)
        {
            var paymentId = await _subscriptionPaymentExtensionDataRepository.GetPaymentIdOrNullAsync(
                StripeGatewayManager.StripeSessionIdSubscriptionPaymentExtensionDataKey,
                input.StripeSessionId
            );
            
            if (!paymentId.HasValue)
            {
                throw new ApplicationException($"Cannot find any payment with sessionId {input.StripeSessionId}");
            }

            return ObjectMapper.Map<SubscriptionPaymentDto>(
                await _subscriptionPaymentRepository.GetAsync(paymentId.Value)
            );
        }

        public async Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(input.PaymentId);

            var paymentTypes = _stripePaymentGatewayConfiguration.PaymentMethodTypes;
            var sessionCreateOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = paymentTypes,
                SuccessUrl = input.SuccessUrl + (input.SuccessUrl.Contains("?") ? "&" : "?") +
                             "sessionId={CHECKOUT_SESSION_ID}",
                CancelUrl = input.CancelUrl
            };
            
            if (payment.IsRecurring && !payment.IsProrationPayment())
            {
                var plan = await _stripeGatewayManager.GetOrCreatePlanForPayment(input.PaymentId);

                sessionCreateOptions.SubscriptionData = new SessionSubscriptionDataOptions
                {
                    Items = new List<SessionSubscriptionDataItemOptions>
                    {
                        new SessionSubscriptionDataItemOptions
                        {
                            Plan = plan.Id,
                        }
                    }
                };
            }
            else
            {
                sessionCreateOptions.LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Amount = (long) _stripeGatewayManager.ConvertToStripePrice(payment.Amount),
                        Name = StripeGatewayManager.ProductName,
                        Currency = AppFrameworkConsts.Currency,
                        Description = payment.Description,
                        Quantity = 1
                    }
                };
            }

            var service = new SessionService();
            var session = await service.CreateAsync(sessionCreateOptions);

            await _subscriptionPaymentExtensionDataRepository.SetExtensionDataAsync(
                payment.Id,
                StripeGatewayManager.StripeSessionIdSubscriptionPaymentExtensionDataKey,
                session.Id
            );

            return session.Id;
        }

        public async Task<StripePaymentResultOutput> GetPaymentResult(StripePaymentResultInput input)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(input.PaymentId);
            var sessionId = await _subscriptionPaymentExtensionDataRepository.GetExtensionDataAsync(input.PaymentId,
                StripeGatewayManager.StripeSessionIdSubscriptionPaymentExtensionDataKey);

            if (string.IsNullOrEmpty(sessionId))
            {
                throw new UserFriendlyException(L("ThereIsNoStripeSessionIdOnPayment", input.PaymentId));
            }

            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var tenant = await TenantManager.GetByIdAsync(payment.TenantId);
                await _stripeGatewayManager.UpdateCustomerDescriptionAsync(sessionId, tenant.TenancyName);
            }
            
            if (payment.Status == SubscriptionPaymentStatus.Completed)
            {
                return new StripePaymentResultOutput
                {
                    PaymentDone = true
                };
            }

            return new StripePaymentResultOutput
            {
                PaymentDone = false
            };
        }
    }
}
