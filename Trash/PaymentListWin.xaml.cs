using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для PaymentListWin.xaml
    /// </summary>
    public partial class PaymentListWin : Window, ISQLFiltredWindow
    {

        public PaymentListWin()
        {
            InitializeComponent();
            this.PaymentlistUC.CloseButton.Click += thisCloseButton_Click;
            this.PaymentlistUC.CloseButton.Visibility = Visibility.Visible;
            this.PaymentlistUC.CloseButtonSeparator.Visibility = Visibility.Visible;
            this.PaymentlistUC.MainMenuSeparator.Visibility = Visibility.Visible;
            this.PaymentlistUC.MainMenu.Visibility = Visibility.Visible;
        }

        private void winPaymentList_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.PaymentlistUC.SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                this.PaymentlistUC.Filter.RemoveCurrentWhere();
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void RunFilter()
        {
            this.PaymentlistUC.RunFilter();
        }
        public bool IsShowFilter
        {
            get
            {
                return this.PaymentlistUC.IsShowFilter;
            }

            set
            {
                this.PaymentlistUC.IsShowFilter=value;
            }
        }
        public SQLFilter Filter
        {
            get
            {
                return this.PaymentlistUC.Filter;
            }
            set
            {
                this.PaymentlistUC.Filter=value;
            }
        }
    }
    public class TransSunIsReadOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal d;
            decimal.TryParse(value.ToString(), out d);
            return d > 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TransSunIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal d;
            decimal.TryParse(value.ToString(), out d);
            return d == 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
