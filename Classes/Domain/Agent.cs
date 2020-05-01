using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Agent : lib.DomainBaseStamp
    {
        public Agent(int id, long stamp, lib.DomainObjectState domainstate
            ,DateTime dateentry,string fullname,string name,string recommend,byte state
            ) : base(id, stamp, null, null, domainstate)
        {
            mydateentry = dateentry;
            myfullname = fullname;
            myname = name;
            myrecommend = recommend;
            mystate = state;
        }
        public Agent():this(lib.NewObjectId.NewId,0,lib.DomainObjectState.Added
            , DateTime.Today, null, null, null, 0) { }

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

    internal class AgentStore : lib.DomainStorageLoad<Agent>
    {
        public AgentStore(AgentDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(Agent olditem, Agent newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class AgentDBM : lib.DBManagerStamp<Agent>
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

        protected override Agent CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            Agent agent = new Agent(reader.GetInt32(0), reader.GetInt64(reader.GetOrdinal("stamp")), lib.DomainObjectState.Unchanged
                , reader.GetDateTime(reader.GetOrdinal("agentDayEntry"))
                , reader.IsDBNull(reader.GetOrdinal("agentFullName")) ? null : reader.GetString(reader.GetOrdinal("agentFullName"))
                , reader.GetString(reader.GetOrdinal("agentName"))
                , reader.IsDBNull(reader.GetOrdinal("agentRecommend")) ? null : reader.GetString(reader.GetOrdinal("agentRecommend"))
                , reader.GetByte(reader.GetOrdinal("agentState"))
                );
            agent = CustomBrokerWpf.References.AgentStore.UpdateItem(agent);
            if(myadbm != null)
            {
                myadbm.Command.Connection = addcon;
                myadbm.Agent = agent;
                myadbm.Collection = agent.Aliases;
                myadbm.Fill();
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
            return true;
        }
        protected override void SetSelectParametersValue()
        {
            this.NeedAddConnection = myadbm != null;
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
        protected override void LoadObjects(Agent item)
        {
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class AgentVM : lib.ViewModelErrorNotifyItem<Agent>
    {
        public AgentVM(Agent model):base(model)
        {
            DeleteRefreshProperties.AddRange(new string[] { nameof(AgentVM.DayEntry), nameof(AgentVM.FullName), nameof(AgentVM.Name), nameof(AgentVM.Recommend),nameof(AgentVM.State) });
            ValidetingProperties.AddRange(new string[] { nameof(this.Name)});
            InitProperties();
        }
        public AgentVM():this(new Agent()) { }

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

        protected override bool DirtyCheckProperty()
        {
            return string.IsNullOrEmpty(this.DomainObject.Name);
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
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
            throw new NotImplementedException();
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
        }
        protected override void SettingView()
        {
            base.SettingView();
            myview.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as AgentVM).DomainObject.State < 208; };
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }
}
