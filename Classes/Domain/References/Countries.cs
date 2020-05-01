using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace KirillPolyanskiy.CustomBrokerWpf.Domain.References
{
    public class Country : DomainBaseClass
    {

        public Country() : this(0, string.Empty, string.Empty, string.Empty, null, DomainObjectState.Added) { }
        public Country(int code, string shortname, string fullname, string synonym, int? pricecategory, DomainObjectState initstate) : base()
        {
            mycode = code;
            myshortname = shortname;
            myfullname = fullname;
            mysynonym = synonym;
            mystate = initstate;
            mypricecategory = pricecategory;
        }

        int mycode;
        public int Code
        {
            set
            {
                if (!int.Equals(mycode, value))
                {
                    string name = "Code";
                    //myhaserror=true; ErrorsChangedNotification(name);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycode);
                    mycode = value;
                    if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
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
                if (!string.Equals(myshortname, value))
                {
                    string name = "ShortName";
                    if (string.IsNullOrEmpty(value))
                    {
                        AddErrorMessageForProperty(name, "Необходимо указать наименование страны.");
                        ErrorsChangedNotification(name);
                        return;
                    }
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myshortname);
                    myshortname = value;
                    if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
                    PropertyChangedNotification(name);
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
                    if (string.IsNullOrEmpty(value))
                    {
                        AddErrorMessageForProperty(name, "Необходимо указать наименование страны.");
                        ErrorsChangedNotification(name);
                        return;
                    }
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myfullname);
                    myfullname = value;
                    if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myfullname; }
        }
        string mysynonym;
        public string Synonym
        {
            set
            {
                if (!string.Equals(mysynonym, value))
                {
                    string name = "Synonym";
                    if (string.IsNullOrEmpty(value))
                    {
                        AddErrorMessageForProperty(name, "Необходимо указать наименование страны.");
                        ErrorsChangedNotification(name);
                        return;
                    }
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mysynonym);
                    mysynonym = value;
                    if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mysynonym; }
        }
        private int? mypricecategory;
        public int? PriceCategory
        {
            set
            {
                if (mypricecategory.HasValue != value.HasValue || (value.HasValue && mypricecategory != value))
                {
                    string name = "PriceCategory";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mypricecategory);
                    mypricecategory = value;
                    if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
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
            }
            return;
        }
    }

    public class CountryList: ObservableCollection<Country>, IReference<Country>
    {
        public CountryList():base()
        {
            myisnotifycollectionchanged = true;
            Fill();
        }

        private bool myisnotifycollectionchanged;
        public bool IsNotifyCollectionChanged { set { myisnotifycollectionchanged = value; } get{ return myisnotifycollectionchanged; } }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if(myisnotifycollectionchanged)
                base.OnCollectionChanged(e);
        }

        private SqlDataReader GetReader(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT code,shortname,fullname,synonym,pricecategory FROM dbo.Country_tb ORDER BY code";
            cmd.Connection = conn;
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
        }
        private void Fill()
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = GetReader(conn);
                while (reader.Read())
                {
                    this.Add(new Country(reader.GetInt32(0), reader.IsDBNull(1)?string.Empty:reader.GetString(1), reader.IsDBNull(2) ? string.Empty : reader.GetString(2), reader.IsDBNull(3) ? string.Empty : reader.GetString(3), reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4), DomainObjectState.Unchanged));
                }
                reader.Close();
            }
        }
        public void Refresh()
        {
            using (BlockReentrancy())
            {
                myisnotifycollectionchanged = false;
                using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                {
                    int compare, startIndex;
                    startIndex = 0;
                    SqlDataReader reader = GetReader(conn);
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0)) continue;
                        compare = -1;
                        Country newitem = new Country(reader.GetInt32(0), reader.IsDBNull(1) ? string.Empty : reader.GetString(1), reader.IsDBNull(2) ? string.Empty : reader.GetString(2), reader.IsDBNull(3) ? string.Empty : reader.GetString(3), reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4), DomainObjectState.Unchanged);
                        for (int i = startIndex; i < this.Count; i++)
                        {
                            startIndex = i + 1;
                            Country olditem = this[i];
                            compare = olditem.Code.CompareTo(newitem.Code);
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
                    reader.Close();
                    if (this.Count > startIndex) for (int i = startIndex; i < this.Count; i++) this.RemoveAt(i);
                }
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
}
