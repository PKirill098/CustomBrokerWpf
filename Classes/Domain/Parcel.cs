﻿using Microsoft.Win32;
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

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
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
            , decimal? deliveryprice, decimal? insuranceprice, decimal? tdeliveryprice, decimal? tinsuranceprice
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

            myrater = new CurrencyRateProxy(CustomBrokerWpf.References.CurrencyRate);
            myrater.PropertyChanged += Rater_PropertyChanged;
        }
        public Parcel() : this(id: lib.NewObjectId.NewId, stamp: 0, updated: null, updater: null, domainstate: lib.DomainObjectState.Added
            , parcelnumber: null, status: CustomBrokerWpf.References.RequestStates.FindFirstItem("Name", "Загрузка"), parceltype: CustomBrokerWpf.References.ParcelTypes.FindFirstItem("Id", 2)
            , shipplandate: DateTime.Today, shipdate: null, prepared: null, crossedborder: null, terminalin: null, terminalout: null, unloaded: null
            , carrier: null, carrierperson: null, carriertel: null, declaration: null, docdirpath: null, goodstype: null
            , lorry: null, lorryregnum: null, lorrytonnage: null, lorryvolume: null, lorryvin: null
            , shipmentnumber: null, trailerregnum: null, trailervin: null, trucker: null, truckertel: null
            , deliveryprice: null, insuranceprice: null, tdeliveryprice: null, tinsuranceprice: null
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
            get { return this.ShipPlanDate.Year.ToString() + (this.ParcelNumber??"9999").PadLeft(4, '0'); }
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
            set {
                Action action = () =>
                {
                    this.UsdRate = null;
                    if (myratedate.HasValue)
                    {
                        myrater.RateDate = myratedate.Value;
                    }
                };
                SetProperty<DateTime?>(ref myratedate, value, action); }
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
                base.SetProperty<lib.ReferenceSimpleItem>(ref mystatus, value, () => { Count(); PropertiesChangedNotifycation(); });
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

        private decimal? mydeliveryprice, myinsuranceprice, mytdeliveryprice, mytinsuranceprice;
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

        private ImporterParcelRequestTotal mytotal;
        public ImporterParcelRequestTotal RequestTotal
        {
            get
            {
                if (mytotal == null)
                {
                    mytotal = new ImporterParcelRequestTotal(this, null);
                    mytotal.Requests = this.Requests;
                    mytotal.PropertyChanged += RequestTotal_PropertyChanged;
                    PropertyChangedNotification("OverVolume");
                    PropertyChangedNotification("OverWeight");
                }
                return mytotal;
            }
        }
        private ImporterParcelRequestTotal mytotald;
        public ImporterParcelRequestTotal RequestTotalD
        {
            get
            {
                if (mytotald == null)
                {
                    mytotald = new ImporterParcelRequestTotal(this, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2));
                    mytotald.Requests = this.Requests;
                    mytotald.PropertyChanged += RequestTotalDT_PropertyChanged;
                }
                return mytotald;
            }
        }
        private ImporterParcelRequestTotal mytotalt;
        public ImporterParcelRequestTotal RequestTotalT
        {
            get
            {
                if (mytotalt == null)
                {
                    mytotalt = new ImporterParcelRequestTotal(this, CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1));
                    mytotalt.Requests = this.Requests;
                    mytotalt.PropertyChanged += RequestTotalDT_PropertyChanged;

                }
                return mytotalt;
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

        public bool OverVolume
        { get { return (this.LorryVolume ?? 0M) - RequestTotal.Volume < 0; } }
        public bool OverWeight
        { get { return (this.LorryTonnage ?? 0M) - RequestTotal.ActualWeight < 0; } }
        private void RequestTotal_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
        private void RequestTotalDT_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ActualWeight":
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
                    PropertyChangedNotification("VolumeForeground");
                    break;
            }
        }

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

        private void Count()
        {
            if (this.RequestsIsNull) return;
            myactualweightfree = 0M;
            mycellnumberfree = 0M;
            myinvoicefree = 0M;
            myinvoicediscountfree = 0M;
            myofficialweightfree = 0M;
            myvolumefree = 0M;
            foreach (Request item in myrequests)
            {
                item.ValueChanged -= Request_ValueChanged;
                item.PropertyChanged -= Request_PropertyChanged;
            }
            if (mystatus.Id < 60)
                foreach (Request item in myrequests)
                {
                    item.ValueChanged += Request_ValueChanged;
                    if (!item.ParcelId.HasValue)
                    {
                        item.PropertyChanged += Request_PropertyChanged;
                        if (item.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                            ValuesPlus(item);
                    }
                }
            PropertiesChangedNotifycation();
        }
        private void ValuesPlus(Request item)
        {
            myactualweightfree += item.ActualWeight ?? 0M;
            mycellnumberfree += item.CellNumber ?? 0;
            myinvoicefree += item.Invoice ?? 0M;
            myinvoicediscountfree += item.InvoiceDiscount ?? 0M;
            myofficialweightfree += item.OfficialWeight ?? 0M;
            myvolumefree += item.Volume ?? 0M;
        }
        private void ValuesMinus(Request item)
        {
            myactualweightfree -= item.ActualWeight ?? 0M;
            mycellnumberfree -= item.CellNumber ?? 0;
            myinvoicefree -= item.Invoice ?? 0M;
            myinvoicediscountfree -= item.InvoiceDiscount ?? 0M;
            myofficialweightfree -= item.OfficialWeight ?? 0M;
            myvolumefree -= item.Volume ?? 0M;
        }
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
                    myrdbm = new RequestDBM();
                    myrdbm.Parcel = this.Id;
                    myrdbm.FillType = lib.FillType.PrefExist;
                    myrequestsloaded = false;
                    myrdbm.FillAsyncCompleted = () =>
                    {
                        if (myrdbm.Errors.Count > 0) throw new Exception(myrdbm.ErrorMessage);
                        else
                        {
                            myrdbm = null;
                            myrequests.CollectionChanged += Requests_CollectionChanged;
                            Count();
                            ForegroundNotifyChanged();
                            if (myspecifications != null)
                                for (int i = 0;i< myspecifications.Count;i++)
                                    myspecifications[i].CustomersLegalsRefresh();
                            myrequestsloaded = true;
                            this.PropertyChangedNotification(nameof(this.Requests));
                            this.PropertyChangedNotification(nameof(this.RequestsIsNull));
                            this.PropertyChangedNotification(nameof(this.RequestsIsLoaded));
                        }
                    };
                    myrdbm.FillAsync();
                    myrequests = myrdbm.Collection;
                }
                return myrequests;
            }
        }
        internal bool RequestsIsNull { get { return myrequests == null; } }
        private bool myrequestsloaded;
        internal bool RequestsIsLoaded { get { return myrequestsloaded; } }
        private void Requests_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                Count();
            else
            {
                if (e.NewItems != null)
                    foreach (Request item in e.NewItems)
                    {
                        item.ValueChanged -= Request_ValueChanged; // объект из хранилища добавлен в коллекцию повторно при обновлении
                        item.ValueChanged += Request_ValueChanged;
                        if (!item.ParcelId.HasValue) // считаем
                        {
                            item.PropertyChanged -= Request_PropertyChanged;
                            item.PropertyChanged += Request_PropertyChanged;
                            if (item.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                            { ValuesPlus(item); PropertiesChangedNotifycation(); }
                        }
                    }
                if (e.OldItems != null)
                    foreach (Request item in e.OldItems)
                    {
                        item.ValueChanged -= Request_ValueChanged;
                        if (!item.ParcelId.HasValue)
                        {
                            item.PropertyChanged -= Request_PropertyChanged;
                            if (item.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                            { ValuesMinus(item); PropertiesChangedNotifycation(); }
                        }
                    }
            }
        }
        private void Request_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DomainState")
            {
                Request request = sender as Request;
                if (!request.ParcelId.HasValue)
                {
                    if (request.DomainState == DataModelClassLibrary.DomainObjectState.Deleted & request.DomainStatePrevious < DataModelClassLibrary.DomainObjectState.Deleted)
                    { ValuesMinus(request); PropertiesChangedNotifycation(); }
                    else if (request.DomainStatePrevious == DataModelClassLibrary.DomainObjectState.Deleted & request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                    { ValuesPlus(request); PropertiesChangedNotifycation(); }
                }
            }
        }
        private void Request_ValueChanged(object sender, DataModelClassLibrary.Interfaces.ValueChangedEventArgs<object> e)
        {
            Request request = sender as Request;
            switch (e.PropertyName)
            {
                case "ParcelId":
                    {
                        int? newvalue = (int?)e.NewValue, oldvalue = (int?)e.OldValue;
                        if (!newvalue.HasValue && oldvalue.HasValue)// теперь считаем
                        {
                            request.PropertyChanged += Request_PropertyChanged;
                            if (request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                            { ValuesPlus(request); PropertiesChangedNotifycation(); }
                        }
                        else if (newvalue.HasValue && !oldvalue.HasValue) // больше не считаем
                        {
                            request.PropertyChanged -= Request_PropertyChanged;
                            if (request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                            { ValuesMinus(request); PropertiesChangedNotifycation(); }
                        }
                    }
                    break;
                default:
                    if (!request.ParcelId.HasValue)
                    {
                        {
                            decimal newvalue, oldvalue;
                            switch (e.PropertyName)
                            {
                                case "ActualWeight":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myactualweightfree += newvalue - oldvalue;
                                    PropertyChangedNotification("ActualWeightFree");
                                    PropertyChangedNotification("DifferenceWeightFree");
                                    break;
                                case "CellNumber":
                                    newvalue = (short)(e.NewValue ?? (short)0); oldvalue = (short)(e.OldValue ?? (short)0);
                                    mycellnumberfree += newvalue - oldvalue;
                                    PropertyChangedNotification("CellNumberFree");
                                    break;
                                case "Invoice":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myinvoicefree += newvalue - oldvalue;
                                    PropertyChangedNotification("InvoiceFree");
                                    break;
                                case "InvoiceDiscount":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myinvoicediscountfree += newvalue - oldvalue;
                                    PropertyChangedNotification("InvoiceDiscountFree");
                                    break;
                                case "OfficialWeight":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myofficialweightfree += newvalue - oldvalue;
                                    PropertyChangedNotification("OfficialWeightFree");
                                    PropertyChangedNotification("DifferenceWeightFree");
                                    break;
                                case "Volume":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myvolumefree += newvalue - oldvalue;
                                    PropertyChangedNotification("VolumeFree");
                                    break;
                            }
                        }
                    }
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
            myspecifications = null;
            this.PropertyChangedNotification("Specifications");
        }
        private void SpecificationLoad()
        {
            Specification.SpecificationDBM sdbm = new Specification.SpecificationDBM();
            sdbm.Parcel = this;
            sdbm.FillAsyncCompleted = () => { if (sdbm.Errors.Count > 0) throw new Exception(sdbm.ErrorMessage); };
            sdbm.Collection = myspecifications;
            sdbm.Fill();
            if (sdbm.Errors.Count > 0) System.Windows.MessageBox.Show(sdbm.ErrorMessage, "Загрузка ГТД", System.Windows.MessageBoxButton.OK,System.Windows.MessageBoxImage.Error);
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
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
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
        }
    }

    internal class ParcelStore : lib.DomainStorageLoad<Parcel, ParcelDBM>
    {
        public ParcelStore(ParcelDBM dbm) : base(dbm) { }

        protected override void UpdateProperties(Parcel olditem, Parcel newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    internal class ParcelDBM : lib.DBManagerStamp<Parcel>
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
                myinsertparams[0]
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

                ,new SqlParameter("@old",false)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                myinsertupdateparams[0]
                ,new SqlParameter("@parcelnumber", System.Data.SqlDbType.NVarChar,5) {Direction = System.Data.ParameterDirection.InputOutput}
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
            };

            myrdbm = new RequestDBM(); myrdbm.Command = new SqlCommand(); myrdbm.LegalDBM = new RequestCustomerLegalDBM();
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
        { set { myrdbm=value; } get { return myrdbm; } }
        private Specification.SpecificationDBM mysdbm;

        protected override Parcel CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            Parcel newitem = new Parcel(reader.GetInt32(0), reader.GetInt32(this.Fields["stamp"]), reader.IsDBNull(this.Fields["UpdateWho"]) ? null : reader.GetString(this.Fields["UpdateWho"]), reader.IsDBNull(this.Fields["UpdateWhen"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["UpdateWhen"]), lib.DomainObjectState.Unchanged
                , reader.GetString(this.Fields["parcelnumber"])
                , CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", reader.GetInt32(this.Fields["parcelstatus"]))
                , CustomBrokerWpf.References.ParcelTypes.FindFirstItem("Id", (int)reader.GetByte(this.Fields["parceltype"]))
                , reader.GetDateTime(this.Fields["shipplandate"])
                , reader.IsDBNull(this.Fields["shipdate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["shipdate"])
                , reader.IsDBNull(this.Fields["preparation"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["preparation"])
                , reader.IsDBNull(this.Fields["borderdate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["borderdate"])
                , reader.IsDBNull(this.Fields["terminalin"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["terminalin"])
                , reader.IsDBNull(this.Fields["terminalout"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["terminalout"])
                , reader.IsDBNull(this.Fields["unloaded"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["unloaded"])
                , reader.IsDBNull(this.Fields["carrier"]) ? null : reader.GetString(this.Fields["carrier"])
                , reader.IsDBNull(this.Fields["carrierperson"]) ? null : reader.GetString(this.Fields["carrierperson"])
                , reader.IsDBNull(this.Fields["carriertel"]) ? null : reader.GetString(this.Fields["carriertel"])
                , reader.IsDBNull(this.Fields["declaration"]) ? null : reader.GetString(this.Fields["declaration"])
                , reader.IsDBNull(this.Fields["docdirpath"]) ? null : reader.GetString(this.Fields["docdirpath"])
                , reader.IsDBNull(this.Fields["goodstype"]) ? null : CustomBrokerWpf.References.GoodsTypesParcel.FindFirstItem("Id", reader.GetInt32(this.Fields["goodstype"]))
                , reader.IsDBNull(this.Fields["lorry"]) ? null : reader.GetString(this.Fields["lorry"])
                , reader.IsDBNull(this.Fields["lorryregnum"]) ? null : reader.GetString(this.Fields["lorryregnum"])
                , reader.IsDBNull(this.Fields["lorrytonnage"]) ? (decimal?)null : reader.GetDecimal(this.Fields["lorrytonnage"])
                , reader.IsDBNull(this.Fields["lorryvolume"]) ? (decimal?)null : reader.GetDecimal(this.Fields["lorryvolume"])
                , reader.IsDBNull(this.Fields["lorryvin"]) ? null : reader.GetString(this.Fields["lorryvin"])
                , reader.IsDBNull(this.Fields["shipmentnumber"]) ? null : reader.GetString(this.Fields["shipmentnumber"])
                , reader.IsDBNull(this.Fields["trailerregnum"]) ? null : reader.GetString(this.Fields["trailerregnum"])
                , reader.IsDBNull(this.Fields["trailervin"]) ? null : reader.GetString(this.Fields["trailervin"])
                , reader.IsDBNull(this.Fields["trucker"]) ? null : reader.GetString(this.Fields["trucker"])
                , reader.IsDBNull(this.Fields["truckertel"]) ? null : reader.GetString(this.Fields["truckertel"])
                , reader.IsDBNull(this.Fields["deliveryprice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["deliveryprice"])
                , reader.IsDBNull(this.Fields["insuranceprice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["insuranceprice"])
                , reader.IsDBNull(this.Fields["tdeliveryprice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["tdeliveryprice"])
                , reader.IsDBNull(this.Fields["tinsuranceprice"]) ? (decimal?)null : reader.GetDecimal(this.Fields["tinsuranceprice"])
                , reader.IsDBNull(this.Fields["usdrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["usdrate"])
                , reader.IsDBNull(this.Fields["ratedate"]) ? (DateTime?)null : reader.GetDateTime(this.Fields["ratedate"])
                );
            newitem = CustomBrokerWpf.References.ParcelStore.UpdateItem(newitem,this.FillType==lib.FillType.Refresh);
            if (!(newitem.RequestsIsNull | this.CancelingLoad))
            {
                myrdbm.Command.Connection = addcon;
                if (mydispatcher.Thread.ManagedThreadId == System.Windows.Threading.Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
                    RequestsRefresh(newitem);
                else
                    mydispatcher.Invoke(() => { RequestsRefresh(newitem); });
            }
            return newitem;
        }

        protected override void GetOutputSpecificParametersValue(Parcel item)
        {
            if(item.DomainState==lib.DomainObjectState.Added)
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
            if (myrdbm!=null && !item.RequestsIsNull)
            {
                myrdbm.Errors.Clear();
                myrdbm.Parcel = item.Id;
                myrdbm.Collection = item.Requests;
                myrdbm.Command.Connection = this.Command.Connection;
                if (!myrdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
                }
            }
            if (!item.SpecificationsIsNull)
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
            return isSuccess;
        }
        protected override bool SaveIncludedObject(Parcel item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            if(myrdbm!=null) myrdbm.Command.Connection = this.Command.Connection;
            mysdbm.Command.Connection = this.Command.Connection;
            return true;
        }
        protected override bool SetSpecificParametersValue(Parcel item)
        {
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
                }
            }
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override void CancelLoad()
        { myrdbm.CancelingLoad = this.CancelingLoad; }

        private void RequestsRefresh(Parcel parcel)
        {
            myrdbm.Errors.Clear();
            myrdbm.Parcel = parcel.Id;
            myrdbm.FillType = this.FillType;
            //myrdbm.FillAsyncCompleted = () => { if (myrdbm.Errors.Count > 0) foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err); else foreach (Request ritem in myrdbm.Collection) if (!parcel.Requests.Contains(ritem)) parcel.Requests.Add(ritem); };
            myrdbm.Fill();
            if (myrdbm.Errors.Count > 0)
                foreach (lib.DBMError err in myrdbm.Errors) this.Errors.Add(err);
            else
            {
                foreach (Request ritem in myrdbm.Collection) { ritem.CustomerLegalsRefresh(); if (!parcel.Requests.Contains(ritem)) parcel.Requests.Add(ritem); }
            }
        }
        internal bool CheckGroup()
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
            ValidetingProperties.AddRange(new string[] { "ParcelType", "Requests", "ShipPlanDate" });
            DeleteRefreshProperties.AddRange(new string[] { "Carrier", "CarrierPerson", "CarrierTel", "CrossedBorder", "Declaration", "DocDirPath", "GoodsType", "Lorry", "LorryRegNum", "LorryTonnage", "LorryVIN", "LorryVolume", "ParcelNumber", "ParcelNumberEntire", "ParcelType", "Prepared", "RateDate", "ShipDate", "ShipPlanDate", "ShipmentNumber", "Status", "TerminalIn", "TerminalOut", "TrailerRegNum", "TrailerVIN", "Trucker", "TruckerTel", "Unloaded", "UsdRate" });
            InitProperties();
        }

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
        public string DocDirPath
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.DocDirPath, value)))
                {
                    string name = "DocDirPath";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.DocDirPath);
                    ChangingDomainProperty = name; this.DomainObject.DocDirPath = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.DocDirPath : null; }
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
                if (this.IsEnabled & this.DomainObject.OverWeight)
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
                if (this.IsEnabled & this.DomainObject.OverVolume)
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

        public string ShipPlanDateMailImage
        {
            get
            {
                string path;
                switch (this.DomainObject.MailState.ShipDate)
                {
                    case 1:
                        path = "Images/mail_1.png";
                        break;
                    case 2:
                        path = "Images/mail_3.png";
                        break;
                    default:
                        path = "Images/mail_2.png";
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
                        path = "Images/mail_1.png";
                        break;
                    case 2:
                        path = "Images/mail_3.png";
                        break;
                    default:
                        path = "Images/mail_2.png";
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
                        path = "Images/mail_1.png";
                        break;
                    case 2:
                        path = "Images/mail_3.png";
                        break;
                    default:
                        path = "Images/mail_2.png";
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
                        path = "Images/mail_1.png";
                        break;
                    case 2:
                        path = "Images/mail_3.png";
                        break;
                    default:
                        path = "Images/mail_2.png";
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
                        path = "Images/mail_1.png";
                        break;
                    case 2:
                        path = "Images/mail_3.png";
                        break;
                    default:
                        path = "Images/mail_2.png";
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
                        path = "Images/mail_1.png";
                        break;
                    case 2:
                        path = "Images/mail_3.png";
                        break;
                    default:
                        path = "Images/mail_2.png";
                        break;
                }
                return path;
            }
        }

        private ImporterParcelRequestTotalVM myrequesttotal;
        public ImporterParcelRequestTotalVM RequestTotal
        {
            get
            {
                if (myrequesttotal == null)
                    myrequesttotal = new ImporterParcelRequestTotalVM(this.DomainObject.RequestTotal, this);
                return this.IsEnabled ? myrequesttotal : null;
            }
        }
        private ImporterParcelRequestTotalVM myrequesttotald;
        public ImporterParcelRequestTotalVM RequestTotalD
        {
            get
            {
                if (myrequesttotald == null)
                    myrequesttotald = new ImporterParcelRequestTotalVM(this.DomainObject.RequestTotalD, this);
                return this.IsEnabled ? myrequesttotald : null;
            }
        }
        private ImporterParcelRequestTotalVM myrequesttotalt;
        public ImporterParcelRequestTotalVM RequestTotalT
        {
            get
            {
                if (myrequesttotalt == null)
                    myrequesttotalt = new ImporterParcelRequestTotalVM(this.DomainObject.RequestTotalT, this);
                return this.IsEnabled ? myrequesttotalt : null;
            }
        }

        public Brush ActualWeightForeground
        {
            get
            {
                Brush brush;
                if (this.IsEnabled & (RequestTotalD.ActualWeight + RequestTotalT.ActualWeight != RequestTotal.ActualWeight))
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public object OfficialWeightForeground
        {
            get
            {
                object brush;
                if (this.IsEnabled & RequestTotalD.OfficialWeight + RequestTotalT.OfficialWeight != RequestTotal.OfficialWeight)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public object CellNumberForeground
        {
            get
            {
                object brush;
                if (this.IsEnabled & RequestTotalD.CellNumber + RequestTotalT.CellNumber != RequestTotal.CellNumber)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public object InvoiceForeground
        {
            get
            {
                object brush;
                if (this.IsEnabled & RequestTotalD.Invoice + RequestTotalT.Invoice != RequestTotal.Invoice)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public object InvoiceDiscountForeground
        {
            get
            {
                object brush;
                if (this.IsEnabled & RequestTotalD.InvoiceDiscount + RequestTotalT.InvoiceDiscount != RequestTotal.InvoiceDiscount)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }
        public object VolumeForeground
        {
            get
            {
                object brush;
                if (this.IsEnabled & (RequestTotalD.Volume + RequestTotalT.Volume != RequestTotal.Volume))
                    brush = Brushes.Red;
                else
                    brush = Brushes.Black;
                return brush;
            }
        }

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

        private ParcelRequestsTotal myparcelrequeststotal;
        public ParcelRequestsTotal ParcelRequestsTotal
        { get { return myparcelrequeststotal; } }
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
                    myrequests.Filter = (object item) => { return this.Status.Id < 60 && !(item as RequestVM).DomainObject.ParcelId.HasValue && lib.ViewModelViewCommand.ViewFilterDefault(item); };
                    myrequests.SortDescriptions.Add(new SortDescription("CustomerName", ListSortDirection.Ascending));
                    myrequests.SortDescriptions.Add(new SortDescription("ParcelGroup", ListSortDirection.Ascending));
                    myrequests.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                    myrequests.MoveCurrentToPosition(-1);
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
                    myparcelrequests.Filter = (object item) => { return (item as RequestVM).DomainObject.ParcelId == this.DomainObject.Id && lib.ViewModelViewCommand.ViewFilterDefault(item); };
                    myparcelrequests.SortDescriptions.Add(new SortDescription("CustomerName", ListSortDirection.Ascending));
                    myparcelrequests.SortDescriptions.Add(new SortDescription("ParcelGroup", ListSortDirection.Ascending));
                    myparcelrequests.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                    myparcelrequests.MoveCurrentToPosition(-1);
                    myparcelrequeststotal = new ParcelRequestsTotal(myparcelrequests);
                    this.PropertyChangedNotification(nameof(this.ParcelRequestsTotal));
                    myparcelrequeststotal.FilteringProperties.Add("ParcelId");
                }
                return myparcelrequests;
            }
        }

        private Specification.SpecificationSynchronizer myssync;
        private ListCollectionView myspecifications;
        public ListCollectionView Specifications
        {
            get
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
            }
        }
        protected override void InitProperties()
        {
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
                    this.DomainObject.DocDirPath = (string)value;
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
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "ParcelType":
                    if (this.ParcelType == null)
                    {
                        errmsg = "Необходимо указать тип перевозки!";
                        isvalid = false;
                    }
                    break;
                case "Requests":
                    if (myrsync != null && this.Status?.Id==50)
                        foreach (RequestVM ritem in myrsync.ViewModelCollection)
                            if(ritem.Parcel!=null) isvalid &= ritem.Validate(inform);
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
            return false;
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

    public class ParcelCurItemCommander : lib.ViewModelCurrentItemCommand<ParcelVM>
    {
        internal ParcelCurItemCommander() : base()
        {
            myfilter = new SQLFilter("parcel", "AND");
            mypdbm = new ParcelDBM();
            mydbm = mypdbm;
            mypdbm.Filter = myfilter.FilterWhereId;
            mypdbm.FillAsyncCompleted = () => { if (mydbm.Errors.Count > 0) OpenPopup(mydbm.ErrorMessage, true); else mypdbm.FillType = lib.FillType.Refresh; SettingView(); };
            mypdbm.FillAsync();
            base.Collection = mypdbm.Collection;
            base.DeleteQuestionHeader = "Удалить перевозку?";
            
            myfolderopen = new RelayCommand(FolderOpenExec, FolderOpenCanExec);
            mysetstoreinform = new RelayCommand(SetStoreInformExec, SetStoreInformCanExec);
            mymovespecification = new RelayCommand(MoveSpecificationExec, MoveSpecificationCanExec);
            mycreateexcelreport = new RelayCommand(CreateExcelReportExec, CreateExcelReportCanExec);
            mysendmail = new RelayCommand(SendMailExec, SendMailCanExec);
            myspecfolderopen = new RelayCommand(SpecFolderOpenExec, SpecFolderOpenCanExec);
            myspecadd = new RelayCommand(SpecAddExec, SpecAddCanExec);
            myspecdel = new RelayCommand(SpecDelExec, SpecDelCanExec);
            mytdload = new RelayCommand(TDLoadExec, TDLoadCanExec);
        }

        ParcelDBM mypdbm;
        private SQLFilter myfilter;
        internal SQLFilter Filter
        {
            set
            {
                if (!SaveDataChanges())
                    this.OpenPopup("Применение фильтра\nПрименение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", true);
                else
                {
                    myfilter.RemoveCurrentWhere();
                    myfilter = value;
                    this.Refresh.Execute(null);
                }
            }
            get { return myfilter; }
        }

        private RelayCommand myfolderopen;
        public ICommand FolderOpen
        {
            get { return myfolderopen; }
        }
        private void FolderOpenExec(object parametr)
        {
            try
            {
                if (this.CurrentItem != null)
                {
                    string path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + "Отправки\\" + this.CurrentItem.DocDirPath??string.Empty;
                    if (!Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    System.Diagnostics.Process.Start(path);
                    //else if (Directory.Exists("E:\\Счета\\" + prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5)))
                    //{
                    //    prow.docdirpath = prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5);
                    //    prow.EndEdit();
                    //    System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
                    //}
                    //else
                    //{
                    //    if (MessageBox.Show("Не удалось найти папку отправки: E:\\Счета\\" + prow.docdirpath + "\nСоздать папку?", "Папка документов", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    //    {
                    //        System.IO.Directory.CreateDirectory("E:\\Счета\\" + prow.docdirpath);
                    //        System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                this.OpenPopup("Папка документов\n" + ex.Message, true);
            }
        }
        private bool FolderOpenCanExec(object parametr)
        { return true; }

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
                this.CurrentItem.ParcelRequests.CommitEdit();
                foreach (RequestVM item in this.CurrentItem.ParcelRequests)
                {
                    if (!item.StoreInform.HasValue)
                    {
                        item.StoreInform = DateTime.Today;
                    }
                }
                this.CurrentItem.ParcelRequests.CommitEdit();
            }
            else
                this.OpenPopup("Не удалось применить изменения! Проверте корректность и полноту данных.", true);
        }
        private bool SetStoreInformCanExec(object parametr)
        { return this.CurrentItem != null; }

        private RelayCommand mymovespecification;
        public ICommand MoveSpecification
        {
            get { return mymovespecification; }
        }
        private void MoveSpecificationExec(object parametr)
        {
            if (this.EndEdit() && this.SaveDataChanges() && this.CurrentItem != null && this.CurrentItem.ParcelType.Id == 1)
            {
                FileInfo[] files;
                DirectoryInfo dirIn = new DirectoryInfo(@"V:\Отправки");
                if (dirIn.Exists)
                {
                    if (dirIn.GetDirectories(this.CurrentItem.ParcelNumber + "_*").Length > 0)
                    {
                        dirIn = dirIn.GetDirectories(this.CurrentItem.ParcelNumber + "_*")[0];
                        DirectoryInfo dirOut = new DirectoryInfo(@"V:\Спецификации");
                        if (dirOut.Exists)
                        {
                            foreach (Classes.Domain.RequestVM row in this.CurrentItem.ParcelRequests)
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
                        this.OpenPopup("Перенос спецификаций\n" + @"Папка 'V:\Отправки\" + this.CurrentItem.ParcelNumber + "_...' не найдена!", true);
                }
                else
                    this.OpenPopup("Перенос спецификаций\n" + @"Папка 'V:\Отправки' не найдена!", true);
            }
        }
        private bool MoveSpecificationCanExec(object parametr)
        { return this.CurrentItem?.ParcelType?.Id == 1; }

        private RelayCommand mycreateexcelreport;
        public ICommand CreateExcelReport
        {
            get { return mycreateexcelreport; }
        }
        private void CreateExcelReportExec(object parametr)
        {
            bool isNew;
            if (this.CurrentItem != null && parametr is bool)
            {
                isNew = (bool)parametr;
                ExcelReport(null, isNew);
                ExcelReport(1, isNew);
                ExcelReport(2, isNew);
            }
        }
        private bool CreateExcelReportCanExec(object parametr)
        { return this.CurrentItem != null; }
        private void ExcelReport(int? importerid, bool isNew)
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
                exWh.Name = this.CurrentItem.ParcelNumberEntire;
                exWh.Cells[1, 1] = "Позиция по складу"; exWh.Cells[1, 2] = "Дата поступления"; exWh.Cells[1, 3] = "Группа загрузки"; exWh.Cells[1, 4] = "Клиент"; exWh.Cells[1, 5] = "Юр. лица"; exWh.Cells[1, 6] = "Поставщик"; exWh.Cells[1, 7] = "Импортер"; exWh.Cells[1, 8] = "Группа менеджеров";
                exWh.Cells[1, 9] = "Кол-во мест"; exWh.Cells[1, 10] = "Вес по док, кг"; exWh.Cells[1, 11] = "Вес факт, кг"; exWh.Cells[1, 12] = "Объем, м3"; exWh.Cells[1, 13] = "Инвойс"; exWh.Cells[1, 14] = "Инвойс, cо скидкой"; exWh.Cells[1, 15] = "Услуга"; exWh.Cells[1, 16] = "Примечание менеджера";
                r = exWh.Columns[9, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[10, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[11, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[12, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[13, Type.Missing]; r.NumberFormat = "#,##0.00";
                r = exWh.Columns[14, Type.Missing]; r.NumberFormat = "#,##0.00";
                foreach (Classes.Domain.RequestVM itemRow in this.CurrentItem.ParcelRequests)
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
                    string filename = Path.Combine(CustomBrokerWpf.Properties.Settings.Default.DocFileRoot, "Отправки", this.CurrentItem.DocDirPath, this.CurrentItem.Lorry + " - " + (importerid == 1 ? "Трейд" : (importerid == 2 ? "Деливери" : string.Empty)) + ".xlsx");
                    if (File.Exists(filename))
                        File.Delete(filename);
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
                this.OpenPopup("Создание заявки/n" + ex.Message, true);
            }
            finally
            {
                exApp = null;
                if (exAppProt != null && exAppProt.Workbooks.Count == 0) exAppProt.Quit();
                exAppProt = null;
            }
        }

        private RelayCommand mysendmail;
        public ICommand SendMail
        {
            get { return mysendmail; }
        }
        private void SendMailExec(object parametr)
        {
            if (parametr != null)
            {
                bool iserr = false;
                int state = int.Parse((string)parametr);
                this.CurrentItem.DomainObject.MailState.Send(state);
                if (this.CurrentItem.DomainObject.MailState.SendErrors.Count > 0)
                {
                    System.Text.StringBuilder text = new System.Text.StringBuilder();
                    foreach (lib.DBMError err in this.CurrentItem.DomainObject.MailState.SendErrors)
                    {
                        text.AppendLine(err.Message);
                        iserr |= !string.Equals(err.Code, "0");
                    }
                    if (iserr) { text.Insert(0, "Отправка выполнена с ошибкой!\n"); }
                    this.OpenPopup(text.ToString(), iserr);
                }

            }
        }
        private bool SendMailCanExec(object parametr)
        { return this.CurrentItem != null; }

        private RelayCommand myspecfolderopen;
        public ICommand SpecFolderOpen
        {
            get { return myspecfolderopen; }
        }
        private void SpecFolderOpenExec(object parametr)
        {
            try
            {
                if (this.CurrentItem != null)
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
        private bool SpecFolderOpenCanExec(object parametr)
        { return true; }

        private lib.TaskAsync.TaskAsync myexceltask;
        private RelayCommand myspecadd;
        public ICommand SpecAdd
        {
            get { return myspecadd; }
        }
        private void SpecAddExec(object parametr)
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
                            Specification.Specification spec = this.CurrentItem.DomainObject.Specifications.FirstOrDefault<Specification.Specification>((Specification.Specification item) => { return item.Consolidate == request.Consolidate && item.ParcelGroup == (string.IsNullOrEmpty(request.Consolidate) ? request.ParcelGroup : null) && item.Request == (string.IsNullOrEmpty(request.Consolidate) & !request.ParcelGroup.HasValue ? request.DomainObject : null); });
                            if (spec != null)
                            {
                                if (System.Windows.MessageBox.Show("Такая спецификация уже есть. Перезаписать?", "Загрузка спецификации", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.No)
                                    return;
                                else
                                {
                                    if (spec.DetailsIsNull)
                                    {
                                        Specification.SpecificationDetailDBM sdbm = new Specification.SpecificationDetailDBM() { Specification = spec };
                                        sdbm.Fill();
                                        foreach (Specification.SpecificationDetail item in sdbm.Collection)
                                            item.DomainState = lib.DomainObjectState.Deleted;
                                        if (!sdbm.SaveCollectionChanches())
                                        {
                                            this.OpenPopup(sdbm.ErrorMessage, true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Specification.SpecificationVM specvm = null;
                                        foreach (Specification.SpecificationVM vm in this.CurrentItem.Specifications)
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
                                    parcel:this.CurrentItem.DomainObject,
                                    consolidate:request.Consolidate,
                                    parcelgroup:string.IsNullOrEmpty(request.Consolidate) ? request.ParcelGroup : null,
                                    request:string.IsNullOrEmpty(request.Consolidate) & !request.ParcelGroup.HasValue ? request.DomainObject : null,
                                    agent:CustomBrokerWpf.References.AgentStore.GetItemLoad(request.AgentId??0, out _),
                                    importer:request.Importer);
                                spec.CustomersLegalsRefresh();
                                this.CurrentItem.Specifications.AddNewItem(new Specification.SpecificationVM(spec));
                                this.CurrentItem.Specifications.CommitNew();
                            }
                            if(string.IsNullOrEmpty(spec.FilePath)) spec.BuildFileName(fd.FileName);
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
        private bool SpecAddCanExec(object parametr)
        { return this.CurrentItem != null & (myexceltask == null || !myexceltask.IsBusy); }
        private KeyValuePair<bool, string> OnExcelImport(object parm)
        {
			int maxr, usedr = 0, r = 10;
            decimal v;
            object[] param = parm as object[];
            string filepath = (string)param[0];
            Specification.Specification spec = (Specification.Specification)param[1];
            Specification.SpecificationDetail detail;
            CustomerLegal legal = spec.CustomerLegalsList?.Count == 1 ? spec.CustomerLegalsList[0] : null;

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
                myexceltask.ProgressChange(5);

                for (; r <= maxr; r++)
                {
                    if (string.IsNullOrEmpty(exWh.Cells[r, 6].Text as string)) continue;
                    detail = new Specification.SpecificationDetail();

                    if (int.TryParse(exWh.Cells[r, 13].Text as string,out int n) && n < 0 | n > 10000000)
                        throw new Exception("Некорректное значение количества товара: " + exWh.Cells[r, 6].Text);
                    else
                        detail.Amount = n;
                    detail.Branch = exWh.Cells[r, 10].Text;
                    detail.Brand = exWh.Cells[r, 11].Text;
                    detail.CellNumber = (exWh.Cells[r, 16].Value)?.ToString();
                    detail.Certificate = exWh.Cells[r, 22].Text;
                    detail.Contexture = exWh.Cells[r, 5].Text;
                    if (decimal.TryParse(exWh.Cells[r, 19].Value.ToString(), out v) && v < 0)
                        throw new Exception("Некорректное значение стоимости товара: " + exWh.Cells[r, 19].Value.ToString());
                    else
                        detail.Cost = v;
                    detail.CountryEN = exWh.Cells[r, 21].Text;
                    detail.CountryRU = exWh.Cells[r, 20].Text;
                    detail.Customer = exWh.Cells[r, 28].Text;
                    detail.Description = exWh.Cells[r, 6].Text;
                    detail.Gender = exWh.Cells[r, 4].Text;
                    detail.GrossWeight = (decimal?)exWh.Cells[r, 15].Value;
                    detail.Name = exWh.Cells[r, 3].Text;
                    detail.NetWeight = (decimal?)exWh.Cells[r, 14].Value;
                    detail.Note = exWh.Cells[r, 24].Text;
                    detail.Packing = exWh.Cells[r, 17].Text;
                    detail.Price = (decimal?)exWh.Cells[r, 18].Value;
                    detail.Producer = exWh.Cells[r, 13].Text;
                    detail.RowOrder = r - 10;
                    detail.SizeEN = exWh.Cells[r, 7].Text;
                    detail.SizeRU = exWh.Cells[r, 8].Text;
                    detail.Specification = spec;
                    detail.TNVED = (exWh.Cells[r, 12].Value)?.ToString();
                    detail.VendorCode = (exWh.Cells[r, 9].Value)?.ToString();
                    if((exWh.Cells[r, 29].Value)?.ToString().Length > 0)
					{
                        //Request request = CustomBrokerWpf.References.RequestStore.GetItemLoad((exWh.Cells[r, 29].Value)?.ToString(), out List<lib.DBMError> errors);
                        Request request = spec.Requests.FirstOrDefault((Request req) => { return req.StorePoint == (exWh.Cells[r, 29].Value)?.ToString(); });
                        //if (errors.Count>0)
                        //    throw new Exception(errors[0].Message);
                        //else 
                        if (request == null)
                            throw new Exception("Позиция по складу " + (exWh.Cells[r, 29].Value)?.ToString() + " не соответствует ни одной заявке в разбивке!");
                        //    throw new Exception("Не найдена заявка с позицией по складу " + (exWh.Cells[r, 29].Value)?.ToString());
                        //else if(request.Parcel != spec.Parcel)
                        //    throw new Exception("Заявка с позицией по складу " + (exWh.Cells[r, 29].Value)?.ToString() + " не найдена в заявках разбивки!");
                        else
                            detail.Request = request;
                    }
                    detail.Client = legal;
                    App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action<Specification.SpecificationDetail>(spec.Details.Add), detail);
                    //return new KeyValuePair<bool, string>(true, "Сертификат " + sert + " (ячейка Excel " + exWh.Cells[r, 1].Address(false, false) + ") не найден!");
                    //double color = exWh.Cells[r, 1].Interior.Color ?? 16777215D;
                    //if (color != 16777215D) findbranchcnt.Goods.ColorMark = lib.Common.MsOfficeHelper.OfficeColorToString(color);
                    usedr++;
                    myexceltask.ProgressChange(r, maxr, 0.85M, 0.15M);
                }
                myexceltask.ProgressChange(99);
                spec.RefreshTotalDetails();
                exWb.Close();
                exApp.Quit();

                if (spec.Request != null)
                    spec.Request.IsSpecification = true;
                else if (spec.Parcel != null)
                    foreach (Request req in spec.Parcel.Requests)
                    {
                        if (!string.IsNullOrEmpty(spec.Consolidate))
                        {
                            if (req.Consolidate == spec.Consolidate)
                                req.IsSpecification = true;
                        }
                        else if (spec.ParcelGroup.HasValue)
                        {
                            if (req.ParcelGroup == spec.ParcelGroup)
                                req.IsSpecification = true;
                        }
                    }

                myexceltask.ProgressChange(100);
                return new KeyValuePair<bool, string>(false, "Разбивка загружена. " + usedr.ToString() + " строк обработано.");
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

        private RelayCommand myspecdel;
        public ICommand SpecDel
        {
            get { return myspecdel; }
        }
        private void SpecDelExec(object parametr)
        {
            Specification.SpecificationVM item = this.CurrentItem.Specifications.CurrentItem as Specification.SpecificationVM;
            this.CurrentItem.Specifications.EditItem(item);
            item.DomainState = lib.DomainObjectState.Deleted;
            this.CurrentItem.Specifications.CommitEdit();
        }
        private bool SpecDelCanExec(object parametr)
        { return false/* this.CurrentItem != null && this.CurrentItem.Specifications.CurrentItem != null*/; }

        private RelayCommand mytdload;
        public ICommand TDLoad
        {
            get { return mytdload; }
        }
        private void TDLoadExec(object parametr)
        {
            if (parametr is Specification.SpecificationVM)
            {
                OpenFileDialog fd = new OpenFileDialog();
                fd.Multiselect = false;
                fd.CheckPathExists = true;
                fd.CheckFileExists = true;
                fd.Title = "Выбор файла декларации";
                fd.Filter = "Файлы XML|*.xml;";
                if (fd.ShowDialog().Value)
                {
                    Specification.Specification spec = (parametr as Specification.SpecificationVM).DomainObject;
                    Specification.Declaration decl = new Specification.Declaration();
                    if (spec.Declaration?.TotalSum != null)
                        if (System.Windows.MessageBox.Show("Таможенная декларация уже загружена. Перезаписать?", "Загрузка ТД", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.No)
                            return;
                        else
                            decl = spec.Declaration;
                    else
                    { decl = new Specification.Declaration(); spec.Declaration = decl; }

                    string err = decl.LoadDeclaration(fd.FileName);
                    if (string.IsNullOrEmpty(err))
                        this.OpenPopup("ТД загружена!", false);
                    else
                        this.OpenPopup("НЕ удалось разобрать структуру файла ТД!/n" + err, true);
                }
            }
        }
        private bool TDLoadCanExec(object parametr)
        { return true; }

        private ListCollectionView mystates;
        public ListCollectionView States
        {
            get
            {
                if (mystates == null)
                {
                    mystates = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
                    mystates.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                    mystates.Filter = (object item) => { return (item as lib.ReferenceSimpleItem).Id > 49; };
                }
                return mystates;
            }
        }
        private ListCollectionView myrequeststates;
        public ListCollectionView RequestStates
        {
            get
            {
                if (myrequeststates == null)
                {
                    myrequeststates = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
                    myrequeststates.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Ascending));
                    myrequeststates.Filter = (object item) => { return (item as lib.ReferenceSimpleItem).Id < 50; };
                }
                return myrequeststates;
            }
        }
        private ListCollectionView mygoodstypes;
        public ListCollectionView GoodsTypes
        {
            get
            {
                if (mygoodstypes == null)
                {
                    mygoodstypes = new ListCollectionView(CustomBrokerWpf.References.GoodsTypesParcel);
                    mygoodstypes.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return mygoodstypes;
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
        private ListCollectionView myagents;
        public ListCollectionView Agents
        {
            get
            {
                if (myagents == null)
                {
                    myagents = new ListCollectionView(CustomBrokerWpf.References.AgentNames);
                    myagents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return myagents;
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

        public override bool SaveDataChanges()
        {
            DirectoryInfo dir = new DirectoryInfo(CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + "Отправки\\");
            if (!dir.Exists) dir.Create();
            bool isSuccess = true;
            if (myview != null)
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                err.AppendLine("Изменения не сохранены");
                isSuccess = this.CurrentItem == null || !(this.CurrentItem.DomainState == lib.DomainObjectState.Added || this.CurrentItem.DomainState == lib.DomainObjectState.Modified) || this.CurrentItem.Validate(true);
                if (!isSuccess)
                    err.AppendLine(this.CurrentItem.Errors);
                if (this.CurrentItem != null && this.CurrentItem.Status.Id==50)
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
                            else
                                err.AppendLine(item.DomainObject.UpdateDocDirPath());
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
                            else
                                err.AppendLine(item.DomainObject.UpdateDocDirPath());
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
                    if (parcel.DocDirPath != parcel.ParcelNumberEntire)
                    {
                        try
                        {
                            DirectoryInfo parceldir = new DirectoryInfo(dir.FullName + "\\" + parcel.DocDirPath);
                            if (parceldir.Exists)
                                parceldir.MoveTo(dir.FullName + "\\" + parcel.ParcelNumberEntire);
                            else
                                if (!Directory.Exists(dir.FullName + "\\" + parcel.ParcelNumber)) dir.CreateSubdirectory(parcel.ParcelNumber);
                            parcel.DocDirPath = parcel.ParcelNumberEntire;
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

        }
        protected override void RefreshData(object parametr)
        {
            Parcel current = this.CurrentItem?.DomainObject;
            mypdbm.Filter = myfilter.FilterWhereId;
            //mypdbm.FillAsyncCompleted = () =>
            //{
            //    this.Items.MoveCurrentTo(current);
            //    if (this.CurrentItem != null)
            //    {
            //        this.CurrentItem.Requests.Refresh();
            //        this.CurrentItem.ParcelRequests.Refresh();
            //    }
            //};
            mypdbm.Fill();
            this.Items.MoveCurrentTo(current);
            if (this.CurrentItem != null)
            {
                this.CurrentItem.Requests.Refresh();
                this.CurrentItem.ParcelRequests.Refresh();
                this.CurrentItem.DomainObject.SpecificationsRefresh();
            }
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
        }
    }

    public class ParcelRequestsTotal : lib.TotalCollectionValues<RequestVM>
    {
        public ParcelRequestsTotal(ListCollectionView view) : base(view) { }

        private decimal mycellnumber;
        public decimal CellNumber
        { get { return mycellnumber; } }
        private decimal myactualweight;
        public decimal ActualWeight
        { get { return myactualweight; } }
        private decimal myofficialweight;
        public decimal OfficialWeight
        { get { return myofficialweight; } }
        public decimal DifferenceWeight
        { get { return myactualweight - myofficialweight; } }
        private decimal myinvoicediscount;
        public decimal InvoiceDiscount
        { get { return myinvoicediscount; } }
        private decimal myvolume;
        public decimal Volume
        { get { return myvolume; } }


        protected override void Item_ValueChangedHandler(RequestVM sender, ValueChangedEventArgs<object> e)
        {
            //decimal oldvalue = (decimal)(e.OldValue ?? 0M), newvalue = (decimal)(e.NewValue ?? 0M);
            switch (e.PropertyName)
            {
                case "CellNumber":
                    mycellnumber += (Int16)(e.NewValue ?? (Int16)0) - (Int16)(e.OldValue ?? (Int16)0);
                    PropertyChangedNotification("CellNumber");
                    break;
                case "ActualWeight":
                    myactualweight += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                    PropertyChangedNotification("ActualWeight");
                    PropertyChangedNotification("DifferenceWeight");
                    break;
                case "OfficialWeight":
                    myofficialweight += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                    PropertyChangedNotification("OfficialWeight");
                    PropertyChangedNotification("DifferenceWeight");
                    break;
                case "InvoiceDiscount":
                    myinvoicediscount += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                    PropertyChangedNotification("InvoiceDiscount");
                    break;
                case "Volume":
                    myvolume += (decimal)(e.NewValue ?? 0M) - (decimal)(e.OldValue ?? 0M);
                    PropertyChangedNotification("Volume");
                    break;
            }
        }

        protected override void PropertiesChangedNotifycation()
        {
            this.PropertyChangedNotification("CellNumber");
            this.PropertyChangedNotification("ActualWeight");
            this.PropertyChangedNotification("OfficialWeight");
            this.PropertyChangedNotification("DifferenceWeight");
            this.PropertyChangedNotification("InvoiceDiscount");
            this.PropertyChangedNotification("Volume");
        }

        protected override void ValuesReset()
        {
            mycellnumber = 0M;
            myactualweight = 0M;
            myofficialweight = 0M;
            myinvoicediscount = 0M;
            myvolume = 0M;
        }
        protected override void ValuesMinus(RequestVM item)
        {
            mycellnumber = mycellnumber - (item.DomainObject.CellNumber ?? 0M);
            myactualweight = myactualweight - (item.DomainObject.ActualWeight ?? 0M);
            myofficialweight = myofficialweight - (item.DomainObject.OfficialWeight ?? 0M);
            myinvoicediscount = myinvoicediscount - (item.DomainObject.InvoiceDiscount ?? 0M);
            myvolume = myvolume - (item.DomainObject.Volume ?? 0M);
            //base.ValuesMinus(item);
        }
        protected override void ValuesPlus(RequestVM item)
        {
            mycellnumber = mycellnumber + (item.DomainObject.CellNumber ?? 0M);
            myactualweight = myactualweight + (item.DomainObject.ActualWeight ?? 0M);
            myofficialweight = myofficialweight + (item.DomainObject.OfficialWeight ?? 0M);
            myinvoicediscount = myinvoicediscount + (item.DomainObject.InvoiceDiscount ?? 0M);
            myvolume = myvolume + (item.DomainObject.Volume ?? 0M);
            //base.ValuesPlus(item);
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

    internal class ParcelNumberDBM : lib.DBMSFill<ParcelNumber>
    {
        internal ParcelNumberDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = false;
            SelectCommandText = "SELECT * FROM parcel.FullNumber_vw ORDER BY sort DESC";
        }

        protected override ParcelNumber CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new ParcelNumber() { Id=reader.GetInt32(0),Status=reader.GetInt32(1),FullNumber=reader.GetString(2),Sort=reader.GetString(3)};
        }
        protected override void CancelLoad()
        {
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
