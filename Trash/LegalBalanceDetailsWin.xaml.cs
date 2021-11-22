using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для LegalBalanceDetailsWin.xaml
    /// </summary>
    public partial class LegalBalanceDetailsWin : Window
    {
        int _id;
        internal int LegalId { set { _id = value; } get { return _id; } }
        string _name;
        internal string LegalName { set { _name = value; this.Title = "Актив " + value; } get { return _name; } }

        List<LegalBalanceDetail> detailList;

        public LegalBalanceDetailsWin()
        {
            InitializeComponent();
            _id = 0;
            detailList = new List<LegalBalanceDetail>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
            this.mainDataGrid.ItemsSource = detailList;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            DataLoad();
            System.Windows.Data.ListCollectionView view= System.Windows.Data.CollectionViewSource.GetDefaultView(detailList) as System.Windows.Data.ListCollectionView;
            view.Refresh();

        }
        private void startDetailTextBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DataLoad();
            (System.Windows.Data.CollectionViewSource.GetDefaultView(detailList) as System.Windows.Data.ListCollectionView).Refresh();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void DataLoad()
        {
            try
            {
                DateTime dt;
                DateTime.TryParseExact(this.startDetailTextBox.Text, new string[] { "dd.MM.yy", "dd.MM.yyyy", "dd/MM/yy", "dd/MM/yyyy", "dd,MM,yy", "dd,MM,yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                if (DateTime.MinValue.Equals(dt) & this.startDetailTextBox.Text.Length > 0)
                {
                    MessageBox.Show("Значение не удалось преобразовать в дату", "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                detailList.Clear();
                using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    conn.Open();
                    SqlParameter customerid = new SqlParameter("@legalid", _id);
                    SqlParameter datedetail = new SqlParameter("@startdate", dt);
                    SqlParameter startbalance = new SqlParameter("@startbalance", SqlDbType.Money);
                    startbalance.Direction = ParameterDirection.Output;
                    startbalance.IsNullable = true;
                    SqlCommand comm = new SqlCommand("dbo.LegalBalanceDetails_sp", conn);
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.Add(customerid);
                    if (!DateTime.MinValue.Equals(dt)) comm.Parameters.Add(datedetail);
                    comm.Parameters.Add(startbalance);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        LegalBalanceDetail item = new LegalBalanceDetail();
                        for (int c = 0; c < reader.FieldCount; c++)
                        {
                            switch (reader.GetName(c))
                            {
                                case "trandate":
                                    item.TranDate = reader.GetDateTime(c);
                                    break;
                                case "recipient":
                                    if (!reader.IsDBNull(c)) item.Recipient = reader.GetString(c);
                                    else item.Recipient = string.Empty;
                                    break;
                                case "docnum":
                                    if (!reader.IsDBNull(c)) item.DocNumber = reader.GetString(c);
                                    else item.DocNumber = string.Empty;
                                    break;
                                case "docdate":
                                    if (!reader.IsDBNull(c)) item.DocDate = reader.GetDateTime(c);
                                    else item.DocDate = null;
                                    break;
                                case "docsum":
                                    if (!reader.IsDBNull(c)) item.Sum = reader.GetDecimal(c);
                                    else item.Sum = 0M;
                                    break;
                                case "balance":
                                    item.Balance = reader.GetDecimal(c);
                                    break;
                                case "descr":
                                    if (!reader.IsDBNull(c)) item.Description = reader.GetString(c);
                                    else item.Description = string.Empty;
                                    break;
                                case "rang":
                                    item.Rang = reader.GetInt32(c);
                                    break;
                            }
                        }
                        detailList.Add(item);
                    }
                    reader.Close();
                    startBalanceTextBlock.Text = startbalance.Value == DBNull.Value ? "0" : ((decimal)startbalance.Value).ToString("N");
                    conn.Close();
                    conn.Dispose();
                }
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
    public class LegalBalanceDetail
    {
        public int Rang { set; get; }
        public string Recipient { set; get; }
        public string DocNumber { set; get; }
        public string Description { set; get; }
        public DateTime TranDate { set; get; }
        public DateTime? DocDate { set; get; }
        public decimal Sum { set; get; }
        public decimal Balance { set; get; }
        internal LegalBalanceDetail() { }
    }
}
