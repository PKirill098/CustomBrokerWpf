using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Linq;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Contact : lib.DomainBaseReject
    {
        public Contact(int contactid, lib.DomainObjectState dstate
            , string contacttype, string name, string surname, string thirdname
            ) : base(contactid, dstate)
        {
            mycontacttype = contacttype;
            myname = name;
            mysurname = surname;
            mythirdname = thirdname;
        }
        public Contact() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added, null, null, null,null) { }

        private string mycontacttype;
        public string ContactType
        {
            set { SetProperty<string>(ref mycontacttype, value); }
            get { return mycontacttype; }
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

        protected override void PropertiesUpdate(DomainBaseReject sample)
        {
            Contact newitem = (Contact)sample;
            this.ContactType = newitem.ContactType;
            this.Name = newitem.Name;
            this.SurName = newitem.SurName;
            this.ThirdName = newitem.ThirdName;
        }
        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
    }

    public class ContactVM : lib.ViewModelErrorNotifyItem<Contact>
    {
        public ContactVM(Contact item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { "DependancyObject"});
            DeleteRefreshProperties.AddRange(new string[] { "ContactType", "Name", "SurName", "ThirdName" });
            InitProperties();
        }
        public ContactVM() : this(new Contact()) { }

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
        { get { return this.IsEnabled ? ((this.DomainObject.Name ?? string.Empty) + (" " + this.DomainObject.SurName ?? string.Empty) + (" " + this.DomainObject.ThirdName ?? string.Empty)).TrimStart() : null; } }
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
        protected override void InitProperties() { }
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
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "DependancyObject":
                    System.Text.StringBuilder err = new System.Text.StringBuilder();
                    if (mypoints != null)
                        foreach (ContactPointVM item in mypoints.OfType<ContactPointVM>())
                            if (!item.Validate(true))
                                err.AppendLine(item.Errors);
                    if (err.Length > 0)
                    {
                        errmsg = err.ToString();
                        isvalid = false;
                    }
                    break;
            }
            if (isvalid) ClearErrorMessageForProperty(propertyname);
            else if (inform) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return false;
        }
    }

    internal class ContactSynchronizer : lib.ModelViewCollectionsSynchronizer<Contact, ContactVM>
    {
        protected override Contact UnWrap(ContactVM wrap)
        {
            return wrap.DomainObject as Contact;
        }
        protected override ContactVM Wrap(Contact fill)
        {
            return new ContactVM(fill);
        }
    }
}
