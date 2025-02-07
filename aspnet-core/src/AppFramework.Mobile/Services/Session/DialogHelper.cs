using Acr.UserDialogs;
using AppFramework.Shared.Localization;

namespace AppFramework.Shared
{
    public static class DialogHelper
    {
        public static void Warn(string localizationKeyOrMessage,
            LocalizationSource localizationSource = LocalizationSource.RemoteTranslation)
        {
            UserDialogs.Instance.Alert(Localize(localizationKeyOrMessage, localizationSource), Local.Localize("Warning"));
        }

        public static void Error(string localizationKeyOrMessage,
            LocalizationSource localizationSource = LocalizationSource.RemoteTranslation)
        {
            UserDialogs.Instance.Alert(Localize(localizationKeyOrMessage, localizationSource), Local.Localize("Error"));
        }

        public static void Success(string localizationKeyOrMessage,
            LocalizationSource localizationSource = LocalizationSource.RemoteTranslation)
        {
            UserDialogs.Instance.Alert(Localize(localizationKeyOrMessage, localizationSource), Local.Localize("Success"));
        }

        private static string Localize(string localizationKeyOrMessage,
            LocalizationSource localizationSource = LocalizationSource.RemoteTranslation)
        {
            switch (localizationSource)
            {
                case LocalizationSource.RemoteTranslation:
                    return Local.Localize(localizationKeyOrMessage);

                case LocalizationSource.LocalTranslation:
                    return LocalTranslationHelper.Localize(localizationKeyOrMessage);

                case LocalizationSource.NoTranslation:
                    return localizationKeyOrMessage;

                default:
                    return localizationKeyOrMessage;
            }
        }
    }
}