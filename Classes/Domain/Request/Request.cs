using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Specification;
using System.Text;
using KirillPolyanskiy.DataModelClassLibrary;
using System.Collections.Generic;
using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using KirillPolyanskiy.DataModelClassLibrary.Filter;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.References;
using System.Threading;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    //internal enum ShipmentDelay { timely, expiring, expired }
    public struct RequestRecord
    {
        internal int id; 
        internal Int64 stamp;
        internal DateTime? updated;
        internal string updater;

        internal int? agent;
        internal int status;
        internal int? country;
        internal int currency;
        internal int? customer;
        internal int? freight;
        internal int? parcelgroup;
        internal int? parcel;
        internal int? store;
        internal  short? cellnumber;
        internal byte? statedoc;
        internal byte? stateexc;
        internal byte? stateinv;
        internal bool currencypa;
        internal bool specloaded;
        internal bool ttlpayinvoice;
        internal bool ttlpaycurrency;
        internal int? parceltype;
        internal decimal? additionalcost;
        internal decimal? additionalpay;
        internal decimal? actualweight;
        internal decimal? bringcost;
        internal decimal? bringpay;
        internal decimal? brokercost;
        internal decimal? brokerpay;
        internal decimal? currencyrate;
        internal decimal? currencysum;
        internal decimal? customscost;
        internal decimal? customspay;
        internal decimal? deliverycost;
        internal decimal? deliverypay;
        internal decimal? dtrate;
        internal decimal? goodvalue;
        internal decimal? freightcost;
        internal decimal? freightpay;
        internal decimal? insurancecost;
        internal decimal? insurancepay;
        internal decimal? invoice;
        internal decimal? invoicediscount;
        internal decimal? officialweight;
        internal decimal? preparatncost;
        internal decimal? preparatnpay;
        internal decimal? selling;
        internal decimal? sellingmarkup;
        internal decimal? sellingmarkuprate;
        internal decimal? sertificatcost;
        internal decimal? sertificatpay;
        internal decimal? tdcost;
        internal decimal? tdpay;
        internal decimal? volume;
        internal DateTime? currencydate;
        internal DateTime? currencypaiddate;
        internal DateTime? gtddate;
        internal DateTime requestdate;
        internal DateTime? shipplandate;
        internal DateTime? specification;
        internal DateTime? storedate;
        internal DateTime? storeinform;
        internal string algorithmnote1;
        internal string algorithmnote2;
        internal string cargo;
        internal string colormark;
        internal string consolidate;
        internal string currencynote;
        internal string customernote;
        internal string docdirpath;
        internal string gtd;
        internal string fullnumber;
        internal string managergroupname;
        internal string managernote;
        internal string mskstorenote;
        internal string servicetype;
        internal string storenote;
        internal string storepoint;
        internal int? importer;
        internal int? manager;
    }

    public class Request : lib.DomainStampValueChanged
    {
        bool mycurrencypaid, myisspecification, myttlpayinvoice, myttlpaycurrency;
        byte? mystatedoc, mystateexc, mystateinv;
        short? mycellnumber;
        int? myagentid, mycustomerid, mycustomerlegal, myfreightid, myparcelid, myparcelgroup, mystoreid;
        decimal? myadditionalpay, myadditionalcost, myactualweight, mybringcost, mybringpay, mybrokercost, mybrokerpay, mycorrcost, mycorrpay, mycurrencyrate, mycurrencysum, mycustomscost, mycustomspay, mydeliverycost, mydeliverypay, mydtrate, mygoodvalue, myfreightcost, myfreightpay, myinsurancecost, myinsurancepay, myinvoice, myinvoicediscount, myofficialweight, mypreparatncost, mypreparatnpay, myselling, mysellingmarkup, mysellingmarkuprate, mysertificatcost, mysertificatpay, mytdcost, mytdpay, myvolume;
        DateTime myrequestdate;
        DateTime? mycurrencydate, mycurrencypaiddate, mygtddate, myshipplandate, mystoredate, mystoreinform;
        string myalgorithmnote1, myalgorithmnote2, mycolormark, myconsolidate, mycurrencynote, mycustomernote, mycargo, mydocdirpath, mygtd, myfullnumber, mymanagergroup, mymanagernote, mymskstorenote, myservicetype, mystorenote, mystorepoint;
        lib.ReferenceSimpleItem mystatus, myparceltype;
        private Parcel myparcel;
        private Importer myimporter;

        public Request() : this(id: lib.NewObjectId.NewId, stamp: 0, updated: null, updater: null, domainstate: lib.DomainObjectState.Added
            , agent: null, agentid: null
            ,country: CustomBrokerWpf.References.Countries.First((References.Country item)=> { return item.RequestList && item.Code == 276; })
            , currency: 0, customer: null, customerid: null, customerlegal: null, freightid: null
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
            , algorithmnote1: "Свободное поле", algorithmnote2: null, cargo: null, colormark: null, consolidate: null, currencynote: null, customernote: null, docdirpath: null, gtd: null, fullnumber: null, managergroup: null, managernote: null, mskstorenote: null, servicetype: null, storenote: null, storepoint: null
            , importer: null, manager: null
            )
        { }
        public Request(int id, Int64 stamp, DateTime? updated, string updater, lib.DomainObjectState domainstate
            , Agent agent, lib.ReferenceSimpleItem status, int? agentid, References.Country country, int currency, Customer customer, int? customerid, int? customerlegal, int? freightid, int? parcelgroup, int? parcelid, int? storeid
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
            , DateTime? currencydate, DateTime? currencypaiddate, DateTime? gtddate, DateTime requestdate, DateTime? shipplandate, DateTime? specification, DateTime? storedate, DateTime? storeinform
            , string algorithmnote1, string algorithmnote2, string cargo, string colormark, string consolidate, string currencynote, string customernote, string docdirpath, string gtd, string fullnumber, string managergroup, string managernote, string mskstorenote, string servicetype, string storenote, string storepoint
            , Importer importer, Manager manager, Parcel parcel=null
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
            mycountry = country;
            mycurrency = currency;
            mycurrencydate = currencydate;
            mycurrencynote = currencynote;
            mycurrencypaid = currencypaid;
            mycurrencypaiddate = currencypaiddate;
            mycurrencyrate = currencyrate;
            mycurrencysum = currencysum;
            mycustomer = customer;
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
            mymskstorenote = mskstorenote;
            myofficialweight = officialweight;
            myparcelgroup = parcelgroup;
            myparcel = parcel;
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
        { set { SetProperty<Agent>(ref myagent, value, () => { this.AgentId = value?.Id; BrandRefresh(); }); } get { return myagent; } }
        public int? AgentId
        {
            set
            {
                base.SetProperty<int?>(ref myagentid, value, () => { this.Agent = myagentid.HasValue ? CustomBrokerWpf.References.AgentStore.GetItemLoad(myagentid.Value, out _) : null; });
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
        private string mybrandnames;
        public string BrandNames // refresh in BrandNamesRefresh and RequestBrand.Selected
        { get { return this.Brands!=null ? mybrandnames : mybrandnames; } } // to initialize the download
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
        private References.Country mycountry;
        public References.Country Country
        { set { base.SetProperty<References.Country>(ref mycountry, value); } get { return mycountry; } }
        private int mycurrency;
        public int Currency
        { set { base.SetProperty<int>(ref mycurrency, value); } get { return mycurrency; } }
        public DateTime? CurrencyDate
        {
            set
            {
                base.SetProperty<DateTime?>(ref mycurrencydate, value);
            }
            get { return mycurrencydate; }
        }
        public string CurrencyName
        { get { string name = string.Empty; switch (mycurrency) { case 0: name = "EURO"; break; case 1: name = "USD"; break; } return name; } }
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
        private Customer mycustomer;
        public Customer Customer
        { set { SetProperty<Customer>(ref mycustomer, value, () => { this.CustomerId = value?.Id; }); } get { return mycustomer; } }
        public int? CustomerId
        {
            set
            {
                base.SetProperty<int?>(ref mycustomerid, value, () => {
                    mycustomer = mycustomerid.HasValue ? CustomBrokerWpf.References.CustomerStore.GetItemLoad(mycustomerid.Value, out _) : null;
                    this.ManagerGroupName = mycustomer.ManagerGroup.Name;
                    base.PropertyChangedNotification("Customer");
                    base.PropertyChangedNotification("CustomerName");
                    RequestCustomerLegalDBM ldbm = App.Current.Dispatcher.Invoke<RequestCustomerLegalDBM>(() => { return new RequestCustomerLegalDBM(); });
                    this.CustomerLegalsRefresh(ldbm); });
            }
            get { return mycustomerid; }
        }
        public string CustomerName
        {
            get
            {
                //if (mycustomername == null & this.CustomerId.HasValue)
                //{
                //    ReferenceDS refds = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
                //    if (refds.tableCustomerName.Count == 0) refds.CustomerNameRefresh();
                //    System.Data.DataRow[] rows = refds.tableCustomerName.Select("customerID=" + this.CustomerId.Value.ToString());
                //    if (rows.Length > 0)
                //        mycustomername = (rows[0] as ReferenceDS.tableCustomerNameRow).customerName;
                //}
                return mycustomer?.Name;
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
                return mycustomerlegalsnames;
            }
        }
        private System.Windows.FontWeight mycustomerlegalsnamesfontweigh;
        public System.Windows.FontWeight CustomerLegalsNamesFontWeight
        { get { return mycustomerlegalsnamesfontweigh; } }
        public string CustomerNote
        {
            set
            {
                base.SetProperty<string>(ref mycustomernote, value);
            }
            get { return mycustomernote; }
        }
        public int ShipmentDelay
        {
            get { return !mystoredate.HasValue || mystoredate.Value.AddDays(5) > DateTime.Today || this.Parcel != null ? 0 : ( mystoredate.Value.AddDays(5) > CustomBrokerWpf.References.ParcelLastShipdate.Shipdate ? 1 : 2); }
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
            internal set { mymanagergroup = value; this.PropertyChangedNotification(nameof(this.ManagerGroupName)); }
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
        public string MSKStoreNote
        {
            set
            {
                base.SetProperty<string>(ref mymskstorenote, value);
            }
            get { return mymskstorenote; }
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
                base.SetProperty<Parcel>(ref myparcel, value, () => { 
                    this.SetPropertyOnValueChanged<int?>(ref myparcelid, value?.Id, nameof(this.ParcelId));
                    this.DeliveryCost = null;
                     });
            }
            get
            {
                if (myparcel == null & myparcelid != null)
                {
                    myparcel = CustomBrokerWpf.References.ParcelStore.GetItemLoad(myparcelid.Value, out _);
                    myparcel.PropertyChanged += Parcel_PropertyChanged;
                }
                return myparcel;
            }
        }
        public int? ParcelGroup
        {
            set
            {
                int? oldvalue = myparcelgroup;
                base.SetProperty<int?>(ref myparcelgroup, value,()=> { this.OnValueChanged("ParcelGroup", oldvalue, myparcelgroup); });
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
                //if (myspecification == null & this.ParcelId.HasValue)
                //{
                //    myspecification = CustomBrokerWpf.References.SpecificationStore.GetItemLoad(this, out _) ?? new Specification.Specification(
                //                        parcel: this.Parcel,
                //                        consolidate: this.Consolidate,
                //                        parcelgroup: string.IsNullOrEmpty(this.Consolidate) ? this.ParcelGroup : null,
                //                        request: string.IsNullOrEmpty(this.Consolidate) & !this.ParcelGroup.HasValue ? this : null,
                //                        agent: CustomBrokerWpf.References.AgentStore.GetItemLoad(this.AgentId.Value, out _),
                //                        importer: this.Importer);
                //    if (myspecification != null) PropertyChangedNotification(nameof(this.Specification));
                //}
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
                base.SetProperty<lib.ReferenceSimpleItem>(ref mystatus, value, () => {
                    this.PropertyChangedNotification(nameof(Request.MailStateStatus));
                    if (mystatus.Id == 104) UpdateGroupStatus();
                });
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
            get { return ((string.IsNullOrEmpty(this.StorePoint) ? (this.Id>0 ? this.Id.ToString().PadRight(7, ' ') : string.Empty) : this.StorePoint + " ") + (this.StoreDate.HasValue ? this.StoreDate.Value.ToShortDateString() : this.RequestDate.ToShortDateString())); }
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
                    myalgorithmcmd.RequestProperties.ValueChanged += RequestProperties_ValueChanged;
                }
                return myalgorithmcmd;
            }
        }
        private void RequestProperties_ValueChanged(object sender, ValueChangedEventArgs<object> e)
        {
            this.OnValueChanged(e.PropertyName,e.OldValue,e.NewValue);
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
                    this.PropertyChangedNotification("AdditionalCost");
                    this.AlgorithmCMD?.RequestProperties.SetDeliveryTotal();
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
                if (value.HasValue) value = decimal.Round(value.Value, 4);
                this.SetProperty<decimal?>(ref mybrokercost, value);
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
				if (value.HasValue) value = decimal.Round(value.Value, 4);
				if (this.DomainState < lib.DomainObjectState.Deleted && (mydeliverycost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mydeliverycost.Value, value.Value))))
                {
                    if (!myUnchangedPropertyCollection.ContainsKey(nameof(this.DeliveryCost)))
                        this.myUnchangedPropertyCollection.Add(nameof(this.DeliveryCost),mydeliverycost);
                    mydeliverycost = value;
                    this.PropertyChangedNotification(nameof(this.DeliveryCost));
                    this.AlgorithmCMD?.RequestProperties.SetDeliveryTotal();
                }
            }
            get
            {
                //if (this.Parcel != null && myimporter != null) // update properties depend on Parcel
                //{
                //    if (myimporter.Id == 1)
                //    {
                //        if (this.Parcel.TransportTUn.HasValue) // old algoritm if in Parcel missing Transport
                //            mydeliverycost = this.Parcel.TransportTUn.Value * myvolume;
                //    }
                //    else
                //    {
                //        if (this.Parcel.TransportDUn.HasValue)
                //            mydeliverycost = this.Parcel.TransportDUn.Value * myvolume;
                //    }
                //}
                return mydeliverycost; 
            }
        }
        public decimal? DeliveryPay
        {
            set
            {
                if (this.DomainState < lib.DomainObjectState.Deleted && (mydeliverypay.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mydeliverypay.Value, value.Value))))
                {
                    decimal? old = mydeliverypay;
                    mydeliverypay = value;
                    this.PropertyChangedNotification(nameof(Request.DeliveryPay));
                    this.OnValueChanged(nameof(Request.DeliveryPay), old, mydeliverypay);
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
                    this.AlgorithmCMD?.RequestProperties.SetDeliveryTotal();
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
                    this.PropertyChangedNotification("PreparatnCost");
                    this.AlgorithmCMD?.RequestProperties.SetDeliveryTotal();
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
        
        System.Windows.Controls.Primitives.Popup mymailstatepopup;
        private RequestMailState mymailstatestatus;
        internal RequestMailState MailStateStatus
        {
            get
            {
                if (mymailstatestatus == null || mymailstatestatus.MailStateId != this.Status.Id)
                {
                    mymailstatestatus = new RequestMailState(this, this.Status.Id);
                    mymailstatestatus.PropertyChanged += MailStateStatus_PropertyChanged;
                }
                return mymailstatestatus;
            }
        }
        private void MailStateStatus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChangedNotification("MailStateStatus" + e.PropertyName);
        }
        internal void SendMailStatus() // вынести вывод сообщения в более интерфейсный объект  
        {
            string message;
            bool iserr = false, isshow = true;
            this.MailStateStatus.Send(); // инициализация
            this.MailStateStatus.HandleSendErrors(out isshow, out message, out iserr);
            if (isshow)
            {
                mymailstatepopup = KirillPolyanskiy.Common.PopupCreator.GetPopup(message
                    , iserr ? new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFDDBE0")) : System.Windows.Media.Brushes.WhiteSmoke
                    , (iserr ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Black)
                    , System.Windows.Media.Brushes.Beige
                    , iserr
                    , System.Windows.Controls.Primitives.PlacementMode.Mouse
                    );
                mymailstatepopup.IsOpen = true;
                this.Messages.Add(new ModelMessage(this, "Request.SendMailStatus.MailStateStatus.HandleSendErrors", message, iserr ? "sendmail" : string.Empty, isshow));
            }
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

        private ObservableCollection<RequestBrand> mybrands;
        internal ObservableCollection<RequestBrand> Brands
        {
            get
            {
                if (mybrands == null)
                {
                    mybrands = App.Current.Dispatcher.Invoke<ObservableCollection<RequestBrand>>(() => { return new ObservableCollection<RequestBrand>(); });
                    BrandRefresh();
                }
                return mybrands;
            }
        }
        internal bool BrandsIsNull
        { get { return mybrands == null; } }
        private object mylegalslock;
        private ObservableCollection<RequestCustomerLegal> mylegals;
        internal ObservableCollection<RequestCustomerLegal> CustomerLegals
        {
            get
            {
                if (mylegals == null)
                {
                    mylegals = App.Current.Dispatcher.Invoke<ObservableCollection<RequestCustomerLegal>>(() => { return new ObservableCollection<RequestCustomerLegal>(); });
                    //if (this.CustomerId.HasValue)
                    //{
                    //    RequestCustomerLegalDBM ldbm;
                    //    ldbm = App.Current.Dispatcher.Invoke<RequestCustomerLegalDBM>(() => { return new RequestCustomerLegalDBM(); });
                    //    ldbm.FillType = lib.FillType.PrefExist;
                    //    CustomerLegalsFill(ldbm);
                    //}
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
                if (!dirty)
                {
                    if (this.DirtyThread.Contains(System.Threading.Thread.CurrentThread.ManagedThreadId))
                        return false;
                    else
                        this.DirtyThread.Add(System.Threading.Thread.CurrentThread.ManagedThreadId);
                }
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
                if (!dirty && mybrands != null)
                    foreach (RequestBrand item in mybrands)
                    { dirty |= item.IsDirty; }
                this.DirtyThread.Remove(System.Threading.Thread.CurrentThread.ManagedThreadId);
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
                case nameof(this.Agent):
                    myagent = (Agent)value;
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
                case nameof(this.Country):
                    mycountry = (References.Country)value;
                    break;
                case "Currency":
                    mycurrency = (int)value;
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
                case nameof(this.Customer):
                    mycustomer = (Customer)value;
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
                case nameof(Request.MSKStoreNote):
                    mymskstorenote = (string)value;
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
            this.BringCost = newitem.BringCost;
            this.BringPay = newitem.BringPay;
            this.BrokerCost = newitem.BrokerCost;
            this.BrokerPay = newitem.BrokerPay;
            this.Cargo = newitem.Cargo;
            this.CellNumber = newitem.CellNumber;
            this.ColorMark = newitem.ColorMark;
            this.Consolidate = newitem.Consolidate;
            this.TotalCost = newitem.TotalCost;
            this.Country = newitem.Country;
            this.Currency = newitem.Currency;
            this.CurrencyDate = newitem.CurrencyDate;
            this.CurrencyNote = newitem.CurrencyNote;
            this.CurrencyPaid = newitem.CurrencyPaid;
            this.CurrencyPaidDate = newitem.CurrencyPaidDate;
            this.CurrencyRate = newitem.CurrencyRate;
            this.CurrencySum = newitem.CurrencySum;
            this.Customer = newitem.Customer;
            this.CustomerId = newitem.CustomerId;
            this.ManagerGroupName = newitem.ManagerGroupName;
            this.CustomerLegal = newitem.CustomerLegal;
            this.CustomerNote = newitem.CustomerNote;
            this.DeliveryCost = newitem.DeliveryCost;
            this.DeliveryPay = newitem.DeliveryPay;
            this.DocDirPath = newitem.DocDirPath;
            this.DTRate = newitem.DTRate;
            this.FreightId = newitem.FreightId;
            this.FreightCost = newitem.FreightCost;
            this.FreightPay = newitem.FreightPay;
            this.GoodValue = newitem.GoodValue;
            this.GTD = newitem.GTD;
            this.GTDDate = newitem.GTDDate;
            this.Importer = newitem.Importer;
            this.Invoice = newitem.Invoice;
            this.InvoiceDiscount = newitem.InvoiceDiscount;
            this.IsSpecification = newitem.IsSpecification;
            this.ManagerNote = newitem.ManagerNote;
            this.MSKStoreNote = newitem.MSKStoreNote;
            this.Manager = newitem.Manager;
            this.OfficialWeight = newitem.OfficialWeight;
            this.ParcelGroup = newitem.ParcelGroup;
            if (this.ParcelId != newitem.ParcelId) this.ParcelId = newitem.ParcelId; // because parcel not to null
            if (!string.Equals(myfullnumber, newitem.ParcelNumber)) myfullnumber = newitem.ParcelNumber; PropertyChangedNotification("ParcelNumber");
            this.ParcelType = newitem.ParcelType;
            this.PreparatnCost = newitem.PreparatnCost;
            this.PreparatnPay = newitem.PreparatnPay;
            this.RequestDate = newitem.RequestDate;
            this.Selling = newitem.Selling;
            this.SellingMarkup = newitem.SellingMarkup;
            this.SellingMarkupRate = newitem.SellingMarkupRate;
            this.SertificatCost = newitem.SertificatCost;
            this.SertificatPay = newitem.SertificatPay;
            this.ServiceType = newitem.ServiceType;
            this.ShipPlanDate = newitem.ShipPlanDate;
            this.Specification = newitem.Specification;
            this.SpecificationDate = newitem.SpecificationDate;
            this.StateDoc = newitem.StateDoc;
            this.StateExc = newitem.StateExc;
            this.StateInv = newitem.StateInv;
            this.Status = newitem.Status;
            this.StoreDate = newitem.StoreDate;
            this.StoreId = newitem.StoreId;
            this.StoreInform = newitem.StoreInform;
            this.StoreNote = newitem.StoreNote;
            this.StorePoint = newitem.StorePoint;
            this.TtlPayInvoice = newitem.TtlPayInvoice;
            this.TtlPayCurrency = newitem.TtlPayCurrency;
            this.Volume = newitem.Volume;
            this.UpdatingSample = false;
            if (mymailstatestock != null) mymailstatestock.Update();
            if (mymailstatetakegoods9 != null) mymailstatetakegoods9.Update();
            if(mybrands!=null) BrandRefresh();
        }
        public override bool ValidateProperty(string propertyname, object value, out string errmsg, out byte messageKey)
        {
            bool isvalid = true;
            errmsg = null;
            messageKey = 0;
            if (myupdate) return true;
            switch (propertyname)
            {
                case nameof(this.AgentId):
                    if (value == null) //!this.CustomerLegalsIsNull && this.CustomerLegals.Count > 0 && 
                    {
                        errmsg = "В заявке  " + (this.StorePointDate ?? (this.Id > 0 ? this.Id.ToString() : string.Empty)) + " необходимо указать поставщика!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.Brands):
                    if(!(this.BrandsIsNull || this.Brands.Count==0 || this.Brands.Any((RequestBrand item)=> { return item.Selected; })))
                    {
                        errmsg = "В заявке " + (this.StorePointDate??(this.Id>0 ? this.Id.ToString() : string.Empty)) + " необходимо указать торговые марки!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.Cargo):
                    if (string.IsNullOrEmpty((string)value))
                    {
                        errmsg = "В заявке " + (this.Id > 0 ? this.Id.ToString() : string.Empty) + " необходимо указать описание груза!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.Country):
                    if(value==null)
                    {
                        errmsg = "В заявке " + (this.StorePointDate ?? (this.Id > 0 ? this.Id.ToString() : string.Empty)) + " необходимо указать страну!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.Customer):
                    if (value == null)
                    {
                        errmsg = "В заявке  " + (this.StorePointDate ?? (this.Id > 0 ? this.Id.ToString() : string.Empty)) + " необходимо указать клиента!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.Importer):
                    if (value == null) //!this.CustomerLegalsIsNull && this.CustomerLegals.Count>0 && 
                    {
                        errmsg = "В заявке  " + (this.StorePointDate ?? (this.Id > 0 ? this.Id.ToString() : string.Empty)) + " необходимо указать импортера!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.InvoiceDiscount):
                    int legals = this.CustomerLegals?.Where((RequestCustomerLegal item) => { return item.Selected; }).Count() ?? 0;
                    if (((decimal?)value??0M)>0M && legals == 0)
                    {
                        errmsg = "У заявки  " + (this.StorePointDate ?? (this.Id > 0 ? this.Id.ToString() : string.Empty)) + " нет юр. лиц!";
                        messageKey = 1;
                        isvalid = false;
                    }
                    if (((decimal?)value ?? 0M) > 0M && this.InvoiceDiscount!=(decimal?)value && legals>1) //( || (this.CustomerLegals?.Where((RequestCustomerLegal item) => { return item.Selected; }).Sum((RequestCustomerLegal item) => { return item.Prepays.Count; })??0) > 1)
                    {
                        errmsg = "У заявки  "+ (this.StorePointDate ?? (this.Id > 0 ? this.Id.ToString() : string.Empty)) + " несколько юр. лиц! Для изменения суммы воспользуйтесь разделом оплат в карточке заявки.";
                        isvalid = false;
                    }
                    break;
                case nameof(this.ShipPlanDate):
                    if(!(this.CustomerLegalsIsNull || this.CustomerLegals.Count == 0 || ((DateTime?)value).HasValue))
                    {
                        errmsg = "В заявке  " + (this.Id > 0 ? this.Id.ToString() : string.Empty) + " необходимо указать предполагаемую дату отгрузки!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.ServiceType):
                    if(string.IsNullOrEmpty((string)value) && !this.CustomerLegalsIsNull && this.CustomerLegals.Count > 0 && (this.CustomerLegals?.Where((RequestCustomerLegal item) => { return item.Selected; }).Sum((RequestCustomerLegal item) => { return item.Prepays.Sum((PrepayCustomerRequest prepay) => { return prepay.EuroSum; }); }) ?? 0M) > 0M)
                    {
                        errmsg = "В заявке " + (this.Id > 0 ? this.Id.ToString() : string.Empty) + " необходимо указать услугу!";
                        isvalid = false;
                    }
                    break;
                case nameof(Request.Status):
                    int id = ((lib.ReferenceSimpleItem)value).Id;
                    if (id > 99) //есть товары которые не растаможиваются
                    {
                        if (this.SpecificationIsNull)
                            try { this.SpecificationInit = CustomBrokerWpf.References.SpecificationStore.GetItemLoad(this, out _); } catch { }
                        if (!this.SpecificationIsNull && this.Specification.Declaration?.Number == null)
                        {
                            errmsg = "Статус заявки " + (this.Id > 0 ? this.Id.ToString() : string.Empty) + " не может быть повышен до " + ((lib.ReferenceSimpleItem)value).Name + " нет таможенной декларации!";
                            isvalid = false;
                        }
                    }
                    break;
            }
            return isvalid;
        }
        public override void AcceptChanches()
        {
            bool sendmail = CustomBrokerWpf.References.CurrentUserRoles.Contains("Managers")
                && this.HasPropertyOutdatedValue(nameof(Request.Status)) && this.Status != this.GetPropertyOutdatedValue(nameof(Request.Status))
                & ( this.Status.Id == 1
                    || this.Status.Id == 30
                    || this.Status.Id == 70
                    || this.Status.Id == 90
                    || this.Status.Id == 100
                    || this.Status.Id == 104
                    || this.Status.Id == 107);
            base.AcceptChanches();
            if(sendmail)
                SendMailStatus();
        }

        private bool myupdate;
        internal void BrandRefresh()
        {
            if (this.AgentId.HasValue)
            {
                RequestBrandDBM bdbm;
                bdbm = App.Current.Dispatcher.Invoke<RequestBrandDBM>(() => { return new RequestBrandDBM(); });
                bdbm.Collection = mybrands;
                bdbm.Request = this;
                bdbm.Agent = this.Agent;
                bdbm.FillAsyncCompleted = () => { this.PropertyChangedNotification(nameof(this.Brands)); BrandNamesRefresh(); };
                bdbm.FillAsync();
            }
        }
        internal void BrandNamesRefresh()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (RequestBrand item in this.Brands.OrderBy((RequestBrand item)=> { return item.Brand.Brand.Name; }))
            {
                if (item.Selected)
                {
                    if (str.Length > 0)
                        str.Append(", ");
                    str.Append(item.Brand?.Brand?.Name);
                }
            }
            mybrandnames = str.ToString();
            App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => { this.PropertyChangedNotification(nameof(this.BrandNames)); }));
        }
        internal string CustomerLegalsFill(RequestCustomerLegalDBM ldbm)
        {
            lock (mylegalslock)
            {
                if (mylegals == null && ldbm == null) return string.Empty;
                ldbm.Request = this;
                if (mylegals != null) ldbm.Collection = mylegals;
                myupdate = true;
                ldbm.Fill();
                myupdate = false;
                mylegals = ldbm.Collection;
                foreach (RequestCustomerLegal item in mylegals)
                {
                    item.PropertyChanged -= RequestCustomerLegal_PropertyChanged;
                    item.PropertyChanged += RequestCustomerLegal_PropertyChanged;
                }
                this.PropertyChangedNotification(nameof(CustomerLegals));
                this.PropertyChangedNotification(nameof(InvoiceDiscountFill));
            }
            return ldbm?.ErrorMessage;
        }
        internal void CustomerLegalsRefresh(RequestCustomerLegalDBM ldbm)
        {
            CustomerLegalsFill(ldbm);
            CustomerLegalsNamesFill();
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.PropertyChangedNotification("CustomerLegals");
                this.PropertyChangedNotification("CustomerLegalsSelected");
            }));
        }
        private void RequestCustomerLegal_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Selected")
            { 
                CustomerLegalsNamesFill();
                this.PropertyChangedNotification("CustomerLegalsSelected");
            }
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
                    case "InvoiceDiscount": // Пытаемся устанвить 0 для всех Legal
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
                        //if (!string.IsNullOrEmpty(this.DocDirPath) & System.IO.Directory.Exists(pathfalse))
                        //{
                        //    if (System.IO.Directory.Exists(path))
                        //    {
                        //        foreach (string movepath in System.IO.Directory.EnumerateDirectories(pathfalse))
                        //        {
                        //            System.IO.DirectoryInfo movethis = new System.IO.DirectoryInfo(movepath);
                        //            movethis.MoveTo(path + "\\" + movethis.Name);
                        //        }
                        //        foreach (string movepath in System.IO.Directory.EnumerateFiles(pathfalse))
                        //        {
                        //            System.IO.FileInfo movethis = new System.IO.FileInfo(movepath);
                        //            movethis.MoveTo(path + "\\" + movethis.Name);
                        //        }
                        //        System.IO.Directory.Delete(pathfalse);
                        //    }
                        //    else
                        //        System.IO.Directory.Move(pathfalse, path);
                        //}
                        this.DocDirPath = docdirpath;
                    }
                    //else if (!System.IO.Directory.Exists(path))
                    //    System.IO.Directory.CreateDirectory(path);
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
            mycustomerlegalsnamesfontweigh = System.Windows.FontWeights.Normal;
            foreach (RequestCustomerLegal item in this.CustomerLegals)
            {
                if (item.Selected)
                {
                    if (str.Length > 0)
                        str.Append(", ");
                    str.Append(item.CustomerLegal?.Name);
                    if (mycustomerlegalsnamesfontweigh == System.Windows.FontWeights.Normal && (item.CustomerLegal?.isNoteSpecial ?? false))
                    {
                        mycustomerlegalsnamesfontweigh = System.Windows.FontWeights.Bold;
                        App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => { this.PropertyChangedNotification(nameof(this.CustomerLegalsNamesFontWeight)); }));
                    }
                }
            }
            mycustomerlegalsnames = str.ToString();
            App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => { this.PropertyChangedNotification(nameof(this.CustomerLegalsNames)); }));
        }
        internal void DocFolderOpen()
        {
            try
            {
                string path;
                if (this.Importer==null)
                {
                    MessageBox.Show("Не указан поставщик!", "Папка документов", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (this.Parcel==null)
                {
                    MessageBox.Show("Заявка не поставлена в загруку!", "Папка документов", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + this.Parcel.ParcelNumber +"\\_"+ this.Importer.Name;
                if (System.IO.Directory.Exists(path))
                {
                    System.Diagnostics.Process.Start(path);
                }
                else
                    MessageBox.Show("Папка документов " + path + " не найдена!", "Папка документов", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Папка документов");
            }
        }
        private bool UpdateGroupStatus()
        {
            bool success = true;
            try
            {
                // новый WarehouseRu создается при сохранении
                if (this.ParcelGroup != null)
                {
                    foreach (Request req in this.Parcel.Requests)
                    {
                        if (req.ParcelGroup == this.ParcelGroup & req.Status != this.Status)
                        {
                            req.Status = this.Status;
                            break; // req обновит следующего
                        }
                    }
                }
            }
            catch
            { success = false; }
            return success;
        }
        private void Parcel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(Parcel.TransportDUn) when myimporter?.Id==2: // old algoritm if in Parcel missing Transport
                    if(this.Parcel.TransportDUn.HasValue)
                        this.DeliveryCost = this.Parcel.TransportDUn.Value * myvolume;
                    else if(this.AlgorithmCMD?.Algorithm?.Formulas!=null) // если алгоритм уже был расчитан возмем из алгоритма
                        foreach (Algorithm.AlgorithmValuesRequest values in this.AlgorithmCMD.Algorithm.Formulas)
                            if (values.Formula.Code == "П14")
                            {
                                this.DeliveryCost = values.Value1;
                                break;
                            }
                    break;
                case nameof(Parcel.TransportTUn) when myimporter?.Id == 1:
                    if(Parcel.TransportTUn.HasValue)
                        this.DeliveryCost = this.Parcel.TransportTUn.Value * myvolume;
                    else if (this.AlgorithmCMD?.Algorithm?.Formulas != null) // если алгоритм уже был расчитан возмем из алгоритма
                        foreach (Algorithm.AlgorithmValuesRequest values in this.AlgorithmCMD.Algorithm.Formulas)
                            if (values.Formula.Code == "П14")
                            {
                                this.DeliveryCost = values.Value1;
                                break;
                            }
                    break;
            }
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
            if (this.Blocked | this.DomainState == lib.DomainObjectState.Added) return true;
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
                if (Application.Current.Dispatcher.Thread.ManagedThreadId == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
                {
                    Window active = null;
                    foreach (Window win in Application.Current.Windows)
                        if (win.IsActive) { active = win; break; }
                    active.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        if (mypopupblock == null || !mypopupblock.IsOpen)
                        {
                            mypopupblock = Common.PopupCreator.GetPopup(text: msg.Replace("Объект", "Заявка " + this.StorePointDate)
                       , background: System.Windows.Media.Brushes.LightPink
                       , foreground: System.Windows.Media.Brushes.Red
                       , staysopen: true
                       );
                            mypopupblock.IsOpen = true;
                        }
                    }));
                }
                else
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        Window active = null;
                        foreach (Window win in Application.Current.Windows)
                            if (win.IsActive) { active = win; break; }
                        active.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                        {
                            if (mypopupblock == null || !mypopupblock.IsOpen)
                            {
                                mypopupblock = Common.PopupCreator.GetPopup(text: msg.Replace("Объект", "Заявка " + this.StorePointDate)
                           , background: System.Windows.Media.Brushes.LightPink
                           , foreground: System.Windows.Media.Brushes.Red
                           , staysopen: true
                           );
                                mypopupblock.IsOpen = true;
                            }
                        }));
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
        //#region ITotalValuesItem
        //public bool ProcessedIn { get; set; }
        //public bool ProcessedOut { get; set; }
        //public bool Selected { get; set; }
        //#endregion
    }

    public class RequestDBM : lib.DBManagerWhoWhen<RequestRecord,Request>
    {
        public RequestDBM()
        {
            base.NeedAddConnection = true;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.Request_sp";
            InsertCommandText = "dbo.RequestAdd_sp";
            UpdateCommandText = "dbo.RequestUpd_sp";
            DeleteCommandText = "dbo.RequestDel_sp";

            SelectParams = new SqlParameter[] 
            {
                new SqlParameter("@id", System.Data.SqlDbType.Int),
                new SqlParameter("@storagepoint", System.Data.SqlDbType.NChar,6),
                new SqlParameter("@filterId", System.Data.SqlDbType.Int){ Value = 0},
                new SqlParameter("@parcel", System.Data.SqlDbType.Int),
                new SqlParameter("@datechanged", System.Data.SqlDbType.DateTime),
                new SqlParameter("@consolidate", System.Data.SqlDbType.NVarChar,5)
            };
            myinsertparams[0].ParameterName = "@requestId";
            myupdateparams[0].ParameterName = "@requestId";
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
                //,new SqlParameter("@specificationtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@storagePointtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@storageDatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@customerIdtrue", System.Data.SqlDbType.Bit)
                //,new SqlParameter("@customerlegaltrue", System.Data.SqlDbType.Bit)
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
                ,new SqlParameter("@mskstorenotetrue", System.Data.SqlDbType.Bit)
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
                ,new SqlParameter("@storageInformtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@countrytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@old", false)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0],myinsertupdateparams[1],myinsertupdateparams[2]
                ,new SqlParameter("@status", System.Data.SqlDbType.Int){Direction = System.Data.ParameterDirection.InputOutput}
                //,new SqlParameter("@specification", System.Data.SqlDbType.Date)
                ,new SqlParameter("@storagePoint", System.Data.SqlDbType.NVarChar,6)
                ,new SqlParameter("@storageDate", System.Data.SqlDbType.Date)
                ,new SqlParameter("@customerId", System.Data.SqlDbType.Int)
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
                ,new SqlParameter("@mskstorenote", System.Data.SqlDbType.NVarChar,200)
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
                ,new SqlParameter("@storageInform", System.Data.SqlDbType.Date)
                ,new SqlParameter("@currency", System.Data.SqlDbType.Int)
                ,new SqlParameter("@country", System.Data.SqlDbType.Int)
            };
            mydeleteparams = new SqlParameter[]
            {
                mydeleteparams[0]
                ,new SqlParameter("@stamp", System.Data.SqlDbType.Int)
            };

            mybdbm = new RequestBrandDBM();
            //mypmdbm = new RequestPaymentDBM(); mypmdbm.Command = new SqlCommand();
            myldbm = new RequestCustomerLegalDBM();
            myspdbm = new SpecificationDBM();
        }

        private RequestBrandDBM mybdbm;
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

        internal string Consolidate { set; get; }
        internal int Filter { set; get; }
        internal Parcel Parcel { set; get; }
        internal string StorePoint { set; get; }
        internal DateTime? UpdateWhen { set; get; }
        internal bool SpecificationLoad { set; get; }

        protected override RequestRecord CreateRecord(SqlDataReader reader)
        {
                return new RequestRecord()
                {
                    id = reader.GetInt32(0), stamp=reader.GetInt32(this.Fields["stamp"]), updated=reader.GetDateTime(this.Fields["UpdateWhen"]), updater=reader.GetString(this.Fields["UpdateWho"])
                    , status=reader.GetInt32(this.Fields["status"])
                    , agent = reader.IsDBNull(this.Fields["agentId"]) ? (int?)null : reader.GetInt32(this.Fields["agentId"])
                    , country=reader.IsDBNull(this.Fields["country"]) ? (int?)null : reader.GetInt32(this.Fields["country"])
                    , currency=reader.GetInt32(this.Fields["currency"])
                    , customer = reader.IsDBNull(this.Fields["customerId"]) ? (int?)null : reader.GetInt32(this.Fields["customerId"])
                    , freight=reader.IsDBNull(this.Fields["freight"]) ? (int?)null : reader.GetInt32(this.Fields["freight"])
                    , parcelgroup=reader.IsDBNull(this.Fields["parcelgroup"]) ? (int?)null : reader.GetInt32(this.Fields["parcelgroup"])
                    , parcel=reader.IsDBNull(this.Fields["parcel"]) ? (int?)null : reader.GetInt32(this.Fields["parcel"])
                    , store=reader.IsDBNull(this.Fields["storeid"]) ? (int?)null : reader.GetInt32(this.Fields["storeid"])
                    , cellnumber=reader.IsDBNull(this.Fields["cellNumber"]) ? (short?)null : reader.GetInt16(this.Fields["cellNumber"])
                    , statedoc=reader.IsDBNull(this.Fields["statedoc"]) ? (byte?)null : reader.GetByte(this.Fields["statedoc"])
                    , stateexc=reader.IsDBNull(this.Fields["stateexc"]) ? (byte?)null : reader.GetByte(this.Fields["stateexc"])
                    , stateinv=reader.IsDBNull(this.Fields["stateinv"]) ? (byte?)null : reader.GetByte(this.Fields["stateinv"])
                    , currencypa=reader.IsDBNull(this.Fields["currencypaid"]) ? false : reader.GetBoolean(this.Fields["currencypaid"])
                    , specloaded=reader.IsDBNull(this.Fields["specloaded"]) ? false : reader.GetBoolean(this.Fields["specloaded"])
                    , ttlpayinvoice=reader.IsDBNull(this.Fields["ttlpayinvoice"]) ? false : reader.GetBoolean(this.Fields["ttlpayinvoice"])
                    , ttlpaycurrency=reader.IsDBNull(this.Fields["ttlpaycurrency"]) ? false : reader.GetBoolean(this.Fields["ttlpaycurrency"])
                    , parceltype=reader.IsDBNull(this.Fields["parceltype"]) ? (int?)null : (int)reader.GetByte(this.Fields["parceltype"])
                    , additionalcost=reader.IsDBNull(this.Fields["additionalcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["additionalcost"])
                    , additionalpay=reader.IsDBNull(this.Fields["additionalpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["additionalpay"])
                    , actualweight=reader.IsDBNull(this.Fields["actualWeight"]) ? (decimal?)null : reader.GetDecimal(this.Fields["actualWeight"])
                    , bringcost=reader.IsDBNull(this.Fields["bringcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["bringcost"])
                    , bringpay=reader.IsDBNull(this.Fields["bringpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["bringpay"])
                    , brokercost=reader.IsDBNull(this.Fields["brokercost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["brokercost"])
                    , brokerpay=reader.IsDBNull(this.Fields["brokerpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["brokerpay"])
                    , currencyrate=reader.IsDBNull(this.Fields["currencyrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["currencyrate"])
                    , currencysum=reader.IsDBNull(this.Fields["currencysum"]) ? (decimal?)null : reader.GetDecimal(this.Fields["currencysum"])
                    , customscost=reader.IsDBNull(this.Fields["customscost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["customscost"])
                    , customspay=reader.IsDBNull(this.Fields["customspay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["customspay"])
                    , deliverycost=reader.IsDBNull(this.Fields["deliverycost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["deliverycost"])
                    , deliverypay=reader.IsDBNull(this.Fields["deliverypay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["deliverypay"])
                    , dtrate=reader.IsDBNull(this.Fields["dtrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["dtrate"])
                    , goodvalue=reader.IsDBNull(this.Fields["goodValue"]) ? (decimal?)null : reader.GetDecimal(this.Fields["goodValue"])
                    , freightcost=reader.IsDBNull(this.Fields["freightcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["freightcost"])
                    , freightpay=reader.IsDBNull(this.Fields["freightpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["freightpay"])
                    , insurancecost=reader.IsDBNull(this.Fields["insurancecost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["insurancecost"])
                    , insurancepay=reader.IsDBNull(this.Fields["insurancepay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["insurancepay"])
                    , invoice=reader.IsDBNull(this.Fields["invoice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["invoice"])
                    , invoicediscount=reader.IsDBNull(this.Fields["invoicediscount"]) ? (decimal?)null : reader.GetDecimal(this.Fields["invoicediscount"])
                    , officialweight=reader.IsDBNull(this.Fields["officialWeight"]) ? (decimal?)null : reader.GetDecimal(this.Fields["officialWeight"])
                    , preparatncost=reader.IsDBNull(this.Fields["preparatncost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["preparatncost"])
                    , preparatnpay=reader.IsDBNull(this.Fields["preparatnpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["preparatnpay"])
                    , selling=reader.IsDBNull(this.Fields["selling"]) ? (decimal?)null : reader.GetDecimal(this.Fields["selling"])
                    , sellingmarkup=reader.IsDBNull(this.Fields["sellingmarkup"]) ? (decimal?)null : reader.GetDecimal(this.Fields["sellingmarkup"])
                    , sellingmarkuprate=reader.IsDBNull(this.Fields["sellingmarkuprate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["sellingmarkuprate"])
                    , sertificatcost=reader.IsDBNull(this.Fields["sertificatcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["sertificatcost"])
                    , sertificatpay=reader.IsDBNull(this.Fields["sertificatpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["sertificatpay"])
                    , tdcost=reader.IsDBNull(this.Fields["tdcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["tdcost"])
                    , tdpay=reader.IsDBNull(this.Fields["tdpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["tdpay"])
                    , volume=reader.IsDBNull(this.Fields["volume"]) ? (decimal?)null : reader.GetDecimal(this.Fields["volume"])
                    , currencydate=reader.IsDBNull(this.Fields["currencydate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["currencydate"])
                    , currencypaiddate=reader.IsDBNull(this.Fields["currencypaiddate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["currencypaiddate"])
                    , gtddate=reader.IsDBNull(this.Fields["gtddate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["gtddate"])
                    , requestdate=reader.GetDateTime(this.Fields["requestDate"])
                    , shipplandate=reader.IsDBNull(this.Fields["shipplandate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["shipplandate"])
                    , specification=reader.IsDBNull(this.Fields["specification"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["specification"])
                    , storedate=reader.IsDBNull(this.Fields["storageDate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["storageDate"])
                    , storeinform=reader.IsDBNull(this.Fields["storageInform"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["storageInform"])
                    , algorithmnote1=reader.IsDBNull(this.Fields["algorithmnote1"]) ? null : reader.GetString(this.Fields["algorithmnote1"])
                    , algorithmnote2=reader.IsDBNull(this.Fields["algorithmnote2"]) ? null : reader.GetString(this.Fields["algorithmnote2"])
                    , cargo=reader.IsDBNull(this.Fields["loadDescription"]) ? null : reader.GetString(this.Fields["loadDescription"])
                    , colormark=reader.IsDBNull(this.Fields["colorMark"]) ? null : reader.GetString(this.Fields["colorMark"])
                    , consolidate=reader.IsDBNull(this.Fields["consolidate"]) ? null : reader.GetString(this.Fields["consolidate"])
                    , currencynote=reader.IsDBNull(this.Fields["currencynote"]) ? null : reader.GetString(this.Fields["currencynote"])
                    , customernote=reader.IsDBNull(this.Fields["customerNote"]) ? null : reader.GetString(this.Fields["customerNote"])
                    , docdirpath=reader.IsDBNull(this.Fields["docdirpath"]) ? null : reader.GetString(this.Fields["docdirpath"])
                    , gtd=reader.IsDBNull(this.Fields["gtd"]) ? null : reader.GetString(this.Fields["gtd"])
                    , fullnumber=reader.IsDBNull(this.Fields["fullnumber"]) ? null : reader.GetString(this.Fields["fullnumber"])
                    , managergroupname=reader.IsDBNull(this.Fields["managergroupName"]) ? null : reader.GetString(this.Fields["managergroupName"])
                    , managernote=reader.IsDBNull(this.Fields["managerNote"]) ? null : reader.GetString(this.Fields["managerNote"])
                    , mskstorenote=reader.IsDBNull(this.Fields["mskstorenote"]) ? null : reader.GetString(this.Fields["mskstorenote"])
                    , servicetype=reader.IsDBNull(this.Fields["servicetype"]) ? null : reader.GetString(this.Fields["servicetype"])
                    , storenote=reader.IsDBNull(this.Fields["storageNote"]) ? null : reader.GetString(this.Fields["storageNote"])
                    , storepoint=reader.IsDBNull(this.Fields["storagePoint"]) ? null : reader.GetString(this.Fields["storagePoint"])
                    , importer=reader.IsDBNull(this.Fields["importer"]) ? (int?)null : reader.GetInt32(this.Fields["importer"])
                    , manager=reader.IsDBNull(this.Fields["managerid"]) ? (int?)null : reader.GetInt32(this.Fields["managerid"])
                };
        }
        protected override Request CreateModel(RequestRecord record,SqlConnection addcon, CancellationToken canceltasktoken = default)
        {
            Request request = null;
            if (this.FillType == lib.FillType.PrefExist)
                request = CustomBrokerWpf.References.RequestStore.GetItem(record.id);
            if(request == null)
            {
                List<lib.DBMError> errors=new List<DBMError>();
                Agent agent = record.agent.HasValue ? CustomBrokerWpf.References.AgentStore.GetItemLoad(record.agent.Value, addcon, out errors) : null;
                this.Errors.AddRange(errors);
                Customer customer = record.customer.HasValue ? CustomBrokerWpf.References.CustomerStore.GetItemLoad(record.customer.Value, addcon, out errors) : null;
                this.Errors.AddRange(errors);
                Request newitem = new Request(record.id, record.stamp, record.updated, record.updater, lib.DomainObjectState.Unchanged
                    , agent
                    , CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", record.status)
                    , record.agent
                    , record.country.HasValue ? CustomBrokerWpf.References.Countries.FindFirstItem("Code", record.country.Value) : null
                    , record.currency
                    , customer
                    , record.customer
                    , (int?)null
                    , record.freight
                    , record.parcelgroup
                    , record.parcel
                    , record.store
                    , record.cellnumber
                    , record.statedoc
                    , record.stateexc
                    , record.stateinv
                    , record.currencypa
                    , record.specloaded
                    , record.ttlpayinvoice
                    , record.ttlpaycurrency
                    , record.parceltype.HasValue ? CustomBrokerWpf.References.ParcelTypes.FindFirstItem("Id", record.parceltype) : null
                    , record.additionalcost
                    , record.additionalpay
                    , record.actualweight
                    , record.bringcost
                    , record.bringpay
                    , record.brokercost
                    , record.brokerpay
                    , record.currencyrate
                    , record.currencysum
                    , record.customscost
                    , record.customspay
                    , record.deliverycost
                    , record.deliverypay
                    , record.dtrate
                    , record.goodvalue
                    , record.freightcost
                    , record.freightpay
                    , record.insurancecost
                    , record.insurancepay
                    , record.invoice
                    , record.invoicediscount
                    , record.officialweight
                    , record.preparatncost
                    , record.preparatnpay
                    , record.selling
                    , record.sellingmarkup
                    , record.sellingmarkuprate
                    , record.sertificatcost
                    , record.sertificatpay
                    , record.tdcost
                    , record.tdpay
                    , record.volume
                    , record.currencydate
                    , record.currencypaiddate
                    , record.gtddate
                    , record.requestdate
                    , record.shipplandate
                    , record.specification
                    , record.storedate
                    , record.storeinform
                    , record.algorithmnote1
                    , record.algorithmnote2
                    , record.cargo
                    , record.colormark
                    , record.consolidate
                    , record.currencynote
                    , record.customernote
                    , record.docdirpath
                    , record.gtd
                    , record.fullnumber
                    , record.managergroupname
                    , record.managernote
                    , record.mskstorenote
                    , record.servicetype
                    , record.storenote
                    , record.storepoint
                    , record.importer.HasValue ? CustomBrokerWpf.References.Importers.FindFirstItem("Id", record.importer.Value) : null
                    , record.manager.HasValue ? CustomBrokerWpf.References.Managers.FindFirstItem("Id", record.manager.Value) : null
                    );
                request = CustomBrokerWpf.References.RequestStore.UpdateItem(newitem, this.FillType == lib.FillType.Refresh);
                
                if (canceltasktoken.IsCancellationRequested) return request;
                if(!request.BrandsIsNull & this.FillType == lib.FillType.Refresh)
                {
                    mybdbm.Request= request;
                    mybdbm.Agent = request.Agent;
                    mybdbm.FillType = this.FillType;
                    mybdbm.Connection = addcon;
                    mybdbm.Fill();
                }
                if (myldbm != null && (request.CustomerLegalsIsNull | this.FillType == lib.FillType.Refresh)) // Fill only first fill or willfully refresh (CustomerLegals contains new record, id<0, have Lost)
                {
                    myldbm.FillType = this.FillType;
                    myldbm.Connection = addcon;
                    request.CustomerLegalsRefresh(myldbm);
                    myldbm.Collection = null;
                }
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
                    if (!request.MailStateStockIsNull) request.MailStateStock.Update();
                    if (!request.MailStateTakeGoods9IsNull) request.MailStateTakeGoods9.Update();
                }
                if (this.FillType == lib.FillType.Refresh)
                {
                    mydispatcher.Invoke(() =>
                    {
                        request.AlgorithmCMD?.Refresh.Execute(null);
                        request.AlgorithmConCMD?.Refresh.Execute(null);
                    });
                }
                request.IsLoaded = true;
            }
            return request;
        }
        protected override void GetOutputSpecificParametersValue(Request item)
        {
            SqlParameter status = myinsertupdateparams.First((SqlParameter par) => { return par.ParameterName == "@status"; });
            if (status.Value != null)
                item.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", (int)status.Value);
            //item.ManagerGroupName = (string)(DBNull.Value == myinsertupdateparams[3].Value ? null : myinsertupdateparams[3].Value);
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
            if (!item.BrandsIsNull)
            {
                mybdbm.Errors.Clear();
                mybdbm.Request = item;
                mybdbm.Agent = item.Agent;
                mybdbm.Collection = item.Brands;
                if (!mybdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    foreach (lib.DBMError err in mybdbm.Errors) this.Errors.Add(err);
                }
            }
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
            int i = 18;
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
                    case "@actualWeighttrue":
                        par.Value = item.HasPropertyOutdatedValue("ActualWeight");
                        break;
                    case "@agentIdtrue":
                        par.Value = item.HasPropertyOutdatedValue("AgentId") || item.HasPropertyOutdatedValue("Agent");
                        break;
                    case "@algorithmnote1":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.AlgorithmNote1));
                        break;
                    case "@algorithmnote2":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.AlgorithmNote2));
                        break;
                    case "@cellNumbertrue":
                        par.Value=item.HasPropertyOutdatedValue("CellNumber");
                        break;
                    case "@consolidatetrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Consolidate));
                        break;
                    case "@countrytrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Country));
                        break;
                    case "@currencytrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Currency));
                        break;
                    case "@customerIdtrue":
                        par.Value = item.HasPropertyOutdatedValue("CustomerId") || item.HasPropertyOutdatedValue("Customer");
                        break;
                    case "@freighttrue":
                        par.Value = item.HasPropertyOutdatedValue("FreightId");
                        break;
                    case "@goodValuetrue":
                        par.Value = item.HasPropertyOutdatedValue("GoodValue");
                        break;
                    case "@importertrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Importer));
                        break;
                    case "@isspecification":
                        par.Value = item.IsSpecification;
                        break;
                    case "@loadDescriptiontrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Cargo));
                        break;
                    case "@manageridtrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.Manager));
                        break;
                    case "@managerNotetrue":
                        par.Value = item.HasPropertyOutdatedValue("ManagerNote");
                        break;
                    case "@mskstorenotetrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.MSKStoreNote));
                        break;
                    case "@officialWeighttrue":
                        par.Value = item.HasPropertyOutdatedValue("OfficialWeight");
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
                    case "@statustrue":
                        par.Value = item.HasPropertyOutdatedValue("Status");
                        break;
                    case "@storageDatetrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.StoreDate));
                        break;
                    case "@storageInformtrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.StoreInform));
                        break;
                    case "@storagePointtrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Request.StorePoint));
                        break;
                    case "@storeidtrue":
                        par.Value = item.HasPropertyOutdatedValue("StoreId");
                        break;
                    case "@storageNotetrue":
                        par.Value = item.HasPropertyOutdatedValue("StoreNote");
                        break;
                    case "@volumetrue":
                        par.Value = item.HasPropertyOutdatedValue("Volume");
                        break;
                }
            i = 19;
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
                    case "@actualWeight":
                        par.Value = item.ActualWeight;
                        break;
                    case "@agentId":
                        par.Value = item.Agent?.Id??item.AgentId;
                        break;
                    case "@algorithmnote1":
                        par.Value = item.AlgorithmNote1;
                        break;
                    case "@algorithmnote2":
                        par.Value = item.AlgorithmNote2;
                        break;
                    case "@cellNumber":
                        par.Value = item.CellNumber;
                        break;
                    case "@consolidate":
                        par.Value = item.Consolidate;
                        break;
                    case "@country":
                        par.Value = item.Country?.Code;
                        break;
                    case "@currency":
                        par.Value = item.Currency;
                        break;
                    case "@customerId":
                        par.Value = item.Customer?.Id??item.CustomerId;
                        break;
                    case "@freight":
                        par.Value = item.FreightId;
                        break;
                    case "@goodValue":
                        par.Value = item.GoodValue;
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
                    case "@managerNote":
                        par.Value = item.ManagerNote;
                        break;
                    case "@mskstorenote":
                        par.Value = item.MSKStoreNote;
                        break;
                    case "@officialWeight":
                        par.Value = item.OfficialWeight;
                        break;
                    case "@shipplandate":
                        par.Value = item.ShipPlanDate;
                        break;
                    case "@status":
                        par.Value = item.Status?.Id;
                        break;
                    case "@storageDate":
                        par.Value = item.StoreDate;
                        break;
                    case "@storageInform":
                        par.Value = item.StoreInform;
                        break;
                    case "@storageNote":
                        par.Value = item.StoreNote;
                        break;
                    case "@storagePoint":
                        par.Value = item.StorePoint;
                        break;
                    case "@storeid":
                        par.Value = item.StoreId;
                        break;
                    case "@volume":
                        par.Value = item.Volume;
                        break;
                }
            mydeleteparams[1].Value = myinsertupdateparams[0].Value;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            foreach(SqlParameter par in this.SelectParams)
                switch(par.ParameterName)
				{
                    case "@filterId":
                        par.Value = this.Filter;
                        break;
                    case "@parcel":
                        par.Value = this.Parcel?.Id;
                        break;
                    case "@datechanged":
                        par.Value = this.UpdateWhen;
                        break;
                    case "@storagepoint":
                        par.Value = this.StorePoint;
                        break;
                    case "@consolidate":
                        par.Value = this.Consolidate;
                        break;
                }
        }
        //protected override void CancelLoad()
        //{
        //    mybdbm.CancelingLoad = this.CancelingLoad;
        //    myldbm.CancelingLoad = this.CancelingLoad;
        //}
    }

    internal class RequestStore : lib.DomainStorageLoad<RequestRecord,Request, RequestDBM>
    {
        public RequestStore(RequestDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(Request olditem, Request newitem)
        {
            olditem.UpdateProperties(newitem);
        }

        public Request GetItemLoad(string storepoint, out List<DBMError> errors)
        {
            return GetItemLoad(storepoint, null, out errors);
        }
        internal Request GetItemLoad(string storepoint, SqlConnection conection, out List<lib.DBMError> errors)
		{
            errors = new List<DBMError>();
            while (myupdatingcoll > 0)
                System.Threading.Thread.Sleep(10);
            this.myforcount++;
            Request firstitem;
            try
            {
                firstitem = mycollection.Values.FirstOrDefault<Request>((Request item) => { return item.StorePoint== storepoint; });
            }
            finally { this.myforcount--; }
            if (firstitem == default(Request))
            {
                RequestDBM dbm = GetDBM();
                dbm.ItemId = null;
                dbm.StorePoint = storepoint;
                dbm.Command.Connection = conection;
                firstitem = dbm.GetFirst();
                if (firstitem != null) firstitem = UpdateItem(firstitem);
                dbm.Command.Connection = null;
                errors.AddRange(dbm.Errors);
                dbm.Errors.Clear();
                mydbmanagers.Enqueue(dbm);
            }
            return firstitem;
        }
    }

    public class RequestVM : lib.ViewModelErrorNotifyItem<Request>, lib.Interfaces.ITotalValuesItem
    {
        public RequestVM(Request item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.AgentId), nameof(this.Brands), nameof(this.Cargo), nameof(this.Customer), nameof(this.Country), nameof(this.CustomerLegals), nameof(this.Importer), nameof(this.InvoiceDiscount),nameof(this.ShipPlanDate), nameof(this.ServiceType) });
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
        public Agent Agent
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Agent, value)))
                {
                    string name = nameof(this.Agent);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Agent);
                    ChangingDomainProperty = name; this.DomainObject.Agent = value;
                    this.ValidateProperty(name, true); // for indication
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Agent : null; }
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
        public string BrandNames
        {
            get
            {
                return this.IsEnabled ? this.DomainObject.BrandNames : null;
            }
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
            get { return this.DomainObject.ColorMark??"Transparent"; }
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
        public References.Country Country
        {
            set
            {
                this.SetProperty<References.Country>(this.DomainObject.Country, (References.Country param) => { this.DomainObject.Country = param; }, value);
            }
            get { return this.IsEnabled ? this.DomainObject.Country : null; }
        }
        public int? Currency
        {
            set
            {
                if (!this.IsReadOnly && value.HasValue && this.DomainObject.Currency != value.Value)
                {
                    string name = "Currency";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Currency);
                    ChangingDomainProperty = name; this.DomainObject.Currency = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Currency : (int?)null; }
        }
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
        public string CurrencyName
        { get { return this.IsEnabled ? this.DomainObject.CurrencyName : null; } }
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
        public Customer Customer
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Customer, value)))
                {
                    string name = nameof(this.Customer);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Customer);
                    ChangingDomainProperty = name; this.DomainObject.Customer = value;
                    this.ValidateProperty(name, true); // for indication
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Customer : null; }
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
        public System.Windows.FontWeight CustomerLegalsNamesFontWeight
        { get { return this.DomainObject.CustomerLegalsNamesFontWeight; } }
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
                    if (this.ValidateProperty(name) || (!string.IsNullOrEmpty(myerrorscontainer.GetError(name,1))))
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
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Manager, value) || (this.DomainObject.Manager != null && this.DomainObject.Manager.Name != CustomBrokerWpf.References.CurrentManager.NameComb && !CustomBrokerWpf.References.CurrentUserRoles.Contains("TopManagers") && !CustomBrokerWpf.References.CurrentUserRoles.Contains("Accounts"))))
                {
                    string name = "Manager";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Manager);
                    ChangingDomainProperty = name; this.DomainObject.Manager = value.Id<0 ? null: value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Manager : null; }
        }
        public System.Windows.Visibility ManagerIsReadOnlyVisible
        { get { return this.DomainObject.Manager != null && this.DomainObject.Manager.Name != CustomBrokerWpf.References.CurrentManager.NameComb && !CustomBrokerWpf.References.CurrentUserRoles.Contains("TopManagers") & !CustomBrokerWpf.References.CurrentUserRoles.Contains("Accounts") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; } }
        public System.Windows.Visibility ManageEditableVisible
        { get { return this.DomainObject.Manager != null && this.DomainObject.Manager.Name != CustomBrokerWpf.References.CurrentManager.NameComb && !CustomBrokerWpf.References.CurrentUserRoles.Contains("TopManagers") & !CustomBrokerWpf.References.CurrentUserRoles.Contains("Accounts") ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }
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
        public string MSKStoreNote
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.MSKStoreNote, value)))
                {
                    string name = nameof(RequestVM.MSKStoreNote);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.MSKStoreNote);
                    ChangingDomainProperty = name; this.DomainObject.MSKStoreNote = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.MSKStoreNote : null; }
        }
        public string Notes
        {
            get
            {
                return this.IsEnabled ? (this.DomainObject.StoreNote ?? string.Empty) + " " + (this.DomainObject.ManagerNote ?? string.Empty)+ " " + (this.DomainObject.MSKStoreNote ?? string.Empty) + " " + (this.DomainObject.CustomerNote ?? string.Empty) : null;
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
        public int? ShipmentDelay
        { get { return this.IsEnabled ? this.DomainObject.ShipmentDelay : (int?)null; } }
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
        //private lib.ReferenceSimpleItem mystatus;
        public lib.ReferenceSimpleItem Status
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Status, value)))
                {
                    string name = nameof(this.Status);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Status);
                    //mystatus = value;
                    //if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.Status = value; }
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
        public Algorithm.AlgorithmFormulaRequestCommand AlgorithmCMD
        { get { return this.IsEnabled ? this.DomainObject.AlgorithmCMD : null; } }
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
        public string MailStateImage
        {
            get
            {
                string path;
                switch (this.DomainObject.MailStateStatus.State)
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
        public string MailStateToolTip
        {
            get
            {
                string path;
                switch (this.DomainObject.MailStateStatus.State)
                {
                    case 1:
                        path = "Не удалось отправить сообщение";
                        break;
                    case 2:
                        path = "Сообщение отправлено";
                        break;
                    default:
                        path = "Сообщение не отправляется";
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

        private RequestBrandSynchronizer mybsync;
        private ListCollectionView mybrands;
        public ListCollectionView Brands
        {
            get
            {
                if (mybrands == null)
                {
                    if (mybsync == null)
                    {
                        mybsync = new RequestBrandSynchronizer();
                        mybsync.DomainCollection = this.DomainObject.Brands;
                    }
                    mybrands = new ListCollectionView(mybsync.ViewModelCollection);
                    mybrands.SortDescriptions.Add(new System.ComponentModel.SortDescription("Selected", System.ComponentModel.ListSortDirection.Descending));
                    mybrands.SortDescriptions.Add(new System.ComponentModel.SortDescription("Brand.Name", System.ComponentModel.ListSortDirection.Ascending));
                }
                return mybrands;
            }
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
                case nameof(Request.Agent):
                    mybrands = null;
                    this.PropertyChangedNotification(nameof(this.Brands));
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
                    if (mycustomerlegalsselected != null)
                    {
                        if (mycustomerlegalsselected.IsAddingNew) mycustomerlegalsselected.CommitNew();
                        if (mycustomerlegalsselected.IsEditingItem) mycustomerlegalsselected.CommitEdit();
                        mycustomerlegalsselected.Refresh();
                        mycustomerlegalsselected.MoveCurrentToFirst();
                    }
                    break;
                case "CustomerId":
                    mycustomername = null;
                    this.PropertyChangedNotification("CustomerName");
                    mycustomerlegals = null;
                    this.PropertyChangedNotification("CustomerLegals");
                    break;
                case nameof(RequestVM.CustomerNote):
                case nameof(RequestVM.ManagerNote):
                case nameof(RequestVM.MSKStoreNote):
                case nameof(RequestVM.StoreNote):
                    this.PropertyChangedNotification(nameof(this.Notes));
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
                case "MailStateStockState": // такого свойства у Request нет
                    PropertyChangedNotification("MailStateStockImage");
                    break;
                case "MailStateTakeGoods9State": // такого свойства у Request нет
                    PropertyChangedNotification("MailStateTakeGoods9Image");
                    break;
                case nameof(Request.MailStateStatus):
                case "MailStateStatusState": // такого свойства у Request нет
                    PropertyChangedNotification(nameof(RequestVM.MailStateImage));
                    PropertyChangedNotification(nameof(RequestVM.MailStateToolTip));
                    break;
                case nameof(Request.Parcel):
                case nameof(Request.ParcelId):
                    this.PropertyChangedNotification("StatusVisible");
                    this.PropertyChangedNotification("ParcelNumber");
                    this.PropertyChangedNotification("ParcelNumberVisible");
                    this.PropertyChangedNotification("ParcelTypeEnable");
                    this.PropertyChangedNotification("StatusParcel");
                    this.PropertyChangedNotification("StatusVisible");
                    this.PropertyChangedNotification("StatusEditable");
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
                case nameof(Request.Status):
                    //mystatus = this.DomainObject.Status;
                    this.PropertyChangedNotification("StatusParcel");
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
            //mystatus = this.DomainObject.Status;
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
                case "Agent":
                    this.DomainObject.Agent = (Agent)value;
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
                case nameof(this.Country):
                    this.DomainObject.Country = (References.Country)value;
                    break;
                case "Currency":
                    this.DomainObject.Currency = (int)value;
                    break;
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
                case nameof(this.Customer):
                    this.DomainObject.Customer = (Customer)value;
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
                case nameof(RequestVM.MSKStoreNote):
                    this.DomainObject.MSKStoreNote = (string)value;
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
                    //if (mystatus != this.DomainObject.Status)
                    //    mystatus = this.DomainObject.Status;
                    //else
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
                    i = 0;
                    RequestBrandVM[] removed = new RequestBrandVM[this.Brands.Count];
                    foreach (RequestBrandVM item in this.Brands)
                    {
                        if (item.DomainState == lib.DomainObjectState.Added)
                        {
                            removed[i] = item;
                            i++;
                        }
                        else
                            item.RejectChanges();
                    }
                    foreach (RequestBrandVM item in removed)
                        if (item != null) this.Brands.Remove(item);
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            byte errcode = 0;
            switch (propertyname)
            {
                case nameof(this.AgentId):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.AgentId, out errmsg, out errcode);
                    break;
                case nameof(this.Brands):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, null, out errmsg, out errcode);
                    break;
                case nameof(this.Cargo):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Cargo, out errmsg, out errcode);
                    break;
                case nameof(this.Country):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Country, out errmsg, out errcode);
                    break;
                case nameof(this.Customer):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Customer, out errmsg, out errcode);
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
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Importer,out errmsg, out errcode);
                    break;
                case nameof(this.InvoiceDiscount):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myinvoicediscount, out errmsg, out errcode);
                    if(isvalid && myinvoicediscount != this.DomainObject.InvoiceDiscount)
					{
                        ChangingDomainProperty = nameof(this.DomainObject.InvoiceDiscount); this.DomainObject.UpdateInvoiceDiscount(myinvoicediscount, 0);
                    }
                    break;
                case nameof(this.ShipPlanDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.ShipPlanDate, out errmsg, out errcode);
                    break;
                case nameof(this.ServiceType):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.ServiceType, out errmsg, out errcode);
                    break;
                //case nameof(this.Status):
                //    isvalid = this.DomainObject.ValidateProperty(propertyname, mystatus, out errmsg, out errcode);
                //    break;
            }
            if (isvalid)
                ClearErrorMessageForProperty(propertyname);
            else if (inform) AddErrorMessageForProperty(propertyname, errmsg, errcode);
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
                        //if (!string.IsNullOrEmpty(this.DomainObject.DocDirPath) & System.IO.Directory.Exists(pathfalse))
                        //{
                        //    if (System.IO.Directory.Exists(path))
                        //    {
                        //        foreach (string movepath in System.IO.Directory.EnumerateDirectories(pathfalse))
                        //        {
                        //            System.IO.DirectoryInfo movethis = new System.IO.DirectoryInfo(movepath);
                        //            movethis.MoveTo(path + "\\" + movethis.Name);
                        //        }
                        //        foreach (string movepath in System.IO.Directory.EnumerateFiles(pathfalse))
                        //        {
                        //            System.IO.FileInfo movethis = new System.IO.FileInfo(movepath);
                        //            movethis.MoveTo(path + "\\" + movethis.Name);
                        //        }
                        //        System.IO.Directory.Delete(pathfalse);
                        //    }
                        //    else
                        //        System.IO.Directory.Move(pathfalse, path);
                        //}
                        this.DomainObject.DocDirPath = docdirpath;
                    }
                    //if (!System.IO.Directory.Exists(path))
                    //    System.IO.Directory.CreateDirectory(path);
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

    public class RequestVMCommand : lib.ViewModelCommand<RequestRecord,Request, RequestVM, RequestDBM>
    {
        public RequestVMCommand(RequestVM vm, ListCollectionView view) : base(vm, view)
        {
            mydbm = new Domain.RequestDBM();
            mydbm.ItemId = vm.Id;
            mydbm.LegalDBM = new RequestCustomerLegalDBM();
            myvm.IsReadOnly = myvm.DomainState == lib.DomainObjectState.Unchanged;
            myvm.DomainObject.AlgorithmCMD.IsReadOnly = myvm.IsReadOnly;
            myvm.DomainObject.AlgorithmConCMD.IsReadOnly = myvm.IsReadOnly;
            mydetailsadd = new RelayCommand(DetailsAddExec, DetailsAddCanExec);
            myprepaydel = new RelayCommand(PrepayDelExec, PrepayDelCanExec);
            myrefreshsave = new RelayCommand(RefreshSaveExec, RefreshSaveCanExec);
            mysendemail = new RelayCommand(SendEmailExec, SendEmailCanExec);
            myratedbm = new SpecificationCustomerInvoiceRateDBM();
            if (CustomBrokerWpf.References.CurrentUserRoles.Contains("TopManagers"))
                mymanagers = new ListCollectionView(CustomBrokerWpf.References.Managers);
            else if (CustomBrokerWpf.References.CurrentManager != null)
            {
                mymanagers = new ListCollectionView(new List<Manager>() { new Manager(), CustomBrokerWpf.References.CurrentManager });
                mymanagers.Filter = (object item) => { return (item as Manager).Unfile == 0; };
            }
            mycountries = new ListCollectionView(CustomBrokerWpf.References.Countries);
            mycountries.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as References.Country).RequestList; };
            mycountries.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            CustomBrokerWpf.References.CountryViewCollector.AddView(mycountries);
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

        private ListCollectionView myagents;
        public ListCollectionView Agents
        {
            get
            {
                if (myagents == null)
                {
                    myagents = new ListCollectionView(CustomBrokerWpf.References.AgentNames);
                    myagents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    CustomBrokerWpf.References.AgentNames.RefreshViewAdd(myagents);
                }
                return myagents;
            }
        }
        private ListCollectionView mycountries;
        public ListCollectionView Countries
        {
            get { return mycountries; }
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
        private ListCollectionView mymanagers;
        public ListCollectionView Managers
        { get { return mymanagers; } }
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
                { view = item.Prepays; legal = item; break; }
            if (view != null & (!prepay.Prepay.InvoiceDate.HasValue || (prepay.Prepay.IsPrepay??false)))
            {
                if (view.IsAddingNew)
                    view.CancelNew();
                else
                {
                    if (prepay.DomainState == lib.DomainObjectState.Added)
                    {
                        if (prepay.Prepay.IsPrepay ?? false)
                        {
                            prepay.EuroSum = 0M;
                            prepay.DTSum = 0M;
                        }
                        else
                        {
                            view.Remove(prepay);
                        }
                    }
                    else
                    {
                        //if (prepay.Prepay.IsPrepay ?? false)
                        //{
                        //    prepay.DTSum = null;
                        //    prepay.EuroSum = 0M;
                        //}
                        legal.DomainObject.UpdateInvoiceDiscount((legal.InvoiceDiscount ?? 0M) - prepay.DTSum, 'p');
                        view.EditItem(prepay);
                        prepay.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
                        view.CommitEdit();
                    }
                }
                legal.DomainObject.PropertyChangedNotification(nameof(legal.PrepaySum));
                legal.DomainObject.PropertyChangedNotification(nameof(legal.InvoiceDiscount));
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

        private RelayCommand myrefreshsave;
        public ICommand RefreshSave
        {
            get { return myrefreshsave; }
        }
        private void RefreshSaveExec(object parametr)
        {
            if (myvm.IsReadOnly)
                this.Refresh.Execute(parametr);
            else
                this.SaveRefresh.Execute(parametr);
        }
        internal bool RefreshSaveCanExec(object parametr)
        { return myvm.IsReadOnly ? this.Refresh.CanExecute(parametr) : this.SaveRefresh.CanExecute(parametr); }

        private RelayCommand mysendemail;
        public ICommand SendEmail
        {
            get { return mysendemail; }
        }
        private void SendEmailExec(object parametr)
        {
            Mail mail = new Mail();
            StringBuilder body = new StringBuilder(
                "Менеджер - " + CustomBrokerWpf.References.CurrentUser
                + "\nСтатус - " + (this.VModel.Status.Id > 0 ? "Заявка" : "Предоплата")
                + "\nИмпортер - " + (this.VModel.Importer?.Name ?? string.Empty)
                + "\nПоставщик - " + ((this.VModel.Agent?.FullName ?? this.VModel.Agent?.Name) ?? string.Empty)
                + "\nКлиент - " + ((this.VModel.CustomerLegalsNames) ?? string.Empty)
                + "\nСумма - " + (this.VModel.InvoiceDiscount.HasValue ? this.VModel.InvoiceDiscount.Value.ToString("N2") : string.Empty));
            mail.Send("Гузель Закирова", "zakirova.guzal@art-delivery.ru", CustomBrokerWpf.References.CurrentUser + ". " + ((this.VModel.CustomerLegalsNames) ?? string.Empty), body.ToString(), BodySubtype.plain);
            this.OpenPopup("Письмо отправлено",false);
        }
        internal bool SendEmailCanExec(object parametr)
        { return true; }

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
            //RequestCustomerLegalDBM ldbm = App.Current.Dispatcher.Invoke<RequestCustomerLegalDBM>(() => { return new RequestCustomerLegalDBM(); });
            //ldbm.FillType = lib.FillType.Refresh;
            //myvm.DomainObject.CustomerLegalsRefresh(ldbm);
            //this.AlgorithmCommand.Refresh.Execute(parametr);
            //this.AlgorithmConCommand.Refresh.Execute(parametr);
            //if (!myvm.DomainObject.SpecificationIsNull) // coefficients for DTSum
            //{
            //    myvm.DomainObject.Specification.InvoiceDTRates.Clear();
            //    myratedbm.Specification = myvm.DomainObject.Specification;
            //    myratedbm.Load();
            //    if (myratedbm.Errors.Count > 0) foreach (lib.DBMError err in myratedbm.Errors) errstr.AppendLine(err.Message);
            //}
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
            //try
            //{
            //    if (myvm.CustomerId.HasValue & ((!string.IsNullOrEmpty(myvm.StorePoint) & myvm.StoreDate.HasValue) | myvm.ParcelGroup.HasValue))
            //    {
            //        string path, pathfalse, docdirpath;
            //        if (myvm.DomainObject.ParcelId.HasValue)
            //            docdirpath = "Отправки\\" + myvm.Parcel.DocDirPath + "\\" + myvm.CustomerName + "_" + (myvm.ParcelGroup.HasValue ? myvm.ParcelGroup.ToString() : myvm.StorePointDate);
            //        else
            //            docdirpath = "Прямые\\" + myvm.CustomerName + "_" + (myvm.ParcelGroup.HasValue ? myvm.ParcelGroup.ToString() : myvm.StorePointDate);
            //        path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + docdirpath;
            //        if (!string.Equals(docdirpath, myvm.DomainObject.DocDirPath))
            //        {
            //            pathfalse = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + myvm.DomainObject.DocDirPath;
            //            if (!string.IsNullOrEmpty(myvm.DomainObject.DocDirPath) & System.IO.Directory.Exists(pathfalse))
            //            {
            //                if (System.IO.Directory.Exists(path))
            //                {
            //                    foreach (string movepath in System.IO.Directory.EnumerateDirectories(pathfalse))
            //                    {
            //                        System.IO.DirectoryInfo moveitem = new System.IO.DirectoryInfo(movepath);
            //                        moveitem.MoveTo(path + "\\" + moveitem.Name);
            //                    }
            //                    foreach (string movepath in System.IO.Directory.EnumerateFiles(pathfalse))
            //                    {
            //                        System.IO.FileInfo moveitem = new System.IO.FileInfo(movepath);
            //                        moveitem.MoveTo(path + "\\" + moveitem.Name);
            //                    }
            //                    System.IO.Directory.Delete(pathfalse);
            //                }
            //                else
            //                    System.IO.Directory.Move(pathfalse, path);
            //            }
            //            myvm.DomainObject.DocDirPath = docdirpath;
            //        }
            //        if (!System.IO.Directory.Exists(path))
            //            System.IO.Directory.CreateDirectory(path);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.PopupText = "Не удалось сохранить папку заявки" + myvm.StorePointDate + "!\nЗакройте все документы из этой папки и повторите сохранение.\n\n" + ex.Message;
            //    //lib.ExceptionHandler handler = new lib.ExceptionHandler("Сохранение изменений");
            //    //handler.Handle(new Exception("Не удалось сохранить папку заявки " + myvm.StorePointDate + "!\nЗакройте все документы из этой папки и повторите сохранение.\n\n" + ex.Message));
            //    //handler.ShowMessage();
            //}
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
        //internal static string MoveFolder(RequestVM item)
        //{
        //    return item.MoveFolder();
        //}
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

    public class RequestViewCommand : lib.ViewModelViewCommand, lib.Interfaces.IFilterWindowOwner
    {
        internal RequestViewCommand()
        {
            myfilter = new lib.SQLFilter.SQLFilter("Request", "AND", CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            mydbm = new RequestDBM();
            mydbm.LegalDBM = new RequestCustomerLegalDBM() { LegalDBM = new CustomerLegalDBM() };
            mydbm.Collection = new ObservableCollection<Request>();
            mydbm.Filter = myfilter.FilterWhereId;
            mydbm.FillAsyncCompleted = () => { 
                if (mydbm.Errors.Count > 0) 
                    OpenPopup(mydbm.ErrorMessage, true);
                else
                    mydbm.FillAsyncCompleted = () => {
                        if (mydbm.Errors.Count > 0)
                            OpenPopup(mydbm.ErrorMessage, true);
                        //else
                        //{
                        //    RequestCustomerLegalDBM ldbm = App.Current.Dispatcher.Invoke<RequestCustomerLegalDBM>(() => { return new RequestCustomerLegalDBM(); });
                        //    ldbm.FillType = lib.FillType.Refresh;
                        //    foreach (Request ritem in mydbm.Collection)
                        //    { ritem.CustomerLegalsRefresh(ldbm); ldbm.Collection = null; }
                        //}
                    };
            };
            mydbm.FillAsync();
            mysync = new RequestSynchronizer();
            mysync.DomainCollection = mydbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            myfoldermove = new RelayCommand(FoldersMoveExec, FoldersMoveCanExec);
            if (CustomBrokerWpf.References.CurrentUserRoles.Contains("TopManagers"))
                mymanagers = new ListCollectionView(CustomBrokerWpf.References.Managers);
            else
            {
                mymanagers = new ListCollectionView(new List<Manager>() { new Manager(), CustomBrokerWpf.References.CurrentManager });
                mymanagers.Filter = (object item) => { return (item as Manager)?.Unfile == 0; };
            }

            myfastfilter = new lib.SQLFilter.SQLFilter("Request", "AND",CustomBrokerWpf.References.ConnectionString);
            myfastfilter.RemoveCurrentWhere();
            mynumbergroup = myfastfilter.GroupAdd(myfastfilter.FilterWhereId, "numbergroup", "OR");
            myrequeststoragepointfilter = string.Empty;
            myrunfastfilter = new RelayCommand(RunFastFilterExec, RunFastFilterCanExec);
        }

        private lib.SQLFilter.SQLFilter myfilter;
        public lib.SQLFilter.SQLFilter Filter
        { get { return myfilter; } }
        private lib.SQLFilter.SQLFilter myfastfilter;
        private int mynumbergroup;
        private int? myrequestclientfilter;
        public int? RequestClientFilter
        {
            set
            {
                myrequestclientfilter = value;
                PropertyChangedNotification("RequestClientFilter");
            }
            get { return myrequestclientfilter; }
        }
        private string myrequeststoragepointfilter;
        public string RequestStoragePointFilter
        {
            set
            {
                myrequeststoragepointfilter = value;
                PropertyChangedNotification("RequestStoragePointFilter");
            }
            get { return myrequeststoragepointfilter; }
        }


        private new RequestDBM mydbm;
        private RequestSynchronizer mysync;
        private System.Threading.Tasks.Task myrefreshtask;

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
        
        private ListCollectionView myagents;
        public ListCollectionView Agents
        {
            get
            {
                if (myagents == null)
                {
                    myagents = new ListCollectionView(CustomBrokerWpf.References.AgentNames);
                    CustomBrokerWpf.References.AgentNames.RefreshViewAdd(myagents);
                    myagents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myagents;
            }
        }
        private ListCollectionView mycustomers;
        public ListCollectionView Customers
        {
            get
            {
                if (mycustomers == null)
                {
                    mycustomers = new ListCollectionView(CustomBrokerWpf.References.CustomersName);
                    CustomBrokerWpf.References.CustomersName.RefreshViewAdd(mycustomers);
                    mycustomers.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return mycustomers;
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
        private ListCollectionView mymanagers;
        public ListCollectionView Managers
        { get { return mymanagers; } }
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

        private RelayCommand myrunfastfilter;
        public ICommand RunFastFilter
        {
            get { return myrunfastfilter; }
        }
        private void RunFastFilterExec(object parametr)
        {
            myfastfilter.SetNumber(myfastfilter.FilterWhereId, "customerId", 0, (RequestClientFilter?.ToString() ?? string.Empty));
            myfastfilter.SetNumber(mynumbergroup, "storagePoint", 0, this.RequestStoragePointFilter);
            myfastfilter.SetNumber(mynumbergroup, "requestID", 0, this.RequestStoragePointFilter);
            if (!this.SaveDataChanges())
                this.OpenPopup("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", true);
            else
            {
                this.Refresh.Execute(null);
            }
        }
        private bool RunFastFilterCanExec(object parametr)
        { return true; }

        private bool myisshowfilterwindow;
        public bool IsShowFilterWindow
        {
            set
            {
                myisshowfilterwindow = value;
                this.PropertyChangedNotification(nameof(this.IsShowFilterWindow));
            }
            get { return myisshowfilterwindow; }
        }
        public void RunFilter(FilterItem[] filters)
        {
            RunFastFilterExec(null);
        }

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
                        //else
                        //    err.AppendLine(item.DomainObject.UpdateDocDirPath());
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
            return !(myview.IsAddingNew | myview.IsEditingItem) & (myrefreshtask == null || myrefreshtask.IsCompleted);
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
            CustomBrokerWpf.References.ParcelLastShipdate.Update();
            mydbm.FillType = lib.FillType.Refresh;
            if(myfastfilter.isEmpty)
                mydbm.Filter = myfilter.FilterWhereId;
            else
                mydbm.Filter = myfastfilter.FilterWhereId;
            myrefreshtask =mydbm.FillAsync();
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
