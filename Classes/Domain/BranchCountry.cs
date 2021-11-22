using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.References;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class BranchCountry: System.ComponentModel.INotifyPropertyChanged
    {
        internal BranchCountry(GoodsVM goods, BranchVM[] branches)
        {
            mygoods = goods;
            mybranches = branches;
        }

        private GoodsVM mygoods;
        public GoodsVM Goods
        { set { mygoods = value; PropertyChangedNotification("Goods"); } get { return mygoods; } }
        private BranchVM[] mybranches;
        public BranchVM[] Branches { get { return mybranches; } }
        public bool IsDirty
        {
            get
            {
                bool isdirty = false;
                foreach (BranchVM item in mybranches)
                    if (item != null && item.IsDirty)
                    {
                        isdirty = true;
                        break;
                    }
                return isdirty;
            }
        }

        //INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        internal void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    public class BranchCountryDBM : DataModelClassLibrary.DBMSFill<BranchCountry>
    {
        internal BranchCountryDBM()
        {
            this.NeedAddConnection = true;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectCommandText = "[spec].[BranchCountry_sp]";
            SelectProcedure = true;
        }

        internal Country[] Countries { set; get; }

        protected override BranchCountry CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            if(this.Countries==null)
            {
                this.Countries = new Country[reader.FieldCount - 1];
                for (int i = 1; i < reader.FieldCount; i++)
                {
                    this.Countries[i - 1] = CustomBrokerWpf.References.Countries.FindFirstItem("Code", int.Parse(reader.GetName(i)));
                }
            }
            BranchCountry item = new BranchCountry(new GoodsVM(CustomBrokerWpf.References.GoodsStore.GetItemLoad(reader.GetInt32(0), addcon, out var errors)), new BranchVM[reader.FieldCount - 1]);
            this.Errors.AddRange(errors);
            for (int i = 1; i < reader.FieldCount; i++)
            {
                item.Branches[i - 1] = reader.IsDBNull(i) ? null : new BranchVM(CustomBrokerWpf.References.BranchStore.GetItemLoad(reader.GetInt32(i), addcon, out errors));
                this.Errors.AddRange(errors);
            }
            return item;
        }
        protected override void PrepareFill(SqlConnection addcon)
        {
        }
        protected override void CancelLoad()
        { }
    }

    public class BranchCountryCommand : DataModelClassLibrary.ViewModelBaseCommand
    {
        internal BranchCountryCommand()
        {
            mydbm = new BranchCountryDBM();
            mydbm.FillAsyncCompleted = () => 
            {
                if (mydbm.Errors.Count > 0)
                    OpenPopup(mydbm.ErrorMessage, true);
                else
                {
                    OpenPopup("Данные обновлены." + (myProducerFilterCommand.FilterOn | myCertificateFilterCommand.FilterOn ? string.Empty : " Задайте фильтр."), false);

                    myProducerFilterCommand.BranchCountry = mydbm.Collection;
                    myCertificateFilterCommand.BranchCountry = mydbm.Collection;

                    PropertyChangedNotification("ProducerFilterCommand");
                    PropertyChangedNotification("CertificateFilterCommand");
                }
            };
            mydbm.Collection = new System.Collections.ObjectModel.ObservableCollection<BranchCountry>();
            myview = new ListCollectionView(mydbm.Collection);
            myview.Filter = (object item) => { return false; } ;
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Goods.Certificate2", System.ComponentModel.ListSortDirection.Ascending));
            mybdbm = new BranchDBM();
            mygdbm = new GoodsDBM();
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            myexcelimport = new RelayCommand(ExcelImportExec, ExcelImportCanExec);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);

            myProducerFilterCommand = new BranchProducerCheckListBoxVM();
            myProducerFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
            myProducerFilterCommand.ExecCommand2 = () => { myProducerFilterCommand.Clear(); };
            myCertificateFilterCommand = new BranchCertificate2CheckListBoxVM();
            myCertificateFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
            myCertificateFilterCommand.ExecCommand2 = () => { myCertificateFilterCommand.Clear(); };

        }

        private BranchCountryDBM mydbm;
        private BranchDBM mybdbm;
        private GoodsDBM mygdbm;
        private ListCollectionView myview;
        public ListCollectionView Items { get { return myview; } }
        public Country[] Countries
        {
            get
            {
                return mydbm.Countries;
            }
        }
        private lib.TaskAsync.TaskAsync ExcelTask;

        private BranchProducerCheckListBoxVM myProducerFilterCommand;
        public BranchProducerCheckListBoxVM ProducerFilterCommand
        { get { return myProducerFilterCommand; } }
        private BranchCertificate2CheckListBoxVM myCertificateFilterCommand;
        public BranchCertificate2CheckListBoxVM CertificateFilterCommand
        { get { return myCertificateFilterCommand; } }
        public string ProducerColor
        {
            set
            {
                if (myview.CurrentItem != null)
                {
                    string producer = (myview.CurrentItem as BranchCountry).Goods.Producer;
                    foreach (BranchCountry item in mydbm.Collection)
                        if (producer.Equals(item.Goods.Producer))
                            item.Goods.ColorMark = value;
                }
            }
        }

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            this.EndEdit();
            myview.Filter = OnFilter;
        }
        private bool FilterRunCanExec(object parametr)
        { return true; }
        private bool OnFilter(object item)
        {
            GoodsVM gitem = (item as BranchCountry).Goods;
            bool where = lib.ViewModelViewCommand.ViewFilterDefault(gitem);
            if (where & myProducerFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myProducerFilterCommand.SelectedItems)
                    if (gitem.Producer == nameitem)
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myCertificateFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myCertificateFilterCommand.SelectedItems)
                    if (gitem.Certificate2 == nameitem)
                    {
                        where = true;
                        break;
                    }
            }
            return where;
        }

        private RelayCommand myfilterclear;
        public System.Windows.Input.ICommand FilterClear
        {
            get { return myfilterclear; }
        }
        private void FilterClearExec(object parametr)
        {
            myProducerFilterCommand.Clear();
            myProducerFilterCommand.IconVisibileChangedNotification();
            myCertificateFilterCommand.Clear();
            myCertificateFilterCommand.IconVisibileChangedNotification();
            OpenPopup("Фильтры очищены. Задайте новый фильтр", false);
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
            OpenFileDialog fd = new OpenFileDialog();
            fd.CheckPathExists = true;
            fd.CheckFileExists = true;
            fd.Multiselect = false;
            fd.Title = "Выбор файла с данными";
            fd.Filter = "Файл Excel |*.xls;*.xlsx";
            fd.ShowDialog();
            if (System.IO.File.Exists(fd.FileName))
            {
                if (ExcelTask == null)
                    ExcelTask = new lib.TaskAsync.TaskAsync();
                if(!ExcelTask.IsBusy)
                {
                    this.EndEdit();
                    ExcelTask.DoProcessing = OnExcelImport;
                    ExcelTask.Run(fd.FileName);
                }
                else
                {
                    System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
                }
            }
        }
        private bool ExcelImportCanExec(object parametr)
        { return ExcelTask == null || !ExcelTask.IsBusy; }

        private RelayCommand myexcelexport;
        public ICommand ExcelExport
        {
            get { return myexcelexport; }
        }
        private void ExcelExportExec(object parametr)
        {
            this.myendedit();
            if (ExcelTask == null)
                ExcelTask = new lib.TaskAsync.TaskAsync();
            if (!ExcelTask.IsBusy)
            {
                this.EndEdit();
                ExcelTask.DoProcessing = OnExcelExport;
                ExcelTask.Run(null);
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool ExcelExportCanExec(object parametr)
        { return ExcelTask == null || !ExcelTask.IsBusy; }

        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void RefreshData(object parametr)
        {
            mybdbm.FillAsyncCompleted = () => { if (mybdbm.Errors.Count > 0) OpenPopup(mybdbm.ErrorMessage, true); else mydbm.FillAsync(); };
            mybdbm.FillAsync();
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void RejectChanges(object parametr)
        {
            foreach (BranchCountry item in mydbm.Collection)
            {
                for (int i = 0; i < item.Branches.Length; i++)
                    if (item.Branches[i] != null)
                    {
                        if (item.Branches[i].DomainState == lib.DomainObjectState.Added)
                            item.Branches[i] = null;
                        else
                            item.Branches[i].RejectChanges();
                    }
            }
        }
        protected override bool CanRejectChanges()
        {
            return true; ;
        }
        protected override bool CanAddData(object parametr)
        {
            return false;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return false;
        }
        public override bool SaveDataChanges()
        {
            BranchVM branch;
            List<Branch> branches=new List<Branch>();
            List<Goods> goods = new List<Goods>();
            bool isSuccess = true, isvalid;
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            err.AppendLine("Изменения не сохранены");
            mygdbm.Errors.Clear();
            mybdbm.Errors.Clear();
            foreach (BranchCountry item in mydbm.Collection)
            {
                if (item.Goods.DomainState == lib.DomainObjectState.Added || item.Goods.DomainState == lib.DomainObjectState.Modified)
                    goods.Add(item.Goods.DomainObject);
                for (int i = 0; i < item.Branches.Length; i++)
                    if (item.Branches[i] != null)
                    {
                        branch = item.Branches[i];
                        if (branch.DomainState == lib.DomainObjectState.Added || branch.DomainState == lib.DomainObjectState.Modified || branch.DomainState == lib.DomainObjectState.Deleted)
                        {
                            isvalid = branch.DomainState == lib.DomainObjectState.Deleted || branch.Validate(true);
                            if (isvalid)
                                branches.Add(branch.DomainObject);
                            else
                                err.AppendLine(branch.Errors);
                            isSuccess &= isvalid;
                        }
                    }
            }

            if(goods.Count>0)
            {
                mygdbm.Collection = new System.Collections.ObjectModel.ObservableCollection<Goods>(goods);
                if (!mygdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    err.AppendLine(mygdbm.ErrorMessage);
                }
            }
            if (branches.Count > 0)
            {
                mybdbm.Collection = new System.Collections.ObjectModel.ObservableCollection<Branch>(branches);
                if (!mybdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    err.AppendLine(mydbm.ErrorMessage);
                }
            }
            if (!isSuccess)
                this.PopupText = err.ToString();
            return isSuccess;
        }
        protected override void AddData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override void DeleteData(object parametr)
        {
            throw new NotImplementedException();
        }

        internal void CountriesRefresh()
        {
            CustomBrokerWpf.References.Countries.Refresh();
            mydbm.Countries = null;
            mydbm.GetFirst();
        }
        private KeyValuePair<bool, string> OnExcelImport(object parm)
        {
            int maxr,maxc;
            string filepath = (string)parm,sert;

            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            try
            {
                exApp.Visible = false;
                exApp.DisplayAlerts = false;
                exApp.ScreenUpdating = false;

                Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
                Excel.Worksheet exWh = exWb.Sheets[1];
                maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                maxc = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;
                ExcelTask.ProgressChange(5);
                Country country;
                KeyValuePair<int, Country>[] countries = new KeyValuePair<int, Country>[maxc+1];
                for (int c = 3; c <= maxc; c++)
                {
                    sert = (exWh.Cells[1, c].Text as string).Trim();
                    if (!string.IsNullOrEmpty(sert))
                    {
                        country = CustomBrokerWpf.References.Countries.FindFirstItem("Name", sert);
                        if (country != null)
                        {
                            int i = 0;
                            foreach (Country thiscountry in mydbm.Countries)
                            {
                                if (country == thiscountry)
                                {
                                    countries[c] = new KeyValuePair<int, Country>(i, country);
                                    break;
                                }
                                else
                                    i++;
                            }
                        }
                        if (countries[c].Value == null)
                            return new KeyValuePair<bool, string>(true, "Страна " + sert + " не найдена в Филиалах!");
                    }
                    ExcelTask.ProgressChange(c, maxc,0.1M, 0.05M);
                }

                Branch branch;
                BranchCountry findbranchcnt;
                for (int r = 2; r <= maxr; r++)
                {
                    sert = (exWh.Cells[r, 1].Text as string)?.Trim().ToUpper();
                    if (string.IsNullOrEmpty(sert)) continue;
                    findbranchcnt = null;
                    foreach (BranchCountry branchctr in mydbm.Collection)
                    {
                        if (string.Equals(branchctr.Goods.Certificate2.ToUpper(), sert))
                        {
                            findbranchcnt = branchctr;
                            break;
                        }
                    }
                    if (findbranchcnt == null)
                        return new KeyValuePair<bool, string>(true, "Сертификат " + sert + " (ячейка Excel " + exWh.Cells[r, 1].Address(false, false) + ") не найден!");
                    else
                    {
                        double color = exWh.Cells[r, 1].Interior.Color?? 16777215D;
                        if (color != 16777215D) findbranchcnt.Goods.ColorMark = lib.Common.MsOfficeHelper.OfficeColorToString(color);
                        for (int c = 3; c <= maxc; c++)
                        {
                            sert = (exWh.Cells[r, c].Text as string).Trim();
                            if (!string.IsNullOrEmpty(sert))
                            {
                                if (countries[c].Value == null)
                                    return new KeyValuePair<bool, string>(true, "Для филиала (ячейка Excel " + exWh.Cells[r, c].Address(false, false) + ") не указана страна!");
                                else
                                {
                                    if (findbranchcnt.Branches[countries[c].Key] == null)
                                    {
                                        branch = new Branch();
                                        branch.Goods = findbranchcnt.Goods.DomainObject;
                                        branch.Country = countries[c].Value;
                                        branch.Name = sert;
                                        findbranchcnt.Branches[countries[c].Key] = new BranchVM(branch);
                                    }
                                    else
                                        findbranchcnt.Branches[countries[c].Key].Name = sert;
                                }
                            }
                        }
                    }
                    ExcelTask.ProgressChange(r,maxr, 0.85M, 0.15M);
                }
                exWb.Close();
                exApp.Quit();
                myview.Dispatcher.Invoke(new Action(myview.Refresh));
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
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
            return new KeyValuePair<bool, string>(false, maxr.ToString() + " строк обработано");
        }
        private KeyValuePair<bool, string> OnExcelExport(object parm)
        {
            Excel.Workbook exWb;
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();

            try
            {
                exApp.Visible = false;
                exApp.DisplayAlerts = false;
                exApp.ScreenUpdating = false;

                int row = 2, column=3;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                Excel.Range r;

                exWh.Cells[1, 1] = "НОМЕР ДС, СРТ"; exWh.Cells[1, 2] = "ПРОИЗВОДИТЕЛЬ";
                foreach(Country country in mydbm.Countries)
                {
                    exWh.Cells[1, column].Orientation = 90;
                    exWh.Cells[1, column] = country.Name;
                    column++;
                }
                ExcelTask.ProgressChange(5);
                double color= 16777215D;
                foreach (BranchCountry item in myview)
                {
                    exWh.Cells[row, 1] = item.Goods.Certificate2;
                    exWh.Cells[row, 2] = item.Goods.Producer;
                    if(item.Goods.ColorMark != null)
                    {
                        color = lib.Common.MsOfficeHelper.StringToOfficeColor((string)item.Goods.ColorMark);
                        exWh.Cells[row, 1].Interior.Color = color;
                        exWh.Cells[row, 2].Interior.Color = color;
                    }
                    column = 3;
                    foreach (BranchVM branch in item.Branches)
                    {
                        if (branch != null)
                        {
                            exWh.Cells[row, column] = branch.Name;
                            if (item.Goods.ColorMark != null)
                            {
                                exWh.Cells[row, column].Interior.Color = color;
                            }
                        }
                        column++;
                    }
                    row++;
                    ExcelTask.ProgressChange(row, myview.Count, 0.90M, 0.05M);
                }

                r = exWh.Range[exWh.Rows[1], exWh.Rows[1]];
                r.Font.Bold = true;
                r.VerticalAlignment = Excel.Constants.xlCenter;
                r.Rows.AutoFit();
                r.Columns.AutoFit();
                exWh.Range[exWh.Columns[1], exWh.Columns[2]].Columns.AutoFit();

                exWh.Range[exWh.Cells[1, 1], exWh.Cells[row - 1, 1]].Borders[Excel.XlBordersIndex.xlEdgeLeft].ColorIndex = 0;
                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[row-1, column-1]];
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideHorizontal].ColorIndex = 0;

                exApp.Visible = true;
                exApp.DisplayAlerts = true;
                exApp.ScreenUpdating = true;
                exWh = null;
                return new KeyValuePair<bool, string>(false, row.ToString() + " строк обработано");
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
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }

    }

    public class BranchProducerCheckListBoxVM : libui.CheckListBoxVM
    {
        internal BranchProducerCheckListBoxVM()
        {
            this.DisplayPath = System.Windows.Data.Binding.DoNothing;
            this.GetDisplayPropertyValueFunc = (item) => { return (string)item; };
            this.SearchPath = System.Windows.Data.Binding.DoNothing;
            this.ItemsViewFilterDefault = lib.ViewModelViewCommand.ViewFilterDefault;
            this.SelectedAll = false;
        }

        private IList<BranchCountry> mysource;
        public IList<BranchCountry> BranchCountry
        {
            set
            {
                mysource = value;
                Refresh();
            }
        }

        internal new void Refresh()
        {
            if (this.Items == null)
                this.Items = new List<string>();
            Items.Clear();
            foreach (BranchCountry item in mysource)
                if (!Items.Contains(item.Goods.Producer)) Items.Add(item.Goods.Producer);
            this.ItemsView.Refresh();
            PropertyChangedNotification("ItemsView");
        }
    }
    public class BranchCertificate2CheckListBoxVM : libui.CheckListBoxVM
    {
        internal BranchCertificate2CheckListBoxVM()
        {
            this.DisplayPath = System.Windows.Data.Binding.DoNothing;
            this.GetDisplayPropertyValueFunc = (item) => { return (string)item; };
            this.SearchPath = System.Windows.Data.Binding.DoNothing;
            this.ItemsViewFilterDefault = lib.ViewModelViewCommand.ViewFilterDefault;
            this.SelectedAll = false;
        }

        private IList<BranchCountry> mysource;
        public IList<BranchCountry> BranchCountry
        {
            set
            {
                mysource = value;
                Refresh();
            }
        }

        internal new void Refresh()
        {
            if (this.Items == null)
                this.Items = new List<string>();
            Items.Clear();
            foreach (BranchCountry item in mysource)
                if (!Items.Contains(item.Goods.Certificate2)) Items.Add(item.Goods.Certificate2);
            this.ItemsView.Refresh();
            PropertyChangedNotification("ItemsView");
        }
    }
}
