using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Address : lib.DomainBaseReject
    {
        public Address(int id, lib.DomainObjectState dstate
                        , string addressdescription, byte addresstypeid, string locality, string town
                        ) : base(id,dstate)
        {
            myaddressdescription = addressdescription;
            myaddresstypeid = addresstypeid;
            mylocality = locality;
            mytown = town;
        }
        public Address() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added, null, 0, null, null) { }

        private string myaddressdescription;
        public string AddressDescription
        {
            set { SetProperty<string>(ref myaddressdescription, value, () => { this.PropertyChangedNotification("FullAddressDescription"); }); }
            get { return myaddressdescription; }
        }
        private byte myaddresstypeid;
        public byte AddressTypeID
        {
            set
            {
                SetProperty<byte>(ref myaddresstypeid, value);
            }
            get { return myaddresstypeid; }
        }
        private string mylocality;
        public string Locality
        {
            set { SetProperty<string>(ref mylocality, value, () => { this.PropertyChangedNotification("FullAddress"); this.PropertyChangedNotification("FullAddressDescription"); }); }
            get { return mylocality; }
        }
        private string mytown;
        public string Town
        {
            set { SetProperty<string>(ref mytown, value, () => { this.PropertyChangedNotification("FullAddress"); this.PropertyChangedNotification("FullAddressDescription"); }); }
            get { return mytown; }
        }
        public string FullAddress
        { get { return (mytown ?? string.Empty) + ((string.IsNullOrWhiteSpace(mytown) | string.IsNullOrWhiteSpace(mylocality)) ? string.Empty : ", ") + (mylocality ?? string.Empty); } }
        public string FullAddressDescription
        { get { return (FullAddress ?? string.Empty) + ("( " + myaddressdescription + " )") ?? string.Empty; } }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(DomainBaseUpdate sample)
        {
            Address newitem = (Address)sample;
            this.AddressDescription = newitem.AddressDescription;
            this.AddressTypeID = newitem.AddressTypeID;
            this.Locality = newitem.Locality;
            this.Town = newitem.Town;
        }

    }

    public class AddressVM : lib.ViewModelErrorNotifyItem<Address>
    {
        public AddressVM(Address item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.AddressTypeID) });
            DeleteRefreshProperties.AddRange(new string[] { nameof(this.AddressTypeID) });
            InitProperties();
        }
        public AddressVM() : this(new Address()) { }

        public string AddressDescription
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.AddressDescription, value)))
                {
                    string name = "AddressDescription";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.AddressDescription);
                    ChangingDomainProperty = name; this.DomainObject.AddressDescription = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.AddressDescription : null; }
        }
        private byte? myaddresstypeid;
        public byte? AddressTypeID
        {
            set
            {
                if (!this.IsReadOnly & value.HasValue && myaddresstypeid != value)
                {
                    string name = "AddressTypeID";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.AddressTypeID);
                    myaddresstypeid = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.AddressTypeID = value.Value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? myaddresstypeid : null; }
        }
        public string Locality
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Locality, value)))
                {
                    string name = "Locality";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Locality);
                    ChangingDomainProperty = name; this.DomainObject.Locality = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Locality : null; }
        }
        public string Town
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Town, value)))
                {
                    string name = "Town";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Town);
                    ChangingDomainProperty = name; this.DomainObject.Town = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Town : null; }
        }
        public string FullAddress
        {
            get { return this.IsEnabled ? this.DomainObject.FullAddress : null; }
        }
        public string FullAddressDescription
        {
            get { return this.IsEnabled ? this.DomainObject.FullAddressDescription : null; }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "AddressTypeID":
                    this.AddressTypeID = this.DomainObject.AddressTypeID;
                    break;
            }
        }
        protected override void InitProperties() { myaddresstypeid = this.DomainObject.AddressTypeID; }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "AddressDescription":
                    this.DomainObject.AddressDescription = (string)value;
                    break;
                case "AddressTypeID":
                    if (myaddresstypeid != this.DomainObject.AddressTypeID)
                        myaddresstypeid = this.DomainObject.AddressTypeID;
                    else
                        this.AddressTypeID = (byte?)value;
                    break;
                case "Locality":
                    this.DomainObject.Locality = (string)value;
                    break;
                case "Town":
                    this.DomainObject.Town = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            //string errmsg = null;
            //switch (propertyname)
            //{
            //    case nameof(this.AddressTypeID):
            //        if (!this.AddressTypeID.HasValue || this.AddressTypeID == 0)
            //        {
            //            errmsg = "Необходимо указать вид адреса!";
            //            isvalid = false;
            //        }
            //        break;
            //}
            //if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myaddresstypeid != this.DomainObject.AddressTypeID;
        }
    }

}
