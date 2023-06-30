using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class AgentAlias : lib.DomainBaseStamp
    {
        public AgentAlias(int id, long stamp, lib.DomainObjectState domainstate
            ,Agent agent,string alias
            ) : base(id, stamp, null, null, domainstate)
        {
            myagent = agent;
            myalias = alias;
        }
        public AgentAlias():this(lib.NewObjectId.NewId,0,lib.DomainObjectState.Added
            , null, null)
        { }

        private Agent myagent;
        public Agent Agent
        { set { SetProperty<Agent>(ref myagent, value); } get { return myagent; } }
        private string myalias;
        public string Alias
        { set { SetProperty<string>(ref myalias, value); } get { return myalias; } }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            AgentAlias templ = sample as AgentAlias;
            this.Alias = templ.Alias;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Alias):
                    this.Alias = (string)this.GetPropertyOutdatedValue(nameof(this.Alias
));
                    break;
            }
        }
        internal bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                case nameof(this.Alias):
                    if (string.IsNullOrEmpty((string)value))
                    {
                        errmsg = "Наименование псевдонима поставщика не может быть пустым!";
                        isvalid = false;
                    }
                    break;
            }
            return isvalid;
        }
    }
    public struct AgentAliasRecord
    {
        internal int id;
        internal long stamp;
        internal int agent;
        internal string alias;
	}
	public class AgentAliasDBM : lib.DBManagerStamp<AgentAliasRecord,AgentAlias>
    {
        public AgentAliasDBM()
        {
            this.NeedAddConnection = true;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.AgentAlias_sp";
            InsertCommandText = "dbo.AgentAliasAdd_sp";
            UpdateCommandText = "dbo.AgentAliasUpd_sp";
            DeleteCommandText = "dbo.AgentAliasDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@agent", System.Data.SqlDbType.Int),new SqlParameter("@alias", System.Data.SqlDbType.NVarChar,100)
            };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0]
                ,new SqlParameter("@agentid", System.Data.SqlDbType.Int)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0]
                ,new SqlParameter("@alias", System.Data.SqlDbType.NVarChar,100)
            };
        }

        internal Agent Agent { set; get; }
        internal string Alias { set; get; }

        protected override AgentAliasRecord CreateRecord(SqlDataReader reader)
        {
            return new AgentAliasRecord(){
                id=reader.GetInt32(0),
                stamp=reader.GetInt64(reader.GetOrdinal("stamp")),
                agent=reader.GetInt32(reader.GetOrdinal("agentid")),
                alias=reader.GetString(reader.GetOrdinal("alias")) };
        }
		protected override AgentAlias CreateModel(AgentAliasRecord record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			return new AgentAlias(record.id, record.stamp, lib.DomainObjectState.Unchanged
				, this.Agent ?? CustomBrokerWpf.References.AgentStore.GetItemLoad(record.agent, addcon, out _)
				, record.alias);
		}
		protected override void GetOutputSpecificParametersValue(AgentAlias item)
        {
        }
        protected override bool SaveChildObjects(AgentAlias item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(AgentAlias item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[0].Value = this.Agent?.Id;
            this.SelectParams[1].Value = this.Alias;
        }
        protected override bool SetSpecificParametersValue(AgentAlias item)
        {
            myinsertparams[1].Value=item.Agent.Id;
            myinsertupdateparams[1].Value = item.Alias;
            return item.Agent.Id > 0;
        }
    }

    public class AgentAliasVM : lib.ViewModelErrorNotifyItem<AgentAlias>
    {
        public AgentAliasVM() : this(new AgentAlias()) { }
        public AgentAliasVM(AgentAlias model):base(model)
        {
            DeleteRefreshProperties.AddRange(new string[] { nameof(AgentVM.DayEntry), nameof(AgentVM.FullName), nameof(AgentVM.Name), nameof(AgentVM.Recommend), nameof(AgentVM.State) });
            ValidetingProperties.AddRange(new string[] { nameof(this.Alias) });
            InitProperties();
        }

        private string myalias;
        public string Alias
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(myalias, value)))
                {
                    string name = nameof(this.Alias);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myalias);
                    myalias = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Alias = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? myalias : null; }
        }

        protected override bool DirtyCheckProperty()
        {
            return !string.Equals(this.DomainObject.Alias, myalias);
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch(property)
            {
                case nameof(AgentAlias.Alias):
                    myalias = this.DomainObject.Alias;
                    break;
            }
        }
        protected override void InitProperties()
        {
            myalias = this.DomainObject.Alias;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Alias):
                    if (this.DomainObject.Alias != myalias)
                        myalias = this.DomainObject.Alias;
                    else
                        this.DomainObject.Alias = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.Alias):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myalias, out errmsg);
                    break;
            }
            if (isvalid)
                ClearErrorMessageForProperty(propertyname);
            else if (inform) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    internal class AgentAliasSynchronizer : lib.ModelViewCollectionsSynchronizer<AgentAlias, AgentAliasVM>
    {
        protected override AgentAlias UnWrap(AgentAliasVM wrap)
        {
            return wrap.DomainObject as AgentAlias;
        }
        protected override AgentAliasVM Wrap(AgentAlias fill)
        {
            return new AgentAliasVM(fill);
        }
    }
}
