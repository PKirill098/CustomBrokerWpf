using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.References
{
    public class Country : lib.DomainBaseReject
    {

        public Country() : this(0, string.Empty, string.Empty, string.Empty, null, false, lib.DomainObjectState.Added) { }
        public Country(int code, string shortname, string fullname, string synonym, PriceCategory pricecategory, bool requestlist, lib.DomainObjectState initstate) : base(code, initstate)
        {
            mycode = code;
            myshortname = shortname;
            myfullname = fullname;
            mysynonym = synonym;
            mypricecategory = pricecategory;
            myrequestlist = requestlist;
        }

        int mycode;
        public int Code
        {
            set
            {
                SetProperty<int>(ref mycode, value);
            }
            get { return mycode; }
        }
        public string Name
        {
            get { return string.IsNullOrEmpty(myshortname) ? myfullname : myshortname; }
        }
        string myshortname;
        public string ShortName
        {
            set
            {
                if (this.ValidateProperty(nameof(this.ShortName), value, out _, out _))
                    SetProperty<string>(ref myshortname, value);
            }
            get { return myshortname; }
        }
        string myfullname;
        public string FullName
        {
            set
            {
                if (this.ValidateProperty(nameof(this.FullName), value, out _, out _))
                    SetProperty<string>(ref myfullname, value);
            }
            get { return myfullname; }
        }
        string mysynonym;
        public string Synonym
        {
            set { SetProperty<string>(ref mysynonym, value); }
            get { return mysynonym; }
        }
        private PriceCategory mypricecategory;
        public PriceCategory PriceCategory
        {
            set { SetProperty<PriceCategory>(ref mypricecategory, value); }
            get { return mypricecategory; }
        }
        private bool myrequestlist;
        public bool RequestList
        { set { SetProperty<bool>(ref myrequestlist, value); } get { return myrequestlist; } }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Code":
                    this.mycode = (int)value;
                    break;
                case "ShortName":
                    this.myshortname = (string)value;
                    break;
                case "FullName":
                    this.myfullname = (string)value;
                    break;
                case "Synonym":
                    this.mysynonym = (string)value;
                    break;
                case nameof(this.RequestList):
                    myrequestlist = (bool)value;
                    break;
            }
            return;
        }
        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            Country newitem = (Country)sample;
            this.Code = newitem.Code;
            this.FullName = newitem.FullName;
            this.PriceCategory = newitem.PriceCategory;
            this.RequestList = newitem.RequestList;
            this.ShortName = newitem.ShortName;
            this.Synonym = newitem.Synonym;
        }

        public override bool ValidateProperty(string propertyname, object value, out string errmsg, out byte messageey)
        {
            bool isvalid = true;
            errmsg = null;
            messageey = 0;
            switch (propertyname)
            {
                case nameof(this.FullName):
                    if (string.IsNullOrEmpty((string)value) & string.IsNullOrEmpty(myshortname))
                    {
                        errmsg = "Необходимо указать наименование страны!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.ShortName):
                    if (string.IsNullOrEmpty((string)value) & string.IsNullOrEmpty(myfullname))
                    {
                        errmsg = "Необходимо указать наименование страны!";
                        isvalid = false;
                    }
                    break;
            }
            return isvalid;
        }
    }

    public class CountryDBM : lib.DBManager<Country,Country>
    {
        public CountryDBM()
        {
            this.ConnectionString = KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString;

            this.SelectProcedure = false;
            this.SelectCommandText = "SELECT code,shortname,fullname,synonym,pricecategory,isrequest FROM dbo.Country_tb ORDER BY code";
            this.InsertProcedure = false;
            this.InsertCommandText = "INSERT INTO [dbo].[Country_tb] (code,shortname,fullname,synonym,pricecategory,isrequest) VALUES (@code,@shortname,@fullname,@synonym,@pricecategory,@isrequest)";
            this.UpdateProcedure = false;
            this.UpdateCommandText = "UPDATE [dbo].[Country_tb] SET code=@code,shortname=@shortname,fullname=@fullname,synonym=@synonym,pricecategory=@pricecategory,isrequest=@isrequest WHERE code=@codeold";
            this.DeleteProcedure = false;
            this.DeleteCommandText = "DELETE FROM [dbo].[Country_tb] WHERE code=@codeold";

            this.UpdateParams = new SqlParameter[] {
                new SqlParameter("@codeold", System.Data.SqlDbType.Int)
            };
            this.InsertUpdateParams = new SqlParameter[] {
                new SqlParameter("@code", System.Data.SqlDbType.Int),
                new SqlParameter("@shortname", System.Data.SqlDbType.NVarChar, 30),
                new SqlParameter("@fullname", System.Data.SqlDbType.NVarChar, 100),
                new SqlParameter("@synonym", System.Data.SqlDbType.NVarChar, 200),
                new SqlParameter("@pricecategory", System.Data.SqlDbType.Int),
                new SqlParameter("@isrequest", System.Data.SqlDbType.Bit)
            };
            this.DeleteParams = new SqlParameter[] { new SqlParameter("@codeold", System.Data.SqlDbType.Int) };
        }

		protected override Country CreateRecord(SqlDataReader reader)
		{
            return new Country(
                        reader.GetInt32(0),
                        reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                        reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        reader.IsDBNull(4) ? null : CustomBrokerWpf.References.PriceCategories.FindFirstItem("Id", reader.GetInt32(4)),
                        reader.GetBoolean(5),
                        lib.DomainObjectState.Unchanged);
		}
        protected override Country CreateModel(Country reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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
        protected override void GetOutputParametersValue(Country item)
        {
        }

        protected override bool SetParametersValue(Country item)
        {
            this.UpdateParams[0].Value = item.HasPropertyOutdatedValue("code") ? item.GetPropertyOutdatedValue("code") : item.Code;
            foreach (SqlParameter par in this.InsertUpdateParams)
                switch(par.ParameterName)
                {
                    case "@code":
                        par.Value = item.Code;
                        break;
                    case "@fullname":
                        par.Value = string.IsNullOrEmpty(item.FullName) ? (object)DBNull.Value : item.FullName;
                        break;
                    case "@isrequest":
                        par.Value = item.RequestList;
                        break;
                    case "@pricecategory":
                        par.Value = item.PriceCategory == null ? (object)DBNull.Value : item.PriceCategory.Id;
                        break;
                    case "@shortname":
                        par.Value = string.IsNullOrEmpty(item.ShortName) ? (object)DBNull.Value : item.ShortName;
                        break;
                    case "@synonym":
                        par.Value = string.IsNullOrEmpty(item.Synonym) ? (object)DBNull.Value : item.Synonym;
                        break;
                }
            this.DeleteParams[0].Value = item.HasPropertyOutdatedValue("code") ? item.GetPropertyOutdatedValue("code") : item.Code;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
    }

    public class CountryList : ObservableCollection<Country>, IReference<Country>
    {
        public CountryList() : base()
        {
            mydbm = new CountryDBM();
            myisnotifycollectionchanged = true;
            Fill();
        }

        private CountryDBM mydbm;
        private bool myisnotifycollectionchanged;
        public bool IsNotifyCollectionChanged { set { myisnotifycollectionchanged = value; } get { return myisnotifycollectionchanged; } }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (myisnotifycollectionchanged)
                base.OnCollectionChanged(e);
        }

        private void Fill()
        {
            mydbm.Collection = this;
            mydbm.Fill();
            mydbm.Collection = null;
        }
        public void Refresh()
        {
            using (BlockReentrancy())
            {
                myisnotifycollectionchanged = false;
                int compare, startIndex;
                startIndex = 0;
                mydbm.Fill();
                foreach (Country newitem in mydbm.Collection)
                {
                    compare = -1;
                    for (int i = startIndex; i < this.Count; i++)
                    {
                        startIndex = i + 1;
                        Country olditem = this[i];
                        compare = olditem.Code.CompareTo(newitem.Code);
                        olditem.UpdateProperties(newitem);
                        if (compare < 0)
                        {
                            this.RemoveAt(i);
                            i--;
                        }
                        if (compare == 0)
                        {
                            break;
                        }
                        else if (compare > 0)
                        {
                            this.Insert(i, newitem);
                            break;
                        }
                    }
                    if (compare < 0) this.Add(newitem);
                }
                if (this.Count > startIndex) for (int i = startIndex; i < this.Count; i++) this.RemoveAt(i);
                base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                myisnotifycollectionchanged = true;
            }
        }

        public Country FindFirstItem(string propertyName, object value)
        {
            Country item = null;
            switch (propertyName)
            {
                case "Code":
                    item = this.FirstOrDefault<Country>(x => x.Code.Equals(value));
                    break;
                case "Name":
                    item = this.FirstOrDefault<Country>(x => x.Name.ToUpper().Equals((value as string).ToUpper()));
                    break;
                case "ShortName":
                    item = this.FirstOrDefault<Country>(x => x.ShortName.ToUpper().Equals((value as string).ToUpper()));
                    break;
                case "FullName":
                    item = this.FirstOrDefault<Country>(x => x.FullName.ToUpper().Equals((value as string).ToUpper()));
                    break;
            }
            return item;
        }
    }

    public class CountryVM : lib.ViewModelErrorNotifyItem<Country>
    {
        public CountryVM() : this(new Country()) { }
        internal CountryVM(Country country) : base(country)
        {
            this.ValidetingProperties.AddRange(new string[] { nameof(this.FullName), nameof(this.ShortName) });
            this.DeleteRefreshProperties.AddRange(new string[] { nameof(this.Code), nameof(this.Name), nameof(this.FullName), nameof(this.ShortName), nameof(this.PriceCategory), nameof(this.FullName) });
            InitProperties();
        }

        public int? Code
        {
            set
            {
                if (!this.IsReadOnly & value.HasValue && !int.Equals(this.DomainObject.Code, value.Value))
                {
                    string name = nameof(this.Code);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Code);
                    ChangingDomainProperty = name; this.DomainObject.Code = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Code : (int?)null; }
        }
        public string Name
        {
            get { return this.DomainObject.Name; }
        }
        string myshortname;
        public string ShortName
        {
            set
            {
                if (!this.IsReadOnly & !string.Equals(this.DomainObject.ShortName, value))
                {
                    string name = nameof(this.ShortName);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ShortName);
                    myshortname = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.ShortName = value;
                        ClearErrorMessageForProperty(name);
                        ClearErrorMessageForProperty(nameof(this.FullName));
                    }
                }
            }
            get { return this.IsEnabled ? myshortname : null; }
        }
        string myfullname;
        public string FullName
        {
            set
            {
                if (!this.IsReadOnly & !string.Equals(this.DomainObject.FullName, value))
                {
                    string name = nameof(this.FullName);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FullName);
                    myfullname = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.FullName = value;
                        ClearErrorMessageForProperty(name);
                        ClearErrorMessageForProperty(nameof(this.ShortName));
                    }
                }
            }
            get { return myfullname; }
        }
        public string Synonym
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Synonym, value)))
                {
                    string name = "Synonym";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Synonym);
                    ChangingDomainProperty = name; this.DomainObject.Synonym = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Synonym : null; }
        }
        public PriceCategory PriceCategory
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.PriceCategory, value)))
                {
                    string name = "PriceCategory";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PriceCategory);
                    ChangingDomainProperty = name; this.DomainObject.PriceCategory = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PriceCategory : null; }
        }
        public bool? RequestList
        {
            set
            {
                if(!this.IsReadOnly & value.HasValue && !bool.Equals(this.DomainObject.RequestList, value.Value))
                {
                    string name = nameof(this.RequestList);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.RequestList);
                    ChangingDomainProperty = name; this.DomainObject.RequestList = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.RequestList : (bool?)null; }
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Code):
                    this.DomainObject.Code = (int)value;
                    break;
                case nameof(this.FullName):
                    if (myfullname != this.DomainObject.FullName)
                        myfullname = this.DomainObject.FullName;
                    else
                        this.DomainObject.FullName = (string)value;
                    break;
                case nameof(this.ShortName):
                    if (myshortname != this.DomainObject.ShortName)
                        myshortname = this.DomainObject.ShortName;
                    else
                        this.DomainObject.ShortName = (string)value;
                    break;
                case nameof(this.Synonym):
                    this.DomainObject.Synonym = (string)value;
                    break;
                case nameof(this.PriceCategory):
                    this.DomainObject.PriceCategory = (PriceCategory)value;
                    break;
                case nameof(this.RequestList):
                    this.DomainObject.RequestList = (bool)value;
                    break;
            }
            return;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(this.FullName):
                    this.FullName = this.DomainObject.FullName;
                    break;
                case nameof(this.ShortName):
                    this.ShortName = this.DomainObject.ShortName;
                    break;
            }
        }
        protected override bool DirtyCheckProperty()
        {
            return !(myfullname == this.DomainObject.FullName && myshortname == this.DomainObject.ShortName);
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.FullName):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myfullname, out errmsg, out _);
                    break;
                case nameof(this.ShortName):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myshortname, out errmsg, out _);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override void InitProperties()
        {
            myshortname = this.DomainObject.ShortName;
            myfullname = this.DomainObject.FullName;
        }
    }

    public class CountrySynchronizer : lib.ModelViewCollectionsSynchronizer<Country, CountryVM>
    {
        protected override Country UnWrap(CountryVM wrap)
        {
            return wrap.DomainObject as Country;
        }
        protected override CountryVM Wrap(Country fill)
        {
            return new CountryVM(fill);
        }
    }

    public class CountriesVM : lib.ViewModelViewCommand
    {
        internal CountriesVM() : base()
        {
            mysync = new CountrySynchronizer();
            mysync.DomainCollection = CustomBrokerWpf.References.Countries;
            mydbm = new CountryDBM();
            (mydbm as CountryDBM).Collection = mysync.DomainCollection;
            this.Collection = mysync.ViewModelCollection;

            this.DeleteQuestionHeader = "Удалить страну?";

            mypricecategory = new ListCollectionView(CustomBrokerWpf.References.PriceCategories);
            mypricecategory.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        private CountrySynchronizer mysync;

        private ListCollectionView mypricecategory;
        public ListCollectionView PriceCategories
        { get { return mypricecategory; } }

        protected override void OtherViewRefresh()
        {
            CustomBrokerWpf.References.CountryViewCollector.RefreshViews();
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
        protected override void RefreshData(object parametr)
        {
            CustomBrokerWpf.References.Countries.Refresh();
            OtherViewRefresh();
        }
    }

}
