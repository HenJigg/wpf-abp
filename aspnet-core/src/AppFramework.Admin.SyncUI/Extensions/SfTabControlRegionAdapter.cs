using Prism.Regions;
using Syncfusion.Windows.Tools.Controls;
using System; 

namespace AppFramework.Admin.SyncUI
{
    public class SfTabControlRegionAdapter : RegionAdapterBase<TabControlExt>
    {
        public SfTabControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        { }

        protected override void Adapt(IRegion region, TabControlExt regionTarget)
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