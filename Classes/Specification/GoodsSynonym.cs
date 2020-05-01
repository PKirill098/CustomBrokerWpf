using System;
using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;


namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class GoodsSynonym : lib.DomainBaseNotifyChanged
    {
        public GoodsSynonym(int id, Mapping mapping, string name, lib.DomainObjectState state) : base(id, state)
        {
            myname = name;
            mymapping = mapping;
        }
        public GoodsSynonym(Mapping mapping) :this(lib.NewObjectId.NewId, mapping, string.Empty, lib.DomainObjectState.Added) { }
        public GoodsSynonym() : this(null) { }

        private Mapping mymapping;
        public Mapping Mapping
        {
            set
            {
                if (!object.Equals(mymapping,value))
                {
                    string name = "Mapping";
                    mymapping = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mymapping; }
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
    }

    internal class GoodsSynonymDBM : lib.DBManager<GoodsSynonym>
    {
        internal GoodsSynonymDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = false;
            SelectCommandText = "SELECT id,name FROM [spec].[GoodsSynonym_tb] WHERE mappingid=@mappingid";
            SelectParams = new SqlParameter[] { new SqlParameter("@mappingid", System.Data.SqlDbType.Int) };

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);

            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@mappingid", System.Data.SqlDbType.Int)};
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,50),
                
            };
            mydeleteparams = new SqlParameter[] { parid };

            InsertProcedure = false;
            myinsertcommandtext = "INSERT INTO [spec].[GoodsSynonym_tb] (mappingid,name) VALUES(@mappingid,@name); SET @id=SCOPE_IDENTITY();";
            UpdateProcedure = false;
            myupdatecommandtext = "UPDATE [spec].[GoodsSynonym_tb] SET name=@name WHERE id=@id";
            DeleteProcedure = false;
            mydeletecommandtext = "DELETE FROM [spec].[GoodsSynonym_tb] WHERE id=@id";
        }

        private Mapping mymapping;
        public Mapping Mapping
        {
            set
            {
                mymapping = value;
                SelectParams[0].Value = value.Id;
            }
            get { return mymapping; }
        }

        protected override void SetSelectParametersValue()
        {
        }
        protected override GoodsSynonym CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new GoodsSynonym(reader.GetInt32(0), mymapping, reader.GetString(1), lib.DomainObjectState.Unchanged);
        }
        protected override void GetOutputParametersValue(GoodsSynonym item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
            }
        }
        protected override void ItemAcceptChanches(GoodsSynonym item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(GoodsSynonym item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(GoodsSynonym item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(GoodsSynonym item)
        {
            myupdateparams[0].Value = item.Id;
            myinsertparams[1].Value = mymapping.Id;
            myinsertupdateparams[0].Value = item.Name;
            return true;
        }
        protected override void LoadObjects(GoodsSynonym item)
        {
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class GoodsSynonymVM : lib.ViewModelErrorNotifyItem<GoodsSynonym>
    {
        public GoodsSynonymVM(GoodsSynonym domain) : base(domain)
        {
            ValidetingProperties.AddRange(new string[] { "Name" });
            InitProperties();
        }
        public GoodsSynonymVM() : this(new GoodsSynonym()) { }

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

    internal class GoodsSynonymSynchronizer : lib.ModelViewCollectionsSynchronizer<GoodsSynonym, GoodsSynonymVM>
    {
        protected override GoodsSynonym UnWrap(GoodsSynonymVM wrap)
        {
            return wrap.DomainObject as GoodsSynonym;
        }

        protected override GoodsSynonymVM Wrap(GoodsSynonym fill)
        {
            return new GoodsSynonymVM(fill);
        }
    }

}
