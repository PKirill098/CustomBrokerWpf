using System;
using System.Windows.Data;
using System.Windows.Media;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class AlternationBackground : IValueConverter
    {
        int _lastPropertyValueInt;
        byte _lastbrushnumber;
        SolidColorBrush _firstbrush = System.Windows.Media.Brushes.White;
        SolidColorBrush _secondbrush = Brushes.Silver;
        SolidColorBrush _lastbrush;

        public AlternationBackground()
        {
            Reset();
        }
        public void Reset()
        {
            _lastbrushnumber = 0;
            _lastbrush = _firstbrush;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() == typeof(int))
            {
                int currentValue = (int)value;
                if (currentValue != _lastPropertyValueInt)
                {
                    if (_lastbrushnumber != 1)
                    {
                        _lastbrushnumber = 1;
                        _lastbrush = _firstbrush;
                    }
                    else
                    {
                        _lastbrushnumber = 2;
                        _lastbrush = _secondbrush;
                    }
                    _lastPropertyValueInt = currentValue;
                }
            }
            return _lastbrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
