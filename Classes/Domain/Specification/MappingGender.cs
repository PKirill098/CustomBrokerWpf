using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class MappingGender : lib.DomainBaseReject
    {
        public MappingGender(Gender gender, lib.DomainObjectState domainstate) :base(0,domainstate)
        {
            mygender = gender;
        }
        public MappingGender() : this(null, lib.DomainObjectState.Added) { }

        private Gender mygender;
        public Gender Gender
        {
            set
            {
                if (!object.Equals(mygender, value))
                {
                    string name = "Gender";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mygender);
                    mygender = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mygender; }
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Gender":
                    mygender = value as Gender;
                    break;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class MappingGenderDBM : lib.DBManager<MappingGender,MappingGender>
    {
        internal MappingGenderDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = false;
            SelectParams = new SqlParameter[] { new SqlParameter("@mappingid", System.Data.SqlDbType.Int) };
            SelectCommandText = "SELECT genderid FROM spec.MappingGender_tb WHERE mappingid=@mappingid";

            SqlParameter parmapping = new SqlParameter("@mappingid", System.Data.SqlDbType.Int);
            SqlParameter pargender = new SqlParameter("@genderid", System.Data.SqlDbType.Int);
            SqlParameter pargenderold = new SqlParameter("@genderoldid", System.Data.SqlDbType.Int);

            myinsertparams = new SqlParameter[] { };
            myinsertupdateparams = new SqlParameter[] { parmapping, pargender };
            myupdateparams = new SqlParameter[] { pargenderold };
            mydeleteparams = new SqlParameter[] { parmapping, pargenderold };

            InsertProcedure = false;
            myinsertcommandtext = "INSERT INTO [spec].[MappingGender_tb] (mappingid,genderid) VALUES(@mappingid,@genderid);";
            UpdateProcedure = false;
            myupdatecommandtext = "UPDATE [spec].[MappingGender_tb] SET genderid=@genderid WHERE mappingid=@mappingid AND genderid=@genderoldid";
            DeleteProcedure = false;
            mydeletecommandtext = "DELETE FROM [spec].[MappingGender_tb] WHERE mappingid=@mappingid AND genderid=@genderoldid";
        }

        public int MappingId
        {
            get
            {
                return (int)SelectParams[0].Value;
            }
            set
            {
                SelectParams[0].Value = value;
                mydeleteparams[0].Value = value;
            }
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
		protected override MappingGender CreateRecord(SqlDataReader reader)
		{
            return new MappingGender(References.Genders.FindFirstItem("Id",reader.GetInt32(0)),lib.DomainObjectState.Unchanged);
		}
		protected override MappingGender CreateModel(MappingGender reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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
		protected override void GetOutputParametersValue(MappingGender item) { }
        protected override void ItemAcceptChanches(MappingGender item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(MappingGender item)
        {
           return true;
        }
        protected override bool SaveIncludedObject(MappingGender item)
        {
            bool isSuccess = true;
            if (item.Gender.DomainState == lib.DomainObjectState.Added)
            {
                GenderDBM gdbm = new GenderDBM();
                isSuccess = gdbm.SaveItemChanches(item.Gender);
                if (!isSuccess)
                {
                    foreach (lib.DBMError err in gdbm.Errors) this.Errors.Add(err);
                }
            }
            return isSuccess;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }

        protected override bool SetParametersValue(MappingGender item)
        {
            myinsertupdateparams[1].Value = item.Gender.Id;
            if (item.HasPropertyOutdatedValue("Gender"))
                myupdateparams[0].Value = (item.GetPropertyOutdatedValue("Gender") as Gender)?.Id;
            else
                myupdateparams[0].Value = item.Gender.Id;
            return true;
        }
    }

    public class MappingGenderVM : lib.ViewModelErrorNotifyItem<MappingGender>
    {
        public MappingGenderVM(MappingGender domain):base(domain)
        {
            ValidetingProperties.AddRange(new string[] { "Gender" });
            InitProperties();
        }
        public MappingGenderVM():this(new MappingGender()) { }

        private Gender mygender;
        public Gender Gender
        {
            set
            {
                if (!object.Equals(mygender, value) & base.IsEnabled)
                {
                    string name = "Name";
                    mygender = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Gender);
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name;
                        base.DomainObject.Gender = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return base.IsEnabled ? mygender : null; }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Gender":
                    mygender = this.DomainObject.Gender;
                    break;
            }
        }
        protected override void InitProperties()
        {
            mygender = this.DomainObject.Gender;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Gender":
                    if (mygender != this.DomainObject.Gender)
                        mygender = this.DomainObject.Gender;
                    else
                        this.Gender = (Gender)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Gender":
                    if (mygender==null)
                    {
                        errmsg = "Не указан пол";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return mygender!= base.DomainObject.Gender;
        }
    }

    internal class MappingGenderSynchronizer : lib.ModelViewCollectionsSynchronizer<MappingGender, MappingGenderVM>
    {
        protected override MappingGender UnWrap(MappingGenderVM wrap)
        {
            return wrap.DomainObject as MappingGender;
        }

        protected override MappingGenderVM Wrap(MappingGender fill)
        {
            return new MappingGenderVM(fill);
        }
    }

}
