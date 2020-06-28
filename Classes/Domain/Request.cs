using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.References;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Specification;
using System.Text;
using System.Windows.Documents;
using KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Request : lib.DomainStampValueChanged
    {
        bool mycurrencypaid, myisspecification, myttlpayinvoice, myttlpaycurrency;
        byte? mystatedoc, mystateexc, mystateinv;
        short? mycellnumber;
        int? myagentid, mycustomerid, mycustomerlegal, myfreightid, myparcelid, myparcelgroup, mystoreid;
        decimal? myadditionalpay, myadditionalcost, myactualweight, mybringcost, mybringpay, mybrokercost, mybrokerpay, mycorrcost, mycorrpay, mycurrencyrate, mycurrencysum, mycustomscost, mycustomspay, mydeliverycost, mydeliverypay, mydtrate, mygoodvalue, myfreightcost, myfreightpay, myinsurancecost, myinsurancepay, myinvoice, myinvoicediscount, myofficialweight, mypreparatncost, mypreparatnpay, myselling, mysellingmarkup, mysellingmarkuprate, mysertificatcost, mysertificatpay, mytdcost, mytdpay, myvolume;
        DateTime myrequestdate;
        DateTime? mycurrencydate, mycurrencypaiddate, mygtddate, myshipplandate, mystoredate, mystoreinform;
        string myalgorithmnote1, myalgorithmnote2, mycolormark, myconsolidate, mycurrencynote, mycustomernote, mycargo, mydocdirpath, mygtd, myfullnumber, mymanagergroup, mymanagernote, myservicetype, mystorenote, mystorepoint;
        lib.ReferenceSimpleItem mystatus, myparceltype;
        private Parcel myparcel;
        private Importer myimporter;

        public Request() : this(id: lib.NewObjectId.NewId, stamp: 0, updated: null, updater: null, domainstate: lib.DomainObjectState.Added
            , agent: null, agentid: null, customerid: null, customerlegal: null, freightid: null
            , parcelgroup: null, parcelid: null, parceltype: CustomBrokerWpf.References.ParcelTypes.GetDefault()
            , status: CustomBrokerWpf.References.RequestStates.GetDefault(), storeid: null
            , cellnumber: null, statedoc: null, stateexc: null, stateinv: null
            , currencypaid: false, isspecification: false, ttlpayinvoice: false, ttlpaycurrency: false
            , additionalcost: null, additionalpay: null, actualweight: null
            , bringcost: null, bringpay: null
            , brokercost: null, brokerpay: null
            , currencyrate: null, currencysum: null
            , customscost: null, customspay: null
            , deliverycost: null, deliverypay: null, dtrate: null, goodvalue: null
            , freightcost: null, freightpay: null
            , insurancecost: null, insurancepay: null
            , invoice: null, invoicediscount: null, officialweight: null
            , preparatncost: null, preparatnpay: null
            , selling: null, sellingmarkup: null, sellingmarkuprate: 0.37M
            , sertificatcost: null, sertificatpay: null
            , tdcost: null, tdpay: null, volume: null
            , currencydate: null, currencypaiddate: null, gtddate: null, requestdate: DateTime.Now, shipplandate: CustomBrokerWpf.References.EndQuarter(DateTime.Today.AddDays(10)), specification: null, storedate: null, storeinform: null
            , algorithmnote1: "Свободное поле", algorithmnote2: null, cargo: null, colormark: null, consolidate: null, currencynote: null, customernote: null, docdirpath: null, gtd: null, fullnumber: null, managergroup: null, managernote: null, servicetype: null, storenote: null, storepoint: null
            , importer: null,manager: null
            )
        { }
        public Request(int id, Int64 stamp, DateTime? updated, string updater, lib.DomainObjectState domainstate
            , Agent agent, lib.ReferenceSimpleItem status, int? agentid, int? customerid, int? customerlegal, int? freightid, int? parcelgroup, int? parcelid, int? storeid
            , short? cellnumber, byte? statedoc, byte? stateexc, byte? stateinv
            , bool currencypaid, bool isspecification, bool ttlpayinvoice, bool ttlpaycurrency, lib.ReferenceSimpleItem parceltype
            , decimal? additionalcost, decimal? additionalpay
            , decimal? actualweight
            , decimal? bringcost, decimal? bringpay
            , decimal? brokercost, decimal? brokerpay
            , decimal? currencyrate, decimal? currencysum
            , decimal? customscost, decimal? customspay
            , decimal? deliverycost, decimal? deliverypay, decimal? dtrate
            , decimal? goodvalue
            , decimal? freightcost, decimal? freightpay
            , decimal? insurancecost, decimal? insurancepay
            , decimal? invoice, decimal? invoicediscount, decimal? officialweight
            , decimal? preparatncost, decimal? preparatnpay
            , decimal? selling, decimal? sellingmarkup, decimal? sellingmarkuprate
            , decimal? sertificatcost, decimal? sertificatpay
            , decimal? tdcost, decimal? tdpay, decimal? volume
            , DateTime? currencydate, DateTime? currencypaiddate, DateTime? gtddate, DateTime requestdate,DateTime? shipplandate, DateTime? specification, DateTime? storedate, DateTime? storeinform
            , string algorithmnote1, string algorithmnote2, string cargo, string colormark, string consolidate, string currencynote, string customernote, string docdirpath, string gtd, string fullnumber, string managergroup, string managernote, string servicetype, string storenote, string storepoint
            , Importer importer, Manager manager
           ) : base(id, stamp, updated, updater, domainstate)
        {
            myactualweight = actualweight;
            myadditionalcost = additionalcost;
            myadditionalpay = additionalpay;
            myagentid = agentid;
            myagent = agent;
            myalgorithmnote1 = algorithmnote1;
            myalgorithmnote2 = algorithmnote2;
            mybringcost = bringcost;
            mybringpay = bringpay;
            mybrokercost = brokercost;
            mybrokerpay = brokerpay;
            mycargo = cargo;
            mycellnumber = cellnumber;
            mycolormark = colormark;
            myconsolidate = consolidate;
            mycurrencydate = currencydate;
            mycurrencynote = currencynote;
            mycurrencypaid = currencypaid;
            mycurrencypaiddate = currencypaiddate;
            mycurrencyrate = currencyrate;
            mycurrencysum = currencysum;
            mycustomerid = customerid;
            mycustomerlegal = customerlegal;
            mycustomernote = customernote;
            mydeliverycost = deliverycost;
            mydeliverypay = deliverypay;
            mydocdirpath = docdirpath;
            mydtrate = dtrate;
            mygoodvalue = goodvalue;
            mygtd = gtd;
            mygtddate = gtddate;
            myfreightid = freightid;
            myfreightcost = freightcost;
            myfreightpay = freightpay;
            myfullnumber = fullnumber;
            myimporter = importer;
            myinsurancecost = insurancecost;
            myinsurancepay = insurancepay;
            myinvoice = invoice;
            myinvoicediscount = invoicediscount;
            myisspecification = isspecification;
            mymanagergroup = managergroup;
            mymanager = manager;
            mymanagernote = managernote;
            myofficialweight = officialweight;
            myparcelgroup = parcelgroup;
            myparcelid = parcelid;
            myparceltype = parceltype;
            mypreparatncost = preparatncost;
            mypreparatnpay = preparatnpay;
            myrequestdate = requestdate;
            myselling = selling;
            mysellingmarkup = sellingmarkup;
            mysellingmarkuprate = sellingmarkuprate;
            mysertificatcost = sertificatcost;
            mysertificatpay = sertificatpay;
            myservicetype = servicetype;
            myshipplandate = shipplandate;
            myspecificationdate = specification;
            mystatedoc = statedoc;
            mystateexc = stateexc;
            mystateinv = stateinv;
            mystatus = status;
            mystoredate = storedate;
            mystoreid = storeid;
            mystoreinform = storeinform;
            mystorenote = storenote;
            mystorepoint = storepoint;
            mytdcost = tdcost;
            mytdpay = tdpay;
            myttlpayinvoice = ttlpayinvoice;
            myttlpaycurrency = ttlpaycurrency;
            myvolume = volume;

            myinvoiceinvoice = 0M;
            mybalanceprepay = 0M;
            mybalancefinal = 0M;

            base.PropertyChanged += Request_PropertyChanged;
            this.LoadedPropertiesNotification.Add(nameof(this.CustomerLegalsNames));
            mylegalslock = new object();
        }

        public decimal? ActualWeight
        {
            set
            {
                base.SetPropertyOnValueChanged(ref myactualweight, value);
            }
            get { return myactualweight; }
        }
        private Agent myagent;
        public Agent Agent
        { set { SetProperty<Agent>(ref myagent, value); } get { return myagent; } }
        public int? AgentId
        {
            set
            {
                base.SetProperty<int?>(ref myagentid, value);
            }
            get { return myagentid; }
        }
        public string AlgorithmNote1
        {
            set
            {
                base.SetProperty<string>(ref myalgorithmnote1, value);
            }
            get { return myalgorithmnote1; }
        }
        public string AlgorithmNote2
        {
            set
            {
                base.SetProperty<string>(ref myalgorithmnote2, value);
            }
            get { return myalgorithmnote2; }
        }
        public string Cargo
        {
            set
            {
                base.SetProperty<string>(ref mycargo, value);
            }
            get { return mycargo; }
        }
        public short? CellNumber
        {
            set
            {
                base.SetPropertyOnValueChanged<short?>(ref mycellnumber, value);
            }
            get { return mycellnumber; }
        }
        public string ColorMark
        {
            set
            {
                base.SetProperty<string>(ref mycolormark, value);
            }
            get { return mycolormark; }
        }
        public string Consolidate
        {
            set
            {
                base.SetProperty<string>(ref myconsolidate, value);
            }
            get { return myconsolidate; }
        }
        public DateTime? CurrencyDate
        {
            set
            {
                base.SetProperty<DateTime?>(ref mycurrencydate, value);
            }
            get { return mycurrencydate; }
        }
        public string CurrencyNote
        {
            set
            {
                base.SetProperty<string>(ref mycurrencynote, value);
            }
            get { return mycurrencynote; }
        }
        public bool CurrencyPaid
        {
            set
            {
                base.SetProperty<bool>(ref mycurrencypaid, value);
            }
            get { return mycurrencypaid; }
        }
        public DateTime? CurrencyPaidDate
        {
            set
            {
                base.SetProperty<DateTime?>(ref mycurrencypaiddate, value);
            }
            get { return mycurrencypaiddate; }
        }
        public decimal? CurrencyRate
        {
            set
            {
                base.SetProperty<decimal?>(ref mycurrencyrate, value);
            }
            get { return mycurrencyrate; }
        }
        public decimal? CurrencySum
        {
            set
            {
                base.SetProperty<decimal?>(ref mycurrencysum, value);
            }
            get { return mycurrencysum; }
        }
        public int? CustomerId
        {
            set
            {
                base.SetProperty<int?>(ref mycustomerid, value, () => { mycustomername = null; base.PropertyChangedNotification("CustomerName"); this.CustomerLegalsRefresh(); });
            }
            get { return mycustomerid; }
        }
        private string mycustomername;
        public string CustomerName
        {
            get
            {
                if (mycustomername == null & this.CustomerId.HasValue)
                {
                    ReferenceDS refds = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
                    if (refds.tableCustomerName.Count == 0) refds.CustomerNameRefresh();
                    System.Data.DataRow[] rows = refds.tableCustomerName.Select("customerID=" + this.CustomerId.Value.ToString());
                    if (rows.Length > 0)
                        mycustomername = (rows[0] as ReferenceDS.tableCustomerNameRow).customerName;
                }
                return mycustomername;
            }
        }
        public int? CustomerLegal
        {
            set
            {
                base.SetProperty<int?>(ref mycustomerlegal, value);
            }
            get { return mycustomerlegal; }
        }
        private string mycustomerlegalsnames;
        public string CustomerLegalsNames
        {
            get
            {
                if (mycustomerlegalsnames == null)
                {
                    CustomerLegalsNamesFill();
                }
                return mycustomerlegalsnames;
            }
        }
        public string CustomerNote
        {
            set
            {
                base.SetProperty<string>(ref mycustomernote, value);
            }
            get { return mycustomernote; }
        }
        public string DocDirPath
        {
            set
            {
                base.SetProperty<string>(ref mydocdirpath, value);
            }
            get { return mydocdirpath; }
        }
        public decimal? DTRate
        {
            set
            {
                base.SetProperty<decimal?>(ref mydtrate, value);
            }
            get { return mydtrate; }
        }
        public int? FreightId
        {
            set
            {
                base.SetProperty<int?>(ref myfreightid, value);
            }
            get { return myfreightid; }
        }
        public decimal? GoodValue
        {
            set
            {
                base.SetProperty<decimal?>(ref mygoodvalue, value);
            }
            get { return mygoodvalue; }
        }
        public string GTD
        {
            set
            {
                base.SetProperty<string>(ref mygtd, value);
            }
            get { return mygtd; }
        }
        public DateTime? GTDDate
        {
            set
            {
                base.SetProperty<DateTime?>(ref mygtddate, value);
            }
            get { return mygtddate; }
        }
        public Importer Importer
        {
            set
            {
                base.SetPropertyOnValueChanged<Importer>(ref myimporter, value);
            }
            get { return myimporter; }
        }
        public decimal? Invoice
        {
            set
            {
                Action notify = () =>
                {
                    base.PropertyChangedNotification("InsuranceCost");
                    base.PropertyChangedNotification("InsurancePay");
                    //UpdateSingleLegal("Invoice");
                };
                base.SetPropertyOnValueChanged<decimal?>(ref myinvoice, value, notify);
            }
            get { return myinvoice; }
        }
        public decimal? InvoiceDiscount
        {
            set
            {
                base.SetPropertyOnValueChanged<decimal?>(ref myinvoicediscount, value);
            }
            get { return myinvoicediscount; }
        }
        public bool InvoiceDiscountFill
        { get {
                return this.CustomerLegals?.Count > 0 && this.CustomerLegals.Count((RequestCustomerLegal legal) => { return legal.Selected; }) > 0 /*&& this.CustomerLegals.Count((RequestCustomerLegal legal) => { return legal.Selected && (legal.Prepays == null || legal.Prepays.Count == 0 || legal.Prepays.Count((PrepayCustomerRequest prepay)=>{ return prepay.EuroSum == 0M; })>0); }) == 0*/;
            } }
        public bool IsSpecification
        {
            set
            {
                base.SetProperty<bool>(ref myisspecification, value);
            }
            get { return myisspecification; }
        }
        public string ManagerGroupName
        {
            internal set
            {
                base.SetProperty<string>(ref mymanagergroup, value);
            }
            get { return mymanagergroup; }
        }
        private Manager mymanager;
        public Manager Manager
        {
            set { SetProperty<Manager>(ref mymanager, value); }
            get { return mymanager; }
        }
        public string ManagerNote
        {
            set
            {
                base.SetProperty<string>(ref mymanagernote, value);
            }
            get { return mymanagernote; }
        }
        public decimal? OfficialWeight
        {
            set
            {
                base.SetPropertyOnValueChanged<decimal?>(ref myofficialweight, value, () =>
                {
                    UpdateSingleLegal("OfficialWeight");
                });
            }
            get { return myofficialweight; }
        }
        public Parcel Parcel
        {
            set
            {
                myparcelid = value?.Id;
                base.SetProperty<Parcel>(ref myparcel, value, () => { this.PropertyChangedNotification(nameof(this.ParcelId)); });
            }
            get
            {
                if (myparcel == null & myparcelid != null)
                {
                    myparcel = CustomBrokerWpf.References.ParcelStore.GetItemLoad(myparcelid.Value, out _);
                }
                return myparcel;
            }
        }
        public int? ParcelGroup
        {
            set
            {
                base.SetProperty<int?>(ref myparcelgroup, value);
            }
            get { return myparcelgroup; }
        }
        public int? ParcelId
        {
            set
            {
                myparcel = null;
                base.SetPropertyOnValueChanged<int?>(ref myparcelid, value,()=> { this.PropertyChangedNotification(nameof(this.Parcel)); });
            }
            get { return myparcel == null ? myparcelid : myparcel.Id; }
        }
        public string ParcelNumber
        { get { return myfullnumber; } }
        public bool ParcelIsNull
        {
            get { return myparcel == null; }
        }
        public lib.ReferenceSimpleItem ParcelType
        {
            set
            {
                base.SetProperty<lib.ReferenceSimpleItem>(ref myparceltype, value);
            }
            get { return myparceltype; }
        }
        public DateTime RequestDate
        {
            set
            {
                base.SetProperty<DateTime>(ref myrequestdate, value);
            }
            get { return myrequestdate; }
        }
        public decimal? Selling
        {
            set
            {
                base.SetProperty<decimal?>(ref myselling, value);
            }
            get { return myselling; }
        }
        public decimal? SellingMarkup
        {
            set
            {
                base.SetProperty<decimal?>(ref mysellingmarkup, value);
            }
            get { return mysellingmarkup; }
        }
        public decimal? SellingMarkupRate
        {
            set
            {
                base.SetProperty<decimal?>(ref mysellingmarkuprate, value);
            }
            get { return mysellingmarkuprate; }
        }
        public string ServiceType
        {
            set
            {
                base.SetProperty<string>(ref myservicetype, value);
            }
            get { return myservicetype; }
        }
        public DateTime? ShipPlanDate
        {
            set { SetProperty<DateTime?>(ref myshipplandate, value,()=> { if (value.HasValue) foreach (RequestCustomerLegal legal in this.CustomerLegals) foreach (PrepayCustomerRequest prepay in legal.Prepays) if (!prepay.Prepay.InvoiceDate.HasValue) prepay.Prepay.ShipPlanDate = value.Value; }); }
            get { return myshipplandate; }
        }
        private Specification.Specification myspecification;
        public Specification.Specification Specification
        {
            internal set
            {
                base.SetProperty<Specification.Specification>(ref myspecification, value);
            }
            get
            {
                if(myspecification==null & this.ParcelId.HasValue)
                {
                    myspecification = CustomBrokerWpf.References.SpecificationStore.GetItemLoad(this,out _) ?? new Specification.Specification(
                                        parcel: this.Parcel,
                                        consolidate: this.Consolidate,
 
                                       parcelgroup: string.IsNullOrEmpty(this.Consolidate) ? this.ParcelGroup : null,
                                        request: string.IsNullOrEmpty(this.Consolidate) & !this.ParcelGroup.HasValue ? this : null,
                                        agent: CustomBrokerWpf.References.AgentStore.GetItemLoad(this.AgentId.Value, out _),
                                        importer: this.Importer);
                    if(myspecification != null) PropertyChangedNotification(nameof(this.Specification));
                }
                return myspecification;
            }
        }
        internal Specification.Specification SpecificationInit
        { set { myspecification = value; this.PropertyChangedNotification(nameof(this.Specification)); } }
        internal bool SpecificationIsNull
        { get { return myspecification == null; } }
        private DateTime? myspecificationdate;
        public DateTime? SpecificationDate
        { set { SetProperty<DateTime?>(ref myspecificationdate, value); } get { return myspecificationdate; } }
        public byte? StateDoc
        {
            set
            {
                base.SetProperty<byte?>(ref mystatedoc, value);
            }
            get { return mystatedoc; }
        }
        public byte? StateExc
        {
            set
            {
                base.SetProperty<byte?>(ref mystateexc, value);
            }
            get { return mystateexc; }
        }
        public byte? StateInv
        {
            set
            {
                base.SetProperty<byte?>(ref mystateinv, value);
            }
            get { return mystateinv; }
        }
        public lib.ReferenceSimpleItem Status
        {
            set
            {
                base.SetProperty<lib.ReferenceSimpleItem>(ref mystatus, value);
            }
            get { return mystatus; }
        }
        public DateTime? StoreDate
        {
            set
            {
                base.SetProperty<DateTime?>(ref mystoredate, value, () => { base.PropertyChangedNotification("StorePointDate"); });
            }
            get { return mystoredate; }
        }
        public int? StoreId
        {
            set
            {
                base.SetProperty<int?>(ref mystoreid, value);
            }
            get { return mystoreid; }
        }
        public DateTime? StoreInform
        {
            set
            {
                base.SetProperty<DateTime?>(ref mystoreinform, value);
            }
            get { return mystoreinform; }
        }
        public string StoreNote
        {
            set
            {
                base.SetProperty<string>(ref mystorenote, value);
            }
            get { return mystorenote; }
        }
        public string StorePoint
        {
            set
            {
                base.SetProperty<string>(ref mystorepoint, value, () => { base.PropertyChangedNotification("StorePointDate"); });
            }
            get { return mystorepoint; }
        }
        public string StorePointDate
        {
            get { return ((string.IsNullOrEmpty(this.StorePoint) ? string.Empty : this.StorePoint + " ") + (this.StoreDate.HasValue ? this.StoreDate.Value.ToShortDateString() : string.Empty)); }
        }
        public bool TtlPayInvoice
        {
            set
            {
                base.SetProperty<bool>(ref myttlpayinvoice, value);
            }
            get { return myttlpayinvoice; }
        }
        public bool TtlPayCurrency
        {
            set
            {
                base.SetProperty<bool>(ref myttlpaycurrency, value);
            }
            get { return myttlpaycurrency; }
        }
        public decimal? Volume
        {
            set
            {
                base.SetPropertyOnValueChanged<decimal?>(ref myvolume, value);
            }
            get { return myvolume; }
        }

        #region Algorithm
        bool myalgloaded;
        private Algorithm.AlgorithmFormulaRequestCommand myalgorithmcmd;
        public Algorithm.AlgorithmFormulaRequestCommand AlgorithmCMD
        {
            get
            {
                if (!myalgloaded & myalgorithmcmd == null)
                {
                    myalgloaded = true;
                    myalgorithmcmd = new Algorithm.AlgorithmFormulaRequestCommand(this);
                    myalgloaded = false;
                }
                return myalgorithmcmd;
            }
        }
        bool myalgconloaded;
        private Algorithm.AlgorithmConsolidateCommand myalgorithmconcmd;
        public Algorithm.AlgorithmConsolidateCommand AlgorithmConCMD
        {
            get
            {
                if (!myalgconloaded & myalgorithmconcmd == null)
                {
                    myalgconloaded = true;
                    myalgorithmconcmd = new Algorithm.AlgorithmConsolidateCommand(this);
                    myalgconloaded = false;
                    if (!string.IsNullOrEmpty(this.Consolidate)) myalgorithmconcmd.RequestAttached(this);
                }
                return myalgorithmconcmd;
            }
        }
        //private decimal? myconcellnumber;
        //public decimal? ConCellNumber
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconcellnumber.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconcellnumber.Value, value.Value))))
        //        {
        //            myconcellnumber = value;
        //            this.PropertyChangedNotification("ConCellNumber");
        //        }
        //    }
        //    get
        //    {
        //        return myconcellnumber;
        //    }
        //}
        //private decimal? myconcorr;
        //public decimal? ConCorr
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconcorr.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconcorr.Value, value.Value))))
        //        {
        //            myconcorr = value;
        //            this.PropertyChangedNotification("ConCorr");
        //        }
        //    }
        //    get
        //    {

        //        return myconcorr;
        //    }
        //}
        //private decimal? myconcorrper;
        //public decimal? ConCorrPer
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconcorrper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconcorrper.Value, value.Value))))
        //        {
        //            myconcorrper = value;
        //            this.PropertyChangedNotification("ConCorrPer");
        //        }
        //    }
        //    get
        //    {

        //        return myconcorrper;
        //    }
        //}
        //private decimal? myconcost;
        //public decimal? ConCost
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconcost.Value, value.Value))))
        //        {
        //            myconcost = value;
        //            this.PropertyChangedNotification("ConCost");
        //        }
        //    }
        //    get
        //    {

        //        return myconcost;
        //    }
        //}
        //private decimal? myconcostper;
        //public decimal? ConCostPer
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconcostper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconcostper.Value, value.Value))))
        //        {
        //            myconcostper = value;
        //            this.PropertyChangedNotification("ConCostPer");
        //        }
        //    }
        //    get
        //    {

        //        return myconcostper;
        //    }
        //}
        //private decimal? myconcustomspay;
        //public decimal? ConCustomsPay
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconcustomspay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconcustomspay.Value, value.Value))))
        //        {
        //            myconcustomspay = value;
        //            this.PropertyChangedNotification("ConCustomsPay");
        //        }
        //    }
        //    get
        //    {

        //        return myconcustomspay;
        //    }
        //}
        //private decimal? myconcustomspayper;
        //public decimal? ConCustomsPayPer
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconcustomspayper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconcustomspayper.Value, value.Value))))
        //        {
        //            myconcustomspayper = value;
        //            this.PropertyChangedNotification("ConCustomsPayPer");
        //        }
        //    }
        //    get
        //    {

        //        return myconcustomspayper;
        //    }
        //}
        //private decimal? myconincome;
        //public decimal? ConIncome
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconincome.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconincome.Value, value.Value))))
        //        {
        //            myconincome = value;
        //            this.PropertyChangedNotification("ConIncome");
        //        }
        //    }
        //    get
        //    {

        //        return myconincome;
        //    }
        //}
        //private decimal? myconincomeper;
        //public decimal? ConIncomePer
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconincomeper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconincomeper.Value, value.Value))))
        //        {
        //            myconincomeper = value;
        //            this.PropertyChangedNotification("ConIncomePer");
        //        }
        //    }
        //    get
        //    {

        //        return myconincomeper;
        //    }
        //}
        //private decimal? myconinvoice;
        //public decimal? ConInvoice
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconinvoice.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconinvoice.Value, value.Value))))
        //        {
        //            myconinvoice = value;
        //            this.PropertyChangedNotification("ConInvoice");
        //        }
        //    }
        //    get
        //    {

        //        return myconinvoice;
        //    }
        //}
        //private decimal? myconinvoicediscount;
        //public decimal? ConInvoiceDiscount
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconinvoicediscount.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconinvoicediscount.Value, value.Value))))
        //        {
        //            myconinvoicediscount = value;
        //            this.PropertyChangedNotification("ConInvoiceDiscount");
        //        }
        //    }
        //    get
        //    {

        //        return myconinvoicediscount;
        //    }
        //}
        //private decimal? myconlogisticscost;
        //public decimal? ConLogisticsCost
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconlogisticscost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconlogisticscost.Value, value.Value))))
        //        {
        //            myconlogisticscost = value;
        //            this.PropertyChangedNotification("ConLogisticsCost");
        //        }
        //    }
        //    get
        //    {

        //        return myconlogisticscost;
        //    }
        //}
        //private decimal? myconlogisticspay;
        //public decimal? ConLogisticsPay
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconlogisticspay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconlogisticspay.Value, value.Value))))
        //        {
        //            myconlogisticspay = value;
        //            this.PropertyChangedNotification("ConLogisticsPay");
        //        }
        //    }
        //    get { return myconlogisticspay; }
        //}
        //private decimal? myconpay;
        //public decimal? ConPay
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconpay.Value, value.Value))))
        //        {
        //            myconpay = value;
        //            this.PropertyChangedNotification("ConPay");
        //        }
        //    }
        //    get
        //    {

        //        return myconpay;
        //    }
        //}
        //private decimal? myconpayper;
        //public decimal? ConPayPer
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconpayper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconpayper.Value, value.Value))))
        //        {
        //            myconpayper = value;
        //            this.PropertyChangedNotification("ConPayPer");
        //        }
        //    }
        //    get
        //    {

        //        return myconpayper;
        //    }
        //}
        //private decimal? myconvolume;
        //public decimal? ConVolume
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconvolume.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconvolume.Value, value.Value))))
        //        {
        //            myconvolume = value;
        //            this.PropertyChangedNotification("ConVolume");
        //        }
        //    }
        //    get { return myconvolume; }
        //}
        //private decimal? myconweight;
        //public decimal? ConWeight
        //{
        //    set
        //    {
        //        if (this.DomainState < lib.DomainObjectState.Deleted && (myconweight.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myconweight.Value, value.Value))))
        //        {
        //            myconweight = value;
        //            this.PropertyChangedNotification("ConWeight");
        //        }
        //    }
        //    get { return myconweight; }
        //}

        public decimal? AdditionalCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (myadditionalcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myadditionalcost.Value, value.Value))))
                {
                    myadditionalcost = value;
                    //AdditionalPay = myadditionalcost * 1.05M;
                    this.PropertyChangedNotification("AdditionalCost");
                }
            }
            get { return myadditionalcost; }
        }
        public decimal? AdditionalPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (myadditionalpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myadditionalpay.Value, value.Value))))
                {
                    myadditionalpay = value;
                    this.PropertyChangedNotification("AdditionalPay");
                }
            }
            get { return myadditionalpay; }
        }
        public decimal? BringCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mybringcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mybringcost.Value, value.Value))))
                {
                    mybringcost = value;
                    this.PropertyChangedNotification("BringCost");
                }
            }
            get { return mybringcost; }
        }
        public decimal? BringPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mybringpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mybringpay.Value, value.Value))))
                {
                    mybringpay = value;
                    this.PropertyChangedNotification("BringPay");
                }
            }
            get { return mybringpay; }
        }
        public decimal? BrokerCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mybrokercost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mybrokercost.Value, value.Value))))
                {
                    mybrokercost = value;
                    this.PropertyChangedNotification("BrokerCost");
                }
            }
            get { return mybrokercost; }
        }
        public decimal? BrokerPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mybrokerpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mybrokerpay.Value, value.Value))))
                {
                    mybrokerpay = value;
                    this.PropertyChangedNotification("BrokerPay");
                }
            }
            get { return mybrokerpay; }
        }
        public decimal? CorrCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mycorrcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycorrcost.Value, value.Value))))
                {
                    mycorrcost = value;
                    this.PropertyChangedNotification("CorrCost");
                }
            }
            get { return mycorrcost; }
        }
        public decimal? CustomsCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mycustomscost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycustomscost.Value, value.Value))))
                {
                    mycustomscost = value;
                    mycustomspay = value;
                    this.PropertyChangedNotification("CustomsCost");
                    this.PropertyChangedNotification("CustomsPay");
                }
            }
            get { return mycustomscost; }
        }
        public decimal? CustomsPay
        {
            //set
            //{
            //    base.SetProperty<decimal?>(ref mycustomspay, value);
            //}
            get { return mycustomspay; }
        }
        private decimal? mycustomspayinvoice;
        public decimal? CustomsPayInvoice
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mycustomspayinvoice.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycustomspayinvoice.Value, value.Value))))
                {
                    mycustomspayinvoice = value;
                    this.PropertyChangedNotification("CustomsPayInvoice");
                }
            }
            get { return mycustomspayinvoice; }
        }
        public decimal? DeliveryCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mydeliverycost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mydeliverycost.Value, value.Value))))
                {
                    mydeliverycost = value;
                    this.PropertyChangedNotification("DeliveryCost");
                }
            }
            get { return mydeliverycost; }
        }
        public decimal? DeliveryPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mydeliverypay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mydeliverypay.Value, value.Value))))
                {
                    mydeliverypay = value;
                    this.PropertyChangedNotification("DeliveryPay");
                }
            }
            get { return mydeliverypay; }
        }
        public decimal? FreightCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (myfreightcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myfreightcost.Value, value.Value))))
                {
                    myfreightcost = value;
                    this.PropertyChangedNotification("FreightCost");
                }
            }
            get { return myfreightcost; }
        }
        public decimal? FreightPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (myfreightpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myfreightpay.Value, value.Value))))
                {
                    myfreightpay = value;
                    this.PropertyChangedNotification("FreightPay");
                }
            }
            get { return myfreightpay; }
        }
        private decimal? myincome;
        public decimal? Income
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (myincome.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myincome.Value, value.Value))))
                {
                    myincome = value;
                    this.PropertyChangedNotification("Income");
                }
            }
            get
            {

                return myincome;
            }
        }
        private decimal? myincomem3;
        public decimal? IncomeM3
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (myincomem3.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myincomem3.Value, value.Value))))
                {
                    myincomem3 = value;
                    this.PropertyChangedNotification("IncomeM3");
                }
            }
            get
            {

                return myincomem3;
            }
        }
        private decimal? myincomepay;
        public decimal? IncomePay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (myincomepay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myincomepay.Value, value.Value))))
                {
                    myincomepay = value;
                    this.PropertyChangedNotification("IncomePay");
                }
            }
            get { return myincomepay; }
        }
        public bool IncomePayPoor
        { get { return (myincomepay ?? 999M) < 50M; } }
        public decimal? InsuranceCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (myinsurancecost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myinsurancecost.Value, value.Value))))
                {
                    myinsurancecost = value;
                    this.PropertyChangedNotification("InsuranceCost");
                }
            }
            get { return myinsurancecost; }
        }
        public decimal? InsurancePay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (myinsurancepay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myinsurancepay.Value, value.Value))))
                {
                    myinsurancepay = value;
                    this.PropertyChangedNotification("InsurancePay");
                }
            }
            get { return myinsurancepay; }
        }
        private decimal? mylog;
        public decimal? Log
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mylog.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mylog.Value, value.Value))))
                {
                    mylog = value;
                    this.PropertyChangedNotification("Log");
                }
            }
            get { return mylog; }
        }
        private decimal? mylogisticscost;
        public decimal? LogisticsCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mylogisticscost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mylogisticscost.Value, value.Value))))
                {
                    mylogisticscost = value;
                    this.PropertyChangedNotification("LogisticsCost");
                }
            }
            get
            {

                return mylogisticscost;
            }
        }
        private decimal? mylogisticspay;
        public decimal? LogisticsPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mylogisticspay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mylogisticspay.Value, value.Value))))
                {
                    mylogisticspay = value;
                    this.PropertyChangedNotification("LogisticsPay");
                }
            }
            get { return mylogisticspay; }
        }
        public decimal? PreparatnCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mypreparatncost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mypreparatncost.Value, value.Value))))
                {
                    mypreparatncost = value;
                    //PreparatnPay = mypreparatncost * 1.05M;
                    this.PropertyChangedNotification("PreparatnCost");
                }
            }
            get { return mypreparatncost; }
        }
        public decimal? PreparatnPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mypreparatnpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mypreparatnpay.Value, value.Value))))
                {
                    mypreparatnpay = value;
                    this.PropertyChangedNotification("PreparatnPay");
                }
            }
            get { return mypreparatnpay; }
        }
        public decimal? SertificatCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mysertificatcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mysertificatcost.Value, value.Value))))
                {
                    mysertificatcost = value;
                    this.PropertyChangedNotification("SertificatCost");
                }
            }
            get { return mysertificatcost; }
        }
        public decimal? SertificatPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mysertificatpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mysertificatpay.Value, value.Value))))
                {
                    mysertificatpay = value;
                    this.PropertyChangedNotification("SertificatPay");
                }
            }
            get { return mysertificatpay; }
        }
        public decimal? TDPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mytdpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mytdpay.Value, value.Value))))
                {
                    mytdpay = value;
                    this.PropertyChangedNotification("TDPay");
                }
            }
            get { return mytdpay; }
        }
        public decimal? TotalCost
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mytdcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mytdcost.Value, value.Value))))
                {
                    mytdcost = value;
                    this.PropertyChangedNotification("TotalCost");
                }
            }
            get { return mytdcost; }
        }
        public decimal? TotalPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mycorrpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycorrpay.Value, value.Value))))
                {
                    mycorrpay = value;
                    this.PropertyChangedNotification("TotalPay");
                }
            }
            get
            {
                var a = this.AlgorithmCMD;
                return mycorrpay;
            }
        }
        private decimal? mytotalpayinvoiceper;
        public decimal? TotalPayInvoicePer
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mytotalpayinvoiceper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mytotalpayinvoiceper.Value, value.Value))))
                {
                    mytotalpayinvoiceper = value;
                    this.PropertyChangedNotification("TotalPayInvoicePer");
                }
            }
            get { return mytotalpayinvoiceper; }
        }
        #endregion

        #region MailState
        private RequestMailState mymailstatestock;
        internal RequestMailState MailStateStock
        {
            get
            {
                if (mymailstatestock == null)
                {
                    mymailstatestock = new RequestMailState(this, 30);
                    mymailstatestock.PropertyChanged += MailStateStock_PropertyChanged;
                }
                return mymailstatestock;
            }
        }
        internal bool MailStateStockIsNull
        { get { return mymailstatestock == null; } }
        private void MailStateStock_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChangedNotification("MailStateStock" + e.PropertyName);
        }
        private RequestMailState mymailstatetakegoods9;
        internal RequestMailState MailStateTakeGoods9
        {
            get
            {
                if (mymailstatetakegoods9 == null)
                {
                    mymailstatetakegoods9 = new RequestMailState(this, 52);
                    mymailstatetakegoods9.PropertyChanged += MailStateTakeGoods9_PropertyChanged;
                }
                return mymailstatetakegoods9;
            }
        }
        internal bool MailStateTakeGoods9IsNull
        { get { return mymailstatetakegoods9 == null; } }
        private void MailStateTakeGoods9_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChangedNotification("MailStateTakeGoods9" + e.PropertyName);
        }
        #endregion

        private decimal myinvoiceinvoice;
        public decimal BalanceInvoice
        {
            get { return myinvoiceinvoice; }
        }
        private decimal mybalanceprepay;
        public decimal BalancePrepayments
        {
            get { return mybalanceprepay; }
        }
        private decimal mybalancefinal;
        public decimal BalanceFinal
        {
            get { return mybalancefinal; }
        }

        private object mylegalslock;
        private ObservableCollection<RequestCustomerLegal> mylegals;
        internal ObservableCollection<RequestCustomerLegal> CustomerLegals
        {
            set {
                mylegals = value;
                foreach (RequestCustomerLegal item in mylegals)
                    item.PropertyChanged += this.RequestCustomerLegal_PropertyChanged;
                this.PropertyChangedNotification(nameof(CustomerLegals));
                this.PropertyChangedNotification(nameof(InvoiceDiscountFill));
            }
            get
            {
                if (mylegals == null)
                {
                    mylegals = new ObservableCollection<RequestCustomerLegal>();
                    if (this.CustomerId.HasValue)
                    {
                        CustomerLegalsFill();
                    }
                }
                return mylegals;
            }
        }
        internal bool CustomerLegalsIsNull
        { get { return mylegals == null; } }
        //private ObservableCollection<RequestPayment> mypayments;
        //internal ObservableCollection<RequestPayment> Payments
        //{
        //    get
        //    {
        //        if (mypayments == null)
        //        {
        //            mypayments = new ObservableCollection<RequestPayment>();
        //            mypayments.CollectionChanged += Payments_CollectionChanged;
        //            RequestPaymentDBM pdbm = new RequestPaymentDBM();
        //            pdbm.Request = this;
        //            pdbm.Collection = mypayments;
        //            pdbm.Fill();
        //        }
        //        return mypayments;
        //    }
        //}
        //internal bool PaymentsIsNull
        //{ get { return mypayments == null; } }

        //private void Payments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    decimal action;
        //    action = e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add ? 1M : -1M;
        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        //    {
        //        foreach (RequestPayment item in e.NewItems)
        //        {
        //            JoinBalance(item, action);
        //        }
        //    }
        //    else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        //    {
        //        foreach (RequestPayment item in e.OldItems)
        //        {
        //            JoinBalance(item, action);
        //        }
        //    }
        //    else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
        //    {
        //        myinvoiceinvoice = 0M;
        //        mybalanceprepay = 0M;
        //        mybalancefinal = 0M;
        //        foreach (RequestPayment item in mypayments)
        //        {
        //            JoinBalance(item, 1M);
        //        }
        //    }
        //}

        //private void JoinBalance(RequestPayment item, decimal action)
        //{
        //    if (action > 0)
        //    {
        //        item.PropertyChanged += RequestPayment_PropertyChanged;
        //        item.ValueChanged += RequestPayment_ValueChanged;
        //    }
        //    else
        //    {
        //        item.ValueChanged -= RequestPayment_ValueChanged;
        //        item.PropertyChanged -= RequestPayment_PropertyChanged;
        //    }
        //    SetBalance(item, action);
        //}
        //private void SetBalance(RequestPayment item, decimal action)
        //{
        //    decimal doctype, sum;
        //    doctype = item.DocType == 1 ? -1M : 1M;
        //    sum = item.Sum * action * doctype;
        //    switch (item.PaymentType)
        //    {
        //        case 1:
        //            myinvoiceinvoice = myinvoiceinvoice + sum;
        //            PropertyChangedNotification("BalanceInvoice");
        //            break;
        //        case 2:
        //            mybalanceprepay = mybalanceprepay + sum;
        //            PropertyChangedNotification("BalancePrepayments");
        //            break;
        //        case 3:
        //            mybalancefinal = mybalancefinal + sum;
        //            PropertyChangedNotification("BalanceFinal");
        //            break;
        //    }
        //}

        //private void RequestPayment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    RequestPayment item = sender as RequestPayment;
        //    switch (e.PropertyName)
        //    {
        //        case "DocType":
        //            SetBalance(item, 2);
        //            break;
        //        case "DomainState":
        //            if (item.DomainState == lib.DomainObjectState.Deleted)
        //                SetBalance(item, -1);
        //            else if (item.DomainState == lib.DomainObjectState.Added)
        //                SetBalance(item, 1);
        //            break;
        //    }
        //}
        //private void RequestPayment_ValueChanged(object sender, lib.Interfaces.ValueChangedEventArgs<object> e)
        //{
        //    RequestPayment item = sender as RequestPayment;
        //    switch (e.PropertyName)
        //    {
        //        case "Sum":
        //            decimal doctype, sum;
        //            doctype = item.DocType == 1 ? -1M : 1M;
        //            sum = ((decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M)) * doctype;
        //            switch (item.PaymentType)
        //            {
        //                case 1:
        //                    myinvoiceinvoice = myinvoiceinvoice + sum;
        //                    PropertyChangedNotification("BalanceInvoice");
        //                    break;
        //                case 2:
        //                    mybalanceprepay = mybalanceprepay + sum;
        //                    PropertyChangedNotification("BalancePrepayments");
        //                    break;
        //                case 3:
        //                    mybalancefinal = mybalancefinal + sum;
        //                    PropertyChangedNotification("BalanceFinal");
        //                    break;
        //            }
        //            break;
        //    }
        //}

        public override bool IsDirty
        {
            get
            {
                bool dirty = base.IsDirty;
                if (this.myimporter != null)
                    dirty |= this.myimporter.IsDirty;
                if (!dirty && mylegals != null)
                    foreach (RequestCustomerLegal item in mylegals)
                    {
                        if (item.IsDirty)
                        { dirty = true;break; }
                        else
                            foreach(PrepayCustomerRequest prepay in item.Prepays)
                                if (prepay.IsDirty)
                                { dirty = true; break; }
                    }
                //if (!dirty && mypayments != null)
                //    foreach (RequestPayment item in mypayments)
                //        dirty |= item.IsDirty;
                return dirty;
            }
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "ActualWeight":
                    myactualweight = (decimal?)value;
                    break;
                case "AdditionalCost":
                    myadditionalcost = (decimal?)value;
                    break;
                case "AgentId":
                    myagentid = (int?)value;
                    break;
                case "AlgorithmNote1":
                    myalgorithmnote1 = (string)value;
                    break;
                case "AlgorithmNote2":
                    myalgorithmnote2 = (string)value;
                    break;
                case "BrokerCost":
                    mybrokercost = (decimal?)value;
                    break;
                case "Cargo":
                    mycargo = (string)value;
                    break;
                case "CellNumber":
                    mycellnumber = (short?)value;
                    break;
                case "ColorMark":
                    mycolormark = (string)value;
                    break;
                case "Consolidate":
                    myconsolidate = (string)value;
                    break;
                case "CurrencyDate":
                    mycurrencydate = (DateTime?)value;
                    break;
                case "CurrencyNote":
                    mycurrencynote = (string)value;
                    break;
                case "CurrencyPaid":
                    mycurrencypaid = (bool)value;
                    break;
                case "CurrencyPaidDate":
                    mycurrencypaiddate = (DateTime?)value;
                    break;
                case "CurrencyRate":
                    mycurrencyrate = (decimal?)value;
                    break;
                case "CurrencySum":
                    mycurrencysum = (decimal?)value;
                    break;
                case "CustomerId":
                    mycustomerid = (int?)value;
                    break;
                case "CustomerLegal":
                    mycustomerlegal = (int?)value;
                    break;
                case "CustomerNote":
                    mycustomernote = (string)value;
                    break;
                case "CustomsCost":
                    mycustomscost = (decimal?)value;
                    break;
                case "CustomsPay":
                    mycustomspay = (decimal?)value;
                    break;
                case "DeliveryCost":
                    mydeliverycost = (decimal?)value;
                    break;
                case "DeliveryPay":
                    mydeliverypay = (decimal?)value;
                    break;
                case "DTRate":
                    mydtrate = (decimal?)value;
                    break;
                case "FreightId":
                    myfreightid = (int?)value;
                    break;
                case "GoodValue":
                    mygoodvalue = (decimal?)value;
                    break;
                case "GTD":
                    mygtd = (string)value;
                    break;
                case "GTDDate":
                    mygtddate = (DateTime?)value;
                    break;
                case "Importer":
                    myimporter = (Importer)value;
                    break;
                case "Invoice":
                    myinvoice = (decimal?)value;
                    break;
                case "InvoiceDiscount":
                    myinvoicediscount = (decimal?)value;
                    break;
                case "IsSpecification":
                    myisspecification = (bool)value;
                    break;
                case nameof(this.Manager):
                    mymanager = (Manager)value;
                    break;
                case "ManagerNote":
                    mymanagernote = (string)value;
                    break;
                case "OfficialWeight":
                    myofficialweight = (decimal?)value;
                    break;
                case "ParcelGroup":
                    myparcelgroup = (int?)value;
                    break;
                case "ParcelId":
                    myparcelid = (int?)value;
                    break;
                case "ParcelType":
                    myparceltype = (lib.ReferenceSimpleItem)value;
                    break;
                case "PreparatnCost":
                    mypreparatncost = (decimal?)value;
                    break;
                case "RequestDate":
                    myrequestdate = (DateTime)value;
                    break;
                case "Selling":
                    myselling = (decimal?)value;
                    break;
                case "SellingMarkup":
                    mysellingmarkup = (decimal?)value;
                    break;
                case "SellingMarkupRate":
                    mysellingmarkuprate = (decimal?)value;
                    break;
                case "SertificatCost":
                    mysertificatcost = (decimal?)value;
                    break;
                case "ServiceType":
                    myservicetype = (string)value;
                    break;
                case nameof(this.ShipPlanDate):
                    myshipplandate = (DateTime?)value;
                    break;
                case "Specification":
                    myspecification = (Specification.Specification)value;
                    break;
                case nameof(Request.SpecificationDate):
                    myspecificationdate = (DateTime?)value;
                    break;
                case "StateDoc":
                    mystatedoc = (byte?)value;
                    break;
                case "StateExc":
                    mystateexc = (byte?)value;
                    break;
                case "StateInv":
                    mystateinv = (byte?)value;
                    break;
                case "Status":
                    mystatus = (lib.ReferenceSimpleItem)value;
                    break;
                case "StoreDate":
                    mystoredate = (DateTime?)value;
                    break;
                case "StoreId":
                    mystoreid = (int?)value;
                    break;
                case "StoreInform":
                    mystoreinform = (DateTime?)value;
                    break;
                case "StoreNote":
                    mystorenote = (string)value;
                    break;
                case "StorePoint":
                    mystorepoint = (string)value;
                    break;
                case "TotalCost":
                    mytdcost = (decimal?)value;
                    break;
                case "TtlPayInvoice":
                    myttlpayinvoice = (bool)value;
                    break;
                case "TtlPayCurrency":
                    myttlpaycurrency = (bool)value;
                    break;
                case "Volume":
                    myvolume = (decimal?)value;
                    break;
                case "DependentNew":
                    //int i = 0;
                    //if (mypayments != null)
                    //{
                    //    lib.DomainBaseClass[] additem = new lib.DomainBaseClass[mypayments.Count];
                    //    foreach (lib.DomainBaseClass item in mypayments)
                    //    {
                    //        if (item.DomainState == lib.DomainObjectState.Added)
                    //        { additem[i] = item; i++; }
                    //        else if (item.DomainState == lib.DomainObjectState.Deleted)
                    //        {
                    //            item.DomainState = lib.DomainObjectState.Unchanged;
                    //        }
                    //    }

                    //    for (int ii = 0; ii < i; ii++) mypayments.Remove(additem[ii] as RequestPayment);
                    //}
                    break;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Request newitem = (Request)sample;
            this.ActualWeight = newitem.ActualWeight;
            this.AdditionalCost = newitem.AdditionalCost;
            this.AdditionalPay = newitem.AdditionalPay;
            this.Agent = newitem.Agent;
            this.AgentId = newitem.AgentId;
            this.AlgorithmNote1 = newitem.AlgorithmNote1;
            this.AlgorithmNote2 = newitem.AlgorithmNote2;
            if (!this.HasPropertyOutdatedValue("BringCost")) this.BringCost = newitem.BringCost;
            if (!this.HasPropertyOutdatedValue("BringPay")) this.BringPay = newitem.BringPay;
            if (!this.HasPropertyOutdatedValue("BrokerCost")) this.BrokerCost = newitem.BrokerCost;
            if (!this.HasPropertyOutdatedValue("BrokerPay")) this.BrokerPay = newitem.BrokerPay;
            if (!this.HasPropertyOutdatedValue("Cargo")) this.Cargo = newitem.Cargo;
            if (!this.HasPropertyOutdatedValue("CellNumber")) this.CellNumber = newitem.CellNumber;
            if (!this.HasPropertyOutdatedValue("ColorMark")) this.ColorMark = newitem.ColorMark;
            if (!this.HasPropertyOutdatedValue("Consolidate")) this.Consolidate = newitem.Consolidate;
            if (!this.HasPropertyOutdatedValue("TotalCost")) this.TotalCost = newitem.TotalCost;
            if (!this.HasPropertyOutdatedValue("CurrencyDate")) this.CurrencyDate = newitem.CurrencyDate;
            if (!this.HasPropertyOutdatedValue("CurrencyNote")) this.CurrencyNote = newitem.CurrencyNote;
            if (!this.HasPropertyOutdatedValue("CurrencyPaid")) this.CurrencyPaid = newitem.CurrencyPaid;
            if (!this.HasPropertyOutdatedValue("CurrencyPaidDate")) this.CurrencyPaidDate = newitem.CurrencyPaidDate;
            if (!this.HasPropertyOutdatedValue("CurrencyRate")) this.CurrencyRate = newitem.CurrencyRate;
            if (!this.HasPropertyOutdatedValue("CurrencySum")) this.CurrencySum = newitem.CurrencySum;
            if (!this.HasPropertyOutdatedValue("CustomerId")) this.CustomerId = newitem.CustomerId; this.ManagerGroupName = newitem.ManagerGroupName;
            if (!this.HasPropertyOutdatedValue("CustomerLegal")) this.CustomerLegal = newitem.CustomerLegal;
            if (!this.HasPropertyOutdatedValue("CustomerNote")) this.CustomerNote = newitem.CustomerNote;
            //if (!this.HasPropertyOutdatedValue("CustomsCost")) this.CustomsCost = newitem.CustomsCost;
            //if (!this.HasPropertyOutdatedValue("CustomsPay")) this.CustomsPay = newitem.CustomsPay;
            if (!this.HasPropertyOutdatedValue("DeliveryCost")) this.DeliveryCost = newitem.DeliveryCost;
            if (!this.HasPropertyOutdatedValue("DeliveryPay")) this.DeliveryPay = newitem.DeliveryPay;
            if (!this.HasPropertyOutdatedValue("DocDirPath")) this.DocDirPath = newitem.DocDirPath;
            if (!this.HasPropertyOutdatedValue("DTRate")) this.DTRate = newitem.DTRate;
            if (!this.HasPropertyOutdatedValue("FreightId")) this.FreightId = newitem.FreightId;
            if (!this.HasPropertyOutdatedValue("FreightCost")) this.FreightCost = newitem.FreightCost;
            if (!this.HasPropertyOutdatedValue("FreightPay")) this.FreightPay = newitem.FreightPay;
            if (!this.HasPropertyOutdatedValue("GoodValue")) this.GoodValue = newitem.GoodValue;
            if (!this.HasPropertyOutdatedValue("GTD")) this.GTD = newitem.GTD;
            if (!this.HasPropertyOutdatedValue("GTDDate")) this.GTDDate = newitem.GTDDate;
            if (!this.HasPropertyOutdatedValue("Importer")) this.Importer = newitem.Importer;
            //if (!this.HasPropertyOutdatedValue("InsuranceCost")) this.InsuranceCost = newitem.InsuranceCost;
            //if (!this.HasPropertyOutdatedValue("InsurancePay")) this.InsurancePay = newitem.InsurancePay;
            if (!this.HasPropertyOutdatedValue("Invoice")) this.Invoice = newitem.Invoice;
            if (!this.HasPropertyOutdatedValue("InvoiceDiscount")) this.InvoiceDiscount = newitem.InvoiceDiscount;
            if (!this.HasPropertyOutdatedValue("IsSpecification")) this.IsSpecification = newitem.IsSpecification;
            if (!this.HasPropertyOutdatedValue("ManagerNote")) this.ManagerNote = newitem.ManagerNote;
            this.Manager = newitem.Manager;
            if (!this.HasPropertyOutdatedValue("OfficialWeight")) this.OfficialWeight = newitem.OfficialWeight;
            if (!this.HasPropertyOutdatedValue("ParcelGroup")) this.ParcelGroup = newitem.ParcelGroup;
            this.ParcelId = newitem.ParcelId;
            if (!(this.HasPropertyOutdatedValue("ParcelId") || string.Equals(myfullnumber, newitem.ParcelNumber))) myfullnumber = newitem.ParcelNumber; PropertyChangedNotification("ParcelNumber");
            if (!this.HasPropertyOutdatedValue("ParcelType")) this.ParcelType = newitem.ParcelType;
            if (!this.HasPropertyOutdatedValue("PreparatnCost")) this.PreparatnCost = newitem.PreparatnCost;
            if (!this.HasPropertyOutdatedValue("PreparatnPay")) this.PreparatnPay = newitem.PreparatnPay;
            if (!this.HasPropertyOutdatedValue("RequestDate")) this.RequestDate = newitem.RequestDate;
            if (!this.HasPropertyOutdatedValue("Selling")) this.Selling = newitem.Selling;
            if (!this.HasPropertyOutdatedValue("SellingMarkup")) this.SellingMarkup = newitem.SellingMarkup;
            if (!this.HasPropertyOutdatedValue("SellingMarkupRate")) this.SellingMarkupRate = newitem.SellingMarkupRate;
            if (!this.HasPropertyOutdatedValue("SertificatCost")) this.SertificatCost = newitem.SertificatCost;
            if (!this.HasPropertyOutdatedValue("SertificatPay")) this.SertificatPay = newitem.SertificatPay;
            if (!this.HasPropertyOutdatedValue("ServiceType")) this.ServiceType = newitem.ServiceType;
            this.ShipPlanDate = newitem.ShipPlanDate;
            if (!this.HasPropertyOutdatedValue("Specification")) this.Specification = newitem.Specification;
            this.SpecificationDate = newitem.SpecificationDate;
            if (!this.HasPropertyOutdatedValue("StateDoc")) this.StateDoc = newitem.StateDoc;
            if (!this.HasPropertyOutdatedValue("StateExc")) this.StateExc = newitem.StateExc;
            if (!this.HasPropertyOutdatedValue("StateInv")) this.StateInv = newitem.StateInv;
            if (!this.HasPropertyOutdatedValue("Status")) this.Status = newitem.Status;
            if (!this.HasPropertyOutdatedValue("StoreDate")) this.StoreDate = newitem.StoreDate;
            if (!this.HasPropertyOutdatedValue("StoreId")) this.StoreId = newitem.StoreId;
            if (!this.HasPropertyOutdatedValue("StoreInform")) this.StoreInform = newitem.StoreInform;
            if (!this.HasPropertyOutdatedValue("StoreNote")) this.StoreNote = newitem.StoreNote;
            if (!this.HasPropertyOutdatedValue("StorePoint")) this.StorePoint = newitem.StorePoint;
            if (!this.HasPropertyOutdatedValue("TtlPayInvoice")) this.TtlPayInvoice = newitem.TtlPayInvoice;
            if (!this.HasPropertyOutdatedValue("TtlPayCurrency")) this.TtlPayCurrency = newitem.TtlPayCurrency;
            if (!this.HasPropertyOutdatedValue("Volume")) this.Volume = newitem.Volume;
            this.UpdateIsOver = false;
            if (mymailstatestock != null) mymailstatestock.Update();
            if (mymailstatetakegoods9 != null) mymailstatetakegoods9.Update();
        }
        internal bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                case nameof(this.AgentId):
                    if (!this.CustomerLegalsIsNull && this.CustomerLegals.Count > 0 && value == null)
                    {
                        errmsg = "В заявке  " + this.StorePointDate + " необходимо указать поставщика!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.Importer):
                    if (!this.CustomerLegalsIsNull && this.CustomerLegals.Count>0 && value == null)
                    {
                        errmsg = "В заявке  " + this.StorePointDate + " необходимо указать импортера!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.InvoiceDiscount):
                    int legals = this.CustomerLegals?.Where((RequestCustomerLegal item) => { return item.Selected; }).Count() ?? 0;
                    if (((decimal?)value??0M)>0M && legals == 0)
                    {
                        errmsg = "У заявки  " + this.StorePointDate + " нет юр. лиц!";
                        isvalid = false;
                    }
                    if (((decimal?)value ?? 0M) > 0M && this.InvoiceDiscount!=(decimal?)value && legals>1) //( || (this.CustomerLegals?.Where((RequestCustomerLegal item) => { return item.Selected; }).Sum((RequestCustomerLegal item) => { return item.Prepays.Count; })??0) > 1)
                    {
                        errmsg = "У заявки  "+ this.StorePointDate + " несколько юр. лиц! Для изменения суммы воспользуйтесь разделом оплат в карточке заявки.";
                        isvalid = false;
                    }
                    break;
                case nameof(this.ShipPlanDate):
                    if(!(this.CustomerLegalsIsNull || this.CustomerLegals.Count == 0 || ((DateTime?)value).HasValue))
                    {
                        errmsg = "В заявке  " + this.StorePointDate + " необходимо указать предполагаемую дату отгрузки!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.ServiceType):
                    if(string.IsNullOrEmpty((string)value) && !this.CustomerLegalsIsNull && this.CustomerLegals.Count > 0 && (this.CustomerLegals?.Where((RequestCustomerLegal item) => { return item.Selected; }).Sum((RequestCustomerLegal item) => { return item.Prepays.Sum((PrepayCustomerRequest prepay) => { return prepay.EuroSum; }); }) ?? 0M) > 0M)
                    {
                        errmsg = "В заявке  " + this.StorePointDate + " необходимо указать услугу!";
                        isvalid = false;
                    }
                    break;
            }
            return isvalid;
        }

        internal string CustomerLegalsFill(RequestCustomerLegalDBM ldbm=null)
        {
            lock (mylegalslock)
            {
                if (mylegals == null && ldbm == null) return string.Empty;
                if (ldbm == null) { App.Current.Dispatcher.Invoke(() => { ldbm = new RequestCustomerLegalDBM(); }); ldbm.FillType = lib.FillType.Refresh; }
                ldbm.Request = this;
                if (mylegals != null) ldbm.Collection = mylegals;
                ldbm.Fill();
                mylegals = ldbm.Collection;
                foreach (RequestCustomerLegal item in mylegals)
                {
                    item.PropertyChanged -= RequestCustomerLegal_PropertyChanged;
                    item.PropertyChanged += RequestCustomerLegal_PropertyChanged;
                    if(!item.CustomsInvoiceIsNull)
                    {

                    }
                }
            }
            this.PropertyChangedNotification(nameof(CustomerLegals));
            this.PropertyChangedNotification(nameof(InvoiceDiscountFill));
            return ldbm.ErrorMessage;
        }
        //internal void PaymentsFill()
        //{
        //    if (mypayments == null) return;
        //    RequestPaymentDBM pdbm = new RequestPaymentDBM();
        //    pdbm.Request = this;
        //    pdbm.Collection = mypayments;
        //    pdbm.Fill();
        //}
        internal void CustomerLegalsRefresh(RequestCustomerLegalDBM ldbm = null)
        {
            CustomerLegalsFill(ldbm);
            //PaymentsFill();
            CustomerLegalsNamesFill();
            this.PropertyChangedNotification("CustomerLegals");
            this.PropertyChangedNotification("CustomerLegalsSelected");
        }
        private void RequestCustomerLegal_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
            { CustomerLegalsNamesFill(); this.PropertyChangedNotification("CustomerLegalsSelected");  }
        }
        private void UpdateSingleLegal(string PropertyName)
        {
            int n = 0;
            RequestCustomerLegal single = null;
            foreach (RequestCustomerLegal item in this.CustomerLegals)
            {
                if (item.Selected)
                {
                    if (n == 0)
                        single = item;
                    n++;
                }
            }
            if (n == 1 & single != null)
            {
                switch (PropertyName)
                {
                    //case "Invoice":
                    //   single.Invoice = myinvoice;
                    //    break;
                    case "InvoiceDiscount":
                        single.UpdateInvoiceDiscount(myinvoicediscount,'r');
                        this.InvoiceDiscount = single.InvoiceDiscount; // if update is fail
                        break;
                        //case "OfficialWeight":
                        //    single.OfficialWeight = myofficialweight;
                        //    break;
                }
            }
            else
            {
                switch (PropertyName)
                {
                    case "Invoice":
                        if ((myinvoice??0M) == 0M)
                            foreach (RequestCustomerLegal item in this.CustomerLegals)
                                if (item.Selected) { item.Invoice = 0M; }
                        break;
                    case "InvoiceDiscount":
                        if ((myinvoicediscount ?? 0M) == 0M)
                        {
                            decimal? result = 0M;
                            foreach (RequestCustomerLegal item in this.CustomerLegals)
                                if (item.Selected) { item.UpdateInvoiceDiscount(0M,'r'); result += item.InvoiceDiscount; }
                            this.InvoiceDiscount = result;
                        }
                        break;
                    case "OfficialWeight":
                        if ((myofficialweight??0M) == 0M)
                            foreach (RequestCustomerLegal item in this.CustomerLegals)
                                if (item.Selected) { item.OfficialWeight = 0M; }
                        break;
                }
            }
        }
        internal void UpdateInvoiceDiscount(decimal? value,byte entry)
        {
            decimal? oldvalue = myinvoicediscount;
            this.InvoiceDiscount = value;
            if (entry == 0 & oldvalue != myinvoicediscount)
                UpdateSingleLegal("InvoiceDiscount");
        }
        internal string UpdateDocDirPath()
        {
            try
            {
                if (this.CustomerId.HasValue & ((!string.IsNullOrEmpty(this.StorePoint) & this.StoreDate.HasValue) | this.ParcelGroup.HasValue))
                {
                    string path, pathfalse, docdirpath;
                    if (this.ParcelId.HasValue)
                        docdirpath = "Отправки\\" + this.Parcel.DocDirPath + "\\" + this.CustomerName + "_" + (this.ParcelGroup.HasValue ? this.ParcelGroup.ToString() : this.StorePointDate);
                    else
                        docdirpath = "Прямые\\" + this.CustomerName + "_" + (this.ParcelGroup.HasValue ? this.ParcelGroup.ToString() : this.StorePointDate);
                    path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + docdirpath;
                    if (!string.Equals(docdirpath, this.DocDirPath))
                    {
                        pathfalse = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + this.DocDirPath;
                        if (!string.IsNullOrEmpty(this.DocDirPath) & System.IO.Directory.Exists(pathfalse))
                        {
                            if (System.IO.Directory.Exists(path))
                            {
                                foreach (string movepath in System.IO.Directory.EnumerateDirectories(pathfalse))
                                {
                                    System.IO.DirectoryInfo movethis = new System.IO.DirectoryInfo(movepath);
                                    movethis.MoveTo(path + "\\" + movethis.Name);
                                }
                                foreach (string movepath in System.IO.Directory.EnumerateFiles(pathfalse))
                                {
                                    System.IO.FileInfo movethis = new System.IO.FileInfo(movepath);
                                    movethis.MoveTo(path + "\\" + movethis.Name);
                                }
                                System.IO.Directory.Delete(pathfalse);
                            }
                            else
                                System.IO.Directory.Move(pathfalse, path);
                        }
                        this.DocDirPath = docdirpath;
                    }
                    else if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "Не удалось сохранить папку заявки " + this.StorePointDate + "!\nЗакройте все документы из этой папки и повторите сохранение.\n\n" + ex.Message;
            }
        }
        private void CustomerLegalsNamesFill()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (RequestCustomerLegal item in this.CustomerLegals)
            {
                if (item.Selected)
                {
                    if (str.Length > 0)
                        str.Append(", ");
                    str.Append(item.CustomerLegal?.Name);
                }
            }
            mycustomerlegalsnames = str.ToString();
            this.PropertyChangedNotification(nameof(this.CustomerLegalsNames));
        }
        #region Blocking
        private RequestDBM mydbm;
        private lib.Common.BlockingDBM myblockingdbm;
        private void Request_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DomainState")
            {
                if (this.DomainStatePrevious == lib.DomainObjectState.Unchanged & (this.DomainState == lib.DomainObjectState.Modified | this.DomainState == lib.DomainObjectState.Deleted))
                {
                    Blocking();
                }
                else if (this.DomainStatePrevious == lib.DomainObjectState.Modified | this.DomainStatePrevious == lib.DomainObjectState.Deleted)
                    UnBlocking();
            }
        }
        internal bool HoldBlocking { set; get; }
        private bool myblocked;
        private System.Windows.Controls.Primitives.Popup mypopupblock;
        public bool Blocked { private set { myblocked = value;this.PropertyChangedNotification("Blocked"); } get { return myblocked; } }
        internal bool Blocking()
        {
            if (this.DomainState == lib.DomainObjectState.Added) return true;
            if (myblockingdbm == null)
                myblockingdbm = new lib.Common.BlockingDBM(CustomBrokerWpf.References.ConnectionString, "reqst", this.Id);
            string msg = myblockingdbm.Lock();
            if (string.IsNullOrEmpty(msg))
            {
                if (mydbm == null)
                {
                    mydbm = new RequestDBM();
                    mydbm.ItemId = this.Id;
                }
                this.Blocked = true;
                if (CustomBrokerWpf.References.RequestStore.GetItem(this.Id) != null) mydbm.GetFirst();
            }
            else
            {
                this.RejectChanges();
                Window active = null;
                foreach (Window win in Application.Current.Windows)
                    if (win.IsActive) { active = win; break; }
                active.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                {
                    if (mypopupblock == null || !mypopupblock.IsOpen)
                    { mypopupblock = Common.PopupCreator.GetPopup(text: msg.Replace("Объект", "Заявка " + this.StorePointDate)
                     , background: System.Windows.Media.Brushes.LightPink
                     , foreground: System.Windows.Media.Brushes.Red
                     , staysopen: true
                     );
                        mypopupblock.IsOpen = true;
                    }
                }));
            }
            return string.IsNullOrEmpty(msg);
        }
        internal void UnBlocking()
        {
            if (!this.HoldBlocking && (this.DomainState == lib.DomainObjectState.Unchanged | this.DomainState == lib.DomainObjectState.Destroyed))
            {
                if (myblockingdbm == null)
                    myblockingdbm = new lib.Common.BlockingDBM(CustomBrokerWpf.References.ConnectionString, "reqst", this.Id);
                myblockingdbm.UnLock();
                this.Blocked = false;
            }
        }
        internal void Refresh()
        {
            if (mydbm == null)
            {
                mydbm = new RequestDBM();
                mydbm.ItemId = this.Id;
            }
            mydbm.GetFirst();
        }
        #endregion
    }

    public class RequestDBM : lib.DBManagerWhoWhen<Request>
    {
        public RequestDBM()
        {
            base.NeedAddConnection = true;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.Request_sp";
            InsertCommandText = "dbo.RequestAdd_sp";
            UpdateCommandText = "dbo.RequestUpd_sp";
            DeleteCommandText = "dbo.RequestDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int), new SqlParameter("@filterId", System.Data.SqlDbType.Int), new SqlParameter("@parcel", System.Data.SqlDbType.Int), new SqlParameter("@datechanged", System.Data.SqlDbType.DateTime) };
            base.SelectParams[1].Value = 0;
            myinsertparams[0].ParameterName = "@requestId";
            myupdateparams[0].ParameterName = "@requestId";
            SqlParameter parstatus = new SqlParameter("@status", System.Data.SqlDbType.Int); parstatus.Direction = System.Data.ParameterDirection.InputOutput;
            SqlParameter parmngrgr = new SqlParameter("@managerGroup", System.Data.SqlDbType.NVarChar, 20); parmngrgr.Direction = System.Data.ParameterDirection.Output;
            myinsertparams = new SqlParameter[]
           {
                myinsertparams[0]
                ,new SqlParameter("@requestDate", System.Data.SqlDbType.Date)
           };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@parcel", System.Data.SqlDbType.Int)
                ,new SqlParameter("@isspecification", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@statustrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@specificationtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@storagePointtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@storageDatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@customerIdtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@customerlegaltrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@loadDescriptiontrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@agentIdtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@storeidtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@cellNumbertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@officialWeighttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@actualWeighttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@volumetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@goodValuetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@freighttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@storageNotetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@managerNotetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@customerNotetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@parcelgrouptrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@colorMarktrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@eurousdtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@pay1true", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@specloadedtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@parceltypetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicediscounttrue", System.Data.SqlDbType.Bit)
                //,new SqlParameter("@customscosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@deliverycosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@brokercosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@insurancecosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@freightcosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@sertificatcosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@additionalcosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@preparatncosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@tdcosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@bringcosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@corrcosttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@customspaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@deliverypaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@brokerpaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@insurancepaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@freightpaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@sertificatpaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@additionalpaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@preparatnpaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@tdpaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@bringpaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@corrpaytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@servicetypetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencyratetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencysumtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencydatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencypaidtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencypaiddatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencynotetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@sellingtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@sellingmarkuptrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@sellingmarkupratetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@gtdtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@gtddatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@dtratetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@parceltrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@isspecificationtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@statedoctrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@stateexctrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@stateinvtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@docdirpathtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@ttlpayinvoicetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@ttlpaycurrencytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@importertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@consolidatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@algorithmnote1true", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@algorithmnote2true", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@shipplandatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@manageridtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@old", false)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0],myinsertupdateparams[1],myinsertupdateparams[2]
                ,parmngrgr,parstatus
                ,new SqlParameter("@specification", System.Data.SqlDbType.Date)
                ,new SqlParameter("@storagePoint", System.Data.SqlDbType.NChar,6)
                ,new SqlParameter("@storageDate", System.Data.SqlDbType.Date)
                ,new SqlParameter("@customerId", System.Data.SqlDbType.Int)
                ,new SqlParameter("@customerlegal", System.Data.SqlDbType.Int)
                ,new SqlParameter("@loadDescription", System.Data.SqlDbType.NVarChar,50)
                ,new SqlParameter("@agentId", System.Data.SqlDbType.Int)
                ,new SqlParameter("@storeid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@cellNumber", System.Data.SqlDbType.SmallInt)
                ,new SqlParameter("@officialWeight", System.Data.SqlDbType.SmallMoney)
                ,new SqlParameter("@actualWeight", System.Data.SqlDbType.SmallMoney)
                ,new SqlParameter("@volume", System.Data.SqlDbType.SmallMoney)
                ,new SqlParameter("@goodValue", System.Data.SqlDbType.Money)
                ,new SqlParameter("@freight", System.Data.SqlDbType.Int)
                ,new SqlParameter("@storageNote", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@managerNote", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@customerNote", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@parcelgroup", System.Data.SqlDbType.Int)
                ,new SqlParameter("@colorMark", System.Data.SqlDbType.NChar,9)
                ,new SqlParameter("@eurousd", System.Data.SqlDbType.Money)
                ,new SqlParameter("@pay1", System.Data.SqlDbType.Money)
                ,new SqlParameter("@specloaded", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@parceltype", System.Data.SqlDbType.TinyInt)
                ,new SqlParameter("@invoice", System.Data.SqlDbType.Money)
                ,new SqlParameter("@invoicediscount", System.Data.SqlDbType.Money)
                //,new SqlParameter("@customscost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@deliverycost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@brokercost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@insurancecost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@freightcost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@sertificatcost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@additionalcost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@preparatncost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@tdcost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@bringcost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@corrcost", System.Data.SqlDbType.Money)
                ,new SqlParameter("@customspay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@deliverypay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@brokerpay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@insurancepay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@freightpay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@sertificatpay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@additionalpay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@preparatnpay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@tdpay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@bringpay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@corrpay", System.Data.SqlDbType.Money)
                ,new SqlParameter("@servicetype", System.Data.SqlDbType.NVarChar,10)
                ,new SqlParameter("@currencyrate", System.Data.SqlDbType.Money)
                ,new SqlParameter("@currencysum", System.Data.SqlDbType.Money)
                ,new SqlParameter("@currencydate", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@currencypaid", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencypaiddate", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@currencynote", System.Data.SqlDbType.NVarChar,200)
                ,new SqlParameter("@selling", System.Data.SqlDbType.Money)
                ,new SqlParameter("@sellingmarkup", System.Data.SqlDbType.Money)
                ,new SqlParameter("@sellingmarkuprate", System.Data.SqlDbType.Money)
                ,new SqlParameter("@gtd", System.Data.SqlDbType.NVarChar,25)
                ,new SqlParameter("@gtddate", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@dtrate", System.Data.SqlDbType.Money)
                ,new SqlParameter("@statedoc", System.Data.SqlDbType.TinyInt)
                ,new SqlParameter("@stateexc", System.Data.SqlDbType.TinyInt)
                ,new SqlParameter("@stateinv", System.Data.SqlDbType.TinyInt)
                ,new SqlParameter("@docdirpath", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@ttlpayinvoice", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@ttlpaycurrency", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@importer", System.Data.SqlDbType.Int)
                ,new SqlParameter("@consolidate", System.Data.SqlDbType.NVarChar,5)
                ,new SqlParameter("@algorithmnote1", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@algorithmnote2", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@shipplandate", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@managerid", System.Data.SqlDbType.Int)
            };
            mydeleteparams = new SqlParameter[]
            {
                mydeleteparams[0]
                ,new SqlParameter("@stamp", System.Data.SqlDbType.Int)
            };

            //mypmdbm = new RequestPaymentDBM(); mypmdbm.Command = new SqlCommand();
            myldbm = new RequestCustomerLegalDBM();
            myspdbm = new SpecificationDBM();
        }

        private ParcelDBM mypdbm;
        internal ParcelDBM ParcelDBM
        { set { mypdbm = value; } get { return mypdbm; } }
        //private RequestPaymentDBM mypmdbm;
        private RequestCustomerLegalDBM myldbm;
        internal RequestCustomerLegalDBM LegalDBM
        {
            set { myldbm = value; }
            get { return myldbm; }
        }
        private SpecificationDBM myspdbm;
        internal SpecificationDBM SpecificationDBM
        { set { myspdbm = value; } get { return myspdbm; } }

        internal int Filter
        { set { base.SelectParams[1].Value = value; } get { return (int)base.SelectParams[1].Value; } }
        internal int Parcel
        {
            set { base.SelectParams[2].Value = value; }
            get { return (int)base.SelectParams[2].Value; }
        }
        internal DateTime? UpdateWhen
        {
            set { base.SelectParams[3].Value = value; }
            get { return (DateTime?)base.SelectParams[3].Value; }
        }
        internal bool SpecificationLoad { set; get; }

        protected override Request CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
			System.Collections.Generic.List<lib.DBMError> errors=new System.Collections.Generic.List<DBMError>();
            Agent agent = reader.IsDBNull(reader.GetOrdinal("agentId")) ? null : CustomBrokerWpf.References.AgentStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("agentId")), addcon, out errors);
            this.Errors.AddRange(errors);
            Request newitem = new Request(reader.GetInt32(0), reader.GetInt32(reader.GetOrdinal("stamp")), reader.GetDateTime(reader.GetOrdinal("UpdateWhen")), reader.GetString(reader.GetOrdinal("UpdateWho")), lib.DomainObjectState.Unchanged
                ,agent
                , CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("status")))
                , reader.IsDBNull(reader.GetOrdinal("agentId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("agentId"))
                , reader.IsDBNull(reader.GetOrdinal("customerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("customerId"))
                , reader.IsDBNull(reader.GetOrdinal("customerlegal")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("customerlegal"))
                , reader.IsDBNull(reader.GetOrdinal("freight")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("freight"))
                , reader.IsDBNull(reader.GetOrdinal("parcelgroup")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("parcelgroup"))
                , reader.IsDBNull(reader.GetOrdinal("parcel")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("parcel"))
                , reader.IsDBNull(reader.GetOrdinal("storeid")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("storeid"))
                , reader.IsDBNull(reader.GetOrdinal("cellNumber")) ? (short?)null : reader.GetInt16(reader.GetOrdinal("cellNumber"))
                , reader.IsDBNull(reader.GetOrdinal("statedoc")) ? (byte?)null : reader.GetByte(reader.GetOrdinal("statedoc"))
                , reader.IsDBNull(reader.GetOrdinal("stateexc")) ? (byte?)null : reader.GetByte(reader.GetOrdinal("stateexc"))
                , reader.IsDBNull(reader.GetOrdinal("stateinv")) ? (byte?)null : reader.GetByte(reader.GetOrdinal("stateinv"))
                , reader.IsDBNull(reader.GetOrdinal("currencypaid")) ? false : reader.GetBoolean(reader.GetOrdinal("currencypaid"))
                , reader.IsDBNull(reader.GetOrdinal("specloaded")) ? false : reader.GetBoolean(reader.GetOrdinal("specloaded"))
                , reader.IsDBNull(reader.GetOrdinal("ttlpayinvoice")) ? false : reader.GetBoolean(reader.GetOrdinal("ttlpayinvoice"))
                , reader.IsDBNull(reader.GetOrdinal("ttlpaycurrency")) ? false : reader.GetBoolean(reader.GetOrdinal("ttlpaycurrency"))
                , reader.IsDBNull(reader.GetOrdinal("parceltype")) ? null : CustomBrokerWpf.References.ParcelTypes.FindFirstItem("Id", (int)reader.GetByte(reader.GetOrdinal("parceltype")))
                , reader.IsDBNull(reader.GetOrdinal("additionalcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("additionalcost"))
                , reader.IsDBNull(reader.GetOrdinal("additionalpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("additionalpay"))
                , reader.IsDBNull(reader.GetOrdinal("actualWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("actualWeight"))
                , reader.IsDBNull(reader.GetOrdinal("bringcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("bringcost"))
                , reader.IsDBNull(reader.GetOrdinal("bringpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("bringpay"))
                , reader.IsDBNull(reader.GetOrdinal("brokercost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("brokercost"))
                , reader.IsDBNull(reader.GetOrdinal("brokerpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("brokerpay"))
                , reader.IsDBNull(reader.GetOrdinal("currencyrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("currencyrate"))
                , reader.IsDBNull(reader.GetOrdinal("currencysum")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("currencysum"))
                , reader.IsDBNull(reader.GetOrdinal("customscost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("customscost"))
                , reader.IsDBNull(reader.GetOrdinal("customspay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("customspay"))
                , reader.IsDBNull(reader.GetOrdinal("deliverycost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("deliverycost"))
                , reader.IsDBNull(reader.GetOrdinal("deliverypay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("deliverypay"))
                , reader.IsDBNull(reader.GetOrdinal("dtrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("dtrate"))
                , reader.IsDBNull(reader.GetOrdinal("goodValue")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("goodValue"))
                , reader.IsDBNull(reader.GetOrdinal("freightcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("freightcost"))
                , reader.IsDBNull(reader.GetOrdinal("freightpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("freightpay"))
                , reader.IsDBNull(reader.GetOrdinal("insurancecost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("insurancecost"))
                , reader.IsDBNull(reader.GetOrdinal("insurancepay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("insurancepay"))
                , reader.IsDBNull(reader.GetOrdinal("invoice")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("invoice"))
                , reader.IsDBNull(reader.GetOrdinal("invoicediscount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("invoicediscount"))
                , reader.IsDBNull(reader.GetOrdinal("officialWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("officialWeight"))
                , reader.IsDBNull(reader.GetOrdinal("preparatncost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("preparatncost"))
                , reader.IsDBNull(reader.GetOrdinal("preparatnpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("preparatnpay"))
                , reader.IsDBNull(reader.GetOrdinal("selling")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("selling"))
                , reader.IsDBNull(reader.GetOrdinal("sellingmarkup")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sellingmarkup"))
                , reader.IsDBNull(reader.GetOrdinal("sellingmarkuprate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sellingmarkuprate"))
                , reader.IsDBNull(reader.GetOrdinal("sertificatcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sertificatcost"))
                , reader.IsDBNull(reader.GetOrdinal("sertificatpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sertificatpay"))
                , reader.IsDBNull(reader.GetOrdinal("tdcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("tdcost"))
                , reader.IsDBNull(reader.GetOrdinal("tdpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("tdpay"))
                , reader.IsDBNull(reader.GetOrdinal("volume")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("volume"))
                , reader.IsDBNull(reader.GetOrdinal("currencydate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("currencydate"))
                , reader.IsDBNull(reader.GetOrdinal("currencypaiddate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("currencypaiddate"))
                , reader.IsDBNull(reader.GetOrdinal("gtddate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gtddate"))
                , reader.GetDateTime(reader.GetOrdinal("requestDate"))
                , reader.IsDBNull(reader.GetOrdinal("shipplandate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("shipplandate"))
                , reader.IsDBNull(reader.GetOrdinal("specification")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("specification"))
                , reader.IsDBNull(reader.GetOrdinal("storageDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("storageDate"))
                , reader.IsDBNull(reader.GetOrdinal("storageInform")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("storageInform"))
                , reader.IsDBNull(reader.GetOrdinal("algorithmnote1")) ? null : reader.GetString(reader.GetOrdinal("algorithmnote1"))
                , reader.IsDBNull(reader.GetOrdinal("algorithmnote2")) ? null : reader.GetString(reader.GetOrdinal("algorithmnote2"))
                , reader.IsDBNull(reader.GetOrdinal("loadDescription")) ? null : reader.GetString(reader.GetOrdinal("loadDescription"))
                , reader.IsDBNull(reader.GetOrdinal("colorMark")) ? null : reader.GetString(reader.GetOrdinal("colorMark"))
                , reader.IsDBNull(reader.GetOrdinal("consolidate")) ? null : reader.GetString(reader.GetOrdinal("consolidate"))
                , reader.IsDBNull(reader.GetOrdinal("currencynote")) ? null : reader.GetString(reader.GetOrdinal("currencynote"))
                , reader.IsDBNull(reader.GetOrdinal("customerNote")) ? null : reader.GetString(reader.GetOrdinal("customerNote"))
                , reader.IsDBNull(reader.GetOrdinal("docdirpath")) ? null : reader.GetString(reader.GetOrdinal("docdirpath"))
                , reader.IsDBNull(reader.GetOrdinal("gtd")) ? null : reader.GetString(reader.GetOrdinal("gtd"))
                , reader.IsDBNull(reader.GetOrdinal("fullnumber")) ? null : reader.GetString(reader.GetOrdinal("fullnumber"))
                , reader.IsDBNull(reader.GetOrdinal("managergroupName")) ? null : reader.GetString(reader.GetOrdinal("managergroupName"))
                , reader.IsDBNull(reader.GetOrdinal("managerNote")) ? null : reader.GetString(reader.GetOrdinal("managerNote"))
                , reader.IsDBNull(reader.GetOrdinal("servicetype")) ? null : reader.GetString(reader.GetOrdinal("servicetype"))
                , reader.IsDBNull(reader.GetOrdinal("storageNote")) ? null : reader.GetString(reader.GetOrdinal("storageNote"))
                , reader.IsDBNull(reader.GetOrdinal("storagePoint")) ? null : reader.GetString(reader.GetOrdinal("storagePoint"))
                , reader.IsDBNull(reader.GetOrdinal("importer")) ? null : CustomBrokerWpf.References.Importers.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("importer")))
                , reader.IsDBNull(reader.GetOrdinal("managerid")) ? null : CustomBrokerWpf.References.Managers.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("managerid")))
                );
            Request request = CustomBrokerWpf.References.RequestStore.UpdateItem(newitem);
            if (this.SpecificationLoad)
            {
                if (request.SpecificationIsNull != (this.FillType == lib.FillType.Refresh))
                {
                    if (this.FillType == lib.FillType.Refresh)
                        request.SpecificationInit = CustomBrokerWpf.References.SpecificationStore.UpdateItem(request.Specification.Id, addcon, out errors);
                    else
                        request.SpecificationInit = CustomBrokerWpf.References.SpecificationStore.GetItemLoad(request, addcon, out errors);
                    this.Errors.AddRange(errors);
                }
            }
            if (request != newitem)
            {
                //if (!request.CustomerLegalsIsNull) this.mydispatcher.Invoke(() => { request.CustomerLegalsRefresh(myldbm); });
                if (!request.MailStateStockIsNull) request.MailStateStock.Update();
                if (!request.MailStateTakeGoods9IsNull) request.MailStateTakeGoods9.Update();
            }
            if(this.FillType == lib.FillType.Refresh)
            {
                mydispatcher.Invoke(() => {
                    request.AlgorithmCMD?.Refresh.Execute(null);
                    request.AlgorithmConCMD?.Refresh.Execute(null);
                });
            }
            request.IsLoaded = true;
            return request;
        }
        protected override void GetOutputSpecificParametersValue(Request item)
        {
            if (myinsertupdateparams[4].Value != null)
                item.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", (int)myinsertupdateparams[4].Value);
            item.ManagerGroupName = (string)(DBNull.Value == myinsertupdateparams[3].Value ? null : myinsertupdateparams[3].Value);
            if(item.DomainState==lib.DomainObjectState.Added)
                CustomBrokerWpf.References.RequestStore.UpdateItem(item);
        }
        protected override bool SaveChildObjects(Request item)
        {
            bool isSuccess = true;
            if (myldbm!=null & !item.CustomerLegalsIsNull)
            {
                myldbm.Errors.Clear();
                myldbm.Request = item;
                myldbm.Collection = item.CustomerLegals;
                myldbm.Command.Connection = this.Command.Connection;
                if (!myldbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    foreach (lib.DBMError err in myldbm.Errors) this.Errors.Add(err);
                }
            }
            //if (!item.PaymentsIsNull)
            //{
            //    mypmdbm.Errors.Clear();
            //    mypmdbm.Request = item;
            //    mypmdbm.Collection = item.Payments;
            //    if (!mypmdbm.SaveCollectionChanches())
            //    {
            //        isSuccess = false;
            //        foreach (lib.DBMError err in mypmdbm.Errors) this.Errors.Add(err);
            //    }
            //}
            if (!(myspdbm==null | item.SpecificationIsNull))
            {
                myspdbm.Errors.Clear();
                myspdbm.Command.Connection = this.Command.Connection;
                if (!myspdbm.SaveItemChanches(item.Specification))
                {
                    isSuccess = false;
                    foreach (lib.DBMError err in myspdbm.Errors) this.Errors.Add(err);
                }
            }
            return isSuccess;
        }
        protected override bool SaveIncludedObject(Request item)
        {
            bool success = true;
            if (mypdbm!=null && !item.ParcelIsNull)
            {
                mypdbm.Errors.Clear();
                mypdbm.Command.Connection = this.Command.Connection;
                if (!mypdbm.SaveItemChanches(item.Parcel))
                {
                    foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
                    success = false;
                }
            }
            return success;
        }
        protected override bool SaveReferenceObjects()
        {
            //mypmdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetSpecificParametersValue(Request item)
        {
            myinsertparams[1].Value = item.RequestDate;
            int i = 1;
            myupdateparams[++i].Value = item.IsSpecification;
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("Status");
            myupdateparams[++i].Value = false;
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("StorePoint");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("StoreDate");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CustomerId");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CustomerLegal");
            ++i;
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("AgentId");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("StoreId");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CellNumber");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("OfficialWeight");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("ActualWeight");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("Volume");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("GoodValue");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("FreightId");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("StoreNote");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("ManagerNote");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CustomerNote");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("ParcelGroup");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("ColorMark");
            myupdateparams[++i].Value = false;
            myupdateparams[++i].Value = false;
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("IsSpecification");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("ParcelType");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("Invoice");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("InvoiceDiscount");
            //myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CustomsCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("DeliveryCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("BrokerCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("InsuranceCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("FreightCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("SertificatCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("AdditionalCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("PreparatnCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("TotalCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("BringCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CorrCost");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CustomsPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("DeliveryPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("BrokerPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("InsurancePay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("FreightPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("SertificatPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("AdditionalPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("PreparatnPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("TDPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("BringPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("TotalPay");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("ServiceType");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CurrencyRate");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CurrencySum");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CurrencyDate");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CurrencyPaid");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CurrencyPaidDate");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("CurrencyNote");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("Selling");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("SellingMarkup");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("SellingMarkupRate");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("GTD");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("GTDDate");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("DTRate");
            i++;
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("IsSpecification");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("StateDoc");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("StateExc");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("StateInv");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("DocDirPath");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("TtlPayInvoice");
            myupdateparams[++i].Value = item.HasPropertyOutdatedValue("TtlPayCurrency");
            foreach (SqlParameter par in myupdateparams)
                switch (par.ParameterName)
                {
                    case "@algorithmnote1":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.AlgorithmNote1));
                        break;
                    case "@algorithmnote2":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.AlgorithmNote2));
                        break;
                    case "@consolidatetrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Consolidate));
                        break;
                    case "@importertrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Importer));
                        break;
                    case "@loadDescriptiontrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Cargo));
                        break;
                    case "@manageridtrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Manager));
                        break;
                    case "@parcel":
                        par.Value = item.ParcelId;
                        break;
                    case "@parceltrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Parcel)) || item.HasPropertyOutdatedValue(nameof(Request.ParcelId));
                        break;
                    case "@shipplandatetrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.ShipPlanDate));
                        break;
                }
            i = 4;
            myinsertupdateparams[i++].Value = item.Status?.Id;
            myinsertupdateparams[i++].Value = null;
            myinsertupdateparams[i++].Value = item.StorePoint;
            myinsertupdateparams[i++].Value = item.StoreDate;
            myinsertupdateparams[i++].Value = item.CustomerId;
            myinsertupdateparams[i++].Value = item.CustomerLegal;
            i++;
            myinsertupdateparams[i++].Value = item.AgentId;
            myinsertupdateparams[i++].Value = item.StoreId;
            myinsertupdateparams[i++].Value = item.CellNumber;
            myinsertupdateparams[i++].Value = item.OfficialWeight;
            myinsertupdateparams[i++].Value = item.ActualWeight;
            myinsertupdateparams[i++].Value = item.Volume;
            myinsertupdateparams[i++].Value = item.GoodValue;
            myinsertupdateparams[i++].Value = item.FreightId;
            myinsertupdateparams[i++].Value = item.StoreNote;
            myinsertupdateparams[i++].Value = item.ManagerNote;
            myinsertupdateparams[i++].Value = item.CustomerNote;
            myinsertupdateparams[i++].Value = item.ParcelGroup;
            myinsertupdateparams[i++].Value = item.ColorMark;
            myinsertupdateparams[i++].Value = null;
            myinsertupdateparams[i++].Value = null;
            myinsertupdateparams[i++].Value = item.IsSpecification;
            myinsertupdateparams[i++].Value = item.ParcelType?.Id;
            myinsertupdateparams[i++].Value = item.Invoice;
            myinsertupdateparams[i++].Value = item.InvoiceDiscount;
            //myinsertupdateparams[i++].Value = item.CustomsCost;
            myinsertupdateparams[i++].Value = item.DeliveryCost;
            myinsertupdateparams[i++].Value = item.BrokerCost;
            myinsertupdateparams[i++].Value = item.InsuranceCost;
            myinsertupdateparams[i++].Value = item.FreightCost;
            myinsertupdateparams[i++].Value = item.SertificatCost;
            myinsertupdateparams[i++].Value = item.AdditionalCost;
            myinsertupdateparams[i++].Value = item.PreparatnCost;
            myinsertupdateparams[i++].Value = item.TotalCost;
            myinsertupdateparams[i++].Value = item.BringCost;
            myinsertupdateparams[i++].Value = item.CorrCost;
            myinsertupdateparams[i++].Value = item.CustomsPay;
            myinsertupdateparams[i++].Value = item.DeliveryPay;
            myinsertupdateparams[i++].Value = item.BrokerPay;
            myinsertupdateparams[i++].Value = item.InsurancePay;
            myinsertupdateparams[i++].Value = item.FreightPay;
            myinsertupdateparams[i++].Value = item.SertificatPay;
            myinsertupdateparams[i++].Value = item.AdditionalPay;
            myinsertupdateparams[i++].Value = item.PreparatnPay;
            myinsertupdateparams[i++].Value = item.TDPay;
            myinsertupdateparams[i++].Value = item.BringPay;
            myinsertupdateparams[i++].Value = null;//item.TotalPay
            myinsertupdateparams[i++].Value = item.ServiceType;
            myinsertupdateparams[i++].Value = item.CurrencyRate;
            myinsertupdateparams[i++].Value = item.CurrencySum;
            myinsertupdateparams[i++].Value = item.CurrencyDate;
            myinsertupdateparams[i++].Value = item.CurrencyPaid;
            myinsertupdateparams[i++].Value = item.CurrencyPaidDate;
            myinsertupdateparams[i++].Value = item.CurrencyNote;
            myinsertupdateparams[i++].Value = item.Selling;
            myinsertupdateparams[i++].Value = item.SellingMarkup;
            myinsertupdateparams[i++].Value = item.SellingMarkupRate;
            myinsertupdateparams[i++].Value = item.GTD;
            myinsertupdateparams[i++].Value = item.GTDDate;
            myinsertupdateparams[i++].Value = item.DTRate;
            myinsertupdateparams[i++].Value = item.StateDoc;
            myinsertupdateparams[i++].Value = item.StateExc;
            myinsertupdateparams[i++].Value = item.StateInv;
            myinsertupdateparams[i++].Value = item.DocDirPath;
            myinsertupdateparams[i++].Value = item.TtlPayInvoice;
            myinsertupdateparams[i++].Value = item.TtlPayCurrency;
            foreach(SqlParameter par in myinsertupdateparams)
                switch(par.ParameterName)
                {
                    case "@algorithmnote1":
                        par.Value = item.AlgorithmNote1;
                        break;
                    case "@algorithmnote2":
                        par.Value = item.AlgorithmNote2;
                        break;
                    case "@consolidate":
                        par.Value = item.Consolidate;
                        break;
                    case "@importer":
                        par.Value = item.Importer?.Id;
                        break;
                    case "@loadDescription":
                        par.Value = item.Cargo;
                        break;
                    case "@managerid":
                        par.Value = item.Manager?.Id;
                        break;
                    case "@shipplandate":
                        par.Value = item.ShipPlanDate;
                        break;
                }
            mydeleteparams[1].Value = myinsertupdateparams[0].Value;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override bool LoadObjects()
        {
            return true;
        }
    }

    internal class RequestStore : lib.DomainStorageLoad<Request, RequestDBM>
    {
        public RequestStore(RequestDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(Request olditem, Request newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class RequestVM : lib.ViewModelErrorNotifyItem<Request>, lib.Interfaces.ITotalValuesItem
    {
        public RequestVM(Request item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.AgentId), nameof(this.CustomerLegals), nameof(this.Importer), nameof(this.InvoiceDiscount),nameof(this.ShipPlanDate), nameof(this.ServiceType) });
            DeleteRefreshProperties.AddRange(new string[] { "AdditionalPay", "BringPay", "BrokerPay", "TotalCost", "CustomsPay", "DeliveryPay", "FreightPay", "InsurancePay", "Invoice", "InvoiceDiscount", "PreparatnPay", "SertificatPay", "ServiceType", "TDPay" });
            RejectPropertiesOrder.AddRange(new string[] { "CustomerId", "CustomerLegal" });
            InitProperties();
            mysendmail = new RelayCommand(SendMailExec, SendMailCanExec);
            base.PropertyChanged += this.RequestVM_PropertyChanged;
            item.ValueChanged += this.DomenObject_ValueChanged;
        }
        public RequestVM() : this(new Request()) { }

        decimal? myadditionalpay, myadditionalcost, mybringpay, mybringcost;

        public decimal? ActualWeight
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.ActualWeight.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.ActualWeight.Value, value.Value))))
                {
                    string name = "ActualWeight";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ActualWeight);
                    ChangingDomainProperty = name; this.DomainObject.ActualWeight = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ActualWeight : null; }
        }
        public decimal? AdditionalCost
        {
            set
            {
                if (!this.IsReadOnly && (myadditionalcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myadditionalcost.Value, value.Value))))
                {
                    string name = "AdditionalCost";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myadditionalcost);
                    myadditionalcost = value.Value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.AdditionalCost = value;
                        ClearErrorMessageForProperty(name);
                        this.PropertyChangedNotification("TotalCost");
                    }
                }
            }
            get { return this.IsEnabled ? myadditionalcost : null; }
        }
        public decimal? AdditionalPay
        {
            set
            {
                if (!this.IsReadOnly && (myadditionalpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myadditionalpay.Value, value.Value))))
                {
                    string name = "AdditionalPay";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myadditionalpay);
                    myadditionalpay = value.Value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.AdditionalPay = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? myadditionalpay : null; }
        }
        public int? AgentId
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.AgentId.HasValue != value.HasValue || (value.HasValue && this.DomainObject.AgentId.Value != value.Value)))
                {
                    string name = "AgentId";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.AgentId);
                    ChangingDomainProperty = name; this.DomainObject.AgentId = value;
                    this.ValidateProperty(name, true); // for indication
                }
            }
            get { return this.IsEnabled ? this.DomainObject.AgentId : null; }
        }
        private string myagentname;
        public string AgentName
        {
            get
            {
                if (myagentname == null & this.DomainObject.AgentId.HasValue)
                {
                    myagentname = CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", this.DomainObject.AgentId.Value)?.Name;
                }
                return myagentname;
            }
        }
        public string AlgorithmNote1
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.AlgorithmNote1, value)))
                {
                    string name = "AlgorithmNote1";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.AlgorithmNote1);
                    ChangingDomainProperty = name; this.DomainObject.AlgorithmNote1 = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.AlgorithmNote1 : null; }
        }
        public string AlgorithmNote2
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.AlgorithmNote2, value)))
                {
                    string name = "AlgorithmNote2";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.AlgorithmNote2);
                    ChangingDomainProperty = name; this.DomainObject.AlgorithmNote2 = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.AlgorithmNote2 : null; }
        }
        public decimal? BringCost
        {
            set
            {
                if (!this.IsReadOnly && (mybringcost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mybringcost.Value, value.Value))))
                {
                    string name = "BringCost";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mybringcost);
                    mybringcost = value.Value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.BringCost = value;
                        ClearErrorMessageForProperty(name);
                        this.PropertyChangedNotification("TotalCost");
                    }
                }
            }
            get { return this.IsEnabled ? mybringcost : null; }
        }
        public decimal? BringPay
        {
            set
            {
                if (!this.IsReadOnly && (mybringpay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mybringpay.Value, value.Value))))
                {
                    string name = "BringPay";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mybringpay);
                    mybringpay = value.Value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.BringPay = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? mybringpay : null; }
        }
        public decimal? BrokerCost
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.BrokerCost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.BrokerCost.Value, value.Value))))
                {
                    string name = "BrokerCost";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.BrokerCost);
                    ChangingDomainProperty = name; this.DomainObject.BrokerCost = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.BrokerCost : null; }
        }
        public decimal? BrokerPay
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.BrokerPay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.BrokerPay.Value, value.Value))))
                {
                    string name = "BrokerPay";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.BrokerPay);
                    ChangingDomainProperty = name; this.DomainObject.BrokerPay = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.BrokerPay : null; }
        }
        public string BrokerCostPay
        { get { return this.IsEnabled ? ((this.DomainObject.BrokerCost.HasValue ? this.DomainObject.BrokerCost.Value.ToString("N2") : string.Empty) + " / " + (this.DomainObject.BrokerPay.HasValue ? this.DomainObject.BrokerPay.Value.ToString("N2") : string.Empty)) : null; } }
        public string Cargo
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Cargo, value)))
                {
                    string name = "Cargo";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Cargo);
                    ChangingDomainProperty = name; this.DomainObject.Cargo = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Cargo : null; }
        }
        public short? CellNumber
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CellNumber.HasValue != value.HasValue || (value.HasValue && this.DomainObject.CellNumber.Value != value.Value)))
                {
                    string name = "CellNumber";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CellNumber);
                    ChangingDomainProperty = name; this.DomainObject.CellNumber = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CellNumber : null; }
        }
        public string ColorMark
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.ColorMark, value)))
                {
                    string name = "ColorMark";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ColorMark);
                    ChangingDomainProperty = name; this.DomainObject.ColorMark = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ColorMark : null; }
        }
        public string Consolidate
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Consolidate, value)))
                {
                    string name = "Consolidate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Consolidate);
                    ChangingDomainProperty = name; this.DomainObject.Consolidate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Consolidate : null; }
        }
        public decimal? CorrCost
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CorrCost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CorrCost.Value, value.Value))))
                {
                    string name = "CorrCost";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CorrCost);
                    ChangingDomainProperty = name; this.DomainObject.CorrCost = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CorrCost : null; }
        }
        //public decimal? CorrPay
        //{
        //    set
        //    {
        //        if (!this.IsReadOnly && (this.DomainObject.CorrPay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CorrPay.Value, value.Value))))
        //        {
        //            string name = "CorrPay";
        //            if (!myUnchangedPropertyCollection.ContainsKey(name))
        //                this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CorrPay);
        //            ChangingDomainProperty = name; this.DomainObject.CorrPay = value;
        //        }
        //    }
        //    get { return this.IsEnabled ? this.DomainObject.CorrPay : null; }
        //}
        public DateTime? CurrencyDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CurrencyDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.CurrencyDate.Value, value.Value))))
                {
                    string name = "CurrencyDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyDate);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyDate : null; }
        }
        public string CurrencyNote
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CurrencyNote, value)))
                {
                    string name = "CurrencyNote";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyNote);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyNote = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyNote : null; }
        }
        public bool? CurrencyPaid
        {
            set
            {
                if (!this.IsReadOnly && (value.HasValue && this.DomainObject.CurrencyPaid != value.Value))
                {
                    string name = "CurrencyPaid";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyPaid);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyPaid = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyPaid : (bool?)null; }
        }
        public DateTime? CurrencyPaidDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CurrencyPaidDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.CurrencyPaidDate.Value, value.Value))))
                {
                    string name = "CurrencyPaidDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyPaidDate);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyPaidDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyPaidDate : null; }
        }
        public decimal? CurrencyRate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CurrencyRate.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CurrencyRate.Value, value.Value))))
                {
                    string name = "CurrencyRate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyRate);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyRate = value;
                    this.PropertyChangedNotification("CurrencySum");
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyRate : null; }
        }
        public decimal? CurrencySum
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CurrencySum.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CurrencySum.Value, value.Value))))
                {
                    string name = "CurrencySum";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencySum);
                    ChangingDomainProperty = name; this.DomainObject.CurrencySum = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencySum = this.DomainObject.InvoiceDiscount * this.CurrencyRate : null; }
        }
        public int? CustomerId
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CustomerId.HasValue != value.HasValue || (value.HasValue && this.DomainObject.CustomerId.Value != value.Value)))
                {
                    string name = "CustomerId";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CustomerId);
                    //this.CustomerLegal = null;// для истории
                    ChangingDomainProperty = name; this.DomainObject.CustomerId = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CustomerId : null; }
        }
        private string mycustomername;
        public string CustomerName
        {
            get
            {
                if (mycustomername == null & this.DomainObject.CustomerId.HasValue)
                {
                    ReferenceDS refds = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
                    if (refds.tableCustomerName.Count == 0) refds.CustomerNameRefresh();
                    System.Data.DataRow[] rows = refds.tableCustomerName.Select("customerID=" + this.DomainObject.CustomerId.Value.ToString());
                    if (rows.Length > 0)
                        mycustomername = (rows[0] as ReferenceDS.tableCustomerNameRow).customerName;
                }
                return mycustomername;
            }
        }
        public string CustomerLegalsNames
        {
            get
            {
                return this.IsEnabled ? this.DomainObject.CustomerLegalsNames : null;
            }
        }
        //public int? CustomerLegal
        //{
        //    set
        //    {
        //        if (!this.IsReadOnly && (this.DomainObject.CustomerLegal.HasValue != value.HasValue || (value.HasValue && this.DomainObject.CustomerLegal.Value != value.Value)))
        //        {
        //            string name = "CustomerLegal";
        //            if (!myUnchangedPropertyCollection.ContainsKey(name))
        //                this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CustomerLegal);
        //            ChangingDomainProperty = name; this.DomainObject.CustomerLegal = value;
        //        }
        //    }
        //    get { return this.IsEnabled ? this.DomainObject.CustomerLegal : null; }
        //}
        public string CustomerNote
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CustomerNote, value)))
                {
                    string name = "CustomerNote";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CustomerNote);
                    ChangingDomainProperty = name; this.DomainObject.CustomerNote = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CustomerNote : null; }
        }
        public decimal? CustomsPay
        {
            //set
            //{
            //    if (!this.IsReadOnly && (this.DomainObject.CustomsPay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CustomsPay.Value, value.Value))))
            //    {
            //        string name = "CustomsPay";
            //        if (!myUnchangedPropertyCollection.ContainsKey(name))
            //            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CustomsPay);
            //        ChangingDomainProperty = name; this.DomainObject.CustomsPay = value;
            //    }
            //}
            get { return this.IsEnabled ? this.DomainObject.CustomsPay : null; }
        }
        public decimal? CustomsPayInvoice
        {
            get { return this.IsEnabled ? this.DomainObject.CustomsPayInvoice : null; }
        }
        public decimal? CustomsPayPer
        {
            get { return this.IsEnabled ? this.DomainObject.CustomsPay * 0.22M : null; }
        }
        public decimal? CustomsPayAddPer
        {
            get { return this.IsEnabled ? this.DomainObject.InvoiceDiscount * 0.22M : null; }
        }
        public decimal? DeliveryCost
        {
            //set
            //{
            //    if (!this.IsReadOnly && (this.DomainObject.DeliveryCost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.DeliveryCost.Value, value.Value))))
            //    {
            //        string name = "DeliveryCost";
            //        if (!myUnchangedPropertyCollection.ContainsKey(name))
            //            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DeliveryCost);
            //        ChangingDomainProperty = name; this.DomainObject.DeliveryCost = value;
            //    }
            //}
            get { return this.IsEnabled ? this.DomainObject.DeliveryCost : null; }
        }
        public decimal? DeliveryPay
        {
            //set
            //{
            //    if (!this.IsReadOnly && (this.DomainObject.DeliveryPay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.DeliveryPay.Value, value.Value))))
            //    {
            //        string name = "DeliveryPay";
            //        if (!myUnchangedPropertyCollection.ContainsKey(name))
            //            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DeliveryPay);
            //        ChangingDomainProperty = name; this.DomainObject.DeliveryPay = value;
            //    }
            //}
            get { return this.IsEnabled ? this.DomainObject.DeliveryPay : null; }
        }
        public string DeliveryCostPay
        { get { return this.IsEnabled ? ((this.DomainObject.DeliveryCost.HasValue ? this.DomainObject.DeliveryCost.Value.ToString("N2") : string.Empty) + " / " + (this.DomainObject.DeliveryPay.HasValue ? this.DomainObject.DeliveryPay.Value.ToString("N2") : string.Empty)) : null; } }
        public decimal? DTRate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.DTRate.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.DTRate.Value, value.Value))))
                {
                    string name = "DTRate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DTRate);
                    ChangingDomainProperty = name; this.DomainObject.DTRate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DTRate : null; }
        }
        public decimal? FreightCost
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.FreightCost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.FreightCost.Value, value.Value))))
                {
                    string name = "FreightCost";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FreightCost);
                    ChangingDomainProperty = name; this.DomainObject.FreightCost = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.FreightCost : null; }
        }
        public decimal? FreightPay
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.FreightPay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.FreightPay.Value, value.Value))))
                {
                    string name = "FreightPay";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FreightPay);
                    ChangingDomainProperty = name; this.DomainObject.FreightPay = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.FreightPay : null; }
        }
        public decimal? GoodValue
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.GoodValue.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.GoodValue.Value, value.Value))))
                {
                    string name = "GoodValue";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GoodValue);
                    ChangingDomainProperty = name; this.DomainObject.GoodValue = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.GoodValue : null; }
        }
        public string GTD
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.GTD, value)))
                {
                    string name = "GTD";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GTD);
                    ChangingDomainProperty = name; this.DomainObject.GTD = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.GTD : null; }
        }
        public DateTime? GTDDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.GTDDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.GTDDate.Value, value.Value))))
                {
                    string name = "GTDDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GTDDate);
                    ChangingDomainProperty = name; this.DomainObject.GTDDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.GTDDate : null; }
        }
        public Importer Importer
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Importer, value)))
                {
                    string name = "Importer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Importer);
                    ChangingDomainProperty = name; this.DomainObject.Importer = value;
                    this.ValidateProperty(name, true); // for indication
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Importer : null; }
        }
        public decimal? InsuranceCost
        {
            get { return this.IsEnabled ? this.DomainObject.InsuranceCost : null; }
        }
        public decimal? InsurancePay
        {
            get { return this.IsEnabled ? this.DomainObject.InsurancePay : null; }
        }
        public decimal? Invoice
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Invoice.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Invoice.Value, value.Value))))
                {
                    string name = "Invoice";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Invoice);
                    ChangingDomainProperty = name; this.DomainObject.Invoice = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Invoice : null; }
        }
        public decimal? Invoice2per
        {
            get { return this.IsEnabled ? this.DomainObject.InvoiceDiscount * 0.02M : null; }
        }
        public decimal? InvoiceAdd2per
        {
            get { return this.IsEnabled ? this.DomainObject.InvoiceDiscount * 1.02M : null; }
        }
        private decimal? myinvoicediscount;
        public decimal? InvoiceDiscount
        {
            set
            {
                if (!this.IsReadOnly && (myinvoicediscount.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myinvoicediscount.Value, value.Value))))
                {
                    string name = nameof(Request.InvoiceDiscount);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.InvoiceDiscount);
                    myinvoicediscount = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.UpdateInvoiceDiscount(value,0); }
                }
            }
            get { return this.IsEnabled ? myinvoicediscount : null; }
        }
        public bool? InvoiceDiscountFill
        { get { return this.IsEnabled ? this.DomainObject.InvoiceDiscountFill : (bool?)null; } }
        public bool? IsSpecification
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || this.DomainObject.IsSpecification == value.Value))
                {
                    string name = "IsSpecification";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.IsSpecification);
                    ChangingDomainProperty = name; this.DomainObject.IsSpecification = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.IsSpecification : (bool?)null; }
        }
        public Manager Manager
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Manager, value)))
                {
                    string name = "Manager";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Manager);
                    ChangingDomainProperty = name; this.DomainObject.Manager = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Manager : null; }
        }
        public string ManagerGroupName
        {
            get { return this.IsEnabled ? this.DomainObject.ManagerGroupName : null; }
        }
        public string ManagerGroupImage
        {
            get { return this.IsEnabled ? (string.IsNullOrEmpty(this.DomainObject.ManagerGroupName) ? "/CustomBrokerWpf;component/Images/plus.gif" : (this.DomainObject.ManagerGroupName.IndexOf('1') > 0 ? "/CustomBrokerWpf;component/Images/group_1.jpg" : "/CustomBrokerWpf;component/Images/group_2.jpg")) : "/CustomBrokerWpf;component/Images/plus.gif"; }
        }
        public string ManagerNote
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.ManagerNote, value)))
                {
                    string name = "ManagerNote";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ManagerNote);
                    ChangingDomainProperty = name; this.DomainObject.ManagerNote = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ManagerNote : null; }
        }
        public string Notes
        {
            get
            {
                return this.IsEnabled ? (this.DomainObject.StoreNote ?? string.Empty) + " " + (this.DomainObject.ManagerNote ?? string.Empty) + " " + (this.DomainObject.CustomerNote ?? string.Empty) : null;
            }
        }
        public decimal? OfficialWeight
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.OfficialWeight.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.OfficialWeight.Value, value.Value))))
                {
                    string name = "OfficialWeight";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.OfficialWeight);
                    ChangingDomainProperty = name; this.DomainObject.OfficialWeight = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.OfficialWeight : null; }
        }
        public Parcel Parcel
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Parcel, value)))
                {
                    string name = "Parcel";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Parcel);
                    ChangingDomainProperty = name; this.DomainObject.Parcel = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Parcel : null; }
        }
        public int? ParcelGroup
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.ParcelGroup.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.ParcelGroup.Value, value.Value))))
                {
                    string name = "ParcelGroup";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ParcelGroup);
                    ChangingDomainProperty = name; this.DomainObject.ParcelGroup = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ParcelGroup : null; }
        }
        public string ParcelNumber
        {
            get { return this.IsEnabled ? this.DomainObject.ParcelNumber : null; }
        }
        public System.Windows.Visibility ParcelNumberVisible
        { get { return this.DomainObject.ParcelId.HasValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; } }
        public lib.ReferenceSimpleItem ParcelType
        {
            set
            {
                if (value != null && !(this.IsReadOnly || object.Equals(this.DomainObject.ParcelType, value)))
                {
                    string name = "ParcelType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ParcelType);
                    ChangingDomainProperty = name; this.DomainObject.ParcelType = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ParcelType : null; }
        }
        public bool ParcelTypeEnable
        { get { return !(this.DomainObject.ParcelId.HasValue | this.IsReadOnly); } }
        private void RequestVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsReadOnly") PropertyChangedNotification("ParcelTypeEnable");
        }
        public decimal? PreparatnCost
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.PreparatnCost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.PreparatnCost.Value, value.Value))))
                {
                    string name = "PreparatnCost";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PreparatnCost);
                    ChangingDomainProperty = name; this.DomainObject.PreparatnCost = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PreparatnCost : null; }
        }
        public decimal? PreparatnPay
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.PreparatnPay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.PreparatnPay.Value, value.Value))))
                {
                    string name = "PreparatnPay";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PreparatnPay);
                    ChangingDomainProperty = name; this.DomainObject.PreparatnPay = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PreparatnPay : null; }
        }
        public DateTime? RequestDate
        {
            //set
            //{
            //    if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(this.DomainObject.RequestDate, value.Value)))
            //    {
            //        string name = "RequestDate";
            //        if (!myUnchangedPropertyCollection.ContainsKey(name))
            //            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.RequestDate);
            //        ChangingDomainProperty = name; this.DomainObject.RequestDate = value.Value;
            //    }
            //}
            get { return this.IsEnabled ? this.DomainObject.RequestDate : (DateTime?)null; }
        }
        public decimal? Selling
        {
            //set
            //{
            //    if (!this.IsReadOnly && (this.DomainObject.Selling.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.Selling.Value, value.Value))))
            //    {
            //        string name = "Selling";
            //        if (!myUnchangedPropertyCollection.ContainsKey(name))
            //            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Selling);
            //        ChangingDomainProperty = name; this.DomainObject.Selling = value;
            //    }
            //}
            get { return this.IsEnabled ? this.SellingMarkup + this.CurrencySum : null; }
        }
        public decimal? SellingMarkup
        {
            //set
            //{
            //    if (!this.IsReadOnly && (this.DomainObject.SellingMarkup.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.SellingMarkup.Value, value.Value))))
            //    {
            //        string name = "SellingMarkup";
            //        if (!myUnchangedPropertyCollection.ContainsKey(name))
            //            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SellingMarkup);
            //        ChangingDomainProperty = name; this.DomainObject.SellingMarkup = value;
            //        if (value.HasValue & this.DomainObject.CurrencySum.HasValue && this.DomainObject.CurrencySum.Value != 0M)
            //        {
            //            ChangingDomainProperty = "SellingMarkupRate";
            //            this.DomainObject.SellingMarkupRate = decimal.Divide(value.Value, this.DomainObject.CurrencySum.Value);
            //            ChangingDomainProperty = "Selling";
            //            this.DomainObject.Selling = this.DomainObject.CurrencySum.Value + this.DomainObject.SellingMarkup.Value;
            //        }
            //    }
            //}
            get { return this.IsEnabled ? this.InvoiceDiscount * this.DTRate * this.SellingMarkupRate : null; }
        }
        public decimal? SellingMarkupRate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.SellingMarkupRate.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.SellingMarkupRate.Value, value.Value))))
                {
                    string name = "SellingMarkupRate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SellingMarkupRate);
                    ChangingDomainProperty = name; this.DomainObject.SellingMarkupRate = value;
                    if (value.HasValue & this.DomainObject.CurrencySum.HasValue)
                    {
                        ChangingDomainProperty = "SellingMarkup";
                        this.DomainObject.SellingMarkup = value.Value * this.DomainObject.CurrencySum.Value;
                        ChangingDomainProperty = "Selling";
                        this.DomainObject.Selling = this.DomainObject.CurrencySum.Value + this.DomainObject.SellingMarkup.Value;
                    }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.SellingMarkupRate : null; }
        }
        public decimal? SertificatCost
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.SertificatCost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.SertificatCost.Value, value.Value))))
                {
                    string name = "SertificatCost";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SertificatCost);
                    ChangingDomainProperty = name; this.DomainObject.SertificatCost = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.SertificatCost : null; }
        }
        public decimal? SertificatPay
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.SertificatPay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.SertificatPay.Value, value.Value))))
                {
                    string name = "SertificatPay";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SertificatPay);
                    ChangingDomainProperty = name; this.DomainObject.SertificatPay = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.SertificatPay : null; }
        }
        public string ServiceType
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.ServiceType, value)))
                {
                    string name = "ServiceType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ServiceType);
                    ChangingDomainProperty = name; this.DomainObject.ServiceType = value;
                    this.ValidateProperty(name, true);
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ServiceType : null; }
        }
        private DateTime? myshipplandate;
        public DateTime? ShipPlanDate
        {
            set
            {
                if (!this.IsReadOnly && (myshipplandate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(myshipplandate.Value, value.Value))))
                {
                    string name = nameof(this.ShipPlanDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ShipPlanDate);
                    myshipplandate = value;
                    if (ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.ShipPlanDate = value; }
                }
            }
            get { return this.IsEnabled ? myshipplandate : null; }
        }
        public Specification.Specification Specification
        {
            get { return this.IsEnabled ? this.DomainObject.Specification : null; }
        }
        public DateTime? SpecificationDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.SpecificationDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.SpecificationDate.Value, value.Value))))
                {
                    string name = nameof(this.SpecificationDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SpecificationDate);
                    ChangingDomainProperty = name; this.DomainObject.SpecificationDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.SpecificationDate : null; }
        }
        public byte? StateDoc
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.StateDoc.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.StateDoc.Value, value.Value))))
                {
                    string name = "StateDoc";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.StateDoc);
                    ChangingDomainProperty = name; this.DomainObject.StateDoc = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.StateDoc : null; }
        }
        public byte? StateExc
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.StateExc.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.StateExc.Value, value.Value))))
                {
                    string name = "StateExc";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.StateExc);
                    ChangingDomainProperty = name; this.DomainObject.StateExc = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.StateExc : null; }
        }
        public string StateExcImagePath
        {
            get
            {
                string path;
                if (this.DomainObject.IsSpecification)
                    path = @"/CustomBrokerWpf;component/Images/excel_1.png";
                else
                    path = @"/CustomBrokerWpf;component/Images/plus.gif";
                return path;
            }
        }
        public byte? StateInv
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.StateInv.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.StateInv.Value, value.Value))))
                {
                    string name = "StateInv";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.StateInv);
                    ChangingDomainProperty = name; this.DomainObject.StateInv = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.StateInv : null; }
        }
        public lib.ReferenceSimpleItem Status
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Status, value)))
                {
                    string name = "Status";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Status);
                    ChangingDomainProperty = name; this.DomainObject.Status = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Status : null; }
        }
        public string StatusParcel
        { get { return this.IsEnabled ? (this.DomainObject.ParcelId.HasValue ? this.DomainObject.ParcelNumber : this.DomainObject.Status?.Name) : null; } }
        public System.Windows.Visibility StatusVisible
        { get { return this.DomainObject.ParcelId.HasValue ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }
        public bool StatusEditable
        {
            get { return !this.DomainObject.ParcelId.HasValue; }
        }
        public DateTime? StoreDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.StoreDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.StoreDate.Value, value.Value))))
                {
                    string name = "StoreDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.StoreDate);
                    ChangingDomainProperty = name; this.DomainObject.StoreDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.StoreDate : null; }
        }
        public int? StoreId
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.StoreId.HasValue != value.HasValue || (value.HasValue && !int.Equals(this.DomainObject.StoreId.Value, value.Value))))
                {
                    string name = "StoreId";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.StoreId);
                    ChangingDomainProperty = name; this.DomainObject.StoreId = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.StoreId : null; }
        }
        public string StoreNote
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.StoreNote, value)))
                {
                    string name = "StoreNote";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.StoreNote);
                    ChangingDomainProperty = name; this.DomainObject.StoreNote = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.StoreNote : null; }
        }
        public string StorePoint
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.StorePoint, value)))
                {
                    string name = "StorePoint";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.StorePoint);
                    ChangingDomainProperty = name; this.DomainObject.StorePoint = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.StorePoint : null; }
        }
        public string StorePointDate
        {
            get { return this.IsEnabled ? this.DomainObject.StorePointDate : null; }
        }
        public DateTime? StoreInform
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.StoreInform.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.StoreInform.Value, value.Value))))
                {
                    string name = "StoreInform";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.StoreInform);
                    ChangingDomainProperty = name; this.DomainObject.StoreInform = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.StoreInform : null; }
        }
        //public decimal? TDCost
        //{
        //    //set
        //    //{
        //    //    if (!this.IsReadOnly && (this.DomainObject.TDCost.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.TDCost.Value, value.Value))))
        //    //    {
        //    //        string name = "TDCost";
        //    //        if (!myUnchangedPropertyCollection.ContainsKey(name))
        //    //            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TDCost);
        //    //        ChangingDomainProperty = name; this.DomainObject.TDCost = value;
        //    //    }
        //    //}
        //    get { return null; }
        //}
        public decimal? TDPay
        {
            get { return this.IsEnabled ? this.DomainObject.TDPay : null; }
            //get { return this.IsEnabled && this.ServiceType == "ТД" & this.Invoice.HasValue ? (decimal?)decimal.Multiply(this.Invoice.Value, 0.03M) : null; }
        }
        public decimal? TotalCost
        {
            //get { return this.IsEnabled ? ((AdditionalCost ?? 0M) + (BringCost ?? 0M) + (BrokerCost ?? 0M) + (CorrCost ?? 0M) + (this.ServiceType == "ТЭО" ? 0M : (CustomsCost ?? 0M)) + (DeliveryCost ?? 0M) + (FreightCost ?? 0M) + (InsuranceCost ?? 0M) + (PreparatnCost ?? 0M) + (SertificatCost ?? 0M) + (TDCost ?? 0M)) : (decimal?)null; }
            get { return this.IsEnabled ? this.DomainObject.TotalCost : null; }
        }
        public decimal? TotalPay
        {
            get { return this.IsEnabled ? this.DomainObject.TotalPay : null; }
            //get { return this.IsEnabled ? ((AdditionalPay ?? 0M) + (BringPay ?? 0M) + (BrokerPay ?? 0M) + (CorrPay ?? 0M) + (this.ServiceType == "ТЭО" ? 0M : (CustomsPay ?? 0M)) + (DeliveryPay ?? 0M) + (FreightPay ?? 0M) + (InsurancePay ?? 0M) + (PreparatnPay ?? 0M) + (SertificatPay ?? 0M) + (TDPay ?? 0M)) : (decimal?)null; }
        }
        public decimal? TotalPayInvoicePer
        {
            get { return this.IsEnabled ? this.DomainObject.TotalPayInvoicePer : null; }
        }
        public bool? TtlPayInvoice
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || this.DomainObject.TtlPayInvoice == value.Value))
                {
                    string name = "TtlPayInvoice";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TtlPayInvoice);
                    ChangingDomainProperty = name; this.DomainObject.TtlPayInvoice = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TtlPayInvoice : (bool?)null; }
        }
        public bool? TtlPayCurrency
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || this.DomainObject.TtlPayCurrency == value.Value))
                {
                    string name = "TtlPayCurrency";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TtlPayCurrency);
                    ChangingDomainProperty = name; this.DomainObject.TtlPayCurrency = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TtlPayCurrency : (bool?)null; }
        }
        public DateTime? UpdateWhen
        { get { return this.DomainObject.UpdateWhen; } }
        public string UpdateWho
        { get { return this.DomainObject.UpdateWho; } }
        public decimal? Volume
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Volume.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Volume.Value, value.Value))))
                {
                    string name = "Volume";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Volume);
                    ChangingDomainProperty = name; this.DomainObject.Volume = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Volume : null; }
        }

        public bool IsSelected { set; get; } // will deleted
        public bool ProcessedIn { set; get; }
        public bool ProcessedOut { set; get; }
        private bool myselected;
        public bool Selected
        {
            set
            {
                bool oldvalue = myselected; myselected = value;
                this.OnValueChanged("Selected", oldvalue, value);
            }
            get { return myselected; }
        }

        #region Algorithm
        public Algorithm.AlgorithmConsolidateCommand AlgorithmConCMD
        { get { return this.IsEnabled ? this.DomainObject.AlgorithmConCMD : null; } }
        public Visibility ConVisibility
        { get { return string.IsNullOrEmpty(this.DomainObject.Consolidate) ? Visibility.Collapsed : Visibility.Visible; } }
        //public decimal? ConAdditionalCost
        //{
        //    get { return this.IsEnabled ? this.DomainObject.AlgorithmConCMD.AdditionalCost : null; }
        //}
        //public decimal? ConCellNumber
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConCellNumber : null; }
        //}
        //public decimal? ConCorr
        //{
        //    set
        //    {
        //        if (!this.IsReadOnly && (this.DomainObject.ConCorr.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.ConCorr.Value, value.Value))))
        //        {
        //            string name = "ConCorr";
        //            if (!myUnchangedPropertyCollection.ContainsKey(name))
        //                this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ConCorr);
        //            ChangingDomainProperty = name; this.DomainObject.ConCorr = value;
        //        }
        //    }
        //    get { return this.IsEnabled ? this.DomainObject.ConCorr : null; }
        //}
        //public decimal? ConCorrPer
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConCorrPer : null; }
        //}
        //public decimal? ConCost
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConCost : null; }
        //}
        //public decimal? ConCostPer
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConCostPer : null; }
        //}
        //public decimal? ConCustomsPay
        //{
        //    set
        //    {
        //        if (!this.IsReadOnly && (this.DomainObject.ConCustomsPay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.ConCustomsPay.Value, value.Value))))
        //        {
        //            string name = "ConCustomsPay";
        //            if (!myUnchangedPropertyCollection.ContainsKey(name))
        //                this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ConCustomsPay);
        //            ChangingDomainProperty = name; this.DomainObject.ConCustomsPay = value;
        //        }
        //    }
        //    get { return this.IsEnabled ? this.DomainObject.ConCustomsPay : null; }
        //}
        //public decimal? ConCustomsPayPer
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConCustomsPayPer : null; }
        //}
        //public decimal? ConFreightCost
        //{
        //    get { return this.IsEnabled ? this.DomainObject.AlgorithmConCMD.FreightCost : null; }
        //}
        //public decimal? ConIncome
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConIncome : null; }
        //}
        //public decimal? ConIncomePer
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConIncomePer : null; }
        //}
        //public decimal? ConInvoice
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConInvoice : null; }
        //}
        //public decimal? ConInvoiceDiscount
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConInvoiceDiscount : null; }
        //}
        //public decimal? ConLogisticsCost
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConLogisticsCost : null; }
        //}
        //public decimal? ConLogisticsPay
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConLogisticsPay : null; }
        //}
        //public decimal? ConPay
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConPay : null; }
        //}
        //public decimal? ConPayPer
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConPayPer : null; }
        //}
        //public decimal? ConPreparatnCost
        //{
        //    get { return this.IsEnabled ? this.DomainObject.AlgorithmConCMD.PreparatnCost : null; }
        //}
        //public decimal? ConVolume
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConVolume : null; }
        //}
        //public decimal? ConWeight
        //{
        //    get { return this.IsEnabled ? this.DomainObject.ConWeight : null; }
        //}

        public decimal? CustomsCost
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CustomsCost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CustomsCost.Value, value.Value))))
                {
                    string name = "CustomsCost";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CustomsCost);
                    ChangingDomainProperty = name; this.DomainObject.CustomsCost = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CustomsCost : null; }
        }
        public decimal? Income
        {
            get { return this.IsEnabled ? this.DomainObject.Income : null; }
        }
        public decimal? IncomePay
        {
            get { return this.IsEnabled ? this.DomainObject.IncomePay : null; }
        }
        public bool? IncomePayPoor
        { get { return this.IsEnabled ? this.DomainObject.IncomePayPoor : (bool?)null; } }
        public decimal? IncomeM3
        {
            get { return this.IsEnabled ? this.DomainObject.IncomeM3 : null; }
        }
        public decimal? Log
        {
            get { return this.IsEnabled ? this.DomainObject.Log : null; }
        }
        public decimal? LogisticsCost
        {
            get { return this.IsEnabled ? this.DomainObject.LogisticsCost : null; }
        }
        public decimal? LogisticsPay
        {
            get { return this.IsEnabled ? this.DomainObject.LogisticsPay : null; }
        }
        #endregion

        #region MailState
        public string MailStateStockImage
        {
            get
            {
                string path;
                switch (this.DomainObject.MailStateStock.State)
                {
                    case 1:
                        path = "/CustomBrokerWpf;component/Images/mail_1.png";
                        break;
                    case 2:
                        path = "/CustomBrokerWpf;component/Images/mail_3.png";
                        break;
                    default:
                        path = "/CustomBrokerWpf;component/Images/mail_2.png";
                        break;
                }
                return path;
            }
        }
        public string MailStateTakeGoods9Image
        {
            get
            {
                string path;
                switch (this.DomainObject.MailStateTakeGoods9.State)
                {
                    case 1:
                        path = "/CustomBrokerWpf;component/Images/mail_1.png";
                        break;
                    case 2:
                        path = "/CustomBrokerWpf;component/Images/mail_3.png";
                        break;
                    default:
                        path = "/CustomBrokerWpf;component/Images/mail_2.png";
                        break;
                }
                return path;
            }
        }

        System.Windows.Controls.Primitives.Popup mypopup;
        private RelayCommand mysendmail;
        public ICommand SendMail
        {
            get { return mysendmail; }
        }
        private void SendMailExec(object parametr)
        {
            bool iserr = false;
            Request req = this.DomainObject;
            switch ((string)parametr)
            {
                case "Stock":
                    req.MailStateStock.Send();
                    if (req.MailStateStock.SendErrors.Count > 0)
                    {
                        System.Text.StringBuilder text = new System.Text.StringBuilder();
                        foreach (lib.DBMError err in req.MailStateStock.SendErrors)
                        {
                            text.AppendLine(err.Message);
                            iserr |= !string.Equals(err.Code, "0");
                        }
                        if (iserr) { text.Insert(0, "Отправка выполнена с ошибкой!\n"); }
                        mypopup = KirillPolyanskiy.Common.PopupCreator.GetPopup(text.ToString()
                            , iserr ? new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFDDBE0")) : System.Windows.Media.Brushes.WhiteSmoke
                            , (iserr ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Black)
                            , System.Windows.Media.Brushes.Beige
                            , false
                            , System.Windows.Controls.Primitives.PlacementMode.Mouse
                            );
                        mypopup.IsOpen = true;
                    }
                    break;
                case "TakeGoods9":
                    req.MailStateTakeGoods9.Send();
                    if (req.MailStateTakeGoods9.SendErrors.Count > 0)
                    {
                        System.Text.StringBuilder text = new System.Text.StringBuilder();
                        foreach (lib.DBMError err in req.MailStateTakeGoods9.SendErrors)
                        {
                            text.AppendLine(err.Message);
                            iserr |= !string.Equals(err.Code, "0");
                        }
                        if (iserr) { text.Insert(0, "Отправка выполнена с ошибкой!\n"); }
                        mypopup = KirillPolyanskiy.Common.PopupCreator.GetPopup(text.ToString()
                            , iserr ? new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFDDBE0")) : System.Windows.Media.Brushes.WhiteSmoke
                            , (iserr ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Black)
                            , System.Windows.Media.Brushes.Beige
                            , false
                            , System.Windows.Controls.Primitives.PlacementMode.Mouse
                            );
                        mypopup.IsOpen = true;
                    }
                    break;
            }
        }
        private bool SendMailCanExec(object parametr)
        { return true; }
        #endregion

        public decimal BalanceInvoice
        {
            get { return this.DomainObject.BalanceInvoice; }
        }
        public decimal BalancePrepayments
        {
            get { return this.DomainObject.BalancePrepayments; }
        }
        public decimal BalanceFinal
        {
            get { return this.DomainObject.BalanceFinal; }
        }
        public string BalanceInvoiceColor
        {
            get { return this.DomainObject.BalanceInvoice > 0M ? "Green" : (this.DomainObject.BalanceInvoice < 0M ? "Red" : "Black"); }
        }
        public string BalancePrepaymentsColor
        {
            get { return this.DomainObject.BalancePrepayments > 0M ? "Green" : (this.DomainObject.BalancePrepayments < 0M ? "Red" : "Black"); }
        }
        public string BalanceFinalColor
        {
            get { return this.DomainObject.BalanceFinal > 0M ? "Green" : (this.DomainObject.BalanceFinal < 0M ? "Red" : "Black"); }
        }

        private RequestCustomerLegalSynchronizer mylsync;
        private ListCollectionView mycustomerlegals;
        public ListCollectionView CustomerLegals
        {
            get
            {
                if (mycustomerlegals == null)
                {
                    if (mylsync == null)
                    {
                        mylsync = new RequestCustomerLegalSynchronizer();
                        mylsync.DomainCollection = this.DomainObject.CustomerLegals;
                    }
                    mycustomerlegals = new ListCollectionView(mylsync.ViewModelCollection);
                    mycustomerlegals.SortDescriptions.Add(new System.ComponentModel.SortDescription("Selected", System.ComponentModel.ListSortDirection.Descending));
                    mycustomerlegals.SortDescriptions.Add(new System.ComponentModel.SortDescription("CustomerLegal.Name", System.ComponentModel.ListSortDirection.Ascending));
                }
                return mycustomerlegals;
            }
        }
        private ListCollectionView mycustomerlegalsselected;
        public ListCollectionView CustomerLegalsSelected
        {
            get
            {
                if (mycustomerlegalsselected == null)
                {
                    if (mylsync == null)
                    {
                        mylsync = new RequestCustomerLegalSynchronizer();
                        mylsync.DomainCollection = this.DomainObject.CustomerLegals;
                    }
                    mycustomerlegalsselected = new ListCollectionView(mylsync.ViewModelCollection);
                    mycustomerlegalsselected.Filter = (object item) => { return (item as RequestCustomerLegalVM).Selected; };
                    mycustomerlegalsselected.SortDescriptions.Add(new System.ComponentModel.SortDescription("CustomerLegal.Name", System.ComponentModel.ListSortDirection.Ascending));
                }
                return mycustomerlegalsselected;
            }
        }

        //private RequestPaymentSynchronizer mypsync;
        //private ListCollectionView mypaymentsinvoice;
        //public ListCollectionView PaymentsInvoice
        //{
        //    get
        //    {
        //        if (mypaymentsinvoice == null)
        //        {
        //            if (mypsync == null)
        //            {
        //                mypsync = new RequestPaymentSynchronizer();
        //                mypsync.DomainCollection = this.DomainObject.Payments;
        //            }
        //            mypaymentsinvoice = new ListCollectionView(mypsync.ViewModelCollection);
        //            mypaymentsinvoice.Filter = (item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as RequestPaymentVM).PaymentType == 1; };
        //            mypaymentsinvoice.SortDescriptions.Add(new System.ComponentModel.SortDescription("Date", System.ComponentModel.ListSortDirection.Ascending));
        //            mypaymentsinvoice.SortDescriptions.Add(new System.ComponentModel.SortDescription("DocType", System.ComponentModel.ListSortDirection.Ascending));
        //            //mypaymentsinvoice.CurrentChanging += PaymentsInvoice_CurrentChanging;
        //            mypaymentsinvoice.CurrentChanged += PaymentsInvoice_CurrentChanged;
        //        }
        //        return mypaymentsinvoice;
        //    }
        //}
        //private ListCollectionView myprepayments;
        //public ListCollectionView Prepayments
        //{
        //    get
        //    {
        //        if (myprepayments == null)
        //        {
        //            if (mypsync == null)
        //            {
        //                mypsync = new RequestPaymentSynchronizer();
        //                mypsync.DomainCollection = this.DomainObject.Payments;
        //            }
        //            myprepayments = new ListCollectionView(mypsync.ViewModelCollection);
        //            myprepayments.Filter = (item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as RequestPaymentVM).PaymentType == 2; };
        //            myprepayments.SortDescriptions.Add(new System.ComponentModel.SortDescription("Date", System.ComponentModel.ListSortDirection.Ascending));
        //            myprepayments.SortDescriptions.Add(new System.ComponentModel.SortDescription("DocType", System.ComponentModel.ListSortDirection.Ascending));
        //            //myprepayments.CurrentChanging += Prepayments_CurrentChanging;
        //            myprepayments.CurrentChanged += Prepayments_CurrentChanged;
        //        }
        //        return myprepayments;
        //    }
        //}
        //private ListCollectionView myfinalpayments;
        //public ListCollectionView FinalPayments
        //{
        //    get
        //    {
        //        if (myfinalpayments == null)
        //        {
        //            if (mypsync == null)
        //            {
        //                mypsync = new RequestPaymentSynchronizer();
        //                mypsync.DomainCollection = this.DomainObject.Payments;
        //            }
        //            myfinalpayments = new ListCollectionView(mypsync.ViewModelCollection);
        //            myfinalpayments.Filter = (item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as RequestPaymentVM).PaymentType == 3; };
        //            myfinalpayments.SortDescriptions.Add(new System.ComponentModel.SortDescription("Date", System.ComponentModel.ListSortDirection.Ascending));
        //            myfinalpayments.SortDescriptions.Add(new System.ComponentModel.SortDescription("DocType", System.ComponentModel.ListSortDirection.Ascending));
        //            myfinalpayments.CurrentChanged += FinalPayments_CurrentChanged;
        //        }
        //        return myfinalpayments;
        //    }
        //}
        //private ListCollectionView mycustomerlegals;
        //public ListCollectionView CustomerLegals
        //{
        //    get
        //    {
        //        if (this.CustomerId.HasValue & mycustomerlegals == null)
        //        {
        //            CustomerLegalDBM cldbm = new CustomerLegalDBM();
        //            cldbm.CustomerId = this.CustomerId.Value;
        //            cldbm.Fill();
        //            mycustomerlegals = new ListCollectionView(cldbm.Collection);
        //            mycustomerlegals.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        //        }
        //        return mycustomerlegals;
        //    }
        //}
        //private void PaymentsInvoice_CurrentChanging(object sender, CurrentChangingEventArgs e)
        //{
        //    if (mypaymentsinvoice.CurrentItem != null && (mypaymentsinvoice.CurrentItem as RequestPaymentVM).DocType == 1)
        //        InvoiceRate.RateDate = (mypaymentsinvoice.CurrentItem as RequestPaymentVM).Date;
        //}
        //private void PaymentsInvoice_CurrentChanged(object sender, EventArgs e)
        //{
        //    if (mypaymentsinvoice.IsAddingNew && (mypaymentsinvoice.CurrentAddItem as RequestPaymentVM).PaymentType == 0)
        //    {
        //        (mypaymentsinvoice.CurrentAddItem as RequestPaymentVM).PaymentType = 1;
        //        //(mypaymentsinvoice.CurrentAddItem as RequestPaymentVM).Request = this;
        //    }
        //    //else if (mypaymentsinvoice.CurrentItem != null && (mypaymentsinvoice.CurrentItem as RequestPaymentVM).DocType == 1)
        //    //    InvoiceRate.RateDate = (mypaymentsinvoice.CurrentItem as RequestPaymentVM).Date;

        //}
        //private void Prepayments_CurrentChanging(object sender, CurrentChangingEventArgs e)
        //{
        //    if (myprepayments.CurrentItem is RequestPaymentVM && (myprepayments.CurrentItem as RequestPaymentVM).DocType == 1)
        //        PrepaymentsRate.RateDate = (myprepayments.CurrentItem as RequestPaymentVM).Date;
        //}
        //private void Prepayments_CurrentChanged(object sender, EventArgs e)
        //{
        //    if (myprepayments.IsAddingNew && (myprepayments.CurrentAddItem as RequestPaymentVM).PaymentType == 0)
        //    {
        //        (myprepayments.CurrentAddItem as RequestPaymentVM).PaymentType = 2;
        //        //(myprepayments.CurrentAddItem as RequestPaymentVM).Request = this;
        //    }
        //    //else if (myprepayments.CurrentItem != null && (myprepayments.CurrentItem as RequestPaymentVM).DocType == 1)
        //    //    PrepaymentsRate.RateDate = (myprepayments.CurrentItem as RequestPaymentVM).Date;
        //}
        //private void FinalPayments_CurrentChanged(object sender, EventArgs e)
        //{
        //    if (myfinalpayments.IsAddingNew && (myfinalpayments.CurrentAddItem as RequestPaymentVM).PaymentType == 0)
        //    {
        //        (myfinalpayments.CurrentAddItem as RequestPaymentVM).PaymentType = 3;
        //        //(myfinalpayments.CurrentAddItem as RequestPaymentVM).Request = this;
        //    }
        //}
        //private Classes.CurrencyRate myinvoicerate;
        //public Classes.CurrencyRate InvoiceRate
        //{
        //    get
        //    {
        //        if (myinvoicerate == null)
        //        {
        //            foreach (object item in this.PaymentsInvoice)
        //            {
        //                if (item is RequestPaymentVM)
        //                {
        //                    RequestPaymentVM pitem = item as RequestPaymentVM;
        //                    if (pitem.DocType == 1)
        //                    {
        //                        myinvoicerate = new CurrencyRate(pitem.Date.Value);
        //                        PropertyChangedNotification("InvoiceRate");
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        return myinvoicerate;
        //    }
        //}
        //private Classes.CurrencyRate myprepaymentsrate;
        //public Classes.CurrencyRate PrepaymentsRate
        //{
        //    get
        //    {
        //        if (myprepaymentsrate == null)
        //        {
        //            foreach (object item in this.Prepayments)
        //            {
        //                if (item is RequestPaymentVM)
        //                {
        //                    RequestPaymentVM pitem = item as RequestPaymentVM;
        //                    if (pitem.DocType == 1)
        //                    {
        //                        myprepaymentsrate = new CurrencyRate(pitem.Date.Value);
        //                        PropertyChangedNotification("PrepaymentsRate");
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        return myprepaymentsrate;
        //    }
        //}

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "AdditionalCost":
                    myadditionalcost = this.DomainObject.AdditionalCost;
                    break;
                case "AdditionalPay":
                    myadditionalpay = this.DomainObject.AdditionalPay;
                    break;
                case "AgentId":
                    myagentname = null;
                    this.PropertyChangedNotification("AgentName");
                    break;
                case "BringCost":
                    mybringcost = this.DomainObject.BringCost;
                    break;
                case "BringPay":
                    mybringpay = this.DomainObject.BringPay;
                    break;
                case "BrokerCost":
                    this.PropertyChangedNotification("BrokerCostPay");
                    break;
                case "BrokerPay":
                    this.PropertyChangedNotification("BrokerCostPay");
                    break;
                case "TotalCost":
                case "TotalPay":
                    this.PropertyChangedNotification("Income");
                    break;
                case "FreightPay":
                case "InsurancePay":
                    this.PropertyChangedNotification("TotalPayInvoicePer");
                    break;
                case "PreparatnPay":
                    this.PropertyChangedNotification("TotalPayInvoicePer");
                    break;
                case "Consolidate":
                    this.PropertyChangedNotification("ConVisibility");
                    break;
                case "CurrencySum":
                    this.PropertyChangedNotification("Selling");
                    break;
                case "CustomerLegalsSelected":
                    this.PropertyChangedNotification("CustomerLegalsNames");
                    this.PropertyChangedNotification(nameof(this.InvoiceDiscountFill));
                    if (mycustomerlegalsselected != null) mycustomerlegalsselected.Refresh();
                    break;
                case "CustomerId":
                    mycustomername = null;
                    this.PropertyChangedNotification("CustomerName");
                    mycustomerlegals = null;
                    this.PropertyChangedNotification("CustomerLegals");
                    break;
                case "CustomsPay":
                    this.PropertyChangedNotification("CustomsPayInvoice");
                    this.PropertyChangedNotification("CustomsPayPer");
                    this.PropertyChangedNotification("TotalPayInvoicePer");
                    break;
                case "DeliveryCost":
                    this.PropertyChangedNotification("DeliveryCostPay");
                    break;
                case "DeliveryPay":
                    this.PropertyChangedNotification("DeliveryCostPay");
                    this.PropertyChangedNotification("TotalPayInvoicePer");
                    break;
                case "DTRate":
                    this.PropertyChangedNotification("SellingMarkup");
                    this.PropertyChangedNotification("Selling");
                    break;
                case "Invoice":
                    this.PropertyChangedNotification("CustomsPayInvoice");
                    this.PropertyChangedNotification("TDPay");
                    this.PropertyChangedNotification("TotalPayInvoicePer");
                    break;
                case "InvoiceDiscount":
                    myinvoicediscount = this.DomainObject.InvoiceDiscount;
                    this.PropertyChangedNotification("CustomsPayAddPer");
                    this.PropertyChangedNotification("Invoice2per");
                    this.PropertyChangedNotification("InvoiceAdd2per");
                    this.PropertyChangedNotification("SellingMarkup");
                    this.PropertyChangedNotification("Selling");
                    break;
                case nameof(Request.IsSpecification):
                    this.PropertyChangedNotification(nameof(RequestVM.StateExcImagePath));
                    break;
                case "ManagerGroupName":
                    this.PropertyChangedNotification("ManagerGroupImage");
                    break;
                case "MailStateStockState":
                    PropertyChangedNotification("MailStateStockImage");
                    break;
                case "MailStateTakeGoods9State":
                    PropertyChangedNotification("MailStateTakeGoods9Image");
                    break;
                case "ParcelId":
                    this.PropertyChangedNotification("StatusVisible");
                    this.PropertyChangedNotification("ParcelNumber");
                    this.PropertyChangedNotification("ParcelNumberVisible");
                    this.PropertyChangedNotification("ParcelTypeEnable");
                    this.PropertyChangedNotification("StatusParcel");
                    this.PropertyChangedNotification("StatusVisible");
                    this.PropertyChangedNotification("StatusEditable");
                    break;
                case nameof(Request.Status):
                    this.PropertyChangedNotification("StatusParcel");
                    break;
                case "SellingMarkupRate":
                    this.PropertyChangedNotification("Selling");
                    this.PropertyChangedNotification("SellingMarkup");
                    break;
                case "SertificatPay":
                    this.PropertyChangedNotification("TotalPayInvoicePer");
                    break;
                case "ServiceType":
                    this.PropertyChangedNotification("TDPay");
                    this.PropertyChangedNotification("TotalPayInvoicePer");
                    break;
                case nameof(Request.ShipPlanDate):
                    myshipplandate = this.DomainObject.ShipPlanDate;
                    break;
                case "StorePoint":
                case "StoreDate":
                    this.PropertyChangedNotification("StorePointDate");
                    break;
                case "BalanceInvoice":
                    PropertyChangedNotification("BalanceInvoiceColor");
                    break;
                case "BalancePrepayments":
                    PropertyChangedNotification("BalancePrepaymentsColor");
                    break;
                case "BalanceFinal":
                    PropertyChangedNotification("BalanceFinalColor");
                    break;
            }
        }
        protected override void InitProperties()
        {
            myadditionalcost = this.DomainObject.AdditionalCost;
            myadditionalpay = this.DomainObject.AdditionalPay;
            mybringcost = this.DomainObject.BringCost;
            mybringpay = this.DomainObject.BringPay;
            myinvoicediscount = this.DomainObject.InvoiceDiscount;
            myshipplandate = this.DomainObject.ShipPlanDate;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "ActualWeight":
                    this.DomainObject.ActualWeight = (decimal?)value;
                    break;
                case "AdditionalCost":
                    if (myadditionalcost != this.DomainObject.AdditionalCost)
                        myadditionalcost = this.DomainObject.AdditionalCost;
                    else
                        this.AdditionalCost = (decimal?)value;
                    break;
                case "AdditionalPay":
                    if (myadditionalpay != this.DomainObject.AdditionalPay)
                        myadditionalpay = this.DomainObject.AdditionalPay;
                    else
                        this.AdditionalPay = (decimal?)value;
                    break;
                case "AgentId":
                    this.DomainObject.AgentId = (int?)value;
                    break;
                case "BringCost":
                    if (mybringcost != this.DomainObject.BringCost)
                        mybringcost = this.DomainObject.BringCost;
                    else
                        this.BringCost = (decimal?)value;
                    break;
                case "BringPay":
                    if (mybringpay != this.DomainObject.BringPay)
                        mybringpay = this.DomainObject.BringPay;
                    else
                        this.BringPay = (decimal?)value;
                    break;
                case "BrokerCost":
                    this.DomainObject.BrokerCost = (decimal?)value;
                    break;
                case "BrokerPay":
                    this.DomainObject.BrokerPay = (decimal?)value;
                    break;
                case "Cargo":
                    this.DomainObject.Cargo = (string)value;
                    break;
                case "CellNumber":
                    this.DomainObject.CellNumber = (short?)value;
                    break;
                case "ColorMark":
                    this.DomainObject.ColorMark = (string)value;
                    break;
                case "Consolidate":
                    this.DomainObject.Consolidate = (string)value;
                    break;
                case "CorrCost":
                    this.DomainObject.CorrCost = (decimal?)value;
                    break;
                //case "CorrPay":
                //    this.DomainObject.CorrPay = (decimal?)value;
                //    break;
                case "CurrencyDate":
                    this.DomainObject.CurrencyDate = (DateTime?)value;
                    break;
                case "CurrencyNote":
                    this.DomainObject.CurrencyNote = (string)value;
                    break;
                case "CurrencyPaid":
                    this.DomainObject.CurrencyPaid = (bool)value;
                    break;
                case "CurrencyPaidDate":
                    this.DomainObject.CurrencyPaidDate = (DateTime?)value;
                    break;
                case "CurrencyRate":
                    this.DomainObject.CurrencyRate = (decimal?)value;
                    break;
                case "CurrencySum":
                    this.DomainObject.CurrencySum = (decimal?)value;
                    break;
                case "CustomerId":
                    this.DomainObject.CustomerId = (int?)value;
                    break;
                case "CustomerLegal":
                    this.DomainObject.CustomerLegal = (int?)value;
                    break;
                case "CustomerNote":
                    this.DomainObject.CustomerNote = (string)value;
                    break;
                case "CustomsCost":
                    this.DomainObject.CustomsCost = (decimal?)value;
                    break;
                //case "CustomsPay":
                //    this.DomainObject.CustomsPay = (decimal?)value;
                //    break;
                case "DeliveryCost":
                    this.DomainObject.DeliveryCost = (decimal?)value;
                    break;
                case "DeliveryPay":
                    this.DomainObject.DeliveryPay = (decimal?)value;
                    break;
                case "DTRate":
                    this.DomainObject.DTRate = (decimal?)value;
                    break;
                case "FreightCost":
                    this.DomainObject.FreightCost = (decimal?)value;
                    break;
                case "FreightPay":
                    this.DomainObject.FreightPay = (decimal?)value;
                    break;
                case "GoodValue":
                    this.DomainObject.GoodValue = (decimal?)value;
                    break;
                case "GTD":
                    this.DomainObject.GTD = (string)value;
                    break;
                case "GTDDate":
                    this.DomainObject.GTDDate = (DateTime?)value;
                    break;
                case "Importer":
                    this.DomainObject.Importer = (Importer)value;
                    break;
                //case "InsuranceCost":
                //    this.DomainObject.InsuranceCost = (decimal?)value;
                //    break;
                //case "InsurancePay":
                //    this.DomainObject.InsurancePay = (decimal?)value;
                //    break;
                case "Invoice":
                    this.DomainObject.Invoice = (decimal?)value;
                    break;
                case "InvoiceDiscount":
                    if (myinvoicediscount != this.DomainObject.InvoiceDiscount)
                        myinvoicediscount = this.DomainObject.InvoiceDiscount;
                    else
                        this.DomainObject.InvoiceDiscount = (decimal?)value;
                    break;
                case "ManagerNote":
                    this.DomainObject.ManagerNote = (string)value;
                    break;
                case "OfficialWeight":
                    this.DomainObject.OfficialWeight = (decimal?)value;
                    break;
                case "ParcelGroup":
                    this.DomainObject.ParcelGroup = (int?)value;
                    break;
                case "ParcelType":
                    this.DomainObject.ParcelType = (lib.ReferenceSimpleItem)value;
                    break;
                case "PreparatnCost":
                    this.DomainObject.PreparatnCost = (decimal?)value;
                    break;
                case "PreparatnPay":
                    this.DomainObject.PreparatnPay = (decimal?)value;
                    break;
                case "SellingMarkupRate":
                    this.DomainObject.SellingMarkupRate = (decimal?)value;
                    break;
                case "SertificatCost":
                    this.DomainObject.SertificatCost = (decimal?)value;
                    break;
                case "SertificatPay":
                    this.DomainObject.SertificatPay = (decimal?)value;
                    break;
                case "ServiceType":
                    this.DomainObject.ServiceType = (string)value;
                    break;
                case nameof(this.ShipPlanDate):
                    if (myshipplandate != this.DomainObject.ShipPlanDate)
                        myshipplandate = this.DomainObject.ShipPlanDate;
                    else
                        this.DomainObject.ShipPlanDate = (DateTime?)value;
                    break;
                case "Status":
                    this.DomainObject.Status = (lib.ReferenceSimpleItem)value;
                    break;
                case "StoreDate":
                    this.DomainObject.StoreDate = (DateTime?)value;
                    break;
                case "StoreId":
                    this.DomainObject.StoreId = (int?)value;
                    break;
                case "StoreNote":
                    this.DomainObject.StoreNote = (string)value;
                    break;
                case "StorePoint":
                    this.DomainObject.StorePoint = (string)value;
                    break;
                case "StoreInform":
                    this.DomainObject.StoreInform = (DateTime?)value;
                    break;
                //case "TDCost":
                //    this.DomainObject.TDCost = (decimal?)value;
                //    break;
                case "TDPay":
                    this.DomainObject.TDPay = (decimal?)value;
                    break;
                case "Volume":
                    this.DomainObject.Volume = (decimal?)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    if (mycustomerlegals != null)
                    {
                        RequestCustomerLegalVM[] lremoved = new RequestCustomerLegalVM[mylsync.ViewModelCollection.Count];
                        foreach (RequestCustomerLegalVM litem in mylsync.ViewModelCollection)
                        {
                            if (litem.DomainState == lib.DomainObjectState.Added)
                            {
                                lremoved[i] = litem;
                                i++;
                            }
                            else
                            {
                                this.CustomerLegals.EditItem(litem);
                                litem.RejectChanges();
                                this.CustomerLegals.CommitEdit();
                            }
                        }
                        foreach (RequestCustomerLegalVM litem in lremoved)
                            if (litem != null) mylsync.ViewModelCollection.Remove(litem);
                    }
                    //i = 0;
                    //RequestPayment[] removed = new RequestPayment[this.DomainObject.Payments.Count];
                    //foreach (RequestPayment pitem in this.DomainObject.Payments)
                    //{
                    //    if (pitem.DomainState == lib.DomainObjectState.Added)
                    //    {
                    //        removed[i] = pitem;
                    //        i++;
                    //    }
                    //    else
                    //        pitem.RejectChanges();
                    //}
                    //foreach (RequestPayment pitem in removed)
                    //    if (pitem != null) this.DomainObject.Payments.Remove(pitem);
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.AgentId):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.AgentId, out errmsg);
                    break;
                case nameof(this.CustomerLegals):
                    StringBuilder err = new StringBuilder();
                    foreach (RequestCustomerLegalVM legal in this.CustomerLegals)
                    {
                        if (legal.Errors.Length > 0)
                        {
                            err.AppendLine(legal.Errors);
                            isvalid = false;
                        }
                        //valid = (!legal.Selected || legal.Validate(inform));
                        //if (!valid) err.AppendLine(legal.Errors);
                        //isvalid &= valid;
                    }
                    errmsg = err.ToString();
                    break;
                case nameof(this.Importer):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Importer,out errmsg);
                    break;
                case nameof(this.InvoiceDiscount):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myinvoicediscount, out errmsg);
                    if(isvalid && myinvoicediscount != this.DomainObject.InvoiceDiscount)
					{
                        ChangingDomainProperty = nameof(this.DomainObject.InvoiceDiscount); this.DomainObject.UpdateInvoiceDiscount(myinvoicediscount, 0);
                    }
                    break;
                case nameof(this.ShipPlanDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.ShipPlanDate, out errmsg);
                    break;
                case nameof(this.ServiceType):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.ServiceType, out errmsg);
                    break;
            }
            if (isvalid)
                ClearErrorMessageForProperty(propertyname);
            else if (inform) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myshipplandate!=this.DomainObject.ShipPlanDate || myinvoicediscount!=this.DomainObject.InvoiceDiscount;
        }

        internal string MoveFolder()
        {
            string err = string.Empty;
            try
            {
                if (this.CustomerId.HasValue)
                {
                    string path, pathfalse, docdirpath;
                    if (this.DomainObject.ParcelId.HasValue)
                        docdirpath = "Отправки\\" + this.Parcel.DocDirPath + "\\" + this.CustomerName + "_" + (this.ParcelGroup.HasValue ? this.ParcelGroup.ToString() : (string.IsNullOrEmpty(this.StorePointDate) ? (this.Id.ToString() + " " + this.RequestDate.Value.ToShortDateString()) : this.StorePointDate));
                    else
                        docdirpath = "Прямые\\" + this.CustomerName + "_" + (this.ParcelGroup.HasValue ? this.ParcelGroup.ToString() : (string.IsNullOrEmpty(this.StorePointDate) ? (this.Id.ToString() + " " + this.RequestDate.Value.ToShortDateString()) : this.StorePointDate));
                    path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + docdirpath;
                    if (!string.Equals(docdirpath, this.DomainObject.DocDirPath))
                    {
                        pathfalse = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + this.DomainObject.DocDirPath;
                        if (!string.IsNullOrEmpty(this.DomainObject.DocDirPath) & System.IO.Directory.Exists(pathfalse))
                        {
                            if (System.IO.Directory.Exists(path))
                            {
                                foreach (string movepath in System.IO.Directory.EnumerateDirectories(pathfalse))
                                {
                                    System.IO.DirectoryInfo movethis = new System.IO.DirectoryInfo(movepath);
                                    movethis.MoveTo(path + "\\" + movethis.Name);
                                }
                                foreach (string movepath in System.IO.Directory.EnumerateFiles(pathfalse))
                                {
                                    System.IO.FileInfo movethis = new System.IO.FileInfo(movepath);
                                    movethis.MoveTo(path + "\\" + movethis.Name);
                                }
                                System.IO.Directory.Delete(pathfalse);
                            }
                            else
                                System.IO.Directory.Move(pathfalse, path);
                        }
                        this.DomainObject.DocDirPath = docdirpath;
                    }
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                }
                else if (!this.CustomerId.HasValue)
                    err = "Необходимо указать клиента!";
                //else if (string.IsNullOrEmpty(item.StorePoint) | !item.StoreDate.HasValue | !item.ParcelGroup.HasValue)
                //    MessageBox.Show("Необходимо указать складской номер и дату!", "Папка документов");
                //else if (this.DomainState != lib.DomainObjectState.Unchanged)
                //    err = "Сохраните изменения!";
            }
            catch (Exception ex)
            {
                err = "Не удалось сохранить папку заявки" + (string.IsNullOrEmpty(this.StorePointDate) ? (this.Id.ToString() + " " + this.RequestDate.Value.ToShortDateString()) : this.StorePointDate) + "!\nЗакройте все документы из этой папки и повторите сохранение.\n\n" + ex.Message;
            }
            return err;
        }
        private void DomenObject_ValueChanged(object sender, lib.Interfaces.ValueChangedEventArgs<object> e)
        {
            this.OnValueChanged(e.PropertyName, e.OldValue, e.NewValue);
        }
    }

    public class RequestVMCommand : lib.ViewModelCommand<Request, RequestVM, RequestDBM>
    {
        public RequestVMCommand(RequestVM vm, ListCollectionView view) : base(vm, view)
        {
            mydbm = new Domain.RequestDBM();
            mydbm.ItemId = vm.Id;
            mydbm.LegalDBM = new RequestCustomerLegalDBM();
            myvm.IsReadOnly = myvm.DomainState==lib.DomainObjectState.Unchanged;
            myvm.DomainObject.AlgorithmCMD.IsReadOnly = myvm.IsReadOnly;
            myvm.DomainObject.AlgorithmConCMD.IsReadOnly = myvm.IsReadOnly;
            mydetailsadd = new RelayCommand(DetailsAddExec, DetailsAddCanExec);
            myprepaydel = new RelayCommand(PrepayDelExec, PrepayDelCanExec);
            myratedbm = new SpecificationCustomerInvoiceRateDBM();
        }

        public RequestVM VM
        {
            get { return myvm; }
        }
        public Algorithm.AlgorithmFormulaRequestCommand AlgorithmCommand
        {
            get { return VModel.DomainObject.AlgorithmCMD; }
        }
        public Algorithm.AlgorithmConsolidateCommand AlgorithmConCommand
        {
            get { return VModel.DomainObject.AlgorithmConCMD; }
        }
        SpecificationCustomerInvoiceRateDBM myratedbm;

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
        private ListCollectionView mystatuses;
        public ListCollectionView Statuses
        {
            get
            {
                if (mystatuses == null)
                {
                    mystatuses = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
                    mystatuses.Filter = (item) => { return (item as lib.ReferenceSimpleItem).Id < 50; };
                }
                return mystatuses;
            }
        }
        private ListCollectionView myparceltypes;
        public ListCollectionView ParcelTypes
        {
            get
            {
                if (myparceltypes == null)
                {
                    myparceltypes = new ListCollectionView(CustomBrokerWpf.References.ParcelTypes);
                }
                return myparceltypes;
            }
        }
        private ListCollectionView myloaddescriptions;
        public ListCollectionView LoadDescriptions
        {
            get
            {
                if (myloaddescriptions == null)
                {
                    myloaddescriptions = new ListCollectionView(CustomBrokerWpf.References.GoodsTypesParcel);
                    myloaddescriptions.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myloaddescriptions;
            }
        }
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
        //private ListCollectionView myratepers;
        //public ListCollectionView RatePers
        //{
        //    get
        //    {
        //        if (myratepers == null)
        //        {
        //            myratepers = new ListCollectionView(new RatePer[] { new RatePer(0.02M), new RatePer(0.22M) });
        //            myratepers.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        //        }
        //        return myratepers;
        //    }
        //}
        private ListCollectionView myimporters;
        public ListCollectionView Importers
        {
            get
            {
                if (myimporters == null)
                {
                    myimporters = new ListCollectionView(CustomBrokerWpf.References.Importers);
                    myimporters.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myimporters;
            }
        }

        private System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, string>> mydoctypes;
        public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, string>> DocTypes
        {
            get
            {
                if (mydoctypes == null)
                {
                    mydoctypes = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<int, string>>();
                    mydoctypes.Add(new System.Collections.Generic.KeyValuePair<int, string>(1, "Счет"));
                    mydoctypes.Add(new System.Collections.Generic.KeyValuePair<int, string>(2, "ПП"));
                }
                return mydoctypes;
            }
        }

        private RelayCommand mydetailsadd;
        public ICommand DetailsAdd
        {
            get { return mydetailsadd; }
        }
        private void DetailsAddExec(object parametr)
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
                    if (!System.IO.Directory.Exists(rootdir))
                        System.IO.Directory.CreateDirectory(rootdir);
                    path.Append(System.IO.Path.Combine(rootdir, this.DetailsBuildFileName() + System.IO.Path.GetExtension(fd.FileName)));
                    if (System.IO.File.Exists(path.ToString()))
                        System.IO.File.Delete(path.ToString());
                    System.IO.File.Copy(fd.FileName, path.ToString());
                    if (CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault != System.IO.Path.GetDirectoryName(fd.FileName))
                    {
                        CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault = System.IO.Path.GetDirectoryName(fd.FileName);
                        CustomBrokerWpf.Properties.Settings.Default.Save();
                    }
                    this.OpenPopup("Файл разбивки скопирован.", false);
                }
                catch (Exception ex)
                {
                    this.OpenPopup("Не удалось загрузить файл.\n" + ex.Message, true);
                }
            }
        }
        private bool DetailsAddCanExec(object parametr)
        { return !myvm.IsReadOnly; }
        private string DetailsBuildFileName()
        {
            System.Text.StringBuilder name = new System.Text.StringBuilder();
            if (string.IsNullOrEmpty(this.VModel.Consolidate))
            {
                if (this.VModel.ParcelGroup.HasValue)
                    name.Append(this.VModel.Parcel.ParcelNumber).Append("_gr").Append(this.VModel.ParcelGroup.ToString()).Append('_').Append(this.VModel.CustomerName).Append('_').Append(this.VModel.AgentName);
                else
                    name.Append(this.VModel.Parcel.ParcelNumber).Append("_s").Append(this.VModel.StorePoint).Append('_').Append(this.VModel.CustomerName).Append('_').Append(this.VModel.AgentName);
            }
            else
                name.Append(this.VModel.Parcel.ParcelNumber).Append("_").Append(this.VModel.Consolidate).Append('_').Append(this.VModel.AgentName);
            return name.ToString();

        }

        private RelayCommand myprepaydel;
        public ICommand PrepayDel
        {
            get { return myprepaydel; }
        }
        private void PrepayDelExec(object parametr)
        {
            PrepayCustomerRequestVM prepay = parametr as PrepayCustomerRequestVM;
            System.Windows.Data.ListCollectionView view = null;
            RequestCustomerLegalVM legal = null;
            foreach (RequestCustomerLegalVM item in myvm.CustomerLegalsSelected)
                if (item.DomainObject == prepay.Customer.DomainObject)
                { view = item.Prepays; legal = item; }
            if (view != null & !prepay.Prepay.InvoiceDate.HasValue)
            {
                if (view.IsAddingNew)
                    view.CancelNew();
                else
                {
                    view.EditItem(prepay);
                    prepay.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
                    view.CommitEdit();
                }
                legal.DomainObject.PropertyChangedNotification(nameof(legal.PrepaySum));
            }
        }
        private bool PrepayDelCanExec(object parametr)
        { return !myvm.IsReadOnly; }
        internal bool PrepayAddCanExec()
        { return !myvm.IsReadOnly; }
        internal void PrepayAddExec(object parametr)
        {
            RequestCustomerLegalVM legal = parametr as RequestCustomerLegalVM;
            legal.DomainObject.AddPrepay();
        }

        protected override void RejectChanges(object parametr)
        {
            this.AlgorithmCommand.Reject.Execute(parametr);
            base.RejectChanges(parametr);
            this.AlgorithmCommand.Reject.Execute(parametr);
            this.AlgorithmConCommand.Reject.Execute(parametr);
        }
        protected override void RefreshData(object parametr)
        {
            StringBuilder errstr = new StringBuilder();
            mydbm.FillType = lib.FillType.Refresh;
            mydbm.GetFirst();
            if (mydbm.Errors.Count > 0) foreach (lib.DBMError err in mydbm.Errors) errstr.AppendLine(err.Message);
            myvm.DomainObject.CustomerLegalsRefresh();
            //this.AlgorithmCommand.Refresh.Execute(parametr);
            //this.AlgorithmConCommand.Refresh.Execute(parametr);
            if (!myvm.DomainObject.SpecificationIsNull) // coefficients for DTSum
            {
                myvm.DomainObject.Specification.InvoiceDTRates.Clear();
                myratedbm.Specification = myvm.DomainObject.Specification;
                myratedbm.Load();
                if (myratedbm.Errors.Count > 0) foreach (lib.DBMError err in myratedbm.Errors) errstr.AppendLine(err.Message);
            }
            mydbm.FillType = lib.FillType.Initial;
            if (errstr.Length > 0) this.PopupText=errstr.ToString();
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void AddData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override bool CanAddData(object parametr)
        {
            return false;
        }
        //protected override bool CanSaveDataChanges()
        //{
        //    return base.CanSaveDataChanges();
        //}
        public override bool SaveDataChanges()
        {
            try
            {
                if (myvm.CustomerId.HasValue & ((!string.IsNullOrEmpty(myvm.StorePoint) & myvm.StoreDate.HasValue) | myvm.ParcelGroup.HasValue))
                {
                    string path, pathfalse, docdirpath;
                    if (myvm.DomainObject.ParcelId.HasValue)
                        docdirpath = "Отправки\\" + myvm.Parcel.DocDirPath + "\\" + myvm.CustomerName + "_" + (myvm.ParcelGroup.HasValue ? myvm.ParcelGroup.ToString() : myvm.StorePointDate);
                    else
                        docdirpath = "Прямые\\" + myvm.CustomerName + "_" + (myvm.ParcelGroup.HasValue ? myvm.ParcelGroup.ToString() : myvm.StorePointDate);
                    path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + docdirpath;
                    if (!string.Equals(docdirpath, myvm.DomainObject.DocDirPath))
                    {
                        pathfalse = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + myvm.DomainObject.DocDirPath;
                        if (!string.IsNullOrEmpty(myvm.DomainObject.DocDirPath) & System.IO.Directory.Exists(pathfalse))
                        {
                            if (System.IO.Directory.Exists(path))
                            {
                                foreach (string movepath in System.IO.Directory.EnumerateDirectories(pathfalse))
                                {
                                    System.IO.DirectoryInfo moveitem = new System.IO.DirectoryInfo(movepath);
                                    moveitem.MoveTo(path + "\\" + moveitem.Name);
                                }
                                foreach (string movepath in System.IO.Directory.EnumerateFiles(pathfalse))
                                {
                                    System.IO.FileInfo moveitem = new System.IO.FileInfo(movepath);
                                    moveitem.MoveTo(path + "\\" + moveitem.Name);
                                }
                                System.IO.Directory.Delete(pathfalse);
                            }
                            else
                                System.IO.Directory.Move(pathfalse, path);
                        }
                        myvm.DomainObject.DocDirPath = docdirpath;
                    }
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                this.PopupText = "Не удалось сохранить папку заявки" + myvm.StorePointDate + "!\nЗакройте все документы из этой папки и повторите сохранение.\n\n" + ex.Message;
                //lib.ExceptionHandler handler = new lib.ExceptionHandler("Сохранение изменений");
                //handler.Handle(new Exception("Не удалось сохранить папку заявки " + myvm.StorePointDate + "!\nЗакройте все документы из этой папки и повторите сохранение.\n\n" + ex.Message));
                //handler.ShowMessage();
            }
            //    if (isSuccess)
            //    {
            //        if (VM.DomainState == lib.DomainObjectState.Added)
            //        {
            //            VM.DomainObject.Id = mydatavm.Row.requestId;
            //            VM.DomainState = lib.DomainObjectState.Modified;
            //        }
            //        if (isSuccess && mydatavm.Row.stamp != (int)VM.DomainObject.Stamp) VM.DomainObject.Stamp = mydatavm.Row.stamp;
            //        isSuccess &= base.SaveDataChanges();
            //    }
            //    if (isSuccess && mydatavm.Row.stamp != (int)VM.DomainObject.Stamp) mydatavm.Row.stamp = (int)VM.DomainObject.Stamp;
            //    PopupIsOpen = true;
            bool isSuccess = base.SaveDataChanges();
            this.AlgorithmCommand.Save.Execute(null);
            this.AlgorithmConCommand.Save.Execute(null);
            if (this.AlgorithmCommand.PopupText != "Изменения сохранены")
            {
                isSuccess = false;
                this.PopupText = this.AlgorithmCommand.PopupText;
            }
            if (this.AlgorithmConCommand.PopupText != "Изменения сохранены")
            {
                isSuccess = false;
                this.PopupText = this.AlgorithmConCommand.PopupText;
            }
            return isSuccess;
        }
        protected override bool CanSaveDataChanges()
        {
            return base.CanSaveDataChanges() & !myvm.IsReadOnly;
        }
        internal static string MoveFolder(RequestVM item)
        {
            return item.MoveFolder();
        }
        protected override bool CanRejectChanges()
        {
            return base.CanRejectChanges() & !myvm.IsReadOnly;
        }

        public bool IsEditable
        {
            set
            {
                if (this.myvm.Status?.Id > 499)
                {
                    this.OpenPopup("Изменение данных невозможно. Перевозка закрыта!", false);
                    return;
                }
                if (value)
                {
                    myvm.IsReadOnly = !myvm.DomainObject.Blocking();
                    if (!myvm.IsReadOnly)
                        myvm.DomainObject.HoldBlocking = true;
                    this.SaveRefresh.Execute(null);
                }
                else if (!value && (myvm.DomainState != lib.DomainObjectState.Unchanged))
                {
                    this.OpenPopup("Сохраните или отмените изменения!", true);
                }
                else if (!value)
                {
                    myvm.IsReadOnly = true;
                    myvm.DomainObject.HoldBlocking = false;
                    myvm.DomainObject.UnBlocking();
                }
                myvm.DomainObject.AlgorithmCMD.IsReadOnly = myvm.IsReadOnly;
                myvm.DomainObject.AlgorithmConCMD.IsReadOnly = myvm.IsReadOnly;
                PropertyChangedNotification("IsEditable");
                PropertyChangedNotification("IsReadOnly");
            }
            get { return !myvm.IsReadOnly; }
        }
        public bool IsReadOnly
        { get { return myvm.IsReadOnly; } }
    }

    public class RequestSynchronizer : lib.ModelViewCollectionsSynchronizer<Request, RequestVM>
    {
        protected override Request UnWrap(RequestVM wrap)
        {
            return wrap.DomainObject as Request;
        }
        protected override RequestVM Wrap(Request fill)
        {
            return new RequestVM(fill);
        }
    }

    public class RequestViewCommand : lib.ViewModelViewCommand
    {
        internal RequestViewCommand()
        {
            myfilter = new SQLFilter("Request", "AND");
            myfilter.GetDefaultFilter(SQLFilterPart.Where);
            mydbm = new RequestDBM();
            mydbm.LegalDBM = new RequestCustomerLegalDBM();
            mydbm.Filter = myfilter.FilterWhereId;
            mydbm.FillAsyncCompleted = () => { if (mydbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); else mydbm.FillAsyncCompleted = () => { if (mydbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); else foreach (Request ritem in mydbm.Collection) ritem.CustomerLegalsRefresh(); }; };
            mydbm.FillAsync();
            mysync = new RequestSynchronizer();
            mysync.DomainCollection = mydbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            myfoldermove = new RelayCommand(FoldersMoveExec, FoldersMoveCanExec);
        }

        private SQLFilter myfilter;
        internal SQLFilter Filter
        { get { return myfilter; } }
        private new RequestDBM mydbm;
        private RequestSynchronizer mysync;

        public System.Windows.Media.Imaging.BitmapImage Funnel
        {
            get
            {
                string uribitmap;
                if (myfilter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
                else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
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
        private ListCollectionView mystatuses;
        public ListCollectionView Statuses
        {
            get
            {
                if (mystatuses == null)
                {
                    mystatuses = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
                    mystatuses.Filter = (item) => { return (item as lib.ReferenceSimpleItem).Id < 50; };
                }
                return mystatuses;
            }
        }
        private ListCollectionView myparceltypes;
        public ListCollectionView ParcelTypes
        {
            get
            {
                if (myparceltypes == null)
                {
                    myparceltypes = new ListCollectionView(CustomBrokerWpf.References.ParcelTypes);
                }
                return myparceltypes;
            }
        }
        private ListCollectionView myloaddescriptions;
        public ListCollectionView LoadDescriptions
        {
            get
            {
                if (myloaddescriptions == null)
                {
                    myloaddescriptions = new ListCollectionView(CustomBrokerWpf.References.GoodsTypesParcel);
                    myloaddescriptions.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myloaddescriptions;
            }
        }
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
        private ListCollectionView myimporters;
        public ListCollectionView Importers
        {
            get
            {
                if (myimporters == null)
                {
                    myimporters = new ListCollectionView(CustomBrokerWpf.References.Importers);
                    myimporters.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myimporters;
            }
        }

        private RelayCommand myfoldermove;
        public ICommand FoldersMove
        {
            get { return myfoldermove; }
        }
        private void FoldersMoveExec(object parametr)
        {
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            DateTime startdate;
            ParametrDBM pdbm = new ParametrDBM();
            pdbm.Id = "dirlm";
            Parameter par = pdbm.GetFirst();
            if (pdbm.Errors.Count > 0)
            {
                this.OpenPopup(pdbm.ErrorMessage, true);
                return;
            }
            if (!DateTime.TryParse(par?.Value, out startdate))
                startdate = DateTime.FromOADate(6576D); //{01.01.18 0:00:00}

            RequestDBM rdbm = new RequestDBM();
            rdbm.UpdateWhen = startdate;
            rdbm.Fill();
            foreach (Request item in rdbm.Collection)
            {
                RequestVM itemvm = new RequestVM(item);
                err.AppendLine(itemvm.MoveFolder());
            }

            if (err.Length == 0)
            {
                if (DateTime.Equals(startdate, DateTime.FromOADate(6576D)))
                {
                    par = new Parameter(0, "dirlm", string.Empty, "Дата последнего перемещения папок", DateTime.Today.ToShortDateString(), lib.DomainObjectState.Added);
                }
                else
                {
                    par.Value = DateTime.Today.ToShortDateString();
                }
                pdbm.SaveItemChanches(par);
                if (pdbm.Errors.Count > 0)
                {
                    this.OpenPopup(pdbm.ErrorMessage, true);
                }
                else
                    this.OpenPopup("Все папки обновлены", false);
            }
            else
            {
                this.OpenPopup(err.ToString(), true);
            }
        }
        private bool FoldersMoveCanExec(object parametr)
        { return true; }

        public override bool SaveDataChanges()
        {
            bool isSuccess = true;
            if (myview != null)
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                err.AppendLine("Изменения не сохранены");
                mydbm.Errors.Clear();
                foreach (RequestVM item in myview.SourceCollection)
                {
                    if ((item.DomainState == lib.DomainObjectState.Added || item.DomainState == lib.DomainObjectState.Modified))
                    {
                        if (!item.Validate(true))
                        {
                            err.AppendLine(item.Errors);
                            isSuccess = false;
                        }
                        else
                            err.AppendLine(item.DomainObject.UpdateDocDirPath());
                    }
                }
                if (!mydbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    err.AppendLine(mydbm.ErrorMessage);
                }
                if (!isSuccess)
                    this.PopupText = err.ToString();
            }
            return isSuccess;
        }
        protected override void AddData(object parametr)
        {
            if (parametr == null)
                myview.AddNew();
            else
                myview.AddNewItem(parametr);
        }
        protected override bool CanAddData(object parametr)
        {
            return !(myview.IsAddingNew | myview.IsEditingItem);
        }
        protected override bool CanDeleteData(object parametr)
        {
            return myview.CurrentItem != null && myview.CurrentItem is RequestVM;
        }
        protected override bool CanRefreshData()
        {
            return !(myview.IsAddingNew | myview.IsEditingItem);
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
            mydbm.FillType = lib.FillType.Refresh;
            mydbm.Filter = myfilter.FilterWhereId;
            mydbm.FillAsync();
        }
        protected override void RejectChanges(object parametr)
        {
            System.Collections.IList rejects;
            if (parametr is System.Collections.IList && (parametr as System.Collections.IList).Count > 0)
                rejects = parametr as System.Collections.IList;
            else
                rejects = mysync.ViewModelCollection;

            System.Collections.Generic.List<RequestVM> deleted = new System.Collections.Generic.List<RequestVM>();
            foreach (object item in rejects)
            {
                if (item is RequestVM)
                {
                    RequestVM ritem = item as RequestVM;
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
            foreach (RequestVM delitem in deleted)
            {
                mysync.ViewModelCollection.Remove(delitem);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
            }
        }
        protected override void SettingView() { }
    }
}
