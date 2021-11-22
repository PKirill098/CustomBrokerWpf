using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для PaymentWin.xaml
    /// </summary>
    public partial class PaymentAddWin : Window
    {
        int parcel;
        internal int Parcel
        { set { parcel = value; } get { return parcel; } }

        public PaymentAddWin()
        {
            InitializeComponent();
        }

        private void winPayment_Loaded(object sender, RoutedEventArgs e)
        {
            this.customerComboBox.ItemsSource = CustomerLoad(parcel);
            ReferenceDS referenceDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (referenceDS.tableLegalEntity.Count == 0)
            {
                ReferenceDSTableAdapters.LegalEntityAdapter thisLegalEntityAdapter = new ReferenceDSTableAdapters.LegalEntityAdapter();
                thisLegalEntityAdapter.Fill(referenceDS.tableLegalEntity);
            }
            this.accountComboBox.ItemsSource = new DataView(referenceDS.tableLegalEntity, string.Empty, "namelegal", DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent);
            this.DataContext = new Payment();
            
        }

        private void winPayment_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void customerComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.customerComboBox.Text.Length > 0)
            {
                ClientWin win = new ClientWin();
                win.Show();
                win.CustomerNameList.Text = this.customerComboBox.Text;
            }
        }

        private List<CustomerName> CustomerLoad(int parcelid)
        {
            using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                List<CustomerName> listcust = new List<CustomerName>();
                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "dbo.ParcelCustomerList_sp";
                    SqlParameter par = new SqlParameter("@parcelid", parcelid);
                    com.Parameters.Add(par);
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read()) listcust.Add(new CustomerName(reader.GetInt32(0), reader.GetString(1)));
                    reader.Close();
                    reader.Dispose();
                    return listcust;
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
                        listcust = CustomerLoad(parcel);
                    }
                }
                con.Close();
                con.Dispose();
                return listcust;
            }
        }
        private List<AccountSettlement> SettlementLoad()
        {
            using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                List<AccountSettlement> listbank = new List<AccountSettlement>();
                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText = "SELECT [accountid],[bankaccount],bankName,bankaccountcurr FROM dbo.AccountSettlement_vw";
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read()) listbank.Add(new AccountSettlement(reader.GetInt32(0), reader.GetString(1), reader.GetString(3), reader.GetString(1) + " " + reader.GetString(3) + " " + reader.GetString(2), reader.GetString(2)));
                    reader.Close();
                    reader.Dispose();
                    return listbank;
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
                        listbank = SettlementLoad();
                    }
                }
                con.Close();
                con.Dispose();
                return listbank;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges())
            {
                PopupText.Text = "Изменения сохранены";
                popInf.IsOpen = true;
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                IInputElement fcontrol = FocusManager.GetFocusedElement(this);
                if (fcontrol is TextBox)
                {
                    BindingExpression be;
                    be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                    if (be != null) be.UpdateSource();
                }
                this.JoinsDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                this.JoinsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                Payment newpay = this.DataContext as Payment;
                isSuccess = newpay.Save();
                customerComboBox.IsReadOnly = !isSuccess;
                accountComboBox.IsReadOnly = !isSuccess;
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                        {
                            errs.Append(sqlerr.Message + "\n");
                        }
                        MessageBox.Show(errs.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (ex is System.Data.NoNullAllowedException)
                {
                    MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля или удалите платеж.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    isSuccess = SaveChanges();
                }
            }
            return isSuccess;
        }

        private void Grid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action != ValidationErrorEventAction.Removed)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        private void DCJoinButton_Click(object sender, RoutedEventArgs e)
        {
            Payment pay = this.DataContext as Payment;
            pay.DCJoinPost();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Отменить платеж?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Payment newpay = this.DataContext as Payment;
            }
        }
    }
    public class CustomerName
    {
        int idc;
        string namec;
        string fullnamec;
        public int Id
        { set { idc = value; } get { return idc; } }
        public string Name
        { set { namec = value; } get { return namec; } }
        public string FullName
        { set { fullnamec = value; } get { return fullnamec; } }
        public CustomerName(int id, string name)
        {
            idc = id;
            namec = name;
        }
    }
    public class AccountSettlement
    {
        private int _accountid;
        private string _account;
        private string _coraccount;
        private string _curr;
        private string _note;
        private string _bank;
        private string _bankbic;
        public int AccountId
        { set { _accountid = value; } get { return _accountid; } }
        public string Account
        { set { _account = value; } get { return _account; } }
        public string CorAccount
        { set { _coraccount = value; } get { return _coraccount; } }
        public string Currency
        { set { _curr = value; } get { return _curr; } }
        public string Note
        { set { _note = value; } get { return _note; } }
        public string Bank
        { set { _bank = value; } get { return _bank; } }
        public string BankBIC
        { set { _bankbic = value; } get { return _bankbic; } }
        public AccountSettlement(int id, string account, string currency, string note, string bank)
        {
            _accountid = id;
            _account = account;
            _coraccount = string.Empty;
            _curr = currency;
            _note = note;
            _bank = bank;
            _bankbic = string.Empty;
        }
        public AccountSettlement(int id, string account, string coraccount, string currency, string bank, string bankbic, string note)
        {
            _accountid = id;
            _account = account;
            _coraccount = coraccount;
            _curr = currency;
            _note = note;
            _bank = bank;
            _bankbic = bankbic;
        }
    }
    public class Payment : System.ComponentModel.INotifyPropertyChanged
    {
        bool isrefreshfreesumm;

        bool _dirty;

        int id;
        int _idtran;
        int _customerid;
        int _debetid;
        string _npp;
        string _note;
        DateTime _dpp;
        bool _dppisnull;
        DateTime _datepay;
        decimal _summ;
        bool _summisnull;
        decimal _freesumm;
        ObservableCollection<PaymentJoin> _dcjoins = new ObservableCollection<PaymentJoin>();

        public int PaymentId
        { get { return id; } }
        public int TranId
        { set { _idtran = value; } get { return _idtran; } }
        public int CustomerId
        {
            set
            {
                if (_customerid != value)
                {
                    _customerid = value;
                    _dirty = true;
                }
            }
            get { return _customerid; }
        }
        public int DebetId
        {
            set
            {
                if (_debetid != value)
                {
                    _debetid = value;
                    _dirty = true;
                }
            }
            get { return _debetid; }
        }
        public string NumberPP
        {
            set
            {
                if (!string.Equals(_npp, value))
                {
                    _npp = value;
                    _dirty = true;
                }
            }
            get { return _npp; }
        }
        public string Purpose
        {
            set
            {
                if (!string.Equals(_note, value))
                {
                    _note = value;
                    _dirty = true;
                }
            }
            get { return _note; }
        }
        public DateTime? DatePP
        {
            set
            {
                if (value.HasValue)
                {
                    if (!_dpp.Equals(value.Value))
                    {
                        _dpp = value.Value;
                        _dppisnull = false;
                        _dirty = true;
                    }
                }
                else if (!_dppisnull)
                {
                    _dppisnull = true;
                    _dirty = true;
                }
            }
            get
            {
                if (_dppisnull) return null;
                else return _dpp;
            }
        }
        public DateTime DatePay
        {
            set
            {
                if (!_datepay.Equals(value))
                {
                    _datepay = value;
                    _dirty = true;
                }
            }
            get { return _datepay; }
        }
        public decimal? SumPay
        {
            set
            {
                if (value.HasValue)
                {
                    if (!(value.Value > 0)) throw new Exception("Сумма платежа должна быть больше ноля.");
                    if (_summ != value.Value)
                    {
                        _summ = value.Value;
                        _summisnull = false;
                        _dirty = true;
                        refreshFreeSumm();
                    }
                }
                else if (!_summisnull)
                {
                    _summisnull = true;
                    _dirty = true;
                }
            }
            get
            {
                if (_summisnull) return null;
                else return _summ;
            }
        }
        public decimal FreeSumm
        { get { return _freesumm; } }
        public ObservableCollection<PaymentJoin> DCJoin
        { get { return _dcjoins; } }

        public bool Dirty
        { get { return _dirty; } }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(info));
            }
        }

        public Payment()
            : this(0, 0, 0, 0, DateTime.Today, null, DateTime.MinValue, 0, null)
        {
            _summisnull = true;
            _dppisnull = true;
        }
        public Payment(int paymentid, int tranid, int customerid, int debetid, DateTime datepay, string numberpp, DateTime datepp, decimal summ, string purpose)
        {
            id = paymentid;
            _customerid = customerid;
            _debetid = debetid;
            _npp = numberpp;
            _dpp = datepp;
            _datepay = datepay;
            _summ = summ;
            _note = purpose;
            _dppisnull = false;
            _dirty = false;
            isrefreshfreesumm = true;
            //_dcjoins.CollectionChanged+=new System.Collections.Specialized.NotifyCollectionChangedEventHandler(dcjoins_CollectionChanged);
        }

        internal bool Save()
        {
            bool isnew = this.id == 0;
            if (_customerid == 0 | _debetid == 0) throw new System.Data.NoNullAllowedException();
            if (!(_summ > 0)) throw new Exception("Сумма платежа должна быть больше ноля.");
            SqlCommand com = new SqlCommand();
            com.CommandType = CommandType.StoredProcedure;
            if (this._dirty)
            {
                using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    com.Connection = con;
                    SqlParameter sum = new SqlParameter("@sum", _summ);
                    SqlParameter paydate = new SqlParameter("@paydate", this._datepay);
                    SqlParameter ppnum = new SqlParameter("@ppnum", SqlDbType.NVarChar, 20, string.Empty); ppnum.IsNullable = true;
                    if (string.IsNullOrEmpty(_npp)) ppnum.Value = DBNull.Value; else ppnum.Value = _npp;
                    SqlParameter ppdate = new SqlParameter("@ppdate", SqlDbType.DateTime); ppdate.IsNullable = true;
                    if (_dppisnull) ppdate.Value = DBNull.Value; else ppdate.Value = _dpp;
                    SqlParameter descr = new SqlParameter("@descr", SqlDbType.NVarChar, 100); descr.IsNullable = true;
                    if (string.IsNullOrEmpty(_note)) descr.Value = DBNull.Value; else descr.Value = _note;
                    com.Parameters.Add(sum); com.Parameters.Add(paydate); com.Parameters.Add(ppnum); com.Parameters.Add(ppdate); com.Parameters.Add(descr);
                    con.Open();
                    if (id > 0)
                    {
                        com.CommandText = "dbo.AccountPaymentUpd_sp";
                        SqlParameter paymentid = new SqlParameter("@paymentid", this.id);
                        com.Parameters.Add(paymentid);
                        com.ExecuteNonQuery();
                    }
                    else
                    {
                        com.CommandText = "dbo.AccountPaymentAdd_sp";
                        SqlParameter customerid = new SqlParameter("@customerid", this._customerid);
                        SqlParameter daccountid = new SqlParameter("@daccountid", this._debetid);
                        SqlParameter paymentid = new SqlParameter("@paymentid", SqlDbType.Int);
                        paymentid.Direction = ParameterDirection.Output;
                        SqlParameter tranid = new SqlParameter("@idtran", SqlDbType.Int);
                        tranid.Direction = ParameterDirection.Output;
                        com.Parameters.Add(customerid); com.Parameters.Add(daccountid); com.Parameters.Add(paymentid); com.Parameters.Add(tranid);
                        com.ExecuteNonQuery();
                        this.id = (int)paymentid.Value;
                        this._idtran = (int)tranid.Value;
                    }
                    this._dirty = false;
                    con.Close();
                    con.Dispose();
                }
            }
            if (!isnew)
            {
                using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    com.Connection = con;
                    com.Parameters.Clear();
                    com.CommandText = "dbo.AccountDCJoinUpd_sp";
                    SqlParameter idtrand = new SqlParameter("@idtranD", SqlDbType.Int);
                    SqlParameter idtranc = new SqlParameter("@idtranC", SqlDbType.Int);
                    SqlParameter jsum = new SqlParameter("@joinSum", SqlDbType.Money);
                    com.Parameters.Add(idtrand); com.Parameters.Add(idtranc); com.Parameters.Add(jsum);
                    List<PaymentJoin> dirtyjoin = new List<PaymentJoin>();
                    foreach (PaymentJoin itemjoin in this._dcjoins)
                    {
                        if (itemjoin.Dirty) dirtyjoin.Add(itemjoin);
                    }
                    dirtyjoin.Sort(ComparePaymentJoinByPaySum);
                    foreach (PaymentJoin itemjoin in dirtyjoin)
                    {
                        //  itemjoin.Save();
                        if (con.State != ConnectionState.Open) con.Open();
                        idtrand.Value = itemjoin.IdJoinTran;
                        idtranc.Value = itemjoin.Payment.TranId;
                        jsum.Value = itemjoin.PaySum;
                        com.ExecuteNonQuery();
                        itemjoin.Dirty = false;
                    }
                    com.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }
            else LoadDCJoin();
            return !this._dirty;
        }
        private int ComparePaymentJoinByPaySum(PaymentJoin x, PaymentJoin y)
        {
            if (x.PaySum > y.PaySum) return 1;
            if (x.PaySum < y.PaySum) return -1;
            return 0;
        }
        internal bool LoadDCJoin()
        {
            bool loaded = false;
            if (this._idtran > 0)
            {
                using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "dbo.AccountDCJoin_sp";
                    SqlParameter idtran = new SqlParameter("@idtran", _idtran);
                    SqlParameter dc = new SqlParameter("@dc", 'c');
                    com.Parameters.Add(idtran); com.Parameters.Add(dc);
                    SqlDataReader reader = com.ExecuteReader();
                    this._dcjoins.Clear();
                    while (reader.Read())
                    {
                        PaymentJoin newjoin = new PaymentJoin(this, reader.GetInt32(0), reader.GetDecimal(1), reader.GetDecimal(2), reader.GetDecimal(3), reader.IsDBNull(4) ? string.Empty : reader.GetString(4));
                        newjoin.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(newjoin_PropertyChanged);
                        this._dcjoins.Add(newjoin);
                    }
                    refreshFreeSumm();
                    loaded = true;
                    reader.Close();
                    reader.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }
            return loaded;
        }

        void newjoin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            refreshFreeSumm();
        }
        //private void dcjoins_CollectionChanged(Object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    refreshFreeSumm();
        //}
        private void refreshFreeSumm()
        {
            if (isrefreshfreesumm)
            {
                decimal fresumm = this._summ;
                foreach (PaymentJoin join in this._dcjoins) fresumm = fresumm - join.PaySum;
                if (_freesumm != fresumm)
                {
                    _freesumm = fresumm;
                    NotifyPropertyChanged("FreeSumm");
                }
            }
        }

        internal void DCJoinPost()
        {
            isrefreshfreesumm = false;
            decimal freesum = _freesumm;
            foreach (PaymentJoin join in _dcjoins)
            {
                if ((join.FreeSum - join.PaySum) < freesum)
                {
                    freesum = freesum + join.PaySum - join.FreeSum;
                    join.PaySum = join.FreeSum;
                }
                else
                {
                    join.PaySum = join.PaySum + freesum;
                    freesum = 0M;
                    break;
                }
            }
            _freesumm = freesum;
            NotifyPropertyChanged("FreeSumm");
            isrefreshfreesumm = true;
        }
    }
    public class PaymentJoin : System.ComponentModel.INotifyPropertyChanged
    {
        bool _dirty;

        Payment _payment;
        int _idjtran;
        decimal _transum;
        decimal _freesum;
        decimal _paysum;
        string _note;

        public bool Dirty
        { set { _dirty = value; } get { return _dirty; } }
        public Payment Payment
        { set { _payment = value; } get { return _payment; } }
        public int IdJoinTran
        {
            set
            {
                _idjtran = value;
                _dirty = true;
            }
            get { return _idjtran; }
        }
        public decimal TranSum
        {
            set
            {
                _transum = value;
                _dirty = true;
            }
            get { return _transum; }
        }
        public decimal FreeSum
        {
            set
            {
                _freesum = value;
                _dirty = true;
            }
            get { return _freesum; }
        }
        public decimal PaySum
        {
            set
            {
                if (value < 0M) throw new Exception("Сумма разноски не может быть меньше ноля.");
                if (decimal.Round(_paysum, 2) != decimal.Round(value, 2))
                {
                    if (decimal.Round(_freesum, 2) != decimal.Round(value, 2)) _paysum = value;
                    else _paysum = _freesum;
                    _dirty = true;
                    NotifyPropertyChanged("PaySum");
                }
            }
            get { return _paysum; }
        }
        public string Description
        {
            set
            {
                if (!string.Equals(_note, value))
                {
                    _note = value;
                    _dirty = true;
                }
            }
            get { return _note; }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(info));
            }
        }

        public PaymentJoin(Payment payment, int idjointranc, decimal transum, decimal freesum, decimal paysum, string description)
        {
            _payment = payment;
            _idjtran = idjointranc;
            _transum = transum;
            _freesum = freesum;
            _paysum = paysum < 0M ? 0M : paysum;
            _note = description;
            _dirty = false;
        }

        internal bool Save()
        {
            if (this._dirty)
            {
                if (_paysum < 0) throw new Exception("Сумма разноски не может быть меньше ноля.");
                using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "dbo.AccountDCJoinUpd_sp";
                    SqlParameter idtrand = new SqlParameter("@idtranD", this._payment.TranId);
                    SqlParameter idtranc = new SqlParameter("@idtranC", _idjtran);
                    SqlParameter jsum = new SqlParameter("@joinSum", _paysum);
                    com.Parameters.Add(idtrand); com.Parameters.Add(idtranc); com.Parameters.Add(jsum);
                    con.Open();
                    com.ExecuteNonQuery();
                    this._dirty = false;
                    con.Close();
                    con.Dispose();
                }
            }
            return !this._dirty;
        }
    }
    public class AccountTransaction
    {

    }
    public class IsCreatedPaymentConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)value) > 0 ? true : false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
    public class ExistsFreeSummConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((decimal)value) != 0M ? true : false;
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
