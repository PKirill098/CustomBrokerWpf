using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Storage
{
    public class WarehouseContact : Contact
    {
        public WarehouseContact(int contactid, lib.DomainObjectState dstate
            , string contacttype, string name, string surname, string thirdname
            , Warehouse warehouse
            ) : base(contactid, dstate, contacttype, name, surname, thirdname)
        {
            mywarehouse = warehouse;
        }
        public WarehouseContact() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added, null, null, null, null, null) { }

        private Warehouse mywarehouse;
        public Warehouse Warehouse
        {
            set
            {
                SetProperty<Warehouse>(ref mywarehouse, value);
            }
            get { return mywarehouse; }
        }
    }

    internal class WarehouseContactDBM:lib.DBManager<WarehouseContact,WarehouseContact>
    {
        internal WarehouseContactDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.WarehouseContact_sp";
            InsertCommandText = "dbo.WarehouseContactAdd_sp";
            UpdateCommandText = "dbo.WarehouseContactUpd_sp";
            DeleteCommandText = "dbo.WarehouseContactDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@warehouseid", System.Data.SqlDbType.Int)
            };
            SqlParameter paridout = new SqlParameter("@ContactID", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@ContactID", System.Data.SqlDbType.Int);
            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@warehouseid", System.Data.SqlDbType.Int) };
            myupdateparams = new SqlParameter[] {
                parid
                ,new SqlParameter("@nametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@surnametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@thirdnametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@typetrue", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@ContactName", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@surname", System.Data.SqlDbType.NVarChar,25)
                ,new SqlParameter("@thirdname", System.Data.SqlDbType.NVarChar,25) 
                ,new SqlParameter("@ContactType", System.Data.SqlDbType.NVarChar,50) };
            mydeleteparams = new SqlParameter[] { parid, new SqlParameter("@Contacttypeid", System.Data.SqlDbType.Int) };

            mypdbm = new ContactPointDBM(); mypdbm.Command = new SqlCommand();
        }

        private ContactPointDBM mypdbm;
        private Warehouse mywarehouse;
        internal Warehouse Warehouse
        {
            set { mywarehouse = value; }
            get { return mywarehouse; }
        }

		protected override WarehouseContact CreateRecord(SqlDataReader reader)
		{
            return new WarehouseContact(reader.GetInt32(this.Fields["ContactID"]), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(this.Fields["contactType"]) ? null : reader.GetString(this.Fields["contactType"])
                , reader.IsDBNull(this.Fields["ContactName"]) ? null : reader.GetString(this.Fields["ContactName"])
                , reader.IsDBNull(this.Fields["surname"]) ? null : reader.GetString(this.Fields["surname"])
                , reader.IsDBNull(this.Fields["thirdname"]) ? null : reader.GetString(this.Fields["thirdname"])
                , mywarehouse);
		}
        protected override WarehouseContact CreateModel(WarehouseContact reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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
        protected override void GetOutputParametersValue(WarehouseContact item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(WarehouseContact item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(WarehouseContact item)
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
        protected override bool SaveIncludedObject(WarehouseContact item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            mypdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetParametersValue(WarehouseContact item)
        {
            myinsertparams[1].Value = item.Warehouse?.Id ?? mywarehouse.Id;
            myupdateparams[0].Value = item.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue(nameof(WarehouseContact.Name));
            myupdateparams[2].Value = item.HasPropertyOutdatedValue(nameof(WarehouseContact.SurName));
            myupdateparams[3].Value = item.HasPropertyOutdatedValue(nameof(WarehouseContact.ThirdName));
            myupdateparams[4].Value = item.HasPropertyOutdatedValue(nameof(WarehouseContact.ContactType));
            myinsertupdateparams[0].Value = item.Name;
            myinsertupdateparams[1].Value = item.SurName;
            myinsertupdateparams[2].Value = item.ThirdName;
            myinsertupdateparams[3].Value = item.ContactType;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            SelectParams[0].Value = mywarehouse?.Id;
        }
    }

    public class WarehouseContactVM : ContactVM
    {
        public WarehouseContactVM(WarehouseContact model) : base(model)
        {
            mymodel = model;
        }
        public WarehouseContactVM():this(new WarehouseContact()) { }

        private WarehouseContact mymodel;
        public WarehouseContact Model
        { get { return mymodel; } }

        public Warehouse Warehouse
        {
            set
            {
                if (!this.IsReadOnly && object.Equals(mymodel.Warehouse, value))
                {
                    string name = nameof(this.Warehouse);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mymodel.Warehouse);
                    ChangingDomainProperty = name; mymodel.Warehouse = value;
                }
            }
            get { return this.IsEnabled ? mymodel.Warehouse : null; }
        }
    }

    internal class WarehouseContactSynchronizer : lib.ModelViewCollectionsSynchronizer<WarehouseContact, WarehouseContactVM>
    {
        protected override WarehouseContact UnWrap(WarehouseContactVM wrap)
        {
            return wrap.Model;
        }
        protected override WarehouseContactVM Wrap(WarehouseContact fill)
        {
            return new WarehouseContactVM(fill);
        }
    }
}
