using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for MainMenuUC.xaml
    /// </summary>
    public partial class MainMenuUC : UserControl
    {
        public MainMenuUC()
        {
            InitializeComponent();
            mainwin = Application.Current.MainWindow as MainWindow;
        }

        MainWindow mainwin;

        private void MenuItemRequest_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winRequest") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new RequestWin();
                mainwin.ListChildWindow.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemStoreMerge_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winStoreMerge") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new StoreMergeWin();
                mainwin.ListChildWindow.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemParcel_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winParcel") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ParcelWin();
                mainwin.ListChildWindow.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuListParcel_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winParcelList") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ParcelListWin();
                mainwin.ListChildWindow.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuPayParcel_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winParcelTransaction") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ParcelTransactionWin();
                mainwin.ListChildWindow.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuPPParcel_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winPaymentList") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new PaymentListWin();
                mainwin.ListChildWindow.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }

        private void MenuItemClient_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winClient") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ClientWin();
                mainwin.ListChildWindow.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }

        private void MenuItemDebtor_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winCustomerBalance") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CustomerBalanceWin();
                mainwin.ListChildWindow.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemLegalBalance_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winLegalBalance") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new LegalBalanceWin();
                mainwin.ListChildWindow.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemParcelReport_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mainwin.ListChildWindow)
            {
                if (item.Name == "winParcelReport") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ParcelReportWin();
                mainwin.ListChildWindow.Add(ObjectWin);
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
