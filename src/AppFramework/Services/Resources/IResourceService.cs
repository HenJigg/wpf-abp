using System.Windows;

namespace AppFramework.Services
{
    public interface IResourceService
    {
        void AddResources(ResourceDictionary Resource);

        void UpdateResources(ResourceDictionary Resource, string themeName);
    }
}