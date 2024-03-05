using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Data.SqlClient;
using System.Threading;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class PrepayRubPay : lib.DomainBaseStamp
    {
        public PrepayRubPay(int id, long stamp, lib.DomainObjectState mstate, DateTime? updated,string updater 
            ,DateTime pdate,Prepay prepay,decimal psum
            ) : base(id, stamp, updated, updater, mstate)
        {
            mypdate = pdate;
            myprepay = prepay;
            mypsum = psum;
        }

        private DateTime mypdate;
        public DateTime PayDate
        { set { SetProperty<DateTime>(ref mypdate, value); } get { return mypdate; } }
        private decimal mypsum;
        public decimal PaySum
        { set { SetProperty<decimal>(ref mypsum, value); } get { return mypsum; } }
        private Prepay myprepay;
        public Prepay Prepay
        { set { SetProperty<Prepay>(ref myprepay, value); }get { return myprepay; } }

        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            PrepayRubPay templ = sample as PrepayRubPay;
            this.PayDate=templ.PayDate;
            this.PaySum = templ.PaySum;
            this.Prepay = templ.Prepay;
            this.UpdateWhen = templ.UpdateWhen;
            this.UpdateWho = templ.UpdateWho;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.PayDate):
                    this.PayDate = (DateTime)value;
                    break;
                case nameof(this.PaySum):
                    this.PaySum = (decimal)value;
                    break;
                case nameof(this.Prepay):
                    this.Prepay = (Prepay)value;
                    break;
                case nameof(this.UpdateWhen):
                    this.UpdateWhen = (DateTime?)value;
                    break;
                case nameof(this.UpdateWho):
                    this.UpdateWho = (string)value;
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
                case nameof(this.PayDate):
                    if(this.Prepay.InvoiceDate.HasValue && (DateTime)value<this.Prepay.InvoiceDate.Value)
                    {
                        errmsg = "Дата оплаты не может быть меньше даты выставления счета!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.PaySum):
                    if ((decimal)value < 0M)
                    {
                        errmsg = "Сумма оплаты не может быть меньше ноля!";
                        isvalid = false;
                    }
                    //else if (this.Prepay.InvoiceDate.HasValue && (decimal)value - this.PaySum - this.Prepay.RubSum + this.Prepay.RubPaySum > 0.99M)
                    //{
                    //    errmsg = "Переплата. Сумма оплат не может быть больше суммы выставленного счета!";
                    //    isvalid = false;
                    //}
                    break;
            }
            return isvalid;
        }
    }

    internal class PrepayRubPayDBM : lib.DBManagerWhoWhen<PrepayRubPay,PrepayRubPay>
    {
        public PrepayRubPayDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "account.InvoiceClientPay_sp";
            InsertCommandText = "account.InvoiceClientPayAdd_sp";
            UpdateCommandText = "account.InvoiceClientPayUpd_sp";
            DeleteCommandText = "account.InvoiceClientPayDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@prepayid", System.Data.SqlDbType.Int)/*, new SqlParameter("@requestid", System.Data.SqlDbType.Int), new SqlParameter("@parcelid", System.Data.SqlDbType.Int)*/ };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0],myinsertparams[1]
                ,new SqlParameter("@prepayid",System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@psumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@pdateupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
               myinsertupdateparams[0],myinsertupdateparams[1]
               ,new SqlParameter("@psum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@pdate",System.Data.SqlDbType.DateTime2)
             };
        }

        private Prepay myprepay;
        internal Prepay Prepay { set { myprepay = value; } get { return myprepay; } }

        protected override PrepayRubPay CreateRecord(SqlDataReader reader)
        {
            return new PrepayRubPay(reader.GetInt32(reader.GetOrdinal("id")), reader.GetInt64(reader.GetOrdinal("stamp")), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")), reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
                , reader.GetDateTime(reader.GetOrdinal("pdate")), myprepay, reader.GetDecimal(reader.GetOrdinal("psum")));
        }
		protected override PrepayRubPay CreateModel(PrepayRubPay record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			return record;
		}
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			base.TakeItem(this.CreateRecord(reader));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
        protected override bool SaveChildObjects(PrepayRubPay item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(PrepayRubPay item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[0].Value= myprepay?.Id;
        }
        protected override bool SetParametersValue(PrepayRubPay item)
        {
            base.SetParametersValue(item);
            myinsertparams[2].Value=item.Prepay.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue(nameof(PrepayRubPay.PaySum));
            myupdateparams[2].Value = item.HasPropertyOutdatedValue(nameof(PrepayRubPay.PayDate));
            myinsertupdateparams[2].Value=item.PaySum;
            myinsertupdateparams[3].Value = item.PayDate;
            return true;
        }
    }

    public class PrepayRubPayVM: lib.ViewModelErrorNotifyItem<PrepayRubPay>
    {
        public PrepayRubPayVM(PrepayRubPay model) : base(model)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.PaySum),nameof(this.PayDate) });
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
                    { ChangingDomainProperty = name; this.DomainObject.PaySum = value.Value;this.ClearErrorMessageForProperty(name); }
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
        public DateTime? UpdateWhen
        { get { return this.IsEnabled ? this.DomainObject.UpdateWhen: null; } }
        public string UpdateWho
        { get { return this.IsEnabled ? this.DomainObject.UpdateWho : null; } }

        protected override bool DirtyCheckProperty()
        {
            return mypaysum != this.DomainObject.PaySum || mypaydate!=this.DomainObject.PayDate;
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
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mypaysum, out errmsg, out _);
                    break;
                case nameof(this.PayDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mypaydate, out errmsg, out _);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    public class PrepayRubPaySynchronizer : lib.ModelViewCollectionsSynchronizer<PrepayRubPay, PrepayRubPayVM>
    {
        protected override PrepayRubPay UnWrap(PrepayRubPayVM wrap)
        {
            return wrap.DomainObject as PrepayRubPay;
        }
        protected override PrepayRubPayVM Wrap(PrepayRubPay fill)
        {
            return new PrepayRubPayVM(fill);
        }
    }

    public class PrepayRubPayViewCommand : lib.ViewModelViewCommand
    {
        internal PrepayRubPayViewCommand(Prepay prepay):base()
        {
            myrdbm = new PrepayRubPayDBM();
            mydbm = myrdbm;
            myrdbm.Prepay = prepay;
            myrdbm.Collection = myrdbm.Prepay.RubPays;
            mysync = new PrepayRubPaySynchronizer();
            mysync.DomainCollection = myrdbm.Prepay.RubPays;
            base.Collection = mysync.ViewModelCollection;
        }

        private PrepayRubPayDBM myrdbm;
        private PrepayRubPaySynchronizer mysync;
        public Prepay Prepay { get { return myrdbm.Prepay; } }

        protected override void AddData(object parametr)
        {
            PrepayRubPayVM item = new PrepayRubPayVM(new PrepayRubPay(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added,null,null, DateTime.Today, myrdbm.Prepay, 0M));
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
            myrdbm.Fill();
            if (myrdbm.Errors.Count > 0) this.PopupText = myrdbm.ErrorMessage;
        }
        public override bool SaveDataChanges()
        {
            bool success = base.SaveDataChanges();
            if (success)
            {
                myrdbm.Prepay.PropertyChangedNotification(nameof(Prepay.RubPaySum));
                myrdbm.Prepay.PropertyChangedNotification(nameof(Prepay.RubPaidDate));
                myrdbm.Prepay.PropertyChangedNotification(nameof(Prepay.RubDebt));
            }
            return success;
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("PayDate",System.ComponentModel.ListSortDirection.Ascending));
        }
    }
}
