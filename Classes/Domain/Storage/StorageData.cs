using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Storage
{
	public class StorageData : lib.DomainBaseReject
	{
		public StorageData(lib.DomainObjectState mstate
			, string agentname, int cellnumber, string customername, DateTime date, string doc, decimal? forwarding, decimal? freightcost, string freightnumber, decimal? goodvalue
			, decimal grossweight, decimal? netweight, string note, string point, Request request, decimal? service, string shipmentnumber, lib.ReferenceSimpleItem storage, string store, decimal volume
			) :base(0, mstate)
		{
			myagentname = agentname;
			mycellnumber = cellnumber;
			mycustomername = customername;
			mydate = date;
			mydoc = doc;
			myforwarding = forwarding;
			myfreightcost = freightcost;
			myfreightnumber = freightnumber;
			mygoodvalue = goodvalue;
			mygrossweight = grossweight;
			mynetweight = netweight;
			mynote = note;
			mypoint = point;
			myrequest = request;
			myservice = service;
			myshipmentnumber = shipmentnumber;
			mystorage = storage;
			mystore = store;
			myvolume = volume;
		}
		public StorageData():this(lib.DomainObjectState.Added
			, string.Empty, 0, string.Empty, DateTime.Now, null, 0, 0, null, null, 0, null, null, null, null, 0, null, CustomBrokerWpf.References.Stores.GetDefault(), null, 0)
		{ }

		private string myagentname;
		public string AgentName
		{ set { SetProperty<string>(ref myagentname, value); } get { return myagentname; } }
		private int mycellnumber;
		public int CellNumber
		{ set { SetProperty<int>(ref mycellnumber, value); } get { return mycellnumber; } }
		private string mycustomername;
		public string CustomerName
		{ set { SetProperty<string>(ref mycustomername, value); } get { return mycustomername; } }
		private DateTime mydate;
		public DateTime Date
		{ set { SetProperty<DateTime>(ref mydate, value); } get { return mydate; } }
		private string mydoc;
		public string Doc
		{ set { SetProperty<string>(ref mydoc, value); } get { return mydoc; } }
		private decimal? myforwarding;
		public decimal? Forwarding
		{ set { SetProperty(ref myforwarding, value); } get { return myforwarding; } }
		private decimal? myfreightcost;
		public decimal? FreightCost
		{ set { SetProperty(ref myfreightcost, value); } get { return myfreightcost; } }
		private string myfreightnumber;
		public string FreightNumber
		{ set { SetProperty<string>(ref myfreightnumber, value); } get { return myfreightnumber; } }
		private decimal? mygoodvalue;
		public decimal? GoodValue
		{ set { SetProperty(ref mygoodvalue, value); } get { return mygoodvalue; } }
		private decimal mygrossweight;
		public decimal GrossWeight
		{ set { SetProperty<decimal>(ref mygrossweight, value); } get { return mygrossweight; } }
		private decimal? mynetweight;
		public decimal? NetWeight
		{ set { SetProperty<decimal?>(ref mynetweight, value); } get { return mynetweight; } }
		private string mynote;
		public string Note
		{ set { SetProperty<string>(ref mynote, value); } get { return mynote; } }
		private string mypoint;
		public string Point
		{ set { SetProperty<string>(ref mypoint, value); } get { return mypoint; } }
		private Request myrequest;
		public Request Request
		{ set { SetProperty<Request>(ref myrequest, value); } get { return myrequest; } }
		private decimal? myservice;
		public decimal? Service
		{ set { SetProperty<decimal?>(ref myservice, value); } get { return myservice; } }
		private string myshipmentnumber;
		public string ShipmentNumber
		{ set { SetProperty<string>(ref myshipmentnumber, value); } get { return myshipmentnumber; } }
		private lib.ReferenceSimpleItem mystorage;
		public lib.ReferenceSimpleItem Storage
		{ set { SetProperty<lib.ReferenceSimpleItem>(ref mystorage, value); } get { return mystorage; } }
		private string mystore;
		public string Store
		{ set { SetProperty<string>(ref mystore, value); } get { return mystore; } }
		private decimal myvolume;
		public decimal Volume
		{ set { SetProperty<decimal>(ref myvolume, value); } get { return myvolume; } }

		protected override void PropertiesUpdate(DomainBaseReject sample)
		{
			
		}
		protected override void RejectProperty(string property, object value)
		{
			
		}
		internal bool ValidateProperty(string propertyname, object value, out string errmsg)
		{
			bool isvalid = true;
			errmsg = null;
			switch (propertyname)
			{
				case nameof(this.Date):
					if (this.Date==DateTime.MinValue)
					{
						errmsg = "Не указана дата поступления!";
						isvalid = false;
					}
					break;
				case nameof(this.Storage):
					if (this.Storage==null)
					{
						errmsg = "Не указан склад!";
						isvalid = false;
					}
					break;
				case nameof(this.Point):
					if (string.IsNullOrEmpty(this.Point))
					{
						errmsg = "Не указана позиция по складу!";
						isvalid = false;
					}
					break;
			}
			return isvalid;
		}

	}

	internal class StorageDataDBM : lib.DBManager<SqlDataReader,StorageData>
	{
		internal StorageDataDBM()
		{
			this.NeedAddConnection = true;
			this.ConnectionString = CustomBrokerWpf.References.ConnectionString;
			this.InsertProcedure = true;
			this.UpdateProcedure = true;
			this.DeleteProcedure = true;

			this.SelectCommandText = "dbo.StorageData_sp";
			this.InsertCommandText = "dbo.StorageDataAdd_sp";
			this.UpdateCommandText = "dbo.StorageDataUpd_sp";
			this.DeleteCommandText = "dbo.StorageDel_sp";

			this.SelectParams = new SqlParameter[] { new SqlParameter("@filterid", System.Data.SqlDbType.Int) };
			this.InsertParams = new SqlParameter[] {
				new SqlParameter("@requestId", System.Data.SqlDbType.Int){Direction=System.Data.ParameterDirection.Output}
			};
			this.InsertUpdateParams = new SqlParameter[] {
				new SqlParameter("@storeId", System.Data.SqlDbType.Int),
				new SqlParameter("@storagePoint", System.Data.SqlDbType.NVarChar,6),
				new SqlParameter("@storageDate", System.Data.SqlDbType.Date),
				new SqlParameter("@agent", System.Data.SqlDbType.NVarChar,100),
				new SqlParameter("@customer", System.Data.SqlDbType.NVarChar,100),
				new SqlParameter("@grossweight", System.Data.SqlDbType.SmallMoney),
				new SqlParameter("@netweight", System.Data.SqlDbType.SmallMoney),
				new SqlParameter("@cellnumber", System.Data.SqlDbType.SmallInt),
				new SqlParameter("@volume", System.Data.SqlDbType.SmallMoney),
				new SqlParameter("@goodvalue", System.Data.SqlDbType.Money),
				new SqlParameter("@service", System.Data.SqlDbType.Money),
				new SqlParameter("@forwarding", System.Data.SqlDbType.Money),
				new SqlParameter("@shipmentnumber", System.Data.SqlDbType.NVarChar,6),
				new SqlParameter("@storagenote", System.Data.SqlDbType.NVarChar,100),
				new SqlParameter("@store", System.Data.SqlDbType.NVarChar,15),
				new SqlParameter("@doc", System.Data.SqlDbType.NVarChar,180),
				new SqlParameter("@freightnumber", System.Data.SqlDbType.NVarChar,6),
				new SqlParameter("@freightcost", System.Data.SqlDbType.Money),
			};
			this.UpdateParams = new SqlParameter[] {
				new SqlParameter("@agentupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@customerupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@grossweightupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@netweightupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@cellnumberupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@volumeupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@goodvalueupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@serviceupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@forwardingupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@shipmentnumberupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@storagenoteupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@storeupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@docupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@freightnumberupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@freightcostupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@requestIdupd", System.Data.SqlDbType.Bit),
				new SqlParameter("@requestId", System.Data.SqlDbType.Int)
			};
			this.DeleteParams = new SqlParameter[] {
				new SqlParameter("@storeId", System.Data.SqlDbType.Int),
				new SqlParameter("@storagePoint", System.Data.SqlDbType.NVarChar, 6),
				new SqlParameter("@storageDate", System.Data.SqlDbType.Date)
			};
		}

		private lib.SQLFilter.SQLFilter myfilter;
		internal lib.SQLFilter.SQLFilter Filter
		{ set { myfilter = value; } get { return myfilter; } }

		protected override SqlDataReader CreateRecord(SqlDataReader reader)
		{
			return reader;
		}
		protected override StorageData CreateModel(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			Request request = null;
			if (!reader.IsDBNull(this.Fields["requestId"]))
			{ 
				request = CustomBrokerWpf.References.RequestStore.GetItemLoad(reader.GetInt32(this.Fields["requestId"]), addcon, out var errors);
				this.Errors.AddRange(errors);
			}
			return new StorageData(lib.DomainObjectState.Unchanged
				,reader.GetString(this.Fields["agent"])
				,reader.GetInt16(this.Fields["cellnumber"])
				,reader.GetString(this.Fields["customer"])
				,reader.GetDateTime(this.Fields["storageDate"])
				,reader.IsDBNull(this.Fields["doc"]) ? null : reader.GetString(this.Fields["doc"])
				,reader.IsDBNull(this.Fields["forwarding"]) ? (decimal?)null : reader.GetDecimal(this.Fields["forwarding"])
				,reader.IsDBNull(this.Fields["freightcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["freightcost"])
				,reader.IsDBNull(this.Fields["freightnumber"]) ? null : reader.GetString(this.Fields["freightnumber"])
				,reader.IsDBNull(this.Fields["goodvalue"]) ? (decimal?)null : reader.GetDecimal(this.Fields["goodvalue"])
				,reader.GetDecimal(this.Fields["grossweight"])
				,reader.IsDBNull(this.Fields["netweight"]) ? (decimal?)null : reader.GetDecimal(this.Fields["netweight"])
				,reader.IsDBNull(this.Fields["storagenote"]) ? null : reader.GetString(this.Fields["storagenote"])
				,reader.GetString(this.Fields["storagePoint"])
				,request
				,reader.IsDBNull(this.Fields["service"]) ? (decimal?)null : reader.GetDecimal(this.Fields["service"])
				,reader.IsDBNull(this.Fields["shipmentnumber"]) ? null : reader.GetString(this.Fields["shipmentnumber"])
				,CustomBrokerWpf.References.Stores.FindFirstItem("Id", reader.GetInt32(this.Fields["storeId"]))
				,reader.IsDBNull(this.Fields["store"]) ? null : reader.GetString(this.Fields["store"])
				, reader.GetDecimal(this.Fields["volume"])
				);
		}
		protected override void GetRecord(SqlDataReader reader, SqlConnection addcon, CancellationToken canceltasktoken = default)
		{
			base.ModelFirst=this.CreateModel(reader,addcon,canceltasktoken);
		}
		protected override StorageData GetModel(SqlConnection addcon, CancellationToken canceltasktoken)
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
		protected override void GetOutputParametersValue(StorageData item)
		{
			if (item.DomainState == lib.DomainObjectState.Added && this.InsertParams.First((SqlParameter par) => { return par.ParameterName == "@requestId"; }).Value != DBNull.Value)
			{
				item.Request = CustomBrokerWpf.References.RequestStore.GetItemLoad((int)this.InsertParams.First((SqlParameter par) => { return par.ParameterName == "@requestId"; }).Value, out var errors);
				this.Errors.AddRange(errors);
			}
		}
		protected override void ItemAcceptChanches(StorageData item)
		{
			item.AcceptChanches();
		}

		protected override bool SaveChildObjects(StorageData item)
		{
			return true;
		}
		protected override bool SaveIncludedObject(StorageData item)
		{
			return true;
		}
		protected override bool SaveReferenceObjects()
		{
			return true;
		}
		protected override bool SetParametersValue(StorageData item)
		{
			if (item.DomainState == lib.DomainObjectState.Deleted)
				foreach (SqlParameter par in this.DeleteParams)
					switch (par.ParameterName)
					{
						case "@storeId":
							par.Value = item.Storage.Id;
							break;
						case "@storagePoint":
							par.Value = item.Point;
							break;
						case "@storageDate":
							par.Value = item.Date;
							break;
					}
			else
			{
				foreach (SqlParameter par in this.InsertParams)
					switch (par.ParameterName)
					{
						case "@requestId":
							par.Value = item.Request?.Id;
							break;
					}
				foreach (SqlParameter par in this.InsertUpdateParams)
					switch (par.ParameterName)
					{
						case "@agent":
							par.Value = item.AgentName;
							break;
						case "@customer":
							par.Value = item.CustomerName;
							break;
						case "@grossweight":
							par.Value = item.GrossWeight;
							break;
						case "@netweight":
							par.Value = item.NetWeight;
							break;
						case "@cellnumber":
							par.Value = item.CellNumber;
							break;
						case "@volume":
							par.Value = item.Volume;
							break;
						case "@goodvalue":
							par.Value = item.GoodValue;
							break;
						case "@service":
							par.Value = item.Service;
							break;
						case "@forwarding":
							par.Value = item.Forwarding;
							break;
						case "@shipmentnumber":
							par.Value = item.ShipmentNumber;
							break;
						case "@storageDate":
							par.Value = item.Date;
							break;
						case "@storagenote":
							par.Value = item.Note;
							break;
						case "@storagePoint":
							par.Value = item.Point;
							break;
						case "@store":
							par.Value = item.Store;
							break;
						case "@storeId":
							par.Value = item.Storage?.Id;
							break;
						case "@doc":
							par.Value = item.Doc;
							break;
						case "@freightnumber":
							par.Value = item.FreightNumber;
							break;
						case "@freightcost":
							par.Value = item.FreightCost;
							break;
					}
				if (item.DomainState == lib.DomainObjectState.Modified)
					foreach (SqlParameter par in this.UpdateParams)
						switch (par.ParameterName)
					{
						case "@storeIdupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Storage.Id));
							break;
						case "@storagePointupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Point));
							break;
						case "@storageDateupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Date));
							break;
						case "@agentupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.AgentName));
							break;
						case "@customerupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.CustomerName));
							break;
						case "@grossweightupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.GrossWeight));
							break;
						case "@netweightupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.NetWeight));
							break;
						case "@cellnumberupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.CellNumber));
							break;
						case "@volumeupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Volume));
							break;
						case "@goodvalueupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.GoodValue));
							break;
						case "@serviceupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Service));
							break;
						case "@forwardingupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Forwarding));
							break;
						case "@shipmentnumberupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.ShipmentNumber));
							break;
						case "@storagenoteupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Note));
							break;
						case "@storeupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Store));
							break;
						case "@docupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Doc));
							break;
						case "@freightnumberupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.FreightNumber));
							break;
						case "@freightcostupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.FreightCost));
							break;
						case "@requestIdupd":
							par.Value = item.HasPropertyOutdatedValue(nameof(item.Request));
							break;
						case "@requestId":
							par.Value = item.Request?.Id;
							break;
					}
			}
			return true;
		}
		protected override void SetSelectParametersValue(SqlConnection addcon)
		{
			foreach (SqlParameter par in this.SelectParams)
				switch (par.ParameterName)
				{
					case "@filterid":
						par.Value = myfilter?.FilterWhereId;
						break;
				}
		}
	}

	public class StorageDataVM : lib.ViewModelErrorNotifyItem<StorageData>
	{
		public StorageDataVM(StorageData model) : base(model)
		{
			ValidetingProperties.AddRange(new string[] { nameof(this.Storage), nameof(this.Point) });
			InitProperties();
		}

		public string AgentName
		{
			set
			{
				if (!(this.IsReadOnly || string.Equals(this.DomainObject.AgentName, value)))
				{
					string name = nameof(this.AgentName);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.AgentName);
					ChangingDomainProperty = name; this.DomainObject.AgentName = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.AgentName : null; }
		}
		public int? CellNumber
		{
			set
			{
				if (value.HasValue && !(this.IsReadOnly || int.Equals(this.DomainObject.CellNumber, value.Value)))
				{
					string name = nameof(this.CellNumber);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CellNumber);
					if (this.ValidateProperty(name))
					{ ChangingDomainProperty = name; this.DomainObject.CellNumber = value.Value; }
				}
			}
			get { return this.IsEnabled ? this.DomainObject.CellNumber : (int?)null; }
		}
		public string CustomerName
		{
			set
			{
				if (!(this.IsReadOnly || string.Equals(this.DomainObject.CustomerName, value)))
				{
					string name = nameof(this.CustomerName);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CustomerName);
					ChangingDomainProperty = name; this.DomainObject.CustomerName = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.CustomerName : null; }
		}
		public DateTime? Date
		{
			set
			{
				if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(this.DomainObject.Date, value.Value)))
				{
					string name = nameof(this.Date);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Date);
					ChangingDomainProperty = name; this.DomainObject.Date = value.Value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.Date : (DateTime?)null; }
		}
		public string Doc
		{
			set
			{
				if (!(this.IsReadOnly || string.Equals(this.DomainObject.Doc, value)))
				{
					string name = nameof(this.Doc);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Doc);
					ChangingDomainProperty = name; this.DomainObject.Doc = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.Doc : null; }
		}
		public decimal? Forwarding
		{
			set
			{
				if (!this.IsReadOnly && (this.DomainObject.Forwarding.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Forwarding.Value, value.Value))))
				{
					string name = nameof(this.Forwarding);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Forwarding);
					ChangingDomainProperty = name; this.DomainObject.Forwarding = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.Forwarding : null; }
		}
		public decimal? FreightCost
		{
			set
			{
				if (!this.IsReadOnly && (this.DomainObject.FreightCost.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.FreightCost.Value, value.Value))))
				{
					string name = nameof(this.FreightCost);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FreightCost);
					ChangingDomainProperty = name; this.DomainObject.FreightCost = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.FreightCost : null; }
		}
		public string FreightNumber
		{
			set
			{
				if (!(this.IsReadOnly || string.Equals(this.DomainObject.FreightNumber, value)))
				{
					string name = nameof(this.FreightNumber);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FreightNumber);
					ChangingDomainProperty = name; this.DomainObject.FreightNumber = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.FreightNumber : null; }
		}
		public decimal? GoodValue
		{
			set
			{
				if (!this.IsReadOnly && (this.DomainObject.GoodValue.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.GoodValue.Value, value.Value))))
				{
					string name = nameof(this.GoodValue);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GoodValue);
					ChangingDomainProperty = name; this.DomainObject.GoodValue = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.GoodValue : null; }
		}
		public decimal? GrossWeight
		{
			set
			{
				if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.GrossWeight, value.Value)))
				{
					string name = nameof(this.GrossWeight);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GrossWeight);
					if (this.ValidateProperty(name))
					{ ChangingDomainProperty = name; this.DomainObject.GrossWeight=value.Value; }
				}
			}
			get { return this.IsEnabled ? this.DomainObject.GrossWeight : (decimal?)null; }
		}
		public bool IsJoin
		{ get { return this.Request != null; } }
		public decimal? NetWeight
		{
			set
			{
				if (!this.IsReadOnly && (this.DomainObject.NetWeight.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.NetWeight.Value, value.Value))))
				{
					string name = nameof(this.NetWeight);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.NetWeight);
					ChangingDomainProperty = name; this.DomainObject.NetWeight = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.NetWeight : null; }
		}
		public string Note
		{
			set
			{
				if (!(this.IsReadOnly || string.Equals(this.DomainObject.Note, value)))
				{
					string name = nameof(this.Note);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Note);
					ChangingDomainProperty = name; this.DomainObject.Note = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.Note : null; }
		}
		private string mypoint;
		public string Point
		{
			set
			{
				if (!(this.IsReadOnly || string.Equals(mypoint, value)))
				{
					string name = nameof(this.Point);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mypoint);
					mypoint = value;
					if (ValidateProperty(name))
					{
						ChangingDomainProperty = name; this.DomainObject.Point = value;
						ClearErrorMessageForProperty(name);
					}
				}
			}
			get { return this.IsEnabled ? mypoint : null; }
		}
		//private RequestVM myrequest;
		public Request Request
		{
			set
			{
				if (!(this.IsReadOnly || object.Equals(this.DomainObject.Request, value)))
				{
					string name = nameof(this.Request);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Request);
					ChangingDomainProperty = name; this.DomainObject.Request = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.Request : null; }
		}
		public decimal? Service
		{
			set
			{
				if (value.HasValue && !(this.IsReadOnly || decimal.Equals(this.DomainObject.Service, value.Value)))
				{
					string name = nameof(this.Service);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Service);
					if (this.ValidateProperty(name))
					{ ChangingDomainProperty = name; this.DomainObject.Service = value.Value; }
				}
			}
			get { return this.IsEnabled ? this.DomainObject.Service : (decimal?)null; }
		}
		public string ShipmentNumber
		{
			set
			{
				if (!(this.IsReadOnly || string.Equals(this.DomainObject.ShipmentNumber, value)))
				{
					string name = nameof(this.ShipmentNumber);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ShipmentNumber);
					ChangingDomainProperty = name; this.DomainObject.ShipmentNumber = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.ShipmentNumber : null; }
		}
		private lib.ReferenceSimpleItem mystorage;
		public lib.ReferenceSimpleItem Storage
		{
			set
			{
				if (!(this.IsReadOnly || object.Equals(mystorage, value)))
				{
					string name = nameof(this.Storage);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mystorage);
					mystorage = value;
					if (ValidateProperty(name))
					{
						ChangingDomainProperty = name; this.DomainObject.Storage = value;
						ClearErrorMessageForProperty(name);
					}
				}
			}
			get { return this.IsEnabled ? mystorage : null; }
		}
		public string Store
		{
			set
			{
				if (!(this.IsReadOnly || string.Equals(this.DomainObject.Store, value)))
				{
					string name = nameof(this.Store);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Store);
					ChangingDomainProperty = name; this.DomainObject.Store = value;
				}
			}
			get { return this.IsEnabled ? this.DomainObject.Store : null; }
		}
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

		protected override bool DirtyCheckProperty()
		{
			return mypoint != this.DomainObject.Point || mystorage!= this.DomainObject.Storage;
		}
		protected override void DomainObjectPropertyChanged(string property)
		{
			switch (property)
			{
				case nameof(this.DomainObject.Storage):
					mystorage = this.DomainObject.Storage;
					break;
				case nameof(this.DomainObject.Point):
					mypoint = this.DomainObject.Point;
					break;
				case nameof(this.DomainObject.Request):
					this.PropertyChangedNotification(nameof(this.IsJoin));
					break;
			}
		}
		protected override void InitProperties()
		{
			mystorage = this.DomainObject.Storage;
			mypoint = this.DomainObject.Point;
		}
		protected override void RejectProperty(string property, object value)
		{
			switch (property)
			{
				case nameof(this.AgentName):
					this.DomainObject.AgentName = (string)value;
					break;
				case nameof(this.CellNumber):
					this.DomainObject.CellNumber = (int)value;
					break;
				case nameof(this.CustomerName):
					this.DomainObject.CustomerName = (string)value;
					break;
				case nameof(this.Date):
					this.DomainObject.Date = (DateTime)value;
					break;
				case nameof(this.Doc):
					this.DomainObject.Doc = (string)value;
					break;
				case nameof(this.Forwarding):
					this.DomainObject.Forwarding = (decimal?)value;
					break;
				case nameof(this.FreightCost):
					this.DomainObject.FreightCost = (decimal?)value;
					break;
				case nameof(this.GoodValue):
					this.DomainObject.GoodValue = (decimal?)value;
					break;
				case nameof(this.GrossWeight):
					this.DomainObject.GrossWeight = (decimal)value;
					break;
				case nameof(this.NetWeight):
					this.DomainObject.NetWeight = (decimal?)value;
					break;
				case nameof(this.Note):
					this.DomainObject.Note = (string)value;
					break;
				case nameof(this.Point):
					if (mypoint != this.DomainObject.Point)
						mypoint = this.DomainObject.Point;
					else
						this.DomainObject.Point = (string)value;
					break;
				case nameof(this.Request):
					this.DomainObject.Request = (Request)value;
					break;
				case nameof(this.Service):
					this.DomainObject.Service = (decimal)value;
					break;
				case nameof(this.ShipmentNumber):
					this.DomainObject.ShipmentNumber = (string)value;
					break;
				case nameof(this.Storage):
					if (mystorage != this.DomainObject.Storage)
						mystorage = this.DomainObject.Storage;
					else
						this.DomainObject.Storage = (lib.ReferenceSimpleItem)value;
					break;
				case nameof(this.Volume):
					this.DomainObject.Volume = (decimal)value;
					break;
			}
		}
		protected override bool ValidateProperty(string propertyname, bool inform = true)
		{
			bool isvalid = true;
			string errmsg = null;
			switch (propertyname)
			{
				case nameof(this.Point):
					isvalid = this.DomainObject.ValidateProperty(propertyname, mypoint, out errmsg);
					break;
				case nameof(this.Storage):
					isvalid = this.DomainObject.ValidateProperty(propertyname, mystorage, out errmsg);
					break;
			}
			if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
			return isvalid;
		}
	}

	public class StorageDataSynchronizer : lib.ModelViewCollectionsSynchronizer<StorageData, StorageDataVM>
	{
		protected override StorageData UnWrap(StorageDataVM wrap)
		{
			return wrap.DomainObject as StorageData;
		}
		protected override StorageDataVM Wrap(StorageData fill)
		{
			return new StorageDataVM(fill);
		}
	}

	public class StorageMath
	{
		internal StorageMath(Request request, StorageData storage)
		{
			myrequest = request;
			mystorage = storage;
		}
		internal StorageMath(StorageData storage):this(new Request(), storage) { }

		private Request myrequest;
		public Request Request
		{ set { myrequest = value; } get { return myrequest; } }
		private StorageData mystorage;
		public StorageData StorageData
		{ set { mystorage = value; } get { return mystorage; } }
	}

	internal class StorageMathDBM
	{
		internal StorageMathDBM()
		{
			mydbm = new RequestDBM();
			mydbm.SelectCommandText = "StorageDateMath_sp";
			mydbm.SelectParams = new SqlParameter[] { 
				new SqlParameter("@id", System.Data.SqlDbType.Int),
				new SqlParameter("@storagePoint", System.Data.SqlDbType.NChar,6),
				new SqlParameter("@storage", System.Data.SqlDbType.Int),
				new SqlParameter("@customer", System.Data.SqlDbType.NVarChar,100),
				new SqlParameter("@agent", System.Data.SqlDbType.NVarChar,100)
			};
			mycollection = new System.Collections.ObjectModel.ObservableCollection<StorageMath>();
		}

		private RequestDBM mydbm;
		internal SqlConnection Connection
		{ set { mydbm.Command.Connection = value; } get { return mydbm.Command.Connection; } }
		internal SqlTransaction Transaction
		{ set { mydbm.Transaction = value; } get { return mydbm.Transaction; } }
		internal lib.TransactionArea TransactionArea
		{ set { mydbm.TransactionArea = value; } get { return mydbm.TransactionArea; } }
		internal List<DBMError> Errors
		{ get { return mydbm.Errors; } }
		internal string ErrorMessage
		{ get { return mydbm.ErrorMessage; } }
		private System.Collections.ObjectModel.ObservableCollection<StorageMath> mycollection;
		public System.Collections.ObjectModel.ObservableCollection<StorageMath> Collection
		{ get { return mycollection; } }
		private StorageData mydata;
		internal StorageData Data
		{ set { mydata = value; } get { return mydata; } }

		internal void Fill()
		{
			mycollection.Clear();
			foreach (SqlParameter par in mydbm.SelectParams)
				switch (par.ParameterName)
				{
					case "@id":
						par.Value = mydata.Request?.Id;
						break;
					case "@storagePoint":
						par.Value = mydata.Point;
						break;
					case "@storage":
						par.Value = mydata.Storage.Id;
						break;
					case "@customer":
						par.Value = mydata.CustomerName;
						break;
					case "@agent":
						par.Value = mydata.AgentName;
						break;
				}
			mydbm.Fill();
			foreach (Request item in mydbm.Collection)
				mycollection.Add(new StorageMath(item, mydata));
		}
		internal bool SaveItemChanches(Request item)
		{
			return mydbm.SaveItemChanches(item);
		}
		internal bool SaveCollectionChanches()
		{
			if (mydbm.Collection == null)
				mydbm.Collection = new System.Collections.ObjectModel.ObservableCollection<Request>();
			else
				mydbm.Collection.Clear();
			foreach (StorageMath item in mycollection)
				mydbm.Collection.Add(item.Request);
			return mydbm.SaveCollectionChanches();
		}
	}

	public class StorageMathVM
	{
		internal StorageMathVM(StorageMath math)
		{
			mymath = math;
			myrequest = new RequestVM(math.Request);
		}

		private StorageMath mymath;
		internal StorageMath DomainObject
		{ get { return mymath; } }
		private RequestVM myrequest;
		public RequestVM Request
		{ set { myrequest = value; } get { return myrequest; } }
		public StorageData StorageData
		{ set { mymath.StorageData = value; } get { return mymath.StorageData; } }
	}

	public class StorageMathSynchronizer : lib.ModelViewCollectionsSynchronizer<StorageMath, StorageMathVM>
	{
		protected override StorageMath UnWrap(StorageMathVM wrap)
		{
			return wrap.DomainObject as StorageMath;
		}
		protected override StorageMathVM Wrap(StorageMath fill)
		{
			return new StorageMathVM(fill);
		}
	}

	public class StorageDataManager:lib.ViewModelViewCommand
	{
		internal StorageDataManager()
		{
			stores = new ListCollectionView(CustomBrokerWpf.References.Stores);
			CustomBrokerWpf.References.Stores.RefreshViewAdd(stores);
			stores.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
			
			mysync = new StorageDataSynchronizer();
			mysync.DomainCollection = new System.Collections.ObjectModel.ObservableCollection<StorageData>();
			base.Collection = mysync.ViewModelCollection;
			myfilter = new lib.SQLFilter.SQLFilter("storage", "AND",CustomBrokerWpf.References.ConnectionString);
			myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
			mymaindbm = new StorageDataDBM();
			mymaindbm.Filter = myfilter;
			mydbm = mymaindbm;
			mymaindbm.Collection = mysync.DomainCollection;
			mymaindbm.FillAsyncCompleted = () => { if (mydbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); };
			mymaindbm.FillAsync();

			mymathdbm = new StorageMathDBM();
			mymathsync = new StorageMathSynchronizer();
			mymathsync.DomainCollection = new System.Collections.ObjectModel.ObservableCollection<StorageMath>();
			mymathview = new ListCollectionView(mymathsync.ViewModelCollection);

			this.DeleteQuestionHeader = "Удалить выделенные строки склада?";
			myloaddata = new RelayCommand(LoadDataExec, LoadDataCanExec);
			mymath = new RelayCommand(MathExec, MathCanExec);
			mymerge = new RelayCommand(MergeExec, MergeCanExec);
			myrequestcreate = new RelayCommand(RequestCreateExec, RequestCreateCanExec);
			mysever = new RelayCommand(SeverExec, SeverCanExec);
			myrequestdel = new RelayCommand(RequestDeleteExec, RequestDeleteCanExec);
			myadbm = new AgentDBM();
			myadbm.AliasDBM = new AgentAliasDBM();
			mycdbm = new CustomerDBM();
		}

		private StorageDataDBM mymaindbm;
		private StorageDataSynchronizer mysync;
		private StorageMathDBM mymathdbm;
		private StorageMathSynchronizer mymathsync;
		private ListCollectionView mymathview;
		public ListCollectionView MathView
		{ get { return mymathview; } }
		
		private AgentDBM myadbm;
		private CustomerDBM mycdbm;
		private lib.SQLFilter.SQLFilter myfilter;
		internal lib.SQLFilter.SQLFilter Filter
		{
			get { return myfilter; }
		}
		private bool mymathenabled;

		private ListCollectionView stores;
		public ListCollectionView Stores
		{ get { return stores; } }
		private List<lib.ReferenceSimpleItem> myagents
		{ get { return (List<lib.ReferenceSimpleItem>)(new List<lib.ReferenceSimpleItem>{ new lib.ReferenceSimpleItem(0, "Новый", true, true, lib.DomainObjectState.Sealed) }).Concat(CustomBrokerWpf.References.AgentNames).ToList(); } }
		private List<lib.ReferenceSimpleItem> myclients
		{ get { return (List<lib.ReferenceSimpleItem>)(new List<lib.ReferenceSimpleItem> { new lib.ReferenceSimpleItem(0, "Новый", true, true, lib.DomainObjectState.Sealed) }).Concat(CustomBrokerWpf.References.CustomersName).ToList(); } }

		private lib.TaskAsync.TaskAsync myexceltask;
		private RelayCommand myloaddata;
		public System.Windows.Input.ICommand LoadData
		{
			get { return myloaddata; }
		}
		private void LoadDataExec(object parametr)
		{
			if (!bool.Parse((string)parametr)) return;
			if (myexceltask == null)
			{
				myexceltask = new lib.TaskAsync.TaskAsync();
				myexceltask.Complete = () => { this.Save.Execute(null); };
			}
			if (!myexceltask.IsBusy)
			{
				Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
				fd.Multiselect = false;
				fd.CheckPathExists = true;
				fd.CheckFileExists = true;
				fd.Title = "Выбор файла с данными склада";
				fd.Filter = "Файлы Excel|*.xls;*.xlsx;*.xlsm;";
				if (fd.ShowDialog().Value)
				{
					myexceltask.DoProcessing = OnExcelImport;
					myexceltask.Run(new object[2] { fd.FileName, this.Stores.CurrentItem });
				}
			}
			else
			{
				System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
			}
		}
		private bool LoadDataCanExec(object parametr)
		{ return (myexceltask == null || !myexceltask.IsBusy); }
		private KeyValuePair<bool, string> OnExcelImport(object parm)
		{
			int maxr, usedr = 0, r = 2;

			object[] param = parm as object[];
			string filepath = (string)param[0];
			lib.ReferenceSimpleItem store = (lib.ReferenceSimpleItem)param[1];
			
			Excel.Application exApp = new Excel.Application();
			Excel.Application exAppProt = new Excel.Application();
			try
			{
				exApp.Visible = false;
				exApp.DisplayAlerts = false;
				exApp.ScreenUpdating = false;

				Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
				Excel.Worksheet exWh = exWb.Sheets[1];
				maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
				myexceltask.ProgressChange(5);

				StorageData item;
				decimal dval; Int16 bval;string str; DateTime sdate;
				string[] dateformats = new string[] { "yy.MM.dd", "yyyy.MM.dd", "dd.MM.yyyy", "dd.MM.yy", "yy-MM-dd", "yyyy-MM-dd", "dd-MM-yyyy", "dd-MM-yy" };
				if (store.Id == 1)
				{
					for (; r <= maxr; r++)
					{
						string storagePoint = (exWh.Cells[r, 1].Text as string).Trim();
						if (storagePoint.Length > 6) storagePoint = storagePoint.Substring(0, 6);
						if (storagePoint.Length > 0 && decimal.TryParse(storagePoint, out dval) && !(exWh.Cells[r, 17].Text as string).Trim().Equals("+", StringComparison.Ordinal))
						{
							str = (exWh.Cells[r, 2].Text as string).Trim();
							if (DateTime.TryParseExact(str, dateformats, System.Globalization.CultureInfo.GetCultureInfo("de-DE"), System.Globalization.DateTimeStyles.None, out sdate))
							{
								item = mysync.DomainCollection.FirstOrDefault((StorageData olditem) => { return string.Equals(olditem.Point, storagePoint) && DateTime.Equals(olditem.Date, sdate) && object.Equals(olditem.Storage, store); });
								if (item == null) item = new StorageData();
								if (item.Request == null)
								{
									item.Point = storagePoint;
									item.Date = sdate;
									str = (exWh.Cells[r, 3].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
									item.AgentName = str;
									str = (exWh.Cells[r, 4].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
									item.CustomerName = str;
									if (decimal.TryParse(exWh.Cells[r, 5].Value.ToString(), out dval))
										item.GrossWeight = dval;
									else
										throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 5].Address(false, false) + " к формату числа!");
									if (decimal.TryParse(exWh.Cells[r, 6].Value.ToString(), out dval))
										item.NetWeight = dval;
									else
										throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 6].Address(false, false) + " к формату числа!");
									if (Int16.TryParse(exWh.Cells[r, 7].Value.ToString(), out bval))
										item.CellNumber = bval;
									else
										throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " к формату целого числа!");
									if (decimal.TryParse(exWh.Cells[r, 8].Value.ToString(), out dval))
										item.Volume = dval;
									else
										throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 8].Address(false, false) + " к формату числа!");
									if (decimal.TryParse(exWh.Cells[r, 9].Value.ToString(), out dval))
										item.GoodValue = dval;
									else
										throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 9].Address(false, false) + " к формату числа!");
									str = exWh.Cells[r, 10].Text;
									if (str.Length > 0)
									{
										if (decimal.TryParse(str, out dval))
											item.Service = dval;
										else
											throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 10].Address(false, false) + " к формату числа!");
									}
									else item.FreightCost = 0;
									str = exWh.Cells[r, 11].Text;
									if (str.Length > 0)
										if (decimal.TryParse(str, out dval))
											item.Forwarding = dval;
										else
											throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 11].Address(false, false) + " к формату числа!");
									else item.FreightCost = 0;
									str = (exWh.Cells[r, 12].Text as string).Trim(); if (str.Length > 6) str = str.Substring(0, 6);
									item.ShipmentNumber = str;
									str = (exWh.Cells[r, 13].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
									item.Note = str;
									item.Store = exWh.Cells[r, 14].Text;
									str = (exWh.Cells[r, 15].Text as string).Trim(); if (str.Length > 180) str = str.Substring(0, 180);
									item.Doc = str;
									str = (exWh.Cells[r, 16].Text as string).Trim(); if (str.Length > 6) str = str.Substring(0, 6);
									item.FreightNumber = str;
									str = exWh.Cells[r, 18].Text;
									if (str.Length > 0)
										if (decimal.TryParse(str, out dval))
											item.FreightCost = dval;
										else
											throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 18].Address(false, false) + " к формату числа!");
									else item.FreightCost = 0;
									item.Storage = store;
									App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<StorageData>(mysync.DomainCollection.Add), item);
								}
								usedr++;
							}
							else
							{
								throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 2].Address(false, false) + " к формату даты!");
							}
						}
						myexceltask.ProgressChange(r, maxr, 0.99M, 0.05M);
					}
				}
				else
				{
					for (; r <= maxr; r++)
					{
						string storagePoint = (exWh.Cells[r, 2].Text as string).Trim();
						if (storagePoint.Length > 6) storagePoint = storagePoint.Substring(0, 6);
						if (storagePoint.Length > 0 && decimal.TryParse(storagePoint, out dval))
						{
							str = (exWh.Cells[r, 3].Text as string).Trim();
							if (DateTime.TryParseExact(str, dateformats, System.Globalization.CultureInfo.GetCultureInfo("de-DE"), System.Globalization.DateTimeStyles.None, out sdate))
							{
								item = mysync.DomainCollection.FirstOrDefault((StorageData olditem) => { return string.Equals(olditem.Point, storagePoint) && DateTime.Equals(olditem.Date, sdate) && object.Equals(olditem.Storage, store); });
								if (item == null) item = new StorageData();
								if (item.Request == null)
								{
									item.Point = storagePoint;
									item.Date = sdate;
									str = (exWh.Cells[r, 5].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
									item.CustomerName = str;
									str = (exWh.Cells[r, 7].Text as string).Trim(); if (str.Length > 100) str = str.Substring(0, 100);
									item.AgentName = str;
									str = (exWh.Cells[r, 8].Text as string).Trim();
									str = str.Substring(0, str.LastIndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, 0) + 1).Trim();
									if (Int16.TryParse(str, out bval))
										item.CellNumber = bval;
									else
										throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 8].Address(false, false) + " к формату целого числа!");
									if (decimal.TryParse(exWh.Cells[r, 9].Value.ToString(), out dval))
										item.GrossWeight = dval;
									else
										throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 9].Address(false, false) + " к формату числа!");
									if (decimal.TryParse(exWh.Cells[r, 10].Value.ToString(), out dval))
										item.NetWeight = dval;
									else
										throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 10].Address(false, false) + " к формату числа!");
									if (decimal.TryParse(exWh.Cells[r, 11].Value.ToString(), out dval))
										item.Volume = dval;
									else
										throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 11].Address(false, false) + " к формату числа!");
									str = (exWh.Cells[r, 12].Text as string).Trim(); if (str.Length > 180) str = str.Substring(0, 180);
									item.Doc = str;
									str = exWh.Cells[r, 6].Value.ToString().Trim(); if (str.Length > 100) str = str.Substring(0, 100);
									item.Note = str;
									str = exWh.Cells[r, 13].Value.ToString().Trim(); if (str.Length + item.Note.Length > 100) str = str.Substring(0, 100 - item.Note.Length);
									item.Note = item.Note + str;
									item.Storage = store;
									App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<StorageData>(mysync.DomainCollection.Add), item);
								}
								usedr++;
							}
							else
							{
								throw new ApplicationException("Не удалось привести значение ячейки Excel " + exWh.Cells[r, 3].Address(false, false) + " к формату даты!");
							}
						}
						myexceltask.ProgressChange(r, maxr, 0.99M, 0.05M);
					}
				}

				exWb.Close();
				exApp.Quit();

				myexceltask.ProgressChange(100);
				return new KeyValuePair<bool, string>(false, "Данные склада загружены. " + usedr.ToString() + " строк обработано.");
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
				throw new Exception("Ошибка в строке " + r.ToString() + ": " + ex.Message);
			}
			finally
			{
				exApp = null;
				if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
				exAppProt = null;
			}
		}

		private RelayCommand mymath;
		public System.Windows.Input.ICommand Math
		{ get { return mymath; } }
		private void MathExec(object parametr)
		{
			this.EndEdit();
			mymathsync.DomainCollection.Clear();
			foreach (StorageData data in mysync.DomainCollection)
			{
				mymathdbm.Data = data;
				mymathdbm.Fill();
				foreach (StorageMath math in mymathdbm.Collection)
					mymathsync.DomainCollection.Add(math);
			}
		}
		private bool MathCanExec(object parametr)
		{
			return myview.Count > 0;
		}

		private RelayCommand mymerge;
		public System.Windows.Input.ICommand Merge
		{ get { return mymerge; } }
		private void MergeExec(object parametr)
		{
			this.EndEdit();
			if (myview.CurrentPosition > -1)
			{
				if ((mymathview.CurrentPosition > -1) | (mymathview.Count == 1))
				{
					StorageDataVM storage = myview.CurrentItem as StorageDataVM;
					if (storage.Request==null)
					{
						StringBuilder err = new StringBuilder();
						StorageMathVM math = (mymathview.CurrentItem ?? mymathview.GetItemAt(0)) as StorageMathVM;
						if (math.Request.CellNumber.HasValue && (math.StorageData.CellNumber != math.Request.CellNumber.Value)) err.Append("Количество мест не совпадает " + math.StorageData.CellNumber.ToString() + " / " + math.Request.CellNumber.Value.ToString() + "\n");
						if (math.Request.Volume.HasValue && math.StorageData.Volume != math.Request.Volume.Value) err.Append("Объем не совпадает " + math.StorageData.Volume.ToString() + " / " + math.Request.Volume.Value.ToString() + "\n");
						if (math.Request.GoodValue.HasValue && math.StorageData.GoodValue.HasValue && math.StorageData.GoodValue.Value != math.Request.GoodValue.Value) err.Append("Стоимость не совпадает " + math.StorageData.GoodValue.Value.ToString() + " / " + math.Request.GoodValue.Value.ToString() + "\n");
						if (math.Request.OfficialWeight.HasValue && math.StorageData.GrossWeight != math.Request.OfficialWeight.Value) err.Append("Вес по документам не совпадает " + math.StorageData.GrossWeight.ToString() + " / " + math.Request.OfficialWeight.Value.ToString() + "\n");
						if (math.Request.ActualWeight.HasValue && math.StorageData.NetWeight.HasValue && math.StorageData.NetWeight.Value != math.Request.ActualWeight.Value) err.Append("Вес фактический не совпадает " + math.StorageData.NetWeight.Value.ToString() + " / " + math.Request.ActualWeight.Value.ToString() + "\n");
						if (err.Length > 0)
						{
							err.Append("\nСвязать склад и заявку?");
							if (System.Windows.MessageBox.Show(err.ToString(), "Привязка", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) != System.Windows.MessageBoxResult.Yes)
								return;
						}
						if (math.Request.DomainObject.Blocked || math.Request.DomainObject.Blocking())
						{
							this.AgentClientIdentify(math);
							mymathdbm.SaveItemChanches(math.Request.DomainObject); // сохраняем без транзакции изменения вложенных объектов сделанные не на складе (возможно)
							math.Request.StorePoint = math.StorageData.Point;
							math.Request.StoreDate = math.StorageData.Date;
							math.Request.StoreId = math.StorageData.Storage.Id; // установка склада
							if (!math.Request.CellNumber.HasValue || (math.StorageData.CellNumber != math.Request.CellNumber.Value)) math.Request.CellNumber = (short?)math.StorageData.CellNumber;
							if (!math.Request.Volume.HasValue || math.StorageData.Volume != math.Request.Volume) math.Request.Volume = math.StorageData.Volume;
							if (math.StorageData.GoodValue.HasValue && (!math.Request.GoodValue.HasValue || math.StorageData.GoodValue != math.Request.GoodValue)) math.Request.GoodValue = math.StorageData.GoodValue;
							if (!math.Request.OfficialWeight.HasValue || math.StorageData.GrossWeight != math.Request.OfficialWeight) math.Request.OfficialWeight = math.StorageData.GrossWeight;
							if (!math.Request.ActualWeight.HasValue || math.StorageData.NetWeight != math.Request.ActualWeight) math.Request.ActualWeight = math.StorageData.NetWeight;
							if (string.IsNullOrEmpty(math.Request.StoreNote)) math.Request.StoreNote = math.StorageData.Note;
							storage.Request = math.Request.DomainObject; // привязано, storage for if Reject
							this.TransactionSave(storage, math);
						}
					}
					else
					{
						System.Windows.MessageBox.Show("Склад уже привязан к заявке!", "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
					}
				}
				else
				{
					System.Windows.MessageBox.Show("Выдилите заявку", "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
				}
			}
			else
			{
				System.Windows.MessageBox.Show("Выдилите строку склада", "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
			}
		}
		private bool MergeCanExec(object parametr)
		{
			return mymathenabled;
		}

		private RelayCommand myrequestcreate;
		public System.Windows.Input.ICommand RequestCreate
		{
			get { return myrequestcreate; }
		}
		private void RequestCreateExec(object parametr)
		{
			if (myview.CurrentPosition > -1)
			{
				this.EndEdit();
				StorageDataVM storage = myview.CurrentItem as StorageDataVM;
				if (storage.Request == null)
				{
					StorageMathVM math = new StorageMathVM(new StorageMath(new Request(),storage.DomainObject));
					this.AgentClientIdentify(math);
					math.Request.StorePoint = math.StorageData.Point;
					math.Request.StoreDate = math.StorageData.Date;
					math.Request.CellNumber = (short?)math.StorageData.CellNumber;
					math.Request.Volume = math.StorageData.Volume;
					if (math.StorageData.GoodValue.HasValue) math.Request.GoodValue = math.StorageData.GoodValue;
					math.Request.OfficialWeight = math.StorageData.GrossWeight;
					math.Request.ActualWeight = math.StorageData.NetWeight;
					math.Request.StoreNote = math.StorageData.Note;
					math.Request.StoreId = math.StorageData.Storage.Id;
					storage.Request = math.Request.DomainObject;
					mymathsync.ViewModelCollection.Add(math);
					this.TransactionSave(storage, math);
				}
				else
				{
					System.Windows.MessageBox.Show("Склад уже привязан к заявке!", "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
				}
			}
			else
			{
				System.Windows.MessageBox.Show("Выдилите строку склада", "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
			}
		}
		private bool RequestCreateCanExec(object parametr)
		{
			return mymathenabled;
		}

		private RelayCommand mysever;
		public System.Windows.Input.ICommand Sever
		{
			get { return mysever; }
		}
		private void SeverExec(object parametr)
		{
			if (myview.CurrentPosition > -1)
			{
				if (mymathview.Count > 0)
				{
					StorageDataVM storage = myview.CurrentItem as StorageDataVM;
					StorageMathVM math = (mymathview.CurrentItem ?? mymathview.GetItemAt(0)) as StorageMathVM;
					if (storage?.Request != null && System.Windows.MessageBox.Show("Развязать заявку и информацию со склада?", "Привязка", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes && (math.Request.DomainObject.Blocked || math.Request.DomainObject.Blocking()))
					{
						storage.Request = null;
						mymathdbm.SaveItemChanches(math.Request.DomainObject); // сохраняем без транзакции изменения вложенных объектов сделанные не на складе (возможно)
						math.Request.StorePoint = null;
						math.Request.StoreDate = null;
						math.Request.StoreId = null;
						math.Request.StoreNote = null;
						this.TransactionSave(storage, math);
					}
				}
				else
				{
					System.Windows.MessageBox.Show("Выполните подбор заявок", "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
				}
			}
			else
			{
				System.Windows.MessageBox.Show("Выдилите строку склада", "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
			}
		}
		private bool SeverCanExec(object parametr)
		{
			return !mymathenabled;
		}

		private RelayCommand myrequestdel;
		public System.Windows.Input.ICommand RequestDelete
		{
			get { return myrequestdel; }
		}
		private void RequestDeleteExec(object parametr)
		{
			System.Collections.ICollection deleting = parametr as System.Collections.ICollection;
			if (deleting?.Count > 0)
			{
				List<StorageMathVM> nomaths = new List<StorageMathVM>();
				foreach (StorageMathVM nomath in deleting) // коллекция может быть изменена
						if(nomath.StorageData.Request==null) nomaths.Add(nomath);
				foreach (StorageMathVM nomath in nomaths)
					mymathsync.ViewModelCollection.Remove(nomath);
			}
		}
		private bool RequestDeleteCanExec(object parametr)
		{
			return (parametr as System.Collections.ICollection)?.Count > 0;
		}

		protected override bool CanAddData(object parametr)
		{
			return false; ;
		}
		protected override bool CanDeleteData(object parametr)
		{
			return parametr is IList<object> && (parametr as IList<object>).Count>0;
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
			this.MathExec(null);
		}
		protected override void SettingView()
		{
			myview.SortDescriptions.Add(new System.ComponentModel.SortDescription("IsJoin", System.ComponentModel.ListSortDirection.Descending));
			myview.CurrentChanged += StorageData_CurrentChanged;
		}

		private void StorageData_CurrentChanged(object sender, EventArgs e)
		{
			if(myview.CurrentItem is StorageDataVM)
			{
				StorageDataVM current = myview.CurrentItem as StorageDataVM;
				if (current.Request == null)
				{
					mymathview.Filter = (object item) => { StorageMathVM math = item as StorageMathVM; return lib.ViewModelViewCommand.ViewFilterDefault(item) && math?.StorageData == current.DomainObject && string.IsNullOrEmpty(math?.Request.DomainObject.StorePoint); };
					mymathenabled = true;
				}
				else
				{
					mymathview.Filter = (object item) => { StorageMathVM math = item as StorageMathVM; return lib.ViewModelViewCommand.ViewFilterDefault(item) && math?.StorageData == current.DomainObject && math?.Request.DomainObject == current.Request; };
					mymathenabled = false;
				}
			}
		}
		public override bool SaveDataChanges()
		{
			bool isSuccess = true, isvalid;
			System.Text.StringBuilder err = new System.Text.StringBuilder();
			err.AppendLine("Изменения не сохранены");
			mymathdbm.Errors.Clear();
			foreach (StorageMathVM item in mymathsync.ViewModelCollection)
			{
				isvalid = !item.Request.IsDirty || item.Request.Validate(true);
				if (!isvalid)
					err.AppendLine(item.Request.Errors);
				isSuccess &= isvalid;
			}
			if (!mymathdbm.SaveCollectionChanches())
			{
				isSuccess = false;
				err.AppendLine(mymathdbm.ErrorMessage);
			}
			if (!isSuccess)
				this.PopupText = err.ToString();

			return isSuccess & base.SaveDataChanges();
		}

		private void AgentClientIdentify(StorageMathVM math)
		{
			Agent agent;
			myadbm.Errors.Clear();
			myadbm.Name = math.StorageData.AgentName;
			agent = myadbm.GetFirst();
			if (myadbm.Errors.Count > 0)
			{
				System.Windows.MessageBox.Show(myadbm.ErrorMessage, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
			}
			if (agent == null)
			{
				if (math.Request.Agent != null)
				{
					myadbm.Errors.Clear();
					math.Request.Agent.Aliases.Add(new AgentAlias(0, 0, lib.DomainObjectState.Added, math.Request.Agent, math.StorageData.AgentName));
					myadbm.SaveItemChanches(math.Request.Agent);
					if (myadbm.Errors.Count > 0)
						System.Windows.MessageBox.Show(myadbm.ErrorMessage, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
				}
				else
				{
					StorageAgentIdentify win = new StorageAgentIdentify();
					win.AgentName = math.StorageData.AgentName;
					win.Agents = myagents;
					foreach (System.Windows.Window window in App.Current.Windows)
						if (window.IsActive)
						{ win.Owner = window; break; }
					if (win.ShowDialog() ?? false)
					{
						if ((win.Agent?.Id??0M) == 0)
						{
							agent = new Agent(math.StorageData.AgentName, math.StorageData.AgentName);
							myadbm.SaveItemChanches(agent);
							CustomBrokerWpf.References.AgentNames.Refresh();
							CustomBrokerWpf.References.CustomersName.RefreshViews();
							CustomBrokerWpf.References.AgentStore.UpdateItem(agent);
						}
						else
						{
							agent = CustomBrokerWpf.References.AgentStore.GetItemLoad(win.Agent.Id, out var errors);
							if (errors.Count > 0)
								System.Windows.MessageBox.Show(errors[0].Message, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
							if (agent != null)
							{
								agent.Aliases.Add(new AgentAlias(0, 0, lib.DomainObjectState.Added, agent, math.StorageData.AgentName));
								myadbm.SaveItemChanches(agent);
								if (myadbm.Errors.Count > 0)
									System.Windows.MessageBox.Show(myadbm.ErrorMessage, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
							}
						}
						math.Request.Agent = agent;
					}
				}
			}
			else if(math.Request.Agent != agent && (math.Request.Agent == null || System.Windows.MessageBox.Show("Сменить поставщика " + math.Request.Agent.Name + " на " + agent.Name + "?", "Привязка", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question)==System.Windows.MessageBoxResult.Yes))
				math.Request.Agent = agent;
			
			Customer customer;
			mycdbm.Errors.Clear();
			mycdbm.Name = math.StorageData.CustomerName;
			customer = mycdbm.GetFirst();
			if (mycdbm.Errors.Count > 0)
			{
				System.Windows.MessageBox.Show(mycdbm.ErrorMessage, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
			}
			if (customer == null)
			{
				if (math.Request.Customer != null)
				{
					mycdbm.Errors.Clear();
					math.Request.Customer.Aliases.Add(new Alias(0, lib.DomainObjectState.Added, math.Request.Customer.Id, math.StorageData.CustomerName));
					mycdbm.SaveItemChanches(math.Request.Customer);
					if (mycdbm.Errors.Count > 0)
						System.Windows.MessageBox.Show(mycdbm.ErrorMessage, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
				}
				else
				{
					StorageAgentIdentify win = new StorageAgentIdentify();
					win.Title = "Привязка клиента";
					win.AgentName = math.StorageData.CustomerName;
					win.Agents = myclients;
					foreach (System.Windows.Window window in App.Current.Windows)
						if (window.IsActive)
						{ win.Owner = window; break; }
					if (win.ShowDialog() ?? false)
					{
						if ((win.Agent?.Id ?? 0M) == 0)
						{
							customer = new Customer(math.StorageData.CustomerName, math.StorageData.CustomerName);
							mycdbm.SaveItemChanches(customer);
							CustomBrokerWpf.References.CustomersName.Refresh();
							CustomBrokerWpf.References.CustomersName.RefreshViews();
							CustomBrokerWpf.References.CustomerStore.UpdateItem(customer);
						}
						else
						{
							customer = CustomBrokerWpf.References.CustomerStore.GetItemLoad(win.Agent.Id, out var errors);
							if (errors.Count > 0)
								System.Windows.MessageBox.Show(errors[0].Message, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
							if (customer != null)
							{
								customer.Aliases.Add(new Alias(0, lib.DomainObjectState.Added, customer.Id, math.StorageData.CustomerName));
								mycdbm.SaveItemChanches(customer);
								if (mycdbm.Errors.Count > 0)
									System.Windows.MessageBox.Show(mycdbm.ErrorMessage, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
							}
						}
						math.Request.Customer = customer;
					}
				}
			}
			else if (math.Request.Customer != customer && (math.Request.Customer == null || System.Windows.MessageBox.Show("Сменить клиента " + math.Request.Customer.Name + " на " + customer.Name + "?", "Привязка", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes))
				math.Request.Customer = customer;
		}
		private void TransactionSave(StorageDataVM storage, StorageMathVM math)
		{
			using (SqlConnection connection = new SqlConnection(mymaindbm.ConnectionString))
			{
				try
				{
					connection.Open();
					mymaindbm.Errors.Clear();
					mymaindbm.Command.Connection = connection;
					mymaindbm.TransactionArea=lib.TransactionArea.Collection;
					mymaindbm.Transaction = connection.BeginTransaction();
					mymathdbm.Errors.Clear();
					mymathdbm.Connection = connection;
					mymathdbm.TransactionArea = lib.TransactionArea.Collection;
					mymathdbm.Transaction = mymaindbm.Transaction;
					mymathdbm.SaveItemChanches(math.Request.DomainObject);
					if (mymathdbm.Errors.Count > 0)
					{
						mymaindbm.Transaction.Rollback();
						math.Request.RejectChanges();
						storage.RejectChanges();
						System.Windows.MessageBox.Show(mymathdbm.ErrorMessage, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
					}
					else
					{
						mymaindbm.SaveItemChanches(math.StorageData);
						if (mymathdbm.Errors.Count > 0)
						{
							mymaindbm.Transaction.Rollback();
							math.Request.RejectChanges();
							storage.RejectChanges();
							System.Windows.MessageBox.Show(mymaindbm.ErrorMessage, "Привязка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
						}
						else
						{
							mymaindbm.Transaction.Commit();
							math.Request.DomainObject.AcceptChanches();
							math.StorageData.AcceptChanches();
						}
					}
					connection.Close();
				}
				catch (Exception ex)
				{
					mymaindbm.Transaction.Rollback();
					if (ex is System.Data.SqlClient.SqlException)
					{
						System.Data.SqlClient.SqlException sqlex = ex as System.Data.SqlClient.SqlException;
						if (sqlex.Number > 49999)
						{
							System.Windows.MessageBox.Show(sqlex.Message, "Сохранение привязки", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
						}
						else
						{
							System.Text.StringBuilder errs = new System.Text.StringBuilder();
							foreach (System.Data.SqlClient.SqlError sqlerr in sqlex.Errors)
							{
								errs.Append(sqlerr.Message + "\n");
							}
							System.Windows.MessageBox.Show(errs.ToString(), "Сохранение привязки", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
						}
					}
					else
					{
						System.Windows.MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение привязки", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
					}
				}
			}
			if (storage.Request != null)
			{
				List<StorageMath> nomaths = new List<StorageMath>();
				foreach (StorageMath nomath in mymathsync.DomainCollection)
				{
					if (nomath != math.DomainObject && nomath.StorageData == math.StorageData)
						nomaths.Add(nomath);
				}
				foreach (StorageMath nomath in nomaths)
					mymathsync.DomainCollection.Remove(nomath);
				mymathview.Filter = (object item) => { StorageMathVM imath = item as StorageMathVM; return lib.ViewModelViewCommand.ViewFilterDefault(item) && imath?.StorageData == storage.DomainObject && imath?.Request.DomainObject == storage.Request; };
				mymathenabled = false;
			}
			else
			{
				mymathview.Filter = (object item) => { StorageMathVM imath = item as StorageMathVM; return lib.ViewModelViewCommand.ViewFilterDefault(item) && imath?.StorageData == storage.DomainObject && string.IsNullOrEmpty(imath?.Request.DomainObject.StorePoint); };
				mymathenabled = true;
			}
		}
	}
}
