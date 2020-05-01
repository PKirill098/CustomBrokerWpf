using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using KirillPolyanskiy.CustomBrokerWpf.Domain.References;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    public class CountryVM : ViewModelBaseItem
    {
        Country mycountry;
        public CountryVM() : this(new Country()) { }
        internal CountryVM(Country country) : base(country)
        {
            mycountry = country;
            myshortname = mycountry.ShortName;
            myfullname = mycountry.FullName;
            if(mycountry.PriceCategory.HasValue) mypricecategory = References.PriceCategories.FindFirstItem("Id", mycountry.PriceCategory.Value);
        }

        public int Code
        {
            set
            {
                if (!int.Equals(mycountry.Code, value))
                {
                    string name = "Code";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycountry.Code);
                    mycountry.Code = value;
                }
            }
            get { return mycountry.Code; }
        }
        public string Name
        {
            get { return mycountry.Name; }
        }
        string myshortname;
        public string ShortName
        {
            set
            {
                if (!string.Equals(mycountry.ShortName, value))
                {
                    string name = "ShortName";
                    myshortname = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycountry.ShortName);
                    if (string.IsNullOrEmpty(value) & string.IsNullOrEmpty(mycountry.FullName))
                    {
                        AddErrorMessageForProperty(name, "Необходимо указать наименование страны.");
                        AddErrorMessageForProperty("FullName", "Необходимо указать наименование страны.");
                        return;
                    }
                    ClearErrorMessageForProperty(name);
                    ClearErrorMessageForProperty("FullName");
                    mycountry.ShortName = value;
                    mycountry.FullName = myfullname; //если не сбросилось в домен из-за перекрестной ошибки
                }
            }
            get { return myshortname; }
        }
        string myfullname;
        public string FullName
        {
            set
            {
                if (!string.Equals(myfullname, value))
                {
                    string name = "FullName";
                    myfullname = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycountry.FullName);
                    if (string.IsNullOrEmpty(value) & string.IsNullOrEmpty(mycountry.ShortName))
                    {
                        AddErrorMessageForProperty(name, "Необходимо указать наименование страны.");
                        AddErrorMessageForProperty("ShortName", "Необходимо указать наименование страны.");
                        return;
                    }
                    mycountry.FullName = value;
                    mycountry.ShortName = myshortname;
                    ClearErrorMessageForProperty(name);
                    ClearErrorMessageForProperty("ShortName");
                }
            }
            get { return myfullname; }
        }
        public string Synonym
        {
            set
            {
                if (!string.Equals(mycountry.Synonym, value))
                {
                    string name = "Synonym";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycountry.Synonym);
                    mycountry.Synonym = value;
                }
            }
            get { return mycountry.Synonym; }
        }
        private Domain.References.PriceCategory mypricecategory;
        public Domain.References.PriceCategory PriceCategory
        {
            set
            {
                if (mypricecategory != value)
                {
                    string name = "PriceCategory";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycountry.PriceCategory);
                    mypricecategory = value;
                    mycountry.PriceCategory = value?.Id;
                    PropertyChangedNotification(name);
                }
            }
            get { return mypricecategory; }
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Code":
                    mycountry.Code = (int)value;
                    break;
                case "ShortName":
                    this.ShortName = (string)value;
                    break;
                case "FullName":
                    this.FullName = (string)value;
                    break;
                case "Synonym":
                    this.Synonym = (string)value;
                    break;
            }
            return;
        }
    }

    public class CountriesVM : ViewModelBaseList
    {
        private ExceptionHandler myexhandler;
        ObservableCollection<CountryVM> mycountries;
        public ListCollectionView Countries { get { return myview; } }
        internal CountriesVM() : base()
        {
            myexhandler = new ExceptionHandler();
            mycountries = new ObservableCollection<CountryVM>();
            foreach (Country item in References.Countries)
            {
                CountryVM newvm = new CountryVM(item);
                newvm.ErrorsChanged += base.ItemErrorsChanged;
                mycountries.Add(newvm);
            }
            myview = new System.Windows.Data.ListCollectionView(mycountries);
            myview.Filter = delegate (object item) { return ((item as CountryVM).DomainObject as DomainBaseClass).DomainState != DomainObjectState.Deleted; };
            mycountries.CollectionChanged += Mycountries_CollectionChanged;
            mypricecategory = new ListCollectionView(References.PriceCategories);
            mypricecategory.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        private ListCollectionView mypricecategory;
        public ListCollectionView PriceCategories
        { get { return mypricecategory; } }

        private void Mycountries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (CountryVM item in e.NewItems)
                    References.Countries.Add(item.DomainObject as Country);
            }
        }

        protected override bool CanRejectChanges()
        {
            return true;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void RefreshData()
        {
            References.Countries.Refresh();
        }
        protected override void RejectChanges()
        {
            ListCollectionView view = CollectionViewSource.GetDefaultView(mycountries) as ListCollectionView;
            if (view.IsAddingNew) view.CancelNew();
            if (view.CanCancelEdit) view.CancelEdit();
            for (int i = mycountries.Count - 1; i > -1; i--)
            {
                CountryVM item = mycountries[i];
                if (item.DomainObject.DomainState == DomainObjectState.Added)
                {
                    this.mycountries.Remove(item);
                    References.Countries.Remove(item.DomainObject as Country);
                }
                else
                {
                    item.RejectChanges();
                }
            }
            if (!(view.IsEditingItem | view.IsAddingNew)) view.Refresh();
        }
        internal override bool SaveDataChanges()
        {
            bool isSuccess = false;
            myexhandler.Title = "Сохранение изменений";
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;

                SqlParameter parcodeold = new SqlParameter("@codeold", System.Data.SqlDbType.Int);
                SqlParameter parcode = new SqlParameter("@code", System.Data.SqlDbType.Int);
                SqlParameter parshortname = new SqlParameter("@shortname", System.Data.SqlDbType.NVarChar, 30);
                SqlParameter parfullname = new SqlParameter("@fullname", System.Data.SqlDbType.NVarChar, 100);
                SqlParameter parsynonym = new SqlParameter("@synonym", System.Data.SqlDbType.NVarChar, 200);
                SqlParameter parpricecategory = new SqlParameter("@pricecategory", System.Data.SqlDbType.Int);
                SqlParameter[] pars = { parcodeold, parcode, parshortname, parfullname, parsynonym, parpricecategory };
                try
                {
                    connection.Open();
                    foreach (CountryVM item in mycountries)
                    {
                        if (item.DomainObject.DomainState != DomainObjectState.Unchanged)
                        {
                            cmd.Parameters.Clear();

                            parcodeold.Value = item.DomainObject.HasPropertyOutdatedValue("code") ? item.DomainObject.GetPropertyOutdatedValue("code") : item.Code;
                            parcode.Value = item.Code;
                            parshortname.Value = string.IsNullOrEmpty(item.ShortName) ? (object)DBNull.Value : item.ShortName;
                            parfullname.Value = string.IsNullOrEmpty(item.FullName) ? (object)DBNull.Value : item.FullName;
                            parsynonym.Value = string.IsNullOrEmpty(item.Synonym) ? (object)DBNull.Value : item.Synonym;
                            parpricecategory.Value = item.PriceCategory == null ? (object)DBNull.Value : item.PriceCategory.Id;
                            switch (item.DomainObject.DomainState)
                            {
                                case DomainObjectState.Added:
                                    cmd.CommandText = "INSERT INTO [dbo].[Country_tb] (code,shortname,fullname,synonym,pricecategory) VALUES (@code,@shortname,@fullname,@synonym,@pricecategory)";
                                    cmd.Parameters.AddRange(pars);
                                    break;
                                case DomainObjectState.Modified:
                                    cmd.CommandText = "UPDATE [dbo].[Country_tb] SET code=@code,shortname=@shortname,fullname=@fullname,synonym=@synonym,pricecategory=@pricecategory WHERE code=@codeold";
                                    cmd.Parameters.AddRange(pars);
                                    break;
                                case DomainObjectState.Deleted:
                                    cmd.CommandText = "DELETE FROM [dbo].[Country_tb] WHERE code=@codeold";
                                    cmd.Parameters.Add(parcodeold);
                                    break;
                            }
                            cmd.ExecuteNonQuery();
                            if (item.DomainObject.DomainState == DomainObjectState.Deleted)
                            {
                                References.Countries.Remove(item.DomainObject as Country);
                                mycountries.Remove(item);
                            }
                            else
                                item.DomainObject.AcceptChanches();
                        }
                    }

                    isSuccess = true;
                }
                catch (Exception ex)
                { myexhandler.Handle(ex); myexhandler.ShowMessage(); }
                finally
                { if (connection.State == System.Data.ConnectionState.Open) connection.Close(); }
            }
            return isSuccess;
        }
        protected override void DeleteData(object parametr)
        {
            if (parametr != null && MessageBox.Show("Удалить страну?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (this.myview.IsAddingNew) this.myview.CancelNew();
                if (this.myview.CanCancelEdit) this.myview.CancelEdit();
                this.myview.EditItem(parametr);
                (parametr as CountryVM).DomainObject.DomainState = DomainObjectState.Deleted;
                this.myview.CommitEdit();
            }
        }
        protected override bool CanDeleteData()
        {
            return true;
        }
    }
}
