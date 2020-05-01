using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class UpperCaseFirstConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = value.ToString().Trim();
            if (s.Length > 0)
            {
                
                string u = s.Substring(0, 1).ToUpper();
                string l=string.Empty;
                if (s.Length>1) l = s.Substring(1).ToLower();
                s = u + l;
            }
                return s;
        }
    }
}
