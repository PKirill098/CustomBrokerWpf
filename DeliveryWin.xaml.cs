using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using libui = KirillPolyanskiy.WpfControlLibrary;
using System.Linq;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для DeliveryWin.xaml
    /// </summary>
    public partial class DeliveryWin : Window
    {
        DeliveryCarViewCommand mycarscmd;
        DeliveryCarryViewCommand mycarrycmd;
        private lib.BindingDischarger mycarsbinddisp;
        private lib.BindingDischarger mycarrybinddisp;
        public DeliveryWin()
        {
            InitializeComponent();
            mycarscmd = new DeliveryCarViewCommand();
            mycarsbinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.CarsDataGrid });
            mycarscmd.EndEdit = mycarsbinddisp.EndEdit;
            mycarscmd.CancelEdit = mycarsbinddisp.CancelEdit;
            mycarrycmd = new DeliveryCarryViewCommand();
            mycarrybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.CarryDataGrid });
            mycarrycmd.EndEdit = mycarrybinddisp.EndEdit;
            mycarrycmd.CancelEdit = mycarrybinddisp.CancelEdit;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CarsTabItem.DataContext = mycarscmd;
            this.CarryTabItem.DataContext = mycarrycmd;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mycarrybinddisp.EndEdit() & mycarsbinddisp.EndEdit())
            {
                bool isdirtycars = false, isdirtycarry = false;
                foreach (Classes.Domain.DeliveryCarVM item in mycarscmd.Items.SourceCollection) isdirtycars = isdirtycars | item.DomainObject.IsDirty;
                foreach (Classes.Domain.DeliveryCarryVM item in mycarrycmd.Items.SourceCollection) isdirtycarry = isdirtycarry | item.DomainObject.IsDirty;
                if (isdirtycars | isdirtycarry)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!mycarscmd.SaveDataChanges())
                        {
                            this.Activate();
                            this.CarsTabItem.IsSelected = true;
                            if (MessageBox.Show("\nИзменения в ДС не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                mycarscmd.Reject.Execute(null);
                        }
                        if (!mycarrycmd.SaveDataChanges())
                        {
                            this.Activate();
                            this.CarryTabItem.IsSelected = true;
                            if (MessageBox.Show("\nИзменения в филиалах не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                mycarrycmd.Reject.Execute(null);
                        }
                    }
                    else
                    {
                        mycarscmd.Reject.Execute(null);
                        mycarrycmd.Reject.Execute(null);
                    }
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                {
                    mycarscmd.Reject.Execute(null);
                    mycarrycmd.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                (App.Current.MainWindow as MainWindow).Activate();
            }
        }

        private void CloseButton_Clic(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ParceltoExcelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ParcelFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.DeliveryCarryViewCommand cmd = this.CarryTabItem.DataContext as Classes.Domain.DeliveryCarryViewCommand;
            if (cmd.ParcelFilter != null && cmd.ParcelFilter.Items.Count==0) cmd.ParcelFilter.Fill();
            Popup ppp = this.CarryDataGrid.FindResource("ParcelFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RequestFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.DeliveryCarryViewCommand cmd = this.CarryTabItem.DataContext as Classes.Domain.DeliveryCarryViewCommand;
            if (cmd.RequestFilter != null) cmd.RequestFilter.Fill();
            Popup ppp = this.CarryDataGrid.FindResource("RequestFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CustomerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.CarryDataGrid.FindResource("CustomerFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }

        private void CustomerLegalFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Classes.Domain.DeliveryCarryViewCommand cmd = this.CarryTabItem.DataContext as Classes.Domain.DeliveryCarryViewCommand;
            if (cmd.CustomerFilter!= null) cmd.CustomerLegalFilter.ExecRefresh();
            Popup ppp = this.CarryDataGrid.FindResource("CustomerLegalFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }

        private void ImporterFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.CarryDataGrid.FindResource("ImporterFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }

        private void ServiceTypeFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.CarryDataGrid.FindResource("ServiceTypeFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }

        private void ShipmentDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.CarryDataGrid.FindResource("ShipmentDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
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
    }
}
