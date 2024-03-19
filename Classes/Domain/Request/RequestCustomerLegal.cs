using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public struct RequestCustomerLegalRecord
    {
        internal int id;
        internal long stamp;
        internal int request;
        internal int customerlegal;
        internal bool selected;
        internal decimal? actualweight;
        internal int? cellnumber;
        internal DateTime? currencydate;
        internal decimal? currencyrate;
        internal decimal? invoice;
        internal decimal? invoicediscount;
        internal decimal? officialweight;
        internal decimal? volume;
    }

    public class RequestCustomerLegal : lib.DomainBaseStamp
    {
        public RequestCustomerLegal(int id, long stamp, lib.DomainObjectState state
            , Request request, CustomerLegal customerlegal, bool selected
            , decimal? actualweight, int? cellnumber, DateTime? currencydate, decimal? currencyrate, decimal? invoice, decimal? invoicediscount, decimal? officialweight, decimal? volume
            ) : base(id, stamp, null, null, state)
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

            if (myrequest != null) myrequest.PropertyChanged += Request_PropertyChanged;
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
                    mycustomsinvoice = CustomBrokerWpf.References.CustomsInvoiceStore.GetItemLoad(this, out _);
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
                    myprepays[0].DTSum = value ?? 0M;
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
            set { SetProperty<bool>(ref myselected, value, () => { SingleSelected(); }); } // PaymentsDelete();
            get { return myselected; }
        }
        decimal? myinvoice;
        public decimal? Invoice
        {
            set
            {
                decimal oldvalue = myinvoice ?? 0M;
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
            //set
            //{ // всегда обновляем Prepay и InvoiceDiscount если суммы были равны
            //    decimal? oldsum = this.PrepaySum;
            //    if (oldsum == this.InvoiceDiscount & this.UpdatePrepay(value, oldsum ?? 0M))
            //        UpdateInvoiceDiscount(value,'l');
            //    PropertyChangedNotification(nameof(this.PrepaySum));
            //}
            get { return myprepays?.Sum<PrepayCustomerRequest>((PrepayCustomerRequest item) => { return item.DomainState < lib.DomainObjectState.Deleted ? item.EuroSum : 0M; }); }
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
                myprepays.CollectionChanged += Prepays_CollectionChanged;
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
        private void Prepays_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (PrepayCustomerRequest prepay in e.OldItems)
                    this.UpdateInvoiceDiscount((this.InvoiceDiscount ?? 0M) - prepay.DTSum, 'p');
            }
            //if (e.NewItems != null)
            //{
            //    foreach (PrepayCustomerRequest prepay in e.OldItems)
            //        this.UpdateInvoiceDiscount((this.InvoiceDiscount ?? 0M) + prepay.DTSum, 'p');
            //}
            this.PropertyChangedNotification(nameof(this.PrepaySum));
        }

        internal bool PrepaysIsNull
        {
            get { return myprepays == null; }
        }
        private void PrepaysRefresh()
        {
            PrepayCustomerRequestDBM pdbm = new PrepayCustomerRequestDBM();
            if (myprepays != null) // не заменять созданную коллекцию - разрыв связи с VM
                pdbm.Collection = myprepays;
            else
                pdbm.FillType = lib.FillType.PrefExist;
            if (this.DomainState != lib.DomainObjectState.Added) // чтобы не затерлись добавленные Prepay
            {
                pdbm.RequestCustomer = this;
                pdbm.Fill();
            }
            if (this.Request.Status.Id > 0)
            {
                bool find = false;
                PrepayFundDBM pfdbm = new PrepayFundDBM();
                pfdbm.Customer = this;
                pfdbm.Command.Connection = pdbm.Command.Connection;
                pfdbm.Fill();
                Application.Current.Dispatcher.Invoke(() =>
                { this.PrePrepays.Clear(); });
                foreach (Prepay pay in pfdbm.Collection)
                {
                    find = false;
                    foreach (PrepayCustomerRequest item in pdbm.Collection)
                        if (item.Prepay.Id == pay.Id)
                        {
                            find = true;
                            break;
                        }
                    if (!find)
                        Application.Current.Dispatcher.Invoke(() =>
                        { this.PrePrepays.Add(new PrepayCustomerRequest(this, pay, null)); });
                }
                if (pdbm.Errors.Count > 0 | pfdbm.Errors.Count > 0)
                    Common.PopupCreator.GetPopup(text: pdbm.ErrorMessage + "/n" + pfdbm.ErrorMessage
                         , background: System.Windows.Media.Brushes.LightPink
                         , foreground: System.Windows.Media.Brushes.Red
                         , staysopen: false
                         ).IsOpen = true;
            }
            if (myprepays == null)
                this.Prepays = pdbm.Collection;
            this.PropertyChangedNotification(nameof(this.Prepays));// this.PropertyChangedNotification(nameof(this.InvoiceDiscount));
            foreach (PrepayCustomerRequest item in myprepays)
            { item.PropertyChangedNotification(nameof(PrepayCustomerRequest.FinalInvoiceRubSumPaid)); item.PropertyChangedNotification(nameof(PrepayCustomerRequest.CustomerBalance)); }
        }
        private ObservableCollection<PrepayCustomerRequest> mypreprepays;
        internal ObservableCollection<PrepayCustomerRequest> PrePrepays
        {
            set
            {
                mypreprepays = value;
                this.PropertyChangedNotification(nameof(this.PrePrepays));
            }
            get
            {
                if (mypreprepays == null)
                {
                    mypreprepays = new ObservableCollection<PrepayCustomerRequest>(); // чтобы небыло гонки
                }
                return mypreprepays;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            RequestCustomerLegal templ = sample as RequestCustomerLegal;
            this.CustomerLegal = templ.CustomerLegal;
            this.Request = templ.Request;
            this.Selected = templ.Selected;
            this.CustomerLegal = templ.CustomerLegal;
            this.Invoice = templ.Invoice;
            this.InvoiceDiscount = templ.InvoiceDiscount;
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

        //public bool WarehouseIsNull // чтобы не создавать объект
        //{ get { return mywarehouseid == 0; } }
        //private int mywarehouseid;
        //internal int WarehouseId
        //{ set { mywarehouseid = value; } }
        //private WarehouseRU mywarehouse;
        //public WarehouseRU Warehouse
        //{
        //    get
        //    {
        //        if(mywarehouse==null & mywarehouseid != 0)
        //        {

        //        }
        //        return mywarehouse;
        //    }
        //}

        internal void UnSubscribe()
        {
            if (myrequest != null) myrequest.PropertyChanged -= Request_PropertyChanged;
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
                            isvalid = prepays.First().ValidateProperty(nameof(PrepayCustomerRequest.EuroSum), value, out errmsg, out _);
                    }
                    break;
                case nameof(this.InvoiceDiscount):
                    if (this.Selected && (decimal?)value > 0M && (decimal?)value != this.InvoiceDiscount)
                    {
                        List<PrepayCustomerRequest> prepays = myprepays.Where((PrepayCustomerRequest prepay) => { return lib.ViewModelViewCommand.ViewFilterDefault(prepay); }).ToList<PrepayCustomerRequest>();
                        if (prepays.Count() > 1) // without deleted
                        {
                            errmsg = "У юр. лица несколько предоплат! Для изменения суммы воспользуйтесь списком предоплат!";
                            isvalid = false;
                        }
                        else if (prepays.Count() == 1)
                            isvalid = prepays.First().ValidateProperty(nameof(PrepayCustomerRequest.DTSum), value, out errmsg, out _);
                    }
                    break;
            }
            return isvalid;
        }
        public override void AcceptChanches()
        {
            bool sendmail = false;
            if (this.Id < 0 && CustomBrokerWpf.References.CurrentUserRoles.Contains("Managers"))
                sendmail = true;
            base.AcceptChanches();
            if (sendmail)
                this.Request.SendMailStatus();
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
                    //single.Invoice = myrequest.Invoice;
                    single.UpdateInvoiceDiscount(myrequest.InvoiceDiscount, 'r');
                    if (!myselected) this.UpdateInvoiceDiscount(0, 'r');
                    //single.OfficialWeight = myrequest.OfficialWeight;
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
                else
                {
                    this.Request.UpdateInvoiceDiscount(this.Request.InvoiceDiscount + (myselected ? 1M : -1M) * this.InvoiceDiscount, 1);
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
                if (n == 1 & object.Equals(single, this))
                {
                    switch (PropertyName)
                    {
                        case "Invoice":
                            myrequest.Invoice = myinvoice;
                            break;
                        case "InvoiceDiscount":
                            myrequest.UpdateInvoiceDiscount(this.InvoiceDiscount, 1);
                            break;
                            //case "OfficialWeight":
                            //    myrequest.OfficialWeight = myofficialweight;
                            //    break;
                    }
                }
                else if (n > 1 && this.Selected)
                {
                    switch (PropertyName)
                    {
                        //case "Invoice":
                        //    myrequest.Invoice += myrequest.Invoice + myinvoice - oldvalue;
                        //    break;
                        case "InvoiceDiscount":
                            myrequest.UpdateInvoiceDiscount((myrequest.InvoiceDiscount ?? 0M) + this.InvoiceDiscount - oldvalue, 1);
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
            if (value == (myinvoicediscount ?? 0M)) return;
            decimal oldvalue = myinvoicediscount ?? 0M;
            if (entry == 'l')
            {
                if (this.ValidateProperty(nameof(this.InvoiceDiscount), value, out string errmsg))
                {
                    this.InvoiceDiscount = value;
                    this.UpdatePrepayDT(value, oldvalue);
                    this.UpdatedRequest(nameof(this.InvoiceDiscount), oldvalue);
                }
                else
                {
                    Window active = null;
                    foreach (Window win in Application.Current.Windows)
                        if (win.IsActive) { active = win; break; }
                    active.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        Common.PopupCreator.GetPopup(text: errmsg
                       , background: System.Windows.Media.Brushes.LightPink
                       , foreground: System.Windows.Media.Brushes.Red
                       ).IsOpen = true;
                    }));
                }
            }
            else if (entry == 'r')
            {
                if (this.ValidateProperty(nameof(this.InvoiceDiscount), value, out string errmsg))
                {
                    this.InvoiceDiscount = value;
                    this.UpdatePrepayDT(value, oldvalue);
                }
                else
                {
                    Window active = null;
                    foreach (Window win in Application.Current.Windows)
                        if (win.IsActive) { active = win; break; }
                    active.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                    {
                        Common.PopupCreator.GetPopup(text: errmsg
                       , background: System.Windows.Media.Brushes.LightPink
                       , foreground: System.Windows.Media.Brushes.Red
                       ).IsOpen = true;
                    }));
                }
            }
            else if (entry == 'p')
            {
                this.InvoiceDiscount = value;
                UpdatedRequest(nameof(this.InvoiceDiscount), oldvalue);
            }
        }
        private PrepayCustomerRequest GetNewPrepay()
        {
            return new PrepayCustomerRequest(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, this, 0M, 0M, null, 0M, null
                                , new Prepay(id: lib.NewObjectId.NewId, stamp: 0, updated: null, updater: null, mstate: lib.DomainObjectState.Added,
                                agent: CustomBrokerWpf.References.AgentStore.GetItemLoad(this.Request.AgentId ?? 0, out _),
                                cbrate: null, currencypaiddate: null, customer: this.CustomerLegal, dealpassport: true, eurosum: 0M, importer: this.Request.Importer, initsum: 0M, invoicedate: null, invoicenumber: null, percent: 0M, refund: 0M,
                                shipplandate: this.Request.ShipPlanDate ?? CustomBrokerWpf.References.EndQuarter(DateTime.Today.AddDays(10)))
                                , null, null, null, 1M, null, null);
        }
        private bool UpdatePrepayDT(decimal? value, decimal oldvalue)
        {
            bool changed = false;
            if ((value ?? 0M) == 0M)
            {
                List<PrepayCustomerRequest> prepays = myprepays.ToList(); //myprepays changed when DTSum = 0
                foreach (PrepayCustomerRequest item in prepays)
                {
                    //item.EuroSum = 0M;
                    item.DTSum = 0M;
                }
                changed = true;
            }
            else
            {
                List<PrepayCustomerRequest> prepays = myprepays.Where((PrepayCustomerRequest prepay) => { return lib.ViewModelViewCommand.ViewFilterDefault(prepay); }).ToList<PrepayCustomerRequest>();
                if (prepays.Count < 2)
                {
                    if (prepays.Count == 0)
                    {
                        prepays.Add(this.GetNewPrepay());
                        myprepays.Add(prepays[0]);
                    }
                    if ((value ?? 0M) > 0 && prepays[0].DTSum == myprepays[0].EuroSum && !prepays.First().Prepay.InvoiceDate.HasValue)
                    {
                        myprepays[0].EuroSum = value ?? 0M;
                    }
                    prepays[0].DTSum = value ?? 0M;
                    changed = true;
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
                decimal total = 0M, m = 0M, d = 0M, d1 = 0M, d2 = 0M, sd = 0M, s = 0M, sr = 0M, sdr = 0M;
                switch (property)
                {
                    case nameof(PrepayCustomerRequest.DTSum):
                        foreach (PrepayCustomerRequest prepay in this.Prepays)
                        {
                            if (prepay.DTSumSet.HasValue)
                                m += prepay.DTSumSet.Value;
                            else
                                total += prepay.EuroSum;
                        }
                        if (total == 0M) return;
                        total = decimal.Divide(decimal.Round(this.InvoiceDiscount.Value - m, decimals), total);
                        break;
                }
                foreach (PrepayCustomerRequest prepay in this.Prepays)
                {
                    if (prepay.DTSumSet.HasValue) continue;
                    switch (property)
                    {
                        case nameof(prepay.DTSum):

                            if (prepay.EuroSum > 0M)
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

    internal class RequestCustomerLegalStore : lib.DomainStorageLoad<RequestCustomerLegalRecord,RequestCustomerLegal, RequestCustomerLegalDBM>
    {
        public RequestCustomerLegalStore(RequestCustomerLegalDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(RequestCustomerLegal olditem, RequestCustomerLegal newitem)
        {
            olditem.UpdateProperties(newitem);
        }

        internal RequestCustomerLegal GetItem(CustomerLegal customer, Request request)
        {
            //return Dispatcher.Invoke<RequestCustomerLegal>(() =>
            //{
            RequestCustomerLegal firstitem = default(RequestCustomerLegal);
            if (request != null && customer != null)
            {
                while (myupdatingcoll > 0)
                    System.Threading.Thread.Sleep(10);
                this.myforcount++;
                try
                {
                    foreach (RequestCustomerLegal item in mycollection.Values)
                        if (item.CustomerLegal == customer && item.Request == request)
                        { firstitem = item; break; }
                }
                finally { this.myforcount--; }
            }
            return firstitem;
            //});
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
                    dbm.ItemId = null;
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

    public class RequestCustomerLegalDBM : lib.DBManagerId<RequestCustomerLegalRecord,RequestCustomerLegal>
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
                new SqlParameter("@param3", System.Data.SqlDbType.Int),
                new SqlParameter("@param1", System.Data.SqlDbType.Int),
                new SqlParameter("@param2", System.Data.SqlDbType.Int),
                new SqlParameter("@param4", System.Data.SqlDbType.Int),
                new SqlParameter("@param5", System.Data.SqlDbType.Int)
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
        private CustomerLegal mycustomer;
        public CustomerLegal CustomerLegal
        { set { mycustomer = value; } get { return mycustomer; } }
        private PrepayCustomerRequestDBM mypdbm;
        private CustomsInvoiceDBM mycidbm;
        private CustomerLegalDBM myldbm;
        internal CustomerLegalDBM LegalDBM { set { myldbm = value; } get { return myldbm; } }
        private WarehouseRU mysku;
        internal WarehouseRU SKU
        { set { mysku = value; this.Collection = mysku.CustomerLegals; } get { return mysku; } }

        protected override void SetSelectParametersValue()
        {
            SelectParams[1].Value = myrequest?.Id;
            SelectParams[2].Value = myrequest?.CustomerId;
            SelectParams[3].Value = mycustomer?.Id;
            SelectParams[4].Value = mysku?.Id;
            mypdbm.FillType = this.FillType;
        }
        protected override RequestCustomerLegalRecord CreateRecord(SqlDataReader reader)
        {
            return new RequestCustomerLegalRecord()
            {
                id=reader.IsDBNull(0) ? lib.NewObjectId.NewId : reader.GetInt32(0), stamp=reader.IsDBNull(1) ? 0 : reader.GetInt64(1)
                , request=reader.GetInt32(this.Fields["requestid"])
                , customerlegal=reader.GetInt32(this.Fields["customerlegalid"])
                , selected=reader.GetBoolean(this.Fields["selected"])
                , actualweight=reader.IsDBNull(this.Fields["actualweight"]) ? (decimal?)null : reader.GetDecimal(this.Fields["actualweight"])
                , cellnumber=reader.IsDBNull(this.Fields["cellnumber"]) ? (int?)null : reader.GetInt16(this.Fields["cellnumber"])
                , currencydate=reader.IsDBNull(this.Fields["currencydate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["currencydate"])
                , currencyrate=reader.IsDBNull(this.Fields["currencyrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["currencyrate"])
                , invoice=reader.IsDBNull(this.Fields["invoice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["invoice"])
                , invoicediscount=reader.IsDBNull(this.Fields["invoicediscount"]) ? (decimal?)null : reader.GetDecimal(this.Fields["invoicediscount"])
                , officialweight=reader.IsDBNull(this.Fields["officialweight"]) ? (decimal?)null : reader.GetDecimal(this.Fields["officialweight"])
                , volume=reader.IsDBNull(this.Fields["volume"]) ? (decimal?)null : reader.GetDecimal(this.Fields["volume"])
            };
        }
        protected override RequestCustomerLegal CreateModel(RequestCustomerLegalRecord record, SqlConnection addcon, CancellationToken canceltasktoken = default)
        {
            CustomerLegal customerlegal = CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(record.customerlegal, addcon, out List<lib.DBMError> errors);
            this.Errors.AddRange(errors);
            if (canceltasktoken.IsCancellationRequested) return null;
            Request request = myrequest ?? (this.FillType == lib.FillType.Refresh ? CustomBrokerWpf.References.RequestStore.UpdateItem(record.request, addcon, out errors) : CustomBrokerWpf.References.RequestStore.GetItemLoad(record.request, addcon, out errors));
            this.Errors.AddRange(errors);
            if (canceltasktoken.IsCancellationRequested) return null;
            RequestCustomerLegal item = new RequestCustomerLegal(record.id, record.stamp, lib.DomainObjectState.Unchanged
                , request
                , customerlegal
                , record.selected
                , record.actualweight
                , record.cellnumber
                , record.currencydate
                , record.currencyrate
                , record.invoice
                , record.invoicediscount
                , record.officialweight
                , record.volume);
            if (canceltasktoken.IsCancellationRequested) return null;
            if (item.Id > 0) item = CustomBrokerWpf.References.RequestCustomerLegalStore.UpdateItem(item, this.FillType == lib.FillType.Refresh);

            if (canceltasktoken.IsCancellationRequested) return null;
            if ((this.FillType == lib.FillType.Refresh) & !item.CustomsInvoiceIsNull)
            {
                CustomBrokerWpf.References.CustomsInvoiceStore.UpdateItem(item.CustomsInvoice.Id, addcon, out errors);
                this.Errors.AddRange(errors);
            }
            if (canceltasktoken.IsCancellationRequested) return null;
            mypdbm.Command.Connection = addcon;
            mypdbm.Errors.Clear();
            mypdbm.FillType = this.FillType;
            mypdbm.Collection = null;
            mypdbm.RequestCustomer = item;
            if (canceltasktoken.IsCancellationRequested) return null;
            mypdbm.Fill();
            if (mypdbm.Errors.Count > 0 | canceltasktoken.IsCancellationRequested)
                foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
            else
            {
                if (item.PrepaysIsNull)
                    item.Prepays = mypdbm.Collection;
                else
                {
                    if (canceltasktoken.IsCancellationRequested) return null;
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
            if (canceltasktoken.IsCancellationRequested) return null;
            this.RefreshFund(item, this.Errors, addcon, null);
            mypdbm.Collection = null;
            item.IsLoaded = true;

            return item;
        }
        protected override void GetOutputParametersValue(RequestCustomerLegal item)
        {
            //if (item.Id <= 0)
            //{
            //    item.Id = (int)myinsertparams[0].Value;
            //    CustomBrokerWpf.References.RequestCustomerLegalStore.UpdateItem(item);
            //}
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
                // Удаляем, уничтожаем и обновляем в PrePrepays, иначе удаленные могут быть сохранены из бухг блока
                List<PrepayCustomerRequest> destroyed = new List<PrepayCustomerRequest>();
                foreach (PrepayCustomerRequest prepay in item.PrePrepays)
                    if (prepay.DomainState == lib.DomainObjectState.Modified & prepay.EuroSum == 0M)
                    {
                        prepay.DomainState = lib.DomainObjectState.Deleted;
                        if (mypdbm.SaveItemChanches(prepay))
                            destroyed.Add(prepay);
                        else
                        {
                            isSuccess = false;
                            foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
                        }
                    }
                foreach (PrepayCustomerRequest prepay in destroyed)
                    item.PrePrepays.Remove(prepay);
                this.RefreshFund(item, this.Errors, mypdbm.Command.Connection, mypdbm.Command.Transaction);
            }
            return isSuccess;
        }
        protected override bool SaveIncludedObject(RequestCustomerLegal item)
        {
            bool Success = true;
            if (!item.CustomsInvoiceIsNull)
            {
                mycidbm.Errors.Clear();
                if (!mycidbm.SaveItemChanches(item.CustomsInvoice))
                {
                    Success = false;
                    foreach (lib.DBMError err in mycidbm.Errors) this.Errors.Add(err);
                }
            }
            if(myldbm!=null)
            {
                myldbm.Errors.Clear();
                if (!myldbm.SaveItemChanches(item.CustomerLegal))
                {
                    Success = false;
                    foreach (lib.DBMError err in myldbm.Errors) this.Errors.Add(err);
                }
            }
            return Success;
        }
        protected override bool SaveReferenceObjects()
        {
            mycidbm.Command.Connection = this.Command.Connection;
            mycidbm.Transaction = this.Transaction;
            mypdbm.Command.Connection = this.Command.Connection;
            mypdbm.Transaction = this.Transaction;
            return true;
        }
        protected override bool SetParametersValue(RequestCustomerLegal item)
        {
            foreach (SqlParameter par in myupdateparams)
            {
                switch (par.ParameterName)
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
        //protected override void CancelLoad()
        //{
        //    mypdbm.CancelingLoad = this.CancelingLoad;
        //}
        private void RefreshFund(RequestCustomerLegal requestlegal, List<lib.DBMError> errors, SqlConnection con, SqlTransaction tran)
        {
            if (requestlegal.Request.Status.Id == 0) return;
            bool find = false;
            PrepayFundDBM mypfdbm = new PrepayFundDBM();
            mypfdbm.Customer = requestlegal;
            mypfdbm.Command.Connection = con;
            mypfdbm.Command.Transaction = tran;
            mypfdbm.Fill();
            if (mypfdbm.Errors.Count > 0)
                foreach (lib.DBMError err in mypfdbm.Errors) errors.Add(err);
            this.mydispatcher.Invoke(new Action(() => { requestlegal.PrePrepays.Clear(); })); 
            foreach (Prepay pay in mypfdbm.Collection)
            {
                find = false;
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
                    foreach (PrepayCustomerRequest item in requestlegal.PrePrepays)
                        if (item.Prepay.Id == pay.Id)
                        {
                            find = true;
                            break;
                        }
                if (!find) // BeginInvoke - double items because twice invoke in one time 
                    this.mydispatcher.Invoke(new Action(() =>
                      { requestlegal.PrePrepays.Add(new PrepayCustomerRequest(requestlegal, pay, null)); }));
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
                    ChangingDomainProperty = name; this.DomainObject.CustomsInvoice.Percent = decimal.Divide(value.Value, 100M);
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
            get { return this.IsEnabled ? this.DomainObject.DTSum : null; }
        }
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
                    { ChangingDomainProperty = name; this.DomainObject.UpdateInvoiceDiscount(value, 'l'); myinvoicediscount = null; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? (myinvoicediscount ?? this.DomainObject.InvoiceDiscount) : null; }
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
            //set
            //{
            //    if (!this.IsReadOnly && ((myprepay ?? this.DomainObject.PrepaySum).HasValue != value.HasValue || (value.HasValue && !decimal.Equals((myprepay ?? this.DomainObject.PrepaySum).Value, value.Value))))
            //    {
            //        string name = nameof(this.PrepaySum);
            //        myprepay = value;
            //        if (ValidateProperty(name))
            //        { ChangingDomainProperty = name; this.DomainObject.PrepaySum = value; myprepay = null; this.ClearErrorMessageForProperty(name); }
            //    }
            //}
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
        private PrepayCustomerRequestSynchronizer mypresync;
        private ListCollectionView mypreprepays;
        public ListCollectionView PrePrepays
        {
            get
            {
                if (mypreprepays == null)
                {
                    if (mypresync == null)
                    {
                        mypresync = new PrepayCustomerRequestSynchronizer();
                        mypresync.DomainCollection = this.DomainObject.PrePrepays;
                        mypresync.DomainCollection.CollectionChanged += DomainCollection_CollectionChanged;
                        this.PropertyChangedNotification(nameof(this.PrePrepaysVisibility));
                    }
                    mypreprepays = new ListCollectionView(mypresync.ViewModelCollection);
                    mypreprepays.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    //mypreprepays.CurrentChanged += (object sender, EventArgs e)=> { this.PropertyChangedNotification(nameof(this.PrePrepaysVisibility)); };
                }
                return mypreprepays;
            }
        }
        private void DomainCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.PropertyChangedNotification(nameof(this.PrePrepaysVisibility));
        }
        public Visibility PrePrepaysVisibility
        { get { return mypresync?.DomainCollection.Count > 0 ? Visibility.Visible : Visibility.Collapsed; } }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(RequestCustomerLegal.CustomerLegal):
                    if (mycustomerlegal == null && this.DomainObject.CustomerLegal != null)
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
            if (this.DomainObject.CustomerLegal != null) mycustomerlegal = new CustomerLegalVM(this.DomainObject.CustomerLegal);
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
                                if (prepay.DomainState == lib.DomainObjectState.Deleted)
                                    this.DomainObject.UpdateInvoiceDiscount((this.InvoiceDiscount ?? 0M) + prepay.DomainObject.DTSum, 'p');
                                this.Prepays.EditItem(prepay);
                                prepay.RejectChanges();
                                this.Prepays.CommitEdit();
                            }
                        }
                        foreach (PrepayCustomerRequestVM prepay in removed)
                            if (prepay != null) mypsync.ViewModelCollection.Remove(prepay);
                        this.DomainObject.PropertyChangedNotification(nameof(RequestCustomerLegal.PrepaySum));
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
                            if (item.Prepay.InvoiceDate.HasValue)
                            {
                                errmsg = "На юр. лицо уже выписан счет!";
                                isvalid = false;
                                break;
                            }
                    }
                    break;
                case nameof(this.PrepaySum):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myprepay, out errmsg);
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