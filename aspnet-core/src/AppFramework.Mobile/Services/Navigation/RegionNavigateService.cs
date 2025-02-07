using Prism.Regions;
using Prism.Regions.Navigation;
using System;

namespace AppFramework.Shared.Services.Navigation
{
    /// <summary>
    /// 区域导航服务
    /// </summary>
    public class RegionNavigateService : IRegionNavigateService
    {
        private readonly IRegionManager regionManager;
        private readonly IRegionNavigationJournal journal;

        public RegionNavigateService(IRegionManager regionManager,
            IRegionNavigationJournal journal)
        {
            this.regionManager = regionManager;
            this.journal = journal;
        }

        public void Clear()
        {
            journal.Clear();
        }

        public void GoBack()
        {
            if (journal.CanGoBack) journal.GoBack();
        }

        public void GoForward()
        {
            if (journal.CanGoForward) journal.GoForward();
        }

        public void Navigate(string regionName, string pageName)
        {
            regionManager.RequestNavigate(regionName, pageName, back =>
            {
#if DEBUG
                if (!(bool)back.Result)
                {
                    System.Diagnostics.Debug.WriteLine($"Navigate Error,ex:{back.Error.Message}");
                }
#endif
            });
        }
    }
}