using System.Globalization;
using System.Threading;

namespace Arkivverket.Arkade.Core.Languages
{
    public static class LanguageManager
    {
        internal static void SetResourcesCultureForTesting(string outputLanguage)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(outputLanguage);
            
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            
            Resources.ArkadeTestDisplayNames.Culture = cultureInfo;
            Resources.AddmlMessages.Culture = cultureInfo;
            Resources.ExceptionMessages.Culture = cultureInfo;
            Resources.Messages.Culture = cultureInfo;
            Resources.Noark5Messages.Culture = cultureInfo;
            Resources.Noark5TestDescriptions.Culture = cultureInfo;
            Resources.OutputFileNames.Culture = cultureInfo;
            Resources.Report.Culture = cultureInfo;
        }

        internal static void SetResourceCultureForPackageCreation(string outputLanguage)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(outputLanguage);

            Resources.ArkadeTestDisplayNames.Culture = cultureInfo;
            Resources.AddmlMessages.Culture = cultureInfo;
            Resources.ExceptionMessages.Culture = cultureInfo;
            Resources.FormatAnalysisResultFileContent.Culture = cultureInfo;
            Resources.Messages.Culture = cultureInfo;
            Resources.OutputFileNames.Culture = cultureInfo;
        }

        internal static void SetResourceCultureForStandalonePronomAnalysis(string outputLanguage)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(outputLanguage);

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
