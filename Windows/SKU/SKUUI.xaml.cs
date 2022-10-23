﻿using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using lib=KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для SKUUI.xaml
    /// </summary>
    public partial class SKUUI : UserControl
    {
        WarehouseRUViewCommader mycmd;
        lib.BindingDischarger mybinddisp;
        public object SelectedItems
        { get { return this.MainDataGrid.SelectedItems; } }
        public SKUUI()
        {
            InitializeComponent();
            mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.MainDataGrid });
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
    }
}