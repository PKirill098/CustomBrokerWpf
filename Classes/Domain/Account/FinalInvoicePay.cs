using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Data.SqlClient;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class FinalInvoicePay : lib.DomainBaseStamp // delete
    {
        public FinalInvoicePay(int id, long stamp, DateTime? updated, string updater, lib.DomainObjectState mstate
            , CustomsInvoice invoice, DateTime curpdate, decimal curpsum, DateTime rubpdate, decimal rubpsum
            ) : base(id, stamp, updated, updater, mstate)
        {
            myinvoice = invoice;
            mypdate = curpdate;
            mycurpsum = curpsum;
            myrubpdate = rubpdate;
            myrubpsum = rubpsum;
        }
        private CustomsInvoice myinvoice;
        public CustomsInvoice Invoice
        { set { SetProperty<CustomsInvoice>(ref myinvoice, value); } get { return myinvoice; } }
        private DateTime mypdate;
        public DateTime PayDate
        { set { SetProperty<DateTime>(ref mypdate, value); } get { return mypdate; } }
        private decimal mycurpsum;
        public decimal CurPaySum
        { set { SetProperty<decimal>(ref mycurpsum, value); } get { return mycurpsum; } }
        private DateTime myrubpdate;// delete
        public DateTime RubPayDate
        { set { SetProperty<DateTime>(ref myrubpdate, value); } get { return myrubpdate; } }
        private decimal myrubpsum;
        public decimal RubPaySum
        { set { SetProperty<decimal>(ref myrubpsum, value); } get { return myrubpsum; } }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            FinalInvoicePay templ = sample as FinalInvoicePay;
            this.Invoice = templ.Invoice;
            this.PayDate = templ.PayDate;
            this.CurPaySum = templ.CurPaySum;
            this.RubPayDate = templ.RubPayDate;
            this.RubPaySum = templ.RubPaySum;
            this.UpdateWhen = templ.UpdateWhen;
            this.UpdateWho = templ.UpdateWho;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Invoice):
                    this.Invoice = (CustomsInvoice)value;
                    break;
                case nameof(this.PayDate):
                    this.PayDate = (DateTime)value;
                    break;
                case nameof(this.CurPaySum):
                    this.CurPaySum = (decimal)value;
                    break;
                case nameof(this.RubPayDate):
                    this.RubPayDate = (DateTime)value;
                    break;
                case nameof(this.RubPaySum):
                    this.RubPaySum = (decimal)value;
                    break;
                case nameof(this.UpdateWhen):
                    this.UpdateWhen = (DateTime?)value;
                    break;
                case nameof(this.UpdateWho):
                    this.UpdateWho = (string)value;
                    break;
            }
        }
        public override bool ValidateProperty(string propertyname, object value, out string errmsg, out byte messageKey)
        {
            bool isvalid = true;
            errmsg = null;
            messageKey = 0;
            switch (propertyname)
            {
                case nameof(this.PayDate):
                case nameof(this.RubPayDate):
                    if ((DateTime)value < this.Invoice.InvoiceDate)
                    {
                        errmsg = "Дата оплаты не может быть меньше даты выставления счета!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.CurPaySum):
                    if ((decimal)value < 0M)
                    {
                        errmsg = "Сумма оплаты не может быть меньше ноля!";
                        isvalid = false;
                    }
                    //else if ((decimal)value > this.Invoice.FinalCurSum-this.Invoice.FinalCurPaySum)
                    //{
                    //    errmsg = "Сумма оплаты не должна быть больше суммы выставленного счета!";
                    //    isvalid = false;
                    //}
                    break;
                case nameof(this.RubPaySum):
                    if ((decimal)value < 0M)
                    {
                        errmsg = "Сумма оплаты не может быть меньше ноля!";
                        isvalid = false;
                    }
                    //else if ((decimal)value > this.Invoice.FinalRubSum - this.Invoice.FinalRubPaySum)
                    //{
                    //    errmsg = "Сумма оплаты не должна быть больше суммы выставленного счета!";
                    //    isvalid = false;
                    //}
                    break;
            }
            return isvalid;
        }
        internal void UnSubscribe()
        {
           
        }
    }

    internal class FinalInvoicePayDBM : lib.DBManagerWhoWhen<FinalInvoicePay,FinalInvoicePay>
    {
        public FinalInvoicePayDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "account.FinalInvoicePay_sp";
            InsertCommandText = "account.FinalInvoicePayAdd_sp";
            UpdateCommandText = "account.FinalInvoicePayUpd_sp";
            DeleteCommandText = "account.FinalInvoicePayDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@invoiceid", System.Data.SqlDbType.Int) };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0],myinsertparams[1]
                ,new SqlParameter("@invoiceid",System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@curpsumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@curpdateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@rubpsumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@rubpdateupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
               myinsertupdateparams[0],myinsertupdateparams[1]
               ,new SqlParameter("@curpsum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@curpdate",System.Data.SqlDbType.DateTime2)
               ,new SqlParameter("@rubpsum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@rubpdate",System.Data.SqlDbType.DateTime2)
             };
        }

        private CustomsInvoice myinvoice;
        internal CustomsInvoice Invoice { set { myinvoice = value; } get { return myinvoice; } }
		protected override FinalInvoicePay CreateRecord(SqlDataReader reader)
		{
            return new FinalInvoicePay(reader.GetInt32(reader.GetOrdinal("id")), reader.GetInt64(reader.GetOrdinal("stamp"))
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")), reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
                , lib.DomainObjectState.Unchanged
                , myinvoice, reader.GetDateTime(reader.GetOrdinal("curpdate")), reader.GetDecimal(reader.GetOrdinal("curpsum")), reader.GetDateTime(reader.GetOrdinal("rubpdate")), reader.GetDecimal(reader.GetOrdinal("rubpsum")));
		}
		protected override FinalInvoicePay CreateModel(FinalInvoicePay reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
            return reader;
        }
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader), addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
		protected override void GetOutputSpecificParametersValue(FinalInvoicePay item)
        {
        }
        protected override bool SaveChildObjects(FinalInvoicePay item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(FinalInvoicePay item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[0].Value = myinvoice?.Id;
        }
        protected override bool SetSpecificParametersValue(FinalInvoicePay item)
        {
            myinsertparams[2].Value = item.Invoice.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue(nameof(item.CurPaySum));
            myupdateparams[2].Value = item.HasPropertyOutdatedValue(nameof(item.PayDate));
            myupdateparams[3].Value = item.HasPropertyOutdatedValue(nameof(item.RubPaySum));
            myupdateparams[4].Value = item.HasPropertyOutdatedValue(nameof(item.RubPayDate));
            myinsertupdateparams[2].Value = item.CurPaySum;
            myinsertupdateparams[3].Value = item.PayDate;
            myinsertupdateparams[4].Value = item.RubPaySum;
            myinsertupdateparams[5].Value = item.RubPayDate;
            return true;
        }
    }

    public class FinalInvoicePayVM : lib.ViewModelErrorNotifyItem<FinalInvoicePay>
    {
        public FinalInvoicePayVM(FinalInvoicePay model) : base(model)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.CurPaySum), nameof(this.PayDate), nameof(this.RubPaySum), nameof(this.RubPayDate) });
            InitProperties();
        }

        private decimal? mycurpaysum;
        public decimal? CurPaySum
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(mycurpaysum, value.Value)))
                {
                    string name = nameof(this.CurPaySum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurPaySum);
                    mycurpaysum = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.CurPaySum = value.Value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? mycurpaysum : (decimal?)null; }
        }
        private DateTime? mypaydate;
        public DateTime? PayDate
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(mypaydate, value.Value)))
                {
                    string name = nameof(this.PayDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PayDate);
                    mypaydate = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.PayDate = value.Value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? mypaydate : (DateTime?)null; }
        }
        private decimal? myrubpaysum;
        public decimal? RubPaySum
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(myrubpaysum, value.Value)))
                {
                    string name = nameof(this.RubPaySum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.RubPaySum);
                    myrubpaysum = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.RubPaySum = value.Value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? myrubpaysum : (decimal?)null; }
        }
        private DateTime? myrubpaydate;
        public DateTime? RubPayDate
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(myrubpaydate, value.Value)))
                {
                    string name = nameof(this.RubPayDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.RubPayDate);
                    myrubpaydate = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.RubPayDate = value.Value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? myrubpaydate : (DateTime?)null; }
        }
        public DateTime? Updated
        { get { return this.IsEnabled ? this.DomainObject.UpdateWhen : null; } }
        public string Updater
        { get { return this.IsEnabled ? this.DomainObject.UpdateWho : null; } }

        protected override bool DirtyCheckProperty()
        {
            return mycurpaysum != this.DomainObject.CurPaySum || mypaydate != this.DomainObject.PayDate || myrubpaysum != this.DomainObject.RubPaySum || myrubpaydate != this.DomainObject.RubPayDate;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(this.DomainObject.CurPaySum):
                    mycurpaysum = this.DomainObject.CurPaySum;
                    break;
                case nameof(this.DomainObject.PayDate):
                    mypaydate = this.DomainObject.PayDate;
                    break;
                case nameof(this.DomainObject.RubPaySum):
                    myrubpaysum = this.DomainObject.RubPaySum;
                    break;
                case nameof(this.DomainObject.RubPayDate):
                    myrubpaydate = this.DomainObject.RubPayDate;
                    break;
            }
        }
        protected override void InitProperties()
        {
            mycurpaysum = this.DomainObject.CurPaySum;
            mypaydate = this.DomainObject.PayDate;
            myrubpaysum = this.DomainObject.RubPaySum;
            myrubpaydate = this.DomainObject.RubPayDate;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.CurPaySum):
                    if (mycurpaysum != this.DomainObject.CurPaySum)
                        mycurpaysum = this.DomainObject.CurPaySum;
                    else
                        this.CurPaySum = (decimal)value;
                    break;
                case nameof(this.PayDate):
                    if (mypaydate != this.DomainObject.PayDate)
                        mypaydate = this.DomainObject.PayDate;
                    else
                        this.PayDate = (DateTime)value;
                    break;
                case nameof(this.RubPaySum):
                    if (myrubpaysum != this.DomainObject.RubPaySum)
                        myrubpaysum = this.DomainObject.RubPaySum;
                    else
                        this.RubPaySum = (decimal)value;
                    break;
                case nameof(this.RubPayDate):
                    if (myrubpaydate != this.DomainObject.RubPayDate)
                        myrubpaydate = this.DomainObject.RubPayDate;
                    else
                        this.RubPayDate = (DateTime)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.CurPaySum):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mycurpaysum, out errmsg, out _);
                    break;
                case nameof(this.PayDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mypaydate, out errmsg, out _);
                    break;
                case nameof(this.RubPaySum):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myrubpaysum, out errmsg, out _);
                    break;
                case nameof(this.RubPayDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, myrubpaydate, out errmsg, out _);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    public class FinalInvoicePaySynchronizer : lib.ModelViewCollectionsSynchronizer<FinalInvoicePay, FinalInvoicePayVM>
    {
        protected override FinalInvoicePay UnWrap(FinalInvoicePayVM wrap)
        {
            return wrap.DomainObject as FinalInvoicePay;
        }
        protected override FinalInvoicePayVM Wrap(FinalInvoicePay fill)
        {
            return new FinalInvoicePayVM(fill);
        }
    }

    public class FinalInvoicePayViewCommand : lib.ViewModelViewCommand
    {
        internal FinalInvoicePayViewCommand(CustomsInvoice invoice) : base()
        {
            mymaindbm = new FinalInvoicePayDBM();
            mydbm = mymaindbm;
            mymaindbm.Invoice = invoice;
            mymaindbm.Collection = mymaindbm.Invoice.FinalRubPays;
            mysync = new FinalInvoicePaySynchronizer();
            mysync.DomainCollection = mymaindbm.Invoice.FinalRubPays;
            base.Collection = mysync.ViewModelCollection;
        }

        private FinalInvoicePayDBM mymaindbm;
        private FinalInvoicePaySynchronizer mysync;
        internal CustomsInvoice Invoice { get { return mymaindbm.Invoice; } }

        protected override void AddData(object parametr)
        {
            FinalInvoicePayVM item = new FinalInvoicePayVM(new FinalInvoicePay(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, mymaindbm.Invoice, DateTime.Today, 0M, DateTime.Today, 0M));
            base.AddData(item);
        }
        protected override bool CanAddData(object parametr)
        {
            return true; ;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return true;
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
        }
        public override bool SaveDataChanges()
        {
            bool sucess = base.SaveDataChanges();
            mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.FinalCurPaySum));
            mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.FinalRubPaidDate));
            mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.FinalRubPaySum));
            //mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.FinalRubPaidDate));
            return sucess;
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(FinalInvoicePayVM.PayDate), System.ComponentModel.ListSortDirection.Ascending));
        }
    }
}
