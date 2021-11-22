using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class ParcelTransactionDetailWin : Window
    {
        int requestid;
        internal int RequestId
        { set { requestid = value; } get { return requestid; } }

        public ParcelTransactionDetailWin()
        {
            InitializeComponent();
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
                ParcelTransactionDSTableAdapters.ReturnAdapter adapter = new ParcelTransactionDSTableAdapters.ReturnAdapter();
                adapter.Adapter.InsertCommand.Parameters["@ttyp"].Value = "return0000";
                adapter.Adapter.UpdateCommand.Parameters["@ttyp"].Value = "return0000";
                ParcelTransactionDS.tableReturnDataTable thistable = (mainDataGrid.ItemsSource as DataView).Table as ParcelTransactionDS.tableReturnDataTable;
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
