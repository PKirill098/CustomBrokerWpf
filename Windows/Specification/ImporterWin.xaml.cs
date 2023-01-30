using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ImporterWin.xaml
    /// </summary>
    public partial class ImporterWin : Window
    {
        public ImporterWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { MainDataGrid });
        }

        private Classes.Domain.ImporterViewCommand mycmd;
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mybindingdischanger; } }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mycmd = new Classes.Domain.ImporterViewCommand();
            mycmd.EndEdit = mybindingdischanger.EndEdit;
            mycmd.CancelEdit = mybindingdischanger.CancelEdit;
            this.DataContext = mycmd;
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
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ImporterVM item = this.MainDataGrid.SelectedItem as ImporterVM;
            mycmd.Delete.Execute(item);
        }
    }
}
