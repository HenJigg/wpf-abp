using AppFramework.ApiClient;
using Prism.Ioc;
using System;
using System.Globalization;

namespace AppFramework.Shared
{
    public static class Local
    {
        private static readonly Lazy<IApplicationContext> appContext =
         new Lazy<IApplicationContext>(
         ContainerLocator.Container.Resolve<IApplicationContext>);

        public static string Localize(string text)
        {
            return LocalizeInternal(text);
        }

        public static string Localize(string text, params object[] args)
        {
            return string.Format(LocalizeInternal(text), args);
        }

        public static string LocalizeWithThreeDots(string text, params object[] args)
        {
            var localizedText = Localize(text, args);
            return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft ? "..." + localizedText : localizedText + "...";
        }

        public static string LocalizeWithParantheses(string text, object valueWithinParentheses, params object[] args)
        {
            var localizedText = Localize(text);
            return CultureInfo.CurrentCulture.TextInfo.IsRightToLeft
                ? " (" + valueWithinParentheses + ")" + localizedText
                : localizedText + " (" + valueWithinParentheses + ")";
        }

        private static string LocalizeInternal(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (appContext.Value.Configuration == null)
                return text;

            return appContext.Value.Configuration.Localization.Localize(text);
        }
    }
}