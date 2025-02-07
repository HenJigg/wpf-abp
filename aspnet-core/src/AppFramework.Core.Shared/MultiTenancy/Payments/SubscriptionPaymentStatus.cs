namespace AppFramework.MultiTenancy.Payments
{
    public enum SubscriptionPaymentStatus
    {
        NotPaid = 1,
        Paid = 2,
        Failed = 3,
        Cancelled = 4,
        Completed = 5
    }
}