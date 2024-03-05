using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using excel = Microsoft.Office.Interop.Excel;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using KirillPolyanskiy.DataModelClassLibrary;
using MailKit;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public struct ParcelRecord
    {
        internal int id;
        internal long stamp;
        internal string updater;
        internal DateTime? updated;
        
        internal string parcelnumber;
        internal int status;
        internal int parceltype;
        internal DateTime shipplandate;
        internal DateTime? shipdate;
        internal DateTime? prepared;
        internal DateTime? crossedborder;
        internal DateTime? terminalin;
        internal DateTime? terminalout;
        internal DateTime? unloaded;
        internal string carrier;
        internal string carrierperson;
        internal string carriertel;
        internal string declaration;
        internal string docdirpath;
        internal int? goodstype;
        internal string lorry;
        internal string lorryregnum;
        internal decimal? lorrytonnage;
        internal decimal? lorryvolume;
        internal string lorryvin;
        internal string shipmentnumber;
        internal string trailerregnum;
        internal string trailervin;
        internal string trucker;
        internal string truckertel;
        internal decimal? deliveryprice;
        internal decimal? insuranceprice;
        internal decimal? tdeliveryprice;
        internal decimal? tinsuranceprice;
        internal decimal? transportd;
        internal decimal? transportt;
        internal decimal? usdrate;
        internal DateTime? ratedate;
    }

    public class Parcel : lib.DomainBaseStamp
    {
        private decimal? mylorrytonnage, mylorryvolume, myusdrate;
        private string mycarrier, mycarrierperson, mycarriertel, mydeclaration, mydocdirpath, mylorry, mylorryregnum, mylorryvin, myparcelnumber, myshipmentnumber, mytrailerregnum, mytrailervin, mytrucker, mytruckertel;
        private DateTime myshipplandate;
        private DateTime? mycrossedborder, myprepared, myratedate, myshipdate, myterminalin, myterminalout, myunloaded;
        private lib.ReferenceSimpleItem mystatus, myparceltype, mygoodstype;

        public Parcel(int id, long stamp, string updater, DateTime? updated, lib.DomainObjectState domainstate
            , string parcelnumber, lib.ReferenceSimpleItem status, lib.ReferenceSimpleItem parceltype
            , DateTime shipplandate, DateTime? shipdate, DateTime? prepared, DateTime? crossedborder, DateTime? terminalin, DateTime? terminalout, DateTime? unloaded
            , string carrier, string carrierperson, string carriertel, string declaration, string docdirpath, lib.ReferenceSimpleItem goodstype
            , string lorry, string lorryregnum, decimal? lorrytonnage, decimal? lorryvolume, string lorryvin
            , string shipmentnumber, string trailerregnum, string trailervin, string trucker, string truckertel
            , decimal? deliveryprice, decimal? insuranceprice, decimal? tdeliveryprice, decimal? tinsuranceprice,decimal? transportd,decimal? transportt
            , decimal? usdrate, DateTime? ratedate
            ) : base(id, stamp, updated, updater, domainstate)
        {
            myparcelnumber = parcelnumber;
            mystatus = status;
            myparceltype = parceltype;
            myshipplandate = shipplandate;
            myshipdate = shipdate;
            myprepared = prepared;
            mycrossedborder = crossedborder;
            myterminalin = terminalin;
            myterminalout = terminalout;
            myunloaded = unloaded;
            mycarrier = carrier;
            mycarrierperson = carrierperson;
            mycarriertel = carriertel;
            mydeclaration = declaration;
            mydocdirpath = docdirpath;
            mygoodstype = goodstype;
            mylorry = lorry;
            mylorryregnum = lorryregnum;
            mylorrytonnage = lorrytonnage;
            mylorryvolume = lorryvolume;
            mylorryvin = lorryvin;
            myshipmentnumber = shipmentnumber;
            mytrailerregnum = trailerregnum;
            mytrailervin = trailervin;
            mytrucker = trucker;
            mytruckertel = truckertel;
            mydeliveryprice = deliveryprice;
            myinsuranceprice = insuranceprice;
            mytdeliveryprice = tdeliveryprice;
            mytinsuranceprice = tinsuranceprice;
            myusdrate = usdrate;
            myratedate = ratedate;
            mytransportd = transportd;
            mytransportt = transportt;

            myrater = new CurrencyRateProxy(CustomBrokerWpf.References.CurrencyRate);
            myrater.PropertyChanged += Rater_PropertyChanged;
        }
        public Parcel() : this(id: lib.NewObjectId.NewId, stamp: 0, updated: null, updater: null, domainstate: lib.DomainObjectState.Added
            , parcelnumber: null, status: CustomBrokerWpf.References.RequestStates.FindFirstItem("Name", "Загрузка"), parceltype: CustomBrokerWpf.References.ParcelTypes.FindFirstItem("Id", 2)
            , shipplandate: DateTime.Today, shipdate: null, prepared: null, crossedborder: null, terminalin: null, terminalout: null, unloaded: null
            , carrier: null, carrierperson: null, carriertel: null, declaration: null, docdirpath: null, goodstype: null
            , lorry: null, lorryregnum: null, lorrytonnage: null, lorryvolume: null, lorryvin: null
            , shipmentnumber: null, trailerregnum: null, trailervin: null, trucker: null, truckertel: null
            , deliveryprice: null, insuranceprice: null, tdeliveryprice: null, tinsuranceprice: null,transportd: null,transportt: null
            , usdrate: null, ratedate: null
            )
        { }

        public string Carrier
        {
            set
            {
                SetProperty<string>(ref mycarrier, value);
            }
            get { return mycarrier; }
        }
        public string CarrierPerson
        {
            set
            {
                SetProperty<string>(ref mycarrierperson, value);
            }
            get { return mycarrierperson; }
        }
        public string CarrierTel
        {
            set
            {
                SetProperty<string>(ref mycarriertel, value);
            }
            get { return mycarriertel; }
        }
        public DateTime? CrossedBorder
        {
            set
            {
                Action action = () =>
                {
                    if (!myterminalin.HasValue & !myterminalout.HasValue & !myunloaded.HasValue)
                    {
                        if (value.HasValue)
                            this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 80);
                        else
                        {
                            if (myprepared.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 70);
                            else if (myshipdate.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 60);
                            else
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 50);
                        }
                    }
                };
                SetProperty<DateTime?>(ref mycrossedborder, value, action);
            }
            get { return mycrossedborder; }
        }
        public string Declaration
        {
            set
            {
                SetProperty<string>(ref mydeclaration, value);
            }
            get { return mydeclaration; }
        }
        public string DocDirPath
        {
            set
            {
                SetProperty<string>(ref mydocdirpath, value);
            }
            get { return mydocdirpath; }
        }
        public lib.ReferenceSimpleItem GoodsType
        {
            set
            {
                SetProperty<lib.ReferenceSimpleItem>(ref mygoodstype, value);
            }
            get { return mygoodstype; }
        }
        public string Lorry
        {
            set
            {
                SetProperty<string>(ref mylorry, value, () => { this.PropertyChangedNotification("ParcelNumberEntire"); });
            }
            get { return mylorry; }
        }
        public string LorryRegNum
        {
            set
            {
                SetProperty<string>(ref mylorryregnum, value);
            }
            get { return mylorryregnum; }
        }
        public decimal? LorryTonnage
        {
            set
            {
                SetProperty<decimal?>(ref mylorrytonnage, value, () => { PropertyChangedNotification("OverWeight"); });
            }
            get { return mylorrytonnage; }
        }
        public string LorryVIN
        {
            set
            {
                SetProperty<string>(ref mylorryvin, value);
            }
            get { return mylorryvin; }
        }
        public decimal? LorryVolume
        {
            set
            {
                SetProperty<decimal?>(ref mylorryvolume, value, () => { PropertyChangedNotification("OverVolume"); });
            }
            get { return mylorryvolume; }
        }
        public string ParcelNumber
        {
            set { SetProperty<string>(ref myparcelnumber, value, () => { this.PropertyChangedNotification("ParcelNumberEntire"); }); }
            get { return myparcelnumber; }
        }
        public string ParcelNumberEntire
        { get { return (myparcelnumber ?? string.Empty) + '-' + ((mylorry?.Trim()) ?? string.Empty) + '-' + myshipplandate.ToString("yy"); } }
        public string ParcelNumberOrder
        {
            get { return this.ShipPlanDate.Year.ToString() + (this.ParcelNumber ?? "9999").PadLeft(4, '0'); }
        }
        public lib.ReferenceSimpleItem ParcelType
        {
            set
            {
                base.SetProperty<lib.ReferenceSimpleItem>(ref myparceltype, value);
            }
            get { return myparceltype; }
        }
        public DateTime? Prepared
        {
            set
            {
                Action action = () =>
                {
                    if (!mycrossedborder.HasValue & !myterminalin.HasValue & !myterminalout.HasValue & !myunloaded.HasValue)
                    {
                        if (value.HasValue)
                            this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 70);
                        else
                        {
                            if (myshipdate.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 60);
                            else
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 50);
                        }
                    }
                };
                SetProperty<DateTime?>(ref myprepared, value, action);
            }
            get { return myprepared; }
        }
        public DateTime? RateDate
        {
            set
            {
                Action action = () =>
                {
                    this.UsdRate = null;
                    if (myratedate.HasValue)
                    {
                        myrater.RateDate = myratedate.Value;
                    }
                };
                SetProperty<DateTime?>(ref myratedate, value, action);
            }
            get { return myratedate; }
        }
        public DateTime? ShipDate
        {
            set
            {
                Action action = () =>
                {
                    if (!myprepared.HasValue & !mycrossedborder.HasValue & !myterminalin.HasValue & !myterminalout.HasValue & !myunloaded.HasValue)
                    {
                        if (value.HasValue)
                            this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 60);
                        else
                        {
                            this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 50);
                        }
                    }
                };
                SetProperty<DateTime?>(ref myshipdate, value, action);
            }
            get { return myshipdate; }
        }
        public DateTime ShipPlanDate
        {
            set { SetProperty<DateTime>(ref myshipplandate, value, () => { this.PropertyChangedNotification("ParcelNumberEntire"); }); }
            get { return myshipplandate; }
        }
        public string ShipmentNumber
        {
            set { SetProperty<string>(ref myshipmentnumber, value); }
            get { return myshipmentnumber; }
        }
        public lib.ReferenceSimpleItem Status
        {
            set
            {
                lib.ReferenceSimpleItem oldstatus = mystatus;
                base.SetProperty<lib.ReferenceSimpleItem>(ref mystatus, value, () =>
                {
                    if (!this.RequestsIsNull && mystatus.Id < 100 && oldstatus.Id < 100)
                    {
                        foreach (Request item in this.Requests.Where((Request rq) => { return rq.Parcel == this; }))
                            if (item.Status == oldstatus)
                                item.Status = this.Status;
                    }
                    else if (mystatus.Id == 110)
                    {
                        lib.ReferenceSimpleItem storemoscow = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 104);
                        lib.ReferenceSimpleItem issued = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 120);
                        try
                        { // есть ли склад москва
                            using (SqlConnection connection = new SqlConnection(CustomBrokerWpf.References.ConnectionString))
                            {
                                connection.Open();
                                WarehouseRUDBM wdbm = new WarehouseRUDBM();
                                wdbm.Connection = connection;
                                foreach (Request item in this.Requests.Where((Request rq) => { return rq.Parcel == this; }))
                                    if (item.Status.Id == 100) // растаможен
                                    {
                                        wdbm.Legal = item.CustomerLegals.FirstOrDefault((RequestCustomerLegal legal) => { return legal.Selected; });
                                        wdbm.Fill();

                                        if (wdbm.Collection.Count > 0)
                                            item.Status = storemoscow;
                                        else
                                            item.Status = issued;
                                    }
                                connection.Close();
                                if (wdbm.Errors.Count > 0)
                                    CustomBrokerWpf.References.PopupMessage(wdbm.ErrorMessage, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            CustomBrokerWpf.References.PopupMessage(ex.Message, true);
                        }
                    }
                }); //Count(); PropertiesChangedNotifycation();
            }
            get { return mystatus; }
        }
        public DateTime? TerminalIn
        {
            set
            {
                Action action = () =>
                {
                    if (!myterminalout.HasValue & !myunloaded.HasValue)
                    {
                        if (value.HasValue)
                            this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 90);
                        else
                        {
                            if (mycrossedborder.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 80);
                            else if (myprepared.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 70);
                            else if (myshipdate.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 60);
                            else
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 50);
                        }
                    }
                };
                SetProperty<DateTime?>(ref myterminalin, value, action);
            }
            get { return myterminalin; }
        }
        public DateTime? TerminalOut
        {
            set
            {
                Action action = () =>
                {
                    if (!myunloaded.HasValue)
                    {
                        if (value.HasValue)
                            this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 100);
                        else
                        {
                            if (myterminalin.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 90);
                            else if (mycrossedborder.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 80);
                            else if (myprepared.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 70);
                            else if (myshipdate.HasValue)
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 60);
                            else
                                this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 50);
                        }
                    }
                };
                SetProperty<DateTime?>(ref myterminalout, value, action);
            }
            get { return myterminalout; }
        }
        public string TrailerRegNum
        {
            set { SetProperty<string>(ref mytrailerregnum, value); }
            get { return mytrailerregnum; }
        }
        public string TrailerVIN
        {
            set { SetProperty<string>(ref mytrailervin, value); }
            get { return mytrailervin; }
        }
        public string Trucker
        {
            set { SetProperty<string>(ref mytrucker, value); }
            get { return mytrucker; }
        }
        public string TruckerTel
        {
            set { SetProperty<string>(ref mytruckertel, value); }
            get { return mytruckertel; }
        }
        public DateTime? Unloaded
        {
            set
            {
                Action action = () =>
                {
                    if (value.HasValue)
                        this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 110);
                    else if (myterminalout.HasValue)
                        this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 100);
                    else if (myterminalin.HasValue)
                        this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 90);
                    else if (mycrossedborder.HasValue)
                        this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 80);
                    else if (myprepared.HasValue)
                        this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 70);
                    else if (myshipdate.HasValue)
                        this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 60);
                    else
                        this.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 50);
                };
                SetProperty<DateTime?>(ref myunloaded, value, action);
            }
            get { return myunloaded; }
        }
        public decimal? UsdRate
        {
            set
            {
                SetProperty<decimal?>(ref myusdrate, value);
            }
            get { return myusdrate; }
        }

        private decimal? mydeliveryprice, myinsuranceprice, mytdeliveryprice, mytinsuranceprice,mytransportd, mytransportt;
        public decimal? DeliveryPrice
        {
            set
            {
                SetProperty<decimal?>(ref mydeliveryprice, value);
            }
            get { return mydeliveryprice; }
        }
        public decimal? InsurancePrice
        {
            set
            {
                SetProperty<decimal?>(ref myinsuranceprice, value);
            }
            get { return myinsuranceprice; }
        }
        public decimal? TDeliveryPrice
        {
            set
            {
                SetProperty<decimal?>(ref mytdeliveryprice, value);
            }
            get { return mytdeliveryprice; }
        }
        public decimal? TInsurancePrice
        {
            set
            {
                SetProperty<decimal?>(ref mytinsuranceprice, value);
            }
            get { return mytinsuranceprice; }
        }
        public decimal? TransportD
        {
            set { SetProperty<decimal?>(ref mytransportd, value, ()=> { this.PropertyChangedNotification(nameof(this.TransportDUn)); }); }
            get { return mytransportd; }
        }
        public decimal? TransportT
        {
            set { SetProperty<decimal?>(ref mytransportt, value, () => { this.PropertyChangedNotification(nameof(this.TransportTUn)); }); }
            get { return mytransportt; }
        }
        internal decimal? TransportDUn
        {
            get {
                return RequestsTotalDelivery.Volume == 0M ? null : mytransportd / RequestsTotalDelivery.Volume;
            }
        }
        internal decimal? TransportTUn
        {
            get {
                return RequestsTotalTrade.Volume == 0M ? null : mytransportt / RequestsTotalTrade.Volume;
            }
        }

        //private ImporterParcelRequestTotal mytotal;
        //public ImporterParcelRequestTotal RequestTotal
        //{
        //    get
        //    {
        //        if (mytotal == null)
        //        {
        //            mytotal = new ImporterParcelRequestTotal(this, null);
        //            mytotal.Requests = this.Requests;

        //            PropertyChangedNotification("OverVolume");
        //            PropertyChangedNotification("OverWeight");
        //        }
        //        return mytotal;
        //    }
        //}
        //private ImporterParcelRequestTotal mytotald;
        //public ImporterParcelRequestTotal RequestTotalD
        //{
        //    get
        //    {
        //        if (mytotald == null)
        //        {
        //            mytotald = new ImporterParcelRequestTotal(this, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2));
        //            mytotald.Requests = this.Requests;
        //            mytotald.PropertyChanged += RequestTotalDT_PropertyChanged;
        //        }
        //        return mytotald;
        //    }
        //}
        //private ImporterParcelRequestTotal mytotalt;
        //public ImporterParcelRequestTotal RequestTotalT
        //{
        //    get
        //    {
        //        if (mytotalt == null)
        //        {
        //            mytotalt = new ImporterParcelRequestTotal(this, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1));
        //            mytotalt.Requests = this.Requests;
        //            mytotalt.PropertyChanged += RequestTotalDT_PropertyChanged;

        //        }
        //        return mytotalt;
        //    }
        //}

        //public bool OverVolume
        //{ get { return (this.LorryVolume ?? 0M) - RequestTotal.Volume < 0; } }
        //public bool OverWeight
        //{ get { return (this.LorryTonnage ?? 0M) - RequestTotal.ActualWeight < 0; } }

        #region Free
        private decimal myactualweightfree;
        public decimal ActualWeightFree
        {
            get { return myactualweightfree; }
        }
        private decimal mycellnumberfree;
        public decimal CellNumberFree
        { get { return mycellnumberfree; } }
        public decimal DifferenceWeightFree
        { get { return myactualweightfree - myofficialweightfree; } }
        private decimal myinvoicefree;
        public decimal InvoiceFree
        {
            get { return myinvoicefree; }
        }
        private decimal myinvoicediscountfree;
        public decimal InvoiceDiscountFree
        {
            get { return myinvoicediscountfree; }
        }
        private decimal myofficialweightfree;
        public decimal OfficialWeightFree
        { get { return myofficialweightfree; } }
        private decimal myvolumefree;
        public decimal VolumeFree
        { get { return myvolumefree; } }

        //private void Count()
        //{
        //    if (this.RequestsIsNull) return;
        //    myactualweightfree = 0M;
        //    mycellnumberfree = 0M;
        //    myinvoicefree = 0M;
        //    myinvoicediscountfree = 0M;
        //    myofficialweightfree = 0M;
        //    myvolumefree = 0M;
        //    foreach (Request item in myrequests)
        //    {
        //        item.ValueChanged -= Request_ValueChanged;
        //        item.PropertyChanged -= Request_PropertyChanged;
        //    }
        //    if (mystatus.Id < 60)
        //        foreach (Request item in myrequests)
        //        {
        //            item.ValueChanged += Request_ValueChanged;
        //            if (!item.ParcelId.HasValue)
        //            {
        //                item.PropertyChanged += Request_PropertyChanged;
        //                if (item.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
        //                    ValuesPlus(item);
        //            }
        //        }
        //    PropertiesChangedNotifycation();
        //}
        //private void ValuesPlus(Request item)
        //{
        //    myactualweightfree += item.ActualWeight ?? 0M;
        //    mycellnumberfree += item.CellNumber ?? 0;
        //    myinvoicefree += item.Invoice ?? 0M;
        //    myinvoicediscountfree += item.InvoiceDiscount ?? 0M;
        //    myofficialweightfree += item.OfficialWeight ?? 0M;
        //    myvolumefree += item.Volume ?? 0M;
        //}
        //private void ValuesMinus(Request item)
        //{
        //    myactualweightfree -= item.ActualWeight ?? 0M;
        //    mycellnumberfree -= item.CellNumber ?? 0;
        //    myinvoicefree -= item.Invoice ?? 0M;
        //    myinvoicediscountfree -= item.InvoiceDiscount ?? 0M;
        //    myofficialweightfree -= item.OfficialWeight ?? 0M;
        //    myvolumefree -= item.Volume ?? 0M;
        //}
        private void PropertiesChangedNotifycation()
        {
            PropertyChangedNotification("ActualWeightFree");
            PropertyChangedNotification("CellNumberFree");
            PropertyChangedNotification("DifferenceWeightFree");
            PropertyChangedNotification("InvoiceFree");
            PropertyChangedNotification("InvoiceDiscountFree");
            PropertyChangedNotification("OfficialWeightFree");
            PropertyChangedNotification("VolumeFree");
        }
        #endregion

        RequestDBM myrdbm;
        private ObservableCollection<Request> myrequests;
        public ObservableCollection<Request> Requests
        {
            get
            {
                if (myrequests == null)
                {
                    myrequests = new ObservableCollection<Request>();
                    myrdbm = new RequestDBM();
                    myrdbm.Parcel = this;
                    myrdbm.FillType = lib.FillType.PrefExist;
                    myrequestsloaded = false;
                    myrdbm.FillAsyncCompleted = () =>
                    {
                        if (myrdbm.Errors.Count > 0) throw new Exception(myrdbm.ErrorMessage);
                        else
                        {
                            myrdbm = null;
                            //myrequests.CollectionChanged += Requests_CollectionChanged;
                            //Count();
                            //ForegroundNotifyChanged();
                            if (myspecifications != null)
                                for (int i = 0; i < myspecifications.Count; i++)
                                    myspecifications[i].CustomersLegalsRefresh();
                            myrequestsloaded = true;
                            this.PropertyChangedNotification(nameof(this.Requests));
                            this.PropertyChangedNotification(nameof(this.RequestsIsNull));
                            this.PropertyChangedNotification(nameof(this.RequestsIsLoaded));
                        }
                    };
                    myrdbm.Collection = myrequests;
                    myrdbm.FillAsync();
                }
                return myrequests;
            }
        }
        internal bool RequestsIsNull { get { return myrequests == null; } }
        private bool myrequestsloaded;
        internal bool RequestsIsLoaded { get { return myrequestsloaded; } }
        //private void Requests_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
        //        Count();
        //    else
        //    {
        //        if (e.NewItems != null)
        //            foreach (Request item in e.NewItems)
        //            {
        //                item.ValueChanged -= Request_ValueChanged; // объект из хранилища добавлен в коллекцию повторно при обновлении
        //                item.ValueChanged += Request_ValueChanged;
        //                if (!item.ParcelId.HasValue) // считаем
        //                {
        //                    item.PropertyChanged -= Request_PropertyChanged;
        //                    item.PropertyChanged += Request_PropertyChanged;
        //                    //if (item.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
        //                    //{ ValuesPlus(item); PropertiesChangedNotifycation(); }
        //                }
        //            }
        //        if (e.OldItems != null)
        //            foreach (Request item in e.OldItems)
        //            {
        //                item.ValueChanged -= Request_ValueChanged;
        //                if (!item.ParcelId.HasValue)
        //                {
        //                    item.PropertyChanged -= Request_PropertyChanged;
        //                    if (item.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
        //                    { ValuesMinus(item); PropertiesChangedNotifycation(); }
        //                }
        //            }
        //    }
        //}
        //private void Request_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "DomainState")
        //    {
        //        Request request = sender as Request;
        //        if (!request.ParcelId.HasValue)
        //        {
        //            if (request.DomainState == DataModelClassLibrary.DomainObjectState.Deleted & request.DomainStatePrevious < DataModelClassLibrary.DomainObjectState.Deleted)
        //            { ValuesMinus(request); PropertiesChangedNotifycation(); }
        //            else if (request.DomainStatePrevious == DataModelClassLibrary.DomainObjectState.Deleted & request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
        //            { ValuesPlus(request); PropertiesChangedNotifycation(); }
        //        }
        //    }
        //}
        //private void Request_ValueChanged(object sender, DataModelClassLibrary.Interfaces.ValueChangedEventArgs<object> e)
        //{
        //    Request request = sender as Request;
        //    switch (e.PropertyName)
        //    {
        //        case "ParcelId":
        //            {
        //                int? newvalue = (int?)e.NewValue, oldvalue = (int?)e.OldValue;
        //                if (!newvalue.HasValue && oldvalue.HasValue)// теперь считаем
        //                {
        //                    request.PropertyChanged += Request_PropertyChanged;
        //                    if (request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
        //                    { ValuesPlus(request); PropertiesChangedNotifycation(); }
        //                }
        //                else if (newvalue.HasValue && !oldvalue.HasValue) // больше не считаем
        //                {
        //                    request.PropertyChanged -= Request_PropertyChanged;
        //                    if (request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
        //                    { ValuesMinus(request); PropertiesChangedNotifycation(); }
        //                }
        //            }
        //            break;
        //        default:
        //            if (!request.ParcelId.HasValue)
        //            {
        //                {
        //                    decimal newvalue, oldvalue;
        //                    switch (e.PropertyName)
        //                    {
        //                        case "ActualWeight":
        //                            newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
        //                            myactualweightfree += newvalue - oldvalue;
        //                            PropertyChangedNotification("ActualWeightFree");
        //                            PropertyChangedNotification("DifferenceWeightFree");
        //                            break;
        //                        case "CellNumber":
        //                            newvalue = (short)(e.NewValue ?? (short)0); oldvalue = (short)(e.OldValue ?? (short)0);
        //                            mycellnumberfree += newvalue - oldvalue;
        //                            PropertyChangedNotification("CellNumberFree");
        //                            break;
        //                        case "Invoice":
        //                            newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
        //                            myinvoicefree += newvalue - oldvalue;
        //                            PropertyChangedNotification("InvoiceFree");
        //                            break;
        //                        case "InvoiceDiscount":
        //                            newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
        //                            myinvoicediscountfree += newvalue - oldvalue;
        //                            PropertyChangedNotification("InvoiceDiscountFree");
        //                            break;
        //                        case "OfficialWeight":
        //                            newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
        //                            myofficialweightfree += newvalue - oldvalue;
        //                            PropertyChangedNotification("OfficialWeightFree");
        //                            PropertyChangedNotification("DifferenceWeightFree");
        //                            break;
        //                        case "Volume":
        //                            newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
        //                            myvolumefree += newvalue - oldvalue;
        //                            PropertyChangedNotification("VolumeFree");
        //                            break;
        //                    }
        //                }
        //            }
        //            break;
        //    }
        //}
        private ParcelRequestsTotal myrequeststotald;
        public ParcelRequestsTotal RequestsTotalDelivery
        {
            get 
            {
                if(myrequeststotald==null)
                {
                    myrequeststotald = new ParcelRequestsTotal(this.Requests, this, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2));
                    myrequeststotald.PropertyChanged += RequestsTotalDelivery_PropertyChanged;
                    myrequeststotald.StartCount();
                    this.PropertyChangedNotification(nameof(this.RequestsTotalDelivery));
                }
                return myrequeststotald; 
            } 
        }
        private void RequestsTotalDelivery_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ParcelRequestsTotal.Volume):
                    this.PropertyChangedNotification(nameof(this.TransportDUn));
                    break;
            }
        }
        private ParcelRequestsTotal myrequeststotalt;
        public ParcelRequestsTotal RequestsTotalTrade
        {
            get
            {
                if (myrequeststotalt == null)
                {
                    myrequeststotalt = new ParcelRequestsTotal(this.Requests, this, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1));
                    myrequeststotalt.PropertyChanged += RequestsTotalTrade_PropertyChanged;
                    myrequeststotalt.StartCount();
                    this.PropertyChangedNotification(nameof(this.RequestsTotalTrade));
                }
                return myrequeststotalt;
            }
        }
        private void RequestsTotalTrade_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(ParcelRequestsTotal.Volume):
                    this.PropertyChangedNotification(nameof(this.TransportTUn));
                    break;
            }
        }

        private ObservableCollection<Specification.Specification> myspecifications;
        public ObservableCollection<Specification.Specification> Specifications
        {
            get
            {
                if (myspecifications == null)
                    SpecificationLoad();
                return myspecifications;
            }
        }
        internal bool SpecificationsIsNull { get { return myspecifications == null; } }
        internal void SpecificationsRefresh()
        {
            if (myspecifications != null)
            {
                SpecificationLoad(lib.FillType.Refresh);
                this.PropertyChangedNotification("Specifications");
            }
        }
        private void SpecificationLoad(lib.FillType filltype = lib.FillType.PrefExist)
        {
            Specification.SpecificationDBM sdbm = new Specification.SpecificationDBM();
            sdbm.Parcel = this;
            sdbm.FillAsyncCompleted = () => { if (sdbm.Errors.Count > 0) throw new Exception(sdbm.ErrorMessage); };
            sdbm.Collection = myspecifications;
            sdbm.FillType = filltype;
            sdbm.Fill();
            if (sdbm.Errors.Count > 0) System.Windows.MessageBox.Show(sdbm.ErrorMessage, "Загрузка ГТД", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            if (myspecifications == null)
                myspecifications = sdbm.Collection;
        }

        private ParcelMailState mymailstate;
        public ParcelMailState MailState
        {
            get { if (mymailstate == null) { mymailstate = new ParcelMailState(this); mymailstate.PropertyChanged += MailState_PropertyChanged; } return mymailstate; }
        }
        private void MailState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChangedNotification("MailState" + e.PropertyName);
        }

        private Classes.CurrencyRateProxy myrater;
        private void Rater_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EURRate" & myratedate.HasValue)
            {
                this.UsdRate = myrater.EURRate;
            }
        }


        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            Parcel newitem = (Parcel)sample;

            this.Carrier = newitem.Carrier;
            this.CarrierPerson = newitem.CarrierPerson;
            this.CarrierTel = newitem.CarrierTel;
            this.CrossedBorder = newitem.CrossedBorder;
            this.Declaration = newitem.Declaration;
            this.DocDirPath = newitem.DocDirPath;
            this.GoodsType = newitem.GoodsType;
            this.Lorry = newitem.Lorry;
            this.LorryRegNum = newitem.LorryRegNum;
            this.LorryTonnage = newitem.LorryTonnage;
            this.LorryVIN = newitem.LorryVIN;
            this.LorryVolume = newitem.LorryVolume;
            this.ParcelNumber = newitem.ParcelNumber;
            this.ParcelType = newitem.ParcelType;
            this.RateDate = newitem.RateDate;
            this.Prepared = newitem.Prepared;
            this.ShipDate = newitem.ShipDate;
            this.ShipPlanDate = newitem.ShipPlanDate;
            this.ShipmentNumber = newitem.ShipmentNumber;
            this.Status = newitem.Status;
            this.TerminalIn = newitem.TerminalIn;
            this.TerminalOut = newitem.TerminalOut;
            this.TrailerRegNum = newitem.TrailerRegNum;
            this.TrailerVIN = newitem.TrailerVIN;
            this.Trucker = newitem.Trucker;
            this.TruckerTel = newitem.TruckerTel;
            this.Unloaded = newitem.Unloaded;
            this.UsdRate = newitem.UsdRate;

            this.DeliveryPrice = newitem.DeliveryPrice;
            this.InsurancePrice = newitem.InsurancePrice;
            this.TDeliveryPrice = newitem.TDeliveryPrice;
            this.TInsurancePrice = newitem.TInsurancePrice;
            this.TransportD = newitem.TransportD;
            this.TransportT = newitem.TransportT;
        }
		public override bool ValidateProperty(string propertyname, object value, out string errmsg, out byte errmsgkey)
		{
            bool isvalid = true;
            errmsg = null;
            errmsgkey = 0;
            switch (propertyname)
            {
                case nameof(this.Lorry):
                    string str = (string)value;
                    if (!lib.Common.PathExtension.CheckInvalidChars(str))
                    { 
                        errmsg = "Указан некорректный номер машины!";
                        isvalid = false;
                    }
                    break;
            }
            return isvalid;
        }
    }

    internal class ParcelStore : lib.DomainStorageLoad<ParcelRecord,Parcel, ParcelDBM>
    {
        public ParcelStore(ParcelDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(Parcel olditem, Parcel newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class ParcelDBM : lib.DBManagerStamp<ParcelRecord,Parcel>
    {
        public ParcelDBM()
        {
            this.NeedAddConnection = true;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.Parcel_sp";
            InsertCommandText = "dbo.ParcelAdd_sp";
            UpdateCommandText = "dbo.ParcelUpd_sp";
            DeleteCommandText = "dbo.ParcelDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@id", System.Data.SqlDbType.Int),
                new SqlParameter("@filterId", System.Data.SqlDbType.Int)
            };
            base.SelectParams[1].Value = 0;
            myinsertparams[0].ParameterName = "@parcelId";
            myupdateparams[0].ParameterName = "@parcelId";
            mydeleteparams[0].ParameterName = "@parcelId";
            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0],myinsertparams[1]
                ,new SqlParameter("@parcelstatus", System.Data.SqlDbType.Int)
                ,new SqlParameter("@docdirpath", System.Data.SqlDbType.NVarChar,100) {Direction = System.Data.ParameterDirection.Output}
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@borderdatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@carrierpersontrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@carrierteltrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@carriertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@declarationtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@deliverypricetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@docdirpath", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@docdirpathtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@goodstypetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@insurancepricetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@lorryregnumtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@lorrytonnagetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@lorrytrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@lorryvintrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@lorryvolumetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@parcelnumbertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@parcelstatus", System.Data.SqlDbType.Int) {Direction = System.Data.ParameterDirection.InputOutput}
                ,new SqlParameter("@parcelstatustrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@parceltypetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@preparationtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@ratedate", System.Data.SqlDbType.Date)
                ,new SqlParameter("@ratedateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@shipdatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@shipmentnumbertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@shipplandatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@tdeliverypricetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@terminalintrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@terminalouttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@tinsurancepricetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@trailerregnumtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@trailervintrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@truckerteltrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@truckertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@unloadedtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@gtlsrate", System.Data.SqlDbType.Money)
                ,new SqlParameter("@gtlsrateupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@transportdupd", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@transporttupd", System.Data.SqlDbType.Bit)

                ,new SqlParameter("@old",false)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@parcelnumber", System.Data.SqlDbType.NVarChar,5) {Direction = System.Data.ParameterDirection.InputOutput}
                ,new SqlParameter("@parceltype", System.Data.SqlDbType.TinyInt)
                ,new SqlParameter("@declaration", System.Data.SqlDbType.NVarChar,106)
                ,new SqlParameter("@goodstype", System.Data.SqlDbType.Int)
                ,new SqlParameter("@shipmentnumber", System.Data.SqlDbType.NChar,6)
                ,new SqlParameter("@shipplandate", System.Data.SqlDbType.DateTime)
                ,new SqlParameter("@shipdate", System.Data.SqlDbType.DateTime)
                ,new SqlParameter("@preparation", System.Data.SqlDbType.DateTime)
                ,new SqlParameter("@borderdate", System.Data.SqlDbType.DateTime)
                ,new SqlParameter("@terminalin", System.Data.SqlDbType.DateTime)
                ,new SqlParameter("@terminalout", System.Data.SqlDbType.DateTime)
                ,new SqlParameter("@unloaded", System.Data.SqlDbType.DateTime)
                ,new SqlParameter("@carrier", System.Data.SqlDbType.NVarChar,100)
                ,new SqlParameter("@carrierperson", System.Data.SqlDbType.NVarChar,30)
                ,new SqlParameter("@carriertel", System.Data.SqlDbType.NVarChar,20)
                ,new SqlParameter("@lorry", System.Data.SqlDbType.NVarChar,5)
                ,new SqlParameter("@lorryregnum", System.Data.SqlDbType.NVarChar,20)
                ,new SqlParameter("@lorryvin", System.Data.SqlDbType.NVarChar,20)
                ,new SqlParameter("@lorryvolume", System.Data.SqlDbType.Money)
                ,new SqlParameter("@lorrytonnage", System.Data.SqlDbType.Money)
                ,new SqlParameter("@trailerregnum", System.Data.SqlDbType.NVarChar,20)
                ,new SqlParameter("@trailervin", System.Data.SqlDbType.NVarChar,20)
                ,new SqlParameter("@trucker", System.Data.SqlDbType.NVarChar,30)
                ,new SqlParameter("@truckertel", System.Data.SqlDbType.NVarChar,20)
                ,new SqlParameter("@deliveryprice", System.Data.SqlDbType.Money)
                ,new SqlParameter("@insuranceprice", System.Data.SqlDbType.Money)
                ,new SqlParameter("@tdeliveryprice", System.Data.SqlDbType.Money)
                ,new SqlParameter("@tinsuranceprice", System.Data.SqlDbType.Money)
                ,new SqlParameter("@transportd", System.Data.SqlDbType.Money)
                ,new SqlParameter("@transportt", System.Data.SqlDbType.Money)
            };

            myrdbm = new RequestDBM() { Command = new SqlCommand(), LegalDBM = new RequestCustomerLegalDBM() { LegalDBM = new CustomerLegalDBM() } };
            mysdbm = new Specification.SpecificationDBM(); mysdbm.Command = new SqlCommand();
        }

        internal int Filter
        { set { base.SelectParams[0].Value = null; base.SelectParams[1].Value = value; } get { return (int)base.SelectParams[1].Value; } }
        internal int ParcelId
        {
            set { base.SelectParams[0].Value = value; base.SelectParams[1].Value = 0; }
            get { return (int)base.SelectParams[0].Value; }
        }
        private RequestDBM myrdbm;
        internal RequestDBM RequestDBM
        { set { myrdbm = value; } get { return myrdbm; } }
        internal bool RequestRefreshFill { set; get; } // load request for new parcel
        private Specification.SpecificationDBM mysdbm;

		protected override ParcelRecord CreateRecord(SqlDataReader reader)
		{
			return new ParcelRecord()
            {
                id = reader.GetInt32(0), stamp = reader.GetInt32(this.Fields["stamp"]), updater = reader.IsDBNull(this.Fields["UpdateWho"]) ? null : reader.GetString(this.Fields["UpdateWho"]), updated = reader.IsDBNull(this.Fields["UpdateWhen"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["UpdateWhen"])
                , parcelnumber = reader.GetString(this.Fields["parcelnumber"])
                , status = reader.GetInt32(this.Fields["parcelstatus"])
                , parceltype = (int)reader.GetByte(this.Fields["parceltype"])
                , shipplandate = reader.GetDateTime(this.Fields["shipplandate"])
                , shipdate = reader.IsDBNull(this.Fields["shipdate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["shipdate"])
                , prepared = reader.IsDBNull(this.Fields["preparation"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["preparation"])
                , crossedborder = reader.IsDBNull(this.Fields["borderdate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["borderdate"])
                , terminalin = reader.IsDBNull(this.Fields["terminalin"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["terminalin"])
                , terminalout = reader.IsDBNull(this.Fields["terminalout"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["terminalout"])
                , unloaded = reader.IsDBNull(this.Fields["unloaded"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["unloaded"])
                , carrier = reader.IsDBNull(this.Fields["carrier"]) ? null : reader.GetString(this.Fields["carrier"])
                , carrierperson = reader.IsDBNull(this.Fields["carrierperson"]) ? null : reader.GetString(this.Fields["carrierperson"])
                , carriertel = reader.IsDBNull(this.Fields["carriertel"]) ? null : reader.GetString(this.Fields["carriertel"])
                , declaration = reader.IsDBNull(this.Fields["declaration"]) ? null : reader.GetString(this.Fields["declaration"])
                , docdirpath = reader.IsDBNull(this.Fields["docdirpath"]) ? null : reader.GetString(this.Fields["docdirpath"])
                , goodstype = reader.IsDBNull(this.Fields["goodstype"]) ? (int?)null : reader.GetInt32(this.Fields["goodstype"])
                , lorry = reader.IsDBNull(this.Fields["lorry"]) ? null : reader.GetString(this.Fields["lorry"])
                , lorryregnum = reader.IsDBNull(this.Fields["lorryregnum"]) ? null : reader.GetString(this.Fields["lorryregnum"])
                , lorrytonnage = reader.IsDBNull(this.Fields["lorrytonnage"]) ? (decimal?)null : reader.GetDecimal(this.Fields["lorrytonnage"])
                , lorryvolume = reader.IsDBNull(this.Fields["lorryvolume"]) ? (decimal?)null : reader.GetDecimal(this.Fields["lorryvolume"])
                , lorryvin = reader.IsDBNull(this.Fields["lorryvin"]) ? null : reader.GetString(this.Fields["lorryvin"])
                , shipmentnumber = reader.IsDBNull(this.Fields["shipmentnumber"]) ? null : reader.GetString(this.Fields["shipmentnumber"])
                , trailerregnum = reader.IsDBNull(this.Fields["trailerregnum"]) ? null : reader.GetString(this.Fields["trailerregnum"])
                , trailervin = reader.IsDBNull(this.Fields["trailervin"]) ? null : reader.GetString(this.Fields["trailervin"])
                , trucker = reader.IsDBNull(this.Fields["trucker"]) ? null : reader.GetString(this.Fields["trucker"])
                , truckertel = reader.IsDBNull(this.Fields["truckertel"]) ? null : reader.GetString(this.Fields["truckertel"])
                , deliveryprice = reader.IsDBNull(this.Fields["deliveryprice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["deliveryprice"])
                , insuranceprice = reader.IsDBNull(this.Fields["insuranceprice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["insuranceprice"])
                , tdeliveryprice = reader.IsDBNull(this.Fields["tdeliveryprice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["tdeliveryprice"])
                , tinsuranceprice = reader.IsDBNull(this.Fields["tinsuranceprice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["tinsuranceprice"])
                , transportd = reader.IsDBNull(this.Fields["transportd"]) ? (decimal?)null : reader.GetDecimal(this.Fields["transportd"])
                , transportt = reader.IsDBNull(this.Fields["transportt"]) ? (decimal?)null : reader.GetDecimal(this.Fields["transportt"])
                , usdrate = reader.IsDBNull(this.Fields["usdrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["usdrate"])
                , ratedate = reader.IsDBNull(this.Fields["ratedate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["ratedate"])
            };
		}
        protected override Parcel CreateModel(ParcelRecord record, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
            Parcel newitem = new Parcel(record.id, record.stamp, record.updater, record.updated, lib.DomainObjectState.Unchanged
                , record.parcelnumber
                , CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", record.status)
                , CustomBrokerWpf.References.ParcelTypes.FindFirstItem("Id", record.parceltype)
                , record.shipplandate
                , record.shipdate
                , record.prepared
                , record.crossedborder
                , record.terminalin
                , record.terminalout
                , record.unloaded
                , record.carrier
                , record.carrierperson
                , record.carriertel
                , record.declaration
                , record.docdirpath
                , record.goodstype.HasValue ? CustomBrokerWpf.References.GoodsTypesParcel.FindFirstItem("Id", record.goodstype.Value) : null
                , record.lorry
                , record.lorryregnum
                , record.lorrytonnage
                , record.lorryvolume
                , record.lorryvin
                , record.shipmentnumber
                , record.trailerregnum
                , record.trailervin
                , record.trucker
                , record.truckertel
                , record.deliveryprice
                , record.insuranceprice
                , record.tdeliveryprice
                , record.tinsuranceprice
                , record.transportd
                , record.transportt
                , record.usdrate
                , record.ratedate
                );
            Parcel item = CustomBrokerWpf.References.ParcelStore.UpdateItem(newitem, this.FillType == lib.FillType.Refresh);
            if ((!item.RequestsIsNull | (this.RequestRefreshFill && item == newitem)) && !canceltasktoken.IsCancellationRequested) // refresh if the pascel has requests or parcel is new and it needs to refresh requests
            {
                myrdbm.Command.Connection = addcon;
                if (mydispatcher.Thread.ManagedThreadId == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
                    RequestsRefresh(item);
                else
                    mydispatcher.Invoke(() => { RequestsRefresh(item); });
            }
            return item;
        }

        protected override void GetOutputParametersValue(Parcel item)
        {
            base.GetOutputParametersValue(item);
            if (item.DomainState == lib.DomainObjectState.Added)
                CustomBrokerWpf.References.ParcelStore.UpdateItem(item);
            SqlParameter par = myinsertupdateparams.Where((SqlParameter ipar) => { return ipar.ParameterName == "@parcelnumber"; }).First();
            if (par.Value != null)
                item.ParcelNumber = (string)par.Value;
            par = myupdateparams.Where((SqlParameter ipar) => { return ipar.ParameterName == "@parcelstatus"; }).First();
            if (par.Value != null)
                item.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", par.Value);
            par = myinsertparams.Where((SqlParameter ipar) => { return ipar.ParameterName == "@docdirpath"; }).First();
            if (par.Value != null)
                item.DocDirPath = (string)par.Value;
        }
        protected override bool SaveChildObjects(Parcel item)
        {
            bool isSuccess = true;
            if (!item.SpecificationsIsNull) // меняем статус заявок
            {
                mysdbm.Errors.Clear();
                mysdbm.Parcel = item;
                mysdbm.Collection = item.Specifications;
                mysdbm.Command.Connection = this.Command.Connection;
                if (!mysdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    foreach (lib.DBMError err in mysdbm.Errors) this.Errors.Add(err);
                }
            }
            if (myrdbm != null && !item.RequestsIsNull)
            {
                myrdbm.Errors.Clear();
                myrdbm.Parcel = item;
                myrdbm.Collection = item.Requests;
                myrdbm.Command.Connection = this.Command.Connection;
                if (!myrdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
                }
                myrdbm.Collection = null;
            }
            return isSuccess;
        }
        protected override bool SaveIncludedObject(Parcel item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            if (myrdbm != null) myrdbm.Command.Connection = this.Command.Connection;
            mysdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetParametersValue(Parcel item)
        {
            base.SetParametersValue(item);
            foreach (SqlParameter par in myinsertparams)
            {
                switch (par.ParameterName)
                {
                    case "@parcelstatus":
                        par.Value = item.Status.Id;
                        break;
                    case "@docdirpath":
                        par.Value = item.DocDirPath;
                        break;
                }
            }
            foreach (SqlParameter par in myupdateparams)
            {
                switch (par.ParameterName)
                {
                    case "@parcelstatus":
                        par.Value = item.Status.Id;
                        break;
                    case "@docdirpath":
                        par.Value = item.DocDirPath;
                        break;
                    case "@parceltypetrue":
                        par.Value = item.HasPropertyOutdatedValue("ParcelType");
                        break;
                    case "@parcelstatustrue":
                        par.Value = item.HasPropertyOutdatedValue("Status");
                        break;
                    case "@declarationtrue":
                        par.Value = item.HasPropertyOutdatedValue("Declaration");
                        break;
                    case "@docdirpathtrue":
                        par.Value = item.HasPropertyOutdatedValue("DocDirPath");
                        break;
                    case "@goodstypetrue":
                        par.Value = item.HasPropertyOutdatedValue("GoodsType");
                        break;
                    case "@shipmentnumbertrue":
                        par.Value = item.HasPropertyOutdatedValue("ShipmentNumber");
                        break;
                    case "@shipplandatetrue":
                        par.Value = item.HasPropertyOutdatedValue("ShipPlanDate");
                        break;
                    case "@shipdatetrue":
                        par.Value = item.HasPropertyOutdatedValue("ShipDate");
                        break;
                    case "@preparationtrue":
                        par.Value = item.HasPropertyOutdatedValue("Prepared");
                        break;
                    case "@borderdatetrue":
                        par.Value = item.HasPropertyOutdatedValue("CrossedBorder");
                        break;
                    case "@terminalintrue":
                        par.Value = item.HasPropertyOutdatedValue("TerminalIn");
                        break;
                    case "@terminalouttrue":
                        par.Value = item.HasPropertyOutdatedValue("TerminalOut");
                        break;
                    case "@unloadedtrue":
                        par.Value = item.HasPropertyOutdatedValue("Unloaded");
                        break;
                    case "@carriertrue":
                        par.Value = item.HasPropertyOutdatedValue("Carrier");
                        break;
                    case "@carrierpersontrue":
                        par.Value = item.HasPropertyOutdatedValue("CarrierPerson");
                        break;
                    case "@carrierteltrue":
                        par.Value = item.HasPropertyOutdatedValue("CarrierTel");
                        break;
                    case "@lorrytrue":
                        par.Value = item.HasPropertyOutdatedValue("Lorry");
                        break;
                    case "@lorryregnumtrue":
                        par.Value = item.HasPropertyOutdatedValue("LorryRegNum");
                        break;
                    case "@lorryvintrue":
                        par.Value = item.HasPropertyOutdatedValue("LorryVIN");
                        break;
                    case "@lorryvolumetrue":
                        par.Value = item.HasPropertyOutdatedValue("LorryVolume");
                        break;
                    case "@lorrytonnagetrue":
                        par.Value = item.HasPropertyOutdatedValue("LorryTonnage");
                        break;
                    case "@trailerregnumtrue":
                        par.Value = item.HasPropertyOutdatedValue("TrailerRegNum");
                        break;
                    case "@trailervintrue":
                        par.Value = item.HasPropertyOutdatedValue("TrailerVIN");
                        break;
                    case "@truckertrue":
                        par.Value = item.HasPropertyOutdatedValue("Trucker");
                        break;
                    case "@truckerteltrue":
                        par.Value = item.HasPropertyOutdatedValue("TruckerTel");
                        break;
                    case "@deliverypricetrue":
                        par.Value = item.HasPropertyOutdatedValue("DeliveryPrice");
                        break;
                    case "@insurancepricetrue":
                        par.Value = item.HasPropertyOutdatedValue("InsurancePrice");
                        break;
                    case "@tdeliverypricetrue":
                        par.Value = item.HasPropertyOutdatedValue("TDeliveryPrice");
                        break;
                    case "@tinsurancepricetrue":
                        par.Value = item.HasPropertyOutdatedValue("TInsurancePrice");
                        break;
                    case "@parcelnumbertrue":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.ParcelNumber));
                        break;
                    case "@ratedate":
                        par.Value = item.RateDate;
                        break;
                    case "@ratedateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.RateDate));
                        break;
                    case "@gtlsrate":
                        par.Value = item.UsdRate;
                        break;
                    case "@gtlsrateupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.UsdRate));
                        break;
                    case "@transportdupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.TransportD));
                        break;
                    case "@transporttupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(item.TransportT));
                        break;
                }
            }
            foreach (SqlParameter par in myinsertupdateparams)
            {
                switch (par.ParameterName)
                {
                    case "@parcelnumber":
                        par.Value = item.ParcelNumber ?? string.Empty;
                        break;
                    case "@parceltype":
                        par.Value = item.ParcelType.Id;
                        break;
                    case "@parcelstatus":
                        par.Value = item.Status.Id;
                        break;
                    case "@declaration":
                        par.Value = item.Declaration;
                        break;
                    case "@goodstype":
                        par.Value = item.GoodsType?.Id;
                        break;
                    case "@shipmentnumber":
                        par.Value = item.ShipmentNumber;
                        break;
                    case "@shipplandate":
                        par.Value = item.ShipPlanDate;
                        break;
                    case "@shipdate":
                        par.Value = item.ShipDate;
                        break;
                    case "@preparation":
                        par.Value = item.Prepared;
                        break;
                    case "@borderdate":
                        par.Value = item.CrossedBorder;
                        break;
                    case "@terminalin":
                        par.Value = item.TerminalIn;
                        break;
                    case "@terminalout":
                        par.Value = item.TerminalOut;
                        break;
                    case "@unloaded":
                        par.Value = item.Unloaded;
                        break;
                    case "@carrier":
                        par.Value = item.Carrier;
                        break;
                    case "@carrierperson":
                        par.Value = item.CarrierPerson;
                        break;
                    case "@carriertel":
                        par.Value = item.CarrierTel;
                        break;
                    case "@lorry":
                        par.Value = item.Lorry;
                        break;
                    case "@lorryregnum":
                        par.Value = item.LorryRegNum;
                        break;
                    case "@lorryvin":
                        par.Value = item.LorryVIN;
                        break;
                    case "@lorryvolume":
                        par.Value = item.LorryVolume;
                        break;
                    case "@lorrytonnage":
                        par.Value = item.LorryTonnage;
                        break;
                    case "@trailerregnum":
                        par.Value = item.TrailerRegNum;
                        break;
                    case "@trailervin":
                        par.Value = item.TrailerVIN;
                        break;
                    case "@trucker":
                        par.Value = item.Trucker;
                        break;
                    case "@truckertel":
                        par.Value = item.TruckerTel;
                        break;
                    case "@deliveryprice":
                        par.Value = item.DeliveryPrice;
                        break;
                    case "@insuranceprice":
                        par.Value = item.InsurancePrice;
                        break;
                    case "@tdeliveryprice":
                        par.Value = item.TDeliveryPrice;
                        break;
                    case "@tinsuranceprice":
                        par.Value = item.TInsurancePrice;
                        break;
                    case "@transportd":
                        par.Value = item.TransportD;
                        break;
                    case "@transportt":
                        par.Value = item.TransportT;
                        break;
                }
            }
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        //protected override void CancelLoad()
        //{ myrdbm.CancelingLoad = this.CancelingLoad; }

        private void RequestsRefresh(Parcel parcel)
        {
            myrdbm.Errors.Clear();
            myrdbm.Parcel = parcel;
            myrdbm.FillType = this.FillType;
            //myrdbm.FillAsyncCompleted = () => { if (myrdbm.Errors.Count > 0) foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err); else foreach (Request ritem in myrdbm.Collection) if (!parcel.Requests.Contains(ritem)) parcel.Requests.Add(ritem); };
            myrdbm.Fill();
            if (myrdbm.Errors.Count > 0)
                foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
            else
            {
                //RequestCustomerLegalDBM ldbm = App.Current.Dispatcher.Invoke<RequestCustomerLegalDBM>(() => { return new RequestCustomerLegalDBM(); });
                //ldbm.FillType = lib.FillType.Refresh;
                foreach (Request ritem in myrdbm.Collection)
                {
                    //ritem.CustomerLegalsRefresh(ldbm);
                    //ldbm.Collection = null;
                    if (!parcel.Requests.Contains(ritem)) parcel.Requests.Add(ritem);
                }
            }
        }
        internal bool CheckGroup(Parcel parcel = null)
        {
            bool isSuccess = true;
            SqlCommand com = new SqlCommand();
            using (SqlConnection con = new SqlConnection(base.ConnectionString))
            {
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "ParcelGroupCheck_sp";
                com.Connection = con;
                SqlParameter parId = new SqlParameter();
                parId.ParameterName = "@parcelId";
                parId.SqlDbType = System.Data.SqlDbType.Int;
                com.Parameters.Add(parId);
                SqlParameter parRez = new SqlParameter();
                parRez.Direction = System.Data.ParameterDirection.Output;
                parRez.ParameterName = "@equals";
                parRez.SqlDbType = System.Data.SqlDbType.TinyInt;
                com.Parameters.Add(parRez);
                try
                {
                    con.Open();
                    if (parcel == null)
                        foreach (Parcel item in this.Collection)
                        {
                            if (this.SaveFilter(item))
                            {
                                parId.Value = item.Id;
                                com.ExecuteNonQuery();
                                if ((byte)parRez.Value != 0)
                                {
                                    this.Errors.Add(new DataModelClassLibrary.DBMError(item, "Не все группы заявок поставлены в загрузку " + item.ParcelNumberEntire + " полностью!", "group"));
                                    isSuccess = false;
                                }
                            }
                        }
                    else
                    {
                        parId.Value = parcel.Id;
                        com.ExecuteNonQuery();
                        if ((byte)parRez.Value != 0)
                        {
                            this.Errors.Add(new DataModelClassLibrary.DBMError(parcel, "Не все группы заявок поставлены в загрузку " + parcel.ParcelNumberEntire + " полностью!", "group"));
                            isSuccess = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    myexhandler.Handle(ex);
                    myerrors.Add(new DataModelClassLibrary.DBMError(null, myexhandler.Message, myexhandler.Code));
                }
                con.Close();
            }
            return isSuccess;
        }
    }

    public class ParcelVM : lib.ViewModelErrorNotifyItem<Parcel>
    {
        public ParcelVM(Parcel item) : base(item)
        {
            mylock = new object();
            ValidetingProperties.AddRange(new string[] { "ParcelType", "ParcelRequests", "ShipPlanDate", nameof(ParcelVM.DocDirPath) });
            DeleteRefreshProperties.AddRange(new string[] { "Carrier", "CarrierPerson", "CarrierTel", "CrossedBorder", "Declaration", "DocDirPath", "GoodsType", "Lorry", "LorryRegNum", "LorryTonnage", "LorryVIN", "LorryVolume", "ParcelNumber", "ParcelNumberEntire", "ParcelType", "Prepared", "RateDate", "ShipDate", "ShipPlanDate", "ShipmentNumber", "Status", "TerminalIn", "TerminalOut", "TrailerRegNum", "TrailerVIN", "Trucker", "TruckerTel", "Unloaded", "UsdRate" });
            InitProperties();
        }
        public ParcelVM() : this(new Parcel()) { }

        public string Carrier
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Carrier, value)))
                {
                    string name = "Carrier";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Carrier);
                    ChangingDomainProperty = name; this.DomainObject.Carrier = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Carrier : null; }
        }
        public string CarrierPerson
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CarrierPerson, value)))
                {
                    string name = "CarrierPerson";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CarrierPerson);
                    ChangingDomainProperty = name; this.DomainObject.CarrierPerson = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CarrierPerson : null; }
        }
        public string CarrierTel
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.CarrierTel, value)))
                {
                    string name = "CarrierTel";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CarrierTel);
                    ChangingDomainProperty = name; this.DomainObject.CarrierTel = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CarrierTel : null; }
        }
        public DateTime? CrossedBorder
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.CrossedBorder.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.CrossedBorder.Value, value.Value))))
                {
                    string name = "CrossedBorder";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.CrossedBorder);
                    ChangingDomainProperty = name; this.DomainObject.CrossedBorder = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.CrossedBorder : null; }
        }
        public string Declaration
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Declaration, value)))
                {
                    string name = "Declaration";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Declaration);
                    ChangingDomainProperty = name; this.DomainObject.Declaration = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Declaration : null; }
        }
        private string mydocdirpath;
        public string DocDirPath
        {
            set
            {
                SetPropertyValidate<string>(ref mydocdirpath, () => { this.DomainObject.DocDirPath = value; }, value);
                
                //if (!(this.IsReadOnly || string.Equals(this.DomainObject.DocDirPath, value)))
                //{
                //    string name = "DocDirPath";
                //    if (!myUnchangedPropertyCollection.ContainsKey(name))
                //        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DocDirPath);
                //    ChangingDomainProperty = name; this.DomainObject.DocDirPath = value;
                //}
            }
            get { return this.IsEnabled ? mydocdirpath : null; }
        }
        public lib.ReferenceSimpleItem GoodsType
        {
            set
            {
                if (!(this.IsReadOnly || object.Equals(this.DomainObject.GoodsType, value)))
                {
                    string name = "GoodsType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.GoodsType);
                    ChangingDomainProperty = name; this.DomainObject.GoodsType = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.GoodsType : null; }
        }
        public string Lorry
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Lorry, value)))
                {
                    string name = "Lorry";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Lorry);
                    ChangingDomainProperty = name; this.DomainObject.Lorry = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Lorry : null; }
        }
        public string LorryRegNum
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.LorryRegNum, value)))
                {
                    string name = "LorryRegNum";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.LorryRegNum);
                    ChangingDomainProperty = name; this.DomainObject.LorryRegNum = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.LorryRegNum : null; }
        }
        public decimal? LorryTonnage
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.LorryTonnage.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.LorryTonnage.Value, value.Value))))
                {
                    string name = "LorryTonnage";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.LorryTonnage);
                    ChangingDomainProperty = name; this.DomainObject.LorryTonnage = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.LorryTonnage : null; }
        }
        public object LorryTonnageForeground
        {
            get
            {
                object brush;
                if (this.IsEnabled & (this.LorryTonnage ?? 0M) - Total.ActualWeight < 0)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public string LorryVIN
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.LorryVIN, value)))
                {
                    string name = "LorryVIN";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.LorryVIN);
                    ChangingDomainProperty = name; this.DomainObject.LorryVIN = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.LorryVIN : null; }
        }
        public decimal? LorryVolume
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.LorryVolume.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.LorryVolume.Value, value.Value))))
                {
                    string name = "LorryVolume";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.LorryVolume);
                    ChangingDomainProperty = name; this.DomainObject.LorryVolume = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.LorryVolume : null; }
        }
        public object LorryVolumeForeground
        {
            get
            {
                object brush;
                if (this.IsEnabled & (this.LorryVolume ?? 0M) - Total.Volume < 0)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public string ParcelNumber
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.ParcelNumber, value)))
                {
                    string name = nameof(this.ParcelNumber);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ParcelNumber);
                    ChangingDomainProperty = name; this.DomainObject.ParcelNumber = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ParcelNumber : null; }
        }
        public string ParcelNumberEntire
        { get { return this.IsEnabled ? this.DomainObject.ParcelNumberEntire : null; } }
        public string ParcelNumberOrder
        {
            get { return this.DomainObject.ParcelNumberOrder; }
        }
        public lib.ReferenceSimpleItem ParcelType
        {
            set
            {
                if (value != null && !(this.IsReadOnly || object.Equals(this.DomainObject.ParcelType, value)))
                {
                    string name = "ParcelType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ParcelType);
                    ChangingDomainProperty = name; this.DomainObject.ParcelType = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ParcelType : null; }
        }
        public DateTime? Prepared
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Prepared.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.Prepared.Value, value.Value))))
                {
                    string name = "Prepared";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Prepared);
                    ChangingDomainProperty = name; this.DomainObject.Prepared = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Prepared : null; }
        }
        public DateTime? RateDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.RateDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.RateDate.Value, value.Value))))
                {
                    string name = nameof(this.RateDate);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.RateDate);
                    ChangingDomainProperty = name; this.DomainObject.RateDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.RateDate : null; }
        }
        public DateTime? ShipDate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.ShipDate.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.ShipDate.Value, value.Value))))
                {
                    string name = "ShipDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ShipDate);
                    ChangingDomainProperty = name; this.DomainObject.ShipDate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ShipDate : null; }
        }
        public DateTime? ShipPlanDate
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || DateTime.Equals(this.DomainObject.ShipPlanDate, value.Value)))
                {
                    string name = "ShipPlanDate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ShipPlanDate);
                    ChangingDomainProperty = name; this.DomainObject.ShipPlanDate = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ShipPlanDate : (DateTime?)null; }
        }
        public string ShipmentNumber
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.ShipmentNumber, value)))
                {
                    string name = "ShipmentNumber";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.ShipmentNumber);
                    ChangingDomainProperty = name; this.DomainObject.ShipmentNumber = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.ShipmentNumber : null; }
        }
        public lib.ReferenceSimpleItem Status
        {
            set
            {
                if (value != null && !(this.IsReadOnly || object.Equals(this.DomainObject.Status, value)))
                {
                    string name = "Status";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Status);
                    ChangingDomainProperty = name; this.DomainObject.Status = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Status : null; }
        }
        public DateTime? TerminalIn
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.TerminalIn.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.TerminalIn.Value, value.Value))))
                {
                    string name = "TerminalIn";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TerminalIn);
                    ChangingDomainProperty = name; this.DomainObject.TerminalIn = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TerminalIn : null; }
        }
        public DateTime? TerminalOut
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.TerminalOut.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.TerminalOut.Value, value.Value))))
                {
                    string name = "TerminalOut";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TerminalOut);
                    ChangingDomainProperty = name; this.DomainObject.TerminalOut = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TerminalOut : null; }
        }
        public string TrailerRegNum
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.TrailerRegNum, value)))
                {
                    string name = "TrailerRegNum";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TrailerRegNum);
                    ChangingDomainProperty = name; this.DomainObject.TrailerRegNum = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TrailerRegNum : null; }
        }
        public string TrailerVIN
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.TrailerVIN, value)))
                {
                    string name = "TrailerVIN";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TrailerVIN);
                    ChangingDomainProperty = name; this.DomainObject.TrailerVIN = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TrailerVIN : null; }
        }
        public string Trucker
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Trucker, value)))
                {
                    string name = "Trucker";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Trucker);
                    ChangingDomainProperty = name; this.DomainObject.Trucker = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Trucker : null; }
        }
        public string TruckerTel
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.TruckerTel, value)))
                {
                    string name = "TruckerTel";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TruckerTel);
                    ChangingDomainProperty = name; this.DomainObject.TruckerTel = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TruckerTel : null; }
        }
        public DateTime? Unloaded
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Unloaded.HasValue != value.HasValue || (value.HasValue && !DateTime.Equals(this.DomainObject.Unloaded.Value, value.Value))))
                {
                    string name = "Unloaded";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Unloaded);
                    ChangingDomainProperty = name; this.DomainObject.Unloaded = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Unloaded : null; }
        }
        public decimal? UsdRate
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.UsdRate.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.UsdRate.Value, value.Value))))
                {
                    string name = "UsdRate";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.UsdRate);
                    ChangingDomainProperty = name; this.DomainObject.UsdRate = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.UsdRate : null; }
        }

        public decimal? DeliveryPrice
        {
            get { return this.IsEnabled ? (this.DomainObject.DeliveryPrice ?? 0M) + (this.DomainObject.TDeliveryPrice ?? 0M) : (decimal?)null; }
        }
        public decimal? DDeliveryPrice
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.DeliveryPrice.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.DeliveryPrice.Value, value.Value))))
                {
                    string name = "DeliveryPrice";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DeliveryPrice);
                    ChangingDomainProperty = name; this.DomainObject.DeliveryPrice = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DeliveryPrice : null; }
        }
        public decimal? TDeliveryPrice
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.TDeliveryPrice.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.TDeliveryPrice.Value, value.Value))))
                {
                    string name = "TDeliveryPrice";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TDeliveryPrice);
                    ChangingDomainProperty = name; this.DomainObject.TDeliveryPrice = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TDeliveryPrice : null; }
        }
        public decimal? InsurancePrice
        {
            get { return this.IsEnabled ? (this.DomainObject.InsurancePrice ?? 0M) + (this.DomainObject.TInsurancePrice ?? 0M) : (decimal?)null; }
        }
        public decimal? DInsurancePrice
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.InsurancePrice.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.InsurancePrice.Value, value.Value))))
                {
                    string name = "InsurancePrice";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.InsurancePrice);
                    ChangingDomainProperty = name; this.DomainObject.InsurancePrice = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.InsurancePrice : null; }
        }
        public decimal? TInsurancePrice
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.TInsurancePrice.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.TInsurancePrice.Value, value.Value))))
                {
                    string name = "TInsurancePrice";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TInsurancePrice);
                    ChangingDomainProperty = name; this.DomainObject.TInsurancePrice = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TInsurancePrice : null; }
        }
        public decimal? Transport
        {
            get { return this.IsEnabled ? (this.DomainObject.TransportD ?? 0M) + (this.DomainObject.TransportT ?? 0M) : (decimal?)null; }
        }
        public decimal? TransportD
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.TransportD.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.TransportD.Value, value.Value))))
                {
                    string name = nameof(ParcelVM.TransportD);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TransportD);
                    ChangingDomainProperty = name; this.DomainObject.TransportD = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TransportD : null; }
        }
        public decimal? TransportT
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.TransportT.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.TransportT.Value, value.Value))))
                {
                    string name = nameof(Parcel.TransportT);
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.TransportT);
                    ChangingDomainProperty = name; this.DomainObject.TransportT = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.TransportT : null; }
        }

        public string ShipPlanDateMailImage
        {
            get
            {
                string path;
                switch (this.DomainObject.MailState.ShipDate)
                {
                    case 1:
                        path = "/CustomBrokerWpf;component/Images/mail_1.png";
                        break;
                    case 2:
                        path = "/CustomBrokerWpf;component/Images/mail_3.png";
                        break;
                    default:
                        path = "/CustomBrokerWpf;component/Images/mail_2.png";
                        break;
                }
                return path;
            }
        }
        public string PreparedMailImage
        {
            get
            {
                string path;
                switch (this.DomainObject.MailState.Prepared)
                {
                    case 1:
                        path = "/CustomBrokerWpf;component/Images/mail_1.png";
                        break;
                    case 2:
                        path = "/CustomBrokerWpf;component/Images/mail_3.png";
                        break;
                    default:
                        path = "/CustomBrokerWpf;component/Images/mail_2.png";
                        break;
                }
                return path;
            }
        }
        public string CrossedBorderMailImage
        {
            get
            {
                string path;
                switch (this.DomainObject.MailState.CrossedBorder)
                {
                    case 1:
                        path = "/CustomBrokerWpf;component/Images/mail_1.png";
                        break;
                    case 2:
                        path = "/CustomBrokerWpf;component/Images/mail_3.png";
                        break;
                    default:
                        path = "/CustomBrokerWpf;component/Images/mail_2.png";
                        break;
                }
                return path;
            }
        }
        public string TerminalInMailImage
        {
            get
            {
                string path;
                switch (this.DomainObject.MailState.TerminalIn)
                {
                    case 1:
                        path = "/CustomBrokerWpf;component/Images/mail_1.png";
                        break;
                    case 2:
                        path = "/CustomBrokerWpf;component/Images/mail_3.png";
                        break;
                    default:
                        path = "/CustomBrokerWpf;component/Images/mail_2.png";
                        break;
                }
                return path;
            }
        }
        public string TerminalOutMailImage
        {
            get
            {
                string path;
                switch (this.DomainObject.MailState.TerminalOut)
                {
                    case 1:
                        path = "/CustomBrokerWpf;component/Images/mail_1.png";
                        break;
                    case 2:
                        path = "/CustomBrokerWpf;component/Images/mail_3.png";
                        break;
                    default:
                        path = "/CustomBrokerWpf;component/Images/mail_2.png";
                        break;
                }
                return path;
            }
        }
        public string UnloadedMailImage
        {
            get
            {
                string path;
                switch (this.DomainObject.MailState.UnLoaded)
                {
                    case 1:
                        path = "/CustomBrokerWpf;component/Images/mail_1.png";
                        break;
                    case 2:
                        path = "/CustomBrokerWpf;component/Images/mail_3.png";
                        break;
                    default:
                        path = "/CustomBrokerWpf;component/Images/mail_2.png";
                        break;
                }
                return path;
            }
        }

        //private ImporterParcelRequestTotalVM myrequesttotal;
        //public ImporterParcelRequestTotalVM RequestTotal
        //{
        //    get
        //    {
        //        if (myrequesttotal == null)
        //            myrequesttotal = new ImporterParcelRequestTotalVM(this.DomainObject.RequestTotal, this);
        //        return this.IsEnabled ? myrequesttotal : null;
        //    }
        //}
        //private ImporterParcelRequestTotalVM myrequesttotald;
        //public ImporterParcelRequestTotalVM RequestTotalD
        //{
        //    get
        //    {
        //        if (myrequesttotald == null)
        //            myrequesttotald = new ImporterParcelRequestTotalVM(this.DomainObject.RequestTotalD, this);
        //        return this.IsEnabled ? myrequesttotald : null;
        //    }
        //}
        //private ImporterParcelRequestTotalVM myrequesttotalt;
        //public ImporterParcelRequestTotalVM RequestTotalT
        //{
        //    get
        //    {
        //        if (myrequesttotalt == null)
        //            myrequesttotalt = new ImporterParcelRequestTotalVM(this.DomainObject.RequestTotalT, this);
        //        return this.IsEnabled ? myrequesttotalt : null;
        //    }
        //}

        #region Free
        private decimal myactualweightfreepre;
        public decimal? ActualWeightFree
        {
            set { myactualweightfreepre += value ?? 0M; PropertyChangedNotification("ActualWeightFree"); PropertyChangedNotification("DifferenceWeightFree"); }
            get { return this.IsEnabled ? this.DomainObject.ActualWeightFree + myactualweightfreepre : (decimal?)null; }
        }
        private decimal mycellnumberfreepre;
        public decimal? CellNumberFree
        {
            set { mycellnumberfreepre += value ?? 0M; PropertyChangedNotification("CellNumberFree"); }
            get { return this.IsEnabled ? this.DomainObject.CellNumberFree + mycellnumberfreepre : (decimal?)null; }
        }
        public decimal? DifferenceWeightFree
        { get { return this.IsEnabled ? this.DomainObject.DifferenceWeightFree + myactualweightfreepre - myofficialweightfreepre : (decimal?)null; } }
        private decimal myinvoicefreepre;
        public decimal? InvoiceFree
        {
            set { myinvoicefreepre += value ?? 0M; PropertyChangedNotification("InvoiceFree"); }
            get { return this.IsEnabled ? this.DomainObject.InvoiceFree + myinvoicefreepre : (decimal?)null; }
        }
        private decimal myinvoicediscountfreepre;
        public decimal? InvoiceDiscountFree
        {
            set { myinvoicediscountfreepre += value ?? 0M; PropertyChangedNotification("InvoiceDiscountFree"); }
            get { return this.IsEnabled ? this.DomainObject.InvoiceDiscountFree + myinvoicediscountfreepre : (decimal?)null; }
        }
        private decimal myofficialweightfreepre;
        public decimal? OfficialWeightFree
        {
            set { myofficialweightfreepre += value ?? 0M; PropertyChangedNotification("OfficialWeightFree"); PropertyChangedNotification("DifferenceWeightFree"); }
            get { return this.IsEnabled ? this.DomainObject.OfficialWeightFree + myofficialweightfreepre : (decimal?)null; }
        }
        private decimal myvolumefreepre;
        public decimal? VolumeFree
        {
            set { myvolumefreepre += value ?? 0M; PropertyChangedNotification("VolumeFree"); }
            get { return this.IsEnabled ? this.DomainObject.VolumeFree + myvolumefreepre : (decimal?)null; }
        }
        internal void ResetFree()
        {
            myactualweightfreepre = 0M;
            mycellnumberfreepre = 0M;
            myinvoicefreepre = 0M;
            myinvoicediscountfreepre = 0M;
            myofficialweightfreepre = 0M;
            myvolumefreepre = 0M;
            PropertyChangedNotification("ActualWeightFree");
            PropertyChangedNotification("CellNumberFree");
            PropertyChangedNotification("DifferenceWeightFree");
            PropertyChangedNotification("InvoiceFree");
            PropertyChangedNotification("InvoiceDiscountFree");
            PropertyChangedNotification("OfficialWeightFree");
            PropertyChangedNotification("VolumeFree");
        }
        #endregion

        private ParcelRequestsVMTotal myparcelrequeststotal;
        public ParcelRequestsVMTotal ParcelRequestsTotal
        { get { return myparcelrequeststotal; } }
        private ParcelRequestsVMTotal myparcelrequeststotald;
        public ParcelRequestsVMTotal ParcelRequestsTotalDelivery
        { get { return myparcelrequeststotald; } }
        private ParcelRequestsVMTotal myparcelrequeststotalt;
        public ParcelRequestsVMTotal ParcelRequestsTotalTrade
        { get { return myparcelrequeststotalt; } }

        private ParcelRequestsVMTotal myparcelrequeststotalselected;
        public ParcelRequestsVMTotal ParcelRequestsTotalSelected
        { get { return myparcelrequeststotalselected; } }

        private ParcelRequestsVMTotal myrequeststotal;
        public ParcelRequestsVMTotal RequestsTotal
        { get { return myrequeststotal; } }
        private ParcelRequestsVMTotal myrequeststotalselected;
        public ParcelRequestsVMTotal RequestsTotalSelected
        { get { return myrequeststotalselected; } }
        private ParcelRequestsVMTotal myrequeststotalselectedd;
        public ParcelRequestsVMTotal RequestsTotalSelectedDelivery
        { get { return myrequeststotalselectedd; } }
        private ParcelRequestsVMTotal myrequeststotalselectedt;
        public ParcelRequestsVMTotal RequestsTotalSelectedTrade
        { get { return myrequeststotalselectedt; } }

        private ParcelTotal mytotal;
        public ParcelTotal Total
        {
            get
            {
                if (mytotal == null)
                {
                    mytotal = new ParcelTotal(myparcelrequeststotal, myrequeststotalselected);
                    mytotal.PropertyChanged += Total_PropertyChanged;
                    this.PropertyChangedNotification(nameof(this.Total));
                    ForegroundNotifyChanged();
                }
                return this.IsEnabled ? mytotal : null;
            }
        }
        private ParcelTotal mytotaldelivery;
        public ParcelTotal TotalDelivery
        {
            get
            {
                if (mytotaldelivery == null)
                {
                    mytotaldelivery = new ParcelTotal(myparcelrequeststotald, myrequeststotalselectedd);
                    this.PropertyChangedNotification(nameof(this.TotalDelivery));
                }
                return this.IsEnabled ? mytotaldelivery : null;
            }
        }
        private ParcelTotal mytotaltrade;
        public ParcelTotal TotalTrade
        {
            get
            {
                if (mytotaltrade == null)
                    mytotaltrade = new ParcelTotal(myparcelrequeststotalt, myrequeststotalselectedt);
                return this.IsEnabled ? mytotaltrade : null;
            }
        }

        public Brush ActualWeightForeground
        {
            get
            {
                Brush brush;
                if (this.IsEnabled & (TotalDelivery.ActualWeight + TotalTrade.ActualWeight != Total.ActualWeight))
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public Brush OfficialWeightForeground
        {
            get
            {
                Brush brush;
                if (this.IsEnabled & TotalDelivery.OfficialWeight + TotalTrade.OfficialWeight != Total.OfficialWeight)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public Brush CellNumberForeground
        {
            get
            {
                Brush brush;
                if (this.IsEnabled & TotalDelivery.CellNumber + TotalTrade.CellNumber != Total.CellNumber)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public Brush InvoiceForeground
        {
            get
            {
                Brush brush;
                if (this.IsEnabled & TotalDelivery.Invoice + TotalTrade.Invoice != Total.Invoice)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public Brush InvoiceDiscountForeground
        {
            get
            {
                Brush brush;
                if (this.IsEnabled & TotalDelivery.InvoiceDiscount + TotalTrade.InvoiceDiscount != Total.InvoiceDiscount)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public Brush VolumeForeground
        {
            get
            {
                Brush brush;
                if (this.IsEnabled & (TotalDelivery.Volume + TotalTrade.Volume != Total.Volume))
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        private void ForegroundNotifyChanged()
        {
            PropertyChangedNotification("ActualWeightForeground");
            PropertyChangedNotification("OfficialWeightForeground");
            PropertyChangedNotification("CellNumberForeground");
            PropertyChangedNotification("InvoiceForeground");
            PropertyChangedNotification("InvoiceDiscountForeground");
            PropertyChangedNotification("VolumeForeground");
        }
        private void Total_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ActualWeight":
                    PropertyChangedNotification("OverWeight");
                    PropertyChangedNotification("ActualWeightForeground");
                    break;
                case "OfficialWeight":
                    PropertyChangedNotification("OfficialWeightForeground");
                    break;
                case "CellNumber":
                    PropertyChangedNotification("CellNumberForeground");
                    break;
                case "Invoice":
                    PropertyChangedNotification("InvoiceForeground");
                    break;
                case "InvoiceDiscount":
                    PropertyChangedNotification("InvoiceDiscountForeground");
                    break;
                case "Volume":
                    PropertyChangedNotification("OverVolume");
                    PropertyChangedNotification("VolumeForeground");
                    break;
            }
        }

        private RequestSynchronizer myrsync;
        private ListCollectionView myrequests;
        public ListCollectionView Requests
        {
            get
            {
                if (myrsync == null)
                {
                    myrsync = new RequestSynchronizer();
                    myrsync.DomainCollection = this.DomainObject.Requests;
                }
                if (myrequests == null)
                {
                    myrequests = new ListCollectionView(myrsync.ViewModelCollection);
                    myrequests.Filter = (object item) => { return this.Status?.Id < 60 && (item as RequestVM).DomainObject.Parcel == null && lib.ViewModelViewCommand.ViewFilterDefault(item); };
                    myrequests.SortDescriptions.Add(new SortDescription("CustomerName", ListSortDirection.Ascending));
                    myrequests.SortDescriptions.Add(new SortDescription("ParcelGroup", ListSortDirection.Ascending));
                    myrequests.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                    myrequests.MoveCurrentToPosition(-1);

                    //myrequeststotal = new ParcelRequestsTotal(myrequests, -10000);
                    //myrequeststotal.FilteringProperties.Add("Parcel");
                    //myrequeststotal.StartCount();
                    //this.PropertyChangedNotification(nameof(this.RequestsTotal));

                    myrequeststotalselected = new ParcelRequestsVMTotal(myrequests, 2);
                    myrequeststotalselected.FilteringProperties.Add("Parcel");
                    myrequeststotalselected.StartCount();
                    this.PropertyChangedNotification(nameof(this.ParcelRequestsTotalSelected));

                    myrequeststotalselectedd = new ParcelRequestsVMTotal(myrequests, 2, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2));
                    myrequeststotalselectedd.FilteringProperties.Add("Parcel");
                    myrequeststotalselectedd.StartCount();
                    this.PropertyChangedNotification(nameof(this.RequestsTotalSelectedDelivery));

                    myrequeststotalselectedt = new ParcelRequestsVMTotal(myrequests, 2, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1));
                    myrequeststotalselectedt.FilteringProperties.Add("Parcel");
                    myrequeststotalselectedt.StartCount();
                    this.PropertyChangedNotification(nameof(this.RequestsTotalSelectedTrade));

                    this.Total.Selected = myrequeststotalselected;
                    this.TotalDelivery.Selected = myrequeststotalselectedd;
                    this.TotalTrade.Selected = myrequeststotalselectedt;
                }
                return myrequests;
            }
        }
        private ListCollectionView myparcelrequests;
        public ListCollectionView ParcelRequests
        {
            get
            {
                if (myrsync == null)
                {
                    myrsync = new RequestSynchronizer();
                    myrsync.DomainCollection = this.DomainObject.Requests;
                }
                if (myparcelrequests == null)
                {
                    myparcelrequests = new ListCollectionView(myrsync.ViewModelCollection);
                    myparcelrequests.Filter = (object item) => { return (item as RequestVM).DomainObject.Parcel == this.DomainObject && lib.ViewModelViewCommand.ViewFilterDefault(item); };
                    myparcelrequests.SortDescriptions.Add(new SortDescription("CustomerName", ListSortDirection.Ascending));
                    myparcelrequests.SortDescriptions.Add(new SortDescription("ParcelGroup", ListSortDirection.Ascending));
                    myparcelrequests.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                    myparcelrequests.MoveCurrentToPosition(-1);

                    myparcelrequeststotal = new ParcelRequestsVMTotal(myparcelrequests, -10000);
                    myparcelrequeststotal.FilteringProperties.Add("Parcel");
                    myparcelrequeststotal.StartCount();
                    this.PropertyChangedNotification(nameof(this.ParcelRequestsTotal));

                    myparcelrequeststotald = new ParcelRequestsVMTotal(myparcelrequests, -10000, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2));
                    myparcelrequeststotald.FilteringProperties.Add("Parcel");
                    myparcelrequeststotald.StartCount();
                    this.PropertyChangedNotification(nameof(this.ParcelRequestsTotalDelivery));

                    myparcelrequeststotalt = new ParcelRequestsVMTotal(myparcelrequests, -10000, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1));
                    myparcelrequeststotalt.FilteringProperties.Add("Parcel");
                    myparcelrequeststotalt.StartCount();
                    this.PropertyChangedNotification(nameof(this.ParcelRequestsTotalTrade));

                    myparcelrequeststotalselected = new ParcelRequestsVMTotal(myparcelrequests);
                    myparcelrequeststotalselected.FilteringProperties.Add("Parcel");
                    myparcelrequeststotalselected.StartCount();
                    this.PropertyChangedNotification(nameof(this.ParcelRequestsTotalSelected));

                    this.Total.Total = myparcelrequeststotal;
                    this.TotalDelivery.Total = myparcelrequeststotald;
                    this.TotalTrade.Total = myparcelrequeststotalt;
                }
                return myparcelrequests;
            }
        }

        private object mylock;
        private Specification.SpecificationSynchronizer myssync;
        private ListCollectionView myspecifications;
        public ListCollectionView Specifications
        {
            get
            {
                if (myspecifications == null)
                {
                    lock (mylock)
                    {
                        if (myssync == null)
                        {
                            myssync = new Specification.SpecificationSynchronizer();
                            myssync.DomainCollection = this.DomainObject.Specifications;
                        }
                        if (myspecifications == null)
                        {
                            myspecifications = new ListCollectionView(myssync.ViewModelCollection);
                            myspecifications.Filter = (object item) => { return lib.ViewModelViewCommand.ViewFilterDefault(item); };
                            myspecifications.SortDescriptions.Add(new SortDescription("Request.StorePointDate", ListSortDirection.Ascending));
                            myspecifications.SortDescriptions.Add(new SortDescription("ParcelGroup", ListSortDirection.Ascending));
                            myspecifications.SortDescriptions.Add(new SortDescription("Consolidate", ListSortDirection.Ascending));
                            myspecifications.MoveCurrentToPosition(-1);
                        }
                    }
                }
                return myspecifications;
            }
        }

        public System.Windows.Visibility RequestAreaVisibility
        { get { return this.Status.Id == 50 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; } }
        public System.Windows.Visibility SpecificationAreaVisibility
        { get { return this.Status.Id == 50 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case nameof(Parcel.DocDirPath):
                    mydocdirpath=this.DomainObject.DocDirPath;
                    break;
                case "Status":
                    this.Requests.Refresh();
                    PropertyChangedNotification("RequestAreaVisibility");
                    PropertyChangedNotification("SpecificationAreaVisibility");
                    break;
                case "OverVolume":
                    PropertyChangedNotification("LorryVolumeForeground");
                    break;
                case "OverWeight":
                    PropertyChangedNotification("LorryTonnageForeground");
                    break;
                case "DeliveryPrice":
                    PropertyChangedNotification("DDeliveryPrice");
                    break;
                case "TDeliveryPrice":
                    PropertyChangedNotification("DeliveryPrice");
                    break;
                case "InsurancePrice":
                    PropertyChangedNotification("DInsurancePrice");
                    break;
                case "TInsurancePrice":
                    PropertyChangedNotification("InsurancePrice");
                    break;
                case "MailStateShipDate":
                    PropertyChangedNotification("ShipPlanDateMailImage");
                    break;
                case "MailStatePrepared":
                    PropertyChangedNotification("PreparedMailImage");
                    break;
                case "MailStateCrossedBorder":
                    PropertyChangedNotification("CrossedBorderMailImage");
                    break;
                case "MailStateTerminalIn":
                    PropertyChangedNotification("TerminalInMailImage");
                    break;
                case "MailStateTerminalOut":
                    PropertyChangedNotification("TerminalOutMailImage");
                    break;
                case "MailStateUnLoaded":
                    PropertyChangedNotification("UnloadedMailImage");
                    break;
                case nameof(Parcel.Requests):
                    PropertyChangedNotification(nameof(this.ParcelRequests));
                    break;
                case nameof(Parcel.TransportD):
                case nameof(Parcel.TransportT):
                    PropertyChangedNotification(nameof(this.Transport));
                    break;
            }
        }
        protected override void InitProperties()
        {
            mydocdirpath = this.DomainObject.DocDirPath;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Carrier":
                    this.DomainObject.Carrier = (string)value;
                    break;
                case "CarrierPerson":
                    this.DomainObject.CarrierPerson = (string)value;
                    break;
                case "CarrierTel":
                    this.DomainObject.CarrierTel = (string)value;
                    break;
                case "CrossedBorder":
                    this.DomainObject.CrossedBorder = (DateTime?)value;
                    break;
                case "Declaration":
                    this.DomainObject.Declaration = (string)value;
                    break;
                case "DocDirPath":
                    if (mydocdirpath != this.DomainObject.DocDirPath)
                        mydocdirpath = this.DomainObject.DocDirPath;
                    else
                        mydocdirpath = (string)value;
                    break;
                case "GoodsType":
                    this.DomainObject.GoodsType = (lib.ReferenceSimpleItem)value;
                    break;
                case "Lorry":
                    this.DomainObject.Lorry = (string)value;
                    break;
                case "LorryRegNum":
                    this.DomainObject.LorryRegNum = (string)value;
                    break;
                case "LorryTonnage":
                    this.DomainObject.LorryTonnage = (decimal?)value;
                    break;
                case "LorryVIN":
                    this.DomainObject.LorryVIN = (string)value;
                    break;
                case "LorryVolume":
                    this.DomainObject.LorryVolume = (decimal?)value;
                    break;
                case "ParcelType":
                    this.DomainObject.ParcelType = (lib.ReferenceSimpleItem)value;
                    break;
                case "Prepared":
                    this.DomainObject.Prepared = (DateTime?)value;
                    break;
                case "RateDate":
                    this.DomainObject.RateDate = (DateTime?)value;
                    break;
                case "ShipDate":
                    this.DomainObject.ShipDate = (DateTime?)value;
                    break;
                case "ShipPlanDate":
                    this.DomainObject.ShipPlanDate = (DateTime)value;
                    break;
                case "ShipmentNumber":
                    this.DomainObject.ShipmentNumber = (string)value;
                    break;
                case "Status":
                    this.DomainObject.Status = (lib.ReferenceSimpleItem)value;
                    break;
                case "TerminalIn":
                    this.DomainObject.TerminalIn = (DateTime?)value;
                    break;
                case "TerminalOut":
                    this.DomainObject.TerminalOut = (DateTime?)value;
                    break;
                case "TrailerRegNum":
                    this.DomainObject.TrailerRegNum = (string)value;
                    break;
                case "TrailerVIN":
                    this.DomainObject.TrailerVIN = (string)value;
                    break;
                case "Trucker":
                    this.DomainObject.Trucker = (string)value;
                    break;
                case "TruckerTel":
                    this.DomainObject.TruckerTel = (string)value;
                    break;
                case "Unloaded":
                    this.DomainObject.Unloaded = (DateTime?)value;
                    break;
                case "UsdRate":
                    this.DomainObject.UsdRate = (decimal?)value;
                    break;
                case "DependentNew":
                    int i = 0;
                    if (myrsync != null)
                    {
                        RequestVM[] removed = new RequestVM[this.DomainObject.Requests.Count];
                        foreach (RequestVM item in myrsync.ViewModelCollection)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                removed[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach (RequestVM item in removed)
                            if (item != null) myrsync.ViewModelCollection.Remove(item);
                        this.Requests.Refresh();
                        this.ParcelRequests.Refresh();
                    }
                    if (myssync != null)
                    {
                        Specification.SpecificationVM[] specremoved = new Specification.SpecificationVM[this.DomainObject.Specifications.Count];
                        foreach (Specification.SpecificationVM item in myssync.ViewModelCollection)
                        {
                            if (item.DomainState == lib.DomainObjectState.Added)
                            {
                                specremoved[i] = item;
                                i++;
                            }
                            else
                                item.RejectChanges();
                        }
                        foreach (Specification.SpecificationVM item in specremoved)
                            if (item != null) myssync.ViewModelCollection.Remove(item);
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
                case nameof(ParcelVM.DocDirPath):
                    isvalid = this.DomainObject.ValidateProperty(propertyname, mydocdirpath, out errmsg,out _);
                    break;
                case "ParcelType":
                    if (this.ParcelType == null)
                    {
                        errmsg = "Необходимо указать тип перевозки!";
                        isvalid = false;
                    }
                    break;
                case "ParcelRequests":
                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                    if (myrsync != null && this.Status?.Id == 50)
                    {
                        foreach (RequestVM ritem in myrsync.ViewModelCollection)
                            if (ritem.Parcel != null && !ritem.Validate(inform))
                            {
                                isvalid = false;
                                errs.AppendLine(ritem.Errors);
                            }
                        errmsg = errs.ToString();
                    }
                    break;
                case "ShipPlanDate":
                    if (!this.ShipPlanDate.HasValue)
                    {
                        errmsg = "Необходимо указать плановую дату отгрузки!";
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return mydocdirpath != this.DomainObject.DocDirPath;
        }
    }

    public class ParcelSynchronizer : lib.ModelViewCollectionsSynchronizer<Parcel, ParcelVM>
    {
        protected override Parcel UnWrap(ParcelVM wrap)
        {
            return wrap.DomainObject as Parcel;
        }
        protected override ParcelVM Wrap(Parcel fill)
        {
            return new ParcelVM(fill);
        }
    }

    internal class ParcelCommands
    {
        internal Action<string, bool> OpenPopup { set; get; }
        internal Func<bool> EndEdit { set; get; }
        internal Func<bool> SaveDataChanges { set; get; }

        internal void FolderOpenExec(ParcelVM parcel)
        {
            if (parcel != null)
            {
                string path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + parcel.ParcelNumber ?? string.Empty; // chahge ParcelNumber to DocDirPath
                if (!Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                System.Diagnostics.Process.Start(path);
            }
        }
        internal void SetStoreInformExec(ParcelVM parcel)
        {
            parcel.ParcelRequests.CommitEdit();
            foreach (RequestVM item in parcel.ParcelRequests)
            {
                if (!item.StoreInform.HasValue)
                {
                    item.StoreInform = DateTime.Today;
                }
            }
            parcel.ParcelRequests.CommitEdit();
        }
        internal void MoveSpecificationExec(ParcelVM parcel)
        {
            if (this.EndEdit() && this.SaveDataChanges() && parcel != null && parcel.ParcelType.Id == 1)
            {
                FileInfo[] files;
                DirectoryInfo dirIn = new DirectoryInfo(@"V:\Отправки");
                if (dirIn.Exists)
                {
                    if (dirIn.GetDirectories(parcel.ParcelNumber + "_*").Length > 0)
                    {
                        dirIn = dirIn.GetDirectories(parcel.ParcelNumber + "_*")[0];
                        DirectoryInfo dirOut = new DirectoryInfo(@"V:\Спецификации");
                        if (dirOut.Exists)
                        {
                            foreach (Classes.Domain.RequestVM row in parcel.ParcelRequests)
                            {
                                if (!row.DomainObject.ParcelId.HasValue) continue;
                                files = dirOut.GetFiles("*" + row.StorePoint + "*");
                                if (files.Length > 0)
                                {
                                    try
                                    {
                                        if (File.Exists(dirIn.FullName + "\\" + files[0].Name))
                                            File.Delete(dirIn.FullName + "\\" + files[0].Name);
                                        files[0].MoveTo(dirIn.FullName + "\\" + files[0].Name);
                                    }
                                    catch (Exception ex)
                                    {
                                        this.OpenPopup("Ошибка доступа к файлу/n" + ex.Message, true);
                                    }
                                }
                                if (dirIn.GetFiles("*" + row.StorePoint + "*").Length > 0)
                                {
                                    row.IsSpecification = true;
                                }
                            }
                        }
                        else
                            this.OpenPopup("Перенос спецификаций\n" + @"Папка 'V:\Спецификации' не найдена!", true);
                    }
                    else
                        this.OpenPopup("Перенос спецификаций\n" + @"Папка 'V:\Отправки\" + parcel.ParcelNumber + "_...' не найдена!", true);
                }
                else
                    this.OpenPopup("Перенос спецификаций\n" + @"Папка 'V:\Отправки' не найдена!", true);
            }
        }
        internal void CreateExcelReportExec(ParcelVM parcel, object parametr)
        {
            bool isNew;
            if (parcel != null && parametr is bool)
            {
                isNew = (bool)parametr;
                ExcelReport(parcel, null, isNew);
                ExcelReport(parcel, 1, isNew);
                ExcelReport(parcel, 2, isNew);
            }
        }
        private void ExcelReport(ParcelVM parcel, int? importerid, bool isNew)
        {
            excel.Application exApp = new excel.Application();
            excel.Application exAppProt = new excel.Application();
            excel.Workbook exWb;
            try
            {
                int i = 2;
                exApp.SheetsInNewWorkbook = 1;
                exWb = exApp.Workbooks.Add(Type.Missing);
                excel.Worksheet exWh = exWb.Sheets[1];
                excel.Range r;
                exWh.Name = parcel.ParcelNumberEntire;
                exWh.Cells[1, 1] = "Позиция по складу"; exWh.Cells[1, 2] = "Дата поступления"; exWh.Cells[1, 3] = "Группа загрузки"; exWh.Cells[1, 4] = "Клиент"; exWh.Cells[1, 5] = "Юр. лица"; exWh.Cells[1, 6] = "Поставщик"; exWh.Cells[1, 7] = "Импортер"; exWh.Cells[1, 8] = "Группа менеджеров";
                exWh.Cells[1, 9] = "Кол-во мест"; exWh.Cells[1, 10] = "Вес по док, кг"; exWh.Cells[1, 11] = "Вес факт, кг"; exWh.Cells[1, 12] = "Объем, м3"; exWh.Cells[1, 13] = "Инвойс"; exWh.Cells[1, 14] = "Инвойс, cо скидкой"; exWh.Cells[1, 15] = "Услуга"; exWh.Cells[1, 16] = "Примечание менеджера";
                r = exWh.Columns[9, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[10, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[11, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[12, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[13, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[14, Type.Missing]; r.NumberFormat = "#,##0.00";
                foreach (Classes.Domain.RequestVM itemRow in parcel.ParcelRequests)
                {
                    if (importerid != itemRow.Importer?.Id || (isNew && itemRow.StoreInform.HasValue)) continue;
                    if (!string.IsNullOrEmpty(itemRow.StorePoint)) exWh.Cells[i, 1] = itemRow.StorePoint;
                    if (itemRow.StoreDate.HasValue) exWh.Cells[i, 2] = itemRow.StoreDate;
                    if (itemRow.ParcelGroup.HasValue) exWh.Cells[i, 3] = itemRow.ParcelGroup;
                    if (!string.IsNullOrEmpty(itemRow.CustomerName)) exWh.Cells[i, 4] = itemRow.CustomerName;
                    if (!string.IsNullOrEmpty(itemRow.CustomerLegalsNames)) exWh.Cells[i, 5] = itemRow.CustomerLegalsNames;
                    if (!string.IsNullOrEmpty(itemRow.AgentName)) exWh.Cells[i, 6] = itemRow.AgentName;
                    if (!string.IsNullOrEmpty(itemRow.Importer?.Name)) exWh.Cells[i, 7] = itemRow.Importer.Name;
                    if (!string.IsNullOrEmpty(itemRow.ManagerGroupName)) exWh.Cells[i, 8] = itemRow.ManagerGroupName;
                    if (itemRow.CellNumber.HasValue) exWh.Cells[i, 9] = itemRow.CellNumber.Value;
                    if (itemRow.OfficialWeight.HasValue) exWh.Cells[i, 10] = itemRow.OfficialWeight.Value;
                    if (itemRow.ActualWeight.HasValue) exWh.Cells[i, 11] = itemRow.ActualWeight.Value;
                    if (itemRow.Volume.HasValue) exWh.Cells[i, 12] = itemRow.Volume.Value;
                    if (itemRow.Invoice.HasValue) exWh.Cells[i, 13] = itemRow.Invoice.Value;
                    if (itemRow.InvoiceDiscount.HasValue) exWh.Cells[i, 14] = itemRow.InvoiceDiscount.Value;
                    if (!string.IsNullOrEmpty(itemRow.ServiceType)) exWh.Cells[i, 15] = itemRow.ServiceType;
                    if (!string.IsNullOrEmpty(itemRow.ManagerNote)) exWh.Cells[i, 16] = itemRow.ManagerNote;
                    itemRow.StoreInform = DateTime.Now;
                    i++;
                }
                if (i > 2)
                {
                    string filename = Path.Combine(CustomBrokerWpf.Properties.Settings.Default.DocFileRoot, parcel.DocDirPath, parcel.Lorry + " - " + (importerid == 1 ? "Трейд" : (importerid == 2 ? "Деливери" : string.Empty)) + ".xlsx");
                    if (File.Exists(filename))
                        File.Delete(filename);
                    else if (!Directory.Exists(Path.Combine(CustomBrokerWpf.Properties.Settings.Default.DocFileRoot, parcel.DocDirPath)))
                        Directory.CreateDirectory(Path.Combine(CustomBrokerWpf.Properties.Settings.Default.DocFileRoot, parcel.DocDirPath));
                    exWb.SaveAs(Filename: filename);
                    exApp.Visible = true;
                }
                else
                {
                    exWb.Close(false);
                    this.OpenPopup("Нет новых заявок", false);
                }
                exWh = null;
            }
            catch (Exception ex)
            {
                if (exApp != null)
                {
                    foreach (excel.Workbook itemBook in exApp.Workbooks)
                    {
                        itemBook.Close(false);
                    }
                    exApp.Quit();
                }
                this.OpenPopup("Создание заявки " + ex.Message, true);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }

        internal void RequestExcelExec(ParcelVM parcel)
        {
            if (parcel == null) return;
            if (myrequestexceltask == null)
                myrequestexceltask = new lib.TaskAsync.TaskAsync();

            if (!myrequestexceltask.IsBusy)
            {
                this.EndEdit();
                myrequestexceltask.DoProcessing = RequestExcelProcessing;
                myrequestexceltask.Run(parcel);
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        private lib.TaskAsync.TaskAsync myrequestexceltask;
        private KeyValuePair<bool, string> RequestExcelProcessing(object parm)
        {
            ParcelVM parcel = parm as ParcelVM;
            foreach (Classes.Domain.RequestVM item in parcel.ParcelRequests)
                if (item.Importer == null)
                {
                    throw new Exception("В заявке " + item.StorePointDate + " не указан импортер!");
                }
            myrequestexceltask.ProgressChange(5);

            string path = null, num = null;
            if (parcel != null)
            {
                path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + parcel.ParcelNumber ?? string.Empty;
                if (!Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
            }
            else
                return new KeyValuePair<bool, string>(true, "Необходимо выбрать перевозку!");
            myrequestexceltask.ProgressChange(7);
            excel.Application exApp = new excel.Application();
            excel.Application exAppProt = new excel.Application();
            excel.Workbook exWb;
            ListCollectionView view = null;
            try
            {
                exApp.Visible = false;
                exApp.DisplayAlerts = false;
                exApp.ScreenUpdating = false;
                exApp.SheetsInNewWorkbook = 1;
                view = new ListCollectionView(parcel.ParcelRequests.SourceCollection as System.Collections.IList);
                view.SortDescriptions.Add(new SortDescription("CustomerName", ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription("ParcelGroup", ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                view.Filter = (object item) => { Classes.Domain.RequestVM ritem = item as Classes.Domain.RequestVM; return ritem.Importer?.Name == "ДЕЛИВЕРИ" & ritem.DomainObject.ParcelId.HasValue && lib.ViewModelViewCommand.ViewFilterDefault(item); };
                if (view.Count > 0)
                {
                    string templ = Environment.CurrentDirectory + @"\Templates\Заявка на перевозку GTLS GmbH АД.xltx";
                    if (!System.IO.File.Exists(templ))
                        throw new Exception("Шаблон Заявка на перевозку GTLS GmbH АД.xltx не найден!");
                    else
                    {
                        int r = 24;
                        exWb = exApp.Workbooks.Add(templ);
                        excel.Worksheet exWh = exWb.Sheets[1];
                        myrequestexceltask.ProgressChange(10);
                        foreach (Classes.Domain.RequestVM item in view)
                        {
                            if (r > 24)
                            {
                                exWh.Rows[(r - 2).ToString() + ":" + (r - 1).ToString()].Copy();
                                exWh.Rows[r.ToString() + ":" + r.ToString()].Insert(excel.XlInsertShiftDirection.xlShiftDown);
                            }
                            exWh.Cells[r, 3] = r / 2 - 11;
                            if (item.CellNumber.HasValue) exWh.Cells[r, 4] = item.CellNumber.Value;
                            if (item.Volume.HasValue) exWh.Cells[r, 8] = item.Volume.Value;
                            if (item.OfficialWeight.HasValue) exWh.Cells[r, 17] = item.OfficialWeight.Value;

                            r += 2;
                            myrequestexceltask.ProgressChange(10 + (int)(45 * ((r - 24) / view.Count) / 2));
                        }
                        exWb.SaveAs(path + @"\Заявка на перевозку_АД_" + num);
                    }
                }
                view.Filter = (object item) => { Classes.Domain.RequestVM ritem = item as Classes.Domain.RequestVM; return ritem.Importer?.Name == "ТРЕЙД" & ritem.DomainObject.ParcelId.HasValue && lib.ViewModelViewCommand.ViewFilterDefault(item); };
                if (view.Count > 0)
                {
                    string templ = Environment.CurrentDirectory + @"\Templates\Заявка на перевозку GTLS GmbH АТ.xltx";
                    if (!System.IO.File.Exists(templ))
                        throw new Exception("Шаблон Заявка на перевозку GTLS GmbH АТ.xltx не найден!");
                    else
                    {
                        int r = 24;
                        exWb = exApp.Workbooks.Add(templ);
                        excel.Worksheet exWh = exWb.Sheets[1];
                        foreach (Classes.Domain.RequestVM item in view)
                        {
                            if (r > 24)
                            {
                                exWh.Rows[(r - 2).ToString() + ":" + (r - 1).ToString()].Copy();
                                exWh.Rows[r.ToString() + ":" + r.ToString()].Insert(excel.XlInsertShiftDirection.xlShiftDown);
                            }
                            exWh.Cells[r, 3] = r / 2 - 11;
                            if (item.CellNumber.HasValue) exWh.Cells[r, 4] = item.CellNumber.Value;
                            if (item.Volume.HasValue) exWh.Cells[r, 8] = item.Volume.Value;
                            if (item.OfficialWeight.HasValue) exWh.Cells[r, 17] = item.OfficialWeight.Value;

                            r += 2;
                            myrequestexceltask.ProgressChange(55 + (int)(45 * ((r - 24) / view.Count) / 2));
                        }
                        exWb.SaveAs(path + @"\Заявка на перевозку_АТ_" + num);
                    }
                }

                exApp.Visible = true;
                exApp.DisplayAlerts = true;
                exApp.ScreenUpdating = true;
            }
            catch (Exception ex)
            {
                if (exApp != null)
                {
                    foreach (excel.Workbook itemBook in exApp.Workbooks)
                    {
                        itemBook.Close(false);
                    }
                    exApp.Quit();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (view != null)
                {
                    view.DetachFromSourceCollection();
                    view = null;
                }
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }

            myrequestexceltask.ProgressChange(100);
            return new KeyValuePair<bool, string>(false, parcel.ParcelRequests.Count.ToString() + " строк обработано");
        }

        internal void SendMailExec(ParcelVM parcel, object parametr)
        {
            if (parametr != null)
            {
                bool iserr = false;
                int state = int.Parse((string)parametr);
                parcel.DomainObject.MailState.Send(state);
                if (parcel.DomainObject.MailState.SendErrors.Count > 0)
                {
                    System.Text.StringBuilder text = new System.Text.StringBuilder();
                    foreach (lib.DBMError err in parcel.DomainObject.MailState.SendErrors)
                    {
                        text.AppendLine(err.Message);
                        iserr |= !string.Equals(err.Code, "0");
                    }
                    if (iserr) { text.Insert(0, "Отправка выполнена с ошибкой!\n"); }
                    this.OpenPopup(text.ToString(), iserr);
                }

            }
        }
        internal void SpecFolderOpenExec(ParcelVM parcel)
        {
            try
            {
                if (parcel != null)
                {
                    string path = CustomBrokerWpf.Properties.Settings.Default.DetailsFileRoot;
                    if (!Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    System.Diagnostics.Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                this.OpenPopup("Папка документов\n" + ex.Message, true);
            }
        }

        private lib.TaskAsync.TaskAsync myexceltask;
        private KeyValuePair<bool, string> OnExcelImport(object parm)
        {
            object[] param = parm as object[];
            string filepath = (string)param[0];
            Specification.Specification spec = (Specification.Specification)param[1];
            return new KeyValuePair<bool, string>(false, "Разбивка загружена. " + spec.ImportDetail(filepath, myexceltask).ToString() + " строк обработано.");
        }
        internal void SpecAddExec(ParcelVM parcel, object parametr)
        {
            if (myexceltask == null)
                myexceltask = new lib.TaskAsync.TaskAsync();
            if (!myexceltask.IsBusy)
            {
                if (parametr is RequestVM)
                {
                    RequestVM request = parametr as RequestVM;
                    System.Text.StringBuilder path = new System.Text.StringBuilder();
                    string rootdir = CustomBrokerWpf.Properties.Settings.Default.DetailsFileRoot;
                    OpenFileDialog fd = new OpenFileDialog();
                    fd.Multiselect = false;
                    fd.CheckPathExists = true;
                    fd.CheckFileExists = true;
                    if (System.IO.Directory.Exists(CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault)) fd.InitialDirectory = CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault;
                    fd.Title = "Выбор файла разбивки";
                    fd.Filter = "Файлы Excel|*.xls;*.xlsx;*.xlsm;";
                    if (fd.ShowDialog().Value)
                    {
                        try
                        {
                            if (!System.IO.Directory.Exists(rootdir))
                                System.IO.Directory.CreateDirectory(rootdir);
                            Specification.Specification spec = parcel.DomainObject.Specifications.FirstOrDefault<Specification.Specification>((Specification.Specification item) => { return ViewModelViewCommand.ViewFilterDefault(item) && item.Consolidate == request.Consolidate && item.ParcelGroup == (string.IsNullOrEmpty(request.Consolidate) ? request.ParcelGroup : null) && item.Request == (string.IsNullOrEmpty(request.Consolidate) & !request.ParcelGroup.HasValue ? request.DomainObject : null); });
                            if (spec != null)
                            {
                                if (spec.Details.Count > 0)
                                {
                                    if (System.Windows.MessageBox.Show("Разбивка уже загружена. Перезаписать?", "Загрузка разбивок", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.No)
                                        return;
                                    else
                                    {
                                        Specification.SpecificationVM specvm = null;
                                        foreach (Specification.SpecificationVM vm in parcel.Specifications)
                                            if (vm.DomainObject == spec) specvm = vm;
                                        ObservableCollection<Specification.SpecificationDetailVM> detsvm = specvm.Details.SourceCollection as ObservableCollection<Specification.SpecificationDetailVM>;
                                        for (int i = 0; i < detsvm.Count; i++)
                                        {
                                            if (detsvm[i].DomainState == lib.DomainObjectState.Added)
                                            {
                                                detsvm.RemoveAt(i);
                                                i--;
                                            }
                                            else
                                            {
                                                Specification.SpecificationDetailVM item = detsvm[i];
                                                specvm.Details.EditItem(item);
                                                item.DomainState = lib.DomainObjectState.Deleted;
                                                specvm.Details.CommitEdit();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                spec = new Specification.Specification(
                                    parcel: parcel.DomainObject,
                                    consolidate: request.Consolidate,
                                    parcelgroup: string.IsNullOrEmpty(request.Consolidate) ? request.ParcelGroup : null,
                                    request: string.IsNullOrEmpty(request.Consolidate) & !request.ParcelGroup.HasValue ? request.DomainObject : null,
                                    agent: CustomBrokerWpf.References.AgentStore.GetItemLoad(request.AgentId ?? 0, out _),
                                    importer: request.Importer);
                                spec.CustomersLegalsRefresh();
                                parcel.Specifications.AddNewItem(new Specification.SpecificationVM(spec));
                                parcel.Specifications.CommitNew();
                            }
                            if (string.IsNullOrEmpty(spec.FilePath)) spec.BuildFileName(fd.FileName);
                            path.Append(System.IO.Path.Combine(rootdir, spec.FilePath));
                            if (System.IO.File.Exists(path.ToString()))
                            {
                                if (!fd.FileName.Equals(path.ToString(), StringComparison.InvariantCultureIgnoreCase))
                                {
                                    System.IO.File.Delete(path.ToString());
                                    System.IO.File.Copy(fd.FileName, path.ToString());
                                }
                            }
                            else
                                System.IO.File.Copy(fd.FileName, path.ToString());
                            if (CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault != System.IO.Path.GetDirectoryName(fd.FileName))
                            {
                                CustomBrokerWpf.Properties.Settings.Default.DetailsFileDefault = System.IO.Path.GetDirectoryName(fd.FileName);
                                CustomBrokerWpf.Properties.Settings.Default.Save();
                            }
                            myexceltask.DoProcessing = OnExcelImport;
                            myexceltask.Run(new object[2] { path.ToString(), spec });
                        }
                        catch (Exception ex)
                        {
                            this.OpenPopup("Не удалось загрузить файл.\n" + ex.Message, true);
                        }
                    }
                }
                else
                    this.OpenPopup("Необходимо выделить заявку в верхнем списке!", false);
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }
        internal bool SpecAddCanExec(ParcelVM parcel)
        { return parcel != null & (myexceltask == null || !myexceltask.IsBusy); }

        internal void SpecDelExec(ParcelVM parcel, object parametr)
        {
            if ((parametr is System.Collections.IEnumerable | parametr is IViewModelBaseItem) && System.Windows.MessageBox.Show("Удалить выделенные спецификации", "Удаление", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
            {
                if (parametr is System.Collections.IEnumerable)
                {
                    List<IViewModelBaseItem> list = new List<IViewModelBaseItem>();
                    if (parcel.Specifications.IsAddingNew) parcel.Specifications.CancelNew();
                    if (parcel.Specifications.CanCancelEdit) parcel.Specifications.CancelEdit();
                    foreach (object item in parametr as System.Collections.IEnumerable)
                    {
                        if (item is IViewModelBaseItem) list.Add(item as IViewModelBaseItem);
                    }
                    foreach (Specification.SpecificationVM item in list)
                    {
                        parcel.Specifications.EditItem(item);
                        item.DomainState = lib.DomainObjectState.Deleted;
                        parcel.Specifications.CommitEdit();
                    }
                }
                else if (parametr is Specification.SpecificationVM)
                {
                    Specification.SpecificationVM item = parametr as Specification.SpecificationVM;
                    parcel.Specifications.EditItem(item);
                    item.DomainState = lib.DomainObjectState.Deleted;
                    parcel.Specifications.CommitEdit();
                }
            }
        }
        internal bool SpecDelCanExec(ParcelVM parcel)
        { return parcel != null && !parcel.DomainObject.SpecificationsIsNull && parcel.Specifications.CurrentItem != null; }

        internal void TDLoadExec(object parametr)
        {
            if (parametr is Specification.SpecificationVM)
            {
                Specification.Specification spec = (parametr as Specification.SpecificationVM).DomainObject;
                EventLoger log = new EventLoger() { What = "DT", Message = (spec?.Declaration?.Number ?? "Новая") + " Start Parcel", ObjectId = (spec?.Declaration?.Id ?? 0) };
                log.Execute();
                string err = spec.LoadDeclaration();
                if (string.IsNullOrEmpty(err))
                    this.OpenPopup("ТД загружена!", false);
                else
                    this.OpenPopup(err, true);
                log.Message = (spec?.Declaration?.Number ?? "Новая") + " Finish Parcel";
                log.ObjectId = (spec?.Declaration?.Id ?? 0);
                log.Execute();
            }
        }

        internal void Selling1CExec(ParcelVM parcel, object parametr)
        {
            if (parametr is System.Collections.IEnumerable & parcel != null)
            {
                List<Specification.SpecificationVM> speclist = (parametr as System.Collections.IEnumerable).OfType<Specification.SpecificationVM>().ToList();
                if (speclist.Count == 0)
                {
                    if (System.Windows.MessageBox.Show("Подготовить реализацию для всех разбивок?", "Реализация для 1С", System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.OK)
                        speclist = parcel.Specifications.SourceCollection.OfType<Specification.SpecificationVM>().ToList();
                    else
                        return;
                }
                foreach (Specification.SpecificationVM item in speclist)
                    if (!string.IsNullOrEmpty(item.FilePath))
                    {
                        item.DomainObject.Income1C();
                        item.DomainObject.Selling1C();
                    }
            }
        }
        internal bool Selling1CCanExec(ParcelVM parcel)
        { return parcel != null && parcel.Specifications.Count > 0; }


        internal bool ParcelIsNull(ParcelVM parcel)
        { return parcel != null; }
        internal bool MoveSpecificationCanExec(ParcelVM parcel)
        { return parcel?.ParcelType?.Id == 1; }

        internal ListCollectionView InitStates()
        {
            ListCollectionView states = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
            states.Filter = (object item) => { lib.ReferenceSimpleItem state = item as lib.ReferenceSimpleItem; return state.Id > 49 && state.Id != 104 && state.Id != 107 && state.Id != 120; };
            states.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            return states;
        }
        internal ListCollectionView InitRequestStates()
        {
            ListCollectionView requeststates = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
            requeststates.Filter = (object item) => { return (item as lib.ReferenceSimpleItem).Id < 50; };
            requeststates.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            return requeststates;
        }
        internal ListCollectionView InitParcelRequestStates()
        {
            ListCollectionView states = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
            states.Filter = (object item) => { lib.ReferenceSimpleItem state = item as lib.ReferenceSimpleItem; return state.Id > 49 && state.Id != 110; };
            states.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
            return states;
        }
        internal ListCollectionView InitGoods()
        {
            ListCollectionView goodstypes = new ListCollectionView(CustomBrokerWpf.References.GoodsTypesParcel);
            goodstypes.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            return goodstypes;
        }
        internal ListCollectionView InitManagers()
        {
            ListCollectionView managers = null;
            if (CustomBrokerWpf.References.CurrentUserRoles.Contains("TopManagers"))
                managers = new ListCollectionView(CustomBrokerWpf.References.Managers);
            else if (CustomBrokerWpf.References.CurrentManager != null)
            {
                managers = new ListCollectionView(new List<Manager>() { new Manager(), CustomBrokerWpf.References.CurrentManager });
                managers.Filter = (object item) => { return (item as Manager).Unfile == 0; };
            }
            return managers;
        }
    }

    public class ParcelCommander : lib.ViewModelCommand<ParcelRecord,Parcel, ParcelVM, ParcelDBM>
    {
        public ParcelCommander(ParcelVM parcel, ListCollectionView view) : base(parcel, view)
        {
            base.PropertyChanged += ParcelCommander_PropertyChanged;
            mycommands = new ParcelCommands()
            {
                OpenPopup = this.OpenPopup,
                EndEdit = this.EndEdit,
                SaveDataChanges = this.SaveDataChanges
            };

            mycreateexcelreport = new RelayCommand(CreateExcelReportExec, CreateExcelReportCanExec);
            myfolderopen = new RelayCommand(FolderOpenExec, FolderOpenCanExec);
            mymovespecification = new RelayCommand(MoveSpecificationExec, MoveSpecificationCanExec);
            myrequestexcel = new RelayCommand(RequestExcelExec, RequestExcelCanExec);
            myselling1c = new RelayCommand(Selling1CExec, Selling1CCanExec);
            mysendmail = new RelayCommand(SendMailExec, SendMailCanExec);
            mysetstoreinform = new RelayCommand(SetStoreInformExec, SetStoreInformCanExec);
            myspecadd = new RelayCommand(SpecAddExec, SpecAddCanExec);
            myspecdel = new RelayCommand(SpecDelExec, SpecDelCanExec);
            myspecfolderopen = new RelayCommand(SpecFolderOpenExec, SpecFolderOpenCanExec);
            mytdload = new RelayCommand(TDLoadExec, TDLoadCanExec);

            mymanagers = mycommands.InitManagers();
            mystates = mycommands.InitStates();
            myrequeststates = mycommands.InitRequestStates();
            myparcelrequeststates = mycommands.InitParcelRequestStates();
            mygoodstypes = mycommands.InitGoods();
        }

        private void ParcelCommander_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(base.VModel))
                this.PropertyChangedNotification(nameof(this.Title));
        }

        public string Title
        { get { return "Отправка " + VModel.ParcelNumberEntire; } }
        public System.Windows.Visibility ChooseVisible
        { get { return System.Windows.Visibility.Collapsed; } }
        public System.Windows.Visibility CloseVisible
        { get { return System.Windows.Visibility.Visible; } }


        private ListCollectionView myagents;
        public ListCollectionView Agents
        {
            get
            {
                if (myagents == null)
                {
                    myagents = new ListCollectionView(CustomBrokerWpf.References.AgentNames);
                    CustomBrokerWpf.References.AgentNames.RefreshViewAdd(myagents);
                    myagents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myagents;
            }
        }
        private System.Data.DataView mycustomers;
        public System.Data.DataView Customers
        {
            get
            {
                if (mycustomers == null)
                {
                    ReferenceDS refds = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
                    if (refds.tableCustomerName.Count == 0) refds.CustomerNameRefresh();
                    mycustomers = new System.Data.DataView(refds.tableCustomerName, string.Empty, "customerName", System.Data.DataViewRowState.CurrentRows);
                }
                return mycustomers;
            }
        }
        private ListCollectionView mygoodstypes;
        public ListCollectionView GoodsTypes
        {
            get
            {
                if (mygoodstypes == null)
                    mygoodstypes = mycommands.InitGoods();
                return mygoodstypes;
            }
        }
        private ListCollectionView myimporters;
        public ListCollectionView Importers
        {
            get
            {
                if (myimporters == null)
                {
                    myimporters = new ListCollectionView(CustomBrokerWpf.References.Importers);
                    myimporters.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myimporters;
            }
        }
        private ListCollectionView myloaddescriptions;
        public ListCollectionView LoadDescriptions
        {
            get
            {
                if (myloaddescriptions == null)
                {
                    myloaddescriptions = new ListCollectionView(CustomBrokerWpf.References.GoodsTypesParcel);
                    myloaddescriptions.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myloaddescriptions;
            }
        }
        private ListCollectionView mymanagers;
        public ListCollectionView Managers
        { get { return mymanagers; } }
        private ListCollectionView myparcelrequeststates;
        public ListCollectionView ParcelRequestStates
        { get { return myparcelrequeststates; } }
        private ListCollectionView myrequeststates;
        public ListCollectionView RequestStates
        {
            get
            {
                return myrequeststates;
            }
        }
        private ListCollectionView myservicetypes;
        public ListCollectionView ServiceTypes
        {
            get
            {
                if (myservicetypes == null)
                {
                    myservicetypes = new ListCollectionView(CustomBrokerWpf.References.ServiceTypes);
                }
                return myservicetypes;
            }
        }
        private ListCollectionView mystates;
        public ListCollectionView States
        {
            get
            {
                return mystates;
            }
        }

        ParcelCommands mycommands;

        private RelayCommand mycreateexcelreport;
        public ICommand CreateExcelReport
        {
            get { return mycreateexcelreport; }
        }
        private void CreateExcelReportExec(object parametr)
        {
            mycommands.CreateExcelReportExec(this.VModel, parametr);
        }
        private bool CreateExcelReportCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.VModel); }

        private RelayCommand myfolderopen;
        public ICommand FolderOpen
        {
            get { return myfolderopen; }
        }
        private void FolderOpenExec(object parametr)
        {
            try
            {
                mycommands.FolderOpenExec(this.VModel);
            }
            catch (Exception ex)
            {
                this.OpenPopup("Папка документов\n" + ex.Message, true);
            }
        }
        private bool FolderOpenCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.VModel); }

        private RelayCommand mysetstoreinform;
        public ICommand SetStoreInform
        {
            get { return mysetstoreinform; }
        }
        private void SetStoreInformExec(object parametr)
        {
            if (this.VModel == null) return;
            if (this.EndEdit())
            {
                mycommands.SetStoreInformExec(this.VModel);
            }
            else
                this.OpenPopup("Не удалось применить изменения! Проверте корректность и полноту данных.", true);
        }
        private bool SetStoreInformCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.VModel); }

        private RelayCommand myrequestexcel;
        public ICommand RequestExcel
        { get { return myrequestexcel; } }
        private void RequestExcelExec(object parametr)
        {
            mycommands.RequestExcelExec(this.VModel);
        }
        private bool RequestExcelCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.VModel); }

        private RelayCommand mymovespecification;
        public ICommand MoveSpecification
        {
            get { return mymovespecification; }
        }
        private void MoveSpecificationExec(object parametr)
        {
            mycommands.MoveSpecificationExec(this.VModel);
        }
        private bool MoveSpecificationCanExec(object parametr)
        { return mycommands.MoveSpecificationCanExec(this.VModel); }

        private RelayCommand mysendmail;
        public ICommand SendMail
        {
            get { return mysendmail; }
        }
        private void SendMailExec(object parametr)
        {
            mycommands.SendMailExec(this.VModel, parametr);
        }
        private bool SendMailCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.VModel); }

        private RelayCommand myspecfolderopen;
        public ICommand SpecFolderOpen
        {
            get { return myspecfolderopen; }
        }
        private void SpecFolderOpenExec(object parametr)
        {
            mycommands.SpecFolderOpenExec(this.VModel);
        }
        private bool SpecFolderOpenCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.VModel); }

        private RelayCommand myspecadd;
        public ICommand SpecAdd
        {
            get { return myspecadd; }
        }
        private void SpecAddExec(object parametr)
        {
            mycommands.SpecAddExec(this.VModel, parametr);
        }
        private bool SpecAddCanExec(object parametr)
        { return mycommands.SpecAddCanExec(this.VModel); }

        private RelayCommand myspecdel;
        public ICommand SpecDel
        {
            get { return myspecdel; }
        }
        private void SpecDelExec(object parametr)
        {
            mycommands.SpecDelExec(this.VModel, parametr);
        }
        private bool SpecDelCanExec(object parametr)
        { return mycommands.SpecDelCanExec(this.VModel); }

        private RelayCommand mytdload;
        public ICommand TDLoad
        {
            get { return mytdload; }
        }
        private void TDLoadExec(object parametr)
        {
            mycommands.TDLoadExec(parametr);
        }
        private bool TDLoadCanExec(object parametr)
        { return true; }

        private RelayCommand myselling1c;
        public ICommand Selling1C
        {
            get { return myselling1c; }
        }
        private void Selling1CExec(object parametr)
        {
            mycommands.Selling1CExec(this.VModel, parametr);
        }
        private bool Selling1CCanExec(object parametr)
        { return mycommands.Selling1CCanExec(this.VModel); }

        protected override bool CanDeleteData(object parametr)
        {
            return base.VModel?.Status.Id < 40 && base.VModel?.ParcelRequests.Count > 0;
        }
        protected override void RefreshData(object parametr)
        {
            this.VModel.ParcelRequestsTotal.StopCount();
            this.VModel.ParcelRequestsTotalDelivery.StopCount();
            this.VModel.ParcelRequestsTotalTrade.StopCount();
            this.VModel.ParcelRequestsTotalSelected.StopCount();
            this.VModel.RequestsTotalSelected?.StopCount();
            this.VModel.RequestsTotalSelectedDelivery?.StopCount();
            this.VModel.RequestsTotalSelectedTrade?.StopCount();
            CustomBrokerWpf.References.ParcelLastShipdate.Update();
            base.RefreshData(parametr);
            if (this.VModel.IsEnabled)
            {
                this.VModel.Requests.Refresh();
                this.VModel.ParcelRequests.Refresh();
                this.VModel.DomainObject.SpecificationsRefresh();
                this.VModel.ParcelRequestsTotal.StartCount();
                this.VModel.ParcelRequestsTotalDelivery.StartCount();
                this.VModel.ParcelRequestsTotalTrade.StartCount();
                this.VModel.ParcelRequestsTotalSelected.StartCount();
                this.VModel.RequestsTotalSelected?.StartCount();
                this.VModel.RequestsTotalSelectedDelivery?.StartCount();
                this.VModel.RequestsTotalSelectedTrade?.StartCount();
            }
        }
        public override bool SaveDataChanges()
        {
            DirectoryInfo dir = new DirectoryInfo(CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + "Отправки\\");
            if (!dir.Exists) dir.Create();
            bool isSuccess = true;
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            err.AppendLine("Изменения не сохранены");
            isSuccess = this.VModel == null || !(this.VModel.DomainState == lib.DomainObjectState.Added || this.VModel.DomainState == lib.DomainObjectState.Modified) || this.VModel.Validate(true);
            if (!isSuccess)
                err.AppendLine(this.VModel.Errors);
            if (this.VModel != null && this.VModel.Status.Id == 50)
            {
                foreach (RequestVM item in this.VModel.ParcelRequests)
                {
                    if (item.DomainState == lib.DomainObjectState.Added || item.DomainState == lib.DomainObjectState.Modified)
                    {
                        if (!item.Validate(true))
                        {
                            err.AppendLine(item.Errors);
                            isSuccess = false;
                        }
                    }
                }
                foreach (RequestVM item in this.VModel.Requests)
                {
                    if (item.DomainState == lib.DomainObjectState.Added || item.DomainState == lib.DomainObjectState.Modified)
                    {
                        if (!item.Validate(true))
                        {
                            err.AppendLine(item.Errors);
                            isSuccess = false;
                        }
                    }
                }
            }
            Parcel parcel = this.VModel.DomainObject;
            if (mydbm == null)
                mydbm = new ParcelDBM();
            else
                mydbm.Errors.Clear();
            if (parcel.DomainState == lib.DomainObjectState.Added)
            {
                if (!mydbm.SaveItemChanches(parcel))
                {
                    isSuccess = false;
                    err.AppendLine(mydbm.ErrorMessage);
                }
                try
                {
                    if (parcel.DocDirPath != null & !Directory.Exists(dir.FullName + "\\" + parcel.DocDirPath)) dir.CreateSubdirectory(parcel.DocDirPath);
                }
                catch (Exception ex)
                {
                    err.AppendLine("Сохранение изменений/n" + "Не удалось создать папку для документов Доставки " + parcel.ParcelNumberEntire + " !\n" + ex.Message);
                }
                mydbm.Errors.Clear();
                if (!mydbm.CheckGroup(parcel))
                    foreach (lib.DBMError erm in mydbm.Errors)
                    {
                        err.AppendLine(erm.Message);
                        if (erm.Code != "group")
                            isSuccess = false;
                    }
            }
            else if (parcel.DomainState == lib.DomainObjectState.Modified)
            {
                if (parcel.DocDirPath != parcel.ParcelNumber)
                {
                    try
                    {
                        DirectoryInfo parceldir = new DirectoryInfo(dir.FullName + "\\" + parcel.DocDirPath);
                        if (parceldir.Exists)
                            parceldir.MoveTo(dir.FullName + "\\" + parcel.ParcelNumber);
                        else
                            if (!Directory.Exists(dir.FullName + "\\" + parcel.ParcelNumber)) dir.CreateSubdirectory(parcel.ParcelNumber);
                        parcel.DocDirPath = parcel.ParcelNumber;
                    }
                    catch (Exception ex)
                    {
                        err.AppendLine("Сохранение изменений\nНе удалось переименовать папку для документов Доставки!\n\n" + ex.Message);
                    }

                }
                if (!mydbm.SaveItemChanches(parcel))
                {
                    isSuccess = false;
                    err.AppendLine(mydbm.ErrorMessage);
                }
                mydbm.Errors.Clear();
                if (!mydbm.CheckGroup(parcel))
                    foreach (lib.DBMError erm in mydbm.Errors)
                    {
                        err.AppendLine(erm.Message);
                        if (erm.Code != "group")
                            isSuccess = false;
                    }
            }
            else
                if (!mydbm.SaveItemChanches(parcel))
            {
                isSuccess = false;
                err.AppendLine(mydbm.ErrorMessage);
            }

            if (!isSuccess) this.PopupText = err.ToString();
            return isSuccess;
        }
    }

    public class ParcelViewCommander : lib.ViewModelViewCommand, lib.Interfaces.IFilterWindowOwner
    {
        internal ParcelViewCommander() : base()
        {
            mycommands = new ParcelCommands();
            myfilter = new lib.SQLFilter.SQLFilter("parcel", "AND", CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            mypdbm = new ParcelDBM();
            mydbm = mypdbm;
            mypdbm.Filter = myfilter.FilterWhereId;
            mysync = new ParcelSynchronizer();
            if (CustomBrokerWpf.References.Parcels == null)
            {
                CustomBrokerWpf.References.Parcels = new ObservableCollection<Parcel>();
                mypdbm.Collection = CustomBrokerWpf.References.Parcels;
                mypdbm.FillAsyncCompleted = () =>
                {
                    if (mydbm.Errors.Count > 0)
                        OpenPopup(mydbm.ErrorMessage, true);
                    mypdbm.FillType = lib.FillType.Refresh;
                    mypdbm.RequestRefreshFill = true; // load request for new parcel
                    SettingView();
                };
                mypdbm.FillAsync();
                mysync.DomainCollection = mypdbm.Collection;
                base.Collection = mysync.ViewModelCollection;
            }
            else
            {
                mypdbm.Collection = CustomBrokerWpf.References.Parcels;
                mypdbm.FillType = lib.FillType.Refresh;
                mypdbm.RequestRefreshFill = true; // load request for new parcel
                mysync.DomainCollection = mypdbm.Collection;
                base.Collection = mysync.ViewModelCollection;
                SettingView();
            }

            base.DeleteQuestionHeader = "Удалить перевозку?";
            mystates = mycommands.InitStates();
            mygoodstypes = mycommands.InitGoods();
            myfilterbuttonimagepath = @"/CustomBrokerWpf;component/Images/funnel.png";
        }

        private ListCollectionView mystates;
        public ListCollectionView States
        {
            get
            {
                if (mystates == null)
                    mystates = mycommands.InitStates();
                return mystates;
            }
        }
        private ListCollectionView mygoodstypes;
        public ListCollectionView GoodsTypes
        {
            get
            {
                if (mygoodstypes == null)
                    mygoodstypes = mycommands.InitGoods();
                return mygoodstypes;
            }
        }

        ParcelCommands mycommands;
        ParcelDBM mypdbm;
        ParcelSynchronizer mysync;
        private lib.SQLFilter.SQLFilter myfilter;
        public lib.SQLFilter.SQLFilter Filter
        {
            get { return myfilter; }
        }
        private bool myisshowfilterwindow;
        public bool IsShowFilterWindow
        {
            set
            {
                myisshowfilterwindow = value;
                this.PropertyChangedNotification(nameof(this.IsShowFilterWindow));
            }
            get { return myisshowfilterwindow; }
        }
        public void RunFilter(lib.Filter.FilterItem[] filters)
        {
            if (!SaveDataChanges())
                this.OpenPopup("Применение фильтра\nПрименение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", true);
            else
            {
                this.Refresh.Execute(null);
            }
        }
        private string myfilterbuttonimagepath;
        public string FilterButtonImagePath
        { get { return myfilterbuttonimagepath; } }
        public string IsFiltered
        { get { return myfilter.isEmpty ? string.Empty : "Фильтр!"; } }

        protected override void OtherViewRefresh()
        {
            CustomBrokerWpf.References.ParcelViewCollector.RefreshViews(this.Items as IRefresh);
        }
        protected override void RefreshData(object parametr)
        {
            CustomBrokerWpf.References.ParcelLastShipdate.Update();
            mypdbm.Filter = myfilter.FilterWhereId;
            mypdbm.Fill();
            ParcelSetFilterButtonImage();
        }
        protected override void SettingView()
        {
            myview.Filter = lib.ViewModelViewCommand.ViewFilterDefault;
            myview.SortDescriptions.Add(new SortDescription("ParcelNumberOrder", ListSortDirection.Descending));
            CustomBrokerWpf.References.ParcelViewCollector.AddView(this.Items as IRefresh);
        }

        private void ParcelSetFilterButtonImage()
        {
            //string uribitmap;
            if (myfilter.isEmpty) myfilterbuttonimagepath = @"/CustomBrokerWpf;component/Images/funnel.png";
            else myfilterbuttonimagepath = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            this.PropertyChangedNotification(nameof(this.FilterButtonImagePath));
            this.PropertyChangedNotification(nameof(this.IsFiltered));
            //System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            //(ParcelFilterButton.Content as Image).Source = bi3;
        }
    }

    public class ParcelCurItemCommander : lib.ViewModelCurrentItemCommand<ParcelVM>, lib.Interfaces.IFilterWindowOwner
    {
        internal ParcelCurItemCommander() : base()
        {
            mycommands = new ParcelCommands()
            {
                OpenPopup = this.OpenPopup,
                EndEdit = this.EndEdit,
                SaveDataChanges = this.SaveDataChanges
            };
            myfilter = new lib.SQLFilter.SQLFilter("parcel", "AND", CustomBrokerWpf.References.ConnectionString);
            myfilter.GetDefaultFilter(lib.SQLFilter.SQLFilterPart.Where);
            mypdbm = new ParcelDBM();
            mydbm = mypdbm;
            mypdbm.Filter = myfilter.FilterWhereId;
            mypdbm.FillAsyncCompleted = () =>
            {
                if (mydbm.Errors.Count > 0)
                    OpenPopup(mydbm.ErrorMessage, true);
                mypdbm.FillType = lib.FillType.Refresh;
                mypdbm.RequestRefreshFill = true; // load request for new parcel
                SettingView();
            };
            mypdbm.Collection = new ObservableCollection<Parcel>();
            mypdbm.FillAsync();
            base.Collection = mypdbm.Collection;
            CustomBrokerWpf.References.Parcels = mypdbm.Collection;
            base.DeleteQuestionHeader = "Удалить перевозку?";

            mycreateexcelreport = new RelayCommand(CreateExcelReportExec, CreateExcelReportCanExec);
            myfolderopen = new RelayCommand(FolderOpenExec, FolderOpenCanExec);
            mymovespecification = new RelayCommand(MoveSpecificationExec, MoveSpecificationCanExec);
            myrequestexcel = new RelayCommand(RequestExcelExec, RequestExcelCanExec);
            myselling1c = new RelayCommand(Selling1CExec, Selling1CCanExec);
            mysendmail = new RelayCommand(SendMailExec, SendMailCanExec);
            mysetstoreinform = new RelayCommand(SetStoreInformExec, SetStoreInformCanExec);
            myspecadd = new RelayCommand(SpecAddExec, SpecAddCanExec);
            myspecdel = new RelayCommand(SpecDelExec, SpecDelCanExec);
            myspecfolderopen = new RelayCommand(SpecFolderOpenExec, SpecFolderOpenCanExec);
            mytdload = new RelayCommand(TDLoadExec, TDLoadCanExec);

            mymanagers = mycommands.InitManagers();
            mystates = mycommands.InitStates();
            myrequeststates = mycommands.InitRequestStates();
            myparcelrequeststates = mycommands.InitParcelRequestStates();
            mygoodstypes = mycommands.InitGoods();
        }

        ParcelCommands mycommands;
        ParcelDBM mypdbm;
        private lib.SQLFilter.SQLFilter myfilter;
        public lib.SQLFilter.SQLFilter Filter
        {
            get { return myfilter; }
        }
        private bool myisshowfilterwindow;
        public bool IsShowFilterWindow
        {
            set
            {
                myisshowfilterwindow = value;
                this.PropertyChangedNotification(nameof(this.IsShowFilterWindow));
            }
            get { return myisshowfilterwindow; }
        }
        public void RunFilter(lib.Filter.FilterItem[] filters)
        {
            if (!SaveDataChanges())
                this.OpenPopup("Применение фильтра\nПрименение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", true);
            else
            {
                this.Refresh.Execute(null);
            }
        }
        private string myfilterbuttonimagepath;
        public string FilterButtonImagePath
        { get { return myfilterbuttonimagepath; } }
        public string IsFiltered
        { get { return myfilter.isEmpty ? string.Empty : "Фильтр!"; } }

        public System.Windows.Visibility ChooseVisible
        { get { return System.Windows.Visibility.Visible; } }
        public System.Windows.Visibility CloseVisible
        { get { return System.Windows.Visibility.Collapsed; } }

        private RelayCommand myfolderopen;
        public ICommand FolderOpen
        {
            get { return myfolderopen; }
        }
        private void FolderOpenExec(object parametr)
        {
            try
            {
                mycommands.FolderOpenExec(this.CurrentItem);
            }
            catch (Exception ex)
            {
                this.OpenPopup("Папка документов\n" + ex.Message, true);
            }
        }
        private bool FolderOpenCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.CurrentItem); }

        private RelayCommand mysetstoreinform;
        public ICommand SetStoreInform
        {
            get { return mysetstoreinform; }
        }
        private void SetStoreInformExec(object parametr)
        {
            if (this.CurrentItem == null) return;
            if (this.EndEdit())
            {
                mycommands.SetStoreInformExec(this.CurrentItem);
            }
            else
                this.OpenPopup("Не удалось применить изменения! Проверте корректность и полноту данных.", true);
        }
        private bool SetStoreInformCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.CurrentItem); }

        private RelayCommand mymovespecification;
        public ICommand MoveSpecification
        {
            get { return mymovespecification; }
        }
        private void MoveSpecificationExec(object parametr)
        {
            mycommands.MoveSpecificationExec(this.CurrentItem);
        }
        private bool MoveSpecificationCanExec(object parametr)
        { return mycommands.MoveSpecificationCanExec(this.CurrentItem); }

        private RelayCommand mycreateexcelreport;
        public ICommand CreateExcelReport
        {
            get { return mycreateexcelreport; }
        }
        private void CreateExcelReportExec(object parametr)
        {
            mycommands.CreateExcelReportExec(this.CurrentItem, parametr);
        }
        private bool CreateExcelReportCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.CurrentItem); }

        private RelayCommand mysendmail;
        public ICommand SendMail
        {
            get { return mysendmail; }
        }
        private void SendMailExec(object parametr)
        {
            mycommands.SendMailExec(this.CurrentItem, parametr);
        }
        private bool SendMailCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.CurrentItem); }

        private RelayCommand myspecfolderopen;
        public ICommand SpecFolderOpen
        {
            get { return myspecfolderopen; }
        }
        private void SpecFolderOpenExec(object parametr)
        {
            mycommands.SpecFolderOpenExec(this.CurrentItem);
        }
        private bool SpecFolderOpenCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.CurrentItem); }

        private RelayCommand myspecadd;
        public ICommand SpecAdd
        {
            get { return myspecadd; }
        }
        private void SpecAddExec(object parametr)
        {
            mycommands.SpecAddExec(this.CurrentItem, parametr);
        }
        private bool SpecAddCanExec(object parametr)
        { return mycommands.SpecAddCanExec(this.CurrentItem); }

        private RelayCommand myspecdel;
        public ICommand SpecDel
        {
            get { return myspecdel; }
        }
        private void SpecDelExec(object parametr)
        {
            mycommands.SpecDelExec(this.CurrentItem, parametr);
        }
        private bool SpecDelCanExec(object parametr)
        { return mycommands.SpecDelCanExec(this.CurrentItem); }

        private RelayCommand mytdload;
        public ICommand TDLoad
        {
            get { return mytdload; }
        }
        private void TDLoadExec(object parametr)
        {
            mycommands.TDLoadExec(parametr);
        }
        private bool TDLoadCanExec(object parametr)
        { return true; }

        private RelayCommand myselling1c;
        public ICommand Selling1C
        {
            get { return myselling1c; }
        }
        private void Selling1CExec(object parametr)
        {
            mycommands.Selling1CExec(this.CurrentItem, parametr);
        }
        private bool Selling1CCanExec(object parametr)
        { return mycommands.Selling1CCanExec(this.CurrentItem); }

        private RelayCommand myrequestexcel;
        public ICommand RequestExcel
        { get { return myrequestexcel; } }
        private void RequestExcelExec(object parametr)
        {
            mycommands.RequestExcelExec(this.CurrentItem);
        }
        private bool RequestExcelCanExec(object parametr)
        { return mycommands.ParcelIsNull(this.CurrentItem); }

        private ListCollectionView myagents;
        public ListCollectionView Agents
        {
            get
            {
                if (myagents == null)
                {
                    myagents = new ListCollectionView(CustomBrokerWpf.References.AgentNames);
                    CustomBrokerWpf.References.AgentNames.RefreshViewAdd(myagents);
                    myagents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myagents;
            }
        }
        private System.Data.DataView mycustomers;
        public System.Data.DataView Customers
        {
            get
            {
                if (mycustomers == null)
                {
                    ReferenceDS refds = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
                    if (refds.tableCustomerName.Count == 0) refds.CustomerNameRefresh();
                    mycustomers = new System.Data.DataView(refds.tableCustomerName, string.Empty, "customerName", System.Data.DataViewRowState.CurrentRows);
                }
                return mycustomers;
            }
        }
        private ListCollectionView mygoodstypes;
        public ListCollectionView GoodsTypes
        {
            get
            {
                if (mygoodstypes == null)
                    mygoodstypes = mycommands.InitGoods();
                return mygoodstypes;
            }
        }
        private ListCollectionView myimporters;
        public ListCollectionView Importers
        {
            get
            {
                if (myimporters == null)
                {
                    myimporters = new ListCollectionView(CustomBrokerWpf.References.Importers);
                    myimporters.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myimporters;
            }
        }
        private ListCollectionView myloaddescriptions;
        public ListCollectionView LoadDescriptions
        {
            get
            {
                if (myloaddescriptions == null)
                {
                    myloaddescriptions = new ListCollectionView(CustomBrokerWpf.References.GoodsTypesParcel);
                    myloaddescriptions.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myloaddescriptions;
            }
        }
        private ListCollectionView mymanagers;
        public ListCollectionView Managers
        { get { return mymanagers; } }
        private ListCollectionView myrequeststates;
        private ListCollectionView myparcelrequeststates;
        public ListCollectionView ParcelRequestStates
        { get { return myparcelrequeststates; } }
        public ListCollectionView RequestStates
        {
            get
            {
                return myrequeststates;
            }
        }
        private ListCollectionView myservicetypes;
        public ListCollectionView ServiceTypes
        {
            get
            {
                if (myservicetypes == null)
                {
                    myservicetypes = new ListCollectionView(CustomBrokerWpf.References.ServiceTypes);
                }
                return myservicetypes;
            }
        }
        private ListCollectionView mystates;
        public ListCollectionView States
        {
            get
            {
                return mystates;
            }
        }

        public override bool SaveDataChanges()
        {
            DirectoryInfo dir = new DirectoryInfo(CustomBrokerWpf.Properties.Settings.Default.DocFileRoot);
            if (!dir.Exists) dir.Create();
            bool isSuccess = true;
            if (myview != null)
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                err.AppendLine("Изменения не сохранены");
                isSuccess = this.CurrentItem == null || !(this.CurrentItem.DomainState == lib.DomainObjectState.Added || this.CurrentItem.DomainState == lib.DomainObjectState.Modified) || this.CurrentItem.Validate(true);
                if (!isSuccess)
                    err.AppendLine(this.CurrentItem.Errors);
                if (this.CurrentItem != null && this.CurrentItem.Status.Id == 50)
                {
                    foreach (RequestVM item in this.CurrentItem.ParcelRequests)
                    {
                        if (item.DomainState == lib.DomainObjectState.Added || item.DomainState == lib.DomainObjectState.Modified)
                        {
                            if (!item.Validate(true))
                            {
                                err.AppendLine(item.Errors);
                                isSuccess = false;
                            }
                            //else
                            //    err.AppendLine(item.DomainObject.UpdateDocDirPath());
                        }
                    }
                    foreach (RequestVM item in this.CurrentItem.Requests)
                    {
                        if (item.DomainState == lib.DomainObjectState.Added || item.DomainState == lib.DomainObjectState.Modified)
                        {
                            if (!item.Validate(true))
                            {
                                err.AppendLine(item.Errors);
                                isSuccess = false;
                            }
                            //else
                            //    err.AppendLine(item.DomainObject.UpdateDocDirPath());
                        }
                    }
                }
                List<Parcel> parcels = new List<Parcel>();
                foreach (Parcel item in mypdbm.Collection)
                    if (item.DomainState == lib.DomainObjectState.Added) parcels.Add(item);
                mypdbm.Errors.Clear();
                mypdbm.SaveFilter = (Parcel item) => { return item.DomainState == lib.DomainObjectState.Added; };
                if (!mypdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    err.AppendLine(mydbm.ErrorMessage);
                }
                foreach (Parcel parcel in parcels)
                {
                    try
                    {
                        if (parcel.DocDirPath != null & !Directory.Exists(dir.FullName + "\\" + parcel.DocDirPath)) dir.CreateSubdirectory(parcel.DocDirPath);
                    }
                    catch (Exception ex)
                    {
                        err.AppendLine("Сохранение изменений/n" + "Не удалось создать папку для документов Доставки " + parcel.ParcelNumberEntire + " !\n" + ex.Message);
                    }
                }
                mypdbm.Errors.Clear();
                if (!mypdbm.CheckGroup())
                    foreach (lib.DBMError erm in mypdbm.Errors)
                    {
                        err.AppendLine(erm.Message);
                        if (erm.Code != "group")
                            isSuccess = false;
                    }
                parcels.Clear();
                foreach (Parcel item in mypdbm.Collection)
                    if (item.DomainState == lib.DomainObjectState.Modified) parcels.Add(item);
                foreach (Parcel parcel in parcels)
                    if (parcel.DocDirPath != parcel.ParcelNumber)
                    {
                        try
                        {
                            DirectoryInfo parceldir = new DirectoryInfo(dir.FullName + "\\" + parcel.DocDirPath);
                            if (parceldir.Exists)
                                parceldir.MoveTo(dir.FullName + "\\" + parcel.ParcelNumber);
                            else
                                if (!Directory.Exists(dir.FullName + "\\" + parcel.ParcelNumber)) dir.CreateSubdirectory(parcel.ParcelNumber);
                            parcel.DocDirPath = parcel.ParcelNumber;
                        }
                        catch (Exception ex)
                        {
                            err.AppendLine("Сохранение изменений\nНе удалось переименовать папку для документов Доставки!\n\n" + ex.Message);
                        }
                    }
                mypdbm.Errors.Clear();
                mypdbm.SaveFilter = (Parcel item) => { return item.DomainState == lib.DomainObjectState.Modified; };
                if (!mypdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    err.AppendLine(mydbm.ErrorMessage);
                }
                if (!mypdbm.CheckGroup())
                    foreach (lib.DBMError erm in mypdbm.Errors)
                    {
                        err.AppendLine(erm.Message);
                        if (erm.Code != "group")
                            isSuccess = false;
                    }
                mypdbm.Errors.Clear();
                mypdbm.SaveFilter = (Parcel item) => { return true; };
                if (!mypdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    err.AppendLine(mydbm.ErrorMessage);
                }

                if (!isSuccess) this.PopupText = err.ToString();
            }
            return isSuccess;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return this.CurrentItem != null && base.CurrentItem?.Status.Id < 40 && base.CurrentItem?.ParcelRequests.Count > 0;
        }
        protected override bool CanRejectChanges()
        {
            return this.CurrentItem != null;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override ParcelVM CreateCurrentViewItem(lib.DomainBaseNotifyChanged domainobject)
        {
            return new ParcelVM(domainobject as Parcel);
        }
        protected override void OnCurrentItemChanged()
        {
            //this.PropertyChangedNotification("RequestAreaVisibility");
        }
        protected override void OtherViewRefresh()
        {
            CustomBrokerWpf.References.ParcelNumbers.RefreshAsinc();
            CustomBrokerWpf.References.ParcelViewCollector.RefreshViews(this.Items as IRefresh);
        }
        protected override void RefreshData(object parametr)
        {
            Parcel current = this.CurrentItem?.DomainObject;
            //mypdbm.Filter = myfilter.FilterWhereId;
            //mypdbm.FillAsyncCompleted = () =>
            //{
            //    this.Items.MoveCurrentTo(current);
            //    if (this.CurrentItem != null)
            //    {
            //        this.CurrentItem.Requests.Refresh();
            //        this.CurrentItem.ParcelRequests.Refresh();
            //    }
            //};
            if (this.CurrentItem != null)
            {
                this.CurrentItem.ParcelRequestsTotal.StopCount();
                this.CurrentItem.ParcelRequestsTotalDelivery.StopCount();
                this.CurrentItem.ParcelRequestsTotalTrade.StopCount();
                this.CurrentItem.ParcelRequestsTotalSelected.StopCount();
                this.CurrentItem.RequestsTotalSelected?.StopCount();
                this.CurrentItem.RequestsTotalSelectedDelivery?.StopCount();
                this.CurrentItem.RequestsTotalSelectedTrade?.StopCount();
            }
            CustomBrokerWpf.References.ParcelLastShipdate.Update();
            mypdbm.Filter = myfilter.FilterWhereId;
            mypdbm.Fill();
            this.Items.MoveCurrentTo(current);
            if (this.CurrentItem != null)
            {
                this.CurrentItem.Requests.Refresh();
                this.CurrentItem.ParcelRequests.Refresh();
                this.CurrentItem.DomainObject.SpecificationsRefresh();
                this.CurrentItem.ParcelRequestsTotal.StartCount();
                this.CurrentItem.ParcelRequestsTotalDelivery.StartCount();
                this.CurrentItem.ParcelRequestsTotalTrade.StartCount();
                this.CurrentItem.ParcelRequestsTotalSelected.StartCount();
                this.CurrentItem.RequestsTotalSelected?.StartCount();
                this.CurrentItem.RequestsTotalSelectedDelivery?.StartCount();
                this.CurrentItem.RequestsTotalSelectedTrade?.StartCount();
            }
            ParcelSetFilterButtonImage();
        }
        protected override void RejectChanges(object parametr)
        {
            if (myview != null)
            {
                if (myview.IsAddingNew)
                {
                    base.CurrentItem.RejectChanges();
                    base.CurrentItem.DomainState = lib.DomainObjectState.Destroyed;
                    myview.CancelNew();
                }
                else if (myview.IsEditingItem)
                {
                    base.CurrentItem.RejectChanges();
                    myview.CancelEdit();
                }
                else
                {
                    myview.EditItem(base.CurrentItem);
                    base.CurrentItem.RejectChanges();
                    myview.CommitEdit();
                }
                if (base.CurrentItem.DomainState == lib.DomainObjectState.Added)
                    myview.Remove(base.CurrentItem);
            }
        }
        protected override void SettingView()
        {
            base.SettingView();
            myview.SortDescriptions.Add(new SortDescription("ParcelNumberOrder", ListSortDirection.Descending));
            myview.MoveCurrentToFirst();
            CustomBrokerWpf.References.ParcelViewCollector.AddView(this.Items as IRefresh);
        }

        private void ParcelSetFilterButtonImage()
        {
            //string uribitmap;
            if (myfilter.isEmpty) myfilterbuttonimagepath = @"/CustomBrokerWpf;component/Images/funnel.png";
            else myfilterbuttonimagepath = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            this.PropertyChangedNotification(nameof(this.FilterButtonImagePath));
            this.PropertyChangedNotification(nameof(this.IsFiltered));
            //System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            //(ParcelFilterButton.Content as Image).Source = bi3;
        }
    }

    public class ParcelRequestsTotal : lib.TotalValues.TotalCollectionValues<Request>
    {
        public ParcelRequestsTotal(ObservableCollection<Request> requests, Parcel parcel, Importer importer = null) : base(requests)
        {
            myimporter = importer;
            myparel = parcel;
            this.Filter = (Request item) => { return item.Importer == myimporter && item.Parcel != null && !(item.DomainState == lib.DomainObjectState.Deleted || item.DomainState == lib.DomainObjectState.Destroyed); };
            this.CheckProcessing= (Request item, string property, object oldvalue) => {
                
                return
                    property == nameof(Request.Importer) || property == nameof(Request.Parcel) || property == nameof(Request.DomainState) ?
                    (
                            (property == nameof(Request.Importer)    ?  (Importer)oldvalue == myimporter : item.Importer == myimporter)
                        &&  (property == nameof(Request.Parcel)      ?  (Parcel)oldvalue == myparel      : item.Parcel == myparel)
                        &&  (property == nameof(Request.DomainState) ? !(
                                                                            (lib.DomainObjectState)oldvalue == lib.DomainObjectState.Deleted
                                                                         || (lib.DomainObjectState)oldvalue == lib.DomainObjectState.Destroyed)
                                                                    : !(
                                                                            item.DomainState == lib.DomainObjectState.Deleted
                                                                         || item.DomainState == lib.DomainObjectState.Destroyed)
                            )
                    ) != this.Filter(item)
                    : false;
            };
        }

        private Importer myimporter;
        public Importer Importer
        { get { return myimporter; } }
        private Parcel myparel;
        public Parcel Parcel
        { get { return myparel; } }

        public bool RequestCounted
        { get { return myparel.RequestsIsLoaded & !this.Processing; } }

        private decimal myactualweight;
        public decimal ActualWeight
        { get { return myactualweight; } }
        private decimal mycellnumber;
        public decimal CellNumber
        { get { return mycellnumber; } }
        public decimal DifferenceWeight
        { get { return myactualweight - myofficialweight; } }
        private decimal myinvoice;
        public decimal Invoice
        { get { return myinvoice; } }
        private decimal myinvoicediscount;
        public decimal InvoiceDiscount
        { get { return myinvoicediscount; } }
        private decimal myinvoicediscounttd;
        public decimal InvoiceDiscountTD
        {
            get { return myinvoicediscounttd; }
        }
        private decimal myinvoicediscountteo;
        public decimal InvoiceDiscountTEO
        {
            get { return myinvoicediscountteo; }
        }
        private decimal myofficialweight;
        public decimal OfficialWeight
        { get { return myofficialweight; } }
        private decimal mytransport;
        public decimal Transport
        { get { return mytransport; } }
        private decimal myvolume;
        public decimal Volume
        { get { return myvolume; } }

        protected override void Item_ValueChangedHandler(Request sender, ValueChangedEventArgs<object> e)
        {
            //decimal newvalue, oldvalue;
            switch (e.PropertyName)
            {
                //case "ActualWeight":
                //    myactualweight += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                //    PropertyChangedNotification("DifferenceWeight");
                //    break;
                //case "CellNumber":
                //    mycellnumber += (Int16)(e.NewValue ?? (Int16)0) - (Int16)(e.OldValue ?? (Int16)0);
                //    break;
                //case nameof(RequestVM.Invoice):
                //    myinvoice += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                //    break;
                //case nameof(RequestVM.InvoiceDiscount):
                //    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                //    myinvoicediscount += newvalue - oldvalue;
                //    if (sender.ServiceType == "ТД") myinvoicediscounttd += newvalue - oldvalue;
                //    if (sender.ServiceType == "ТЭО") myinvoicediscountteo += newvalue - oldvalue;
                //    PropertyChangedNotification(nameof(this.InvoiceDiscountTD));
                //    PropertyChangedNotification(nameof(this.InvoiceDiscountTEO));
                //    break;
                //case "OfficialWeight":
                //    myofficialweight += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                //    PropertyChangedNotification("DifferenceWeight");
                //    break;
                //case nameof(RequestVM.DeliveryPay):
                //    mytransport += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                //    PropertyChangedNotification("Transport");
                //    break;
                case "Volume":
                    myvolume += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                    break;
            }
            PropertyChangedNotification(e.PropertyName);
        }

        protected override void PropertiesChangedNotifycation()
        {
            //this.PropertyChangedNotification("ActualWeight");
            //this.PropertyChangedNotification("CellNumber");
            //this.PropertyChangedNotification("DifferenceWeight");
            //this.PropertyChangedNotification(nameof(this.Invoice));
            //this.PropertyChangedNotification(nameof(this.InvoiceDiscount));
            //this.PropertyChangedNotification(nameof(this.InvoiceDiscountTD));
            //this.PropertyChangedNotification(nameof(this.InvoiceDiscountTEO));
            //this.PropertyChangedNotification("OfficialWeight");
            //this.PropertyChangedNotification(nameof(this.Transport));
            this.PropertyChangedNotification("Volume");
        }

        protected override void ValuesReset()
        {
            myactualweight = 0M;
            mycellnumber = 0M;
            myinvoice = 0M;
            myinvoicediscount = 0M;
            myinvoicediscounttd = 0M;
            myinvoicediscountteo = 0M;
            myofficialweight = 0M;
            mytransport = 0M;
            myvolume = 0M;
        }
        protected override void ValuesMinus(Request item)
        {
            //myactualweight = myactualweight - (item.ActualWeight ?? 0M);
            //mycellnumber = mycellnumber - (item.CellNumber ?? 0M);
            //myinvoice = myinvoice - (item.Invoice ?? 0M);
            //myinvoicediscount = myinvoicediscount - (item.InvoiceDiscount ?? 0M);
            //if (item.ServiceType == "ТД") myinvoicediscounttd -= item.InvoiceDiscount ?? 0M;
            //if (item.ServiceType == "ТЭО") myinvoicediscountteo -= item.InvoiceDiscount ?? 0M;
            //myofficialweight = myofficialweight - (item.OfficialWeight ?? 0M);
            //mytransport = mytransport - (item.DeliveryPay ?? 0M);
            myvolume = myvolume - (item.Volume ?? 0M);
        }
        protected override void ValuesPlus(Request item)
        {
            //myactualweight = myactualweight + (item.ActualWeight ?? 0M);
            //mycellnumber = mycellnumber + (item.CellNumber ?? 0M);
            //myinvoice = myinvoice + (item.Invoice ?? 0M);
            //myinvoicediscount = myinvoicediscount + (item.InvoiceDiscount ?? 0M);
            //if (item.ServiceType == "ТД") myinvoicediscounttd += item.InvoiceDiscount ?? 0M;
            //if (item.ServiceType == "ТЭО") myinvoicediscountteo += item.InvoiceDiscount ?? 0M;
            //myofficialweight = myofficialweight + (item.OfficialWeight ?? 0M);
            //mytransport = mytransport + (item.DeliveryPay ?? 0M);
            myvolume = myvolume + (item.Volume ?? 0M);
        }
    }

    public class ParcelRequestsVMTotal : lib.TotalValues.TotalViewValues<RequestVM>
    {
        public ParcelRequestsVMTotal(ListCollectionView view, int initselected = 0, Importer importer = null) : base(view)
        {
            myinitselected = initselected;
            myimporter = importer;
            myview = view;
            myparcelgroups = new List<int>();
        }

        private ListCollectionView myview;
        private Importer myimporter;
        public Importer Importer
        { get { return myimporter; } }
        private List<int> myparcelgroups;

        private decimal myactualweight;
        public decimal ActualWeight
        { get { return myactualweight; } }
        private decimal mycellnumber;
        public decimal CellNumber
        { get { return mycellnumber; } }
        public decimal DifferenceWeight
        { get { return myactualweight - myofficialweight; } }
        private decimal myinvoice;
        public decimal Invoice
        { get { return myinvoice; } }
        private decimal myinvoicediscount;
        public decimal InvoiceDiscount
        { get { return myinvoicediscount; } }
        private decimal myinvoicediscounttd;
        public decimal InvoiceDiscountTD
        {
            get { return myinvoicediscounttd; }
        }
        private decimal myinvoicediscountteo;
        public decimal InvoiceDiscountTEO
        {
            get { return myinvoicediscountteo; }
        }
        private decimal myofficialweight;
        public decimal OfficialWeight
        { get { return myofficialweight; } }
        private decimal mytransport;
        public decimal Transport
        { get { return mytransport; } }
        private decimal myvolume;
        public decimal Volume
        { get { return myvolume; } }

        protected override void Item_ValueChangedHandler(RequestVM sender, ValueChangedEventArgs<object> e)
        {
            switch (e.PropertyName)
            {
                case "Importer":
                    if (myimporter != null && myview.Filter(sender))
                    {
                        if (e.NewValue == myimporter && e.OldValue != myimporter)
                            ValuesPlus(sender);
                        else if (e.NewValue != myimporter && e.OldValue == myimporter)
                            Minus(sender);
                        this.PropertiesChangedNotifycation();
                    }
                    break;
                default:
                    if (myimporter == null || sender.Importer == myimporter)
                    {
                        decimal newvalue, oldvalue;
                        switch (e.PropertyName)
                        {
                            case "ActualWeight":
                                myactualweight += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                                PropertyChangedNotification("DifferenceWeight");
                                break;
                            case "CellNumber":
                                mycellnumber += (Int16)(e.NewValue ?? (Int16)0) - (Int16)(e.OldValue ?? (Int16)0);
                                break;
                            case nameof(RequestVM.Invoice):
                                myinvoice += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                                break;
                            case nameof(RequestVM.InvoiceDiscount):
                                newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                myinvoicediscount += newvalue - oldvalue;
                                if (sender.ServiceType == "ТД") myinvoicediscounttd += newvalue - oldvalue;
                                if (sender.ServiceType == "ТЭО") myinvoicediscountteo += newvalue - oldvalue;
                                PropertyChangedNotification(nameof(this.InvoiceDiscountTD));
                                PropertyChangedNotification(nameof(this.InvoiceDiscountTEO));
                                break;
                            case "OfficialWeight":
                                myofficialweight += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                                PropertyChangedNotification("DifferenceWeight");
                                break;
                            case nameof(Request.AlgorithmCMD.RequestProperties.DeliveryTotal):
                                mytransport += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                                PropertyChangedNotification("Transport");
                                break;
                            case "ParcelGroup":
                                if (((int?)e.NewValue).HasValue & !((int?)e.OldValue).HasValue & myparcelgroups.Contains(((int?)e.NewValue).Value))
                                    mytransport -= (decimal)(sender.DomainObject.AlgorithmCMD?.RequestProperties.DeliveryTotal??0M);
                                break;
                            case "Volume":
                                myvolume += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                                break;
                        }
                        PropertyChangedNotification(e.PropertyName);
                    }
                    break;
            }
        }

        protected override void PropertiesChangedNotifycation()
        {
            this.PropertyChangedNotification("ActualWeight");
            this.PropertyChangedNotification("CellNumber");
            this.PropertyChangedNotification("DifferenceWeight");
            this.PropertyChangedNotification(nameof(this.Invoice));
            this.PropertyChangedNotification(nameof(this.InvoiceDiscount));
            this.PropertyChangedNotification(nameof(this.InvoiceDiscountTD));
            this.PropertyChangedNotification(nameof(this.InvoiceDiscountTEO));
            this.PropertyChangedNotification("OfficialWeight");
            this.PropertyChangedNotification(nameof(this.Transport));
            this.PropertyChangedNotification("Volume");
        }

        protected override void ValuesReset()
        {
            myactualweight = 0M;
            mycellnumber = 0M;
            myinvoice = 0M;
            myinvoicediscount = 0M;
            myinvoicediscounttd = 0M;
            myinvoicediscountteo = 0M;
            myofficialweight = 0M;
            mytransport = 0M;
            myvolume = 0M;
            myparcelgroups.Clear();
        }
        protected override void ValuesMinus(RequestVM item)
        {
            if (myimporter == null || myimporter == item.Importer)
                this.Minus(item); // из за смены импортера
        }
        protected override void ValuesPlus(RequestVM item)
        {
            if (myimporter == null || myimporter == item.Importer)
            {
                myactualweight = myactualweight + (item.DomainObject.ActualWeight ?? 0M);
                mycellnumber = mycellnumber + (item.DomainObject.CellNumber ?? 0M);
                myinvoice = myinvoice + (item.DomainObject.Invoice ?? 0M);
                myinvoicediscount = myinvoicediscount + (item.DomainObject.InvoiceDiscount ?? 0M);
                if (item.ServiceType == "ТД") myinvoicediscounttd += item.DomainObject.InvoiceDiscount ?? 0M;
                if (item.ServiceType == "ТЭО") myinvoicediscountteo += item.DomainObject.InvoiceDiscount ?? 0M;
                myofficialweight = myofficialweight + (item.DomainObject.OfficialWeight ?? 0M);
                if (!(item.ParcelGroup.HasValue && myparcelgroups.Contains(item.ParcelGroup.Value)))
                { 
                    mytransport = mytransport + (item.DomainObject.AlgorithmCMD?.RequestProperties.DeliveryTotal??0M);
                    if (item.ParcelGroup.HasValue) myparcelgroups.Add(item.ParcelGroup.Value);
                }
                myvolume = myvolume + (item.DomainObject.Volume ?? 0M);
            }
        }
        private void Minus(RequestVM item)
        {
            myactualweight = myactualweight - (item.DomainObject.ActualWeight ?? 0M);
            mycellnumber = mycellnumber - (item.DomainObject.CellNumber ?? 0M);
            myinvoice = myinvoice - (item.DomainObject.Invoice ?? 0M);
            myinvoicediscount = myinvoicediscount - (item.DomainObject.InvoiceDiscount ?? 0M);
            if (item.ServiceType == "ТД") myinvoicediscounttd -= item.DomainObject.InvoiceDiscount ?? 0M;
            if (item.ServiceType == "ТЭО") myinvoicediscountteo -= item.DomainObject.InvoiceDiscount ?? 0M;
            myofficialweight = myofficialweight - (item.DomainObject.OfficialWeight ?? 0M);
            if (!(item.ParcelGroup.HasValue && myparcelgroups.Contains(item.ParcelGroup.Value)))
            {
                mytransport = mytransport - (item.DomainObject.AlgorithmCMD.RequestProperties.DeliveryTotal);
                if (item.ParcelGroup.HasValue) myparcelgroups.Add(item.ParcelGroup.Value);
            }
            myvolume = myvolume - (item.DomainObject.Volume ?? 0M);
        }
    }
    public class ParcelTotal : INotifyPropertyChanged
    {
        internal ParcelTotal(ParcelRequestsVMTotal total, ParcelRequestsVMTotal selected)
        {
            myselected = selected;
            mytotal = total;
            this.Init();
        }

        private ParcelRequestsVMTotal myselected;
        internal ParcelRequestsVMTotal Selected
        { set { myselected = value; this.Init(); } }
        private ParcelRequestsVMTotal mytotal;
        internal ParcelRequestsVMTotal Total
        { set { mytotal = value; this.Init(); } }

        private decimal myactualweight;
        public decimal ActualWeight
        { get { return myactualweight; } }
        private decimal mycellnumber;
        public decimal CellNumber
        { get { return mycellnumber; } }
        public decimal DifferenceWeight
        { get { return myactualweight - myofficialweight; } }
        private decimal myinvoice;
        public decimal Invoice
        { get { return myinvoice; } }
        private decimal myinvoicediscount;
        public decimal InvoiceDiscount
        { get { return myinvoicediscount; } }
        private decimal myinvoicediscounttd;
        public decimal InvoiceDiscountTD
        {
            get { return myinvoicediscounttd; }
        }
        private decimal myinvoicediscountteo;
        public decimal InvoiceDiscountTEO
        {
            get { return myinvoicediscountteo; }
        }
        private decimal myofficialweight;
        public decimal OfficialWeight
        { get { return myofficialweight; } }
        private decimal mytransport;
        public decimal Transport
        { get { return mytransport; } }
        private decimal myvolume;
        public decimal Volume
        { get { return myvolume; } }

        private void Count()
        {
            myactualweight = (myselected?.ActualWeight ?? 0M) + mytotal.ActualWeight;
            mycellnumber = (myselected?.CellNumber ?? 0M) + mytotal.CellNumber;
            myinvoice = (myselected?.Invoice ?? 0M) + mytotal.Invoice;
            myinvoicediscount = (myselected?.InvoiceDiscount ?? 0M) + mytotal.InvoiceDiscount;
            myinvoicediscounttd = (myselected?.InvoiceDiscountTD ?? 0M) + mytotal.InvoiceDiscountTD;
            myinvoicediscountteo = (myselected?.InvoiceDiscountTEO ?? 0M) + mytotal.InvoiceDiscountTEO;
            myofficialweight = (myselected?.OfficialWeight ?? 0M) + mytotal.OfficialWeight;
            mytransport = (myselected?.Transport ?? 0M) + mytotal.Transport;
            myvolume = (myselected?.Volume ?? 0M) + mytotal.Volume;
        }
        private void Init()
        {
            if (mytotal != null)
            {
                Count();
                if (myselected != null) myselected.PropertyChanged += Total_PropertyChanged;
                mytotal.PropertyChanged += Total_PropertyChanged;
                PropertiesChangedNotification();
            }
        }
        private void PropertiesChangedNotification()
        {
            PropertyChangedNotification("ActualWeight");
            PropertyChangedNotification("CellNumber");
            PropertyChangedNotification("DifferenceWeight");
            PropertyChangedNotification(nameof(this.Invoice));
            PropertyChangedNotification(nameof(this.InvoiceDiscount));
            PropertyChangedNotification(nameof(this.InvoiceDiscountTD));
            PropertyChangedNotification(nameof(this.InvoiceDiscountTEO));
            PropertyChangedNotification("OfficialWeight");
            PropertyChangedNotification(nameof(this.Transport));
            PropertyChangedNotification("Volume");
        }
        private void Total_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ActualWeight":
                    myactualweight = (myselected?.ActualWeight ?? 0M) + mytotal.ActualWeight;
                    PropertyChangedNotification("ActualWeight");
                    PropertyChangedNotification("DifferenceWeight");
                    break;
                case "CellNumber":
                    mycellnumber = (myselected?.CellNumber ?? 0M) + mytotal.CellNumber;
                    PropertyChangedNotification("CellNumber");
                    break;
                case nameof(ParcelRequestsVMTotal.Invoice):
                    myinvoice = (myselected?.Invoice ?? 0M) + mytotal.Invoice;
                    PropertyChangedNotification(nameof(this.Invoice));
                    break;
                case nameof(ParcelRequestsVMTotal.InvoiceDiscount):
                    myinvoicediscount = (myselected?.InvoiceDiscount ?? 0M) + mytotal.InvoiceDiscount;
                    PropertyChangedNotification(nameof(this.InvoiceDiscount));
                    break;
                case nameof(ParcelRequestsVMTotal.InvoiceDiscountTD):
                    myinvoicediscounttd = (myselected?.InvoiceDiscountTD ?? 0M) + mytotal.InvoiceDiscountTD;
                    PropertyChangedNotification(nameof(this.InvoiceDiscountTD));
                    break;
                case nameof(ParcelRequestsVMTotal.InvoiceDiscountTEO):
                    myinvoicediscountteo = (myselected?.InvoiceDiscountTEO ?? 0M) + mytotal.InvoiceDiscountTEO;
                    PropertyChangedNotification(nameof(this.InvoiceDiscountTEO));
                    break;
                case "OfficialWeight":
                    myofficialweight = (myselected?.OfficialWeight ?? 0M) + mytotal.OfficialWeight;
                    PropertyChangedNotification("OfficialWeight");
                    PropertyChangedNotification("DifferenceWeight");
                    break;
                case nameof(ParcelRequestsVMTotal.Transport):
                    mytransport = (myselected?.Transport ?? 0M) + mytotal.Transport;
                    PropertyChangedNotification(nameof(this.Transport));
                    break;
                case "Volume":
                    myvolume = (myselected?.Volume ?? 0M) + mytotal.Volume;
                    PropertyChangedNotification("Volume");
                    break;
            }
        }

        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ParcelNumber
    {
        public int Id { set; get; }
        public int Status { set; get; }
        public string FullNumber { set; get; }
        public string Sort { set; get; }
        internal void UpdateProperties(ParcelNumber parcel)
        {
            this.Status = parcel.Status;
            this.FullNumber = parcel.FullNumber;
            this.Sort = parcel.Sort;
        }
    }

    internal class ParcelNumberDBM : lib.DBMSFill<ParcelNumber,ParcelNumber>
    {
        internal ParcelNumberDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = false;
            SelectCommandText = "SELECT * FROM parcel.FullNumber_vw ORDER BY sort DESC";
        }

        protected override ParcelNumber CreateRecord(SqlDataReader reader)
        {
            return new ParcelNumber() { Id = reader.GetInt32(0), Status = reader.GetInt32(1), FullNumber = reader.GetString(2), Sort = reader.GetString(3) };
        }
		protected override ParcelNumber CreateModel(ParcelNumber record, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			return record;
		}
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader),addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
        protected override void PrepareFill(SqlConnection addcon)
        {
        }
    }

    internal class ParcelNumberCollection : lib.ReferenceCollection<ParcelNumber>
    {
        public ParcelNumberCollection() : base(new ParcelNumberDBM())
        {
        }

        protected override int Compare(ParcelNumber item1, ParcelNumber item2)
        {
            return item1.Sort.CompareTo(item2.Sort);
        }
        protected override bool IsFirst(ParcelNumber item, string propertyName, object value)
        {
            bool isfirst = false;
            switch (propertyName)
            {
                case "Id":
                    isfirst = item.Id == (int)value;
                    break;
            }
            return isfirst;
        }
        protected override void UpdateItem(ParcelNumber olditem, ParcelNumber newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }
}
