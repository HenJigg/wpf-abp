using Abp.Application.Services.Dto;
using AppFramework.Editions;

namespace AppFramework.MultiTenancy.Payments.Dto
{
    public class SubscriptionPaymentDto : EntityDto<long>
    {
        public string Description { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }

        public decimal Amount { get; set; }

        public int EditionId { get; set; }

        public int TenantId { get; set; }

        public int DayCount { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        public string PaymentId { get; set; }

        public string PayerId { get; set; }

        public string EditionDisplayName { get; set; }

        public long InvoiceNo { get; set; }

        public SubscriptionPaymentStatus Status { get; set; }

        public bool IsRecurring { get; set; }
        
        public string ExternalPaymentId { get; set; }

        public string SuccessUrl { get; set; }

        public string ErrorUrl { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
