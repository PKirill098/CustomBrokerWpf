using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для DebtorsWin.xaml
    /// </summary>
    public partial class DebtorsWin : Window
    {
        private Debtors _debtors;
        public DebtorsWin()
        {
            InitializeComponent();
            _debtors = new Debtors();
        }

        private void winDebtors_Loaded(object sender, RoutedEventArgs e)
        {
            this.delayPicker.SelectedDate = DateTime.Today.AddDays(-5D);
            DataLoad();
        }
        private void winDebtors_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
        }
        
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void debtorDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == debtorDataGrid && !(debtorDataGrid.SelectedItems.Count == 1 & e.RemovedItems.Count < 2 & e.AddedItems.Count == 1)) totalDataRefresh();
        }
        private void delayPicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void DataLoad()
        {
            try
            {
                _debtors.Load(this.delayPicker.DisplayDate.AddDays(1D));
                debtorDataGrid.ItemsSource = _debtors;
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
        private void Refresh()
        {
            try
            {
                _debtors.Refresh(this.delayPicker.DisplayDate.AddDays(1D));
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
            if (this.debtorDataGrid.SelectedItems.Count > 1)
            {
                for (int i = 0; i < this.debtorDataGrid.SelectedItems.Count; i++)
                {
                    if (this.debtorDataGrid.SelectedItems[i] is Debtor)
                    {
                        totalSum = totalSum + (this.debtorDataGrid.SelectedItems[i] as Debtor).SummDebtor;
                    }
                }
            }
            else
            {
                foreach (object item in this.debtorDataGrid.Items)
                {
                    if (item is Debtor)
                    {
                        totalSum = totalSum + (item as Debtor).SummDebtor;
                    }
                }
            }
            totalsumTextBlock.Text = totalSum.ToString("N");
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
    }
    internal class Debtors : ObservableCollection<Debtor>
    {
        internal void Load(DateTime delay)
        {
            this.Clear();
            using (SqlConnection con = new SqlConnection(References.ConnectionString))
            {
                con.Open();
                SqlCommand comm = new SqlCommand("account.Debtors_sp", con);
                comm.CommandType = CommandType.StoredProcedure;
                SqlParameter delayp = new SqlParameter("@delay", delay);
                comm.Parameters.Add(delayp);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read()) this.Add(new Debtor(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(3)));
                reader.Close();
                reader.Dispose();
                con.Close();
            }
        }
        internal void Refresh(DateTime delay)
        {
            foreach (Debtor deb in this) { deb.Deleted = true; }
            using (SqlConnection con = new SqlConnection(References.ConnectionString))
            {
                con.Open();
                SqlCommand comm = new SqlCommand("account.Debtors_sp", con);
                comm.CommandType = CommandType.StoredProcedure;
                SqlParameter delayp = new SqlParameter("@delay", delay);
                comm.Parameters.Add(delayp);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    Debtor newdeb=new Debtor(reader.GetInt32(0), reader.GetString(1), reader.GetDecimal(3));
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
    }
    public class Debtor : IEquatable<Debtor>, IComparable<Debtor>, System.ComponentModel.INotifyPropertyChanged 
    {
        bool _isDel;
        internal bool Deleted
        { 
            set 
            {
                _isDel = value;
                foreach (DebtorDetail det in _details) { det.Deleted = value; }
            }
            get { return _isDel; }
        }

        int _id;
        string _name;
        decimal _sum;
        ObservableCollection<DebtorDetail> _details;

        public int Id { set { _id = value; } get { return _id; } }
        public string Name { set { _name = value; } get { return _name; } }
        public decimal SummDebtor { set { _sum = value; } get { return _sum; } }
        public ObservableCollection<DebtorDetail> Details
        {
            get
            {
                if (_details.Count == 0) LoadDetails();
                return _details;
            }
        }

        public Debtor(int id, string name, decimal summ)
        {
            _id = id;
            _name = name;
            _sum = summ;
            _details = GetDetails();
        }
        public void LoadDetails()
        {
            this._details.Clear();
            _details = GetDetails();
        }
        public void Refresh(Debtor newvers)
        {
            if (this.SummDebtor != newvers.SummDebtor)
            {
                this.SummDebtor = newvers.SummDebtor;
                NotifyPropertyChanged("SummDebtor");
            }
            ObservableCollection<DebtorDetail> newversdet = GetDetails();
            foreach (DebtorDetail newdet in newversdet)
            {
                if (this._details.Contains(newdet))
                {
                    DebtorDetail olddet = this._details[this._details.IndexOf(newdet)];
                    if (olddet.Invoicesum != newdet.Invoicesum) olddet.Invoicesum = newdet.Invoicesum;
                    if (olddet.DetailSum != newdet.DetailSum) olddet.DetailSum = newdet.DetailSum;
                    olddet.Deleted = false;
                }
                else this._details.Add(newdet);
            }
            for (int i = this._details.Count-1; i > -1; i--)
            {
                if (this._details[i].Deleted) this._details.RemoveAt(i);
            }
            this.Deleted = false;
        }
        private ObservableCollection<DebtorDetail> GetDetails()
        {
            ObservableCollection<DebtorDetail> details = new ObservableCollection<DebtorDetail>();
            using (SqlConnection con = new SqlConnection(References.ConnectionString))
            {
                con.Open();
                SqlCommand comm = new SqlCommand("account.DebtorsDetail_sp", con);
                comm.CommandType = CommandType.StoredProcedure;
                SqlParameter par = new SqlParameter("@customerid", _id);
                comm.Parameters.Add(par);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read()) details.Add(new DebtorDetail(reader.GetInt32(1), reader.GetString(0), reader.GetDecimal(2), reader.GetDecimal(3)));
                reader.Close();
                reader.Dispose();
                con.Close();
            }
            return details;
        }
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
    public class DebtorDetail : IEquatable<DebtorDetail>, IComparable<DebtorDetail>, System.ComponentModel.INotifyPropertyChanged
    {
        bool _isDel;
        internal bool Deleted { set { _isDel = value; } get { return _isDel; } }

        int _id;
        string _parcel;
        decimal _invocesum;
        decimal _detailsum;

        public int Id { set { _id = value; } get { return _id; } }
        public string ParcelFullNumber { set { _parcel = value; } get { return _parcel; } }
        public decimal Invoicesum
        {
            set
            {
                _invocesum = value;
                NotifyPropertyChanged("Invoicesum");
            }
            get { return _invocesum; } }
        public decimal DetailSum
        {
            set
            {
                _detailsum = value;
                NotifyPropertyChanged("DetailSum");
            }
            get { return _detailsum; } }

        public DebtorDetail(int id, string parcel, decimal invoicesum, decimal detailsum)
        {
            _id = id;
            _parcel = parcel;
            _invocesum = invoicesum;
            _detailsum = detailsum;
        }

        #region Interfases

        public bool Equals(DebtorDetail other)
        {
            return this._id == other.Id;
        }
        public int CompareTo(DebtorDetail other)
        {
            return this._id.CompareTo(other.Id);
        }
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
