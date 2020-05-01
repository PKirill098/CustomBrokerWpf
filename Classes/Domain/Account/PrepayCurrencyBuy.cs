using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using System;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Linq;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class PrepayCurrencyBuy : lib.DomainBaseStamp
    {
        public PrepayCurrencyBuy(int id, long stamp, lib.DomainObjectState mstate, DateTime? updated, string updater
            , DateTime buydate, decimal buyrate, decimal cursum, Prepay prepay
            ) : base(id, stamp, updated, updater, mstate)
        {
            mybuydate = buydate;
            mybuyrate = buyrate;
            mycursum = cursum;
            myprepay = prepay;
        }
        internal int oblprepayid { set; get; }

        private DateTime mybuydate;
        public DateTime BuyDate
        { set { SetProperty<DateTime>(ref mybuydate, value); } get { return mybuydate; } }
        private decimal mybuyrate;
        public decimal BuyRate
        { set { SetProperty<decimal>(ref mybuyrate, value); } get { return mybuyrate; } }
        private decimal mycursum;
        public decimal CurSum
        { set { SetProperty<decimal>(ref mycursum, value); } get { return mycursum; } }
        internal bool Selected { set; get; }
        private Prepay myprepay;
        public Prepay Prepay
        { set { SetProperty<Prepay>(ref myprepay, value); } get { return myprepay; } }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            PrepayCurrencyBuy templ = sample as PrepayCurrencyBuy;
            this.BuyDate = templ.BuyDate;
            this.BuyRate = templ.BuyRate;
            this.CurSum = templ.CurSum;
            this.Prepay = templ.Prepay;
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
        internal bool ValidateProperty(string propertyname, object value, out string errmsg)
        {
            bool isvalid = true;
            errmsg = null;
            switch (propertyname)
            {
                //case nameof(this.BuyDate):
                //    if (this.Prepay.InvoiceDate.HasValue && (DateTime)value < this.Prepay.InvoiceDate.Value)
                //    {
                //        errmsg = "Дата оплаты не может быть меньше даты выставления счета!";
                //        isvalid = false;
                //    }
                //    break;
                case nameof(this.CurSum):
                    if ((decimal)value <= 0M)
                    {
                        errmsg = "Сумма должна быть больше ноля!";
                        isvalid = false;
                    }
                    //else if (((decimal)value - mycursum) * mybuyrate - this.Prepay.RubPaySum + this.Prepay.CurrencyBuys.Sum((PrepayCurrencyBuy buy) => { return decimal.Multiply(buy.BuyRate, buy.CurSum); }) > 0.99M)
                    //{
                    //    errmsg = "Недостаточно средств. Сумма покупки не может быть больше суммы оплаты!";
                    //    isvalid = false;
                    //}
                    break;
            }
            return isvalid;
        }
    }

    internal class PrepayCurrencyBuyDBM : lib.DBManagerWhoWhen<PrepayCurrencyBuy>
    {
        public PrepayCurrencyBuyDBM()
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

        protected override PrepayCurrencyBuy CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new PrepayCurrencyBuy(reader.GetInt32(reader.GetOrdinal("id")), reader.GetInt64(reader.GetOrdinal("stamp")), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")), reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
                , reader.GetDateTime(reader.GetOrdinal("buydate")), reader.GetDecimal(reader.GetOrdinal("buyrate")), reader.GetDecimal(reader.GetOrdinal("cursum")), myprepay);
        }
        protected override void GetOutputSpecificParametersValue(PrepayCurrencyBuy item)
        {
        }
        protected override void LoadObjects(PrepayCurrencyBuy item)
        {
        }
        protected override bool LoadObjects()
        {
            return true;
        }
        protected override bool SaveChildObjects(PrepayCurrencyBuy item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(PrepayCurrencyBuy item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue()
        {
            this.SelectParams[0].Value = myprepay?.Id;
        }
        protected override bool SetSpecificParametersValue(PrepayCurrencyBuy item)
        {
            myinsertparams[1].Value = item.Prepay.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue(nameof(PrepayCurrencyBuy.CurSum));
            myupdateparams[2].Value = item.HasPropertyOutdatedValue(nameof(PrepayCurrencyBuy.BuyDate));
            myupdateparams[3].Value = item.HasPropertyOutdatedValue(nameof(PrepayCurrencyBuy.BuyRate));
            myinsertupdateparams[3].Value = item.CurSum;
            myinsertupdateparams[4].Value = item.BuyDate;
            myinsertupdateparams[5].Value = item.BuyRate;
            return true;
        }
    }

    internal class CurrencyBuyDBM:PrepayCurrencyBuyDBM
    {
        internal CurrencyBuyDBM():base()
        {
            SelectCommandText = "account.CurrencyBuy_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@importerid", System.Data.SqlDbType.Int) };
            this.SaveFilter = (PrepayCurrencyBuy item) => { return item.Selected; };
        }

        private Importer myimporter;
        internal Importer Importer
        { set { myimporter = value; } get { return myimporter; } }

        protected override void SetSelectParametersValue()
        {
            this.SelectParams[0].Value = myimporter?.Id;
        }
        protected override PrepayCurrencyBuy CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new PrepayCurrencyBuy(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added
                , null, null
                , DateTime.Today, 0M, reader.GetDecimal(reader.GetOrdinal("cursum")),null)
            {oblprepayid=reader.GetInt32(reader.GetOrdinal("prepayid")) };
        }
        protected override void LoadObjects(PrepayCurrencyBuy item)
        {
            item.Prepay= CustomBrokerWpf.References.PrepayStore.GetItemLoad(item.oblprepayid, this.Command.Connection);
            if (CustomBrokerWpf.References.PrepayStore.Errors.Count > 0)
                foreach (lib.DBMError err in CustomBrokerWpf.References.PrepayStore.Errors)
                    this.Errors.Add(err);
        }
        protected override bool LoadObjects()
        {
            foreach(PrepayCurrencyBuy item in this.Collection)
                LoadObjects(item);
            return this.Errors.Count == 0;
        }
    }

    public class PrepayCurrencyBuyVM : lib.ViewModelErrorNotifyItem<PrepayCurrencyBuy>, lib.Interfaces.ITotalValuesItem
    {
        public PrepayCurrencyBuyVM(PrepayCurrencyBuy model) : base(model)
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
                    decimal oldvalue;
                    string name = nameof(this.CurSum);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CurSum);
                    oldvalue = mycursum??0M;
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
    }

    public class CurrencyBuyVM: PrepayCurrencyBuyVM
    {
        public CurrencyBuyVM(PrepayCurrencyBuy model) : base(model)
        {
        }

        public new bool Selected
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
        }
    }

    public class PrepayCurrencyBuySynchronizer : lib.ModelViewCollectionsSynchronizer<PrepayCurrencyBuy, PrepayCurrencyBuyVM>
    {
        protected override PrepayCurrencyBuy UnWrap(PrepayCurrencyBuyVM wrap)
        {
            return wrap.DomainObject as PrepayCurrencyBuy;
        }
        protected override PrepayCurrencyBuyVM Wrap(PrepayCurrencyBuy fill)
        {
            return new PrepayCurrencyBuyVM(fill);
        }
    }

    public class CurrencyBuySynchronizer : lib.ModelViewCollectionsSynchronizer<PrepayCurrencyBuy, CurrencyBuyVM>
    {
        protected override PrepayCurrencyBuy UnWrap(CurrencyBuyVM wrap)
        {
            return wrap.DomainObject as PrepayCurrencyBuy;
        }
        protected override CurrencyBuyVM Wrap(PrepayCurrencyBuy fill)
        {
            return new CurrencyBuyVM(fill);
        }
    }

    public class PrepayCurrencyBuyViewCommand : lib.ViewModelViewCommand
    {
        internal PrepayCurrencyBuyViewCommand(Prepay prepay) : base()
        {
            myrdbm = new PrepayCurrencyBuyDBM();
            mydbm = myrdbm;
            myrdbm.Prepay = prepay;
            myrdbm.Collection = myrdbm.Prepay.CurrencyBuys;
            mysync = new PrepayCurrencyBuySynchronizer();
            mysync.DomainCollection = myrdbm.Prepay.CurrencyBuys;
            base.Collection = mysync.ViewModelCollection;
        }

        PrepayCurrencyBuyDBM myrdbm;
        PrepayCurrencyBuySynchronizer mysync;
        internal Prepay Prepay { get { return myrdbm.Prepay; } }

        protected override void AddData(object parametr)
        {
            PrepayCurrencyBuyVM item = new PrepayCurrencyBuyVM(new PrepayCurrencyBuy(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added,null,null, DateTime.Today,0M,0M, myrdbm.Prepay));
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
        protected override void SettingView()
        {
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(PrepayCurrencyBuy.BuyDate), System.ComponentModel.ListSortDirection.Ascending));
        }
        public override bool SaveDataChanges()
        {
            bool sucess = base.SaveDataChanges();
            Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyBuySum));
            Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyBoughtDate));
            return sucess;
        }
    }

    public class CurrencyBuyViewCommand : lib.ViewModelViewCommand
    {
        internal CurrencyBuyViewCommand(Importer importer)
        {
            myrdbm = new CurrencyBuyDBM();
            mydbm = myrdbm;
            myrdbm.Importer = importer;
            myrdbm.SaveFilter = (PrepayCurrencyBuy item) => { return item.Selected; };
            myrdbm.Fill();
            mysync = new CurrencyBuySynchronizer();
            mysync.DomainCollection = myrdbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            mytotal = new CurrencyBuyTotal(myview);
            if (myrdbm.Errors.Count > 0)
                this.OpenPopup(myrdbm.ErrorMessage, true);
        }

        CurrencyBuyDBM myrdbm;
        CurrencyBuySynchronizer mysync;
        private CurrencyBuyTotal mytotal;
        public CurrencyBuyTotal Total { get { return mytotal; } }
        internal Importer Importer
        { get { return myrdbm.Importer; } }

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
            myrdbm.Fill();
            if (myrdbm.Errors.Count > 0) this.PopupText = myrdbm.ErrorMessage;
        }
        public override bool SaveDataChanges()
        {
            bool success=true;
            
            if (mytotal.BuyRate > 0)
            {
                foreach (CurrencyBuyVM item in mysync.ViewModelCollection)
                    if (item.Selected) { item.BuyDate = this.Total.BuyDate; item.BuyRate = this.Total.BuyRate; }
                success = base.SaveDataChanges();
                foreach (CurrencyBuyVM item in mysync.ViewModelCollection)
                    if (item.Selected && item.DomainState == lib.DomainObjectState.Unchanged && !item.DomainObject.Prepay.CurrencyBuys.Contains(item.DomainObject))
                    {
                        item.DomainObject.Prepay.CurrencyBuys.Add(item.DomainObject);
                        item.DomainObject.Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyBuySum));
                        item.DomainObject.Prepay.PropertyChangedNotification(nameof(Prepay.CurrencyBoughtDate));
                    }
            }
            else if(mysync.DomainCollection.Count>0)
            {
                success = false;
                this.PopupText = "Не указан курс покупки валюты!";
            }
            return success;
        }
        protected override void SettingView()
        {
            myview.Filter = (object item) => { return true; };
            myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Prepay.Customer. Name",System.ComponentModel.ListSortDirection.Ascending));
        }
    }

    public class CurrencyBuyTotal : lib.TotalCollectionValues<CurrencyBuyVM>
    {
        internal CurrencyBuyTotal(ListCollectionView view) : base(view)
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
        
        protected override void Item_ValueChangedHandler(CurrencyBuyVM sender, ValueChangedEventArgs<object> e)
        {
            decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
            switch (e.PropertyName)
            {
                case nameof(PrepayCurrencyBuyVM.CurSum):
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
        protected override void ValuesPlus(CurrencyBuyVM item)
        {
            myitemcount++;
            mytotalcost = mytotalcost + (item.DomainObject.CurSum);
        }
        protected override void ValuesMinus(CurrencyBuyVM item)
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
}
