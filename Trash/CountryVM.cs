using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using KirillPolyanskiy.CustomBrokerWpf.Domain.References;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
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
