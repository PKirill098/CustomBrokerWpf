using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using KirillPolyanskiy.CustomBrokerWpf.WindowsAccount;
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
using System.Windows.Shapes;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class AccountMainWin : Window, lib.Interfaces.IMainWindow
    {
        public AccountMainWin()
        {
            InitializeComponent();
            mychildwindows = new List<Window>();
        }

        private List<Window> mychildwindows;
        public List<Window> ListChildWindow
        { get { return mychildwindows; } }
        private PaymentRegisterViewCommander mypaydcmd;
        private PaymentRegisterViewCommander mypaytcmd;
        private GTDRegisterViewCommander mygtddcmd;
        private GTDRegisterViewCommander mygtdtcmd;
        private GTDRegisterViewCommander myteodcmd;
        private GTDRegisterViewCommander myteotcmd;
        private Classes.Domain.ParcelCurItemCommander myparcelcmd;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
                mypaydcmd = new PaymentRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2));
                this.PaymentDeliveryGrid.DataContext = mypaydcmd;
        }
        private void TabItem2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mypaytcmd == null)
            {
                mypaytcmd = new PaymentRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1));
                this.PaymentTradeGrid.DataContext = mypaytcmd;
            }
        }
        private void TabItem3_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mygtddcmd == null)
            {
                mygtddcmd = new GTDRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 2),"ТД");
                this.GTDDeliveryGrid.DataContext = mygtddcmd;
            }
        }
        private void TabItem4_GotFocus(object sender, RoutedEventArgs e)
        {
            if (mygtdtcmd == null)
            {
                mygtdtcmd = new GTDRegisterViewCommander(CustomBrokerWpf.References.Importers.FindFirstItem("Id", 1),"ТД");
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
        private void TabItemParcel_GotFocus(object sender, RoutedEventArgs e)
        {
            if (myparcelcmd == null)
            {
                myparcelcmd = new Classes.Domain.ParcelCurItemCommander();
                ParcelGrid.DataContext = myparcelcmd;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int i = 0, c1;
            while (i < mychildwindows.Count)
            {
                c1 = mychildwindows.Count;
                mychildwindows[i].Close();
                i = i + 1 - c1 + mychildwindows.Count;
            }
            if (mychildwindows.Count > 0) e.Cancel = true;
            else
            {
                e.Cancel = this.PaymentDeliveryGrid.HostWindow_Close() || this.PaymentTradeGrid.HostWindow_Close() || this.GTDDeliveryGrid.HostWindow_Close() || this.GTDTradeGrid.HostWindow_Close() || this.TEODeliveryGrid.HostWindow_Close() || this.TEOTradeGrid.HostWindow_Close();
                if (!e.Cancel)
                {
                    mypaydcmd?.Filter.Dispose();
                    mypaytcmd?.Filter.Dispose();
                    mygtddcmd?.Filter.Dispose();
                    mygtdtcmd?.Filter.Dispose();
                    myteodcmd?.Filter.Dispose();
                    myteotcmd?.Filter.Dispose();
                }
            }
        }
        
        #region Parcel
        private void MailSMS_Click(object sender, RoutedEventArgs e)
        {
            MailSMSWin win = new MailSMSWin();
            int parcelid = ParcelNumberList.SelectedValue != null ? (ParcelNumberList.SelectedValue as Classes.Domain.Parcel).Id : 0;
            Classes.MailSMSCommand cmd = new Classes.MailSMSCommand(parcelid);
            win.DataContext = cmd;
            win.Owner = this;
            win.Show();
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
        private void HistoryOpen_Click(object sender, RoutedEventArgs e)
        {
            RequestHistoryWin newHistory = new RequestHistoryWin();
            if ((sender as Button).Tag is Classes.Domain.RequestVM)
            {
                Classes.Domain.Request request = ((sender as Button).Tag as Classes.Domain.RequestVM).DomainObject;
                Classes.Domain.RequestHistoryViewCommand cmd = new Classes.Domain.RequestHistoryViewCommand(request);
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
                    item.DomainObject.DocFolderOpen();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Папка документов");
                }
            }
        }
        #endregion
    }
}
