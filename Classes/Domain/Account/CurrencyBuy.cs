using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using System;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Linq;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Windows.Input;
using Org.BouncyCastle.Asn1;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class CurrencyBuy : lib.DomainStampValueChanged
    {
        public CurrencyBuy(int id, long stamp, lib.DomainObjectState mstate, DateTime? updated, string updater
            , DateTime buydate, decimal buyrate, decimal cursum
            ) : base(id, stamp, updated, updater, mstate)
        {
            mybuydate = buydate;
            mybuyrate = buyrate;
            mycursum = cursum;
        }

        private DateTime mybuydate;
        public DateTime BuyDate
        { set { SetProperty<DateTime>(ref mybuydate, value); } get { return mybuydate; } }
        private decimal mybuyrate;
        public decimal BuyRate
        { set { SetProperty<decimal>(ref mybuyrate, value); } get { return mybuyrate; } }
        private decimal mycursum;
        public decimal CurSum
        { set { SetPropertyOnValueChanged<decimal>(ref mycursum, value); } get { return mycursum; } }
        internal bool Selected { set; get; }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            CurrencyBuy templ = sample as CurrencyBuy;
            this.BuyDate = templ.BuyDate;
            this.BuyRate = templ.BuyRate;
            this.CurSum = templ.CurSum;
            this.UpdateWhen = templ.UpdateWhen;
            this.UpdateWho = templ.UpdateWho;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.BuyDate):
                    this.BuyDate = (DateTime)value;
                    break;
                case nameof(this.BuyRate):
                    this.BuyRate = (decimal)value;
                    break;
                case nameof(this.CurSum):
                    this.CurSum = (decimal)value;
                    break;
                case nameof(this.UpdateWhen):
                    this.UpdateWhen = (DateTime?)value;
                    break;
                case nameof(this.UpdateWho):
                    this.UpdateWho = (string)value;
                    break;
            }
        }
        internal virtual bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                case nameof(this.CurSum):
                    if ((decimal)value <= 0M)
                    {
                        errmsg = "Сумма должна быть больше ноля!";
                        isvalid = false;
                    }
                    break;
            }
            return isvalid;
        }
    }

    public class CurrencyBuyVM : lib.ViewModelErrorNotifyItem<CurrencyBuy>, lib.Interfaces.ITotalValuesItem
    {
        public CurrencyBuyVM(CurrencyBuy model) : base(model)
        {
            ValidetingProperties.AddRange(new string[] { nameof(this.BuyDate), nameof(this.CurSum) });
            InitProperties();
        }

        private DateTime? mybuydate;
        public DateTime? BuyDate
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(mybuydate, value.Value)))
                {
                    string name = nameof(this.BuyDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.BuyDate);
                    mybuydate = value;
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.BuyDate = value.Value; this.ClearErrorMessageForProperty(name); }
                }
            }
            get { return this.IsEnabled ? mybuydate : (DateTime?)null; }
        }
        public decimal? BuyRate
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.BuyRate, value.Value)))
                {
                    string name = nameof(this.BuyRate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.BuyRate);
                    ChangingDomainProperty = name; this.DomainObject.BuyRate = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.BuyRate : (decimal?)null; }
        }
        private decimal? mycursum;
        public decimal? CurSum
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(mycursum, value.Value)))
                {
                    //decimal oldvalue;
                    string name = nameof(this.CurSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurSum);
                    //oldvalue = mycursum ?? 0M;
                    mycursum = value;
                    if (this.ValidateProperty(name))
                    {
                        //oldvalue = this.DomainObject.CurSum;
                        ChangingDomainProperty = name; this.DomainObject.CurSum = value.Value; this.ClearErrorMessageForProperty(name);
                        //this.OnValueChanged(nameof(this.CurSum), oldvalue, mycursum);
                    }
                }
            }
            get { return mycursum; }
        }
        public DateTime? Updated
        { get { return this.IsEnabled ? this.DomainObject.UpdateWhen : null; } }
        public string Updater
        { get { return this.IsEnabled ? this.DomainObject.UpdateWho : null; } }

        public bool ProcessedIn { set; get; }
        public bool ProcessedOut { set; get; }
        public virtual bool Selected
        {
            set { }
            get { return true; }
        }

        protected override bool DirtyCheckProperty()
        {
            return mybuydate != this.DomainObject.BuyDate || mycursum != this.DomainObject.CurSum;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(this.DomainObject.BuyDate):
                    mybuydate = this.DomainObject.BuyDate;
                    break;
                case nameof(this.DomainObject.CurSum):
                    mycursum = this.DomainObject.CurSum;
                    break;
                case nameof(this.DomainObject.UpdateWhen):
                    PropertyChangedNotification(nameof(PrepayCurrencyBuyVM.Updated));
                    break;
                case nameof(this.DomainObject.UpdateWho):
                    PropertyChangedNotification(nameof(this.Updater));
                    break;
            }
        }
        protected override void InitProperties()
        {
            mybuydate = this.DomainObject.BuyDate;
            mycursum = this.DomainObject.CurSum;
            if(this.DomainObject!=null) this.DomainObject.ValueChanged += this.Model_ValueChanged;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.BuyDate):
                    if (mybuydate != this.DomainObject.BuyDate)
                        mybuydate = this.DomainObject.BuyDate;
                    else
                        this.BuyDate = (DateTime)value;
                    break;
                case nameof(this.BuyRate):
                        this.DomainObject.BuyRate = (decimal)value;
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
                case nameof(this.BuyDate):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mybuydate, out errmsg);
                    break;
                case nameof(this.CurSum):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mycursum, out errmsg);
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

    public class CurrencyBuySynchronizer : lib.ModelViewCollectionsSynchronizer<CurrencyBuy, CurrencyBuyVM>
    {
        protected override CurrencyBuy UnWrap(CurrencyBuyVM wrap)
        {
            return wrap.DomainObject as CurrencyBuy;
        }
        protected override CurrencyBuyVM Wrap(CurrencyBuy fill)
        {
            return new CurrencyBuyVM(fill);
        }
    }


    public class CurrencyBuyJoint : CurrencyBuy
	{
        public CurrencyBuyJoint(int id, long stamp, lib.DomainObjectState mstate
            , DateTime buydate, decimal buyrate, decimal cursum, lib.DomainBaseStamp host
            ) : base(id, stamp, mstate, null, null, buydate, buyrate, cursum)
        {
            myhost = host;
        }

        private lib.DomainBaseStamp myhost;
        public lib.DomainBaseStamp Host
        {
            set { SetProperty<lib.DomainBaseStamp>(ref myhost, value); }
            get { return myhost; }
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Host):
                    this.Host = (lib.DomainBaseStamp)value;
                    break;
                default:
                    base.RejectProperty(property, value);
                    break;
            }
        }
    }

	internal class CurrencyBuyJointDBM : lib.DBMSFill<CurrencyBuyJoint>,lib.IDBManager
	{
		internal CurrencyBuyJointDBM() : base()
		{
            this.NeedAddConnection = true;
            this.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            this.SelectProcedure = true;
            SelectCommandText = "account.CurrencyBuy_sp";
			SelectParams = new SqlParameter[] { new SqlParameter("@importerid", System.Data.SqlDbType.Int) };
            //this.SaveFilter = (CurrencyBuyJoint item) => { return item.Selected; };
            myprdbm = new CurrencyBuyPrepayDBM();
            myfcdbm = new CurrencyBuyInvoiceDBM();
        }

        private CurrencyBuyPrepayDBM myprdbm;
        private CurrencyBuyInvoiceDBM myfcdbm;
        private Importer myimporter;
		internal Importer Importer
		{ set { myimporter = value; } get { return myimporter; } }

		protected override CurrencyBuyJoint CreateItem(SqlDataReader reader, SqlConnection addcon)
		{
			System.Collections.Generic.List<lib.DBMError> errors =new System.Collections.Generic.List<lib.DBMError>();
            lib.DomainBaseStamp host=null;
            switch(reader.GetString(reader.GetOrdinal("class")))
			{
                case "prepay":
                    host = CustomBrokerWpf.References.PrepayStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("invoiceid")), addcon, out errors);
                    break;
                case "final":
                    host = CustomBrokerWpf.References.CustomsInvoiceStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("invoiceid")), addcon, out errors);
                    break;
            }
            this.Errors.AddRange(errors);
            return new CurrencyBuyJoint(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added
				, DateTime.Today, 0M, reader.GetDecimal(reader.GetOrdinal("cursum")), host);
		}
		protected override void CancelLoad()
		{
            myfcdbm.CancelingLoad = this.CancelingLoad;
            myfcdbm.CancelingLoad = this.CancelingLoad;
        }
		protected override void PrepareFill(SqlConnection addcon)
		{
			this.SelectParams[0].Value = myimporter?.Id;
		}

        public bool SaveCollectionChanches()
        {
            using (SqlConnection con = new SqlConnection(myconnectionstring))
			{
                con.Open();
                myprdbm.Command.Connection = con;
                myfcdbm.Command.Connection = con;
                lib.ModelComparer comparer = new lib.ModelComparer();
                foreach (CurrencyBuyJoint item in this.Collection)
				{
                    if (!item.Selected) continue;
                    if (item.Host is Prepay)
                    {
                        CurrencyBuyPrepay prepay = new CurrencyBuyPrepay(item.Id,item.Stamp,item.DomainState,item.UpdateWhen,item.UpdateWho,item.BuyDate,item.BuyRate,item.CurSum, item.Host as Prepay);
                        myprdbm.SaveItemChanches(prepay);
                        if (item.Selected && prepay.DomainState == lib.DomainObjectState.Unchanged && !prepay.Prepay.CurrencyBuys.Contains(prepay, comparer))
                        {
                            item.Id = prepay.Id; item.AcceptChanches();
                            prepay.Prepay.CurrencyBuys.Add(prepay);
                            prepay.Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyBuySum));
                            prepay.Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyBoughtDate));
                        }
                    }
                    else
                    {
                        CurrencyBuyInvoice prepay = new CurrencyBuyInvoice(item.Id, item.Stamp, item.DomainState, item.UpdateWhen, item.UpdateWho, item.BuyDate, item.BuyRate, item.CurSum, item.Host as CustomsInvoice);
                        myfcdbm.SaveItemChanches(prepay);
                        if (item.Selected && prepay.DomainState == lib.DomainObjectState.Unchanged && !prepay.Invoice.CurrencyBuys.Contains(prepay, comparer))
                        {
                            item.Id = prepay.Id; item.AcceptChanches();
                            prepay.Invoice.CurrencyBuys.Add(prepay);
                            prepay.Invoice.PropertyChangedNotification(nameof(Prepay.CurrencyBuySum));
                            prepay.Invoice.PropertyChangedNotification(nameof(Prepay.CurrencyBoughtDate));
                        }
                    }

                }
                con.Close();
            }
            return true;
        }
    }

	public class CurrencyBuyJointVM : CurrencyBuyVM
    {
        public CurrencyBuyJointVM(CurrencyBuyJoint model) : base(model) {}

        private CurrencyBuyJoint mymodel;
        public lib.DomainBaseStamp Host
        { get { return mymodel.Host; } }
        
        public Agent Agent
        { get { return mymodel.Host is Prepay ? (mymodel.Host as Prepay).Agent : (mymodel.Host as CustomsInvoice).RequestCustomer.Request.Agent; } }
        public decimal? CBRate
        { get { return mymodel.Host is Prepay ? (mymodel.Host as Prepay).CBRate : (mymodel.Host as CustomsInvoice).FinalCurCBRate; } }
        public decimal? CBRate2p
        { get { return mymodel.Host is Prepay ? (mymodel.Host as Prepay).CBRatep2p : (mymodel.Host as CustomsInvoice).FinalCurCBRate2p; } }
        public CustomerLegal Customer
        { get { return mymodel.Host is Prepay ? (mymodel.Host as Prepay).Customer : (mymodel.Host as CustomsInvoice).Customer; } }
        public override bool Selected
        {
            set
            {
                if(value && this.DomainState == lib.DomainObjectState.Deleted)
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
            mymodel = this.DomainObject as CurrencyBuyJoint;
        }
    }

    public class CurrencyBuyJointSynchronizer : lib.ModelViewCollectionsSynchronizer<CurrencyBuyJoint, CurrencyBuyJointVM>
    {
        protected override CurrencyBuyJoint UnWrap(CurrencyBuyJointVM wrap)
        {
            return wrap.DomainObject as CurrencyBuyJoint;
        }
        protected override CurrencyBuyJointVM Wrap(CurrencyBuyJoint fill)
        {
            return new CurrencyBuyJointVM(fill);
        }
    }

	public class CurrencyBuyJointViewCommand : lib.ViewModelViewCommand
	{
		internal CurrencyBuyJointViewCommand(Importer importer)
		{
			mymaindbm = new CurrencyBuyJointDBM();
			mydbm = mymaindbm;
			mymaindbm.Importer = importer;
			//mymaindbm.SaveFilter = (CurrencyBuyPrepay item) => { return item.Selected; };
			mymaindbm.Fill();
			mysync = new CurrencyBuyJointSynchronizer();
			mysync.DomainCollection = mymaindbm.Collection;
			base.Collection = mysync.ViewModelCollection;
			mytotal = new CurrencyBuyJointTotal(myview);
			if (mymaindbm.Errors.Count > 0)
				this.OpenPopup(mymaindbm.ErrorMessage, true);

			myselectall = new RelayCommand(SelectAllExec, SelectAllCanExec);
		}

        private CurrencyBuyJointDBM mymaindbm;
        private CurrencyBuyJointSynchronizer mysync;
		private CurrencyBuyJointTotal mytotal;
		public CurrencyBuyJointTotal Total { get { return mytotal; } }
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
			mymaindbm.Fill();
			if (mymaindbm.Errors.Count > 0) this.PopupText = mymaindbm.ErrorMessage;
		}
		public override bool SaveDataChanges()
		{
			bool success = true;

			if (mytotal.BuyRate > 0)
			{
                foreach (CurrencyBuyJointVM item in mysync.ViewModelCollection)
                    if (item.Selected) { item.BuyDate = this.Total.BuyDate; item.BuyRate = this.Total.BuyRate; }
                success = base.SaveDataChanges();
			}
			else if (mysync.DomainCollection.Count > 0)
			{
				success = false;
				this.PopupText = "Не указан курс покупки валюты!";
			}
			return success;
		}
		protected override void SettingView()
		{
			myview.Filter = (object item) => { return true; };
			myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Host.Customer. Name", System.ComponentModel.ListSortDirection.Ascending));
		}
	}

	public class CurrencyBuyJointTotal : lib.TotalCollectionValues<CurrencyBuyJointVM>
    {
        internal CurrencyBuyJointTotal(ListCollectionView view) : base(view)
        {
            myinitselected = 2; // if not selected - sum=0
            myselectedcount = view.Count + myinitselected; // start with select all
            //myselectedall = true;
            mybuydate = DateTime.Today;
        }

        private DateTime mybuydate;
        public DateTime BuyDate
        {
            set
            {
                if (!DateTime.Equals(mybuydate, value))
                {
                    mybuydate = value;
                }
            }
            get { return mybuydate; }
        }
        private decimal mybuyrate;
        public decimal BuyRate
        {
            set { mybuyrate = value; PropertyChangedNotification("TotalCostRUB"); }
            get { return mybuyrate; }
        }

        private int myitemcount;
        public int ItemCount { set { myitemcount = value; } get { return myitemcount; } }
        private decimal mytotalcost;
        public decimal TotalCost { set { mytotalcost = value; } get { return mytotalcost; } }
        public decimal TotalCostRUB { get { return mytotalcost * mybuyrate; } }
        
        protected override void Item_ValueChangedHandler(CurrencyBuyJointVM sender, ValueChangedEventArgs<object> e)
        {
            decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
            switch (e.PropertyName)
            {
                case nameof(CurrencyBuyJointVM.CurSum):
                    mytotalcost += newvalue - oldvalue;
                    PropertyChangedNotification("TotalCost");
                    PropertyChangedNotification("TotalCostRUB");
                    break;
            }
        }
        protected override void ValuesReset()
        {
            myitemcount = 0;
            mytotalcost = 0M;
        }
        protected override void ValuesPlus(CurrencyBuyJointVM item)
        {
            myitemcount++;
            mytotalcost = mytotalcost + (item.DomainObject.CurSum);
        }
        protected override void ValuesMinus(CurrencyBuyJointVM item)
        {
            myitemcount--;
            mytotalcost = mytotalcost - (item.DomainObject.CurSum);
        }
        protected override void PropertiesChangedNotifycation()
        {
            this.PropertyChangedNotification("ItemCount");
            this.PropertyChangedNotification("TotalCost");
            this.PropertyChangedNotification("TotalCostRUB");
        }
    }


    public class CurrencyBuyPrepay : CurrencyBuy
    {
        public CurrencyBuyPrepay(int id, long stamp, lib.DomainObjectState mstate, DateTime? updated, string updater
            , DateTime buydate, decimal buyrate, decimal cursum, Prepay prepay
            ) : base(id, stamp, mstate, updated, updater, buydate, buyrate, cursum)
        {
            myprepay = prepay;
        }

        private Prepay myprepay;
        public Prepay Prepay
        { set { SetProperty<Prepay>(ref myprepay, value); } get { return myprepay; } }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            base.PropertiesUpdate(sample);
            CurrencyBuyPrepay templ = sample as CurrencyBuyPrepay;
            this.Prepay = templ.Prepay;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Prepay):
                    this.Prepay = (Prepay)value;
                    break;
                default:
                    base.RejectProperty(property, value);
                    break;
            }
        }
    //    internal override bool ValidateProperty(string propertyname, object value, out string errmsg)
    //    {
    //        errmsg = null;
    //        bool isvalid = base.ValidateProperty(propertyname, value, out errmsg);
    //        switch (propertyname)
    //        {
				//case nameof(this.BuyDate):
				//	if (this.Prepay.InvoiceDate.HasValue && (DateTime)value < this.Prepay.InvoiceDate.Value)
				//	{
				//		errmsg = "Дата оплаты не может быть меньше даты выставления счета!";
				//		isvalid = false;
				//	}
				//	break;
				//case nameof(this.CurSum):
    //                if (((decimal)value - mycursum) * mybuyrate - this.Prepay.RubPaySum + this.Prepay.CurrencyBuys.Sum((PrepayCurrencyBuy buy) => { return decimal.Multiply(buy.BuyRate, buy.CurSum); }) > 0.99M)
				//	{
				//		errmsg = "Недостаточно средств. Сумма покупки не может быть больше суммы оплаты!";
				//		isvalid = false;
				//	}
				//	break;
    //        }
    //        return isvalid;
    //    }
    }

    internal class CurrencyBuyPrepayDBM : lib.DBManagerWhoWhen<CurrencyBuyPrepay>
    {
        public CurrencyBuyPrepayDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "account.CurrencyBuyPrepay_sp";
            InsertCommandText = "account.CurrencyBuyPrepayAdd_sp";
            UpdateCommandText = "account.CurrencyBuyPrepayUpd_sp";
            DeleteCommandText = "account.CurrencyBuyPrepayDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@prepayid", System.Data.SqlDbType.Int)/*, new SqlParameter("@requestid", System.Data.SqlDbType.Int), new SqlParameter("@parcelid", System.Data.SqlDbType.Int)*/ };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0]
                ,new SqlParameter("@prepayid",System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@cursumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@buydateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@buyrateupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
               myinsertupdateparams[0],myinsertupdateparams[1],myinsertupdateparams[2]
               ,new SqlParameter("@cursum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@buydate",System.Data.SqlDbType.DateTime2)
               ,new SqlParameter("@buyrate",System.Data.SqlDbType.Money)
             };
        }

        private Prepay myprepay;
        internal Prepay Prepay { set { myprepay = value; } get { return myprepay; } }

        protected override CurrencyBuyPrepay CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new CurrencyBuyPrepay(reader.GetInt32(reader.GetOrdinal("id")), reader.GetInt64(reader.GetOrdinal("stamp")), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")), reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
                , reader.GetDateTime(reader.GetOrdinal("buydate")), reader.GetDecimal(reader.GetOrdinal("buyrate")), reader.GetDecimal(reader.GetOrdinal("cursum")), myprepay);
        }
        protected override void GetOutputSpecificParametersValue(CurrencyBuyPrepay item)
        {
        }
        protected override void CancelLoad()
        {
        }
        protected override bool SaveChildObjects(CurrencyBuyPrepay item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(CurrencyBuyPrepay item)
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
        protected override bool SetSpecificParametersValue(CurrencyBuyPrepay item)
        {
            myinsertparams[1].Value = item.Prepay.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue(nameof(CurrencyBuyPrepay.CurSum));
            myupdateparams[2].Value = item.HasPropertyOutdatedValue(nameof(CurrencyBuyPrepay.BuyDate));
            myupdateparams[3].Value = item.HasPropertyOutdatedValue(nameof(CurrencyBuyPrepay.BuyRate));
            myinsertupdateparams[3].Value = item.CurSum;
            myinsertupdateparams[4].Value = item.BuyDate;
            myinsertupdateparams[5].Value = item.BuyRate;
            return true;
        }
    }

    public class PrepayCurrencyBuyVM : CurrencyBuyVM
    {
        public PrepayCurrencyBuyVM(CurrencyBuyPrepay model) : base(model) {}

        private CurrencyBuyPrepay mymodel;
        public Prepay Prepay
        { get { return mymodel.Prepay; } }

        protected override void InitProperties()
        {
            base.InitProperties();
            mymodel = this.DomainObject as CurrencyBuyPrepay;
        }
    }

    public class CurrencyBuyPrepaySynchronizer : lib.ModelViewCollectionsSynchronizer<CurrencyBuyPrepay, PrepayCurrencyBuyVM>
    {
        protected override CurrencyBuyPrepay UnWrap(PrepayCurrencyBuyVM wrap)
        {
            return wrap.DomainObject as CurrencyBuyPrepay;
        }
        protected override PrepayCurrencyBuyVM Wrap(CurrencyBuyPrepay fill)
        {
            return new PrepayCurrencyBuyVM(fill);
        }
    }

    public class PrepayCurrencyBuyViewCommand : lib.ViewModelViewCommand
    {
        internal PrepayCurrencyBuyViewCommand(lib.DomainBaseStamp prepay) : base()
        {
            if (prepay is Prepay)
            {
                myrdbm = new CurrencyBuyPrepayDBM();
                mydbm = myrdbm;
                myrdbm.Prepay = prepay as Prepay;
                myrdbm.Collection = myrdbm.Prepay.CurrencyBuys;
                mypsync = new CurrencyBuyPrepaySynchronizer();
                mypsync.DomainCollection = myrdbm.Prepay.CurrencyBuys;
                base.Collection = mypsync.ViewModelCollection;
            }
            else if (prepay is CustomsInvoice)
            {
                myidbm = new CurrencyBuyInvoiceDBM();
                mydbm = myidbm;
                myidbm.Invoice = prepay as CustomsInvoice;
                myidbm.Collection = myidbm.Invoice.CurrencyBuys;
                myisync = new CurrencyBuyInvoiceSynchronizer();
                myisync.DomainCollection = myidbm.Invoice.CurrencyBuys;
                base.Collection = myisync.ViewModelCollection;
            }
        }

        CurrencyBuyPrepayDBM myrdbm;
        CurrencyBuyInvoiceDBM myidbm;
        CurrencyBuyPrepaySynchronizer mypsync;
        CurrencyBuyInvoiceSynchronizer myisync;
        internal Prepay Prepay { get { return myrdbm?.Prepay; } }
        internal CustomsInvoice Invoice { get { return myidbm?.Invoice; } }

        protected override void AddData(object parametr)
        {
            CurrencyBuyVM item;
            if (this.Prepay == null & this.Invoice == null) return;
            if(this.Prepay!=null)
                item = new PrepayCurrencyBuyVM(new CurrencyBuyPrepay(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null, DateTime.Today, 0M, 0M, myrdbm.Prepay));
            else
                item = new CurrencyBuyInvoiceVM(new CurrencyBuyInvoice(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null, DateTime.Today, 0M, 0M, myidbm.Invoice));
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
            if (this.Prepay != null)
            {
                myrdbm.Fill();
                if (myrdbm.Errors.Count > 0) this.PopupText = myrdbm.ErrorMessage;
            }
            else if (this.Invoice != null)
            {
                myidbm.Fill();
                if (myidbm.Errors.Count > 0) this.PopupText = myidbm.ErrorMessage;
            }
        }
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(CurrencyBuyPrepay.BuyDate), System.ComponentModel.ListSortDirection.Ascending));
        }
        public override bool SaveDataChanges()
        {
            bool sucess = true;
            if (this.Prepay != null || this.Invoice != null)
            {
                sucess = base.SaveDataChanges();
                if (this.Prepay != null)
                {
                    Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyBuySum));
                    Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyBoughtDate));
                }
                else if (this.Invoice != null)
                {
                    Invoice.PropertyChangedNotification(nameof(Invoice.CurrencyBuySum));
                    Invoice.PropertyChangedNotification(nameof(Invoice.CurrencyBoughtDate));
                }
            }
            return sucess;
        }
    }


    public class CurrencyBuyInvoice : CurrencyBuy
    {
        public CurrencyBuyInvoice(int id, long stamp, lib.DomainObjectState mstate, DateTime? updated, string updater
            , DateTime buydate, decimal buyrate, decimal cursum, CustomsInvoice invoice
            ) : base(id, stamp, mstate, updated, updater, buydate, buyrate, cursum)
        {
            myinvoice = invoice;
        }

        private CustomsInvoice myinvoice;
        public CustomsInvoice Invoice
        { set { SetProperty<CustomsInvoice>(ref myinvoice, value); } get { return myinvoice; } }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            base.PropertiesUpdate(sample);
            CurrencyBuyInvoice templ = sample as CurrencyBuyInvoice;
            this.Invoice = templ.Invoice;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Invoice):
                    this.Invoice = (CustomsInvoice)value;
                    break;
                default:
                    base.RejectProperty(property, value);
                    break;
            }
        }
    }

    internal class CurrencyBuyInvoiceDBM : lib.DBManagerWhoWhen<CurrencyBuyInvoice>
    {
        public CurrencyBuyInvoiceDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "account.CurrencyBuyInvoice_sp";
            InsertCommandText = "account.CurrencyBuyInvoiceAdd_sp";
            UpdateCommandText = "account.CurrencyBuyInvoiceUpd_sp";
            DeleteCommandText = "account.CurrencyBuyInvoiceDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@invoice", System.Data.SqlDbType.Int) };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0]
                ,new SqlParameter("@invoice",System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@cursumupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@buydateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@buyrateupd", System.Data.SqlDbType.Bit)
           };
            myinsertupdateparams = new SqlParameter[]
            {
               myinsertupdateparams[0],myinsertupdateparams[1],myinsertupdateparams[2]
               ,new SqlParameter("@cursum",System.Data.SqlDbType.Money)
               ,new SqlParameter("@buydate",System.Data.SqlDbType.DateTime2)
               ,new SqlParameter("@buyrate",System.Data.SqlDbType.Money)
             };
        }

        private CustomsInvoice myinvoice;
        internal CustomsInvoice Invoice { set { myinvoice = value; } get { return myinvoice; } }

        protected override CurrencyBuyInvoice CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            return new CurrencyBuyInvoice(reader.GetInt32(reader.GetOrdinal("id")), reader.GetInt64(reader.GetOrdinal("stamp")), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")), reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
                , reader.GetDateTime(reader.GetOrdinal("buydate")), reader.GetDecimal(reader.GetOrdinal("buyrate")), reader.GetDecimal(reader.GetOrdinal("cursum")), myinvoice);
        }
        protected override void GetOutputSpecificParametersValue(CurrencyBuyInvoice item)
        {
        }
        protected override void CancelLoad()
        {
        }
        protected override bool SaveChildObjects(CurrencyBuyInvoice item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(CurrencyBuyInvoice item)
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
        protected override bool SetSpecificParametersValue(CurrencyBuyInvoice item)
        {
            myinsertparams[1].Value = item.Invoice.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue(nameof(CurrencyBuy.CurSum));
            myupdateparams[2].Value = item.HasPropertyOutdatedValue(nameof(CurrencyBuy.BuyDate));
            myupdateparams[3].Value = item.HasPropertyOutdatedValue(nameof(CurrencyBuy.BuyRate));
            myinsertupdateparams[3].Value = item.CurSum;
            myinsertupdateparams[4].Value = item.BuyDate;
            myinsertupdateparams[5].Value = item.BuyRate;
            return true;
        }
    }

    public class CurrencyBuyInvoiceVM : CurrencyBuyVM
    {
        public CurrencyBuyInvoiceVM(CurrencyBuyInvoice model) : base(model) {}

        private CurrencyBuyInvoice mymodel;
        public CustomsInvoice Invoice
        { get { return this.mymodel.Invoice; } }

        protected override void InitProperties()
        {
            base.InitProperties();
            mymodel = this.DomainObject as CurrencyBuyInvoice;
        }
    }

    public class CurrencyBuyInvoiceSynchronizer : lib.ModelViewCollectionsSynchronizer<CurrencyBuyInvoice, CurrencyBuyInvoiceVM>
    {
        protected override CurrencyBuyInvoice UnWrap(CurrencyBuyInvoiceVM wrap)
        {
            return wrap.DomainObject as CurrencyBuyInvoice;
        }
        protected override CurrencyBuyInvoiceVM Wrap(CurrencyBuyInvoice fill)
        {
            return new CurrencyBuyInvoiceVM(fill);
        }
    }
}
