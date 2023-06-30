using System;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Recipient : lib.DomainBaseStamp
    {
        public Recipient(int id, long stamp, string updater, DateTime? updated, lib.DomainObjectState dstate
            ,Customer customer, string fullname, string inn, string name, string note, string passportn, string passports, string passportwho, DateTime? passportdate, byte state, string type
            ) :base(id, stamp, updated, updater, dstate)
        {
            mycustomer = customer;
            myfullname = fullname;
            myinn = inn;
            myname = name;
            mynote = note;
            mypassportn = passportn;
            mypassports = passports;
            mypassportwho = passportwho;
            mypassportdate = passportdate;
            mystate = state;
            mytype = type;
        }
        public Recipient():this(id: lib.NewObjectId.NewId, stamp: 0, updater: null, updated: null, dstate: lib.DomainObjectState.Added
            , customer: null, fullname:null, inn:null, name:null, note:null, passportn:null, passports:null, passportwho:null, passportdate:null, state:0, type: null)
        { }

        private lib.DomainBaseStamp mycustomer;
        public lib.DomainBaseStamp Customer
        {
            set
            {
                SetProperty<lib.DomainBaseStamp>(ref mycustomer, value);
            }
            get { return mycustomer; }
        }
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
        private string myname;
        public string Name
        {
            set
            {
                SetProperty<string>(ref myname, value);
            }
            get { return myname; }
        }
        private string mynote;
        public string Note
        {
            set { SetProperty<string>(ref mynote, value); }
            get { return mynote; }
        }
        private string mypassportn;
        public string PassportN
        {
            set { SetProperty<string>(ref mypassportn, value); }
            get { return mypassportn; }
        }
        private string mypassports;
        public string PassportS
        {
            set { SetProperty<string>(ref mypassports, value); }
            get { return mypassports; }
        }
        private string mypassportwho;
        public string PassportWho
        {
            set { SetProperty<string>(ref mypassportwho, value); }
            get { return mypassportwho; }
        }
        private DateTime? mypassportdate;
        public DateTime? PassportDate
        {
            set { SetProperty<DateTime?>(ref mypassportdate, value); }
            get { return mypassportdate; }
        }
        private byte mystate;
        public byte State
        {
            set
            {
                SetProperty<byte>(ref mystate, value);
            }
            get { return mystate; }
        }
        private string mytype;
        public string Type
        {
            set { SetProperty<string>(ref mytype, value); }
            get { return mytype; }
        }

        private System.Collections.ObjectModel.ObservableCollection<CustomerAddress> myaddresses;
        internal System.Collections.ObjectModel.ObservableCollection<CustomerAddress> Addresses
        {
            get
            {
                if (myaddresses == null)
                {
                    myaddresses = new System.Collections.ObjectModel.ObservableCollection<CustomerAddress>();
                    CustomerAddressDBM ldbm = new CustomerAddressDBM();
                    ldbm.SelectCommandText = "dbo.RecipientAddress_sp";
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
        internal bool AddressesIsNull
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
                    ldbm.SelectCommandText = "dbo.RecipientContact_sp";
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
        internal bool ContactsIsNull
        { get { return mycontacts == null; } }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Recipient newitem = (Recipient)sample;
            if (!this.HasPropertyOutdatedValue("FullName")) this.FullName = newitem.FullName;
            if (!this.HasPropertyOutdatedValue("INN")) this.INN = newitem.INN;
            if (!this.HasPropertyOutdatedValue("Name")) this.Name = newitem.Name;
            if (!this.HasPropertyOutdatedValue("Note")) this.Note = newitem.Note;
            if (!this.HasPropertyOutdatedValue("PassportN")) this.PassportN = newitem.PassportN;
            if (!this.HasPropertyOutdatedValue("PassportS")) this.PassportS = newitem.PassportS;
            if (!this.HasPropertyOutdatedValue("PassportWho")) this.PassportWho = newitem.PassportWho;
            if (!this.HasPropertyOutdatedValue("PassportDate")) this.PassportDate = newitem.PassportDate;
            if (!this.HasPropertyOutdatedValue("State")) this.State = newitem.State;
            if (!this.HasPropertyOutdatedValue("Type")) this.Type = newitem.Type;
        }
    }

    public class RecipientDBM : lib.DBManagerWhoWhen<Recipient, Recipient>
    {
        public RecipientDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.Recipitnt_sp";
            InsertCommandText = "dbo.RecipientAdd_sp";
            UpdateCommandText = "dbo.RecipientUpd_sp";
            DeleteCommandText = "dbo.RecipientDel_sp";

            SelectParams = new SqlParameter[]
            {
                        new SqlParameter("@param1", System.Data.SqlDbType.Int),
                        new SqlParameter("@customerId", System.Data.SqlDbType.Int)
            };
            myinsertparams = new SqlParameter[]
           {
                        myinsertparams[0]
                        ,new SqlParameter("@customerId", System.Data.SqlDbType.Int)
           };
            myinsertparams[0].ParameterName = "@recipientID";
            myupdateparams = new SqlParameter[]
            {
                        myupdateparams[0]
                        ,new SqlParameter("@nametrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@fullnametrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@typetrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@inntrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@passportstrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@passportntrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@passportwhotrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@passportdatetrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@notetrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@statetrue", System.Data.SqlDbType.Bit)
                        ,new SqlParameter("@old", 0)
            };
            myupdateparams[0].ParameterName = "@recipientID";
            myinsertupdateparams = new SqlParameter[]
           {
                        myinsertupdateparams[0],myinsertupdateparams[1],myinsertupdateparams[2]
                        ,new SqlParameter("@recipientName", System.Data.SqlDbType.NVarChar,30)
                        ,new SqlParameter("@recipientFullName", System.Data.SqlDbType.NVarChar,100)
                        ,new SqlParameter("@recipientType", System.Data.SqlDbType.NVarChar,3)
                        ,new SqlParameter("@recipientINN", System.Data.SqlDbType.NChar,12)
                        ,new SqlParameter("@recipientPassportS", System.Data.SqlDbType.NChar,4)
                        ,new SqlParameter("@recipientPassportN", System.Data.SqlDbType.NChar,6)
                        ,new SqlParameter("@recipientPassportWho", System.Data.SqlDbType.NVarChar,100)
                        ,new SqlParameter("@recipientPassportDate", System.Data.SqlDbType.DateTime)
                        ,new SqlParameter("@recipientNote", System.Data.SqlDbType.NVarChar,500)
                        ,new SqlParameter("@recipientState", System.Data.SqlDbType.Int)
           };

            mycdbm = new CustomerAddressDBM();
            mycdbm.SelectCommandText = "dbo.RecipientAddress_sp";
            mycdbm.InsertCommandText = "dbo.RecipientAddressAdd_sp";
            mycdbm.UpdateCommandText = "dbo.RecipientAddressUpd_sp";
            mycdbm.InsertUpdateParams[0].ParameterName = "@recipientID";
            mycdbm.Command = new SqlCommand();

            myccdbm = new CustomerContactDBM();
            myccdbm.SelectCommandText = "dbo.RecipientContact_sp";
            myccdbm.InsertCommandText = "dbo.RecipientContactAdd_sp";
            myccdbm.InsertParams[1].ParameterName = "@recipientID";
            myccdbm.Command = new SqlCommand();
        }

        private CustomerAddressDBM mycdbm;
        private CustomerContactDBM myccdbm;
        internal int CustomerId
        {
            set { base.SelectParams[1].Value = value; }
            get { return (int)base.SelectParams[1].Value; }
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override Recipient CreateRecord(SqlDataReader reader)
        {
            Recipient newitem = new Recipient(id: reader.GetInt32(0), stamp: reader.GetInt32(reader.GetOrdinal("stamp")), updater: reader.IsDBNull(reader.GetOrdinal("updtWho")) ? null : reader.GetString(reader.GetOrdinal("updtWho")), updated: reader.IsDBNull(reader.GetOrdinal("updtDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updtDate")), dstate: lib.DomainObjectState.Unchanged
                , customer: CustomBrokerWpf.References.CustomerStore.GetItemLoad(reader.GetInt32(1), out _)
                , fullname: reader.IsDBNull(3) ? null : reader.GetString(3)
                , inn: reader.IsDBNull(reader.GetOrdinal("recipientINN")) ? null : reader.GetString(reader.GetOrdinal("recipientINN"))
                , name: reader.IsDBNull(reader.GetOrdinal("recipientName")) ? null : reader.GetString(reader.GetOrdinal("recipientName"))
                , note: reader.IsDBNull(reader.GetOrdinal("recipientNote")) ? null : reader.GetString(reader.GetOrdinal("recipientNote"))
                , passportn: reader.IsDBNull(reader.GetOrdinal("recipientPassportN")) ? null : reader.GetString(reader.GetOrdinal("recipientPassportN"))
                , passports: reader.IsDBNull(reader.GetOrdinal("recipientPassportS")) ? null : reader.GetString(reader.GetOrdinal("recipientPassportS"))
                , passportwho: reader.IsDBNull(reader.GetOrdinal("recipientPassportWho")) ? null : reader.GetString(reader.GetOrdinal("recipientPassportWho"))
                , passportdate: reader.IsDBNull(reader.GetOrdinal("recipientPassportDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("recipientPassportDate"))
                , state: reader.GetByte(reader.GetOrdinal("recipientState"))
                , type: reader.IsDBNull(reader.GetOrdinal("recipientType")) ? null : reader.GetString(reader.GetOrdinal("recipientType"))
                );
            return newitem;//CustomBrokerWpf.References.RecipientStore.UpdateItem()
        }
		protected override Recipient CreateModel(Recipient record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			return record;
		}
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			base.TakeItem(this.CreateRecord(reader));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
		protected override void GetOutputSpecificParametersValue(Recipient item)
        {
        }
        protected override bool SaveChildObjects(Recipient item)
        {
            bool issuccess = true;
            if (!item.AddressesIsNull)
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
            if (!item.ContactsIsNull)
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
            return issuccess;
        }
        protected override bool SaveIncludedObject(Recipient item)
        {
            bool issuccess = true;
            if (item.Customer.DomainState == lib.DomainObjectState.Added)
            {
                if(item.Customer is Customer)
                {
                    CustomerDBM cdbm = new CustomerDBM();
                    cdbm.Command = new SqlCommand() { Connection = this.Command.Connection };
                    if (!cdbm.SaveItemChanches(item.Customer as Customer))
                    {
                        issuccess = false;
                        foreach (lib.DBMError err in cdbm.Errors) this.Errors.Add(err);
                    }
                }
                else
                {
                    CustomerLegalDBM cdbm = new CustomerLegalDBM();
                    cdbm.Command = new SqlCommand() { Connection = this.Command.Connection };
                    if (!cdbm.SaveItemChanches(item.Customer as CustomerLegal))
                    {
                        issuccess = false;
                        foreach (lib.DBMError err in cdbm.Errors) this.Errors.Add(err);
                    }

                }
            }
            return issuccess;
        }
        protected override bool SaveReferenceObjects()
        {
            mycdbm.Command.Connection = this.Command.Connection;
            myccdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetSpecificParametersValue(Recipient item)
        {
            if (item.Customer.DomainState == lib.DomainObjectState.Added)
                return false;
            myinsertparams[1].Value = item.Customer.Id;
            int i = 1;
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Name");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("FullName");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Type");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("INN");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("PassportS");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("PassportN");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("PassportWho");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("PassportDate");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Note");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("State");
            i = 3;
            myinsertupdateparams[i++].Value = item.Name;
            myinsertupdateparams[i++].Value = item.FullName;
            myinsertupdateparams[i++].Value = item.Type;
            myinsertupdateparams[i++].Value = item.INN;
            myinsertupdateparams[i++].Value = item.PassportS;
            myinsertupdateparams[i++].Value = item.PassportN;
            myinsertupdateparams[i++].Value = item.PassportWho;
            myinsertupdateparams[i++].Value = item.PassportDate;
            myinsertupdateparams[i++].Value = item.Note;
            myinsertupdateparams[i++].Value = item.State;
            return true;
        }

        internal void RefreshCollection()
        {
            this.Errors.Clear();
            this.Fill();
            foreach (Recipient item in this.Collection)
            {
                if (!item.AddressesIsNull)
                {
                    mycdbm.Errors.Clear();
                    mycdbm.ItemId = item.Id;
                    mycdbm.Collection = item.Addresses;
                    mycdbm.Fill();
                    foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
                }
                if (!item.ContactsIsNull)
                {
                    myccdbm.Errors.Clear();
                    myccdbm.ItemId = item.Id;
                    myccdbm.Collection = item.Contacts;
                    myccdbm.Fill();
                    foreach (lib.DBMError err in myccdbm.Errors) this.Errors.Add(err);
                }
            }
        }
    }

    internal class RecipientStore : lib.DomainStorageLoad<Recipient,Recipient, RecipientDBM>
    {
        public RecipientStore(RecipientDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(Recipient olditem, Recipient newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class RecipientVM : lib.ViewModelErrorNotifyItem<Recipient>
    {
        public RecipientVM(Recipient item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { "Name" });
            DeleteRefreshProperties.AddRange(new string[] { "Customer", "FullName", "INN", "Name", "Note", "PassportN", "PassportS", "PassportWho", "PassportDate", "Type" });
            InitProperties();
        }
        public RecipientVM() : this(new Recipient()) { }

        public new int? Id
        { get { return this.DomainObject.Id > 0 ? this.DomainObject.Id : (int?)null; } }
        private lib.ViewModelErrorNotifyItem mycustomer;
        public lib.ViewModelErrorNotifyItem Customer
        {
            set
            {
                if (!this.IsReadOnly && object.Equals(this.DomainObject.Customer, value.DomainObject))
                {
                    string name = "Customer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Customer);
                    ChangingDomainProperty = name; this.DomainObject.Customer = value.DomainObject as lib.DomainBaseStamp;
                }
            }
            get { return this.IsEnabled ? mycustomer : null; }
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
        public string Note
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Note, value)))
                {
                    string name = "Note";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Note);
                    ChangingDomainProperty = name; this.DomainObject.Note = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Note : null; }
        }
        public string PassportN
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.PassportN, value)))
                {
                    string name = "PassportN";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PassportN);
                    ChangingDomainProperty = name; this.DomainObject.PassportN = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PassportN : null; }
        }
        public string PassportS
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.PassportS, value)))
                {
                    string name = "PassportS";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PassportS);
                    ChangingDomainProperty = name; this.DomainObject.PassportS = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PassportS : null; }
        }
        public string PassportWho
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.PassportWho, value)))
                {
                    string name = "PassportWho";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PassportWho);
                    ChangingDomainProperty = name; this.DomainObject.PassportWho = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PassportWho : null; }
        }
        public DateTime? PassportDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.PassportDate.HasValue != value.HasValue || (value.HasValue && this.DomainObject.PassportDate.Value != value.Value)))
                {
                    string name = "PassportDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PassportDate);
                    ChangingDomainProperty = name; this.DomainObject.PassportDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PassportDate : null; }
        }
        public byte? State
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
            get { return this.IsEnabled ? this.DomainObject.State : (byte?)null; }
        }
        public string Type
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Type, value)))
                {
                    string name = "Type";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Type);
                    ChangingDomainProperty = name; this.DomainObject.Type = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Type : null; }
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

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Name":
                    myname = this.DomainObject.Name;
                    break;
                case "Customer":
                    if (this.DomainObject.Customer != null & mycustomer == null)
                        if(this.DomainObject.Customer is Customer)
                            mycustomer = new CustomerVM(this.DomainObject.Customer as Customer);
                        else
                            mycustomer = new CustomerLegalVM(this.DomainObject.Customer as CustomerLegal);
                    else if (this.DomainObject.Customer == null)
                        mycustomer = null;
                    break;
            }
        }
        protected override void InitProperties()
        {
            myname = this.DomainObject.Name;
            if (this.DomainObject.Customer != null)
            {
                if (this.DomainObject.Customer is Customer)
                    mycustomer = new CustomerVM(this.DomainObject.Customer as Customer); 
                else
                    mycustomer = new CustomerLegalVM(this.DomainObject.Customer as CustomerLegal);
            }
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "FullName":
                    this.DomainObject.FullName = (string)value;
                    break;
                case "INN":
                    this.DomainObject.INN = (string)value;
                    break;
                case "Name":
                    if (myname != this.DomainObject.Name)
                        myname = this.DomainObject.Name;
                    else
                        this.Name = (string)value;
                    break;
                case "Note":
                    this.DomainObject.Note = (string)value;
                    break;
                case "PassportN":
                    this.DomainObject.PassportN = (string)value;
                    break;
                case "PassportS":
                    this.DomainObject.PassportS = (string)value;
                    break;
                case "PassportWho":
                    this.DomainObject.PassportWho = (string)value;
                    break;
                case "PassportDate":
                    this.DomainObject.PassportDate = (DateTime?)value;
                    break;
                case "Type":
                    this.DomainObject.Type = (string)value;
                    break;
                case "DependentNew":
                    int i = 0;
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
            return myname!= this.DomainObject.Name;
        }
    }

    internal class RecipientSynchronizer : lib.ModelViewCollectionsSynchronizer<Recipient, RecipientVM>
    {
        protected override Recipient UnWrap(RecipientVM wrap)
        {
            return wrap.DomainObject as Recipient;
        }
        protected override RecipientVM Wrap(Recipient fill)
        {
            return new RecipientVM(fill);
        }
    }

    public class RecipientVMCommand : lib.ViewModelCommand<Recipient,Recipient, RecipientVM, RecipientDBM>
    {
        public RecipientVMCommand(RecipientVM vm, System.Windows.Data.ListCollectionView view) : base(vm, view)
        {
            mydbm = new Domain.RecipientDBM();
            mydbm.ItemId = vm.Id;
            mycopycontact = new RelayCommand(CopyContactExec, CopyContactCanExec);
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
        private System.Data.DataView mymanagergroups;
        public System.Data.DataView ManagerGroups
        {
            get
            {
                if (mymanagergroups == null)
                {
                    mymanagergroups = new System.Data.DataView(CustomBrokerWpf.References.ReferenceDS.tableManagerGroup, string.Empty, "managergroupName", System.Data.DataViewRowState.CurrentRows);
                }
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
            //foreach (CustomerAddressVM item in myvm.Customer.Addresses)
            //{
            //    CustomerAddress newitem = new CustomerAddress(lib.NewObjectId.NewId, lib.DomainObjectState.Added, item.AddressDescription, item.AddressTypeID.Value, myvm.DomainObject.Id, item.Locality, item.Town);
            //    myvm.Addresses.AddNewItem(new CustomerAddressVM(newitem));
            //    myvm.Addresses.CommitNew();
            //}
            //foreach (CustomerContactVM item in myvm.Customer.Contacts)
            //{
            //    CustomerContact newitem = new CustomerContact(lib.NewObjectId.NewId, lib.DomainObjectState.Added, item.ContactType, myvm.DomainObject.Id, item.Name, item.SurName, item.ThirdName);
            //    foreach (ContactPointVM pitem in item.Points)
            //    {
            //        ContactPoint newpitem = new ContactPoint(lib.NewObjectId.NewId, lib.DomainObjectState.Added, pitem.Name, pitem.Value);
            //        newitem.Points.Add(newpitem);
            //    }
            //    myvm.Contacts.AddNewItem(new CustomerContactVM(newitem));
            //    myvm.Contacts.CommitNew();
            //}
        }
        private bool CopyContactCanExec(object parametr)
        { return true; }

        protected override bool CanAddData(object parametr)
        {
            return myview != null && myvm.Validate(false);
        }
        protected override void AddData(object parametr)
        {
            this.VModel = base.myview.AddNew() as RecipientVM;
            myview.CommitNew();
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void RefreshData(object parametr)
        {
            mydbm.GetFirst();
            if (!myvm.DomainObject.AddressesIsNull)
            {
                CustomerAddressDBM dbm = new CustomerAddressDBM();
                dbm.ItemId = myvm.DomainObject.Id;
                dbm.Collection = myvm.DomainObject.Addresses;
                dbm.Fill();
            }
            if (!myvm.DomainObject.ContactsIsNull)
            {
                CustomerContactDBM dbm = new CustomerContactDBM();
                dbm.ItemId = myvm.DomainObject.Id;
                dbm.Collection = myvm.DomainObject.Contacts;
                dbm.Fill();
            }
        }
    }

    public class RecipientCurrentCommand : lib.ViewModelCurrentItemCommand<RecipientVM>
    {
        internal RecipientCurrentCommand(CustomerVM customer, CustomerLegalVM customerlegal)
        {
            mydbm = new RecipientDBM();
            mysync = new RecipientSynchronizer();
            //mydbm.Fill();
            //if (mydbm.Errors.Count > 0)
            //    this.OpenPopup("Загрузка данных\n" + mydbm.ErrorMessage, true);
            //else
            //{
            mycustomer = customer;
            mycustomerlegal = customerlegal;
            (mydbm as RecipientDBM).Collection = customer == null ? customerlegal.DomainObject.Recipients : customer.DomainObject.Recipients;
            mysync.DomainCollection = (mydbm as RecipientDBM).Collection;//
            base.Collection = mysync.ViewModelCollection;
            //}
            myfastfilter = new RelayCommand(FastFilterExec, FastFilterCanExec);
            base.DeleteQuestionHeader = "Удалить получателя?";
        }

        CustomerVM mycustomer;
        CustomerLegalVM mycustomerlegal;
        RecipientSynchronizer mysync;

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
        protected override RecipientVM CreateCurrentViewItem(lib.DomainBaseNotifyChanged domainobject)
        {
            throw new NotImplementedException();
        }
        protected override void OnCurrentItemChanged()
        {
        }
        protected override void OtherViewRefresh()
        {
        }
        protected override void RefreshData(object parametr)
        {
            Recipient current = this.CurrentItem?.DomainObject;
            (mydbm as RecipientDBM).CustomerId = mycustomer == null ? mycustomerlegal.Id : mycustomer.Id;
            (mydbm as RecipientDBM).RefreshCollection();
            if (mydbm.Errors.Count > 0)
                this.OpenPopup("Обновление данных\n" + mydbm.ErrorMessage, true);
            if (current != null)
            {
                foreach (RecipientVM item in myview)
                    if (current.Id == item.DomainObject.Id)
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

            System.Collections.Generic.List<RecipientVM> deleted = new System.Collections.Generic.List<RecipientVM>();
            foreach (object item in rejects)
            {
                if (item is RecipientVM)
                {
                    RecipientVM ritem = item as RecipientVM;
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
            foreach (RecipientVM delitem in deleted)
            {
                mysync.DomainCollection.Remove(delitem.DomainObject);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
            }
        }
    }
}
