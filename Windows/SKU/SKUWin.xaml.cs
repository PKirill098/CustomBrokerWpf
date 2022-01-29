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
using lib = KirillPolyanskiy.DataModelClassLibrary;


namespace KirillPolyanskiy.CustomBrokerWpf.Windows.SKU
{
    /// <summary>
    /// Логика взаимодействия для SKUWin.xaml
    /// </summary>
    public partial class SKUWin : Window, lib.Interfaces.IMainWindow
    {
        private int mychildwindowscount;
        private List<Window> mychildwindows = new List<Window>();
        public List<Window> ListChildWindow
        { get { return mychildwindows; } }
        public SKUWin()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WarehouseRU_Loaded();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = WarehouseRU_Closing();
            int i = 0, c1;
            while (i < mychildwindows.Count)
            {
                c1 = mychildwindows.Count;
                mychildwindows[i].Close();
                i = i + 1 - c1 + mychildwindows.Count;
            }
            if (mychildwindows.Count > 0)
            {
                if (mychildwindows.Count != mychildwindowscount)
                {
                    e.Cancel = true;
                    mychildwindowscount = mychildwindows.Count;
                }
            }
            else
            {
                myskucmd.Filter.Dispose();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private WarehouseRUViewCommader myskucmd;
        private void WarehouseRU_Loaded()
        {
            myskucmd = new Classes.Domain.WarehouseRUViewCommader();
            myskucmd.IsReadOnly = false;
            this.DataContext = myskucmd;
        }
        private bool WarehouseRU_Closing()
        {
            bool cancel = false;
            myskucmd.Save.Execute(null);
            if (!myskucmd.LastSaveResult)
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    cancel = true;
                    this.Activate();
                }
            }
            return cancel;
        }

    }
}
