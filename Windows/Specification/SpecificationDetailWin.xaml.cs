using KirillPolyanskiy.CustomBrokerWpf.Classes.Specification;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для SpecificationDetailWin.xaml
    /// </summary>
    public partial class SpecificationDetailWin : Window
    {
        private Classes.Specification.SpecificationDetailViewCommand mycmd;
        private lib.BindingDischarger mybinddisp;
        public SpecificationDetailWin()
        {
            InitializeComponent();
            mycmd = new Classes.Specification.SpecificationDetailViewCommand();
            mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.MainDataGrid });
            mycmd.EndEdit = mybinddisp.EndEdit;
            mycmd.CancelEdit = mybinddisp.CancelEdit;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = mycmd;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mybinddisp.EndEdit())
            {
                bool isdirty = false;
                foreach (SpecificationDetailVM item in mycmd.Items.SourceCollection) isdirty = isdirty | item.DomainObject.IsDirty;
                if (isdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!mycmd.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения в ДС не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                mycmd.Reject.Execute(null);
                        }
                    }
                    else
                        mycmd.Reject.Execute(null);
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
                    mycmd.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                mycmd.Dispose();
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                (App.Current.MainWindow as MainWindow).Activate();
            }
        }

        private void BranchFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.BranchFilter != null && !mycmd.BranchFilter.FilterOn) mycmd.BranchFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("BranchFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void BrandFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.BrandFilter != null && !mycmd.BrandFilter.FilterOn) mycmd.BrandFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("BrandFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CertificateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.CertificateFilter != null && !mycmd.CertificateFilter.FilterOn) mycmd.CertificateFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("CertificateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ClientFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.ClientFilter != null && !mycmd.ClientFilter.FilterOn) mycmd.ClientFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("ClientFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CountryRuFilterFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.CountryRuFilter != null && !mycmd.CountryRuFilter.FilterOn) mycmd.CountryRuFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("CountryRuFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void GenderFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("GenderFilterPopup") as Popup;
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
        private void VendorCodeFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.VendorCodeFilter != null && !mycmd.VendorCodeFilter.FilterOn) mycmd.VendorCodeFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("VendorCodeFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void LegalFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.LegalFilter != null && !mycmd.LegalFilter.FilterOn) mycmd.LegalFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("LegalFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
    }
}
