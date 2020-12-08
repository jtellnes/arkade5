using System.Globalization;

namespace Arkivverket.Arkade.Core.Languages
{
    public static class LanguageManager
    {
        internal static void SetResourcesCultureForTesting(SupportedLanguage selectedLanguage)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(selectedLanguage.ToString());

            Resources.ArkadeTestDisplayNames.Culture = cultureInfo;
            Resources.AddmlMessages.Culture = cultureInfo;
            Resources.ExceptionMessages.Culture = cultureInfo;
            Resources.Messages.Culture = cultureInfo;
            Resources.Noark5Messages.Culture = cultureInfo;
            Resources.Noark5TestDescriptions.Culture = cultureInfo;
            Resources.OutputFileNames.Culture = cultureInfo;
            Resources.Report.Culture = cultureInfo;
        }

        internal static void SetResourceCultureForPackageCreation(SupportedLanguage selectedLanguage)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(selectedLanguage.ToString());

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

    public enum SupportedLanguage
    {
        en,
        nb,
    }
}
