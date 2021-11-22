using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class RequestPayment : lib.DomainStampValueChanged
    {
        public RequestPayment(int id, long stamp, string updater, DateTime? updated, lib.DomainObjectState dstate
            ,RequestCustomerLegal customer, byte paymenttype,byte doctype,string name,decimal sum,DateTime date,decimal? rate,decimal? perrate
            ) : base(id, stamp, updated, updater, dstate)
        {
            myrequestcustomer = customer;
            mypaymenttype = paymenttype;
            mydoctype = doctype;
            myname = name;
            mysum = sum;
            mydate = date;
            myrate = rate;
            myperrate = perrate;
            this.PropertyChanged += RequestPayment_PropertyChanged;
        }
        public RequestPayment():this(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added,null, 0, 0, null, 0M, DateTime.Now,null,null) { }

        //private Request myrequest;
        //public Request Request
        //{ set { SetProperty<Request>(ref myrequest, value); } get { return myrequest; } }
        private RequestCustomerLegal myrequestcustomer;
        public RequestCustomerLegal RequestCustomer
        { set { SetProperty<RequestCustomerLegal>(ref myrequestcustomer, value); CalcSum(); } get { return myrequestcustomer; } } //myrequestcustomer.PropertyChanged += RequestCL_PropertyChanged;
        private int mypaymenttype;
        public int PaymentType
        { set { SetProperty<int>(ref mypaymenttype, value); } get { return mypaymenttype; } }
        private int mydoctype;
        public int DocType
        { set { SetProperty<int>(ref mydoctype, value); CalcSum(); } get { return mydoctype; } }
        private string myname;
        public string Name
        { set { SetProperty<string>(ref myname, value); } get { return myname; } }
        private decimal mysum;
        public decimal Sum
        { set { SetPropertyOnValueChanged(ref mysum, value); } get { return mysum; } }
        private DateTime mydate;
        public DateTime Date
        { set { SetProperty<DateTime>(ref mydate, value,()=> { Rater.RateDate = mydate; }); } get { return mydate; } }
        private decimal? myrate;
        public decimal? Rate
        { set { SetProperty<decimal?>(ref myrate, value,()=> { CalcSum(); }); }
            get { return myrate; } }
        private decimal? myperrate;
        public decimal? RatePer
        { set { SetProperty<decimal?>(ref myperrate, value, () => { CalcSum(); }); } get { return myperrate; } }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Date":
                    this.Date = (DateTime)value;
                    break;
                case "Name":
                    this.Name = (string)value;
                    break;
                case "Sum":
                    this.Sum = (decimal)value;
                    break;
                case "DocType":
                    this.DocType = (int)value;
                    break;
                case "PaymentType":
                    this.PaymentType = (int)value;
                    break;
                case "Rate":
                    this.Rate = (decimal?)value;
                    break;
                case "RatePer":
                    this.RatePer = (decimal?)value;
                    break;
                case "Request":
                    this.RequestCustomer = (RequestCustomerLegal)value;
                    break;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            RequestPayment newitem=(RequestPayment)sample;
            this.PaymentType = newitem.PaymentType;
            this.DocType = newitem.DocType;
            this.Name = newitem.Name;
            this.Sum = newitem.Sum;
            this.Date = newitem.Date;
            this.Rate = newitem.Rate;
            this.RatePer = newitem.RatePer;
        }

        private Classes.CurrencyRate myrater;
        private Classes.CurrencyRate Rater
        {
            get
            {
                if (myrater == null)
                {
                    myrater = new CurrencyRate();
                    myrater.PropertyChanged += Rater_PropertyChanged;
                }
                return myrater;
            }
        }
        private void Rater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EURRate") this.Rate = myrater.EURRate;
            PropertyChangedNotification("EURRate");
        }
        internal void CalcSum()
        {
            if (this.DocType == 0/*this.Sum.HasValue & this.Sum > 0M & */ | myrequestcustomer == null) return;
            if (this.DocType == 1 & !this.Rate.HasValue & this.PaymentType != 3)
            {
                Rater.RateDate = mydate;
                return;
            }
            System.Windows.Data.ListCollectionView view = new System.Windows.Data.ListCollectionView(myrequestcustomer.Request.Payments);
            view.Filter = (item) => { return (item as RequestPayment).PaymentType == this.PaymentType & (item as RequestPayment).Id != this.Id; };
            decimal sum = 0M;
            if (this.DocType == 1 & this.PaymentType != 3)
            {
                if (this.PaymentType == 1)
                    sum = this.RequestCustomer.InvoiceDiscountAdd2per ?? 0M;
                else if (this.PaymentType == 2)
                    sum = (this.RequestCustomer.InvoiceDiscount ?? 0M) * 0.22M;
                if (this.PaymentType == 1 & this.RatePer.HasValue && this.RatePer.Value == 0.02M)
                    sum = sum * this.Rate.Value * 1.02M;
                else
                    sum = sum * this.Rate.Value;
            }
            foreach (RequestPayment item in view)
            {
                if (item.RequestCustomer != this.RequestCustomer) continue;
                if (this.DocType == 1 & this.PaymentType != 3) //счет
                {
                    if (item.DocType == 1)
                        sum -= item.Sum;
                }
                else if (item.DocType == this.DocType) //платежки
                    sum -= item.Sum;
                else
                    sum += item.Sum;
            }
            this.Sum = sum; // иначе не запишется признак изменения
        }
        //private void RequestCL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    switch (e.PropertyName)
        //    {
        //        case "InvoiceDiscountAdd2per":
        //            if(this.DocType==1) CalcSum();
        //            break;
        //            //case "InvoiceRate":
        //            //    if(this.PaymentType == 1 & myrequest.InvoiceRate != null) myrequest.InvoiceRate.PropertyChanged += Request_PropertyChanged;
        //            //    break;
        //            //case "PrepaymentsRate":
        //            //    if (this.PaymentType == 2 & myrequest.PrepaymentsRate != null) myrequest.PrepaymentsRate.PropertyChanged += Request_PropertyChanged;
        //            //    break;
        //    }
        //}

        #region Blocking
        private void RequestPayment_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DomainState")
            {
                if (this.DomainStatePrevious == lib.DomainObjectState.Unchanged & (this.DomainState == lib.DomainObjectState.Modified | this.DomainState == lib.DomainObjectState.Deleted))
                {
                    myrequestcustomer.Request.Blocking();
                }
                else if (this.DomainStatePrevious == lib.DomainObjectState.Modified | this.DomainStatePrevious == lib.DomainObjectState.Deleted)
                    myrequestcustomer.Request.UnBlocking();
            }
        }
        #endregion
    }

    public class RequestPaymentDBM : lib.DBManagerWhoWhen<RequestPayment>
    {
        public RequestPaymentDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "RequestPayment_sp";
            InsertCommandText = "RequestPaymentAdd_sp";
            UpdateCommandText = "RequestPaymentUpd_sp";
            DeleteCommandText = "RequestPaymentDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@requestid", System.Data.SqlDbType.Int) };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0]
                ,new SqlParameter("@requestid", System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@paymenttypetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@doctypetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@nametrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@sumtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@datetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@customerlegalidtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@ratetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@perratetrue", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0],myinsertupdateparams[1],myinsertupdateparams[2]
                ,new SqlParameter("@paymenttype", System.Data.SqlDbType.Int)
                ,new SqlParameter("@doctype", System.Data.SqlDbType.Int)
                ,new SqlParameter("@name", System.Data.SqlDbType.NVarChar,50)
                ,new SqlParameter("@sum", System.Data.SqlDbType.Money)
                ,new SqlParameter("@date", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@customerlegalid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@rate", System.Data.SqlDbType.Money)
                ,new SqlParameter("@perrate", System.Data.SqlDbType.Money)
            };
        }

        private Request myrequest;
        internal Request Request
        { set { myrequest = value; base.SelectParams[0].Value = value.Id; } get { return myrequest; } }

        protected override void SetSelectParametersValue()
        {
        }
        protected override RequestPayment CreateItem(SqlDataReader reader)
        {
            RequestCustomerLegal legal=null;
            int legalid = reader.GetInt32(reader.GetOrdinal("customerlegalid"));
            foreach(RequestCustomerLegal item in myrequest.CustomerLegals)
                if(item.CustomerLegal.Id== legalid)
                {
                    legal = item;
                    break;
                }
            return new RequestPayment(reader.GetInt32(0), reader.GetInt64(reader.GetOrdinal("stamp")), reader.IsDBNull(3) ? null : reader.GetString(3), reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2), lib.DomainObjectState.Unchanged
                , legal
                , reader.GetByte(reader.GetOrdinal("paymenttype"))
                , reader.GetByte(reader.GetOrdinal("doctype"))
                , reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"))
                , reader.GetDecimal(reader.GetOrdinal("sum"))
                , reader.GetDateTime(reader.GetOrdinal("date"))
                , reader.IsDBNull(reader.GetOrdinal("rate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("rate"))
                , reader.IsDBNull(reader.GetOrdinal("perrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("perrate"))
                );
        }
        protected override void GetOutputSpecificParametersValue(RequestPayment item) { }
        protected override bool SaveChildObjects(RequestPayment item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(RequestPayment item)
        {
            bool issuccess=true;
            if (myrequest.DomainState == lib.DomainObjectState.Added)
            {
                RequestDBM rdbm = new RequestDBM();
                rdbm.Command = this.Command;
                issuccess =rdbm.SaveItemChanches(myrequest);
                if (!issuccess)
                    this.Errors.AddRange(rdbm.Errors);
            }
            if (item.RequestCustomer?.DomainState == lib.DomainObjectState.Added)
            {
                CustomerLegalDBM rdbm = new CustomerLegalDBM();
                rdbm.Command = this.Command;
                issuccess &= rdbm.SaveItemChanches(item.RequestCustomer.CustomerLegal);
                if (!issuccess)
                    this.Errors.AddRange(rdbm.Errors);
            }
            return issuccess;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetSpecificParametersValue(RequestPayment item)
        {
            myinsertparams[1].Value = myrequest.Id;
            int i = 1;
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("PaymentType");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("DocType");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Name");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Sum");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Date");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("RequestCustomer");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("Rate");
            myupdateparams[i++].Value = item.HasPropertyOutdatedValue("PerRate");
            i = 3;
            myinsertupdateparams[i++].Value = item.PaymentType;
            myinsertupdateparams[i++].Value = item.DocType;
            myinsertupdateparams[i++].Value = item.Name;
            myinsertupdateparams[i++].Value = item.Sum;
            myinsertupdateparams[i++].Value = item.Date;
            myinsertupdateparams[i++].Value = item.RequestCustomer?.CustomerLegal.Id;
            myinsertupdateparams[i++].Value = item.Rate;
            myinsertupdateparams[i++].Value = item.RatePer;
            return true;
        }
        protected override void LoadObjects(RequestPayment item)
        {
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class RequestPaymentVM : lib.ViewModelErrorNotifyItem<RequestPayment>
    {
        public RequestPaymentVM(RequestPayment item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { "PaymentType", "DocType", "Sum", "Customer" });
            DeleteRefreshProperties.AddRange(new string[] { "PaymentType", "DocType", "Name", "Sum", "Date", "Customer" });
            InitProperties();
        }
        public RequestPaymentVM() : this(new RequestPayment()) { }

        //private RequestVM myrequest;
        //public RequestVM Request
        //{
        //    set
        //    {
        //        if (!(this.IsReadOnly || object.Equals(myrequest, value)))
        //        {
        //            string name = "Request";
        //            if (!myUnchangedPropertyCollection.ContainsKey(name))
        //                this.myUnchangedPropertyCollection.Add(name, myrequest);
        //            if (myrequest!=null)
        //            {
        //                myrequest.PropertyChanged -= Request_PropertyChanged;
        //                //if(myrequest.InvoiceRate!=null) myrequest.InvoiceRate.PropertyChanged -= Request_PropertyChanged;
        //            }
        //            myrequest = value;
        //            ChangingDomainProperty = name; this.DomainObject.Request = value?.DomainObject;
        //            if (myrequest != null && this.DocType == 1 & (this.PaymentType == 1 | this.PaymentType == 2))
        //            {
        //                myrequest.PropertyChanged += Request_PropertyChanged;
        //                //if (this.PaymentType == 1 & myrequest.InvoiceRate != null)
        //                //{
        //                //    myrequest.InvoiceRate.PropertyChanged += Request_PropertyChanged;
        //                //    myrequest.InvoiceRate.RateDate = this.Date;
        //                //}
        //                //if (this.PaymentType == 2 & myrequest.PrepaymentsRate != null)
        //                //{
        //                //    myrequest.PrepaymentsRate.PropertyChanged += Request_PropertyChanged;
        //                //    myrequest.PrepaymentsRate.RateDate = this.Date;
        //                //}
        //                CalcSum();
        //            }
        //        }
        //    }
        //    get { return this.IsEnabled ? myrequest : null; }
        //}
        private RequestCustomerLegal mycustomer;
        public RequestCustomerLegal RequestCustomer
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(mycustomer, value)))
                {
                    string name = "RequestCustomer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycustomer);
                    mycustomer = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.RequestCustomer = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.IsEnabled ? mycustomer : null; }
        }
        public int? PaymentType
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || (this.DomainObject.PaymentType == value.Value)))
                {
                    string name = "PaymentType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PaymentType);
                    ChangingDomainProperty = name; this.DomainObject.PaymentType = value.Value;
                    ClearErrorMessageForProperty(name);
                }
            }
            get { return this.IsEnabled ? this.DomainObject.PaymentType : (int?)null; }
        }
        public int? DocType
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || (this.DomainObject.DocType == value.Value)))
                {
                    string name = "DocType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DocType);
                    ChangingDomainProperty = name; this.DomainObject.DocType = value.Value;
                    PropertyChangedNotification("RateVisible");
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DocType : (int?)null; }
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
        private decimal mysum;
        public decimal? Sum
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(mysum, value.Value)))
                {
                    string name = "Sum";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mysum);
                    mysum = value.Value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Sum = value.Value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get
            {
                return this.IsEnabled ? mysum : (decimal?)null;
            }
        }
        public DateTime? Date
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(this.DomainObject.Date, value.Value)))
                {
                    string name = "Date";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Date);
                    ChangingDomainProperty = name; this.DomainObject.Date = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Date : (DateTime?)null; }
        }

        public string RateCB
        { get { return this.IsEnabled && this.DomainObject.DocType == 1 ? "Курс ЦБ" : null; } }
        public decimal? Rate
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.Rate, value.Value)))
                {
                    string name = "Rate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Rate);
                    this.DomainObject.Rate = value.Value;
                    ChangingDomainProperty = name; this.DomainObject.Rate = value.Value;
                }
            }
            get
            {
                return this.IsEnabled && this.DomainObject.DocType == 1 ? (this.RatePer.HasValue && this.RatePer.Value==0.02M ? this.DomainObject.Rate*1.02M : this.DomainObject.Rate) : (decimal?)null;
            }
        }
        public decimal? RatePer
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.RatePer, value.Value)))
                {
                    string name = "PerRate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.RatePer);
                    ChangingDomainProperty = name; this.DomainObject.RatePer = value.Value;
                    PropertyChangedNotification("Rate");
                }
            }
            get
            {
                return this.IsEnabled && this.DomainObject.DocType == 1 ? this.DomainObject.RatePer : (decimal?)null;
            }
        }
        public bool RateVisible
        { get { return this.DomainObject.DocType == 1; } }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "DocType":
                    PropertyChangedNotification("RateVisible");
                    PropertyChangedNotification("RateCB");
                    PropertyChangedNotification("RatePer");
                    PropertyChangedNotification("Rate");
                    break;
                case "Sum":
                    mysum = this.DomainObject.Sum;
                    break;
                case "RequestCustomer":
                    mycustomer = this.DomainObject.RequestCustomer;
                    break;
                case "PerRate":
                    PropertyChangedNotification("Rate");
                    break;
            }
        }
        protected override void InitProperties()
        {
            mysum =this.DomainObject.Sum;
            mycustomer = this.DomainObject.RequestCustomer;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Sum":
                    if (mysum != this.DomainObject.Sum)
                        mysum = this.DomainObject.Sum;
                    else
                        this.Sum = (decimal?)value;
                    break;
                case "PaymentType":
                    this.DomainObject.PaymentType = (int)value;
                    break;
                case "DocType":
                    this.DomainObject.DocType = (int)value;
                    break;
                case "Name":
                    this.DomainObject.Name = (string)value;
                    break;
                case "Date":
                    this.DomainObject.Date = (DateTime)value;
                    break;
                case "RequestCustomer":
                    if (mycustomer != this.DomainObject.RequestCustomer)
                        mycustomer = this.DomainObject.RequestCustomer;
                    else
                        this.RequestCustomer = (RequestCustomerLegal)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "PaymentType":
                    if (this.PaymentType==0)
                    {
                        errmsg = "Не указан тип платежа-счета!";
                        isvalid = false;
                    }
                    break;
                case "DocType":
                    if (this.DocType == 0)
                    {
                        errmsg = "Не указан тип документа!";
                        isvalid = false;
                    }
                    break;
                case "Sum":
                    if (this.DocType ==1 & !(mysum > 0M) ) // счет любой
                    {
                        errmsg = "Сумма платежа должна быть больше нуля!";
                        isvalid = false;
                    }
                    break;
                case "RequestCustomer":
                    if(mycustomer == null)
                    {
                        errmsg = "Необходимо указать юр. лицо!";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return mycustomer!= this.DomainObject.RequestCustomer || mysum!= this.DomainObject.Sum;
        }
    }

    internal class RequestPaymentSynchronizer : lib.ModelViewCollectionsSynchronizer<RequestPayment, RequestPaymentVM>
    {
        protected override RequestPayment UnWrap(RequestPaymentVM wrap)
        {
            return wrap.DomainObject;
        }
        protected override RequestPaymentVM Wrap(RequestPayment fill)
        {
            return new RequestPaymentVM(fill);
        }
    }

    internal class RequestPaymentViewComand : lib.ViewModelViewCommand
    {
        internal RequestPaymentViewComand()
        {
            mydbm = new RequestPaymentDBM();
            mysync = new RequestPaymentSynchronizer();
            mydbm.Fill();
            mysync.DomainCollection = mydbm.Collection;
            base.Collection = mysync.ViewModelCollection;
        }

        new RequestPaymentDBM mydbm;
        RequestPaymentSynchronizer mysync;

        public override bool SaveDataChanges()
        {
            bool isSuccess = true, isvalid;
            if (myview != null)
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                err.AppendLine("Изменения не сохранены");
                if (mydbm == null)
                    mydbm = new RequestPaymentDBM();
                else
                    mydbm.Errors.Clear();
                foreach (RequestPaymentVM item in myview.SourceCollection)
                {
                    isvalid = !(item.DomainState == lib.DomainObjectState.Added || item.DomainState == lib.DomainObjectState.Modified) || item.Validate(true);
                    if (isvalid)
                    {
                        isvalid = mydbm.SaveItemChanches(item.DomainObject);
                        if (item.DomainState == lib.DomainObjectState.Destroyed)
                            mysync.ViewModelCollection.Remove(item);
                    }
                    else
                        err.AppendLine(item.Errors);
                    isSuccess &= isvalid;
                }
                if (!isSuccess)
                {
                    err.AppendLine(mydbm.ErrorMessage);
                    this.PopupText = err.ToString();
                }
            }
            return isSuccess;
        }
        protected override void AddData(object parametr)
        {
            if (parametr is RequestPayment)
            {
                mysync.ViewModelCollection.Add(parametr as RequestPaymentVM);
            }
        }
        protected override bool CanAddData(object parametr)
        {
            return true;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return myview.CurrentItem != null && myview.CurrentItem is RequestPaymentVM;
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
        protected override void OtherViewRefresh() { }
        protected override void RefreshData(object parametr)
        {
            mydbm.Collection.Clear();
            mydbm.FillAsync();
        }
        protected override void RejectChanges(object parametr)
        {
            List<RequestPaymentVM> deleted = new List<RequestPaymentVM>();
            foreach (RequestPaymentVM item in mysync.ViewModelCollection)
            {
                if (item.DomainState == lib.DomainObjectState.Added)
                    deleted.Add(item);
                else
                {
                    myview.EditItem(item);
                    item.RejectChanges();
                    myview.CommitEdit();
                }
            }
            foreach (RequestPaymentVM delitem in deleted)
            {
                mysync.ViewModelCollection.Remove(delitem);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
            }
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Date",System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class RatePer
    {
        internal RatePer(decimal per)
        {
            myper = per;
        }
        private decimal myper;
        public decimal Per
        { get { return myper; } }
        public string PerStr
        { get { return (myper * 100M).ToString("N0")+"%"; } }
    }
}
