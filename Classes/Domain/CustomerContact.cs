using System;
using System.Data.SqlClient;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class CustomerContact : lib.DomainBaseReject
    {
        public CustomerContact(int contactid, lib.DomainObjectState dstate
            , string contacttype, int customerid, string name, string surname, string thirdname
            ) : base(contactid, dstate)
        {
            mycontacttype = contacttype;
            mycustomerid = customerid;
            myname = name;
            mysurname = surname;
            mythirdname = thirdname;
        }
        public CustomerContact() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added,null,0,null,null,null) { }

        private string mycontacttype;
        public string ContactType
        {
            set { SetProperty<string>(ref mycontacttype, value); }
            get { return mycontacttype; }
        }
        private int mycustomerid;
        public int CustomerId
        {
            set
            {
                SetProperty<int>(ref mycustomerid, value);
            }
            get { return mycustomerid; }
        }
        private string myname;
        public string Name
        {
            set
            {
                SetProperty<string>(ref myname, value);
            }
            get { return myname; }
        }
        private string mysurname;
        public string SurName
        {
            set { SetProperty<string>(ref mysurname, value); }
            get { return mysurname; }
        }
        private string mythirdname;
        public string ThirdName
        {
            set { SetProperty<string>(ref mythirdname, value); }
            get { return mythirdname; }
        }

        private System.Collections.ObjectModel.ObservableCollection<ContactPoint> mypoints;
        internal System.Collections.ObjectModel.ObservableCollection<ContactPoint> Points
        {
            get
            {
                if (mypoints == null)
                {
                    mypoints = new System.Collections.ObjectModel.ObservableCollection<ContactPoint>();
                    ContactPointDBM ldbm = new ContactPointDBM();
                    ldbm.ItemId = this.Id;
                    ldbm.Collection = mypoints;
                    ldbm.Fill();
                }
                return mypoints;
            }
        }
        internal bool PointsIsNull
        { get { return mypoints == null; } }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            CustomerContact newitem = (CustomerContact)sample;
            if (!this.HasPropertyOutdatedValue("ContactType")) this.ContactType = newitem.ContactType;
            if (!this.HasPropertyOutdatedValue("Name")) this.Name = newitem.Name;
            if (!this.HasPropertyOutdatedValue("SurName")) this.SurName = newitem.SurName;
            if (!this.HasPropertyOutdatedValue("ThirdName")) this.ThirdName = newitem.ThirdName;
        }
    }

    public class CustomerContactDBM : lib.DBManagerId<CustomerContact>
    {
        public CustomerContactDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "dbo.CustomerContact_sp";
            InsertCommandText = "dbo.CustomerContactAdd_sp";
            UpdateCommandText = "dbo.CustomerContactUpd_sp";
            DeleteCommandText = "dbo.CustomerContactDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int)
            };
            SqlParameter paridout = new SqlParameter("@ContactID", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@ContactID", System.Data.SqlDbType.Int);
            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@customerID", System.Data.SqlDbType.Int)};
            myupdateparams = new SqlParameter[] {
                parid
                ,new SqlParameter("@nametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@surnametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@thirdnametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@typetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@old", 0)
            };
            myinsertupdateparams = new SqlParameter[]
            {new SqlParameter("@ContactName", System.Data.SqlDbType.NVarChar,100),new SqlParameter("@surname", System.Data.SqlDbType.NVarChar,25),new SqlParameter("@thirdname", System.Data.SqlDbType.NVarChar,25),new SqlParameter("@ContactType", System.Data.SqlDbType.NVarChar,50)  };
            mydeleteparams = new SqlParameter[] { parid };
            mypdbm = new ContactPointDBM(); mypdbm.Command = new SqlCommand();
        }

        private ContactPointDBM mypdbm;

        protected override CustomerContact CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new CustomerContact(reader.GetInt32(reader.GetOrdinal("ContactId")), lib.DomainObjectState.Unchanged, reader.IsDBNull(reader.GetOrdinal("contactType")) ? null : reader.GetString(reader.GetOrdinal("contactType")), reader.GetInt32(0), reader.IsDBNull(3) ? null : reader.GetString(3), reader.IsDBNull(4) ? null : reader.GetString(4), reader.IsDBNull(5) ? null : reader.GetString(5));
        }
        protected override void GetOutputParametersValue(CustomerContact item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(CustomerContact item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(CustomerContact item)
        {
            bool issuccess = true;
            if (!item.PointsIsNull)
            {
                mypdbm.Errors.Clear();
                mypdbm.ItemId = item.Id;
                mypdbm.Collection = item.Points;
                if (!mypdbm.SaveCollectionChanches())
                {
                    issuccess = false;
                    foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
                }
            }

            return issuccess;
        }
        protected override bool SaveIncludedObject(CustomerContact item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            mypdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetParametersValue(CustomerContact item)
        {
            myinsertparams[1].Value = this.ItemId;
            myupdateparams[0].Value = item.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue("Name");
            myupdateparams[2].Value = item.HasPropertyOutdatedValue("SurName");
            myupdateparams[3].Value = item.HasPropertyOutdatedValue("ThirdName");
            myupdateparams[4].Value = item.HasPropertyOutdatedValue("ContactType");
            myinsertupdateparams[0].Value = item.Name;
            myinsertupdateparams[1].Value = item.SurName;
            myinsertupdateparams[2].Value = item.ThirdName;
            myinsertupdateparams[3].Value = item.ContactType;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override void CancelLoad()
        { }
    }

    public class CustomerContactVM : lib.ViewModelErrorNotifyItem<CustomerContact>
    {
        public CustomerContactVM(CustomerContact item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { });
            DeleteRefreshProperties.AddRange(new string[] { "ContactType", "Name", "SurName", "ThirdName" });
            InitProperties();
        }
        public CustomerContactVM() : this(new CustomerContact()) { }

        public string ContactType
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.ContactType, value)))
                {
                    string name = "ContactType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ContactType);
                    ChangingDomainProperty = name; this.DomainObject.ContactType = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ContactType : null; }
        }
        public string Name
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Name, value)))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Name);
                    ChangingDomainProperty = name; this.DomainObject.Name = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Name : null; }
        }
        public string SurName
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.SurName, value)))
                {
                    string name = "SurName";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SurName);
                    ChangingDomainProperty = name; this.DomainObject.SurName = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.SurName : null; }
        }
        public string ThirdName
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.ThirdName, value)))
                {
                    string name = "ThirdName";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ThirdName);
                    ChangingDomainProperty = name; this.DomainObject.ThirdName = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ThirdName : null; }
        }
        public string FullName
        { get { return this.IsEnabled ? ((this.DomainObject.Name??string.Empty) + (" "+ this.DomainObject.SurName ?? string.Empty) + (" " + this.DomainObject.ThirdName ?? string.Empty)).TrimStart() : null; } }
        private ContactPointSynchronizer mypsync;
        private ListCollectionView mypoints;
        public ListCollectionView Points
        {
            get
            {
                if (mypoints == null)
                {
                    if (mypsync == null)
                    {
                        mypsync = new ContactPointSynchronizer();
                        mypsync.DomainCollection = this.DomainObject.Points;
                    }
                    mypoints = new ListCollectionView(mypsync.ViewModelCollection);
                    mypoints.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    mypoints.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
                }
                return mypoints;
            }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
        }
        protected override void InitProperties() {}
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "ContactType":
                    this.DomainObject.ContactType = (string)value;
                    break;
                case "Name":
                    this.DomainObject.Name = (string)value;
                    break;
                case "SurName":
                    this.DomainObject.SurName = (string)value;
                    break;
                case "ThirdName":
                    this.DomainObject.ThirdName = (string)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    if (this.mypoints != null)
                    {
                        ContactPointVM[] lremoved = new ContactPointVM[this.DomainObject.Points.Count];
                        foreach (ContactPointVM litem in this.mypsync.ViewModelCollection)
                        {
                            if (litem.DomainState == lib.DomainObjectState.Added)
                            {
                                lremoved[i] = litem;
                                i++;
                            }
                            else
                            {
                                this.mypoints.EditItem(litem);
                                litem.RejectChanges();
                                this.mypoints.CommitEdit();
                            }
                        }
                        foreach (ContactPointVM litem in lremoved)
                            if (litem != null) this.DomainObject.Points.Remove(litem.DomainObject);
                    }
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            return true;
        }
        protected override bool DirtyCheckProperty()
        {
            return false;
        }
    }

    internal class CustomerContactSynchronizer : lib.ModelViewCollectionsSynchronizer<CustomerContact, CustomerContactVM>
    {
        protected override CustomerContact UnWrap(CustomerContactVM wrap)
        {
            return wrap.DomainObject as CustomerContact;
        }
        protected override CustomerContactVM Wrap(CustomerContact fill)
        {
            return new CustomerContactVM(fill);
        }
    }
}
