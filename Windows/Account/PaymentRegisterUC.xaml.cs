using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.WindowsAccount
{
    public partial class PaymentRegisterUC : UserControl
    {
        public PaymentRegisterUC()
        {
            InitializeComponent();
        }

        private bool myisempty;
        private double myscroloffset;
        private void Scrol_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (this.MainDataGrid.HasItems)
            {
                if (myisempty)
                {
                    ScrollViewer scrol = System.Windows.Media.VisualTreeHelper.GetChild(System.Windows.Media.VisualTreeHelper.GetChild(this.MainDataGrid, 0), 0) as ScrollViewer;
                    scrol.ScrollToHorizontalOffset(myscroloffset);
                    myisempty = false;
                    e.Handled = true;
                }
                else if (e.HorizontalChange != 0D)
                    myscroloffset = e.HorizontalOffset;
            }
            else if(!myisempty)
            {
                this.MainScrollViewer.ScrollToHorizontalOffset(myscroloffset);
                myisempty = true;
                //ScrollViewer scrol = System.Windows.Media.VisualTreeHelper.GetChild(System.Windows.Media.VisualTreeHelper.GetChild(this.MainDataGrid, 0), 0) as ScrollViewer;
                //scrol.ScrollToHorizontalOffset(myscroloffset);
                //e.Handled = true;
            }
        }
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (myisempty && e.ExtentWidthChange==0 && !this.MainDataGrid.HasItems)
            {
                    myscroloffset = e.HorizontalOffset;
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue is PaymentRegisterViewCommander)
            {
                myhost = null;
                FrameworkElement win = this;
                while (myhost == null & win!=null)
                    if (win.Parent is Window) myhost = win.Parent as Window;
                    else win = win.Parent as FrameworkElement;
                if (myhost != null)
                {
                    //ScrollViewer scrol = System.Windows.Media.VisualTreeHelper.GetChild(System.Windows.Media.VisualTreeHelper.GetChild(this.MainDataGrid, 0), 0) as ScrollViewer;
                    //scrol.ScrollChanged += this.Scrol_ScrollChanged;
                    mychildwindows = (myhost as lib.Interfaces.IMainWindow).ListChildWindow;
                    mybinddisp = new lib.BindingDischarger(myhost, new DataGrid[] { this.MainDataGrid });
                    mycmd = e.NewValue as PaymentRegisterViewCommander;
                    mycmd.CancelEdit = mybinddisp.CancelEdit;
                    mycmd.EndEdit = mybinddisp.EndEdit;
                }
                else
                {
                    MessageBox.Show("Не удалось определить Host для PaymentRegisterUC!", nameof(PaymentRegisterUC), MessageBoxButton.OK, MessageBoxImage.Error);
                    this.DataContext = null;
                }
            }
        }

        private Window myhost;
        private List<Window> mychildwindows;
        private lib.BindingDischarger mybinddisp;
        private PaymentRegisterViewCommander mycmd;

        private void CurrencyBuyButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            Classes.Domain.Importer importer = (this.DataContext as PaymentRegisterViewCommander).Importer;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winCurrencyBuy" && (item.DataContext as CurrencyBuyViewCommand).Importer == importer) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CurrencyBuyWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.DataContext = new Classes.Domain.Account.CurrencyBuyViewCommand(importer);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void CurrencyPayButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            Classes.Domain.Importer importer = (this.DataContext as PaymentRegisterViewCommander).Importer;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winCurrencyPay" && (item.DataContext as CurrencyPayViewCommand).Importer == importer) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CurrencyPayWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.DataContext = new Classes.Domain.Account.CurrencyPayViewCommand(null, importer);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void CustomsInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            Classes.Domain.Importer importer = (this.DataContext as PaymentRegisterViewCommander).Importer;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winCustomsInvoice" && (item.DataContext as CustomsInvoiceViewCommand).Importer == importer) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CustomsInvoiceWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.DataContext = new Classes.Domain.Account.CustomsInvoiceViewCommand(importer) { PaymentRegisterCMD = (sender as Button).DataContext as PaymentRegisterViewCommander };
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }

        private void CustomsInvoicePayButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            CustomsInvoice invoice = ((sender as Button).Tag as PrepayCustomerRequestVM).CustomsInvoice;
            if (invoice == null) return;
            foreach (Window item in myhost.OwnedWindows)
            {
                if (item.Name == "winCustomsInvoicePay" && (item.DataContext as CustomsInvoicePayRubViewCommand).Invoice == invoice) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CustomsInvoicePayWin();
                ObjectWin.Owner = myhost;
                ObjectWin.DataContext = new Classes.Domain.Account.CustomsInvoicePayRubViewCommand(invoice);
                ObjectWin.WindowState = WindowState.Normal;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void FinalInvoicePayButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            CustomsInvoice invoice = ((sender as Button).Tag as PrepayCustomerRequestVM).CustomsInvoice;
            if (invoice == null) return;
            foreach (Window item in myhost.OwnedWindows)
            {
                if (item.Name == "winFinalInvoicePay" && (item.DataContext as FinalInvoicePayViewCommand).Invoice == invoice) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new FinalInvoicePayWin();
                ObjectWin.Owner = myhost;
                ObjectWin.DataContext = new Classes.Domain.Account.FinalInvoicePayViewCommand(invoice);
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
            foreach (Window item in myhost.OwnedWindows)
            {
                if (item.Name == "winCustomsInvoicePay" && (item.DataContext as CustomsInvoicePayFinalCur1ViewCommand).Invoice == invoice) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CustomsInvoicePayWin();
                ObjectWin.Owner = myhost;
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
            foreach (Window item in myhost.OwnedWindows)
            {
                if (item.Name == "winCustomsInvoicePay" && (item.DataContext as CustomsInvoicePayFinalCur2ViewCommand).Invoice == invoice) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CustomsInvoicePayWin();
                ObjectWin.Owner = myhost;
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
        private void PrepayCurrencyBuyButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            Prepay prepay = ((sender as Button).Tag as PrepayCustomerRequestVM).Prepay;
            foreach (Window item in myhost.OwnedWindows)
            {
                if (item.Name == "winPrepayCurrencyBuy" && (item.DataContext as PrepayCurrencyBuyViewCommand).Prepay == prepay) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new PrepayCurrencyBuyWin();
                ObjectWin.Owner = myhost;
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
            foreach (Window item in myhost.OwnedWindows)
            {
                if (item.Name == "winPrepayCurrencyPay" && (item.DataContext as PrepayCurrencyPayViewCommand).Prepay == prepay) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new PrepayCurrencyPayWin();
                ObjectWin.Owner = myhost;
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
        private void PrepayRubPayButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            Prepay prepay = ((sender as Button).Tag as PrepayCustomerRequestVM).Prepay;
            foreach (Window item in myhost.OwnedWindows)
            {
                if (item.Name == "winPrepayRubPay" && (item.DataContext as PrepayRubPayViewCommand).Prepay == prepay) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new PrepayRubPayWin();
                ObjectWin.Owner = myhost;
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
        #region Filter
        private void RequestTDAgentFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.AgentFilter != null && !mycmd.AgentFilter.FilterOn) mycmd.AgentFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("AgentFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDCBRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CBRateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDCBRatep2pFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CBRatep2pFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDConsolidateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.ConsolidateFilter != null && !mycmd.ConsolidateFilter.FilterOn) mycmd.ConsolidateFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("ConsolidateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDCurrencyBoughtDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CurrencyBoughtDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDCurrencyBuyRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CurrencyBuyRateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDCurrencyPaidDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CurrencyPaidDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CurrencyPayFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CurrencyPayFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CustomerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.CustomerFilter != null && !mycmd.CustomerFilter.FilterOn) mycmd.CustomerFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("CustomerFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CustomerBalanceFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CustomerBalanceFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDCustomsInvoiceDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CustomsInvoiceDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDCustomsInvoicePaidDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CustomsInvoicePaidDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CustomsInvoicePercentFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CustomsInvoicePercentFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CustomsInvoiceRubSumFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CustomsInvoiceRubSumFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void DealPassportFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("DealPassportFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void DTSumFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("DTSumFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDEuroSumFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("EuroSumFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDExpiryDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("ExpiryDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDFinalPaidDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("FinalPaidDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void FinalCurPaidDate1FilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("FinalCurPaidDate1FilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void FinalCurPaidDate2FilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("FinalCurPaidDate2FilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void FinalInvoiceCur1SumFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("FinalInvoiceCur1SumFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void FinalInvoiceCur2SumFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("FinalInvoiceCur2SumFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void FinalRubSumFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("FinalRubSumFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void FinalRubSumPaidFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("FinalRubSumPaidFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDInvoiceDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("InvoiceDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDInvoiceNumberFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.InvoiceNumberFilter != null && !mycmd.InvoiceNumberFilter.FilterOn) mycmd.InvoiceNumberFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("InvoiceNumberFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDManagerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.ManagerFilter != null && !mycmd.ManagerFilter.FilterOn) mycmd.ManagerFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("ManagerFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void NoteFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.NoteFilter != null && !mycmd.NoteFilter.FilterOn) mycmd.NoteFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("NoteFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void OverPayFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("OverPayFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ParcelFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.ParcelFilter != null && !mycmd.ParcelFilter.FilterOn) mycmd.ParcelFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("ParcelFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDPercentFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.PercentFilter != null && !mycmd.PercentFilter.FilterOn) mycmd.PercentFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("PercentFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDPrepayFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("PrepayFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RateDiffPerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("RateDiffPerFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RateDiffResultFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("RateDiffResultFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RefundFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("RefundFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RubDiffFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("RubDiffFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDRubPaidDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("RubPaidDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDRubSumFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("RubSumFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void SellingFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("SellingFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDSellingDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("SellingDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestTDSPDDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("SPDDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        #endregion
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems != null)
                foreach (lib.Interfaces.ISelectable item in e.RemovedItems.OfType<lib.Interfaces.ISelectable>())
                    item.Selected = false;
            if (e.AddedItems != null)
                foreach (lib.Interfaces.ISelectable item in e.AddedItems.OfType<lib.Interfaces.ISelectable>())
                    item.Selected = true;
        }

        private void DataGridColumnHeader_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
