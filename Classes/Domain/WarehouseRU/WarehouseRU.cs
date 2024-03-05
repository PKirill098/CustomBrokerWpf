using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class WarehouseRU : lib.DomainBaseStamp
    {
        public WarehouseRU(int id, long stamp, DateTime? updatewhen, string updatewho, lib.DomainObjectState state
            , CustomerLegal legal, string note, DateTime? receipted, DateTime? shipped, lib.ReferenceSimpleItem status
            ) : base(id, stamp, updatewhen, updatewho, state, true)
        {
            mylegal = legal;
            mynote = note;
            myreceipted = receipted;
            myshipped = shipped;
            mystatus = status;

            mylegals = App.Current.Dispatcher.Invoke<ObservableCollection<RequestCustomerLegal>>(() =>
            { return new ObservableCollection<RequestCustomerLegal>(); });
        }

        private CustomerLegal mylegal;
        public CustomerLegal Legal
        { set { SetProperty(ref mylegal, value); } get { return mylegal; } }
        private string mynote;
        public string Note
        { set { SetProperty(ref mynote, value); } get { return mynote; } }
        private DateTime? myreceipted;
        public DateTime? Receipted
        { set { SetProperty(ref myreceipted, value); } get { return myreceipted; } }
        private DateTime? myshipped;
        public DateTime? Shipped
        {
            set
            {
                SetProperty(ref myshipped, value, () =>
        {
            if (value == null && mystatus.Id == 120)
                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 104);
            else if (value != null && mystatus.Id == 104)
                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 120);
        });
            }
            get { return myshipped; }
        }
        private lib.ReferenceSimpleItem mystatus;
        public lib.ReferenceSimpleItem Status
        {
            set { this.SetProperty(ref mystatus, value, () => { StatusUpdated(); }); }
            get { return mystatus; }
        }

        private decimal? myactualweight;
        public decimal? ActualWeight
        { get { return myactualweight; } }
        private Agent myagent;
        public Agent Agent
        { get { return myagent; } }
        private string mybrandnames;
        public string BrandNames
        { get { return mybrandnames; } }
        private string mycargo;
        public string Cargo
        { get { return mycargo; } }
        private int? mycellnumber;
        public int? CellNumber
        { get { return mycellnumber; } }
        private string mydeliveryaddress;
        public string DeliveryAddress
        { get { return mydeliveryaddress; } }
        private Importer myimporter;
        public Importer Importer
        { get { return myimporter; } }
        private string mymanagers;
        public string Managers
        { get { return mymanagers; } }
        private string mymanagernotes;
        public string ManagerNotes
        { get { return mymanagernotes; } }
        private decimal? myofficialweight;
        public decimal? OfficialWeight
        { get { return myofficialweight; } }
        private Parcel myparcel;
        public Parcel Parcel
        { get { return myparcel; } }
        private string myrequestsid;
        public string RequestsId
        { get { return myrequestsid; } }
        private string myservicetype;
        public string ServiceType
        { get { return myservicetype; } }
        private string mystorageid;
        public string StorageId
        { get { return mystorageid; } }
        private decimal? myvolume;
        public decimal? Volume
        { get { return myvolume; } }

        private ObservableCollection<RequestCustomerLegal> mylegals;
        internal ObservableCollection<RequestCustomerLegal> CustomerLegals
        {
            get
            {
                return mylegals;
            }
        }
        internal bool CustomerLegalsIsNull
        { get { return mylegals == null; } }

        internal void CustomerLegalsPropertyUpdate()
        {
            myactualweight = mylegals?.Sum((RequestCustomerLegal item) => { return item.Request.ActualWeight; });
            myagent = mylegals?.FirstOrDefault()?.Request.Agent;
            StringBuilder rids = new StringBuilder();
            foreach (RequestCustomerLegal item in mylegals)
            {
                if (item.Request.BrandsIsNull && string.IsNullOrEmpty(item.Request.BrandNames)) // запрос загрузки брендов
                    item.Request.PropertyChanged += Request_PropertyChanged;
                else
                {
                    rids.Append(item.Request.BrandNames);
                    rids.Append(", ");
                }
            }
            mybrandnames = rids.ToString().TrimEnd(new char[] { ',', ' ' });
            rids.Clear();
            List<lib.ReferenceSimpleItem> cargolist = new List<ReferenceSimpleItem>();
            foreach (RequestCustomerLegal item in mylegals)
            {
                foreach(RequestCargo cargo in item.Request.CargoList)
                    if(!cargolist.Any((lib.ReferenceSimpleItem check) => { return check.Id == cargo.Id; }))
                        cargolist.Add(cargo.InnerObject); 
            }
            foreach(lib.ReferenceSimpleItem cargo in cargolist.OrderBy((lib.ReferenceSimpleItem item) => { return item.Name; }))
                rids.Append(cargo.Name); rids.Append(", ");
            mycargo = rids.ToString().TrimEnd(new char[] { ',', ' ' });
            mycellnumber = mylegals?.Sum((RequestCustomerLegal item) => { return item.Request.CellNumber; });
            mydeliveryaddress = mylegal.Addresses.FirstOrDefault((Address adr) => { return adr.AddressTypeID == 4; })?.FullAddress;
            myimporter = mylegals?.FirstOrDefault()?.Request.Importer;
            mymanagers = mylegals?.Select(l=>l.Request.Manager?.Name).Where(n=>!string.IsNullOrEmpty(n)).Distinct().Aggregate(string.Empty,(ms, m) => string.IsNullOrEmpty(ms) ? m : ms + ", " + m);
            myofficialweight = mylegals?.Sum((RequestCustomerLegal item) => { return item.Request.OfficialWeight; });
            myparcel = mylegals?.FirstOrDefault()?.Request.Parcel;
            rids.Clear();
            foreach (RequestCustomerLegal item in mylegals?.OrderBy((RequestCustomerLegal item) => { return item.Request.Id; }))
            { rids.Append(item.Request.Id); rids.Append(", "); }
            myrequestsid = rids.ToString().TrimEnd(new char[] { ',', ' ' });
            myservicetype = mylegals?.FirstOrDefault()?.Request.ServiceType;
            mystorageid = (mylegals?.FirstOrDefault()?.Request.ParcelGroup == null ? mylegals?.FirstOrDefault()?.Request.StorePoint : mylegals?.FirstOrDefault()?.Request.ParcelGroup?.ToString());
            myvolume = mylegals?.Sum((RequestCustomerLegal item) => { return item.Request.Volume; });
            rids.Clear();
            int[] groups=new int[50];
            foreach (RequestCustomerLegal item in mylegals?.OrderBy((RequestCustomerLegal item) => { return item.Request.Id; }))
            {
                if (!(item.Request.ParcelGroup.HasValue && groups.Contains(item.Request.ParcelGroup.Value))) // примечания повторяются для всех заявок группы
                {
                    if (!string.IsNullOrEmpty(item.Request.ManagerNote)) { rids.Append(item.Request.ManagerNote); rids.Append("; "); }
                    if (!string.IsNullOrEmpty(item.Request.StoreNote)) { rids.Append(item.Request.StoreNote); rids.Append("; "); }
                    if (item.Request.ParcelGroup.HasValue) groups[groups.Count<int>((int n) => { return n > 0; })]=item.Request.ParcelGroup.Value;
                }
                if (!string.IsNullOrEmpty(item.Request.CustomerNote)) { rids.Append(item.Request.CustomerNote); rids.Append("; "); }
                if (!string.IsNullOrEmpty(item.Request.MSKStoreNote)) { rids.Append(item.Request.MSKStoreNote); rids.Append("; "); }
           }
            mymanagernotes = rids.ToString().TrimEnd(new char[] { ';', ' ' });

            this.PropertyChangedNotification(nameof(this.ActualWeight));
            this.PropertyChangedNotification(nameof(this.Agent));
            this.PropertyChangedNotification(nameof(this.BrandNames));
            this.PropertyChangedNotification(nameof(this.Cargo));
            this.PropertyChangedNotification(nameof(this.CellNumber));
            this.PropertyChangedNotification(nameof(this.DeliveryAddress));
            this.PropertyChangedNotification(nameof(this.Importer));
            this.PropertyChangedNotification(nameof(this.ManagerNotes));
            this.PropertyChangedNotification(nameof(this.OfficialWeight));
            this.PropertyChangedNotification(nameof(this.RequestsId));
            this.PropertyChangedNotification(nameof(this.ServiceType));
            this.PropertyChangedNotification(nameof(this.StorageId));
            this.PropertyChangedNotification(nameof(this.Volume));
        }
        private void Request_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Request.BrandNames))
            {
                StringBuilder rids = new StringBuilder();
                foreach (RequestCustomerLegal item in mylegals)
                {
                    if (!item.Request.BrandsIsNull)
                    {
                        item.Request.PropertyChanged -= Request_PropertyChanged;
                        rids.Append(item.Request.BrandNames);
                        rids.Append(", ");
                    }
                }
                mybrandnames = rids.ToString().TrimEnd(new char[] { ',', ' ' });
                this.PropertyChangedNotification(nameof(this.BrandNames));
            }
        }

        protected override void PropertiesUpdate(DomainBaseUpdate sample)
        {
            WarehouseRU temp = sample as WarehouseRU;
            this.Legal = temp.Legal;
            this.Note = temp.Note;
            this.Receipted = temp.Receipted;
            this.Shipped = temp.Shipped;
            this.Status = temp.Status;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Legal):
                    mylegal = (CustomerLegal)value;
                    break;
                case nameof(this.Note):
                    mynote = (string)value;
                    break;
                case nameof(this.Receipted):
                    myreceipted = (DateTime?)value;
                    break;
                case nameof(this.Shipped):
                    myshipped = (DateTime?)value;
                    break;
                case nameof(this.Status):
                    mystatus = (lib.ReferenceSimpleItem)value;
                    break;
            }
        }

        private void StatusUpdated()
        {
            if (mystatus.Id == 120 && myshipped == null)
                this.Shipped = DateTime.Now;
            else if (mystatus.Id == 104 && myshipped != null)
                this.Shipped = null;
            //foreach (RequestCustomerLegal legal in this.CustomerLegals)  -- если не сохранять Request блокируется
            //    legal.Request.Status = mystatus;
        }
    }

    public class WarehouseRUDBM : lib.DBManagerStamp<SqlDataReader,WarehouseRU>
    {
        public WarehouseRUDBM()
        {
            this.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            base.NeedAddConnection = true;

            SelectCommandText = "dbo.WarehouseRu_sp";
            InsertCommandText = "dbo.WarehouseRuAdd_sp";
            UpdateCommandText = "dbo.WarehouseRuUpd_sp";
            DeleteCommandText = "dbo.WarehouseRuDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@id", System.Data.SqlDbType.Int),
                new SqlParameter("@rlegal", System.Data.SqlDbType.Int),
                new SqlParameter("@filter", System.Data.SqlDbType.Int){ Value = 0},
            };
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0],myinsertparams[1]
                ,new SqlParameter("@costomer", System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@noteupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@receiptedupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@shippedupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@statusupd", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                 new SqlParameter("@note", System.Data.SqlDbType.NVarChar,300)
                ,new SqlParameter("@receipted", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@shipped", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@status", System.Data.SqlDbType.Int)
            };

            myrdbm = new RequestCustomerLegalDBM();
        }

        private RequestCustomerLegalDBM myrdbm;
        private RequestCustomerLegal mylegal;
        internal RequestCustomerLegal Legal
        { set { mylegal = value; } get { return mylegal; } }
        private lib.SQLFilter.SQLFilter myfilter;
        public lib.SQLFilter.SQLFilter Filter
        { set { myfilter = value; } get { return myfilter; } }

        //protected override void CancelLoad()
        //{
        //    myrdbm.CancelingLoad = this.CancelingLoad;
        //}
		protected override SqlDataReader CreateRecord(SqlDataReader reader)
		{
			return reader;
		}
        protected override WarehouseRU CreateModel(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
            List<lib.DBMError> errors = new List<DBMError>();
            CustomerLegal legal = CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(reader.GetInt32(this.Fields["costomer"]), addcon, out errors);
            this.Errors.AddRange(errors);
            WarehouseRU sku = new WarehouseRU(reader.GetInt32(this.Fields["id"]), reader.GetInt64(this.Fields["stamp"])
                , reader.GetDateTime(this.Fields["updated"]), reader.GetString(this.Fields["updater"]), lib.DomainObjectState.Unchanged
                , legal
                , reader.IsDBNull(this.Fields["note"]) ? null : reader.GetString(this.Fields["note"])
                , reader.IsDBNull(this.Fields["receipted"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["receipted"])
                , reader.IsDBNull(this.Fields["shipped"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["shipped"])
                , CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", reader.GetInt32(this.Fields["status"]))
                );

            myrdbm.Errors.Clear();
            myrdbm.SKU = sku; // коллекцию передаем в свойстве
            myrdbm.Connection = addcon;
            myrdbm.Fill();
            this.Errors.AddRange(myrdbm.Errors);
            sku.CustomerLegalsPropertyUpdate();
            return sku;
        }
		protected override void GetRecord(SqlDataReader reader, SqlConnection addcon, CancellationToken canceltasktoken = default)
		{
			base.ModelFirst = this.CreateModel(reader, addcon, canceltasktoken);
		}
		protected override WarehouseRU GetModel(SqlConnection addcon, CancellationToken canceltasktoken)
		{
			return base.ModelFirst;
		}
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader), addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            foreach (SqlParameter par in this.SelectParams)
                switch (par.ParameterName)
                {
                    case "@filter":
                        par.Value = myfilter?.FilterWhereId;
                        break;
                    case "@rlegal":
                        par.Value = mylegal?.Id;
                        break;
                }
        }
        protected override bool SetParametersValue(WarehouseRU item)
        {
            base.SetParametersValue(item);
            foreach (SqlParameter par in this.InsertParams)
                switch (par.ParameterName)
                {
                    case "@costomer":
                        par.Value = item.Legal.Id;
                        break;
                }
            foreach (SqlParameter par in this.UpdateParams)
                switch (par.ParameterName)
                {
                    case "@noteupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(WarehouseRU.Note));
                        break;
                    case "@receiptedupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(WarehouseRU.Receipted));
                        break;
                    case "@shippedupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(WarehouseRU.Shipped));
                        break;
                    case "@statusupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(WarehouseRU.Status));
                        break;
                }
            foreach (SqlParameter par in this.InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@note":
                        par.Value = item.Note;
                        break;
                    case "@receipted":
                        par.Value = item.Receipted;
                        break;
                    case "@shipped":
                        par.Value = item.Shipped;
                        break;
                    case "@status":
                        par.Value = item.Status.Id;
                        break;
                }

            return true;
        }
    }

    public class WarehouseRUVM : lib.ViewModelErrorNotifyItem<WarehouseRU>, lib.Interfaces.ITotalValuesItem
    {
        public WarehouseRUVM(WarehouseRU model) : base(model)
        {
            InitProperties();
        }

        public CustomerLegal Legal
        {
            set { SetProperty(this.DomainObject.Legal, (CustomerLegal legal) => { this.DomainObject.Legal = legal; }, value); }
            get { return GetProperty(this.DomainObject.Legal, null); }
        }
        public string Note
        {
            set { SetProperty(this.DomainObject.Note, (string note) => { this.DomainObject.Note = note; }, value); }
            get { return GetProperty(this.DomainObject.Note, null); }
        }
        public DateTime? Receipted
        {
            set { SetProperty(this.DomainObject.Receipted, (DateTime? date) => { this.DomainObject.Receipted = date; }, value); }
            get { return GetProperty(this.DomainObject.Receipted, null); }
        }
        public DateTime? Shipped
        {
            set { SetProperty(this.DomainObject.Shipped, (DateTime? date) => { this.DomainObject.Shipped = date; }, value); }
            get { return GetProperty(this.DomainObject.Shipped, null); }
        }
        public lib.ReferenceSimpleItem Status
        {
            set { this.SetProperty(this.DomainObject.Status, (lib.ReferenceSimpleItem newval) => { this.DomainObject.Status = newval; }, value); }
            get { return GetProperty(this.DomainObject.Status, null); }
        }

        public decimal? ActualWeight
        { get { return GetProperty(this.DomainObject.ActualWeight, null); } }
        public Agent Agent
        { get { return GetProperty(this.DomainObject.Agent, null); } }
        public string BrandNames
        { get { return GetProperty(this.DomainObject.BrandNames, null); } }
        public string Cargo
        { get { return GetProperty(this.DomainObject.Cargo, null); } }
        public int? CellNumber
        { get { return GetProperty(this.DomainObject.CellNumber, null); } }
        public string DeliveryAddress
        { get { return GetProperty(this.DomainObject.DeliveryAddress, null); } }
        public Importer Importer
        { get { return GetProperty(this.DomainObject.Importer, null); } }
        public string Managers
        { get { return GetProperty(this.DomainObject.Managers, null); } }
        public string ManagerNotes
        { get { return GetProperty(this.DomainObject.ManagerNotes, null); } }
        public decimal? OfficialWeight
        { get { return GetProperty(this.DomainObject.OfficialWeight, null); } }
        public Parcel Parcel
        { get { return GetProperty(this.DomainObject.Parcel, null); } }
        public string RequestsId
        { get { return GetProperty(this.DomainObject.RequestsId, null); } }
        public string ServiceType
        { get { return GetProperty(this.DomainObject.ServiceType, null); } }
        public string StorageId
        { get { return GetProperty(this.DomainObject.StorageId, null); } }
        public decimal? Volume
        { get { return GetProperty(this.DomainObject.Volume, null); } }

        private ListCollectionView mylegals;
        internal ListCollectionView CustomerLegals
        {
            get
            {
                return mylegals;
            }
        }

        public bool ProcessedIn { get; set; }
        public bool ProcessedOut { get; set; }
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

        public override bool IsReadOnly
        { get { return base.IsReadOnly || !(this.Status.Id == 104 || this.Status.Id == 107 || this.Status.Id == 120); } }

        protected override bool DirtyCheckProperty()
        {
            return false;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch(property)
            {
                case nameof(WarehouseRUVM.Status):
                    this.PropertyChangedNotification(nameof(WarehouseRUVM.IsReadOnly));
                    break;
            }
        }
        protected override void InitProperties()
        {
            //if (this.DomainObject.CustomerLegalsIsNull)
            //{
            //    mylegals = new ListCollectionView(this.DomainObject.CustomerLegals);
            //    mylegals.Filter = ViewModelViewCommand.ViewFilterDefault;
            //}
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Legal):
                    this.DomainObject.Legal = (CustomerLegal)value;
                    break;
                case nameof(this.Note):
                    this.DomainObject.Note = (string)value;
                    break;
                case nameof(this.Receipted):
                    this.DomainObject.Receipted = (DateTime?)value;
                    break;
                case nameof(this.Shipped):
                    this.DomainObject.Shipped = (DateTime?)value;
                    break;
                case nameof(this.Status):
                    this.DomainObject.Status = (lib.ReferenceSimpleItem)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            return true;
        }
    }

    public class WarehouseRUSynchronizer : lib.ModelViewCollectionsSynchronizer<WarehouseRU, WarehouseRUVM>
    {
        protected override WarehouseRU UnWrap(WarehouseRUVM wrap)
        {
            return wrap.DomainObject as WarehouseRU;
        }
        protected override WarehouseRUVM Wrap(WarehouseRU fill)
        {
            return new WarehouseRUVM(fill);
        }
    }

    public class WarehouseRUViewCommader : lib.ViewModelViewCommand,IDisposable
    {
        internal WarehouseRUViewCommader()
        {
            myfilter = new lib.SQLFilter.SQLFilter("sku", "AND", CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            mywdbm = new WarehouseRUDBM();
            mydbm = mywdbm;
            mywdbm.Collection = new ObservableCollection<WarehouseRU>();
            mywdbm.Filter = myfilter;
            mywdbm.FillAsyncCompleted = () =>
            {
                if (mywdbm.Errors.Count > 0)
                    OpenPopup(mywdbm.ErrorMessage, true);
                mytotal.StartCount();
            };
            mywdbm.FillAsync();
            mysync = new WarehouseRUSynchronizer();
            mysync.DomainCollection = mywdbm.Collection;
            base.Collection = mysync.ViewModelCollection;

            mystatuses = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
            mystatuses.Filter = (object item) => { lib.ReferenceSimpleItem status = item as lib.ReferenceSimpleItem; return status.Id == 104 || status.Id == 120; };
            mystatuses.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            myfilterstatuses = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
            myfilterstatuses.Filter = (object item) => { lib.ReferenceSimpleItem status = item as lib.ReferenceSimpleItem; return status.Id >= 60 && status.Id != 110; };
            myfilterstatuses.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));

            base.DeleteQuestionHeader = "Удалить позицию со склада?";
            #region Filter
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            myfilterdefault = new RelayCommand(FilterDefaultExec, FilterDefaultCanExec);
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfiltersave = new RelayCommand(FilterSaveExec, FilterSaveCanExec);

            myactualweightfilter = new libui.NumberFilterVM();
            myactualweightfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myactualweightfilter.ExecCommand2 = () => { myactualweightfilter.Clear(); };
            myagentfilter = new WarehouseRUAgentCheckListBoxVMFillDefault();
            myagentfilter.DeferredFill = true;
            myagentfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            myagentfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myagentfilter.ExecCommand2 = () => { myagentfilter.Clear(); };
            myagentfilter.FillDefault = () =>
            {
                bool empty = this.Items.Count == 0 && this.FilterEmpty;
                if (empty)
                    foreach (lib.ReferenceSimpleItem item in CustomBrokerWpf.References.AgentNames)
                        myagentfilter.Items.Add(item);
                return empty;
            };
            myagentfilter.ItemsSource = myview.OfType<WarehouseRUVM>();
            mybrandfilter = new WarehouseRUBrandCheckListBoxVMFillDefault();
            mybrandfilter.DeferredFill = true;
            mybrandfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            mybrandfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mybrandfilter.ExecCommand2 = () => { mybrandfilter.Clear(); };
            mybrandfilter.FillDefault = () =>
            {
                bool empty = this.Items.Count == 0 && this.FilterEmpty;
                if (empty)
                    foreach (Brand item in mybrandfilter.DefaultList)
                        mybrandfilter.Items.Add(item);
                return empty;
            };
            mybrandfilter.ItemsSource = myview.OfType<WarehouseRUVM>();
            mycargofilter = new libui.CheckListBoxVM();
            mycargofilter.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            mycargofilter.Items = CustomBrokerWpf.References.GoodsTypesParcel;
            mycargofilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycargofilter.ExecCommand2 = () => { mycargofilter.Clear(); };
            mycellnumberfilter = new libui.NumberFilterVM();
            mycellnumberfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycellnumberfilter.ExecCommand2 = () => { mycellnumberfilter.Clear(); };
            mycustomerfilter = new WarehouseRUCustomerCheckListBoxVMFillDefault();
            mycustomerfilter.DeferredFill = true;
            mycustomerfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            mycustomerfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycustomerfilter.ExecCommand2 = () => { mycustomerfilter.Clear(); };
            mycustomerfilter.FillDefault = () =>
            {
                bool empty = this.Items.Count == 0 && this.FilterEmpty;
                if (empty)
                    foreach (CustomerLegal item in mycustomerfilter.DefaultList)
                        mycustomerfilter.Items.Add(item);
                return empty;
            };
            mycustomerfilter.ItemsSource = myview.OfType<WarehouseRUVM>();
            mydeliveryaddressfilter = new WarehouseRUDeliveryAddressCheckListBoxVMFill();
            mydeliveryaddressfilter.DeferredFill = true;
            mydeliveryaddressfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mydeliveryaddressfilter.ExecCommand2 = () => { mydeliveryaddressfilter.Clear(); };
            mydeliveryaddressfilter.ItemsSource = myview.OfType<WarehouseRUVM>();
            mydeliverytypefilter = new libui.CheckListBoxVM();
            mydeliverytypefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mydeliverytypefilter.ExecCommand2 = () => { mydeliverytypefilter.Clear(); };
            mydeliverytypefilter.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            mydeliverytypefilter.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            mydeliverytypefilter.Items = CustomBrokerWpf.References.DeliveryTypes;
            myimporterfilter = new libui.CheckListBoxVM();
            myimporterfilter.SearchPath = "Name";
            myimporterfilter.DisplayPath = "Name";
            myimporterfilter.GetDisplayPropertyValueFunc = (item) => { return ((Importer)item).Name; };
            myimporterfilter.Items = CustomBrokerWpf.References.Importers;
            myimporterfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myimporterfilter.ExecCommand2 = () => { myimporterfilter.Clear(); };
            mynotefilter = new WarehouseRUNoteCheckListBoxVMFill();
            mynotefilter.DeferredFill = true;
            mynotefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mynotefilter.ExecCommand2 = () => { mynotefilter.Clear(); };
            mynotefilter.ItemsSource = myview.OfType<WarehouseRUVM>();
            myofficialweightfilter = new libui.NumberFilterVM();
            myofficialweightfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myofficialweightfilter.ExecCommand2 = () => { myofficialweightfilter.Clear(); };
            myparcelfilter = new WarehouseRUParcelCheckListBoxVMFillDefault();
            myparcelfilter.DeferredFill = true;
            myparcelfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myparcelfilter.ExecCommand2 = () => { myparcelfilter.Clear(); };
            myparcelfilter.FillDefault = () =>
            {
                bool fempty = this.Items.Count == 0 && this.FilterEmpty;
                if (fempty)
                {
                    ParcelNumber empty = new ParcelNumber() { Sort = "999999" };
                    myparcelfilter.Items.Add(empty);
                    foreach (ParcelNumber item in CustomBrokerWpf.References.ParcelNumbers)
                        myparcelfilter.Items.Add(item);
                }
                return fempty;
            };
            myparcelfilter.ItemsSource = myview.OfType<WarehouseRUVM>();
            myreceiptedfilter = new libui.DateFilterVM();
            //myreceiptedfilter.IsNull = false;
            myreceiptedfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myreceiptedfilter.ExecCommand2 = () => { myreceiptedfilter.Clear(); };
            myrequestidfilter = new WarehouseRURequestCheckListBoxVMFill();
            myrequestidfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myrequestidfilter.ExecCommand2 = () => { myrequestidfilter.Clear(); };
            myrequestidfilter.ItemsSource = myview.OfType<WarehouseRUVM>();
            myservicetypefilter = new libui.CheckListBoxVM();
            myservicetypefilter.SearchPath = "Name";
            myservicetypefilter.Items = CustomBrokerWpf.References.ServiceTypes;
            myservicetypefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myservicetypefilter.ExecCommand2 = () => { myservicetypefilter.Clear(); };
            myshippedfilter = new libui.DateFilterVM();
            myshippedfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myshippedfilter.ExecCommand2 = () => { myshippedfilter.Clear(); };
            mystatusfilter = new libui.CheckListBoxVM();
            mystatusfilter.DisplayPath = "Name";
            mystatusfilter.SearchPath = "Name";
            mystatusfilter.GetDisplayPropertyValueFunc = (item) => { return ((lib.ReferenceSimpleItem)item).Name; };
            mystatusfilter.ItemsView = this.FilterStatuses;
            mystatusfilter.SelectedItems = new List<object>();
            mystatusfilter.SelectedItems.Add(CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 104));
            mystatusfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mystatusfilter.ExecCommand2 = () => { mystatusfilter.Clear(); };
            mystorenumfilter = new WarehouseRUStoreNumCheckListBoxVMFill();
            mystorenumfilter.DeferredFill = true;
            mystorenumfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mystorenumfilter.ExecCommand2 = () => { mystorenumfilter.Clear(); };
            mystorenumfilter.ItemsSource = myview.OfType<WarehouseRUVM>();
            myvolumefilter = new libui.NumberFilterVM();
            myvolumefilter.ExecCommand1 = () => { FilterRunExec(null); };
            myvolumefilter.ExecCommand2 = () => { myvolumefilter.Clear(); };

            this.FilterFill();

            if (myfilter.isEmpty)
                this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
            #endregion
        }
        ~WarehouseRUViewCommader()
        { Dispose(); }

        private WarehouseRUDBM mywdbm;
        private WarehouseRUSynchronizer mysync;
        private bool myisreadonly;
        public bool IsReadOnly
        {
            set { myisreadonly = value; }
            get { return myisreadonly; }
        }
        public System.Windows.Visibility VisibilityEdit
        { get { return myisreadonly ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }
        private ListCollectionView mystatuses;
        public ListCollectionView Statuses
        {
            get { return mystatuses; }
        }
        private ListCollectionView myfilterstatuses;
        public ListCollectionView FilterStatuses
        {
            get { return myfilterstatuses; }
        }

        private WarehouseRUTotal mytotal;
        public WarehouseRUTotal Total { get { return mytotal; } }

        #region Filter
        private lib.SQLFilter.SQLFilter myfilter;
        public lib.SQLFilter.SQLFilter Filter
        {
            get { return myfilter; }
        }

        private WarehouseRUAgentCheckListBoxVMFillDefault myagentfilter;
        public WarehouseRUAgentCheckListBoxVMFillDefault AgentFilter
        {
            get { return myagentfilter; }
        }
        private WarehouseRUBrandCheckListBoxVMFillDefault mybrandfilter;
        public WarehouseRUBrandCheckListBoxVMFillDefault BrandFilter
        { get { return mybrandfilter; } }
        private WarehouseRUCustomerCheckListBoxVMFillDefault mycustomerfilter;
        public WarehouseRUCustomerCheckListBoxVMFillDefault CustomerFilter
        {
            get { return mycustomerfilter; }
        }
        private WarehouseRUNoteCheckListBoxVMFill mynotefilter;
        public WarehouseRUNoteCheckListBoxVMFill NoteFilter
        { get { return mynotefilter; } }
        private WarehouseRUParcelCheckListBoxVMFillDefault myparcelfilter;
        public WarehouseRUParcelCheckListBoxVMFillDefault ParcelFilter
        {
            get { return myparcelfilter; }
        }
        private libui.CheckListBoxVM mystatusfilter;
        public libui.CheckListBoxVM StatusFilter
        {
            get { return mystatusfilter; }
        }
        private libui.DateFilterVM myreceiptedfilter;
        public libui.DateFilterVM ReceiptedFilter
        { get { return myreceiptedfilter; } }
        private libui.DateFilterVM myshippedfilter;
        public libui.DateFilterVM ShippedFilter
        { get { return myshippedfilter; } }
        private WarehouseRURequestCheckListBoxVMFill myrequestidfilter;
        public WarehouseRURequestCheckListBoxVMFill RequestIdFilter
        { get { return myrequestidfilter; } }
        private WarehouseRUStoreNumCheckListBoxVMFill mystorenumfilter;
        public WarehouseRUStoreNumCheckListBoxVMFill StoreNumFilter
        { get { return mystorenumfilter; } }
        private libui.CheckListBoxVM myimporterfilter;
        public libui.CheckListBoxVM ImporterFilter
        { get { return myimporterfilter; } }
        private libui.NumberFilterVM myofficialweightfilter;
        public libui.NumberFilterVM OfficialWeightFilter
        { get { return myofficialweightfilter; } }
        private libui.NumberFilterVM myactualweightfilter;
        public libui.NumberFilterVM ActualWeightFilter
        { get { return myactualweightfilter; } }
        private libui.NumberFilterVM myvolumefilter;
        public libui.NumberFilterVM VolumeFilter
        { get { return myvolumefilter; } }
        private libui.NumberFilterVM mycellnumberfilter;
        public libui.NumberFilterVM CellNumberFilter
        { get { return mycellnumberfilter; } }
        private libui.CheckListBoxVM myservicetypefilter;
        public libui.CheckListBoxVM ServiceTypeFilter
        { get { return myservicetypefilter; } }
        private libui.CheckListBoxVM mycargofilter;
        public libui.CheckListBoxVM CargoFilter
        { get { return mycargofilter; } }
        private libui.CheckListBoxVM mydeliverytypefilter;
        public libui.CheckListBoxVM DeliveryTypeFilter
        { get { return mydeliverytypefilter; } }
        private WarehouseRUDeliveryAddressCheckListBoxVMFill mydeliveryaddressfilter;
        public WarehouseRUDeliveryAddressCheckListBoxVMFill DeliveryAddressFilter
        { get { return mydeliveryaddressfilter; } }

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            RunFilter(null);
        }
        private bool FilterRunCanExec(object parametr)
        { return true; }

        private RelayCommand myfilterclear;
        public ICommand FilterClear
        {
            get { return myfilterclear; }
        }
        private void FilterClearExec(object parametr)
        {
            myagentfilter.Clear();
            myagentfilter.IconVisibileChangedNotification();
            mybrandfilter.Clear();
            mybrandfilter.IconVisibileChangedNotification();
            mycustomerfilter.Clear();
            mycustomerfilter.IconVisibileChangedNotification();
            mynotefilter.Clear();
            mynotefilter.IconVisibileChangedNotification();
            myparcelfilter.Clear();
            myparcelfilter.IconVisibileChangedNotification();
            mystatusfilter.Clear();
            mystatusfilter.IconVisibileChangedNotification();
            myreceiptedfilter.Clear();
            myreceiptedfilter.IconVisibileChangedNotification();
            myshippedfilter.Clear();
            myshippedfilter.IconVisibileChangedNotification();
            myrequestidfilter.Clear();
            myrequestidfilter.IconVisibileChangedNotification();
            mystorenumfilter.Clear();
            mystorenumfilter.IconVisibileChangedNotification();
            myimporterfilter.Clear();
            myimporterfilter.IconVisibileChangedNotification();
            myofficialweightfilter.Clear();
            myofficialweightfilter.IconVisibileChangedNotification();
            myactualweightfilter.Clear();
            myactualweightfilter.IconVisibileChangedNotification();
            myvolumefilter.Clear();
            myvolumefilter.IconVisibileChangedNotification();
            mycellnumberfilter.Clear();
            mycellnumberfilter.IconVisibileChangedNotification();
            myservicetypefilter.Clear();
            myservicetypefilter.IconVisibileChangedNotification();
            mycargofilter.Clear();
            mycargofilter.IconVisibileChangedNotification();
            mydeliverytypefilter.Clear();
            mydeliverytypefilter.IconVisibileChangedNotification();
            mydeliveryaddressfilter.Clear();
            mydeliveryaddressfilter.IconVisibileChangedNotification();
            this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }
        
        private RelayCommand myfilterdefault;
        public ICommand FilterDefault
        {
            get { return myfilterdefault; }
        }
        private void FilterDefaultExec(object parametr)
        {
            this.Save.Execute(null);
            if (!LastSaveResult)
                this.OpenPopup("Применение фильтра\nПрименение фильтра невозможно. Не удалось сохранить изменения. \n Сохраните или отмените изменения, затем примените фильтр.", true);
            else
            {
                myfilter.RemoveCurrentWhere();
                myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
                FilterFill();
                this.Refresh.Execute(null);
            }
        }
        private bool FilterDefaultCanExec(object parametr)
        { return true; }

        private RelayCommand myfiltersave;
        public ICommand FilterSave
        {
            get { return myfiltersave; }
        }
        private void FilterSaveExec(object parametr)
        {
            if (System.Windows.MessageBox.Show("Фильтр по умолчанию будет заменён текущим фильтром.\nПродолжить?", "Сохранение фильтра", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) != System.Windows.MessageBoxResult.No)
            {
                try
                {
                    this.UpdateFilter();
                    myfilter.SetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
                }
                catch (Exception ex)
                {
                    if (ex is System.Data.SqlClient.SqlException)
                    {
                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                        if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение фильтра", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Сохранение фильтра", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение фильтра", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private bool FilterSaveCanExec(object parametr)
        { return !this.FilterEmpty; }

        private bool FilterEmpty
        {
            get
            {
                return !(
                    myagentfilter.FilterOn
                    || mybrandfilter.FilterOn
                    || mycustomerfilter.FilterOn
                    || mynotefilter.FilterOn
                    || myparcelfilter.FilterOn
                    || mystatusfilter.FilterOn
                    || myreceiptedfilter.FilterOn
                    || myshippedfilter.FilterOn
                    || myrequestidfilter.FilterOn
                    || mystorenumfilter.FilterOn
                    || myimporterfilter.FilterOn
                    || myofficialweightfilter.FilterOn
                    || myactualweightfilter.FilterOn
                    || myvolumefilter.FilterOn
                    || mycellnumberfilter.FilterOn
                    || myservicetypefilter.FilterOn
                    || mycargofilter.FilterOn
                    || mydeliverytypefilter.FilterOn
                    || mydeliveryaddressfilter.FilterOn
                );
            }
        }

        private void UpdateFilter()
        {
            if (myagentfilter.FilterOn)
            {
                string[] items = new string[myagentfilter.SelectedItems.Count];
                for (int i = 0; i < myagentfilter.SelectedItems.Count; i++)
                    items[i] = (myagentfilter.SelectedItems[i] as lib.ReferenceSimpleItem).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "agent", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "agent", new string[0]);
            if (mybrandfilter.FilterOn)
            {
                string[] items = new string[mybrandfilter.SelectedItems.Count];
                for (int i = 0; i < mybrandfilter.SelectedItems.Count; i++)
                    items[i] = (mybrandfilter.SelectedItems[i] as Brand).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "brand", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "brand", new string[0]);
            if (mycustomerfilter.FilterOn)
            {
                string[] items = new string[mycustomerfilter.SelectedItems.Count];
                for (int i = 0; i < mycustomerfilter.SelectedItems.Count; i++)
                    items[i] = (mycustomerfilter.SelectedItems[i] as CustomerLegal).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "customer", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "customer", new string[0]);
            if (myimporterfilter.FilterOn)
            {
                string[] items = new string[myimporterfilter.SelectedItems.Count];
                for (int i = 0; i < myimporterfilter.SelectedItems.Count; i++)
                    items[i] = (myimporterfilter.SelectedItems[i] as Importer).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "importer", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "importer", new string[0]);
            if (mynotefilter.FilterOn)
            {
                if (mynotefilter.SelectedItems.Count > 0)
                {
                    string[] items = new string[mynotefilter.SelectedItems.Count];
                    for (int i = 0; i < mynotefilter.SelectedItems.Count; i++)
                        items[i] = (string)mynotefilter.SelectedItems[i];
                    myfilter.SetList(myfilter.FilterWhereId, "note", items);
                }
                else
                    myfilter.SetString(myfilter.FilterWhereId, "note", mynotefilter.ItemsViewFilter);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "note", new string[0]);
            if (myparcelfilter.FilterOn)
            {
                string[] items = new string[myparcelfilter.SelectedItems.Count];
                for (int i = 0; i < myparcelfilter.SelectedItems.Count; i++)
                    items[i] = (myparcelfilter.SelectedItems[i] as ParcelNumber).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "parcel", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "parcel", new string[0]);
            if (myrequestidfilter.FilterOn)
            {
                string[] items = new string[myrequestidfilter.SelectedItems.Count];
                for (int i = 0; i < myrequestidfilter.SelectedItems.Count; i++)
                    items[i] = (myrequestidfilter.SelectedItems[i] as Request).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "request", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "request", new string[0]);
            if (mystatusfilter.FilterOn)
            {
                string[] items = new string[mystatusfilter.SelectedItems.Count];
                for (int i = 0; i < mystatusfilter.SelectedItems.Count; i++)
                    items[i] = (mystatusfilter.SelectedItems[i] as lib.ReferenceSimpleItem).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "status", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "status", new string[0]);
            if (mystorenumfilter.FilterOn)
            {
                string[] items = new string[mystorenumfilter.SelectedItems.Count];
                for (int i = 0; i < mystorenumfilter.SelectedItems.Count; i++)
                    items[i] = (string)mystorenumfilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "storenum", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "storenum", new string[0]);
            myfilter.SetDate(myfilter.FilterWhereId, "receipted", "receipted", myreceiptedfilter.DateStart, myreceiptedfilter.DateStop, myreceiptedfilter.IsNull);
            myfilter.SetDate(myfilter.FilterWhereId, "shipped", "shipped", myshippedfilter.DateStart, myshippedfilter.DateStop, myshippedfilter.IsNull);

        }
        public void RunFilter(lib.Filter.FilterItem[] filters)
        {
            this.Save.Execute(null);
            if (!LastSaveResult)
                this.OpenPopup("Применение фильтра\nПрименение фильтра невозможно. Не удалось сохранить изменения. \n Сохраните или отмените изменения, затем примените фильтр.", true);
            else
            {
                this.Refresh.Execute(null);
            }
        }
        private void FilterFill()
        {
            //mystatusfilter.SelectedItems.Clear();
            //mystatusfilter.SelectedItems.Add();
            //myfilter.PullListBox(myfilter.FilterWhereId, "agent", "Id", myagentfilter., true);
            bool isnull;
            DateTime? date1, date2;
            myfilter.PullDate(myfilter.FilterWhereId, "receipted", "receipted", out date1, out date2, out isnull);
            myreceiptedfilter.IsNull = isnull;
            myreceiptedfilter.DateStart = date1;
            myreceiptedfilter.DateStop = date2;
            myreceiptedfilter.IconVisibileChangedNotification();
            myfilter.PullDate(myfilter.FilterWhereId, "shipped", "shipped", out date1, out date2, out isnull);
            myshippedfilter.IsNull = isnull;
            myshippedfilter.DateStart = date1;
            myshippedfilter.DateStop = date2;
            myshippedfilter.IconVisibileChangedNotification();
        }
        private string myfilterbuttonimagepath;
        public string FilterButtonImagePath
        { get { return myfilterbuttonimagepath; } }
        public string IsFiltered
        { get { return myfilter.isEmpty ? string.Empty : "Фильтр!"; } }
        #endregion

        private lib.TaskAsync.TaskAsync myexceltask;
		private libui.ExcelHelper myexcelexport;
		public ICommand ExcelExport
		{
			get
            {
                if (myexcelexport == null)
                {
                    myexcelexport = new libui.ExcelHelper();
                    myexcelexport.SheetName = "Склад Москва";
                    myexcelexport.ColumnFormatDefault = new libui.ExcelFormat(string.Empty,"@");
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.Status)+nameof(WarehouseRUVM.Status.Id), "@", libui.ExcelHorizontalAlignment.Center));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(Parcel.ParcelNumberOrder), "@", libui.ExcelHorizontalAlignment.Center));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.RequestsId), "@", libui.ExcelHorizontalAlignment.Center));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.StorageId), "@", libui.ExcelHorizontalAlignment.Center));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.Importer) + nameof(Importer.Name), "@", libui.ExcelHorizontalAlignment.Center));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.Managers), "@", libui.ExcelHorizontalAlignment.Center));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.OfficialWeight), @"# ##0,00", libui.ExcelHorizontalAlignment.None));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.ActualWeight), @"# ##0,00", libui.ExcelHorizontalAlignment.None));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.Volume), @"# ##0,00", libui.ExcelHorizontalAlignment.None));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.CellNumber), @"# ##0", libui.ExcelHorizontalAlignment.Center));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.Receipted), @"date", libui.ExcelHorizontalAlignment.Center));
                    myexcelexport.ColumnsFormat.Add(new libui.ExcelFormat(nameof(WarehouseRUVM.Shipped), @"date", libui.ExcelHorizontalAlignment.Center));
                    myexcelexport.GetCellValue = GetExcelCellValue;
                    myexcelexport.TransformData = (System.Collections.IEnumerable coll)=> { return coll.OfType<WarehouseRUVM>(); };
                    myexcelexport.CanExecute = ExcelExportCanExec;
                }
                return myexcelexport.Export;
            }
		}
        private object GetExcelCellValue(string property,object row)
        {
            object value = null;
            WarehouseRUVM item = row as WarehouseRUVM;
            switch (property)
            {
                case nameof(WarehouseRUVM.Status)+nameof(WarehouseRUVM.Status.Id):
                    value = item.Status.Name;
                    break;
                case nameof(WarehouseRUVM.Receipted):
                    value = item.Receipted;
                    break;
                case nameof(WarehouseRUVM.Shipped):
                    value = item.Shipped;
                    break;
                case nameof(Parcel.ParcelNumberOrder):
                    value = item.Parcel?.ParcelNumber;
                    break;
                case nameof(WarehouseRUVM.RequestsId):
                    value = item.RequestsId;
                    break;
                case nameof(WarehouseRUVM.StorageId):
                    value = item.StorageId;
                    break;
                case nameof(WarehouseRUVM.Legal)+nameof(CustomerLegal.Name):
                    value = item.Legal.Name;
                    break;
                case nameof(CustomerLegal.Customer)+nameof(Customer.Name):
                    value = item.Legal.Customer.Name;
                    break;
                case nameof(WarehouseRUVM.Agent)+nameof(Agent.Name):
                    value = item.Agent?.Name;
                    break;
                case nameof(WarehouseRUVM.BrandNames):
                    value = item.BrandNames;
                    break;
                case nameof(WarehouseRUVM.Importer) + nameof(Importer.Name):
                    value = item.Importer?.Name;
                    break;
                case nameof(WarehouseRUVM.Managers):
                    value = item.Managers;
                    break;
                case nameof(WarehouseRUVM.ManagerNotes):
                    value = item.ManagerNotes;
                    break;
                case nameof(WarehouseRUVM.OfficialWeight):
                    value = item.OfficialWeight;
                    break;
                case nameof(WarehouseRUVM.ActualWeight):
                    value = item.ActualWeight;
                    break;
                case nameof(WarehouseRUVM.Volume):
                    value = item.Volume;
                    break;
                case nameof(WarehouseRUVM.CellNumber):
                    value = item.CellNumber;
                    break;
                case nameof(WarehouseRUVM.ServiceType):
                    value = item.ServiceType;
                    break;
                case nameof(item.Cargo):
                    value = item.Cargo;
                    break;
                case nameof(CustomerLegal.DeliveryType_)+nameof(CustomerLegal.DeliveryType_.Name):
                    value = item.Legal.DeliveryType_?.Name;
                    break;
                case nameof(item.DeliveryAddress):
                    value = item.DeliveryAddress;
                    break;
                case nameof(item.Note):
                    value = item.Note;
                    break;
            }
            return value;
        }
		private bool ExcelExportCanExec(object parametr)
		{ return !(myview == null || myview.IsAddingNew | myview.IsEditingItem); }


        protected override void OtherViewRefresh()
        {
        }
        protected override void RefreshData(object parametr)
        {
            mytotal.StopCount();
            UpdateFilter();
            mywdbm.Errors.Clear();
            mywdbm.FillAsync();
        }
        protected override void SettingView()
        {
            mytotal = new WarehouseRUTotal(myview);
            this.PropertyChangedNotification(nameof(Total));
        }
        protected override bool CanAddData(object parametr)
        {
            return false;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return myview.CurrentItem is WarehouseRUVM && (myview.CurrentItem as WarehouseRUVM).DomainObject.CustomerLegals?.Count == 0;
        }

        public void Dispose()
        {
            myfilter.RemoveFilter();
        }
    }

    public class WarehouseRUAgentCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<WarehouseRUVM, lib.ReferenceSimpleItem>
    {
        internal WarehouseRUAgentCheckListBoxVMFillDefault() : base()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return ((lib.ReferenceSimpleItem)item).Name; };
        }

        protected override void AddItem(WarehouseRUVM item)
        {
            lib.ReferenceSimpleItem name = CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", item.Agent?.Id??0);
            if (!Items.Contains(name)) Items.Add(name);
        }
    }
    public class WarehouseRUBrandCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<WarehouseRUVM, Brand>
    {
        internal WarehouseRUBrandCheckListBoxVMFillDefault() : base()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return ((Brand)item).Name; };
            // запустим загрузку списка по умолчанию
            // из за долгой загрузки
            BrandDBM bdbm;
            bdbm = App.Current.Dispatcher.Invoke<BrandDBM>(() => { mydefaultlist = new ObservableCollection<Brand>(); return new BrandDBM(); });
            bdbm.Collection = mydefaultlist;
            bdbm.FillAsync();
        }

        private ObservableCollection<Brand> mydefaultlist;
        internal ObservableCollection<Brand> DefaultList
        {
            get
            {
                return mydefaultlist;
            }
        }

        protected override void AddItem(WarehouseRUVM item)
        {
            if(item.CustomerLegals!=null)
                foreach (RequestCustomerLegal legal in item.CustomerLegals)
                    if(legal.Request?.Brands!=null)
                        foreach (RequestBrand brand in legal.Request?.Brands)
                            if (!Items.Contains(brand.Brand?.Brand)) Items.Add(brand.Brand?.Brand);
        }
    }
    public class WarehouseRUCustomerCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<WarehouseRUVM, CustomerLegal>
    {
        internal WarehouseRUCustomerCheckListBoxVMFillDefault() : base()
        {
            this.DisplayPath = "Name";
            this.SearchPath = "Name";
            this.GetDisplayPropertyValueFunc = (item) => { return ((CustomerLegal)item).Name; };
        }

        private List<CustomerLegal> mydefaultlist;
        internal List<CustomerLegal> DefaultList
        {
            get
            {
                if (mydefaultlist == null)
                {
                    mydefaultlist = new List<CustomerLegal>(); // из за долгой загрузки
                    CustomerLegalDBM dbm = new CustomerLegalDBM();
                    dbm.Fill();
                    mydefaultlist = dbm.Collection.ToList<CustomerLegal>();
                }
                return mydefaultlist;
            }
        }

        protected override void AddItem(WarehouseRUVM item)
        {
            if (!Items.Contains(item.Legal)) Items.Add(item.Legal);
        }
    }
    public class WarehouseRUDeliveryAddressCheckListBoxVMFill : libui.CheckListBoxVMFill<WarehouseRUVM, string>
    {
        protected override void AddItem(WarehouseRUVM item)
        {
            if (Items.Count == 0)
                Items.Add(string.Empty);
            if (!(string.IsNullOrEmpty(item.DeliveryAddress) || Items.Contains(item.DeliveryAddress))) Items.Add(item.DeliveryAddress);
        }
    }
    public class WarehouseRUNoteCheckListBoxVMFill : libui.CheckListBoxVMFill<WarehouseRUVM, string>
    {
        protected override void AddItem(WarehouseRUVM item)
        {
            if (Items.Count == 0)
                Items.Add(string.Empty);
            if (!(string.IsNullOrEmpty(item.Note) || Items.Contains(item.Note))) Items.Add(item.Note);
        }
    }
    public class WarehouseRUParcelCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<WarehouseRUVM, ParcelNumber>
    {
        internal WarehouseRUParcelCheckListBoxVMFillDefault() : base()
        {
            this.DisplayPath = "FullNumber";
            this.SearchPath = "Sort";
            this.SortDescriptions.Add(new System.ComponentModel.SortDescription("Sort", System.ComponentModel.ListSortDirection.Descending));
            this.GetDisplayPropertyValueFunc = (item) => { return ((ParcelNumber)item).FullNumber; };
        }

        protected override void AddItem(WarehouseRUVM item)
        {
            ParcelNumber name;
            if (Items.Count == 0)
            { name = new ParcelNumber() { Sort = "999999" }; Items.Add(name); }
            if (item.Parcel?.Id > 0)
            {
                name = CustomBrokerWpf.References.ParcelNumbers.FindFirstItem("Id", item.Parcel.Id);
                if (!Items.Contains(name)) Items.Add(name);
            }
        }
    }
    public class WarehouseRURequestCheckListBoxVMFill : libui.CheckListBoxVMFill<WarehouseRUVM, Request>
    {
        public WarehouseRURequestCheckListBoxVMFill() : base()
        {
            this.DeferredFill = true;
            this.DisplayPath = "Id";
            this.SearchPath = "Id";
            this.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Descending));
            this.GetDisplayPropertyValueFunc = (item) => { return ((Request)item).Id.ToString(); };
        }
        protected override void AddItem(WarehouseRUVM item)
        {
            foreach (RequestCustomerLegal legal in item.CustomerLegals)
            {
                if (!Items.Contains(legal.Request)) Items.Add(legal.Request);
            }
        }
    }
    public class WarehouseRUStoreNumCheckListBoxVMFill : libui.CheckListBoxVMFill<WarehouseRUVM, string>
    {
        protected override void AddItem(WarehouseRUVM item)
        {
            if (!Items.Contains(item.StorageId)) Items.Add(item.StorageId);
        }
    }

    public class WarehouseRUTotal : lib.TotalValues.TotalViewValues<WarehouseRUVM>
    {
        internal WarehouseRUTotal(ListCollectionView view) : base(view)
        {
            //myinitselected = 2; // if not selected - sum=0
        }
        private int myitemcount;
        public int ItemCount { set { myitemcount = value; } get { return myitemcount; } }
        
        private decimal myactualweight;
        public decimal ActualWeight { set { myactualweight = value; } get { return myactualweight; } }
        private decimal mycellnumber;
        public decimal CellNumber { set { mycellnumber = value; } get { return mycellnumber; } }
        private decimal myofficialweight;
        public decimal OfficialWeight { set { myofficialweight = value; } get { return myofficialweight; } }
        private decimal myvolume;
        public decimal Volume { set { myvolume = value; } get { return myvolume; } }

        protected override void Item_ValueChangedHandler(WarehouseRUVM sender, lib.Interfaces.ValueChangedEventArgs<object> e)
        {
            decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
            switch (e.PropertyName)
            {
                case "Total" + nameof(WarehouseRUTotal.ActualWeight):
                    myactualweight += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.ActualWeight));
                    break;
                case "Total" + nameof(WarehouseRUTotal.CellNumber):
                    mycellnumber += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.CellNumber));
                    break;
                case "Total" + nameof(WarehouseRUTotal.OfficialWeight):
                    myofficialweight += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.OfficialWeight));
                    break;
                case "Total" + nameof(WarehouseRUTotal.Volume):
                    myvolume += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.Volume));
                    break;
            }
        }
        protected override void ValuesReset()
        {
            myactualweight = 0;
            mycellnumber = 0M;
            myofficialweight = 0M;
            myvolume = 0M;
        }
        protected override void ValuesPlus(WarehouseRUVM item)
        {
            myitemcount++;
            myactualweight += item.ActualWeight??0;
            mycellnumber += item.CellNumber??0;
            myofficialweight += item.OfficialWeight ?? 0;
            myvolume += item.Volume??0;
        }
        protected override void ValuesMinus(WarehouseRUVM item)
        {
            myitemcount--;
            myactualweight -= item.ActualWeight ?? 0;
            mycellnumber -= item.CellNumber ?? 0;
            myofficialweight -= item.OfficialWeight??0;
            myvolume -= item.Volume??0;
        }
        protected override void PropertiesChangedNotifycation()
        {
            this.PropertyChangedNotification("ItemCount");
            this.PropertyChangedNotification(nameof(this.ActualWeight));
            this.PropertyChangedNotification(nameof(this.CellNumber));
            this.PropertyChangedNotification(nameof(this.OfficialWeight));
            this.PropertyChangedNotification(nameof(this.Volume));
        }
    }
}
