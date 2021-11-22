using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Specification;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для SpecificationWin.xaml
    /// </summary>
    public partial class SpecificationWin : Window
    {
        private lib.BindingDischarger mybinddisp;
        public SpecificationWin()
        {
            InitializeComponent();
            mybinddisp = new lib.BindingDischarger(this, new DataGrid[]{ mainDataGrid });
        }
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue!=null)
            {
                lib.ViewModelBaseCommand cmd = e.NewValue as lib.ViewModelBaseCommand;
                cmd.CancelEdit = mybinddisp.CancelEdit;
                cmd.EndEdit = mybinddisp.EndEdit;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            lib.ViewModelBaseCommand cmd = this.DataContext as lib.ViewModelBaseCommand;
            if (!(mybinddisp.EndEdit() && cmd.SaveDataChanges()))
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и могут быть потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            { this.Owner.Activate(); /*(App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this); App.Current.MainWindow.Activate(); */}
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                CustomerLegal legal = e.AddedItems[0] as CustomerLegal;
                if (mainDataGrid.SelectedItems.Count > 0 )
                {
                    foreach (SpecificationDetailVM row in mainDataGrid.SelectedItems.OfType<SpecificationDetailVM>())
                    {
                        row.Client = legal;
                    }
                }
                else if(mainDataGrid.SelectedCells.Count > 0)
                {
                    foreach (DataGridCellInfo cell in mainDataGrid.SelectedCells)
                        if(cell.Column.SortMemberPath == "Client.Name" & cell.Item is SpecificationDetailVM)
                            (cell.Item as SpecificationDetailVM ).Client = legal;
                }
            }
        }
        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Copy_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (mainDataGrid.SelectedCells.Count == 1)
            {
                DataGridCellInfo cell = mainDataGrid.SelectedCells[0];
                if (cell.IsValid && cell.Column.SortMemberPath == "Client.Name" & cell.Item is SpecificationDetailVM && (cell.Item as SpecificationDetailVM).Client!=null)
                {
                    DataObject data = new DataObject(typeof(CustomerLegal), (cell.Item as SpecificationDetailVM).Client); //Client must be [Serializable]
                    Clipboard.SetDataObject(data,false);
                }
            }
        }
        private void Paste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //"Юр. лицо\r\nPetrova-Q\r\n"

            //IDataObject myRetrievedObject = Clipboard.GetDataObject();
            //e.CanExecute = myRetrievedObject.GetDataPresent(typeof(CustomerLegal));
            //if (Clipboard.ContainsText() && mainDataGrid.SelectedCells.Count > 0)
            //{
            //    DataGridCellInfo cell = mainDataGrid.SelectedCells[0];
            //    e.CanExecute = cell.Column.SortMemberPath == "Client.Name" & cell.Item is SpecificationDetailVM;
            //}
            if(!e.CanExecute)
                e.CanExecute = Clipboard.ContainsText() && Clipboard.GetText().StartsWith("Юр. лицо") && (mainDataGrid.SelectedItems.Count > 0 || mainDataGrid.SelectedCells.Count<DataGridCellInfo>((DataGridCellInfo item) => { return item.Column.SortMemberPath != "Client.Name"; }) == 0);
        }
        private void Paste_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            if (Clipboard.ContainsText() && mainDataGrid.SelectedCells.Count > 0)
            {
                string text = Clipboard.GetText();
                if(text.StartsWith("Юр. лицо"))
                {
                    text = text.Substring(10, text.Length - 12);
                    SpecificationVMCommand cmd = this.DataContext as SpecificationVMCommand;
                    CustomerLegal legal = cmd.VModel.DomainObject.CustomerLegalsList.FirstOrDefault<CustomerLegal>((CustomerLegal item) => { return item.Name == text; });
                    if (mainDataGrid.SelectedItems.Count > 0)
                        foreach (SpecificationDetailVM row in mainDataGrid.SelectedItems.OfType<SpecificationDetailVM>())
                            row.Client = legal;
                    else
                        foreach (DataGridCellInfo cell in mainDataGrid.SelectedCells)
                            if (cell.Column.SortMemberPath == "Client.Name" & cell.Item is SpecificationDetailVM)
                                (cell.Item as SpecificationDetailVM).Client = legal;
                }
            //e.Handled = true;
            }
        }
    }
}
