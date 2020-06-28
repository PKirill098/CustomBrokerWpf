using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using System.Data.SqlClient;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class SpecificationDetail : lib.DomainBaseStamp
    {
        private SpecificationDetail(int id, long stamp, lib.DomainObjectState mstate,
            int? amount, string branch, string brand, string cellnumber, string certificate, string contexture, decimal? cost, string countryru, string countryen, string customer, string description, string gender, decimal? grossweight, decimal? netweight, string name, string note, string packing, decimal? price, string producer, int roworder, string sizeen, string sizeru, string tnved, string vendorcode
            ) : base(id, stamp, null, null, mstate)
        {
            myamount = amount;
            mybranch = branch;
            mybrand = brand;
            mycellnumber = cellnumber;
            mycertificate = certificate;
            mycontexture = contexture;
            mycost = cost;
            mycountryru = countryru;
            mycountryen = countryen;
            mycustomer = customer;
            mydescription = description;
            mygender = gender;
            mygrossweight = grossweight;
            mynetweight = netweight;
            myname = name;
            mynote = note;
            mypacking = packing;
            myprice = price;
            myproducer = producer;
            myroworder = roworder;
            mysizeen = sizeen;
            mysizeru = sizeru;
            mytnved = tnved;
            myvendorcode = vendorcode;
        }
        public SpecificationDetail(int id, long stamp, lib.DomainObjectState mstate,
            int? amount, string branch, string brand, string cellnumber, string certificate, CustomerLegal client, string contexture, decimal? cost, string countryru, string countryen, string customer, string description, string gender, decimal? grossweight, decimal? netweight, string name, string note, string packing, decimal? price, string producer, int roworder, string sizeen, string sizeru, Specification spec, string tnved, string vendorcode
            ):this(id, stamp, mstate,amount,branch,brand,cellnumber,certificate,contexture,cost,countryru,countryen,customer,description,gender,grossweight,netweight,name,note,packing,price,producer,roworder,sizeen,sizeru,tnved,vendorcode)
        {
            myclient = client;
            myspec = spec;
        }
        public SpecificationDetail() : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added
            , 0, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,0, null, null, null, null
            )
        { }

        private int? myamount;
        public int? Amount
        {
            set { SetProperty<int?>(ref myamount, value); }
            get { return myamount; }
        }
        private string mybranch;
        public string Branch
        {
            set { SetProperty<string>(ref mybranch, value); }
            get { return mybranch; }
        }
        private string mybrand;
        public string Brand
        {
            set { SetProperty<string>(ref mybrand, value); }
            get { return mybrand; }
        }
        private string mycellnumber;
        public string CellNumber
        {
            set { SetProperty<string>(ref mycellnumber, value); }
            get { return mycellnumber; }
        }
        private string mycertificate;
        public string Certificate
        {
            set { SetProperty<string>(ref mycertificate, value); }
            get { return mycertificate; }
        }
        private CustomerLegal myclient;
        public CustomerLegal Client
        {
            set { SetProperty<CustomerLegal>(ref myclient, value); }
            get { return myclient; }
        }
        private string mycontexture;
        public string Contexture
        {
            set { SetProperty<string>(ref mycontexture, value); }
            get { return mycontexture; }
        }
        private decimal? mycost;
        public decimal? Cost
        {
            set { SetProperty<decimal?>(ref mycost, value); }
            get { return mycost; }
        }
        private string mycountryru;
        public string CountryRU
        {
            set { SetProperty<string>(ref mycountryru, value); }
            get { return mycountryru; }
        }
        private string mycountryen;
        public string CountryEN
        {
            set { SetProperty<string>(ref mycountryen, value); }
            get { return mycountryen; }
        }
        private string mycustomer;
        public string Customer
        {
            set { SetProperty<string>(ref mycustomer, value); }
            get { return mycustomer; }
        }
        private string mydescription;
        public string Description
        {
            set { SetProperty<string>(ref mydescription, value); }
            get { return mydescription; }
        }
        private string mygender;
        public string Gender
        {
            set { SetProperty<string>(ref mygender, value); }
            get { return mygender; }
        }
        private decimal? mygrossweight;
        public decimal? GrossWeight
        {
            set { SetProperty<decimal?>(ref mygrossweight, value); }
            get { return mygrossweight; }
        }
        private string myname;
        public string Name
        {
            set { SetProperty<string>(ref myname, value); }
            get { return myname; }
        }
        private decimal? mynetweight;
        public decimal? NetWeight
        {
            set { SetProperty<decimal?>(ref mynetweight, value); }
            get { return mynetweight; }
        }
        private string mynote;
        public string Note
        {
            set { SetProperty<string>(ref mynote, value); }
            get { return mynote; }
        }
        private string mypacking;
        public string Packing
        {
            set { SetProperty<string>(ref mypacking, value); }
            get { return mypacking; }
        }
        private decimal? myprice;
        public decimal? Price
        {
            set { SetProperty<decimal?>(ref myprice, value); }
            get { return myprice; }
        }
        private string myproducer;
        public string Producer
        {
            set { SetProperty<string>(ref myproducer, value); }
            get { return myproducer; }
        }
        private int myroworder;
        public int RowOrder
        {
            set { SetProperty<int>(ref myroworder, value); }
            get { return myroworder; }
        }
        private string mysizeen;
        public string SizeEN
        {
            set { SetProperty<string>(ref mysizeen, value); }
            get { return mysizeen; }
        }
        private string mysizeru;
        public string SizeRU
        {
            set { SetProperty<string>(ref mysizeru, value); }
            get { return mysizeru; }
        }
        private Specification myspec;
        public Specification Specification
        {
            set { SetProperty<Specification>(ref myspec, value); }
            get { return myspec; }
        }
        private string mytnved;
        public string TNVED
        {
            set { SetProperty<string>(ref mytnved, value); }
            get { return mytnved; }
        }
        private string myvendorcode;
        public string VendorCode
        {
            set { SetProperty<string>(ref myvendorcode, value); }
            get { return myvendorcode; }
        }

        protected override void PropertiesUpdate(lib.DomainBaseReject templ)
        {
            SpecificationDetail sample = (SpecificationDetail)templ;
            this.Amount = sample.Amount;
            this.Branch = sample.Branch;
            this.Brand = sample.Brand;
            this.CellNumber = sample.CellNumber;
            this.Certificate = sample.Certificate;
            this.Client = sample.Client;
            this.Contexture = sample.Contexture;
            this.Cost = sample.Cost;
            this.CountryEN = sample.CountryEN;
            this.CountryRU = sample.CountryRU;
            this.Customer = sample.Customer;
            this.Description = sample.Description;
            this.Gender = sample.Gender;
            this.GrossWeight = sample.GrossWeight;
            this.Name = sample.Name;
            this.NetWeight = sample.NetWeight;
            this.Note = sample.Note;
            this.Packing = sample.Packing;
            this.Price = sample.Price;
            this.Producer = sample.Producer;
            this.RowOrder = sample.RowOrder;
            this.SizeEN = sample.SizeEN;
            this.SizeRU = sample.SizeRU;
            this.Specification = sample.Specification;
            this.TNVED = sample.TNVED;
            this.VendorCode = sample.VendorCode;
        }
        protected override void RejectProperty(string property, object value)
        {
        }
    }

    public class SpecificationDetailDBM : lib.DBManagerStamp<SpecificationDetail>
    {
        public SpecificationDetailDBM()
        {
            this.NeedAddConnection = true;
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "spec.SpecificationDetail_sp";
            InsertCommandText = "spec.SpecificationDetailAdd_sp";
            UpdateCommandText = "spec.SpecificationDetailUpd_sp";
            DeleteCommandText = "spec.SpecificationDetailDel_sp";

            SelectParams = new SqlParameter[] { new SqlParameter("@specid", System.Data.SqlDbType.Int), new SqlParameter("@filterid", System.Data.SqlDbType.Int) };
            InsertParams = new SqlParameter[] { InsertParams[0]
                , new SqlParameter("@specificationid", System.Data.SqlDbType.Int)
            };
            UpdateParams = new SqlParameter[] {UpdateParams[0]
                , new SqlParameter("@customeridtrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@amounttrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@branchtrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@brandtrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@cellnumbertrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@certificatetrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@contexturetrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@costtrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@countryrutrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@countryentrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@customertrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@descriptiontrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@gendertrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@grossweighttrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@netweighttrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@nametrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@notetrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@packingtrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@pricetrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@producertrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@rowordertrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@sizeentrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@sizerutrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@tnvedtrue", System.Data.SqlDbType.Bit)
                , new SqlParameter("@vendorcodetrue", System.Data.SqlDbType.Bit)
            };
            InsertUpdateParams = new SqlParameter[] {InsertUpdateParams[0]
                , new SqlParameter("@amount", System.Data.SqlDbType.Int)
                , new SqlParameter("@branch", System.Data.SqlDbType.NVarChar,100)
                , new SqlParameter("@brand", System.Data.SqlDbType.NVarChar,100)
                , new SqlParameter("@cellnumber", System.Data.SqlDbType.NVarChar,10)
                , new SqlParameter("@certificate", System.Data.SqlDbType.NVarChar,60)
                , new SqlParameter("@contexture", System.Data.SqlDbType.NVarChar,100)
                , new SqlParameter("@cost", System.Data.SqlDbType.Money)
                , new SqlParameter("@countryru", System.Data.SqlDbType.NVarChar,100)
                , new SqlParameter("@countryen", System.Data.SqlDbType.NVarChar,5)
                , new SqlParameter("@customer", System.Data.SqlDbType.NVarChar,50)
                , new SqlParameter("@customerid", System.Data.SqlDbType.Int)
                , new SqlParameter("@description", System.Data.SqlDbType.NVarChar,200)
                , new SqlParameter("@gender", System.Data.SqlDbType.NVarChar,10)
                , new SqlParameter("@grossweight", System.Data.SqlDbType.SmallMoney)
                , new SqlParameter("@netweight", System.Data.SqlDbType.SmallMoney)
                , new SqlParameter("@name", System.Data.SqlDbType.NVarChar,50)
                , new SqlParameter("@note", System.Data.SqlDbType.NVarChar,200)
                , new SqlParameter("@packing", System.Data.SqlDbType.NVarChar,50)
                , new SqlParameter("@price", System.Data.SqlDbType.Money)
                , new SqlParameter("@producer", System.Data.SqlDbType.NVarChar,100)
                , new SqlParameter("@roworder", System.Data.SqlDbType.Int)
                , new SqlParameter("@sizeen", System.Data.SqlDbType.NVarChar,10)
                , new SqlParameter("@sizeru", System.Data.SqlDbType.NVarChar,100)
                , new SqlParameter("@tnved", System.Data.SqlDbType.NVarChar,10)
                , new SqlParameter("@vendorcode", System.Data.SqlDbType.NVarChar,50)
            };
        }

        private Specification myspec;
        public Specification Specification
        { set { myspec = value; } get { return myspec; } }
        public SQLFilter Filter { set; get; }

        protected override SpecificationDetail CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
			return new SpecificationDetail(reader.GetInt32(0), reader.GetInt64(reader.GetOrdinal("stamp")), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(reader.GetOrdinal("amount")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("amount"))
                , reader.IsDBNull(reader.GetOrdinal("branch")) ? null : reader.GetString(reader.GetOrdinal("branch"))
                , reader.IsDBNull(reader.GetOrdinal("brand")) ? null : reader.GetString(reader.GetOrdinal("brand"))
                , reader.IsDBNull(reader.GetOrdinal("cellnumber")) ? null : reader.GetString(reader.GetOrdinal("cellnumber"))
                , reader.IsDBNull(reader.GetOrdinal("certificate")) ? null : reader.GetString(reader.GetOrdinal("certificate"))
                , CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(reader.IsDBNull(reader.GetOrdinal("customerid")) ? 0 : reader.GetInt32(reader.GetOrdinal("customerid")), addcon, out _)
                , reader.IsDBNull(reader.GetOrdinal("contexture")) ? null : reader.GetString(reader.GetOrdinal("contexture"))
                , reader.IsDBNull(reader.GetOrdinal("cost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("cost"))
                , reader.IsDBNull(reader.GetOrdinal("countryru")) ? null : reader.GetString(reader.GetOrdinal("countryru"))
                , reader.IsDBNull(reader.GetOrdinal("countryen")) ? null : reader.GetString(reader.GetOrdinal("countryen"))
                , reader.IsDBNull(reader.GetOrdinal("customer")) ? null : reader.GetString(reader.GetOrdinal("customer"))
                , reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description"))
                , reader.IsDBNull(reader.GetOrdinal("gender")) ? null : reader.GetString(reader.GetOrdinal("gender"))
                , reader.IsDBNull(reader.GetOrdinal("grossweight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("grossweight"))
                , reader.IsDBNull(reader.GetOrdinal("netweight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("netweight"))
                , reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name"))
                , reader.IsDBNull(reader.GetOrdinal("note")) ? null : reader.GetString(reader.GetOrdinal("note"))
                , reader.IsDBNull(reader.GetOrdinal("packing")) ? null : reader.GetString(reader.GetOrdinal("packing"))
                , reader.IsDBNull(reader.GetOrdinal("price")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("price"))
                , reader.IsDBNull(reader.GetOrdinal("producer")) ? null : reader.GetString(reader.GetOrdinal("producer"))
                , reader.GetInt32(reader.GetOrdinal("roworder"))
                , reader.IsDBNull(reader.GetOrdinal("sizeen")) ? null : reader.GetString(reader.GetOrdinal("sizeen"))
                , reader.IsDBNull(reader.GetOrdinal("sizeru")) ? null : reader.GetString(reader.GetOrdinal("sizeru"))
                , this.Specification ?? (reader.IsDBNull(reader.GetOrdinal("specificationid")) ? null : CustomBrokerWpf.References.SpecificationStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("specificationid")), addcon, out _))
                , reader.IsDBNull(reader.GetOrdinal("tnved")) ? null : reader.GetString(reader.GetOrdinal("tnved"))
                , reader.IsDBNull(reader.GetOrdinal("vendorcode")) ? null : reader.GetString(reader.GetOrdinal("vendorcode"))
            );
        }
        protected override void GetOutputSpecificParametersValue(SpecificationDetail item)
        {
        }
        protected override bool SaveChildObjects(SpecificationDetail item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(SpecificationDetail item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            SelectParams[0].Value = myspec?.Id;
            SelectParams[1].Value = this.Filter?.FilterWhereId;
        }
        protected override bool SetSpecificParametersValue(SpecificationDetail item)
        {
            InsertParams[1].Value = item.Specification.Id;
            foreach (SqlParameter par in UpdateParams)
            {
                switch (par.ParameterName)
                {
                    case "@amounttrue":
                        par.Value = item.HasPropertyOutdatedValue("Amount");
                        break;
                    case "@branchtrue":
                        par.Value = item.HasPropertyOutdatedValue("Branch");
                        break;
                    case "@brandtrue":
                        par.Value = item.HasPropertyOutdatedValue("Brand");
                        break;
                    case "@cellnumbertrue":
                        par.Value = item.HasPropertyOutdatedValue("CellNumber");
                        break;
                    case "@certificatetrue":
                        par.Value = item.HasPropertyOutdatedValue("Certificate");
                        break;
                    case "@contexturetrue":
                        par.Value = item.HasPropertyOutdatedValue("Contexture");
                        break;
                    case "@costtrue":
                        par.Value = item.HasPropertyOutdatedValue("Cost");
                        break;
                    case "@countryrutrue":
                        par.Value = item.HasPropertyOutdatedValue("CountryRU");
                        break;
                    case "@countryentrue":
                        par.Value = item.HasPropertyOutdatedValue("CountryEN");
                        break;
                    case "@customertrue":
                        par.Value = item.HasPropertyOutdatedValue("Customer");
                        break;
                    case "@customeridtrue":
                        par.Value = item.HasPropertyOutdatedValue("Client");
                        break;
                    case "@descriptiontrue":
                        par.Value = item.HasPropertyOutdatedValue("Description");
                        break;
                    case "@gendertruetrue":
                        par.Value = item.HasPropertyOutdatedValue("Gender");
                        break;
                    case "@grossweighttrue":
                        par.Value = item.HasPropertyOutdatedValue("GrossWeight");
                        break;
                    case "@netweighttrue":
                        par.Value = item.HasPropertyOutdatedValue("NetWeight");
                        break;
                    case "@nametrue":
                        par.Value = item.HasPropertyOutdatedValue("Name");
                        break;
                    case "@notetrue":
                        par.Value = item.HasPropertyOutdatedValue("Note");
                        break;
                    case "@packingtrue":
                        par.Value = item.HasPropertyOutdatedValue("Packing");
                        break;
                    case "@pricetrue":
                        par.Value = item.HasPropertyOutdatedValue("Price");
                        break;
                    case "@producertrue":
                        par.Value = item.HasPropertyOutdatedValue("Producer");
                        break;
                    case "@rowordertrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(SpecificationDetail.RowOrder));
                        break;
                    case "@sizeentrue":
                        par.Value = item.HasPropertyOutdatedValue("SizeEN");
                        break;
                    case "@sizerutrue":
                        par.Value = item.HasPropertyOutdatedValue("SizeRU");
                        break;
                    case "@tnvedtrue":
                        par.Value = item.HasPropertyOutdatedValue("TNVED");
                        break;
                    case "@vendorcodetrue":
                        par.Value = item.HasPropertyOutdatedValue("VendorCode");
                        break;
                }
            }
            foreach (SqlParameter par in InsertUpdateParams)
                switch (par.ParameterName)
                {
                    case "@amount":
                        par.Value = item.Amount;
                        break;
                    case "@branch":
                        par.Value = item.Branch;
                        break;
                    case "@brand":
                        par.Value = item.Brand;
                        break;
                    case "@cellnumber":
                        par.Value = item.CellNumber;
                        break;
                    case "@certificate":
                        par.Value = item.Certificate;
                        break;
                    case "@contexture":
                        par.Value = item.Contexture;
                        break;
                    case "@cost":
                        par.Value = item.Cost;
                        break;
                    case "@countryru":
                        par.Value = item.CountryRU;
                        break;
                    case "@countryen":
                        par.Value = item.CountryEN;
                        break;
                    case "@customer":
                        par.Value = item.Customer;
                        break;
                    case "@customerid":
                        par.Value = item.Client?.Id;
                        break;
                    case "@description":
                        par.Value = item.Description;
                        break;
                    case "@gender":
                        par.Value = item.Gender;
                        break;
                    case "@grossweight":
                        par.Value = item.GrossWeight;
                        break;
                    case "@netweight":
                        par.Value = item.NetWeight;
                        break;
                    case "@name":
                        par.Value = item.Name;
                        break;
                    case "@note":
                        par.Value = item.Note;
                        break;
                    case "@packing":
                        par.Value = item.Packing;
                        break;
                    case "@price":
                        par.Value = item.Price;
                        break;
                    case "@producer":
                        par.Value = item.Producer;
                        break;
                    case "@roworder":
                        par.Value = item.RowOrder;
                        break;
                    case "@sizeen":
                        par.Value = item.SizeEN;
                        break;
                    case "@sizeru":
                        par.Value = item.SizeRU;
                        break;
                    case "@tnved":
                        par.Value = item.TNVED;
                        break;
                    case "@vendorcode":
                        par.Value = item.VendorCode;
                        break;
                }
            if (!(item.Specification.Id > 0)) this.Errors.Add(new lib.DBMError(item, "Спецификация не сохранена", "specnew"));
            return item.Specification.Id > 0;
        }
        protected override bool LoadObjects()
        {
            return this.Errors.Count == 0;
        }
    }

    public class SpecificationDetailVM : lib.ViewModelErrorNotifyItem<SpecificationDetail>, lib.Interfaces.ISelectable
    {
        public SpecificationDetailVM(SpecificationDetail model) : base(model)
        {
            InitProperties();
            myfileopen = new RelayCommand(FileOpenExec, FileOpenCanExec);
        }

        public int? Amount
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Amount.HasValue != value.HasValue || (value.HasValue && this.DomainObject.Amount.Value != value.Value)))
                {
                    string name = "Amount";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Amount);
                    ChangingDomainProperty = name; this.DomainObject.Amount = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Amount : null; }
        }
        public string Branch
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Branch, value)))
                {
                    string name = "Branch";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Branch);
                    ChangingDomainProperty = name; this.DomainObject.Branch = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Branch : null; }
        }
        public string Brand
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Brand, value)))
                {
                    string name = "Brand";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Brand);
                    ChangingDomainProperty = name; this.DomainObject.Brand = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Brand : null; }
        }
        public string CellNumber
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CellNumber, value)))
                {
                    string name = "CellNumber";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CellNumber);
                    ChangingDomainProperty = name; this.DomainObject.CellNumber = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CellNumber : null; }
        }
        public string Certificate
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Certificate, value)))
                {
                    string name = "Certificate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Certificate);
                    ChangingDomainProperty = name; this.DomainObject.Certificate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Certificate : null; }
        }
        public CustomerLegal Client
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Client, value)))
                {
                    string name = "Client";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Client);
                    ChangingDomainProperty = name; this.DomainObject.Client = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Client : null; }
        }
        public string Contexture
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Contexture, value)))
                {
                    string name = "Contexture";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Contexture);
                    ChangingDomainProperty = name; this.DomainObject.Contexture = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Contexture : null; }
        }
        public decimal? Cost
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Cost.HasValue != value.HasValue || (value.HasValue && this.DomainObject.Cost.Value != value.Value)))
                {
                    string name = "Cost";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Cost);
                    ChangingDomainProperty = name; this.DomainObject.Cost = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Cost : null; }
        }
        public string CountryRU
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CountryRU, value)))
                {
                    string name = "CountryRU";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CountryRU);
                    ChangingDomainProperty = name; this.DomainObject.CountryRU = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CountryRU : null; }
        }
        public string CountryEN
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CountryEN, value)))
                {
                    string name = "CountryEN";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CountryEN);
                    ChangingDomainProperty = name; this.DomainObject.CountryEN = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CountryEN : null; }
        }
        public string Customer
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Customer, value)))
                {
                    string name = "Customer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Customer);
                    ChangingDomainProperty = name; this.DomainObject.Customer = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Customer : null; }
        }
        public string Description
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Description, value)))
                {
                    string name = "Description";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Description);
                    ChangingDomainProperty = name; this.DomainObject.Description = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Description : null; }
        }
        public string Gender
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Gender, value)))
                {
                    string name = "Gender";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Gender);
                    ChangingDomainProperty = name; this.DomainObject.Gender = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Gender : null; }
        }
        public decimal? GrossWeight
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.GrossWeight.HasValue != value.HasValue || (value.HasValue && this.DomainObject.GrossWeight.Value != value.Value)))
                {
                    string name = "GrossWeight";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GrossWeight);
                    ChangingDomainProperty = name; this.DomainObject.GrossWeight = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.GrossWeight : null; }
        }
        public string Name
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Name, value)))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Name);
                    ChangingDomainProperty = name; this.DomainObject.Name = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Name : null; }
        }
        public decimal? NetWeight
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.NetWeight.HasValue != value.HasValue || (value.HasValue && this.DomainObject.NetWeight.Value != value.Value)))
                {
                    string name = "NetWeight";
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
                    string name = "Note";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Note);
                    ChangingDomainProperty = name; this.DomainObject.Note = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Note : null; }
        }
        public string Packing
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Packing, value)))
                {
                    string name = "Packing";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Packing);
                    ChangingDomainProperty = name; this.DomainObject.Packing = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Packing : null; }
        }
        public decimal? Price
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Price.HasValue != value.HasValue || (value.HasValue && this.DomainObject.Price.Value != value.Value)))
                {
                    string name = "Price";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Price);
                    ChangingDomainProperty = name; this.DomainObject.Price = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Price : null; }
        }
        public string Producer
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Producer, value)))
                {
                    string name = "Producer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Producer);
                    ChangingDomainProperty = name; this.DomainObject.Producer = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Producer : null; }
        }
        public int? RowOrder
        { get { return this.IsEnabled ? this.DomainObject.RowOrder : (int?)null; } }
        private bool myselected;
        public bool Selected
        {
            set
            {
                bool oldvalue = myselected; myselected = value;
                this.OnValueChanged("Selected", (oldvalue ? 1M : 0M), (value ? 1M : 0M));
            }
            get { return myselected; }
        }
        public string SizeEN
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.SizeEN, value)))
                {
                    string name = "SizeEN";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SizeEN);
                    ChangingDomainProperty = name; this.DomainObject.SizeEN = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.SizeEN : null; }
        }
        public string SizeRU
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.SizeRU, value)))
                {
                    string name = "SizeRU";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.SizeRU);
                    ChangingDomainProperty = name; this.DomainObject.SizeRU = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.SizeRU : null; }
        }
        public Specification Specification
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.Specification, value)))
                {
                    string name = "Specification";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Specification);
                    ChangingDomainProperty = name; this.DomainObject.Specification = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Specification : null; }
        }
        public string TNVED
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.TNVED, value)))
                {
                    string name = "TNVED";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TNVED);
                    ChangingDomainProperty = name; this.DomainObject.TNVED = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TNVED : null; }
        }
        public string VendorCode
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.VendorCode, value)))
                {
                    string name = "VendorCode";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.VendorCode);
                    ChangingDomainProperty = name; this.DomainObject.VendorCode = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.VendorCode : null; }
        }

        private RelayCommand myfileopen;
        public ICommand FileOpen
        {
            get { return myfileopen; }
        }
        private void FileOpenExec(object parametr)
        {
            try
            {
                System.Diagnostics.Process.Start(System.IO.Path.Combine(CustomBrokerWpf.Properties.Settings.Default.DetailsFileRoot, this.Specification?.FilePath));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Не удалось открыть файл.\n" + ex.Message);
            }
        }
        private bool FileOpenCanExec(object parametr)
        { return !(this.IsReadOnly | string.IsNullOrEmpty(this.Specification?.FilePath)); }

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
                case "Client":
                    this.DomainObject.Client = (CustomerLegal)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            return true;
        }
    }

    public class SpecificationDetailSynchronizer : lib.ModelViewCollectionsSynchronizer<SpecificationDetail, SpecificationDetailVM>
    {
        protected override SpecificationDetail UnWrap(SpecificationDetailVM wrap)
        {
            return wrap.DomainObject as SpecificationDetail;
        }
        protected override SpecificationDetailVM Wrap(SpecificationDetail fill)
        {
            return new SpecificationDetailVM(fill);
        }
    }

    public class SpecificationDetailViewCommand : lib.ViewModelViewCommand
    {
        internal SpecificationDetailViewCommand()
        {
            myfilter = new SQLFilter("specdetail", "AND");
            mysddbm = new SpecificationDetailDBM() { Filter = myfilter };
            mydbm = mysddbm;
            mysddbm.Collection = new System.Collections.ObjectModel.ObservableCollection<SpecificationDetail>();
            mysddbm.FillAsyncCompleted = () => { if (mysddbm.Errors.Count > 0) OpenPopup(mysddbm.ErrorMessage, true); };
            mysync = new SpecificationDetailSynchronizer() { DomainCollection = mysddbm.Collection };
            base.Collection = mysync.ViewModelCollection;

            #region Filters
            myfilterrun = new RelayCommand(FilterRunExec, FilterRunCanExec);
            myfilterclear = new RelayCommand(FilterClearExec, FilterClearCanExec);
            mybranchfilter = new SpecificationDetailBranchFilter();
            mybranchfilter.DeferredFill = true;
            mybranchfilter.ItemsSource = myview.OfType<SpecificationDetailVM>();
            mybranchfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mybranchfilter.ExecCommand2 = () => { mybranchfilter.Clear(); };
            mybranchfilter.FillDefault = () =>
            {
                if (myfilter.isEmpty)
                    foreach (string item in mybranchfilter.DefaultList)
                        mybranchfilter.Items.Add(item);
                return myfilter.isEmpty;
            };
            mybrandfilter = new SpecificationDetailBrandFilter();
            mybrandfilter.DeferredFill = true;
            mybrandfilter.ItemsSource = myview.OfType<SpecificationDetailVM>();
            mybrandfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mybrandfilter.ExecCommand2 = () => { mybrandfilter.Clear(); };
            mybrandfilter.FillDefault = () =>
            {
                if (myfilter.isEmpty)
                    foreach (string item in mybrandfilter.DefaultList)
                        mybrandfilter.Items.Add(item);
                return myfilter.isEmpty;
            };
            myparcelfilter = new SpecificationDetailParcelNumberEntireFilter();
            myparcelfilter.DeferredFill = true;
            myparcelfilter.ItemsSource = myview.OfType<SpecificationDetailVM>();
            myparcelfilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Descending));
            myparcelfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myparcelfilter.ExecCommand2 = () => { myparcelfilter.Clear(); };
            myparcelfilter.FillDefault = () =>
            {
                if (myfilter.isEmpty)
                    foreach (Parcel item in myparcelfilter.DefaultList)
                        myparcelfilter.Items.Add(item);
                return myfilter.isEmpty;
            };
            mycertificatefilter = new SpecificationDetailCertificateFilter();
            mycertificatefilter.DeferredFill = true;
            mycertificatefilter.ItemsSource = myview.OfType<SpecificationDetailVM>();
            //mycertificatefilter.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Descending));
            mycertificatefilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycertificatefilter.ExecCommand2 = () => { mycertificatefilter.Clear(); };
            mycertificatefilter.FillDefault = () =>
            {
                if (myfilter.isEmpty)
                    foreach (string item in mycertificatefilter.DefaultList)
                        mycertificatefilter.Items.Add(item);
                return myfilter.isEmpty;
            };
            mycountryrufilter = new SpecificationDetailCountryRuFilter();
            mycountryrufilter.DeferredFill = true;
            mycountryrufilter.ItemsSource = myview.OfType<SpecificationDetailVM>();
            mycountryrufilter.ExecCommand1 = () => { FilterRunExec(null); };
            mycountryrufilter.ExecCommand2 = () => { mycountryrufilter.Clear(); };
            mycountryrufilter.FillDefault = () =>
            {
                if (myfilter.isEmpty)
                    foreach (CustomBrokerWpf.Domain.References.Country item in CustomBrokerWpf.References.Countries)
                        mycountryrufilter.Items.Add(item.Name);
                return myfilter.isEmpty;
            };

            myclientfilter = new SpecificationDetailClientFilter();
            myclientfilter.DeferredFill = true;
            myclientfilter.ItemsSource = myview.OfType<SpecificationDetailVM>();
            myclientfilter.ExecCommand1 = () => { FilterRunExec(null); };
            myclientfilter.ExecCommand2 = () => { myclientfilter.Clear(); };
            myclientfilter.FillDefault = () =>
            {
                if (myfilter.isEmpty)
                    foreach (lib.ReferenceSimpleItem item in CustomBrokerWpf.References.CustomersName)
                        myclientfilter.Items.Add(item.Name);
                return myfilter.isEmpty;
            };

            mygenderfilter = new libui.CheckListBoxVM();
            mygenderfilter.DisplayPath = "Name";
            mygenderfilter.GetDisplayPropertyValueFunc = (item) => { return ((Gender)item).Name; };
            mygenderfilter.SearchPath = "Name";
            mygenderfilter.Items = CustomBrokerWpf.References.Genders;
            mygenderfilter.ItemsViewFilterDefault = lib.ViewModelViewCommand.ViewFilterDefault;
            mygenderfilter.SelectedAll = false;
            mygenderfilter.ExecCommand1 = () => { FilterRunExec(null); };
            mygenderfilter.ExecCommand2 = () => { mygenderfilter.Clear(); };
            mygenderfilter.AreaFilterIsVisible = false;
            #endregion
            if (myfilter.isEmpty)
                this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);

            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
            myspecfolderopen = new RelayCommand(SpecFolderOpenExec, SpecFolderOpenCanExec);
        }

        private SpecificationDetailDBM mysddbm;
        private SpecificationDetailSynchronizer mysync;
        private SQLFilter myfilter;
        internal SQLFilter Filter
        { get { return myfilter; } }
        private SpecificationDetailBranchFilter mybranchfilter;
        public SpecificationDetailBranchFilter BranchFilter
        {
            get { return mybranchfilter; }
        }
        private SpecificationDetailBrandFilter mybrandfilter;
        public SpecificationDetailBrandFilter BrandFilter
        {
            get { return mybrandfilter; }
        }
        private SpecificationDetailCertificateFilter mycertificatefilter;
        public SpecificationDetailCertificateFilter CertificateFilter
        {
            get { return mycertificatefilter; }
        }
        private SpecificationDetailClientFilter myclientfilter;
        public SpecificationDetailClientFilter ClientFilter
        {
            get { return myclientfilter; }
        }
        private SpecificationDetailCountryRuFilter mycountryrufilter;
        public SpecificationDetailCountryRuFilter CountryRuFilter
        {
            get { return mycountryrufilter; }
        }
        private libui.CheckListBoxVM mygenderfilter;
        public libui.CheckListBoxVM GenderFilter
        { get { return mygenderfilter; } }
        private SpecificationDetailParcelNumberEntireFilter myparcelfilter;
        public SpecificationDetailParcelNumberEntireFilter ParcelFilter
        {
            get { return myparcelfilter; }
        }

        private RelayCommand myfilterrun;
        public ICommand FilterRun
        {
            get { return myfilterrun; }
        }
        private void FilterRunExec(object parametr)
        {
            this.EndEdit();
            if (mybranchfilter.FilterOn)
            {
                string[] items = new string[mybranchfilter.SelectedItems.Count];
                for (int i = 0; i < mybranchfilter.SelectedItems.Count; i++)
                    items[i] = (string)mybranchfilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "branch", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "branch", new string[0]);
            if (mybrandfilter.FilterOn)
            {
                string[] items = new string[mybrandfilter.SelectedItems.Count];
                for (int i = 0; i < mybrandfilter.SelectedItems.Count; i++)
                    items[i] = (string)mybrandfilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "brand", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "brand", new string[0]);
            if (mycertificatefilter.FilterOn)
            {
                string[] items = new string[mycertificatefilter.SelectedItems.Count];
                for (int i = 0; i < mycertificatefilter.SelectedItems.Count; i++)
                    items[i] = (string)mycertificatefilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "certificate", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "certificate", new string[0]);
            if (myclientfilter.FilterOn)
            {
                string[] items = new string[myclientfilter.SelectedItems.Count];
                for (int i = 0; i < myclientfilter.SelectedItems.Count; i++)
                    items[i] = (myclientfilter.SelectedItems[i] as Parcel).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "customer", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "customer", new string[0]);
            if (mycountryrufilter.FilterOn)
            {
                string[] items = new string[mycountryrufilter.SelectedItems.Count];
                for (int i = 0; i < mycountryrufilter.SelectedItems.Count; i++)
                    items[i] = (string)mycountryrufilter.SelectedItems[i];
                myfilter.SetList(myfilter.FilterWhereId, "countryru", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "countryru", new string[0]);
            if (mygenderfilter.FilterOn)
            {
                string[] items = new string[mygenderfilter.SelectedItems.Count];
                for (int i = 0; i < mygenderfilter.SelectedItems.Count; i++)
                    items[i] = (mygenderfilter.SelectedItems[i] as Gender).Name;
                myfilter.SetList(myfilter.FilterWhereId, "gender", items);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "gender", new string[0]);
            if (myparcelfilter.FilterOn)
            {
                string[] parcels = new string[myparcelfilter.SelectedItems.Count];
                for (int i = 0; i < myparcelfilter.SelectedItems.Count; i++)
                    parcels[i] = (myparcelfilter.SelectedItems[i] as Parcel).Id.ToString();
                myfilter.SetList(myfilter.FilterWhereId, "parcel", parcels);
            }
            else
                myfilter.SetList(myfilter.FilterWhereId, "parcel", new string[0]);
            //if (myservicetypefilter.FilterOn)
            //{
            //    bool isNullOrEmpty = false;
            //    string[] parcels = new string[myservicetypefilter.SelectedItems.Count];
            //    for (int i = 0; i < myservicetypefilter.SelectedItems.Count; i++)
            //    {
            //        parcels[i] = (myservicetypefilter.SelectedItems[i] as lib.ReferenceSimpleItem).Name;
            //        if (string.IsNullOrEmpty(parcels[i])) isNullOrEmpty = true;
            //    }
            //    myfilter.SetList(myservicetypefiltergroup, "servicetype", parcels);
            //    List<SQLFilterCondition> conds = myfilter.ConditionGet(myservicetypefiltergroup, "servicetype");
            //    if (isNullOrEmpty)
            //    { if (conds.Count == 1) myfilter.ConditionAdd(myservicetypefiltergroup, "servicetype", "IS NULL"); }
            //    else
            //        if (conds.Count > 1) myfilter.ConditionDel(myfilter.ConditionGet(myservicetypefiltergroup, "servicetype")[1].propertyid);
            //}
            //else
            //    foreach (SQLFilterCondition cond in myfilter.ConditionGet(myservicetypefiltergroup, "servicetype"))
            //        myfilter.ConditionDel(cond.propertyid);
            //myfilter.SetDate(myfilter.FilterWhereId, "shipmentdate", "shipmentdate", myshipmentdatefilter.DateStart, myshipmentdatefilter.DateStop);
            if (!(mybranchfilter.FilterOn | mybrandfilter.FilterOn | myparcelfilter.FilterOn | myclientfilter.FilterOn | mycountryrufilter.FilterOn | mygenderfilter.FilterOn | mycertificatefilter.FilterOn))
                this.OpenPopup("Фильтр. Пожалуйста, задайте критерии выбора грузов!", false);
            else
                mysddbm.FillAsync();
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
            mybranchfilter.Clear();
            mybranchfilter.IconVisibileChangedNotification();
            mybrandfilter.Clear();
            mybrandfilter.IconVisibileChangedNotification();
            myparcelfilter.Clear();
            myparcelfilter.IconVisibileChangedNotification();
            myclientfilter.Clear();
            myclientfilter.IconVisibileChangedNotification();
            mycountryrufilter.Clear();
            mycountryrufilter.IconVisibileChangedNotification();
            mygenderfilter.Clear();
            mygenderfilter.IconVisibileChangedNotification();
            mycertificatefilter.Clear();
            mycertificatefilter.IconVisibileChangedNotification();
            this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
        }
        private bool FilterClearCanExec(object parametr)
        { return true; }

        private RelayCommand myspecfolderopen;
        public ICommand SpecFolderOpen
        {
            get { return myspecfolderopen; }
        }
        private void SpecFolderOpenExec(object parametr)
        {
            try
            {
                string path = CustomBrokerWpf.Properties.Settings.Default.DetailsFileRoot;
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex)
            {
                this.OpenPopup("Папка документов\n" + ex.Message, true);
            }
        }
        private bool SpecFolderOpenCanExec(object parametr)
        { return true; }

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
                object[] columns = null;
                libui.PopUpWindow win = new libui.PopUpWindow();
                win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                Grid grid = new Grid();
                Grid.SetIsSharedSizeScope(grid, true);
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition() { Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Auto) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star), MinWidth = 20 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Auto), SharedSizeGroup = "b" });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star), MinWidth = 20 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Auto), SharedSizeGroup = "b" });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star), MinWidth = 20 });
                ListBox list = new ListBox();
                list.Style = list.FindResource("CheckBoxListStyle") as System.Windows.Style;
                list.DisplayMemberPath = "Header";
                DataGrid source = parametr as DataGrid;
                IOrderedEnumerable<DataGridColumn> sortcolumns = source.Columns.OrderBy<DataGridColumn, int>((DataGridColumn item) => { return item.DisplayIndex; });
                foreach (DataGridColumn column in sortcolumns) // safe order columns as datagrid in the window
                    list.Items.Add(new ColumnInfo() { Header = column.Header.ToString(), Property = (column.Header.ToString() != "Файл" ? column.SortMemberPath.Substring(column.SortMemberPath.LastIndexOf('.') + 1) : "FilePath"), Order = column.DisplayIndex });
                list.SelectAll();
                list.SetValue(Grid.ColumnSpanProperty, 5);
                list.Margin = new System.Windows.Thickness(2D, 2D, 2D, 10D);
                grid.Children.Add(list);
                Button bok = new Button() { Content = "OK", IsDefault = true };
                bok.Click += (object sender, System.Windows.RoutedEventArgs e) => { win.DialogResult = true; columns = new ColumnInfo[list.SelectedItems.Count]; list.SelectedItems.CopyTo(columns, 0); win.Close(); };
                bok.SetValue(Grid.RowProperty, 1);
                bok.SetValue(Grid.ColumnProperty, 1);
                bok.Margin = new System.Windows.Thickness(0D, 0D, 0D, 6D);
                grid.Children.Add(bok);
                Button besc = new Button() { Content = "Отмена", IsDefault = false };
                besc.Click += (object sender, System.Windows.RoutedEventArgs e) => { win.DialogResult = false; win.Close(); };
                besc.SetValue(Grid.RowProperty, 1);
                besc.SetValue(Grid.ColumnProperty, 3);
                besc.Margin = new System.Windows.Thickness(0D, 0D, 0D, 6D);
                grid.Children.Add(besc);
                win.Content = grid;
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
                    myexceltask.Run(new object[3] { columns, items, count });
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool ExcelExportCanExec(object parametr)
        { return !(myview.IsAddingNew | myview.IsEditingItem); }
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
                exWh.Name = "Разбивки";

                int maxrow = (int)(args as object[])[2] + 1;
                System.Collections.IEnumerable items = (args as object[])[1] as System.Collections.IEnumerable;
                IOrderedEnumerable<ColumnInfo> columns = ((args as object[])[0] as ColumnInfo[]).OrderBy<ColumnInfo, int>((ColumnInfo item) => { return item.Order; });
                exWh.Rows[1, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                foreach (ColumnInfo columninfo in columns)
                {
                    if (!string.IsNullOrEmpty(columninfo.Property))
                    {
                        exWh.Cells[1, column] = columninfo.Header;
                        switch (columninfo.Property)
                        {
                            case nameof(SpecificationDetailVM.Amount):
                            case nameof(SpecificationDetailVM.CellNumber):
                                exWh.Columns[column, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                                break;
                            case nameof(SpecificationVM.CFPR):
                            case nameof(SpecificationDetailVM.Gender):
                            case nameof(SpecificationDetailVM.VendorCode):
                            case nameof(SpecificationDetailVM.SizeEN):
                            case nameof(SpecificationDetailVM.TNVED):
                            case nameof(SpecificationDetailVM.CountryEN):
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                exWh.Columns[column, Type.Missing].HorizontalAlignment = Excel.Constants.xlCenter;
                                break;
                            case nameof(SpecificationVM.FilePath):
                            case nameof(Parcel.ParcelNumberEntire):
                            case nameof(SpecificationDetailVM.Contexture):
                                exWh.Columns[column, Type.Missing].NumberFormat = "@";
                                break;
                            case nameof(SpecificationDetailVM.NetWeight):
                            case nameof(SpecificationDetailVM.GrossWeight):
                            case nameof(SpecificationDetailVM.Price):
                            case nameof(SpecificationDetailVM.Cost):
                                try { exWh.Columns[column, Type.Missing].NumberFormat = "# #0,00"; } catch { }
                                break;
                        }
                        column++;
                    }
                    else
                        break;
                }
                myexceltask.ProgressChange(2 + (int)(decimal.Divide(1, maxrow) * 100));

                foreach (SpecificationDetailVM item in items.OfType<SpecificationDetailVM>())
                {
                    column = 1;
                    foreach (ColumnInfo columninfo in columns)
                    {
                        switch (columninfo.Property)
                        {
                            case nameof(SpecificationVM.FilePath):
                                exWh.Cells[row, column] = item.Specification.FilePath;
                                break;
                            case nameof(Parcel.ParcelNumberEntire):
                                exWh.Cells[row, column] = item.Specification.Parcel.ParcelNumberEntire;
                                break;
                            case nameof(SpecificationVM.CFPR):
                                exWh.Cells[row, column] = item.Specification.CFPR;
                                break;
                            case nameof(SpecificationVM.Customers):
                                exWh.Cells[row, column] = item.Specification.Customers;
                                break;
                            case nameof(SpecificationVM.CustomerLegals):
                                exWh.Cells[row, column] = item.Specification.CustomerLegals;
                                break;
                            case nameof(SpecificationDetailVM.Client):
                                exWh.Cells[row, column] = item.Client?.Name;
                                break;
                            case nameof(SpecificationDetailVM.Name):
                                exWh.Cells[row, column] = item.Name;
                                break;
                            case nameof(SpecificationDetailVM.Gender):
                                exWh.Cells[row, column] = item.Gender;
                                break;
                            case nameof(SpecificationDetailVM.Contexture):
                                exWh.Cells[row, column] = item.Contexture;
                                break;
                            case nameof(SpecificationDetailVM.Description):
                                exWh.Cells[row, column] = item.Description;
                                break;
                            case nameof(SpecificationDetailVM.SizeEN):
                                exWh.Cells[row, column] = item.SizeEN;
                                break;
                            case nameof(SpecificationDetailVM.SizeRU):
                                exWh.Cells[row, column] = item.SizeRU;
                                break;
                            case nameof(SpecificationDetailVM.VendorCode):
                                exWh.Cells[row, column] = item.VendorCode;
                                break;
                            case nameof(SpecificationDetailVM.Branch):
                                exWh.Cells[row, column] = item.Branch;
                                break;
                            case nameof(SpecificationDetailVM.Brand):
                                exWh.Cells[row, column] = item.Brand;
                                break;
                            case nameof(SpecificationDetailVM.TNVED):
                                exWh.Cells[row, column] = item.TNVED;
                                break;
                            case nameof(SpecificationDetailVM.Amount):
                                exWh.Cells[row, column] = item.Amount;
                                break;
                            case nameof(SpecificationDetailVM.NetWeight):
                                exWh.Cells[row, column] = item.NetWeight;
                                break;
                            case nameof(SpecificationDetailVM.GrossWeight):
                                exWh.Cells[row, column] = item.GrossWeight;
                                break;
                            case nameof(SpecificationDetailVM.CellNumber):
                                exWh.Cells[row, column] = item.CellNumber;
                                break;
                            case nameof(SpecificationDetailVM.Packing):
                                exWh.Cells[row, column] = item.Packing;
                                break;
                            case nameof(SpecificationDetailVM.Price):
                                exWh.Cells[row, column] = item.Price;
                                break;
                            case nameof(SpecificationDetailVM.Cost):
                                exWh.Cells[row, column] = item.Cost;
                                break;
                            case nameof(SpecificationDetailVM.CountryRU):
                                exWh.Cells[row, column] = item.CountryRU;
                                break;
                            case nameof(SpecificationDetailVM.CountryEN):
                                exWh.Cells[row, column] = item.CountryEN;
                                break;
                            case nameof(SpecificationDetailVM.Certificate):
                                exWh.Cells[row, column] = item.Certificate;
                                break;
                            case nameof(SpecificationDetailVM.Note):
                                exWh.Cells[row, column] = item.Note;
                                break;
                        }
                        column++;
                    }
                    row++;
                    myexceltask.ProgressChange(2 + (int)(decimal.Divide(row, maxrow) * 100));
                }

                r = exWh.Range[exWh.Cells[1, 1], exWh.Cells[1, column - 1]];
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlThin;
                r.Borders[Excel.XlBordersIndex.xlEdgeBottom].ColorIndex = 0;
                r.Borders[Excel.XlBordersIndex.xlInsideVertical].ColorIndex = 0;
                r.VerticalAlignment = Excel.Constants.xlTop;
                r.WrapText = true;
                //r = exWh.Range[exWh.Columns[1, Type.Missing], exWh.Columns[17, Type.Missing]]; r.Columns.AutoFit();

                exWh = null;
                exApp.Visible = true;
                exApp.DisplayAlerts = true;
                exApp.ScreenUpdating = true;
                myexceltask.ProgressChange(100);
                return new KeyValuePair<bool, string>(false, "Данные выгружены. " + maxrow.ToString() + " строк обработано.");
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
        private class ColumnInfo
        {
            public string Header { set; get; }
            public string Property { set; get; }
            public int Order { set; get; }
        }

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
            return false;
        }
        protected override bool CanSaveDataChanges()
        {
            return false;
        }
        protected override void OtherViewRefresh()
        { }
        protected override void RefreshData(object parametr)
        {
            if (myfilter.isEmpty)
                this.OpenPopup("Пожалуйста, задайте критерии выбора!", false);
            else
                mysddbm.FillAsync();
        }
        protected override void RejectChanges(object parametr)
        { }
        protected override void SettingView()
        {
        }

        public void Dispose()
        {
            myfilter.RemoveFilter();
            //CustomBrokerWpf.References.CarsViewCollector.RemoveView(mycars);
            //CustomBrokerWpf.References.CarryViewCollector.RemoveView(myview);
        }
    }

    public class SpecificationDetailBranchFilter : libui.CheckListBoxVMFillDefault<SpecificationDetailVM, string>
    {
        private List<string> mydefaultlist;
        internal List<string> DefaultList
        {
            get
            {
                if (mydefaultlist == null)
                {
                    mydefaultlist = new List<string>();
                    BranchDBM pdbm = new BranchDBM();
                    pdbm.Fill();
                    foreach (Branch goods in pdbm.Collection)
                        mydefaultlist.Add(goods.Name);
                }
                return mydefaultlist;
            }
        }

        protected override void AddItem(SpecificationDetailVM item)
        {
            if (!Items.Contains(item.Branch)) Items.Add(item.Branch);
        }
    }
    public class SpecificationDetailBrandFilter : libui.CheckListBoxVMFillDefault<SpecificationDetailVM, string>
    {
        private List<string> mydefaultlist;
        internal List<string> DefaultList
        {
            get
            {
                if (mydefaultlist == null)
                {
                    string[] names;
                    bool contains = false;
                    mydefaultlist = new List<string>();
                    GoodsDBM pdbm = new GoodsDBM();
                    pdbm.Fill();
                    foreach (Goods goods in pdbm.Collection)
                    {
                        names = goods.Brand.Trim(new char[] { ' ', ',' }).Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string name in names)
                        {
                            contains = false;
                            foreach (string contry in mydefaultlist)
                                if (string.Equals(contry, name, StringComparison.CurrentCultureIgnoreCase))
                                { contains = true; break; }
                            if (!contains) mydefaultlist.Add(name);
                        }
                    }
                }
                return mydefaultlist;
            }
        }

        protected override void AddItem(SpecificationDetailVM item)
        {
            if (!Items.Contains(item.Brand)) Items.Add(item.Brand);
        }
    }
    public class SpecificationDetailCertificateFilter : libui.CheckListBoxVMFillDefault<SpecificationDetailVM, string>
    {
        private List<string> mydefaultlist;
        internal List<string> DefaultList
        {
            get
            {
                if (mydefaultlist == null)
                {
                    mydefaultlist = new List<string>();
                    GoodsDBM pdbm = new GoodsDBM();
                    pdbm.Fill();
                    foreach (Goods goods in pdbm.Collection)
                        mydefaultlist.Add(goods.Certificate);
                }
                return mydefaultlist;
            }
        }

        protected override void AddItem(SpecificationDetailVM item)
        {
            if (!Items.Contains(item.Certificate)) Items.Add(item.Certificate);
        }
    }
    public class SpecificationDetailClientFilter : libui.CheckListBoxVMFillDefault<SpecificationDetailVM, string>
    {
        protected override void AddItem(SpecificationDetailVM item)
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
    public class SpecificationDetailCountryRuFilter : libui.CheckListBoxVMFillDefault<SpecificationDetailVM, string>
    {
        protected override void AddItem(SpecificationDetailVM item)
        {
            if (!Items.Contains(item.CountryRU)) Items.Add(item.CountryRU);
        }
    }
    public class SpecificationDetailParcelNumberEntireFilter : libui.CheckListBoxVMFillDefault<SpecificationDetailVM, Parcel>
    {
        internal SpecificationDetailParcelNumberEntireFilter() : base()
        {
            this.DisplayPath = "ParcelNumberEntire";
            this.SearchPath = "ParcelNumberEntire";
            this.GetDisplayPropertyValueFunc = (item) => { return ((Parcel)item).ParcelNumberEntire; };
        }

        private List<Parcel> mydefaultlist;
        internal List<Parcel> DefaultList
        {
            get
            {
                if (mydefaultlist == null)
                {
                    SQLFilter pfilter = new SQLFilter("parcel", "AND");
                    ParcelDBM pdbm = new ParcelDBM();
                    pdbm.Filter = pfilter.FilterWhereId;
                    pdbm.Fill();
                    mydefaultlist = pdbm.Collection.ToList<Parcel>();
                }
                return mydefaultlist;
            }
        }

        protected override void AddItem(SpecificationDetailVM item)
        {
            if (!Items.Contains(item.Specification.Parcel)) Items.Add(item.Specification.Parcel);
        }
    }
}
