using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class RequestCustomerLegal : lib.DomainBaseStamp
    {
        public RequestCustomerLegal(int id, long stamp, lib.DomainObjectState state
            , Request request, CustomerLegal customerlegal, bool selected
            , decimal? actualweight,int? cellnumber, DateTime? currencydate, decimal? currencyrate, decimal? invoice, decimal? invoicediscount, decimal? officialweight,decimal? volume
            ) : base(id, stamp,null,null, state)
        {
            myrequest = request;
            mycustomerlegal = customerlegal;
            myselected = selected;
            myactualweight = actualweight;
            mycellnumber = cellnumber;
            mycurrencydate = currencydate;
            mycurrencyrate = currencyrate;
            myinvoice = invoice;
            //myinvoicediscount = invoicediscount;
            myofficialweight = officialweight;
            myvolume = volume;

            if(myrequest!=null) myrequest.PropertyChanged += Request_PropertyChanged;
            this.PropertyChanged += RequestCustomerLegal_PropertyChanged;
            this.LoadedPropertiesNotification.Add(nameof(this.CustomerLegal));
            this.LoadedPropertiesNotification.Add(nameof(this.Request));
        }
        private void Request_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DTRate":
                case "SellingMarkupRate":
                    this.PropertyChangedNotification("SellingMarkup");
                    break;
            }
        }

        decimal? myactualweight;
        public decimal? ActualWeight
        {
            set
            {
                base.SetProperty<decimal?>(ref myactualweight, value);
            }
            get { return myofficialweight; }
        }
        private int? mycellnumber;
        public int? CellNumber
        {
            set
            {
                base.SetProperty<int?>(ref mycellnumber, value);
            }
            get { return mycellnumber; }
        }
        DateTime? mycurrencydate;
        public DateTime? CurrencyDate
        {
            set
            {
                base.SetProperty<DateTime?>(ref mycurrencydate, value);
            }
            get { return mycurrencydate; }
        }
        decimal? mycurrencyrate;
        public decimal? CurrencyRate
        {
            set
            {
                base.SetProperty<decimal?>(ref mycurrencyrate, value);
            }
            get { return mycurrencyrate; }
        }
        private CustomerLegal mycustomerlegal;
        public CustomerLegal CustomerLegal
        {
            set { SetProperty<CustomerLegal>(ref mycustomerlegal, value); }
            get { return mycustomerlegal; }
        }
        private CustomsInvoice mycustomsinvoice;
        internal bool CustomsInvoiceIsNull
        { get { return mycustomsinvoice == null; } }
        public CustomsInvoice CustomsInvoice
        {
            get
            {
                if (mycustomsinvoice == null && this.Request?.Parcel != null)
                {
                    mycustomsinvoice = CustomBrokerWpf.References.CustomsInvoiceStore.GetItemLoad(this.CustomerLegal, this.Request?.Importer, this.Request.Parcel);
                    if (mycustomsinvoice == null)
                        mycustomsinvoice = new CustomsInvoice(this.CustomerLegal, this.Request.Importer, this.Request.Parcel);
                }
                return mycustomsinvoice;
            }
        }
        public decimal? DTSum
        {
            set
            {
                if (value.HasValue & myprepays.Count == 0)
                {
                    myprepays.Add(this.GetNewPrepay());
                }
                if (myprepays.Count == 1)
                    myprepays[0].DTSum = value??0M;
            }
            get { return myprepays?.Sum<PrepayCustomerRequest>((PrepayCustomerRequest item) => { return item.DTSum; }); }
        }
        private Request myrequest;
        public Request Request
        {
            set { SetProperty<Request>(ref myrequest, value, () => { myrequest.PropertyChanged += Request_PropertyChanged; }); }
            get { return myrequest; }
        }
        private bool myselected;
        public bool Selected
        {
            set { SetProperty<bool>(ref myselected, value, () => {  }); } // PaymentsDelete();if (value) SingleSelected();
            get { return myselected; }
        }
        decimal? myinvoice;
        public decimal? Invoice
        {
            set
            {
                decimal oldvalue = myinvoice??0M;
                Action notify = () =>
                {
                    base.PropertyChangedNotification("InsuranceCost");
                    base.PropertyChangedNotification("InsurancePay");
                    UpdatedRequest("Invoice", oldvalue);
                };
                base.SetProperty<decimal?>(ref myinvoice, value, notify);
            }
            get { return myinvoice; }
        }
        //decimal? myinvoicediscount;
        public decimal? InvoiceDiscount
        {
            set
            {
                if (value.HasValue & myprepays.Count == 0)
                {
                    myprepays.Add(this.GetNewPrepay());
                }
                if (myprepays.Count == 1)
                {
                    //decimal oldvalue = this.InvoiceDiscount??0M;
                    myprepays[0].EuroSum = value.HasValue ? value.Value : 0M;
                    //this.UpdatedRequest("InvoiceDiscount", oldvalue);
                    //this.PropertyChangedNotification("InvoiceDiscountAdd2per");
                    this.Request.PropertyChangedNotification("InvoiceDiscount");
                }
                else if((value??0M)==0M)
                    foreach(PrepayCustomerRequest item in myprepays)
                        item.EuroSum = 0M;
            }
            get { return myprepays?.Sum<PrepayCustomerRequest>((PrepayCustomerRequest item)=> { return item.EuroSum; }); }
        }
        public decimal? InvoiceDiscountAdd2per
        {
            get { return this.InvoiceDiscount * 1.02M; }
        }
        decimal? myofficialweight;
        public decimal? OfficialWeight
        {
            set
            {
                decimal oldvalue = myofficialweight ?? 0M;
                Action notify = () =>
                {
                    UpdatedRequest("OfficialWeight", oldvalue);
                };
                base.SetProperty<decimal?>(ref myofficialweight, value, notify);
            }
            get { return myofficialweight; }
        }
        internal bool RequestUpdating { set; get; }
        public decimal? SellingMarkup
        {
            get { return this.InvoiceDiscount * this.Request.DTRate * this.Request.SellingMarkupRate; }
        }
        private decimal? myvolume;
        public decimal? Volume
        {
            set
            {
                base.SetProperty<decimal?>(ref myvolume, value);
            }
            get { return myvolume; }
        }

        private ObservableCollection<PrepayCustomerRequest> myprepays; //created at boot
        internal ObservableCollection<PrepayCustomerRequest> Prepays
        {
            set
            {
                myprepays = value;
                this.PropertyChangedNotification(nameof(this.Prepays));
                this.PropertyChangedNotification(nameof(this.InvoiceDiscount));
                foreach (PrepayCustomerRequest item in myprepays)
                { item.PropertyChangedNotification(nameof(PrepayCustomerRequest.FinalInvoiceRubSumPaid)); item.PropertyChangedNotification(nameof(PrepayCustomerRequest.CustomerBalance)); }
            }
            get
            {
                if (myprepays == null)
                {
                    PrepayCustomerRequestDBM mypdbm = new PrepayCustomerRequestDBM();
                    mypdbm.RequestCustomer = this;
                    mypdbm.Fill();
                    this.Prepays = mypdbm.Collection;
                    this.PropertyChangedNotification(nameof(this.Prepays)); this.PropertyChangedNotification(nameof(this.InvoiceDiscount));
                    foreach (PrepayCustomerRequest item in myprepays)
                    { item.PropertyChangedNotification(nameof(PrepayCustomerRequest.FinalInvoiceRubSumPaid)); item.PropertyChangedNotification(nameof(PrepayCustomerRequest.CustomerBalance)); }
                }
                return myprepays;
            }
        }
        internal bool PrepaysIsNull
        {
            get { return myprepays == null; }
        }
        protected override void RejectProperty(string property, object value)
        {
            decimal oldvalue;
            switch (property)
            {
                case "CurrencyDate":
                    mycurrencydate = (DateTime)value;
                    break;
                case "CurrencyRate":
                    mycurrencyrate = (decimal?)value;
                    break;
                case "CustomerLegal":
                    mycustomerlegal = (CustomerLegal)value;
                    break;
                case "Invoice":
                    oldvalue = myinvoice ?? 0M;
                    myinvoice = (decimal?)value;
                    UpdatedRequest("Invoice", oldvalue);
                    base.PropertyChangedNotification("InsuranceCost");
                    base.PropertyChangedNotification("InsurancePay");
                    break;
                //case "InvoiceDiscount":
                //    myinvoicediscount = (decimal?)value;
                //    this.PropertyChangedNotification("InvoiceDiscountAdd2per");
                //    UpdatedSingleLegal("InvoiceDiscount");
                //    break;
                case "OfficialWeight":
                    oldvalue = myofficialweight ?? 0M;
                    myofficialweight = (decimal?)value;
                    UpdatedRequest("Invoice", oldvalue);
                    break;
                case "DependentNew":
                    int i = 0;
                    if (myprepays != null)
                    {
                        PrepayCustomerRequest[] additem = new PrepayCustomerRequest[myprepays.Count];
                        foreach (PrepayCustomerRequest item in myprepays)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            { additem[i] = item; i++; }
                            else if (item.DomainState == lib.DomainObjectState.Deleted)
                            {
                                item.RejectChanges();
                            }
                        }
                        for (int ii = 0; ii < i; ii++) myprepays.Remove(additem[ii]);
                    }
                    break;
            }
        }
        private void SingleSelected()
        {
            if (myrequest != null)
            {
                int n = 0;
                RequestCustomerLegal single = null;
                foreach (RequestCustomerLegal item in myrequest.CustomerLegals)
                {
                    if (item.Selected)
                    {
                        if (n == 0)
                            single = item;
                        n++;
                    }
                }
                if (n == 1 & single != null)
                {
                    single.Invoice = myrequest.Invoice;
                    single.InvoiceDiscount = myrequest.InvoiceDiscount;
                    single.OfficialWeight = myrequest.OfficialWeight;
                    //if (myrequest.Payments.Count == 0)
                    //{
                    //    RequestPayment payment = new RequestPayment(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, single, 1, 0, null, 0M, DateTime.Today, null, null);
                    //    payment.DocType = 1;
                    //    myrequest.Payments.Add(payment);
                    //    payment = new RequestPayment(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, single, 2, 0, null, 0M, DateTime.Today, null, null);
                    //    payment.DocType = 1;
                    //    myrequest.Payments.Add(payment);
                    //    //payment = new RequestPayment(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, single, 3, 0, null, 0M, DateTime.Today, null, null);
                    //    //payment.DocType = 1;
                    //    //myrequest.Payments.Add(payment);
                    //}
                }
            }
        }
        //private void PaymentsDelete()
        //{
        //    List<RequestPayment> deleted = new List<RequestPayment>();
        //    if (myrequest != null)
        //        foreach (RequestPayment item in myrequest.Payments)
        //            if (item.RequestCustomer == this & (item.DomainState == lib.DomainObjectState.Added))
        //            {
        //                deleted.Add(item);
        //            }
        //    foreach (RequestPayment item in deleted)
        //        myrequest.Payments.Remove(item);
        //}
        internal void UpdatedRequest(string PropertyName, decimal oldvalue)
        {
            if (!this.RequestUpdating && myrequest != null)
            {
                int n = 0;
                RequestCustomerLegal single = null;
                foreach (RequestCustomerLegal item in myrequest.CustomerLegals)
                {
                    if (item.Selected)
                    {
                        if (n == 0)
                            single = item;
                        n++;
                    }
                }
                if (n == 1 & object.Equals(single,this))
                {
                    switch (PropertyName)
                    {
                        case "Invoice":
                            myrequest.Invoice = myinvoice;
                            break;
                        case "InvoiceDiscount":
                            myrequest.InvoiceDiscount = this.InvoiceDiscount;
                            break;
                            //case "OfficialWeight":
                            //    myrequest.OfficialWeight = myofficialweight;
                            //    break;
                    }
                }
                else if(n>1 && this.Selected)
                {
                    switch (PropertyName)
                    {
                        case "Invoice":
                            myrequest.Invoice += myinvoice - oldvalue;
                            break;
                        case "InvoiceDiscount":
                            myrequest.InvoiceDiscount += this.InvoiceDiscount - oldvalue;
                            break;
                            //case "OfficialWeight":
                            //    myrequest.OfficialWeight = myofficialweight;
                            //    break;
                    }
                }
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            RequestCustomerLegal templ= sample as RequestCustomerLegal;
            this.CustomerLegal = templ.CustomerLegal;
            this.Request = templ.Request;
            this.Selected = templ.Selected;
            this.CustomerLegal = templ.CustomerLegal;
            this.Invoice = templ.Invoice;
            //this.InvoiceDiscount = templ.InvoiceDiscount;
        }
        internal bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                case nameof(this.InvoiceDiscount):
                    if (myprepays.Count > 1)
                    {
                        errmsg = "Для изменения суммы воспользуйтесь окном предоплат!";
                        isvalid = false;
                    }
                    else if (myprepays.Count == 1)
                        isvalid = myprepays[0].ValidateProperty(nameof(PrepayCustomerRequest.EuroSum), value,out errmsg);
                    break;
            }
            return isvalid;
        }
        private PrepayCustomerRequest GetNewPrepay()
        {
            return new PrepayCustomerRequest(lib.NewObjectId.NewId, 0,null,null, lib.DomainObjectState.Added, this,null, null, 0M, myinvoice??0M, null
                                , new Prepay(id:lib.NewObjectId.NewId, stamp:0,updated:null,updater:null, mstate:lib.DomainObjectState.Added,
                                agent:CustomBrokerWpf.References.AgentStore.GetItemLoad(this.Request.AgentId ?? 0),
                                cbrate: null, currencypaiddate:null, customer:this.CustomerLegal,dealpassport:true, eurosum:0M, importer:this.Request.Importer, initsum:myinvoice ?? 0M, invoicedate:null, invoicenumber:null, percent:0M, refund:0M,
                                shipplandate: this.Request.ShipPlanDate ?? CustomBrokerWpf.References.EndQuarter(DateTime.Today.AddDays(10)) )
                                ,null, null);
        }

        #region Blocking
        private void RequestCustomerLegal_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DomainState")
            {
                if (this.DomainStatePrevious == lib.DomainObjectState.Unchanged & (this.DomainState == lib.DomainObjectState.Modified | this.DomainState == lib.DomainObjectState.Deleted))
                {
                    this.Request.Blocking();
                }
                else if (this.DomainStatePrevious == lib.DomainObjectState.Modified | this.DomainStatePrevious == lib.DomainObjectState.Deleted)
                    this.Request.UnBlocking();
            }
        }
        #endregion
    }

    internal class RequestCustomerLegalStore : lib.DomainStorageLoad<RequestCustomerLegal>
    {
        public RequestCustomerLegalStore(lib.DBManagerId<RequestCustomerLegal> dbm) : base(dbm) { }

        protected override void UpdateProperties(RequestCustomerLegal olditem, RequestCustomerLegal newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class RequestCustomerLegalDBM : lib.DBManagerId<RequestCustomerLegal>
    {
        internal RequestCustomerLegalDBM() : base()
        {
            NeedAddConnection = true;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            base.SelectProcedure = true;
            base.UpdateProcedure = true;

            SelectCommandText = "dbo.RequestCustomerLegal_sp";
            UpdateCommandText = "dbo.RequestCustomerLegalUpd_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int),
                new SqlParameter("@param2", System.Data.SqlDbType.Int),
                new SqlParameter("@param3", System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                new SqlParameter("@param0", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@param1", System.Data.SqlDbType.Int)
                ,new SqlParameter("@param2", System.Data.SqlDbType.Int)
                ,new SqlParameter("@invoice", System.Data.SqlDbType.Money)
                ,new SqlParameter("@invoicetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicediscount", System.Data.SqlDbType.Money)
                ,new SqlParameter("@invoicediscounttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@cellnumber", System.Data.SqlDbType.SmallInt)
                ,new SqlParameter("@cellnumbertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@actualweight", System.Data.SqlDbType.SmallMoney)
                ,new SqlParameter("@actualweighttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@officialweight", System.Data.SqlDbType.SmallMoney)
                ,new SqlParameter("@officialweighttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@volume", System.Data.SqlDbType.SmallMoney)
                ,new SqlParameter("@volumetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencyrate", System.Data.SqlDbType.Money)
                ,new SqlParameter("@currencyratetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@currencydate", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@currencydatetrue", System.Data.SqlDbType.Bit)
            };
            mycidbm = new CustomsInvoiceDBM();
            mypdbm = new PrepayCustomerRequestDBM();
        }

        private Request myrequest;
        internal Request Request
        {
            set { myrequest = value; }
            get { return myrequest; }
        }
        public override int? ItemId { get; set; }
        private PrepayCustomerRequestDBM mypdbm;
        private CustomsInvoiceDBM mycidbm;

        protected override void SetSelectParametersValue()
        {
            SelectParams[0].Value = myrequest?.Id;
            SelectParams[1].Value = myrequest?.CustomerId;
            SelectParams[2].Value = this.ItemId;
        }
        protected override RequestCustomerLegal CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            Request request = myrequest ?? CustomBrokerWpf.References.RequestStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("requestid")), addcon);
            CustomerLegal customerlegal = CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("customerlegalid")), addcon);
            RequestCustomerLegal item = new RequestCustomerLegal(reader.IsDBNull(0) ? 0 : reader.GetInt32(0), reader.IsDBNull(1) ? 0 : reader.GetInt64(1), lib.DomainObjectState.Unchanged
                , request
                , customerlegal
                , reader.GetBoolean(reader.GetOrdinal("selected"))
                , reader.IsDBNull(reader.GetOrdinal("actualweight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("actualweight"))
                , reader.IsDBNull(reader.GetOrdinal("cellnumber")) ? (int?)null : reader.GetInt16(reader.GetOrdinal("cellnumber"))
                , reader.IsDBNull(reader.GetOrdinal("currencydate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("currencydate"))
                , reader.IsDBNull(reader.GetOrdinal("currencyrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("currencyrate"))
                , reader.IsDBNull(reader.GetOrdinal("invoice")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("invoice"))
                , reader.IsDBNull(reader.GetOrdinal("invoicediscount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("invoicediscount"))
                , reader.IsDBNull(reader.GetOrdinal("officialWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("officialWeight"))
                , reader.IsDBNull(reader.GetOrdinal("volume")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("volume")));
            if (item.Id > 0) item = CustomBrokerWpf.References.RequestCustomerLegalStore.UpdateItem(item);

            if (this.Refreshing & !item.CustomsInvoiceIsNull)
                CustomBrokerWpf.References.CustomsInvoiceStore.UpdateItem(item.CustomsInvoice.Id, addcon);

            mypdbm.Command.Connection = addcon;
            mypdbm.Errors.Clear();
            mypdbm.RequestCustomer = item;
            mypdbm.Fill();
            if (mypdbm.Errors.Count > 0)
                foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
            else
            {
                if (item.PrepaysIsNull)
                    item.Prepays = mypdbm.Collection;
                else
                {
                    this.mydispatcher.Invoke(() =>
                    {
                        item.Prepays.Clear();
                        foreach (PrepayCustomerRequest prepay in mypdbm.Collection)
                            item.Prepays.Add(prepay);
                    });
                    item.PropertyChangedNotification(nameof(item.InvoiceDiscount));
                    foreach (PrepayCustomerRequest prepay in item.Prepays)
                    { prepay.PropertyChangedNotification(nameof(PrepayCustomerRequest.FinalInvoiceRubSumPaid)); prepay.PropertyChangedNotification(nameof(PrepayCustomerRequest.CustomerBalance)); }
                }
            }
            mypdbm.Collection = null;
            item.IsLoaded = true;

            return item;
        }
        protected override void GetOutputParametersValue(RequestCustomerLegal item)
        {
            if (item.Id <= 0)
            {
                //item.Id = (int)myinsertparams[0].Value;
                //CustomBrokerWpf.References.RequestCustomerLegalStore.UpdateItem(item);
            }
        }
        protected override void ItemAcceptChanches(RequestCustomerLegal item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(RequestCustomerLegal item)
        {
            bool isSuccess = true;
            if (!item.Selected && item.DomainState == lib.DomainObjectState.Unchanged)
                item.Prepays?.Clear();
            else
            {
                mypdbm.Errors.Clear();
                mypdbm.RequestCustomer = item;
                mypdbm.Collection = item.Prepays;
                if (!mypdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
                }
            }
            return isSuccess;
        }
        protected override bool SaveIncludedObject(RequestCustomerLegal item)
        {
            bool Success = true;
            if (!item.CustomsInvoiceIsNull)
            {
                mycidbm.Errors.Clear();
                if(!mycidbm.SaveItemChanches(item.CustomsInvoice))
                {
                    Success = false;
                    foreach (lib.DBMError err in mycidbm.Errors) this.Errors.Add(err);
                }
            }
            return Success;
        }
        protected override bool SaveReferenceObjects()
        {
            mycidbm.Command.Connection = this.Command.Connection;
            mypdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetParametersValue(RequestCustomerLegal item)
        {
            foreach(SqlParameter par in myupdateparams)
            {
                switch(par.ParameterName)
                {
                    case "@param0":
                        par.Value = item.Selected;
                        break;
                    case "@param1":
                        par.Value = item.Request.Id;
                        break;
                    case "@param2":
                        par.Value = item.CustomerLegal.Id;
                        break;
                    case "@invoice":
                        par.Value = item.Invoice;
                        break;
                    case "@invoicetrue":
                        par.Value = item.HasPropertyOutdatedValue("Invoice");
                        break;
                    case "@invoicediscount":
                        par.Value = item.InvoiceDiscount;
                        break;
                    case "@invoicediscounttrue":
                        par.Value = item.HasPropertyOutdatedValue("InvoiceDiscount");
                        break;
                    case "@cellnumber":
                        par.Value = item.CellNumber;
                        break;
                    case "@cellnumbertrue":
                        par.Value = item.HasPropertyOutdatedValue("CellNumber");
                        break;
                    case "@actualweight":
                        par.Value = item.ActualWeight;
                        break;
                    case "@actualweighttrue":
                        par.Value = item.HasPropertyOutdatedValue("ActualWeight");
                        break;
                    case "@officialweight":
                        par.Value = item.OfficialWeight;
                        break;
                    case "@officialweighttrue":
                        par.Value = item.HasPropertyOutdatedValue("OfficialWeight");
                        break;
                    case "@volume":
                        par.Value = item.Volume;
                        break;
                    case "@volumetrue":
                        par.Value = item.HasPropertyOutdatedValue("Volume");
                        break;
                    case "@currencyrate":
                        par.Value = item.CurrencyRate;
                        break;
                    case "@currencyratetrue":
                        par.Value = item.HasPropertyOutdatedValue("CurrencyRate");
                        break;
                    case "@currencydate":
                        par.Value = item.CurrencyDate;
                        break;
                    case "@currencydatetrue":
                        par.Value = item.HasPropertyOutdatedValue("CurrencyDate");
                        break;
                }
            }
            return true;
        }
        protected override void LoadObjects(RequestCustomerLegal item)
        {
        }
        protected override bool LoadObjects()
        {
            //mypdbm.Command.Connection = this.Command.Connection;
            //foreach (RequestCustomerLegal item in this.Collection)
            //{
            //    LoadObjects(item);
            //}
            return this.Errors.Count==0;
        }
    }

    public class RequestCustomerLegalVM : lib.ViewModelErrorNotifyItem<RequestCustomerLegal>
    {
        public RequestCustomerLegalVM(RequestCustomerLegal legal) : base(legal)
        {
            ValidetingProperties.AddRange(new string[] { "Selected", nameof(this.InvoiceDiscount) });
            InitProperties();
        }

        private bool myselected;
        public bool Selected
        {
            set
            {
                if (!this.IsReadOnly && myselected != value)
                {
                    string name = "Selected";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Selected);
                    myselected = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Selected = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Selected : false; }
        }
        public decimal? ActualWeight
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.ActualWeight.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.ActualWeight.Value, value.Value))))
                {
                    string name = "ActualWeight";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ActualWeight);
                    ChangingDomainProperty = name; this.DomainObject.ActualWeight = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ActualWeight : null; }
        }
        public int? CellNumber
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CellNumber.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CellNumber.Value, value.Value))))
                {
                    string name = "CellNumber";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CellNumber);
                    ChangingDomainProperty = name; this.DomainObject.CellNumber = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CellNumber : null; }
        }
        public DateTime? CurrencyDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CurrencyDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.CurrencyDate.Value, value.Value))))
                {
                    string name = "CurrencyDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyDate);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyDate : null; }
        }
        public decimal? CurrencyRate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CurrencyRate.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CurrencyRate.Value, value.Value))))
                {
                    string name = "CurrencyRate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyRate);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyRate = value;
                    this.PropertyChangedNotification("CurrencySum");
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyRate : null; }
        }
        public decimal? CurrencySum
        {
            get { return this.IsEnabled ? this.DomainObject.InvoiceDiscount * this.CurrencyRate : null; }
        }
        private CustomerLegalVM mycustomerlegal;
        public CustomerLegalVM CustomerLegal
        { get { return mycustomerlegal; } }
        public decimal? CustomsPercent
        {
            set
            {
                string name = "CustomsPercent";
                if (this.DomainObject.Request.ParcelId.HasValue)
                {
                    if (!this.IsReadOnly && ((this.DomainObject.CustomsInvoice?.Percent).HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CustomsInvoice.Percent * 100M, value.Value))))
                    {
                        if (!myUnchangedPropertyCollection.ContainsKey(name))
                            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CustomsInvoice?.Percent);
                        ChangingDomainProperty = name; this.DomainObject.CustomsInvoice.Percent = decimal.Divide(value.Value,100M);
                    }
                }
                else if (value.HasValue)
                    AddErrorMessageForProperty(name, "Ставка для таможенного счета можно устанавливать только после постановки заявки в загрузку!");
                else
                    ClearErrorMessageForProperty(name);
            }
            get { return this.IsEnabled ? this.DomainObject.CustomsInvoice?.Percent * 100M : null; }
        }
        private decimal? mydtsum;
        public decimal? DTSum
        {
            set
            {
                if (!this.IsReadOnly && ((mydtsum ?? this.DomainObject.DTSum).HasValue != value.HasValue || (value.HasValue && !decimal.Equals((mydtsum ?? this.DomainObject.DTSum).Value, value.Value))))
                {
                    string name = nameof(this.DTSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DTSum);
                    mydtsum = value;
                    if (ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.DTSum = value; mydtsum = null; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DTSum : null; } }
        public decimal? Invoice
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Invoice.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Invoice.Value, value.Value))))
                {
                    string name = "Invoice";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Invoice);
                    ChangingDomainProperty = name; this.DomainObject.Invoice = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Invoice : null; }
        }
        private decimal? myinvoicediscount;
        public decimal? InvoiceDiscount
        {
            set
            {
                if (!this.IsReadOnly && ((myinvoicediscount ?? this.DomainObject.InvoiceDiscount).HasValue != value.HasValue || (value.HasValue && !decimal.Equals((myinvoicediscount ?? this.DomainObject.InvoiceDiscount).Value, value.Value))))
                {
                    string name = nameof(this.InvoiceDiscount);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.InvoiceDiscount);
                    myinvoicediscount = value;
                    if (ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.InvoiceDiscount = value; myinvoicediscount = null;this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? (myinvoicediscount?? this.DomainObject.InvoiceDiscount) : null; }
        }
        public bool InvoiceDiscountIsReadOnly
        { get { return this.DomainObject.Prepays.Count > 1; } }
        public decimal? InvoiceDiscountAdd2per
        {
            get { return this.IsEnabled ? this.DomainObject.InvoiceDiscountAdd2per : null; }
        }
        public decimal? OfficialWeight
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.OfficialWeight.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.OfficialWeight.Value, value.Value))))
                {
                    string name = "OfficialWeight";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.OfficialWeight);
                    ChangingDomainProperty = name; this.DomainObject.OfficialWeight = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.OfficialWeight : null; }
        }
        public RequestCustomerLegal RequestCustomerLegal
        { get { return this.DomainObject; } }
        public decimal? SellingMarkup
        {
            get { return this.IsEnabled ? this.DomainObject.SellingMarkup : null; }
        }
        public decimal? Volume
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Volume.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Volume.Value, value.Value))))
                {
                    string name = "Volume";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Volume);
                    ChangingDomainProperty = name; this.DomainObject.Volume = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Volume : null; }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(RequestCustomerLegal.CustomerLegal):
                    if (mycustomerlegal==null && this.DomainObject.CustomerLegal != null)
                        mycustomerlegal = new CustomerLegalVM(this.DomainObject.CustomerLegal);
                    break;
                case "Selected":
                    myselected = this.DomainObject.Selected;
                    break;
                case nameof(this.DomainObject.Prepays):
                case nameof(this.DomainObject.InvoiceDiscount):
                    this.PropertyChangedNotification(nameof(this.InvoiceDiscountIsReadOnly));
                    break;
            }
        }
        protected override void InitProperties()
        {
            myselected = this.DomainObject.Selected;
            if(this.DomainObject.CustomerLegal!=null) mycustomerlegal = new CustomerLegalVM(this.DomainObject.CustomerLegal);
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Selected":
                    if (myselected != this.DomainObject.Selected)
                        myselected = this.DomainObject.Selected;
                    else
                        this.Selected = (bool)value;
                    break;
                case "Invoice":
                    this.DomainObject.Invoice = (decimal?)value;
                    break;
                case "InvoiceDiscount":
                    if (this.InvoiceDiscount != this.DomainObject.InvoiceDiscount)
                        myinvoicediscount = null;
                    else
                        this.DomainObject.InvoiceDiscount = (decimal?)value;
                    break;
                case "ActualWeight":
                    this.DomainObject.ActualWeight = (decimal?)value;
                    break;
                case "CellNumber":
                    this.DomainObject.CellNumber = (int?)value;
                    break;
                case "OfficialWeight":
                    this.DomainObject.OfficialWeight = (decimal?)value;
                    break;
                case "Volume":
                    this.DomainObject.Volume = (decimal?)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Selected":
                    if (!myselected)
                    {
                        foreach (PrepayCustomerRequest item in this.DomainObject.Prepays)
                            if (item.Prepay.InvoiceDate.HasValue )
                            {
                                errmsg = "На юр. лицо уже выписан счет!";
                                isvalid = false;
                                break;
                            }
                    }
                    break;
                case nameof(this.InvoiceDiscount):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myinvoicediscount,out errmsg);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myselected != this.DomainObject.Selected || this.InvoiceDiscount != this.DomainObject.InvoiceDiscount;
        }
    }

    internal class RequestCustomerLegalSynchronizer : lib.ModelViewCollectionsSynchronizer<RequestCustomerLegal, RequestCustomerLegalVM>
    {
        protected override RequestCustomerLegal UnWrap(RequestCustomerLegalVM wrap)
        {
            return wrap.DomainObject as RequestCustomerLegal;
        }
        protected override RequestCustomerLegalVM Wrap(RequestCustomerLegal fill)
        {
            return new RequestCustomerLegalVM(fill);
        }
    }
}