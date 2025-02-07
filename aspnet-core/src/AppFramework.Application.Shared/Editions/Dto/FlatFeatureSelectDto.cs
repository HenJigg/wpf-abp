using Abp.UI.Inputs;

namespace AppFramework.Editions.Dto
{
    public class FlatFeatureSelectDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public IInputType InputType { get; set; }

        public string TextHtmlColor { get; set; }
    }
}