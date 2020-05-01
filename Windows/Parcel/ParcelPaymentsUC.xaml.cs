using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;


namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for ParcelPaymentsUC.xaml
    /// </summary>
    public partial class ParcelPaymentsUC : UserControl, ISQLFiltredWindow
    {
        public ParcelPaymentsUC()
        {
            InitializeComponent();
            thisDS = new ParcelTransactionDS();
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
        private decimal totalOldValue = 0;
        ParcelTransactionDS thisDS;
        internal ParcelTransactionDS TransactionDS
        { get { return thisDS; } }
        private RequestDBM myrdbm;
        private RequestSynchronizer myrsync;
        ListCollectionView viewParcelRequest;

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void ParcelNumberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ParcelChanged();
        }
        private void toDocButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ParcelNumberList.SelectedItem is DataRowView)
                {
                    ParcelTransactionDS.tableParcelRow prow = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow;
                    string path = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName + "\\" + "Отправки\\" + prow.docdirpath;
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    System.Diagnostics.Process.Start(path);
                    //if (System.IO.Directory.Exists("E:\\Счета\\" + prow.docdirpath))
                    //{
                    //    System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
                    //}
                    //else if (System.IO.Directory.Exists("E:\\Счета\\" + prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5)))
                    //{
                    //    System.Diagnostics.Process.Start("E:\\Счета\\" + prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5));
                    //}
                    //else
                    //{
                    //    if (MessageBox.Show("Не удалось найти папку отправки: E:\\Счета\\" + prow.docdirpath + "\nСоздать папку?", "Папка документов", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    //    {
                    //        System.IO.Directory.CreateDirectory("E:\\Счета\\" + prow.docdirpath);
                    //        System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Папка документов");
            }
        }
        private void CustomsButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Проставить таможенный платеж?", "Таможенный платеж", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //foreach (ParcelTransactionDS.tableParcelTransactionRow trow in thisDS.tableParcelTransaction)
                //{
                //    if (!trow.IscustomspayNull() & trow.specloaded)
                //    {
                //        trow.customs000 = trow.customspay;
                //        trow.EndEdit();
                //    }
                //}
            }
        }
        private void InvoiceCreateButton_Click(object sender, RoutedEventArgs e)
        {
            //if (SaveChanges())
            //{
            //    using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            //    {
            //        try
            //        {
            //            con.Open();
            //            SqlCommand comm = new SqlCommand("dbo.ParcelTransactionInvoiceCreate_sp", con);
            //            comm.CommandType = CommandType.StoredProcedure;
            //            SqlParameter parcelid = new SqlParameter("@parcelid", ParcelNumberList.SelectedValue != null ? (int)ParcelNumberList.SelectedValue : 0);
            //            comm.Parameters.Add(parcelid);
            //            comm.ExecuteNonQuery();
            //            comm.Dispose();
            //            string curparcel = this.ParcelNumberList.Text;
            //            DataLoad();
            //            this.ParcelNumberList.Text = curparcel;
            //            con.Close();
            //            con.Dispose();
            //        }
            //        catch (Exception ex)
            //        {
            //            con.Close();
            //            con.Dispose();
            //            if (ex is System.Data.SqlClient.SqlException)
            //            {
            //                System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
            //                if (err.Number > 49999) MessageBox.Show(err.Message, "Расчет суммы счета", MessageBoxButton.OK, MessageBoxImage.Error);
            //                else
            //                {
            //                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
            //                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
            //                    {
            //                        errs.Append(sqlerr.Message + "\n");
            //                    }
            //                    MessageBox.Show(errs.ToString(), "Расчет суммы счета", MessageBoxButton.OK, MessageBoxImage.Error);
            //                }
            //            }
            //            else
            //            {
            //                MessageBox.Show(ex.Message + "\n" + ex.Source, "Расчет суммы счета", MessageBoxButton.OK, MessageBoxImage.Error);
            //            }
            //        }
            //    }
            //}
        }
        private void toExcelButton_Click(object sender, RoutedEventArgs e)
        {
            //ExcelReport();
        }
        private void AllInvoicePrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParcelNumberList.SelectedItem is DataRowView)
            {
                //ParcelTransactionDS.tableAccountBalanceRow[] balancerows = new ParcelTransactionDS.tableAccountBalanceRow[thisDS.tableAccountBalance.Rows.Count];
                //thisDS.tableAccountBalance.Rows.CopyTo(balancerows, 0);
                //CreateListInvoiceExcel((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow, balancerows, true, sender);
            }
        }
        private void AllInvoiceExcelButton_Click(object sender, RoutedEventArgs e)
        {
            //if (ParcelNumberList.SelectedItem is DataRowView)
            //{
            //    ParcelTransactionDS.tableAccountBalanceRow[] balancerows = new ParcelTransactionDS.tableAccountBalanceRow[thisDS.tableAccountBalance.Rows.Count];
            //    thisDS.tableAccountBalance.Rows.CopyTo(balancerows, 0);
            //    CreateListInvoiceExcel((ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow, balancerows, false, sender);
            //}
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                DataRefresh();
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
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Отменить несохраненные изменения в перевозке?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ParcelTransactionDS.tableParcelRow row = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelTransactionDS.tableParcelRow;
                row.RejectChanges();
                ParcelRequestDataGrid.CancelEdit(DataGridEditingUnit.Row);
                foreach (Classes.Domain.RequestVM tranrow in myrsync.ViewModelCollection)
                {
                    tranrow.RejectChanges();
                    //foreach (ParcelTransactionDS.tableOtherRow otherrow in tranrow.GettableOtherRows())
                    //    otherrow.RejectChanges();
                    //foreach (ParcelTransactionDS.tableReturnRow returnrow in tranrow.GettableReturnRows())
                    //    returnrow.RejectChanges();
                }
            }
        }
        private void ParcelDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGrid)?.CurrentItem is Classes.Domain.RequestVM)
            {
                if ((sender as DataGrid).CurrentCell.Column.SortMemberPath == "StorePointDate")
                {
                    RequestNewWin newWin = null;
                    DataGrid dg = sender as DataGrid;
                    foreach (Window item in this.OwnerWindow.OwnedWindows)
                    {
                        if (item.Name == "winRequestNew")
                        {
                            if ((item.DataContext as Classes.Domain.RequestVMCommand).VModel.Id == (dg.CurrentItem as Classes.Domain.RequestVM).Id)
                                newWin = item as RequestNewWin;
                        }
                    }
                    if (newWin == null)
                    {
                        newWin = new RequestNewWin();
                        newWin.Owner = this.myownerwin;

                        newWin.thisStoragePointValidationRule.RequestId = (dg.CurrentItem as Classes.Domain.RequestVM).Id;
                        Classes.Domain.RequestVMCommand cmd = new Classes.Domain.RequestVMCommand((dg.CurrentItem as Classes.Domain.RequestVM), viewParcelRequest);
                        newWin.DataContext = cmd;
                        newWin.Show();
                    }
                    else
                    {
                        newWin.Activate();
                        if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
                    }
                }
                //else if((sender as DataGrid).CurrentCell.Column.SortMemberPath == "StorePointDate")
                //{

                //}
                e.Handled = true;
            }
        }
        private void HistoryOpen_Click(object sender, RoutedEventArgs e)
        {
            RequestHistoryWin newHistory = new RequestHistoryWin();
            if ((sender as Button).Tag is RequestVM)
            {
                Request request = ((sender as Button).Tag as RequestVM).DomainObject;
                RequestHistoryViewCommand cmd = new RequestHistoryViewCommand(request);
                newHistory.DataContext = cmd;
            }
            newHistory.Owner = myownerwin;
            newHistory.Show();
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
        //private void setFilterButtonImage()
        //{
        //    string uribitmap;
        //    if (thisfilter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
        //    else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
        //    System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
        //    (FilterButton.Content as Image).Source = bi3;
        //}

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

        private System.Data.DataView mycustomers;
        public System.Data.DataView Customers
        {
            get
            {
                if (mycustomers == null)
                {
                    ReferenceDS refds = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
                    if (refds.tableCustomerName.Count == 0) refds.CustomerNameRefresh();
                    mycustomers = new System.Data.DataView(refds.tableCustomerName, string.Empty, "customerName", System.Data.DataViewRowState.CurrentRows);
                }
                return mycustomers;
            }
        }
        private ListCollectionView myagents;
        public ListCollectionView Agents
        {
            get
            {
                if (myagents == null)
                {
                    myagents = new ListCollectionView(CustomBrokerWpf.References.AgentNames);
                    myagents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myagents;
            }
        }
        private ListCollectionView myservicetypes;
        public ListCollectionView ServiceTypes
        {
            get
            {
                if (myservicetypes == null)
                {
                    myservicetypes = new ListCollectionView(CustomBrokerWpf.References.ServiceTypes);
                }
                return myservicetypes;
            }
        }

        private void DataLoad()
        {
            ReferenceDS referenceDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (referenceDS.tableRequestStatus.Count == 0)
            {
                ReferenceDSTableAdapters.RequestStatusAdapter adapterStatus = new ReferenceDSTableAdapters.RequestStatusAdapter();
                adapterStatus.Fill(referenceDS.tableRequestStatus);
            }
            statusComboBox.ItemsSource = new System.Data.DataView(referenceDS.tableRequestStatus, "rowId>49", "rowId", DataViewRowState.CurrentRows);
            myrdbm = new Classes.Domain.RequestDBM();
            myrdbm.Collection = new System.Collections.ObjectModel.ObservableCollection<Classes.Domain.Request>();
            myrsync = new Classes.Domain.RequestSynchronizer();
            myrsync.DomainCollection = myrdbm.Collection;
            viewParcelRequest = new ListCollectionView(myrsync.ViewModelCollection);
            viewParcelRequest.Filter = (object item) => { return (item as Classes.Domain.RequestVM).DomainObject.ParcelId.HasValue && lib.ViewModelViewCommand.ViewFilterDefault(item); };
            viewParcelRequest.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            viewParcelRequest.SortDescriptions.Add(new SortDescription("ParcelGroup", ListSortDirection.Ascending));
            viewParcelRequest.SortDescriptions.Add(new SortDescription("CustomerName", ListSortDirection.Ascending));
            ParcelTransactionDSTableAdapters.ParcelAdapter adapterParcel = new ParcelTransactionDSTableAdapters.ParcelAdapter();
            ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged; ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged; ParcelNumberList.SelectionChanged -= ParcelNumberList_SelectionChanged;
            this.mainGrid.DataContext = null;
            ParcelRequestDataGrid.ItemsSource = null;
            adapterParcel.Fill(thisDS.tableParcel, this.thisfilter.FilterWhereId);
            ParcelNumberList.SelectionChanged += ParcelNumberList_SelectionChanged;
            this.mainGrid.DataContext = new DataView(thisDS.tableParcel, string.Empty, "sortnumber DESC", DataViewRowState.CurrentRows);
            //setFilterButtonImage();
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
        private void ParcelChanged()
        {
            int parcelid = ParcelNumberList.SelectedValue != null ? (int)ParcelNumberList.SelectedValue : 0;
            if (parcelid != 0)
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
                        //if (row.IsdeliverypriceNull())
                        //{
                        //    com.CommandText = "dbo.Reference_sp";
                        //    SqlParameter refid = new SqlParameter("@refid", SqlDbType.Char);
                        //    SqlParameter refname = new SqlParameter("@refname", SqlDbType.NVarChar);
                        //    refname.Direction = ParameterDirection.Output;
                        //    refname.Size = 50;
                        //    SqlParameter refval = new SqlParameter("@refvalue", SqlDbType.NVarChar);
                        //    refval.Direction = ParameterDirection.Output;
                        //    refval.Size = 10;
                        //    com.Parameters.Add(refid);
                        //    com.Parameters.Add(refname);
                        //    com.Parameters.Add(refval);
                        //    refid.Value = "dlvcn";
                        //    com.ExecuteNonQuery();
                        //    if (decimal.TryParse(refval.Value.ToString(), out d))
                        //    {
                        //        row.deliveryprice = d;
                        //        row.EndEdit();
                        //    }
                        //}
                        //if (row.IsinsurancepriceNull())
                        //{
                        //    com.Parameters.Clear();
                        //    com.CommandText = "dbo.Reference_sp";
                        //    SqlParameter refid = new SqlParameter("@refid", SqlDbType.Char);
                        //    SqlParameter refname = new SqlParameter("@refname", SqlDbType.NVarChar);
                        //    refname.Direction = ParameterDirection.Output;
                        //    refname.Size = 50;
                        //    SqlParameter refval = new SqlParameter("@refvalue", SqlDbType.NVarChar);
                        //    refval.Direction = ParameterDirection.Output;
                        //    refval.Size = 10;
                        //    com.Parameters.Add(refid);
                        //    com.Parameters.Add(refname);
                        //    com.Parameters.Add(refval);
                        //    refid.Value = "inspr";
                        //    com.ExecuteNonQuery();
                        //    if (decimal.TryParse(refval.Value.ToString(), out d))
                        //    {
                        //        row.insuranceprice = d;
                        //        row.EndEdit();
                        //    }
                        //}
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

            this.ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            this.ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            if(!SaveChanges()) return;
            ParcelRequestDataGrid.ItemsSource = null;
            //balanceDataGrid.ItemsSource = null;
            //legalDataGrid.ItemsSource = null;
            //foreach (Window frwin in this.OwnerWindow.OwnedWindows)
            //{
            //    if (frwin.Name == "winParcelTransactionOther" | frwin.Name == "winParcelTransactionDetail")
            //    {
            //        frwin.Close();
            //    }
            //}
            if (parcelid != 0)
            {
                //ParcelTransactionDSTableAdapters.ParcelTransactionAdapter tranAdapter = new ParcelTransactionDSTableAdapters.ParcelTransactionAdapter();
                //ParcelTransactionDSTableAdapters.adapterOther adapterOther = new ParcelTransactionDSTableAdapters.adapterOther();
                //ParcelTransactionDSTableAdapters.ReturnAdapter adaptertReturn = new ParcelTransactionDSTableAdapters.ReturnAdapter();
                //ParcelTransactionDSTableAdapters.AccountBalanceAdapter balanceadapter = new ParcelTransactionDSTableAdapters.AccountBalanceAdapter();
                //ParcelTransactionDSTableAdapters.LegalAdapter legalaAdapter = new ParcelTransactionDSTableAdapters.LegalAdapter();
                //adapterOther.ClearBeforeFill = false;
                try
                {
                    //thisDS.tableOther.Clear();
                    //thisDS.tableReturn.Clear();
                    //tranAdapter.Fill(thisDS.tableParcelTransaction, parcelid);
                    //adapterOther.Fill(thisDS.tableOther, parcelid);
                    //adaptertReturn.Fill(thisDS.tableReturn, parcelid);
                    //balanceadapter.Fill(thisDS.tableAccountBalance, parcelid);
                    //legalaAdapter.Fill(thisDS.tableLegal, parcelid);
                    //thisDS.tableLegal.AddtableLegalRow(string.Empty, (decimal)thisDS.tableLegal.Compute("SUM(invoicesum)", string.Empty), (decimal)thisDS.tableLegal.Compute("SUM(takesum)", string.Empty), (decimal)thisDS.tableLegal.Compute("SUM(passum)", string.Empty), (decimal)thisDS.tableLegal.Compute("SUM(jsum)", string.Empty), (decimal)thisDS.tableLegal.Compute("SUM(costsum)", string.Empty));
                    //DataView viewrequest = thisDS.tableParcelTransaction.DefaultView;
                    //viewrequest.Sort = "customerName,requestId";
                    //(this.mainGrid.FindResource("keyAlternationBackground") as AlternationBackground).Reset();
                    myrdbm.Errors.Clear();
                    myrdbm.Parcel = parcelid;
                    myrdbm.Fill();
                    if (myrdbm.Errors.Count > 0) MessageBox.Show(myrdbm.ErrorMessage, "Загрузка платежей", MessageBoxButton.OK, MessageBoxImage.Error);
                    ParcelRequestDataGrid.ItemsSource = viewParcelRequest;
                    //SetRowBackGround();
                    //thisDS.tableAccountBalance.DefaultView.Sort = "customername";
                    //thisDS.tableLegal.DefaultView.Sort = "sortcolumn";
                    //balanceDataGrid.ItemsSource = thisDS.tableAccountBalance.DefaultView;
                    //legalDataGrid.ItemsSource = thisDS.tableLegal.DefaultView;
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
        public bool SaveRequestChanges()
        {
            bool isSuccess = true;
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            err.AppendLine("Изменения не сохранены");
            myrdbm.Errors.Clear();
            foreach (Classes.Domain.RequestVM item in viewParcelRequest)
            {
                if ((item.DomainState == lib.DomainObjectState.Added || item.DomainState == lib.DomainObjectState.Modified) && !item.Validate(true))
                {
                    err.AppendLine(item.Errors);
                    isSuccess = false;
                }
            }
            if (!myrdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                err.AppendLine(myrdbm.ErrorMessage);
            }
            if (!isSuccess)
                MessageBox.Show(err.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
            return isSuccess;
        }
        internal bool SaveChanges()
        {
            bool isSuccess = true;
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.mainGrid.DataContext) as BindingListCollectionView;
                IInputElement fcontrol = System.Windows.Input.FocusManager.GetFocusedElement(this.OwnerWindow);
                if (view.CurrentItem != null & fcontrol is TextBox)
                {
                    BindingExpression be;
                    be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                    if (be != null)
                    {
                        if (be.IsDirty) be.UpdateSource();
                        if (be.HasError) return false;
                    }
                }
                if (view.IsEditingItem) view.CommitEdit();
                this.ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                this.ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                ParcelTransactionDSTableAdapters.ParcelAdapter parcelAdapter = new ParcelTransactionDSTableAdapters.ParcelAdapter();
                //ParcelTransactionDSTableAdapters.ParcelTransactionAdapter tranAdapter = new ParcelTransactionDSTableAdapters.ParcelTransactionAdapter();
                //ParcelTransactionDSTableAdapters.adapterOther adapterOther = new ParcelTransactionDSTableAdapters.adapterOther();
                //ParcelTransactionDSTableAdapters.ReturnAdapter adapterReturn = new ParcelTransactionDSTableAdapters.ReturnAdapter();
                //thisDS.tableOther.SetStatus();
                //adapterOther.Update(thisDS.tableOther);
                //thisDS.tableReturn.SetStatus();
                //adapterReturn.Update(thisDS.tableReturn);
                //thisDS.tableParcelTransaction.SetStatus();
                //tranAdapter.Update(thisDS.tableParcelTransaction);
                isSuccess = SaveRequestChanges();
                parcelAdapter.Update(thisDS.tableParcel);
            }
            catch (Exception ex)
            {
                isSuccess = false;
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
            }
            return isSuccess;
        }

        #region Data Grid Total Sum
        private void totalDataRefresh()
        {
            decimal totalCustoms = 0M, totalDelivery = 0M, totalBroker = 0M, totalTD = 0M, totalTotalPay = 0M, totalFreightgmn = 0M, totalPreparatgm = 0M, totalBringPay = 0M, totalInsurance = 0M, totalCorrPay = 0M, totalSertificat = 0M, totalClaim = 0M, totalAdditional = 0M, totalReturn = 0M;
            decimal totalWeight = 0M, totalVolume = 0M, totalAmountur = 0M, totalInvoiceDiscount = 0M, totalOthers = 0M, totalInvoicesum = 0M, totalPaysum = 0M;
            if (this.ParcelRequestDataGrid.SelectedItems.Count > 1)
            {
                for (int i = 0; i < this.ParcelRequestDataGrid.SelectedItems.Count; i++)
                {
                    if (this.ParcelRequestDataGrid.SelectedItems[i] is Classes.Domain.RequestVM)
                    {
                        Classes.Domain.RequestVM row = this.ParcelRequestDataGrid.SelectedItems[i] as Classes.Domain.RequestVM;
                        if (row.CustomsPay.HasValue) totalCustoms = totalCustoms + row.CustomsPay.Value;
                        if (row.DeliveryPay.HasValue) totalDelivery = totalDelivery + row.DeliveryPay.Value;
                        if (row.BrokerPay.HasValue) totalBroker = totalBroker + row.BrokerPay.Value;
                        if (row.InsurancePay.HasValue) totalInsurance = totalInsurance + row.InsurancePay.Value;
                        if (row.TDPay.HasValue) totalTD = totalTD + row.TDPay.Value;
                        if (row.FreightPay.HasValue) totalFreightgmn = totalFreightgmn + row.FreightPay.Value;
                        if (row.PreparatnPay.HasValue) totalPreparatgm = totalPreparatgm + row.PreparatnPay.Value;
                        if (row.AdditionalPay.HasValue) totalAdditional = totalAdditional + row.AdditionalPay.Value;
                        if (row.BringPay.HasValue) totalBringPay = totalBringPay + row.BringPay.Value;
                        if (row.SertificatPay.HasValue) totalSertificat = totalSertificat + row.SertificatPay.Value;
                        //if (row.CorrPay.HasValue) totalCorrPay = totalCorrPay + row.CorrPay.Value;
                        if (row.TotalPay.HasValue) totalTotalPay = totalTotalPay + row.TotalPay.Value;
                        if (row.OfficialWeight.HasValue) totalWeight = totalWeight + row.OfficialWeight.Value;
                        if (row.Volume.HasValue) totalVolume = totalVolume + row.Volume.Value;
                        if (row.Invoice.HasValue) totalInvoicesum = totalInvoicesum + row.Invoice.Value;
                        if (row.InvoiceDiscount.HasValue) totalInvoiceDiscount = totalInvoiceDiscount + row.InvoiceDiscount.Value;
                    }
                }
            }
            else
            {
                foreach (object item in this.ParcelRequestDataGrid.Items)
                {
                    if (item is Classes.Domain.RequestVM)
                    {
                        Classes.Domain.RequestVM row = item as Classes.Domain.RequestVM;
                        if (row.CustomsPay.HasValue) totalCustoms = totalCustoms + row.CustomsPay.Value;
                        if (row.DeliveryPay.HasValue) totalDelivery = totalDelivery + row.DeliveryPay.Value;
                        if (row.BrokerPay.HasValue) totalBroker = totalBroker + row.BrokerPay.Value;
                        if (row.InsurancePay.HasValue) totalInsurance = totalInsurance + row.InsurancePay.Value;
                        if (row.TDPay.HasValue) totalTD = totalTD + row.TDPay.Value;
                        if (row.FreightPay.HasValue) totalFreightgmn = totalFreightgmn + row.FreightPay.Value;
                        if (row.PreparatnPay.HasValue) totalPreparatgm = totalPreparatgm + row.PreparatnPay.Value;
                        if (row.AdditionalPay.HasValue) totalAdditional = totalAdditional + row.AdditionalPay.Value;
                        if (row.BringPay.HasValue) totalBringPay = totalBringPay + row.BringPay.Value;
                        if (row.SertificatPay.HasValue) totalSertificat = totalSertificat + row.SertificatPay.Value;
                        //if (row.CorrPay.HasValue) totalCorrPay = totalCorrPay + row.CorrPay.Value;
                        if (row.TotalPay.HasValue) totalTotalPay = totalTotalPay + row.TotalPay.Value;
                        if (row.OfficialWeight.HasValue) totalWeight = totalWeight + row.OfficialWeight.Value;
                        if (row.Volume.HasValue) totalVolume = totalVolume + row.Volume.Value;
                        if (row.Invoice.HasValue) totalInvoicesum = totalInvoicesum + row.Invoice.Value;
                        if (row.InvoiceDiscount.HasValue) totalInvoiceDiscount = totalInvoiceDiscount + row.InvoiceDiscount.Value;
                    }
                }
            }
            TotalCustomsTextBox.Text = totalCustoms.ToString("N");
            TotalDeliveryTextBox.Text = totalDelivery.ToString("N");
            TotalEscortmscwTextBox.Text = totalBroker.ToString("N");
            TotalPrggermanyTextBox.Text = totalTD.ToString("N");
            TotalPaysumTextBox.Text = totalTotalPay.ToString("N");
            TotalFreightgmnTextBox.Text = totalFreightgmn.ToString("N");
            TotalPreparatgmTextBox.Text = totalPreparatgm.ToString("N");
            TotalDeliverymsTextBox.Text = totalBringPay.ToString("N");
            TotalInsuranceTextBox.Text = totalInsurance.ToString("N");
            TotalReturnTextBox.Text = totalCorrPay.ToString("N");
            TotalSertificatTextBox.Text = totalSertificat.ToString("N");
            //TotalClaimTextBox.Text = totalClaim.ToString("N");
            TotalStoregermnTextBox.Text = totalAdditional.ToString("N");
            TotalWeightTextBox.Text = totalWeight.ToString("N");
            TotalVolumeTextBox.Text = totalVolume.ToString("N");
            //TotalAmounteuTextBox.Text = totalAmountur.ToString("N");
            //TotalOthersTextBox.Text = totalOthers.ToString("N");
            TotalInvoicesumTextBox.Text = totalInvoicesum.ToString("N0");
            TotalAmountusdTextBox.Text = totalInvoiceDiscount.ToString("N");
            //TotalDebtsTextBox.Text = (totalInvoicesum - totalPaysum).ToString("N");
        }
        private void ParcelRequestDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = e.Row.Item != null && !(e.Row.Item as RequestVM).DomainObject.Blocking();
            if (e.Column.GetCellContent(e.Row) is TextBlock)
                decimal.TryParse((e.Column.GetCellContent(e.Row) as TextBlock).Text, out totalOldValue);
        }
        private void ParcelRequestDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!(e.Column.GetCellContent(e.Row) is TextBlock)) return;

            decimal newvalue = 0;
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                Classes.Domain.RequestVM row = e.Row.Item as Classes.Domain.RequestVM;
                switch (e.Column.SortMemberPath)
                {
                    case "CustomsPay":
                        if (row.CustomsPay.HasValue) newvalue = row.CustomsPay.Value; else newvalue = 0;
                        TotalCustomsTextBox.Text = (decimal.Parse(TotalCustomsTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "DeliveryPay":
                        if (row.DeliveryPay.HasValue) newvalue = row.DeliveryPay.Value; else newvalue = 0;
                        TotalDeliveryTextBox.Text = (decimal.Parse(TotalDeliveryTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "BrokerPay":
                        if (row.BrokerPay.HasValue) newvalue = row.BrokerPay.Value; else newvalue = 0;
                        TotalEscortmscwTextBox.Text = (decimal.Parse(TotalEscortmscwTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "InsurancePay":
                        if (row.InsurancePay.HasValue) newvalue = row.InsurancePay.Value; else newvalue = 0;
                        TotalInsuranceTextBox.Text = (decimal.Parse(TotalInsuranceTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "TDPay":
                        if (row.TDPay.HasValue) newvalue = row.TDPay.Value; else newvalue = 0;
                        TotalPrggermanyTextBox.Text = (decimal.Parse(TotalPrggermanyTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "FreightPay":
                        if (row.FreightPay.HasValue) newvalue = row.FreightPay.Value; else newvalue = 0;
                        TotalFreightgmnTextBox.Text = (decimal.Parse(TotalFreightgmnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "PreparatnPay":
                        if (row.PreparatnPay.HasValue) newvalue = row.PreparatnPay.Value; else newvalue = 0;
                        TotalPreparatgmTextBox.Text = (decimal.Parse(TotalPreparatgmTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "AdditionalPay":
                        if (row.AdditionalPay.HasValue) newvalue = row.AdditionalPay.Value; else newvalue = 0;
                        TotalStoregermnTextBox.Text = (decimal.Parse(TotalStoregermnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "BringPay":
                        if (row.BringPay.HasValue) newvalue = row.BringPay.Value; else newvalue = 0;
                        TotalDeliverymsTextBox.Text = (decimal.Parse(TotalDeliverymsTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "SertificatPay":
                        if (row.SertificatPay.HasValue) newvalue = row.SertificatPay.Value; else newvalue = 0;
                        TotalSertificatTextBox.Text = (decimal.Parse(TotalSertificatTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    //case "CorrPay":
                    //    if (row.CorrPay.HasValue) newvalue = row.CorrPay.Value; else newvalue = 0;
                    //    TotalReturnTextBox.Text = (decimal.Parse(TotalReturnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                    //    break;
                    case "OfficialWeight":
                        if (row.OfficialWeight.HasValue) newvalue = row.OfficialWeight.Value; else newvalue = 0;
                        TotalWeightTextBox.Text = (decimal.Parse(TotalWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Volume":
                        if (row.Volume.HasValue) newvalue = row.Volume.Value; else newvalue = 0;
                        TotalVolumeTextBox.Text = (decimal.Parse(TotalVolumeTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Invoice":
                        if (row.Invoice.HasValue) newvalue = row.Invoice.Value; else newvalue = 0;
                        TotalInvoicesumTextBox.Text = (decimal.Parse(TotalInvoicesumTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "InvoiceDiscount":
                        if (row.InvoiceDiscount.HasValue) newvalue = row.InvoiceDiscount.Value; else newvalue = 0;
                        TotalAmountusdTextBox.Text = (decimal.Parse(TotalAmountusdTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                }
            }
            else
            {
                if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                {
                    switch (e.Column.SortMemberPath)
                    {
                        case "CustomsPay":
                            TotalCustomsTextBox.Text = (decimal.Parse(TotalCustomsTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "DeliveryPay":
                            TotalDeliveryTextBox.Text = (decimal.Parse(TotalDeliveryTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "BrokerPay":
                            TotalEscortmscwTextBox.Text = (decimal.Parse(TotalEscortmscwTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "InsurancePay":
                            TotalInsuranceTextBox.Text = (decimal.Parse(TotalInsuranceTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "TDPay":
                            TotalPrggermanyTextBox.Text = (decimal.Parse(TotalPrggermanyTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "FreightPay":
                            TotalFreightgmnTextBox.Text = (decimal.Parse(TotalFreightgmnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "PreparatnPay":
                            TotalPreparatgmTextBox.Text = (decimal.Parse(TotalPreparatgmTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "AdditionalPay":
                            TotalStoregermnTextBox.Text = (decimal.Parse(TotalStoregermnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "BringPay":
                            TotalDeliverymsTextBox.Text = (decimal.Parse(TotalDeliverymsTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "SertificatPay":
                            TotalSertificatTextBox.Text = (decimal.Parse(TotalSertificatTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "CorrPay":
                            TotalReturnTextBox.Text = (decimal.Parse(TotalReturnTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "OfficialWeight":
                            TotalWeightTextBox.Text = (decimal.Parse(TotalWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Volume":
                            TotalVolumeTextBox.Text = (decimal.Parse(TotalVolumeTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "Invoice":
                            TotalInvoicesumTextBox.Text = (decimal.Parse(TotalInvoicesumTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                        case "InvoiceDiscount":
                            TotalAmountusdTextBox.Text = (decimal.Parse(TotalAmountusdTextBox.Text) - totalOldValue + newvalue).ToString("N");
                            break;
                    }
                    if (e.Column.SortMemberPath.IndexOf("Pay")>0)
                        TotalPaysumTextBox.Text = (decimal.Parse(TotalPaysumTextBox.Text) - totalOldValue + newvalue).ToString("N");
                }
            }
        }
        private void ParcelRequestDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if ((e.Row.Item as RequestVM).DomainState == lib.DomainObjectState.Unchanged) (e.Row.Item as RequestVM).DomainObject.UnBlocking();
            if (e.EditAction == DataGridEditAction.Cancel) totalDataRefresh();
        }
        private void ParcelRequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == ParcelRequestDataGrid) totalDataRefresh();
        }
        #endregion
    }
}
