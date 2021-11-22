using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ExpenditureWin.xaml
    /// </summary>
    public partial class ExpenditureListWin : Window, ISQLFiltredWindow
    {
        internal ExpenditureDS thisDS;
        public ExpenditureListWin()
        {
            InitializeComponent();
            thisDS = new ExpenditureDS();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refds = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refds.tableExpenditureItem.Count == 0) refds.ExpenditureItemRefresh();
            if (refds.tableExpenditureType.Count == 0) refds.ExpenditureTypeRefresh();
            if (refds.tableAccountCurrency.Count == 0) refds.AccountCurrencyRefresh();
            if (refds.tableFullNumber.Count == 0) refds.FullNumberRefresh();
            if (refds.tableLegalEntity.Count == 0) refds.LegalEntityRefresh();
            if (refds.tableCustomerName.Count == 0) refds.CustomerNameRefresh();
            (mainDataGrid.FindResource("keyExpenditureTypeVS") as CollectionViewSource).Source = refds.tableExpenditureType.DefaultView;
            (mainDataGrid.FindResource("keyExpenditureItemVS") as CollectionViewSource).Source = refds.tableExpenditureItem.DefaultView;
            (mainDataGrid.FindResource("keyExpenditureCurrecyVS") as CollectionViewSource).Source = refds.tableAccountCurrency.DefaultView;
            (mainDataGrid.FindResource("keyParcelFullNumberVS") as CollectionViewSource).Source = refds.tableFullNumber.DefaultView;//new System.Data.DataView(refds.tableFullNumber, "status<300", "sort DESC", System.Data.DataViewRowState.CurrentRows);
            (mainDataGrid.FindResource("keyLegalEntityVS") as CollectionViewSource).Source = refds.tableLegalEntity.DefaultView;
            (mainDataGrid.FindResource("keyClientVS") as CollectionViewSource).Source = refds.tableCustomerName.DefaultView;
            (mainDataGrid.FindResource("keyRecipientListVS") as CollectionViewSource).Source = KirillPolyanskiy.CustomBrokerWpf.References.Contractors;
            dataLoad();
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
                if (item.Name == "winExpenditureFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new ExpenditureFilterWin();
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
            if (CloseChildren() & SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                dataLoad();
            }
        }
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Отменить несохраненные изменения?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (this.mainDataGrid.SelectedItem is DataRowView & this.mainDataGrid.SelectedItems.Count == 1)
                {
                    this.mainDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                    this.mainDataGrid.CancelEdit(DataGridEditingUnit.Row);
                    RejectRow((this.mainDataGrid.SelectedItem as DataRowView).Row as ExpenditureDS.tableExpenditureRow);
                }
                else
                {
                    this.mainDataGrid.CancelEdit(DataGridEditingUnit.Cell);
                    this.mainDataGrid.CancelEdit(DataGridEditingUnit.Row);
                    for (int i = 0; i < this.thisDS.tableExpenditure.Rows.Count; i++)
                    {
                        RejectRow(this.thisDS.tableExpenditure.Rows[i] as ExpenditureDS.tableExpenditureRow);
                    }
                    this.thisDS.tableExpenditure.RejectChanges();
                }
                totalDataRefresh();
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
                ExpenditureWin win = new ExpenditureWin();
                win.ExpenditureRowView = thisDS.tableExpenditure.DefaultView.AddNew();
                win.Owner = this;
                win.Show();
                this.mainDataGrid.CurrentItem = win.ExpenditureRowView;
                if (!this.mainDataGrid.IsFocused) this.mainDataGrid.Focus();

            }
            else MessageBox.Show("Ошибка в текущей строке, исправте!", "Добавление", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалть все сведения о затрате?", "Затраты", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes && CloseChildren()) (this.mainDataGrid.CurrentItem as DataRowView).Delete();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void WithdrawalButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainDataGrid.CurrentItem is DataRowView)
            {
                ExpenditureDS.tableExpenditureRow row = (this.mainDataGrid.CurrentItem as DataRowView).Row as ExpenditureDS.tableExpenditureRow;
                if (row.opertype == 0)
                {
                    DataRowView rowview;
                    WithdrawalDS.tableWithdrawalDataTable table = new WithdrawalDS.tableWithdrawalDataTable();
                    table.nojoinsumColumn.Expression = "csum-joinsum";
                    WithdrawalDSTableAdapters.WithdrawalAdapter adapter = new WithdrawalDSTableAdapters.WithdrawalAdapter();
                    if (row.IspayIDNull())
                        rowview = table.DefaultView.AddNew();
                    else
                    {
                        adapter.Fill(table, null, row.payID);
                        if (table.Count != 0)
                            rowview = table.DefaultView[0];
                        else
                            rowview = table.DefaultView.AddNew();
                    }
                    WithdrawalWin win = new WithdrawalWin();
                    win.WithdrawalRowView = rowview;
                    win.Owner = this;
                    win.Show();
                }
            }
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.mainDataGrid.CurrentItem is DataRowView)) return;
            this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, false);
            if (this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
            {
                ExpenditureDS.tableExpenditureRow row = (this.mainDataGrid.CurrentItem as DataRowView).Row as ExpenditureDS.tableExpenditureRow;
                ExpenditureDSTableAdapters.ExpenditureAdapter adapter = new ExpenditureDSTableAdapters.ExpenditureAdapter();
                adapter.Update(row);
                ExpenditureWin win = null;
                foreach (Window owwin in this.OwnedWindows)
                {
                    if (owwin.Name == "winExpenditure")
                    {
                        if ((owwin as ExpenditureWin).ExpenditureRow.ExpenditureID == row.ExpenditureID)
                        {
                            win = owwin as ExpenditureWin;
                            break;
                        }
                    }
                }
                if (win == null)
                {
                    win = new ExpenditureWin();
                    win.ExpenditureRowView = this.mainDataGrid.CurrentItem as DataRowView;
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

        private void DataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }
        private void OperType_SelectedCanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count > 0 & e.RemovedItems.Count > 0)
            {
                ExpendetureOperType operrow = e.AddedItems[0] as ExpendetureOperType;
                ExpenditureDS.tableExpenditureRow exrow = (mainDataGrid.CurrentItem as DataRowView).Row as ExpenditureDS.tableExpenditureRow;
                if (exrow.HasVersion(System.Data.DataRowVersion.Original))
                {
                    if (operrow.Id != (sbyte)exrow["opertype", System.Data.DataRowVersion.Original])
                    {
                        exrow.SetpayIDNull();
                        exrow.SetlegalAccountIdNull();
                    }
                    else if (operrow.Id == (sbyte)exrow["opertype", System.Data.DataRowVersion.Original])
                    {
                        if (exrow["payID", System.Data.DataRowVersion.Original] == DBNull.Value)
                            exrow.SetpayIDNull();
                        else
                            exrow.payID = (int)exrow["payID", System.Data.DataRowVersion.Original];
                        if (exrow["legalAccountId", System.Data.DataRowVersion.Original] == DBNull.Value)
                            exrow.SetlegalAccountIdNull();
                        else
                            exrow.legalAccountId = (int)exrow["legalAccountId", System.Data.DataRowVersion.Original];
                    }
                }

                int columnIndex = 0;
                DataGridCell cell;
                System.Windows.Controls.Primitives.DataGridCellsPresenter presenter;
                DataGridRow row = (DataGridRow)mainDataGrid.ItemContainerGenerator.ContainerFromItem(mainDataGrid.CurrentItem);
                foreach (DataGridColumn column in mainDataGrid.Columns)
                    if (column.Header != null && column.Header.ToString() == "Источник")
                        columnIndex = column.DisplayIndex;
                presenter = GetVisualChild<System.Windows.Controls.Primitives.DataGridCellsPresenter>(row);
                cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                cell.IsEnabled = (e.AddedItems[0] as ExpendetureOperType).Id == 0;

                foreach (DataGridColumn column in mainDataGrid.Columns)
                    if (column.Header != null && column.Header.ToString() == "Клиент")
                        columnIndex = column.DisplayIndex;
                presenter = GetVisualChild<System.Windows.Controls.Primitives.DataGridCellsPresenter>(row);
                cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                cell.IsEnabled = (e.AddedItems[0] as ExpendetureOperType).Id == 1;
            }
        }
        public static T GetVisualChild<T>(System.Windows.Media.Visual parent) where T : System.Windows.Media.Visual
        {
            T child = default(T);
            int numVisuals = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                System.Windows.Media.Visual v = (System.Windows.Media.Visual)System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        private void dataLoad()
        {
            if (!CloseChildren()) return;

            this.mainDataGrid.ItemsSource = null;
            KirillPolyanskiy.CustomBrokerWpf.References.Contractors.Refresh();
            ExpenditureDSTableAdapters.ExpenditureAdapter adapter = new ExpenditureDSTableAdapters.ExpenditureAdapter();
            adapter.Fill(thisDS.tableExpenditure, thisfilter.FilterWhereId);
            //thisDS.tableExpenditure.DefaultView.Sort = "dateEx DESC";
            this.mainDataGrid.ItemsSource = thisDS.tableExpenditure.DefaultView;
            setFilterButtonImage();
            totalDataRefresh();
        }
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                if (
                        this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true) &
                        this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true)
                    )
                {
                    ExpenditureDSTableAdapters.ExpenditureAdapter adapter = new ExpenditureDSTableAdapters.ExpenditureAdapter();
                    adapter.Update(thisDS.tableExpenditure);
                    isSuccess = true;
                }
                else
                {
                    DataGridRow item;
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
                    MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля или удалите перевозку.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private bool CloseChildren()
        {
            foreach (Window owwin in this.OwnedWindows)
            {
                if (owwin.Name == "winExpenditure") owwin.Close();
            }
            bool isSuccess = true;
            foreach (Window owwin in this.OwnedWindows)
            {
                if (owwin.Name == "winExpenditure") isSuccess = false;
            }
            return isSuccess;
        }
        private void RejectRow(ExpenditureDS.tableExpenditureRow row)
        {
            bool isreject = false;
            foreach (Window owwin in this.OwnedWindows)
            {
                if (owwin.Name == "winExpenditure")
                {
                    ExpenditureWin exwin = owwin as ExpenditureWin;
                    if (exwin.ExpenditureRow.ExpenditureID == row.ExpenditureID)
                    {
                        exwin.Reject();
                        isreject = true;
                        break;
                    }
                }
            }
            this.mainDataGrid.CancelEdit();
            if (!isreject) row.RejectChanges();
        }

        #region Filter
        private CustomBrokerWpf.SQLFilter thisfilter = new SQLFilter("expenditure", "AND");
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
                    if (this.IsLoaded) dataLoad();
                }
            }
        }
        public void RunFilter()
        {
            if (!SaveChanges())
                MessageBox.Show("Применение фильтра невозможно. Не удалось сохранить изменения. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                dataLoad();
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

        #region Total Sum
        string totalOldCurrency;
        decimal totalOldValue;
        private void totalDataRefresh()
        {
            decimal totalSumPayRub = 0;
            ExpendetureTotalSumm itemTotalSumm;
            ListNotifyChanged<ExpendetureTotalSumm> list = new ListNotifyChanged<ExpendetureTotalSumm>();
            if (this.mainDataGrid.SelectedItems.Count > 1)
            {
                for (int i = 0; i < this.mainDataGrid.SelectedItems.Count; i++)
                {
                    if (this.mainDataGrid.SelectedItems[i] is DataRowView)
                    {
                        ExpenditureDS.tableExpenditureRow row = (this.mainDataGrid.SelectedItems[i] as DataRowView).Row as ExpenditureDS.tableExpenditureRow;
                        if (row.RowState != DataRowState.Deleted)
                        {
                            itemTotalSumm = list.Find(x => x.Currency == row.currency);
                            if (itemTotalSumm == null)
                            {
                                itemTotalSumm = new ExpendetureTotalSumm(row.currency);
                                list.Add(itemTotalSumm);
                            }
                            if (!row.IssumExNull()) itemTotalSumm.Expenditure = itemTotalSumm.Expenditure + row.sumEx;
                            if (!row.IssumPayCurrNull()) itemTotalSumm.PayCurrency = itemTotalSumm.PayCurrency + row.sumPayCurr;
                            if (!row.IssumPayRubNull())
                            {
                                itemTotalSumm.PayRub = itemTotalSumm.PayRub + row.sumPayRub;
                                totalSumPayRub = totalSumPayRub + row.sumPayRub;
                            }
                        }
                    }
                }
            }
            else
            {
                DataRow[] rows = thisDS.tableExpenditure.Select(string.Empty, string.Empty, DataViewRowState.CurrentRows);
                foreach (ExpenditureDS.tableExpenditureRow row in rows)
                {
                    itemTotalSumm = list.Find(x => x.Currency == row.currency);
                    if (itemTotalSumm == null)
                    {
                        itemTotalSumm = new ExpendetureTotalSumm(row.currency);
                        list.Add(itemTotalSumm);
                    }
                    if (!row.IssumExNull()) itemTotalSumm.Expenditure = itemTotalSumm.Expenditure + row.sumEx;
                    if (!row.IssumPayCurrNull()) itemTotalSumm.PayCurrency = itemTotalSumm.PayCurrency + row.sumPayCurr;
                    if (!row.IssumPayRubNull())
                    {
                        itemTotalSumm.PayRub = itemTotalSumm.PayRub + row.sumPayRub;
                        totalSumPayRub = totalSumPayRub + row.sumPayRub;
                    }
                }
            }
            list.Sort(delegate(ExpendetureTotalSumm x, ExpendetureTotalSumm y) { return x.Currency.CompareTo(y.Currency); });
            totalDataGrid.ItemsSource = list;
            totalSumPayRubTextBox.Text = totalSumPayRub.ToString("N");
        }
        private void mainDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string col = e.Column.Header != null ? e.Column.Header.ToString() : string.Empty;
            if (col == "Сумма затраты" | col == "Оплата, вал" | col == "Оплата, руб")
            {
                decimal.TryParse((e.Column.GetCellContent(e.Row) as TextBlock).Text, out totalOldValue);
            }
            else if (col == "Валюта*") totalOldCurrency = (e.Column.GetCellContent(e.Row) as ComboBox).Text;
        }
        private void mainDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            decimal newvalue = 0;
            ExpendetureTotalSumm itemTotalSumm;
            ListNotifyChanged<ExpendetureTotalSumm> list = totalDataGrid.ItemsSource as ListNotifyChanged<ExpendetureTotalSumm>;
            ExpenditureDS.tableExpenditureRow row = (e.Row.Item as DataRowView).Row as ExpenditureDS.tableExpenditureRow;
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                switch (e.Column.Header != null ? e.Column.Header.ToString() : string.Empty)
                {
                    case "Сумма затраты":
                        if (!row.IssumExNull()) newvalue = row.sumEx; else newvalue = 0;
                        itemTotalSumm = list.Find(x => x.Currency == row.currency);
                        if (itemTotalSumm == null) // если новая запись без валюты
                        {
                            itemTotalSumm = new ExpendetureTotalSumm(row.currency);
                            list.Add(itemTotalSumm);
                        }
                        itemTotalSumm.Expenditure = itemTotalSumm.Expenditure - totalOldValue + newvalue;
                        break;
                    case "Оплата, вал":
                        if (!row.IssumPayCurrNull()) newvalue = row.sumPayCurr; else newvalue = 0;
                        itemTotalSumm = list.Find(x => x.Currency == row.currency);
                        if (itemTotalSumm == null)
                        {
                            itemTotalSumm = new ExpendetureTotalSumm(row.currency);
                            list.Add(itemTotalSumm);
                        }
                        itemTotalSumm.PayCurrency = itemTotalSumm.PayCurrency - totalOldValue + newvalue;
                        break;
                    case "Оплата, руб":
                        if (!row.IssumPayRubNull()) newvalue = row.sumPayRub; else newvalue = 0;
                        itemTotalSumm = list.Find(x => x.Currency == row.currency);
                        if (itemTotalSumm == null)
                        {
                            itemTotalSumm = new ExpendetureTotalSumm(row.currency);
                            list.Add(itemTotalSumm);
                        }
                        itemTotalSumm.PayRub = itemTotalSumm.PayRub - totalOldValue + newvalue;
                        totalSumPayRubTextBox.Text = (decimal.Parse(totalSumPayRubTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Валюта*":
                        if (row.currency != totalOldCurrency)
                        {
                            itemTotalSumm = list.Find(x => x.Currency == row.currency);
                            if (itemTotalSumm == null)
                            {
                                itemTotalSumm = new ExpendetureTotalSumm(row.currency);
                                list.Add(itemTotalSumm);
                                list.Sort(delegate(ExpendetureTotalSumm x, ExpendetureTotalSumm y) { return x.Currency.CompareTo(y.Currency); });
                                list.OnResetCollectionChanged();
                            }
                            if (!row.IssumExNull()) itemTotalSumm.Expenditure = itemTotalSumm.Expenditure + row.sumEx;
                            if (!row.IssumPayCurrNull()) itemTotalSumm.PayCurrency = itemTotalSumm.PayCurrency + row.sumPayCurr;
                            if (!row.IssumPayRubNull()) itemTotalSumm.PayRub = itemTotalSumm.PayRub + row.sumPayRub;

                            itemTotalSumm = list.Find(x => x.Currency == totalOldCurrency);
                            if (!row.IssumExNull()) itemTotalSumm.Expenditure = itemTotalSumm.Expenditure - row.sumEx;
                            if (!row.IssumPayCurrNull()) itemTotalSumm.PayCurrency = itemTotalSumm.PayCurrency - row.sumPayCurr;
                            if (!row.IssumPayRubNull()) itemTotalSumm.PayRub = itemTotalSumm.PayRub - row.sumPayRub;
                            if (itemTotalSumm.Expenditure + itemTotalSumm.PayCurrency + itemTotalSumm.PayRub == 0M)
                            {
                                list.Remove(itemTotalSumm);
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (e.Column.Header != null ? e.Column.Header.ToString() : string.Empty)
                {
                    case "Сумма затраты":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                        {
                            itemTotalSumm = list.Find(x => x.Currency == row.currency);
                            if (itemTotalSumm == null) // если новая запись без валюты
                            {
                                itemTotalSumm = new ExpendetureTotalSumm(row.currency);
                                list.Add(itemTotalSumm);
                            }
                            itemTotalSumm.Expenditure = itemTotalSumm.Expenditure - totalOldValue + newvalue;
                        }
                        break;
                    case "Оплата, вал":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                        {
                            itemTotalSumm = list.Find(x => x.Currency == row.currency);
                            if (itemTotalSumm == null)
                            {
                                itemTotalSumm = new ExpendetureTotalSumm(row.currency);
                                list.Add(itemTotalSumm);
                            }
                            itemTotalSumm.PayCurrency = itemTotalSumm.PayCurrency - totalOldValue + newvalue;
                        }
                        break;
                    case "Оплата, руб":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                        {
                            itemTotalSumm = list.Find(x => x.Currency == row.currency);
                            if (itemTotalSumm == null)
                            {
                                itemTotalSumm = new ExpendetureTotalSumm(row.currency);
                                list.Add(itemTotalSumm);
                            }
                            itemTotalSumm.PayRub = itemTotalSumm.PayRub - totalOldValue + newvalue;
                            totalSumPayRubTextBox.Text = (decimal.Parse(totalSumPayRubTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        }
                        break;
                    case "Валюта*":
                        string newcurrency = (e.EditingElement as ComboBox).Text;
                        if (newcurrency != totalOldCurrency)
                        {
                            decimal sumEx=0M, sumPayCurr=0M, sumPayRub=0M;
                            foreach (DataGridColumn column in mainDataGrid.Columns)
                            {
                                switch (column.Header != null ? column.Header.ToString() : string.Empty)
                                {
                                    case "Сумма затраты":
                                        decimal.TryParse((column.GetCellContent(e.Row) as TextBlock).Text, out sumEx);
                                        break;
                                    case "Оплата, вал":
                                        decimal.TryParse((column.GetCellContent(e.Row) as TextBlock).Text, out sumPayCurr);
                                        break;
                                    case "Оплата, руб":
                                        decimal.TryParse((column.GetCellContent(e.Row) as TextBlock).Text, out sumPayRub);
                                        break;
                                }
                            }
                            itemTotalSumm = list.Find(x => x.Currency == newcurrency);
                            if (itemTotalSumm == null)
                            {
                                itemTotalSumm = new ExpendetureTotalSumm(newcurrency);
                                list.Add(itemTotalSumm);
                                list.Sort(delegate(ExpendetureTotalSumm x, ExpendetureTotalSumm y) { return x.Currency.CompareTo(y.Currency); });
                                list.OnResetCollectionChanged();
                            }
                            if (!row.IssumExNull()) itemTotalSumm.Expenditure = itemTotalSumm.Expenditure + sumEx;
                            if (!row.IssumPayCurrNull()) itemTotalSumm.PayCurrency = itemTotalSumm.PayCurrency + sumPayCurr;
                            if (!row.IssumPayRubNull()) itemTotalSumm.PayRub = itemTotalSumm.PayRub + sumPayRub;

                            itemTotalSumm = list.Find(x => x.Currency == totalOldCurrency);
                            if (itemTotalSumm != null)
                            {
                                if (!row.IssumExNull()) itemTotalSumm.Expenditure = itemTotalSumm.Expenditure - sumEx;
                                if (!row.IssumPayCurrNull()) itemTotalSumm.PayCurrency = itemTotalSumm.PayCurrency - sumPayCurr;
                                if (!row.IssumPayRubNull()) itemTotalSumm.PayRub = itemTotalSumm.PayRub - sumPayRub;
                                if (itemTotalSumm.Expenditure + itemTotalSumm.PayCurrency + itemTotalSumm.PayRub == 0M)
                                {
                                    list.Remove(itemTotalSumm);
                                }
                            }
                        }
                        break;
                }
            }
        }
        private void mainDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                totalDataRefresh();
            }
        }
        private void mainDataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if ((e.Row.Item is DataRowView) && ((e.Row.Item as DataRowView).Row.RowState == DataRowState.Detached | (e.Row.Item as DataRowView).Row.RowState == DataRowState.Deleted))
            {
                totalDataRefresh();
            }
        }
        private void mainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == mainDataGrid)
                totalDataRefresh();
        }
        #endregion

    }

    public class ExpendetureOperType
    {
        int _id;
        string _name;
        public int Id { set { _id = value; } get { return _id; } }
        public string Name { set { _name = value; } get { return _name; } }
        public ExpendetureOperType(int id, string name)
        {
            _id = id;
            _name = name;
        }
    }
    public class ExpendetureOperTypeList : System.Collections.Generic.List<ExpendetureOperType>
    {
        public ExpendetureOperTypeList()
            : base()
        {
            this.Add(new ExpendetureOperType(0, "Платеж"));
            this.Add(new ExpendetureOperType(1, "Зачет"));
        }

    }
    public class ExpendetureTotalSumm : System.ComponentModel.INotifyPropertyChanged
    {
        string _currency;
        decimal _sumexp, _paycurr, _payrub;
        public string Currency
        {
            set
            {
                _currency = value;
                PropertyChangedNotification("Currency");
            }
            get { return _currency; }
        }
        public decimal Expenditure
        {
            set
            {
                _sumexp = value;
                PropertyChangedNotification("Expenditure");
                PropertyChangedNotification("DebtCurrency");
            }
            get { return _sumexp; }
        }
        public decimal PayCurrency
        {
            set
            {
                _paycurr = value;
                PropertyChangedNotification("PayCurrency");
                PropertyChangedNotification("DebtCurrency");
            }
            get { return _paycurr; }
        }
        public decimal DebtCurrency { get { return this.Expenditure - this.PayCurrency; } }
        public decimal PayRub
        {
            set
            {
                _payrub = value;
                PropertyChangedNotification("PayRub");
            }
            get { return _payrub; }
        }
        public ExpendetureTotalSumm(string currency, decimal expenditure, decimal payCurrency, decimal payRub)
        {
            this.Currency = currency;
            this.Expenditure = expenditure;
            this.PayCurrency = payCurrency;
            this.PayRub = payRub;
        }
        public ExpendetureTotalSumm(string currency) : this(currency, 0M, 0M, 0M) { }
        public ExpendetureTotalSumm() : this(string.Empty) { }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
