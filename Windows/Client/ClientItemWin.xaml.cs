using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class ClientItemWin : Window
    {
        private Classes.Domain.CustomerCommand mycmd;
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mybindingdischanger; } }

        public ClientItemWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { CustomerLegalDataGrid, AliasCustomerDataGrid, AddressDataGrid, ContactDataGrid, ContactPointDataGrid, RecipientDataGrid });
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mycmd!=null && !(mybindingdischanger.EndEdit() && mycmd.SaveDataChanges()))
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
                this.Owner.Activate();
            }
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Classes.Domain.CustomerCommand)
            {
                mycmd = e.NewValue as Classes.Domain.CustomerCommand;
                mycmd.EndEdit = mybindingdischanger.EndEdit;
                mycmd.CancelEdit = mybindingdischanger.CancelEdit;
            }
            else
                mycmd = null;
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

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomerLegalVM item = this.CustomerLegalDataGrid.SelectedItem as CustomerLegalVM;
            mycmd.VModel.Legals.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.VModel.Legals.CommitEdit();
        }
        private void Aliases_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Aliases_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AliasVM item = this.AliasCustomerDataGrid.SelectedItem as AliasVM;
            mycmd.VModel.Aliases.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.VModel.Aliases.CommitEdit();
        }
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
            mycmd.VModel.Legals.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.VModel.Legals.CommitEdit();
        }

        private void CustomerLegalOpen_Click(object sender, RoutedEventArgs e)
        {
            mybindingdischanger.EndEdit();
            Classes.Domain.CustomerLegalVM legal = (sender as Button).Tag as Classes.Domain.CustomerLegalVM;
            if (legal == null)
            {
                legal = mycmd.VModel.Legals.AddNew() as Classes.Domain.CustomerLegalVM;
                legal.Customer = mycmd.VModel;
            }
            Classes.Domain.CustomerLegalVMCommand cmd = new Classes.Domain.CustomerLegalVMCommand(legal, mycmd.VModel.Legals);
            ClientLegalWin win = new ClientLegalWin();
            win.DataContext = cmd;
            win.Show();
        }

        private void RecipientDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RecipientWin rwin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winRecipient" && (item as RecipientWin).CustomerID == mycmd.VModel.Id) rwin = item as RecipientWin;
            }
            if (rwin == null)
            {

                rwin = new RecipientWin();
                rwin.Owner = this;
                rwin.CustomerID = mycmd.VModel.Id;
                RecipientCurrentCommand rcmd = new RecipientCurrentCommand(mycmd.VModel, null);
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
}
