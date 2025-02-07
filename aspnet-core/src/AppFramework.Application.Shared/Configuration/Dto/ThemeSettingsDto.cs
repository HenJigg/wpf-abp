namespace AppFramework.Configuration.Dto
{
    public class ThemeSettingsDto
    {
        public string Theme { get; set; }

        public ThemeLayoutSettingsDto Layout { get; set; } = new ThemeLayoutSettingsDto();

        public ThemeHeaderSettingsDto Header { get; set; } = new ThemeHeaderSettingsDto();

        public ThemeSubHeaderSettingsDto SubHeader { get; set; } = new ThemeSubHeaderSettingsDto();

        public ThemeMenuSettingsDto Menu { get; set; } = new ThemeMenuSettingsDto();

        public ThemeFooterSettingsDto Footer { get; set; } = new ThemeFooterSettingsDto();
    }
}