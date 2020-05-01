using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class AllPrice : lib.DomainBaseStamp
    {
        public AllPrice() : this(lib.NewObjectId.NewId, string.Empty, string.Empty, null, string.Empty, string.Empty, string.Empty, null,null, null, null,null,null,null,null,null,null,null,null,null,null,null,null,DateTime.Today,null,null
            , 0, null, null, lib.DomainObjectState.Added) { }
        public AllPrice(int id, string code, string name, string spelling, string composition, string translation, string unit, string risk, string note, decimal? addper
            , decimal? cat1brand, decimal? cat1no, decimal? cat2brand, decimal? cat2no, decimal? cat3brand, decimal? cat3no, decimal? cat4brand, decimal? cat4no, decimal? cat5brand, decimal? cat5no
            , decimal? ratevat, decimal? rateper, decimal? rateadd, DateTime ratedate, decimal? rateper2009, decimal? rateadd2009
            , Int64 stamp,DateTime? updated,string updater, lib.DomainObjectState state):base(id, stamp, updated, updater, state)
        {
            mycode = code;
            myname = name;
            myspelling = spelling;
            mytranslation = translation;
            mycomposition = composition;
            myunit = unit;
            myrisk = risk;
            mynote = note;
            myaddper = addper;
            mycat1brand = cat1brand;
            mycat1no = cat1no;
            mycat2brand = cat2brand;
            mycat2no = cat2no;
            mycat3brand = cat3brand;
            mycat3no = cat3no;
            mycat4brand = cat4brand;
            mycat4no = cat4no;
            mycat5brand = cat5brand;
            mycat5no = cat5no;
            myratevat = ratevat;
            myrateper = rateper;
            myrateadd = rateadd;
            myratedate = ratedate;
            myrateper2009 = rateper2009;
            myrateadd2009 = rateadd2009;
        }

        private string mycode;
        public string Code
        {
            set
            {
                if (!string.Equals(mycode, value))
                {
                    string name = "Code";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycode);
                    mycode = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycode; }
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
        private string myspelling;
        public string Spelling
        {
            set
            {
                if (!string.Equals(myspelling, value))
                {
                    string name = "Spelling";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myspelling);
                    myspelling = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myspelling; }
        }
        private string mytranslation;
        public string Translation
        {
            set
            {
                if (!string.Equals(mytranslation, value))
                {
                    string name = "Translation";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mytranslation);
                    mytranslation = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mytranslation; }
        }
        private string mycomposition;
        public string Composition
        {
            set
            {
                if (!string.Equals(mycomposition, value))
                {
                    string name = "Composition";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycomposition);
                    mycomposition = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycomposition; }
        }
        private string myunit;
        public string Unit
        {
            set
            {
                if (!string.Equals(myunit, value))
                {
                    string name = "Unit";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myunit);
                    myunit = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myunit; }
        }
        private string myrisk;
        public string Risk
        {
            set
            {
                if (!string.Equals(myrisk, value))
                {
                    string name = "Risk";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myrisk);
                    myrisk = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myrisk; }
        }
        private string mynote;
        public string Note
        {
            set
            {
                if (!string.Equals(mynote, value))
                {
                    string name = "Note";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mynote);
                    mynote = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mynote; }
        }
        private decimal? myaddper;
        public decimal? AddPer
        {
            set
            {
                if (myaddper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myaddper.Value, value.Value)))
                {
                    string name = "AddPer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myaddper);
                    myaddper = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myaddper; }
        }
        private decimal? mycat1brand;
        public decimal? Category1Brand
        {
            set
            {
                if (mycat1brand.HasValue!=value.HasValue || (value.HasValue && !decimal.Equals(mycat1brand.Value, value.Value)))
                {
                    string name = "Category1Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat1brand);
                    mycat1brand = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat1brand; }
        }
        private decimal? mycat1no;
        public decimal? Category1No
        {
            set
            {
                if (mycat1no.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycat1no.Value, value.Value)))
                {
                    string name = "Category1No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat1no);
                    mycat1no = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat1no; }
        }
        private decimal? mycat2brand;
        public decimal? Category2Brand
        {
            set
            {
                if (mycat2brand.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycat2brand.Value, value.Value)))
                {
                    string name = "Category2Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat2brand);
                    mycat2brand = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat2brand; }
        }
        private decimal? mycat2no;
        public decimal? Category2No
        {
            set
            {
                if (mycat2no.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycat2no.Value, value.Value)))
                {
                    string name = "Category2No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat2no);
                    mycat2no = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat2no; }
        }
        private decimal? mycat3brand;
        public decimal? Category3Brand
        {
            set
            {
                if (mycat3brand.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycat3brand.Value, value.Value)))
                {
                    string name = "Category3Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat3brand);
                    mycat3brand = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat3brand; }
        }
        private decimal? mycat3no;
        public decimal? Category3No
        {
            set
            {
                if (mycat3no.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycat3no.Value, value.Value)))
                {
                    string name = "Category3No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat3no);
                    mycat3no = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat3no; }
        }
        private decimal? mycat4brand;
        public decimal? Category4Brand
        {
            set
            {
                if (mycat4brand.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycat4brand.Value, value.Value)))
                {
                    string name = "Category4Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat4brand);
                    mycat4brand = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat4brand; }
        }
        private decimal? mycat4no;
        public decimal? Category4No
        {
            set
            {
                if (mycat4no.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycat4no.Value, value.Value)))
                {
                    string name = "Category4No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat4no);
                    mycat4no = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat4no; }
        }
        private decimal? mycat5brand;
        public decimal? Category5Brand
        {
            set
            {
                if (mycat5brand.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycat5brand.Value, value.Value)))
                {
                    string name = "Category5Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat5brand);
                    mycat5brand = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat5brand; }
        }
        private decimal? mycat5no;
        public decimal? Category5No
        {
            set
            {
                if (mycat5no.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycat5no.Value, value.Value)))
                {
                    string name = "Category5No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycat5no);
                    mycat5no = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycat5no; }
        }
        private decimal? myratevat;
        public decimal? RateVat
        {
            set
            {
                if (myratevat.HasValue!=value.HasValue || (value.HasValue && !decimal.Equals(myratevat.Value, value.Value)))
                {
                    string name = "RateVat";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myratevat);
                    myratevat = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myratevat; }
        }
        private decimal? myrateper;
        public decimal? RatePer
        {
            set
            {
                if (myrateper.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myrateper, value)))
                {
                    string name = "RatePer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myrateper);
                    myrateper = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myrateper; }
        }
        private decimal? myrateadd;
        public decimal? RateAdd
        {
            set
            {
                if (myrateadd.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myrateadd, value)))
                {
                    string name = "RateAdd";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myrateadd);
                    myrateadd = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myrateadd; }
        }
        private DateTime myratedate;
        public DateTime RateDate
        {
            set
            {
                if (!DateTime.Equals(myratedate, value))
                {
                    string name = "RateDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myratedate);
                    myratedate = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myratedate; }
        }
        private decimal? myrateper2009;
        public decimal? RatePer2009
        {
            set
            {
                if (myrateper2009.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myrateper2009, value)))
                {
                    string name = "RatePer2009";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myrateper2009);
                    myrateper2009 = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myrateper2009; }
        }
        private decimal? myrateadd2009;
        public decimal? RateAdd2009
        {
            set
            {
                if (myrateadd2009.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myrateadd2009, value)))
                {
                    string name = "RateAdd2009";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myrateadd2009);
                    myrateadd2009 = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myrateadd2009; }
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Code":
                    mycode = (string)value;
                    break;
                case "Name":
                    myname = (string)value;
                    break;
                case "Spelling":
                    myspelling = (string)value;
                    break;
                case "Translation":
                    mytranslation = (string)value;
                    break;
                case "Composition":
                    mycomposition = (string)value;
                    break;
                case "Unit":
                    myunit = (string)value;
                    break;
                case "Risk":
                    myrisk = (string)value;
                    break;
                case "Note":
                    mynote = (string)value;
                    break;
                case "AddPer":
                    myaddper = (decimal?)value;
                    break;
                case "Category1Brand":
                    mycat1brand = (decimal?)value;
                    break;
                case "Category1No":
                    mycat1no = (decimal?)value;
                    break;
                case "Category2Brand":
                    mycat2brand = (decimal?)value;
                    break;
                case "Category2No":
                    mycat2no = (decimal?)value;
                    break;
                case "Category3Brand":
                    mycat3brand = (decimal?)value;
                    break;
                case "Category3No":
                    mycat3no = (decimal?)value;
                    break;
                case "Category4Brand":
                    mycat4brand = (decimal?)value;
                    break;
                case "Category4No":
                    mycat4no = (decimal?)value;
                    break;
                case "Category5Brand":
                    mycat5brand = (decimal?)value;
                    break;
                case "Category5No":
                    mycat5no = (decimal?)value;
                    break;
                case "RateVat":
                    myratevat = (decimal)value;
                    break;
                case "RatePer":
                    myrateper = (decimal?)value;
                    break;
                case "RateAdd":
                    myrateadd = (decimal?)value;
                    break;
                case "RatePer2009":
                    myrateper2009 = (decimal?)value;
                    break;
                case "RateAdd2009":
                    myrateadd2009 = (decimal?)value;
                    break;
                case "RateDate":
                    myratedate = (DateTime)value;
                    break;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            throw new NotImplementedException();
        }
    }

    internal class AllPriceDBM : lib.DBManager<AllPrice>
    {
        internal AllPriceDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = false;
            SelectCommandText= "SELECT id,code,name,spelling,composition,translation,unit,addper,cat1brand,cat1no,cat2brand,cat2no,cat3brand,cat3no,cat4brand,cat4no,cat5brand,cat5no,ratevat,rateper,rateadd,ratedate,rateper2009,rateadd2009,risk,note,Convert(bigint,stamp),updatewhen,updatewho FROM spec.AllPrice_tb";
            base.SelectParams = new SqlParameter[] {};

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);
            SqlParameter parstamp = new SqlParameter("@stamp", System.Data.SqlDbType.BigInt); parstamp.Direction = System.Data.ParameterDirection.InputOutput;
            SqlParameter parupdatewhen = new SqlParameter("@updatewhen", System.Data.SqlDbType.DateTime2); parupdatewhen.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parupdatewho = new SqlParameter("@updatewho", System.Data.SqlDbType.NVarChar, 20); parupdatewho.Direction = System.Data.ParameterDirection.Output;

            myinsertparams = new SqlParameter[] { paridout };
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@code", System.Data.SqlDbType.NChar,10),
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,1000),
                new SqlParameter("@spelling", System.Data.SqlDbType.NVarChar,250),
                new SqlParameter("@composition", System.Data.SqlDbType.NVarChar,250),
                new SqlParameter("@translation", System.Data.SqlDbType.NVarChar,1000),
                new SqlParameter("@unit", System.Data.SqlDbType.NVarChar,3),
                new SqlParameter("@risk", System.Data.SqlDbType.NVarChar,250),
                new SqlParameter("@note", System.Data.SqlDbType.NVarChar,250),
                new SqlParameter("@addper", System.Data.SqlDbType.Money),
                new SqlParameter("@cat1brand", System.Data.SqlDbType.Money),
                new SqlParameter("@cat2brand", System.Data.SqlDbType.Money),
                new SqlParameter("@cat3brand", System.Data.SqlDbType.Money),
                new SqlParameter("@cat4brand", System.Data.SqlDbType.Money),
                new SqlParameter("@cat5brand", System.Data.SqlDbType.Money),
                new SqlParameter("@cat1no", System.Data.SqlDbType.Money),
                new SqlParameter("@cat2no", System.Data.SqlDbType.Money),
                new SqlParameter("@cat3no", System.Data.SqlDbType.Money),
                new SqlParameter("@cat4no", System.Data.SqlDbType.Money),
                new SqlParameter("@cat5no", System.Data.SqlDbType.Money),
                new SqlParameter("@ratevat", System.Data.SqlDbType.Money),
                new SqlParameter("@rateper", System.Data.SqlDbType.Money),
                new SqlParameter("@rateadd", System.Data.SqlDbType.Money),
                new SqlParameter("@rateper2009", System.Data.SqlDbType.Money),
                new SqlParameter("@rateadd2009", System.Data.SqlDbType.Money),
                new SqlParameter("@ratedate", System.Data.SqlDbType.DateTime2),
                parstamp, parupdatewhen, parupdatewho
            };
            mydeleteparams = new SqlParameter[] { parid };

            InsertProcedure = true;
            myinsertcommandtext = "spec.AllPriceAdd_sp";
            UpdateProcedure = true;
            myupdatecommandtext = "spec.AllPriceUpd_sp";
            DeleteProcedure = true;
            mydeletecommandtext = "spec.AllPriceDel_sp";
        }

        protected override AllPrice CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new AllPrice(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                reader.IsDBNull(25) ? string.Empty : reader.GetString(25),
                reader.IsDBNull(7) ? (decimal?)null : reader.GetDecimal(7),
                reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                reader.IsDBNull(9) ? (decimal?)null : reader.GetDecimal(9),
                reader.IsDBNull(10) ? (decimal?)null : reader.GetDecimal(10),
                reader.IsDBNull(11) ? (decimal?)null : reader.GetDecimal(11),
                reader.IsDBNull(12) ? (decimal?)null : reader.GetDecimal(12),
                reader.IsDBNull(13) ? (decimal?)null : reader.GetDecimal(13),
                reader.IsDBNull(14) ? (decimal?)null : reader.GetDecimal(14),
                reader.IsDBNull(15) ? (decimal?)null : reader.GetDecimal(15),
                reader.IsDBNull(16) ? (decimal?)null : reader.GetDecimal(16),
                reader.IsDBNull(17) ? (decimal?)null : reader.GetDecimal(17),
                reader.IsDBNull(18) ? (decimal?)null : reader.GetDecimal(18),
                reader.IsDBNull(19) ? (decimal?)null : reader.GetDecimal(19),
                reader.IsDBNull(20) ? (decimal?)null : reader.GetDecimal(20),
                reader.GetDateTime(21),
                reader.IsDBNull(22) ? (decimal?)null : reader.GetDecimal(22),
                reader.IsDBNull(23) ? (decimal?)null : reader.GetDecimal(23),
                reader.GetInt64(26),
                reader.GetDateTime(27),
                reader.IsDBNull(28) ? string.Empty : reader.GetString(28),
                lib.DomainObjectState.Unchanged);
        }
        protected override void GetOutputParametersValue(AllPrice item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
                item.Stamp = (Int64)myinsertupdateparams[25].Value;
            }
            else if (item.DomainState == lib.DomainObjectState.Modified)
            {
                item.Stamp = (Int64)myinsertupdateparams[25].Value;
                item.UpdateWhen = (DateTime)myinsertupdateparams[26].Value;
            }
        }
        protected override bool SaveChildObjects(AllPrice item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(AllPrice item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(AllPrice item)
        {
            myupdateparams[0].Value = item.Id;
            myinsertupdateparams[0].Value = item.Code;
            myinsertupdateparams[1].Value = item.Name;
            myinsertupdateparams[2].Value = item.Spelling!=null?item.Spelling : (object)DBNull.Value;
            myinsertupdateparams[3].Value = item.Composition;
            myinsertupdateparams[4].Value = item.Translation;
            myinsertupdateparams[5].Value = item.Unit!=null?item.Unit : (object)DBNull.Value;
            myinsertupdateparams[6].Value = item.Risk!=null?item.Risk : (object)DBNull.Value;
            myinsertupdateparams[7].Value = item.Note!=null?item.Note : (object)DBNull.Value;
            myinsertupdateparams[8].Value = item.AddPer.HasValue ? item.AddPer.Value : (object)DBNull.Value;
            myinsertupdateparams[9].Value = item.Category1Brand.HasValue ? item.Category1Brand : (object)DBNull.Value;
            myinsertupdateparams[10].Value = item.Category2Brand.HasValue ? item.Category2Brand : (object)DBNull.Value;
            myinsertupdateparams[11].Value = item.Category3Brand.HasValue ? item.Category3Brand : (object)DBNull.Value;
            myinsertupdateparams[12].Value = item.Category4Brand.HasValue ? item.Category4Brand : (object)DBNull.Value;
            myinsertupdateparams[13].Value = item.Category5Brand.HasValue ? item.Category5Brand : (object)DBNull.Value;
            myinsertupdateparams[14].Value = item.Category1No.HasValue ? item.Category1No : (object)DBNull.Value;
            myinsertupdateparams[15].Value = item.Category2No.HasValue ? item.Category2No : (object)DBNull.Value;
            myinsertupdateparams[16].Value = item.Category3No.HasValue ? item.Category3No : (object)DBNull.Value;
            myinsertupdateparams[17].Value = item.Category4No.HasValue ? item.Category4No : (object)DBNull.Value;
            myinsertupdateparams[18].Value = item.Category5No.HasValue ? item.Category5No : (object)DBNull.Value;
            myinsertupdateparams[19].Value = item.RateVat.HasValue ? item.RateVat.Value : (object)DBNull.Value;
            myinsertupdateparams[20].Value = item.RatePer.HasValue ? item.RatePer.Value : (object)DBNull.Value;
            myinsertupdateparams[21].Value = item.RateAdd.HasValue ? item.RateAdd.Value : (object)DBNull.Value;
            myinsertupdateparams[22].Value = item.RatePer2009.HasValue ? item.RatePer2009.Value : (object)DBNull.Value;
            myinsertupdateparams[23].Value = item.RateAdd2009.HasValue ? item.RateAdd2009.Value : (object)DBNull.Value;
            myinsertupdateparams[24].Value = item.RateDate;
            myinsertupdateparams[25].Value = item.Stamp;
            mydeleteparams[0].Value = item.Id;
            return true;
        }
        protected override void ItemAcceptChanches(AllPrice item)
        {
            item.AcceptChanches();
        }
        protected override void SetSelectParametersValue()
        {
        }
        protected override void LoadObjects(AllPrice item)
        {
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class AllPriceVM : lib.ViewModelErrorNotifyItem<AllPrice>
    {
        public AllPriceVM() : this(new AllPrice()) { }
        public AllPriceVM(AllPrice rental) : base(rental)
        {
            ValidetingProperties.AddRange(new string[] { "Code", "Name" });
            InitProperties();
        }

        public new int? Id
        { get { return this.DomainObject.Id < 1 ? (int?)null : this.DomainObject.Id; } }
        private string mycode;
        public string Code
        {
            set
            {
                if (!string.Equals(mycode, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Code";
                    mycode = value;
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Code);
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name;
                        base.DomainObject.Code = value;
                        ClearErrorMessageForProperty(name);
                    }
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? mycode : null; }
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
        public string Spelling
        {
            set
            {
                if (!string.Equals(base.DomainObject.Spelling, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Spelling";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Spelling);
                    ChangingDomainProperty = name;
                    base.DomainObject.Spelling = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Spelling : null; }
        }
        public string Translation
        {
            set
            {
                if (!string.Equals(base.DomainObject.Translation, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Translation";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Translation);
                    ChangingDomainProperty = name;
                    base.DomainObject.Translation = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Translation : null; }
        }
        public string Composition
        {
            set
            {
                if (!string.Equals(base.DomainObject.Composition, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Composition";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Composition);
                    ChangingDomainProperty = name;
                    base.DomainObject.Composition = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Composition : null; }
        }
        public string Unit
        {
            set
            {
                if (!string.Equals(base.DomainObject.Unit, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Unit";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Unit);
                    ChangingDomainProperty = name;
                    base.DomainObject.Unit = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Unit : null; }
        }
        public string Risk
        {
            set
            {
                if (!string.Equals(base.DomainObject.Risk, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Risk";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Risk);
                    ChangingDomainProperty = name;
                    base.DomainObject.Risk = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Risk : null; }
        }
        public string Note
        {
            set
            {
                if (!string.Equals(base.DomainObject.Note, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Note";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Note);
                    ChangingDomainProperty = name;
                    base.DomainObject.Note = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Note : null; }
        }
        public DateTime RateDate
        {
            set
            {
                if (!DateTime.Equals(base.DomainObject.RateDate, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RateDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RateDate);
                    ChangingDomainProperty = name;
                    base.DomainObject.RateDate = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RateDate : DateTime.MinValue; }
        }
        public decimal? AddPer
        {
            set
            {
                if (base.DomainObject.AddPer.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(decimal.Multiply(base.DomainObject.AddPer.Value, 100M), value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "AddPer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.AddPer);
                    ChangingDomainProperty = name;
                    base.DomainObject.AddPer = value.HasValue?decimal.Divide((decimal)value.Value, 100M):value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.AddPer.HasValue? decimal.Multiply(base.DomainObject.AddPer.Value, 100M): base.DomainObject.AddPer : null; }
        }
        public decimal? Category1Brand
        {
            set
            {
                if (base.DomainObject.Category1Brand.HasValue!=value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category1Brand.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category1Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category1Brand);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category1Brand = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category1Brand : null; }
        }
        public decimal? Category2Brand
        {
            set
            {
                if (base.DomainObject.Category2Brand.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category2Brand.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category3Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category2Brand);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category2Brand = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category2Brand : null; }
        }
        public decimal? Category3Brand
        {
            set
            {
                if (base.DomainObject.Category3Brand.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category3Brand.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category3Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category3Brand);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category3Brand = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category3Brand : null; }
        }
        public decimal? Category4Brand
        {
            set
            {
                if (base.DomainObject.Category4Brand.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category4Brand.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category4Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category4Brand);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category4Brand = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category4Brand : null; }
        }
        public decimal? Category5Brand
        {
            set
            {
                if (base.DomainObject.Category5Brand.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category5Brand.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category5Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category5Brand);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category5Brand = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category5Brand : null; }
        }
        public decimal? Category1No
        {
            set
            {
                if (base.DomainObject.Category1No.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category1No.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category1No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category1No);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category1No = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category1No : null; }
        }
        public decimal? Category2No
        {
            set
            {
                if (base.DomainObject.Category2No.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category2No.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category2No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category2No);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category2No = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category2No : null; }
        }
        public decimal? Category3No
        {
            set
            {
                if (base.DomainObject.Category3No.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category3No.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category3No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category3No);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category3No = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category3No : null; }
        }
        public decimal? Category4No
        {
            set
            {
                if (base.DomainObject.Category4No.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category4No.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category4No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category4No);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category4No = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category4No : null; }
        }
        public decimal? Category5No
        {
            set
            {
                if (base.DomainObject.Category5No.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.Category5No.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Category5No";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Category5No);
                    ChangingDomainProperty = name;
                    base.DomainObject.Category5No = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Category5No : null; }
        }
        public decimal? RateVat
        {
            set
            {
                if ((base.DomainObject.RateVat.HasValue!=value.HasValue || (value.HasValue && !decimal.Equals(decimal.Multiply(base.DomainObject.RateVat.Value, 100M), value.Value))) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RateVat";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RateVat);
                    ChangingDomainProperty = name;
                    base.DomainObject.RateVat = value.HasValue?decimal.Divide((decimal)value.Value, 100M): value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? (base.DomainObject.RateVat.HasValue ? decimal.Multiply(base.DomainObject.RateVat.Value, 100M): base.DomainObject.RateVat) : null; }
        }
        public decimal? RatePer
        {
            set
            {
                if (base.DomainObject.RatePer.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(decimal.Multiply(base.DomainObject.RatePer.Value, 100M), value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RatePer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RatePer);
                    ChangingDomainProperty = name;
                    base.DomainObject.RatePer = value.HasValue ? decimal.Divide((decimal)value.Value, 100M) : value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RatePer.HasValue? decimal.Multiply(base.DomainObject.RatePer.Value, 100M): base.DomainObject.RatePer : null; }
        }
        public decimal? RateAdd
        {
            set
            {
                if (base.DomainObject.RateAdd.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.RateAdd.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RateAdd";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RateAdd);
                    ChangingDomainProperty = name;
                    base.DomainObject.RateAdd = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RateAdd : null; }
        }
        public decimal? RatePer2009
        {
            set
            {
                if (base.DomainObject.RatePer2009.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(decimal.Multiply(base.DomainObject.RatePer2009.Value, 100M), value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RatePer2009";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RatePer2009);
                    ChangingDomainProperty = name;
                    base.DomainObject.RatePer2009 = value.HasValue ? decimal.Divide((decimal)value.Value, 100M) : value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RatePer2009.HasValue?decimal.Multiply(base.DomainObject.RatePer2009.Value, 100M): base.DomainObject.RatePer2009 : null; }
        }
        public decimal? RateAdd2009
        {
            set
            {
                if (base.DomainObject.RateAdd2009.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.RateAdd2009.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RateAdd2009";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RateAdd2009);
                    ChangingDomainProperty = name;
                    base.DomainObject.RateAdd2009 = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RateAdd2009 : null; }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Code":
                    mycode = this.DomainObject.Code;
                    break;
                case "Name":
                    myname = this.DomainObject.Name;
                    break;
            }
        }
        protected override void InitProperties()
        {
            mycode = this.DomainObject.Code;
            myname = this.DomainObject.Name;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Code":
                    if (mycode != this.DomainObject.Code)
                        mycode = this.DomainObject.Code;
                    else
                        this.Code = (string)value;
                    break;
                case "Name":
                    if (myname != this.DomainObject.Name)
                        myname = this.DomainObject.Name;
                    else
                        this.Name = (string)value;
                    break;
                case "Spelling":
                    this.DomainObject.Spelling = (string)value;
                    break;
                case "Translation":
                    this.DomainObject.Translation = (string)value;
                    break;
                case "Composition":
                    this.DomainObject.Composition = (string)value;
                    break;
                case "Unit":
                    this.DomainObject.Unit = (string)value;
                    break;
                case "Risk":
                    this.DomainObject.Risk = (string)value;
                    break;
                case "Note":
                    this.DomainObject.Note = (string)value;
                    break;
                case "RateDate":
                    this.DomainObject.RateDate = (DateTime)value;
                    break;
                case "AddPer":
                    this.DomainObject.AddPer = (decimal?)value;
                    break;
                case "Category1Brand":
                    this.DomainObject.Category1Brand = (decimal?)value;
                    break;
                case "Category2Brand":
                    this.DomainObject.Category2Brand = (decimal?)value;
                    break;
                case "Category3Brand":
                    this.DomainObject.Category3Brand = (decimal?)value;
                    break;
                case "Category4Brand":
                    this.DomainObject.Category4Brand = (decimal?)value;
                    break;
                case "Category5Brand":
                    this.DomainObject.Category5Brand = (decimal?)value;
                    break;
                case "Category1No":
                    this.DomainObject.Category1No = (decimal?)value;
                    break;
                case "Category2No":
                    this.DomainObject.Category2No = (decimal?)value;
                    break;
                case "Category3No":
                    this.DomainObject.Category3No = (decimal?)value;
                    break;
                case "Category4No":
                    this.DomainObject.Category4No = (decimal?)value;
                    break;
                case "Category5No":
                    this.DomainObject.Category5No = (decimal?)value;
                    break;
                case "RateVat":
                    this.DomainObject.RateVat = (decimal)value;
                    break;
                case "RatePer":
                    this.DomainObject.RatePer = (decimal?)value;
                    break;
                case "RateAdd":
                    this.DomainObject.RateAdd = (decimal?)value;
                    break;
                case "RatePer2009":
                    this.DomainObject.RatePer2009 = (decimal?)value;
                    break;
                case "RateAdd2009":
                    this.DomainObject.RateAdd2009 = (decimal?)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Code":
                    if (string.IsNullOrEmpty(mycode))
                    {
                        errmsg = "Отсутствует код ТНВЭД";
                        isvalid = false;
                    }
                    break;
                case "Name":
                    if (string.IsNullOrEmpty(myname))
                    {
                        errmsg = "Отсутствует описание";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return mycode!= base.DomainObject.Code || myname!= base.DomainObject.Name;
        }
    }

    internal class AllPriceSynchronizer : lib.ModelViewCollectionsSynchronizer<AllPrice, AllPriceVM>
    {
        protected override AllPrice UnWrap(AllPriceVM wrap)
        {
            return wrap.DomainObject as AllPrice;
        }

        protected override AllPriceVM Wrap(AllPrice fill)
        {
            return new AllPriceVM(fill);
        }
    }

    public class AllPriceCommand : lib.ViewModelBaseCommand
    {
        internal AllPriceCommand(AllPriceVM item, ListCollectionView view)
        {
            myitem = item;
            mydbm = new AllPriceDBM();
            myview = view;
            myexhandler = new DataModelClassLibrary.ExceptionHandler("Сохранение изменений");
        }

        private AllPriceVM myitem;
        public AllPriceVM Item
        {
            set { myitem = value; }
            get { return myitem; }
        }
        private AllPriceDBM mydbm;
        private ListCollectionView myview;
        private lib.ExceptionHandler myexhandler;
        public lib.ReferenceCollectionSimpleItem Units
        {
            get { return KirillPolyanskiy.CustomBrokerWpf.References.Units; }
        }

        public override bool SaveDataChanges()
        {
            mydbm.Errors.Clear();
            bool isSuccess = myitem.Validate(true);
            if (isSuccess)
                isSuccess = mydbm.SaveItemChanches(myitem.DomainObject);
            if (mydbm.Errors.Count > 0)
            {
                myexhandler.Handle(new Exception(mydbm.ErrorMessage));
                myexhandler.ShowMessage();
            }
            return isSuccess;
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
            return myview!=null;
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
            myview.EditItem(myitem);
            if(myitem.DomainState==lib.DomainObjectState.Added)
                myview.Remove(myitem);
            myitem.DomainState = lib.DomainObjectState.Deleted;
            myview.CommitEdit();
        }
        protected override void RefreshData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override void RejectChanges(object parametr)
        {
            myview.EditItem(myitem);
            myitem.RejectChanges();
            myview.CommitEdit();
            if (myitem.DomainState == lib.DomainObjectState.Destroyed)
                myview.Remove(myitem);
        }
    }

    public class AllPriceViewCommand : lib.ViewModelViewCommand, IDisposable
    {
        internal AllPriceViewCommand()
        {
            mydbm = new AllPriceDBM();
            mydbm.Fill();
            mysync = new AllPriceSynchronizer();
            mysync.DomainCollection = mydbm.Collection;
            base.Collection = mysync.ViewModelCollection;

            myexhandler = new DataModelClassLibrary.ExceptionHandler("Сохранение");
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            myexcelimport = new RelayCommand(ExcelImportExec, ExcelImportCanExec);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
        }

        private AllPriceSynchronizer mysync;
        private new AllPriceDBM mydbm;
        private lib.ExceptionHandler myexhandler;
        private System.ComponentModel.BackgroundWorker mybw;
        private ExcelImportWin myExcelImportWin;

        //private System.Windows.Data.ListCollectionView myunits;
        //public System.Windows.Data.ListCollectionView Units
        //{
        //    get
        //    {
        //        if(myunits==null)
        //        {
        //            myunits = new System.Windows.Data.ListCollectionView(KirillPolyanskiy.CustomBrokerWpf.References.Units);
        //            //myunits.Filter = delegate (object item) { return (item as lib.ReferenceSimpleItem).IsActual; };
        //        }
        //        return myunits;
        //    }
        //}
        public lib.ReferenceCollectionSimpleItem Units
        {
            get { return KirillPolyanskiy.CustomBrokerWpf.References.Units; }
        }

        private string myfiltercode;
        public string FilterCode
        {
            set
            {
                myfiltercode = value;
                PropertyChangedNotification("FilterCode");
            }
            get { return myfiltercode; }
        }
        private string myfiltername;
        public string FilterName
        {
            set
            {
                myfiltername = value;
                PropertyChangedNotification("FilterName");
            }
            get { return myfiltername; }
        }

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            myview.Filter = OnFilter;
        }
        private bool FilterRunCanExec(object parametr)
        { return true; }
        private bool OnFilter(object item)
        {
            bool where = lib.ViewModelViewCommand.ViewFilterDefault(item);
            string[] str;
            AllPriceVM price = item as AllPriceVM;
            if (where & !string.IsNullOrEmpty(myfiltercode))
            {
                where = where & price.Code.ToLower().IndexOf(myfiltercode) > -1;
            }
            if (where & !string.IsNullOrEmpty(myfiltername))
            {
                str = myfiltername.Trim().ToLower().Split(' ');
                foreach (string nameitem in str)
                    where = where & price.Name.ToLower().IndexOf(nameitem) > -1;
            }
            return where;
        }

        private RelayCommand myfilterclear;
        public ICommand FilterClear
        {
            get { return myfilterclear; }
        }
        private void FilterClearExec(object parametr)
        {
            FilterCode = string.Empty;
            FilterName = string.Empty;
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }

        private RelayCommand myexcelimport;
        public ICommand ExcelImport
        {
            get { return myexcelimport; }
        }
        private void ExcelImportExec(object parametr)
        {
            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
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
                    if (myExcelImportWin != null && myExcelImportWin.IsVisible)
                    {
                        myExcelImportWin.MessageTextBlock.Text = string.Empty;
                        myExcelImportWin.ProgressBar1.Value = 0;
                    }
                    else
                    {
                        myExcelImportWin = new ExcelImportWin();
                        myExcelImportWin.Show();
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
                if (myExcelImportWin != null && myExcelImportWin.IsVisible)
                {
                    myExcelImportWin.MessageTextBlock.Text = string.Empty;
                    myExcelImportWin.ProgressBar1.Value = 0;
                }
                else
                {
                    myExcelImportWin = new ExcelImportWin();
                    myExcelImportWin.Show();
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

        public void Dispose()
        {
        }

        public override bool SaveDataChanges()
        {
            this.PopupText = "Изменения сохранены";
            bool isSuccess = !(myview.CurrentItem is AllPriceVM) || (myview.CurrentItem as AllPriceVM).Validate(true);
            if (isSuccess)
            {
                mydbm.Errors.Clear();
                isSuccess = mydbm.SaveCollectionChanches();
            }
            if (mydbm.Errors.Count > 0)
            {
                myexhandler.Handle(new Exception(mydbm.ErrorMessage));
                myexhandler.ShowMessage();
            }
            return isSuccess;
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
        protected override void OtherViewRefresh() {}
        protected override void RefreshData(object parametr)
        {
            mydbm.Collection.Clear();
            mydbm.Fill();
        }
        protected override void RejectChanges(object parametr)
        {
            List<AllPriceVM> destroied = new List<AllPriceVM>();
            foreach (AllPriceVM item in mysync.ViewModelCollection)
            {
                if (item.DomainState == lib.DomainObjectState.Added)
                    destroied.Add(item);
                else if (item.DomainState != lib.DomainObjectState.Unchanged)
                {
                    base.myview.EditItem(item);
                    item.RejectChanges();
                    base.myview.CommitEdit();
                }
            }
            foreach (AllPriceVM item in destroied) mysync.ViewModelCollection.Remove(item);
        }
        protected override void SettingView()
        {
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;
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
                myExcelImportWin.MessageTextBlock.Text = "Обработка прервана из-за ошибки" + "\n" + e.Error.Message;
            }
            else
            {
                myExcelImportWin.MessageTextBlock.Text = "Обработка выполнена успешно." + "\n" + e.Result.ToString() + " строк обработано";
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            myExcelImportWin.ProgressBar1.Value = e.ProgressPercentage;
        }

        private int OnExcelImport(BackgroundWorker worker, Excel.Application exApp, string filepath, bool ismiss)
        {
            int maxr, id = 0;
            DateTime d;
            decimal m;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            AllPriceVM newitem = null;

            Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
            Excel.Worksheet exWh = exWb.Sheets[1];
            // Задать форматы столбцов
            maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            for (int r = 4; r <= maxr; r++)
            {
                if (string.IsNullOrEmpty((exWh.Cells[r, 1].Text as string).Trim())) continue;
                newitem = null;
                if (int.TryParse((exWh.Cells[r, 26].Text as string).Trim(), out id))
                    foreach (AllPriceVM item in mysync.ViewModelCollection)
                        if (item.DomainObject.Id == id)
                        {
                            newitem = item;
                            break;
                        }
                if (newitem == null) newitem = new AllPriceVM();
                else if (ismiss) continue;

                str.Clear();
                str.Append((exWh.Cells[r, 1].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 1000)
                    throw new ApplicationException("Ячейки Excel " + exWh.Cells[r, 1].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Name = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 2].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 10)
                    throw new ApplicationException("Ячейки Excel " + exWh.Cells[r, 2].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Code = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 3].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 250)
                    throw new ApplicationException("Ячейки Excel " + exWh.Cells[r, 3].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Spelling = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 4].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 250)
                    throw new ApplicationException("Ячейки Excel " + exWh.Cells[r, 4].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Composition = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 5].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 1000)
                    throw new ApplicationException("Ячейки Excel " + exWh.Cells[r, 5].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Translation = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 6].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 3)
                    throw new ApplicationException("Ячейки Excel " + exWh.Cells[r, 6].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Unit = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 7].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.AddPer = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 8].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category1Brand = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 9].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category1No = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 10].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category3Brand = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 11].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category3No = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 12].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category4Brand = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 13].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category4No = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 14].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category5Brand = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 15].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category5No = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 16].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category2Brand = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 17].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.Category2No = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 18].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.RateVat = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 19].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.RatePer = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 20].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.RateAdd = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 21].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (DateTime.TryParseExact(str.ToString(), "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                        newitem.RateDate = d;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как дату!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 22].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.RatePer2009 = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 23].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.RateAdd2009 = m;
                    else
                        throw new ApplicationException("Не удалось разобрать значение ячейки Excel " + exWh.Cells[r, 7].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 24].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 250)
                    throw new ApplicationException("Ячейки Excel " + exWh.Cells[r, 8].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Risk = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 25].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 250)
                    throw new ApplicationException("Ячейки Excel " + exWh.Cells[r, 9].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Note = str.ToString();

                if (!mysync.ViewModelCollection.Contains(newitem)) this.myview.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<AllPriceVM>(mysync.ViewModelCollection.Add), newitem);
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
                int row = 4;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet exWh = exWb.Sheets[1];
                Excel.Range r;

                exWh.Cells[3, 1] = "ОПИСАНИЕ"; exWh.Cells[3, 2] = "КОД ТНВЭД"; exWh.Cells[3, 3] = "НАПИСАНИЕ"; exWh.Cells[3, 4] = "СОСТАВ"; exWh.Cells[3, 5] = "ПЕРЕВОД";
                exWh.Cells[1, 6] = @"ЦЕНЫ, $              (Бренд +15% | не бренд +10% | Турция +18%) +0,01с";
                exWh.Cells[2, 6] = @"КГ
ШТ
М2";
                exWh.Cells[2, 7] = "+%";
                exWh.Cells[2, 8] = @"Филиалы 1 категории ЕС
Страны ЕС
Австралия, Австрия, Бельгия, Болгария, Великобитания, Великое Герцогство Люксембург, Венгрия, Германия, Греция, Дания, Ирландия, Испания, Италия, Канада, Латвия, Литва, Люксембург, Мальта, Нидерланды, Польша, Португалия, Республика Кипр, Румыния, Словакия, Словения, Соединенное Королевство Великобритании и Северной Ирландии, США, Финляндия, Франция, Хорватия, Чехия, Швейцария, Швеция, Эстония, Япония";
                exWh.Cells[3, 8] = "бренд"; exWh.Cells[3, 9] = "не бренд";
                exWh.Cells[2, 10] = @"Филиалы 3 категории EA
Страны Азии и Европы
Албания, Босния и Герцеговина, Израиль, Индонезия, Камбоджа, Македония, Малайзия, Монголия, Мьянма, Пакистан, Сербия, Таиланд, Филиппины, Шри-Ланка";
                exWh.Cells[3, 10] = "бренд"; exWh.Cells[3, 11] = "не бренд";
                exWh.Cells[2, 12] = @"Филиалы 4 категории AZ
Бангладеш, Вьетнам, Гонконг, Индия, Китай, Корея, Тайвань";
                exWh.Cells[3, 12] = "бренд"; exWh.Cells[3, 13] = "не бренд";
                exWh.Cells[2, 14] = @"Филиалы 5 категории AF
Страны Северной, Центральной и Южной Америки, Океании и Африки, Арктика и Антарктика
за исключением стран, перечисленных в др. Столбцах
Бразилия, Доминиканская республика, Египет, Маврикий, Мадагаскар, Макао, Марокко, Мауритиус, Мьянмар, Перу, Тунис, Уругвай, Эфиопия";
                exWh.Cells[3, 14] = "бренд"; exWh.Cells[3, 15] = "не бренд";
                exWh.Cells[2, 16] = @"ТУРЦИЯ TR
Турция";
                exWh.Cells[3, 16] = "бренд"; exWh.Cells[3, 17] = "не бренд";
                exWh.Cells[3, 18] = "НДС";
                exWh.Cells[1, 19] = "СТАВКИ"; exWh.Cells[2, 19] = "ПОСЛЕДНИЕ"; exWh.Cells[3, 19] = "%"; exWh.Cells[3, 20] = "ЕВРО"; exWh.Cells[3, 21] = "ДАТА";
                exWh.Cells[2, 22] = "ПЛАТЕЖ 2009"; exWh.Cells[3, 22] = "%"; exWh.Cells[3, 23] = "ЕВРО";
                exWh.Cells[3, 24] = "РИСКИ"; exWh.Cells[3, 25] = "ПРИМЕЧАНИЯ";

                exWh.Range[exWh.Cells[1, 1], exWh.Cells[2, 5]].Merge();
                exWh.Range[exWh.Cells[1, 6], exWh.Cells[1, 17]].Merge();
                //exWh.Range[exWh.Cells[1, 19], exWh.Cells[1, 23]].Merge();
                //exWh.Range[exWh.Cells[1, 24], exWh.Cells[2, 25]].Merge();

                r = exWh.Range[exWh.Cells[3, 1], exWh.Cells[3, 26]];
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].Weight = Excel.XlBorderWeight.xlMedium;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                r.VerticalAlignment = Excel.Constants.xlTop;
                r.WrapText = true;

                exWh.Columns[1, Type.Missing].NumberFormat = "@";
                exWh.Columns[2, Type.Missing].NumberFormat = "@";
                exWh.Columns[3, Type.Missing].NumberFormat = "@";
                exWh.Columns[4, Type.Missing].NumberFormat = "@";
                exWh.Columns[5, Type.Missing].NumberFormat = "@";
                exWh.Columns[6, Type.Missing].NumberFormat = "@";
                exWh.Columns[7, Type.Missing].NumberFormat = "@";
                exWh.Columns[8, Type.Missing].NumberFormat = "@";
                exWh.Columns[10, Type.Missing].NumberFormat = "@";
                exWh.Columns[11, Type.Missing].NumberFormat = "@";
                exWh.Columns[12, Type.Missing].NumberFormat = "@";
                exWh.Columns[13, Type.Missing].NumberFormat = "@";
                exWh.Columns[14, Type.Missing].NumberFormat = "@";
                exWh.Columns[15, Type.Missing].NumberFormat = "@";
                exWh.Columns[16, Type.Missing].NumberFormat = "@";
                exWh.Columns[17, Type.Missing].NumberFormat = "@";
                exWh.Columns[18, Type.Missing].NumberFormat = "@";
                exWh.Columns[19, Type.Missing].NumberFormat = "@";
                exWh.Columns[20, Type.Missing].NumberFormat = "@";
                exWh.Columns[21, Type.Missing].NumberFormat = @"mm.dd.yy";
                exWh.Columns[22, Type.Missing].NumberFormat = "@";
                exWh.Columns[23, Type.Missing].NumberFormat = "@";
                exWh.Columns[24, Type.Missing].NumberFormat = "@";
                exWh.Columns[25, Type.Missing].NumberFormat = "@";

                foreach (object itemobj in myview)
                {
                    if (!(itemobj is AllPriceVM)) continue;

                    AllPriceVM item = itemobj as AllPriceVM;
                    exWh.Cells[row, 1] = item.Name;
                    exWh.Cells[row, 2] = item.Code;
                    exWh.Cells[row, 3] = item.Spelling;
                    exWh.Cells[row, 4] = item.Composition;
                    exWh.Cells[row, 5] = item.Translation;
                    exWh.Cells[row, 6] = item.Unit;
                    exWh.Cells[row, 7] = item.AddPer.HasValue ? item.AddPer.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 8] = item.Category1Brand.HasValue ? item.Category1Brand.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 9] = item.Category1No.HasValue ? item.Category1No.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 10] = item.Category3Brand.HasValue ? item.Category3Brand.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 11] = item.Category3No.HasValue ? item.Category3No.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 12] = item.Category4Brand.HasValue ? item.Category4Brand.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 13] = item.Category4No.HasValue ? item.Category4No.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 14] = item.Category5Brand.HasValue ? item.Category5Brand.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 15] = item.Category5No.HasValue ? item.Category5No.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 16] = item.Category2Brand.HasValue ? item.Category2Brand.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 17] = item.Category2No.HasValue ? item.Category2No.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 18] = item.RateVat.HasValue?item.RateVat.Value.ToString("0.#####"):string.Empty;
                    exWh.Cells[row, 19] = item.RatePer.HasValue ? item.RatePer.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 20] = item.RateAdd.HasValue ? item.RateAdd.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 21] = item.RateDate.ToString("dd.MM.yy");
                    exWh.Cells[row, 22] = item.RatePer2009.HasValue ? item.RatePer2009.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 23] = item.RateAdd2009.HasValue ? item.RateAdd2009.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 24] = item.Risk;
                    exWh.Cells[row, 25] = item.Note;
                    exWh.Cells[row, 26] = item.Id;

                    row++;
                }

                exApp.Visible = true;
                exWh = null;
                return row - 4;
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
}
