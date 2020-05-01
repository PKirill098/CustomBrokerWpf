using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Globalization;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для InvoicePaymentWin.xaml
    /// </summary>
    public partial class InvoicePaymentWin : Window
    {
        int _customerid;
        internal int CustomerId { set { _customerid = value; } get { return _customerid; } }
        string _customername;
        internal string CustomerName { set { _customername = value; this.CustomerNameTextBlock.Text = value; } get { return _customername; } }

        public InvoicePaymentWin()
        {
            InitializeComponent();
            _customerid = 0;
        }

        private void winInvoicePayment_Loaded(object sender, RoutedEventArgs e)
        {
            
            DataLoad();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void startDetailTextBox_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DataLoad();
        }

        private void DataLoad()
        {
            try
            {
                DateTime dt,ds;
                DateTime.TryParseExact(this.startDetailTextBox.Text, new string[] { "dd.MM.yy", "dd.MM.yyyy", "dd/MM/yy", "dd/MM/yyyy", "dd,MM,yy", "dd,MM,yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                DateTime.TryParseExact(this.stopDetailTextBox.Text, new string[] { "dd.MM.yy", "dd.MM.yyyy", "dd/MM/yy", "dd/MM/yyyy", "dd,MM,yy", "dd,MM,yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out ds);
                if (DateTime.MinValue.Equals(dt) & this.startDetailTextBox.Text.Length > 0)
                {
                    MessageBox.Show("Значение не удалось преобразовать в дату", "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.TransactionDataGrid.ItemsSource = null;
                ObservableCollection<InvoicePayment> invoicepayments = new ObservableCollection<InvoicePayment>();
                using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    
                    conn.Open();
                    SqlParameter customerid = new SqlParameter("@customerid", _customerid);
                    SqlParameter datedetail = new SqlParameter("@datedetail", dt);
                    SqlParameter datestop = new SqlParameter("@datestop", ds);
                    SqlParameter startbalance = new SqlParameter("@startbalance", SqlDbType.Money);
                    startbalance.Direction = ParameterDirection.Output;
                    startbalance.IsNullable = true;
                    SqlCommand comm = new SqlCommand("CustomerBalanceDetails_sp", conn);
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add(customerid);
                    if (!DateTime.MinValue.Equals(dt)) comm.Parameters.Add(datedetail);
                    if (!DateTime.MinValue.Equals(ds)) comm.Parameters.Add(datestop);
                    comm.Parameters.Add(startbalance);
                    SqlDataReader reader= comm.ExecuteReader();
                    while (reader.Read()) invoicepayments.Add(new InvoicePayment(reader.IsDBNull(0) ? string.Empty : reader.GetString(0)
                                                                                ,reader.IsDBNull(1) ? null: (int?)reader.GetInt32(1)
                                                                                ,reader.IsDBNull(2) ? null: (decimal?)reader.GetDecimal(2)
                                                                                , reader.IsDBNull(3) ? null : (decimal?)reader.GetDecimal(3)
                                                                                , reader.IsDBNull(4) ? string.Empty : reader.GetString(4)
                                                                                , reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                                                                                , reader.IsDBNull(5) ? null : reader.GetDateTime(5) as DateTime?
                                                                                , reader.GetDecimal(7), reader.GetDecimal(8)
                                                                                ,reader.GetInt32(9)==1));
                    reader.Close();
                    startBalanceTextBlock.Text = startbalance.Value == DBNull.Value ? "0" : ((decimal)startbalance.Value).ToString("N");
                    conn.Close();
                    conn.Dispose();
                }
                this.TransactionDataGrid.ItemsSource = invoicepayments;
                decimal invsum=0M, paysum=0M;
                foreach(InvoicePayment item in invoicepayments)
                {
                    if (item.isInvoice) invsum = invsum + item.summ;
                    else paysum = paysum + item.summ;
                }
                this.totalInvoice.Text = invsum.ToString("N2");
                this.totalPayment.Text = paysum.ToString("N2");
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                    {
                        errs.Append(sqlerr.Message + "\n");
                    }
                    MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    DataLoad();
                }
            }
        }

    }
    public class InvoicePayment
    {
        string _parcel;
        public string parcel { get { return _parcel; } }
        int? _cellnumber;
        public int? cellNumber { get { return _cellnumber; } }
        decimal? _volume;
        public decimal? volume { get { return _volume; } }
        decimal? _weight;
        public decimal? weight { get { return _weight; } }
        string _docnum;
        public string docnum { get { return _docnum; } }
        string _descr;
        public string descr { get { return _descr; } }
        DateTime _docdate;
        bool _docdateisnull;
        public DateTime? docdate
        {
            get
            {
                if (_docdateisnull) return null;
                else return _docdate;
            }
        }
        decimal _summ;
        public decimal summ { get { return _summ; } }
        decimal _balnc;
        public decimal balance { get { return _balnc; } }
        bool _isinvoice;
        public bool isInvoice { get{return _isinvoice;}}
        internal InvoicePayment(string parcel,int? cellnumber,decimal? volume,decimal? weight, string docnum, string descr, DateTime? docdate, decimal summ, decimal balance, bool isinvoice)
        {
            _parcel = parcel;
            _cellnumber = cellnumber;
            _volume = volume;
            _weight = weight;
            _docnum = docnum;
            _descr = descr;
            if (docdate.HasValue)
            {
                _docdate = docdate.Value;
                _docdateisnull = false;
            }
            else
            {
                _docdate = DateTime.MinValue;
                _docdateisnull = true;
            }
            _summ = summ;
            _balnc = balance;
            _isinvoice = isinvoice;
        }
    }
}
