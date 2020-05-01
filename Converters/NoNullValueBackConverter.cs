using System;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class NoNullValueBackConverter: IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, System.Globalization.CultureInfo cultrure)
        {
            return value;
        }
        public object ConvertBack(object value, Type TargetType, object parameter, System.Globalization.CultureInfo cultrure)
        {
            if (value != null)
            {
                return value;
            }
            else
            {
                return Binding.DoNothing;
            }
        }
    }
}
