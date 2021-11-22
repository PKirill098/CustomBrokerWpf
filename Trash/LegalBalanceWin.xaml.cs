using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для LegalBalanceWin.xaml
    /// </summary>
    public partial class LegalBalanceWin : Window
    {
        List<LegalBalance> lagalList;

        public LegalBalanceWin()
        {
            InitializeComponent();
            lagalList = new List<LegalBalance>();
        }

        private void winDebtors_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
            this.mainDataGrid.ItemsSource = lagalList;
        }
        private void winDebtors_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterButton.Content = FilterButton.IsChecked.Value ? "Все" : "Астивные";
            DataLoad();
            (System.Windows.Data.CollectionViewSource.GetDefaultView(lagalList) as System.Windows.Data.ListCollectionView).Refresh();
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            DataLoad();
            (System.Windows.Data.CollectionViewSource.GetDefaultView(lagalList) as System.Windows.Data.ListCollectionView).Refresh();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BalanceInfoButton_Click(object sender, RoutedEventArgs e)
        {
            LegalBalance row = mainDataGrid.CurrentItem as LegalBalance;
            LegalBalanceDetailsWin win = null;
            foreach (Window frwin in this.OwnedWindows)
            {
                if (frwin.Name == "winLegalBalanceDetails")
                {
                    if ((frwin as LegalBalanceDetailsWin).LegalId == row.Id)
                    {
                        win = frwin as LegalBalanceDetailsWin;
                        break;
                    }
                }
            }

            if (win == null)
            {
                win = new LegalBalanceDetailsWin();
                win.LegalId = row.Id;
                win.LegalName = row.Name;
                win.Owner = this;
                win.Show();
            }
            else
            {
                win.Activate();
                if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
            }
        }

        private void mainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == mainDataGrid && !(mainDataGrid.SelectedItems.Count == 1 & e.RemovedItems.Count < 2 & e.AddedItems.Count == 1)) totalDataRefresh();
        }

        private void DataLoad()
        {
            lagalList.Clear();
            using (SqlConnection con = new SqlConnection(References.ConnectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand comm = new SqlCommand("dbo.LegalBalance_sp", con);
                    comm.CommandType = CommandType.StoredProcedure;
                    SqlParameter isall = new SqlParameter("@isShowAll", this.FilterButton.IsChecked);
                    comm.Parameters.Add(isall);
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        lagalList.Add(new LegalBalance((int)reader["idlegalentity"], (string)reader["namelegal"], (decimal)reader["balance"], (decimal)reader["notran"]));
                    }
                    reader.Close();
                    reader.Dispose();
                    con.Close();
                    totalDataRefresh();
                }
                catch (Exception ex)
                {
                    con.Close();
                    ExpectionShowErrMessage(ex, "Загрузка данных");
                }
            }
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
        private void totalDataRefresh()
        {
            decimal totalSum = 0M;
            decimal totalNoTran = 0M;
            if (this.mainDataGrid.SelectedItems.Count > 1)
            {
                for (int i = 0; i < this.mainDataGrid.SelectedItems.Count; i++)
                {
                    if (this.mainDataGrid.SelectedItems[i] is LegalBalance)
                    {
                        totalSum = totalSum + (this.mainDataGrid.SelectedItems[i] as LegalBalance).Balance;
                        totalNoTran = totalNoTran + (this.mainDataGrid.SelectedItems[i] as LegalBalance).noTransactionSum;
                    }
                }
            }
            else
            {
                foreach (LegalBalance item in this.lagalList)
                {
                        totalSum = totalSum + item.Balance;
                        totalNoTran = totalNoTran + item.noTransactionSum;
                }
            }
            totalsumTextBlock.Text = totalSum.ToString("N");
            totalnotranTextBlock.Text = totalNoTran.ToString("N");
            totalsuppsumTextBlock.Text = (totalSum + totalNoTran).ToString("N");
        }
    }

    public class LegalBalance
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public decimal Balance { set; get; }
        public decimal noTransactionSum { set; get; }
        public decimal ExpectedBalance { get { return this.Balance + this.noTransactionSum; } }
        internal LegalBalance(int id, string name, decimal balance, decimal notranSum)
        {
            this.Id = id;
            this.Name = name;
            this.Balance = balance;
            this.noTransactionSum = notranSum;
        }
    }
}
