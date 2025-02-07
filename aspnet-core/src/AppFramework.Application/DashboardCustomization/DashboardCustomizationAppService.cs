using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.UI;
using AppFramework.Configuration;
using AppFramework.DashboardCustomization.Definitions;
using AppFramework.DashboardCustomization.Dto;
using Newtonsoft.Json;

namespace AppFramework.DashboardCustomization
{
    [AbpAuthorize]
    public class DashboardCustomizationAppService : AppFrameworkAppServiceBase, IDashboardCustomizationAppService
    {
        private readonly DashboardConfiguration _dashboardConfiguration;
        private readonly IIocResolver _iocResolver;

        public DashboardCustomizationAppService(DashboardConfiguration dashboardConfiguration, IIocResolver iocResolver)
        {
            _dashboardConfiguration = dashboardConfiguration;
            _iocResolver = iocResolver;
        }

        public async Task<Dashboard> GetUserDashboard(GetDashboardInput input)
        {
            return await GetDashboardWithAuthorizedWidgets(input.Application, input.DashboardName);
        }

        public async Task SavePage(SavePageInput input)
        {
            var dashboard = await GetDashboardWithAuthorizedWidgets(input.Application, input.DashboardName);

            foreach (var inputPage in input.Pages)
            {
                var page = dashboard.Pages.FirstOrDefault(p => p.Id == inputPage.Id);
                var pageIndex = dashboard.Pages.IndexOf(page);

                dashboard.Pages.RemoveAt(pageIndex);

                if (page != null)
                {
                    inputPage.Name = page.Name;
                    dashboard.Pages.Insert(pageIndex, inputPage);
                }
            }

            await SaveDashboardSettingForUser(input.Application, dashboard);
        }

        public async Task RenamePage(RenamePageInput input)
        {
            var dashboard = await GetDashboardWithAuthorizedWidgets(input.Application, input.DashboardName);

            var page = dashboard.Pages.FirstOrDefault(p => p.Id == input.Id);
            if (page == null)
            {
                return;
            }

            page.Name = input.Name;

            await SaveDashboardSettingForUser(input.Application, dashboard);
        }

        public async Task<AddNewPageOutput> AddNewPage(AddNewPageInput input)
        {
            var dashboard = await GetDashboardWithAuthorizedWidgets(input.Application, input.DashboardName);

            var page = new Page
            {
                Name = input.Name,
                Widgets = new List<Widget>(),
            };

            dashboard.Pages.Add(page);
            await SaveDashboardSettingForUser(input.Application, dashboard);

            return new AddNewPageOutput {PageId = page.Id};
        }

        public async Task DeletePage(DeletePageInput input)
        {
            var dashboard = await GetDashboardWithAuthorizedWidgets(input.Application, input.DashboardName);

            dashboard.Pages.RemoveAll(p => p.Id == input.Id);

            if (dashboard.Pages.Count == 0) // return to default
            {
                dashboard = await GetDefaultDashboardValue(input.Application, input.DashboardName);
            }

            await SaveDashboardSettingForUser(input.Application, dashboard);
        }

        public async Task<Widget> AddWidget(AddWidgetInput input)
        {
            var widgetDefinition =
                _dashboardConfiguration.WidgetDefinitions.FirstOrDefault(w => w.Id == input.WidgetId);
            if (widgetDefinition == null)
            {
                throw new UserFriendlyException(L("WidgetNotFound"));
            }

            var dashboard = await GetDashboardWithAuthorizedWidgets(input.Application, input.DashboardName);

            var page = dashboard.Pages.Single(p => p.Id == input.PageId);

            if (!widgetDefinition.AllowMultipleInstanceInSamePage &&
                page.Widgets.Any(w => w.WidgetId == widgetDefinition.Id))
            {
                throw new UserFriendlyException(L("WidgetCanNotBePlacedMoreThanOnceInAPageWarning"));
            }

            var widget = new Widget
            {
                WidgetId = input.WidgetId,
                Height = input.Height,
                Width = input.Width,
                PositionX = 0,
                PositionY = CalculatePositionY(page.Widgets)
            };

            page.Widgets.Add(widget);

            await SaveDashboardSettingForUser(input.Application, dashboard);
            return widget;
        }

