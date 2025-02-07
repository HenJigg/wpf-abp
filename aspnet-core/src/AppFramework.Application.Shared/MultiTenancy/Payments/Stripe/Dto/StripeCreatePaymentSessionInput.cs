namespace AppFramework.MultiTenancy.Payments.Stripe.Dto
{
    public class StripeCreatePaymentSessionInput
    {
        public long PaymentId { get; set; }

        public string SuccessUrl { get; set; }

        public string CancelUrl { get; set; }
    }
}
