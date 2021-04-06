using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
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
            Parcel_Loaded();
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
        private void MenuItemAgent_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = new AgentWin();
            mychildwindows.Add(ObjectWin);
            ObjectWin.Show();
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
                Classes.CountriesVM vm = new Classes.CountriesVM();
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
        private void MenuItemGoodsType_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winGoodsType") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new GoodsTypeWin();
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
                if (item.Name == "winReferenceSympleItem" && item.Title == "Склад") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ReferenceSympleItemWin();
                ObjectWin.Title = "Склад";
                ObjectWin.Icon = System.Windows.Media.Imaging.BitmapFrame.Create(new Uri("pack://application:,,,/CustomBrokerWpf;component/Images/forklifter.png"));
                (ObjectWin as ReferenceSympleItemWin).CanAddRows = false;
                (ObjectWin as ReferenceSympleItemWin).CanDeleteRows = false;
                (ObjectWin as ReferenceSympleItemWin).SetDataContext(CustomBrokerWpf.References.Stores, false);
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
        private void MenuItemRequest_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winRequest") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new RequestWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
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
        private void MenuItemLegal_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winLegalEntity") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new LegalEntityWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
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
        private void MenuItemParcel_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winParcel") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new ParcelWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
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
        private void MenuPPParcel_Click(object sender, RoutedEventArgs e)
        {
            PaymentListWin ObjectWin = new PaymentListWin();
            mychildwindows.Add(ObjectWin);
            ObjectWin.Show();
        }
        private void MenuWayBill_Click(object sender, RoutedEventArgs e)
        {
            Classes.WayBill wb = Classes.WayBill.GetWayBill();
            wb.CreateWayBillFromSpec();
        }
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
        private void MenuInvoice_Click(object sender, RoutedEventArgs e)
        {
            InvoiceListWin ObjectWin = new InvoiceListWin();
            mychildwindows.Add(ObjectWin);
            ObjectWin.Show();
        }
        private void MenuPPAccount_Click(object sender, RoutedEventArgs e)
        {
            PaymentListWin ObjectWin = new PaymentListWin();
            mychildwindows.Add(ObjectWin);
            ObjectWin.Show();
        }
        private void MenuItemDebtor_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winCustomerBalance") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CustomerBalanceWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void MenuItemLegalBalance_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winLegalBalance") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new LegalBalanceWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private int mychildwindowscount;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = Request_Closing();
            e.Cancel |= Parcel_Closing();
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
                this.RequestFilter.Dispose();
                myparcelcmd.Filter.Dispose();
                //this.ParcelPaymentsUC.Filter.Dispose();
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

        private void MenuItemAllPrice_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in mychildwindows)
            {
                if (item.Name == "winAllPrice") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new AllPriceWin();
                mychildwindows.Add(ObjectWin);
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
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
            myrequestfilter = myrequestcmd.Filter;
            this.RequestGrid.DataContext = myrequestcmd;
            RequestStoragePointFilter = string.Empty;
            RequestTotalDataRefresh();
        }
        //private void RequestDataLoad()
        //{
        //    try
        //    {
        //        KirillPolyanskiy.CustomBrokerWpf.RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.RequestGrid.FindResource("requestDS")));
        //        RequestDSTableAdapters.tableAgentNameAdapter thisAgentNameAdapter = new RequestDSTableAdapters.tableAgentNameAdapter();
        //        thisAgentNameAdapter.Fill(requestDS.tableAgentName);
        //        RequestDSTableAdapters.tableCustomerNameAdapter thisCustomerNameAdapter = new RequestDSTableAdapters.tableCustomerNameAdapter();
        //        thisCustomerNameAdapter.Fill(requestDS.tableCustomerName);
        //        ReferenceDS refDS = this.RequestGrid.FindResource("keyReferenceDS") as ReferenceDS;
        //        if (refDS.tableRequestStatus.Count == 0)
        //        {
        //            ReferenceDSTableAdapters.RequestStatusAdapter adapterStatus = new ReferenceDSTableAdapters.RequestStatusAdapter();
        //            adapterStatus.Fill(refDS.tableRequestStatus);
        //        }
        //        CollectionViewSource statusVS = this.RequestGrid.FindResource("keyStatusVS") as CollectionViewSource;
        //        statusVS.Source = new System.Data.DataView(refDS.tableRequestStatus, "rowId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
        //        if (refDS.tableGoodsType.Count == 0)
        //        {
        //            ReferenceDSTableAdapters.GoodsTypeAdapter adapterGoodsType = new ReferenceDSTableAdapters.GoodsTypeAdapter();
        //            adapterGoodsType.Fill(refDS.tableGoodsType);
        //        }
        //        CollectionViewSource goodsVS = this.RequestGrid.FindResource("keyGoodsTypeVS") as CollectionViewSource;
        //        goodsVS.Source = new System.Data.DataView(refDS.tableGoodsType, "Iditem>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
        //        if (refDS.tableStore.Count == 0)
        //        {
        //            ReferenceDSTableAdapters.StoreAdapter adapterStore = new ReferenceDSTableAdapters.StoreAdapter();
        //            adapterStore.Fill(refDS.tableStore);
        //        }
        //        CollectionViewSource storeVS = this.RequestGrid.FindResource("keyStoreVS") as CollectionViewSource;
        //        storeVS.Source = new System.Data.DataView(refDS.tableStore, "storeId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
        //        if (refDS.tableForwarder.Count == 0)
        //        {
        //            ReferenceDSTableAdapters.ForwarderAdapter adapterStore = new ReferenceDSTableAdapters.ForwarderAdapter();
        //            adapterStore.Fill(refDS.tableForwarder);
        //        }
        //        CollectionViewSource forwarderVS = this.RequestGrid.FindResource("keyForwarderVS") as CollectionViewSource;
        //        forwarderVS.Source = new System.Data.DataView(refDS.tableForwarder, "itemId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
        //        if (refDS.tableParcelType.Count == 0) refDS.ParcelTypeRefresh();
        //        CollectionViewSource parceltypeVS = this.RequestGrid.FindResource("keyParcelTypeVS") as CollectionViewSource;
        //        parceltypeVS.Source = new System.Data.DataView(refDS.tableParcelType);
        //        RequestDataRefresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is System.Data.SqlClient.SqlException)
        //        {
        //            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //            System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //            {
        //                errs.Append(sqlerr.Message + "\n");
        //            }
        //            MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        else
        //        {
        //            MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
        //        {
        //            RequestDataLoad();
        //        }
        //    }
        //}
        //private void RequestDataRefresh()
        //{
        //    try
        //    {
        //        BindingListCollectionView view;
        //        KirillPolyanskiy.CustomBrokerWpf.RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.RequestGrid.FindResource("requestDS")));
        //        view = CollectionViewSource.GetDefaultView(requestDS.tableRequest.DefaultView) as BindingListCollectionView;
        //        System.ComponentModel.SortDescription[] sortColl = new System.ComponentModel.SortDescription[view.SortDescriptions.Count];
        //        view.SortDescriptions.CopyTo(sortColl, 0);
        //        KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest requestAdapter = new KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest();
        //        RequestDataGrid.ItemsSource = null;
        //        requestAdapter.Fill(requestDS.tableRequest, myrequestfilter.FilterWhereId);
        //        RequestDataGrid.ItemsSource = requestDS.tableRequest.DefaultView;
        //        using (view.DeferRefresh())
        //        {
        //            foreach (System.ComponentModel.SortDescription itemsort in sortColl)
        //            {
        //                view.SortDescriptions.Add(itemsort);
        //                foreach (DataGridColumn colmn in RequestDataGrid.Columns)
        //                {
        //                    if (colmn.SortMemberPath.Equals(itemsort.PropertyName))
        //                    {
        //                        colmn.SortDirection = itemsort.Direction;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        RequestTotalDataRefresh();
        //        //string uribitmap;
        //        //if (myrequestfilter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
        //        //else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
        //        //System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
        //        //(RequestFilterButton.Content as Image).Source = bi3;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is System.Data.SqlClient.SqlException)
        //        {
        //            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //            System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //            {
        //                errs.Append(sqlerr.Message + "\n");
        //            }
        //            MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        else
        //        {
        //            MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
        //        {
        //            RequestDataLoad();
        //        }
        //    }
        //}
        //private void RequestFilterLoad()
        //{
        //    using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
        //    {
        //        try
        //        {
        //            SqlCommand com = new SqlCommand();
        //            com.Connection = con;
        //            com.CommandType = CommandType.StoredProcedure;
        //            com.CommandText = "dbo.UserFilter_sp";
        //            SqlParameter winname = new SqlParameter("@winName", this.Name);
        //            com.Parameters.Add(winname);
        //            System.Xml.XmlReader reader = com.ExecuteXmlReader();

        //        }
        //        catch (Exception ex)
        //        {
        //            if (ex is System.Data.SqlClient.SqlException)
        //            {
        //                System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //                System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //                {
        //                    errs.Append(sqlerr.Message + "\n");
        //                }
        //                MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //            else
        //            {
        //                MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //            if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
        //            {
        //                RequestDataLoad();
        //            }
        //        }
        //        finally { con.Close(); }
        //    }
        //}
        //private bool RequestSaveChanges()
        //{
        //    bool isSuccess = false;
        //    RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.RequestGrid.FindResource("requestDS")));
        //    try
        //    {
        //        RequestDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
        //        RequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
        //        KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest requestDSRequest_tbTableAdapter = new KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest();
        //        requestDSRequest_tbTableAdapter.Adapter.ContinueUpdateOnError = false;
        //        requestDSRequest_tbTableAdapter.Update(requestDS.tableRequest);

        //        isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is System.Data.SqlClient.SqlException)
        //        {
        //            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //            if (err.Number > 49999)
        //            {
        //                switch (err.Number)
        //                {
        //                    case 50000:
        //                        MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        break;
        //                    case 50001:
        //                        try
        //                        {
        //                            DataRow[] errrows = requestDS.tableRequest.GetErrors();
        //                            RequestDS.tableRequestRow requestrow = errrows[0] as RequestDS.tableRequestRow;
        //                            RequestConflictResolution res = new RequestConflictResolution(requestrow);
        //                            int newstamp = res.isCheckedRow();
        //                            if (newstamp != 0)
        //                            {
        //                                requestrow.ClearErrors();
        //                                requestrow.stamp = newstamp;
        //                                requestrow.EndEdit();
        //                                return RequestSaveChanges();
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            }
        //                        }
        //                        catch (Exception ep)
        //                        {
        //                            MessageBox.Show(ep.Message + "\n" + ep.Source, "Разрешение конфликта записи", MessageBoxButton.OK, MessageBoxImage.Error);
        //                        }
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //                {
        //                    errs.Append(sqlerr.Message + "\n");
        //                }
        //                MessageBox.Show(errs.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    return isSuccess;
        //}
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

        private void FreightColumn_Click(object sender, RoutedEventArgs e)
        {
            RequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            if (RequestDataGrid.CurrentItem is DataRowView)
            {
                RequestDS.tableRequestRow row = (RequestDataGrid.CurrentItem as DataRowView).Row as RequestDS.tableRequestRow;
                FreightWin winFreight = null;
                foreach (Window frwin in this.OwnedWindows)
                {
                    if (frwin.Name == "winFreight")
                    {
                        if ((frwin as FreightWin).RequestRow.requestId == row.requestId) winFreight = frwin as FreightWin;
                    }
                }
                if (winFreight == null)
                {
                    foreach (Window item in this.OwnedWindows)
                    {
                        if (item.Name == "winRequestItem")
                        {
                            if ((item as RequestItemWin).mainGrid.DataContext.Equals(this.RequestDataGrid.CurrentItem))
                            {
                                foreach (Window frwin in item.OwnedWindows)
                                {
                                    if (frwin.Name == "winFreight")
                                    {
                                        if ((frwin as FreightWin).RequestRow.requestId == row.requestId) winFreight = frwin as FreightWin;
                                    }
                                }
                            }
                        }
                    }
                }
                if (winFreight == null)
                {
                    winFreight = new FreightWin();
                    if (row.isfreight) winFreight.FreightId = row.freight;
                    else winFreight.FreightId = 0;
                    RequestDS requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.RequestGrid.FindResource("requestDS")));
                    winFreight.agentComboBox.ItemsSource = new System.Data.DataView(requestDS.tableAgentName, string.Empty, "agentName", System.Data.DataViewRowState.CurrentRows);
                    if (!row.IsagentIdNull()) winFreight.agentComboBox.SelectedValue = row.agentId;
                    winFreight.RequestRow = row;
                    winFreight.Owner = this;
                    winFreight.Show();
                }
                else
                {
                    winFreight.Activate();
                    if (winFreight.WindowState == WindowState.Minimized) winFreight.WindowState = WindowState.Normal;
                }
            }
        }

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

        private CustomBrokerWpf.SQLFilter myrequestfilter;
        public bool RequestIsShowFilter
        {
            set
            {
                this.RequestFilterButton.IsChecked = value;
            }
            get { return this.RequestFilterButton.IsChecked.Value; }
        }
        internal SQLFilter RequestFilter
        {
            get { return myrequestfilter; }
            set
            {
                if (!myrequestcmd.SaveDataChanges())
                    MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    myrequestfilter = value;
                    myrequestcmd.Refresh.Execute(null);
                }
            }
        }
        internal void RequestRunFilter()
        {
            if (!myrequestcmd.SaveDataChanges())
                MessageBox.Show("Применение фильтра невозможно. Регистр содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                myrequestcmd.Refresh.Execute(null);
            }
        }
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
                    ObjectWin = new RequestFilterWin();
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
            this.RequestFilter.SetNumber(this.RequestFilter.FilterWhereId, "customerId", 0, (RequestClientFilter?.ToString() ?? string.Empty));
            //if (string.IsNullOrEmpty(this.RequestStoragePointFilter))
            this.RequestFilter.SetNumber(this.RequestFilter.FilterWhereId, "storagePoint", 0, this.RequestStoragePointFilter);
            //this.RequestFilter.ConditionValueAdd(this.RequestFilter.ConditionAdd(this.RequestFilter.FilterWhereId, "storagePoint", "="), this.RequestStoragePointFilter, 0);
            this.RequestRunFilter();
        }
        private int? myrequestclientfilter;
        public int? RequestClientFilter
        {
            set
            {
                myrequestclientfilter = value;
                PropertyChangedNotification("RequestClientFilter");
            }
            get { return myrequestclientfilter; }
        }
        private string myrequeststoragepointfilter;
        public string RequestStoragePointFilter
        {
            set
            {
                myrequeststoragepointfilter = value;
                PropertyChangedNotification("RequestStoragePointFilter");
            }
            get { return myrequeststoragepointfilter; }
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

        private void ColmarkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RequestDataGrid.SelectedItems.Count > 0 & e.AddedItems.Count > 0)
            {
                RequestDS.tableRequestRow row;
                foreach (DataRowView viewrow in RequestDataGrid.SelectedItems)
                {
                    if (viewrow != RequestDataGrid.CurrentItem)
                    {
                        row = viewrow.Row as RequestDS.tableRequestRow;
                        row.colmark = (e.AddedItems[0] as System.Windows.Shapes.Rectangle).Fill.ToString();
                        row.EndEdit();
                    }
                }
            }
        }
        private void RequestDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RequestDataGrid.CurrentCell.Column?.SortMemberPath == "StorePointDate")
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
        private lib.BindingDischarger myparcelbinddisp;

        private void Parcel_Loaded()
        {
            myparcelbinddisp = new lib.BindingDischarger(this, new DataGrid[] { ParcelRequestDataGrid, NoParcelRequestDataGrid });
            myparcelcmd = new ParcelCurItemCommander();
            myparcelcmd.CancelEdit = myparcelbinddisp.CancelEdit;
            myparcelcmd.EndEdit = myparcelbinddisp.EndEdit;
            ParcelGrid.DataContext = myparcelcmd;

            //Синхронизация ширины столбцов
            for(int i=0;i< this.ParcelRequestDataGrid.Columns.Count;i++)
                if (this.ParcelRequestDataGrid.Columns[i].ActualWidth > this.NoParcelRequestDataGrid.Columns[i].ActualWidth)
                    this.NoParcelRequestDataGrid.Columns[i].Width = this.ParcelRequestDataGrid.Columns[i].ActualWidth;
                else if(this.ParcelRequestDataGrid.Columns[i].ActualWidth < this.NoParcelRequestDataGrid.Columns[i].ActualWidth)
                    this.ParcelRequestDataGrid.Columns[i].Width = this.NoParcelRequestDataGrid.Columns[i].ActualWidth;
            DependencyPropertyDescriptor textDescr = DependencyPropertyDescriptor.FromProperty(DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));
            if (textDescr != null)
            {
                foreach (DataGridColumn column in this.ParcelRequestDataGrid.Columns)
                {
                    textDescr.AddValueChanged(column, delegate
                  {
                      if(column.DisplayIndex>=0) ParcelRequestDataGrid_SizeChanged(column);
                  });
                }
                foreach (DataGridColumn column in this.NoParcelRequestDataGrid.Columns)
                {
                    textDescr.AddValueChanged(column, delegate
                    {
                        if (column.DisplayIndex >= 0) NoParcelRequestDataGrid_SizeChanged(column);
                    });
                }
            }
        }

        private bool Parcel_Closing()
        {
            bool cancel = false;
            if (!myparcelbinddisp.EndEdit() || !myparcelcmd.SaveDataChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    cancel = true;
                }
            }
            return cancel;
        }
        //private void CheckGroup(DataRow[] rows)
        //{
        //    SqlCommand com = new SqlCommand();
        //    using (SqlConnection con = new SqlConnection(References.ConnectionString))
        //    {
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.CommandText = "ParcelGroupCheck_sp";
        //        com.Connection = con;
        //        SqlParameter parId = new SqlParameter();
        //        parId.ParameterName = "@parcelId";
        //        parId.SqlDbType = SqlDbType.Int;
        //        com.Parameters.Add(parId);
        //        SqlParameter parRez = new SqlParameter();
        //        parRez.Direction = ParameterDirection.Output;
        //        parRez.ParameterName = "@equals";
        //        parRez.SqlDbType = SqlDbType.TinyInt;
        //        com.Parameters.Add(parRez);
        //        con.Open();
        //        foreach (DataRow row in rows)
        //        {
        //            parId.Value = (row as ParcelDS.tableParcelRow).parcelId;
        //            com.ExecuteNonQuery();
        //            if ((byte)parRez.Value != 0) MessageBox.Show("Не все группы заявок поставлены в загрузку " + (row as ParcelDS.tableParcelRow).fullNumber + " полностью!", "Группы заявок", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        }
        //        con.Close();
        //    }
        //}

        private void ParceltoExcelButton_Click(object sender, RoutedEventArgs e)
        {
            //bool isNew;
            (sender as Button).CommandParameter = MessageBox.Show("Перенести в Excel только новые заявки?", "в Excel", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
            //ExcelReport(null, isNew);
            //ExcelReport(1, isNew);
            //ExcelReport(2, isNew);
        }
        //private void ParceltoDocButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (ParcelNumberList.SelectedItem is DataRowView)
        //        {
        //            ParcelDS.tableParcelRow prow = (ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow;
        //            string path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + "Отправки\\" + prow.docdirpath;
        //            if (!Directory.Exists(path))
        //            {
        //                System.IO.Directory.CreateDirectory(path);
        //            }
        //            System.Diagnostics.Process.Start(path);
        //            //else if (Directory.Exists("E:\\Счета\\" + prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5)))
        //            //{
        //            //    prow.docdirpath = prow.fullNumber + prow.docdirpath.Substring(prow.docdirpath.Length - 5);
        //            //    prow.EndEdit();
        //            //    System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
        //            //}
        //            //else
        //            //{
        //            //    if (MessageBox.Show("Не удалось найти папку отправки: E:\\Счета\\" + prow.docdirpath + "\nСоздать папку?", "Папка документов", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //            //    {
        //            //        System.IO.Directory.CreateDirectory("E:\\Счета\\" + prow.docdirpath);
        //            //        System.Diagnostics.Process.Start("E:\\Счета\\" + prow.docdirpath);
        //            //    }
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Папка документов");
        //    }
        //}
        //private void MoveInformStore_Click(object sender, RoutedEventArgs e)
        //{
        //    ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
        //    ParcelRequestDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
        //    for (int i = 0; i < ParcelRequestDataGrid.Items.Count; i++)
        //    {
        //        Classes.Domain.RequestVM row = this.ParcelRequestDataGrid.Items[i] as Classes.Domain.RequestVM;
        //        if (!row.StoreInform.HasValue)
        //        {
        //            row.StoreInform = DateTime.Today;
        //        }
        //    }
        //}
        //private void MoveSpecification_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.ParcelSaveChanges() && this.ParcelNumberList.SelectedIndex > -1 && ((this.ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).parceltype == 1)
        //    {
        //        FileInfo[] files;
        //        string num = ((this.ParcelNumberList.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow).parcelnumber;
        //        DirectoryInfo dirIn = new DirectoryInfo(@"V:\Отправки");
        //        if (dirIn.Exists)
        //        {
        //            if (dirIn.GetDirectories(num + "_*").Length > 0)
        //            {
        //                dirIn = dirIn.GetDirectories(num + "_*")[0];
        //                DirectoryInfo dirOut = new DirectoryInfo(@"V:\Спецификации");
        //                if (dirOut.Exists)
        //                {
        //                    foreach (Classes.Domain.RequestVM row in viewParcelRequest)
        //                    {
        //                        if (!row.DomainObject.ParcelId.HasValue) continue;
        //                        files = dirOut.GetFiles("*" + row.StorePoint + "*");
        //                        if (files.Length > 0)
        //                        {
        //                            try
        //                            {
        //                                if (File.Exists(dirIn.FullName + "\\" + files[0].Name))
        //                                    File.Delete(dirIn.FullName + "\\" + files[0].Name);
        //                                files[0].MoveTo(dirIn.FullName + "\\" + files[0].Name);
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                MessageBox.Show(ex.Message, "Ошибка доступа к файлу", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            }
        //                        }
        //                        if (dirIn.GetFiles("*" + row.StorePoint + "*").Length > 0)
        //                        {
        //                            row.IsSpecification = true;
        //                        }
        //                    }
        //                }
        //                else
        //                    MessageBox.Show(@"Папка 'V:\Спецификации' не найдена!", "Перенос спецификаций", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //            else
        //                MessageBox.Show(@"Папка 'V:\Отправки\" + num + "_...' не найдена!", "Перенос спецификаций", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        else
        //            MessageBox.Show(@"Папка 'V:\Отправки' не найдена!", "Перенос спецификаций", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        private void RequestExcelButton_Click(object sender, RoutedEventArgs e)
        {
            if (myparcelcmd.CurrentItem == null) return;
            if (RequestExcelTask == null || RequestExcelTask.IsCompleted)
            {
                myparcelbinddisp.EndEdit();
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
                RequestExcelTask = RequestExcelProcessingAsync();
            }
            else
            {
                System.Windows.MessageBox.Show("Предыдущая обработка еще не завершена, подождите.", "Обработка данных", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Hand);
            }
        }

        private void RequestAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (myparcelcmd.CurrentItem == null) return;
            if (myparcelcmd.CurrentItem.DomainState == lib.DomainObjectState.Added)
                myparcelcmd.Save.Execute(null);
            if ((NoParcelRequestDataGrid.SelectedIndex > -1) | (NoParcelRequestDataGrid.Items.Count == 1))
            {
                if (NoParcelRequestDataGrid.Items.Count == 1) this.NoParcelRequestDataGrid.SelectedItems.Add(this.NoParcelRequestDataGrid.Items[0]);

                Classes.Domain.RequestVM[] rows = new Classes.Domain.RequestVM[NoParcelRequestDataGrid.SelectedItems.Count];
                for (int i = 0; i < NoParcelRequestDataGrid.SelectedItems.Count; i++)
                {
                    Classes.Domain.RequestVM row = this.NoParcelRequestDataGrid.SelectedItems[i] as Classes.Domain.RequestVM;
                    if (!row.InvoiceDiscountFill.Value)
                    {
                        MessageBox.Show("В заявке " + row.StorePointDate + " инвойс со скидкой не разнесен по юр лицам!", "Постановка в загрузку", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    if (!row.Validate(true))
                    {
                        MessageBox.Show(row.Errors, "Постановка в загрузку", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                    rows[i] = this.NoParcelRequestDataGrid.SelectedItems[i] as Classes.Domain.RequestVM;
                }
                this.NoParcelRequestDataGrid.SelectedItems.Clear();
                foreach (Classes.Domain.RequestVM row in rows)
                {
                    try
                    {
                        if (row.DomainObject.Blocking())
                        {
                            myparcelcmd.CurrentItem.ParcelRequests.EditItem(row);
                            myparcelcmd.CurrentItem.Requests.EditItem(row);
                            //row.DomainObject.Parcel = null;
                            row.DomainObject.Parcel = myparcelcmd.CurrentItem.DomainObject;
                            row.DomainObject.Status = myparcelcmd.CurrentItem.Status;
                            myparcelcmd.CurrentItem.Requests.CommitEdit();
                            myparcelcmd.CurrentItem.ParcelRequests.CommitEdit();
                        }
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message, "Поставка заявки в загрузку"); }
                }
            }
            else
            {
                MessageBox.Show("Выделите строки в нижнем списке", "Постановка в загрузку", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void RequestOutButton_Click(object sender, RoutedEventArgs e)
        {
            if ((ParcelRequestDataGrid.SelectedIndex > -1) | (ParcelRequestDataGrid.Items.Count == 1))
            {
                if (ParcelRequestDataGrid.Items.Count == 1) this.ParcelRequestDataGrid.SelectedItems.Add(this.ParcelRequestDataGrid.Items[0]);
                Classes.Domain.RequestVM[] rows = new Classes.Domain.RequestVM[ParcelRequestDataGrid.SelectedItems.Count];
                for (int i = 0; i < ParcelRequestDataGrid.SelectedItems.Count; i++)
                {
                    rows[i] = this.ParcelRequestDataGrid.SelectedItems[i] as Classes.Domain.RequestVM;
                }
                ParcelRequestDataGrid.SelectionChanged -= ParcelRequestDataGrid_SelectionChanged;
                foreach (Classes.Domain.RequestVM row in rows)
                {
                    try
                    {
                        if (row.DomainObject.Blocking())
                        {
                            myparcelcmd.CurrentItem.ParcelRequests.EditItem(row);
                            myparcelcmd.CurrentItem.Requests.EditItem(row);
                            row.DomainObject.ParcelId = null; // не устанавливать через Parcel не обновляется после Refresh
                            row.StoreInform = null;
                            row.DomainObject.Status = CustomBrokerWpf.References.RequestStates.FindFirstItem("Id", 40);
                            myparcelcmd.CurrentItem.Requests.CommitEdit();
                            myparcelcmd.CurrentItem.ParcelRequests.CommitEdit();
                        }
                    }
                    catch (Exception ex)
                    { MessageBox.Show(ex.Message, "Снятие заявки с загрузки"); }
                }
                ParcelRequestDataGrid.SelectionChanged += ParcelRequestDataGrid_SelectionChanged;
            }
            else
            {
                MessageBox.Show("Выделите строку в верхнем списке", "Снятие с загрузки", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        //private void ParcelNumberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //        cellNumberFreeTextBox.Text = (myparcelcmd.CurrentItem?.RequestTotal.CellNumber ?? 0M).ToString("N0");
        //        VolumeTextBox.Text = (myparcelcmd.CurrentItem?.RequestTotal.Volume ?? 0M).ToString("N4");
        //        OfficialWeightTextBox.Text = (myparcelcmd.CurrentItem?.RequestTotal.OfficialWeight ?? 0M).ToString("N4");
        //        actualWeightFreeTextBox.Text = (myparcelcmd.CurrentItem?.RequestTotal.ActualWeight ?? 0M).ToString("N4");
        //        offactWeightFreeTextBox.Text = (myparcelcmd.CurrentItem?.RequestTotal.DifferenceWeight ?? 0M).ToString("N4");
        //        goodValueFreeTextBox.Text = (myparcelcmd.CurrentItem?.RequestTotal.InvoiceDiscount ?? 0M).ToString("N2");
        //}

        private void NoParcelRequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridCellInfo cellinf;
            if (!(e.OriginalSource is DataGrid) || myparcelcmd.CurrentItem == null) return;
            Classes.Domain.RequestVM[] noreadyrowview = new Classes.Domain.RequestVM[NoParcelRequestDataGrid.Items.Count];
            foreach (Classes.Domain.RequestVM rowview in e.AddedItems)
            {
                if (!(rowview is Classes.Domain.RequestVM)) continue;
                rowview.Selected = true;
                if (rowview.ParcelGroup.HasValue)
                {
                    foreach (Classes.Domain.RequestVM viewrow in myparcelcmd.CurrentItem.Requests)
                    {
                        if (viewrow.ParcelGroup.HasValue && rowview.ParcelGroup == viewrow.ParcelGroup && !NoParcelRequestDataGrid.SelectedItems.Contains(viewrow))
                        {
                            NoParcelRequestDataGrid.SelectedItems.Add(viewrow);
                            foreach (DataGridColumn colm in this.NoParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(viewrow, colm);
                                if (!NoParcelRequestDataGrid.SelectedCells.Contains(cellinf)) NoParcelRequestDataGrid.SelectedCells.Add(cellinf);
                            }
                            break;
                        }
                    }
                }
            }
            foreach (Classes.Domain.RequestVM rowview in e.RemovedItems)
            {
                if (!(rowview is Classes.Domain.RequestVM)) continue;
                rowview.Selected = false;
                if (rowview.ParcelGroup.HasValue)
                {
                    foreach (Classes.Domain.RequestVM viewrow in NoParcelRequestDataGrid.SelectedItems)
                    {
                        if (viewrow.ParcelGroup.HasValue && rowview.ParcelGroup == viewrow.ParcelGroup)
                        {
                            NoParcelRequestDataGrid.SelectedItems.Remove(viewrow);
                            foreach (DataGridColumn colm in this.NoParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(viewrow, colm);
                                if (NoParcelRequestDataGrid.SelectedCells.Contains(cellinf)) NoParcelRequestDataGrid.SelectedCells.Remove(cellinf);
                            }
                            break;
                        }
                    }
                }
            }

            //myparcelcmd.CurrentItem.RequestTotal.ResetPre();
            //myparcelcmd.CurrentItem.RequestTotalD.ResetPre();
            //myparcelcmd.CurrentItem.RequestTotalT.ResetPre();
            myparcelcmd.CurrentItem.ResetFree();
            foreach (Classes.Domain.RequestVM rowview in NoParcelRequestDataGrid.SelectedItems)
            {
                if (rowview.Volume.HasValue)
                {
                    //if (rowview.Importer?.Id == 2)
                    //    myparcelcmd.CurrentItem.RequestTotalD.Volume = rowview.Volume.Value;
                    //else if (rowview.Importer?.Id == 1)
                    //    myparcelcmd.CurrentItem.RequestTotalT.Volume = rowview.Volume.Value;
                    //myparcelcmd.CurrentItem.RequestTotal.Volume = rowview.Volume.Value;
                    myparcelcmd.CurrentItem.VolumeFree = -rowview.Volume.Value;
                }
                if (rowview.ActualWeight.HasValue)
                {
                    //if (rowview.Importer?.Id == 2)
                    //    myparcelcmd.CurrentItem.RequestTotalD.ActualWeight = rowview.ActualWeight.Value;
                    //else if (rowview.Importer?.Id == 1)
                    //    myparcelcmd.CurrentItem.RequestTotalT.ActualWeight = rowview.ActualWeight.Value;
                    //myparcelcmd.CurrentItem.RequestTotal.ActualWeight = rowview.ActualWeight.Value;
                    myparcelcmd.CurrentItem.ActualWeightFree = -rowview.ActualWeight.Value;
                }
                if (rowview.OfficialWeight.HasValue)
                {
                    //if (rowview.Importer?.Id == 2)
                    //    myparcelcmd.CurrentItem.RequestTotalD.OfficialWeight = rowview.OfficialWeight.Value;
                    //else if (rowview.Importer?.Id == 1)
                    //    myparcelcmd.CurrentItem.RequestTotalT.OfficialWeight = rowview.OfficialWeight.Value;
                    //myparcelcmd.CurrentItem.RequestTotal.OfficialWeight = rowview.OfficialWeight.Value;
                    myparcelcmd.CurrentItem.OfficialWeightFree = -rowview.OfficialWeight.Value;
                }
                if (rowview.Invoice.HasValue)
                {
                    //if (rowview.Importer?.Id == 2)
                    //    myparcelcmd.CurrentItem.RequestTotalD.Invoice = rowview.Invoice.Value;
                    //else if (rowview.Importer?.Id == 1)
                    //    myparcelcmd.CurrentItem.RequestTotalT.Invoice = rowview.Invoice.Value;
                    //myparcelcmd.CurrentItem.RequestTotal.Invoice = rowview.Invoice.Value;
                    myparcelcmd.CurrentItem.InvoiceFree = -rowview.Invoice.Value;
                }
                if (rowview.InvoiceDiscount.HasValue)
                {
                    //if (rowview.Importer?.Id == 2)
                    //    myparcelcmd.CurrentItem.RequestTotalD.InvoiceDiscount = rowview.InvoiceDiscount.Value;
                    //else if (rowview.Importer?.Id == 1)
                    //    myparcelcmd.CurrentItem.RequestTotalT.InvoiceDiscount = rowview.InvoiceDiscount.Value;
                    //myparcelcmd.CurrentItem.RequestTotal.InvoiceDiscount = rowview.InvoiceDiscount.Value;
                    myparcelcmd.CurrentItem.InvoiceDiscountFree = -rowview.InvoiceDiscount.Value;
                }
                if (rowview.CellNumber.HasValue)
                {
                    //if (rowview.Importer?.Id == 2)
                    //    myparcelcmd.CurrentItem.RequestTotalD.CellNumber = rowview.CellNumber.Value;
                    //else if (rowview.Importer?.Id == 1)
                    //    myparcelcmd.CurrentItem.RequestTotalT.CellNumber = rowview.CellNumber.Value;
                    //myparcelcmd.CurrentItem.RequestTotal.CellNumber = rowview.CellNumber.Value;
                    myparcelcmd.CurrentItem.CellNumberFree = -rowview.CellNumber.Value;
                }
            }
        }
        //private bool mygroupselect;
        private void ParcelRequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridCellInfo cellinf;
            if (!(e.OriginalSource is DataGrid) || myparcelcmd.CurrentItem == null) return;
            foreach (Classes.Domain.RequestVM rowview in e.RemovedItems)
            {
                if (!(rowview is Classes.Domain.RequestVM)) continue;
                rowview.Selected = false;
                if (rowview.ParcelGroup.HasValue)
                {
                    foreach (Classes.Domain.RequestVM itemrow in ParcelRequestDataGrid.SelectedItems)
                    {
                        if (itemrow.ParcelGroup.HasValue && rowview.ParcelGroup == itemrow.ParcelGroup)
                        {
                            ParcelRequestDataGrid.SelectedItems.Remove(itemrow);
                            foreach (DataGridColumn colm in this.ParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(itemrow, colm);
                                if (ParcelRequestDataGrid.SelectedCells.Contains(cellinf)) ParcelRequestDataGrid.SelectedCells.Remove(cellinf);
                            }
                            break;
                        }
                    }
                }
            }
            foreach (Classes.Domain.RequestVM rowview in e.AddedItems)
            {
                if (!(rowview is Classes.Domain.RequestVM)) continue;
                rowview.Selected = true;
                if (rowview.ParcelGroup.HasValue)
                {
                    foreach (Classes.Domain.RequestVM viewrow in myparcelcmd.CurrentItem.ParcelRequests)
                    {
                        if (viewrow.ParcelGroup.HasValue && rowview.ParcelGroup == viewrow.ParcelGroup && !ParcelRequestDataGrid.SelectedItems.Contains(viewrow))
                        {
                            ParcelRequestDataGrid.SelectedItems.Add(viewrow);
                            foreach (DataGridColumn colm in this.ParcelRequestDataGrid.Columns)
                            {
                                cellinf = new DataGridCellInfo(viewrow, colm);
                                if (!ParcelRequestDataGrid.SelectedCells.Contains(cellinf)) ParcelRequestDataGrid.SelectedCells.Add(cellinf);
                            }
                            break;
                        }
                    }
                }
            }
        }
        //private bool ParcelRequestDataGridRowChanged;
        //private void ParcelRequestDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        //{
        //    ParcelRequestDataGridRowChanged = e.EditAction==DataGridEditAction.Commit;
        //}

        //private void ParcelRequestUpDown_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if ((bool)e.NewValue)
        //        viewRequest.Filter = (object item) => { return !(item as Classes.Domain.RequestVM).DomainObject.ParcelId.HasValue && lib.ViewModelViewCommand.ViewFilterDefault(item); };
        //    else
        //        viewRequest.Filter = (object item) => { return false; };
        //}

        private void ParcelRequestDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = e.Row.Item != null && !(e.Row.Item as RequestVM).DomainObject.Blocking();
        }
        private void ParcelRequestDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if ((e.Row.Item as RequestVM).DomainState == lib.DomainObjectState.Unchanged) (e.Row.Item as RequestVM).DomainObject.UnBlocking();
        }
        private void NoParcelRequestDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = e.Row.Item != null && !(e.Row.Item as RequestVM).DomainObject.Blocking();
        }
        private void NoParcelRequestDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if ((e.Row.Item as RequestVM).DomainState == lib.DomainObjectState.Unchanged) (e.Row.Item as RequestVM).DomainObject.UnBlocking();
        }

        private bool mycolumnchanging;//Проверить наличие SortMemberPath у столбцов DataGridTemplateColumn, если сортировка не нужна добавить произвольное значение и установить CanUserSort="False"
        private void ParcelRequestDataGrid_ColumnDisplayIndexChanged(object sender, DataGridColumnEventArgs e)
        {
            if (!mycolumnchanging && this.ParcelRequestDataGrid.IsLoaded)
            {
                DataGridColumn column = null;
                foreach (DataGridColumn item in this.NoParcelRequestDataGrid.Columns)
                {
                    if (string.Equals(item.SortMemberPath, e.Column.SortMemberPath))
                    { column = item; break; }
                }
                if (column != null && column.DisplayIndex != e.Column.DisplayIndex)
                {
                    mycolumnchanging = true;
                    column.DisplayIndex = e.Column.DisplayIndex;
                    mycolumnchanging = false;
                }
            }
        }
        private void NoParcelRequestDataGrid_ColumnDisplayIndexChanged(object sender, DataGridColumnEventArgs e)
        {
            if (!mycolumnchanging && this.NoParcelRequestDataGrid.IsLoaded)
            {
                DataGridColumn column = null;
                foreach (DataGridColumn item in this.ParcelRequestDataGrid.Columns)
                {
                    if (string.Equals(item.SortMemberPath, e.Column.SortMemberPath))
                    { column = item; break; }
                }
                if (column != null && column.DisplayIndex != e.Column.DisplayIndex)
                {
                    mycolumnchanging = true;
                    column.DisplayIndex = e.Column.DisplayIndex;
                    mycolumnchanging = false;
                }
            }
        }

        #region Filter
        //private CustomBrokerWpf.SQLFilter parcelfilter = new SQLFilter("parcel", "AND");
        public SQLFilter ParcelFilter
        {
            get { return myparcelcmd.Filter; }
            set
            {
                    myparcelcmd.Filter = value;
            }
        }
        public bool ParcelIsShowFilter
        {
            set
            {
                this.ParcelFilterButton.IsChecked = value;
            }
            get { return this.ParcelFilterButton.IsChecked.Value; }
        }
        public void ParcelRunFilter()
        {
            if (!myparcelcmd.SaveDataChanges())
                MessageBox.Show("Применение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                myparcelcmd.Refresh.Execute(null);
            }
        }
        private void ParcelSetFilterButtonImage()
        {
            string uribitmap;
            if (myparcelcmd.Filter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (ParcelFilterButton.Content as Image).Source = bi3;
        }

        private void ParcelFilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winParcelFilter") ObjectWin = item;
            }
            if (ParcelFilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new ParcelFilterWin();
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

        private System.Threading.Tasks.Task RequestExcelTask;
        private ExcelImportWin myExcelImportWin;
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
        private async Task RequestExcelProcessingAsync()
        {
            Task<string> t = Task<string>.Run(() => RequestExcelDoProcessing());
            try { await (t); }
            catch { }
            if (t.Exception != null)
            {
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                myExcelImportWin.MessageTextBlock.Text += "Обработка прервана из-за ошибки:" + "\n" + (t.Exception.InnerException == null ? t.Exception.Message : t.Exception.InnerException.Message);
            }
            else
            {
                myExcelImportWin.MessageTextBlock.Foreground = System.Windows.Media.Brushes.Green;
                myExcelImportWin.MessageTextBlock.Text = "Обработка выполнена успешно." + "\n" + t.Result;
            }
        }
        private string RequestExcelDoProcessing()
        {
            foreach (Classes.Domain.RequestVM item in myparcelcmd.CurrentItem.ParcelRequests)
                if (item.Importer == null)
                {
                    throw new Exception("В заявке " + item.StorePointDate + " не указан импортер!");
                }
            ProgressChange(5);
            string path = null, num = null;
            if (myparcelcmd.CurrentItem != null)
            {
                path = CustomBrokerWpf.Properties.Settings.Default.DocFileRoot + myparcelcmd.CurrentItem.ParcelNumber ?? string.Empty;
                if (!Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
            }
            else
                return "Необходимо выбрать перевозку!";
            ProgressChange(7);
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
                view = new ListCollectionView(myparcelcmd.CurrentItem.ParcelRequests.SourceCollection as System.Collections.IList);
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
                        ProgressChange(10);
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
                            ProgressChange(10 + (int)(45 * ((r - 24) / view.Count) / 2));
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
                            ProgressChange(55 + (int)(45 * ((r - 24) / view.Count) / 2));
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

            ProgressChange(100);
            return myparcelcmd.CurrentItem.ParcelRequests.Count.ToString() + " строк обработано";
        }

        private void mainValidation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action != ValidationErrorEventAction.Removed)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        private void MailSMS_Click(object sender, RoutedEventArgs e)
        {
            MailSMSWin win = new MailSMSWin();
            int parcelid = ParcelNumberList.SelectedValue != null ? (ParcelNumberList.SelectedValue as Parcel).Id : 0;
            Classes.MailSMSCommand cmd = new Classes.MailSMSCommand(parcelid);
            win.DataContext = cmd;
            win.Owner = this;
            win.Show();
        }

        private void ParcelDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGrid)?.CurrentItem is Classes.Domain.RequestVM)
            {
                if ((sender as DataGrid).CurrentCell.Column.SortMemberPath == "StorePointDate")
                {
                    RequestNewWin newWin = null;
                    DataGrid dg = sender as DataGrid;
                    foreach (Window item in this.OwnedWindows)
                    {
                        if (item.Name == "winRequestNew")
                        {
                            if ((item.DataContext as Classes.Domain.RequestVMCommand).VModel.Id == (dg.CurrentItem as Classes.Domain.RequestVM).Id)
                                newWin = item as RequestNewWin;
                        }
                    }
                    if (newWin == null)
                    {
                        newWin = new RequestNewWin();
                        newWin.Owner = this;

                        newWin.thisStoragePointValidationRule.RequestId = (dg.CurrentItem as Classes.Domain.RequestVM).Id;
                        Classes.Domain.RequestVMCommand cmd = new Classes.Domain.RequestVMCommand((dg.CurrentItem as Classes.Domain.RequestVM), myparcelcmd.CurrentItem.ParcelRequests);
                        newWin.DataContext = cmd;
                        newWin.Show();
                    }
                    else
                    {
                        newWin.Activate();
                        if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
                    }
                }
                e.Handled = true;
            }
        }
        private void ParcelRequestDataGrid_SizeChanged(DataGridColumn column)
        {
            int position = this.ParcelRequestDataGrid.Columns.IndexOf(column);
            if ((this.ParcelRequestDataGrid.IsLoaded && column.ActualWidth != this.NoParcelRequestDataGrid.Columns[position].ActualWidth) || column.ActualWidth > this.NoParcelRequestDataGrid.Columns[position].ActualWidth)
                this.NoParcelRequestDataGrid.Columns[position].Width = column.ActualWidth;
        }
        private void NoParcelRequestDataGrid_SizeChanged(DataGridColumn column)
        {
            int position = this.NoParcelRequestDataGrid.Columns.IndexOf(column);
            if ((this.NoParcelRequestDataGrid.IsLoaded && column.ActualWidth != this.ParcelRequestDataGrid.Columns[position].ActualWidth) || column.ActualWidth > this.ParcelRequestDataGrid.Columns[position].ActualWidth)
                this.ParcelRequestDataGrid.Columns[position].Width = column.ActualWidth;
        }
        private ListCollectionView mystatuses;
        public ListCollectionView Statuses
        {
            get
            {
                if (mystatuses == null)
                {
                    mystatuses = new ListCollectionView(CustomBrokerWpf.References.RequestStates);
                    mystatuses.Filter = (item) => { return (item as lib.ReferenceSimpleItem).Id < 50; };
                }
                return mystatuses;
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

        private void SpecificationDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGrid)?.CurrentItem is Classes.Specification.SpecificationVM)
            {
                if ((sender as DataGrid).CurrentCell.Column.SortMemberPath == "CFPR" || (sender as DataGrid).CurrentCell.Column.SortMemberPath == "Importer.Name")
                {
                    SpecificationWin newWin = null;
                    DataGrid dg = sender as DataGrid;
                    foreach (Window item in this.OwnedWindows)
                    {
                        if (item.Name == "winSpecification")
                        {
                            if ((item.DataContext as Classes.Specification.SpecificationVMCommand).VModel.Id == (dg.CurrentItem as Classes.Specification.SpecificationVM).Id)
                                newWin = item as SpecificationWin;
                        }
                    }
                    if (newWin == null)
                    {
                        newWin = new SpecificationWin();
                        newWin.Owner = this;

                        Classes.Specification.SpecificationVMCommand cmd = new Classes.Specification.SpecificationVMCommand((dg.CurrentItem as Classes.Specification.SpecificationVM), myparcelcmd.CurrentItem.Specifications);
                        newWin.DataContext = cmd;
                        newWin.Show();
                    }
                    else
                    {
                        newWin.Activate();
                        if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
                    }
                }
                e.Handled = true;
            }
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

        //INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

    }
}
