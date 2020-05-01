using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account;
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
    public partial class RequestPrepayWin : Window
    {
        public RequestPrepayWin()
        {
            InitializeComponent();
            mydischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { MainDataGrid });
        }

        private DataModelClassLibrary.BindingDischarger mydischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mydischanger; } }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PrepayCustomerRequestCustomerCommander cmd = this.DataContext as PrepayCustomerRequestCustomerCommander;
            bool isdirty = !mydischanger.EndEdit();
            if (!isdirty)
                foreach (PrepayCustomerRequestVM item in cmd.Items)
                    if (item.IsDirty)
                    { isdirty = true; break; }
            if (!isdirty)
            {
                if (!cmd.SaveDataChanges())
                {
                    this.Activate();
                    if (MessageBox.Show("\nИзменения в ДС не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        e.Cancel = true;
                    }
                    else
                        cmd.Reject.Execute(null);
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                {
                    cmd.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                //if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                //App.Current.MainWindow.Activate();
                this.Owner.Activate();
            }
        }
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PrepayCustomerRequestCustomerCommander cmd = e.NewValue as PrepayCustomerRequestCustomerCommander;
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
    }
}
