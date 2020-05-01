using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for CountriesWin.xaml
    /// </summary>
    public partial class CountriesWin : Window, IViewModelWindous
    {
        public CountriesWin()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!(vmEndEdit() & (this.DataContext as Classes.CountriesVM).SaveDataChanges()))
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                (App.Current.MainWindow as DataModelClassLibrary.Interfaces.IMainWindow).ListChildWindow.Remove(this);
            }
        }

        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool vmEndEdit()
        {
            bool isEnd = this.mainDataGrid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Cell, true);
            isEnd = isEnd & this.mainDataGrid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Row, true);
            return isEnd;
        }
        public void vmCancelEdit()
        {
            this.mainDataGrid.CancelEdit();
        }
    }
}
