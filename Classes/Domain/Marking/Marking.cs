using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Marking
{
	public class Marking : lib.DomainBaseStamp
	{
		public Marking(int id, long stamp, DateTime? updated, string updater, lib.DomainObjectState state
			, string brand, string color, string country, string ean13, string filename, long gtin, string inn, string materialdown, string materialin, string materialup
			, string productname, string producttype, DateTime published, string size, string tnved, string vendorcode
			) : base(id, stamp, updated, updater, state, true)
		{
			brand = mybrand;
			color = mycolor;
			country=mycountry;
			ean13 = myean13;
			filename = myfilename;
			gtin = mygtin;
			inn=myinn;
			materialdown = mymaterialdown;
			materialin = mymaterialin;
			materialup = mymaterialup;
			productname = myproductname;
			producttype = myproducttype;
			published = mypublished;
			size = mysize;
			tnved = mytnved;
			vendorcode = myvendorcode;
		}
		private string mybrand;
		public string Brand
		{ set { SetProperty<string>(ref mybrand, value); } get{return mybrand;}} 
		private string mycolor;
		public string Color
		{ set { SetProperty<string>(ref mycolor, value); } get { return mycolor; } }
		private string mycountry;
		public string Country
		{ set { SetProperty<string>(ref mycountry, value); } get { return mycountry;}}
		private string myean13;
		public string Ean13
		{ set { SetProperty<string>(ref myean13, value); } get { return myean13; } }
		private string myfilename;
		public string FileName
		{ set { SetProperty<string>(ref myfilename, value); } get { return myfilename;}}
		private long mygtin;
		public long Gtin
		{ set { SetProperty<long>(ref mygtin,value); } get { return mygtin;}}
		private string myinn;
		public string Inn
		{ set { SetProperty<string>(ref myinn,value); } get { return myinn;}}
		private string mymaterialdown;
		public string MaterialDown
		{ set { SetProperty<string>(ref mymaterialdown,value); } get { return mymaterialdown;}}
		private string mymaterialin;
		public string MaterialInn
		{ set { SetProperty<string>(ref mymaterialin, value); } get { return mymaterialin;}}
		private string mymaterialup;
		public string MaterialUp
		{ set { SetProperty<string>(ref mymaterialup,value); } get { return mymaterialup;}}
		private string myproductname;
		public string ProductName
		{ set { SetProperty<string>(ref myproductname,value); } get { return myproductname;}}
		private string myproducttype;
		public string ProductType
		{ set { SetProperty<string>(ref myproducttype, value); } get { return myproducttype; } }
		private DateTime mypublished;
		public DateTime Published
		{ set { SetProperty<DateTime>(ref mypublished,value); } get { return mypublished;}}
		private string mysize;
		public string Size
		{ set { SetProperty<string>(ref mysize, value); } get { return mysize;}}
		private string mytnved;
		public string Tnved
		{ set { SetProperty<string>(ref mytnved,value); } get { return mytnved;}}
		private string myvendorcode;
		public string VendorCode
		{ set { SetProperty<string>(ref myvendorcode,value); } get { return myvendorcode;}}

		public override bool ValidateProperty(string propertyname, object value, out string errmsg, out byte errmsgkey)
		{
			bool isvalid = true;
			errmsg = null;
			errmsgkey = 0;
			switch (propertyname)
			{
				case nameof(this.Brand):
					string str = (string)value;
					if (string.IsNullOrEmpty(str))
						errmsg = "Не указана торговая марка!";
					break;
				case nameof(this.Ean13):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
						errmsg = "Не указан EAN-13!";
					break;
				case nameof(this.FileName):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
						errmsg = "Не указано имя файла(поставка)!";
					break;
				case nameof(this.Gtin):
					long? lng = (long?)value;
					if (!lng.HasValue || lng.Value==0L)
						errmsg = "Не указан GTIN!";
					break;
				case nameof(this.Inn):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
						errmsg = "Не указан ИНН!";
					break;
				case nameof(this.MaterialUp):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
						errmsg = "Не указан состав сырья(материал верха)!";
					break;
				case nameof(this.ProductName):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
						errmsg = "Не указано наименование товара!";
					break;
				case nameof(this.ProductType):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
						errmsg = "Не указан вид изделия!";
					break;
				case nameof(this.Published):
					DateTime? date = (DateTime?)value;
					if (!date.HasValue)
						errmsg = "Не указана дата публикации!";
					break;
				case nameof(this.Tnved):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
						errmsg = "Не указан ТНВЭД!";
					break;
				case nameof(this.VendorCode):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
						errmsg = "Не указан артикул (модель производителя)!";
					break;
			}
			return isvalid;
		}
		protected override void PropertiesUpdate(lib.DomainBaseReject sample)
		{
			Marking temp = sample as Marking;
			this.Brand = temp.Brand;
			this.Color = temp.Color;
			this.Country = temp.Country;
			this.Ean13 = temp.Ean13;
			this.FileName = temp.FileName;
			this.Gtin = temp.Gtin;
			this.Inn = temp.Inn;
			this.MaterialDown=temp.MaterialDown;
			this.MaterialInn=temp.MaterialInn;
			this.MaterialUp=temp.MaterialUp;
			this.ProductName=temp.ProductName;
			this.ProductType=temp.ProductType;
			this.Published=temp.Published;
			this.Size=temp.Size;
			this.Tnved=temp.Tnved;
			this.VendorCode=temp.VendorCode;
		}
		protected override void RejectProperty(string property, object value)
		{
			switch (property)
			{
				case nameof(this.Brand):
					mybrand = (string)value;
					break;
				case nameof(this.Color):
					mycolor = (string)value;
					break;
				case nameof(this.Country):
					mycountry = (string)value;
					break;
				case nameof(this.Ean13):
					myean13 = (string)value;
					break;
				case nameof(this.FileName):
					myfilename = (string)value;
					break;
				case nameof(this.Gtin):
					mygtin = (long)value;
					break;
				case nameof(this.Inn):
					myinn = (string)value;
					break;
				case nameof(this.MaterialDown):
					mymaterialdown = (string)value;
					break;
				case nameof(this.MaterialInn):
					mymaterialin = (string)value;
					break;
				case nameof(this.MaterialUp):
					mymaterialup = (string)value;
					break;
				case nameof(this.ProductName):
					myproductname = (string)value;
					break;
				case nameof(this.ProductType):
					myproducttype = (string)value;
					break;
				case nameof(this.Published):
					mypublished = (DateTime)value;
					break;
				case nameof(this.Size):
					mysize = (string)value;
					break;
				case nameof(this.Tnved):
					mytnved = (string)value;
					break;
				case nameof(this.VendorCode):
					myvendorcode = (string)value;
					break;
			}
		}
	}

	public class MarkingDBM : lib.DBManagerStamp<Marking>
	{
		public MarkingDBM()
		{
			this.ConnectionString = CustomBrokerWpf.References.ConnectionString;
			base.NeedAddConnection = false;

			SelectCommandText = "dbo.Marking_sp";
			InsertCommandText = "dbo.MarkingAdd_sp";
			UpdateCommandText = "dbo.MarkingUpd_sp";
			DeleteCommandText = "dbo.MarkingDel_sp";

			SelectParams = new SqlParameter[]
			{
				new SqlParameter("@id", System.Data.SqlDbType.Int),
				new SqlParameter("@filter", System.Data.SqlDbType.Int){ Value = 0},
			};
			myinsertparams = new SqlParameter[]
			{
				myinsertparams[0],
				myinsertupdateparams[0]
			};
			myupdateparams = new SqlParameter[]
			{
				myupdateparams[0]
				,new SqlParameter("@brandupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@colorupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@countryupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@ean13upd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@filenameupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@gtinupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@innupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@materialdownupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@materialinupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@materialupupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@productnameupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@producttypeupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@publishedupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@sizeupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@tnvedupd", System.Data.SqlDbType.Bit)
				,new SqlParameter("@vendorcodeupd", System.Data.SqlDbType.Bit)
			};
			myinsertupdateparams = new SqlParameter[]
			{
				new SqlParameter("@brand", System.Data.SqlDbType.NVarChar,128)
				,new SqlParameter("@color", System.Data.SqlDbType.NVarChar,80)
				,new SqlParameter("@country", System.Data.SqlDbType.NVarChar,110)
				,new SqlParameter("@ean13", System.Data.SqlDbType.NVarChar,128)
				,new SqlParameter("@filename", System.Data.SqlDbType.NVarChar,128)
				,new SqlParameter("@gtin", System.Data.SqlDbType.BigInt)
				,new SqlParameter("@inn", System.Data.SqlDbType.NVarChar,12)
				,new SqlParameter("@materialdown", System.Data.SqlDbType.NVarChar,128)
				,new SqlParameter("@materialin", System.Data.SqlDbType.NVarChar,128)
				,new SqlParameter("@materialup", System.Data.SqlDbType.NVarChar,128)
				,new SqlParameter("@productname", System.Data.SqlDbType.NVarChar,1024)
				,new SqlParameter("@producttype", System.Data.SqlDbType.NVarChar,100)
				,new SqlParameter("@published", System.Data.SqlDbType.DateTime2)
				,new SqlParameter("@size", System.Data.SqlDbType.NVarChar,128)
				,new SqlParameter("@tnved", System.Data.SqlDbType.NVarChar,10)
				,new SqlParameter("@vendorcode", System.Data.SqlDbType.NVarChar,50)
			};
		}

		private lib.SQLFilter.SQLFilter myfilter;
		public lib.SQLFilter.SQLFilter Filter
		{ set { myfilter = value; } get { return myfilter; } }

		protected override void CancelLoad()
		{
		}
		protected override Marking CreateItem(SqlDataReader reader, SqlConnection addcon)
		{
			return new Marking(reader.GetInt32(this.Fields["id"]), reader.GetInt64(this.Fields["stamp"]), reader.GetDateTime(this.Fields["updated"]), reader.GetString(this.Fields["updater"]), lib.DomainObjectState.Unchanged
				,reader.GetString(this.Fields["brand"])
				, reader.IsDBNull(this.Fields["color"]) ? null : reader.GetString(this.Fields["color"])
				, reader.IsDBNull(this.Fields["country"]) ? null : reader.GetString(this.Fields["country"])
				, reader.GetString(this.Fields["ean13"])
				, reader.GetString(this.Fields["filename"])
				, reader.GetInt64(this.Fields["gtin"])
				, reader.GetString(this.Fields["inn"])
				, reader.IsDBNull(this.Fields["materialdown"]) ? null : reader.GetString(this.Fields["materialdown"])
				, reader.IsDBNull(this.Fields["materialin"]) ? null : reader.GetString(this.Fields["materialin"])
				, reader.GetString(this.Fields["materialup"])
				, reader.GetString(this.Fields["productname"])
				, reader.GetString(this.Fields["producttype"])
				, reader.GetDateTime(this.Fields["published"])
				, reader.IsDBNull(this.Fields["size"]) ? null : reader.GetString(this.Fields["size"])
				, reader.GetString(this.Fields["tnved"])
				, reader.GetString(this.Fields["vendorcode"])
				);
		}
		protected override void GetOutputSpecificParametersValue(Marking item)
		{
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

		protected override bool SetSpecificParametersValue(Marking item)
		{
			foreach (SqlParameter par in this.UpdateParams)
				switch (par.ParameterName)
				{
					case "@brandupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.Brand));
						break;
					case "@colorupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.Color));
						break;
					case "@countryupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.Country));
						break;
					case "@ean13upd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.Ean13));
						break;
					case "@filenameupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.FileName));
						break;
					case "@gtinupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.Gtin));
						break;
					case "@innupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.Inn));
						break;
					case "@materialdownupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.MaterialDown));
						break;
					case "@materialinupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.MaterialInn));
						break;
					case "@materialupupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.MaterialUp));
						break;
					case "@productnameupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.ProductName));
						break;
					case "@producttypeupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.ProductType));
						break;
					case "@publishedupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.Published));
						break;
					case "@sizeupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.Size));
						break;
					case "@tnvedupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.Tnved));
						break;
					case "@vendorcodeupd":
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.VendorCode));
						break;
				}
			foreach (SqlParameter par in this.InsertUpdateParams)
				switch (par.ParameterName)
				{
					case "@brand":
						par.Value = item.Brand;
						break;
					case "@color":
						par.Value = item.Color;
						break;
					case "@country":
						par.Value = item.Country;
						break;
					case "@ean13":
						par.Value = item.Ean13;
						break;
					case "@filename":
						par.Value = item.FileName;
						break;
					case "@gtin":
						par.Value = item.Gtin;
						break;
					case "@inn":
						par.Value = item.Inn;
						break;
					case "@materialdown":
						par.Value = item.MaterialDown;
						break;
					case "@materialin":
						par.Value = item.MaterialInn;
						break;
					case "@materialup":
						par.Value = item.MaterialUp;
						break;
					case "@productname":
						par.Value = item.ProductName;
						break;
					case "@producttype":
						par.Value = item.ProductType;
						break;
					case "@published":
						par.Value = item.Published;
						break;
					case "@size":
						par.Value = item.Size;
						break;
					case "@tnved":
						par.Value = item.Tnved;
						break;
					case "@vendorcode":
						par.Value = item.VendorCode;
						break;
				}
			return true;
		}
	}

	public class MarkingVM : lib.ViewModelErrorNotifyItem<Marking>
	{
		public MarkingVM(Marking model) : base(model)
		{
			ValidetingProperties.AddRange(new string[] { nameof(this.Brand), nameof(this.Ean13), nameof(this.FileName), nameof(this.Gtin), nameof(this.Inn), nameof(this.MaterialUp), nameof(this.ProductName), nameof(this.ProductType), nameof(this.Published), nameof(this.Tnved), nameof(this.VendorCode) });
		}

		private string mybrand;
		public string Brand
		{
			set { SetPropertyValidateNotNull(ref mybrand, ()=>{ this.DomainObject.Brand = value; }, value); }
			get { return GetProperty(mybrand, null); }
		}
		public string Color
		{
			set { SetProperty(this.DomainObject.Color, (string v) => { this.DomainObject.Color = v; }, value); }
			get { return GetProperty(this.DomainObject.Color, null); }
		}
		public string Country
		{
			set { SetProperty(this.DomainObject.Country, (string v) => { this.DomainObject.Country = v; }, value); }
			get { return GetProperty(this.DomainObject.Country, null); }
		}
		public string Ean13
		{
			set { SetPropertyNotNull(this.DomainObject.Ean13, (string v) => { this.DomainObject.Ean13 = v; }, value); }
			get { return GetProperty(this.DomainObject.Ean13, null); }
		}
		public string FileName
		{
			set { SetPropertyNotNull(this.DomainObject.FileName, (string v) => { this.DomainObject.FileName = v; }, value); }
			get { return GetProperty(this.DomainObject.FileName, null); }
		}
		private long? mygtin;
		public long?  Gtin
		{
			set { SetPropertyValidateNotNull<long?>(ref mygtin, () => { this.DomainObject.Gtin = value.Value; }, value); }
			get { return GetProperty(mygtin, (long?)null); }
		}
		public string Inn
		{
			set { SetPropertyNotNull(this.DomainObject.Inn, (string v) => { this.DomainObject.Inn = v; }, value); }
			get { return GetProperty(this.DomainObject.Inn, null); }
		}
		public string MaterialDown
		{
			set { SetProperty(this.DomainObject.MaterialDown, (string v) => { this.DomainObject.MaterialDown = v; }, value); }
			get { return GetProperty(this.DomainObject.MaterialDown, null); }
		}
		public string MaterialInn
		{
			set { SetProperty(this.DomainObject.MaterialInn, (string v) => { this.DomainObject.MaterialInn = v; }, value); }
			get { return GetProperty(this.DomainObject.MaterialInn, null); }
		}
		public string MaterialUp
		{
			set { SetPropertyNotNull(this.DomainObject.MaterialUp, (string v) => { this.DomainObject.MaterialUp = v; }, value); }
			get { return GetProperty(this.DomainObject.MaterialUp, null); }
		}
		public string ProductName
		{
			set { SetPropertyNotNull(this.DomainObject.ProductName, (string v) => { this.DomainObject.ProductName = v; }, value); }
			get { return GetProperty(this.DomainObject.ProductName, null); }
		}
		public string ProductType
		{
			set { SetPropertyNotNull(this.DomainObject.ProductType, (string v) => { this.DomainObject.ProductType = v; }, value); }
			get { return GetProperty(this.DomainObject.ProductType, null); }
		}
		public DateTime? Published
		{
			set { SetProperty<DateTime?>(this.DomainObject.Published, (DateTime? v) => { this.DomainObject.Published = v.Value; }, value); }
			get { return GetProperty(this.DomainObject.Published, (DateTime?)null); }
		}
		public string Size
		{
			set { SetProperty(this.DomainObject.Size, (string v) => { this.DomainObject.Size = v; }, value); }
			get { return GetProperty(this.DomainObject.Size, null); }
		}
		public string Tnved
		{
			set { SetPropertyNotNull(this.DomainObject.Tnved, (string v) => { this.DomainObject.Tnved = v; }, value); }
			get { return GetProperty(this.DomainObject.Tnved, null); }
		}
		public string VendorCode
		{
			set { SetPropertyNotNull(this.DomainObject.VendorCode, (string v) => { this.DomainObject.VendorCode = v; }, value); }
			get { return GetProperty(this.DomainObject.VendorCode, null); }
		}

		protected override bool DirtyCheckProperty()
		{
			return false;
		}
		protected override void DomainObjectPropertyChanged(string property)
		{
		}
		protected override void InitProperties()
		{
		}

		protected override void RejectProperty(string property, object value)
		{
			switch (property)
			{
				case nameof(this.Brand):
					if (mybrand != this.DomainObject.Brand)
						mybrand = this.DomainObject.Brand;
					else
						this.Brand = (string)value;
					break;
				case nameof(this.Color):
					this.DomainObject.Color = (string)value;
					break;
				case nameof(this.Country):
					this.DomainObject.Country = (string)value;
					break;
				case nameof(this.Ean13):
					this.DomainObject.Ean13 = (string)value;
					break;
				case nameof(this.FileName):
					this.DomainObject.FileName = (string)value;
					break;
				case nameof(this.Gtin):
					if (mygtin != this.DomainObject.Gtin)
						mygtin = this.DomainObject.Gtin;
					else
						this.Gtin = (long?)value;
					break;
				case nameof(this.Inn):
					this.DomainObject.Inn = (string)value;
					break;
				case nameof(this.MaterialDown):
					this.DomainObject.MaterialDown = (string)value;
					break;
				case nameof(this.MaterialInn):
					this.DomainObject.MaterialInn = (string)value;
					break;
				case nameof(this.MaterialUp):
					this.DomainObject.MaterialUp = (string)value;
					break;
				case nameof(this.ProductName):
					this.DomainObject.ProductName = (string)value;
					break;
				case nameof(this.ProductType):
					this.DomainObject.ProductType = (string)value;
					break;
				case nameof(this.Published):
					DateTime? date = (DateTime?)value;
					if(date.HasValue) this.DomainObject.Published = date.Value;
					break;
				case nameof(this.Size):
					this.DomainObject.Size = (string)value;
					break;
				case nameof(this.Tnved):
					this.DomainObject.Tnved = (string)value;
					break;
				case nameof(this.VendorCode):
					this.DomainObject.VendorCode = (string)value;
					break;
			}
		}
		protected override bool ValidateProperty(string propertyname, bool inform = true)
		{
			bool isvalid = true;
			string errmsg = null;
			switch (propertyname)
			{
				case nameof(this.Brand):
					isvalid = this.DomainObject.ValidateProperty(propertyname, mybrand, out errmsg, out _);
					break;
				case nameof(this.Ean13):
					isvalid = this.DomainObject.ValidateProperty(propertyname, this.Ean13, out errmsg, out _);
					break;
				case nameof(this.FileName):
					isvalid = this.DomainObject.ValidateProperty(propertyname, this.FileName, out errmsg, out _);
					break;
				case nameof(this.Gtin):
					isvalid = this.DomainObject.ValidateProperty(propertyname, mygtin, out errmsg, out _);
					break;
				case nameof(this.Inn):
					isvalid = this.DomainObject.ValidateProperty(propertyname, this.Inn, out errmsg, out _);
					break;
				case nameof(this.MaterialUp):
					isvalid = this.DomainObject.ValidateProperty(propertyname, this.MaterialUp, out errmsg, out _);
					break;
				case nameof(this.ProductName):
					isvalid = this.DomainObject.ValidateProperty(propertyname, this.ProductName, out errmsg, out _);
					break;
				case nameof(this.ProductType):
					isvalid = this.DomainObject.ValidateProperty(propertyname, this.ProductType, out errmsg, out _);
					break;
				case nameof(this.Published):
					isvalid = this.DomainObject.ValidateProperty(propertyname, this.Published, out errmsg, out _);
					break;
				case nameof(this.Tnved):
					isvalid = this.DomainObject.ValidateProperty(propertyname, this.Tnved, out errmsg, out _);
					break;
				case nameof(this.VendorCode):
					isvalid = this.DomainObject.ValidateProperty(propertyname, this.VendorCode, out errmsg, out _);
					break;
			}
			if (isvalid)
				this.ClearErrorMessageForProperty(propertyname);
			else if (inform) AddErrorMessageForProperty(propertyname, errmsg);
			return isvalid;
		}
	}

	public class MarkingSynchronizer : lib.ModelViewCollectionsSynchronizer<Marking, MarkingVM>
	{
		protected override Marking UnWrap(MarkingVM wrap)
		{
			return wrap.DomainObject as Marking;
		}
		protected override MarkingVM Wrap(Marking fill)
		{
			return new MarkingVM(fill);
		}
	}

	public class MarkingViewCommader : lib.ViewModelViewCommand, IDisposable
	{
		internal MarkingViewCommader()
		{
			myfilter = new lib.SQLFilter.SQLFilter("marking", "AND", CustomBrokerWpf.References.ConnectionString);
			myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
			mymdbm = new MarkingDBM();
			mydbm = mymdbm;
			mymdbm.Collection = new ObservableCollection<Marking>();
			mymdbm.Filter = myfilter;
			mymdbm.FillAsyncCompleted = () =>
			{
				if (mymdbm.Errors.Count > 0)
					OpenPopup(mymdbm.ErrorMessage, true);
			};
			mymdbm.FillAsync();
			mysync = new MarkingSynchronizer();
			mysync.DomainCollection = mymdbm.Collection;
			base.Collection = mysync.ViewModelCollection;

			base.DeleteQuestionHeader = "Удалить строки?";
		}

		~MarkingViewCommader()
		{ Dispose(); }

		private MarkingDBM mymdbm;
		private MarkingSynchronizer mysync;

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

		protected override void OtherViewRefresh()
		{
		}
		protected override void RefreshData(object parametr)
		{
			UpdateFilter();
		}
		protected override void SettingView()
		{
		}

		public void Dispose()
		{
			myfilter.RemoveFilter();
		}
	}
}
