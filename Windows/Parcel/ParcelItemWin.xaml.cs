using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf.Windows.Parcel
{
    public partial class ParcelItemWin : Window
    {
        public ParcelItemWin()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Classes.Domain.ParcelCommander mycmd = this.DataContext as Classes.Domain.ParcelCommander;
            mycmd.Save.Execute(null);
            if (!mycmd.LastSaveResult)
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
            }
        }
    }
}
