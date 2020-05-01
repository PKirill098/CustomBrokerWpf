using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для WithdrawalWin.xaml
    /// </summary>
    public partial class WithdrawalWin : Window
    {
        private DataRowView thisDataRowView;
        internal DataRowView WithdrawalRowView
        {
            set
            {
                this.DataContext = value;
                if (value is DataRowView)
                {
                    thisDataRowView = value;
                    tableWithdrawal = thisDataRowView.Row.Table as WithdrawalDS.tableWithdrawalDataTable;
                }
            }
            get { return this.thisDataRowView; }
        }
        internal WithdrawalDS.tableWithdrawalRow WithdrawalRow
        {
            get
            {
                return this.thisDataRowView.Row as WithdrawalDS.tableWithdrawalRow;
            }
        }
        private WithdrawalDS.tableWithdrawalDataTable tableWithdrawal;
        private WithdrawalDS.tableExpenditureDataTable tableExpenditure;
        private WithdrawalDSTableAdapters.ExpenditureAdapter adapterExpenditure;
        
        public WithdrawalWin()
        {
            InitializeComponent();
            tableExpenditure = new WithdrawalDS.tableExpenditureDataTable();
            adapterExpenditure = new WithdrawalDSTableAdapters.ExpenditureAdapter(); 
        }
        private void winWithdrawal_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refds = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refds.tableLegalEntity.Count == 0) refds.LegalEntityRefresh();
            if (refds.tableAccountCurrency.Count == 0) refds.AccountCurrencyRefresh();
            legalComboBox.ItemsSource = new DataView(refds.tableLegalEntity, "accountid<>0", "namelegal", DataViewRowState.CurrentRows);
            System.Windows.Data.CollectionViewSource recipientCollectionView = new System.Windows.Data.CollectionViewSource();
            recipientCollectionView.Source=References.Contractors;
            recipientCollectionView.View.Filter = delegate (object item) { return (item as Domain.References.Contractor).Name.Length > 0; };
            recipientComboBox.ItemsSource = recipientCollectionView.View;
            currencyComboBox.ItemsSource = refds.tableAccountCurrency.DefaultView;
            DataLoad();
            this.Title = this.Title + " - " + WithdrawalRow.withdrawalID.ToString();
        }
        private void winWithdrawal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

        }
        private void DCJoinAllButton_Click(object sender, RoutedEventArgs e)
        {

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
                IInputElement focelm = FocusManager.GetFocusedElement(this);
                FocusManager.SetFocusedElement(this, sender as Button);

                Reject();
                totalDataRefresh();
                FocusManager.SetFocusedElement(this, focelm);
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
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалть все сведения о платеже?", "Платеж", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                this.DataContext = null;
                thisDataRowView.Delete();
                foreach (DataRow row in tableExpenditure.Rows) row.Delete();
            }
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
            if (!(this.expenditureDataGrid.CurrentItem is DataRowView)) return;
            this.expenditureDataGrid.CommitEdit(DataGridEditingUnit.Cell, false);
            if (this.expenditureDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
            {
                ExpenditureDS.tableExpenditureRow row = (this.expenditureDataGrid.CurrentItem as DataRowView).Row as ExpenditureDS.tableExpenditureRow;
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
                    win.ExpenditureRowView = this.expenditureDataGrid.CurrentItem as DataRowView;
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

        private void ComboBox_Loaded(object sender, RoutedEventArgs e) //Bug ComboBoxItem
        { (sender as ComboBox).IsDropDownOpen = true; (sender as ComboBox).IsDropDownOpen = false; }

        private void DataLoad()
        {
            this.expenditureDataGrid.ItemsSource = null;
            adapterExpenditure.Fill(tableExpenditure, WithdrawalRow.withdrawalID);
            this.expenditureDataGrid.ItemsSource = tableExpenditure;
            totalDataRefresh();
        }
        private bool SaveChanges()
        {
            bool isSuccess = false, isNew;
            isNew = WithdrawalRow.RowState == DataRowState.Added | WithdrawalRowView.IsNew;
            IInputElement focelm = FocusManager.GetFocusedElement(this);
            FocusManager.SetFocusedElement(this, SaveButton);
            if ((focelm is DependencyObject) && Validation.GetHasError(focelm as DependencyObject)) return false;
            this.expenditureDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            this.expenditureDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            
            try
            {
                if (this.DataContext is DataRowView)
                {
                    DataRowView rowview = this.DataContext as DataRowView;
                    rowview.EndEdit();
                }
                if (WithdrawalRow.HasErrors | tableExpenditure.HasErrors)
                {
                    string err=string.Empty;
                    //err = WithdrawalRow.RowError;
                    if (WithdrawalRow.GetColumnsInError().Length > 0)
                        err = WithdrawalRow.GetColumnError(WithdrawalRow.GetColumnsInError()[0]);
                    if (err.Length == 0 & tableExpenditure.GetErrors().Length > 0)
                        if (tableExpenditure.GetErrors()[0].GetColumnsInError().Length > 0)
                            err = tableExpenditure.GetErrors()[0].GetColumnError(tableExpenditure.GetErrors()[0].GetColumnsInError()[0]);
                        //else
                        //    err = tableExpenditure.GetErrors()[0].RowError;
                    if (err.Length > 0)
                    {
                        MessageBox.Show(err, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                WithdrawalDSTableAdapters.WithdrawalAdapter adapterW = new WithdrawalDSTableAdapters.WithdrawalAdapter();
                adapterW.Update(tableWithdrawal);
                if(isNew) this.Title=this.Title.Substring(0,this.Title.IndexOf("- ")+2) + WithdrawalRow.withdrawalID.ToString();
                WithdrawalDSTableAdapters.ExpenditureAdapter adapterE = new WithdrawalDSTableAdapters.ExpenditureAdapter();
                if (WithdrawalRow.RowState != DataRowState.Detached)
                {
                    adapterE.Adapter.UpdateCommand.Parameters["@WithdrawalID"].Value = WithdrawalRow.withdrawalID;
                    adapterE.Adapter.DeleteCommand.Parameters["@WithdrawalID"].Value = WithdrawalRow.withdrawalID;
                }
                else
                {
                    adapterE.Adapter.DeleteCommand.Parameters["@WithdrawalID"].Value = 0;
                    adapterE.Adapter.DeleteCommand.Parameters["@sumCurrPay"].Value = 0M;
                    adapterE.Adapter.DeleteCommand.Parameters["@sumRubPay"].Value = 0M;
                }
                DataRow[] rows = tableExpenditure.Select("sumCurrPay>0 OR sumRubPay>0", "", DataViewRowState.ModifiedCurrent);
                adapterE.Update(tableExpenditure);
                isSuccess = true;
                if(this.Owner is ExpenditureListWin | this.Owner is ExpenditureWin)
                {
                    ExpenditureDS exds;
                    if (this.Owner is ExpenditureListWin) exds = (this.Owner as ExpenditureListWin).thisDS; else exds = (this.Owner.Owner as ExpenditureListWin).thisDS;
                    foreach(DataRow row in rows)
                    {
                        WithdrawalDS.tableExpenditureRow wexrow=row as WithdrawalDS.tableExpenditureRow;
                        ExpenditureDS.tableExpenditureRow exrow= exds.tableExpenditure.FindByExpenditureID(wexrow.ExpenditureID);
                        if (exrow != null)
                        {
                            if (exrow.IspayIDNull()) exrow.payID = this.WithdrawalRow.withdrawalID;
                            exrow.sumPayCurr = wexrow.sumCurrPay;
                            exrow.sumPayRub = wexrow.sumRubPay;
                            exrow.EndEdit();
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
                        //foreach(DataRowView row in expenditureDataGrid.Items)
                        //{
                        //expenditureDataGrid.BindingGroup.BindingExpressions[0].ValidateWithoutUpdate();
                        //}
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
        internal void Reject()
        {
            tableExpenditure.RejectChanges();
            tableWithdrawal.RejectChanges();
            if (tableWithdrawal.Count > 0)
                this.DataContext = thisDataRowView;
            else
                this.DataContext = null;
        }

        #region Total Sum
        decimal totalOldValue;
        private void totalDataRefresh()
        {
            decimal totalNoJoinSum = 0;
            DataRow[] rows = tableExpenditure.Select(string.Empty, string.Empty, DataViewRowState.CurrentRows);
            foreach (WithdrawalDS.tableExpenditureRow row in rows)
            {
                if (!row.IssumRubPayNull()) totalNoJoinSum = totalNoJoinSum + row.sumRubPay;
                //else if (!row.IssumCurrPayNull() && row.sumCurrPay!=0M) totalNoJoinSum = totalNoJoinSum + row.sumCurrPay;
            }
            //noJoinSumTextBox.Text = (WithdrawalRow.IscsumNull()?0M:WithdrawalRow.csum - totalNoJoinSum).ToString("N2");
        }
        private void mainDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string col = e.Column.Header != null ? e.Column.Header.ToString() : string.Empty;
            if (col == "Оплата, руб")
            {
                decimal.TryParse((e.Column.GetCellContent(e.Row) as TextBlock).Text, out totalOldValue);
            }
        }
        private void mainDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            decimal newvalue = 0M;

            string col = e.Column.Header != null ? e.Column.Header.ToString() : string.Empty;
            WithdrawalDS.tableExpenditureRow row = (e.Row.Item as DataRowView).Row as WithdrawalDS.tableExpenditureRow;
            if (col == "Оплата, руб")
            {
                if (e.EditAction == DataGridEditAction.Cancel)
                {
                    if (!row.IssumRubPayNull()) newvalue = row.sumRubPay; else newvalue = 0M;
                     WithdrawalRow.joinsum=WithdrawalRow.joinsum + newvalue - totalOldValue;
                    //noJoinSumTextBox.Text = (decimal.Parse(noJoinSumTextBox.Text) + totalOldValue - newvalue).ToString("N");
                }
                else
                {
                    if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                        WithdrawalRow.joinsum = WithdrawalRow.joinsum + newvalue - totalOldValue;
                        //noJoinSumTextBox.Text = (decimal.Parse(noJoinSumTextBox.Text) + totalOldValue - newvalue).ToString("N");
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
            if (e.OriginalSource == expenditureDataGrid) totalDataRefresh();
        }
        #endregion
    }
}
