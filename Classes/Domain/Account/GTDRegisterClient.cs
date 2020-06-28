using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class GTDRegisterClient : lib.DomainStampValueChanged
    {
        public GTDRegisterClient(int id, long stamp, DateTime? updatewhen, string updatewho, lib.DomainObjectState state
            , decimal? algvalue2, decimal? buyrate,CustomerLegal client, decimal? dtsum, decimal eurosum, GTDRegister gtd, decimal? selling,DateTime? sellingdate, decimal? sl,decimal? volume
            ) : base(id, stamp, updatewhen, updatewho, state)
        {
            myalgvalue2 = algvalue2;
            mybuyrate = buyrate;
            myclient = client;
            mydtsum = dtsum;
            myeurosum = eurosum;
            mygtd = gtd;
            mygtd.PropertyChanged += this.GTD_PropertyChanged;
            mygtd.Specification.PropertyChanged += this.Specification_PropertyChanged;
            mygtd.Specification.Declaration.PropertyChanged += this.Declaration_PropertyChanged;
            myselling = selling;
            mysellingdate = sellingdate;
            mysl = sl;
            myvolume = volume;
        }

        private decimal? myalgvalue2;
        internal decimal? AlgValue2
        {
            get
            {
                return myalgvalue2;
            }
        }
        private decimal? mybuyrate;
        public decimal? BuyRate
        { private set {
                decimal? cc = this.CC;
                Action action = () => {
                    this.PropertyChangedNotification(nameof(this.CC)); this.OnValueChanged(nameof(this.CC), cc, this.CC);
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupBU));
                    this.PropertyChangedNotification(nameof(this.MarkupAlg));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                };
                SetProperty<decimal?>(ref mybuyrate, value, action);
            }
            get { return mybuyrate; }
        }
        public decimal? CC
        { get { return mydtsum.HasValue & mybuyrate.HasValue ? mydtsum.Value * mybuyrate.Value :(decimal?)null; } }
        private Domain.CustomerLegal myclient;
        public Domain.CustomerLegal Client
        { internal set { SetProperty<Domain.CustomerLegal>(ref myclient, value); } get { return myclient; } }
        public decimal? CostLogistics
        { get { return this.DDSpidy + this.GTLS + this.MFK + this.Pari + this.SLWithoutRate + this.WestGateWithoutRate; } }
        public decimal? CostPer
        { get { return (this.SellingWithoutRate ?? 0) > 0M ? this.CostTotal / this.SellingWithoutRate : null; } }
        public decimal? CostTotal
        { get { return this.Fee + this.Tax + this.CC + this.CostLogistics; } }
        public decimal? DDSpidy
        {
            get { return this.Rate.HasValue & this.GTD.Specification.DDSpidy.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.DDSpidy.Value) : (decimal?)null; }
        }
        private decimal? mydtsum;
        public decimal? DTSum
        {
            private set
            {
                //decimal? sellingrate = this.SellingRate; this.OnValueChanged(nameof(this.SellingWithoutRate), sellingwithoutrate, this.SellingWithoutRate);
                decimal? cc = this.CC;
                Action action = () => {
                    this.PropertyChangedNotification(nameof(this.CC)); this.OnValueChanged(nameof(this.CC), cc, this.CC);
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupBU));
                    this.PropertyChangedNotification(nameof(this.MarkupAlg));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                };
                SetPropertyOnValueChanged<decimal?>(ref mydtsum, value, action);
            }
            get { return mydtsum; }
        }
        private decimal myeurosum;
        public decimal EuroSum
        { private set { SetProperty<decimal>(ref myeurosum, value); }  get { return myeurosum; } }
        public decimal? Fee
        { get { return this.Rate * this.GTD.Specification.Declaration.Fee; } }
        private GTDRegister mygtd;
        public GTDRegister GTD
        { set { SetProperty<GTDRegister>(ref mygtd, value); } get { return mygtd; } }
        public decimal? GTLS
        {
            get { return this.Rate.HasValue & this.GTD.Specification.GTLS.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.GTLS.Value) : (decimal?)null; }
        }
        public decimal? GTLSCur
        {
            get { return this.Rate.HasValue & this.GTD.Specification.GTLSCur.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.GTLSCur.Value) : (decimal?)null; }
        }
        public decimal? MarkupAlg
        {
            get { return (this.CC ?? 0M) > 0M ? (this.Selling - this.CC) / this.CC : null; }
        }
        public decimal? MarkupBU
        {
            get { return (this.CC ?? 0M) > 0M ? (this.SellingWithoutRate - this.CC) / this.CC : null; }
        }
        public decimal? MarkupTotal
        {
            get { return (this.CostTotal ?? 0M) > 0M ? (this.SellingWithoutRate - this.CostTotal) / this.CostTotal : null; }
        }
        public decimal? MFK
        {
            get { return this.Rate.HasValue & this.GTD.Specification.MFK.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.MFK.Value) : (decimal?)null; }
        }
        public decimal? MFKRate
        { get { return this.MFK * 20M / 120M; } }
        public decimal? MFKWithoutRate
        { get { return this.MFK - this.MFKRate; } }
        public decimal? Pari
        {
            get { return this.Rate.HasValue & this.GTD.Specification.Pari.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.Pari.Value) : (decimal?)null; }
        }
        public decimal? Profit
        { get { return this.SellingWithoutRate - this.CostTotal; } }
        public decimal? Profitability
        { get { return this.Profit.HasValue & this.SellingWithoutRate.HasValue ? decimal.Divide(this.Profit.Value, this.SellingWithoutRate.Value) : (decimal?)null; } }
        public decimal? Rate
        { get { return mygtd.DTSum.HasValue ? mydtsum / mygtd.DTSum : null; } }
        private decimal? mysl;
        public decimal? SL
        { set {
                decimal? slrate = this.SLRate;
                decimal? slwithoutrate = this.SLWithoutRate;
                Action action = () => {
                    this.PropertyChangedNotification(nameof(this.SLRate)); this.OnValueChanged(nameof(this.SLRate), slrate, this.SLRate);
                    this.PropertyChangedNotification(nameof(this.SLWithoutRate)); this.OnValueChanged(nameof(this.SLWithoutRate), slwithoutrate, this.SLWithoutRate);
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                };
                SetPropertyOnValueChanged<decimal?>(ref mysl, value, action); } get { return mysl; } }
        public decimal? SLRate
        { get { return mysl * 20M / 120M; } }
        public decimal? SLWithoutRate
        { get { return mysl - this.SLRate; } }
        private decimal? myselling;
        public decimal? Selling
        { private set
            {
                decimal? sellingrate = this.SellingRate;
                decimal? sellingwithoutrate = this.SellingWithoutRate;
                Action action = () => {
                    this.OnValueChanged(nameof(this.SellingRate), sellingrate, this.SellingRate); this.OnValueChanged(nameof(this.SellingWithoutRate), sellingwithoutrate, this.SellingWithoutRate);
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.MarkupBU));
                    this.PropertyChangedNotification(nameof(this.MarkupAlg));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                };
                SetPropertyOnValueChanged<decimal?>(ref myselling, value, action);
            } get { return myselling; } }
        public decimal? SellingRate
        { get { return myselling * 20M/120M; } }
        private DateTime? mysellingdate;
        public DateTime? SellingDate
        { internal set { SetProperty<DateTime?>(ref mysellingdate, value); } get { return mysellingdate; } }
        public decimal? SellingWithoutRate
        { get { return myselling - this.SellingRate; } }
        public decimal? Tax
        { get { return this.Rate * this.GTD.Specification.Declaration.Tax; } }
        public decimal? VAT
        { get { return this.Rate * this.GTD.Specification.Declaration.VAT; } }
        public decimal? VATPay
        { get { return this.SellingRate - this.VAT - this.SLRate - this.WestGateRate - this.MFKRate; } }
        private decimal? myvolume;
        public decimal? Volume
        { set { SetPropertyOnValueChanged<decimal?>(ref myvolume, value,()=> {this.PropertyChangedNotification(nameof(this.VolumeProfit));}); } get { return myvolume; } }
        public decimal? VolumeProfit
        { get { return (myvolume??0M)>0M && this.Profit.HasValue ? decimal.Divide(this.Profit.Value, myvolume.Value):(decimal?)null; } }
        public decimal? WestGate
        {
            get { return this.Rate.HasValue & this.GTD.Specification.WestGate.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.WestGate.Value) : (decimal?)null; }
        }
        public decimal? WestGateRate
        { get { return this.WestGate * 20M / 120M; } }
        public decimal? WestGateWithoutRate
        { get { return this.WestGate - this.WestGateRate; } }


        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            GTDRegisterClient templ = sample as GTDRegisterClient;
            this.BuyRate = templ.BuyRate;
            this.DTSum = templ.DTSum;
            this.EuroSum = templ.EuroSum;
            this.Selling = templ.Selling;
            this.SellingDate = templ.SellingDate;
            this.SL = templ.SL;
            this.Volume = templ.Volume;
        }
        protected override void RejectProperty(string property, object value)
        {
        }
        private void GTD_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(GTDRegister.DTSum):
                    this.PropertyChangedNotification(nameof(this.Rate));
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.Fee));
                    this.PropertyChangedNotification(nameof(this.DDSpidy));
                    this.PropertyChangedNotification(nameof(this.GTLS));
                    this.PropertyChangedNotification(nameof(this.GTLSCur));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.MFK));
                    this.PropertyChangedNotification(nameof(this.MFKRate));
                    this.PropertyChangedNotification(nameof(this.MFKWithoutRate));
                    this.PropertyChangedNotification(nameof(this.Pari));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.Tax));
                    this.PropertyChangedNotification(nameof(this.WestGate));
                    this.PropertyChangedNotification(nameof(this.WestGateRate));
                    this.PropertyChangedNotification(nameof(this.WestGateWithoutRate));
                    this.PropertyChangedNotification(nameof(this.VAT));
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    break;
            }
        }
        private void Specification_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Specification.Specification.DDSpidy):
                    this.PropertyChangedNotification(nameof(this.DDSpidy));
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    break;
                case nameof(Specification.Specification.Declaration):
                    mygtd.Specification.Declaration.PropertyChanged += this.Declaration_PropertyChanged;
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.Fee));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.Tax));
                    this.PropertyChangedNotification(nameof(this.VAT));
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    break;
                case nameof(Specification.Specification.GTLS):
                case nameof(Specification.Specification.GTLSCur):
                    this.PropertyChangedNotification(nameof(this.GTLS));
                    this.PropertyChangedNotification(nameof(this.GTLSCur));
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    break;
                case nameof(Specification.Specification.MFK):
                    this.PropertyChangedNotification(nameof(this.MFK));
                    this.PropertyChangedNotification(nameof(this.MFKRate));
                    this.PropertyChangedNotification(nameof(this.MFKWithoutRate));
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    break;
                case nameof(Specification.Specification.Pari):
                    this.PropertyChangedNotification(nameof(this.Pari));
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    break;
                case nameof(Specification.Specification.WestGate):
                    this.PropertyChangedNotification(nameof(this.WestGate));
                    this.PropertyChangedNotification(nameof(this.WestGateRate));
                    this.PropertyChangedNotification(nameof(this.WestGateWithoutRate));
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    break;
            }
        }
        private void Declaration_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Specification.Specification.Declaration.Fee):
                    this.PropertyChangedNotification(nameof(this.Fee));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    break;
                case nameof(Specification.Specification.Declaration.Tax):
                    this.PropertyChangedNotification(nameof(this.Tax));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    break;
                case nameof(Specification.Specification.Declaration.VAT):
                    this.PropertyChangedNotification(nameof(this.VAT));
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    break;
                case nameof(Specification.Specification.Declaration.CBRate):
                case nameof(Specification.Specification.Declaration.TotalSum):
                    this.PropertyChangedNotification(nameof(this.Selling));
                    this.PropertyChangedNotification(nameof(this.SellingRate));
                    this.PropertyChangedNotification(nameof(this.SellingWithoutRate));
                    break;
            }
        }
        internal void Unbind()
        {
            mygtd.PropertyChanged -= this.GTD_PropertyChanged;
            mygtd.Specification.PropertyChanged -= this.Specification_PropertyChanged;
            mygtd.Specification.Declaration.PropertyChanged -= this.Declaration_PropertyChanged;
        }
    }

    internal class GTDRegisterClientDBM:lib.DBManagerWhoWhen<GTDRegisterClient>
    {
        internal GTDRegisterClientDBM()
        {
            NeedAddConnection = true;
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "spec.SpecificationCustomer_sp";
            //InsertCommandText = "spec.SpecificationCustomerAdd_sp";
            UpdateCommandText = "spec.SpecificationCustomerUpd_sp";
            //DeleteCommandText = "spec.SpecificationCustomerDel_sp";

            SelectParams = new SqlParameter[] {
                new SqlParameter("@gtdid", System.Data.SqlDbType.Int),
                new SqlParameter("@filterid", System.Data.SqlDbType.Int),
                new SqlParameter("@servicetype", System.Data.SqlDbType.NVarChar,10)
            };
            UpdateParams = new SqlParameter[] { UpdateParams[0]
                , new SqlParameter("@customerid", System.Data.SqlDbType.Int)
                , new SqlParameter("@gtdid", System.Data.SqlDbType.Int)
                , new SqlParameter("@mysellingdate", System.Data.SqlDbType.DateTime2)
                , new SqlParameter("@mysellingdateupd", System.Data.SqlDbType.Bit)
                , new SqlParameter("@slupd", System.Data.SqlDbType.Bit)
                , new SqlParameter("@volumeupd", System.Data.SqlDbType.Bit)
                , new SqlParameter("@sl", System.Data.SqlDbType.Money)
                , new SqlParameter("@volume", System.Data.SqlDbType.Money)
            };
            UpdateParams[0].Direction = System.Data.ParameterDirection.InputOutput;
        }

        private GTDRegister mygtd;
        internal GTDRegister GTD
        { set { mygtd = value; } get { return mygtd; } }
        //private lib.SQLFilter.SQLFilter myfilter;
        //internal lib.SQLFilter.SQLFilter Filter
        //{ set { myfilter = value; } get { return myfilter; } }
        private GTDRegisterDBM mygdtdbm;
        internal GTDRegisterDBM GTDDBM
        { set { mygdtdbm = value; }  get { return mygdtdbm; } }
        private string myservicetype;
        public string ServiceType
        { set { myservicetype = value; } get { return myservicetype; } }
        private GTDRegisterStore mygtdstore;

        protected override GTDRegisterClient CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            List<DBMError> errors = new List<DBMError>();
            CustomerLegal customer = reader.IsDBNull(reader.GetOrdinal("customerid")) ? null : CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("customerid")), addcon, out errors);
            this.Errors.AddRange(errors);
            GTDRegisterClient item = new GTDRegisterClient(reader.GetInt32(0)
                , reader.IsDBNull(reader.GetOrdinal("stamp")) ? 0 : reader.GetInt64(reader.GetOrdinal("stamp"))
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated"))
                , reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater")), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("algvalue2")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("algvalue2"))
                , reader.IsDBNull(reader.GetOrdinal("buyrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("buyrate"))
                , customer
                , reader.IsDBNull(reader.GetOrdinal("dtsum")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("dtsum"))
                , 0//reader.IsDBNull(reader.GetOrdinal("eurosum")) ? 0 : reader.GetDecimal(reader.GetOrdinal("eurosum"))
                , mygtd
                , reader.IsDBNull(reader.GetOrdinal("selling")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("selling"))
                , reader.IsDBNull(reader.GetOrdinal("sellingdate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("sellingdate"))
                , reader.IsDBNull(reader.GetOrdinal("sl")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sl"))
                , reader.IsDBNull(reader.GetOrdinal("volume")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("volume"))
                );
            return item;
        }
        protected override void GetOutputSpecificParametersValue(GTDRegisterClient item)
        {
        }
        protected override bool LoadObjects()
        {
            //foreach (GTDRegisterClient item in this.Collection)
            //    LoadObjects(item);
            return this.Errors.Count == 0;
        }
        protected override bool SaveChildObjects(GTDRegisterClient item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(GTDRegisterClient item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            foreach (SqlParameter par in SelectParams)
                switch (par.ParameterName)
                {
                    case "@gtdid":
                        par.Value = mygtd?.Id;
                        break;
                    //case "@filterid":
                    //    par.Value = myfilter?.FilterWhereId;
                    //    break;
                    case "@servicetype":
                        par.Value = myservicetype;
                        break;
                }
            if (mygdtdbm != null)
            {
                if (mygtdstore == null)
                    mygtdstore = new GTDRegisterStore(mygdtdbm);
                else
                    mygtdstore.Clear();
            }
        }
        protected override bool SetSpecificParametersValue(GTDRegisterClient item)
        {
            foreach (SqlParameter par in this.UpdateParams)
            {
                switch (par.ParameterName)
                {
                    case "@customerid":
                        par.Value = item.Client.Id;
                        break;
                    case "@gtdid":
                        par.Value = item.GTD.Id;
                        break;
                    case "@mysellingdate":
                        par.Value = item.SellingDate;
                        break;
                    case "@mysellingdateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.SellingDate));
                        break;
                    case "@sl":
                        par.Value = item.SL;
                        break;
                    case "@slupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(GTDRegisterClient.SL));
                        break;
                    case "@volume":
                        par.Value = item.Volume;
                        break;
                    case "@volumeupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(GTDRegisterClient.Volume));
                        break;
                }
            }
            return true;
        }
    }

    public class GTDRegisterClientVM : lib.ViewModelErrorNotifyItem<GTDRegisterClient>
    {
        public GTDRegisterClientVM(GTDRegisterClient model) : base(model)
        {
            InitProperties();
        }

        public decimal? BuyRate
        { get { return this.IsEnabled ? this.DomainObject.BuyRate : (decimal?)null; } }
        public decimal? CC
        { get { return this.IsEnabled ? this.DomainObject.CC : (decimal?)null; } }
        private CustomerLegalVM mycustomer;
        public CustomerLegalVM Client
        {
            get { return mycustomer; }
        }
        public decimal? CostLogistics
        { get { return this.IsEnabled ? this.DomainObject.CostLogistics : (decimal?)null; } }
        public decimal? CostPer
        { get { return this.IsEnabled ? this.DomainObject.CostPer : (decimal?)null; } }
        public decimal? CostTotal
        { get { return this.IsEnabled ? this.DomainObject.CostTotal : (decimal?)null; } }
        public decimal? DDSpidy
        {
            get { return this.IsEnabled ? this.DomainObject.DDSpidy : (decimal?)null; }
        }
        public decimal? DTSum
        { get { return this.IsEnabled ? this.DomainObject.DTSum : (decimal?)null; } }
        public decimal? EuroSum
        { get { return this.IsEnabled ? this.DomainObject.EuroSum : (decimal?)null; } }
        public decimal? Fee
        { get { return this.IsEnabled ? this.DomainObject.Fee : (decimal?)null; } }
        //private GTDRegisterVM mygtd;
        public GTDRegister GTD
        { get { return this.IsEnabled ? this.DomainObject.GTD : null; } }
        public decimal? GTLS
        { get { return this.IsEnabled ? this.DomainObject.GTLS : (decimal?)null; } }
        public decimal? GTLSCur
        { get { return this.IsEnabled ? this.DomainObject.GTLSCur : (decimal?)null; } }
        public decimal? MarkupAlg
        {
            get { return this.DomainObject.MarkupAlg; }
        }
        public decimal? MarkupBU
        {
            get { return this.DomainObject.MarkupBU; }
        }
        public decimal? MarkupTotal
        {
            get { return this.DomainObject.MarkupTotal; }
        }
        public decimal? MFK
        { get { return this.IsEnabled ? this.DomainObject.MFK : (decimal?)null; } }
        public decimal? MFKRate
        { get { return this.IsEnabled ? this.DomainObject.MFKRate : (decimal?)null; } }
        public decimal? MFKWithoutRate
        { get { return this.IsEnabled ? this.DomainObject.MFKWithoutRate : (decimal?)null; } }
        public decimal? Pari
        { get { return this.IsEnabled ? this.DomainObject.Pari : (decimal?)null; } }
        public decimal? Profit
        { get { return this.IsEnabled ? this.DomainObject.Profit : (decimal?)null; } }
        public decimal? Profitability
        { get { return this.DomainObject.Profitability; } }
        public decimal? Rate
        { get { return this.IsEnabled ? this.DomainObject.Rate : (decimal?)null; } }
        public decimal? SL
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.SL, value.Value)))
                {
                    string name = nameof(this.SL);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SL);
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.SL = value.Value; }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.SL : (decimal?)null; }
        }
        public decimal? SLRate
        { get { return this.IsEnabled ? this.DomainObject.SLRate : (decimal?)null; } }
        public decimal? SLWithoutRate
        { get { return this.IsEnabled ? this.DomainObject.SLWithoutRate : (decimal?)null; } }
        public decimal? Selling
        { get { return this.IsEnabled ? this.DomainObject.Selling : (decimal?)null; } }
        public decimal? SellingRate
        { get { return this.IsEnabled ? this.DomainObject.SellingRate : (decimal?)null; } }
        public DateTime? SellingDate
        { get { return this.IsEnabled ? this.DomainObject.SellingDate : (DateTime?)null; } }
        public decimal? SellingWithoutRate
        { get { return this.IsEnabled ? this.DomainObject.SellingWithoutRate : (decimal?)null; } }
        public decimal? Tax
        { get { return this.IsEnabled ? this.DomainObject.Tax : (decimal?)null; } }
        public decimal? VAT
        { get { return this.IsEnabled ? this.DomainObject.VAT : (decimal?)null; } }
        public decimal? VATPay
        { get { return this.DomainObject.VATPay; } }
        public decimal? Volume
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.Volume, value.Value)))
                {
                    string name = nameof(this.Volume);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Volume);
                    if (this.ValidateProperty(name))
                    { ChangingDomainProperty = name; this.DomainObject.Volume = value.Value; }
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Volume : (decimal?)null; }
        }
        public decimal? VolumeProfit
        { get { return this.IsEnabled ? this.DomainObject.VolumeProfit : (decimal?)null; } }
        public decimal? WestGate
        { get { return this.IsEnabled ? this.DomainObject.WestGate : (decimal?)null; } }
        public decimal? WestGateRate
        { get { return this.IsEnabled ? this.DomainObject.WestGateRate : (decimal?)null; } }
        public decimal? WestGateWithoutRate
        { get { return this.IsEnabled ? this.DomainObject.WestGateWithoutRate : (decimal?)null; } }

        protected override bool DirtyCheckProperty()
        {
            return false;
        }
        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(this.DomainObject.IsLoaded):
                    mycustomer = new CustomerLegalVM(this.DomainObject.Client);
                    this.PropertyChangedNotification(nameof(this.Client));
                    break;
            }
        }
        protected override void InitProperties()
        {
            if (this.DomainObject.Client != null)
                mycustomer = new CustomerLegalVM(this.DomainObject.Client);
            //mygtd = new GTDRegisterVM(this.DomainObject.GTD);
            this.DomainObject.ValueChanged += this.Model_ValueChanged;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.SL):
                    this.DomainObject.SL = (decimal?)value;
                    break;
                case nameof(this.Volume):
                    this.DomainObject.Volume = (decimal?)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            return true;
        }
        private void Model_ValueChanged(object sender, lib.Interfaces.ValueChangedEventArgs<object> e)
        {
            this.OnValueChanged(e.PropertyName, e.OldValue, e.NewValue);
        }
    }

    public class GTDRegisterClientSynchronizer : lib.ModelViewCollectionsSynchronizer<GTDRegisterClient, GTDRegisterClientVM>
    {
        protected override GTDRegisterClient UnWrap(GTDRegisterClientVM wrap)
        {
            return wrap.DomainObject as GTDRegisterClient;
        }
        protected override GTDRegisterClientVM Wrap(GTDRegisterClient fill)
        {
            return new GTDRegisterClientVM(fill);
        }
    }

    public class GTDRegisterClientViewCommand : lib.ViewModelViewOnDemandCommand
    {
        internal GTDRegisterClientViewCommand(Importer importer) :base()
        {
            mygtddbm = new GTDRegisterDBM(); // для загрузки
            mygtddbm.Importer = importer;
            mygtddbm.ClientDBM = new GTDRegisterClientDBM();
            mymaindbm = new GTDRegisterClientDBM();
            mymaindbm.Collection = new System.Collections.ObjectModel.ObservableCollection<GTDRegisterClient>();
            mymaindbm.GTDDBM = mygtddbm; //для сохранения
            mydbm = mymaindbm;
            mysync = new GTDRegisterClientSynchronizer();
            mygtddbm.FillAsyncCompleted = () => {
                if (mymaindbm.Errors.Count > 0) OpenPopup(mymaindbm.ErrorMessage, true);
                foreach (GTDRegister gtd in mygtddbm.Collection)
                    foreach (GTDRegisterClient client in gtd.Clients)
                        mymaindbm.Collection.Add(client);
            };

            #region Filter
            mysellingdatefilter = new libui.DateFilterVM();
            mysellingdatefilter.IsNull = true;
            mysellingdatefilter.DateStart = DateTime.Today.AddMonths(-4);
            mysellingdatefilter.ExecCommand1 = () => { DatePeriodFilterRun(mysellingdatefilter, "sellingdate", "sellingdatemin", "sellingdatemax"); };
            mysellingdatefilter.ExecCommand2 = () => { mysellingdatefilter.Clear(); };
            #endregion
        }

        private GTDRegisterClientDBM mymaindbm;
        private GTDRegisterDBM mygtddbm;
        private GTDRegisterClientSynchronizer mysync;
        internal Importer Importer
        { get { return mygtddbm.Importer; } }

        #region Filter
        private lib.SQLFilter.SQLFilter myfilter;
        internal lib.SQLFilter.SQLFilter Filter
        { get { return myfilter; } }
        private GTDRegisterAgentCheckListBoxVMFillDefault myagentfilter;
        public GTDRegisterAgentCheckListBoxVMFillDefault AgentFilter
        { get { return myagentfilter; } }
        private int mydeclarationnumberfiltergroup;
        private GTDRegisterDeclarationNumberCheckListBoxVMFill mydeclarationnumberfilter;
        public GTDRegisterDeclarationNumberCheckListBoxVMFill DeclarationNumberFilter
        { get { return mydeclarationnumberfilter; } }
        private int myparcelfiltergroup;
        private GTDRegisterParcelCheckListBoxVMFillDefault myparcelfilter;
        public GTDRegisterParcelCheckListBoxVMFillDefault ParcelFilter
        { get { return myparcelfilter; } }
        private libui.DateFilterVM mysellingdatefilter;
        public libui.DateFilterVM SellingDateFilter
        { get { return mysellingdatefilter; } }

        private void NumberFilterRun(libui.NumberFilterVM filter, string property)
        {
            List<lib.SQLFilter.SQLFilterCondition> cond = myfilter.ConditionGet(myfilter.FilterWhereId, property);
            if (filter.FilterOn)
            {
                if (!filter.IsNotNull)
                {
                    if (cond.Count > 0)
                    {
                        if (!cond[0].propertyOperator.Equals("IS NULL"))
                        {
                            myfilter.ConditionValuesDel(cond[0].propertyid);
                            myfilter.ConditionUpd(cond[0].propertyid, "IS NULL");
                        }
                    }
                    else
                        myfilter.ConditionAdd(myfilter.FilterWhereId, property, "IS NULL");
                }
                else if (filter.IsRange)
                    myfilter.SetRange(myfilter.FilterWhereId, property, filter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture), filter.NumberStop?.ToString(System.Globalization.CultureInfo.InvariantCulture));
                else
                    myfilter.SetNumber(myfilter.FilterWhereId, property, filter.Operator, filter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (cond.Count > 0)
                myfilter.ConditionDel(cond[0].propertyid);
            RefreshData(null);
        }
        private void DateFilterRun(libui.DateFilterVM filter, string property)
        {
            myfilter.SetDate(myfilter.FilterWhereId, property, property, filter.DateStart, filter.DateStop, filter.IsNull);
        }
        private void DatePeriodFilterRun(libui.DateFilterVM filter, string group, string propertystart, string propertystop)
        {
            myfilter.SetDatePeriod(myfilter.FilterWhereId, group, propertystart, propertystop, mysellingdatefilter.DateStart, mysellingdatefilter.DateStop, mysellingdatefilter.IsNull);
        }
        #endregion

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
        protected override IList CreateCollectionOnDemand()
        {
            myfilter = new lib.SQLFilter.SQLFilter("GTDRegister", "AND", CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            mydeclarationnumberfiltergroup = myfilter.GroupAdd(myfilter.FilterWhereId, "decnum", "OR");
            myparcelfiltergroup = myfilter.GroupAdd(myfilter.FilterWhereId, "parcel", "OR");
            myfilter.SetDatePeriod(myfilter.FilterWhereId, "sellingdate", "sellingdatemin", "sellingdatemax", mysellingdatefilter.DateStart, mysellingdatefilter.DateStop, mysellingdatefilter.IsNull);

            mygtddbm.Filter = myfilter;
            mygtddbm.Collection = new System.Collections.ObjectModel.ObservableCollection<GTDRegister>();
            mygtddbm.FillAsync();
            mysync.DomainCollection = mymaindbm.Collection;
            return mysync.ViewModelCollection;
        }
        protected override void OtherViewRefresh()
        {
        }
        protected override void RefreshData(object parametr)
        {
            if (myfilter.isEmpty)
                this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
            else
            {
                mymaindbm.Collection.Clear();
                foreach (GTDRegister item in mygtddbm.Collection) item.Unbind();
                mygtddbm.FillAsync();
            }
        }
        protected override void SettingView()
        {
            //myagentfilter.ItemsSource = myview.OfType<GTDRegisterVM>();
            //mydeclarationnumberfilter.ItemsSource = myview.OfType<GTDRegisterVM>();
            //myparcelfilter.ItemsSource = myview.OfType<GTDRegisterVM>();
        }
    }
}
