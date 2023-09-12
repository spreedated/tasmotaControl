using neXn.Lib.Maui.ViewLogic;
using System;
using System.Globalization;
using System.Linq;

namespace TasCon.ViewLogic.Converters
{
    public class LastOctett : ValueConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string lo = (string)value;

            if (string.IsNullOrEmpty(lo) || lo.Count(x => x == '.') != 4)
            {
                return null;
            }

            return lo[(lo.LastIndexOf('.') + 1)..];
        }
    }
}
