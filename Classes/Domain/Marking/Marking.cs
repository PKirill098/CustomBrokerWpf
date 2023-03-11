using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.References;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Specification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using lib=KirillPolyanskiy.DataModelClassLibrary;
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
			throw new NotImplementedException();
		}
	}
}
