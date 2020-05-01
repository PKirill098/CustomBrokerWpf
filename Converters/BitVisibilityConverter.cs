using System;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    enum asCompare { Less, Equal, More };
    class BitVisibilityConverter :System.ComponentModel.Component, IValueConverter
    {

        int _collapsed, _hidden, _visible,_value,_parameter;

        public asCompare asCompare { set; get; }

        public int asCollapsed { set { _collapsed = value; } get { return _collapsed; } }
        public int asHidden { set { _hidden = value; } get { return _hidden; } }
        public int asVisible { set { _visible = value; } get { return _visible; } }
        
        public BitVisibilityConverter()
        { this.asCompare = asCompare.Less; }
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(this.DesignMode) return System.Windows.Visibility.Visible;
            
            if (value.GetType() == typeof(Boolean))
            {
                return (bool)value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            else if ((parameter != null) && (int.TryParse(parameter.ToString(), out _parameter)) & (int.TryParse(value.ToString(), out _value)))
            {
                if (this.asCompare == asCompare.Less)
                return _value < _parameter ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                else if (this.asCompare == asCompare.Equal)
                   return _value == _parameter ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                else if (this.asCompare == asCompare.More)
                   return _value > _parameter ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
               else throw new NotImplementedException("Не обрабатываемый тип параметра конвертера!");
            }
            else throw new NotImplementedException("Не обрабатываемый тип параметра конвертера!");
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
