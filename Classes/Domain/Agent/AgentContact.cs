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
    public class AgentContact:Contact
    {
        public AgentContact(int contactid, lib.DomainObjectState dstate
            , string contacttype, string name, string surname, string thirdname
            , Agent agent
    ) : base(contactid, dstate, contacttype, name, surname, thirdname)
        {
            myagent = agent;
        }
        public AgentContact() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added, null, null, null, null, null) { }

        private Agent myagent;
        public Agent Agent
        {
            set
            {
                SetProperty<Agent>(ref myagent, value);
            }
            get { return myagent; }
        }
    }

    public class AgentContactDBM : lib.DBManager<AgentContact, AgentContact>
    {
        public AgentContactDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.AgentContact_sp";
            InsertCommandText = "dbo.AgentContactAdd_sp";
            UpdateCommandText = "dbo.AgentContactUpd_sp";
            DeleteCommandText = "dbo.AgentContactDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@agentid", System.Data.SqlDbType.Int)
            };
            SqlParameter paridout = new SqlParameter("@ContactID", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@ContactID", System.Data.SqlDbType.Int);
            myinsertparams = new SqlParameter[] { paridout };
            myupdateparams = new SqlParameter[] {
                parid
                ,new SqlParameter("@nametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@surnametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@thirdnametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@typetrue", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[] {
                new SqlParameter("@agentID", System.Data.SqlDbType.Int)
                ,new SqlParameter("@ContactName", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@surname", System.Data.SqlDbType.NVarChar,25)
                ,new SqlParameter("@thirdname", System.Data.SqlDbType.NVarChar,25)
                ,new SqlParameter("@ContactType", System.Data.SqlDbType.NVarChar,50)
            };
            mydeleteparams = new SqlParameter[] { parid };
            mypdbm = new ContactPointDBM(); mypdbm.Command = new SqlCommand();
        }

        private Agent myagent;
        public Agent Agent
        {
            set
            {
                myagent = value;
            }
            get
            {
                return myagent;
            }
        }

        private ContactPointDBM mypdbm;

        protected override AgentContact CreateRecord(SqlDataReader reader)
        {
            return new AgentContact(reader.GetInt32(this.Fields["ContactID"]), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(this.Fields["contactType"]) ? null : reader.GetString(this.Fields["contactType"])
                , reader.IsDBNull(this.Fields["ContactName"]) ? null : reader.GetString(this.Fields["ContactName"])
                , reader.IsDBNull(this.Fields["surname"]) ? null : reader.GetString(this.Fields["surname"])
                , reader.IsDBNull(this.Fields["thirdname"]) ? null : reader.GetString(this.Fields["thirdname"])
                , myagent);
        }
		protected override AgentContact CreateModel(AgentContact record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
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
		protected override void GetOutputParametersValue(AgentContact item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(AgentContact item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(AgentContact item)
        {
            bool issuccess = true;
            if (!item.PointsIsNull)
            {
                mypdbm.Errors.Clear();
                mypdbm.ItemId = item.Id;
                mypdbm.Collection = item.Points;
                if (!mypdbm.SaveCollectionChanches())
                {
                    issuccess = false;
                    foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
                }
            }

            return issuccess;
        }
        protected override bool SaveIncludedObject(AgentContact item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            mypdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetParametersValue(AgentContact item)
        {
            myupdateparams[0].Value = item.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue("Name");
            myupdateparams[2].Value = item.HasPropertyOutdatedValue("SurName");
            myupdateparams[3].Value = item.HasPropertyOutdatedValue("ThirdName");
            myupdateparams[4].Value = item.HasPropertyOutdatedValue("ContactType");
            myinsertupdateparams[0].Value = item.Agent.Id;
            myinsertupdateparams[1].Value = item.Name;
            myinsertupdateparams[2].Value = item.SurName;
            myinsertupdateparams[3].Value = item.ThirdName;
            myinsertupdateparams[4].Value = item.ContactType;
            return true;
        }
        protected override void SetSelectParametersValue()
        {
            SelectParams[0].Value = myagent?.Id;
        }
    }
    
    public class AgentContactVM: ContactVM
    {
        public AgentContactVM(AgentContact item) : base(item) { }
        public AgentContactVM() : this(new AgentContact()) { }
    }

    internal class AgentContactSynchronizer : lib.ModelViewCollectionsSynchronizer<AgentContact, AgentContactVM>
    {
        protected override AgentContact UnWrap(AgentContactVM wrap)
        {
            return wrap.DomainObject as AgentContact;
        }
        protected override AgentContactVM Wrap(AgentContact fill)
        {
            return new AgentContactVM(fill);
        }
    }
}
