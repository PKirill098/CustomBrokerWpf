using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.References;
using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    internal struct BrandRecord
    {
        internal int Id;
        internal Int64 Stamp;
        internal DateTime? Updated;
        internal String Updater;
        internal int? Homeland;
        internal string Name;
        internal string Producer;
        internal int? SizePlus;
    }
    public class Brand : lib.DomainBaseStamp
    {
        public Brand(int id, Int64 stamp, DateTime? updeted, String updater, lib.DomainObjectState state
            , Country homeland, string name, string producer, int? sizeplus
            ) : base(id, stamp, updeted, updater, state)
        {
            myhomeland = homeland;
            myname = name;
            myproducer = producer;
            mysizeplus = sizeplus;
        }
        public Brand() : this(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, null, null, null, null) { }

        private Country myhomeland;
        public Country Homeland
        { set { SetProperty(ref myhomeland, value); } get { return myhomeland; } }
        private string myname;
        public string Name
        { set { SetProperty<string>(ref myname, value); } get { return myname; } }
        private string myproducer;
        public string Producer
        { set { SetProperty(ref myproducer, value); } get { return myproducer; } }
        private int? mysizeplus;
        public int? SizePlus
        { set { SetProperty(ref mysizeplus, value); } get { return mysizeplus; } }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Name):
                    myname = (string)this.GetPropertyOutdatedValue(nameof(this.Name));
                    break;
            }
        }
        protected override void PropertiesUpdate(DomainBaseUpdate sample)
        {
            Brand templ = (Brand)sample;
            this.Homeland = templ.Homeland;
            this.Name = templ.Name;
            this.Producer = templ.Producer;
            this.SizePlus = templ.SizePlus;
        }
        public override bool ValidateProperty(string propertyname, object value, out string errmsg, out byte errmsgkey)
        {
            errmsg = string.Empty;
            errmsgkey = 0;
            bool result = true;
            switch (propertyname)
            {
                case nameof(Brand.Name):
                    if (string.IsNullOrEmpty((string)value))
                    {
                        errmsg = "Наименование не может быть пустым!";
                        result = false;
                    }
                    break;
            }
            return result;
        }
    }

    internal class BrandDBM : lib.DBManagerId<BrandRecord, Brand>
    {
        public BrandDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectCommandText = "Brand_sp";
            InsertCommandText = "BrandAdd_sp";
            UpdateCommandText = "BrandUpd_sp";
            DeleteCommandText = "BrandDel_sp";

            SelectParams = new SqlParameter[] { SelectParams[0], new SqlParameter("@filter", System.Data.SqlDbType.Int) };
            InsertParams = new SqlParameter[] { InsertParams[0] };
            InsertUpdateParams = new SqlParameter[] {
                new SqlParameter("@homeland", System.Data.SqlDbType.Int),
                new SqlParameter("@name",System.Data.SqlDbType.NVarChar,100),
                new SqlParameter("@producer", System.Data.SqlDbType.NVarChar, 100),
                new SqlParameter("@size", System.Data.SqlDbType.Int)
            };
            UpdateParams = new SqlParameter[]
            {
                UpdateParams[0],
                new SqlParameter("@homelandupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@nameupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@producerupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@sizeupd", System.Data.SqlDbType.Bit)
            };
        }

        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { set { myfilter = value; } get { return myfilter; } }

        protected override BrandRecord CreateRecord(SqlDataReader reader)
        {
            return new BrandRecord()
            {
                Id = reader.GetInt32(this.Fields["id"]),
                Stamp = reader.GetInt64(this.Fields["stamp"]),
                Updated = reader.GetDateTime(this.Fields["updated"]),
                Updater = reader.GetString(this.Fields["updater"]),
                Homeland = reader.IsDBNull(this.Fields["homeland"]) ? (int?)null : reader.GetInt32(this.Fields["homeland"]),
                Name = reader.GetString(this.Fields["brandName"]),
                Producer = reader.IsDBNull(this.Fields["producer"]) ? null : reader.GetString(this.Fields["producer"]),
                SizePlus = reader.IsDBNull(this.Fields["size+"]) ? (int?)null : reader.GetInt32(this.Fields["size+"])
            };
        }
        protected override Brand CreateModel(BrandRecord record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
        {
            Brand brand = null;
            if (this.FillType == FillType.PrefExist)
                brand = CustomBrokerWpf.References.BrandStorage.GetItem(record.Id);
            if (brand == null)
                brand = CustomBrokerWpf.References.BrandStorage.UpdateItem(
                    new Brand(record.Id, record.Stamp, record.Updated, record.Updater, lib.DomainObjectState.Unchanged
                        , CustomBrokerWpf.References.Countries.FindFirstItem("Code", record.Homeland), record.Name, record.Producer, record.SizePlus
                ));
            return brand;
        }
        protected override bool SetParametersValue(Brand item)
        {
            base.SetParametersValue(item);
            foreach (SqlParameter par in this.InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@homeland":
                        par.Value = item.Homeland?.Code;
                        break;
                    case "@name":
                        par.Value = item.Name;
                        break;
                    case "@producer":
                        par.Value = item.Producer;
                        break;
                    case "@size":
                        par.Value = item.SizePlus;
                        break;
                }
            foreach (SqlParameter par in this.SelectParams)
                switch (par.ParameterName)
                {
                    case "@homelandupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Brand.Homeland));
                        break;
                    case "@nameupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Brand.Name));
                        break;
                    case "@producerupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Brand.Producer));
                        break;
                    case "@sizeupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Brand.SizePlus));
                        break;
                }
            return true;
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            foreach (SqlParameter par in this.SelectParams)
                switch (par.ParameterName)
                {
                    case "@filter":
                        par.Value = myfilter?.FilterWhereId;
                        break;
                }
        }
    }

    internal class BrandStorage : lib.DomainStorageLoad<BrandRecord, Brand, BrandDBM>
    {
        public BrandStorage() : base(new BrandDBM()) { }
    }

    public class BrandVM : lib.ViewModelErrorNotifyItem<Brand>
    {
        public BrandVM(Brand brand) : base(brand)
        {
            this.ValidetingProperties.AddRange(new string[] { nameof(BrandVM.Name) });
            this.DeleteRefreshProperties.AddRange(new string[] { nameof(BrandVM.Homeland), nameof(BrandVM.Name), nameof(BrandVM.Producer), nameof(BrandVM.SizePlus) });
            InitProperties();

            mycountries = new ListCollectionView(CustomBrokerWpf.References.Countries);
        }
        public BrandVM() : this(new Brand()) { }

        public Country Homeland
        { set { SetProperty(this.DomainObject.Homeland, (Country country) => { this.DomainObject.Homeland = country; }, value); } get { return GetProperty(this.DomainObject.Homeland, null); } }
        private string myname;
        public string Name
        { set { SetPropertyValidateNotNull(ref myname, () => { this.DomainObject.Name = value; }, value); } get { return GetProperty(myname, null); } }
        public string Producer
        { set { SetProperty(this.DomainObject.Producer, (String pr) => { this.DomainObject.Producer = pr; }, value); } get { return GetProperty(this.DomainObject.Producer, null); } }
        public int? SizePlus
        { set { SetProperty(this.DomainObject.SizePlus, (int? size) => { this.DomainObject.SizePlus = size; }, value); } get { return GetProperty(this.DomainObject.SizePlus, (int?)null); } }
        public override bool IsDirty => myname != this.DomainObject.Name;
        protected override void DomainObjectPropertyChanged(string property)
        {
        }
        protected override bool DirtyCheckProperty()
        {
            return myname != this.DomainObject.Name;
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(BrandVM.Homeland):
                    this.DomainObject.Homeland = (Country)value; break;
                case nameof(BrandVM.Name):
                    if (myname != this.DomainObject.Name)
                        myname = this.DomainObject.Name;
                    else
                        this.Name = (string)value;
                    break;
                case nameof(BrandVM.Producer):
                    this.DomainObject.Producer = (string)value; break;
                case nameof(BrandVM.SizePlus):
                    this.DomainObject.SizePlus = (int?)value; break;
            }
        }

        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.Name):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myname, out errmsg, out _);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }

        protected override void InitProperties()
        {
            myname = this.DomainObject.Name;
        }

        private ListCollectionView mycountries;
        public CountryList Countries
        { get { return CustomBrokerWpf.References.Countries; } }

    }

    public class BrandSynchronizer : lib.ModelViewCollectionsSynchronizer<Brand, BrandVM>
    {
        protected override BrandVM Wrap(Brand fill)
        {
            return new BrandVM(fill);
        }

        protected override Brand UnWrap(BrandVM wrap)
        {
            return wrap.DomainObject;
        }
    }

    public class BrandViewCMD : lib.ViewModelViewCommand
    {
        internal BrandViewCMD()
        {
            myfiltermanager = new libui.Filter.CommandFilterManager(CustomBrokerWpf.References.ConnectionString, "brand");
            myfiltermanager.RunFilter = this.RunFilter;
            mybdbm = new BrandDBM();
            mydbm = mybdbm;
            mybdbm.Collection = new System.Collections.ObjectModel.ObservableCollection<Brand>();
            mybdbm.Filter = myfiltermanager.SQLFilter;
            mybdbm.FillAsyncCompleted = () =>
            {
                if (mydbm.Errors.Count > 0)
                    OpenPopup(mydbm.ErrorMessage, true);
            };
            mybdbm.FillAsync();
            mysync = new BrandSynchronizer();
            mysync.DomainCollection = mybdbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            mycountries = new ListCollectionView(CustomBrokerWpf.References.Countries);
            mycountries.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(Country.Name), System.ComponentModel.ListSortDirection.Ascending));
            CustomBrokerWpf.References.CountryViewCollector.AddView(mycountries as lib.Interfaces.IRefresh);

            #region Filter
            mybrandfilter = myfiltermanager.FilterCreate<BrandCheckListFilter>(libui.Filter.FilterType.CheckListFill, "brand", myview.OfType<BrandVM>());
            myproducerfilter = myfiltermanager.FilterCreate<ProducerCheckListFilter>(libui.Filter.FilterType.CheckListFill, "producer", myview.OfType<BrandVM>());
            myhomelandfilter = myfiltermanager.FilterCreate<HomelandCheckListFilter>(libui.Filter.FilterType.CheckListFill, "homeland", myview.OfType<BrandVM>());
            mysizeplusfilter = myfiltermanager.FilterCreate<libui.NumberFilterVM>(libui.Filter.FilterType.NumberFilter, "sizeplus");
   			if (myfiltermanager.SQLFilter.isEmpty)
				this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
            else
                myfiltermanager.FilterFill();
            #endregion
        }
        ~BrandViewCMD() { Dispose(); }
        public void Dispose()
        {
            CustomBrokerWpf.References.CountryViewCollector.RemoveView(mycountries as lib.Interfaces.IRefresh);
            myfiltermanager.Dispose();
        }

        private BrandDBM mybdbm;
        private BrandSynchronizer mysync;
        #region Filter
        private libui.Filter.CommandFilterManager myfiltermanager;
        public libui.Filter.CommandFilterManager FilterManager { get { return myfiltermanager; } }

        private BrandCheckListFilter mybrandfilter;
        public BrandCheckListFilter BrandFilter { get { return mybrandfilter; } }
        private ProducerCheckListFilter myproducerfilter;
        public ProducerCheckListFilter ProducerFilter { get { return myproducerfilter; } }
        private HomelandCheckListFilter myhomelandfilter;
        public HomelandCheckListFilter HomelandFilter { get { return myhomelandfilter; } }
        private libui.NumberFilterVM mysizeplusfilter;
        public libui.NumberFilterVM SizePlusFilter { get { return mysizeplusfilter; } }

        public void RunFilter(object filters)
		{
			this.Save.Execute(null);
			if (!LastSaveResult)
				this.OpenPopup("Применение фильтра\nПрименение фильтра невозможно. Не удалось сохранить изменения. \n Сохраните или отмените изменения, затем примените фильтр.", true);
			else
			{
				this.Refresh.Execute(null);
			}
		}

        #endregion
        private ListCollectionView mycountries;
        public ListCollectionView Countries
        { get { return mycountries; } }

        protected override void OtherViewRefresh()
        {
            CustomBrokerWpf.References.BrandViewCollector.RefreshViews();
        }
        protected override void SettingView()
        {
            myview.NewItemPlaceholderPosition = System.ComponentModel.NewItemPlaceholderPosition.AtBeginning;
            this.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(BrandVM.Name), System.ComponentModel.ListSortDirection.Ascending));
        }
        protected override void RefreshData(object parametr)
        {
            myfiltermanager.FilterUpdate();
            this.FilterUpdate();
            mybdbm.FillAsync();
        }
        private void FilterUpdate()
        {
			if (mybrandfilter.FilterOn)
			{
				string[] items = new string[mybrandfilter.SelectedItems.Count];
				for (int i = 0; i < mybrandfilter.SelectedItems.Count; i++)
					items[i] = (string)mybrandfilter.SelectedItems[i];
				myfiltermanager.SQLFilter.SetList(myfiltermanager.SQLFilter.FilterWhereId, "brand", items);
			}
			else
				myfiltermanager.SQLFilter.SetList(myfiltermanager.SQLFilter.FilterWhereId, "brand", new string[0]);
			if (myproducerfilter.FilterOn)
			{
				string[] items = new string[myproducerfilter.SelectedItems.Count];
				for (int i = 0; i < myproducerfilter.SelectedItems.Count; i++)
					items[i] = (string)myproducerfilter.SelectedItems[i];
				myfiltermanager.SQLFilter.SetList(myfiltermanager.SQLFilter.FilterWhereId, "producer", items);
			}
			else
				myfiltermanager.SQLFilter.SetList(myfiltermanager.SQLFilter.FilterWhereId, "producer", new string[0]);
			if (myhomelandfilter.FilterOn)
			{
				string[] items = new string[myhomelandfilter.SelectedItems.Count];
				for (int i = 0; i < myhomelandfilter.SelectedItems.Count; i++)
					items[i] = (myhomelandfilter.SelectedItems[i] as Country).Code.ToString();
				myfiltermanager.SQLFilter.SetList(myfiltermanager.SQLFilter.FilterWhereId, "homeland", items);
			}
			else
				myfiltermanager.SQLFilter.SetList(myfiltermanager.SQLFilter.FilterWhereId, "homeland", new string[0]);
        }
    }

    public class BrandCheckListFilter : libui.CheckListBoxVMFill<BrandVM, string>
    {
        protected override void AddItem(BrandVM item)
        {
            if (!Items.Contains(item.Name)) Items.Add(item.Name);
        }
    }
    public class ProducerCheckListFilter : libui.CheckListBoxVMFill<BrandVM, string>
    {
        protected override void AddItem(BrandVM item)
        {
            if (!Items.Contains(item.Producer)) Items.Add(item.Producer);
        }
    }
    public class HomelandCheckListFilter : libui.CheckListBoxVMFill<BrandVM, Country>
    {
        public HomelandCheckListFilter()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            this.GetDisplayPropertyValueFunc = (item) => { return ((Country)item).Name; };
        }
        protected override void AddItem(BrandVM item)
        {
            if (!Items.Contains(item.Homeland)) Items.Add(item.Homeland);
        }
    }

}
