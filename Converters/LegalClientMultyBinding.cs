using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class LegalClientMultyBinding: System.Windows.Data.IMultiValueConverter

    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == null || values[0].Equals(System.Windows.DependencyProperty.UnsetValue))
                return null;
            else if ((int)values[0] == 0)
                return values[1];
            else if ((int)values[0] == 1)
                return values[2];

            return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] splitValues = ((string)value).Split(' ');
            return splitValues;
        }

    }
}
