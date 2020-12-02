using System.Globalization;

namespace Arkivverket.Arkade.Core.Languages
{
    public static class LanguageManager
    {
        public static bool TryParseFromString(string cultureInfoName, out SupportedLanguages? supportedLanguage)
        {
            switch (cultureInfoName)
            {
                case "en-GB":
                    supportedLanguage = SupportedLanguages.English;
                    return true;
                case "nb-NO":
                    supportedLanguage = SupportedLanguages.Norsk_BM;
                    return true;
            }

            supportedLanguage = null;
            return false;
        }
    }

    public enum SupportedLanguages
    {
        English,
        Norsk_BM,
    }
}
