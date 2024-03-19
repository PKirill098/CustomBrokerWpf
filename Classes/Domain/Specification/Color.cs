using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class Color:lib.DomainBaseStamp
    {
        public Color(int id,long stamp,lib.DomainObjectState state
            ,string brand,string colorcode,string colorname,string producer 
            ):base(id,stamp,null,null,state)
        {
            mybrand = brand;
            mycolorcode = colorcode;
            mycolorname = colorname;
            myproducer = producer;
        }
        public Color():this(lib.NewObjectId.NewId,0L,lib.DomainObjectState.Added
            ,string.Empty, string.Empty, string.Empty, string.Empty)
        { }

        private string mybrand;
        public string Brand
        { set { this.SetProperty(ref mybrand, value); } get { return mybrand; } }
        private string mycolorcode;
        public string ColorCode
        { set { this.SetProperty(ref mycolorcode, value); } get { return mycolorcode; } }
        private string mycolorname;
        public string ColorName
        { set { this.SetProperty<string>(ref mycolorname, value); } get { return mycolorname; } }
        private string myproducer;
        public string Producer
        { set { this.SetProperty(ref myproducer,value); } get { return myproducer; } }

        protected override void RejectProperty(string property, object value)
        {
        }

        protected override void PropertiesUpdate(DomainBaseUpdate sample)
        {
            Color temple = (Color)sample; 
            this.Brand= temple.Brand;
            this.ColorCode= temple.ColorCode;
            this.ColorName= temple.ColorName;
            this.Producer = temple.Producer;
        }
    }

    internal class ColorDBM:lib.DBManagerStamp<Color,Color>
    {
        public ColorDBM()
        {
            this.NeedAddConnection = false;
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "spec.Color_sp";
            InsertCommandText = "spec.ColorAdd_sp";
            UpdateCommandText = "spec.ColorUpd_sp";
            DeleteCommandText = "spec.ColorDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int), new SqlParameter("@filter", System.Data.SqlDbType.Int), new SqlParameter("@brand", System.Data.SqlDbType.NVarChar, 200), new SqlParameter("@colorcode", System.Data.SqlDbType.NVarChar, 100), new SqlParameter("@producer", System.Data.SqlDbType.NVarChar, 100) };
            InsertUpdateParams = new SqlParameter[] { new SqlParameter("@brand", System.Data.SqlDbType.NVarChar, 200), new SqlParameter("@colorcode", System.Data.SqlDbType.NVarChar, 100), new SqlParameter("@colorname", System.Data.SqlDbType.NVarChar, 100), new SqlParameter("@producer", System.Data.SqlDbType.NVarChar, 100) };
            UpdateParams = new SqlParameter[] { UpdateParams[0], new SqlParameter("@brandupd", System.Data.SqlDbType.Bit), new SqlParameter("@colorcodeupd", System.Data.SqlDbType.Bit), new SqlParameter("@colornameupd", System.Data.SqlDbType.Bit), new SqlParameter("@producerupd", System.Data.SqlDbType.Bit) };
        }

        internal lib.SQLFilter.SQLFilter Filter { set; get; }
        internal string Brand {  set; get; }
        internal string ColorCode { set; get; }
        internal string Producer { set; get;}

        protected override bool SetParametersValue(Color item)
        {
            base.SetParametersValue(item);
            foreach(SqlParameter par in UpdateParams)
                switch(par.ParameterName)
                {
                    case "@brandupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Color.Brand)); break;
                    case "@colorcodeupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Color.ColorCode)); break;
                    case "@colornameupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Color.ColorName)); break;
                    case "@producerupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Color.Producer)); break;
                }
            foreach(SqlParameter par in InsertUpdateParams)
                switch(par.ParameterName)
                {
                    case "@brand":
                        par.Value=item.Brand; break;
                    case "@colorcode":
                        par.Value=item.ColorCode; break;
                    case "@colorname":
                        par.Value=item.ColorName; break;
                    case "@producer":
                        par.Value=item.Producer; break;
                }
            return true;
        }

        protected override void SetSelectParametersValue()
        {
            foreach(SqlParameter par in this.SelectParams)
                switch(par.ParameterName)
                {
                    case "@filter":
                        par.Value=this.Filter?.FilterWhereId; break;
                    case "@brand":
                        par.Value=this.Brand; break;
                    case "@colorcode":
                        par.Value=this.ColorCode; break;
                    case "@producer":
                        par.Value=this.Producer; break;
                }
        }

        protected override Color CreateRecord(SqlDataReader reader)
        {
            return new Color(
                reader.GetInt32(this.Fields["id"]),reader.GetInt64(this.Fields["stamp"]),lib.DomainObjectState.Unchanged
                , reader.IsDBNull(this.Fields["brand"]) ? string.Empty : reader.GetString(this.Fields["brand"])
                ,reader.GetString(this.Fields["colorcode"])
                ,reader.GetString(this.Fields["colorname"])
                , reader.IsDBNull(this.Fields["producer"]) ? string.Empty : reader.GetString(this.Fields["producer"])
                );
        }
        protected override Color CreateModel(Color record, SqlConnection addcon, CancellationToken canceltasktoken = default)
        {
            return record;
        }
    }

    public class ColorVM:lib.ViewModelErrorNotifyItem<Color>
    {
        public ColorVM(Color model):base(model)
        {
            DeleteRefreshProperties.AddRange(new string[] {nameof(ColorVM.Brand),nameof(ColorVM.ColorCode),nameof(ColorVM.ColorName),nameof(ColorVM.Producer) });
            InitProperties();
        }
        public ColorVM():this(new Color()) { }
        
        public string Brand
        { 
            set { this.SetProperty<string>(this.DomainObject.Brand, (string parametr) => { this.DomainObject.Brand = parametr; }, value); }
            get {return GetProperty<string>(this.DomainObject.Brand, string.Empty); }
        }
        public string ColorCode
        { 
            set { this.SetProperty<string>(this.DomainObject.ColorCode, (string parametr) => { this.DomainObject.ColorCode = parametr; }, value); }
            get {return GetProperty<string>(this.DomainObject.ColorCode, string.Empty); }
        }
        public string ColorName
        { 
            set { this.SetProperty<string>(this.DomainObject.ColorName, (string parametr) => { this.DomainObject.ColorName = parametr; }, value); }
            get {return GetProperty<string>(this.DomainObject.ColorName, string.Empty); }
        }
        public string Producer
        { 
            set { this.SetProperty<string>(this.DomainObject.Producer, (string parametr) => { this.DomainObject.Producer = parametr; }, value); }
            get {return GetProperty<string>(this.DomainObject.Producer, string.Empty); }
        }
        
        protected override void DomainObjectPropertyChanged(string property)
        {
        }
        protected override bool DirtyCheckProperty()
        {
            return false;
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(ColorVM.Brand):
                    this.DomainObject.Brand = (string)value;
                    break;
                case nameof(ColorVM.ColorCode):
                    this.DomainObject.ColorCode = (string)value;
                    break;
                case nameof(ColorVM.ColorName):
                    this.DomainObject.ColorName = (string)value;
                    break;
                case nameof(ColorVM.Producer):
                    this.DomainObject.Producer = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            return true;
        }
        protected override void InitProperties()
        {
        }
    }

    public class ColorSynchronizer : lib.ModelViewCollectionsSynchronizer<Color, ColorVM>
    {
        protected override Color UnWrap(ColorVM wrap)
        {
            return wrap.DomainObject as Color;
        }
        protected override ColorVM Wrap(Color fill)
        {
            return new ColorVM(fill);
        }
    }

    public class ColorViewCommand : lib.ViewModelViewCommand
    {
        public ColorViewCommand()
        { 
            myfilter = new lib.SQLFilter.SQLFilter("speccolor", "AND", CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            mycdbm = new ColorDBM();
            mydbm = mycdbm;
            mycdbm.Collection = new ObservableCollection<Color>();
            mycdbm.Filter = myfilter;
            mycdbm.FillAsyncCompleted = () =>
			{
				if (mycdbm.Errors.Count > 0) OpenPopup(mycdbm.ErrorMessage, true);
			};
            mysync = new ColorSynchronizer();
            mysync.DomainCollection = mycdbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            #region Filters
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            mybrandfilter = new ColorBrandFilter();
            mybrandfilter.DeferredFill = true;
            mybrandfilter.ItemsSource = myview.OfType<ColorVM>();
            mybrandfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mybrandfilter.ExecCommand2 = () => { mybrandfilter.Clear(); };
            mybrandfilter.FillDefault = () =>
            {
                if (myfilter.isEmpty)
                    foreach (string item in mybrandfilter.DefaultList)
                        mybrandfilter.Items.Add(item);
                return myfilter.isEmpty;
            };
            myproducerfilter = new ColorProducerFilter();
            myproducerfilter.DeferredFill = true;
            myproducerfilter.ItemsSource = myview.OfType<ColorVM>();
            myproducerfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myproducerfilter.ExecCommand2 = () => { myproducerfilter.Clear(); };
            myproducerfilter.FillDefault = () =>
            {
                if (myfilter.isEmpty)
                    foreach (string item in myproducerfilter.DefaultList)
                        myproducerfilter.Items.Add(item);
                return myfilter.isEmpty;
            };
            #endregion
            if (myfilter.isEmpty)
                this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
        }

        private ColorDBM mycdbm;
        private ColorSynchronizer mysync;
        #region Filters
        private lib.SQLFilter.SQLFilter myfilter;
        public lib.SQLFilter.SQLFilter Filter
        { get { return myfilter; } }
        private ColorBrandFilter mybrandfilter;
        public ColorBrandFilter BrandFilter { get { return mybrandfilter; } }
        private ColorProducerFilter myproducerfilter;
        public ColorProducerFilter ProducerFilter { get { return myproducerfilter; } }
        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            this.EndEdit();
            if (mybrandfilter.FilterOn)
            {
                string[] items = new string[mybrandfilter.SelectedItems.Count];
                for (int i = 0; i < mybrandfilter.SelectedItems.Count; i++)
                    items[i] = (string)mybrandfilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "brand", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "brand", new string[0]);
            if (myproducerfilter.FilterOn)
            {
                string[] items = new string[myproducerfilter.SelectedItems.Count];
                for (int i = 0; i < myproducerfilter.SelectedItems.Count; i++)
                    items[i] = (string)myproducerfilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "producer", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "producer", new string[0]);
            if (!(mybrandfilter.FilterOn | myproducerfilter.FilterOn))
                this.OpenPopup("Фильтр. Пожалуйста, задайте критерии выбора!", false);
            else
                this.RefreshData(null);
        }
        private bool FilterRunCanExec(object parametr)
        { return true; }
        private RelayCommand myfilterclear;
        public ICommand FilterClear
        {
            get { return myfilterclear; }
        }
        private void FilterClearExec(object parametr)
        {
            mybrandfilter.Clear();
            mybrandfilter.IconVisibileChangedNotification();
            myproducerfilter.Clear();
            myproducerfilter.IconVisibileChangedNotification();
            this.FilterRunExec(null);
            this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }
        #endregion
        protected override void OtherViewRefresh()
        {
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Producer",System.ComponentModel.ListSortDirection.Ascending));
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Brand",System.ComponentModel.ListSortDirection.Ascending));
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("ColorCode",System.ComponentModel.ListSortDirection.Ascending));
        }
        protected override void RefreshData(object parametr)
        {
            //if (myfilter.isEmpty)
            //    this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
            //else
            //{  }
            mycdbm.Errors.Clear(); mycdbm.FillAsync();
        }
    
        public void Dispose()
        {
            myfilter.RemoveFilter();
        }
    }

    public class ColorBrandFilter : libui.CheckListBoxVMFillDefault<ColorVM, string>
    {
        private List<string> mydefaultlist;
        internal List<string> DefaultList
        {
            get
            {
                if (mydefaultlist == null)
                {
                    string[] names;
                    bool contains = false;
                    mydefaultlist = new List<string>();
                    Domain.GoodsDBM pdbm = new Domain.GoodsDBM();
                    pdbm.Fill();
                    foreach (Domain.Goods goods in pdbm.Collection)
                    {
                        names = goods.Brand.Trim(new char[] { ' ', ',' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string name in names)
                        {
                            contains = false;
                            foreach (string contry in mydefaultlist)
                                if (string.Equals(contry, name, StringComparison.CurrentCultureIgnoreCase))
                                { contains = true; break; }
                            if (!contains) mydefaultlist.Add(name);
                        }
                    }
                }
                return mydefaultlist;
            }
        }

        protected override void AddItem(ColorVM item)
        {
            if (!this.Items.Contains(item.Brand)) this.Items.Add(item.Brand);
        }
    }
    public class ColorProducerFilter : libui.CheckListBoxVMFillDefault<ColorVM, string>
    {
        private List<string> mydefaultlist;
        internal List<string> DefaultList
        {
            get
            {
                if (mydefaultlist == null)
                {
                    mydefaultlist = new List<string>();
                    Domain.GoodsDBM pdbm = new Domain.GoodsDBM();
                    pdbm.Fill();
                    foreach (Domain.Goods goods in pdbm.Collection)
                        if (!mydefaultlist.Contains(goods.Producer))
                            mydefaultlist.Add(goods.Producer);
                }
                return mydefaultlist;
            }
        }

        protected override void AddItem(ColorVM item)
        {
            if (!Items.Contains(item.Producer)) Items.Add(item.Producer);
        }
    }
}
