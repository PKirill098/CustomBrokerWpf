using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
	public class InvoiceCurrencyPay : CustomsInvoicePay
	{
		public InvoiceCurrencyPay(int id, long stamp, DateTime? updated, string updater, lib.DomainObjectState mstate
			, decimal cursum, CustomsInvoice invoice, DateTime pdate, decimal psum, IValidator validator
			) : base(id, stamp, updated, updater, mstate, invoice, pdate, psum, validator)
		{
			mycursum = cursum;
			base.PropertyChanged += Base_PropertyChanged;
			myrater = new CurrencyRateProxy(CustomBrokerWpf.References.CurrencyRate);
			myrater.PropertyChanged += Rater_PropertyChanged;
			if (this.CurSum > 0M & this.PaySum > 0M)
				mycbrate = decimal.Round(decimal.Divide(decimal.Divide(this.PaySum, this.CurSum), 1.02M), 4);
			else
				myrater.RateDate = this.PayDate;
		}

		private decimal mycbrate;
		public decimal CBRate
		{
			set
			{
				mycbrate = value;
				if (!this.UpdatingSample)
				{
					this.PropertyChangedNotification(nameof(this.CBRatep2p));
					if (this.PaySum > 0M)
					{
						this.CurSum = mycbrate > 0M ? decimal.Round(decimal.Divide(this.PaySum, this.CBRatep2p), 2) : 0M;
						//this.PropertyChangedNotification(nameof(this.CurSum));
					}
					else if (this.CurSum > 0M)
					{
						this.PaySum = decimal.Round(decimal.Multiply(this.CurSum, this.CBRatep2p), 2);
						//this.PropertyChangedNotification(nameof(this.PaySum));
					}
				}
			}
			get { return mycbrate; }
		}
		public decimal CBRatep2p
		{
			set { this.CBRate = decimal.Round(decimal.Divide(value, 1.02M), 4); }
			get { return decimal.Round(this.CBRate * 1.02M, 4); }
		}
		private decimal mycursum;
		public decimal CurSum
		{
			internal set { SetProperty<decimal>(ref mycursum, value); }
			get { return mycursum; }
		}
		private Classes.CurrencyRateProxy myrater;
		private void Rater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "EURRate" & myrater.EURRate.HasValue)
			{
				this.CBRate = myrater.EURRate.Value;
			}
		}


		private void Base_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (this.UpdatingSample) return;
			switch (e.PropertyName)
			{
				case nameof(CustomsInvoicePay.PayDate):
					this.CBRate = 0;
					myrater.RateDate = this.PayDate;
					break;
				case nameof(CustomsInvoicePay.PaySum):
					this.CurSum = mycbrate > 0M ? decimal.Round(decimal.Divide(this.PaySum, this.CBRatep2p), 2) : 0M;
					break;
			}
		}
		protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
		{
			base.PropertiesUpdate(sample);
			InvoiceCurrencyPay templ = sample as InvoiceCurrencyPay;
			this.CurSum = templ.CurSum;
		}
		protected override void RejectProperty(string property, object value)
		{
			switch (property)
			{
				case nameof(this.CurSum):
					this.CurSum = (decimal)value;
					break;
				default:
					base.RejectProperty(property, value);
					break;
			}
		}
	}

	public class InvoiceCurrencyPayDBM : lib.DBManagerWhoWhen<InvoiceCurrencyPay, InvoiceCurrencyPay>
	{
		public InvoiceCurrencyPayDBM() : base()
		{
			ConnectionString = CustomBrokerWpf.References.ConnectionString;
			SelectParams = new SqlParameter[] { new SqlParameter("@invoiceid", System.Data.SqlDbType.Int) };
			myinsertparams = new SqlParameter[]
			{
				myinsertparams[0],myinsertparams[1]
				,new SqlParameter("@invoiceid",System.Data.SqlDbType.Int)
			};
			myupdateparams = new SqlParameter[]
			{
				myupdateparams[0]
				,new SqlParameter("@cursumupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@pdateupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@psumupd", System.Data.SqlDbType.Bit)
			};
			myinsertupdateparams = new SqlParameter[]
			{
			   myinsertupdateparams[0],myinsertupdateparams[1]
			   ,new SqlParameter("@psum",System.Data.SqlDbType.Money)
			   ,new SqlParameter("@pdate",System.Data.SqlDbType.DateTime2)
			   ,new SqlParameter("@cursum",System.Data.SqlDbType.Money)
			 };
		}

		private CustomsInvoice myinvoice;
		internal CustomsInvoice Invoice { set { myinvoice = value; } get { return myinvoice; } }
		internal IValidator Validator { set; get; }
		protected override InvoiceCurrencyPay CreateRecord(SqlDataReader reader)
		{
			return new InvoiceCurrencyPay(reader.GetInt32(reader.GetOrdinal("id")), reader.GetInt64(reader.GetOrdinal("stamp"))
				, reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")), reader.IsDBNull(reader.GetOrdinal("updater")) ? null : reader.GetString(reader.GetOrdinal("updater"))
				, lib.DomainObjectState.Unchanged
				, reader.GetDecimal(reader.GetOrdinal("cursum"))
				, this.Invoice, reader.GetDateTime(reader.GetOrdinal("pdate")), reader.GetDecimal(reader.GetOrdinal("psum"))
				, this.Validator);
		}
		protected override InvoiceCurrencyPay CreateModel(InvoiceCurrencyPay reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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
		protected override bool SaveChildObjects(InvoiceCurrencyPay item)
		{
			return true;
		}
		protected override bool SaveIncludedObject(InvoiceCurrencyPay item)
		{
			return true;
		}
		protected override bool SaveReferenceObjects()
		{
			return true;
		}
		protected override void SetSelectParametersValue()
		{
			this.SelectParams[0].Value = myinvoice?.Id;
		}
		protected override bool SetParametersValue(InvoiceCurrencyPay item)
		{
            base.SetParametersValue(item);
			myinsertparams[2].Value = item.Invoice.Id;
			myinsertupdateparams[2].Value = item.PaySum;
			myinsertupdateparams[3].Value = item.PayDate;
			foreach (SqlParameter par in myupdateparams)
				switch (par.ParameterName)
				{
					case "@cursumupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(InvoiceCurrencyPay.CurSum));
						break;
					case "@pdateupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(InvoiceCurrencyPay.PayDate));
						break;
					case "@psumupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(InvoiceCurrencyPay.PaySum));
						break;
				}
			foreach (SqlParameter par in myinsertupdateparams)
				switch (par.ParameterName)
				{
					case "@cursum":
						par.Value = item.CurSum;
						break;
					case "@pdate":
						par.Value = item.PayDate;
						break;
					case "@psum":
						par.Value = item.PaySum;
						break;
				}
			return true;
		}
	}

	public class InvoiceCurrencyPayVM : CustomsInvoicePayVM
	{
		public InvoiceCurrencyPayVM(InvoiceCurrencyPay model) : base(model)
		{ mymodel = model; }

		private InvoiceCurrencyPay mymodel;
		public decimal? CurSum
		{
			//set
			//{
			//    if (value.HasValue && !(this.IsReadOnly || decimal.Equals(mypaysum, value.Value)))
			//    {
			//        string name = nameof(this.PaySum);
			//        if (!myUnchangedPropertyCollection.ContainsKey(name))
			//            this.myUnchangedPropertyCollection.Add(name, this.DomainObject.PaySum);
			//        mypaysum = value;
			//        if (this.ValidateProperty(name))
			//        { ChangingDomainProperty = name; this.DomainObject.PaySum = value.Value; this.ClearErrorMessageForProperty(name); }
			//    }
			//}
			//get { return this.IsEnabled ? mypaysum : (decimal?)null; }
			get { return this.IsEnabled ? mymodel.CurSum : (decimal?)null; }
		}
		public decimal? CBRatep2p
		{
			set
			{
				if (value.HasValue && !(this.IsReadOnly || decimal.Equals(mymodel.CBRatep2p, value.Value)))
				{
					string name = nameof(this.CBRatep2p);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mymodel.CBRatep2p);
					{ ChangingDomainProperty = name; mymodel.CBRatep2p = value.Value; }
				}
			}
			get { return this.IsEnabled ? mymodel.CBRatep2p : (decimal?)null; }
		}
	}

	public class InvoiceCurrencyPaySynchronizer : lib.ModelViewCollectionsSynchronizer<InvoiceCurrencyPay, InvoiceCurrencyPayVM>
	{
		protected override InvoiceCurrencyPay UnWrap(InvoiceCurrencyPayVM wrap)
		{
			return wrap.DomainObject as InvoiceCurrencyPay;
		}
		protected override InvoiceCurrencyPayVM Wrap(InvoiceCurrencyPay fill)
		{
			return new InvoiceCurrencyPayVM(fill);
		}
	}

	public class InvoiceCurrencyPayCommand : lib.ViewModelViewCommand
	{
		public InvoiceCurrencyPayCommand(CustomsInvoice invoice, IValidator validator) : base()
		{
			mymaindbm = new InvoiceCurrencyPayDBM();
			mymaindbm.Invoice = invoice;
			mymaindbm.Validator = validator;
			mydbm = mymaindbm;
			mysync = new InvoiceCurrencyPaySynchronizer();
		}
		protected InvoiceCurrencyPayDBM mymaindbm;
		protected InvoiceCurrencyPaySynchronizer mysync;
		internal CustomsInvoice Invoice { get { return mymaindbm.Invoice; } }

		protected override void AddData(object parametr)
		{
			InvoiceCurrencyPayVM item = new InvoiceCurrencyPayVM(new InvoiceCurrencyPay(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, 0M, mymaindbm.Invoice, DateTime.Today, 0M, mymaindbm.Validator));
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
	public class CustomsInvoicePayFinalCur1ViewCommand : InvoiceCurrencyPayCommand
	{
		public CustomsInvoicePayFinalCur1ViewCommand(CustomsInvoice invoice) : base(invoice, new CustomsInvoicePayValidatorFinalCur1())
		{
			mymaindbm.SelectCommandText = "account.FinalInvoicePayCur1_sp";
			mymaindbm.InsertCommandText = "account.FinalInvoicePayCur1Add_sp";
			mymaindbm.UpdateCommandText = "account.FinalInvoicePayCur1Upd_sp";
			mymaindbm.DeleteCommandText = "account.FinalInvoicePayCur1Del_sp";
			mymaindbm.Collection = invoice.FinalCurPays1;
			mysync.DomainCollection = invoice.FinalCurPays1;
			base.Collection = mysync.ViewModelCollection;
		}
		public override bool SaveDataChanges()
		{
			bool sucess = base.SaveDataChanges();
			mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.FinalCurPaySum));
			mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.FinalCurPaidDate1));
			return sucess;
		}

	}
	public class CustomsInvoicePayFinalCur2ViewCommand : InvoiceCurrencyPayCommand
	{
		public CustomsInvoicePayFinalCur2ViewCommand(CustomsInvoice invoice) : base(invoice, new CustomsInvoicePayValidatorFinalCur2())
		{
			mymaindbm.SelectCommandText = "account.FinalInvoicePayCur2_sp";
			mymaindbm.InsertCommandText = "account.FinalInvoicePayCur2Add_sp";
			mymaindbm.UpdateCommandText = "account.FinalInvoicePayCur2Upd_sp";
			mymaindbm.DeleteCommandText = "account.FinalInvoicePayCur2Del_sp";
			mymaindbm.Collection = mymaindbm.Invoice.FinalCurPays2;
			mysync.DomainCollection = mymaindbm.Invoice.FinalCurPays2;
			base.Collection = mysync.ViewModelCollection;
		}
		public override bool SaveDataChanges()
		{
			bool sucess = base.SaveDataChanges();
			mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.FinalCurPaySum2));
			mymaindbm.Invoice.PropertyChangedNotification(nameof(CustomsInvoice.FinalCurPaidDate2));
			return sucess;
		}
	}
}
