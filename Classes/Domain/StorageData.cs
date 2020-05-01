using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class StorageData : lib.DomainBaseReject
    {
        public StorageData(int id, lib.DomainObjectState state
            ,string agent, short cellnumber, string customer,string doc,decimal? forwarding,string freightnumber, decimal? freightcost, decimal? goodvalue, decimal grossweight,decimal? netweight, Request request, decimal? service,string shipmentnumber, lib.ReferenceCollectionSimpleItem store, DateTime storedate,string storenote, string storepoint, decimal volume
            ) : base(id, state)
        {
            myagent = agent;
            mycellnumber = cellnumber;
            mycustomer = customer;
            mydoc = doc;
            myforwarding = forwarding;
            myfreightnumber = freightnumber;
            myfreightcost = freightcost;
            mygoodvalue = goodvalue;
            mygrossweight = grossweight;
            mynetweight = netweight;
            myrequest = request;
            myservice = service;
            myshipmentnumber = shipmentnumber;
            mystore = store;
            mystoredate = storedate;
            mystorenote = storenote;
            mystorepoint = storepoint;
            myvolume = volume;
        }
        private string myagent;
        public string Agent
        { set { SetProperty<string>(ref myagent, value); } get { return myagent; } }
        private short mycellnumber;
        public short CellNumber
        {
            set
            {
                base.SetProperty<short>(ref mycellnumber, value);
            }
            get { return mycellnumber; }
        }
        private string mycustomer;
        public string Customer
        { set { SetProperty<string>(ref mycustomer, value); } get { return mycustomer; } }
        private string mydoc;
        public string Doc
        { set { SetProperty<string>(ref mydoc, value); } get { return mydoc; } }
        private decimal? myforwarding;
        public decimal? Forwarding
        { set { SetProperty<decimal?>(ref myforwarding, value); } get { return myforwarding; } }
        private string myfreightnumber;
        public string FreightNumber
        { set { SetProperty<string>(ref myfreightnumber, value); } get { return myfreightnumber; } }
        private decimal? myfreightcost;
        public decimal? FreightCost
        { set { SetProperty<decimal?>(ref myfreightcost, value); } get { return myfreightcost; } }
        private decimal? mygoodvalue;
        public decimal? GoodValue
        {
            set
            {
                base.SetProperty<decimal?>(ref mygoodvalue, value);
            }
            get { return mygoodvalue; }
        }
        private decimal mygrossweight;
        public decimal GrossWeight
        {
            set
            {
                base.SetProperty<decimal>(ref mygrossweight, value);
            }
            get { return mygrossweight; }
        }
        private decimal? mynetweight;
        public decimal? NetWeight
        {
            set
            {
                base.SetProperty<decimal?>(ref mynetweight, value);
            }
            get { return mynetweight; }
        }
        private Request myrequest;
        public Request Request
        { set { SetProperty<Request>(ref myrequest, value); } get { return myrequest; } }
        private decimal? myservice;
        public decimal? Service
        { set { SetProperty<decimal?>(ref myservice, value); } get { return myservice; } }
        private string myshipmentnumber;
        public string ShipmentNumber
        { set { SetProperty<string>(ref myshipmentnumber, value); } get { return myshipmentnumber; } }
        private lib.ReferenceCollectionSimpleItem mystore;
        public lib.ReferenceCollectionSimpleItem Store
        { set { SetProperty<lib.ReferenceCollectionSimpleItem>(ref mystore, value); } get { return mystore; } }
        private DateTime mystoredate;
        public DateTime StoreDate
        {
            set
            {
                base.SetProperty<DateTime>(ref mystoredate, value);
            }
            get { return mystoredate; }
        }
        private string mystorenote;
        public string StoreNote
        {
            set
            {
                base.SetProperty<string>(ref mystorenote, value);
            }
            get { return mystorenote; }
        }
        private string mystorepoint;
        public string StorePoint
        {
            set
            {
                base.SetProperty<string>(ref mystorepoint, value);
            }
            get { return mystorepoint; }
        }
        private decimal myvolume;
        public decimal Volume
        {
            set
            {
                base.SetProperty<decimal>(ref myvolume, value);
            }
            get { return myvolume; }
        }


        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
        }
        protected override void RejectProperty(string property, object value)
        {
        }
    }

    internal class StorageDataDBM : lib.DBManager<StorageData>
    {
        protected override StorageData CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            throw new NotImplementedException();
        }

        protected override void GetOutputParametersValue(StorageData item)
        {
            throw new NotImplementedException();
        }

        protected override void ItemAcceptChanches(StorageData item)
        {
            throw new NotImplementedException();
        }

        protected override void LoadObjects(StorageData item)
        {
            throw new NotImplementedException();
        }

        protected override bool LoadObjects()
        {
            throw new NotImplementedException();
        }

        protected override bool SaveChildObjects(StorageData item)
        {
            throw new NotImplementedException();
        }

        protected override bool SaveIncludedObject(StorageData item)
        {
            throw new NotImplementedException();
        }

        protected override bool SaveReferenceObjects()
        {
            throw new NotImplementedException();
        }

        protected override bool SetParametersValue(StorageData item)
        {
            throw new NotImplementedException();
        }

        protected override void SetSelectParametersValue()
        {
            throw new NotImplementedException();
        }
    }
}
