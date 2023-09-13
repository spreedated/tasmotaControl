using neXn.Lib.Maui.ViewLogic;
using System;
using System.Globalization;

namespace TasCon.ViewLogic.Converters
{
    public class IsStringNullOrEmpty : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value);
        }
    }
}
