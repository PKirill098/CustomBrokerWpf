using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Controls;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using Excel = Microsoft.Office.Interop.Excel;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class PrepayExpire : lib.ViewModelViewCommand
    {
        public PrepayExpire()
        {
            mymaindbm = new PrepayCustomerRequestDBM();
            mydbm = mymaindbm;
            mymaindbm.SetRequestDBM();
            mymaindbm.SelectCommandText = "account.SPDDateExpire_sp";
            mymaindbm.SelectParams = new System.Data.SqlClient.SqlParameter[0];
            mymaindbm.Collection = new System.Collections.ObjectModel.ObservableCollection<PrepayCustomerRequest>();
            mymaindbm.FillAsyncCompleted = () => {
                if (mymaindbm.Errors.Count > 0) OpenPopup(mymaindbm.ErrorMessage, true);
            };
            //mymaindbm.FillAsync();
            mysync = new PrepayCustomerRequestSynchronizer();
            mysync.DomainCollection = mymaindbm.Collection;
            this.Collection = mysync.ViewModelCollection;

            mymanagers = new ListCollectionView(CustomBrokerWpf.References.Managers);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
        }

        private PrepayCustomerRequestDBM mymaindbm;
        private PrepayCustomerRequestSynchronizer mysync;
        private System.Threading.Tasks.Task myrefreshtask;
        private System.Threading.Tasks.Task myloadtask;
        private System.Threading.CancellationTokenSource mycanceltasksource;
        private System.Threading.CancellationToken mycanceltasktoken;

        private ListCollectionView mymanagers;
        public ListCollectionView Managers
        { get { return mymanagers; } }

        internal System.Collections.ObjectModel.ObservableCollection<PrepayCustomerRequest> DomainCollection
        { get { return mysync.DomainCollection; } }

        private lib.TaskAsync.TaskAsync myexceltask;
        private RelayCommand myexcelexport;
        public ICommand ExcelExport
        {
            get { return myexcelexport; }
        }
        private void ExcelExportExec(object parametr)
        {
            this.myendedit();
            if (myexceltask == null)
                myexceltask = new lib.TaskAsync.TaskAsync();
            if (!myexceltask.IsBusy)
            {
                System.Windows.Controls.DataGrid source = parametr as System.Windows.Controls.DataGrid;
                libui.ExcelExportPopUpWindow win = new libui.ExcelExportPopUpWindow();
                win.SetProperty = (DataGridColumn column) =>
                {
                    string name = column.SortMemberPath.Substring(column.SortMemberPath.LastIndexOf('.') + 1);
                    if (name == "Name" | name == "InvoiceDate" | name == "Percent")
                        name = column.SortMemberPath.Substring(column.SortMemberPath.LastIndexOf('.', column.SortMemberPath.LastIndexOf('.') - 1) + 1).Replace(".", string.Empty);
                    return name;
                };
                win.SourceDataGrid = source;
                bool? ok = win.ShowDialog();
                if (ok.HasValue && ok.Value)
                {
                    int count;
                    System.Collections.IEnumerable items;
                    if (source.SelectedItems.Count > 1)
                    {
                        items = source.SelectedItems;
                        count = source.SelectedItems.Count;
                    }
                    else
                    {
                        items = myview;
                        count = myview.Count;
                    }
                    myexceltask.DoProcessing = OnExcelExport;
                    myexceltask.Run(new object[3] { win.Columns, items, count });
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool ExcelExportCanExec(object parametr)
        { return !(myview == null || myview.IsAddingNew | myview.IsEditingItem); }
        private KeyValuePair<bool, string> OnExcelExport(object args)
        {
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            myexceltask.ProgressChange(2);
            try
            {
                int row = 2, column = 1;
                exApp.SheetsInNewWorkbook = 1;
                Excel.Workbook exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                Excel.Range r;
                exWh.Name = "Поставки с истекающим сроком";

                int o;
                string d, m, y, s, dateformat;
                o = (int)exApp.International[Excel.XlApplicationInternational.xlDateOrder];
                s = exApp.International[Excel.XlApplicationInternational.xlDateSeparator];
                d = exApp.International[Excel.XlApplicationInternational.xlDayCode];
                if (exApp.International[Excel.XlApplicationInternational.xlDayLeadingZero])
                    d = d + d;
                m = exApp.International[Excel.XlApplicationInternational.xlMonthCode];
                if (exApp.International[Excel.XlApplicationInternational.xlMonthLeadingZero])
                    m = m + m;
                y = exApp.International[Excel.XlApplicationInternational.xlYearCode];
                y = y + y;
                if (exApp.International[Excel.XlApplicationInternational.xl4DigitYears])
                    y = y + y;
                dateformat = o == 0 ? string.Format("{2}{0}{1}{0}{3}", s, d, m, y) : (o == 1 ? string.Format("{1}{0}{2}{0}{3}", s, d, m, y) : string.Format("{3}{0}{2}{0}{1}", s, d, m, y));

                int maxrow = (int)(args as object[])[2] + 1;
                System.Collections.IEnumerable items = (args as object[])[1] as System.Collections.IEnumerable;
                libui.ColumnInfo[] columns = ((args as object[])[0] as libui.ColumnInfo[]);
                exWh.Rows[1, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                foreach (libui.ColumnInfo columninfo in columns)
                {
                    if (!string.IsNullOrEmpty(columninfo.Property))
                    {
                        exWh.Cells[1, column] = columninfo.Header;
                        switch (columninfo.Property)
                        {
                            case "Consolidate":
                            case nameof(Prepay.InvoiceNumber):
                            case nameof(Parcel.ParcelNumberOrder):
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                exWh.Columns[column, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                                break;
                            case "AgentName":
                            case "CustomerName":
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                break;
                            case "PrepayPercent":
                            case nameof(Prepay.RateDiffPer):
                            case "CustomsInvoicePercent":
                                exWh.Columns[column, Type.Missing].NumberFormat = "0%";
                                break;
                            case nameof(PrepayCustomerRequestVM.CurrencyPaySum):
                            case nameof(PrepayCustomerRequestVM.CustomerBalance):
                            case nameof(PrepayCustomerRequestVM.CustomsInvoiceRubSum):
                            case nameof(PrepayCustomerRequestVM.DTSum):
                            case nameof(PrepayCustomerRequestVM.EuroSum):
                            case nameof(PrepayCustomerRequestVM.FinalInvoiceCurSum):
                            case nameof(PrepayCustomerRequestVM.FinalInvoiceRubSum):
                            case nameof(PrepayCustomerRequestVM.FinalInvoiceRubSumPaid):
                            case nameof(PrepayCustomerRequestVM.OverPay):
                            case nameof(PrepayCustomerRequestVM.Refund):
                            case nameof(PrepayCustomerRequestVM.RubDiff):
                            case nameof(PrepayCustomerRequestVM.RubSum):
                            case nameof(PrepayCustomerRequestVM.Selling):
                                exWh.Columns[column, Type.Missing].NumberFormat = @"# ##0,00";
                                break;
                            case nameof(Prepay.CBRate):
                            case nameof(Prepay.CBRatep2p):
                            case nameof(Prepay.CurrencyBuyRate):
                                exWh.Columns[column, Type.Missing].NumberFormat = @"# ##0,0000";
                                break;
                            case "CustomsInvoiceInvoiceDate":
                            case nameof(CustomsInvoice.FinalRubPaidDate):
                            case nameof(PrepayCustomerRequestVM.SellingDate):
                            case nameof(CustomsInvoice.FinalCurPaidDate1):
                            case nameof(CustomsInvoice.FinalCurPaidDate2):
                                exWh.Columns[column, Type.Missing].NumberFormat = dateformat;
                                break;
                        }
                        column++;
                    }
                    else
                        break;
                }
                myexceltask.ProgressChange(2 + (int)(decimal.Divide(1, maxrow) * 100));

                foreach (PrepayCustomerRequestVM item in items.OfType<PrepayCustomerRequestVM>())
                {
                    column = 1;
                    foreach (libui.ColumnInfo columninfo in columns)
                    {
                        switch (columninfo.Property)
                        {
                            case "CustomerName":
                                exWh.Cells[row, column] = item.Prepay.Customer.Name;
                                break;
                            case "CustomsInvoicePercent":
                                exWh.Cells[row, column] = item.CustomsInvoice.Percent;
                                break;
                            case nameof(PrepayCustomerRequestVM.IsPrepay):
                                exWh.Cells[row, column] = item.IsPrepay ? "Пр" : string.Empty;
                                break;
                            case "PrepayPercent":
                                exWh.Cells[row, column] = item.Prepay.Percent;
                                break;
                            case nameof(Request.Consolidate):
                                exWh.Cells[row, column] = item.Request?.Consolidate;
                                break;
                            case nameof(item.RubSum):
                                exWh.Cells[row, column] = item.RubSum;
                                break;
                            case nameof(Prepay.InvoiceNumber):
                                exWh.Cells[row, column] = item.Prepay.InvoiceNumber;
                                break;
                            case "PrepayInvoiceDate":
                                exWh.Cells[row, column] = item.Prepay.InvoiceDate;
                                break;
                            case nameof(Prepay.CBRate):
                                exWh.Cells[row, column] = item.Prepay.CBRate;
                                break;
                            case nameof(Prepay.CBRatep2p):
                                exWh.Cells[row, column] = item.Prepay.CBRatep2p;
                                break;
                            case nameof(Prepay.RubPaidDate):
                                exWh.Cells[row, column] = item.Prepay.RubPaidDate;
                                break;
                            case "AgentName":
                                exWh.Cells[row, column] = item.Prepay.Agent.Name;
                                break;
                            case nameof(PrepayCustomerRequestVM.EuroSum):
                                exWh.Cells[row, column] = item.EuroSum;
                                break;
                            case nameof(Prepay.CurrencyBoughtDate):
                                exWh.Cells[row, column] = item.Prepay.CurrencyBoughtDate;
                                break;
                            case nameof(Prepay.CurrencyBuyRate):
                                exWh.Cells[row, column] = item.Prepay.CurrencyBuyRate;
                                break;
                            case nameof(PrepayCustomerRequestVM.CurrencyPaySum):
                                exWh.Cells[row, column] = item.CurrencyPaySum;
                                break;
                            case nameof(Prepay.CurrencyPaidDate):
                                exWh.Cells[row, column] = item.Prepay.CurrencyPaidDate;
                                break;
                            case nameof(PrepayCustomerRequestVM.DTSum):
                                exWh.Cells[row, column] = item.DTSum;
                                break;
                            case nameof(Specification.Declaration.SPDDate):
                                exWh.Cells[row, column] = item.Request?.Specification?.Declaration?.SPDDate;
                                break;
                            case nameof(Parcel.ParcelNumberOrder):
                                exWh.Cells[row, column] = item.Request.Parcel?.ParcelNumber;
                                break;
                            case nameof(PrepayCustomerRequestVM.CustomsInvoiceRubSum):
                                exWh.Cells[row, column] = item.CustomsInvoiceRubSum;
                                break;
                            case "CustomsInvoiceInvoiceDate":
                                exWh.Cells[row, column] = item.CustomsInvoice?.InvoiceDate;
                                break;
                            case nameof(CustomsInvoice.PaidDate):
                                exWh.Cells[row, column] = item.CustomsInvoice?.PaidDate;
                                break;
                            case nameof(PrepayCustomerRequestVM.Selling):
                                exWh.Cells[row, column] = item.Selling;
                                break;
                            case nameof(PrepayCustomerRequestVM.SellingDate):
                                exWh.Cells[row, column] = item.SellingDate;
                                break;
                            case nameof(PrepayCustomerRequestVM.FinalInvoiceRubSum):
                                exWh.Cells[row, column] = item.FinalInvoiceRubSum;
                                break;
                            case nameof(PrepayCustomerRequestVM.FinalInvoiceRubSumPaid):
                                exWh.Cells[row, column] = item.FinalInvoiceRubSumPaid;
                                break;
                            case nameof(PrepayCustomerRequestVM.FinalInvoiceCurSum):
                                exWh.Cells[row, column] = item.FinalInvoiceCurSum;
                                break;
                            case nameof(PrepayCustomerRequestVM.FinalInvoiceCurSum2):
                                exWh.Cells[row, column] = item.FinalInvoiceCurSum2;
                                break;
                            case nameof(CustomsInvoice.FinalCurPaidDate1):
                                exWh.Cells[row, column] = item.CustomsInvoice.FinalCurPaidDate1;
                                break;
                            case nameof(CustomsInvoice.FinalCurPaidDate2):
                                exWh.Cells[row, column] = item.CustomsInvoice.FinalCurPaidDate2;
                                break;
                            case nameof(CustomsInvoice.FinalRubPaidDate):
                                exWh.Cells[row, column] = item.CustomsInvoice?.FinalRubPaidDate;
                                break;
                            case nameof(PrepayCustomerRequestVM.CustomerBalance):
                                exWh.Cells[row, column] = item.CustomerBalance;
                                break;
                            case nameof(PrepayCustomerRequestVM.OverPay):
                                exWh.Cells[row, column] = item.OverPay;
                                break;
                            case nameof(Prepay.NotDealPassport):
                                exWh.Cells[row, column] = item.Prepay.NotDealPassport ? "без ПС" : string.Empty;
                                break;
                            case nameof(PrepayCustomerRequest.ExpiryDate):
                                exWh.Cells[row, column] = item.ExpiryDate;
                                break;
                            case nameof(PrepayCustomerRequest.ExpiryDaysLeft):
                                exWh.Cells[row, column] = item.ExpiryDaysLeft;
                                break;
                            case nameof(PrepayCustomerRequestVM.Refund):
                                exWh.Cells[row, column] = item.Refund;
                                break;
                            case nameof(Request.Manager):
                                exWh.Cells[row, column] = item.Request?.Manager?.NameComb;
                                break;
                            case nameof(Prepay.RateDiffPer):
                                exWh.Cells[row, column] = item.Prepay.RateDiffPer;
                                break;
                            case nameof(PrepayCustomerRequestVM.RubDiff):
                                exWh.Cells[row, column] = item.RubDiff;
                                break;
                            case nameof(Prepay.RateDiffResult):
                                exWh.Cells[row, column] = item.Prepay.RateDiffResult;
                                break;
                            case nameof(PrepayCustomerRequestVM.Note):
                                exWh.Cells[row, column] = item.Note;
                                break;
                            case nameof(PrepayCustomerRequestVM.Updated):
                                exWh.Cells[row, column] = item.Updated;
                                break;
                            case nameof(PrepayCustomerRequestVM.Updater):
                                exWh.Cells[row, column] = item.Updater;
                                break;
                        }
                        column++;
                    }
                    row++;
                    myexceltask.ProgressChange(2 + (int)(decimal.Divide(row, maxrow) * 100));
                }

                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, column - 1]];
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                r.VerticalAlignment = Excel.Constants.xlTop;
                r.WrapText = true;
                r = exWh.Range[exWh.Columns[1, Type.Missing], exWh.Columns[column - 1, Type.Missing]]; r.Columns.AutoFit();

                exWh = null;
                exApp.Visible = true;
                exApp.DisplayAlerts = true;
                exApp.ScreenUpdating = true;
                myexceltask.ProgressChange(100);
                return new KeyValuePair<bool, string>(false, "Данные выгружены. " + (row - 2).ToString() + " строк обработано.");
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
                throw new Exception(ex.Message);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }

        protected override void AddData(object parametr)
        {
        }
        protected override bool CanAddData(object parametr)
        {
            return false;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return false;
        }
        protected override bool CanRefreshData()
        {
            return myloadtask == null || !(myloadtask.Status == System.Threading.Tasks.TaskStatus.Running || myloadtask.Status == System.Threading.Tasks.TaskStatus.WaitingForActivation);
        }
        protected override bool CanRejectChanges()
        {
            return true;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void OtherViewRefresh()
        {
        }
        protected override void RefreshData(object parametr)
        {
            this.RefreshSuccessMessageHide = true;
            LoadAsyncStop();
            StringBuilder errstr = new StringBuilder();
            PrepayDBM prdbm = new PrepayDBM();
            RequestDBM rqdbm = new RequestDBM();
            RequestCustomerLegalDBM ldbm = new RequestCustomerLegalDBM();
            mycanceltasksource = new System.Threading.CancellationTokenSource();
            mycanceltasktoken = mycanceltasksource.Token;
            myrefreshtask = new Task(() => {
                rqdbm.SpecificationLoad = true;
                List<int> prepays = new List<int>();
                List<int> requests = new List<int>();
                List<int> invs = new List<int>();
                List<int> specs = new List<int>();
                foreach (PrepayCustomerRequest item in mymaindbm.Collection)
                {
                    if (mycanceltasktoken.IsCancellationRequested) return;
                    if (!prepays.Contains(item.Prepay.Id))
                    {
                        prdbm.ItemId = item.Prepay.Id;
                        CustomBrokerWpf.References.PrepayStore.UpdateItem(prdbm.GetFirst());
                        if (prdbm.Errors.Count > 0)
                            foreach (lib.DBMError err in prdbm.Errors) errstr.AppendLine(err.Message);
                        prepays.Add(item.Prepay.Id);
                    }
                    if (mycanceltasktoken.IsCancellationRequested) return;
                    if (!requests.Contains(item.Request.Id))
                    {
                        rqdbm.Command.Connection = prdbm.Command.Connection;
                        rqdbm.ItemId = item.Request.Id;
                        rqdbm.GetFirst();
                        if (mycanceltasktoken.IsCancellationRequested) return;
                        if (rqdbm.Errors.Count > 0)
                            foreach (lib.DBMError err in rqdbm.Errors) errstr.AppendLine(err.Message);
                        requests.Add(item.Request.Id);
                        if (mycanceltasktoken.IsCancellationRequested) return;
                    }
                    if (mycanceltasktoken.IsCancellationRequested) return;
                    if (item.CustomsInvoice != null && !invs.Contains(item.CustomsInvoice.Id))
                    {
                        CustomBrokerWpf.References.CustomsInvoiceStore.UpdateItem(item.CustomsInvoice.Id, rqdbm.Command.Connection, out var errors);
                        if (mycanceltasktoken.IsCancellationRequested) return;
                        if (errors.Count > 0) foreach (lib.DBMError err in errors) errstr.AppendLine(err.Message);
                        invs.Add(item.CustomsInvoice.Id);
                    }
                    if (mycanceltasktoken.IsCancellationRequested) return;
                }
            }, mycanceltasktoken);
            myloadtask = myrefreshtask.ContinueWith((task) => {
                if (myrefreshtask.IsCanceled) return;
                mymaindbm.Errors.Clear();
                if (mycanceltasktoken.IsCancellationRequested) return;
                mymaindbm.Fill();
                if (mycanceltasktoken.IsCancellationRequested) return;
                if (mymaindbm.Errors.Count > 0)
                    foreach (lib.DBMError err in mymaindbm.Errors) errstr.AppendLine(err.Message);
                if (errstr.Length > 0)
                    this.OpenPopup(errstr.ToString(), true);
                else
                    this.OpenPopup("Даннные обновлены", false);
            }, mycanceltasktoken);
            myrefreshtask.Start();
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(PrepayCustomerRequestVM.ExpiryDate),System.ComponentModel.ListSortDirection.Ascending));
        }
        public void LoadAsyncStop()
        {
            if (myrefreshtask != null && (myrefreshtask.Status == System.Threading.Tasks.TaskStatus.Running || myrefreshtask.Status == System.Threading.Tasks.TaskStatus.WaitingForActivation))
            {
                mycanceltasksource.Cancel();
                if (myrefreshtask.Status == System.Threading.Tasks.TaskStatus.Running)
                    myrefreshtask.Wait(500);
                mycanceltasksource.Dispose();
            }
            if (myloadtask != null && (myloadtask.Status == System.Threading.Tasks.TaskStatus.Running || myloadtask.Status == System.Threading.Tasks.TaskStatus.WaitingForActivation))
            {
                mymaindbm.LoadAsyncStop();
                mycanceltasksource.Cancel();
                if (myloadtask.Status == System.Threading.Tasks.TaskStatus.Running)
                    myloadtask.Wait(500);
                if (@myrefreshtask.IsCanceled) mycanceltasksource.Dispose();
            }
        }
    }
}
