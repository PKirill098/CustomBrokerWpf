using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ExpenditureItemWin.xaml
    /// </summary>
    public partial class ExpenditureItemWin : Window
    {
        public ExpenditureItemWin()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
        }

        private void DataLoad()
        {
            try
            {
                ReferenceDS itemDS = ((ReferenceDS)(this.FindResource("keyReferenceDS")));
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.AccountTransactionTypeAdapter ttadapter = new ReferenceDSTableAdapters.AccountTransactionTypeAdapter();
                ttadapter.ClearBeforeFill = false;
                ttadapter.Fill(itemDS.tableAccountTransactionType);
                CollectionViewSource ttDS = this.FindResource("keyTransactionTypeVS") as CollectionViewSource;
                ttDS.Source = itemDS.tableAccountTransactionType;
                ttDS.SortDescriptions.Add(new System.ComponentModel.SortDescription("typedescr", System.ComponentModel.ListSortDirection.Ascending));
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureItemAdapter аdapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureItemAdapter();
                аdapter.ClearBeforeFill = false;
                аdapter.Fill(itemDS.tableExpenditureItem);
                mainDataGrid.ItemsSource = itemDS.tableExpenditureItem.DefaultView;
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
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDS itemDS = ((KirillPolyanskiy.CustomBrokerWpf.ReferenceDS)(this.FindResource("keyReferenceDS")));
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureItemAdapter adapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureItemAdapter();
                adapter.Update(itemDS.tableExpenditureItem);
                isSuccess = true;
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

    }
}
