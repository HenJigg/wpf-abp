namespace AppFramework.Shared.Models
{
    public class TopStatusItem
    { 
        public string Logo { get; set; }

        public string Title { get; set; }

        public decimal Amount { get; set; }

        public string Foreground { get; set; }

        /// <summary>
        /// Gets or sets the property that has been displays the background gradient start.
        /// </summary>
        public string BackgroundGradientStart { get; set; }

        /// <summary>
        /// Gets or sets the property that has been displays the background gradient end.
        /// </summary>
        public string BackgroundGradientEnd { get; set; }
    }
}