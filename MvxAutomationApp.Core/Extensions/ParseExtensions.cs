using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MvxAutomationApp.Core.Extensions
{
    public static class ParseExtensions
    {
        public static double ParseToDouble(this string s)
            => double.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}
