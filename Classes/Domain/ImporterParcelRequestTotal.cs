using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class ImporterParcelRequestTotal : INotifyPropertyChanged
    {
        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        public ImporterParcelRequestTotal(Parcel parcel, Importer importer)
        { myparcel = parcel; myimporter = importer; }

        private Parcel myparcel;
        private Importer myimporter;
        public Importer Importer
        { get { return myimporter; } }
        private ObservableCollection<Request> myrequests;
        internal ObservableCollection<Request> Requests
        {
            set
            {
                myrequests = value;
                Count();
                myrequests.CollectionChanged += Requests_CollectionChanged;
            }
        }

        private void Requests_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                Count();
            else
            {
                if (e.NewItems != null)
                    foreach (Request item in e.NewItems)
                    {
                        item.ValueChanged -= Request_ValueChanged;
                        item.ValueChanged += Request_ValueChanged;
                        if (item.ParcelId == myparcel.Id && (myimporter == null || item.Importer == myimporter))
                        {
                            item.PropertyChanged -= Request_PropertyChanged;
                            item.PropertyChanged += Request_PropertyChanged;
                            if (item.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                                ValuesPlus(item);
                        }
                    }
                if (e.OldItems != null)
                    foreach (Request item in e.OldItems)
                    {
                        item.ValueChanged -= Request_ValueChanged;
                        if (item.ParcelId == myparcel.Id && (myimporter == null || item.Importer == myimporter))
                        {
                            item.PropertyChanged -= Request_PropertyChanged;
                            if (item.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                                ValuesMinus(item);
                        }
                    }
            }
        }
        private void Request_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DomainState")
            {
                Request request = sender as Request;
                if (request.ParcelId == myparcel.Id && (myimporter == null || request.Importer == myimporter))
                {
                    if (request.DomainState == DataModelClassLibrary.DomainObjectState.Deleted & request.DomainStatePrevious < DataModelClassLibrary.DomainObjectState.Deleted)
                        ValuesMinus(request);
                    else if (request.DomainStatePrevious == DataModelClassLibrary.DomainObjectState.Deleted & request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                        ValuesPlus(request);
                }
            }
        }
        //private void Request_ParcelChanged(object sender, DataModelClassLibrary.ValueChangedEventArgs<int> e)
        //{
        //    if (e.PropertyName != "ParcelId") return;
        //    if (e.NewValue == myparcel.Id && e.OldValue != myparcel.Id)
        //    {
        //        Request request = sender as Request;
        //        if (myimporter != null) request.ValueChangedObject += Request_ImporterChanged;
        //        if (myimporter == null || request.Importer == myimporter)
        //        {
        //            ValuesPlus(request);
        //            request.ValueChanged += Request_ValueChanged;
        //        }
        //    }
        //    else if (e.NewValue != myparcel.Id && e.OldValue == myparcel.Id)
        //    {
        //        Request request = sender as Request;
        //        request.ValueChangedObject -= Request_ImporterChanged;
        //        request.ValueChanged -= Request_ValueChanged;
        //        if (myimporter == null || request.Importer == myimporter)
        //            ValuesMinus(request);
        //    }
        //}
        //private void Request_ImporterChanged(object sender, DataModelClassLibrary.ValueChangedEventArgs<object> e)
        //{
        //    Request request = sender as Request;
        //    if (e.PropertyName != "Importer" || request.ParcelId!=myparcel.Id) return;
        //    if (e.NewValue==myimporter && e.OldValue!=myimporter)
        //    {
        //        ValuesPlus(request);
        //        request.ValueChanged += Request_ValueChanged;
        //    }
        //    else if(e.NewValue != myimporter && e.OldValue == myimporter)
        //    {
        //        request.ValueChanged -= Request_ValueChanged;
        //        ValuesMinus(request);
        //    }
        //}
        private void Request_ValueChanged(object sender, DataModelClassLibrary.Interfaces.ValueChangedEventArgs<object> e)
        {
            Request request = sender as Request;
            switch (e.PropertyName)
            {
                case "Importer":
                    if (request.ParcelId == myparcel.Id && myimporter != null)
                    {
                        if (e.NewValue == myimporter && e.OldValue != myimporter)
                        {
                            request.PropertyChanged += Request_PropertyChanged;
                            if (request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                                ValuesPlus(request);
                        }
                        else if (e.NewValue != myimporter && e.OldValue == myimporter)
                        {
                            request.PropertyChanged -= Request_PropertyChanged;
                            if (request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                                ValuesMinus(request);
                        }
                    }
                    break;
                case "ParcelId":
                    {
                        int newvalue = (int)(e.NewValue ?? 0), oldvalue = (int)(e.OldValue ?? 0);
                        if (myimporter == null || request.Importer == myimporter)
                        {
                            if (newvalue == myparcel.Id && oldvalue != myparcel.Id)
                            {
                                request.PropertyChanged += Request_PropertyChanged;
                                if (request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                                    ValuesPlus(request);
                            }
                            else if (newvalue != myparcel.Id && oldvalue == myparcel.Id)
                            {
                                request.PropertyChanged -= Request_PropertyChanged;
                                if (request.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                                    ValuesMinus(request);
                            }
                        }
                    }
                    break;
                default:
                    if (request.ParcelId == myparcel.Id && (myimporter == null || request.Importer == myimporter))
                    {
                        {
                            decimal newvalue, oldvalue;
                            switch (e.PropertyName)
                            {
                                case "ActualWeight":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myactualweight += newvalue - oldvalue;
                                    PropertyChangedNotification("ActualWeight");
                                    PropertyChangedNotification("DifferenceWeight");
                                    break;
                                case "CellNumber":
                                    newvalue = (short)(e.NewValue ?? (short)0); oldvalue = (short)(e.OldValue ?? (short)0);
                                    mycellnumber += newvalue - oldvalue;
                                    PropertyChangedNotification("CellNumber");
                                    break;
                                case "Invoice":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myinvoice += newvalue - oldvalue;
                                    PropertyChangedNotification("Invoice");
                                    break;
                                case "InvoiceDiscount":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myinvoicediscount += newvalue - oldvalue;
                                    PropertyChangedNotification("InvoiceDiscount");
                                    break;
                                case "OfficialWeight":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myofficialweight += newvalue - oldvalue;
                                    PropertyChangedNotification("OfficialWeight");
                                    PropertyChangedNotification("DifferenceWeight");
                                    break;
                                case "Volume":
                                    newvalue = (decimal)(e.NewValue ?? 0M); oldvalue = (decimal)(e.OldValue ?? 0M);
                                    myvolume += newvalue - oldvalue;
                                    PropertyChangedNotification("Volume");
                                    break;
                            }
                        }
                    }
                    break;
            }
        }

        private decimal myactualweight;
        public decimal ActualWeight
        {
            get { return myactualweight; }
        }
        private decimal mycellnumber;
        public decimal CellNumber
        {
            get { return mycellnumber; }
        }
        public decimal DifferenceWeight
        { get { return myactualweight - myofficialweight; } }
        private decimal myinvoice;
        public decimal Invoice
        {
            get { return myinvoice; }
        }
        private decimal myinvoicediscount;
        public decimal InvoiceDiscount
        {
            get { return myinvoicediscount; }
        }
        private decimal myofficialweight;
        public decimal OfficialWeight
        {
            get { return myofficialweight; }
        }
        private decimal myvolume;
        public decimal Volume
        {
            get { return myvolume; }
        }

        private void Count()
        {
            myactualweight = 0M;
            mycellnumber = 0M;
            myinvoice = 0M;
            myinvoicediscount = 0M;
            myofficialweight = 0M;
            myvolume = 0M;
            foreach (Request item in myrequests)
            {
                item.ValueChanged -= Request_ValueChanged;
                item.PropertyChanged -= Request_PropertyChanged;
            }
            foreach (Request item in myrequests)
            {
                item.ValueChanged += Request_ValueChanged;
                if (item.ParcelId == myparcel.Id && (myimporter == null || item.Importer == myimporter))
                {
                    item.PropertyChanged += Request_PropertyChanged;
                    if (item.DomainState < DataModelClassLibrary.DomainObjectState.Deleted)
                        ValuesPlus(item);
                }
            }
        }
        private void ValuesPlus(Request item)
        {
            myactualweight += item.ActualWeight ?? 0M;
            mycellnumber += item.CellNumber ?? 0;
            myinvoice += item.Invoice ?? 0M;
            myinvoicediscount += item.InvoiceDiscount ?? 0M;
            myofficialweight += item.OfficialWeight ?? 0M;
            myvolume += item.Volume ?? 0M;

            PropertiesChangedNotifycation();
        }
        private void ValuesMinus(Request item)
        {
            myactualweight -= item.ActualWeight ?? 0M;
            mycellnumber -= item.CellNumber ?? 0;
            myinvoice -= item.Invoice ?? 0M;
            myinvoicediscount -= item.InvoiceDiscount ?? 0M;
            myofficialweight -= item.OfficialWeight ?? 0M;
            myvolume -= item.Volume ?? 0M;

            PropertiesChangedNotifycation();
        }
        private void PropertiesChangedNotifycation()
        {
            PropertyChangedNotification("ActualWeight");
            PropertyChangedNotification("CellNumber");
            PropertyChangedNotification("DifferenceWeight");
            PropertyChangedNotification("Invoice");
            PropertyChangedNotification("InvoiceDiscount");
            PropertyChangedNotification("OfficialWeight");
            PropertyChangedNotification("Volume");
        }
    }
    
    public class ImporterParcelRequestTotalVM : INotifyPropertyChanged
    {
        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        public ImporterParcelRequestTotalVM(ImporterParcelRequestTotal total, ParcelVM parcel)
        {
            mytotal = total;
            mytotal.PropertyChanged += DomainObjectPropertyChanged;
            myparcel = parcel;
        }

        private ImporterParcelRequestTotal mytotal;
        private ParcelVM myparcel;

        private decimal myactualweightpre;
        public decimal? ActualWeight
        {
            set { myactualweightpre += value ?? 0M; PropertyChangedNotification("ActualWeight"); PropertyChangedNotification("DifferenceWeight"); }
            get { return myparcel.IsEnabled ? mytotal.ActualWeight + myactualweightpre : (decimal?)null; }
        }
        private decimal mycellnumberpre;
        public decimal? CellNumber
        {
            set { mycellnumberpre += value ?? 0M; PropertyChangedNotification("CellNumber"); }
            get { return myparcel.IsEnabled ? mytotal.CellNumber + mycellnumberpre : (decimal?)null; }
        }
        public decimal? DifferenceWeight
        { get { return myparcel.IsEnabled ? mytotal.DifferenceWeight + myactualweightpre - myofficialweightpre : (decimal?)null; } }
        private decimal myinvoicepre;
        public decimal? Invoice
        {
            set { myinvoicepre += value ?? 0M; PropertyChangedNotification("Invoice"); }
            get { return myparcel.IsEnabled ? mytotal.Invoice + myinvoicepre : (decimal?)null; }
        }
        private decimal myinvoicediscountpre;
        public decimal? InvoiceDiscount
        {
            set { myinvoicediscountpre += value ?? 0M; PropertyChangedNotification("InvoiceDiscount"); }
            get { return myparcel.IsEnabled ? mytotal.InvoiceDiscount + myinvoicediscountpre : (decimal?)null; }
        }
        private decimal myofficialweightpre;
        public decimal? OfficialWeight
        {
            set { myofficialweightpre += value ?? 0M; PropertyChangedNotification("OfficialWeight"); PropertyChangedNotification("DifferenceWeight"); }
            get { return myparcel.IsEnabled ? mytotal.OfficialWeight + myofficialweightpre : (decimal?)null; }
        }
        private decimal myvolumepre;
        public decimal? Volume
        {
            set { myvolumepre += value ?? 0M; PropertyChangedNotification("Volume"); }
            get { return myparcel.IsEnabled ? mytotal.Volume + myvolumepre : (decimal?)null; }
        }

        internal void ResetPre()
        {
            myactualweightpre = 0M;
            mycellnumberpre = 0M;
            myinvoicepre = 0M;
            myinvoicediscountpre = 0M;
            myofficialweightpre = 0M;
            myvolumepre = 0M;
            PropertyChangedNotification("ActualWeight");
            PropertyChangedNotification("CellNumber");
            PropertyChangedNotification("DifferenceWeight");
            PropertyChangedNotification("Invoice");
            PropertyChangedNotification("InvoiceDiscount");
            PropertyChangedNotification("OfficialWeight");
            PropertyChangedNotification("Volume");
        }
        private void DomainObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedNotification(e.PropertyName);
        }
    }
}
