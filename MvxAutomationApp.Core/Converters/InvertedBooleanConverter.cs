using MvvmCross.Converters;
using System;
using System.Globalization;

namespace MvxAutomationApp.Core.Converters
{
    public class InvertedBooleanConverter : MvxValueConverter<bool, bool>
    {
        protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture) => !value;
    }
}