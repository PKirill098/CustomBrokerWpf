using KirillPolyanskiy.DataModelClassLibrary.Metadata;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class ClientNewWin : Window, IFiltredWindow, System.ComponentModel.INotifyPropertyChanged
    {
        //INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        private MetadataDataGrid mymetadatadatagrid;
        private Classes.Domain.CustomerViewCommand mycmd;
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;

        public ClientNewWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { MainDataGrid });
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mymetadatadatagrid = new MetadataDataGrid("ClientNewWinMainDataGrid", CustomBrokerWpf.References.CurrentUser, MainDataGrid);
            mymetadatadatagrid.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            mymetadatadatagrid.Set();
            mycmd = new Classes.Domain.CustomerViewCommand();
            mycmd.EndEdit = mybindingdischanger.EndEdit;
            mycmd.CancelEdit = mybindingdischanger.CancelEdit;
            this.DataContext = mycmd;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mycmd.Filter.Dispose();
            if (!(mybindingdischanger.EndEdit() && mycmd.SaveDataChanges()))
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow)?.ListChildWindow.Remove(this);
                mymetadatadatagrid.Save();
            }
        }

        #region Filter
        private void CustomerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.CustomerFillFilter != null && !mycmd.CustomerFillFilter.FilterOn) mycmd.CustomerFillFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("CustomerFilterPopup") as Popup;
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
        private void INNFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.INNFilter != null && !mycmd.INNFilter.FilterOn) mycmd.INNFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("INNFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ParcelCountFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("ParcelCountFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ParcelLastDateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("ParcelLastDateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        #endregion

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool IsShowFilter
        {
            get { return this.FilterButton.IsChecked.Value; }
            set { this.FilterButton.IsChecked = value; }
        }
        public ItemFilter[] Filter
        {
            get
            {
                return mycmd.FilterWin;
            }
            set
            {
                mycmd.FilterWin = value;

                //ItemFilter states = value.FirstOrDefault((ItemFilter filter) => { return filter?.PropertyName == "State"; });
                //if (states != null)
                //{
                //    string[] items = states.Value.Split(',');//.Replace("1","208")
                //    mycmd.Filter.SetList(mycmd.Filter.FilterWhereId, "state", items);
                //}
                //else
                //    mycmd.Filter.SetList(mycmd.Filter.FilterWhereId, "state", new string[0]);
                mycmd.FilterRunNew.Execute(null);

                mycmd.Items.Filter = mycmd.OverallFilterOn;
            }
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winClientFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new ClientFilterWin();
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerOpen(null);
        }

        private void CustomerOpen(Classes.Domain.CustomerVM client)
        {
            if (client == null)
            {
                mycmd.Add.Execute(null);
                client = mycmd.Items.CurrentItem as Classes.Domain.CustomerVM;
            }

            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winClientItem" && (item.DataContext as Classes.Domain.CustomerCommand).VModel.DomainObject == client.DomainObject) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                Classes.Domain.CustomerCommand cmd = new Classes.Domain.CustomerCommand(client, mycmd.Items);
                ObjectWin = new ClientItemWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = cmd;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        private void CustomerLegalOpen(Classes.Domain.CustomerLegalVM legal)
        {
            mybindingdischanger.EndEdit();
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winClientLegal" && (item.DataContext as Classes.Domain.CustomerLegalVMCommand).VModel.DomainObject == legal.DomainObject) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                Classes.Domain.CustomerLegalVMCommand cmd = new Classes.Domain.CustomerLegalVMCommand(legal, (mycmd.Items.CurrentItem as Classes.Domain.CustomerVM).Legals);
                ObjectWin = new ClientLegalWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = cmd;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }

        private void MainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is DataGrid && e.OriginalSource is TextBlock)
            {
                object item = (e.OriginalSource as TextBlock).GetBindingExpression(TextBlock.TextProperty).ResolvedSource;
                string property = (e.OriginalSource as TextBlock).GetBindingExpression(TextBlock.TextProperty).ResolvedSourcePropertyName;
                switch (item.GetType().Name)
                {
                    case "CustomerVM":
                        if (mycmd.Items.CurrentItem is Classes.Domain.CustomerVM & MainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
                        {
                            CustomerOpen(item as Classes.Domain.CustomerVM);
                        }
                        break;
                    case "CustomerLegalVM":
                        if (mycmd.Items.CurrentItem is Classes.Domain.CustomerVM & MainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
                        {
                            CustomerLegalOpen(item as Classes.Domain.CustomerLegalVM);
                        }
                        break;
                }
                //switch (property)
                //{
                //    case "Name":
                //        if (mycmd.Items.CurrentItem is Classes.Domain.CustomerVM & MainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
                //        {
                //            CustomerOpen(mycmd.Items.CurrentItem as Classes.Domain.CustomerVM);
                //        }
                //        break;
                //    case "":

                //        break;
                //}
            }
        }

    }
}
