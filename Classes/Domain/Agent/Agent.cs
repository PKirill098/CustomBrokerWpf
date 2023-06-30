using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Agent : lib.DomainBaseStamp
    {
        public Agent(int id, long stamp, lib.DomainObjectState domainstate
            ,string creater, DateTime dateentry,string fullname,string name,string recommend,byte state
            ) : base(id, stamp, null, null, domainstate)
        {
            mycreater = creater;
            mydateentry = dateentry;
            myfullname = fullname;
            myname = name;
            myrecommend = recommend;
            mystate = state;
            mycontractlock = new object();
            mycontractlocker = 0;
        }
        public Agent(string fullname, string name) : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added
            ,string.Empty, DateTime.Today, fullname, name, null, 0)
        { }
        public Agent():this( null, null) { }

        private string mycreater;
        public string Creater
        { get { return mycreater; } }
        private DateTime mydateentry;
        public DateTime DayEntry
        {
            set { SetProperty<DateTime>(ref mydateentry, value); }
            get { return mydateentry; }
        }
        private string myfullname;
        public string FullName
        {
            set { SetProperty<string>(ref myfullname, value); }
            get { return myfullname; }
        }
        private string myname;
        public string Name
        {
            set { SetProperty<string>(ref myname, value); }
            get { return myname; }
        }
        private string myrecommend;
        public string Recommend
        {
            set { SetProperty<string>(ref myrecommend, value); }
            get { return myrecommend; }
        }
        private byte mystate;
        public byte State
        {
            set { SetProperty<byte>(ref mystate, value); }
            get { return mystate; }
        }

        private ObservableCollection<AgentAlias> myaliases;
        internal ObservableCollection<AgentAlias> Aliases
        {
            set
            {
                SetProperty<ObservableCollection<AgentAlias>>(ref myaliases,value);
            }
            get
            {
                if (myaliases == null)
                {
                    myaliases = new ObservableCollection<AgentAlias>();
                    myaliases.CollectionChanged += Aliases_CollectionChanged;
                }
                return myaliases;
            }
        }
        private void Aliases_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (AgentAlias item in e.NewItems)
                   if(item.Agent != this) item.Agent = this;
        }
        private ObservableCollection<AgentAddress> myaddresses;
        internal ObservableCollection<AgentAddress> Addresses
        {
            get
            {
                if (myaddresses == null)
                {
                    myaddresses = new System.Collections.ObjectModel.ObservableCollection<AgentAddress>();
                    AgentAddressDBM ldbm = new AgentAddressDBM();
                    ldbm.Agent = this;
                    ldbm.Collection = myaddresses;
                    ldbm.Fill();
                    myaddresses.CollectionChanged += AgentAddresses_CollectionChanged;
                }
                return myaddresses;
            }
        }
        private void AgentAddresses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (AgentAddress item in e.NewItems)
                    item.Agent = this;
        }
        internal bool AgentAddressesIsNull
        { get { return myaddresses == null; } }
        private ObservableCollection<AgentBrand> mybrands;
        internal ObservableCollection<AgentBrand> Brands
        {
            get
            {
                if (mybrands == null)
                {
                    mybrands = new System.Collections.ObjectModel.ObservableCollection<AgentBrand>();
                    AgentBrandDBM dbm = new AgentBrandDBM();
                    dbm.Agent = this;
                    dbm.Collection = mybrands;
                    dbm.Fill();
                    mybrands.CollectionChanged += Brands_CollectionChanged;
                }
                return mybrands;
            }
        }
        private void Brands_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (AgentBrand item in e.NewItems)
                    item.Agent = this;
        }
        internal bool AgentBrandsIsNull
        { get { return mybrands == null; } }
        private System.Collections.ObjectModel.ObservableCollection<AgentContact> mycontacts;
        internal System.Collections.ObjectModel.ObservableCollection<AgentContact> Contacts
        {
            get
            {
                if (mycontacts == null)
                {
                    mycontacts = new System.Collections.ObjectModel.ObservableCollection<AgentContact>();
                    AgentContactDBM ldbm = new AgentContactDBM();
                    ldbm.Agent = this;
                    ldbm.Collection = mycontacts;
                    ldbm.Fill();
                    mycontacts.CollectionChanged += Contact_CollectionChanged;
                }
                return mycontacts;
            }
        }
        private void Contact_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (AgentContact item in e.NewItems)
                    item.Agent = this;
        }
        internal bool ContactsIsNull
        { get { return mycontacts == null; } }
        private int mycontractlocker;
        private object mycontractlock;
        private ObservableCollection<Contract> mycontracts;
        internal ObservableCollection<Contract> Contracts
        {
            get
            {
                lock (mycontractlock)
                    if (mycontracts == null)
                    {
                        App.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => { mycontracts = new ObservableCollection<Contract>(); }));
                        mycontractlocker = System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId;
                    }
                if (mycontractlocker > 0 && mycontractlocker == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
                {
                    ContractDBM dbm=null;
                    App.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => { dbm = new ContractDBM(); }));
                    dbm.Agent = this;
                    dbm.Collection = mycontracts;
                    dbm.Fill();
                    mycontracts.CollectionChanged += Contracts_CollectionChanged;
                    mycontractlocker = 0;
                }
                return mycontracts;
            }
        }
        private void Contracts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (Contract item in e.NewItems)
                    item.Agent = this;
        }
        internal bool ContractsIsNull
        { get { return mycontracts == null; } }

        protected override void RejectProperty(string property, object value)
        {
            switch(property)
            {
                case nameof(this.DayEntry):
                    this.DayEntry = (DateTime)this.GetPropertyOutdatedValue(nameof(this.DayEntry));
                    break;
                case nameof(this.FullName):
                    this.FullName = (string)this.GetPropertyOutdatedValue(nameof(this.FullName));
                    break;
                case nameof(this.Name):
                    this.Name = (string)this.GetPropertyOutdatedValue(nameof(this.Name));
                    break;
                case nameof(this.Recommend):
                    this.Recommend = (string)this.GetPropertyOutdatedValue(nameof(this.Recommend));
                    break;
                case nameof(this.State):
                    this.State = (byte)this.GetPropertyOutdatedValue(nameof(this.State));
                    break;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Agent templ = sample as Agent;
            this.DayEntry = templ.DayEntry;
            this.FullName = templ.FullName;
            this.Name = templ.Name;
            this.Recommend = templ.Recommend;
            this.State = templ.State;
        }
    }

    public struct AgentRecord
    {
        internal int id;
        internal long stamp;
		internal string mycreater;
		internal DateTime mydateentry;
		internal string myfullname;
		internal string myname;
		internal string myrecommend;
		internal byte mystate;
	}

	internal class AgentStore : lib.DomainStorageLoad<AgentRecord,Agent, AgentDBM>
    {
        public AgentStore(AgentDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(Agent olditem, Agent newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class AgentDBM : lib.DBManagerStamp<AgentRecord,Agent>
    {
        public AgentDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.Agent_sp";
            InsertCommandText = "dbo.AgentAdd_sp";
            UpdateCommandText = "dbo.AgentUpd_sp";
            DeleteCommandText = "dbo.AgentDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@id", System.Data.SqlDbType.Int),
                new SqlParameter("@agentname", System.Data.SqlDbType.NVarChar,100),
                new SqlParameter("@filtrid", System.Data.SqlDbType.Int)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0]//,myinsertupdateparams[1],myinsertupdateparams[2]
                ,new SqlParameter("@name", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@fullname", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@dayentry", System.Data.SqlDbType.DateTime)
                ,new SqlParameter("@recommend", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@state", System.Data.SqlDbType.TinyInt)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@nameupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@fullnameupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@dayentryupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@recommendupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@stateupd", System.Data.SqlDbType.Bit)
           };
        }

        private AgentAliasDBM myadbm;
        internal AgentAliasDBM AliasDBM
        { set { myadbm = value; } }
        private AgentAddressDBM myaddbm;
        internal AgentAddressDBM AddressDBM
        { set { myaddbm = value; } }
        private AgentBrandDBM mybrdbm;
        internal AgentBrandDBM BrandDBM
        { set { mybrdbm = value; } }
        private AgentContactDBM mycdbm;
        internal AgentContactDBM ContactDBM
        { set { mycdbm = value; } }
        private ContractDBM mycntrdbm;
        internal ContractDBM ContractDBM
        { set { mycntrdbm = value; } }
        
        private string myname;
        public string Name
        { set { myname = value; } }
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { set { myfilter = value; } get { return myfilter; } }

        protected override AgentRecord CreateRecord(SqlDataReader reader)
        {
            return new AgentRecord() {
                id=reader.GetInt32(0)
                , stamp=reader.GetInt64(this.Fields["stamp"])
                , mycreater = reader.IsDBNull(this.Fields["creater"]) ? null : reader.GetString(this.Fields["creater"])
                , mydateentry = reader.GetDateTime(this.Fields["agentDayEntry"])
                , myfullname = reader.IsDBNull(this.Fields["agentFullName"]) ? null : reader.GetString(this.Fields["agentFullName"])
                , myname = reader.GetString(this.Fields["agentName"])
                , myrecommend = reader.IsDBNull(this.Fields["agentRecommend"]) ? null : reader.GetString(this.Fields["agentRecommend"])
                , mystate = reader.GetByte(this.Fields["agentState"])
                };
        }
		protected override Agent CreateModel(AgentRecord record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			Agent agent = null;
			if (this.FillType == lib.FillType.PrefExist)
				agent = CustomBrokerWpf.References.AgentStore.GetItem(record.id);
			if (agent == null)
			{
				agent = new Agent(record.id, record.stamp, lib.DomainObjectState.Unchanged
				  , record.mycreater
				  , record.mydateentry
				  , record.myfullname
				  , record.myname
				  , record.myrecommend
				  , record.mystate
				  );
				agent = CustomBrokerWpf.References.AgentStore.UpdateItem(agent);
			}
			if ((agent.Aliases.Count == 0 | this.FillType == lib.FillType.Refresh) & myadbm != null)
			{
				myadbm.Errors.Clear();
				myadbm.Command.Connection = addcon;
				myadbm.Agent = agent;
				myadbm.Collection = agent.Aliases;
				myadbm.Fill();
				foreach (lib.DBMError err in myadbm.Errors) this.Errors.Add(err);
			}
			if (this.FillType == lib.FillType.Refresh)
			{
				if (!agent.AgentAddressesIsNull & myaddbm != null)
				{
					myaddbm.Errors.Clear();
					myaddbm.Command.Connection = addcon;
					myaddbm.Agent = agent;
					myaddbm.Collection = agent.Addresses;
					myaddbm.Fill();
					foreach (lib.DBMError err in myaddbm.Errors) this.Errors.Add(err);
				}
				if (!agent.AgentBrandsIsNull & mybrdbm != null)
				{
					mybrdbm.Errors.Clear();
					mybrdbm.Command.Connection = addcon;
					mybrdbm.Agent = agent;
					mybrdbm.Collection = agent.Brands;
					mybrdbm.Fill();
					foreach (lib.DBMError err in mybrdbm.Errors) this.Errors.Add(err);
				}
				if (!agent.ContactsIsNull & mycdbm != null)
				{
					mycdbm.Errors.Clear();
					mycdbm.Command.Connection = addcon;
					mycdbm.Agent = agent;
					mycdbm.Collection = agent.Contacts;
					mycdbm.Fill();
					foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
				}
				if (!agent.ContractsIsNull & mycntrdbm != null)
				{
					mycntrdbm.Errors.Clear();
					mycntrdbm.Command.Connection = addcon;
					mycntrdbm.Agent = agent;
					mycntrdbm.Collection = agent.Contracts;
					mycntrdbm.Fill();
					foreach (lib.DBMError err in mycntrdbm.Errors) this.Errors.Add(err);
				}
			}
			return agent;
		}
		protected override void GetOutputSpecificParametersValue(Agent item)
        {
        }
        protected override bool SaveChildObjects(Agent item)
        {
            bool success = true;
            if (myadbm != null)
            {
                myadbm.Errors.Clear();
                myadbm.Collection = item.Aliases;
                if (!myadbm.SaveCollectionChanches())
                {
                    success = false;
                    foreach (lib.DBMError err in myadbm.Errors) this.Errors.Add(err);
                }
            }
            if (myaddbm != null)
            {
                myaddbm.Errors.Clear();
                myaddbm.Collection = item.Addresses;
                if (!myaddbm.SaveCollectionChanches())
                {
                    success = false;
                    foreach (lib.DBMError err in myaddbm.Errors) this.Errors.Add(err);
                }
            }
            if (mybrdbm != null)
            {
                mybrdbm.Errors.Clear();
                mybrdbm.Collection = item.Brands;
                if (!mybrdbm.SaveCollectionChanches())
                {
                    success = false;
                    foreach (lib.DBMError err in mybrdbm.Errors) this.Errors.Add(err);
                }
            }
            if (mycdbm != null)
            {
                mycdbm.Errors.Clear();
                mycdbm.Collection = item.Contacts;
                if (!mycdbm.SaveCollectionChanches())
                {
                    success = false;
                    foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
                }
            }
            if(mycntrdbm != null)
            {
                mycntrdbm.Errors.Clear();
                mycntrdbm.Collection = item.Contracts;
                if (!mycntrdbm.SaveCollectionChanches())
                {
                    success = false;
                    foreach (lib.DBMError err in mycntrdbm.Errors) this.Errors.Add(err);
                }
            }
            return success;
        }
        protected override bool SaveIncludedObject(Agent item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            if (myadbm != null)
                myadbm.Command.Connection = this.Command.Connection;
            if (myaddbm != null)
                myaddbm.Command.Connection = this.Command.Connection;
            if (mybrdbm != null)
                mybrdbm.Command.Connection = this.Command.Connection;
            if (mycdbm != null)
                mycdbm.Command.Connection = this.Command.Connection;
            if (mycntrdbm != null)
                mycntrdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[1].Value = myname;
            this.SelectParams[2].Value = myfilter?.FilterWhereId;
            this.NeedAddConnection = myadbm != null | myaddbm != null | mybrdbm != null | mycdbm != null | mycntrdbm != null;
        }
        protected override bool SetSpecificParametersValue(Agent item)
        {
            foreach (SqlParameter par in this.InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@name":
                        par.Value = item.Name;
                        break;
                    case "@fullname":
                        par.Value = item.FullName;
                        break;
                    case "@dayentry":
                        par.Value = item.DayEntry;
                        break;
                    case "@recommend":
                        par.Value = item.Recommend;
                        break;
                    case "@state":
                        par.Value = item.State;
                        break;
                }
            foreach(SqlParameter par in this.UpdateParams)
                switch (par.ParameterName)
                {
                    case "@nameupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Agent.Name));
                        break;
                    case "@fullnameupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Agent.FullName));
                        break;
                    case "@dayentryupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Agent.DayEntry));
                        break;
                    case "@recommendupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Agent.Recommend));
                        break;
                    case "@stateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Agent.State));
                        break;
                }
            return true;
        }
    }

    public class AgentVM : lib.ViewModelErrorNotifyItem<Agent>
    {
        public AgentVM(Agent model):base(model)
        {
            DeleteRefreshProperties.AddRange(new string[] { nameof(AgentVM.DayEntry), nameof(AgentVM.FullName), nameof(AgentVM.Name), nameof(AgentVM.Recommend),nameof(AgentVM.State) });
            ValidetingProperties.AddRange(new string[] { nameof(this.Name), "DependancyObject"});
            InitProperties();
        }
        public AgentVM():this(new Agent()) { }

        public string Creater
        { get { return this.IsEnabled ? this.DomainObject.Creater : null; } }
        public DateTime? DayEntry
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(this.DomainObject.DayEntry, value.Value)))
                {
                    string name = nameof(this.DayEntry);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DayEntry);
                    ChangingDomainProperty = name; this.DomainObject.DayEntry = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DayEntry : (DateTime?)null; }
        }
        public string FullName
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.FullName, value)))
                {
                    string name = nameof(this.FullName);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FullName);
                    ChangingDomainProperty = name; this.DomainObject.FullName = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.FullName : null; }
        }
        public string Name
        {
            set
            {
                if (!string.IsNullOrEmpty(value) && !(this.IsReadOnly || string.Equals(this.DomainObject.Name, value)))
                {
                    string name = nameof(this.Name);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Name);
                    ChangingDomainProperty = name; this.DomainObject.Name = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Name : null; }
        }
        public string Recommend
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Recommend, value)))
                {
                    string name = nameof(this.Recommend);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Recommend);
                    ChangingDomainProperty = name; this.DomainObject.Recommend = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Recommend : null; }
        }
        public byte? State
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || short.Equals(this.DomainObject.State, value.Value)))
                {
                    string name = nameof(this.State);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.State);
                    ChangingDomainProperty = name; this.DomainObject.State = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.State : (byte?)null; }
        }
        public lib.ReferenceSimpleItem StateReferenceSimple
        {
            get { return this.IsEnabled ? CustomBrokerWpf.References.CustomerRowStates.FindFirstItem("Id", (int)this.DomainObject.State) : null; }
        }

        private AgentAliasSynchronizer myaliasessync;
        private ListCollectionView myaliases;
        public ListCollectionView Aliases
        {
            get
            {
                if(myaliases==null)
                {
                    if(myaliasessync==null)
                    {
                        myaliasessync = new AgentAliasSynchronizer();
                        myaliasessync.DomainCollection = this.DomainObject.Aliases;
                    }
                    myaliases = new ListCollectionView(myaliasessync.ViewModelCollection);
                    myaliases.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    myaliases.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(AgentAliasVM.Alias), System.ComponentModel.ListSortDirection.Ascending));
                }
                return myaliases;
            }
        }
        private AgentAddressSynchronizer myadrsync;
        private ListCollectionView myaddresses;
        public ListCollectionView Addresses
        {
            get
            {
                if (myaddresses == null)
                {
                    if (myadrsync == null)
                    {
                        myadrsync = new AgentAddressSynchronizer();
                        myadrsync.DomainCollection = this.DomainObject.Addresses;
                    }
                    myaddresses = new ListCollectionView(myadrsync.ViewModelCollection);
                    myaddresses.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                }
                return myaddresses;
            }
        }
        private AgentBrandSynchronizer mybrsync;
        private ListCollectionView mybrands;
        public ListCollectionView Brands
        {
            get
            {
                if (mybrands == null)
                {
                    if (mybrsync == null)
                    {
                        mybrsync = new AgentBrandSynchronizer();
                        mybrsync.DomainCollection = this.DomainObject.Brands;
                    }
                    mybrands = new ListCollectionView(mybrsync.ViewModelCollection);
                    mybrands.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                }
                return mybrands;
            }
        }
        private AgentContactSynchronizer mycntsync;
        private ListCollectionView mycontacts;
        public ListCollectionView Contacts
        {
            get
            {
                if (mycontacts == null)
                {
                    if (mycntsync == null)
                    {
                        mycntsync = new AgentContactSynchronizer();
                        mycntsync.DomainCollection = this.DomainObject.Contacts;
                    }
                    mycontacts = new ListCollectionView(mycntsync.ViewModelCollection);
                    mycontacts.Filter = lib.ViewModelViewCommand.ViewFilterDefault;

                }
                return mycontacts;
            }
        }
        private ContractSynchronizer mycntrsync;
        private ListCollectionView mycontracts;
        public ListCollectionView Contracts
        {
            get 
            {
                if(mycontracts ==null)
                {
                    if(mycntrsync == null)
                    {
                        mycntrsync = new ContractSynchronizer();
                        mycntrsync.DomainCollection = this.DomainObject.Contracts;
                    }
                    mycontracts = new ListCollectionView(mycntrsync.ViewModelCollection);
                    mycontracts.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    mycontracts.SortDescriptions.Add(new System.ComponentModel.SortDescription("Expired", System.ComponentModel.ListSortDirection.Descending));
                    mycontracts.SortDescriptions.Add(new System.ComponentModel.SortDescription("ExpiryDate", System.ComponentModel.ListSortDirection.Descending));
                }
                return mycontracts;
            }
        }

        protected override bool DirtyCheckProperty()
        {
            return string.IsNullOrEmpty(this.DomainObject.Name);
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch(property)
            {
                case nameof(Agent.State):
                    this.PropertyChangedNotification(nameof(this.StateReferenceSimple));
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
                case nameof(this.DayEntry):
                    this.DomainObject.DayEntry = (DateTime)value;
                    break;
                case nameof(this.FullName):
                    this.DomainObject.FullName = (string)value;
                    break;
                case nameof(this.Name):
                    this.DomainObject.Name = (string)value;
                    break;
                case nameof(this.Recommend):
                    this.DomainObject.Recommend = (string)value;
                    break;
                case nameof(this.State):
                    this.DomainObject.State = (byte)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    AgentAliasVM[] removed = new AgentAliasVM[this.DomainObject.Aliases.Count];
                    foreach (AgentAliasVM litem in this.Aliases)
                    {
                        if (litem.DomainState == lib.DomainObjectState.Added)
                        {
                            removed[i] = litem;
                            i++;
                        }
                        else
                            litem.RejectChanges();
                    }
                    foreach (AgentAliasVM litem in removed)
                        if (litem != null) this.Aliases.Remove(litem);
                    i = 0;
                    if (!this.DomainObject.AgentAddressesIsNull)
                    {
                        AgentAddressVM[] adremoved = new AgentAddressVM[this.DomainObject.Addresses.Count];
                        foreach (AgentAddressVM litem in this.Addresses)
                        {
                            if (litem.DomainState == lib.DomainObjectState.Added)
                            {
                                adremoved[i] = litem;
                                i++;
                            }
                            else
                                litem.RejectChanges();
                        }
                        foreach (AgentAddressVM litem in adremoved)
                            if (litem != null) this.Addresses.Remove(litem);
                    }
                    i = 0;
                    if (!this.DomainObject.AgentBrandsIsNull)
                    {
                        AgentBrandVM[] bremoved = new AgentBrandVM[this.DomainObject.Brands.Count];
                        foreach (AgentBrandVM litem in this.Brands)
                        {
                            if (litem.DomainState == lib.DomainObjectState.Added)
                            {
                                bremoved[i] = litem;
                                i++;
                            }
                            else
                                litem.RejectChanges();
                        }
                        foreach (AgentBrandVM litem in bremoved)
                            if (litem != null) this.Brands.Remove(litem);
                    }
                    i = 0;
                    if (!this.DomainObject.ContactsIsNull)
                    {
                        AgentContactVM[] cremoved = new AgentContactVM[this.DomainObject.Contacts.Count];
                        foreach (AgentContactVM litem in this.Contacts)
                        {
                            if (litem.DomainState == lib.DomainObjectState.Added)
                            {
                                cremoved[i] = litem;
                                i++;
                            }
                            else
                                litem.RejectChanges();
                        }
                        foreach (AgentContactVM litem in cremoved)
                            if (litem != null) this.Contacts.Remove(litem);
                    }
                    i = 0;
                    if (!this.DomainObject.ContractsIsNull)
                    {
                        ContractVM[] cntremoved = new ContractVM[this.DomainObject.Contracts.Count];
                        foreach(ContractVM item in this.Contracts)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                cntremoved[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach(ContractVM item in cntremoved)
                            if (item != null) this.Contracts.Remove(item);
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
                case nameof(this.Name):
                    if (string.IsNullOrEmpty(this.DomainObject.Name))
                    {
                        errmsg = "Имя агента не может быть пустым!";
                        isvalid = false;
                    }
                    break;
                case "DependancyObject":
                    System.Text.StringBuilder err = new System.Text.StringBuilder();
                    if (myaliases != null)
                        foreach (AgentAliasVM item in myaliases.OfType<AgentAliasVM>())
                            if (!item.Validate(true))
                                err.AppendLine(item.Errors);
                    if (myaddresses != null)
                        foreach (AgentAddressVM item in myaddresses.OfType<AgentAddressVM>())
                            if (!item.Validate(true))
                                err.AppendLine(item.Errors);
                    if (mybrands != null)
                        foreach (AgentBrandVM item in mybrands.OfType<AgentBrandVM>())
                            if (!item.Validate(true))
                                err.AppendLine(item.Errors);
                    if (mycontacts != null)
                        foreach (AgentContactVM item in mycontacts.OfType<AgentContactVM>())
                            if (!item.Validate(true))
                                err.AppendLine(item.Errors);
                    if (mycontracts != null)
                        foreach (ContractVM item in mycontracts.OfType<ContractVM>())
                            if (!item.Validate(true))
                                err.AppendLine(item.Errors);
                    if (err.Length > 0)
                    {
                        errmsg = err.ToString();
                        isvalid = false;
                    }
                    break;
            }
            if (isvalid) ClearErrorMessageForProperty(propertyname);
            else if (inform) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    internal class AgentSynchronizer : lib.ModelViewCollectionsSynchronizer<Agent, AgentVM>
    {
        protected override Agent UnWrap(AgentVM wrap)
        {
            return wrap.DomainObject as Agent;
        }
        protected override AgentVM Wrap(Agent fill)
        {
            return new AgentVM(fill);
        }
    }

    public class AgentCommand : lib.ViewModelCommand<AgentRecord, Agent, AgentVM, AgentDBM>
    {
        public AgentCommand(AgentVM vm, ListCollectionView view) : base(vm, view)
        {
            try
            {
                ReferenceDS referenceDS = CustomBrokerWpf.References.ReferenceDS;
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
                if (referenceDS.tableContactType.Count == 0)
                {
                    ReferenceDSTableAdapters.ContactTypeAdapter thisContactTypeAdapter = new ReferenceDSTableAdapters.ContactTypeAdapter();
                    thisContactTypeAdapter.Fill(referenceDS.tableContactType);
                }
                mycontacttypes = new System.Data.DataView(referenceDS.tableContactType, string.Empty, string.Empty, System.Data.DataViewRowState.Unchanged | System.Data.DataViewRowState.ModifiedCurrent);
                if (referenceDS.ContactPointTypeTb.Count == 0)
                {
                    ReferenceDSTableAdapters.ContactPointTypeAdapter thisTypeAdapter = new ReferenceDSTableAdapters.ContactPointTypeAdapter();
                    thisTypeAdapter.Fill(referenceDS.ContactPointTypeTb);
                }
                mypointtypes = new System.Data.DataView(referenceDS.ContactPointTypeTb, string.Empty, string.Empty, System.Data.DataViewRowState.Unchanged | System.Data.DataViewRowState.ModifiedCurrent);
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
            
            base.DeleteQuestionHeader = "Удалить поставщика?";
            mydbm = new AgentDBM();
            mydbm.FillType = lib.FillType.Refresh;
            mydbm.AddressDBM = new AgentAddressDBM();
            mydbm.AliasDBM = new AgentAliasDBM();
            mydbm.BrandDBM = new AgentBrandDBM();
            mydbm.ContactDBM = new AgentContactDBM();
            mydbm.ContractDBM = new ContractDBM();

            BrandDBM bdbm = new BrandDBM() { Collection = new System.Collections.Generic.List<Brand>()};
            bdbm.Load();
            mybrands = new ListCollectionView(bdbm.Collection);
        }
        public AgentCommand(AgentVM vm) : this(vm,null) { }

        private System.Data.DataView myaddresstypes;
        public System.Data.DataView AddressTypes
        { get { return myaddresstypes; } }
        private ListCollectionView mybrands;
        public ListCollectionView Brands
        { get { return mybrands; } }
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
        private System.Data.DataView mytowns;
        public System.Data.DataView Towns
        { get { return mytowns; } }
        
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void RefreshData(object parametr)
        {
            mydbm.ItemId = this.VModel.DomainObject.Id;
            mydbm.GetFirst();
            this.PopupText = mydbm.ErrorMessage;
        }
        public override bool SaveDataChanges()
        {
            bool needrefresh = this.VModel.DomainObject.HasPropertyOutdatedValue(nameof(Agent.State));
            bool succses = base.SaveDataChanges();
            if (needrefresh)
            {
                CustomBrokerWpf.References.AgentNames.Refresh();
                CustomBrokerWpf.References.AgentNames.RefreshViews();
            }
            return succses;
        }
    }

    public class AgentItemViewCommander : lib.ViewModelCurrentItemCommand<AgentVM>
    {
        internal AgentItemViewCommander():base()
        {
            myadbm = new AgentDBM();
            mydbm = myadbm;
            mysync = new AgentSynchronizer();
            myadbm.AliasDBM = new AgentAliasDBM();
            myadbm.Fill();
            if (myadbm.Errors.Count > 0)
                this.OpenPopup("Загрузка данных\n" + mydbm.ErrorMessage, true);
            else
            {
                mysync.DomainCollection = myadbm.Collection;
                base.Collection = mysync.ViewModelCollection;
            }
            base.DeleteQuestionHeader = "Удалить агента?";
        }

        AgentDBM myadbm;
        AgentSynchronizer mysync;

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

        protected override bool CanDeleteData(object parametr)
        {
            return this.CurrentItem == null || this.CurrentItem.Validate(true);
        }
        protected override bool CanRejectChanges()
        {
            return true;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override AgentVM CreateCurrentViewItem(lib.DomainBaseNotifyChanged domainobject)
        {
            return new AgentVM(domainobject as Agent);
        }
        protected override void OnCurrentItemChanged()
        {
        }
        protected override void OtherViewRefresh()
        {
            CustomBrokerWpf.References.AgentNames.RefreshViews();
        }
        protected override void RefreshData(object parametr)
        {
            Agent current = this.CurrentItem?.DomainObject;
            myadbm.Fill();
            if (myadbm.Errors.Count > 0)
                this.PopupText = mydbm.ErrorMessage;
            if (current != null)
            {
                foreach (AgentVM item in myview)
                    if (object.Equals(current, item.DomainObject))
                        myview.MoveCurrentTo(item);
            }
            CustomBrokerWpf.References.AgentNames.Refresh();
            CustomBrokerWpf.References.AgentNames.RefreshViews();
        }
        protected override void SettingView()
        {
            base.SettingView();
            myview.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as AgentVM).DomainObject.State < 208; };
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class AgentViewCommand : lib.ViewModelViewCommand
    {
        public AgentViewCommand()
        {
            myadbm = new AgentDBM(); // default not load old
            mydbm = myadbm;
            myadbm.AliasDBM = new AgentAliasDBM();
            mysync = new AgentSynchronizer();
            myadbm.FillAsyncCompleted = () =>
            {
                if (mydbm.Errors.Count > 0)
                    this.OpenPopup("Загрузка данных\n" + mydbm.ErrorMessage, true);
                myadbm.FillType = lib.FillType.Refresh;
            };
            myadbm.Collection = new ObservableCollection<Agent>();
            myadbm.FillAsync();
            mysync.DomainCollection = myadbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            base.DeleteQuestionHeader = "Удалить поставщика?";

            #region Filter
            myfilter = new lib.SQLFilter.SQLFilter("agent", "AND", CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            myadbm.Filter = myfilter;

            mystatefilter = new StateCheckListBox();
            mystatefilter.ExecCommand1 = () => { this.Items.Filter = OverallFilterOn; };
            mystatefilter.ExecCommand2 = () => { mystatefilter.Clear(); };
            mystatefilter.SelectedItems = new System.Collections.Generic.List<lib.ReferenceSimpleItem>();
            mystatefilter.SelectedItems.Add(CustomBrokerWpf.References.CustomerRowStates.FindFirstItem("Id",0));
            this.Items.Filter = OverallFilterOn;
            mynamefilter = new AgentNameCheckListBoxVMFill();
            mynamefilter.DeferredFill = true;
            mynamefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mynamefilter.ExecCommand2 = () => { mynamefilter.Clear(); };
            mynamefilter.ItemsSource = myview.OfType<AgentVM>();
            myfullnamefilter = new AgentFullNameCheckListBoxVMFill();
            myfullnamefilter.DeferredFill = true;
            myfullnamefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myfullnamefilter.ExecCommand2 = () => { myfullnamefilter.Clear(); };
            myfullnamefilter.ItemsSource = myview.OfType<AgentVM>();
            mycreaterfilter = new AgentCreaterCheckListBoxVMFill();
            mycreaterfilter.DeferredFill = true;
            mycreaterfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycreaterfilter.ExecCommand2 = () => { mycreaterfilter.Clear(); };
            mycreaterfilter.ItemsSource = myview.OfType<AgentVM>();
            myrecommendfilter = new AgentRecommendCheckListBoxVMFill();
            myrecommendfilter.DeferredFill = true;
            myrecommendfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myrecommendfilter.ExecCommand2 = () => { myrecommendfilter.Clear(); };
            myrecommendfilter.ItemsSource = myview.OfType<AgentVM>();
            mydayentryfilter = new libui.DateFilterVM();
            mydayentryfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mydayentryfilter.ExecCommand2 = () => { mydayentryfilter.Clear(); };

            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            #endregion
        }

        AgentDBM myadbm;
        AgentSynchronizer mysync;

        #region Filter
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { get { return myfilter; } }

        private AgentNameCheckListBoxVMFill mynamefilter;
        public AgentNameCheckListBoxVMFill NameFilter
        { get { return mynamefilter; } }
        private AgentFullNameCheckListBoxVMFill myfullnamefilter;
        public AgentFullNameCheckListBoxVMFill FullNameFilter
        { get { return myfullnamefilter; } }
        private AgentCreaterCheckListBoxVMFill mycreaterfilter;
        public AgentCreaterCheckListBoxVMFill CreaterFilter
        { get { return mycreaterfilter; } }
        private AgentRecommendCheckListBoxVMFill myrecommendfilter;
        public AgentRecommendCheckListBoxVMFill RecommendFilter
        { get { return myrecommendfilter; } }
        private StateCheckListBox mystatefilter;
        public StateCheckListBox StateFilter
        { get { return mystatefilter; } }
        private libui.DateFilterVM mydayentryfilter;
        public libui.DateFilterVM DayEntryFilter
        { get { return mydayentryfilter; } }

        private bool FilterEmpty
        {
            get
            {
                return !(myfullnamefilter.FilterOn || mynamefilter.FilterOn || mycreaterfilter.FilterOn || myrecommendfilter.FilterOn || mydayentryfilter.FilterOn);// || mystatefilter.FilterOn
            }
        }
        private void FilterActualise()
        {
            if (mynamefilter.FilterOn)
            {
                string[] items = new string[mynamefilter.SelectedItems.Count];
                for (int i = 0; i < mynamefilter.SelectedItems.Count; i++)
                    items[i] = (mynamefilter.SelectedItems[i] as Agent).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "agent", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "agent", new string[0]);
            if (myfullnamefilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items = new string[myfullnamefilter.SelectedItems.Count];
                for (int i = 0; i < myfullnamefilter.SelectedItems.Count; i++)
                {
                    items[i] = (string)myfullnamefilter.SelectedItems[i];
                    if (string.IsNullOrEmpty(items[i]))
                        isNullOrEmpty = true;
                }
                myfilter.SetList(myfilter.FilterWhereId, "fullname", items, isNullOrEmpty);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "fullname", new string[0], false);
            if (mycreaterfilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items = new string[mycreaterfilter.SelectedItems.Count];
                for (int i = 0; i < mycreaterfilter.SelectedItems.Count; i++)
                {
                    items[i] = (string)mycreaterfilter.SelectedItems[i];
                    if (string.IsNullOrEmpty(items[i]))
                        isNullOrEmpty = true;
                }
                myfilter.SetList(myfilter.FilterWhereId, "creater", items, isNullOrEmpty);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "creater", new string[0], false);
            if (myrecommendfilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] items = new string[myrecommendfilter.SelectedItems.Count];
                for (int i = 0; i < myrecommendfilter.SelectedItems.Count; i++)
                {
                    items[i] = (string)myrecommendfilter.SelectedItems[i];
                    if (string.IsNullOrEmpty(items[i]))
                        isNullOrEmpty = true;
                }
                myfilter.SetList(myfilter.FilterWhereId, "recommend", items, isNullOrEmpty);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "recommend", new string[0], false);
            //if (mystatefilter.FilterOn)
            //{
            //    string[] items = new string[mystatefilter.SelectedItems.Count];
            //    for (int i = 0; i < mystatefilter.SelectedItems.Count; i++)
            //    {
            //        items[i] = (mystatefilter.SelectedItems[i] as lib.ReferenceSimpleItem).Id.ToString();
            //    }
            //    myfilter.SetList(myfilter.FilterWhereId, "state", items);
            //}
            //else
            //    myfilter.SetList(myfilter.FilterWhereId, "state", new string[0], false);
            myfilter.SetDate(myfilter.FilterWhereId, "dayentry", "dayentry", mydayentryfilter.DateStart, mydayentryfilter.DateStop, mydayentryfilter.IsNull);
        }

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            this.EndEdit();
            FilterActualise();
            SaveRefresh.Execute(null);
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
            mynamefilter.Clear();
            mynamefilter.IconVisibileChangedNotification();
            myfullnamefilter.Clear();
            myfullnamefilter.IconVisibileChangedNotification();
            mycreaterfilter.Clear();
            mycreaterfilter.IconVisibileChangedNotification();
            myrecommendfilter.Clear();
            myrecommendfilter.IconVisibileChangedNotification();
            mystatefilter.Clear();
            mystatefilter.IconVisibileChangedNotification();
            mydayentryfilter.Clear();
            mydayentryfilter.IconVisibileChangedNotification();
            this.OverallFilterSet = string.Empty;
        }
        private bool FilterClearCanExec(object parametr)
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
        internal bool OverallFilterOn(object item)
        {
            bool where = lib.ViewModelViewCommand.ViewFilterDefault(item);

            if (where & !string.IsNullOrEmpty(myoverallfilter))
            {
                where = false;
                string filter = myoverallfilter.ToLower();
                AgentVM agent = item as AgentVM;
                
                if (agent.Name?.ToLower().IndexOf(filter) > -1 || agent.FullName?.ToLower().IndexOf(filter) > -1)
                    where = true;
                else if (agent.Recommend?.ToLower().IndexOf(filter) > -1)
                    where = true;
                else if (agent.Creater?.ToLower().IndexOf(filter) > -1)
                    where = true;
                else if (agent.DayEntry?.ToString().IndexOf(filter) > -1)
                    where = true;
                else
                {
                    if (!where)
                    {
                        foreach (AliasVM alias in agent.Aliases.OfType<AliasVM>())
                            if (alias?.Name?.ToLower().IndexOf(filter) > -1)
                            { where = true; break; }
                    }
                    //if (!where)
                    //{
                    //    foreach (AgentAddressVM address in agent.Addresses.OfType<CustomerAddressVM>())
                    //        if (address?.AddressDescription?.ToLower().IndexOf(filter) > -1)
                    //        { where = true; break; }
                    //        else if (address?.Locality?.ToLower().IndexOf(filter) > -1)
                    //        { where = true; break; }
                    //        else if (address?.Town?.ToLower().IndexOf(filter) > -1)
                    //        { where = true; break; }
                    //        else if (address?.AddressTypeID > 0)
                    //        {
                    //            if (CustomBrokerWpf.References.ReferenceDS.tableAddressType.Count == 0)
                    //            {
                    //                ReferenceDSTableAdapters.AddressTypeAdapter thisAddressTypeAdapter = new ReferenceDSTableAdapters.AddressTypeAdapter();
                    //                thisAddressTypeAdapter.Fill(CustomBrokerWpf.References.ReferenceDS.tableAddressType);
                    //            }
                    //            if (CustomBrokerWpf.References.ReferenceDS.tableAddressType.FindByaddresstypeID(address.AddressTypeID.Value).addresstypeName.ToLower().IndexOf(filter) > -1)
                    //                where = true; break;
                    //        }
                    //}
                    //if (!where)
                    //{
                    //    foreach (AgentContactVM contact in agent.Contacts.OfType<CustomerContactVM>())
                    //        if (contact?.FullName.ToLower().IndexOf(filter) > -1)
                    //        { where = true; break; }
                    //        else
                    //        {
                    //            foreach (ContactPointVM point in contact.Points.OfType<ContactPointVM>())
                    //                if (point?.Name?.ToLower().IndexOf(filter) > -1)
                    //                { where = true; break; }
                    //                else if (point?.Value?.ToLower().IndexOf(filter) > -1)
                    //                { where = true; break; }
                    //                else if (!string.IsNullOrEmpty(point?.Value) && point?.Value?.ToLower().Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).IndexOf(filter) > -1)
                    //                { where = true; break; }
                    //            if (where) break;
                    //        }
                    //}
                }
            }
            else if(where & string.IsNullOrEmpty(myoverallfilter) && mystatefilter.FilterOn)
            {
                where = false;
                AgentVM agent = item as AgentVM;
                foreach (lib.ReferenceSimpleItem state in mystatefilter.SelectedItems)
                {
                    if ((agent.State ?? 0) == state.Id)
                    {
                        where = true;
                        break;
                    }
                }
            }

            return where;
        }
        #endregion

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
            CustomBrokerWpf.References.AgentNames.Refresh();
            CustomBrokerWpf.References.AgentNames.RefreshViews();
        }
        protected override void RefreshData(object parametr)
        {
            myadbm.Fill();
            // refresh agents in another views
            CustomBrokerWpf.References.AgentNames.Refresh();
            CustomBrokerWpf.References.AgentNames.RefreshViews();
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class AgentNameCheckListBoxVMFill : libui.CheckListBoxVMFill<AgentVM, Agent>
    {
        internal AgentNameCheckListBoxVMFill() : base()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return ((Agent)item).Name; };
            this.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        protected override void AddItem(AgentVM item)
        {
            if (!Items.Contains(item.DomainObject)) Items.Add(item.DomainObject);
        }
    }
    public class AgentFullNameCheckListBoxVMFill : libui.CheckListBoxVMFill<AgentVM, string>
    {
        internal AgentFullNameCheckListBoxVMFill() : base()
        { }

        protected override void AddItem(AgentVM item)
        {
            if (!Items.Contains(item.FullName??string.Empty)) Items.Add(item.FullName ?? string.Empty);
        }
    }
    public class AgentCreaterCheckListBoxVMFill : libui.CheckListBoxVMFill<AgentVM, string>
    {
        internal AgentCreaterCheckListBoxVMFill() : base()
        { }

        protected override void AddItem(AgentVM item)
        {
            if (!Items.Contains(item.Creater)) Items.Add(item.Creater);
        }
    }
    public class AgentRecommendCheckListBoxVMFill : libui.CheckListBoxVMFill<AgentVM, string>
    {
        internal AgentRecommendCheckListBoxVMFill() : base()
        { }

        protected override void AddItem(AgentVM item)
        {
            if (!Items.Contains(item.Recommend ?? string.Empty)) Items.Add(item.Recommend ?? string.Empty);
        }
    }
    public class StateCheckListBox: libui.CheckListBoxVM
    {
        public StateCheckListBox()
        {
            base.ItemsView = new ListCollectionView(CustomBrokerWpf.References.CustomerRowStates);
            base.RefreshIsVisible = false;
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return ((lib.ReferenceSimpleItem)item).Name; };
        }
    }
}
