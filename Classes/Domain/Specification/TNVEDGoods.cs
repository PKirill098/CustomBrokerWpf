using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class TNVEDGoods : lib.DomainBaseNotifyChanged
    {
        public TNVEDGoods(int id, string name, lib.DomainObjectState state) : base(id, state)
        {
            myname = name;
        }
        public TNVEDGoods() : this(lib.NewObjectId.NewId, null, lib.DomainObjectState.Added) { }
        
        private string myname;
        public string Name
        {
            set
            {
                if (!string.Equals(myname, value))
                {
                    string name = "Name";
                    myname = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myname; }
        }
    }

    internal class TNVEDGoodsDBM : lib.DBManager<TNVEDGoods,TNVEDGoods>
    {
        internal TNVEDGoodsDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectCommandText = "[spec].TNVEDGoods_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@tnvedgroupid", System.Data.SqlDbType.Int) };

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);

            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@tnvedgroupid", System.Data.SqlDbType.Int) };
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@goodsname", System.Data.SqlDbType.NVarChar,50),
            };
            mydeleteparams = new SqlParameter[] { parid };

            InsertProcedure = true;
            myinsertcommandtext = "[spec].TNVEDGoodsAdd_sp";
            UpdateProcedure = true;
            myupdatecommandtext = "[spec].TNVEDGoodsUpd_sp";
            DeleteProcedure = true;
            mydeletecommandtext = "[spec].TNVEDGoodsDel_sp";
        }

        private TNVEDGroup mygroup;
        public TNVEDGroup TNVEDGroup
        {
            set
            {
                mygroup = value;
            }
            get { return mygroup; }
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            SelectParams[0].Value = mygroup.Id;
        }
		protected override TNVEDGoods CreateRecord(SqlDataReader reader)
		{
            return new TNVEDGoods(reader.GetInt32(0), reader.GetString(2), lib.DomainObjectState.Unchanged);
		}
		protected override TNVEDGoods CreateModel(TNVEDGoods reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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
		protected override void GetOutputParametersValue(TNVEDGoods item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
            }
        }
        protected override void ItemAcceptChanches(TNVEDGoods item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(TNVEDGoods item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(TNVEDGoods item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(TNVEDGoods item)
        {
            myupdateparams[0].Value = item.Id;
            myinsertparams[1].Value = mygroup.Id;
            myinsertupdateparams[0].Value = item.Name;
            return true;
        }
    }

    public class TNVEDGoodsVM : lib.ViewModelErrorNotifyItem<TNVEDGoods>
    {
        public TNVEDGoodsVM(TNVEDGoods model) : base(model)
        {
            ValidetingProperties.AddRange(new string[] { "Name" });
            DeleteRefreshProperties.AddRange(new string[] { "Name" });
            InitProperties();
        }
        public TNVEDGoodsVM() : this(new TNVEDGoods()) { }

        private string myname;
        public string Name
        {
            set
            {
                if (!string.Equals(myname, value) & base.IsEnabled)
                {
                    string name = "Name";
                    myname = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Name);
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name;
                        base.DomainObject.Name = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return base.IsEnabled ? myname : null; }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Name":
                    myname = this.DomainObject.Name;
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
                    if (string.IsNullOrEmpty(myname))
                    {
                        errmsg = "Отсутствует наименование";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myname!= base.DomainObject.Name;
        }
    }

    internal class TNVEDGoodsSynchronizer : lib.ModelViewCollectionsSynchronizer<TNVEDGoods, TNVEDGoodsVM>
    {
        protected override TNVEDGoods UnWrap(TNVEDGoodsVM wrap)
        {
            return wrap.DomainObject as TNVEDGoods;
        }
        protected override TNVEDGoodsVM Wrap(TNVEDGoods fill)
        {
            return new TNVEDGoodsVM(fill);
        }
    }

}
