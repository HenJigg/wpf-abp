using Prism.Ioc;
using System;

namespace AppFramework.Shared
{
    public static class LocalTranslationHelper
    {
        private static readonly Lazy<ILocaleCulture> locale = new Lazy<ILocaleCulture>(
        ContainerLocator.Container.Resolve<ILocaleCulture>);

        public static string Localize(string key)
        {
            return GetValue(key) ?? key;
        }

        private static string GetValue(string key)
        {
            return locale.Value.GetString(key);
        }
    }
}