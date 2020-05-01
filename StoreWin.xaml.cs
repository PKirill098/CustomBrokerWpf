using System;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для StoreWin.xaml
    /// </summary>
    public partial class StoreWin : Window
    {
        public StoreWin()
        {
            InitializeComponent();
        }

        private void winStore_Loaded(object sender, RoutedEventArgs e)
        {

            DataLoad();
        }
        private void DataLoad()
        {
            try
            {
                ReferenceDS itemDS = ((ReferenceDS)(this.FindResource("keyReferenceDS")));
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.StoreAdapter аdapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.StoreAdapter();
                аdapter.ClearBeforeFill = false;
                аdapter.Fill(itemDS.tableStore);
                mainDataGrid.ItemsSource = itemDS.tableStore.DefaultView;
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
                mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDS itemDS = ((KirillPolyanskiy.CustomBrokerWpf.ReferenceDS)(this.FindResource("keyReferenceDS")));
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.StoreAdapter adapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.StoreAdapter();
                adapter.Update(itemDS.tableStore);
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
    }
}
