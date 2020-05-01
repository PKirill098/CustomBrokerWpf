using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для InvoiceWin.xaml
    /// </summary>
    public partial class InvoiceWin : Window
    {
        private int id = 0;
        private InvoiceDSTableAdapters.InvoiceAdapter invoiceAdapter;
        InvoiceDSTableAdapters.InvoiceDetailAdapter detAdapter;
        internal DataRowView InvoiceRowView
        { set { this.mainGrid.DataContext = value; } get { return this.mainGrid.DataContext as DataRowView; } }
        internal InvoiceDS.tableInvoiceRow Invoice
        {
            get
            {
                if (this.mainGrid.DataContext is DataRowView)
                    return (this.mainGrid.DataContext as DataRowView).Row as InvoiceDS.tableInvoiceRow;
                else return null;
            }
        }

        public InvoiceWin()
        {
            InitializeComponent();
        }

        private void winInvoice_Loaded(object sender, RoutedEventArgs e)
        {
            InvoiceDS ds = (this.Owner as InvoiceListWin).thisDS;
            DataView ivoiceview = new DataView(ds.tableCustomerName, string.Empty, "customerName", DataViewRowState.CurrentRows);
            this.customerComboBox.ItemsSource = ivoiceview;
            DataView accountview = new DataView(ds.tableLegalEntity, string.Empty, "namelegal", DataViewRowState.CurrentRows);
            this.accountComboBox.ItemsSource = accountview;
            invoiceAdapter = new InvoiceDSTableAdapters.InvoiceAdapter();
            detAdapter = new InvoiceDSTableAdapters.InvoiceDetailAdapter();
            detAdapter.ClearBeforeFill = false;
            detAdapter.Fill(ds.tableInvoiceDetail, Invoice.invoiceid);
        }
        private void winInvoice_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                try
                {
                    Invoice.Refresh();
                    detAdapter.Fill((this.Owner as InvoiceListWin).thisDS.tableInvoiceDetail, Invoice.invoiceid);
                }
                catch (System.Exception ex)
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
            }
        }
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Отменить несохраненные изменения?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                IInputElement focelm = FocusManager.GetFocusedElement(this);
                FocusManager.SetFocusedElement(this, sender as Button);
                Reject();
                FocusManager.SetFocusedElement(this, focelm);
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges())
            {
                PopupText.Text = "Изменения сохранены";
                popInf.PlacementTarget = sender as UIElement;
                popInf.IsOpen = true;
            }
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалть все сведения о счете?", "Счет", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                id = this.Invoice.invoiceid;
                this.Invoice.Delete();
                this.InvoiceRowView = null;
            }
        }
        private void InvoiceExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges())
            {
                CustomBrokerWpf.Invoice invobj = new CustomBrokerWpf.Invoice();
                invobj.CreateInvoiceExcel(this.Invoice);
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void payerComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.customerComboBox.Text.Length > 0)
            {
                ClientWin win = new ClientWin();
                win.Show();
                win.CustomerNameList.Text = this.customerComboBox.Text;
            }
        }


        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        { (sender as ComboBox).IsDropDownOpen = true; (sender as ComboBox).IsDropDownOpen = false; }

        private void mainDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (this.IsVisible & e.Action == ValidationErrorEventAction.Added)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        private bool SaveChanges()
        {
            bool isSuccess = false,updatenewbinding;
            int detailselectindex;
            IInputElement focelm = FocusManager.GetFocusedElement(this);
            FocusManager.SetFocusedElement(this, SaveButton);
            if ((focelm is DependencyObject) && Validation.GetHasError(focelm as DependencyObject)) return false;
            detailDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            detailDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            try
            {
                if (this.mainGrid.DataContext is DataRowView)
                {
                    DataRowView rowview = this.mainGrid.DataContext as DataRowView;
                    rowview.EndEdit();
                }
                InvoiceDS ds = (this.Owner as InvoiceListWin).thisDS;

                if (this.mainGrid.DataContext == null)
                {
                    detAdapter.Update(ds.tableInvoiceDetail.Select("invoiceid=" + id.ToString(), string.Empty, DataViewRowState.Deleted));
                    invoiceAdapter.Update(ds.tableInvoice.Select("invoiceid=" + id.ToString(), string.Empty, DataViewRowState.Deleted));
                }
                else
                {
                    string err = string.Empty;
                    DataRow[] detailrows = ds.tableInvoiceDetail.Select("invoiceid=" + this.Invoice.invoiceid.ToString(), string.Empty, DataViewRowState.CurrentRows | DataViewRowState.Deleted);
                    if (Invoice.HasErrors)
                    {
                        err = Invoice.RowError;
                        if (Invoice.GetColumnsInError().Length > 0)
                            err = Invoice.GetColumnError(Invoice.GetColumnsInError()[0]);
                    }
                    else if (ds.tableInvoiceDetail.GetErrors().Length > 0)
                        foreach (DataRow detailrow in detailrows)
                        {
                            if (detailrow.HasErrors)
                            {
                                err = detailrow.RowError;
                                if (detailrow.GetColumnsInError().Length > 0)
                                    err = detailrow.GetColumnError(detailrow.GetColumnsInError()[0]);
                                break;
                            }
                        }
                    if (err.Length > 0)
                    {
                        MessageBox.Show(err, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    detailselectindex = detailDataGrid.SelectedIndex;
                    updatenewbinding=Invoice.RowState==DataRowState.Added;
                    invoiceAdapter.Update(Invoice);
                    detAdapter.Update(detailrows);
                    if(updatenewbinding)
                    {
                        System.Windows.Data.BindingExpression childbinding = detailDataGrid.GetBindingExpression(DataGrid.ItemsSourceProperty);
                        childbinding.UpdateTarget();
                        detailDataGrid.SelectedIndex = detailselectindex;
                    }
                }
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
                    if (Invoice.HasErrors) Invoice.ClearErrors();
                    else foreach (DataRow row in Invoice.GetChildRows("FK_tableInvoice_tableInvoiceDetail")) row.ClearErrors();
                }
                else if (ex is System.Data.NoNullAllowedException)
                {
                    MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля или удалите платеж.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            FocusManager.SetFocusedElement(this, focelm);
            return isSuccess;
        }
        private void Reject()
        {
            this.detailDataGrid.CancelEdit(DataGridEditingUnit.Cell);
            this.detailDataGrid.CancelEdit(DataGridEditingUnit.Row);
            InvoiceDS ds = (this.Owner as InvoiceListWin).thisDS;
            if (this.Invoice == null)
            {
                DataRow[] invoicerows = ds.tableInvoice.Select("invoiceid=" + id.ToString(), string.Empty, DataViewRowState.Deleted);
                if (invoicerows.Length > 0)
                {
                    invoicerows[0].RejectChanges();
                    foreach (DataRowView viewrow in invoicerows[0].Table.DefaultView)
                    {
                        if (viewrow.Row.Field<int>("invoiceid") == id)
                        {
                            this.InvoiceRowView = viewrow;
                            break;
                        }
                    }
                    id = 0;
                }
                else
                    return;
            }
            DataRow[] detailrows = ds.tableInvoiceDetail.Select("invoiceid=" + this.Invoice.invoiceid.ToString(), string.Empty, DataViewRowState.CurrentRows | DataViewRowState.Deleted);
            foreach (DataRow row in detailrows) row.RejectChanges();
            this.InvoiceRowView.CancelEdit();
            this.Invoice.RejectChanges();
            if (this.Invoice.RowState == DataRowState.Detached)
            {
                this.InvoiceRowView = null;
                this.Close();
            }
        }

        private void detailDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                if (InvoiceRowView!=null && InvoiceRowView.IsNew) InvoiceRowView.EndEdit();
            }
            catch (NoNullAllowedException)
            {
                MessageBox.Show("Одно из обязательных для заполнения полей оставлено пустым. \n Введите значение в поле.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source, "Счет", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void customerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InvoiceDS.tableCustomerNameRow row = (e.AddedItems[0] as DataRowView).Row as InvoiceDS.tableCustomerNameRow;
            if (!row.IspayaccountNull() & ((e.AddedItems.Count > 0 & e.RemovedItems.Count > 0) | this.Invoice.IsaccountIdNull() || this.Invoice.accountId == 0))
            {
                if (this.InvoiceRowView.IsNew) this.InvoiceRowView.EndEdit();
                this.Invoice.accountId = row.payaccount;
            }
        }

    }
}
