using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class AccountWin : Window
    {
        int _accountid;
        public int AccountId { set { _accountid = value; } get { return _accountid; } }
        CustomBrokerWpf.AccountTransactionsDS thisDS;
        public AccountWin()
        {
            InitializeComponent();
            thisDS = new AccountTransactionsDS();
        }

        private void winAccount_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void winAccount_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (!SaveChanges())
            //{
            //    this.Activate();
            //    if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
            //    {
            //        e.Cancel = true;
            //    }
            //}
            //if (!e.Cancel)
            //{
            //    (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            //    thisfilter.RemoveFilter();
            //}
        }

        private void DataLoad()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    conn.Open();
                    SqlParameter accountid = new SqlParameter("@accountid", _accountid);
                    SqlParameter phostid = new SqlParameter("@hostId", SqlDbType.Int);
                    phostid.Direction = ParameterDirection.Output;
                    SqlParameter pnumber = new SqlParameter("@accountNumber", SqlDbType.NVarChar,20);
                    pnumber.Direction = ParameterDirection.Output;
                    SqlParameter pdescr = new SqlParameter("@accountDesc", SqlDbType.NVarChar, 100);
                    pdescr.Direction = ParameterDirection.Output;
                    SqlParameter pisan = new SqlParameter("@isan", SqlDbType.Bit);
                    pisan.Direction = ParameterDirection.Output;
                    SqlParameter pcurr = new SqlParameter("@accountcurr", SqlDbType.NChar, 3);
                    pcurr.Direction = ParameterDirection.Output;
                    SqlParameter psurplus = new SqlParameter("@accountsum", SqlDbType.Money);
                    psurplus.Direction = ParameterDirection.Output;
                    SqlParameter pbalance = new SqlParameter("@balance", SqlDbType.Money);
                    pbalance.Direction = ParameterDirection.Output;
                    SqlParameter pfreesum = new SqlParameter("@freesum", SqlDbType.Money);
                    pfreesum.Direction = ParameterDirection.Output;

                    SqlCommand comm = new SqlCommand("dbo.Account_sp", conn);
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add(accountid); comm.Parameters.Add(phostid); comm.Parameters.Add(pnumber); comm.Parameters.Add(pdescr); comm.Parameters.Add(pisan);
                    comm.Parameters.Add(pcurr); comm.Parameters.Add(psurplus); comm.Parameters.Add(pbalance); comm.Parameters.Add(pfreesum);
                    comm.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                    AccountObj thisaccount = new AccountObj(_accountid,(int) phostid.Value, pnumber.Value.ToString(), pdescr.Value.ToString(),pcurr.Value.ToString(),(decimal) psurplus.Value,(decimal) pbalance.Value,(decimal) pfreesum.Value,(bool) pisan.Value);
                    this.mainGrid.DataContext = thisaccount;
                }
                this.TransactionDataGrid.ItemsSource = null;
                AccountTransactionsDSTableAdapters.AccountTransactionsAdapter adapter = new AccountTransactionsDSTableAdapters.AccountTransactionsAdapter();
                adapter.Fill(thisDS.tableAccountTransactions, _accountid);
                thisDS.tableAccountTransactions.DefaultView.Sort = "datetran";
                this.TransactionDataGrid.ItemsSource = thisDS.tableAccountTransactions.DefaultView;
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class AccountObj
    {
        int accountid,hostId;
        string accountNumber,accountDesc,accountcurr;
        decimal _surplus, _balance, _freesum;
        bool _isan;

        public int Id { get { return accountid; } }
        public int HostId { get { return hostId; } }
        public string Number { get { return accountNumber; } }
        public string Description { get { return accountDesc; } }
        public string Currency { get { return accountcurr; } }
        public decimal Surplus { get { return _surplus; } }
        public decimal Balance { get { return _balance; } }
        public decimal FreeSum { get { return _freesum; } }
        public bool isAnalitic { get { return _isan; } }

        public AccountObj(int id, int idhost, string number, string description, string currancy, decimal surplus, decimal balance, decimal freesum, bool isan)
        {
            accountid=id;
            hostId = idhost;
            accountNumber=number;
            accountDesc=description;
            accountcurr = currancy;
            _surplus = surplus;
            _balance=balance;
            _freesum=freesum;
            _isan = isan;
        }
    }
}
