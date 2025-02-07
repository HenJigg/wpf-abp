namespace AppFramework.DashboardCustomization.Dto
{
    public class AddWidgetInput
    {
        public string WidgetId { get; set; }

        public string PageId { get; set; }

        public string DashboardName { get; set; }

        public byte Width { get; set; }

        public byte Height { get; set; }

        public string Application { get; set; }
    }
}