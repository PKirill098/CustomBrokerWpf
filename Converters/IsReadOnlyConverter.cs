using System;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class IsReadOnlyConverter : IValueConverter
    {
        int _value,_parameter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           if ((parameter != null) && (int.TryParse(parameter.ToString(), out _parameter)) & (int.TryParse(value.ToString(), out _value)))
            {
                return !(_value < _parameter);
            }
            else throw new NotImplementedException("Не обрабатываемый тип параметра конвертера!");
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IsReadOnlyHasValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value == null || value == DBNull.Value) == bool.Parse((string)parameter);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
