namespace AppFramework.Editions
{
    public enum EditionPaymentType
    {
        /// <summary>
        /// Payment on first tenant registration.
        /// </summary>
        NewRegistration = 0,

        /// <summary>
        /// Purchasing by an existing tenant that currently using trial version of a paid edition.
        /// </summary>
        BuyNow = 1,

        /// <summary>
        /// A tenant is upgrading it's edition (either from a free edition or from a low-price paid edition).
        /// </summary>
        Upgrade = 2,

        /// <summary>
        /// A tenant is extending it's current edition (without changing the edition).
        /// </summary>
        Extend = 3
    }
}