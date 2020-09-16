using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class PrepayCustomerRequest : lib.DomainStampValueChanged
    {
        public PrepayCustomerRequest(int id, long stamp, DateTime? updated, string updater, lib.DomainObjectState mstate
            , RequestCustomerLegal customer, decimal? dtsum, decimal eurosum, DateTime? expirydate, decimal initsum, string note, Prepay prepay, Request request, decimal? selling, DateTime? sellingdate, decimal sellingrate
            ) : base(id, stamp, updated, updater, mstate)
        {
            mycustomer = customer;
            //mycustomsinvoice = customsinvoice;, CustomsInvoice customsinvoice,CustomsInvoice invoice
            if (mycustomer != null) mycustomer.CustomsInvoice.PropertyChanged += this.Customsinvoice_PropertyChanged;
            mydtsum = dtsum;
            myeurosum = eurosum;
            myexpirydate = expirydate;
            myinitsum = initsum;
            mynote = note;
            myprepay = prepay;
            myrequest = request;
            myselling = selling;
            mysellingdate = sellingdate;
            mysellingrate = sellingrate;
            SetPrepayAction();

            myrater = new CurrencyRateProxy(CustomBrokerWpf.References.CurrencyRate);
            myrater.PropertyChanged += Rater_PropertyChanged;
            if (mysellingdate.HasValue && mysellingrate == 1M && !myprepay.CBRate.HasValue)
                myrater.RateDate = mysellingdate.Value;
        }
        internal PrepayCustomerRequest(RequestCustomerLegal customer, Prepay prepay, Request request) : this(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, customer, null, 0M, null, 0M, null, prepay, request, null, null, 1M) { }

        private decimal? mycustomerbalance;
        public decimal? CustomerBalance
        {
            get
            {
                if (this.IsLoaded)
                {
                    decimal? seling = this.Selling;
                    return seling.HasValue && this.CustomsInvoice != null ? seling.Value
                            - (this.Prepay.EuroSum > 0M ? decimal.Multiply(decimal.Divide(this.EuroSum, this.Prepay.EuroSum), this.Prepay.RubPaySum) : 0M)
                            - (this.CustomsInvoice.CustmRubSum > 0M ? decimal.Multiply(decimal.Divide(this.CustomsInvoice.PaySum, this.CustomsInvoice.CustmRubSum.Value), (this.CustomsInvoiceRubSum ?? 0M)) : 0M)
                            - (this.FinalInvoiceRubSumPaid ?? 0M) - (this.FinalInvoiceCurSumPaid ?? 0M) - (this.FinalInvoiceCur2SumPaid ?? 0M)
                        : (decimal?)null;
                }
                else
                    return null;
            }
        }
        public CustomsInvoice CustomsInvoice
        {
            get
            {
                return mycustomer?.CustomsInvoice;
            }
        }
        private decimal? mycustomsinvoicerubsum;
        private decimal? mycustomsinvoicerubsumold;
        public decimal? CustomsInvoiceRubSum
        {
            internal set { this.OnValueChanged(nameof(this.CustomsInvoiceRubSum), myfinalinvoicecursum, value); mycustomsinvoicerubsum = value; this.PropertyChangedNotification(nameof(this.CustomsInvoiceRubSum)); }
            get
            {
                if (!mycustomsinvoicerubsum.HasValue && this.DTSum.HasValue && this.RequestCustomer?.InvoiceDiscount != null && this.CustomsInvoice != null)
                {
                    this.CustomsInvoice.PrepayDistribute(nameof(this.CustomsInvoiceRubSum), 0);
                }
                return mycustomsinvoicerubsum;
            }
        }
        public DateTime? CurrencyBoughtDate
        {
            set
            {
                if (value.HasValue)
                {
                    if (this.Prepay.CBRate.HasValue)
                    {
                        this.Prepay.CurrencyBuyRate = mycurrencybuyrate;
                        this.Prepay.CurrencyBoughtDate = value;
                    }
                    else if (this.CustomsInvoice != null)
                    {
                        this.CustomsInvoice.CurrencyBuyRate = mycurrencybuyrate;
                        this.CustomsInvoice.CurrencyBoughtDate = value;
                    }
                }
                else
                {
                    if (this.Prepay.CurrencyPays?.Count > 0)
                        System.Windows.MessageBox.Show("Невозможно удалить покупку валюты, оплачено поставщику!", "Дата покупки валюты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                    else if (System.Windows.MessageBox.Show("Удалить все покупки валюты ?", "Дата покупки валюты", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                    {
                        this.Prepay.CurrencyBoughtDate = null;
                        if (this.CustomsInvoice != null) this.CustomsInvoice.CurrencyBoughtDate = null;
                        this.PropertyChangedNotification(nameof(this.CurrencyBuySumTotal));
                    }
                }
                this.PropertyChangedNotification(nameof(this.CurrencyBoughtDate));
            }
            get
            {
                return (this.Prepay.CurrencyBoughtDate ?? DateTime.MinValue) > (this.CustomsInvoice?.CurrencyBoughtDate ?? DateTime.MinValue) ? this.Prepay.CurrencyBoughtDate : this.CustomsInvoice?.CurrencyBoughtDate;
            }
        }
        private decimal? mycurrencybuyrate;
        public decimal? CurrencyBuyRate
        {
            set { if (!this.CurrencyBoughtDate.HasValue) mycurrencybuyrate = value; this.PropertyChangedNotification(nameof(this.CurrencyBuyRate)); }
            get { return mycurrencybuyrate.HasValue ? mycurrencybuyrate : (this.CurrencyBuySumTotal > 0 ? decimal.Multiply(decimal.Divide(this.Prepay.CurrencyBuySum, this.CurrencyBuySumTotal), this.Prepay.CurrencyBuyRate ?? 0M) + decimal.Multiply(decimal.Divide((this.CustomsInvoice?.CurrencyBuySum ?? 0M), this.CurrencyBuySumTotal), this.CustomsInvoice?.CurrencyBuyRate ?? 0M) : (decimal?)null); }
        }
        private decimal CurrencyBuySumTotal
        { get { return this.Prepay.CurrencyBuySum + this.CustomsInvoice?.CurrencyBuySum ?? 0M; } }
        public decimal CurrencyBuySum
        { get { return decimal.Multiply(this.Prepay.CurrencyBuySum, decimal.Divide(this.EuroSum, this.Prepay.EuroSum)) + (this.CustomsInvoice != null ? decimal.Multiply(this.CustomsInvoice.CurrencyBuySum, decimal.Divide((this.DTSum ?? 0M), (this.CustomsInvoice.CustmCurSum ?? 0M))) : 0M); } }
        private decimal? mycurrencypaysum;
        public DateTime? CurrencyPaidDate
        {
            set
            {
                if (value.HasValue)
                {
                    DateTime maxdate = this.Prepay.CurrencyPays.Count((PrepayCurrencyPay item) => { return item.DomainState < lib.DomainObjectState.Deleted; }) > 0 ? this.Prepay.CurrencyPays.Max((PrepayCurrencyPay item) => { return item.DomainState < lib.DomainObjectState.Deleted ? item.PayDate : DateTime.MinValue; }).Date : DateTime.MinValue.Date;
                    if (maxdate > value.Value)
                        System.Windows.MessageBox.Show("Дата оплаты поставщику не может быть меньше даты платежа!", "Дата оплаты поставщику", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                    else
                    {
                        if (maxdate < value.Value)
                        {
                            AgentCustomerBalanceDBM bdbm = new AgentCustomerBalanceDBM() { Agent = this.Prepay.Agent, Customer = this.Prepay.Customer, MinBalance = 0M, Importer = this.Prepay.Importer };
                            decimal balance = bdbm.GetFirst()?.Balance ?? 0M;
                            if (myprepay.EuroSum + (this.CustomsInvoice?.FinalCurSum ?? 0M) - this.Prepay.CurrencyPaySum - balance > 0.0099M)
                            {
                                PrepayCurrencyPay pay = new PrepayCurrencyPay(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null, value.Value, myprepay.EuroSum + (this.CustomsInvoice?.FinalCurSum ?? 0M) - this.Prepay.CurrencyPaySum - balance, this.Prepay);
                                this.Prepay.CurrencyPays.Add(pay);
                                this.PropertyChangedNotification(nameof(this.CurrencyPaySum));
                            }
                        }
                    }
                }
                this.Prepay.CurrencyPaidDate = value;
            }
            get
            {
                return this.Prepay.CurrencyPaidDate;
            }
        }
        public decimal CurrencyPaySum
        { get { return myprepay != null && myprepay.EuroSum + (this.CustomsInvoice?.FinalCurSum ?? 0M) > 0M ? decimal.Multiply(decimal.Divide(myeurosum + (this.FinalInvoiceCurSum ?? 0M), myprepay.EuroSum + (this.CustomsInvoice?.FinalCurSum ?? 0M)), myprepay.CurrencyPaySum) : 0M; } }
        private RequestCustomerLegal mycustomer;
        public RequestCustomerLegal RequestCustomer
        { set { SetProperty<RequestCustomerLegal>(ref mycustomer, value); } get { return mycustomer; } }
        private decimal? mydtsum;
        public decimal? DTSum
        {
            set
            {
                SetPropertyOnValueChanged<decimal?>(ref mydtsum, value, () =>
                {
                    //if (!this.UpdatingSample)
                    //{
                    //    decimal totdtsum = (myrequest ?? mycustomer.Request).CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal selected) => { return selected.Prepays.Sum((PrepayCustomerRequest rprepay) => { return rprepay.DTSum ?? 0M; }); });
                    //    if ((myrequest ?? mycustomer.Request).InvoiceDiscount != totdtsum)
                    //        (myrequest ?? mycustomer.Request).InvoiceDiscount = totdtsum;
                    //}
                    //mycustomer?.PropertyChangedNotification(nameof(RequestCustomerLegal.DTSum));
                    DTSumOnValueChanged();
                });
            }
            get
            {
                if (!mydtsum.HasValue)
                    this.RequestCustomer.PrepayDistribute(nameof(this.DTSum), 2);
                return mydtsum;
            }
        }
        public decimal? DTSumSet
        {
            set
            {
                if (mydtsum != value)
                {
                    decimal? oldvalue = mydtsum;
                    mydtsum = value;
                    this.PropertyChangedNotification(nameof(this.DTSum));
                    this.OnValueChanged(nameof(this.DTSum), oldvalue, value);
                    DTSumOnValueChanged();
                }
            }
            get { return mydtsum; }
        }
        private decimal myeurosum;
        public decimal EuroSum
        {
            set
            {
                decimal d = value - myeurosum;
                if (d == 0M) return;
                if (!this.Prepay.InvoiceDate.HasValue || (this.Prepay.FundSum >= d && this.Prepay.IsPrepay.HasValue && this.Prepay.IsPrepay.Value && (myrequest ?? mycustomer.Request).Status.Id != 0))
                {
                    if (this.Prepay.IsPrepay.HasValue && this.Prepay.IsPrepay.Value && (myrequest ?? mycustomer.Request).Status.Id != 0)
                    {
                        if (this.Prepay.FundSum >= d)
                        {
                            this.Prepay.FundSum = this.Prepay.FundSum.Value - d; // коррр остаток
                            d = 0M;
                        }
                        else
                        {
                            d = d - this.Prepay.FundSum.Value;
                            this.Prepay.FundSum = 0M;
                        }
                        if (value > 0M && this.RequestCustomer.PrePrepays.Contains(this))
                        {
                            this.RequestCustomer.PrePrepays.Remove(this);
                            this.RequestCustomer.Prepays.Add(this);
                        }
                        else if (value == 0M && this.RequestCustomer.Prepays.Contains(this))
                        {
                            this.RequestCustomer.Prepays.Remove(this);
                            this.RequestCustomer.PrePrepays.Add(this);
                        }
                    }
                    if (d != 0M)
                        this.Prepay.EuroSum = this.Prepay.EuroSum + d; // счет не выставлен коррек prepay

                    decimal oldvalue = myeurosum;
                    SetPropertyOnValueChanged<decimal>(ref myeurosum, value, () =>
                    {
                        //if ((myrequest ?? mycustomer.Request).CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal selected) => { return selected.Prepays.Count((PrepayCustomerRequest rprepay)=> { return rprepay.Prepay.InvoiceDate.HasValue; }); }) == 0
                        //    && ((myrequest ?? mycustomer.Request).InvoiceDiscount != myeurosum || (myrequest ?? mycustomer.Request).CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal selected) => { return selected.Prepays.Count; }) > 1))
                        //    (myrequest ?? mycustomer.Request).InvoiceDiscount = ((myrequest ?? mycustomer.Request).InvoiceDiscount ?? 0M) + myeurosum - oldvalue;
                        //myrequest?.PropertyChangedNotification(nameof(Request.InvoiceDiscountFill));
                        EuroSumChangedNotification();
                    });
                }
                else
                    System.Windows.MessageBox.Show("Нельзя изменять сумму предоплаты после выставления счета!", "Предоплата", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
            }
            get { return myeurosum; }
        }
        private DateTime? myexpirydate;
        public DateTime? ExpiryDate
        {
            set { SetProperty<DateTime?>(ref myexpirydate, value); }
            get { return myexpirydate.HasValue ? myexpirydate : myprepay.ExpiryDate; }
        }
        private decimal? myfinalinvoicerubsum;
        public decimal? FinalInvoiceRubSum
        {
            get { return this.Selling.HasValue && this.RubSum.HasValue && this.CustomsInvoiceRubSum.HasValue ? this.Selling.Value - this.CustomsInvoiceRubSum.Value - (this.RubSum ?? 0M) : (decimal?)null; }
        }
        public decimal? FinalInvoiceRubSumPaid
        {
            get { return ((this.CustomsInvoice?.FinalRubSum ?? 0M) > 0M ? decimal.Multiply(decimal.Divide(this.CustomsInvoice.FinalRubPaySum, (this.CustomsInvoice.FinalRubSum ?? 1M)), (this.FinalInvoiceRubSum ?? 0M)) : (decimal?)null); }
        }
        private decimal? myfinalinvoicecursum;
        public decimal? FinalInvoiceCurSum
        {
            set
            {
                if (this.CustomsInvoice != null) // удалось создать
                {
                    this.CustomsInvoice.FinalCurSum = value ?? 0M;
                }
            }
            get
            {
                //return this.DTSum.HasValue ? decimal.Multiply(decimal.Divide(this.DTSum.Value, this.CustomsInvoice.CustmCurSum.Value), this.CustomsInvoice.FinalCurSum) : (decimal?)null;
                if (!this.DTSum.HasValue && this.Prepay.CBRate.HasValue && this.CustomsInvoice == null)
                    return null;
                else if (!myfinalinvoicecursum.HasValue && this.CustomsInvoice.FinalCurSum != 0M)
                {
                    this.CustomsInvoice.PrepayDistribute(nameof(CustomsInvoice.FinalCurSum), 2);
                }
                return myfinalinvoicecursum;
            }
        }
        internal decimal? FinalInvoiceCurSumSet
        {
            set
            {
                if (myfinalinvoicecursum != value)
                {
                    decimal? oldvalue = myfinalinvoicecursum;
                    myfinalinvoicecursum = value;
                    this.PropertyChangedNotification(nameof(this.FinalInvoiceCurSum));
                    this.OnValueChanged(nameof(this.FinalInvoiceCurSum), oldvalue, value); // else cycle to PrepayDistribute
                }
            }
            get { return myfinalinvoicecursum; }
        }
        public decimal? FinalInvoiceCurSumPaid
        {
            get { return (!this.Prepay.CBRate.HasValue && (this.CustomsInvoice?.CustmCurSum ?? 0M) > 0M && this.DTSum.HasValue ? decimal.Multiply(decimal.Divide(this.DTSum.Value, this.CustomsInvoice.CustmCurSum.Value), this.CustomsInvoice.FinalCurPaySum) : (decimal?)null); }
        }
        private decimal? myfinalinvoicecursum2;
        public decimal? FinalInvoiceCurSum2
        {
            set
            {
                if (this.CustomsInvoice != null) // удалось создать
                    this.CustomsInvoice.FinalCurSum2 = value ?? 0M;
            }
            get
            {
                //return !this.Prepay.CBRate.HasValue && this.CustomsInvoice != null && (this.CustomsInvoice.CustmCurSum??0M) > 0M && this.DTSum.HasValue ? decimal.Multiply(decimal.Divide(this.DTSum.Value, this.CustomsInvoice.CustmCurSum.Value), this.CustomsInvoice.FinalCurSum2) : (decimal?)null; 
                if (!this.DTSum.HasValue && this.Prepay.CBRate.HasValue && this.CustomsInvoice == null)
                    return null;
                else if (!myfinalinvoicecursum2.HasValue && this.CustomsInvoice.FinalCurSum2 != 0M)
                {
                    this.CustomsInvoice.PrepayDistribute(nameof(CustomsInvoice.FinalCurSum2), 2);
                }
                return myfinalinvoicecursum2;
            }
        }
        internal decimal? FinalInvoiceCurSum2Set
        {
            set
            {
                if(myfinalinvoicecursum2 != value)
                { 
                    decimal? oldvalue = myfinalinvoicecursum2;
                    myfinalinvoicecursum2 = value;
                    this.PropertyChangedNotification(nameof(this.FinalInvoiceCurSum2));
                    this.OnValueChanged(nameof(this.FinalInvoiceCurSum2), oldvalue, value); // else cycle to PrepayDistribute
                }
            }
            get { return myfinalinvoicecursum2; }
        }
        public decimal? FinalInvoiceCur2SumPaid
        {
            get { return (!this.Prepay.CBRate.HasValue && (this.CustomsInvoice?.CustmCurSum ?? 0M) > 0M && this.DTSum.HasValue ? decimal.Multiply(decimal.Divide(this.DTSum.Value, this.CustomsInvoice.CustmCurSum.Value), this.CustomsInvoice.FinalCurPaySum2) : (decimal?)null); }
        }
        private decimal myinitsum;
        public decimal InitSum
        { set { SetProperty<decimal>(ref myinitsum, value); } get { return myinitsum; } }
        public bool IsOverPay
        { get { return OverPay.HasValue && this.OverPay.Value > 0; } }
        public bool IsPrepay
        { get { return this.Request?.Status.Id == 0; } }
        private string mynote;
        public string Note
        { set { SetProperty<string>(ref mynote, value); } get { return mynote; } }
        private decimal? myoverpay;
        public decimal? OverPay
        { get { return myeurosum - this.DTSum; } }
        private Prepay myprepay;
        public Prepay Prepay
        {
            set
            {
                SetProperty<Prepay>(ref myprepay, value, SetPrepayAction);
            }
            get { return myprepay; }
        }
        private decimal? myrefund;
        public decimal Refund
        {
            set { this.Prepay.Refund = value; this.PropertyChangedNotification(nameof(this.Refund)); }
            get { return myprepay != null ? (myprepay.EuroSum > 0M ? decimal.Divide(myeurosum, myprepay.EuroSum) : 1M) * myprepay.Refund : 0M; }
        }
        private Request myrequest;
        public Request Request
        {
            //set { SetProperty<Request>(ref myrequest, value, () => 
            //{
            //    this.PropertyChangedNotification(nameof(this.DTSum));
            //    this.PropertyChangedNotification(nameof(this.IsPrepay));
            //    this.PropertyChangedNotification(nameof(this.Selling));
            //    Specification_Set();
            //    SellingOnValueChanged();
            //    myrequest.PropertyChanged += this.Request_PropertyChanged; 
            //}); }
            get { return myrequest; }
        }
        private decimal? myrubsum;
        public decimal? RubSum
        {
            get { return decimal.Round(myeurosum * (1M + this.Prepay?.Percent ?? 0M) * this.Prepay?.CBRate ?? 0M); }
        }
        private decimal? myrubdiff;
        public decimal? RubDiff
        { get { return this.RubSum.HasValue && (this.Prepay?.CurrencyBuyRate.HasValue ?? false) ? decimal.Multiply(myeurosum, this.Prepay.CurrencyBuyRate.Value) - this.RubSum.Value : (decimal?)null; } }
        private decimal? myselling;
        private decimal? mysellingold; // для OnValueChanged
        public decimal? Selling
        {
            internal set { myselling = value; SellingOnValueChanged(); }
            // если нет предоплаты считаем в евро
            get { return myselling; }//(this.DTSum.HasValue && this.Request?.AlgorithmCMD != null && (this.Prepay?.CBRatep2p==null || (this.Request?.Specification?.Declaration?.CBRate) != null)) ? this.DTSum * ((this.Prepay?.CBRatep2p??1M) + (this.Prepay?.CBRatep2p!=null ? this.Request?.Specification?.Declaration?.CBRate : 1M) * this.Request?.TotalPayInvoicePer):null; }
        }
        private DateTime? mysellingdate;
        public DateTime? SellingDate
        {
            set
            {
                SetProperty<DateTime?>(ref mysellingdate, value, () =>
                {
                    if (!(myprepay.CBRate.HasValue | this.UpdatingSample))
                    {
                        this.SellingRate = 1M;
                        if (mysellingdate.HasValue)
                        {
                            myrater.RateDate = mysellingdate.Value;
                        }
                    }
                });
            }
            get { return mysellingdate; }
        }
        private decimal mysellingrate;
        public decimal SellingRate
        {
            set { SetProperty<decimal>(ref mysellingrate, value); }
            get { return mysellingrate; }
        }
        public DateTime? Updated
        {
            get
            {
                return this.UpdateWhen > myprepay?.UpdateWhen ? base.UpdateWhen : myprepay?.UpdateWhen;
            }
        }
        public string Updater
        { get { return base.UpdateWhen > myprepay?.UpdateWhen ? base.UpdateWho : myprepay?.UpdateWho; } }

        public override bool IsDirty
        {
            get
            {
                bool dirty = base.IsDirty;
                // problem with cross reference
                if (!dirty)
                {
                    if (this.DirtyThread.Contains(Thread.CurrentThread.ManagedThreadId))
                        return false;
                    else
                        this.DirtyThread.Add(Thread.CurrentThread.ManagedThreadId);
                }
                if (!dirty && mycustomer != null)
                    dirty = mycustomer.IsDirty;
                if (!dirty && myprepay != null)
                    dirty = myprepay.IsDirty;
                if (!dirty && myrequest != null)
                    dirty = myrequest.IsDirty;
                this.DirtyThread.Remove(Thread.CurrentThread.ManagedThreadId);
                return dirty;
            }
        }

        private void Prepay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Prepay.CBRatep2p):
                    SellingOnValueChanged();
                    if (mysellingrate != 1M && Prepay.CBRatep2p.HasValue)
                        mysellingrate = 1M;
                    else if (mysellingdate.HasValue && mysellingrate == 1M && !Prepay.CBRatep2p.HasValue)
                        myrater.RateDate = mysellingdate.Value;
                    break;
                case nameof(Prepay.CurrencyBuyRate):
                    this.RubDiffOnValueChanged();
                    break;
                case nameof(Prepay.CurrencyPaySum):
                    this.PropertyChangedNotification(nameof(this.CurrencyPaySum));
                    CurrencyPaySumOnValueChanged();
                    break;
                case nameof(Prepay.EuroSum):
                    this.PropertyChangedNotification(nameof(this.CurrencyPaySum));
                    RefundOnValueChanged();
                    break;
                case nameof(Prepay.ExpiryDate):
                    this.PropertyChangedNotification(nameof(this.ExpiryDate));
                    break;
                case nameof(Prepay.Refund):
                    RefundOnValueChanged();
                    break;
                case nameof(Prepay.RubPaySum):
                    this.PropertyChangedNotification(nameof(this.CustomerBalance));
                    this.CustomerBalanceOnValueChanged();
                    break;
                case nameof(Prepay.RubSum):
                    this.PropertyChangedNotification(nameof(this.RubSum));
                    RubSumOnValueChanged();
                    this.PropertyChangedNotification(nameof(this.CustomerBalance));
                    this.CustomerBalanceOnValueChanged();
                    break;
            }
        }
        private void Customsinvoice_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CustomsInvoice.CustmCurSum):
                    FinalInvoiceCurSumOnValueChanged();
                    this.PropertyChangedNotification(nameof(this.CustomerBalance));
                    this.CustomerBalanceOnValueChanged();
                    break;
                case nameof(CustomsInvoice.CustmRubSum):
                    mycustomsinvoicerubsum = null;
                    this.PropertyChangedNotification(nameof(this.CustomsInvoiceRubSum));
                    CustomsInvoiceRubSumOnValueChanged();
                    FinalInvoiceCurSumOnValueChanged();
                    this.PropertyChangedNotification(nameof(this.CustomerBalance));
                    this.CustomerBalanceOnValueChanged();
                    break;
                case nameof(CustomsInvoice.FinalCurSum):
                    this.PropertyChangedNotification(nameof(this.FinalInvoiceCurSum));
                    FinalInvoiceCurSumOnValueChanged();
                    break;
                case nameof(CustomsInvoice.FinalCurSum2):
                    this.PropertyChangedNotification(nameof(this.FinalInvoiceCurSum2));
                    FinalInvoiceCur2SumOnValueChanged();
                    break;
                case nameof(CustomsInvoice.FinalRubPaySum):
                case nameof(CustomsInvoice.FinalRubSum):
                case nameof(CustomsInvoice.PaySum):
                    this.PropertyChangedNotification(nameof(this.FinalInvoiceRubSum));
                    this.PropertyChangedNotification(nameof(this.FinalInvoiceRubSumPaid));
                    this.PropertyChangedNotification(nameof(this.CustomerBalance));
                    this.CustomerBalanceOnValueChanged();
                    break;
            }
        }
        private void Request_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Request.TotalPayInvoicePer):
                    this.PropertyChangedNotification(nameof(this.Selling));
                    SellingOnValueChanged();
                    break;
                case nameof(Request.Specification):
                    Specification_Set();
                    break;
            }
        }
        private void Specification_Set()
        {
            if (!mydtsum.HasValue)
            {
                this.OnValueChanged(nameof(this.DTSum), 0M, this.DTSum);
                this.FinalInvoiceCurSumOnValueChanged();
                this.CustomerBalanceOnValueChanged();
                this.OverPayOnValueChanged();
            }
            if (myrequest.Specification != null)
            {
                myrequest.Specification.PropertyChanged += Specification_PropertyChanged;
            }
        }
        private void Specification_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Specification.Specification.Declaration):
                    this.PropertyChangedNotification(nameof(this.Selling));
                    SellingOnValueChanged();
                    if (myrequest.Specification.Declaration != null)
                    {
                        myrequest.Specification.Declaration.PropertyChanged += Declaration_PropertyChanged;
                    }
                    break;
            }
        }
        private void Declaration_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Specification.Declaration.CBRate):
                    SellingOnValueChanged();
                    break;
            }
        }
        private void SetPrepayAction()
        {
            myprepay.PropertyChanged += this.Prepay_PropertyChanged;
            this.PropertyChangedNotification(nameof(this.CurrencyPaySum));
            CurrencyPaySumOnValueChanged();
            this.PropertyChangedNotification(nameof(this.DTSum));
            this.PropertyChangedNotification(nameof(this.RubSum));
            RubSumOnValueChanged();
            this.PropertyChangedNotification(nameof(this.Selling));
            this.PropertyChangedNotification(nameof(this.CustomerBalance));
            this.PropertyChangedNotification(nameof(this.Updated));
            this.PropertyChangedNotification(nameof(this.Updater));
        }
        internal void UnSubscribe()
        {
            myprepay.PropertyChanged -= this.Prepay_PropertyChanged;
            if (this.CustomsInvoice != null) this.CustomsInvoice.PropertyChanged -= this.Customsinvoice_PropertyChanged;
            myrequest.PropertyChanged -= this.Request_PropertyChanged;
            if (myrequest.Specification != null)
            {
                myrequest.Specification.PropertyChanged -= Specification_PropertyChanged;
                if (myrequest.Specification.Declaration != null) myrequest.Specification.Declaration.PropertyChanged -= Declaration_PropertyChanged;
            }
            myrater.PropertyChanged -= Rater_PropertyChanged;
        }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            PrepayCustomerRequest templ = sample as PrepayCustomerRequest;
            this.DTSumSet = templ.DTSumSet;
            EuroSumUpd(templ.EuroSum);
            this.ExpiryDate = templ.ExpiryDate;
            this.InitSum = templ.InitSum;
            //this.CustomsInvoice = templ.CustomsInvoice;
            this.Note = templ.Note;
            if (templ.Selling.HasValue) this.Selling = templ.Selling;
            this.SellingDate = templ.SellingDate;
            this.SellingRate = templ.SellingRate;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.EuroSum):
                    this.EuroSum = (decimal)value;
                    break;
                case nameof(this.InitSum):
                    this.InitSum = (decimal)value;
                    break;
                case nameof(this.ExpiryDate):
                    this.ExpiryDate = (DateTime?)value;
                    break;
                case nameof(this.Note):
                    this.Note = (string)value;
                    break;
                case nameof(this.SellingDate):
                    this.SellingDate = (DateTime?)value;
                    break;
                case nameof(this.SellingRate):
                    this.SellingRate = (decimal)value;
                    break;
            }
        }
        internal bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                case nameof(this.DTSum):
                    if ((decimal)value < 0M)
                    {
                        errmsg = "Сумма инвойса по ДТ не может быть меньше ноля!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.EuroSum):
                    if (this.Prepay.InvoiceDate.HasValue && ((mycustomer.Request ?? myrequest).Status.Id == 0 || !(this.Prepay.IsPrepay ?? false)))
                    {
                        errmsg = "Выставлен счет. Сумма предоплаты не может быть изменена после выставления счета!";
                        isvalid = false;
                    }
                    else if (this.Prepay.InvoiceDate.HasValue && (decimal)value > this.Prepay.EuroSum)
                    {
                        errmsg = "Недостаточно средств. Сумма инвойса не может быть больше суммы выставленного счета!";
                        isvalid = false;
                    }
                    else if ((decimal)value < 0M)
                    {
                        errmsg = "Сумма инвойса не может быть меньше ноля!";
                        isvalid = false;
                    }
                    break;
            }
            return isvalid;
        }

        private void DTSumOnValueChanged()
        {
            CustomsInvoiceRubSumOnValueChanged();
            SellingOnValueChanged();
            FinalInvoiceRubSumOnValueChanged();
            FinalInvoiceCurSumOnValueChanged();
            OverPayOnValueChanged();
        }

        private void CustomerBalanceOnValueChanged()
        {
            decimal runsum = this.CustomerBalance ?? 0M;
            if ((mycustomerbalance ?? 0M) != runsum)
            {
                this.PropertyChangedNotification(nameof(PrepayCustomerRequest.CustomerBalance));
                this.OnValueChanged(nameof(this.CustomerBalance), mycustomerbalance ?? 0M, runsum);
                mycustomerbalance = runsum;
            }
        }
        private void CurrencyPaySumOnValueChanged()
        {
            decimal runsum = this.CurrencyPaySum;
            if ((mycurrencypaysum ?? 0M) != runsum) this.OnValueChanged(nameof(this.CurrencyPaySum), mycurrencypaysum ?? 0M, runsum);
            mycurrencypaysum = runsum;
        }
        private void CustomsInvoiceRubSumOnValueChanged()
        {
            decimal runsum = this.CustomsInvoiceRubSum ?? 0M;
            if ((mycustomsinvoicerubsumold ?? 0M) != runsum)
            {
                this.OnValueChanged(nameof(this.CustomsInvoiceRubSum), mycustomsinvoicerubsumold ?? 0M, runsum);
                mycustomsinvoicerubsumold = runsum;
                FinalInvoiceRubSumOnValueChanged();
            }
        }
        internal void FinalInvoiceRubSumOnValueChanged()
        {
            decimal runsum = this.FinalInvoiceRubSum ?? 0M;
            if ((myfinalinvoicerubsum ?? 0M) != runsum)
            {
                this.PropertyChangedNotification(nameof(PrepayCustomerRequest.FinalInvoiceRubSum));
                this.OnValueChanged(nameof(this.FinalInvoiceRubSum), myfinalinvoicerubsum ?? 0M, runsum);
                myfinalinvoicerubsum = runsum;
            }
        }
        internal void FinalInvoiceCurSumOnValueChanged()
        {
            decimal runsum = this.FinalInvoiceCurSum ?? 0M;
            if ((myfinalinvoicecursum ?? 0M) != runsum) this.OnValueChanged(nameof(this.FinalInvoiceCurSum), myfinalinvoicecursum ?? 0M, runsum);
            myfinalinvoicecursum = runsum;
        }
        internal void FinalInvoiceCur2SumOnValueChanged()
        {
            decimal runsum = this.FinalInvoiceCurSum2 ?? 0M;
            if ((myfinalinvoicecursum2 ?? 0M) != runsum) this.OnValueChanged(nameof(this.FinalInvoiceCurSum2), myfinalinvoicecursum2 ?? 0M, runsum);
            myfinalinvoicecursum2 = runsum;
        }
        private void OverPayOnValueChanged()
        {
            this.GetPropertyChangedNotification(ref myoverpay, this.OverPay, nameof(this.OverPay));
        }
        private void RefundOnValueChanged()
        {
            this.GetPropertyChangedNotification(ref myrefund, this.Refund, nameof(this.Refund));
        }
        private void RubDiffOnValueChanged()
        {
            this.GetPropertyChangedNotification(ref myrubdiff, this.RubDiff, nameof(this.RubDiff));
        }
        private void RubSumOnValueChanged()
        {
            decimal runsum = this.RubSum ?? 0M;
            if ((myrubsum ?? 0M) != runsum)
            {
                this.OnValueChanged(nameof(this.RubSum), myrubsum ?? 0M, runsum);
                myrubsum = runsum;
                this.FinalInvoiceRubSumOnValueChanged();
                this.RubDiffOnValueChanged();
            }
        }
        internal void SellingOnValueChanged()
        {
            decimal runsum = this.Selling ?? 0M;
            if ((mysellingold ?? 0M) != runsum)
            {
                this.PropertyChangedNotification(nameof(PrepayCustomerRequest.Selling));
                this.OnValueChanged(nameof(this.Selling), mysellingold ?? 0M, runsum);
                mysellingold = runsum;
                FinalInvoiceRubSumOnValueChanged();
                CustomerBalanceOnValueChanged();
            }
        }
        private void GetPropertyChangedNotification(ref decimal? oldvalue, decimal? newvalue, string name)
        {
            if ((oldvalue ?? 0M) != (newvalue ?? 0M))
            {
                this.PropertyChangedNotification(name);
                this.OnValueChanged(name, oldvalue ?? 0M, newvalue ?? 0M);
                oldvalue = newvalue;
            }
        }
        internal void UpdateEuroSum(decimal value, bool self)
        {
            decimal oldvalue = myeurosum;
            this.EuroSum = value;
            if (self && mycustomer != null & oldvalue != myeurosum & oldvalue == (this.DTSum ?? oldvalue))
                mycustomer.UpdateInvoiceDiscount((mycustomer.InvoiceDiscount ?? 0M) + myeurosum - oldvalue, 'p');
        }
        private void EuroSumChangedNotification()
        {
            mycustomer?.PropertyChangedNotification(nameof(mycustomer.PrepaySum));
            RubSumOnValueChanged();
            this.PropertyChangedNotification(nameof(this.CurrencyPaySum));
            CurrencyPaySumOnValueChanged();
            mycustomer?.PropertyChangedNotification(nameof(RequestCustomerLegal.InvoiceDiscount));
            this.PropertyChangedNotification(nameof(this.CustomerBalance));
            this.CustomerBalanceOnValueChanged();
            this.OverPayOnValueChanged();
            this.RefundOnValueChanged();
            this.RubDiffOnValueChanged();
        }
        private void EuroSumUpd(decimal eurosum)
        {
            myeurosum = eurosum;
            this.PropertyChangedNotification(nameof(this.EuroSum));
            EuroSumChangedNotification();
        }

        private Classes.CurrencyRateProxy myrater;
        private void Rater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EURRate" & mysellingdate.HasValue & myrater.EURRate.HasValue)
            {
                this.SellingRate = myrater.EURRate.Value;
            }
        }
    }

    public class PrepayCustomerRequestDBM : lib.DBManagerStamp<PrepayCustomerRequest>
    {
        public PrepayCustomerRequestDBM()
        {
            NeedAddConnection = true;
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.RequestCustomerPrepay_sp";
            InsertCommandText = "dbo.RequestCustomerPrepayAdd_sp";
            UpdateCommandText = "dbo.RequestCustomerPrepayUpd_sp";
            DeleteCommandText = "dbo.RequestCustomerPrepayDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int), new SqlParameter("@customerid", System.Data.SqlDbType.Int), new SqlParameter("@filterid", System.Data.SqlDbType.Int), new SqlParameter("@importerid", System.Data.SqlDbType.Int), new SqlParameter("@parcelid", System.Data.SqlDbType.Int), new SqlParameter("@prepayid", System.Data.SqlDbType.Int), new SqlParameter("@requestid", System.Data.SqlDbType.Int) };
            InsertParams = new SqlParameter[] {
                myinsertparams[0],
                new SqlParameter("@customerid", System.Data.SqlDbType.Int),
                new SqlParameter("@prepayid", System.Data.SqlDbType.Int) { Direction=System.Data.ParameterDirection.InputOutput},
                new SqlParameter("@requestid", System.Data.SqlDbType.Int),
                new SqlParameter("@index", System.Data.SqlDbType.Int) };
            UpdateParams = new SqlParameter[] {
                myupdateparams[0],
                new SqlParameter("@initsumupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@eurosumupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@expirydateupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@dtsumupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@noteupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@sellingdateupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@sellingrateupd", System.Data.SqlDbType.Bit) };
            InsertUpdateParams = new SqlParameter[] {
                myinsertupdateparams[0],
                new SqlParameter("@initsum", System.Data.SqlDbType.Money),
                new SqlParameter("@eurosum", System.Data.SqlDbType.Money),
                new SqlParameter("@expirydate", System.Data.SqlDbType.Date),
                new SqlParameter("@dtsum", System.Data.SqlDbType.Money),
                new SqlParameter("@note", System.Data.SqlDbType.NVarChar,300),
                new SqlParameter("@sellingdate", System.Data.SqlDbType.DateTime2),
                new SqlParameter("@sellingrate", System.Data.SqlDbType.SmallMoney) };

            mypdbm = new PrepayDBM();
        }

        private CustomerLegal mycustomer;
        internal CustomerLegal Customer
        { set { mycustomer = value; } get { return mycustomer; } }
        private Parcel myparcel;
        internal Parcel Parcel
        { set { myparcel = value; } get { return myparcel; } }
        private Importer myimporter;
        internal Importer Importer
        { set { myimporter = value; } get { return myimporter; } }
        private RequestCustomerLegal myrequestcustomer;
        internal RequestCustomerLegal RequestCustomer
        { set { myrequestcustomer = value; } get { return myrequestcustomer; } }
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { set { myfilter = value; } get { return myfilter; } }
        private PrepayDBM mypdbm;
        private RequestDBM myrdbm;
        public RequestDBM RequestDBM
        { set { myrdbm = value; } get { return myrdbm; } }
        private CustomsInvoiceDBM mycidbm;

        protected override PrepayCustomerRequest CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            PrepayCustomerRequest item, itemold = null;
            if (this.FillType == lib.FillType.PrefExist)
                itemold = CustomBrokerWpf.References.PrepayRequestStore.GetItem(reader.GetInt32(0));
            if (itemold == null)
            {
                Prepay prepay;
                List<lib.DBMError> errors;
                if (mypdbm.FillType == lib.FillType.Refresh)
                    prepay = CustomBrokerWpf.References.PrepayStore.UpdateItem(reader.GetInt32(reader.GetOrdinal("prepayid")), addcon, out errors);
                else
                    prepay = CustomBrokerWpf.References.PrepayStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("prepayid")), addcon, out errors);
                this.Errors.AddRange(errors);
                Request request;
                if (this.FillType==lib.FillType.Refresh && myfilter!=null)
                    request = CustomBrokerWpf.References.RequestStore.UpdateItem(reader.GetInt32(reader.GetOrdinal("requestid")), addcon, out errors);
                else
                    request = CustomBrokerWpf.References.RequestStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("requestid")), addcon, out errors);
                if (errors.Count > 0)
                    this.Errors.AddRange(errors);
                else
                {
                    if (request?.SpecificationIsNull ?? false)
                        request.SpecificationInit = CustomBrokerWpf.References.SpecificationStore.GetItemLoad(request, addcon, out errors);
                    else if(this.FillType == lib.FillType.Refresh && myfilter != null)
                        CustomBrokerWpf.References.SpecificationStore.UpdateItem(request.Specification.Id, addcon, out errors);
                    this.Errors.AddRange(errors);
                }
                RequestCustomerLegal customer;
                if (myrequestcustomer != null)
                    customer = myrequestcustomer;
                else
                {
                    customer = CustomBrokerWpf.References.RequestCustomerLegalStore.GetItemLoad(prepay.Customer, request, addcon, out errors);
                    this.Errors.AddRange(errors);
                }

                item = new PrepayCustomerRequest(reader.GetInt32(0), reader.GetInt64(this.Fields["stamp"]), reader.GetDateTime(this.Fields["updated"]), reader.GetString(this.Fields["updater"]), lib.DomainObjectState.Unchanged
                    , customer
                    , reader.IsDBNull(this.Fields["dtsum"]) ? (decimal?)null : reader.GetDecimal(this.Fields["dtsum"])
                    , reader.GetDecimal(this.Fields["eurosum"])
                    , reader.IsDBNull(this.Fields["expirydate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["expirydate"])
                    , reader.GetDecimal(this.Fields["initsum"])
                    , reader.IsDBNull(this.Fields["note"]) ? null : reader.GetString(this.Fields["note"])
                    , prepay
                    , request
                    , reader.IsDBNull(this.Fields["selling"]) ? (decimal?)null : reader.GetDecimal(this.Fields["selling"])
                    , reader.IsDBNull(this.Fields["sellingdate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["sellingdate"])
                    , reader.GetDecimal(this.Fields["sellingrate"]));

                itemold = CustomBrokerWpf.References.PrepayRequestStore.UpdateItem(item, this.FillType == lib.FillType.Refresh);
                if (item.Selling.HasValue) itemold.Selling = item.Selling;
                if (myrequestcustomer == null && customer != null && this.FillType == lib.FillType.Refresh && myfilter != null)
                    CustomBrokerWpf.References.RequestCustomerLegalStore.UpdateItem(customer.Id, addcon, out errors);
            }
            return itemold;
        }
        protected override void GetOutputSpecificParametersValue(PrepayCustomerRequest item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                int prepayid = (int)InsertParams.First((SqlParameter par) => { return par.ParameterName == "@prepayid"; }).Value;
                if (item.Prepay.Id != prepayid)
                {
                    List<lib.DBMError> errors;
                    item.Prepay = CustomBrokerWpf.References.PrepayStore.GetItemLoad(prepayid, this.Command.Connection, out errors);
                    this.Errors.AddRange(errors);
                }
                CustomBrokerWpf.References.PrepayRequestStore.UpdateItem(item);
            }
        }
        protected override bool SaveChildObjects(PrepayCustomerRequest item)
        {
            if (item.EuroSum == 0M && item.DomainState == lib.DomainObjectState.Sealed)  // не сохраняем новый без EuroSum
                item.DomainState = lib.DomainObjectState.Added;
            return true;
        }
        protected override bool SaveIncludedObject(PrepayCustomerRequest item)
        {
            bool success = true;
            if (item.EuroSum == 0M && item.DomainState == lib.DomainObjectState.Added && (item.Prepay.IsPrepay ?? false) && (item.Request ?? item.RequestCustomer.Request).Status.Id > 0) // не сохраняем новый без EuroSum, не предоплатный(для разделения предоплаты по dtsum)
                item.DomainState = lib.DomainObjectState.Sealed;
            else
            {
                mypdbm.Errors.Clear();
                if (!mypdbm.SaveItemChanches(item.Prepay))
                {
                    foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
                    success = false;
                }
                if (myrdbm != null && item.Request != null)
                {
                    myrdbm.Errors.Clear();
                    if (!myrdbm.SaveItemChanches(item.Request))
                    {
                        foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
                        success = false;
                    }
                }
                if (mycidbm != null && item.CustomsInvoice != null)
                {
                    mycidbm.Errors.Clear();
                    if (!mycidbm.SaveItemChanches(item.CustomsInvoice))
                    {
                        foreach (lib.DBMError err in mycidbm.Errors) this.Errors.Add(err);
                        success = false;
                    }
                }
            }
            return success;
        }
        protected override bool SaveReferenceObjects()
        {

            if (mycidbm == null)
            {
                mycidbm = new CustomsInvoiceDBM();
            }
            mycidbm.Command.Connection = this.Command.Connection;
            mypdbm.Command.Connection = this.Command.Connection;
            if (myrdbm != null)
            {
                myrdbm.Command.Connection = this.Command.Connection;
                if (myrdbm.LegalDBM != null) myrdbm.LegalDBM.Command.Connection = this.Command.Connection;
                if (myrdbm.SpecificationDBM != null) myrdbm.SpecificationDBM.Command.Connection = this.Command.Connection;
                if (myrdbm.ParcelDBM != null) myrdbm.ParcelDBM.Command.Connection = this.Command.Connection;
            }
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.Command.CommandTimeout = 1000;
            foreach (SqlParameter par in this.SelectParams)
                switch (par.ParameterName)
                {
                    case "@customerid":
                        par.Value = myrequestcustomer?.CustomerLegal.Id ?? mycustomer?.Id;
                        break;
                    case "@filterid":
                        par.Value = myfilter?.FilterWhereId;
                        break;
                    case "@importerid":
                        par.Value = myrequestcustomer?.Request.Importer?.Id ?? myimporter?.Id;
                        break;
                    case "@parcelid":
                        par.Value = myparcel?.Id;
                        break;
                    case "@requestid":
                        par.Value = myrequestcustomer?.Request.Id;
                        break;
                }
            mypdbm.FillType = this.FillType;
        }
        protected override bool SetSpecificParametersValue(PrepayCustomerRequest item)
        {
            foreach (SqlParameter par in this.InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@dtsum":
                        par.Value = item.DTSumSet;
                        break;
                    case "@eurosum":
                        par.Value = item.EuroSum;
                        break;
                    case "@expirydate":
                        par.Value = item.ExpiryDate;
                        break;
                    case "@initsum":
                        par.Value = item.InitSum;
                        break;
                    case "@note":
                        par.Value = item.Note;
                        break;
                    case "@sellingdate":
                        par.Value = item.SellingDate;
                        break;
                    case "@sellingrate":
                        par.Value = item.SellingRate;
                        break;
                }
            foreach (SqlParameter par in this.UpdateParams)
                switch (par.ParameterName)
                {
                    case "@dtsumupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.DTSum));
                        break;
                    case "@eurosumupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.EuroSum));
                        break;
                    case "@expirydateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.ExpiryDate));
                        break;
                    case "@initsumupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.InitSum));
                        break;
                    case "@noteupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.Note));
                        break;
                    case "@sellingdateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.SellingDate));
                        break;
                    case "@sellingrateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.SellingRate));
                        break;
                }
            foreach (SqlParameter par in this.InsertParams)
                switch (par.ParameterName)
                {
                    case "@customerid":
                        par.Value = item.RequestCustomer?.CustomerLegal?.Id;
                        break;
                    case "@index":
                        par.Value = (item.RequestCustomer?.PrepaysIsNull ?? true) ? 0 : item.RequestCustomer?.Prepays.Count;
                        break;
                    case "@prepayid":
                        par.Value = item.Prepay?.Id;
                        break;
                    case "@requestid":
                        par.Value = item.RequestCustomer?.Request?.Id;
                        break;
                }
            return item.Prepay?.Id > 0 & !(item.RequestCustomer?.DomainState == lib.DomainObjectState.Added);
        }
        protected override void CancelLoad()
        {
        }

        internal void SetRequestDBM()
        {
            myrdbm = new RequestDBM();
            myrdbm.LegalDBM = null;
            myrdbm.SpecificationDBM = new Specification.SpecificationDBM();
            myrdbm.ParcelDBM = new ParcelDBM();
            myrdbm.ParcelDBM.RequestDBM = new RequestDBM();
            myrdbm.ParcelDBM.RequestDBM.LegalDBM = null;
            myrdbm.ParcelDBM.RequestDBM.ParcelDBM = null;
            myrdbm.ParcelDBM.RequestDBM.SpecificationDBM = null;
        }
    }

    public class PrepayCustomerRequestStore : lib.DomainStorageLoad<PrepayCustomerRequest, PrepayCustomerRequestDBM>
    {
        public PrepayCustomerRequestStore(PrepayCustomerRequestDBM dbm) : base(dbm) { }
        protected override void UpdateProperties(PrepayCustomerRequest olditem, PrepayCustomerRequest newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class PrepayCustomerRequestVM : lib.ViewModelErrorNotifyItem<PrepayCustomerRequest>, lib.Interfaces.ITotalValuesItem
    {
        public PrepayCustomerRequestVM(PrepayCustomerRequest model) : base(model)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.EuroSum), nameof(this.Request) });
            InitProperties();
            myfolderopen = new RelayCommand(PrepayRubPayAddExec, PrepayRubPayAddCanExec);
        }

        public decimal? CustomerBalance
        { get { return this.IsEnabled ? this.DomainObject.CustomerBalance : (decimal?)null; } }
        public CustomsInvoice CustomsInvoice
        { get { return this.IsEnabled ? this.DomainObject.CustomsInvoice : null; } }
        public decimal? CustomsInvoiceRubSum
        { get { return this.IsEnabled ? this.DomainObject.CustomsInvoiceRubSum : null; } }
        public DateTime? CurrencyBoughtDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CurrencyBoughtDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.CurrencyBoughtDate.Value, value.Value))))
                {
                    string name = nameof(this.CurrencyBoughtDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyBoughtDate);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyBoughtDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyBoughtDate : null; }
        }
        public decimal? CurrencyBuyRate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CurrencyBuyRate.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CurrencyBuyRate.Value, value.Value))))
                {
                    string name = nameof(this.CurrencyBuyRate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyBuyRate);
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.CurrencyBuyRate = value; }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyBuyRate : (decimal?)null; }
        }
        public decimal? CurrencyBuySum
        {
            get { return this.IsEnabled ? this.DomainObject.CurrencyBuySum : (decimal?)null; }
        }
        public DateTime? CurrencyPaidDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CurrencyPaidDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.CurrencyPaidDate.Value, value.Value))))
                {
                    string name = nameof(this.CurrencyPaidDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurrencyPaidDate);
                    ChangingDomainProperty = name; this.DomainObject.CurrencyPaidDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CurrencyPaidDate : null; }
        }
        public decimal? CurrencyPaySum
        { get { return this.IsEnabled ? this.DomainObject.CurrencyPaySum : (decimal?)null; } }
        private RequestCustomerLegalVM mycustomer;
        public RequestCustomerLegalVM Customer
        {
            get { return mycustomer; }
        }
        public decimal? DTSum
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.DTSum.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.DTSum.Value, value.Value))))
                {
                    string name = nameof(this.DTSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DTSumSet);
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.DTSum = value; }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DTSum : (decimal?)null; }
        }
        private decimal? myeurosum;
        public decimal? EuroSum
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.EuroSum, value.Value)))
                {
                    string name = nameof(this.EuroSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.EuroSum);
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.UpdateEuroSum(value.Value, true); }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.EuroSum : (decimal?)null; }
        }
        public DateTime? ExpiryDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.ExpiryDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.ExpiryDate.Value, value.Value))))
                {
                    string name = nameof(this.ExpiryDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ExpiryDate);
                    ChangingDomainProperty = name; this.DomainObject.ExpiryDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ExpiryDate : null; }
        }
        public decimal? FinalInvoiceCurSum
        {
            set
            {
                if (!(this.IsReadOnly || this.DomainObject.CustomsInvoice == null || decimal.Equals(this.DomainObject.CustomsInvoice?.FinalCurSum, (value, 0M))))
                {
                    string name = nameof(this.FinalInvoiceCurSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FinalInvoiceCurSum);
                    ChangingDomainProperty = name; this.DomainObject.FinalInvoiceCurSum = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.FinalInvoiceCurSum : (decimal?)null; }
        }
        public decimal? FinalInvoiceCurSum2
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || this.DomainObject.CustomsInvoice == null || decimal.Equals(this.DomainObject.CustomsInvoice?.FinalCurSum2, value.Value)))
                {
                    string name = nameof(this.FinalInvoiceCurSum2);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FinalInvoiceCurSum2);
                    ChangingDomainProperty = name; this.DomainObject.FinalInvoiceCurSum2 = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.FinalInvoiceCurSum2 : (decimal?)null; }
        }
        public decimal? FinalInvoiceRubSum
        { get { return this.IsEnabled ? this.DomainObject.FinalInvoiceRubSum : (decimal?)null; } }
        public decimal? FinalInvoiceRubSumPaid
        { get { return this.IsEnabled ? this.DomainObject.FinalInvoiceRubSumPaid : (decimal?)null; } }
        public decimal? InitSum
        {
            set
            {
                if (!(this.IsReadOnly || value.HasValue || decimal.Equals(this.DomainObject.InitSum, value.Value)))
                {
                    string name = nameof(this.InitSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.InitSum);
                    ChangingDomainProperty = name; this.DomainObject.InitSum = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.InitSum : (decimal?)null; }
        }
        public bool IsOverPay
        { get { return this.DomainObject.IsOverPay; } }
        public bool IsPrepay
        { get { return this.DomainObject.IsPrepay; } }
        public string Note
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Note, value)))
                {
                    string name = nameof(this.Note);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Note);
                    ChangingDomainProperty = name; this.DomainObject.Note = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Note : null; }
        }
        public decimal? OverPay
        { get { return this.IsEnabled ? this.DomainObject.OverPay : (decimal?)null; } }
        public Prepay Prepay
        { get { return this.IsEnabled ? this.DomainObject.Prepay : null; } }
        public decimal? Refund
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.Prepay.Refund, value.Value)))
                {
                    string name = nameof(this.DTSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Prepay.Refund);
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.Refund = value.Value; }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Refund : (decimal?)null; }
        }
        private RequestVM myrequestvm;
        public RequestVM Request
        {
            get { return this.IsEnabled ? myrequestvm : null; }
        }
        public decimal? RubDiff
        { get { return this.IsEnabled ? this.DomainObject.RubDiff : (decimal?)null; } }
        public decimal? RubSum
        {
            get { return this.IsEnabled ? this.DomainObject.RubSum : (decimal?)null; }
        }
        public decimal? Selling
        {
            get { return this.IsEnabled ? this.DomainObject.Selling : (decimal?)null; }
        }
        public DateTime? SellingDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.SellingDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.SellingDate.Value, value.Value))))
                {
                    string name = nameof(this.SellingDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SellingDate);
                    ChangingDomainProperty = name; this.DomainObject.SellingDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.SellingDate : null; }
        }
        public DateTime? Updated
        { get { return this.DomainObject.Updated; } }
        public string Updater
        { get { return this.DomainObject.Updater; } }

        public bool ProcessedIn { set; get; }
        public bool ProcessedOut { set; get; }
        private bool myselected;
        public bool Selected
        {
            set
            {
                bool oldvalue = myselected; myselected = value; this.OnValueChanged("Selected", oldvalue, value);
                this.PropertyChangedNotification(nameof(this.Selected));
            }
            get { return myselected; }
        }

        private RelayCommand myfolderopen;
        public ICommand PrepayRubPayAdd
        {
            get { return myfolderopen; }
        }
        private void PrepayRubPayAddExec(object parametr)
        {
        }
        private bool PrepayRubPayAddCanExec(object parametr)
        { return this.Prepay.InvoiceDate.HasValue; }

        protected override bool DirtyCheckProperty()
        {
            return myeurosum != this.DomainObject.EuroSum;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(this.DomainObject.IsLoaded):
                    this.PropertyChangedNotification(nameof(this.Prepay));
                    this.PropertyChangedNotification(nameof(this.Request));
                    this.PropertyChangedNotification(nameof(this.CustomsInvoice));
                    this.PropertyChangedNotification(nameof(this.CustomsInvoiceRubSum));
                    this.PropertyChangedNotification(nameof(this.CurrencyPaySum));
                    this.PropertyChangedNotification(nameof(this.DTSum));
                    this.PropertyChangedNotification(nameof(this.FinalInvoiceRubSum));
                    this.PropertyChangedNotification(nameof(this.FinalInvoiceCurSum));
                    this.PropertyChangedNotification(nameof(this.OverPay));
                    this.PropertyChangedNotification(nameof(this.Refund));
                    this.PropertyChangedNotification(nameof(this.RubDiff));
                    this.PropertyChangedNotification(nameof(this.RubSum));
                    this.PropertyChangedNotification(nameof(this.Selling));
                    break;
                case nameof(this.DomainObject.RequestCustomer):
                    mycustomer = new RequestCustomerLegalVM(this.DomainObject.RequestCustomer);
                    break;
                case nameof(this.DomainObject.EuroSum):
                    myeurosum = this.DomainObject.EuroSum;
                    break;
                case nameof(PrepayCustomerRequest.Request):
                    if (this.DomainObject.Request != null)
                        myrequestvm = new RequestVM(this.DomainObject.Request);
                    else
                        myrequestvm = null;
                    break;
            }
        }
        protected override void InitProperties()
        {
            if (this.DomainObject.RequestCustomer != null)
                mycustomer = new RequestCustomerLegalVM(this.DomainObject.RequestCustomer);
            if (this.DomainObject.Request != null)
                myrequestvm = new RequestVM(this.DomainObject.Request);
            myeurosum = this.DomainObject.EuroSum;
            //mydtsum = this.DomainObject.DtSum;
            if (this.DomainObject != null) this.DomainObject.ValueChanged += this.Model_ValueChanged;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.CurrencyBoughtDate):
                    this.DomainObject.CurrencyBoughtDate = (DateTime?)value;
                    break;
                case nameof(this.CurrencyBuyRate):
                    this.DomainObject.CurrencyBuyRate = (decimal?)value;
                    break;
                case nameof(this.CurrencyPaidDate):
                    this.DomainObject.CurrencyPaidDate = (DateTime?)value;
                    break;
                case nameof(this.DTSum):
                    this.DomainObject.DTSum = (decimal)value;
                    break;
                case nameof(this.EuroSum):
                    if (myeurosum != this.DomainObject.EuroSum)
                        myeurosum = this.DomainObject.EuroSum;
                    else
                        this.EuroSum = (decimal)value;
                    break;
                case nameof(this.FinalInvoiceCurSum):
                    this.DomainObject.FinalInvoiceCurSum = (decimal)value;
                    break;
                case nameof(this.FinalInvoiceCurSum2):
                    this.DomainObject.FinalInvoiceCurSum2 = (decimal)value;
                    break;
                case nameof(this.InitSum):
                    this.DomainObject.InitSum = (decimal)value;
                    break;
                case nameof(this.Note):
                    this.DomainObject.Note = (string)value;
                    break;
                case nameof(this.Refund):
                    this.DomainObject.Refund = (decimal)value;
                    break;
                //case nameof(this.Request):
                //    this.DomainObject.Request = (value as RequestVM)?.DomainObject;
                //    break;
                case nameof(this.SellingDate):
                    this.DomainObject.SellingDate = (DateTime?)value;
                    break;
                case "DependentNew":
                    //this.Request?.RejectChanges();
                    this.DomainObject.Prepay.RejectChanges();
                    //this.CustomsInvoice?.RejectChanges();
                    break;
                case "DependentOld":
                    //this.Request?.RejectChanges();
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.EuroSum):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myeurosum, out errmsg);
                    break;
                case nameof(this.Request):
                    isvalid = this.Request.Validate(inform);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }

        private void Model_ValueChanged(object sender, lib.Interfaces.ValueChangedEventArgs<object> e)
        {
            this.OnValueChanged(e.PropertyName, e.OldValue, e.NewValue);
        }
    }

    public class PrepayCustomerRequestSynchronizer : lib.ModelViewCollectionsSynchronizer<PrepayCustomerRequest, PrepayCustomerRequestVM>
    {
        protected override PrepayCustomerRequest UnWrap(PrepayCustomerRequestVM wrap)
        {
            return wrap.DomainObject as PrepayCustomerRequest;
        }
        protected override PrepayCustomerRequestVM Wrap(PrepayCustomerRequest fill)
        {
            return new PrepayCustomerRequestVM(fill);
        }
    }

    public class PrepayCustomerRequestCustomerCommander : lib.ViewModelViewCommand // not use
    {
        internal PrepayCustomerRequestCustomerCommander(RequestCustomerLegal customer) : base()
        {
            mycustomer = customer;
            mypfdbm = new PrepayFundDBM();
            mypfdbm.Customer = customer;
            StringBuilder errstr = new StringBuilder();
            RefreshFund(errstr);

            mypcdbm = new PrepayCustomerRequestDBM();
            mypcdbm.RequestCustomer = customer;
            mydbm = mypcdbm;
            mypcdbm.Collection = customer.Prepays;
            mysync = new PrepayCustomerRequestSynchronizer();
            mysync.DomainCollection = customer.Prepays;
            base.Collection = mysync.ViewModelCollection;

            customer.Request.PropertyChanged += this.Request_PropertyChanged;
            myratedbm = new Specification.SpecificationCustomerInvoiceRateDBM();
            if (errstr.Length > 0)
                OpenPopup(mydbm.ErrorMessage, true);
        }

        private void Request_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Request.Blocked)) { this.PropertyChangedNotification(nameof(this.IsEditable)); this.PropertyChangedNotification(nameof(this.IsReadOnly)); }
        }

        private RequestCustomerLegal mycustomer;
        private PrepayFundDBM mypfdbm;
        private PrepayCustomerRequestDBM mypcdbm;
        private PrepayCustomerRequestSynchronizer mysync;
        Specification.SpecificationCustomerInvoiceRateDBM myratedbm;

        public bool IsEditable
        {
            get { return mycustomer.Request.Blocked; }
        }
        public bool IsReadOnly
        { get { return !mycustomer.Request.Blocked; } }

        protected override void AddData(object parametr)
        {
            base.AddData(new PrepayCustomerRequestVM(new PrepayCustomerRequest(mycustomer, new Prepay(CustomBrokerWpf.References.AgentStore.GetItemLoad(mycustomer.Request.AgentId ?? 0, out var errors), mycustomer.CustomerLegal, mycustomer.Request.Importer, mycustomer.Request.ShipPlanDate ?? CustomBrokerWpf.References.EndQuarter(DateTime.Today.AddDays(10))), null)));
        }
        protected override bool CanAddData(object parametr)
        {
            return this.IsEditable;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return this.IsEditable;
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override bool CanRejectChanges()
        {
            return this.IsEditable;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void OtherViewRefresh()
        {
        }
        protected override void RefreshData(object parametr)
        {
            StringBuilder errstr = new StringBuilder();
            mypcdbm.Errors.Clear();
            mypcdbm.Fill();
            if (mypcdbm.Errors.Count > 0)
                foreach (lib.DBMError err in mypcdbm.Errors) errstr.AppendLine(err.Message);
            else
            {
                mycustomer.Prepays = mypcdbm.Collection;
                RefreshFund(errstr);
            }
            if (!mycustomer.Request.SpecificationIsNull) // coefficients for DTSum
            {
                mycustomer.Request.Specification.InvoiceDTRates.Clear();
                myratedbm.Specification = mycustomer.Request.Specification;
                myratedbm.Load();
                if (myratedbm.Errors.Count > 0) foreach (lib.DBMError err in myratedbm.Errors) errstr.AppendLine(err.Message);
            }
            if (errstr.Length > 0) this.PopupText = errstr.ToString();
        }
        protected override void SettingView()
        {
        }

        private void RefreshFund(StringBuilder errstr)
        {
            if (mycustomer.Request.Status.Id == 0) return;
            bool find = false;
            mypfdbm.Errors.Clear();
            mypfdbm.Fill();
            if (mypfdbm.Errors.Count > 0)
                foreach (lib.DBMError err in mypfdbm.Errors) errstr.AppendLine(err.Message);
            foreach (Prepay pay in mypfdbm.Collection)
            {
                foreach (PrepayCustomerRequest item in mycustomer.Prepays)
                    if (item.Prepay.Id == pay.Id)
                    { item.Prepay.UpdateProperties(pay); find = true; break; }
                if (!find)
                    mycustomer.Prepays.Add(new PrepayCustomerRequest(mycustomer, pay, null));
            }
        }
    }
}
