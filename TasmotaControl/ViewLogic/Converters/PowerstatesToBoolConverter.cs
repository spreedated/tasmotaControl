using neXn.Lib.Maui.ViewLogic;
using System;
using System.Globalization;
using TasCon.Logic;

namespace TasCon.ViewLogic.Converters
{
    public class PowerstatesToBoolConverter : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (TasmotaDevice.Powerstates)value == TasmotaDevice.Powerstates.On;
        }
    }
}