using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ClientWin.xaml
    /// </summary>
    public partial class ClientWin : Window, IFiltredWindow, System.ComponentModel.INotifyPropertyChanged
    {
        private Classes.Domain.CustomerCurrentCommand mycmd;
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mybindingdischanger; } }

        public ClientWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { CustomerLegalDataGrid, AliasCustomerDataGrid, AddressDataGrid, ContactDataGrid, ContactPointDataGrid, RecipientDataGrid });
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
            mycmd = new CustomerCurrentCommand();
            mycmd.EndEdit = mybindingdischanger.EndEdit;
            mycmd.CancelEdit = mybindingdischanger.CancelEdit;
            this.DataContext = mycmd;
        }
        private void DataLoad()
        {
            try
            {
                ReferenceDS referenceDS = this.FindResource("keyReferenceDS") as ReferenceDS;
                if (referenceDS.ContactPointTypeTb.Rows.Count == 0)
                {
                    ReferenceDSTableAdapters.ContactPointTypeAdapter AdapterContactPointType = new ReferenceDSTableAdapters.ContactPointTypeAdapter();
                    AdapterContactPointType.Fill(referenceDS.ContactPointTypeTb);
                }
                if (referenceDS.tableManagerGroup.Count == 0)
                {
                    ReferenceDSTableAdapters.ManagerGroupAdapter thisManagerGroupAdapter = new ReferenceDSTableAdapters.ManagerGroupAdapter();
                    thisManagerGroupAdapter.Fill(referenceDS.tableManagerGroup);
                }
                CollectionViewSource managerGroupVS = this.FindResource("keyManagerGroupVS") as CollectionViewSource;
                managerGroupVS.Source = new DataView(referenceDS.tableManagerGroup, string.Empty, string.Empty, DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent);
                if (referenceDS.DeliveryType.Count == 0)
                {
                    ReferenceDSTableAdapters.DeliveryType thisDeliveryTypeAdapter = new ReferenceDSTableAdapters.DeliveryType();
                    thisDeliveryTypeAdapter.Fill(referenceDS.DeliveryType);
                }
                CollectionViewSource deliveryTypeVS = this.FindResource("keyDeliveryTypeVS") as CollectionViewSource;
                deliveryTypeVS.Source = new DataView(referenceDS.DeliveryType, string.Empty, string.Empty, DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent);
                if (referenceDS.tablePaymentType.Count == 0)
                {
                    ReferenceDSTableAdapters.PaymentTypeAdapter thisPaymentTypeAdapter = new ReferenceDSTableAdapters.PaymentTypeAdapter();
                    thisPaymentTypeAdapter.Fill(referenceDS.tablePaymentType);
                }
                CollectionViewSource paymentTypeVS = this.FindResource("keyPaymentTypeVS") as CollectionViewSource;
                paymentTypeVS.Source = new DataView(referenceDS.tablePaymentType, string.Empty, string.Empty, DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent);
                if (referenceDS.tableLegalEntity.Count == 0) referenceDS.LegalEntityRefresh();
                CollectionViewSource accountSettlementVS = this.FindResource("keyAccountSettlementVS") as CollectionViewSource;
                accountSettlementVS.Source = referenceDS.tableLegalEntity.DefaultView;
                if (referenceDS.tableTown.Count == 0)
                {
                    ReferenceDSTableAdapters.TownAdapter thisTownAdapter = new ReferenceDSTableAdapters.TownAdapter();
                    thisTownAdapter.Fill(referenceDS.tableTown);
                }
                CollectionViewSource townVS = this.FindResource("keyTownVS") as CollectionViewSource;
                townVS.Source = new DataView(referenceDS.tableTown, string.Empty, string.Empty, DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent);
                if (referenceDS.tableAddressType.Count == 0)
                {
                    ReferenceDSTableAdapters.AddressTypeAdapter thisAddressTypeAdapter = new ReferenceDSTableAdapters.AddressTypeAdapter();
                    thisAddressTypeAdapter.Fill(referenceDS.tableAddressType);
                }
                CollectionViewSource addressTypeVS = this.FindResource("keyAddressTypeVS") as CollectionViewSource;
                addressTypeVS.Source = new DataView(referenceDS.tableAddressType, string.Empty, string.Empty, DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent);
                if (referenceDS.tableContactType.Count == 0)
                {
                    ReferenceDSTableAdapters.ContactTypeAdapter thisContactTypeAdapter = new ReferenceDSTableAdapters.ContactTypeAdapter();
                    thisContactTypeAdapter.Fill(referenceDS.tableContactType);
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                    {
                        errs.Append(sqlerr.Message + "\n");
                    }
                    MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    DataLoad();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!(mybindingdischanger.EndEdit() && mycmd.SaveDataChanges()))
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel) (App.Current.MainWindow as MainWindow)?.ListChildWindow.Remove(this);
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AliasDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action != ValidationErrorEventAction.Removed)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        private void ComboBoxPointType_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            if (obj != null)
            {
                var myTextBox = (TextBox)obj.Template.FindName("PART_EditableTextBox", obj);
                if (myTextBox != null)
                {
                    myTextBox.MaxLength = 100;
                }
            }
        }
        private void ComboBox15_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            if (obj != null)
            {
                var myTextBox = (TextBox)obj.Template.FindName("PART_EditableTextBox", obj);
                if (myTextBox != null)
                {
                    myTextBox.MaxLength = 50;
                }
            }
        }
        private void ComboBox_Loaded(object sender, RoutedEventArgs e) //Bug ComboBoxItem
        { (sender as ComboBox).IsDropDownOpen = true; (sender as ComboBox).IsDropDownOpen = false; }

        private void Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mycmd.FastFilter.Execute(null);
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomerLegalVM item = this.CustomerLegalDataGrid.SelectedItem as CustomerLegalVM;
            mycmd.CurrentItem.Legals.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.CurrentItem.Legals.CommitEdit();
        }
        private void Aliases_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Aliases_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AliasVM item = this.AliasCustomerDataGrid.SelectedItem as AliasVM;
            mycmd.CurrentItem.Aliases.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.CurrentItem.Aliases.CommitEdit();
        }
        private void Addresses_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Addresses_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomerAddressVM item = this.AddressDataGrid.SelectedItem as CustomerAddressVM;
            mycmd.CurrentItem.Addresses.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.CurrentItem.Addresses.CommitEdit();
        }
        private void Contacts_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Contacts_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomerContactVM item = this.ContactDataGrid.SelectedItem as CustomerContactVM;
            mycmd.CurrentItem.Contacts.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.CurrentItem.Contacts.CommitEdit();
        }
        private void Points_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Points_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ContactPointVM item = this.ContactPointDataGrid.SelectedItem as ContactPointVM;
            (mycmd.CurrentItem.Contacts.CurrentItem as CustomerContactVM).Points.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            (mycmd.CurrentItem.Contacts.CurrentItem as CustomerContactVM).Points.CommitEdit();
        }
        private void Recipients_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Recipients_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomerLegalVM item = this.RecipientDataGrid.SelectedItem as CustomerLegalVM;
            mycmd.CurrentItem.Legals.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.CurrentItem.Legals.CommitEdit();
        }

        private void CustomerLegalOpen_Click(object sender, RoutedEventArgs e)
        {
            mybindingdischanger.EndEdit();
            Classes.Domain.CustomerLegalVM legal = (sender as Button).Tag as Classes.Domain.CustomerLegalVM;
            if (legal == null)
            {
                legal = mycmd.CurrentItem.Legals.AddNew() as Classes.Domain.CustomerLegalVM;
                legal.Customer = mycmd.CurrentItem;
            }
            Classes.Domain.CustomerLegalVMCommand cmd = new Classes.Domain.CustomerLegalVMCommand(legal, mycmd.CurrentItem.Legals);
            ClientLegalWin win = new ClientLegalWin();
            win.DataContext = cmd;
            win.Show();
        }

        ItemFilter[] thisfilter = new ItemFilter[0];
        public bool IsShowFilter
        {
            get { return this.FilterButton.IsChecked.Value; }
            set { this.FilterButton.IsChecked = value; }
        }
        public ItemFilter[] Filter
        {
            get
            {
                return thisfilter;
            }
            set
            {
                thisfilter = value;
                ListCollectionView view = this.CustomerNameList.ItemsSource as ListCollectionView;
                view.Filter = (object item) =>
                {
                    bool where = true;
                    string[] ids;
                    CustomerVM client = item as CustomerVM;
                    foreach (ItemFilter filter in thisfilter)
                    {
                        if (!where) break;
                        if (!(filter is ItemFilter)) continue;
                        switch (filter.PropertyName)
                        {
                            case "AliasCustomer":
                                where = false;
                                foreach (AliasVM alias in client.Aliases.OfType< AliasVM>())
                                {
                                    if (alias.Name.IndexOf(filter.Value) > -1)
                                    {
                                        where = true;
                                        break;
                                    }
                                }
                                break;
                            case "CustomerRecipient":
                                where = false;
                                foreach (RecipientVM rsp in client.Recipients.OfType<RecipientVM>())
                                {
                                    if (rsp.Name.IndexOf(filter.Value) > -1 || rsp.FullName?.IndexOf(filter.Value) > -1)
                                    {
                                        where = true;
                                        break;
                                    }
                                }
                                break;
                            case "managergroupID":
                                ids = filter.Value.Split(',');
                                foreach(string id in ids)
                                    if(!(client.ManagerGroup.HasValue && client.ManagerGroup.Value==int.Parse(id)))
                                    {
                                        where = false;
                                        break;
                                    }
                                break;
                            case "paytypeID":
                                ids = filter.Value.Split(',');
                                foreach (string id in ids)
                                    if (!(client.PayType.HasValue && client.PayType.Value == int.Parse(id)))
                                    {
                                        where = false;
                                        break;
                                    }
                                break;
                            case "deliverytypeID":
                                ids = filter.Value.Split(',');
                                foreach (string id in ids)
                                    if (!(client.DeliveryType.HasValue && client.DeliveryType.Value == int.Parse(id)))
                                    {
                                        where = false;
                                        break;
                                    }
                                break;
                            case "customerID":
                                where = client.Id.ToString() == filter.Value;
                                break;
                            case "customerDayEntry":
                                if (filter.Operation == "Between")
                                    where = client.DayEntry >= DateTime.Parse(filter.Value.Substring(0, filter.Value.IndexOf(' '))) && client.DayEntry < DateTime.Parse(filter.Value.Substring(filter.Value.IndexOf(' ') + 1));
                                else if (filter.Operation == ">")
                                    where = client.DayEntry >= DateTime.Parse(filter.Value);
                                else if (filter.Operation == "<")
                                    where = client.DayEntry < DateTime.Parse(filter.Value);
                                break;
                            case "customerRecommend":
                                where = (client.Recommend?.IndexOf(filter.Value)??-1) > -1;
                                break;
                            case "customerNoteSpecial":
                                where = (client.NoteSpecial?.IndexOf(filter.Value)??-1) > -1;
                                break;
                            case "Town":
                                where = false;
                                foreach (CustomerAddressVM adr in client.Addresses.OfType<CustomerAddressVM>())
                                {
                                    if (adr.Town == filter.Value)
                                    {
                                        where = true;
                                        break;
                                    }
                                }
                                break;
                            case "Locality":
                                where = false;
                                foreach (CustomerAddressVM adr in client.Addresses.OfType<CustomerAddressVM>())
                                {
                                    if ((adr.Locality?.IndexOf(filter.Value)??-1) > -1)
                                    {
                                        where = true;
                                        break;
                                    }
                                }
                                break;
                            case "FIO":
                                where = false;
                                foreach (CustomerContactVM cnt in client.Contacts.OfType<CustomerContactVM>())
                                {
                                    if ((cnt.Name?.IndexOf(filter.Value)??-1) > -1 || (cnt.SurName?.IndexOf(filter.Value)??-1) > -1 || (cnt.ThirdName?.IndexOf(filter.Value)??-1) > -1)
                                    {
                                        where = true;
                                        break;
                                    }
                                }
                                break;
                            case "PointValue":
                                where = false;
                                foreach (CustomerContactVM cnt in client.Contacts.OfType<CustomerContactVM>())
                                {
                                    foreach (ContactPointVM pnt in cnt.Points.OfType<ContactPointVM>())
                                        if (pnt.Value.IndexOf(filter.Value) > -1)
                                        {
                                            where = true;
                                            break;
                                        }
                                    if (where) break;
                                }
                                break;
                        }
                    }
                    return where;
                };
                view.MoveCurrentToFirst();
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

        private void AgentNameList_GotFocus(object sender, RoutedEventArgs e)
        {
            AliasCustomerDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            AddressDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            ContactDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            ContactPointDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (CollectionViewSource.GetDefaultView(this.CustomerNameList.ItemsSource) as BindingListCollectionView).AddNew();
                this.DayEntryTextBox.Text = DateTime.Today.ToShortDateString();
                BindingExpression bex = this.DayEntryTextBox.GetBindingExpression(TextBox.TextProperty);
                bex.UpdateSource();
            }
            catch (NoNullAllowedException)
            {
                MessageBox.Show("Одно из обязательных для заполнения полей оставлено пустым. \n Введите значение в поле.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DelAgentButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить клиента?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                object curitem = (CollectionViewSource.GetDefaultView(this.CustomerNameList.ItemsSource) as BindingListCollectionView).CurrentItem;
                if (curitem != null) (curitem as DataRowView).Delete();
            }
        }

        private void JoinDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.CustomerNameList.ItemsSource) as BindingListCollectionView;
                if (view.IsAddingNew) view.CommitNew();
            }
            catch (NoNullAllowedException)
            {
                MessageBox.Show("Одно из обязательных для заполнения полей оставлено пустым. \n Введите значение в поле.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RecipientDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CustomerNameList.SelectedItem != null)
            {
                RecipientWin rwin = null;
                foreach (Window item in this.OwnedWindows)
                {
                    if (item.Name == "winRecipient" && (item as RecipientWin).CustomerID == mycmd.CurrentItem.Id) rwin = item as RecipientWin;
                }
                if (rwin == null)
                {

                    rwin = new RecipientWin();
                    rwin.Owner = this;
                    rwin.CustomerID = mycmd.CurrentItem.Id;
                    RecipientCurrentCommand rcmd = new RecipientCurrentCommand(mycmd.CurrentItem, null);
                    rwin.DataContext = rcmd;
                    rwin.Show();
                }
                else
                {
                    rwin.Activate();
                    if (rwin.WindowState == WindowState.Minimized) rwin.WindowState = WindowState.Normal;
                }
                if (RecipientDataGrid.CurrentItem != null)
                    rwin.RecipientNameList.Text = (RecipientDataGrid.CurrentItem as RecipientVM).Name;
                else
                    (rwin.DataContext as RecipientCurrentCommand).Add.Execute(null);
            }
        }

        //INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        private void CustomerNameList_GotFocus(object sender, RoutedEventArgs e)
        {
            mybindingdischanger.EndEdit();
        }

        //private string PointFormat(string pointName,string pointValue)
        //{
        //    if (pointValue.Length > 0)
        //    {
        //        try
        //        {
        //            string pointtemp = string.Empty;
        //            ReferenceDS ds = this.FindResource("keyReferenceDS") as ReferenceDS;
        //            ReferenceDS.ContactPointTypeTbDataTable pointtype = ds.ContactPointTypeTb;
        //            ReferenceDS.ContactPointTypeTbRow typerow = pointtype.FindBypointName(pointName);
        //            if (typerow != null) pointtemp = typerow.pointtemplate;
        //            else pointtemp = string.Empty;

        //            if (pointtemp == "telnumber")
        //            {
        //                char s;
        //                byte p = 0;
        //                bool isClose = false;
        //                bool isOpen = false;
        //                StringBuilder ss = new StringBuilder();
        //                char[] charValue = charReverse(pointValue.ToCharArray());
        //                for (int i = 0; i < charValue.Length; i++)
        //                {
        //                    s = charValue[i];
        //                    //if ((s >= '0' & s <= '9')
        //                    //    | (s == '(' & !isClose) | (s == ')' & !isOpen)
        //                    //    | (s == '-' & (p == 5 | p == 2) & !isOpen))
        //                    if (((s != '-') & (s != '(') & (s != ' ')) || (s == '-' & (p == 5 | p == 2) & !isOpen) || ((s == '(') & (p == 15)))
        //                    {
        //                        p++;
        //                        if (s == ')')
        //                        {
        //                            isOpen = true;
        //                            ss.Append(' ');
        //                            p++;
        //                        }
        //                        if (s == '(')
        //                        {
        //                            isClose = true;
        //                            ss.Append('(');
        //                            s = ' ';
        //                            p++;
        //                        }
        //                        if ((p == 15) & (s != '(') & !isClose)
        //                        {
        //                            ss.Append("( ");
        //                            isClose = true;
        //                            p = 17;
        //                        }
        //                        if (p == 10 & s != ')' & !isOpen)
        //                        {
        //                            ss.Append(" )");
        //                            isOpen = true;
        //                            p = 12;
        //                        }
        //                        if (p == 6 & s != '-' & !isOpen)
        //                        {
        //                            ss.Append("-");
        //                            p = 7;
        //                        }
        //                        if (p == 3 & s != '-' & !isOpen)
        //                        {
        //                            ss.Append("-");
        //                            p = 4;
        //                        }
        //                        ss.Append(s);
        //                    }
        //                }
        //                if (ss.Length == 9) ss.Append(" )594( 7+");
        //                else if (ss.Length == 14) ss.Append(isClose ? " 7+" : "( 7+");
        //                else if (ss.Length == 16) ss.Append("7+");
        //                else if ((ss.Length == 17) & ss.ToString().EndsWith("8")) ss.Replace("8", "7+", 16, 1);
        //                else if ((ss.Length > 16) & !ss.ToString().EndsWith("+")) ss.Append("+");
        //                charValue = new char[ss.Length];
        //                ss.CopyTo(0, charValue, 0, ss.Length);
        //                pointValue = string.Concat(charReverse(charValue));
        //            }
        //        }
        //        catch { }
        //    }
        //    pointtemp = string.Empty;
        //    object[] values = { Binding.DoNothing, pointValue };
        //    return values;

        //    return pointValue;
        //}
    }
}
