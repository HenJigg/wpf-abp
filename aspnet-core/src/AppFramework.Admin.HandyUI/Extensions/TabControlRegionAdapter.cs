using AppFramework.Admin.HandyUI.Themes.Controls;
using Prism.Regions; 
using System; 

namespace AppFramework.Admin.HandyUI
{
    internal class TabControlRegionAdapter : RegionAdapterBase<TabControl>
    {
        public TabControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        { }

        protected override void Adapt(IRegion region, TabControl regionTarget)
        {
            if (region == null)
                throw new ArgumentNullException(nameof(region));

            if (regionTarget == null)
                throw new ArgumentNullException(nameof(regionTarget));
             
            regionTarget.ItemsSource = region.Views;
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}