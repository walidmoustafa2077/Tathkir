using System.Globalization;
using Tathkīr_WPF.Services.StartupServices.Interfaces;

namespace Tathkīr_WPF.Services.StartupServices
{
    public class CultureService : ICultureService
    {
        public const string CultureAr = "ar-SA";
        public const string CultureEn = "en-US";

        public bool IsRightToLeft { get; private set; }

        public void SetCulture(string language)
        {
            string cultureCode = language == Strings.Arabic ? CultureAr : CultureEn;
            var ci = new CultureInfo(cultureCode);

            ci.NumberFormat.NativeDigits = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            ci.NumberFormat.DigitSubstitution = DigitShapes.None;
            ci.NumberFormat.NumberDecimalSeparator = ".";
            ci.NumberFormat.NumberGroupSeparator = ",";
            ci.DateTimeFormat.Calendar = new GregorianCalendar();

            Strings.Culture = ci;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;

            IsRightToLeft = cultureCode.StartsWith("ar", StringComparison.OrdinalIgnoreCase);
        }
    }

}
