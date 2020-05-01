using System;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class BooleanClipBoardConverter:IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() == typeof(bool))
                return (bool)value ? "Да" : "Нет";
            else
                return value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool b;
            b=string.Compare(value.ToString().ToLower(),"да")==0;
            if (!b) b = string.Compare(value.ToString(), "1") == 0;
            if (!b) b = string.Compare(value.ToString(), "-1") == 0;
            return b;
        }
    }
}
