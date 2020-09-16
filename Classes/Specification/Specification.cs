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
            myinvoicedtrates = new Dictionary<int, SpecificationCustomerInvoiceRate>();
            myinvoicedtrateslock = new object();
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
        { set { SetProperty<Declaration>(ref mydeclaration, value); }
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
            get { return mygtlscur.HasValue && this.Parcel.UsdRate.HasValue ? mygtlscur * this.Parcel.UsdRate: mygtls; }
        }
        private decimal? mygtlscur;
        public decimal? GTLSCur
        {
            set { SetProperty<decimal?>(ref mygtlscur, value,()=> { this.PropertyChangedNotification(nameof(this.GTLS)); }); }
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
            set {
                Action action = () => { this.PropertyChangedNotification(nameof(this.MFKRate)); this.PropertyChangedNotification(nameof(this.MFKWithoutRate)); };
                SetProperty<decimal?>(ref mymfk, value, action); }
            get { return mymfk; }
        }
        public decimal? MFKRate
        { get { return mymfk * 20M / 120M; } }
        public decimal? MFKWithoutRate
        { get { return mymfk - this.MFKRate; } }
        private Parcel myparcel;
        public Parcel Parcel
        {
            set {
                SetProperty<Parcel>(ref myparcel, value, () => { if (myparcel != null) CustomersLegalsRefresh(); }); }
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
            set {
                Action action = () => { this.PropertyChangedNotification(nameof(this.WestGateRate)); this.PropertyChangedNotification(nameof(this.WestGateWithoutRate)); };
                SetProperty<decimal?>(ref mywestgate, value, action); }
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
                    myddbm = new SpecificationDetailDBM() { Specification = this };
                    myddbm.FillAsyncCompleted = () => { if (myddbm.Errors.Count > 0) throw (new Exception(myddbm.ErrorMessage)); else RefreshTotalDetails(); };
                    myddbm.FillAsync();
                    mydetails = myddbm.Collection;
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
                name.Append('_').Append((this.Request??this.Requests.First()).CustomerName);
            }
            else
                name.Append("_").Append(this.Consolidate);
            name.Append('_').Append(this.Agent.Name);
            name.Append(System.IO.Path.GetExtension(sourcepath));
            this.FilePath = name.ToString();
            return this.FilePath;
        }
        protected override void RejectProperty(string property, object value)
        {

        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Specification newitem = (Specification)sample;
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
            if (this.DetailsIsNull && newitem.Amount>0)
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
        private Dictionary<int, SpecificationCustomerInvoiceRate> myinvoicedtrates;
        internal Dictionary<int, SpecificationCustomerInvoiceRate> InvoiceDTRates
        { get { return myinvoicedtrates; } }
        private object myinvoicedtrateslock;
        internal void InvoiceDTRatesAdd(SpecificationCustomerInvoiceRate rate)
        {
            lock(myinvoicedtrateslock)
            {
                if (rate.CustomerId.HasValue && !myinvoicedtrates.ContainsKey(rate.CustomerId.Value))
                    myinvoicedtrates.Add(rate.CustomerId.Value, rate);
            }
        }
        internal SpecificationCustomerInvoiceRate GetCustomerInvoiceCostRate(CustomerLegal customer)
        {
            SpecificationCustomerInvoiceRate customercost;
            //lock (mythreadlock)
            //{
            //    CustomerLegal threadcustomer = customer;
            //    customercost = this.Details.Where((SpecificationDetail detail) => { return detail.Client == threadcustomer; }).Sum((SpecificationDetail detail) => { return detail.Cost; });
            //if(customercost.HasValue && customercost.Value>0.0099M)
            //{
            //    try
            //    { 
            //    decimal? requestcost = this.Requests.Sum(
            //        (Request request)=> { return
            //            request.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.CustomerLegal == threadcustomer; }).Sum(
            //                (RequestCustomerLegal legal)=>{ return
            //                    legal.Prepays.Sum((Domain.Account.PrepayCustomerRequest requestprepay) => { return
            //                       requestprepay.DtSumSet ?? requestprepay.EuroSum ; });
            //                }
            //            );
            //        }
            //    );
            //     customercost = requestcost / customercost;
            //       }
            //    catch { }
            //}
            return this.InvoiceDTRates.TryGetValue(customer.Id, out customercost) ? customercost: null;
            //}
        }
    }

    internal class SpecificationStore : lib.DomainStorageLoad<Specification, SpecificationDBM>
    {
        public SpecificationStore(SpecificationDBM dbm) : base(dbm) { }

        internal Specification GetItemLoad(Request request, SqlConnection conection, out List<lib.DBMError> errors)
        {
            //return Dispatcher.Invoke<Specification>(() =>
            //{
                Specification firstitem = default(Specification);
                errors = new List<lib.DBMError>();
            try
            {
                if (request.Parcel != null)
                {
                    if (myupdatingcoll > 0) System.Threading.Thread.Sleep(20);
                    myforcount++;
                    foreach (Specification item in mycollection.Values)
                        if (item.Parcel == request.Parcel && string.Equals(item.Consolidate, request.Consolidate)
                            && (!string.IsNullOrEmpty(request.Consolidate) || (item.ParcelGroup == request.ParcelGroup
                            && (request.ParcelGroup.HasValue || item.Request?.Id == request.Id))))
                        { firstitem = item; break; }
                    myforcount--;
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
                        dbm.Errors.Clear();
                        mydbmanagers.Enqueue(dbm);
                    }
                }
            }
            catch(Exception ex)
            {
                errors.Add(new lib.DBMError(request, ex.Message, "SpecificationStore.GetItemLoad"));
            }
                return firstitem;
            //});
        }
        internal Specification GetItemLoad(Request request, out List<lib.DBMError> errors)
        {
            return GetItemLoad( request, null, out errors);
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

        protected override Specification CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            List<lib.DBMError> errors;
            Agent agent = CustomBrokerWpf.References.AgentStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("agentid")), addcon,out errors);
            this.Errors.AddRange(errors);
            Declaration declaration = null;
            if (!reader.IsDBNull(reader.GetOrdinal("declarationid")))
            {
                mytddbm.Errors.Clear();
                mytddbm.Command.Connection = addcon;
                mytddbm.ItemId = reader.GetInt32(reader.GetOrdinal("declarationid"));
                declaration = mytddbm.GetFirst();
            }
            Parcel parcel = CustomBrokerWpf.References.ParcelStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("parcelid")), addcon, out errors);
            this.Errors.AddRange(errors);
            Request request = null;
            if (!reader.IsDBNull(reader.GetOrdinal("requestid")))
            {
                request = CustomBrokerWpf.References.RequestStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("requestid")), addcon, out errors);
                this.Errors.AddRange(errors);
            }
            Specification spec = new Specification(reader.GetInt32(0), reader.GetInt64(reader.GetOrdinal("stamp")), lib.DomainObjectState.Unchanged
                , agent
                , reader.IsDBNull(reader.GetOrdinal("consolidate")) ? null : reader.GetString(reader.GetOrdinal("consolidate"))
                , declaration
                , reader.IsDBNull(reader.GetOrdinal("filepath")) ? null : reader.GetString(reader.GetOrdinal("filepath"))
                , CustomBrokerWpf.References.Importers.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("importerid")))
                , parcel
                , reader.IsDBNull(reader.GetOrdinal("parcelgroup")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("parcelgroup"))
                , request
                , reader.IsDBNull(reader.GetOrdinal("pari")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("pari"))
                , reader.IsDBNull(reader.GetOrdinal("gtls")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("gtls"))
                , reader.IsDBNull(reader.GetOrdinal("gtlscur")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("gtlscur"))
                , reader.IsDBNull(reader.GetOrdinal("gtlsrate")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("gtlsrate"))
                , reader.IsDBNull(reader.GetOrdinal("ddspidy")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("ddspidy"))
                , reader.IsDBNull(reader.GetOrdinal("westgate")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("westgate"))
                , reader.IsDBNull(reader.GetOrdinal("mfk")) ? (decimal?)null : (decimal)reader.GetDecimal(reader.GetOrdinal("mfk"))
                , reader.IsDBNull(reader.GetOrdinal("amount")) ? 0 : reader.GetInt32(reader.GetOrdinal("amount"))
                , reader.IsDBNull(reader.GetOrdinal("cellnumber")) ? 0M : reader.GetDecimal(reader.GetOrdinal("cellnumber"))
                , reader.IsDBNull(reader.GetOrdinal("clientsumdiff")) ? 0M : reader.GetDecimal(reader.GetOrdinal("clientsumdiff"))
                , reader.IsDBNull(reader.GetOrdinal("cost")) ? 0M : reader.GetDecimal(reader.GetOrdinal("cost"))
                , reader.IsDBNull(reader.GetOrdinal("fondsum")) ? 0M : reader.GetDecimal(reader.GetOrdinal("fondsum"))
                , reader.IsDBNull(reader.GetOrdinal("grossweight")) ? 0M : reader.GetDecimal(reader.GetOrdinal("grossweight"))
                , reader.IsDBNull(reader.GetOrdinal("netweight")) ? 0M : reader.GetDecimal(reader.GetOrdinal("netweight"))
                );
            Specification specsore = CustomBrokerWpf.References.SpecificationStore.UpdateItem(spec);
            if (myparcel != null && myrequest==null )
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
            specsore.InvoiceDTRates.Clear();
            myratedbm.Command.Connection = addcon;
            myratedbm.Specification = specsore;
            myratedbm.Load();
            if(myratedbm.Errors.Count>0) foreach (lib.DBMError err in myratedbm.Errors) this.Errors.Add(err);

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
                        par.Value = myrequest?.Consolidate;
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
                        par.Value=item.HasPropertyOutdatedValue(nameof(Specification.FilePath));
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
            get { return this.IsEnabled ? this.DomainObject.Pari : (decimal?)null; } }
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
                if (notequals && (this.Declaration?.TotalSum == null || !this.Cost.HasValue || decimal.Round(this.Declaration.TotalSum.Value - this.Cost.Value,2) == 0M))
                {
                    decimal? invoice = this.DomainObject.Invoice;
                    notequals = invoice.HasValue;
                    if (this.Declaration?.TotalSum != null)
                        notequals = notequals && decimal.Round(this.Declaration.TotalSum.Value - invoice.Value, 2) != 0M;
                    else
                        notequals = notequals && decimal.Round(this.Cost.Value - invoice.Value,2) != 0M;
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
            switch(property)
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
