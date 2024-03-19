using System;
using System.Data.SqlClient;
using System.Threading;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Gender : lib.DomainBaseNotifyChanged
    {
        public Gender(int id, int over, string name, string shortname,lib.DomainObjectState state) :base(id,state)
        {
            myoverid = over;
            myname = name;
            myshortname = shortname;
        }
        public Gender(int over) :this(lib.NewObjectId.NewId, over, string.Empty,string.Empty, lib.DomainObjectState.Added) { }

        private int myoverid;
        private Gender myover;
        public Gender Over
        {
            set
            {
                if (!object.Equals(myover, value))
                {
                    string name = "Over";
                    myover = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get
            {
                if(myover==null)
                {
                    myover = CustomBrokerWpf.References.Genders.FindFirstItem("Id", myoverid);
                }
                return myover;
            }
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
        private string myshortname;
        public string ShortName
        {
            set
            {
                if (!string.Equals(myshortname, value))
                {
                    string name = "ShortName";
                    myshortname = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myshortname; }
        }
    }

    internal class GenderDBM : lib.DBManager<Gender,Gender>
    {
        internal GenderDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectCommandText = "spec.Gender_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@param1", System.Data.SqlDbType.Int), new SqlParameter("@param2", System.Data.SqlDbType.Int), new SqlParameter("@param3", System.Data.SqlDbType.Int) };

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);

            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@over", System.Data.SqlDbType.Int)};
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,10),
                new SqlParameter("@shortname", System.Data.SqlDbType.NVarChar,5)
            };
            mydeleteparams = new SqlParameter[] { parid };

            InsertProcedure = true;
            myinsertcommandtext = "spec.GenderAdd_sp";
            UpdateProcedure = true;
            myupdatecommandtext = "spec.GenderUpd_sp";
            DeleteProcedure = true;
            mydeletecommandtext = "spec.GenderDel_sp";
        }

        public int? ItemId
        {
            get
            {
                return (int?)SelectParams[0].Value;
            }
            set
            {
                SelectParams[0].Value = value;
            }
        }
        public int? ItemOver
        {
            get
            {
                return (int?)SelectParams[1].Value;
            }
            set
            {
                SelectParams[1].Value = value;
            }
        }
        public int? ItemMapping
        {
            get
            {
                return (int?)SelectParams[2].Value;
            }
            set
            {
                SelectParams[2].Value = value;
            }
        }

        protected override void SetSelectParametersValue()
        {
        }
        protected override Gender CreateRecord(SqlDataReader reader)
        {
            return new Gender(reader.GetInt32(0),reader.GetInt32(1), reader.GetString(2), reader.GetString(3), lib.DomainObjectState.Unchanged);
        }
		protected override Gender CreateModel(Gender record, SqlConnection addcon, CancellationToken canceltasktoken = default)
		{
			return record;
		}
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, CancellationToken canceltasktoken = default)
		{
			base.TakeItem(this.CreateRecord(reader));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
		protected override void GetOutputParametersValue(Gender item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
            }
        }
        protected override void ItemAcceptChanches(Gender item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(Gender item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Gender item)
        {
            return item.Over?.DomainState!=lib.DomainObjectState.Added || this.SaveItemChanches(item);
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(Gender item)
        {
            myinsertparams[1].Value = item.Over?.Id;
            myupdateparams[0].Value = item.Id;
            myinsertupdateparams[0].Value = item.Name;
            myinsertupdateparams[1].Value = item.ShortName;
            return item.Over?.DomainState != lib.DomainObjectState.Added;
        }
    }

    public class GenderCollection : lib.ReferenceCollectionDomainBase<Gender>
    {
        public GenderCollection():this(new GenderDBM()) { }
        public GenderCollection(lib.DBManager<Gender, Gender> dbm) : base(dbm) { }

        public override Gender FindFirstItem(string propertyName, object value)
        {
            Gender first = null;
            foreach (Gender item in this)
            {
                switch (propertyName)
                {
                    case "Id":
                        if (item.Id == (int)value)
                            first = item;
                        break;
                    case "Name":
                        if (item.Name.ToUpper().Equals(((string)value).ToUpper()))
                            first = item;
                        break;
                    case "ShortName":
                        if (item.ShortName.ToUpper().Equals(((string)value).ToUpper()))
                            first = item;
                        break;
                    default:
                        throw new NotImplementedException("Свойство " + propertyName + " не реализовано");
                }
            }
            return first;
        }
        protected override int CompareReferences(Gender item1, Gender item2)
        {
            return item1.Id.CompareTo(item2.Id);
        }
        protected override void UpdateItem(Gender olditem, Gender newitem) {}

        internal void DataLoad()
        { base.Fill(); }
    }

    public class GenderVM : lib.ViewModelErrorNotifyItem<Gender>,IDisposable
    {
        public GenderVM(Gender domain) :base(domain)
        {
            ValidetingProperties.AddRange(new string[] { "Name", "ShortName" });
            mysync = new GenderSynchronizer();
            InitProperties();
        }

        public void Dispose()
        {
            CustomBrokerWpf.References.Genders.RefreshViewRemove(myunder);
        }

        public GenderVM(int over) : this(new Gender(over)) { }

        public Gender Over
        { get { return this.DomainObject.Over; } }
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
        private string myshortname;
        public string ShortName
        {
            set
            {
                if (!string.Equals(myshortname, value) & base.IsEnabled)
                {
                    string name = "ShortName";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.ShortName);
                    myshortname = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name;
                        base.DomainObject.ShortName = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return base.IsEnabled ? myshortname : null; }
        }
        private GenderSynchronizer mysync;
        private System.Windows.Data.ListCollectionView myunder;
        public System.Windows.Data.ListCollectionView Under
        {
            get
            {
                if (myunder == null)
                {
                    mysync.DomainCollection = CustomBrokerWpf.References.Genders;
                    myunder = new System.Windows.Data.ListCollectionView(mysync.ViewModelCollection);
                    myunder.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && object.Equals((item as GenderVM).DomainObject.Over, this.DomainObject); };
                    CustomBrokerWpf.References.Genders.RefreshViewAdd(myunder);
                }
                return myunder;
            }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Name":
                    myname = this.DomainObject.Name;
                    break;
                case "ShortName":
                    myshortname = this.DomainObject.ShortName;
                    break;
            }
        }
        protected override void InitProperties()
        {
            myname = this.DomainObject.Name;
            myshortname = this.DomainObject.ShortName;
        }
        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
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
                case "ShortName":
                    if (string.IsNullOrEmpty(myshortname))
                    {
                        errmsg = "Отсутствует краткое наименование";
                        isvalid = false;
                    }
                    break;
                //case "Dependent":
                //    if(myunder!=null)
                //    foreach (GenderVM item in myunder)
                //        if (item != null) isvalid &= item.Validate(inform);
                //    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myname!=this.DomainObject.Name || myshortname!=this.DomainObject.ShortName;
        }
    }

    internal class GenderSynchronizer : lib.ModelViewCollectionsSynchronizer<Gender, GenderVM>
    {
        protected override Gender UnWrap(GenderVM wrap)
        {
            return wrap.DomainObject as Gender;
        }

        protected override GenderVM Wrap(Gender fill)
        {
            return new GenderVM(fill);
        }
    }

    public class GenderViewCommand : lib.ViewModelViewCommand, IDisposable
    {
        internal GenderViewCommand():base()
        {
            mysync = new GenderSynchronizer();
            mysync.DomainCollection = CustomBrokerWpf.References.Genders;
            myview = new System.Windows.Data.ListCollectionView(mysync.ViewModelCollection);
            CustomBrokerWpf.References.Genders.RefreshViewAdd(myview);
            SettingView();
            myexhandler = new DataModelClassLibrary.ExceptionHandler("Сохранение");
        }

        public void Dispose()
        {
            CustomBrokerWpf.References.Genders.RefreshViewRemove(myview);
        }

        private GenderSynchronizer mysync;
        private new GenderDBM mydbm;
        private lib.ExceptionHandler myexhandler;

        public bool IsDirtyTree
        {
            get
            {
                bool isdirty = false;
                foreach (GenderVM item in myview)
                {
                    if (item is GenderVM)
                    {
                        isdirty = IsDirtyHierarchy(item);
                        if (isdirty)
                            break;
                    }
                }
                return isdirty;
            }
        }
        private bool IsDirtyHierarchy(GenderVM top)
        {
            if (top.IsDirty)
                return true;
           else
            {
                bool isdirty = false;
                foreach (GenderVM item in top.Under)
                {
                    isdirty = IsDirtyHierarchy(item);
                    if (isdirty)
                        break;
                }
                return isdirty;
            }
        }

        public override bool SaveDataChanges()
        {
            bool isSuccess;
            if (mydbm == null)
            {
                mydbm = new GenderDBM();
                mydbm.Collection = CustomBrokerWpf.References.Genders;
            }
            else
                mydbm.Errors.Clear();
            string str = ValidateTree();
            isSuccess = mydbm.SaveCollectionChanches() & string.IsNullOrEmpty(str);
            if (!isSuccess)
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                err.Append(str);
                err.AppendLine(mydbm.ErrorMessage);
                myexhandler.Handle(new Exception(err.ToString()));
                myexhandler.ShowMessage();
            }
            return isSuccess;
        }
        internal string ValidateTree()
        {
            string str = string.Empty;
            foreach (GenderVM item in myview)
            {
                if (item is GenderVM)
                {
                    str = ValidateHierarchy(item);
                    if (!string.IsNullOrEmpty(str))
                        break;
                }
            }
            return str;
        }
        private string ValidateHierarchy(GenderVM top)
        {
            if (top.Validate(true))
            {
                string str=string.Empty;
                foreach (GenderVM item in top.Under)
                {
                    str= ValidateHierarchy(item);
                    if (!string.IsNullOrEmpty(str))
                        break;
                }
                return str;
            }
            else
                return top.Errors;
        }

        protected override void AddData(object parametr)
        {
            System.Windows.Data.ListCollectionView view;
            if (parametr is GenderVM)
            {
                GenderVM over = parametr as GenderVM;
                view = over.Under;
                view.AddNewItem(new GenderVM(over.DomainObject.Id));
            }
            else
            {
                view = myview;
                view.AddNewItem(new GenderVM(0));
            }
            view.CommitNew();
        }
        protected override bool CanAddData(object parametr)
        {
            return true;
        }
        protected override void DeleteData(object parametr)
        {
            if (parametr is GenderVM)
            {
                System.Windows.Data.ListCollectionView view;
                GenderVM item = parametr as GenderVM;
                if (item.Over != null)
                    view = GetView(item.Over, null);
                else
                    view = myview;

                if (item.DomainState == lib.DomainObjectState.Added)
                {
                    item.DomainState = lib.DomainObjectState.Destroyed;
                    if (view.IsAddingNew && item == view.CurrentAddItem)
                        view.CancelNew();
                    else
                        view.Remove(item);
                }
                else
                {
                    view.EditItem(item);
                    item.DomainState = lib.DomainObjectState.Deleted;
                    view.CommitEdit();
                }
            }
        }
        protected override bool CanDeleteData(object parametr)
        {
            return parametr!=null;
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override bool CanRejectChanges()
        {
            return false;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void OtherViewRefresh()
        {
            CustomBrokerWpf.References.Genders.Refresh();
        }
        protected override void RefreshData(object parametr)
        {
            CustomBrokerWpf.References.Genders.Refresh();
        }
        protected override void RejectChanges(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override void SettingView()
        {
            myview.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as GenderVM).DomainObject.Over ==null; };
        }

        private System.Windows.Data.ListCollectionView GetView(Gender item, System.Windows.Data.ListCollectionView view)
        {
            if (view == null) view = myview;
            foreach(GenderVM vmitem in view)
            {
                if (object.Equals(vmitem.DomainObject, item))
                {
                    view = vmitem.Under;
                    break;
                }
                else
                    GetView(item, vmitem.Under);
            }
            return view;
        }
    }
}
