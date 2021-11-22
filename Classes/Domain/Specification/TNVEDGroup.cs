using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using libui = KirillPolyanskiy.WpfControlLibrary;


namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class TNVEDGroup:lib.DomainBaseNotifyChanged
    {
        public TNVEDGroup(int id,lib.DomainObjectState modelstate
            ,string tnvedgroup, Material material
            ) :base(id,modelstate)
        {
            mytnvedgroup = tnvedgroup;
            mymaterial = material;
            gsdbm = new TNVEDGoodsDBM();
            gsdbm.TNVEDGroup = this;
            gsdbm.Fill();
            mygoods = gsdbm.Collection;
            GoodsChanged();
            mygoods.CollectionChanged += Goods_CollectionChanged;
        }
        public TNVEDGroup():this(lib.NewObjectId.NewId, lib.DomainObjectState.Added,null,null) { }

        private Material mymaterial;
        public Material Material
        {
            set { SetProperty<Material>(ref mymaterial, value); }
            get { return mymaterial; }
        }
        private string mytnvedgroup;
        public string Group
        {
            set { SetProperty<string>(ref mytnvedgroup, value); }
            get { return mytnvedgroup; }
        }
        private string mygoodsstr;
        public string GoodsStr
        {
            private set { mygoodsstr = value; PropertyChangedNotification("GoodsStr"); }
            get { return mygoodsstr; }
        }
        private TNVEDGoodsDBM gsdbm;
        private ObservableCollection<TNVEDGoods> mygoods;
        public ObservableCollection<TNVEDGoods> Goods
        {
            get
            {
                if (mygoods == null)
                {
                    gsdbm = new TNVEDGoodsDBM();
                    gsdbm.TNVEDGroup = this;
                    gsdbm.Fill();
                    mygoods = gsdbm.Collection;
                    GoodsChanged();
                    mygoods.CollectionChanged += Goods_CollectionChanged;
                }
                return mygoods;
            }
        }

        public void GoodsChanged()
        {
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            foreach (TNVEDGoods item in this.Goods)
                str.Append(", " + item.Name);
            if (str.Length > 0) str.Remove(0, 2);
            this.GoodsStr = str.ToString();
        }
        private void Goods_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GoodsChanged();
        }
    }

    public class TNVEDGroupDBM : lib.DBManager<TNVEDGroup>
    {
        public TNVEDGroupDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);

            myinsertparams = new SqlParameter[] { paridout };
            myupdateparams = new SqlParameter[]
            {
                parid,
                new SqlParameter("@tnvedgrouptrue", System.Data.SqlDbType.Bit),
                new SqlParameter("@materialidtrue", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@tnvedgroup", System.Data.SqlDbType.NVarChar, 10),
                new SqlParameter("@materialid", System.Data.SqlDbType.Int),
            };
            mydeleteparams = new SqlParameter[] { parid };

            SelectProcedure = true;
            SelectCommandText = "spec.TNVEDGroup_sp";
            InsertProcedure = true;
            myinsertcommandtext = "spec.TNVEDGroupAdd_sp";
            UpdateProcedure = true;
            myupdatecommandtext = "spec.TNVEDGroupUpd_sp";
            DeleteProcedure = true;
            mydeletecommandtext = "spec.TNVEDGroupDel_sp";
            mygdbm = new TNVEDGoodsDBM(); mygdbm.Command = new SqlCommand();
        }

        private TNVEDGoodsDBM mygdbm;

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override TNVEDGroup CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new TNVEDGroup(reader.GetInt32(0), lib.DomainObjectState.Unchanged, reader.GetString(1), reader.IsDBNull(2) ? null : References.Materials.FindFirstItem("Id", reader.GetInt32(2)));
        }
        protected override bool SaveChildObjects(TNVEDGroup item)
        {
            bool isSuccess = true;
            mygdbm.Errors.Clear();
            mygdbm.TNVEDGroup = item;
            mygdbm.Collection = item.Goods;
            if (!mygdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in mygdbm.Errors) this.Errors.Add(err);
            }

            return isSuccess;
        }
        protected override bool SaveIncludedObject(TNVEDGroup item)
        {
            bool isSuccess = true;
            if (item.Material != null && item.Material.DomainState == lib.DomainObjectState.Added)
            {
                MaterialDBM mdbm = new MaterialDBM();
                mdbm.Command = new SqlCommand() { Connection = this.Command.Connection };
                if (!mdbm.SaveItemChanches(item.Material))
                {
                    isSuccess = false;
                    foreach (lib.DBMError err in mdbm.Errors) this.Errors.Add(err);
                }
            }
            return isSuccess;
        }
        protected override bool SaveReferenceObjects()
        {
            mygdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetParametersValue(TNVEDGroup item)
        {
            myupdateparams[0].Value = item.Id;
            myupdateparams[1].Value = true;
            myupdateparams[2].Value = true;
            myinsertupdateparams[0].Value = item.Group;
            myinsertupdateparams[1].Value = item.Material != null ? (object)item.Material.Id : DBNull.Value;
            return true;
        }
        protected override void GetOutputParametersValue(TNVEDGroup item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
            }
        }
        protected override void ItemAcceptChanches(TNVEDGroup item)
        {
            item.AcceptChanches();
        }
        protected override void CancelLoad()
        { }
    }

    public class TNVEDGroupVM : lib.ViewModelErrorNotifyItem<TNVEDGroup>
    {
        public TNVEDGroupVM(TNVEDGroup domain) : base(domain)
        {
            ValidetingProperties.AddRange(new string[] { "Group", "Material" });
            DeleteRefreshProperties.AddRange(new string[] {"Group", "Material", "Goods", "GoodsStr" });
            InitProperties();
        }
        public TNVEDGroupVM() : this(new TNVEDGroup()) { }

        private TNVEDGoodsSynchronizer gssync;

        private string mytnvedgroup;
        public string Group
        {
            set
            {
                if (!string.Equals(mytnvedgroup, value) & base.IsEnabled)
                {
                    string name = "Group";
                    mytnvedgroup = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Group);
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name;
                        base.DomainObject.Group = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return base.IsEnabled ? mytnvedgroup : null; }
        }
        private Material mymaterial;
        public Material Material
        {
            set
            {
                if (!object.Equals(mymaterial, value) & base.IsEnabled)
                {
                    string name = "Material";
                    mymaterial = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Material);
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name;
                        base.DomainObject.Material = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return base.IsEnabled ? mymaterial : null; }
        }
        public string GoodsStr
        { get { return this.DomainObject.GoodsStr; } }
        private System.Windows.Data.ListCollectionView mygoods;
        public System.Windows.Data.ListCollectionView Goods
        {
            get
            {
                if (mygoods == null)
                {
                    gssync = new TNVEDGoodsSynchronizer();
                    gssync.DomainCollection = this.DomainObject.Goods;
                    mygoods = new System.Windows.Data.ListCollectionView(gssync.ViewModelCollection);
                    mygoods.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    mygoods.CurrentChanged += Goods_CurrentChanged;
                }
                return base.IsEnabled ? mygoods : null;
            }
        }
        public void DeleteGoods(object parametr)
        {
            if (parametr is TNVEDGoods)
            {
                mygoods.EditItem(parametr);
                (parametr as TNVEDGoods).DomainState = (parametr as TNVEDGoods).DomainState == lib.DomainObjectState.Added ? lib.DomainObjectState.Destroyed : lib.DomainObjectState.Deleted;
                mygoods.CommitEdit();
            }
        }

        private void Goods_CurrentChanged(object sender, EventArgs e)
        {
            this.DomainObject.GoodsChanged();
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Material":
                    mymaterial = this.DomainObject.Material;
                    break;
                case "Group":
                    mytnvedgroup = this.DomainObject.Group;
                    break;
            }
        }
        protected override void InitProperties()
        {
            mymaterial = this.DomainObject.Material;
            mytnvedgroup = this.DomainObject.Group;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Material":
                    if (mymaterial != this.DomainObject.Material)
                        mymaterial = this.DomainObject.Material;
                    else
                        this.Material = (Material)value;
                    break;
                case "Group":
                    if (mytnvedgroup != this.DomainObject.Group)
                        mytnvedgroup = this.DomainObject.Group;
                    else
                        this.Group = (string)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    if (mygoods != null)
                    {
                        TNVEDGoodsVM[] additem = new TNVEDGoodsVM[mygoods.Count];
                        foreach (TNVEDGoodsVM item in mygoods.SourceCollection)
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                additem[i] = item; i++;
                            }
                            else if (item.DomainState == lib.DomainObjectState.Deleted)
                            {
                                mygoods.EditItem(item);
                                item.DomainState = lib.DomainObjectState.Modified;
                                mygoods.CommitEdit();
                            }
                            else
                                item.RejectChanges();
                        for (int ii = 0; ii < i; ii++) mygoods.Remove(additem[ii]);
                    }
                    this.DomainObject.GoodsChanged();
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Material":
                    if (mymaterial==null)
                    {
                        errmsg = "Отсутствует материал";
                        isvalid = false;
                    }
                    break;
                case "Group":
                    if (string.IsNullOrEmpty(mytnvedgroup))
                    {
                        errmsg = "Отсутствует группа ТН ВЭД";
                        isvalid = false;
                    }
                    break;
                case "Dependent":
                    foreach (TNVEDGoodsVM item in this.Goods)
                        if (item != null) isvalid &= item.Validate(inform);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return mytnvedgroup!= base.DomainObject.Group || mymaterial!= base.DomainObject.Material;
        }
    }

    internal class TNVEDGroupSynchronizer : lib.ModelViewCollectionsSynchronizer<TNVEDGroup, TNVEDGroupVM>
    {
        protected override TNVEDGroup UnWrap(TNVEDGroupVM wrap)
        {
            return wrap.DomainObject as TNVEDGroup;
        }

        protected override TNVEDGroupVM Wrap(TNVEDGroup fill)
        {
            return new TNVEDGroupVM(fill);
        }
    }

    public class TNVEDGroupCommand : lib.ViewModelCommand<TNVEDGroup, TNVEDGroupVM, TNVEDGroupDBM>
    {
        internal TNVEDGroupCommand(TNVEDGroupVM vm, System.Windows.Data.ListCollectionView view) : base(vm, view)
        {
            mymaterials = new System.Windows.Data.ListCollectionView(References.Materials);
            mymaterials.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }
        internal TNVEDGroupCommand(System.Windows.Data.ListCollectionView view) : this(new TNVEDGroupVM(), view) { }

        private System.Windows.Data.ListCollectionView mymaterials;
        public System.Windows.Data.ListCollectionView Materials
        {
            get { return mymaterials; }
        }

        internal void GoodsDeleteExec(object parametr)
        {
            if (parametr is System.Collections.IList)
            {
                List<TNVEDGoodsVM> items = new List<TNVEDGoodsVM>();
                foreach (TNVEDGoodsVM item in parametr as System.Collections.IList)
                    items.Add(item);
                foreach (TNVEDGoodsVM item in items)
                {
                    if (item.DomainState == lib.DomainObjectState.Added)
                    {
                        item.DomainState = lib.DomainObjectState.Destroyed;
                        if (VModel.Goods.IsAddingNew && item == VModel.Goods.CurrentAddItem)
                            VModel.Goods.CancelNew();
                        else
                            VModel.Goods.Remove(item);
                    }
                    else
                    {
                        VModel.Goods.EditItem(item);
                        item.DomainState = lib.DomainObjectState.Deleted;
                        VModel.Goods.CommitEdit();
                    }
                }
            }
        }
        internal bool GoodsDeleteCanExec(object parametr)
        { return !(VModel.Goods.IsAddingNew | VModel.Goods.IsEditingItem); }

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

    public class TNVEDGroupViewCommand : lib.ViewModelViewCommand
    {
        internal TNVEDGroupViewCommand()
        {
            mysync = new TNVEDGroupSynchronizer();
            mysync.DomainCollection = new ObservableCollection<TNVEDGroup>();
            mygdbm = new TNVEDGroupDBM();
            mydbm = mygdbm;
            mygdbm.FillAsyncCompleted = () => {
                if (mygdbm.Errors.Count > 0)
                    OpenPopup(mygdbm.ErrorMessage, true);
                else
                {
                    mygoodsnamefiltercommand = new TNVEDGGoodsCheckListBoxVM();
                    mygoodsnamefiltercommand.DeferredFill = true;
                    mygoodsnamefiltercommand.ItemsSource = myview.OfType<TNVEDGroupVM>();
                    mygoodsnamefiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
                    mygoodsnamefiltercommand.ExecCommand2 = () => { mygoodsnamefiltercommand.Clear(); };

                    mytnvedfiltercommand = new TNVEDGroupTNVEDGroupCheckListBoxVM();
                    mytnvedfiltercommand.DeferredFill = true;
                    mytnvedfiltercommand.ItemsSource = myview.OfType<TNVEDGroupVM>();
                    mytnvedfiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
                    mytnvedfiltercommand.ExecCommand2 = () => { mytnvedfiltercommand.Clear(); };

                    mymaterialfiltercommand = new TNVEDGroupMaterialCheckListBoxVM();
                    mymaterialfiltercommand.DeferredFill = true;
                    mymaterialfiltercommand.ItemsSource = myview.OfType<TNVEDGroupVM>();
                    mymaterialfiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
                    mymaterialfiltercommand.ExecCommand2 = () => { mymaterialfiltercommand.Clear(); };
                }
            };
            mygdbm.Collection = mysync.DomainCollection;
            mygdbm.FillAsync();
            base.Collection = mysync.ViewModelCollection;
            mymaterials = new System.Windows.Data.ListCollectionView(References.Materials);
            mymaterials.Filter = delegate (object item) { Material mitem = item as Material; return Classes.Specification.MappingViewCommand.ViewFilterDefault(item) & (mitem.Id == 12 | mitem.Id == 13 | mitem.Upper?.Id == 15 | mitem.Upper?.Id == 16 | mitem.Upper?.Id == 22 | mitem.Upper?.Id == 23); };
            mymaterials.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            myexhandler = new DataModelClassLibrary.ExceptionHandler("Сохранение");
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            myexcelimport = new RelayCommand(ExcelImportExec, ExcelImportCanExec);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
        }

        private TNVEDGroupDBM mygdbm;
        private TNVEDGroupSynchronizer mysync;
        private lib.ExceptionHandler myexhandler;
        private System.ComponentModel.BackgroundWorker mybw;
        private ExcelImportWin myExcelImportWin;

        private System.Windows.Data.ListCollectionView mymaterials;
        public System.Windows.Data.ListCollectionView Materials { get { return mymaterials; } }

        private TNVEDGGoodsCheckListBoxVM mygoodsnamefiltercommand;
        public TNVEDGGoodsCheckListBoxVM GoodsNameFilterCommand
        { get { return mygoodsnamefiltercommand; } }
        private TNVEDGroupTNVEDGroupCheckListBoxVM mytnvedfiltercommand;
        public TNVEDGroupTNVEDGroupCheckListBoxVM TNVEDFilterCommand
        { get { return mytnvedfiltercommand; } }
        private TNVEDGroupMaterialCheckListBoxVM mymaterialfiltercommand;
        public TNVEDGroupMaterialCheckListBoxVM MaterialFilterCommand
        { get { return mymaterialfiltercommand; } }

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
            TNVEDGroupVM mapp = item as TNVEDGroupVM;
            if (mygoodsnamefiltercommand != null && mygoodsnamefiltercommand.FilterOn)
            {
                where = false;
                foreach (string name in mygoodsnamefiltercommand.SelectedItems)
                {
                    foreach (TNVEDGoods goods in mapp.DomainObject.Goods)
                    {
                        if (string.Equals(name, goods.Name.ToLower()))
                        {
                            where = true;
                            break;
                        }
                    }
                    if (where) break;
                }
            }
            if (where && mytnvedfiltercommand != null && mytnvedfiltercommand.FilterOn)
            {
                where = false;
                foreach (string name in mytnvedfiltercommand.SelectedItems)
                {
                    if (string.Equals(mapp.Group, name))
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
                    if (string.Equals(mapp.Material.Name, name))
                    {
                        where = true;
                        break;
                    }
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
            int maxr, n;
            string[] strs;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            TNVEDGroupVM newitem;

            Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
            Excel.Worksheet exWh = null;
            foreach (Excel.Worksheet sheetWh in exWb.Sheets) if (sheetWh.Name == "ГРУППА ТН ВЭД") { exWh = sheetWh; break; }
            if (exWh == null)
            {
                throw new Exception("Вкладка ГРУППА ТН ВЭД не найдена!");
            }
            maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            for (int r = 2; r <= maxr; r++)
            {
                if (string.IsNullOrEmpty((exWh.Cells[r, 1].Text as string).Trim())) continue;
                newitem = null;

                str.Clear();
                str.Append((exWh.Cells[r, 6].Text as string).Trim());
                if (str.Length > 0 & int.TryParse(str.ToString(), out n))
                    foreach (TNVEDGroupVM item in mysync.ViewModelCollection)
                        if (item.DomainObject.Id == n)
                        {
                            newitem = item;
                            break;
                        }
                if (newitem == null)
                    this.myview.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate () { newitem = new TNVEDGroupVM(); int p = newitem.Goods.Count; }));
                else if (ismiss) continue;

                str.Clear();
                str.Append((exWh.Cells[r, 1].Text as string).Trim());
                if (str.Length > 10)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 1].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Group = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 2].Text as string).Trim());
                if (str.Length > 0)
                {
                    Material matr = References.Materials.FindFirstItem("Name", str.ToString());
                    if (matr == null)
                        throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 4].Address(false, false) + " такой материал не найден!");
                    else
                        newitem.Material = matr;
                }

                bool contains = false;
                strs = (exWh.Cells[r, 2].Text as string).Trim().Replace((char)13, ',').Replace((char)10, ',').Replace("  ", " ").Replace(", ", ",").Replace(" ,", ",").Split(',');
                foreach (TNVEDGoods synitem in newitem.Goods)
                {
                    contains = false;
                    foreach (string stritem in strs)
                        contains |= synitem.Name.Equals(stritem.Trim());
                    if (!contains)
                        newitem.Goods.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Func<TNVEDGoods, bool>(newitem.DomainObject.Goods.Remove), synitem);
                }
                foreach (string stritem in strs)
                    if (stritem.Trim().Length > 50)
                        throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 2].Address(false, false) + " содержит слишком длинный текст для названия товара!");
                    else if (stritem.Trim().Length > 0)
                    {
                        contains = false;
                        foreach (TNVEDGoods synitem in newitem.Goods)
                            contains |= synitem.Name.Equals(stritem.Trim());
                        if (!contains)
                            newitem.Goods.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<TNVEDGoods>(newitem.DomainObject.Goods.Add), new TNVEDGoods(lib.NewObjectId.NewId, stritem.Trim(), lib.DomainObjectState.Added));
                    }

                if (!mysync.ViewModelCollection.Contains(newitem)) this.myview.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<TNVEDGroupVM>(mysync.ViewModelCollection.Add), newitem);
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
                exWh.Name = "Группы ТНВЭД";
                Excel.Range r;

                exWh.Cells[1, 1] = "ГРУППА ТН ВЭД"; exWh.Cells[1, 2] = "МАТЕРИАЛ"; exWh.Cells[1, 3] = "ТОВАР"; exWh.Cells[1, 6] = "ИД";

                exWh.Columns[1, Type.Missing].NumberFormat = "@";
                exWh.Columns[2, Type.Missing].NumberFormat = "@";
                exWh.Columns[3, Type.Missing].NumberFormat = "@";
                exWh.Columns[4, Type.Missing].NumberFormat = "@";
                exWh.Columns[5, Type.Missing].NumberFormat = "@";
                exWh.Columns[6, Type.Missing].NumberFormat = "@";
                foreach (object itemobj in myview)
                {
                    if (!(itemobj is TNVEDGroupVM)) continue;

                    TNVEDGroupVM item = itemobj as TNVEDGroupVM;
                    exWh.Cells[row, 1] = item.Group;
                    if (item.Material != null) exWh.Cells[row, 2] = item.Material.Name;
                    exWh.Cells[row, 3] = item.GoodsStr;
                    exWh.Cells[row, 6] = item.DomainObject.Id;

                    row++;
                }

                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, 3]];
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                r.VerticalAlignment = Excel.Constants.xlTop;
                r.WrapText = true;
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
            mygdbm.Collection.Clear();
            mygdbm.Fill();
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
            List<TNVEDGroupVM> destroied = new List<TNVEDGroupVM>();
            foreach (TNVEDGroupVM item in mysync.ViewModelCollection)
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
            foreach (TNVEDGroupVM item in destroied) mysync.ViewModelCollection.Remove(item);
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Group", System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class TNVEDGroupTNVEDGroupCheckListBoxVM : libui.CheckListBoxVMFill<TNVEDGroupVM, string>
    {
        protected override void AddItem(TNVEDGroupVM item)
        {
            if (!Items.Contains(item.Group)) Items.Add(item.Group);
        }
    }
    public class TNVEDGroupMaterialCheckListBoxVM : libui.CheckListBoxVMFill<TNVEDGroupVM, string>
    {
        protected override void AddItem(TNVEDGroupVM item)
        {
            if (!Items.Contains(item.Material.Name)) Items.Add(item.Material.Name);
        }
    }
    public class TNVEDGGoodsCheckListBoxVM : libui.CheckListBoxVMFill<TNVEDGroupVM, string>
    {
        protected override void AddItem(TNVEDGroupVM item)
        {
            foreach (TNVEDGoods goods in item.DomainObject.Goods)
            {
                if (!Items.Contains(goods.Name.ToLower())) Items.Add(goods.Name.ToLower());
            }
        }
    }
}
