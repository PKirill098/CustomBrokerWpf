using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using Microsoft.Win32;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class VendorCode : lib.DomainBaseStamp
    {
        public VendorCode(int id, long stamp,DateTime updated, lib.DomainObjectState mstate
            ,string brand,string contexture,string countryru,string description,string goods,string gender,string note, bool noupdate,string tnved,string translation, string vendorcode
            ) : base(id, stamp, updated, null, mstate)
        {
            mybrand = brand;
            mycontexture = contexture;
            mycountryru = countryru;
            mydescription = description;
            mygoods = goods;
            mygender = gender;
            mynote = note;
            mynoupdate = noupdate;
            mytnved = tnved;
            mytranslation = translation;
            mycode = vendorcode;
        }
        public VendorCode():this(lib.NewObjectId.NewId,0,DateTime.Now,lib.DomainObjectState.Added
            , null, null, null, null, null, null, null, false, null, null, null)
        { }

        private string mybrand;
        public string Brand
        {
            set { SetProperty<string>(ref mybrand, value); }
            get { return mybrand; }
        }
        private string mycode;
        public string Code
        {
            set { SetProperty<string>(ref mycode, value); }
            get { return mycode; }
        }
        private string mycontexture;
        public string Contexture
        {
            set { SetProperty<string>(ref mycontexture, value); }
            get { return mycontexture; }
        }
        private string mycountryru;
        public string CountryRU
        {
            set { SetProperty<string>(ref mycountryru, value); }
            get { return mycountryru; }
        }
        private string mydescription;
        public string Description
        {
            set { SetProperty<string>(ref mydescription, value); }
            get { return mydescription;
 }
        }
        private string mygender;
        public string Gender
        {
            set { SetProperty<string>(ref mygender, value); }
            get { return mygender; }
        }
        private string mygoods;
        public string Goods
        {
            set { SetProperty<string>(ref mygoods, value); }
            get { return mygoods; }
        }
        private string mynote;
        public string Note
        {
            set { SetProperty<string>(ref mynote, value); }
            get { return mynote; }
        }
        private bool mynoupdate;
        public bool NoUpdate
        {
            set { SetProperty<bool>(ref mynoupdate, value); }
            get { return mynoupdate; }
        }
        private string mytnved;
        public string TNVED
        {
            set { SetProperty<string>(ref mytnved, value); }
            get { return mytnved; }
        }
        private string mytranslation;
        public string Translation
        {
            set { SetProperty<string>(ref mytranslation, value); }
            get { return mytranslation; }
        }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {

        }
    }

    public class VendorCodeDBM : lib.DBManagerStamp<VendorCode,VendorCode>
    {
        public VendorCodeDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "spec.VendorCode_sp";
            InsertCommandText = "spec.VendorCodeUpd_sp";
            UpdateCommandText = "spec.VendorCodeUpd_sp";
            DeleteCommandText = "spec.VendorCodeDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@param1", System.Data.SqlDbType.Int) };
            UpdateParams = new SqlParameter[] {UpdateParams[0]
                ,new SqlParameter("@noupdatechd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@vendorcodechd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@brandchd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@contexturechd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@countryruchd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@descriptionchd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@goodschd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@genderchd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@notechd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@tnvedchd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@translationchd", System.Data.SqlDbType.Bit)
            };
            InsertUpdateParams = new SqlParameter[] {
                 new SqlParameter("@idupd", System.Data.SqlDbType.Int)
                ,new SqlParameter("@noupdate", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@vendorcode", System.Data.SqlDbType.NVarChar,50)
                ,new SqlParameter("@brand", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@contexture", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@countryru", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@description", System.Data.SqlDbType.NVarChar,200)
                ,new SqlParameter("@goods", System.Data.SqlDbType.NVarChar,1000)
                ,new SqlParameter("@gender", System.Data.SqlDbType.NVarChar,10)
                ,new SqlParameter("@note", System.Data.SqlDbType.NVarChar,200)
                ,new SqlParameter("@tnved", System.Data.SqlDbType.NVarChar,10)
                ,new SqlParameter("@translation", System.Data.SqlDbType.NVarChar,50)
            };
        }

        private SQLFilter myfilter;
        internal SQLFilter Filter
        {
            set { myfilter = value; }
            get { return myfilter; }
        }

		protected override VendorCode CreateRecord(SqlDataReader reader)
		{
            return new VendorCode(reader.GetInt32(0), reader.GetInt64(1), reader.GetDateTime(this.Fields["updated"]),lib.DomainObjectState.Unchanged
                , reader.IsDBNull(this.Fields["brand"]) ? null: reader.GetString(this.Fields["brand"])
                , reader.IsDBNull(this.Fields["contexture"]) ? null: reader.GetString(this.Fields["contexture"])
                , reader.IsDBNull(this.Fields["countryru"]) ? null: reader.GetString(this.Fields["countryru"])
                , reader.IsDBNull(this.Fields["description"]) ? null: reader.GetString(this.Fields["description"])
                , reader.IsDBNull(this.Fields["goods"]) ? null: reader.GetString(this.Fields["goods"])
                , reader.IsDBNull(this.Fields["gender"]) ? null: reader.GetString(this.Fields["gender"])
                , reader.IsDBNull(this.Fields["note"]) ? null: reader.GetString(this.Fields["note"])
                , reader.GetBoolean(this.Fields["noupdate"])
                , reader.IsDBNull(this.Fields["tnved"]) ? null: reader.GetString(this.Fields["tnved"])
                , reader.IsDBNull(this.Fields["translation"]) ? null: reader.GetString(this.Fields["translation"])
                , reader.IsDBNull(this.Fields["vendorcode"])?null:reader.GetString(this.Fields["vendorcode"])
                );
		}
        protected override VendorCode CreateModel(VendorCode reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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
        protected override void GetOutputSpecificParametersValue(VendorCode item)
        {
        }
        protected override bool SaveChildObjects(VendorCode item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(VendorCode item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
           SelectParams[0].Value= myfilter?.FilterWhereId;
        }
        protected override bool SetSpecificParametersValue(VendorCode item)
        {
            foreach (SqlParameter par in UpdateParams)
                switch (par.ParameterName)
                {
                    case "@brandchd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Brand));
                        break;
                    case "@contexturechd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Contexture));
                        break;
                    case "@countryruchd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.CountryRU));
                        break;
                    case "@descriptionchd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Description));
                        break;
                    case "@goodschd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Goods));
                        break;
                    case "@genderchd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Gender));
                        break;
                    case "@notechd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Note));
                        break;
                    case "@noupdatechd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.NoUpdate));
                        break;
                    case "@tnvedchd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.TNVED));
                        break;
                    case "@translationchd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Translation));
                        break;
                    case "@vendorcodechd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Code));
                        break;
                }
            foreach (SqlParameter par in InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@brand":
                        par.Value = item.Brand;
                        break;
                    case "@contexture":
                        par.Value = item.Contexture;
                        break;
                    case "@countryru":
                        par.Value = item.CountryRU;
                        break;
                    case "@description":
                        par.Value = item.Description;
                        break;
                    case "@goods":
                        par.Value = item.Goods;
                        break;
                    case "@gender":
                        par.Value = item.Gender;
                        break;
                    case "@idupd":
                        par.Value = item.Id;
                        break;
                    case "@note":
                        par.Value = item.Note;
                        break;
                    case "@noupdate":
                        par.Value = item.NoUpdate;
                        break;
                    case "@tnved":
                        par.Value = item.TNVED;
                        break;
                    case "@translation":
                        par.Value = item.Translation;
                        break;
                    case "@vendorcode":
                        par.Value = item.Code;
                        break;
                }
            return true;
        }
    }

    internal class VendorCodesDBM : lib.DBMExec
    {
        internal VendorCodesDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectCommandText = "spec.VendorCodeUpd_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@param1", System.Data.SqlDbType.Int) };
        }

        private Specification myspecification;
        internal Specification Specification
        {
            set { myspecification = value; }
            get { return myspecification; }
        }

        protected override void PrepareFill(SqlConnection addcon)
        {
            SelectParams[0].Value=myspecification?.Id
;
        }
    }

    public class VendorCodeVM : lib.ViewModelErrorNotifyItem<VendorCode>
    {
        public VendorCodeVM(VendorCode model):base(model)
        {
            DeleteRefreshProperties.AddRange(new string[] { nameof(this.Code), nameof(this.Brand), nameof(this.Contexture), nameof(this.CountryRU), nameof(this.Description), nameof(this.Gender), nameof(this.Goods), nameof(this.Note), nameof(this.NoUpdate), nameof(this.TNVED), nameof(this.Translation) });
            ValidetingProperties.AddRange(new string[] { nameof(this.Code),nameof(this.Brand)});
            InitProperties();
        }
        public VendorCodeVM():this(new VendorCode()) { }

        public string Brand
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Brand, value)))
                {
                    string name = "Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Brand);
                    ChangingDomainProperty = name; this.DomainObject.Brand = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Brand : null; }
        }
        public string Code
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Code, value)))
                {
                    string name = nameof(this.Code);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Code);
                    ChangingDomainProperty = name; this.DomainObject.Code = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Code : null; }
        }
        public string Contexture
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Contexture, value)))
                {
                    string name = "Contexture";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Contexture);
                    ChangingDomainProperty = name; this.DomainObject.Contexture = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Contexture : null; }
        }
        public string CountryRU
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CountryRU, value)))
                {
                    string name = "CountryRU";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CountryRU);
                    ChangingDomainProperty = name; this.DomainObject.CountryRU = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CountryRU : null; }
        }
        public string Description
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Description, value)))
                {
                    string name = "Description";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Description);
                    ChangingDomainProperty = name; this.DomainObject.Description = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Description : null; }
        }
        public string Gender
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Gender, value)))
                {
                    string name = "Gender";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Gender);
                    ChangingDomainProperty = name; this.DomainObject.Gender = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Gender : null; }
        }
        public string Goods
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Goods, value)))
                {
                    string name = nameof(this.Goods);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Goods);
                    ChangingDomainProperty = name; this.DomainObject.Goods = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Goods : null; }
        }
        public string Note
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Note, value)))
                {
                    string name = "Note";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Note);
                    ChangingDomainProperty = name; this.DomainObject.Note = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Note : null; }
        }
        public bool? NoUpdate
        {
            set
            {
                if (this.NoUpdate.HasValue && !(this.IsReadOnly || bool.Equals(this.DomainObject.NoUpdate, value.Value)))
                {
                    string name = nameof(this.NoUpdate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.NoUpdate);
                    ChangingDomainProperty = name; this.DomainObject.NoUpdate = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.NoUpdate : (bool?)null; }
        }
        public string TNVED
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.TNVED, value)))
                {
                    string name = "TNVED";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TNVED);
                    ChangingDomainProperty = name; this.DomainObject.TNVED = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TNVED : null; }
        }
        public string Translation
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Translation, value)))
                {
                    string name = nameof(this.Translation);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Translation);
                    ChangingDomainProperty = name; this.DomainObject.Translation = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Translation : null; }
        }
        public DateTime? Updated { get { return this.IsEnabled ? this.DomainObject.UpdateWhen : null; } }

        protected override bool DirtyCheckProperty()
        {
            return string.IsNullOrEmpty(this.Brand) || string.IsNullOrEmpty(this.Code);
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
                case nameof(this.Brand):
                    this.DomainObject.Brand = (string)value;
                    break;
                case nameof(this.Code):
                    this.DomainObject.Code = (string)value;
                    break;
                case nameof(this.Contexture):
                    this.DomainObject.Contexture = (string)value;
                    break;
                case nameof(this.CountryRU):
                    this.DomainObject.CountryRU = (string)value;
                    break;
                case nameof(this.Description):
                    this.DomainObject.Description = (string)value;
                    break;
                case nameof(this.Gender):
                    this.DomainObject.Gender = (string)value;
                    break;
                case nameof(this.Goods):
                    this.DomainObject.Goods = (string)value;
                    break;
                case nameof(this.Note):
                    this.DomainObject.Note = (string)value;
                    break;
                case nameof(this.NoUpdate):
                    this.DomainObject.NoUpdate = (bool)value;
                    break;
                case nameof(this.TNVED):
                    this.DomainObject.TNVED = (string)value;
                    break;
                case nameof(this.Translation):
                    this.DomainObject.Translation = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.Brand):
                    if (string.IsNullOrEmpty(this.Brand))
                    {
                        errmsg = "Отсутствует Торговая марка";
                        isvalid = false;
                    }
                    break;
                case nameof(this.Code):
                    if (string.IsNullOrEmpty(this.Code))
                    {
                        errmsg = "Отсутствует Артикул";
                        isvalid = false;
                    }
                    break;
            }
            if(isvalid)
                ClearErrorMessageForProperty(propertyname);
            else if (inform & !isvalid)
                AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    public class VendorCodeSynchronizer : lib.ModelViewCollectionsSynchronizer<VendorCode, VendorCodeVM>
    {
        protected override VendorCode UnWrap(VendorCodeVM wrap)
        {
            return wrap.DomainObject as VendorCode;
        }
        protected override VendorCodeVM Wrap(VendorCode fill)
        {
            return new VendorCodeVM(fill);
        }
    }

    public class VendorCodeViewCommand : lib.ViewModelViewCommand,IDisposable
    {
        internal VendorCodeViewCommand()
        {
            myfilter = new SQLFilter("vendorcode", "AND");
            myvddbm = new VendorCodeDBM() { Filter = myfilter };
            mydbm = myvddbm;
            myvddbm.Collection = new System.Collections.ObjectModel.ObservableCollection<VendorCode>();
            myvddbm.FillAsyncCompleted = () => { if (myvddbm.Errors.Count > 0) OpenPopup(myvddbm.ErrorMessage, true); };
            mysync = new VendorCodeSynchronizer() { DomainCollection = myvddbm.Collection };
            base.Collection = mysync.ViewModelCollection;

            #region Filters
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            mybrandfilter = new VendorCodeBrandFilter();
            mybrandfilter.DeferredFill = true;
            mybrandfilter.ItemsSource = myview.OfType<VendorCodeVM>();
            mybrandfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mybrandfilter.ExecCommand2 = () => { mybrandfilter.Clear(); };
            mybrandfilter.FillDefault = () =>
            {
                if (this.FilterIsEmpty)
                    foreach (string item in mybrandfilter.DefaultList)
                        mybrandfilter.Items.Add(item);
                return myfilter.isEmpty;
            };
            mycontexturefilter = new VendorCodeContextureFilter();
            mycontexturefilter.DeferredFill = true;
            mycontexturefilter.ItemsSource = myview.OfType<VendorCodeVM>();
            mycontexturefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycontexturefilter.ExecCommand2 = () => { mycontexturefilter.Clear(); };
            mycountryrufilter = new VendorCodeCountryRuFilter();
            mycountryrufilter.DeferredFill = true;
            mycountryrufilter.ItemsSource = myview.OfType<VendorCodeVM>();
            mycountryrufilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycountryrufilter.ExecCommand2 = () => { mycountryrufilter.Clear(); };
            mycountryrufilter.FillDefault = () =>
            {
                if (this.FilterIsEmpty)
                    foreach (Domain.References.Country item in CustomBrokerWpf.References.Countries)
                        mycountryrufilter.Items.Add(item.Name);
                return myfilter.isEmpty;
            };
            mydescriptionfilter = new VendorCodeDescriptionFilter();
            mydescriptionfilter.DeferredFill = true;
            mydescriptionfilter.ItemsSource = myview.OfType<VendorCodeVM>();
            mydescriptionfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mydescriptionfilter.ExecCommand2 = () => { mydescriptionfilter.Clear(); };
            mygenderfilter = new libui.CheckListBoxVM();
            mygenderfilter.DisplayPath = "Name";
            mygenderfilter.GetDisplayPropertyValueFunc = (item) => { return ((Gender)item).Name; };
            mygenderfilter.SearchPath = "Name";
            mygenderfilter.Items = CustomBrokerWpf.References.Genders;
            mygenderfilter.ItemsViewFilterDefault = lib.ViewModelViewCommand.ViewFilterDefault;
            mygenderfilter.SelectedAll = false;
            mygenderfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mygenderfilter.ExecCommand2 = () => { mygenderfilter.Clear(); };
            mygenderfilter.AreaFilterIsVisible = false;
            mygoodsfilter = new VendorCodeGoodsFilter();
            mygoodsfilter.DeferredFill = true;
            mygoodsfilter.ItemsSource = myview.OfType<VendorCodeVM>();
            mygoodsfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mygoodsfilter.ExecCommand2 = () => { mygoodsfilter.Clear(); };
            mynotefilter = new VendorCodeNoteFilter();
            mynotefilter.DeferredFill = true;
            mynotefilter.ItemsSource = myview.OfType<VendorCodeVM>();
            mynotefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mynotefilter.ExecCommand2 = () => { mynotefilter.Clear(); };
            mytnvedfilter = new VendorCodeTNVEDFilter();
            mytnvedfilter.DeferredFill = true;
            mytnvedfilter.ItemsSource = myview.OfType<VendorCodeVM>();
            mytnvedfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mytnvedfilter.ExecCommand2 = () => { mytnvedfilter.Clear(); };
            mytranslationfilter = new VendorCodeTranslationFilter();
            mytranslationfilter.DeferredFill = true;
            mytranslationfilter.ItemsSource = myview.OfType<VendorCodeVM>();
            mytranslationfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mytranslationfilter.ExecCommand2 = () => { mytranslationfilter.Clear(); };
            myvendorcodefilter = new VendorCodeCodeFilter();
            myvendorcodefilter.DeferredFill = true;
            myvendorcodefilter.ItemsSource = myview.OfType<VendorCodeVM>();
            myvendorcodefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myvendorcodefilter.ExecCommand2 = () => { myvendorcodefilter.Clear(); };
            #endregion
            if (myfilter.isEmpty)
                this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
            myexcelimport = new RelayCommand(ExcelImportExec, ExcelImportCanExec);
        }
        ~VendorCodeViewCommand()
        { Dispose(); }

        private VendorCodeDBM myvddbm;
        private VendorCodeSynchronizer mysync;
        private SQLFilter myfilter;
        internal SQLFilter Filter
        { get { return myfilter; } }
        private VendorCodeBrandFilter mybrandfilter;
        public VendorCodeBrandFilter BrandFilter
        {
            get { return mybrandfilter; }
        }
        private VendorCodeContextureFilter mycontexturefilter;
        public VendorCodeContextureFilter ContextureFilter
        { get { return mycontexturefilter; } }
        private VendorCodeCountryRuFilter mycountryrufilter;
        public VendorCodeCountryRuFilter CountryRuFilter
        {
            get { return mycountryrufilter; }
        }
        private VendorCodeDescriptionFilter mydescriptionfilter;
        public VendorCodeDescriptionFilter DescriptionFilter
        { get { return mydescriptionfilter; } }
        private libui.CheckListBoxVM mygenderfilter;
        public libui.CheckListBoxVM GenderFilter
        { get { return mygenderfilter; } }
        private VendorCodeGoodsFilter mygoodsfilter;
        public VendorCodeGoodsFilter GoodsFilter
        { get { return mygoodsfilter; } }
        private VendorCodeNoteFilter mynotefilter;
        public VendorCodeNoteFilter NoteFilter
        { get { return mynotefilter; } }
        private VendorCodeTNVEDFilter mytnvedfilter;
        public VendorCodeTNVEDFilter TNVEDFilter
        { get { return mytnvedfilter; } }
        private VendorCodeTranslationFilter mytranslationfilter;
        public VendorCodeTranslationFilter TranslationFilter
        { get { return mytranslationfilter; } }
        private VendorCodeCodeFilter myvendorcodefilter;
        public VendorCodeCodeFilter VendorCodeFilter
        {get { return myvendorcodefilter; } }

        private bool FilterIsEmpty
        { get { return !(mybrandfilter.FilterOn | mycontexturefilter.FilterOn | mycountryrufilter.FilterOn | mydescriptionfilter.FilterOn | mygenderfilter.FilterOn | mygoodsfilter.FilterOn | mynotefilter.FilterOn | mytnvedfilter.FilterOn | mytranslationfilter.FilterOn | myvendorcodefilter.FilterOn); } }
        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            UpdateFilter();
            this.EndEdit();
            RefreshData(null);
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
            mycontexturefilter.Clear();
            mycontexturefilter.IconVisibileChangedNotification();
            mycountryrufilter.Clear();
            mycountryrufilter.IconVisibileChangedNotification();
            mydescriptionfilter.Clear();
            mydescriptionfilter.IconVisibileChangedNotification();
            mygenderfilter.Clear();
            mygenderfilter.IconVisibileChangedNotification();
            mygoodsfilter.Clear();
            mygoodsfilter.IconVisibileChangedNotification();
            mynotefilter.Clear();
            mynotefilter.IconVisibileChangedNotification();
            mytnvedfilter.Clear();
            mytnvedfilter.IconVisibileChangedNotification();
            mytranslationfilter.Clear();
            mytranslationfilter.IconVisibileChangedNotification();
            myvendorcodefilter.Clear();
            myvendorcodefilter.IconVisibileChangedNotification();
            this.UpdateFilter();
            this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }
        private void UpdateFilter()
        {
            if (mybrandfilter.FilterOn)
            {
                string[] items = new string[mybrandfilter.SelectedItems.Count];
                for (int i = 0; i < mybrandfilter.SelectedItems.Count; i++)
                    items[i] = (string)mybrandfilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "brand", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "brand", new string[0]);
            if (mycontexturefilter.FilterOn)
            {
                if (mycontexturefilter.SelectedItems.Count > 0)
                {
                    string[] items = new string[mycontexturefilter.SelectedItems.Count];
                    for (int i = 0; i < mycontexturefilter.SelectedItems.Count; i++)
                        items[i] = (string)mycontexturefilter.SelectedItems[i];
                    myfilter.SetList(myfilter.FilterWhereId, "contexture", items);
                }
                else
                    myfilter.SetString(myfilter.FilterWhereId, "contexture", mycontexturefilter.ItemsViewFilter);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "contexture", new string[0]);
            if (mycountryrufilter.FilterOn)
            {
                string[] items = new string[mycountryrufilter.SelectedItems.Count];
                for (int i = 0; i < mycountryrufilter.SelectedItems.Count; i++)
                    items[i] = (string)mycountryrufilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "countryru", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "countryru", new string[0]);
            if (mydescriptionfilter.FilterOn)
            {
                if (mydescriptionfilter.SelectedItems.Count > 0)
                {
                    string[] items = new string[mydescriptionfilter.SelectedItems.Count];
                    for (int i = 0; i < mydescriptionfilter.SelectedItems.Count; i++)
                        items[i] = (string)mydescriptionfilter.SelectedItems[i];
                    myfilter.SetList(myfilter.FilterWhereId, "description", items);
                }
                else
                    myfilter.SetString(myfilter.FilterWhereId, "description", mydescriptionfilter.ItemsViewFilter);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "description", new string[0]);
            if (mygenderfilter.FilterOn)
            {
                string[] items = new string[mygenderfilter.SelectedItems.Count];
                for (int i = 0; i < mygenderfilter.SelectedItems.Count; i++)
                    items[i] = (mygenderfilter.SelectedItems[i] as Gender).Name;
                myfilter.SetList(myfilter.FilterWhereId, "gender", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "gender", new string[0]);
            if (mygoodsfilter.FilterOn)
            {
                if (mygoodsfilter.SelectedItems.Count > 0)
                {
                    string[] items = new string[mygoodsfilter.SelectedItems.Count];
                    for (int i = 0; i < mygoodsfilter.SelectedItems.Count; i++)
                        items[i] = (string)mygoodsfilter.SelectedItems[i];
                    myfilter.SetList(myfilter.FilterWhereId, "goods", items);
                }
                else
                    myfilter.SetString(myfilter.FilterWhereId, "goods", mygoodsfilter.ItemsViewFilter);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "goods", new string[0]);
            if (mynotefilter.FilterOn)
            {
                if (mynotefilter.SelectedItems.Count > 0)
                {
                    string[] items = new string[mynotefilter.SelectedItems.Count];
                    for (int i = 0; i < mynotefilter.SelectedItems.Count; i++)
                        items[i] = (string)mynotefilter.SelectedItems[i];
                    myfilter.SetList(myfilter.FilterWhereId, "note", items);
                }
                else
                    myfilter.SetString(myfilter.FilterWhereId, "note", mynotefilter.ItemsViewFilter);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "note", new string[0]);
            if (mytnvedfilter.FilterOn)
            {
                if (mytnvedfilter.SelectedItems.Count > 0)
                {
                    string[] items = new string[mytnvedfilter.SelectedItems.Count];
                    for (int i = 0; i < mytnvedfilter.SelectedItems.Count; i++)
                        items[i] = (string)mytnvedfilter.SelectedItems[i];
                    myfilter.SetList(myfilter.FilterWhereId, "tnved", items);
                }
                else
                    myfilter.SetString(myfilter.FilterWhereId, "tnved", mytnvedfilter.ItemsViewFilter);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "tnved", new string[0]);
            if (mytranslationfilter.FilterOn)
            {
                if (mytranslationfilter.SelectedItems.Count > 0)
                {
                    string[] items = new string[mytranslationfilter.SelectedItems.Count];
                    for (int i = 0; i < mytranslationfilter.SelectedItems.Count; i++)
                        items[i] = (string)mytranslationfilter.SelectedItems[i];
                    myfilter.SetList(myfilter.FilterWhereId, "translation", items);
                }
                else
                    myfilter.SetString(myfilter.FilterWhereId, "translation", mytranslationfilter.ItemsViewFilter);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "translation", new string[0]);
            if (myvendorcodefilter.FilterOn)
            {
                if (myvendorcodefilter.SelectedItems.Count > 0)
                {
                    string[] items = new string[myvendorcodefilter.SelectedItems.Count];
                    for (int i = 0; i < myvendorcodefilter.SelectedItems.Count; i++)
                        items[i] = (string)myvendorcodefilter.SelectedItems[i];
                    myfilter.SetList(myfilter.FilterWhereId, "vendorcode", items);
                }
                else
                    myfilter.SetString(myfilter.FilterWhereId, "vendorcode", myvendorcodefilter.ItemsViewFilter);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "vendorcode", new string[0]);
        }

        private lib.TaskAsync.TaskAsync myexceltask;
        private RelayCommand myexcelexport;
        public ICommand ExcelExport
        {
            get { return myexcelexport; }
        }
        private void ExcelExportExec(object parametr)
        {
            this.myendedit();
            if (myexceltask == null)
                myexceltask = new lib.TaskAsync.TaskAsync();
            if (!myexceltask.IsBusy)
            {
                System.Windows.Controls.DataGrid source = parametr as System.Windows.Controls.DataGrid;
                libui.ExcelExportPopUpWindow  win = new libui.ExcelExportPopUpWindow();
                win.SourceDataGrid = source;
                bool? ok = win.ShowDialog();
                if (ok.HasValue && ok.Value)
                {
                    int count;
                    System.Collections.IEnumerable items;
                    if (source.SelectedItems.Count > 1)
                    {
                        items = source.SelectedItems;
                        count = source.SelectedItems.Count;
                    }
                    else
                    {
                        items = myview;
                        count = myview.Count;
                    }
                    myexceltask.DoProcessing = OnExcelExport;
                    myexceltask.Run(new object[3] { win.Columns, items, count });
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool ExcelExportCanExec(object parametr)
        { return !(myview.IsAddingNew | myview.IsEditingItem); }
        private KeyValuePair<bool, string> OnExcelExport(object args)
        {
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            myexceltask.ProgressChange(2);
            try
            {
                int row = 2, column = 1;
                exApp.SheetsInNewWorkbook = 1;
                Excel.Workbook exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                Excel.Range r;
                exWh.Name = "Артикулы";

                int maxrow = (int)(args as object[])[2] + 1;
                System.Collections.IEnumerable items = (args as object[])[1] as System.Collections.IEnumerable;
                libui.WPFDataGrid.DataGridColumnInfo[] columns = ((args as object[])[0] as libui.WPFDataGrid.DataGridColumnInfo[]);
                exWh.Rows[1, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                foreach (libui.WPFDataGrid.DataGridColumnInfo columninfo in columns)
                {
                    if (!string.IsNullOrEmpty(columninfo.Property))
                    {
                        exWh.Cells[1, column] = columninfo.Header;
                        switch (columninfo.Property)
                        {
                            case nameof(VendorCodeVM.Gender):
                            case nameof(VendorCodeVM.Code):
                            case nameof(VendorCodeVM.TNVED):
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                exWh.Columns[column, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                                break;
                            case nameof(VendorCodeVM.Contexture):
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                break;
                        }
                        column++;
                    }
                    else
                        break;
                }
                myexceltask.ProgressChange(2 + (int)(decimal.Divide(1, maxrow) * 100));

                foreach (VendorCodeVM item in items.OfType<VendorCodeVM>())
                {
                    column = 1;
                    foreach (libui.WPFDataGrid.DataGridColumnInfo columninfo in columns)
                    {
                        switch (columninfo.Property)
                        {
                            case nameof(VendorCodeVM.Brand):
                                exWh.Cells[row, column] = item.Brand;
                                break;
                            case nameof(VendorCodeVM.Code):
                                exWh.Cells[row, column] = item.Code;
                                break;
                            case nameof(VendorCodeVM.Contexture):
                                exWh.Cells[row, column] = item.Contexture;
                                break;
                            case nameof(VendorCodeVM.CountryRU):
                                exWh.Cells[row, column] = item.CountryRU;
                                break;
                            case nameof(VendorCodeVM.Description):
                                exWh.Cells[row, column] = item.Description;
                                break;
                            case nameof(VendorCodeVM.Gender):
                                exWh.Cells[row, column] = item.Gender;
                                break;
                            case nameof(VendorCodeVM.Goods):
                                exWh.Cells[row, column] = item.Goods;
                                break;
                            case nameof(VendorCodeVM.Note):
                                exWh.Cells[row, column] = item.Note;
                                break;
                            case nameof(VendorCodeVM.TNVED):
                                exWh.Cells[row, column] = item.TNVED;
                                break;
                            case nameof(VendorCodeVM.Translation):
                                exWh.Cells[row, column] = item.Translation;
                                break;
                        }
                        column++;
                    }
                    row++;
                    myexceltask.ProgressChange(2 + (int)(decimal.Divide(row, maxrow) * 100));
                }

                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, column - 1]];
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                r.VerticalAlignment = Excel.Constants.xlTop;
                r.WrapText = true;
                r = exWh.Range[exWh.Columns[1, Type.Missing], exWh.Columns[column - 1, Type.Missing]]; r.Columns.AutoFit();

                exWh = null;
                exApp.Visible = true;
                exApp.DisplayAlerts = true;
                exApp.ScreenUpdating = true;
                myexceltask.ProgressChange(100);
                return new KeyValuePair<bool, string>(false, "Данные выгружены. " + (row-2).ToString() + " строк обработано.");
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

        private RelayCommand myexcelimport;
        public ICommand ExcelImport
        {
            get { return myexcelimport; }
        }
        private void ExcelImportExec(object parametr)
        {
            if (myexceltask == null)
                myexceltask = new lib.TaskAsync.TaskAsync();
            if (!myexceltask.IsBusy)
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.Multiselect = false;
                fd.CheckPathExists = true;
                fd.CheckFileExists = true;
                if (System.IO.Directory.Exists(CustomBrokerWpf.Properties.Settings.Default.VendorCodeDefault)) fd.InitialDirectory = CustomBrokerWpf.Properties.Settings.Default.VendorCodeDefault;
                fd.Title = "Выбор файла разбивки";
                fd.Filter = "Файлы Excel|*.xls;*.xlsx;*.xlsm;";
                if (fd.ShowDialog().Value)
                {
                    try
                    {
                        if (CustomBrokerWpf.Properties.Settings.Default.VendorCodeDefault != System.IO.Path.GetDirectoryName(fd.FileName))
                        {
                            CustomBrokerWpf.Properties.Settings.Default.VendorCodeDefault = System.IO.Path.GetDirectoryName(fd.FileName);
                            CustomBrokerWpf.Properties.Settings.Default.Save();
                        }
                        myexceltask.DoProcessing = OnExcelImport;
                        myexceltask.Run(fd.FileName);
                    }
                    catch (Exception ex)
                    {
                        this.OpenPopup("Не удалось загрузить файл.\n" + ex.Message, true);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool ExcelImportCanExec(object parametr)
        { return (myexceltask == null || !myexceltask.IsBusy); }
        private KeyValuePair<bool, string> OnExcelImport(object parm)
        {
            int maxr, maxc, usedr = 0, r = 2;
            string filepath = (string)parm, code, brand, goods;
            VendorCode vendorcode;

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
                myexceltask.ProgressChange(5);

                for (; r <= maxr; r++)
                {
                    code = exWh.Cells[r, 1].Text as string;
                    brand = exWh.Cells[r, 2].Text as string;
                    goods = exWh.Cells[r, 3].Text as string;
                    if (string.IsNullOrEmpty(code) | string.IsNullOrEmpty(brand)) continue;
                    vendorcode = mysync.DomainCollection.FirstOrDefault((VendorCode item)=> { return string.Equals(item.Code, code) && string.Equals(item.Brand, brand); });// && string.Equals(item.Goods, goods)
                    if (vendorcode == null)
                    {
                        vendorcode = new VendorCode();
                        vendorcode.Code = code;
                        vendorcode.Brand = brand;
                    }
                    vendorcode.Goods = goods;
                    vendorcode.Description = exWh.Cells[r, 4].Text as string;
                    vendorcode.Contexture = exWh.Cells[r, 5].Text as string;
                    vendorcode.Gender = exWh.Cells[r, 6].Text as string;
                    vendorcode.TNVED = exWh.Cells[r, 7].Text as string;
                    vendorcode.Translation = exWh.Cells[r, 8].Text as string;
                    vendorcode.CountryRU = exWh.Cells[r, 9].Text as string;
                    vendorcode.Note = exWh.Cells[r, 10].Text as string;
                    App.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<VendorCode>(mysync.DomainCollection.Add), vendorcode);
                    usedr++;
                    myexceltask.ProgressChange(r, maxr);
                }
                myexceltask.ProgressChange(99);
                exWb.Close();
                exApp.Quit();

                myexceltask.ProgressChange(100);
                return new KeyValuePair<bool, string>(false, "Данные загружены. " + usedr.ToString() + " строк обработано.");
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
                throw new Exception("Ошибка в строке " + r.ToString() + ": " + ex.Message);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }

        protected override bool CanAddData(object parametr)
        {
            return !(myview.IsAddingNew || myview.IsEditingItem);
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
            if (!myfilter.isEmpty || MessageBox.Show("Загрузить ВСЕ артикулы?", "Артикулы", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                myvddbm.FillAsync();
            else
                this.RefreshSuccessMessageHide = true;
        }
        protected override void SettingView()
        {
           myview.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(VendorCode.Brand),System.ComponentModel.ListSortDirection.Ascending));
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(VendorCode.Code), System.ComponentModel.ListSortDirection.Ascending));
        }

        public void Dispose()
        {
            myfilter.RemoveFilter();
        }
    }

    public class VendorCodeBrandFilter : libui.CheckListBoxVMFillDefault<VendorCodeVM, string>
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
                    GoodsDBM pdbm = new GoodsDBM();
                    pdbm.Fill();
                    foreach (Goods goods in pdbm.Collection)
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

        protected override void AddItem(VendorCodeVM item)
        {
            if (!Items.Contains(item.Brand??string.Empty)) Items.Add(item.Brand ?? string.Empty);
        }
    }
    public class VendorCodeContextureFilter : libui.CheckListBoxVMFill<VendorCodeVM, string>
    {
        protected override void AddItem(VendorCodeVM item)
        {
            if (!Items.Contains(item.Contexture??string.Empty)) Items.Add(item.Contexture ?? string.Empty);
        }
    }
    public class VendorCodeCountryRuFilter : libui.CheckListBoxVMFillDefault<VendorCodeVM, string>
    {
        protected override void AddItem(VendorCodeVM item)
        {
            if (!Items.Contains(item.CountryRU ?? string.Empty)) Items.Add(item.CountryRU??string.Empty);
        }
    }
    public class VendorCodeDescriptionFilter : libui.CheckListBoxVMFill<VendorCodeVM, string>
    {
        protected override void AddItem(VendorCodeVM item)
        {
            if (!Items.Contains(item.Description ?? string.Empty)) Items.Add(item.Description ?? string.Empty);
        }
    }
    public class VendorCodeGoodsFilter : libui.CheckListBoxVMFill<VendorCodeVM, string>
    {
        protected override void AddItem(VendorCodeVM item)
        {
            if (!Items.Contains(item.Goods??string.Empty)) Items.Add(item.Goods??string.Empty);
        }
    }
    public class VendorCodeNoteFilter : libui.CheckListBoxVMFill<VendorCodeVM, string>
    {
        protected override void AddItem(VendorCodeVM item)
        {
            if (!Items.Contains(item.Note ?? string.Empty)) Items.Add(item.Note ?? string.Empty);
        }
    }
    public class VendorCodeTNVEDFilter : libui.CheckListBoxVMFill<VendorCodeVM, string>
    {
        protected override void AddItem(VendorCodeVM item)
        {
            if (!Items.Contains(item.TNVED??string.Empty)) Items.Add(item.TNVED??string.Empty);
        }
    }
    public class VendorCodeTranslationFilter : libui.CheckListBoxVMFill<VendorCodeVM, string>
    {
        protected override void AddItem(VendorCodeVM item)
        {
            if (!Items.Contains(item.Translation ?? string.Empty)) Items.Add(item.Translation ?? string.Empty);
        }
    }
    public class VendorCodeCodeFilter : libui.CheckListBoxVMFill<VendorCodeVM, string>
    {
        protected override void AddItem(VendorCodeVM item)
        {
            if (!Items.Contains(item.Code ?? string.Empty)) Items.Add(item.Code ?? string.Empty);
        }
    }
}
