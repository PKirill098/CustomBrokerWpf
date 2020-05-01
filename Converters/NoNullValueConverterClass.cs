using System;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class NoNullValueConverter : IValueConverter
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
    class UserRoleVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, System.Globalization.CultureInfo cultrure)
        {
            ReferenceDS refDS = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
            //refDS.tableUserRoles.FindByRoleName(
            return System.Windows.Visibility.Visible;
        }
        public object ConvertBack(object value, Type TargetType, object parameter, System.Globalization.CultureInfo cultrure)
        {
            return Binding.DoNothing;
        }
    }
}
