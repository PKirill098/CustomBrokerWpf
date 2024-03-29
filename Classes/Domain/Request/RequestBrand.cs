﻿using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    internal struct RequestBrandRecord
    {
        internal int brandid;
        internal bool selected;
    }
    public class RequestBrand:lib.DomainBaseNotifyChanged //lib.ReferenceContainer<Brand>
    {
        public RequestBrand(AgentBrand brand, Request request, bool selected, lib.DomainObjectState state) : base(0, state)
        {
            mybrand = brand;
            myrequest = request;
            myselected = selected;
        }

        private AgentBrand mybrand;
        public AgentBrand Brand
        { set { SetProperty<AgentBrand>(ref mybrand, value); } get { return mybrand; } }
        private Request myrequest;
        internal Request Request
        {
            set { myrequest = value; }
            get { return myrequest; }
        }
        private bool myselected;
        public bool Selected
        {
            set { SetProperty<bool>(ref myselected, value, () => { myrequest.BrandNamesRefresh(); }); }
            get { return myselected; }
        }
    }

    internal class RequestBrandDBM : lib.DBManager<RequestBrandRecord,RequestBrand>
    {
        internal RequestBrandDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            NeedAddConnection = true;
            SelectProcedure = true;
            UpdateProcedure = true;

            SelectCommandText = "dbo.RequestBrand_sp";
            UpdateCommandText = "dbo.RequestBrandUpd_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@agent", System.Data.SqlDbType.Int), new SqlParameter("@request", System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[] {
                new SqlParameter("@brand", System.Data.SqlDbType.Int)
                ,new SqlParameter("@request", System.Data.SqlDbType.Int)
                ,new SqlParameter("@selected",System.Data.SqlDbType.Bit)
            };

            abdbm = new AgentBrandDBM();
        }

        private AgentBrandDBM abdbm;
        private Agent myagent;
        internal Agent Agent
        {
            set { myagent = value; abdbm.Agent = value; }
            get { return myagent; }
        }
        private Request myrequest;
        internal Request Request
        {
            set { myrequest = value; }
            get { return myrequest; }
        }

        //protected override void CancelLoad()
        //{
        //    abdbm.CancelingLoad = this.CancelingLoad;
        //}
		protected override RequestBrandRecord CreateRecord(SqlDataReader reader)
		{   
            return new RequestBrandRecord() { 
                brandid = reader.GetInt32(this.Fields["brandID"]),
                selected = reader.GetBoolean(this.Fields["selected"])
            };
		}
        protected override RequestBrand CreateModel(RequestBrandRecord record, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
            System.Collections.Generic.List<lib.DBMError> errors;
            RequestBrand brand = new RequestBrand(
                new AgentBrand(lib.DomainObjectState.Unchanged
                    , myagent
                    , CustomBrokerWpf.References.BrandStorage.GetItemLoad(record.brandid, addcon, out errors))
                ,myrequest
                , record.selected
                , lib.DomainObjectState.Unchanged
                );
            if (errors != null) { this.Errors.AddRange(errors); }
			return brand;
        }
        protected override void GetOutputParametersValue(RequestBrand item)
        {
        }
        protected override void ItemAcceptChanches(RequestBrand item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(RequestBrand item)
        {
            return true; ;
        }
        protected override bool SaveIncludedObject(RequestBrand item)
        {
            bool Success = true;
            abdbm.Errors.Clear();
            if (!abdbm.SaveItemChanches(item.Brand))
            {
                Success = false;
                foreach (lib.DBMError err in abdbm.Errors) this.Errors.Add(err);
            }
            return Success;
        }
        protected override bool SaveReferenceObjects()
        {
            abdbm.Connection=this.Command.Connection;
            return true;
        }
        protected override bool SetParametersValue(RequestBrand item)
        {
            foreach (SqlParameter par in UpdateParams)
                switch (par.ParameterName)
                {
                    case "@brand":
                        par.Value = item.Brand?.Id;
                        break;
                    case "@request":
                        par.Value = myrequest?.Id;
                        break;
                    case "@selected":
                        par.Value = item.Selected;
                        break;
                }
            return true;
        }
        protected override void SetSelectParametersValue()
        {
            SelectParams[0].Value=this.Agent?.Id;
            SelectParams[1].Value = this.Request?.Id;
        }
    }

    public class RequestBrandVM:lib.ViewModelErrorNotifyItem<RequestBrand>
    {
        public RequestBrandVM(RequestBrand model):base(model)
        {
            DeleteRefreshProperties.AddRange(new string[] { nameof(this.Brand), nameof(this.Selected) });
            InitProperties();
        }

        public Brand Brand
        {
            get { return this.IsEnabled ? this.DomainObject.Brand.Brand : null; }
        }
        public bool Selected
        {
            set
            {
                if (!this.IsReadOnly && this.DomainObject.Selected != value)
                {
                    string name = "Selected";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Selected);
                        ChangingDomainProperty = name; this.DomainObject.Selected = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Selected : false; }
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
                case "Selected":
                    this.DomainObject.Selected = this.DomainObject.Selected;
                    break;
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

    internal class RequestBrandSynchronizer : lib.ModelViewCollectionsSynchronizer<RequestBrand, RequestBrandVM>
    {
        protected override RequestBrand UnWrap(RequestBrandVM wrap)
        {
            return wrap.DomainObject;
        }
        protected override RequestBrandVM Wrap(RequestBrand fill)
        {
            return new RequestBrandVM(fill);
        }
    }
}
