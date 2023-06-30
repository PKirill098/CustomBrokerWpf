using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows.Data;
using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using System.ComponentModel;
using System.Threading;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class Prepay : lib.DomainBaseStamp
    {
        private Prepay(int id, long stamp, DateTime? updated, string updater, lib.DomainObjectState mstate
            , decimal? cbrate, DateTime? currencypaiddate, bool dealpassport, decimal eurosum, Importer importer, decimal initsum, DateTime? invoicedate, string invoicenumber, decimal percent, decimal refund, DateTime shipplandate
            ) : base(id, stamp, updated, updater, mstate)
        {
            mycbrate = cbrate;
            mydealpassport = dealpassport;
            myeurosum = eurosum;
            myimporter = importer;
            myinitsum = initsum;
            myinvoicedate = invoicedate;
            myinvoicenumber = invoicenumber;
            mycurrencypaiddate = currencypaiddate;
            mypercent = percent;
            myrefund = refund;
            myshipplandate = shipplandate;

            myrater = new CurrencyRateProxy(CustomBrokerWpf.References.CurrencyRate);
            myrater.PropertyChanged += Rater_PropertyChanged;
        }
        public Prepay(int id, long stamp, DateTime? updated,string updater, lib.DomainObjectState mstate
             , Agent agent, decimal? cbrate, DateTime? currencypaiddate, CustomerLegal customer, bool dealpassport, decimal eurosum, Importer importer, decimal initsum, DateTime? invoicedate, string invoicenumber, decimal percent, decimal refund, DateTime shipplandate
            ) : this(id, stamp, updated, updater, mstate, cbrate, currencypaiddate, dealpassport, eurosum, importer, initsum, invoicedate, invoicenumber, percent, refund, shipplandate)
        {
            myagent = agent;
            mycustomer = customer;
        }
        public Prepay(Agent agent, CustomerLegal customer, Importer importer, DateTime shipplandate) : this(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added
            , agent, null, null, customer,true, 0M, importer, 0M, null, null, 0M,0M, shipplandate) { }
        public Prepay() : this(null, null, null, CustomBrokerWpf.References.EndQuarter(DateTime.Today.AddDays(10))) { }

        private Agent myagent;
        public Agent Agent
        { set { SetProperty<Agent>(ref myagent, value); } get { return myagent; } }
        private decimal? mycbrate;
        public decimal? CBRate
        {
            set { SetProperty<decimal?>(ref mycbrate, value, () => { this.PropertyChangedNotification(nameof(this.CBRatep2p)); this.PropertyChangedNotification(nameof(this.RubSum)); }); }
            get { return myinvoicedate.HasValue ? mycbrate : null ; } //myrater.EURRate
        }
        public decimal? CBRatep2p
        { get { return this.CBRate * 1.02M; } }
        public DateTime? CurrencyBoughtDate
        {
            set
            {
                if (value.HasValue)
                {
                    //if (this.RubSum - this.RubPaySum < 0.0099M)
                    //{
                        if (myeurosum - this.CurrencyBuySum > 0.0099M)
                        {
                            if (!mycurrencybuyrate.HasValue)
                                System.Windows.MessageBox.Show("Покупка валюты не может быть завершена. Валюта куплена не полностью!\nДля покупки валюты воспользуйтесь окном покупок или явно укажите курс покупки.", "Дата покупки валюты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                            else
                            {
                                CurrencyBuyPrepay buy = new CurrencyBuyPrepay(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null, value.Value, mycurrencybuyrate.Value, this.EuroSum - this.CurrencyBuySum,this);
                                this.CurrencyBuys.Add(buy);
                                mycurrencybuyrate = null;
                                this.PropertyChangedNotification(nameof(this.CurrencyBuyRate));
                                this.PropertyChangedNotification(nameof(this.CurrencyBuySum));
                            }
                        }
                        else
                        {
                            DateTime maxdate = DateTime.MinValue;
                            CurrencyBuyPrepay buy = null;
                            foreach (CurrencyBuyPrepay item in this.CurrencyBuys)
                                if (item.BuyDate > maxdate)
                                { maxdate = item.BuyDate; buy = item; }
                            if(buy!=null) buy.BuyDate = value.Value;
                        }
                    //}
                    //else
                    //    System.Windows.MessageBox.Show("Покупка валюты не может быть завершена. Счет оплачен не полностью!", "Дата покупки валюты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                }
                else
                {
                    if (this.CurrencyPays?.Count>0)
                        System.Windows.MessageBox.Show("Невозможно удалить покупку валюты, оплачено поставщику!", "Дата покупки валюты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                    else //if (System.Windows.MessageBox.Show("Удалить все покупки валюты ?", "Дата покупки валюты", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                    {
                        List<CurrencyBuyPrepay> del = new List<CurrencyBuyPrepay>();
                        foreach (CurrencyBuyPrepay item in this.CurrencyBuys)
                            if (item.DomainState == lib.DomainObjectState.Added)
                                del.Add(item);
                            else
                                item.DomainState = lib.DomainObjectState.Deleted;
                        foreach (CurrencyBuyPrepay item in del)
                            this.CurrencyBuys.Remove(item);
                        this.PropertyChangedNotification(nameof(this.CurrencyBuySum));
                    }
                }
                this.PropertyChangedNotification(nameof(this.CurrencyBoughtDate));
            }
            get
            {
                return this.CurrencyBuys.Count>0 && myeurosum - this.CurrencyBuySum < 0.99M ? DateTime.FromOADate(this.CurrencyBuys.Max<CurrencyBuyPrepay>((CurrencyBuyPrepay item) => { return item.BuyDate.ToOADate(); })) : (DateTime?)null;
            }
        }
        private decimal? mycurrencybuyrate;
        public decimal? CurrencyBuyRate
        {
            set { if (!this.CurrencyBoughtDate.HasValue) mycurrencybuyrate = value;this.PropertyChangedNotification(nameof(this.CurrencyBuyRate)); }
            get { return mycurrencybuyrate.HasValue ? mycurrencybuyrate : ((this.CurrencyBuys?.Count ?? 0) > 0 ? this.CurrencyBuys.Sum((CurrencyBuyPrepay buy) => { return decimal.Multiply(buy.BuyRate,decimal.Divide(buy.CurSum,this.CurrencyBuySum)); }) : (decimal?)null); } }
        public decimal CurrencyBuySum
        { get { return mycurrencybuys?.Sum<CurrencyBuyPrepay>((CurrencyBuyPrepay item) => { return item.CurSum; }) ?? 0M; } }
        private DateTime? mycurrencypaiddate;
        public DateTime? CurrencyPaidDate
        {
            set
            {
                if (this.UpdatingSample)
                    SetProperty<DateTime?>(ref mycurrencypaiddate, value);
                else
                {
                    if (value.HasValue)
                    {
                        DateTime maxdate = this.CurrencyPays.Count((PrepayCurrencyPay item) => { return item.DomainState < lib.DomainObjectState.Deleted; }) > 0 ? this.CurrencyPays.Max((PrepayCurrencyPay item) => { return item.DomainState < lib.DomainObjectState.Deleted ? item.PayDate : DateTime.MinValue; }).Date : DateTime.MinValue.Date;
                        if (maxdate > value.Value)
                            System.Windows.MessageBox.Show("Дата оплаты поставщику не может быть меньше даты платежа!", "Дата оплаты поставщику", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                        else
                        {
                            //if (maxdate < value.Value)
                            //{
                            //    AgentCustomerBalanceDBM bdbm = new AgentCustomerBalanceDBM() { Agent = this.Agent, Customer = this.Customer, MinBalance = 0M, Importer = this.Importer };
                            //    decimal balance = bdbm.GetFirst()?.Balance ?? 0M;
                            //    if (this.EuroSum - this.CurrencyPaySum - balance > 0.0099M)
                            //    {
                            //        PrepayCurrencyPay pay = new PrepayCurrencyPay(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null, value.Value, this.EuroSum - this.CurrencyPaySum - balance, this);
                            //        this.CurrencyPays.Add(pay);
                            //        this.PropertyChangedNotification(nameof(this.CurrencyPaySum));
                            //    }
                            //}
                            SetProperty<DateTime?>(ref mycurrencypaiddate, value);
                        }
                    }
                    else
                    {
                        if (mycurrencypaiddate.HasValue && System.Windows.MessageBox.Show("Удалить все оплаты поставщику?", "Дата оплаты поставщику", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                        {
                            List<PrepayCurrencyPay> del = new List<PrepayCurrencyPay>();
                            foreach (PrepayCurrencyPay item in this.CurrencyPays)
                                if (item.DomainState == lib.DomainObjectState.Added)
                                    del.Add(item);
                                else
                                    item.DomainState = lib.DomainObjectState.Deleted;
                            foreach (PrepayCurrencyPay item in del)
                                this.CurrencyPays.Remove(item);
                        }
                        SetProperty<DateTime?>(ref mycurrencypaiddate, value);
                        this.PropertyChangedNotification(nameof(this.CurrencyPaySum));
                    }
                }
            }
            get
            {
                return mycurrencypaiddate;//this.CurrencyPays?.Count > 0 && myeurosum - this.CurrencyPaySum < 0.99M ? DateTime.FromOADate(this.CurrencyPays.Max<PrepayCurrencyPay>((PrepayCurrencyPay item) => { return item.PayDate.ToOADate(); })) : (DateTime?)null;
            }
        }
        public decimal CurrencyPaySum
        { get { return mycurrencypays?.Sum<PrepayCurrencyPay>((PrepayCurrencyPay item) => { return item.CurSum; }) ?? 0M; } }
        private CustomerLegal mycustomer;
        public CustomerLegal Customer
        { set { SetProperty<CustomerLegal>(ref mycustomer, value); } get { return mycustomer; } }
        private bool mydealpassport;
        public bool DealPassport
        {
            set { SetProperty<bool>(ref mydealpassport, value, ()=> { this.PropertyChangedNotification(nameof(this.NotDealPassport)); this.PropertyChangedNotification(nameof(this.ExpiryDate)); }); }
            get { return mydealpassport; }
        }
        public bool NotDealPassport
        { set { this.DealPassport = !value; } get { return !mydealpassport; } }
        private decimal myeurosum;
        public decimal EuroSum
        { set
            {
                SetProperty<decimal>(ref myeurosum, value,()=> { this.PropertyChangedNotification(nameof(this.RubSum)); });
            } get { return myeurosum; } }
        private DateTime? myexpirydate;
        public DateTime? ExpiryDate
        { get { return mydealpassport && (this.CurrencyPays?.Count ?? 0) > 0 ? this.CurrencyPays.Min((PrepayCurrencyPay pay) => { return pay.PayDate; }).AddDays(240):(DateTime?)null; } }
        private decimal? myfundsum;
        public decimal? FundSum
        {
            set 
            {
                //SetProperty<decimal?>(ref myfundsum, value); 
                if (myfundsum != value)
                {
                    myfundsum = value;
                    this.PropertyChangedNotification(nameof(this.FundSum));
                }
            }
            get
            {
                if (myfundsum == null)
                {
                    PrepayFundDBM fdbm = new PrepayFundDBM() { ItemId = this.Id };
                    Prepay fund = fdbm.GetFirst();
                    if (fdbm.Errors.Count > 0)
                        KirillPolyanskiy.Common.PopupCreator.GetPopup(fdbm.ErrorMessage, System.Windows.Media.Brushes.LightCoral);
                    else if (fund == null)
                        myfundsum = 0M;
                }
                return myfundsum;
            }
        }
        internal bool FundSumIsNull
        { get { return myfundsum == null; } }
        private Importer myimporter;
        public Importer Importer
        { set { SetProperty<Importer>(ref myimporter, value); } get { return myimporter; } }
        private decimal myinitsum;
        private DateTime? myinvoicedate;
        public DateTime? InvoiceDate
        {
            set
            {
                Action action = () =>
                {
                    if (this.UpdatingSample) return;
                    this.CBRate = null;
                    this.Percent = this.GetPercent();
                    if (myinvoicedate.HasValue)
                    {
                        myrater.RateDate = myinvoicedate.Value;
                        PrepayCustomerRequestDBM rpdbm = new PrepayCustomerRequestDBM();
                        rpdbm.FillType = lib.FillType.Initial; // Update  from DB no changed
                        rpdbm.Prepay = this;
                        rpdbm.Fill();
                        if (rpdbm.Errors.Count > 0)
                        {
                            System.Windows.Window active = null;
                            foreach (System.Windows.Window win in System.Windows.Application.Current.Windows)
                                if (win.IsActive) { active = win; break; }
                            active.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ContextIdle, new Action(() =>
                            {
                                Common.PopupCreator.GetPopup(text: "Не удалось расчитать сумму счета!/nУдалите дату счета и укажите ее повторно./n" + rpdbm.ErrorMessage
                               , background: System.Windows.Media.Brushes.LightPink
                               , foreground: System.Windows.Media.Brushes.Red
                               , staysopen: false
                               ).IsOpen = true;
                            }));
                        }
                        else
                            this.EuroSum = rpdbm.Collection.Sum((PrepayCustomerRequest rp) => { return rp.EuroSum; });
                    }
                    else
                        this.EuroSum = 0M;
                };
                SetProperty<DateTime?>(ref myinvoicedate, value, action);
                EventLoger log = new EventLoger() { What = "Prepay", Message = (myinvoicedate?.ToShortDateString() ?? "NULL") + " : UpdatingSample " + this.UpdatingSample.ToString(), ObjectId = this.Id };
                log.Execute();
            }
            get { return myinvoicedate; }
        }
        private string myinvoicenumber;
        public string InvoiceNumber
        { set { SetProperty<string>(ref myinvoicenumber, value); } get { return myinvoicenumber; } }
        internal bool? IsPrepay
        { set; get; }
        private decimal mypercent;
        public decimal Percent
        { 
            set { if(value >= 1M) value = value / 100M; SetProperty<decimal>(ref mypercent, value, () => { this.PropertyChangedNotification(nameof(this.RubSum)); }); }
            get { return mypercent; }
        }
        public decimal Percent100
        {
            set { if (value >= 1M) value = value / 100M; SetProperty<decimal>(ref mypercent, value, () => { this.PropertyChangedNotification(nameof(this.RubSum)); }); }
            get { return mypercent*100M; }
        }
        public decimal? RateDiffPer
        { get { return this.CBRatep2p.HasValue && this.CurrencyBoughtDate.HasValue && this.CurrencyBuyRate.HasValue ? decimal.Divide(this.CurrencyBuyRate.Value, this.CBRatep2p.Value) - 1M : (decimal?)null; } }
        public bool? RateDiffResult
        { get { return this.RateDiffPer.HasValue ? this.RateDiffPer<=0.005M : (bool?)null ; } }
        public string RateDiffResultR
        { get { return this.RateDiffPer.HasValue ? (this.RateDiffPer <= 0.005M ? "ИСТИНА" : "ЛОЖЬ") : null; } }
        private decimal myrefund;
        public decimal Refund
        { set { SetProperty<decimal>(ref myrefund, value); } get { return myrefund; } }
        public decimal RubDebt
        { get { return (this.RubSum??0M) - this.RubPaySum; } }
        public decimal RubPaySum
        { get { return this.RubPays?.Sum<PrepayRubPay>((PrepayRubPay item) => { return item.DomainState<lib.DomainObjectState.Deleted ? item.PaySum: 0M; }) ?? 0M; } }
        public DateTime? RubPaidDate
        {
            set
            {
                if (value.HasValue)
                {
                    if (this.InvoiceDate.HasValue)
                    {
                        if (this.RubSum - this.RubPaySum > 0.0099M)
                        {
                            PrepayRubPay pay = new PrepayRubPay(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null, value.Value, this, this.RubSum.Value - this.RubPaySum);
                            this.RubPays.Add(pay);
                            this.PropertyChangedNotification(nameof(this.RubPaySum));
                        }
                        else
                        {
                            DateTime maxdate = DateTime.MinValue;
                            PrepayRubPay pay = null;
                            foreach (PrepayRubPay item in this.RubPays)
                                if (item.PayDate > maxdate)
                                { maxdate = item.PayDate; pay = item; }
                            pay.PayDate = value.Value;
                        }
                    }
                    else
                        System.Windows.MessageBox.Show("Перед внесением платежей необходимо указать дату выставления счета!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                }
                else
                {
                    if (this.CurrencyBuys?.Count > 0)
                        System.Windows.MessageBox.Show("Невозможно удалить платежи, куплена валюта!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                    else if (System.Windows.MessageBox.Show("Удалить все оплаты ?", "Дата оплаты", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                    {
                        List<PrepayRubPay> del = new List<PrepayRubPay>();
                        foreach (PrepayRubPay item in this.RubPays)
                            if (item.DomainState == lib.DomainObjectState.Added)
                                del.Add(item);
                            else
                                item.DomainState = lib.DomainObjectState.Deleted;
                        foreach (PrepayRubPay item in del)
                            this.RubPays.Remove(item);
                        this.PropertyChangedNotification(nameof(this.RubPaySum));
                    }
                }
                this.PropertyChangedNotification(nameof(this.RubPaidDate));
            }
            get
            {
                return this.RubPays.Count>0 && this.RubSum - this.RubPaySum < 0.99M ? DateTime.FromOADate(this.RubPays.Max<PrepayRubPay>((PrepayRubPay item) => { return item.PayDate.ToOADate(); })) : (DateTime?)null;
            }
        }
        public decimal? RubSum
        {
            get
            {
                decimal? sum = myeurosum * (1M + this.Percent) * this.CBRate;
                if (sum.HasValue)
                    sum = decimal.Round(sum.Value);
                return sum;
            }
        }
        public bool Selected { set; get; }
        private DateTime myshipplandate;
        public DateTime ShipPlanDate
        { set { SetProperty<DateTime>(ref myshipplandate, value); } get { return myshipplandate; } }

        private ObservableCollection<PrepayCustomerRequest> myrequestprepay;
        private ObservableCollection<PrepayRubPay> myrubpays; //created at boot
        internal ObservableCollection<PrepayRubPay> RubPays
        {
            set { myrubpays = value; this.PropertyChangedNotification(nameof(this.RubPaySum)); }
            get
            {
                return myrubpays;
            }
        }
        private ObservableCollection<CurrencyBuyPrepay> mycurrencybuys; //created at boot
        internal ObservableCollection<CurrencyBuyPrepay> CurrencyBuys
        {
            set { mycurrencybuys = value; this.PropertyChangedNotification(nameof(this.CurrencyBuySum)); }
            get
            {
                return mycurrencybuys;
            }
        }
        private ObservableCollection<PrepayCurrencyPay> mycurrencypays; //created at boot
        internal ObservableCollection<PrepayCurrencyPay> CurrencyPays
        {
            set { mycurrencypays = value; this.PropertyChangedNotification(nameof(this.CurrencyPaySum)); }
            get
            {
                return mycurrencypays;
            }
        }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Prepay templ = sample as Prepay;
            this.Agent = templ.Agent;
            this.CBRate = templ.CBRate;
            this.Customer = templ.Customer;
            this.DealPassport = templ.DealPassport;
            this.EuroSum = templ.EuroSum;
            if (templ.FundSumIsNull) // не запрашивать когда не нужна
            { 
                this.FundSum = null; // refresh
            }
            else
                this.FundSum = templ.FundSum + templ.EuroSum - this.EuroSum;
            this.Importer = templ.Importer;
            this.InvoiceDate = templ.InvoiceDate;
            this.InvoiceNumber = templ.InvoiceNumber;
            this.CurrencyPaidDate = templ.CurrencyPaidDate;
            this.Percent = templ.Percent;
            this.Refund = templ.Refund;
            this.ShipPlanDate = templ.ShipPlanDate;
            this.UpdateWhen = templ.UpdateWhen;
            this.UpdateWho = templ.UpdateWho;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Agent):
                    this.Agent = (Agent)value;
                    break;
                case nameof(this.CBRate):
                    this.CBRate = (decimal?)value;
                    break;
                case nameof(this.Customer):
                    this.Customer = (CustomerLegal)value;
                    break;
                case nameof(this.DealPassport):
                    this.DealPassport = (bool)value;
                    break;
                case nameof(this.EuroSum):
                    this.EuroSum = (decimal)value;
                    break;
                case nameof(this.Importer):
                    this.Importer = (Importer)value;
                    break;
                case nameof(this.InvoiceDate):
                    this.InvoiceDate = (DateTime?)value;
                    break;
                case nameof(this.InvoiceNumber):
                    this.InvoiceNumber = (string)value;
                    break;
                case nameof(this.CurrencyPaidDate):
                    this.CurrencyPaidDate = (DateTime?)value;
                    break;
                case nameof(this.Percent):
                    this.Percent = (decimal)value;
                    break;
                case nameof(this.Refund):
                    this.Refund = (decimal)value;
                    break;
                case nameof(this.ShipPlanDate):
                    this.ShipPlanDate = (DateTime)value;
                    break;
                case "DependentNew":
                    if (myrubpays != null)
                    {
                        int i = 0;
                        PrepayRubPay[] removed = new PrepayRubPay[myrubpays.Count];
                        foreach (PrepayRubPay item in myrubpays)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                removed[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach (PrepayRubPay item in removed)
                            if (item != null) myrubpays.Remove(item);
                    }
                    if (mycurrencybuys != null)
                    {
                        int i = 0;
                        CurrencyBuyPrepay[] removed = new CurrencyBuyPrepay[mycurrencybuys.Count];
                        foreach (CurrencyBuyPrepay item in mycurrencybuys)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                removed[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach (CurrencyBuyPrepay item in removed)
                            if (item != null) mycurrencybuys.Remove(item);
                    }
                    if (mycurrencypays != null)
                    {
                        int i = 0;
                        PrepayCurrencyPay[] rubpayremoved = new PrepayCurrencyPay[mycurrencypays.Count];
                        foreach (PrepayCurrencyPay item in mycurrencypays)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                rubpayremoved[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach (PrepayCurrencyPay item in rubpayremoved)
                            if (item != null) mycurrencypays.Remove(item);
                    }
                    break;
            }
        }
        public override bool ValidateProperty(string propertyname, object value, out string errmsg, out byte messageey)
        {
            bool isvalid = true;
            errmsg = null;
            messageey = 0;
            switch (propertyname)
            {
                case nameof(this.CurrencyBoughtDate):
                    DateTime? date = (DateTime?)value;
                    if (date.HasValue)
                    {
                        //if (this.RubSum - this.RubPaySum < 0.99M)
                        //{
                        //    if (myeurosum - this.CurrencyBuySum > 0.0099M)
                        //        errmsg="Покупка валюты не может быть завершена. Валюта куплена не полностью!";
                        //}
                        //else
                        //    errmsg = "Покупка валюты не может быть завершена. Счет оплачен не полностью!";
                    }
                    else if (this.CurrencyPaidDate.HasValue)
                        errmsg = "Невозможно удалить покупку валюты, оплачено поставщику!";
                    break;
                case nameof(this.RubPaidDate):
                    date = (DateTime?)value;
                    if (date.HasValue)
                    {
                        if (!this.RubSum.HasValue)
                            errmsg = "Перед внесением платежей необходимо указать дату выставления счета!";
                    }
                    else
                    {
                        if (this.CurrencyBuys?.Count > 0)
                            errmsg = "Невозможно удалить платежи, куплена валюта!";
                    }
                    break;
                case nameof(this.CurrencyPaidDate):
                    date = (DateTime?)value;
                    if (date.HasValue)
                    {
                        DateTime? maxdate = null;
                        if(this.CurrencyPays.Count()>0)
                            maxdate = this.CurrencyPays.Max((PrepayCurrencyPay item) => { return item.PayDate; }).Date;
                        if (maxdate.HasValue && maxdate.Value > date.Value)
                            errmsg = "Дата оплаты поставщику не может быть меньше даты платежа!";
                    }
                    break;
            }
            return isvalid;
        }

        private decimal GetPercent()
        {
            return myshipplandate.Month / 4 == (myinvoicedate ?? DateTime.Today).Month / 4 ? 0.02M : 0.22M;
        }
        private Classes.CurrencyRateProxy myrater;
        private void Rater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EURRate" & myinvoicedate.HasValue)
            {
                this.CBRate = myrater.EURRate;
            }
        }
    }
    
    public struct PrepayRecord
    {
		internal int id;
		internal long stamp;
		internal DateTime? updated;
		internal string updater;
        internal int agent;
		internal decimal? cbrate;
		internal DateTime? paydate;
        internal int customer;
		internal bool dealpass;
		internal decimal eurosum;
		internal int importer;
		internal decimal initsum;
		internal DateTime? invoicedate;
		internal string invoicenumber;
		internal decimal percent;
		internal decimal refund;
        internal DateTime shipplandate;
        internal decimal? fundsum;
        internal bool? isprepay;
	}

    internal class PrepayStore : lib.DomainStorageLoad<PrepayRecord,Prepay, PrepayDBM>
    {
        public PrepayStore(PrepayDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(Prepay olditem, Prepay newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class PrepayDBM : lib.DBManagerStamp<PrepayRecord,Prepay>
    {
        public PrepayDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "account.Prepay_sp";
            InsertCommandText = "account.PrepayAdd_sp";
            UpdateCommandText = "account.PrepayUpd_sp";
            DeleteCommandText = "account.PrepayDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int)/*, new SqlParameter("@requestid", System.Data.SqlDbType.Int), new SqlParameter("@parcelid", System.Data.SqlDbType.Int)*/ };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@agentidupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@cbrateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@customeridupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@dealpassupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@eurosumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@importeridupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@initsumupd",System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicedateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicenumberupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@paydateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@percentupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@refundupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@shipplandateupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
               myinsertupdateparams[0]
               ,new SqlParameter("@agentid",System.Data.SqlDbType.Int)
               ,new SqlParameter("@cbrate",System.Data.SqlDbType.Money)
               ,new SqlParameter("@customerid",System.Data.SqlDbType.Int)
               ,new SqlParameter("@dealpass", System.Data.SqlDbType.Bit)
               ,new SqlParameter("@eurosum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@importerid",System.Data.SqlDbType.Int)
               ,new SqlParameter("@initsum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@invoicedate",System.Data.SqlDbType.DateTime2)
               ,new SqlParameter("@invoicenumber", System.Data.SqlDbType.NVarChar,10)
               ,new SqlParameter("@paydate",System.Data.SqlDbType.DateTime2)
               ,new SqlParameter("@percent",System.Data.SqlDbType.Decimal){Scale=9,Precision=9}
               ,new SqlParameter("@refund",System.Data.SqlDbType.Money)
               ,new SqlParameter("@shipplandate",System.Data.SqlDbType.DateTime2)
             };
            myrdbm = new PrepayRubPayDBM();
            mycbdbm = new CurrencyBuyPrepayDBM();
            mycpdbm = new PrepayCurrencyPayDBM();
        }

        private PrepayRubPayDBM myrdbm;
        private CurrencyBuyPrepayDBM mycbdbm;
        private PrepayCurrencyPayDBM mycpdbm;

        protected override PrepayRecord CreateRecord(SqlDataReader reader)
        {
            return new PrepayRecord()
            { id=reader.GetInt32(0), stamp=reader.GetInt64(this.Fields["stamp"]), updated=reader.GetDateTime(this.Fields["updated"]), updater=reader.GetString(this.Fields["updater"])
                , agent= reader.GetInt32(this.Fields["agentid"])
                , cbrate=reader.IsDBNull(this.Fields["cbrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["cbrate"])
                , paydate=reader.IsDBNull(this.Fields["paydate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["paydate"])
                , customer= reader.GetInt32(this.Fields["customerid"])
                , dealpass=reader.GetBoolean(this.Fields["dealpass"])
                , eurosum=reader.GetDecimal(this.Fields["eurosum"])
                , importer=reader.GetInt32(this.Fields["importerid"])
                , initsum=reader.GetDecimal(this.Fields["initsum"])
                , invoicedate=reader.IsDBNull(this.Fields["invoicedate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["invoicedate"])
                , invoicenumber=reader.IsDBNull(this.Fields["invoicenumber"]) ? null : reader.GetString(this.Fields["invoicenumber"])
                , percent=reader.GetDecimal(this.Fields["percent"])
                , refund=reader.GetDecimal(this.Fields["refund"])
                , shipplandate=reader.GetDateTime(this.Fields["shipplandate"])
            };
        }
		protected override Prepay CreateModel(PrepayRecord record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			List<lib.DBMError> errors;
			Agent agent = CustomBrokerWpf.References.AgentStore.GetItemLoad(record.agent, addcon, out errors);
			this.Errors.AddRange(errors);
			CustomerLegal customer = CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(record.customer, addcon, out errors);
			this.Errors.AddRange(errors);
			Prepay item = new Prepay(record.id, record.stamp, record.updated, record.updater, lib.DomainObjectState.Unchanged
				, agent
				, record.cbrate
				, record.paydate
				, customer
				, record.dealpass
				, record.eurosum
				, CustomBrokerWpf.References.Importers.FindFirstItem("Id", record.importer)
				, record.initsum
				, record.invoicedate
				, record.invoicenumber
				, record.percent
				, record.refund
				, record.shipplandate);

			myrdbm.Errors.Clear();
			mycbdbm.Errors.Clear();
			mycpdbm.Errors.Clear();
			myrdbm.Prepay = item;
			mycbdbm.Prepay = item;
			mycpdbm.Prepay = item;
			if (item.RubPays != null)
			{
				myrdbm.Collection = item.RubPays;
				myrdbm.Fill();
			}
			else
			{
				myrdbm.Fill();
				item.RubPays = myrdbm.Collection;
			}
			if (item.CurrencyBuys != null)
			{
				mycbdbm.Collection = item.CurrencyBuys;
				mycbdbm.Fill();
			}
			else
			{
				mycbdbm.Fill();
				item.CurrencyBuys = mycbdbm.Collection;
			}
			if (item.CurrencyPays != null)
			{
				mycpdbm.Collection = item.CurrencyPays;
				mycpdbm.Fill();
			}
			else
			{
				mycpdbm.Fill();
				item.CurrencyPays = mycpdbm.Collection;
			}
			item = CustomBrokerWpf.References.PrepayStore.UpdateItem(item, this.FillType == lib.FillType.Refresh);
			myrdbm.Collection = null;
			mycbdbm.Collection = null;
			mycpdbm.Collection = null;
			foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
			foreach (lib.DBMError err in mycbdbm.Errors) this.Errors.Add(err);
			foreach (lib.DBMError err in mycpdbm.Errors) this.Errors.Add(err);

			return item;
		}
		protected override void GetOutputSpecificParametersValue(Prepay item)
        {
            if(item.DomainState==lib.DomainObjectState.Added)
                CustomBrokerWpf.References.PrepayStore.UpdateItem(item);
        }
        protected override bool SaveChildObjects(Prepay item)
        {
            bool isSuccess = true;
            myrdbm.Errors.Clear();
            myrdbm.Collection = item.RubPays;
            if (!myrdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
            }
            mycbdbm.Errors.Clear();
            mycbdbm.Collection = item.CurrencyBuys;
            if (!mycbdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in mycbdbm.Errors) this.Errors.Add(err);
            }
            mycpdbm.Errors.Clear();
            mycpdbm.Collection = item.CurrencyPays;
            if (!mycpdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in mycpdbm.Errors) this.Errors.Add(err);
            }
            return isSuccess;
        }
        protected override bool SaveIncludedObject(Prepay item)
        {
            bool success = true;
            if (item?.Agent.Id < 0)
            {
                AgentDBM adbm = new AgentDBM();
                adbm.Command.Connection = this.Command.Connection;
                if (!adbm.SaveItemChanches(item.Agent))
                {
                    foreach (lib.DBMError err in adbm.Errors) this.Errors.Add(err);
                    success = false;
                }
            }
            if (item?.Customer.DomainState == lib.DomainObjectState.Added)
            {
                CustomerLegalDBM ldbm = new CustomerLegalDBM();
                ldbm.Command.Connection = this.Command.Connection;
                if (!ldbm.SaveItemChanches(item.Customer))
                {
                    foreach (lib.DBMError err in ldbm.Errors) this.Errors.Add(err);
                    success = false;
                }
            }
            return success;
        }
        protected override bool SaveReferenceObjects()
        {
            myrdbm.Command.Connection = this.Command.Connection;
            mycbdbm.Command.Connection = this.Command.Connection;
            mycpdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            myrdbm.Command.Connection = addcon;
            mycbdbm.Command.Connection = addcon;
            mycpdbm.Command.Connection = addcon;
            this.Command.CommandTimeout = 1000;
        }
        protected override bool SetSpecificParametersValue(Prepay item)
        {
            foreach (SqlParameter par in this.InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@agentid":
                        par.Value = item.Agent?.Id;
                        break;
                    case "@cbrate":
                        par.Value = item.CBRate;
                        break;
                    case "@customerid":
                        par.Value = item.Customer?.Id;
                        break;
                    case "@dealpass":
                        par.Value = item.DealPassport;
                        break;
                    case "@eurosum":
                        par.Value = item.EuroSum;
                        break;
                    case "@importerid":
                        par.Value = item.Importer?.Id;
                        break;
                    case "@invoicedate":
                        par.Value = item.InvoiceDate;
                        break;
                    case "@invoicenumber":
                        par.Value = item.InvoiceNumber;
                        break;
                    case "@paydate":
                        par.Value = item.CurrencyPaidDate;
                        break;
                    case "@percent":
                        par.Value = item.Percent;
                        break;
                    case "@refund":
                        par.Value = item.Refund;
                        break;
                    case "@shipplandate":
                        par.Value = item.ShipPlanDate;
                        break;
                }
            foreach (SqlParameter par in this.UpdateParams)
                switch (par.ParameterName)
                {
                    case "@agentidupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.Agent));
                        break;
                    case "@cbrateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.CBRate));
                        break;
                    case "@customeridupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.Customer));
                        break;
                    case "@dealpassupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.DealPassport));
                        break;
                    case "@eurosumupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.EuroSum));
                        break;
                    case "@importeridupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.Importer));
                        break;
                    case "@invoicedateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.InvoiceDate));
                        break;
                    case "@invoicenumberupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.InvoiceNumber));
                        break;
                    case "@paydateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.CurrencyPaidDate));
                        break;
                    case "@percentupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.Percent));
                        break;
                    case "@refundupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.Refund));
                        break;
                    case "@shipplandateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(Prepay.ShipPlanDate));
                        break;
                }
            return item.Agent?.Id > 0 & item.Customer?.Id > 0;
        }
    }

    internal class PrepayFundDBM : PrepayDBM
    {
        internal PrepayFundDBM() : base()
        {
            SelectCommandText = "account.PrepayFund_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int), new SqlParameter("@agentid", System.Data.SqlDbType.Int), new SqlParameter("@customerid", System.Data.SqlDbType.Int), new SqlParameter("@importerid", System.Data.SqlDbType.Int) };
        }

        private RequestCustomerLegal mycustomer;
        internal RequestCustomerLegal Customer
        { set { mycustomer = value; } }

		protected override PrepayRecord CreateRecord(SqlDataReader reader)
		{
			PrepayRecord prepay=base.CreateRecord(reader);
            prepay.fundsum= reader.IsDBNull(reader.GetOrdinal("fundsum")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("fundsum"));
            prepay.isprepay= reader.IsDBNull(reader.GetOrdinal("isprepay")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("isprepay"));
			return prepay;
		}
		protected override Prepay CreateModel(PrepayRecord record,SqlConnection addcon, CancellationToken mycanceltasktoken = default)
        {
            Prepay prepay = base.CreateModel(record, addcon, mycanceltasktoken);
            prepay.FundSum = record.fundsum;
            prepay.IsPrepay = record.isprepay;
            return prepay;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            foreach (SqlParameter par in this.SelectParams)
                switch (par.ParameterName)
                {
                    case "@agentid":
                        par.Value = mycustomer?.Request?.AgentId;
                        break;
                    case "@customerid":
                        par.Value = mycustomer?.CustomerLegal?.Id;
                        break;
                    case "@importerid":
                        par.Value = mycustomer?.Request?.Importer?.Id;
                        break;
                }
        }
    }

    //internal class PrepayCurrencyPayDBM : PrepayDBM
    //{
    //    internal PrepayCurrencyPayDBM()
    //    {
    //        SelectCommandText = "account.CurrencyPay_sp";
    //        SelectParams = new SqlParameter[] { new SqlParameter("@agentid", System.Data.SqlDbType.Int), new SqlParameter("@importerid", System.Data.SqlDbType.Int) };
    //    }

    //    private Agent myagent;
    //    internal Agent Agent
    //    {
    //        set { myagent = value; }
    //        get
    //        {
    //            return myagent;
    //        }
    //    }
    //    private Importer myimporter;
    //    internal Importer Importer
    //    { set { myimporter = value; } get { return myimporter; } }

    //    protected override void SetSelectParametersValue(SqlConnection addcon)
    //    {
    //        foreach (SqlParameter par in this.SelectParams)
    //            switch (par.ParameterName)
    //            {
    //                case "@agentid":
    //                    par.Value = myagent?.Id;
    //                    break;
    //                case "@importerid":
    //                    par.Value = myimporter?.Id;
    //                    break;
    //            }
    //    }
    //}

    public class PrepayVM : lib.ViewModelErrorNotifyItem<Prepay>, lib.Interfaces.ITotalValuesItem
    {
        internal PrepayVM(Prepay model) : base(model)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.CurrencyPaidDate) });
            InitProperties();
        }

        private DateTime? mycurrencyboughtdate;
        public DateTime? CurrencyBoughtDate
        {
            set
            {
                if (!this.IsReadOnly && (mycurrencyboughtdate.HasValue != value.HasValue || (value.HasValue && DateTime.Equals(mycurrencyboughtdate, value.Value))))
                {
                    string name = nameof(this.CurrencyBoughtDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyBoughtDate);
                    mycurrencyboughtdate = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.CurrencyBoughtDate = value.Value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? mycurrencyboughtdate : (DateTime?)null; }
        }
        public decimal? CurrencyBuyRate
        {
            set
            {
                if (!this.IsReadOnly)
                {
                    string name = nameof(this.CurrencyBuyRate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyBuyRate);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyBuyRate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyBuyRate : (decimal?)null; }
        }
        private DateTime? mypaydate;
        public DateTime? CurrencyPaidDate
        {
            set
            {
                if (!this.IsReadOnly && (mypaydate.HasValue != value.HasValue || (value.HasValue && DateTime.Equals(mypaydate, value.Value))))
                {
                    string name = nameof(this.CurrencyPaidDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyPaidDate);
                    mypaydate = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.CurrencyPaidDate = value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? mypaydate : (DateTime?)null; }
        }
        public CustomerLegal Customer
        {
            get { return this.IsEnabled ? this.DomainObject.Customer : null; }
        }
        public bool NotDealPassport
        {
            set
            {
                if (!this.IsReadOnly && bool.Equals(this.DomainObject.DealPassport, value))
                {
                    string name = nameof(this.NotDealPassport);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DealPassport);
                    ChangingDomainProperty = name; this.DomainObject.DealPassport = !value;
                }
            }
            get { return this.IsEnabled ? !this.DomainObject.DealPassport : false; } }
        public decimal? EuroSum
        {
            set
            {
                if (!this.IsReadOnly && value.HasValue && !decimal.Equals(this.DomainObject.EuroSum, value.Value))
                {
                    string name = nameof(this.EuroSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.EuroSum);
                    ChangingDomainProperty = name; this.DomainObject.EuroSum = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.EuroSum : (decimal?)null; }
        }
        public DateTime? ExpiryDate
        { get { return this.IsEnabled ? this.DomainObject.ExpiryDate : (DateTime?)null; } }
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
        public decimal? RateDiffPer
        { get { return this.IsEnabled ? this.DomainObject.RateDiffPer : (decimal?)null; } }
        public string RateDiffResult
        { get { return this.DomainObject.RateDiffResult.HasValue ? (this.DomainObject.RateDiffResult.Value ? "ИСТИНА":"ЛОЖЬ") : string.Empty ; } }
        public decimal? Refund
        { get { return this.IsEnabled ? this.DomainObject.Refund : (decimal?)null; } }
        public decimal? RubDebt
        { get { return this.IsEnabled ? this.DomainObject.RubDebt : (decimal?)null; } }
        private DateTime? myrubpaiddate;
        public DateTime? RubPaidDate
        {
            set
            {
                if (!this.IsReadOnly && (myrubpaiddate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(myrubpaiddate, value.Value))))
                {
                    string name = nameof(this.RubPaidDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.RubPaidDate);
                    myrubpaiddate = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.RubPaidDate = value.Value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? myrubpaiddate : (DateTime?)null; }
        }
        public DateTime? ShipPlanDate
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(this.DomainObject.ShipPlanDate, value.Value)))
                {
                    string name = nameof(this.ShipPlanDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ShipPlanDate);
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.ShipPlanDate = value.Value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ShipPlanDate : (DateTime?)null; }
        }

        public bool ProcessedIn { set; get; }
        public bool ProcessedOut { set; get; }
        public bool Selected
        {
            set
            {
                bool oldvalue = this.DomainObject.Selected; this.DomainObject.Selected = value; this.OnValueChanged(nameof(this.Selected), oldvalue, value);
            }
            get { return this.DomainObject.Selected; }
        }

        protected override bool DirtyCheckProperty()
        {
            return mypaydate != this.DomainObject.CurrencyPaidDate || myrubpaiddate!=this.DomainObject.RubPaidDate || mycurrencyboughtdate!=this.DomainObject.CurrencyBoughtDate;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(this.DomainObject.CurrencyPaidDate):
                    mypaydate = this.DomainObject.CurrencyPaidDate;
                    break;
                case nameof(this.DomainObject.DealPassport):
                    this.PropertyChangedNotification(nameof(this.NotDealPassport));
                    break;
                case nameof(this.DomainObject.RubPaidDate):
                    myrubpaiddate = this.DomainObject.RubPaidDate;
                    break;
                case nameof(this.DomainObject.CurrencyBoughtDate):
                    mycurrencyboughtdate = this.DomainObject.CurrencyBoughtDate;
                    break;
            }
        }
        protected override void InitProperties()
        {
            this.DomainObject.Selected = true;
            mypaydate = this.DomainObject.CurrencyPaidDate;
            myrubpaiddate = this.DomainObject.RubPaidDate;
            mycurrencyboughtdate = this.DomainObject.CurrencyBoughtDate;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.CurrencyBoughtDate):
                    if (mycurrencyboughtdate != this.DomainObject.CurrencyBoughtDate)
                        mycurrencyboughtdate = this.DomainObject.CurrencyBoughtDate;
                    else
                        this.CurrencyBoughtDate = (DateTime)value;
                    break;
                case nameof(this.CurrencyBuyRate):
                    this.DomainObject.CurrencyBuyRate = (decimal?)value;
                    break;
                case nameof(this.CurrencyPaidDate):
                    if (mypaydate != this.DomainObject.CurrencyPaidDate)
                        mypaydate = this.DomainObject.CurrencyPaidDate;
                    else
                        this.CurrencyPaidDate = (DateTime?)value;
                    break;
                case nameof(this.NotDealPassport):
                    this.DomainObject.DealPassport = !(bool)value;
                    break;
                case nameof(this.EuroSum):
                    this.DomainObject.EuroSum = (decimal)value;
                    break;
                case nameof(this.Importer):
                    this.DomainObject.Importer = (Importer)value;
                    break;
                case nameof(this.RubPaidDate):
                    if (myrubpaiddate != this.DomainObject.RubPaidDate)
                        myrubpaiddate = this.DomainObject.RubPaidDate;
                    else
                        this.RubPaidDate = (DateTime)value;
                    break;
                case nameof(this.ShipPlanDate):
                    this.DomainObject.ShipPlanDate = (DateTime)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.RubPaidDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myrubpaiddate, out errmsg, out _);
                    break;
                case nameof(this.CurrencyBoughtDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mycurrencyboughtdate, out errmsg, out _);
                    break;
            }
            if(isvalid)
                this.ClearErrorMessageForProperty(propertyname);
            else if (inform) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    public class PrepaySynchronizer : lib.ModelViewCollectionsSynchronizer<Prepay, PrepayVM>
    {
        protected override Prepay UnWrap(PrepayVM wrap)
        {
            return wrap.DomainObject as Prepay;
        }
        protected override PrepayVM Wrap(Prepay fill)
        {
            return new PrepayVM(fill);
        }
    }

    //public class CurrencyPayViewCommand : lib.ViewModelViewCommand
    //{
    //    internal CurrencyPayViewCommand(Agent agent, Importer importer) : base()
    //    {
    //        mymaindbm = new PrepayCurrencyPayDBM();
    //        mydbm = mymaindbm;
    //        mymaindbm.Agent = agent;
    //        mymaindbm.Importer = importer;
    //        mymaindbm.SaveFilter = (Prepay item) => { return item.Selected; };
    //        if (agent != null & importer != null)
    //            mymaindbm.Fill();
    //        else
    //            mymaindbm.Collection = new ObservableCollection<Prepay>();
    //        mysync = new PrepaySynchronizer();
    //        mysync.DomainCollection = mymaindbm.Collection;
    //        base.Collection = mysync.ViewModelCollection;
    //        mytotal = new CurrencyPayTotal(myview);
    //        if (mymaindbm.Errors.Count > 0)
    //            this.OpenPopup(mymaindbm.ErrorMessage, true);
    //    }

    //    private PrepayCurrencyPayDBM mymaindbm;
    //    private PrepaySynchronizer mysync;
    //    private CurrencyPayTotal mytotal;
    //    public CurrencyPayTotal Total { get { return mytotal; } }
    //    private ListCollectionView myagents;
    //    public ListCollectionView Agents
    //    {
    //        get
    //        {
    //            if (myagents == null)
    //            {
    //                myagents = new ListCollectionView(CustomBrokerWpf.References.AgentNames);
    //                myagents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
    //            }
    //            return myagents;
    //        }
    //    }
    //    public int AgentId
    //    {
    //        set
    //        {
    //            mymaindbm.Agent = CustomBrokerWpf.References.AgentStore.GetItemLoad(value);
    //            mymaindbm.Fill();
    //        }
    //        get { return mymaindbm.Agent?.Id ?? 0; }
    //    }
    //    internal Importer Importer
    //    { get { return mymaindbm.Importer; } }

    //    protected override bool CanAddData(object parametr)
    //    {
    //        return false;
    //    }
    //    protected override bool CanDeleteData(object parametr)
    //    {
    //        return false;
    //    }
    //    protected override bool CanRefreshData()
    //    {
    //        return true; ;
    //    }
    //    protected override bool CanRejectChanges()
    //    {
    //        return true;
    //    }
    //    protected override bool CanSaveDataChanges()
    //    {
    //        return true;
    //    }
    //    protected override void OtherViewRefresh()
    //    {
    //    }
    //    protected override void RefreshData(object parametr)
    //    {
    //        mymaindbm.Fill();
    //    }
    //    public override bool SaveDataChanges()
    //    {
    //        foreach (PrepayVM item in mysync.ViewModelCollection)
    //            if (item.Selected) { item.PayDate = this.Total.PayDate; }
    //        return base.SaveDataChanges();
    //    }
    //    protected override void SettingView()
    //    {
    //        myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Customer. Name", System.ComponentModel.ListSortDirection.Ascending));
    //    }
    //}

    //public class CurrencyPayTotal : lib.TotalValues.TotalViewValues<PrepayVM>
    //{
    //    internal CurrencyPayTotal(ListCollectionView view) : base(view)
    //    {
    //        myinitselected = 2; // if not selected - sum=0
    //        myselectedcount = view.Count + myinitselected; // start with select all
    //        mypaydate = DateTime.Today;
    //    }

    //    private DateTime mypaydate;
    //    public DateTime PayDate
    //    {
    //        set
    //        {
    //            if (DateTime.Equals(mypaydate, value))
    //            {
    //                mypaydate = value;
    //            }
    //        }
    //        get { return mypaydate; }
    //    }

    //    private int myitemcount;
    //    public int ItemCount { set { myitemcount = value; } get { return myitemcount; } }
    //    private decimal mytotalcost;
    //    public decimal TotalCost { set { mytotalcost = value; } get { return mytotalcost; } }

    //    protected override void Item_ValueChangedHandler(PrepayVM sender, ValueChangedEventArgs<object> e)
    //    {
    //        decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
    //        switch (e.PropertyName)
    //        {
    //            case nameof(PrepayCurrencyBuyVM.CurSum):
    //                mytotalcost += newvalue - oldvalue;
    //                PropertyChangedNotification(nameof(TotalCost));
    //                break;
    //        }
    //    }
    //    protected override void ValuesReset()
    //    {
    //        myitemcount = 0;
    //        mytotalcost = 0M;
    //    }
    //    protected override void ValuesPlus(PrepayVM item)
    //    {
    //        myitemcount++;
    //        mytotalcost = mytotalcost + (item.DomainObject.EuroSum);
    //    }
    //    protected override void ValuesMinus(PrepayVM item)
    //    {
    //        myitemcount--;
    //        mytotalcost = mytotalcost - (item.DomainObject.EuroSum);
    //    }
    //    protected override void PropertiesChangedNotifycation()
    //    {
    //        this.PropertyChangedNotification("ItemCount");
    //        this.PropertyChangedNotification("TotalCost");
    //    }
    //}
}
