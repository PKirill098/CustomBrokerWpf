using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using KirillPolyanskiy.DataModelClassLibrary;
using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    internal struct GTDRegisterClientRecord
    {
        internal int id;
        internal long stamp;
        internal DateTime? updated;
        internal string updater;

        internal decimal? buyrate;
        internal int? customer;
        internal decimal? dtsum;
        internal decimal? incomealg;
        internal decimal? p3347value1;
        internal decimal? p3140value2;
        internal decimal? selling;
        internal DateTime? sellingdate;
        internal decimal? sl;
        internal decimal? volume;
    }
    public class GTDRegisterClient : lib.DomainStampValueChanged
    {
        public GTDRegisterClient(int id, long stamp, DateTime? updatewhen, string updatewho, lib.DomainObjectState state
            , decimal? p3347value1, decimal? p3140value2, decimal? buyrate,CustomerLegal client, decimal? dtsum, decimal eurosum, GTDRegister gtd,decimal? incomealg, decimal? selling,DateTime? sellingdate, decimal? sl,decimal? volume
            ) : base(id, stamp, updatewhen, updatewho, state)
        {
            myprofitalge = p3347value1;
            myalgvalue2 = p3140value2;
            mybuyrate = buyrate;
            myclient = client;
            mydtsum = dtsum;
            myeurosum = eurosum;
            mygtd = gtd;
            //mygtd.PropertyChanged += this.GTD_PropertyChanged;
            mygtd.Specification.PropertyChanged += this.Specification_PropertyChanged;
            mygtd.Specification.Declaration.PropertyChanged += this.Declaration_PropertyChanged;
			mygtd.Specification.Parcel.PropertyChanged += this.Parcel_PropertyChanged;
            myincomealg = incomealg;
            myselling = selling;
            mysellingdate = sellingdate;
            mysl = sl;
            myvolume = volume;

            this.CalculatedUpdate();
            this.OldUpdate();
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
        { get { return (this.DDSpidy??0M) + (this.GTLS??0M) + (this.MFK??0M) + (this.Pari??0M) + (this.SLWithoutRate??0M) + (this.WestGateWithoutRate??0); } }
        public decimal? CostPer
        { get { return (this.SellingWithoutRate ?? 0) > 0M ? this.CostTotal / this.SellingWithoutRate : null; } }
        public decimal? CostTotal
        { get { return (this.Fee??0M) + (this.Tax??0M) + (this.CC??0M) + (this.CostLogistics??0M); } }
        private decimal? myddspidyold;
        public decimal? DDSpidy { private set; get; }
        public decimal? DifProfitIncomeAlg
        { get { return this.Profit - this.IncomeAlg; } }
        private decimal? mydtsum;
        public decimal? DTSum
        {
            private set
            {
                //decimal? sellingrate = this.SellingRate; this.OnValueChanged(nameof(this.SellingWithoutRate), sellingwithoutrate, this.SellingWithoutRate);
                decimal? cc = this.CC;
                
                Action action = () => {
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.CC)); this.OnValueChanged(nameof(this.CC), cc, this.CC);
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.DDSpidy)); this.OnValueChanged(nameof(this.DDSpidy), myddspidyold, this.DDSpidy);
                    this.PropertyChangedNotification(nameof(this.Fee)); this.OnValueChanged(nameof(this.Fee), myfeeold, this.Fee);
                    this.PropertyChangedNotification(nameof(this.GTLS)); this.OnValueChanged(nameof(this.GTLS), mygtlsold, this.GTLS);
                    this.PropertyChangedNotification(nameof(this.GTLSCur)); this.OnValueChanged(nameof(this.GTLSCur), mygtlscurold, this.GTLSCur);
                    this.PropertyChangedNotification(nameof(this.MFK)); this.OnValueChanged(nameof(this.MFK), mymfkold, this.MFK);
                    this.PropertyChangedNotification(nameof(this.MarkupBU));
                    this.PropertyChangedNotification(nameof(this.MarkupAlg));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Pari)); this.OnValueChanged(nameof(this.Pari), mypariold, this.Pari);
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.Tax)); this.OnValueChanged(nameof(this.Tax), mytaxold, this.Tax);
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    this.PropertyChangedNotification(nameof(this.VAT)); this.OnValueChanged(nameof(this.VAT), myvatold, this.VAT);
                    this.PropertyChangedNotification(nameof(this.WestGate)); this.OnValueChanged(nameof(this.WestGate), mywestgateold, this.WestGate);
                    this.OldUpdate();
                };
                SetPropertyOnValueChanged<decimal?>(ref mydtsum, value, action);
            }
            get { return mydtsum; }
        }
        private decimal myeurosum;
        public decimal EuroSum
        { private set { SetProperty<decimal>(ref myeurosum, value); }  get { return myeurosum; } }
        private decimal? myfeeold;
        public decimal? Fee { private set; get; }
        private GTDRegister mygtd;
        public GTDRegister GTD
        { set { SetProperty<GTDRegister>(ref mygtd, value); } get { return mygtd; } }
        private decimal? mygtlsold;
        public decimal? GTLS { private set; get; }
        private decimal? mygtlscurold;
        public decimal? GTLSCur { private set; get; }
        private decimal? myincomealg;
        public decimal? IncomeAlg
        { private set 
            {
                decimal? difprofitincomealgold = this.DifProfitIncomeAlg;
                decimal? incomealgold = myincomealg;
                myincomealg = value;
                this.PropertyChangedNotification(nameof(this.IncomeAlg));
                this.OnValueChanged(nameof(this.IncomeAlg), incomealgold, myincomealg);
                this.PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
                this.OnValueChanged(nameof(this.DifProfitIncomeAlg), difprofitincomealgold, this.DifProfitIncomeAlg);
            } 
            get { return myincomealg; } }
		private List<Manager> mymanagers;
		public List<Manager> Managers
		{
			get
			{
				if (mymanagers == null)
				{
					mymanagers = new List<Manager>();
					this.ManagersRefresh();
				}
				return mymanagers;
			}
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
        private decimal? mymfkold;
        public decimal? MFK { private set; get; }
        public decimal? MFKRate
        { get { return this.MFK * 20M / 120M; } }
        public decimal? MFKWithoutRate
        { get { return this.MFK - this.MFKRate; } }
        private decimal? mypariold;
        public decimal? Pari { private set; get; }
        public decimal? Profit
        { get { return this.SellingWithoutRate - this.CostTotal; } }
        public decimal? Profitability
        { get { return this.Profit.HasValue & this.SellingWithoutRate.HasValue ? decimal.Divide(this.Profit.Value, this.SellingWithoutRate.Value) : (decimal?)null; } }
        private decimal? myprofitalge;
        public decimal? ProfitAlgE
        { private set 
            {
                decimal? myprofitalgeold = myprofitalge;
                myprofitalge = value;
                this.PropertyChangedNotification(nameof(this.ProfitAlgE));
                this.OnValueChanged(nameof(this.ProfitAlgE), myprofitalgeold, myprofitalge);
                this.PropertyChangedNotification(nameof(this.ProfitAlgR));
            } 
            get { return myprofitalge; } }
        private decimal? myprofitalgrold;
        public decimal? ProfitAlgR { private set; get; }
        public decimal? ProfitDiff
        { get { return this.Profit - this.ProfitAlgR; } }
        public decimal? Rate
        { get { return (mygtd.Specification.Cost ?? 0M)>0M ? mydtsum / mygtd.Specification.Cost : null; } }
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
        private decimal? mytaxold;
        public decimal? Tax { get; private set; }
        private decimal? myvatold;
        public decimal? VAT { private set; get; }
        public decimal? VATPay
        { get { return this.SellingRate - this.VAT - (this.SLRate??0M) - (this.WestGateRate??0M) - (this.MFKRate??0M); } }
        private decimal? myvolume;
        public decimal? Volume
        { set { SetPropertyOnValueChanged<decimal?>(ref myvolume, value,()=> {this.PropertyChangedNotification(nameof(this.VolumeProfit));}); } get { return myvolume; } }
        public decimal? VolumeProfit
        { get { return (myvolume??0M)>0M && this.Profit.HasValue ? decimal.Divide(this.Profit.Value, myvolume.Value):(decimal?)null; } }
        private decimal? mywestgateold;
        public decimal? WestGate { private set; get; }
        public decimal? WestGateRate
        { get { return this.WestGate * 20M / 120M; } }
        public decimal? WestGateWithoutRate
        { get { return this.WestGate - this.WestGateRate; } }

        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            GTDRegisterClient templ = sample as GTDRegisterClient;
            this.ProfitAlgE = templ.ProfitAlgE;
            this.BuyRate = templ.BuyRate;
            this.DTSum = templ.DTSum;
            this.EuroSum = templ.EuroSum;
            this.IncomeAlg = templ.IncomeAlg;
            this.Selling = templ.Selling;
            this.SellingDate = templ.SellingDate;
            this.SL = templ.SL;
            this.Volume = templ.Volume;
            ManagersRefresh();
		}
        protected override void RejectProperty(string property, object value)
        {
        }
        
        private void CalculatedUpdate()
        {
            DDSpidy = this.Rate.HasValue & this.GTD.Specification.DDSpidy.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.DDSpidy.Value) : (decimal?)null;
            Fee = this.Rate * this.GTD.Specification.Declaration.Fee;
            GTLS = this.Rate.HasValue & this.GTD.Specification.GTLS.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.GTLS.Value) : (decimal?)null;
            GTLSCur = this.Rate.HasValue & this.GTD.Specification.GTLSCur.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.GTLSCur.Value) : (decimal?)null;
            MFK = this.Rate.HasValue & this.GTD.Specification.MFK.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.MFK.Value) : (decimal?)null;
            Pari = this.Rate.HasValue & this.GTD.Specification.Pari.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.Pari.Value) : (decimal?)null;
            ProfitAlgR = this.ProfitAlgE * this.GTD.Specification?.Declaration.CBRate;
            Tax = this.Rate * this.GTD.Specification.Declaration.Tax;
            VAT = this.Rate * this.GTD.Specification.Declaration.VAT;
            WestGate = this.Rate.HasValue & this.GTD.Specification.WestGate.HasValue ? decimal.Multiply(this.Rate.Value, this.GTD.Specification.WestGate.Value) : (decimal?)null;
        }
        private void OldUpdate()
        {
            myddspidyold = this.DDSpidy;
            myfeeold = this.Fee;
            mygtlsold = this.GTLS;
            mygtlscurold = this.GTLSCur;
            mymfkold = this.MFK;
            mypariold = this.Pari;
            myprofitalgrold = this.ProfitAlgR;
            mytaxold = this.Tax;
            myvatold = this.VAT;
            mywestgateold = this.WestGate;
        }
        //private void GTD_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    switch(e.PropertyName)
        //    {
        //        case nameof(GTDRegister.DTSum):
        //            this.CalculatedUpdate();
        //            break;
        //    }
        //}
        private void Specification_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Specification.Specification.DDSpidy):
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.DDSpidy));
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    myddspidyold = this.DDSpidy;
                    break;
                case nameof(Specification.Specification.Declaration):
                    mygtd.Specification.Declaration.PropertyChanged += this.Declaration_PropertyChanged;
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.DDSpidy)); this.OnValueChanged(nameof(this.DDSpidy), myddspidyold, this.DDSpidy);
                    this.PropertyChangedNotification(nameof(this.Fee)); this.OnValueChanged(nameof(this.Fee), myfeeold, this.Fee);
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.MFK)); this.OnValueChanged(nameof(this.MFK), mymfkold, this.MFK);
                    this.PropertyChangedNotification(nameof(this.Pari)); this.OnValueChanged(nameof(this.Pari), mypariold, this.Pari);
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.ProfitAlgR)); this.OnValueChanged(nameof(this.ProfitAlgR), myprofitalgrold, this.ProfitAlgR);
                    this.PropertyChangedNotification(nameof(this.Tax)); this.OnValueChanged(nameof(this.Tax), mytaxold, this.Tax);
                    this.PropertyChangedNotification(nameof(this.VAT)); this.OnValueChanged(nameof(this.VAT), myvatold, this.VAT);
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    this.PropertyChangedNotification(nameof(this.WestGate)); this.OnValueChanged(nameof(this.WestGate), mywestgateold, this.WestGate);
                    this.OldUpdate();
                    break;
                case nameof(Specification.Specification.GTLS):
                case nameof(Specification.Specification.GTLSCur):
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.GTLS)); this.OnValueChanged(nameof(this.GTLS), mygtlsold, this.GTLS);
                    this.PropertyChangedNotification(nameof(this.GTLSCur)); this.OnValueChanged(nameof(this.GTLSCur), mygtlscurold, this.GTLSCur);
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    mygtlsold = this.GTLS; mygtlscurold = this.GTLSCur;
                    break;
                case nameof(Specification.Specification.MFK):
                    this.CalculatedUpdate();
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
                    mymfkold = this.MFK;
                    break;
                case nameof(Specification.Specification.Pari):
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.Pari)); this.OnValueChanged(nameof(this.Pari), mypariold, this.Pari);
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    mypariold = this.Pari;
                    break;
                case nameof(Specification.Specification.WestGate):
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.WestGate)); this.OnValueChanged(nameof(this.WestGate), mywestgateold, this.WestGate);
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
                    mywestgateold = this.WestGate;
                    break;
            }
        }
        private void Declaration_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Specification.Specification.Declaration.Fee):
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.Fee)); this.OnValueChanged(nameof(this.Fee), myfeeold, this.Fee);
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    myfeeold = this.Fee;
                    break;
                case nameof(Specification.Specification.Declaration.Tax):
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.Tax)); this.OnValueChanged(nameof(this.Tax), mytaxold, this.Tax);
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    mytaxold = this.Tax;
                    break;
                case nameof(Specification.Specification.Declaration.VAT):
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.VAT)); this.OnValueChanged(nameof(this.VAT), myvatold, this.VAT);
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    myvatold = this.VAT;
                    break;
                case nameof(Specification.Specification.Declaration.CBRate):
                case nameof(Specification.Specification.Declaration.TotalSum):
                    this.CalculatedUpdate();
                    this.PropertyChangedNotification(nameof(this.Rate));
                    this.PropertyChangedNotification(nameof(this.CostLogistics));
                    this.PropertyChangedNotification(nameof(this.CostPer));
                    this.PropertyChangedNotification(nameof(this.CostTotal));
                    this.PropertyChangedNotification(nameof(this.DDSpidy)); this.OnValueChanged(nameof(this.DDSpidy), myddspidyold, this.DDSpidy);
                    this.PropertyChangedNotification(nameof(this.Fee)); this.OnValueChanged(nameof(this.Fee), myfeeold, this.Fee);
                    this.PropertyChangedNotification(nameof(this.GTLS)); this.OnValueChanged(nameof(this.GTLS), mygtlsold, this.GTLS);
                    this.PropertyChangedNotification(nameof(this.GTLSCur)); this.OnValueChanged(nameof(this.GTLSCur), mygtlscurold, this.GTLSCur);
                    this.PropertyChangedNotification(nameof(this.MarkupTotal));
                    this.PropertyChangedNotification(nameof(this.MFK)); this.OnValueChanged(nameof(this.MFK), mymfkold, this.MFK);
                    this.PropertyChangedNotification(nameof(this.MFKRate));
                    this.PropertyChangedNotification(nameof(this.MFKWithoutRate));
                    this.PropertyChangedNotification(nameof(this.Pari)); this.OnValueChanged(nameof(this.Pari), mypariold, this.Pari);
                    this.PropertyChangedNotification(nameof(this.Profit));
                    this.PropertyChangedNotification(nameof(this.Profitability));
                    this.PropertyChangedNotification(nameof(this.ProfitAlgR)); this.OnValueChanged(nameof(this.ProfitAlgR), myprofitalgrold, this.ProfitAlgR);
                    this.PropertyChangedNotification(nameof(this.Selling));
                    this.PropertyChangedNotification(nameof(this.SellingRate));
                    this.PropertyChangedNotification(nameof(this.SellingWithoutRate));
                    this.PropertyChangedNotification(nameof(this.Tax)); this.OnValueChanged(nameof(this.Tax), mytaxold, this.Tax);
                    this.PropertyChangedNotification(nameof(this.WestGate));
                    this.PropertyChangedNotification(nameof(this.WestGateRate));
                    this.PropertyChangedNotification(nameof(this.WestGateWithoutRate));
                    this.PropertyChangedNotification(nameof(this.VAT)); this.OnValueChanged(nameof(this.VAT), myvatold, this.VAT);
                    this.PropertyChangedNotification(nameof(this.VATPay));
                    this.PropertyChangedNotification(nameof(this.VolumeProfit));
                    this.PropertyChangedNotification(nameof(this.WestGate)); this.OnValueChanged(nameof(this.WestGate), mywestgateold, this.WestGate);
                    this.OldUpdate();
                    //myprofitalgrold = this.ProfitAlgR;
                    break;
            }
        }
		private void Parcel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(Parcel.Requests):
					this.ManagersRefresh();
					break;
			}
		}
		internal void Unbind()
        {
            //mygtd.PropertyChanged -= this.GTD_PropertyChanged;
            mygtd.Specification.PropertyChanged -= this.Specification_PropertyChanged;
            mygtd.Specification.Declaration.PropertyChanged -= this.Declaration_PropertyChanged;
        }
		private void ManagersRefresh()
		{
			if (mymanagers == null) return;
            mymanagers.Clear();
			foreach (Request request in this.GTD.Specification.Requests)
				if (request.Manager != null 
                        && !mymanagers.Contains(request.Manager)
                        && request.CustomerLegals.FirstOrDefault<RequestCustomerLegal>((RequestCustomerLegal legal) => { return legal.Selected && legal.CustomerLegal == this.Client; })!=null)
					mymanagers.Add(request.Manager);
			this.PropertyChangedNotification(nameof(GTDRegisterClient.Managers));
		}
	}

	internal class GTDRegisterClientDBM:lib.DBManagerWhoWhen<GTDRegisterClientRecord,GTDRegisterClient>
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

        protected override GTDRegisterClientRecord CreateRecord(SqlDataReader reader)
        {
            return new GTDRegisterClientRecord()
            {
                id=reader.GetInt32(0)
                , stamp=reader.IsDBNull(reader.GetOrdinal("stamp")) ? 0 : reader.GetInt64(reader.GetOrdinal("stamp"))
                , updated=reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated"))
                , updater=reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
                , buyrate=reader.IsDBNull(reader.GetOrdinal("buyrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("buyrate"))
                , customer = reader.IsDBNull(reader.GetOrdinal("customerid")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("customerid"))
                , dtsum=reader.IsDBNull(reader.GetOrdinal("dtsum")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("dtsum"))
                , incomealg=reader.IsDBNull(reader.GetOrdinal("r44value1")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("r44value1"))
                , p3347value1=reader.IsDBNull(reader.GetOrdinal("p3347value1")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("p3347value1"))
                , p3140value2=reader.IsDBNull(reader.GetOrdinal("p3140value2")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("p3140value2"))
                , selling=reader.IsDBNull(reader.GetOrdinal("selling")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("selling"))
                , sellingdate=reader.IsDBNull(reader.GetOrdinal("sellingdate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("sellingdate"))
                , sl=reader.IsDBNull(reader.GetOrdinal("sl")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sl"))
                , volume=reader.IsDBNull(reader.GetOrdinal("volume")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("volume"))
            };
        }
        protected override GTDRegisterClient CreateModel(GTDRegisterClientRecord record,SqlConnection addcon, CancellationToken canceltasktoken = default)
        {
            List<DBMError> errors = new List<DBMError>();
            CustomerLegal customer = record.customer.HasValue ? CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(record.customer.Value, addcon, out errors) : null;
            this.Errors.AddRange(errors);
            GTDRegisterClient item = new GTDRegisterClient(record.id, record.stamp, record.updated, record.updater, lib.DomainObjectState.Unchanged
                , record.p3347value1
                , record.p3140value2
                , record.buyrate
                , customer
                , record.dtsum
                , 0//reader.IsDBNull(reader.GetOrdinal("eurosum")) ? 0 : reader.GetDecimal(reader.GetOrdinal("eurosum"))
                , mygtd
                , record.incomealg
                , record.selling
                , record.sellingdate
                , record.sl
                , record.volume
                );
            return item;
        }
        //protected override void CancelLoad()
        //{
        //    if (mygdtdbm != null) mygdtdbm.CancelingLoad = this.CancelingLoad;
        //}
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
        protected override bool SetParametersValue(GTDRegisterClient item)
        {
            base.SetParametersValue(item);
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

    public class GTDRegisterClientVM : lib.ViewModelErrorNotifyItem<GTDRegisterClient>, lib.Interfaces.ITotalValuesItem
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
        public decimal? DifProfitIncomeAlg
        { get { return this.IsEnabled ? this.DomainObject.DifProfitIncomeAlg : (decimal?)null; } }
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
        public decimal? IncomeAlg
        { get { return this.IsEnabled ? this.DomainObject.IncomeAlg : (decimal?)null; } }
		private string mymanagers;
		public string Managers
		{
			get
			{
				return mymanagers;
			}
		}
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
        public decimal? ProfitAlgE
        { get { return this.IsEnabled ? this.DomainObject.ProfitAlgE : (decimal?)null; } }
        public decimal? ProfitAlgR
        { get { return this.IsEnabled ? this.DomainObject.ProfitAlgR : (decimal?)null; } }
        public decimal? ProfitDiff
        { get { return this.IsEnabled ? this.DomainObject.ProfitDiff : (decimal?)null; } }
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

        public bool ProcessedIn { get; set; }
        public bool ProcessedOut { get; set; }
        public bool Selected { get { return false; } set { } }

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
				case nameof(GTDRegisterClient.Managers):
					if (!myinitmanagers) this.InitManagers();
					break;
			}
		}
        protected override void InitProperties()
        {
            if (this.DomainObject.Client != null)
            {
                mycustomer = new CustomerLegalVM(this.DomainObject.Client);
				this.InitManagers();
			}
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

		private bool myinitmanagers;
		private void InitManagers()
		{
			myinitmanagers = true; // первая инициализация повторный вызов через событие
			StringBuilder str = new StringBuilder();
			foreach (Manager manager in this.DomainObject.Managers)
			{
				str.Append((str.Length == 0 ? string.Empty : ", ") + manager.Name);
			}
			mymanagers = str.ToString();
			this.PropertyChangedNotification(nameof(GTDRegisterClientVM.Managers));
			myinitmanagers = false;
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
			if (filter.Synchronized) return;
			myfilter.SetNumber(myfilter.FilterWhereId, property, filter.Operator1.SQLOperator, filter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture), filter.Operator2.SQLOperator, filter.NumberStop?.ToString(System.Globalization.CultureInfo.InvariantCulture),filter.IsNull);
			filter.Synchronized = true;
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

    public class GTDRegisterClientTotal : lib.TotalValues.TotalViewValues<GTDRegisterClientVM>, lib.Interfaces.IValueChanged<decimal>
    {
        internal GTDRegisterClientTotal(System.Windows.Data.ListCollectionView view) : base(view)
        {
            //myinitselected = 2; // if not selected - sum=0
        }

        private int myitemcount;
        public int ItemCount { set { myitemcount = value; } get { return myitemcount; } }
        private decimal mycc;
        public decimal CC { set { mycc = value; } get { return mycc; } }
        //private decimal myeurosum;
        //public decimal EuroSum { set { myeurosum = value; } get { return myeurosum; } }
        private decimal myddspidy;
        public decimal DDSpidy { set { myddspidy = value; } get { return myddspidy; } }
        private decimal mydtsum;
        public decimal DTSum { set { mydtsum = value; } get { return mydtsum; } }
        private decimal myfee;
        public decimal Fee { set { myfee = value; } get { return myfee; } }
        private decimal mygtls;
        public decimal GTLS { set { mygtls = value; } get { return mygtls; } }
        private decimal mygtlscur;
        public decimal GTLSCur { set { mygtlscur = value; } get { return mygtlscur; } }
        private decimal myincomealg;
        public decimal IncomeAlg { set { myincomealg = value; } get { return myincomealg; } }
        private decimal mymfk;
        public decimal MFK { set { mymfk = value; } get { return mymfk; } }
        private decimal mypari;
        public decimal Pari { set { mypari = value; } get { return mypari; } }
        private decimal myprofitalge;
        public decimal ProfitAlgE { set { myprofitalge = value; } get { return myprofitalge; } }
        private decimal myprofitalgr;
        public decimal ProfitAlgR { set { myprofitalgr = value; } get { return myprofitalgr; } }
        private decimal myselling;
        public decimal Selling { set { myselling = value; } get { return myselling; } }
        private decimal mysl;
        public decimal SL { set { mysl = value; } get { return mysl; } }
        private decimal mytax;
        public decimal Tax { set { mytax = value; } get { return mytax; } }
        private decimal myvat;
        public decimal Vat { set { myvat = value; } get { return myvat; } }
        private decimal myvolume;
        public decimal Volume { set { myvolume = value; } get { return myvolume; } }
        private decimal mywestgate;
        public decimal WestGate { set { mywestgate = value; } get { return mywestgate; } }

        protected override void Item_ValueChangedHandler(GTDRegisterClientVM sender, lib.Interfaces.ValueChangedEventArgs<object> e)
        {
            decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M), propertyoldvalue;
            switch (e.PropertyName)
            {
                case nameof(GTDRegisterClientVM.CC):
                    propertyoldvalue = mycc;
                    mycc += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.CC));
                    OnValueChanged(nameof(this.CC), propertyoldvalue, mycc);
                    break;
                case nameof(GTDRegisterClientVM.MFK):
                    propertyoldvalue = mymfk;
                    mymfk += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.MFK));
                    OnValueChanged(nameof(this.MFK), propertyoldvalue, mymfk);
                    break;
                case nameof(GTDRegisterClientVM.DDSpidy):
                    propertyoldvalue = myddspidy;
                    myddspidy += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.DDSpidy));
                    OnValueChanged(nameof(this.DDSpidy), propertyoldvalue, myddspidy);
                    break;
                case nameof(GTDRegisterClientVM.DTSum):
                    propertyoldvalue = mydtsum;
                    mydtsum += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.DTSum));
                    OnValueChanged(nameof(this.DTSum), propertyoldvalue, mydtsum);
                    break;
                //case nameof(GTDRegisterClientVM.EuroSum):
                //    propertyoldvalue = myeurosum;
                //    myeurosum += newvalue - oldvalue;
                //    PropertyChangedNotification(nameof(this.EuroSum));
                //    OnValueChanged(nameof(this.CC), propertyoldvalue, myeurosum);
                //    break;
                case nameof(GTDRegisterClientVM.Fee):
                    propertyoldvalue = myfee;
                    myfee += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.Fee));
                    OnValueChanged(nameof(this.Fee), propertyoldvalue, myfee);
                    break;
                case nameof(GTDRegisterClientVM.GTLS):
                    propertyoldvalue = mygtls;
                    mygtls += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.GTLS));
                    OnValueChanged(nameof(this.GTLS), propertyoldvalue, mygtls);
                    break;
                case nameof(GTDRegisterClientVM.GTLSCur):
                    propertyoldvalue = mygtlscur;
                    mygtlscur += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.GTLSCur));
                    OnValueChanged(nameof(this.GTLSCur), propertyoldvalue, mygtlscur);
                    break;
                case nameof(GTDRegisterClientVM.IncomeAlg):
                    propertyoldvalue = myincomealg;
                    myincomealg += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.IncomeAlg));
                    OnValueChanged(nameof(this.IncomeAlg), propertyoldvalue, myincomealg);
                    break;
                case nameof(GTDRegisterClientVM.Pari):
                    propertyoldvalue = mypari;
                    mypari += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.Pari));
                    OnValueChanged(nameof(this.Pari), propertyoldvalue, mypari);
                    break;
                case nameof(GTDRegisterClientVM.ProfitAlgE):
                    propertyoldvalue = myprofitalge;
                    myprofitalge += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.ProfitAlgE));
                    OnValueChanged(nameof(this.ProfitAlgE), propertyoldvalue, myprofitalge);
                    break;
                case nameof(GTDRegisterClientVM.ProfitAlgR):
                    propertyoldvalue = myprofitalgr;
                    myprofitalgr += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.ProfitAlgR));
                    OnValueChanged(nameof(this.ProfitAlgR), propertyoldvalue, myprofitalgr);
                    break;
                case nameof(GTDRegisterClientVM.Selling):
                    propertyoldvalue = myselling;
                    myselling += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.Selling));
                    OnValueChanged(nameof(this.Selling), propertyoldvalue, myselling);
                    break;
                case nameof(GTDRegisterClientVM.SL):
                    propertyoldvalue = mysl;
                    mysl += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.SL));
                    OnValueChanged(nameof(this.SL), propertyoldvalue, mysl);
                    break;
                case nameof(GTDRegisterClientVM.Tax):
                    propertyoldvalue = mytax;
                    mytax += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.Tax));
                    OnValueChanged(nameof(this.Tax), propertyoldvalue, mytax);
                    break;
                case nameof(GTDRegisterClientVM.VAT):
                    propertyoldvalue = myvat;
                    myvat += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.Vat));
                    OnValueChanged(nameof(this.Vat), propertyoldvalue, myvat);
                    break;
                case nameof(GTDRegisterClientVM.Volume):
                    propertyoldvalue = myvolume;
                    myvolume += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.Volume));
                    OnValueChanged(nameof(this.Volume), propertyoldvalue, myvolume);
                    break;
                case nameof(GTDRegisterClientVM.WestGate):
                    propertyoldvalue = mywestgate;
                    mywestgate += newvalue - oldvalue;
                    PropertyChangedNotification(nameof(this.WestGate));
                    OnValueChanged(nameof(this.WestGate), propertyoldvalue, mywestgate);
                    break;
            }
        }
        protected override void ValuesReset()
        {
            myitemcount = 0;
            mycc = 0M;
            //myeurosum = 0M;
            myddspidy = 0M;
            mydtsum = 0M;
            myfee = 0M;
            mygtls = 0M;
            mygtlscur = 0M;
            myincomealg = 0M;
            mymfk = 0M;
            mypari = 0M;
            myprofitalge = 0M;
            myprofitalgr = 0M;
            myselling = 0M;
            mysl = 0M;
            mytax = 0M;
            myvat = 0M;
            myvolume = 0M;
            mywestgate = 0M;
        }
        protected override void ValuesPlus(GTDRegisterClientVM item)
        {
            myitemcount++;
            mycc += item.CC ?? 0M;
            myddspidy += item.DDSpidy ?? 0M;
            mydtsum += item.DTSum ?? 0M;
            myfee += item.Fee ?? 0M;
            mygtls += item.GTLS ?? 0M;
            mygtlscur += item.GTLSCur ?? 0M;
            myincomealg += item.IncomeAlg ?? 0M;
            mymfk += item.MFK ?? 0M;
            //myeurosum += item.EuroSum ?? 0M;
            mypari += item.Pari ?? 0M;
            myprofitalge += item.ProfitAlgE ?? 0M;
            myprofitalgr += item.ProfitAlgR ?? 0M;
            myselling += item.Selling ?? 0M;
            mysl += item.SL ?? 0M;
            mytax += item.Tax ?? 0M;
            myvat += item.VAT ?? 0M;
            myvolume += item.Volume ?? 0M;
            mywestgate += item.WestGate ?? 0M;
        }
        protected override void ValuesMinus(GTDRegisterClientVM item)
        {
            myitemcount--;
            mycc -= item.CC ?? 0M;
            myddspidy -= item.DDSpidy ?? 0M;
            mydtsum -= item.DTSum ?? 0M;
            myfee -= item.Fee ?? 0M;
            mygtls -= item.GTLS ?? 0M;
            mygtlscur -= item.GTLSCur ?? 0M;
            myincomealg -= item.IncomeAlg ?? 0M;
            mymfk -= item.MFK ?? 0M;
            //myeurosum -= item.EuroSum ?? 0M;
            mypari -= item.Pari ?? 0M;
            myprofitalge -= item.ProfitAlgE ?? 0M;
            myprofitalgr -= item.ProfitAlgR ?? 0M;
            myselling -= item.Selling ?? 0M;
            mysl -= item.SL ?? 0M;
            mytax -= item.Tax ?? 0M;
            myvat -= item.VAT ?? 0M;
            myvolume -= item.Volume ?? 0M;
            mywestgate -= item.WestGate ?? 0M;
        }
        protected override void PropertiesChangedNotifycation()
        {
            this.PropertyChangedNotification("ItemCount");
            this.PropertyChangedNotification(nameof(this.CC));
            this.PropertyChangedNotification(nameof(this.DDSpidy));
            this.PropertyChangedNotification(nameof(this.DTSum));
            this.PropertyChangedNotification(nameof(this.Fee));
            this.PropertyChangedNotification(nameof(this.GTLS));
            this.PropertyChangedNotification(nameof(this.GTLSCur));
            this.PropertyChangedNotification(nameof(this.IncomeAlg));
            this.PropertyChangedNotification(nameof(this.MFK));
            //this.PropertyChangedNotification(nameof(this.EuroSum));
            this.PropertyChangedNotification(nameof(this.Pari));
            this.PropertyChangedNotification(nameof(this.ProfitAlgE));
            this.PropertyChangedNotification(nameof(this.ProfitAlgR));
            this.PropertyChangedNotification(nameof(this.Selling));
            this.PropertyChangedNotification(nameof(this.SL));
            this.PropertyChangedNotification(nameof(this.Tax));
            this.PropertyChangedNotification(nameof(this.Vat));
            this.PropertyChangedNotification(nameof(this.Volume));
            this.PropertyChangedNotification(nameof(this.WestGate));
        }

        public event ValueChangedEventHandler<decimal> ValueChanged;
        public void OnValueChanged(string propertyname, decimal oldvalue, decimal newvalue)
        {
            if (ValueChanged != null)
                ValueChanged(this, new lib.Interfaces.ValueChangedEventArgs<decimal>(propertyname, oldvalue, newvalue));
        }
    }

}
