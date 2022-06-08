using System;
using System.Linq;
using System.Windows;

namespace AppFramework.Services
{
    public class ResourceService : IResourceService
    {
        public void AddResources(ResourceDictionary Resource)
        {
            Resource.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/AppFramework;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        public void UpdateResources(ResourceDictionary Resource, string themeName)
        { 
            var lastResource = Resource.MergedDictionaries.LastOrDefault();
            Resource.MergedDictionaries.Clear();
            if (lastResource != null) Resource.MergedDictionaries.Add(lastResource); 

            AddResources(Resource); 
        } 
    }
}