using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Data.SqlClient;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Specification;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Data;
using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using System.Threading;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
	public struct GTDRegisterRecord
	{
		internal int id;
		internal long stamp;
		internal DateTime? updatewhen;
		internal string updatewho;

		internal int agent;
		internal int amount;
		internal int cellnumber;
		internal decimal clientsumdiff;
		internal string consolidate;
		internal decimal cost;
		internal decimal? ddspidy;
		internal int? declaration;
		internal string filepath;
		internal decimal fondsum;
		internal decimal grossweight;
		internal decimal? gtls;
		internal decimal? gtlscur;
		internal decimal? gtlsrate;
		internal int importer;
		internal decimal? mfk;
		internal decimal netweight;
		internal int parcel;
		internal int? parcelgroup;
		internal decimal? pari;

		internal int? request;
		internal int spec;
		internal string servicetype;
		internal decimal? westgate;
	}

	public class GTDRegister : lib.DomainStampValueChanged
	{
		public GTDRegister(int id, long stamp, DateTime? updatewhen, string updatewho, lib.DomainObjectState state
			, Specification.Specification spec, string servicetype
			, bool isloaded = true) : this(id, stamp, updatewhen, updatewho, state, isloaded)
		{
			myspec = spec;
			myspec.PropertyChanged += this.Specification_PropertyChanged;
			myspec.Declaration.PropertyChanged += this.Declaration_PropertyChanged;
			//myspec.Declaration.ValueChanged += this.Declaration_ValueChanged;
			myspec.Parcel.PropertyChanged += this.Parcel_PropertyChanged;
			myservicetype = servicetype;
		}


		private GTDRegister(int id, long stamp, DateTime? updatewhen, string updatewho, lib.DomainObjectState state
			, bool isloaded = true) : base(id, stamp, updatewhen, updatewho, state, isloaded)
		{
			myclients = App.Current.Dispatcher.Invoke<ObservableCollection<GTDRegisterClient>>(() => { return new ObservableCollection<GTDRegisterClient>(); });
			//myclienttotals = App.Current.Dispatcher.Invoke<List<GTDRegisterClientTotal>>(() => { return new List<GTDRegisterClientTotal>(); });
		}

		private Specification.Specification myspec;
		public Specification.Specification Specification
		{
			set { myspec = value; }
			get { return this.IsLoaded ? myspec : null; }
		}
		public decimal? CC
		{ get { return myclients.Sum((GTDRegisterClient item) => { return item.CC ?? 0M; }); } }
		public CustomerLegal Client
		{ get { return myclients.Count > 1 ? null : myclients.Min((GTDRegisterClient item) => { return item.Client; }); } }
		public decimal? CostLogistics
		{ get { return (this.Specification.Pari ?? 0M) + (this.SLWithoutRate ?? 0M) + (this.Specification.GTLS ?? 0M) + (this.Specification.DDSpidy ?? 0M) + (this.Specification.WestGateWithoutRate ?? 0M) + (this.Specification.MFK ?? 0M); } }
		public decimal? CostPer
		{ get { return (this.SellingWithoutRate ?? 0) > 0M ? this.CostTotal / this.SellingWithoutRate : null; } }
		public decimal? CostTotal
		{ get { return this.Specification.Declaration.Fee + this.Specification.Declaration.Tax + this.CC + this.CostLogistics; } }
		public decimal? DifProfitIncomeAlg
		{ get { return this.Profit - this.IncomeAlg; } }
		public decimal? DTSum
		{ get { return myclients.Sum((GTDRegisterClient item) => { return item.DTSum ?? 0M; }); } }
		public decimal? DTSumRub
		{ get { return myspec.Declaration?.CBRate * myspec.Declaration?.TotalSum; } }
		public decimal? IncomeAlg
		{ get { return myclients.Sum((GTDRegisterClient item) => { return item.IncomeAlg; }); } }
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
			get
			{
				decimal? value;
				if (myservicetype == "ТД")
					value = (this.CC ?? 0M) > 0M ? (this.Selling - this.CC) / this.CC : null;
				else
					value = (this.DTSumRub ?? 0M) > 0M ? this.SellingWithoutRate / this.DTSumRub : null;
				return value;
			}
		}
		public decimal? MarkupBU
		{
			get { return (this.CC ?? 0M) > 0M ? (this.SellingWithoutRate - this.CC) / this.CC : null; }
		}
		public decimal? MarkupTotal
		{
			get
			{
				decimal? value;
				if (this.Specification.Importer.Id == 1)
					value = (this.CostLogistics ?? 0M) > 0M ? (this.Selling - this.CostLogistics) / this.CostLogistics : null;
				else
					value = (this.CostTotal ?? 0M) > 0M ? (this.SellingWithoutRate - this.CostTotal) / this.CostTotal : null;
				return value;
			}
		}
		public decimal? Profit
		{ get { return this.SellingWithoutRate - (myservicetype == "ТД" ? (this.CostTotal ?? 0M) : this.CostLogistics); } }
		public decimal? Profitability
		{ get { return this.Profit.HasValue && (this.SellingWithoutRate ?? 0M) != 0M ? decimal.Divide(this.Profit.Value, this.SellingWithoutRate.Value) : (decimal?)null; } }
		public decimal? ProfitAlgE
		{ get { return myservicetype == "ТД" ? myclients.Sum((GTDRegisterClient item) => { return item.ProfitAlgE; }) : (decimal?)null; } }
		public decimal? ProfitAlgR
		{ get { return myservicetype == "ТД" && this.Specification.Declaration?.CBRate != null ? myclients.Sum((GTDRegisterClient item) => { return item.ProfitAlgR; }) : (decimal?)null; } }
		public decimal? ProfitDiff
		{ get { return myservicetype == "ТД" ? this.Profit - this.ProfitAlgR : (decimal?)null; } }
		public int Rate
		{ get { return myclients.Count > 1 ? 2 : myclients.Count; } }
		public decimal? SL
		{
			set { if (myclients.Count == 1) myclients[0].SL = value; }
			get { return myclients.Sum((GTDRegisterClient item) => { return item.SL ?? 0M; }); }
		}
		public decimal? SLRate
		{ get { return myclients.Sum((GTDRegisterClient item) => { return item.SLRate ?? 0M; }); } }
		public decimal? SLWithoutRate
		{ get { return myclients.Sum((GTDRegisterClient item) => { return item.SLWithoutRate ?? 0M; }); } }
		public decimal? Selling
		{
			get
			{
				decimal? selling;
				if (myservicetype == "ТД")
					selling = myclients.Sum((GTDRegisterClient item) => { return item.Selling ?? 0M; });
				else
				{
					selling = myclients.FirstOrDefault()?.AlgValue2 * this.Specification?.Declaration.CBRate * this.Specification?.Declaration.TotalSum;
					if (selling.HasValue) selling = decimal.Round(selling.Value);
				}
				return selling;
			}
		}
		public DateTime? SellingDate
		{
			set
			{
				foreach (GTDRegisterClient item in myclients)
					item.SellingDate = value;
			}
			get { return myclients.Count == 0 ? null : myclients.Max((GTDRegisterClient item) => { return item.SellingDate; }); }
		}
		public decimal? SellingRate
		{ get { return myservicetype == "ТД" ? myclients.Sum((GTDRegisterClient item) => { return item.SellingRate ?? 0M; }) : 0M; } }
		public decimal? SellingWithoutRate
		{ get { return myservicetype == "ТД" ? myclients.Sum((GTDRegisterClient item) => { return item.SellingWithoutRate ?? 0M; }) : this.Selling; } }
		private string myservicetype;
		public string ServiceType
		{ get { return myservicetype; } }
		public decimal? VATPay
		{ get { return this.SellingRate - this.Specification.Declaration.VAT - this.SLRate - (this.Specification.WestGateRate ?? 0M) - (this.Specification.MFKRate ?? 0M); } }
		public decimal? Volume
		{
			set { if (myclients.Count == 1) myclients[0].Volume = value; }
			get { return myclients.Count > 1 ? null : myclients.Sum((GTDRegisterClient item) => { return item.Volume; }); }
		}
		public decimal? VolumeProfit
		{ get { return myclients.Count == 1 && (this.Volume ?? 0M) > 0M && this.Profit.HasValue ? decimal.Divide(this.Profit.Value, this.Volume.Value) : (decimal?)null; } }

		private ObservableCollection<GTDRegisterClient> myclients;
		public ObservableCollection<GTDRegisterClient> Clients
		{
			//internal set
			//{
			//    myclients = value;
			//    foreach (GTDRegisterClient item in myclients)
			//        item.PropertyChanged += this.Client_PropertyChanged;
			//    this.PropertyChangedNotification(nameof(this.Clients));
			//    for (int i = 0; i < myclienttotals.Count; i++) myclienttotals[i].StartCount();
			//}
			get { return myclients; }
		}
		//private List<GTDRegisterClientTotal> myclienttotals;
		//internal List<GTDRegisterClientTotal> ClientTotals
		//{ get { return myclienttotals; } }

		public override bool IsDirty
		{
			get
			{
				bool dirty = base.IsDirty;
				if (!dirty && myspec != null)
					dirty |= myspec.IsDirty;
				if (!dirty && myclients != null)
					foreach (GTDRegisterClient item in myclients)
						if (item.IsDirty)
						{ dirty = true; break; }
				return dirty;
			}
		}

		protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
		{
			this.Specification.UpdateProperties((sample as GTDRegister).Specification);
			ManagersRefresh();
		}
		protected override void RejectProperty(string property, object value)
		{
		}
		private void Declaration_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(Declaration.Fee):
				case nameof(Declaration.Tax):
					this.PropertyChangedNotification(nameof(this.CostLogistics));
					this.PropertyChangedNotification(nameof(this.CostPer));
					this.PropertyChangedNotification(nameof(this.CostTotal));
					this.PropertyChangedNotification(nameof(this.MarkupTotal));
					this.PropertyChangedNotification(nameof(this.Profit));
					this.PropertyChangedNotification(nameof(this.Profitability));
					this.PropertyChangedNotification(nameof(this.VATPay));
					break;
				case nameof(Declaration.CBRate):
					this.PropertyChangedNotification(nameof(this.CostPer));
					this.PropertyChangedNotification(nameof(this.DTSumRub));
					this.PropertyChangedNotification(nameof(this.MarkupAlg));
					this.PropertyChangedNotification(nameof(this.MarkupBU));
					this.PropertyChangedNotification(nameof(this.MarkupTotal));
					this.PropertyChangedNotification(nameof(this.Profit));
					this.PropertyChangedNotification(nameof(this.Profitability));
					this.PropertyChangedNotification(nameof(this.ProfitAlgE));
					this.PropertyChangedNotification(nameof(this.ProfitAlgR));
					this.PropertyChangedNotification(nameof(this.ProfitDiff));
					this.PropertyChangedNotification(nameof(this.Selling));
					this.PropertyChangedNotification(nameof(this.SellingWithoutRate));
					this.PropertyChangedNotification(nameof(this.VolumeProfit));
					break;
				case nameof(Declaration.TotalSum):
					this.PropertyChangedNotification(nameof(this.CostPer));
					this.PropertyChangedNotification(nameof(this.DTSumRub));
					this.PropertyChangedNotification(nameof(this.MarkupAlg));
					this.PropertyChangedNotification(nameof(this.MarkupBU));
					this.PropertyChangedNotification(nameof(this.MarkupTotal));
					this.PropertyChangedNotification(nameof(this.Profit));
					this.PropertyChangedNotification(nameof(this.Profitability));
					this.PropertyChangedNotification(nameof(this.ProfitDiff));
					this.PropertyChangedNotification(nameof(this.Selling));
					this.PropertyChangedNotification(nameof(this.SellingWithoutRate));
					this.PropertyChangedNotification(nameof(this.VolumeProfit));
					break;
				case nameof(Declaration.VAT):
					this.PropertyChangedNotification(nameof(this.VATPay));
					break;
			}
		}
		private void Declaration_ValueChanged(object sender, lib.Interfaces.ValueChangedEventArgs<object> e)
		{
			switch (e.PropertyName)
			{
				case nameof(Declaration.CBRate):
					this.OnValueChanged(nameof(this.DTSumRub), ((decimal?)e.OldValue ?? 0M) * myspec.Declaration?.TotalSum, ((decimal?)e.NewValue ?? 0M) * myspec.Declaration?.TotalSum);
					break;
				case nameof(Declaration.TotalSum):
					this.OnValueChanged(nameof(this.DTSum), myspec.Declaration?.TotalSum, myspec.Declaration?.TotalSum);
					this.OnValueChanged(nameof(this.DTSumRub), ((decimal?)e.OldValue ?? 0M) * myspec.Declaration?.TotalSum, ((decimal?)e.NewValue ?? 0M) * myspec.Declaration?.TotalSum);
					break;
			}

		}
		private void Parcel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(Parcel.UsdRate):
					this.Specification.PropertyChangedNotification(nameof(Specification.GTLS));
					break;
				case nameof(Parcel.Requests):
					this.ManagersRefresh();
					break;
			}
		}
		private void Specification_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(Specification.DDSpidy):
				case nameof(Specification.GTLS):
				case nameof(Specification.MFK):
				case nameof(Specification.Pari):
				case nameof(Specification.WestGate):
					this.PropertyChangedNotification(nameof(this.CostLogistics));
					this.PropertyChangedNotification(nameof(this.CostPer));
					this.PropertyChangedNotification(nameof(this.CostTotal));
					this.PropertyChangedNotification(nameof(this.MarkupTotal));
					this.PropertyChangedNotification(nameof(this.Profit));
					this.PropertyChangedNotification(nameof(this.Profitability));
					this.PropertyChangedNotification(nameof(this.VATPay));
					break;
				case nameof(Specification.Declaration):
					myspec.Declaration.PropertyChanged += this.Declaration_PropertyChanged;
					this.PropertyChangedNotification(nameof(this.CostLogistics));
					this.PropertyChangedNotification(nameof(this.CostPer));
					this.PropertyChangedNotification(nameof(this.CostTotal));
					this.PropertyChangedNotification(nameof(this.MarkupTotal));
					this.PropertyChangedNotification(nameof(this.Profit));
					this.PropertyChangedNotification(nameof(this.Profitability));
					this.PropertyChangedNotification(nameof(this.ProfitAlgE));
					this.PropertyChangedNotification(nameof(this.ProfitAlgR));
					this.PropertyChangedNotification(nameof(this.ProfitDiff));
					this.PropertyChangedNotification(nameof(this.VATPay));
					break;
			}
		}
		internal void Client_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(GTDRegisterClient.CC):
					this.PropertyChangedNotification(nameof(this.CC));
					this.PropertyChangedNotification(nameof(this.CostPer));
					this.PropertyChangedNotification(nameof(this.CostTotal));
					this.PropertyChangedNotification(nameof(this.MarkupBU));
					this.PropertyChangedNotification(nameof(this.MarkupAlg));
					this.PropertyChangedNotification(nameof(this.MarkupTotal));
					this.PropertyChangedNotification(nameof(this.Profit));
					this.PropertyChangedNotification(nameof(this.Profitability));
					this.PropertyChangedNotification(nameof(this.ProfitDiff));
					break;
				case nameof(GTDRegisterClient.DTSum):
					this.PropertyChangedNotification(nameof(this.DTSum));
					break;
				case nameof(GTDRegisterClient.ProfitAlgE):
					this.PropertyChangedNotification(nameof(this.ProfitAlgE));
					this.PropertyChangedNotification(nameof(this.ProfitAlgR));
					this.PropertyChangedNotification(nameof(this.ProfitDiff));
					break;
				case nameof(GTDRegisterClient.Selling):
					this.PropertyChangedNotification(nameof(this.CostPer));
					this.PropertyChangedNotification(nameof(this.MarkupBU));
					this.PropertyChangedNotification(nameof(this.MarkupAlg));
					this.PropertyChangedNotification(nameof(this.MarkupTotal));
					this.PropertyChangedNotification(nameof(this.Selling));
					this.PropertyChangedNotification(nameof(this.SellingRate));
					this.PropertyChangedNotification(nameof(this.SellingWithoutRate));
					this.PropertyChangedNotification(nameof(this.Profit));
					this.PropertyChangedNotification(nameof(this.Profitability));
					this.PropertyChangedNotification(nameof(this.ProfitDiff));
					this.PropertyChangedNotification(nameof(this.VATPay));
					break;
				case nameof(GTDRegisterClient.SL):
					this.PropertyChangedNotification(nameof(this.CostLogistics));
					this.PropertyChangedNotification(nameof(this.CostPer));
					this.PropertyChangedNotification(nameof(this.CostTotal));
					this.PropertyChangedNotification(nameof(this.MarkupTotal));
					this.PropertyChangedNotification(nameof(this.SL));
					this.PropertyChangedNotification(nameof(this.SLRate));
					this.PropertyChangedNotification(nameof(this.SLWithoutRate));
					this.PropertyChangedNotification(nameof(this.Profit));
					this.PropertyChangedNotification(nameof(this.Profitability));
					this.PropertyChangedNotification(nameof(this.ProfitDiff));
					this.PropertyChangedNotification(nameof(this.VATPay));
					break;
				case nameof(GTDRegisterClient.Volume):
					this.PropertyChangedNotification(nameof(this.Volume));
					this.PropertyChangedNotification(nameof(this.VolumeProfit));
					break;
			}
		}
		private void ManagersRefresh()
		{
			if (mymanagers == null) return;
			mymanagers.Clear();
			foreach (Request request in this.Specification.Requests)
				if (request.Manager != null && !mymanagers.Contains(request.Manager))
					mymanagers.Add(request.Manager);
			this.PropertyChangedNotification(nameof(GTDRegister.Managers));
		}
		internal void Unbind()
		{
			myspec.PropertyChanged -= this.Specification_PropertyChanged;
			myspec.Declaration.PropertyChanged -= this.Declaration_PropertyChanged;
			//myspec.Declaration.ValueChanged -= this.Declaration_ValueChanged;
			myspec.Parcel.PropertyChanged -= this.Parcel_PropertyChanged;
			foreach (GTDRegisterClient item in myclients)
			{
				item.PropertyChanged -= this.Client_PropertyChanged;
				item.Unbind();
			}
		}
	}

	internal class GTDRegisterStore : lib.DomainStorageLoad<GTDRegisterRecord, GTDRegister, GTDRegisterDBM>
	{
		public GTDRegisterStore(GTDRegisterDBM dbm) : base(dbm) { }

		protected override void UpdateProperties(GTDRegister olditem, GTDRegister newitem)
		{
			olditem.UpdateProperties(newitem);
		}
	}

	public class GTDRegisterDBM : lib.DBManagerStamp<GTDRegisterRecord, GTDRegister>
	{
		public GTDRegisterDBM()
		{
			ConnectionString = CustomBrokerWpf.References.ConnectionString;
			base.NeedAddConnection = true;

			SelectCommandText = "spec.Specification_sp";
			UpdateCommandText = "spec.SpecificationUpd_sp";

			SelectParams = new SqlParameter[] {
				new SqlParameter("@id", System.Data.SqlDbType.Int),
				new SqlParameter("@importerid", System.Data.SqlDbType.Int),
				new SqlParameter("@consolidate", System.Data.SqlDbType.NVarChar,5),
				new SqlParameter("@filterid", System.Data.SqlDbType.Int),
				new SqlParameter("@parcelid", System.Data.SqlDbType.Int),
				new SqlParameter("@parcelgroup", System.Data.SqlDbType.Int),
				new SqlParameter("@requestid", System.Data.SqlDbType.Int),
				new SqlParameter("@servicetype", System.Data.SqlDbType.NVarChar,10)
			};
			UpdateParams = new SqlParameter[] { UpdateParams[0]
				, new SqlParameter("@filepathtrue", System.Data.SqlDbType.Bit)
				, new SqlParameter("@declarationidtrue", System.Data.SqlDbType.Bit)
				, new SqlParameter("@paritrue", System.Data.SqlDbType.Bit)
				, new SqlParameter("@gtlstrue", System.Data.SqlDbType.Bit)
				, new SqlParameter("@ddspidytrue", System.Data.SqlDbType.Bit)
				, new SqlParameter("@westgatetrue", System.Data.SqlDbType.Bit)
				, new SqlParameter("@mfktrue", System.Data.SqlDbType.Bit)
			};
			InsertUpdateParams = new SqlParameter[] {
				  new SqlParameter("@filepath", System.Data.SqlDbType.NVarChar,200)
				, new SqlParameter("@declarationid", System.Data.SqlDbType.Int)
				, new SqlParameter("@pari", System.Data.SqlDbType.Money)
				, new SqlParameter("@gtls", System.Data.SqlDbType.Money)
				, new SqlParameter("@ddspidy", System.Data.SqlDbType.Money)
				, new SqlParameter("@westgate", System.Data.SqlDbType.Money)
				, new SqlParameter("@mfk", System.Data.SqlDbType.Money)
			};
			mysdbm = new SpecificationDBM();
			mytddbm = new DeclarationDBM();
			myratedbm = new SpecificationCustomerInvoiceRateDBM();
			mypldbm = new ParcelDBM();
		}

		private Importer myimporter;
		internal Importer Importer
		{ set { myimporter = value; } get { return myimporter; } }
		private string myservicetype;
		public string ServiceType
		{ set { myservicetype = value; } get { return myservicetype; } }
		private lib.SQLFilter.SQLFilter myfilter;
		internal lib.SQLFilter.SQLFilter Filter
		{ set { myfilter = value; } get { return myfilter; } }
		private Specification.SpecificationDBM mysdbm;
		private Specification.DeclarationDBM mytddbm;
		private SpecificationCustomerInvoiceRateDBM myratedbm;
		private GTDRegisterClientDBM mycdbm;
		internal GTDRegisterClientDBM ClientDBM
		{ set { mycdbm = value; mycdbm.Command.CommandTimeout = 10000; } get { return mycdbm; } }
		private ParcelDBM mypldbm;

		protected override GTDRegisterRecord CreateRecord(SqlDataReader reader)
		{
			return new GTDRegisterRecord()
			{
				id = reader.GetInt32(0), stamp = reader.GetInt64(this.Fields["stamp"])
				, agent = reader.GetInt32(this.Fields["agentid"])
				, consolidate = reader.IsDBNull(this.Fields["consolidate"]) ? null : reader.GetString(this.Fields["consolidate"])
				, declaration = reader.IsDBNull(this.Fields["declarationid"]) ? (int?)null : reader.GetInt32(this.Fields["declarationid"])
				, filepath = reader.IsDBNull(this.Fields["filepath"]) ? null : reader.GetString(this.Fields["filepath"])
				, importer = reader.GetInt32(this.Fields["importerid"])
				, parcel = reader.GetInt32(this.Fields["parcelid"])
				, parcelgroup = reader.IsDBNull(this.Fields["parcelgroup"]) ? (int?)null : reader.GetInt32(this.Fields["parcelgroup"])
				, request = reader.IsDBNull(this.Fields["requestid"]) ? (int?)null : reader.GetInt32(this.Fields["requestid"])
				, pari = reader.IsDBNull(this.Fields["pari"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["pari"])
				, gtls = reader.IsDBNull(this.Fields["gtls"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["gtls"])
				, gtlscur = reader.IsDBNull(this.Fields["gtlscur"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["gtlscur"])
				, gtlsrate = reader.IsDBNull(this.Fields["gtlsrate"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["gtlsrate"])
				, ddspidy = reader.IsDBNull(this.Fields["ddspidy"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["ddspidy"])
				, westgate = reader.IsDBNull(this.Fields["westgate"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["westgate"])
				, mfk = reader.IsDBNull(this.Fields["mfk"]) ? (decimal?)null : (decimal)reader.GetDecimal(this.Fields["mfk"])
				, amount = reader.IsDBNull(this.Fields["amount"]) ? 0 : reader.GetInt32(this.Fields["amount"])
				, cellnumber = reader.IsDBNull(this.Fields["cellnumber"]) ? 0 : (int)reader.GetDecimal(this.Fields["cellnumber"])
				, clientsumdiff = reader.IsDBNull(this.Fields["clientsumdiff"]) ? 0M : reader.GetDecimal(this.Fields["clientsumdiff"])
				, cost = reader.IsDBNull(this.Fields["cost"]) ? 0M : reader.GetDecimal(this.Fields["cost"])
				, fondsum = reader.IsDBNull(this.Fields["fondsum"]) ? 0M : reader.GetDecimal(this.Fields["fondsum"])
				, grossweight = reader.IsDBNull(this.Fields["grossweight"]) ? 0M : reader.GetDecimal(this.Fields["grossweight"])
				, netweight = reader.IsDBNull(this.Fields["netweight"]) ? 0M : reader.GetDecimal(this.Fields["netweight"])
			};
		}
		protected override GTDRegister CreateModel(GTDRegisterRecord record, SqlConnection addcon, CancellationToken canceltasktoken = default)
		{
			List<lib.DBMError> errors;
			Agent agent = CustomBrokerWpf.References.AgentStore.GetItemLoad(record.agent, addcon, out errors);
			this.Errors.AddRange(errors);
			Declaration declaration = null;
			if (record.declaration.HasValue)
			{
				mytddbm.Errors.Clear();
				mytddbm.Command.Connection = addcon;
				mytddbm.ItemId = record.declaration.Value;
				declaration = mytddbm.GetFirst();
			}
			errors.Clear();
			Parcel parcel = CustomBrokerWpf.References.ParcelStore.GetItemLoad(record.parcel, addcon, out errors);
			this.Errors.AddRange(errors);
			Request request = null;
			if (record.request.HasValue)
			{
				errors.Clear();
				request = CustomBrokerWpf.References.RequestStore.GetItemLoad(record.request.Value, addcon, out errors);
				this.Errors.AddRange(errors);
			}

			Specification.Specification spec = new Specification.Specification(record.id, record.stamp, lib.DomainObjectState.Unchanged
				, agent
				, record.consolidate
				, declaration
				, record.filepath
				, CustomBrokerWpf.References.Importers.FindFirstItem("Id", record.importer)
				, parcel
				, record.parcelgroup
				, request
				, record.pari
				, record.gtls
				, record.gtlscur
				, record.gtlsrate
				, record.ddspidy
				, record.westgate
				, record.mfk
				, record.amount
				, record.cellnumber
				, record.clientsumdiff
				, record.cost
				, record.fondsum
				, record.grossweight
				, record.netweight
				);
			spec = CustomBrokerWpf.References.SpecificationStore.UpdateItem(spec);
			GTDRegister gtd = new GTDRegister(spec.Id, spec.Stamp, spec.UpdateWhen, spec.UpdateWho, spec.DomainState, spec, myservicetype, true);
			if (!canceltasktoken.IsCancellationRequested & mycdbm != null)
			{
				mycdbm.Errors.Clear();
				mycdbm.Command.Connection = addcon;
				mycdbm.GTD = gtd;
				mycdbm.Collection = gtd.Clients;
				foreach (GTDRegisterClient item in gtd.Clients)
					item.PropertyChanged -= gtd.Client_PropertyChanged;
				if (!canceltasktoken.IsCancellationRequested) mycdbm.Fill();
				foreach (GTDRegisterClient item in gtd.Clients)
					item.PropertyChanged += gtd.Client_PropertyChanged;
				mycdbm.Collection = null;
				if (mycdbm.Errors.Count > 0) foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
			}
			return gtd;
		}
		protected override bool SaveChildObjects(GTDRegister item)
		{
			bool issuccess = true;
			if (mycdbm != null && item.Clients.Count > 0)
			{
				mycdbm.Errors.Clear();
				mycdbm.Collection = item.Clients;
				if (!mycdbm.SaveCollectionChanches())
				{
					issuccess = false;
					foreach (lib.DBMError err in mycdbm.Errors) this.Errors.Add(err);
				}
			}
			return issuccess;
		}
		protected override bool SaveIncludedObject(GTDRegister item)
		{
			bool success = true;
			mysdbm.Errors.Clear();
			if (!mysdbm.SaveItemChanches(item.Specification))
			{
				foreach (lib.DBMError err in mysdbm.Errors) this.Errors.Add(err);
				success = false;
			}
			mypldbm.Errors.Clear();
			if (!mypldbm.SaveItemChanches(item.Specification.Parcel))
			{
				foreach (lib.DBMError err in mypldbm.Errors) this.Errors.Add(err);
				success = false;
			}
			return success;
		}
		protected override bool SaveReferenceObjects()
		{
			mysdbm.Command.Connection = this.Command.Connection;
			if (mycdbm != null) mycdbm.Command.Connection = this.Command.Connection;
			mypldbm.Command.Connection = this.Command.Connection;
			return true;
		}
		protected override void SetSelectParametersValue()
		{
			foreach (SqlParameter par in SelectParams)
				switch (par.ParameterName)
				{
					case "@importerid":
						par.Value = myimporter?.Id;
						break;
					case "@filterid":
						par.Value = myfilter?.FilterWhereId;
						break;
					case "@servicetype":
						par.Value = myservicetype;
						break;
				}
		}
		protected override bool SetParametersValue(GTDRegister item)
		{
            base.SetParametersValue(item);
			foreach (SqlParameter par in this.UpdateParams)
			{
				switch (par.ParameterName)
				{
					case "@paritrue":
						par.Value = item.Specification.HasPropertyOutdatedValue(nameof(Specification.Specification.Pari));
						break;
					case "@gtlstrue":
						par.Value = item.Specification.HasPropertyOutdatedValue(nameof(Specification.Specification.GTLS));
						break;
					case "@gtlscurtrue":
						par.Value = item.Specification.HasPropertyOutdatedValue(nameof(Specification.Specification.GTLSCur));
						break;
					case "@gtlsratetrue":
						par.Value = item.Specification.HasPropertyOutdatedValue(nameof(Specification.Specification.GTLSRate));
						break;
					case "@ddspidytrue":
						par.Value = item.Specification.HasPropertyOutdatedValue(nameof(Specification.Specification.DDSpidy));
						break;
					case "@westgatetrue":
						par.Value = item.Specification.HasPropertyOutdatedValue(nameof(Specification.Specification.WestGate));
						break;
					case "@mfktrue":
						par.Value = item.Specification.HasPropertyOutdatedValue(nameof(Specification.Specification.MFK));
						break;
				}
			}
			foreach (SqlParameter par in this.InsertUpdateParams)
			{
				switch (par.ParameterName)
				{
					case "@pari":
						par.Value = item.Specification.Pari;
						break;
					case "@gtls":
						par.Value = item.Specification.GTLS;
						break;
					case "@gtlscur":
						par.Value = item.Specification.GTLSCur;
						break;
					case "@gtlsrate":
						par.Value = item.Specification.GTLSRate;
						break;
					case "@ddspidy":
						par.Value = item.Specification.DDSpidy;
						break;
					case "@westgate":
						par.Value = item.Specification.WestGate;
						break;
					case "@mfk":
						par.Value = item.Specification.MFK;
						break;
				}
			}
			return true;
		}
		//protected override void CancelLoad()
		//{
		//    if(mycdbm!=null) mycdbm.CancelingLoad = this.CancelingLoad;
		//    mypldbm.CancelingLoad = this.CancelingLoad;
		//    mysdbm.CancelingLoad = this.CancelingLoad;
		//    mytddbm.CancelingLoad = this.CancelingLoad;
		//    myratedbm.CancelingLoad = this.CancelingLoad;
		//}
	}

	public class GTDRegisterVM : lib.ViewModelErrorNotifyItem<GTDRegister>, ITotalValuesItem
	{
		internal GTDRegisterVM(GTDRegister model) : base(model)
		{
			myclientsynchronizer = new GTDRegisterClientSynchronizer();
			InitProperties();
		}

		private Specification.SpecificationVM myspec;
		public Specification.SpecificationVM Specification
		{ get { return this.IsLoaded && this.IsEnabled ? myspec : null; } }
		public decimal? CC
		{ get { return this.DomainObject.CC; } }
		public CustomerLegal Client
		{ get { return this.DomainObject.Client; } }
		public System.Windows.Visibility ClientsVisible
		{ get { return this.DomainObject.Clients.Count > 1 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; } }
		public decimal? CostLogistics
		{ get { return this.DomainObject.CostLogistics; } }
		public decimal? CostPer
		{ get { return this.DomainObject.CostPer; } }
		public decimal? CostTotal
		{ get { return this.DomainObject.CostTotal; } }
		public decimal? DifProfitIncomeAlg
		{ get { return this.IsEnabled ? this.DomainObject.DifProfitIncomeAlg : (decimal?)null; } }
		public decimal? DTSum
		{ get { return this.DomainObject.DTSum; } }
		public decimal? DTSumRub
		{ get { return this.DomainObject.DTSumRub; } }
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
		public decimal? Profit
		{ get { return this.DomainObject.Profit; } }
		public decimal? ProfitAlgE
		{ get { return this.IsEnabled ? this.DomainObject.ProfitAlgE : (decimal?)null; } }
		public decimal? ProfitAlgR
		{ get { return this.IsEnabled ? this.DomainObject.ProfitAlgR : (decimal?)null; } }
		public decimal? ProfitDiff
		{ get { return this.IsEnabled ? this.DomainObject.ProfitDiff : (decimal?)null; } }
		public decimal? Profitability
		{ get { return this.DomainObject.Profitability; } }
		public int Rate
		{ get { return this.DomainObject.Rate; } }
		public decimal? SL
		{
			set
			{
				if (!this.IsReadOnly && (this.DomainObject.SL.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.SL.Value, value.Value))))
				{
					string name = nameof(this.SL);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SL);
					ChangingDomainProperty = name; this.DomainObject.SL = value;
				}
			}
			get { return this.DomainObject.SL; }
		}
		public decimal? SLRate
		{ get { return this.DomainObject.SLRate; } }
		public decimal? SLWithoutRate
		{ get { return this.DomainObject.SLWithoutRate; } }
		public decimal? Selling
		{ get { return this.DomainObject.Selling; } }
		public DateTime? SellingDate
		{
			set
			{
				if (!this.IsReadOnly && (this.DomainObject.SellingDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.SellingDate.Value, value.Value))))
				{
					string name = "SellingDate";
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SellingDate);
					ChangingDomainProperty = name; this.DomainObject.SellingDate = value;
				}
			}
			get { return this.DomainObject.SellingDate; }
		}
		public decimal? SellingRate
		{ get { return this.DomainObject.SellingRate; } }
		public decimal? SellingWithoutRate
		{ get { return this.DomainObject.SellingWithoutRate; } }
		public decimal? VATPay
		{ get { return this.DomainObject.VATPay; } }
		public decimal? Volume
		{
			set
			{
				if (!this.IsReadOnly && (this.DomainObject.Volume.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Volume.Value, value.Value))))
				{
					string name = nameof(this.Volume);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Volume);
					ChangingDomainProperty = name; this.DomainObject.Volume = value;
				}
			}
			get { return this.DomainObject.Volume; }
		}
		public decimal? VolumeProfit
		{ get { return this.DomainObject.VolumeProfit; } }

		private GTDRegisterClientSynchronizer myclientsynchronizer;
		private ListCollectionView myclients;
		public ListCollectionView Clients
		{ get { return myclients; } }
		internal Predicate<object> ClientsFilter
		{ set { myclients.Filter = value; myclienttotal.CountAsinc(); } }
		private GTDRegisterClientTotal myclienttotal;
		public GTDRegisterClientTotal ClientTotal
		{ get { return myclienttotal; } }
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

		protected override bool DirtyCheckProperty()
		{
			return false;
		}
		protected override void DomainObjectPropertyChanged(string property)
		{
			switch (property)
			{
				case nameof(GTDRegister.IsLoaded):
					if (myspec == null)
						myspec = new Classes.Specification.SpecificationVM(this.DomainObject.Specification);
					else
						myspec.DomainObject = this.DomainObject.Specification;
					break;
				case nameof(GTDRegister.Managers):
					if (!myinitmanagers) this.InitManagers();
					break;
			}
		}
		protected override void InitProperties()
		{
			myspec = new Classes.Specification.SpecificationVM(this.DomainObject.Specification);
			this.InitManagers();
			myclientsynchronizer.DomainCollection = this.DomainObject.Clients;
			myclients = new ListCollectionView(myclientsynchronizer.ViewModelCollection);
			myclients.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
			myclients.SortDescriptions.Add(new System.ComponentModel.SortDescription("Client.Name", System.ComponentModel.ListSortDirection.Ascending));
			myclienttotal = new GTDRegisterClientTotal(myclients);
			myclienttotal.ValueChanged += (object sender, lib.Interfaces.ValueChangedEventArgs<decimal> e) => { this.OnValueChanged("Total" + e.PropertyName, e.OldValue, e.NewValue); };
			myclienttotal.StartCount();
		}
		protected override void RejectProperty(string property, object value)
		{
			switch (property)
			{
				case nameof(this.Volume):
					this.DomainObject.Volume = (decimal?)value;
					break;
				case "DependentOld":
					this.Specification.RejectChanges();
					break;
				case "DependentNew":
					int i = 0;
					GTDRegisterClientVM[] removed = new GTDRegisterClientVM[this.DomainObject.Clients.Count];
					foreach (GTDRegisterClientVM item in myclientsynchronizer.ViewModelCollection)
					{
						if (item.DomainState == lib.DomainObjectState.Added)
						{
							removed[i] = item;
							i++;
						}
						else
							item.RejectChanges();
					}
					foreach (GTDRegisterClientVM item in removed)
						if (item != null) myclientsynchronizer.ViewModelCollection.Remove(item);
					break;
			}
		}
		protected override bool ValidateProperty(string propertyname, bool inform = true)
		{
			return true;
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
			this.PropertyChangedNotification(nameof(GTDRegisterVM.Managers));
			myinitmanagers = false;
		}
	}

	public class GTDRegisterSynchronizer : lib.ModelViewCollectionsSynchronizer<GTDRegister, GTDRegisterVM>
	{
		private Predicate<object> myclientfilter;
		internal Predicate<object> ClientsFilter
		{ set { myclientfilter = value; } }

		protected override GTDRegister UnWrap(GTDRegisterVM wrap)
		{
			return wrap.DomainObject as GTDRegister;
		}
		protected override GTDRegisterVM Wrap(GTDRegister fill)
		{
			return new GTDRegisterVM(fill) { ClientsFilter = myclientfilter };
		}
	}

	public class GTDRegisterViewCommander : lib.ViewModelViewOnDemandCommand
	{
		internal GTDRegisterViewCommander(Importer importer, string servicetype) : base()
		{
			mymaindbm = new GTDRegisterDBM();
			mydbm = mymaindbm;
			mymaindbm.Importer = importer;
			mymaindbm.ServiceType = servicetype;
			mymaindbm.ClientDBM = new GTDRegisterClientDBM() { ServiceType = servicetype };
			mymaindbm.FillAsyncCompleted = () =>
			{
				if (mymaindbm.Errors.Count > 0) OpenPopup(mymaindbm.ErrorMessage, true);
				myview.SortDescriptions.Clear();
				myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Specification.Parcel.ParcelNumberOrder", System.ComponentModel.ListSortDirection.Ascending));
				myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Specification.Agent.Name", System.ComponentModel.ListSortDirection.Ascending));
				mytotal.StartCount();
			};
			mysync = new GTDRegisterSynchronizer() { ClientsFilter = this.ClientsFilter };
			mytdload = new RelayCommand(TDLoadExec, TDLoadCanExec);

			#region Filter
			myagentfilter = new GTDRegisterAgentCheckListBoxVMFillDefault();
			myagentfilter.DeferredFill = true;
			myagentfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
			myagentfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myagentfilter.ExecCommand2 = () => { myagentfilter.Clear(); };
			myagentfilter.FillDefault = () =>
			{
				bool empty = this.FilterEmpty;
				if (empty)
					foreach (lib.ReferenceSimpleItem item in CustomBrokerWpf.References.AgentNames)
						myagentfilter.Items.Add(item);
				return empty;
			};
			myccfilter = new libui.NumberFilterVM();
			myccfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myccfilter.ExecCommand2 = () => { myccfilter.Clear(); };
			myclientfilter = new GTDRegisterCustomerCheckListBoxVMFillDefault();
			myclientfilter.DeferredFill = true;
			myclientfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
			myclientfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myclientfilter.ExecCommand2 = () => { myclientfilter.Clear(); };
			myclientfilter.FillDefault = () =>
			{
				bool empty = this.FilterEmpty;
				if (empty)
					foreach (CustomerLegal item in myclientfilter.DefaultList)
						myclientfilter.Items.Add(item);
				return empty;
			};
			mycostlogisticsfilter = new libui.NumberFilterVM();
			mycostlogisticsfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mycostlogisticsfilter.ExecCommand2 = () => { mycostlogisticsfilter.Clear(); };
			mycostperfilter = new libui.NumberFilterVM();
			mycostperfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mycostperfilter.ExecCommand2 = () => { mycostperfilter.Clear(); };
			mycosttotalfilter = new libui.NumberFilterVM();
			mycosttotalfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mycosttotalfilter.ExecCommand2 = () => { mycosttotalfilter.Clear(); };
			myddspidyfilter = new libui.NumberFilterVM();
			myddspidyfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myddspidyfilter.ExecCommand2 = () => { myddspidyfilter.Clear(); };
			mydeclarationnumberfilter = new GTDRegisterDeclarationNumberCheckListBoxVMFill();
			mydeclarationnumberfilter.DeferredFill = true;
			mydeclarationnumberfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mydeclarationnumberfilter.ExecCommand2 = () => { mydeclarationnumberfilter.Clear(); };
			mydifprofitincomealgfilter = new libui.NumberFilterVM();
			mydifprofitincomealgfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mydifprofitincomealgfilter.ExecCommand2 = () => { mydtratefilter.Clear(); };
			mydtratefilter = new libui.NumberFilterVM();
			mydtratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mydtratefilter.ExecCommand2 = () => { mydtratefilter.Clear(); };
			mydtsumrubfilter = new libui.NumberFilterVM();
			mydtsumrubfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mydtsumrubfilter.ExecCommand2 = () => { mydtsumrubfilter.Clear(); };
			myfeefilter = new libui.NumberFilterVM();
			myfeefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myfeefilter.ExecCommand2 = () => { myfeefilter.Clear(); };
			mygtlsfilter = new libui.NumberFilterVM();
			mygtlsfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mygtlsfilter.ExecCommand2 = () => { mygtlsfilter.Clear(); };
			mygtlscurfilter = new libui.NumberFilterVM();
			mygtlscurfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mygtlscurfilter.ExecCommand2 = () => { mygtlscurfilter.Clear(); };
			mygtlsdatefilter = new libui.DateFilterVM();
			mygtlsdatefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mygtlsdatefilter.ExecCommand2 = () => { mygtlsdatefilter.Clear(); };
			mygtlsratefilter = new libui.NumberFilterVM();
			mygtlsratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mygtlsratefilter.ExecCommand2 = () => { mygtlsratefilter.Clear(); };
			myincomealgfilter = new libui.NumberFilterVM();
			myincomealgfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myincomealgfilter.ExecCommand2 = () => { mymarkupalgfilter.Clear(); };
			mymarkupalgfilter = new libui.NumberFilterVM();
			mymarkupalgfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mymarkupalgfilter.ExecCommand2 = () => { mymarkupalgfilter.Clear(); };
			mymarkupbufilter = new libui.NumberFilterVM();
			mymarkupbufilter.ExecCommand1 = () => { FilterRunExec(null); };
			mymarkupbufilter.ExecCommand2 = () => { mymarkupbufilter.Clear(); };
			mymarkuptotalilter = new libui.NumberFilterVM();
			mymarkuptotalilter.ExecCommand1 = () => { FilterRunExec(null); };
			mymarkuptotalilter.ExecCommand2 = () => { mymarkuptotalilter.Clear(); };
			mymfkfilter = new libui.NumberFilterVM();
			mymfkfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mymfkfilter.ExecCommand2 = () => { mymfkfilter.Clear(); };
			mymfkratefilter = new libui.NumberFilterVM();
			mymfkratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mymfkratefilter.ExecCommand2 = () => { mymfkratefilter.Clear(); };
			mymfkwithoutratefilter = new libui.NumberFilterVM();
			mymfkwithoutratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mymfkwithoutratefilter.ExecCommand2 = () => { mymfkwithoutratefilter.Clear(); };
			myparifilter = new libui.NumberFilterVM();
			myparifilter.ExecCommand1 = () => { FilterRunExec(null); };
			myparifilter.ExecCommand2 = () => { myparifilter.Clear(); };
			myparcelfilter = new GTDRegisterParcelCheckListBoxVMFillDefault();
			myparcelfilter.DeferredFill = true;
			myparcelfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myparcelfilter.ExecCommand2 = () => { myparcelfilter.Clear(); };
			myparcelfilter.FillDefault = () =>
			{
				bool empty = this.FilterEmpty;
				if (empty)
					foreach (ParcelNumber item in CustomBrokerWpf.References.ParcelNumbers)
						myparcelfilter.Items.Add(item);
				return empty;
			};
			myprofitfilter = new libui.NumberFilterVM();
			myprofitfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myprofitfilter.ExecCommand2 = () => { myprofitfilter.Clear(); };
			myprofitabilityfilter = new libui.NumberFilterVM();
			myprofitabilityfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myprofitabilityfilter.ExecCommand2 = () => { myprofitabilityfilter.Clear(); };
			myprofitalgefilter = new libui.NumberFilterVM();
			myprofitalgefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myprofitalgefilter.ExecCommand2 = () => { myprofitalgefilter.Clear(); };
			myprofitalgrfilter = new libui.NumberFilterVM();
			myprofitalgrfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myprofitalgrfilter.ExecCommand2 = () => { myprofitalgrfilter.Clear(); };
			myprofitdifffilter = new libui.NumberFilterVM();
			myprofitdifffilter.ExecCommand1 = () => { FilterRunExec(null); };
			myprofitdifffilter.ExecCommand2 = () => { myprofitdifffilter.Clear(); };
			myratefilter = new libui.CheckListBoxVM();
			myratefilter.RefreshIsVisible = false;
			myratefilter.AreaFilterIsVisible = false;
			myratefilter.Items = new List<string>(); myratefilter.Items.Add("0"); myratefilter.Items.Add("1"); myratefilter.Items.Add("2");
			myratefilter.ItemsView.SortDescriptions.Clear();
			myratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myratefilter.ExecCommand2 = () => { myratefilter.Clear(); };
			mysellingfilter = new libui.NumberFilterVM();
			mysellingfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mysellingfilter.ExecCommand2 = () => { mysellingfilter.Clear(); };
			mysellingdatefilter = new libui.DateFilterVM();
			mysellingdatefilter.IsNull = true;
			//mysellingdatefilter.DateStart = DateTime.Today.AddMonths(-4);
			mysellingdatefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mysellingdatefilter.ExecCommand2 = () => { mysellingdatefilter.Clear(); };
			mysellingratefilter = new libui.NumberFilterVM();
			mysellingratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mysellingratefilter.ExecCommand2 = () => { mysellingratefilter.Clear(); };
			mysellingwithoutratefilter = new libui.NumberFilterVM();
			mysellingwithoutratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mysellingwithoutratefilter.ExecCommand2 = () => { mysellingwithoutratefilter.Clear(); };
			myslfilter = new libui.NumberFilterVM();
			myslfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myslfilter.ExecCommand2 = () => { myslfilter.Clear(); };
			myslratefilter = new libui.NumberFilterVM();
			myslratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myslratefilter.ExecCommand2 = () => { myslratefilter.Clear(); };
			myslwithoutratefilter = new libui.NumberFilterVM();
			myslwithoutratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myslwithoutratefilter.ExecCommand2 = () => { myslwithoutratefilter.Clear(); };
			mytaxfilter = new libui.NumberFilterVM();
			mytaxfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mytaxfilter.ExecCommand2 = () => { mytaxfilter.Clear(); };
			mytotalsumfilter = new libui.NumberFilterVM();
			mytotalsumfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mytotalsumfilter.ExecCommand2 = () => { mytotalsumfilter.Clear(); };
			myvatfilter = new libui.NumberFilterVM();
			myvatfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myvatfilter.ExecCommand2 = () => { myvatfilter.Clear(); };
			myvatpayfilter = new libui.NumberFilterVM();
			myvatpayfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myvatpayfilter.ExecCommand2 = () => { myvatpayfilter.Clear(); };
			myvolumefilter = new libui.NumberFilterVM();
			myvolumefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myvolumefilter.ExecCommand2 = () => { myvolumefilter.Clear(); };
			myvolumeprofitfilter = new libui.NumberFilterVM();
			myvolumeprofitfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myvolumeprofitfilter.ExecCommand2 = () => { myvolumeprofitfilter.Clear(); };
			mywestgatefilter = new libui.NumberFilterVM();
			mywestgatefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mywestgatefilter.ExecCommand2 = () => { mywestgatefilter.Clear(); };
			mywestgateratefilter = new libui.NumberFilterVM();
			mywestgateratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mywestgateratefilter.ExecCommand2 = () => { mywestgateratefilter.Clear(); };
			mywestgatewithoutratefilter = new libui.NumberFilterVM();
			mywestgatewithoutratefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mywestgatewithoutratefilter.ExecCommand2 = () => { mywestgatewithoutratefilter.Clear(); };

			myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
			myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
			#endregion

			myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
		}

		private GTDRegisterDBM mymaindbm;
		private GTDRegisterSynchronizer mysync;
		private Task myrefreshtask;
		internal Importer Importer
		{ get { return mymaindbm.Importer; } }
		internal string ServiceType
		{ get { return mymaindbm.ServiceType; } }
		private GTDRegisterTotal mytotal;
		public GTDRegisterTotal Total { get { return mytotal; } }

		public Visibility TDVisible
		{ get { return this.ServiceType == "ТД" ? Visibility.Visible : Visibility.Collapsed; } }

		#region Filter
		private lib.SQLFilter.SQLFilter myfilter;
		internal lib.SQLFilter.SQLFilter Filter
		{ get { return myfilter; } }
		private GTDRegisterAgentCheckListBoxVMFillDefault myagentfilter;
		public GTDRegisterAgentCheckListBoxVMFillDefault AgentFilter
		{ get { return myagentfilter; } }
		private libui.NumberFilterVM myccfilter;
		public libui.NumberFilterVM CCFilter
		{ get { return myccfilter; } }
		private GTDRegisterCustomerCheckListBoxVMFillDefault myclientfilter;
		public GTDRegisterCustomerCheckListBoxVMFillDefault ClientFilter
		{ get { return myclientfilter; } }
		private libui.NumberFilterVM mycostlogisticsfilter;
		public libui.NumberFilterVM CostLogisticsFilter
		{ get { return mycostlogisticsfilter; } }
		private libui.NumberFilterVM mycostperfilter;
		public libui.NumberFilterVM CostPerFilter
		{ get { return mycostperfilter; } }
		private libui.NumberFilterVM mycosttotalfilter;
		public libui.NumberFilterVM CostTotalFilter
		{ get { return mycosttotalfilter; } }
		private libui.NumberFilterVM myddspidyfilter;
		public libui.NumberFilterVM DDSpidyFilter
		{ get { return myddspidyfilter; } }
		private int mydeclarationnumberfiltergroup;
		private GTDRegisterDeclarationNumberCheckListBoxVMFill mydeclarationnumberfilter;
		public GTDRegisterDeclarationNumberCheckListBoxVMFill DeclarationNumberFilter
		{ get { return mydeclarationnumberfilter; } }
		private libui.NumberFilterVM mydifprofitincomealgfilter;
		public libui.NumberFilterVM DifProfitIncomeAlgFilter
		{ get { return mydifprofitincomealgfilter; } }
		private libui.NumberFilterVM mydtratefilter;
		public libui.NumberFilterVM DTRateFilter
		{ get { return mydtratefilter; } }
		private libui.NumberFilterVM mydtsumrubfilter;
		public libui.NumberFilterVM DTSumRubFilter
		{ get { return mydtsumrubfilter; } }
		private libui.NumberFilterVM myfeefilter;
		public libui.NumberFilterVM FeeFilter
		{ get { return myfeefilter; } }
		private libui.NumberFilterVM mygtlsfilter;
		public libui.NumberFilterVM GTLSFilter
		{ get { return mygtlsfilter; } }
		private libui.NumberFilterVM mygtlscurfilter;
		public libui.NumberFilterVM GTLSCurFilter
		{ get { return mygtlscurfilter; } }
		private libui.DateFilterVM mygtlsdatefilter;
		public libui.DateFilterVM GTLSDateFilter
		{ get { return mygtlsdatefilter; } }
		private libui.NumberFilterVM mygtlsratefilter;
		public libui.NumberFilterVM GTLSRateFilter
		{ get { return mygtlsratefilter; } }
		private libui.NumberFilterVM myincomealgfilter;
		public libui.NumberFilterVM IncomeAlgFilter
		{ get { return myincomealgfilter; } }
		private libui.NumberFilterVM mymarkupalgfilter;
		public libui.NumberFilterVM MarkupAlgFilter
		{ get { return mymarkupalgfilter; } }
		private libui.NumberFilterVM mymarkupbufilter;
		public libui.NumberFilterVM MarkupBUFilter
		{ get { return mymarkupbufilter; } }
		private libui.NumberFilterVM mymarkuptotalilter;
		public libui.NumberFilterVM MarkupTotalFilter
		{ get { return mymarkuptotalilter; } }
		private libui.NumberFilterVM mymfkfilter;
		public libui.NumberFilterVM MFKFilter
		{ get { return mymfkfilter; } }
		private libui.NumberFilterVM mymfkratefilter;
		public libui.NumberFilterVM MFKRateFilter
		{ get { return mymfkratefilter; } }
		private libui.NumberFilterVM mymfkwithoutratefilter;
		public libui.NumberFilterVM MFKWithoutRateFilter
		{ get { return mymfkwithoutratefilter; } }
		private libui.NumberFilterVM myparifilter;
		public libui.NumberFilterVM PariFilter
		{ get { return myparifilter; } }
		private int myparcelfiltergroup;
		private GTDRegisterParcelCheckListBoxVMFillDefault myparcelfilter;
		public GTDRegisterParcelCheckListBoxVMFillDefault ParcelFilter
		{ get { return myparcelfilter; } }
		private libui.NumberFilterVM myprofitfilter;
		public libui.NumberFilterVM ProfitFilter
		{ get { return myprofitfilter; } }
		private libui.NumberFilterVM myprofitabilityfilter;
		public libui.NumberFilterVM ProfitabilityFilter
		{ get { return myprofitabilityfilter; } }
		private libui.NumberFilterVM myprofitalgefilter;
		public libui.NumberFilterVM ProfitAlgEFilter
		{ get { return myprofitalgefilter; } }
		private libui.NumberFilterVM myprofitalgrfilter;
		public libui.NumberFilterVM ProfitAlgRFilter
		{ get { return myprofitalgrfilter; } }
		private libui.NumberFilterVM myprofitdifffilter;
		public libui.NumberFilterVM ProfitDiffFilter
		{ get { return myprofitdifffilter; } }
		private libui.CheckListBoxVM myratefilter;
		public libui.CheckListBoxVM RateFilter
		{ get { return myratefilter; } }
		private libui.NumberFilterVM mysellingfilter;
		public libui.NumberFilterVM SellingFilter
		{ get { return mysellingfilter; } }
		private libui.DateFilterVM mysellingdatefilter;
		public libui.DateFilterVM SellingDateFilter
		{ get { return mysellingdatefilter; } }
		private libui.NumberFilterVM mysellingratefilter;
		public libui.NumberFilterVM SellingRateFilter
		{ get { return mysellingratefilter; } }
		private libui.NumberFilterVM mysellingwithoutratefilter;
		public libui.NumberFilterVM SellingWithoutRateFilter
		{ get { return mysellingwithoutratefilter; } }
		private libui.NumberFilterVM myslfilter;
		public libui.NumberFilterVM SLFilter
		{ get { return myslfilter; } }
		private libui.NumberFilterVM myslratefilter;
		public libui.NumberFilterVM SLRateFilter
		{ get { return myslratefilter; } }
		private libui.NumberFilterVM myslwithoutratefilter;
		public libui.NumberFilterVM SLWithoutRateFilter
		{ get { return myslwithoutratefilter; } }
		private libui.NumberFilterVM mytaxfilter;
		public libui.NumberFilterVM TaxFilter
		{ get { return mytaxfilter; } }
		private libui.NumberFilterVM mytotalsumfilter;
		public libui.NumberFilterVM TotalSumFilter
		{ get { return mytotalsumfilter; } }
		private libui.NumberFilterVM myvatfilter;
		public libui.NumberFilterVM VATFilter
		{ get { return myvatfilter; } }
		private libui.NumberFilterVM myvatpayfilter;
		public libui.NumberFilterVM VATPayFilter
		{ get { return myvatpayfilter; } }
		private libui.NumberFilterVM myvolumefilter;
		public libui.NumberFilterVM VolumeFilter
		{ get { return myvolumefilter; } }
		private libui.NumberFilterVM myvolumeprofitfilter;
		public libui.NumberFilterVM VolumeProfitFilter
		{ get { return myvolumeprofitfilter; } }
		private libui.NumberFilterVM mywestgatefilter;
		public libui.NumberFilterVM WestGateFilter
		{ get { return mywestgatefilter; } }
		private libui.NumberFilterVM mywestgateratefilter;
		public libui.NumberFilterVM WestGateRateFilter
		{ get { return mywestgateratefilter; } }
		private libui.NumberFilterVM mywestgatewithoutratefilter;
		public libui.NumberFilterVM WestGateWithoutRateFilter
		{ get { return mywestgatewithoutratefilter; } }

		private RelayCommand myfilterrun;
		public ICommand FilterRun
		{
			get { return myfilterrun; }
		}
		private void FilterRunExec(object parametr)
		{
			this.EndEdit();
			FilterActualise();
			if (myagentfilter.FilterOn)
			{
				string[] items = new string[myagentfilter.SelectedItems.Count];
				for (int i = 0; i < myagentfilter.SelectedItems.Count; i++)
					items[i] = (myagentfilter.SelectedItems[i] as lib.ReferenceSimpleItem).Id.ToString();
				myfilter.SetList(myfilter.FilterWhereId, "agent", items);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "agent", new string[0]);
			if (mydeclarationnumberfilter.FilterOn)
			{
				bool isNullOrEmpty = false;
				string[] items = new string[mydeclarationnumberfilter.SelectedItems.Count];
				for (int i = 0; i < mydeclarationnumberfilter.SelectedItems.Count; i++)
				{
					items[i] = (string)mydeclarationnumberfilter.SelectedItems[i];
					if (items[i] == string.Empty)
						isNullOrEmpty = true;
				}
				myfilter.SetList(mydeclarationnumberfiltergroup, "decnum", items);
				List<lib.SQLFilter.SQLFilterCondition> conds = myfilter.ConditionGet(mydeclarationnumberfiltergroup, "decnum");
				if (isNullOrEmpty)
				{ if (conds.Count == 1) myfilter.ConditionAdd(mydeclarationnumberfiltergroup, "decnum", "IS NULL"); }
				else if (conds.Count > 1)
					myfilter.ConditionDel(myfilter.ConditionGet(mydeclarationnumberfiltergroup, "decnum").First((lib.SQLFilter.SQLFilterCondition con) => { return con.propertyOperator == "IS NULL"; }).propertyid);
			}
			else
				foreach (lib.SQLFilter.SQLFilterCondition cond in myfilter.ConditionGet(mydeclarationnumberfiltergroup, "decnum"))
					myfilter.ConditionDel(cond.propertyid);
			if (myparcelfilter.FilterOn)
			{
				string[] items = new string[myparcelfilter.SelectedItems.Count];
				for (int i = 0; i < myparcelfilter.SelectedItems.Count; i++)
					items[i] = (myparcelfilter.SelectedItems[i] as ParcelNumber).Id.ToString();
				myfilter.SetList(myfilter.FilterWhereId, "parcel", items);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "parcel", new string[0]);
			//myfilter.SetDatePeriod(myfilter.FilterWhereId, "sellingdate", "sellingdatemin", "sellingdatemax", mysellingdatefilter.DateStart, mysellingdatefilter.DateStop, mysellingdatefilter.IsNull);
			DatePeriodFilterSet(mysellingdatefilter, "sellingdate", "sellingdatemin", "sellingdatemax");
			RefreshData(null);
		}
		private bool FilterRunCanExec(object parametr)
		{ return true; }
		private void FilterActualise()
		{
			NumberFilterSet(myccfilter, "cc");
			if (myclientfilter.FilterOn)
			{
				string[] items = new string[myclientfilter.SelectedItems.Count];
				for (int i = 0; i < myclientfilter.SelectedItems.Count; i++)
					items[i] = (myclientfilter.SelectedItems[i] as CustomerLegal).Id.ToString();
				myfilter.SetList(myfilter.FilterWhereId, "customer", items);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "customer", new string[0]);
			NumberFilterSet(mycostlogisticsfilter, "costlogistics");
			NumberFilterSet(mycostperfilter, "costper");
			NumberFilterSet(mycosttotalfilter, "costtotal");
			NumberFilterSet(myddspidyfilter, "ddspidy");
			NumberFilterSet(mydtratefilter, "dtrate");
			NumberFilterSet(mydtsumrubfilter, "dtsumrub");
			NumberFilterSet(myfeefilter, "fee");
			NumberFilterSet(mygtlsfilter, "gtls");
			NumberFilterSet(mygtlscurfilter, "gtlscur");
			DateFilterSet(mygtlsdatefilter, "gtlsdate");
			NumberFilterSet(mygtlsratefilter, "gtlsrate");
			NumberFilterSet(mymarkupalgfilter, "markupalg");
			NumberFilterSet(mymarkupbufilter, "markupbu");
			NumberFilterSet(mymarkuptotalilter, "markuptotal");
			NumberFilterSet(mymfkfilter, "mfk");
			NumberFilterSet(mymfkratefilter, "mfkrate");
			NumberFilterSet(mymfkwithoutratefilter, "mfkwithoutrate");
			NumberFilterSet(myparifilter, "pari");
			NumberFilterSet(myprofitfilter, "profit");
			NumberFilterSet(myprofitabilityfilter, "profitability");
			if (myratefilter.FilterOn)
			{
				string[] items = new string[myratefilter.SelectedItems.Count];
				for (int i = 0; i < myratefilter.SelectedItems.Count; i++)
					items[i] = (string)myratefilter.SelectedItems[i];
				myfilter.SetList(myfilter.FilterWhereId, "rate", items);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "rate", new string[0]);
			NumberFilterSet(myslfilter, "sl");
			NumberFilterSet(myslratefilter, "slrate");
			NumberFilterSet(myslwithoutratefilter, "slwithoutrate");
			NumberFilterSet(mytaxfilter, "tax");
			NumberFilterSet(mytotalsumfilter, "totalsum");
			NumberFilterSet(myvatfilter, "vat");
			NumberFilterSet(myvatpayfilter, "vatpay");
			NumberFilterSet(myvolumefilter, "volume");
			NumberFilterSet(myvolumeprofitfilter, "volumeprofit");
			NumberFilterSet(mywestgatefilter, "westgate");
			NumberFilterSet(mywestgateratefilter, "westgaterate");
			NumberFilterSet(mywestgatewithoutratefilter, "westgtwtrate");
		}
		private RelayCommand myfilterclear;
		public ICommand FilterClear
		{
			get { return myfilterclear; }
		}
		private void FilterClearExec(object parametr)
		{
			myagentfilter.Clear();
			myagentfilter.IconVisibileChangedNotification();
			myccfilter.Clear();
			myccfilter.IconVisibileChangedNotification();
			myclientfilter.Clear();
			myclientfilter.IconVisibileChangedNotification();
			mycostlogisticsfilter.Clear();
			mycostlogisticsfilter.IconVisibileChangedNotification();
			mycostperfilter.Clear();
			mycostperfilter.IconVisibileChangedNotification();
			mycosttotalfilter.Clear();
			mycosttotalfilter.IconVisibileChangedNotification();
			myddspidyfilter.Clear();
			myddspidyfilter.IconVisibileChangedNotification();
			mydeclarationnumberfilter.Clear();
			mydeclarationnumberfilter.IconVisibileChangedNotification();
			mydtratefilter.Clear();
			mydtratefilter.IconVisibileChangedNotification();
			mydtsumrubfilter.Clear();
			mydtsumrubfilter.IconVisibileChangedNotification();
			myfeefilter.Clear();
			myfeefilter.IconVisibileChangedNotification();
			mygtlsfilter.Clear();
			mygtlsfilter.IconVisibileChangedNotification();
			mygtlscurfilter.Clear();
			mygtlscurfilter.IconVisibileChangedNotification();
			mygtlsdatefilter.Clear();
			mygtlsdatefilter.IconVisibileChangedNotification();
			mygtlsratefilter.Clear();
			mygtlsratefilter.IconVisibileChangedNotification();
			mymarkupalgfilter.Clear();
			mymarkupalgfilter.IconVisibileChangedNotification();
			mymarkupbufilter.Clear();
			mymarkupbufilter.IconVisibileChangedNotification();
			mymarkuptotalilter.Clear();
			mymarkuptotalilter.IconVisibileChangedNotification();
			mymfkfilter.Clear();
			mymfkfilter.IconVisibileChangedNotification();
			mymfkratefilter.Clear();
			mymfkratefilter.IconVisibileChangedNotification();
			mymfkwithoutratefilter.Clear();
			mymfkwithoutratefilter.IconVisibileChangedNotification();
			myparifilter.Clear();
			myparifilter.IconVisibileChangedNotification();
			myparcelfilter.Clear();
			myparcelfilter.IconVisibileChangedNotification();
			myprofitfilter.Clear();
			myprofitfilter.IconVisibileChangedNotification();
			myprofitabilityfilter.Clear();
			myprofitabilityfilter.IconVisibileChangedNotification();
			myratefilter.Clear();
			myratefilter.IconVisibileChangedNotification();
			mysellingfilter.Clear();
			mysellingfilter.IconVisibileChangedNotification();
			mysellingdatefilter.Clear();
			mysellingdatefilter.IconVisibileChangedNotification();
			mysellingratefilter.Clear();
			mysellingratefilter.IconVisibileChangedNotification();
			mysellingwithoutratefilter.Clear();
			mysellingwithoutratefilter.IconVisibileChangedNotification();
			myslfilter.Clear();
			myslfilter.IconVisibileChangedNotification();
			myslratefilter.Clear();
			myslratefilter.IconVisibileChangedNotification();
			myslwithoutratefilter.Clear();
			myslwithoutratefilter.IconVisibileChangedNotification();
			mytaxfilter.Clear();
			mytaxfilter.IconVisibileChangedNotification();
			mytotalsumfilter.Clear();
			mytotalsumfilter.IconVisibileChangedNotification();
			myvatfilter.Clear();
			myvatfilter.IconVisibileChangedNotification();
			myvatpayfilter.Clear();
			myvatpayfilter.IconVisibileChangedNotification();
			myvolumefilter.Clear();
			myvolumefilter.IconVisibileChangedNotification();
			myvolumeprofitfilter.Clear();
			myvolumeprofitfilter.IconVisibileChangedNotification();
			mywestgatefilter.Clear();
			mywestgatefilter.IconVisibileChangedNotification();
			mywestgateratefilter.Clear();
			mywestgateratefilter.IconVisibileChangedNotification();
			mywestgatewithoutratefilter.Clear();
			mywestgatewithoutratefilter.IconVisibileChangedNotification();
			this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
		}
		private bool FilterClearCanExec(object parametr)
		{ return true; }
		private void NumberFilterSet(libui.NumberFilterVM filter, string property)
		{
			if (filter.Synchronized) return;
			myfilter.SetNumber(myfilter.FilterWhereId, property, filter.Operator1.SQLOperator, filter.NumberStart?.ToString(System.Globalization.CultureInfo.InvariantCulture), filter.Operator2.SQLOperator, filter.NumberStop?.ToString(System.Globalization.CultureInfo.InvariantCulture),filter.IsNull);
			filter.Synchronized = true;
		}
		private void PercentFilterSet(libui.NumberFilterVM filter, string property)
		{
			if (filter.Synchronized) return;
			myfilter.SetNumber(myfilter.FilterWhereId, property, filter.Operator1.SQLOperator, filter.NumberStart.HasValue ? decimal.Divide(filter.NumberStart.Value, 100M).ToString(System.Globalization.CultureInfo.InvariantCulture) : null, filter.Operator2.SQLOperator, filter.NumberStop.HasValue ? decimal.Divide(filter.NumberStop.Value, 100M).ToString(System.Globalization.CultureInfo.InvariantCulture) : null, filter.IsNull);
			filter.Synchronized = true;
		}
		private void DateFilterSet(libui.DateFilterVM filter, string property)
		{
			if (!filter.Synchronized)
			{
				myfilter.SetDate(myfilter.FilterWhereId, property, property, filter.DateStart, filter.DateStop, filter.IsNull);
				filter.Synchronized = true;
			}
		}
		private void DatePeriodFilterSet(libui.DateFilterVM filter, string group, string propertystart, string propertystop)
		{
			this.DatePeriodFilterSet(filter, myfilter.FilterWhereId, group, propertystart, propertystop);
		}
		private void DatePeriodFilterSet(libui.DateFilterVM filter, int parentgroupid, string groupname, string propertystart, string propertystop)
		{
			if (!filter.Synchronized)
			{
				myfilter.SetDatePeriod(parentgroupid, groupname, propertystart, propertystop, filter.DateStart, filter.DateStop, filter.IsNull);
				filter.Synchronized = true;
			}
		}
		public bool ClientsFilter(object item)
		{
			bool where = true;
			if (where & myclientfilter.FilterOn)
			{
				where = false;
				GTDRegisterClientVM client = item as GTDRegisterClientVM;
				foreach (CustomerLegal nameitem in myclientfilter.SelectedItems)
					if (client.Client.DomainObject == nameitem)
					{
						where = true;
						break;
					}
			}
			return where;
		}
		private bool FilterEmpty
		{
			get
			{
				return !(myparcelfilter.FilterOn ||
					myagentfilter.FilterOn ||
					myccfilter.FilterOn ||
					myclientfilter.FilterOn ||
					mycostlogisticsfilter.FilterOn ||
					mycostperfilter.FilterOn ||
					mycosttotalfilter.FilterOn ||
					myddspidyfilter.FilterOn ||
					mydeclarationnumberfilter.FilterOn ||
					mydtratefilter.FilterOn ||
					mydtsumrubfilter.FilterOn ||
					myfeefilter.FilterOn ||
					mygtlsfilter.FilterOn ||
					mygtlscurfilter.FilterOn ||
					mygtlsdatefilter.FilterOn ||
					mygtlsratefilter.FilterOn ||
					mymarkupalgfilter.FilterOn ||
					mymarkupbufilter.FilterOn ||
					mymarkuptotalilter.FilterOn ||
					mymfkfilter.FilterOn ||
					mymfkratefilter.FilterOn ||
					mymfkwithoutratefilter.FilterOn ||
					myparifilter.FilterOn ||
					myprofitfilter.FilterOn ||
					myprofitabilityfilter.FilterOn ||
					myratefilter.FilterOn ||
					mysellingfilter.FilterOn ||
					mysellingdatefilter.FilterOn ||
					mysellingratefilter.FilterOn ||
					mysellingwithoutratefilter.FilterOn ||
					myslfilter.FilterOn ||
					myslratefilter.FilterOn ||
					myslwithoutratefilter.FilterOn ||
					mytaxfilter.FilterOn ||
					mytotalsumfilter.FilterOn ||
					myvatfilter.FilterOn ||
					myvatpayfilter.FilterOn ||
					myvolumefilter.FilterOn ||
					myvolumeprofitfilter.FilterOn ||
					mywestgatefilter.FilterOn ||
					mywestgateratefilter.FilterOn ||
					mywestgatewithoutratefilter.FilterOn);
			}
		}
		#endregion

		private lib.TaskAsync.TaskAsync myexceltask;
		private RelayCommand myexcelexport;
		public ICommand ExcelExport
		{
			get { return myexcelexport; }
		}
		private void ExcelExportExec(object parametr)
		{
			this.myendedit();
			if (myexceltask == null)
				myexceltask = new lib.TaskAsync.TaskAsync();
			if (!myexceltask.IsBusy)
			{
				System.Windows.Controls.DataGrid source = parametr as System.Windows.Controls.DataGrid;
				libui.ExcelExportPopUpWindow win = new libui.ExcelExportPopUpWindow();
				win.SetProperty = (System.Windows.Controls.DataGridColumn column) =>
				{
					string name = column.SortMemberPath.Substring(column.SortMemberPath.LastIndexOf('.') + 1);
					if ((name == "Name" | name == "Rate") && column.SortMemberPath.LastIndexOf('.') > 0)
					{
						if (column.SortMemberPath.LastIndexOf('.', column.SortMemberPath.LastIndexOf('.') - 1) + 1 > 0)
							name = column.SortMemberPath.Substring(column.SortMemberPath.LastIndexOf('.', column.SortMemberPath.LastIndexOf('.') - 1) + 1).Replace(".", string.Empty);
						else
							name = column.SortMemberPath.Replace(".", string.Empty);
					}
					return name;
				};
				win.SourceDataGrid = source;
				bool? ok = win.ShowDialog();
				if (ok.HasValue && ok.Value)
				{
					int count;
					System.Collections.IEnumerable items;
					if (source.SelectedItems.Count > 1)
					{
						items = source.SelectedItems;
						count = source.SelectedItems.Count;
					}
					else
					{
						items = myview;
						count = myview.Count;
					}
					myexceltask.DoProcessing = OnExcelExport;
					myexceltask.Run(new object[3] { win.Columns, items, count });
				}
			}
			else
			{
				System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
			}
		}
		private bool ExcelExportCanExec(object parametr)
		{ return !(myview == null || myview.IsAddingNew | myview.IsEditingItem); }
		private KeyValuePair<bool, string> OnExcelExport(object args)
		{
			Excel.Application exApp = new Excel.Application();
			Excel.Application exAppProt = new Excel.Application();
			exApp.Visible = false;
			exApp.DisplayAlerts = false;
			exApp.ScreenUpdating = false;
			myexceltask.ProgressChange(2);
			try
			{
				int row = 2, column = 1;
				exApp.SheetsInNewWorkbook = 1;
				Excel.Workbook exWb = exApp.Workbooks.Add(Type.Missing);
				Excel.Worksheet exWh = exWb.Sheets[1];
				Excel.Range r;
				exWh.Name = "Реестр ГТД " + this.Importer.Name;

				int o;
				string d, m, y, s, dateformat;
				o = (int)exApp.International[Excel.XlApplicationInternational.xlDateOrder];
				s = exApp.International[Excel.XlApplicationInternational.xlDateSeparator];
				d = exApp.International[Excel.XlApplicationInternational.xlDayCode];
				if (exApp.International[Excel.XlApplicationInternational.xlDayLeadingZero])
					d = d + d;
				m = exApp.International[Excel.XlApplicationInternational.xlMonthCode];
				if (exApp.International[Excel.XlApplicationInternational.xlMonthLeadingZero])
					m = m + m;
				y = exApp.International[Excel.XlApplicationInternational.xlYearCode];
				y = y + y;
				if (exApp.International[Excel.XlApplicationInternational.xl4DigitYears])
					y = y + y;
				dateformat = o == 0 ? string.Format("{2}{0}{1}{0}{3}", s, d, m, y) : (o == 1 ? string.Format("{1}{0}{2}{0}{3}", s, d, m, y) : string.Format("{3}{0}{2}{0}{1}", s, d, m, y));

				int maxrow = (int)(args as object[])[2] + 1;
				System.Collections.IEnumerable items = (args as object[])[1] as System.Collections.IEnumerable;
				libui.WPFDataGrid.DataGridColumnInfo[] columns = ((args as object[])[0] as libui.WPFDataGrid.DataGridColumnInfo[]);
				exWh.Rows[1, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
				foreach (libui.WPFDataGrid.DataGridColumnInfo columninfo in columns)
				{
					if (!string.IsNullOrEmpty(columninfo.Property))
					{
						exWh.Cells[1, column] = columninfo.Header;
						switch (columninfo.Property)
						{
							case nameof(Parcel.ParcelNumberOrder):
							case nameof(GTDRegister.Managers):
								exWh.Columns[column, Type.Missing].NumberFormat = "@";
								exWh.Columns[column, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
								break;
							case "AgentName":
							case nameof(Declaration.Number):
							case "ClientName":
								exWh.Columns[column, Type.Missing].NumberFormat = "@";
								break;
							case nameof(GTDRegister.CostPer):
							case nameof(GTDRegister.MarkupAlg):
							case nameof(GTDRegister.MarkupBU):
							case nameof(GTDRegister.MarkupTotal):
							case nameof(GTDRegister.Profitability):
								exWh.Columns[column, Type.Missing].NumberFormat = "0%";
								break;
							case nameof(Declaration.CBRate):
							case nameof(GTDRegister.Volume):
							case nameof(Parcel.UsdRate):
								exWh.Columns[column, Type.Missing].NumberFormat = @"# ##0,0000";
								break;
							case nameof(GTDRegister.SellingDate):
							case nameof(Parcel.RateDate):
								exWh.Columns[column, Type.Missing].NumberFormat = dateformat;//exApp.International[Excel.XlApplicationInternational.xlYearCode]@"[$-419]dd.MM.yyyy;@" System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern
								break;
							case nameof(GTDRegister.Rate):
								break;
							default:
								exWh.Columns[column, Type.Missing].NumberFormat = @"# ##0,00";
								break;
						}
						column++;
					}
					else
						break;
				}
				myexceltask.ProgressChange(2 + (int)(decimal.Divide(1, maxrow) * 100));

				foreach (GTDRegisterVM item in items.OfType<GTDRegisterVM>())
				{
					column = 1;
					foreach (libui.WPFDataGrid.DataGridColumnInfo columninfo in columns)
					{
						switch (columninfo.Property)
						{
							case nameof(Parcel.ParcelNumberOrder):
								exWh.Cells[row, column] = item.Specification.Parcel.ParcelNumber;
								break;
							case nameof(GTDRegister.Managers):
								exWh.Cells[row, column] = item.Managers;
								break;
							case nameof(Declaration.Number):
								exWh.Cells[row, column] = item.Specification.Declaration.Number;
								break;
							case "AgentName":
								exWh.Cells[row, column] = item.Specification.Agent.Name;
								break;
							case nameof(GTDRegister.SellingDate):
								exWh.Cells[row, column] = item.SellingDate;
								break;
							case nameof(GTDRegister.Selling):
								exWh.Cells[row, column] = item.Selling;
								break;
							case nameof(GTDRegister.SellingWithoutRate):
								exWh.Cells[row, column] = item.SellingWithoutRate;
								break;
							case nameof(GTDRegister.SellingRate):
								exWh.Cells[row, column] = item.SellingRate;
								break;
							case "ClientName":
								exWh.Cells[row, column] = item.Client?.Name;
								break;
							case nameof(GTDRegister.Rate):
								exWh.Cells[row, column] = item.Rate;
								break;
							case nameof(Specification.Specification.Declaration.Fee):
								exWh.Cells[row, column] = item.Specification.Declaration.Fee;
								break;
							case nameof(Specification.Specification.Declaration.Tax):
								exWh.Cells[row, column] = item.Specification.Declaration.Tax;
								break;
							case "VAT":
								exWh.Cells[row, column] = item.Specification.Declaration.VAT;
								break;
							case nameof(Declaration.TotalSum):
								exWh.Cells[row, column] = item.Specification.Declaration.TotalSum;
								break;
							case nameof(Declaration.CBRate):
								exWh.Cells[row, column] = item.Specification.Declaration.CBRate;
								break;
							case nameof(item.DTSumRub):
								exWh.Cells[row, column] = item.DTSumRub;
								break;
							case nameof(item.CC):
								exWh.Cells[row, column] = item.CC;
								break;
							case nameof(item.Specification.Pari):
								exWh.Cells[row, column] = item.Specification.Pari;
								break;
							case nameof(item.SL):
								exWh.Cells[row, column] = item.SL;
								break;
							case nameof(item.SLWithoutRate):
								exWh.Cells[row, column] = item.SLWithoutRate;
								break;
							case nameof(item.SLRate):
								exWh.Cells[row, column] = item.SLRate;
								break;
							case nameof(Parcel.RateDate):
								exWh.Cells[row, column] = item.Specification.Parcel.RateDate;
								break;
							case nameof(Parcel.UsdRate):
								exWh.Cells[row, column] = item.Specification.Parcel.UsdRate;
								break;
							case nameof(item.Specification.GTLSCur):
								exWh.Cells[row, column] = item.Specification.GTLSCur;
								break;
							case nameof(item.Specification.GTLS):
								exWh.Cells[row, column] = item.Specification.GTLS;
								break;
							case nameof(item.Specification.DDSpidy):
								exWh.Cells[row, column] = item.Specification.DDSpidy;
								break;
							case nameof(item.Specification.WestGate):
								exWh.Cells[row, column] = item.Specification.WestGate;
								break;
							case nameof(item.Specification.WestGateWithoutRate):
								exWh.Cells[row, column] = item.Specification.WestGateWithoutRate;
								break;
							case nameof(item.Specification.WestGateRate):
								exWh.Cells[row, column] = item.Specification.WestGateRate;
								break;
							case nameof(item.Specification.MFK):
								exWh.Cells[row, column] = item.Specification.MFK;
								break;
							case nameof(item.Specification.MFKWithoutRate):
								exWh.Cells[row, column] = item.Specification.MFKWithoutRate;
								break;
							case nameof(item.Specification.MFKRate):
								exWh.Cells[row, column] = item.Specification.MFKRate;
								break;
							case nameof(item.CostLogistics):
								exWh.Cells[row, column] = item.CostLogistics;
								break;
							case nameof(item.CostTotal):
								exWh.Cells[row, column] = item.CostTotal;
								break;
							case nameof(item.CostPer):
								exWh.Cells[row, column] = item.CostPer;
								break;
							case nameof(item.Profit):
								exWh.Cells[row, column] = item.Profit;
								break;
							case nameof(item.IncomeAlg):
								exWh.Cells[row, column] = item.IncomeAlg;
								break;
							case nameof(item.DifProfitIncomeAlg):
								exWh.Cells[row, column] = item.DifProfitIncomeAlg;
								break;
							case nameof(item.MarkupAlg):
								exWh.Cells[row, column] = item.MarkupAlg;
								break;
							case nameof(item.MarkupBU):
								exWh.Cells[row, column] = item.MarkupBU;
								break;
							case nameof(item.MarkupTotal):
								exWh.Cells[row, column] = item.MarkupTotal;
								break;
							case nameof(item.Profitability):
								exWh.Cells[row, column] = item.Profitability;
								break;
							case nameof(item.VATPay):
								exWh.Cells[row, column] = item.VATPay;
								break;
							case nameof(item.Volume):
								exWh.Cells[row, column] = item.Volume;
								break;
							case nameof(item.VolumeProfit):
								exWh.Cells[row, column] = item.VolumeProfit;
								break;
						}
						column++;
					}
					row++;
					if (item.DomainObject.Clients.Count > 1)
						foreach (GTDRegisterClientVM client in item.Clients.OfType<GTDRegisterClientVM>())
						{
							column = 1;
							foreach (libui.WPFDataGrid.DataGridColumnInfo columninfo in columns)
							{
								switch (columninfo.Property)
								{
									case nameof(GTDRegister.SellingDate):
										if (client.SellingDate.HasValue) exWh.Cells[row, column].Value2 = client.SellingDate.Value;
										break;
									case nameof(GTDRegister.Selling):
										exWh.Cells[row, column] = client.Selling;
										break;
									case nameof(GTDRegister.SellingWithoutRate):
										exWh.Cells[row, column] = client.SellingWithoutRate;
										break;
									case nameof(GTDRegister.SellingRate):
										exWh.Cells[row, column] = client.SellingRate;
										break;
									case "ClientName":
										exWh.Cells[row, column] = client.Client.Name;
										break;
									case nameof(GTDRegister.Managers):
										exWh.Cells[row, column] = client.Managers;
										break;
									case nameof(GTDRegister.Rate):
										exWh.Cells[row, column] = client.Rate;
										break;
									case nameof(Specification.Specification.Declaration.Fee):
										exWh.Cells[row, column] = client.Fee;
										break;
									case nameof(Specification.Specification.Declaration.Tax):
										exWh.Cells[row, column] = client.Tax;
										break;
									case "VAT":
										exWh.Cells[row, column] = client.VAT;
										break;
									case nameof(Declaration.TotalSum):
										exWh.Cells[row, column] = client.DTSum;
										break;
									case nameof(Declaration.CBRate):
										exWh.Cells[row, column] = client.BuyRate;
										break;
									case nameof(item.DTSumRub):
										exWh.Cells[row, column] = item.DTSumRub;
										break;
									case nameof(item.CC):
										exWh.Cells[row, column] = client.CC;
										break;
									case nameof(item.Specification.Pari):
										exWh.Cells[row, column] = client.Pari;
										break;
									case nameof(item.SL):
										exWh.Cells[row, column] = client.SL;
										break;
									case nameof(item.SLWithoutRate):
										exWh.Cells[row, column] = client.SLWithoutRate;
										break;
									case nameof(item.SLRate):
										exWh.Cells[row, column] = client.SLRate;
										break;
									case nameof(item.Specification.GTLS):
										exWh.Cells[row, column] = client.GTLS;
										break;
									case nameof(item.Specification.DDSpidy):
										exWh.Cells[row, column] = client.DDSpidy;
										break;
									case nameof(item.Specification.WestGate):
										exWh.Cells[row, column] = client.WestGate;
										break;
									case nameof(item.Specification.WestGateWithoutRate):
										exWh.Cells[row, column] = client.WestGateWithoutRate;
										break;
									case nameof(item.Specification.WestGateRate):
										exWh.Cells[row, column] = client.WestGateRate;
										break;
									case nameof(item.Specification.MFK):
										exWh.Cells[row, column] = client.MFK;
										break;
									case nameof(item.Specification.MFKWithoutRate):
										exWh.Cells[row, column] = client.MFKWithoutRate;
										break;
									case nameof(item.Specification.MFKRate):
										exWh.Cells[row, column] = client.MFKRate;
										break;
									case nameof(item.CostLogistics):
										exWh.Cells[row, column] = client.CostLogistics;
										break;
									case nameof(item.CostTotal):
										exWh.Cells[row, column] = client.CostTotal;
										break;
									case nameof(item.CostPer):
										exWh.Cells[row, column] = client.CostPer;
										break;
									case nameof(item.Profit):
										exWh.Cells[row, column] = client.Profit;
										break;
									case nameof(item.IncomeAlg):
										exWh.Cells[row, column] = client.IncomeAlg;
										break;
									case nameof(item.DifProfitIncomeAlg):
										exWh.Cells[row, column] = client.DifProfitIncomeAlg;
										break;
									case nameof(item.MarkupAlg):
										exWh.Cells[row, column] = client.MarkupAlg;
										break;
									case nameof(item.MarkupBU):
										exWh.Cells[row, column] = client.MarkupBU;
										break;
									case nameof(item.MarkupTotal):
										exWh.Cells[row, column] = client.MarkupTotal;
										break;
									case nameof(item.Profitability):
										exWh.Cells[row, column] = client.Profitability;
										break;
									case nameof(item.VATPay):
										exWh.Cells[row, column] = client.VATPay;
										break;
									case nameof(item.Volume):
										exWh.Cells[row, column] = client.Volume;
										break;
									case nameof(item.VolumeProfit):
										exWh.Cells[row, column] = client.VolumeProfit;
										break;
								}
								column++;
							}
							row++;
						}
					myexceltask.ProgressChange(2 + (int)(decimal.Divide(row, maxrow) * 100));
				}

				r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, column - 1]];
				r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
				r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
				r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
				r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
				r.VerticalAlignment = Excel.Constants.xlTop;
				r.WrapText = true;
				r = exWh.Range[exWh.Columns[1, Type.Missing], exWh.Columns[column - 1, Type.Missing]]; r.Columns.AutoFit();

				exWh = null;
				exApp.Visible = true;
				exApp.DisplayAlerts = true;
				exApp.ScreenUpdating = true;
				myexceltask.ProgressChange(100);
				return new KeyValuePair<bool, string>(false, "Данные выгружены. " + (row - 2).ToString() + " строк обработано.");
			}
			catch (Exception ex)
			{
				if (exApp != null)
				{
					foreach (Excel.Workbook itemBook in exApp.Workbooks)
					{
						itemBook.Close(false);
					}
					exApp.Quit();
				}
				throw new Exception(ex.Message);
			}
			finally
			{
				exApp = null;
				if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
				exAppProt = null;
			}
		}

		private RelayCommand mytdload;
		public ICommand TDLoad
		{
			get { return mytdload; }
		}
		private void TDLoadExec(object parametr)
		{
			if (parametr is GTDRegisterVM)
			{
				GTDRegister request = (parametr as GTDRegisterVM).DomainObject;
				EventLoger log = new EventLoger() { What = "DT", Message = (request.Specification?.Declaration?.Number ?? "Новая") + " Start GTD", ObjectId = (request.Specification?.Declaration?.Id ?? 0) };
				log.Execute();
				if (request.Specification != null)
				{
					Specification.Specification spec = request.Specification;
					string err = spec.LoadDeclaration();
					if (string.IsNullOrEmpty(err))
						this.OpenPopup("ТД загружена!", false);
					else
						this.OpenPopup(err, true);
				}
				else
					this.OpenPopup("Загрузка ТД невозможна, заявка еще не включена в перевозку!", true);
				log.Message = (request.Specification?.Declaration?.Number ?? "Новая") + " Finish GTD";
				log.ObjectId = (request.Specification?.Declaration?.Id ?? 0);
				log.Execute();
			}
		}
		private bool TDLoadCanExec(object parametr)
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
			return myrefreshtask == null || myrefreshtask.IsCompleted;
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
			//myfilter.SetDatePeriod(myfilter.FilterWhereId, "sellingdate", "sellingdatemin", "sellingdatemax", mysellingdatefilter.DateStart, mysellingdatefilter.DateStop, mysellingdatefilter.IsNull);

			mymaindbm.Filter = myfilter;
			mymaindbm.Collection = new System.Collections.ObjectModel.ObservableCollection<GTDRegister>();
			//mymaindbm.FillAsync();
			mysync.DomainCollection = mymaindbm.Collection;
			return mysync.ViewModelCollection;
		}
		protected override void OtherViewRefresh()
		{
		}
		protected override void RefreshData(object parametr)
		{
			if (this.FilterEmpty)
				this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
			else
			{
				foreach (GTDRegister item in mymaindbm.Collection) item.Unbind();
				mytotal.StopCount();
				myrefreshtask = mymaindbm.FillAsync();
			}
		}
		protected override void SettingView()
		{
			myagentfilter.ItemsSource = myview.OfType<GTDRegisterVM>();
			myclientfilter.ItemsSource = myview.OfType<GTDRegisterVM>();
			mydeclarationnumberfilter.ItemsSource = myview.OfType<GTDRegisterVM>();
			myparcelfilter.ItemsSource = myview.OfType<GTDRegisterVM>();

			mytotal = new GTDRegisterTotal(myview, mymaindbm.ServiceType);

			this.PropertyChangedNotification(nameof(Total));
			this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
		}
	}

	public class GTDRegisterAgentCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<GTDRegisterVM, lib.ReferenceSimpleItem>
	{
		internal GTDRegisterAgentCheckListBoxVMFillDefault() : base()
		{
			this.DisplayPath = "Name";
			this.SearchPath = "Name";
			this.GetDisplayPropertyValueFunc = (item) => { return ((lib.ReferenceSimpleItem)item).Name; };
		}

		protected override void AddItem(GTDRegisterVM item)
		{
			lib.ReferenceSimpleItem name = CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", item.Specification.Agent.Id);
			if (!Items.Contains(name)) Items.Add(name);
		}
	}
	public class GTDRegisterCustomerCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<GTDRegisterVM, CustomerLegal>
	{
		internal GTDRegisterCustomerCheckListBoxVMFillDefault() : base()
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

		protected override void AddItem(GTDRegisterVM item)
		{
			foreach (GTDRegisterClientVM client in item.Clients)
				if (!Items.Contains(client.Client.DomainObject)) Items.Add(client.Client.DomainObject);
		}
	}
	public class GTDRegisterDeclarationNumberCheckListBoxVMFill : libui.CheckListBoxVMFill<GTDRegisterVM, string>
	{
		protected override void AddItem(GTDRegisterVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(item.Specification.Declaration?.Number == null || Items.Contains(item.Specification.Declaration?.Number))) Items.Add(item.Specification.Declaration.Number);
		}
	}
	public class GTDRegisterParcelCheckListBoxVMFillDefault : libui.CheckListBoxVMFillDefault<GTDRegisterVM, ParcelNumber>
	{
		internal GTDRegisterParcelCheckListBoxVMFillDefault() : base()
		{
			this.DisplayPath = "FullNumber";
			this.SearchPath = "Sort";
			this.GetDisplayPropertyValueFunc = (item) => { return ((ParcelNumber)item).FullNumber; };
			this.SortDescriptions.Add(new System.ComponentModel.SortDescription("Sort", System.ComponentModel.ListSortDirection.Descending));
		}

		protected override void AddItem(GTDRegisterVM item)
		{
			ParcelNumber name;
			if (item.Specification.Parcel?.Id > 0)
			{
				name = CustomBrokerWpf.References.ParcelNumbers.FindFirstItem("Id", item.Specification.Parcel.Id);
				if (!Items.Contains(name)) Items.Add(name);
			}
		}
	}

	public class GTDRegisterTotal : lib.TotalValues.TotalViewValues<GTDRegisterVM>
	{
		internal GTDRegisterTotal(ListCollectionView view, string servicetype) : base(view)
		{
			//myinitselected = 2; // if not selected - sum=0
			myservicetype = servicetype;
		}
		private readonly string myservicetype;
		private int myitemcount;
		public int ItemCount { set { myitemcount = value; } get { return myitemcount; } }
		private decimal mycc;
		public decimal CC { set { mycc = value; } get { return mycc; } }
		public decimal CostLogistics { get { return mypari + this.SLWithoutRate + mygtls + myddspidy + this.WestGateWithoutRate + mymfk; } }
		public decimal CostTotal { get { return mycc + myfee + mytax + this.CostLogistics; } }
		//private decimal myeurosum;
		//public decimal EuroSum { set { myeurosum = value; } get { return myeurosum; } }
		private decimal myddspidy;
		public decimal DDSpidy { set { myddspidy = value; } get { return myddspidy; } }
		public decimal DifProfitIncomeAlg { get { return myservicetype == "ТД" ? this.Profit - this.IncomeAlg : 0M; } }
		private decimal mydtsum;
		public decimal DTSum { set { mydtsum = value; } get { return mydtsum; } }
		private decimal mydtsumrub;
		public decimal DTSumRub { set { mydtsumrub = value; } get { return mydtsumrub; } }
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
		public decimal MFKRate { get { return mymfk * 20M / 120M; } }
		public decimal MFKWithoutRate { get { return mymfk - this.MFKRate; } }
		private decimal mypari;
		public decimal Pari { set { mypari = value; } get { return mypari; } }
		public decimal Profit { get { return this.SellingWithoutRate - (myservicetype == "ТД" ? this.CostTotal : this.CostLogistics); } }
		public decimal ProfitDiff { get { return myservicetype == "ТД" ? this.Profit - this.ProfitAlgR : 0M; } }
		private decimal myprofitalge;
		public decimal ProfitAlgE { set { myprofitalge = value; } get { return myprofitalge; } }
		private decimal myprofitalgr;
		public decimal ProfitAlgR { set { myprofitalgr = value; } get { return myprofitalgr; } }
		private decimal myselling;
		public decimal Selling { set { myselling = value; } get { return myselling; } }
		public decimal SellingRate { get { return myservicetype == "ТД" ? myselling * 20M / 120M : 0M; } }
		public decimal SellingWithoutRate { get { return myservicetype == "ТД" ? myselling - this.SellingRate : this.Selling; } }
		private decimal mysl;
		public decimal SL { set { mysl = value; } get { return mysl; } }
		public decimal SLRate { get { return mysl * 20M / 120M; } }
		public decimal SLWithoutRate { get { return mysl - this.SLRate; } }
		private decimal mytax;
		public decimal Tax { set { mytax = value; } get { return mytax; } }
		private decimal myvat;
		public decimal VAT { set { myvat = value; } get { return myvat; } }
		public decimal VATPay { get { return this.SellingRate - this.VAT - this.SLRate - this.WestGateRate - this.MFKRate; } }
		private decimal myvolume;
		public decimal Volume { set { myvolume = value; } get { return myvolume; } }
		private decimal mywestgate;
		public decimal WestGate { set { mywestgate = value; } get { return mywestgate; } }
		public decimal WestGateRate { get { return mywestgate * 20M / 120M; } }
		public decimal WestGateWithoutRate { get { return mywestgate - this.WestGateRate; } }

		protected override void Item_ValueChangedHandler(GTDRegisterVM sender, ValueChangedEventArgs<object> e)
		{
			decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
			switch (e.PropertyName)
			{
				case "Total" + nameof(GTDRegisterClientTotal.CC):
					mycc += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.CC));
					PropertyChangedNotification(nameof(this.CostTotal));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.DDSpidy):
					myddspidy += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.DDSpidy));
					PropertyChangedNotification(nameof(this.CostLogistics));
					PropertyChangedNotification(nameof(this.CostTotal));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.DTSum):
					mydtsum += newvalue - oldvalue;
					mydtsumrub = mydtsum * (sender.Specification.Declaration.CBRate ?? 0M);
					PropertyChangedNotification(nameof(this.DTSum));
					PropertyChangedNotification(nameof(this.DTSumRub));
					break;
				//case "Total" + nameof(GTDRegisterClientTotal.EuroSum):
				//    myeurosum += newvalue - oldvalue;
				//    PropertyChangedNotification(nameof(this.EuroSum));
				//    break;
				case "Total" + nameof(GTDRegisterClientTotal.Fee):
					myfee += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.Fee));
					PropertyChangedNotification(nameof(this.CostTotal));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.GTLS):
					mygtls += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.GTLS));
					PropertyChangedNotification(nameof(this.CostLogistics));
					PropertyChangedNotification(nameof(this.CostTotal));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.GTLSCur):
					mygtlscur += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.GTLSCur));
					break;
				case "Total" + nameof(GTDRegisterClientVM.IncomeAlg):
					myincomealg += myservicetype == "ТД" ? newvalue - oldvalue : 0M;
					PropertyChangedNotification(nameof(this.IncomeAlg));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.MFK):
					mymfk += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.MFK));
					PropertyChangedNotification(nameof(this.MFKRate));
					PropertyChangedNotification(nameof(this.MFKWithoutRate));
					PropertyChangedNotification(nameof(this.CostLogistics));
					PropertyChangedNotification(nameof(this.CostTotal));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					PropertyChangedNotification(nameof(this.VATPay));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.Pari):
					mypari += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.Pari));
					PropertyChangedNotification(nameof(this.CostLogistics));
					PropertyChangedNotification(nameof(this.CostTotal));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.ProfitAlgE):
					myprofitalge += myservicetype == "ТД" ? newvalue - oldvalue : 0M;
					PropertyChangedNotification(nameof(this.ProfitAlgE));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.ProfitAlgR):
					myprofitalgr += myservicetype == "ТД" ? newvalue - oldvalue : 0M;
					PropertyChangedNotification(nameof(this.ProfitAlgR));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					break;
				case nameof(GTDRegisterClient.Selling) when myservicetype == "ТЭО":
					myselling += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					PropertyChangedNotification(nameof(this.Selling));
					PropertyChangedNotification(nameof(this.SellingRate));
					PropertyChangedNotification(nameof(this.SellingWithoutRate));
					PropertyChangedNotification(nameof(this.VATPay));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.Selling) when myservicetype == "ТД":
					myselling += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					PropertyChangedNotification(nameof(this.Selling));
					PropertyChangedNotification(nameof(this.SellingRate));
					PropertyChangedNotification(nameof(this.SellingWithoutRate));
					PropertyChangedNotification(nameof(this.VATPay));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.SL):
					mysl += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.SL));
					PropertyChangedNotification(nameof(this.SLRate));
					PropertyChangedNotification(nameof(this.SLWithoutRate));
					PropertyChangedNotification(nameof(this.CostLogistics));
					PropertyChangedNotification(nameof(this.CostTotal));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					PropertyChangedNotification(nameof(this.VATPay));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.Tax):
					mytax += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.Tax));
					PropertyChangedNotification(nameof(this.CostTotal));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.Vat):
					myvat += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.VAT));
					PropertyChangedNotification(nameof(this.VATPay));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.Volume):
					myvolume += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.Volume));
					break;
				case "Total" + nameof(GTDRegisterClientTotal.WestGate):
					mywestgate += newvalue - oldvalue;
					PropertyChangedNotification(nameof(this.WestGate));
					PropertyChangedNotification(nameof(this.WestGateRate));
					PropertyChangedNotification(nameof(this.WestGateWithoutRate));
					PropertyChangedNotification(nameof(this.CostLogistics));
					PropertyChangedNotification(nameof(this.CostTotal));
					PropertyChangedNotification(nameof(this.Profit));
					PropertyChangedNotification(nameof(this.ProfitDiff));
					PropertyChangedNotification(nameof(this.VATPay));
					PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
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
			mydtsumrub = 0M;
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
		protected override void ValuesPlus(GTDRegisterVM item)
		{
			myitemcount++;
			mycc += item.ClientTotal.CC;
			myddspidy += item.ClientTotal.DDSpidy;
			mydtsum += myservicetype == "ТД" ? item.ClientTotal.DTSum : item.Specification.Declaration.TotalSum ?? 0M;
			mydtsumrub = mydtsum * (item.Specification.Declaration.CBRate ?? 0M);
			myfee += item.ClientTotal.Fee;
			mygtls += item.ClientTotal.GTLS;
			mygtlscur += item.ClientTotal.GTLSCur;
			myincomealg += item.IncomeAlg ?? 0M;
			mymfk += item.ClientTotal.MFK;
			//myeurosum += item.ClientTotal.EuroSum;
			mypari += item.ClientTotal.Pari;
			if (myservicetype == "ТД") myprofitalge += item.ClientTotal.ProfitAlgE;
			if (myservicetype == "ТД") myprofitalgr += item.ClientTotal.ProfitAlgR;
			myselling += myservicetype == "ТД" ? item.ClientTotal.Selling : (item.Selling ?? 0M);
			mysl += item.ClientTotal.SL;
			mytax += item.ClientTotal.Tax;
			myvat += item.ClientTotal.Vat;
			myvolume += item.ClientTotal.Volume;
			mywestgate += item.ClientTotal.WestGate;
		}
		protected override void ValuesMinus(GTDRegisterVM item)
		{
			myitemcount--;
			mycc -= item.ClientTotal.CC;
			myddspidy -= item.ClientTotal.DDSpidy;
			mydtsum -= myservicetype == "ТД" ? item.ClientTotal.DTSum : item.Specification.Declaration.TotalSum ?? 0M;
			mydtsumrub = mydtsum * (item.Specification.Declaration.CBRate ?? 0M);
			myfee -= item.ClientTotal.Fee;
			mygtls -= item.ClientTotal.GTLS;
			mygtlscur -= item.ClientTotal.GTLSCur;
			myincomealg -= item.IncomeAlg ?? 0M;
			mymfk -= item.ClientTotal.MFK;
			//myeurosum -= item.ClientTotal.EuroSum;
			mypari -= item.ClientTotal.Pari;
			if (myservicetype == "ТД") myprofitalge -= item.ClientTotal.ProfitAlgE;
			if (myservicetype == "ТД") myprofitalgr -= item.ClientTotal.ProfitAlgR;
			myselling -= myservicetype == "ТД" ? item.ClientTotal.Selling : (item.Selling ?? 0M);
			mysl -= item.ClientTotal.SL;
			mytax -= item.ClientTotal.Tax;
			myvat -= item.ClientTotal.Vat;
			myvolume -= item.ClientTotal.Volume;
			mywestgate -= item.ClientTotal.WestGate;
		}
		protected override void PropertiesChangedNotifycation()
		{
			this.PropertyChangedNotification("ItemCount");
			this.PropertyChangedNotification(nameof(this.CC));
			this.PropertyChangedNotification(nameof(this.CostLogistics));
			this.PropertyChangedNotification(nameof(this.CostTotal));
			this.PropertyChangedNotification(nameof(this.DDSpidy));
			this.PropertyChangedNotification(nameof(this.DifProfitIncomeAlg));
			this.PropertyChangedNotification(nameof(this.DTSum));
			this.PropertyChangedNotification(nameof(this.DTSumRub));
			this.PropertyChangedNotification(nameof(this.Fee));
			this.PropertyChangedNotification(nameof(this.GTLS));
			this.PropertyChangedNotification(nameof(this.GTLSCur));
			this.PropertyChangedNotification(nameof(this.IncomeAlg));
			this.PropertyChangedNotification(nameof(this.MFK));
			this.PropertyChangedNotification(nameof(this.MFKRate));
			this.PropertyChangedNotification(nameof(this.MFKWithoutRate));
			//this.PropertyChangedNotification(nameof(this.EuroSum));
			this.PropertyChangedNotification(nameof(this.Pari));
			this.PropertyChangedNotification(nameof(this.Profit));
			this.PropertyChangedNotification(nameof(this.ProfitDiff));
			this.PropertyChangedNotification(nameof(this.ProfitAlgE));
			this.PropertyChangedNotification(nameof(this.ProfitAlgR));
			this.PropertyChangedNotification(nameof(this.Selling));
			this.PropertyChangedNotification(nameof(this.SellingRate));
			this.PropertyChangedNotification(nameof(this.SellingWithoutRate));
			this.PropertyChangedNotification(nameof(this.SL));
			this.PropertyChangedNotification(nameof(this.SLRate));
			this.PropertyChangedNotification(nameof(this.SLWithoutRate));
			this.PropertyChangedNotification(nameof(this.Tax));
			this.PropertyChangedNotification(nameof(this.VAT));
			this.PropertyChangedNotification(nameof(this.VATPay));
			this.PropertyChangedNotification(nameof(this.Volume));
			this.PropertyChangedNotification(nameof(this.WestGate));
			this.PropertyChangedNotification(nameof(this.WestGateRate));
			this.PropertyChangedNotification(nameof(this.WestGateWithoutRate));
		}
	}
}
