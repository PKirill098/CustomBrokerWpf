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
    /// Логика взаимодействия для ContractorWin.xaml
    /// </summary>
    public partial class ContractorWin : Window
    {
        Domain.References.ContractorListVM vm;

        public ContractorWin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new Domain.References.ContractorListVM();
            vm.EndEdit = delegate () { return this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true) & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true); };
            vm.CancelEdit = delegate () { this.mainDataGrid.CancelEdit(DataGridEditingUnit.Cell); this.mainDataGrid.CancelEdit(DataGridEditingUnit.Row); };
            this.DataContext = vm;
            CommandBinding binding = new CommandBinding(DataGrid.DeleteCommand);
            binding.Executed += vm.BindingDelete;
            this.mainDataGrid.CommandBindings.Add(binding);
            //this.mainDataGrid.ItemsSource = vm.Contractors;
        }

        private void Binding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!(this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true) & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true) & vm.SaveDataChanges()))
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
        }

        private void DataLoad()
        {
            try
            {
                System.Windows.Data.CollectionViewSource recipientCollectionView = new System.Windows.Data.CollectionViewSource();
                recipientCollectionView.Source = References.Contractors;
                recipientCollectionView.View.Filter = delegate (object item) { return (item as Domain.References.Contractor).Id > 0; };
                this.mainDataGrid.ItemsSource = recipientCollectionView.View;
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
        private bool SaveChanges()
        {
            return true;
        }
    }
}
