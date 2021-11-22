using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для CustomerBalanceWin.xaml
    /// </summary>
    public partial class CustomerBalanceWin : Window, ISQLFiltredWindow
    {
        DateTime _lastDateInvoice;
        CustomerBalances _balances;
        public CustomerBalanceWin()
        {
            InitializeComponent();
            _balances = new CustomerBalances();
            _lastDateInvoice = DateTime.Today.AddDays(1D);
        }
        internal DateTime LastDateInvoice { set { _lastDateInvoice = value; } get { return _lastDateInvoice; } }
        private void winDebtors_Loaded(object sender, RoutedEventArgs e)
        {
            (CollectionViewSource.GetDefaultView(_balances) as CollectionView).SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            DataLoad();
        }
        private void winDebtors_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            thisfiltercust.RemoveCurrentWhere();
            thisfiltertran.RemoveCurrentWhere();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.Refresh();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == mainDataGrid && !(mainDataGrid.SelectedItems.Count == 1 & e.RemovedItems.Count < 2 & e.AddedItems.Count == 1)) totalDataRefresh();
        }
        private void BalanceInfoButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerBalance row = mainDataGrid.CurrentItem as CustomerBalance;
            InvoicePaymentWin win = null;
            foreach (Window frwin in this.OwnedWindows)
            {
                if (frwin.Name == "winInvoicePayment")
                {
                    if ((frwin as InvoicePaymentWin).CustomerId == row.Id)
                    {
                        win = frwin as InvoicePaymentWin;
                        break;
                    }
                }
            }

            if (win == null)
            {
                win = new InvoicePaymentWin();
                win.CustomerId = row.Id;
                win.CustomerName = row.Name;
                win.Owner = this;
                win.Show();
            }
            else
            {
                win.Activate();
                if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
            }
        }

        private void DataLoad()
        {
            try
            {
                _balances.Load(this.thisfiltercust.FilterWhereId, this.thisfiltertran.FilterWhereId, _lastDateInvoice);
                mainDataGrid.ItemsSource = _balances;
                totalDataRefresh();
                setFilterButtonImage();
            }
            catch (Exception ex)
            {
                ExpectionShowErrMessage(ex, "Загрузка данных");
                if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    DataLoad();
                }
            }
        }
        private void Refresh()
        {
            try
            {
                _balances.Refresh(this.thisfiltercust.FilterWhereId, this.thisfiltertran.FilterWhereId, _lastDateInvoice);
                totalDataRefresh();
            }
            catch (Exception ex)
            {
                ExpectionShowErrMessage(ex, "Загрузка данных");
                if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    DataLoad();
                }
            }
        }
        private void totalDataRefresh()
        {
            decimal totalSum = 0M;
            decimal totalNoTran = 0M;
            if (this.mainDataGrid.SelectedItems.Count > 1)
            {
                for (int i = 0; i < this.mainDataGrid.SelectedItems.Count; i++)
                {
                    if (this.mainDataGrid.SelectedItems[i] is CustomerBalance)
                    {
                        totalSum = totalSum + (this.mainDataGrid.SelectedItems[i] as CustomerBalance).SummDebtor;
                        totalNoTran = totalNoTran + (this.mainDataGrid.SelectedItems[i] as CustomerBalance).SummNoTran;
                    }
                }
            }
            else
            {
                foreach (object item in this.mainDataGrid.Items)
                {
                    if (item is CustomerBalance)
                    {
                        totalSum = totalSum + (item as CustomerBalance).SummDebtor;
                        totalNoTran = totalNoTran + (item as CustomerBalance).SummNoTran;
                    }
                }
            }
            totalsumTextBlock.Text = totalSum.ToString("N");
            totalnotranTextBlock.Text = totalNoTran.ToString("N");
            totalsuppsumTextBlock.Text = (totalSum - totalNoTran).ToString("N");
        }
        private void ExpectionShowErrMessage(System.Exception ex, string captionMessage)
        {
            if (ex is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                if (err.Number > 49999) MessageBox.Show(err.Message, captionMessage, MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                    {
                        errs.Append(sqlerr.Message + "\n");
                    }
                    MessageBox.Show(errs.ToString(), captionMessage, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source, captionMessage, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Filter
        private CustomBrokerWpf.SQLFilter thisfiltercust = new SQLFilter("customerbalance", "AND");
        private CustomBrokerWpf.SQLFilter thisfiltertran = new SQLFilter("customerbaltran", "AND");
        public bool IsShowFilter
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        public SQLFilter Filter
        {
            get { return thisfiltercust; }
            set
            {
                thisfiltercust.RemoveCurrentWhere();
                thisfiltercust = value;
                if (this.IsLoaded) DataLoad();
            }
        }
        public SQLFilter CustFilter
        {
            get { return thisfiltercust; }
            set
            {
                thisfiltercust.RemoveCurrentWhere();
                thisfiltercust = value;
                if (this.IsLoaded) DataLoad();
            }
        }
        public SQLFilter TranFilter
        {
            get { return thisfiltertran; }
            set
            {
                thisfiltertran.RemoveCurrentWhere();
                thisfiltertran = value;
                if (this.IsLoaded) DataLoad();
            }
        }

        public void RunFilter()
        {
            DataLoad();
        }
        private void setFilterButtonImage()
        {
            string uribitmap;
            if (thisfiltercust.isEmpty & thisfiltertran.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (FilterButton.Content as Image).Source = bi3;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winCustomerBalanceFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new CustomerBalanceFilterWin();
                    ObjectWin.Owner = this;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
            else
            {
                if (ObjectWin != null)
                {
                    ObjectWin.Close();
                }
            }
        }
        #endregion
    }
        internal class CustomerBalances : ObservableCollection<CustomerBalance>
        {
            internal void Load(int custfilterid,int tranfilterid,DateTime delay)
            {
                this.Clear();
                using (SqlConnection con = new SqlConnection(References.ConnectionString))
                {
                    con.Open();
                    SqlCommand comm = new SqlCommand("dbo.CustomerBalance_sp", con);
                    comm.CommandType = CommandType.StoredProcedure;
                    SqlParameter cfidp = new SqlParameter("@filterIdCust", custfilterid);
                    SqlParameter tfidp = new SqlParameter("@filterIdTran", tranfilterid);
                    SqlParameter delayp = new SqlParameter("@invlastdate", delay);
                    comm.Parameters.Add(cfidp); comm.Parameters.Add(tfidp); comm.Parameters.Add(delayp);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read()) this.Add(new CustomerBalance(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(3), reader.GetDecimal(4)));
                    reader.Close();
                    reader.Dispose();
                    con.Close();
                }
            }
            internal void Refresh(int custfilterid, int tranfilterid, DateTime delay)
            {
                foreach (CustomerBalance deb in this) { deb.Deleted = true; }
                using (SqlConnection con = new SqlConnection(References.ConnectionString))
                {
                    con.Open();
                    SqlCommand comm = new SqlCommand("dbo.CustomerBalance_sp", con);
                    comm.CommandType = CommandType.StoredProcedure;
                    SqlParameter cfidp = new SqlParameter("@filterIdCust", custfilterid);
                    SqlParameter tfidp = new SqlParameter("@filterIdTran", tranfilterid);
                    SqlParameter delayp = new SqlParameter("@invlastdate", delay);
                    comm.Parameters.Add(cfidp); comm.Parameters.Add(tfidp); comm.Parameters.Add(delayp);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        CustomerBalance newdeb = new CustomerBalance(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(3), reader.GetDecimal(4));
                        if (this.Contains(newdeb)) this[this.IndexOf(newdeb)].Refresh(newdeb);
                        else this.Add(newdeb);
                    }
                    for (int i = this.Count - 1; i > -1; i--)
                    {
                        if (this[i].Deleted) this.RemoveAt(i);
                    }
                    reader.Close();
                    reader.Dispose();
                    con.Close();
                }
            }
            private string SortByName(CustomerBalance customer)
            {
                return customer.Name;
            }
        }
        public class CustomerBalance : IEquatable<Debtor>, IComparable<Debtor>, System.ComponentModel.INotifyPropertyChanged
        {
            bool _isDel;
            internal bool Deleted
            {
                set
                {
                    _isDel = value;
                    //foreach (DebtorDetail det in _details) { det.Deleted = value; }
                }
                get { return _isDel; }
            }

            int _id;
            string _name;
            decimal _sum;
            decimal _sumnt;
            //ObservableCollection<DebtorDetail> _details;

            public int Id { set { _id = value; } get { return _id; } }
            public string Name { set { _name = value; } get { return _name; } }
            public decimal SummDebtor { set { _sum = value; } get { return _sum; } }
            public decimal SummNoTran { set { _sumnt = value; } get { return _sumnt; } }
            public decimal SummDebtorNoTran { get { return _sum - _sumnt; } }
            //public ObservableCollection<DebtorDetail> Details
            //{
            //    get
            //    {
            //        if (_details.Count == 0) LoadDetails();
            //        return _details;
            //    }
            //}

            public CustomerBalance(int id, string name, decimal summ, decimal summnt)
            {
                _id = id;
                _name = name;
                _sum = summ;
                _sumnt = summnt;
                //_details = GetDetails();
            }
            //public void LoadDetails()
            //{
            //    this._details.Clear();
            //    _details = GetDetails();
            //}
            public void Refresh(CustomerBalance newvers)
            {
                if (this.SummDebtor != newvers.SummDebtor)
                {
                    this.SummDebtor = newvers.SummDebtor;
                    NotifyPropertyChanged("SummDebtor");
                }
                //ObservableCollection<DebtorDetail> newversdet = GetDetails();
                //foreach (DebtorDetail newdet in newversdet)
                //{
                //    if (this._details.Contains(newdet))
                //    {
                //        DebtorDetail olddet = this._details[this._details.IndexOf(newdet)];
                //        if (olddet.Invoicesum != newdet.Invoicesum) olddet.Invoicesum = newdet.Invoicesum;
                //        if (olddet.DetailSum != newdet.DetailSum) olddet.DetailSum = newdet.DetailSum;
                //        olddet.Deleted = false;
                //    }
                //    else this._details.Add(newdet);
                //}
                //for (int i = this._details.Count - 1; i > -1; i--)
                //{
                //    if (this._details[i].Deleted) this._details.RemoveAt(i);
                //}
                this.Deleted = false;
            }
            //private ObservableCollection<DebtorDetail> GetDetails()
            //{
            //    ObservableCollection<DebtorDetail> details = new ObservableCollection<DebtorDetail>();
            //    using (SqlConnection con = new SqlConnection(References.ConnectionString))
            //    {
            //        con.Open();
            //        SqlCommand comm = new SqlCommand("account.DebtorsDetail_sp", con);
            //        comm.CommandType = CommandType.StoredProcedure;
            //        SqlParameter par = new SqlParameter("@customerid", _id);
            //        comm.Parameters.Add(par);
            //        SqlDataReader reader = comm.ExecuteReader();
            //        while (reader.Read()) details.Add(new DebtorDetail(reader.GetInt32(1), reader.GetString(0), reader.GetDecimal(2), reader.GetDecimal(3)));
            //        reader.Close();
            //        reader.Dispose();
            //        con.Close();
            //    }
            //    return details;
            //}
            #region Interfases
            public bool Equals(Debtor other)
            {
                return this._id == other.Id;
            }
            public int CompareTo(Debtor other)
            {
                return this._name.CompareTo(other.Name);
            }
            //INotifyPropertyChanged
            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(String info)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(info));
                }
            }
            #endregion
        }
}
