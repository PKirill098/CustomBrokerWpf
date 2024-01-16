using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.References;
using System;
using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Branch : lib.DomainBaseStamp
    {
        public Branch(int id, Int64 stamp, DateTime? updated, string updater, lib.DomainObjectState domainstate
            ,Goods goods,Country country,string name
            ) : base(id, stamp, updated, updater, domainstate)
        {
            mygoods = goods;
            mycountry = country;
            myname = name;
        }
        public Branch():this(lib.NewObjectId.NewId,0,null,null,lib.DomainObjectState.Added
            ,null, null, null) { }

        private Goods mygoods;
        public Goods Goods
        { set { SetProperty<Goods>(ref mygoods, value); } get { return mygoods; } }
        private Country mycountry;
        public Country Country
        { set { SetProperty<Country>(ref mycountry, value); } get { return mycountry; } }
        private string myname;
        public string Name
        { set { SetProperty<string>(ref myname, value); } get { return myname; } }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Branch bsample = (Branch)sample;
            this.Name = bsample.Name;
        }
    }

    public class BranchDBM : lib.DBManagerStamp<Branch,Branch>
    {
        public BranchDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "[spec].[Branch_sp]";
            InsertCommandText = "[spec].[BranchAdd_sp]";
            UpdateCommandText = "[spec].[BranchUpd_sp]";
            DeleteCommandText = "[spec].[BranchDel_sp]";

            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0],myinsertparams[1]
                ,new SqlParameter("@goodsid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@countryid", System.Data.SqlDbType.Int)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,100)
            };
        }

		protected override Branch CreateRecord(SqlDataReader reader)
		{
            Branch item = new Branch(reader.GetInt32(0), reader.GetInt64(1), null, null, lib.DomainObjectState.Unchanged
                ,CustomBrokerWpf.References.GoodsStore.GetItem(reader.GetInt32(4))
                , CustomBrokerWpf.References.Countries.FindFirstItem("Id", reader.GetInt32(5))
                ,reader.GetString(6)
                );
            return CustomBrokerWpf.References.BranchStore.UpdateItem(item);
		}
        protected override Branch CreateModel(Branch reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
			return reader;
        }
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader), addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
        protected override void GetOutputSpecificParametersValue(Branch item) {}
        protected override bool SaveChildObjects(Branch item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Branch item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetSpecificParametersValue(Branch item)
        {
            myinsertparams[2].Value=item.Goods.Id;
            myinsertparams[3].Value = item.Country?.Code;
            myinsertupdateparams[0].Value = item.Name;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
    }

    internal class BranchStore : lib.DomainStorageLoad<Branch,Branch, BranchDBM>
    {
        public BranchStore(BranchDBM dbm) : base(dbm) {}

        protected override void UpdateProperties(Branch olditem, Branch newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class BranchVM : lib.ViewModelErrorNotifyItem<Branch>
    {
        public BranchVM(Branch domain):base(domain)
        {
            ValidetingProperties.AddRange(new string[] { "Name" });
            DeleteRefreshProperties.AddRange(new string[] { "Name" });
            InitProperties();
        }
        public BranchVM():this(new Branch()) { }

        private lib.DomainObjectState mydomainstate;
        private string myname;
        public string Name
        {
            set
            {
                if (!string.Equals(myname, value))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myname);
                    myname = value;
                    if (string.IsNullOrEmpty(value))
                    {
                        mydomainstate = this.DomainState;
                        this.DomainState = lib.DomainObjectState.Deleted;
                    }
                    else if (this.DomainState == lib.DomainObjectState.Deleted)
                        this.DomainState = mydomainstate;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Name = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? myname : null; }
        }
        private GoodsVM mygoods;
        public GoodsVM Goods
        { get { return mygoods; } }


        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Name":
                    myname = this.DomainObject.Name;
                    break;
                case "Goods":
                    mygoods = this.DomainObject.Goods != null ? new GoodsVM(this.DomainObject.Goods) : null;
                    break;
            }
        }
        protected override void InitProperties()
        {
            myname = this.DomainObject.Name;
            mygoods = this.DomainObject.Goods != null ? new GoodsVM(this.DomainObject.Goods) : null;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Name":
                    if (myname != this.DomainObject.Name)
                        myname = this.DomainObject.Name;
                    else
                        this.Name = (string)value;
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
                    if (string.IsNullOrEmpty(myname) && this.DomainState != lib.DomainObjectState.Deleted)
                    {
                        errmsg = "Необходимо указать название филиала!";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return this.DomainObject.Name != myname;
        }
    }
}