        public DashboardOutput GetDashboardDefinition(GetDashboardInput input)
        {
            var dashboardDefinition =
                _dashboardConfiguration.DashboardDefinitions.FirstOrDefault(d => d.Name == input.DashboardName);
            if (dashboardDefinition == null)
            {
                throw new UserFriendlyException(L("UnknownDashboard", input.DashboardName));
            }

            //widgets which used in that dashboard
            var usedWidgetDefinitions = GetWidgetDefinitionsFilteredPermissionAndMultiTenancySide(dashboardDefinition);

            List<WidgetFilterOutput> GetNeededWidgetFiltersOutput(WidgetDefinition widget)
            {
                if (widget.UsedWidgetFilters == null || !widget.UsedWidgetFilters.Any())
                {
                    return new List<WidgetFilterOutput>();
                }

                var allNeededFilters = widget.UsedWidgetFilters.Distinct().ToList();

                return _dashboardConfiguration.WidgetFilterDefinitions
                    .Where(definition => allNeededFilters.Contains(definition.Id))
                    .Select(x => new WidgetFilterOutput(x.Id, x.Name))
                    .ToList();
            }

            return new DashboardOutput(
                dashboardDefinition.Name,
                usedWidgetDefinitions
                    .Select(widget => new WidgetOutput(
                        widget.Id,
                        widget.Name,
                        widget.Description,
                        filters: GetNeededWidgetFiltersOutput(widget))
                    ).ToList()
            );
        }

        public List<WidgetOutput> GetAllWidgetDefinitions(GetDashboardInput input)
        {
            var dashboardDefinition =
                _dashboardConfiguration.DashboardDefinitions.FirstOrDefault(d => d.Name == input.DashboardName);
            if (dashboardDefinition == null)
            {
                throw new UserFriendlyException(L("UnknownDashboard", input.DashboardName));
            }

            return GetWidgetDefinitionsFilteredPermissionAndMultiTenancySide(dashboardDefinition)
                .Select(widget => new WidgetOutput(widget.Id, widget.Name, widget.Description)).ToList();
        }

        public async Task<List<WidgetOutput>> GetAllAvailableWidgetDefinitionsForPage(
            GetAvailableWidgetDefinitionsForPageInput input)
        {
            var dashboardDefinition =
                _dashboardConfiguration.DashboardDefinitions.FirstOrDefault(d => d.Name == input.DashboardName);
            if (dashboardDefinition == null)
            {
                throw new UserFriendlyException(L("UnknownDashboard", input.DashboardName));
            }

            var dashboard = await GetUserDashboard(new GetDashboardInput()
            {
                Application = input.Application,
                DashboardName = input.DashboardName
            });

            var page = dashboard.Pages.FirstOrDefault(p => p.Id == input.PageId);
            if (page == null)
            {
                throw new UserFriendlyException(L("UnknownPage"));
            }

            var widgetsAlreadyInPage = page.Widgets.Select(w => w.WidgetId).ToList();

            return GetWidgetDefinitionsFilteredPermissionAndMultiTenancySide(dashboardDefinition)
                .Where(widgetDefinition =>
                    widgetDefinition.AllowMultipleInstanceInSamePage ||
                    !widgetsAlreadyInPage.Contains(widgetDefinition.Id)
                )
                .Select(widget => new WidgetOutput(widget.Id, widget.Name, widget.Description)).ToList();
        }

        private async Task<Dashboard> GetDashboardWithAuthorizedWidgets(string application, string dashboardName)
        {
            var dashboardConfigAsJsonString =
                await SettingManager.GetSettingValueAsync(GetSettingName(application, dashboardName));

            if (string.IsNullOrWhiteSpace(dashboardConfigAsJsonString))
            {
                return null;
            }

            var dashboard = JsonConvert.DeserializeObject<Dashboard>(dashboardConfigAsJsonString);
            if (dashboard == null)
            {
                throw new UserFriendlyException(L("UnknownDashboard", dashboardName));
            }

            var dashboardDefinition =
                _dashboardConfiguration.DashboardDefinitions.FirstOrDefault(d => d.Name == dashboardName);
            if (dashboardDefinition == null)
            {
                throw new UserFriendlyException(L("UnknownDashboard", dashboardName));
            }

            //widgets which used in that dashboard
            var authorizedWidgetIdsForDashboardAndUser =
                GetWidgetDefinitionsFilteredPermissionAndMultiTenancySide(dashboardDefinition)
                    .Select(definition => definition.Id).ToList();

            //if user's permission changed, we should remove all widgets which are not allowed for user
            foreach (var dashboardPage in dashboard.Pages)
            {
                dashboardPage.Widgets = dashboardPage.Widgets
                    .Where(widget => authorizedWidgetIdsForDashboardAndUser.Contains(widget.WidgetId)).ToList();
            }

            return dashboard;
        }

