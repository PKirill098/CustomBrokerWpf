using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Storage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class WarehouseItemWin : Window
    {
        private WarehouseCommand mycmd;
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mybindingdischanger; } }

        public WarehouseItemWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { AddressDataGrid, ContactDataGrid, ContactPointDataGrid });
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is WarehouseCommand)
            {
                mycmd = e.NewValue as WarehouseCommand;
                mycmd.EndEdit = mybindingdischanger.EndEdit;
                mycmd.CancelEdit = mybindingdischanger.CancelEdit;
            }
            else
                mycmd = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mycmd != null && !(mybindingdischanger.EndEdit() && mycmd.SaveDataChanges()))
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
                this.Owner?.Activate();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
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
            Classes.Domain.AgentAddressVM item = this.AddressDataGrid.SelectedItem as Classes.Domain.AgentAddressVM;
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
            Classes.Domain.ContactVM item = this.ContactDataGrid.SelectedItem as Classes.Domain.ContactVM;
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
            Classes.Domain.ContactPointVM item = this.ContactPointDataGrid.SelectedItem as Classes.Domain.ContactPointVM;
            (mycmd.VModel.Contacts.CurrentItem as Classes.Domain.ContactVM).Points.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            (mycmd.VModel.Contacts.CurrentItem as Classes.Domain.ContactVM).Points.CommitEdit();
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
    }
}
