using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Data.SqlClient;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class CustomsInvoicePay : lib.DomainBaseStamp
    {
        public CustomsInvoicePay(int id, long stamp, DateTime? updated, string updater, lib.DomainObjectState mstate
            , CustomsInvoice invoice, DateTime pdate, decimal psum
            , IValidator validator) : base(id, stamp, updated, updater, mstate)
        {
            myinvoice = invoice;
            mypdate = pdate;
            mypsum = psum;
            myvalidator = validator;
        }

        private CustomsInvoice myinvoice;
        public CustomsInvoice Invoice
        { set { SetProperty<CustomsInvoice>(ref myinvoice, value); } get { return myinvoice; } }
        private DateTime mypdate;
        public DateTime PayDate
        { set { SetProperty<DateTime>(ref mypdate, value); } get { return mypdate; } }
        private decimal mypsum;
        public decimal PaySum
        { set { SetProperty<decimal>(ref mypsum, value); } get { return mypsum; } }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            CustomsInvoicePay templ = sample as CustomsInvoicePay;
            this.Invoice = templ.Invoice;
            this.PayDate = templ.PayDate;
            this.PaySum = templ.PaySum;
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
                case nameof(this.PaySum):
                    this.PaySum = (decimal)value;
                    break;
                case nameof(this.UpdateWhen):
                    this.UpdateWhen = (DateTime?)value;
                    break;
                case nameof(this.UpdateWho):
                    this.UpdateWho = (string)value;
                    break;
            }
        }
        private IValidator myvalidator;
        internal bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            myvalidator.ValidateObject = this;
            return myvalidator.ValidateProperty(propertyname, value, out errmsg);
        }
    }
    public interface IValidator
    {
        object ValidateObject { set; }
        bool ValidateProperty(string propertyname, object value, out string errmsg);
    }
    internal class CustomsInvoicePayValidatorRub: IValidator
    {
        private CustomsInvoicePay mypay;

        public object ValidateObject { set => mypay=value as CustomsInvoicePay; }

        public bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                case nameof(mypay.PayDate):
                    if ((DateTime)value < mypay.Invoice.InvoiceDate)
                    {
                        errmsg = "Дата оплаты не может быть меньше даты выставления счета!";
                        isvalid = false;
                    }
                    break;
                case nameof(mypay.PaySum):
                    if ((decimal)value < 0M)
                    {
                        errmsg = "Сумма оплаты не может быть меньше ноля!";
                        isvalid = false;
                    }
                    //else if ((decimal)value > mypay.Invoice.RubSum - mypay.Invoice.PaySum)
                    //{
                    //    errmsg = "Сумма оплаты не должна быть больше суммы выставленного счета!";
                    //    isvalid = false;
                    //}
                    break;
            }
            return isvalid;
        }
    }
    internal class CustomsInvoicePayValidatorFinalCur1 : IValidator
    {
        private CustomsInvoicePay mypay;

        public object ValidateObject { set => mypay = value as CustomsInvoicePay; }

        public bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                case nameof(mypay.PayDate):
                    if ((DateTime)value < mypay.Invoice.InvoiceDate)
                    {
                        errmsg = "Дата оплаты не может быть меньше даты выставления счета!";
                        isvalid = false;
                    }
                    break;
                case nameof(mypay.PaySum):
                    if ((decimal)value < 0M)
                    {
                        errmsg = "Сумма оплаты не может быть меньше ноля!";
                        isvalid = false;
                    }
                    //else if ((decimal)value > mypay.Invoice.FinalCurSum - mypay.Invoice.FinalCurPaySum)
                    //{
                    //    errmsg = "Сумма оплаты не должна быть больше суммы выставленного счета!";
                    //    isvalid = false;
                    //}
                    break;
            }
            return isvalid;
        }
    }
    internal class CustomsInvoicePayValidatorFinalCur2 : IValidator
    {
        private CustomsInvoicePay mypay;

        public object ValidateObject { set => mypay = value as CustomsInvoicePay; }

        public bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                case nameof(mypay.PayDate):
                    if ((DateTime)value < mypay.Invoice.InvoiceDate)
                    {
                        errmsg = "Дата оплаты не может быть меньше даты выставления счета!";
                        isvalid = false;
                    }
                    break;
                case nameof(mypay.PaySum):
                    if ((decimal)value < 0M)
                    {
                        errmsg = "Сумма оплаты не может быть меньше ноля!";
                        isvalid = false;
                    }
                    //else if ((decimal)value > mypay.Invoice.FinalCurSum2 - mypay.Invoice.FinalCurPaySum2)
                    //{
                    //    errmsg = "Сумма оплаты не должна быть больше суммы выставленного счета!";
                    //    isvalid = false;
                    //}
                    break;
            }
            return isvalid;
        }
    }

    public class CustomsInvoicePayDBM : lib.DBManagerWhoWhen<CustomsInvoicePay>
    {
        public CustomsInvoicePayDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "account.CustomsInvoicePay_sp";
            InsertCommandText = "account.CustomsInvoicePayAdd_sp";
            UpdateCommandText = "account.CustomsInvoicePayUpd_sp";
            DeleteCommandText = "account.CustomsInvoicePayDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@invoiceid", System.Data.SqlDbType.Int) };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0]
                ,new SqlParameter("@invoiceid",System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@psumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@pdateupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
               myinsertupdateparams[0],myinsertupdateparams[1],myinsertupdateparams[2]
               ,new SqlParameter("@psum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@pdate",System.Data.SqlDbType.DateTime2)
             };
        }

        private CustomsInvoice myinvoice;
        internal CustomsInvoice Invoice { set { myinvoice = value; } get { return myinvoice; } }
        internal IValidator Validator { set; get; }

        protected override CustomsInvoicePay CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new CustomsInvoicePay(reader.GetInt32(reader.GetOrdinal("id")), reader.GetInt64(reader.GetOrdinal("stamp"))
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")), reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
                , lib.DomainObjectState.Unchanged
                , myinvoice, reader.GetDateTime(reader.GetOrdinal("pdate")), reader.GetDecimal(reader.GetOrdinal("psum"))
                ,this.Validator);
        }
        protected override void GetOutputSpecificParametersValue(CustomsInvoicePay item)
        {
        }
        protected override bool LoadObjects()
        {
            return true;
        }
        protected override bool SaveChildObjects(CustomsInvoicePay item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(CustomsInvoicePay item)
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
        protected override bool SetSpecificParametersValue(CustomsInvoicePay item)
        {
            myinsertparams[1].Value = item.Invoice.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue(nameof(item.PaySum));
            myupdateparams[2].Value = item.HasPropertyOutdatedValue(nameof(item.PayDate));
            myinsertupdateparams[3].Value = item.PaySum;
            myinsertupdateparams[4].Value = item.PayDate;
            return true;
        }
    }

    public class CustomsInvoicePayVM : lib.ViewModelErrorNotifyItem<CustomsInvoicePay>
    {
        public CustomsInvoicePayVM(CustomsInvoicePay model) : base(model)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.PaySum), nameof(this.PayDate) });
            InitProperties();
        }

        private decimal? mypaysum;
        public decimal? PaySum
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(mypaysum, value.Value)))
                {
                    string name = nameof(this.PaySum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PaySum);
                    mypaysum = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.PaySum = value.Value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? mypaysum : (decimal?)null; }
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
        public DateTime? Updated
        { get { return this.IsEnabled ? this.DomainObject.UpdateWhen : null; } }
        public string Updater
        { get { return this.IsEnabled ? this.DomainObject.UpdateWho : null; } }

        protected override bool DirtyCheckProperty()
        {
            return mypaysum != this.DomainObject.PaySum || mypaydate != this.DomainObject.PayDate;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(this.DomainObject.PaySum):
                    mypaysum = this.DomainObject.PaySum;
                    break;
                case nameof(this.DomainObject.PayDate):
                    mypaydate = this.DomainObject.PayDate;
                    break;
            }
        }
        protected override void InitProperties()
        {
            mypaysum = this.DomainObject.PaySum;
            mypaydate = this.DomainObject.PayDate;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.PaySum):
                    if (mypaysum != this.DomainObject.PaySum)
                        mypaysum = this.DomainObject.PaySum;
                    else
                        this.PaySum = (decimal)value;
                    break;
                case nameof(this.PayDate):
                    if (mypaydate != this.DomainObject.PayDate)
                        mypaydate = this.DomainObject.PayDate;
                    else
                        this.PayDate = (DateTime)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.PaySum):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mypaysum, out errmsg);
                    break;
                case nameof(this.PayDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mypaydate, out errmsg);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    public class CustomsInvoicePaySynchronizer : lib.ModelViewCollectionsSynchronizer<CustomsInvoicePay, CustomsInvoicePayVM>
    {
        protected override CustomsInvoicePay UnWrap(CustomsInvoicePayVM wrap)
        {
            return wrap.DomainObject as CustomsInvoicePay;
        }
        protected override CustomsInvoicePayVM Wrap(CustomsInvoicePay fill)
        {
            return new CustomsInvoicePayVM(fill);
        }
    }

    public class CustomsInvoicePayViewCommand : lib.ViewModelViewCommand
    {
        internal CustomsInvoicePayViewCommand(CustomsInvoice invoice, IValidator validator) : base()
        {
            mymaindbm = new CustomsInvoicePayDBM();
            mydbm = mymaindbm;
            mymaindbm.Invoice = invoice;
            mymaindbm.Validator = validator;
            mysync = new CustomsInvoicePaySynchronizer();
        }

        protected CustomsInvoicePayDBM mymaindbm;
        protected CustomsInvoicePaySynchronizer mysync;
        internal CustomsInvoice Invoice { get { return mymaindbm.Invoice; } }

        protected override void AddData(object parametr)
        {
            CustomsInvoicePayVM item = new CustomsInvoicePayVM(new CustomsInvoicePay(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, mymaindbm.Invoice, DateTime.Today, 0M, mymaindbm.Validator));
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
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(CustomsInvoicePayVM.PayDate), System.ComponentModel.ListSortDirection.Ascending));
        }
    }
    public class CustomsInvoicePayRubViewCommand: CustomsInvoicePayViewCommand
    {
        public CustomsInvoicePayRubViewCommand(CustomsInvoice invoice):base(invoice,new CustomsInvoicePayValidatorRub())
        {
            mymaindbm.Collection = mymaindbm.Invoice.Pays;
            mysync.DomainCollection = mymaindbm.Invoice.Pays;
            base.Collection = mysync.ViewModelCollection;
        }
        public override bool SaveDataChanges()
        {
            bool sucess = base.SaveDataChanges();
            mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.PaySum));
            mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.PaidDate));
            return sucess;
        }
    }
}
