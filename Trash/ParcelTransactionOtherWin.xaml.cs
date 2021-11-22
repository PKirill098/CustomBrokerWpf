using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ParcelTransactionOtherWin.xaml
    /// </summary>
    public partial class ParcelTransactionOtherWin : Window
    {
        int requestid;
        internal int RequestId
        {set { requestid=value;} get {return requestid;}}
        //ParcelTransactionOtherDS thisDS;
        public ParcelTransactionOtherWin()
        {
            InitializeComponent();
            //thisDS = new ParcelTransactionOtherDS();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
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
                this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                ParcelTransactionDSTableAdapters.adapterOther adapter = new ParcelTransactionDSTableAdapters.adapterOther();
                //adapter.Adapter.InsertCommand.Parameters["@requestid"].Value = this.requestid;
                ParcelTransactionDS.tableOtherDataTable thistable = (mainDataGrid.ItemsSource as DataView).Table as ParcelTransactionDS.tableOtherDataTable;
                thistable.SetStatus();
                adapter.Update(thistable.Select("requestid=" + this.requestid.ToString()));
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

        private void mainDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }
    }
}
