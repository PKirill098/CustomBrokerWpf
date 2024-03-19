using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using Excel = Microsoft.Office.Interop.Excel;
namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Marking
{
	public class Marking : lib.DomainBaseStamp
	{
		public Marking(int id, long stamp, DateTime? updated, string updater, lib.DomainObjectState state
			, string brand, string color, string country, string ean13, string filename, long gtin, string inn, string materialdown, string materialin, string materialup
			, string productname, string producttype, DateTime published, string size, string tnved, string vendorcode
			) : base(id, stamp, updated, updater, state, true)
		{
			mybrand = brand;
			mycolor = color;
			mycountry=country;
			myean13 = ean13;
			myfilename = filename;
			mygtin = gtin;
			myinn=inn;
			mymaterialdown = materialdown;
			mymaterialin = materialin;
			mymaterialup = materialup;
			myproductname = productname;
			myproducttype = producttype;
			mypublished = published;
			mysize = size;
			mytnved = tnved;
			myvendorcode = vendorcode;
		}
		public Marking() : this(lib.NewObjectId.NewId,0L,null,null,lib.DomainObjectState.Added
			, null,null,null,null,null,0L,null,null,null,null,null,null,DateTime.Today,null,null,null) { }

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
		public string MaterialIn
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
					{
						errmsg = "Не указана торговая марка!";
						isvalid = false;
					}
					break;
				case nameof(this.Ean13):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
					{ errmsg = "Не указан EAN-13!"; isvalid = false; }
					break;
				case nameof(this.FileName):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
					{
						errmsg = "Не указано имя файла(поставка)!";
						isvalid = false;
					}
					break;
				case nameof(this.Gtin):
					long? lng = (long?)value;
					if (!lng.HasValue || lng.Value==0L)
					{
						errmsg = "Не указан GTIN!";
						isvalid = false;
					}
					break;
				case nameof(this.Inn):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
					{
						errmsg = "Не указан ИНН!";
						isvalid = false;
					}
					break;
				case nameof(this.MaterialUp):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
					{
						errmsg = "Не указан состав сырья(материал верха)!";
						isvalid = false;
					}
					break;
				case nameof(this.ProductName):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
					{
						errmsg = "Не указано наименование товара!";
						isvalid = false;
					}
					break;
				case nameof(this.ProductType):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
					{
						errmsg = "Не указан вид изделия!";
						isvalid = false;
					}
					break;
				case nameof(this.Published):
					DateTime? date = (DateTime?)value;
					if (!date.HasValue)
					{
						errmsg = "Не указана дата публикации!";
						isvalid = false;
					}
					break;
				case nameof(this.Tnved):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
					{
						errmsg = "Не указан ТНВЭД!";
						isvalid = false;
					}
					break;
				case nameof(this.VendorCode):
					str = (string)value;
					if (string.IsNullOrEmpty(str))
					{
						errmsg = "Не указан артикул (модель производителя)!";
						isvalid = false;
					}
					break;
			}
			return isvalid;
		}
		protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
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
			this.MaterialIn=temp.MaterialIn;
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
				case nameof(this.MaterialIn):
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

	public class MarkingDBM : lib.DBManagerStamp<Marking,Marking>
	{
		public MarkingDBM()
		{
			this.ConnectionString = CustomBrokerWpf.References.ConnectionString;
			base.NeedAddConnection = false;

			SelectCommandText = "mark.Marking_sp";
			InsertCommandText = "mark.MarkingAdd_sp";
			UpdateCommandText = "mark.MarkingUpd_sp";
			DeleteCommandText = "mark.MarkingDel_sp";

			SelectParams = new SqlParameter[]
			{
				new SqlParameter("@id", System.Data.SqlDbType.Int),
				new SqlParameter("@filterid", System.Data.SqlDbType.Int){ Value = 0},
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
				,new SqlParameter("@tnved", System.Data.SqlDbType.NVarChar,1024)
				,new SqlParameter("@vendorcode", System.Data.SqlDbType.NVarChar,50)
			};
		}

		private lib.SQLFilter.SQLFilter myfilter;
		public lib.SQLFilter.SQLFilter Filter
		{ set { myfilter = value; } get { return myfilter; } }

		protected override Marking CreateRecord(SqlDataReader reader)
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
		protected override Marking CreateModel(Marking reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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

		protected override void SetSelectParametersValue()
		{
			foreach (SqlParameter par in this.SelectParams)
				switch (par.ParameterName)
				{
					case "@filterid":
						par.Value = myfilter?.FilterWhereId;
						break;
				}
		}

		protected override bool SetParametersValue(Marking item)
		{
            base.SetParametersValue(item);
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
						par.Value = item.HasPropertyOutdatedValue(nameof(Marking.MaterialIn));
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
						par.Value = item.MaterialIn;
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
			InitProperties();
		}
		public MarkingVM():this(new Marking()) { }

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
		public string MaterialIn
		{
			set { SetProperty(this.DomainObject.MaterialIn, (string v) => { this.DomainObject.MaterialIn = v; }, value); }
			get { return GetProperty(this.DomainObject.MaterialIn, null); }
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
			return mybrand != this.DomainObject.Brand || mygtin != this.DomainObject.Gtin;
		}
		protected override void DomainObjectPropertyChanged(string property)
		{
			switch (property)
			{
				case nameof(Marking.Brand):
					mybrand = this.DomainObject.Brand;
					break;
				case nameof(Marking.Gtin):
					mygtin = this.DomainObject.Gtin;
					break;
			}
		}
		protected override void InitProperties()
		{
			mybrand = this.DomainObject.Brand;
			mygtin = this.DomainObject.Gtin;
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
				case nameof(this.MaterialIn):
					this.DomainObject.MaterialIn = (string)value;
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

			#region Filter
			myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
			myfilterdefault = new RelayCommand(FilterDefaultExec, FilterDefaultCanExec);
			myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
			myfiltersave = new RelayCommand(FilterSaveExec, FilterSaveCanExec);

			mybrandfilter = new MarkingBrandCheckListBoxVMFill();
			mybrandfilter.DeferredFill = true;
			mybrandfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mybrandfilter.ExecCommand2 = () => { mybrandfilter.Clear(); };
			mybrandfilter.ItemsSource = myview.OfType<MarkingVM>();
			mycolorfilter = new MarkingColorCheckListBoxVMFill();
			mycolorfilter.DeferredFill = true;
			mycolorfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mycolorfilter.ExecCommand2 = () => { mycolorfilter.Clear(); };
			mycolorfilter.ItemsSource = myview.OfType<MarkingVM>();
			mycountryfilter = new MarkingCountryCheckListBoxVMFill();
			mycountryfilter.DeferredFill = true;
			mycountryfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mycountryfilter.ExecCommand2 = () => { mycountryfilter.Clear(); };
			mycountryfilter.ItemsSource = myview.OfType<MarkingVM>();
			myean13filter = new MarkingEan13CheckListBoxVMFill();
			myean13filter.DeferredFill = true;
			myean13filter.ExecCommand1 = () => { FilterRunExec(null); };
			myean13filter.ExecCommand2 = () => { myean13filter.Clear(); };
			myean13filter.ItemsSource = myview.OfType<MarkingVM>();
			myfilenamefilter = new MarkingFileNameCheckListBoxVMFill();
			myfilenamefilter.DeferredFill = true;
			myfilenamefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myfilenamefilter.ExecCommand2 = () => { myfilenamefilter.Clear(); };
			myfilenamefilter.ItemsSource = myview.OfType<MarkingVM>();
			mygtinfilter = new MarkingGtinCheckListBoxVMFill();
			mygtinfilter.DeferredFill = true;
			mygtinfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mygtinfilter.ExecCommand2 = () => { mygtinfilter.Clear(); };
			mygtinfilter.ItemsSource = myview.OfType<MarkingVM>();
			mygtinfilter.GetDisplayPropertyValueFunc = (object item) => { return item.ToString(); };
			myinnfilter = new MarkingInnCheckListBoxVMFill();
			myinnfilter.DeferredFill = true;
			myinnfilter.ExecCommand1 = () => { FilterRunExec(null); };
			myinnfilter.ExecCommand2 = () => { myinnfilter.Clear(); };
			myinnfilter.ItemsSource = myview.OfType<MarkingVM>();
			mymaterialdownfilter = new MarkingMaterialDownCheckListBoxVMFill();
			mymaterialdownfilter.DeferredFill = true;
			mymaterialdownfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mymaterialdownfilter.ExecCommand2 = () => { mymaterialdownfilter.Clear(); };
			mymaterialdownfilter.ItemsSource = myview.OfType<MarkingVM>();
			mymaterialinfilter = new MarkingMaterialInCheckListBoxVMFill();
			mymaterialinfilter.DeferredFill = true;
			mymaterialinfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mymaterialinfilter.ExecCommand2 = () => { mymaterialinfilter.Clear(); };
			mymaterialinfilter.ItemsSource = myview.OfType<MarkingVM>();
			mymaterialupfilter = new MarkingMaterialUpCheckListBoxVMFill();
			mymaterialupfilter.DeferredFill = true;
			mymaterialupfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mymaterialupfilter.ExecCommand2 = () => { mymaterialupfilter.Clear(); };
			mymaterialupfilter.ItemsSource = myview.OfType<MarkingVM>();
			myproductnamefilter = new MarkingProductNameCheckListBoxVMFill();
			myproductnamefilter.DeferredFill = true;
			myproductnamefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myproductnamefilter.ExecCommand2 = () => { myproductnamefilter.Clear(); };
			myproductnamefilter.ItemsSource = myview.OfType<MarkingVM>();
			myproducttypefilter = new MarkingProductTypeCheckListBoxVMFill();
			myproducttypefilter.DeferredFill = true;
			myproducttypefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myproducttypefilter.ExecCommand2 = () => { myproducttypefilter.Clear(); };
			myproducttypefilter.ItemsSource = myview.OfType<MarkingVM>();
			mysizefilter = new MarkingSizeCheckListBoxVMFill();
			mysizefilter.DeferredFill = true;
			mysizefilter.ExecCommand1 = () => { FilterRunExec(null); };
			mysizefilter.ExecCommand2 = () => { mysizefilter.Clear(); };
			mysizefilter.ItemsSource = myview.OfType<MarkingVM>();
			mytnvedfilter = new MarkingTnvedCheckListBoxVMFill();
			mytnvedfilter.DeferredFill = true;
			mytnvedfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mytnvedfilter.ExecCommand2 = () => { mytnvedfilter.Clear(); };
			mytnvedfilter.ItemsSource = myview.OfType<MarkingVM>();
			myvendorcodefilter = new MarkingVendorCodeCheckListBoxVMFill();
			myvendorcodefilter.DeferredFill = true;
			myvendorcodefilter.ExecCommand1 = () => { FilterRunExec(null); };
			myvendorcodefilter.ExecCommand2 = () => { myvendorcodefilter.Clear(); };
			myvendorcodefilter.ItemsSource = myview.OfType<MarkingVM>();

			mypublishedfilter = new libui.DateFilterVM();
			mypublishedfilter.ExecCommand1 = () => { FilterRunExec(null); };
			mypublishedfilter.ExecCommand2 = () => { mypublishedfilter.Clear(); };

			this.FilterFill();

			if (myfilter.isEmpty)
				this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
			#endregion

			mymarkingadd = new RelayCommand(MarkingAddExec, MarkingAddCanExec);
		}

		~MarkingViewCommader()
		{ Dispose(); }

		private MarkingDBM mymdbm;
		private MarkingSynchronizer mysync;

		private RelayCommand mymarkingadd;
		public ICommand ExcelImport
		{
			get { return mymarkingadd; }
		}
		private void MarkingAddExec(object parametr)
		{
			if (myexceltask == null)
				myexceltask = new lib.TaskAsync.TaskAsync();
			if (!myexceltask.IsBusy)
			{
				Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
				fd.Multiselect = false;
				fd.CheckPathExists = true;
				fd.CheckFileExists = true;
				if (System.IO.Directory.Exists(CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault)) fd.InitialDirectory = CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault;
				fd.Title = "Выбор файла маркировки";
				fd.Filter = "Файлы Excel|*.xls;*.xlsx;*.xlsm;";
				if (fd.ShowDialog().Value)
				{
					try
					{
						int producttype=0;
						if (fd.FileName.ToLower().IndexOf("обувь") > 0)
							producttype = 1;
						else if (fd.FileName.ToLower().IndexOf("одежда") > 0)
							producttype = 2;
						else
						{
							this.OpenPopup("В имени файла не указан тип маркированого товара!", true);
							return;
						}
						myexceltask.DoProcessing = OnExcelImport;
						myexceltask.Run(new object[2] { fd.FileName, producttype });
					}
					catch (Exception ex)
					{
						this.OpenPopup("Не удалось загрузить файл.\n" + ex.Message, true);
					}
				}
			}
			else
			{
				System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
			}
		}
		private bool MarkingAddCanExec(object parametr)
		{ return myexceltask == null || !myexceltask.IsBusy; }
		private lib.TaskAsync.TaskAsync myexceltask;
		private KeyValuePair<bool, string> OnExcelImport(object parm)
		{
			object[] param = parm as object[];
			string filepath = (string)param[0];
			int filetype = (int)param[1];
			return new KeyValuePair<bool, string>(false, this.ImportMarking(filepath, filetype, myexceltask).ToString() + " строк обработано.");
		}
		internal int ImportMarking(string filepath, int filetype, lib.TaskAsync.TaskAsync myexceltask)
		{
			int maxc, maxr, usedr = 0, c, r = 3;
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
				maxc = 50; //exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Column;
				myexceltask.ProgressChange(5);

				int cean13=0;
				long gtin;
				string ean13, inn;
				DateTime date;
				Marking marking;
				System.Text.StringBuilder str = new System.Text.StringBuilder();
				string[] dateformats = new string[] { "dd.MM.yyyy", "dd.MM.yy", "dd-MM-yyyy", "dd-MM-yy" };
				c = 1; r = 3;
				for (; c <= maxc; c++)
				{
					ean13 = (exWh.Cells[r, c].Text as string).Trim().ToLower();
					switch ((exWh.Cells[r, c].Text as string).Trim().ToLower())
					{
						case "ean-13":
							cean13 = c;
							break;
					}
				}
				if(cean13==0)
					throw new Exception("Столбец EAN-13 не найден!");
				r = 7;
				for (; r <= maxr; r++)
				{
					if (string.IsNullOrEmpty(exWh.Cells[r, 2].Text as string)) continue;

					// c = filetype == 1 ? 19 : 30; 1 - shoes 2 - clothes
					if (string.IsNullOrEmpty(exWh.Cells[r, cean13].Text as string))
						throw new Exception("Отсутствует EAN-13");
					else
						ean13 = exWh.Cells[r, cean13].Text;
					c = filetype == 1 ? 8 : 8;
					if (string.IsNullOrEmpty(exWh.Cells[r, c].Text as string))
						throw new Exception("Отсутствует ИНН");
					else
						inn = exWh.Cells[r, c].Text;
					marking = mysync.DomainCollection.FirstOrDefault((Marking item) => { return item.Ean13 == ean13 && item.Inn == inn; });
					if (marking == null)
					{
						marking = new Marking();
						marking.Ean13 = ean13;
						marking.Inn = inn;
					}
					if ((exWh.Cells[r, 2].Text as string).Length > 20)
						throw new Exception("Некорректное значение GTIN: " + exWh.Cells[r, 2].Text);
					else
					{
						if(long.TryParse(exWh.Cells[r, 2].Text as String,out gtin))
							marking.Gtin = gtin;
						else
							throw new Exception("Не удалось преобразовать GTIN "+ exWh.Cells[r, 2].Text as String + " в число!");
					}
					c = filetype == 1 ? 4 : 3;
					if (string.IsNullOrEmpty(exWh.Cells[r, c].Text as string))
						throw new Exception("Отсутствует Модель производителя");
					else
						marking.VendorCode = exWh.Cells[r, c].Text;
					c = filetype == 1 ? 6 : 5;
					if (string.IsNullOrEmpty(exWh.Cells[r, c].Text as string))
						throw new Exception("Отсутствует Наименование товара");
					else
						marking.ProductName = exWh.Cells[r, c].Text;
					c = filetype == 1 ? 7 : 6;
					if (string.IsNullOrEmpty(exWh.Cells[r, c].Text as string))
						throw new Exception("Отсутствует Бренд (торговая марка)");
					else
						marking.Brand = exWh.Cells[r, c].Text;
					c = filetype == 1 ? 9 : 7;
					marking.Country = exWh.Cells[r, c].Text;
					c = filetype == 1 ? 10 : 10;
					if (string.IsNullOrEmpty(exWh.Cells[r, c].Text as string))
						throw new Exception("Отсутствует Вид изделия");
					else
						marking.ProductType = exWh.Cells[r, c].Text;
					c = filetype == 1 ? 11 : 19;
					if (string.IsNullOrEmpty(exWh.Cells[r, c].Text as string))
						throw new Exception("Отсутствует " + (filetype == 1 ? "Материал верха" : "(Состав сырья)"));
					else
						marking.MaterialUp = exWh.Cells[r, c].Text;
					if(filetype == 1) marking.MaterialIn = exWh.Cells[r, 12].Text;
					if (filetype == 1) marking.MaterialDown = exWh.Cells[r, 13].Text;
					c = filetype == 1 ? 14 : 16;
					marking.Color = exWh.Cells[r, c].Text;
					c = filetype == 1 ? 15 : 14;
					marking.Size = exWh.Cells[r, c].Text;
					c = filetype == 1 ? 17 : 12;
					if (string.IsNullOrEmpty(exWh.Cells[r, c].Text as string))
						throw new Exception("Отсутствует ТНВЭД");
					else
						marking.Tnved = exWh.Cells[r, c].Text;
					c = filetype == 1 ? 5 : 4;
					str.Clear();
					str.Append((exWh.Cells[r, c].Text as string).Trim());
					if (DateTime.TryParseExact(str.ToString(), dateformats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date))
						marking.Published = date;
					else
						throw new Exception("Отсутствует Дата публикации");
					marking.FileName = System.IO.Path.GetFileName(filepath);

					App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<Marking>(mysync.DomainCollection.Add), marking);
					usedr++;
					myexceltask.ProgressChange(r, maxr, 0.85M, 0.15M);
				}
				myexceltask.ProgressChange(99);
				exWb.Close();
				exApp.Quit();

				myexceltask.ProgressChange(100);
				return usedr;
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

		#region Filter
		private lib.SQLFilter.SQLFilter myfilter;
		public lib.SQLFilter.SQLFilter Filter
		{
			get { return myfilter; }
		}

		private MarkingBrandCheckListBoxVMFill mybrandfilter;
		public MarkingBrandCheckListBoxVMFill BrandFilter
		{ get { return mybrandfilter; } }
		private MarkingColorCheckListBoxVMFill mycolorfilter;
		public MarkingColorCheckListBoxVMFill ColorFilter
		{
			get { return mycolorfilter; }
		}
		private MarkingCountryCheckListBoxVMFill mycountryfilter;
		public MarkingCountryCheckListBoxVMFill CountryFilter
		{
			get { return mycountryfilter; }
		}
		private MarkingEan13CheckListBoxVMFill myean13filter;
		public MarkingEan13CheckListBoxVMFill Ean13Filter
		{ get { return myean13filter; } }
		private MarkingFileNameCheckListBoxVMFill myfilenamefilter;
		public MarkingFileNameCheckListBoxVMFill FileNameFilter
		{
			get { return myfilenamefilter; }
		}
		private MarkingGtinCheckListBoxVMFill mygtinfilter;
		public MarkingGtinCheckListBoxVMFill GtinFilter
		{
			get { return mygtinfilter; }
		}
		private MarkingInnCheckListBoxVMFill myinnfilter;
		public MarkingInnCheckListBoxVMFill InnFilter
		{ get { return myinnfilter; } }
		private MarkingMaterialDownCheckListBoxVMFill mymaterialdownfilter;
		public MarkingMaterialDownCheckListBoxVMFill MaterialDownFilter
		{ get { return mymaterialdownfilter; } }
		private MarkingMaterialInCheckListBoxVMFill mymaterialinfilter;
		public MarkingMaterialInCheckListBoxVMFill MaterialInFilter
		{ get { return mymaterialinfilter; } }
		private MarkingMaterialUpCheckListBoxVMFill mymaterialupfilter;
		public MarkingMaterialUpCheckListBoxVMFill MaterialUpFilter
		{ get { return mymaterialupfilter; } }
		private MarkingProductNameCheckListBoxVMFill myproductnamefilter;
		public MarkingProductNameCheckListBoxVMFill ProductNameFilter
		{ get { return myproductnamefilter; } }
		private MarkingProductTypeCheckListBoxVMFill myproducttypefilter;
		public MarkingProductTypeCheckListBoxVMFill ProductTypeFilter
		{ get { return myproducttypefilter; } }
		private libui.DateFilterVM mypublishedfilter;
		public libui.DateFilterVM PublishedFilter
		{ get { return mypublishedfilter; } }
		private MarkingSizeCheckListBoxVMFill mysizefilter;
		public MarkingSizeCheckListBoxVMFill SizeFilter
		{ get { return mysizefilter; } }
		private MarkingTnvedCheckListBoxVMFill mytnvedfilter;
		public MarkingTnvedCheckListBoxVMFill TnvedFilter
		{ get { return mytnvedfilter; } }
		private MarkingVendorCodeCheckListBoxVMFill myvendorcodefilter;
		public MarkingVendorCodeCheckListBoxVMFill VendorCodeFilter
		{ get { return myvendorcodefilter; } }

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
			mybrandfilter.Clear();
			mybrandfilter.IconVisibileChangedNotification();
			mycolorfilter.Clear();
			mycolorfilter.IconVisibileChangedNotification();
			mycountryfilter.Clear();
			mycountryfilter.IconVisibileChangedNotification();
			myean13filter.Clear();
			myean13filter.IconVisibileChangedNotification();
			myfilenamefilter.Clear();
			myfilenamefilter.IconVisibileChangedNotification();
			mygtinfilter.Clear();
			mygtinfilter.IconVisibileChangedNotification();
			myinnfilter.Clear();
			myinnfilter.IconVisibileChangedNotification();
			mypublishedfilter.Clear();
			mypublishedfilter.IconVisibileChangedNotification();
			mymaterialdownfilter.Clear();
			mymaterialdownfilter.IconVisibileChangedNotification();
			mymaterialinfilter.Clear();
			mymaterialinfilter.IconVisibileChangedNotification();
			mymaterialupfilter.Clear();
			mymaterialupfilter.IconVisibileChangedNotification();
			myproductnamefilter.Clear();
			myproductnamefilter.IconVisibileChangedNotification();
			myproducttypefilter.Clear();
			myproducttypefilter.IconVisibileChangedNotification();
			mysizefilter.Clear();
			mysizefilter.IconVisibileChangedNotification();
			mytnvedfilter.Clear();
			mytnvedfilter.IconVisibileChangedNotification();
			myvendorcodefilter.Clear();
			myvendorcodefilter.IconVisibileChangedNotification();
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
					mybrandfilter.FilterOn
					|| mycolorfilter.FilterOn
					|| mycountryfilter.FilterOn
					|| myean13filter.FilterOn
					|| myfilenamefilter.FilterOn
					|| mygtinfilter.FilterOn
					|| myinnfilter.FilterOn
					|| mymaterialdownfilter.FilterOn
					|| mymaterialinfilter.FilterOn
					|| mymaterialupfilter.FilterOn
					|| myproductnamefilter.FilterOn
					|| myproducttypefilter.FilterOn
					|| mypublishedfilter.FilterOn
					|| mysizefilter.FilterOn
					|| mytnvedfilter.FilterOn
					|| myvendorcodefilter.FilterOn
				);
			}
		}

		private void UpdateFilter()
		{
			if (mybrandfilter.FilterOn)
			{
				if (mybrandfilter.SelectedItems.Count > 0)
				{
					string[] items = new string[mybrandfilter.SelectedItems.Count];
					for (int i = 0; i < mybrandfilter.SelectedItems.Count; i++)
						items[i] = (string)mybrandfilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "brand", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "brand", mybrandfilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "brand", new string[0]);
			if (mycolorfilter.FilterOn)
			{
				if (mycolorfilter.SelectedItems.Count > 0)
				{
					string[] items = new string[mycolorfilter.SelectedItems.Count];
					for (int i = 0; i < mycolorfilter.SelectedItems.Count; i++)
						items[i] = (string)mycolorfilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "color", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "color", mycolorfilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "color", new string[0]);
			if (mycountryfilter.FilterOn)
			{
				if (mycountryfilter.SelectedItems.Count > 0)
				{
					string[] items = new string[mycountryfilter.SelectedItems.Count];
					for (int i = 0; i < mycountryfilter.SelectedItems.Count; i++)
						items[i] = (string)mycountryfilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "country", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "country", mycountryfilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "country", new string[0]);
			if (myean13filter.FilterOn)
			{
				if (myean13filter.SelectedItems.Count > 0)
				{
					string[] items = new string[myean13filter.SelectedItems.Count];
					for (int i = 0; i < myean13filter.SelectedItems.Count; i++)
						items[i] = (string)myean13filter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "ean13", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "ean13", myean13filter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "ean13", new string[0]);
			if (myfilenamefilter.FilterOn)
			{
				if (myfilenamefilter.SelectedItems.Count > 0)
				{
					string[] items = new string[myfilenamefilter.SelectedItems.Count];
					for (int i = 0; i < myfilenamefilter.SelectedItems.Count; i++)
						items[i] = (string)myfilenamefilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "filename", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "filename", myfilenamefilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "filename", new string[0]);
			if (mygtinfilter.FilterOn)
			{
				if (mygtinfilter.SelectedItems.Count > 0)
				{
					string[] items = new string[mygtinfilter.SelectedItems.Count];
					for (int i = 0; i < mygtinfilter.SelectedItems.Count; i++)
						items[i] = ((long)mygtinfilter.SelectedItems[i]).ToString();
					myfilter.SetList(myfilter.FilterWhereId, "gtin", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "gtin", mygtinfilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "gtin", new string[0]);
			if (myinnfilter.FilterOn)
			{
				if (myinnfilter.SelectedItems.Count > 0)
				{
					string[] items = new string[myinnfilter.SelectedItems.Count];
					for (int i = 0; i < myinnfilter.SelectedItems.Count; i++)
						items[i] = (string)myinnfilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "inn", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "inn", myinnfilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "inn", new string[0]);
			if (mymaterialdownfilter.FilterOn)
			{
				if (mymaterialdownfilter.SelectedItems.Count > 0)
				{
					string[] items = new string[mymaterialdownfilter.SelectedItems.Count];
					for (int i = 0; i < mymaterialdownfilter.SelectedItems.Count; i++)
						items[i] = (string)mymaterialdownfilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "materialdown", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "materialdown", mymaterialdownfilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "materialdown", new string[0]);
			if (mymaterialinfilter.FilterOn)
			{
				if (mymaterialinfilter.SelectedItems.Count > 0)
				{
					string[] items = new string[mymaterialinfilter.SelectedItems.Count];
					for (int i = 0; i < mymaterialinfilter.SelectedItems.Count; i++)
						items[i] = (string)mymaterialinfilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "materialin", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "materialin", mymaterialinfilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "materialin", new string[0]);
			if (mymaterialupfilter.FilterOn)
			{
				if (mymaterialupfilter.SelectedItems.Count > 0)
				{
					string[] items = new string[mymaterialupfilter.SelectedItems.Count];
					for (int i = 0; i < mymaterialupfilter.SelectedItems.Count; i++)
						items[i] = (string)mymaterialupfilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "materialup", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "materialup", mymaterialupfilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "materialup", new string[0]);
			if (myproductnamefilter.FilterOn)
			{
				if (myproductnamefilter.SelectedItems.Count > 0)
				{
					string[] items = new string[myproductnamefilter.SelectedItems.Count];
					for (int i = 0; i < myproductnamefilter.SelectedItems.Count; i++)
						items[i] = (string)myproductnamefilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "productname", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "productname", myproductnamefilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "productname", new string[0]);
			if (myproducttypefilter.FilterOn)
			{
				if (myproducttypefilter.SelectedItems.Count > 0)
				{
					string[] items = new string[myproducttypefilter.SelectedItems.Count];
					for (int i = 0; i < myproducttypefilter.SelectedItems.Count; i++)
						items[i] = (string)myproducttypefilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "producttype", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "producttype", myproducttypefilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "producttype", new string[0]);
			if (mysizefilter.FilterOn)
			{
				if (mysizefilter.SelectedItems.Count > 0)
				{
					string[] items = new string[mysizefilter.SelectedItems.Count];
					for (int i = 0; i < mysizefilter.SelectedItems.Count; i++)
						items[i] = (string)mysizefilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "size", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "size", mysizefilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "size", new string[0]);
			if (mytnvedfilter.FilterOn)
			{
				if (mytnvedfilter.SelectedItems.Count > 0)
				{
					string[] items = new string[mytnvedfilter.SelectedItems.Count];
					for (int i = 0; i < mytnvedfilter.SelectedItems.Count; i++)
						items[i] = (string)mytnvedfilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "tnved", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "tnved", mytnvedfilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "tnved", new string[0]);
			if (myvendorcodefilter.FilterOn)
			{
				if (myvendorcodefilter.SelectedItems.Count > 0)
				{
					string[] items = new string[myvendorcodefilter.SelectedItems.Count];
					for (int i = 0; i < myvendorcodefilter.SelectedItems.Count; i++)
						items[i] = (string)myvendorcodefilter.SelectedItems[i];
					myfilter.SetList(myfilter.FilterWhereId, "vendorcode", items);
				}
				else
					myfilter.SetString(myfilter.FilterWhereId, "vendorcode", myvendorcodefilter.ItemsViewFilter);
			}
			else
				myfilter.SetList(myfilter.FilterWhereId, "vendorcode", new string[0]);
			myfilter.SetDate(myfilter.FilterWhereId, "published", "published", mypublishedfilter.DateStart, mypublishedfilter.DateStop, mypublishedfilter.IsNull);
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
			//myfilter.PullListBox(myfilter.FilterWhereId, "agent", "Id", myagentfilter., true);
			bool isnull;
			DateTime? date1, date2;
			myfilter.PullDate(myfilter.FilterWhereId, "published", "published", out date1, out date2, out isnull);
			mypublishedfilter.IsNull = isnull;
			mypublishedfilter.DateStart = date1;
			mypublishedfilter.DateStop = date2;
			mypublishedfilter.IconVisibileChangedNotification();
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
			mymdbm.FillAsync();
		}
		protected override void SettingView()
		{
			myview.NewItemPlaceholderPosition = System.ComponentModel.NewItemPlaceholderPosition.AtBeginning;
		}

		public void Dispose()
		{
			myfilter.RemoveFilter();
			myfilter.Dispose();
		}
	}

	public class MarkingBrandCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (!Items.Contains(item.Brand)) Items.Add(item.Brand);
		}
	}
	public class MarkingColorCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (!Items.Contains(item.Color)) Items.Add(item.Color);
		}
	}
	public class MarkingCountryCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.Country) || Items.Contains(item.Country))) Items.Add(item.Country);
		}
	}
	public class MarkingEan13CheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (!Items.Contains(item.Ean13)) Items.Add(item.Ean13);
		}
	}
	public class MarkingFileNameCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.FileName) || Items.Contains(item.FileName))) Items.Add(item.FileName);
		}
	}
	public class MarkingGtinCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, long>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (item.Gtin.HasValue && !Items.Contains(item.Gtin.Value)) Items.Add(item.Gtin.Value);
		}
	}
	public class MarkingInnCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.Inn) || Items.Contains(item.Inn))) Items.Add(item.Inn);
		}
	}
	public class MarkingMaterialDownCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.MaterialDown) || Items.Contains(item.MaterialDown))) Items.Add(item.MaterialDown);
		}
	}
	public class MarkingMaterialInCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.MaterialIn) || Items.Contains(item.MaterialIn))) Items.Add(item.MaterialIn);
		}
	}
	public class MarkingMaterialUpCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.MaterialUp) || Items.Contains(item.MaterialUp))) Items.Add(item.MaterialUp);
		}
	}
	public class MarkingProductNameCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.ProductName) || Items.Contains(item.ProductName))) Items.Add(item.ProductName);
		}
	}
	public class MarkingProductTypeCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.ProductType) || Items.Contains(item.ProductType))) Items.Add(item.ProductType);
		}
	}
	public class MarkingSizeCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.Size) || Items.Contains(item.Size))) Items.Add(item.Size);
		}
	}
	public class MarkingTnvedCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.Tnved) || Items.Contains(item.Tnved))) Items.Add(item.Tnved);
		}
	}
	public class MarkingVendorCodeCheckListBoxVMFill : libui.CheckListBoxVMFill<MarkingVM, string>
	{
		protected override void AddItem(MarkingVM item)
		{
			if (Items.Count == 0)
				Items.Add(string.Empty);
			if (!(string.IsNullOrEmpty(item.VendorCode) || Items.Contains(item.VendorCode))) Items.Add(item.VendorCode);
		}
	}
}
