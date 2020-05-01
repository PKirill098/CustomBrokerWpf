using System;
using System.Data;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class ChildRelationConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DataRowView)
                return (value as DataRowView).CreateChildView(parameter.ToString());
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
