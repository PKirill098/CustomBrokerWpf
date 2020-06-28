using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Linq;
using KirillPolyanskiy.DataModelClassLibrary.Interfaces;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class DeliveryCar : lib.DomainBaseStamp,IValueChanged<object>
    {
        public DeliveryCar(int id, long stamp, lib.DomainObjectState domainstate
            , int? number, string invoicenumber, DateTime? invoicedate, decimal? invoicesum, string carnumber, string company, DateTime? deliverydate, Importer importer, lib.ReferenceSimpleItem state, string note
            ) : base(id, stamp, null, null, domainstate)
        {
            mynumber = number;
            myinvoicenumber = invoicenumber;
            myinvoicedate = invoicedate;
            myinvoicesum = invoicesum;
            mycarnumber = carnumber;
            mycompany = company;
            mydeliverydate = deliverydate;
            myimporter = importer;
            mystate = state;
            mynote = note;
            if (this.DomainState == lib.DomainObjectState.Added)
                mycarry = new ObservableCollection<DeliveryCarry>();
            else
            {
                DeliveryCarryDBM rdbm = new DeliveryCarryDBM();
                rdbm.Car = this;
                rdbm.FillAsyncCompleted = () => { if (rdbm.Errors.Count > 0) throw new Exception(rdbm.ErrorMessage); else Count(); mycarry.CollectionChanged += Carry_CollectionChanged; };
                rdbm.FillAsync();
                mycarry = rdbm.Collection;
            }
            this.PropertyChanged += RefreshViewPropertyChanged;
        }

        public DeliveryCar() : this(id:lib.NewObjectId.NewId, stamp:0, domainstate:lib.DomainObjectState.Added
            ,number:null, invoicenumber:null, invoicedate:null, invoicesum:null, carnumber:null, company: "Столица логистики", deliverydate:null, importer:null, state:CustomBrokerWpf.References.DeliveryCarStates.GetDefault(), note:null
            ) { }

        private string mycarnumber;
        public string CarNumber
        {
            set { SetProperty<string>(ref mycarnumber, value); }
            get { return mycarnumber; }
        }
        private string mycompany;
        public string Company
        {
            set { SetProperty<string>(ref mycompany, value); }
            get { return mycompany; }
        }
        private DateTime? mydeliverydate;
        public DateTime? DeliveryDate
        {
            set { SetProperty<DateTime?>(ref mydeliverydate, value); }
            get { return mydeliverydate; }
        }
        private Importer myimporter;
        public Importer Importer
        {
            set { SetProperty<Importer>(ref myimporter, value); }
            get { return myimporter; }
        }
        private DateTime? myinvoicedate;
        public DateTime? InvoiceDate
        {
            set { SetProperty<DateTime?>(ref myinvoicedate, value); }
            get { return myinvoicedate; }
        }
        private string myinvoicenumber;
        public string InvoiceNumber
        {
            set { SetProperty<string>(ref myinvoicenumber, value); }
            get { return myinvoicenumber; }
        }
        private decimal? myinvoicesum;
        public decimal? InvoiceSum
        {
            set {
                decimal? oldvalue = myinvoicesum;
                Action notify = () => { PropertyChangedNotification("Price"); this.OnValueChanged("InvoiceSum", (oldvalue??0M), (value??0M)); };
                SetProperty<decimal?>(ref myinvoicesum, value, notify); }
            get { return myinvoicesum; }
        }
        private string mynote;
        public string Note
        {
            set { SetProperty<string>(ref mynote, value); }
            get { return mynote; }
        }
        private int? mynumber;
        public int? Number
        {
            set { SetProperty<int?>(ref mynumber, value); }
            get { return mynumber; }
        }
        private lib.ReferenceSimpleItem mystate;
        public lib.ReferenceSimpleItem State
        {
            set { SetProperty<lib.ReferenceSimpleItem>(ref mystate, value,()=> { CustomBrokerWpf.References.CarsViewCollector.RefreshItem(this); }); }
            get { return mystate; }
        }

        private decimal myactualweight;
        public decimal ActualWeight
        {
            get { return myactualweight; }
        }
        private decimal mycellnumber;
        public decimal CellNumber
        { get { return mycellnumber; } }
        private decimal myofficialweight;
        public decimal OfficialWeight
        { get { return myofficialweight; } }
        private decimal myvolume;
        public decimal Volume
        { get { return myvolume; } }
        public decimal? Price
        { get { return myofficialweight==0M ? (decimal?)null : decimal.Divide((myinvoicesum ?? 0M),myofficialweight); } }
        private ObservableCollection<DeliveryCarry> mycarry;

        public ObservableCollection<DeliveryCarry> Carry
        {
            get
            {
                if (mycarry == null)
                {
                    DeliveryCarryDBM rdbm = new DeliveryCarryDBM();
                    rdbm.Car = this;
                    rdbm.FillAsyncCompleted = () => { if (rdbm.Errors.Count > 0) throw new Exception(rdbm.ErrorMessage); else Count(); mycarry.CollectionChanged += Carry_CollectionChanged; };
                    rdbm.FillAsync();
                    mycarry = rdbm.Collection;
                }
                return mycarry;
            }
        }
        private void Carry_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                Count();
            else
            {
                if (e.NewItems != null)
                    foreach (DeliveryCarry item in e.NewItems)
                    {
                        if (item.Car1 == this || item.Car2 == this || item.Car3 == this)
                        { ValuesPlus(item); CarryPropertiesChangedNotifycation();
                            OnValueChanged("ActualWeight", 0M, myactualweight);
                            OnValueChanged("CellNumber", 0M, mycellnumber);
                            OnValueChanged("OfficialWeight", 0M, myofficialweight);
                            OnValueChanged("Volume", 0M, myvolume);
                        }
                    }
                if (e.OldItems != null)
                    foreach (DeliveryCarry item in e.OldItems)
                    {
                        ValuesMinus(item); CarryPropertiesChangedNotifycation();
                        OnValueChanged("ActualWeight", myactualweight, 0M);
                        OnValueChanged("CellNumber", mycellnumber, 0M);
                        OnValueChanged("OfficialWeight", myofficialweight, 0M);
                        OnValueChanged("Volume", myvolume, 0M);

                    }
            }
        }
        private void Count()
        {
            myactualweight = 0M;
            mycellnumber = 0M;
            myofficialweight = 0M;
            myvolume = 0M;
            foreach (DeliveryCarry item in mycarry)
            {
                if (item.Car1==this || item.Car2 == this || item.Car3 == this)
                    ValuesPlus(item);
            }
            CarryPropertiesChangedNotifycation();
            OnValueChanged("ActualWeight", 0M, myactualweight);
            OnValueChanged("CellNumber", 0M, mycellnumber);
            OnValueChanged("OfficialWeight", 0M, myofficialweight);
            OnValueChanged("Volume", 0M, myvolume);
        }
        private void ValuesPlus(DeliveryCarry item)
        {
            myactualweight += item.Request.ActualWeight ?? 0M;
            mycellnumber += item.Request.CellNumber ?? 0;
            myofficialweight += item.Request.OfficialWeight ?? 0M;
            myvolume += item.Request.Volume ?? 0M;
        }
        private void ValuesMinus(DeliveryCarry item)
        {
            myactualweight -= item.Request.ActualWeight ?? 0M;
            mycellnumber -= item.Request.CellNumber ?? 0;
            myofficialweight -= item.Request.OfficialWeight ?? 0M;
            myvolume -= item.Request.Volume ?? 0M;
        }
        private void CarryPropertiesChangedNotifycation()
        {
            PropertyChangedNotification("ActualWeight");
            PropertyChangedNotification("CellNumber");
            PropertyChangedNotification("DifferenceWeight");
            PropertyChangedNotification("Invoice");
            PropertyChangedNotification("InvoiceDiscount");
            PropertyChangedNotification("OfficialWeight");
            PropertyChangedNotification("Volume");
            PropertyChangedNotification("Price");
        }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            DeliveryCar newitem=(DeliveryCar)sample;
            this.CarNumber = newitem.CarNumber;
            this.Company = newitem.Company;
            this.DeliveryDate = newitem.DeliveryDate;
            this.Importer = newitem.Importer;
            this.InvoiceDate = newitem.InvoiceDate;
            this.InvoiceNumber = newitem.InvoiceNumber;
            this.InvoiceSum = newitem.InvoiceSum;
            this.Note = newitem.Note;
            this.Number = newitem.Number;
            this.State = newitem.State;
        }

        private void RefreshViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName=="DomainState")
                CustomBrokerWpf.References.CarsViewCollector.RefreshItem(this);
        }

        public event ValueChangedEventHandler<object> ValueChanged;
        public void OnValueChanged(string propertyname, object oldvalue, object newvalue)
        {
            if (this.ValueChanged != null) ValueChanged(this, new ValueChangedEventArgs<object>(propertyname, oldvalue, newvalue));
        }
    }

    public class DeliveryCarDBM : lib.DBManagerStamp<DeliveryCar>
    {
        public DeliveryCarDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "delivery.DeliveryCars_sp";
            InsertCommandText = "delivery.DeliveryCarsAdd_sp";
            UpdateCommandText = "delivery.DeliveryCarsUpd_sp";
            DeleteCommandText = "delivery.DeliveryCarsDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@id", System.Data.SqlDbType.Int),
                new SqlParameter("@all", System.Data.SqlDbType.Bit)
            };
            base.SelectParams[1].Value = 0;
            myinsertparams = new SqlParameter[] { myinsertparams[0], new SqlParameter("@number", System.Data.SqlDbType.Int) };
            myinsertparams[1].Direction = System.Data.ParameterDirection.InputOutput;
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@number", System.Data.SqlDbType.Int)
                ,new SqlParameter("@numbertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicenumbertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicedatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicesumtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@carnumbertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@companytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@deliverydatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@importertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@statetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@notetrue", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0]
                ,new SqlParameter("@invoicenumber", System.Data.SqlDbType.NVarChar,5)
                ,new SqlParameter("@invoicedate", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@invoicesum", System.Data.SqlDbType.Money)
                ,new SqlParameter("@carnumber", System.Data.SqlDbType.NVarChar,10)
                ,new SqlParameter("@company", System.Data.SqlDbType.NVarChar,50)
                ,new SqlParameter("@deliverydate", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@importer", System.Data.SqlDbType.Int)
                ,new SqlParameter("@state", System.Data.SqlDbType.Int)
                ,new SqlParameter("@note", System.Data.SqlDbType.NVarChar,200)
            };
        }

        internal bool isAll
        {
            set { base.SelectParams[1].Value = value; base.SelectParams[0].Value = null; }
            get { return (bool)base.SelectParams[1].Value; }
        }

        protected override DeliveryCar CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            DeliveryCar car = new DeliveryCar(
                reader.GetInt32(0),reader.GetInt64(1),lib.DomainObjectState.Unchanged
                , reader.GetInt32(reader.GetOrdinal("number"))
                , reader.IsDBNull(reader.GetOrdinal("invoicenumber")) ? null : reader.GetString(reader.GetOrdinal("invoicenumber"))
                , reader.IsDBNull(reader.GetOrdinal("invoicedate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("invoicedate"))
                , reader.IsDBNull(reader.GetOrdinal("invoicesum")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("invoicesum"))
                , reader.IsDBNull(reader.GetOrdinal("carnumber")) ? null : reader.GetString(reader.GetOrdinal("carnumber"))
                , reader.IsDBNull(reader.GetOrdinal("company")) ? null : reader.GetString(reader.GetOrdinal("company"))
                , reader.IsDBNull(reader.GetOrdinal("deliverydate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("deliverydate"))
                , reader.IsDBNull(reader.GetOrdinal("importer")) ? null : CustomBrokerWpf.References.Importers.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("importer")))
                , CustomBrokerWpf.References.DeliveryCarStates.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("state")))
                , reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString(reader.GetOrdinal("note"))
                );
            return CustomBrokerWpf.References.DeliveryCarStore.UpdateItem(car);
        }
        protected override void GetOutputSpecificParametersValue(DeliveryCar item)
        {
            if(item.DomainState==lib.DomainObjectState.Added)
                item.Number = (int?)myinsertparams[1].Value;
        }
        protected override bool SaveChildObjects(DeliveryCar item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(DeliveryCar item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
           return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override bool SetSpecificParametersValue(DeliveryCar item)
        {
            myinsertparams[1].Value = item.Number;
            foreach (SqlParameter par in myupdateparams)
            {
                switch (par.ParameterName)
                {
                    case "@number":
                        par.Value = item.Number;
                        break;
                    case "@numbertrue":
                        par.Value = item.HasPropertyOutdatedValue("Number");
                        break;
                    case "@invoicenumbertrue":
                        par.Value = item.HasPropertyOutdatedValue("InvoiceNumber");
                        break;
                    case "@invoicedatetrue":
                        par.Value = item.HasPropertyOutdatedValue("InvoiceDate");
                        break;
                    case "@invoicesumtrue":
                        par.Value = item.HasPropertyOutdatedValue("InvoiceSum");
                        break;
                    case "@carnumbertrue":
                        par.Value = item.HasPropertyOutdatedValue("CarNumber");
                        break;
                    case "@companytrue":
                        par.Value = item.HasPropertyOutdatedValue("Company");
                        break;
                    case "@deliverydatetrue":
                        par.Value = item.HasPropertyOutdatedValue("DeliveryDate");
                        break;
                    case "@importertrue":
                        par.Value = item.HasPropertyOutdatedValue("Importer");
                        break;
                    case "@statetrue":
                        par.Value = item.HasPropertyOutdatedValue("State");
                        break;
                    case "@notetrue":
                        par.Value = item.HasPropertyOutdatedValue("Note");
                        break;
                }
            }
            foreach (SqlParameter par in myinsertupdateparams)
            {
                switch (par.ParameterName)
                {
                    case "@invoicenumber":
                        par.Value = item.InvoiceNumber;
                        break;
                    case "@invoicedate":
                        par.Value = item.InvoiceDate;
                        break;
                    case "@invoicesum":
                        par.Value = item.InvoiceSum;
                        break;
                    case "@carnumber":
                        par.Value = item.CarNumber;
                        break;
                    case "@company":
                        par.Value = item.Company;
                        break;
                    case "@deliverydate":
                        par.Value = item.DeliveryDate;
                        break;
                    case "@importer":
                        par.Value = item.Importer?.Id;
                        break;
                    case "@state":
                        par.Value = item.State.Id;
                        break;
                    case "@note":
                        par.Value = item.Note;
                        break;
                }
            }
            return true;
        }
        protected override bool LoadObjects()
        { return true; }
    }

    internal class DeliveryCarStore : lib.DomainStorageLoad<DeliveryCar, DeliveryCarDBM>
    {
        public DeliveryCarStore(DeliveryCarDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(DeliveryCar olditem, DeliveryCar newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class DeliveryCarVM : lib.ViewModelErrorNotifyItem<DeliveryCar>, lib.Interfaces.ITotalValuesItem
    {
        public DeliveryCarVM(DeliveryCar domen):base(domen)
        {
            ValidetingProperties.AddRange(new string[] { "Number" });
            DeleteRefreshProperties.AddRange(new string[] { "CarNumber", "Company", "DeliveryDate", "Importer", "InvoiceDate", "InvoiceNumber", "InvoiceSum", "Note", "Number", "State" });
            domen.ValueChanged += this.Domen_ValueChanged;
            InitProperties();
        }

        public DeliveryCarVM():this(new DeliveryCar()) { }

        public string CarNumber
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CarNumber, value)))
                {
                    string name = "CarNumber";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CarNumber);
                    ChangingDomainProperty = name; this.DomainObject.CarNumber = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CarNumber : null; }
        }
        public string Company
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Company, value)))
                {
                    string name = "Company";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Company);
                    ChangingDomainProperty = name; this.DomainObject.Company = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Company : null; }
        }
        public DateTime? DeliveryDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.DeliveryDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.DeliveryDate.Value, value.Value))))
                {
                    string name = "DeliveryDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DeliveryDate);
                    ChangingDomainProperty = name; this.DomainObject.DeliveryDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DeliveryDate : null; }
        }
        public Importer Importer
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Importer, value)))
                {
                    string name = "Importer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Importer);
                    ChangingDomainProperty = name; this.DomainObject.Importer = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Importer : null; }
        }
        public DateTime? InvoiceDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.InvoiceDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.InvoiceDate.Value, value.Value))))
                {
                    string name = "InvoiceDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.InvoiceDate);
                    ChangingDomainProperty = name; this.DomainObject.InvoiceDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.InvoiceDate : null; }
        }
        public string InvoiceNumber
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.InvoiceNumber, value)))
                {
                    string name = "InvoiceNumber";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.InvoiceNumber);
                    ChangingDomainProperty = name; this.DomainObject.InvoiceNumber = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.InvoiceNumber : null; }
        }
        public decimal? InvoiceSum
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.InvoiceSum.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.InvoiceSum.Value, value.Value))))
                {
                    string name = "InvoiceSum";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.InvoiceSum);
                    ChangingDomainProperty = name; this.DomainObject.InvoiceSum = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.InvoiceSum : null; }
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
        public int? Number
        {
            set
            {
                if (!this.IsReadOnly && value.HasValue && (!(this.DomainObject.Number.HasValue && int.Equals(this.DomainObject.Number.Value, value.Value))))
                {
                    string name = "Number";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Number);
                    ChangingDomainProperty = name; this.DomainObject.Number = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Number : null; }
        }
        public lib.ReferenceSimpleItem State
        {
            set
            {
                if (!this.IsReadOnly && !int.Equals(this.DomainObject.State, value))
                {
                    string name = "State";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.State);
                    ChangingDomainProperty = name; this.DomainObject.State = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.State : null; }
        }
        public int Sort
        { get { return this.DomainObject.Id < 0 ? -this.DomainObject.Id : this.DomainObject.Id; } }

        public bool ProcessedIn { set; get; }
        public bool ProcessedOut { set; get; }
        private bool myselected;
        public bool Selected
        { set { bool oldvalue = myselected; myselected = value; this.OnValueChanged("Selected", oldvalue, value);} get { return myselected; } }

        public decimal? ActualWeight
        {
            get { return this.IsEnabled ? this.DomainObject.ActualWeight : (decimal?)null; }
        }
        public decimal? CellNumber
        { get { return this.IsEnabled ? this.DomainObject.CellNumber : (decimal?)null; } }
        public decimal? OfficialWeight
        { get { return this.IsEnabled ? this.DomainObject.OfficialWeight : (decimal?)null; } }
        public decimal? Volume
        { get { return this.IsEnabled ? this.DomainObject.Volume : (decimal?)null; } }
        public decimal? Price
        { get { return this.IsEnabled ? this.DomainObject.Price : (decimal?)null; } }

        private void Domen_ValueChanged(object sender, ValueChangedEventArgs<object> e)
        {
            OnValueChanged(e.PropertyName, e.OldValue, e.NewValue);
        }
        protected override void DomainObjectPropertyChanged(string property)
        {        }
        protected override void InitProperties()
        {
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "CarNumber":
                    this.DomainObject.CarNumber = (string)value;
                    break;
                case "Company":
                    this.DomainObject.Company = (string)value;
                    break;
                case "DeliveryDate":
                    this.DomainObject.DeliveryDate = (DateTime?)value;
                    break;
                case "Importer":
                    this.DomainObject.Importer = (Importer)value;
                    break;
                case "InvoiceDate":
                    this.DomainObject.InvoiceDate = (DateTime?)value;
                    break;
                case "InvoiceNumber":
                    this.DomainObject.InvoiceNumber = (string)value;
                    break;
                case "InvoiceSum":
                    this.DomainObject.InvoiceSum = (decimal?)value;
                    break;
                case "Note":
                    this.DomainObject.Note = (string)value;
                    break;
                case "Number":
                    this.DomainObject.Number = (int?)value;
                    break;
                case "State":
                    this.DomainObject.State = (lib.ReferenceSimpleItem)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Number":
                    if (this.Number == null)
                    {
                        errmsg = "Необходимо указать номер перевозки!";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            else if (isvalid) ClearErrorMessageForProperty(propertyname);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return false;
        }
    }

    public class DeliveryCarSynchronizer : lib.ModelViewCollectionsSynchronizer<DeliveryCar, DeliveryCarVM>
    {
        protected override DeliveryCar UnWrap(DeliveryCarVM wrap)
        {
            return wrap.DomainObject as DeliveryCar;
        }
        protected override DeliveryCarVM Wrap(DeliveryCar fill)
        {
            return new DeliveryCarVM(fill);
        }
    }

    public class DeliveryCarViewCommand : lib.ViewModelViewCommand
    {
        internal DeliveryCarViewCommand()
        {
            mycdbm = new DeliveryCarDBM();
            mydbm = mycdbm;
            mycdbm.isAll = false;
            mycdbm.FillAsyncCompleted = () => { if (mycdbm.Errors.Count > 0) OpenPopup(mycdbm.ErrorMessage, true); };
            mycdbm.FillAsync();
            mysync = new DeliveryCarSynchronizer();
            mysync.DomainCollection = mycdbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            mysync.DomainCollection.CollectionChanged += Collection_CollectionChanged;
            mytotal = new DeliveryCarTotal(myview);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
        }

        public bool IsAll
        {
            set { mycdbm.isAll = value; this.SaveRefresh.Execute(null); }
            get { return mycdbm.isAll; }
        }
        //private SQLFilter myfilter;
        //internal SQLFilter Filter
        //{ get { return myfilter; } }
        private DeliveryCarDBM mycdbm;
        private DeliveryCarSynchronizer mysync;
        private System.ComponentModel.BackgroundWorker mybw;
        private ExcelImportWin myexcelimportwin;
        private DeliveryCarTotal mytotal;
        public DeliveryCarTotal Total { get { return mytotal; } }
        internal ObservableCollection<DeliveryCar> Cars
        { get { return mysync.DomainCollection; } }
        private ListCollectionView myimporters;
        public ListCollectionView Importers
        {
            get
            {
                if (myimporters == null)
                {
                    myimporters = new ListCollectionView(CustomBrokerWpf.References.Importers);
                    myimporters.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myimporters;
            }
        }
        private ListCollectionView mystates;
        public ListCollectionView States
        {
            get
            {
                if (mystates == null)
                {
                    mystates = new ListCollectionView(CustomBrokerWpf.References.DeliveryCarStates);
                    mystates.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                }
                return mystates;
            }
        }

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
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star),MinWidth=20 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Auto), SharedSizeGroup = "b" });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star), MinWidth = 20 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Auto), SharedSizeGroup = "b" });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star), MinWidth = 20 });
                ListBox list = new ListBox();
                list.Style = list.FindResource("CheckBoxListStyle") as System.Windows.Style;
                list.Items.Add("№"); list.Items.Add("№ счета"); list.Items.Add("№ АМ"); list.Items.Add("ТК"); list.Items.Add("Дата счета"); list.Items.Add("Дата перевозки"); list.Items.Add("Покупатель"); list.Items.Add("Сумма счета"); list.Items.Add("Вес Д"); list.Items.Add("Вес Ф"); list.Items.Add("Объем"); list.Items.Add("Мест"); list.Items.Add("Стоим за кг"); list.Items.Add("Статус"); list.Items.Add("Примечания");
                list.SelectAll();
                list.SetValue(Grid.ColumnSpanProperty, 5);
                list.Margin = new System.Windows.Thickness(2D, 2D, 2D, 10D);
                grid.Children.Add(list);
                Button bok = new Button() { Content="OK", IsDefault=true };
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
                int row = 2,column = 1;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                exWh.Name = "Транспорт ";
                Excel.Range r;

                exWh.Rows[1, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                foreach (string name in arg.Columns)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        exWh.Cells[1, column] = name;
                        switch(name)
                        {
                            case "№":
                            case "№ счета":
                            case "№ АМ":
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                exWh.Columns[column, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                                break;
                            case "Примечания":
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                break;
                            case "Дата счета":
                            case "Дата перевозки":
                                //exWh.Columns[column, Type.Missing].NumberFormat = "dd/mm/yy";
                                exWh.Columns[column, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                                break;
                            case "Сумма счета":
                            case "Вес Д":
                            case "Вес Ф":
                            case "Объем":
                            case "Стоим за кг":
                                try { exWh.Columns[column, Type.Missing].NumberFormat = "# #0,00"; } catch { }
                                break;
                        }
                        column++;
                    }
                    else
                        break;
                }
                worker.ReportProgress(2+(int)(decimal.Divide(1, arg.Count + 1) * 100));
                foreach (DeliveryCarVM item in arg.SelectedItems.OfType<DeliveryCarVM>())
                {
                    column = 1;
                    foreach (string name in arg.Columns)
                    {
                        switch (name)
                        {
                            case "№":
                                exWh.Cells[row, column] = item.Number;
                                break;
                            case "№ счета":
                                exWh.Cells[row, column] = item.InvoiceNumber;
                                break;
                            case "№ АМ":
                                exWh.Cells[row, column] = item.CarNumber;
                                break;
                            case "ТК":
                                exWh.Cells[row, column] = item.Company;
                                break;
                            case "Дата счета":
                                exWh.Cells[row, column] = item.InvoiceDate;
                                break;
                            case "Дата перевозки":
                                exWh.Cells[row, column] = item.DeliveryDate;
                                break;
                            case "Покупатель":
                                exWh.Cells[row, column] = item.Importer?.Name;
                                break;
                            case "Сумма счета":
                                exWh.Cells[row, column] = item.InvoiceSum;
                                break;
                            case "Вес Д":
                                exWh.Cells[row, column] = item.OfficialWeight;
                                break;
                            case "Вес Ф":
                                exWh.Cells[row, column] = item.ActualWeight;
                                break;
                            case "Объем":
                                exWh.Cells[row, column] = item.Volume;
                                break;
                            case "Мест":
                                exWh.Cells[row, column] = item.CellNumber;
                                break;
                            case "Стоим за кг":
                                exWh.Cells[row, column] = item.Price;
                                break;
                            case "Статус":
                                exWh.Cells[row, column] = item.State?.Name;
                                break;
                            case "Примечания":
                                exWh.Cells[row, column] = item.Note;
                                break;
                        }
                        column++;
                    }

                    worker.ReportProgress(2 + (int)(decimal.Divide(row+1, arg.Count+1) * 100));
                    row++;
                }

                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, column-1]];
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

        protected override void AddData(object parametr)
        {
            base.AddData(parametr);
            CustomBrokerWpf.References.DeliveryCars.Add((myview.CurrentItem as DeliveryCarVM).DomainObject);
        }
        protected override bool CanAddData(object parametr)
        {
            return !(myview.IsAddingNew | myview.IsEditingItem);
        }
        protected override bool CanDeleteData(object parametr)
        {
            return myview.CurrentItem != null && myview.CurrentItem is DeliveryCarVM;
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
        { CustomBrokerWpf.References.CarsViewCollector.RefreshViews(); }
        protected override void RefreshData(object parametr)
        {
            mycdbm.FillAsync();
            DeliveryCarryDBM cdbm = new DeliveryCarryDBM();
            cdbm.FillAsync();
        }
        protected override void RejectChanges(object parametr)
        {
            System.Collections.IList rejects;
            if (parametr is System.Collections.IList && (parametr as System.Collections.IList).Count > 0)
                rejects = parametr as System.Collections.IList;
            else
                rejects = mysync.ViewModelCollection;

            System.Collections.Generic.List<DeliveryCarVM> deleted = new System.Collections.Generic.List<DeliveryCarVM>();
            foreach (object item in rejects)
            {
                if (item is DeliveryCarVM)
                {
                    DeliveryCarVM ritem = item as DeliveryCarVM;
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
            foreach (DeliveryCarVM delitem in deleted)
            {
                mysync.ViewModelCollection.Remove(delitem);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
            }
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new SortDescription("Sort", ListSortDirection.Descending));
        }

        private void Collection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems!=null)
            foreach (DeliveryCar item in e.NewItems)
                if(item.DomainState==lib.DomainObjectState.Added)
                    CustomBrokerWpf.References.DeliveryCars.Add(item);
        }
    }

    public class DeliveryCarTotal : lib.TotalCollectionValues<DeliveryCarVM>
    {
        internal DeliveryCarTotal(ListCollectionView view) : base(view) { }

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

        protected override void Item_ValueChangedHandler(DeliveryCarVM sender, ValueChangedEventArgs<object> e)
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
                case "InvoiceSum":
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
        protected override void ValuesPlus(DeliveryCarVM item)
        {
            myitemcount++;
            mytotalcost = mytotalcost + (item.DomainObject.InvoiceSum ?? 0M);
            myofficialweight = myofficialweight + item.DomainObject.OfficialWeight;
            myactualweight = myactualweight + item.DomainObject.ActualWeight;
            myvolume = myvolume + item.DomainObject.Volume;
            mycellnumber = mycellnumber + item.DomainObject.CellNumber;
            //base.ValuesPlus(item);
        }
        protected override void ValuesMinus(DeliveryCarVM item)
        {
            myitemcount--;
            mytotalcost = mytotalcost - (item.DomainObject.InvoiceSum ?? 0M);
            myofficialweight = myofficialweight - item.DomainObject.OfficialWeight;
            myactualweight = myactualweight - item.DomainObject.ActualWeight;
            myvolume = myvolume - item.DomainObject.Volume;
            mycellnumber = mycellnumber - item.DomainObject.CellNumber;
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
        //protected override void AttachValueChangedEvent(DeliveryCarVM item, ValueChangedEventHandler<object> handler)
        //{
        //    item.ValueChanged += handler;
        //}
        //protected override void DetachValueChangedEvent(DeliveryCarVM item, ValueChangedEventHandler<object> handler)
        //{
        //    item.ValueChanged -= handler;
        //}
    }
}
