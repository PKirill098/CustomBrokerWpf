using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Windows.Specification
{
    /// <summary>
    /// Interaction logic for ColorWin.xaml
    /// </summary>
    public partial class ColorWin : Window
    {
        private Classes.Specification.ColorViewCommand mycmd;
        private lib.BindingDischarger mybinddisp;
        public ColorWin()
        {
            InitializeComponent();
            mycmd = new Classes.Specification.ColorViewCommand();
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
            if (mybinddisp.EndEdit())
            {
                bool isdirty = false;
                foreach (Classes.Specification.ColorVM item in mycmd.Items.SourceCollection) isdirty = isdirty | item.DomainObject.IsDirty;
                if (isdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!mycmd.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения в ДС не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                mycmd.Reject.Execute(null);
                        }
                    }
                    else
                        mycmd.Reject.Execute(null);
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
                    mycmd.Reject.Execute(null);
                }
            }
            if (!e.Cancel)
            {
                mycmd.Dispose();
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                (App.Current.MainWindow as MainWindow).Activate();
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BrandFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.BrandFilter != null && !mycmd.BrandFilter.FilterOn) mycmd.BrandFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("BrandFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void ProducerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.ProducerFilter != null && !mycmd.ProducerFilter.FilterOn) mycmd.ProducerFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("ProducerFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
    }
}
