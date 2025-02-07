namespace AppFramework.DashboardCustomization.Dto;

public class GetAvailableWidgetDefinitionsForPageInput
{
    public string DashboardName { get; set; }

    public string Application { get; set; }
    
    public string PageId { get; set; }
}
