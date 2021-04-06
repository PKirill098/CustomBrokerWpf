using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using KirillPolyanskiy.DataModelClassLibrary.Interfaces;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class DeliveryCarry : lib.DomainBaseStamp
    {
        public DeliveryCarry(int id, long stamp, lib.DomainObjectState domainstate
            , Request request, DeliveryCar car1, DeliveryCar car2, DeliveryCar car3, string address, string note, DateTime? shipmentdate, lib.ReferenceSimpleItem shipmenttype
            ) : base(id, stamp, null, null, domainstate)
        {
            myrequest = request;
            mycar1 = car1; mycar2 = car2; mycar3 = car3;
            myaddress = address;
            mynote = note;
            myshipmentdate = shipmentdate;
            myshipmenttype = shipmenttype;
            if (mycar1 != null) mycar1.PropertyChanged += Car1_PropertyChanged;
            if (mycar2 != null) mycar2.PropertyChanged += Car2_PropertyChanged;
            if (mycar3 != null) mycar3.PropertyChanged += Car3_PropertyChanged;
        }

        private string myaddress;
        public string Address
        {
            set { SetProperty<string>(ref myaddress, value); }
            get { return myaddress; }
        }
        private DeliveryCar mycar1;
        public DeliveryCar Car1
        {
            set
            {
                DeliveryCar carold = mycar1;
                SetProperty<DeliveryCar>(ref mycar1, value, () => { if (value != null) { value.Carry.Add(this); value.PropertyChanged += Car1_PropertyChanged; } if (carold != null) { carold.Carry.Remove(this); carold.PropertyChanged -= Car1_PropertyChanged; } PropertyChangedNotification("Car1Cost"); PropertyChangedNotification("TotalCost"); });
            }
            get { return mycar1; }
        }
        public decimal? Car1Cost
        { get { return myrequest.OfficialWeight * mycar1?.Price; } }
        private DeliveryCar mycar2;
        public DeliveryCar Car2
        {
            set
            {
                DeliveryCar carold = mycar2;
                SetProperty<DeliveryCar>(ref mycar2, value, () => { if (value != null) { value.Carry.Add(this); value.PropertyChanged += Car2_PropertyChanged; } if (carold != null) { carold.Carry.Remove(this); carold.PropertyChanged -= Car2_PropertyChanged; } PropertyChangedNotification("Car2Cost"); PropertyChangedNotification("TotalCost"); });
            }
            get { return mycar2; }
        }
        public decimal? Car2Cost
        { get { return myrequest.OfficialWeight * mycar2?.Price; } }
        private DeliveryCar mycar3;
        public DeliveryCar Car3
        {
            set
            {
                DeliveryCar carold = mycar3;
                SetProperty<DeliveryCar>(ref mycar3, value, () => { if (value != null) { value.Carry.Add(this); value.PropertyChanged += Car3_PropertyChanged; } if (carold != null) { carold.Carry.Remove(this); carold.PropertyChanged -= Car3_PropertyChanged; } PropertyChangedNotification("Car3Cost"); PropertyChangedNotification("TotalCost"); });
            }
            get { return mycar3; }
        }
        public decimal? Car3Cost
        { get { return myrequest.OfficialWeight * mycar3?.Price; } }
        private string mynote;
        public string Note
        {
            set { SetProperty<string>(ref mynote, value); }
            get { return mynote; }
        }
        private Request myrequest;
        public Request Request
        { get { return myrequest; } }
        private DateTime? myshipmentdate;
        public DateTime? ShipmentDate
        {
            set { SetProperty<DateTime?>(ref myshipmentdate, value); }
            get { return myshipmentdate; }
        }
        private lib.ReferenceSimpleItem myshipmenttype;
        public lib.ReferenceSimpleItem ShipmentType
        {
            set { SetProperty<lib.ReferenceSimpleItem>(ref myshipmenttype, value); }
            get { return myshipmenttype; }
        }
        public decimal? TotalCost
        { get { return (this.Car1Cost ?? 0M) + (this.Car2Cost ?? 0M) + (this.Car3Cost ?? 0M); } }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            DeliveryCarry newitem = (DeliveryCarry)sample;
            this.Address = newitem.Address;
            this.Car1 = newitem.Car1;
            this.Car2 = newitem.Car2;
            this.Car3 = newitem.Car3;
            this.Note = newitem.Note;
            this.ShipmentDate = newitem.ShipmentDate;
            this.ShipmentType = newitem.ShipmentType;
        }
        private void Car1_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Price") CarPriceChanged("1");
        }
        private void Car2_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Price") CarPriceChanged("2");
        }
        private void Car3_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Price") CarPriceChanged("3");
        }
        private void CarPriceChanged(string n)
        { PropertyChangedNotification("Car" + n + "Cost"); PropertyChangedNotification("TotalCost"); }
    }

    internal class DeliveryCarryStore : lib.DomainStorageLoad<DeliveryCarry, DeliveryCarryDBM>
    {
        public DeliveryCarryStore(DeliveryCarryDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(DeliveryCarry olditem, DeliveryCarry newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class DeliveryCarryDBM : lib.DBManagerStamp<DeliveryCarry>
    {
        public DeliveryCarryDBM()
        {
            this.NeedAddConnection = true;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "delivery.DeliveryCarry_sp";
            UpdateCommandText = "delivery.DeliveryCarryUpd_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@carid", System.Data.SqlDbType.Int),
                new SqlParameter("@all", System.Data.SqlDbType.Bit),
                new SqlParameter("@filterid", System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@requestid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@c1", System.Data.SqlDbType.Int)
                ,new SqlParameter("@c1true", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@c2", System.Data.SqlDbType.Int)
                ,new SqlParameter("@c2true", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@c3", System.Data.SqlDbType.Int)
                ,new SqlParameter("@c3true", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@shipmentdate", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@shipmentdatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@shipmenttype", System.Data.SqlDbType.NVarChar,50)
                ,new SqlParameter("@shipmenttypetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@address", System.Data.SqlDbType.NVarChar,800)
                ,new SqlParameter("@addresstrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@note", System.Data.SqlDbType.NVarChar,200)
                ,new SqlParameter("@notetrue", System.Data.SqlDbType.Bit)
            };
            myupdateparams[0].Direction = System.Data.ParameterDirection.InputOutput;
        }

        private DeliveryCar mycars;
        internal DeliveryCar Car
        {
            set { mycars = value; }
            get { return mycars; }
        }
        private bool myisall;
        internal bool IsAll
        {
            set { myisall = value; }
            get { return myisall; }
        }
        private SQLFilter myfilter;
        internal SQLFilter Filter
        {
            set { myfilter = value; }
        }

        protected override DeliveryCarry CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            DeliveryCarry newitem = new DeliveryCarry(reader.GetInt32(0), reader.GetInt64(1), lib.DomainObjectState.Unchanged
                , CustomBrokerWpf.References.RequestStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("requestId")), addcon, out _)
                , reader.IsDBNull(reader.GetOrdinal("c1")) ? null : CustomBrokerWpf.References.DeliveryCarStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("c1")), addcon, out _)
                , reader.IsDBNull(reader.GetOrdinal("c2")) ? null : CustomBrokerWpf.References.DeliveryCarStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("c2")), addcon, out _)
                , reader.IsDBNull(reader.GetOrdinal("c3")) ? null : CustomBrokerWpf.References.DeliveryCarStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("c3")), addcon, out _)
                , reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString(reader.GetOrdinal("address"))
                , reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString(reader.GetOrdinal("note"))
                , reader.IsDBNull(reader.GetOrdinal("shipmentdate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("shipmentdate"))
                , reader.IsDBNull(reader.GetOrdinal("shipmenttype")) ? null : CustomBrokerWpf.References.DeliveryTypes.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("shipmenttype"))));
            return CustomBrokerWpf.References.DeliveryCarryStore.UpdateItem(newitem);
        }
        protected override void GetOutputSpecificParametersValue(DeliveryCarry item)
        {
            item.Id = (int)myupdateparams[0].Value;
        }
        protected override bool SaveChildObjects(DeliveryCarry item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(DeliveryCarry item)
        {
            bool isSuccess = true;
            if (item.Car1?.DomainState == lib.DomainObjectState.Added | item.Car2?.DomainState == lib.DomainObjectState.Added | item.Car3?.DomainState == lib.DomainObjectState.Added)
            {
                DeliveryCarDBM cdbm = new DeliveryCarDBM();
                if (item.Car1?.DomainState == lib.DomainObjectState.Added)
                    cdbm.SaveItemChanches(item.Car1);
                if (item.Car2?.DomainState == lib.DomainObjectState.Added)
                    cdbm.SaveItemChanches(item.Car2);
                if (item.Car3?.DomainState == lib.DomainObjectState.Added)
                    cdbm.SaveItemChanches(item.Car3);
                if (cdbm.Errors.Count > 0)
                {
                    isSuccess = false;
                    foreach (lib.DBMError err in cdbm.Errors) this.Errors.Add(err);
                }
            }
            return isSuccess;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetSpecificParametersValue(DeliveryCarry item)
        {
            foreach (SqlParameter par in myupdateparams)
            {
                switch (par.ParameterName)
                {
                    case "@requestid":
                        par.Value = item.Request.Id;
                        break;
                    case "@c1":
                        par.Value = item.Car1?.Id;
                        break;
                    case "@c1true":
                        par.Value = item.HasPropertyOutdatedValue("Car1");
                        break;
                    case "@c2":
                        par.Value = item.Car2?.Id;
                        break;
                    case "@c2true":
                        par.Value = item.HasPropertyOutdatedValue("Car2");
                        break;
                    case "@c3":
                        par.Value = item.Car3?.Id;
                        break;
                    case "@c3true":
                        par.Value = item.HasPropertyOutdatedValue("Car3");
                        break;
                    case "@shipmentdate":
                        par.Value = item.ShipmentDate;
                        break;
                    case "@shipmentdatetrue":
                        par.Value = item.HasPropertyOutdatedValue("ShipmentDate");
                        break;
                    case "@shipmenttype":
                        par.Value = item.ShipmentType?.Id;
                        break;
                    case "@shipmenttypetrue":
                        par.Value = item.HasPropertyOutdatedValue("ShipmentType");
                        break;
                    case "@address":
                        par.Value = item.Address;
                        break;
                    case "@addresstrue":
                        par.Value = item.HasPropertyOutdatedValue("Address");
                        break;
                    case "@note":
                        par.Value = item.Note;
                        break;
                    case "@notetrue":
                        par.Value = item.HasPropertyOutdatedValue("Note");
                        break;
                }
            }
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            base.SelectParams[0].Value = mycars?.Id;
            base.SelectParams[1].Value = myisall;
            base.SelectParams[2].Value = myfilter?.FilterWhereId;
        }
        protected override void CancelLoad()
        { }
    }

    public class DeliveryCarryVM : lib.ViewModelErrorNotifyItem<DeliveryCarry>, lib.Interfaces.ITotalValuesItem
    {
        public DeliveryCarryVM(DeliveryCarry domen) : base(domen)
        {
            ValidetingProperties.AddRange(new string[] { "ShipmentType", "Car1", "Car2", "Car3" });
            DeleteRefreshProperties.AddRange(new string[] { "CarNumber", "Company", "DeliveryDate", "Importer", "InvoiceDate", "InvoiceNumber", "InvoiceSum", "Note", "Number", "State" });
            InitProperties();
        }

        public string Address
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Address, value)))
                {
                    string name = "Address";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Address);
                    ChangingDomainProperty = name; this.DomainObject.Address = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Address : null; }
        }
        //public DeliveryCar Car1
        //{
        //    set
        //    {
        //        if (!(this.IsReadOnly || string.Equals(this.DomainObject.Car1, value)))
        //        {
        //            string name = "Car1";
        //            if (!myUnchangedPropertyCollection.ContainsKey(name))
        //                this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Car1);
        //            ChangingDomainProperty = name; this.DomainObject.Car1 = value;
        //        }
        //    }
        //    get { return this.IsEnabled ? this.DomainObject.Car1 : null; }
        //}
        private DeliveryCar mycar1;
        public DeliveryCar Car1
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(mycar1, value)))
                {
                    string name = "Car1";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycar1);
                    mycar1 = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Car1 = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? mycar1 : null; }
        }
        public decimal? Car1Cost
        { get { return this.DomainObject.Car1Cost; } }
        //public DeliveryCar Car2
        //{
        //    set
        //    {
        //        if (!(this.IsReadOnly || string.Equals(this.DomainObject.Car2, value)))
        //        {
        //            string name = "Car2";
        //            if (!myUnchangedPropertyCollection.ContainsKey(name))
        //                this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Car2);
        //            ChangingDomainProperty = name; this.DomainObject.Car2 = value;
        //        }
        //    }
        //    get { return this.IsEnabled ? this.DomainObject.Car2 : null; }
        //}
        private DeliveryCar mycar2;
        public DeliveryCar Car2
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(mycar2, value)))
                {
                    string name = "Car2";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycar2);
                    mycar2 = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Car2 = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? mycar2 : null; }
        }
        public decimal? Car2Cost
        { get { return this.DomainObject.Car2Cost; } }
        //public DeliveryCar Car3
        //{
        //    set
        //    {
        //        if (!(this.IsReadOnly || string.Equals(this.DomainObject.Car3, value)))
        //        {
        //            string name = "Car3";
        //            if (!myUnchangedPropertyCollection.ContainsKey(name))
        //                this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Car3);
        //            ChangingDomainProperty = name; this.DomainObject.Car3 = value;
        //        }
        //    }
        //    get { return this.IsEnabled ? this.DomainObject.Car3 : null; }
        //}
        private DeliveryCar mycar3;
        public DeliveryCar Car3
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(mycar3, value)))
                {
                    string name = "Car3";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycar3);
                    mycar3 = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Car3 = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? mycar3 : null; }
        }
        public decimal? Car3Cost
        { get { return this.DomainObject.Car3Cost; } }
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
        public Request Request
        { get { return this.IsEnabled ? this.DomainObject.Request : null; } }
        public DateTime? ShipmentDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.ShipmentDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.ShipmentDate.Value, value.Value))))
                {
                    string name = "ShipmentDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ShipmentDate);
                    ChangingDomainProperty = name; this.DomainObject.ShipmentDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ShipmentDate : null; }
        }
        private lib.ReferenceSimpleItem myshipmenttype;
        //public lib.ReferenceSimpleItem ShipmentType
        //{
        //    set
        //    {
        //        if (!(this.IsReadOnly || object.Equals(this.DomainObject.ShipmentType, value)))
        //        {
        //            string name = "ShipmentType";
        //            if (!myUnchangedPropertyCollection.ContainsKey(name))
        //                this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ShipmentType);
        //            ChangingDomainProperty = name; this.DomainObject.ShipmentType = value;
        //        }
        //    }
        //    get { return this.IsEnabled ? this.DomainObject.ShipmentType : null; }
        //}
        public lib.ReferenceSimpleItem ShipmentType
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(myshipmenttype, value)))
                {
                    string name = "ShipmentType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myshipmenttype);
                    myshipmenttype = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.ShipmentType = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? myshipmenttype : null; }
        }
        private decimal mytotalcostold;
        public decimal? TotalCost
        { get { return this.DomainObject.TotalCost; } }

        public bool ProcessedIn { set; get; }
        public bool ProcessedOut { set; get; }
        private bool myselected;
        public bool Selected
        { set { bool oldvalue = myselected; myselected = value; this.OnValueChanged("Selected", oldvalue, value); } get { return myselected; } }

        private RequestAddressDeliveryDBM myddbm;
        private ObservableCollection<CustomerAddressSelected> mydeliveryaddresses;
        private ListCollectionView mydeliveryaddressesview;
        public ListCollectionView DeliveryAddresses
        {
            get
            {
                if (mydeliveryaddressesview == null)
                {
                    myddbm = new RequestAddressDeliveryDBM(this.Request);
                    myddbm.FillAsyncCompleted = () =>
                    {
                        foreach (CustomerAddressSelected item in myddbm.Collection)
                        {
                            if (!string.IsNullOrEmpty(this.Address) && this.Address.IndexOf(item.FullAddressDescription) > -1)
                                item.Selected = true;
                            item.Carry = this;
                        }
                    };
                    mydeliveryaddresses = new ObservableCollection<CustomerAddressSelected>();
                    myddbm.Collection = mydeliveryaddresses;
                    myddbm.FillAsync();
                    mydeliveryaddressesview = new ListCollectionView(mydeliveryaddresses);
                    mydeliveryaddressesview.SortDescriptions.Add(new System.ComponentModel.SortDescription("FullAddressDescription", System.ComponentModel.ListSortDirection.Ascending));
                }
                return mydeliveryaddressesview;
            }
        }
        internal bool DeliveryAddressesIsNull
        { get { return mydeliveryaddressesview == null; } }

        internal void DeliveryAddressesRefresh()
        {
            if (myddbm != null)
            {
                myddbm.FillAsync();
            }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Car1":
                    mycar1 = this.DomainObject.Car1;
                    break;
                case "Car2":
                    mycar2 = this.DomainObject.Car2;
                    break;
                case "Car3":
                    mycar3 = this.DomainObject.Car3;
                    break;
                case "ShipmentType":
                    myshipmenttype = this.DomainObject.ShipmentType;
                    break;
                case "TotalCost":
                    this.OnValueChanged("TotalCost", mytotalcostold, this.TotalCost ?? 0M);
                    mytotalcostold = this.TotalCost ?? 0M;
                    break;
            }
        }
        protected override void InitProperties()
        {
            mycar1 = this.DomainObject.Car1;
            mycar2 = this.DomainObject.Car2;
            mycar3 = this.DomainObject.Car3;
            myshipmenttype = this.DomainObject.ShipmentType;
            mytotalcostold = this.TotalCost ?? 0M;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Address":
                    this.DomainObject.Address = (string)value;
                    break;
                case "Car1":
                    if (mycar1 != this.DomainObject.Car1)
                        mycar1 = this.DomainObject.Car1;
                    else
                        this.DomainObject.Car1 = (DeliveryCar)value;
                    break;
                case "Car2":
                    if (mycar2 != this.DomainObject.Car2)
                        mycar2 = this.DomainObject.Car2;
                    else
                        this.DomainObject.Car2 = (DeliveryCar)value;
                    break;
                case "Car3":
                    if (mycar3 != this.DomainObject.Car3)
                        mycar3 = this.DomainObject.Car3;
                    else
                        this.DomainObject.Car3 = (DeliveryCar)value;
                    break;
                case "Note":
                    this.DomainObject.Note = (string)value;
                    break;
                case "ShipmentDate":
                    this.DomainObject.ShipmentDate = (DateTime?)value;
                    break;
                case "ShipmentType":
                    if (myshipmenttype != this.DomainObject.ShipmentType)
                        myshipmenttype = this.DomainObject.ShipmentType;
                    else
                        this.DomainObject.ShipmentType = (lib.ReferenceSimpleItem)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "ShipmentType":
                    if (this.ShipmentType != null && this.ShipmentType.IsDefault && (this.Car1 != null | this.Car2 != null | this.Car3 != null))
                    {
                        errmsg = "Самовывоз!!! Необходимо удалить груз из перевозки!";
                        isvalid = false;
                    }
                    break;
                    //case "Car1":
                    //    if (this.ShipmentType != null && this.ShipmentType.IsDefault && this.Car1 != null)
                    //    {
                    //        errmsg = "Самовывоз!!! Необходимо удалить груз из перевозки!";
                    //        isvalid = false;
                    //    }
                    //    break;
                    //case "Car2":
                    //    if (this.ShipmentType != null && this.ShipmentType.IsDefault && this.Car2 != null)
                    //    {
                    //        errmsg = "Самовывоз!!! Необходимо удалить груз из перевозки!";
                    //        isvalid = false;
                    //    }
                    //    break;
                    //case "Car3":
                    //    if (this.ShipmentType != null && this.ShipmentType.IsDefault && this.Car3 != null)
                    //    {
                    //        errmsg = "Самовывоз!!! Необходимо удалить груз из перевозки!";
                    //        isvalid = false;
                    //    }
                    //    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            else if (isvalid) ClearErrorMessageForProperty(propertyname);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return mycar1 != this.DomainObject.Car1 || mycar2 != this.DomainObject.Car2 || mycar3 != this.DomainObject.Car3 || myshipmenttype != this.DomainObject.ShipmentType;
        }
    }

    public class DeliveryCarrySynchronizer : lib.ModelViewCollectionsSynchronizer<DeliveryCarry, DeliveryCarryVM>
    {
        protected override DeliveryCarry UnWrap(DeliveryCarryVM wrap)
        {
            return wrap.DomainObject as DeliveryCarry;
        }
        protected override DeliveryCarryVM Wrap(DeliveryCarry fill)
        {
            return new DeliveryCarryVM(fill);
        }
    }

    public class DeliveryCarryViewCommand : lib.ViewModelViewCommand, IDisposable
    {
        internal DeliveryCarryViewCommand()
        {
            mycdbm = new DeliveryCarryDBM();
            mydbm = mycdbm;
            mycdbm.FillAsyncCompleted = () => { mytotal.StartCount(); if (mycdbm.Errors.Count > 0) OpenPopup(mycdbm.ErrorMessage, true); };
            mycdbm.Collection = new ObservableCollection<DeliveryCarry>();
            mysync = new DeliveryCarrySynchronizer();
            mysync.DomainCollection = mycdbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            SettingView();

            mytotal = new DeliveryCarryTotal(myview);
            myfilter = new SQLFilter("deliverycarry", "AND");
            myservicetypefiltergroup = myfilter.GroupAdd(myfilter.FilterWhereId, "servicetype", "OR");
            mycdbm.Filter = myfilter;
            //if (myfilter.isEmpty)
            //    this.OpenPopup("Грузы. Пожалуйста, задайте критерии выбора грузов!", false);
            //else
            mycdbm.FillAsync();

            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            myparcelfilter = new ParcelCheckListBoxVM();
            myparcelfilter.ExecCommand1 = () => { this.FilterRunExec(null); };
            myrequestfilter = new RequestCheckListBoxVM();
            myrequestfilter.ExecCommand1 = () => { this.FilterRunExec(null); };
            myrequestfilter.ParcelFilter = myparcelfilter;
            mycustomerfilter = new CustomerCheckListBoxVM();
            mycustomerfilter.ExecCommand1 = () => { this.FilterRunExec(null); };
            mycustomerlegalfilter = new CustomerLegalCheckListBoxVM();
            mycustomerlegalfilter.ExecCommand1 = () => { this.FilterRunExec(null); };
            mycustomerlegalfilter.CustomerFilter = mycustomerfilter;
            myimporterfilter = new ImporterCheckListBoxVM();
            myimporterfilter.ExecCommand1 = () => { this.FilterRunExec(null); };
            myservicetypefilter = new ServiceTypeCheckListBoxVM();
            myservicetypefilter.ExecCommand1 = () => { this.FilterRunExec(null); };
            myshipmentdatefilter = new libui.DateFilterVM();
            myshipmentdatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myshipmentdatefilter.ExecCommand2 = () => { FilterRunExec(null); };
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
        }

        private DeliveryCarryDBM mycdbm;
        private DeliveryCarrySynchronizer mysync;

        private SQLFilter myfilter;
        internal SQLFilter Filter
        { get { return myfilter; } }
        private ParcelCheckListBoxVM myparcelfilter;
        public ParcelCheckListBoxVM ParcelFilter
        { get { return myparcelfilter; } }
        private RequestCheckListBoxVM myrequestfilter;
        public RequestCheckListBoxVM RequestFilter
        { get { return myrequestfilter; } }
        private CustomerCheckListBoxVM mycustomerfilter;
        public CustomerCheckListBoxVM CustomerFilter
        { get { return mycustomerfilter; } }
        private CustomerLegalCheckListBoxVM mycustomerlegalfilter;
        public CustomerLegalCheckListBoxVM CustomerLegalFilter
        { get { return mycustomerlegalfilter; } }
        private ImporterCheckListBoxVM myimporterfilter;
        public ImporterCheckListBoxVM ImporterFilter
        { get { return myimporterfilter; } }
        private int myservicetypefiltergroup;
        private ServiceTypeCheckListBoxVM myservicetypefilter;
        public ServiceTypeCheckListBoxVM ServiceTypeFilter
        { get { return myservicetypefilter; } }
        private libui.DateFilterVM myshipmentdatefilter;
        public libui.DateFilterVM ShipmentDateFilterCommand
        { get { return myshipmentdatefilter; } }
        private DeliveryCarryTotal mytotal;
        public DeliveryCarryTotal Total { get { return mytotal; } }

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            //if (!(myparcelfilter.FilterOn | myrequestfilter.FilterOn | mycustomerfilter.FilterOn | mycustomerlegalfilter.FilterOn | myimporterfilter.FilterOn | myservicetypefilter.FilterOn | myshipmentdatefilter.FilterOn))
            //    this.OpenPopup("Фильтр. Пожалуйста, задайте критерии выбора грузов!", false);
            //else
            //{
            this.EndEdit();
            if (myparcelfilter.FilterOn)
            {
                string[] parcels = new string[myparcelfilter.SelectedItems.Count];
                for (int i = 0; i < myparcelfilter.SelectedItems.Count; i++)
                    parcels[i] = (myparcelfilter.SelectedItems[i] as Parcel).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "parcel", parcels);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "parcel", new string[0]);
            if (myrequestfilter.FilterOn)
            {
                string[] requests = new string[myrequestfilter.SelectedItems.Count];
                for (int i = 0; i < myrequestfilter.SelectedItems.Count; i++)
                    requests[i] = (myrequestfilter.SelectedItems[i] as Request).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "request", requests);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "request", new string[0]);
            if (mycustomerfilter.FilterOn)
            {
                string[] parcels = new string[mycustomerfilter.SelectedItems.Count];
                for (int i = 0; i < mycustomerfilter.SelectedItems.Count; i++)
                    parcels[i] = (mycustomerfilter.SelectedItems[i] as Customer).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "customer", parcels);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "customer", new string[0]);
            if (mycustomerlegalfilter.FilterOn)
            {
                string[] parcels = new string[mycustomerlegalfilter.SelectedItems.Count];
                for (int i = 0; i < mycustomerlegalfilter.SelectedItems.Count; i++)
                    parcels[i] = (mycustomerlegalfilter.SelectedItems[i] as CustomerLegal).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "customerlegal", parcels);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "customerlegal", new string[0]);
            if (myimporterfilter.FilterOn)
            {
                string[] parcels = new string[myimporterfilter.SelectedItems.Count];
                for (int i = 0; i < myimporterfilter.SelectedItems.Count; i++)
                    parcels[i] = (myimporterfilter.SelectedItems[i] as Importer).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "importer", parcels);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "importer", new string[0]);
            if (myservicetypefilter.FilterOn)
            {
                bool isNullOrEmpty = false;
                string[] parcels = new string[myservicetypefilter.SelectedItems.Count];
                for (int i = 0; i < myservicetypefilter.SelectedItems.Count; i++)
                {
                    parcels[i] = (myservicetypefilter.SelectedItems[i] as lib.ReferenceSimpleItem).Name;
                    if (string.IsNullOrEmpty(parcels[i])) isNullOrEmpty = true;
                }
                myfilter.SetList(myservicetypefiltergroup, "servicetype", parcels);
                List<SQLFilterCondition> conds = myfilter.ConditionGet(myservicetypefiltergroup, "servicetype");
                if (isNullOrEmpty)
                { if (conds.Count == 1) myfilter.ConditionAdd(myservicetypefiltergroup, "servicetype", "IS NULL"); }
                else
                    if (conds.Count > 1) myfilter.ConditionDel(myfilter.ConditionGet(myservicetypefiltergroup, "servicetype")[1].propertyid);
            }
            else
                foreach (SQLFilterCondition cond in myfilter.ConditionGet(myservicetypefiltergroup, "servicetype"))
                    myfilter.ConditionDel(cond.propertyid);
            myfilter.SetDate(myfilter.FilterWhereId, "shipmentdate", "shipmentdate", myshipmentdatefilter.DateStart, myshipmentdatefilter.DateStop);

            mytotal.StopCount();
            mycdbm.FillAsync();
            //}
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
            myparcelfilter.Clear();
            myparcelfilter.IconVisibileChangedNotification();
            myrequestfilter.Clear();
            myrequestfilter.IconVisibileChangedNotification();
            mycustomerfilter.Clear();
            mycustomerfilter.IconVisibileChangedNotification();
            mycustomerlegalfilter.Clear();
            mycustomerlegalfilter.IconVisibileChangedNotification();
            myimporterfilter.Clear();
            myimporterfilter.IconVisibileChangedNotification();
            myservicetypefilter.Clear();
            myservicetypefilter.IconVisibileChangedNotification();
            myshipmentdatefilter.Clear();
            myshipmentdatefilter.IconVisibileChangedNotification();
            this.FilterRunExec(null);
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }

        private System.ComponentModel.BackgroundWorker mybw;
        private ExcelImportWin myexcelimportwin;
        private RelayCommand myexcelexport;
        public ICommand ExcelExport
        {
            get { return myexcelexport; }
        }
        private void ExcelExportExec(object parametr)
        {
            this.myendedit();
            if (mybw == null)
            {
                mybw = new System.ComponentModel.BackgroundWorker();
                mybw.DoWork += BackgroundWorker_DoWork;
                mybw.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                mybw.WorkerReportsProgress = true;
                mybw.ProgressChanged += BackgroundWorker_ProgressChanged;
            }
            if (!mybw.IsBusy)
            {
                string[] columns;
                libui.PopUpWindow win = new libui.PopUpWindow();
                win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                Grid grid = new Grid();
                Grid.SetIsSharedSizeScope(grid, true);
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition() { Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star), MinWidth = 20 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Auto), SharedSizeGroup = "b" });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star), MinWidth = 20 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Auto), SharedSizeGroup = "b" });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star), MinWidth = 20 });
                ListBox list = new ListBox();
                list.Style = list.FindResource("CheckBoxListStyle") as System.Windows.Style;
                list.Items.Add("№ а//м"); list.Items.Add("Складской номер"); list.Items.Add("Клиент"); list.Items.Add("Юр лица"); list.Items.Add("Импортер"); list.Items.Add("Услуга"); list.Items.Add("Вес Д"); list.Items.Add("Вес Ф"); list.Items.Add("Объем"); list.Items.Add("Мест"); list.Items.Add("№ 1 перевозки"); list.Items.Add("Стоимость 1 перевозки"); list.Items.Add("№ 2 перевозки"); list.Items.Add("Стоимость 2 перевозки"); list.Items.Add("№ 3 перевозки"); list.Items.Add("Стоимость 3 перевозки"); list.Items.Add("СУММА ПЕРЕВОЗКИ"); list.Items.Add("Дата отгрузки"); list.Items.Add("Тип отгрузки"); list.Items.Add("Адреса"); list.Items.Add("Примечания");
                list.SelectAll();
                list.SetValue(Grid.ColumnSpanProperty, 5);
                list.Margin = new System.Windows.Thickness(2D, 2D, 2D, 10D);
                grid.Children.Add(list);
                Button bok = new Button() { Content = "OK", IsDefault = true };
                columns = new string[list.Items.Count];
                bok.Click += (object sender, System.Windows.RoutedEventArgs e) => { win.DialogResult = true; list.SelectedItems.CopyTo(columns, 0); win.Close(); };
                bok.SetValue(Grid.RowProperty, 1);
                bok.SetValue(Grid.ColumnProperty, 1);
                bok.Margin = new System.Windows.Thickness(0D, 0D, 0D, 6D);
                grid.Children.Add(bok);
                Button besc = new Button() { Content = "Отмена", IsDefault = false };
                besc.Click += (object sender, System.Windows.RoutedEventArgs e) => { win.DialogResult = false; win.Close(); };
                besc.SetValue(Grid.RowProperty, 1);
                besc.SetValue(Grid.ColumnProperty, 3);
                besc.Margin = new System.Windows.Thickness(0D, 0D, 0D, 6D);
                grid.Children.Add(besc);
                win.Content = grid;
                bool? ok = win.ShowDialog();
                if (ok.HasValue && ok.Value)
                {
                    DoWorkContaner arg = new DoWorkContaner();
                    arg.Columns = columns;
                    if (parametr is System.Collections.IList && (parametr as System.Collections.IList).Count > 1)
                    {
                        arg.SelectedItems = parametr as System.Collections.IList;
                        arg.Count = (parametr as System.Collections.IList).Count;
                    }
                    else
                    {
                        arg.SelectedItems = myview;
                        arg.Count = myview.Count;
                    }
                    if (myexcelimportwin != null && myexcelimportwin.IsVisible)
                    {
                        myexcelimportwin.MessageTextBlock.Text = string.Empty;
                        myexcelimportwin.ProgressBar1.Value = 0;
                    }
                    else
                    {
                        myexcelimportwin = new ExcelImportWin();
                        myexcelimportwin.Show();
                    }
                    mybw.RunWorkerAsync(arg);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool ExcelExportCanExec(object parametr)
        { return !(myview.IsAddingNew | myview.IsEditingItem); }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            worker.ReportProgress(2);
            //string[] args = e.Argument as string[];
            //bool isclose = bool.Parse(args[0]);
            try
            {
                //if (isclose)
                //    e.Result = OnExcelImport(worker, exApp, args[1], bool.Parse(args[2]));
                //else
                e.Result = OnExcelExport(worker, exApp, e.Argument as DoWorkContaner);
                worker.ReportProgress(100);

            }
            finally
            {
                if (exApp != null)
                {
                    //if (isclose)
                    //{
                    //    foreach (Excel.Workbook itemBook in exApp.Workbooks)
                    //    {
                    //        itemBook.Close(false);
                    //    }
                    //    exApp.DisplayAlerts = true;
                    //    exApp.ScreenUpdating = true;
                    //    exApp.Quit();
                    //}
                    //else
                    //{
                    exApp.Visible = true;
                    exApp.DisplayAlerts = true;
                    exApp.ScreenUpdating = true;
                    //}
                    exApp = null;
                }
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                myexcelimportwin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                myexcelimportwin.MessageTextBlock.Text = "Загрузка прервана из-за ошибки" + "\n" + e.Error.Message;
            }
            else
            {
                myexcelimportwin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                myexcelimportwin.MessageTextBlock.Text = "Загрузка выполнена успешно." + "\n" + e.Result.ToString() + " строк обработано";
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            myexcelimportwin.ProgressBar1.Value = e.ProgressPercentage;
        }

        private int OnExcelExport(BackgroundWorker worker, Excel.Application exApp, DoWorkContaner arg)
        {
            Excel.Workbook exWb;
            try
            {
                int row = 2, column = 1;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                exWh.Name = "Перевозки";
                Excel.Range r;

                exWh.Rows[1, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                foreach (string name in arg.Columns)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        exWh.Cells[1, column] = name;
                        switch (name)
                        {
                            case "№ а//м":
                            case "Складской номер":
                            case "№ 1 перевозки":
                            case "№ 2 перевозки":
                            case "№ 3 перевозки":
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                exWh.Columns[column, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                                break;
                            case "Примечания":
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                break;
                            case "Дата отгрузки":
                                //exWh.Columns[column, Type.Missing].NumberFormat = "dd/mm/yy";
                                exWh.Columns[column, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                                break;
                            case "Стоимость 1 перевозки":
                            case "Стоимость 2 перевозки":
                            case "Стоимость 3 перевозки":
                            case "Вес Д":
                            case "Вес Ф":
                            case "Объем":
                            case "СУММА ПЕРЕВОЗКИ":
                                try { exWh.Columns[column, Type.Missing].NumberFormat = "# #0,00"; } catch { }
                                break;
                        }
                        column++;
                    }
                    else
                        break;
                }
                worker.ReportProgress(2 + (int)(decimal.Divide(1, arg.Count + 1) * 100));
                foreach (DeliveryCarryVM item in arg.SelectedItems.OfType<DeliveryCarryVM>())
                {
                    column = 1;
                    foreach (string name in arg.Columns)
                    {
                        switch (name)
                        {
                            case "№ а//м":
                                exWh.Cells[row, column] = item.Request.Parcel.ParcelNumberEntire;
                                break;
                            case "Складской номер":
                                exWh.Cells[row, column] = item.Request.StorePointDate;
                                break;
                            case "Клиент":
                                exWh.Cells[row, column] = item.Request.CustomerName;
                                break;
                            case "Юр лица":
                                exWh.Cells[row, column] = item.Request.CustomerLegalsNames;
                                break;
                            case "Импортер":
                                exWh.Cells[row, column] = item.Request.Importer.Name;
                                break;
                            case "Услуга":
                                exWh.Cells[row, column] = item.Request.ServiceType;
                                break;
                            case "Вес Д":
                                exWh.Cells[row, column] = item.Request.OfficialWeight;
                                break;
                            case "Вес Ф":
                                exWh.Cells[row, column] = item.Request.ActualWeight;
                                break;
                            case "Объем":
                                exWh.Cells[row, column] = item.Request.Volume;
                                break;
                            case "Мест":
                                exWh.Cells[row, column] = item.Request.CellNumber;
                                break;
                            case "№ 1 перевозки":
                                exWh.Cells[row, column] = item.Car1?.Number;
                                break;
                            case "Стоимость 1 перевозки":
                                exWh.Cells[row, column] = item.Car1Cost;
                                break;
                            case "№ 2 перевозки":
                                exWh.Cells[row, column] = item.Car2?.Number;
                                break;
                            case "Стоимость 2 перевозки":
                                exWh.Cells[row, column] = item.Car2Cost;
                                break;
                            case "№ 3 перевозки":
                                exWh.Cells[row, column] = item.Car3?.Number;
                                break;
                            case "Стоимость 3 перевозки":
                                exWh.Cells[row, column] = item.Car3Cost;
                                break;
                            case "СУММА ПЕРЕВОЗКИ":
                                exWh.Cells[row, column] = item.TotalCost;
                                break;
                            case "Дата отгрузки":
                                exWh.Cells[row, column] = item.ShipmentDate;
                                break;
                            case "Тип отгрузки":
                                exWh.Cells[row, column] = item.ShipmentType;
                                break;
                            case "Адреса":
                                exWh.Cells[row, column] = item.Address;
                                break;
                            case "Примечания":
                                exWh.Cells[row, column] = item.Note;
                                break;
                        }
                        column++;
                    }

                    worker.ReportProgress(2 + (int)(decimal.Divide(row + 1, arg.Count + 1) * 100));
                    row++;
                }

                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, column - 1]];
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                r.VerticalAlignment = Excel.Constants.xlTop;
                r.WrapText = true;
                //r = exWh.Range[exWh.Columns[1, Type.Missing], exWh.Columns[17, Type.Missing]]; r.Columns.AutoFit();
                exApp.Visible = true;
                exWh = null;
                return row - 2;
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
                    exApp = null;
                }
                throw new ApplicationException(ex.Message);
            }
        }
        private class DoWorkContaner
        {
            internal string[] Columns { set; get; }
            internal System.Collections.IEnumerable SelectedItems { set; get; }
            internal int Count { set; get; }
        }

        public bool IsAll
        {
            set { mycdbm.IsAll = value; this.SaveRefresh.Execute(null); }
            get { return mycdbm.IsAll; }
        }
        private ListCollectionView mycars;
        public ListCollectionView Cars
        {
            get
            {
                if (mycars == null)
                {
                    //DeliveryCarDBM cdbm = new DeliveryCarDBM();
                    //cdbm.isAll = false;
                    //cdbm.FillAsyncCompleted = () => { if (cdbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); };
                    //cdbm.FillAsync();
                    mycars = new ListCollectionView(CustomBrokerWpf.References.DeliveryCars);
                    mycars.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item) && (item as DeliveryCar).State.Id < 10; };
                    mycars.SortDescriptions.Add(new System.ComponentModel.SortDescription("Number", System.ComponentModel.ListSortDirection.Ascending));
                    CustomBrokerWpf.References.CarsViewCollector.AddView(mycars);
                }
                return mycars;
            }
        }
        private ListCollectionView mydeliverytypes;
        public ListCollectionView DeliveryTypes
        {
            get
            {
                if (mydeliverytypes == null)
                {
                    mydeliverytypes = new ListCollectionView(CustomBrokerWpf.References.DeliveryTypes);
                    mydeliverytypes.SortDescriptions.Add(new System.ComponentModel.SortDescription("IsDefault", System.ComponentModel.ListSortDirection.Descending));
                    mydeliverytypes.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
                }
                return mydeliverytypes;
            }
        }

        protected override bool CanAddData(object parametr)
        {
            return false;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return false;
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
            CustomBrokerWpf.References.CarryViewCollector.RefreshViews();
        }
        protected override void RefreshData(object parametr)
        {
            mycdbm.FillAsyncCompleted = () => { if (mydbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); else foreach (DeliveryCarryVM item in mysync.ViewModelCollection) item.DeliveryAddressesRefresh(); mytotal.StartCount(); };
            //if(myfilter.isEmpty)
            //    this.OpenPopup("Фильтр. Пожалуйста, задайте критерии выбора грузов!", false);
            //else
            mytotal.StopCount();
            mycdbm.FillAsync();
            CustomBrokerWpf.References.DeliveryCars = null;
            DeliveryCarDBM cdbm = new DeliveryCarDBM();
            cdbm.isAll = false;
            cdbm.FillAsync();
        }
        protected override void RejectChanges(object parametr)
        {
            System.Collections.IList rejects;
            if (parametr is System.Collections.IList && (parametr as System.Collections.IList).Count > 0)
                rejects = parametr as System.Collections.IList;
            else
                rejects = mysync.ViewModelCollection;

            System.Collections.Generic.List<DeliveryCarryVM> deleted = new System.Collections.Generic.List<DeliveryCarryVM>();
            foreach (object item in rejects)
            {
                if (item is DeliveryCarryVM)
                {
                    DeliveryCarryVM ritem = item as DeliveryCarryVM;
                    if (ritem.DomainState == lib.DomainObjectState.Added)
                        deleted.Add(ritem);
                    else
                    {
                        myview.EditItem(ritem);
                        ritem.RejectChanges();
                        myview.CommitEdit();
                    }
                }
            }
            foreach (DeliveryCarryVM delitem in deleted)
            {
                mysync.ViewModelCollection.Remove(delitem);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
            }
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Request.Parcel.Id", System.ComponentModel.ListSortDirection.Ascending));
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Request.CustomerName", System.ComponentModel.ListSortDirection.Ascending));
            CustomBrokerWpf.References.CarryViewCollector.AddView(myview);
        }

        public void Dispose()
        {
            myfilter.RemoveFilter();
            CustomBrokerWpf.References.CarsViewCollector.RemoveView(mycars);
            CustomBrokerWpf.References.CarryViewCollector.RemoveView(myview);
        }
    }

    public class AddressCheckListBoxVM : libui.CheckListBoxVMFill<DeliveryCarryVM, string>
    {
        protected override void AddItem(DeliveryCarryVM item)
        {
            if (!Items.Contains(item.Address)) Items.Add(item.Address);
        }
    }
    public class Car1CheckListBoxVM : libui.CheckListBoxVMFill<DeliveryCarryVM, DeliveryCar>
    {
        public Car1CheckListBoxVM() : base()
        {
            this.DisplayPath = "Number";
            this.SearchPath = "Number";
            this.GetDisplayPropertyValueFunc = (item) => { return ((DeliveryCar)item).Number.ToString(); };
        }

        protected override void AddItem(DeliveryCarryVM item)
        {
            if (!Items.Contains(item.Car1)) Items.Add(item.Car1);
        }
    }
    public class Car2CheckListBoxVM : libui.CheckListBoxVMFill<DeliveryCarryVM, DeliveryCar>
    {
        public Car2CheckListBoxVM() : base()
        {
            this.DisplayPath = "Number";
            this.SearchPath = "Number";
            this.GetDisplayPropertyValueFunc = (item) => { return ((DeliveryCar)item).Number.ToString(); };
        }

        protected override void AddItem(DeliveryCarryVM item)
        {
            if (!Items.Contains(item.Car2)) Items.Add(item.Car2);
        }
    }
    public class Car3CheckListBoxVM : libui.CheckListBoxVMFill<DeliveryCarryVM, DeliveryCar>
    {
        public Car3CheckListBoxVM() : base()
        {
            this.DisplayPath = "Number";
            this.SearchPath = "Number";
            this.GetDisplayPropertyValueFunc = (item) => { return ((DeliveryCar)item).Number.ToString(); };
        }

        protected override void AddItem(DeliveryCarryVM item)
        {
            if (!Items.Contains(item.Car3)) Items.Add(item.Car3);
        }
    }

    public class ParcelCheckListBoxVM : libui.CheckListBoxVM
    {
        internal ParcelCheckListBoxVM()
        {
            this.DisplayPath = "ParcelNumberEntire";
            this.SearchPath = "ParcelNumberEntire";
            this.GetDisplayPropertyValueFunc = (item) => { return (item as Parcel).ParcelNumberEntire; };
            this.SelectedAll = false;
            this.RefreshIsVisible = true;
            this.ExecRefresh = () => { this.Fill(); };
            this.ExecCommand2 = () => { this.Clear(); };

            myfilter = new lib.SQLFilter.SQLFilter("Parcel", "AND", CustomBrokerWpf.References.ConnectionString);
            if (!myfilter.isEmpty) myfilter.RemoveCurrentWhere();
            myfilter.GroupAdd(myfilter.FilterWhereId, "date", "AND");
            myfilter.SetDate(myfilter.FilterWhereId, "date", "shipplandate", DateTime.MinValue.AddYears(2018), null);
            myfilter.SetNumber(myfilter.FilterWhereId, "parceltype", lib.SQLFilter.Operators.Equal, "2");
            myfilter.SetNumber(myfilter.FilterWhereId, "parcelstatus", lib.SQLFilter.Operators.Less, "500");
            myfilter.ConditionAdd(myfilter.FilterWhereId, "terminalin", "NOT NULL");
            mydbm = new ParcelDBM();
            mydbm.Filter = myfilter.FilterWhereId;
            mydbm.Collection = new ObservableCollection<Parcel>();
            mydbm.FillAsyncCompleted = () => { this.ItemsView.Refresh(); PropertyChangedNotification("ItemsView"); };
            this.Items = mydbm.Collection;
            this.ItemsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Descending));
        }

        ParcelDBM mydbm;
        lib.SQLFilter.SQLFilter myfilter;

        internal void Fill()
        {
            mydbm.FillAsync();
        }
    }
    public class RequestCheckListBoxVM : libui.CheckListBoxVM
    {
        internal RequestCheckListBoxVM()
        {
            this.DisplayPath = "StorePointDate";
            this.SearchPath = "StorePointDate";
            this.GetDisplayPropertyValueFunc = (item) => { return (item as Request).StorePointDate; };
            this.SelectedAll = false;
            this.RefreshIsVisible = true;
            this.ExecRefresh = () => { this.Fill(); };
            this.ExecCommand2 = () => { this.Clear(); };

            myfilter = new lib.SQLFilter.SQLFilter("Request", "AND", CustomBrokerWpf.References.ConnectionString);
            if (!myfilter.isEmpty) myfilter.RemoveCurrentWhere();
            myfilter.GroupAdd(myfilter.FilterWhereId, "date", "AND");
            myfilter.SetNumber(myfilter.FilterWhereId, "parceltype", lib.SQLFilter.Operators.Equal, "2");
            myfilter.SetNumber(myfilter.FilterWhereId, "status", lib.SQLFilter.Operators.Less, "500");
            mydbm = new RequestDBM();
            mydbm.Filter = myfilter.FilterWhereId;
            mydbm.Collection = new ObservableCollection<Request>();
            mydbm.FillAsyncCompleted = () => { this.ItemsView.Refresh(); PropertyChangedNotification("ItemsView"); };
            this.Items = mydbm.Collection;
            this.ItemsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Descending));
        }

        RequestDBM mydbm;
        lib.SQLFilter.SQLFilter myfilter;
        private ParcelCheckListBoxVM myparcelfilter;
        internal ParcelCheckListBoxVM ParcelFilter
        { set { myparcelfilter = value; } }

        internal void Fill()
        {
            if (myparcelfilter != null && myparcelfilter.FilterOn)
            {
                string[] str = new string[myparcelfilter.SelectedItems.Count];
                for (int i = 0; i < myparcelfilter.SelectedItems.Count; i++)
                    str[i] = (myparcelfilter.SelectedItems[i] as Parcel).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "parcel", str);
                myfilter.SetDate(myfilter.FilterWhereId, "date", "shipplandate", null, null);
                List<lib.SQLFilter.SQLFilterCondition> cond = myfilter.ConditionGet(myfilter.FilterWhereId, "terminalin");
                if (cond.Count > 0) myfilter.ConditionDel(cond[0].propertyid);
            }
            else
            {
                myfilter.SetList(myfilter.FilterWhereId, "parcel", new string[] { });
                myfilter.SetDate(myfilter.FilterWhereId, "date", "shipplandate", DateTime.MinValue.AddYears(2018), null);
                List<lib.SQLFilter.SQLFilterCondition> cond = myfilter.ConditionGet(myfilter.FilterWhereId, "terminalin");
                if (cond.Count == 0) myfilter.ConditionAdd(myfilter.FilterWhereId, "terminalin", "NOT NULL");
            }
            mydbm.FillAsync();
        }
    }
    public class ImporterCheckListBoxVM : libui.CheckListBoxVM
    {
        internal ImporterCheckListBoxVM()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return (item as Importer).Name; };
            this.SelectedAll = false;
            this.RefreshIsVisible = false;
            this.ExecCommand2 = () => { this.Clear(); };

            this.Items = CustomBrokerWpf.References.Importers;
            this.ItemsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }
    public class ServiceTypeCheckListBoxVM : libui.CheckListBoxVM
    {
        internal ServiceTypeCheckListBoxVM()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return (item as lib.ReferenceSimpleItem).Name; };
            this.SelectedAll = false;
            this.RefreshIsVisible = false;
            this.ExecCommand2 = () => { this.Clear(); };

            this.Items = CustomBrokerWpf.References.ServiceTypes;
            this.ItemsView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class CustomerAddressSelected : CustomerAddress
    {
        internal CustomerAddressSelected(int addressid, lib.DomainObjectState dstate, string addressdescription, byte addresstypeid, int customerid, string locality, string town) : base(addressid, dstate, addressdescription, addresstypeid, customerid, locality, town) { }
        private bool myselected;
        public bool Selected
        {
            set
            {
                myselected = value;
                PropertyChangedNotification("Selected");
                if (mycarry != null)
                {
                    if (myselected)
                    {
                        if (string.IsNullOrEmpty(mycarry.Address) || mycarry.Address.IndexOf(this.FullAddressDescription) == -1)
                        {
                            if (string.IsNullOrEmpty(mycarry.Address))
                                mycarry.Address = this.FullAddressDescription;
                            else
                                mycarry.Address = mycarry.Address + ", " + this.FullAddressDescription;
                        }
                    }
                    else
                    {
                        int n = mycarry.Address.IndexOf(this.FullAddressDescription);
                        if (!string.IsNullOrEmpty(mycarry.Address) && n > -1)
                        {
                            if (n == 0)
                            {
                                string address = mycarry.Address.Substring(this.FullAddressDescription.Length);
                                if (!string.IsNullOrEmpty(address))
                                    address = address.Substring(2);
                                mycarry.Address = address;
                            }
                            else
                                mycarry.Address = mycarry.Address.Replace(", " + this.FullAddressDescription, "");
                        }
                    }
                }
            }
            get { return myselected; }
        }
        private DeliveryCarryVM mycarry;
        internal DeliveryCarryVM Carry
        { set { mycarry = value; } get { return mycarry; } }
    }
    internal class RequestAddressDeliveryDBM : lib.DBMSFill<CustomerAddressSelected>
    {
        internal RequestAddressDeliveryDBM(Request request) : base()
        {
            myrequest = request;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectCommandText = "dbo.RequestAddressDelivery_sp";
            SelectProcedure = true;
            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int)
            };
        }

        private Request myrequest;

        protected override CustomerAddressSelected CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new CustomerAddressSelected(reader.GetInt32(2), lib.DomainObjectState.Unchanged, reader.IsDBNull(3) ? null : reader.GetString(3), reader.GetByte(1), reader.GetInt32(0), reader.IsDBNull(4) ? null : reader.GetString(4), reader.IsDBNull(5) ? null : reader.GetString(5));
        }
        protected override void PrepareFill(SqlConnection addcon)
        {
            myselectparams[0].Value = myrequest.Id;
        }
        protected override void CancelLoad()
        { }
    }

    public class DeliveryCarryTotal : lib.TotalValues.TotalViewValues<DeliveryCarryVM>
    {
        internal DeliveryCarryTotal(ListCollectionView view) : base(view) { }

        private int myitemcount;
        public int ItemCount { set { myitemcount = value; } get { return myitemcount; } }
        private decimal mytotalcost;
        public decimal TotalCost { set { mytotalcost = value; } get { return mytotalcost; } }
        private decimal myofficialweight;
        public decimal OfficialWeight { set { myofficialweight = value; } get { return myofficialweight; } }
        private decimal myactualweight;
        public decimal ActualWeight { set { myactualweight = value; } get { return myactualweight; } }
        private decimal myvolume;
        public decimal Volume { set { myvolume = value; } get { return myvolume; } }
        private decimal mycellnumber;
        public decimal CellNumber { set { mycellnumber = value; } get { return mycellnumber; } }

        protected override void Item_ValueChangedHandler(DeliveryCarryVM sender, ValueChangedEventArgs<object> e)
        {
            decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
            switch (e.PropertyName)
            {
                case "ActualWeight":
                    myactualweight += newvalue - oldvalue;
                    PropertyChangedNotification("ActualWeight");
                    break;
                case "CellNumber":
                    mycellnumber += newvalue - oldvalue;
                    PropertyChangedNotification("CellNumber");
                    break;
                case "OfficialWeight":
                    myofficialweight += newvalue - oldvalue;
                    PropertyChangedNotification("OfficialWeight");
                    break;
                case "TotalCost":
                    mytotalcost += newvalue - oldvalue;
                    PropertyChangedNotification("TotalCost");
                    break;
                case "Volume":
                    myvolume += newvalue - oldvalue;
                    PropertyChangedNotification("Volume");
                    break;
            }
        }
        protected override void ValuesReset()
        {
            myitemcount = 0;
            mytotalcost = 0M;
            myofficialweight = 0M;
            myactualweight = 0M;
            myvolume = 0M;
            mycellnumber = 0M;
        }
        protected override void ValuesPlus(DeliveryCarryVM item)
        {
            myitemcount++;
            mytotalcost = mytotalcost + (item.DomainObject.TotalCost ?? 0M);
            myofficialweight = myofficialweight + (item.Request.OfficialWeight ?? 0M);
            myactualweight = myactualweight + (item.Request.ActualWeight ?? 0M);
            myvolume = myvolume + (item.Request.Volume ?? 0M);
            mycellnumber = mycellnumber + (item.Request.CellNumber ?? 0M);
            //base.ValuesPlus(item);
        }
        protected override void ValuesMinus(DeliveryCarryVM item)
        {
            myitemcount--;
            mytotalcost = mytotalcost - (item.DomainObject.TotalCost ?? 0M);
            myofficialweight = myofficialweight - (item.Request.OfficialWeight ?? 0M);
            myactualweight = myactualweight - (item.Request.ActualWeight ?? 0M);
            myvolume = myvolume - (item.Request.Volume ?? 0M);
            mycellnumber = mycellnumber - (item.Request.CellNumber ?? 0M);
            //base.ValuesMinus(item);
        }
        protected override void PropertiesChangedNotifycation()
        {
            this.PropertyChangedNotification("ItemCount");
            this.PropertyChangedNotification("TotalCost");
            this.PropertyChangedNotification("OfficialWeight");
            this.PropertyChangedNotification("ActualWeight");
            this.PropertyChangedNotification("Volume");
            this.PropertyChangedNotification("CellNumber");
        }
        //protected override void AttachValueChangedEvent(DeliveryCarryVM item, ValueChangedEventHandler<object> handler)
        //{
        //    item.ValueChanged+=handler;
        //}
        //protected override void DetachValueChangedEvent(DeliveryCarryVM item, ValueChangedEventHandler<object> handler)
        //{
        //    item.ValueChanged -= handler;
        //}
    }
}
