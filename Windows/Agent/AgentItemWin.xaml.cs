using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для AgentItemWin.xaml
    /// </summary>
    public partial class AgentItemWin : Window
    {
        private Classes.Domain.AgentCommand mycmd;
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mybindingdischanger; } }

        public AgentItemWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { AliasDataGrid, AddressDataGrid, BrandDataGrid, ContactDataGrid, ContactPointDataGrid, ContractDataGrid });
        }
        
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Classes.Domain.AgentCommand)
            {
                mycmd = e.NewValue as Classes.Domain.AgentCommand;
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

        private void Aliases_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.AliasDataGrid.CurrentItem is Classes.Domain.AgentAliasVM;
            e.Handled = true;
        }
        private void Aliases_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить псевдоним?", "Удаление псевдонима", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Classes.Domain.AgentAliasVM item = this.AliasDataGrid.SelectedItem as Classes.Domain.AgentAliasVM;
                mycmd.VModel.Aliases.EditItem(item);
                item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
                mycmd.VModel.Aliases.CommitEdit();
            }
        }

        private void Brands_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.BrandDataGrid.CurrentItem is Classes.Domain.AgentBrandVM;
            e.Handled = true;
        }
        private void Brands_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить торговую марку?", "Удаление торговой марки", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Classes.Domain.AgentBrandVM item = this.BrandDataGrid.SelectedItem as Classes.Domain.AgentBrandVM;
                mycmd.VModel.Brands.EditItem(item);
                item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
                mycmd.VModel.Brands.CommitEdit();
            }
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

        private void Contracts_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Classes.Domain.ContractVM item = this.ContractDataGrid.SelectedItem as Classes.Domain.ContractVM;
            e.CanExecute = item.DomainState == DataModelClassLibrary.DomainObjectState.Added;
            e.Handled = true;
        }
        private void Contracts_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Classes.Domain.ContractVM item = this.ContractDataGrid.SelectedItem as Classes.Domain.ContractVM;
            mycmd.VModel.Contracts.EditItem(item);
            item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
            mycmd.VModel.Contracts.CommitEdit();
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
