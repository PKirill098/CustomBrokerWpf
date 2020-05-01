using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для WithdrawalListWin.xaml
    /// </summary>
    public partial class WithdrawalListWin : Window
    {
        internal WithdrawalDS thisDS;
        public WithdrawalListWin()
        {
            InitializeComponent();
            thisDS = new WithdrawalDS();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refds = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refds.tableLegalEntity.Count == 0) refds.LegalEntityRefresh();
            if (refds.tableAccountCurrency.Count == 0) refds.AccountCurrencyRefresh();
            (mainDataGrid.FindResource("keyLegalEntityVS") as CollectionViewSource).Source = new DataView(refds.tableLegalEntity, "accountid<>0", "namelegal", DataViewRowState.CurrentRows);
            CollectionViewSource recipientCollectionView = mainDataGrid.FindResource("keyRecipientListVS") as CollectionViewSource;
            recipientCollectionView.Source = References.Contractors;
            recipientCollectionView.View.Filter = delegate (object item) { return (item as Domain.References.Contractor).Name.Length > 0; };
            (mainDataGrid.FindResource("keyCurrecyVS") as CollectionViewSource).Source = refds.tableAccountCurrency.DefaultView;
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
                if (item.Name == "winWithdrawalFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    //ObjectWin = new WithdrawalFilterWin();
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
                    RejectRow((this.mainDataGrid.SelectedItem as DataRowView).Row as WithdrawalDS.tableWithdrawalRow);
                }
                else
                {
                    for (int i = 0; i < this.thisDS.tableWithdrawal.Rows.Count; i++)
                    {
                        RejectRow(this.thisDS.tableWithdrawal.Rows[i] as WithdrawalDS.tableWithdrawalRow);
                    }
                    this.thisDS.tableWithdrawal.RejectChanges();
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
                WithdrawalWin win = new WithdrawalWin();
                win.WithdrawalRowView = thisDS.tableWithdrawal.DefaultView.AddNew();
                win.Owner = this;
                win.Show();
                this.mainDataGrid.CurrentItem = win.WithdrawalRowView;
                if (!this.mainDataGrid.IsFocused) this.mainDataGrid.Focus();
            }
            else MessageBox.Show("Ошибка в текущей строке, исправте!", "Добавление", MessageBoxButton.OK, MessageBoxImage.Stop);
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить все сведения о платеже?", "Затраты", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes && CloseChildren()) (this.mainDataGrid.CurrentItem as DataRowView).Delete();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.mainDataGrid.CurrentItem is DataRowView)) return;
            this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, false);
            if (this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
            {
                WithdrawalDS.tableWithdrawalRow row = (this.mainDataGrid.CurrentItem as DataRowView).Row as WithdrawalDS.tableWithdrawalRow;
                WithdrawalWin win = null;
                foreach (Window owwin in this.OwnedWindows)
                {
                    if (owwin.Name == "winWithdrawal")
                    {
                        if ((owwin as WithdrawalWin).WithdrawalRow.withdrawalID == row.withdrawalID)
                        {
                            win = owwin as WithdrawalWin;
                            break;
                        }
                    }
                }
                if (win == null)
                {
                    win = new WithdrawalWin();
                    win.WithdrawalRowView = this.mainDataGrid.CurrentItem as DataRowView;
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

        private void DataLoad()
        {
            if (!CloseChildren()) return;

            this.mainDataGrid.ItemsSource = null;
            References.Contractors.Refresh();
            WithdrawalDSTableAdapters.WithdrawalAdapter adapter = new WithdrawalDSTableAdapters.WithdrawalAdapter();
            adapter.Fill(thisDS.tableWithdrawal, thisfilter.FilterWhereId, null);
            this.mainDataGrid.ItemsSource = thisDS.tableWithdrawal.DefaultView;
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
                    WithdrawalDSTableAdapters.WithdrawalAdapter adapter = new WithdrawalDSTableAdapters.WithdrawalAdapter();
                    adapter.Update(thisDS.tableWithdrawal);
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
            }
            return isSuccess;
        }
        private bool CloseChildren()
        {
            foreach (Window owwin in this.OwnedWindows)
            {
                if (owwin.Name == "winWithdrawal") owwin.Close();
            }
            bool isSuccess = true;
            foreach (Window owwin in this.OwnedWindows)
            {
                if (owwin.Name == "winWithdrawal") isSuccess = false;
            }
            return isSuccess;
        }
        private void RejectRow(WithdrawalDS.tableWithdrawalRow row)
        {
            bool isreject = false;
            foreach (Window owwin in this.OwnedWindows)
            {
                if (owwin.Name == "winWithdrawal")
                {
                    WithdrawalWin exwin = owwin as WithdrawalWin;
                    if (exwin.WithdrawalRow.withdrawalID == row.withdrawalID)
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
        private CustomBrokerWpf.SQLFilter thisfilter = new SQLFilter("withdrawal", "AND");
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

        #region Total Sum
        decimal totalOldValue;
        private void totalDataRefresh()
        {
            decimal totalSum = 0, totalJoin = 0;
            if (this.mainDataGrid.SelectedItems.Count > 1)
            {
                for (int i = 0; i < this.mainDataGrid.SelectedItems.Count; i++)
                {
                    if (this.mainDataGrid.SelectedItems[i] is DataRowView)
                    {
                        WithdrawalDS.tableWithdrawalRow row = (this.mainDataGrid.SelectedItems[i] as DataRowView).Row as WithdrawalDS.tableWithdrawalRow;
                        if (row.RowState != DataRowState.Deleted)
                        {
                            if (!row.IscsumNull()) totalSum = totalSum + row.csum;
                            if (!row.IsnojoinsumNull()) totalJoin = totalJoin + row.joinsum;
                        }
                    }
                }
            }
            else
            {
                DataRow[] rows = thisDS.tableWithdrawal.Select(string.Empty, string.Empty, DataViewRowState.CurrentRows);
                foreach (WithdrawalDS.tableWithdrawalRow row in rows)
                {
                    if (!row.IscsumNull()) totalSum = totalSum + row.csum;
                    if (!row.IsnojoinsumNull()) totalJoin = totalJoin + row.joinsum;
                }
            }
            totalSumTextBox.Text = totalSum.ToString("N2");
            totalJoinTextBox.Text = totalJoin.ToString("N2");
            totalNoJoinTextBox.Text = (totalSum - totalJoin).ToString("N2");
        }
        private void mainDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string col = e.Column.Header != null ? e.Column.Header.ToString() : string.Empty;
            if (col == "Сумма платежа")
            {
                decimal.TryParse((e.Column.GetCellContent(e.Row) as TextBlock).Text, out totalOldValue);
            }
        }
        private void mainDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            decimal newvalue = 0;

            if (e.EditAction == DataGridEditAction.Cancel)
            {
                WithdrawalDS.tableWithdrawalRow row = (e.Row.Item as DataRowView).Row as WithdrawalDS.tableWithdrawalRow;
                switch (e.Column.Header != null ? e.Column.Header.ToString() : string.Empty)
                {
                    case "Сумма платежа":
                        if (!row.IscsumNull()) newvalue = row.csum; else newvalue = 0M;
                        totalSumTextBox.Text = (decimal.Parse(totalSumTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        totalNoJoinTextBox.Text = (decimal.Parse(totalNoJoinTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                }
            }
            else
            {
                switch (e.Column.Header != null ? e.Column.Header.ToString() : string.Empty)
                {
                    case "Сумма платежа":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                        {
                            totalSumTextBox.Text = (decimal.Parse(totalSumTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            totalNoJoinTextBox.Text = (decimal.Parse(totalNoJoinTextBox.Text) - totalOldValue + newvalue).ToString("N");
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
            if (e.OriginalSource == mainDataGrid) totalDataRefresh();
        }
        #endregion

        private void mainDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.SortMemberPath == "contractor")
            {
               
            }
        }
    }
}
