using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using Excel = Microsoft.Office.Interop.Excel;


namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class Material : lib.DomainBaseNotifyChanged
    {
        private Material(int id, string name, string shortname, string goodsname, lib.DomainObjectState state) : base(id, state)
        {
            myname = name;
            myshortname = shortname;
            mygoodsname = goodsname;
        }
        public Material(int id, Material upper, Material substitution, string name, string shortname, string goodsname, lib.DomainObjectState state) : this(id, name, shortname, goodsname, state)
        {
            myupper = upper;
            mysubstitution = substitution;
        }
        public Material(int id, int? upperid, int? substitutionid, string name, string shortname, string goodsname, lib.DomainObjectState state) : this(id, name, shortname, goodsname, state)
        {
            myupperid = upperid;
            mysubstitutionid = substitutionid;
        }
        public Material(int id, int? upperid, int? substitutionid, string name, string shortname, string goodsname, string tnvedgroup, lib.DomainObjectState state) : this(id, upperid, substitutionid, name, shortname, goodsname, state)
        {
            mytnvedgroup = tnvedgroup;
        }
        public Material() : this(lib.NewObjectId.NewId, (int?)null, (int?)null, string.Empty, string.Empty, string.Empty, lib.DomainObjectState.Added) { }

        private int? myupperid;
        private Material myupper;
        public Material Upper
        {
            set
            {
                if (!object.Equals(value, myupper))
                {
                    string name = "Upper";
                    myupper = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get
            {
                if (myupper == null & myupperid.HasValue)
                    myupper = CustomBrokerWpf.References.Materials.FindFirstItem("Id", myupperid.Value);
                return myupper;
            }
        }
        private int? mysubstitutionid;
        private Material mysubstitution;
        public Material Substitution
        {
            set
            {
                if (!object.Equals(value, mysubstitution))
                {
                    string name = "Substitution";
                    mysubstitution = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get
            {
                if (mysubstitution == null & mysubstitutionid.HasValue)
                    mysubstitution = CustomBrokerWpf.References.Materials.FindFirstItem("Id", mysubstitutionid.Value);
                return mysubstitution;
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
        private string mygoodsname;
        public string GoodsName
        {
            set
            {
                if (!string.Equals(mygoodsname, value))
                {
                    string name = "GoodsName";
                    mygoodsname = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mygoodsname; }
        }
        private string mytnvedgroup;
        public string TNVEDGroup
        { set { SetProperty<string>(ref mytnvedgroup, value); } get { return mytnvedgroup; } }
    }

    public class MaterialDBM : lib.DBManager<Material>
    {
        public MaterialDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = false;
            SelectCommandText = "SELECT id,upper,name,shortname,goodsname,substitution,tnvedgroup FROM [spec].[Material_tb] ORDER BY id";

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);

            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@upper", System.Data.SqlDbType.Int) };
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@shortname", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@goodsname", System.Data.SqlDbType.NVarChar,10),
                new SqlParameter("@substitution", System.Data.SqlDbType.Int),
                new SqlParameter("@tnvedgroup", System.Data.SqlDbType.NVarChar,10)
            };
            mydeleteparams = new SqlParameter[] { parid };

            InsertProcedure = false;
            myinsertcommandtext = "INSERT INTO [spec].[Material_tb] (upper,name,shortname,goodsname,substitution,tnvedgroup) VALUES(@upper,@name,@shortname,@goodsname,@substitution,@tnvedgroup); SET @id=SCOPE_IDENTITY();";
            UpdateProcedure = false;
            myupdatecommandtext = "UPDATE [spec].[Material_tb] SET name=@name,shortname=@shortname,goodsname=@goodsname,substitution=@substitution,tnvedgroup=@tnvedgroup WHERE id=@id";
            DeleteProcedure = false;
            mydeletecommandtext = "DELETE FROM [spec].[Material_tb] WHERE id=@id";
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override Material CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new Material(reader.GetInt32(0), reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1), reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5), reader.GetString(2), reader.GetString(3), reader.IsDBNull(4) ? null : reader.GetString(4), reader.IsDBNull(6) ? null : reader.GetString(6), lib.DomainObjectState.Unchanged);
        }
        protected override void GetOutputParametersValue(Material item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
            }
        }
        protected override void ItemAcceptChanches(Material item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(Material item)
        {
            bool issuccess = true;
            System.Windows.Data.ListCollectionView children = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Materials);
            children.Filter = (object child) => { return object.Equals((child as Material).Upper, item) & (child as Material).DomainState != lib.DomainObjectState.Unchanged; };
            foreach (Material child in children)
                issuccess &= this.SaveItemChanches(child);
            return issuccess;
        }
        protected override bool SaveIncludedObject(Material item)
        {
            bool issacses = true;
            if (item.Upper?.DomainState == lib.DomainObjectState.Added)
            {
                issacses = this.SaveItemChanches(item.Upper);
            }
            if (item.Substitution?.DomainState == lib.DomainObjectState.Added)
            {
                issacses &= this.SaveItemChanches(item.Substitution);
            }
            return issacses;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(Material item)
        {
            myupdateparams[0].Value = item.Id;
            myinsertparams[1].Value = item.Upper!=null? item.Upper.Id: (object)DBNull.Value;
            myinsertupdateparams[0].Value = item.Name;
            myinsertupdateparams[1].Value = item.ShortName;
            myinsertupdateparams[2].Value = string.IsNullOrEmpty(item.GoodsName) ? DBNull.Value : (object)item.GoodsName;
            myinsertupdateparams[3].Value = item.Substitution != null ? item.Substitution.Id : (object)DBNull.Value;
            myinsertupdateparams[4].Value = string.IsNullOrEmpty(item.TNVEDGroup) ? DBNull.Value : (object)item.TNVEDGroup;
            return item.Upper?.DomainState != lib.DomainObjectState.Added;
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class MaterialCollection : lib.ReferenceCollectionDomainBase<Material>
    {
        public MaterialCollection() : this(new MaterialDBM()) { }
        public MaterialCollection(lib.DBManager<Material> dbm) : base(dbm) { }

        public override Material FindFirstItem(string propertyName, object value)
        {
            Material first = null;
            foreach (Material item in this)
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
                    case "GoodsName":
                        if (item.ShortName.ToUpper().Equals(((string)value).ToUpper()))
                            first = item;
                        break;
                    case "TNVEDGroup":
                        if (item.TNVEDGroup.Equals(((string)value)))
                            first = item;
                        break;
                    default:
                        throw new NotImplementedException("Свойство " + propertyName + " не реализовано");
                }
            }
            return first;
        }
        protected override int CompareReferences(Material item1, Material item2)
        {
            return item1.Id.CompareTo(item2.Id);
        }
        protected override void UpdateItem(Material olditem, Material newitem)
        { olditem.Name = newitem.Name; olditem.ShortName = newitem.ShortName; olditem.GoodsName = newitem.GoodsName; olditem.TNVEDGroup = newitem.TNVEDGroup; }

        internal void DataLoad()
        { try { base.Fill(); } catch (Exception ex) { lib.ExceptionHandler myexhandler = new DataModelClassLibrary.ExceptionHandler("Загрузка материалов"); myexhandler.Handle(ex); myexhandler.ShowMessage(); } }

        private MaterialSynchronizer mysinc;
        internal System.Collections.ObjectModel.ObservableCollection<MaterialVM> MaterialVMCollection
        {
            get
            {
                if (mysinc == null)
                {
                    mysinc = new MaterialSynchronizer();
                    mysinc.DomainCollection = this;
                }
                return mysinc.ViewModelCollection;
            }
        }
    }

    public class MaterialVM : lib.ViewModelErrorNotifyItem<Material>
    {
        public MaterialVM(Material domain) : base(domain)
        {
            ValidetingProperties.AddRange(new string[] { "Name", "ShortName" });
            InitProperties();
        }
        public MaterialVM() : this(new Material()) { }

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
                        if (string.IsNullOrEmpty(myshortname)) this.ShortName = value;
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
        public string GoodsName
        {
            set
            {
                if (!string.Equals(this.DomainObject.GoodsName, value) & base.IsEnabled)
                {
                    string name = "GoodsName";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.GoodsName);
                    ChangingDomainProperty = name; base.DomainObject.GoodsName = value;
                }
            }
            get { return base.IsEnabled ? this.DomainObject.GoodsName : null; }
        }
        public Material Substitution
        {
            set
            {
                if (!object.Equals(this.DomainObject.Substitution, value) & base.IsEnabled)
                {
                    string name = "Substitution";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Substitution);
                    ChangingDomainProperty = name; base.DomainObject.Substitution = value;
                }
            }
            get { return base.IsEnabled ? this.DomainObject.Substitution : null; }
        }
        public string TNVEDGroup
        {
            set
            {
                if (!string.Equals(this.DomainObject.TNVEDGroup, value) & base.IsEnabled)
                {
                    string name = "TNVEDGroup";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.TNVEDGroup);
                    ChangingDomainProperty = name; base.DomainObject.TNVEDGroup = value;
                }
            }
            get { return base.IsEnabled ? this.DomainObject.TNVEDGroup : null; }
        }

        private System.Windows.Data.ListCollectionView mysubproducts;
        public System.Windows.Data.ListCollectionView SubProducts
        {
            get
            {
                if (mysubproducts == null)
                {
                    mysubproducts = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Materials.MaterialVMCollection);
                    mysubproducts.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    mysubproducts.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as MaterialVM).DomainObject.Upper == this.DomainObject; };
                }
                return mysubproducts;
            }
        }
        private string mysubproductsstr;
        public string SubProductsStr
        {
            private set { mysubproductsstr = value; PropertyChangedNotification("SubProductsStr"); }
            get
            {
                if (mysubproductsstr == null)
                    SubProductsChanged();
                return mysubproductsstr;
            }
        }

        public void SubProductsChanged()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (object item in this.SubProducts)
                if(item is MaterialVM) str.Append(", " + (item as MaterialVM).Name);
            if (str.Length > 0) str.Remove(0, 2);
            this.SubProductsStr = str.ToString();
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
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myname!= base.DomainObject.Name || myshortname!= base.DomainObject.ShortName;
        }
    }

    internal class MaterialSynchronizer : lib.ModelViewCollectionsSynchronizer<Material, MaterialVM>
    {
        protected override Material UnWrap(MaterialVM wrap)
        {
            return wrap.DomainObject as Material;
        }

        protected override MaterialVM Wrap(Material fill)
        {
            return new MaterialVM(fill);
        }
    }

    public class MaterialCommand : lib.ViewModelCommand<Material, MaterialVM, MaterialDBM>
    {
        internal MaterialCommand(MaterialVM vm, System.Windows.Data.ListCollectionView view) : base(vm, view)
        {
            mymaterials = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Materials.MaterialVMCollection);
            mymaterials.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
        internal MaterialCommand(System.Windows.Data.ListCollectionView view) : this(new MaterialVM(), view) { }

        private System.Windows.Data.ListCollectionView mymaterials;
        public System.Windows.Data.ListCollectionView Materials
        {
            get { return mymaterials; }
        }

        internal void DeleteSubMaterialExec(object parametr)
        {
            if (parametr is System.Collections.IList)
            {
                System.Collections.Generic.List<MaterialVM> items = new System.Collections.Generic.List<MaterialVM>();
                foreach (MaterialVM item in parametr as System.Collections.IList)
                    items.Add(item);
                foreach (MaterialVM item in items)
                {
                    if (item.DomainState == lib.DomainObjectState.Added)
                    {
                        item.DomainState = lib.DomainObjectState.Destroyed;
                        if (VModel.SubProducts.IsAddingNew && item == VModel.SubProducts.CurrentAddItem)
                            VModel.SubProducts.CancelNew();
                        else
                            VModel.SubProducts.Remove(item);
                    }
                    else
                    {
                        VModel.SubProducts.EditItem(item);
                        item.DomainState = lib.DomainObjectState.Deleted;
                        VModel.SubProducts.CommitEdit();
                    }
                }
            }
        }
        internal bool DeleteSubMaterialCanExec(object parametr)
        { return !(VModel.SubProducts.IsAddingNew | VModel.SubProducts.IsEditingItem); }

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

    public class MaterialViewCommand : lib.ViewModelViewCommand
    {
        internal MaterialViewCommand()
        {
            base.Collection = CustomBrokerWpf.References.Materials.MaterialVMCollection;
            mynatureal = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Materials.MaterialVMCollection);
            mynatureal.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as MaterialVM).DomainObject.Upper?.Id == 15; };
            mynatureal.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            mychemical = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Materials.MaterialVMCollection);
            mychemical.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as MaterialVM).DomainObject.Upper?.Id == 16; };
            mychemical.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            myothers = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Materials.MaterialVMCollection);
            myothers.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as MaterialVM).DomainObject.Upper?.Id == 23; };
            myothers.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            mynowoven = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Materials.MaterialVMCollection);
            mynowoven.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as MaterialVM).DomainObject.Upper?.Id == 22; };
            mynowoven.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            myhalf = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Materials.MaterialVMCollection);
            myhalf.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as MaterialVM).DomainObject.Upper?.Id == 17; };
            myhalf.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            myexhandler = new DataModelClassLibrary.ExceptionHandler("Сохранение");
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            myexcelimport = new RelayCommand(ExcelImportExec, ExcelImportCanExec);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
        }

        private new MaterialDBM mydbm;
        private lib.ExceptionHandler myexhandler;
        private System.ComponentModel.BackgroundWorker mybw;
        private ExcelImportWin myExcelImportWin;

        private System.Windows.Data.ListCollectionView mynatureal;
        public System.Windows.Data.ListCollectionView Natureal
        {
            get { return mynatureal; }
        }
        private System.Windows.Data.ListCollectionView mychemical;
        public System.Windows.Data.ListCollectionView Chemical
        {
            get { return mychemical; }
        }
        private System.Windows.Data.ListCollectionView myothers;
        public System.Windows.Data.ListCollectionView Others
        {
            get { return myothers; }
        }
        private System.Windows.Data.ListCollectionView mynowoven;
        public System.Windows.Data.ListCollectionView NoWoven
        {
            get { return mynowoven; }
        }
        private System.Windows.Data.ListCollectionView myhalf;
        public System.Windows.Data.ListCollectionView Half
        {
            get { return myhalf; }
        }

        private string myfiltername;
        public string FilterName
        {
            set
            {
                myfiltername = value;
                PropertyChangedNotification("FilterName");
            }
            get { return myfiltername; }
        }

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
            if (where & !string.IsNullOrEmpty(myfiltername))
            {
                string[] str;
                MaterialVM mapp = item as MaterialVM;
                str = myfiltername.Trim().ToLower().Split(' ');
                foreach (string stritem in str)
                {
                    where &= mapp.Name.ToLower().IndexOf(stritem) > -1;
                    if (!where) break;
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
            FilterName = string.Empty;
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
                    string[] arg = { "true", fd.FileName, (System.Windows.MessageBox.Show("Пропускать уже имеющиеся позиции (по наименованию)?\nИмеющиеся позиции не будут обновлены значениями из файла.", "Загрузка данных", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes).ToString() };
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
                myExcelImportWin.MessageTextBlock.Text = "Загрузка прервана из-за ошибки" + "\n" + e.Error.Message;
            }
            else
            {
                myExcelImportWin.MessageTextBlock.Text = "Загрузка выполнена успешно." + "\n" + e.Result.ToString() + " строк обработано";
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            myExcelImportWin.ProgressBar1.Value = e.ProgressPercentage;
        }

        private int OnExcelImport(BackgroundWorker worker, Excel.Application exApp, string filepath, bool ismiss)
        {
            int maxr;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            MaterialVM newitem;

            Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
            Excel.Worksheet exWh = null;
            foreach (Excel.Worksheet sheetWh in exWb.Sheets) if (sheetWh.Name == "МАТЕРИАЛ") { exWh = sheetWh; break; }
            if (exWh == null)
            {
                throw new Exception("Вкладка МАТЕРИАЛ не найдена!");
            }
            // Задать форматы столбцов
            maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            for (int r = 2; r <= maxr; r++)
            {
                if (string.IsNullOrEmpty((exWh.Cells[r, 1].Text as string).Trim())) continue;
                newitem = null;

                str.Clear();
                str.Append((exWh.Cells[r, 1].Text as string).Trim());
                foreach (MaterialVM item in CustomBrokerWpf.References.Materials.MaterialVMCollection)
                    if (item.Name.ToLower() == str.ToString().ToLower())
                    {
                        newitem = item;
                        break;
                    }
                if (newitem == null)
                {
                    newitem = new MaterialVM();
                    if (str.Length > 50)
                        throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 1].Address(false, false) + " содержит слишком длинный текст!");
                    else
                        newitem.Name = str.ToString();
                }
                else if (ismiss) continue;

                str.Clear();
                str.Append((exWh.Cells[r, 2].Text as string).Trim());
                if (str.Length > 50)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 3].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.ShortName = str.ToString();

                if (!CustomBrokerWpf.References.Materials.MaterialVMCollection.Contains(newitem)) this.myview.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<MaterialVM>(CustomBrokerWpf.References.Materials.MaterialVMCollection.Add), newitem);
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
                exWh.Name = "МАТЕРИАЛ";
                Excel.Range r;

                exWh.Cells[1, 1] = "Материал/Ткань"; exWh.Cells[1, 2] = "Поиск";

                exWh.Columns[1, Type.Missing].NumberFormat = "@";
                exWh.Columns[2, Type.Missing].NumberFormat = "@";
                foreach (object itemobj in myview)
                {
                    if (!(itemobj is MaterialVM)) continue;

                    MaterialVM item = itemobj as MaterialVM;
                    exWh.Cells[row, 1] = item.Name;
                    exWh.Cells[row, 2] = item.ShortName;

                    row++;
                }

                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, 2]];
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                r.VerticalAlignment = Excel.Constants.xlTop;
                r.WrapText = true;
                r = exWh.Range[exWh.Columns[1, Type.Missing], exWh.Columns[17, Type.Missing]]; r.Columns.AutoFit();
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

        internal void IngredientDelete(object parametr)
        {
            System.Windows.Controls.DataGrid dg = parametr as System.Windows.Controls.DataGrid;
            if (dg != null)
            {
                System.Windows.Data.ListCollectionView view = dg.ItemsSource as System.Windows.Data.ListCollectionView;
                System.Collections.Generic.List<MaterialVM> items = new System.Collections.Generic.List<MaterialVM>();
                foreach (MaterialVM item in dg.SelectedItems)
                    items.Add(item);
                foreach (MaterialVM item in items)
                {
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
        }
        internal bool IngredientCanDelete(object parametr)
        {
            System.Windows.Controls.DataGrid dg = parametr as System.Windows.Controls.DataGrid;
            if (dg != null)
            {
                System.Windows.Data.ListCollectionView view = dg.ItemsSource as System.Windows.Data.ListCollectionView;
                return !(view.IsAddingNew | view.IsEditingItem);
            }
            else
                return false;
        }

        public override bool SaveDataChanges()
        {
            bool isSuccess = !(myview.CurrentItem is MaterialVM) || (myview.CurrentItem as MaterialVM).Validate(true);
            isSuccess &= !(mynatureal.CurrentItem is MaterialVM) || (mynatureal.CurrentItem as MaterialVM).Validate(true);
            isSuccess &= !(mychemical.CurrentItem is MaterialVM) || (mychemical.CurrentItem as MaterialVM).Validate(true);
            isSuccess &= !(myothers.CurrentItem is MaterialVM) || (myothers.CurrentItem as MaterialVM).Validate(true);
            isSuccess &= !(mynowoven.CurrentItem is MaterialVM) || (mynowoven.CurrentItem as MaterialVM).Validate(true);
            isSuccess &= !(myhalf.CurrentItem is MaterialVM) || (myhalf.CurrentItem as MaterialVM).Validate(true);
            if (mydbm == null)
            {
                mydbm = new MaterialDBM();
                mydbm.Collection = CustomBrokerWpf.References.Materials;
            }
            else
                mydbm.Errors.Clear();
            isSuccess &= mydbm.SaveCollectionChanches();
            if (!isSuccess)
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                if (myview.CurrentItem is MaterialVM) err.Append((myview.CurrentItem as MaterialVM).Errors);
                if (mynatureal.CurrentItem is MaterialVM) err.Append((mynatureal.CurrentItem as MaterialVM).Errors);
                if (mychemical.CurrentItem is MaterialVM) err.Append((mychemical.CurrentItem as MaterialVM).Errors);
                if (myothers.CurrentItem is MaterialVM) err.Append((myothers.CurrentItem as MaterialVM).Errors);
                if (mynowoven.CurrentItem is MaterialVM) err.Append((mynowoven.CurrentItem as MaterialVM).Errors);
                if (myhalf.CurrentItem is MaterialVM) err.Append((myhalf.CurrentItem as MaterialVM).Errors);
                err.AppendLine(mydbm.ErrorMessage);
                myexhandler.Handle(new Exception(err.ToString()));
                myexhandler.ShowMessage();
            }
            return isSuccess;
        }
        protected override void AddData(object parametr)
        {
            System.Windows.Data.ListCollectionView view = parametr as System.Windows.Data.ListCollectionView;
            if (parametr != null)
            {
                ;
            }
        }
        protected override bool CanAddData(object parametr)
        {
            return false;
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
            return false;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void OtherViewRefresh()
        {
            CustomBrokerWpf.References.Materials.RefreshViews();
        }
        protected override void RefreshData(object parametr)
        {
            CustomBrokerWpf.References.Materials.Refresh();
        }
        protected override void RejectChanges(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }
}
