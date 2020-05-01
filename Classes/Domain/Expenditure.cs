using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
	class Expenditure : Classes.Domain.DomainBaseClass
	{
		private int myid, mytype, mycostitem, mycountdetails;
		private int? myparcelid;
		private string mycurrency, mynote, mysubject, myinvoicenumber;
		private DateTime? myperiodstart, myperiodend, myinvoicedate;
		private decimal mypaycur, mypayrub;
		//private System.Collections.ObjectModel.ObservableCollection<Contractor> mydetails;

		public int Id
		{
			internal set { myid = value; }
			get { return myid; }
		}
		public int Type
		{
			set
			{
				if (!int.Equals(mytype, value))
				{
					string name = "Type";
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mytype);
					mytype = value;
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return mytype; }
		}
		public int CostItem
		{
			set
			{
				if (!int.Equals(mycostitem, value))
				{
					string name = "CostItem";
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mycostitem);
					mycostitem = value;
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return mycostitem; }
		}
		public int? ParcelID
		{
			set
			{
				if (!int.Equals(myparcelid, value))
				{
					string name = "ParcelID";
					myparcelid = value;
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, myparcelid);
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return myparcelid; }
		}
		public string Currency
		{
			set
			{
				if (!string.Equals(mycurrency,value))
				{
					string name = "Currency";
					mycurrency = value;
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mycurrency);
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return mycurrency; }
		}
		public string Subject
		{
			set
			{
				if (!string.Equals(mysubject, value))
				{
					string name = "Subject";
					mysubject = value;
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mysubject);
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return mysubject; }
		}
		public string Note
		{
			set
			{
				if (!string.Equals(mynote,value))
				{
					string name = "Note";
					mynote = value;
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mynote);
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return mynote; }
		}
		public DateTime? PeriodStart
		{
			set
			{
				if (!DateTime.Equals(myperiodstart, value))
				{
					string name = "PeriodStart";
					myperiodstart = value;
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, myperiodstart);
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return myperiodstart; }
		}
		public DateTime? PeriodEnd
		{
			set
			{
				if (!DateTime.Equals(myperiodend, value))
				{
					string name = "PeriodEnd";
					myperiodend = value;
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, myperiodend);
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return myperiodend; }
		}
		//Helper
		internal int IdDetail { set; get; }
		internal int IdWithdrawal { set; get; }
		internal int StampDetail { set; get; }
		internal int StampWEx { set; get; }
		internal int StampWithdrawal { set; get; }
		internal int StampClearing { set; get; }
		internal int CountWithdrawal { set; get; }
		internal int CountDetails
		{
			set
			{
				if (!int.Equals(mycountdetails, value))
				{
					string name = "CountDetails";
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mycountdetails);
					mycountdetails = value;
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return mycountdetails; }
		}
		//Detail
		public decimal? SumEx { set; get; }
		//public System.Collections.ObjectModel.ObservableCollection<Contractor> Details
		//{get { return mydetails; }}
		// InvoiceIn
		public string InvoiceNumber
		{
			set
			{
				if (!string.Equals(myinvoicenumber, value))
				{
					string name = "InvoiceNumber";
					myinvoicenumber = value;
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, myinvoicenumber);
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return myinvoicenumber; }
		}
		public DateTime? InvoiceDate
		{
			set
			{
				if (!DateTime.Equals(myinvoicedate, value))
				{
					string name = "InvoiceDate";
					myinvoicedate = value;
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, myinvoicedate);
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return myinvoicedate; }
		}
		//ExpenditureWithdrawal
		public decimal PaymentCur
		{
			set
			{
				if (!decimal.Equals(mypaycur, value))
				{
					string name = "PaymentCur";
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mypaycur);
					mypaycur = value;
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return mypaycur; }
		}
		public decimal PaymentRub
		{
			set
			{
				if (!decimal.Equals(mypayrub, value))
				{
					string name = "PaymentRub";
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mypayrub);
					mypayrub = value;
					if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return mypayrub; }
		}
		//withdrawal
		public int Withdrawaltype { set; get; }
		public int WithdrawalAccount { set; get; }
		public int Recipient { set; get; }
		public DateTime? DateEx { set; get; }
		public decimal? SumPay { set; get; }

		public Expenditure()
			:this(id:0, exptype:0, costitem:0,
				 parcel:null, currency:"RUB", periodstart:null, periodend:null, countdetails:0, initstate:DomainObjectState.Added){}
		
		internal Expenditure(int id,int exptype, int costitem,int? parcel,string currency,DateTime? periodstart,DateTime? periodend,int countdetails, DomainObjectState initstate) :base()
		{
			mystate = initstate;
			myid = id; mytype = exptype; mycostitem = costitem;
			myparcelid = parcel;
			myperiodstart = periodstart; myperiodend = periodend;
			mycurrency = currency;
			mycountdetails = countdetails;
		}

		protected override void RejectProperty(string property, object value)
        { return; }

    }
}
