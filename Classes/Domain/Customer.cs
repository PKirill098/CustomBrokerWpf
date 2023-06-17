using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using excel = Microsoft.Office.Interop.Excel;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Customer : lib.DomainBaseStamp, lib.Interfaces.INameId
    {
        public Customer(int id, long stamp, string updater, DateTime? updated, lib.DomainObjectState dstate
            , int? account, string bankaccount, string bankbic, string bankname, DateTime? contractdate, string contractnum, string corraccount, DateTime dayentry, int? deliverytype, string fullname, string inn, lib.ReferenceSimpleItem managergroup, string name, string notespecial, int? payaccount, int? paytypeid, string recommend, int state, string status
            ,int? parcelcount,DateTime? parcellastdate
            ) : base(id, stamp, updated, updater, dstate)
        {
            myaccount = account;
            mybankaccount = bankaccount;
            mybankbic = bankbic;
            mybankname = bankname;
            mycontractdate = contractdate;
            mycontractnum = contractnum;
            mycorraccount = corraccount;
            mydayentry = dayentry;
            mydeliverytype = deliverytype;
            mydeliverytype_ = deliverytype.HasValue ? CustomBrokerWpf.References.DeliveryTypes.FindFirstItem("Id", deliverytype.Value) : null;
            myfullname = fullname;
            myinn = inn;
            mymanagergroup = managergroup;
            myname = name;
            mynotespecial = notespecial;
            myparcelcount = parcelcount;
            myparcellastdate = parcellastdate;
            mypayaccount = payaccount;
            mypaytypeid = paytypeid;
            myrecommend = recommend;
            mystate = state;
            mystatus = status;

            myparcellock = new object();
        }
        public Customer(int id, long stamp, string updater, DateTime? updated, lib.DomainObjectState dstate
            , int? account, string bankaccount, string bankbic, string bankname, DateTime? contractdate, string contractnum, string corraccount, DateTime dayentry, int? deliverytype, string fullname, string inn, lib.ReferenceSimpleItem managergroup, string name, string notespecial, int? payaccount, int? paytypeid, string recommend, int state, string status
            ) : this(id,stamp,updater,updated,dstate
            ,account, bankaccount, bankbic, bankname, contractdate,contractnum,corraccount,dayentry,deliverytype,fullname,inn,managergroup,name,notespecial,payaccount,paytypeid,recommend,state,status
            , null, null) {}
        public Customer(string fullname, string name) : this(id: lib.NewObjectId.NewId, stamp: 0, updater: null, updated: null, dstate: lib.DomainObjectState.Added
            , account: null, bankaccount: null, bankbic: null, bankname: null, contractdate: null, contractnum: null, corraccount: null, dayentry: DateTime.Now, deliverytype: null, fullname: fullname, inn: null, managergroup: null, name: name, notespecial: null, payaccount: null, paytypeid: null, recommend: null, state: 0, status: "Заявка"
            )
        { }
        public Customer() : this(fullname: null, name: null) { }

        private int? myaccount;
        public int? Account
        {
            set
            {
                base.SetProperty<int?>(ref myaccount, value);
            }
            get { return myaccount; }
        }
        private string mybankaccount;
        public string BankAccount
        {
            set
            {
                SetProperty<string>(ref mybankaccount, value);
            }
            get { return mybankaccount; }
        }
        private string mybankbic;
        public string BankBIC
        {
            set
            {
                SetProperty<string>(ref mybankbic, value);
            }
            get { return mybankbic; }
        }
        private string mybankname;
        public string BankName
        {
            set
            {
                SetProperty<string>(ref mybankname, value);
            }
            get { return mybankname; }
        }
        private DateTime? mycontractdate;
        public DateTime? ContractDate
        {
            set { SetProperty<DateTime?>(ref mycontractdate, value); }
            get { return mycontractdate; }
        }
        private string mycontractnum;
        public string ContractNumber
        {
            set
            {
                SetProperty<string>(ref mycontractnum, value);
            }
            get { return mycontractnum; }
        }
        private string mycorraccount;
        public string CorrAccount
        {
            set
            {
                SetProperty<string>(ref mycorraccount, value);
            }
            get { return mycorraccount; }
        }
        private DateTime mydayentry;
        public DateTime DayEntry
        {
            set { SetProperty<DateTime>(ref mydayentry, value); }
            get { return mydayentry; }
        }
        private int? mydeliverytype;
        public int? DeliveryType
        {
            set
            {
                base.SetProperty<int?>(ref mydeliverytype, value,()=> {
                    
                    mydeliverytype_ = value.HasValue ? CustomBrokerWpf.References.DeliveryTypes.FindFirstItem("Id",mydeliverytype.Value) : null;
                    this.PropertyChangedNotification(nameof(this.DeliveryType_));
                });
            }
            get { return mydeliverytype; }
        }
        private lib.ReferenceSimpleItem mydeliverytype_;
        public lib.ReferenceSimpleItem DeliveryType_
        { get { return mydeliverytype_; } }
        private string myfullname;
        public string FullName
        {
            set
            {
                SetProperty<string>(ref myfullname, value);
            }
            get { return myfullname; }
        }
        private string myinn;
        public string INN
        {
            set
            {
                SetProperty<string>(ref myinn, value);
            }
            get { return myinn; }
        }
        private lib.ReferenceSimpleItem mymanagergroup;
        public lib.ReferenceSimpleItem ManagerGroup
        {
            set
            {
                base.SetProperty<lib.ReferenceSimpleItem>(ref mymanagergroup, value);
            }
            get { return mymanagergroup; }
        }
        private string myname;
        public string Name
        {
            set
            {
                SetProperty<string>(ref myname, value);
            }
            get { return myname; }
        }
        public System.Windows.FontWeight NameFontWeight
        { get { return this.isNoteSpecial ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal; } }
        private string mynotespecial;
        public string NoteSpecial
        {
            set { SetProperty<string>(ref mynotespecial, value); }
            get { return mynotespecial; }
        }
        public bool isNoteSpecial
        { get { return !string.IsNullOrWhiteSpace(mynotespecial); } }
        private int? myparcelcount;
        private object myparcellock;
        private int myparcelinitprocess = -1;
        public int? ParcelCount
        { set { SetProperty<int?>(ref myparcelcount, value); }
            get {
                lock(myparcellock)
                    if(myparcelinitprocess < 0)
                    {
                        myparcelinitprocess= System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId;
                    }
                if (myparcelinitprocess > 0 && myparcelinitprocess == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
                {

                    myparcelinitprocess = 0;
                }
                return myparcelcount; } }
        private DateTime? myparcellastdate;
        public DateTime? ParcelLastDate
        {
            set { SetProperty<DateTime?>(ref myparcellastdate, value); }
            get
            {
                lock (myparcellock)
                    if (myparcelinitprocess < 0)
                    {
                        myparcelinitprocess = System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId;
                    }
                if (myparcelinitprocess > 0 && myparcelinitprocess == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
                {

                    myparcelinitprocess = 0;
                }
                return myparcellastdate;
            }
        }
        private int? mypayaccount;
        public int? PayAccount
        {
            set
            {
                base.SetProperty<int?>(ref mypayaccount, value);
            }
            get { return mypayaccount; }
        }
        private int? mypaytypeid;
        public int? PayType
        {
            set
            {
                base.SetProperty<int?>(ref mypaytypeid, value);
            }
            get { return mypaytypeid; }
        }
        private string myrecommend;
        public string Recommend
        {
            set { SetProperty<string>(ref myrecommend, value); }
            get { return myrecommend; }
        }
        private string mystatus;
        public string Status
        {
            set { SetProperty<string>(ref mystatus, value); }
            get { return mystatus; }
        }
        private int mystate;
        public int State
        {
            set
            {
                SetProperty<int>(ref mystate, value);
            }
            get { return mystate; }
        }

        private System.Collections.ObjectModel.ObservableCollection<CustomerLegal> mylegals;
        internal System.Collections.ObjectModel.ObservableCollection<CustomerLegal> Legals
        {
            get
            {
                if (mylegals == null)
                {
                    mylegals = new System.Collections.ObjectModel.ObservableCollection<CustomerLegal>();
                    CustomerLegalDBM ldbm = new CustomerLegalDBM();
                    ldbm.CustomerId = this.Id;
                    ldbm.Collection = mylegals;
                    ldbm.Fill();
                    mylegals.CollectionChanged += Legals_CollectionChanged;
                }
                return mylegals;
            }
        }
        private void Legals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (CustomerLegal item in e.NewItems)
                    item.Customer = this;
        }
        internal bool LegalsIsNull
        { get { return mylegals == null; } }
        private System.Collections.ObjectModel.ObservableCollection<Alias> myaliases;
        internal System.Collections.ObjectModel.ObservableCollection<Alias> Aliases
        {
            get
            {
                if (myaliases == null)
                {
                    myaliases = new System.Collections.ObjectModel.ObservableCollection<Alias>();
                    AliasDBM ldbm = new AliasDBM();
                    ldbm.ItemId = this.Id;
                    ldbm.Collection = myaliases;
                    ldbm.Fill();
                    myaliases.CollectionChanged += Aliases_CollectionChanged;
                }
                return myaliases;
            }
        }
        private void Aliases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (Alias item in e.NewItems)
                    item.CustomerId = this.Id;
        }
        internal bool AliasesIsNull
        { get { return myaliases == null; } }
        private System.Collections.ObjectModel.ObservableCollection<CustomerAddress> myaddresses;
        internal System.Collections.ObjectModel.ObservableCollection<CustomerAddress> Addresses
        {
            get
            {
                if (myaddresses == null)
                {
                    myaddresses = new System.Collections.ObjectModel.ObservableCollection<CustomerAddress>();
                    CustomerAddressDBM ldbm = new CustomerAddressDBM();
                    ldbm.ItemId = this.Id;
                    ldbm.Collection = myaddresses;
                    ldbm.Fill();
                    myaddresses.CollectionChanged += CustomerAddresses_CollectionChanged;
                }
                return myaddresses;
            }
        }
        private void CustomerAddresses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (CustomerAddress item in e.NewItems)
                    item.CustomerId = this.Id;
        }
        internal bool CustomerAddressesIsNull
        { get { return myaddresses == null; } }
        private System.Collections.ObjectModel.ObservableCollection<CustomerContact> mycontacts;
        internal System.Collections.ObjectModel.ObservableCollection<CustomerContact> Contacts
        {
            get
            {
                if (mycontacts == null)
                {
                    mycontacts = new System.Collections.ObjectModel.ObservableCollection<CustomerContact>();
                    CustomerContactDBM ldbm = new CustomerContactDBM();
                    ldbm.ItemId = this.Id;
                    ldbm.Collection = mycontacts;
                    ldbm.Fill();
                    mycontacts.CollectionChanged += CustomerContact_CollectionChanged;
                }
                return mycontacts;
            }
        }
        private void CustomerContact_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (CustomerContact item in e.NewItems)
                    item.CustomerId = this.Id;
        }
        internal bool CustomerContactsIsNull
        { get { return mycontacts == null; } }
        private System.Collections.ObjectModel.ObservableCollection<Recipient> myrecipients;
        internal System.Collections.ObjectModel.ObservableCollection<Recipient> Recipients
        {
            get
            {
                if (myrecipients == null)
                {
                    myrecipients = new System.Collections.ObjectModel.ObservableCollection<Recipient>();
                    RecipientDBM ldbm = new RecipientDBM();
                    ldbm.CustomerId = this.Id;
                    ldbm.Collection = myrecipients;
                    ldbm.Fill();
                    myrecipients.CollectionChanged += Recipients_CollectionChanged;
                }
                return myrecipients;
            }
        }
        private void Recipients_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (Recipient item in e.NewItems)
                    item.Customer = this;
        }
        internal bool RecipientsIsNull
        { get { return myrecipients == null; } }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Customer newitem = (Customer)sample;
            this.Account = newitem.Account;
            this.BankAccount = newitem.BankAccount;
            this.BankBIC = newitem.BankBIC;
            this.BankName = newitem.BankName;
            this.ContractDate = newitem.ContractDate;
            this.ContractNumber = newitem.ContractNumber;
            this.CorrAccount = newitem.CorrAccount;
            this.DayEntry = newitem.DayEntry;
            this.DeliveryType = newitem.DeliveryType;
            this.FullName = newitem.FullName;
            this.INN = newitem.INN;
            this.ManagerGroup = newitem.ManagerGroup;
            this.Name = newitem.Name;
            this.NoteSpecial = newitem.NoteSpecial;
            if(newitem.ParcelCount!=null) this.ParcelCount = newitem.ParcelCount;
            if (newitem.ParcelLastDate != null) this.ParcelLastDate = newitem.ParcelLastDate;
            this.PayAccount = newitem.PayAccount;
            this.PayType = newitem.PayType;
            this.Recommend = newitem.Recommend;
            this.Status = newitem.Status;
            this.State = newitem.State;
        }
    }

    public class CustomerDBM : lib.DBManagerWhoWhen<Customer>
    {
        public CustomerDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.Customer_sp";
            InsertCommandText = "dbo.CustomerAdd_sp";
            UpdateCommandText = "dbo.CustomerUpd_sp";
            DeleteCommandText = "dbo.CustomerDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int),
                new SqlParameter("@param2", System.Data.SqlDbType.Int),
                new SqlParameter("@param3", System.Data.SqlDbType.NVarChar,100),
                new SqlParameter("@param4", System.Data.SqlDbType.Bit),
                new SqlParameter("@param5", System.Data.SqlDbType.Int)
            };
            myinsertparams = new SqlParameter[]
           {
                myinsertparams[0]
                ,new SqlParameter("@parentid", 0)
           };
            myinsertparams[0].ParameterName = "@customerID";
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@nametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@fullnametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@dayentrytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@recommendtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@notespecialtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@statustrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@paytypeidtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@payaccountidtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@deliverytypeidtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@managergroupidtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@inntrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@raccounttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@coraccounttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@bankbictrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@banknametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@contractnumtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@contractdatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@statetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@old", 0)
            };
            myupdateparams[0].ParameterName = "@customerID";
            myinsertupdateparams = new SqlParameter[]
           {
                myinsertupdateparams[0],myinsertupdateparams[1],myinsertupdateparams[2]
                ,new SqlParameter("@customerName", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@customerFullName", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@customerDayEntry", System.Data.SqlDbType.DateTime)
                ,new SqlParameter("@customerRecommend", System.Data.SqlDbType.NVarChar,50)
                ,new SqlParameter("@customerNoteSpecial", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@customerStatus", System.Data.SqlDbType.NChar,6)
                ,new SqlParameter("@paytypeID", System.Data.SqlDbType.Int)
                ,new SqlParameter("@payaccountid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@deliverytypeID", System.Data.SqlDbType.Int)
                ,new SqlParameter("@managerGroupID", System.Data.SqlDbType.Int)
                ,new SqlParameter("@inn", System.Data.SqlDbType.NChar,12)
                ,new SqlParameter("@raccount", System.Data.SqlDbType.Char,20)
                ,new SqlParameter("@coraccount", System.Data.SqlDbType.Char,20)
                ,new SqlParameter("@bankbic", System.Data.SqlDbType.Char,9)
                ,new SqlParameter("@bankname", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@contractnum", System.Data.SqlDbType.NVarChar,20)
                ,new SqlParameter("@contractdate", System.Data.SqlDbType.Date)
                ,new SqlParameter("@customerState", System.Data.SqlDbType.TinyInt)
           };
            myinsertupdateparams[1].ParameterName = "@updtDate";
            myinsertupdateparams[2].ParameterName = "@updtWho";
            myldbm = new CustomerLegalDBM(); myldbm.Command = new SqlCommand();
            myadbm = new AliasDBM(); myadbm.Command = new SqlCommand();
            mycdbm = new CustomerAddressDBM(); mycdbm.Command = new SqlCommand();
            myccdbm = new CustomerContactDBM(); myccdbm.Command = new SqlCommand();
            myrdbm = new RecipientDBM(); myrdbm.Command = new SqlCommand();
        }

        private CustomerLegalDBM myldbm;
        private AliasDBM myadbm;
        private CustomerAddressDBM mycdbm;
        private CustomerContactDBM myccdbm;
        private RecipientDBM myrdbm;
        private string myname;
        internal string Name
        { set { myname = value; } get { return myname; } }
        private bool myall;
        internal bool All
        { set { myall = value; } get { return myall; } }
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { set { myfilter = value; } get { return myfilter; } }

        protected override Customer CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            Customer newitem = new Customer(id: reader.GetInt32(0), stamp: reader.GetInt32(this.Fields["stamp"]), updater: reader.IsDBNull(this.Fields["updtWho"]) ? null : reader.GetString(this.Fields["updtWho"]), updated: reader.IsDBNull(this.Fields["updtDate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["updtDate"]), dstate: lib.DomainObjectState.Unchanged
                , account: null
                , bankaccount: reader.IsDBNull(this.Fields["raccount"]) ? null : reader.GetString(this.Fields["raccount"])
                , bankbic: reader.IsDBNull(this.Fields["bankbic"]) ? null : reader.GetString(this.Fields["bankbic"])
                , bankname: reader.IsDBNull(this.Fields["bankname"]) ? null : reader.GetString(this.Fields["bankname"])
                , contractdate: reader.IsDBNull(this.Fields["contractdate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["contractdate"])
                , contractnum: reader.IsDBNull(this.Fields["contractnum"]) ? null : reader.GetString(this.Fields["contractnum"])
                , corraccount: reader.IsDBNull(this.Fields["coraccount"]) ? null : reader.GetString(this.Fields["coraccount"])
                , dayentry: reader.GetDateTime(this.Fields["customerDayEntry"])
                , deliverytype: reader.IsDBNull(this.Fields["deliverytypeID"]) ? (int?)null : reader.GetInt32(this.Fields["deliverytypeID"])
                , fullname: reader.IsDBNull(this.Fields["customerFullName"]) ? null : reader.GetString(this.Fields["customerFullName"])
                , inn: reader.IsDBNull(this.Fields["inn"]) ? null : reader.GetString(this.Fields["inn"])
                , managergroup: reader.IsDBNull(this.Fields["managerGroupID"]) ? null : CustomBrokerWpf.References.ManagerGroups.FindFirstItem("Id", reader.GetInt32(this.Fields["managerGroupID"]))
                , name: reader.IsDBNull(this.Fields["customerName"]) ? null : reader.GetString(this.Fields["customerName"])
                , notespecial: reader.IsDBNull(this.Fields["customerNoteSpecial"]) ? null : reader.GetString(this.Fields["customerNoteSpecial"])
                , payaccount: reader.IsDBNull(this.Fields["payaccount"]) ? (int?)null : reader.GetInt32(this.Fields["payaccount"])
                , paytypeid: reader.IsDBNull(this.Fields["paytypeID"]) ? (int?)null : reader.GetInt32(this.Fields["paytypeID"])
                , recommend: reader.IsDBNull(this.Fields["customerRecommend"]) ? null : reader.GetString(this.Fields["customerRecommend"])
                , state: reader.GetByte(this.Fields["customerState"])
                , status: reader.IsDBNull(this.Fields["customerStatus"]) ? null : reader.GetString(this.Fields["customerStatus"])
                , parcelcount: this.Fields.ContainsKey("parcelcount") ? reader.GetInt32(this.Fields["parcelcount"]) : (int?)null
                , parcellastdate: this.Fields.ContainsKey("parcellastdate") && !reader.IsDBNull(this.Fields["parcellastdate"]) ? reader.GetDateTime(this.Fields["parcellastdate"]) : (DateTime?)null
                );
            Customer olditem = CustomBrokerWpf.References.CustomerStore.UpdateItem(newitem);
            if (this.FillType==lib.FillType.Refresh | newitem.ParcelCount != null)
            {
                olditem.ParcelCount = newitem.ParcelCount;
                olditem.ParcelLastDate = newitem.ParcelLastDate;
            }
            return olditem;
        }
        protected override void GetOutputSpecificParametersValue(Customer item) { }
        protected override bool SaveChildObjects(Customer item)
        {
            bool issuccess = true;
            if (!item.LegalsIsNull)
            {
                myldbm.Errors.Clear();
                myldbm.CustomerId = item.Id;
                myldbm.Collection = item.Legals;
                if (!myldbm.SaveCollectionChanches())
                {
                    issuccess = false;
                    foreach (lib.DBMError err in myldbm.Errors) this.Errors.Add(err);
                }
            }
            if (!item.AliasesIsNull)
            {
                myadbm.Errors.Clear();
                myadbm.ItemId = item.Id;
                myadbm.Collection = item.Aliases;
                if (!myadbm.SaveCollectionChanches())
                {
                    issuccess = false;
                    foreach (lib.DBMError err in myadbm.Errors) this.Errors.Add(err);
                }
            }
            if (!item.CustomerAddressesIsNull)
            {
                mycdbm.Errors.Clear();
                mycdbm.ItemId = item.Id;
                mycdbm.Collection = item.Addresses;
                if (!mycdbm.SaveCollectionChanches())
                {
                    issuccess = false;
                    foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
                }
            }
            if (!item.CustomerContactsIsNull)
            {
                myccdbm.Errors.Clear();
                myccdbm.ItemId = item.Id;
                myccdbm.Collection = item.Contacts;
                if (!myccdbm.SaveCollectionChanches())
                {
                    issuccess = false;
                    foreach (lib.DBMError err in myccdbm.Errors) this.Errors.Add(err);
                }
            }
            if (!item.RecipientsIsNull)
            {
                myrdbm.Errors.Clear();
                myrdbm.CustomerId = item.Id;
                myrdbm.Collection = item.Recipients;
                if (!myrdbm.SaveCollectionChanches())
                {
                    issuccess = false;
                    foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
                }
            }
            return issuccess;
        }
        protected override bool SaveIncludedObject(Customer item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            myldbm.Command.Connection = this.Command.Connection;
            myadbm.Command.Connection = this.Command.Connection;
            mycdbm.Command.Connection = this.Command.Connection;
            myccdbm.Command.Connection = this.Command.Connection;
            myrdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetSpecificParametersValue(Customer item)
        {
            int i = 1;
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Name");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("FullName");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("DayEntry");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Recommend");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("NoteSpecial");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Status");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("PayType");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("PayAccount");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("DeliveryType");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("ManagerGroup");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("INN");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("BankAccount");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("CorrAccount");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("BankBIC");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("BankName");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("ContractNumber");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("ContractDate");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("State");
            i = 3;
            myinsertupdateparams[i++].Value = item.Name;
            myinsertupdateparams[i++].Value = item.FullName;
            myinsertupdateparams[i++].Value = item.DayEntry;
            myinsertupdateparams[i++].Value = item.Recommend;
            myinsertupdateparams[i++].Value = item.NoteSpecial;
            myinsertupdateparams[i++].Value = item.Status;
            myinsertupdateparams[i++].Value = item.PayType;
            myinsertupdateparams[i++].Value = item.PayAccount;
            myinsertupdateparams[i++].Value = item.DeliveryType;
            myinsertupdateparams[i++].Value = item.ManagerGroup?.Id;
            myinsertupdateparams[i++].Value = item.INN;
            myinsertupdateparams[i++].Value = item.BankAccount;
            myinsertupdateparams[i++].Value = item.CorrAccount;
            myinsertupdateparams[i++].Value = item.BankBIC;
            myinsertupdateparams[i++].Value = item.BankName;
            myinsertupdateparams[i++].Value = item.ContractNumber;
            myinsertupdateparams[i++].Value = item.ContractDate;
            myinsertupdateparams[i++].Value = item.State;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            foreach (SqlParameter par in SelectParams)
                switch (par.ParameterName)
                {
                    case "@param3":
                        par.Value = myname;
                        break;
                    case "@param4":
                        par.Value = myall;
                        break;
                    case "@param5":
                        par.Value = myfilter?.FilterWhereId;
                        break;
                }
        }

        internal void RefreshCollection()
        {
            this.Errors.Clear();
            this.Fill();
            foreach (Customer item in this.Collection)
            {
                if (!item.LegalsIsNull)
                {
                    myldbm.Errors.Clear();
                    myldbm.CustomerId = item.Id;
                    myldbm.Collection = item.Legals;
                    myldbm.RefreshCollection();
                    foreach (lib.DBMError err in myldbm.Errors) this.Errors.Add(err);
                }
                if (!item.AliasesIsNull)
                {
                    myadbm.Errors.Clear();
                    myadbm.ItemId = item.Id;
                    myadbm.Collection = item.Aliases;
                    myadbm.Fill();
                    foreach (lib.DBMError err in myadbm.Errors) this.Errors.Add(err);
                }
                if (!item.CustomerAddressesIsNull)
                {
                    mycdbm.Errors.Clear();
                    mycdbm.ItemId = item.Id;
                    mycdbm.Collection = item.Addresses;
                    mycdbm.Fill();
                    foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
                }
                if (!item.CustomerContactsIsNull)
                {
                    myccdbm.Errors.Clear();
                    myccdbm.ItemId = item.Id;
                    myccdbm.Collection = item.Contacts;
                    myccdbm.Fill();
                    foreach (lib.DBMError err in myccdbm.Errors) this.Errors.Add(err);
                }
                if (!item.RecipientsIsNull)
                {
                    myrdbm.Errors.Clear();
                    myrdbm.CustomerId = item.Id;
                    myrdbm.Collection = item.Recipients;
                    myrdbm.Fill();
                    foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
                }
            }
        }
        protected override void CancelLoad()
        { }
    }

    internal class CustomerStore : lib.DomainStorageLoad<Customer, CustomerDBM>
    {
        public CustomerStore(CustomerDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(Customer olditem, Customer newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class CustomerVM : lib.ViewModelErrorNotifyItem<Customer>, lib.Interfaces.INameId
    {
        public CustomerVM(Customer item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { "Name" });
            DeleteRefreshProperties.AddRange(new string[] { "Account", "BankAccount", "BankBIC", "BankName", "ContractDate", "ContractNumber", "CorrAccount", "DayEntry", "DeliveryType", "FullName", "INN", "ManagerGroup", "Name", "NoteSpecial", "PayAccount", "PayType", "Recommend", "Status" });
            InitProperties();
        }
        public CustomerVM() : this(new Customer()) { }

        public int? Account
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Account.HasValue != value.HasValue || (value.HasValue && this.DomainObject.Account.Value != value.Value)))
                {
                    string name = "Account";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Account);
                    ChangingDomainProperty = name; this.DomainObject.Account = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Account : null; }
        }
        public string BankAccount
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.BankAccount, value)))
                {
                    string name = "BankAccount";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.BankAccount);
                    ChangingDomainProperty = name; this.DomainObject.BankAccount = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.BankAccount : null; }
        }
        public string BankBIC
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.BankBIC, value)))
                {
                    string name = "BankBIC";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.BankBIC);
                    ChangingDomainProperty = name; this.DomainObject.BankBIC = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.BankBIC : null; }
        }
        public string BankName
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.BankName, value)))
                {
                    string name = "BankName";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.BankName);
                    ChangingDomainProperty = name; this.DomainObject.BankName = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.BankName : null; }
        }
        public DateTime? ContractDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.ContractDate.HasValue != value.HasValue || (value.HasValue && this.DomainObject.ContractDate.Value != value.Value)))
                {
                    string name = "ContractDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ContractDate);
                    ChangingDomainProperty = name; this.DomainObject.ContractDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ContractDate : null; }
        }
        public string ContractNumber
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.ContractNumber, value)))
                {
                    string name = "ContractNumber";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ContractNumber);
                    ChangingDomainProperty = name; this.DomainObject.ContractNumber = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ContractNumber : null; }
        }
        public string CorrAccount
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CorrAccount, value)))
                {
                    string name = "CorrAccount";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CorrAccount);
                    ChangingDomainProperty = name; this.DomainObject.CorrAccount = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CorrAccount : null; }
        }
        public DateTime? DayEntry
        {
            set
            {
                if (!this.IsReadOnly & value.HasValue && this.DomainObject.DayEntry != value.Value)
                {
                    string name = "DayEntry";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DayEntry);
                    ChangingDomainProperty = name; this.DomainObject.DayEntry = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DayEntry : (DateTime?)null; }
        }
        public int? DeliveryType
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.DeliveryType.HasValue != value.HasValue || (value.HasValue && this.DomainObject.DeliveryType.Value != value.Value)))
                {
                    string name = "DeliveryType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DeliveryType);
                    ChangingDomainProperty = name; this.DomainObject.DeliveryType = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DeliveryType : null; }
        }
        public lib.ReferenceSimpleItem DeliveryType_
        {
            get { return this.IsEnabled ? this.DomainObject.DeliveryType_ : null; }
        }
        public string FullName
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.FullName, value)))
                {
                    string name = "FullName";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FullName);
                    ChangingDomainProperty = name; this.DomainObject.FullName = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.FullName : null; }
        }
        public string INN
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.INN, value)))
                {
                    string name = "INN";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.INN);
                    ChangingDomainProperty = name; this.DomainObject.INN = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.INN : null; }
        }
        public lib.ReferenceSimpleItem ManagerGroup
        {
            set
            {
                SetProperty<lib.ReferenceSimpleItem>(this.DomainObject.ManagerGroup, (lib.ReferenceSimpleItem v) => { this.DomainObject.ManagerGroup = value; }, value);
            }
            get { return this.IsEnabled ? this.DomainObject.ManagerGroup : null; }
        }
        private string myname;
        public string Name
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(myname, value)))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myname);
                    myname = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Name = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? myname : null; }
        }
        public System.Windows.FontWeight NameFontWeight
        { get { return this.isNoteSpecial ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal; } }
        public string NoteSpecial
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.NoteSpecial, value)))
                {
                    string name = "NoteSpecial";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.NoteSpecial);
                    ChangingDomainProperty = name; this.DomainObject.NoteSpecial = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.NoteSpecial : null; }
        }
        public bool isNoteSpecial
        { get { return this.IsEnabled & this.DomainObject.isNoteSpecial; } }
        public int? ParcelCount
        { get { return this.IsEnabled ? this.DomainObject.ParcelCount : null; } }
        public DateTime? ParcelLastDate
        { get { return this.IsEnabled ? this.DomainObject.ParcelLastDate : null; } }
        public int? PayAccount
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.PayAccount.HasValue != value.HasValue || (value.HasValue && this.DomainObject.PayAccount.Value != value.Value)))
                {
                    string name = "PayAccount";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PayAccount);
                    ChangingDomainProperty = name; this.DomainObject.PayAccount = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PayAccount : null; }
        }
        public int? PayType
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.PayType.HasValue != value.HasValue || (value.HasValue && this.DomainObject.PayType.Value != value.Value)))
                {
                    string name = "PayType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PayType);
                    ChangingDomainProperty = name; this.DomainObject.PayType = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PayType : null; }
        }
        public string Recommend
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Recommend, value)))
                {
                    string name = "Recommend";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Recommend);
                    ChangingDomainProperty = name; this.DomainObject.Recommend = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Recommend : null; }
        }
        public string Status
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Status, value)))
                {
                    string name = "Status";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Status);
                    ChangingDomainProperty = name; this.DomainObject.Status = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Status : null; }
        }
        public int? State
        {
            set
            {
                if (!this.IsReadOnly & value.HasValue && this.DomainObject.State != value.Value)
                {
                    string name = "State";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.State);
                    ChangingDomainProperty = name; this.DomainObject.State = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.State : (int?)null; }
        }

        private CustomerLegalSynchronizer mylsync;
        private ListCollectionView mylegals;
        public ListCollectionView Legals
        {
            get
            {
                if (mylegals == null)
                {
                    if (mylsync == null)
                    {
                        mylsync = new CustomerLegalSynchronizer();
                        mylsync.DomainCollection = this.DomainObject.Legals;
                    }
                    mylegals = new ListCollectionView(mylsync.ViewModelCollection);
                    mylegals.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    mylegals.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
                    mylegals.MoveCurrentToPosition(-1);
                }
                return mylegals;
            }
        }
        private AliasSynchronizer myasync;
        private ListCollectionView myaliases;
        public ListCollectionView Aliases
        {
            get
            {
                if (myaliases == null)
                {
                    if (myasync == null)
                    {
                        myasync = new AliasSynchronizer();
                        myasync.DomainCollection = this.DomainObject.Aliases;
                    }
                    myaliases = new ListCollectionView(myasync.ViewModelCollection);
                    myaliases.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    myaliases.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
                }
                return myaliases;
            }
        }
        private CustomerAddressSynchronizer myadrsync;
        private ListCollectionView myaddresses;
        public ListCollectionView Addresses
        {
            get
            {
                if (myaddresses == null)
                {
                    if (myadrsync == null)
                    {
                        myadrsync = new CustomerAddressSynchronizer();
                        myadrsync.DomainCollection = this.DomainObject.Addresses;
                    }
                    myaddresses = new ListCollectionView(myadrsync.ViewModelCollection);
                    myaddresses.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                }
                return myaddresses;
            }
        }
        private CustomerContactSynchronizer mycntsync;
        private ListCollectionView mycontacts;
        public ListCollectionView Contacts
        {
            get
            {
                if (mycontacts == null)
                {
                    if (mycntsync == null)
                    {
                        mycntsync = new CustomerContactSynchronizer();
                        mycntsync.DomainCollection = this.DomainObject.Contacts;
                    }
                    mycontacts = new ListCollectionView(mycntsync.ViewModelCollection);
                    mycontacts.Filter = lib.ViewModelViewCommand.ViewFilterDefault;

                }
                return mycontacts;
            }
        }
        private RecipientSynchronizer myrsync;
        private ListCollectionView myrecipients;
        public ListCollectionView Recipients
        {
            get
            {
                if (myrecipients == null)
                {
                    if (myrsync == null)
                    {
                        myrsync = new RecipientSynchronizer();
                        myrsync.DomainCollection = this.DomainObject.Recipients;
                    }
                    myrecipients = new ListCollectionView(myrsync.ViewModelCollection);
                    myrecipients.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    myrecipients.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
                }
                return myrecipients;
            }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Name":
                    myname = this.DomainObject.Name;
                    break;
            }
        }
        protected override void InitProperties()
        {
            myname = this.DomainObject.Name;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Account":
                    this.DomainObject.Account = (int?)value;
                    break;
                case "BankAccount":
                    this.DomainObject.BankAccount = (string)value;
                    break;
                case "BankBIC":
                    this.DomainObject.BankBIC = (string)value;
                    break;
                case "BankName":
                    this.DomainObject.BankName = (string)value;
                    break;
                case "ContractDate":
                    this.DomainObject.ContractDate = (DateTime?)value;
                    break;
                case "ContractNumber":
                    this.DomainObject.ContractNumber = (string)value;
                    break;
                case "CorrAccount":
                    this.DomainObject.CorrAccount = (string)value;
                    break;
                case "DayEntry":
                    this.DomainObject.DayEntry = (DateTime)value;
                    break;
                case "DeliveryType":
                    this.DomainObject.DeliveryType = (int?)value;
                    break;
                case "FullName":
                    this.DomainObject.FullName = (string)value;
                    break;
                case "INN":
                    this.DomainObject.INN = (string)value;
                    break;
                case "ManagerGroup":
                    this.DomainObject.ManagerGroup = (lib.ReferenceSimpleItem)value;
                    break;
                case "Name":
                    if (myname != this.DomainObject.Name)
                        myname = this.DomainObject.Name;
                    else
                        this.Name = (string)value;
                    break;
                case "NoteSpecial":
                    this.DomainObject.NoteSpecial = (string)value;
                    break;
                case "PayAccount":
                    this.DomainObject.PayAccount = (int?)value;
                    break;
                case "PayType":
                    this.DomainObject.PayType = (int?)value;
                    break;
                case "Recommend":
                    this.DomainObject.Recommend = (string)value;
                    break;
                case "Status":
                    this.DomainObject.Name = (string)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    if (this.mylegals != null)
                    {
                        CustomerLegalVM[] lremoved = new CustomerLegalVM[this.DomainObject.Legals.Count];
                        foreach (CustomerLegalVM litem in this.mylsync.ViewModelCollection)
                        {
                            if (litem.DomainState == lib.DomainObjectState.Added)
                            {
                                lremoved[i] = litem;
                                i++;
                            }
                            else
                            {
                                this.mylegals.EditItem(litem);
                                litem.RejectChanges();
                                this.mylegals.CommitEdit();
                            }
                        }
                        foreach (CustomerLegalVM litem in lremoved)
                            if (litem != null) this.Legals.Remove(litem);
                    }
                    if (this.myaliases != null)
                    {
                        i = 0;
                        AliasVM[] aremoved = new AliasVM[this.DomainObject.Aliases.Count];
                        foreach (AliasVM litem in this.myasync.ViewModelCollection)
                        {
                            if (litem.DomainState == lib.DomainObjectState.Added)
                            {
                                aremoved[i] = litem;
                                i++;
                            }
                            else
                            {
                                this.myaliases.EditItem(litem);
                                litem.RejectChanges();
                                this.myaliases.CommitEdit();
                            }
                        }
                        foreach (AliasVM litem in aremoved)
                            if (litem != null) this.Aliases.Remove(litem);
                    }
                    if (this.myaddresses != null)
                    {
                        i = 0;
                        CustomerAddressVM[] removed = new CustomerAddressVM[this.DomainObject.Addresses.Count];
                        foreach (CustomerAddressVM pitem in this.myadrsync.ViewModelCollection)
                        {
                            if (pitem.DomainState == lib.DomainObjectState.Added)
                            {
                                removed[i] = pitem;
                                i++;
                            }
                            else
                            {
                                this.myaddresses.EditItem(pitem);
                                pitem.RejectChanges();
                                this.myaddresses.CommitEdit();
                            }
                        }
                        foreach (CustomerAddressVM pitem in removed)
                            if (pitem != null) this.Addresses.Remove(pitem);
                    }
                    if (this.mycontacts != null)
                    {
                        i = 0;
                        CustomerContactVM[] ccremoved = new CustomerContactVM[this.DomainObject.Contacts.Count];
                        foreach (CustomerContactVM pitem in this.mycntsync.ViewModelCollection)
                        {
                            if (pitem.DomainState == lib.DomainObjectState.Added)
                            {
                                ccremoved[i] = pitem;
                                i++;
                            }
                            else
                            {
                                this.mycontacts.EditItem(pitem);
                                pitem.RejectChanges();
                                this.mycontacts.CommitEdit();
                            }
                        }
                        foreach (CustomerContactVM pitem in ccremoved)
                            if (pitem != null) this.Contacts.Remove(pitem);
                    }
                    if (this.myrecipients != null)
                    {
                        i = 0;
                        RecipientVM[] rremoved = new RecipientVM[this.DomainObject.Recipients.Count];
                        foreach (RecipientVM litem in this.myrsync.ViewModelCollection)
                        {
                            if (litem.DomainState == lib.DomainObjectState.Added)
                            {
                                rremoved[i] = litem;
                                i++;
                            }
                            else
                            {
                                this.myrecipients.EditItem(litem);
                                litem.RejectChanges();
                                this.myrecipients.CommitEdit();
                            }
                        }
                        foreach (RecipientVM litem in rremoved)
                            if (litem != null) this.Recipients.Remove(litem);
                    }
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Name":
                    if (string.IsNullOrEmpty(this.Name))
                    {
                        errmsg = "Наименование не может быть пустым!";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myname != this.DomainObject.Name;
        }
    }

    internal class CustomerSynchronizer : lib.ModelViewCollectionsSynchronizer<Customer, CustomerVM>
    {
        protected override Customer UnWrap(CustomerVM wrap)
        {
            return wrap.DomainObject as Customer;
        }
        protected override CustomerVM Wrap(Customer fill)
        {
            return new CustomerVM(fill);
        }
    }

    public class CustomerCommand : lib.ViewModelCommand<Customer, CustomerVM, CustomerDBM>
    {
        public CustomerCommand(CustomerVM vm, ListCollectionView view) : base(vm, view)
        {
            mymanagergroups = new ListCollectionView(CustomBrokerWpf.References.ManagerGroups);
            mymanagergroups.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            try
            {
                ReferenceDS referenceDS = CustomBrokerWpf.References.ReferenceDS;
                if (referenceDS.DeliveryType.Count == 0)
                {
                    ReferenceDSTableAdapters.DeliveryType thisDeliveryTypeAdapter = new ReferenceDSTableAdapters.DeliveryType();
                    thisDeliveryTypeAdapter.Fill(referenceDS.DeliveryType);
                }
                mydeliverytypes = new System.Data.DataView(referenceDS.DeliveryType, string.Empty, string.Empty, System.Data.DataViewRowState.Unchanged | System.Data.DataViewRowState.ModifiedCurrent);
                if (referenceDS.tablePaymentType.Count == 0)
                {
                    ReferenceDSTableAdapters.PaymentTypeAdapter thisPaymentTypeAdapter = new ReferenceDSTableAdapters.PaymentTypeAdapter();
                    thisPaymentTypeAdapter.Fill(referenceDS.tablePaymentType);
                }
                mypaymenttypes = new System.Data.DataView(referenceDS.tablePaymentType, string.Empty, string.Empty, System.Data.DataViewRowState.Unchanged | System.Data.DataViewRowState.ModifiedCurrent);
                if (referenceDS.tableLegalEntity.Count == 0) referenceDS.LegalEntityRefresh();
                myaccountsettlements = referenceDS.tableLegalEntity.DefaultView;
                if (referenceDS.tableTown.Count == 0)
                {
                    ReferenceDSTableAdapters.TownAdapter thisTownAdapter = new ReferenceDSTableAdapters.TownAdapter();
                    thisTownAdapter.Fill(referenceDS.tableTown);
                }
                mytowns = new System.Data.DataView(referenceDS.tableTown, string.Empty, string.Empty, System.Data.DataViewRowState.Unchanged | System.Data.DataViewRowState.ModifiedCurrent);
                if (referenceDS.tableAddressType.Count == 0)
                {
                    ReferenceDSTableAdapters.AddressTypeAdapter thisAddressTypeAdapter = new ReferenceDSTableAdapters.AddressTypeAdapter();
                    thisAddressTypeAdapter.Fill(referenceDS.tableAddressType);
                }
                myaddresstypes = new System.Data.DataView(referenceDS.tableAddressType, string.Empty, string.Empty, System.Data.DataViewRowState.Unchanged | System.Data.DataViewRowState.ModifiedCurrent);
                if (referenceDS.ContactPointTypeTb.Rows.Count == 0)
                {
                    ReferenceDSTableAdapters.ContactPointTypeAdapter AdapterContactPointType = new ReferenceDSTableAdapters.ContactPointTypeAdapter();
                    AdapterContactPointType.Fill(referenceDS.ContactPointTypeTb);
                }
                if (referenceDS.tableContactType.Count == 0)
                {
                    ReferenceDSTableAdapters.ContactTypeAdapter thisContactTypeAdapter = new ReferenceDSTableAdapters.ContactTypeAdapter();
                    thisContactTypeAdapter.Fill(referenceDS.tableContactType);
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                    {
                        errs.Append(sqlerr.Message + "\n");
                    }
                    this.OpenPopup("Загрузка данных\n" + errs.ToString(), true);
                }
                else
                {
                    this.OpenPopup("Загрузка данных\n" + ex.Message + "\n" + ex.Source, true);
                }
            }
        }

        private System.Data.DataView mycontacttypes;
        public System.Data.DataView ContactTypes
        { get { return mycontacttypes; } }
        private System.Data.DataView mypointtypes;
        public System.Data.DataView ContactPointTypes
        { get { return mypointtypes; } }
        private ListCollectionView mystates;
        public ListCollectionView States
        {
            get
            {
                if (mystates == null)
                {
                    mystates = new ListCollectionView(CustomBrokerWpf.References.CustomerRowStates);
                }
                return mystates;
            }
        }
        private ListCollectionView mymanagergroups;
        public ListCollectionView ManagerGroups
        { get { return mymanagergroups; } }
        private System.Data.DataView mydeliverytypes;
        public System.Data.DataView DeliveryTypes
        { get { return mydeliverytypes; } }
        private System.Data.DataView mypaymenttypes;
        public System.Data.DataView PaymentTypes
        { get { return mypaymenttypes; } }
        private System.Data.DataView myaccountsettlements;
        public System.Data.DataView AccountSettlements
        { get { return myaccountsettlements; } }
        private System.Data.DataView mytowns;
        public System.Data.DataView Towns
        { get { return mytowns; } }
        private System.Data.DataView myaddresstypes;
        public System.Data.DataView AddressTypes
        { get { return myaddresstypes; } }

        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void RefreshData(object parametr)
        {
            mydbm.ItemId = myvm.DomainObject.Id;
            mydbm.GetFirst();
        }
        public override bool SaveDataChanges()
        {
            bool needrefresh = this.VModel.DomainObject.HasPropertyOutdatedValue(nameof(Customer.State));
            bool succses = base.SaveDataChanges();
            if (needrefresh)
            {
                CustomBrokerWpf.References.CustomersName.Refresh();
                CustomBrokerWpf.References.AgentNames.RefreshViews();
            }
            return succses;
        }
    }

    public class CustomerCurrentCommand : lib.ViewModelCurrentItemCommand<CustomerVM>
    {
        internal CustomerCurrentCommand()
        {
            mydbm = new CustomerDBM();
            mysync = new CustomerSynchronizer();
            mydbm.Fill();
            if (mydbm.Errors.Count > 0)
                this.OpenPopup("Загрузка данных\n" + mydbm.ErrorMessage, true);
            else
            {
                mysync.DomainCollection = (mydbm as CustomerDBM).Collection;
                base.Collection = mysync.ViewModelCollection;
            }
            myfastfilter = new RelayCommand(FastFilterExec, FastFilterCanExec);
            base.DeleteQuestionHeader = "Удалить клиента?";
			mymanagergroups = new ListCollectionView(CustomBrokerWpf.References.ManagerGroups);
			mymanagergroups.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
		}

		CustomerSynchronizer mysync;

		private ListCollectionView mymanagergroups;
		public ListCollectionView ManagerGroups
		{ get { return mymanagergroups; } }
		private ListCollectionView mystates;
        public ListCollectionView States
        {
            get
            {
                if (mystates == null)
                {
                    mystates = new ListCollectionView(CustomBrokerWpf.References.CustomerRowStates);
                }
                return mystates;
            }
        }

        private int? mystoragepointfilter;
        public int? StoragePointFilter
        {
            set
            {
                mystoragepointfilter = value;
                PropertyChangedNotification("StoragePointFilter");
            }
            get { return mystoragepointfilter; }
        }
        private RelayCommand myfastfilter;
        public System.Windows.Input.ICommand FastFilter
        {
            get { return myfastfilter; }
        }
        private void FastFilterExec(object parametr)
        {
            if (mystoragepointfilter.HasValue)
            {
                foreach (CustomerVM item in this.Items)
                {
                    if (item.Id == mystoragepointfilter.Value)
                        this.Items.MoveCurrentTo(item);
                }
            }
        }
        private bool FastFilterCanExec(object parametr)
        { return true; }

        protected override void AddData(object parametr)
        {
            if (parametr == null)
                myview.AddNew();
            else
                myview.AddNewItem(parametr);
        }
        protected override bool CanAddData(object parametr)
        {
            return this.CurrentItem == null || this.CurrentItem.Validate(true);
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
        protected override CustomerVM CreateCurrentViewItem(lib.DomainBaseNotifyChanged domainobject)
        {
            throw new NotImplementedException();
        }
        protected override void OnCurrentItemChanged()
        {
        }
        protected override void OtherViewRefresh()
        {
            CustomBrokerWpf.References.CustomerViewCollector.RefreshViews();
        }
        protected override void RefreshData(object parametr)
        {
            Customer current = this.CurrentItem?.DomainObject;
            (mydbm as CustomerDBM).RefreshCollection();
            CustomBrokerWpf.References.ReferenceDS.CustomerNameRefresh();
            if (mydbm.Errors.Count > 0)
                this.OpenPopup("Обновление данных\n" + mydbm.ErrorMessage, true);
            if (current != null)
            {
                foreach (CustomerVM item in myview)
                    if (object.Equals(current, item.DomainObject))
                        myview.MoveCurrentTo(item);
            }
        }
        protected override void RejectChanges(object parametr)
        {
            System.Collections.IList rejects;
            if (parametr is System.Collections.IList && (parametr as System.Collections.IList).Count > 0)
                rejects = parametr as System.Collections.IList;
            else
                rejects = mysync.ViewModelCollection;

            System.Collections.Generic.List<CustomerVM> deleted = new System.Collections.Generic.List<CustomerVM>();
            foreach (object item in rejects)
            {
                if (item is CustomerVM)
                {
                    CustomerVM ritem = item as CustomerVM;
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
            foreach (CustomerVM delitem in deleted)
            {
                mysync.DomainCollection.Remove(delitem.DomainObject);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
            }
        }
        protected override void SettingView()
        {
            base.SettingView();
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class CustomerViewCommand : lib.ViewModelViewCommand
    {
        public CustomerViewCommand()
        {
            mydbm = new CustomerDBM(); // default not load old
            mysync = new CustomerSynchronizer();
            //System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() =>
            //    {
            //        foreach (CustomerVM citem in mysync.ViewModelCollection)
            //            foreach (CustomerContactVM contact in citem.Contacts)
            //                contact.Points.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => { contact.Points.Filter = (object point) => { return (point as ContactPointVM).Value.Contains("@"); }; }));
            //    });
            (mydbm as CustomerDBM).FillAsyncCompleted = () =>
                {
                    if (mydbm.Errors.Count > 0)
                        this.OpenPopup("Загрузка данных\n" + mydbm.ErrorMessage, true);
                    //task.Start();
                };
            (mydbm as CustomerDBM).Collection = new ObservableCollection<Customer>();
            (mydbm as CustomerDBM).All = true;
            (mydbm as CustomerDBM).FillAsync();
            mysync.DomainCollection = (mydbm as CustomerDBM).Collection;
            base.Collection = mysync.ViewModelCollection;
            base.DeleteQuestionHeader = "Удалить клиента?";

            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            mycreateexcelreport = new RelayCommand(CreateExcelReportExec, CreateExcelReportCanExec);

            mycustomerfilter = new CustomerCheckListBoxVM();
            mycustomerfilter.ExecCommand1 = () => { this.FilterRunExec(null); };
            mycustomerlegalfilter = new CustomerLegalCheckListBoxVM();
            mycustomerlegalfilter.ExecCommand1 = () => { this.FilterRunExec(null); };
            mycustomerlegalfilter.CustomerFilter = mycustomerfilter;

            #region Filter
            myfilter = new lib.SQLFilter.SQLFilter("client", "AND", CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            mylegalfiltergroup = myfilter.GroupAdd(myfilter.FilterWhereId, "legal", "OR");
            myinnfiltergroup = myfilter.GroupAdd(myfilter.FilterWhereId, "inn", "OR");
            (mydbm as CustomerDBM).Filter = myfilter;

            mycustomerfillfilter = new CustomerCheckListBoxVMFill();
            mycustomerfillfilter.DeferredFill = true;
            mycustomerfillfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            mycustomerfillfilter.ExecCommand1 = () => { FilterRunNewExec(null); };
            mycustomerfillfilter.ExecCommand2 = () => { mycustomerfillfilter.Clear(); };
            mycustomerfillfilter.ItemsSource = myview.OfType<CustomerVM>();
            mylegalfilter = new CustomerLegalCheckListBoxVMFill();
            mylegalfilter.DeferredFill = true;
            mylegalfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            mylegalfilter.ExecCommand1 = () => { FilterRunNewExec(null); };
            mylegalfilter.ExecCommand2 = () => { mylegalfilter.Clear(); };
            mylegalfilter.ItemsSource = myview.OfType<CustomerVM>();
            //mycustomerlegalfillfilter.FillDefault = () =>
            //{
            //    bool empty = this.FilterEmpty;
            //    if (empty)
            //        foreach (CustomerLegal item in mycustomerlegalfillfilter.DefaultList)
            //            mycustomerlegalfillfilter.Items.Add(item);
            //    return empty;
            //};
            myinnfilter = new CustomerINNCheckListBoxVMFill();
            myinnfilter.DeferredFill = true;
            myinnfilter.ExecCommand1 = () => { FilterRunNewExec(null); };
            myinnfilter.ExecCommand2 = () => { myinnfilter.Clear(); };
            myinnfilter.ItemsSource = myview.OfType<CustomerVM>();
            myparcelcountfilter = new libui.NumberFilterVM();
            myparcelcountfilter.ExecCommand1 = () => { FilterRunNewExec(null); };
            myparcelcountfilter.ExecCommand2 = () => { myparcelcountfilter.Clear(); };
            myparcellastdatefilter = new libui.DateFilterVM();
            myparcellastdatefilter.ExecCommand1 = () => { FilterRunNewExec(null); };
            myparcellastdatefilter.ExecCommand2 = () => { myparcellastdatefilter.Clear(); };

            myfilterrunnew = new RelayCommand(FilterRunNewExec, FilterRunNewCanExec);
            myfilterclearnew = new RelayCommand(FilterClearNewExec, FilterClearNewCanExec);
            #endregion
        }

        CustomerSynchronizer mysync;
        private CustomerCheckListBoxVM mycustomerfilter;
        public CustomerCheckListBoxVM CustomerFilter
        { get { return mycustomerfilter; } }
        private CustomerLegalCheckListBoxVM mycustomerlegalfilter;
        public CustomerLegalCheckListBoxVM CustomerLegalFilter
        { get { return mycustomerlegalfilter; } }

        #region Filter
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { get { return myfilter; } }

        private int mylegalfiltergroup;
        private int myinnfiltergroup;

        private CustomerCheckListBoxVMFill mycustomerfillfilter;
        public CustomerCheckListBoxVMFill CustomerFillFilter
        { get { return mycustomerfillfilter; } }
        private CustomerLegalCheckListBoxVMFill mylegalfilter;
        public CustomerLegalCheckListBoxVMFill LegalFilter
        { get { return mylegalfilter; } }
        private CustomerINNCheckListBoxVMFill myinnfilter;
        public CustomerINNCheckListBoxVMFill INNFilter
        { get { return myinnfilter; } }
        private libui.NumberFilterVM myparcelcountfilter;
        public libui.NumberFilterVM ParcelCountFilter
        { get { return myparcelcountfilter; } }
        private libui.DateFilterVM myparcellastdatefilter;
        public libui.DateFilterVM ParcelLastDateFilter
        { get { return myparcellastdatefilter; } }

        private bool FilterEmpty
        {
            get
            {
                return !(mylegalfilter.FilterOn || mycustomerfillfilter.FilterOn);
            }
        }
        private void FilterActualise()
        {
            if (mycustomerfillfilter.FilterOn)
            {
                string[] items = new string[mycustomerfillfilter.SelectedItems.Count];
                for (int i = 0; i < mycustomerfillfilter.SelectedItems.Count; i++)
                    items[i] = (mycustomerfillfilter.SelectedItems[i] as Customer).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "client", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "client", new string[0]);
            List<lib.SQLFilter.SQLFilterCondition> conds = myfilter.ConditionGet(mylegalfiltergroup, "legalnull");
            if (mylegalfilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items = new string[mylegalfilter.SelectedItems.Count];
                for (int i = 0; i < mylegalfilter.SelectedItems.Count; i++)
                {
                    CustomerLegal legal = mylegalfilter.SelectedItems[i] as CustomerLegal;
                    if (legal.Id < 0)
                        isNullOrEmpty = true;
                    else
                        items[isNullOrEmpty ? i - 1 : i] = legal.Id.ToString();
                }
                if (isNullOrEmpty) Array.Resize(ref items, items.Length - 1);
                //if (string.IsNullOrEmpty(items[0]))
                //    myfilter.SetList(mylegalfiltergroup, "legal", new string[0]);
                //else
                myfilter.SetList(mylegalfiltergroup, "legal", items);
                if (isNullOrEmpty & conds.Count == 0)
                    myfilter.ConditionAdd(mylegalfiltergroup, "legalnull", "Null");
                else if (!isNullOrEmpty && conds.Count > 0)
                    myfilter.ConditionDel(conds[0].propertyid);
            }
            else
            {
                myfilter.SetList(mylegalfiltergroup, "legal", new string[0], false);
                if (conds.Count > 0)
                    myfilter.ConditionDel(conds[0].propertyid);
            }
            if (myinnfilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items = new string[myinnfilter.SelectedItems.Count];
                for (int i = 0; i < myinnfilter.SelectedItems.Count; i++)
                {
                    items[i] = (string)myinnfilter.SelectedItems[i];
                    if (string.IsNullOrEmpty(items[i]))
                        isNullOrEmpty = true;
                }
                myfilter.SetList(myinnfiltergroup, "inn", items, isNullOrEmpty);
            }
            else
                myfilter.SetList(myinnfiltergroup, "inn", new string[0], false);
            NumberFilterRun(myparcelcountfilter, "parcelcount");
            myfilter.SetDate(myfilter.FilterWhereId, "parcellastdate", "parcellastdate", myparcellastdatefilter.DateStart, myparcellastdatefilter.DateStop, myparcellastdatefilter.IsNull);
        }
        private void NumberFilterRun(libui.NumberFilterVM filter, string property)
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
                {
                    myfilter.ConditionValuesDel(cond[0].propertyid);
                    myfilter.SetNumber(myfilter.FilterWhereId, property, filter.Operator, filter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }
            }
            else if (cond.Count > 0)
                myfilter.ConditionDel(cond[0].propertyid);
        }

        private RelayCommand myfilterrunnew;
        public ICommand FilterRunNew
        {
            get { return myfilterrunnew; }
        }
        private void FilterRunNewExec(object parametr)
        {
            this.EndEdit();
            FilterActualise();
            RefreshData(null);
        }
        private bool FilterRunNewCanExec(object parametr)
        { return true; }
        private RelayCommand myfilterclearnew;
        public ICommand FilterClearNew
        {
            get { return myfilterclearnew; }
        }
        private void FilterClearNewExec(object parametr)
        {
            mycustomerfillfilter.Clear();
            mycustomerfillfilter.IconVisibileChangedNotification();
            mylegalfilter.Clear();
            mylegalfilter.IconVisibileChangedNotification();
            myinnfilter.Clear();
            myinnfilter.IconVisibileChangedNotification();
            this.OverallFilterSet = string.Empty;
            this.FilterWin = new ItemFilter[1] { new ItemFilter("State", "=", "0") };
        }
        private bool FilterClearNewCanExec(object parametr)
        { return true; }

        private string myoverallfilter;
        public string OverallFilter
        {
            set
            {
                myoverallfilter = value;
                this.PropertyChangedNotification("OverallFilter");
                this.Items.Filter = OverallFilterOn;
            }
            get { return myoverallfilter; }
        }
        public string OverallFilterSet
        {
            set
            {
                myoverallfilter = value;
                this.PropertyChangedNotification("OverallFilter");
            }
        }
        internal ItemFilter[] FilterWin = new ItemFilter[1] { new ItemFilter("State", "=", "0") };
        internal bool OverallFilterOn(object item)
        {
            bool where = lib.ViewModelViewCommand.ViewFilterDefault(item);
            CustomerVM citem = item as CustomerVM;
            
            if (where & !string.IsNullOrEmpty(myoverallfilter))
            {
                where = false;
                string filter = myoverallfilter.ToLower();
                if (citem.FullName?.ToLower().IndexOf(filter) > -1 || citem.Name?.ToLower().IndexOf(filter) > -1)
                    where = true;
                else
                {
                    if (citem.Legals.Count > 0)
                        foreach (CustomerLegalVM legal in citem.Legals.OfType<CustomerLegalVM>())
                        {
                            if (legal.FullName?.ToLower().IndexOf(filter) > -1 || legal.Name?.ToLower().IndexOf(filter) > -1)
                            { where = true; break; }
                            else if (legal.INN?.ToLower().IndexOf(filter) > -1)
                            { where = true; break; }
                            if (!where)
                            {
                                foreach (AliasVM alias in legal.Aliases.OfType<AliasVM>())
                                    if (alias?.Name?.ToLower().IndexOf(filter) > -1)
                                    { where = true; break; }
                            }
                            if (!where)
                            {
                                foreach (CustomerAddressVM address in legal.Addresses.OfType<CustomerAddressVM>())
                                    if (address?.AddressDescription?.ToLower().IndexOf(filter) > -1)
                                    { where = true; break; }
                                    else if (address?.Locality?.ToLower().IndexOf(filter) > -1)
                                    { where = true; break; }
                                    else if (address?.Town?.ToLower().IndexOf(filter) > -1)
                                    { where = true; break; }
                                    else if (address?.AddressTypeID > 0)
                                    {
                                        if (CustomBrokerWpf.References.ReferenceDS.tableAddressType.Count == 0)
                                        {
                                            ReferenceDSTableAdapters.AddressTypeAdapter thisAddressTypeAdapter = new ReferenceDSTableAdapters.AddressTypeAdapter();
                                            thisAddressTypeAdapter.Fill(CustomBrokerWpf.References.ReferenceDS.tableAddressType);
                                        }
                                        if (CustomBrokerWpf.References.ReferenceDS.tableAddressType.FindByaddresstypeID(address.AddressTypeID.Value).addresstypeName.ToLower().IndexOf(filter) > -1)
                                            where = true; break;
                                    }
                            }
                            if (!where)
                            {
                                foreach (CustomerContactVM contact in legal.Contacts.OfType<CustomerContactVM>())
                                    if (contact?.FullName.ToLower().IndexOf(filter) > -1)
                                    { where = true; break; }
                                    else
                                    {
                                        foreach (ContactPointVM point in contact.Points.OfType<ContactPointVM>())
                                            if (point?.Name?.ToLower().IndexOf(filter) > -1)
                                            { where = true; break; }
                                            else if (point?.Value?.ToLower().IndexOf(filter) > -1)
                                            { where = true; break; }
                                            else if (!string.IsNullOrEmpty(point?.Value) && point?.Value?.ToLower().Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).IndexOf(filter) > -1)
                                            { where = true; break; }
                                        if (where) break;
                                    }
                            }
                        }
                    if (!where)
                    {
                        foreach (AliasVM alias in citem.Aliases.OfType<AliasVM>())
                            if (alias?.Name?.ToLower().IndexOf(filter) > -1)
                            { where = true; break; }
                    }
                    if (!where)
                    {
                        foreach (CustomerAddressVM address in citem.Addresses.OfType<CustomerAddressVM>())
                            if (address?.AddressDescription?.ToLower().IndexOf(filter) > -1)
                            { where = true; break; }
                            else if (address?.Locality?.ToLower().IndexOf(filter) > -1)
                            { where = true; break; }
                            else if (address?.Town?.ToLower().IndexOf(filter) > -1)
                            { where = true; break; }
                            else if (address?.AddressTypeID > 0)
                            {
                                if (CustomBrokerWpf.References.ReferenceDS.tableAddressType.Count == 0)
                                {
                                    ReferenceDSTableAdapters.AddressTypeAdapter thisAddressTypeAdapter = new ReferenceDSTableAdapters.AddressTypeAdapter();
                                    thisAddressTypeAdapter.Fill(CustomBrokerWpf.References.ReferenceDS.tableAddressType);
                                }
                                if (CustomBrokerWpf.References.ReferenceDS.tableAddressType.FindByaddresstypeID(address.AddressTypeID.Value).addresstypeName.ToLower().IndexOf(filter) > -1)
                                    where = true; break;
                            }
                    }
                    if (!where)
                    {
                        foreach (CustomerContactVM contact in citem.Contacts.OfType<CustomerContactVM>())
                            if (contact?.FullName.ToLower().IndexOf(filter) > -1)
                            { where = true; break; }
                            else
                            {
                                foreach (ContactPointVM point in contact.Points.OfType<ContactPointVM>())
                                    if (point?.Name?.ToLower().IndexOf(filter) > -1)
                                    { where = true; break; }
                                    else if (point?.Value?.ToLower().IndexOf(filter) > -1)
                                    { where = true; break; }
                                    else if (!string.IsNullOrEmpty(point?.Value) && point?.Value?.ToLower().Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).IndexOf(filter) > -1)
                                    { where = true; break; }
                                if (where) break;
                            }
                    }
                    if (!where && citem.ParcelLastDate?.ToShortDateString().IndexOf(filter) > -1)
                        where = true;
                }
            }

            string[] ids;
            if (where)
                foreach (ItemFilter filter in this.FilterWin)
                {
                    if (!where) break;
                    if (!(filter is ItemFilter)) continue;
                    switch (filter.PropertyName)
                    {
                        case "AliasCustomer":
                            where = false;
                            foreach (Classes.Domain.AliasVM alias in citem.Aliases.OfType<Classes.Domain.AliasVM>())
                            {
                                if (alias.Name?.ToLower().IndexOf(filter.Value) > -1)
                                {
                                    where = true;
                                    break;
                                }
                            }
                            break;
                        case "CustomerRecipient":
                            where = false;
                            foreach (Classes.Domain.RecipientVM rsp in citem.Recipients.OfType<Classes.Domain.RecipientVM>())
                            {
                                if (rsp.Name.IndexOf(filter.Value) > -1 || rsp.FullName?.IndexOf(filter.Value) > -1)
                                {
                                    where = true;
                                    break;
                                }
                            }
                            break;
                        case "managergroupID":
                            where = false;
                            ids = filter.Value.Split(',');
                            foreach (string id in ids)
                                if (citem.ManagerGroup?.Id == int.Parse(id))
                                {
                                    where = true;
                                    break;
                                }
                            break;
                        case "paytypeID":
                            where = false;
                            ids = filter.Value.Split(',');
                            foreach (string id in ids)
                                if (citem.PayType.HasValue && citem.PayType.Value == int.Parse(id))
                                {
                                    where = true;
                                    break;
                                }
                            break;
                        case "deliverytypeID":
                            where = false;
                            ids = filter.Value.Split(',');
                            foreach (string id in ids)
                                if (citem.DeliveryType.HasValue && citem.DeliveryType.Value == int.Parse(id))
                                {
                                    where = true;
                                    break;
                                }
                            break;
                        case "customerID":
                            where = citem.Id.ToString() == filter.Value;
                            break;
                        case "customerDayEntry":
                            if (filter.Operation == "Between")
                                where = citem.DayEntry >= DateTime.Parse(filter.Value.Substring(0, filter.Value.IndexOf(' '))) && citem.DayEntry < DateTime.Parse(filter.Value.Substring(filter.Value.IndexOf(' ') + 1));
                            else if (filter.Operation == ">")
                                where = citem.DayEntry >= DateTime.Parse(filter.Value);
                            else if (filter.Operation == "<")
                                where = citem.DayEntry < DateTime.Parse(filter.Value);
                            break;
                        case "customerRecommend":
                            where = (citem.Recommend?.ToLower().IndexOf(filter.Value) ?? -1) > -1;
                            break;
                        case "customerNoteSpecial":
                            where = (citem.NoteSpecial?.ToLower().IndexOf(filter.Value) ?? -1) > -1;
                            break;
                        case "Town":
                            where = false;
                            foreach (Classes.Domain.CustomerAddressVM adr in citem.Addresses.OfType<Classes.Domain.CustomerAddressVM>())
                            {
                                if (adr.Town?.ToLower() == filter.Value)
                                {
                                    where = true;
                                    break;
                                }
                            }
                            break;
                        case "Locality":
                            where = false;
                            foreach (Classes.Domain.CustomerAddressVM adr in citem.Addresses.OfType<Classes.Domain.CustomerAddressVM>())
                            {
                                if ((adr.Locality?.ToLower().IndexOf(filter.Value) ?? -1) > -1)
                                {
                                    where = true;
                                    break;
                                }
                            }
                            break;
                        case "FIO":
                            where = false;
                            foreach (Classes.Domain.CustomerContactVM cnt in citem.Contacts.OfType<Classes.Domain.CustomerContactVM>())
                            {
                                if ((cnt.Name?.ToLower().IndexOf(filter.Value) ?? -1) > -1 || (cnt.SurName?.ToLower().IndexOf(filter.Value) ?? -1) > -1 || (cnt.ThirdName?.ToLower().IndexOf(filter.Value) ?? -1) > -1)
                                {
                                    where = true;
                                    break;
                                }
                            }
                            break;
                        case "PointValue":
                            where = false;
                            foreach (Classes.Domain.CustomerContactVM cnt in citem.Contacts.OfType<Classes.Domain.CustomerContactVM>())
                            {
                                foreach (Classes.Domain.ContactPointVM pnt in cnt.Points.OfType<Classes.Domain.ContactPointVM>())
                                    if (pnt.Value?.ToLower().IndexOf(filter.Value) > -1)
                                    {
                                        where = true;
                                        break;
                                    }
                                if (where) break;
                            }
                            break;
                        case "State":
                            if (string.IsNullOrEmpty(myoverallfilter))
                            {
                                where = false;
                                ids = filter.Value.Split(',');
                                foreach (string state in ids)
                                {
                                    if ((citem.State ?? 0) == int.Parse(state))
                                    {
                                        where = true;
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                }
            return where;
        }
        #endregion

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            this.EndEdit();
            myview.Filter = FilterOn;
        }
        private bool FilterRunCanExec(object parametr)
        { return true; }
        private bool FilterOn(object item)
        {
            bool where = lib.ViewModelViewCommand.ViewFilterDefault(item);
            CustomerVM citem = item as CustomerVM;

            if (where & mycustomerfilter.FilterOn)
            {
                where = false;
                foreach (Customer sitem in mycustomerfilter.SelectedItems)
                    if (citem.DomainObject == sitem)
                    {
                        where = true;
                        break;
                    }
            }
            if (where & mycustomerlegalfilter.FilterOn)
            {
                where = false;
                foreach (Customer sitem in mycustomerlegalfilter.SelectedItems)
                    foreach (CustomerVM litem in citem.Legals)
                        if (litem.DomainObject == sitem)
                        {
                            where = true;
                            break;
                        }
            }

            return where;
        }
        private RelayCommand myfilterclear;
        public ICommand FilterClear
        {
            get { return myfilterclear; }
        }
        private void FilterClearExec(object parametr)
        {
            mycustomerfilter.Clear();
            mycustomerfilter.IconVisibileChangedNotification();
            mycustomerlegalfilter.Clear();
            mycustomerlegalfilter.IconVisibileChangedNotification();
            myoverallfilter = string.Empty;
            myfilterclearnew.Execute(null);
            myfilterrunnew.Execute(null);
            this.FilterRunExec(null);
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }

        private RelayCommand mycreateexcelreport;
        public ICommand CreateExcelReport
        {
            get { return mycreateexcelreport; }
        }
        private void CreateExcelReportExec(object parametr)
        {
            ExcelReport();
        }
        private bool CreateExcelReportCanExec(object parametr)
        { return true; }
        private void ExcelReport()
        {
            excel.Application exApp = new excel.Application();
            excel.Application exAppProt = new excel.Application();
            excel.Workbook exWb;
            try
            {
                int i = 2, ii = 0, iii = 0;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                excel.Worksheet exWh = exWb.Sheets[1];
                //excel.Range r;
                //exWh.Name = this.CurrentItem.ParcelNumberEntire;
                exWh.Cells[1, 1] = "Клиент"; exWh.Cells[1, 2] = "Контакты: тип"; exWh.Cells[1, 3] = "лицо"; exWh.Cells[1, 4] = "описание"; exWh.Cells[1, 5] = "контакт";// exWh.Cells[1, 5] = "Юр. лица"; exWh.Cells[1, 6] = "Поставщик"; exWh.Cells[1, 7] = "Импортер"; exWh.Cells[1, 8] = "Группа менеджеров";
                //exWh.Cells[1, 9] = "Кол-во мест"; exWh.Cells[1, 10] = "Вес по док, кг"; exWh.Cells[1, 11] = "Вес факт, кг"; exWh.Cells[1, 12] = "Объем, м3"; exWh.Cells[1, 13] = "Инвойс"; exWh.Cells[1, 14] = "Инвойс, cо скидкой"; exWh.Cells[1, 15] = "Услуга"; exWh.Cells[1, 16] = "Примечание менеджера";
                //r = exWh.Columns[9, Type.Missing]; r.NumberFormat = "#,##0.00";
                //r = exWh.Columns[10, Type.Missing]; r.NumberFormat = "#,##0.00";
                //r = exWh.Columns[11, Type.Missing]; r.NumberFormat = "#,##0.00";
                //r = exWh.Columns[12, Type.Missing]; r.NumberFormat = "#,##0.00";
                //r = exWh.Columns[13, Type.Missing]; r.NumberFormat = "#,##0.00";
                //r = exWh.Columns[14, Type.Missing]; r.NumberFormat = "#,##0.00";
                foreach (CustomerVM item in this.Items)
                {
                    if (!string.IsNullOrEmpty(item.Name)) exWh.Cells[i, 1] = item.Name;
                    ii = 0;
                    foreach (CustomerContactVM contact in item.Contacts)
                    {
                        if (!string.IsNullOrEmpty(contact.ContactType)) exWh.Cells[i + ii, 2] = contact.ContactType;
                        if (!string.IsNullOrEmpty(contact.FullName)) exWh.Cells[i + ii, 3] = contact.FullName;
                        iii = 0;
                        foreach (ContactPointVM point in contact.Points)
                        {
                            if (!string.IsNullOrEmpty(point.Name)) exWh.Cells[i + ii + iii, 4] = point.Name;
                            if (!string.IsNullOrEmpty(point.Value)) exWh.Cells[i + ii + iii, 5] = point.Value;
                            iii++;
                        }
                        if (iii > 0) iii--;
                        ii += iii;
                        ii++;
                    }
                    if (ii > 0) ii--;
                    i += ii;
                    i++;
                }
                //if (i > 2)
                //{
                //    string filename = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Отправки", this.CurrentItem.DocDirPath, this.CurrentItem.Lorry + " - " + (importerid == 1 ? "Трейд" : (importerid == 2 ? "Деливери" : string.Empty)) + ".xlsx");
                //    if (File.Exists(filename))
                //        File.Delete(filename);
                //    exWb.SaveAs(Filename: filename);
                //    exApp.Visible = true;
                //}
                //else
                //    exWb.Close(false);
                exApp.Visible = true;
            }
            catch (Exception ex)
            {
                if (exApp != null)
                {
                    foreach (excel.Workbook itemBook in exApp.Workbooks)
                    {
                        itemBook.Close(false);
                    }
                    exApp.Quit();
                }
                this.OpenPopup("Выгрузка в Excel/n" + ex.Message, true);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }

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
            CustomBrokerWpf.References.CustomersName.Refresh();
            CustomBrokerWpf.References.CustomersName.RefreshViews();
        }

        protected override void AddData(object parametr)
        {
            base.AddData(parametr);
        }
        protected override void RefreshData(object parametr)
        {
            mydbm.Fill();
            // refresh CustomersName in another views
            CustomBrokerWpf.References.CustomersName.Refresh();
            CustomBrokerWpf.References.CustomersName.RefreshViews();
        }
        protected override void RejectChanges(object parametr)
        {

        }

        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class CustomerCheckListBoxVM : libui.CheckListBoxVM
    {
        internal CustomerCheckListBoxVM()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return (item as Customer).Name; };
            this.SelectedAll = false;
            this.RefreshIsVisible = false;
            this.ExecCommand2 = () => { this.Clear(); };

            mydbm = new CustomerDBM();
            mydbm.Collection = new ObservableCollection<Customer>();
            mydbm.FillAsyncCompleted = () => { this.ItemsView.Refresh(); PropertyChangedNotification("ItemsView"); };
            mydbm.FillAsync();
            this.Items = mydbm.Collection;
            this.ItemsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        CustomerDBM mydbm;
    }
    public class CustomerLegalCheckListBoxVM : libui.CheckListBoxVM
    {
        internal CustomerLegalCheckListBoxVM()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return (item as CustomerLegal).Name; };
            this.SelectedAll = false;
            this.RefreshIsVisible = true;
            this.ExecRefresh = () => { this.Fill(); };
            this.ExecCommand2 = () => { this.Clear(); };

            mydbm = new CustomerLegalDBM();
            mydbm.Collection = new ObservableCollection<CustomerLegal>();
            mydbm.FillAsyncCompleted = () => { this.ItemsView.Refresh(); PropertyChangedNotification("ItemsView"); };
            mydbm.FillAsync();
            this.Items = mydbm.Collection;
            this.ItemsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        CustomerLegalDBM mydbm;
        private CustomerCheckListBoxVM mycustomerfilter;
        internal CustomerCheckListBoxVM CustomerFilter
        { set { mycustomerfilter = value; } }

        private void Fill()
        {
            if (mycustomerfilter != null && mycustomerfilter.FilterOn)
                this.ItemsView.Filter = (object item) =>
                {
                    bool isfind = false;
                    foreach (Customer customer in mycustomerfilter.SelectedItems)
                        isfind |= customer == (item as CustomerLegal).Customer;
                    return isfind;
                };
            else
                this.ItemsView.Filter = null;
        }
    }

    public class CustomerCheckListBoxVMFill : libui.CheckListBoxVMFill<CustomerVM, Customer>
    {
        internal CustomerCheckListBoxVMFill() : base()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return ((Customer)item).Name; };
        }

        //private List<Customer> mydefaultlist;
        //internal List<Customer> DefaultList
        //{
        //    get
        //    {
        //        if (mydefaultlist == null)
        //        {
        //            mydefaultlist = new List<Customer>(); // из за долгой загрузки
        //            CustomerDBM dbm = new CustomerDBM();
        //            dbm.All = true;
        //            dbm.Fill();
        //            mydefaultlist = dbm.Collection.ToList<Customer>();
        //        }
        //        return mydefaultlist;
        //    }
        //}

        protected override void AddItem(CustomerVM item)
        {
            if (!Items.Contains(item.DomainObject)) Items.Add(item.DomainObject);
        }
    }
    public class CustomerLegalCheckListBoxVMFill : libui.CheckListBoxVMFill<CustomerVM, CustomerLegal>
    {
        internal CustomerLegalCheckListBoxVMFill() : base()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return ((CustomerLegal)item).Name; };
            legal0 = new CustomerLegal();
        }

        //private List<CustomerLegal> mydefaultlist;
        //internal List<CustomerLegal> DefaultList
        //{
        //    get
        //    {
        //        if (mydefaultlist == null)
        //        {
        //            mydefaultlist = new List<CustomerLegal>(); // из за долгой загрузки
        //            CustomerLegalDBM dbm = new CustomerLegalDBM();
        //            dbm.Fill();
        //            mydefaultlist = dbm.Collection.ToList<CustomerLegal>();
        //        }
        //        return mydefaultlist;
        //    }
        //}

        private CustomerLegal legal0;
        protected override void AddItem(CustomerVM item)
        {
            if (item.Legals.OfType<CustomerLegalVM>().Count() > 0)
                foreach (CustomerLegalVM legal in item.Legals.OfType<CustomerLegalVM>())
                { if (!Items.Contains(legal.DomainObject ?? legal0)) Items.Add(legal.DomainObject ?? legal0); }
            else if (!Items.Contains(legal0)) Items.Add(legal0);
        }
    }
    public class CustomerINNCheckListBoxVMFill : libui.CheckListBoxVMFill<CustomerVM, string>
    {
        internal CustomerINNCheckListBoxVMFill() : base()
        {
            //this.DisplayPath = "Name";
            //this.SearchPath = "Name";
            //this.GetDisplayPropertyValueFunc = (item) => { return ((CustomerLegalVM)item).Name; };
        }

        protected override void AddItem(CustomerVM item)
        {
            foreach (CustomerLegalVM legal in item.Legals.OfType<CustomerLegalVM>())
                if (!Items.Contains(legal.INN ?? string.Empty)) Items.Add(legal.INN ?? string.Empty);
        }
    }
}
