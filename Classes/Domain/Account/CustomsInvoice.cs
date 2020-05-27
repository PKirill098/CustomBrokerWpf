﻿using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class CustomsInvoice : lib.DomainStampValueChanged
    {
        public CustomsInvoice(int id, long stamp, DateTime? updated, string updater, lib.DomainObjectState mstate
            , decimal? cbrate, decimal? dtsum, CustomerLegal customer, Importer importer, DateTime? invoicedate, string invoicenumber,decimal finalcursum, decimal finalcursum2, RequestCustomerLegal request, decimal percent, decimal rubsum
            ) : base(id, stamp, updated, updater, mstate)
        {
            mycbrate = cbrate;
            mydtsum = dtsum;
            mycustomer = customer;
            myimporter = importer;
            myinvoicedate = invoicedate;
            myinvoicenumber = invoicenumber;
            myfinalcursum = finalcursum;
            myfinalcursum2 = finalcursum2;
            myparcel = request;
            mypercent = percent;//0.27M
            myrubsum = rubsum;
            //myparcel.PropertyChanged += this.Parcel_PropertyChanged;
            myrater = new CurrencyRateProxy(CustomBrokerWpf.References.CurrencyRate);
            myrater.PropertyChanged += Rater_PropertyChanged;
        }
        public CustomsInvoice(RequestCustomerLegal request) :this(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, null,0M, request.CustomerLegal, request.Request.Importer, null,null,0M, 0M, request, 0.27M, 0M)
        {
            mypays = new ObservableCollection<CustomsInvoicePay>();
            myfinalpays = new ObservableCollection<FinalInvoicePay>();
            myfinalcurpays1 = new ObservableCollection<CustomsInvoicePay>();
            myfinalcurpays2 = new ObservableCollection<CustomsInvoicePay>();
        }

        private decimal? mycbrate;
        public decimal? CBRate
        {
            internal set
            {
                Action action = () =>
                      {
                          if (mycbrate.HasValue && myparcel.InvoiceDiscount.HasValue)
                              this.RubSum = decimal.Round(decimal.Multiply(decimal.Multiply(mycbrate.Value, myparcel.InvoiceDiscount.Value), mypercent));
                          else
                              this.RubSum = 0M;
                      };
                if ((mypays?.Count ?? 0M) == 0)
                    SetProperty<decimal?>(ref mycbrate, value, action);
            }
            get { return mycbrate; }
        }
        private CustomerLegal mycustomer;
        public CustomerLegal Customer
        { set { SetProperty<CustomerLegal>(ref mycustomer, value); } get { return mycustomer; } }
        private decimal? mydtsum;
        public decimal? DTSum
        {
            internal set { SetPropertyOnValueChanged<decimal?>(ref mydtsum, value); }
            get { return !mydtsum.HasValue & (mycbrate??0M)>0M ? decimal.Divide(decimal.Divide(myrubsum, mycbrate.Value),mypercent) : mydtsum; }
        }
        private Importer myimporter;
        public Importer Importer
        { set { SetProperty<Importer>(ref myimporter, value); } get { return myimporter; } }
        private DateTime? myinvoicedate;
        public DateTime? InvoiceDate
        { set {
                Action action = () =>
                {
                        if(myrater.RateDate == myinvoicedate)
                            this.CBRate = myrater.EURRate;
                        else
                        {
                            this.CBRate = null;
                            myrater.RateDate = myinvoicedate;
                        }
                };
                if ((mypays?.Count ?? 0M) == 0)
                    SetProperty<DateTime?>(ref myinvoicedate, value, action); }
            get { return myinvoicedate; } }
        private string myinvoicenumber;
        public string InvoiceNumber
        { set { SetProperty<string>(ref myinvoicenumber, value); } get { return myinvoicenumber; } }
        private decimal myfinalcursum;
        public decimal FinalCurSum
        {
            set { SetProperty<decimal>(ref myfinalcursum, value); }
            get { return myfinalcursum; }
        }
        private decimal myfinalcursum2;
        public decimal FinalCurSum2
        {
            set { SetProperty<decimal>(ref myfinalcursum2, value); }
            get { return myfinalcursum2; }
        }
        public decimal FinalCurPaySum
        { get { return this.FinalCurPays1?.Sum<CustomsInvoicePay>((CustomsInvoicePay item) => { return item.DomainState < lib.DomainObjectState.Deleted ? item.PaySum : 0M; }) ?? 0M; } }
        public decimal FinalCurPaySum2
        { get { return this.FinalCurPays2?.Sum<CustomsInvoicePay>((CustomsInvoicePay item) => { return item.DomainState < lib.DomainObjectState.Deleted ? item.PaySum : 0M; }) ?? 0M; } }
        public DateTime? FinalCurPaidDate1
        {
            set
            {
                if (value.HasValue)
                {
                    if (this.FinalCurSum - this.FinalCurPaySum > 0.99M)
                    {
                        CustomsInvoicePay pay = new CustomsInvoicePay(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, this, value.Value, this.FinalCurSum - this.FinalCurPaySum, new CustomsInvoicePayValidatorFinalCur1());
                        this.FinalCurPays1.Add(pay);
                        this.PropertyChangedNotification(nameof(this.FinalCurPaySum));
                    }
                    else
                    {
                        DateTime maxdate = DateTime.MinValue;
                        CustomsInvoicePay pay = null;
                        foreach (CustomsInvoicePay item in this.FinalCurPays1)
                            if (item.PayDate > maxdate)
                            { maxdate = item.PayDate; pay = item; }
                        pay.PayDate = value.Value;
                    }
                    //}
                    //else
                    //    System.Windows.MessageBox.Show("Перед внесением платежей необходимо указать дату выставления счета!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                }
                else
                {
                    //if (this.CurrencyBuys?.Count > 0)
                    //    System.Windows.MessageBox.Show("Невозможно удалить платежи, куплена валюта!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                    //else
                    if (System.Windows.MessageBox.Show("Удалить все оплаты ?", "Дата оплаты", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                    {
                        List<CustomsInvoicePay> del = new List<CustomsInvoicePay>();
                        foreach (CustomsInvoicePay item in this.FinalCurPays1)
                            if (item.DomainState == lib.DomainObjectState.Added)
                                del.Add(item);
                            else
                                item.DomainState = lib.DomainObjectState.Deleted;
                        foreach (CustomsInvoicePay item in del)
                            this.FinalCurPays1.Remove(item);
                        this.PropertyChangedNotification(nameof(this.FinalCurSum));
                    }
                }
                this.PropertyChangedNotification(nameof(this.FinalCurPaidDate1));
            }
            get
            {
                return this.FinalCurSum - this.FinalCurPaySum < 0.99M && this.FinalCurPays1.Count > 0 ? DateTime.FromOADate(this.FinalCurPays1.Max<CustomsInvoicePay>((CustomsInvoicePay item) => { return item.PayDate.ToOADate(); })) : (DateTime?)null;
            }
        }
        public DateTime? FinalCurPaidDate2
        {
            set
            {
                if (value.HasValue)
                {
                    if (this.FinalCurSum2 - this.FinalCurPaySum2 > 0.99M)
                    {
                        CustomsInvoicePay pay = new CustomsInvoicePay(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, this, value.Value, this.FinalCurSum2 - this.FinalCurPaySum2, new CustomsInvoicePayValidatorFinalCur2());
                        this.FinalCurPays2.Add(pay);
                        this.PropertyChangedNotification(nameof(this.FinalCurPaySum2));
                    }
                    else
                    {
                        DateTime maxdate = DateTime.MinValue;
                        CustomsInvoicePay pay = null;
                        foreach (CustomsInvoicePay item in this.FinalCurPays2)
                            if (item.PayDate > maxdate)
                            { maxdate = item.PayDate; pay = item; }
                        pay.PayDate = value.Value;
                    }
                    //}
                    //else
                    //    System.Windows.MessageBox.Show("Перед внесением платежей необходимо указать дату выставления счета!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                }
                else
                {
                    //if (this.CurrencyBuys?.Count > 0)
                    //    System.Windows.MessageBox.Show("Невозможно удалить платежи, куплена валюта!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                    //else
                    if (System.Windows.MessageBox.Show("Удалить все оплаты ?", "Дата оплаты", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                    {
                        List<CustomsInvoicePay> del = new List<CustomsInvoicePay>();
                        foreach (CustomsInvoicePay item in this.FinalCurPays2)
                            if (item.DomainState == lib.DomainObjectState.Added)
                                del.Add(item);
                            else
                                item.DomainState = lib.DomainObjectState.Deleted;
                        foreach (CustomsInvoicePay item in del)
                            this.FinalCurPays2.Remove(item);
                        this.PropertyChangedNotification(nameof(this.FinalCurSum2));
                    }
                }
                this.PropertyChangedNotification(nameof(this.FinalCurPaidDate2));
            }
            get
            {
                return this.FinalCurSum2 - this.FinalCurPaySum2 < 0.99M && this.FinalCurPays2.Count > 0 ? DateTime.FromOADate(this.FinalCurPays2.Max<CustomsInvoicePay>((CustomsInvoicePay item) => { return item.PayDate.ToOADate(); })) : (DateTime?)null;
            }
        }
        public DateTime? FinalRubPaidDate
        {
            set
            {
                if (value.HasValue)
                {
                    if (this.FinalRubSum - this.FinalRubPaySum > 0.0099M)
                    {
                        FinalInvoicePay pay = new FinalInvoicePay(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, this, value.Value, 0M, value.Value, this.RubSum - this.PaySum);
                        this.FinalRubPays.Add(pay);
                        this.PropertyChangedNotification(nameof(this.FinalRubPaySum));
                    }
                    else
                    {
                        DateTime maxdate = DateTime.MinValue;
                        FinalInvoicePay pay = null;
                        foreach (FinalInvoicePay item in this.FinalRubPays)
                            if (item.PayDate > maxdate)
                            { maxdate = item.PayDate; pay = item; }
                        pay.PayDate = value.Value;
                    }
                    //}
                    //else
                    //    System.Windows.MessageBox.Show("Перед внесением платежей необходимо указать дату выставления счета!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                }
                else
                {
                    //if (this.CurrencyBuys?.Count > 0)
                    //    System.Windows.MessageBox.Show("Невозможно удалить платежи, куплена валюта!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                    //else
                    if (System.Windows.MessageBox.Show("Удалить все оплаты ?", "Дата оплаты", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                    {
                        List<FinalInvoicePay> del = new List<FinalInvoicePay>();
                        foreach (FinalInvoicePay item in this.FinalRubPays)
                            if (item.DomainState == lib.DomainObjectState.Added)
                                del.Add(item);
                            else
                                item.DomainState = lib.DomainObjectState.Deleted;
                        foreach (FinalInvoicePay item in del)
                            this.FinalRubPays.Remove(item);
                        this.PropertyChangedNotification(nameof(this.FinalRubPaySum));
                    }
                }
                this.PropertyChangedNotification(nameof(this.FinalRubPaidDate));
            }
            get
            {
                return this.FinalRubSum.HasValue && (this.FinalRubSum ?? 0M) - this.FinalRubPaySum < 0.9M && this.FinalRubPays.Count>0 ? DateTime.FromOADate(this.FinalRubPays.Max<FinalInvoicePay>((FinalInvoicePay item) => { return item.PayDate.ToOADate(); })) : (DateTime?)null;
            }
        }
        public decimal FinalRubPaySum
        { get { return this.FinalRubPays?.Sum<FinalInvoicePay>((FinalInvoicePay item) => { return item.DomainState < lib.DomainObjectState.Deleted ? item.RubPaySum : 0M; }) ?? 0M; } }
        public decimal? FinalRubSum
        { get { return this.RubSum>0M ? myparcel.Prepays.Sum((prepay)=> { return prepay.FinalInvoiceRubSum??0M; }) : (decimal?)null; } }
        private RequestCustomerLegal myparcel;
        public RequestCustomerLegal Parcel
        { set {
                //if (myparcel!=null)
                //    myparcel.PropertyChanged -= this.Parcel_PropertyChanged;
                SetProperty<RequestCustomerLegal>(ref myparcel, value);
                //if (myparcel != null)
                //    myparcel.PropertyChanged += this.Parcel_PropertyChanged;
            }
            get { return myparcel; } }
        public decimal PaySum
        { get { return this.Pays?.Sum<CustomsInvoicePay>((CustomsInvoicePay item) => { return item.DomainState < lib.DomainObjectState.Deleted ? item.PaySum : 0M; }) ?? 0M; } }
        public DateTime? PaidDate
        {
            set
            {
                if (value.HasValue)
                {
                    //if (this.InvoiceDate.HasValue)
                    //{
                    if (this.RubSum - this.PaySum > 0.99M)
                    {
                        CustomsInvoicePay pay = new CustomsInvoicePay(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, this, value.Value, this.RubSum - this.PaySum,new CustomsInvoicePayValidatorRub());
                        this.Pays.Add(pay);
                        this.PropertyChangedNotification(nameof(this.PaySum));
                    }
                    else
                    {
                        DateTime maxdate = DateTime.MinValue;
                        CustomsInvoicePay pay = null;
                        foreach (CustomsInvoicePay item in this.Pays)
                            if (item.PayDate > maxdate)
                            { maxdate = item.PayDate; pay = item; }
                        pay.PayDate = value.Value;
                    }
                    //}
                    //else
                    //    System.Windows.MessageBox.Show("Перед внесением платежей необходимо указать дату выставления счета!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                }
                else
                {
                    //if (this.CurrencyBuys?.Count > 0)
                    //    System.Windows.MessageBox.Show("Невозможно удалить платежи, куплена валюта!", "Дата оплаты", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                    //else
                    if (System.Windows.MessageBox.Show("Удалить все оплаты ?", "Дата оплаты", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                    {
                        List<CustomsInvoicePay> del = new List<CustomsInvoicePay>();
                        foreach (CustomsInvoicePay item in this.Pays)
                            if (item.DomainState == lib.DomainObjectState.Added)
                                del.Add(item);
                            else
                                item.DomainState = lib.DomainObjectState.Deleted;
                        foreach (CustomsInvoicePay item in del)
                            this.Pays.Remove(item);
                        this.PropertyChangedNotification(nameof(this.PaySum));
                    }
                }
                this.PropertyChangedNotification(nameof(this.PaidDate));
            }
            get
            {
                return this.Pays.Count > 0 && this.RubSum - this.PaySum < 0.99M ? DateTime.FromOADate(this.Pays.Max<CustomsInvoicePay>((CustomsInvoicePay item) => { return item.PayDate.ToOADate(); })) : (DateTime?)null;
            }
        }
        private decimal mypercent;
        public decimal Percent
        { set { SetProperty<decimal>(ref mypercent, value, () => { this.PropertyChangedNotification(nameof(this.RubSum)); }); } get { return mypercent; } }
        private decimal myrubsum;
        public decimal RubSum
        {
            set {
                Action action = () =>
                {
                    this.PropertyChangedNotification(nameof(this.PaidDate));
                    this.PropertyChangedNotification(nameof(this.FinalRubSum));
                    this.PropertyChangedNotification(nameof(this.FinalRubPaidDate));
                };
                SetPropertyOnValueChanged<decimal>(ref myrubsum, value, action); }
            get { return myrubsum; }
        }

        internal bool Selected { set; get; }

        private ObservableCollection<CustomsInvoicePay> mypays; //created at boot
        internal ObservableCollection<CustomsInvoicePay> Pays
        {
            set { mypays = value; this.PropertyChangedNotification(nameof(this.PaySum)); }
            get
            {
                return mypays;
            }
        }
        private ObservableCollection<FinalInvoicePay> myfinalpays; //created at boot
        internal ObservableCollection<FinalInvoicePay> FinalRubPays
        {
            set
            {
                myfinalpays = value;
                foreach(FinalInvoicePay pay in myfinalpays)
                    pay.PropertyChanged += this.FinalRubPay_PropertyChanged;
                myfinalpays.CollectionChanged += this.FinalRubPays_CollectionChanged;
                this.PropertyChangedNotification(nameof(this.FinalRubPays));
            }
            get
            {
                return myfinalpays;
            }
        }
        private void FinalRubPays_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action== System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach(FinalInvoicePay pay in e.NewItems)
                    pay.PropertyChanged += this.FinalRubPay_PropertyChanged;
        }
        private void FinalRubPay_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if(e.PropertyName==nameof(FinalInvoicePay.RubPaySum)) this.PropertyChangedNotification(nameof(this.FinalRubPaySum));
        }

        private ObservableCollection<CustomsInvoicePay> myfinalcurpays1; //created at boot
        internal ObservableCollection<CustomsInvoicePay> FinalCurPays1
        {
            set { myfinalcurpays1 = value; this.PropertyChangedNotification(nameof(this.FinalCurPays1)); }
            get
            {
                return myfinalcurpays1;
            }
        }
        private ObservableCollection<CustomsInvoicePay> myfinalcurpays2; //created at boot
        internal ObservableCollection<CustomsInvoicePay> FinalCurPays2
        {
            set { myfinalcurpays2 = value; this.PropertyChangedNotification(nameof(this.FinalCurPays2)); }
            get
            {
                return myfinalcurpays2;
            }
        }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            CustomsInvoice templ = sample as CustomsInvoice;
            this.CBRate = templ.CBRate;
            this.DTSum = templ.DTSum;
            this.InvoiceDate = templ.InvoiceDate;
            this.InvoiceNumber = templ.InvoiceNumber;
            this.FinalCurSum = templ.FinalCurSum;
            this.FinalCurSum2 = templ.FinalCurSum2;
            this.Percent = templ.Percent;
            this.RubSum = templ.RubSum;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.CBRate):
                    this.CBRate = (decimal)CBRate;
                    break;
                case nameof(this.Customer):
                    this.Customer = (CustomerLegal)value;
                    break;
                case nameof(this.Importer):
                    this.Importer = (Importer)value;
                    break;
                case nameof(this.InvoiceDate):
                    this.InvoiceDate = (DateTime)value;
                    break;
                case nameof(this.InvoiceNumber):
                    this.InvoiceNumber = (string)value;
                    break;
                case nameof(this.FinalCurSum):
                    this.FinalCurSum = (decimal)value;
                    break;
                case nameof(this.FinalCurSum2):
                    this.FinalCurSum2 = (decimal)value;
                    break;
                case nameof(this.Parcel):
                    this.Parcel = (RequestCustomerLegal)value;
                    break;
                case nameof(this.Percent):
                    this.Percent = (decimal)value;
                    break;
                case nameof(this.RubSum):
                    this.RubSum = (decimal)value;
                    break;
                case "DependentNew":
                    if (mypays != null)
                    {
                        int i = 0;
                        CustomsInvoicePay[] removed = new CustomsInvoicePay[mypays.Count];
                        foreach (CustomsInvoicePay item in mypays)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                removed[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach (CustomsInvoicePay item in removed)
                            if (item != null) mypays.Remove(item);
                    }
                    if (myfinalpays != null)
                    {
                        int i = 0;
                        FinalInvoicePay[] removed = new FinalInvoicePay[myfinalpays.Count];
                        foreach (FinalInvoicePay item in myfinalpays)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                removed[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach (FinalInvoicePay item in removed)
                            if (item != null) myfinalpays.Remove(item);
                    }
                    if (myfinalcurpays1 != null)
                    {
                        int i = 0;
                        CustomsInvoicePay[] removed = new CustomsInvoicePay[myfinalcurpays1.Count];
                        foreach (CustomsInvoicePay item in myfinalcurpays1)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                removed[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach (CustomsInvoicePay item in removed)
                            if (item != null) myfinalcurpays1.Remove(item);
                    }
                    if (myfinalcurpays2 != null)
                    {
                        int i = 0;
                        CustomsInvoicePay[] removed = new CustomsInvoicePay[myfinalcurpays2.Count];
                        foreach (CustomsInvoicePay item in myfinalcurpays2)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                removed[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach (CustomsInvoicePay item in removed)
                            if (item != null) myfinalcurpays2.Remove(item);
                    }
                    break;
            }
        }
        //private void Parcel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case nameof(Parcel.Requests):
        //            this.PropertyChangedNotification(nameof(this.FinalRubSum));
        //            this.PropertyChangedNotification(nameof(this.FinalRubPaidDate));
        //            break;
        //    }
        //}
        private Task myprepaydistributetask;
        private PrepayCustomerRequestDBM myrpdbm;
        internal void PrepayDistribute(string property,int decimals)
        {
            if (!myparcel.InvoiceDiscount.HasValue) return;
            if(myrpdbm==null)
            {
                myrpdbm = new PrepayCustomerRequestDBM() { RequestCustomer=this.Parcel };//, Importer = this.ImporterCustomer = this.Customer, Requ = this.Parcel
                myrpdbm.Fill();
                //myrpdbm.FillAsyncCompleted = () => {
                //    decimal? val;
                //    decimal d = 0M, d1 = 0M, d2 =0M, sd = 0M, s= 0M,sr=0M,sdr=0M;
                //    decimal total = myrpdbm.Collection.Sum((PrepayCustomerRequest prepay) => { return prepay.DTSum ?? 0M; });
                //    switch (property)
                //    {
                //        case nameof(PrepayCustomerRequest.CustomsInvoiceRubSum):
                //            total = decimal.Divide(decimal.Round(this.RubSum, decimals), total);
                //            break;
                //    }
                //    foreach (PrepayCustomerRequest prepay in myrpdbm.Collection)
                //    {
                //        if (prepay.DTSum.HasValue)
                //        {
                //            s = decimal.Multiply(total, prepay.DTSum.Value);
                //            sr = decimal.Round(s, decimals);
                //            d1 = s > sr ? s - sr : sr - s;
                //            sd = s + d;
                //            sdr = decimal.Round(sd, decimals);
                //            d2 = sd > sdr ? sd - sdr : sdr - sd;
                //            if (d1 > d2)
                //            {
                //                d = d2;
                //                val = sdr;
                //            }
                //            else
                //            {
                //                d = d + d2;
                //                val = sr;
                //            }
                //        }
                //        else
                //            val = null;
                //        switch (property)
                //        {
                //            case nameof(prepay.CustomsInvoiceRubSum):
                //                prepay.CustomsInvoiceRubSum = val;
                //                break;

                //        }
                //    }
                //};
            }
            //if(myprepaydistributetask==null || myprepaydistributetask.Status!=TaskStatus.Running)
            //    myprepaydistributetask=myrpdbm.FillAsync();
            if (myrpdbm.Collection.Count == 1)
                switch (property)
                {
                    case nameof(PrepayCustomerRequest.CustomsInvoiceRubSum):
                        myrpdbm.Collection[0].CustomsInvoiceRubSum = this.RubSum;
                        break;
                }
            else
            {
                decimal? val;
                decimal d = 0M, d1 = 0M, d2 = 0M, sd = 0M, s = 0M, sr = 0M, sdr = 0M;
                decimal total = myrpdbm.Collection.Sum((PrepayCustomerRequest prepay) => { return prepay.DTSum ?? 0M; });
                if (total == 0M) return;
                switch (property)
                {
                    case nameof(PrepayCustomerRequest.CustomsInvoiceRubSum):
                        total = decimal.Divide(decimal.Round(this.RubSum, decimals), total);
                        break;
                }
                foreach (PrepayCustomerRequest prepay in myrpdbm.Collection)
                {
                    if (prepay.DTSum.HasValue)
                    {
                        s = decimal.Multiply(total, prepay.DTSum.Value);
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
                        val = null;
                    switch (property)
                    {
                        case nameof(prepay.CustomsInvoiceRubSum):
                            prepay.CustomsInvoiceRubSum = val;
                            break;

                    }
                }
            }
        }
        internal void UnSubscribe()
        {
            //if (myparcel != null)
            //    myparcel.PropertyChanged -= this.Parcel_PropertyChanged;
            if(myfinalpays!=null)
            foreach (FinalInvoicePay pay in myfinalpays)
                pay.PropertyChanged -= this.FinalRubPay_PropertyChanged;
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

    internal class CustomsInvoiceDBM : lib.DBManagerWhoWhen<CustomsInvoice>
    {
        internal CustomsInvoiceDBM()
        {
            NeedAddConnection = true;
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "account.CustomsInvoice_sp";
            InsertCommandText = "account.CustomsInvoiceAdd_sp";
            UpdateCommandText = "account.CustomsInvoiceUpd_sp";
            DeleteCommandText = "account.CustomsInvoiceDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int), new SqlParameter("@customerid", System.Data.SqlDbType.Int), new SqlParameter("@importerid", System.Data.SqlDbType.Int), new SqlParameter("@parcelid", System.Data.SqlDbType.Int), new SqlParameter("@requestid", System.Data.SqlDbType.Int), new SqlParameter("@notready", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output } };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0]
                ,new SqlParameter("@customerid",System.Data.SqlDbType.Int)
                ,new SqlParameter("@importerid",System.Data.SqlDbType.Int)
                ,new SqlParameter("@parcelid",System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@cbrateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicedateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@invoicenumberupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@finalcursumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@finalcursum2upd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@percentupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@rubsumupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
               myinsertupdateparams[0],myinsertupdateparams[1],myinsertupdateparams[2]
               ,new SqlParameter("@cbrate",System.Data.SqlDbType.Money)
               ,new SqlParameter("@invoicedate",System.Data.SqlDbType.DateTime2)
               ,new SqlParameter("@invoicenumber", System.Data.SqlDbType.NVarChar,10)
               ,new SqlParameter("@finalcursum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@finalcursum2",System.Data.SqlDbType.Money)
               ,new SqlParameter("@percent",System.Data.SqlDbType.Money)
               ,new SqlParameter("@rubsum",System.Data.SqlDbType.Money)
             };
            mypdbm = new CustomsInvoicePayDBM();
            myfpdbm = new FinalInvoicePayDBM();
        }

        CustomsInvoicePayDBM mypdbm;
        FinalInvoicePayDBM myfpdbm;
        private CustomerLegal mycustomer;
        internal CustomerLegal Customer
        { set { mycustomer = value; } get { return mycustomer; } }
        private Importer myimporter;
        internal Importer Importer
        { set { myimporter = value; } get { return myimporter; } }
        internal Parcel Parcel { set; get; }
        private Request myrequest;
        internal Request Request
        { set { myrequest = value; } get { return myrequest; } }
        private RequestCustomerLegal myparcel;
        internal RequestCustomerLegal RequestCustomer
        { set { myparcel = value; } get { return myparcel; } }

        protected override CustomsInvoice CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            RequestCustomerLegal customer = CustomBrokerWpf.References.RequestCustomerLegalStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("parcelid")), addcon);

            CustomsInvoice item = new CustomsInvoice(reader.IsDBNull(0) ? lib.NewObjectId.NewId : reader.GetInt32(0), reader.GetInt64(reader.GetOrdinal("stamp"))
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated"))
                , reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
                , reader.IsDBNull(0) ? lib.DomainObjectState.Added : lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("cbrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("cbrate"))
                , reader.IsDBNull(reader.GetOrdinal("cursum")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("cursum"))
                , customer.CustomerLegal
                , reader.IsDBNull(reader.GetOrdinal("importerid")) ? null : CustomBrokerWpf.References.Importers.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("importerid")))
                , reader.IsDBNull(reader.GetOrdinal("invoicedate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("invoicedate"))
                , reader.IsDBNull(reader.GetOrdinal("invoicenumber")) ? null : reader.GetString(reader.GetOrdinal("invoicenumber"))
                , reader.GetDecimal(reader.GetOrdinal("finalcursum"))
                , reader.GetDecimal(reader.GetOrdinal("finalcursum2"))
                , customer
                , reader.GetDecimal(reader.GetOrdinal("percent"))
                , reader.GetDecimal(reader.GetOrdinal("rubsum")));
            if (item.Id > 0) item = CustomBrokerWpf.References.CustomsInvoiceStore.UpdateItem(item);

            mypdbm.Errors.Clear();
            mypdbm.Command.Connection = addcon;
            mypdbm.Invoice = item;
            mypdbm.Validator = new CustomsInvoicePayValidatorRub();
            mypdbm.SelectCommandText = "account.CustomsInvoicePay_sp";
            if (item.Pays != null)
            {
                mypdbm.Collection = item.Pays;
                mypdbm.Fill();
            }
            else
            {
                mypdbm.Fill();
                item.Pays = mypdbm.Collection;
            }
            mypdbm.Collection = null;
            myfpdbm.Errors.Clear();
            myfpdbm.Command.Connection = addcon;
            myfpdbm.Invoice = item;
            if (item.FinalRubPays != null)
            {
                myfpdbm.Collection = item.FinalRubPays;
                myfpdbm.Fill();
            }
            else
            {
                myfpdbm.Fill();
                item.FinalRubPays = myfpdbm.Collection;
            }
            myfpdbm.Collection = null;
            mypdbm.Validator = new CustomsInvoicePayValidatorFinalCur1();
            mypdbm.SelectCommandText = "account.FinalInvoicePayCur1_sp";
            if (item.FinalCurPays1 != null)
            {
                mypdbm.Collection = item.FinalCurPays1;
                mypdbm.Fill();
            }
            else
            {
                mypdbm.Fill();
                item.FinalCurPays1 = mypdbm.Collection;
            }
            mypdbm.Collection = null;
            mypdbm.Validator = new CustomsInvoicePayValidatorFinalCur2();
            mypdbm.SelectCommandText = "account.FinalInvoicePayCur2_sp";
            if (item.FinalCurPays2 != null)
            {
                mypdbm.Collection = item.FinalCurPays2;
                mypdbm.Fill();
            }
            else
            {
                mypdbm.Fill();
                item.FinalCurPays2 = mypdbm.Collection;
            }
            mypdbm.Collection = null;
            return item;
        }
        protected override void GetOutputSpecificParametersValue(CustomsInvoice item)
        {
        }
        protected override bool LoadObjects()
        {
            //bool success = true;
            //foreach (CustomsInvoice item in this.Collection)
            //{
            //    LoadObjects(item);
            //}
            return true;
        }
        protected override void LoadObjects(CustomsInvoice item)
        {
        }
        protected override bool SaveChildObjects(CustomsInvoice item)
        {
            bool isSuccess = true;
            mypdbm.Errors.Clear();
            mypdbm.InsertCommandText = "account.CustomsInvoicePayAdd_sp";
            mypdbm.UpdateCommandText = "account.CustomsInvoicePayUpd_sp";
            mypdbm.DeleteCommandText = "account.CustomsInvoicePayDel_sp";
            mypdbm.Collection = item.Pays;
            if (!mypdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
            }
            mypdbm.Errors.Clear();
            mypdbm.InsertCommandText = "account.FinalInvoicePayCur1Add_sp";
            mypdbm.UpdateCommandText = "account.FinalInvoicePayCur1Upd_sp";
            mypdbm.DeleteCommandText = "account.FinalInvoicePayCur1Del_sp";
            mypdbm.Collection = item.FinalCurPays1;
            if (!mypdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
            }
            mypdbm.Errors.Clear();
            mypdbm.InsertCommandText = "account.FinalInvoicePayCur2Add_sp";
            mypdbm.UpdateCommandText = "account.FinalInvoicePayCur2Upd_sp";
            mypdbm.DeleteCommandText = "account.FinalInvoicePayCur2Del_sp";
            mypdbm.Collection = item.FinalCurPays2;
            if (!mypdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in mypdbm.Errors) this.Errors.Add(err);
            }
            myfpdbm.Errors.Clear();
            myfpdbm.Collection = item.FinalRubPays;
            if (!myfpdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in myfpdbm.Errors) this.Errors.Add(err);
            }
            return isSuccess;
        }
        protected override bool SaveIncludedObject(CustomsInvoice item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            mypdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override void SetSelectParametersValue()
        {
            foreach (SqlParameter par in this.SelectParams)
                switch (par.ParameterName)
                {
                    case "@customerid":
                        par.Value = mycustomer?.Id?? myparcel?.CustomerLegal.Id;
                        break;
                    case "@importerid":
                        par.Value = myimporter?.Id;
                        break;
                    case "@parcelid":
                        par.Value = this.Parcel?.Id;
                        break;
                    case "@requestid":
                        par.Value = myrequest?.Id ?? myparcel?.Request.Id;
                        break;
                }
        }
        protected override bool SetSpecificParametersValue(CustomsInvoice item)
        {
            foreach (SqlParameter par in this.InsertParams)
                switch (par.ParameterName)
                {
                    case "@customerid":
                        par.Value = item.Customer.Id;
                        break;
                    case "@importerid":
                        par.Value = item.Importer?.Id;
                        break;
                    case "@parcelid":
                        par.Value = item.Parcel.Request.Id;
                        break;
                }
            foreach (SqlParameter par in this.UpdateParams)
                switch (par.ParameterName)
                {
                    case "@cbrateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(CustomsInvoice.CBRate));
                        break;
                    case "@invoicedateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(CustomsInvoice.InvoiceDate));
                        break;
                    case "@invoicenumberupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(CustomsInvoice.InvoiceNumber));
                        break;
                    case "@finalcursumupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(CustomsInvoice.FinalCurSum));
                        break;
                    case "@finalcursum2upd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(CustomsInvoice.FinalCurSum2));
                        break;
                    case "@percentupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(CustomsInvoice.Percent));
                        break;
                    case "@rubsumupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(CustomsInvoice.RubSum));
                        break;
                }
            foreach (SqlParameter par in this.InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@cbrate":
                        par.Value = item.CBRate;
                        break;
                    case "@invoicedate":
                        par.Value = item.InvoiceDate;
                        break;
                    case "@invoicenumber":
                        par.Value = item.InvoiceNumber;
                        break;
                    case "@finalcursum":
                        par.Value = item.FinalCurSum;
                        break;
                    case "@finalcursum2":
                        par.Value = item.FinalCurSum2;
                        break;
                    case "@percent":
                        par.Value = item.Percent;
                        break;
                    case "@rubsum":
                        par.Value = item.RubSum;
                        break;
                }
            return true;
        }
    }

    internal class CustomsInvoiceStore : lib.DomainStorageLoad<CustomsInvoice>
    {
        public CustomsInvoiceStore(CustomsInvoiceDBM dbm) : base(dbm) { }

        internal CustomsInvoice GetItem(CustomerLegal customer, Request request)
        {
            return Dispatcher.Invoke<CustomsInvoice>(() =>
            {
                CustomsInvoice firstitem = default(CustomsInvoice);
                if (request != null && customer != null)
                {
                    foreach (CustomsInvoice item in mycollection.Values)
                        if (item.Customer == customer && item.Parcel.Request == request)
                        { firstitem = item; break; }
                }
                return firstitem;
            });
        }
        internal CustomsInvoice GetItem(RequestCustomerLegal customer)
        {
            return Dispatcher.Invoke<CustomsInvoice>(() =>
            {
                CustomsInvoice firstitem = default(CustomsInvoice);
                if (customer != null)
                {
                    foreach (CustomsInvoice item in mycollection.Values)
                        if (item.Parcel == customer)
                        { firstitem = item; break; }
                }
                return firstitem;
            });
        }
        internal CustomsInvoice GetItemLoad(CustomerLegal customer, Request request)
        {
            return Dispatcher.Invoke<CustomsInvoice>(() =>
            {
                CustomsInvoice firstitem = default(CustomsInvoice);
                if (request != null && customer != null)
                {
                    firstitem = this.GetItem(customer, request);
                    if (firstitem == default(CustomsInvoice))
                    {
                        mydbm.Errors.Clear();
                        CustomsInvoiceDBM cidbm = mydbm as CustomsInvoiceDBM;
                        cidbm.Customer = customer;
                        //cidbm.Importer = importer;
                        cidbm.Request = request;
                        firstitem = mydbm.GetFirst();
                        if (firstitem == default(CustomsInvoice))
                        {
                            firstitem = new CustomsInvoice(request.CustomerLegals.First((RequestCustomerLegal legal)=> { return legal.CustomerLegal == customer; }));
                            cidbm.SaveItemChanches(firstitem);
                            if (!mycollection.ContainsKey(firstitem.Id)) base.AddItem(firstitem);
                        }
                    }
                }
                return firstitem;
            });
        }
        internal CustomsInvoice GetItemLoad(RequestCustomerLegal customer)
        {
            return Dispatcher.Invoke<CustomsInvoice>(() =>
            {
                CustomsInvoice firstitem = default(CustomsInvoice);
                if (customer != null)
                {
                    firstitem = this.GetItem(customer);
                    if (firstitem == default(CustomsInvoice))
                    {
                        mydbm.Errors.Clear();
                        CustomsInvoiceDBM cidbm = mydbm as CustomsInvoiceDBM;
                        cidbm.RequestCustomer = customer;
                        firstitem = mydbm.GetFirst();
                        if (firstitem == default(CustomsInvoice))
                        {
                            firstitem = new CustomsInvoice(customer);
                            cidbm.SaveItemChanches(firstitem);
                            if(!mycollection.ContainsKey(firstitem.Id))base.AddItem(firstitem);
                        }
                    }
                }
                return firstitem;
            });
        }
        internal CustomsInvoice GetItemLoad(CustomerLegal customer, Request request, SqlConnection conection)
        {
            return Dispatcher.Invoke<CustomsInvoice>(() =>
            {
                mydbm.Command.Connection = conection;
                return this.GetItemLoad(customer, request);
            });
        }
        internal CustomsInvoice GetItemLoad(RequestCustomerLegal customer, SqlConnection conection)
        {
            return Dispatcher.Invoke<CustomsInvoice>(() =>
            {
                mydbm.Command.Connection = conection;
                return this.GetItemLoad(customer);
            });
        }
        protected override void UpdateProperties(CustomsInvoice olditem, CustomsInvoice newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class CustomsInvoiceVM : lib.ViewModelErrorNotifyItem<CustomsInvoice>, lib.Interfaces.ITotalValuesItem
    {
        public CustomsInvoiceVM(CustomsInvoice model) : base(model)
        {
            //ValidetingProperties.AddRange(new string[] { nameof(this.EuroSum), nameof(this.Request) });
            InitProperties();
            //myfolderopen = new RelayCommand(PrepayRubPayAddExec, PrepayRubPayAddCanExec);
            model.ValueChanged += this.Model_ValueChanged;
        }

        public decimal? CBRate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CBRate.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.CBRate.Value, value.Value))))
                {
                    string name = nameof(this.CBRate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CBRate);
                    ChangingDomainProperty = name; this.DomainObject.CBRate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CBRate : (decimal?)null; }
        }
        public CustomerLegal Customer
        { get { return this.DomainObject.Customer; } }
        public decimal? CurSum
        { get { return this.DomainObject.DTSum; } }
        public Importer Importer
        { get { return this.IsEnabled ? this.DomainObject.Importer : null; } }
        public DateTime? InvoiceDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.InvoiceDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.InvoiceDate.Value, value.Value))))
                {
                    string name = nameof(this.InvoiceDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.InvoiceDate);
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.InvoiceDate = value; }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.InvoiceDate : (DateTime?)null; }
        }
        public string InvoiceNumber
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.InvoiceNumber, value)))
                {
                    string name = nameof(this.InvoiceNumber);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.InvoiceNumber);
                    ChangingDomainProperty = name; this.DomainObject.InvoiceNumber = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.InvoiceNumber : null; }
        }
        public decimal? FinalCurSum
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.FinalCurSum, value.Value)))
                {
                    string name = nameof(this.FinalCurSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FinalCurSum);
                    ChangingDomainProperty = name; this.DomainObject.FinalCurSum = value.Value;
                }
            }
            get { return this.DomainObject.FinalCurSum; }
        }
        public decimal? FinalCurSum2
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.FinalCurSum2, value.Value)))
                {
                    string name = nameof(this.FinalCurSum2);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FinalCurSum2);
                    ChangingDomainProperty = name; this.DomainObject.FinalCurSum2 = value.Value;
                }
            }
            get { return this.DomainObject.FinalCurSum2; }
        }
        public decimal? FinalRubSum
        {
            get { return this.DomainObject.FinalRubSum; }
        }

        public RequestCustomerLegal Parcel
        { get { return this.IsEnabled ? this.DomainObject.Parcel : null; } }
        public decimal? PaySum
        { get { return this.IsEnabled ? this.DomainObject.PaySum : (decimal?)null; } }
        public DateTime? PaidDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.PaidDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.PaidDate.Value, value.Value))))
                {
                    string name = nameof(this.PaidDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PaidDate);
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.PaidDate = value.Value; }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PaidDate : (DateTime?)null; }
        }
        public decimal? Percent
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.Percent*100, value.Value)))
                {
                    string name = nameof(this.FinalCurSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Percent);
                    ChangingDomainProperty = name; this.DomainObject.Percent = value.Value/100M;
                }
            }
            get { return this.DomainObject.Percent * 100; }
        }
        public decimal? RubSum
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.RubSum, value.Value)))
                {
                    string name = nameof(this.RubSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.RubSum);
                    ChangingDomainProperty = name; this.DomainObject.RubSum = value.Value;
                }
            }
            get { return this.DomainObject.RubSum; }
        }

        public bool ProcessedIn { set; get; }
        public bool ProcessedOut { set; get; }
        public bool Selected
        {
            set
            {
                //if (value && this.DomainState == lib.DomainObjectState.Deleted)
                //    this.DomainState = this.DomainObject.DomainStatePrevious;
                //else if (!value && this.DomainState != lib.DomainObjectState.Added)
                //    this.DomainState = lib.DomainObjectState.Deleted;
                bool oldvalue = this.DomainObject.Selected; this.DomainObject.Selected = value; this.OnValueChanged("Selected", oldvalue, value);
                this.PropertyChangedNotification(nameof(this.Selected));
            }
            get { return this.DomainObject.Selected; }
        }

        protected override bool DirtyCheckProperty()
        {
            return false;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
        }
        protected override void InitProperties()
        {
            this.DomainObject.Selected = !this.DomainObject.InvoiceDate.HasValue;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.CBRate):
                    this.DomainObject.CBRate = (decimal?)value;
                    break;
                case nameof(this.FinalCurSum):
                    this.DomainObject.FinalCurSum = (decimal)value;
                    break;
                case nameof(this.FinalCurSum2):
                    this.DomainObject.FinalCurSum2 = (decimal)value;
                    break;
                case nameof(this.InvoiceDate):
                    this.DomainObject.InvoiceDate = (DateTime?)value;
                    break;
                case nameof(this.InvoiceNumber):
                    this.DomainObject.InvoiceNumber = (string)value;
                    break;
                case nameof(this.PaidDate):
                    this.DomainObject.PaidDate = (DateTime?)value;
                    break;
                case nameof(this.RubSum):
                    this.DomainObject.RubSum = (decimal)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            return true;
        }

        private void Model_ValueChanged(object sender, ValueChangedEventArgs<object> e)
        {
            this.OnValueChanged(e.PropertyName, e.OldValue, e.NewValue);
        }
    }

    public class CustomsInvoiceSynchronizer : lib.ModelViewCollectionsSynchronizer<CustomsInvoice, CustomsInvoiceVM>
    {
        protected override CustomsInvoice UnWrap(CustomsInvoiceVM wrap)
        {
            return wrap.DomainObject as CustomsInvoice;
        }
        protected override CustomsInvoiceVM Wrap(CustomsInvoice fill)
        {
            return new CustomsInvoiceVM(fill as CustomsInvoice);
        }
    }

    public class CustomsInvoiceViewCommand : lib.ViewModelViewCommand
    {
        internal CustomsInvoiceViewCommand(Importer importer)
        {
            mymaindbm = new CustomsInvoiceDBM();
            mydbm = mymaindbm;
            mymaindbm.Importer = importer;
            mymaindbm.SaveFilter = (CustomsInvoice item) => { return item.Selected || item.DomainState == lib.DomainObjectState.Deleted; };
            mymaindbm.Collection = new ObservableCollection<CustomsInvoice>();
            mysync = new CustomsInvoiceSynchronizer();
            mysync.DomainCollection = mymaindbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            mytotal = new CustomsInvoiceTotal(myview);
            myparcels = new ListCollectionView(CustomBrokerWpf.References.ParcelNumbers);
            myparcels.SortDescriptions.Add(new System.ComponentModel.SortDescription("Sort", System.ComponentModel.ListSortDirection.Descending));
            myselectall = new RelayCommand(SelectAllExec, SelectAllCanExec);
            myrater = new CurrencyRateProxy(CustomBrokerWpf.References.CurrencyRate);
            myrater.PropertyChanged += Rater_PropertyChanged;
            mycalc = new RelayCommand(CalculateExec, CalculateCanExec);
            //if (mymaindbm.Errors.Count > 0)
            //    this.OpenPopup(mymaindbm.ErrorMessage, true);
        }

        CustomsInvoiceDBM mymaindbm;
        CustomsInvoiceSynchronizer mysync;
        private CustomsInvoiceTotal mytotal;
        public CustomsInvoiceTotal Total { get { return mytotal; } }
        internal Importer Importer
        { get { return mymaindbm.Importer; } }
        public int ParcelId
        {
            set
            {
                mymaindbm.Parcel = CustomBrokerWpf.References.ParcelStore.GetItemLoad(value);
                mymaindbm.Fill();
                if (mymaindbm.Errors.Count > 0) { this.PopupText = mymaindbm.ErrorMessage; this.PopupIsOpen = true; }
                //else if ((bool)mymaindbm.SelectParams.Where((SqlParameter par) => { return par.ParameterName == "@notready"; }).First().Value)
                //{ this.OpenPopup("Невозможно расчитать суммы таможенных счетов по машине. Нет разноски Юр. лиц по всем разбивкам!", true); }
                myview.Refresh();
            }
            get { return mymaindbm.Parcel?.Id ?? 0; }
        }
        private ListCollectionView myparcels;
        public ListCollectionView Parcels
        { get { return myparcels; } }
        private PaymentRegisterViewCommander myrcmd;
        internal PaymentRegisterViewCommander PaymentRegisterCMD
        { set { myrcmd = value; } get { return myrcmd; } }

        private DateTime? mybuydate;
        public DateTime? InvoiceDate
        {
            set
            {
                if (mybuydate.HasValue!=value.HasValue || (value.HasValue && !DateTime.Equals(mybuydate.Value, value.Value)))
                {
                    myrater.RateDate = value;
                    mybuydate = value;
                    foreach (CustomsInvoiceVM item in myview)
                        if (item.Selected) item.InvoiceDate = value;
                }
            }
            get { return mybuydate; }
        }
        private decimal? mybuyrate;
        public decimal? CBRate
        {
            internal set
            {
                mybuyrate = value;
                PropertyChangedNotification(nameof(this.CBRate));
                foreach (CustomsInvoiceVM item in myview)
                    if (item.Selected) item.CBRate = value;
            }
            get { return mybuyrate; }
        }
        private Classes.CurrencyRateProxy myrater;
        private void Rater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EURRate")
            {
                this.CBRate = myrater.EURRate ?? 0M;
            }
        }

        private RelayCommand myselectall;
        public ICommand SelectAll
        {
            get { return myselectall; }
        }
        private void SelectAllExec(object parametr)
        {
            bool select = (bool)parametr;
            foreach (object item in myview) if (item is ISelectable) (item as ISelectable).Selected = select;
        }
        private bool SelectAllCanExec(object parametr)
        { return true; }

        private RelayCommand mycalc;
        public ICommand Calculate
        {
            get { return mycalc; }
        }
        private void CalculateExec(object parametr)
        {
            foreach (CustomsInvoiceVM item in myview)
                if (item.Selected) item.InvoiceDate = mybuydate;
            foreach (CustomsInvoiceVM item in myview)
                if (item.Selected) item.CBRate = mybuyrate;
        }
        private bool CalculateCanExec(object parametr)
        { return true; }

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
        }
        protected override void RefreshData(object parametr)
        {
            mymaindbm.Fill();
            if (mymaindbm.Errors.Count > 0) this.PopupText = mymaindbm.ErrorMessage;
            //else if ((bool)mymaindbm.SelectParams.Where((SqlParameter par) => { return par.ParameterName == "@notready"; }).First().Value)
            //    this.PopupText = "Невозможно расчитать суммы таможенных счетов по машине. Нет разноски Юр. лиц по всем разбивкам!";
            myview.Refresh();
        }
        public override bool SaveDataChanges()
        {
            bool success = true;

            //foreach (CustomsInvoiceVM item in mysync.ViewModelCollection)
            //    if (item.Selected) { item.InvoiceDate = this.Total.InvoiceDate; item.CBRate = this.Total.CBRate; }
            success = base.SaveDataChanges();
            CustomBrokerWpf.References.CustomsInvoiceStore.ClearDestroyed();
            //foreach (CustomsInvoiceVM item in mysync.ViewModelCollection)
            //    if (item.Selected && item.DomainState == lib.DomainObjectState.Unchanged)
            //        CustomBrokerWpf.References.CustomsInvoiceStore.UpdateItem(item.DomainObject);
            if (myrcmd != null)
            {
                int p, p0;
                PrepayCustomerRequestVM curitem;
                p0 = myrcmd.Items.CurrentPosition;
                p = p0;
                while (p > -1 & p0 - p < 100)
                {
                    curitem = myrcmd.Items.GetItemAt(p) as PrepayCustomerRequestVM;
                    //if (curitem.CustomsInvoice?.DomainState == lib.DomainObjectState.Destroyed)
                    //    curitem.DomainObject.CustomsInvoice = null;
                    //else
                    //{
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.CustomsInvoice));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.CustomsInvoiceRubSum));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.Selling));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.FinalInvoiceRubSum));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.FinalInvoiceCurSum));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.FinalInvoiceCurSum2));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.CustomerBalance));
                    //}
                    p--;
                }
                p = p0 + 1;
                while (p < myrcmd.Items.Count & p - p0 < 100)
                {
                    curitem = myrcmd.Items.GetItemAt(p) as PrepayCustomerRequestVM;
                    //if (curitem.CustomsInvoice?.DomainState == lib.DomainObjectState.Destroyed)
                    //    curitem.DomainObject.CustomsInvoice = null;
                    //else
                    //{
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.CustomsInvoice));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.CustomsInvoiceRubSum));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.Selling));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.FinalInvoiceRubSum));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.FinalInvoiceCurSum));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.FinalInvoiceCurSum2));
                        curitem.DomainObject.PropertyChangedNotification(nameof(curitem.CustomerBalance));
                    //}
                    p++;
                }
            }
            return success;
        }
        protected override void SettingView()
        {
            myview.Filter = (object item) => { return true; };
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Customer.Name", System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class CustomsInvoiceTotal : lib.TotalCollectionValues<CustomsInvoiceVM>
    {
        internal CustomsInvoiceTotal(ListCollectionView view) : base(view)
        {
            myinitselected = 2; // if not selected - sum=0
            myselectedcount = view.Count + myinitselected; // start with select all
        }

        private int myitemcount;
        public int ItemCount { set { myitemcount = value; } get { return myitemcount; } }
        private decimal mytotalcost;
        public decimal TotalCost { set { mytotalcost = value; } get { return mytotalcost; } }
        private decimal mytotalcostrub;
        public decimal TotalCostRUB { set { mytotalcostrub = value; } get { return mytotalcostrub; } }

        protected override void Item_ValueChangedHandler(CustomsInvoiceVM sender, ValueChangedEventArgs<object> e)
        {
            decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
            switch (e.PropertyName)
            {
                case nameof(CustomsInvoiceVM.CurSum):
                    mytotalcost += newvalue - oldvalue;
                    PropertyChangedNotification("TotalCost");
                    break;
                case nameof(CustomsInvoiceVM.RubSum):
                    mytotalcostrub += newvalue - oldvalue;
                    PropertyChangedNotification("TotalCostRUB");
                    break;
            }
        }
        protected override void ValuesReset()
        {
            myitemcount = 0;
            mytotalcost = 0M;
            mytotalcostrub = 0M;
        }
        protected override void ValuesPlus(CustomsInvoiceVM item)
        {
            myitemcount++;
            mytotalcost = mytotalcost + (item.DomainObject.DTSum??0M);
            mytotalcostrub = mytotalcostrub + (item.DomainObject.RubSum);
        }
        protected override void ValuesMinus(CustomsInvoiceVM item)
        {
            myitemcount--;
            mytotalcost = mytotalcost - (item.DomainObject.DTSum??0M);
            mytotalcostrub = mytotalcostrub - (item.DomainObject.RubSum);
        }
        protected override void PropertiesChangedNotifycation()
        {
            this.PropertyChangedNotification("ItemCount");
            this.PropertyChangedNotification("TotalCost");
            this.PropertyChangedNotification("TotalCostRUB");
        }
    }
}
