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
        private bool mycurrencychanged;
        public bool CurrencyChanged
        {
            set { SetProperty<bool>(ref mycurrencychanged, value); }
            get { return mycurrencychanged; }
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
                , reader.IsDBNull(this.Fields["UpdateWhen"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["UpdateWhen"])
                , reader.IsDBNull(this.Fields["UpdateWho"]) ? null : reader.GetString(this.Fields["UpdateWho"]), lib.DomainObjectState.Sealed
                , null
                , CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", reader.GetInt32(this.Fields["status"]))
                , reader.IsDBNull(this.Fields["agentId"]) ? (int?)null : reader.GetInt32(this.Fields["agentId"])
                , reader.IsDBNull(this.Fields["country"]) ? null : CustomBrokerWpf.References.Countries.FindFirstItem("Code", reader.GetInt32(this.Fields["country"]))
                , reader.IsDBNull(this.Fields["currency"]) ? 0 : reader.GetInt32(this.Fields["currency"])
                , null
                , reader.IsDBNull(this.Fields["customerId"]) ? (int?)null : reader.GetInt32(this.Fields["customerId"])
                , null
                , reader.IsDBNull(this.Fields["freight"]) ? (int?)null : reader.GetInt32(this.Fields["freight"])
                , reader.IsDBNull(this.Fields["parcelgroup"]) ? (int?)null : reader.GetInt32(this.Fields["parcelgroup"])
                , reader.IsDBNull(this.Fields["parcelid"]) ? (int?)null : reader.GetInt32(this.Fields["parcelid"])
                , reader.IsDBNull(this.Fields["storeid"]) ? (int?)null : reader.GetInt32(this.Fields["storeid"])
                , reader.IsDBNull(this.Fields["cellNumber"]) ? (short?)null : reader.GetInt16(this.Fields["cellNumber"])
                , reader.IsDBNull(this.Fields["statedoc"]) ? (byte?)null : reader.GetByte(this.Fields["statedoc"])
                , reader.IsDBNull(this.Fields["stateexc"]) ? (byte?)null : reader.GetByte(this.Fields["stateexc"])
                , reader.IsDBNull(this.Fields["stateinv"]) ? (byte?)null : reader.GetByte(this.Fields["stateinv"])
                , reader.IsDBNull(this.Fields["currencypaid"]) ? false : reader.GetBoolean(this.Fields["currencypaid"])
                , reader.IsDBNull(this.Fields["specloaded"]) ? false : reader.GetBoolean(this.Fields["specloaded"])
                , reader.IsDBNull(this.Fields["ttlpayinvoice"]) ? false : reader.GetBoolean(this.Fields["ttlpayinvoice"])
                , reader.IsDBNull(this.Fields["ttlpaycurrency"]) ? false : reader.GetBoolean(this.Fields["ttlpaycurrency"])
                , reader.IsDBNull(this.Fields["parceltype"]) ? null : CustomBrokerWpf.References.ParcelTypes.FindFirstItem("Id", (int)reader.GetByte(this.Fields["parceltype"]))
                , reader.IsDBNull(this.Fields["additionalcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["additionalcost"])
                , reader.IsDBNull(this.Fields["additionalpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["additionalpay"])
                , reader.IsDBNull(this.Fields["actualWeight"]) ? (decimal?)null : reader.GetDecimal(this.Fields["actualWeight"])
                , reader.IsDBNull(this.Fields["bringcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["bringcost"])
                , reader.IsDBNull(this.Fields["bringpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["bringpay"])
                , reader.IsDBNull(this.Fields["brokercost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["brokercost"])
                , reader.IsDBNull(this.Fields["brokerpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["brokerpay"])
                , reader.IsDBNull(this.Fields["currencyrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["currencyrate"])
                , reader.IsDBNull(this.Fields["currencysum"]) ? (decimal?)null : reader.GetDecimal(this.Fields["currencysum"])
                , reader.IsDBNull(this.Fields["customscost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["customscost"])
                , reader.IsDBNull(this.Fields["customspay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["customspay"])
                , reader.IsDBNull(this.Fields["deliverycost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["deliverycost"])
                , reader.IsDBNull(this.Fields["deliverypay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["deliverypay"])
                , reader.IsDBNull(this.Fields["dtrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["dtrate"])
                , reader.IsDBNull(this.Fields["goodValue"]) ? (decimal?)null : reader.GetDecimal(this.Fields["goodValue"])
                , reader.IsDBNull(this.Fields["freightcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["freightcost"])
                , reader.IsDBNull(this.Fields["freightpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["freightpay"])
                , reader.IsDBNull(this.Fields["insurancecost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["insurancecost"])
                , reader.IsDBNull(this.Fields["insurancepay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["insurancepay"])
                , reader.IsDBNull(this.Fields["invoice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["invoice"])
                , reader.IsDBNull(this.Fields["invoicediscount"]) ? (decimal?)null : reader.GetDecimal(this.Fields["invoicediscount"])
                , reader.IsDBNull(this.Fields["officialWeight"]) ? (decimal?)null : reader.GetDecimal(this.Fields["officialWeight"])
                , reader.IsDBNull(this.Fields["preparatncost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["preparatncost"])
                , reader.IsDBNull(this.Fields["preparatnpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["preparatnpay"])
                , reader.IsDBNull(this.Fields["selling"]) ? (decimal?)null : reader.GetDecimal(this.Fields["selling"])
                , reader.IsDBNull(this.Fields["sellingmarkup"]) ? (decimal?)null : reader.GetDecimal(this.Fields["sellingmarkup"])
                , reader.IsDBNull(this.Fields["sellingmarkuprate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["sellingmarkuprate"])
                , reader.IsDBNull(this.Fields["sertificatcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["sertificatcost"])
                , reader.IsDBNull(this.Fields["sertificatpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["sertificatpay"])
                , reader.IsDBNull(this.Fields["tdcost"]) ? (decimal?)null : reader.GetDecimal(this.Fields["tdcost"])
                , reader.IsDBNull(this.Fields["tdpay"]) ? (decimal?)null : reader.GetDecimal(this.Fields["tdpay"])
                , reader.IsDBNull(this.Fields["volume"]) ? (decimal?)null : reader.GetDecimal(this.Fields["volume"])
                , reader.IsDBNull(this.Fields["currencydate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["currencydate"])
                , reader.IsDBNull(this.Fields["currencypaiddate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["currencypaiddate"])
                , reader.IsDBNull(this.Fields["gtddate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["gtddate"])
                , reader.GetDateTime(this.Fields["requestDate"])
                , reader.IsDBNull(this.Fields["shipplandate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["shipplandate"])
                , reader.IsDBNull(this.Fields["specification"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["specification"])
                , reader.IsDBNull(this.Fields["storageDate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["storageDate"])
                , reader.IsDBNull(this.Fields["storageInform"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["storageInform"])
                , reader.IsDBNull(this.Fields["algorithmnote1"]) ? null : reader.GetString(this.Fields["algorithmnote1"])
                , reader.IsDBNull(this.Fields["algorithmnote2"]) ? null : reader.GetString(this.Fields["algorithmnote2"])
                , reader.IsDBNull(this.Fields["loadDescription"]) ? null : reader.GetString(this.Fields["loadDescription"])
                , reader.IsDBNull(this.Fields["colorMark"]) ? null : reader.GetString(this.Fields["colorMark"])
                , reader.IsDBNull(this.Fields["consolidate"]) ? null : reader.GetString(this.Fields["consolidate"])
                , reader.IsDBNull(this.Fields["currencynote"]) ? null : reader.GetString(this.Fields["currencynote"])
                , reader.IsDBNull(this.Fields["customerNote"]) ? null : reader.GetString(this.Fields["customerNote"])
                , reader.IsDBNull(this.Fields["docdirpath"]) ? null : reader.GetString(this.Fields["docdirpath"])
                , reader.IsDBNull(this.Fields["gtd"]) ? null : reader.GetString(this.Fields["gtd"])
                , reader.IsDBNull(this.Fields["parcel"]) ? null : reader.GetString(this.Fields["parcel"])
                , reader.IsDBNull(this.Fields["managergroupName"]) ? null : reader.GetString(this.Fields["managergroupName"])
                , reader.IsDBNull(this.Fields["managerNote"]) ? null : reader.GetString(this.Fields["managerNote"])
                , reader.IsDBNull(this.Fields["servicetype"]) ? null : reader.GetString(this.Fields["servicetype"])
                , reader.IsDBNull(this.Fields["storageNote"]) ? null : reader.GetString(this.Fields["storageNote"])
                , reader.IsDBNull(this.Fields["storagePoint"]) ? null : reader.GetString(this.Fields["storagePoint"])
                , reader.IsDBNull(this.Fields["importer"]) ? null : CustomBrokerWpf.References.Importers.FindFirstItem("Id", reader.GetInt32(this.Fields["importer"]))
                , reader.IsDBNull(this.Fields["managerid"]) ? null : CustomBrokerWpf.References.Managers.FindFirstItem("Id", reader.GetInt32(this.Fields["managerid"]))
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
        public System.Windows.Media.Brush CurrencyBackground
        {
            get { return this.DomainObject.CurrencyChanged ? mychangedbrush : mystillbrush; }
        }
        public System.Windows.Media.Brush CurrencyBackgroundSelected
        { get { return this.DomainObject.CurrencyChanged ? mychangedbrushselected : mystillbrushselected; } }
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
            mysync = new RequestHistorySynchronizer();
            mysync.DomainCollection = new System.Collections.ObjectModel.ObservableCollection<RequestHistory>();
            mydbm = new RequestHistoryDBM();
            mydbm.Request = request;
            mydbm.FillAsyncCompleted = () => { if (mydbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); };
            mydbm.Collection = mysync.DomainCollection;
            mydbm.FillAsync();
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
