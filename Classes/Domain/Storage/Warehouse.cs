using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Storage
{
    public class Warehouse : lib.DomainBaseUpdate
    {
        public Warehouse(int id, lib.DomainObjectState domainstate
            ,string name
            ):base(id, domainstate)
        {
            myname = name;
        }
        public Warehouse() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added, null) { }

        private string myname;
        public string Name
        {
            set { SetProperty<string>(ref myname, value); }
            get { return myname; }
        }

        private ObservableCollection<WarehouseAddress> myaddresses;
        internal ObservableCollection<WarehouseAddress> Addresses
        {
            get
            {
                if (myaddresses == null)
                {
                    myaddresses = new ObservableCollection<WarehouseAddress>();
                    WarehouseAddressDBM ldbm = new WarehouseAddressDBM();
                    ldbm.Warehouse = this;
                    ldbm.Collection = myaddresses;
                    ldbm.Fill();
                    myaddresses.CollectionChanged += WarehouseAddresses_CollectionChanged;
                }
                return myaddresses;
            }
        }
        private void WarehouseAddresses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (WarehouseAddress item in e.NewItems)
                    item.Warehouse = this;
        }
        internal bool AddressesIsNull
        { get { return myaddresses == null; } }
        private System.Collections.ObjectModel.ObservableCollection<WarehouseContact> mycontacts;
        internal System.Collections.ObjectModel.ObservableCollection<WarehouseContact> Contacts
        {
            get
            {
                if (mycontacts == null)
                {
                    mycontacts = new System.Collections.ObjectModel.ObservableCollection<WarehouseContact>();
                    WarehouseContactDBM ldbm = new WarehouseContactDBM();
                    ldbm.Warehouse = this;
                    ldbm.Collection = mycontacts;
                    ldbm.Fill();
                    mycontacts.CollectionChanged += Contact_CollectionChanged;
                }
                return mycontacts;
            }
        }
        private void Contact_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (WarehouseContact item in e.NewItems)
                    item.Warehouse = this;
        }
        internal bool ContactsIsNull
        { get { return mycontacts == null; } }

        protected override void PropertiesUpdate(DomainBaseUpdate sample)
        {
            Warehouse templ = sample as Warehouse;
            this.Name = templ.Name;
        }
    }

    public class WarehouseDBM : lib.DBManagerId<Warehouse>
    {
        public WarehouseDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            base.NeedAddConnection = true;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.Warehouse_sp";
            InsertCommandText = "dbo.WarehouseAdd_sp";
            UpdateCommandText = "dbo.WarehouseUpd_sp";
            DeleteCommandText = "dbo.WarehouseDel_sp";

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);
            myinsertparams = new SqlParameter[] { paridout };
            myupdateparams = new SqlParameter[] {
                parid
            };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,15)
            };
            mydeleteparams = new SqlParameter[] { parid };

            myaddbm = new WarehouseAddressDBM();
            mycdbm = new WarehouseContactDBM();
        }

        private WarehouseAddressDBM myaddbm;
        private WarehouseContactDBM mycdbm;

        protected override void CancelLoad()
        {
        }
        protected override Warehouse CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            Warehouse warehouse = new Warehouse(reader.GetInt32(this.Fields["id"]), lib.DomainObjectState.Unchanged, reader.GetString(this.Fields["name"]));
            if (this.FillType == lib.FillType.Refresh)
            {
                if (!warehouse.AddressesIsNull & myaddbm != null)
                {
                    myaddbm.Errors.Clear();
                    myaddbm.Command.Connection = addcon;
                    myaddbm.Warehouse = warehouse;
                    myaddbm.Collection = warehouse.Addresses;
                    myaddbm.Fill();
                    foreach (lib.DBMError err in myaddbm.Errors) this.Errors.Add(err);
                }
                if (!warehouse.ContactsIsNull & mycdbm != null)
                {
                    mycdbm.Errors.Clear();
                    mycdbm.Command.Connection = addcon;
                    mycdbm.Warehouse = warehouse;
                    mycdbm.Collection = warehouse.Contacts;
                    mycdbm.Fill();
                    foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
                }
            }
            return warehouse;
        }
        protected override void GetOutputParametersValue(Warehouse item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(Warehouse item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(Warehouse item)
        {
            bool success = true;
            if (myaddbm != null)
            {
                myaddbm.Errors.Clear();
                myaddbm.Collection = item.Addresses;
                if (!myaddbm.SaveCollectionChanches())
                {
                    success = false;
                    foreach (lib.DBMError err in myaddbm.Errors) this.Errors.Add(err);
                }
            }
            if (mycdbm != null)
            {
                mycdbm.Errors.Clear();
                mycdbm.Collection = item.Contacts;
                if (!mycdbm.SaveCollectionChanches())
                {
                    success = false;
                    foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
                }
            }
            return success;
        }
        protected override bool SaveIncludedObject(Warehouse item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            if (myaddbm != null)
                myaddbm.Command.Connection = this.Command.Connection;
            if (mycdbm != null)
                mycdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetParametersValue(Warehouse item)
        {
            foreach (SqlParameter par in this.InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@name":
                        par.Value = item.Name;
                        break;
                }
            foreach (SqlParameter par in this.UpdateParams)
                switch (par.ParameterName)
                {
                    case "@id":
                        par.Value = item.Id;
                        break;
                }
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
    }

    public class WarehouseVM: lib.ViewModelErrorNotifyItem<Warehouse>
    {
        public WarehouseVM(Warehouse model) : base(model)
        {
            DeleteRefreshProperties.AddRange(new string[] { nameof(this.Name) });
            ValidetingProperties.AddRange(new string[] { nameof(this.Name) });
            InitProperties();
        }
        public WarehouseVM() : this(new Warehouse()) { }

        public string Name
        {
            set
            {
                if (!string.IsNullOrEmpty(value) && !(this.IsReadOnly || string.Equals(this.DomainObject.Name, value)))
                {
                    string name = nameof(this.Name);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Name);
                    ChangingDomainProperty = name; this.DomainObject.Name = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Name : null; }
        }

        private WarehouseAddressSynchronizer myadrsync;
        private ListCollectionView myaddresses;
        public ListCollectionView Addresses
        {
            get
            {
                if (myaddresses == null)
                {
                    if (myadrsync == null)
                    {
                        myadrsync = new WarehouseAddressSynchronizer();
                        myadrsync.DomainCollection = this.DomainObject.Addresses;
                    }
                    myaddresses = new ListCollectionView(myadrsync.ViewModelCollection);
                    myaddresses.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                }
                return myaddresses;
            }
        }
        private WarehouseContactSynchronizer mycntsync;
        private ListCollectionView mycontacts;
        public ListCollectionView Contacts
        {
            get
            {
                if (mycontacts == null)
                {
                    if (mycntsync == null)
                    {
                        mycntsync = new WarehouseContactSynchronizer();
                        mycntsync.DomainCollection = this.DomainObject.Contacts;
                    }
                    mycontacts = new ListCollectionView(mycntsync.ViewModelCollection);
                    mycontacts.Filter = lib.ViewModelViewCommand.ViewFilterDefault;

                }
                return mycontacts;
            }
        }

        protected override bool DirtyCheckProperty()
        {
            return string.IsNullOrEmpty(this.DomainObject.Name);
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
        }
        protected override void InitProperties()
        {
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Name):
                    this.DomainObject.Name = (string)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    WarehouseAddressVM[] aremoved = new WarehouseAddressVM[this.DomainObject.Addresses.Count];
                    foreach (WarehouseAddressVM litem in this.Addresses)
                    {
                        if (litem.DomainState == lib.DomainObjectState.Added)
                        {
                            aremoved[i] = litem;
                            i++;
                        }
                        else
                            litem.RejectChanges();
                    }
                    foreach (WarehouseAddressVM litem in aremoved)
                        if (litem != null) this.Addresses.Remove(litem);
                    i = 0;
                    WarehouseContactVM[] cremoved = new WarehouseContactVM[this.DomainObject.Contacts.Count];
                    foreach (WarehouseContactVM litem in this.Contacts)
                    {
                        if (litem.DomainState == lib.DomainObjectState.Added)
                        {
                            cremoved[i] = litem;
                            i++;
                        }
                        else
                            litem.RejectChanges();
                    }
                    foreach (WarehouseContactVM litem in cremoved)
                        if (litem != null) this.Contacts.Remove(litem);
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.Name):
                    if (string.IsNullOrEmpty(this.DomainObject.Name))
                    {
                        errmsg = "Наименование склада не может быть пустым!";
                        isvalid = false;
                    }
                    break;
            }
            if (isvalid) ClearErrorMessageForProperty(propertyname);
            else if (inform) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    internal class WarehouseSynchronizer : lib.ModelViewCollectionsSynchronizer<Warehouse, WarehouseVM>
    {
        protected override Warehouse UnWrap(WarehouseVM wrap)
        {
            return wrap.DomainObject as Warehouse;
        }
        protected override WarehouseVM Wrap(Warehouse fill)
        {
            return new WarehouseVM(fill);
        }
    }

    public class WarehouseCommand : lib.ViewModelCommand<Warehouse, WarehouseVM, WarehouseDBM>
    {
        public WarehouseCommand(WarehouseVM vm, ListCollectionView view) : base(vm, view)
        {
            try
            {
                ReferenceDS referenceDS = CustomBrokerWpf.References.ReferenceDS;
                if (referenceDS.tableTown.Count == 0)
                {
                    ReferenceDSTableAdapters.TownAdapter thisTownAdapter = new ReferenceDSTableAdapters.TownAdapter();
                    thisTownAdapter.Fill(referenceDS.tableTown);
                }
                mytowns = new System.Data.DataView(referenceDS.tableTown, string.Empty, string.Empty, System.Data.DataViewRowState.Unchanged | System.Data.DataViewRowState.ModifiedCurrent);
                myaddresstypes = new ListCollectionView(CustomBrokerWpf.References.StoreAddressTypes);
                mycontacttypes = new ListCollectionView(CustomBrokerWpf.References.StoreContactTypes);
                if (referenceDS.ContactPointTypeTb.Count == 0)
                {
                    ReferenceDSTableAdapters.ContactPointTypeAdapter thisTypeAdapter = new ReferenceDSTableAdapters.ContactPointTypeAdapter();
                    thisTypeAdapter.Fill(referenceDS.ContactPointTypeTb);
                }
                mypointtypes = new System.Data.DataView(referenceDS.ContactPointTypeTb, string.Empty, string.Empty, System.Data.DataViewRowState.Unchanged | System.Data.DataViewRowState.ModifiedCurrent);
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                    {
                        errs.Append(sqlerr.Message + "\n");
                    }
                    this.OpenPopup("Загрузка данных\n" + errs.ToString(), true);
                }
                else
                {
                    this.OpenPopup("Загрузка данных\n" + ex.Message + "\n" + ex.Source, true);
                }
            }

            base.DeleteQuestionHeader = "Удалить склад?";
            mydbm = new WarehouseDBM();
            mydbm.FillType = lib.FillType.Refresh;
        }

        private ListCollectionView myaddresstypes;
        public ListCollectionView AddressTypes
        { get { return myaddresstypes; } }
        private ListCollectionView mycontacttypes;
        public ListCollectionView ContactTypes
        { get { return mycontacttypes; } }
        private System.Data.DataView mypointtypes;
        public System.Data.DataView ContactPointTypes
        { get { return mypointtypes; } }
        private ListCollectionView mystates;
        public ListCollectionView States
        {
            get
            {
                if (mystates == null)
                {
                    mystates = new ListCollectionView(CustomBrokerWpf.References.CustomerRowStates);
                }
                return mystates;
            }
        }
        private System.Data.DataView mytowns;
        public System.Data.DataView Towns
        { get { return mytowns; } }

        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void RefreshData(object parametr)
        {
            mydbm.ItemId = this.VModel.DomainObject.Id;
            mydbm.GetFirst();
            this.PopupText = mydbm.ErrorMessage;
        }
        public override bool SaveDataChanges()
        {
            bool succses = base.SaveDataChanges();
            CustomBrokerWpf.References.Stores.Refresh();
            CustomBrokerWpf.References.Stores.RefreshViews();
            return succses;
        }
    }

    public class WarehouseViewCommander : lib.ViewModelViewCommand
    {
        internal WarehouseViewCommander()
        {
            mywdbm = new WarehouseDBM();
            mydbm = mywdbm;
            mysync = new WarehouseSynchronizer();
            mywdbm.Fill();
            if (mydbm.Errors.Count > 0)
                this.OpenPopup("Загрузка данных\n" + mydbm.ErrorMessage, true);
            mywdbm.FillType = lib.FillType.Refresh;
            mysync.DomainCollection = mywdbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            base.DeleteQuestionHeader = "Удалить склад?";
        }

        WarehouseDBM mywdbm;
        WarehouseSynchronizer mysync;

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
            CustomBrokerWpf.References.Stores.Refresh();
            CustomBrokerWpf.References.Stores.RefreshViews();
        }
        protected override void RefreshData(object parametr)
        {
            mywdbm.Fill();
            CustomBrokerWpf.References.Stores.Refresh();
            CustomBrokerWpf.References.Stores.RefreshViews();
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }
}
