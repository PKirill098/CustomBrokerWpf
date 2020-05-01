using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для InvoiceWin.xaml
    /// </summary>
    public partial class InvoiceListWin : Window
    {
        internal InvoiceDS thisDS;
        private InvoiceDSTableAdapters.InvoiceAdapter thisAdapter;
        public InvoiceListWin()
        {
            InitializeComponent();
            thisDS = new InvoiceDS();
            thisAdapter = new InvoiceDSTableAdapters.InvoiceAdapter();
            thisfilter = new SQLFilter("invoice", "AND");
        }

        private void winInvoice_Loaded(object sender, RoutedEventArgs e)
        {
            CustomBrokerWpf.InvoiceDSTableAdapters.tableCustomerNameTableAdapter customerAdapter = new CustomBrokerWpf.InvoiceDSTableAdapters.tableCustomerNameTableAdapter();
            customerAdapter.Fill(thisDS.tableCustomerName);
            CustomBrokerWpf.InvoiceDSTableAdapters.tableLegalEntityTableAdapter legalAdapter = new InvoiceDSTableAdapters.tableLegalEntityTableAdapter();
            legalAdapter.Fill(thisDS.tableLegalEntity);
            thisDS.tableLegalEntity.DefaultView.Sort = "namelegal";
            thisDS.tableCustomerName.DefaultView.Sort = "customerName";
            (this.mainDataGrid.FindResource("keyLegalEntityVS") as CollectionViewSource).Source = thisDS.tableLegalEntity;
            (this.mainDataGrid.FindResource("keyCustomerVS") as CollectionViewSource).Source = thisDS.tableCustomerName;
            thisDS.tableInvoice.DefaultView.Sort = "invoicedate Desc";
            DataLoad();
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
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                thisfilter.RemoveCurrentWhere();
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winInvoiceFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new PaymentListFilterWin();
                    ObjectWin.Owner = this;
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
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                DataLoad();
            }
        }
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Отменить несохраненные изменения?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.mainDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                this.mainDataGrid.CancelEdit(DataGridEditingUnit.Row);
                if (this.mainDataGrid.SelectedItem is DataRowView & this.mainDataGrid.SelectedItems.Count == 1)
                {
                    //bool isClosed = true;
                    InvoiceDS.tableInvoiceRow row = (this.mainDataGrid.SelectedItem as DataRowView).Row as InvoiceDS.tableInvoiceRow;
                    row.RejectChanges();
                    DataRow[] detailrows = thisDS.tableInvoiceDetail.Select("invoiceid=" + row.invoiceid.ToString(), string.Empty, DataViewRowState.CurrentRows | DataViewRowState.Deleted);
                    foreach (DataRow detrow in detailrows) detrow.RejectChanges();
                    //foreach (Window owwin in this.OwnedWindows)
                    //{
                    //    if (owwin is InvoiceWin && (owwin as InvoiceWin).Invoice.invoiceid == row.invoiceid)
                    //    {
                    //        owwin.Close();
                    //        break;
                    //    }
                    //}
                    //foreach (Window owwin in this.OwnedWindows)
                    //{
                    //    if (owwin is InvoiceWin && (owwin as InvoiceWin).Invoice.invoiceid == row.invoiceid)
                    //    {
                    //        isClosed = false;
                    //        break;
                    //    }

                    //}
                    //if (isClosed) 
                }
                else
                {
                    this.thisDS.RejectChanges();
                }
                //totalDataRefresh();
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
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            if (mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
            {
                InvoiceWin win = new InvoiceWin();
                win.InvoiceRowView = thisDS.tableInvoice.DefaultView.AddNew();
                win.Owner = this;
                win.Show();
                this.mainDataGrid.CurrentItem = win.InvoiceRowView;
                if (!this.mainDataGrid.IsFocused) this.mainDataGrid.Focus();
            }
            else MessageBox.Show("Ошибка в текущей строке, исправте!", "Добавление", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            //bool isClosed = true;
            if (this.mainDataGrid.SelectedItems.Count > 0 && MessageBox.Show("Удалть все сведения о счете?", "Счета", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                for (int i = 0; i < this.mainDataGrid.SelectedItems.Count; i++)
                {
                    DataRowView rowview = this.mainDataGrid.SelectedItems[i] as DataRowView;
                    rowview.Delete();
                    foreach (Window owwin in this.OwnedWindows)
                    {
                        if (owwin is InvoiceWin && (owwin as InvoiceWin).Invoice.invoiceid == (rowview.Row as InvoiceDS.tableInvoiceRow).invoiceid)
                        {
                            owwin.Close();
                            break;
                        }
                    }
                    //foreach (Window owwin in this.OwnedWindows)
                    //{
                    //    if (owwin is InvoiceWin && (owwin as InvoiceWin).Invoice.invoiceid == (rowview.Row as InvoiceDS.tableInvoiceRow).invoiceid)
                    //    {
                    //        isClosed = false;
                    //        break;
                    //    }

                    //}
                    //if (isClosed) 
                }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if ((e.OriginalSource as Button).Tag == null) return;
            this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, false);
            if (this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
            {
                InvoiceWin win = null;
                foreach (Window owwin in this.OwnedWindows)
                {
                    if (owwin.Name == "winInvoice")
                    {
                        if ((owwin as InvoiceWin).Invoice.invoiceid == (int)(e.OriginalSource as Button).Tag)
                        {
                            win = owwin as InvoiceWin;
                            break;
                        }
                    }
                }
                if (win == null)
                {
                    win = new InvoiceWin();
                    win.InvoiceRowView = this.mainDataGrid.CurrentItem as DataRowView;
                    win.Owner = this;
                    win.Show();
                }
                else
                {
                    win.Activate();
                    if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
                }
            }
            else MessageBox.Show("Ошибка в строке, исправте!", "Добавление", MessageBoxButton.OK, MessageBoxImage.Stop);
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
        private void customerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                InvoiceDS.tableCustomerNameRow row = (e.AddedItems[0] as DataRowView).Row as InvoiceDS.tableCustomerNameRow;
                DataRowView viewrow = this.mainDataGrid.CurrentItem as DataRowView;
                InvoiceDS.tableInvoiceRow invrow = viewrow.Row as InvoiceDS.tableInvoiceRow;

                if (!row.IspayaccountNull() & ((e.AddedItems.Count > 0 & e.RemovedItems.Count > 0) | invrow.IsaccountIdNull() || invrow.accountId == 0))
                {
                    //if (viewrow.IsNew) viewrow.EndEdit();
                    invrow.accountId = row.payaccount;
                }
            }
        }

        private void DataLoad()
        {
            if (!CloseChildren()) return;
            thisDS.tableInvoiceDetail.Clear();
            mainDataGrid.ItemsSource = null;
            this.thisAdapter.Fill(thisDS.tableInvoice, 0);
            mainDataGrid.ItemsSource = thisDS.tableInvoice;
            setFilterButtonImage();
            //totalDataRefresh();
        }
        private bool SaveChanges()
        {
            bool isSuccess = false;
            DataGridRow item;
            try
            {
                if (
                        this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true) &
                        this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true)
                    )
                {
                    thisAdapter.Update(thisDS.tableInvoice);
                    isSuccess = true;
                }
                else
                {
                    for (int i = 0; i < this.mainDataGrid.Items.Count; i++)
                    {
                        item = (DataGridRow)this.mainDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                        if ((item is DataGridRow) && Validation.GetHasError(item))
                        {
                            this.mainDataGrid.ScrollIntoView(item.Item);
                            this.mainDataGrid.SelectedItems.Add(item.Item);
                            MessageBox.Show(Validation.GetErrors(item)[0].ErrorContent.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                    }
                }
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
                    MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля или удалите платеж.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    isSuccess = SaveChanges();
                }
                for (int i = 0; i < this.mainDataGrid.Items.Count; i++)
                {
                    item = (DataGridRow)this.mainDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                    if ((item is DataGridRow) && Validation.GetHasError(item))
                    {
                        this.mainDataGrid.ScrollIntoView(item.Item);
                        this.mainDataGrid.SelectedItems.Add(item.Item);
                        break;
                    }
                }
            }
            return isSuccess;
        }
        private bool CloseChildren()
        {
            foreach (Window owwin in this.OwnedWindows)
            {
                owwin.Close();
            }
            bool isSuccess = true;
            if (this.OwnedWindows.Count > 0) isSuccess = false;
            return isSuccess;
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
                    MessageBox.Show("Применение фильтра невозможно. Не удалось сохранить изменения. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    thisfilter.RemoveCurrentWhere();
                    thisfilter = value;
                    if (this.IsLoaded) DataLoad();
                }
            }
        }
        public void runFilter()
        {
            if (!SaveChanges())
                MessageBox.Show("Применение фильтра невозможно. Не удалось сохранить изменения. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                DataLoad();
            }
        }
        private void setFilterButtonImage()
        {
            string uribitmap;
            if (thisfilter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (FilterButton.Content as Image).Source = bi3;
        }
        #endregion

        private void InvoiceExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainDataGrid.SelectedItems.Count > 0)
            {
                if (SaveChanges())
                {
                    InvoiceDSTableAdapters.InvoiceDetailAdapter detAdapter = null;
                    foreach (DataRowView viewrow in mainDataGrid.SelectedItems)
                    {
                        InvoiceDS.tableInvoiceRow invoice = viewrow.Row as InvoiceDS.tableInvoiceRow;
                        InvoiceDS.tableInvoiceDetailRow[] detailrows = invoice.GettableInvoiceDetailRows();
                        if (detailrows.Length == 0)
                        {
                            if (detAdapter == null)
                            {
                                detAdapter = new InvoiceDSTableAdapters.InvoiceDetailAdapter();
                                detAdapter.ClearBeforeFill = false;
                            }
                            detAdapter.Fill(thisDS.tableInvoiceDetail, invoice.invoiceid);
                        }
                        CustomBrokerWpf.Invoice invobj = new CustomBrokerWpf.Invoice();
                        invobj.CreateInvoiceExcel(invoice);
                    }
                }
            }
            else MessageBox.Show("Нет выделенных счетов", "Счета", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

    }
}
