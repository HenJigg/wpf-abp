namespace AppFramework.DashboardCustomization.Dto
{
    public class WidgetFilterOutput
    {
        public string Id { get; }

        public string Name { get; }

        public WidgetFilterOutput(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}