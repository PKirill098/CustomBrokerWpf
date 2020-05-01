using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public class ManagerDataGridColumnsVM
    {
        private ColumnMetadataList metadata;
        private List<SQLFilterGroupProperty> properties;
        private List<AggregateFunction> aggfunclist;
        private PreFunctionList prefunclist;
        private ListNotifyChanged<SQLFilterSaved> savedgroups;

        private CustomBrokerWpf.SQLFilter thisfilter;

        private CustomBrokerWpf.DinamicPropertySort comparer;

        private bool? isStopIndex;
        private ObservableCollection<ManagerDataGridColunm> mycolumns;
        private ListCollectionView columnsview;
        private DataGrid mycontrolleddg;
        private Grid mycontrolledtg;
        private ManagerDataGridColunm colunmManaged;
        private BitVisibilityConverter itemvsblcnv;

        private RelayCommand changeDisplayIndex;
        private RelayCommand getReportData;
        private RelayCommand saveDefault;
        private RelayCommand loadDefault;
        private RelayCommand addcolumn;
        private RelayCommand addsavedgroup;
        private RelayCommand loadsavedgroup;
        private RelayCommand refreshsavedgroup;
        private RelayCommand savesavedgroup;
        private RelayCommand defaultsavedgroup;
        private RelayCommand deletesavedgroup;
        private RelayCommand openchart;

        private VisibleAllColumns visibleAll;

        private Classes.Chart.ChartManager mychartmanager;

        internal Action DataLoad;

        internal ManagerDataGridColumnsVM(string filterClass, DataGrid controlled, Grid total, Window reportwin)
            : base()
        {
            isStopIndex = null;
            mycontrolleddg = controlled;
            mycontrolledtg = total;
            itemvsblcnv = new BitVisibilityConverter();

            thisfilter = new SQLFilter(filterClass, "AND");

            comparer = new DinamicPropertySort();

            savedgroups = new ListNotifyChanged<SQLFilterSaved>();
            RefreshSavedGroupsExec(null);
            metadata = new ColumnMetadataList(filterClass);
            aggfunclist = new List<AggregateFunction>();
            thisfilter.FillAggregateFunction(aggfunclist);
            prefunclist = new PreFunctionList();
            mycolumns = new ObservableCollection<ManagerDataGridColunm>();
            mycolumns.CollectionChanged += columns_CollectionChanged;
            columnsview = CollectionViewSource.GetDefaultView(mycolumns) as ListCollectionView;
            columnsview.SortDescriptions.Add(new SortDescription("DisplayIndex", ListSortDirection.Ascending));
            properties = new List<SQLFilterGroupProperty>();
            FillColumns();

            ChartType = 0;
            mychartmanager = new Classes.Chart.ChartManager(reportwin);
        }

        public ObservableCollection<ManagerDataGridColunm> Columns { set { mycolumns = value; } get { return mycolumns; } }
        void columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                ManagerDataGridColunm managercolumn = e.NewItems[0] as ManagerDataGridColunm;
                managercolumn.ManagerColumns = this;
                managercolumn.AllAggregateFunctions = aggfunclist;
                managercolumn.AllPreFunctions = prefunclist;
                managercolumn.ControlledDataGrid = mycontrolleddg;
                managercolumn.TotalGrid = mycontrolledtg;
                //managercolumn.Converter = converter;
                managercolumn.PropertyChanged += ListenerPropertyChanged;
                this.properties.Add(managercolumn.GroupProperty);
                this.TotalAdd(managercolumn);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                int c = 0;
                foreach (ManagerDataGridColunm managercolumn in e.OldItems)
                {
                    for (int i=0;i< mycontrolledtg.Children.Count;i++)
                    {
                        TextBox item = mycontrolledtg.Children[i] as TextBox;
                        if ((int)item.Tag == managercolumn.GetHashCode())
                        {
                            mycontrolledtg.Children.Remove(item);
                            c = Grid.GetColumn(item);
                            i--;
                        }
                    }
                    mycontrolledtg.ColumnDefinitions.RemoveAt(c);
                    foreach (TextBox item in mycontrolledtg.Children)
                    {
                        if(Grid.GetColumn(item)>c)
                            Grid.SetColumn(item, Grid.GetColumn(item)-1);
                    }
                    managercolumn.PropertyChanged -= ListenerPropertyChanged;
                    managercolumn.DeleteColumn();
                }
            }
        }

        public SQLFilter Filter
        {
            get { return thisfilter; }
            set
            {
                thisfilter.RemoveCurrentWhere();
                thisfilter = value;
                //if (this.IsLoaded)
                //    DataLoad();
            }
        }
        public ColumnMetadataList ColumnsMetadata { get { return metadata; } }
        public ManagerDataGridColunm ColumnManaged
        {
            set { colunmManaged = value; }
            get { return colunmManaged; }
        }
        public ListNotifyChanged<SQLFilterSaved> SavedGroups { get { return savedgroups; } }
        public string SavedGroupName { set; get; }
        public byte ChartType { set; get; }
        internal BitVisibilityConverter ItemVisibilityConverter { get { return itemvsblcnv; } }
        internal CustomBrokerWpf.DinamicPropertySort Sorter { get { return comparer; } }
        internal void ListenerPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ManagerDataGridColunm column;
            switch (e.PropertyName)
            {
                case "DisplayIndex":
                    column = sender as ManagerDataGridColunm;
                    if (!(columnsview.IsAddingNew | columnsview.IsEditingItem))
                    {
                        if (!isStopIndex.HasValue)
                        {
                            isStopIndex = column.OldDisplayIndex > column.DisplayIndex;
                        }
                        else
                        {
                            if (isStopIndex == (column.OldDisplayIndex < column.DisplayIndex))
                            {
                                isStopIndex = null;
                                columnsview.Refresh();
                            }
                        }
                    }
                    if (this.mycontrolleddg.IsLoaded)
                    {
                        foreach (TextBox item in mycontrolledtg.Children)
                        {
                            if ((int)item.Tag == column.GetHashCode())
                            {
                                mycontrolledtg.ColumnDefinitions[column.DisplayIndex.Value].Width = new GridLength(column.IsVisible ? column.ManagedColumn.ActualWidth : 0D);
                                Grid.SetColumn(item, column.DisplayIndex.Value);
                            }
                        }
                    }
                    break;
                case "MetaData":
                    column = sender as ManagerDataGridColunm;
                    foreach (TextBox item in mycontrolledtg.Children)
                    {
                        if ((int)item.Tag == column.GetHashCode())
                        {
                            switch (column.AggregateFunction == "CNT" ? "00ncr" : column.MetaData?.Datatype ?? string.Empty)
                            {
                                case "00nrt":
                                case "02nrt":
                                    item.TextAlignment = TextAlignment.Right;
                                    break;
                                case "00ncr":
                                case "00fcr":
                                    item.TextAlignment = TextAlignment.Center;
                                    break;
                            }
                        }
                    }
                    break;
                case "ActualWidth":
                case "IsVisible":
                    column = sender as ManagerDataGridColunm;
                    foreach (TextBox item in mycontrolledtg.Children)
                    {
                        if ((int)item.Tag == column.GetHashCode())
                        {
                            mycontrolledtg.ColumnDefinitions[Grid.GetColumn(item)].Width = new GridLength(column.IsVisible ? column.ManagedColumn.ActualWidth : 0D);
                            break;
                        }
                    }
                    break;
                case "ToolTipText":
                    column = sender as ManagerDataGridColunm;
                    foreach (TextBox item in mycontrolledtg.Children)
                    {
                        if ((int)item.Tag == column.GetHashCode())
                        {
                            item.ToolTip = column.ToolTipText+(Grid.GetRow(item)==0?" (Сумма)":" (Среднее)");
                        }
                    }
                    break;
            }
        }

        internal int ColumnsCount { get { return mycolumns.Count; } }
        #region Command
        public ICommand VisibleAll
        {
            get
            {
                if (this.visibleAll == null) visibleAll = new VisibleAllColumns(mycolumns);
                return visibleAll;
            }
        }

        public ICommand ChangeDisplayIndex
        {
            get
            {
                if (changeDisplayIndex == null)
                {
                    changeDisplayIndex = new RelayCommand(this.DisplayIndexChange, this.CanDisplayIndexChange);
                }
                return changeDisplayIndex;
            }
        }
        private void DisplayIndexChange(object parametr)
        {
            int selectedcount = 0;
            ManagerDataGridColunm[] selected = new ManagerDataGridColunm[mycolumns.Count];
            foreach (ManagerDataGridColunm column in this.mycolumns)
            {
                if (column.isSelected)
                {
                    selected[selectedcount] = column;
                    selectedcount++;
                }
            }
            if (selectedcount == 0) return;
            if (parametr.ToString() == "Up")
            {
                if (selected[0].DisplayIndex > 0) for (int i = 0; i < selectedcount; i++) selected[i].DisplayIndex--;
            }
            else
            {
                if (selected[selectedcount - 1].DisplayIndex < mycolumns.Count - 1) for (int i = selectedcount - 1; i > -1; i--) selected[i].DisplayIndex++;
            }
            for (int i = 0; i < selectedcount; i++) selected[i].isSelected = true;
        }
        private bool CanDisplayIndexChange(object parametr)
        {
            return true;//(columns.Count > 0 && ((parametr.ToString() == "Up" & !columns.Single(i => i.DisplayIndex == 0).isSelected) | (parametr.ToString() != "Up" & !columns.Single(i=> i.DisplayIndex==columns.Count-1).isSelected)));
        }

        public ICommand GetReportData
        {
            get
            {
                if (getReportData == null) getReportData = new RelayCommand(this.ReportDataGet, this.CanReportDataGet);
                return getReportData;
            }
        }
        private void ReportDataGet(object parametr)
        {
            try
            {
                SaveGroup();
                PrepareColumns();
                DataLoad();
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999) MessageBox.Show(err.Message, "Подготовка отчета", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                        {
                            errs.Append(sqlerr.Message + "\n");
                        }
                        MessageBox.Show(errs.ToString(), "Подготовка отчета", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Подготовка отчета", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool CanReportDataGet(object parametr)
        { return ColumnsHasErr(); }

        public ICommand SaveDefault
        {
            get
            {
                if (saveDefault == null) saveDefault = new RelayCommand(this.SaveDefaultExec, this.SaveDefaultCanExec);
                return saveDefault;
            }
        }
        private void SaveDefaultExec(object parametr)
        {
            if (MessageBox.Show("Настройки столбцов по умолчанию будет заменены текущими настройками.\nПродолжить?", "Сохранение фильтра", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
            {
                try
                {
                    SaveGroup();
                    this.thisfilter.SetDefaultFilterGroup();
                }
                catch (Exception ex)
                {
                    if (ex is System.Data.SqlClient.SqlException)
                    {
                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                        if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private bool SaveDefaultCanExec(object parametr)
        {
            return ColumnsHasErr();
        }
        public ICommand LoadDefault
        {
            get
            {
                if (loadDefault == null) loadDefault = new RelayCommand(this.LoadDefaultExec, this.LoadDefaultCanExec);
                return loadDefault;
            }
        }
        private void LoadDefaultExec(object parametr)
        {
            try
            {
                while (this.mycolumns.Count > 0) this.mycolumns.RemoveAt(0);
                this.properties.Clear();
                this.thisfilter.RemoveCurrentGroup();

                this.thisfilter.GetDefaultFilter(SQLFilterPart.Group);
                FillColumns();
                DataLoad();
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                        {
                            errs.Append(sqlerr.Message + "\n");
                        }
                        MessageBox.Show(errs.ToString(), "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool LoadDefaultCanExec(object parametr)
        { return true; }

        public ICommand AddColumn
        {
            get
            {
                if (addcolumn == null) addcolumn = new RelayCommand(this.AddColumnExec, this.AddColumnCanExec);
                return addcolumn;
            }
        }
        private void AddColumnExec(object parametr)
        {
            this.mycolumns.Add(new ManagerDataGridColunm());
        }
        private bool AddColumnCanExec(object parametr)
        { return ColumnsHasErr(); }

        public ICommand AddSavedGroups
        {
            get
            {
                if (addsavedgroup == null) addsavedgroup = new RelayCommand(this.AddSavedGroupsExec, this.AddSavedGroupsCanExec);
                return addsavedgroup;
            }
        }
        private void AddSavedGroupsExec(object parametr)
        {
            this.savedgroups.Add(new SQLFilterSaved());
            this.savedgroups.OnResetCollectionChanged();
        }
        private bool AddSavedGroupsCanExec(object parametr)
        { return false; }
        public ICommand LoadSavedGroups
        {
            get
            {
                if (loadsavedgroup == null) loadsavedgroup = new RelayCommand(this.LoadSavedGroupsExec, this.LoadSavedGroupsCanExec);
                return loadsavedgroup;
            }
        }
        private void LoadSavedGroupsExec(object parametr)
        {
            ListCollectionView view;
            view = CollectionViewSource.GetDefaultView(this.savedgroups) as ListCollectionView;
            if (!(view.CurrentPosition < 0))
            {
                try
                {
                    while (this.mycolumns.Count > 0) this.mycolumns.RemoveAt(0);
                    this.properties.Clear();
                    this.thisfilter.RemoveCurrentGroup();

                    this.thisfilter.GetFilter(0, (view.CurrentItem as SQLFilterSaved).Id);
                    FillColumns();
                    DataLoad();
                }
                catch (Exception ex)
                {
                    if (ex is System.Data.SqlClient.SqlException)
                    {
                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                        if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }
        private bool LoadSavedGroupsCanExec(object parametr)
        { return !((CollectionViewSource.GetDefaultView(this.savedgroups) as ListCollectionView).CurrentPosition < 0); }
        public ICommand RefreshSavedGroups
        {
            get
            {
                if (refreshsavedgroup == null) refreshsavedgroup = new RelayCommand(this.RefreshSavedGroupsExec, this.RefreshSavedGroupsCanExec);
                return refreshsavedgroup;
            }
        }
        private void RefreshSavedGroupsExec(object parametr)
        {
            this.savedgroups.Clear();
            this.thisfilter.FillSavedGroups(savedgroups);
            this.savedgroups.OnResetCollectionChanged();
        }
        private bool RefreshSavedGroupsCanExec(object parametr)
        { return true; }
        public ICommand SaveSavedGroups
        {
            get
            {
                if (savesavedgroup == null) savesavedgroup = new RelayCommand(this.SaveSavedGroupsExec, this.SaveSavedGroupsCanExec);
                return savesavedgroup;
            }
        }
        private void SaveSavedGroupsExec(object parametr)
        {
            ListCollectionView view = CollectionViewSource.GetDefaultView(this.savedgroups) as ListCollectionView;
            if (view.CurrentPosition < 0)
            {
                SQLFilterSaved newgroup = new SQLFilterSaved(0, SavedGroupName, true);
                this.savedgroups.Add(newgroup);
                this.savedgroups.Sort(delegate (SQLFilterSaved x, SQLFilterSaved y) { return x.Name.CompareTo(y.Name); });
                this.savedgroups.OnResetCollectionChanged();
                view.MoveCurrentTo(newgroup);
            }
            else if (MessageBox.Show("Выбранные настройки будут заменены текущими.\nПродолжить?", "Сохранение настроек", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            try
            {
                SaveGroup();
                this.thisfilter.SetSavedGroup(view.CurrentItem as SQLFilterSaved);
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                        {
                            errs.Append(sqlerr.Message + "\n");
                        }
                        MessageBox.Show(errs.ToString(), "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        private bool SaveSavedGroupsCanExec(object parametr)
        {
            ListCollectionView view = CollectionViewSource.GetDefaultView(this.savedgroups) as ListCollectionView;
            return ColumnsHasErr() & !string.IsNullOrEmpty(this.SavedGroupName) & (view.CurrentPosition < 0 || !(view.CurrentItem as SQLFilterSaved).IsReadOnly);
        }
        public ICommand DefaultSavedGroups
        {
            get
            {
                if (defaultsavedgroup == null) defaultsavedgroup = new RelayCommand(this.DefaultSavedGroupsExec, this.DefaultSavedGroupsCanExec);
                return defaultsavedgroup;
            }
        }
        private void DefaultSavedGroupsExec(object parametr)
        {
            ListCollectionView view;
            if (MessageBox.Show("Настройки столбцов по умолчанию будут заменены выбранными сохраненными настройками.\nПродолжить?", "Сохранение настроек", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
            {
                view = CollectionViewSource.GetDefaultView(this.savedgroups) as ListCollectionView;
                if (!(view.CurrentPosition < 0))
                {
                    try
                    {
                        this.thisfilter.SetDefaultSavedFilter(null, view.CurrentItem as SQLFilterSaved);
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Data.SqlClient.SqlException)
                        {
                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                            if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                            else
                            {
                                System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                {
                                    errs.Append(sqlerr.Message + "\n");
                                }
                                MessageBox.Show(errs.ToString(), "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
        private bool DefaultSavedGroupsCanExec(object parametr)
        { return !((CollectionViewSource.GetDefaultView(this.savedgroups) as ListCollectionView).CurrentPosition < 0); }
        public ICommand DeleteSavedGroups
        {
            get
            {
                if (deletesavedgroup == null) deletesavedgroup = new RelayCommand(this.DeleteSavedGroupsExec, this.DeleteSavedGroupsCanExec);
                return deletesavedgroup;
            }
        }
        private void DeleteSavedGroupsExec(object parametr)
        {
            if (MessageBox.Show("Выбранные сохраненные настройки будут удалены.\nПродолжить?", "Сохранение настроек", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
            {
                SQLFilterSaved deleting = (CollectionViewSource.GetDefaultView(this.savedgroups) as ListCollectionView).CurrentItem as SQLFilterSaved;
                if (deleting != null)
                {
                    try
                    {
                        this.thisfilter.DeleteSavedGroup(deleting);
                        this.savedgroups.Remove(deleting);
                        this.savedgroups.OnResetCollectionChanged();
                    }
                    catch (Exception ex)
                    {
                        if (ex is System.Data.SqlClient.SqlException)
                        {
                            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                            if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                            else
                            {
                                System.Text.StringBuilder errs = new System.Text.StringBuilder();
                                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                                {
                                    errs.Append(sqlerr.Message + "\n");
                                }
                                MessageBox.Show(errs.ToString(), "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
        private bool DeleteSavedGroupsCanExec(object parametr)
        {
            ListCollectionView view = CollectionViewSource.GetDefaultView(this.savedgroups) as ListCollectionView;
            return !(view.CurrentPosition < 0 || (view.CurrentItem as SQLFilterSaved).IsReadOnly);
        }

        public ICommand OpenChart
        {
            get
            {
                if (openchart == null) openchart = new RelayCommand(this.OpenChartExec, this.OpenChartCanExec);
                return openchart;
            }
        }
        private void OpenChartExec(object parametr)
        {
            if (mycontrolleddg.ItemsSource is List<Dictionary<string, DinamicPropertyItem>>)
            {
                int n = 0;
                string dimen = string.Empty;
                string sersource = string.Empty, serkey = string.Empty;
                List<object> dimenpoints = new List<object>();
                SortedList<object, string> series = new SortedList<object, string>();
                List<KeyValuePair<string, string>> measures = new List<KeyValuePair<string, string>>();
                mychartmanager.ChartSeries.Clear();
                mychartmanager.ChartType = this.ChartType < 2 ? (this.ChartType == 0 ? Classes.Chart.SeriesType.Column : Classes.Chart.SeriesType.Pie) : (this.ChartType == 2 ? Classes.Chart.SeriesType.Area : Classes.Chart.SeriesType.Line);
                CollectionView dataset;
                if (mycontrolleddg.SelectedItems.Count > 1)
                    dataset = CollectionViewSource.GetDefaultView(mycontrolleddg.SelectedItems) as CollectionView;
                else
                    dataset = CollectionViewSource.GetDefaultView(mycontrolleddg.ItemsSource) as CollectionView;
                try
                {
                    foreach (ManagerDataGridColunm column in this.mycolumns)
                    {
                        if (column.ChartValueType == "dimen")
                        {
                            dimen = column.GroupProperty.SelectName;
                            mychartmanager.AxisXTitle = column.Header;
                        }
                        else if (column.ChartValueType == "seres")
                        {
                            serkey = column.GroupProperty.SortName;
                            sersource = column.GroupProperty.SelectName;
                            mychartmanager.LegendTitle = column.Header;
                        }
                        else if (column.ChartValueType == "measr")
                        {
                            measures.Add(new KeyValuePair<string, string>(column.GroupProperty.SelectName, column.Header));
                            Classes.Chart.Series oneseries = new Classes.Chart.Series();
                            oneseries.Name = column.Header;
                            oneseries.DataPoints = new ListNotifyChanged<KeyValuePair<object, float>>();
                            mychartmanager.ChartSeries.Add(oneseries);
                        }
                    }
                    foreach (Dictionary<string, DinamicPropertyItem> item in dataset)
                    {
                        if (!(string.IsNullOrEmpty(dimen) || dimenpoints.Contains(item[dimen].Value))) dimenpoints.Add(item[dimen].Value);
                        if (!(string.IsNullOrEmpty(serkey) || series.ContainsKey(item[serkey].Value))) series.Add(item[serkey].Value, (string)item[sersource].Value.ToString());
                    }
                    if (dimen != string.Empty & measures.Count > 0)
                    {
                        mychartmanager.AxisYTitle = string.Empty;
                        if (measures.Count > 1)
                        {
                            for (int s = 0; s < series.Count; s++)
                            {
                                for (int m = 0; m < measures.Count; m++)
                                {
                                    if (s > 0)
                                    {
                                        Classes.Chart.Series oneseries = new Classes.Chart.Series();
                                        oneseries.Name = series.Values[s] + " (" + measures[m].Value + ")";
                                        oneseries.DataPoints = new ListNotifyChanged<KeyValuePair<object, float>>();
                                        mychartmanager.ChartSeries.Add(oneseries);
                                    }
                                    else
                                    {
                                        mychartmanager.ChartSeries[m].Name = series.Values[0] + " (" + measures[m].Value + ")";
                                    }
                                }
                            }
                        }
                        else
                        {
                            mychartmanager.AxisYTitle = mychartmanager.ChartSeries[0].Name;
                            for (int s = 0; s < series.Count; s++)
                            {
                                if (s > 0)
                                {
                                    Classes.Chart.Series oneseries = new Classes.Chart.Series();
                                    oneseries.Name = series.Values[s];
                                    oneseries.DataPoints = new ListNotifyChanged<KeyValuePair<object, float>>();
                                    mychartmanager.ChartSeries.Add(oneseries);
                                }
                                else mychartmanager.ChartSeries[0].Name = series.Values[0];
                            }
                        }
                        foreach (Dictionary<string, DinamicPropertyItem> item in dataset)
                        {
                            if (!string.IsNullOrEmpty(sersource)) n = series.IndexOfKey(item[serkey].Value) * measures.Count;
                            for (int i = 0; i < measures.Count; i++)
                            {
                                mychartmanager.ChartSeries[n + i].DataPoints.Add(new KeyValuePair<object, float>(item[dimen].Value, Convert.ToSingle(item[measures[i].Key].Value)));
                            }
                        }
                        if (series.Count > 1)
                            foreach (Classes.Chart.Series seritem in mychartmanager.ChartSeries)
                            {
                                foreach (object point in dimenpoints)
                                { if (!seritem.DataPoints.Exists(x => x.Key.Equals(point))) seritem.DataPoints.Add(new KeyValuePair<object, float>(point, 0f)); }
                                seritem.DataPoints.Sort(delegate (KeyValuePair<object, float> x, KeyValuePair<object, float> y)
                                { return dimenpoints.FindIndex(d => d.Equals(x.Key)).CompareTo(dimenpoints.FindIndex(d => d.Equals(y.Key))); });
                            }
                        mychartmanager.CreateChart();
                    }
                    else MessageBox.Show("Необходимо указать столбец для набора и значений диаграммы.", "Построение диаграммы", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Построение диаграммы", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool OpenChartCanExec(object parametr)
        {
            return ColumnsHasErr();
        }
        #endregion
        private bool ColumnsHasErr()
        {
            bool iserr = true;
            foreach (ManagerDataGridColunm column in this.mycolumns)
            {
                if (column.Error.Length > 0)
                {
                    iserr = false;
                    break;
                }
            }
            return iserr;
        }
        private void ColumnsCommitEdit()
        {
            if (columnsview.IsAddingNew) columnsview.CommitNew();
            if (columnsview.IsEditingItem) columnsview.CommitEdit();
        }
        private void ColumnsCancelEdit()
        {
            if (columnsview.IsAddingNew) columnsview.CancelNew();
            if (columnsview.IsEditingItem) columnsview.CancelEdit();
        }
        private void SaveGroup()
        {
            if (columnsview.IsAddingNew) columnsview.CommitNew();
            if (columnsview.IsEditingItem) columnsview.CommitEdit();
            mycontrolleddg.CommitEdit(DataGridEditingUnit.Row, true);
            foreach (ManagerDataGridColunm column in mycolumns)
            {
                int sortorder = 0;
                column.PrepareProperties();
                for (int i = 0; i < this.comparer.Propertys.Count; i++)
                {
                    if (column.GroupProperty.SortName == this.comparer.Propertys[i].Property)
                    {
                        sortorder = 1 + (this.comparer.Propertys[i].Direction == System.ComponentModel.ListSortDirection.Ascending ? i : -i);
                        break;
                    }
                }
                column.GroupProperty.SortOrder = sortorder;
            }
            thisfilter.SetGroupProperties(properties);
        }
        private void PrepareColumns()
        {
            foreach (ManagerDataGridColunm column in mycolumns)
            {
                if (column.GroupProperty.State != SQLFilterGroupPropertyState.Unchanged)
                {
                    column.PrepareColumnFill();
                    if (column.GroupProperty.SortOrder != 0)
                        this.comparer.Propertys[(column.GroupProperty.SortOrder > 0 ? column.GroupProperty.SortOrder : -column.GroupProperty.SortOrder) - 1].Property = column.GroupProperty.SortName;
                }
            }
        }
        private void FillColumns()
        {
            thisfilter.FillGroupProperties(properties);
            int count = properties.Count;
            for (int i = 0; i < count; i++)
            {
                ManagerDataGridColunm managerColunm = new ManagerDataGridColunm();
                mycolumns.Add(managerColunm);
                managerColunm.MetaData = metadata.FindFirstItem(properties[i].BasisId);
                managerColunm.GroupProperty = properties[i];
                managerColunm.PrepareColumnFill();
                SortDescriptor descr = new SortDescriptor();
                if (properties[i].SortOrder != 0)
                {
                    descr.Property = properties[i].SortName;
                    descr.Direction = properties[i].SortOrder > 0 ? System.ComponentModel.ListSortDirection.Ascending : System.ComponentModel.ListSortDirection.Descending;
                    if ((properties[i].SortOrder > 0 ? properties[i].SortOrder : -properties[i].SortOrder) > this.comparer.Propertys.Count)
                        this.comparer.Propertys.Add(descr);
                    else
                        this.comparer.Propertys.Insert((properties[i].SortOrder > 0 ? properties[i].SortOrder : -properties[i].SortOrder) - 1, descr);
                    managerColunm.ManagedColumn.SortDirection = descr.Direction;
                }
            }
            while (properties.Count > count) properties.RemoveAt(count);
            // настройка строки сумм
            mycontrolledtg.Children.Clear();
            mycontrolledtg.ColumnDefinitions.Clear();
            foreach (ManagerDataGridColunm item in mycolumns)
            {
                this.TotalAdd(item);
            }
        }
        private void TotalAdd(ManagerDataGridColunm item)
        {
            ColumnDefinition coldef = new ColumnDefinition();
            coldef.Width = new GridLength(item.IsVisible ? item.ManagedColumn.ActualWidth : 0D);
            mycontrolledtg.ColumnDefinitions.Add(coldef);
            TextBox tbox = new TextBox();
            tbox.Tag = item.GetHashCode();
            tbox.ToolTip = item.ToolTipText + " (Сумма)";
            switch (item.AggregateFunction == "CNT" ? "00ncr" : item.MetaData?.Datatype ?? string.Empty)
            {
                case "00nrt":
                case "02nrt":
                    tbox.TextAlignment = TextAlignment.Right;
                    break;
                case "00ncr":
                case "00fcr":
                    tbox.TextAlignment = TextAlignment.Center;
                    break;
            }
            Grid.SetRow(tbox, 0);
            Grid.SetColumn(tbox, item.DisplayIndex ?? 0);
            mycontrolledtg.Children.Add(tbox);

            tbox = new TextBox();
            tbox.Tag = item.GetHashCode();
            tbox.ToolTip = item.ToolTipText + " (Среднее)";
            switch (item.AggregateFunction == "CNT" ? "00ncr" : item.MetaData?.Datatype ?? string.Empty)
            {
                case "00nrt":
                case "02nrt":
                    tbox.TextAlignment = TextAlignment.Right;
                    break;
                case "00ncr":
                case "00fcr":
                    tbox.TextAlignment = TextAlignment.Center;
                    break;
            }
            Grid.SetRow(tbox, 1);
            Grid.SetColumn(tbox, item.DisplayIndex ?? 0);
            mycontrolledtg.Children.Add(tbox);
        }
        internal void Sorting(DataGridColumn dgc)
        {
            for (int i = 0; i < this.Sorter.Propertys.Count; i++)
            {
                DataGridColumn column = this.mycontrolleddg.Columns.FirstOrDefault(x => x.SortMemberPath == this.Sorter.Propertys[i].Property);
                if (column == null || !column.SortDirection.HasValue)
                { this.Sorter.Propertys.RemoveAt(i); i--; }
            }
            SortDescriptor descriptor = this.Sorter.Propertys.FirstOrDefault(x => x.Property == dgc.SortMemberPath);
            if (descriptor == null)
            {
                descriptor = new SortDescriptor();
                descriptor.Property = dgc.SortMemberPath;
                this.Sorter.Propertys.Add(descriptor);
            }
            if (dgc.SortDirection.HasValue && dgc.SortDirection.Value == System.ComponentModel.ListSortDirection.Ascending)
            {
                dgc.SortDirection = System.ComponentModel.ListSortDirection.Descending;
                descriptor.Direction = System.ComponentModel.ListSortDirection.Descending;
            }
            else
            {
                dgc.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                descriptor.Direction = System.ComponentModel.ListSortDirection.Ascending;
            }
        }
    }

    public class ManagerDataGridColunm : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            if (propertyName.Equals("MetaData") | propertyName.Equals("AggregateFunction") | propertyName.Equals("PreFunction"))
            {
                if (metadata != null)
                {
                    if (aggfunc == myproperties.AggrigateFunction & prefunc == myproperties.PreFunction & metadata.Id == myproperties.BasisId & (metadata.IsStatic || (column as DataGridTextColumn).Binding == null))
                        PrepareColumnFill();
                    else if ((metadata.IsStatic || (column as DataGridTextColumn).Binding != null) & !(aggfunc == myproperties.AggrigateFunction & prefunc == myproperties.PreFunction & metadata.Id == myproperties.BasisId))
                        UnmakeColumn();
                }
                else if ((column as DataGridTextColumn).Binding != null)
                    UnmakeColumn();
            }
        }

        public string Error
        {
            get
            {
                string errmsg = string.Empty;
                if (this.metadata == null) errmsg = "Не указан настраиваемый столбец!";
                return errmsg;
            }
        }
        public string this[string columnName]
        {
            get
            {
                string errmsg = String.Empty;
                switch (columnName)
                {
                    case "MetaData":
                        if (this.metadata == null) errmsg = "Необходимо указать настраиваемый столбец!";
                        break;
                }
                return errmsg;
            }
        }

        ManagerDataGridColumnsVM managerColumns;
        private DataGrid mycontrolleddg;
        private Grid mytotalgrid;
        internal Grid TotalGrid
        { set { mytotalgrid = value; } }
        DataGridColumn column;
        DinamicPropertyConvertor converter;

        bool iselected;
        int olddispleyindex, sortorder;
        private string aggfunc, prefunc, chartvaluetype, mychartnumber;
        ColumnMetaData metadata;
        private SQLFilterGroupProperty myproperties;
        ListCollectionView aggfunclist;
        ListCollectionView prefunclist;
        private ListNotifyChanged<KeyValuePair<string, string>> mychartvaluetypes;

        public ManagerDataGridColunm()
            : base()
        {
            myproperties = new SQLFilterGroupProperty();
            CreateDataGridColumn();
            this.mychartvaluetypes = new ListNotifyChanged<KeyValuePair<string, string>>();
        }
        internal ManagerDataGridColunm(SQLFilterGroupProperty properties)
            : this()
        {
            this.myproperties = properties;
        }
        ~ManagerDataGridColunm()
        {
            if (column != null)
            {
                DependencyPropertyDescriptor textDescr;
                textDescr = DependencyPropertyDescriptor.FromProperty(DataGridColumn.DisplayIndexProperty, typeof(DataGridColumn));
                if (textDescr != null)
                {
                    textDescr.RemoveValueChanged(column, delegate
                    {
                        PropertyChangedNotification("DisplayIndex");
                        this.olddispleyindex = column.DisplayIndex;
                    });
                }
                column = null;
            }
        }

        public ColumnMetaData MetaData
        {
            set
            {
                if (metadata != null | value != null)
                {
                    int? displindex;
                    bool isvible, isfix;
                    DataGridColumn removecolumn;
                    if (value == null)
                    {
                        if (metadata.IsStatic)
                        { ChangeStaticToDinamic(); metadata.IsVisible = true; }
                    }
                    else
                    {
                        if (value.IsStatic)
                        {
                            isvible = this.IsVisible;
                            isfix = this.IsFrozen;
                            displindex = this.DisplayIndex.Value;
                            removecolumn = this.column;
                            foreach (DataGridColumn dgcolumn in mycontrolleddg.Columns)
                            {
                                if (dgcolumn.SortMemberPath == value.ColumnName)
                                {
                                    this.column = dgcolumn;
                                    break;
                                }
                            }
                            if (displindex.HasValue) this.column.DisplayIndex = displindex.Value;
                            InitDisplayIndex();
                            InitHeaderStyle();
                            this.IsFrozen = isfix;
                            this.IsVisible = isvible;
                            value.IsVisible = false;
                            RemoveDataGridColumn(removecolumn, metadata != null && metadata.IsStatic);
                        }
                        else if (this.metadata != null && this.metadata.IsStatic)
                        { ChangeStaticToDinamic(); metadata.IsVisible = true; }

                        aggfunclist.Filter = delegate (object item) { return (item as AggregateFunction).isMemberGroup(value.AggregateFunctionGroup); };
                        prefunclist.Filter = delegate (object item) { return ((item as PreFunction).Type == "comon" | (item as PreFunction).Type == value.PreFunctionFilter); };
                        mychartvaluetypes.Clear();
                        mychartvaluetypes.Add(new KeyValuePair<string, string>("nthng", string.Empty));
                        switch (value.Chart)
                        {
                            case "dimen":
                                mychartvaluetypes.Add(new KeyValuePair<string, string>("dimen", "Набор(X)"));
                                mychartvaluetypes.Add(new KeyValuePair<string, string>("seres", "Ряд"));
                                break;
                            case "measr":
                                mychartvaluetypes.Add(new KeyValuePair<string, string>("measr", "Значение(Y)"));
                                break;
                            case "both2":
                                mychartvaluetypes.Add(new KeyValuePair<string, string>("dimen", "Набор(X)"));
                                mychartvaluetypes.Add(new KeyValuePair<string, string>("measr", "Значение(Y)"));
                                mychartvaluetypes.Add(new KeyValuePair<string, string>("seres", "Ряд"));
                                break;
                            default:
                                break;
                        }
                        (CollectionViewSource.GetDefaultView(mychartvaluetypes) as ListCollectionView).Refresh();
                    }
                    metadata = value;
                    this.Header = value != null ? value.Header : string.Empty;
                    PropertyChangedNotification("MetaData");
                    PropertyChangedNotification("ToolTipText");
                }
            }
            get { return metadata; }
        }

        public int? DisplayIndex
        {
            set
            {
                if (!column.IsFrozen & value < mycontrolleddg.FrozenColumnCount)
                {
                    mycontrolleddg.FrozenColumnCount = mycontrolleddg.FrozenColumnCount + 1;
                    PropertyChangedNotification("IsFrozen");
                }
                else if (column.IsFrozen & !(value < mycontrolleddg.FrozenColumnCount))
                {
                    mycontrolleddg.FrozenColumnCount = mycontrolleddg.FrozenColumnCount - 1;
                    PropertyChangedNotification("IsFrozen");
                }
                column.DisplayIndex = value.Value < managerColumns.Columns.Count ? value.Value > -1 ? value.Value : 0 : managerColumns.Columns.Count - 1;
                PropertyChangedNotification("DisplayIndex");
            }
            get
            {
                if (column == null)
                    return null;
                else
                    return column.DisplayIndex;
            }
        }
        public int OldDisplayIndex { internal set { olddispleyindex = value; } get { return olddispleyindex; } }
        public string ChartValueType
        {
            set
            {
                switch (value)
                {
                    case "nthng":
                        this.mychartnumber = string.Empty;
                        break;
                    case "dimen":
                        this.mychartnumber = "1";
                        foreach (ManagerDataGridColunm item in this.managerColumns.Columns)
                        {
                            if (item.ChartValueType == "dimen") item.ChartValueType = "nthng";
                        }
                        break;

                    case "seres":
                        this.mychartnumber = "1";
                        foreach (ManagerDataGridColunm item in this.managerColumns.Columns)
                        {
                            if (item.ChartValueType == "seres") item.ChartValueType = "nthng";
                        }
                        break;
                    default:
                        this.mychartnumber = "1";
                        break;
                }
                chartvaluetype = value;
                PropertyChangedNotification("ChartValueType");
            }
            get { return chartvaluetype; }
        }
        public string ChartNumber { get { return mychartnumber; } }
        public ListNotifyChanged<KeyValuePair<string, string>> ChartValueTypes { get { return mychartvaluetypes; } }
        public string Header
        {
            get
            {
                if (column.Header == null) return string.Empty;
                return column.Header.GetType() == typeof(String) ? column.Header.ToString() : metadata == null ? string.Empty : metadata.Header;
            }
            set
            {
                if (metadata == null || !metadata.IsStatic)
                {
                    column.Header = value;
                }
                PropertyChangedNotification("Header");
            }
        }
        public bool IsFrozen
        {
            set
            {
                if (column.IsFrozen == value) return;
                if (value)
                {
                    olddispleyindex = column.DisplayIndex;
                    column.DisplayIndex = mycontrolleddg.FrozenColumnCount;
                    mycontrolleddg.FrozenColumnCount = mycontrolleddg.FrozenColumnCount + 1;
                }
                else
                {
                    mycontrolleddg.FrozenColumnCount = mycontrolleddg.FrozenColumnCount - 1;
                    column.DisplayIndex = mycontrolleddg.FrozenColumnCount;
                }
                PropertyChangedNotification("IsFrozen");
            }
            get
            {
                return column.IsFrozen;
            }
        }
        public bool IsVisible
        {
            set
            {
                column.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                PropertyChangedNotification("IsVisible");
            }
            get { return column.Visibility == System.Windows.Visibility.Visible; }
        }
        public bool isSelected
        {
            set { iselected = value; }
            get { return iselected; }
        }
        public ListCollectionView GroupFunctions { get { return aggfunclist; } }
        public string AggregateFunction
        {
            set
            {
                if (!(string.IsNullOrEmpty(value) || value.Equals(aggfunc)))
                {
                    aggfunc = value;
                    PropertyChangedNotification("AggregateFunction");
                    PropertyChangedNotification("ToolTipText");
                }
            }
            get { return aggfunc; }
        }
        public ListCollectionView PreFunctions { get { return prefunclist; } }
        public string PreFunction
        {
            set
            {
                if (!(string.IsNullOrEmpty(value) || value.Equals(prefunc)))
                {
                    prefunc = value;
                    PropertyChangedNotification("PreFunction");
                    PropertyChangedNotification("ToolTipText");
                }
            }
            get { return prefunc; }
        }
        public string ToolTipText
        {
            get
            {

                string aggdesc = string.Empty, predesc = string.Empty;
                if (this.metadata == null) return string.Empty;
                if (this.aggfunc != null && this.aggfunc != "GRP" & this.aggfunc != "VAL")
                {
                    AggregateFunction aggfuncl = (aggfunclist.SourceCollection as List<AggregateFunction>).Find(x => x.Id.Equals(this.aggfunc));
                    aggdesc = ": " + aggfuncl.Name.ToLower();
                }
                if (this.prefunc != null && this.prefunc != "NothingFnc")
                {
                    PreFunction prefuncl = (prefunclist.SourceCollection as List<PreFunction>).Find(x => x.Id.Equals(this.prefunc));
                    predesc = " по " + prefuncl.Name.ToLower();
                }
                return metadata.Header + aggdesc + predesc;
            }
        }
        internal DataGridColumn ManagedColumn { get { return this.column; } }
        internal int SortOrder { set { sortorder = value; } get { return sortorder; } }
        internal ManagerDataGridColumnsVM ManagerColumns { set { managerColumns = value; } }
        internal SQLFilterGroupProperty GroupProperty
        {
            set
            {
                myproperties = value;
                this.aggfunc = value.AggrigateFunction;
                this.prefunc = value.PreFunction;
                this.ChartValueType = value.ChartValueType;
                this.IsVisible = value.IsVisible;
                this.DisplayIndex = value.DisplayIndex;
                this.IsFrozen = value.IsFix;
                this.Header = value.Header;
            }
            get { return myproperties; }
        }
        internal DataGrid ControlledDataGrid
        {
            set
            {
                mycontrolleddg = value;
                mycontrolleddg.Columns.Add(column);
                column.DisplayIndex = managerColumns.Columns.Count - 1;
                InitDisplayIndex();
                InitHeaderStyle();
            }
        }
        internal DinamicPropertyConvertor Converter { set { converter = value; } }
        internal List<AggregateFunction> AllAggregateFunctions
        {
            set
            {
                aggfunclist = new ListCollectionView(value);
            }
        }
        internal PreFunctionList AllPreFunctions
        {
            set
            {
                prefunclist = new ListCollectionView(value);
            }
        }

        void ManagerDataGridColunm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DisplayIndex":
                    PropertyChangedNotification(e.PropertyName);
                    break;
                case "ActualWidth":
                    PropertyChangedNotification("ActualWidth");
                    break;
            }
        }

        internal void PrepareProperties()
        {
            myproperties.DisplayIndex = this.DisplayIndex.Value;
            myproperties.IsFix = this.IsFrozen;
            myproperties.BasisId = this.metadata.Id;
            myproperties.IsVisible = this.IsVisible;
            myproperties.Header = this.Header;
            myproperties.AggrigateFunction = this.aggfunc;
            myproperties.PreFunction = this.prefunc;
            myproperties.ChartValueType = this.ChartValueType;
        }
        internal void PrepareColumnFill() /*настроука столбца DataGrid */
        {
            if (!this.metadata.IsStatic)
            {
                string format;
                Style style;
                Binding binding = new Binding("[" + myproperties.SelectName + "].Value");
                DataGridTextColumn textcolumn = this.column as DataGridTextColumn;
                if (this.aggfunc == "CNT") format = "00ncr";
                else if (this.prefunc == "vdate") format = "strcr";
                else format = this.metadata.Datatype;
                switch (format)
                {
                    case "02nrt":
                        binding.StringFormat = "{0:N}";
                        style = mycontrolleddg.FindResource("StyleTextBlockAlignmentRight") as Style;
                        break;
                    case "00ncr":
                        binding.StringFormat = "{0:N0}";
                        style = mycontrolleddg.FindResource("StyleTextBlockAlignmentCenter") as Style;
                        break;
                    case "00nrt":
                        binding.StringFormat = "{0:N0}";
                        style = mycontrolleddg.FindResource("StyleTextBlockAlignmentRight") as Style;
                        break;
                    case "00fcr":
                        binding.StringFormat = "{0:F0}";
                        style = mycontrolleddg.FindResource("StyleTextBlockAlignmentCenter") as Style;
                        break;
                    case "dmycr":
                        binding.StringFormat = "{0:d}";
                        style = mycontrolleddg.FindResource("StyleTextBlockAlignmentCenter") as Style;
                        break;
                    case "strcr":
                        style = mycontrolleddg.FindResource("StyleTextBlockAlignmentCenter") as Style;
                        break;
                    default:
                        style = null;
                        break;
                }
                if (style != null)
                {
                    //if (textcolumn.ElementStyle != null)
                    //{
                    //    style.BasedOn = textcolumn.ElementStyle;
                    //}
                    textcolumn.ElementStyle = style;
                }
                binding.Mode = BindingMode.OneWay;
                //binding.Converter = converter;
                //binding.ConverterParameter = properties.SelectName;
                (column as DataGridTextColumn).Binding = binding;
                column.SortMemberPath = myproperties.SortName;
            }
            if (myproperties.SortOrder == 0)
                column.SortDirection = null;
            else
                column.SortDirection = myproperties.SortOrder > 0 ? System.ComponentModel.ListSortDirection.Ascending : System.ComponentModel.ListSortDirection.Descending;
        }
        internal void UnmakeColumn()
        {
            if (!this.metadata.IsStatic)
            {
                column.SortMemberPath = string.Empty;
                (column as DataGridTextColumn).Binding = null;
            }
        }
        internal void DeleteColumn()
        {
            RemoveDataGridColumn(this.column, metadata != null && metadata.IsStatic);
            myproperties.BasisId = -1;
            this.aggfunclist.Filter = null;
            this.prefunclist.Filter = null;
            this.prefunclist = null;
            this.aggfunclist = null;
            this.mychartvaluetypes = null;
            this.mycontrolleddg = null;
            this.metadata = null;
            //this.column = null;
        }

        private void CreateDataGridColumn()
        {
            column = new DataGridTextColumn();
            column.IsReadOnly = true;
            column.Header = "Новый";
        }
        private void InitDisplayIndex()
        {
            this.olddispleyindex = this.DisplayIndex.Value;
            DependencyPropertyDescriptor textDescr;
            textDescr = DependencyPropertyDescriptor.FromProperty(DataGridColumn.DisplayIndexProperty, typeof(DataGridColumn));
            if (textDescr != null)
            {
                textDescr.AddValueChanged(column, delegate
                {
                    PropertyChangedNotification("DisplayIndex");
                    this.olddispleyindex = column.DisplayIndex;
                });
            }
            textDescr = DependencyPropertyDescriptor.FromProperty(DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));
            if (textDescr != null)
            {
                textDescr.AddValueChanged(column, delegate
                {
                    PropertyChangedNotification("ActualWidth");
                });
            }
        }
        private void InitHeaderStyle()//после контекстного меню на уровле DataGrid
        {
            Binding binding = new Binding("ToolTipText");
            binding.Source = this;
            binding.Mode = BindingMode.OneWay;

            Style style = new Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));
            if (column.HeaderStyle != null)
                style.BasedOn = column.HeaderStyle;
            style.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.ToolTipProperty, binding));
            style.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.ContextMenuProperty, CreateColumnContextMenu()));
            column.HeaderStyle = style;
        }
        private void RemoveDataGridColumn(DataGridColumn column, bool isStatic)
        {
            DependencyPropertyDescriptor textDescr;
            textDescr = DependencyPropertyDescriptor.FromProperty(DataGridColumn.DisplayIndexProperty, typeof(DataGridColumn));
            if (textDescr != null)
            {
                textDescr.RemoveValueChanged(column, delegate
                {
                    PropertyChangedNotification("DisplayIndex");
                    this.olddispleyindex = column.DisplayIndex;
                });
            }
            if (isStatic)
            {
                this.metadata.IsVisible = true;
                Style style = new Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));
                if (column.HeaderStyle != null)
                    style.BasedOn = column.HeaderStyle;
                style.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.ToolTipProperty, null));
                style.Setters.Add(new Setter(System.Windows.Controls.Primitives.DataGridColumnHeader.ContextMenuProperty, null));
                column.HeaderStyle = style;
                column.Visibility = Visibility.Collapsed;
                column.DisplayIndex = mycontrolleddg.Columns.Count - 1;
            }
            else this.mycontrolleddg.Columns.Remove(column);
        }
        private void ChangeStaticToDinamic()
        {
            int? displindex;
            bool isvible, isfix;

            isvible = this.IsVisible;
            isfix = this.IsFrozen;
            displindex = this.DisplayIndex.Value;
            RemoveDataGridColumn(this.column, true);
            CreateDataGridColumn();
            mycontrolleddg.Columns.Add(this.column);
            if (displindex.HasValue) this.column.DisplayIndex = displindex.Value;
            InitDisplayIndex();
            this.IsFrozen = isfix;
            this.IsVisible = IsVisible;
        }
        private ContextMenu CreateColumnContextMenu()
        {
            double height = 26D;
            ContextMenu menu = new ContextMenu();

            MenuItem hide = new MenuItem();
            hide.Command = this.HideColumn;
            Image imhide = new Image();
            imhide.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"Images/table.png", UriKind.Relative));
            imhide.Stretch = System.Windows.Media.Stretch.Uniform;
            hide.Icon = imhide;
            hide.Header = "Скрыть";
            hide.Height = height;
            menu.Items.Add(hide);

            MenuItem fix = new MenuItem();
            fix.IsCheckable = true;
            Binding fixbind = new Binding("IsFrozen");
            fixbind.Source = this;
            fixbind.Mode = BindingMode.TwoWay;
            fix.SetBinding(MenuItem.IsCheckedProperty, fixbind);
            Image fixim = new Image();
            fixim.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"Images/column_fix.png", UriKind.Relative));
            fixim.Stretch = System.Windows.Media.Stretch.Uniform;
            fix.Icon = fixim;
            fix.Header = "Закреплен";
            fix.Height = height;
            menu.Items.Add(fix);

            //Separator spr = new Separator();
            //spr.Margin = new Thickness(0D);
            //spr.Padding = new Thickness(0D);
            //spr.Height = 18D;
            //menu.Items.Add(spr);

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Vertical;
            panel.HorizontalAlignment = HorizontalAlignment.Center;

            TextBlock bl = new TextBlock();
            bl.Text = "Столбец";
            bl.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Children.Add(bl);
            ComboBox cmb = new ComboBox();
            cmb.ItemsSource = this.managerColumns.ColumnsMetadata;
            cmb.DisplayMemberPath = "Header";
            Binding cmbbind = new Binding("MetaData");
            cmbbind.Source = this;
            cmbbind.Mode = BindingMode.TwoWay;
            cmbbind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            cmbbind.ValidatesOnDataErrors = true;
            cmb.SetBinding(ComboBox.SelectedItemProperty, cmbbind);
            Binding vsbl = new Binding("IsVisible");
            Binding enbl = new Binding("IsVisible");
            vsbl.Mode = BindingMode.OneWay;
            enbl.Mode = BindingMode.OneWay;
            vsbl.Converter = this.managerColumns.ItemVisibilityConverter;
            Style visblitem = new Style(typeof(ComboBoxItem));
            visblitem.Setters.Add(new Setter(ComboBoxItem.VisibilityProperty, vsbl));
            visblitem.Setters.Add(new Setter(ComboBoxItem.IsEnabledProperty, enbl));
            cmb.ItemContainerStyle = visblitem;
            panel.Children.Add(cmb);

            TextBlock bl2 = new TextBlock();
            bl2.Text = "Преобразование";
            bl2.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Children.Add(bl2);
            ComboBox cmbpre = new ComboBox();
            cmbpre.ItemsSource = this.PreFunctions;
            cmbpre.SelectedValuePath = "Id";
            cmbpre.DisplayMemberPath = "Name";
            cmbpre.ToolTip = "Преобразовать значение в столбце в";
            Binding prebind = new Binding("PreFunction");
            prebind.Source = this;
            prebind.Mode = BindingMode.TwoWay;
            prebind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            cmbpre.SetBinding(ComboBox.SelectedValueProperty, prebind);
            panel.Children.Add(cmbpre);

            TextBlock bl3 = new TextBlock();
            bl3.Text = "Объединение";
            bl3.HorizontalAlignment = HorizontalAlignment.Center;
            panel.Children.Add(bl3);
            ComboBox cmbgrp = new ComboBox();
            cmbgrp.ItemsSource = this.GroupFunctions;
            cmbgrp.SelectedValuePath = "Id";
            cmbgrp.DisplayMemberPath = "Name";
            cmbgrp.ToolTip = "Объединение";
            Binding grpbind = new Binding("AggregateFunction");
            grpbind.Source = this;
            grpbind.Mode = BindingMode.TwoWay;
            grpbind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            cmbgrp.SetBinding(ComboBox.SelectedValueProperty, grpbind);
            panel.Children.Add(cmbgrp);

            menu.Items.Add(panel);

            StackPanel panel4 = new StackPanel();
            panel4.Orientation = Orientation.Vertical;
            panel4.HorizontalAlignment = HorizontalAlignment.Left;
            TextBlock bl4 = new TextBlock();
            bl4.Text = "Заголовок";
            bl4.HorizontalAlignment = HorizontalAlignment.Center;
            panel4.Children.Add(bl4);
            TextBox headerbox = new TextBox();
            Binding headerbind = new Binding("Header");
            headerbind.Source = this;
            headerbind.Mode = BindingMode.TwoWay;
            headerbind.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            headerbox.SetBinding(TextBox.TextProperty, headerbind);
            panel4.Children.Add(headerbox);
            menu.Items.Add(panel4);

            menu.Items.Add(new Separator());
            StackPanel panel5 = new StackPanel();
            panel5.Orientation = Orientation.Vertical;
            panel5.HorizontalAlignment = HorizontalAlignment.Left;
            TextBlock charttb = new TextBlock();
            charttb.Text = "Диаграмма";
            charttb.ToolTip = "Тип данных для диаграммы";
            panel5.Children.Add(charttb);
            ComboBox cmbchart = new ComboBox();
            cmbchart.ItemsSource = this.ChartValueTypes;
            cmbchart.SelectedValuePath = "Key";
            cmbchart.DisplayMemberPath = "Value";
            cmbchart.ToolTip = "Тип данных для диаграммы";
            Binding chartbind = new Binding("ChartValueType");
            chartbind.Source = this;
            chartbind.Mode = BindingMode.TwoWay;
            chartbind.UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            cmbchart.SetBinding(ComboBox.SelectedValueProperty, chartbind);
            panel5.Children.Add(cmbchart);
            menu.Items.Add(panel5);

            menu.Items.Add(new Separator());
            MenuItem del = new MenuItem();
            del.Command = this.Delete;
            Image imdel = new Image();
            imdel.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"Images/column_delete.png", UriKind.Relative));
            imdel.Stretch = System.Windows.Media.Stretch.Uniform;
            del.Icon = imdel;
            del.Header = "Удалить";
            del.Height = height;
            menu.Items.Add(del);

            return menu;
        }

        private RelayCommand hide;
        public ICommand HideColumn
        {
            get
            {
                if (hide == null) hide = new RelayCommand(this.HideColumnExec, this.HideColumnCanExec);
                return hide;
            }
        }
        private void HideColumnExec(object parametr)
        {
            this.IsVisible = false;
        }
        private bool HideColumnCanExec(object parametr)
        {
            return this.Error.Length == 0;
        }

        private RelayCommand fix;
        public ICommand FixColumn
        {
            get
            {
                if (fix == null) fix = new RelayCommand(this.FixColumnExec, this.FixColumnCanExec);
                return fix;
            }
        }
        private void FixColumnExec(object parametr)
        {
            this.IsFrozen = !this.IsFrozen;
        }
        private bool FixColumnCanExec(object parametr)
        {
            return true;
        }

        private RelayCommand del;
        public ICommand Delete
        {
            get
            {
                if (del == null) del = new RelayCommand(this.DeleteExec, this.DeleteCanExec);
                return del;
            }
        }
        private void DeleteExec(object parametr)
        {
            this.managerColumns.Columns.Remove(this);
        }
        private bool DeleteCanExec(object parametr)
        {
            return true;
        }
    }

    internal class VisibleAllColumns : ICommand
    {
        ObservableCollection<ManagerDataGridColunm> columns;
        public event EventHandler CanExecuteChanged;

        internal VisibleAllColumns(ObservableCollection<ManagerDataGridColunm> columns)
        {
            this.columns = columns;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            foreach (ManagerDataGridColunm column in columns) column.IsVisible = true;
        }
    }

    public class PreFunctionList : List<PreFunction>
    {
        internal PreFunctionList()
            : base()
        {
            Fill();
        }
        private void Fill()
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT id,funcname,functype FROM dbo.FilterGroupPreFunction_vw ORDER BY sort";
                    cmd.Connection = conn;
                    conn.Open();
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
                    while (reader.Read())
                    {
                        this.Add(new PreFunction(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                    }
                }
                finally { if (reader != null) reader.Close(); }
            }
        }
    }

    public class ColumnMetaData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        public int Id { set; get; }
        internal int Level { set; get; }
        public string ColumnName { set; get; }
        public string Header { set; get; }
        internal Int16 AggregateFunctionGroup { set; get; }
        internal string PreFunctionFilter { set; get; }
        internal string Datatype { set; get; }
        internal string Chart { set; get; }
        internal bool IsStatic { set; get; }
        private bool isvisible;
        public bool IsVisible
        {
            set
            {
                isvisible = value;
                PropertyChangedNotification("IsVisible");
            }
            get { return isvisible; }
        }

        internal ColumnMetaData() { isvisible = true; }
        internal ColumnMetaData(int id, int level, string columnname, string header, Int16 aggregategroup, string prefilter, string datatype, bool isstatic)
            : this()
        {
            this.Id = id;
            this.Level = level;
            this.ColumnName = columnname;
            this.Header = header;
            this.AggregateFunctionGroup = aggregategroup;
            this.PreFunctionFilter = prefilter;
            this.Datatype = datatype;
            this.IsStatic = isstatic;
        }
    }
    public class ColumnMetadataList : List<ColumnMetaData>
    {
        internal ColumnMetadataList(string groupClass) : base() { Fill(groupClass); }
        private void Fill(string groupClass)
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.FilterGroupColumnDefinition_sp";
                    cmd.Parameters.Add(new SqlParameter("@class", groupClass));
                    cmd.Connection = conn;
                    conn.Open();
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
                    while (reader.Read())
                    {
                        ColumnMetaData item = new ColumnMetaData();
                        for (int c = 0; c < reader.FieldCount; c++)
                        {
                            switch (reader.GetName(c))
                            {
                                case "id":
                                    if (!reader.IsDBNull(c)) item.Id = reader.GetInt32(c);
                                    break;
                                case "level":
                                    if (!reader.IsDBNull(c)) item.Level = reader.GetInt32(c);
                                    break;
                                case "aggfuncgroup":
                                    if (!reader.IsDBNull(c)) item.AggregateFunctionGroup = reader.GetByte(c);
                                    break;
                                case "prefuncfilter":
                                    if (!reader.IsDBNull(c)) item.PreFunctionFilter = reader.GetString(c);
                                    break;
                                case "header":
                                    if (!reader.IsDBNull(c)) item.Header = reader.GetString(c);
                                    break;
                                case "isstatic":
                                    if (!reader.IsDBNull(c)) item.IsStatic = reader.GetBoolean(c);
                                    break;
                                case "selectName":
                                    if (!reader.IsDBNull(c)) item.ColumnName = reader.GetString(c);
                                    break;
                                case "datatype":
                                    if (!reader.IsDBNull(c)) item.Datatype = reader.GetString(c);
                                    break;
                                case "chart":
                                    if (!reader.IsDBNull(c)) item.Chart = reader.GetString(c);
                                    break;
                            }
                        }
                        this.Add(item);
                    }
                    this.Sort(delegate (ColumnMetaData x, ColumnMetaData y) { return x.Header.CompareTo(y.Header); });
                }
                catch (Exception ex)
                {
                    if (ex is System.Data.SqlClient.SqlException)
                    {
                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                        if (err.Number > 49999) MessageBox.Show(err.Message, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                finally { if (reader != null) reader.Close(); }
            }
            //(CollectionViewSource.GetDefaultView(this) as ListCollectionView).Filter(new Predicate<ColumnMetaData>(x=>x.IsVisible));
        }
        public ColumnMetaData FindFirstItem(int id)
        {
            return this.Find(x => x.Id.Equals(id));
        }
    }
}
