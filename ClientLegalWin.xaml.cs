using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ClientWin.xaml
    /// </summary>
    public partial class ClientLegalWin : Window, System.ComponentModel.INotifyPropertyChanged//, IFiltredWindow
    {
        ItemFilter[] thisfilter = new ItemFilter[0];
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mybindingdischanger; } }
        private Classes.Domain.CustomerLegalVMCommand mycmd;

        public ClientLegalWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { AddressDataGrid, ContactDataGrid, ContactPointDataGrid, RecipientDataGrid });// AliasCustomerDataGrid,
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mycmd = this.DataContext as Classes.Domain.CustomerLegalVMCommand;
            mycmd.EndEdit = mybindingdischanger.EndEdit;
            mycmd.CancelEdit = mybindingdischanger.CancelEdit;
            DataLoad();
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
                if(referenceDS.tableManagerGroup.Count==0)
                {
                ReferenceDSTableAdapters.ManagerGroupAdapter thisManagerGroupAdapter = new ReferenceDSTableAdapters.ManagerGroupAdapter();
                thisManagerGroupAdapter.Fill(referenceDS.tableManagerGroup);
                }
                if (referenceDS.DeliveryType.Count == 0)
                {
                    ReferenceDSTableAdapters.DeliveryType thisDeliveryTypeAdapter = new ReferenceDSTableAdapters.DeliveryType();
                    thisDeliveryTypeAdapter.Fill(referenceDS.DeliveryType);
                }
                if (referenceDS.tablePaymentType.Count == 0)
                {
                    ReferenceDSTableAdapters.PaymentTypeAdapter thisPaymentTypeAdapter = new ReferenceDSTableAdapters.PaymentTypeAdapter();
                    thisPaymentTypeAdapter.Fill(referenceDS.tablePaymentType);
                }
                if (referenceDS.tableLegalEntity.Count == 0) referenceDS.LegalEntityRefresh();
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
            mycmd = this.DataContext as Classes.Domain.CustomerLegalVMCommand;
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

        //private void Aliases_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = true;
        //    e.Handled = true;
        //}
        //private void Aliases_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    AliasVM item = this.AliasCustomerDataGrid.SelectedItem as AliasVM;
        //    mycmd.VModel.Aliases.EditItem(item);
        //    item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
        //    mycmd.VModel.Aliases.CommitEdit();
        //}
        private void Addresses_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Addresses_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomerAddressVM item = this.AddressDataGrid.SelectedItem as CustomerAddressVM;
            mycmd.VModel.Addresses.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.VModel.Addresses.CommitEdit();
        }
        private void Contacts_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Contacts_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomerContactVM item = this.ContactDataGrid.SelectedItem as CustomerContactVM;
            mycmd.VModel.Contacts.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.VModel.Contacts.CommitEdit();
        }
        private void Points_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Points_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ContactPointVM item = this.ContactPointDataGrid.SelectedItem as ContactPointVM;
            (mycmd.VModel.Contacts.CurrentItem as CustomerContactVM).Points.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            (mycmd.VModel.Contacts.CurrentItem as CustomerContactVM).Points.CommitEdit();
        }
        private void Recipients_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Recipients_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomerLegalVM item = this.RecipientDataGrid.SelectedItem as CustomerLegalVM;
            mycmd.VModel.Addresses.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.VModel.Addresses.CommitEdit();
        }

        private void AgentNameList_GotFocus(object sender, RoutedEventArgs e)
        {
            //AliasCustomerDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            AddressDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            ContactDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            ContactPointDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
        }

        private void DelAgentButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить клиента?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //object curitem = (CollectionViewSource.GetDefaultView(this.CustomerNameList.ItemsSource) as BindingListCollectionView).CurrentItem;
                //if (curitem != null) (curitem as DataRowView).Delete();
            }
        }

        private void JoinDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                //BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.CustomerNameList.ItemsSource) as BindingListCollectionView;
                //if (view.IsAddingNew) view.CommitNew();
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

        private void RecipientDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (CustomerNameList.SelectedItem != null)
            //{
            //    if((CustomerNameList.SelectedItem as DataRowView).Row.RowState==DataRowState.Added)
            //    {
            //        if (!SaveChanges()) return;
            //    }
            //    RecipientWin rwin = new RecipientWin();
            //    rwin.Owner = this;
            //    rwin.CustomerID = ((CustomerNameList.SelectedItem as DataRowView).Row as CustomerDS.tableCustomerRow).customerID;
            //    rwin.Show();
            //    if (RecipientDataGrid.CurrentItem != null)
            //    {
            //        rwin.CurrentRecipientName = ((RecipientDataGrid.CurrentItem as DataRowView).Row as CustomerDS.tableCustomerRecipientRow).recipientName;
            //    }
            //    else
            //    {
            //        BindingListCollectionView recipientView = CollectionViewSource.GetDefaultView(rwin.RecipientNameList.ItemsSource) as BindingListCollectionView;
            //        RecipientDS.tableRecipientRow newRow = (recipientView.AddNew() as DataRowView).Row as RecipientDS.tableRecipientRow;
            //        newRow.customerId = rwin.CustomerID;
            //        recipientView.MoveCurrentToLast();
            //    }
            //}
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

        //private void FilterButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Window ObjectWin = null;
        //    foreach (Window item in this.OwnedWindows)
        //    {
        //        if (item.Name == "winClientFilter") ObjectWin = item;
        //    }
        //    if (FilterButton.IsChecked.Value)
        //    {
        //        if (ObjectWin == null)
        //        {
        //            ObjectWin = new ClientFilterWin();
        //            ObjectWin.Owner = this;
        //            ObjectWin.Show();
        //        }
        //        else
        //        {
        //            ObjectWin.Activate();
        //            if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
        //        }
        //    }
        //    else
        //    {
        //        if (ObjectWin != null)
        //        {
        //            ObjectWin.Close();
        //        }
        //    }
        //}

        private void ComboBox_Loaded(object sender, RoutedEventArgs e) //Bug ComboBoxItem
        { (sender as ComboBox).IsDropDownOpen = true; (sender as ComboBox).IsDropDownOpen = false; }

        private int? mystoragepointfilter;
        public int? StoragePointFilter
        {
            set
            {
                mystoragepointfilter = value;
                PropertyChangedNotification("StoragePointFilter");
            }
            get { return mystoragepointfilter; }
        }
        private void Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FastFilterRun();
            }
        }
        private void FastFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FastFilterRun();
        }
        private void FastFilterRun()
        {
            if(mystoragepointfilter.HasValue)
            {
                //foreach(DataRowView item in this.CustomerNameList.ItemsSource)
                //{
                //    CustomerDS.tableCustomerRow row = item.Row as CustomerDS.tableCustomerRow;
                //    if (row.customerID == mystoragepointfilter.Value)
                //        CollectionViewSource.GetDefaultView(this.CustomerNameList.ItemsSource).MoveCurrentTo(item);
                //}
            }
        }

        //INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
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
