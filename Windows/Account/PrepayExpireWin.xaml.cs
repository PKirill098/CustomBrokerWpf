using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using System.Windows;
using System.Windows.Controls;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Windows.Account
{
    /// <summary>
    /// Логика взаимодействия для PrepayExpireWin.xaml
    /// </summary>
    public partial class PrepayExpireWin : Window
    {
        private lib.BindingDischarger mybinddisp;
        private PrepayExpire mycmd;

        public PrepayExpireWin()
        {
            InitializeComponent();
        }
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is PrepayExpire)
            {
                mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.MainDataGrid });
                mycmd = e.NewValue as PrepayExpire;
                mycmd.CancelEdit = mybinddisp.CancelEdit;
                mycmd.EndEdit = mybinddisp.EndEdit;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!e.Cancel) (App.Current.MainWindow as AccountMainWin).ListChildWindow.Remove(this);
            App.Current.MainWindow.Activate();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void RequestFolderOpen_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button && (sender as Button).Tag is Classes.Domain.Account.PrepayCustomerRequestVM)
            {
                try
                {
                    Classes.Domain.RequestVM item = ((sender as Button).Tag as PrepayCustomerRequestVM).Request;
                    item.DomainObject.DocFolderOpen();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Папка документов");
                }
            }
        }
        private void PrepayCurrencyBuyButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            lib.DomainBaseStamp prepay;
            if (((sender as Button).Tag as PrepayCustomerRequestVM).Prepay.CBRate.HasValue)
                prepay = ((sender as Button).Tag as PrepayCustomerRequestVM).Prepay;
            else
                prepay = ((sender as Button).Tag as PrepayCustomerRequestVM).CustomsInvoice;
            foreach (Window item in this.OwnedWindows)
                if (item.Name == "winPrepayCurrencyBuy" && ((item.DataContext as PrepayCurrencyBuyViewCommand).Prepay == prepay || (item.DataContext as PrepayCurrencyBuyViewCommand).Invoice == prepay))
                    ObjectWin = item;
            if (ObjectWin == null)
            {
                ObjectWin = new PrepayCurrencyBuyWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = new Classes.Domain.Account.PrepayCurrencyBuyViewCommand(prepay);
                ObjectWin.WindowState = WindowState.Normal;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void PrepayCurrencyPayButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            Prepay prepay = ((sender as Button).Tag as PrepayCustomerRequestVM).Prepay;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winPrepayCurrencyPay" && (item.DataContext as PrepayCurrencyPayViewCommand).Prepay == prepay) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new PrepayCurrencyPayWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = new Classes.Domain.Account.PrepayCurrencyPayViewCommand(prepay);
                ObjectWin.WindowState = WindowState.Normal;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void FinalCurPaidDate1Button_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            CustomsInvoice invoice = ((sender as Button).Tag as PrepayCustomerRequestVM).CustomsInvoice;
            if (invoice == null) return;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winFinalInvoiceCurPay" && (item.DataContext as CustomsInvoicePayFinalCur1ViewCommand)?.Invoice == invoice) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new WindowsAccount.FinalInvoiceCurPayWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = new Classes.Domain.Account.CustomsInvoicePayFinalCur1ViewCommand(invoice);
                ObjectWin.WindowState = WindowState.Normal;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void FinalCurPaidDate2Button_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            CustomsInvoice invoice = ((sender as Button).Tag as PrepayCustomerRequestVM).CustomsInvoice;
            if (invoice == null) return;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winFinalInvoiceCurPay" && (item.DataContext as CustomsInvoicePayFinalCur2ViewCommand)?.Invoice == invoice) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new WindowsAccount.FinalInvoiceCurPayWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = new Classes.Domain.Account.CustomsInvoicePayFinalCur2ViewCommand(invoice);
                ObjectWin.WindowState = WindowState.Normal;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
    }
}
