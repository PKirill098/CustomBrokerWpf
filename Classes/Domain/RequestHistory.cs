using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class RequestHistory : lib.DomainBaseClass
    {
        public RequestHistory(int id,Request request
            , bool actualweightchanged, bool agentchanged
            , bool cargochanged, bool cellnumberchanged, bool customerchanged, bool customernotechanged
            , bool goodvaluechanged, bool importerchanged, bool invoicechanged, bool invoicediscountchanged
            , bool managerchanged, bool managergroupchanged, bool managernotechanged, bool officialweightchanged, bool parcelgroupchanged, bool parcelnumberchanged, bool parceltypechanged
            , bool servicetypechanged, bool specificationchanged,bool statuschanged, bool storedatechanged, bool storeinformchanged, bool storenotechanged, bool storepointchanged
            , bool volumechanged
            ) : base(id, lib.DomainObjectState.Sealed)
        {
            myrequest = request;

            myactualweightchanged = actualweightchanged;
            myagentchanged = agentchanged;
            mycargochanged = cargochanged;
            mycellnumberchanged = cellnumberchanged;
            mycustomerchanged = customerchanged;
            mycustomernotechanged = customernotechanged;
            mygoodvaluechanged = goodvaluechanged;
            myimporterchanged = importerchanged;
            myinvoicechanged = invoicechanged;
            myinvoicediscountchanged = invoicediscountchanged;
            mymanagerchanged = managerchanged;
            mymanagergroupchanged = managergroupchanged;
            mymanagernotechanged = managernotechanged;
            myofficialweightchanged = officialweightchanged;
            myparcelgroupchanged = parcelgroupchanged;
            myparcelnumberchanged = parcelnumberchanged;
            myparceltypechanged = parceltypechanged;
            myservicetypechanged = servicetypechanged;
            myspecificationchanged = specificationchanged;
            mystatuschanged = statuschanged;
            mystoredatechanged = storedatechanged;
            mystoreinformchanged = storeinformchanged;
            mystorenotechanged = storenotechanged;
            mystorepointchanged = storepointchanged;
            myvolumechanged = volumechanged;
        }

        private Request myrequest;
        public Request Request
        {
            set { SetProperty<Request>(ref myrequest, value); }
            get { return myrequest; }
        }

        private bool myactualweightchanged;
        public bool ActualWeightChanged
        {
            set { SetProperty<bool>(ref myactualweightchanged, value); }
            get { return myactualweightchanged; }
        }
        private bool myagentchanged;
        public bool AgentChanged
        {
            set { SetProperty<bool>(ref myagentchanged, value); }
            get { return myagentchanged; }
        }
        private bool mycargochanged;
        public bool CargoChanged
        {
            set { SetProperty<bool>(ref mycargochanged, value); }
            get { return mycargochanged; }
        }
        private bool mycellnumberchanged;
        public bool CellNumberChanged
        {
            set { SetProperty<bool>(ref mycellnumberchanged, value); }
            get { return mycellnumberchanged; }
        }
        private bool mycustomerchanged;
        public bool CustomerChanged
        {
            set { SetProperty<bool>(ref mycustomerchanged, value); }
            get { return mycustomerchanged; }
        }
        private bool mycustomernotechanged;
        public bool CustomerNoteChanged
        {
            set { SetProperty<bool>(ref mycustomernotechanged, value); }
            get { return mycustomernotechanged; }
        }
        private bool mygoodvaluechanged;
        public bool GoodValueChanged
        {
            set { SetProperty<bool>(ref mygoodvaluechanged, value); }
            get { return mygoodvaluechanged; }
        }
        private bool myimporterchanged;
        public bool ImporterChanged
        {
            set { SetProperty<bool>(ref myimporterchanged, value); }
            get { return myimporterchanged; }
        }
        private bool myinvoicechanged;
        public bool InvoiceChanged
        {
            set { SetProperty<bool>(ref myinvoicechanged, value); }
            get { return myinvoicechanged; }
        }
        private bool myinvoicediscountchanged;
        public bool InvoiceDiscountChanged
        {
            set { SetProperty<bool>(ref myinvoicediscountchanged, value); }
            get { return myinvoicediscountchanged; }
        }
        private bool mymanagerchanged;
        public bool ManagerChanged
        {
            set { SetProperty<bool>(ref mymanagerchanged, value); }
            get { return mymanagerchanged; }
        }
        private bool mymanagergroupchanged;
        public bool ManagerGroupChanged
        {
            set { SetProperty<bool>(ref mymanagergroupchanged, value); }
            get { return mymanagergroupchanged; }
        }
        private bool mymanagernotechanged;
        public bool ManagerNoteChanged
        {
            set { SetProperty<bool>(ref mymanagernotechanged, value); }
            get { return mymanagernotechanged; }
        }
        private bool myofficialweightchanged;
        public bool OfficialWeightChanged
        {
            set { SetProperty<bool>(ref myofficialweightchanged, value); }
            get { return myofficialweightchanged; }
        }
        private bool myparcelgroupchanged;
        public bool ParcelGroupChanged
        {
            set { SetProperty<bool>(ref myparcelgroupchanged, value); }
            get { return myparcelgroupchanged; }
        }
        private bool myparcelnumberchanged;
        public bool ParcelNumberChanged
        {
            set { SetProperty<bool>(ref myparcelnumberchanged, value); }
            get { return myparcelnumberchanged; }
        }
        private bool myparceltypechanged;
        public bool ParcelTypeChanged
        {
            set { SetProperty<bool>(ref myparceltypechanged, value); }
            get { return myparceltypechanged; }
        }
        private bool myservicetypechanged;
        public bool ServiceTypeChanged
        {
            set { SetProperty<bool>(ref myservicetypechanged, value); }
            get { return myservicetypechanged; }
        }
        private bool myspecificationchanged;
        public bool SpecificationChanged
        {
            set { SetProperty<bool>(ref myspecificationchanged,value); }
            get { return myspecificationchanged; }
        }
        private bool mystatuschanged;
        public bool StatusChanged
        {
            set { SetProperty<bool>(ref mystatuschanged, value); }
            get { return mystatuschanged; }
        }
        private bool mystoredatechanged;
        public bool StoreDateChanged
        {
            set { SetProperty<bool>(ref mystoredatechanged, value); }
            get { return mystoredatechanged; }
        }
        private bool mystoreinformchanged;
        public bool StoreInformChanged
        {
            set { SetProperty<bool>(ref mystoreinformchanged, value); }
            get { return mystoreinformchanged; }
        }
        private bool mystorenotechanged;
        public bool StoreNoteChanged
        {
            set { SetProperty<bool>(ref mystorenotechanged, value); }
            get { return mystorenotechanged; }
        }
        private bool mystorepointchanged;
        public bool StorePointChanged
        {
            set { SetProperty<bool>(ref mystorepointchanged, value); }
            get { return mystorepointchanged; }
        }
        private bool myvolumechanged;
        public bool VolumeChanged
        {
            set { SetProperty<bool>(ref myvolumechanged, value); }
            get { return myvolumechanged; }
        }
    }

    public class RequestHistoryDBM : lib.DBMSFill<RequestHistory>
    {
        public RequestHistoryDBM() : base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            this.SelectCommandText = "dbo.RequestHistory_sp";
            this.SelectProcedure = true;
            this.SelectParams = new SqlParameter[] { new SqlParameter("@requestId", System.Data.SqlDbType.Int) };
        }

        private Request myrequest;
        internal Request Request
        {
            set { myrequest = value; }
            get { return myrequest; }
        }

        protected override RequestHistory CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            Request newitem = new Request(reader.GetInt32(0), 0
                , reader.IsDBNull(reader.GetOrdinal("UpdateWhen")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdateWhen"))
                , reader.IsDBNull(reader.GetOrdinal("UpdateWho")) ? null : reader.GetString(reader.GetOrdinal("UpdateWho")), lib.DomainObjectState.Sealed
                , null
                , CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("status")))
                , reader.IsDBNull(reader.GetOrdinal("agentId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("agentId"))
                , null
                , reader.IsDBNull(reader.GetOrdinal("customerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("customerId"))
                , null
                , reader.IsDBNull(reader.GetOrdinal("freight")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("freight"))
                , reader.IsDBNull(reader.GetOrdinal("parcelgroup")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("parcelgroup"))
                , reader.IsDBNull(reader.GetOrdinal("parcelid")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("parcelid"))
                , reader.IsDBNull(reader.GetOrdinal("storeid")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("storeid"))
                , reader.IsDBNull(reader.GetOrdinal("cellNumber")) ? (short?)null : reader.GetInt16(reader.GetOrdinal("cellNumber"))
                , reader.IsDBNull(reader.GetOrdinal("statedoc")) ? (byte?)null : reader.GetByte(reader.GetOrdinal("statedoc"))
                , reader.IsDBNull(reader.GetOrdinal("stateexc")) ? (byte?)null : reader.GetByte(reader.GetOrdinal("stateexc"))
                , reader.IsDBNull(reader.GetOrdinal("stateinv")) ? (byte?)null : reader.GetByte(reader.GetOrdinal("stateinv"))
                , reader.IsDBNull(reader.GetOrdinal("currencypaid")) ? false : reader.GetBoolean(reader.GetOrdinal("currencypaid"))
                , reader.IsDBNull(reader.GetOrdinal("specloaded")) ? false : reader.GetBoolean(reader.GetOrdinal("specloaded"))
                , reader.IsDBNull(reader.GetOrdinal("ttlpayinvoice")) ? false : reader.GetBoolean(reader.GetOrdinal("ttlpayinvoice"))
                , reader.IsDBNull(reader.GetOrdinal("ttlpaycurrency")) ? false : reader.GetBoolean(reader.GetOrdinal("ttlpaycurrency"))
                , reader.IsDBNull(reader.GetOrdinal("parceltype")) ? null : CustomBrokerWpf.References.ParcelTypes.FindFirstItem("Id", (int)reader.GetByte(reader.GetOrdinal("parceltype")))
                , reader.IsDBNull(reader.GetOrdinal("additionalcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("additionalcost"))
                , reader.IsDBNull(reader.GetOrdinal("additionalpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("additionalpay"))
                , reader.IsDBNull(reader.GetOrdinal("actualWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("actualWeight"))
                , reader.IsDBNull(reader.GetOrdinal("bringcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("bringcost"))
                , reader.IsDBNull(reader.GetOrdinal("bringpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("bringpay"))
                , reader.IsDBNull(reader.GetOrdinal("brokercost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("brokercost"))
                , reader.IsDBNull(reader.GetOrdinal("brokerpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("brokerpay"))
                , reader.IsDBNull(reader.GetOrdinal("currencyrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("currencyrate"))
                , reader.IsDBNull(reader.GetOrdinal("currencysum")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("currencysum"))
                , reader.IsDBNull(reader.GetOrdinal("customscost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("customscost"))
                , reader.IsDBNull(reader.GetOrdinal("customspay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("customspay"))
                , reader.IsDBNull(reader.GetOrdinal("deliverycost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("deliverycost"))
                , reader.IsDBNull(reader.GetOrdinal("deliverypay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("deliverypay"))
                , reader.IsDBNull(reader.GetOrdinal("dtrate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("dtrate"))
                , reader.IsDBNull(reader.GetOrdinal("goodValue")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("goodValue"))
                , reader.IsDBNull(reader.GetOrdinal("freightcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("freightcost"))
                , reader.IsDBNull(reader.GetOrdinal("freightpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("freightpay"))
                , reader.IsDBNull(reader.GetOrdinal("insurancecost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("insurancecost"))
                , reader.IsDBNull(reader.GetOrdinal("insurancepay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("insurancepay"))
                , reader.IsDBNull(reader.GetOrdinal("invoice")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("invoice"))
                , reader.IsDBNull(reader.GetOrdinal("invoicediscount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("invoicediscount"))
                , reader.IsDBNull(reader.GetOrdinal("officialWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("officialWeight"))
                , reader.IsDBNull(reader.GetOrdinal("preparatncost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("preparatncost"))
                , reader.IsDBNull(reader.GetOrdinal("preparatnpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("preparatnpay"))
                , reader.IsDBNull(reader.GetOrdinal("selling")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("selling"))
                , reader.IsDBNull(reader.GetOrdinal("sellingmarkup")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sellingmarkup"))
                , reader.IsDBNull(reader.GetOrdinal("sellingmarkuprate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sellingmarkuprate"))
                , reader.IsDBNull(reader.GetOrdinal("sertificatcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sertificatcost"))
                , reader.IsDBNull(reader.GetOrdinal("sertificatpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("sertificatpay"))
                , reader.IsDBNull(reader.GetOrdinal("tdcost")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("tdcost"))
                , reader.IsDBNull(reader.GetOrdinal("tdpay")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("tdpay"))
                , reader.IsDBNull(reader.GetOrdinal("volume")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("volume"))
                , reader.IsDBNull(reader.GetOrdinal("currencydate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("currencydate"))
                , reader.IsDBNull(reader.GetOrdinal("currencypaiddate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("currencypaiddate"))
                , reader.IsDBNull(reader.GetOrdinal("gtddate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gtddate"))
                , reader.GetDateTime(reader.GetOrdinal("requestDate"))
                , reader.IsDBNull(reader.GetOrdinal("shipplandate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("shipplandate"))
                , reader.IsDBNull(reader.GetOrdinal("specification")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("specification"))
                , reader.IsDBNull(reader.GetOrdinal("storageDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("storageDate"))
                , reader.IsDBNull(reader.GetOrdinal("storageInform")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("storageInform"))
                , reader.IsDBNull(reader.GetOrdinal("algorithmnote1")) ? null : reader.GetString(reader.GetOrdinal("algorithmnote1"))
                , reader.IsDBNull(reader.GetOrdinal("algorithmnote2")) ? null : reader.GetString(reader.GetOrdinal("algorithmnote2"))
                , reader.IsDBNull(reader.GetOrdinal("loadDescription")) ? null : reader.GetString(reader.GetOrdinal("loadDescription"))
                , reader.IsDBNull(reader.GetOrdinal("colorMark")) ? null : reader.GetString(reader.GetOrdinal("colorMark"))
                , reader.IsDBNull(reader.GetOrdinal("consolidate")) ? null : reader.GetString(reader.GetOrdinal("consolidate"))
                , reader.IsDBNull(reader.GetOrdinal("currencynote")) ? null : reader.GetString(reader.GetOrdinal("currencynote"))
                , reader.IsDBNull(reader.GetOrdinal("customerNote")) ? null : reader.GetString(reader.GetOrdinal("customerNote"))
                , reader.IsDBNull(reader.GetOrdinal("docdirpath")) ? null : reader.GetString(reader.GetOrdinal("docdirpath"))
                , reader.IsDBNull(reader.GetOrdinal("gtd")) ? null : reader.GetString(reader.GetOrdinal("gtd"))
                , reader.IsDBNull(reader.GetOrdinal("parcel")) ? null : reader.GetString(reader.GetOrdinal("parcel"))
                , reader.IsDBNull(reader.GetOrdinal("managergroupName")) ? null : reader.GetString(reader.GetOrdinal("managergroupName"))
                , reader.IsDBNull(reader.GetOrdinal("managerNote")) ? null : reader.GetString(reader.GetOrdinal("managerNote"))
                , reader.IsDBNull(reader.GetOrdinal("servicetype")) ? null : reader.GetString(reader.GetOrdinal("servicetype"))
                , reader.IsDBNull(reader.GetOrdinal("storageNote")) ? null : reader.GetString(reader.GetOrdinal("storageNote"))
                , reader.IsDBNull(reader.GetOrdinal("storagePoint")) ? null : reader.GetString(reader.GetOrdinal("storagePoint"))
                , reader.IsDBNull(reader.GetOrdinal("importer")) ? null : CustomBrokerWpf.References.Importers.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("importer")))
                , reader.IsDBNull(reader.GetOrdinal("managerid")) ? null : CustomBrokerWpf.References.Managers.FindFirstItem("Id", reader.GetInt32(reader.GetOrdinal("managerid")))
                );
            return new RequestHistory(0, newitem
                , reader.GetBoolean(reader.GetOrdinal("actualWeightchd")), reader.GetBoolean(reader.GetOrdinal("agentIdchd"))
                , reader.GetBoolean(reader.GetOrdinal("loadDescriptionchd")), reader.GetBoolean(reader.GetOrdinal("cellNumberchd")), reader.GetBoolean(reader.GetOrdinal("customerIdchd")), reader.GetBoolean(reader.GetOrdinal("customerNotechd"))
                , reader.GetBoolean(reader.GetOrdinal("goodValuechd")), reader.GetBoolean(reader.GetOrdinal("importerchd")), reader.GetBoolean(reader.GetOrdinal("invoicechd")), reader.GetBoolean(reader.GetOrdinal("invoicediscountchd"))
                , reader.GetBoolean(reader.GetOrdinal("managerchd")), reader.GetBoolean(reader.GetOrdinal("managergroupchd")), reader.GetBoolean(reader.GetOrdinal("managerNotechd"))
                , reader.GetBoolean(reader.GetOrdinal("officialWeightchd"))
                , reader.GetBoolean(reader.GetOrdinal("parcelgroupchd")), reader.GetBoolean(reader.GetOrdinal("parcelchd")), reader.GetBoolean(reader.GetOrdinal("parceltypechd"))
                , reader.GetBoolean(reader.GetOrdinal("servicetypechd")), reader.GetBoolean(reader.GetOrdinal("specificationchd")), reader.GetBoolean(reader.GetOrdinal("statuschd"))
                , reader.GetBoolean(reader.GetOrdinal("storageDatechd")), reader.GetBoolean(reader.GetOrdinal("storageInformchd")), reader.GetBoolean(reader.GetOrdinal("storageNotechd")), reader.GetBoolean(reader.GetOrdinal("storagePointchd"))
                , reader.GetBoolean(reader.GetOrdinal("volumechd"))
                );
        }

        protected override void PrepareFill(SqlConnection addcon)
        {
            this.SelectParams[0].Value = myrequest?.Id;
        }
        protected override void CancelLoad()
        { }
    }

    public class RequestHistoryVM
    {
        public RequestHistoryVM(RequestHistory model)
        {
            mydomainobject = model;
            myrequest =new RequestVM(model.Request);
            mystillbrush = System.Windows.Media.Brushes.White;
            mystillbrushselected = System.Windows.SystemColors.HighlightBrush;
            mychangedbrush = System.Windows.Media.Brushes.AliceBlue;
            mychangedbrushselected = System.Windows.Media.Brushes.DarkGray;
        }

        private RequestHistory mydomainobject;
        public RequestHistory DomainObject
        {
            get { return mydomainobject; }
        }

        private RequestVM myrequest;
        public RequestVM Request
        {
            get { return myrequest; }
        }

        private System.Windows.Media.Brush mystillbrush;
        private System.Windows.Media.Brush mystillbrushselected;
        private System.Windows.Media.Brush mychangedbrush;
        private System.Windows.Media.Brush mychangedbrushselected;
        public System.Windows.Media.Brush ActualWeightBackground
        { get { return this.DomainObject.ActualWeightChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush ActualWeightBackgroundSelected
        { get { return this.DomainObject.ActualWeightChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush AgentBackground
        { get { return this.DomainObject.AgentChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush AgentBackgroundSelected
        { get { return this.DomainObject.AgentChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush CargoBackground
        { get { return this.DomainObject.CargoChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush CargoBackgroundSelected
        { get { return this.DomainObject.CargoChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush CellNumberBackground
        { get { return this.DomainObject.CellNumberChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush CellNumberBackgroundSelected
        { get { return this.DomainObject.CellNumberChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush CustomerBackground
        { get { return this.DomainObject.CustomerChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush CustomerBackgroundSelected
        { get { return this.DomainObject.CustomerChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush CustomerNoteBackground
        { get { return this.DomainObject.CustomerNoteChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush CustomerNoteBackgroundSelected
        { get { return this.DomainObject.CustomerNoteChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush GoodValueBackground
        { get { return this.DomainObject.GoodValueChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush GoodValueBackgroundSelected
        { get { return this.DomainObject.GoodValueChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush ImporterBackground
        { get { return this.DomainObject.ImporterChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush ImporterBackgroundSelected
        { get { return this.DomainObject.ImporterChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush InvoiceBackground
        { get { return this.DomainObject.InvoiceChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush InvoiceBackgroundSelected
        { get { return this.DomainObject.InvoiceChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush InvoiceDiscountBackground
        { get { return this.DomainObject.InvoiceDiscountChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush InvoiceDiscountBackgroundSelected
        { get { return this.DomainObject.InvoiceDiscountChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush ManagerGroupBackground
        { get { return this.DomainObject.ManagerGroupChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush ManagerGroupBackgroundSelected
        { get { return this.DomainObject.ManagerGroupChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush ManagerNoteBackground
        { get { return this.DomainObject.ManagerNoteChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush ManagerNoteBackgroundSelected
        { get { return this.DomainObject.ManagerNoteChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush OfficialWeightBackground
        { get { return this.DomainObject.OfficialWeightChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush OfficialWeightBackgroundSelected
        { get { return this.DomainObject.OfficialWeightChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush ParcelGroupBackground
        { get { return this.DomainObject.ParcelGroupChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush ParcelGroupBackgroundSelected
        { get { return this.DomainObject.ParcelGroupChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush ParcelNumberBackground
        { get { return this.DomainObject.ParcelNumberChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush ParcelNumberBackgroundSelected
        { get { return this.DomainObject.ParcelNumberChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush ParcelTypeBackground
        { get { return this.DomainObject.ParcelTypeChanged? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush ParcelTypeBackgroundSelected
        { get { return this.DomainObject.ParcelTypeChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush ServiceTypeBackground
        { get { return this.DomainObject.ServiceTypeChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush ServiceTypeBackgroundSelected
        { get { return this.DomainObject.ServiceTypeChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush SpecificationBackground
        { get { return this.DomainObject.SpecificationChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush SpecificationBackgroundSelected
        { get { return this.DomainObject.SpecificationChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush StatusBackground
        { get { return this.DomainObject.StatusChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush StatusBackgroundSelected
        { get { return this.DomainObject.StatusChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush StoreDateBackground
        { get { return this.DomainObject.StoreDateChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush StoreDateBackgroundSelected
        { get { return this.DomainObject.StoreDateChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush StoreInformBackground
        { get { return this.DomainObject.StoreInformChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush StoreInformBackgroundSelected
        { get { return this.DomainObject.StoreInformChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush StoreNoteBackground
        { get { return this.DomainObject.StoreNoteChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush StoreNoteBackgroundSelected
        { get { return this.DomainObject.StoreNoteChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush StorePointDateBackground
        { get { return this.DomainObject.StoreDateChanged | this.DomainObject.StorePointChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush StorePointDateBackgroundSelected
        { get { return this.DomainObject.StoreDateChanged | this.DomainObject.StorePointChanged ? mychangedbrushselected : mystillbrushselected; } }
        public System.Windows.Media.Brush VolumeBackground
        { get { return this.DomainObject.VolumeChanged ? mychangedbrush : mystillbrush; } }
        public System.Windows.Media.Brush VolumeBackgroundSelected
        { get { return this.DomainObject.VolumeChanged ? mychangedbrushselected : mystillbrushselected; } }
    }

    public class RequestHistorySynchronizer : lib.ModelViewCollectionsSynchronizer<RequestHistory, RequestHistoryVM>
    {
        protected override RequestHistory UnWrap(RequestHistoryVM wrap)
        {
            return wrap.DomainObject as RequestHistory;
        }
        protected override RequestHistoryVM Wrap(RequestHistory fill)
        {
            return new RequestHistoryVM(fill);
        }
    }

    public class RequestHistoryViewCommand:lib.ViewModelSealedCommand
    {
        internal RequestHistoryViewCommand(Request request)
        {
            mydbm = new RequestHistoryDBM();
            mydbm.Request = request;
            mydbm.FillAsyncCompleted = () => { if (mydbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); };
            mydbm.FillAsync();
            mysync = new RequestHistorySynchronizer();
            mysync.DomainCollection = mydbm.Collection;
            myitems = new ListCollectionView(mysync.ViewModelCollection);
            myitems.SortDescriptions.Add(new SortDescription("Request.UpdateWhen", ListSortDirection.Ascending));
        }

        RequestHistoryDBM mydbm;
        private RequestHistorySynchronizer mysync;
        private ListCollectionView myitems;
        public ListCollectionView Items
        {
            get
            {
                return myitems;
            }
        }

        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void RefreshData(object parametr)
        {
            mydbm.FillAsync();
        }
    }
}
