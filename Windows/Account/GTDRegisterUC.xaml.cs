using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
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
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.WindowsAccount
{
    /// <summary>
    /// Логика взаимодействия для GTDRegisterUC.xaml
    /// </summary>
    public partial class GTDRegisterUC : UserControl
    {
        private Window myhost;
        private List<Window> mychildwindows;
        private lib.BindingDischarger mybinddisp;
        private GTDRegisterViewCommander mycmd;

        public GTDRegisterUC()
        {
            InitializeComponent();
        }
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is GTDRegisterViewCommander)
            {
                myhost = null;
                FrameworkElement win = this;
                while (myhost == null & win != null)
                    if (win.Parent is Window) myhost = win.Parent as Window;
                    else win = win.Parent as FrameworkElement;
                if (myhost != null)
                {
                    mychildwindows = (myhost as lib.Interfaces.IMainWindow).ListChildWindow;
                    mybinddisp = new lib.BindingDischarger(myhost, new DataGrid[] { this.MainDataGrid });
                    mycmd = e.NewValue as GTDRegisterViewCommander;
                    mycmd.CancelEdit = mybinddisp.CancelEdit;
                    mycmd.EndEdit = mybinddisp.EndEdit;
                    foreach(DataGridColumn column in MainDataGrid.Columns)
                        switch(column.SortMemberPath)
                        {
                            case "CC":
                                if (mycmd.ServiceType== "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "CostLogistics":
                                if (mycmd.ServiceType == "ТЭО") column.Header = "Итого затраты";
                                break;
                            case "CostPer":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "CostTotal":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "DTSumRub":
                                if (mycmd.ServiceType == "ТД") column.Visibility = Visibility.Collapsed;
                                break;
                            case "MarkupBU":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "Rate":
                                if(mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "ProfitAlgE":
                            case "ProfitAlgR":
                            case "ProfitDiff":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "SellingRate":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "Specification.Declaration.CBRate":
                                if (mycmd.ServiceType == "ТЭО") column.Header = "Курс ДТ";
                                break;
                            case "Specification.Declaration.Fee":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            //case "Specification.GTLSCur":
                            //    if (mycmd.ServiceType == "ТД") column.Visibility = Visibility.Collapsed;
                            //    break;
                            //case "Specification.GTLSRate":
                            //    if (mycmd.ServiceType == "ТД") column.Visibility = Visibility.Collapsed;
                            //    break;
                            case "Specification.MFK":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "Specification.MFKWithoutRate":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "Specification.MFKRate":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "Specification.Declaration.Tax":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                            case "Specification.Declaration.VAT":
                                if (mycmd.ServiceType == "ТЭО") column.Visibility = Visibility.Collapsed;
                                break;
                        }
                }
                else
                {
                    MessageBox.Show("Не удалось определить Host для GTDRegisterUC!", nameof(PaymentRegisterUC), MessageBoxButton.OK, MessageBoxImage.Error);
                    this.DataContext = null;
                }
            }
        }

        private bool myisempty;
        private double myscroloffset;
        private void Scrol_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (this.MainDataGrid.HasItems)
            {
                if (myisempty)
                {
                    ScrollViewer scrol = System.Windows.Media.VisualTreeHelper.GetChild(System.Windows.Media.VisualTreeHelper.GetChild(this.MainDataGrid, 0), 0) as ScrollViewer;
                    scrol.ScrollToHorizontalOffset(myscroloffset);
                    myisempty = false;
                    e.Handled = true;
                }
                else if(e.HorizontalChange!=0D)
                    myscroloffset = e.HorizontalOffset;
            }
            else if (!myisempty)
            {
                this.MainScrollViewer.ScrollToHorizontalOffset(myscroloffset);
                myisempty = true;
            }
        }
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (myisempty && e.ExtentWidthChange == 0 && !this.MainDataGrid.HasItems)
                myscroloffset = e.HorizontalOffset;
        }

        #region Filter
        private void AgentFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.AgentFilter != null && !mycmd.AgentFilter.FilterOn) mycmd.AgentFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("AgentFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CCFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void ClientFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.ClientFilter != null && !mycmd.ClientFilter.FilterOn) mycmd.ClientFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("ClientFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CostLogisticsFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void CostPerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void CostTotalFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void DDSpidyFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("DDSpidyFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void DeclarationFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.DeclarationNumberFilter != null && !mycmd.DeclarationNumberFilter.FilterOn) mycmd.DeclarationNumberFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("DeclarationNumberFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void DTRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("DTRateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void DTSumRubFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("DTSumRubFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void FeeFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("FeeFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void GTLSFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("GTLSFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void GTLSCurFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("GTLSCurFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void GTLSDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("GTLSDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void GTLSRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("GTLSRateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void MarkupAlgFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void MarkupBUFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void MarkupTotalFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void MFKFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("MFKFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void MFKRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("MFKRateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void MFKWithoutRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("MFKWithoutRateFilterPopup") as Popup;
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
        private void PariFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("PariFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ProfitFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void ProfitabilityFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void ProfitAlgEFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void ProfitAlgRFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void ProfitDiffFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void RateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("RateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void SellingFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void SellingDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("SellingDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void SellingWithoutRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void SellingRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void SLFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void SLRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void SLWithoutRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void TaxFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("TaxFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void TotalSumFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("TotalSumFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void VATFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("VATFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void VATPayFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void VolumeFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void VolumeProfitFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {

        }
        private void WestGateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("WestGateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void WestGateRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("WestGateRateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void WestGateWithoutRateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("WestGateWithoutRateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        #endregion

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems != null)
                foreach (lib.Interfaces.ISelectable item in e.RemovedItems.OfType<lib.Interfaces.ISelectable>())
                    item.Selected = false;
            if (e.AddedItems != null)
                foreach (lib.Interfaces.ISelectable item in e.AddedItems.OfType<lib.Interfaces.ISelectable>())
                    item.Selected = true;
        }
        #region Details
        private DataGrid GetDataGridDetail(object item)
        {
            DataGrid grid = null;
            DataGridRow row = (DataGridRow)this.MainDataGrid.ItemContainerGenerator.ContainerFromItem(item);
            if (row != null && row.DetailsVisibility == Visibility.Visible)
            {
                var detailpresenter = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild((VisualTreeHelper.GetChild(row, 0) as Border), 0), 1);
                grid = VisualTreeHelper.GetChild(detailpresenter, 0) as DataGrid;
            }
            return grid;
        }
        private DataGrid GetDataGridDetail(DataGridRow row)
        {
            DataGrid grid = null;
            if (row != null && row.DetailsVisibility == Visibility.Visible)
            {
                var detailpresenter = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild((VisualTreeHelper.GetChild(row, 0) as Border), 0), 1);
                grid = VisualTreeHelper.GetChild(detailpresenter, 0) as DataGrid;
            }
            return grid;
        }
        private string ColumnSortMatch(string name)
        {
            string match;
            switch (name)
            {
                case "Client":
                    match = "Client.Name";
                    break;
                case "Specification.Declaration.CBRate":
                    match = "BuyRate";
                    break;
                case "Specification.DDSpidy":
                    match = "DDSpidy";
                    break;
                case "Specification.Parcel.RateDate":
                    match = "GTD.Specification.Parcel.RateDate";
                    break;
                case "Specification.Parcel.UsdRate":
                    match = "GTD.Specification.Parcel.UsdRate";
                    break;
                case "Specification.GTLS":
                    match = "GTLS";
                    break;
                case "Specification.GTLSCur":
                    match = "GTLSCur";
                    break;
                case "Specification.Pari":
                    match = "Pari";
                    break;
                case "Specification.Declaration.TotalSum":
                    match = "DTSum";
                    break;
                case "Specification.Fee":
                    match = "Fee";
                    break;
                case "Specification.MFK":
                    match = "MFK";
                    break;
                case "Specification.MFKRate":
                    match = "MFKRate";
                    break;
                case "Specification.MFKWithoutRate":
                    match = "MFKWithoutRate";
                    break;
                case "Specification.Rate":
                    match = "VAT";
                    break;
                case "Specification.Tax":
                    match = "Tax";
                    break;
                case "Specification.WestGate":
                    match = "WestGate";
                    break;
                case "Specification.WestGateRate":
                    match = "WestGateRate";
                    break;
                case "Specification.WestGateWithoutRate":
                    match = "WestGateWithoutRate";
                    break;
                default:
                    match = name;
                    break;
            }
            return match;
        }
        private void DataGrid_ColumnDisplayIndexChanged(object sender, DataGridColumnEventArgs e)
        {
            if (this.MainDataGrid.IsLoaded)
            {
                foreach (var item in this.MainDataGrid.ItemsSource)
                {
                    DataGrid grid = GetDataGridDetail(item);
                    if (grid != null)
                    {
                        DataGridColumn column = null;
                        string sort = ColumnSortMatch(e.Column.SortMemberPath);
                        foreach (DataGridColumn cl in grid.Columns)
                        {
                            if (string.Equals(cl.SortMemberPath, sort))
                            { column = cl; break; }
                        }
                        if (column != null && column.DisplayIndex != e.Column.DisplayIndex)
                            column.DisplayIndex = e.Column.DisplayIndex;
                    }
                }
            }
        }
        private void DataGrid_LoadingRowDetails(object sender, DataGridRowDetailsEventArgs e)
        {
            //Синхронизация ширины столбцов
            DataGrid grid = GetDataGridDetail(e.Row);
            for (int i = 0; i < this.MainDataGrid.Columns.Count; i++)
                if (this.MainDataGrid.Columns[i].DisplayIndex == 0)
                    grid.Columns[i].Width = this.MainDataGrid.Columns[i].ActualWidth - grid.RowHeaderWidth - 5;
                else
                {
                    grid.Columns[i].Visibility = this.MainDataGrid.Columns[i].Visibility;
                    grid.Columns[i].Width = this.MainDataGrid.Columns[i].ActualWidth;
                }
            System.ComponentModel.DependencyPropertyDescriptor textDescr = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(DataGridColumn.ActualWidthProperty, typeof(DataGridColumn));
            if (textDescr != null)
            {
                foreach (DataGridColumn column in this.MainDataGrid.Columns)
                {
                    textDescr.AddValueChanged(column, delegate
                    {
                        if (column.DisplayIndex >= 0 && this.MainDataGrid.IsLoaded)
                        {
                            int position = this.MainDataGrid.Columns.IndexOf(column);
                            if (this.MainDataGrid.IsLoaded && column.ActualWidth != grid.Columns[position].ActualWidth)
                                if (this.MainDataGrid.Columns[position].DisplayIndex == 0)
                                    grid.Columns[position].Width = column.ActualWidth - grid.RowHeaderWidth - 5;
                                else
                                    grid.Columns[position].Width = column.ActualWidth;
                        }
                    });
                }
            }
        }
		#endregion

	}
}
