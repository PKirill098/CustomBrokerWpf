using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для InvoiceExcelWin.xaml
    /// </summary>
    public partial class InvoiceExcelWin : Window
    {
        public InvoiceExcelWin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.bankComboBox.ItemsSource = SettlementLoad();
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
                    com.CommandText = "SELECT [bankaccount],bankName,bankaccountcurr,backCorrAccount,bankBIC FROM dbo.AccountSettlement_vw";
                    SqlDataReader reader = com.ExecuteReader();
                    while (reader.Read()) listbank.Add(new AccountSettlement(0, reader.GetString(0), reader.GetString(3), reader.GetString(2), reader.GetString(1), reader.GetString(4), reader.GetString(0) + " " + reader.GetString(1)));
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

        private void InvoicePrintButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void InvoiceExcelButton_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
