using KirillPolyanskiy.CustomBrokerWpf.Classes.Specification;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class PaymentRegisterViewCommander : lib.ViewModelViewOnDemandCommand
    {
        internal PaymentRegisterViewCommander(Importer importer) : base()
        {
            mymaindbm = new PrepayCustomerRequestDBM();
            mymaindbm.SetRequestDBM();
            mydbm = mymaindbm;
            mymaindbm.Importer = importer;
            mymaindbm.FillAsyncCompleted = () => {
                if (mymaindbm.Errors.Count > 0) OpenPopup(mymaindbm.ErrorMessage, true);
                mytotal.StartCount();
            };
            mymaindbm.FillType = lib.FillType.Refresh;
            mysync = new PrepayCustomerRequestSynchronizer();

            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
            mysplitup = new RelayCommand(SplitUpExec, SplitUpCanExec);
            mytdload = new RelayCommand(TDLoadExec, TDLoadCanExec);
            mymanagers = new ListCollectionView(CustomBrokerWpf.References.Managers);

            #region Filter
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            myagentfilter = new PrepayAgentCheckListBoxVMFillDefault();
            myagentfilter.DeferredFill = true;
            myagentfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            myagentfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myagentfilter.ExecCommand2 = () => { myagentfilter.Clear(); };
            myagentfilter.FillDefault = () =>
            {
                bool empty = this.FilterEmpty;
                if (empty)
                    foreach (lib.ReferenceSimpleItem item in CustomBrokerWpf.References.AgentNames)
                        myagentfilter.Items.Add(item);
                return empty;
            };
            myconsolidatefilter = new PrepaConsolidateCheckListBoxVMFill();
            myconsolidatefilter.DeferredFill = true;
            myconsolidatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myconsolidatefilter.ExecCommand2 = () => { myconsolidatefilter.Clear(); };
            mycustomerfilter = new PrepayCustomerCheckListBoxVMFillDefault();
            mycustomerfilter.DeferredFill = true;
            mycustomerfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            mycustomerfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycustomerfilter.ExecCommand2 = () => { mycustomerfilter.Clear(); };
            mycustomerfilter.FillDefault = () =>
            {
                bool empty = this.FilterEmpty;
                if (empty)
                    foreach (CustomerLegal item in mycustomerfilter.DefaultList)
                        mycustomerfilter.Items.Add(item);
                return empty;
            };
            mydealpassportfilter = new WpfControlLibrary.CheckListBoxVM();
            mydealpassportfilter.RefreshIsVisible = false;
            mydealpassportfilter.AreaFilterIsVisible = false;
            mydealpassportfilter.Items = new List<string>(); mydealpassportfilter.Items.Add("с ПС"); mydealpassportfilter.Items.Add("без ПС");
            mydealpassportfilter.ItemsView.SortDescriptions.Clear();
            mydealpassportfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mydealpassportfilter.ExecCommand2 = () => { mydealpassportfilter.Clear(); };
            myinvoicenumberfilter = new PrepaInvoiceNumberCheckListBoxVMFill();
            myinvoicenumberfilter.DeferredFill = true;
            myinvoicenumberfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myinvoicenumberfilter.ExecCommand2 = () => { myinvoicenumberfilter.Clear(); };
            mymanagerfilter = new PrepayManagerCheckListBoxVMFillDefault();
            mymanagerfilter.DeferredFill = true;
            mymanagerfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mymanagerfilter.ExecCommand2 = () => { mymanagerfilter.Clear(); };
            mymanagerfilter.FillDefault = () =>
            {
                bool empty = this.FilterEmpty;
                if (empty)
                {
                    mymanagerfilter.Items.Add(new Manager(0, lib.DomainObjectState.Sealed, null, string.Empty, 208));
                    foreach (Manager item in CustomBrokerWpf.References.Managers)
                        mymanagerfilter.Items.Add(item);
                }
                return empty;
            };
            mynotefilter = new PrepaNoteCheckListBoxVMFill();
            mynotefilter.DeferredFill = true;
            mynotefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mynotefilter.ExecCommand2 = () => { mynotefilter.Clear(); };
            myparcelfilter = new PrepayParcelCheckListBoxVMFillDefault();
            myparcelfilter.DeferredFill = true;
            myparcelfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myparcelfilter.ExecCommand2 = () => { myparcelfilter.Clear(); };
            myparcelfilter.FillDefault = () =>
            {
                bool fempty = this.FilterEmpty;
                if (fempty)
                {
                    ParcelNumber empty = new ParcelNumber() { Sort = "999999" };
                    myparcelfilter.Items.Add(empty);
                    foreach (ParcelNumber item in CustomBrokerWpf.References.ParcelNumbers)
                        myparcelfilter.Items.Add(item);
                }
                return fempty;
            };
            mypercentfilter = new PrepayPercentCheckListBoxVMFill();
            mypercentfilter.DeferredFill = true;
            mypercentfilter.RefreshIsVisible = false;
            mypercentfilter.AreaFilterIsVisible = false;
            mypercentfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mypercentfilter.ExecCommand2 = () => { mypercentfilter.Clear(); };
            myprepayfilter = new WpfControlLibrary.CheckListBoxVM();
            myprepayfilter.RefreshIsVisible = false;
            myprepayfilter.AreaFilterIsVisible = false;
            myprepayfilter.Items=new List<string>(); myprepayfilter.Items.Add("Предоплата"); myprepayfilter.Items.Add("Нет");
            //myprepayfilter.DisplayPath = System.Windows.Data.Binding.DoNothing;
            //myprepayfilter.SearchPath = System.Windows.Data.Binding.DoNothing;
            //myprepayfilter.GetDisplayPropertyValueFunc = (item) => { return (string)item; };
            myprepayfilter.ItemsView.SortDescriptions.Clear();
            myprepayfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myprepayfilter.ExecCommand2 = () => { myprepayfilter.Clear(); };
            myratediffresultfilter = new WpfControlLibrary.CheckListBoxVM();
            myratediffresultfilter.RefreshIsVisible = false;
            myratediffresultfilter.AreaFilterIsVisible = false;
            myratediffresultfilter.Items = new List<string>(); myratediffresultfilter.Items.Add("Истина"); myratediffresultfilter.Items.Add("Ложь");
            myratediffresultfilter.ItemsView.SortDescriptions.Clear();
            myratediffresultfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myratediffresultfilter.ExecCommand2 = () => { myratediffresultfilter.Clear(); };

            mycurrencyboughtdatefilter = new libui.DateFilterVM();
            mycurrencyboughtdatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycurrencyboughtdatefilter.ExecCommand2 = () => { mycurrencyboughtdatefilter.Clear(); };
            mycurrencypaiddatefilter = new libui.DateFilterVM();
            mycurrencypaiddatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycurrencypaiddatefilter.ExecCommand2 = () => { mycurrencypaiddatefilter.Clear(); };
            myexpirydatefilter = new libui.DateFilterVM();
            myexpirydatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myexpirydatefilter.ExecCommand2 = () => { myexpirydatefilter.Clear(); };
            myfincur1paiddatefilter = new libui.DateFilterVM();
            myfincur1paiddatefilter.ExecCommand1 = () => { this.DateFilterRun(myfincur1paiddatefilter, "fincur1pdate"); RefreshData(null); };
            myfincur1paiddatefilter.ExecCommand2 = () => { myfincur1paiddatefilter.Clear(); };
            myfincur2paiddatefilter = new libui.DateFilterVM();
            myfincur2paiddatefilter.ExecCommand1 = () => { this.DateFilterRun(myfincur2paiddatefilter, "fincur2pdate"); RefreshData(null); };
            myfincur2paiddatefilter.ExecCommand2 = () => { myfincur2paiddatefilter.Clear(); };
            myfinalpaiddatefilter = new libui.DateFilterVM();
            myfinalpaiddatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myfinalpaiddatefilter.ExecCommand2 = () => { myfinalpaiddatefilter.Clear(); };
            mycustomsinvoicedatefilter = new libui.DateFilterVM();
            //mycustomsinvoicedatefilter.IsNull = true;
            //mycustomsinvoicedatefilter.DateStart = DateTime.Today.AddMonths(-4);
            mycustomsinvoicedatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycustomsinvoicedatefilter.ExecCommand2 = () => { mycustomsinvoicedatefilter.Clear(); };
            mycustomsinvoicepaiddatefilter = new libui.DateFilterVM();
            mycustomsinvoicepaiddatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycustomsinvoicepaiddatefilter.ExecCommand2 = () => { mycustomsinvoicepaiddatefilter.Clear(); };
            myinvoicedatefilter = new libui.DateFilterVM();
            myinvoicedatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myinvoicedatefilter.ExecCommand2 = () => { myinvoicedatefilter.Clear(); };
            myrubpaiddatefilter = new libui.DateFilterVM();
            myrubpaiddatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myrubpaiddatefilter.ExecCommand2 = () => { myrubpaiddatefilter.Clear(); };
            mysellingdatefilter = new libui.DateFilterVM();
            mysellingdatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mysellingdatefilter.ExecCommand2 = () => { mysellingdatefilter.Clear(); };
            myspddatefilter = new libui.DateFilterVM();
            myspddatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myspddatefilter.ExecCommand2 = () => { myspddatefilter.Clear(); };

            mycbratefilter = new libui.NumberFilterVM();
            mycbratefilter.ExecCommand1 = () => { NumberFilterRun(mycbratefilter, "cbrate"); RefreshData(null); };
            mycbratefilter.ExecCommand2 = () => { mycbratefilter.Clear(); };
            mycbratep2pfilter = new libui.NumberFilterVM();
            mycbratep2pfilter.ExecCommand1 = () => { NumberFilterRun(mycbratep2pfilter, "cbratep2p"); RefreshData(null); };
            mycbratep2pfilter.ExecCommand2 = () => { mycbratep2pfilter.Clear(); };
            mycustomerbalancefilter = new libui.NumberFilterVM();
            mycustomerbalancefilter.ExecCommand1 = () => { NumberFilterRun(mycustomerbalancefilter, "cbalance"); RefreshData(null); };
            mycustomerbalancefilter.ExecCommand2 = () => { mycustomerbalancefilter.Clear(); };
            mycustomsinvoicerubsumfilter = new libui.NumberFilterVM();
            mycustomsinvoicerubsumfilter.ExecCommand1 = () => { NumberFilterRun(mycustomsinvoicerubsumfilter, "custinvrubsum"); RefreshData(null); };
            mycustomsinvoicerubsumfilter.ExecCommand2 = () => { mycustomsinvoicerubsumfilter.Clear(); };
            mycustomsinvoicepercentfilter = new libui.NumberFilterVM();
            mycustomsinvoicepercentfilter.ExecCommand1 = () => { PercentFilterRun(mycustomsinvoicepercentfilter, "custinvpercent"); RefreshData(null); };
            mycustomsinvoicepercentfilter.ExecCommand2 = () => { mycustomsinvoicepercentfilter.Clear(); };
            mycurrencypayfilter = new libui.NumberFilterVM();
            mycurrencypayfilter.ExecCommand1 = () => { NumberFilterRun(mycurrencypayfilter, "curpaidsum"); RefreshData(null); };
            mycurrencypayfilter.ExecCommand2 = () => { mycurrencypayfilter.Clear(); };
            mycurrencybuyratefilter = new libui.NumberFilterVM();
            mycurrencybuyratefilter.ExecCommand1 = () => { NumberFilterRun(mycurrencybuyratefilter, "curbuyrate"); RefreshData(null); };
            mycurrencybuyratefilter.ExecCommand2 = () => { mycurrencybuyratefilter.Clear(); };
            mydtsumfilter = new libui.NumberFilterVM();
            mydtsumfilter.ExecCommand1 = () => { NumberFilterRun(mydtsumfilter, "dtsum"); RefreshData(null); };
            mydtsumfilter.ExecCommand2 = () => { mydtsumfilter.Clear(); };
            myeurosumfilter = new libui.NumberFilterVM();
            myeurosumfilter.ExecCommand1 = () => { NumberFilterRun(myeurosumfilter, "eurosum"); RefreshData(null); };
            myeurosumfilter.ExecCommand2 = () => { myeurosumfilter.Clear(); };
            myfincur1sumfilter = new libui.NumberFilterVM();
            myfincur1sumfilter.ExecCommand1 = () => { NumberFilterRun(myfincur1sumfilter, "fincur1sum"); RefreshData(null); };
            myfincur1sumfilter.ExecCommand2 = () => { myfincur1sumfilter.Clear(); };
            myfincur2sumfilter = new libui.NumberFilterVM();
            myfincur2sumfilter.ExecCommand1 = () => { NumberFilterRun(myfincur2sumfilter, "fincur2sum"); RefreshData(null); };
            myfincur2sumfilter.ExecCommand2 = () => { myfincur2sumfilter.Clear(); };
            myfinrubsumfilter = new libui.NumberFilterVM();
            myfinrubsumfilter.ExecCommand1 = () => { NumberFilterRun(myfinrubsumfilter, "fininvrubsum"); RefreshData(null); };
            myfinrubsumfilter.ExecCommand2 = () => { myfinrubsumfilter.Clear(); };
            myfinrubsumpaidfilter = new libui.NumberFilterVM();
            myfinrubsumpaidfilter.ExecCommand1 = () => { NumberFilterRun(myfinrubsumpaidfilter, "fininvrubpsum"); RefreshData(null); };
            myfinrubsumpaidfilter.ExecCommand2 = () => { myfinrubsumpaidfilter.Clear(); };
            myoverpayfilter = new libui.NumberFilterVM();
            myoverpayfilter.ExecCommand1 = () => { NumberFilterRun(myoverpayfilter, "overpay"); RefreshData(null); };
            myoverpayfilter.ExecCommand2 = () => { myoverpayfilter.Clear(); };
            myratediffperfilter = new libui.NumberFilterVM();
            myratediffperfilter.ExecCommand1 = () => { PercentFilterRun(myratediffperfilter, "ratediffper"); RefreshData(null); };
            myratediffperfilter.ExecCommand2 = () => { myratediffperfilter.Clear(); };
            myrefundfilter = new libui.NumberFilterVM();
            myrefundfilter.ExecCommand1 = () => { NumberFilterRun(myrefundfilter, "refund"); RefreshData(null); };
            myrefundfilter.ExecCommand2 = () => { myrefundfilter.Clear(); };
            myrubdifffilter = new libui.NumberFilterVM();
            myrubdifffilter.ExecCommand1 = () => { NumberFilterRun(myrubdifffilter, "rubdiff"); RefreshData(null); };
            myrubdifffilter.ExecCommand2 = () => { myrubdifffilter.Clear(); };
            myrubsumfilter = new libui.NumberFilterVM();
            myrubsumfilter.ExecCommand1 = () => { NumberFilterRun(myrubsumfilter, "rubsum"); RefreshData(null); };
            myrubsumfilter.ExecCommand2 = () => { myrubsumfilter.Clear(); };
            mysellingfilter = new libui.NumberFilterVM();
            mysellingfilter.ExecCommand1 = () => { NumberFilterRun(mysellingfilter, "sellingsum"); RefreshData(null); };
            mysellingfilter.ExecCommand2 = () => { mysellingfilter.Clear(); };

            #endregion
        }

        private PrepayCustomerRequestDBM mymaindbm;
        private PrepayCustomerRequestSynchronizer mysync;
        private System.Threading.Tasks.Task myrefreshtask;
        private System.Threading.Tasks.Task myloadtask;
        private System.Threading.CancellationTokenSource mycanceltasksource;
        private System.Threading.CancellationToken mycanceltasktoken;
        #region Filter
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { get { return myfilter; } }
        private int myparcelfiltergroup;
        private int myconsolidatefiltergroup;
        private int myinvoicenumberfiltergroup;
        private PrepayAgentCheckListBoxVMFillDefault myagentfilter;
        public PrepayAgentCheckListBoxVMFillDefault AgentFilter
        {
            get { return myagentfilter; }
        }
        private libui.NumberFilterVM mycbratefilter;
        public libui.NumberFilterVM CBRateFilter
        { get { return mycbratefilter; } }
        private libui.NumberFilterVM mycbratep2pfilter;
        public libui.NumberFilterVM CBRatep2pFilter
        { get { return mycbratep2pfilter; } }
        private PrepaConsolidateCheckListBoxVMFill myconsolidatefilter;
        public PrepaConsolidateCheckListBoxVMFill ConsolidateFilter
        { get { return myconsolidatefilter; } }
        private libui.NumberFilterVM mycurrencypayfilter;
        public libui.NumberFilterVM CurrencyPayFilter
        { get { return mycurrencypayfilter; } }
        private libui.DateFilterVM mycurrencyboughtdatefilter;
        public libui.DateFilterVM CurrencyBoughtDateFilter
        { get { return mycurrencyboughtdatefilter; } }
        private libui.NumberFilterVM mycurrencybuyratefilter;
        public libui.NumberFilterVM CurrencyBuyRateFilter
        { get { return mycurrencybuyratefilter; } }
        private libui.DateFilterVM mycurrencypaiddatefilter;
        public libui.DateFilterVM CurrencyPaidDateFilter
        { get { return mycurrencypaiddatefilter; } }
        private PrepayCustomerCheckListBoxVMFillDefault mycustomerfilter;
        public PrepayCustomerCheckListBoxVMFillDefault CustomerFilter
        {
            get { return mycustomerfilter; }
        }
        private libui.NumberFilterVM mycustomerbalancefilter;
        public libui.NumberFilterVM CustomerBalanceFilter
        { get { return mycustomerbalancefilter; } }
        private libui.DateFilterVM mycustomsinvoicedatefilter;
        public libui.DateFilterVM CustomsInvoiceDateFilter
        { get { return mycustomsinvoicedatefilter; } }
        private libui.DateFilterVM mycustomsinvoicepaiddatefilter;
        public libui.DateFilterVM CustomsInvoicePaidDateFilter
        { get { return mycustomsinvoicepaiddatefilter; } }
        private libui.NumberFilterVM mycustomsinvoicerubsumfilter;
        public libui.NumberFilterVM CustomsInvoiceRubSumFilter
        { get { return mycustomsinvoicerubsumfilter; } }
        private libui.NumberFilterVM mycustomsinvoicepercentfilter;
        public libui.NumberFilterVM CustomsInvoicePercentFilter
        { get { return mycustomsinvoicepercentfilter; } }
        private libui.CheckListBoxVM mydealpassportfilter;
        public libui.CheckListBoxVM DealPassportFilter
        {
            get { return mydealpassportfilter; }
        }
        private libui.NumberFilterVM mydtsumfilter;
        public libui.NumberFilterVM DTSumFilter
        { get { return mydtsumfilter; } }
        private libui.NumberFilterVM myeurosumfilter;
        public libui.NumberFilterVM EuroSumFilter
        { get { return myeurosumfilter; } }
        private libui.DateFilterVM myexpirydatefilter;
        public libui.DateFilterVM ExpiryDateFilter
        { get { return myexpirydatefilter; } }
        private libui.DateFilterVM myfincur1paiddatefilter;
        public libui.DateFilterVM FinalCur1PaidDateFilter
        { get { return myfincur1paiddatefilter; } }
        private libui.NumberFilterVM myfincur1sumfilter;
        public libui.NumberFilterVM FinalCur1SumFilter
        { get { return myfincur1sumfilter; } }
        private libui.DateFilterVM myfincur2paiddatefilter;
        public libui.DateFilterVM FinalCur2PaidDateFilter
        { get { return myfincur2paiddatefilter; } }
        private libui.NumberFilterVM myfincur2sumfilter;
        public libui.NumberFilterVM FinalCur2SumFilter
        { get { return myfincur2sumfilter; } }
        private libui.DateFilterVM myfinalpaiddatefilter;
        public libui.DateFilterVM FinalPaidDateFilter
        { get { return myfinalpaiddatefilter; } }
        private libui.NumberFilterVM myfinrubsumfilter;
        public libui.NumberFilterVM FinalRubSumFilter
        { get { return myfinrubsumfilter; } }
        private libui.NumberFilterVM myfinrubsumpaidfilter;
        public libui.NumberFilterVM FinalRubSumPaidFilter
        { get { return myfinrubsumpaidfilter; } }
        private libui.DateFilterVM myinvoicedatefilter;
        public libui.DateFilterVM InvoiceDateFilter
        { get { return myinvoicedatefilter; } }
        private PrepaInvoiceNumberCheckListBoxVMFill myinvoicenumberfilter;
        public PrepaInvoiceNumberCheckListBoxVMFill InvoiceNumberFilter
        { get { return myinvoicenumberfilter; } }
        private PrepayManagerCheckListBoxVMFillDefault mymanagerfilter;
        public PrepayManagerCheckListBoxVMFillDefault ManagerFilter
        {
            get { return mymanagerfilter; }
        }
        private PrepaNoteCheckListBoxVMFill mynotefilter;
        public PrepaNoteCheckListBoxVMFill NoteFilter
        { get { return mynotefilter; } }
        private libui.NumberFilterVM myoverpayfilter;
        public libui.NumberFilterVM OverPayFilter
        { get { return myoverpayfilter; } }
        private PrepayParcelCheckListBoxVMFillDefault myparcelfilter;
        public PrepayParcelCheckListBoxVMFillDefault ParcelFilter
        {
            get { return myparcelfilter; }
        }
        private PrepayPercentCheckListBoxVMFill mypercentfilter;
        public PrepayPercentCheckListBoxVMFill PercentFilter
        {
            get { return mypercentfilter; }
        }
        private libui.CheckListBoxVM myprepayfilter;
        public libui.CheckListBoxVM PrepayFilter
        {
            get { return myprepayfilter;}
        }
        private libui.NumberFilterVM myratediffperfilter;
        public libui.NumberFilterVM RateDiffPerFilter
        { get { return myratediffperfilter; } }
        private libui.CheckListBoxVM myratediffresultfilter;
        public libui.CheckListBoxVM RateDiffResultFilter
        {
            get { return myratediffresultfilter; }
        }
        private libui.NumberFilterVM myrefundfilter;
        public libui.NumberFilterVM RefundFilter
        { get { return myrefundfilter; } }
        private libui.NumberFilterVM myrubdifffilter;
        public libui.NumberFilterVM RubDiffFilter
        { get { return myrubdifffilter; } }
        private libui.DateFilterVM myrubpaiddatefilter;
        public libui.DateFilterVM RubPaidDateFilter
        { get { return myrubpaiddatefilter; } }
        private libui.NumberFilterVM myrubsumfilter;
        public libui.NumberFilterVM RubSumFilter
        { get { return myrubsumfilter; } }
        private libui.NumberFilterVM mysellingfilter;
        public libui.NumberFilterVM SellingFilter
        { get { return mysellingfilter; } }
        private libui.DateFilterVM mysellingdatefilter;
        public libui.DateFilterVM SellingDateFilter
        { get { return mysellingdatefilter; } }
        private libui.DateFilterVM myspddatefilter;
        public libui.DateFilterVM SPDDateFilter
        { get { return myspddatefilter; } }

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            this.EndEdit();
            if (myagentfilter.FilterOn)
            {
                string[] items = new string[myagentfilter.SelectedItems.Count];
                for (int i = 0; i < myagentfilter.SelectedItems.Count; i++)
                    items[i] = (myagentfilter.SelectedItems[i] as lib.ReferenceSimpleItem).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "agent", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "agent", new string[0]);
            //if(!mycbratefilter.IsNotNull)
            //    myfilter.ConditionAdd(myfilter.FilterWhereId, "cbrate", "IS NULL");
            //else if (mycbratefilter.IsRange)
            //    myfilter.SetRange(myfilter.FilterWhereId, "cbrate", mycbratefilter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture), mycbratefilter.NumberStop?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            //else
            //    myfilter.SetNumber(myfilter.FilterWhereId, "cbrate", mycbratefilter.Operator, mycbratefilter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            //if(!mycbratep2pfilter.IsNotNull)
            //    myfilter.ConditionAdd(myfilter.FilterWhereId, "cbratep2p", "IS NULL");
            //else if (mycbratep2pfilter.IsRange)
            //    myfilter.SetRange(myfilter.FilterWhereId, "cbratep2p", mycbratep2pfilter.NumberStart?.ToString("F4", System.Globalization.CultureInfo.InvariantCulture), mycbratep2pfilter.NumberStop?.ToString("F4", System.Globalization.CultureInfo.InvariantCulture));
            //else
            //    myfilter.SetNumber(myfilter.FilterWhereId, "cbratep2p", mycbratep2pfilter.Operator, mycbratep2pfilter.NumberStart?.ToString("F4",System.Globalization.CultureInfo.InvariantCulture));
            if (myconsolidatefilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items = new string[myconsolidatefilter.SelectedItems.Count];
                for (int i = 0; i < myconsolidatefilter.SelectedItems.Count; i++)
                {
                    items[i] = (string)myconsolidatefilter.SelectedItems[i];
                    if (items[i] == string.Empty)
                        isNullOrEmpty = true;
                }
                myfilter.SetList(myconsolidatefiltergroup, "consolidate", items, isNullOrEmpty);
            }
            else
                foreach (lib.SQLFilter.SQLFilterCondition cond in myfilter.ConditionGet(myconsolidatefiltergroup, "consolidate"))
                    myfilter.ConditionDel(cond.propertyid);
            myfilter.SetDate(myfilter.FilterWhereId, "curboughtdate", "curboughtdate", mycurrencyboughtdatefilter.DateStart, mycurrencyboughtdatefilter.DateStop, mycurrencyboughtdatefilter.IsNull);
            //if (!mycurrencybuyratefilter.IsNotNull)
            //    myfilter.ConditionAdd(myfilter.FilterWhereId, "curbuyrate", "IS NULL");
            //else if (mycurrencybuyratefilter.IsRange)
            //    myfilter.SetRange(myfilter.FilterWhereId, "curbuyrate", mycurrencybuyratefilter.NumberStart?.ToString("F4", System.Globalization.CultureInfo.InvariantCulture), mycurrencybuyratefilter.NumberStop?.ToString("F4", System.Globalization.CultureInfo.InvariantCulture));
            //else
            //    myfilter.SetNumber(myfilter.FilterWhereId, "curbuyrate", mycurrencybuyratefilter.Operator, mycurrencybuyratefilter.NumberStart?.ToString("F4", System.Globalization.CultureInfo.InvariantCulture));
            myfilter.SetDate(myfilter.FilterWhereId, "curpaiddate", "curpaiddate", mycurrencypaiddatefilter.DateStart, mycurrencypaiddatefilter.DateStop, mycurrencypaiddatefilter.IsNull);
            if (mycustomerfilter.FilterOn)
            {
                string[] items = new string[mycustomerfilter.SelectedItems.Count];
                for (int i = 0; i < mycustomerfilter.SelectedItems.Count; i++)
                    items[i] = (mycustomerfilter.SelectedItems[i] as CustomerLegal).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "customer", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "customer", new string[0]);
            myfilter.SetDate(myfilter.FilterWhereId, "custinvdate", "custinvdate", mycustomsinvoicedatefilter.DateStart, mycustomsinvoicedatefilter.DateStop, mycustomsinvoicedatefilter.IsNull);
            myfilter.SetDate(myfilter.FilterWhereId, "custinvpdate", "custinvpdate", mycustomsinvoicepaiddatefilter.DateStart, mycustomsinvoicepaiddatefilter.DateStop, mycustomsinvoicepaiddatefilter.IsNull);
            if (mydealpassportfilter.FilterOn)
            {
                if (mydealpassportfilter.SelectedItems[0] == mydealpassportfilter.Items[0])
                    myfilter.SetNumber(myfilter.FilterWhereId, "dealpass", lib.SQLFilter.Operators.Equal, "1");
                else
                    myfilter.SetNumber(myfilter.FilterWhereId, "dealpass", lib.SQLFilter.Operators.Equal, "0");
            }
            else
                myfilter.SetNumber(myfilter.FilterWhereId, "dealpass", lib.SQLFilter.Operators.Equal, string.Empty);
            //if (!myeurosumfilter.IsNotNull)
            //    myfilter.ConditionAdd(myfilter.FilterWhereId, "eurosum", "IS NULL");
            //else if (myeurosumfilter.IsRange)
            //    myfilter.SetRange(myfilter.FilterWhereId, "eurosum", myeurosumfilter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture), myeurosumfilter.NumberStop?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            //else
            //    myfilter.SetNumber(myfilter.FilterWhereId, "eurosum", myeurosumfilter.Operator, myeurosumfilter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            myfilter.SetDate(myfilter.FilterWhereId, "expirydate", "expirydate", myexpirydatefilter.DateStart?.AddDays(-240), myexpirydatefilter.DateStop?.AddDays(-240), myexpirydatefilter.IsNull);
            myfilter.SetDate(myfilter.FilterWhereId, "finrubpdate", "finrubpdate", myfinalpaiddatefilter.DateStart, myfinalpaiddatefilter.DateStop, myfinalpaiddatefilter.IsNull);
            myfilter.SetDate(myfilter.FilterWhereId, "invoicedate", "invoicedate", myinvoicedatefilter.DateStart, myinvoicedatefilter.DateStop, myinvoicedatefilter.IsNull);
            if (myinvoicenumberfilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items;
                if (myinvoicenumberfilter.Items.Count > 0)
                {
                    items = new string[myinvoicenumberfilter.SelectedItems.Count];
                    for (int i = 0; i < myinvoicenumberfilter.SelectedItems.Count; i++)
                    {
                        items[i] = (string)myinvoicenumberfilter.SelectedItems[i];
                        if (items[i] == string.Empty)
                            isNullOrEmpty = true;
                    }
                }
                else
                    items = new string[] { myinvoicenumberfilter.ItemsViewFilter };
                myfilter.SetList(myinvoicenumberfiltergroup, "invoicenumber", items, isNullOrEmpty);
            }
            else
                foreach (lib.SQLFilter.SQLFilterCondition cond in myfilter.ConditionGet(myinvoicenumberfiltergroup, "invoicenumber"))
                    myfilter.ConditionDel(cond.propertyid);
            if (mymanagerfilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items = new string[mymanagerfilter.SelectedItems.Count];
                for (int i = 0; i < mymanagerfilter.SelectedItems.Count; i++)
                {
                    items[i] = (mymanagerfilter.SelectedItems[i] as Manager).Id.ToString();
                    if (items[i] == "0")
                        isNullOrEmpty = true;
                }
                myfilter.SetList(myfilter.FilterWhereId, "manager", items, isNullOrEmpty);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "manager", new string[0]);
            if (mynotefilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items = new string[mynotefilter.SelectedItems.Count];
                for (int i = 0; i < mynotefilter.SelectedItems.Count; i++)
                {
                    items[i] = mynotefilter.SelectedItems[i] as string;
                    if (items[i] == string.Empty)
                        isNullOrEmpty = true;
                }
                myfilter.SetList(myfilter.FilterWhereId, "note", items, isNullOrEmpty);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "note", new string[0]);
            if (myparcelfilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items = new string[myparcelfilter.SelectedItems.Count];
                for (int i = 0; i < myparcelfilter.SelectedItems.Count; i++)
                {
                    items[i] = (myparcelfilter.SelectedItems[i] as ParcelNumber).Id.ToString();
                    if ((myparcelfilter.SelectedItems[i] as ParcelNumber).Id == 0)
                        isNullOrEmpty = true;
                }
                myfilter.SetList(myparcelfiltergroup, "parcel", items, isNullOrEmpty);
            }
            else
                foreach (lib.SQLFilter.SQLFilterCondition cond in myfilter.ConditionGet(myparcelfiltergroup, "parcel"))
                    myfilter.ConditionDel(cond.propertyid);
            if (mypercentfilter.FilterOn)
            {
                string[] items = new string[mypercentfilter.SelectedItems.Count];
                for (int i = 0; i < mypercentfilter.SelectedItems.Count; i++)
                    items[i] = (decimal.Parse((string)mypercentfilter.SelectedItems[i])/100M).ToString(System.Globalization.CultureInfo.InvariantCulture);
                myfilter.SetList(myfilter.FilterWhereId, "percent", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "percent", new string[0]);
            if (myprepayfilter.FilterOn)
            { 
                if(myprepayfilter.SelectedItems[0] == myprepayfilter.Items[0])
                    myfilter.SetNumber(myfilter.FilterWhereId, "prepay", lib.SQLFilter.Operators.Equal, "0");
                else
                    myfilter.SetNumber(myfilter.FilterWhereId, "prepay", lib.SQLFilter.Operators.NotEqual, "0");
            }
            else
                myfilter.SetNumber(myfilter.FilterWhereId, "prepay", lib.SQLFilter.Operators.Equal, string.Empty);
            if (myratediffresultfilter.FilterOn)
            {
                if (myratediffresultfilter.SelectedItems[0] == myratediffresultfilter.Items[0])
                    myfilter.SetNumber(myfilter.FilterWhereId, "ratediffper", lib.SQLFilter.Operators.Less, "0.00501");
                else
                    myfilter.SetNumber(myfilter.FilterWhereId, "ratediffper", lib.SQLFilter.Operators.Greater, "0.00500");
            }
            else
                myfilter.SetNumber(myfilter.FilterWhereId, "ratediffper", lib.SQLFilter.Operators.Equal, string.Empty);
            myfilter.SetDate(myfilter.FilterWhereId, "rubpaiddate", "rubpaiddate", myrubpaiddatefilter.DateStart, myrubpaiddatefilter.DateStop, myrubpaiddatefilter.IsNull);
            //if(!myrubsumfilter.IsNotNull)
            //    myfilter.ConditionAdd(myfilter.FilterWhereId, "rubsum", "IS NULL");
            //else if (myrubsumfilter.IsRange)
            //    myfilter.SetRange(myfilter.FilterWhereId, "rubsum", myrubsumfilter.NumberStart?.ToString("F2", System.Globalization.CultureInfo.InvariantCulture), myrubsumfilter.NumberStop?.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
            //else
            //    myfilter.SetNumber(myfilter.FilterWhereId, "rubsum", myrubsumfilter.Operator, myrubsumfilter.NumberStart?.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
            myfilter.SetDate(myfilter.FilterWhereId, "sellingdate", "sellingdate", mysellingdatefilter.DateStart, mysellingdatefilter.DateStop, mysellingdatefilter.IsNull);
            myfilter.SetDate(myfilter.FilterWhereId, "spddate", "spddate", myspddatefilter.DateStart, myspddatefilter.DateStop, myspddatefilter.IsNull);
            RefreshData(null);
        }
        private bool FilterRunCanExec(object parametr)
        { return true; }
        private RelayCommand myfilterclear;
        public ICommand FilterClear
        {
            get { return myfilterclear; }
        }
        private void FilterClearExec(object parametr)
        {
            myagentfilter.Clear();
            myagentfilter.IconVisibileChangedNotification();
            mycbratefilter.Clear();
            NumberFilterRun(mycbratefilter, "cbrate");
            mycbratefilter.IconVisibileChangedNotification();
            mycbratep2pfilter.Clear();
            NumberFilterRun(mycbratep2pfilter, "cbratep2p");
            mycbratep2pfilter.IconVisibileChangedNotification();
            myconsolidatefilter.Clear();
            myconsolidatefilter.IconVisibileChangedNotification();
            mycustomsinvoicerubsumfilter.Clear();
            NumberFilterRun(mycustomsinvoicerubsumfilter, "custinvrubsum");
            mycustomsinvoicerubsumfilter.IconVisibileChangedNotification();
            mycustomsinvoicepercentfilter.Clear();
            PercentFilterRun(mycustomsinvoicepercentfilter, "custinvpercent");
            mycustomsinvoicepercentfilter.IconVisibileChangedNotification();
            mycurrencypayfilter.Clear();
            NumberFilterRun(mycurrencypayfilter, "curpaidsum");
            mycurrencypayfilter.IconVisibileChangedNotification();
            mycurrencyboughtdatefilter.Clear();
            mycurrencyboughtdatefilter.IconVisibileChangedNotification();
            mycurrencybuyratefilter.Clear();
            NumberFilterRun(mycurrencybuyratefilter, "curbuyrate");
            mycurrencybuyratefilter.IconVisibileChangedNotification();
            mycurrencypaiddatefilter.Clear();
            mycurrencypaiddatefilter.IconVisibileChangedNotification();
            mycustomerfilter.Clear();
            mycustomerfilter.IconVisibileChangedNotification();
            mycustomsinvoicedatefilter.Clear();
            mycustomsinvoicedatefilter.IconVisibileChangedNotification();
            mycustomsinvoicepaiddatefilter.Clear();
            mycustomsinvoicepaiddatefilter.IconVisibileChangedNotification();
            mydealpassportfilter.Clear();
            mydealpassportfilter.IconVisibileChangedNotification();
            mydtsumfilter.Clear();
            NumberFilterRun(mydtsumfilter, "dtsum");
            mydtsumfilter.IconVisibileChangedNotification();
            myeurosumfilter.Clear();
            NumberFilterRun(myeurosumfilter, "eurosum");
            myeurosumfilter.IconVisibileChangedNotification();
            myexpirydatefilter.Clear();
            myexpirydatefilter.IconVisibileChangedNotification();
            myfincur1paiddatefilter.Clear();
            this.DateFilterRun(myfincur1paiddatefilter, "fincur1pdate");
            myfincur1paiddatefilter.IconVisibileChangedNotification();
            myfincur1sumfilter.Clear();
            NumberFilterRun(myfincur1sumfilter, "fincur1sum");
            myfincur1sumfilter.IconVisibileChangedNotification();
            myfincur2paiddatefilter.Clear();
            this.DateFilterRun(myfincur2paiddatefilter, "fincur2pdate");
            myfincur2paiddatefilter.IconVisibileChangedNotification();
            myfincur2sumfilter.Clear();
            NumberFilterRun(myfincur2sumfilter, "fincur2sum");
            myfincur2sumfilter.IconVisibileChangedNotification();
            myfinalpaiddatefilter.Clear();
            myfinalpaiddatefilter.IconVisibileChangedNotification();
            myfinrubsumfilter.Clear();
            NumberFilterRun(myfinrubsumfilter, "fininvrubsum");
            myfinrubsumfilter.IconVisibileChangedNotification();
            myfinrubsumpaidfilter.Clear();
            NumberFilterRun(myfinrubsumpaidfilter, "fininvrubpsum");
            myfinrubsumpaidfilter.IconVisibileChangedNotification();
            myinvoicenumberfilter.Clear();
            myinvoicenumberfilter.IconVisibileChangedNotification();
            mymanagerfilter.Clear();
            mymanagerfilter.IconVisibileChangedNotification();
            mynotefilter.Clear();
            mynotefilter.IconVisibileChangedNotification();
            myoverpayfilter.Clear();
            NumberFilterRun(myfinrubsumpaidfilter, "overpay");
            myoverpayfilter.IconVisibileChangedNotification();
            myparcelfilter.Clear();
            myparcelfilter.IconVisibileChangedNotification();
            mypercentfilter.Clear();
            mypercentfilter.IconVisibileChangedNotification();
            myprepayfilter.Clear();
            myprepayfilter.IconVisibileChangedNotification();
            myratediffperfilter.Clear();
            PercentFilterRun(myratediffperfilter, "ratediffper");
            myratediffperfilter.IconVisibileChangedNotification();
            myratediffresultfilter.Clear();
            myratediffresultfilter.IconVisibileChangedNotification();
            myrefundfilter.Clear();
            NumberFilterRun(myfinrubsumpaidfilter, "refund");
            myrefundfilter.IconVisibileChangedNotification();
            myrubdifffilter.Clear();
            NumberFilterRun(myrubdifffilter, "rubdiff");
            myrubdifffilter.IconVisibileChangedNotification();
            myrubpaiddatefilter.Clear();
            myrubpaiddatefilter.IconVisibileChangedNotification();
            myrubsumfilter.Clear();
            NumberFilterRun(myrubsumfilter, "rubsum");
            myrubsumfilter.IconVisibileChangedNotification();
            mysellingfilter.Clear();
            NumberFilterRun(mysellingfilter, "sellingsum");
            mysellingfilter.IconVisibileChangedNotification();
            mysellingdatefilter.Clear();
            mysellingdatefilter.IconVisibileChangedNotification();
            myspddatefilter.Clear();
            myspddatefilter.IconVisibileChangedNotification();
            this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }
        private void FilterActualise()
        {

        }
        private void NumberFilterRun(libui.NumberFilterVM filter,string property)
        {
            List<lib.SQLFilter.SQLFilterCondition> cond = myfilter.ConditionGet(myfilter.FilterWhereId, property);
            if (filter.FilterOn)
            {
                if (!filter.IsNotNull)
                {
                    if (cond.Count > 0)
                    {
                        if (!cond[0].propertyOperator.Equals("IS NULL"))
                        {
                            myfilter.ConditionValuesDel(cond[0].propertyid);
                            myfilter.ConditionUpd(cond[0].propertyid, "IS NULL");
                        }
                    }
                    else
                        myfilter.ConditionAdd(myfilter.FilterWhereId, property, "IS NULL");
                }
                else if (filter.IsRange)
                    myfilter.SetRange(myfilter.FilterWhereId, property, filter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture), filter.NumberStop?.ToString(System.Globalization.CultureInfo.InvariantCulture));
                else
                    myfilter.SetNumber(myfilter.FilterWhereId, property, filter.Operator, filter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if(cond.Count > 0)
                myfilter.ConditionDel(cond[0].propertyid);
        }
        private void PercentFilterRun(libui.NumberFilterVM filter, string property)
        {
            List<lib.SQLFilter.SQLFilterCondition> cond = myfilter.ConditionGet(myfilter.FilterWhereId, property);
            if (filter.FilterOn)
            {
                if (!filter.IsNotNull)
                {
                    if (cond.Count > 0)
                    {
                        if (!cond[0].propertyOperator.Equals("IS NULL"))
                        {
                            myfilter.ConditionValuesDel(cond[0].propertyid);
                            myfilter.ConditionUpd(cond[0].propertyid, "IS NULL");
                        }
                    }
                    else
                        myfilter.ConditionAdd(myfilter.FilterWhereId, property, "IS NULL");
                }
                else if (filter.IsRange)
                    myfilter.SetRange(myfilter.FilterWhereId, property, filter.NumberStart.HasValue ? decimal.Divide(filter.NumberStart.Value, 100M).ToString(System.Globalization.CultureInfo.InvariantCulture) : null, filter.NumberStop.HasValue ? decimal.Divide(filter.NumberStop.Value, 100M).ToString(System.Globalization.CultureInfo.InvariantCulture) : null);
                else
                    myfilter.SetNumber(myfilter.FilterWhereId, property, filter.Operator, filter.NumberStart.HasValue ? decimal.Divide(filter.NumberStart.Value,100M).ToString(System.Globalization.CultureInfo.InvariantCulture):null);
            }
            else if (cond.Count > 0)
                myfilter.ConditionDel(cond[0].propertyid);
        }
        private void DateFilterRun(libui.DateFilterVM filter, string property)
        {
            if (!filter.Synchronized)
            {
                myfilter.SetDate(myfilter.FilterWhereId, property, property, filter.DateStart, filter.DateStop, filter.IsNull);
                filter.Synchronized = true;
            }
        }
        private void DatePeriodFilterRun(libui.DateFilterVM filter, string group, string propertystart, string propertystop)
        {
            if (!filter.Synchronized)
            {
                myfilter.SetDatePeriod(myfilter.FilterWhereId, group, propertystart, propertystop, filter.DateStart, filter.DateStop, filter.IsNull);
                filter.Synchronized = true;
            }
        }
        private bool FilterEmpty
        { get {
                return !(myparcelfilter.FilterOn ||
                myagentfilter.FilterOn ||
                mycbratefilter.FilterOn ||
                mycbratep2pfilter.FilterOn ||
                myconsolidatefilter.FilterOn ||
                mycurrencypayfilter.FilterOn ||
                mycurrencyboughtdatefilter.FilterOn ||
                mycurrencybuyratefilter.FilterOn ||
                mycurrencypaiddatefilter.FilterOn ||
                mycustomerfilter.FilterOn ||
                mycustomerbalancefilter.FilterOn ||
                mycustomsinvoicedatefilter.FilterOn ||
                mycustomsinvoicepaiddatefilter.FilterOn ||
                mycustomsinvoicerubsumfilter.FilterOn ||
                mycustomsinvoicepercentfilter.FilterOn ||
                mydealpassportfilter.FilterOn ||
                mydtsumfilter.FilterOn ||
                myeurosumfilter.FilterOn ||
                myexpirydatefilter.FilterOn ||
                myfincur1paiddatefilter.FilterOn ||
                myfincur1sumfilter.FilterOn ||
                myfincur2paiddatefilter.FilterOn ||
                myfincur2sumfilter.FilterOn ||
                myfinalpaiddatefilter.FilterOn ||
                myfinrubsumfilter.FilterOn ||
                myfinrubsumpaidfilter.FilterOn ||
                myinvoicedatefilter.FilterOn ||
                myinvoicenumberfilter.FilterOn ||
                mymanagerfilter.FilterOn ||
                mynotefilter.FilterOn ||
                myoverpayfilter.FilterOn ||
                mypercentfilter.FilterOn ||
                myprepayfilter.FilterOn ||
                myratediffperfilter.FilterOn ||
                myratediffresultfilter.FilterOn ||
                myrefundfilter.FilterOn ||
                myrubdifffilter.FilterOn ||
                myrubpaiddatefilter.FilterOn ||
                myrubsumfilter.FilterOn ||
                mysellingfilter.FilterOn ||
                mysellingdatefilter.FilterOn ||
                myspddatefilter.FilterOn);
            }
        }
        #endregion
        internal Importer Importer
        { get { return mymaindbm.Importer; } }
        public bool IsEditable
        {
            get { return !this.IsReadOnly; }
        }
        public bool IsReadOnly
        { set; get; }
        private PaymentRegisterTotal mytotal;
        public PaymentRegisterTotal Total { get { return mytotal; } }

        private ListCollectionView mymanagers;
        public ListCollectionView Managers
        { get { return mymanagers; } }

        private RelayCommand mytdload;
        public ICommand TDLoad
        {
            get { return mytdload; }
        }
        private void TDLoadExec(object parametr)
        {
            if(parametr is PrepayCustomerRequestVM)
            {
                PrepayCustomerRequest request = (parametr as PrepayCustomerRequestVM).DomainObject;
                EventLoger log = new EventLoger();
                if (request.Request.Parcel != null)
                {
                    Specification.Specification spec = request.Request.Specification;
                    log.What = "DT"; log.Message = (spec?.Declaration?.Number ?? "Новая") + " Start Reg"; log.ObjectId = (spec?.Declaration?.Id ?? 0);
                    log.Execute();
                    string err = spec.LoadDeclaration();
                    if (string.IsNullOrEmpty(err))
                        this.OpenPopup("ТД загружена!", false);
                    else
                        this.OpenPopup(err, true);
                }
                else
                    this.OpenPopup("Загрузка ТД невозможна, заявка еще не включена в перевозку!", true);
                log.Message = (request.Request.Specification?.Declaration?.Number ?? "Новая") + " Finish Reg";
                log.ObjectId = (request.Request.Specification?.Declaration?.Id ?? 0);
                log.Execute();
            }
        }
        private bool TDLoadCanExec(object parametr)
        { return true; }

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
                            name = column.SortMemberPath.Substring(column.SortMemberPath.LastIndexOf('.', column.SortMemberPath.LastIndexOf('.')-1) + 1).Replace(".",string.Empty);
                        return name; };
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
        { return !(myview==null || myview.IsAddingNew | myview.IsEditingItem); }
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
                exWh.Name = "Реестр на оплату " + this.Importer.Name;

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
                            case "ManagerName":
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
                                exWh.Columns[column, Type.Missing].NumberFormat = dateformat; // "d.m.yyyy"
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
                                exWh.Cells[row, column] = item.IsPrepay?"Пр":string.Empty;
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
                            case nameof(Declaration.SPDDate):
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
                                exWh.Cells[row, column] = item.Prepay.NotDealPassport ? "без ПС":string.Empty;
                                break;
                            case nameof(PrepayCustomerRequest.ExpiryDate):
                                exWh.Cells[row, column] = item.ExpiryDate;
                                break;
                            case nameof(PrepayCustomerRequestVM.Refund):
                                exWh.Cells[row, column] = item.Refund;
                                break;
                            case "ManagerName":
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

        private RelayCommand mysplitup;
        public ICommand SplitUp
        {
            get { return mysplitup; }
        }
        private void SplitUpExec(object parametr)
        {
            if (parametr is PrepayCustomerRequestVM prepay && prepay.EuroSum > prepay.DTSum && MessageBox.Show("Разделить предоплату на две по сумме ДТ?","Разделение предоплаты",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                PrepayFundDBM pfdbm = new PrepayFundDBM() { ItemId = prepay.Prepay.Id };
                if (!pfdbm.GetFirst().IsPrepay.Value)
                {
                    Request request = new Request()
                    {
                        Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 0),
                        Agent = prepay.Request.Agent,
                        Customer = prepay.Request.Customer,
                        Importer = prepay.Request.Importer,
                        Manager = prepay.Request.Manager,
                        ParcelType = prepay.Request.ParcelType,
                        ServiceType = prepay.Request.ServiceType,
                    };
                    RequestCustomerLegal legal = request.CustomerLegals.First((RequestCustomerLegal item) => { return item.CustomerLegal == prepay.Customer.CustomerLegal.DomainObject; });
                    legal.Selected = true;
                    legal.Prepays.Add(new PrepayCustomerRequest(legal, prepay.Prepay, request));
                    RequestCustomerLegalDBM ldbm = new RequestCustomerLegalDBM();
                    RequestDBM rqdbm = new RequestDBM();
                    rqdbm.LegalDBM = ldbm;
                    rqdbm.SaveItemChanches(request);
                    if (rqdbm.Errors.Count > 0)
                    {
                        this.OpenPopup(rqdbm.ErrorMessage, true);
                        return;
                    }
                    CustomBrokerWpf.References.RequestStore.UpdateItem(request);
                    CustomBrokerWpf.References.RequestCustomerLegalStore.UpdateItem(legal);
                    CustomBrokerWpf.References.PrepayRequestStore.UpdateItem(legal.Prepays[0]);
                    pfdbm.GetFirst();
                    mysync.DomainCollection.Add(legal.Prepays[0]);
                }
                prepay.EuroSum = prepay.DTSum;
                mymaindbm.SaveItemChanches(prepay.DomainObject);
                foreach(PrepayCustomerRequest item in mysync.DomainCollection)
                    if(item.Prepay== prepay.Prepay && item.Request.Status.Id==0)
					{
                        mymaindbm.ItemId = item.Id;
                        mymaindbm.GetFirst();
                        if (mymaindbm.Errors.Count > 0)
                        {
                            this.OpenPopup(mymaindbm.ErrorMessage, true);
                            return;
                        }
                        mymaindbm.ItemId = null;
                        break;
					}
            }
        }
        private bool SplitUpCanExec(object parametr)
        { return parametr is PrepayCustomerRequestVM prepay && prepay.EuroSum > prepay.DTSum; }

        protected override void AddData(object parametr)
        {
            base.AddData(new PrepayCustomerRequestVM(new PrepayCustomerRequest( null, new Prepay(), null)));
        }
        protected override bool CanAddData(object parametr)
        {
            return this.IsEditable;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return this.IsEditable;
        }
        protected override bool CanRefreshData()
        {
            return myloadtask == null || !(myloadtask.Status == System.Threading.Tasks.TaskStatus.Running || myloadtask.Status == System.Threading.Tasks.TaskStatus.WaitingForActivation);
        }
        protected override bool CanRejectChanges()
        {
            return this.IsEditable;
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
            if (this.FilterEmpty)
                this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
            else
            {
                this.RefreshSuccessMessageHide = true;
                //if (myloadtask != null && (myloadtask.Status == System.Threading.Tasks.TaskStatus.Running || myloadtask.Status == System.Threading.Tasks.TaskStatus.WaitingForActivation)) return;
                LoadAsyncStop();
                //foreach (PrepayCustomerRequest item in mymaindbm.Collection)
                //    item.UnSubscribe();
                StringBuilder errstr = new StringBuilder();
                //mymaindbm.FillAsyncCompleted = () => {
                //    if (mycanceltasktoken == null || mycanceltasktoken.IsCancellationRequested) return;
                //    if (mymaindbm.Errors.Count > 0)
                //        foreach (lib.DBMError err in mymaindbm.Errors) errstr.AppendLine(err.Message);
                //    if (errstr.Length > 0)
                //        this.OpenPopup(errstr.ToString(), true);
                //    else
                //        this.OpenPopup("Даннные обновлены", false);
                //};
                PrepayDBM prdbm = new PrepayDBM();
                prdbm.FillType = lib.FillType.Refresh;
                RequestDBM rqdbm = new RequestDBM();
                rqdbm.FillType = lib.FillType.Refresh;
                RequestCustomerLegalDBM ldbm = new RequestCustomerLegalDBM();
                SpecificationCustomerInvoiceRateDBM ratedbm = new SpecificationCustomerInvoiceRateDBM();
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
                            prdbm.GetFirst();
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
                            //ldbm.Command.Connection = rqdbm.Command.Connection;
                            //App.Current.Dispatcher.Invoke(() => { item.Request.CustomerLegalsRefresh(ldbm); });
                            //if (mycanceltasktoken.IsCancellationRequested) return;
                            //if (ldbm.Errors.Count > 0) foreach (lib.DBMError err in ldbm.Errors) errstr.AppendLine(err.Message);
                            requests.Add(item.Request.Id);
                            if (mycanceltasktoken.IsCancellationRequested) return;
                            //if (!item.Request.SpecificationIsNull && !specs.Contains(item.Request.Specification.Id))
                            //{
                            //    item.Request.Specification.InvoiceDTRates.Clear();
                            //    ratedbm.Command.Connection = rqdbm.Command.Connection;
                            //    ratedbm.Specification = item.Request.Specification;
                            //    ratedbm.Load();
                            //    if (mycanceltasktoken.IsCancellationRequested) return;
                            //    if (ratedbm.Errors.Count > 0) foreach (lib.DBMError err in ratedbm.Errors) errstr.AppendLine(err.Message);
                            //    specs.Add(item.Request.Specification.Id);
                            //}
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
                myloadtask = myrefreshtask.ContinueWith((task)=> {
                    if (myrefreshtask.IsCanceled) return;
                    mymaindbm.Errors.Clear();
                    if (mycanceltasktoken.IsCancellationRequested) return;
                    mytotal.StopCount();
                    mymaindbm.Fill();
                    mytotal.StartCount();
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
        }
        protected override void SettingView()
        {
            myagentfilter.ItemsSource = myview.OfType<PrepayCustomerRequestVM>();
            myconsolidatefilter.ItemsSource = myview.OfType<PrepayCustomerRequestVM>();
            mycustomerfilter.ItemsSource = myview.OfType<PrepayCustomerRequestVM>();
            myinvoicenumberfilter.ItemsSource = myview.OfType<PrepayCustomerRequestVM>();
            mymanagerfilter.ItemsSource = myview.OfType<PrepayCustomerRequestVM>();
            mynotefilter.ItemsSource = myview.OfType<PrepayCustomerRequestVM>();
            myparcelfilter.ItemsSource = myview.OfType<PrepayCustomerRequestVM>();
            mypercentfilter.ItemsSource = myview.OfType<PrepayCustomerRequestVM>();

            mytotal = new PaymentRegisterTotal(myview);
            this.PropertyChangedNotification(nameof(Total));
            this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
        }
        protected override IList CreateCollectionOnDemand()
        {
            myfilter = new lib.SQLFilter.SQLFilter("PaymentRegister", "AND",CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            myparcelfiltergroup = myfilter.GroupAdd(myfilter.FilterWhereId, "parcel", "OR");
            myconsolidatefiltergroup = myfilter.GroupAdd(myfilter.FilterWhereId, "consolidate", "OR");
            myinvoicenumberfiltergroup = myfilter.GroupAdd(myfilter.FilterWhereId, "invoicenumber", "OR");
            //myfilter.SetDate(myfilter.FilterWhereId, "custinvdate", "custinvdate", mycustomsinvoicedatefilter.DateStart, mycustomsinvoicedatefilter.DateStop, mycustomsinvoicedatefilter.IsNull);
            mymaindbm.Filter = myfilter;
            mymaindbm.Collection = new System.Collections.ObjectModel.ObservableCollection<PrepayCustomerRequest>();
            //mymaindbm.FillAsync();
            mysync.DomainCollection = mymaindbm.Collection;
            return mysync.ViewModelCollection;
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

    public class PrepayAgentCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<PrepayCustomerRequestVM, lib.ReferenceSimpleItem>
    {
        internal PrepayAgentCheckListBoxVMFillDefault() : base()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return ((lib.ReferenceSimpleItem)item).Name; };
        }

        protected override void AddItem(PrepayCustomerRequestVM item)
        {
            lib.ReferenceSimpleItem name = CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", item.Prepay.Agent.Id);
            if (!Items.Contains(name)) Items.Add(name);
        }
    }
    public class PrepaConsolidateCheckListBoxVMFill : libui.CheckListBoxVMFill<PrepayCustomerRequestVM, string>
    {
        protected override void AddItem(PrepayCustomerRequestVM item)
        {
            if(Items.Count==0)
                Items.Add(string.Empty);
            if (!(item.Request?.Consolidate==null || Items.Contains(item.Request.Consolidate))) Items.Add(item.Request.Consolidate);
        }
    }
    public class PrepayCustomerCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<PrepayCustomerRequestVM, CustomerLegal>
    {
        internal PrepayCustomerCheckListBoxVMFillDefault() : base()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return ((CustomerLegal)item).Name; };
        }

        private List<CustomerLegal> mydefaultlist;
        internal List<CustomerLegal> DefaultList
        {
            get
            {
                if (mydefaultlist == null)
                {
                    mydefaultlist = new List<CustomerLegal>(); // из за долгой загрузки
                    CustomerLegalDBM dbm = new CustomerLegalDBM();
                    dbm.Fill();
                    mydefaultlist = dbm.Collection.ToList<CustomerLegal>();
                }
                return mydefaultlist;
            }
        }

        protected override void AddItem(PrepayCustomerRequestVM item)
        {
            if (!Items.Contains(item.Prepay.Customer)) Items.Add(item.Prepay.Customer);
        }
    }
    public class PrepaInvoiceNumberCheckListBoxVMFill : libui.CheckListBoxVMFill<PrepayCustomerRequestVM, string>
    {
        protected override void AddItem(PrepayCustomerRequestVM item)
        {
            if (!Items.Contains(item.Prepay.InvoiceNumber??string.Empty)) Items.Add(item.Prepay.InvoiceNumber ?? string.Empty);
        }
    }
    public class PrepayManagerCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<PrepayCustomerRequestVM, Manager>
    {
        internal PrepayManagerCheckListBoxVMFillDefault() : base()
        {
            this.DisplayPath = "NameComb";
            this.SearchPath = "NameComb";
            this.GetDisplayPropertyValueFunc = (item) => { return ((Manager)item).NameComb; };
            mynullmanager = new Manager(0, lib.DomainObjectState.Sealed, null, string.Empty, 208);
        }

        Manager mynullmanager;
        protected override void AddItem(PrepayCustomerRequestVM item)
        {
            if (!Items.Contains(item.Request.Manager?? mynullmanager)) Items.Add(item.Request.Manager ?? mynullmanager);
        }
    }
    public class PrepaNoteCheckListBoxVMFill : libui.CheckListBoxVMFill<PrepayCustomerRequestVM, string>
    {
        protected override void AddItem(PrepayCustomerRequestVM item)
        {
            if (Items.Count == 0)
                Items.Add(string.Empty);
            if (!(string.IsNullOrEmpty(item.Note) || Items.Contains(item.Note))) Items.Add(item.Note);
        }
    }
    public class PrepayParcelCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<PrepayCustomerRequestVM, ParcelNumber>
    {
        internal PrepayParcelCheckListBoxVMFillDefault() : base()
        {
            this.DisplayPath = "FullNumber";
            this.SearchPath = "Sort";
            this.SortDescriptions.Add(new System.ComponentModel.SortDescription("Sort", System.ComponentModel.ListSortDirection.Descending));
            this.GetDisplayPropertyValueFunc = (item) => { return ((ParcelNumber)item).FullNumber; };
        }

        protected override void AddItem(PrepayCustomerRequestVM item)
        {
            ParcelNumber name;
            if (Items.Count == 0)
            { name = new ParcelNumber() {Sort="999999" }; Items.Add(name); }
            if (item.Request?.Parcel?.Id > 0)
            {
                name = CustomBrokerWpf.References.ParcelNumbers.FindFirstItem("Id", item.Request.Parcel.Id);
                if (!Items.Contains(name)) Items.Add(name);
            }
        }
    }
    public class PrepayPercentCheckListBoxVMFill : libui.CheckListBoxVMFill<PrepayCustomerRequestVM, string>
    {
        protected override void AddItem(PrepayCustomerRequestVM item)
        {
            if (!(Items.Contains((item.Prepay.Percent*100).ToString("N0")))) Items.Add((item.Prepay.Percent * 100).ToString("N0"));
        }
    }
    public class PrepayUpdaterCheckListBoxVMFill : libui.CheckListBoxVMFill<PrepayCustomerRequestVM, string>
    {
        protected override void AddItem(PrepayCustomerRequestVM item)
        {
            if (!Items.Contains(item.Updater)) Items.Add(item.Updater);
        }
    }

    public class PaymentRegisterTotal : lib.TotalValues.TotalViewValues<PrepayCustomerRequestVM>
    {
        internal PaymentRegisterTotal(ListCollectionView view) : base(view)
        {
            //myinitselected = 2; // if not selected - sum=0
        }

        private int myitemcount;
        public int ItemCount { set { myitemcount = value; } get { return myitemcount; } }
        private decimal myprepayrubsum;
        public decimal PrepayRubSum { set { myprepayrubsum = value; } get { return myprepayrubsum; } }
        private decimal myprepayeurosum;
        public decimal PrepayEuroSum { set { myprepayeurosum = value; } get { return myprepayeurosum; } }
        private decimal mycurrencypaysum;
        public decimal CurrencyPaySum { set { mycurrencypaysum = value; } get { return mycurrencypaysum; } }
        private decimal mydtsum;
        public decimal DTSum { set { mydtsum = value; } get { return mydtsum; } }
        private decimal mycustomsinvoicerubsum;
        public decimal CustomsInvoiceRubSum { set { mycustomsinvoicerubsum = value; } get { return mycustomsinvoicerubsum; } }
        private decimal myselling;
        public decimal Selling { set { myselling = value; } get { return myselling; } }
        private decimal myfinalinvoicerubsum;
        public decimal FinalInvoiceRubSum { set { myfinalinvoicerubsum = value; } get { return myfinalinvoicerubsum; } }
        private decimal myfinalinvoicerubsumpaid;
        public decimal FinalInvoiceRubSumPaid { set { myfinalinvoicerubsumpaid = value; } get { return myfinalinvoicerubsumpaid; } }
        private decimal myfinalinvoicecursum;
        public decimal FinalInvoiceCurSum { set { myfinalinvoicecursum = value; } get { return myfinalinvoicecursum; } }
        private decimal myfinalinvoicecur2sum;
        public decimal FinalInvoiceCur2Sum { set { myfinalinvoicecur2sum = value; } get { return myfinalinvoicecur2sum; } }
        private decimal mycustomerbalance;
        public decimal CustomerBalance { set { mycustomerbalance = value; } get { return mycustomerbalance; } }
        private decimal myoverpay;
        public decimal OverPay { set { myoverpay = value; } get { return myoverpay; } }
        private decimal myrefund;
        public decimal Refund { set { myrefund = value; } get { return myrefund; } }
        private decimal myrubdiff;
        public decimal RubDiff { set { myrubdiff = value; } get { return myrubdiff; } }

        protected override void Item_ValueChangedHandler(PrepayCustomerRequestVM sender, ValueChangedEventArgs<object> e)
        {
            decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
            switch (e.PropertyName)
            {
                case nameof(PrepayCustomerRequestVM.CurrencyPaySum):
                    mycurrencypaysum += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.CurrencyPaySum));
                    break;
                case nameof(PrepayCustomerRequestVM.CustomerBalance):
                    mycustomerbalance += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.CustomerBalance));
                    break;
                case nameof(PrepayCustomerRequestVM.CustomsInvoiceRubSum):
                    mycustomsinvoicerubsum += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.CustomsInvoiceRubSum));
                    break;
                case nameof(PrepayCustomerRequestVM.DTSum):
                    mydtsum += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.DTSum));
                    break;
                case nameof(PrepayCustomerRequestVM.EuroSum):
                    myprepayeurosum += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.PrepayEuroSum));
                    break;
                case nameof(PrepayCustomerRequestVM.FinalInvoiceRubSum):
                    myfinalinvoicerubsum += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.FinalInvoiceRubSum));
                    break;
                case nameof(PrepayCustomerRequestVM.FinalInvoiceRubSumPaid):
                    myfinalinvoicerubsumpaid += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.FinalInvoiceRubSumPaid));
                    break;
                case nameof(PrepayCustomerRequestVM.FinalInvoiceCurSum):
                    myfinalinvoicecursum += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.FinalInvoiceCurSum));
                    break;
                case nameof(PrepayCustomerRequestVM.FinalInvoiceCurSum2):
                    myfinalinvoicecur2sum += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.FinalInvoiceCur2Sum));
                    break;
                case nameof(PrepayCustomerRequestVM.OverPay):
                    myoverpay += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.OverPay));
                    break;
                case nameof(PrepayCustomerRequestVM.Refund):
                    myrefund += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.Refund));
                    break;
                case nameof(PrepayCustomerRequestVM.RubDiff):
                    myrubdiff += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.RubDiff));
                    break;
                case nameof(PrepayCustomerRequestVM.RubSum):
                    myprepayrubsum += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.PrepayRubSum));
                    break;
                case nameof(PrepayCustomerRequestVM.Selling):
                    myselling += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.Selling));
                    break;
            }
        }
        protected override void ValuesReset()
        {
            myitemcount = 0;
            myprepayrubsum = 0M;
            myprepayeurosum = 0M;
            mycurrencypaysum = 0M;
            mydtsum = 0M;
            mycustomsinvoicerubsum = 0M;
            myselling = 0M;
            myfinalinvoicerubsum = 0M;
            myfinalinvoicerubsumpaid = 0M;
            myfinalinvoicecursum = 0M;
            myfinalinvoicecur2sum = 0M;
            mycustomerbalance = 0M;
            myoverpay = 0M;
            myrefund = 0M;
            myrubdiff = 0M;
        }
        protected override void ValuesPlus(PrepayCustomerRequestVM item)
        {
            myitemcount++;
            mycurrencypaysum += item.CurrencyPaySum ?? 0M;
            mycustomerbalance += item.CustomerBalance ?? 0M;
            mycustomsinvoicerubsum += item.CustomsInvoiceRubSum ?? 0M;
            mydtsum += item.DTSum ?? 0M;
            myfinalinvoicecursum += item.FinalInvoiceCurSum ?? 0M;
            myfinalinvoicecur2sum += item.FinalInvoiceCurSum2 ?? 0M;
            myfinalinvoicerubsum += item.FinalInvoiceRubSum ?? 0M;
            myfinalinvoicerubsumpaid += item.FinalInvoiceRubSumPaid ?? 0M;
            myoverpay += item.OverPay ?? 0M;
            myprepayeurosum += item.EuroSum??0M;
            myprepayrubsum += item.RubSum??0M;
            myrefund += item.Refund ?? 0M;
            myrubdiff += item.RubDiff ?? 0M;
            myselling += item.Selling ?? 0M;
        }
        protected override void ValuesMinus(PrepayCustomerRequestVM item)
        {
            myitemcount--;
            mycurrencypaysum -= item.CurrencyPaySum ?? 0M;
            mycustomerbalance -= item.CustomerBalance ?? 0M;
            mycustomsinvoicerubsum -= item.CustomsInvoiceRubSum ?? 0M;
            mydtsum -= item.DTSum ?? 0M;
            myfinalinvoicecursum -= item.FinalInvoiceCurSum ?? 0M;
            myfinalinvoicecur2sum -= item.FinalInvoiceCurSum2 ?? 0M;
            myfinalinvoicerubsum -= item.FinalInvoiceRubSum ?? 0M;
            myfinalinvoicerubsumpaid -= item.FinalInvoiceRubSumPaid ?? 0M;
            myoverpay -= item.OverPay ?? 0M;
            myprepayeurosum -= item.EuroSum ?? 0M;
            myprepayrubsum -= item.RubSum ?? 0M;
            myrefund -= item.Refund ?? 0M;
            myrubdiff -= item.RubDiff ?? 0M;
            myselling -= item.Selling ?? 0M;
        }
        protected override void PropertiesChangedNotifycation()
        {
            this.PropertyChangedNotification("ItemCount");
            this.PropertyChangedNotification(nameof(this.CurrencyPaySum));
            this.PropertyChangedNotification(nameof(this.CustomerBalance));
            this.PropertyChangedNotification(nameof(this.CustomsInvoiceRubSum));
            this.PropertyChangedNotification(nameof(this.DTSum));
            this.PropertyChangedNotification(nameof(this.FinalInvoiceCurSum));
            this.PropertyChangedNotification(nameof(this.FinalInvoiceCur2Sum));
            this.PropertyChangedNotification(nameof(this.FinalInvoiceRubSum));
            this.PropertyChangedNotification(nameof(this.FinalInvoiceRubSumPaid));
            this.PropertyChangedNotification(nameof(this.OverPay));
            this.PropertyChangedNotification(nameof(this.PrepayEuroSum));
            this.PropertyChangedNotification(nameof(this.PrepayRubSum));
            this.PropertyChangedNotification(nameof(this.Refund));
            this.PropertyChangedNotification(nameof(this.RubDiff));
            this.PropertyChangedNotification(nameof(this.Selling));
        }
    }
}
