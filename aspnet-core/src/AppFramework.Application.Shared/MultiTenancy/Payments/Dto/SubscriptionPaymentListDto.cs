using Abp.Application.Services.Dto;

namespace AppFramework.MultiTenancy.Payments.Dto
{
    public class SubscriptionPaymentListDto: AuditedEntityDto
    {
        public string Gateway { get; set; }

        public decimal Amount { get; set; }

        public int EditionId { get; set; }

        public int DayCount { get; set; }

        public string PaymentPeriodType { get; set; }

        public string ExternalPaymentId { get; set; }

        public string PayerId { get; set; }

        public string Status { get; set; }

        public string EditionDisplayName { get; set; }

        public int TenantId { get; set; }

        public string InvoiceNo { get; set; }

    }
}
