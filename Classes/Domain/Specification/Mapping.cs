using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class Mapping : lib.DomainBaseStamp
    {
        public Mapping(int id, string goods, string tnvedgroup, Material material
            , Int64 stamp, DateTime? updated, string updater, lib.DomainObjectState domainstate) : base(id, stamp, updated, updater, domainstate)
        {
            mygoods = goods;
            mytnvedgroup = tnvedgroup;
            mymaterial = material;
        }
        public Mapping() : this(lib.NewObjectId.NewId, string.Empty, string.Empty, null, 0, null, null, lib.DomainObjectState.Added) { }

        private string mygoods;
        public string Goods
        {
            set
            {
                if (!string.Equals(mygoods, value))
                {
                    string name = "Goods";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mygoods);
                    mygoods = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mygoods; }
        }
        private string mytnvedgroup;
        public string TNVEDGroup
        {
            set
            {
                if (!string.Equals(mytnvedgroup, value))
                {
                    string name = "TNVEDGroup";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mytnvedgroup);
                    mytnvedgroup = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mytnvedgroup; }
        }
        private Material mymaterial;
        public Material Material
        {
            set
            {
                if (!object.Equals(mymaterial, value))
                {
                    string name = "Material";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mymaterial);
                    mymaterial = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mymaterial; }
        }

        private GoodsSynonymDBM gsdbm;
        private ObservableCollection<GoodsSynonym> mysynonyms;
        public ObservableCollection<GoodsSynonym> Synonyms
        {
            get
            {
                if (mysynonyms == null)
                {
                    gsdbm = new GoodsSynonymDBM();
                    gsdbm.Mapping = this;
                    gsdbm.Fill();
                    mysynonyms = gsdbm.Collection;
                }
                return mysynonyms;
            }
        }
        private MappingGenderDBM gdbm;
        private ObservableCollection<MappingGender> mygenders;
        public ObservableCollection<MappingGender> Genders
        {
            get
            {
                if (mygenders == null)
                {
                    gdbm = new MappingGenderDBM();
                    gdbm.MappingId = this.Id;
                    gdbm.Fill();
                    mygenders = gdbm.Collection;
                }
                return mygenders;
            }
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Goods":
                    mygoods = (string)value;
                    break;
                case "TNVEDGroup":
                    mytnvedgroup = (string)value;
                    break;
                case "Material":
                    mymaterial = (Material)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    if (mysynonyms != null)
                    {
                        GoodsSynonym[] additem = new GoodsSynonym[mysynonyms.Count];
                        foreach (GoodsSynonym item in mysynonyms)
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                additem[i] = item; i++;
                            }
                        //else
                        //    item.RejectChanges();
                        for (int ii = 0; ii < i; ii++) mysynonyms.Remove(additem[ii]);
                    }
                    i = 0;
                    if (mygenders != null)
                    {
                        MappingGender[] additem = new MappingGender[mygenders.Count];
                        foreach (MappingGender item in mygenders)
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                additem[i] = item; i++;
                            }
                            else
                                item.RejectChanges();
                        for (int ii = 0; ii < i; ii++) mygenders.Remove(additem[ii]);
                    }
                    break;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            throw new NotImplementedException();
        }
    }

    public class MappingDBM : lib.DBManagerWhoWhen<Mapping, Mapping>
    {
        public MappingDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0],myinsertupdateparams[1],
                new SqlParameter("@goods", System.Data.SqlDbType.NVarChar, 50),
                new SqlParameter("@tnvedgroup", System.Data.SqlDbType.Char, 4),
                new SqlParameter("@materialid", System.Data.SqlDbType.Int),
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0],
                new SqlParameter("@goodstrue", System.Data.SqlDbType.Bit),
                new SqlParameter("@tnvedgrouptrue", System.Data.SqlDbType.Bit),
                new SqlParameter("@materialidtrue", System.Data.SqlDbType.Bit)
            };
            SelectProcedure = true;
            SelectCommandText = "spec.Mapping_sp";
            InsertProcedure = true;
            myinsertcommandtext = "spec.MappingAdd_sp";
            UpdateProcedure = true;
            myupdatecommandtext = "spec.MappingUpd_sp";
            DeleteProcedure = true;
            mydeletecommandtext = "spec.MappingDel_sp";
            gsdbm = new GoodsSynonymDBM(); gsdbm.Command = new SqlCommand();
            mgdbm = new MappingGenderDBM(); mgdbm.Command = new SqlCommand();
        }

        private GoodsSynonymDBM gsdbm;
        private MappingGenderDBM mgdbm;

        protected override void SetSelectParametersValue()
        {
        }
		protected override Mapping CreateRecord(SqlDataReader reader)
		{
            return new Mapping(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.IsDBNull(3) ? null : References.Materials.FindFirstItem("Id", reader.GetInt32(3)),
                reader.GetInt64(4),
                reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                lib.DomainObjectState.Unchanged);
		}
		protected override Mapping CreateModel(Mapping reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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
		protected override bool SaveChildObjects(Mapping item)
        {
            bool isSuccess = true;
            gsdbm.Errors.Clear();
            mgdbm.Errors.Clear();
            gsdbm.Mapping = item;
            mgdbm.MappingId = item.Id;
            gsdbm.Collection = item.Synonyms;
            mgdbm.Collection = item.Genders;
            if (!gsdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in gsdbm.Errors) this.Errors.Add(err);
            }
            if (!mgdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in mgdbm.Errors) this.Errors.Add(err);
            }

            return isSuccess;
        }
        protected override bool SaveIncludedObject(Mapping item)
        {
            bool isSuccess = true;
            if (item.Material!=null && item.Material.DomainState==lib.DomainObjectState.Added)
            {
                MaterialDBM mdbm = new MaterialDBM();
                mdbm.Command = new SqlCommand() { Connection = this.Command.Connection };
                isSuccess = mdbm.SaveItemChanches(item.Material);
                if(!isSuccess)
                {
                    foreach (lib.DBMError err in mdbm.Errors) this.Errors.Add(err);
                }
            }
            return isSuccess;
        }
        protected override bool SaveReferenceObjects()
        {
            gsdbm.Command.Connection = this.Command.Connection;
            mgdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetParametersValue(Mapping item)
        {
            base.SetParametersValue(item);
            myinsertupdateparams[2].Value=item.Goods;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue("Goods");
            myinsertupdateparams[3].Value = item.TNVEDGroup;
            myupdateparams[2].Value = item.HasPropertyOutdatedValue("TNVEDGroup");
            myinsertupdateparams[4].Value = item.Material != null?(object)item.Material.Id:DBNull.Value;
            myupdateparams[3].Value = item.HasPropertyOutdatedValue("Material");
            return true;
        }
    }

    public class MappingVM : lib.ViewModelErrorNotifyItem<Mapping>
    {
        public MappingVM(Mapping domain):base(domain)
        {
            ValidetingProperties.AddRange(new string[] { "Goods", "TNVEDGroup" });
            DeleteRefreshProperties.AddRange(new string[] { "Goods", "TNVEDGroup", "Material", "Synonyms", "Genders" });
            domain.Synonyms.CollectionChanged += Synonyms_CollectionChanged;
            domain.Genders.CollectionChanged += Genders_CollectionChanged;
            InitProperties();
        }
        public MappingVM() : this(new Mapping()) { }

        private GoodsSynonymSynchronizer gssync;
        private MappingGenderSynchronizer mgsync;

        private string mygoods;
        public string Goods
        {
            set
            {
                if (!string.Equals(mygoods, value) & base.IsEnabled)
                {
                    string name = "Goods";
                    mygoods = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Goods);
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name;
                        base.DomainObject.Goods = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return base.IsEnabled ? mygoods : null; }
        }
        private string mytnvedgroup;
        public string TNVEDGroup
        {
            set
            {
                if (!string.Equals(mytnvedgroup, value) & base.IsEnabled)
                {
                    string name = "TNVEDGroup";
                    mytnvedgroup = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.TNVEDGroup);
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name;
                        base.DomainObject.TNVEDGroup = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return base.IsEnabled ? mytnvedgroup : null; }
        }
        public Material Material
        {
            set
            {
                if (!string.Equals(base.DomainObject.Material, value) & base.IsEnabled)
                {
                    string name = "Material";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Material);
                    ChangingDomainProperty = name; base.DomainObject.Material = value;
                }
            }
            get { return base.IsEnabled ? base.DomainObject.Material : null; }
        }
        private string mysynonymsstr;
        public string SynonymsStr
        { private set { mysynonymsstr = value; PropertyChangedNotification("SynonymsStr");} get { return mysynonymsstr; } }
        private string mygendersstr;
        public string GendersStr
        { set { mygendersstr = value; PropertyChangedNotification("GendersStr"); } get { return mygendersstr; } }
        private System.Windows.Data.ListCollectionView mysynonyms;
        public System.Windows.Data.ListCollectionView Synonyms
        {
            get
            {
                if (mysynonyms == null)
                {
                    gssync = new GoodsSynonymSynchronizer();
                    gssync.DomainCollection = this.DomainObject.Synonyms;
                    mysynonyms = new System.Windows.Data.ListCollectionView(gssync.ViewModelCollection);
                    mysynonyms.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    mysynonyms.CurrentChanged += MySynonyms_CurrentChanged;
                }
                return base.IsEnabled ? mysynonyms : null;
            }
        }
        public void DeleteSynonym(object parametr)
        {
            if(parametr is GoodsSynonym)
            {
                mysynonyms.EditItem(parametr);
                (parametr as GoodsSynonym).DomainState = (parametr as GoodsSynonym).DomainState == lib.DomainObjectState.Added ? lib.DomainObjectState.Destroyed: lib.DomainObjectState.Deleted;
                mysynonyms.CommitEdit();
            }
        }
        private System.Windows.Data.ListCollectionView mygenders;
        public System.Windows.Data.ListCollectionView Genders
        {
            get
            {
                if (mygenders == null)
                {
                    mgsync = new MappingGenderSynchronizer();
                    mgsync.DomainCollection = this.DomainObject.Genders;
                    mygenders = new System.Windows.Data.ListCollectionView(mgsync.ViewModelCollection);
                    mygenders.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    mygenders.CurrentChanged += MyGenders_CurrentChanged;
                }
                return base.IsEnabled ? mygenders : null;
            }
        }

        private void MySynonyms_CurrentChanged(object sender, EventArgs e)
        {
            SynonymsChanged();
        }
        private void MyGenders_CurrentChanged(object sender, EventArgs e)
        {
            GendersChanged();
        }
        private void Synonyms_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SynonymsChanged();
        }
        private void Genders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GendersChanged();
        }

        public void SynonymsChanged()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (GoodsSynonym item in this.DomainObject.Synonyms)
                str.Append(", "+item.Name);
            if (str.Length > 0) str.Remove(0, 2);
            this.SynonymsStr = str.ToString();
        }
        public void GendersChanged()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (MappingGender item in this.DomainObject.Genders)
                str.Append(", " + item.Gender?.Name);
            if(str.Length>0) str.Remove(0, 2);
            this.GendersStr = str.ToString();
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Goods":
                    mygoods = this.DomainObject.Goods;
                    break;
                case "TNVEDGroup":
                    mytnvedgroup = this.DomainObject.TNVEDGroup;
                    break;
            }
        }
        protected override void InitProperties()
        {
            mygoods = this.DomainObject.Goods;
            mytnvedgroup = this.DomainObject.TNVEDGroup;
            GendersChanged();
            SynonymsChanged();
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Goods":
                    if (mygoods != this.DomainObject.Goods)
                        mygoods = this.DomainObject.Goods;
                    else
                        this.Goods = (string)value;
                    break;
                case "TNVEDGroup":
                    if (mytnvedgroup != this.DomainObject.TNVEDGroup)
                        mytnvedgroup = this.DomainObject.TNVEDGroup;
                    else
                        this.TNVEDGroup = (string)value;
                    break;
                case "Material":
                    this.DomainObject.Material = (Material)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    if (mysynonyms != null)
                    {
                        GoodsSynonymVM[] additem = new GoodsSynonymVM[mysynonyms.Count];
                        foreach (GoodsSynonymVM item in mysynonyms.SourceCollection)
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                additem[i] = item; i++;
                            }
                            else if (item.DomainState == lib.DomainObjectState.Deleted)
                            {
                                mysynonyms.EditItem(item);
                                item.DomainState = lib.DomainObjectState.Modified;
                                mysynonyms.CommitEdit();
                            }
                            else
                                item.RejectChanges();
                        for (int ii = 0; ii < i; ii++) mysynonyms.Remove(additem[ii]);
                    }
                    i = 0;
                    if (mygenders != null)
                    {
                        MappingGenderVM[] additem = new MappingGenderVM[mygenders.Count];
                        foreach (MappingGenderVM item in mygenders.SourceCollection)
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                additem[i] = item; i++;
                            }
                            else
                            {
                                mygenders.EditItem(item);
                                item.RejectChanges();
                                mygenders.CommitEdit();
                            }
                        for (int ii = 0; ii < i; ii++) mygenders.Remove(additem[ii]);
                    }
                    SynonymsChanged();
                    GendersChanged();
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Goods":
                    if (string.IsNullOrEmpty(mygoods))
                    {
                        errmsg = "Отсутствует товар";
                        isvalid = false;
                    }
                    break;
                case "TNVEDGroup":
                    if (string.IsNullOrEmpty(mytnvedgroup))
                    {
                        errmsg = "Отсутствует группа ТН ВЭД";
                        isvalid = false;
                    }
                    break;
                case "Dependent":
                    foreach (MappingGenderVM item in this.Genders)
                        if(item!=null) isvalid &= item.Validate(inform);
                    foreach (GoodsSynonymVM item in this.Synonyms)
                        if (item != null) isvalid &= item.Validate(inform);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return mygoods!= base.DomainObject.Goods || mytnvedgroup!= base.DomainObject.TNVEDGroup;
        }
    }

    internal class MappingSynchronizer : lib.ModelViewCollectionsSynchronizer<Mapping, MappingVM>
    {
        protected override Mapping UnWrap(MappingVM wrap)
        {
            return wrap.DomainObject as Mapping;
        }

        protected override MappingVM Wrap(Mapping fill)
        {
            return new MappingVM(fill);
        }
    }

    public class MappingCommand : lib.ViewModelCommand<Mapping,Mapping,MappingVM, MappingDBM>
    {
        internal MappingCommand(MappingVM vm, System.Windows.Data.ListCollectionView view) :base(vm, view)
        {
            mymaterials = new System.Windows.Data.ListCollectionView(References.Materials);
            mymaterials.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
        internal MappingCommand(System.Windows.Data.ListCollectionView view) : this(new MappingVM(),view) { }

        private System.Windows.Data.ListCollectionView mymaterials;
        public System.Windows.Data.ListCollectionView Materials
        {
            get { return mymaterials; }
        }
        public GenderCollection Genders
        {
            get { return References.Genders; }
        }

        internal void SynonymsDeleteExec(object parametr)
        {
            if (parametr is System.Collections.IList)
            {
                List<GoodsSynonymVM> items = new List<GoodsSynonymVM>();
                foreach (GoodsSynonymVM item in parametr as System.Collections.IList)
                    items.Add(item);
                foreach (GoodsSynonymVM item in items)
                {
                    if (item.DomainState == lib.DomainObjectState.Added)
                    {
                        item.DomainState = lib.DomainObjectState.Destroyed;
                        if (VModel.Synonyms.IsAddingNew && item == VModel.Synonyms.CurrentAddItem)
                            VModel.Synonyms.CancelNew();
                        else
                            VModel.Synonyms.Remove(item);
                    }
                    else
                    {
                        VModel.Synonyms.EditItem(item);
                        item.DomainState = lib.DomainObjectState.Deleted;
                        VModel.Synonyms.CommitEdit();
                    }
                }
            }
        }
        internal bool SynonymsDeleteCanExec(object parametr)
        { return !(VModel.Synonyms.IsAddingNew | VModel.Synonyms.IsEditingItem); }
        internal void GendersDeleteExec(object parametr)
        {
            if (parametr is System.Collections.IList)
            {
                List<MappingGenderVM> items = new List<MappingGenderVM>();
                foreach (MappingGenderVM item in parametr as System.Collections.IList)
                    items.Add(item);
                foreach (MappingGenderVM item in items)
                {
                    if (item.DomainState == lib.DomainObjectState.Added)
                    {
                        item.DomainState = lib.DomainObjectState.Destroyed;
                        if (VModel.Genders.IsAddingNew && item == VModel.Genders.CurrentAddItem)
                            VModel.Genders.CancelNew();
                        else
                            VModel.Genders.Remove(item);
                    }
                    else
                    {
                        VModel.Genders.EditItem(item);
                        item.DomainState = lib.DomainObjectState.Deleted;
                        VModel.Genders.CommitEdit();
                    }
                }
            }
        }
        internal bool GendersDeleteCanExec(object parametr)
        { return !(VModel.Genders.IsAddingNew | VModel.Genders.IsEditingItem); }

        protected override void AddData(object parametr) { }
        protected override bool CanAddData(object parametr)
        {
            return false;
        }
        protected override bool CanRefreshData()
        {
            return false;
        }
        protected override void RefreshData(object parametr)
        {
            throw new NotImplementedException();
        }
    }

    public class MappingViewCommand : lib.ViewModelViewCommand
    {
        internal MappingViewCommand()
        {
            mymdbm = new MappingDBM();
            mydbm = mymdbm;
            mymdbm.FillAsyncCompleted = () =>
            {
                mygoodsnamefiltercommand = new GoodsNameSinonimCheckListBoxVM();
                mygoodsnamefiltercommand.DeferredFill = true;
                mygoodsnamefiltercommand.ItemsSource = myview.OfType<MappingVM>();
                mygoodsnamefiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
                mygoodsnamefiltercommand.ExecCommand2 = () => { mygoodsnamefiltercommand.Clear(); };

                mytnvedfiltercommand = new MappingTNVEDGroupCheckListBoxVM();
                mytnvedfiltercommand.DeferredFill = true;
                mytnvedfiltercommand.ItemsSource = myview.OfType<MappingVM>();
                mytnvedfiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
                mytnvedfiltercommand.ExecCommand2 = () => { mytnvedfiltercommand.Clear(); };

                mymaterialfiltercommand = new MappingMaterialCheckListBoxVM();
                mymaterialfiltercommand.DeferredFill = true;
                mymaterialfiltercommand.ItemsSource = myview.OfType<MappingVM>();
                mymaterialfiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
                mymaterialfiltercommand.ExecCommand2 = () => { mymaterialfiltercommand.Clear(); };
            };
            mymdbm.Fill();
            mysync = new MappingSynchronizer();
            mysync.DomainCollection = mymdbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            mymaterials = new System.Windows.Data.ListCollectionView(References.Materials);
            mymaterials.Filter= delegate (object item) { Material mitem = item as Material; return Classes.Specification.MappingViewCommand.ViewFilterDefault(item) & (mitem.Id == 12 | mitem.Id == 13 | mitem.Upper?.Id == 15 | mitem.Upper?.Id == 16 | mitem.Upper?.Id == 22 | mitem.Upper?.Id == 23); };
            mymaterials.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            myexhandler = new DataModelClassLibrary.ExceptionHandler("Сохранение");
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            myexcelimport = new RelayCommand(ExcelImportExec, ExcelImportCanExec);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);

            mygenderfiltercommand = new libui.CheckListBoxVM();
            mygenderfiltercommand.DisplayPath = "Name";
            mygenderfiltercommand.GetDisplayPropertyValueFunc = (item) => { return ((Gender)item).Name; };
            mygenderfiltercommand.SearchPath = "Name";
            mygenderfiltercommand.Items = CustomBrokerWpf.References.Genders;
            mygenderfiltercommand.ItemsViewFilterDefault = lib.ViewModelViewCommand.ViewFilterDefault;
            mygenderfiltercommand.SelectedAll = false;
            mygenderfiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
            mygenderfiltercommand.ExecCommand2 = () => { mygenderfiltercommand.Clear(); };
            mygenderfiltercommand.AreaFilterIsVisible = false;

            mygoodsnamefiltercommand = new GoodsNameSinonimCheckListBoxVM();
            mygoodsnamefiltercommand.DeferredFill = true;
            mygoodsnamefiltercommand.ItemsSource = myview.OfType<MappingVM>();
            mygoodsnamefiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
            mygoodsnamefiltercommand.ExecCommand2 = () => { mygoodsnamefiltercommand.Clear(); };

            mytnvedfiltercommand = new MappingTNVEDGroupCheckListBoxVM();
            mytnvedfiltercommand.DeferredFill = true;
            mytnvedfiltercommand.ItemsSource = myview.OfType<MappingVM>();
            mytnvedfiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
            mytnvedfiltercommand.ExecCommand2 = () => { mytnvedfiltercommand.Clear(); };

            mymaterialfiltercommand = new MappingMaterialCheckListBoxVM();
            mymaterialfiltercommand.DeferredFill = true;
            mymaterialfiltercommand.ItemsSource = myview.OfType<MappingVM>();
            mymaterialfiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
            mymaterialfiltercommand.ExecCommand2 = () => { mymaterialfiltercommand.Clear(); };

        }

        private MappingDBM mymdbm;
        private MappingSynchronizer mysync;
        private lib.ExceptionHandler myexhandler;
        private System.ComponentModel.BackgroundWorker mybw;
        private ExcelImportWin myExcelImportWin;
        private System.Windows.Data.ListCollectionView mymaterials;
        public System.Windows.Data.ListCollectionView Materials { get {return mymaterials; } }

        private GoodsNameSinonimCheckListBoxVM mygoodsnamefiltercommand;
        public GoodsNameSinonimCheckListBoxVM GoodsNameFilterCommand
        { get { return mygoodsnamefiltercommand; } }
        private MappingTNVEDGroupCheckListBoxVM mytnvedfiltercommand;
        public MappingTNVEDGroupCheckListBoxVM TNVEDFilterCommand
        { get { return mytnvedfiltercommand; } }
        private MappingMaterialCheckListBoxVM mymaterialfiltercommand;
        public MappingMaterialCheckListBoxVM MaterialFilterCommand
        { get { return mymaterialfiltercommand; } }
        private libui.CheckListBoxVM mygenderfiltercommand;
        public libui.CheckListBoxVM GenderFilterCommand
        { get { return mygenderfiltercommand; } }

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            myview.Filter = OnFilter;
        }
        private bool FilterRunCanExec(object parametr)
        { return !(myview.IsAddingNew | myview.IsEditingItem); }
        private bool OnFilter(object item)
        {
            bool where = lib.ViewModelViewCommand.ViewFilterDefault(item);
            MappingVM mapp = item as MappingVM;
            if (mygoodsnamefiltercommand!=null && mygoodsnamefiltercommand.FilterOn)
            {
                where = false;
                string nameitem = mapp.Goods.ToLower();
                string synmitem = mapp.Goods.ToLower();
                foreach (string name in mygoodsnamefiltercommand.SelectedItems)
                {
                    string lname = name.ToLower();
                    if (string.Equals(lname, nameitem))
                    {
                        where = true;
                        break;
                    }
                    else
                    {
                        foreach (GoodsSynonym synm in mapp.DomainObject.Synonyms)
                        {
                            if (string.Equals(lname, synm.Name))
                            {
                                where = true;
                                break;
                            }
                        }
                        if (where) break;
                    }
                }
            }
            if (where && mytnvedfiltercommand != null && mytnvedfiltercommand.FilterOn)
            {
                where = false;
                foreach (string name in mytnvedfiltercommand.SelectedItems)
                {
                    if (string.Equals(mapp.TNVEDGroup, name))
                    {
                        where = true;
                        break;
                    }
                }
            }
            if (where && mymaterialfiltercommand != null && mymaterialfiltercommand.FilterOn)
            {
                where = false;
                foreach (string name in mymaterialfiltercommand.SelectedItems)
                {
                    if (string.Equals(mapp.Material?.Name, name))
                    {
                        where = true;
                        break;
                    }
                }
            }
            if (where & mygenderfiltercommand.FilterOn)
            {
                where = false;
                foreach (Gender sitem in mygenderfiltercommand.SelectedItems)
                {
                    foreach (MappingGenderVM gender in mapp.Genders)
                    {
                        if (object.Equals(sitem, gender.DomainObject.Gender))
                        {
                            where = true;
                            break;
                        }
                    }
                    if (where) break;
                }
            }
            return where;
        }

        private RelayCommand myfilterclear;
        public ICommand FilterClear
        {
            get { return myfilterclear; }
        }
        private void FilterClearExec(object parametr)
        {
            if (mygoodsnamefiltercommand != null)
            {
                mygoodsnamefiltercommand.Clear();
                mygoodsnamefiltercommand.IconVisibileChangedNotification();
            }
            if (mytnvedfiltercommand != null)
            {
                mytnvedfiltercommand.Clear();
                mytnvedfiltercommand.IconVisibileChangedNotification();
            }
            if (mymaterialfiltercommand != null)
            {
                mymaterialfiltercommand.Clear();
                mymaterialfiltercommand.IconVisibileChangedNotification();
            }
            mygenderfiltercommand.Clear();
            mygenderfiltercommand.IconVisibileChangedNotification();

            myview.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }

        private RelayCommand myexcelimport;
        public ICommand ExcelImport
        {
            get { return myexcelimport; }
        }
        private void ExcelImportExec(object parametr)
        {
            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
            fd.CheckPathExists = true;
            fd.CheckFileExists = true;
            fd.Multiselect = false;
            fd.Title = "Выбор файла с данными";
            fd.Filter = "Файл Excel |*.xls;*.xlsx";
            fd.ShowDialog();
            if (System.IO.File.Exists(fd.FileName))
            {
                if (mybw == null)
                {
                    mybw = new System.ComponentModel.BackgroundWorker();
                    mybw.DoWork += BackgroundWorker_DoWork;
                    mybw.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                    mybw.WorkerReportsProgress = true;
                    mybw.ProgressChanged += BackgroundWorker_ProgressChanged;
                }
                if (!mybw.IsBusy)
                {
                    if (myExcelImportWin != null && myExcelImportWin.IsVisible)
                    {
                        myExcelImportWin.MessageTextBlock.Text = string.Empty;
                        myExcelImportWin.ProgressBar1.Value = 0;
                    }
                    else
                    {
                        myExcelImportWin = new ExcelImportWin();
                        myExcelImportWin.Show();
                    }
                    string[] arg = { "true", fd.FileName, (System.Windows.MessageBox.Show("Пропускать уже имеющиеся позиции (по ИД)?\nИмеющиеся позиции не будут обновлены значениями из файла.", "Загрузка данных", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes).ToString() };
                    mybw.RunWorkerAsync(arg);
                }
                else
                {
                    System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
                }
            }
        }
        private bool ExcelImportCanExec(object parametr)
        { return !(myview.IsAddingNew | myview.IsEditingItem); }

        private RelayCommand myexcelexport;
        public ICommand ExcelExport
        {
            get { return myexcelexport; }
        }
        private void ExcelExportExec(object parametr)
        {
            this.myendedit();
            if (mybw == null)
            {
                mybw = new System.ComponentModel.BackgroundWorker();
                mybw.DoWork += BackgroundWorker_DoWork;
                mybw.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                mybw.WorkerReportsProgress = true;
                mybw.ProgressChanged += BackgroundWorker_ProgressChanged;
            }
            if (!mybw.IsBusy)
            {
                if (myExcelImportWin != null && myExcelImportWin.IsVisible)
                {
                    myExcelImportWin.MessageTextBlock.Text = string.Empty;
                    myExcelImportWin.ProgressBar1.Value = 0;
                }
                else
                {
                    myExcelImportWin = new ExcelImportWin();
                    myExcelImportWin.Show();
                }
                string[] arg = { "false" };
                mybw.RunWorkerAsync(arg);
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool ExcelExportCanExec(object parametr)
        { return !(myview.IsAddingNew | myview.IsEditingItem); }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            string[] args = e.Argument as string[];
            bool isclose = bool.Parse(args[0]);
            try
            {
                if (isclose)
                    e.Result = OnExcelImport(worker, exApp, args[1], bool.Parse(args[2]));
                else
                    e.Result = OnExcelExport(worker, exApp);
                worker.ReportProgress(100);

            }
            finally
            {
                if (exApp != null)
                {
                    if (isclose)
                    {
                        foreach (Excel.Workbook itemBook in exApp.Workbooks)
                        {
                            itemBook.Close(false);
                        }
                        exApp.DisplayAlerts = true;
                        exApp.ScreenUpdating = true;
                        exApp.Quit();
                    }
                    else
                    {
                        exApp.Visible = true;
                        exApp.DisplayAlerts = true;
                        exApp.ScreenUpdating = true;
                    }
                    exApp = null;
                }
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                myExcelImportWin.MessageTextBlock.Text = "Загрузка прервана из-за ошибки" + "\n" + e.Error.Message;
            }
            else
            {
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                myExcelImportWin.MessageTextBlock.Text = "Загрузка выполнена успешно." + "\n" + e.Result.ToString() + " строк обработано";
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            myExcelImportWin.ProgressBar1.Value = e.ProgressPercentage;
        }

        private int OnExcelImport(BackgroundWorker worker, Excel.Application exApp, string filepath, bool ismiss)
        {
            int maxr,n;
            string[] strs; 
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            MappingVM newitem;

            Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
            Excel.Worksheet exWh = null;
            foreach (Excel.Worksheet sheetWh in exWb.Sheets) if (sheetWh.Name == "СООТВЕТСТВИЕ") { exWh = sheetWh; break; }
            if (exWh == null)
            {
                throw new Exception("Вкладка СООТВЕТСТВИЕ не найдена!");
            }
            List<Gender> newgenders = new List<Gender>();
            maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            for (int r = 2; r <= maxr; r++)
            {
                if (string.IsNullOrEmpty((exWh.Cells[r, 1].Text as string).Trim())) continue;
                newitem = null;

                str.Clear();
                str.Append((exWh.Cells[r, 6].Text as string).Trim());
                if (str.Length>0 & int.TryParse(str.ToString(),out n))
                    foreach (MappingVM item in mysync.ViewModelCollection)
                        if (item.DomainObject.Id == n)
                        {
                            newitem = item;
                            break;
                        }
                if(newitem == null)
                    this.myview.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { newitem = new MappingVM(); int p = newitem.Synonyms.Count; p = newitem.Genders.Count; }));
                else if (ismiss) continue;

                str.Clear();
                str.Append((exWh.Cells[r, 1].Text as string).Trim());
                if (str.Length > 50)
                        throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 1].Address(false, false) + " содержит слишком длинный текст!");
                    else
                        newitem.Goods = str.ToString();

                bool contains=false;
                strs = (exWh.Cells[r, 2].Text as string).Trim().Replace((char)13, ',').Replace((char)10, ',').Replace("  ", " ").Replace(", ", ",").Replace(" ,", ",").Split(',');
                foreach (GoodsSynonym synitem in newitem.Synonyms)
                {
                    contains = false;
                    foreach (string stritem in strs)
                        contains |= synitem.Name.Equals(stritem.Trim());
                    if (!contains)
                        newitem.Synonyms.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Func<GoodsSynonym,bool>(newitem.DomainObject.Synonyms.Remove), synitem);
                }
                foreach (string stritem in strs)
                    if (stritem.Trim().Length > 50)
                        throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 2].Address(false, false) + " содержит слишком длинный текст для названия синонима!");
                    else if(stritem.Trim().Length > 0)
                    {
                        contains = false;
                        foreach (GoodsSynonymVM synitem in newitem.Synonyms)
                            contains |= synitem.Name.Equals(stritem.Trim());
                        if (!contains)
                            newitem.Synonyms.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<GoodsSynonym>(newitem.DomainObject.Synonyms.Add), new GoodsSynonym(lib.NewObjectId.NewId, newitem.DomainObject, stritem.Trim(), lib.DomainObjectState.Added));
                    }

                str.Clear();
                str.Append((exWh.Cells[r, 3].Text as string).Trim());
                if (str.Length != 4 | !int.TryParse(str.ToString(),out n))
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 3].Address(false, false) + " ГРУППА ТН ВЭД должно быть 4 цифры!");
                else
                    newitem.TNVEDGroup = str.ToString();

                str.Clear();
                str.Append((exWh.Cells[r, 4].Text as string).Trim());
                if (str.Length > 0)
                {
                    Material matr = References.Materials.FindFirstItem("Name", str.ToString());
                    if (matr == null)
                        throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 4].Address(false, false) + " такой материал не найден!");
                    else
                        newitem.Material = matr;
                }

                newgenders.Clear();
                strs = (exWh.Cells[r, 5].Text as string).Trim().Replace((char)13, ',').Replace((char)10, ',').Replace("  ", " ").Replace(", ", ",").Replace(" ,", ",").Split(',');
                foreach (string stritem in strs)
                {
                    Gender gn;
                    if (stritem.Trim().Length > 0)
                    {
                        gn = References.Genders.FindFirstItem("Name", stritem.Trim());
                        if (gn == null) gn = References.Genders.FindFirstItem("ShortName", stritem.Trim());
                        if (gn == null)
                            throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 5].Address(false, false) + " такой пол не найден!");
                        else if (!newgenders.Contains(gn))
                            newgenders.Add(gn);
                    }
                }
                foreach (Gender genderitem in newitem.Genders)
                {
                    if(!newgenders.Contains(genderitem))
                        newitem.Genders.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Func<MappingGender,bool>(newitem.DomainObject.Genders.Remove), genderitem);
                }
                foreach (Gender gn in newgenders)
                {
                       if (!newitem.Genders.Contains(gn))
                            newitem.Genders.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<MappingGender>(newitem.DomainObject.Genders.Add), new MappingGender(gn, lib.DomainObjectState.Added));
                }

                if (!mysync.ViewModelCollection.Contains(newitem)) this.myview.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<MappingVM>(mysync.ViewModelCollection.Add), newitem);
                worker.ReportProgress((int)(decimal.Divide(r, maxr) * 100));
            }
            exWb.Close();
            return maxr;
        }
        private int OnExcelExport(BackgroundWorker worker, Excel.Application exApp)
        {
            Excel.Workbook exWb;
            try
            {
                int row = 2;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                exWh.Name = "СООТВЕТСТВИЕ";
                Excel.Range r;

                exWh.Cells[1, 1] = "ТОВАР"; exWh.Cells[1, 2] = "СИНОНИМЫ"; exWh.Cells[1, 3] = "ГРУППА ТН ВЭД"; exWh.Cells[1, 4] = "МАТЕРИАЛ / ТКАНЬ"; exWh.Cells[1, 5] = "ПОЛ"; ; exWh.Cells[1, 6] = "ИД";

                exWh.Columns[1, Type.Missing].NumberFormat = "@";
                exWh.Columns[2, Type.Missing].NumberFormat = "@";
                exWh.Columns[3, Type.Missing].NumberFormat = "@";
                exWh.Columns[4, Type.Missing].NumberFormat = "@";
                exWh.Columns[5, Type.Missing].NumberFormat = "@";
                exWh.Columns[6, Type.Missing].NumberFormat = "@";
                foreach (object itemobj in myview)
                {
                    if (!(itemobj is MappingVM)) continue;

                    MappingVM item = itemobj as MappingVM;
                    exWh.Cells[row, 1] = item.Goods;
                    exWh.Cells[row, 2] = item.SynonymsStr;
                    exWh.Cells[row, 3] = item.TNVEDGroup;
                    if(item.Material!=null) exWh.Cells[row, 4] = item.Material.Name;
                    exWh.Cells[row, 5] = item.GendersStr;
                    exWh.Cells[row, 6] = item.DomainObject.Id;

                    row++;
                }

                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, 5]];
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                r.VerticalAlignment = Excel.Constants.xlTop;
                r.WrapText = true;
                //r = exWh.Range[exWh.Columns[1, Type.Missing], exWh.Columns[17, Type.Missing]]; r.Columns.AutoFit();
                exApp.Visible = true;
                exWh = null;
                return row - 2;
            }
            catch (Exception ex)
            {
                if (exApp != null)
                {
                    foreach (Excel.Workbook itemBook in exApp.Workbooks)
                    {
                        itemBook.Close(false);
                    }
                    exApp.Quit();
                    exApp = null;
                }
                throw new ApplicationException(ex.Message);
            }
        }

        public override bool SaveDataChanges()
        {
            bool isSuccess = !(myview.CurrentItem is MappingVM) || (myview.CurrentItem as MappingVM).Validate(true);
            if (mymdbm == null)
            {
                mymdbm = new MappingDBM();
                mymdbm.Collection = mysync.DomainCollection;
            }
            else
                mymdbm.Errors.Clear();
            isSuccess &= mymdbm.SaveCollectionChanches();
            if (!isSuccess)
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                if (myview.CurrentItem is MappingVM) err.Append((myview.CurrentItem as MappingVM).Errors);
                err.AppendLine(mymdbm.ErrorMessage);
                myexhandler.Handle(new Exception(err.ToString()));
                myexhandler.ShowMessage();
            }
            return isSuccess;
        }
        protected override void AddData(object parametr)
        {
            throw new NotImplementedException();
        }
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
        protected override void RefreshData(object parametr)
        {
            References.Materials.Refresh();
            References.Genders.Refresh();
            mymdbm.Collection.Clear();
            mymdbm.Fill();
        }
        protected override bool CanRejectChanges()
        {
            return true;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void OtherViewRefresh() { }
        protected override void RejectChanges(object parametr)
        {
            List<MappingVM> destroied = new List<MappingVM>();
            foreach (MappingVM item in mysync.ViewModelCollection)
            {
                if (item.DomainState == lib.DomainObjectState.Added)
                    destroied.Add(item);
                else if (item.DomainState != lib.DomainObjectState.Unchanged)
                {
                    base.myview.EditItem(item);
                    item.RejectChanges();
                    base.myview.CommitEdit();
                }
            }
            foreach (MappingVM item in destroied) mysync.ViewModelCollection.Remove(item);
        }
        protected override void SettingView()
        {
           myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Goods",System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class GoodsNameSinonimCheckListBoxVM : libui.CheckListBoxVMFill<MappingVM, string>
    {
        protected override void AddItem(MappingVM item)
        {
            if(!Items.Contains(item.Goods)) Items.Add(item.Goods);
            foreach (GoodsSynonym synm in item.DomainObject.Synonyms)
            {
                if (!Items.Contains(synm.Name)) Items.Add(synm.Name);
            }
    }
    }
    public class MappingTNVEDGroupCheckListBoxVM : libui.CheckListBoxVMFill<MappingVM, string>
    {
        protected override void AddItem(MappingVM item)
        {
            if (!Items.Contains(item.TNVEDGroup)) Items.Add(item.TNVEDGroup);
        }
    }
    public class MappingMaterialCheckListBoxVM : libui.CheckListBoxVMFill<MappingVM, string>
    {
        protected override void AddItem(MappingVM item)
        {
            if (!Items.Contains(item.Material.Name)) Items.Add(item.Material.Name);
        }
    }

}
