using System.Globalization;
using Arkivverket.Arkade.Core.Resources;
using Arkivverket.Arkade.Core.Util;

namespace Arkivverket.Arkade.Core.Base
{
    public static class ArkadeTestNameProvider
    {
        public static string GetDisplayName(IArkadeTest arkadeTest)
        {
            return GetDisplayName(arkadeTest.GetId(), ArkadeTestDisplayNames.Culture);
        }

        public static string GetDisplayName(TestId testId, CultureInfo culture) // Use type SupportedLanguage instead of CultureInfo?
        {
            return string.Format(ArkadeTestDisplayNames.DisplayNameFormat, testId, GetTestName(testId, culture));
        }

        private static string GetTestName(TestId testId, CultureInfo culture)
        {
            string resourceDisplayNameKey = testId.ToString().Replace('.', '_');

            if (testId.Version.Equals("5.5"))
                resourceDisplayNameKey = $"{resourceDisplayNameKey}v5_5";

            return ArkadeTestDisplayNames.ResourceManager.GetString(resourceDisplayNameKey, culture);
        }
    }
}
