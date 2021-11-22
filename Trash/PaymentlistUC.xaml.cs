using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for PaymentlistUC.xaml
    /// </summary>
    public partial class PaymentlistUC : UserControl
    {
        internal PaymentDS thisDS;
        public PaymentlistUC()
        {
            InitializeComponent();
            thisDS = new PaymentDS();
            thisfilter = new SQLFilter("payment", "AND");
        }

        private void winPaymentList_Loaded(object sender, RoutedEventArgs e)
        {
            CustomBrokerWpf.PaymentDSTableAdapters.tableCustomerNameTableAdapter customerAdapter = new CustomBrokerWpf.PaymentDSTableAdapters.tableCustomerNameTableAdapter();
            customerAdapter.Fill(thisDS.tableCustomerName);
            CustomBrokerWpf.PaymentDSTableAdapters.tableLegalEntityTableAdapter legalAdapter = new PaymentDSTableAdapters.tableLegalEntityTableAdapter();
            legalAdapter.Fill(thisDS.tableLegalEntity);
            thisDS.tableLegalEntity.DefaultView.Sort = "namelegal";
            thisDS.tableCustomerName.DefaultView.Sort = "customerName";
            (this.PaymentDataGrid.FindResource("keyLegalEntityVS") as CollectionViewSource).Source = thisDS.tableLegalEntity;
            (this.PaymentDataGrid.FindResource("keyPayerVS") as CollectionViewSource).Source = thisDS.tableCustomerName;
            thisDS.tablePayment.DefaultView.Sort = "ppDate Desc";
            DataLoad();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            PaymentDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.PaymentDataGrid.ItemsSource) as BindingListCollectionView;
            this.PaymentDataGrid.CurrentItem = view.AddNew();
            if (!this.PaymentDataGrid.IsFocused) this.PaymentDataGrid.Focus();
            this.PaymentDataGrid.CurrentCell = new DataGridCellInfo(this.PaymentDataGrid.CurrentItem, this.PaymentDataGrid.Columns[1]);
            //PaymentInfoOpenForm(this.PaymentDataGrid.CurrentItem as DataRowView);
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentDataGrid.CurrentItem is DataRowView)
            {
                if (((PaymentDataGrid.CurrentItem as DataRowView).Row as PaymentDS.tablePaymentRow).sumpay > 0M)
                {
                    if ((this.FindResource("keyVisibilityAccountVisors") as VisibilityAccountVisors).Visibility != System.Windows.Visibility.Visible)
                    {
                        MessageBox.Show("Недостаточно прав для удаления проведенного платежа!", "Удаление платежа", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    else
                    {
                        MessageBox.Show("Для удаления проведенного платежа используйте окно \"Оплата\"!\n", "Удаление платежа", MessageBoxButton.OK, MessageBoxImage.Stop);
                        PaymentInfo();
                    }
                    return;
                }
                if (MessageBox.Show("Удалить сведения о платеже?", "Платеж", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    (PaymentDataGrid.CurrentItem as DataRowView).Delete();
                }
            }
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                DataLoad();
            }
        }
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.PaymentDataGrid.SelectedItem is DataRowView & this.PaymentDataGrid.SelectedItems.Count == 1)
            {
                if ((this.PaymentDataGrid.SelectedItem as DataRowView).IsEdit | (this.PaymentDataGrid.SelectedItem as DataRowView).IsNew)
                {
                    this.PaymentDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                    this.PaymentDataGrid.CancelEdit(DataGridEditingUnit.Row);
                }
                else RejectRow((this.PaymentDataGrid.SelectedItem as DataRowView).Row as PaymentDS.tablePaymentRow);
            }
            else
            {
                if (MessageBox.Show("Отменить все несохраненные изменения?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    this.PaymentDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                    this.PaymentDataGrid.CancelEdit(DataGridEditingUnit.Row);
                    foreach (PaymentDS.tablePaymentRow row in this.thisDS.tablePayment.Rows)
                    {

                        RejectRow(row);
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

        private void PaymentInfoButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentInfo();
        }
        private void PaymentInfo()
        {
            if (!(this.PaymentDataGrid.CurrentItem is DataRowView)) return;
            this.PaymentDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            if (!this.PaymentDataGrid.CommitEdit(DataGridEditingUnit.Row, true)) return;
            PaymentInfoOpenForm(this.PaymentDataGrid.CurrentItem as DataRowView);
        }
        internal void PaymentInfoOpenForm(DataRowView ppitem)
        {
            PaymentDS.tablePaymentRow row = ppitem.Row as PaymentDS.tablePaymentRow;
            PaymentWin win = null;
            DependencyObject ownerwin= this.Parent;
            while (!(ownerwin is Window)) ownerwin = (ownerwin as FrameworkElement).Parent;
            foreach (Window owwin in (ownerwin as Window).OwnedWindows)
            {
                if (owwin.Name == "winPayment")
                {
                    if ((owwin as PaymentWin).Payment.ppid == row.ppid)
                    {
                        win = owwin as PaymentWin;
                        break;
                    }
                }
            }
            if (win == null)
            {
                win = new PaymentWin();
                win.PaymentRowView = ppitem;
                win.Owner = (ownerwin as Window);
                win.Show();
            }
            else
            {
                win.Activate();
                if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
            }
        }
        private void DataLoad()
        {
            try
            {
                if ((this.FindResource("keyVisibilityAccounts") as VisibilityAccounts).Visibility == System.Windows.Visibility.Visible
                    | (this.FindResource("keyVisibilityLAccounts") as VisibilityLAccounts).Visibility == System.Windows.Visibility.Visible)
                {
                    DependencyObject ownerwin = this.Parent;
                    while (!(ownerwin is Window)) ownerwin = (ownerwin as FrameworkElement).Parent;
                    foreach (Window owwin in (ownerwin as Window).OwnedWindows)
                    {
                        if (owwin.Name == "winPayment") owwin.Close();
                    }
                    bool retrn = false;
                    foreach (Window owwin in (ownerwin as Window).OwnedWindows)
                    {
                        if (owwin.Name == "winPayment") retrn = true;
                    }
                    if (retrn) return;
                }

                thisDS.tableDCJoin.Clear();
                thisDS.tableTransaction.Clear();
                PaymentDataGrid.ItemsSource = null;
                CustomBrokerWpf.PaymentDSTableAdapters.PaymentAdapter payadapter = new PaymentDSTableAdapters.PaymentAdapter();
                payadapter.Fill(thisDS.tablePayment, thisfilter.FilterWhereId);
                PaymentDataGrid.ItemsSource = thisDS.tablePayment.DefaultView;
                SetFilterButtonImage();
            }
            catch (Exception ex)
            {
                ExpectionShowErrMessage(ex, "Загрузка данных");
                if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    DataLoad();
                }

            }
        }
        internal bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                PaymentDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                PaymentDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                if (thisDS.tablePayment.HasErrors)
                {
                    DataRow rowInError;
                    rowInError = thisDS.tablePayment.GetErrors()[0];
                    if (rowInError.GetColumnsInError().Length > 0)
                    {
                        throw new SystemException(rowInError.GetColumnError(rowInError.GetColumnsInError()[0]));
                    }
                    else
                        throw new SystemException(rowInError.RowError);
                }
                PaymentDSTableAdapters.PaymentAdapter padapter = new PaymentDSTableAdapters.PaymentAdapter();
                padapter.Adapter.InsertCommand.Parameters["@pptype"].Value = (this.FindResource("keyVisibilityLAccounts") as VisibilityLAccounts).IsMember & !(this.FindResource("keyVisibilityAccounts") as VisibilityAccounts).IsMember ? "parlg" : "parcl";
                padapter.Update(thisDS.tablePayment);
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
                    DataRow[] rowerrs = thisDS.tablePayment.GetErrors();
                    foreach (DataRow row in rowerrs) row.ClearErrors();
                }
                else if (ex is System.Data.NoNullAllowedException)
                {
                    MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void RejectRow(PaymentDS.tablePaymentRow row)
        {
            if (row.RowState == DataRowState.Deleted)
            {
                row.RejectChanges();
            }
            DataRow[] tranrows = thisDS.tableTransaction.Select("ppid=" + row.ppid.ToString(), string.Empty, DataViewRowState.CurrentRows | DataViewRowState.Deleted);
            if (row.RowState == DataRowState.Added)
            {
                foreach (DataRow tranrow in tranrows)
                {
                    DataRow[] dcjoinrows = tranrow.GetChildRows("tableTransaction_tableDCJoin");
                    foreach (DataRow jrow in dcjoinrows)
                    {
                        jrow.Delete();
                    }
                    tranrow.RejectChanges();
                }
                row.RejectChanges();
            }
            else
            {
                if (row.RowState != DataRowState.Unchanged)
                {
                    row.RejectChanges();
                }
                foreach (DataRow tranrow in tranrows)
                {
                    if (tranrow.RowState == DataRowState.Deleted)
                    {
                        tranrow.RejectChanges();
                        DataRow[] dcjoinrows = thisDS.tableDCJoin.Select("idtran=" + tranrow.Field<int>("idtran"), string.Empty, DataViewRowState.Deleted);
                        foreach (DataRow jrow in dcjoinrows)
                        {
                            jrow.RejectChanges();
                        }
                    }
                    else
                    {
                        DataRow[] dcjoinrows = tranrow.GetChildRows("tableTransaction_tableDCJoin");
                        if (tranrow.RowState == DataRowState.Added)
                        {
                            foreach (DataRow jrow in dcjoinrows)
                            {
                                jrow.Delete();
                            }
                            tranrow.RejectChanges();
                        }
                        else
                        {
                            tranrow.RejectChanges();
                            foreach (DataRow jrow in dcjoinrows)
                            {
                                jrow.RejectChanges();
                            }
                        }
                    }
                }
            }
        }

        #region Filter
        private CustomBrokerWpf.SQLFilter thisfilter;
        public bool IsShowFilter
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        public SQLFilter Filter
        {
            get { return thisfilter; }
            set
            {
                if (this.IsLoaded && !SaveChanges())
                    MessageBox.Show("Применение фильтра невозможно. Окно содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    thisfilter.RemoveCurrentWhere();
                    thisfilter = value;
                    if (this.IsLoaded) DataLoad(); ;
                }
            }
        }
        public void RunFilter()
        {
            if (!SaveChanges())
                MessageBox.Show("Применение фильтра невозможно. Окно содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                DataLoad();
            }
        }
        private void SetFilterButtonImage()
        {
            string uribitmap;
            if (thisfilter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (FilterButton.Content as Image).Source = bi3;
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            DependencyObject ownerwin = this.Parent;
            while (!(ownerwin is Window)) ownerwin = (ownerwin as FrameworkElement).Parent;
            foreach (Window item in (ownerwin as Window).OwnedWindows)
            {
                if (item.Name == "winPaymentListFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new PaymentListFilterWin();
                    ObjectWin.Owner = (ownerwin as Window);
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
            else
            {
                if (ObjectWin != null)
                {
                    ObjectWin.Close();
                }
            }
        }
        #endregion

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

        private void PaymentDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && (e.Row.Item as DataRowView).IsNew)
            {
                //thisDS.tablePayment.TableNewRow += new tablePaymentRowChangeEventHandler(PaymentRowChanging);
                //    using(System.Data.SqlClient.SqlConnection con =new System.Data.SqlClient.SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                //    {
                //        PaymentDS.tablePaymentRow newrow = (e.Row.Item as DataRowView).Row as PaymentDS.tablePaymentRow;
                //        System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand();
                //        comm.CommandType = CommandType.StoredProcedure;
                //        comm.CommandText = "account.PPCheckDouble_sp";
                //        if(!newrow.IsppNumberNull())
                //        {
                //            System.Data.SqlClient.SqlParameter ppNumber = new System.Data.SqlClient.SqlParameter("@ppNumber", newrow.ppNumber);
                //            comm.Parameters.Add(ppNumber);
                //        }
                //        if (!newrow.IsppDateNull())
                //        {
                //            System.Data.SqlClient.SqlParameter ppDate = new System.Data.SqlClient.SqlParameter("@ppDate", newrow.ppDate);
                //            comm.Parameters.Add(ppDate);
                //        }
                //        System.Data.SqlClient.SqlParameter accountid = new System.Data.SqlClient.SqlParameter("@accountid", newrow.accountid);
                //        comm.Parameters.Add(accountid);
                //        System.Data.SqlClient.SqlParameter ppSum = new System.Data.SqlClient.SqlParameter("@ppSum", newrow.ppSum);
                //        comm.Parameters.Add(ppSum);
                //        System.Data.SqlClient.SqlDataReader reader = comm.ExecuteReader();
                //        if (reader.Read())
                //        {

                //        }
                //    }
            }
        }
    }
}
