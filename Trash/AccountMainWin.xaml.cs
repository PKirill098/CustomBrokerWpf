using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class AccountMainWin : Window
    {
        public AccountMainWin()
        {
            InitializeComponent();
            mychildwindows = new List<Window>();
            mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.RequestTDDataGrid,this.GTDTDDataGrid,this.RequestTEODataGrid,this.GTDTEODataGrid });
        }

        private List<Window> mychildwindows;
        internal List<Window> ListChildWindow
        { get { return mychildwindows; } }
        private lib.BindingDischarger mybinddisp;
        private PaymentRegisterViewCommander mypaydcmd;
        private PaymentRegisterViewCommander mypaytcmd;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mypaydcmd = new PaymentRegisterViewCommander();
            mypaydcmd.CancelEdit = mybinddisp.CancelEdit;
            mypaydcmd.EndEdit = mybinddisp.EndEdit;
            this.PaymentDeliveryGrid.DataContext = mypaydcmd;
            mypaytcmd = new PaymentRegisterViewCommander();
            mypaytcmd.CancelEdit = mybinddisp.CancelEdit;
            mypaytcmd.EndEdit = mybinddisp.EndEdit;
            this.PaymentTradeGrid.DataContext = mypaytcmd;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = Request_Closing();
            //e.Cancel |= Parcel_Closing();
            int i = 0, c1;
            while (i < mychildwindows.Count)
            {
                c1 = mychildwindows.Count;
                mychildwindows[i].Close();
                i = i + 1 - c1 + mychildwindows.Count;
            }
            if (mychildwindows.Count > 0) e.Cancel = true;
            else
            {
                //this.RequestFilter.Dispose();
                //myparcelcmd.Filter.Dispose();
                //this.ParcelPaymentsUC.Filter.Dispose();
                //this.PaymentlistUC.Filter.Dispose();
            }
        }

        private void AgentFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void CustomerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void InvoiceNumberFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void ParcelFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }

        private void CurrencyBuyButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            Prepay prepay = ((sender as Button).Tag as PrepayCustomerRequestVM).Prepay;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winPrepayCurrencyBuy" && item.DataContext == prepay) ObjectWin = item;
            }
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
        private void RubPayButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            Prepay prepay = ((sender as Button).Tag as PrepayCustomerRequestVM).Prepay;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winPrepayRubPay" && item.DataContext== prepay) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new PrepayRubPayWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = new Classes.Domain.Account.PrepayRubPayViewCommand(prepay);
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
