using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ExpenditureWin.xaml
    /// </summary>
    public partial class ExpenditureWin : Window, INotifyPropertyChanged
    {
        bool _isTypeChanged = false;
        DataRowView thisDataRowView;
        private ExpenditureDS.tableExpenditureDetailDataTable tableDetail;
        private ExpenditureDSTableAdapters.ExpenditureDetailAdapter detailAdapter;
        private ExpenditureDS.tableExpenditureWithdrawalDataTable tableWithdrawal;
        //private ExpenditureDSTableAdapters.ExpenditureWithdrawalAdapter withdrawalAdapter;
        //private ExpenditureDS.tableRecipientListDataTable tableRecipientList;
        internal DataRowView ExpenditureRowView
        {
            set
            {
                this.mainGrid.DataContext = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsEditable"));
                }

                if (value is DataRowView)
                {
                    thisDataRowView = value;
                    //tableDetail.Columns["ExpenditureID"].DefaultValue = ((this.mainGrid.DataContext as DataRowView).Row as ExpenditureDS.tableExpenditureRow).ExpenditureID;
                }
            }
            get { return this.thisDataRowView; }
        }
        internal ExpenditureDS.tableExpenditureRow ExpenditureRow
        {
            get
            {
                return this.thisDataRowView.Row as ExpenditureDS.tableExpenditureRow;
            }
        }
        public bool IsEditable { get { return this.mainGrid.DataContext is DataRowView; } }

        public ExpenditureWin()
        {
            InitializeComponent();
            tableDetail = new ExpenditureDS.tableExpenditureDetailDataTable();
            detailAdapter = new ExpenditureDSTableAdapters.ExpenditureDetailAdapter();
            detailAdapter.Adapter.InsertCommand.Parameters["@ExpenditureID"].SourceColumn = string.Empty;
            tableWithdrawal = new ExpenditureDS.tableExpenditureWithdrawalDataTable();
            //withdrawalAdapter = new ExpenditureDSTableAdapters.ExpenditureWithdrawalAdapter();
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
            typeComboBox.ItemsSource = refds.tableExpenditureType.DefaultView;
            itemComboBox.ItemsSource = refds.tableExpenditureItem.DefaultView;
            currencyComboBox.ItemsSource = refds.tableAccountCurrency.DefaultView;
            parcelComboBox.ItemsSource = refds.tableFullNumber.DefaultView;//new System.Data.DataView(refds.tableFullNumber, "status<300", "sort DESC", System.Data.DataViewRowState.CurrentRows);
            legalComboBox.ItemsSource = refds.tableLegalEntity.DefaultView;
            customerComboBox.ItemsSource = refds.tableCustomerName.DefaultView;
            if(!ExpenditureRowView.IsNew) mainGrid.BindingGroup.CancelEdit(); // customerComboBox устанавливает Dirty=true
            (this.withdrawalDataGrid.FindResource("keyLegalEntityVS") as CollectionViewSource).Source = refds.tableLegalEntity.DefaultView;

            //tableRecipientList = (ExpenditureRow.Table.DataSet as ExpenditureDS).tableRecipientList;
            //tableRecipientList.DefaultView.Sort = "recipient";
            recipientComboBox.ItemsSource = References.Contractors;

            DataLoad();
            if (tableDetail.Count == 0 & typeComboBox.SelectedIndex > -1) FillDetails((int)typeComboBox.SelectedValue);
            (this.FindResource("keyExpenditureDetailVS") as CollectionViewSource).Source = tableDetail;

            this.Title = this.Title + " - " + ExpenditureRow.ExpenditureID.ToString();
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
                popInf.IsOpen = true;
            }
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалть все сведения о затрате?", "Затраты", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                this.Delete();
            }

        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void WithdrawalButton_Click(object sender, RoutedEventArgs e)
        {
            ExpenditureDS.tableExpenditureRow row = this.ExpenditureRow;
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

        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbsender = sender as ComboBox;
            if (cbsender.IsKeyboardFocused & (cbsender.SelectedIndex != -1)) _isTypeChanged = true;
            else if (cbsender.IsKeyboardFocusWithin & (cbsender.SelectedIndex != -1)) FillDetails((int)cbsender.SelectedValue);
        }
        private void typeComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_isTypeChanged)
            {
                FillDetails((int)typeComboBox.SelectedValue);
                _isTypeChanged = false;
            }
        }
        private void parcelComboBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (parcelComboBox.SelectedIndex < 0) parcelComboBox.Text = string.Empty;
        }
        private void withdrawalDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                string whatdo = string.Empty;
                switch ((e.Error.BindingInError as BindingExpression).ResolvedSourcePropertyName)
                {
                    case "dateW":
                        whatdo = "\nВведите корректное значение даты.";
                        break;
                }
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString() + whatdo, "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        private void DataLoad()
        {
            detailAdapter.Fill(tableDetail, ExpenditureRow.ExpenditureID);
            detailDataGrid.ItemsSource = tableDetail.DefaultView;
            DetailSumRefresh();
            //withdrawalAdapter.Fill(tableWithdrawal, ExpenditureRow.ExpenditureID);
            //withdrawalDataGrid.ItemsSource = tableWithdrawal.DefaultView;
        }
        private bool SaveChanges()
        {
            bool isSuccess = false, isNew=false;
            IInputElement focelm = FocusManager.GetFocusedElement(this);
            FocusManager.SetFocusedElement(this, SaveButton);
            try
            {
                if (this.ExpenditureRow.RowState != DataRowState.Deleted)
                {
                    if (this.detailDataGrid.CommitEdit(DataGridEditingUnit.Cell, true) &
                        this.detailDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
                    {
                        decimal sum = 0M;
                        DataRow[] rows = tableDetail.Select(string.Empty, string.Empty, DataViewRowState.CurrentRows);
                        System.Data.DataRowState ExState = ExpenditureRow.RowState;
                        if (ExpenditureRow.countDet != rows.Length) ExpenditureRow.countDet = rows.Length;
                        decimal.TryParse(ExSumTextBox.Text, out sum);
                        if (!ExpenditureRow.sumEx.Equals(sum))ExpenditureRow.sumEx = sum;
                        ExpenditureRow.EndEdit();
                        if (ExState == System.Data.DataRowState.Unchanged) ExpenditureRow.AcceptChanges();
                        if (mainGrid.BindingGroup.IsDirty | mainGrid.BindingGroup.HasValidationError) mainGrid.BindingGroup.CommitEdit(); //были изменения в сонтролах или ранее была ошибка
                        if(mainGrid.BindingGroup.HasValidationError) // ошибка исправлена?
                        {
                            MessageBox.Show(mainGrid.BindingGroup.ValidationErrors[0].ErrorContent.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                    else
                    {
                        bool handled = false;
                        DataGridRow item;
                        for (int i = 0; i < this.detailDataGrid.Items.Count; i++)
                        {
                            item = (DataGridRow)this.detailDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                            if ((item is DataGridRow) && Validation.GetHasError(item))
                            {
                                string errmsg,errcnt;
                                this.detailDataGrid.ScrollIntoView(item.Item);
                                this.detailDataGrid.SelectedItems.Add(item.Item);
                                errcnt = Validation.GetErrors(item)[0].Exception.Message;
                                if (errcnt.IndexOf("nameExD") > 0) errmsg = "Отсутствует наименование детали затраты.";
                                else errmsg = errcnt;
                                MessageBox.Show(errmsg, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                                handled = true;
                                break;
                            }
                        }
                        if (!handled)
                        {
                            DataRow row;
                            if (tableDetail.HasErrors) row = tableDetail.GetErrors()[0] as DataRow;
                            else row = (tableDetail.DefaultView[tableDetail.DefaultView.Count - 1]).Row;
                            if (row.GetColumnsInError().Length > 0)
                                MessageBox.Show(row.GetColumnError(row.GetColumnsInError()[0]), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        return false;
                    }
                }
                ExpenditureRowView.EndEdit();
                CustomBrokerWpf.ExpenditureDSTableAdapters.ExpenditureAdapter exadapter = new ExpenditureDSTableAdapters.ExpenditureAdapter();
                exadapter.Adapter.InsertCommand.Parameters["@countDet"].SourceColumn = string.Empty;
                exadapter.Adapter.InsertCommand.Parameters["@countDet"].Value = 2;
                exadapter.Adapter.UpdateCommand.Parameters["@countDet"].SourceColumn = string.Empty;
                exadapter.Adapter.UpdateCommand.Parameters["@countDet"].Value = 2;
                if (this.ExpenditureRow.RowState == DataRowState.Added)
                {
                    isNew = true;
                    (CollectionViewSource.GetDefaultView((this.Owner as ExpenditureListWin).mainDataGrid.ItemsSource) as BindingListCollectionView).Refresh();
                }
                exadapter.Update(this.ExpenditureRow);
                if (isNew) this.Title = this.Title.Substring(0, this.Title.IndexOf("- ") + 2) + ExpenditureRow.ExpenditureID.ToString();
                if (this.ExpenditureRow.RowState != DataRowState.Detached)
                {
                    detailAdapter.Adapter.InsertCommand.Parameters["@ExpenditureID"].Value = this.ExpenditureRow.ExpenditureID;
                    detailAdapter.Update(tableDetail);
                    //this.withdrawalDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                    //this.withdrawalDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                    //withdrawalAdapter.Adapter.InsertCommand.Parameters["@ExpenditureID"].Value = this.ExpenditureRow.ExpenditureID;
                    //withdrawalAdapter.Update(tableWithdrawal);
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
                    MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля или удалите затрату.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            FocusManager.SetFocusedElement(this, focelm);
            return isSuccess;
        }
        private void FillDetails(int dtype)
        {
            if (tableDetail.Count != 0 && MessageBox.Show("Затрата уже имеет деталировки!/n/nДобавить деталировки согласно выбранному типу?", "Добавление деталировок", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT [NameEDNL] FROM [account].[ExpenditureDetailNameList_tb] WHERE ExpenditureTypeID=" + dtype.ToString();
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tableDetail.AddtableExpenditureDetailRow(this.ExpenditureRow, reader.GetString(0), 0M, "", DateTime.Now, 0);
            }
        }

        internal void Reject()
        {
            detailDataGrid.CancelEdit();
            withdrawalDataGrid.CancelEdit();
            //BindingListCollectionView coll=CollectionViewSource.GetDefaultView(detailDataGrid.ItemsSource) as BindingListCollectionView;
            //if (coll.IsAddingNew) coll.CancelNew();
            //if (coll.IsEditingItem)  coll.CancelEdit();
            //foreach (DataRowView viewrow in tableDetail.DefaultView) viewrow.CancelEdit();
            //foreach (DataRowView viewrow in tableWithdrawal.DefaultView) viewrow.CancelEdit();
            //tableDetail.DefaultView[0].CancelEdit();
            if (this.ExpenditureRow.RowState == DataRowState.Added | this.ExpenditureRow.RowState == DataRowState.Detached)
            {
                this.tableWithdrawal.RejectChanges();
                this.tableDetail.RejectChanges();
                if (this.ExpenditureRow.RowState == DataRowState.Detached)
                    this.ExpenditureRowView.CancelEdit();
                else
                    this.ExpenditureRow.RejectChanges();
                this.ExpenditureRowView = null;
            }
            else
            {
                if (this.ExpenditureRow.RowState == DataRowState.Deleted) this.ExpenditureRowView = this.thisDataRowView;
                this.ExpenditureRow.RejectChanges();
                this.tableWithdrawal.RejectChanges();
                this.tableDetail.RejectChanges();
            }
            DetailSumRefresh();
        }
        internal void Delete()
        {
            mainGrid.BindingGroup.CancelEdit();
            this.ExpenditureRowView.Delete();
            this.ExpenditureRowView = null;
        }
        private void ComboBox_Loaded(object sender, RoutedEventArgs e) //Bug ComboBoxItem
        { (sender as ComboBox).IsDropDownOpen = true; (sender as ComboBox).IsDropDownOpen = false; }

        #region totalDetailSum
        decimal sumOldValue = 0M;
        private void DetailSumRefresh()
        {
            int totalCount = 0;
            decimal totalSum = 0M;
            DataView view = tableDetail.DefaultView;
            totalCount = view.Count;
            foreach (DataRowView viewrow in view)
            {
                ExpenditureDS.tableExpenditureDetailRow row = viewrow.Row as ExpenditureDS.tableExpenditureDetailRow;
                totalSum = totalSum + row.sumExD;
            }
            ExSumTextBox.Text = totalSum.ToString("N");
        }
        private void detailDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string col = e.Column.Header.ToString();
            if (col == "Сумма")
            {
                decimal.TryParse((e.Column.GetCellContent(e.Row) as TextBlock).Text, out sumOldValue);
            }
        }
        private void detailDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            decimal newvalue = 0;
            if (e.Column.Header.ToString() == "Сумма")
                if (e.EditAction == DataGridEditAction.Cancel)
                {
                    ExpenditureDS.tableExpenditureDetailRow row = (e.Row.Item as DataRowView).Row as ExpenditureDS.tableExpenditureDetailRow;
                    newvalue = row.sumExD;
                    ExSumTextBox.Text = (decimal.Parse(ExSumTextBox.Text) - sumOldValue + newvalue).ToString("N");
                }
                else
                {
                    if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                        ExSumTextBox.Text = (decimal.Parse(ExSumTextBox.Text) - sumOldValue + newvalue).ToString("N");
                }
        }
        private void detailDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                DetailSumRefresh();
            }
        }
        private void detailDataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if ((e.Row.Item is DataRowView) && ((e.Row.Item as DataRowView).Row.RowState == DataRowState.Detached | (e.Row.Item as DataRowView).Row.RowState == DataRowState.Deleted))
            {
                DetailSumRefresh();
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
