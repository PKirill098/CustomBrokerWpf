﻿using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using excel = Microsoft.Office.Interop.Excel;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class MainWindow : Window, ISQLFiltredWindow, INotifyPropertyChanged, lib.Interfaces.IMainWindow
    {
        #region MainWindow
        private Classes.Domain.EventLogVM mylogvm;
        private List<Window> mychildwindows = new List<Window>();
        public List<Window> ListChildWindow
        { get { return mychildwindows; } }
        public MainWindow()
        {
            InitializeComponent();

            //isRequestSave = true;
            //ritemcmd = new System.Collections.Generic.List<Classes.Domain.RequestItemViewCommand>();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.EventLogTab.Visibility = (this.FindResource("keyVisibilityTopManagers") as VisibilityTopManagers).Visibility;
            if ((this.FindResource("keyVisibilityTopManagers") as VisibilityTopManagers).IsMember)
            {
                mylogvm = new Classes.Domain.EventLogVM();
                EventLogGrid.DataContext = mylogvm;
                mylogvm.EndEdit = myEndEdit;
                mylogvm.CancelEdit = myCancelEdit;
                SetFilterButtonImage();
            }
            Request_Loaded();
        }

        private void OpenSingleWindow(Type winClass, string winName)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == winName) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                System.Reflection.ConstructorInfo info = winClass.GetConstructor(System.Type.EmptyTypes);
                ObjectWin = (Window)info.Invoke(null);
                ObjectWin.Owner = this;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemAddressType_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winAddressType") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new AddressTypeWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemAgent_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winAgentList") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new AgentListWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemBrand_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "itemBrandWin") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new BrandWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemColor_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winColor") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new Windows.Specification.ColorWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemContactType_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winContactType") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ContactTypeWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemCountries_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winCountries") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CountriesWin();
                mychildwindows.Add(ObjectWin);
                IViewModelWindous win = ObjectWin as IViewModelWindous;
                Classes.Domain.References.CountriesVM vm = new Classes.Domain.References.CountriesVM();
                vm.EndEdit = win.vmEndEdit;
                vm.CancelEdit = win.vmCancelEdit;
                ObjectWin.DataContext = vm;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemDeliveryType_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winDeliveryType") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new DeliveryTypeWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemMailTemplate_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winMailTemplate") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new MailTemplateWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemManagers_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winManagers") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ManagersWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemManagGroup_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winGroupMng") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new GroupMngWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
            //OpenSingleWindow(GroupMngWin.NameProperty, "winGroupMng");
        }
		private void MenuItemMarking_Click(object sender, RoutedEventArgs e)
		{
            Window ObjectWin = null;
            //foreach (Window item in mychildwindows)
            //{
            //    if (item.Name == "winGoods") ObjectWin = item;
            //}
            //if (ObjectWin == null)
            //{
                ObjectWin = new MarkingWin();
                ObjectWin.DataContext = new Classes.Domain.Marking.MarkingViewCommader();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            //}
            //else
            //{
            //    ObjectWin.Activate();
            //    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            //}
        }
        private void MenuItemPaymentType_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winPaymentType") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new PaymentTypeWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemRequestStatus_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winRequestStatus") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new RequestStatusWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemTown_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winTown") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new TownWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        //private void MenuItemExpenditureItem_Click(object sender, RoutedEventArgs e)
        //{
        //    Window ObjectWin = null;
        //    foreach (Window item in mychildwindows)
        //    {
        //        if (item.Name == "winExpenditureItem") ObjectWin = item;
        //    }
        //    if (ObjectWin == null)
        //    {
        //        ObjectWin = new ExpenditureItemWin();
        //        mychildwindows.Add(ObjectWin);
        //        ObjectWin.Show();
        //    }
        //    else
        //    {
        //        ObjectWin.Activate();
        //        if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
        //    }
        //}
        //private void MenuItemExpenditureType_Click(object sender, RoutedEventArgs e)
        //{
        //    Window ObjectWin = null;
        //    foreach (Window item in mychildwindows)
        //    {
        //        if (item.Name == "winExpenditureType") ObjectWin = item;
        //    }
        //    if (ObjectWin == null)
        //    {
        //        ObjectWin = new ExpenditureTypeWin();
        //        mychildwindows.Add(ObjectWin);
        //        ObjectWin.Show();
        //    }
        //    else
        //    {
        //        ObjectWin.Activate();
        //        if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
        //    }
        //}
        private void MenuItemGoodsType_Click(object sender, RoutedEventArgs e)
        {
            string wintitle = "Вид груза";
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winDictionary" && item.Title == wintitle) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                lib.ReferenceCollectionSimpleItemVM vm = new lib.ReferenceCollectionSimpleItemVM(CustomBrokerWpf.References.GoodsTypesParcel,wintitle,"Удалить выбранные виды груза?");
                ObjectWin = new DictionaryWin();
                ObjectWin.Icon = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri(@"pack://application:,,,/Images\weight2.png", UriKind.Absolute));
                ObjectWin.DataContext = vm;
                //ObjectWin.Title = "Вид груза";
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemStore_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winWarehouses") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new WarehousesWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemStoreAddressType_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winReferenceSympleItem" && item.Title == "Склад, тип адреса") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ReferenceSympleItemWin();
                ObjectWin.Title = "Склад, тип адреса";
                ObjectWin.Icon = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri("pack://application:,,,/CustomBrokerWpf;component/Images/forklifter.png"));
                (ObjectWin as ReferenceSympleItemWin).CanAddRows = true;
                (ObjectWin as ReferenceSympleItemWin).CanDeleteRows = true;
                (ObjectWin as ReferenceSympleItemWin).SetDataContext(CustomBrokerWpf.References.StoreAddressTypes, false);
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemStoreContactType_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winReferenceSympleItem" && item.Title == "Склад, тип контакта") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ReferenceSympleItemWin();
                ObjectWin.Title = "Склад, тип контакта";
                ObjectWin.Icon = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri("pack://application:,,,/CustomBrokerWpf;component/Images/forklifter.png"));
                (ObjectWin as ReferenceSympleItemWin).CanAddRows = true;
                (ObjectWin as ReferenceSympleItemWin).CanDeleteRows = true;
                (ObjectWin as ReferenceSympleItemWin).SetDataContext(CustomBrokerWpf.References.StoreContactTypes, false);
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemForwarder_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winForwarder") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ForwarderWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuNewClient_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winClientNew") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ClientNewWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemClient_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winClient") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ClientWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemClientList_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winClientList") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ClientListWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        //private void MenuItemRequest_Click(object sender, RoutedEventArgs e)
        //{
        //    Window ObjectWin = null;
        //    foreach (Window item in mychildwindows)
        //    {
        //        if (item.Name == "winRequest") ObjectWin = item;
        //    }
        //    if (ObjectWin == null)
        //    {
        //        ObjectWin = new RequestWin();
        //        mychildwindows.Add(ObjectWin);
        //        ObjectWin.Show();
        //    }
        //    else
        //    {
        //        ObjectWin.Activate();
        //        if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
        //    }
        //}
        private void MenuItemAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winAlgorithm") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new AlgorithmWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.DataContext = new Classes.Domain.Algorithm.AlgorithmFormulaCommand();
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemContactPointType_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winContactPointType") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ContactPointTypeWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemBank_Click(object sender, RoutedEventArgs e)
        {
            //Window ObjectWin = null;
            //foreach (Window item in mychildwindows)
            //{
            //    if (item.Name == "winBank") ObjectWin = item;
            //}
            //if (ObjectWin == null)
            //{
            //    ObjectWin = new BankWin();
            //    mychildwindows.Add(ObjectWin);
            //    ObjectWin.Show();
            //}
            //else
            //{
            //    ObjectWin.Activate();
            //    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            //}
        }
        private void MenuItemImporter_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winImporter") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ImporterWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void ExpiringContracts_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winExpiringContracts") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ContractCMD cmd = new ContractCMD(true);
                ObjectWin = new ExpiringContractsWin();
                ObjectWin.DataContext = cmd;
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemContractor_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winContractor") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ContractorWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        //private void MenuItemLegal_Click(object sender, RoutedEventArgs e)
        //{
        //    Window ObjectWin = null;
        //    foreach (Window item in mychildwindows)
        //    {
        //        if (item.Name == "winLegalEntity") ObjectWin = item;
        //    }
        //    if (ObjectWin == null)
        //    {
        //        ObjectWin = new LegalEntityWin();
        //        mychildwindows.Add(ObjectWin);
        //        ObjectWin.Show();
        //    }
        //    else
        //    {
        //        ObjectWin.Activate();
        //        if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
        //    }
        //}
        private void MenuItemStoreMerge_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winStoreMerge") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new StoreMergeWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        //private void MenuItemParcel_Click(object sender, RoutedEventArgs e)
        //{
        //    Window ObjectWin = null;
        //    foreach (Window item in mychildwindows)
        //    {
        //        if (item.Name == "winParcel") ObjectWin = item;
        //    }
        //    if (ObjectWin == null)
        //    {
        //        ObjectWin = new ParcelWin();
        //        mychildwindows.Add(ObjectWin);
        //        ObjectWin.Show();
        //    }
        //    else
        //    {
        //        ObjectWin.Activate();
        //        if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
        //    }
        //}
        private void MenuListParcel_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winParcelList") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ParcelListWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuDelivery_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winDelivery") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new DeliveryWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        //private void MenuPayParcel_Click(object sender, RoutedEventArgs e)
        //{
        //    ParcelTransactionWin ObjectWin = new ParcelTransactionWin();
        //    mychildwindows.Add(ObjectWin);
        //    ObjectWin.Show();
        //}
        //private void MenuPPParcel_Click(object sender, RoutedEventArgs e)
        //{
        //    PaymentListWin ObjectWin = new PaymentListWin();
        //    mychildwindows.Add(ObjectWin);
        //    ObjectWin.Show();
        //}
        //private void MenuWayBill_Click(object sender, RoutedEventArgs e)
        //{
        //    Classes.WayBill wb = Classes.WayBill.GetWayBill();
        //    wb.CreateWayBillFromSpec();
        //}
        //private void MenuCostParcel_Click(object sender, RoutedEventArgs e)
        //{
        //    ExpenditureListWin ObjectWin = new ExpenditureListWin();
        //    mychildwindows.Add(ObjectWin);
        //    ObjectWin.Show();
        //}
        //private void MenuWithdrawal_Click(object sender, RoutedEventArgs e)
        //{
        //    WithdrawalListWin ObjectWin = new WithdrawalListWin();
        //    mychildwindows.Add(ObjectWin);
        //    ObjectWin.Show();
        //}
        //private void MenuInvoice_Click(object sender, RoutedEventArgs e)
        //{
        //    InvoiceListWin ObjectWin = new InvoiceListWin();
        //    mychildwindows.Add(ObjectWin);
        //    ObjectWin.Show();
        //}
        //private void MenuPPAccount_Click(object sender, RoutedEventArgs e)
        //{
        //    PaymentListWin ObjectWin = new PaymentListWin();
        //    mychildwindows.Add(ObjectWin);
        //    ObjectWin.Show();
        //}
        //private void MenuItemDebtor_Click(object sender, RoutedEventArgs e)
        //{
        //    Window ObjectWin = null;
        //    foreach (Window item in mychildwindows)
        //    {
        //        if (item.Name == "winCustomerBalance") ObjectWin = item;
        //    }
        //    if (ObjectWin == null)
        //    {
        //        ObjectWin = new CustomerBalanceWin();
        //        mychildwindows.Add(ObjectWin);
        //        ObjectWin.Show();
        //    }
        //    else
        //    {
        //        ObjectWin.Activate();
        //        if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
        //    }
        //}
        //private void MenuItemLegalBalance_Click(object sender, RoutedEventArgs e)
        //{
        //    Window ObjectWin = null;
        //    foreach (Window item in mychildwindows)
        //    {
        //        if (item.Name == "winLegalBalance") ObjectWin = item;
        //    }
        //    if (ObjectWin == null)
        //    {
        //        ObjectWin = new LegalBalanceWin();
        //        mychildwindows.Add(ObjectWin);
        //        ObjectWin.Show();
        //    }
        //    else
        //    {
        //        ObjectWin.Activate();
        //        if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
        //    }
        //}
        private void MenuItemParcelReport_Click(object sender, RoutedEventArgs e)
        {
            ParcelReportWin ObjectWin = new ParcelReportWin();
            mychildwindows.Add(ObjectWin);
            ObjectWin.Show();
        }
        private void MenuItemSpecificationDetails_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winSpecificationDetail") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new SpecificationDetailWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemVendorCode_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winVendorCode") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new VendorCodeWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        //private void MenuItemAllPrice_Click(object sender, RoutedEventArgs e)
        //{
        //    Window ObjectWin = null;
        //    foreach (Window item in mychildwindows)
        //    {
        //        if (item.Name == "winAllPrice") ObjectWin = item;
        //    }
        //    if (ObjectWin == null)
        //    {
        //        ObjectWin = new AllPriceWin();
        //        mychildwindows.Add(ObjectWin);
        //        ObjectWin.Show();
        //    }
        //    else
        //    {
        //        ObjectWin.Activate();
        //        if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
        //    }
        //}
        private void MenuItemGoods_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winGoods") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new GoodsWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuGoodsEnding_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winGoodsReminder") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                GoodsViewCommand cmd = new GoodsViewCommand(true);
                ObjectWin = new GoodsEndingWin();
                ObjectWin.DataContext = cmd;
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private int mychildwindowscount;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = Request_Closing();
            e.Cancel |= Parcel_Closing();
            e.Cancel |= WarehouseRU_Closing();
            int i = 0, c1;
            while (i < mychildwindows.Count)
            {
                c1 = mychildwindows.Count;
                mychildwindows[i].Close();
                i = i + 1 - c1 + mychildwindows.Count;
            }
            if (mychildwindows.Count > 0)
            {
                if (mychildwindows.Count != mychildwindowscount)
                { 
                    e.Cancel = true;
                    mychildwindowscount = mychildwindows.Count;
                }
                //else
                //    App.Current.Shutdown();
            }
            else
            {
                myrequestcmd.Filter.Dispose();
                //this.PaymentlistUC.Filter.Dispose();
            }
        }

        private bool myEndEdit()
        {
            bool isEnd = this.EventLogDataGrid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.EventLogDataGrid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Row, true);
            return isEnd;
        }
        private void myCancelEdit()
        {
            this.EventLogDataGrid.CancelEdit();
        }

        #region EventLogFilter
        public bool IsShowFilter
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        public SQLFilter Filter
        {
            get { return mylogvm.Filter; }
            set
            {
                if (this.IsLoaded && !mylogvm.SaveDataChanges())
                    MessageBox.Show("Применение фильтра невозможно. Не удалось сохранить изменения. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    mylogvm.Filter.RemoveCurrentWhere();
                    mylogvm.Filter.GetFilter(value.FilterWhereId, value.FilterGroupId);
                    if (this.IsLoaded) mylogvm.Refresh.Execute(null);
                }
            }
        }
        public void RunFilter()
        {
            if (!mylogvm.SaveDataChanges())
                MessageBox.Show("Применение фильтра невозможно. Не удалось сохранить изменения. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                mylogvm.Refresh.Execute(null);
                SetFilterButtonImage();
            }
        }
        private void SetFilterButtonImage()
        {
            string uribitmap;
            if (mylogvm.Filter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (FilterButton.Content as System.Windows.Controls.Image).Source = bi3;
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winEventLogFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new EventLogFilterWin();
                    ObjectWin.Owner = this;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
            else
            {
                if (ObjectWin != null)
                {
                    ObjectWin.Close();
                }
            }
        }
        #endregion

        #endregion

        #region Request
        decimal totalOldValue = 0;
        Classes.Domain.RequestViewCommand myrequestcmd;
        private DataModelClassLibrary.BindingDischarger myrequestdischanger;
        internal DataModelClassLibrary.BindingDischarger BindingRequestDischarger
        { get { return myrequestdischanger; } }

        private void Request_Loaded()
        {
            myrequestdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { RequestDataGrid });
            myrequestcmd = new Classes.Domain.RequestViewCommand();
            myrequestcmd.EndEdit = myrequestdischanger.EndEdit;
            myrequestcmd.CancelEdit = myrequestdischanger.CancelEdit;
            this.RequestGrid.DataContext = myrequestcmd;
            RequestTotalDataRefresh();
        }
        private bool Request_Closing()
        {
            bool cancel = false;
            if (!(myrequestdischanger.EndEdit() && myrequestcmd.SaveDataChanges()))
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    cancel = true;
                }
            }
            return cancel;
        }
        private void RequestRejectButton_Click(object sender, RoutedEventArgs e)
        {
            bool isReject = false;
            if (this.RequestDataGrid.SelectedItem is Classes.Domain.RequestVM & this.RequestDataGrid.SelectedItems.Count == 1)
            {
                isReject = MessageBox.Show("Отменить несохраненные изменения в заявке?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            }
            else
            {
                isReject = MessageBox.Show("Отменить все несохраненные изменения?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            }
            if (isReject)
            {
                myrequestcmd.Reject.Execute(this.RequestDataGrid.SelectedItems);
                PopupText.Text = "Изменения отменены";
                popInf.PlacementTarget = sender as UIElement;
                popInf.IsOpen = true;
            }
        }
        //private void RequestSaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (RequestSaveChanges())
        //    {
        //        PopupText.Text = "Изменения сохранены";
        //        popInf.PlacementTarget = sender as UIElement;
        //        popInf.IsOpen = true;
        //    }
        //}

        private void HistoryOpen_Click(object sender, RoutedEventArgs e)
        {
            RequestHistoryWin newHistory = new RequestHistoryWin();
            if ((sender as Button).Tag is RequestVM)
            {
                Request request = ((sender as Button).Tag as RequestVM).DomainObject;
                RequestHistoryViewCommand cmd = new RequestHistoryViewCommand(request);
                newHistory.DataContext = cmd;
            }
            newHistory.Owner = this;
            newHistory.Show();
        }
        private void RequestFolderOpen_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button && (sender as Button).Tag is Classes.Domain.RequestVM)
            {
                try
                {
                    Classes.Domain.RequestVM item = (sender as Button).Tag as Classes.Domain.RequestVM;
                    myrequestdischanger.EndEdit();
                    item.DomainObject.DocFolderOpen();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Папка документов");
                }
            }
        }

        private void RequestDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        //private void FreightColumn_Click(object sender, RoutedEventArgs e)
        //{
        //    RequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
        //    if (RequestDataGrid.CurrentItem is DataRowView)
        //    {
        //        RequestDS.tableRequestRow row = (RequestDataGrid.CurrentItem as DataRowView).Row as RequestDS.tableRequestRow;
        //        FreightWin winFreight = null;
        //        foreach (Window frwin in this.OwnedWindows)
        //        {
        //            if (frwin.Name == "winFreight")
        //            {
        //                if ((frwin as FreightWin).RequestRow.requestId == row.requestId) winFreight = frwin as FreightWin;
        //            }
        //        }
        //        if (winFreight == null)
        //        {
        //            foreach (Window item in this.OwnedWindows)
        //            {
        //                if (item.Name == "winRequestItem")
        //                {
        //                    if ((item as RequestItemWin).mainGrid.DataContext.Equals(this.RequestDataGrid.CurrentItem))
        //                    {
        //                        foreach (Window frwin in item.OwnedWindows)
        //                        {
        //                            if (frwin.Name == "winFreight")
        //                            {
        //                                if ((frwin as FreightWin).RequestRow.requestId == row.requestId) winFreight = frwin as FreightWin;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (winFreight == null)
        //        {
        //            winFreight = new FreightWin();
        //            if (row.isfreight) winFreight.FreightId = row.freight;
        //            else winFreight.FreightId = 0;
        //            RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.RequestGrid.FindResource("requestDS")));
        //            winFreight.agentComboBox.ItemsSource = new System.Data.DataView(requestDS.tableAgentName, string.Empty, "agentName", System.Data.DataViewRowState.CurrentRows);
        //            if (!row.IsagentIdNull()) winFreight.agentComboBox.SelectedValue = row.agentId;
        //            winFreight.RequestRow = row;
        //            winFreight.Owner = this;
        //            winFreight.Show();
        //        }
        //        else
        //        {
        //            winFreight.Activate();
        //            if (winFreight.WindowState == WindowState.Minimized) winFreight.WindowState = WindowState.Normal;
        //        }
        //    }
        //}

        private void RequestDelete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = myrequestcmd.Delete.CanExecute(this.RequestDataGrid.SelectedItems);
            e.ContinueRouting = false;
        }
        private void RequestDataGridDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myrequestcmd.Delete.Execute(this.RequestDataGrid.SelectedItems);
            e.Handled = true;
            //System.Windows.Input.RoutedCommand com = e.Command as System.Windows.Input.RoutedCommand;
            //if (com != null)
            //{
            //    if (com == ApplicationCommands.Delete && this.RequestDataGrid.SelectedItems.Count > 0)
            //    {
            //        if(MessageBox.Show("Удалить выделенные строки?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //            com.;

            //        e.Handled = true;
            //    }
            //}
        }

        #region Data Grid Total Sum
        private void RequestTotalDataRefresh()
        {
            int totalCellNumber = 0, totalCount = 0;
            decimal totalVolume = 0, totalOfficialWeight = 0, totalActualWeight = 0, totalGoodValue = 0;
            if (this.RequestDataGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < this.RequestDataGrid.SelectedItems.Count; i++)
                {
                    if (this.RequestDataGrid.SelectedItems[i] is Classes.Domain.RequestVM)
                    {
                        totalCount++;
                        Classes.Domain.RequestVM item = this.RequestDataGrid.SelectedItems[i] as Classes.Domain.RequestVM;
                        if (item.CellNumber.HasValue) totalCellNumber = totalCellNumber + item.CellNumber.Value;
                        if (item.Volume.HasValue) totalVolume = totalVolume + item.Volume.Value;
                        if (item.OfficialWeight.HasValue) totalOfficialWeight = totalOfficialWeight + item.OfficialWeight.Value;
                        if (item.ActualWeight.HasValue) totalActualWeight = totalActualWeight + item.ActualWeight.Value;
                        if (item.Invoice.HasValue) totalGoodValue = totalGoodValue + item.Invoice.Value;
                    }
                }
            }
            else
            {
                totalCount = myrequestcmd.Items.Count;
                foreach (object viewrow in myrequestcmd.Items)
                {
                    if (viewrow is Classes.Domain.RequestVM)
                    {
                        Classes.Domain.RequestVM item = viewrow as Classes.Domain.RequestVM;
                        if (item.CellNumber.HasValue) totalCellNumber = totalCellNumber + item.CellNumber.Value;
                        if (item.Volume.HasValue) totalVolume = totalVolume + item.Volume.Value;
                        if (item.OfficialWeight.HasValue) totalOfficialWeight = totalOfficialWeight + item.OfficialWeight.Value;
                        if (item.ActualWeight.HasValue) totalActualWeight = totalActualWeight + item.ActualWeight.Value;
                        if (item.Invoice.HasValue) totalGoodValue = totalGoodValue + item.Invoice.Value;
                    }
                }
            }
            RequestTotalCountTextBox.Text = totalCount.ToString();
            RequestTotalcellNumberTextBox.Text = totalCellNumber.ToString();
            RequestTotalVolumeTextBox.Text = totalVolume.ToString("N4");
            RequestTotalOfficialWeightTextBox.Text = totalOfficialWeight.ToString("N4");
            RequestTotalActualWeightTextBox.Text = totalActualWeight.ToString("N4");
            RequestTotalGoodValueTextBox.Text = totalGoodValue.ToString("N4");
        }
        private void RequestDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = e.Row.Item != null && !(e.Row.Item as RequestVM).DomainObject.Blocking();
            string col = e.Column.Header?.ToString();
            if (col == "Мест" | col == "Объем" | col == "Вес Д" | col == "Вес Ф" | col == "Инвойс")
            {
                decimal.TryParse((e.Column.GetCellContent(e.Row) as TextBlock).Text, out totalOldValue);
            }
        }
        private void RequestDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //if(!e.Row.IsEditing) (e.Row.Item as RequestVM).DomainObject.UnBlocking();
            decimal newvalue = 0;
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                Classes.Domain.RequestVM row = e.Row.Item as Classes.Domain.RequestVM;
                switch (e.Column.Header.ToString())
                {
                    case "Мест":
                        if (row.CellNumber.HasValue) newvalue = row.CellNumber.Value; else newvalue = 0;
                        RequestTotalcellNumberTextBox.Text = (decimal.Parse(RequestTotalcellNumberTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Объем":
                        if (row.Volume.HasValue) newvalue = row.Volume.Value; else newvalue = 0;
                        RequestTotalVolumeTextBox.Text = (decimal.Parse(RequestTotalVolumeTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Вес Д":
                        if (row.OfficialWeight.HasValue) newvalue = row.OfficialWeight.Value; else newvalue = 0;
                        RequestTotalOfficialWeightTextBox.Text = (decimal.Parse(RequestTotalOfficialWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Вес Ф":
                        if (row.ActualWeight.HasValue) newvalue = row.ActualWeight.Value; else newvalue = 0;
                        RequestTotalActualWeightTextBox.Text = (decimal.Parse(RequestTotalActualWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Инвойс":
                        if (row.Invoice.HasValue) newvalue = row.Invoice.Value; else newvalue = 0;
                        RequestTotalGoodValueTextBox.Text = (decimal.Parse(RequestTotalGoodValueTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                }
            }
            else
            {
                switch (e.Column.Header?.ToString())
                {
                    case "Мест":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            RequestTotalcellNumberTextBox.Text = (decimal.Parse(RequestTotalcellNumberTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Объем":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            RequestTotalVolumeTextBox.Text = (decimal.Parse(RequestTotalVolumeTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Вес Д":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            RequestTotalOfficialWeightTextBox.Text = (decimal.Parse(RequestTotalOfficialWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Вес Ф":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            RequestTotalActualWeightTextBox.Text = (decimal.Parse(RequestTotalActualWeightTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                    case "Инвойс":
                        if (decimal.TryParse((e.EditingElement as TextBox).Text, out newvalue))
                            RequestTotalGoodValueTextBox.Text = (decimal.Parse(RequestTotalGoodValueTextBox.Text) - totalOldValue + newvalue).ToString("N");
                        break;
                }
            }
        }
        private void RequestDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if ((e.Row.Item as RequestVM).DomainState==lib.DomainObjectState.Unchanged) (e.Row.Item as RequestVM).DomainObject.UnBlocking();
            if (e.EditAction == DataGridEditAction.Cancel)
            {
                RequestTotalDataRefresh();
            }
        }
        private void RequestDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if ((e.Row.Item is DataRowView) && (e.Row.Item as DataRowView).Row.RowState == DataRowState.Detached)
            {
                RequestTotalCountTextBox.Text = (int.Parse(RequestTotalCountTextBox.Text) + 1).ToString();
            }
        }
        private void RequestDataGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if ((e.Row.Item is DataRowView) && ((e.Row.Item as DataRowView).Row.RowState == DataRowState.Detached | (e.Row.Item as DataRowView).Row.RowState == DataRowState.Deleted))
            {
                RequestTotalDataRefresh();
            }
        }
        private void RequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == RequestDataGrid)
            {
                DataGridCellInfo cellinf;
                foreach (object rowview in e.AddedItems)
                {
                    if (rowview is Classes.Domain.RequestVM)
                    {
                        Classes.Domain.RequestVM vm = rowview as Classes.Domain.RequestVM;
                        if (vm.ParcelGroup.HasValue)
                            foreach (object viewrow in RequestDataGrid.Items)
                            {
                                if (!(viewrow is Classes.Domain.RequestVM)) continue;
                                Classes.Domain.RequestVM item = viewrow as Classes.Domain.RequestVM;
                                if (item.ParcelGroup.HasValue && vm.ParcelGroup == item.ParcelGroup && !RequestDataGrid.SelectedItems.Contains(viewrow))
                                {
                                    RequestDataGrid.SelectedItems.Add(viewrow);
                                    foreach (DataGridColumn colm in this.RequestDataGrid.Columns)
                                    {
                                        cellinf = new DataGridCellInfo(viewrow, colm);
                                        if (!RequestDataGrid.SelectedCells.Contains(cellinf)) RequestDataGrid.SelectedCells.Add(cellinf);
                                    }
                                    break;
                                }
                            }
                    }
                }
                foreach (object rowview in e.RemovedItems)
                {
                    if (rowview is Classes.Domain.RequestVM)
                    {
                        Classes.Domain.RequestVM vm = rowview as Classes.Domain.RequestVM;
                        if (vm.ParcelGroup.HasValue)
                            foreach (object viewrow in RequestDataGrid.SelectedItems)
                            {
                                if (!(viewrow is Classes.Domain.RequestVM)) continue;
                                Classes.Domain.RequestVM item = viewrow as Classes.Domain.RequestVM;
                                if (item.ParcelGroup.HasValue && vm.ParcelGroup == item.ParcelGroup)
                                {
                                    RequestDataGrid.SelectedItems.Remove(viewrow);
                                    foreach (DataGridColumn colm in this.RequestDataGrid.Columns)
                                    {
                                        cellinf = new DataGridCellInfo(viewrow, colm);
                                        if (RequestDataGrid.SelectedCells.Contains(cellinf)) RequestDataGrid.SelectedCells.Remove(cellinf);
                                    }
                                    break;
                                }
                            }
                    }
                }
                RequestTotalDataRefresh();
            }
        }
        #endregion

        //private void RequestRefreshButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (RequestSaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) RequestDataLoad();
        //}

        private void RequestButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            //RequestDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            //RequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            //BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.RequestDataGrid.ItemsSource) as BindingListCollectionView;
            //this.RequestDataGrid.CurrentItem = view.AddNew();
            myrequestcmd.Add.Execute(null);
            this.RequestDataGrid.CurrentItem = myrequestcmd.Items.CurrentItem;
            RequestItem_Click(this, new RoutedEventArgs());
        }
        private void RequestItem_Click(object sender, RoutedEventArgs e)
        {
            if (myrequestcmd.Items.CurrentItem is Classes.Domain.RequestVM & RequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
            {

                //if (!this.RequestDataGrid.CurrentCell.IsValid) //для обновления Grid
                //{
                //    if (!this.RequestDataGrid.IsFocused) this.RequestDataGrid.Focus();
                //    this.RequestDataGrid.CurrentCell = new DataGridCellInfo(this.RequestDataGrid.CurrentItem, this.RequestDataGrid.Columns[4]);
                //}
                //if (((this.RequestDataGrid.CurrentItem as DataRowView).Row as RequestDS.tableRequestRow).parceltype == 2)
                OpenNewRequest();
                //else
                //    OpenOldRequest();
            }
        }
        //private void OpenOldRequest()
        //{
        //    RequestItemWin newWin = null;
        //    foreach (Window item in this.OwnedWindows)
        //    {
        //        if (item.Name == "winRequestItem")
        //        {
        //            if ((item as RequestItemWin).mainGrid.DataContext.Equals(this.RequestDataGrid.CurrentItem))
        //                newWin = item as RequestItemWin;
        //        }
        //    }
        //    if (newWin == null)
        //    {
        //        newWin = new RequestItemWin();
        //        newWin.Owner = this;
        //        ReferenceDS refDS = this.RequestGrid.FindResource("keyReferenceDS") as ReferenceDS;
        //        newWin.statusComboBox.ItemsSource = new System.Data.DataView(refDS.tableRequestStatus, "rowId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
        //        newWin.statusComboBox.IsDropDownOpen = false;
        //        newWin.goodsComboBox.ItemsSource = new System.Data.DataView(refDS.tableGoodsType, "Iditem>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
        //        newWin.parceltypeComboBox.ItemsSource = new System.Data.DataView(refDS.tableParcelType);
        //        if (!((this.RequestDataGrid.CurrentItem as DataRowView).Row as RequestDS.tableRequestRow).IsfullNumberNull()) { newWin.parceltypeComboBox.IsEnabled = false; }
        //        //newWin.forwarderComboBox.ItemsSource = new System.Data.DataView(refDS.tableForwarder, "itemId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
        //        //newWin.storeComboBox.ItemsSource = new System.Data.DataView(refDS.tableStore, "storeId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);

        //        RequestDS requestDS = ((RequestDS)(this.RequestGrid.FindResource("requestDS")));
        //        newWin.customerComboBox.ItemsSource = new System.Data.DataView(requestDS.tableCustomerName, string.Empty, "customerName", System.Data.DataViewRowState.CurrentRows);
        //        newWin.agentComboBox.ItemsSource = new System.Data.DataView(requestDS.tableAgentName, string.Empty, "agentName", System.Data.DataViewRowState.CurrentRows);
        //        newWin.mainGrid.DataContext = this.RequestDataGrid.CurrentItem;
        //        newWin.RequestItemViewCommand = new Classes.Domain.RequestItemViewCommand(((this.RequestDataGrid.CurrentItem as DataRowView).Row as CustomBrokerWpf.RequestDS.tableRequestRow).requestId);
        //        newWin.thisStoragePointValidationRule.RequestId = ((this.RequestDataGrid.CurrentItem as DataRowView).Row as CustomBrokerWpf.RequestDS.tableRequestRow).requestId;
        //        newWin.Show();
        //    }
        //    else
        //    {
        //        newWin.Activate();
        //        if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
        //    }
        //}
        private void OpenNewRequest()
        {
            RequestNewWin newWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winRequestNew")
                {
                    if ((item.DataContext as Classes.Domain.RequestVMCommand).VModel.DomainObject.Equals((this.RequestDataGrid.CurrentItem as Classes.Domain.RequestVM).DomainObject))
                        newWin = item as RequestNewWin;
                }
            }
            if (newWin == null)
            {
                newWin = new RequestNewWin();
                newWin.thisStoragePointValidationRule.RequestId = (this.RequestDataGrid.CurrentItem as Classes.Domain.RequestVM).Id;
                Classes.Domain.RequestVMCommand cmd = new Classes.Domain.RequestVMCommand(this.RequestDataGrid.CurrentItem as Classes.Domain.RequestVM, myrequestcmd.Items);
                cmd.EndEdit = newWin.BindingDischarger.EndEdit;
                cmd.CancelEdit = newWin.BindingDischarger.CancelEdit;
                newWin.DataContext = cmd;
                mychildwindows.Add(newWin);
                newWin.Show();
            }
            else
            {
                newWin.Activate();
                if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
            }
        }

        private void RequestSortAZButton_Click(object sender, RoutedEventArgs e)
        {
            if (RequestDataGrid.CurrentColumn != null)
            {
                try
                {
                    BindingListCollectionView view = CollectionViewSource.GetDefaultView(RequestDataGrid.ItemsSource) as BindingListCollectionView;
                    System.ComponentModel.SortDescription newsort = new System.ComponentModel.SortDescription(RequestDataGrid.CurrentColumn.SortMemberPath, System.ComponentModel.ListSortDirection.Ascending);
                    view.SortDescriptions.Insert(0, newsort);
                    RequestDataGrid.CurrentColumn.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Невозможно изменить сортировку во время редактирования данных. \n Завершите редактирование строки.", "Сортировка", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void RequestSortZAButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RequestDataGrid.CurrentColumn != null)
                {
                    BindingListCollectionView view = CollectionViewSource.GetDefaultView(RequestDataGrid.ItemsSource) as BindingListCollectionView;
                    System.ComponentModel.SortDescription newsort = new System.ComponentModel.SortDescription(RequestDataGrid.CurrentColumn.SortMemberPath, System.ComponentModel.ListSortDirection.Descending);
                    view.SortDescriptions.Insert(0, newsort);
                    RequestDataGrid.CurrentColumn.SortDirection = System.ComponentModel.ListSortDirection.Descending;
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Невозможно изменить сортировку во время редактирования данных. \n Завершите редактирование строки.", "Сортировка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void RequestSoprtClean_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(RequestDataGrid.ItemsSource) as BindingListCollectionView;
                view.SortDescriptions.Clear();
                foreach (DataGridColumn item in RequestDataGrid.Columns)
                {
                    item.SortDirection = null;
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Невозможно изменить сортировку во время редактирования данных. \n Завершите редактирование строки.", "Сортировка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //private CustomBrokerWpf.SQLFilter myrequestfilter;
        //public bool RequestIsShowFilter
        //{
        //    set
        //    {
        //        this.RequestFilterButton.IsChecked = value;
        //    }
        //    get { return this.RequestFilterButton.IsChecked.Value; }
        //}
        //internal SQLFilter RequestFilter
        //{
        //    get { return myrequestfilter; }
        //    set
        //    {
        //        if (!myrequestcmd.SaveDataChanges())
        //            MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        else
        //        {
        //            myrequestfilter = value;
        //            myrequestcmd.Refresh.Execute(null);
        //        }
        //    }
        //}
        //internal void RequestRunFilter()
        //{
        //    if (!myrequestcmd.SaveDataChanges())
        //        MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //    else
        //    {
        //        myrequestcmd.Refresh.Execute(null);
        //    }
        //}
        private void RequestFilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winRequestFilter") ObjectWin = item;
            }
            if (RequestFilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new RequestFilterWin() { FilterOwner = myrequestcmd };
                    ObjectWin.Owner = this;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
            else
            {
                if (ObjectWin != null)
                {
                    ObjectWin.Close();
                }
            }
        }
        private void RequestFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RequestFastFilterRun();
            }
        }
        private void RequestFastFilterRun()
        {
            if(myrequestcmd.RunFastFilter.CanExecute(null))
                myrequestcmd.RunFastFilter.Execute(null);
        }
        private void RequestFastFilterButton_Click(object sender, RoutedEventArgs e)
        {
            RequestFastFilterRun();
        }
        private void UnLockButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Снять блокировки со всех редактируемых объектов?", "Снятие блокировок", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                lib.Common.BlockingDBM bdbm = new lib.Common.BlockingDBM(CustomBrokerWpf.References.ConnectionString);
                bdbm.ClearLocks();
                if (bdbm.Errors.Count > 0)
                    Common.PopupCreator.GetPopup(text: bdbm.ErrorMessage
                        , background: System.Windows.Media.Brushes.LightPink
                        , foreground: System.Windows.Media.Brushes.Red
                        , placementtarget: sender as UIElement
                        , placement: System.Windows.Controls.Primitives.PlacementMode.Bottom
                    ).IsOpen = true;
            }
        }

        //private void ColmarkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (RequestDataGrid.SelectedItems.Count > 0 & e.AddedItems.Count > 0)
        //    {
        //        RequestDS.tableRequestRow row;
        //        foreach (DataRowView viewrow in RequestDataGrid.SelectedItems)
        //        {
        //            if (viewrow != RequestDataGrid.CurrentItem)
        //            {
        //                row = viewrow.Row as RequestDS.tableRequestRow;
        //                row.colmark = (e.AddedItems[0] as System.Windows.Shapes.Rectangle).Fill.ToString();
        //                row.EndEdit();
        //            }
        //        }
        //    }
        //}
        private void RequestDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is TextBlock && (RequestDataGrid.CurrentCell.Column?.SortMemberPath == "StorePointDate" || RequestDataGrid.CurrentCell.Column?.SortMemberPath == "Id"))
            {
                RequestItem_Click(sender, e);
                e.Handled = true;
            }
        }
 
        private void FoldersMoveButton_Click(object sender, RoutedEventArgs e)
        {
            myrequestcmd.FoldersMove.Execute(null);
        }
        #endregion

        #region Машина
        private ParcelCurItemCommander myparcelcmd;
        private void ParcelTabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (myparcelcmd == null)
                Parcel_Loaded();
        }
        private void Parcel_Loaded()
        {
            myparcelcmd = new ParcelCurItemCommander();
            this.ParcelNew.DataContext = myparcelcmd;
        }

        private bool Parcel_Closing()
        {
            bool cancel = false;
            if (myparcelcmd != null)
            {
                myparcelcmd.Save.Execute(null);
                if (!myparcelcmd.LastSaveResult)
                {
                    this.Activate();
                    if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        cancel = true;
                    }
                }
                if (!cancel) myparcelcmd.Filter.Dispose();
            }
            return cancel;
        }

        #endregion

        #region Account
        private PaymentRegisterViewCommander mypaydcmd;
        private PaymentRegisterViewCommander mypaytcmd;
        private GTDRegisterViewCommander mygtddcmd;
        private GTDRegisterViewCommander mygtdtcmd;
        private GTDRegisterViewCommander myteodcmd;
        private GTDRegisterViewCommander myteotcmd;

        private void TabItem1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mypaydcmd == null)
            {
                mypaydcmd = new PaymentRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2));
                mypaydcmd.IsReadOnly = true;
                this.PaymentDeliveryGrid.DataContext = mypaydcmd;
            }
        }
        private void TabItem2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mypaytcmd == null)
            {
                mypaytcmd = new PaymentRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1));
                mypaytcmd.IsReadOnly = true;
                this.PaymentTradeGrid.DataContext = mypaytcmd;
            }
        }
        private void TabItem3_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mygtddcmd == null)
            {
                mygtddcmd = new GTDRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2), "ТД");
                this.GTDDeliveryGrid.DataContext = mygtddcmd;
            }
        }
        private void TabItem4_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mygtdtcmd == null)
            {
                mygtdtcmd = new GTDRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1), "ТД");
                this.GTDTradeGrid.DataContext = mygtdtcmd;
            }
        }
        private void TabItem5_GotFocus(object sender, RoutedEventArgs e)
        {
            if (myteodcmd == null)
            {
                myteodcmd = new GTDRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2), "ТЭО");
                this.TEODeliveryGrid.DataContext = myteodcmd;
            }
        }
        private void TabItem6_GotFocus(object sender, RoutedEventArgs e)
        {
            if (myteotcmd == null)
            {
                myteotcmd = new GTDRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1), "ТЭО");
                this.TEOTradeGrid.DataContext = myteotcmd;
            }
        }
        #endregion

        #region Склад Москва
        private WarehouseRUViewCommader myskucmd;
        private void WarehouseRUTabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (myskucmd == null)
                WarehouseRU_Loaded();
        }
        private void WarehouseRU_Loaded()
        {
            myskucmd = new Classes.Domain.WarehouseRUViewCommader();
            myskucmd.IsReadOnly = true;
            this.WarehouseRUTabItem.DataContext = myskucmd;
        }
        private bool WarehouseRU_Closing()
        {
            bool cancel = false;
            if (myskucmd != null)
            {
                myskucmd.Save.Execute(null);
                if (!myskucmd.LastSaveResult)
                {
                    this.Activate();
                    if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        cancel = true;
                        this.WarehouseRUTabItem.IsSelected = true;
                    }
                }
                if(!cancel) {myskucmd.Filter.Dispose(); }
            }
            return cancel;
        }
        #endregion

        //INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

    }
}
