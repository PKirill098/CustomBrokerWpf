using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class AgentBrand:lib.DomainBaseReject
    {
       public AgentBrand(lib.DomainObjectState state
           , Domain.Agent agent, Brand brand):base(brand.Id, state)
        {
            myagent = agent;
            mybrand = brand;
        }
        public AgentBrand():this(lib.DomainObjectState.Added, null, new Brand()) {}

        private Domain.Agent myagent;
        public Domain.Agent Agent
        { set { SetProperty<Domain.Agent>(ref myagent, value); } get { return myagent; } }
        private Brand mybrand;
        public Brand Brand
        { set { SetProperty<Brand>(ref mybrand, value); } get { return mybrand; } }

        protected override void RejectProperty(string property, object value)
        {
        }
        protected override void PropertiesUpdate(DomainBaseReject sample)
        {
            AgentBrand templ = (AgentBrand)sample;
            this.Brand = templ.Brand;
        }
    }

    public class AgentBrandDBM : lib.DBManager<AgentBrand>
    {
        public AgentBrandDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.AgentBrand_sp";
            InsertCommandText = "dbo.AgentBrandAdd_sp";
            UpdateCommandText = "dbo.AgentBrandUpd_sp";
            DeleteCommandText = "dbo.AgentBrandDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@agent", System.Data.SqlDbType.Int)
            };
            SqlParameter paridout = new SqlParameter("@brandID", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@agentID", System.Data.SqlDbType.Int);
            myinsertparams = new SqlParameter[] { paridout };
            myupdateparams = new SqlParameter[] {
                new SqlParameter("@oldBrandID", System.Data.SqlDbType.Int)
                ,new SqlParameter("@newBrandID", System.Data.SqlDbType.Int){ Direction=System.Data.ParameterDirection.Output}
            };
            myinsertupdateparams = new SqlParameter[]
            {
                parid
                ,new SqlParameter("@brandName", System.Data.SqlDbType.NVarChar,100)
            };
            mydeleteparams = new SqlParameter[] { parid, new SqlParameter("@brandID", System.Data.SqlDbType.Int) };
        }

        private Domain.Agent myagent;
        internal Domain.Agent Agent
        {
            set { myagent = value; }
            get { return myagent; }
        }

        protected override void CancelLoad()
        {
        }
        protected override AgentBrand CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            return new AgentBrand(lib.DomainObjectState.Unchanged
                    , myagent
                    ,new Brand(reader.GetInt32(this.Fields["brandID"])
                        , lib.DomainObjectState.Unchanged
                        , reader.GetString(this.Fields["brandName"]))
                    );
        }
        protected override void GetOutputParametersValue(AgentBrand item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Brand.Id = (int)myinsertparams[0].Value;
            else if(item.DomainState == lib.DomainObjectState.Modified)
                item.Brand.Id = (int)myupdateparams[1].Value;
        }
        protected override void ItemAcceptChanches(AgentBrand item)
        {
            item.AcceptChanches();
            item.Brand.AcceptChanches();
        }
        protected override bool SaveChildObjects(AgentBrand item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(AgentBrand item)
        {
            if (item.DomainState == lib.DomainObjectState.Unchanged & item.Brand.DomainState == lib.DomainObjectState.Modified)
                item.DomainState = lib.DomainObjectState.Modified;
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(AgentBrand item)
        {
            foreach (SqlParameter par in UpdateParams)
                switch (par.ParameterName)
                {
                    case "@oldBrandID":
                        par.Value = item.HasPropertyOutdatedValue(nameof(AgentBrand.Brand)) ? item.GetPropertyOutdatedValue(nameof(AgentBrand.Brand)) : item.Brand?.Id;
                        break;
                }
            foreach (SqlParameter par in InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@agentID":
                        par.Value = item.Agent?.Id;
                        break;
                    case "@brandName":
                        par.Value = item.Brand?.Name;
                        break;
                }
            foreach (SqlParameter par in DeleteParams)
                switch (par.ParameterName)
                {
                    case "@brandID":
                        par.Value = item.Brand?.Id;
                        break;
                }
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            SelectParams[0].Value = myagent?.Id;
        }
    }

    public class AgentBrandVM : lib.ViewModelErrorNotifyItem<AgentBrand>
    {
        public AgentBrandVM(AgentBrand item) : base(item)
        {
            DeleteRefreshProperties.AddRange(new string[] { nameof(this.Brand) });
            InitProperties();
        }
        public AgentBrandVM():this(new AgentBrand()) { }

        public Brand Brand
        {
            get { return this.IsEnabled ? this.DomainObject.Brand : null; }
        }

        protected override bool DirtyCheckProperty()
        {
            return false;
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
                case "DependentNew":
                    this.DomainObject.Brand.RejectChanges();
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            return true;
        }
    }

    internal class AgentBrandSynchronizer : lib.ModelViewCollectionsSynchronizer<AgentBrand, AgentBrandVM>
    {
        protected override AgentBrand UnWrap(AgentBrandVM wrap)
        {
            return wrap.DomainObject;
        }
        protected override AgentBrandVM Wrap(AgentBrand fill)
        {
            return new AgentBrandVM(fill);
        }
    }
}
