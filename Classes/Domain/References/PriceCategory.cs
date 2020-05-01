using System;
using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.References
{
    public class PriceCategory : lib.DomainBaseNotifyChanged
    {
        public PriceCategory():this(lib.NewObjectId.NewId, string.Empty, string.Empty, false, true, lib.DomainObjectState.Added) { }
        public PriceCategory(int id, string name, string description, PriceCategory upper, bool isdefault, bool isactualy, lib.DomainObjectState state) : this(id, name, description, isdefault, isactualy, state)
        {
            myupper = upper;
        }
        public PriceCategory(int id, string name, string description,int? upper, bool isdefault, bool isactualy, lib.DomainObjectState state):this(id, name, description, isdefault, isactualy, state)
        {
            myupperid = upper;
        }
        private PriceCategory(int id,string name, string description,bool isdefault,bool isactualy,lib.DomainObjectState state) : base(id, state)
        {
            myname = name;
            mydesc = description;
            myisdefault = isdefault;
            myisactual = isactualy;
        }

        private int? myupperid;
        private PriceCategory myupper;
        public PriceCategory Upper
        {
            set
            {
                if (!object.Equals(myupper, value))
                {
                    string name = "Upper";
                    myupper = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get
            {
                if(myupper==null & myupperid!=null)
                {
                    myupper = CustomBrokerWpf.References.PriceCategories.FindFirstItem("Id", myupperid);
                }
                return myupper; }
        }
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
        private string mydesc;
        public string Description
        {
            set
            {
                if (!string.Equals(mydesc, value))
                {
                    string name = "Description";
                    mydesc = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mydesc; }
        }
        private bool myisdefault;
        public bool IsDefault
        {
            set
            {
                if (myisdefault != value)
                {
                    string name = "IsDefault";
                    myisdefault = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myisdefault; }
        }
        private bool myisactual;
        public bool IsActual
        {
            set
            {
                if (myisactual != value)
                {
                    string name = "IsActual";
                    myisactual = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myisactual; }
        }
    }

    internal class PriceCategoryDBM : lib.DBManager<PriceCategory>
    {
        internal PriceCategoryDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = false;
            SelectCommandText = "SELECT id,name,description,upperid,isdefault,isactual FROM spec.PriceCategory_tb";
            SelectParams = new SqlParameter[] { };

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);

            myinsertparams = new SqlParameter[] { paridout };
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,4),
                new SqlParameter("@description", System.Data.SqlDbType.NVarChar,150),
                new SqlParameter("@upperid", System.Data.SqlDbType.Int),
                new SqlParameter("@isdefault", System.Data.SqlDbType.Bit),
                new SqlParameter("@isactual", System.Data.SqlDbType.Bit),
            };
            mydeleteparams = new SqlParameter[] { parid };

            InsertProcedure = false;
            myinsertcommandtext = "INSERT INTO spec.PriceCategory_tb VALUES (@name,@description,@upperid,@isdefault,@isactual) SET @id=SCOPE_IDENTITY()";
            UpdateProcedure = false;
            myupdatecommandtext = "UPDATE spec.PriceCategory_tb SET name=@name,description=@description,upperid=@upperid,isdefault=@isdefault,isactual=@isactual";
            DeleteProcedure = false;
            mydeletecommandtext = "DELETE FROM spec.PriceCategory_tb WHERE id=@id";
        }

        protected override void SetSelectParametersValue()
        {
        }
        protected override PriceCategory CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new PriceCategory(reader.GetInt32(0),reader.GetString(1), reader.IsDBNull(2)?null: reader.GetString(2), reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3), reader.GetBoolean(4), reader.GetBoolean(5),lib.DomainObjectState.Unchanged);
        }
        protected override void GetOutputParametersValue(PriceCategory item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
            }
        }
        protected override bool SaveChildObjects(PriceCategory item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(PriceCategory item)
        {
            bool issacses = true;
            if (item.Upper?.DomainState == lib.DomainObjectState.Added)
            {
                issacses = this.SaveItemChanches(item.Upper);
            }
            return issacses;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(PriceCategory item)
        {
            myupdateparams[0].Value = item.Id;
            myinsertupdateparams[0].Value = item.Name;
            myinsertupdateparams[1].Value = item.Description;
            myinsertupdateparams[2].Value = item.Upper != null ? item.Upper.Id : (object)DBNull.Value; ;
            myinsertupdateparams[3].Value = item.IsDefault;
            myinsertupdateparams[4].Value = item.IsActual;
            return item.Upper?.DomainState != lib.DomainObjectState.Added;
        }
        protected override void ItemAcceptChanches(PriceCategory item)
        {
            item.AcceptChanches();
        }
        protected override void LoadObjects(PriceCategory item)
        {
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class PriceCategoryCollection : lib.ReferenceCollectionDomainBase<PriceCategory>
    {
        public PriceCategoryCollection():this(new PriceCategoryDBM()) { }
        public PriceCategoryCollection(lib.DBManager<PriceCategory> dbm) : base(dbm)
        {
            Fill();
        }

        public override PriceCategory FindFirstItem(string propertyName, object value)
        {
            PriceCategory first=null;
            foreach (PriceCategory item in this)
            {
                switch (propertyName)
                {
                    case "Id":
                        if (item.Id == (int)value)
                            first= item;
                            break;
                    case "Name":
                        if (item.Name.ToUpper().Equals(((string)value).ToUpper()))
                            first= item;
                        break;
                    default:
                        throw new NotImplementedException("Свойство " + propertyName + " не реализовано");
                }
            }
            return first;
        }
        protected override int CompareReferences(PriceCategory item1, PriceCategory item2)
        {
            return item1.Id.CompareTo(item2.Id);
        }
        protected override void UpdateItem(PriceCategory olditem, PriceCategory newitem)
        {
            olditem.Name = newitem.Name;
            olditem.Description = newitem.Description;
            olditem.IsDefault = newitem.IsDefault;
            olditem.IsActual = newitem.IsActual;
        }
    }
}
