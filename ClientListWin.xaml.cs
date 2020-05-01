using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class ClientListWin : Window
    {
        private lib.BindingDischarger mybinddisp;
        private CustomerViewCommand mycmd;
        public ClientListWin()
        {
            InitializeComponent();
            mycmd = new CustomerViewCommand();
            mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.MainDataGrid });
            mycmd.EndEdit = mybinddisp.EndEdit;
            mycmd.CancelEdit = mybinddisp.CancelEdit;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = mycmd;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            }
        }
        private void CustomerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("CustomerFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
