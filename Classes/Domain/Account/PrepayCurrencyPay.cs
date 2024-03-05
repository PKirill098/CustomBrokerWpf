using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Linq;
using System.Windows.Input;
using System.Threading;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class PrepayCurrencyPay : lib.DomainBaseStamp
    {
        public PrepayCurrencyPay(int id, long stamp, lib.DomainObjectState mstate, DateTime? updated, string updater
            , DateTime paydate, decimal cursum, Prepay prepay
            ) : base(id, stamp, updated, updater, mstate)
        {
            mypaydate = paydate;
            mycursum = cursum;
            myprepay = prepay;
        }

        private DateTime mypaydate;
        public DateTime PayDate
        { set { SetProperty<DateTime>(ref mypaydate, value); } get { return mypaydate; } }
        private decimal mycursum;
        public decimal CurSum
        { set { SetProperty<decimal>(ref mycursum, value); } get { return mycursum; } }
        internal bool Selected { set; get; }
        private Prepay myprepay;
        public Prepay Prepay
        { set { SetProperty<Prepay>(ref myprepay, value); } get { return myprepay; } }

        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            PrepayCurrencyPay templ = sample as PrepayCurrencyPay;
            this.PayDate = templ.PayDate;
            this.CurSum = templ.CurSum;
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
                case nameof(this.CurSum):
                    this.CurSum = (decimal)value;
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
                    if (this.Prepay.InvoiceDate.HasValue && (DateTime)value < this.Prepay.InvoiceDate.Value)
                    {
                        errmsg = "Дата оплаты не может быть меньше даты выставления счета!";
                        isvalid = false;
                    }
                    break;
                case nameof(this.CurSum):
                    //if ((decimal)value > decimal.Multiply(decimal.Divide(this.Prepay.RubPaySum, this.Prepay.CBRate.Value), this.Prepay.Percent) - this.Prepay.CurrencyPaySum)
                    //{
                    //    errmsg = "Недостаточно средств. Сумма оплаты не может быть больше суммы покупки!";
                    //    isvalid = false;
                    //}
                    //else
                    if ((decimal)value < 0M)
                    {
                        errmsg = "Сумма не может быть меньше ноля!";
                        isvalid = false;
                    }
                    break;
            }
            return isvalid;
        }
    }

    public class CurrencyPay : PrepayCurrencyPay
    {
        public CurrencyPay(int id, long stamp, lib.DomainObjectState mstate, DateTime? updated, string updater,
            DateTime paydate, decimal cursum, Prepay prepay,decimal credit
            ) : base(id, stamp, mstate, updated, updater, paydate, cursum, prepay)
        {
            mycredit = credit;
        }
        private decimal mycredit;
        public decimal Credit { get {return mycredit; }  }
    }

    public struct CurrencyPayRecord
    {
        internal int id;
        internal long stamp;
        internal DateTime? updated;
        internal string updater;
        internal DateTime paydate;
        internal decimal cursum;
        internal int prepay;
        internal decimal credit;
	}

    internal class PrepayCurrencyPayDBM : lib.DBManagerWhoWhen<SqlDataReader, PrepayCurrencyPay>
    {
        public PrepayCurrencyPayDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "account.CurrencyPayPrepay_sp";
            InsertCommandText = "account.CurrencyPayPrepayAdd_sp";
            UpdateCommandText = "account.CurrencyPayPrepayUpd_sp";
            DeleteCommandText = "account.CurrencyPayPrepayDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@prepayid", System.Data.SqlDbType.Int) };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0],myinsertparams[1]
                ,new SqlParameter("@prepayid",System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@cursumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@paydateupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
               myinsertupdateparams[0],myinsertupdateparams[1]
               ,new SqlParameter("@cursum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@paydate",System.Data.SqlDbType.DateTime2)
             };
        }

        private Prepay myprepay;
        internal Prepay Prepay { set { myprepay = value; } get { return myprepay; } }

        protected override SqlDataReader CreateRecord(SqlDataReader reader)
        {
			return reader;
        }
		protected override PrepayCurrencyPay CreateModel(SqlDataReader reader, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
            return new PrepayCurrencyPay(reader.GetInt32(reader.GetOrdinal("id")), reader.GetInt64(reader.GetOrdinal("stamp")), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")), reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
                , reader.GetDateTime(reader.GetOrdinal("paydate")), reader.GetDecimal(reader.GetOrdinal("cursum")), myprepay);
		}
        protected override void GetRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
            this.ModelFirst=CreateModel(reader,addcon,canceltasktoken);
        }
        protected override PrepayCurrencyPay GetModel(SqlConnection addcon, System.Threading.CancellationToken canceltasktoken)
        {
            return this.ModelFirst;
        }
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader), addcon, mycanceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
        protected override bool SaveChildObjects(PrepayCurrencyPay item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(PrepayCurrencyPay item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[0].Value = myprepay?.Id;
        }
        protected override bool SetParametersValue(PrepayCurrencyPay item)
        {
            base.SetParametersValue(item);
            myinsertparams[2].Value = item.Prepay.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue(nameof(PrepayCurrencyPay.CurSum));
            myupdateparams[2].Value = item.HasPropertyOutdatedValue(nameof(PrepayCurrencyPay.PayDate));
            myinsertupdateparams[2].Value = item.CurSum;
            myinsertupdateparams[3].Value = item.PayDate;
            return true;
        }
    }

    internal class CurrencyPayDBM : PrepayCurrencyPayDBM
    {
        internal CurrencyPayDBM()
        {
            this.NeedAddConnection = true;
            SelectCommandText = "account.CurrencyPay_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@agentid", System.Data.SqlDbType.Int), new SqlParameter("@importerid", System.Data.SqlDbType.Int) };
        }

        private Agent myagent;
        internal Agent Agent
        {
            set { myagent = value; }
            get
            {
                return myagent;
            }
        }
        private Importer myimporter;
        internal Importer Importer
        { set { myimporter = value; } get { return myimporter; } }

        protected override PrepayCurrencyPay CreateModel(SqlDataReader reader, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
        {
            return new CurrencyPay(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null
                , DateTime.Today, reader.GetDecimal(reader.GetOrdinal("paysum"))
                , CustomBrokerWpf.References.PrepayStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("prepayid")), addcon,out _)
                , reader.GetDecimal(reader.GetOrdinal("credit")));
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            foreach (SqlParameter par in this.SelectParams)
                switch (par.ParameterName)
                {
                    case "@agentid":
                        par.Value = myagent?.Id;
                        break;
                    case "@importerid":
                        par.Value = myimporter?.Id;
                        break;
                }
        }
    }

    public class PrepayCurrencyPayVM : lib.ViewModelErrorNotifyItem<PrepayCurrencyPay>, lib.Interfaces.ITotalValuesItem
    {
        public PrepayCurrencyPayVM(PrepayCurrencyPay model) : base(model)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.PayDate), nameof(this.CurSum) });
            InitProperties();
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
        private decimal? mycursum;
        public decimal? CurSum
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(mycursum, value.Value)))
                {
                    decimal oldvalue;
                    string name = nameof(this.CurSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurSum);
                    oldvalue = mycursum ?? 0M;
                    mycursum = value;
                    if (this.ValidateProperty(name))
                    {
                        oldvalue = this.DomainObject.CurSum;
                        ChangingDomainProperty = name; this.DomainObject.CurSum = value.Value; this.ClearErrorMessageForProperty(name);
                        this.OnValueChanged(nameof(this.CurSum), oldvalue, mycursum);
                    }
                }
            }
            get { return mycursum; }
        }
        public Prepay Prepay
        { get { return this.DomainObject.Prepay; } }
        public DateTime? Updated
        { get { return this.IsEnabled ? this.DomainObject.UpdateWhen : null; } }
        public string Updater
        { get { return this.IsEnabled ? this.DomainObject.UpdateWho : null; } }

        public bool ProcessedIn { set; get; }
        public bool ProcessedOut { set; get; }
        public virtual bool Selected
        {
            set {}
            get { return true; }
        }

        protected override bool DirtyCheckProperty()
        {
            return mypaydate != this.DomainObject.PayDate || mycursum != this.DomainObject.CurSum;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(this.DomainObject.PayDate):
                    mypaydate = this.DomainObject.PayDate;
                    break;
                case nameof(this.DomainObject.CurSum):
                    mycursum = this.DomainObject.CurSum;
                    break;
                case nameof(this.DomainObject.UpdateWhen):
                    PropertyChangedNotification(nameof(PrepayCurrencyPayVM.Updated));
                    break;
                case nameof(this.DomainObject.UpdateWho):
                    PropertyChangedNotification(nameof(this.Updater));
                    break;
            }
        }
        protected override void InitProperties()
        {
            mypaydate = this.DomainObject.PayDate;
            mycursum = this.DomainObject.CurSum;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.PayDate):
                    if (mypaydate != this.DomainObject.PayDate)
                        mypaydate = this.DomainObject.PayDate;
                    else
                        this.PayDate = (DateTime)value;
                    break;
                case nameof(this.CurSum):
                    if (mycursum != this.DomainObject.CurSum)
                        mycursum = this.DomainObject.CurSum;
                    else
                        this.CurSum = (decimal)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case nameof(this.PayDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mypaydate, out errmsg, out _);
                    break;
                case nameof(this.CurSum):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mycursum, out errmsg, out _);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
    }

    public class CurrencyPayVM : PrepayCurrencyPayVM
    {
        public CurrencyPayVM(CurrencyPay model) : base(model)
        {
        }

        public decimal Credit { get { return (this.DomainObject as CurrencyPay).Credit; } }
        public override bool Selected
        {
            set
            {
                if (value && this.DomainState == lib.DomainObjectState.Deleted)
                    this.DomainState = this.DomainObject.DomainStatePrevious;
                else if (!value && this.DomainState != lib.DomainObjectState.Added)
                    this.DomainState = lib.DomainObjectState.Deleted;
                bool oldvalue = this.DomainObject.Selected; this.DomainObject.Selected = value; this.OnValueChanged("Selected", oldvalue, value);
                this.PropertyChangedNotification(nameof(this.Selected));
            }
            get { return this.DomainObject.Selected; }
        }

        protected override void InitProperties()
        {
            base.InitProperties();
            this.DomainObject.Selected = true;
        }
    }

    public class PrepayCurrencyPaySynchronizer : lib.ModelViewCollectionsSynchronizer<PrepayCurrencyPay, PrepayCurrencyPayVM>
    {
        protected override PrepayCurrencyPay UnWrap(PrepayCurrencyPayVM wrap)
        {
            return wrap.DomainObject as PrepayCurrencyPay;
        }
        protected override PrepayCurrencyPayVM Wrap(PrepayCurrencyPay fill)
        {
            return new PrepayCurrencyPayVM(fill);
        }
    }

    public class CurrencyPaySynchronizer : lib.ModelViewCollectionsSynchronizer<PrepayCurrencyPay, CurrencyPayVM>
    {
        protected override PrepayCurrencyPay UnWrap(CurrencyPayVM wrap)
        {
            return wrap.DomainObject as PrepayCurrencyPay;
        }
        protected override CurrencyPayVM Wrap(PrepayCurrencyPay fill)
        {
            return new CurrencyPayVM(fill as CurrencyPay);
        }
    }

    public class PrepayCurrencyPayViewCommand : lib.ViewModelViewCommand
    {
        internal PrepayCurrencyPayViewCommand(Prepay prepay) : base()
        {
            mymaindbm = new PrepayCurrencyPayDBM();
            mydbm = mymaindbm;
            mymaindbm.Prepay = prepay;
            mymaindbm.Collection = prepay.CurrencyPays;
            mysync = new PrepayCurrencyPaySynchronizer();
            mysync.DomainCollection = prepay.CurrencyPays;
            base.Collection = mysync.ViewModelCollection;
        }

        PrepayCurrencyPayDBM mymaindbm;
        PrepayCurrencyPaySynchronizer mysync;
        internal Prepay Prepay { get { return mymaindbm.Prepay; } }

        protected override void AddData(object parametr)
        {
            PrepayCurrencyPayVM item = new PrepayCurrencyPayVM(new PrepayCurrencyPay(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null, DateTime.Today, 0M, mymaindbm.Prepay));
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
            bool success = base.SaveDataChanges();
            if(success)
            {
                mymaindbm.Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyPaySum));
                if (!mymaindbm.Prepay.CurrencyPaidDate.HasValue & this.Items.Count > 0)
                {
                    AgentCustomerBalanceDBM bdbm = new AgentCustomerBalanceDBM() { Agent = mymaindbm.Prepay.Agent, Customer = mymaindbm.Prepay.Customer, MinBalance = 0M, Importer = mymaindbm.Prepay.Importer };
                    decimal balance = bdbm.GetFirst()?.Balance ?? 0M;
                    if (mymaindbm.Prepay.EuroSum - mymaindbm.Prepay.CurrencyPaySum - balance < 0.0099M)
                    {
                        DateTime maxdate = mymaindbm.Prepay.CurrencyPays.Max((PrepayCurrencyPay item) => { return item.DomainState < lib.DomainObjectState.Deleted ? item.PayDate : DateTime.MinValue; }).Date;
                        if(maxdate != DateTime.MinValue) mymaindbm.Prepay.CurrencyPaidDate = maxdate;
                    }
                }
            }
            return success;
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(PrepayCurrencyPay.PayDate), System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class CurrencyPayViewCommand : lib.ViewModelViewCommand
    {
        internal CurrencyPayViewCommand(Agent agent, Importer importer)
        {
            mymaindbm = new CurrencyPayDBM();
            mydbm = mymaindbm;
            mymaindbm.Agent = agent;
            mymaindbm.Importer = importer;
            mymaindbm.SaveFilter = (PrepayCurrencyPay item) => { return item.Selected; };
            if (agent != null & importer != null)
                mymaindbm.Fill();
            else
                mymaindbm.Collection = new ObservableCollection<PrepayCurrencyPay>();
            mysync = new CurrencyPaySynchronizer();
            mysync.DomainCollection = mymaindbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            mytotal = new CurrencyPayTotal(myview);
            if (agent != null & importer != null)
                mytotal.StartCount();
            if (mymaindbm.Errors.Count > 0)
                this.OpenPopup(mymaindbm.ErrorMessage, true);
        
            myselectall = new RelayCommand(SelectAllExec, SelectAllCanExec);
        }

        CurrencyPayDBM mymaindbm;
        CurrencyPaySynchronizer mysync;
        private CurrencyPayTotal mytotal;
        public CurrencyPayTotal Total { get { return mytotal; } }

        public int AgentId
        {
            set
            {
                mymaindbm.Agent = CustomBrokerWpf.References.AgentStore.GetItemLoad(value,out _);
                mytotal.StopCount(); mymaindbm.Fill(); mytotal.StartCount();
            }
            get { return mymaindbm.Agent?.Id ?? 0; }
        }
        private ListCollectionView myagents;
        public ListCollectionView Agents
        {
            get
            {
                if (myagents == null)
                {
                    myagents = new ListCollectionView(CustomBrokerWpf.References.AgentNames);
                    CustomBrokerWpf.References.AgentNames.RefreshViewAdd(myagents);
                    myagents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myagents;
            }
        }
        internal Importer Importer
        { get { return mymaindbm.Importer; } }

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
            mytotal.StopCount(); mymaindbm.Fill(); mytotal.StartCount();
            if (mymaindbm.Errors.Count > 0) this.PopupText = mymaindbm.ErrorMessage;
        }
        public override bool SaveDataChanges()
        {
            bool success = true;

            foreach (CurrencyPayVM item in mysync.ViewModelCollection)
                if (item.Selected) { item.PayDate = this.Total.PayDate; }
            success = base.SaveDataChanges();
            foreach (CurrencyPayVM item in mysync.ViewModelCollection)
                if (item.Selected && item.DomainState == lib.DomainObjectState.Unchanged && !item.DomainObject.Prepay.CurrencyPays.Contains(item.DomainObject))
                {
                    item.DomainObject.Prepay.CurrencyPays.Add(item.DomainObject);
                    item.DomainObject.Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyPaySum));
                    if (!item.DomainObject.Prepay.CurrencyPaidDate.HasValue)
                    {
                        if (item.DomainObject.Prepay.EuroSum - item.DomainObject.Prepay.CurrencyPaySum - (item.DomainObject as CurrencyPay).Credit < 0.0099M)
                        {
                            DateTime maxdate = item.DomainObject.Prepay.CurrencyPays.Max((PrepayCurrencyPay pay) => { return pay.DomainState < lib.DomainObjectState.Deleted ? pay.PayDate : DateTime.MinValue; }).Date;
                            if (maxdate != DateTime.MinValue) item.DomainObject.Prepay.CurrencyPaidDate = maxdate;
                        }
                    }
                    item.DomainObject.Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyPaidDate));
                }
            return success;
        }
        protected override void SettingView()
        {
            myview.Filter = (object item) => { return true; };
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Prepay.Customer.Name", System.ComponentModel.ListSortDirection.Ascending));
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Prepay.Id", System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class CurrencyPayTotal : lib.TotalValues.TotalViewValues<CurrencyPayVM>
    {
        internal CurrencyPayTotal(ListCollectionView view) : base(view)
        {
            myinitselected = 2; // if not selected - sum=0
            myselectedcount = view.Count + myinitselected; // start with select all
            mypaydate = DateTime.Today;
        }

        private DateTime mypaydate;
        public DateTime PayDate
        {
            set
            {
                if (!DateTime.Equals(mypaydate, value))
                {
                    mypaydate = value;
                }
            }
            get { return mypaydate; }
        }

        private int myitemcount;
        public int ItemCount { set { myitemcount = value; } get { return myitemcount; } }
        private decimal mytotalcost;
        public decimal TotalCost { set { mytotalcost = value; } get { return mytotalcost; } }

        protected override void Item_ValueChangedHandler(CurrencyPayVM sender, ValueChangedEventArgs<object> e)
        {
            decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
            switch (e.PropertyName)
            {
                case nameof(PrepayCurrencyPayVM.CurSum):
                    mytotalcost += newvalue - oldvalue;
                    PropertyChangedNotification("TotalCost");
                    break;
            }
        }
        protected override void ValuesReset()
        {
            myitemcount = 0;
            mytotalcost = 0M;
        }
        protected override void ValuesPlus(CurrencyPayVM item)
        {
            myitemcount++;
            mytotalcost = mytotalcost + (item.DomainObject.CurSum);
        }
        protected override void ValuesMinus(CurrencyPayVM item)
        {
            myitemcount--;
            mytotalcost = mytotalcost - (item.DomainObject.CurSum);
        }
        protected override void PropertiesChangedNotifycation()
        {
            this.PropertyChangedNotification("ItemCount");
            this.PropertyChangedNotification("TotalCost");
        }
    }
}
