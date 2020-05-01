using System;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class NullDateConverter : IValueConverter
    {
        DateTime _date;
        public DateTime DefaultDate
        { set { _date = value; } }
        
        public NullDateConverter()
            :base()
        {
            _date = DateTime.Today;
        }
        public object Convert(object value, Type TargetType, object parameter, System.Globalization.CultureInfo cultrure)
        {
            if (value == null | value == DBNull.Value)
                return _date;
            else
                return value;
        }
        public object ConvertBack(object value, Type TargetType, object parameter, System.Globalization.CultureInfo cultrure)
        {
            return value;
        }
    }
}
