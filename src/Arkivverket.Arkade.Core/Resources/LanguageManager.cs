using System.Globalization;

namespace Arkivverket.Arkade.Core.Languages
{
    public static class LanguageManager
    {
        public static bool TryParseFromString(string cultureInfoName, out SupportedLanguages? supportedLanguage)
        {
            switch (cultureInfoName)
            {
                case "en":
                    supportedLanguage = SupportedLanguages.English;
                    return true;
                case "nb":
                    supportedLanguage = SupportedLanguages.Norsk_BM;
                    return true;
            }

            supportedLanguage = null;
            return false;
        }

        internal static void SetResourcesCultureForTesting(CultureInfo cultureInfo)
        {
            Resources.ArkadeTestDisplayNames.Culture = cultureInfo;
            Resources.AddmlMessages.Culture = cultureInfo;
            Resources.ExceptionMessages.Culture = cultureInfo;
            Resources.Messages.Culture = cultureInfo;
            Resources.Noark5Messages.Culture = cultureInfo;
            Resources.Noark5TestDescriptions.Culture = cultureInfo;
            Resources.OutputFileNames.Culture = cultureInfo;
            Resources.Report.Culture = cultureInfo;
        }

        internal static void SetResourceCultureForPackageCreation(CultureInfo cultureInfo)
        {
            Resources.ArkadeTestDisplayNames.Culture = cultureInfo;
            Resources.AddmlMessages.Culture = cultureInfo;
            Resources.ExceptionMessages.Culture = cultureInfo;
            Resources.FormatAnalysisResultFileContent.Culture = cultureInfo;
            Resources.Messages.Culture = cultureInfo;
            Resources.OutputFileNames.Culture = cultureInfo;
        }

        internal static void SetResourceCultureForStandalonePronomAnalysis(CultureInfo cultureInfo)
        {
            Resources.FormatAnalysisResultFileContent.Culture = cultureInfo;
            Resources.OutputFileNames.Culture = cultureInfo;
        }
    }

    public enum SupportedLanguages
    {
        English,
        Norsk_BM,
    }
}
