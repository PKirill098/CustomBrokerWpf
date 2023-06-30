using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Goods : lib.DomainBaseStamp
    {
        public Goods() : this(lib.NewObjectId.NewId, string.Empty, null, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
            , string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
            , string.Empty, null, null, string.Empty, null, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty
            , 0, null, null, lib.DomainObjectState.Added)
        { }
        internal Goods(int id, string name, Gender gender, string material, string contexture, string contexturenote, string brand, string producer, string titlecountry
            , string cat1, string cat2, string cat3, string cat4, string cat5
            , string certificate, DateTime? certstart, DateTime? certstop, string contractnmbr, DateTime? contractdate, string vendorcode, string filepath, string colormark, string declarant, string type
            , Int64 stamp, DateTime? updated, string updater, lib.DomainObjectState domainstate) : base(id, stamp, updated, updater, domainstate)
        {
            myname = name;
            mygender = gender;
            mymaterial = material;
            mycontexture = contexture;
            mycontexturenote = contexturenote;
            mybrand = brand;
            myproducer = producer;
            mytitlecountry = titlecountry;
            mycat1 = cat1;
            mycat2 = cat2;
            mycat3 = cat3;
            mycat4 = cat4;
            mycat5 = cat5;
            mycertificate = certificate;
            mycertstart = certstart;
            mycertstop = certstop;
            mycontractnmbr = contractnmbr;
            mycontractdate = contractdate;
            myvendorcode = vendorcode;
            myfilepath = filepath;
            mycolormark = colormark;
            mydeclarant = declarant;
            mytype = type;
        }

        private string myname;
        public string Name
        {
            set
            {
                if (!string.Equals(myname, value))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myname);
                    myname = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myname; }
        }
        private Gender mygender;
        public Gender Gender
        {
            set
            {
                if (!object.Equals(mygender, value))
                {
                    string name = "Gender";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mygender);
                    mygender = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mygender; }
        }
        private string mymaterial;
        public string Material
        {
            set
            {
                if (!string.Equals(mymaterial, value))
                {
                    string name = "Material";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mymaterial);
                    mymaterial = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mymaterial; }
        }
        private string mycontexture;
        public string Contexture
        {
            set
            {
                if (!string.Equals(mycontexture, value))
                {
                    string name = "Contexture";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycontexture);
                    mycontexture = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycontexture; }
        }
        private string mycontexturenote;
        public string ContextureNote
        {
            set
            {
                if (!string.Equals(mycontexturenote, value))
                {
                    string name = "ContextureNote";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycontexturenote);
                    mycontexturenote = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycontexturenote; }
        }
        private string mybrand;
        public string Brand
        {
            set
            {
                if (!string.Equals(mybrand, value))
                {
                    string name = "Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mybrand);
                    mybrand = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mybrand; }
        }
        private string myproducer;
        public string Producer
        {
            set
            {
                if (!string.Equals(myproducer, value))
                {
                    string name = "Producer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myproducer);
                    myproducer = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myproducer; }
        }
        private string mytitlecountry;
        public string TitleCountry
        {
            set
            {
                if (!string.Equals(mytitlecountry, value))
                {
                    string name = "TitleCountry";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mytitlecountry);
                    mytitlecountry = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mytitlecountry; }
        }
        private string mycat1;
        public string Cat1
        {
            set
            {
                if (!string.Equals(mycat1, value))
                {
                    string name = "Cat1";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat1);
                    mycat1 = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat1; }
        }
        private string mycat2;
        public string Cat2
        {
            set
            {
                if (!string.Equals(mycat2, value))
                {
                    string name = "Cat2";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat2);
                    mycat2 = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat2; }
        }
        private string mycat3;
        public string Cat3
        {
            set
            {
                if (!string.Equals(mycat3, value))
                {
                    string name = "Cat3";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat3);
                    mycat3 = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat3; }
        }
        private string mycat4;
        public string Cat4
        {
            set
            {
                if (!string.Equals(mycat4, value))
                {
                    string name = "Cat4";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat4);
                    mycat4 = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat4; }
        }
        private string mycat5;
        public string Cat5
        {
            set
            {
                if (!string.Equals(mycat5, value))
                {
                    string name = "Cat5";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat5);
                    mycat5 = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat5; }
        }
        private string mycertificate;
        public string Certificate
        {
            set
            {
                if (!string.Equals(mycertificate, value))
                {
                    string name = "Certificate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycertificate);
                    mycertificate = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycertificate; }
        }
        private DateTime? mycertstart;
        public DateTime? CertStart
        {
            set
            {
                if (mycertstart.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(mycertstart.Value, value.Value)))
                {
                    string name = "CertStart";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycertstart);
                    mycertstart = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycertstart; }
        }
        private DateTime? mycertstop;
        public DateTime? CertStop
        {
            set
            {
                if (mycertstop.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(mycertstop.Value, value.Value)))
                {
                    string name = "CertStop";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycertstop);
                    mycertstop = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycertstop; }
        }
        private string mycontractnmbr;
        public string ContractNmbr
        {
            set
            {
                if (!string.Equals(mycontractnmbr, value))
                {
                    string name = "ContractNmbr";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycontractnmbr);
                    mycontractnmbr = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycontractnmbr; }
        }
        private DateTime? mycontractdate;
        public DateTime? ContractDate
        {
            set
            {
                if (mycontractdate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(mycontractdate.Value, value.Value)))
                {
                    string name = "ContractDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycontractdate);
                    mycontractdate = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycontractdate; }
        }
        private string myvendorcode;
        public string VendorCode
        {
            set
            {
                if (!string.Equals(myvendorcode, value))
                {
                    string name = "VendorCode";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myvendorcode);
                    myvendorcode = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myvendorcode; }
        }
        private string myfilepath;
        public string FilePath
        {
            set { SetProperty<string>(ref myfilepath, value); }
            get { return myfilepath; }
        }
        private string mycolormark;
        public string ColorMark
        {
            set
            {
                if (!string.Equals(mycolormark, value))
                {
                    string name = "ColorMark";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycolormark);
                    mycolormark = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycolormark; }
        }
        private string mydeclarant;
        public string Declarant
        {
            set
            {
                if (!string.Equals(mydeclarant, value))
                {
                    string name = "Declarant";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mydeclarant);
                    mydeclarant = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mydeclarant; }
        }
        private string mytype;
        public string CertType
        {
            set
            {
                if (!string.Equals(mytype, value))
                {
                    string name = "CertType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mytype);
                    mytype = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mytype; }
        }
        public int? DaysEnd
        { get { return mycertstop.HasValue ? (int)mycertstop.Value.Subtract(DateTime.Today).TotalDays : (int?)null; } }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Name":
                    myname = (string)value;
                    break;
                case "Gender":
                    mygender = (Gender)value;
                    break;
                case "Material":
                    mymaterial = (string)value;
                    break;
                case "Contexture":
                    mycontexture = (string)value;
                    break;
                case "ContextureNote":
                    mycontexturenote = (string)value;
                    break;
                case "Brand":
                    mybrand = (string)value;
                    break;
                case "Producer":
                    myproducer = (string)value;
                    break;
                case "TitleCountry":
                    mytitlecountry = (string)value;
                    break;
                case "Cat1":
                    mycat1 = (string)value;
                    break;
                case "Cat2":
                    mycat2 = (string)value;
                    break;
                case "Cat3":
                    mycat3 = (string)value;
                    break;
                case "Cat4":
                    mycat4 = (string)value;
                    break;
                case "Cat5":
                    mycat5 = (string)value;
                    break;
                case "Certificate":
                    mycertificate = (string)value;
                    break;
                case "CertStart":
                    mycertstart = (DateTime?)value;
                    break;
                case "CertStop":
                    mycertstop = (DateTime?)value;
                    break;
                case "ContractNmbr":
                    mycontractnmbr = (string)value;
                    break;
                case "ContractDate":
                    mycontractdate = (DateTime?)value;
                    break;
                case "VendorCode":
                    myvendorcode = (string)value;
                    break;
                case "FilePath":
                    myfilepath = (string)value;
                    break;
                case "ColorMark":
                    mycolormark = (string)value;
                    break;
                case "Declarant":
                    mydeclarant = (string)value;
                    break;
                case "CertType":
                    mytype = (string)value;
                    break;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            Goods newitem=(Goods)sample;
            if (!this.HasPropertyOutdatedValue("Name")) this.Name = newitem.Name;
            if (!this.HasPropertyOutdatedValue("Gender")) this.Gender = newitem.Gender;
            if (!this.HasPropertyOutdatedValue("Material")) this.Material = newitem.Material;
            if (!this.HasPropertyOutdatedValue("Contexture")) this.Contexture = newitem.Contexture;
            if (!this.HasPropertyOutdatedValue("ContextureNote")) this.ContextureNote = newitem.ContextureNote;
            if (!this.HasPropertyOutdatedValue("Brand")) this.Brand = newitem.Brand;
            if (!this.HasPropertyOutdatedValue("Producer")) this.Producer = newitem.Producer;
            if (!this.HasPropertyOutdatedValue("TitleCountry")) this.TitleCountry = newitem.TitleCountry;
            if (!this.HasPropertyOutdatedValue("Cat1")) this.Cat1 = newitem.Cat1;
            this.Cat2 = newitem.Cat2;
            if (!this.HasPropertyOutdatedValue("Certificate")) this.Certificate = newitem.Certificate;
            if (!this.HasPropertyOutdatedValue("CertStop")) this.CertStop = newitem.CertStop;
            this.ContractNmbr = newitem.ContractNmbr;
            this.ContractDate = newitem.ContractDate;
            if (!this.HasPropertyOutdatedValue("VendorCode")) this.VendorCode = newitem.VendorCode;
            if (!this.HasPropertyOutdatedValue("FilePath")) this.FilePath = newitem.FilePath;
            if (!this.HasPropertyOutdatedValue("ColorMark")) this.ColorMark = newitem.ColorMark;
            if (!this.HasPropertyOutdatedValue("Declarant")) this.Declarant = newitem.Declarant;
            if (!this.HasPropertyOutdatedValue("CertType")) this.CertType = newitem.CertType;
        }
    }

    public class GoodsDBM : lib.DBManagerId<Goods,Goods>
    {
        public GoodsDBM():base()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectCommandText = "spec.Goods_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int), new SqlParameter("@ending", System.Data.SqlDbType.Bit) };

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);
            SqlParameter parstamp = new SqlParameter("@stamp", System.Data.SqlDbType.BigInt); parstamp.Direction = System.Data.ParameterDirection.InputOutput;
            SqlParameter parupdated = new SqlParameter("@updated", System.Data.SqlDbType.DateTime2); parupdated.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parupdater = new SqlParameter("@updater", System.Data.SqlDbType.NVarChar, 20); parupdater.Direction = System.Data.ParameterDirection.Output;

            myinsertparams = new SqlParameter[] { paridout };
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,1000),
                new SqlParameter("@gender", System.Data.SqlDbType.Int),
                new SqlParameter("@material", System.Data.SqlDbType.NVarChar,100),
                new SqlParameter("@contexture", System.Data.SqlDbType.NVarChar,1000),
                new SqlParameter("@contexturenote", System.Data.SqlDbType.NVarChar,300),
                new SqlParameter("@brand", System.Data.SqlDbType.NVarChar),
                new SqlParameter("@producer", System.Data.SqlDbType.NVarChar,100),
                new SqlParameter("@titlecountry", System.Data.SqlDbType.NVarChar,100),
                new SqlParameter("@cat1", System.Data.SqlDbType.NVarChar,500),
                new SqlParameter("@cat2", System.Data.SqlDbType.NVarChar,500),
                new SqlParameter("@cat3", System.Data.SqlDbType.NVarChar,500),
                new SqlParameter("@cat4", System.Data.SqlDbType.NVarChar,500),
                new SqlParameter("@cat5", System.Data.SqlDbType.NVarChar,500),
                new SqlParameter("@certificate", System.Data.SqlDbType.NVarChar,60),
                new SqlParameter("@certstart", System.Data.SqlDbType.DateTime2),
                new SqlParameter("@certstop", System.Data.SqlDbType.DateTime2),
                new SqlParameter("@contractnmbr", System.Data.SqlDbType.NVarChar,20),
                new SqlParameter("@contractdate", System.Data.SqlDbType.DateTime2),
                new SqlParameter("@vendorcode", System.Data.SqlDbType.NVarChar,1000),
                new SqlParameter("@filepath", System.Data.SqlDbType.NVarChar,200),
                new SqlParameter("@colormark", System.Data.SqlDbType.NChar,9),
                new SqlParameter("@declarant", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@type", System.Data.SqlDbType.NVarChar,10),
                parstamp, parupdated, parupdater
            };
            mydeleteparams = new SqlParameter[] { parid };

            InsertProcedure = true;
            myinsertcommandtext = "spec.GoodsAdd_sp";
            UpdateProcedure = true;
            myupdatecommandtext = "spec.GoodsUpd_sp";
            DeleteProcedure = true;
            mydeletecommandtext = "spec.GoodsDel_sp";
        }

        public bool? Ending
        {
            get
            {
                return (bool)(SelectParams[1].Value??false);
            }
            set
            {
                SelectParams[1].Value = value;
            }
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
		protected override Goods CreateRecord(SqlDataReader reader)
		{
            Goods item = new Goods(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.IsDBNull(2) ? null : CustomBrokerWpf.References.Genders.FindFirstItem("Id", reader.GetInt32(2)),
                reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                reader.IsDBNull(12) ? string.Empty : reader.GetString(12),
                reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                reader.IsDBNull(15) ? (DateTime?)null : reader.GetDateTime(15),
                reader.IsDBNull(16) ? (DateTime?)null : reader.GetDateTime(16),
                reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
                reader.IsDBNull(18) ? (DateTime?)null : reader.GetDateTime(18),
                reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                reader.IsDBNull(21) ? string.Empty : reader.GetString(21),
                reader.IsDBNull(22) ? string.Empty : reader.GetString(22),
                reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                reader.GetInt64(24),
                reader.IsDBNull(25) ? (DateTime?)null : reader.GetDateTime(25),
                reader.IsDBNull(26) ? string.Empty : reader.GetString(26),
                lib.DomainObjectState.Unchanged);

            return KirillPolyanskiy.CustomBrokerWpf.References.GoodsStore.UpdateItem(item);
		}
        protected override Goods CreateModel(Goods reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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
        protected override void GetOutputParametersValue(Goods item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
                item.Stamp = (Int64)myinsertupdateparams[23].Value;
            }
            else if (item.DomainState == lib.DomainObjectState.Modified)
            {
                item.Stamp = (Int64)myinsertupdateparams[23].Value;
                item.UpdateWhen = (DateTime)myinsertupdateparams[24].Value;
                item.UpdateWho = (string)myinsertupdateparams[25].Value;
            }
        }
        protected override bool SaveChildObjects(Goods item)
        {
            if (item.DomainState == lib.DomainObjectState.Destroyed && System.IO.File.Exists(item.FilePath))
                try
                {
                    System.IO.File.Delete(item.FilePath);
                }
                catch (Exception ex) { this.Errors.Add(new lib.DBMError(item, ex.Message, "file")); }
            return true;
        }
        protected override bool SaveIncludedObject(Goods item)
        {
            bool isSuccess = true;
            if (item.Gender?.DomainState == lib.DomainObjectState.Added)
            {
                GenderDBM gdbm = new GenderDBM();
                isSuccess = gdbm.SaveItemChanches(item.Gender);
                if (!isSuccess)
                {
                    foreach (lib.DBMError err in gdbm.Errors) this.Errors.Add(err);
                }
            }
            return isSuccess;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(Goods item)
        {
            myupdateparams[0].Value = item.Id;
            myinsertupdateparams[0].Value = item.Name;
            myinsertupdateparams[1].Value = item.Gender == null ? (object)DBNull.Value : item.Gender.Id;
            myinsertupdateparams[2].Value = string.IsNullOrEmpty(item.Material) ? (object)DBNull.Value : item.Material;
            myinsertupdateparams[3].Value = string.IsNullOrEmpty(item.Contexture) ? (object)DBNull.Value : item.Contexture;
            myinsertupdateparams[4].Value = string.IsNullOrEmpty(item.ContextureNote) ? (object)DBNull.Value : item.ContextureNote;
            myinsertupdateparams[5].Value = string.IsNullOrEmpty(item.Brand) ? (object)DBNull.Value : item.Brand;
            myinsertupdateparams[6].Value = string.IsNullOrEmpty(item.Producer) ? (object)DBNull.Value : item.Producer;
            myinsertupdateparams[7].Value = string.IsNullOrEmpty(item.TitleCountry) ? (object)DBNull.Value : item.TitleCountry;
            myinsertupdateparams[8].Value = string.IsNullOrEmpty(item.Cat1) ? (object)DBNull.Value : item.Cat1;
            myinsertupdateparams[9].Value = string.IsNullOrEmpty(item.Cat2) ? (object)DBNull.Value : item.Cat2;
            myinsertupdateparams[10].Value = string.IsNullOrEmpty(item.Cat3) ? (object)DBNull.Value : item.Cat3;
            myinsertupdateparams[11].Value = string.IsNullOrEmpty(item.Cat4) ? (object)DBNull.Value : item.Cat4;
            myinsertupdateparams[12].Value = string.IsNullOrEmpty(item.Cat5) ? (object)DBNull.Value : item.Cat5;
            myinsertupdateparams[13].Value = string.IsNullOrEmpty(item.Certificate) ? (object)DBNull.Value : item.Certificate;
            myinsertupdateparams[14].Value = item.CertStart.HasValue ? item.CertStart : (object)DBNull.Value;
            myinsertupdateparams[15].Value = item.CertStop.HasValue ? item.CertStop : (object)DBNull.Value;
            myinsertupdateparams[16].Value = string.IsNullOrEmpty(item.ContractNmbr) ? (object)DBNull.Value : item.ContractNmbr;
            myinsertupdateparams[17].Value = item.ContractDate.HasValue ? item.ContractDate : (object)DBNull.Value;
            myinsertupdateparams[18].Value = string.IsNullOrEmpty(item.VendorCode) ? (object)DBNull.Value : item.VendorCode;
            myinsertupdateparams[19].Value = string.IsNullOrEmpty(item.FilePath) ? (object)DBNull.Value : item.FilePath;
            myinsertupdateparams[20].Value = string.IsNullOrEmpty(item.ColorMark) ? (object)DBNull.Value : item.ColorMark;
            myinsertupdateparams[21].Value = string.IsNullOrEmpty(item.Declarant) ? (object)DBNull.Value : item.Declarant;
            myinsertupdateparams[22].Value = string.IsNullOrEmpty(item.CertType) ? (object)DBNull.Value : item.CertType;
            myinsertupdateparams[23].Value = item.Stamp;
            mydeleteparams[0].Value = item.Id;
            return true;
        }
        protected override void ItemAcceptChanches(Goods item)
        {
            item.AcceptChanches();
        }
    }

    internal class GoodsStore : lib.DomainStorageLoad<Goods,Goods, GoodsDBM>
    {
        public GoodsStore(GoodsDBM dbm) : base(dbm) {}

        protected override void UpdateProperties(Goods olditem, Goods newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class GoodsVM : lib.ViewModelErrorNotifyItem<Goods>
    {
        public GoodsVM() : this(new Goods()) { }
        public GoodsVM(Goods domain) : base(domain)
        {
            ValidetingProperties.AddRange(new string[] { "Name" });
            DeleteRefreshProperties.AddRange(new string[] { "Name", "Gender", "Material", "Contexture", "ContextureNote", "Brand", "Producer", "TitleCountry", "Cat1", "Certificate", "ContractNmbr", "ContractDate", "VendorCode", "Declarant" });
            InitProperties();
            myfileopen = new RelayCommand(FileOpenExec, FileOpenCanExec);
            myfiledel = new RelayCommand(FileDeleteExec, FileDeleteCanExec);
        }

        private string myname;
        public string Name
        {
            set
            {
                if (!string.Equals(myname, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Name";
                    myname = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Name);
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name;
                        base.DomainObject.Name = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? myname : null; }
        }
        public Gender Gender
        {
            set
            {
                if (!object.Equals(base.DomainObject.Gender, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Gender";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Gender);
                    ChangingDomainProperty = name;
                    base.DomainObject.Gender = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Gender : null; }
        }
        public string Material
        {
            set
            {
                if (!string.Equals(base.DomainObject.Material, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Material";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Material);
                    ChangingDomainProperty = name;
                    base.DomainObject.Material = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Material : null; }
        }
        public string Contexture
        {
            set
            {
                if (!string.Equals(base.DomainObject.Contexture, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Contexture";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Contexture);
                    ChangingDomainProperty = name;
                    base.DomainObject.Contexture = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Contexture : null; }
        }
        public string ContextureNote
        {
            set
            {
                if (!string.Equals(base.DomainObject.ContextureNote, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "ContextureNote";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.ContextureNote);
                    ChangingDomainProperty = name;
                    base.DomainObject.ContextureNote = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.ContextureNote : null; }
        }
        public string Brand
        {
            set
            {
                if (!string.Equals(base.DomainObject.Brand, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Brand);
                    ChangingDomainProperty = name;
                    base.DomainObject.Brand = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Brand : null; }
        }
        public string Producer
        {
            set
            {
                if (!string.Equals(base.DomainObject.Producer, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Producer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Producer);
                    ChangingDomainProperty = name;
                    base.DomainObject.Producer = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Producer : null; }
        }
        public string TitleCountry
        {
            set
            {
                if (!string.Equals(base.DomainObject.TitleCountry, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "TitleCountry";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.TitleCountry);
                    ChangingDomainProperty = name;
                    base.DomainObject.TitleCountry = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.TitleCountry : null; }
        }
        public string Cat1
        {
            set
            {
                if (!string.Equals(base.DomainObject.Cat1, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Cat1";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Cat1);
                    ChangingDomainProperty = name;
                    base.DomainObject.Cat1 = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Cat1 : null; }
        }
        public string Cat2
        {
            set
            {
                if (!string.Equals(base.DomainObject.Cat2, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Cat2";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Cat2);
                    ChangingDomainProperty = name;
                    base.DomainObject.Cat2 = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Cat2 : null; }
        }
        public string Cat3
        {
            set
            {
                if (!string.Equals(base.DomainObject.Cat3, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Cat3";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Cat3);
                    ChangingDomainProperty = name;
                    base.DomainObject.Cat3 = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Cat3 : null; }
        }
        public string Cat4
        {
            set
            {
                if (!string.Equals(base.DomainObject.Cat4, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Cat4";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Cat4);
                    ChangingDomainProperty = name;
                    base.DomainObject.Cat4 = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Cat4 : null; }
        }
        public string Cat5
        {
            set
            {
                if (!string.Equals(base.DomainObject.Cat5, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Cat5";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Cat5);
                    ChangingDomainProperty = name;
                    base.DomainObject.Cat5 = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Cat5 : null; }
        }
        public string Certificate
        {
            set
            {
                if (!string.Equals(base.DomainObject.Certificate, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Certificate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Certificate);
                    ChangingDomainProperty = name;
                    base.DomainObject.Certificate = value;
                    PropertyChangedNotification("CertificateFull");
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Certificate : null; }
        }
        public string Certificate2
        { get { return this.DomainState != lib.DomainObjectState.Deleted ? (base.DomainObject.Certificate.IndexOf(" от") > 0 ? base.DomainObject.Certificate.Substring(0, base.DomainObject.Certificate.IndexOf(" от")): base.DomainObject.Certificate) : null; } }
        public DateTime? CertStart
        {
            set
            {
                if (base.DomainObject.CertStart.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(base.DomainObject.CertStart, value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "CertStart";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.CertStart);
                    ChangingDomainProperty = name;
                    base.DomainObject.CertStart = value;
                    PropertyChangedNotification("CertificateFull");
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.CertStart : null; }
        }
        public DateTime? CertStop
        {
            set
            {
                if (base.DomainObject.CertStop.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(base.DomainObject.CertStop, value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "CertStop";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.CertStop);
                    ChangingDomainProperty = name;
                    base.DomainObject.CertStop = value;
                    PropertyChangedNotification("CertificateFull");
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.CertStop : null; }
        }
        public string CertificateFull
        {
            get
            {
                return this.DomainState != lib.DomainObjectState.Deleted ?
                      base.DomainObject.Certificate
                      + (base.DomainObject.CertStart.HasValue ? " от " + base.DomainObject.CertStart.Value.ToShortDateString() : string.Empty)
                      + (base.DomainObject.CertStop.HasValue ? " до " + base.DomainObject.CertStop.Value.ToShortDateString() : string.Empty)
                  : null;
            }
        }
        public string ContractNmbr
        {
            set
            {
                if (!string.Equals(base.DomainObject.ContractNmbr, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "ContractNmbr";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.ContractNmbr);
                    ChangingDomainProperty = name;
                    base.DomainObject.ContractNmbr = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.ContractNmbr : null; }
        }
        public DateTime? ContractDate
        {
            set
            {
                if (base.DomainObject.ContractDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(base.DomainObject.ContractDate, value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "ContractDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.ContractDate);
                    ChangingDomainProperty = name;
                    base.DomainObject.ContractDate = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.ContractDate : null; }
        }
        public string VendorCode
        {
            set
            {
                if (!string.Equals(base.DomainObject.VendorCode, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "VendorCode";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.VendorCode);
                    ChangingDomainProperty = name;
                    base.DomainObject.VendorCode = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.VendorCode : null; }
        }
        public string FilePath
        {
            set
            {
                if (!string.Equals(base.DomainObject.FilePath, value) & !this.IsReadOnly)
                {
                    string name = "FilePath";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.FilePath);
                    ChangingDomainProperty = name;
                    base.DomainObject.FilePath = value;
                    PropertyChangedNotification("FileOpenImage");
                    PropertyChangedNotification("DeleteFileVisible");
                }
            }
            get { return this.IsEnabled ? base.DomainObject.FilePath : null; }
        }
        public object ColorMark
        {
            set
            {
                if (!(string.Equals(base.DomainObject.ColorMark, value) | IsReadOnly))
                {
                    string name = "ColorMark";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.ColorMark);
                    ChangingDomainProperty = name;
                    base.DomainObject.ColorMark = (string)value;
                }
            }
            get
            {
                object response=Binding.DoNothing;
                if (IsEnabled & !string.IsNullOrEmpty(base.DomainObject.ColorMark))
                {
                    System.Windows.Media.Color color = lib.Common.MsOfficeHelper.StringToColor(base.DomainObject.ColorMark);
                    response = new System.Windows.Media.SolidColorBrush(color);
                }
                return response;
            }
        }
        public string Declarant
        {
            set
            {
                if (!string.Equals(base.DomainObject.Declarant, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Declarant";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Declarant);
                    ChangingDomainProperty = name;
                    base.DomainObject.Declarant = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Declarant : null; }
        }
        public string CertType
        {
            set
            {
                if (!string.Equals(base.DomainObject.CertType, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "CertType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.CertType);
                    ChangingDomainProperty = name;
                    base.DomainObject.CertType = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.CertType : null; }
        }
        public int? DaysEnd
        { get { return this.IsEnabled ? this.DomainObject.DaysEnd : null; } }
        public string DaysEndColor
        { get { return this.DomainObject?.DaysEnd > 30 ? "LightYellow" : "Pink"; } }

        public System.Windows.Visibility DeleteFileVisible
        {
            get { return string.IsNullOrEmpty(this.FilePath) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; }
        }
        public string FileOpenImage
        {
            get { return string.IsNullOrEmpty(this.FilePath) ? "/CustomBrokerWpf;component/Images/add2.png" : "/CustomBrokerWpf;component/Images/certificate.png"; }
        }
        internal string myerrcode;
        private string myerrdesc;
        public string ErrDescription
        {
            set
            {
                myerrdesc = value;
                PropertyChangedNotification("ErrDescription");
                if (string.IsNullOrEmpty(myerrdesc))
                    ClearErrorMessageForProperty(string.Empty);
                else
                    AddErrorMessageForProperty(string.Empty, value);
            }
            get
            {
                return myerrdesc;
            }
        }

        private RelayCommand myfileopen;
        public ICommand FileOpen
        {
            get { return myfileopen; }
        }
        private void FileOpenExec(object parametr)
        {
            if (string.IsNullOrEmpty(this.FilePath))
                SertFileAdd();
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, this.FilePath));
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Не удалось открыть файл.\n" + ex.Message);
                }
            }
        }
        private bool FileOpenCanExec(object parametr)
        { return !this.IsReadOnly; }
        private void SertFileAdd()
        {
            System.Text.StringBuilder serverpath = new System.Text.StringBuilder();
            System.Text.StringBuilder serverdirpath = new System.Text.StringBuilder();
            string rootdir = CustomBrokerWpf.Properties.Settings.Default.SertFileRoot;
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.CheckPathExists = true;
            fd.CheckFileExists = true;
            if (System.IO.Directory.Exists(CustomBrokerWpf.Properties.Settings.Default.SertFileDefault)) fd.InitialDirectory = CustomBrokerWpf.Properties.Settings.Default.SertFileDefault;
            fd.Title = "Выбор файла с сертификатом";
            fd.Filter = "Файлы pdf (*.pdf)|*.pdf;";
            if (fd.ShowDialog().Value)
            {
                try
                {
                    serverpath.Clear();
                    if (fd.FileName.StartsWith(rootdir))
                        serverpath.Append(fd.FileName);
                    else
                    {
                        if (!System.IO.Directory.Exists(rootdir))
                            System.IO.Directory.CreateDirectory(rootdir);

                        serverpath.Append(System.IO.Path.Combine(rootdir, this.BuildFileName()+ System.IO.Path.GetExtension(fd.FileName)));

                        if (!string.IsNullOrEmpty(this.FilePath) && System.IO.File.Exists(System.IO.Path.Combine(rootdir, this.FilePath)))
                            try { System.IO.File.Delete(System.IO.Path.Combine(rootdir, this.FilePath)); } catch { }
                        if (System.IO.File.Exists(serverpath.ToString()))
                            System.IO.File.Delete(serverpath.ToString());
                        System.IO.File.Copy(fd.FileName, serverpath.ToString());
                    }
                    this.FilePath = System.IO.Path.GetFileName(serverpath.ToString());
                    if (CustomBrokerWpf.Properties.Settings.Default.SertFileDefault != System.IO.Path.GetDirectoryName(fd.FileName))
                    {
                        CustomBrokerWpf.Properties.Settings.Default.SertFileDefault = System.IO.Path.GetDirectoryName(fd.FileName);
                        CustomBrokerWpf.Properties.Settings.Default.Save();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Не удалось загрузить файл.\n" + ex.Message);
                }
            }
        }
        internal string BuildFileName()
        {
            string certificate;
            System.Text.StringBuilder filename = new System.Text.StringBuilder();

            if (this.Certificate.IndexOf('/') > -1)
                certificate = this.Certificate.Substring(0, this.Certificate.IndexOf('/'));
            else
                certificate = this.Certificate;
            if (certificate.IndexOf('\\') > -1)
                certificate = this.Certificate.Substring(0, this.Certificate.IndexOf('\\'));
            if (certificate.IndexOf(" от ") > -1)
                certificate = this.Certificate.Substring(0, this.Certificate.IndexOf(" от "));
            if (certificate.IndexOf(" с ") > -1)
                certificate = this.Certificate.Substring(0, this.Certificate.IndexOf(" с "));
            if (certificate.IndexOf(" c ") > -1)
                certificate = this.Certificate.Substring(0, this.Certificate.IndexOf(" c "));
            filename.Append(this.Producer + "_" + certificate).Replace("\\", string.Empty).Replace("/", string.Empty).Replace(":", string.Empty).Replace("*", string.Empty).Replace("?", string.Empty).Replace("\"", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty).Replace("|", string.Empty);
            return filename.ToString();
        }

        private RelayCommand myfiledel;
        public ICommand FileDelete
        {
            get { return myfiledel; }
        }
        private void FileDeleteExec(object parametr)
        {
            if (!string.IsNullOrEmpty(this.FilePath) && System.Windows.MessageBox.Show("Удалить файл?", "Удаление", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, this.FilePath)))
                    try { System.IO.File.Delete(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, this.FilePath)); }
                    catch (System.IO.IOException ex)
                    { System.Windows.MessageBox.Show("Не удается удалить файл!\nудалите файл самостоятельно.\n" + ex.Message, "Удаление"); }
                this.FilePath = string.Empty;
            }
        }
        private bool FileDeleteCanExec(object parametr)
        { return !(this.IsReadOnly | string.IsNullOrEmpty(this.FilePath)); }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Name":
                    myname = this.DomainObject.Name;
                    break;
                case "Certificate":
                case "CertStart":
                case "CertStop":
                    PropertyChangedNotification("CertificateFull");
                    break;
                case "DaysEnd":
                    PropertyChangedNotification("DaysEndColor");
                    break;
            }
        }
        protected override void InitProperties()
        {
            myname = this.DomainObject.Name;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Name":
                    if (myname != this.DomainObject.Name)
                        myname = this.DomainObject.Name;
                    else
                        this.Name = (string)value;
                    break;
                case "Gender":
                    this.DomainObject.Gender = (Gender)value;
                    break;
                case "Material":
                    this.DomainObject.Material = (string)value;
                    break;
                case "Contexture":
                    this.DomainObject.Contexture = (string)value;
                    break;
                case "ContextureNote":
                    this.DomainObject.ContextureNote = (string)value;
                    break;
                case "Brand":
                    this.DomainObject.Brand = (string)value;
                    break;
                case "Producer":
                    this.DomainObject.Producer = (string)value;
                    break;
                case "TitleCountry":
                    this.DomainObject.TitleCountry = (string)value;
                    break;
                case "Cat1":
                    this.DomainObject.Cat1 = (string)value;
                    break;
                case "Cat2":
                    this.DomainObject.Cat2 = (string)value;
                    break;
                case "Cat3":
                    this.DomainObject.Cat3 = (string)value;
                    break;
                case "Cat4":
                    this.DomainObject.Cat4 = (string)value;
                    break;
                case "Cat5":
                    this.DomainObject.Cat5 = (string)value;
                    break;
                case "Certificate":
                    this.DomainObject.Certificate = (string)value;
                    break;
                case "CertStart":
                    this.DomainObject.CertStart = (DateTime?)value;
                    break;
                case "CertStop":
                    this.DomainObject.CertStop = (DateTime?)value;
                    break;
                case "ContractNmbr":
                    this.DomainObject.ContractNmbr = (string)value;
                    break;
                case "ContractDate":
                    this.DomainObject.ContractDate = (DateTime?)value;
                    break;
                case "VendorCode":
                    this.DomainObject.VendorCode = (string)value;
                    break;
                case "FilePath":
                    this.DomainObject.FilePath = (string)value;
                    break;
                case "ColorMark":
                    this.DomainObject.ColorMark = (string)value;
                    break;
                case "Declarant":
                    this.DomainObject.Declarant = (string)value;
                    break;
                case "CertType":
                    this.DomainObject.CertType = (string)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Name":
                    if (string.IsNullOrEmpty(myname))
                    {
                        errmsg = "Отсутствует товар";
                        isvalid = false;
                    }
                    break;
                    //case "Certificate":
                    //    if (inform && !(!string.IsNullOrEmpty(myname)/* & this.CertStart.HasValue*/ & this.CertStop.HasValue) & (!string.IsNullOrEmpty(myname) | this.CertStart.HasValue | this.CertStop.HasValue))
                    //    {
                    //        errmsg = string.Empty;
                    //        if (string.IsNullOrEmpty(myname)) errmsg = errmsg + "Отсутствует сертификат";
                    //if (!this.CertStart.HasValue) errmsg = errmsg + "Отсутствует дата начала действия сертификата";
                    //    if (!this.CertStop.HasValue) errmsg = errmsg + "Отсутствует дата окончания действия сертификата";
                    //    isvalid = false;
                    //}
                    //break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myname!= base.DomainObject.Name;
        }
    }

    internal class GoodsSynchronizer : lib.ModelViewCollectionsSynchronizer<Goods, GoodsVM>
    {
        protected override Goods UnWrap(GoodsVM wrap)
        {
            return wrap.DomainObject as Goods;
        }

        protected override GoodsVM Wrap(Goods fill)
        {
            return new GoodsVM(fill);
        }
    }

    public class GoodsCommand : lib.ViewModelCommand<Goods,Goods, GoodsVM, GoodsDBM>
    {
        internal GoodsCommand(GoodsVM item, ListCollectionView view)
        {
            myvm = item;
            mydbm = new GoodsDBM();
            myview = view;
            myexhandler = new DataModelClassLibrary.ExceptionHandler("Сохранение изменений");
            mymaterials = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Materials);
            mymaterials.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            mygenders = new System.Windows.Data.ListCollectionView(CustomBrokerWpf.References.Genders);
            mygenders.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        public GoodsVM Item
        {
            set { myvm = value; }
            get { return myvm; }
        }
        private lib.ExceptionHandler myexhandler;

        private System.Windows.Data.ListCollectionView mymaterials;
        public System.Windows.Data.ListCollectionView Materials
        {
            get { return mymaterials; }
        }
        private System.Windows.Data.ListCollectionView mygenders;
        public System.Windows.Data.ListCollectionView Genders
        {
            get { return mygenders; }
        }

        protected override void AddData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override bool CanAddData(object parametr)
        {
            return false;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return myview != null;
        }
        protected override bool CanRefreshData()
        {
            return false;
        }
        protected override bool CanRejectChanges()
        {
            return true;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void DeleteData(object parametr)
        {
            base.DeleteData(parametr);
            if ((myvm.DomainState == lib.DomainObjectState.Deleted | myvm.DomainState == lib.DomainObjectState.Destroyed) && !string.IsNullOrEmpty(myvm.DomainObject.FilePath) && System.IO.File.Exists(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, myvm.DomainObject.FilePath)) & System.IO.Directory.Exists(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, "Удаленные")))
                try
                {
                    System.IO.File.Move(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, myvm.DomainObject.FilePath), System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, "Удаленные", myvm.DomainObject.FilePath));
                }
                catch (Exception ex)
                {
                    this.OpenPopup(ex.Message, true);
                }
        }
        protected override void RefreshData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override void RejectChanges(object parametr)
        {
            if (myvm.DomainState == lib.DomainObjectState.Deleted & !string.IsNullOrEmpty(myvm.DomainObject.FilePath) && System.IO.File.Exists(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, "Удаленные", myvm.DomainObject.FilePath)))
            {
                try
                {
                    System.IO.File.Move(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, "Удаленные", myvm.DomainObject.FilePath), System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, myvm.DomainObject.FilePath));
                }
                catch (Exception ex)
                {
                    this.OpenPopup(ex.Message, true);
                }
            }
            base.RejectChanges(parametr);
        }
    }

    public class GoodsViewCommand : lib.ViewModelViewCommand
    {
        internal GoodsViewCommand(GoodsDBM gdbm)
        {
            mygdbm = gdbm;
            MyInit(mygdbm);
        }
        internal GoodsViewCommand(bool ending)
        {
            mygdbm = new GoodsDBM();
            mygdbm.Ending = ending;
            mygdbm.Collection = new System.Collections.ObjectModel.ObservableCollection<Goods>();
            mygdbm.FillAsyncCompleted = () =>
            {
                mygoodsnamefiltercommand = new GoodsNameCheckListBoxVM();
                mygoodsnamefiltercommand.DeferredFill = true;
                mygoodsnamefiltercommand.ItemsSource = myview.OfType<GoodsVM>();
                mygoodsnamefiltercommand.ExecCommand1 = () => { FilterRunExec(null); };
                mygoodsnamefiltercommand.ExecCommand2 = () => { mygoodsnamefiltercommand.Clear(); };
                //myGoodsNameFilterCommand.ItemsWiewFilterFunc = (object item, string filter) => { string name = ((string)item).Trim().ToLower(); return name.IndexOf(", " + filter + ", ") > -1 || string.Equals(name, filter) || name.StartsWith(filter + ", ") || name.EndsWith(", " + filter); };

                myMaterialFilterCommand = new MaterialCheckListBoxVM();
                myMaterialFilterCommand.DeferredFill = true;
                myMaterialFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myMaterialFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myMaterialFilterCommand.ExecCommand2 = () => { myMaterialFilterCommand.Clear(); };

                myContextureFilterCommand = new ContextureCheckListBoxVM();
                myContextureFilterCommand.DeferredFill = true;
                myContextureFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myContextureFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myContextureFilterCommand.ExecCommand2 = () => { myContextureFilterCommand.Clear(); };

                myTNVEDFilterCommand = new TNVEDCheckListBoxVM();
                myTNVEDFilterCommand.DeferredFill = true;
                myTNVEDFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myTNVEDFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myTNVEDFilterCommand.ExecCommand2 = () => { myTNVEDFilterCommand.Clear(); };

                myBrandFilterCommand = new BrandCheckListBoxVM();
                myBrandFilterCommand.DeferredFill = true;
                myBrandFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myBrandFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myBrandFilterCommand.ExecCommand2 = () => { myBrandFilterCommand.Clear(); };

                myProducerFilterCommand = new ProducerCheckListBoxVM();
                myProducerFilterCommand.DeferredFill = true;
                myProducerFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myProducerFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myProducerFilterCommand.ExecCommand2 = () => { myProducerFilterCommand.Clear(); };

                myTitleCountryFilterCommand = new TitleCountryCheckListBoxVM();
                myTitleCountryFilterCommand.DeferredFill = true;
                myTitleCountryFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myTitleCountryFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myTitleCountryFilterCommand.ExecCommand2 = () => { myTitleCountryFilterCommand.Clear(); };

                myCat1FilterCommand = new Cat1CheckListBoxVM();
                myCat1FilterCommand.DeferredFill = true;
                myCat1FilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myCat1FilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myCat1FilterCommand.ExecCommand2 = () => { myCat1FilterCommand.Clear(); };

                myCertificateFilterCommand = new CertificateCheckListBoxVM();
                myCertificateFilterCommand.DeferredFill = true;
                myCertificateFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myCertificateFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myCertificateFilterCommand.ExecCommand2 = () => { myCertificateFilterCommand.Clear(); };

                myContractNmbrFilterCommand = new ContractNmbrCheckListBoxVM();
                myContractNmbrFilterCommand.DeferredFill = true;
                myContractNmbrFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myContractNmbrFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myContractNmbrFilterCommand.ExecCommand2 = () => { myContractNmbrFilterCommand.Clear(); };

                myVendorCodeFilterCommand = new VendorCodeCheckListBoxVM();
                myVendorCodeFilterCommand.DeferredFill = true;
                myVendorCodeFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myVendorCodeFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myVendorCodeFilterCommand.ExecCommand2 = () => { myVendorCodeFilterCommand.Clear(); };

                myDeclarantFilterCommand = new DeclarantCheckListBoxVM();
                myDeclarantFilterCommand.DeferredFill = true;
                myDeclarantFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myDeclarantFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myDeclarantFilterCommand.ExecCommand2 = () => { myDeclarantFilterCommand.Clear(); };

                myCertTypeFilterCommand = new CertTypeCheckListBoxVM();
                myCertTypeFilterCommand.DeferredFill = true;
                myCertTypeFilterCommand.ItemsSource = myview.OfType<GoodsVM>();
                myCertTypeFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
                myCertTypeFilterCommand.ExecCommand2 = () => { myCertTypeFilterCommand.Clear(); };

                PropertyChangedNotification("GoodsNameFilterCommand");
                PropertyChangedNotification("MaterialFilterCommand");
                PropertyChangedNotification("ContextureFilterCommand");
                PropertyChangedNotification("TNVEDFilterCommand");
                PropertyChangedNotification("BrandFilterCommand");
                PropertyChangedNotification("ProducerFilterCommand");
                PropertyChangedNotification("TitleCountryFilterCommand");
                PropertyChangedNotification("Cat1FilterCommand");
                PropertyChangedNotification("CertificateFilterCommand");
                PropertyChangedNotification("ContractNmbrFilterCommand");
                PropertyChangedNotification("VendorCodeFilterCommand");
                PropertyChangedNotification("DeclarantFilterCommand");
                PropertyChangedNotification("CertTypeFilterCommand");
                PropertyChangedNotification("GenderFilterCommand");

                mygdbm.FillAsyncCompleted = () => { };
            };
            mygdbm.FillAsync();
            MyInit(mygdbm);
        }
        internal GoodsViewCommand():this(false) { }
        private void MyInit(GoodsDBM gdbm)
        {
            mydbm = gdbm;
            mysync = new GoodsSynchronizer();
            mysync.DomainCollection = mygdbm.Collection;
            base.Collection = mysync.ViewModelCollection;

            myexhandler = new DataModelClassLibrary.ExceptionHandler("Сохранение");
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            //myfiltergenderclear = new RelayCommand(FilterGenderClearExec, FilterGenderClearCanExec);
            myexcelimport = new RelayCommand(ExcelImportExec, ExcelImportCanExec);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
            mylinksertfiles = new RelayCommand(LinkSertFilesExec, LinkSertFilesCanExec);

            myGenderFilterCommand = new libui.CheckListBoxVM();
            myGenderFilterCommand.DisplayPath = "Name";
            myGenderFilterCommand.GetDisplayPropertyValueFunc = (item) => { return ((Gender)item).Name; };
            myGenderFilterCommand.SearchPath = "Name";
            myGenderFilterCommand.Items = CustomBrokerWpf.References.Genders;
            myGenderFilterCommand.ItemsViewFilterDefault = lib.ViewModelViewCommand.ViewFilterDefault;
            myGenderFilterCommand.SelectedAll = false;
            myGenderFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
            myGenderFilterCommand.ExecCommand2 = () => { myGenderFilterCommand.Clear(); };
            myGenderFilterCommand.AreaFilterIsVisible = false;

            myCertStopFilterCommand = new libui.DateFilterVM();
            myCertStopFilterCommand.ExecCommand1 = () => { FilterRunExec(null); };
            myCertStopFilterCommand.ExecCommand2 = () => { FilterRunExec(null); };
        }

        private GoodsSynchronizer mysync;
        private GoodsDBM mygdbm;
        private lib.ExceptionHandler myexhandler;
        private System.ComponentModel.BackgroundWorker mybw;
        private ExcelImportWin myexcelimportwin;

        private GoodsNameCheckListBoxVM mygoodsnamefiltercommand;
        public GoodsNameCheckListBoxVM GoodsNameFilterCommand
        { get { return mygoodsnamefiltercommand; } }
        private MaterialCheckListBoxVM myMaterialFilterCommand;
        public MaterialCheckListBoxVM MaterialFilterCommand
        { get { return myMaterialFilterCommand; } }
        private ContextureCheckListBoxVM myContextureFilterCommand;
        public ContextureCheckListBoxVM ContextureFilterCommand
        { get { return myContextureFilterCommand; } }
        private TNVEDCheckListBoxVM myTNVEDFilterCommand;
        public TNVEDCheckListBoxVM TNVEDFilterCommand
        { get { return myTNVEDFilterCommand; } }
        private BrandCheckListBoxVM myBrandFilterCommand;
        public BrandCheckListBoxVM BrandFilterCommand
        { get { return myBrandFilterCommand; } }
        private ProducerCheckListBoxVM myProducerFilterCommand;
        public ProducerCheckListBoxVM ProducerFilterCommand
        { get { return myProducerFilterCommand; } }
        private TitleCountryCheckListBoxVM myTitleCountryFilterCommand;
        public TitleCountryCheckListBoxVM TitleCountryFilterCommand
        { get { return myTitleCountryFilterCommand; } }
        private Cat1CheckListBoxVM myCat1FilterCommand;
        public Cat1CheckListBoxVM Cat1FilterCommand
        { get { return myCat1FilterCommand; } }
        private CertificateCheckListBoxVM myCertificateFilterCommand;
        public CertificateCheckListBoxVM CertificateFilterCommand
        { get { return myCertificateFilterCommand; } }
        private ContractNmbrCheckListBoxVM myContractNmbrFilterCommand;
        public ContractNmbrCheckListBoxVM ContractNmbrFilterCommand
        { get { return myContractNmbrFilterCommand; } }
        private libui.DateFilterVM myCertStopFilterCommand;
        public libui.DateFilterVM CertStopFilterCommand
        { get { return myCertStopFilterCommand; } }
        private VendorCodeCheckListBoxVM myVendorCodeFilterCommand;
        public VendorCodeCheckListBoxVM VendorCodeFilterCommand
        { get { return myVendorCodeFilterCommand; } }
        private DeclarantCheckListBoxVM myDeclarantFilterCommand;
        public DeclarantCheckListBoxVM DeclarantFilterCommand
        { get { return myDeclarantFilterCommand; } }
        private CertTypeCheckListBoxVM myCertTypeFilterCommand;
        public CertTypeCheckListBoxVM CertTypeFilterCommand
        { get { return myCertTypeFilterCommand; } }
        private libui.CheckListBoxVM myGenderFilterCommand;
        public libui.CheckListBoxVM GenderFilterCommand
        { get { return myGenderFilterCommand; } }

        public GenderCollection Genders
        {
            get { return CustomBrokerWpf.References.Genders; }
        }

        //private string myfiltercode;
        //public string FilterCode
        //{
        //    set
        //    {
        //        myfiltercode = value;
        //        PropertyChangedNotification("FilterCode");
        //    }
        //    get { return myfiltercode; }
        //}
        //private string myfiltername;
        //public string FilterName
        //{
        //    set
        //    {
        //        myfiltername = value;
        //        PropertyChangedNotification("FilterName");
        //    }
        //    get { return myfiltername; }
        //}
        //private string myfiltercountry;
        //public string FilterCountry
        //{
        //    set
        //    {
        //        myfiltercountry = value;
        //        PropertyChangedNotification("FilterCountry");
        //    }
        //    get { return myfiltercountry; }
        //}
        //private string myfilterbrand;
        //public string FilterBrand
        //{
        //    set
        //    {
        //        myfilterbrand = value;
        //        PropertyChangedNotification("FilterBrand");
        //    }
        //    get { return myfilterbrand; }
        //}
        //private string myfilterproducer;
        //public string FilterProducer
        //{
        //    set
        //    {
        //        myfilterproducer = value;
        //        PropertyChangedNotification("FilterProducer");
        //    }
        //    get { return myfilterproducer; }
        //}
        //private string myfiltermaterial;
        //public string FilterMaterial
        //{
        //    set
        //    {
        //        myfiltermaterial = value;
        //        PropertyChangedNotification("FilterMaterial");
        //    }
        //    get { return myfiltermaterial; }
        //}
        //private string myfiltercontexture;
        //public string FilterContexture
        //{
        //    set
        //    {
        //        myfiltercontexture = value;
        //        PropertyChangedNotification("FilterContexture");
        //    }
        //    get { return myfiltercontexture; }
        //}
        //private string myfiltercertificatefull;
        //public string FilterCertificateFull
        //{
        //    set
        //    {
        //        myfiltercertificatefull = value;
        //        PropertyChangedNotification("FilterCertificateFull");
        //    }
        //    get { return myfiltercertificatefull; }
        //}
        //private string myfiltercontractnmbr;
        //public string FilterContractNmbr
        //{
        //    set
        //    {
        //        myfiltercontractnmbr = value;
        //        PropertyChangedNotification("FilterContractNmbr");
        //    }
        //    get { return myfiltercontractnmbr; }
        //}
        //private string myfiltervendorcode;
        //public string FilterVendorCode
        //{
        //    set
        //    {
        //        myfiltervendorcode = value;
        //        PropertyChangedNotification("FilterVendorCode");
        //    }
        //    get { return myfiltervendorcode; }
        //}
        //private DateTime? myfilrtercertificatestart;
        //public DateTime? FilrterCertificateStart
        //{
        //    set
        //    {
        //        myfilrtercertificatestart = value;
        //        PropertyChangedNotification("FilrterCertificateStart");
        //    }
        //    get { return myfilrtercertificatestart; }
        //}
        //private DateTime? myfilrtercertificatestop;
        //public DateTime? FilrterCertificateStop
        //{
        //    set
        //    {
        //        myfilrtercertificatestop = value;
        //        PropertyChangedNotification("FilrterCertificateStop");
        //    }
        //    get { return myfilrtercertificatestop; }
        //}
        //private DateTime? myfilrtercontractstart;
        //public DateTime? FilrterContractStart
        //{
        //    set
        //    {
        //        myfilrtercontractstart = value;
        //        PropertyChangedNotification("FilrterContractStart");
        //    }
        //    get { return myfilrtercontractstart; }
        //}
        //private DateTime? myfilrtercontractstop;
        //public DateTime? FilrterContractStop
        //{
        //    set
        //    {
        //        myfilrtercontractstop = value;
        //        PropertyChangedNotification("FilrterContractStop");
        //    }
        //    get { return myfilrtercontractstop; }
        //}
        //private Gender myfiltergender;
        //public Gender FilterGender
        //{
        //    set
        //    {
        //        myfiltergender = value;
        //        PropertyChangedNotification("FilterGender");
        //        if (myfiltergender != null) FilterRunExec(null);
        //    }
        //    get { return myfiltergender; }
        //}

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            this.EndEdit();
            myview.Filter = OnFilter;
        }
        private bool FilterRunCanExec(object parametr)
        { return true; }
        private bool OnFilter(object item)
        {
            bool where = lib.ViewModelViewCommand.ViewFilterDefault(item);
            GoodsVM gitem = item as GoodsVM;
            if (where & myTNVEDFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myTNVEDFilterCommand.SelectedItems)
                    if (gitem.ContextureNote == nameitem)
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myMaterialFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myMaterialFilterCommand.SelectedItems)
                    if (gitem.Material == nameitem)
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myContextureFilterCommand.FilterOn)
            {
                where = false;
                foreach (string name in myContextureFilterCommand.SelectedItems)
                    if (gitem.Contexture.IndexOf(", " + name + ", ") > -1 || gitem.Contexture == name || gitem.Contexture.StartsWith(name + ", ") || gitem.Contexture.EndsWith(", " + name))
                    {
                        where = true;
                        break;
                    }
            }
            if (where & mygoodsnamefiltercommand.FilterOn)
            {
                where = false;
                string nameitem = gitem.Name.ToLower();
                foreach (string name in mygoodsnamefiltercommand.SelectedItems)
                {
                    string lname = name.ToLower();
                    if (nameitem.IndexOf(", " + lname + ", ") > -1 || string.Equals(nameitem, lname) || nameitem.StartsWith(lname + ", ") || nameitem.EndsWith(", " + lname))
                    {
                        where = true;
                        break;
                    }
                }
            }
            if (where & myBrandFilterCommand.FilterOn)
            {
                where = false;
                string countries = gitem.Brand.ToLower();
                foreach (string nameitem in myBrandFilterCommand.SelectedItems)
                {
                    string lname = nameitem.ToLower();
                    if (countries == lname || countries.StartsWith(lname + ", ") || countries.EndsWith(", " + lname) || countries.IndexOf(", " + lname + ", ") > -1)
                    {
                        where = true;
                        break;
                    }
                }
            }
            if (where & myProducerFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myProducerFilterCommand.SelectedItems)
                    if (gitem.Producer == nameitem)
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myCertificateFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myCertificateFilterCommand.SelectedItems)
                    if (gitem.Certificate == nameitem)
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myContractNmbrFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myContractNmbrFilterCommand.SelectedItems)
                    if (gitem.ContractNmbr == nameitem)
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myTitleCountryFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myTitleCountryFilterCommand.SelectedItems)
                    if (gitem.TitleCountry.ToLower() == nameitem.ToLower())
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myCat1FilterCommand.FilterOn)
            {
                where = false;
                string countries = gitem.Cat1.ToLower();
                foreach (string nameitem in myCat1FilterCommand.SelectedItems)
                {
                    string lname = nameitem.ToLower();
                    if (countries == lname || countries.StartsWith(lname + ", ") || countries.EndsWith(", " + lname) || countries.IndexOf(", " + lname + ", ") > -1)
                    {
                        where = true;
                        break;
                    }
                }
            }
            if (where & myVendorCodeFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myVendorCodeFilterCommand.SelectedItems)
                    if (gitem.VendorCode == nameitem)
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myDeclarantFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myDeclarantFilterCommand.SelectedItems)
                    if (gitem.Declarant == nameitem)
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myCertTypeFilterCommand.FilterOn)
            {
                where = false;
                foreach (string nameitem in myCertTypeFilterCommand.SelectedItems)
                    if (gitem.CertType.Equals(nameitem))
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myGenderFilterCommand.FilterOn)
            {
                where = false;
                foreach (Gender gender in myGenderFilterCommand.SelectedItems)
                    if (object.Equals(gitem.Gender, gender))
                    {
                        where = true;
                        break;
                    }
            }
            if (where & myCertStopFilterCommand.FilterOn)
            {
                //if (!gitem.ContractDate.HasValue)
                //    where = false;
                //else if (myfilrtercontractstart.HasValue & myfilrtercontractstop.HasValue)
                //    where &= DateTime.Compare(gitem.ContractDate.Value, myfilrtercontractstart.Value) > -1 & DateTime.Compare(gitem.ContractDate.Value, myfilrtercontractstop.Value) < 1;
                //else if (myfilrtercontractstart.HasValue)
                //    where &= DateTime.Compare(gitem.ContractDate.Value, myfilrtercontractstart.Value) > -1;
                //else if (myfilrtercontractstop.HasValue)
                //    where &= DateTime.Compare(gitem.ContractDate.Value, myfilrtercontractstop.Value) < 1;
                where = gitem.CertStop.HasValue;
                if (where) where = !myCertStopFilterCommand.DateStart.HasValue || myCertStopFilterCommand.DateStart.Value.CompareTo(gitem.CertStop.Value) < 1;
                if (where) where &= !myCertStopFilterCommand.DateStop.HasValue || myCertStopFilterCommand.DateStop.Value.AddDays(1).CompareTo(gitem.CertStop.Value) > 0;
            }
            //if (where & (myfilrtercertificatestart.HasValue | myfilrtercertificatestop.HasValue))
            //{
            //    if (myfilrtercertificatestart.HasValue & myfilrtercertificatestop.HasValue)
            //        where &= gitem.CertStart.HasValue & gitem.CertStop.HasValue && DateTime.Compare(gitem.CertStart.Value, myfilrtercertificatestart.Value) < 1 & DateTime.Compare(gitem.CertStop.Value, myfilrtercertificatestop.Value) > -1;
            //    else if (myfilrtercertificatestart.HasValue)
            //        where &= gitem.CertStart.HasValue && DateTime.Compare(gitem.CertStart.Value, myfilrtercertificatestart.Value) < 1;
            //    else if (myfilrtercertificatestop.HasValue)
            //        where &= gitem.CertStop.HasValue && DateTime.Compare(gitem.CertStop.Value, myfilrtercertificatestop.Value) > -1;
            //}
            return where;
        }

        private RelayCommand myfilterclear;
        public ICommand FilterClear
        {
            get { return myfilterclear; }
        }
        private void FilterClearExec(object parametr)
        {
            myTNVEDFilterCommand.Clear();
            myTNVEDFilterCommand.IconVisibileChangedNotification();
            mygoodsnamefiltercommand.Clear();
            mygoodsnamefiltercommand.IconVisibileChangedNotification();
            myBrandFilterCommand.Clear();
            myBrandFilterCommand.IconVisibileChangedNotification();
            myProducerFilterCommand.Clear();
            myProducerFilterCommand.IconVisibileChangedNotification();
            myMaterialFilterCommand.Clear();
            myMaterialFilterCommand.IconVisibileChangedNotification();
            myContextureFilterCommand.Clear();
            myContextureFilterCommand.IconVisibileChangedNotification();
            myCertificateFilterCommand.Clear();
            myCertificateFilterCommand.IconVisibileChangedNotification();
            myContractNmbrFilterCommand.Clear();
            myContractNmbrFilterCommand.IconVisibileChangedNotification();
            myCertStopFilterCommand.Clear();
            myCertStopFilterCommand.IconVisibileChangedNotification();
            myTitleCountryFilterCommand.Clear();
            myTitleCountryFilterCommand.IconVisibileChangedNotification();
            myCat1FilterCommand.Clear();
            myCat1FilterCommand.IconVisibileChangedNotification();
            myVendorCodeFilterCommand.Clear();
            myVendorCodeFilterCommand.IconVisibileChangedNotification();
            myDeclarantFilterCommand.Clear();
            myDeclarantFilterCommand.IconVisibileChangedNotification();
            myCertTypeFilterCommand.Clear();
            myCertTypeFilterCommand.IconVisibileChangedNotification();
            myGenderFilterCommand.Clear();
            myGenderFilterCommand.IconVisibileChangedNotification();

            myview.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }

        //private RelayCommand myfiltergenderclear;
        //public ICommand FilterGenderClear
        //{
        //    get { return myfiltergenderclear; }
        //}
        //private void FilterGenderClearExec(object parametr)
        //{
        //    FilterGender = null;
        //}
        //private bool FilterGenderClearCanExec(object parametr)
        //{ return FilterGender != null; }
        private RelayCommand myexcelimport;
        public ICommand ExcelImport
        {
            get { return myexcelimport; }
        }
        private void ExcelImportExec(object parametr)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.CheckPathExists = true;
            fd.CheckFileExists = true;
            fd.Multiselect = false;
            fd.Title = "Выбор файла с данными";
            fd.Filter = "Файл Excel |*.xls;*.xlsx";
            fd.ShowDialog();
            if (System.IO.File.Exists(fd.FileName))
            {
                if (mybw == null)
                {
                    mybw = new System.ComponentModel.BackgroundWorker();
                    mybw.DoWork += BackgroundWorker_DoWork;
                    mybw.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                    mybw.WorkerReportsProgress = true;
                    mybw.ProgressChanged += BackgroundWorker_ProgressChanged;
                }
                if (!mybw.IsBusy)
                {
                    if (myexcelimportwin != null && myexcelimportwin.IsVisible)
                    {
                        myexcelimportwin.MessageTextBlock.Text = string.Empty;
                        myexcelimportwin.ProgressBar1.Value = 0;
                    }
                    else
                    {
                        myexcelimportwin = new ExcelImportWin();
                        myexcelimportwin.Show();
                    }
                    string[] arg = { "true", fd.FileName, (System.Windows.MessageBox.Show("Пропускать уже имеющиеся позиции (по номеру)?\nИмеющиеся позиции не будут обновлены значениями из файла.", "Загрузка данных", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes).ToString() };
                    mybw.RunWorkerAsync(arg);
                }
                else
                {
                    System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
                }
            }
        }
        private bool ExcelImportCanExec(object parametr)
        { return true; }

        private RelayCommand myexcelexport;
        public ICommand ExcelExport
        {
            get { return myexcelexport; }
        }
        private void ExcelExportExec(object parametr)
        {
            this.myendedit();
            if (mybw == null)
            {
                mybw = new System.ComponentModel.BackgroundWorker();
                mybw.DoWork += BackgroundWorker_DoWork;
                mybw.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                mybw.WorkerReportsProgress = true;
                mybw.ProgressChanged += BackgroundWorker_ProgressChanged;
            }
            if (!mybw.IsBusy)
            {
                if (myexcelimportwin != null && myexcelimportwin.IsVisible)
                {
                    myexcelimportwin.MessageTextBlock.Text = string.Empty;
                    myexcelimportwin.ProgressBar1.Value = 0;
                }
                else
                {
                    myexcelimportwin = new ExcelImportWin();
                    myexcelimportwin.Show();
                }
                string[] arg = { "false" };
                mybw.RunWorkerAsync(arg);
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool ExcelExportCanExec(object parametr)
        { return true; }

        private RelayCommand mylinksertfiles;
        public ICommand LinkSertFiles
        {
            get { return mylinksertfiles; }
        }
        private void LinkSertFilesExec(object parametr)
        {
            System.IO.FileInfo[] files;
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot);
            foreach (object item in myview)
            {
                if (item is GoodsVM && string.IsNullOrEmpty((item as GoodsVM).FilePath))
                {
                    GoodsVM gitem = item as GoodsVM;
                    files = dir.GetFiles(gitem.BuildFileName() + ".????");
                    if (files.Length > 0)
                        gitem.FilePath = files[0].Name;
                }
            }
            this.OpenPopup("Файлы сертификатов привязаны",false);
        }
        private bool LinkSertFilesCanExec(object parametr)
        { return !myview.IsAddingNew; }

        //public override bool SaveDataChanges()
        //{
        //    this.PopupText = "Изменения сохранены";
        //    bool isSuccess = !(myview.CurrentItem is GoodsVM) || (myview.CurrentItem as GoodsVM).Validate(true);
        //    if (isSuccess)
        //    {
        //        mydbm.Errors.Clear();
        //        isSuccess = mydbm.SaveCollectionChanches();
        //    }
        //    if (mydbm.Errors.Count > 0)
        //    {
        //        myexhandler.Handle(new Exception(mydbm.ErrorMessage));
        //        myexhandler.ShowMessage();
        //    }
        //    return isSuccess;
        //}
        protected override void AddData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override bool CanAddData(object parametr)
        {
            return true;
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
            mygdbm.Collection.Clear();
            mygdbm.Fill();
        }
        protected override void RejectChanges(object parametr)
        {
            List<GoodsVM> destroied = new List<GoodsVM>();
            foreach (GoodsVM item in mysync.ViewModelCollection)
            {
                if (item.DomainState == lib.DomainObjectState.Deleted & !string.IsNullOrEmpty(item.DomainObject.FilePath) && System.IO.File.Exists(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, "Удаленные", item.DomainObject.FilePath)))
                {
                    try
                    {
                        System.IO.File.Move(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, "Удаленные", item.DomainObject.FilePath), System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, item.DomainObject.FilePath));
                    }
                    catch (Exception ex)
                    {
                        this.OpenPopup(ex.Message, true);
                    }
                }
                if (item.DomainState == lib.DomainObjectState.Added)
                    destroied.Add(item);
                else if (item.DomainState != lib.DomainObjectState.Unchanged)
                {
                    base.myview.EditItem(item);
                    item.RejectChanges();
                    base.myview.CommitEdit();
                }
            }
            foreach (GoodsVM item in destroied) mysync.ViewModelCollection.Remove(item);
        }
        protected override void SettingView() { }
        protected override void DeleteData(object parametr)
        {
            List<Goods> items = new List<Goods>();
            if (parametr is System.Collections.IEnumerable & System.IO.Directory.Exists(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, "Удаленные")))
                foreach (object item in (parametr as System.Collections.IEnumerable))
                    if (item is GoodsVM) items.Add((item as GoodsVM).DomainObject);
            base.DeleteData(parametr);
            foreach (Goods item in items)
                if ((item.DomainState == lib.DomainObjectState.Deleted | item.DomainState == lib.DomainObjectState.Destroyed) && !string.IsNullOrEmpty(item.FilePath) && System.IO.File.Exists(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, item.FilePath)))
                    try
                    {
                        System.IO.File.Move(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, item.FilePath), System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.SertFileRoot, "Удаленные", item.FilePath));
                    }
                    catch (Exception ex)
                    {
                        this.OpenPopup(ex.Message, true);
                    }
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Excel.Application exApp = new Excel.Application();
            Excel.Application exAppProt = new Excel.Application();
            exApp.Visible = false;
            exApp.DisplayAlerts = false;
            exApp.ScreenUpdating = false;
            string[] args = e.Argument as string[];
            bool isclose = bool.Parse(args[0]);
            try
            {
                if (isclose)
                    e.Result = OnExcelImport(worker, exApp, args[1], bool.Parse(args[2]));
                else
                    e.Result = OnExcelExport(worker, exApp);
                worker.ReportProgress(100);

            }
            finally
            {
                if (exApp != null)
                {
                    if (isclose)
                    {
                        foreach (Excel.Workbook itemBook in exApp.Workbooks)
                        {
                            itemBook.Close(false);
                        }
                        exApp.DisplayAlerts = true;
                        exApp.ScreenUpdating = true;
                        exApp.Quit();
                    }
                    else
                    {
                        exApp.Visible = true;
                        exApp.DisplayAlerts = true;
                        exApp.ScreenUpdating = true;
                    }
                    exApp = null;
                }
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                myexcelimportwin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                myexcelimportwin.MessageTextBlock.Text = "Загрузка прервана из-за ошибки" + "\n" + e.Error.Message;
            }
            else
            {
                myexcelimportwin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                myexcelimportwin.MessageTextBlock.Text = "Загрузка выполнена успешно." + "\n" + e.Result.ToString() + " строк обработано";
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            myexcelimportwin.ProgressBar1.Value = e.ProgressPercentage;
        }

        private int OnExcelImport(BackgroundWorker worker, Excel.Application exApp, string filepath, bool ismiss)
        {
            int maxr, id = 0;
            string[] dformats = { "dd.MM.yy", "dd-MM-yy", "dd//MM//yy" };
            DateTime d;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            GoodsVM newgoods;

            Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
            Excel.Worksheet exWh = exWb.Sheets[1];
            maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            for (int r = 2; r <= maxr; r++)
            {
                if (string.IsNullOrEmpty((exWh.Cells[r, 2].Text as string).Trim())) continue;
                newgoods = null;
                if (int.TryParse((exWh.Cells[r, 25].Text as string).Trim(), out id))
                    foreach (GoodsVM item in mysync.ViewModelCollection)
                        if (item.DomainObject.Id == id)
                        {
                            newgoods = item;
                            break;
                        }
                if (newgoods == null) newgoods = new GoodsVM();
                else if (ismiss) continue;

                str.Clear();
                str.Append((exWh.Cells[r, 1].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 10)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 1].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.CertType = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 2].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 1000)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 2].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.Name = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 3].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 10)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 3].Address(false, false) + " содержит слишком длинный текст!");
                if (str.Length > 0 && !str.Equals('-'))
                {
                    Gender gender = CustomBrokerWpf.References.Genders.FindFirstItem("ShortName", str.ToString());
                    if (gender == null)
                        gender = CustomBrokerWpf.References.Genders.FindFirstItem("Name", str.ToString());
                    if (gender == null)
                        throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 3].Address(false, false) + @" пол """ + str.ToString() + @""" не найден!");
                    else
                        newgoods.Gender = gender;
                }
                str.Clear();
                str.Append((exWh.Cells[r, 4].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 100)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 4].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.Material = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 5].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 1000)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 5].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.Contexture = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 6].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 300)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 6].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.ContextureNote = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 7].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                //if (str.Length > 4000)
                //    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 6].Address(false, false) + " содержит слишком длинный текст!");
                //else
                newgoods.Brand = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 8].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 100)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 8].Address(false, false) + " содержит слишком длинный текст!");
                newgoods.Producer = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 9].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 100)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 9].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.TitleCountry = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 10].Text as string).Trim().TrimEnd(',').TrimEnd()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 2000)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 10].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.Cat1 = str.ToString();
                //str.Clear();
                //str.Append((exWh.Cells[r, 10].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                //if (str.Length > 500)
                //    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 10].Address(false, false) + " содержит слишком длинный текст!");
                //else
                //    newgoods.Cat3 = str.ToString();
                //str.Clear();
                //str.Append((exWh.Cells[r, 11].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                //if (str.Length > 500)
                //    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 11].Address(false, false) + " содержит слишком длинный текст!");
                //else
                //    newgoods.Cat4 = str.ToString();
                //str.Clear();
                //str.Append((exWh.Cells[r, 12].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                //if (str.Length > 500)
                //    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 12].Address(false, false) + " содержит слишком длинный текст!");
                //newgoods.Cat5 = str.ToString();
                //str.Clear();
                //str.Append((exWh.Cells[r, 13].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                //if (str.Length > 500)
                //    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 13].Address(false, false) + " содержит слишком длинный текст!");
                //else
                //    newgoods.Cat2 = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 11].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 60)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 11].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.Certificate = str.ToString();
                //{
                //int n = str.ToString().IndexOf(" от");
                //if (n > 0)
                //{
                //    newgoods.Certificate = str.ToString().Substring(0, n);
                //    if (DateTime.TryParseExact(str.ToString().Substring(n + 4, 8), "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                //        newgoods.CertStart = d;
                //    else
                //        throw new ApplicationException("Не удалось разобрать дату ДС от ячейка Excel " + exWh.Cells[r, 14].Address(false, false));
                //}
                //else
                //{
                //    n = str.ToString().IndexOf(" до");
                //    if (n > 0)
                //    {
                //        newgoods.Certificate = str.ToString().Substring(0, n);
                //    }
                //    else
                //        newgoods.Certificate = str.ToString();
                //}
                //}
                str.Clear();
                str.Append((exWh.Cells[r, 13].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 0)
                {
                    if (DateTime.TryParseExact(str.ToString(), dformats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                        newgoods.CertStop = d;
                    else
                        throw new ApplicationException("Не удалось разобрать дату окончания ДС ячейка Excel " + exWh.Cells[r, 13].Address(false, false));
                }
                str.Clear();
                str.Append((exWh.Cells[r, 12].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 20)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 12].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.ContractNmbr = str.ToString();
                //{
                //    newgoods.ContractNmbr = str.ToString().Substring(0, str.Length - 9);
                //    if (DateTime.TryParseExact(str.ToString().Substring(str.Length - 8), "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                //        newgoods.ContractDate = d;
                //    else
                //        throw new ApplicationException("Не удалось разобрать дату договора ячейка Excel " + exWh.Cells[r, 14].Address(false, false));
                //}
                str.Clear();
                str.Append((exWh.Cells[r, 14].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 1000)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 14].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.VendorCode = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 15].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 50)
                    throw new ApplicationException("Ячейка Excel " + exWh.Cells[r, 15].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newgoods.Declarant = str.ToString();

                if (!mysync.ViewModelCollection.Contains(newgoods)) this.myview.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<GoodsVM>(mysync.ViewModelCollection.Add), newgoods);
                worker.ReportProgress((int)(decimal.Divide(r, maxr) * 100));
            }
            exWb.Close();
            return maxr;
        }
        private int OnExcelExport(BackgroundWorker worker, Excel.Application exApp)
        {
            Excel.Workbook exWb;
            try
            {
                int row = 2;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                Excel.Range r;

                exWh.Cells[1, 1] = "ТИП"; exWh.Cells[1, 2] = "ТОВАР"; exWh.Cells[1, 3] = "ПОЛ"; exWh.Cells[1, 4] = "МАТЕРИАЛ"; exWh.Cells[1, 5] = "ТКАНЬ"; exWh.Cells[1, 6] = "ГРУППА ТН ВЭД";
                exWh.Cells[1, 7] = "ТОРГОВАЯ МАРКА"; exWh.Cells[1, 8] = "ПРОИЗВОДИТЕЛЬ"; exWh.Cells[1, 9] = "ТИТУЛЬНАЯ\nСТРАНА"; exWh.Cells[1, 10] = "СТРАНЫ"; exWh.Cells[1, 11] = "НОМЕР ДС, СРТ";
                exWh.Cells[1, 12] = "ДОГОВОР НА\nИСПОЛЬЗОВАНИЕ"; exWh.Cells[1, 13] = "ДАТА\nОКОНЧАНИЯ\nДС, СРТ"; exWh.Cells[1, 14] = "АРТИКУЛЫ\nПРИМЕЧАНИЯ"; exWh.Cells[1, 15] = "ЗАЯВИТЕЛЬ";

                exWh.Columns[1, Type.Missing].NumberFormat = "@";
                exWh.Columns[2, Type.Missing].NumberFormat = "@";
                exWh.Columns[3, Type.Missing].NumberFormat = "@";
                exWh.Columns[4, Type.Missing].NumberFormat = "@";
                exWh.Columns[5, Type.Missing].NumberFormat = "@";
                exWh.Columns[6, Type.Missing].NumberFormat = "@";
                exWh.Columns[7, Type.Missing].NumberFormat = "@";
                exWh.Columns[8, Type.Missing].NumberFormat = "@";
                exWh.Columns[9, Type.Missing].NumberFormat = "@";
                exWh.Columns[10, Type.Missing].NumberFormat = "@";
                exWh.Columns[11, Type.Missing].NumberFormat = "@";
                exWh.Columns[12, Type.Missing].NumberFormat = "@";
                //exWh.Columns[13, Type.Missing].NumberFormat = "d/m/yyyy";
                exWh.Columns[14, Type.Missing].NumberFormat = "@";
                exWh.Columns[15, Type.Missing].NumberFormat = "@";
                foreach (object itemobj in myview)
                {
                    if (!(itemobj is GoodsVM)) continue;

                    GoodsVM item = itemobj as GoodsVM;
                    exWh.Cells[row, 1] = item.CertType;
                    exWh.Cells[row, 2] = item.Name;
                    exWh.Cells[row, 3] = item.Gender.Name;
                    exWh.Cells[row, 4] = item.Material;
                    exWh.Cells[row, 5] = item.Contexture;
                    exWh.Cells[row, 6] = item.ContextureNote;
                    exWh.Cells[row, 7] = item.Brand;
                    exWh.Cells[row, 8] = item.Producer;
                    exWh.Cells[row, 9] = item.TitleCountry;
                    exWh.Cells[row, 10] = item.Cat1;
                    exWh.Cells[row, 11] = item.Certificate;
                    exWh.Cells[row, 12] = item.ContractNmbr;
                    exWh.Cells[row, 13] = item.CertStop.HasValue ? item.CertStop.Value.ToString("dd.MM.yyyy") : string.Empty;
                    exWh.Cells[row, 14] = item.VendorCode;
                    exWh.Cells[row, 15] = item.Declarant;
                    exWh.Cells[row, 25] = item.DomainObject.Id;

                    row++;
                }

                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, 15]];
                r.WrapText = true;
                r.Interior.Color = 14348258;
                r.VerticalAlignment = Excel.Constants.xlCenter;
                r.HorizontalAlignment = Excel.Constants.xlCenter;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[row, 15]];
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlEdgeRight].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                exApp.Visible = true;
                exWh = null;
                return row - 2;
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
                    exApp = null;
                }
                throw new ApplicationException(ex.Message);
            }
        }
    }

    internal class GetProducerDBM : lib.DBMSFill<string,string>
    {
        internal GetProducerDBM(int clientid) : base()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectCommandText = "spec.GetAllProducers_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@clientid", clientid) };
        }

        protected override void PrepareFill(SqlConnection addcon)
        {
        }
		protected override string CreateRecord(SqlDataReader reader)
		{
            return reader.GetString(0);
		}
		protected override string CreateModel(string reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
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
    }

    public class GoodsNameCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            string[] names;
            bool contains;
            names = item.Name.Trim(new char[] { ' ', ',' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string name in names)
            {
                contains = false;
                foreach (string goods in Items)
                    if (string.Equals(goods, name, StringComparison.CurrentCultureIgnoreCase))
                    { contains = true; break; }
                if (!contains) Items.Add(name);
            }
        }
    }
    public class MaterialCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            if (!Items.Contains(item.Material)) Items.Add(item.Material);
        }
    }
    public class ContextureCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            string[] names;
            names = item.Contexture.Trim(new char[] { ' ', ',' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string name in names)
                if (!Items.Contains(name))
                    Items.Add(name);
        }
    }
    public class TNVEDCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            if (!Items.Contains(item.ContextureNote)) Items.Add(item.ContextureNote);
        }
    }
    public class BrandCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            bool contains = false;
            string[] names;
            names = item.Brand.Trim(new char[] { ' ', ',' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string name in names)
            {
                contains = false;
                foreach (string contry in Items)
                    if (string.Equals(contry, name, StringComparison.CurrentCultureIgnoreCase))
                    { contains = true; break; }
                if (!contains) Items.Add(name);
            }
        }
    }
    public class ProducerCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            if (!Items.Contains(item.Producer)) Items.Add(item.Producer);
        }
    }
    public class TitleCountryCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            if (!Items.Contains(item.TitleCountry)) Items.Add(item.TitleCountry);
        }
    }
    public class Cat1CheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            bool contains = false;
            string[] names;
            names = item.Cat1.Trim(new char[] { ' ', ',' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string name in names)
            {
                contains = false;
                foreach (string contry in Items)
                    if (string.Equals(contry, name, StringComparison.CurrentCultureIgnoreCase))
                    { contains = true; break; }
                if (!contains) Items.Add(name);
            }
        }
    }
    public class CertificateCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            if (!Items.Contains(item.Certificate)) Items.Add(item.Certificate);
        }
    }
    public class ContractNmbrCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            if (!Items.Contains(item.ContractNmbr)) Items.Add(item.ContractNmbr);
        }
    }
    public class VendorCodeCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            if (!Items.Contains(item.VendorCode)) Items.Add(item.VendorCode);
        }
    }
    public class DeclarantCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            if (!Items.Contains(item.Declarant)) Items.Add(item.Declarant);
        }
    }
    public class CertTypeCheckListBoxVM : libui.CheckListBoxVMFill<GoodsVM, string>
    {
        protected override void AddItem(GoodsVM item)
        {
            if (!Items.Contains(item.CertType)) Items.Add(item.CertType);
        }
    }
}
