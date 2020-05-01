using KirillPolyanskiy.CustomBrokerWpf.Classes;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Data;
using System.Data.SqlClient;

namespace KirillPolyanskiy.CustomBrokerWpf.Domain.References
{
    public class Contractor: Classes.Domain.DomainBaseClass
    {
        private bool mysincref;

        private int myid;
        public int Id { internal set { myid = value; } get { return myid; } }
        private string myname;
        public string Name
        {
            set
            {
                if (!string.Equals(myname, value))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myname);
                    myname = value;
                    if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                    if (mysincref)//синхронизация справочника
                    {
                        Contractor refcon = CustomBrokerWpf.References.Contractors.FindFirstItem("Id", myid);
                        if (refcon != null)
                        {
                            refcon.Name = myname;
                            CustomBrokerWpf.References.Contractors.Order();
                            CustomBrokerWpf.References.Contractors.OnResetCollectionChanged();
                        }
                    }
                }
            }
            get { return myname; }
        }

        public Contractor() :this(0,string.Empty,DomainObjectState.Added,true){ } //для DataGrid
        public Contractor(int id,string name, DomainObjectState initstate, bool syncref):base()
        {
            myid = id;
            this.Name = name;
            mystate = initstate; // после инициализации св-в
            mysincref = syncref;
        }

        protected override void RejectProperty(string property, object value)
        { if(property=="Name") this.Name= (string)value; }

    }

    public class ContractorList : ListNotifyChanged<Contractor>, IReference<Contractor>
    {
        internal ContractorList()
            : base()
        {
            this.Fill();
        }

        private SqlDataReader GetReader(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT id,name FROM dbo.Contractor_tb";
            cmd.Connection = conn;
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
        }
        private void Fill()
        {
            using (SqlConnection conn = new SqlConnection(CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = GetReader(conn);
                while (reader.Read())
                {
                    this.Add(new Contractor(reader.GetInt32(0),reader.GetString(1),DomainObjectState.Unchanged, false));
                }
                reader.Close();
            }
            if(!(this.Count>0 && this[0].Name.Equals(string.Empty))) this.Insert(0,new Contractor(0,string.Empty, DomainObjectState.Unchanged, false));
        }
        public void Refresh()
        {
            using (SqlConnection conn = new SqlConnection(CustomBrokerWpf.References.ConnectionString))
            {
                int compare,startIndex;
                startIndex = 1; //первый пустой
                SqlDataReader reader = GetReader(conn);
                while (reader.Read())
                {
                    if (reader.IsDBNull(0)) continue;
                    compare = -1;
                    Contractor newRecipient = new Contractor(reader.GetInt32(0), reader.GetString(1), DomainObjectState.Unchanged, false);
                    for (int i = startIndex; i < this.Count; i++)
                    {
                        startIndex = i+1;
                        Contractor oldRecipient = this[i];
                        compare=oldRecipient.Name.CompareTo(newRecipient.Name);
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
                            this.Insert(i, newRecipient);
                            break;
                        }
                    }
                    if(compare<0) this.Add(newRecipient);
                }
                reader.Close();
                if(this.Count > startIndex) this.RemoveRange(startIndex, this.Count - startIndex);
            }
            this.OnResetCollectionChanged();
        }

        public Contractor FindFirstItem(string propertyName, object value)
        {
            Contractor item=null;
            switch (propertyName)
            {
                case "Id":
                    item = this.Find(x => x.Id.Equals(value));
                    break;
                case "Name":
                    item = this.Find(x => x.Name.Equals(value));
                    break;
            }
            return item;
        }
        internal void Order()
        {
            this.Sort((Contractor item1, Contractor item2) => { return string.Compare(item1.Name, item2.Name); });
        }

        public void AddOrder(System.Collections.Generic.IEnumerable<Contractor> range)
        {
            int compare;
            foreach (Contractor newitem in range)
            {
                compare = -1;
                for (int i = 0; i < this.Count; i++)
                {
                    Contractor olditem = this[i];
                    compare = olditem.Name.CompareTo(newitem.Name);
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
        }
        private bool FindRecipient(Contractor recipient)
        {
            return false;
        }
    }

    public class ContractorListVM: Classes.ViewModelBaseList
    {
        private ExceptionHandler myexhandler;
        private System.Collections.ObjectModel.ObservableCollection<Contractor> mycontactors;

        internal ContractorListVM():base()
        {
            myexhandler = new ExceptionHandler();
            mycontactors = new System.Collections.ObjectModel.ObservableCollection<Contractor>();
            myview = new System.Windows.Data.ListCollectionView(mycontactors);
            myview.Filter= delegate (object item) { return (item as Domain.References.Contractor).DomainState != DomainObjectState.Deleted; };
            Fill();
            mycontactors.CollectionChanged += SyncReference;// не слушаем загрузку
        }

        //public System.Collections.ObjectModel.ObservableCollection<Contractor> Contractors
        //{ get { return mycontactors; } }
        public System.Windows.Data.ListCollectionView Contractors
        { get { return myview; } }
        internal override bool SaveDataChanges()
        {
            bool isSuccess = false;
            myexhandler.Title = "Сохранение изменений";
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(CustomBrokerWpf.References.ConnectionString))
            {
                bool refneedrefresh = false;
                SqlCommand comd = new SqlCommand();
                comd.Connection = connection;
                comd.CommandType = System.Data.CommandType.Text;
                SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);
                parid.Direction = System.Data.ParameterDirection.InputOutput;
                SqlParameter parname = new SqlParameter("@name", System.Data.SqlDbType.NVarChar,30);
                comd.Parameters.Add(parid); comd.Parameters.Add(parname);
                try
                {
                connection.Open();
                    foreach (Contractor item in mycontactors)
                    {
                        if (item.DomainState != DomainObjectState.Unchanged)
                        {
                            parname.Value = item.Name;
                            parid.Value = item.Id;
                            switch (item.DomainState)
                            {
                                case DomainObjectState.Added:
                                    comd.CommandText = "INSERT INTO Contractor_tb (name) VALUES (@name); SET @id=SCOPE_IDENTITY();";
                                    refneedrefresh = true;
                                    break;
                                case DomainObjectState.Modified:
                                    comd.CommandText = "UPDATE Contractor_tb SET name=@name WHERE id=@id;";
                                    break;
                                case DomainObjectState.Deleted:
                                    comd.CommandText = "DELETE FROM Contractor_tb WHERE id=@id;";
                                    refneedrefresh = true;
                                    break;
                            }
                            comd.ExecuteNonQuery();
                            if(item.DomainState== DomainObjectState.Added) item.Id = (int)parid.Value;
                            item.AcceptChanches();
                        }
                    }
                    isSuccess = true;
                    if(refneedrefresh) CustomBrokerWpf.References.Contractors.Refresh();
                }
                catch (Exception ex)
                { myexhandler.Handle(ex); }
                finally
                { if (connection.State == System.Data.ConnectionState.Open) connection.Close(); }
            }
            return isSuccess;
        }
        protected override bool CanSaveDataChanges() { return false; }
        protected override void RefreshData() { }
        protected override void RejectChanges() { }
        protected override bool CanRejectChanges() { return false; }
        protected override void DeleteData(object parametr) {}
        protected override bool CanDeleteData()
        {
            return false;
        }

        public void BindingDelete(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            (myview.CurrentItem as Contractor).DomainState = DomainObjectState.Deleted;
            //((sender as System.Windows.Controls.DataGrid).CurrentItem as Contractor).DomainState = DomainObjectState.Deleted;
            myview.Refresh();
        }

        private SqlDataReader GetReader(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT id,name FROM dbo.Contractor_tb";
            cmd.Connection = conn;
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
        }
        private void Fill()
        {
            using (SqlConnection conn = new SqlConnection(CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = GetReader(conn);
                while (reader.Read())
                {
                    mycontactors.Add(new Contractor(reader.GetInt32(0), reader.GetString(1), DomainObjectState.Unchanged, true));
                }
                reader.Close();
            }
        }
        private void SyncReference(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (Contractor olditem in e.OldItems)
                    {
                        Contractor refcnt = CustomBrokerWpf.References.Contractors.FindFirstItem("Id", olditem.Id);
                        if (refcnt!=null)
                            CustomBrokerWpf.References.Contractors.Remove(refcnt);
                    }
                    break;
            }
        }
    }
}
