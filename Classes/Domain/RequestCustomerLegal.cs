using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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
            myinvoicediscount = invoicediscount;
            myofficialweight = officialweight;
            myvolume = volume;

            if(myrequest!=null) myrequest.PropertyChanged += Request_PropertyChanged;
            this.PropertyChanged += RequestCustomerLegal_PropertyChanged;
        }
        private void Request_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DTRate":
                case "SellingMarkupRate":
                    this.PropertyChangedNotification("SellingMarkup");
                    break;
                case nameof(Request.AgentId):
                case nameof(Request.Importer):
                    if (myprepays != null)
                    {
                        PrepaysRefresh();
                    }
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
                if (mycustomsinvoice == null)// && this.Request?.Parcel != null
                {
                    mycustomsinvoice = CustomBrokerWpf.References.CustomsInvoiceStore.GetItemLoad(this,out _);
                    if (mycustomsinvoice == null)
                        mycustomsinvoice = new CustomsInvoice(this);
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
            set { if (myrequest != null) myrequest.PropertyChanged -= Request_PropertyChanged; SetProperty<Request>(ref myrequest, value, () => { myrequest.PropertyChanged += Request_PropertyChanged; }); }
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
                    UpdatedRequest("Invoice", oldvalue);
                };
                base.SetProperty<decimal?>(ref myinvoice, value, notify);
            }
            get { return myinvoice; }
        }
        decimal? myinvoicediscount;
        public decimal? InvoiceDiscount
        {
            set
            {
                base.SetProperty<decimal?>(ref myinvoicediscount, value);
            }
            get { return myinvoicediscount; }
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
        public decimal? PrepaySum
        {
            set
            { // всегда обновляем Prepay и InvoiceDiscount если суммы были равны
                decimal? oldsum = this.PrepaySum;
                if (oldsum == this.InvoiceDiscount & this.UpdatePrepay(value, oldsum ?? 0M))
                    UpdateInvoiceDiscount(value,'l');
                PropertyChangedNotification(nameof(this.PrepaySum));
            }
            get { return myprepays?.Sum<PrepayCustomerRequest>((PrepayCustomerRequest item) => { return item.DomainState<lib.DomainObjectState.Deleted ? item.EuroSum : 0M; }); }
        }

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
                    myprepays = new ObservableCollection<PrepayCustomerRequest>(); // чтобы небыло гонки
                    PrepaysRefresh();
                }
                return myprepays;
            }
        }
        internal bool PrepaysIsNull
        {
            get { return myprepays == null; }
        }
        private void PrepaysRefresh()
        {
            PrepayCustomerRequestDBM pdbm = new PrepayCustomerRequestDBM();
            if (myprepays != null && myprepays.Count > 0)
                pdbm.Collection = myprepays;
            else
                pdbm.FillType = lib.FillType.PrefExist;
            if (this.DomainState != lib.DomainObjectState.Added) // чтобы не затерлись добавленные Prepay
            {
                pdbm.RequestCustomer = this;
                pdbm.Fill();
            }
            bool find = false;
            PrepayFundDBM pfdbm = new PrepayFundDBM();
            pfdbm.Customer = this;
            pfdbm.Command.Connection = pdbm.Command.Connection;
            pfdbm.Fill();
            foreach (Prepay pay in pfdbm.Collection)
            {
                foreach (PrepayCustomerRequest item in pdbm.Collection)
                    if (item.Prepay.Id == pay.Id)
                    {
                        find = true;
                        break;
                    }
                if (!find)
                    Application.Current.Dispatcher.Invoke(() =>
                    { this.Prepays.Add(new PrepayCustomerRequest(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, this, null, null, 0M, 0M, string.Empty, pay, null, null, null)); });
            }
            if (myprepays == null)
                this.Prepays = pdbm.Collection;
            this.PropertyChangedNotification(nameof(this.Prepays));// this.PropertyChangedNotification(nameof(this.InvoiceDiscount));
            foreach (PrepayCustomerRequest item in myprepays)
            { item.PropertyChangedNotification(nameof(PrepayCustomerRequest.FinalInvoiceRubSumPaid)); item.PropertyChangedNotification(nameof(PrepayCustomerRequest.CustomerBalance)); }
            if (pdbm.Errors.Count > 0 | pfdbm.Errors.Count > 0)
                Common.PopupCreator.GetPopup(text: pdbm.ErrorMessage+"/n"+ pfdbm.ErrorMessage
                     , background: System.Windows.Media.Brushes.LightPink
                     , foreground: System.Windows.Media.Brushes.Red
                     , staysopen: false
                     ).IsOpen=true;
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
        internal void UnSubscribe()
        {
            if(myrequest!=null) myrequest.PropertyChanged -= Request_PropertyChanged;
        }
        internal bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                case nameof(this.PrepaySum):
                    if (this.Selected && (decimal?)value != this.PrepaySum)
                    {
                        List<PrepayCustomerRequest> prepays = myprepays.Where((PrepayCustomerRequest prepay) => { return this.Request.Status.Id == 0 || !(prepay.Prepay.IsPrepay ?? false); }).ToList<PrepayCustomerRequest>();
                        if (prepays.Count() > 1)
                        {
                            errmsg = "У юр. лица несколько предоплат! Для изменения суммы воспользуйтесь списком предоплат!";
                            isvalid = false;
                        }
                        else if (prepays.Count() == 1)
                            isvalid = prepays.First().ValidateProperty(nameof(PrepayCustomerRequest.EuroSum), value, out errmsg);
                    }
                    break;
            }
            return isvalid;
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
                if (n == 1 & object.Equals(single,this))
                {
                    switch (PropertyName)
                    {
                        case "Invoice":
                            myrequest.Invoice = myinvoice;
                            break;
                        case "InvoiceDiscount":
                            myrequest.UpdateInvoiceDiscount(this.InvoiceDiscount,1);
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
                        //case "Invoice":
                        //    myrequest.Invoice += myrequest.Invoice + myinvoice - oldvalue;
                        //    break;
                        case "InvoiceDiscount":
                            myrequest.UpdateInvoiceDiscount((myrequest.InvoiceDiscount??0M) + this.InvoiceDiscount - oldvalue,1);
                            break;
                            //case "OfficialWeight":
                            //    myrequest.OfficialWeight = myrequest.OfficialWeight + myofficialweight - oldvalue;
                            //    break;
                    }
                }
            }
        }
        internal void UpdateInvoiceDiscount(decimal? value, char entry)
        {
            decimal oldvalue = myinvoicediscount ?? 0M;
            this.InvoiceDiscount = value;
            if (oldvalue == (myinvoicediscount ?? 0M)) return;
            if(entry=='l')
            {
                UpdatedRequest(nameof(this.InvoiceDiscount), oldvalue);
                //this.UpdatePrepay(value, oldvalue);
            }
            else if(entry == 'r')
                this.UpdatePrepay(value, oldvalue);
            else if (entry == 'p')
                UpdatedRequest(nameof(this.InvoiceDiscount), oldvalue);
        }
        private PrepayCustomerRequest GetNewPrepay()
        {
            return new PrepayCustomerRequest(lib.NewObjectId.NewId, 0,null,null, lib.DomainObjectState.Added, this,null, null, 0M, myinvoice??0M, null
                                , new Prepay(id:lib.NewObjectId.NewId, stamp:0,updated:null,updater:null, mstate:lib.DomainObjectState.Added,
                                agent:CustomBrokerWpf.References.AgentStore.GetItemLoad(this.Request.AgentId ?? 0, out _),
                                cbrate: null, currencypaiddate:null, customer:this.CustomerLegal,dealpassport:true, eurosum:0M, importer:this.Request.Importer, initsum:myinvoice ?? 0M, invoicedate:null, invoicenumber:null, percent:0M, refund:0M,
                                shipplandate: this.Request.ShipPlanDate ?? CustomBrokerWpf.References.EndQuarter(DateTime.Today.AddDays(10)) )
                                , null, null, null);
        }
        private bool UpdatePrepay(decimal? value, decimal oldvalue)
        {
            bool changed = false;
            if ((value ?? 0M) == 0M)
            {
                foreach (PrepayCustomerRequest item in myprepays)
                {
                    item.EuroSum = 0M;
                    item.DTSum = null;
                }
                changed = true;
            }
            else if (oldvalue == myprepays.Sum((PrepayCustomerRequest prepay) => { return prepay.EuroSum; }))
            {
                List<PrepayCustomerRequest> prepays = myprepays.Where((PrepayCustomerRequest prepay) => { return this.Request.Status.Id==0 || !(prepay.Prepay.IsPrepay ?? false); }).ToList<PrepayCustomerRequest>();
                if (prepays.Count < 2)
                {
                    if (prepays.Count == 0)
                    {
                        prepays.Add(this.GetNewPrepay());
                        myprepays.Add(prepays[0]);
                    }
                    if (!prepays.First().Prepay.InvoiceDate.HasValue)
                    {
                        prepays[0].EuroSum = value ?? 0M;
                        changed = true;
                    }
                    //myprepays[0].DTSum = value ?? 0M;
                }
            }
            return changed;
        }
        internal void AddPrepay()
        { myprepays.Add(GetNewPrepay()); }
        internal void PrepayDistribute(string property, int decimals)
        {
            if (!this.InvoiceDiscount.HasValue || this.Prepays.Count == 0) return;
            if (this.Prepays.Count == 1)
                switch (property)
                {
                    case nameof(PrepayCustomerRequest.DTSum):
                        this.Prepays[0].DTSumSet = this.InvoiceDiscount;
                        break;
                }
            else
            {
                decimal? val;
                decimal total = 0M, d = 0M, d1 = 0M, d2 = 0M, sd = 0M, s = 0M, sr = 0M, sdr = 0M;
                switch (property)
                {
                    case nameof(PrepayCustomerRequest.CustomsInvoiceRubSum):
                        total = this.Prepays.Sum((PrepayCustomerRequest prepay) => { return prepay.EuroSum; });
                        if (total == 0M) return;
                        total = decimal.Divide(decimal.Round(this.InvoiceDiscount.Value, decimals), total);
                        break;
                }
                foreach (PrepayCustomerRequest prepay in this.Prepays)
                {
                    switch (property)
                    {
                        case nameof(prepay.DTSum):
                            if (prepay.EuroSum>0M)
                            {
                                s = decimal.Multiply(total, prepay.EuroSum);
                                sr = decimal.Round(s, decimals);
                                d1 = s - sr;
                                sd = s + d;
                                sdr = decimal.Round(sd, decimals);
                                d2 = sd - sdr;
                                if ((s > sr ? d1 : -d1) > (sd > sdr ? d2 : -d2))
                                {
                                    d = d2;
                                    val = sdr;
                                }
                                else
                                {
                                    d = d + d1;
                                    val = sr;
                                }
                            }
                            else
                                val = 0M;
                            prepay.DTSumSet = val;
                            break;
                    }
                }
            }
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

    internal class RequestCustomerLegalStore : lib.DomainStorageLoad<RequestCustomerLegal, RequestCustomerLegalDBM>
    {
        public RequestCustomerLegalStore(RequestCustomerLegalDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(RequestCustomerLegal olditem, RequestCustomerLegal newitem)
        {
            olditem.UpdateProperties(newitem);
        }

        internal RequestCustomerLegal GetItem(CustomerLegal customer, Request request)
        {
            return Dispatcher.Invoke<RequestCustomerLegal>(() =>
            {
                RequestCustomerLegal firstitem = default(RequestCustomerLegal);
                if (request != null && customer != null)
                {
                    foreach (RequestCustomerLegal item in mycollection.Values)
                        if (item.CustomerLegal == customer && item.Request == request)
                        { firstitem = item; break; }
                }
                return firstitem;
            });
        }
        internal RequestCustomerLegal GetItemLoad(CustomerLegal customer, Request request, out List<lib.DBMError> errors)
		{
            return GetItemLoad(customer, request, null, out errors);
        }
        internal RequestCustomerLegal GetItemLoad(CustomerLegal customer, Request request, SqlConnection conection, out List<lib.DBMError> errors)
        {
            //return Dispatcher.Invoke<RequestCustomerLegal>(() =>
            //{
            RequestCustomerLegalDBM dbm;
            errors = new List<lib.DBMError>();
            RequestCustomerLegal firstitem = default(RequestCustomerLegal);
                if (request != null && customer != null)
                {
                    firstitem = this.GetItem(customer, request);
                    if (firstitem == default(RequestCustomerLegal))
                    {
                    dbm = GetDBM();
                    dbm.CustomerLegal = customer;
                    dbm.Request = request;
                    dbm.Command.Connection = conection;
                    firstitem = dbm.GetFirst();
                    if (firstitem != null) firstitem = UpdateItem(firstitem);
                    dbm.Command.Connection = null;
                    errors.AddRange(dbm.Errors);
                    dbm.Errors.Clear();
                    mydbmanagers.Enqueue(dbm);
                    }
                }
                    
            return firstitem;
            //});
        }
    }

    public class RequestCustomerLegalDBM : lib.DBManagerId<RequestCustomerLegal>
    {
        public RequestCustomerLegalDBM() : base()
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
                new SqlParameter("@param3", System.Data.SqlDbType.Int),
                new SqlParameter("@param4", System.Data.SqlDbType.Int)
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
            mypfdbm = new PrepayFundDBM();
        }

        private Request myrequest;
        internal Request Request
        {
            set { myrequest = value; }
            get { return myrequest; }
        }
        private CustomerLegal mycustomer;
        public CustomerLegal CustomerLegal
        { set { mycustomer = value; } get { return mycustomer; } }
        public override int? ItemId { get; set; }
        private PrepayCustomerRequestDBM mypdbm;
        private PrepayFundDBM mypfdbm;
        private CustomsInvoiceDBM mycidbm;

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            SelectParams[0].Value = myrequest?.Id;
            SelectParams[1].Value = myrequest?.CustomerId;
            SelectParams[2].Value = this.ItemId;
            SelectParams[3].Value = mycustomer?.Id;
            mypdbm.FillType = this.FillType;
        }
        protected override RequestCustomerLegal CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            CustomerLegal customerlegal = CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("customerlegalid")), addcon, out List<lib.DBMError> errors);
            this.Errors.AddRange(errors);
            Request request = myrequest ?? CustomBrokerWpf.References.RequestStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("requestid")), addcon, out errors);
            this.Errors.AddRange(errors);
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

            if ((this.FillType == lib.FillType.Refresh) & !item.CustomsInvoiceIsNull)
            {
                CustomBrokerWpf.References.CustomsInvoiceStore.UpdateItem(item.CustomsInvoice.Id, addcon, out errors);
                this.Errors.AddRange(errors);
            }
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
            this.RefreshFund(item, this.Errors, addcon);
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
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.InvoiceDiscount));
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
        protected override bool LoadObjects()
        {
            //mypdbm.Command.Connection = this.Command.Connection;
            //foreach (RequestCustomerLegal item in this.Collection)
            //{
            //    LoadObjects(item);
            //}
            return this.Errors.Count==0;
        }
        private void RefreshFund(RequestCustomerLegal requestlegal,List<lib.DBMError> errors, SqlConnection con)
        {
            bool find = false;
            mypfdbm.Errors.Clear();
            mypfdbm.Customer = requestlegal;
            mypfdbm.Command.Connection = con;
            mypfdbm.Fill();
            if (mypfdbm.Errors.Count > 0)
                foreach (lib.DBMError err in mypfdbm.Errors) errors.Add(err);
            foreach (Prepay pay in mypfdbm.Collection)
            {
                foreach (PrepayCustomerRequest item in requestlegal.Prepays)
                    if (item.Prepay.Id == pay.Id)
                    {  
                        find = true;
                        //item.Prepay.FundSum=pay.FundSum;
                        //item.Prepay.CurrencyBuys.Clear();
                        //foreach (PrepayCurrencyBuy buy in pay.CurrencyBuys)
                        //    item.Prepay.CurrencyBuys.Add(buy);
                        break;
                    }
                if (!find)
                    this.mydispatcher.Invoke(() =>
                    { requestlegal.Prepays.Add(new PrepayCustomerRequest(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, requestlegal, null, null, 0M, 0M, string.Empty, pay, null, null, null)); });
            }
        }
    }

    public class RequestCustomerLegalVM : lib.ViewModelErrorNotifyItem<RequestCustomerLegal>
    {
        public RequestCustomerLegalVM(RequestCustomerLegal legal) : base(legal)
        {
            ValidetingProperties.AddRange(new string[] { "Selected" });
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
                if (!this.IsReadOnly && ((this.DomainObject.CustomsInvoice?.Percent).HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CustomsInvoice.Percent * 100M, value.Value))))
                {
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CustomsInvoice?.Percent);
                    ChangingDomainProperty = name; this.DomainObject.CustomsInvoice.Percent = decimal.Divide(value.Value,100M);
                }
                //if (this.DomainObject.Request.ParcelId.HasValue)
                //{
                //}
                //else if (value.HasValue)
                //    AddErrorMessageForProperty(name, "Ставка для таможенного счета можно устанавливать только после постановки заявки в загрузку!");
                //else
                //    ClearErrorMessageForProperty(name);
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
                    { ChangingDomainProperty = name; this.DomainObject.UpdateInvoiceDiscount(value,'l'); myinvoicediscount = null;this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? (myinvoicediscount?? this.DomainObject.InvoiceDiscount) : null; }
        }
        public bool PrepayIsReadOnly
        { get { return this.Prepays.Count > 1; } }
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
        private decimal? myprepay;
        public decimal? PrepaySum
        {
            set
            {
                if (!this.IsReadOnly && ((myprepay ?? this.DomainObject.PrepaySum).HasValue != value.HasValue || (value.HasValue && !decimal.Equals((myprepay ?? this.DomainObject.PrepaySum).Value, value.Value))))
                {
                    string name = nameof(this.PrepaySum);
                    myprepay = value;
                    if (ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.PrepaySum = value; myprepay = null; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? (myprepay ?? this.DomainObject.PrepaySum) : null; }
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

        private PrepayCustomerRequestSynchronizer mypsync;
        private ListCollectionView myprepays;
        public ListCollectionView Prepays
        {
            get
            {
                if (myprepays == null)
                {
                    if (mypsync == null)
                    {
                        mypsync = new PrepayCustomerRequestSynchronizer();
                        mypsync.DomainCollection = this.DomainObject.Prepays;
                    }
                    myprepays = new ListCollectionView(mypsync.ViewModelCollection);
                    myprepays.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    //myprepays.SortDescriptions.Add(new System.ComponentModel.SortDescription("Selected", System.ComponentModel.ListSortDirection.Descending));
                }
                return myprepays;
            }
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
                case nameof(this.DomainObject.PrepaySum):
                case nameof(this.DomainObject.Prepays):
                    this.PropertyChangedNotification(nameof(this.PrepayIsReadOnly));
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
                case "DependentNew":
                    int i = 0;
                    if (myprepays != null)
                    {
                        PrepayCustomerRequestVM[] removed = new PrepayCustomerRequestVM[mypsync.ViewModelCollection.Count];
                        foreach (PrepayCustomerRequestVM prepay in mypsync.ViewModelCollection)
                        {
                            if (prepay.DomainState == lib.DomainObjectState.Added)
                            {
                                removed[i] = prepay;
                                i++;
                            }
                            else
                            {
                                this.Prepays.EditItem(prepay);
                                prepay.RejectChanges();
                                this.Prepays.CommitEdit();
                            }
                        }
                        foreach (PrepayCustomerRequestVM prepay in removed)
                            if (prepay != null) mypsync.ViewModelCollection.Remove(prepay);
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
                case nameof(this.PrepaySum):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myprepay,out errmsg);
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