        private async Task SaveDashboardSettingForUser(string application, Dashboard dashboard)
        {
            if (dashboard == null)
            {
                return;
            }

            //check if dashboard is available for user to prevent saving unauthorized widgets to dashboard
            var dashboardDefinition =
                _dashboardConfiguration.DashboardDefinitions.FirstOrDefault(d => d.Name == dashboard.DashboardName);
            if (dashboardDefinition == null)
            {
                throw new UserFriendlyException(L("UnknownDashboard", dashboard.DashboardName));
            }

            var authorizedWidgetIdsForDashboardAndUser =
                GetWidgetDefinitionsFilteredPermissionAndMultiTenancySide(dashboardDefinition)
                    .Select(definition => definition.Id)
                    .ToList();

            var allWidgetsOfDashboard = dashboard.Pages.SelectMany(p => p.Widgets)
                .DistinctBy(widget => widget.WidgetId)
                .Select(widget => widget.WidgetId)
                .ToList();

            if (allWidgetsOfDashboard.Any(widgetId => !authorizedWidgetIdsForDashboardAndUser.Contains(widgetId)))
            {
                var unknownWidgetId = allWidgetsOfDashboard.First(widget =>
                    !authorizedWidgetIdsForDashboardAndUser.Contains(widget));
                throw new UserFriendlyException(L("UnknownWidgetId", unknownWidgetId));
            }

            //we can save dashboard now since it is authorized for user
            var dashboardJson = JsonConvert.SerializeObject(dashboard);

            var currentUser = await GetCurrentUserAsync();
            await SettingManager.ChangeSettingForUserAsync(currentUser.ToUserIdentifier(),
                GetSettingName(application, dashboard.DashboardName), dashboardJson);
        }

        private byte CalculatePositionY(List<Widget> widgets)
        {
            if (widgets == null || !widgets.Any())
            {
                return 0;
            }

            return (byte) widgets.Max(w => w.PositionY + w.Height);
        }

        private async Task<Dashboard> GetDefaultDashboardValue(string application, string dashboardName)
        {
            string dashboardConfigAsJsonString;

            if (AbpSession.MultiTenancySide == MultiTenancySides.Host)
            {
                dashboardConfigAsJsonString =
                    await SettingManager.GetSettingValueForApplicationAsync(GetSettingName(application, dashboardName));
            }
            else
            {
                dashboardConfigAsJsonString =
                    await SettingManager.GetSettingValueForTenantAsync(GetSettingName(application, dashboardName),
                        AbpSession.GetTenantId());
            }

            return string.IsNullOrWhiteSpace(dashboardConfigAsJsonString)
                ? null
                : JsonConvert.DeserializeObject<Dashboard>(dashboardConfigAsJsonString);
        }

        private List<WidgetDefinition> GetWidgetDefinitionsFilteredPermissionAndMultiTenancySide(
            DashboardDefinition dashboardDefinition)
        {
            var dashboardWidgets = dashboardDefinition.AvailableWidgets ?? new List<string>();

            var widgetDefinitions = _dashboardConfiguration.WidgetDefinitions
                .Where(wd => dashboardWidgets.Contains(wd.Id)).ToList();

            //filter for multi-tenancy side
            widgetDefinitions = widgetDefinitions.Where(w => w.Side.HasFlag(AbpSession.MultiTenancySide)).ToList();

            //filter for permissions
            var filteredWidgets = new List<WidgetDefinition>();

            using (var scope = _iocResolver.CreateScope())
            {
                var permissionDependencyContext = scope.Resolve<PermissionDependencyContext>();
                permissionDependencyContext.User = AbpSession.ToUserIdentifier();

                var featureDependencyContext = scope.Resolve<FeatureDependencyContext>();
                featureDependencyContext.TenantId = AbpSession.TenantId;

                foreach (var widget in widgetDefinitions)
                {
                    if (widget.PermissionDependency != null &&
                        (!widget.PermissionDependency.IsSatisfied(permissionDependencyContext)))
                    {
                        continue;
                    }
                    
                    if (widget.FeatureDependency != null &&
                        (!widget.FeatureDependency.IsSatisfied(featureDependencyContext)))
                    {
                        continue;
                    }
                    
                    filteredWidgets.Add(widget);
                }
            }

            return filteredWidgets;
        }

        public string GetSettingName(string application, string dashboardName)
        {
            return AppSettings.DashboardCustomization.Configuration + "." + application + "." + dashboardName;
        }
    }
}