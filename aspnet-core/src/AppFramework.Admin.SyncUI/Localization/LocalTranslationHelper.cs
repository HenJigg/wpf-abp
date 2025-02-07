using Prism.Ioc;
using System;
using System.Reflection;
using System.Resources;

namespace AppFramework.Shared
{
    public static class LocalTranslationHelper
    {
        private static readonly Lazy<ILocaleCulture> locale =
        new Lazy<ILocaleCulture>(
        ContainerLocator.Container.Resolve<ILocaleCulture>);

        private const string ResourceId = "AppFramework.Localization.Resources.LocalTranslation";

        public static string Localize(string key)
        {
            return GetValue(key) ?? key;
        }

        private static string GetValue(string key)
        {
            var cultureInfo = locale.Value.GetCurrentCultureInfo();
            var resourceManager = new ResourceManager(ResourceId, typeof(LocalTranslationHelper).GetTypeInfo().Assembly);
            return resourceManager.GetString(key, cultureInfo);
        }
    }
}