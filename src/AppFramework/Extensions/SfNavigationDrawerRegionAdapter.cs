using Prism.Regions;
using Syncfusion.UI.Xaml.NavigationDrawer;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace AppFramework.Extensions
{
    public class SfNavigationDrawerRegionAdapter : RegionAdapterBase<SfNavigationDrawer>
    {
        public SfNavigationDrawerRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, SfNavigationDrawer regionTarget)
        {
            if (regionTarget == null)
                throw new ArgumentNullException(nameof(regionTarget));

            bool contentIsSet = regionTarget.ContentView != null;
            contentIsSet = contentIsSet || null != BindingOperations.GetBinding(regionTarget, ContentControl.ContentProperty);

            if (contentIsSet)
                throw new InvalidOperationException("ContentControlHasContentException");

            region.ActiveViews.CollectionChanged += delegate
            {
                regionTarget.ContentView = region.ActiveViews.FirstOrDefault(); 
            };

            region.Views.CollectionChanged +=
                (sender, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add && region.ActiveViews.Count() == 0)
                    {
                        region.Activate(e.NewItems[0]);
                    }
                };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}