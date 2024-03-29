﻿using System;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class CustomerLegal : lib.DomainBaseStamp,lib.Interfaces.INameId
    {
        public CustomerLegal(int id, long stamp, string updater, DateTime? updated, lib.DomainObjectState dstate
            , int? account, string bankaccount, string bankbic, string bankname, DateTime? contractdate, string contractnum, string corraccount, Customer customer, DateTime dayentry, int? deliverytype, byte edod, byte edot, string fullname, string inn, lib.ReferenceSimpleItem managergroup, string name, string notespecial, int? payaccount, int? paytypeid, string recommend, int state, string status
            ) : base(id, stamp, updated, updater, dstate)
        {
            myaccount = account;
            mybankaccount = bankaccount;
            mybankbic = bankbic;
            mybankname = bankname;
            mycontractdate = contractdate;
            mycontractnum = contractnum;
            mycorraccount = corraccount;
            mycustomer = customer;
            mydayentry = dayentry;
            mydeliverytype = deliverytype;
            mydeliverytype_ = deliverytype.HasValue ? CustomBrokerWpf.References.DeliveryTypes.FindFirstItem("Id", deliverytype.Value) : null;
            myedod = edod;
            myedot = edot;
            myfullname = fullname;
            myinn = inn;
            mymanagergroup = managergroup;
            myname = name;
            mynotespecial = notespecial;
            mypayaccount = payaccount;
            mypaytypeid = paytypeid;
            myrecommend = recommend;
            mystate = state;
            mystatus = status;
        }
        public CustomerLegal() : this(id: lib.NewObjectId.NewId, stamp: 0, updater: null, updated: null, dstate: lib.DomainObjectState.Added
            , account: null, bankaccount: null, bankbic: null, bankname: null, contractdate: null, contractnum: null, corraccount: null, customer: null, dayentry: DateTime.Now, deliverytype: null, edod: 0, edot: 0, fullname: null, inn: null, managergroup: null, name: null, notespecial: null, payaccount: null, paytypeid: null, recommend: null, state: 0, status: "Заявка"
            ) { }

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
        private Customer mycustomer;
        public Customer Customer
        {
            set
            {
                SetProperty<Customer>(ref mycustomer, value);
            }
            get { return mycustomer; }
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
                base.SetProperty<int?>(ref mydeliverytype, value, () => {

                    mydeliverytype_ = value.HasValue ? CustomBrokerWpf.References.DeliveryTypes.FindFirstItem("Id", mydeliverytype.Value) : null;
                    this.PropertyChangedNotification(nameof(this.DeliveryType_));
                });
            }
            get { return mydeliverytype; }
        }
        private lib.ReferenceSimpleItem mydeliverytype_;
        public lib.ReferenceSimpleItem DeliveryType_
        { get { return mydeliverytype_; } }
        private byte myedod;
        public byte EDOD
        { set { SetProperty(ref myedod, value); } get { return myedod; } }
        private byte myedot;
        public byte EDOT
        { set { SetProperty(ref myedot, value); } get { return myedot; } }
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
            get { return mymanagergroup==null? mycustomer.ManagerGroup : mymanagergroup; }
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
            if(e.Action==System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach(Alias item in e.NewItems)
                    item.CustomerId=this.Id;
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
        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            CustomerLegal newitem = (CustomerLegal)sample;
            if (!this.HasPropertyOutdatedValue("Account")) this.Account = newitem.Account;
            if (!this.HasPropertyOutdatedValue("BankAccount")) this.BankAccount = newitem.BankAccount;
            if (!this.HasPropertyOutdatedValue("BankBIC")) this.BankBIC = newitem.BankBIC;
            if (!this.HasPropertyOutdatedValue("BankName")) this.BankName = newitem.BankName;
            if (!this.HasPropertyOutdatedValue("ContractDate")) this.ContractDate = newitem.ContractDate;
            if (!this.HasPropertyOutdatedValue("ContractNumber")) this.ContractNumber = newitem.ContractNumber;
            if (!this.HasPropertyOutdatedValue("CorrAccount")) this.CorrAccount = newitem.CorrAccount;
            if (!this.HasPropertyOutdatedValue("DayEntry")) this.DayEntry = newitem.DayEntry;
            if (!this.HasPropertyOutdatedValue("DeliveryType")) this.DeliveryType = newitem.DeliveryType;
            this.EDOD = newitem.EDOD;
            this.EDOT = newitem.EDOT;
            this.FullName = newitem.FullName;
            if (!this.HasPropertyOutdatedValue("INN")) this.INN = newitem.INN;
            if (!this.HasPropertyOutdatedValue("ManagerGroup")) this.ManagerGroup = newitem.ManagerGroup;
            this.Name = newitem.Name;
            if (!this.HasPropertyOutdatedValue("NoteSpecial")) this.NoteSpecial = newitem.NoteSpecial;
            if (!this.HasPropertyOutdatedValue("PayAccount")) this.PayAccount = newitem.PayAccount;
            if (!this.HasPropertyOutdatedValue("PayType")) this.PayType = newitem.PayType;
            if (!this.HasPropertyOutdatedValue("Recommend")) this.Recommend = newitem.Recommend;
            if (!this.HasPropertyOutdatedValue("Status")) this.Status = newitem.Status;
            if (!this.HasPropertyOutdatedValue("State")) this.State = newitem.State;
        }
        public override string ToString()
        {
            return this.Name;
        }
    }

    public class CustomerLegalDBM : lib.DBManagerWhoWhen<CustomerRecord,CustomerLegal>
    {
        public CustomerLegalDBM()
        {
            base.NeedAddConnection = true;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.CustomerLegal_sp";
            InsertCommandText = "dbo.CustomerAdd_sp";
            UpdateCommandText = "dbo.CustomerUpd_sp";
            DeleteCommandText = "dbo.CustomerDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int),
                new SqlParameter("@param2", System.Data.SqlDbType.Int)
            };
            myinsertparams = new SqlParameter[]
           {
                myinsertparams[0],myinsertparams[1]
                ,new SqlParameter("@parentid", System.Data.SqlDbType.Int)
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
                ,new SqlParameter("@edodtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@edottrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@old", 0)
            };
            myupdateparams[0].ParameterName = "@customerID";
            myinsertupdateparams = new SqlParameter[]
           {
                myinsertupdateparams[0],myinsertupdateparams[1]
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
                ,new SqlParameter("@edod", System.Data.SqlDbType.TinyInt)
                ,new SqlParameter("@edot", System.Data.SqlDbType.TinyInt)
           };
            myinsertupdateparams[0].ParameterName = "@updtDate";
            myinsertupdateparams[1].ParameterName = "@updtWho";
            myadbm = new AliasDBM();
            mycdbm = new CustomerAddressDBM();
            myccdbm = new CustomerContactDBM();
            myrdbm = new RecipientDBM();
        }

        private AliasDBM myadbm;
        private CustomerAddressDBM mycdbm;
        private CustomerContactDBM myccdbm;
        private RecipientDBM myrdbm;
        internal int CustomerId
        {
            set { base.SelectParams[1].Value = value; }
            get { return (int)base.SelectParams[1].Value; }
        }

        protected override void SetSelectParametersValue()
        {
        }
        protected override CustomerRecord CreateRecord(SqlDataReader reader)
        {
            return new CustomerRecord()
            {
                id= reader.GetInt32(0), stamp= reader.GetInt32(1), updater= reader.IsDBNull(3) ? null : reader.GetString(3), updated= reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2)
                , bankaccount= reader.IsDBNull(this.Fields["raccount"]) ? null : reader.GetString(this.Fields["raccount"])
                , bankbic= reader.IsDBNull(this.Fields["bankbic"]) ? null : reader.GetString(this.Fields["bankbic"])
                , bankname= reader.IsDBNull(this.Fields["bankname"]) ? null : reader.GetString(this.Fields["bankname"])
                , contractdate= reader.IsDBNull(this.Fields["contractdate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["contractdate"])
                , contractnum= reader.IsDBNull(this.Fields["contractnum"]) ? null : reader.GetString(this.Fields["contractnum"])
                , corraccount= reader.IsDBNull(this.Fields["coraccount"]) ? null : reader.GetString(this.Fields["coraccount"])
                , parent=reader.GetInt32(this.Fields["parentid"])
                , dayentry= reader.GetDateTime(this.Fields["customerDayEntry"])
                , deliverytype= reader.IsDBNull(this.Fields["deliverytypeID"]) ? (int?)null : reader.GetInt32(this.Fields["deliverytypeID"])
                , edod= reader.GetByte(this.Fields["edod"])
                , edot= reader.GetByte(this.Fields["edot"])
                , fullname= reader.IsDBNull(this.Fields["customerFullName"]) ? null : reader.GetString(this.Fields["customerFullName"])
                , inn= reader.IsDBNull(this.Fields["inn"]) ? null : reader.GetString(this.Fields["inn"])
                , managergroup= reader.IsDBNull(this.Fields["managerGroupID"]) ? (int?)null : reader.GetInt32(this.Fields["managerGroupID"])
                , name= reader.IsDBNull(this.Fields["customerName"]) ? null : reader.GetString(this.Fields["customerName"])
                , notespecial= reader.IsDBNull(this.Fields["customerNoteSpecial"]) ? null : reader.GetString(this.Fields["customerNoteSpecial"])
                , payaccount= reader.IsDBNull(this.Fields["payaccount"]) ? (int?)null : reader.GetInt32(this.Fields["payaccount"])
                , paytypeid= reader.IsDBNull(this.Fields["paytypeID"]) ? (int?)null : reader.GetInt32(this.Fields["paytypeID"])
                , recommend= reader.IsDBNull(this.Fields["customerRecommend"]) ? null : reader.GetString(this.Fields["customerRecommend"])
                , state= reader.GetByte(this.Fields["customerState"])
                , status= reader.IsDBNull(this.Fields["customerStatus"]) ? null : reader.GetString(this.Fields["customerStatus"])
            };
        }
        protected override CustomerLegal CreateModel(CustomerRecord record,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
			System.Collections.Generic.List<lib.DBMError> errors;
            Customer customer = CustomBrokerWpf.References.CustomerStore.GetItemLoad(record.parent, addcon, out errors);
            this.Errors.AddRange(errors);
            CustomerLegal newitem = new CustomerLegal(id: record.id, stamp: record.stamp, updater: record.updater, updated: record.updated, dstate: lib.DomainObjectState.Unchanged
                , account: null
                , bankaccount:record.bankaccount
                , bankbic: record.bankbic
                , bankname: record.bankname
                , contractdate: record.contractdate
                , contractnum: record.contractnum
                , corraccount: record.corraccount
                , customer: customer
                , dayentry: record.dayentry
                , deliverytype: record.deliverytype
                , edod: record.edod
                , edot: record.edot
                , fullname: record.fullname
                , inn: record.inn
                , managergroup: record.managergroup.HasValue ? CustomBrokerWpf.References.ManagerGroups.FindFirstItem("Id", record.managergroup) : null
                , name: record.name
                , notespecial: record.notespecial
                , payaccount: record.payaccount
                , paytypeid: record.paytypeid
                , recommend: record.recommend
                , state: record.state
                , status: record.status
                );
            return CustomBrokerWpf.References.CustomerLegalStore.UpdateItem(newitem);
        }
        protected override bool SaveChildObjects(CustomerLegal item)
        {
            bool issuccess = true;
            if(!item.AliasesIsNull)
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
        protected override bool SaveIncludedObject(CustomerLegal item)
        {
            bool issuccess = true;
            if(item.Customer.DomainState==lib.DomainObjectState.Added)
            {
                CustomerDBM cdbm = new CustomerDBM();
                cdbm.Command = new SqlCommand() { Connection = this.Command.Connection };
                if (!cdbm.SaveItemChanches(item.Customer))
                {
                    issuccess = false;
                    foreach (lib.DBMError err in cdbm.Errors) this.Errors.Add(err);
                }
            }
            return issuccess;
        }
        protected override bool SaveReferenceObjects()
        {
            myadbm.Command.Connection = this.Command.Connection;
            mycdbm.Command.Connection = this.Command.Connection;
            myccdbm.Command.Connection = this.Command.Connection;
            myrdbm.Command.Connection = this.Command.Connection;

            return true;
        }
        protected override bool SetParametersValue(CustomerLegal item)
        {
            base.SetParametersValue(item);
            if (item.Customer.DomainState == lib.DomainObjectState.Added)
                return false;
            myinsertparams[2].Value = item.Customer.Id;
            foreach(SqlParameter par in this.UpdateParams)
                switch(par.ParameterName)
                {
                    case "@raccounttrue":
                        par.Value = item.HasPropertyOutdatedValue("BankAccount");
                        break;
                    case "@bankbictrue":
                        par.Value = item.HasPropertyOutdatedValue("BankBIC");
                        break;
                    case "@banknametrue":
                        par.Value = item.HasPropertyOutdatedValue("BankName");
                        break;
                    case "@contractdatetrue":
                        par.Value = item.HasPropertyOutdatedValue("ContractDate");
                        break;
                    case "@contractnumtrue":
                        par.Value = item.HasPropertyOutdatedValue("ContractNumber");
                        break;
                    case "@coraccounttrue":
                        par.Value = item.HasPropertyOutdatedValue("CorrAccount");
                        break;
                    case "@dayentrytrue":
                        par.Value = item.HasPropertyOutdatedValue("DayEntry");
                        break;
                    case "@deliverytypeidtrue":
                        par.Value = item.HasPropertyOutdatedValue("DeliveryType");
                        break;
                    case "@edodtrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(CustomerLegal.EDOD));
                        break;
                    case "@edottrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(CustomerLegal.EDOT));
                        break;
                    case "@fullnametrue":
                        par.Value = item.HasPropertyOutdatedValue("FullName");
                        break;
                    case "@inntrue":
                        par.Value = item.HasPropertyOutdatedValue("INN");
                        break;
                    case "@managergroupidtrue":
                        par.Value = item.HasPropertyOutdatedValue("ManagerGroup");
                        break;
                    case "@nametrue":
                        par.Value= item.HasPropertyOutdatedValue("Name");
                        break;
                    case "@notespecialtrue":
                        par.Value = item.HasPropertyOutdatedValue("NoteSpecial");
                        break;
                    case "@payaccountidtrue":
                        par.Value = item.HasPropertyOutdatedValue("PayAccount");
                        break;
                    case "@paytypeidtrue":
                        par.Value = item.HasPropertyOutdatedValue("PayType");
                        break;
                    case "@recommendtrue":
                        par.Value = item.HasPropertyOutdatedValue("Recommend");
                        break;
                    case "@statetrue":
                        par.Value = item.HasPropertyOutdatedValue("State");
                        break;
                    case "@statustrue":
                        par.Value = item.HasPropertyOutdatedValue("Status");
                        break;
                }
            foreach(SqlParameter par in this.InsertUpdateParams)
                switch(par.ParameterName)
                {
                    case "@raccount":
                        par.Value = item.BankAccount;
                        break;
                    case "@bankbic":
                        par.Value = item.BankBIC;
                        break;
                    case "@bankname":
                        par.Value = item.BankName;
                        break;
                    case "@contractdate":
                        par.Value = item.ContractDate;
                        break;
                    case "@contractnum":
                        par.Value = item.ContractNumber;
                        break;
                    case "@coraccount":
                        par.Value = item.CorrAccount;
                        break;
                    case "@customerDayEntry":
                        par.Value = item.DayEntry;
                        break;
                    case "@deliverytypeID":
                        par.Value = item.DeliveryType;
                        break;
                    case "@edod":
                        par.Value = item.EDOD;
                        break;
                    case "@edot":
                        par.Value = item.EDOT;
                        break;
                    case "@customerFullName":
                        par.Value = item.FullName;
                        break;
                    case "@inn":
                        par.Value = item.INN;
                        break;
                    case "@managerGroupID":
                        par.Value = item.ManagerGroup?.Id;
                        break;
                    case "@customerName":
                        par.Value = item.Name;
                        break;
                    case "@customerNoteSpecial":
                        par.Value = item.NoteSpecial;
                        break;
                    case "@payaccountid":
                        par.Value = item.PayAccount;
                        break;
                    case "@paytypeID":
                        par.Value = item.PayType;
                        break;
                    case "@customerRecommend":
                        par.Value = item.Recommend;
                        break;
                    case "@customerState":
                        par.Value = item.State;
                        break;
                    case "@customerStatus":
                        par.Value = item.Status;
                        break;
                }
            return true;
        }
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
            bool success = base.GetModels(canceltasktoken);
            if (!myisrefreshcollection || canceltasktoken.IsCancellationRequested) return success;
            foreach (CustomerLegal item in this.Collection)
            {
                if (!item.AliasesIsNull)
                {
                    myadbm.Errors.Clear();
                    myadbm.ItemId = item.Id;
                    myadbm.Collection = item.Aliases;
                    if (!canceltasktoken.IsCancellationRequested) myadbm.Fill();
                    foreach (lib.DBMError err in myadbm.Errors) this.Errors.Add(err);
                }
                if (!item.CustomerAddressesIsNull)
                {
                    mycdbm.Errors.Clear();
                    mycdbm.ItemId = item.Id;
                    mycdbm.Collection = item.Addresses;
                    if (!canceltasktoken.IsCancellationRequested) mycdbm.Fill();
                    foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
                }
                if (!item.CustomerContactsIsNull)
                {
                    myccdbm.Errors.Clear();
                    myccdbm.ItemId = item.Id;
                    myccdbm.Collection = item.Contacts;
                    if (!canceltasktoken.IsCancellationRequested) myccdbm.Fill();
                    foreach (lib.DBMError err in myccdbm.Errors) this.Errors.Add(err);
                }
                if (!item.RecipientsIsNull)
                {
                    myrdbm.Errors.Clear();
                    myrdbm.CustomerId = item.Id;
                    myrdbm.Collection = item.Recipients;
                    if (!canceltasktoken.IsCancellationRequested) myrdbm.Fill();
                    foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
                }
                if (canceltasktoken.IsCancellationRequested) break;
            }
            return this.Errors.Count == 0;
		}
        private bool myisrefreshcollection;
		internal void RefreshCollection()
        {
            this.Errors.Clear();
            myisrefreshcollection = true;
            this.Fill();
            myisrefreshcollection = false;
        }
        //protected override void CancelLoad()
        //{
        //    myadbm.CancelingLoad = this.CancelingLoad;
        //    mycdbm.CancelingLoad = this.CancelingLoad;
        //    myccdbm.CancelingLoad = this.CancelingLoad;
        //    myrdbm.CancelingLoad = this.CancelingLoad;
        //}
    }

    internal class CustomerLegalStore : lib.DomainStorageLoad<CustomerRecord,CustomerLegal, CustomerLegalDBM>
    {
        public CustomerLegalStore(CustomerLegalDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(CustomerLegal olditem, CustomerLegal newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class CustomerLegalVM : lib.ViewModelErrorNotifyItem<CustomerLegal>
    {
        public CustomerLegalVM(CustomerLegal item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { "Name" });
            DeleteRefreshProperties.AddRange(new string[] { "Account", "BankAccount", "BankBIC", "BankName", "ContractDate", "ContractNumber", "CorrAccount", "Customer", "DayEntry", "DeliveryType", "FullName", "INN", "ManagerGroup", "Name", "NoteSpecial", "PayAccount", "PayType", "Recommend", "Status" });
            InitProperties();
        }
        public CustomerLegalVM():this(new CustomerLegal()) { }

        public int? Account
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Account.HasValue != value.HasValue || (value.HasValue && this.DomainObject.Account.Value!=value.Value)))
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
        private CustomerVM mycustomer;
        public CustomerVM Customer
        {
            set
            {
                if (!this.IsReadOnly && object.Equals(this.DomainObject.Customer, value.DomainObject))
                {
                    string name = "Customer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Customer);
                    ChangingDomainProperty = name; this.DomainObject.Customer = value.DomainObject;
                }
            }
            get { return this.IsEnabled ? mycustomer : null; }
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
        public bool? EDOD
        { 
            set { SetProperty<bool?>(this.DomainObject.EDOD == 1, (bool? isedo) => { if(isedo.HasValue) this.DomainObject.EDOD = (isedo.Value ? (byte)1 : (byte)0); }, value); }
            get { return GetProperty<bool?>(this.DomainObject.EDOD == 1, (bool?)null); }
        }
        public bool? EDOT
        {
            set { SetProperty<bool?>(this.DomainObject.EDOT == 1, (bool? isedo) => { if (isedo.HasValue) this.DomainObject.EDOT = (isedo.Value ? (byte)1 : (byte)0); }, value); }
            get { return GetProperty<bool?>(this.DomainObject.EDOT == 1, (bool?)null); }
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
        public string CustomerName
        {
            get
            {
                return this.Customer?.Name;
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
                case "Customer":
                    if (this.DomainObject.Customer != null & mycustomer == null)
                        mycustomer = new CustomerVM(this.DomainObject.Customer);
                    else if (this.DomainObject.Customer == null)
                        mycustomer = null;
                    break;
            }
        }
        protected override void InitProperties()
        {
            myname = this.DomainObject?.Name;
            if(this.DomainObject.Customer!=null)
                mycustomer = new CustomerVM(this.DomainObject.Customer);
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
                case nameof(EDOD):
                    this.DomainObject.EDOD = (bool)value?(byte)1:(byte)0;
                    break;
                case nameof(EDOT):
                    this.DomainObject.EDOT = (bool)value ? (byte)1 : (byte)0;
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
                    if(string.IsNullOrEmpty(this.Name))
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
            return mycustomer.DomainObject != this.DomainObject.Customer || myname!= this.DomainObject.Name;
        }
    }

    internal class CustomerLegalSynchronizer : lib.ModelViewCollectionsSynchronizer<CustomerLegal, CustomerLegalVM>
    {
        protected override CustomerLegal UnWrap(CustomerLegalVM wrap)
        {
            return wrap.DomainObject as CustomerLegal;
        }
        protected override CustomerLegalVM Wrap(CustomerLegal fill)
        {
            return new CustomerLegalVM(fill);
        }
    }

    public class CustomerLegalVMCommand : lib.ViewModelCommand<CustomerRecord,CustomerLegal, CustomerLegalVM, CustomerLegalDBM>
    {
        public CustomerLegalVMCommand(CustomerLegalVM vm, System.Windows.Data.ListCollectionView view) : base(vm, view)
        {
            mydbm = new Domain.CustomerLegalDBM();
            mydbm.ItemId = vm.Id;
            mycopycontact = new RelayCommand(CopyContactExec, CopyContactCanExec);
			mymanagergroups = new ListCollectionView(CustomBrokerWpf.References.ManagerGroups);
			mymanagergroups.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));

		}

		private System.Data.DataView mydeliverytypes;
        public System.Data.DataView DeliveryTypes
        {
            get
            {
                if (mydeliverytypes == null)
                {
                    mydeliverytypes = new System.Data.DataView(CustomBrokerWpf.References.ReferenceDS.DeliveryType, string.Empty, "deliverytypeName", System.Data.DataViewRowState.CurrentRows);
                }
                return mydeliverytypes;
            }
        }
        private ListCollectionView mymanagergroups;
        public ListCollectionView ManagerGroups
        {
            get
            {
                return mymanagergroups;
            }
        }
        private System.Data.DataView mypaymenttypes;
        public System.Data.DataView PaymentTypes
        {
            get
            {
                if (mypaymenttypes == null)
                {
                    mypaymenttypes = new System.Data.DataView(CustomBrokerWpf.References.ReferenceDS.tablePaymentType, string.Empty, "paytypeName", System.Data.DataViewRowState.CurrentRows);
                }
                return mypaymenttypes;
            }
        }
        private System.Data.DataView mylegalentities;
        public System.Data.DataView LegalEntities
        {
            get
            {
                if (mylegalentities == null)
                {
                    mylegalentities = new System.Data.DataView(CustomBrokerWpf.References.ReferenceDS.tableLegalEntity, string.Empty, string.Empty, System.Data.DataViewRowState.CurrentRows);
                }
                return mylegalentities;
            }
        }
        private ListCollectionView mystates;
        public ListCollectionView States
        {
            get
            {
                if (mystates == null)
                {
                    mystates = new ListCollectionView(CustomBrokerWpf.References.RowStates);
                }
                return mystates;
            }
        }

        private RelayCommand mycopycontact;
        public ICommand CopyContact
        {
            get { return mycopycontact; }
        }
        private void CopyContactExec(object parametr)
        {
            foreach (CustomerAddressVM item in myvm.Customer.Addresses)
            {
                CustomerAddress newitem = new CustomerAddress(lib.NewObjectId.NewId, lib.DomainObjectState.Added, item.AddressDescription, item.AddressTypeID.Value, myvm.DomainObject.Id, item.Locality, item.Town);
                myvm.Addresses.AddNewItem(new CustomerAddressVM(newitem));
                myvm.Addresses.CommitNew();
            }
            foreach (CustomerContactVM item in myvm.Customer.Contacts)
            {
                CustomerContact newitem = new CustomerContact(lib.NewObjectId.NewId, lib.DomainObjectState.Added, item.ContactType, myvm.DomainObject.Id, item.Name, item.SurName, item.ThirdName);
                foreach (ContactPointVM pitem in item.Points)
                {
                    ContactPoint newpitem = new ContactPoint(lib.NewObjectId.NewId, lib.DomainObjectState.Added, pitem.Name, pitem.Value);
                    newitem.Points.Add(newpitem);
                }
                myvm.Contacts.AddNewItem(new CustomerContactVM(newitem));
                myvm.Contacts.CommitNew();
            }

        }
        private bool CopyContactCanExec(object parametr)
        { return true; }


        protected override bool CanAddData(object parametr)
        {
            return myview!=null && myvm.Validate(false);
        }
        protected override void AddData(object parametr)
        {
            this.VModel=base.myview.AddNew() as CustomerLegalVM;
            myview.CommitNew();
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void RefreshData(object parametr)
        {
            mydbm.GetFirst();
            if(!myvm.DomainObject.AliasesIsNull)
            {
                AliasDBM dbm = new AliasDBM();
                dbm.ItemId = myvm.DomainObject.Id;
                dbm.Collection = myvm.DomainObject.Aliases;
                dbm.Fill();
            }
            if (!myvm.DomainObject.CustomerAddressesIsNull)
            {
                CustomerAddressDBM dbm = new CustomerAddressDBM();
                dbm.ItemId = myvm.DomainObject.Id;
                dbm.Collection = myvm.DomainObject.Addresses;
                dbm.Fill();
            }
            if (!myvm.DomainObject.CustomerContactsIsNull)
            {
                CustomerContactDBM dbm = new CustomerContactDBM();
                dbm.ItemId = myvm.DomainObject.Id;
                dbm.Collection = myvm.DomainObject.Contacts;
                dbm.Fill();
            }
            if (!myvm.DomainObject.RecipientsIsNull)
            {
                RecipientDBM dbm = new RecipientDBM();
                dbm.CustomerId = myvm.DomainObject.Id;
                dbm.Collection = myvm.DomainObject.Recipients;
                dbm.Fill();
            }
        }
    }

    public class CustomerLegalViewCommand : lib.ViewModelViewCommand
    {
        internal CustomerLegalViewCommand(int customerid)
        {
            mydbm = new CustomerLegalDBM();
            mydbm.CustomerId = customerid;
            mydbm.FillAsyncCompleted = () => { if (mydbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); };
            mydbm.Fill();
            mysync = new CustomerLegalSynchronizer();
            mysync.DomainCollection = mydbm.Collection;
            base.Collection = mysync.ViewModelCollection;
        }

        private new CustomerLegalDBM mydbm;
        private CustomerLegalSynchronizer mysync;
        internal int CustomerId
        {
            set { mydbm.CustomerId = value; }
            get { return mydbm.CustomerId; }
        }

        public override bool SaveDataChanges()
        {
            bool isSuccess = true, isvalid;
            if (myview != null)
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                err.AppendLine("Изменения не сохранены");
                mydbm.Errors.Clear();
                foreach (lib.ViewModelErrorNotifyItem item in myview.SourceCollection)
                {
                    isvalid = !(item.DomainState == lib.DomainObjectState.Added || item.DomainState == lib.DomainObjectState.Modified) || item.Validate(true);
                    if (!isvalid)
                        err.AppendLine(item.Errors);
                    isSuccess &= isvalid;
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
        protected override void OtherViewRefresh() {}
        protected override void RefreshData(object parametr)
        {
            mydbm.RefreshCollection();
        }
        protected override void RejectChanges(object parametr)
        {
            System.Collections.IList rejects;
            if (parametr is System.Collections.IList && (parametr as System.Collections.IList).Count > 0)
                rejects = parametr as System.Collections.IList;
            else
                rejects = mysync.ViewModelCollection;

            System.Collections.Generic.List<CustomerLegalVM> deleted = new System.Collections.Generic.List<CustomerLegalVM>();
            foreach (object item in rejects)
            {
                if (item is CustomerLegalVM)
                {
                    CustomerLegalVM ritem = item as CustomerLegalVM;
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
            foreach (CustomerLegalVM delitem in deleted)
            {
                mysync.DomainCollection.Remove(delitem.DomainObject);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
            }
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            myview.CurrentChanged += Items_CurrentChanged;
        }

        private void Items_CurrentChanged(object sender, EventArgs e)
        {
            if(myview.CurrentAddItem!=null)
                (myview.CurrentAddItem as CustomerLegalVM).DomainObject.Customer = CustomBrokerWpf.References.CustomerStore.GetItemLoad(this.CustomerId,out var errors);
        }
    }
}
