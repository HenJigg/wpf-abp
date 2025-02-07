using System.Collections.Generic;

namespace AppFramework.DashboardCustomization.Dto
{
    public class WidgetOutput
    {
        public string Id { get; }

        public string Name { get; }

        public string Description { get; }

        public List<WidgetFilterOutput> Filters { get; set; }

        public WidgetOutput(string id, string name, string description, List<WidgetFilterOutput> filters = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Filters = filters;
        }
    }
}