using System;
using System.Globalization;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    public class IntColorConverter : IValueConverter
    {
        public IntColorConverter():base()
        {
            mydefault = "White";
            mycolor1 = "Yellow";
            mycolor2 = "LightBlue";
            mycolor3 = "LightGreen";
            mycolor4 = "Pink";
        }

        private string mydefault;
        public string DefaultColor { set { mydefault = value; } }
        private string mycolor1;
        public string Color1 { set { mycolor1 = value; } }
        private string mycolor2;
        public string Color2 { set { mycolor2 = value; } }
        private string mycolor3;
        public string Color3 { set { mycolor3 = value; } }
        private string mycolor4;
        public string Color4 { set { mycolor4 = value; } }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string color = mydefault;
            switch ((int)value)
            {
                case 1:
                    color = mycolor1;
                    break;
                case 2:
                    color = mycolor2;
                    break;
                case 3:
                    color = mycolor3;
                    break;
                case 4:
                    color = mycolor4;
                    break;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
