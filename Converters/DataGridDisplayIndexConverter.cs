using System;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public class DataGridDisplayIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;
            if(((int)value)>0)
                return (int)value+1;
            else
                return 1;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int index;
            if (int.TryParse(value.ToString(), out index))
            {
                if (index > 1)
                    return index - 1;
                else
                    return 0;
            }
            else
                return Binding.DoNothing;
        }
    }
}
