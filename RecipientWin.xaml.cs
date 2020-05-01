using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для RecipientWin.xaml
    /// </summary>
    public partial class RecipientWin : Window
    {
        private int customerID;// для распознования что уже открыта
        internal int CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }
        private Classes.Domain.RecipientCurrentCommand mycmd;
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mybindingdischanger; } }

        public RecipientWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { AddressDataGrid, ContactDataGrid, ContactPointDataGrid });
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                if (referenceDS.tableAddressType.Count == 0)
                {
                    ReferenceDSTableAdapters.AddressTypeAdapter thisAddressTypeAdapter = new ReferenceDSTableAdapters.AddressTypeAdapter();
                    thisAddressTypeAdapter.Fill(referenceDS.tableAddressType);
                }
                CollectionViewSource addressTypeVS = this.FindResource("keyAddressTypeVS") as CollectionViewSource;
                addressTypeVS.Source = new DataView(referenceDS.tableAddressType, string.Empty, string.Empty, DataViewRowState.Unchanged | DataViewRowState.ModifiedCurrent);
                if (referenceDS.tableTown.Count == 0)
                {
                    ReferenceDSTableAdapters.TownAdapter thisTownAdapter = new ReferenceDSTableAdapters.TownAdapter();
                    thisTownAdapter.Fill(referenceDS.tableTown);
                }
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

        private void ToolBar_GotFocus(object sender, RoutedEventArgs e)
        {
            AddressDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            ContactDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            ContactPointDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
        }

        private void ComboBox20_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            if (obj != null)
            {
                var myTextBox = (TextBox)obj.Template.FindName("PART_EditableTextBox", obj);
                if (myTextBox != null)
                {
                    myTextBox.MaxLength = 20;
                }
            }
        }
        private void ComboBox50_Loaded(object sender, RoutedEventArgs e)
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
        private void ComboBox100_Loaded(object sender, RoutedEventArgs e)
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

        private void Grid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                if (e.Error.Exception != null)
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
                else
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
            }
        }
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            mycmd = this.DataContext as Classes.Domain.RecipientCurrentCommand;
            mycmd.EndEdit = mybindingdischanger.EndEdit;
            mycmd.CancelEdit = mybindingdischanger.CancelEdit;
        }
    }
}
