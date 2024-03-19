using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Storage
{
    public class WarehouseAddress : Address
    {
        public WarehouseAddress(int addressid, lib.DomainObjectState dstate
            , string addressdescription, byte addresstypeid, string locality, string town, Warehouse warehouse
            ) : base(addressid, dstate, addressdescription, addresstypeid, locality, town)
        {
            mywarehouse = warehouse;
        }
        public WarehouseAddress() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added, null, 0, null, null, null) { }

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

    public class WarehouseAddressDBM : lib.DBManager<WarehouseAddress,WarehouseAddress>
    {
        public WarehouseAddressDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.WarehouseAddress_sp";
            InsertCommandText = "dbo.WarehouseAddressAdd_sp";
            UpdateCommandText = "dbo.WarehouseAddressUpd_sp";
            DeleteCommandText = "dbo.WarehouseAddressDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@warehouseid", System.Data.SqlDbType.Int)
            };
            SqlParameter paridout = new SqlParameter("@AddressID", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@AddressID", System.Data.SqlDbType.Int);
            myinsertparams = new SqlParameter[] { paridout };
            myupdateparams = new SqlParameter[] {
                parid
                ,new SqlParameter("@addressdescrtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@localitytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@towntrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@addresstypetrue", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@warehouseid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@addresstypeid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@AddressDescr", System.Data.SqlDbType.NVarChar,15)
                ,new SqlParameter("@Locality", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@Town", System.Data.SqlDbType.NVarChar,20)  };
            mydeleteparams = new SqlParameter[] { parid, new SqlParameter("@addresstypeid", System.Data.SqlDbType.Int) };
        }

        private Warehouse mywarehouse;
        internal Warehouse Warehouse
        {
            set { mywarehouse = value; }
            get { return mywarehouse; }
        }

		protected override WarehouseAddress CreateRecord(SqlDataReader reader)
		{
            return new WarehouseAddress(reader.GetInt32(this.Fields["AddressID"]), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(this.Fields["AddressDescr"]) ? null : reader.GetString(this.Fields["AddressDescr"])
                , reader.IsDBNull(this.Fields["addresstypeid"]) ? (byte)0 : reader.GetByte(this.Fields["addresstypeid"])
                , reader.IsDBNull(this.Fields["Locality"]) ? null : reader.GetString(this.Fields["Locality"])
                , reader.IsDBNull(this.Fields["Town"]) ? null : reader.GetString(this.Fields["Town"])
                , mywarehouse);
		}
        protected override WarehouseAddress CreateModel(WarehouseAddress reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
			return reader;
        }
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader), addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default, System.Func<bool> reading =null)
		{
			return true;
		}
        protected override void GetOutputParametersValue(WarehouseAddress item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(WarehouseAddress item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(WarehouseAddress item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(WarehouseAddress item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(WarehouseAddress item)
        {
            bool success = true;
            if (item.AddressTypeID == 0)
            {
                this.Errors.Add(new lib.DBMError(item, "Не указан вид адреса!", ""));
                success = false;
            }
            else
            {
                myupdateparams[0].Value = item.Id;
                myupdateparams[1].Value = item.HasPropertyOutdatedValue("AddressDescription");
                myupdateparams[2].Value = item.HasPropertyOutdatedValue("Locality");
                myupdateparams[3].Value = item.HasPropertyOutdatedValue("Town");
                myupdateparams[4].Value = item.HasPropertyOutdatedValue("AddressTypeID");
                myinsertupdateparams[0].Value = item.Warehouse?.Id ?? mywarehouse.Id;
                myinsertupdateparams[1].Value = item.AddressTypeID;
                myinsertupdateparams[2].Value = item.AddressDescription;
                myinsertupdateparams[3].Value = item.Locality;
                myinsertupdateparams[4].Value = item.Town;
            }
            return success;
        }
        protected override void SetSelectParametersValue()
        {
            SelectParams[0].Value = mywarehouse?.Id;
        }
    }

    public class WarehouseAddressVM : AddressVM
    {
        public WarehouseAddressVM(WarehouseAddress model) : base(model)
        {
            mymodel = model;
        }
        public WarehouseAddressVM() : this(new WarehouseAddress()) { }

        private WarehouseAddress mymodel;
        public WarehouseAddress Model
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
        public int AddressTypeInt
        { set 
            {
                base.AddressTypeID = (byte)value;
                if (ValidateProperty(nameof(base.AddressTypeID)))
                    ClearErrorMessageForProperty(nameof(this.AddressTypeInt));
                else
                    AddErrorMessageForProperty(nameof(this.AddressTypeInt), "Необходимо указать вид адреса!");
            } get { return (int)base.AddressTypeID; } }

    }

    internal class WarehouseAddressSynchronizer : lib.ModelViewCollectionsSynchronizer<WarehouseAddress, WarehouseAddressVM>
    {
        protected override WarehouseAddress UnWrap(WarehouseAddressVM wrap)
        {
            return wrap.Model;
        }
        protected override WarehouseAddressVM Wrap(WarehouseAddress fill)
        {
            return new WarehouseAddressVM(fill);
        }
    }
}
