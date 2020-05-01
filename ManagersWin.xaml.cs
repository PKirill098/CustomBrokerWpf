using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
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
    /// Interaction logic for ManagersWin.xaml
    /// </summary>
    public partial class ManagersWin : Window
    {
        public ManagersWin()
        {
            InitializeComponent();
            mydischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { MainDataGrid });
        }

        private DataModelClassLibrary.BindingDischarger mydischanger;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ManagerViewCommand cmd = new ManagerViewCommand();
            cmd.EndEdit = mydischanger.EndEdit;
            cmd.CancelEdit = mydischanger.CancelEdit;
            this.DataContext = cmd;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ManagerViewCommand cmd = this.DataContext as ManagerViewCommand;
            bool isdirty = !mydischanger.EndEdit();
            if (!isdirty)
                foreach (ManagerVM item in cmd.Items.SourceCollection)
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
                    //else
                    //    cmd.Reject.Execute(null);
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
                //else
                //{
                //    cmd.Reject.Execute(null);
                //}
            }
            if (!e.Cancel)
            {
                if (!e.Cancel) (App.Current.MainWindow as DataModelClassLibrary.Interfaces.IMainWindow).ListChildWindow.Remove(this);
                App.Current.MainWindow.Activate();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
