using System.Data.SqlClient;
using System.Threading;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class CustomerAddress : Address
    {
        public CustomerAddress(int addressid, lib.DomainObjectState dstate
            ,string addressdescription, byte addresstypeid, int customerid, string locality, string town
            ) : base(addressid, dstate, addressdescription, addresstypeid, locality, town)
        {
            mycustomerid = customerid;
        }
        public CustomerAddress() : this(lib.NewObjectId.NewId,lib.DomainObjectState.Added,null,0,0,null,null) { }

        private int mycustomerid;
        public int CustomerId
        {
            set
            {
                SetProperty<int>(ref mycustomerid, value);
            }
            get { return mycustomerid; }
        }
    }

    public class CustomerAddressDBM : lib.DBManagerId<CustomerAddress, CustomerAddress>
    {
        public CustomerAddressDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.CustomerAddress_sp";
            InsertCommandText = "dbo.CustomerAddressAdd_sp";
            UpdateCommandText = "dbo.CustomerAddressUpd_sp";
            DeleteCommandText = "dbo.CustomerAddressDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int)
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
                ,new SqlParameter("@old", 0)
            };
            myinsertupdateparams = new SqlParameter[]
            {new SqlParameter("@customerID", System.Data.SqlDbType.Int),new SqlParameter("@addresstypeID", System.Data.SqlDbType.Int),new SqlParameter("@AddressDescr", System.Data.SqlDbType.NVarChar,15),new SqlParameter("@Locality", System.Data.SqlDbType.NVarChar,100),new SqlParameter("@Town", System.Data.SqlDbType.NVarChar,20)  };
            mydeleteparams = new SqlParameter[] { parid };
        }

        protected override CustomerAddress CreateRecord(SqlDataReader reader)
        {
           return new CustomerAddress(reader.GetInt32(2), lib.DomainObjectState.Unchanged
               , reader.IsDBNull(3) ? null : reader.GetString(3)
               , reader.IsDBNull(1) ? (byte)0 : reader.GetByte(1)
               , reader.GetInt32(0)
               , reader.IsDBNull(4) ? null : reader.GetString(4)
               , reader.IsDBNull(5) ? null : reader.GetString(5));
        }
		protected override CustomerAddress CreateModel(CustomerAddress record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			return record;
		}
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			base.TakeItem(this.CreateRecord(reader));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,System.Func<bool> reading=null)
		{
			return true;
		}
		protected override void GetOutputParametersValue(CustomerAddress item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(CustomerAddress item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(CustomerAddress item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(CustomerAddress item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(CustomerAddress item)
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
                myinsertupdateparams[0].Value = this.ItemId;
                myinsertupdateparams[1].Value = item.AddressTypeID;
                myinsertupdateparams[2].Value = item.AddressDescription;
                myinsertupdateparams[3].Value = item.Locality;
                myinsertupdateparams[4].Value = item.Town;
            }
            return success;
        }
        protected override void SetSelectParametersValue()
        {
        }
    }

    public class CustomerAddressVM: lib.ViewModelErrorNotifyItem<CustomerAddress>
    {
        public CustomerAddressVM(CustomerAddress item):base(item)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.AddressTypeID) });
            DeleteRefreshProperties.AddRange(new string[] { nameof(this.AddressTypeID) });
            InitProperties();
        }
        public CustomerAddressVM():this(new CustomerAddress()) { }

        public string AddressDescription
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.AddressDescription, value)))
                {
                    string name = "AddressDescription";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.AddressDescription);
                    ChangingDomainProperty = name; this.DomainObject.AddressDescription = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.AddressDescription : null; }
        }
        private byte? myaddresstypeid;
        public byte? AddressTypeID
        {
            set
            {
                if (!this.IsReadOnly & value.HasValue && myaddresstypeid!= value)
                {
                    string name = "AddressTypeID";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.AddressTypeID);
                    myaddresstypeid = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.AddressTypeID = value.Value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? myaddresstypeid : null; }
        }
        public string Locality
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Locality, value)))
                {
                    string name = "Locality";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Locality);
                    ChangingDomainProperty = name; this.DomainObject.Locality = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Locality : null; }
        }
        public string Town
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Town, value)))
                {
                    string name = "Town";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Town);
                    ChangingDomainProperty = name; this.DomainObject.Town = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Town : null; }
        }
        public string FullAddress
        {
            get { return this.IsEnabled ? this.DomainObject.FullAddress : null; }
        }
        public string FullAddressDescription
        {
            get { return this.IsEnabled ? this.DomainObject.FullAddressDescription : null; }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "AddressTypeID":
                    this.AddressTypeID = this.DomainObject.AddressTypeID;
                    break;
            }
        }
        protected override void InitProperties() { myaddresstypeid = this.DomainObject.AddressTypeID; }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "AddressDescription":
                    this.DomainObject.AddressDescription = (string)value;
                    break;
                case "AddressTypeID":
                    if (myaddresstypeid != this.DomainObject.AddressTypeID)
                        myaddresstypeid = this.DomainObject.AddressTypeID;
                    else
                        this.AddressTypeID = (byte?)value;
                    break;
                case "Locality":
                    this.DomainObject.Locality = (string)value;
                    break;
                case "Town":
                    this.DomainObject.Town = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.AddressTypeID):
                    if (!this.AddressTypeID.HasValue || this.AddressTypeID==0)
                    {
                        errmsg = "Необходимо указать вид адреса!";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myaddresstypeid != this.DomainObject.AddressTypeID;
        }
    }

    internal class CustomerAddressSynchronizer : lib.ModelViewCollectionsSynchronizer<CustomerAddress, CustomerAddressVM>
    {
        protected override CustomerAddress UnWrap(CustomerAddressVM wrap)
        {
            return wrap.DomainObject as CustomerAddress;
        }
        protected override CustomerAddressVM Wrap(CustomerAddress fill)
        {
            return new CustomerAddressVM(fill);
        }
    }
}
