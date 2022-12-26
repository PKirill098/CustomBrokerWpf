using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class Specification : lib.DomainBaseStamp
    {
        private Specification(int id, long stamp, lib.DomainObjectState mstate
            , int? parcelgroup, string consolidate, Importer importer, string filepath
            , decimal? pari, decimal? gtls, decimal? gtlscur, decimal? gtlsrate, decimal? ddspidy, decimal? westgate, decimal? mfk
            , int amount, decimal cellnumber, decimal clientsumdiff, decimal cost, decimal fondsum, decimal grossweight, decimal netweight
            ) : base(id, stamp, null, null, mstate)
        {
            myconsolidate = consolidate;
            myparcelgroup = parcelgroup;
            myimporter = importer;
            myfilepath = filepath;
            myamount = amount;
            mycellnumber = cellnumber;
            myclientsumdiff = clientsumdiff;
            mycost = cost;
            myfondsum = fondsum;
            mygrossweight = grossweight;
            mynetweight = netweight;
            mypari = pari;
            mygtls = gtls;
            mygtlscur = gtlscur;
            mygtlsrate = gtlsrate;
            myddspidy = ddspidy;
            mywestgate = westgate;
            mymfk = mfk;
            //myinvoicedtrates = new Dictionary<int, SpecificationCustomerInvoiceRate>();
            //myinvoicedtrateslock = new object();
            mycustomerlegalslist = new List<CustomerLegal>();
        }
        public Specification(int id, long stamp, lib.DomainObjectState mstate
            , Agent agent, string consolidate, Declaration declaration, string filepath, Importer importer, Parcel parcel, int? parcelgroup, Request request
            , decimal? pari, decimal? gtls, decimal? gtlscur, decimal? gtlsrate, decimal? ddspidy, decimal? westgate, decimal? mfk
            , int amount, decimal cellnumber, decimal clientsumdiff, decimal cost, decimal fondsum, decimal grossweight, decimal netweight
            ) : this(id, stamp, mstate
                , parcelgroup, consolidate, importer, filepath
                , pari, gtls, gtlscur, gtlsrate, ddspidy, westgate, mfk
                , amount, cellnumber, clientsumdiff, cost, fondsum, grossweight, netweight)
        {
            myagent = agent;
            mydeclaration = declaration;
            myparcel = parcel;
            myrequest = request;
        }
        public Specification(Parcel parcel, string consolidate, int? parcelgroup, Request request, Agent agent, Importer importer) : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added
            , agent, consolidate, null, null, importer, parcel, parcelgroup, request, null, null, null, null, null, null, null, 0, 0M, 0M, 0M, 0M, 0M, 0M)
        { }
        public Specification() : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added
            , null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 0, 0M, 0M, 0M, 0M, 0M, 0M)
        { }

        private Agent myagent;
        public Agent Agent
        { set { SetProperty<Agent>(ref myagent, value); } get { return myagent; } }
        public string CFPR
        { get { return string.IsNullOrEmpty(this.Consolidate) ? (this.ParcelGroup.HasValue ? this.ParcelGroup.ToString() : Request?.StorePoint) : this.Consolidate; } }

        private string myconsolidate;
        public string Consolidate
        {
            set { SetProperty<string>(ref myconsolidate, value); }
            get { return myconsolidate; }
        }
        private decimal? myddspidy;
        public decimal? DDSpidy
        {
            set { SetProperty<decimal?>(ref myddspidy, value); }
            get { return myddspidy; }
        }
        private Declaration mydeclaration;
        public Declaration Declaration
        {
            set { SetProperty<Declaration>(ref mydeclaration, value); }
            get
            {
                if (mydeclaration == null)
                    this.Declaration = new Declaration();
                return mydeclaration;
            }
        }
        private Importer myimporter;
        public Importer Importer
        { set { SetProperty<Importer>(ref myimporter, value); } get { return myimporter; } }
        private string myfilepath;
        public string FilePath
        {
            set { SetProperty<string>(ref myfilepath, value); }
            get { return myfilepath; }
        }
        private decimal? mygtls;
        public decimal? GTLS
        {
            set { SetProperty<decimal?>(ref mygtls, value); }
            get { return mygtlscur.HasValue && this.Parcel.UsdRate.HasValue ? mygtlscur * this.Parcel.UsdRate : mygtls; }
        }
        private decimal? mygtlscur;
        public decimal? GTLSCur
        {
            set { SetProperty<decimal?>(ref mygtlscur, value, () => { this.PropertyChangedNotification(nameof(this.GTLS)); }); }
            get { return mygtlscur; }
        }
        private decimal? mygtlsrate;
        public decimal? GTLSRate
        {
            set { SetProperty<decimal?>(ref mygtlsrate, value); }
            get { return mygtlsrate; }
        }
        private decimal? mymfk;
        public decimal? MFK
        {
            set
            {
                Action action = () => { this.PropertyChangedNotification(nameof(this.MFKRate)); this.PropertyChangedNotification(nameof(this.MFKWithoutRate)); };
                SetProperty<decimal?>(ref mymfk, value, action);
            }
            get { return mymfk; }
        }
        public decimal? MFKRate
        { get { return mymfk * 20M / 120M; } }
        public decimal? MFKWithoutRate
        { get { return mymfk - this.MFKRate; } }
        private Parcel myparcel;
        public Parcel Parcel
        {
            set
            {
                SetProperty<Parcel>(ref myparcel, value, () => { if (myparcel != null) CustomersLegalsRefresh(); });
            }
            get { return myparcel; }
        }
        private int? myparcelgroup;
        public int? ParcelGroup
        {
            set { SetProperty<int?>(ref myparcelgroup, value); }
            get { return myparcelgroup; }
        }
        private decimal? mypari;
        public decimal? Pari
        {
            set { SetProperty<decimal?>(ref mypari, value); }
            get { return mypari; }
        }
        private Request myrequest;
        public Request Request
        {
            set { SetProperty<Request>(ref myrequest, value, () => { if (myrequest != null) { this.PropertyChangedNotification(nameof(this.CFPR)); this.CustomersLegalsRefresh(); } }); }
            get { return myrequest; }
        }
        private decimal? mywestgate;
        public decimal? WestGate
        {
            set
            {
                Action action = () => { this.PropertyChangedNotification(nameof(this.WestGateRate)); this.PropertyChangedNotification(nameof(this.WestGateWithoutRate)); };
                SetProperty<decimal?>(ref mywestgate, value, action);
            }
            get { return mywestgate; }
        }
        public decimal? WestGateRate
        { get { return mywestgate * 20M / 120M; } }
        public decimal? WestGateWithoutRate
        { get { return mywestgate - this.WestGateRate; } }

        private string mycustomers;
        public string Customers
        { get { return mycustomers; } }
        private string mycustomerlegal;
        public string CustomerLegals
        { get { return mycustomerlegal; } }
        private List<CustomerLegal> mycustomerlegalslist;
        public List<CustomerLegal> CustomerLegalsList
        { get { return mycustomerlegalslist; } }

        SpecificationDetailDBM myddbm;
        private ObservableCollection<SpecificationDetail> mydetails;
        public ObservableCollection<SpecificationDetail> Details
        {
            get
            {
                if (mydetails == null)
                {
                    App.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new Action(()=> { mydetails = new ObservableCollection<SpecificationDetail>(); }));
                    if (myddbm == null)
                        App.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => {
                            myddbm = new SpecificationDetailDBM() { Specification = this };
                        }));
                    myddbm.Collection = mydetails;
                    myddbm.Fill();
                    if (myddbm.Errors.Count > 0)
                        throw (new Exception(myddbm.ErrorMessage));
                    else RefreshTotalDetails();
                }
                return mydetails;
            }
        }
        internal bool DetailsIsNull
        { get { return mydetails == null; } }
        internal void RefreshTotalDetails()
        {
            decimal count;
            myamount = 0; mycellnumber = 0M; mycost = 0M; mygrossweight = 0M; mynetweight = 0M; myfondsum = 0M;
            foreach (SpecificationDetail item in mydetails)
            {
                if (item.DomainState < lib.DomainObjectState.Deleted)
                {
                    myamount += item.Amount ?? 0;
                    decimal.TryParse(item.CellNumber, out count);
                    mycellnumber += count;
                    mycost += item.Cost ?? 0M;
                    myfondsum += item.Client == null ? (item.Cost ?? 0M) : 0M;
                    mygrossweight += item.GrossWeight ?? 0M;
                    mynetweight += item.NetWeight ?? 0M;
                }
            }
            this.PropertyChangedNotification("Amount");
            this.PropertyChangedNotification("CellNumber");
            this.PropertyChangedNotification("Cost");
            this.PropertyChangedNotification("FondSum");
            this.PropertyChangedNotification("GrossWeight");
            this.PropertyChangedNotification("NetWeight");
        }
        internal Task DetailsTaskAsync { set; get; }
        internal async Task DetailsGetAsync()
        {
            if (mydetails == null && DetailsTaskAsync == null)
            {
                mydetails = new ObservableCollection<SpecificationDetail>();
                if (myddbm == null) myddbm = new SpecificationDetailDBM() { Specification = this };
                myddbm.FillAsyncCompleted = () => { if (myddbm.Errors.Count > 0) throw (new Exception(myddbm.ErrorMessage)); else RefreshTotalDetails(); DetailsTaskAsync = null; };
                myddbm.Collection = mydetails;
                DetailsTaskAsync = myddbm.FillAsync();
                await DetailsTaskAsync;
            }
        }
        private int myamount;
        public int Amount { set { myamount = value; this.PropertyChangedNotification("Amount"); } get { return myamount; } }
        private decimal mycellnumber;
        public decimal CellNumber { set { mycellnumber = value; this.PropertyChangedNotification("CellNumber"); } get { return mycellnumber; } }
        private decimal? myclientsumdiff;
        public decimal? ClientSumDiff { set { myclientsumdiff = value; this.PropertyChangedNotification("ClientSumDiff"); } get { return myclientsumdiff; } }
        private decimal? mycost;
        public decimal? Cost { set { mycost = value; this.PropertyChangedNotification("Cost"); } get { return mycost; } }
        private decimal? myfondsum;
        public decimal? FondSum { set { myfondsum = value; this.PropertyChangedNotification("FondSum"); } get { return myfondsum; } }
        private decimal mygrossweight;
        public decimal GrossWeight { set { mygrossweight = value; this.PropertyChangedNotification("GrossWeight"); } get { return mygrossweight; } }
        private decimal mynetweight;
        public decimal Invoice { get { return this.Requests?.Sum((item) => { return item.InvoiceDiscount; }) ?? 0M; } }
        public decimal NetWeight { set { mynetweight = value; this.PropertyChangedNotification("NetWeight"); } get { return mynetweight; } }

        internal string BuildFileName(string sourcepath)
        {
            System.Text.StringBuilder name = new System.Text.StringBuilder();
            name.Append(this.Parcel.ParcelNumber);
            if (string.IsNullOrEmpty(this.Consolidate))
            {
                if (this.ParcelGroup.HasValue)
                    name.Append("_gr").Append(this.ParcelGroup.ToString());
                else
                    name.Append("_s").Append(this.Request.StorePoint);
                name.Append('_').Append((this.Request ?? this.Requests.First()).CustomerName);
            }
            else
                name.Append("_").Append(this.Consolidate);
            StringBuilder agentname=new StringBuilder(this.Agent.Name);
            foreach (char c in Path.InvalidPathChars)
                agentname.Replace(c.ToString(), string.Empty);
            name.Append('_').Append(agentname);
            
            name.Append(System.IO.Path.GetExtension(sourcepath));
            this.FilePath = name.ToString();
            return this.FilePath;
        }
        internal string LoadDeclaration()
        {
            if (this.Declaration == null)
                this.Declaration = new Declaration();
            if (this.Declaration?.TotalSum != null)
                if (System.Windows.MessageBox.Show("Таможенная декларация уже загружена. Перезаписать?", "Загрузка ТД", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.No)
                    return string.Empty;

            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.CheckPathExists = true;
            fd.CheckFileExists = true;
            fd.Title = "Выбор файла декларации";
            fd.Filter = "Файлы XML|*.xml;";
            if (fd.ShowDialog().Value)
            {
                string err = this.Declaration.LoadDeclaration(fd.FileName);
                if (!string.IsNullOrEmpty(err))
                    err = "НЕ удалось разобрать структуру файла ТД!\n" + err;
                else
                {
                    lib.ReferenceSimpleItem status= CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 100);
                    foreach (Request request in this.Requests)
                        if(request.Status.Id<100) request.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 100);
                }
                return err;
            }
            else
                return string.Empty;
        }
        protected override void RejectProperty(string property, object value)
        {

        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Specification newitem = (Specification)sample;
            this.Id = newitem.Id;
            this.Agent = newitem.Agent;
            this.FilePath = newitem.FilePath;
            this.Declaration = newitem.Declaration;
            this.DDSpidy = newitem.DDSpidy;
            this.GTLS = newitem.GTLS;
            this.GTLSCur = newitem.GTLSCur;
            this.GTLSRate = newitem.GTLSRate;
            this.MFK = newitem.MFK;
            this.Pari = newitem.Pari;
            this.WestGate = newitem.WestGate;
            if (this.DetailsIsNull && newitem.Amount > 0)
            {
                this.Amount = newitem.Amount;
                this.CellNumber = newitem.CellNumber;
                this.ClientSumDiff = newitem.ClientSumDiff;
                this.Cost = newitem.Cost;
                this.FondSum = newitem.FondSum;
                this.GrossWeight = newitem.GrossWeight;
                this.NetWeight = newitem.NetWeight;
            }
        }
        internal void CustomersLegalsRefresh()
        {
            if (string.IsNullOrEmpty(this.Consolidate) & !this.ParcelGroup.HasValue && this.Request != null)
            {
                mycustomers = this.Request.CustomerName;
                mycustomerlegal = this.Request.CustomerLegalsNames;
                foreach (RequestCustomerLegal legal in this.Request.CustomerLegals)
                    if (legal.Selected && !mycustomerlegalslist.Contains(legal.CustomerLegal)) mycustomerlegalslist.Add(legal.CustomerLegal);
            }
            else if (this.Parcel != null)
                for (int i = 0; i < this.Parcel.Requests.Count; i++)
                {
                    Request req = this.Parcel.Requests[i];
                    if (!string.IsNullOrEmpty(this.Consolidate))
                    {
                        if (req.Consolidate == this.Consolidate)
                        {
                            if (string.IsNullOrEmpty(mycustomers) || mycustomers.IndexOf(req.CustomerName) < 0)
                                mycustomers = (string.IsNullOrEmpty(mycustomers) ? string.Empty : mycustomers + ", ") + req.CustomerName;
                            string[] names = req.CustomerLegalsNames.Split(',');
                            foreach (string name in names)
                                if (string.IsNullOrEmpty(mycustomerlegal) || mycustomerlegal.IndexOf(name) < 0)
                                    mycustomerlegal = (string.IsNullOrEmpty(mycustomerlegal) ? string.Empty : mycustomerlegal + ", ") + name.Trim();
                            foreach (RequestCustomerLegal legal in req.CustomerLegals)
                                if (legal.Selected && !mycustomerlegalslist.Contains(legal.CustomerLegal)) mycustomerlegalslist.Add(legal.CustomerLegal);
                        }
                    }
                    else if (this.ParcelGroup.HasValue)
                    {
                        if (req.ParcelGroup == this.ParcelGroup)
                        {
                            if (string.IsNullOrEmpty(mycustomers) || mycustomers.IndexOf(req.CustomerName) < 0)
                                mycustomers = (string.IsNullOrEmpty(mycustomers) ? string.Empty : mycustomers + ", ") + req.CustomerName;
                            string[] names = req.CustomerLegalsNames.Split(',');
                            foreach (string name in names)
                                if (string.IsNullOrEmpty(mycustomerlegal) || mycustomerlegal.IndexOf(name) < 0)
                                    mycustomerlegal = (string.IsNullOrEmpty(mycustomerlegal) ? string.Empty : mycustomerlegal + ", ") + name.Trim();
                            foreach (RequestCustomerLegal legal in req.CustomerLegals)
                                if (legal.Selected && !mycustomerlegalslist.Contains(legal.CustomerLegal)) mycustomerlegalslist.Add(legal.CustomerLegal);
                        }
                    }
                }
            this.PropertyChangedNotification(nameof(this.Customers));
            this.PropertyChangedNotification(nameof(this.CustomerLegals));
            this.PropertyChangedNotification(nameof(this.CustomerLegalsList));
            this.PropertyChangedNotification(nameof(this.Requests));
            this.PropertyChangedNotification(nameof(this.Invoice));
            this.PropertyChangedNotification(nameof(this.CFPR));
        }

        internal IEnumerable<Request> Requests
        {
            get
            {
                return this.Parcel?.Requests.Where((Request item) =>
                    {
                        return item.Parcel == this.Parcel && string.Equals(item.Consolidate, this.Consolidate)
                          && (!string.IsNullOrEmpty(this.Consolidate) || (item.ParcelGroup == this.ParcelGroup
                          && (this.ParcelGroup.HasValue || item.Id == this.Request?.Id)));
                    });
            }
        }
        //private Dictionary<int, SpecificationCustomerInvoiceRate> myinvoicedtrates;
        //internal Dictionary<int, SpecificationCustomerInvoiceRate> InvoiceDTRates
        //{ get { return myinvoicedtrates; } }
        //private object myinvoicedtrateslock;
        //internal void InvoiceDTRatesAdd(SpecificationCustomerInvoiceRate rate)
        //{
        //    lock(myinvoicedtrateslock)
        //    {
        //        if (rate.CustomerId.HasValue && !myinvoicedtrates.ContainsKey(rate.CustomerId.Value))
        //            myinvoicedtrates.Add(rate.CustomerId.Value, rate);
        //    }
        //}
        //internal SpecificationCustomerInvoiceRate GetCustomerInvoiceCostRate(CustomerLegal customer)
        //{
        //    SpecificationCustomerInvoiceRate customercost;
        //    //lock (mythreadlock)
        //    //{
        //    //    CustomerLegal threadcustomer = customer;
        //    //    customercost = this.Details.Where((SpecificationDetail detail) => { return detail.Client == threadcustomer; }).Sum((SpecificationDetail detail) => { return detail.Cost; });
        //    //if(customercost.HasValue && customercost.Value>0.0099M)
        //    //{
        //    //    try
        //    //    { 
        //    //    decimal? requestcost = this.Requests.Sum(
        //    //        (Request request)=> { return
        //    //            request.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.CustomerLegal == threadcustomer; }).Sum(
        //    //                (RequestCustomerLegal legal)=>{ return
        //    //                    legal.Prepays.Sum((Domain.Account.PrepayCustomerRequest requestprepay) => { return
        //    //                       requestprepay.DtSumSet ?? requestprepay.EuroSum ; });
        //    //                }
        //    //            );
        //    //        }
        //    //    );
        //    //     customercost = requestcost / customercost;
        //    //       }
        //    //    catch { }
        //    //}
        //    return this.InvoiceDTRates.TryGetValue(customer.Id, out customercost) ? customercost: null;
        //    //}
        //}
        internal int ImportDetail(string filepath, lib.TaskAsync.TaskAsync myexceltask)
        {
            int maxr, usedr = 0, r = 10;
            decimal v;
            SpecificationDetail detail;
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            try
            {
                CustomerLegal legal = this.CustomerLegalsList?.Count == 1 ? this.CustomerLegalsList[0] : null;


                exApp.Visible = false;
                exApp.DisplayAlerts = false;
                exApp.ScreenUpdating = false;

                Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
                Excel.Worksheet exWh = exWb.Sheets[1];
                maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                myexceltask.ProgressChange(5);

                for (; r <= maxr; r++)
                {
                    if (string.IsNullOrEmpty(exWh.Cells[r, 6].Text as string)) continue;
                    detail = new SpecificationDetail();

                    if (int.TryParse(exWh.Cells[r, 13].Text as string, out int n) && n < 0 | n > 10000000)
                        throw new Exception("Некорректное значение количества товара: " + exWh.Cells[r, 6].Text);
                    else
                        detail.Amount = n;
                    detail.Branch = exWh.Cells[r, 10].Text;
                    detail.Brand = exWh.Cells[r, 11].Text;
                    detail.CellNumber = (exWh.Cells[r, 16].Value)?.ToString();
                    detail.Certificate = exWh.Cells[r, 22].Text;
                    detail.Contexture = exWh.Cells[r, 5].Text;
                    if (decimal.TryParse(exWh.Cells[r, 19].Value.ToString(), out v) && v < 0)
                        throw new Exception("Некорректное значение стоимости товара: " + exWh.Cells[r, 19].Value.ToString());
                    else
                        detail.Cost = v;
                    detail.CountryEN = exWh.Cells[r, 21].Text;
                    detail.CountryRU = exWh.Cells[r, 20].Text;
                    detail.Customer = exWh.Cells[r, 28].Text;
                    detail.Description = exWh.Cells[r, 6].Text;
                    detail.DescriptionAccount = exWh.Cells[r, 33].Text;
                    detail.Gender = exWh.Cells[r, 4].Text;
                    detail.GrossWeight = (decimal?)exWh.Cells[r, 15].Value;
                    detail.Name = exWh.Cells[r, 3].Text;
                    detail.NetWeight = (decimal?)exWh.Cells[r, 14].Value;
                    detail.Note = exWh.Cells[r, 24].Text;
                    detail.Packing = exWh.Cells[r, 17].Text;
                    detail.Price = (decimal?)exWh.Cells[r, 18].Value;
                    detail.Producer = exWh.Cells[r, 13].Text;
                    detail.RowOrder = r - 10;
                    detail.SizeEN = exWh.Cells[r, 7].Text;
                    detail.SizeRU = exWh.Cells[r, 8].Text;
                    detail.Specification = this;
                    detail.TNVED = (exWh.Cells[r, 12].Value)?.ToString();
                    detail.VendorCode = (exWh.Cells[r, 9].Value)?.ToString();
                    if ((exWh.Cells[r, 29].Value)?.ToString().Length > 0)
                    {
                        Request request = this.Requests.FirstOrDefault((Request req) => { return req.StorePoint == (exWh.Cells[r, 29].Value)?.ToString(); });
                        if (request == null)
                            throw new Exception("Позиция по складу " + (exWh.Cells[r, 29].Value)?.ToString() + " не соответствует ни одной заявке в разбивке!");
                        else
                            detail.Request = request;
                    }
                    detail.Client = legal;
                    App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<SpecificationDetail>(this.Details.Add), detail);
                    usedr++;
                    myexceltask.ProgressChange(r, maxr, 0.85M, 0.15M);
                }
                myexceltask.ProgressChange(99);
                this.RefreshTotalDetails();
                exWb.Close();
                exApp.Quit();

                if (this.Request != null)
                    this.Request.IsSpecification = true;
                else if (this.Parcel != null)
                    foreach (Request req in this.Parcel.Requests)
                    {
                        if (!string.IsNullOrEmpty(this.Consolidate))
                        {
                            if (req.Consolidate == this.Consolidate)
                                req.IsSpecification = true;
                        }
                        else if (this.ParcelGroup.HasValue)
                        {
                            if (req.ParcelGroup == this.ParcelGroup)
                                req.IsSpecification = true;
                        }
                    }

                myexceltask.ProgressChange(100);
                return usedr;
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
                throw new Exception("Ошибка в строке " + r.ToString() + ": " + ex.Message);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        internal void Income1C()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            bool iserr;
            int r = 2;
            string str, filepath = string.Empty;
            Variation variation;
            VariationDBM vdbm = new VariationDBM();
            Excel.Range rng;
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.SheetsInNewWorkbook = 1;
            Excel.Workbook exWb;
            Excel.Worksheet exWh = null;
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(System.Globalization.CultureInfo.InvariantCulture.Name, true);
            try
            {
                exWb = exApp.Workbooks.Add(Type.Missing);
                exWh = exWb.Sheets[1];

                exWh.Cells[1, 1] = "№";
                exWh.Cells[1, 2] = "Вид номенклатуры";
                exWh.Cells[1, 3] = "Наименование";
                exWh.Cells[1, 4] = "Полное наименование";
                exWh.Cells[1, 5] = "Артикул";
                exWh.Cells[1, 6] = "Входит в группу";
                exWh.Cells[1, 7] = "Единица";
                exWh.Cells[1, 8] = "Импортный товар:\nНомер ГТД";
                exWh.Cells[1, 9] = "Импортный товар:\nСтрана происхождения";
                exWh.Cells[1, 10] = "Кол-во";
                exWh.Cells[1, 11] = "Цена за ед.";
                exWh.Cells[1, 12] = "Цена";
                exWh.Cells[1, 13] = "Классификация:\nТН ВЭД";
                
                r = 2;
                
                foreach (SpecificationDetail detail in this.Details.OrderBy((SpecificationDetail item) => { return item.RowOrder; }))
                {
                    str = string.Empty;
                    if (!string.IsNullOrWhiteSpace(detail.Gender) && detail.Gender.Length > 2 && detail.Description.ToLower().IndexOf(" ", detail.Description.ToLower().IndexOf(" " + detail.Gender.Substring(0, 3).ToLower()) + 3) > -1)
                        str = detail.Description.Substring(0, detail.Description.ToLower().IndexOf(" ", detail.Description.ToLower().IndexOf(" " + detail.Gender.Substring(0, 3).ToLower()) + 3));
                    if (str == string.Empty)
                    {
                        str = detail.TNVED.StartsWith("61") ? " трикотаж" : (detail.TNVED.StartsWith("62") ? " текстил" : string.Empty);
                        if (str != string.Empty)
                        {
                            if (detail.Description.ToLower().IndexOf(str) > -1)
                                str = detail.Description.Substring(0, detail.Description.ToLower().IndexOf(str));
                            else
                                str = string.Empty;
                        }
                        if (str == string.Empty)
                            str = detail.Description;
                    }
                    vdbm.Plural = str;
                    variation = vdbm.GetFirst();
                    if (variation != null && !string.IsNullOrEmpty(variation.Singular))
                    {
                        iserr = false;
                        str = (variation.Singular.Length > 0 ? variation.Singular.Substring(0, 1).ToUpper() : string.Empty) + (variation.Singular.Length > 1 ? variation.Singular.Substring(1) : string.Empty);
                    }
                    else
                    {
                        iserr = true;
                        if (variation == null)
                            vdbm.SaveItemChanches(new Variation() { Plural = vdbm.Plural.ToLower() });
                    }
                    exWh.Cells[r, 1] = r - 1;
                    exWh.Cells[r, 2] = string.IsNullOrEmpty(detail.DescriptionAccount) ? "Товары" : (
                        detail.TNVED.StartsWith("64") ? "Обувная продукция" : (
                            detail.TNVED.StartsWith("43") | detail.TNVED.StartsWith("61") | detail.TNVED.StartsWith("62") ? "Товары легкой промышленности" : "Товары"));
                    exWh.Cells[r, 3] = string.IsNullOrEmpty(detail.DescriptionAccount) ? str + " " + detail.VendorCode + " " + detail.Brand : detail.DescriptionAccount;
                    if (iserr)
                        exWh.Cells[r, 3].Interior.Color = 255;
                    else
                        exWh.Cells[r, 3].Interior.Color = 16777215;
                    exWh.Cells[r, 4] = exWh.Cells[r, 3];
                    exWh.Cells[r, 5] = detail.VendorCode;
                    exWh.Cells[r, 6] = this.Agent.Name;
                    exWh.Cells[r, 7] = "шт";
                    exWh.Cells[r, 8] = this.Declaration?.Number;
                    exWh.Cells[r, 9] = detail.CountryRU;
                    exWh.Cells[r, 10] = detail.Amount;
                    exWh.Cells[r, 11] = detail.Price;
                    exWh.Cells[r, 12] = detail.Cost;
                    exWh.Cells[r, 13] = detail.TNVED;

                    r++;
                }

                rng = exWh.Range[exWh.Cells[1, 1], exWh.Cells[r - 1, 13]];
                rng.Borders.Weight = Excel.XlBorderWeight.xlThin;
                rng.HorizontalAlignment = Excel.Constants.xlCenter;
                rng.VerticalAlignment = Excel.Constants.xlCenter;
                exWh.Columns[3, Type.Missing].HorizontalAlignment = Excel.Constants.xlLeft;
                exWh.Columns[4, Type.Missing].HorizontalAlignment = Excel.Constants.xlLeft;
                rng = exWh.Range[exWh.Cells[1, 8], exWh.Cells[1, 9]];
                rng.WrapText = false;
                rng.Columns.AutoFit();
                rng.WrapText = true;
                exWh.Cells[1, 13].WrapText = false;
                exWh.Cells[1, 13].Columns.AutoFit();
                exWh.Cells[1, 13].WrapText = true;
                rng = exWh.Range[exWh.Cells[1, 1], exWh.Cells[r - 1, 2]];
                rng.Columns.AutoFit();
                rng = exWh.Range[exWh.Cells[1, 3], exWh.Cells[1, 4]];
                rng.Columns.AutoFit();
                rng = exWh.Range[exWh.Cells[1, 5], exWh.Cells[r + 1, 13]];
                rng.Columns.AutoFit();

                filepath = Path.Combine(
                    CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + (this.Parcel.ParcelNumber ?? string.Empty),
                    CustomBrokerWpf.Properties.Settings.Default.Income1CFileRoot);
                if (!Directory.Exists(filepath))
                    System.IO.Directory.CreateDirectory(filepath);
                filepath = Path.Combine(filepath,
                    Path.GetFileNameWithoutExtension(this.FilePath).Replace("-разбивка", string.Empty) + "_1C_" +
                    this.Details.Sum((SpecificationDetail detail) => { return detail.Cost ?? 0M; }).ToString("F2", System.Globalization.CultureInfo.CurrentCulture) +
                    ".csv"); //Path.GetExtension(this.FilePath)

                exWb.SaveAs(filepath, 62);
                Mouse.OverrideCursor = null;
                exApp.Visible = true;
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
                KirillPolyanskiy.Common.PopupCreator.GetPopup("Не удалось создать файл из " + this.FilePath + ".\n" + ex.Message+("\n"+ex.InnerException?.Message??string.Empty), true, null, true, System.Windows.Controls.Primitives.PlacementMode.Center).IsOpen = true;
            }
            finally
            {
                Mouse.OverrideCursor = null;
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        internal void Selling1C()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            bool iserr, skip = false, merge =false;
            int client = -1, request = 0, r = 2;
            string str,rootpath,filename=string.Empty;
            decimal eurosum=0M;
            SellingFactors factors;
            Variation variation;
            VariationDBM vdbm = new VariationDBM();
            Excel.Range rng;
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.SheetsInNewWorkbook = 1;
            Excel.Workbook exWb = null;
            Excel.Worksheet exWh = null;
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(System.Globalization.CultureInfo.InvariantCulture.Name, true);
            //culture.NumberFormat.NumberDecimalSeparator = exApp.International[Excel.XlApplicationInternational.xlDecimalSeparator];
            rootpath = Path.Combine(
                    CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + (this.Parcel.ParcelNumber ?? string.Empty),
                    CustomBrokerWpf.Properties.Settings.Default.Selling1CFileRoot);
            if (!Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            SellingFactorsDBM sfdbm = new SellingFactorsDBM() { Specification = this };
            sfdbm.Load();
            if (sfdbm.Errors.Count > 0)
            {
                Mouse.OverrideCursor = null;
                KirillPolyanskiy.Common.PopupCreator.GetPopup("Не удалось загрузить курсы для расчета реализации " + this.CFPR + ".\n" + sfdbm.Errors[0].Message, true, null, true, System.Windows.Controls.Primitives.PlacementMode.Center).IsOpen = true;
                return;
            }
            try
            {
                if (!this.Details.Any((SpecificationDetail item) => { return item.Client != null; }))
                {
                    Mouse.OverrideCursor = null;
                    if (MessageBox.Show("В резбивке " + this.CFPR + " не везде проставлен клиент!\nПродолжить подготовку реализации?", "Реализация 1С", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                        return;
                    Mouse.OverrideCursor = Cursors.Wait;
                }
                foreach (SpecificationDetail detail in this.Details.OrderBy((SpecificationDetail item) => { return item.Request?.Id ?? 0; }).OrderBy((SpecificationDetail item) => { return item.Client?.Id ?? 0; }))
                {
                    if (client != (detail.Client?.Id ?? 0) | request != (detail.Request?.Id ?? 0))
                    {
                        if (!skip & exWh != null)
                        {
                            Selling1CEndFormat(exWh, r);
                            exWb.SaveAs(filename.Replace("!sum!",
                                eurosum.ToString("F2", System.Globalization.CultureInfo.CurrentCulture) + "_" +
                                exWh.Cells[r + 1, 16].Text as string
                                ), 62);
                        }

                        skip = false; // start new Selling
                        client = (detail.Client?.Id ?? 0);
                        request = (detail.Request?.Id ?? 0);
                        factors = sfdbm.SellingFactors.FirstOrDefault((SellingFactors item) => { return item.Customer == detail.Client & item.Request == detail.Request; });
                        if (factors == null)
                        {
                            Mouse.OverrideCursor = null;
                            if (MessageBox.Show("Не найдены коэффициенты для разбивки " + this.CFPR
                                    + ", клиента " + (detail.Client?.Name ?? "<неустановлен>")
                                    + (detail.Request == null ? string.Empty : " и заявки " + detail.Request.StorePointDate)
                                    + "!\nПродолжить подготовку других реализаций?", "Реализация 1С", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                                return;
                            else
                            { skip = true; Mouse.OverrideCursor = Cursors.Wait; continue; }
                        }
                        else if (!(factors.Persent.HasValue & factors.DTRate.HasValue))
                        {
                            Mouse.OverrideCursor = null;
                            if (MessageBox.Show("Не найден Курс ГТД, Коэфиц. Алгоритм для разбивки " + this.CFPR
                                    + ", клиента " + (detail.Client.Name ?? "<неустановлен>")
                                    + (detail.Request == null ? string.Empty : " и заявки " + detail.Request.StorePointDate)
                                    + "!\nПродолжить подготовку других реализаций?", "Реализация 1С", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                                return;
                            else
                            { skip = true; Mouse.OverrideCursor = Cursors.Wait; continue; }
                        }
                        filename = Path.Combine(rootpath,
                            Path.GetFileNameWithoutExtension(this.FilePath).Replace("-разбивка", string.Empty) + "_1C_реализация_" +
                            (detail.Client?.Name.Replace("/", string.Empty) ?? string.Empty) + "_!sum!" +
                            ".csv");//Path.GetExtension(this.FilePath)

                        //if (exWh == null)
                        //    exWh = exWb.Sheets[1];
                        //else
                        //    exWh = exWb.Sheets.Add(Type.Missing, Type.Missing, 1, Excel.XlSheetType.xlWorksheet);
                        exWb = exApp.Workbooks.Add(Type.Missing);
                        exWh = exWb.Sheets[1];
                        str = detail.Client?.Name.Replace("/", string.Empty) ?? string.Empty;
                        exWh.Name = str.Length > 31 ? str.Remove(31) : str;

                        exWh.Columns[5, Type.Missing].NumberFormat = "@";
                        //exWh.Columns[11, Type.Missing].NumberFormat = @"# ##0,00";
                        //exWh.Columns[12, Type.Missing].NumberFormat = @"# ##0,00";
                        exWh.Columns[13, Type.Missing].NumberFormat = "@";
                        //exWh.Columns[14, Type.Missing].NumberFormat = @"# ##0,00";
                        //exWh.Columns[15, Type.Missing].NumberFormat = @"# ##0,00";
                        //exWh.Columns[16, Type.Missing].NumberFormat = @"# ##0,00";
                        exWh.Cells[1, 14].NumberFormat = "@";
                        exWh.Cells[1, 15].NumberFormat = "@";
                        exWh.Cells[1, 16].NumberFormat = "@";

                        exWh.Cells[1, 1] = "№";
                        exWh.Cells[1, 2] = "Вид номенклатуры";
                        exWh.Cells[1, 3] = "Наименование";
                        exWh.Cells[1, 4] = "Полное наименование";
                        exWh.Cells[1, 5] = "Артикул";
                        exWh.Cells[1, 6] = "Входит в группу";
                        exWh.Cells[1, 7] = "Единица";
                        exWh.Cells[1, 8] = "Импортный товар:\nНомер ГТД";
                        exWh.Cells[1, 9] = "Импортный товар:\nСтрана происхождения";
                        exWh.Cells[1, 10] = "Кол-во";
                        exWh.Cells[1, 11] = "Цена за ед.";
                        exWh.Cells[1, 12] = "Цена";
                        exWh.Cells[1, 13] = "Классификация:\nТН ВЭД";
                        exWh.Cells[1, 14] = "1";
                        exWh.Cells[1, 15] = "2";
                        exWh.Cells[1, 16] = "3";
                        exWh.Cells[1, 17] = "Курс покупки валюты";
                        exWh.Cells[1, 18] = "Курс ГТД";
                        exWh.Cells[1, 19] = "Коэфиц.\nАлгоритм";

                        rng = exWh.Range[exWh.Cells[1, 14], exWh.Cells[1, 16]];
                        rng.HorizontalAlignment = Excel.Constants.xlCenter;
                        rng.VerticalAlignment = Excel.Constants.xlCenter;
                        rng.Interior.Color = 14610923;

                        rng = exWh.Range[exWh.Cells[2, 17], exWh.Cells[2, 19]];
                        rng.NumberFormat = "#,##0.0000";
                        rng = exWh.Range[exWh.Cells[1, 17], exWh.Cells[2, 19]];
                        rng.Borders.Weight = Excel.XlBorderWeight.xlThin;
                        rng.VerticalAlignment = Excel.Constants.xlCenter;
                        rng.HorizontalAlignment = Excel.Constants.xlCenter;
                        rng.Interior.Color = 14610923;
                        rng.WrapText = true;
                        rng.Columns.ColumnWidth = 9.29;
                        exWh.Cells[2, 17] = factors.Service == "ТД" ? (factors.CBRate.HasValue ? factors.CBRate.Value : 1M) : 0M;
                        exWh.Cells[2, 18] = !factors.CBRate.HasValue & factors.Service == "ТД" ? 1M : factors.DTRate ?? 0M;
                        exWh.Cells[2, 19] = factors.Persent.Value;

                        r = 2;
                    }
                    else if (skip)
                        continue;

                    str = string.Empty;
                    if (!string.IsNullOrWhiteSpace(detail.Gender) && detail.Gender.Length > 2 && detail.Description.ToLower().IndexOf(" ", detail.Description.ToLower().IndexOf(" " + detail.Gender.Substring(0, 3).ToLower()) + 3) > -1)
                        str = detail.Description.Substring(0, detail.Description.ToLower().IndexOf(" ", detail.Description.ToLower().IndexOf(" " + detail.Gender.Substring(0, 3).ToLower()) + 3));
                    if (str == string.Empty)
                    {
                        str = detail.TNVED.StartsWith("61") ? " трикотаж" : (detail.TNVED.StartsWith("62") ? " текстил" : string.Empty);
                        if (str != string.Empty)
                        {
                            if (detail.Description.ToLower().IndexOf(str) > -1)
                                str = detail.Description.Substring(0, detail.Description.ToLower().IndexOf(str));
                            else
                                str = string.Empty;
                        }
                        if (str == string.Empty)
                            str = detail.Description;
                    }
                    vdbm.Plural = str;
                    variation = vdbm.GetFirst();
                    if (variation != null && !string.IsNullOrEmpty(variation.Singular))
                    {
                        iserr = false;
                        str = (variation.Singular.Length > 0 ? variation.Singular.Substring(0, 1).ToUpper() : string.Empty) + (variation.Singular.Length > 1 ? variation.Singular.Substring(1) : string.Empty);
                    }
                    else
                    {
                        iserr = true;
                        if (variation == null)
                            vdbm.SaveItemChanches(new Variation() { Plural = vdbm.Plural.ToLower() });
                    }
                    string goodstype = string.IsNullOrEmpty(detail.DescriptionAccount) ? "Товары" : (
                        detail.TNVED.StartsWith("64") ? "Обувная продукция" : (
                            detail.TNVED.StartsWith("43") | detail.TNVED.StartsWith("61") | detail.TNVED.StartsWith("62") ? "Товары легкой промышленности" : "Товары"));
                    string descr = string.IsNullOrEmpty(detail.DescriptionAccount) ? str + " " + detail.VendorCode + " " + detail.Brand : detail.DescriptionAccount;
                    // объединение
                    if(goodstype == "Обувная продукция" || goodstype == "Товары легкой промышленности")
                    {
                        for (int i=2; i<r; i++)
                            if(!iserr
                                && exWh.Cells[i, 2].Text == goodstype
                                && exWh.Cells[i, 4].Text == descr
                                && exWh.Cells[i, 6].Text == this.Agent.Name
                                && exWh.Cells[i, 9].Text == detail.CountryRU
                                && exWh.Cells[i, 13].Text == detail.TNVED
                                )
                            {
                                r--;
                                merge = true;
                                if(detail.Amount.HasValue) exWh.Cells[i, 10] = ((int?)exWh.Cells[i, 10].Value??0) + detail.Amount.Value;
                                exWh.Cells[i, 14].FormulaR1C1 = exWh.Cells[i, 14].FormulaR1C1 + "+Round(" + (detail.Price ?? 0M).ToString(culture) + "*R2C[3]" + ", 2)" + "*" + (detail.Amount ?? 0M).ToString(culture);
                                exWh.Cells[i, 15].FormulaR1C1 = exWh.Cells[i, 15].FormulaR1C1 + "+Round(" + (detail.Price ?? 0M).ToString(culture) + "*R2C[3]*R2C[4]" + ", 2)" + "*" + (detail.Amount ?? 0M).ToString(culture);
                                break;
                            }
                    }
                    if (!merge)
                    {
                        exWh.Cells[r, 1] = r - 1;
                        exWh.Cells[r, 2] = goodstype;
                        exWh.Cells[r, 3] = descr;
                        if (iserr)
                            exWh.Cells[r, 3].Interior.Color = 255;
                        else
                            exWh.Cells[r, 3].Interior.Color = 16777215;
                        exWh.Cells[r, 4] = exWh.Cells[r, 3];
                        exWh.Cells[r, 5] = detail.VendorCode;
                        exWh.Cells[r, 6] = this.Agent.Name;
                        exWh.Cells[r, 7] = "шт";
                        exWh.Cells[r, 8] = this.Declaration?.Number;
                        exWh.Cells[r, 9] = detail.CountryRU;
                        exWh.Cells[r, 10] = detail.Amount;
                        //exWh.Cells[r, 12] = detail.Cost;
                        exWh.Cells[r, 13] = detail.TNVED;
                        exWh.Cells[r, 14].FormulaR1C1 = "=Round(" + (detail.Price ?? 0M).ToString(culture) + "*R2C[3]" + ", 2)" + "*" + (detail.Amount ?? 0M).ToString(culture);
                        exWh.Cells[r, 15].FormulaR1C1 = "=Round(" + (detail.Price ?? 0M).ToString(culture) + "*R2C[3]*R2C[4]" + ", 2)" + "*" + (detail.Amount ?? 0M).ToString(culture);
                        exWh.Cells[r, 16].FormulaR1C1 = "=RC[-2]+RC[-1]";
                        exWh.Cells[r, 11].FormulaR1C1 = "=RC[5]/RC[-1]";
                        exWh.Cells[r, 12].FormulaR1C1 = "=RC[4]";
                    }
                    eurosum += detail.Cost ?? 0M;

                    r++;
                    merge = false;
                }

                if (!skip & exWh != null)
                {
                    this.Selling1CEndFormat(exWh, r);
                    exWb.SaveAs(filename.Replace("!sum!",
                        eurosum.ToString("F2", System.Globalization.CultureInfo.CurrentCulture) + "_" +
                        exWh.Cells[r + 1, 16].Text as string), 62);
                    Mouse.OverrideCursor = null;
                    //if (exWh != null) // at least one sheet has been created
                    //    exWb.SaveAs(CustomBrokerWpf.Properties.Settings.Default.DetailsFileRoot + Path.GetFileNameWithoutExtension(this.FilePath).Replace("-разбивка", string.Empty) + "_1C" + "_реализация" + Path.GetExtension(this.FilePath));
                }
                exApp.Visible = true;
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
                KirillPolyanskiy.Common.PopupCreator.GetPopup("Не удалось создать файл из " + this.FilePath + ".\n" + ex.Message + ("\n" + ex.InnerException?.Message ?? string.Empty), true, null, true, System.Windows.Controls.Primitives.PlacementMode.Center).IsOpen = true;
            }
            finally
            {
                Mouse.OverrideCursor = null;
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        private void Selling1CEndFormat(Excel.Worksheet exWh, int r)
        {
            exWh.Cells[r + 1, 13] = "Итого: ";
            exWh.Cells[r + 1, 14].FormulaR1C1 = "=SUM(R2C:R[-2]C)";
            exWh.Cells[r + 1, 15].FormulaR1C1 = "=SUM(R2C:R[-2]C)";
            exWh.Cells[r + 1, 16].FormulaR1C1 = "=SUM(R2C:R[-2]C)";
            Excel.Range rng = exWh.Range[exWh.Cells[r + 1, 14], exWh.Cells[r + 1, 16]];
            rng.Font.Bold = true;

            rng = exWh.Range[exWh.Cells[1, 1], exWh.Cells[r - 1, 16]];
            rng.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rng.HorizontalAlignment = Excel.Constants.xlCenter;
            rng.VerticalAlignment = Excel.Constants.xlCenter;

            exWh.Columns[3, Type.Missing].HorizontalAlignment = Excel.Constants.xlLeft;
            exWh.Columns[4, Type.Missing].HorizontalAlignment = Excel.Constants.xlLeft;
            rng = exWh.Range[exWh.Cells[2, 14], exWh.Cells[r + 1, 16]];
            rng.HorizontalAlignment = Excel.Constants.xlRight;

            rng = exWh.Range[exWh.Cells[1, 8], exWh.Cells[1, 9]];
            rng.WrapText = false;
            rng.Columns.AutoFit();
            rng.WrapText = true;
            exWh.Cells[1, 13].WrapText = false;
            exWh.Cells[1, 13].Columns.AutoFit();
            exWh.Cells[1, 13].WrapText = true;


            rng = exWh.Range[exWh.Cells[1, 1], exWh.Cells[r - 1, 2]];
            rng.Columns.AutoFit();
            rng = exWh.Range[exWh.Cells[1, 3], exWh.Cells[1, 4]];
            rng.Columns.AutoFit();
            rng = exWh.Range[exWh.Cells[1, 5], exWh.Cells[r + 1, 16]];
            rng.Columns.AutoFit();

            //
            //rng.Columns.AutoFit();
            //rng.Rows.AutoFit();
            //rng = exWh.Columns[1, Type.Missing]; rng.AutoFit();
            //rng = exWh.Columns[5, Type.Missing]; rng.AutoFit();
            //rng.Columns[11, Type.Missing].AutoFit();
            //rng = exWh.Columns[12, Type.Missing]; rng.AutoFit();

        }
    }

    internal class SpecificationStore : lib.DomainStorageLoad<Specification, SpecificationDBM>
    {
        public SpecificationStore(SpecificationDBM dbm) : base(dbm) { }

        internal Specification GetItemLoad(Request request, SqlConnection conection, out List<lib.DBMError> errors)
        {
            Specification firstitem = default(Specification);
            errors = new List<lib.DBMError>();
            try
            {
                if (request.Parcel != null)
                {
                    if (myupdatingcoll > 0) System.Threading.Thread.Sleep(20);
                    while (myupdatingcoll > 0)
                        System.Threading.Thread.Sleep(10);
                    myforcount++;
                    try
                    {
                        foreach (Specification item in mycollection.Values)
                            if (item.Parcel == request.Parcel && string.Equals(item.Consolidate, request.Consolidate)
                                && (!string.IsNullOrEmpty(request.Consolidate) || (item.ParcelGroup == request.ParcelGroup
                                && (request.ParcelGroup.HasValue || item.Request?.Id == request.Id))))
                            { firstitem = item; break; }
                    }
                    finally { this.myforcount--; }
                    if (firstitem == default(Specification))
                    {
                        SpecificationDBM dbm;
                        dbm = GetDBM();
                        dbm.Parcel = null;
                        dbm.Importer = null;
                        dbm.Filter = null;
                        dbm.Request = request;
                        dbm.Command.Connection = conection;
                        firstitem = dbm.GetFirst();
                        if (firstitem != null) firstitem = UpdateItem(firstitem);
                        dbm.Command.Connection = null;
                        errors.AddRange(dbm.Errors);
                        dbm.Request = null;
                        dbm.SelectParamsReset();
                        dbm.Errors.Clear();
                        mydbmanagers.Enqueue(dbm);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(new lib.DBMError(request, ex.Message, "SpecificationStore.GetItemLoad"));
            }
            return firstitem;
        }
        internal Specification GetItemLoad(Request request, out List<lib.DBMError> errors)
        {
            return GetItemLoad(request, null, out errors);
        }
        protected override void UpdateProperties(Specification olditem, Specification newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class SpecificationDBM : lib.DBManagerStamp<Specification>
    {
        public SpecificationDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "spec.Specification_sp";
            InsertCommandText = "spec.SpecificationAdd_sp";
            UpdateCommandText = "spec.SpecificationUpd_sp";
            DeleteCommandText = "spec.SpecificationDel_sp";

            SelectParams = new SqlParameter[] {
                new SqlParameter("@id", System.Data.SqlDbType.Int),
                new SqlParameter("@importerid", System.Data.SqlDbType.Int),
                new SqlParameter("@filterid", System.Data.SqlDbType.Int),
                new SqlParameter("@parcelid", System.Data.SqlDbType.Int),
                new SqlParameter("@requestid", System.Data.SqlDbType.Int),
                new SqlParameter("@parcelgroup", System.Data.SqlDbType.Int),
                new SqlParameter("@consolidate", System.Data.SqlDbType.NVarChar,5)
            };
            InsertParams = new SqlParameter[] { InsertParams[0]
                , new SqlParameter("@agentid", System.Data.SqlDbType.Int)
                , new SqlParameter("@consolidate", System.Data.SqlDbType.NVarChar,5)
                , new SqlParameter("@importerid", System.Data.SqlDbType.Int)
                , new SqlParameter("@parcelgroup", System.Data.SqlDbType.Int)
                , new SqlParameter("@parcelid", System.Data.SqlDbType.Int)
                , new SqlParameter("@requestid", System.Data.SqlDbType.Int)
            };
            UpdateParams = new SqlParameter[] { UpdateParams[0]
                , new SqlParameter("@filepathtrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@declarationidtrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@paritrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@gtlstrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@gtlscurtrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@gtlsratetrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@ddspidytrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@westgatetrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@mfktrue", System.Data.SqlDbType.Bit)
            };
            InsertUpdateParams = new SqlParameter[] {InsertUpdateParams[0]
                , new SqlParameter("@filepath", System.Data.SqlDbType.NVarChar,200)
                , new SqlParameter("@declarationid", System.Data.SqlDbType.Int)
                , new SqlParameter("@pari", System.Data.SqlDbType.Money)
                , new SqlParameter("@gtls", System.Data.SqlDbType.Money)
                , new SqlParameter("@gtlscur", System.Data.SqlDbType.Money)
                , new SqlParameter("@gtlsrate", System.Data.SqlDbType.Money)
                , new SqlParameter("@ddspidy", System.Data.SqlDbType.Money)
                , new SqlParameter("@westgate", System.Data.SqlDbType.Money)
                , new SqlParameter("@mfk", System.Data.SqlDbType.Money)
            };
            myddbm = new SpecificationDetailDBM(); myddbm.Command = new SqlCommand();
            mytddbm = new DeclarationDBM();
            myratedbm = new SpecificationCustomerInvoiceRateDBM();
        }

        private Parcel myparcel;
        internal Parcel Parcel
        { set { myparcel = value; } }
        private string mycons;
        internal string Consolidate
        { set { mycons = value; } get { return mycons; } }
        private Request myrequest;
        internal Request Request
        { set { myrequest = value; } get { return myrequest; } }
        private Importer myimporter;
        internal Importer Importer
        { set { myimporter = value; } get { return myimporter; } }
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { set { myfilter = value; } get { return myfilter; } }
        SpecificationDetailDBM myddbm;
        DeclarationDBM mytddbm;
        SpecificationCustomerInvoiceRateDBM myratedbm;

        protected override Specification CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            List<lib.DBMError> errors;
            Agent agent = CustomBrokerWpf.References.AgentStore.GetItemLoad(reader.GetInt32(this.Fields["agentid"]), addcon, out errors);
            this.Errors.AddRange(errors);
            Declaration declaration = null;
            if (!reader.IsDBNull(this.Fields["declarationid"]))
            {
                mytddbm.Errors.Clear();
                mytddbm.Command.Connection = addcon;
                mytddbm.ItemId = reader.GetInt32(this.Fields["declarationid"]);
                declaration = mytddbm.GetFirst();
            }
            Parcel parcel = CustomBrokerWpf.References.ParcelStore.GetItemLoad(reader.GetInt32(this.Fields["parcelid"]), addcon, out errors);
            this.Errors.AddRange(errors);
            Request request = null;
            if (!reader.IsDBNull(this.Fields["requestid"]))
            {
                request = CustomBrokerWpf.References.RequestStore.GetItemLoad(reader.GetInt32(this.Fields["requestid"]), addcon, out errors);
                this.Errors.AddRange(errors);
            }
            Specification spec = new Specification(reader.GetInt32(0), reader.GetInt64(this.Fields["stamp"]), lib.DomainObjectState.Unchanged
                , agent
                , reader.IsDBNull(this.Fields["consolidate"]) ? null : reader.GetString(this.Fields["consolidate"])
                , declaration
                , reader.IsDBNull(this.Fields["filepath"]) ? null : reader.GetString(this.Fields["filepath"])
                , CustomBrokerWpf.References.Importers.FindFirstItem("Id", reader.GetInt32(this.Fields["importerid"]))
                , parcel
                , reader.IsDBNull(this.Fields["parcelgroup"]) ? (int?)null : reader.GetInt32(this.Fields["parcelgroup"])
                , request
                , reader.IsDBNull(this.Fields["pari"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["pari"])
                , reader.IsDBNull(this.Fields["gtls"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["gtls"])
                , reader.IsDBNull(this.Fields["gtlscur"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["gtlscur"])
                , reader.IsDBNull(this.Fields["gtlsrate"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["gtlsrate"])
                , reader.IsDBNull(this.Fields["ddspidy"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["ddspidy"])
                , reader.IsDBNull(this.Fields["westgate"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["westgate"])
                , reader.IsDBNull(this.Fields["mfk"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["mfk"])
                , reader.IsDBNull(this.Fields["amount"]) ? 0 : reader.GetInt32(this.Fields["amount"])
                , reader.IsDBNull(this.Fields["cellnumber"]) ? 0M : reader.GetDecimal(this.Fields["cellnumber"])
                , reader.IsDBNull(this.Fields["clientsumdiff"]) ? 0M : reader.GetDecimal(this.Fields["clientsumdiff"])
                , reader.IsDBNull(this.Fields["cost"]) ? 0M : reader.GetDecimal(this.Fields["cost"])
                , reader.IsDBNull(this.Fields["fondsum"]) ? 0M : reader.GetDecimal(this.Fields["fondsum"])
                , reader.IsDBNull(this.Fields["grossweight"]) ? 0M : reader.GetDecimal(this.Fields["grossweight"])
                , reader.IsDBNull(this.Fields["netweight"]) ? 0M : reader.GetDecimal(this.Fields["netweight"])
                );
            Specification specsore = CustomBrokerWpf.References.SpecificationStore.UpdateItem(spec, this.FillType == lib.FillType.Refresh);
            if ((this.FillType == lib.FillType.Refresh & specsore.Declaration?.DomainState == lib.DomainObjectState.Modified) || specsore.Declaration?.Stamp != declaration?.Stamp)
            {
                if (specsore.Declaration == null || declaration == null)
                    specsore.Declaration = declaration;
                else
                    specsore.Declaration.UpdateProperties(declaration);
            }
            if (myparcel != null && myrequest == null)
            {
                specsore.Amount = spec.Amount;
                specsore.CellNumber = spec.CellNumber;
                specsore.ClientSumDiff = spec.ClientSumDiff;
                specsore.Cost = spec.Cost;
                specsore.FondSum = spec.FondSum;
                specsore.GrossWeight = spec.GrossWeight;
                specsore.NetWeight = spec.NetWeight;
                specsore.CustomersLegalsRefresh();
            }
            //specsore.InvoiceDTRates.Clear();
            //myratedbm.Command.Connection = addcon;
            //myratedbm.Specification = specsore;
            //myratedbm.Load();
            //if(myratedbm.Errors.Count>0) foreach (lib.DBMError err in myratedbm.Errors) this.Errors.Add(err);

            return specsore;
        }
        protected override void GetOutputSpecificParametersValue(Specification item)
        {
        }
        protected override bool SaveChildObjects(Specification item)
        {
            bool issuccess = true;
            if (!item.DetailsIsNull)
            {
                myddbm.Errors.Clear();
                myddbm.Specification = item;
                myddbm.Collection = item.Details;
                if (!myddbm.SaveCollectionChanches())
                {
                    issuccess = false;
                    foreach (lib.DBMError err in myddbm.Errors) this.Errors.Add(err);
                }
                //else if(item.DomainStatePrevious==lib.DomainObjectState.Added)
                //{
                //    VendorCodesDBM vdbm = new VendorCodesDBM();
                //    vdbm.Specification = item;
                //    vdbm.Execute();
                //}
            }
            return issuccess;
        }
        protected override bool SaveIncludedObject(Specification item)
        {
            bool success = true;
            if (item.Declaration != null)
            {
                mytddbm.Errors.Clear();
                if (!mytddbm.SaveItemChanches(item.Declaration))
                {
                    success = false;
                    foreach (lib.DBMError err in mytddbm.Errors) this.Errors.Add(err);
                }
            }
            if (item.HasPropertyOutdatedValue(nameof(Specification.Declaration)) && item.GetPropertyOutdatedValue(nameof(Specification.Declaration)) != null)
            {
                mytddbm.Errors.Clear();
                if (!mytddbm.SaveItemChanches((Declaration)item.GetPropertyOutdatedValue(nameof(Specification.Declaration))))
                {
                    foreach (lib.DBMError err in mytddbm.Errors) this.Errors.Add(err);
                    success = false;
                }
            }
            return success;
        }
        protected override bool SaveReferenceObjects()
        {
            mytddbm.Command.Connection = this.Command.Connection;
            myddbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            foreach (SqlParameter par in SelectParams)
                switch (par.ParameterName)
                {
                    case "@parcelid":
                        par.Value = myparcel?.Id ?? myrequest?.Parcel?.Id;
                        break;
                    case "@consolidate":
                        par.Value = myrequest?.Consolidate ?? mycons;
                        break;
                    case "@parcelgroup":
                        par.Value = string.IsNullOrEmpty(myrequest?.Consolidate) ? myrequest?.ParcelGroup : null;
                        break;
                    case "@requestid":
                        par.Value = string.IsNullOrEmpty(myrequest?.Consolidate) & myrequest?.ParcelGroup == null ? myrequest?.Id : null;
                        break;
                    case "@importerid":
                        par.Value = myimporter?.Id;
                        break;
                    case "@filterid":
                        par.Value = myfilter?.FilterWhereId;
                        break;
                }
        }
        protected override bool SetSpecificParametersValue(Specification item)
        {
            foreach (SqlParameter par in this.InsertParams)
            {
                switch (par.ParameterName)
                {
                    case "@agentid":
                        par.Value = item.Agent.Id;
                        break;
                    case "@consolidate":
                        par.Value = item.Consolidate;
                        break;
                    case "@importerid":
                        par.Value = item.Importer.Id;
                        break;
                    case "@parcelid":
                        par.Value = item.Parcel.Id;
                        break;
                    case "@requestid":
                        par.Value = item.Request?.Id;
                        break;
                    case "@parcelgroup":
                        par.Value = item.ParcelGroup;
                        break;
                }
            }
            foreach (SqlParameter par in this.UpdateParams)
            {
                switch (par.ParameterName)
                {
                    case "@filepathtrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Specification.FilePath));
                        break;
                    case "@declarationidtrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Specification.Declaration));
                        break;
                    case "@paritrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Specification.Pari));
                        break;
                    case "@gtlstrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Specification.GTLS));
                        break;
                    case "@gtlscurtrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Specification.GTLSCur));
                        break;
                    case "@gtlsratetrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Specification.GTLSRate));
                        break;
                    case "@ddspidytrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Specification.DDSpidy));
                        break;
                    case "@westgatetrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Specification.WestGate));
                        break;
                    case "@mfktrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Specification.MFK));
                        break;
                }
            }
            foreach (SqlParameter par in this.InsertUpdateParams)
            {
                switch (par.ParameterName)
                {
                    case "@filepath":
                        par.Value = item.FilePath;
                        break;
                    case "@declarationid":
                        par.Value = item.Declaration?.Id;
                        break;
                    case "@pari":
                        par.Value = item.Pari;
                        break;
                    case "@gtls":
                        par.Value = item.GTLS;
                        break;
                    case "@gtlscur":
                        par.Value = item.GTLSCur;
                        break;
                    case "@gtlsrate":
                        par.Value = item.GTLSRate;
                        break;
                    case "@ddspidy":
                        par.Value = item.DDSpidy;
                        break;
                    case "@westgate":
                        par.Value = item.WestGate;
                        break;
                    case "@mfk":
                        par.Value = item.MFK;
                        break;
                }
            }
            return item.Parcel.Id > 0 & (item.Request?.Id ?? 1) > 0;
        }
        protected override void CancelLoad()
        {
        }
    }

    public class SpecificationVM : lib.ViewModelErrorNotifyItem<Specification>
    {
        public SpecificationVM(Specification model) : base(model)
        {
            DeleteRefreshProperties.AddRange(new string[] { "Amount", "CellNumber", "Cost", "Customers", "CustomerLegals", "GrossWeight", "FilePath", nameof(this.Importer), "NetWeight", "CFPR" });
            InitProperties();
            myfileopen = new RelayCommand(FileOpenExec, FileOpenCanExec);
        }
        public SpecificationVM() : this(new Specification()) { }

        public Agent Agent
        { get { return this.DomainObject.Agent; } }
        public int Amount { get { return this.DomainObject.Amount; } }
        public decimal CellNumber { get { return this.DomainObject.CellNumber; } }
        public string CFPR
        { get { return this.IsEnabled ? this.DomainObject.CFPR : null; } }
        public decimal? ClientSumDiff { get { return this.DomainObject.ClientSumDiff; } }
        public string Consolidate
        {
            get { return this.IsEnabled ? this.DomainObject.Consolidate : null; }
        }
        public decimal? Cost { get { return this.DomainObject.Cost; } }
        public string Customers
        { get { return this.IsEnabled ? this.DomainObject.Customers : null; } }
        public string CustomerLegals
        { get { return this.IsEnabled ? this.DomainObject.CustomerLegals : null; } }
        public decimal? DDSpidy
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.DDSpidy.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.DDSpidy.Value, value.Value))))
                {
                    string name = nameof(this.DDSpidy);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DDSpidy);
                    ChangingDomainProperty = name; this.DomainObject.DDSpidy = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DDSpidy : (decimal?)null; }
        }
        public Declaration Declaration
        { get { return this.DomainObject.Declaration; } }
        public decimal GrossWeight { get { return this.DomainObject.GrossWeight; } }
        public decimal? GTLS
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.GTLS.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.GTLS.Value, value.Value))))
                {
                    string name = nameof(this.GTLS);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GTLS);
                    ChangingDomainProperty = name; this.DomainObject.GTLS = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.GTLS : (decimal?)null; }
        }
        public decimal? GTLSCur
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.GTLSCur.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.GTLSCur.Value, value.Value))))
                {
                    string name = nameof(this.GTLSCur);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GTLSCur);
                    ChangingDomainProperty = name; this.DomainObject.GTLSCur = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.GTLSCur : (decimal?)null; }
        }
        public decimal? GTLSRate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.GTLSRate.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.GTLSRate.Value, value.Value))))
                {
                    string name = nameof(this.GTLSRate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GTLSRate);
                    ChangingDomainProperty = name; this.DomainObject.GTLSRate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.GTLSRate : (decimal?)null; }
        }
        public string FilePath
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.FilePath, value)))
                {
                    string name = "FilePath";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FilePath);
                    ChangingDomainProperty = name; this.DomainObject.FilePath = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.FilePath : null; }
        }
        public decimal? FondSum { get { return this.DomainObject.FondSum; } }
        public Importer Importer { get { return this.IsEnabled ? this.DomainObject.Importer : null; } }
        public decimal Invoice { get { return this.DomainObject.Invoice; } }
        public decimal NetWeight { get { return this.DomainObject.NetWeight; } }
        public decimal? MFK
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.MFK.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.MFK.Value, value.Value))))
                {
                    string name = nameof(this.MFK);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.MFK);
                    ChangingDomainProperty = name; this.DomainObject.MFK = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.MFK : null; }
        }
        public decimal? MFKRate
        { get { return this.IsEnabled ? this.DomainObject.MFKRate : (decimal?)null; } }
        public decimal? MFKWithoutRate
        { get { return this.IsEnabled ? this.DomainObject.MFKWithoutRate : (decimal?)null; } }
        public Parcel Parcel
        {
            //set
            //{
            //    if (!(this.IsReadOnly || object.Equals(this.DomainObject.Parcel, value)))
            //    {
            //        string name = "Parcel";
            //        if (!myUnchangedPropertyCollection.ContainsKey(name))
            //            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Parcel);
            //        ChangingDomainProperty = name; this.DomainObject.Parcel = value;
            //    }
            //}
            get { return this.IsEnabled ? this.DomainObject.Parcel : null; }
        }
        public int? ParcelGroup
        {
            get { return this.IsEnabled ? this.DomainObject.ParcelGroup : null; }
        }
        public decimal? Pari
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Pari.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Pari.Value, value.Value))))
                {
                    string name = nameof(this.Pari);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Pari);
                    ChangingDomainProperty = name; this.DomainObject.Pari = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Pari : (decimal?)null; }
        }
        public Request Request
        {
            //set
            //{
            //    if (!(this.IsReadOnly || object.Equals(this.DomainObject.Request, value)))
            //    {
            //        string name = "Request";
            //        if (!myUnchangedPropertyCollection.ContainsKey(name))
            //            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Request);
            //        ChangingDomainProperty = name; this.DomainObject.Request = value;
            //    }
            //}
            get { return this.IsEnabled ? this.DomainObject.Request : null; }
        }
        public decimal? WestGate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.WestGate.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.WestGate.Value, value.Value))))
                {
                    string name = nameof(this.WestGate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.WestGate);
                    ChangingDomainProperty = name; this.DomainObject.WestGate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.WestGate : null; }
        }
        public decimal? WestGateRate
        { get { return this.IsEnabled ? this.DomainObject.WestGateRate : (decimal?)null; } }
        public decimal? WestGateWithoutRate
        { get { return this.IsEnabled ? this.DomainObject.WestGateWithoutRate : (decimal?)null; } }

        public bool TotalSumNotEquals
        {
            get
            {
                bool notequals = this.Declaration?.TotalSum != null || this.Cost.HasValue;
                if (notequals && (this.Declaration?.TotalSum == null || !this.Cost.HasValue || decimal.Round(this.Declaration.TotalSum.Value - this.Cost.Value, 2) == 0M))
                {
                    decimal? invoice = this.DomainObject.Invoice;
                    notequals = invoice.HasValue;
                    if (this.Declaration?.TotalSum != null)
                        notequals = notequals && decimal.Round(this.Declaration.TotalSum.Value - invoice.Value, 2) != 0M;
                    else
                        notequals = notequals && decimal.Round(this.Cost.Value - invoice.Value, 2) != 0M;
                }
                return notequals;
            }
        }
        public bool TDSumNotEquals
        { get { return this.Declaration?.TotalSum != null && this.TotalSumNotEquals; } }
        public bool DetailSumNotEquals
        { get { return this.Cost.HasValue && this.TotalSumNotEquals; } }
        private SpecificationDetailSynchronizer mysync;
        private ListCollectionView mydetails;
        public ListCollectionView Details
        {
            get
            {
                if (mysync == null)
                {
                    mysync = new SpecificationDetailSynchronizer();
                    if (this.DomainObject.DetailsIsNull)
                        this.DomainObject.DetailsGetAsync();
                    mysync.DomainCollection = this.DomainObject.Details;
                }
                if (mydetails == null)
                {
                    mydetails = new ListCollectionView(mysync.ViewModelCollection);
                    mydetails.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    mydetails.SortDescriptions.Add(new System.ComponentModel.SortDescription("RowOrder", System.ComponentModel.ListSortDirection.Ascending));
                    //mydetails.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
                }
                return mydetails;
            }
        }
        private ListCollectionView mycustomerlegalslist;
        public ListCollectionView CustomerLegalsList
        {
            get
            {
                if (mycustomerlegalslist == null)
                {
                    mycustomerlegalslist = new ListCollectionView(this.DomainObject.CustomerLegalsList);
                    mycustomerlegalslist.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
                }
                return mycustomerlegalslist;
            }
        }

        private RelayCommand myfileopen;
        public ICommand FileOpen
        {
            get { return myfileopen; }
        }
        private void FileOpenExec(object parametr)
        {
            try
            {
                System.Diagnostics.Process.Start(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.DetailsFileRoot, this.FilePath));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Не удалось открыть файл.\n" + ex.Message);
            }
        }
        private bool FileOpenCanExec(object parametr)
        { return !(this.IsReadOnly | string.IsNullOrEmpty(this.FilePath)); }

        protected override bool DirtyCheckProperty()
        {
            return false;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(Specification.Declaration):
                case nameof(Specification.Invoice):
                case nameof(Specification.Cost):
                    this.PropertyChangedNotification(nameof(this.TotalSumNotEquals));
                    this.PropertyChangedNotification(nameof(this.TDSumNotEquals));
                    this.PropertyChangedNotification(nameof(this.DetailSumNotEquals));
                    break;
            }
        }
        protected override void InitProperties()
        {
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "DependentNew":
                    int i = 0;
                    SpecificationDetailVM[] specremoved = new SpecificationDetailVM[this.Details.Count];
                    foreach (SpecificationDetailVM item in mysync.ViewModelCollection)
                    {
                        if (item.DomainState == lib.DomainObjectState.Added)
                        {
                            specremoved[i] = item;
                            i++;
                        }
                        else
                        {
                            this.Details.EditItem(item);
                            item.RejectChanges();
                            this.Details.CommitEdit();
                        }
                    }
                    foreach (SpecificationDetailVM item in specremoved)
                        if (item != null) mysync.ViewModelCollection.Remove(item);
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            return true;
        }
    }

    public class SpecificationSynchronizer : lib.ModelViewCollectionsSynchronizer<Specification, SpecificationVM>
    {
        protected override Specification UnWrap(SpecificationVM wrap)
        {
            return wrap.DomainObject as Specification;
        }
        protected override SpecificationVM Wrap(Specification fill)
        {
            return new SpecificationVM(fill);
        }
    }

    public class SpecificationVMCommand : lib.ViewModelCommand<Specification, SpecificationVM, SpecificationDBM>
    {
        public SpecificationVMCommand(SpecificationVM vm, ListCollectionView view) : base(vm, view)
        {
            myspecfolderopen = new RelayCommand(SpecFolderOpenExec, SpecFolderOpenCanExec);
            myspecadd = new RelayCommand(SpecAddExec, SpecAddCanExec);
            myselling1c = new RelayCommand(Selling1CExec, Selling1CCanExec);
        }

        private RelayCommand myspecfolderopen;
        public ICommand SpecFolderOpen
        {
            get { return myspecfolderopen; }
        }
        private void SpecFolderOpenExec(object parametr)
        {
            try
            {
                if (this.VModel != null)
                {
                    string path = CustomBrokerWpf.Properties.Settings.Default.DetailsFileRoot;
                    if (!Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                this.OpenPopup("Папка документов\n" + ex.Message, true);
            }
        }
        private bool SpecFolderOpenCanExec(object parametr)
        { return this.VModel != null; }

        private lib.TaskAsync.TaskAsync myExceltask;
        private RelayCommand myspecadd;
        public ICommand DetailLoad
        {
            get { return myspecadd; }
        }
        private void SpecAddExec(object parametr)
        {
            if (myExceltask == null)
                myExceltask = new lib.TaskAsync.TaskAsync();
            if (!myExceltask.IsBusy)
            {
                System.Text.StringBuilder path = new System.Text.StringBuilder();
                string rootdir = CustomBrokerWpf.Properties.Settings.Default.DetailsFileRoot;
                OpenFileDialog fd = new OpenFileDialog();
                fd.Multiselect = false;
                fd.CheckPathExists = true;
                fd.CheckFileExists = true;
                if (System.IO.Directory.Exists(CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault)) fd.InitialDirectory = CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault;
                fd.Title = "Выбор файла разбивки";
                fd.Filter = "Файлы Excel|*.xls;*.xlsx;*.xlsm;";
                if (fd.ShowDialog().Value)
                {
                    try
                    {
                        if (this.VModel.DomainObject.Details.Count > 0)
                        {
                            if (System.Windows.MessageBox.Show("Разбивка уже загружена. Перезаписать?", "Загрузка разбивок", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.No)
                                return;
                            else
                            {
                                ObservableCollection<SpecificationDetailVM> detsvm = this.VModel.Details.SourceCollection as ObservableCollection<SpecificationDetailVM>;
                                for (int i = 0; i < detsvm.Count; i++)
                                {
                                    if (detsvm[i].DomainState == lib.DomainObjectState.Added)
                                    {
                                        detsvm.RemoveAt(i);
                                        i--;
                                    }
                                    else
                                    {
                                        SpecificationDetailVM item = detsvm[i];
                                        this.VModel.Details.EditItem(item);
                                        item.DomainState = lib.DomainObjectState.Deleted;
                                        this.VModel.Details.CommitEdit();
                                    }
                                }
                            }
                        }
                        if (!System.IO.Directory.Exists(rootdir))
                            System.IO.Directory.CreateDirectory(rootdir);
                        if (string.IsNullOrEmpty(this.VModel.FilePath)) this.VModel.DomainObject.BuildFileName(fd.FileName);
                        path.Append(System.IO.Path.Combine(rootdir, this.VModel.FilePath));
                        if (System.IO.File.Exists(path.ToString()))
                        {
                            if (!fd.FileName.Equals(path.ToString(), StringComparison.InvariantCultureIgnoreCase))
                            {
                                System.IO.File.Delete(path.ToString());
                                System.IO.File.Copy(fd.FileName, path.ToString());
                            }
                        }
                        else
                            System.IO.File.Copy(fd.FileName, path.ToString());
                        if (CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault != System.IO.Path.GetDirectoryName(fd.FileName))
                        {
                            CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault = System.IO.Path.GetDirectoryName(fd.FileName);
                            CustomBrokerWpf.Properties.Settings.Default.Save();
                        }
                        myExceltask.DoProcessing = OnExcelImport;
                        myExceltask.Run(new object[2] { path.ToString(), this.VModel.DomainObject });
                    }
                    catch (Exception ex)
                    {
                        this.OpenPopup("Не удалось загрузить файл.\n" + ex.Message, true);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool SpecAddCanExec(object parametr)
        { return (myExceltask == null || !myExceltask.IsBusy); }
        private KeyValuePair<bool, string> OnExcelImport(object parm)
        {
            object[] param = parm as object[];
            string filepath = (string)param[0];
            Specification spec = (Specification)param[1];
            return new KeyValuePair<bool, string>(false, "Разбивка загружена. " + spec.ImportDetail(filepath, myExceltask).ToString() + " строк обработано.");
        }

        private RelayCommand myselling1c;
        public ICommand Selling1C
        {
            get { return myselling1c; }
        }
        private void Selling1CExec(object parametr)
        {
            this.VModel.DomainObject.Selling1C();
            this.VModel.DomainObject.Income1C();
        }
        private bool Selling1CCanExec(object parametr)
        { return !(this.VModel.IsReadOnly | string.IsNullOrEmpty(this.VModel.FilePath)); }

        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void RefreshData(object parametr)
        {
            SpecificationDBM dbm = new SpecificationDBM() { ItemId = this.VModel.Id };
            this.VModel.DomainObject.UpdateProperties(dbm.GetFirst());
            SpecificationDetailDBM ddbm = new SpecificationDetailDBM() { Specification = this.VModel.DomainObject };
            ddbm.Collection = this.VModel.DomainObject.Details;
            ddbm.FillAsyncCompleted = () => { if (ddbm.Errors.Count > 0) this.OpenPopup(ddbm.ErrorMessage, true); };
            ddbm.FillAsync();
        }
    }

    public class SpecificationViewCommand : lib.ViewModelViewCommand
    {
        internal SpecificationViewCommand()
        {
            //myfilter = new SQLFilter("Specification", "AND");
            //myfilter.GetDefaultFilter(SQLFilterPart.Where);
            mysdbm = new SpecificationDBM();
            mydbm = mysdbm;
            //mysdbm.Filter = myfilter.FilterWhereId;
            mysdbm.FillAsyncCompleted = () => { if (mysdbm.Errors.Count > 0) OpenPopup(mysdbm.ErrorMessage, true); };
            mysdbm.FillAsync();
            mysync = new SpecificationSynchronizer();
            mysync.DomainCollection = mysdbm.Collection;
            base.Collection = mysync.ViewModelCollection;
        }

        //private SQLFilter myfilter;
        //internal SQLFilter Filter
        //{ get { return myfilter; } }
        private SpecificationDBM mysdbm;
        private SpecificationSynchronizer mysync;

        protected override bool CanAddData(object parametr)
        {
            return true;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return true;
        }
        protected override bool CanRefreshData()
        {
            return true;
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
            mysdbm.FillAsync();
        }
        protected override void RejectChanges(object parametr)
        {
            System.Collections.IList rejects;
            if (parametr is System.Collections.IList && (parametr as System.Collections.IList).Count > 0)
                rejects = parametr as System.Collections.IList;
            else
                rejects = mysync.ViewModelCollection;

            System.Collections.Generic.List<SpecificationVM> deleted = new System.Collections.Generic.List<SpecificationVM>();
            foreach (object item in rejects)
            {
                if (item is SpecificationVM)
                {
                    SpecificationVM ritem = item as SpecificationVM;
                    if (ritem.DomainState == lib.DomainObjectState.Added)
                        deleted.Add(ritem);
                    else
                    {
                        myview.EditItem(ritem);
                        ritem.RejectChanges();
                        myview.CommitEdit();
                    }
                }
            }
            foreach (SpecificationVM delitem in deleted)
            {
                mysync.ViewModelCollection.Remove(delitem);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
            }
        }
        protected override void SettingView()
        {
        }
    }
}
