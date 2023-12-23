using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
	/// <summary>
	/// Логика взаимодействия для SKUUI.xaml
	/// </summary>
	public partial class SKUUI : UserControl
    {
        private Window mywindow;
        WarehouseRUViewCommader mycmd;
        lib.BindingDischarger mybinddisp;
        public DataGrid SKUDataGrid
        { get { return this.MainDataGrid; } }
        public object SelectedItems
        { get { return this.MainDataGrid.SelectedItems; } }
        public SKUUI()
        {
            InitializeComponent();
            mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.MainDataGrid });
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mywindow = null;
            FrameworkElement element = this;
            while (mywindow == null & element != null)
                if (element.Parent is Window) mywindow = element.Parent as Window;
                else
                {
                    element = element.Parent as FrameworkElement;
                }
        }
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                mycmd = e.NewValue as WarehouseRUViewCommader;
                mycmd.CancelEdit = mybinddisp.CancelEdit;
                mycmd.EndEdit = mybinddisp.EndEdit;
            }
            else
                mycmd = null;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems != null)
                foreach (lib.Interfaces.ISelectable item in e.RemovedItems.OfType<lib.Interfaces.ISelectable>())
                    item.Selected = false;
            if (e.AddedItems != null)
                foreach (lib.Interfaces.ISelectable item in e.AddedItems.OfType<lib.Interfaces.ISelectable>())
                    item.Selected = true;
        }

        private void ImporterFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("ImporterFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void LegalFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.CustomerFilter != null && !mycmd.CustomerFilter.FilterOn) mycmd.CustomerFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("CustomerFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ParcelFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.ParcelFilter != null && !mycmd.ParcelFilter.FilterOn) mycmd.ParcelFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("ParcelFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ReceiptedFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("ReceiptedFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ShippedFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("ShippedFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void StatusFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("StatusFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestsIdNumberFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void StorageIdNumberFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void AgentFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            //if (mycmd.AgentFilter != null && !mycmd.AgentFilter.FilterOn) mycmd.AgentFilter?.FillAsync();
            //Popup ppp = this.MainDataGrid.FindResource("AgentFilterPopup") as Popup;
            //ppp.PlacementTarget = (UIElement)sender;
            //ppp.IsOpen = true;
            //e.Handled = true;
        }
        private void BrandFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            //if (mycmd.BrandFilter != null && !mycmd.BrandFilter.FilterOn) mycmd.BrandFilter?.FillAsync();
            //Popup ppp = this.MainDataGrid.FindResource("BrandFilterPopup") as Popup;
            //ppp.PlacementTarget = (UIElement)sender;
            //ppp.IsOpen = true;
            //e.Handled = true;
        }

        private void OfficialWeightFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }

        private void ActualWeightFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }

        private void VolumeFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }

        private void CellNumberFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }

        private void MainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as DataGrid)?.CurrentItem is WarehouseRUVM)
            {
                if (e.OriginalSource is TextBlock && ((sender as DataGrid).CurrentCell.Column.SortMemberPath == "Legal.Name"))
                {
                    CustomerLegal legal = ((sender as DataGrid)?.CurrentItem as WarehouseRUVM).Legal as CustomerLegal;

                    ClientLegalWin win = null;
                    foreach (Window item in mywindow.OwnedWindows)
                    {
                        if (item.Name == "winClientLegal" && (item.DataContext as CustomerLegalVMCommand).VModel.Id == legal.Id)
                        {
                            win = item as ClientLegalWin;
                            break;
                        }
                    }
                    if (win == null)
                    {
                        CustomerLegalVMCommand cmd = new CustomerLegalVMCommand(new CustomerLegalVM(legal), null);
                        win = new ClientLegalWin();
                        win.DataContext = cmd;
                        win.Owner = mywindow;
                        win.Show();
                    }
                    else
                    {
                        win.Activate();
                        if (win.WindowState == WindowState.Minimized) win.WindowState = WindowState.Normal;
                    }
                }
                e.Handled = true;
            }
        }

    }
}
