using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Alias : lib.DomainBaseReject
    {
        public Alias(int id, lib.DomainObjectState dstate
            ,int customerid, string name
            ) : base(id, dstate)
        {
            mycustomerid = customerid;
            myname = name;
        }
        public Alias() : this(lib.NewObjectId.NewId,lib.DomainObjectState.Added,0,null) { }

        private int mycustomerid;
        public int CustomerId
        {
            set
            {
                SetProperty<int>(ref mycustomerid, value);
            }
            get { return mycustomerid; }
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

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            CustomerLegal newitem = (CustomerLegal)sample;
            this.Name = newitem.Name;
        }
    }

    public class AliasDBM : lib.DBManagerId<Alias>
    {
        public AliasDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.CustomerAlias_sp";
            InsertCommandText = "dbo.CustomerAliasAdd_sp";
            UpdateCommandText = "dbo.CustomerAliasUpd_sp";
            DeleteCommandText = "dbo.CustomerAliasDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int),
            };
            SqlParameter paridout = new SqlParameter("@param3", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@AliasID", System.Data.SqlDbType.Int);
            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@param1", System.Data.SqlDbType.Int) };
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {new SqlParameter("@AliasCustomer", System.Data.SqlDbType.NVarChar,100) };
            mydeleteparams = new SqlParameter[] { parid };
        }

        public override int? ItemId
        {
            set
            {
                SelectParams[0].Value=value;
            }
            get
            {
                return (int?)SelectParams[0].Value;
            }
        }
        protected override Alias CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
           return new Alias(reader.GetInt32(0),lib.DomainObjectState.Unchanged, reader.GetInt32(1), reader.IsDBNull(2)?null:reader.GetString(2));
        }
        protected override void GetOutputParametersValue(Alias item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(Alias item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(Alias item){ return true; }
        protected override bool SaveIncludedObject(Alias item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(Alias item)
        {
            myinsertparams[1].Value=this.ItemId;
            myupdateparams[0].Value = item.Id;
            myinsertupdateparams[0].Value = item.Name;
            return true;
        }
        protected override void SetSelectParametersValue()
        {
        }
        protected override void LoadObjects(Alias item)
        {
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class AliasVM:lib.ViewModelErrorNotifyItem<Alias>
    {
        public AliasVM(Alias item):base(item)
        {
            ValidetingProperties.AddRange(new string[] { "Name" });
            DeleteRefreshProperties.AddRange(new string[] { "Name" });
            InitProperties();
        }
        public AliasVM() : this(new Alias()) { }

        private string myname;
        public string Name
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(myname, value)))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Name);
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

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Name":
                    this.Name = this.DomainObject.Name;
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
                    if (string.IsNullOrEmpty(this.Name))
                    {
                        errmsg = "Псевдоним не может быть пустым!";
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

    internal class AliasSynchronizer : lib.ModelViewCollectionsSynchronizer<Alias, AliasVM>
    {
        protected override Alias UnWrap(AliasVM wrap)
        {
            return wrap.DomainObject as Alias;
        }
        protected override AliasVM Wrap(Alias fill)
        {
            return new AliasVM(fill);
        }
    }
}
