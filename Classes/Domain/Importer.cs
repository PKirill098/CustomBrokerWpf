using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Importer : lib.DomainBaseStamp
    {
        public Importer(int id, long stamp, lib.DomainObjectState dstate
            ,string name) : base(id,stamp,null,null, dstate)
        {
            myname = name;
        }
        public Importer():this(lib.NewObjectId.NewId,0,lib.DomainObjectState.Added
            ,null) { }

        private string myname;
        public string Name
        {
            set { base.SetProperty<string>(ref myname, value); }
            get { return myname; }
        }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Importer newitem = (Importer)sample;
            this.Name = newitem.Name;
        }
    }

    internal class ImporterStore : lib.DomainStorage<Importer>
    {
        protected override void UpdateProperties(Importer olditem, Importer newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    internal class ImporterDBM : lib.DBManagerStamp<Importer>
    {
        internal ImporterDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.Importer_sp";
            InsertCommandText = "dbo.ImporterAdd_sp";
            UpdateCommandText = "dbo.ImporterUpd_sp";
            DeleteCommandText = "dbo.ImporterDel_sp";

            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@nametrue", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0]
                ,new SqlParameter("@name", System.Data.SqlDbType.NVarChar,200)
            };
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override Importer CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            Importer item = new Importer(reader.GetInt32(0),reader.GetInt64(1), lib.DomainObjectState.Unchanged
                ,reader.GetString(2));
            return item /*CustomBrokerWpf.References.ImporterStore.UpdateItem(item)*/;
        }
        protected override void GetOutputSpecificParametersValue(Importer item)
        {        }
        protected override bool SaveChildObjects(Importer item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Importer item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetSpecificParametersValue(Importer item)
        {
            myupdateparams[1].Value = item.HasPropertyOutdatedValue("Name");
            myinsertupdateparams[1].Value = item.Name;
            return true;
        }
        protected override void CancelLoad()
        { }
    }

    public class ImporterVM : lib.ViewModelErrorNotifyItem<Importer>
    {
        public ImporterVM(Importer item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { "Name" });
            DeleteRefreshProperties.AddRange(new string[] { "Name" });
            RejectPropertiesOrder.AddRange(new string[] { });
            InitProperties();
        }
        public ImporterVM() : this(new Importer()) { }

        private string myname;
        public string Name
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(myname, value)))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myname);
                    myname = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Name = value;
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
                    myname = this.DomainObject.Name;
                    break;
            }
        }
        protected override void InitProperties()
        {
            myname=this.DomainObject.Name;
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

                    break;
            }
            if (inform & !isvalid)
                AddErrorMessageForProperty(propertyname, errmsg);
            else if(isvalid)
                ClearErrorMessageForProperty(propertyname);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myname!= this.DomainObject.Name;
        }
    }

    public class ImporterSynchronizer : lib.ModelViewCollectionsSynchronizer<Importer, ImporterVM>
    {
        protected override Importer UnWrap(ImporterVM wrap)
        {
            return wrap.DomainObject as Importer;
        }
        protected override ImporterVM Wrap(Importer fill)
        {
            return new ImporterVM(fill);
        }
    }

    public class ImporterViewCommand : lib.ViewModelViewCommand
    {
        internal ImporterViewCommand()
        {
            //myidbm = new ImporterDBM();
            //myidbm.FillAsyncCompleted = () => { if (mydbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); };
            //myidbm.Fill();
            ImporterDBM idbm = new ImporterDBM();
            idbm.Collection = CustomBrokerWpf.References.Importers;
            mydbm = idbm;
            mysync = new ImporterSynchronizer();
            mysync.DomainCollection = idbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            base.DeleteQuestionHeader = "Удалить импортера?";
        }

        //private ImporterDBM myidbm;
        private ImporterSynchronizer mysync;

        protected override bool CanAddData(object parametr)
        {
            return true;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return true;
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override bool CanRejectChanges()
        {
            return true;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void OtherViewRefresh()
        {
        }
        protected override void RefreshData(object parametr)
        {
            CustomBrokerWpf.References.Importers.Refresh();
        }
        protected override void RejectChanges(object parametr)
        {
            System.Collections.IList rejects;
            if (parametr is System.Collections.IList && (parametr as System.Collections.IList).Count > 0)
                rejects = parametr as System.Collections.IList;
            else
                rejects = mysync.ViewModelCollection;

            System.Collections.Generic.List<ImporterVM> deleted = new System.Collections.Generic.List<ImporterVM>();
            foreach (object item in rejects)
            {
                if (item is ImporterVM)
                {
                    ImporterVM ritem = item as ImporterVM;
                    if (ritem.DomainState == lib.DomainObjectState.Added)
                        deleted.Add(ritem);
                    else
                    {
                        myview.EditItem(ritem);
                        ritem.RejectChanges();
                        myview.CommitEdit();
                    }
                }
            }
            foreach (ImporterVM delitem in deleted)
            {
                mysync.ViewModelCollection.Remove(delitem);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
            }
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class ImporterCollection : lib.ReferenceCollectionDomainBase<Importer>
    {
        public ImporterCollection() : this(new ImporterDBM()) { }
        public ImporterCollection(lib.DBManager<Importer> dbm) : base(dbm) { }

        public override Importer FindFirstItem(string propertyName, object value)
        {
            Importer first = null;
            foreach (Importer item in this)
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
                    default:
                        throw new NotImplementedException("Свойство " + propertyName + " не реализовано");
                }
            }
            return first;
        }
        protected override int CompareReferences(Importer item1, Importer item2)
        {
            return item1.Id.CompareTo(item2.Id);
        }
        protected override void UpdateItem(Importer olditem, Importer newitem)
        { olditem.UpdateProperties(newitem); }

        internal void DataLoad()
        { base.Fill(); }
    }

}
