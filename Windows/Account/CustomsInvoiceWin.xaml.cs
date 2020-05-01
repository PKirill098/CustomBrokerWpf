using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf.WindowsAccount
{
    public partial class CustomsInvoiceWin : Window
    {
        public CustomsInvoiceWin()
        {
            InitializeComponent();
            mydischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { MainDataGrid });
        }

        private DataModelClassLibrary.BindingDischarger mydischanger;

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                (App.Current.MainWindow as AccountMainWin).ListChildWindow.Remove(this);
                App.Current.MainWindow.Activate();
            }
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CustomsInvoiceViewCommand cmd = e.NewValue as CustomsInvoiceViewCommand;
            if (cmd != null)
            {
                cmd.EndEdit = mydischanger.EndEdit;
                cmd.CancelEdit = mydischanger.CancelEdit;
            }
        }

        private void BindingUpdate(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                System.Windows.Data.BindingExpression be;
                be = (sender as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                if (be != null)
                {
                    if (be.IsDirty) be.UpdateSource();
                }
            }
        }

        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
