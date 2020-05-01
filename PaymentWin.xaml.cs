using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для PaymentWin.xaml
    /// </summary>
    public partial class PaymentWin : Window
    {
        int ppid = 0;
        internal DataRowView PaymentRowView
        { set { this.mainGrid.DataContext = value; } get { return this.mainGrid.DataContext as DataRowView; } }
        internal PaymentDS.tablePaymentRow Payment
        {
            get
            {
                if (this.mainGrid.DataContext is DataRowView)
                    return (this.mainGrid.DataContext as DataRowView).Row as PaymentDS.tablePaymentRow;
                else return null;
            }
        }
        PaymentlistUC myparent;

        public PaymentWin()
        {
            InitializeComponent();
        }

        private void winPayment_Loaded(object sender, RoutedEventArgs e)
        {
            if (Owner is PaymentListWin)
                myparent = (Owner as PaymentListWin).PaymentlistUC;
            //else
            //    myparent = (Owner as MainWindow).PaymentlistUC;
            PaymentDS ds = myparent.thisDS;
            DataView payerview = new DataView(ds.tableCustomerName, string.Empty, "customerName", DataViewRowState.CurrentRows);
            this.payerComboBox.ItemsSource = payerview;
            (this.mainGrid.FindResource("keyPayerVS") as CollectionViewSource).Source = payerview;
            DataView accountview = new DataView(ds.tableLegalEntity, string.Empty, "namelegal", DataViewRowState.CurrentRows);
            this.accountComboBox.ItemsSource = accountview;
            PaymentDSTableAdapters.TransactionAdapter tranadapter = new PaymentDSTableAdapters.TransactionAdapter();
            tranadapter.ClearBeforeFill = false;
            tranadapter.Fill(ds.tableTransaction, Payment.ppid);
            PaymentDSTableAdapters.DCJoinAdapter dcjadapter = new PaymentDSTableAdapters.DCJoinAdapter();
            dcjadapter.ClearBeforeFill = false;
            DataRow[] tranrows = ds.tableTransaction.Select("ppid=" + this.Payment.ppid.ToString());
            foreach (DataRow row in tranrows)
            {
                dcjadapter.Fill(ds.tableDCJoin, (row as PaymentDS.tableTransactionRow).idtran, null);
            }
            if (tranrows.Length > 0) this.TransDataGrid.CurrentCell = new DataGridCellInfo(this.TransDataGrid.Items[0], this.TransDataGrid.Columns[0]);
            //this.TransDataGrid.ItemsSource = (this.mainGrid.DataContext as DataRowView).CreateChildView("FK_tablePayment_tableTransaction");
            TransDataGrid.CanUserDeleteRows = (this.FindResource("keyVisibilityAccountVisors") as VisibilityAccountVisors).IsMember;
            DataView nojoinview = new DataView(ds.tablePayment, "payerid=" + Payment.payerid.ToString() + " AND ppid<>" + Payment.ppid.ToString() + " AND freeSum>0", "", DataViewRowState.CurrentRows);
            nojoinview.ListChanged += NoJoinPayment_ListChanged;
            this.OtherNoJoinPayment.ItemsSource = nojoinview;
            TotalNoJoinRefresh(nojoinview);
            if (this.JoinsDataGrid.ItemsSource != null) (CollectionViewSource.GetDefaultView(this.JoinsDataGrid.ItemsSource) as BindingListCollectionView).SortDescriptions.Add(new System.ComponentModel.SortDescription("joinsum", System.ComponentModel.ListSortDirection.Descending));
            //if (!(this.FindResource("keyVisibilityTopManagers") as VisibilityTopManagers).IsMember & (this.accountComboBox.SelectedIndex > -1 && ((this.accountComboBox.SelectedItem as DataRowView).Row as PaymentDS.tableLegalEntityRow).istop))
            //{
            //    this.TransButton.IsEnabled = false;
            //    this.TransDataGrid.IsReadOnly = true;
            //}
            //else
            //{
            //    this.TransButton.IsEnabled = true;
            //    TransDataGrid.IsReadOnly = false;
            //}
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

        private void TransButton_Click(object sender, RoutedEventArgs e)
        {
            if (paymentEndEdit() & this.TransDataGrid.CommitEdit(DataGridEditingUnit.Cell, true) & this.TransDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
            {
                decimal tsum;
                PaymentDS ds = myparent.thisDS;
                object ssum = ds.tableTransaction.Compute("Sum(dsum)", "ppid=" + this.Payment.ppid.ToString());

                if (DBNull.Value.Equals(ssum))
                    tsum = this.Payment.ppSum;
                else
                    tsum = this.Payment.ppSum - (decimal)ssum;
                if (tsum > 0M)
                {
                    BindingListCollectionView bview = CollectionViewSource.GetDefaultView(this.TransDataGrid.ItemsSource) as BindingListCollectionView;
                    DataRowView newviewrow = bview.AddNew() as DataRowView;
                    PaymentDS.tableTransactionRow newrow = newviewrow.Row as PaymentDS.tableTransactionRow;
                    newrow.idC = ds.tableCustomerName.FindBycustomerID(this.Payment.payerid).accountid;
                    newrow.idD = this.Payment.accountid;
                    newrow.dsum = tsum;
                    if (!this.Payment.IsdeductedNull()) newrow.datetran = this.Payment.deducted;
                    if (!this.Payment.IspurposeNull()) newrow.descr = this.Payment.purpose;
                    newrow.ppid = this.Payment.ppid;
                    //newrow.EndEdit();
                    //ds.tableTransaction.AddtableTransactionRow(newrow);
                    newviewrow.EndEdit();

                    DataGridCellInfo currcell = new DataGridCellInfo(newviewrow, this.TransDataGrid.Columns[1]);
                    this.TransDataGrid.SelectedCells.Clear();
                    this.TransDataGrid.SelectedCells.Add(currcell);
                    this.TransDataGrid.CurrentCell = currcell;
                }
                else
                {
                    PopupText.Text = "Платежка уже полностью проведена!";
                    popInf.PlacementTarget = sender as UIElement;
                    popInf.IsOpen = true;
                }
            }
        }
        private void DCJoinAllButton_Click(object sender, RoutedEventArgs e)
        {
            int idD;
            decimal freesum;
            PaymentDS.tableDCJoinRow dcjrow;
            PaymentDS ds = myparent.thisDS;
            this.TransDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            this.TransDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            if (JoinsDataGrid.SelectedItems.Count > 0)
            {
                PaymentDS.tableTransactionRow trow = ds.tableTransaction.FindByidtran(((JoinsDataGrid.SelectedItems[0] as DataRowView).Row as PaymentDS.tableDCJoinRow).idtran);
                freesum = trow.freesum;
                idD = trow.idD;
                foreach (DataRowView viewrow in JoinsDataGrid.SelectedItems)
                {
                    dcjrow = viewrow.Row as PaymentDS.tableDCJoinRow;
                    if (dcjrow.status < 500 && (dcjrow.joinsum != dcjrow.freesum) & (freesum > 0M))
                    {
                        if (!(dcjrow.IsidlegalNull() || idD == dcjrow.idlegal))
                        {
                            if (MessageBox.Show("Получатель в счете " + (dcjrow.IsdocnumNull() ? string.Empty : dcjrow.docnum) + " и проводке не совпадают!\nРазносить на этот счет?", "Разноска", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            {
                                if (dcjrow.joinsum > 0M)
                                {
                                    dcjrow.joinsum = 0M;
                                    dcjrow.EndEdit();
                                }
                                continue;
                            }
                        }
                        if ((freesum + dcjrow.joinsum - dcjrow.freesum) > 0M)
                        {
                            freesum = freesum + dcjrow.joinsum - dcjrow.freesum;
                            dcjrow.joinsum = dcjrow.freesum;
                        }
                        else
                        {
                            dcjrow.joinsum = dcjrow.joinsum + freesum;
                            freesum = 0M;
                        }
                        dcjrow.EndEdit();
                    }
                }
            }
            else
            {
                DataRow[] tranrows;
                DataRow[] jrows;
                PaymentDS.tableTransactionRow tranrow;
                if (this.TransDataGrid.SelectedItems.Count > 0)
                {
                    int i = 0;
                    tranrows = new DataRow[this.TransDataGrid.SelectedItems.Count];
                    foreach (DataRowView rowview in this.TransDataGrid.SelectedItems)
                    {
                        tranrows[i] = rowview.Row;
                        i++;
                    }
                }
                else
                {
                    tranrows = ds.tableTransaction.Select("ppid=" + this.Payment.ppid.ToString());
                }
                foreach (DataRow row in tranrows)
                {
                    tranrow = row as PaymentDS.tableTransactionRow;
                    jrows = ds.tableDCJoin.Select("idtran=" + tranrow.idtran.ToString(), "dcjoinsort DESC,docnum");
                    DCJoin(jrows, tranrow.dsum, tranrow.idD);
                }
            }
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                try
                {
                    Payment.Refresh();
                    PaymentDS ds = myparent.thisDS;
                    PaymentDSTableAdapters.TransactionAdapter tranadapter = new PaymentDSTableAdapters.TransactionAdapter();
                    tranadapter.ClearBeforeFill = false;
                    tranadapter.Fill(ds.tableTransaction, Payment.ppid);
                    PaymentDSTableAdapters.DCJoinAdapter dcjadapter = new PaymentDSTableAdapters.DCJoinAdapter();
                    dcjadapter.ClearBeforeFill = false;
                    DataRow[] tranrows = ds.tableTransaction.Select("ppid=" + this.Payment.ppid.ToString());
                    foreach (DataRow row in tranrows)
                    {
                        dcjadapter.Fill(ds.tableDCJoin, (row as PaymentDS.tableTransactionRow).idtran, null);
                    }
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
            if (MessageBox.Show("Отменить все несохраненные изменения?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                PaymentDS ds = myparent.thisDS;
                if (this.Payment == null)
                {
                    DataRow[] payrows = ds.tablePayment.Select("ppid=" + ppid.ToString(), string.Empty, DataViewRowState.Deleted);
                    if (payrows.Length > 0)
                    {
                        payrows[0].RejectChanges();
                        DataView payview = myparent.thisDS.tablePayment.DefaultView;
                        foreach (DataRowView payrow in payview)
                        {
                            if (payrow.Row.Field<int>("ppid") == ppid)
                            {
                                this.PaymentRowView = payrow;
                                break;
                            }
                        }
                        ppid = 0;
                    }
                    else
                        return;
                }
                this.TransDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                this.TransDataGrid.CancelEdit(DataGridEditingUnit.Row);
                this.JoinsDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                this.JoinsDataGrid.CancelEdit(DataGridEditingUnit.Row);
                DataRow[] tranrows = ds.tableTransaction.Select("ppid=" + this.Payment.ppid.ToString(), string.Empty, DataViewRowState.CurrentRows | DataViewRowState.Deleted);
                if (this.Payment.RowState == DataRowState.Added)
                {
                    foreach (DataRow row in tranrows)
                    {
                        DataRow[] dcjoinrows = row.GetChildRows("tableTransaction_tableDCJoin");
                        foreach (DataRow jrow in dcjoinrows)
                        {
                            jrow.Delete();
                        }
                        row.RejectChanges();
                    }
                    this.PaymentRowView.CancelEdit();
                    this.Payment.RejectChanges();
                }
                else
                {
                    if (this.Payment.RowState != DataRowState.Unchanged)
                    {
                        this.PaymentRowView.CancelEdit();
                        this.Payment.RejectChanges();
                    }
                    foreach (DataRow row in tranrows)
                    {
                        if (row.RowState == DataRowState.Deleted)
                        {
                            row.RejectChanges();
                            DataRow[] dcjoinrows = ds.tableDCJoin.Select("idtran=" + row.Field<int>("idtran"), string.Empty, DataViewRowState.Deleted);
                            foreach (DataRow jrow in dcjoinrows)
                            {
                                jrow.RejectChanges();
                            }
                        }
                        else
                        {
                            DataRow[] dcjoinrows = row.GetChildRows("tableTransaction_tableDCJoin");
                            if (row.RowState == DataRowState.Added)
                            {
                                foreach (DataRow jrow in dcjoinrows)
                                {
                                    jrow.Delete();
                                }
                                row.RejectChanges();
                            }
                            else
                            {
                                row.RejectChanges();
                                foreach (DataRow jrow in dcjoinrows)
                                {
                                    jrow.RejectChanges();
                                }
                            }
                        }
                    }
                }
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
            if (MessageBox.Show("Удалть все сведения о платеже?", "Платеж", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                ppid = this.Payment.ppid;
                this.Payment.Delete();
                this.PaymentRowView = null;
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void payerComboBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.payerComboBox.Text.Length > 0)
            {
                ClientWin win = new ClientWin();
                win.Show();
                win.CustomerNameList.Text = this.payerComboBox.Text;
            }
        }

        private void PaymentInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.OtherNoJoinPayment.CurrentItem is DataRowView)
                myparent.PaymentInfoOpenForm(this.OtherNoJoinPayment.CurrentItem as DataRowView);
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

        private void TransDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                if (PaymentRowView.IsNew) PaymentRowView.EndEdit();
            }
            catch (NoNullAllowedException)
            {
                MessageBox.Show("Одно из обязательных для заполнения полей оставлено пустым. \n Введите значение в поле.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void accountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindingExpression transBinding = TransDataGrid.GetBindingExpression(DataGrid.ItemsSourceProperty);
            transBinding.UpdateTarget();
            if (!(this.FindResource("keyVisibilityTopManagers") as VisibilityTopManagers).IsMember & (e.AddedItems.Count>0 && ((e.AddedItems[0] as DataRowView).Row as PaymentDS.tableLegalEntityRow).istop))
            {
                this.TransButton.IsEnabled = false;
                this.TransDataGrid.IsReadOnly = true;
            }
            else
            {
                this.TransButton.IsEnabled = true;
                TransDataGrid.IsReadOnly = false;
            }
        }

        private bool SaveChanges()
        {
            bool isSuccess = false;
            bool UpdatPayBinding = false; bool UpdatTransBinding = false;
            DataGridCellInfo transcell;
            IInputElement focelm = FocusManager.GetFocusedElement(this);
            FocusManager.SetFocusedElement(this, SaveButton);
            if ((focelm is DependencyObject) && Validation.GetHasError(focelm as DependencyObject)) return false;
            try
            {
                PaymentDS ds = myparent.thisDS;
                PaymentDSTableAdapters.PaymentAdapter padapter = new PaymentDSTableAdapters.PaymentAdapter();
                PaymentDSTableAdapters.TransactionAdapter tranadapter = new PaymentDSTableAdapters.TransactionAdapter();
                PaymentDSTableAdapters.DCJoinAdapter dcjadapter = new PaymentDSTableAdapters.DCJoinAdapter();
                if (this.mainGrid.DataContext == null)
                {
                    tranadapter.Update(ds.tableTransaction.Select("ppid=" + ppid.ToString(), string.Empty, DataViewRowState.Deleted));
                    padapter.Update(ds.tablePayment.Select("ppid=" + ppid.ToString(), string.Empty, DataViewRowState.Deleted));
                }
                else
                {
                    if (!paymentEndEdit()) return false;
                    this.TransDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                    this.TransDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                    this.JoinsDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                    this.JoinsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);

                    UpdatPayBinding = this.Payment.RowState == DataRowState.Added;
                    if (UpdatPayBinding) transcell = this.TransDataGrid.CurrentCell;
                    if (this.TransDataGrid.SelectedCells.Count > 0 && this.TransDataGrid.SelectedCells[0].Item is DataRowView) UpdatTransBinding = (this.TransDataGrid.SelectedCells[0].Item as DataRowView).Row.RowState == DataRowState.Added;
                    ppid = this.Payment.ppid;
                    tranadapter.Update(ds.tableTransaction.Select("ppid=" + ppid.ToString(), string.Empty, DataViewRowState.Deleted));
                    DataRow[] tranrows = ds.tableTransaction.Select("ppid=" + ppid.ToString());
                    padapter.Adapter.InsertCommand.Parameters["@pptype"].Value = (this.FindResource("keyVisibilityLAccounts") as VisibilityLAccounts).IsMember & !(this.FindResource("keyVisibilityAccounts") as VisibilityAccounts).IsMember ? "parlg" : "parcl";
                    padapter.Update(this.Payment);
                    if (UpdatPayBinding)
                    {
                        BindingExpression transbinding = TransDataGrid.GetBindingExpression(DataGrid.ItemsSourceProperty);
                        transbinding.UpdateTarget();
                        this.TransDataGrid.CurrentCell = transcell;
                    }
                    tranadapter.Update(tranrows);
                    if (UpdatTransBinding)
                    {
                        BindingExpression joinbinding = JoinsDataGrid.GetBindingExpression(DataGrid.ItemsSourceProperty);
                        joinbinding.UpdateTarget();
                    }
                    int rowcount = 0;
                    DataRow[] tmp = new DataRow[ds.tableDCJoin.Count];
                    DataRow[] dcjrows = new DataRow[ds.tableDCJoin.Count];
                    foreach (DataRow row in tranrows)
                    {
                        tmp = row.GetChildRows("tableTransaction_tableDCJoin");
                        tmp.CopyTo(dcjrows, rowcount);
                        rowcount = rowcount + tmp.Length;
                    }
                    dcjadapter.Update(DCJSort(dcjrows, rowcount));
                    if (this.Payment.RowState != DataRowState.Deleted)
                    {
                        this.Payment.sumpay = this.Payment.ppSum - this.Payment.noPaySum;
                        this.Payment.joinsum = this.Payment.ppSum - this.Payment.freeSum;
                        this.Payment.AcceptChanges();
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
                }
                else if (ex is System.Data.NoNullAllowedException)
                {
                    MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                this.Activate();
                if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    isSuccess = SaveChanges();
                }
            }
            FocusManager.SetFocusedElement(this, focelm);
            return isSuccess;
        }
        private bool paymentEndEdit()
        {
            IInputElement fcontrol = System.Windows.Input.FocusManager.GetFocusedElement(this);
            if (fcontrol is TextBox & this.Payment.RowState != DataRowState.Detached)
            {
                BindingExpression be;
                be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                if (be != null)
                {
                    //DataRow row = this.Payment as DataRow;
                    //decimal d;
                    //DateTime dt;
                    //bool isDirty = false;
                    //switch (be.ParentBinding.Path.Path)
                    //{
                    //    case "ppSum":
                    //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !decimal.TryParse((fcontrol as TextBox).Text, out d) || row.Field<Decimal>(be.ParentBinding.Path.Path) != d;
                    //        break;
                    //    case "ppDate":
                    //    case "deducted":
                    //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !DateTime.TryParse((fcontrol as TextBox).Text, out dt) || row.Field<DateTime>(be.ParentBinding.Path.Path) != dt;
                    //        break;
                    //    case "ppNumber":
                    //    case "purpose":
                    //    case "note":
                    //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !(fcontrol as TextBox).Text.Equals(row.Field<string>(be.ParentBinding.Path.Path));
                    //        break;
                    //    default:
                    //        isDirty = true;
                    //        MessageBox.Show("Поле не добавлено в обработчик сохранения без потери фокуса!", "Сохранение изменений");
                    //        break;
                    //}
                    if (be.IsDirty) be.UpdateSource();
                    return !be.HasError;
                }
            }
            this.Payment.EndEdit();
            return true;
        }
        private DataRow[] DCJSort(DataRow[] dcjarray, int size)
        {
            bool issort;
            int i, j;
            DataRow tmp;
            DataRow[] dcjsort = new DataRow[size];
            do
            {
                issort = true;
                for (j = 0; j < size - 1; ++j) // внутренний цикл прохода
                {
                    if ((dcjarray[j + 1] as PaymentDS.tableDCJoinRow).joinsum < (dcjarray[j] as PaymentDS.tableDCJoinRow).joinsum)
                    {
                        tmp = dcjarray[j + 1];
                        dcjarray[j + 1] = dcjarray[j];
                        dcjarray[j] = tmp;
                        issort = false;
                    }
                }
            } while (!issort);
            for (i = 0; i < size; ++i)
            {
                dcjsort[i] = dcjarray[i];
            }
            return dcjsort;
        }

        private void payerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0 && e.AddedItems[0] != e.RemovedItems[0])
            {

            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e) //Bug ComboBoxItem
        { (sender as ComboBox).IsDropDownOpen = true; (sender as ComboBox).IsDropDownOpen = false; }

        private void totalNoJoinButton_Click(object sender, RoutedEventArgs e)
        {
            DataRow[] jrows, payrows;
            List<PaymentDS.tableTransactionRow> tranrows = new List<PaymentDS.tableTransactionRow>();
            PaymentDSTableAdapters.TransactionAdapter tranadapter = new PaymentDSTableAdapters.TransactionAdapter();
            tranadapter.ClearBeforeFill = false;
            PaymentDSTableAdapters.DCJoinAdapter dcjadapter = new PaymentDSTableAdapters.DCJoinAdapter();
            dcjadapter.ClearBeforeFill = false;
            PaymentDS ds = myparent.thisDS;
            this.JoinsDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            this.JoinsDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            try
            {
                if (OtherNoJoinPayment.SelectedItems.Count > 0)
                {
                    payrows = new DataRow[OtherNoJoinPayment.SelectedItems.Count];
                    for (int i = 0; i < OtherNoJoinPayment.SelectedItems.Count; i++)
                    {
                        payrows[i] = (OtherNoJoinPayment.SelectedItems[i] as DataRowView).Row;
                    }
                }
                else
                {
                    payrows = ds.tablePayment.Select("payerid=" + Payment.payerid.ToString() + " AND ppid<>" + Payment.ppid.ToString() + " AND freeSum>0");
                }
                foreach (PaymentDS.tablePaymentRow payrow in payrows)
                {
                    tranadapter.Fill(ds.tableTransaction, payrow.ppid);
                    foreach (PaymentDS.tableTransactionRow tranrow in payrow.GettableTransactionRows())
                    {
                        dcjadapter.Fill(ds.tableDCJoin, tranrow.idtran, null);
                        if (tranrow.freesum > 0M)
                        {
                            jrows = ds.tableDCJoin.Select("idtran=" + tranrow.idtran.ToString(), "dcjoinsort DESC,docnum");
                            //ds.tableDCJoin.Select("idtran=" + tranrow.idtran.ToString() + " AND idlegal=", "docnum").CopyTo(jrows, jrows.Length);
                            DCJoin(jrows, tranrow.dsum, tranrow.idD);
                            tranadapter.Update(tranrow);
                            dcjadapter.Update(jrows);
                        }
                    }
                }
            }
            #region Catch
            catch (System.Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999) MessageBox.Show(err.Message, "Разноска", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                        {
                            errs.Append(sqlerr.Message + "\n");
                        }
                        MessageBox.Show(errs.ToString(), "Разноска", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Разноска", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }
            #endregion
        }
        private void NoJoinPayment_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded | e.ListChangedType == System.ComponentModel.ListChangedType.ItemChanged | e.ListChangedType == System.ComponentModel.ListChangedType.ItemDeleted | e.ListChangedType == System.ComponentModel.ListChangedType.Reset)
                TotalNoJoinRefresh(sender as DataView);
        }
        private void TotalNoJoinRefresh(DataView view)
        {
            decimal t = 0M;
            foreach (DataRowView rowview in view)
            {
                PaymentDS.tablePaymentRow row = rowview.Row as PaymentDS.tablePaymentRow;
                if (!row.IsnojoinsumNull()) t = t + row.freeSum;
            }
            this.totalNoJoinTexBox.Text = t.ToString("N");
            this.totalNoJoinButton.IsEnabled = t > 0M;
        }
        private decimal DCJoin(DataRow[] jrows,decimal freesum,int estimatedrecipient)
        {
            PaymentDS.tableDCJoinRow dcjrow;
            foreach (DataRow jrow in jrows)
            {
                dcjrow = jrow as PaymentDS.tableDCJoinRow;
                if (dcjrow.status == 500) continue;
                if (!(dcjrow.IsidlegalNull() || estimatedrecipient == dcjrow.idlegal))
                {
                    if (MessageBox.Show("Получатель в счете " + (dcjrow.IsdocnumNull() ? string.Empty : dcjrow.docnum) + " и проводке не совпадают!\nРазносить на этот счет?", "Разноска", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        if (dcjrow.joinsum > 0M)
                        {
                            dcjrow.joinsum = 0M;
                            dcjrow.EndEdit();
                        }
                        continue;
                    }
                }
                if (dcjrow.joinsum == dcjrow.freesum & !(freesum - dcjrow.joinsum < 0M))
                {
                    freesum = freesum - dcjrow.joinsum;
                }
                else if ((freesum - dcjrow.freesum) > 0M)
                {
                    freesum = freesum - dcjrow.freesum;
                    dcjrow.joinsum = dcjrow.freesum;
                }
                else
                {
                    dcjrow.joinsum = freesum;
                    freesum = 0M;
                }
                dcjrow.EndEdit();
                if (freesum == 0M) break;
            }
            return freesum;
        }
    }

    public class AccountLegalConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, System.Globalization.CultureInfo cultrure)
        {
            string namelegal = string.Empty;
            ReferenceDS refDS = App.Current.TryFindResource("keyReferenceDS") as ReferenceDS;
            if (refDS != null)
            {
                if (refDS.tableLegalEntity.Count == 0)
                {
                    ReferenceDSTableAdapters.LegalEntityAdapter adapter = new ReferenceDSTableAdapters.LegalEntityAdapter();
                    adapter.Fill(refDS.tableLegalEntity);
                }
                DataRow[] row = refDS.tableLegalEntity.Select("accountid=" + value.ToString());
                if (row.Length > 0) namelegal = (row[0] as ReferenceDS.tableLegalEntityRow).namelegal;
            }
            return namelegal;
        }
        public object ConvertBack(object value, Type TargetType, object parameter, System.Globalization.CultureInfo cultrure)
        {
            int idlegal = 0;
            ReferenceDS refDS = App.Current.TryFindResource("keyReferenceDS") as ReferenceDS;
            if (refDS != null)
            {
                if (refDS.tableLegalEntity.Count == 0)
                {
                    ReferenceDSTableAdapters.LegalEntityAdapter adapter = new ReferenceDSTableAdapters.LegalEntityAdapter();
                    adapter.Fill(refDS.tableLegalEntity);
                }
                DataRow[] row = refDS.tableLegalEntity.Select("namelegal=" + value.ToString());
                if (row.Length > 0) idlegal = (row[0] as ReferenceDS.tableLegalEntityRow).accountid;
            }
            if (idlegal != 0) return idlegal; else return null;
        }
    }
}
