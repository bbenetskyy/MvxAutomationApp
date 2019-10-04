using System.Globalization;

namespace MvxAutomationApp.Core.Extensions
{
    public static class ParseExtensions
    {
        public static double ParseToDouble(this string s)
            => double.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}
