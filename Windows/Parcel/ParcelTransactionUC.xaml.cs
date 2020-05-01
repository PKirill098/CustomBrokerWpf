using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for ParcelTransactionUC.xaml
    /// </summary>
    public partial class ParcelTransactionUC : UserControl, ISQLFiltredWindow
    {
        public ParcelTransactionUC()
        {
            InitializeComponent();
            thisDS = new ParcelTransactionDS();
        }

        private decimal totalOldValue = 0;
        ParcelTransactionDS thisDS;
        internal ParcelTransactionDS TransactionDS
        { get { return thisDS; } }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private Window myownerwin;
        private Window OwnerWindow
        {
            get
            {
                if (myownerwin == null)
                {
                    DependencyObject ownerwin = this.Parent;
                    while (!(ownerwin is Window)) ownerwin = (ownerwin as FrameworkElement).Parent;
                    myownerwin = ownerwin as Window;
                }
                return myownerwin;
            }
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Отменить несохраненные изменения в перевозке?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ParcelTransactionDS.tableParcelRow row = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow;
                row.RejectChanges();
                transactionDataGrid.CancelEdit(DataGridEditingUnit.Row);
                foreach (ParcelTransactionDS.tableParcelTransactionRow tranrow in row.GettableParcelTransactionRows())
                {
                    tranrow.RejectChanges();
                    foreach (ParcelTransactionDS.tableOtherRow otherrow in tranrow.GettableOtherRows())
                        otherrow.RejectChanges();
                    foreach (ParcelTransactionDS.tableReturnRow returnrow in tranrow.GettableReturnRows())
                        returnrow.RejectChanges();
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
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                DataRefresh();
            }
        }
        private void toExcelButton_Click(object sender, RoutedEventArgs e)
        {
            ExcelReport();
        }
        private void toDocButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ParcelNumberList.SelectedItem is DataRowView)
                {
                    ParcelTransactionDS.tableParcelRow prow = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow;
                    if (System.IO.Directory.Exists("E:\\Счета\\" + prow.docdirpath))
                    {
                        System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
                    }
                    else if (System.IO.Directory.Exists("E:\\Счета\\" + prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5)))
                    {
                        System.Diagnostics.Process.Start("E:\\Счета\\" + prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5));
                    }
                    else
                    {
                        if (MessageBox.Show("Не удалось найти папку отправки: E:\\Счета\\" + prow.docdirpath + "\nСоздать папку?", "Папка документов", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            System.IO.Directory.CreateDirectory("E:\\Счета\\" + prow.docdirpath);
                            System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Папка документов");
            }
        }
        private void AddPayment_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges())
            {
                //PaymentListWin win = null;

                //ParcelTransactionDS.tableParcelRow row = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow;
                //foreach (Window frwin in this.OwnedWindows)
                //{
                //    if (frwin.Name == "winPaymentList") win = frwin as PaymentListWin;
                //}
                //if (win == null)
                //{
                //    string[] customerid=new string[thisDS.tableAccountBalance.Count];
                //    for (int i = 0; i < thisDS.tableAccountBalance.Count; i++) customerid[i] = thisDS.tableAccountBalance[i].customerid.ToString();
                //    win=new PaymentListWin();
                //    win.Filter.ConditionValuesAdd(win.Filter.ConditionAdd(win.Filter.FilterSQLID, "customerID", "IN"),customerid);
                //    win.Owner=this;
                //    win.Show();
                //}
                //else
                //{
                //    win.Activate();
                //    if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
                //}
                if (SaveChanges())
                {
                    PaymentAddWin winAdd = null;
                    ParcelTransactionDS.tableParcelRow row = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow;
                    DependencyObject ownerwin = this.Parent;
                    while (!(ownerwin is Window)) ownerwin = (ownerwin as FrameworkElement).Parent;
                    foreach (Window frwin in (ownerwin as Window).OwnedWindows)
                    {
                        if (frwin.Name == "winPaymentAdd")
                        {
                            if ((frwin as PaymentAddWin).Parcel == row.parcelId) winAdd = frwin as PaymentAddWin;
                        }
                    }
                    if (winAdd == null)
                    {
                        winAdd = new PaymentAddWin();
                        winAdd.Parcel = row.parcelId;
                        winAdd.Owner = (ownerwin as Window);
                        winAdd.Show();
                    }
                    else
                    {
                        winAdd.Activate();
                        if (winAdd.WindowState == WindowState.Minimized) winAdd.WindowState = WindowState.Normal;
                    }
                }
            }
        }
        private void CustomsButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Проставить таможенный платеж?", "Таможенный платеж", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //ParcelTransactionDSTableAdapters.tableCustomsRefreshTableAdapter adapter = new ParcelTransactionDSTableAdapters.tableCustomsRefreshTableAdapter();
                //adapter.Fill(thisDS.tableCustomsRefresh, ParcelNumberList.SelectedValue != null ? (int)ParcelNumberList.SelectedValue : 0, null);
                //foreach (ParcelTransactionDS.tableCustomsRefreshRow rrow in thisDS.tableCustomsRefresh)
                //{
                foreach (ParcelTransactionDS.tableParcelTransactionRow trow in thisDS.tableParcelTransaction)
                {
                    //if (trow.requestId == rrow.requestid)
                    //{
                    if (!trow.IscustomspayNull() & trow.specloaded)
                    {
                        trow.customs000 = trow.customspay;
                        trow.EndEdit();
                    }
                    //break;
                    //}
                }
                //}
            }
        }
        private void InvoiceCreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges())
            {
                using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    try
                    {
                        con.Open();
                        SqlCommand comm = new SqlCommand("dbo.ParcelTransactionInvoiceCreate_sp", con);
                        comm.CommandType = CommandType.StoredProcedure;
                        SqlParameter parcelid = new SqlParameter("@parcelid", ParcelNumberList.SelectedValue != null ? (int)ParcelNumberList.SelectedValue : 0);
                        comm.Parameters.Add(parcelid);
                        comm.ExecuteNonQuery();
                        comm.Dispose();
                        string curparcel = this.ParcelNumberList.Text;
                        DataLoad();
                        this.ParcelNumberList.Text = curparcel;
                        con.Close();
                        con.Dispose();
                    }
                    catch (Exception ex)
                    {
                        con.Close();
                        con.Dispose();
                        if (ex is System.Data.SqlClient.SqlException)
                        {
                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                            if (err.Number > 49999) MessageBox.Show(err.Message, "Расчет суммы счета", MessageBoxButton.OK, MessageBoxImage.Error);
                            else
                            {
                                System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                {
                                    errs.Append(sqlerr.Message + "\n");
                                }
                                MessageBox.Show(errs.ToString(), "Расчет суммы счета", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Расчет суммы счета", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
        private void InvoicePrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParcelNumberList.SelectedItem is DataRowView)
            {
                ParcelTransactionDS.tableAccountBalanceRow[] balancerows = new ParcelTransactionDS.tableAccountBalanceRow[] { (balanceDataGrid.CurrentItem as DataRowView).Row as ParcelTransactionDS.tableAccountBalanceRow };
                CreateListInvoiceExcel((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow, balancerows, true, sender);
            }
        }
        private void InvoiceExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParcelNumberList.SelectedItem is DataRowView)
            {
                ParcelTransactionDS.tableAccountBalanceRow[] balancerows = new ParcelTransactionDS.tableAccountBalanceRow[] { (balanceDataGrid.CurrentItem as DataRowView).Row as ParcelTransactionDS.tableAccountBalanceRow };
                CreateListInvoiceExcel((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow, balancerows, false, sender);
            }
        }
        private void AllInvoicePrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParcelNumberList.SelectedItem is DataRowView)
            {
                ParcelTransactionDS.tableAccountBalanceRow[] balancerows = new ParcelTransactionDS.tableAccountBalanceRow[thisDS.tableAccountBalance.Rows.Count];
                thisDS.tableAccountBalance.Rows.CopyTo(balancerows, 0);
                CreateListInvoiceExcel((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow, balancerows, true, sender);
            }
        }
        private void AllInvoiceExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParcelNumberList.SelectedItem is DataRowView)
            {
                ParcelTransactionDS.tableAccountBalanceRow[] balancerows = new ParcelTransactionDS.tableAccountBalanceRow[thisDS.tableAccountBalance.Rows.Count];
                thisDS.tableAccountBalance.Rows.CopyTo(balancerows, 0);
                CreateListInvoiceExcel((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow, balancerows, false, sender);
            }
        }
        private void OtherDataGridColumn_DoubleClick(object sender, RoutedEventArgs e)
        {
            if (((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow).parcelstatus > 499) return;
            this.transactionDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            this.transactionDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            if (transactionDataGrid.CurrentItem is DataRowView)
            {
                ParcelTransactionDS.tableParcelTransactionRow row = (transactionDataGrid.CurrentItem as DataRowView).Row as ParcelTransactionDS.tableParcelTransactionRow;
                ParcelTransactionOtherWin winOther = null;
                DependencyObject ownerwin = this.Parent;
                while (!(ownerwin is Window)) ownerwin = (ownerwin as FrameworkElement).Parent;
                foreach (Window frwin in (ownerwin as Window).OwnedWindows)
                {
                    if (frwin.Name == "winParcelTransactionOther")
                    {
                        if ((frwin as ParcelTransactionOtherWin).RequestId == row.requestId) winOther = frwin as ParcelTransactionOtherWin;
                    }
                }
                if (winOther == null)
                {
                    winOther = new ParcelTransactionOtherWin();
                    winOther.mainDataGrid.ItemsSource = (transactionDataGrid.CurrentItem as DataRowView).CreateChildView(thisDS.Relations["tableParcelTransaction_ParcelTransactionOther_sp"]);
                    winOther.RequestId = row.requestId;
                    winOther.Title = winOther.Title + " " + row.customerName + " (заявка №" + row.requestId.ToString() + ")";
                    winOther.Owner = (ownerwin as Window);
                    winOther.Show();
                }
                else
                {
                    winOther.Activate();
                    if (winOther.WindowState == WindowState.Minimized) winOther.WindowState = WindowState.Normal;
                }
            }
        }
        private void ReturnDataGridColumn_DoubleClick(object sender, RoutedEventArgs e)
        {
            if (((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow).parcelstatus > 499) return;
            this.transactionDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            this.transactionDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            if (transactionDataGrid.CurrentItem is DataRowView)
            {
                ParcelTransactionDS.tableParcelTransactionRow row = (transactionDataGrid.CurrentItem as DataRowView).Row as ParcelTransactionDS.tableParcelTransactionRow;
                ParcelTransactionDetailWin win = null;
                foreach (Window frwin in this.OwnerWindow.OwnedWindows)
                {
                    if (frwin.Name == "winParcelTransactionDetail")
                    {
                        if ((frwin as ParcelTransactionDetailWin).RequestId == row.requestId) win = frwin as ParcelTransactionDetailWin;
                    }
                }
                if (win == null)
                {
                    win = new ParcelTransactionDetailWin();
                    win.mainDataGrid.ItemsSource = (transactionDataGrid.CurrentItem as DataRowView).CreateChildView(thisDS.Relations["FK_tableParcelTransaction_tableReturn"]);
                    win.RequestId = row.requestId;
                    win.Title = "Возвраты " + row.customerName + " (заявка №" + row.requestId.ToString() + ")";
                    win.Owner = this.OwnerWindow;
                    win.Show();
                }
                else
                {
                    win.Activate();
                    if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
                }
            }
        }

        private void BalanceInfoButton_Click(object sender, RoutedEventArgs e)
        {
            ParcelTransactionDS.tableAccountBalanceRow row = (balanceDataGrid.CurrentItem as DataRowView).Row as ParcelTransactionDS.tableAccountBalanceRow;
            InvoicePaymentWin win = null;
            foreach (Window frwin in this.OwnerWindow.OwnedWindows)
            {
                if (frwin.Name == "winInvoicePayment")
                {
                    if ((frwin as InvoicePaymentWin).CustomerId == row.customerid)
                    {
                        win = frwin as InvoicePaymentWin;
                        break;
                    }
                }
            }

            if (win == null)
            {
                win = new InvoicePaymentWin();
                win.CustomerId = row.customerid;
                win.CustomerName = row.customername;
                win.Owner = this.OwnerWindow;
                win.Show();
            }
            else
            {
                win.Activate();
                if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
            }
            //AccountWin win = null;
            //foreach (Window frwin in this.OwnedWindows)
            //{
            //    if (frwin.Name == "winAccount")
            //    {
            //        if ((frwin as AccountWin).AccountId == row.customeraccount)
            //        {
            //            win = frwin as AccountWin;
            //            break;
            //        }
            //    }
            //}

            //if (win == null)
            //{
            //    win = new AccountWin();
            //    win.AccountId = row.customeraccount;
            //    win.Owner = this;
            //    win.Show();
            //}
            //else
            //{
            //    win.Activate();
            //    if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
            //}
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

        private void ParcelNumberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelChanged();
        }
        private void ParcelChanged()
        {
            int parceltype = ParcelNumberList.SelectedItem != null ? ((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow).parceltype  : 0;
            int parcelid = ParcelNumberList.SelectedValue != null ? (int)ParcelNumberList.SelectedValue : 0;
            if (parcelid != 0 & parceltype !=2)
            {
                ParcelTransactionDS.tableParcelRow row = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow;
                using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    try
                    {
                        decimal d;
                        SqlCommand com = new SqlCommand();
                        com.Connection = con;
                        com.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        if (row.IsdeliverypriceNull())
                        {
                            com.CommandText = "dbo.Reference_sp";
                            SqlParameter refid = new SqlParameter("@refid", SqlDbType.Char);
                            SqlParameter refname = new SqlParameter("@refname", SqlDbType.NVarChar);
                            refname.Direction = ParameterDirection.Output;
                            refname.Size = 50;
                            SqlParameter refval = new SqlParameter("@refvalue", SqlDbType.NVarChar);
                            refval.Direction = ParameterDirection.Output;
                            refval.Size = 10;
                            com.Parameters.Add(refid);
                            com.Parameters.Add(refname);
                            com.Parameters.Add(refval);
                            refid.Value = "dlvcn";
                            com.ExecuteNonQuery();
                            if (decimal.TryParse(refval.Value.ToString(), out d))
                            {
                                row.deliveryprice = d;
                                row.EndEdit();
                            }
                        }
                        if (row.IsinsurancepriceNull())
                        {
                            com.Parameters.Clear();
                            com.CommandText = "dbo.Reference_sp";
                            SqlParameter refid = new SqlParameter("@refid", SqlDbType.Char);
                            SqlParameter refname = new SqlParameter("@refname", SqlDbType.NVarChar);
                            refname.Direction = ParameterDirection.Output;
                            refname.Size = 50;
                            SqlParameter refval = new SqlParameter("@refvalue", SqlDbType.NVarChar);
                            refval.Direction = ParameterDirection.Output;
                            refval.Size = 10;
                            com.Parameters.Add(refid);
                            com.Parameters.Add(refname);
                            com.Parameters.Add(refval);
                            refid.Value = "inspr";
                            com.ExecuteNonQuery();
                            if (decimal.TryParse(refval.Value.ToString(), out d))
                            {
                                row.insuranceprice = d;
                                row.EndEdit();
                            }
                        }
                        if (row.IsusdrateNull())
                        {
                            com.Parameters.Clear();
                            com.CommandText = "dbo.AccountCurrencyRateGet_sp";
                            SqlParameter fromcur = new SqlParameter("@FromCurrencyCode", "XXD");
                            SqlParameter tocur = new SqlParameter("@ToCurrencyCode", "RUB");
                            SqlParameter datecur = new SqlParameter("@RateDate", DateTime.Today);
                            SqlParameter nest = new SqlParameter("@nestind", 1);
                            SqlParameter rate = new SqlParameter("@AverageRate", SqlDbType.Decimal); rate.Size = 18; rate.Scale = 10;
                            rate.Direction = ParameterDirection.Output;
                            com.Parameters.Add(fromcur); com.Parameters.Add(tocur); com.Parameters.Add(datecur); com.Parameters.Add(nest); com.Parameters.Add(rate);
                            com.ExecuteNonQuery();
                            if (decimal.TryParse(rate.Value.ToString(), out d))
                            {
                                row.usdrate = d;
                                row.EndEdit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Data.SqlClient.SqlException)
                        {
                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                            if (err.Number > 49999) MessageBox.Show(err.Message, "Стоимость доставки", MessageBoxButton.OK, MessageBoxImage.Error);
                            else
                            {
                                System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                {
                                    errs.Append(sqlerr.Message + "\n");
                                }
                                MessageBox.Show(errs.ToString(), "Стоимость доставки", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Стоимость доставки", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            loadTransaction(parcelid);
        }
        private void loadTransaction(int parcelid)
        {

            this.transactionDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            this.transactionDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            transactionDataGrid.ItemsSource = null;
            balanceDataGrid.ItemsSource = null;
            legalDataGrid.ItemsSource = null;
            SaveChanges();
            foreach (Window frwin in this.OwnerWindow.OwnedWindows)
            {
                if (frwin.Name == "winParcelTransactionOther" | frwin.Name == "winParcelTransactionDetail")
                {
                    frwin.Close();
                }
            }
            if (parcelid != 0)
            {
                ParcelTransactionDSTableAdapters.ParcelTransactionAdapter tranAdapter = new ParcelTransactionDSTableAdapters.ParcelTransactionAdapter();
                ParcelTransactionDSTableAdapters.adapterOther adapterOther = new ParcelTransactionDSTableAdapters.adapterOther();
                ParcelTransactionDSTableAdapters.ReturnAdapter adaptertReturn = new ParcelTransactionDSTableAdapters.ReturnAdapter();
                ParcelTransactionDSTableAdapters.AccountBalanceAdapter balanceadapter = new ParcelTransactionDSTableAdapters.AccountBalanceAdapter();
                ParcelTransactionDSTableAdapters.LegalAdapter legalaAdapter = new ParcelTransactionDSTableAdapters.LegalAdapter();
                adapterOther.ClearBeforeFill = false;
                try
                {
                    thisDS.tableOther.Clear();
                    thisDS.tableReturn.Clear();
                    tranAdapter.Fill(thisDS.tableParcelTransaction, parcelid);
                    adapterOther.Fill(thisDS.tableOther, parcelid);
                    adaptertReturn.Fill(thisDS.tableReturn, parcelid);
                    balanceadapter.Fill(thisDS.tableAccountBalance, parcelid);
                    legalaAdapter.Fill(thisDS.tableLegal, parcelid);
                    thisDS.tableLegal.AddtableLegalRow(string.Empty, (decimal)thisDS.tableLegal.Compute("SUM(invoicesum)", string.Empty), (decimal)thisDS.tableLegal.Compute("SUM(takesum)", string.Empty), (decimal)thisDS.tableLegal.Compute("SUM(passum)", string.Empty), (decimal)thisDS.tableLegal.Compute("SUM(jsum)", string.Empty), (decimal)thisDS.tableLegal.Compute("SUM(costsum)", string.Empty));
                    DataView viewrequest = thisDS.tableParcelTransaction.DefaultView;
                    viewrequest.Sort = "customerName,requestId";
                    (this.mainGrid.FindResource("keyAlternationBackground") as AlternationBackground).Reset();
                    transactionDataGrid.ItemsSource = viewrequest;
                    SetRowBackGround();
                    thisDS.tableAccountBalance.DefaultView.Sort = "customername";
                    thisDS.tableLegal.DefaultView.Sort = "sortcolumn";
                    balanceDataGrid.ItemsSource = thisDS.tableAccountBalance.DefaultView;
                    legalDataGrid.ItemsSource = thisDS.tableLegal.DefaultView;
                    totalDataRefresh();
                }
                catch (Exception ex)
                {

                    if (ex is System.Data.SqlClient.SqlException)
                    {
                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                        if (err.Number > 49999) MessageBox.Show(err.Message, "Загрузка платежей", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Загрузка платежей", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка платежей", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    return;
                }
            }
        }

        private void DataLoad()
        {
            ReferenceDS referenceDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (referenceDS.tableLegalEntity.Count == 0)
            {
                ReferenceDSTableAdapters.LegalEntityAdapter thisLegalEntityAdapter = new ReferenceDSTableAdapters.LegalEntityAdapter();
                thisLegalEntityAdapter.Fill(referenceDS.tableLegalEntity);
            }
            CollectionViewSource accountSettlementVS = balanceDataGrid.FindResource("keyAccountSettlementVS") as CollectionViewSource;
            accountSettlementVS.Source = new DataView(referenceDS.tableLegalEntity, string.Empty, string.Empty, DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent);
            if (referenceDS.tableRequestStatus.Count == 0)
            {
                ReferenceDSTableAdapters.RequestStatusAdapter adapterStatus = new ReferenceDSTableAdapters.RequestStatusAdapter();
                adapterStatus.Fill(referenceDS.tableRequestStatus);
            }
            statusComboBox.ItemsSource = new System.Data.DataView(referenceDS.tableRequestStatus, "rowId>49", "rowId", DataViewRowState.CurrentRows);
            ParcelTransactionDSTableAdapters.ParcelAdapter adapterParcel = new ParcelTransactionDSTableAdapters.ParcelAdapter();
            ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged; ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged; ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged;
            this.mainGrid.DataContext = null;
            transactionDataGrid.ItemsSource = null;
            thisDS.tableOther.Clear();
            thisDS.tableReturn.Clear();
            thisDS.tableParcelTransaction.Clear();
            adapterParcel.Fill(thisDS.tableParcel, this.thisfilter.FilterWhereId);
            ParcelNumberList.SelectionChanged += ParcelNumberList_SelectionChanged;
            this.mainGrid.DataContext = new DataView(thisDS.tableParcel, string.Empty, "sortnumber DESC", DataViewRowState.CurrentRows);
            setFilterButtonImage();
        }
        private void DataRefresh()
        {
            string curparcel = this.ParcelNumberList.Text;
            ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged; ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged; ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged; ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged;
            DataLoad();
            this.ParcelNumberList.Text = curparcel;
            ParcelChanged();
            ParcelNumberList.SelectionChanged += ParcelNumberList_SelectionChanged;
        }
        internal bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.mainGrid.DataContext) as BindingListCollectionView;
                IInputElement fcontrol = System.Windows.Input.FocusManager.GetFocusedElement(this);
                if (view.CurrentItem != null & fcontrol is TextBox)
                {
                    BindingExpression be;
                    be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                    if (be != null)
                    {
                        //DataRow row = (view.CurrentItem as DataRowView).Row;
                        //decimal d;
                        //bool isDirty = false;
                        //switch (be.ParentBinding.Path.Path)
                        //{
                        //    case "usdrate":
                        //    case "deliveryprice":
                        //    case "insuranceprice":
                        //    case "declaration":
                        //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !decimal.TryParse((fcontrol as TextBox).Text, out d) || row.Field<Decimal>(be.ParentBinding.Path.Path) != d;
                        //        break;
                        //    default:
                        //        isDirty = true;
                        //        MessageBox.Show("Поле не добавлено в обработчик сохранения без потери фокуса!", "Сохранение изменений");
                        //        break;
                        //}
                        if (be.IsDirty) be.UpdateSource();
                        if (be.HasError) return false;
                    }
                }
                if (view.IsEditingItem) view.CommitEdit();
                this.transactionDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                this.transactionDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                ParcelTransactionDSTableAdapters.ParcelAdapter parcelAdapter = new ParcelTransactionDSTableAdapters.ParcelAdapter();
                ParcelTransactionDSTableAdapters.ParcelTransactionAdapter tranAdapter = new ParcelTransactionDSTableAdapters.ParcelTransactionAdapter();
                ParcelTransactionDSTableAdapters.adapterOther adapterOther = new ParcelTransactionDSTableAdapters.adapterOther();
                ParcelTransactionDSTableAdapters.ReturnAdapter adapterReturn = new ParcelTransactionDSTableAdapters.ReturnAdapter();
                thisDS.tableOther.SetStatus();
                adapterOther.Update(thisDS.tableOther);
                thisDS.tableReturn.SetStatus();
                adapterReturn.Update(thisDS.tableReturn);
                thisDS.tableParcelTransaction.SetStatus();
                tranAdapter.Update(thisDS.tableParcelTransaction);
                parcelAdapter.Update(thisDS.tableParcel);
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
                //if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                //{
                //    isSuccess = SaveChanges();
                //}
            }
            return isSuccess;
        }

        private void CreateListInvoiceExcel(ParcelTransactionDS.tableParcelRow parcelrow, ParcelTransactionDS.tableAccountBalanceRow[] balancerows, bool isprint, object hostPopup)
        {
            if (((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow).parcelstatus > 499) return;
            if (SaveChanges() & !System.IO.File.Exists(Environment.CurrentDirectory + @"\Templates\Счет.xlt.xlt"))
            {
                foreach (ParcelTransactionDS.tableAccountBalanceRow balancerow in balancerows)
                {
                    CreateInvoiceExcel(parcelrow, balancerow, isprint);
                }
                PopupText.Text = "Подготовка документов завершена";
                popInf.PlacementTarget = hostPopup as UIElement;
                popInf.IsOpen = true;
            }
        }
        private void CreateInvoiceExcel(ParcelTransactionDS.tableParcelRow parcelrow, ParcelTransactionDS.tableAccountBalanceRow balancerow, bool isprint)
        {
            int customerid, payAccount, invoiceid, group;
            string customerName, customerFullName, legalName;
            decimal sumtot = 0M;

            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            try
            {
                balanceDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                customerid = balancerow.customerid;
                payAccount = balancerow.IsinvoiceaccountNull() ? 0 : balancerow.invoiceaccount;
                customerName = balancerow.customername;
                if (balancerow.IscustomerfullnameNull()) customerFullName = balancerow.customername; else customerFullName = balancerow.customerfullname;
                if (payAccount > 0)
                {
                    ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
                    legalName = refDS.tableLegalEntity.Select("accountid=" + payAccount.ToString())[0].Field<string>("namelegal");
                }
                else legalName = string.Empty;

                Excel.Workbook exWb = exApp.Workbooks.Add(Environment.CurrentDirectory + @"\Templates\Счет.xlt");
                Excel.Worksheet exWh = exWb.Sheets[1];

                using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    con.Open();
                    SqlParameter parparcelid = new SqlParameter("@parcelid", parcelrow.parcelId);
                    SqlParameter parcustomerid = new SqlParameter("@customerid", customerid);
                    SqlParameter paid = new SqlParameter("@accountid", payAccount);
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = con;
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = "dbo.ParcelTransactionInvoice_sp";
                    comm.Parameters.Add(parparcelid); comm.Parameters.Add(parcustomerid); comm.Parameters.Add(paid);
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        const int invoiceidf = 0, finvoicedatef = 1, groupdetailid = 2, agentnamef = 3, storagepointf = 4, customernotef = 5, cellnumber = 6, volumef = 7, calcweightf = 8, detdescriptionf = 9, detamountf = 10, detpricef = 11, detsumf = 12;
                        int dr = 0, dg = 0, drtot = 0, dn = 0;
                        string docpath;
                        decimal rate = 1M, dept = 0M;
                        string curr = "RUB";
                        //if (payAccount > 0)
                        //{
                        //ReferenceDS.tableLegalEntityRow account = (this.FindResource("keyReferenceDS") as ReferenceDS).tableLegalEntity.Select("accountid=" + payAccount.ToString())[0] as ReferenceDS.tableLegalEntityRow;
                        //if (account.bankaccountcurr != curr)
                        //{
                        //    SqlParameter fromcur = new SqlParameter("@FromCurrencyCode", curr);
                        //    SqlParameter tocur = new SqlParameter("@ToCurrencyCode", account.bankaccountcurr);
                        //    SqlParameter ratedate = new SqlParameter("@RateDate", DateTime.Today);
                        //    SqlParameter arate = new SqlParameter("@AverageRate", SqlDbType.Decimal);
                        //    arate.Size = 18; arate.Scale = 10;
                        //    arate.Direction = ParameterDirection.Output;
                        //    SqlCommand commGetRate = new SqlCommand();
                        //    using (SqlConnection conGetRate = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                        //    {
                        //        conGetRate.Open();
                        //        commGetRate.Connection = conGetRate;
                        //        commGetRate.CommandType = CommandType.StoredProcedure;
                        //        commGetRate.CommandText = "dbo.AccountCurrencyRateGet_sp";
                        //        commGetRate.Parameters.Add(fromcur); commGetRate.Parameters.Add(tocur); commGetRate.Parameters.Add(ratedate); commGetRate.Parameters.Add(arate);
                        //        commGetRate.ExecuteNonQuery();
                        //        rate = (decimal)arate.Value;
                        //        curr = account.bankaccountcurr;
                        //        conGetRate.Close();
                        //        conGetRate.Dispose();
                        //    }
                        //}
                        //exWh.Cells[5, 2] = account.bankName;
                        //exWh.Cells[5, 26] = account.bankBIC;
                        //if (!account.IsbackCorrAccountNull()) exWh.Cells[6, 26] = account.backCorrAccount;
                        //exWh.Cells[8, 26] = account.bankaccount;
                        //}
                        reader.Read();
                        group = reader.GetInt32(groupdetailid);
                        if (group == 0)
                        {
                            sumtot = sumtot + reader.GetDecimal(detsumf) * rate;
                            dept = decimal.Ceiling(reader.GetDecimal(detsumf) * rate * 100M) / 100M;
                            reader.Read();
                            group = reader.GetInt32(groupdetailid);
                        }
                        invoiceid = reader.GetInt32(invoiceidf);
                        docpath = invoiceid.ToString() + "-" + parcelrow.fullNumber;
                        exWh.Cells[3, 12] = docpath + " от " + reader.GetDateTime(finvoicedatef).ToShortDateString();
                        exWh.Cells[6, 6] = customerFullName;
                        exWh.Cells[8, 6] = legalName;
                        if (!reader.IsDBNull(agentnamef)) exWh.Cells[10, 9] = reader.GetString(agentnamef);
                        if (!reader.IsDBNull(storagepointf)) exWh.Cells[11, 8] = reader.GetString(storagepointf);
                        if (!reader.IsDBNull(cellnumber)) exWh.Cells[11, 13] = reader.GetInt32(cellnumber);
                        if (!reader.IsDBNull(calcweightf)) exWh.Cells[11, 15] = reader.GetDecimal(calcweightf);
                        if (!reader.IsDBNull(volumef)) exWh.Cells[11, 19] = reader.GetDecimal(volumef);
                        if (reader.IsDBNull(customernotef))
                        {
                            exWh.Rows[12].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                        }
                        else
                        {
                            string notestr = reader.GetString(customernotef);
                            if (notestr.Length > 60) exWh.Rows[12].RowHeight = exWh.Rows[12].RowHeight * 2;
                            exWh.Cells[12, 10] = reader.GetString(customernotef);
                            dn = 1;
                        }
                        exWh.Cells[14 + dn, 4] = reader.GetString(detdescriptionf);
                        exWh.Cells[14 + dn, 18] = reader.GetInt16(detamountf);
                        exWh.Cells[14 + dn, 19].Value = decimal.Divide(decimal.Ceiling(reader.GetDecimal(detpricef) * rate * 100M), 100M);
                        exWh.Cells[14 + dn, 20].Value = decimal.Divide(decimal.Ceiling(reader.GetDecimal(detsumf) * rate * 100M), 100M);
                        sumtot = sumtot + reader.GetDecimal(detsumf) * rate;
                        StringBuilder totsum = new StringBuilder();
                        Excel.Range range;
                        while (reader.Read())
                        {
                            if (group != reader.GetInt32(groupdetailid))
                            {
                                drtot = drtot + dr + 1;
                                range = exWh.Range[exWh.Cells[14 + dg, 2], exWh.Cells[14 + dg + dr + dn, 20]];
                                range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                                range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                                range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                                range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                                range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
                                range.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
                                exWh.Cells[16 + dg + dr + dn, 20].Formula = "=SUM(T" + (14 + dg).ToString() + ":T" + (14 + dg + dr + dn).ToString() + ")";
                                totsum.Append("+SUM(T"); totsum.Append((14 + dg).ToString()); totsum.Append(":T"); totsum.Append((14 + dg + dr + dn).ToString()); totsum.Append(")");
                                exWh.Rows[26 + dg + dr + dn].Insert(Excel.XlInsertShiftDirection.xlShiftDown, exWh.Range[exWh.Rows[17 + dg + dr + dn], exWh.Rows[25 + dg + dr + dn]].Copy());
                                exWh.Cells[27 + dg + dr + dn, 3] = (int)exWh.Cells[18 + dg + dr + dn, 3].Value + 1;
                                dg = dg + dr + dn + 8;
                                dr = 0; dn = 0;
                                group = reader.GetInt32(groupdetailid);
                                if (!reader.IsDBNull(agentnamef)) exWh.Cells[10 + dg, 9] = reader.GetString(agentnamef);
                                if (!reader.IsDBNull(storagepointf)) exWh.Cells[11 + dg, 8] = reader.GetString(storagepointf);
                                if (!reader.IsDBNull(cellnumber)) exWh.Cells[11 + dg, 13] = reader.GetInt32(cellnumber);
                                if (!reader.IsDBNull(calcweightf)) exWh.Cells[11 + dg, 15] = reader.GetDecimal(calcweightf);
                                if (!reader.IsDBNull(volumef)) exWh.Cells[11 + dg, 19] = reader.GetDecimal(volumef);
                                if (reader.IsDBNull(customernotef))
                                {
                                    exWh.Rows[12 + dg].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                                }
                                else
                                {
                                    if (reader.GetString(customernotef).Length > 60) exWh.Rows[12 + dg].RowHeight = exWh.Rows[12 + dg].RowHeight * 2;
                                    exWh.Cells[12 + dg, 10] = reader.GetString(customernotef);
                                    dn = 1;
                                }
                                exWh.Cells[14 + dg + dn, 4] = reader.GetString(detdescriptionf);
                                exWh.Cells[14 + dg + dn, 18] = reader.GetInt16(detamountf);
                                exWh.Cells[14 + dg + dn, 19] = decimal.Divide(decimal.Ceiling(reader.GetDecimal(detpricef) * rate * 100M), 100M);
                                exWh.Cells[14 + dg + dn, 20].Value = decimal.Divide(decimal.Ceiling(reader.GetDecimal(detsumf) * rate * 100M), 100M);
                                sumtot = sumtot + reader.GetDecimal(detsumf) * rate;
                                continue;
                            }
                            dr++;
                            range = exWh.Rows[14 + dg + dr + dn];
                            range.Insert(Excel.XlInsertShiftDirection.xlShiftDown, false);

                            exWh.Range[exWh.Cells[14 + dg + dr + dn, 2], exWh.Cells[14 + dg + dr + dn, 3]].MergeCells = true;
                            exWh.Range[exWh.Cells[14 + dg + dr + dn, 4], exWh.Cells[14 + dg + dr + dn, 17]].MergeCells = true;

                            exWh.Cells[14 + dg + dr + dn, 2] = dr + 1;
                            exWh.Cells[14 + dg + dr + dn, 4] = reader.GetString(detdescriptionf);
                            exWh.Cells[14 + dg + dr + dn, 18] = reader.GetInt16(detamountf).ToString();
                            exWh.Cells[14 + dg + dr + dn, 19] = decimal.Divide(decimal.Ceiling(reader.GetDecimal(detpricef) * rate * 100M), 100M);
                            exWh.Cells[14 + dg + dr + dn, 20].Value = decimal.Divide(decimal.Ceiling(reader.GetDecimal(detsumf) * rate * 100M), 100M);
                            sumtot = sumtot + reader.GetDecimal(detsumf) * rate;
                        }

                        reader.Close();
                        drtot = drtot + dr + 1;
                        range = exWh.Range[exWh.Cells[14 + dg, 2], exWh.Cells[14 + dg + dr + dn, 20]];
                        range.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                        range.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                        range.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                        range.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                        range.Borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = Excel.XlBorderWeight.xlThin;
                        range.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlThin;
                        exWh.Cells[16 + dg + dr + dn, 20].Formula = "=SUM(T" + (14 + dg).ToString() + ":T" + (14 + dg + dr + dn).ToString() + ")";
                        totsum.Append("+SUM(T"); totsum.Append((14 + dg).ToString()); totsum.Append(":T"); totsum.Append((14 + dg + dr + dn).ToString()); totsum.Append(")");
                        totsum.Remove(0, 1); totsum.Insert(0, "=");
                        range = exWh.Range[exWh.Rows[17 + dg + dr + dn], exWh.Rows[25 + dg + dr + dn]];
                        range.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                        //exWh.Cells[19 + dg + dr + dn, 20] = totsum.ToString();
                        if (dept != 0M)
                        {
                            exWh.Cells[18 + dg + dr + dn, 20] = dept;
                            //exWh.Cells[19 + dg + dr + dn, 20].Formula = exWh.Cells[19 + dg + dr + dn, 20].Formula + "+T" + (18 + dg + dr + dn).ToString();
                        }
                        else
                        {
                            exWh.Rows[18 + dg + dr + dn].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                            dr = dr - 1;
                        }
                        exWh.Cells[19 + dg + dr + dn, 20] = decimal.Round(sumtot, 0) > 0M ? decimal.Round(sumtot, 0) : 0M;
                        exWh.Cells[20 + dg + dr + dn, 10] = drtot;
                        exWh.Cells[21 + dg + dr + dn, 2] = AmountCurrWords((decimal)exWh.Cells[19 + dg + dr + dn, 20].Value, (CurrencyName)Enum.Parse(typeof(CurrencyName), curr, true));
                        exWh.Rows[8].Delete(Excel.XlDeleteShiftDirection.xlShiftUp);

                        docpath = docpath.Insert(0, customerName.Replace("\\", string.Empty).Replace("/", string.Empty).Replace(":", string.Empty).Replace("*", string.Empty).Replace("?", string.Empty).Replace("\"", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty).Replace("|", string.Empty).Replace(".", string.Empty) + "-");
                        if (System.IO.Directory.Exists("E:\\Счета\\" + parcelrow.docdirpath))
                        {
                            docpath = docpath.Insert(0, "E:\\Счета\\" + parcelrow.docdirpath + "\\");
                        }
                        else if (System.IO.Directory.Exists("E:\\Счета\\" + parcelrow.fullNumber + parcelrow.docdirpath.Substring(parcelrow.docdirpath.Length - 5)))
                        {
                            docpath = docpath.Insert(0, "E:\\Счета\\" + parcelrow.fullNumber + parcelrow.docdirpath.Substring(parcelrow.docdirpath.Length - 5) + "\\");
                        }
                        else
                        {
                            MessageBox.Show("Не удалось найти папку отправки: E:\\Счета\\" + parcelrow.docdirpath);
                            docpath = string.Empty;
                        }
                        if (docpath != string.Empty)
                        {
                            exWb.SaveAs(docpath);
                        }

                        //if (balancerow.RowState == DataRowState.Modified)
                        //{
                        //    try
                        //    {
                        //        SqlParameter iid = new SqlParameter("@invoiceid", invoiceid);
                        //        SqlParameter aid = new SqlParameter("@accountId", payAccount);
                        //        SqlCommand comm2 = new SqlCommand();
                        //        comm2.Connection = con;
                        //        comm2.CommandType = CommandType.StoredProcedure;
                        //        comm2.CommandText = "account.InvoiceUpd_sp";
                        //        comm2.Parameters.Add(iid); comm2.Parameters.Add(aid);
                        //        comm2.ExecuteNonQuery();
                        //        balancerow.AcceptChanges();
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        if (ex is System.Data.SqlClient.SqlException)
                        //        {
                        //            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                        //            if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                        //            else
                        //            {
                        //                System.Text.StringBuilder errs = new System.Text.StringBuilder();
                        //                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                        //                {
                        //                    errs.Append(sqlerr.Message + "\n");
                        //                }
                        //                MessageBox.Show(errs.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                        //        }
                        //    }
                        //}

                        if (isprint)
                        {
                            exWh.PageSetup.Zoom = false;
                            exWh.PageSetup.FitToPagesWide = 1;
                            exWh.PageSetup.FitToPagesTall = 100;
                            while (exWh.PageSetup.Pages.Count == exWh.PageSetup.FitToPagesTall)
                            {
                                exWh.PageSetup.FitToPagesTall = exWh.PageSetup.FitToPagesTall * 2;
                            }
                            exWh.PrintOutEx(Type.Missing, Type.Missing, 1, false, true);
                        }

                        if (docpath != string.Empty) exWb.Close(); else exApp.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("У клиента " + customerName + " не найдены платежи.\nДля подготовки счета необходимо предварительно рассчитать платежи!", "Подготовка счета", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    reader.Close();
                    con.Close();
                    con.Dispose();
                }
                exWh = null;
                exWb = null;
            }
            catch (Exception ex)
            {
                if (exApp != null)
                {
                    foreach (Excel.Workbook itemBook in exApp.Workbooks)
                    {
                        itemBook.Close(false);
                    }
                    exApp.Quit();
                }
                MessageBox.Show(ex.Message, "Подготовка счета", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                exApp.DisplayAlerts = true;
                exApp.ScreenUpdating = true;
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }

        private enum CurrencyName
        {
            RUB = 0,
            USD = 1,
            EUR = 2
        }
        private string AmountCurrWords(decimal summ, CurrencyName curr)
        {
            int countNumeral;
            string strsumm;
            StringBuilder amountWords = new StringBuilder();
            if (summ.CompareTo(999999999999M) > 0) throw new Exception("Превышен максимальный диапазон преобразования!");
            strsumm = decimal.Truncate(summ).ToString();
            countNumeral = strsumm.Length;
            amountWords.Append(AmountWords(strsumm, false, true));
            switch (curr)
            {
                case CurrencyName.RUB:
                    if ((strsumm.Length > 1 && strsumm[countNumeral - 2] == '1') | strsumm[countNumeral - 1] == '0' | strsumm[countNumeral - 1] > '4')
                        amountWords.Append(" рублей ");
                    else
                        if (strsumm[countNumeral - 1] == '1') amountWords.Append(" рубль "); else amountWords.Append(" рубля ");
                    break;
                case CurrencyName.USD:
                    if ((strsumm.Length > 1 && strsumm[countNumeral - 2] == '1') | strsumm[countNumeral - 1] == '0' | strsumm[countNumeral - 1] > '4')
                        amountWords.Append(" долларов США ");
                    else
                        if (strsumm[countNumeral - 1] == '1') amountWords.Append(" доллар США "); else amountWords.Append(" доллара США ");
                    break;
                case CurrencyName.EUR:
                    amountWords.Append(" евро ");
                    break;
                default:
                    break;
            }
            strsumm = decimal.Truncate(decimal.Multiply(decimal.Subtract(decimal.Round(summ, 2), decimal.Truncate(summ)), 100)).ToString().PadLeft(2, '0');
            if (strsumm != "00")
            {
                amountWords.Append(strsumm);
                switch (curr)
                {
                    case CurrencyName.RUB:
                        if (strsumm[0] == '1' | strsumm[1] == '0' | strsumm[1] > '4')
                            amountWords.Append(" копеек");
                        else
                            if (strsumm[1] == '1') amountWords.Append(" копейка"); else amountWords.Append(" копейки");
                        break;
                    case CurrencyName.USD:
                    case CurrencyName.EUR:
                        if (strsumm[0] == '1' | strsumm[1] == '0' | strsumm[1] > '4')
                            amountWords.Append(" центов");
                        else
                            if (strsumm[1] == '1') amountWords.Append(" цент"); else amountWords.Append(" цента");
                        break;
                    default:
                        break;
                }
            }
            return amountWords.ToString();
        }
        private string AmountWords(string summ, bool isfemale = false, bool isstart = true)
        {

            StringBuilder amountWords = new StringBuilder();
            int countNumeral;
            countNumeral = summ.Length;
            char[] inverse = new char[countNumeral];
            for (int i = 1; i < countNumeral + 1; i++)
            {
                inverse[i - 1] = summ[countNumeral - i];
            }
            if (countNumeral > 1)
            {
                switch (inverse[1])
                {
                    case '0':
                        break;
                    case '1':
                        switch (inverse[0])
                        {
                            case '0':
                                amountWords.Append("десять");
                                break;
                            case '1':
                                amountWords.Append("одиннадцать");
                                break;
                            case '2':
                                amountWords.Append("двенадцать");
                                break;
                            case '3':
                                amountWords.Append("тринадцать");
                                break;
                            case '4':
                                amountWords.Append("четырнадцать");
                                break;
                            case '5':
                                amountWords.Append("пятнадцать");
                                break;
                            case '6':
                                amountWords.Append("шестнадцать");
                                break;
                            case '7':
                                amountWords.Append("семнадцать");
                                break;
                            case '8':
                                amountWords.Append("восемнадцать");
                                break;
                            case '9':
                                amountWords.Append("девятнадцать");
                                break;
                        }
                        break;
                    case '2':
                        amountWords.Append("двадцать");
                        break;
                    case '3':
                        amountWords.Append("тридцать");
                        break;
                    case '4':
                        amountWords.Append("сорок");
                        break;
                    case '5':
                        amountWords.Append("пятьдесят");
                        break;
                    case '6':
                        amountWords.Append("шестьдесят");
                        break;
                    case '7':
                        amountWords.Append("семьдесят");
                        break;
                    case '8':
                        amountWords.Append("восемьдесят");
                        break;
                    case '9':
                        amountWords.Append("девяносто");
                        break;
                }
                if (inverse[1] != '1')
                {
                    switch (inverse[0])
                    {
                        case '0':
                            break;
                        case '1':
                            if (isfemale) amountWords.Append(" одна"); else amountWords.Append(" один");
                            break;
                        case '2':
                            if (isfemale) amountWords.Append(" двe"); else amountWords.Append(" два");
                            break;
                        case '3':
                            amountWords.Append(" три");
                            break;
                        case '4':
                            amountWords.Append(" четыре");
                            break;
                        case '5':
                            amountWords.Append(" пять");
                            break;
                        case '6':
                            amountWords.Append(" шесть");
                            break;
                        case '7':
                            amountWords.Append(" семь");
                            break;
                        case '8':
                            amountWords.Append(" восемь");
                            break;
                        case '9':
                            amountWords.Append(" девять");
                            break;
                    }
                }
                if (countNumeral > 2)
                {
                    switch (inverse[2])
                    {
                        case '0':
                            break;
                        case '1':
                            amountWords.Insert(0, "сто ");
                            break;
                        case '2':
                            amountWords.Insert(0, "двести ");
                            break;
                        case '3':
                            amountWords.Insert(0, "триста ");
                            break;
                        case '4':
                            amountWords.Insert(0, "четыреста ");
                            break;
                        case '5':
                            amountWords.Insert(0, "пятьсот ");
                            break;
                        case '6':
                            amountWords.Insert(0, "шестьсот ");
                            break;
                        case '7':
                            amountWords.Insert(0, "семьсот ");
                            break;
                        case '8':
                            amountWords.Insert(0, "восемьсот ");
                            break;
                        case '9':
                            amountWords.Insert(0, "девятьсот ");
                            break;
                    }
                }
                string thousandsWord;
                StringBuilder thousandstr = new StringBuilder(3);
                if (countNumeral > 3)
                {
                    for (int i = 3; i < countNumeral & i < 6; i++)
                    {
                        thousandstr.Insert(0, inverse[i]);
                    }
                    thousandsWord = AmountWords(thousandstr.ToString(), true, false);
                    if (thousandsWord.Length > 0)
                    {
                        if ((inverse.Length > 4 && inverse[4] == '1') | inverse[3] == '0' | inverse[3] > '4')
                        {
                            amountWords.Insert(0, " тысяч ");
                        }
                        else
                        {
                            if (inverse[3] == '1') amountWords.Insert(0, " тысяча "); else amountWords.Insert(0, " тысячи ");
                        }
                        amountWords.Insert(0, thousandsWord);
                    }
                }
                if (countNumeral > 6)
                {
                    thousandstr.Clear();
                    for (int i = 6; i < countNumeral & i < 9; i++)
                    {
                        thousandstr.Insert(0, inverse[i]);
                    }
                    thousandsWord = AmountWords(thousandstr.ToString(), false, false);
                    if (thousandsWord.Length > 0)
                    {
                        if ((inverse.Length > 7 && inverse[7] == '1') | inverse[6] == '0' | inverse[6] > '4')
                        {
                            amountWords.Insert(0, " миллионов ");
                        }
                        else
                        {
                            if (inverse[6] == '1') amountWords.Insert(0, " миллион "); else amountWords.Insert(0, " миллиона ");
                        }
                        amountWords.Insert(0, thousandsWord);
                    }
                }
                if (countNumeral > 9)
                {
                    thousandstr.Clear();
                    for (int i = 9; i < countNumeral & i < 12; i++)
                    {
                        thousandstr.Insert(0, inverse[i]);
                    }
                    thousandsWord = AmountWords(thousandstr.ToString(), false, false);
                    if (thousandsWord.Length > 0)
                    {
                        if ((inverse.Length > 10 && inverse[10] == '1') | inverse[9] == '0' | inverse[9] > '4')
                        {
                            amountWords.Insert(0, " миллиардов ");
                        }
                        else
                        {
                            if (inverse[9] == '1') amountWords.Insert(0, " миллиард "); else amountWords.Insert(0, " миллиарда ");
                        }
                        amountWords.Insert(0, thousandsWord);
                    }
                }
            }
            else
            {
                switch (inverse[0])
                {
                    case '0':
                        if (isstart) amountWords.Append("ноль");
                        break;
                    case '1':
                        if (isfemale) amountWords.Append("одна"); else amountWords.Append("один");
                        break;
                    case '2':
                        if (isfemale) amountWords.Append("двe"); else amountWords.Append("два");
                        break;
                    case '3':
                        amountWords.Append("три");
                        break;
                    case '4':
                        amountWords.Append("четыре");
                        break;
                    case '5':
                        amountWords.Append("пять");
                        break;
                    case '6':
                        amountWords.Append("шесть");
                        break;
                    case '7':
                        amountWords.Append("семь");
                        break;
                    case '8':
                        amountWords.Append("восемь");
                        break;
                    case '9':
                        amountWords.Append("девять");
                        break;
                }
            }
            return amountWords.ToString();
        }
        #region Filter
        private CustomBrokerWpf.SQLFilter thisfilter = new SQLFilter("parcel", "AND");
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
                    MessageBox.Show("Применение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    thisfilter.RemoveCurrentWhere();
                    thisfilter = value;
                    if (this.IsLoaded) DataRefresh();
                }
            }
        }

        public void RunFilter()
        {
            if (SaveChanges() ||
                MessageBox.Show("Изменения не были сохранены и будут потеряны при применении фильтра.\nОтменить применение фильтра.", "Применение фильтра", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) != MessageBoxResult.Yes)
            {
                DataRefresh();
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

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnerWindow.OwnedWindows)
            {
                if (item.Name == "winParcelFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new ParcelFilterWin();
                    (ObjectWin as ParcelFilterWin).FilterOwner = this;
                    ObjectWin.Owner = this.OwnerWindow;
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
        #region Data Grid Total Sum
        private void totalDataRefresh()
        {
            decimal totalCustoms = 0M, totalDelivery = 0M, totalDiscount = 0M, totalPrggermany = 0M, totalStoregermn = 0M, totalFreightgmn = 0M, totalPreparatgm = 0M, totalPrgmoscow = 0M, totalInsurance = 0M, totalDeliveryms = 0M, totalSertificat = 0M, totalClaim = 0M, totalEscortmscw = 0M, totalReturn = 0M;
            decimal totalWeight = 0M, totalVolume = 0M, totalAmountur = 0M, totalAmountusd = 0M, totalOthers = 0M, totalInvoicesum = 0M, totalPaysum = 0M;
            if (this.transactionDataGrid.SelectedItems.Count > 1)
            {
                for (int i = 0; i < this.transactionDataGrid.SelectedItems.Count; i++)
                {
                    if (this.transactionDataGrid.SelectedItems[i] is DataRowView)
                    {
                        ParcelTransactionDS.tableParcelTransactionRow row = (this.transactionDataGrid.SelectedItems[i] as DataRowView).Row as ParcelTransactionDS.tableParcelTransactionRow;
                        if (!row.Iscustoms000Null()) totalCustoms = totalCustoms + row.customs000;
                        if (!row.IsdeliveryCNull()) totalDelivery = totalDelivery + row.deliveryC;
                        if (!row.Isdiscount00Null()) totalDiscount = totalDiscount + row.discount00;
                        if (!row.IsprggermanyNull()) totalPrggermany = totalPrggermany + row.prggermany;
                        if (!row.IsstoregermnNull()) totalStoregermn = totalStoregermn + row.storegermn;
                        if (!row.IsfreightgmnNull()) totalFreightgmn = totalFreightgmn + row.freightgmn;
                        if (!row.IspreparatgmNull()) totalPreparatgm = totalPreparatgm + row.preparatgm;
                        if (!row.Isprgmoscow0Null()) totalPrgmoscow = totalPrgmoscow + row.prgmoscow0;
                        if (!row.IsinsuranceNull()) totalInsurance = totalInsurance + row.insurance;
                        if (!row.IsdeliverymsNull()) totalDeliveryms = totalDeliveryms + row.deliveryms;
                        if (!row.IssertificatNull()) totalSertificat = totalSertificat + row.sertificat;
                        if (!row.Isclaim00000Null()) totalClaim = totalClaim + row.claim00000;
                        if (!row.IsescortmscwNull()) totalEscortmscw = totalEscortmscw + row.escortmscw;
                        if (!row.Isreturn0000Null()) totalReturn = totalReturn + row.return0000;
                        if (!row.IsCalcWeightNull()) totalWeight = totalWeight + row.CalcWeight;
                        if (!row.IsvolumeNull()) totalVolume = totalVolume + row.volume;
                        if (!row.IsamountusdNull()) totalAmountusd = totalAmountusd + row.amountusd;
                        if (!row.IsgoodValueNull()) totalAmountur = totalAmountur + row.goodValue;
                        if (!row.IsothersNull()) totalOthers = totalOthers + row.others;
                        if (!row.IsinvoicesumNull()) totalInvoicesum = totalInvoicesum + row.invoicesum;
                        if (!row.IspaysumNull()) totalPaysum = totalPaysum + row.paysum;
                    }
                }
            }
            else
            {
                foreach (object item in this.transactionDataGrid.Items)
                {
                    if (item is DataRowView)
                    {
                        ParcelTransactionDS.tableParcelTransactionRow row = (item as DataRowView).Row as ParcelTransactionDS.tableParcelTransactionRow;
                        if (!row.Iscustoms000Null()) totalCustoms = totalCustoms + row.customs000;
                        if (!row.IsdeliveryCNull()) totalDelivery = totalDelivery + row.deliveryC;
                        if (!row.Isdiscount00Null()) totalDiscount = totalDiscount + row.discount00;
                        if (!row.IsprggermanyNull()) totalPrggermany = totalPrggermany + row.prggermany;
                        if (!row.IsstoregermnNull()) totalStoregermn = totalStoregermn + row.storegermn;
                        if (!row.IsfreightgmnNull()) totalFreightgmn = totalFreightgmn + row.freightgmn;
                        if (!row.IspreparatgmNull()) totalPreparatgm = totalPreparatgm + row.preparatgm;
                        if (!row.Isprgmoscow0Null()) totalPrgmoscow = totalPrgmoscow + row.prgmoscow0;
                        if (!row.IsinsuranceNull()) totalInsurance = totalInsurance + row.insurance;
                        if (!row.IsdeliverymsNull()) totalDeliveryms = totalDeliveryms + row.deliveryms;
                        if (!row.IssertificatNull()) totalSertificat = totalSertificat + row.sertificat;
                        if (!row.Isclaim00000Null()) totalClaim = totalClaim + row.claim00000;
                        if (!row.IsescortmscwNull()) totalEscortmscw = totalEscortmscw + row.escortmscw;
                        if (!row.Isreturn0000Null()) totalReturn = totalReturn + row.return0000;
                        if (!row.IsCalcWeightNull()) totalWeight = totalWeight + row.CalcWeight;
                        if (!row.IsvolumeNull()) totalVolume = totalVolume + row.volume;
                        if (!row.IsgoodValueNull()) totalAmountur = totalAmountur + row.goodValue;
                        if (!row.IsamountusdNull()) totalAmountusd = totalAmountusd + row.amountusd;
                        if (!row.IsothersNull()) totalOthers = totalOthers + row.others;
                        if (!row.IsinvoicesumNull()) totalInvoicesum = totalInvoicesum + row.invoicesum;
                        if (!row.IspaysumNull()) totalPaysum = totalPaysum + row.paysum;
                    }
                }
            }
            TotalCustomsTextBox.Text = totalCustoms.ToString("N");
            TotalDeliveryTextBox.Text = totalDelivery.ToString("N");
            TotalDiscountTextBox.Text = totalDiscount.ToString("N");
            TotalPrggermanyTextBox.Text = totalPrggermany.ToString("N");
            TotalStoregermnTextBox.Text = totalStoregermn.ToString("N");
            TotalFreightgmnTextBox.Text = totalFreightgmn.ToString("N");
            TotalPreparatgmTextBox.Text = totalPreparatgm.ToString("N");
            TotalPrgmoscowTextBox.Text = totalPrgmoscow.ToString("N");
            TotalInsuranceTextBox.Text = totalInsurance.ToString("N");
            TotalDeliverymsTextBox.Text = totalDeliveryms.ToString("N");
            TotalSertificatTextBox.Text = totalSertificat.ToString("N");
            TotalClaimTextBox.Text = totalClaim.ToString("N");
            TotalEscortmscwTextBox.Text = totalEscortmscw.ToString("N");
            TotalReturnTextBox.Text = totalReturn.ToString("N");
            TotalWeightTextBox.Text = totalWeight.ToString("N");
            TotalVolumeTextBox.Text = totalVolume.ToString("N");
            TotalAmounteuTextBox.Text = totalAmountur.ToString("N");
            TotalAmountusdTextBox.Text = totalAmountusd.ToString("N");
            TotalOthersTextBox.Text = totalOthers.ToString("N");
            TotalInvoicesumTextBox.Text = totalInvoicesum.ToString("N0");
            TotalPaysumTextBox.Text = totalPaysum.ToString("N");
            TotalDebtsTextBox.Text = (totalInvoicesum - totalPaysum).ToString("N");
        }
        private void transactionDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //string col = e.Column.Header.ToString();
            //if (col == "Кол-во мест" | col == "Объем, м3" | col == "Вес по док, кг" | col == "Вес факт, кг" | col == "Стоимость товара, Е")
            if (e.Column.GetCellContent(e.Row) is TextBlock)
                decimal.TryParse((e.Column.GetCellContent(e.Row) as TextBlock).Text, out totalOldValue);
        }
        private void transactionDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!(e.Column.GetCellContent(e.Row) is TextBlock)) return;

            decimal newvalue = 0;
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                ParcelTransactionDS.tableParcelTransactionRow row = (e.Row.Item as DataRowView).Row as ParcelTransactionDS.tableParcelTransactionRow;
                switch (e.Column.Header.ToString())
                {
                    case "Таможенный":
                        if (!row.Iscustoms000Null()) newvalue = row.customs000; else newvalue = 0;
                        TotalCustomsTextBox.Text = (decimal.Parse(TotalCustomsTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Наценка":
                        if (!row.Isdiscount00Null()) newvalue = row.discount00; else newvalue = 0;
                        TotalDiscountTextBox.Text = (decimal.Parse(TotalDiscountTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Скидка":
                        if (!row.IsprggermanyNull()) newvalue = row.prggermany; else newvalue = 0;
                        TotalPrggermanyTextBox.Text = (decimal.Parse(TotalPrggermanyTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Хранение":
                        if (!row.IsstoregermnNull()) newvalue = row.storegermn; else newvalue = 0;
                        TotalStoregermnTextBox.Text = (decimal.Parse(TotalStoregermnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Фрахт":
                        if (!row.IsfreightgmnNull()) newvalue = row.freightgmn; else newvalue = 0;
                        TotalFreightgmnTextBox.Text = (decimal.Parse(TotalFreightgmnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Оформление":
                        if (!row.IspreparatgmNull()) newvalue = row.preparatgm; else newvalue = 0;
                        TotalPreparatgmTextBox.Text = (decimal.Parse(TotalPreparatgmTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Накладные":
                        if (!row.Isprgmoscow0Null()) newvalue = row.prgmoscow0; else newvalue = 0;
                        TotalPrgmoscowTextBox.Text = (decimal.Parse(TotalPrgmoscowTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Страховка":
                        if (!row.IsinsuranceNull()) newvalue = row.insurance; else newvalue = 0;
                        TotalInsuranceTextBox.Text = (decimal.Parse(TotalInsuranceTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Довоз":
                        if (!row.IsdeliverymsNull()) newvalue = row.deliveryms; else newvalue = 0;
                        TotalDeliverymsTextBox.Text = (decimal.Parse(TotalDeliverymsTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Серт-ты":
                        if (!row.IssertificatNull()) newvalue = row.sertificat; else newvalue = 0;
                        TotalSertificatTextBox.Text = (decimal.Parse(TotalSertificatTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Претензии":
                        if (!row.Isclaim00000Null()) newvalue = row.claim00000; else newvalue = 0;
                        TotalClaimTextBox.Text = (decimal.Parse(TotalClaimTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Корректировка":
                        if (!row.IsescortmscwNull()) newvalue = row.escortmscw; else newvalue = 0;
                        TotalEscortmscwTextBox.Text = (decimal.Parse(TotalEscortmscwTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Возврат":
                        if (!row.Isreturn0000Null()) newvalue = row.return0000; else newvalue = 0;
                        TotalReturnTextBox.Text = (decimal.Parse(TotalReturnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                }
            }
            else
            {
                if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                {
                    switch (e.Column.Header.ToString())
                    {
                        case "Таможенный":
                            TotalCustomsTextBox.Text = (decimal.Parse(TotalCustomsTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Наценка":
                            TotalDiscountTextBox.Text = (decimal.Parse(TotalDiscountTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Скидка":
                            TotalPrggermanyTextBox.Text = (decimal.Parse(TotalPrggermanyTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Хранение":
                            TotalStoregermnTextBox.Text = (decimal.Parse(TotalStoregermnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Фрахт":
                            TotalFreightgmnTextBox.Text = (decimal.Parse(TotalFreightgmnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Оформление":
                            TotalPreparatgmTextBox.Text = (decimal.Parse(TotalPreparatgmTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Накладные":
                            TotalPrgmoscowTextBox.Text = (decimal.Parse(TotalPrgmoscowTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Страховка":
                            TotalInsuranceTextBox.Text = (decimal.Parse(TotalInsuranceTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Довоз":
                            TotalDeliverymsTextBox.Text = (decimal.Parse(TotalDeliverymsTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Серт-ты":
                            TotalSertificatTextBox.Text = (decimal.Parse(TotalSertificatTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Претензии":
                            TotalClaimTextBox.Text = (decimal.Parse(TotalClaimTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Корректировка":
                            TotalEscortmscwTextBox.Text = (decimal.Parse(TotalEscortmscwTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Возврат":
                            TotalReturnTextBox.Text = (decimal.Parse(TotalReturnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                    }
                    TotalAmountusdTextBox.Text = (decimal.Parse(TotalAmountusdTextBox.Text) - totalOldValue + newvalue).ToString("N");
                }
            }
        }
        private void transactionDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Cancel) totalDataRefresh();
        }
        private void transactionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == transactionDataGrid) totalDataRefresh();
        }

        #endregion

        private void transactionDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            //DataGridCellInfo cellinf;
            //cellinf = ((sender as DataGrid).CurrentCell as DataGridCell).;
            //((sender as DataGrid).CurrentCell as DataGridCell).Tostring();
            //DependencyObject obj = (DependencyObject)(sender as DataGrid);
            //var child = System.Windows.Media.VisualTreeHelper.GetParent((sender as DataGrid) as DependencyObject);
        }

        private void ExcelReport()
        {
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            Excel.Workbook exWb;
            try
            {
                ParcelTransactionDS.tableParcelTransactionRow itemRow;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                Excel.Range r;
                exWh.Name = ParcelNumberList.Text;
                exWh.Cells[1, 1] = "№"; exWh.Cells[1, 2] = "Группа менеджеров"; exWh.Cells[1, 3] = "Клиент"; exWh.Cells[1, 4] = "Груз"; exWh.Cells[1, 5] = "Поставщик";
                exWh.Cells[1, 6] = "Кол-во мест"; exWh.Cells[1, 7] = "Вес по док, кг"; exWh.Cells[1, 8] = "Объем, м3"; exWh.Cells[1, 9] = "Вес факт, кг"; exWh.Cells[1, 10] = "Ст-ть, ЕU";
                exWh.Cells[1, 11] = "Ст-ть, кг"; exWh.Cells[1, 12] = "Сумма, $"; exWh.Cells[1, 13] = "Прочие, руб"; exWh.Cells[1, 14] = "Возврат"; exWh.Cells[1, 15] = "Счет, руб"; exWh.Cells[1, 16] = "Оплата, руб";
                exWh.Cells[1, 17] = "Таможенный"; exWh.Cells[1, 18] = "Доставка"; exWh.Cells[1, 19] = "Наценка"; exWh.Cells[1, 20] = "Скидка"; exWh.Cells[1, 21] = "Доп.услуги"; exWh.Cells[1, 22] = "Фрахт"; exWh.Cells[1, 23] = "Оформление";
                exWh.Cells[1, 24] = "Накладные"; exWh.Cells[1, 25] = "Страховка"; exWh.Cells[1, 26] = "Довоз"; exWh.Cells[1, 27] = "Серт-ты"; exWh.Cells[1, 28] = "Претензии"; exWh.Cells[1, 29] = "Корректировка";
                exWh.Cells[1, 30] = "Примечание менеджера";
                r = exWh.Range[exWh.Columns[6, Type.Missing], exWh.Columns[29, Type.Missing]]; r.NumberFormat = "#,##0.00";
                for (int i = 0; i < thisDS.tableParcelTransaction.Count; i++)
                {
                    itemRow = thisDS.tableParcelTransaction[i];
                    exWh.Cells[2 + i, 1] = itemRow.requestId;
                    if (!itemRow.IsmanagerGroupNull()) exWh.Cells[2 + i, 2] = itemRow.managerGroup;
                    exWh.Cells[2 + i, 3] = itemRow.customerName;
                    if (!itemRow.IsloadDescriptionNull()) exWh.Cells[2 + i, 4] = itemRow.loadDescription;
                    if (!itemRow.IsagentNameNull()) exWh.Cells[2 + i, 5] = itemRow.agentName;
                    if (!itemRow.IscellNumberNull()) exWh.Cells[2 + i, 6] = itemRow.cellNumber;
                    if (!itemRow.IsofficialWeightNull()) exWh.Cells[2 + i, 7] = itemRow.officialWeight;
                    if (!itemRow.IsvolumeNull()) exWh.Cells[2 + i, 8] = itemRow.volume;
                    if (!itemRow.IsactualWeightNull()) exWh.Cells[2 + i, 9] = itemRow.actualWeight;
                    if (!itemRow.IsgoodValueNull()) exWh.Cells[2 + i, 10] = itemRow.goodValue;
                    if (!itemRow.IscostkgNull()) exWh.Cells[2 + i, 11] = itemRow.costkg;
                    if (!itemRow.IsamountusdNull()) exWh.Cells[2 + i, 12] = itemRow.amountusd;
                    if (!itemRow.IsothersNull()) exWh.Cells[2 + i, 13] = itemRow.others;
                    if (!itemRow.Isreturn0000Null()) exWh.Cells[2 + i, 14] = itemRow.return0000;
                    if (!itemRow.IsinvoicesumNull()) exWh.Cells[2 + i, 15] = itemRow.invoicesum;
                    if (!itemRow.IspaysumNull()) exWh.Cells[2 + i, 16] = itemRow.paysum;
                    if (!itemRow.Iscustoms000Null()) exWh.Cells[2 + i, 17] = itemRow.customs000;
                    if (!itemRow.IsdeliveryCNull()) exWh.Cells[2 + i, 18] = itemRow.deliveryC;
                    if (!itemRow.Isdiscount00Null()) exWh.Cells[2 + i, 19] = itemRow.discount00;
                    if (!itemRow.IsprggermanyNull()) exWh.Cells[2 + i, 20] = itemRow.prggermany;
                    if (!itemRow.IsstoregermnNull()) exWh.Cells[2 + i, 21] = itemRow.storegermn;
                    if (!itemRow.IsfreightgmnNull()) exWh.Cells[2 + i, 22] = itemRow.freightgmn;
                    if (!itemRow.IspreparatgmNull()) exWh.Cells[2 + i, 23] = itemRow.preparatgm;
                    if (!itemRow.Isprgmoscow0Null()) exWh.Cells[2 + i, 24] = itemRow.prgmoscow0;
                    if (!itemRow.IsinsuranceNull()) exWh.Cells[2 + i, 25] = itemRow.insurance;
                    if (!itemRow.IsdeliverymsNull()) exWh.Cells[2 + i, 26] = itemRow.deliveryms;
                    if (!itemRow.IssertificatNull()) exWh.Cells[2 + i, 27] = itemRow.sertificat;
                    if (!itemRow.Isclaim00000Null()) exWh.Cells[2 + i, 28] = itemRow.claim00000;
                    if (!itemRow.IsescortmscwNull()) exWh.Cells[2 + i, 29] = itemRow.escortmscw;
                    if (!itemRow.IsmanagerNoteNull()) exWh.Cells[2 + i, 30] = itemRow.managerNote;
                }
                r = exWh.Range[exWh.Columns[1, Type.Missing], exWh.Columns[30, Type.Missing]]; r.Columns.AutoFit();
                exApp.Visible = true;
                exWh = null;
            }
            catch (Exception ex)
            {
                if (exApp != null)
                {
                    foreach (Excel.Workbook itemBook in exApp.Workbooks)
                    {
                        itemBook.Close(false);
                    }
                    exApp.Quit();
                }
                MessageBox.Show(ex.Message, "Создание заявки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        private void SetRowBackGround()
        {
            bool unchanged = false;
            KirillPolyanskiy.CustomBrokerWpf.AlternationBackground converter = this.mainGrid.FindResource("keyAlternationBackground") as AlternationBackground;
            BindingListCollectionView view = CollectionViewSource.GetDefaultView(transactionDataGrid.ItemsSource) as BindingListCollectionView;
            foreach (DataRowView rowview in view)
            {
                ParcelTransactionDS.tableParcelTransactionRow row = rowview.Row as ParcelTransactionDS.tableParcelTransactionRow;
                unchanged = (row.RowState == DataRowState.Unchanged);
                row.rowbackground = converter.Convert(row.customerID, typeof(int), null, System.Globalization.CultureInfo.CurrentCulture);
                row.EndEdit();
                if (unchanged) row.AcceptChanges();
            }
        }

        private void ColmarkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (transactionDataGrid.SelectedItems.Count > 0 & e.AddedItems.Count > 0)
            {
                ParcelTransactionDS.tableParcelTransactionRow row;
                foreach (DataRowView viewrow in transactionDataGrid.SelectedItems)
                {
                    if (viewrow != transactionDataGrid.CurrentItem)
                    {
                        row = viewrow.Row as ParcelTransactionDS.tableParcelTransactionRow;
                        row.colormark = (e.AddedItems[0] as System.Windows.Shapes.Rectangle).Fill.ToString();
                        row.EndEdit();
                    }
                }
            }
        }
    }
}
