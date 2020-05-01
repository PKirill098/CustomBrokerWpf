using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class RequestItem : lib.DomainBaseStamp
    {
        public RequestItem() : this(lib.NewObjectId.NewId, 0, null, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
            null, null, 0, null, null, null, null, null, null, null, null, null, string.Empty, 0,
            0, null, null, lib.DomainObjectState.Added)
        { }
        public RequestItem(int id, int requestid, int? number, string cncode, string name, string brand, string producer, string titlecountry, string subcountry, string translation, string composition, string sizes,
            string decree, string certificate, DateTime? certstart, DateTime? certstop,
            int quantity, decimal? wnet, decimal? wgross, decimal? pricekg, decimal? pricepc, decimal? ratevat, decimal? rateper, decimal? rateadd, decimal? rateper2, decimal? rateadd2,
            string note, int state
            , Int64 stamp, DateTime? updated, string updater, lib.DomainObjectState domainstate) : base(id, stamp, updated, updater, domainstate)
        {
            myrequestid = requestid;
            mynumber = number;
            mycncode = cncode;
            myname = name;
            mybrand = brand;
            myproducer = producer;
            mytitlecountry = titlecountry;
            mysubcountry = subcountry;
            mytranslation = translation;
            mycomposition = composition;
            mysizes = sizes;
            mydecree = decree;
            mycertificate = certificate;
            mycertstart = certstart;
            mycertstop = certstop;
            myquantity = quantity;
            mywnet = wnet;
            mywgross = wgross;
            mypricekg = pricekg;
            mypricepc = pricepc;
            myratevat = ratevat;
            myrateper = rateper;
            myrateadd = rateadd;
            myrateper2 = rateper2;
            myrateadd2 = rateadd2;
            mynote = note;
            mystate = state;
        }

        private int myrequestid;
        public int RequestId
        {
            set
            {
                if (myrequestid != value)
                {
                    string name = "RequestId";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myrequestid);
                    myrequestid = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myrequestid; }
        }
        private int? mynumber;
        public int? Number
        {
            set
            {
                if (mynumber.HasValue != value.HasValue || (value.HasValue && mynumber.Value != value.Value))
                {
                    string name = "Number";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mynumber);
                    mynumber = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mynumber; }
        }
        private string mycncode;
        public string CNCode
        {
            set
            {
                if (!string.Equals(mycncode, value))
                {
                    string name = "CNCode";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycncode);
                    mycncode = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mycncode; }
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
        private string mysubcountry;
        public string SubCountry
        {
            set
            {
                if (!string.Equals(mysubcountry, value))
                {
                    string name = "SubCountry";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mysubcountry);
                    mysubcountry = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mysubcountry; }
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
        private string mysizes;
        public string Sizes
        {
            set
            {
                if (!string.Equals(mysizes, value))
                {
                    string name = "Sizes";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mysizes);
                    mysizes = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mysizes; }
        }
        private string mydecree;
        public string Decree
        {
            set
            {
                if (!string.Equals(mydecree, value))
                {
                    string name = "Decree";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mydecree);
                    mydecree = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mydecree; }
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
        private int myquantity;
        public int Quantity
        {
            set
            {
                if (myquantity != value)
                {
                    string name = "Quantity";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myquantity);
                    myquantity = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myquantity; }
        }
        private decimal? mywnet;
        public decimal? WeightNet
        {
            set
            {
                if (mywnet.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mywnet.Value, value.Value)))
                {
                    string name = "WeightNet";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mywnet);
                    mywnet = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mywnet; }
        }
        private decimal? mywgross;
        public decimal? WeightGross
        {
            set
            {
                if (mywgross.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mywgross.Value, value.Value)))
                {
                    string name = "WeightGross";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mywgross);
                    mywgross = value;
                    mywnet = value.HasValue ? decimal.Ceiling(decimal.Multiply(value.Value, 0.95M)) : value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                    PropertyChangedNotification("WeightNet");
                }
            }
            get { return mywgross; }
        }
        private decimal? mypricekg;
        public decimal? PriceKG
        {
            set
            {
                if (mypricekg.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mypricekg.Value, value.Value)))
                {
                    string name = "PriceKG";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mypricekg);
                    mypricekg = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mypricekg; }
        }
        private decimal? mypricepc;
        public decimal? PricePC
        {
            set
            {
                if (mypricepc.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mypricepc.Value, value.Value)))
                {
                    string name = "PricePC";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mypricepc);
                    mypricepc = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mypricepc; }
        }
        private decimal? myratevat;
        public decimal? RateVat
        {
            set
            {
                if (myratevat.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myratevat.Value, value.Value)))
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
        private decimal? myrateper2;
        public decimal? RatePer2009
        {
            set
            {
                if (myrateper2.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myrateper2, value)))
                {
                    string name = "RatePer2009";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myrateper2);
                    myrateper2 = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myrateper2; }
        }
        private decimal? myrateadd2;
        public decimal? RateAdd2009
        {
            set
            {
                if (myrateadd2.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myrateadd2, value)))
                {
                    string name = "RateAdd2009";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myrateadd2);
                    myrateadd2 = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myrateadd2; }
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
        private int mystate;
        internal int State
        {
            set
            {
                if (mystate != value)
                {
                    string name = "State";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mystate);
                    mystate = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return mystate; }
        }
        private ObservableCollection<Specification.RequestItemNote> mynotes;
        internal ObservableCollection<Specification.RequestItemNote> Notes
        {
            get
            {
                if (mynotes == null)
                {
                    Specification.RequestItemNoteDBM ndbm = new Specification.RequestItemNoteDBM(this.Id);
                    ndbm.Fill();
                    mynotes = ndbm.Collection;
                }
                return mynotes;
            }
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Number":
                    mynumber = (int)value;
                    break;
                case "CNCode":
                    mycncode = (string)value;
                    break;
                case "Name":
                    myname = (string)value;
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
                case "SubCountry":
                    mysubcountry = (string)value;
                    break;
                case "Translation":
                    mytranslation = (string)value;
                    break;
                case "Composition":
                    mycomposition = (string)value;
                    break;
                case "Decree":
                    mydecree = (string)value;
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
                case "Quantity":
                    myquantity = (int)value;
                    break;
                case "WeightNet":
                    mywnet = (decimal?)value;
                    break;
                case "WeightGross":
                    mywgross = (decimal?)value;
                    break;
                case "PriceKG":
                    mypricekg = (decimal?)value;
                    break;
                case "PricePC":
                    mypricepc = (decimal?)value;
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
                case "Note":
                    mynote = (string)value;
                    break;
                case "State":
                    mystate = (int)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    if (mynotes != null)
                    {
                        Specification.RequestItemNote[] additem = new Specification.RequestItemNote[mynotes.Count];
                        foreach (Specification.RequestItemNote item in mynotes)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            { additem[i] = item; i++; }
                            else if (item.DomainState == lib.DomainObjectState.Deleted)
                            {
                                item.DomainState = lib.DomainObjectState.Unchanged;
                            }
                        }

                        for (int ii = 0; ii < i; ii++) mynotes.Remove(additem[ii]);
                    }
                    break;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            throw new NotImplementedException();
        }
    }

    internal class RequestItemDBM : lib.DBManagerId<RequestItem>
    {
        internal RequestItemDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectCommandText = "dbo.RequestItem_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int), new SqlParameter("@requestid", System.Data.SqlDbType.Int) };

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);
            SqlParameter parstamp = new SqlParameter("@stamp", System.Data.SqlDbType.BigInt); parstamp.Direction = System.Data.ParameterDirection.InputOutput;
            SqlParameter parupdated = new SqlParameter("@updated", System.Data.SqlDbType.DateTime2); parupdated.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parupdater = new SqlParameter("@updater", System.Data.SqlDbType.NVarChar, 20); parupdater.Direction = System.Data.ParameterDirection.Output;

            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@requestid", System.Data.SqlDbType.Int) };
            myupdateparams = new SqlParameter[]
            {
                parid,
                new SqlParameter("@numbercg", System.Data.SqlDbType.Bit),
                new SqlParameter("@descriptioncg", System.Data.SqlDbType.Bit),
                new SqlParameter("@cncodecg", System.Data.SqlDbType.Bit),
                new SqlParameter("@quantitycg", System.Data.SqlDbType.Bit),
                new SqlParameter("@wnetcg", System.Data.SqlDbType.Bit),
                new SqlParameter("@wgrosscg", System.Data.SqlDbType.Bit),
                new SqlParameter("@pricekgcg", System.Data.SqlDbType.Bit),
                new SqlParameter("@pricepccg", System.Data.SqlDbType.Bit),
                new SqlParameter("@ratevatcg", System.Data.SqlDbType.Bit),
                new SqlParameter("@ratepercg", System.Data.SqlDbType.Bit),
                new SqlParameter("@rateaddcg", System.Data.SqlDbType.Bit),
                new SqlParameter("@rateper2009cg", System.Data.SqlDbType.Bit),
                new SqlParameter("@rateadd2009cg", System.Data.SqlDbType.Bit),
                new SqlParameter("@certificatecg", System.Data.SqlDbType.Bit),
                new SqlParameter("@certstartcg", System.Data.SqlDbType.Bit),
                new SqlParameter("@certstopcg", System.Data.SqlDbType.Bit),
                new SqlParameter("@decreecg", System.Data.SqlDbType.Bit),
                new SqlParameter("@compositioncg", System.Data.SqlDbType.Bit),
                new SqlParameter("@translationcg", System.Data.SqlDbType.Bit),
                new SqlParameter("@brandcg", System.Data.SqlDbType.Bit),
                new SqlParameter("@sizescg", System.Data.SqlDbType.Bit),
                new SqlParameter("@producercg", System.Data.SqlDbType.Bit),
                new SqlParameter("@titlecountrycg", System.Data.SqlDbType.Bit),
                new SqlParameter("@subcountrycg", System.Data.SqlDbType.Bit),
                new SqlParameter("@notecg", System.Data.SqlDbType.Bit),
                new SqlParameter("@statecg", System.Data.SqlDbType.Bit)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@number", System.Data.SqlDbType.Int),
                new SqlParameter("@description", System.Data.SqlDbType.NVarChar,150),
                new SqlParameter("@cncode", System.Data.SqlDbType.NChar,10),
                new SqlParameter("@quantity", System.Data.SqlDbType.Int),
                new SqlParameter("@wnet", System.Data.SqlDbType.SmallMoney),
                new SqlParameter("@wgross", System.Data.SqlDbType.SmallMoney),
                new SqlParameter("@pricekg", System.Data.SqlDbType.Money),
                new SqlParameter("@pricepc", System.Data.SqlDbType.Money),
                new SqlParameter("@ratevat", System.Data.SqlDbType.Money),
                new SqlParameter("@rateper", System.Data.SqlDbType.Money),
                new SqlParameter("@rateadd", System.Data.SqlDbType.Money),
                new SqlParameter("@rateper2009", System.Data.SqlDbType.Money),
                new SqlParameter("@rateadd2009", System.Data.SqlDbType.Money),
                new SqlParameter("@certificate", System.Data.SqlDbType.NVarChar,30),
                new SqlParameter("@certstart", System.Data.SqlDbType.DateTime2),
                new SqlParameter("@certstop", System.Data.SqlDbType.DateTime2),
                new SqlParameter("@decree", System.Data.SqlDbType.NVarChar,100),
                new SqlParameter("@composition", System.Data.SqlDbType.NVarChar,250),
                new SqlParameter("@translation", System.Data.SqlDbType.NVarChar,1000),
                new SqlParameter("@brand", System.Data.SqlDbType.NVarChar,100),
                new SqlParameter("@sizes", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@producer", System.Data.SqlDbType.NVarChar,100),
                new SqlParameter("@titlecountry", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@subcountry", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@note", System.Data.SqlDbType.NVarChar,250),
                new SqlParameter("@state", System.Data.SqlDbType.Int),
                parstamp, parupdated, parupdater
            };
            mydeleteparams = new SqlParameter[] { parid };

            InsertProcedure = true;
            myinsertcommandtext = "dbo.RequestItemAdd_sp";
            UpdateProcedure = true;
            myupdatecommandtext = "dbo.RequestItemUpd_sp";
            DeleteProcedure = true;
            mydeletecommandtext = "dbo.RequestItemDel_sp";
        }

        private Specification.RequestItemNoteDBM ndbm = new Specification.RequestItemNoteDBM();
        public override int? ItemId
        {
            get
            {
                return (int)SelectParams[0].Value;
            }
            set
            {
                SelectParams[0].Value = value;
            }
        }
        internal int RequestId
        {
            get
            {
                return (int)SelectParams[1].Value;
            }
            set
            {
                SelectParams[1].Value = value;
            }
        }

        protected override void SetSelectParametersValue()
        {
        }
        protected override RequestItem CreateItem(SqlDataReader reader,SqlConnection addcon)
        {

            return new RequestItem(
                reader.GetInt32(0),
                reader.GetInt32(1),
                reader.GetInt32(2),
                reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                reader.IsDBNull(reader.GetOrdinal("brand")) ? string.Empty : reader.GetString(reader.GetOrdinal("brand")),
                reader.IsDBNull(reader.GetOrdinal("producer")) ? string.Empty : reader.GetString(reader.GetOrdinal("producer")),
                reader.IsDBNull(reader.GetOrdinal("titlecountry")) ? string.Empty : reader.GetString(reader.GetOrdinal("titlecountry")),
                reader.IsDBNull(reader.GetOrdinal("subcountry")) ? string.Empty : reader.GetString(reader.GetOrdinal("subcountry")),
                reader.IsDBNull(reader.GetOrdinal("translation")) ? string.Empty : reader.GetString(reader.GetOrdinal("translation")),
                reader.IsDBNull(reader.GetOrdinal("composition")) ? string.Empty : reader.GetString(reader.GetOrdinal("composition")),
                reader.IsDBNull(reader.GetOrdinal("sizes")) ? string.Empty : reader.GetString(reader.GetOrdinal("sizes")),
                reader.IsDBNull(reader.GetOrdinal("decree")) ? string.Empty : reader.GetString(reader.GetOrdinal("decree")),
                reader.IsDBNull(reader.GetOrdinal("certificate")) ? string.Empty : reader.GetString(reader.GetOrdinal("certificate")),
                reader.IsDBNull(reader.GetOrdinal("certstart")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("certstart")),
                reader.IsDBNull(reader.GetOrdinal("certstop")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("certstop")),
                reader.GetInt32(reader.GetOrdinal("quantity")),
                reader.IsDBNull(reader.GetOrdinal("wnet")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("wnet")),
                reader.IsDBNull(reader.GetOrdinal("wgross")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("wgross")),
                reader.IsDBNull(reader.GetOrdinal("pricekg")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("pricekg")),
                reader.IsDBNull(reader.GetOrdinal("pricepc")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("pricepc")),
                reader.IsDBNull(reader.GetOrdinal("ratevat")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("ratevat")),
                reader.IsDBNull(reader.GetOrdinal("rateper")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("rateper")),
                reader.IsDBNull(reader.GetOrdinal("rateadd")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("rateadd")),
                reader.IsDBNull(reader.GetOrdinal("rateper2009")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("rateper2009")),
                reader.IsDBNull(reader.GetOrdinal("rateadd2009")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("rateadd2009")),
                reader.IsDBNull(reader.GetOrdinal("note")) ? string.Empty : reader.GetString(reader.GetOrdinal("note")),
                reader.GetInt32(reader.GetOrdinal("state")),
                reader.GetInt64(reader.GetOrdinal("stamp")),
                reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")),
                reader.IsDBNull(reader.GetOrdinal("updater")) ? string.Empty : reader.GetString(reader.GetOrdinal("updater")),
                lib.DomainObjectState.Unchanged);
        }
        protected override void GetOutputParametersValue(RequestItem item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
            {
                item.Id = (int)myinsertparams[0].Value;
                item.Stamp = (Int64)myinsertupdateparams[26].Value;
            }
            else if (item.DomainState == lib.DomainObjectState.Modified)
            {
                item.Stamp = (Int64)myinsertupdateparams[26].Value;
                item.UpdateWhen = (DateTime)myinsertupdateparams[27].Value;
                item.UpdateWho = (string)myinsertupdateparams[28].Value;
            }
        }
        protected override bool SaveChildObjects(RequestItem item)
        {
            bool isSuccess=true;
            ndbm.Errors.Clear();
            ndbm.RequestItemId = item.Id;
            ndbm.Collection = item.Notes;
            if (!ndbm.SaveCollectionChanches())
            {
                isSuccess = false;
                foreach (lib.DBMError err in ndbm.Errors) this.Errors.Add(err);
            }
            return isSuccess;
        }
        protected override bool SaveIncludedObject(RequestItem item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(RequestItem item)
        {
            myinsertparams[1].Value = item.RequestId;
            myupdateparams[0].Value = item.Id;
            myupdateparams[1].Value = item.HasPropertyOutdatedValue("Number");
            myupdateparams[2].Value = item.HasPropertyOutdatedValue("Name");
            myupdateparams[3].Value = item.HasPropertyOutdatedValue("CNCode");
            myupdateparams[4].Value = item.HasPropertyOutdatedValue("Quantity");
            myupdateparams[5].Value = item.HasPropertyOutdatedValue("WeightNet");
            myupdateparams[6].Value = item.HasPropertyOutdatedValue("WeightGross");
            myupdateparams[7].Value = item.HasPropertyOutdatedValue("PriceKG");
            myupdateparams[8].Value = item.HasPropertyOutdatedValue("PricePC");
            myupdateparams[9].Value = item.HasPropertyOutdatedValue("RateVat");
            myupdateparams[10].Value = item.HasPropertyOutdatedValue("RatePer");
            myupdateparams[11].Value = item.HasPropertyOutdatedValue("RateAdd");
            myupdateparams[12].Value = item.HasPropertyOutdatedValue("RatePer2009");
            myupdateparams[13].Value = item.HasPropertyOutdatedValue("RateAdd2009");
            myupdateparams[14].Value = item.HasPropertyOutdatedValue("Certificate");
            myupdateparams[15].Value = item.HasPropertyOutdatedValue("CertStart");
            myupdateparams[16].Value = item.HasPropertyOutdatedValue("CertStop");
            myupdateparams[17].Value = item.HasPropertyOutdatedValue("Decree");
            myupdateparams[18].Value = item.HasPropertyOutdatedValue("Composition");
            myupdateparams[19].Value = item.HasPropertyOutdatedValue("Translation");
            myupdateparams[20].Value = item.HasPropertyOutdatedValue("Brand");
            myupdateparams[21].Value = item.HasPropertyOutdatedValue("Sizes");
            myupdateparams[22].Value = item.HasPropertyOutdatedValue("Producer");
            myupdateparams[23].Value = item.HasPropertyOutdatedValue("TitleCountry");
            myupdateparams[24].Value = item.HasPropertyOutdatedValue("SubCountry");
            myupdateparams[25].Value = item.HasPropertyOutdatedValue("Note");
            myupdateparams[26].Value = item.HasPropertyOutdatedValue("State");
            myinsertupdateparams[0].Value = item.Number.HasValue ? item.Number.Value : (int?)null;
            myinsertupdateparams[1].Value = item.Name;
            myinsertupdateparams[2].Value = item.CNCode;
            myinsertupdateparams[3].Value = item.Quantity;
            myinsertupdateparams[4].Value = item.WeightNet.HasValue ? item.WeightNet.Value : (decimal?)null;
            myinsertupdateparams[5].Value = item.WeightGross.HasValue ? item.WeightGross.Value : (decimal?)null;
            myinsertupdateparams[6].Value = item.PriceKG.HasValue ? item.PriceKG.Value : (decimal?)null;
            myinsertupdateparams[7].Value = item.PricePC.HasValue ? item.PricePC.Value : (decimal?)null;
            myinsertupdateparams[8].Value = item.RateVat.HasValue ? item.RateVat.Value : (decimal?)null;
            myinsertupdateparams[9].Value = item.RatePer.HasValue ? item.RatePer.Value : (decimal?)null;
            myinsertupdateparams[10].Value = item.RateAdd.HasValue ? item.RateAdd.Value : (decimal?)null;
            myinsertupdateparams[11].Value = item.RatePer.HasValue ? item.RatePer.Value : (decimal?)null;
            myinsertupdateparams[12].Value = item.RateAdd.HasValue ? item.RateAdd.Value : (decimal?)null;
            myinsertupdateparams[13].Value = string.IsNullOrEmpty(item.Certificate) ? string.Empty : item.Certificate;
            myinsertupdateparams[14].Value = item.CertStart.HasValue ? item.CertStart.Value : (DateTime?)null;
            myinsertupdateparams[15].Value = item.CertStop.HasValue ? item.CertStop.Value : (DateTime?)null;
            myinsertupdateparams[16].Value = string.IsNullOrEmpty(item.Decree) ? string.Empty : item.Decree;
            myinsertupdateparams[17].Value = string.IsNullOrEmpty(item.Composition) ? string.Empty : item.Composition;
            myinsertupdateparams[18].Value = string.IsNullOrEmpty(item.Translation) ? string.Empty : item.Translation;
            myinsertupdateparams[19].Value = string.IsNullOrEmpty(item.Brand) ? (object)DBNull.Value : item.Brand;
            myinsertupdateparams[20].Value = string.IsNullOrEmpty(item.Sizes) ? string.Empty : item.Sizes;
            myinsertupdateparams[21].Value = string.IsNullOrEmpty(item.Producer) ? (object)DBNull.Value : item.Producer;
            myinsertupdateparams[22].Value = string.IsNullOrEmpty(item.TitleCountry) ? (object)DBNull.Value : item.TitleCountry;
            myinsertupdateparams[23].Value = string.IsNullOrEmpty(item.SubCountry) ? (object)DBNull.Value : item.SubCountry;
            myinsertupdateparams[24].Value = string.IsNullOrEmpty(item.Note) ? (object)DBNull.Value : item.Note;
            myinsertupdateparams[25].Value = item.State;
            myinsertupdateparams[26].Value = item.Stamp;
            mydeleteparams[0].Value = item.Id;
            return true;
        }
        protected override void ItemAcceptChanches(RequestItem item)
        {
            item.AcceptChanches();
        }
        protected override void LoadObjects(RequestItem item)
        {
        }
        protected override bool LoadObjects()
        { return true; }
    }

    public class RequestItemVM : lib.ViewModelErrorNotifyItem<RequestItem>
    {
        public RequestItemVM() : this(new RequestItem()) { }
        public RequestItemVM(RequestItem domain) : base(domain)
        {
            ValidetingProperties.AddRange(new string[] { "Name", "Quantity" });
            InitProperties();
        }

        public int? Number
        {
            set
            {
                if (base.DomainObject.Number.HasValue != value.HasValue || (value.HasValue && base.DomainObject.Number.Value != value.Value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Number";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Number);
                    ChangingDomainProperty = name;
                    base.DomainObject.Number = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Number : null; }
        }
        public string CNCode
        {
            set
            {
                if (!string.Equals(base.DomainObject.CNCode, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Code";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.CNCode);
                    ChangingDomainProperty = name;
                    base.DomainObject.CNCode = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.CNCode : null; }
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
        public string SubCountry
        {
            set
            {
                if (!string.Equals(base.DomainObject.SubCountry, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "SubCountry";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.SubCountry);
                    ChangingDomainProperty = name;
                    base.DomainObject.SubCountry = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.SubCountry : null; }
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
        public string Sizes
        {
            set
            {
                if (!string.Equals(base.DomainObject.Sizes, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Sizes";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Sizes);
                    ChangingDomainProperty = name;
                    base.DomainObject.Sizes = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Sizes : null; }
        }
        public string Decree
        {
            set
            {
                if (!string.Equals(base.DomainObject.Decree, value) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Decree";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Decree);
                    ChangingDomainProperty = name;
                    base.DomainObject.Decree = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.Decree : null; }
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
                      + (base.DomainObject.CertStart.HasValue ? " от " + base.DomainObject.CertStart.Value.ToString("dd.MM.yy") : string.Empty)
                      + (base.DomainObject.CertStop.HasValue ? " до " + base.DomainObject.CertStop.Value.ToString("dd.MM.yy") : string.Empty)
                  : null;
            }
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
        private int myquantity;
        public int? Quantity
        {
            set
            {
                if (value.HasValue && myquantity != value.Value & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "Quantity";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.Quantity);
                    myquantity = value.Value;
                    if (ValidateProperty(name))
                    {
                        OnTotalValueChanged(name, base.DomainObject.Quantity, value);
                        decimal? oldprice = this.PriceTotal, oldtaxtotal = this.TaxTotal, oldtaxtotal2 = this.TaxTotal2009, oldvat = this.TaxVAT, oldvat2 = this.TaxVAT2009;
                        ChangingDomainProperty = name;
                        base.DomainObject.Quantity = value.Value;
                        ClearErrorMessageForProperty(name);
                        OnTotalValueChanged("PriceTotal", oldprice, this.PriceTotal);
                        OnTotalValueChanged("TaxTotal", oldtaxtotal, this.TaxTotal);
                        OnTotalValueChanged("TaxTotal2", oldtaxtotal2, this.TaxTotal2009);
                        OnTotalValueChanged("TaxVAT", oldvat, this.TaxVAT);
                        OnTotalValueChanged("TaxVAT2", oldvat2, this.TaxVAT2009);
                    }
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? myquantity : (int?)null; }
        }
        public decimal? WeightUnit
        {
            get { return this.DomainState != lib.DomainObjectState.Deleted & base.DomainObject.WeightGross.HasValue & base.DomainObject.Quantity > 0 ? decimal.Divide(base.DomainObject.WeightGross.Value, base.DomainObject.Quantity) : (decimal?)null; }
        }
        public decimal? WeightNet
        {
            set
            {
                if ((base.DomainObject.WeightNet.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.WeightNet.Value, value.Value))) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "WeightNet";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.WeightNet);
                    OnTotalValueChanged(name, base.DomainObject.WeightNet, value);
                    decimal? oldprice = this.PriceTotal, oldtaxtotal = this.TaxTotal, oldtaxtotal2 = this.TaxTotal2009, oldvat = this.TaxVAT, oldvat2 = this.TaxVAT2009;
                    ChangingDomainProperty = name;
                    base.DomainObject.WeightNet = value;
                    OnTotalValueChanged("PriceTotal", oldprice, this.PriceTotal);
                    OnTotalValueChanged("TaxTotal", oldtaxtotal, this.TaxTotal);
                    OnTotalValueChanged("TaxTotal2", oldtaxtotal2, this.TaxTotal2009);
                    OnTotalValueChanged("TaxVAT", oldvat, this.TaxVAT);
                    OnTotalValueChanged("TaxVAT2", oldvat2, this.TaxVAT2009);
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.WeightNet : null; }
        }
        public decimal? WeightGross
        {
            set
            {
                if ((base.DomainObject.WeightGross.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.WeightGross.Value, value.Value))) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "WeightGross";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.WeightGross);
                    OnTotalValueChanged(name, base.DomainObject.WeightGross, value);
                    ChangingDomainProperty = name;
                    base.DomainObject.WeightGross = value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.WeightGross : null; }
        }
        public decimal? PriceKG
        {
            set
            {
                if ((base.DomainObject.PriceKG.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.PriceKG.Value, value.Value))) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "PriceKG";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.PriceKG);
                    decimal? oldprice=this.PriceTotal, oldtaxtotal = this.TaxTotal, oldtaxtotal2 = this.TaxTotal2009, oldvat = this.TaxVAT, oldvat2 = this.TaxVAT2009;
                    ChangingDomainProperty = name;
                    base.DomainObject.PriceKG = value;
                    OnTotalValueChanged("PriceTotal", oldprice, this.PriceTotal);
                    OnTotalValueChanged("TaxTotal", oldtaxtotal, this.TaxTotal);
                    OnTotalValueChanged("TaxTotal2", oldtaxtotal2, this.TaxTotal2009);
                    OnTotalValueChanged("TaxVAT", oldvat, this.TaxVAT);
                    OnTotalValueChanged("TaxVAT2", oldvat2, this.TaxVAT2009);
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.PriceKG : null; }
        }
        public decimal? PricePC
        {
            set
            {
                if ((base.DomainObject.PricePC.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.PricePC.Value, value.Value))) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "PricePC";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.PricePC);
                    decimal? oldprice = this.PriceTotal, oldtaxtotal = this.TaxTotal, oldtaxtotal2 = this.TaxTotal2009, oldvat = this.TaxVAT, oldvat2 = this.TaxVAT2009;
                    ChangingDomainProperty = name;
                    base.DomainObject.PricePC = value;
                    OnTotalValueChanged("PriceTotal", oldprice, this.PriceTotal);
                    OnTotalValueChanged("TaxTotal", oldtaxtotal, this.TaxTotal);
                    OnTotalValueChanged("TaxTotal2", oldtaxtotal2, this.TaxTotal2009);
                    OnTotalValueChanged("TaxVAT", oldvat, this.TaxVAT);
                    OnTotalValueChanged("TaxVAT2", oldvat2, this.TaxVAT2009);
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.PricePC : null; }
        }
        public decimal? PriceKGFrom
        {
            get { return this.DomainState != lib.DomainObjectState.Deleted & base.DomainObject.PriceKG.HasValue & base.DomainObject.WeightNet.HasValue ? decimal.Multiply(base.DomainObject.PriceKG.Value, base.DomainObject.WeightNet.Value) : (decimal?)null; }
        }
        public decimal? PricePCFrom
        {
            get { return this.DomainState != lib.DomainObjectState.Deleted & base.DomainObject.PricePC.HasValue & base.DomainObject.Quantity > 0M ? decimal.Multiply(base.DomainObject.PricePC.Value, base.DomainObject.Quantity) : (decimal?)null; }
        }
        public decimal? PriceTotal
        {
            get
            {
                decimal? value = null;
                decimal? pricekg = this.PriceKGFrom;
                decimal? pricepc = this.PricePCFrom;
                if (this.DomainState != lib.DomainObjectState.Deleted & (pricekg.HasValue | pricepc.HasValue))
                {
                    if (pricekg.HasValue & pricepc.HasValue)
                    {
                        if (pricekg.Value > pricepc.Value)
                            value = pricekg;
                        else
                            value = pricepc;
                    }
                    else if (pricekg.HasValue)
                        value = pricekg;
                    else
                        value = pricepc;
                    value = decimal.Ceiling(value.Value);
                }
                return value;
            }
        }
        public decimal? RateVat
        {
            set
            {
                if ((base.DomainObject.RateVat.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.RateVat.Value, value.Value))) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RateVat";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RateVat);
                    decimal? oldvat = this.TaxVAT, oldvat2 = this.TaxVAT2009;
                    ChangingDomainProperty = name;
                    base.DomainObject.RateVat = value;
                    OnTotalValueChanged("TaxVAT", oldvat, this.TaxVAT);
                    OnTotalValueChanged("TaxVAT2", oldvat2, this.TaxVAT2009);
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RateVat : null; }
        }
        public decimal? RateVatPer
        {
            set
            {
                if ((base.DomainObject.RateVat.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(decimal.Multiply(base.DomainObject.RateVat.Value, 100M), value.Value))) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RateVat";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RateVat);
                    ChangingDomainProperty = name;
                    base.DomainObject.RateVat = value.HasValue ? decimal.Divide((decimal)value.Value, 100M) : value;
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? (base.DomainObject.RateVat.HasValue ? decimal.Multiply(base.DomainObject.RateVat.Value, 100M) : base.DomainObject.RateVat) : null; }
        }
        public decimal? RatePer
        {
            set
            {
                if (base.DomainObject.RatePer.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.RatePer.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RatePer";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RatePer);
                    decimal? oldtaxtotal = this.TaxTotal, oldvat = this.TaxVAT;
                    ChangingDomainProperty = name;
                    base.DomainObject.RatePer = value;
                    OnTotalValueChanged("TaxTotal", oldtaxtotal, this.TaxTotal);
                    OnTotalValueChanged("TaxVAT", oldvat, this.TaxVAT);
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RatePer : null; }
        }
        public decimal? RatePerPer
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
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RatePer.HasValue ? decimal.Multiply(base.DomainObject.RatePer.Value, 100M) : base.DomainObject.RatePer : null; }
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
                    decimal? oldtaxtotal = this.TaxTotal, oldvat = this.TaxVAT;
                    base.DomainObject.RateAdd = value;
                    OnTotalValueChanged("TaxTotal", oldtaxtotal, this.TaxTotal);
                    OnTotalValueChanged("TaxVAT", oldvat, this.TaxVAT);
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RateAdd : null; }
        }
        public decimal? RatePer2009
        {
            set
            {
                if (base.DomainObject.RatePer2009.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(base.DomainObject.RatePer2009.Value, value.Value)) & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "RatePer2009";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.RatePer2009);
                    decimal? oldtaxtotal2 = this.TaxTotal2009, oldvat2 = this.TaxVAT2009;
                    ChangingDomainProperty = name;
                    base.DomainObject.RatePer2009 = value;
                    OnTotalValueChanged("TaxTotal2", oldtaxtotal2, this.TaxTotal2009);
                    OnTotalValueChanged("TaxVAT2", oldvat2, this.TaxVAT2009);
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RatePer2009 : null; }
        }
        public decimal? RatePer2009Per
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
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RatePer2009.HasValue ? decimal.Multiply(base.DomainObject.RatePer2009.Value, 100M) : base.DomainObject.RatePer2009 : null; }
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
                    decimal? oldtaxtotal2 = this.TaxTotal2009, oldvat2 = this.TaxVAT2009;
                    ChangingDomainProperty = name;
                    base.DomainObject.RateAdd2009 = value;
                    OnTotalValueChanged("TaxTotal2", oldtaxtotal2, this.TaxTotal2009);
                    OnTotalValueChanged("TaxVAT2", oldvat2, this.TaxVAT2009);
                }
            }
            get { return this.DomainState != lib.DomainObjectState.Deleted ? base.DomainObject.RateAdd2009 : null; }
        }
        internal int State
        {
            set
            {
                if (base.DomainObject.State != value & this.DomainState != lib.DomainObjectState.Deleted)
                {
                    string name = "State";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, base.DomainObject.State);
                    ChangingDomainProperty = name;
                    base.DomainObject.State = value;
                }
            }
            get { return base.DomainObject.State; }
        }
        public int StateColor
        {
            get
            {
                int color = 0;
                if ((base.DomainObject.State & 2) == 2) color = 1;

                return color;
            }
        }
        public decimal? TaxKG
        {
            get { return this.DomainState != lib.DomainObjectState.Deleted & base.DomainObject.WeightNet.HasValue & base.DomainObject.RateAdd.HasValue ? decimal.Multiply(decimal.Multiply(base.DomainObject.WeightNet.Value, base.DomainObject.RateAdd.Value), 1.0588M) : (decimal?)null; }
        }
        public decimal? TaxKG2009
        {
            get { return this.DomainState != lib.DomainObjectState.Deleted & base.DomainObject.WeightNet.HasValue & base.DomainObject.RateAdd2009.HasValue ? decimal.Multiply(decimal.Multiply(base.DomainObject.WeightNet.Value, base.DomainObject.RateAdd2009.Value), 1.0588M) : (decimal?)null; }
        }
        public decimal? TaxPer
        {
            get { return this.DomainState != lib.DomainObjectState.Deleted & this.PriceTotal.HasValue & base.DomainObject.RatePer.HasValue ? decimal.Multiply(this.PriceTotal.Value, base.DomainObject.RatePer.Value) : (decimal?)null; }
        }
        public decimal? TaxPer2009
        {
            get { return this.DomainState != lib.DomainObjectState.Deleted & this.PriceTotal.HasValue & base.DomainObject.RatePer2009.HasValue ? decimal.Multiply(this.PriceTotal.Value, base.DomainObject.RatePer2009.Value) : (decimal?)null; }
        }
        public decimal? TaxTotal
        {
            get
            {
                decimal? value = null;
                decimal? taxkg = this.TaxKG;
                decimal? taxper = this.TaxPer;
                if (this.DomainState != lib.DomainObjectState.Deleted & (taxkg.HasValue | taxper.HasValue))
                {
                    if (taxkg.HasValue & taxper.HasValue)
                    {
                        if (taxkg.Value > taxper.Value)
                            value = taxkg;
                        else
                            value = taxper;
                    }
                    else
                    {
                        if (taxkg.HasValue)
                            value = taxkg;
                        else
                            value = taxper;
                    }
                    value = decimal.Ceiling(value.Value);
                }
                return value;
            }
        }
        public decimal? TaxTotal2009
        {
            get
            {
                decimal? value = null;
                decimal? taxkg = this.TaxKG2009;
                decimal? taxper = this.TaxPer2009;
                if (this.DomainState != lib.DomainObjectState.Deleted & (taxkg.HasValue | taxper.HasValue))
                {
                    if (taxkg.HasValue & taxper.HasValue)
                    {
                        if (taxkg.Value > taxper.Value)
                            value = taxkg;
                        else
                            value = taxper;
                    }
                    else
                    {
                        if (taxkg.HasValue)
                            value = taxkg;
                        else
                            value = taxper;
                    }
                    value = decimal.Ceiling(value.Value);
                }
                return value;
            }
        }
        public decimal? TaxVAT
        {
            get
            {
                decimal? value = null;
                decimal? price = this.PriceTotal;
                decimal? tax = this.TaxTotal;
                decimal? ratevat = base.DomainObject.RateVat;
                if (this.DomainState != lib.DomainObjectState.Deleted & price.HasValue & tax.HasValue & ratevat.HasValue)
                {
                    value = decimal.Ceiling(decimal.Multiply(decimal.Add(price.Value, tax.Value), ratevat.Value));
                }
                return value;
            }
        }
        public decimal? TaxVAT2009
        {
            get
            {
                decimal? value = null;
                decimal? price = this.PriceTotal;
                decimal? tax = this.TaxTotal2009;
                decimal? ratevat = base.DomainObject.RateVat;
                if (this.DomainState != lib.DomainObjectState.Deleted & price.HasValue & tax.HasValue & ratevat.HasValue)
                {
                    value = decimal.Ceiling(decimal.Multiply(decimal.Add(price.Value, tax.Value), ratevat.Value));
                }
                return value;
            }
        }
        private string myerrdescription;
        public string ErrDescription
        {
            set
            {
                if (!string.Equals(myerrdescription,value))
                {
                    myerrdescription=value;
                    PropertyChangedNotification("ErrDescription");
                }
            }
            get
            {
                if (string.IsNullOrEmpty(myerrdescription))
                {
                    if (mynotes == null)
                    {
                        mynotes = new System.Windows.Data.ListCollectionView(this.DomainObject.Notes);
                        mynotes.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                        mynotes.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                    }
                    mynotes.MoveCurrentToFirst();
                    return (mynotes.CurrentItem as Specification.RequestItemNote)?.Note;
                }
                else
                    return myerrdescription;
            }
        }
        public string ErrDescriptions
        {
            get
            {
                System.Text.StringBuilder strb = new System.Text.StringBuilder();
                foreach(object item in mynotes)
                {
                    if(item is Specification.RequestItemNote)
                        strb.AppendLine((item as Specification.RequestItemNote)?.Note);
                }
                return strb.ToString();
            }
        }
        private System.Windows.Data.ListCollectionView mynotes;
        public System.Windows.Data.ListCollectionView Notes
        {
           get
            {
                if (mynotes == null)
                {
                    mynotes = new System.Windows.Data.ListCollectionView(this.DomainObject.Notes);
                    mynotes.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
                    mynotes.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                }
                return mynotes;
            }
        }
        internal void PutErrDescription(string notecode,string note)
        {
            Specification.RequestItemNote inote=null;
            foreach(object item in this.Notes)
            {
                if(string.Equals((item as Specification.RequestItemNote)?.NoteCode, notecode))
                {
                    inote = item as Specification.RequestItemNote;
                    break;
                }
            }
            if (inote == null & !string.IsNullOrEmpty(note))
                if (mynotes.Dispatcher.Thread.ManagedThreadId == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
                    mynotes.AddNewItem(new Specification.RequestItemNote(this.DomainObject, notecode, note));
                else
                    mynotes.Dispatcher.Invoke(() => { mynotes.AddNewItem(new Specification.RequestItemNote(this.DomainObject, notecode, note)); });
            else if (inote != null & string.IsNullOrEmpty(note))
            {
                if (mynotes.Dispatcher.Thread.ManagedThreadId == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
                {
                    mynotes.EditItem(inote);
                    inote.DomainState = lib.DomainObjectState.Deleted;
                    mynotes.CommitEdit();
                }
                else
                    mynotes.Dispatcher.Invoke(() => {
                        mynotes.EditItem(inote);
                        inote.DomainState = lib.DomainObjectState.Deleted;
                        mynotes.CommitEdit();
                    });
            }
            else if (inote != null & !string.IsNullOrEmpty(note))
            {
                if (mynotes.Dispatcher.Thread.ManagedThreadId == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
                {
                    mynotes.EditItem(inote);
                    inote.Note = note;
                    mynotes.CommitEdit();
                }
                else
                    mynotes.Dispatcher.Invoke(() => {
                        mynotes.EditItem(inote);
                        inote.Note = note;
                        mynotes.CommitEdit();
                    });
            }
            PropertyChangedNotification("ErrDescription");
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Name":
                    myname = this.DomainObject.Name;
                    break;
                case "Quantity":
                    myquantity = this.DomainObject.Quantity;
                    PropertyChangedNotification("WeightUnit");
                    PropertyChangedNotification("PricePCFrom");
                    PropertyChangedNotification("PriceTotal");
                    PropertyChangedNotification("TaxPer");
                    PropertyChangedNotification("TaxTotal");
                    PropertyChangedNotification("TaxVAT");
                    PropertyChangedNotification("TaxPer2009");
                    PropertyChangedNotification("TaxTotal2009");
                    PropertyChangedNotification("TaxVAT2009");
                    break;
                case "WeightGross":
                    PropertyChangedNotification("WeightUnit");
                    break;
                case "Certificate":
                case "CertStart":
                case "CertStop":
                    PropertyChangedNotification("CertificateFull");
                    break;
                case "WeightNet":
                case "PriceKG":
                    PropertyChangedNotification("PriceKGFrom");
                    PropertyChangedNotification("PriceTotal");
                    PropertyChangedNotification("TaxKG");
                    PropertyChangedNotification("TaxTotal");
                    PropertyChangedNotification("TaxVAT");
                    PropertyChangedNotification("TaxKG2009");
                    PropertyChangedNotification("TaxTotal2009");
                    PropertyChangedNotification("TaxVAT2009");
                    break;
                case "PricePC":
                    PropertyChangedNotification("PricePCFrom");
                    PropertyChangedNotification("PriceTotal");
                    PropertyChangedNotification("TaxVAT");
                    PropertyChangedNotification("TaxVAT2009");
                    break;
                case "RateAdd":
                    PropertyChangedNotification("TaxKG");
                    PropertyChangedNotification("TaxTotal");
                    PropertyChangedNotification("TaxVAT");
                    break;
                case "RateVat":
                    PropertyChangedNotification("RateVatPer");
                    break;
                case "RatePer":
                    PropertyChangedNotification("RatePerPer");
                    PropertyChangedNotification("TaxPer");
                    PropertyChangedNotification("TaxTotal");
                    PropertyChangedNotification("TaxVAT");
                    break;
                case "RateAdd2009":
                    PropertyChangedNotification("TaxKG2009");
                    PropertyChangedNotification("TaxTotal2009");
                    PropertyChangedNotification("TaxVAT2009");
                    break;
                case "RatePer2009":
                    PropertyChangedNotification("RatePer2009Per");
                    PropertyChangedNotification("TaxPer2009");
                    PropertyChangedNotification("TaxTotal2009");
                    PropertyChangedNotification("TaxVAT2009");
                    break;
                case "State":
                    PropertyChangedNotification("StateColor");
                    break;
            }
        }
        protected override void InitProperties()
        {
            myname = this.DomainObject.Name;
            myquantity = this.DomainObject.Quantity;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Number":
                    this.DomainObject.Number = (int?)value;
                    break;
                case "CNCode":
                    this.DomainObject.CNCode = (string)value;
                    break;
                case "Name":
                    if (myname != this.DomainObject.Name)
                        myname = this.DomainObject.Name;
                    else
                        this.Name = (string)value;
                    break;
                case "Quantity":
                    if (myquantity != this.DomainObject.Quantity)
                        myquantity = this.DomainObject.Quantity;
                    else
                        this.Quantity = (int)value;
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
                case "SubCountry":
                    this.DomainObject.SubCountry = (string)value;
                    break;
                case "Translation":
                    this.DomainObject.Translation = (string)value;
                    break;
                case "Composition":
                    this.DomainObject.Composition = (string)value;
                    break;
                case "Sizes":
                    this.DomainObject.Sizes = (string)value;
                    break;
                case "Decree":
                    this.DomainObject.Decree = (string)value;
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
                case "WeightNet":
                    this.DomainObject.WeightNet = (decimal?)value;
                    break;
                case "WeightGross":
                    this.DomainObject.WeightGross = (decimal?)value;
                    break;
                case "PriceKG":
                    this.DomainObject.PriceKG = (decimal?)value;
                    break;
                case "PricePC":
                    this.DomainObject.PricePC = (decimal?)value;
                    break;
                case "RateVat":
                    this.DomainObject.RateVat = (decimal?)value;
                    break;
                case "RatePer":
                    this.DomainObject.RatePer = (decimal?)value;
                    break;
                case "RateAdd":
                    this.DomainObject.RateAdd = (decimal?)value;
                    break;
                case "RatePer2009":
                    this.DomainObject.RatePer = (decimal?)value;
                    break;
                case "RateAdd2009":
                    this.DomainObject.RateAdd = (decimal?)value;
                    break;
                case "Note":
                    this.DomainObject.Note = (string)value;
                    break;
                case "State":
                    this.DomainObject.State = (int)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    if (mynotes != null)
                    {
                        Specification.RequestItemNote[] additem = new Specification.RequestItemNote[mynotes.Count];
                        foreach (Specification.RequestItemNote item in mynotes)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            { additem[i] = item; i++; }
                            else if (item.DomainState == lib.DomainObjectState.Deleted)
                            {
                                mynotes.EditItem(item);
                                item.DomainState = lib.DomainObjectState.Unchanged;
                                mynotes.CommitEdit();
                            }
                        }

                        for (int ii = 0; ii < i; ii++) mynotes.Remove(additem[ii]);
                    }
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
                case "Quantity":
                    if (!(myquantity > 0))
                    {
                        errmsg = "Количество должно быть больше 0.";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return myname!= base.DomainObject.Name || myquantity!= base.DomainObject.Quantity;
        }

        public delegate void TotalValueChangedDelegate(string propertyname, decimal? oldvalue, decimal? newvalue);
        public event TotalValueChangedDelegate TotalValueChanged;
        private void OnTotalValueChanged(string propertyname, decimal? oldvalue, decimal? newvalue)
        {
            if (TotalValueChanged != null)
                TotalValueChanged(propertyname, oldvalue, newvalue);
        }
    }

    internal class RequestItemSynchronizer : lib.ModelViewCollectionsSynchronizer<RequestItem, RequestItemVM>
    {
        protected override RequestItem UnWrap(RequestItemVM wrap)
        {
            return wrap.DomainObject as RequestItem;
        }

        protected override RequestItemVM Wrap(RequestItem fill)
        {
            return new RequestItemVM(fill);
        }
    }

    public class RequestItemViewCommand : lib.ViewModelViewCommand
    {
        internal static RequestItemViewCommand GetThis(int requestid)
        { return new RequestItemViewCommand(requestid); }

        public RequestItemViewCommand(int requestid) : base()
        {
            mydbm = new RequestItemDBM();
            mydbm.RequestId = requestid;
            mydbm.Fill();
            mysync = new RequestItemSynchronizer();
            mysync.DomainCollection = mydbm.Collection;
            base.Collection = mysync.ViewModelCollection;
            base.Items.MoveCurrentToPosition(-1);
            myexhandler = new lib.ExceptionHandler("Сохранение изменений");
            base.DeleteQuestionHeader = "Удалить выделенные строки?";
            myexcelimport = new RelayCommand(ExcelImportExec, ExcelImportCanExec);
            myexcelimportready = new RelayCommand(ExcelImportReadyExec, ExcelImportReadyCanExec);
            myexcelexport = new RelayCommand(ExcelExportExec, ExcelExportCanExec);
            myprocessing = new RelayCommand(ProcessingExec, ProcessingCanExec);
            foreach(RequestItemVM item in mysync.ViewModelCollection)
            { item.TotalValueChanged += Item_TotalValueChanged; }
            mysync.ViewModelCollection.CollectionChanged += ViewModelCollection_CollectionChanged;
            CalcPays();
        }

        private new RequestItemDBM mydbm;
        private RequestItemSynchronizer mysync;
        private lib.ExceptionHandler myexhandler;
        private System.ComponentModel.BackgroundWorker mybw;
        private System.Threading.Tasks.Task ProcessingTask;
        private ExcelImportWin myExcelImportWin;

        private decimal myquantity;
        public decimal Quantity
        {
            get { return myquantity; }
        }
        private decimal myweightnet;
        public decimal WeightNet
        {
            get { return myweightnet; }
        }
        private decimal myweightgross;
        public decimal WeightGross
        {
            get { return myweightgross; }
        }
        private decimal mypricetotal;
        public decimal PriceTotal
        {
            get
            {
                return mypricetotal;
            }
        }
        private decimal mytaxtotal;
        public decimal TaxTotal
        {
            get
            {
                return mytaxtotal;
            }
        }
        private decimal mytaxvat;
        public decimal TaxVAT
        {
            get
            {
                return mytaxvat;
            }
        }
        private decimal mytaxtotal2;
        public decimal TaxTotal2
        {
            get
            {
                return mytaxtotal2;
            }
        }
        private decimal mytaxvat2;
        public decimal TaxVAT2
        {
            get
            {
                return mytaxvat2;
            }
        }
        private decimal mytaxcert;
        public decimal TaxCert
        {
            get
            {
                return mytaxcert;
            }
        }

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
                    string[] arg = { "true", fd.FileName, (System.Windows.MessageBox.Show("Пропускать уже имеющиеся позиции (по номеру)?\nИмеющиеся позиции не будут обновлены значениями из файла.", "Загрузка данных", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes).ToString(), 0.ToString() };
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

        private RequestDS.tableRequestRow myrequest;
        private RelayCommand myexcelimportready;
        public ICommand ExcelImportReady
        {
            get { return myexcelimportready; }
        }
        private void ExcelImportReadyExec(object parametr)
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
                    myrequest = (parametr as System.Data.DataRowView).Row as RequestDS.tableRequestRow;
                    string[] arg = { "true", fd.FileName, (System.Windows.MessageBox.Show("Пропускать уже имеющиеся позиции (по номеру)?\nИмеющиеся позиции не будут обновлены значениями из файла.", "Загрузка данных", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes).ToString(), 1.ToString() };
                    mybw.RunWorkerAsync(arg);
                }
                else
                {
                    System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
                }
            }
        }
        private bool ExcelImportReadyCanExec(object parametr)
        { return true; }

        private RelayCommand myexcelexport;
        public ICommand ExcelExport
        {
            get { return myexcelexport; }
        }
        private void ExcelExportExec(object parametr)
        {
            myrequest = (parametr as System.Data.DataRowView).Row as RequestDS.tableRequestRow;
            if (!(myrequest.IscustomerIdNull() | myrequest.IsstoragePointNull()))
            {
                ProducerSelectWin win = new ProducerSelectWin();
                win.Client = myrequest.customerId;
                bool? res = win.ShowDialog();
                if (res.HasValue && res.Value & !string.IsNullOrEmpty(win.SelectProducer))
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
                        string[] arg = { "false", win.SelectProducer };
                        mybw.RunWorkerAsync(arg);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
                    }
                }
            }
            else
                System.Windows.MessageBox.Show("Необходимо указать клиента, позицию по складу и вес по документам.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);

        }
        private bool ExcelExportCanExec(object parametr)
        { return true; }

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
                    if (byte.Parse(args[3]) == 0)
                        e.Result = OnExcelImport(worker, exApp, args[1], bool.Parse(args[2]));
                    else
                        e.Result = OnExcelImportReady(worker, exApp, args[1], bool.Parse(args[2]), myrequest, 1, 1);
                else
                    e.Result = OnExcelExport(worker, exApp, args[1]);
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
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                myExcelImportWin.MessageTextBlock.Text = "Обработка прервана из-за ошибки" + "\n" + e.Error.Message;
            }
            else
            {
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                myExcelImportWin.MessageTextBlock.Text = "Обработка выполнена успешно." + "\n" + e.Result.ToString() + " строк обработано";
            }
        }
        private void BackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            myExcelImportWin.ProgressBar1.Value = e.ProgressPercentage;
        }

        private int OnExcelImport(BackgroundWorker worker, Excel.Application exApp, string filepath, bool ismiss)
        {
            bool exist;
            int maxr, n, n1;
            decimal m;
            DateTime d;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            RequestItem newitem = null;

            Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
            Excel.Worksheet exWh = exWb.Sheets[1];

            maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            for (int r = 12; r <= maxr; r++)
            {
                newitem = null;

                if (string.IsNullOrEmpty((exWh.Cells[r, 3].Text as string).Trim()))
                {
                    maxr = r - 12;
                    break;
                }

                exist = true;
                str.Clear();
                str.Append((exWh.Cells[r, 1].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (int.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out n))
                    {
                        foreach (RequestItemVM item in mysync.ViewModelCollection)
                            if (item.DomainObject.Number == n)
                            {
                                newitem = item.DomainObject;
                                break;
                            }
                        if (newitem == null)
                        {
                            exist = false;
                            newitem = new RequestItem();
                            newitem.RequestId = mydbm.RequestId;
                            newitem.Number = n;
                        }
                        else if (ismiss) continue;
                    }
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 1].Address(false, false) + " как целое число!");
                }

                str.Clear();
                str.Append((exWh.Cells[r, 2].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 10)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 2].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.CNCode = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 3].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 1000)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 1].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Name = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 4].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 250)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 4].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Composition = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 5].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 100)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 5].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Brand = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 6].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 100)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 6].Address(false, false) + " содержит слишком длинный текст!");
                newitem.Producer = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 7].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 50)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 7].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.TitleCountry = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 8].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (int.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out n))
                        newitem.Quantity = n;
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 8].Address(false, false) + " как целое число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 10].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.WeightGross = m;
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 10].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 11].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 50)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 11].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Sizes = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 12].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 0)
                {
                    n = str.ToString().IndexOf(" от");
                    if (n > 0)
                    {
                        newitem.Certificate = str.ToString().Substring(0, n);
                        n1 = str.Length - n - 4;
                        if (n1 > 9 && DateTime.TryParseExact(str.ToString().Substring(n + 4, 10), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                            newitem.CertStart = d;
                        else if (n1 > 7 && DateTime.TryParseExact(str.ToString().Substring(n + 4, 8), "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                            newitem.CertStart = d;
                        n = str.ToString().IndexOf(" до");
                        if (n > 0)
                        {
                            n1 = str.Length - n - 4;
                            if (n1 > 9 && DateTime.TryParseExact(str.ToString().Substring(n + 4, 10), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                                newitem.CertStop = d;
                            else if (n1 > 7 && DateTime.TryParseExact(str.ToString().Substring(n + 4, 8), "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                                newitem.CertStop = d;
                        }
                    }
                    else
                    {
                        newitem.Certificate = str.ToString();
                    }
                }
                str.Clear();
                str.Append((exWh.Cells[r, 13].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 250)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 13].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Note = str.ToString();

                if (!exist) this.myview.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<RequestItemVM>(mysync.ViewModelCollection.Add), new RequestItemVM(newitem));
                worker.ReportProgress((int)(decimal.Divide(r - 11, maxr - 11) * 100));
                //r = 0;
                //RequestItemVM item1,item2;
                //for (int i=0;i< mysync.ViewModelCollection.Count;i++ )
                //{
                //    item1 = mysync.ViewModelCollection[i];
                //    for (int j = i + 1; j < mysync.ViewModelCollection.Count; j++)
                //    {
                //        item2 = mysync.ViewModelCollection[j];
                //        if (
                //            string.Equals(item1.CNCode, item2.CNCode)
                //            && string.Equals(item1.Name, item2.Name)
                //            && string.Equals(item1.Brand, item2.Brand)
                //            && string.Equals(item1.TitleCountry, item2.TitleCountry)
                //            && string.Equals(item1.Composition, item2.Composition)
                //            && item1.PriceKG.HasValue == item2.PriceKG.HasValue && (!item1.PriceKG.HasValue || decimal.Equals(item1.PriceKG, item2.PriceKG))
                //           )
                //        {

                //        }
                //    }

                //        worker.ReportProgress((int)(50M + decimal.Divide(i + 1, mysync.ViewModelCollection.Count) * 20));
                //}
            }
            ProgressChange(95);
            CalcPays();
            worker.ReportProgress(100);
            exWb.Close();
            return maxr;
        }
        internal int OnExcelImportReady(BackgroundWorker worker, Excel.Application exApp, string filepath, bool ismiss, System.Data.DataRow request, int num, int count)
        {
            bool exist;
            int maxr, n, n1;
            decimal m;
            DateTime d;
            System.Text.StringBuilder str = new System.Text.StringBuilder();
            RequestItem newitem = null;

            Excel.Workbook exWb = exApp.Workbooks.Open(filepath, false, true);
            Excel.Worksheet exWh = exWb.Sheets["Для расчета"];

            str.Clear();
            str.Append((exWh.Cells[4, 14].Text as string).Trim());
            if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                request["eurousd"] = m;
            else
                throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[14, 4].Address(false, false) + " как число!");

            int r;
            maxr = exWh.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            for (r = 7; r <= maxr; r++)
            {
                newitem = null;

                if (string.IsNullOrEmpty((exWh.Cells[r, 4].Text as string).Trim()))
                {
                    maxr = r - 7;
                    break;
                }

                exist = true;
                str.Clear();
                str.Append((exWh.Cells[r, 1].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (int.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out n))
                    {
                        foreach (RequestItemVM item in mysync.ViewModelCollection)
                            if (item.DomainObject.Number == n)
                            {
                                newitem = item.DomainObject;
                                break;
                            }
                        if (newitem == null)
                        {
                            exist = false;
                            newitem = new RequestItem();
                            newitem.RequestId = mydbm.RequestId;
                            newitem.Number = n;
                        }
                        else if (ismiss) continue;
                    }
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 1].Address(false, false) + " как целое число!");
                }

                str.Clear();
                str.Append((exWh.Cells[r, 3].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 1000)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 3].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Name = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 4].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 10)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 4].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.CNCode = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 5].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (int.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out n))
                        newitem.Quantity = n;
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 5].Address(false, false) + " как целое число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 8].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.WeightGross = m;
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 8].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 9].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.PriceKG = m;
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 9].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 10].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.PricePC = m;
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 10].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 14].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.RatePer = m > 1M ? decimal.Divide(m, 100M) : m;
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 14].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 15].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.RateAdd = m;
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 15].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 16].Text as string).Trim());
                if (str.Length > 0)
                {
                    if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                        newitem.RateVat = m > 1M ? decimal.Divide(m, 100M) : m;
                    else
                        throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 16].Address(false, false) + " как число!");
                }
                str.Clear();
                str.Append((exWh.Cells[r, 21].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 0)
                {
                    n = str.ToString().IndexOf(" от");
                    if (n > 0)
                    {
                        newitem.Certificate = str.ToString().Substring(0, n);
                        n1 = str.Length - n - 4;
                        if (n1 > 9 && DateTime.TryParseExact(str.ToString().Substring(n + 4, 10), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                            newitem.CertStart = d;
                        else if (n1 > 7 && DateTime.TryParseExact(str.ToString().Substring(n + 4, 8), "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                            newitem.CertStart = d;
                        n = str.ToString().IndexOf(" до");
                        if (n > 0)
                        {
                            n1 = str.Length - n - 4;
                            if (n1 > 9 && DateTime.TryParseExact(str.ToString().Substring(n + 4, 10), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                                newitem.CertStop = d;
                            else if (n1 > 7 && DateTime.TryParseExact(str.ToString().Substring(n + 4, 8), "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
                                newitem.CertStop = d;
                        }
                    }
                    else
                    {
                        newitem.Certificate = str.ToString();
                    }
                }
                str.Clear();
                str.Append((exWh.Cells[r, 22].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 100)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 22].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Decree = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 23].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 100)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 23].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Translation = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 24].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 100)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 24].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Brand = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 25].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 250)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 25].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Composition = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 26].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 50)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 26].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Sizes = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 27].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 100)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 27].Address(false, false) + " содержит слишком длинный текст!");
                newitem.Producer = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 28].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 50)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 28].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.TitleCountry = str.ToString();
                str.Clear();
                str.Append((exWh.Cells[r, 29].Text as string).Trim()).Replace((char)13, ' ').Replace((char)10, ' ').Replace("  ", " ");
                if (str.Length > 250)
                    throw new ApplicationException(exWb.Name + "\nЯчейки Excel " + exWh.Cells[r, 29].Address(false, false) + " содержит слишком длинный текст!");
                else
                    newitem.Note = str.ToString();

                if (!exist) this.myview.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<RequestItemVM>(mysync.ViewModelCollection.Add), new RequestItemVM(newitem));
                worker.ReportProgress((int)(decimal.Add(decimal.Divide(num - 1, count), decimal.Divide(r - 6, (maxr - 6) * count)) * 100));
            }
            r++;
            str.Clear();
            str.Append((exWh.Cells[r, 21].Text as string).Trim());
            if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                request["pay1"] = m;
            else
                throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWh.Cells[r, 21].Address(false, false) + " как число!");
            str.Clear();
            str.Append((exWb.Sheets["2009"].Cells[r, 21].Text as string).Trim());
            if (decimal.TryParse(str.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out m))
                request["customspay"] = m;
            else
                throw new ApplicationException(exWb.Name + "\nНе удалось разобрать значение ячейки Excel " + exWb.Sheets["2009"].Cells[r, 21].Address(false, false) + " как число!");
            request["specloaded"] = true;
            request.EndEdit();

            worker.ReportProgress((int)100 * num / count);
            exWb.Close();
            return maxr;
        }
        private int OnExcelExport(BackgroundWorker worker, Excel.Application exApp, string producer)
        {
            Excel.Workbook exWb;
            try
            {
                RequestDS ds = new RequestDS();
                RequestDSTableAdapters.tableGetAllGoodsTableAdapter adapter = new RequestDSTableAdapters.tableGetAllGoodsTableAdapter();
                RequestDS.tableGetAllGoodsDataTable table = adapter.GetData(myrequest.customerId, producer);
                int row = 12;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Environment.CurrentDirectory + @"\Templates\GetAllGoods.xltx");
                Excel.Worksheet exWh = exWb.Sheets[1];

                foreach (RequestDS.tableGetAllGoodsRow item in table)
                {
                    if (row > 12) exWh.Rows[row].Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                    exWh.Cells[row, 1] = row - 11;
                    //exWh.Cells[row, 2] = item.CNCode;
                    exWh.Cells[row, 3] = item.description;
                    //exWh.Cells[row, 4] = item.Composition;
                    //exWh.Cells[row, 5] = item.Translation;
                    exWh.Cells[row, 6] = producer;
                    //exWh.Cells[row, 7] = item.AddPer.HasValue ? item.AddPer.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 8] = item.Category1Brand.HasValue ? item.Category1Brand.Value.ToString("0.#####") : string.Empty;
                    exWh.Cells[row, 9].Formula = "=J" + row.ToString() + "/H" + row.ToString();
                    //exWh.Cells[row, 10] = item.Category3Brand.HasValue ? item.Category3Brand.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 11] = item.Category3No.HasValue ? item.Category3No.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 12] = item.Category4Brand.HasValue ? item.Category4Brand.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 13] = item.Category4No.HasValue ? item.Category4No.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 14] = item.Category5Brand.HasValue ? item.Category5Brand.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 15] = item.Category5No.HasValue ? item.Category5No.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 16] = item.Category2Brand.HasValue ? item.Category2Brand.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 17] = item.Category2No.HasValue ? item.Category2No.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 18] = item.RateVat.HasValue ? item.RateVat.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 19] = item.RatePer.HasValue ? item.RatePer.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 20] = item.RateAdd.HasValue ? item.RateAdd.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 21] = item.RateDate.ToString("dd.MM.yy");
                    //exWh.Cells[row, 22] = item.RatePer2009.HasValue ? item.RatePer2009.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 23] = item.RateAdd2009.HasValue ? item.RateAdd2009.Value.ToString("0.#####") : string.Empty;
                    //exWh.Cells[row, 24] = item.Risk;
                    //exWh.Cells[row, 25] = item.Note;
                    //exWh.Cells[row, 26] = item.Id;

                    row++;
                }
                exWh.Rows[row].Delete();
                exWh.Cells[row, 8].Formula = "=SUM(H12:H" + (row - 1).ToString() + ")";
                exWh.Cells[row, 10].Formula = "=SUM(J12:J" + (row - 1).ToString() + ")";

                exWb.SaveAs(@"V:\Спецификации\" + (myrequest.storagePoint + "_" + myrequest.customerName + "_" + producer + "_" + myrequest.officialWeight.ToString("N0")).Replace("\\", string.Empty).Replace("/", string.Empty).Replace(":", string.Empty).Replace("*", string.Empty).Replace("?", string.Empty).Replace("\"", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty).Replace("|", string.Empty).Replace(".", string.Empty));

                exApp.Visible = true;
                exWh = null;
                return row - 12;
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

        private RelayCommand myprocessing;
        public ICommand Processing
        {
            get { return myprocessing; }
        }
        private void ProcessingExec(object parametr)
        {
            if (ProcessingTask == null || ProcessingTask.IsCompleted)
            {
                this.myendedit();
                myrequest = (parametr as System.Data.DataRowView).Row as RequestDS.tableRequestRow;
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
                ProcessingTask = ProcessingAsync();
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private bool ProcessingCanExec(object parametr)
        { return true; }
        private async Task ProcessingAsync()
        {
            Task<string> t = Task<string>.Run(() => DoProcessing());
            try { await (t); }
            catch { }
            if (t.Exception != null)
            {
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                myExcelImportWin.MessageTextBlock.Text += "Загрузка прервана из-за ошибки:" + "\n" + (t.Exception.InnerException == null ? t.Exception.Message : t.Exception.InnerException.Message);
            }
            else
            {
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                myExcelImportWin.MessageTextBlock.Text = "Загрузка выполнена успешно." + "\n" + t.Result;
            }
        }
        private string DoProcessing()
        {
            Specification.MappingDBM mdbm = new Specification.MappingDBM();
            mdbm.Fill();
            ProgressChange(5);
            Specification.GoodsDescription descr = new Specification.GoodsDescription();
            descr.Mappings = mdbm.Collection;
            GoodsDBM gdbm = new GoodsDBM();
            gdbm.Fill();
            System.Windows.Data.ListCollectionView goods = new System.Windows.Data.ListCollectionView(gdbm.Collection);
            List<Goods> findgoods = new List<Goods>();
            ProgressChange(10);
            bool isdelete = false;
            string str, find;
            Specification.Material mtr;
            int r = 0;
            foreach (object item in Items)
            {
                if (!(item is RequestItemVM)) continue;
                RequestItemVM ritem = item as RequestItemVM;
                descr.ClientDescription = ritem.Name;
                descr.ClientComposition = ritem.Composition;
                if (string.IsNullOrEmpty(descr.Error))
                {
                    goods.Filter = delegate (object objitem)
                    {
                        Goods gitem = objitem as Goods;
                        return Specification.MappingViewCommand.ViewFilterDefault(gitem)
                            && object.Equals(gitem.Gender, descr.Gender)
                            && (gitem.ContextureNote == descr.Mapping.TNVEDGroup || gitem.ContextureNote.IndexOf(descr.Mapping.TNVEDGroup + ", ") > -1 || gitem.ContextureNote.IndexOf(", " + descr.Mapping.TNVEDGroup) > -1)
                            && (string.Equals(gitem.Brand.ToLower(), ritem.Brand.ToLower()) || gitem.Brand.ToLower().IndexOf(ritem.Brand.ToLower() + ", ") > -1 || gitem.Brand.ToLower().IndexOf(", " + ritem.Brand.ToLower()) > -1);
                    };
                    if (goods.Count > 0)
                    {
                        findgoods.Clear();
                        find = descr.GoodsName.ToLower();
                        foreach (Goods gitem in goods)
                            if (string.Equals(gitem.Name.ToLower(), find) || gitem.Name.ToLower().IndexOf(find + ", ") > -1 || gitem.Name.ToLower().IndexOf(", " + find) > -1) findgoods.Add(gitem);
                        if (findgoods.Count == 0)
                        {
                            foreach (Goods gitem in goods)
                                if (string.Equals(gitem.Name.ToLower(), descr.Mapping.Goods.ToLower()) || gitem.Name.ToLower().IndexOf(descr.Mapping.Goods.ToLower() + ", ") > -1 || gitem.Name.ToLower().IndexOf(", " + descr.Mapping.Goods.ToLower()) > -1) findgoods.Add(gitem);
                            if (findgoods.Count == 0)
                            {
                                foreach (Goods gitem in goods)
                                    foreach (Specification.GoodsSynonym gs in descr.Mapping.Synonyms)
                                        if (string.Equals(gitem.Name.ToLower(), gs.Name.ToLower()) || gitem.Name.ToLower().IndexOf(gs.Name.ToLower() + ", ") > -1 || gitem.Name.ToLower().IndexOf(", " + gs.Name.ToLower()) > -1)
                                        {
                                            findgoods.Add(gitem);
                                            break;
                                        }
                            }
                        }
                        if (findgoods.Count > 0)
                        {
                            if (descr.MaxPart != null)
                            {
                                mtr = descr.GetMaterial(descr.MaxPart.PartName);
                                if (mtr != null)
                                {
                                    isdelete = false;
                                    find = mtr.GoodsName.ToLower();
                                    for (int i = findgoods.Count - 1; i > -1; i--)
                                    {
                                        str = findgoods[i].Contexture.ToLower();
                                        if (str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1)
                                        {
                                            if (!isdelete)
                                            {
                                                isdelete = true;
                                                for (int d = findgoods.Count - 1; d > i; d--)
                                                    findgoods.RemoveAt(d);
                                            }
                                        }
                                        else if (isdelete)
                                            findgoods.RemoveAt(i);
                                    }
                                    if (!isdelete)
                                    {
                                        find = "п" + find;
                                        for (int i = findgoods.Count - 1; i > -1; i--)
                                        {
                                            str = findgoods[i].Contexture.ToLower();
                                            if (str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1)
                                            {
                                                if (!isdelete)
                                                {
                                                    isdelete = true;
                                                    for (int d = findgoods.Count - 1; d > i; d--)
                                                        findgoods.RemoveAt(d);
                                                }
                                            }
                                            else if (isdelete)
                                                findgoods.RemoveAt(i);
                                        }
                                    }
                                    while (!isdelete & !string.IsNullOrEmpty(mtr.Upper?.GoodsName))
                                    {
                                        mtr = mtr.Upper;
                                        find = mtr.GoodsName.ToLower();
                                        for (int i = findgoods.Count - 1; i > -1; i--)
                                        {
                                            str = findgoods[i].Contexture.ToLower();
                                            if (str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1)
                                            {
                                                if (!isdelete)
                                                {
                                                    isdelete = true;
                                                    for (int d = findgoods.Count - 1; d > i; d--)
                                                        findgoods.RemoveAt(d);
                                                }
                                            }
                                            else if (isdelete)
                                                findgoods.RemoveAt(i);
                                        }
                                    }
                                    if (!isdelete)
                                    {
                                        mtr = descr.GetMaterial(descr.MaxPart.PartName);
                                        find = mtr.GoodsName.ToLower() + "+";
                                        for (int i = findgoods.Count - 1; i > -1; i--)
                                        {
                                            str = findgoods[i].Contexture.ToLower();
                                            if (str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1)
                                            {
                                                if (!isdelete)
                                                {
                                                    isdelete = true;
                                                    for (int d = findgoods.Count - 1; d > i; d--)
                                                        findgoods.RemoveAt(d);
                                                }
                                            }
                                            else if (isdelete)
                                                findgoods.RemoveAt(i);
                                        }
                                    }
                                    if (isdelete)
                                    {
                                        isdelete = false;
                                        for (int i = findgoods.Count - 1; i > -1; i--)
                                        {
                                            if (Specification.GoodsDescription.StrsInStrs(findgoods[i].Material, descr.Material?.ShortName, ',', ' '))
                                            {
                                                if (!isdelete)
                                                {
                                                    isdelete = true;
                                                    for (int d = findgoods.Count - 1; d > i; d--)
                                                        findgoods.RemoveAt(d);
                                                }
                                            }
                                            else if (isdelete)
                                                findgoods.RemoveAt(i);
                                        }
                                        if (!isdelete & descr.Material?.Substitution != null)
                                        {
                                            for (int i = findgoods.Count - 1; i > -1; i--)
                                            {
                                                if (Specification.GoodsDescription.StrsInStrs(findgoods[i].Material, descr.Material.Substitution.ShortName, ',', ' '))
                                                {
                                                    if (!isdelete)
                                                    {
                                                        isdelete = true;
                                                        for (int d = findgoods.Count - 1; d > i; d--)
                                                            findgoods.RemoveAt(d);
                                                    }
                                                }
                                                else if (isdelete)
                                                    findgoods.RemoveAt(i);
                                            }
                                            if (isdelete)
                                            {
                                                descr.Material = descr.Material.Substitution;
                                                ritem.Name.Replace(descr.ClientMaterial, descr.Material.Name);
                                                ritem.State = ritem.State ^ 1;
                                            }
                                        }
                                        if (isdelete)
                                        {
                                            //List<string> producers = new List<string>();
                                            //for (int i = findgoods.Count - 1; i > -1; i--)
                                            //{
                                            //    if (!producers.Contains(findgoods[i].Producer)) producers.Add(findgoods[i].Producer);
                                            //}
                                            if (findgoods.Count > 1)
                                            {
                                                Goods choiced = App.Current.Dispatcher.Invoke<Goods>(delegate ()
                                                {
                                                    GoodsСhoiceWin win = new GoodsСhoiceWin();
                                                    win.DataContext = findgoods;
                                                    win.ShowDialog();
                                                    return win.GoodsСhoiced;
                                                });
                                                if (choiced != null)
                                                    for (int i = 0; i < findgoods.Count; i++)
                                                        if (!findgoods[i].Equals(choiced)) findgoods.Remove(findgoods[i]);
                                            }
                                            if (findgoods.Count == 1)
                                            {
                                                find = ritem.TitleCountry.ToLower();
                                                str = findgoods[0].TitleCountry.ToLower();
                                                if (!(str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1))
                                                {
                                                    str = findgoods[0].Cat1.ToLower();
                                                    if (!(str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1))
                                                    {
                                                        str = findgoods[0].Cat3.ToLower();
                                                        if (!(str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1))
                                                        {
                                                            str = findgoods[0].Cat4.ToLower();
                                                            if (!(str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1))
                                                            {
                                                                str = findgoods[0].Cat5.ToLower();
                                                                if (!(str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1))
                                                                {
                                                                    CustomBrokerWpf.Domain.References.Country country = null;
                                                                    foreach (CustomBrokerWpf.Domain.References.Country citem in CustomBrokerWpf.References.Countries)
                                                                    {
                                                                        if (string.Equals(find, citem.Name.Trim().ToLower()) | string.Equals(find, citem.FullName.Trim().ToLower()))
                                                                        {
                                                                            country = citem;
                                                                        }
                                                                        else
                                                                        {
                                                                            string[] strs = citem.Synonym.ToLower().Split(',');
                                                                            foreach (string stritem in strs)
                                                                                if (string.Equals(find, stritem.Trim().ToLower()))
                                                                                {
                                                                                    country = citem;
                                                                                    break;
                                                                                }
                                                                        }
                                                                        if (country != null) break;
                                                                    }
                                                                    if (country?.PriceCategory != null)
                                                                    {
                                                                        string[] strs;
                                                                        switch (country.PriceCategory)
                                                                        {
                                                                            case 3:
                                                                                strs = findgoods[0].Cat1.Split(',');
                                                                                descr.CountryCategory = 1;
                                                                                break;
                                                                            case 4:
                                                                                strs = findgoods[0].Cat3.Split(',');
                                                                                descr.CountryCategory = 3;
                                                                                break;
                                                                            case 5:
                                                                                strs = findgoods[0].Cat4.Split(',');
                                                                                descr.CountryCategory = 4;
                                                                                break;
                                                                            default:
                                                                                strs = new string[] { };
                                                                                break;
                                                                        }
                                                                        if (strs.Length > 0)
                                                                        {
                                                                            ritem.PutErrDescription("cngcnt","Cтрана изменена " + ritem.TitleCountry + " изменена на страну " + strs[0] + " из более высокой категории ");
                                                                            ritem.SubCountry = ritem.TitleCountry;
                                                                            ritem.TitleCountry = strs[0];
                                                                            ritem.State = ritem.State ^ 2;
                                                                        }
                                                                        else
                                                                        {
                                                                            descr.CountryCategory = 0;
                                                                            ritem.PutErrDescription("cngcnt", "Нет стран в более высокой категории, для страны: " + ritem.TitleCountry);
                                                                            continue;
                                                                        }
                                                                        ritem.PutErrDescription("notcat", string.Empty);
                                                                    }
                                                                    else
                                                                    {
                                                                        ritem.PutErrDescription("notcat","Не удалось определить категорию страны: " + ritem.TitleCountry);
                                                                        continue;
                                                                    }
                                                                }
                                                                else
                                                                    descr.CountryCategory = 5;
                                                            }
                                                            else
                                                                descr.CountryCategory = 4;
                                                        }
                                                        else
                                                            descr.CountryCategory = 3;
                                                    }
                                                    else
                                                        descr.CountryCategory = 1;
                                                }

                                                ritem.Certificate = findgoods[0].Certificate;
                                                ritem.CertStart = findgoods[0].CertStart;
                                                ritem.CertStop = findgoods[0].CertStop;
                                                ritem.Producer = findgoods[0].Producer;
                                                ritem.PutErrDescription("cnt!=1", string.Empty);
                                            }
                                            else
                                                ritem.PutErrDescription("cnt!=1","В базе ДС найдено более одного соответствия (ДС невыбран)");
                                            ritem.PutErrDescription("notmat", string.Empty);
                                        }
                                        else
                                            ritem.PutErrDescription("notmat","Не найден материал в базе ДС:" + descr.Material?.Name);
                                        ritem.PutErrDescription("notcmp", string.Empty);
                                    }
                                    else
                                        ritem.PutErrDescription("notcmp", "Не найдена ткань в базе ДС:" + mtr.GoodsName + " (" + mtr.Name + ")");
                                    ritem.PutErrDescription("nthcmp", string.Empty);
                                }
                                else
                                    ritem.PutErrDescription("nthcmp","Не удалось идентифицировать основной состав: " + descr.MaxPart?.PartName);
                            }
                            else
                                ritem.PutErrDescription("nthcmp", "Не удалось разобрать состав: " + ritem.Composition);
                            ritem.PutErrDescription("notgds", string.Empty);
                        }
                        else
                            ritem.PutErrDescription("notgds","Не найден товар в базе ДС: " + descr.GoodsName + " (" + descr.Mapping.Goods + ")");
                        ritem.PutErrDescription("nottnv", string.Empty);
                    }
                    else
                        ritem.PutErrDescription("nottnv", "Не найдено в базе ДС: группа ТНВЭД " + descr.Mapping.TNVEDGroup + ", пол " + descr.Gender?.Name + ", бренд ");
                }
                else
                    ritem.ErrDescription = descr.Error;

                AllPriceDBM apdbm = new AllPriceDBM();
                apdbm.Fill();
                System.Windows.Data.ListCollectionView apview = new System.Windows.Data.ListCollectionView(apdbm.Collection);
                apview.Filter = delegate (object objitem)
                {
                    bool where = true;
                    int sp = -1, st = -1, gst = -1, gsp = -1;
                    string name;
                    AllPrice apitem = objitem as AllPrice;
                    if (string.IsNullOrEmpty(apitem.Spelling))
                        where = false;
                    if (where && apitem.Spelling[0] == '[')
                    {
                        sp = apitem.Spelling.IndexOf(']');
                        if (sp > 0)
                        {
                            name = apitem.Spelling.Substring(0, sp + 1).ToLower();
                            where = name.StartsWith('[' + descr.GoodsName.ToLower() + " ** ") || name.IndexOf(" ** " + descr.GoodsName.ToLower() + " ** ") > 0 || name.IndexOf(" ** " + descr.GoodsName.ToLower() + "]") > 0;
                        }
                    }
                    if (where)
                    {
                        if (apitem.Spelling[apitem.Spelling.Length - 1] == ']' & descr.Material != null)
                        {
                            st = apitem.Spelling.LastIndexOf('[');
                            if (st > 0)
                            {
                                name = apitem.Spelling.Substring(st).ToLower();
                                where = name.StartsWith('[' + descr.Material.ShortName.ToLower() + "* ") || name.IndexOf(" ** " + descr.Material.ShortName.ToLower() + "* ") > 0 || name.IndexOf(" ** " + descr.Material.ShortName.ToLower() + "*]") > 0;
                            }
                        }
                        if (where)
                        {
                            if (!(sp < 0 | st < 0))
                            {
                                if (descr.Gender == null)
                                    where = apitem.Spelling.Substring(sp + 1, st - sp - 1).Trim().Length == 0;
                                else
                                {
                                    name = apitem.Spelling.Substring(sp + 1, st - sp - 1).ToLower();
                                    find = descr.Gender?.ShortName.ToLower();
                                    where = name.IndexOf(" [" + find + "*") > -1 | name.IndexOf(" " + find + "*") > -1;
                                }
                            }
                            else
                            {
                                gsp = st < 0 ? apitem.Spelling.Length : st;
                                name = apitem.Spelling.Substring(sp + 1, gsp - sp - 1).ToLower();
                                if (descr.Gender != null)
                                {
                                    find = descr.Gender.ShortName.ToLower();
                                    gst = name.IndexOf(" [" + find + "*");
                                    if (gst > -1)
                                    {
                                        gsp = name.IndexOf("]", gst) + sp + 1;
                                        gst = gst + sp + 1;
                                    }
                                    else
                                    {
                                        gst = name.IndexOf(" " + find + "*");
                                        if (gst > -1)
                                        {
                                            gsp = name.IndexOf(" [");
                                            if (gsp > -1 & gsp < gst)
                                            {
                                                gst = gsp + sp + 1;
                                                gsp = name.IndexOf("]", gsp) + sp + 1;
                                            }
                                            else
                                            {
                                                gst = gst + sp + 1;
                                                gsp = gst + 1 + find.Length;
                                            }
                                        }
                                        else
                                            where = false;
                                    }
                                }
                                if (where)
                                {
                                    if (gst < 0 & descr.Material == null)
                                    {
                                        where = sp > 0 || string.Equals(descr.GoodsName.ToLower(), apitem.Spelling.ToLower());
                                    }
                                    else if (!(gst < 0) & descr.Material == null)
                                    {
                                        where = apitem.Spelling.Length == (gsp + 1) && (sp > 0 || string.Equals(descr.GoodsName.ToLower(), apitem.Spelling.Substring(0, gst).ToLower()));
                                    }
                                    else if (!(gst < 0) & descr.Material != null)
                                    {
                                        if (st < 0)
                                        {
                                            st = apitem.Spelling.IndexOf(' ', gsp);
                                            name = apitem.Spelling.Substring(st + 1).ToLower();
                                            where = st > 0 && (string.Equals(name, descr.Material.ShortName.ToLower() + "*"));
                                        }
                                        if (where & sp < 0)
                                        {
                                            where = string.Equals(descr.GoodsName.ToLower(), apitem.Spelling.Substring(0, gst).ToLower());
                                        }
                                    }
                                    else // нет пола, нужен материал
                                    {
                                        if (sp > 0)
                                        {
                                            where = string.Equals(apitem.Spelling.Substring(sp + 2).ToLower(), descr.Material.ShortName.ToLower() + "*");
                                        }
                                        else if (st > 0)
                                        {
                                            where = string.Equals(apitem.Spelling.Substring(0, st - 1).ToLower(), descr.GoodsName.ToLower());
                                        }
                                        else
                                            where = string.Equals(apitem.Spelling.ToLower(), descr.GoodsName.ToLower() + " " + descr.Material.ShortName.ToLower() + "*");
                                    }
                                }
                                if (where & descr.MaxPart == null) //состав
                                    where = string.IsNullOrEmpty(apitem.Composition);
                            }
                        }
                    }
                    return where;
                };
                List<AllPrice> findprice = new List<AllPrice>();
                foreach (AllPrice pitem in apview)
                    findprice.Add(pitem);
                if (descr.MaxPart != null)
                {
                    isdelete = false;
                    mtr = descr.GetMaterial(descr.MaxPart.PartName);
                    while (!isdelete & mtr != null)
                    {
                        find = mtr.Name.ToLower();
                        for (int i = findprice.Count - 1; i > -1; i--)
                        {
                            str = findprice[i].Composition.ToLower().Trim();
                            if (str == find | str.IndexOf(find + ", ") > -1 | str.IndexOf(", " + find) > -1)
                            {
                                if (!isdelete)
                                {
                                    isdelete = true;
                                    for (int d = findprice.Count - 1; d > i; d--)
                                        findprice.RemoveAt(d);
                                }
                            }
                            else if (isdelete)
                                findprice.RemoveAt(i);
                        }
                        if (!isdelete) mtr = mtr.Upper;
                    }
                    if (!isdelete) findprice.Clear();
                }
                else
                    mtr = null;
                if (findprice.Count == 1)
                {
                    int bst = -1, bsp = -1, gst = 0, gsp = -1;
                    string[] strs;
                    System.Text.StringBuilder strb = new System.Text.StringBuilder();
                    AllPrice apitem = findprice[0];
                    ritem.CNCode = apitem.Code;
                    ritem.Translation = apitem.Translation;
                    ritem.RateVat = apitem.RateVat;
                    ritem.RatePer = apitem.RatePer;
                    ritem.RatePer2009 = apitem.RatePer2009;
                    ritem.RateAdd = apitem.RateAdd;
                    ritem.RateAdd2009 = apitem.RateAdd2009;
                    if (descr.CountryCategory == 0 && !descr.RecognizeCountry(ritem.TitleCountry))
                        ritem.PutErrDescription("notcat", "Цены: Не удалось определить категорию страны: " + ritem.TitleCountry);
                    else
                    {
                        ritem.State = ritem.State ^ 4;
                        ritem.PutErrDescription("notcat", string.Empty);
                    }
                    if (descr.CountryCategory > 0)
                        switch (descr.CountryCategory)
                        {
                            case 1:
                                ritem.PriceKG = apitem.Category1No;
                                break;
                            case 3:
                                ritem.PriceKG = apitem.Category3No;
                                break;
                            case 4:
                                ritem.PriceKG = apitem.Category4No;
                                break;
                            case 5:
                                ritem.PriceKG = apitem.Category5No;
                                break;
                            default:
                                break;
                        }
                    bst = apitem.Name.IndexOf('[');
                    bsp = apitem.Name.IndexOf(']', bst + 1);
                    isdelete = false;
                    while (bst > -1)
                    {
                        strb.Append(apitem.Name.Substring(gst, bst - gst));
                        strs = apitem.Name.Substring(bst + 1, bsp - bst - 1).Split(new string[] { " ** " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string bstr in strs)
                        {
                            if (
                                (bst == 0 & string.Equals(bstr.ToLower(), descr.GoodsName.ToLower()))
                                || (descr.Gender != null & bstr.ToLower().IndexOf(descr.Gender.ShortName.ToLower()) > -1)
                                || (descr.Material != null & Specification.GoodsDescription.StrsInStrs(bstr.ToLower(), descr.Material.ShortName.ToLower(), ' ', ' '))
                                || (descr.MaxPart != null & Specification.GoodsDescription.StrsInStrs(bstr.ToLower(), mtr?.ShortName.ToLower(), ' ', ' '))
                                )
                            {
                                strb.Append(bstr);
                                bst = bsp;
                                break;
                            }
                        }
                        if (bst != bsp)
                        {
                            if (!isdelete) isdelete = true;
                            strb.Append(apitem.Name.Substring(bst + 1, bsp - bst - 1));
                        }
                        gsp = bsp;
                        bst = apitem.Name.IndexOf('[', bsp);
                        bsp = apitem.Name.IndexOf(']', bst + 1);
                    }
                    if (isdelete)
                    {
                        strb.Append(descr.GoodsName);
                        ritem.PutErrDescription("notdsc", "В Цены не удалось разобрать Описание " + descr.GoodsName + " " + descr.Gender?.ShortName + " " + descr.Material?.ShortName + " " + descr.MaxPart?.PartName);
                    }
                    else
                        ritem.PutErrDescription("notdsc", string.Empty);
                    if ((gsp + 1) < apitem.Name.Trim().Length) strb.Append(apitem.Name.Substring(gsp + 1));
                    ritem.Name = strb.ToString();
                    ritem.ErrDescription = null;
                    ritem.PutErrDescription("prs!=1", string.Empty);
                }
                else if (findprice.Count == 0)
                    ritem.PutErrDescription("prs!=1", "В Цены не найдено ни одного соответствия " + descr.GoodsName + " " + descr.Gender?.ShortName + " " + descr.Material?.ShortName + " " + descr.MaxPart?.PartName);
                else
                    ritem.PutErrDescription("prs!=1", "В Цены найдено более одного соответствия " + descr.GoodsName + " " + descr.Gender?.ShortName + " " + descr.Material?.ShortName + " " + descr.MaxPart?.PartName);

                ProgressChange(r++, Items.Count, 0.1M);
            }
            ProgressChange(95);
            CalcPays();
            ProgressChange(100);
            return Items.Count.ToString() + " строк обработано";
        }
        private void ProgressChange(int currentprogress, int currentcount = 0, decimal completed = 0, int totalcount = 1)
        {
            myExcelImportWin.ProgressBar1.Dispatcher.InvokeAsync(delegate
            {
                if (totalcount == 1 & completed == 0M)
                    myExcelImportWin.ProgressBar1.Value = currentcount == 0 ? currentprogress : (int)(decimal.Divide(currentprogress, currentcount) * 100);
                else
                    myExcelImportWin.ProgressBar1.Value = (int)(decimal.Add(decimal.Divide(completed, totalcount), decimal.Divide(currentprogress, currentcount * totalcount)) * 100);
            });
        }

        public override bool SaveDataChanges()
        {
            this.PopupText = "Изменения сохранены";
            bool isSuccess = !(myview.CurrentItem is RequestItemVM) || (myview.CurrentItem as RequestItemVM).Validate(true);
            if (isSuccess)
            {
                mydbm.Errors.Clear();
                isSuccess = mydbm.SaveCollectionChanches();
            }
            if (!isSuccess)
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
        protected override void DeleteData(object parametr)
        {
            List<lib.Interfaces.IViewModelBaseItem> list = new List<lib.Interfaces.IViewModelBaseItem>();
            if (parametr is System.Collections.IEnumerable)
                foreach (object item in parametr as System.Collections.IEnumerable)
                {
                    if (item is lib.Interfaces.IViewModelBaseItem) list.Add(item as lib.Interfaces.IViewModelBaseItem);
                }
            base.DeleteData(parametr);
            foreach (RequestItemVM item in list)
                if (item.DomainState == lib.DomainObjectState.Deleted)
                    ItemRemoveTotal(item);
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
        protected override void OtherViewRefresh() { }
        protected override void RefreshData(object parametr)
        {
            mydbm.Collection.Clear();
            mydbm.Fill();
            CalcPays();
        }
        protected override void RejectChanges(object parametr)
        {
            bool isdeleted;
            List<RequestItemVM> destroied = new List<RequestItemVM>();
            foreach (RequestItemVM item in mysync.ViewModelCollection)
            {
                if (item.DomainState == lib.DomainObjectState.Added)
                {
                    destroied.Add(item);
                    ItemRemoveTotal(item);
                }
                else if (item.DomainState != lib.DomainObjectState.Unchanged)
                {
                    isdeleted = item.DomainState == lib.DomainObjectState.Deleted;
                    base.myview.EditItem(item);
                    item.RejectChanges();
                    if(isdeleted & item.DomainState == lib.DomainObjectState.Unchanged) ItemIncludeTotal(item);
                    base.myview.CommitEdit();
                }
            }
            foreach (RequestItemVM item in destroied) mysync.ViewModelCollection.Remove(item);
        }
        protected override void SettingView() { }

        private void CalcPays()
        {
            bool complite = true, complite2 = true;
            decimal tax = 0M, tax2 = 0M, vat = 0M, vat2 = 0M, price = 0M, count = 0M, wn = 0M, wg = 0M;
            System.Windows.Data.ListCollectionView view = new System.Windows.Data.ListCollectionView(mysync.ViewModelCollection);
            view.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
            foreach (object item in view)
            {
                if (item is RequestItemVM)
                {
                    RequestItemVM ritem = item as RequestItemVM;
                    if (ritem.Quantity.HasValue) count += ritem.Quantity.Value;
                    if (ritem.WeightNet.HasValue) wn += ritem.WeightNet.Value;
                    if (ritem.WeightGross.HasValue) wg += ritem.WeightGross.Value;
                    if (ritem.TaxTotal.HasValue) tax += ritem.TaxTotal.Value; else complite = false;
                    if (ritem.TaxTotal2009.HasValue) tax2 += ritem.TaxTotal2009.Value; else complite2 = false;
                    if (ritem.TaxVAT.HasValue) vat += ritem.TaxVAT.Value; else complite = false;
                    if (ritem.TaxVAT2009.HasValue) vat2 += ritem.TaxVAT2009.Value; else complite2 = false;
                    if (ritem.PriceTotal.HasValue) price += ritem.PriceTotal.Value; else { complite = false; complite2 = false; }
                }
            }
            myquantity = count;
            myweightnet = wn;
            myweightgross = wg;
            mypricetotal = price;
            price = decimal.Ceiling(0.003M * price);
            mytaxcert = price;
            mytaxtotal = tax;
            mytaxtotal2 = tax2;
            mytaxvat = vat;
            mytaxvat2 = vat2;
            if (myrequest != null)
            {
                if (complite)
                    myrequest["pay1"] = tax + vat + price;
                if (complite2)
                    myrequest["customspay"] = tax2 + vat2 + price;
            }
            PropertyChangedNotification("Quantity");
            PropertyChangedNotification("WeightNet");
            PropertyChangedNotification("WeightGross");
            PropertyChangedNotification("PriceTotal");
            PropertyChangedNotification("TaxTotal");
            PropertyChangedNotification("TaxTotal2");
            PropertyChangedNotification("TaxVAT");
            PropertyChangedNotification("TaxVAT2");
            PropertyChangedNotification("TaxCert");
        }
        private void ViewModelCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action==System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (RequestItemVM item in e.NewItems)
                    item.TotalValueChanged += Item_TotalValueChanged;
            }
        }
        private void Item_TotalValueChanged(string propertyname, decimal? oldvalue, decimal? newvalue)
        {
            decimal diff = newvalue ?? 0M - oldvalue ?? 0M;
            switch (propertyname)
            {
                case "Quantity":
                    myquantity = myquantity + diff;
                    PropertyChangedNotification("Quantity");
                    break;
                case "WeightNet":
                    myweightnet = myweightnet + diff;
                    PropertyChangedNotification("WeightNet");
                    break;
                case "WeightGross":
                    myweightgross = myweightgross + diff;
                    PropertyChangedNotification("WeightGross");
                    break;
                case "PriceTotal":
                    mypricetotal = mypricetotal + diff;
                    mytaxcert+= decimal.Ceiling(0.003M * mypricetotal);
                    PropertyChangedNotification("PriceTotal");
                    PropertyChangedNotification("TaxCert");
                    break;
                case "TaxTotal":
                    mytaxtotal += diff;
                    PropertyChangedNotification("TaxTotal");
                    break;
                case "TaxTotal2":
                    mytaxtotal2 += diff;
                    PropertyChangedNotification("TaxTotal2");
                    break;
                case "TaxVAT":
                    mytaxvat += diff;
                    PropertyChangedNotification("TaxVAT");
                    break;
                case "TaxVAT2":
                    mytaxvat2 += diff;
                    PropertyChangedNotification("TaxVAT2");
                    break;
            }
        }
        private void ItemRemoveTotal(RequestItemVM item)
        {
            myquantity = myquantity - item.Quantity ?? 0M;
            PropertyChangedNotification("Quantity");
            myweightnet = myweightnet - item.WeightNet ?? 0M;
            PropertyChangedNotification("WeightNet");
            myweightgross = myweightgross - item.WeightGross ?? 0M;
            PropertyChangedNotification("WeightGross");
            mypricetotal = mypricetotal - item.PriceTotal ?? 0M;
            mytaxcert -= decimal.Ceiling(0.003M * mypricetotal);
            PropertyChangedNotification("PriceTotal");
            PropertyChangedNotification("TaxCert");
            mytaxtotal -= item.TaxTotal ?? 0M;
            PropertyChangedNotification("TaxTotal");
            mytaxtotal2 -= item.TaxTotal2009 ?? 0M;
            PropertyChangedNotification("TaxTotal2");
            mytaxvat -= item.TaxVAT ?? 0M;
            PropertyChangedNotification("TaxVAT");
            mytaxvat2 -= item.TaxVAT2009 ?? 0M;
            PropertyChangedNotification("TaxVAT2");
            item.TotalValueChanged -= Item_TotalValueChanged;
        }
        private void ItemIncludeTotal(RequestItemVM item)
        {
            myquantity = myquantity + item.Quantity ?? 0M;
            PropertyChangedNotification("Quantity");
            myweightnet = myweightnet + item.WeightNet ?? 0M;
            PropertyChangedNotification("WeightNet");
            myweightgross = myweightgross + item.WeightGross ?? 0M;
            PropertyChangedNotification("WeightGross");
            mypricetotal = mypricetotal + item.PriceTotal ?? 0M;
            mytaxcert += decimal.Ceiling(0.003M * mypricetotal);
            PropertyChangedNotification("PriceTotal");
            PropertyChangedNotification("TaxCert");
            mytaxtotal += item.TaxTotal ?? 0M;
            PropertyChangedNotification("TaxTotal");
            mytaxtotal2 += item.TaxTotal2009 ?? 0M;
            PropertyChangedNotification("TaxTotal2");
            mytaxvat += item.TaxVAT ?? 0M;
            PropertyChangedNotification("TaxVAT");
            mytaxvat2 += item.TaxVAT2009 ?? 0M;
            PropertyChangedNotification("TaxVAT2");
            item.TotalValueChanged += Item_TotalValueChanged;
        }
    }
}
