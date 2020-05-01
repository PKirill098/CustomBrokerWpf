using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf.WindowsAccount
{
    public partial class CustomsInvoicePayWin : Window
    {
        public CustomsInvoicePayWin()
        {
            InitializeComponent();
            mydischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { MainDataGrid });
        }

        private DataModelClassLibrary.BindingDischarger mydischanger;

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CustomsInvoicePayViewCommand cmd = e.NewValue as CustomsInvoicePayViewCommand;
            if (cmd != null)
            {
                cmd.EndEdit = mydischanger.EndEdit;
                cmd.CancelEdit = mydischanger.CancelEdit;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CustomsInvoicePayViewCommand cmd = this.DataContext as CustomsInvoicePayViewCommand;
            bool isdirty = !mydischanger.EndEdit();
            if (!isdirty)
                foreach (CustomsInvoicePayVM item in cmd.Items)
                    if (item.IsDirty)
                    { isdirty = true; break; }
            if (!isdirty)
            {
                if (!cmd.SaveDataChanges())
                {
                    this.Activate();
                    if (MessageBox.Show("\nИзменения не сохранены. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                //if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                //App.Current.MainWindow.Activate();
                this.Owner.Activate();
            }
        }

    }
}
