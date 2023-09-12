using neXn.Lib.Maui.ViewLogic;
using System;
using System.Globalization;

namespace TasCon.ViewLogic.Converters
{
    public class BoolToOnOffString : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "On" : "Off";
        }
    }
}
